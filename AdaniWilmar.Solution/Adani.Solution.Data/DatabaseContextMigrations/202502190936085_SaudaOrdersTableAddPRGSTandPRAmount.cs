namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaudaOrdersTableAddPRGSTandPRAmount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaudaOrders", "PRGST", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SaudaOrders", "PRAmount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaudaOrders", "PRAmount");
            DropColumn("dbo.SaudaOrders", "PRGST");
        }
    }
}
