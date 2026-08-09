namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Gamification_Dashboard_Details_Table_Update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GamificationDashboards", "DistributorTargetMT", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("dbo.GamificationDashboards", "DistributorAchievementTillN1MT", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("dbo.GamificationDashboards", "RemainingTargetToAchieveMT", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            AlterColumn("dbo.GamificationDashboards", "EarnedPoints", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GamificationDashboards", "EarnedPoints", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.GamificationDashboards", "RemainingTargetToAchieveMT", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.GamificationDashboards", "DistributorAchievementTillN1MT", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.GamificationDashboards", "DistributorTargetMT", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
