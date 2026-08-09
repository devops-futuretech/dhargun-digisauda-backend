namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QPSDiscountSkuMappingandQPSSlabDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QPSDiscountSkuMappings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        QpsDiscountId = c.Long(nullable: false),
                        ZoneId = c.Long(nullable: false),
                        StateId = c.Long(nullable: false),
                        SkuId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QpsDiscounts", t => t.QpsDiscountId, cascadeDelete: true)
                .Index(t => t.QpsDiscountId);
            
            CreateTable(
                "dbo.QPSSlabDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        QpsDiscountId = c.Long(nullable: false),
                        FromRange = c.Int(nullable: false),
                        ToRange = c.Int(nullable: false),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QpsDiscounts", t => t.QpsDiscountId, cascadeDelete: true)
                .Index(t => t.QpsDiscountId);
            
            DropColumn("dbo.QpsDiscounts", "ZoneId");
            DropColumn("dbo.QpsDiscounts", "StateId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QpsDiscounts", "StateId", c => c.Long(nullable: false));
            AddColumn("dbo.QpsDiscounts", "ZoneId", c => c.Long(nullable: false));
            DropForeignKey("dbo.QPSSlabDetails", "QpsDiscountId", "dbo.QpsDiscounts");
            DropForeignKey("dbo.QPSDiscountSkuMappings", "QpsDiscountId", "dbo.QpsDiscounts");
            DropIndex("dbo.QPSSlabDetails", new[] { "QpsDiscountId" });
            DropIndex("dbo.QPSDiscountSkuMappings", new[] { "QpsDiscountId" });
            DropTable("dbo.QPSSlabDetails");
            DropTable("dbo.QPSDiscountSkuMappings");
        }
    }
}
