CREATE PROCEDURE [dbo].[GetNewSaudaReport]
(
	@VerticalId bigint,
	@FromDate datetime,
	@ToDate datetime,
	@LoginUserId BigInt,
	@RoleId BigInt
)
AS
BEGIN 

Declare @DealerList Table(DealerId BigInt)

-- StateTrader
	if(@RoleId = 7)
	Begin
			Insert Into @DealerList
			select CustomerId from UserCustomerMappings where UserId  = @LoginUserId
	End
	Else if(@RoleId = 9)--ZonalTrader
	Begin
		 Insert Into @DealerList 
		 select CustomerId from UserCustomerMappings where UserId  in (
		 Select Id From Users where OrganizationReportingToId = @LoginUserId)
	End
	Else if(@RoleId = 12)--NationalTrader
	Begin
		 Insert Into @DealerList 
		 select CustomerId from UserCustomerMappings where UserId  in (
		 Select Id From Users where OrganizationReportingToId in (Select Id From Users where OrganizationReportingToId =  @LoginUserId))
	End
	Else
	Begin
		Insert Into @DealerList 
		select UserId from UserRoles where RoleId = 5
	End

 Select sku.SkuName,
 sku.SkuCode,
 o.Name as OilTypeName,
 p.Name as Plant,
 so.SaudaNumber,
 so.SaudaId as BookedNumber,
 so.BidQuantity as QuantityInMT,
 so.BidQuantityCase as QuantityInCase,
 s.BiddingDate,
 so.BidPricePerCase as SaudaBidPrice,
 inco.Name as Incoterms,
 u.Name as DealerName,
 u.Code as DealerCode,
 fr.Name as FreightRoute,
 bkt.Name as BookingType,
 approvalStatus.Name as Status,
 state.StateName as State ,
 createdby.Name as CreatedBy,
 StateTrader.Name as BdoName,
 StateTrader.Code as BdoCode
From Saudas as s
inner join SaudaOrders as so With(NoLock) on s.Id = so.SaudaId
Inner Join @DealerList DealerList On DealerList.DealerId = s.UserId 
inner join Skus as sku With(NoLock) on so.SkuId = sku.Id
inner join OilTypes as o With(NoLock) on so.OilTypeId = o.Id
inner join Depots as p With(NoLock) on so.PlantId = p.Id
inner join IncoTerms as inco With(NoLock) on so.Incoterms2 = inco.Id
inner join Users as u With(NoLock) On s.UserId = u.Id	
inner join FreightRoutes as fr With(NoLock) on u.FreightRouteId = fr.Id
inner join SaudaBookingTypes as bkt With(NoLock) on so.SaudaBookingTypeId = bkt.Id
inner join [Status] approvalStatus With(NoLock) on so.StatusId = approvalStatus.Id
inner join States as state With(NoLock) on u.StateId = state.Id
inner join users as createdby With(NoLock) on so.CreatedBy = createdby.Id
inner join UserCustomerMappings ucm With(NoLock) on s.UserId = ucm.CustomerId	
inner join Users StateTrader With(NoLock) on StateTrader.Id = ucm.UserId   	
inner join UserRoles ur With(NoLock) on ur.UserId = ucm.UserId and ur.RoleId = 7
where 
(Convert(varchar, S.BiddingDate, 111) >= Convert(varchar, @FromDate, 111)
	AND Convert(varchar, S.BiddingDate, 111) <= Convert(varchar, @ToDate, 111)) AND (sku.VerticalId = @VerticalId or @VerticalId = 0)


END