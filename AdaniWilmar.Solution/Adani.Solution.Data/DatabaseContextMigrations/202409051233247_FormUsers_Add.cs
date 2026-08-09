namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FormUsers_Add : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FormUsers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FormId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Forms", t => t.FormId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.FormId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FormUsers", "UserId", "dbo.Users");
            DropForeignKey("dbo.FormUsers", "FormId", "dbo.Forms");
            DropIndex("dbo.FormUsers", new[] { "UserId" });
            DropIndex("dbo.FormUsers", new[] { "FormId" });
            DropTable("dbo.FormUsers");
        }
    }
}
