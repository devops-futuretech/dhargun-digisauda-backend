IF EXISTS (SELECT * FROM sysobjects WHERE type IN (N'FN', N'IF', N'TF') AND name = 'Fn_Tbl_Emami_GetPROO')
    BEGIN
        DROP  Function Fn_Tbl_Emami_GetPROO
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Fn_Tbl_Emami_GetPROO] 
(  
   @PlantId BigInt,  
   @StateId BigInt,  
   @SkuId BigInt
)  
returns Table  
as  
 
	return Select 
		SO.PlantId,
		D.StateId,
		SO.SkuId,
		Sum(Case 
			When So.Proo > 0 Then So.Proo
			Else 
			(P.MaterialCost 
			+ P.PackingCost 
			+ Case When SO.Incoterms2 in (2,4) Then 0 Else P.HoneycombCost End
			+ Case When S.SaudaBookingTypeId = 1 Then P.Margin + P.CushionMargin Else P.RaMargin End
			+ P.SchemeCostRecovery
			+ Case When SO.SpecialRateRequestId > 0 and SO.BidQuantityCase > 0
				Then -(SO.QuotedPrice - SO.BidPrice) / SO.BidQuantityCase 
				Else 
			  Case When SO.DiscountTypeId = 2 and SO.BidQuantityCase > 0
				Then SO.DiscountAmount / SO.BidQuantityCase 
				Else 0 End End) ---Premium
			- Case 
				When SO.SpecialRateRequestId > 0 and SO.BidQuantityCase > 0
				Then (SO.QuotedPrice - SO.BidPrice) / SO.BidQuantityCase 
				Else 
			  Case 
				When SO.DiscountTypeId = 2 and SO.BidQuantityCase > 0
				Then SO.DiscountAmount / SO.BidQuantityCase 
				Else 0 
				End 
			End --- Discount
		End )As PROO,
		Sum(Case 
			When So.Proo > 0 Then So.Proo
			Else 
			(P.MaterialCost 
			+ Case 
				When S.SaudaBookingTypeId = 1 
				Then P.Margin + P.CushionMargin 
				Else P.RaMargin 
			  End
			+ P.SchemeCostRecovery
			+ Case 
				When SO.SpecialRateRequestId > 0 and SO.BidQuantityCase > 0
				Then -(SO.QuotedPrice - SO.BidPrice) / SO.BidQuantityCase 
				Else 
			  Case 
				When SO.DiscountTypeId = 2 and SO.BidQuantityCase > 0
				Then SO.DiscountAmount / SO.BidQuantityCase 
				Else 0 
			  End 
			End) ---Premium
			- Case 
				When SO.SpecialRateRequestId > 0 and SO.BidQuantityCase > 0
				Then (SO.QuotedPrice - SO.BidPrice) / SO.BidQuantityCase 
				Else 
			  Case 
				When SO.DiscountTypeId = 2 and SO.BidQuantityCase > 0
				Then SO.DiscountAmount / SO.BidQuantityCase 
				Else 0 End 
			  End --- Discount
			End )As RealizationPercase,
		Sum(Case 
			When So.Proo > 0 Then So.Proo
			Else 
			(P.MaterialCost 
			+ Case 
				When S.SaudaBookingTypeId = 1 
				Then P.Margin + P.CushionMargin 
				Else P.RaMargin 
			  End
			+ P.SchemeCostRecovery
			+ Case 
				When SO.SpecialRateRequestId > 0 and SO.BidQuantityCase > 0
				Then -(SO.QuotedPrice - SO.BidPrice) / SO.BidQuantityCase 
				Else 
			  Case 
				When SO.DiscountTypeId = 2 and SO.BidQuantityCase > 0
				Then SO.DiscountAmount / SO.BidQuantityCase 
				Else 0 
			  End 
			End) ---Premium
			- Case 
				When SO.SpecialRateRequestId > 0 and SO.BidQuantityCase > 0
				Then (SO.QuotedPrice - SO.BidPrice) / SO.BidQuantityCase 
				Else 
			  Case 
				When SO.DiscountTypeId = 2 and SO.BidQuantityCase > 0
				Then SO.DiscountAmount / SO.BidQuantityCase 
				Else 0 
			  End 
			End --- Discount
		End ) - 0 As RealizationPercasePostBrokerage
	From Saudas S With(NoLock)
	Inner Join SaudaOrders SO With(NoLock) On S.Id = SO.SaudaId
	Inner Join Depots D With(NoLock) On SO.PlantId = D.Id And D.IsPlant = 1
	Inner Join States ST With(NoLock) On D.StateId = ST.Id
	Inner Join Skus SKU With(NoLock) On SKU.Id = SO.SkuId
	Inner Join Pricings P With(NoLock) On P.Id = SO.PricingId
	Inner Join OilTypes OT With(NoLock) On Sku.OilTypeId = OT.Id
	Where SO.PlantId = @PlantId And D.StateId = @StateId And SO.SkuId = @SkuId
	Group By
	--SO.PlantId,D.Name,D.StateId,ST.StateName,SO.SkuId,SKU.SkuName
	So.Proo,P.MaterialCost,P.PackingCost,P.HoneycombCost,SO.Incoterms2,S.SaudaBookingTypeId,P.Margin,P.CushionMargin,P.RaMargin,P.SchemeCostRecovery,SO.SpecialRateRequestId,SO.QuotedPrice,SO.BidPrice
	,SO.DiscountAmount,SO.BidQuantityCase,SO.DiscountTypeId,
	SO.PlantId,
		D.StateId,
		SO.SkuId 
