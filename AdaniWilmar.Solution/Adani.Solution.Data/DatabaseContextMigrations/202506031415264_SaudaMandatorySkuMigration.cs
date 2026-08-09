namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaudaMandatorySkuMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SaudaOrders", "IsMandatotySku", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SaudaOrders", "IsMandatotySku");
        }
    }
}
