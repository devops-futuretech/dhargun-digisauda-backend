ALTER PROCEDURE usp_DeactivateExistingSaudaConfigurations
    @SalesOrganizationId BIGINT,
    @DistributionChannelId BIGINT,
    @DivisionId BIGINT,
    @OilTypeId VARCHAR(MAX),
    @PackGroupId BIGINT,
    @CurrentConfigId BIGINT,
    @IsUpdate BIT,
    @ZoneIds dbo.IdList READONLY,
    @StateIds dbo.IdList READONLY
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        -- Split input @OilTypeId CSV into table variable
        DECLARE @OilTypeTable TABLE (Id BIGINT);
        INSERT INTO @OilTypeTable (Id)
        SELECT TRY_CAST(value AS BIGINT)
        FROM STRING_SPLIT(@OilTypeId, ',')
        WHERE TRY_CAST(value AS BIGINT) IS NOT NULL;

        WITH ExistingConfigs AS (
            SELECT scb.Id, szsm.ZoneId, szsm.StateId
            FROM SaudaConditionalBookingConfigurations scb
            INNER JOIN SaudaConditionalBookingZoneStateMappings szsm
                ON scb.Id = szsm.SaudaConditionalConfigurationId
            WHERE scb.Id != @CurrentConfigId
              AND scb.SalesOrganizationId = @SalesOrganizationId
              AND scb.DistributionChannelId = @DistributionChannelId
              AND scb.DivisionId = @DivisionId
              AND scb.PackGroupId = @PackGroupId
              AND scb.IsActive = 1
              AND EXISTS (
                  SELECT 1
                  FROM @OilTypeTable ot
                  WHERE ',' + scb.OilTypeId + ',' LIKE '%,' + CAST(ot.Id AS VARCHAR) + ',%'
              )
              AND (@IsUpdate = 0 OR scb.EndDate > GETDATE())
        ),
        GroupedConfigs AS (
            SELECT ec.Id
            FROM ExistingConfigs ec
            GROUP BY ec.Id
            HAVING 
                COUNT(DISTINCT ec.ZoneId) = (SELECT COUNT(*) FROM @ZoneIds)
                AND COUNT(DISTINCT ec.StateId) = (SELECT COUNT(*) FROM @StateIds)
                AND NOT EXISTS (
                    SELECT 1 FROM (
                        SELECT DISTINCT ZoneId FROM ExistingConfigs WHERE Id = ec.Id
                        EXCEPT
                        SELECT Id FROM @ZoneIds
                    ) AS z
                )
                AND NOT EXISTS (
                    SELECT 1 FROM (
                        SELECT DISTINCT StateId FROM ExistingConfigs WHERE Id = ec.Id
                        EXCEPT
                        SELECT Id FROM @StateIds
                    ) AS s
                )
        )

        UPDATE SaudaConditionalBookingConfigurations 
        SET IsActive = 0  
        WHERE Id IN (SELECT Id FROM GroupedConfigs);
        
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(MAX) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
