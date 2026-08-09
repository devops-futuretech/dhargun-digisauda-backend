namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GeographyDiscountIsActivceMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DiscountGeographies", "IsActive", c => c.Boolean(nullable: false));          
            AddColumn("dbo.SaudaOrders", "EmployeeSkuDiscountId", c => c.Long(nullable: false,defaultValue:0));          
        }
        
        public override void Down()
        {
            DropColumn("dbo.DiscountGeographies", "IsActive");         
            DropColumn("dbo.SaudaOrders", "EmployeeSkuDiscountId");         
        }
    }
}
