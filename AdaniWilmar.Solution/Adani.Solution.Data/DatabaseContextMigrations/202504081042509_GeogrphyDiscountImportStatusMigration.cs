namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GeogrphyDiscountImportStatusMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DiscountGeographyImportStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SalesOrganization = c.String(),
                        DistributionChannel = c.String(),
                        Division = c.String(),
                        MaterialCode = c.String(),
                        DiscountReason = c.String(),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValidFrom = c.DateTime(nullable: false),
                        ValidTo = c.DateTime(nullable: false),
                        LoginUserId = c.Long(nullable: false),
                        Zone = c.String(),
                        State = c.String(),
                        District = c.String(),
                        City = c.String(),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.Id);                    
        }
        
        public override void Down()
        {          
            DropTable("dbo.DiscountGeographyImportStatus");
        }
    }
}
