namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserTableColumnsRemoved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "HeadquartersId", "dbo.Headquarters");
            DropForeignKey("dbo.Users", "SaudaBookingTypeId", "dbo.SaudaBookingTypes");
            DropForeignKey("dbo.Users", "ZoneId", "dbo.Zones");
            DropIndex("dbo.Users", new[] { "ZoneId" });
            DropIndex("dbo.Users", new[] { "HeadquartersId" });
            DropIndex("dbo.Users", new[] { "SaudaBookingTypeId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Users", "SaudaBookingTypeId");
            CreateIndex("dbo.Users", "HeadquartersId");
            CreateIndex("dbo.Users", "ZoneId");
            AddForeignKey("dbo.Users", "ZoneId", "dbo.Zones", "Id");
            AddForeignKey("dbo.Users", "SaudaBookingTypeId", "dbo.SaudaBookingTypes", "Id");
            AddForeignKey("dbo.Users", "HeadquartersId", "dbo.Headquarters", "Id");
        }
    }
}
