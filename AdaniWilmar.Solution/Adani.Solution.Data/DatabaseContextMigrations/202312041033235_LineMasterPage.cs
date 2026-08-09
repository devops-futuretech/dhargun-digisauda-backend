namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LineMasterPage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lines",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "LineId", c => c.String());
            AddColumn("dbo.Skus", "LineId", c => c.String());
        }
        
        public override void Down()
        { 
            DropColumn("dbo.Skus", "LineId");
            DropColumn("dbo.Users", "LineId");
            DropTable("dbo.Lines");
        }
    }
}
