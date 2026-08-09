namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddcolumnsinDiscountGeographyImportStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DiscountGeographyImportStatus", "OilType", c => c.String());
            AddColumn("dbo.DiscountGeographyImportStatus", "PackGroup", c => c.String());
            AddColumn("dbo.DiscountGeographyImportStatus", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DiscountGeographyImportStatus", "IsActive");
            DropColumn("dbo.DiscountGeographyImportStatus", "PackGroup");
            DropColumn("dbo.DiscountGeographyImportStatus", "OilType");
        }
    }
}
