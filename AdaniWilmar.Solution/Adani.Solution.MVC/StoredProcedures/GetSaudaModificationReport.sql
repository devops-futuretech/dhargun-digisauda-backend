CREATE PROCEDURE [dbo].[GetSaudaModificationReport]
(	
	@LoginUserId bigint,
	@RoleId bigint,
	@FromDate DateTime,
	@ToDate DateTime,
	@VerticalId BigInt,
	@SalesOrganizationId bigint,
	@DistributionChannelId bigint,
	@StateIds Nvarchar(Max),
	@StatusIds Nvarchar(Max)
)
AS 
BEGIN
CREATE TABLE #DealerIdsTemp(DealerId BIGINT)   
CREATE TABLE #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)

-- Get divisions for the user
if(@RoleId = 1) -- Admin
begin
	insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
	Select SalesOrganizationId,DistributionChannelId,Id as DivisionId from Divisions 
end
else
begin
	insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
	select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@LoginUserId
end

-- Get dealer IDs based on role
IF(@RoleId = 12) -- NH (National Trader)
BEGIN  
	INSERT INTO #DealerIdsTemp(DealerId)  
	Select DISTINCT ucm.CustomerId as DealerId  
	From UserReportingToMappings zh  
	INNER JOIN UserReportingToMappings bdo ON zh.UserId = bdo.ReportingToUserId  
	INNER JOIN UserCustomerMappings ucm ON ucm.UserId = bdo.UserId  
	Where zh.ReportingToUserId = @LoginUserId   
END  
ELSE IF(@RoleId = 9) -- ZH (Zonal Trader)
BEGIN  
	INSERT INTO #DealerIdsTemp(DealerId)  
	Select DISTINCT cus.Id as DealerId 
	From UserReportingToMappings bdo  
	INNER JOIN UserCustomerMappings ucm ON ucm.UserId = bdo.UserId 
	INNER JOIN Users cus ON ucm.CustomerId = cus.Id  
	Where bdo.ReportingToUserId = @LoginUserId 
END  
ELSE IF(@RoleId = 7) -- BDO (State Trader)
BEGIN  
	INSERT INTO #DealerIdsTemp(DealerId)  
	Select DISTINCT cus.Id as DealerId   
	From UserCustomerMappings ucm   
	JOIN Users cus ON ucm.CustomerId = cus.Id  
	Where ucm.UserId = @LoginUserId
END
ELSE -- Admin  
BEGIN  
	INSERT INTO #DealerIdsTemp(DealerId)  
	Select Distinct u.Id as DealerId From Users u   
	Join UserRoles ur on u.Id = ur.UserId 
	Where ur.RoleId = 5 -- Dealer role
END

-- Parse StateIds and StatusIds
Declare @Temp Table(Id Int, Data Nvarchar(Max))
IF @StateIds IS NOT NULL AND @StateIds != '' AND @StateIds != '0'
BEGIN
	Insert Into @Temp
	select Id,Data from dbo.Split(@StateIds,',') 
END

Declare @StatusTemp Table(Id Int, Data Nvarchar(Max))
IF @StatusIds IS NOT NULL AND @StatusIds != '' AND @StatusIds != '0' AND @StatusIds != '-1'
BEGIN
	Insert Into @StatusTemp
	select Id,Data from dbo.Split(@StatusIds,',') 
END

-- Main query to get Sauda Modification Report data
select 
	sm.SaudaNumber as SaudaNumber,
	sm.Id as SaudaModificationNumber,
	s.Id as SaudaBookedNumber,
	ISNULL(sm.ModifiedDate, sm.CreatedDate) as ModificationDate,
	dealer.Name as DealerName,
	z.Name as Zone,
	state.StateName as State,
	dist.DistrictName as District,
	city.CityName as City,
	ot.Name as OilTypeName,
	(CASE 
		WHEN sml.OilPackGroupTypeId = 0 THEN 'Unknown'
		WHEN sml.OilPackGroupTypeId = 1 THEN 'BP'
		WHEN sml.OilPackGroupTypeId = 2 THEN 'CP'
		ELSE 'Unknown'
	END) as OilPackGroupTypeName,
	sku.SkuName as MaterialName,
	sku.SkuCode as MaterialCode,
	smi.QuantityInCase as QuantityInCase,
	smi.SaudaQuantity as QuantityInMT,
	smi.Price as Price,
	smi.Discount as Discount,
	status.Name as Status,
	createdBy.Name as CreatedBy
from SaudaModifications sm with(NOLOCK)
join Saudas s with(NOLOCK) on sm.SaudaNumber = s.SaudaNumber
join SaudaModificationLines sml with(NOLOCK) on sm.Id = sml.SaudaModificationId
join SaudaModificationItems smi with(NOLOCK) on sml.Id = smi.SaudaModificationLineId
join Skus sku with(NOLOCK) on smi.skuId = sku.Id
join OilTypes ot with(NOLOCK) on sml.OilTypeId = ot.Id
join Users dealer with(NOLOCK) on s.UserId = dealer.Id
join Users createdBy with(NOLOCK) on sm.CreatedBy = createdBy.Id
left join Zones z with(NOLOCK) on dealer.ZoneId = z.Id
left join States state with(NOLOCK) on dealer.StateId = state.Id
left join Districts dist with(NOLOCK) on dealer.DistrictId = dist.Id
left join Cities city with(NOLOCK) on dealer.CityId = city.Id
left join Status status with(NOLOCK) on sm.StatusId = status.Id
join #UserDivision ud on s.SalesOrganizationId=ud.SalesOrganizationId 
	and s.DistributionChannelId=ud.DistributionChannelId 
	and s.DivisionId=ud.DivisionId
where 
	-- Date filter on SaudaModification.CreatedDate
	CAST(sm.CreatedDate as Date) >= CAST(@FromDate as Date) 
	and CAST(sm.CreatedDate as Date) <= CAST(@ToDate as Date)
	-- Dealer filter
	and s.UserId in (Select DealerId from #DealerIdsTemp)
	-- State filter
	and 1 = CASE 
		WHEN @StateIds IS NULL OR @StateIds = '' OR @StateIds = '0' THEN 1 
		ELSE (CASE WHEN dealer.StateId IN (SELECT Data from @Temp) THEN 1 ELSE 0 END) 
	END
	-- Status filter
	and 1 = CASE 
		WHEN @StatusIds IS NULL OR @StatusIds = '' OR @StatusIds = '0' OR '-1' IN (SELECT Data from @StatusTemp) THEN 1 
		ELSE (CASE WHEN sm.StatusId IN (SELECT Data from @StatusTemp) THEN 1 ELSE 0 END) 
	END
	-- Sales Organization filter
	and ((@SalesOrganizationId > 0 AND s.SalesOrganizationId = @SalesOrganizationId) OR @SalesOrganizationId = 0)
	-- Distribution Channel filter
	and ((@DistributionChannelId > 0 AND s.DistributionChannelId = @DistributionChannelId) OR @DistributionChannelId = 0)
	-- Vertical (Division) filter
	and ((@VerticalId > 0 AND s.DivisionId = @VerticalId) OR @VerticalId = 0)

DROP TABLE #DealerIdsTemp  
DROP TABLE #UserDivision  

END
GO

