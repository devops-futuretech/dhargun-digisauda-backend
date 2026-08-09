namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SalesShiptoprtyAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SalesRegisters", "ShiptoParty", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SalesRegisters", "ShiptoParty");
        }
    }
}
