namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pending_Contract_Create_Date_add : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PendingContracts", "ContractValidFrom", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PendingContracts", "ContractValidFrom");
        }
    }
}
