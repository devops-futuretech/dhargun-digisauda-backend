namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datatypechangeroleid : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Forms", "RoleIds", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Forms", "RoleIds", c => c.Long(nullable: false));
        }
    }
}
