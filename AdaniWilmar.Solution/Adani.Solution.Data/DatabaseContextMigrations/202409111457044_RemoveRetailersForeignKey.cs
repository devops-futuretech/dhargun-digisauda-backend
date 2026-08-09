namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRetailersForeignKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubmittedFormQuestions", "Answer", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SubmittedFormQuestions", "Answer");
        }
    }
}
