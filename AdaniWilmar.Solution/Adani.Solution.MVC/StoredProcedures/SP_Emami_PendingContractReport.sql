Alter Procedure SP_Emami_PendingContractReport
(
	@OilTypeIds Nvarchar(Max),
	@BDOIds Nvarchar(Max),
	@LoginUserId BigInt
)
As
Begin
	Declare @LoginUserRoleId Int 
	Declare @DealerList Table(Id Int)
	Declare @BDOList Table(Id Int)
	Declare @OilTypeIdList Table(Id Int)

	Declare @SaudaOrderLiftingRequestMappings Table(Id Int, LiftingQuantityCase decimal(18,4),LiftingQuantity decimal(18,4))

	Declare @Invoice Table(Id Int, QuantityInCase decimal(18,4),ActualBilledQuantity decimal(18,4))

	Select @LoginUserRoleId = Id From Roles Where Name = 'StateTrader'

	Insert Into @BDOList
	Select Data From dbo.Split(@BDOIds,',')

	Insert Into @OilTypeIdList
	Select Data From dbo.Split(@OilTypeIds,',')

	Insert Into @SaudaOrderLiftingRequestMappings 
	Select SaudaOrderId,LiftingQuantityCase,LiftingQuantity From SaudaOrderLiftingRequestMappings With(NoLock) Where StatusId != 14

	Insert Into @Invoice 
	Select SaudaOrderId, ID.QuantityInCase,ID.ActualBilledQuantity From Invoices I With(NoLock)
	Inner join InvoiceDetails ID With(NoLock) On I.Id = ID.InvoiceId Where SalesDocumentType = 'ZHCR' 

	If Exists (Select Top 1 1 From UserRoles With(NoLock) Where UserId = @LoginUserId And RoleId = @LoginUserRoleId)
	Begin
		Insert Into @DealerList
		Select CustomerId From UserCustomerMappings With(NoLock) Where UserId = @LoginUserId
	End
	Else
	Begin
		Insert Into @DealerList
		Select CustomerId From UserCustomerMappings With(NoLock) Where UserId In (Select Id From @BDOList)
	End

	Select 
		SO.Id,
		D.Name As PlantName, 
		ST.StateName As [State], 
		U.Code CustomerCode, 
		U.Name As CustomerName, 
		sku.SkuCode As MaterialCode,
		sku.SkuName MaterialDescription,
		OT.Name As OilType, 
		SO.BidQuantityCase - isnull(A.LiftingQuantityCase,0) + isnull(B.QuantityInCase,0) As PendingQtyCases,
		SO.BidQuantity - isnull(A.LiftingQuantity,0) + isnull(B.ActualBilledQuantity,0) As PendingQty_MT,
		Case When SO.BidQuantityCase > 0 And SO.BidPrice > 0 Then  SO.BidPrice / SO.BidQuantityCase Else 0 End As BasicRatePerCase,
		I.Name As IncoTerms,
		SO.Id As ContractNo,
		SO.SaudaNumber As SAPContractNo,
		S.BiddingDate As SaudaDate,
		SO.ValidFromDate As ContractValidFrom,
		SO.ValidToDate As ContractValidTo,
		Case When SO.BrokerId > 0  Then  (Select Name From Users Where Id = SO.BrokerId) Else '' End BrokerName,
		A.LiftingQuantityCase,A.LiftingQuantity,B.QuantityInCase,B.ActualBilledQuantity
	From Saudas S
	Inner Join SaudaOrders SO With(NoLock) On S.Id = SO.SaudaId
	Inner Join Status APS With(NoLock) On SO.StatusId = APS.Id
	Inner Join Depots D With(NoLock) On SO.PlantId = D.Id
	Inner Join FreightRoutes FR With(NoLock) On SO.DealerLocationId = FR.Id
	Inner Join Users U With(NoLock) On U.Id = S.UserId
	Inner Join States ST With(NoLock) On U.StateId = ST.Id
	Inner Join IncoTerms I With(NoLock) On I.ID = SO.Incoterms2
	Inner Join Skus sku With(NoLock) On sku.Id = SO.SkuId
	Inner Join OilTypes OT With(NoLock) On OT.Id = SO.OilTypeId
	Inner Join @DealerList DL On DL.Id = S.UserId
	Inner Join @OilTypeIdList OTL On OTL.Id = SO.OilTypeId 
	Left Join 
	(
		Select Id, Sum(LiftingQuantityCase) As LiftingQuantityCase,Sum(LiftingQuantity) As LiftingQuantity From @SaudaOrderLiftingRequestMappings 
		Group By Id
	) As A On A.Id = SO.Id
	Left Join 
	(
		Select Id, Sum(QuantityInCase) As QuantityInCase,Sum(ActualBilledQuantity) As ActualBilledQuantity From @Invoice 
		Group By Id
	) As B On B.Id = SO.Id
End