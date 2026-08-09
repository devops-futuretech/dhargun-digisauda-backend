namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStateInDiscountUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DiscountUsers", "StateId", c => c.Long(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DiscountUsers", "StateId");
        }
    }
}
