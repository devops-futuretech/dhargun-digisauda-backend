namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FormsTableRoleIdscolumnAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Forms", "RoleIds", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Forms", "RoleIds");
        }
    }
}
