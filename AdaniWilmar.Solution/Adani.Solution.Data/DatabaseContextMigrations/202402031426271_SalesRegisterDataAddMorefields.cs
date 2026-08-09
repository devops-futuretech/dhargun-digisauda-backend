namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesRegisterDataAddMorefields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesRegisters", "OrderNumber", c => c.String());
            AddColumn("dbo.SalesRegisters", "ContractNumber", c => c.String());
            AddColumn("dbo.SalesRegisters", "BrokerName", c => c.String());
            AddColumn("dbo.SalesRegisters", "LRNo", c => c.String());
            AddColumn("dbo.SalesRegisters", "VehicleNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesRegisters", "VehicleNumber");
            DropColumn("dbo.SalesRegisters", "LRNo");
            DropColumn("dbo.SalesRegisters", "BrokerName");
            DropColumn("dbo.SalesRegisters", "ContractNumber");
            DropColumn("dbo.SalesRegisters", "OrderNumber");
        }
    }
}
