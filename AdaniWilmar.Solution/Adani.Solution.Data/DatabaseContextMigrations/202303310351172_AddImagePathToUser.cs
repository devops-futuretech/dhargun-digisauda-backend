namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImagePathToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ProfilePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ProfilePath");
        }
    }
}
