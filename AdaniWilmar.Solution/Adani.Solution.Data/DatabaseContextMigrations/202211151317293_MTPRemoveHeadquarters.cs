namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MTPRemoveHeadquarters : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MonthlyTourPlanDetails", "HeadquartersId", "dbo.Headquarters");
            DropForeignKey("dbo.MonthlyTourPlanDetails", "TownId", "dbo.Cities");
            DropIndex("dbo.MonthlyTourPlanDetails", new[] { "TownId" });
            DropIndex("dbo.MonthlyTourPlanDetails", new[] { "HeadquartersId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.MonthlyTourPlanDetails", "HeadquartersId");
            CreateIndex("dbo.MonthlyTourPlanDetails", "TownId");
            AddForeignKey("dbo.MonthlyTourPlanDetails", "TownId", "dbo.Cities", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MonthlyTourPlanDetails", "HeadquartersId", "dbo.Headquarters", "Id", cascadeDelete: true);
        }
    }
}
