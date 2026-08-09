namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedQPSIdWithDiscountField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaudaOrders", "QPSIdWithDiscount", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaudaOrders", "QPSIdWithDiscount");
        }
    }
}
