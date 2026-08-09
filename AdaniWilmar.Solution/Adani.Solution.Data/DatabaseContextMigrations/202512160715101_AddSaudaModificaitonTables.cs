namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSaudaModificaitonTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SaudaModificationApprovals",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SaudaModificationId = c.Long(nullable: false),
                        RequestedBy = c.Long(nullable: false),
                        RequestedTo = c.Long(nullable: false),
                        ApprovedBy = c.Long(nullable: false),
                        StatusId = c.Long(nullable: false),
                        Remarks = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SaudaModifications", t => t.SaudaModificationId)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.SaudaModificationId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.SaudaModifications",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SaudaNumber = c.String(),
                        StatusId = c.Int(nullable: false),
                        IsSentToSAP = c.Boolean(nullable: false),
                        Remarks = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SaudaModificationItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SaudaModificationLineId = c.Long(nullable: false),
                        skuId = c.Long(nullable: false),
                        QuantityInCase = c.Decimal(nullable: false, precision: 18, scale: 3),
                        SaudaQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SaudaModificationLines", t => t.SaudaModificationLineId)
                .ForeignKey("dbo.Skus", t => t.skuId)
                .Index(t => t.SaudaModificationLineId)
                .Index(t => t.skuId);
            
            CreateTable(
                "dbo.SaudaModificationLines",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SaudaModificationId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        OilPackGroupTypeId = c.Long(nullable: false),
                        TotalOriginalPendingQty = c.Decimal(nullable: false, precision: 18, scale: 3),
                        TotalModifiedQty = c.Decimal(nullable: false, precision: 18, scale: 3),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId)
                .ForeignKey("dbo.SaudaModifications", t => t.SaudaModificationId)
                .Index(t => t.SaudaModificationId)
                .Index(t => t.OilTypeId);
            
            CreateTable(
                "dbo.SaudaModificationOldItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SaudaModificationLineId = c.Long(nullable: false),
                        skuId = c.Long(nullable: false),
                        QuantityInCase = c.Decimal(nullable: false, precision: 18, scale: 3),
                        SaudaQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SaudaModificationLines", t => t.SaudaModificationLineId)
                .ForeignKey("dbo.Skus", t => t.skuId)
                .Index(t => t.SaudaModificationLineId)
                .Index(t => t.skuId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SaudaModificationOldItems", "skuId", "dbo.Skus");
            DropForeignKey("dbo.SaudaModificationOldItems", "SaudaModificationLineId", "dbo.SaudaModificationLines");
            DropForeignKey("dbo.SaudaModificationItems", "skuId", "dbo.Skus");
            DropForeignKey("dbo.SaudaModificationItems", "SaudaModificationLineId", "dbo.SaudaModificationLines");
            DropForeignKey("dbo.SaudaModificationLines", "SaudaModificationId", "dbo.SaudaModifications");
            DropForeignKey("dbo.SaudaModificationLines", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.SaudaModificationApprovals", "StatusId", "dbo.Status");
            DropForeignKey("dbo.SaudaModificationApprovals", "SaudaModificationId", "dbo.SaudaModifications");
            DropIndex("dbo.SaudaModificationOldItems", new[] { "skuId" });
            DropIndex("dbo.SaudaModificationOldItems", new[] { "SaudaModificationLineId" });
            DropIndex("dbo.SaudaModificationLines", new[] { "OilTypeId" });
            DropIndex("dbo.SaudaModificationLines", new[] { "SaudaModificationId" });
            DropIndex("dbo.SaudaModificationItems", new[] { "skuId" });
            DropIndex("dbo.SaudaModificationItems", new[] { "SaudaModificationLineId" });
            DropIndex("dbo.SaudaModificationApprovals", new[] { "StatusId" });
            DropIndex("dbo.SaudaModificationApprovals", new[] { "SaudaModificationId" });
            DropTable("dbo.SaudaModificationOldItems");
            DropTable("dbo.SaudaModificationLines");
            DropTable("dbo.SaudaModificationItems");
            DropTable("dbo.SaudaModifications");
            DropTable("dbo.SaudaModificationApprovals");
        }
    }
}
