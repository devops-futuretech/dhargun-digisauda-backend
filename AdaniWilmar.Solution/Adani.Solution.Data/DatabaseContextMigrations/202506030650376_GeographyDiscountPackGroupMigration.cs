namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GeographyDiscountPackGroupMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SaudaConditionalBookingConfigurations", "OilTypeId", "dbo.OilTypes");
            DropIndex("dbo.SaudaConditionalBookingConfigurations", new[] { "OilTypeId" });
            AddColumn("dbo.DiscountGeographies", "PackGroupId", c => c.Long(nullable: false));
            AlterColumn("dbo.SaudaConditionalBookingConfigurations", "OilTypeId", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SaudaConditionalBookingConfigurations", "OilTypeId", c => c.Long(nullable: false));
            DropColumn("dbo.DiscountGeographies", "PackGroupId");
            CreateIndex("dbo.SaudaConditionalBookingConfigurations", "OilTypeId");
            AddForeignKey("dbo.SaudaConditionalBookingConfigurations", "OilTypeId", "dbo.OilTypes", "Id");
        }
    }
}
