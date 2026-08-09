namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QpsDiscountandSlabDiscountDetailsAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QpsDiscounts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        SalesOrgId = c.Long(nullable: false),
                        DistributionChannelId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        ZoneId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SlabDiscountDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        QPSId = c.Int(nullable: false),
                        SlabName = c.String(),
                        FromRange = c.Int(nullable: false),
                        ToRange = c.Int(nullable: false),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SlabDiscountDetails");
            DropTable("dbo.QpsDiscounts");
        }
    }
}
