-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[GetFinalPriceDataExportForAllRecords] 
	-- Add the parameters for the stored procedure here
	@Skip bigint,
	@Take bigint,
	@StateId bigint,
	@VerticalId bigint,
	@SaudaBookingTypeId bigint
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

select
sk.SkuName,
o.Name as OiltypeName,
sb.Name as SaudaBookingType,
v.Name as VerticalName,
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
p.LoadQuantity
,p.IngredientCostId
,p.SkuIngrediantPlantId
,p.ExPlantGuaranteePrice
,p.ForPlantGuaranteePrice
,p.ExDepotGuaranteePrice
,p.ForDepotGuaranteePrice
,p.ExRakeGuaranteePrice
,p.ForRakeGuaranteePrice
,p.CustomerGroupId
,p.GPjump
,p.ExPlantSGST
,p.ExPlantCGST
,p.ExPlantIGST
,p.ForPlantSGST
,p.ForPlantCGST
,p.ForPlantIGST
,p.ExDepotSGST
,p.ExDepotCGST
,p.ExDepotIGST
,p.ForDepotSGST
,p.ForDepotCGST
,p.ForDepotIGST
,p.GstId
,p.CustomerGroupMarginId
,p.CustomerGroupMargin
,p.ExPlantPriceWithoutGst
,p.ForPlantPriceWithoutGst
,p.ExDepotPriceWithoutGst
,p.ForDepotPriceWithoutGst
,p.ExPlantGst
,p.ForPlantGst
,p.ExDepotGst
,p.ForDepotGst
,p.PlantGSTPercentage
,p.DepotGSTPercentage
,p.AdditionalCostId
,p.AdditionalCost
,p.OilTransferCosForPlantId
,p.OilTransferCostForPlant
,p.OilTransferCosForDepotId
,p.OilTransferCostForDepot
from TodayPricings p With (NoLock)
join OilTypes o With (NoLock) on p.OilTypeId = o.Id
join Skus sk With (NoLock) on p.SkuId = sk.Id
join SaudaBookingTypes sb With (NoLock) on p.SaudaBookingTypeId = sb.Id
join PackGroups pt With (NoLock) on P.OilPackingTypeId = pt.Id
join TransportModes tm With (NoLock) on p.TransportModeId = tm.Id
join States s With (NoLock) on p.StateId = s.Id
join Depots pl With (NoLock) on p.PlantId = pl.Id and pl.IsPlant = 1
join Depots de With (NoLock) on p.DepotId = de.Id and de.IsPlant = 0
join FreightZones fz With (NoLock) on p.FrieghtZoneId = fz.Id
join FreightRoutes fr With (NoLock) on p.FrieghtRouteId = fr.Id
join Verticals v on sk.VerticalId = v.Id
--Where Convert(varchar, p.CreatedDate, 111) = Convert(varchar, GETDATE(), 111) and IsPublish = 1
Where p.StateId = @StateId 
and sk.VerticalId = @VerticalId
and p.SaudaBookingTypeId = @SaudaBookingTypeId
--And IsPublish = 1
--Where p.CreatedDate > GETDATE() - 1
ORDER BY p.Id Desc 
OFFSET @Skip ROWS
FETCH NEXT @Take ROWS ONLY
END