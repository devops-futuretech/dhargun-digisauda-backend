IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SP_GetRaSaudaOrderReport')
    BEGIN
        DROP  Procedure SP_GetRaSaudaOrderReport
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure [dbo].[SP_GetRaSaudaOrderReport]
(	
	@VerticalId nvarchar(MAX),
	@StatusIds nvarchar(MAX),
	@StateIds nvarchar(MAX),
	@FromDate datetime,
	@ToDate datetime
)
AS
BEGIN

		Select
		U.Id,
		BW.Name As BiddingWindow,
		cg.Name As CustomerGroup,	
		U.Name As DealerName,
		SBT.Name AS SaudaBookingType, 
		OT.Name AS OilTypeName,
		sku.SkuName As SkuName,
		sku.SkuCode As SkuCode,
		OPT.Name AS OilPackingType,
		--S.BiddingDate AS BiddingDate,
		CONVERT(varchar, S.BiddingDate,111) as BiddingDate,

		SO.QuotedPrice as BidPriceTotal,
		SO.BidQuantity AS BidQuantityInMT,

		SO.BidPrice AS BidPriceAfterDiscount,
		SO.BidQuantityCase AS BidQuanityInCase,

		(SO.QuotedPrice / SO.BidQuantityCase) AS BidRatePerCase,
		(SO.BidPrice / SO.BidQuantityCase) AS BidPriceAfterDiscountPerCase,
		
		SO.BaseRate,
		((SO.BidPrice / SO.BidQuantityCase) - SO.BaseRate) AS MarginPerCase,

		approvalStatus.Name AS Status,
		Plant.Name AS PlantName,
		Depot.Name AS DepotName,
		states.StateName AS StateName,
		FZ.Name AS FrieghtZone,
		FR.Name AS FrieghtRoute,
		U.Loadability AS LoadQuantity,
		TM.Name AS TransportMode,

		SO.CounterBidOffer,
		CASE 
			WHEN SO.CounterBidOffer > 0 THEN 'Approved' 
			ELSE ''
		END as CounterBidStatus,

		SO.SchemeDiscount,
		SO.VolumeDiscount,
		SO.SkuDiscount,

		CASE 
			WHEN SO.GPBenefitType = 1 THEN 'SAP'
			WHEN SO.GPBenefitType = 2 THEN 'NONSAP'
			ELSE ''
		END as GPBenefitType,

		CASE 
			WHEN SO.GPBenefitAppliedTypeId = 1 THEN 'User'
			WHEN SO.GPBenefitAppliedTypeId = 2 THEN 'Geography'
			ELSE ''
		END as GPBenefitAppliedType,
		bf.BenefitCategory as GPBenefitCategory,
		SO.GPBenefitDiscountOrDay,

		CASE 
			WHEN SO.SurpriseBenefitType = 1 THEN 'SAP'
			WHEN SO.SurpriseBenefitType = 2 THEN 'NONSAP'
			ELSE ''
		END as SurpriseBenefitType,

		CASE 
			WHEN SO.SurpriseBenefitAppliedTypeId = 1 THEN 'User'
			WHEN SO.SurpriseBenefitAppliedTypeId = 2 THEN 'Geography'
			ELSE ''
		END as SurpriseBenefitAppliedType,
		sbf.BenefitCategory as SurpriseBenefitBenefitCategory,
		SO.SurpriseBenefitDiscountOrDay,

		--SO.SurpriseNonSapDiscount,
		--SO.SurpriseBenefitDays,

		CONVERT(varchar, SO.ValidFromDate,111) as SaudaValidFrom,
		CONVERT(varchar, SO.ValidToDate,111) as SaudaValidTo,
		DATEDIFF(d,SO.ValidFromDate,SO.ValidToDate) as SaudaValidityDays,

		PR.MaterialCost AS MaterialCost,
		PR.PackingCost AS PackingCost,
		PR.PrimaryFrieght AS PrimaryFrieght,
		PR.SecondaryFrieght AS SecondaryFrieght,
		PR.PlantSecondaryFrieght AS PlantSecondaryFrieght,
		PR.DepotCost AS DepotCost,
		PR.DetentionCost AS DetentionCost,
		PR.HoneycombCost AS HoneycombCost,
		PR.SchemeCostRecovery AS SchemeCostRecovery,
		PR.RaMargin AS RaMargin,		
		PR.CustomerGroupMargin AS CustomerGroupMargin,
		PR.SumOfIngredientCost AS SumOfIngredientCost,

		PR.ExPlantSGST AS ExPlantSGST,
		PR.ExPlantCGST AS ExPlantCGST,
		PR.ForPlantSGST AS ForPlantSGST,
		PR.ForPlantCGST AS ForPlantCGST,
		PR.ExPlantIGST AS ExPlantIGST,
		PR.ForPlantIGST AS ForPlantIGST,
		PR.ExDepotSGST AS ExDepotSGST,
		PR.ExDepotCGST AS ExDepotCGST,
		PR.ForDepotSGST AS ForDepotSGST,
		PR.ForDepotCGST AS ForDepotCGST,
		PR.ExDepotIGST AS ExDepotIGST,
		PR.ForDepotIGST AS ForDepotIGST,

		PR.ExPlantPrice AS ExPlantPrice,
		PR.ForDepotPrice AS ForDepotPrice,
		PR.ForPlantPrice AS ForPlantPrice,
		PR.ExDepotPrice AS ExDepotPrice,

		PR.ExPlantGuaranteePrice AS ExPlantGuaranteePrice,
		PR.ForPlantGuaranteePrice AS ForPlantGuaranteePrice,
		PR.ExDepotGuaranteePrice AS ExDepotGuaranteePrice,
		PR.ForDepotGuaranteePrice AS ForDepotGuaranteePrice,

		StateTrader.Name as BdoName,
	    bdost.StateName as BdoState,
		bdoct.CityName as BdoCity,
		dest.StateName as DealerState,
		dect.Cityname as DealerCity

	FROM SaudaOrders SO  
	INNER JOIN Saudas S With(NoLock) On SO.SaudaId = S.Id
	INNER JOIN Users U With(NoLock) On S.UserId = U.Id	
	INNER JOIN OilTypes OT With(NoLock) On SO.OilTypeId = OT.Id
	INNER JOIN BiddingWindows BW With(NoLock) On SO.BiddingwindowId = BW.Id
	INNER JOIN [Status] approvalStatus With(NoLock) On SO.StatusId = approvalStatus.Id
	INNER JOIN SaudaBookingTypes SBT With(NoLock) On SO.SaudaBookingTypeId = SBT.Id
	INNER JOIN Skus sku With(NoLock) On SO.SkuId = sku.Id
	INNER JOIN PackTypes OPT With(NoLock) On sku.PackTypeId = OPT.Id
	INNER JOIN States states With(NoLock) On U.StateId = states.Id
	INNER JOIN FreightZones FZ With(NoLock) On U.FreightZoneId = FZ.Id --AND U.FreightZoneId IS NOT NULL
	INNER JOIN FreightRoutes FR With(NoLock) On U.FreightRouteId = FR.Id --AND U.FreightRouteId IS NOT NULL
	INNER JOIN TransportModes TM With(NoLock) On U.TransportModeId = TM.Id --AND U.TransportModeId IS NOT NULL
	INNER JOIN Pricings PR With(NoLock) On SO.PricingId = PR.Id
	INNER JOIN Depots Depot With(NoLock) On PR.DepotId = Depot.Id And Depot.IsPlant = 0
	INNER JOIN Depots Plant With(NoLock) On PR.PlantId = Plant.Id And Plant.IsPlant = 1
	INNER JOIN CustomerGroupDetails cgd With(NoLock) On cgd.CustomerId = S.UserId
	INNER JOIN CustomerGroups cg With(NoLock) On cgd.CustomerGroupId = cg.Id
	
	INNER JOIN UserCustomerMappings ucm With(NoLock) on s.UserId = ucm.CustomerId	
	INNER JOIN Users StateTrader With(NoLock) on StateTrader.Id = ucm.UserId   
	INNER JOIN UserRoles ur With(NoLock) on ur.UserId = ucm.UserId and ur.RoleId = 7	
	LEFT JOIN Benefits bf With(NoLock) on bf.Id = SO.GPBenefitOrCategoryId
	LEFT JOIN Benefits sbf With(NoLock) on sbf.Id = SO.SurpriseBenefitCategoryId

	LEFT JOIN States bdost With(NoLock) on bdost.Id = StateTrader.StateId   --StateTrader State
	LEFT JOIN Cities bdoct With(NoLock) on bdoct.Id = StateTrader.CityId   --StateTrader City

	LEFT JOIN States dest With(NoLock) on dest.Id = u.StateId   --Dealer State
	LEFT JOIN Cities dect With(NoLock) on dect.Id = u.CityId   --StateTrader City

	WHERE OT.VerticalId IN (select * from STRING_SPLIT(@VerticalId, ','))
	AND SO.StatusId IN (Select * From STRING_SPLIT(@StatusIds, ','))
	AND U.StateId IN (Select * From STRING_SPLIT(@StateIds, ','))
	AND Convert(varchar, S.BiddingDate, 111) >= Convert(varchar, @FromDate, 111)
	AND Convert(varchar, S.BiddingDate, 111) <= Convert(varchar, @ToDate, 111)	
	AND cg.IsActive = 1
							
End


--EXEC SP_GetRaSaudaOrderReport '1','1','40','2019/12/31','2019/12/31'