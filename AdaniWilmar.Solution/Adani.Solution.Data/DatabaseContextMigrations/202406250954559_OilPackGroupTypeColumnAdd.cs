namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OilPackGroupTypeColumnAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Skus", "OilPackGroupTypeId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Skus", "OilPackGroupTypeId");
        }
    }
}
