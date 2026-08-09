namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSpecialRateRequestIdInSaudaTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Saudas", "SpecialRateRequestIdInParentTable", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Saudas", "SpecialRateRequestIdInParentTable");
        }
    }
}
