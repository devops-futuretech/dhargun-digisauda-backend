namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOpenSalesOrderQuantityInPendingContract : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PendingContracts", "OpenSalesOrderQuantity", c => c.Decimal(nullable: false, precision: 18, scale: 3));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PendingContracts", "OpenSalesOrderQuantity");
        }
    }
}
