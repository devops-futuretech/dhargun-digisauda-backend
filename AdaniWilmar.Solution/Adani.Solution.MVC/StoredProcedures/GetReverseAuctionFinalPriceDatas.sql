IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetReverseAuctionFinalPriceDatas')
    BEGIN
        DROP  Procedure GetReverseAuctionFinalPriceDatas
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetReverseAuctionFinalPriceDatas]
(
        @PublishId bigint,
        @CustomerGroupId bigint,
        @BiddingWindowId bigint,
		@SearchDate datetime,
     	@Skip bigint,
		@Take bigint
)
AS 
BEGIN

IF(Cast(@SearchDate as Date) = Cast(GETDATE() as Date))
BEGIN
select
--count(p.Id)
bw.Name as BiddingWindow,
cg.Name as CustomerGroup,
o.Name as OiltypeName,
pt.Name as OilPackingType,
sk.SkuName,
sb.Name as SaudaBookingType,
s.StateName,
fz.Name as FrieghtZone,
fr.Name as FrieghtRoute,
p.LoadQuantity,
tm.Name as TransportMode,
pl.Name as PlantName,
de.Name as DepotName,
--ISNULL(CONVERT(varchar, p.BiddingDate,111),'') as BiddingDate,
CASE ISDATE(CONVERT(varchar, p.BiddingDate,111)) 
	WHEN 1 THEN CONVERT(varchar, p.BiddingDate,111)
	ELSE ''
END as BiddingDate,
p.MaterialCost,
p.PackingCost,
p.PrimaryFrieght,
p.SecondaryFrieght,
p.PlantSecondaryFrieght,
p.DepotCost,
p.DetentionCost,
p.HoneycombCost,
p.SchemeCostRecovery,
p.RaMargin,
p.CushionMargin,
p.CustomerGroupMargin,
p.SumOfIngredientCost,
ExPlantSGST,ExPlantCGST,ForPlantSGST,ForPlantCGST,ExPlantIGST,ForPlantIGST,ExDepotSGST,ExDepotCGST,ForDepotSGST,ForDepotCGST,ExDepotIGST,ForDepotIGST,
p.ExPlantPrice,
p.ForDepotPrice,
p.ForPlantPrice,
p.ExDepotPrice,
p.ExPlantGuaranteePrice,
p.ForPlantGuaranteePrice,
p.ExDepotGuaranteePrice,
p.ForDepotGuaranteePrice,
p.GPJump as GuaranteePriceJump,
p.AdditionalCost,
p.OilTransferCostForPlant as OilTransferCost
from TodayPricings p 
join OilTypes o on p.OilTypeId = o.Id
join Skus sk on p.SkuId = sk.Id
join SaudaBookingTypes sb on p.SaudaBookingTypeId = sb.Id
join PackGroups pt on P.OilPackingTypeId = pt.Id
join TransportModes tm on p.TransportModeId = tm.Id
join States s on p.StateId = s.Id
join Depots pl on p.PlantId = pl.Id and pl.IsPlant = 1
join Depots de on p.DepotId = de.Id and de.IsPlant = 0
join FreightZones fz on p.FrieghtZoneId = fz.Id
join FreightRoutes fr on p.FrieghtRouteId = fr.Id
Join CustomerGroups cg on cg.Id = p.CustomerGroupId
Join BiddingWindows bw on bw.Id = p.BiddingWindowId
Where p.PublishId = @PublishId and p.CustomerGroupId = @CustomerGroupId and p.BiddingWindowId = @BiddingWindowId
ORDER BY p.Id Desc 
OFFSET @Skip ROWS
FETCH NEXT @Take ROWS ONLY
END
ELSE IF(Cast(@SearchDate as Date) < Cast(GETDATE() as Date))
BEGIN
select
--count(p.Id)
bw.Name as BiddingWindow,
cg.Name as CustomerGroup,
o.Name as OiltypeName,
pt.Name as OilPackingType,
sk.SkuName,
sb.Name as SaudaBookingType,
s.StateName,
fz.Name as FrieghtZone,
fr.Name as FrieghtRoute,
p.LoadQuantity,
tm.Name as TransportMode,
pl.Name as PlantName,
de.Name as DepotName,
--ISNULL(CONVERT(varchar, p.BiddingDate,111),'') as BiddingDate,
CASE ISDATE(CONVERT(varchar, p.BiddingDate,111)) 
	WHEN 1 THEN CONVERT(varchar, p.BiddingDate,111)
	ELSE ''
END as BiddingDate,
p.MaterialCost,
p.PackingCost,
p.PrimaryFrieght,
p.SecondaryFrieght,
p.PlantSecondaryFrieght,
p.DepotCost,
p.DetentionCost,
p.HoneycombCost,
p.SchemeCostRecovery,
p.RaMargin,
p.CushionMargin,
p.CustomerGroupMargin,
p.SumOfIngredientCost,
ExPlantSGST,ExPlantCGST,ForPlantSGST,ForPlantCGST,ExPlantIGST,ForPlantIGST,ExDepotSGST,ExDepotCGST,ForDepotSGST,ForDepotCGST,ExDepotIGST,ForDepotIGST,
p.ExPlantPrice,
p.ForDepotPrice,
p.ForPlantPrice,
p.ExDepotPrice,
p.ExPlantGuaranteePrice,
p.ForPlantGuaranteePrice,
p.ExDepotGuaranteePrice,
p.ForDepotGuaranteePrice,
p.ExPlantGuaranteePrice - p.ExPlantPrice as ExPlantGPJump,
p.ForPlantGuaranteePrice - p.ForPlantPrice as ForPlantGPJump,
p.ExDepotGuaranteePrice - p.ExDepotPrice as ExDepotGPJump,
p.ForDepotGuaranteePrice - p.ForDepotPrice as ForDepotGPJump,
p.ForDepotGuaranteePrice - p.ForDepotPrice as ForDepotGPJump,
p.ForDepotGuaranteePrice - p.ForDepotPrice as ForDepotGPJump,
p.AdditionalCost,
p.OilTransferCostForPlant as OilTransferCost
from PricingBackups p 
join OilTypes o on p.OilTypeId = o.Id
join Skus sk on p.SkuId = sk.Id
join SaudaBookingTypes sb on p.SaudaBookingTypeId = sb.Id
join PackGroups pt on P.OilPackingTypeId = pt.Id
join TransportModes tm on p.TransportModeId = tm.Id
join States s on p.StateId = s.Id
join Depots pl on p.PlantId = pl.Id and pl.IsPlant = 1
join Depots de on p.DepotId = de.Id and de.IsPlant = 0
join FreightZones fz on p.FrieghtZoneId = fz.Id
join FreightRoutes fr on p.FrieghtRouteId = fr.Id
Join CustomerGroups cg on cg.Id = p.CustomerGroupId
Join BiddingWindows bw on bw.Id = p.BiddingWindowId
Where p.PublishId = @PublishId and p.CustomerGroupId = @CustomerGroupId and p.BiddingWindowId = @BiddingWindowId
ORDER BY p.Id Desc 
OFFSET @Skip ROWS
FETCH NEXT @Take ROWS ONLY
END

END;

--EXEC [dbo].[GetReverseAuctionFinalPriceDatas] 11,6,12

--Select count(*) from Pricings

--EXEC [dbo].[GetReverseAuctionFinalPriceDatas] 11,6,12

--Select count(*) from Pricings

--EXEC [dbo].[GetReverseAuctionFinalPriceDatas] 11,6,12

--Select count(*) from Pricings

--EXEC [dbo].[GetReverseAuctionFinalPriceDatas] 11,6,12

--Select count(*) from Pricings