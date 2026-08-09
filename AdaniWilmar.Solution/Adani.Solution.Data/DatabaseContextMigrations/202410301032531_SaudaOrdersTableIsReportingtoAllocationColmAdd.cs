namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaudaOrdersTableIsReportingtoAllocationColmAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaudaOrders", "IsReportingtoAllocation", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaudaOrders", "IsReportingtoAllocation");
        }
    }
}
