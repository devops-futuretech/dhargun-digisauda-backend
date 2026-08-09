namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShiptoPartyCodeAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ShipToPartyCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ShipToPartyCode");
        }
    }
}
