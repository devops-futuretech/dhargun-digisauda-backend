namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CrossAndUpsellMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SaudaConditionalBookingConfigurations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SalesOrganizationId = c.Long(nullable: false),
                        DistributionChannelId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        OilTypeId = c.String(nullable: false),
                        PackGroupId = c.Long(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DistributionChannels", t => t.DistributionChannelId)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.PackGroups", t => t.PackGroupId)
                .ForeignKey("dbo.SalesOrganizations", t => t.SalesOrganizationId)
                .Index(t => t.SalesOrganizationId)
                .Index(t => t.DistributionChannelId)
                .Index(t => t.DivisionId)
                .Index(t => t.PackGroupId);
            
            CreateTable(
                "dbo.SaudaConditionalBookingEssentialSkuMappings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SaudaConditionalConfigurationId = c.Long(nullable: false),
                        EssentialSkuId = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SaudaConditionalBookingConfigurations", t => t.SaudaConditionalConfigurationId)
                .Index(t => t.SaudaConditionalConfigurationId);
            
            CreateTable(
                "dbo.SaudaConditionalBookingMandatorySkuMappings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ConditionalBookingEssentialSkuMappingId = c.Long(nullable: false),
                        MandatorySkuId = c.Long(nullable: false),
                        MandatorySkuCode = c.String(nullable: false),
                        MandatorySkuPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SaudaConditionalBookingEssentialSkuMappings", t => t.ConditionalBookingEssentialSkuMappingId)
                .Index(t => t.ConditionalBookingEssentialSkuMappingId);
            
            CreateTable(
                "dbo.SaudaConditionalBookingZoneStateMappings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SaudaConditionalConfigurationId = c.Long(nullable: false),
                        ZoneId = c.Long(nullable: false),
                        StateId = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SaudaConditionalBookingConfigurations", t => t.SaudaConditionalConfigurationId)
                .Index(t => t.SaudaConditionalConfigurationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaudaConditionalBookingZoneStateMappings", "SaudaConditionalConfigurationId", "dbo.SaudaConditionalBookingConfigurations");
            DropForeignKey("dbo.SaudaConditionalBookingMandatorySkuMappings", "ConditionalBookingEssentialSkuMappingId", "dbo.SaudaConditionalBookingEssentialSkuMappings");
            DropForeignKey("dbo.SaudaConditionalBookingEssentialSkuMappings", "SaudaConditionalConfigurationId", "dbo.SaudaConditionalBookingConfigurations");
            DropForeignKey("dbo.SaudaConditionalBookingConfigurations", "SalesOrganizationId", "dbo.SalesOrganizations");
            DropForeignKey("dbo.SaudaConditionalBookingConfigurations", "PackGroupId", "dbo.PackGroups");
            DropForeignKey("dbo.SaudaConditionalBookingConfigurations", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.SaudaConditionalBookingConfigurations", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.SaudaConditionalBookingConfigurations", "DistributionChannelId", "dbo.DistributionChannels");
            DropIndex("dbo.SaudaConditionalBookingZoneStateMappings", new[] { "SaudaConditionalConfigurationId" });
            DropIndex("dbo.SaudaConditionalBookingMandatorySkuMappings", new[] { "ConditionalBookingEssentialSkuMappingId" });
            DropIndex("dbo.SaudaConditionalBookingEssentialSkuMappings", new[] { "SaudaConditionalConfigurationId" });
            DropIndex("dbo.SaudaConditionalBookingConfigurations", new[] { "PackGroupId" });
            DropIndex("dbo.SaudaConditionalBookingConfigurations", new[] { "OilTypeId" });
            DropIndex("dbo.SaudaConditionalBookingConfigurations", new[] { "DivisionId" });
            DropIndex("dbo.SaudaConditionalBookingConfigurations", new[] { "DistributionChannelId" });
            DropIndex("dbo.SaudaConditionalBookingConfigurations", new[] { "SalesOrganizationId" });
            DropTable("dbo.SaudaConditionalBookingZoneStateMappings");
            DropTable("dbo.SaudaConditionalBookingMandatorySkuMappings");
            DropTable("dbo.SaudaConditionalBookingEssentialSkuMappings");
            DropTable("dbo.SaudaConditionalBookingConfigurations");
        }
    }
}
