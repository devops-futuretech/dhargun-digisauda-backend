namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QpsDiscounts", "StateId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QpsDiscounts", "StateId");
        }
    }
}
