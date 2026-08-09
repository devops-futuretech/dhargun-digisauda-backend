namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedTerritoryIdDistrictsTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Districts", "TerritoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Districts", "TerritoryId", c => c.Int(nullable: false));
        }
    }
}
