namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewStatusAddInLift : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LiftingRequests", "IsCompleted", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Skus", "LineId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Skus", "LineId", c => c.Long());
            DropColumn("dbo.LiftingRequests", "IsCompleted");
        }
    }
}
