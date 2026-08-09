namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Gamification_Dashboard_Details_Table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GamificationDashboards",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DistributorId = c.Long(nullable: false),
                        DistributorCode = c.String(),
                        DistributorAchievementTillN1MT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RemainingTargetToAchieveMT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EarnedPoints = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrentSlab = c.String(),
                        NextHigherSlab = c.String(),
                        PointsToBeEarnedToReachNextHigherSlab = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalEarningsInRs = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SpecialBonusMessage = c.String(),
                        WholePointsStructure = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsDiamond = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GamificationDashboards");
        }
    }
}
