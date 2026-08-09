namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSaudaSalesAreaRestrictionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SaudaSalesAreaRestrictions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SalesOrganizationId = c.Long(nullable: false),
                        DistributionChannelId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        TimeRestriction = c.Time(nullable: false, precision: 7),
                        IsActive = c.Boolean(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DistributionChannels", t => t.DistributionChannelId)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.SalesOrganizations", t => t.SalesOrganizationId)
                .Index(t => t.SalesOrganizationId)
                .Index(t => t.DistributionChannelId)
                .Index(t => t.DivisionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaudaSalesAreaRestrictions", "SalesOrganizationId", "dbo.SalesOrganizations");
            DropForeignKey("dbo.SaudaSalesAreaRestrictions", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.SaudaSalesAreaRestrictions", "DistributionChannelId", "dbo.DistributionChannels");
            DropIndex("dbo.SaudaSalesAreaRestrictions", new[] { "DivisionId" });
            DropIndex("dbo.SaudaSalesAreaRestrictions", new[] { "DistributionChannelId" });
            DropIndex("dbo.SaudaSalesAreaRestrictions", new[] { "SalesOrganizationId" });
            DropTable("dbo.SaudaSalesAreaRestrictions");
        }
    }
}
