namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BaselineMigrationDelta : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                IF COL_LENGTH('dbo.DiscountGeographies', 'PackTypeId') IS NULL
                    ALTER TABLE dbo.DiscountGeographies ADD PackTypeId BIGINT NOT NULL DEFAULT 0;

                IF COL_LENGTH('dbo.DiscountGeographyImportStatus', 'OilType') IS NULL
                    ALTER TABLE dbo.DiscountGeographyImportStatus ADD OilType NVARCHAR(MAX) NULL;

                IF COL_LENGTH('dbo.DiscountGeographyImportStatus', 'PackGroup') IS NULL
                    ALTER TABLE dbo.DiscountGeographyImportStatus ADD PackGroup NVARCHAR(MAX) NULL;

                IF COL_LENGTH('dbo.DiscountGeographyImportStatus', 'IsActive') IS NULL
                    ALTER TABLE dbo.DiscountGeographyImportStatus ADD IsActive BIT NOT NULL DEFAULT 0;

                IF COL_LENGTH('dbo.DiscountGeographyImportStatus', 'PackType') IS NULL
                    ALTER TABLE dbo.DiscountGeographyImportStatus ADD PackType NVARCHAR(MAX) NULL;
            ");
        }
        
        public override void Down()
        {
            Sql(@"
                IF COL_LENGTH('dbo.DiscountGeographyImportStatus', 'PackType') IS NOT NULL
                    ALTER TABLE dbo.DiscountGeographyImportStatus DROP COLUMN PackType;

                IF COL_LENGTH('dbo.DiscountGeographyImportStatus', 'IsActive') IS NOT NULL
                    ALTER TABLE dbo.DiscountGeographyImportStatus DROP COLUMN IsActive;

                IF COL_LENGTH('dbo.DiscountGeographyImportStatus', 'PackGroup') IS NOT NULL
                    ALTER TABLE dbo.DiscountGeographyImportStatus DROP COLUMN PackGroup;

                IF COL_LENGTH('dbo.DiscountGeographyImportStatus', 'OilType') IS NOT NULL
                    ALTER TABLE dbo.DiscountGeographyImportStatus DROP COLUMN OilType;

                IF COL_LENGTH('dbo.DiscountGeographies', 'PackTypeId') IS NOT NULL
                    ALTER TABLE dbo.DiscountGeographies DROP COLUMN PackTypeId;
            ");
        }
    }
}
