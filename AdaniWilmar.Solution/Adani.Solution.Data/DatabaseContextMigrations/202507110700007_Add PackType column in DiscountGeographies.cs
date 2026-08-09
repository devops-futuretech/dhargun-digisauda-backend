namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPackTypecolumninDiscountGeographies : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DiscountGeographies", "PackTypeId", c => c.Long(nullable: false));
            AddColumn("dbo.DiscountGeographyImportStatus", "PackType", c => c.String());
        }
        
        public override void Down()
        {            
            DropColumn("dbo.DiscountGeographyImportStatus", "PackType");
            DropColumn("dbo.DiscountGeographies", "PackTypeId");
        }
    }
}
