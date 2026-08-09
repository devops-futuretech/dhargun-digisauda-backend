namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComplaintManagement_Entities_Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnswerOptions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        QuestionId = c.Long(nullable: false),
                        Option = c.String(nullable: false, maxLength: 1000),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionMasters", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.QuestionMasters",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Query = c.String(nullable: false, maxLength: 4000),
                        QueryIdentifer = c.String(),
                        QuestionTypeId = c.Int(nullable: false),
                        QuestionSectionId = c.Long(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        IsMandatory = c.Boolean(nullable: false),
                        Description = c.String(maxLength: 4000),
                        OrderId = c.Int(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionSections", t => t.QuestionSectionId, cascadeDelete: true)
                .ForeignKey("dbo.QuestionTypes", t => t.QuestionTypeId, cascadeDelete: true)
                .Index(t => t.QuestionTypeId)
                .Index(t => t.QuestionSectionId);
            
            CreateTable(
                "dbo.FormQuestions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FormId = c.Long(nullable: false),
                        QuestionId = c.Long(nullable: false),
                        QuestionSectionId = c.Long(nullable: false),
                        OrderNo = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Forms", t => t.FormId, cascadeDelete: true)
                .ForeignKey("dbo.QuestionMasters", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.FormId)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Forms",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 2000),
                        IsActive = c.Boolean(nullable: false),
                        IsFormStatus = c.Boolean(nullable: false),
                        ParentFormId = c.Long(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.ScheduleDemoUsers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DemoUserId = c.Long(nullable: false),
                        SubmittedFormId = c.Long(nullable: false),
                        DependentMasterFormId = c.Long(),
                        DemoDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        DemoInchargeId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Form_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.DemoUserId, cascadeDelete: true)
                .ForeignKey("dbo.SubmittedForms", t => t.SubmittedFormId, cascadeDelete: true)
                .ForeignKey("dbo.Forms", t => t.Form_Id)
                .Index(t => t.DemoUserId)
                .Index(t => t.SubmittedFormId)
                .Index(t => t.Form_Id);
            
            CreateTable(
                "dbo.SubmittedForms",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(),
                        CustomerName = c.String(),
                        FormId = c.Long(nullable: false),
                        FormName = c.String(),
                        IsFormStatus = c.Boolean(nullable: false),
                        FormStatusId = c.Int(),
                        FormApprovalStatusId = c.Long(),
                        ParentFormId = c.Long(),
                        DemoUserId = c.Long(),
                        DemoId = c.Long(),
                        Remarks = c.String(maxLength: 4000),
                        DealerId = c.Long(),
                        DealerName = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Forms", t => t.FormId, cascadeDelete: true)
                .ForeignKey("dbo.FormStatus", t => t.FormStatusId)
                .ForeignKey("dbo.Retailers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.FormId)
                .Index(t => t.FormStatusId);
            
            CreateTable(
                "dbo.FormStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Retailers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AccountName = c.String(nullable: false, maxLength: 150),
                        Code = c.String(maxLength: 150),
                        Email = c.String(maxLength: 100),
                        MobileNumber = c.String(maxLength: 20),
                        SPFZoneId = c.Long(),
                        StateId = c.Int(),
                        DistrictId = c.Int(),
                        CityId = c.Int(),
                        TerritoryId = c.Int(),
                        Pincode = c.String(maxLength: 10),
                        Address = c.String(maxLength: 4000),
                        IsActive = c.Boolean(nullable: false),
                        FreightZoneId = c.Long(),
                        FreightRouteId = c.Long(),
                        VisitDay = c.String(),
                        DistributorSalesMan = c.String(),
                        DistributorSalesManCode = c.String(),
                        DistributorCode = c.String(),
                        DistributorName = c.String(),
                        ASOASEname = c.String(),
                        ASOASECode = c.String(),
                        AccountManager = c.String(),
                        AccountType = c.String(),
                        AreaName = c.String(),
                        OwnersName = c.String(),
                        DecisionMakerName = c.String(),
                        DecisionMakerNumber = c.String(),
                        ChefName = c.String(),
                        ChefNumber = c.String(),
                        Longitude = c.String(),
                        Latitude = c.String(),
                        VerticalId = c.Long(),
                        DealerId = c.Long(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Districts", t => t.DistrictId)
                .ForeignKey("dbo.FreightRoutes", t => t.FreightRouteId)
                .ForeignKey("dbo.FreightZones", t => t.FreightZoneId)
                .ForeignKey("dbo.Zones", t => t.SPFZoneId)
                .ForeignKey("dbo.States", t => t.StateId)
                .ForeignKey("dbo.Territories", t => t.TerritoryId)
                .ForeignKey("dbo.Verticals", t => t.VerticalId)
                .Index(t => t.SPFZoneId)
                .Index(t => t.StateId)
                .Index(t => t.DistrictId)
                .Index(t => t.CityId)
                .Index(t => t.TerritoryId)
                .Index(t => t.FreightZoneId)
                .Index(t => t.FreightRouteId)
                .Index(t => t.VerticalId);
            
            CreateTable(
                "dbo.FreightRoutes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FreightZoneId = c.Long(nullable: false),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FreightZones", t => t.FreightZoneId, cascadeDelete: true)
                .Index(t => t.FreightZoneId);
            
            CreateTable(
                "dbo.FreightZones",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        StateId = c.Int(),
                        ZoneId = c.Long(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.States", t => t.StateId)
                .ForeignKey("dbo.Zones", t => t.ZoneId)
                .Index(t => t.StateId)
                .Index(t => t.ZoneId);
            
            CreateTable(
                "dbo.Verticals",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Code = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        SAPCode = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubmittedFormQuestions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SubmittedFormId = c.Long(nullable: false),
                        QuestionId = c.Long(nullable: false),
                        Query = c.String(nullable: false, maxLength: 4000),
                        QuestionTypeId = c.Int(nullable: false),
                        QuestionTypeName = c.String(),
                        SectionId = c.Long(nullable: false),
                        SectionName = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SubmittedForms", t => t.SubmittedFormId, cascadeDelete: true)
                .Index(t => t.SubmittedFormId);
            
            CreateTable(
                "dbo.SubmittedFormAnswerOptions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SubmittedFormQuestionId = c.Long(nullable: false),
                        QuestionId = c.Long(nullable: false),
                        AnswerOptionId = c.Long(),
                        Option = c.String(maxLength: 1000),
                        TextAnswer = c.String(),
                        IsYes = c.Boolean(),
                        IsSelected = c.Boolean(),
                        AttachmentFileName = c.String(),
                        MediaTypeId = c.Int(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AnswerOptions", t => t.AnswerOptionId)
                .ForeignKey("dbo.MediaTypes", t => t.MediaTypeId)
                .ForeignKey("dbo.QuestionMasters", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.SubmittedFormQuestions", t => t.SubmittedFormQuestionId, cascadeDelete: true)
                .Index(t => t.SubmittedFormQuestionId)
                .Index(t => t.QuestionId)
                .Index(t => t.AnswerOptionId)
                .Index(t => t.MediaTypeId);
            
            CreateTable(
                "dbo.QuestionSections",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SectionName = c.String(nullable: false, maxLength: 1000),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.SectionName, unique: true);
            
            CreateTable(
                "dbo.QuestionTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionMasters", "QuestionTypeId", "dbo.QuestionTypes");
            DropForeignKey("dbo.QuestionMasters", "QuestionSectionId", "dbo.QuestionSections");
            DropForeignKey("dbo.FormQuestions", "QuestionId", "dbo.QuestionMasters");
            DropForeignKey("dbo.ScheduleDemoUsers", "Form_Id", "dbo.Forms");
            DropForeignKey("dbo.ScheduleDemoUsers", "SubmittedFormId", "dbo.SubmittedForms");
            DropForeignKey("dbo.SubmittedFormQuestions", "SubmittedFormId", "dbo.SubmittedForms");
            DropForeignKey("dbo.SubmittedFormAnswerOptions", "SubmittedFormQuestionId", "dbo.SubmittedFormQuestions");
            DropForeignKey("dbo.SubmittedFormAnswerOptions", "QuestionId", "dbo.QuestionMasters");
            DropForeignKey("dbo.SubmittedFormAnswerOptions", "MediaTypeId", "dbo.MediaTypes");
            DropForeignKey("dbo.SubmittedFormAnswerOptions", "AnswerOptionId", "dbo.AnswerOptions");
            DropForeignKey("dbo.SubmittedForms", "UserId", "dbo.Retailers");
            DropForeignKey("dbo.Retailers", "VerticalId", "dbo.Verticals");
            DropForeignKey("dbo.Retailers", "TerritoryId", "dbo.Territories");
            DropForeignKey("dbo.Retailers", "StateId", "dbo.States");
            DropForeignKey("dbo.Retailers", "SPFZoneId", "dbo.Zones");
            DropForeignKey("dbo.Retailers", "FreightZoneId", "dbo.FreightZones");
            DropForeignKey("dbo.Retailers", "FreightRouteId", "dbo.FreightRoutes");
            DropForeignKey("dbo.FreightRoutes", "FreightZoneId", "dbo.FreightZones");
            DropForeignKey("dbo.FreightZones", "ZoneId", "dbo.Zones");
            DropForeignKey("dbo.FreightZones", "StateId", "dbo.States");
            DropForeignKey("dbo.Retailers", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.Retailers", "CityId", "dbo.Cities");
            DropForeignKey("dbo.SubmittedForms", "FormStatusId", "dbo.FormStatus");
            DropForeignKey("dbo.SubmittedForms", "FormId", "dbo.Forms");
            DropForeignKey("dbo.ScheduleDemoUsers", "DemoUserId", "dbo.Users");
            DropForeignKey("dbo.FormQuestions", "FormId", "dbo.Forms");
            DropForeignKey("dbo.AnswerOptions", "QuestionId", "dbo.QuestionMasters");
            DropIndex("dbo.QuestionTypes", new[] { "Name" });
            DropIndex("dbo.QuestionSections", new[] { "SectionName" });
            DropIndex("dbo.SubmittedFormAnswerOptions", new[] { "MediaTypeId" });
            DropIndex("dbo.SubmittedFormAnswerOptions", new[] { "AnswerOptionId" });
            DropIndex("dbo.SubmittedFormAnswerOptions", new[] { "QuestionId" });
            DropIndex("dbo.SubmittedFormAnswerOptions", new[] { "SubmittedFormQuestionId" });
            DropIndex("dbo.SubmittedFormQuestions", new[] { "SubmittedFormId" });
            DropIndex("dbo.FreightZones", new[] { "ZoneId" });
            DropIndex("dbo.FreightZones", new[] { "StateId" });
            DropIndex("dbo.FreightRoutes", new[] { "FreightZoneId" });
            DropIndex("dbo.Retailers", new[] { "VerticalId" });
            DropIndex("dbo.Retailers", new[] { "FreightRouteId" });
            DropIndex("dbo.Retailers", new[] { "FreightZoneId" });
            DropIndex("dbo.Retailers", new[] { "TerritoryId" });
            DropIndex("dbo.Retailers", new[] { "CityId" });
            DropIndex("dbo.Retailers", new[] { "DistrictId" });
            DropIndex("dbo.Retailers", new[] { "StateId" });
            DropIndex("dbo.Retailers", new[] { "SPFZoneId" });
            DropIndex("dbo.FormStatus", new[] { "Name" });
            DropIndex("dbo.SubmittedForms", new[] { "FormStatusId" });
            DropIndex("dbo.SubmittedForms", new[] { "FormId" });
            DropIndex("dbo.SubmittedForms", new[] { "UserId" });
            DropIndex("dbo.ScheduleDemoUsers", new[] { "Form_Id" });
            DropIndex("dbo.ScheduleDemoUsers", new[] { "SubmittedFormId" });
            DropIndex("dbo.ScheduleDemoUsers", new[] { "DemoUserId" });
            DropIndex("dbo.Forms", new[] { "Name" });
            DropIndex("dbo.FormQuestions", new[] { "QuestionId" });
            DropIndex("dbo.FormQuestions", new[] { "FormId" });
            DropIndex("dbo.QuestionMasters", new[] { "QuestionSectionId" });
            DropIndex("dbo.QuestionMasters", new[] { "QuestionTypeId" });
            DropIndex("dbo.AnswerOptions", new[] { "QuestionId" });
            DropTable("dbo.QuestionTypes");
            DropTable("dbo.QuestionSections");
            DropTable("dbo.SubmittedFormAnswerOptions");
            DropTable("dbo.SubmittedFormQuestions");
            DropTable("dbo.Verticals");
            DropTable("dbo.FreightZones");
            DropTable("dbo.FreightRoutes");
            DropTable("dbo.Retailers");
            DropTable("dbo.FormStatus");
            DropTable("dbo.SubmittedForms");
            DropTable("dbo.ScheduleDemoUsers");
            DropTable("dbo.Forms");
            DropTable("dbo.FormQuestions");
            DropTable("dbo.QuestionMasters");
            DropTable("dbo.AnswerOptions");
        }
    }
}
