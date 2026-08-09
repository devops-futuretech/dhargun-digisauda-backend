namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesRegisterData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesRegisters", "DeliveryNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesRegisters", "DeliveryNumber");
        }
    }
}
