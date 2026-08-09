namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequestedDiscountDateAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SpecalityFatDiscountUsers", "RequestedDiscountDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SpecalityFatDiscountUsers", "RequestedDiscountDate");
        }
    }
}
