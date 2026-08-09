namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedDateFieldInSaudaConfigTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaudaBookingConfigurations", "StartDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaudaBookingConfigurations", "StartDate");
        }
    }
}
