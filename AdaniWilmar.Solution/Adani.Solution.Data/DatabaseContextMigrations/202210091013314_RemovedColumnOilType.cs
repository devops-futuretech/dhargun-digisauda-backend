namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedColumnOilType : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OilTypes", "IsRasoi");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OilTypes", "IsRasoi", c => c.Boolean(nullable: false));
        }
    }
}
