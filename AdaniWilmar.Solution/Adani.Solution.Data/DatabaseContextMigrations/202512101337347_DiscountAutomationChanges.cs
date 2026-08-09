namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DiscountAutomationChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Skus", "DiscountAutomationConversionUomId", c => c.Long());
            AddColumn("dbo.Skus", "DiscountAutomationConversionRelationUomId", c => c.Long());
            AddColumn("dbo.Skus", "DiscountAutomationConversionFactor1", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Skus", "DiscountAutomationConversionFactor2", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Skus", "DiscountAutomationConversionFactor2");
            DropColumn("dbo.Skus", "DiscountAutomationConversionFactor1");
            DropColumn("dbo.Skus", "DiscountAutomationConversionRelationUomId");
            DropColumn("dbo.Skus", "DiscountAutomationConversionUomId");
        }
    }
}
