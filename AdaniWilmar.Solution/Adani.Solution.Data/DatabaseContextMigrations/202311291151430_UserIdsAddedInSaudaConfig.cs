namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserIdsAddedInSaudaConfig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaudaBookingConfigurations", "UserIds", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaudaBookingConfigurations", "UserIds");
        }
    }
}
