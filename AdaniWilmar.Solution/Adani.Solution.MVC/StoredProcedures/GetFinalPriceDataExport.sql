IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetFinalPriceDataExport')
    BEGIN
        DROP  Procedure GetFinalPriceDataExport
    END
 Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[GetFinalPriceDataExport]
(
	@PriceId BIGINT,
	@SearchDate datetime,
	@Skip bigint,
	@Take bigint
)
AS 
BEGIN

IF(Cast(@SearchDate as Date) = Cast(GetDate() as Date))
BEGIN
select
sk.SkuName,
o.Name as OiltypeName,
sb.Name as SaudaBookingType,
pt.Name as OilPackingType,
s.StateName,
tm.Name as TransportMode,
pl.Name as PlantName,
de.Name as DepotName,
fz.Name as FrieghtZone,
fr.Name as FrieghtRoute,
p.BiddingDate,
p.MaterialCost,
p.PackingCost,
p.PrimaryFrieght,
p.SecondaryFrieght,
p.PlantSecondaryFrieght,
p.DepotCost,
p.DetentionCost,
p.HoneycombCost,
p.Margin,
p.CushionMargin,
p.SchemeCostRecovery,
p.ProcessCost,
p.SumOfIngredientCost,
p.TpPrice,
p.RaMargin,
p.BaseRate,
p.XMargin,
p.FinalRate,  
p.ExPlantPrice,
p.ForDepotPrice,
p.ForPlantPrice,
p.ExDepotPrice,
p.ClearanceRate,
p.CounterBidOffer,
p.CounterBidLimit,
p.BpCpJumb,
p.ExRakePrice,
p.ForRakePrice,
p.LoadQuantity,
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
Where p.PublishId in (Select pgd.Id From PriceGenerates pg Join PriceGenerateDetails pgd on pg.Id = pgd.PriceGenerateId Where pg.Id = @PriceId)
ORDER BY p.Id Desc 
OFFSET @Skip ROWS
FETCH NEXT @Take ROWS ONLY
END
ELSE IF(Cast(@SearchDate as Date) < Cast(GetDate() as Date))
BEGIN
select
sk.SkuName,
o.Name as OiltypeName,
sb.Name as SaudaBookingType,
pt.Name as OilPackingType,
s.StateName,
tm.Name as TransportMode,
pl.Name as PlantName,
de.Name as DepotName,
fz.Name as FrieghtZone,
fr.Name as FrieghtRoute,
p.BiddingDate,
p.MaterialCost,
p.PackingCost,
p.PrimaryFrieght,
p.SecondaryFrieght,
p.PlantSecondaryFrieght,
p.DepotCost,
p.DetentionCost,
p.HoneycombCost,
p.Margin,
p.CushionMargin,
p.SchemeCostRecovery,
p.ProcessCost,
p.SumOfIngredientCost,
p.TpPrice,
p.RaMargin,
p.BaseRate,
p.XMargin,
p.FinalRate,  
p.ExPlantPrice,
p.ForDepotPrice,
p.ForPlantPrice,
p.ExDepotPrice,
p.ClearanceRate,
p.CounterBidOffer,
p.CounterBidLimit,
p.BpCpJumb,
p.ExRakePrice,
p.ForRakePrice,
p.LoadQuantity,
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
Where p.PublishId in (Select pgd.Id From PriceGenerates pg Join PriceGenerateDetails pgd on pg.Id = pgd.PriceGenerateId Where pg.Id = @PriceId)
ORDER BY p.Id Desc 
OFFSET @Skip ROWS
FETCH NEXT @Take ROWS ONLY
END

END;
