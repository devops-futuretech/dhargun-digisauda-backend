IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'SP_GetSurpriseBenefitAppliedCustomersWithSauda')
    BEGIN
        DROP  Procedure SP_GetSurpriseBenefitAppliedCustomersWithSauda
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SP_GetSurpriseBenefitAppliedCustomersWithSauda]
(	
	@SaudaOrderIds nvarchar(MAX)
)
AS
BEGIN
	Select
	 SO.Id AS SaudaOrderId,
	 S.Id AS SaudaId,
	 U.Id As CustomerId, 
	 OT.Id AS OilTypeId,
	 sku.Id AS SkuId,
	 U.CityId AS CityId,
	 U.StateId AS StateId,
	 SO.StatusId AS StatusId,
	 SO.SaudaBookingTypeId AS SaudaBookingTypeId,
	 U.FreightZoneId AS FreightZoneId,
	 U.FreightRouteId AS FreightRouteId,
	 U.TransportModeId AS TransportModeId,
	 SO.PricingId AS PricingId,
	 PR.PlantId AS PlantId,
	 PR.DepotId AS DepotId,
	 SO.BiddingwindowId AS BiddingwindowId,
     BW.Name As BiddingWindow,  
	 cg.Name As CustomerGroup,
	 U.Name As DealerName, 
	 U.Code As DealerCode, 
	 sku.SkuCode As SkuCode,
	 sku.SkuName As SkuName,
	 SO.BidPrice AS BidRate, 
	 SO.BidPricePerCase AS BidRatePerCase, 
	 SO.BidQuantityCase AS BidQuantityInCase, 
	 SO.BidQuantity AS BidQuantityInMT,
	 approvalStatus.Name AS Status, 
	 (SO.BidPricePerCase - SO.BaseRate) AS MarginPerCase, 
	 StateTrader.Name AS BDOName,
	 SO.SchemeDiscount AS SchemeDiscount,
	 SO.VolumeDiscount AS VolumeDiscount,
	 SO.SkuDiscount AS SkuDiscount,
	 BT.Name AS GPBenefitType,
	 GPBenefit.BenefitDiscountOrDays AS GPBenefitDiscountOrDays,
	 OT.Name AS OilTypeName,
	 OPT.Name AS OilPackingType,
	 SBT.Name AS SaudaBookingType,
	 states.StateName AS StateName,
	 FZ.Name AS FrieghtZone,
	 FR.Name AS FrieghtRoute,
	 U.Loadability AS LoadQuantity,
	 TM.Name AS TransportMode,
	 Plant.Name AS PlantName,
	 Depot.Name AS DepotName,
	 S.BiddingDate AS BiddingDate,
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
	 PR.CushionMargin AS CushionMargin,
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
	 PR.ExRakePrice AS ExRakePrice,
	 PR.ForRakePrice AS ForRakePrice,
	 PR.ExPlantGuaranteePrice AS ExPlantGuaranteePrice,
	 PR.ForPlantGuaranteePrice AS ForPlantGuaranteePrice,
	 PR.ExDepotGuaranteePrice AS ExDepotGuaranteePrice,
	 PR.ForDepotGuaranteePrice AS ForDepotGuaranteePrice

	FROM SaudaOrders SO With(NoLock) 
	JOIN Saudas S With(NoLock) On SO.SaudaId = S.Id
	LEFT JOIN Users U With(NoLock) On S.UserId = U.Id
	LEFT JOIN UserCustomerMappings UCM With(NoLock) On UCM.CustomerId = S.UserId
	LEFT JOIN Users StateTrader With(NoLock) On StateTrader.Id = UCM.UserId
	LEFT JOIN UserRoles UR With(NoLock) On UCM.UserId = UR.UserId and UR.RoleId = 7
	LEFT JOIN OilTypes OT With(NoLock) On SO.OilTypeId = OT.Id
	LEFT JOIN BiddingWindows BW With(NoLock) On SO.BiddingwindowId = BW.Id
	LEFT JOIN [Status] approvalStatus With(NoLock) On SO.StatusId = approvalStatus.Id
	LEFT JOIN SaudaBookingTypes SBT With(NoLock) On SO.SaudaBookingTypeId = SBT.Id
	LEFT JOIN Skus sku With(NoLock) On SO.SkuId = sku.Id
	LEFT JOIN PackTypes OPT With(NoLock) On sku.PackTypeId = OPT.Id
	LEFT JOIN States states With(NoLock) On U.StateId = states.Id
	LEFT JOIN FreightZones FZ With(NoLock) On U.FreightZoneId = FZ.Id --AND U.FreightZoneId IS NOT NULL
	LEFT JOIN FreightRoutes FR With(NoLock) On U.FreightRouteId = FR.Id --AND U.FreightRouteId IS NOT NULL
	LEFT JOIN TransportModes TM With(NoLock) On U.TransportModeId = TM.Id --AND U.TransportModeId IS NOT NULL
	LEFT JOIN Pricings PR With(NoLock) On SO.PricingId = PR.Id
	LEFT JOIN Depots Depot With(NoLock) On PR.DepotId = Depot.Id And Depot.IsPlant = 0
	LEFT JOIN Depots Plant With(NoLock) On PR.PlantId = Plant.Id And Plant.IsPlant = 1
	LEFT JOIN CustomerGroupDetails cgd With(NoLock) On cgd.CustomerId = S.UserId
	LEFT JOIN CustomerGroups cg With(NoLock) On cgd.CustomerGroupId = cg.Id
	LEFT JOIN SurpriseAndGPBenefitHistories GPBenefit With(NoLock) On GPBenefit.SaudaOrderId = SO.Id AND IsGPBenefit=1
	LEFT JOIN BenefitTypes BT With(NoLock) On GPBenefit.BenefitTypeId = BT.Id 

	WHERE SO.Id IN (select * from STRING_SPLIT(@SaudaOrderIds, ','))
	AND SO.IsSurpriseBenefitApplied = 0	
	AND U.SaudaBookingTypeId = 2	
	AND cg.IsActive = 1
End