namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CrossAndUpsellContgractMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Saudas", "IsCrossAndUpsellContract", c => c.Boolean(nullable: false));
            AddColumn("dbo.SaudaOrders", "IsMandatorySku", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Saudas", "IsCrossAndUpsellContract");
            DropColumn("dbo.SaudaOrders", "IsMandatorySku");
        }
    }
}
