namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShiftSaudaValidityPeriodAndPlantAtDivisionLevel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserDivisionDepotMappings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserDivisionId = c.Long(nullable: false),
                        DepotId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Depots", t => t.DepotId, cascadeDelete: true)
                .ForeignKey("dbo.UserDivisionMappings", t => t.UserDivisionId, cascadeDelete: true)
                .Index(t => t.UserDivisionId)
                .Index(t => t.DepotId);
            
            AddColumn("dbo.UserDivisionMappings", "SaudaValidityPeriod", c => c.Int());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDivisionDepotMappings", "UserDivisionId", "dbo.UserDivisionMappings");
            DropForeignKey("dbo.UserDivisionDepotMappings", "DepotId", "dbo.Depots");
            DropIndex("dbo.UserDivisionDepotMappings", new[] { "DepotId" });
            DropIndex("dbo.UserDivisionDepotMappings", new[] { "UserDivisionId" });
            DropColumn("dbo.UserDivisionMappings", "SaudaValidityPeriod");
            DropTable("dbo.UserDivisionDepotMappings");
        }
    }
}
