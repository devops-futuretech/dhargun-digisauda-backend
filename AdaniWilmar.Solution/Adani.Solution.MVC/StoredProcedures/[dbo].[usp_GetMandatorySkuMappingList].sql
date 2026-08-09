CREATE NONCLUSTERED INDEX IX_TodayPricing_SkuId_PlantId_ValidFrom_ValidTo
ON TodayPricings (SkuId, PlantId, ValidFrom, ValidTo);
GO

CREATE NONCLUSTERED INDEX IX_MandatorySkuMappings_EssentialId_MandatorySkuId
ON SaudaConditionalBookingMandatorySkuMappings (ConditionalBookingEssentialSkuMappingId, MandatorySkuId);
GO

CREATE PROCEDURE usp_GetMandatorySkuMappingList
    @EssentialSkuMappingId BIGINT,
    @PlantId BIGINT,
    @CurrentDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1 WITH TIES
        scm.MandatorySkuPercentage AS MandatoryBookingQuantityPercentage,
        scm.MandatorySkuCode,
        scm.MandatorySkuId,
        (s.SkuName + '-' + s.SkuCode + '-' + ISNULL(pg.Name, '')) AS MandatorySkuName,
        s.OilTypeId,
        tp.Price AS MandatorySkuPrice
    FROM SaudaConditionalBookingMandatorySkuMappings scm
    INNER JOIN Skus s ON scm.MandatorySkuId = s.Id
    INNER JOIN TodayPricings tp ON scm.MandatorySkuId = tp.SkuId
    LEFT JOIN PackGroups pg ON s.PackGroupId = pg.Id
    WHERE scm.ConditionalBookingEssentialSkuMappingId = @EssentialSkuMappingId
      AND tp.PlantId = @PlantId
      AND CAST(@CurrentDate AS DATE) BETWEEN CAST(tp.ValidFrom AS DATE) AND CAST(tp.ValidTo AS DATE)
      AND s.IsActive = 1
    ORDER BY ROW_NUMBER() OVER (PARTITION BY scm.MandatorySkuId ORDER BY scm.MandatorySkuId);
END
