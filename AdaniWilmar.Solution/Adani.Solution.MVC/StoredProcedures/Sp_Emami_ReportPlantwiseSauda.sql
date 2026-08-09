IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'Sp_Emami_ReportPlantwiseSauda')
    BEGIN
        DROP  Procedure Sp_Emami_ReportPlantwiseSauda
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure Sp_Emami_ReportPlantwiseSauda
(
	@FromDate DateTime,
	@ToDate DateTime,
	@StatusIds Nvarchar(Max),
	@StateIds Nvarchar(Max),
	@VerticalId BigInt
)
As
Begin
	
	Declare @Temp Table(Id Int, Data Nvarchar(Max))
	Insert Into @Temp
	select Id,Data from  dbo.Split(@StateIds,',') 

	Declare @TempStatus Table(Id Int, Data Nvarchar(Max))
	Insert Into @TempStatus
	select Id,Data from  dbo.Split(@StatusIds,',') 

	If(@StatusIds = '-1')
		Select @StatusIds = ''

	If(@StateIds = '-1')
		Select @StateIds = ''

	Select *, RealizationPMT - PurchasePMT As MarginPMT From 
	(Select Distinct
		SO.PlantId,
		D.Name As PlantName,
		D.StateId,
		ST.StateName,
		SO.SkuId,
		SKU.SkuName,
		Sum(SO.BidQuantityCase) As MaterialQtyInCase,
		Sum(SO.BidQuantity) As MaterialQtyInMT,
		--(Select Sum(PROO) From dbo.Fn_Tbl_Emami_GetPROO(SO.PlantId,D.StateId,SO.SkuId)) As PROO,
		--(Select Sum(RealizationPercase) From dbo.Fn_Tbl_Emami_GetPROO(SO.PlantId,D.StateId,SO.SkuId)) As RealizationPercase, 
		--(Select Sum(RealizationPercasePostBrokerage) From dbo.Fn_Tbl_Emami_GetPROO(SO.PlantId,D.StateId,SO.SkuId))  - 0 As RealizationPercasePostBrokerage, 
		--Case When SKU.UomId = 2 Then (SKU.Quantity * 1000) / OT.LitreConversion Else SKU.Quantity End SKUWiseWeight,
		--(Select Sum(RealizationPercasePostBrokerage) From dbo.Fn_Tbl_Emami_GetPROO(SO.PlantId,D.StateId,SO.SkuId)) / (Case When SKU.UomId = 2 Then (SKU.Quantity * 1000) / OT.LitreConversion Else SKU.Quantity End) * 1000 As RealizationPerMTPostBrokerage,
		--(Select Sum(RealizationPercasePostBrokerage) From dbo.Fn_Tbl_Emami_GetPROO(SO.PlantId,D.StateId,SO.SkuId)) / (Case When SKU.UomId = 2 Then (SKU.Quantity * 1000) / OT.LitreConversion Else SKU.Quantity End) * 1000 - 0 As FinalRealization,
		((Select Sum(RealizationPercasePostBrokerage) From dbo.Fn_Tbl_Emami_GetPROO(SO.PlantId,D.StateId,SO.SkuId)) / (Case When SKU.UomId = 2 Then (SKU.Quantity * 1000) / OT.LitreConversion Else SKU.Quantity End) * 1000 - 0) * Sum(SO.BidQuantity) As Realizatio
nPMT,
		isnull(Sum(((TTD.OilCost * TTD.Proportion) / 100 ) + TTD.ProcessCost),0) As PurchasePMT
	From Saudas S With(NoLock)
	Inner Join SaudaOrders SO With(NoLock) On S.Id = SO.SaudaId
	Inner Join Depots D With(NoLock) On SO.PlantId = D.Id And D.IsPlant = 1
	Inner Join States ST With(NoLock) On D.StateId = ST.Id
	Inner Join Skus SKU With(NoLock) On SKU.Id = SO.SkuId
	Inner Join OilTypes OT With(NoLock) On Sku.OilTypeId = OT.Id
	Inner Join Pricings P With(NoLock) On P.Id = SO.PricingId
	Left Join TradeTickets TT With(NoLock) On SO.TradeTicketNumber = TT.TradeTicketNumber
	Left Join TradeTicketDetails TTD With(NoLock) On TT.Id = TTD.TradeTicketId
	Where ((@StateIds = '' And 1 = 1 ) Or (@StateIds <> '' And D.StateId In (Select Data From @Temp)))
	And ((@StatusIds = '' And 1 = 1 ) Or (@StatusIds <> '' And SO.StatusId In (Select Data From @TempStatus)))
	And ((@VerticalId = 0 And 1 = 1 ) Or (@VerticalId <> 0 And OT.VerticalId = @VerticalId))  
	And SO.CreatedDate >= @FromDate And SO.CreatedDate <= @ToDate
	Group By
	SO.PlantId,D.Name,D.StateId,ST.StateName,SO.SkuId,SKU.SkuName,SKU.Quantity,SKU.UomId,OT.LitreConversion
	) As A
End