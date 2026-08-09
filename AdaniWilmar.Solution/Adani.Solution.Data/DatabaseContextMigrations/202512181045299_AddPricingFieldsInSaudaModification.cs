namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPricingFieldsInSaudaModification : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaudaModificationItems", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.SaudaModificationItems", "Discount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaudaModificationItems", "Discount");
            DropColumn("dbo.SaudaModificationItems", "Price");
        }
    }
}
