namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SaudaTypeAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Saudas", "SaudaType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Saudas", "SaudaType");
        }
    }
}
