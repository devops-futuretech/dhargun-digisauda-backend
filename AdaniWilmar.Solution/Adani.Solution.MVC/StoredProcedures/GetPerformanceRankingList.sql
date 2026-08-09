Create PROCEDURE [dbo].[GetPerformanceRankingList]
(	
	@FromDate DateTime,
	@ToDate DateTime,
	@LoginUserId bigint,
	@RoleId bigint
)
AS 
BEGIN
DECLARE @monthDiff INT;  
DECLARE @counter  INT;  
declare @idColumn int;
DECLARE @UserId bigint;

Create Table #UserDivisionLogin(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
Create Table #UserIds(UserId bigint)
Create Table #FinalResult(UserId bigint,UserCode varchar(50),UserName varchar(max),UserTarget decimal(18,5),UserAchievment decimal(18,5),AchievmentPercentage decimal(10,3))
CREATE TABLE #MonthsAndYear(Id BIGINT,MonthId INT,YearId BIGINT)

--Get month and year between start and end date  
SET @counter = 0;  
SELECT @monthDiff = DATEDIFF(mm, @FromDate, @ToDate);  
  
WHILE @counter <= @monthDiff  
BEGIN  
    INSERT INTO #MonthsAndYear(Id,MonthId,YearId)   
    SELECT @counter + 1, Month(DATEADD(mm, @counter, @FromDate)),Year(DATEADD(mm, @counter, @ToDate));  
  
    SET @counter = @counter + 1;  
END 

--Get LoginUserCombination Begins
if(@RoleId = 1)
begin
	insert into #UserDivisionLogin(SalesOrganizationId,DistributionChannelId,DivisionId) Select SalesOrganizationId,DistributionChannelId,Id as DivisionId from Divisions 
end
else
begin
	insert into #UserDivisionLogin(SalesOrganizationId,DistributionChannelId,DivisionId) select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@LoginUserId
end
--Get LoginUserCombination Ends



--Get Current Role Users Begins


IF(@RoleId = 9) -- ZH  
BEGIN  
   Insert into #UserIds
select distinct u.Id from Users u
join UserRoles ur on u.Id=ur.UserId
join UserDivisionMappings ud on u.Id=ud.UserId
join #UserDivisionLogin udm on ud.SalesOrganizationId=udm.SalesOrganizationId
and ud.DistributionChannelId=udm.DistributionChannelId and ud.DivisionId=udm.DivisionId
where ur.RoleId=@RoleId and u.IsActive=1
END  
ELSE 
BEGIN  
    Insert into #UserIds
	select distinct u.Id from Users u
	join UserRoles ur on u.Id=ur.UserId
	join UserDivisionMappings ud on u.Id=ud.UserId
	join #UserDivisionLogin udm on ud.SalesOrganizationId=udm.SalesOrganizationId
	and ud.DistributionChannelId=udm.DistributionChannelId and ud.DivisionId=udm.DivisionId
	where ur.RoleId=@RoleId and u.IsActive=1
END
----Get Current Role Users Ends


DECLARE UserIds CURSOR FOR
SELECT UserId FROM #UserIds

OPEN UserIds

FETCH NEXT FROM UserIds INTO @UserId

WHILE @@FETCH_STATUS = 0
BEGIN
	DECLARE @Target bigint;
	DECLARE @Quantity bigint;
	select @Target=Sum(uct.Target) from UserCustomerSalesTargets uct
	where uct.MonthId in (select MonthId from #MonthsAndYear)
	and uct.Year in (select YearId from #MonthsAndYear)
	and uct.AssignedToId=@UserId

	Create Table #DealerIdsTemp(DealerId bigint)
	Create Table #BdoId(BdoId bigint)
	Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)

	if(@RoleId=9)
	Begin
	insert into #BdoId(BdoId) select UserId from UserReportingToMappings where ReportingToUserId=@UserId

	insert into #DealerIdsTemp(DealerId) select CustomerId from UserCustomerMappings where UserId in (select BdoId from #BdoId)
	End
	Else
	Begin
	insert into #DealerIdsTemp(DealerId) select CustomerId from UserCustomerMappings where UserId=@UserId
	End
	
	insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
	select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@LoginUserId

	

	select
	@Quantity=(Case when Sum(s.QuantityMT) is null then 0 else Sum(s.QuantityMT) end)
	from SalesRegisters s with(NOLOCK)
	join Skus sku with(NOLOCK) on s.MaterialCode=sku.SkuCode and s.SalesOrganizationId=sku.SalesOrganizationId
	and s.DistributionChannelId=sku.DistributionChannelId and s.DivisionId=sku.DivisionId
	join Users u with(NOLOCK) on s.CustomerCode=u.Code
	join #UserDivision ud on s.SalesOrganizationId=ud.SalesOrganizationId and s.DistributionChannelId=ud.DistributionChannelId
	and s.DivisionId=ud.DivisionId
	where 
	u.Id in (select DealerId from #DealerIdsTemp)
	and Cast(s.InvoiceDate as date) <= Cast(@ToDate as date)
	and Cast(s.InvoiceDate as date) >= Cast(@FromDate as date)

	insert into #FinalResult(UserId,UserCode,UserName,UserTarget,UserAchievment,AchievmentPercentage)
	select @UserId,u.Code,u.Name,@Target,@Quantity,
	(CASE
	WHEN @Target > 0
	THEN (@Quantity / @Target) * 100 
	ELSE 0
	END) AS AchievmentPercentage
	from Users u
	where u.Id=@UserId

drop table #BdoId
drop table #DealerIdsTemp
drop table #UserDivision

  --SELECT * FROM OtherTable WHERE UserId = @UserId

  FETCH NEXT FROM UserIds INTO @UserId
END

CLOSE UserIds
DEALLOCATE UserIds
select * from #FinalResult

drop table #FinalResult
drop table #MonthsAndYear
drop table #UserDivisionLogin
drop table #UserIds

END