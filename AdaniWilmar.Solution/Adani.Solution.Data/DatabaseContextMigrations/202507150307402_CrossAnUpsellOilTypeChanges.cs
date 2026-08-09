namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CrossAnUpsellOilTypeChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SaudaConditionalBookingConfigurations", "PackGroupId", "dbo.PackGroups");
            DropIndex("dbo.SaudaConditionalBookingConfigurations", new[] { "PackGroupId" });
            AddColumn("dbo.SaudaConditionalBookingEssentialSkuMappings", "OilTypeId", c => c.String(nullable: false));
            AddColumn("dbo.SaudaConditionalBookingEssentialSkuMappings", "PackGroupId", c => c.Long(nullable: false));
            AddColumn("dbo.SaudaConditionalBookingMandatorySkuMappings", "OilTypeId", c => c.Long(nullable: false));
            AddColumn("dbo.SaudaConditionalBookingMandatorySkuMappings", "PackGroupId", c => c.Long(nullable: false));
            DropColumn("dbo.SaudaConditionalBookingConfigurations", "OilTypeId");
            DropColumn("dbo.SaudaConditionalBookingConfigurations", "PackGroupId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SaudaConditionalBookingConfigurations", "PackGroupId", c => c.Long(nullable: false));
            AddColumn("dbo.SaudaConditionalBookingConfigurations", "OilTypeId", c => c.String(nullable: false));
            AddColumn("dbo.DiscountGeographyImportStatus", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.DiscountGeographyImportStatus", "PackType", c => c.String());
            AddColumn("dbo.DiscountGeographyImportStatus", "PackGroup", c => c.String());
            AddColumn("dbo.DiscountGeographyImportStatus", "OilType", c => c.String());
            AddColumn("dbo.DiscountGeographies", "PackTypeId", c => c.Long(nullable: false));
            CreateIndex("dbo.SaudaConditionalBookingConfigurations", "PackGroupId");
            AddForeignKey("dbo.SaudaConditionalBookingConfigurations", "PackGroupId", "dbo.PackGroups", "Id");
        }
    }
}
