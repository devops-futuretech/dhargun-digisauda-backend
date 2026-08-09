CREATE PROCEDURE dbo.BackupAndCleanupPricings
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

		 SET IDENTITY_INSERT dbo.TodayPricingBackups ON;


        /* =========================================================
           1. Identify latest pricing rows per business grouping
           ========================================================= */
        ;WITH LatestPricings AS
        (
            SELECT
                tp.*,
                ROW_NUMBER() OVER
                (
                    PARTITION BY
                        tp.SkuId,
                        tp.PlantId,
                        tp.SalesOrganizationId,
                        tp.DistributionChannelId,
                        tp.DivisionId,
                        tp.OilTypeId,
                        tp.OilPackingTypeId
                    ORDER BY
                        tp.CreatedDate DESC
                ) AS RN
            FROM dbo.TodayPricings tp
        )

        /* =========================================================
           2. Backup ONLY latest pricing rows
           ========================================================= */

        INSERT INTO dbo.TodayPricingBackups
        (
            Id,
            SAPPricingCode,
            SkuId,
            SkuCode,
            OilTypeId,
            OilPackingTypeId,
            PlantId,
            PlantCode,
            DepotCode,
            Price,
            SalesOrganization,
            SalesOrganizationId,
            DistributionChannel,
            DistributionChannelId,
            Division,
            DivisionId,
            ValidFrom,
            ValidTo,
            PricingReferneceId,
            PerUnit,
            CreatedBy,
            CreatedDate,
            ModifiedBy,
            ModifiedDate
        )
        SELECT
            lp.Id,
            lp.SAPPricingCode,
            lp.SkuId,
            lp.SkuCode,
            lp.OilTypeId,
            lp.OilPackingTypeId,
            lp.PlantId,
            lp.PlantCode,
            lp.DepotCode,
            lp.Price,
            lp.SalesOrganization,
            lp.SalesOrganizationId,
            lp.DistributionChannel,
            lp.DistributionChannelId,
            lp.Division,
            lp.DivisionId,
            lp.ValidFrom,
            lp.ValidTo,
            lp.PricingReferneceId,
            lp.PerUnit,
            lp.CreatedBy,
            lp.CreatedDate,
            lp.ModifiedBy,
            lp.ModifiedDate
        FROM LatestPricings lp
        WHERE lp.RN = 1
          AND NOT EXISTS
          (
              SELECT 1
              FROM dbo.TodayPricingBackups tb
              WHERE tb.Id = lp.Id
          );

        SET IDENTITY_INSERT dbo.TodayPricingBackups OFF;

        /* =========================================================
           3. Cleanup backups older than 45 days
           ========================================================= */
        DELETE FROM dbo.TodayPricingBackups
        WHERE CreatedDate < DATEADD(DAY, -45, CAST(GETDATE() AS DATE));

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
		IF (IDENT_CURRENT('dbo.TodayPricingBackups') IS NOT NULL)
            SET IDENTITY_INSERT dbo.TodayPricingBackups OFF;
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END