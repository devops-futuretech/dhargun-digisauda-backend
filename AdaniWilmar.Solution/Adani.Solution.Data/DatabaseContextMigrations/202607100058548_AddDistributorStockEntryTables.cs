namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDistributorStockEntryTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DistributorStockEntries",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        ReportedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);

            CreateIndex("dbo.DistributorStockEntries", new[] { "UserId", "ReportedDate" }, name: "IX_DistributorStockEntries_UserId_ReportedDate");

            CreateTable(
                "dbo.DistributorStockEntryDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DistributorStockEntryId = c.Long(nullable: false),
                        SkuId = c.Long(nullable: false),
                        QuantityInCase = c.Decimal(nullable: false, precision: 18, scale: 4),
                        QuantityInMT = c.Decimal(nullable: false, precision: 18, scale: 8),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DistributorStockEntries", t => t.DistributorStockEntryId)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .Index(t => t.DistributorStockEntryId)
                .Index(t => t.SkuId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DistributorStockEntryDetails", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.DistributorStockEntryDetails", "DistributorStockEntryId", "dbo.DistributorStockEntries");
            DropForeignKey("dbo.DistributorStockEntries", "UserId", "dbo.Users");
            DropIndex("dbo.DistributorStockEntryDetails", new[] { "SkuId" });
            DropIndex("dbo.DistributorStockEntryDetails", new[] { "DistributorStockEntryId" });
            DropIndex("dbo.DistributorStockEntries", "IX_DistributorStockEntries_UserId_ReportedDate");
            DropIndex("dbo.DistributorStockEntries", new[] { "UserId" });
            DropTable("dbo.DistributorStockEntryDetails");
            DropTable("dbo.DistributorStockEntries");
        }
    }
}
