namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CompletedDoNumbers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompletedDoNumbers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DoNumber = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CompletedDoNumbers");
        }
    }
}
