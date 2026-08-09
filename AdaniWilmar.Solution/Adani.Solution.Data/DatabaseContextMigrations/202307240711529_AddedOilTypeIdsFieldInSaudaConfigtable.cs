namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOilTypeIdsFieldInSaudaConfigtable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaudaBookingConfigurations", "OilTypeIds", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaudaBookingConfigurations", "OilTypeIds");
        }
    }
}
