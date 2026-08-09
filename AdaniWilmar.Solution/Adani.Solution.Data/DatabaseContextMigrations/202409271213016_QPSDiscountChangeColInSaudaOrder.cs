namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QPSDiscountChangeColInSaudaOrder : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SaudaOrders", "QPSIdWithDiscount");
            AddColumn("dbo.SaudaOrders", "QpsId", c => c.String());
            AddColumn("dbo.SaudaOrders", "IndividualQPSDiscount", c => c.String());
        }
        
        public override void Down()
        {
            AddColumn("dbo.SaudaOrders", "QPSIdWithDiscount", c => c.String());
            DropColumn("dbo.SaudaOrders", "IndividualQPSDiscount");
            DropColumn("dbo.SaudaOrders", "QpsId");
        }
    }
}
