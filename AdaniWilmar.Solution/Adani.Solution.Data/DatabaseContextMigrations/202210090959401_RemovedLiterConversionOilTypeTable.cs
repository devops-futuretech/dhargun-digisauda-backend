namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedLiterConversionOilTypeTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OilTypes", "LitreConversion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OilTypes", "LitreConversion", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
