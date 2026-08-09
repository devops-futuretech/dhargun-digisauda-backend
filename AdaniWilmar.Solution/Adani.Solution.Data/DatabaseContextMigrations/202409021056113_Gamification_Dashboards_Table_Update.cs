namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Gamification_Dashboards_Table_Update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GamificationDashboards", "DistributorTargetMT", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GamificationDashboards", "DistributorTargetMT");
        }
    }
}
