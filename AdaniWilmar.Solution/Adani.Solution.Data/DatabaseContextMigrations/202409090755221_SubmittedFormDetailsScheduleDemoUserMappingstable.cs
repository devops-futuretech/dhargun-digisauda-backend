namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubmittedFormDetailsScheduleDemoUserMappingstable : DbMigration
    {
        public override void Up()
        {
            ///DropForeignKey("dbo.QuestionMasters", "QuestionSectionId", "dbo.QuestionSections");
            //DropIndex("dbo.QuestionMasters", new[] { "QuestionSectionId" });
            //DropIndex("dbo.QuestionSections", new[] { "SectionName" });
            CreateTable(
                "dbo.ScheduleDemoUserMappings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DemoId = c.Long(nullable: false),
                        EALUserId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ScheduleDemoUsers", t => t.DemoId, cascadeDelete: true)
                .Index(t => t.DemoId);
            
            CreateTable(
                "dbo.SubmittedFormDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SubmittedFormId = c.Long(nullable: false),
                        SkuId = c.Long(nullable: false),
                        PlantId = c.Long(nullable: false),
                        StateId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: true)
                .ForeignKey("dbo.States", t => t.StateId, cascadeDelete: true)
                .ForeignKey("dbo.SubmittedForms", t => t.SubmittedFormId, cascadeDelete: true)
                .Index(t => t.SubmittedFormId)
                .Index(t => t.SkuId)
                .Index(t => t.StateId)
                .Index(t => t.CityId);
            
            //AddColumn("dbo.GamificationDashboards", "DistributorTargetMT", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            //AlterColumn("dbo.GamificationDashboards", "DistributorAchievementTillN1MT", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            //AlterColumn("dbo.GamificationDashboards", "RemainingTargetToAchieveMT", c => c.Decimal(nullable: false, precision: 18, scale: 4));
            //AlterColumn("dbo.GamificationDashboards", "EarnedPoints", c => c.Long(nullable: false));
            //DropColumn("dbo.QuestionMasters", "QuestionSectionId");
            //DropTable("dbo.QuestionSections");
        }
        
        public override void Down()
        {
            //CreateTable(
            //    "dbo.QuestionSections",
            //    c => new
            //        {
            //            Id = c.Long(nullable: false, identity: true),
            //            SectionName = c.String(nullable: false, maxLength: 1000),
            //            IsActive = c.Boolean(nullable: false),
            //            CreatedBy = c.Long(nullable: false),
            //            CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
            //            ModifiedBy = c.Long(),
            //            ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //AddColumn("dbo.QuestionMasters", "QuestionSectionId", c => c.Long(nullable: false));
            DropForeignKey("dbo.SubmittedFormDetails", "SubmittedFormId", "dbo.SubmittedForms");
            DropForeignKey("dbo.SubmittedFormDetails", "StateId", "dbo.States");
            DropForeignKey("dbo.SubmittedFormDetails", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.SubmittedFormDetails", "CityId", "dbo.Cities");
            DropForeignKey("dbo.ScheduleDemoUserMappings", "DemoId", "dbo.ScheduleDemoUsers");
            DropIndex("dbo.SubmittedFormDetails", new[] { "CityId" });
            DropIndex("dbo.SubmittedFormDetails", new[] { "StateId" });
            DropIndex("dbo.SubmittedFormDetails", new[] { "SkuId" });
            DropIndex("dbo.SubmittedFormDetails", new[] { "SubmittedFormId" });
            DropIndex("dbo.ScheduleDemoUserMappings", new[] { "DemoId" });
            //AlterColumn("dbo.GamificationDashboards", "EarnedPoints", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            //AlterColumn("dbo.GamificationDashboards", "RemainingTargetToAchieveMT", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            //AlterColumn("dbo.GamificationDashboards", "DistributorAchievementTillN1MT", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            //DropColumn("dbo.GamificationDashboards", "DistributorTargetMT");
            DropTable("dbo.SubmittedFormDetails");
            DropTable("dbo.ScheduleDemoUserMappings");
            //CreateIndex("dbo.QuestionSections", "SectionName", unique: true);
            //CreateIndex("dbo.QuestionMasters", "QuestionSectionId");
            //AddForeignKey("dbo.QuestionMasters", "QuestionSectionId", "dbo.QuestionSections", "Id", cascadeDelete: true);
        }
    }
}
