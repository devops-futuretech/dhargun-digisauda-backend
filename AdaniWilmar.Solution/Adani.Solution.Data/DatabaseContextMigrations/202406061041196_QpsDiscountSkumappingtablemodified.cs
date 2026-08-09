namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QpsDiscountSkumappingtablemodified : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QPSDiscountSkuMappings", "OilTypeId", c => c.Long(nullable: false));
            AddColumn("dbo.QPSDiscountSkuMappings", "IsActive", c => c.Boolean(nullable: false));
            DropColumn("dbo.QpsDiscounts", "OilTypeId");
            DropColumn("dbo.QPSDiscountSkuMappings", "ZoneId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QPSDiscountSkuMappings", "ZoneId", c => c.Long(nullable: false));
            AddColumn("dbo.QpsDiscounts", "OilTypeId", c => c.Long(nullable: false));
            DropColumn("dbo.QPSDiscountSkuMappings", "IsActive");
            DropColumn("dbo.QPSDiscountSkuMappings", "OilTypeId");
        }
    }
}
