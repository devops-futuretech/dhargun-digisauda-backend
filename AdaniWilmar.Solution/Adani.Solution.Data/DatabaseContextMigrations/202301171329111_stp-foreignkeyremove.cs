namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stpforeignkeyremove : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SalesTourPlanMtpHistories", "CityId", "dbo.Cities");
            DropForeignKey("dbo.SalesTourPlanMtpHistories", "HeadquartersId", "dbo.Headquarters");
            DropForeignKey("dbo.SalesTourPlanPcpHistories", "CityId", "dbo.Cities");
            DropForeignKey("dbo.SalesTourPlanPcpHistories", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.SalesTourPlanPcpHistories", "StateId", "dbo.States");
            DropForeignKey("dbo.SalesTourPlanPcpHistories", "TerritoryId", "dbo.Territories");
            DropIndex("dbo.SalesTourPlanMtpHistories", new[] { "CityId" });
            DropIndex("dbo.SalesTourPlanMtpHistories", new[] { "HeadquartersId" });
            DropIndex("dbo.SalesTourPlanPcpHistories", new[] { "StateId" });
            DropIndex("dbo.SalesTourPlanPcpHistories", new[] { "TerritoryId" });
            DropIndex("dbo.SalesTourPlanPcpHistories", new[] { "DistrictId" });
            DropIndex("dbo.SalesTourPlanPcpHistories", new[] { "CityId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.SalesTourPlanPcpHistories", "CityId");
            CreateIndex("dbo.SalesTourPlanPcpHistories", "DistrictId");
            CreateIndex("dbo.SalesTourPlanPcpHistories", "TerritoryId");
            CreateIndex("dbo.SalesTourPlanPcpHistories", "StateId");
            CreateIndex("dbo.SalesTourPlanMtpHistories", "HeadquartersId");
            CreateIndex("dbo.SalesTourPlanMtpHistories", "CityId");
            AddForeignKey("dbo.SalesTourPlanPcpHistories", "TerritoryId", "dbo.Territories", "Id");
            AddForeignKey("dbo.SalesTourPlanPcpHistories", "StateId", "dbo.States", "Id");
            AddForeignKey("dbo.SalesTourPlanPcpHistories", "DistrictId", "dbo.Districts", "Id");
            AddForeignKey("dbo.SalesTourPlanPcpHistories", "CityId", "dbo.Cities", "Id");
            AddForeignKey("dbo.SalesTourPlanMtpHistories", "HeadquartersId", "dbo.Headquarters", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SalesTourPlanMtpHistories", "CityId", "dbo.Cities", "Id", cascadeDelete: true);
        }
    }
}
