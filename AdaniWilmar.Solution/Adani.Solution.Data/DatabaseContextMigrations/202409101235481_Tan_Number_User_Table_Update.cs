namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tan_Number_User_Table_Update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "TANNumber", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "TANNumber");
        }
    }
}
