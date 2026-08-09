namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Datatypechange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SlabDiscountDetails", "QPSId", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SlabDiscountDetails", "QPSId", c => c.Int(nullable: false));
        }
    }
}
