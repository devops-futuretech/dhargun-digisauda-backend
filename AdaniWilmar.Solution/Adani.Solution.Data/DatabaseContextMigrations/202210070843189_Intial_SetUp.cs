namespace Adani.Solution.Data.DatabaseContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Intial_SetUp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountStatements",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        StatementDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DurationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ClosingBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DepositAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BankGuarantee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Email = c.String(maxLength: 150),
                        MobileNumber = c.String(maxLength: 40),
                        AdditionalMobileNumber = c.String(maxLength: 250),
                        Password = c.String(maxLength: 250),
                        OtpNumber = c.String(maxLength: 10),
                        PushTokenKey = c.String(maxLength: 1000),
                        ReportingToId = c.Long(),
                        Remarks = c.String(),
                        LastLoggedInDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        PreviousLoggedInDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        ApprovedBy = c.Long(),
                        ApprovedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        IsActiveForCall = c.Boolean(nullable: false),
                        IsBlacklisted = c.Boolean(nullable: false),
                        ImageUrl = c.String(maxLength: 1000),
                        ParentUserId = c.Long(),
                        RegistrationTypeId = c.Int(),
                        Region = c.String(maxLength: 500),
                        Pincode = c.String(maxLength: 10),
                        Street = c.String(maxLength: 500),
                        ZoneId = c.Long(),
                        DistrictId = c.Int(nullable: false),
                        District = c.String(maxLength: 500),
                        CityId = c.Int(nullable: false),
                        City = c.String(maxLength: 500),
                        StateId = c.Int(nullable: false),
                        State = c.String(maxLength: 500),
                        TerritoryId = c.Int(nullable: false),
                        Territory = c.String(maxLength: 500),
                        ExecutivePassword = c.String(maxLength: 250),
                        McsNo = c.String(maxLength: 50),
                        Code = c.String(maxLength: 4000),
                        MobileNumber2 = c.String(maxLength: 20),
                        GSTN = c.String(),
                        VisitDay = c.String(),
                        SaudaValidityPeriod = c.Int(),
                        SaudaLimit = c.Decimal(nullable: false, precision: 18, scale: 4),
                        WeeklyClosingDay = c.String(),
                        MonthlyPotential = c.String(),
                        Loadability = c.Decimal(nullable: false, precision: 18, scale: 4),
                        DepotLoadability = c.Decimal(nullable: false, precision: 18, scale: 4),
                        Address1 = c.String(maxLength: 4000),
                        Address2 = c.String(maxLength: 4000),
                        CustClass = c.String(),
                        Branch = c.String(),
                        SalesAccess = c.String(),
                        Designation = c.String(maxLength: 150),
                        HeadquartersId = c.Long(),
                        Acedns = c.String(),
                        SaudaBookingTypeId = c.Long(),
                        IncoTermsId = c.Long(),
                        TransportModeId = c.Long(),
                        IsSelf = c.Boolean(nullable: false),
                        IsBroker = c.Boolean(nullable: false),
                        PasswordModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ADRNR = c.String(),
                        CentralDeletionFlag = c.String(),
                        CustomerGroup = c.String(),
                        FSSAINumber = c.String(),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        InActiveRemarks = c.String(),
                        CustomerGroupFiveId = c.Long(nullable: false),
                        InActiveRemarkId = c.Long(),
                        ContactPersonName = c.String(maxLength: 250),
                        CompanyCode = c.String(),
                        DepartmentName = c.String(),
                        DirectManagerEmployee = c.String(),
                        OfficeCountry = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Headquarters", t => t.HeadquartersId)
                .ForeignKey("dbo.DeleteListCreations", t => t.InActiveRemarkId)
                .ForeignKey("dbo.IncoTerms", t => t.IncoTermsId)
                .ForeignKey("dbo.SaudaBookingTypes", t => t.SaudaBookingTypeId)
                .ForeignKey("dbo.Zones", t => t.ZoneId)
                .Index(t => t.ZoneId)
                .Index(t => t.HeadquartersId)
                .Index(t => t.SaudaBookingTypeId)
                .Index(t => t.IncoTermsId)
                .Index(t => t.InActiveRemarkId);
            
            CreateTable(
                "dbo.Headquarters",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Address = c.String(maxLength: 1000),
                        IsActive = c.Boolean(nullable: false),
                        ZoneId = c.Long(nullable: false),
                        StateId = c.Int(nullable: false),
                        TerritoryId = c.Int(nullable: false),
                        DistrictId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Districts", t => t.DistrictId)
                .ForeignKey("dbo.States", t => t.StateId)
                .ForeignKey("dbo.Territories", t => t.TerritoryId)
                .ForeignKey("dbo.Zones", t => t.ZoneId, cascadeDelete: true)
                .Index(t => t.ZoneId)
                .Index(t => t.StateId)
                .Index(t => t.TerritoryId)
                .Index(t => t.DistrictId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CityName = c.String(nullable: false, maxLength: 150),
                        DistrictId = c.Int(nullable: false),
                        SortOrder = c.Int(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Districts", t => t.DistrictId)
                .Index(t => t.DistrictId);
            
            CreateTable(
                "dbo.Districts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DistrictName = c.String(nullable: false, maxLength: 150),
                        StateId = c.Int(nullable: false),
                        TerritoryId = c.Int(nullable: false),
                        SortOrder = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.States", t => t.StateId)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StateName = c.String(nullable: false, maxLength: 150),
                        CountryId = c.Int(nullable: false),
                        SortOrder = c.Int(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.CountryId)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Code = c.String(maxLength: 3),
                        CurrencyName = c.String(maxLength: 100),
                        SortOrder = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Territories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        StateId = c.Int(nullable: false),
                        SortOrder = c.Int(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.States", t => t.StateId, cascadeDelete: true)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.Zones",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DeleteListCreations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DeleteListId = c.Long(nullable: false),
                        Remarks = c.String(nullable: false, maxLength: 4000),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IncoTerms",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Code = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        Type = c.Int(nullable: false),
                        SAPName = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SaudaBookingTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AdditionalCosts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OilTypeId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        PlantId = c.Long(nullable: false),
                        RatePerMt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId)
                .ForeignKey("dbo.Depots", t => t.PlantId, cascadeDelete: true)
                .Index(t => t.OilTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.PlantId);
            
            CreateTable(
                "dbo.Divisions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Code = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        ZPR4 = c.Boolean(nullable: false),
                        SalesDocumentType = c.String(),
                        SalesOrderDocumentType = c.String(),
                        SalesOrganizationId = c.Long(nullable: false),
                        DistributionChannelId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DistributionChannels", t => t.DistributionChannelId, cascadeDelete: true)
                .ForeignKey("dbo.SalesOrganizations", t => t.SalesOrganizationId, cascadeDelete: true)
                .Index(t => t.SalesOrganizationId)
                .Index(t => t.DistributionChannelId);
            
            CreateTable(
                "dbo.DistributionChannels",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        SalesOrganizationId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SalesOrganizations", t => t.SalesOrganizationId)
                .Index(t => t.SalesOrganizationId);
            
            CreateTable(
                "dbo.SalesOrganizations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Code = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OilTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        SalesOrganizationId = c.Long(nullable: false),
                        DistributionChannelId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        LitreConversion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        SAPCode = c.String(),
                        IsRasoi = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DistributionChannels", t => t.DistributionChannelId, cascadeDelete: true)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.SalesOrganizations", t => t.SalesOrganizationId, cascadeDelete: true)
                .Index(t => t.SalesOrganizationId)
                .Index(t => t.DistributionChannelId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "dbo.Depots",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Code = c.String(maxLength: 150),
                        Email = c.String(maxLength: 100),
                        MobileNumber = c.String(),
                        Pincode = c.String(maxLength: 10),
                        Location = c.String(maxLength: 4000),
                        StorageTypeId = c.Int(nullable: false),
                        Usage = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsPlant = c.Boolean(nullable: false),
                        IsSAPData = c.Boolean(nullable: false),
                        IsSAPDataSyncOrNot = c.Boolean(nullable: false),
                        DepotId = c.Long(nullable: false),
                        MappedStateId = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        QuestionId = c.Long(nullable: false),
                        Answer = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Question = c.String(nullable: false),
                        Isactive = c.Boolean(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RecordId = c.Long(nullable: false),
                        PageId = c.Int(nullable: false),
                        Url = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AudioFileDetailsForActiveCustomers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        MediaTypeId = c.Int(nullable: false),
                        AudioFileName = c.String(),
                        ImagePaths = c.String(),
                        DialerMobileNumber = c.String(),
                        ReceiverMobileNumber = c.String(),
                        DialerId = c.Long(nullable: false),
                        ReceiverId = c.Long(nullable: false),
                        CallRecordedFileName = c.String(),
                        CallDuation = c.Int(nullable: false),
                        CallStartTime = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MediaTypes", t => t.MediaTypeId, cascadeDelete: true)
                .Index(t => t.MediaTypeId);
            
            CreateTable(
                "dbo.MediaTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Audits",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Resource = c.String(nullable: false, maxLength: 20),
                        Action = c.Int(nullable: false),
                        Changeset = c.String(nullable: false),
                        PerformedBy = c.Long(),
                        PerformedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BaseGroupMargins",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OilTypeId = c.Long(nullable: false),
                        PackGroupId = c.Long(nullable: false),
                        CustomerGroupId = c.Long(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Margin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerGroups", t => t.CustomerGroupId)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId)
                .ForeignKey("dbo.PackGroups", t => t.PackGroupId)
                .Index(t => t.OilTypeId)
                .Index(t => t.PackGroupId)
                .Index(t => t.CustomerGroupId);
            
            CreateTable(
                "dbo.BaseGroupMarginStates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BaseGroupMarginId = c.Long(nullable: false),
                        StateId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BaseGroupMargins", t => t.BaseGroupMarginId)
                .Index(t => t.BaseGroupMarginId);
            
            CreateTable(
                "dbo.CustomerGroups",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        SalesOrganizationId = c.Long(nullable: false),
                        DistributionChannelId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsBaseGroup = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DistributionChannels", t => t.DistributionChannelId, cascadeDelete: true)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.SalesOrganizations", t => t.SalesOrganizationId, cascadeDelete: true)
                .Index(t => t.SalesOrganizationId)
                .Index(t => t.DistributionChannelId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "dbo.CustomerGroupDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CustomerGroupId = c.Long(nullable: false),
                        CustomerId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.CustomerGroups", t => t.CustomerGroupId, cascadeDelete: true)
                .Index(t => t.CustomerGroupId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.DerivedGroupMargins",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BaseGroupMarginId = c.Long(nullable: false),
                        CustomerGroupId = c.Long(nullable: false),
                        Formula = c.String(),
                        Margin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerGroups", t => t.CustomerGroupId)
                .ForeignKey("dbo.BaseGroupMargins", t => t.BaseGroupMarginId)
                .Index(t => t.BaseGroupMarginId)
                .Index(t => t.CustomerGroupId);
            
            CreateTable(
                "dbo.PackGroups",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        SAPName = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BaseSkuPriceDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SkuId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        SaudaBookingTypeId = c.Long(nullable: false),
                        OilPackingTypeId = c.Long(nullable: false),
                        StateId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        TransportModeId = c.Long(nullable: false),
                        PlantId = c.Long(nullable: false),
                        DepotId = c.Long(nullable: false),
                        FrieghtZoneId = c.Long(nullable: false),
                        FrieghtRouteId = c.Long(nullable: false),
                        BiddingWindowId = c.Long(nullable: false),
                        BiddingDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        MaterialCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PackingCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PrimaryFrieght = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SecondaryFrieght = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DepotCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DetentionCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HoneycombCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Margin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CushionMargin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SchemeCostRecovery = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Premium = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProcessCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SumOfIngredientCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TpPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RaMargin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BaseRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        XMargin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FinalRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExPlantPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ForDepotPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ForPlantPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExDepotPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExRakePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ForRakePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExPlantGuaranteePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ForPlantGuaranteePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExDepotGuaranteePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ForDepotGuaranteePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExRakeGuaranteePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ForRakeGuaranteePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ClearanceRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CounterBidOffer = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CounterBidLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BpCpJumb = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        PlantSecondaryFrieght = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LoadQuantity = c.Decimal(nullable: false, precision: 18, scale: 4),
                        PublishId = c.Long(),
                        IsPublish = c.Boolean(nullable: false),
                        MaterialCostId = c.Long(nullable: false),
                        IngredientCostId = c.String(),
                        PackingCostId = c.Long(nullable: false),
                        DepotCostId = c.Long(nullable: false),
                        DetentionCostId = c.Long(nullable: false),
                        ProfitMarginId = c.Long(nullable: false),
                        CushionMarginId = c.Long(nullable: false),
                        SchemeCostId = c.Long(nullable: false),
                        PrimaryFrieghtId = c.Long(nullable: false),
                        SecondaryFrieghtId = c.Long(nullable: false),
                        SecondaryFrieghtForPlantId = c.Long(nullable: false),
                        HoneycombCostId = c.Long(nullable: false),
                        RaMarginId = c.Long(nullable: false),
                        LoadCapacityId = c.Long(nullable: false),
                        SkuIngrediantPlantId = c.Long(nullable: false),
                        Guid = c.Guid(),
                        CustomerGroupId = c.Long(nullable: false),
                        GPjump = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExPlantSGST = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExPlantCGST = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExPlantIGST = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ForPlantSGST = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ForPlantCGST = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ForPlantIGST = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExDepotSGST = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExDepotCGST = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExDepotIGST = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ForDepotSGST = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ForDepotCGST = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ForDepotIGST = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GstId = c.Long(nullable: false),
                        CustomerGroupMarginId = c.Long(nullable: false),
                        CustomerGroupMargin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BaseSkuPrices",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PriceGenerateDetailId = c.Long(nullable: false),
                        CustomerGroupId = c.Long(nullable: false),
                        SkuId = c.Long(nullable: false),
                        BaseSkuTaskStatusId = c.Int(nullable: false),
                        DerivedSkuTaskStatusId = c.Int(nullable: false),
                        ParentId = c.Long(nullable: false),
                        GuId = c.Guid(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BdoChoosenDealerDetailsDuringCalls",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DealerId = c.Long(nullable: false),
                        DealerMobileNumber = c.String(),
                        BDOId = c.Long(nullable: false),
                        BDOMobileNumber = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BdoCompetitors",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Remarks = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        UserType = c.Int(nullable: false),
                        DealerId = c.Long(nullable: false),
                        BdoWholesellerId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BdoCompetitorSkus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BdoCompetitorId = c.Long(nullable: false),
                        SkuName = c.String(),
                        QuanityPerMt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BdoCompetitors", t => t.BdoCompetitorId, cascadeDelete: true)
                .Index(t => t.BdoCompetitorId);
            
            CreateTable(
                "dbo.Benefits",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BenefitTypeId = c.Long(nullable: false),
                        BenefitCategory = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BenefitTypes", t => t.BenefitTypeId, cascadeDelete: true)
                .Index(t => t.BenefitTypeId);
            
            CreateTable(
                "dbo.BenefitTypes",
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
            
            CreateTable(
                "dbo.BiddingWindows",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        BiddingWindowCustomerGroupId = c.Long(nullable: false),
                        BiddingDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        StartTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EndTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        NoOfAttemptsForBidding = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        StatusId = c.Int(nullable: false),
                        SkuAllocationTimeLimit = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        SaudaAllocationStartTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        SaudaAllocationEndTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        SaudaAllocationStatusId = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BiddingWindowCustomerGroups",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BiddingWindowId = c.Long(nullable: false),
                        CustomerGroupId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerGroups", t => t.CustomerGroupId)
                .ForeignKey("dbo.BiddingWindows", t => t.BiddingWindowId, cascadeDelete: true)
                .Index(t => t.BiddingWindowId)
                .Index(t => t.CustomerGroupId);
            
            CreateTable(
                "dbo.BiddingWindowVolumeCapacities",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BiddingWindowId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        VolumeCapacity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId)
                .ForeignKey("dbo.BiddingWindows", t => t.BiddingWindowId)
                .Index(t => t.BiddingWindowId)
                .Index(t => t.OilTypeId);
            
            CreateTable(
                "dbo.BiddingWindowNotificationTimings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BiddingWindowId = c.Long(nullable: false),
                        NotificationTypeId = c.Long(nullable: false),
                        NotificationTime = c.DateTime(nullable: false),
                        StatusId = c.Long(nullable: false),
                        CustomerGroupId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BiddingWindowStatus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BiddingWindowTimings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BiddingDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        FromHours = c.Time(nullable: false, precision: 7),
                        ToHours = c.Time(nullable: false, precision: 7),
                        Isactive = c.Boolean(nullable: false),
                        IsLastWindowPerDay = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BookingTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Bulletins",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(maxLength: 1000),
                        Content = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        ReviewedBy = c.Long(),
                        IsApproved = c.Boolean(),
                        ContentTypeId = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContentTypes", t => t.ContentTypeId, cascadeDelete: true)
                .Index(t => t.ContentTypeId);
            
            CreateTable(
                "dbo.BulletinMedias",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MediaPath = c.String(nullable: false, maxLength: 1500),
                        MediaTypeId = c.Int(nullable: false),
                        BulletinId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bulletins", t => t.BulletinId, cascadeDelete: true)
                .ForeignKey("dbo.MediaTypes", t => t.MediaTypeId, cascadeDelete: true)
                .Index(t => t.MediaTypeId)
                .Index(t => t.BulletinId);
            
            CreateTable(
                "dbo.ContentTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ChequeInventoryDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ControllingArea = c.String(),
                        UserCode = c.String(),
                        UserName = c.String(),
                        ChequeNo = c.String(),
                        NameOfBank = c.String(),
                        BranchName = c.String(),
                        UserId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Claims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 4000),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleClaims",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoleId = c.Long(nullable: false),
                        ClaimId = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Claims", t => t.ClaimId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.ClaimId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 4000),
                        IsPrime = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        RoleTypeId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        RoleHierarchy_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RoleTypes", t => t.RoleTypeId, cascadeDelete: true)
                .ForeignKey("dbo.RoleHierarchies", t => t.RoleHierarchy_Id)
                .Index(t => t.RoleTypeId)
                .Index(t => t.RoleHierarchy_Id);
            
            CreateTable(
                "dbo.RoleTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 4000),
                        HierarchyId = c.Int(nullable: false),
                        IsPrime = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleTypeClaims",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoleTypeId = c.Long(nullable: false),
                        ClaimId = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        RoleHierarchy_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Claims", t => t.ClaimId, cascadeDelete: true)
                .ForeignKey("dbo.RoleTypes", t => t.RoleTypeId, cascadeDelete: true)
                .ForeignKey("dbo.RoleHierarchies", t => t.RoleHierarchy_Id)
                .Index(t => t.RoleTypeId)
                .Index(t => t.ClaimId)
                .Index(t => t.RoleHierarchy_Id);
            
            CreateTable(
                "dbo.Competitors",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ZoneId = c.Long(nullable: false),
                        StateId = c.Int(nullable: false),
                        DistrictId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        TerritoryId = c.Int(nullable: false),
                        Address = c.String(maxLength: 4000),
                        Pincode = c.String(maxLength: 10),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.States", t => t.StateId, cascadeDelete: true)
                .ForeignKey("dbo.Zones", t => t.ZoneId, cascadeDelete: true)
                .Index(t => t.ZoneId)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.CompetitorAnalysis",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SkuId = c.Long(nullable: false),
                        OilTypeId = c.Long(),
                        StatusId = c.Long(),
                        Margin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EmamiPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remarks = c.String(maxLength: 4000),
                        WorkableQuantity = c.Long(nullable: false),
                        WorkablePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.SkuId)
                .Index(t => t.OilTypeId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.Skus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SkuName = c.String(nullable: false, maxLength: 150),
                        SkuCode = c.String(maxLength: 150),
                        SalesOrganizationId = c.Long(nullable: false),
                        DistributionChannelId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        OilTypeId = c.Long(),
                        IsActive = c.Boolean(nullable: false),
                        IsRequiredToAttachTT = c.Boolean(nullable: false),
                        ProcessCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PackTypeId = c.Long(nullable: false),
                        PackGroupId = c.Long(),
                        DivisionGroupId = c.Long(nullable: false),
                        UomId = c.Long(),
                        SubCategoryId = c.Long(),
                        SapStatusId = c.Int(nullable: false),
                        LitreConversion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsSAPData = c.Boolean(nullable: false),
                        IsSAPDataSyncOrNot = c.Boolean(nullable: false),
                        IsBaseSku = c.Boolean(nullable: false),
                        GrossWeight = c.Decimal(nullable: false, precision: 18, scale: 8),
                        PremiumAmount = c.Decimal(precision: 18, scale: 2),
                        StorageLocation = c.String(),
                        BusinessLine = c.String(),
                        ParentMaterialCode = c.String(),
                        QuantityTypeUom = c.Long(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DistributionChannels", t => t.DistributionChannelId, cascadeDelete: true)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId)
                .ForeignKey("dbo.PackGroups", t => t.PackGroupId)
                .ForeignKey("dbo.PackTypes", t => t.PackTypeId, cascadeDelete: true)
                .ForeignKey("dbo.SalesOrganizations", t => t.SalesOrganizationId, cascadeDelete: true)
                .ForeignKey("dbo.SubCategories", t => t.SubCategoryId)
                .ForeignKey("dbo.Uoms", t => t.UomId)
                .Index(t => t.SalesOrganizationId)
                .Index(t => t.DistributionChannelId)
                .Index(t => t.DivisionId)
                .Index(t => t.OilTypeId)
                .Index(t => t.PackTypeId)
                .Index(t => t.PackGroupId)
                .Index(t => t.UomId)
                .Index(t => t.SubCategoryId);
            
            CreateTable(
                "dbo.PackTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        SAPCode = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubCategories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Uoms",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        IsQuantityType = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        SAPName = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CompetitorAnalysisApprovals",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CompetitorAnalysisId = c.Long(nullable: false),
                        RequestedBy = c.Long(nullable: false),
                        RequestedTo = c.Long(nullable: false),
                        ApprovedBy = c.Long(nullable: false),
                        StatusId = c.Long(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CompetitorAnalysis", t => t.CompetitorAnalysisId, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.CompetitorAnalysisId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.CompetitorAnalysisDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CompetitorAnalysisId = c.Long(nullable: false),
                        CompetitorId = c.Long(nullable: false),
                        SaudaRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MarketOperatingPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Competitors", t => t.CompetitorId, cascadeDelete: true)
                .ForeignKey("dbo.CompetitorAnalysis", t => t.CompetitorAnalysisId, cascadeDelete: true)
                .Index(t => t.CompetitorAnalysisId)
                .Index(t => t.CompetitorId);
            
            CreateTable(
                "dbo.CompetitorSkus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CompetitorId = c.Long(nullable: false),
                        SkuId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Competitors", t => t.CompetitorId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: true)
                .Index(t => t.CompetitorId)
                .Index(t => t.SkuId);
            
            CreateTable(
                "dbo.ConfigurationForDivisionsAndEmails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Key = c.String(maxLength: 250),
                        Value = c.String(),
                        Isactive = c.Boolean(nullable: false),
                        TypeId = c.Int(nullable: false),
                        SaudaBookingTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Configurations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Key = c.String(maxLength: 250),
                        Value = c.String(maxLength: 250),
                        Isactive = c.Boolean(nullable: false),
                        TypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConsentImageDetailsForCustomers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        FileName = c.String(),
                        MediaPath = c.String(),
                        MediaTypeId = c.Int(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MediaTypes", t => t.MediaTypeId)
                .Index(t => t.MediaTypeId);
            
            CreateTable(
                "dbo.ContractTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Code = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConversionFormulaDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ConversionFormulaId = c.Long(nullable: false),
                        SkuId = c.Long(nullable: false),
                        Formula = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ConversionFormulas", t => t.ConversionFormulaId)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .Index(t => t.ConversionFormulaId)
                .Index(t => t.SkuId);
            
            CreateTable(
                "dbo.ConversionFormulas",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OilTypeId = c.Long(nullable: false),
                        PackGroupId = c.Long(nullable: false),
                        SkuId = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId)
                .ForeignKey("dbo.PackGroups", t => t.PackGroupId)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .Index(t => t.OilTypeId)
                .Index(t => t.PackGroupId)
                .Index(t => t.SkuId);
            
            CreateTable(
                "dbo.CounterBidJumps",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(precision: 7, storeType: "datetime2"),
                        OilTypeId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        PackGroupId = c.Long(nullable: false),
                        CounterbidJump = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        DistributionChannel_Id = c.Long(),
                        SalesOrganization_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DistributionChannels", t => t.DistributionChannel_Id)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId, cascadeDelete: true)
                .ForeignKey("dbo.PackGroups", t => t.PackGroupId, cascadeDelete: true)
                .ForeignKey("dbo.SalesOrganizations", t => t.SalesOrganization_Id)
                .Index(t => t.OilTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.PackGroupId)
                .Index(t => t.DistributionChannel_Id)
                .Index(t => t.SalesOrganization_Id);
            
            CreateTable(
                "dbo.CounterBidNotifications",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BiddingWindowId = c.Long(nullable: false),
                        SaudaBiddingCartId = c.Long(nullable: false),
                        CounterBidOffer = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StatusId = c.Long(nullable: false),
                        SkuId = c.Long(nullable: false),
                        DealerId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CreditNotes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        CreditNoteDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Number = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CushionMargins",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SkuId = c.Long(),
                        SalesOrganizationId = c.Long(),
                        DistributionChannelId = c.Long(),
                        DivisionId = c.Long(),
                        OilTypeId = c.Long(nullable: false),
                        OilPackingTypeId = c.Long(nullable: false),
                        ZoneId = c.Long(nullable: false),
                        StateId = c.Int(nullable: false),
                        TerritoryId = c.Int(nullable: false),
                        DistrictId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        RatePerMt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CustomerCategoryWise = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.DistributionChannels", t => t.DistributionChannelId)
                .ForeignKey("dbo.Districts", t => t.DistrictId)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.PackGroups", t => t.OilPackingTypeId, cascadeDelete: true)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId, cascadeDelete: true)
                .ForeignKey("dbo.SalesOrganizations", t => t.SalesOrganizationId)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .ForeignKey("dbo.States", t => t.StateId)
                .ForeignKey("dbo.Territories", t => t.TerritoryId)
                .ForeignKey("dbo.Zones", t => t.ZoneId, cascadeDelete: true)
                .Index(t => t.SkuId)
                .Index(t => t.SalesOrganizationId)
                .Index(t => t.DistributionChannelId)
                .Index(t => t.DivisionId)
                .Index(t => t.OilTypeId)
                .Index(t => t.OilPackingTypeId)
                .Index(t => t.ZoneId)
                .Index(t => t.StateId)
                .Index(t => t.TerritoryId)
                .Index(t => t.DistrictId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.CustomerGroupFives",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        GroupCode = c.String(nullable: false),
                        GroupName = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomerGroupMappings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CustomerGroupId = c.Long(nullable: false),
                        DerivedCustomerGroupId = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerGroups", t => t.CustomerGroupId, cascadeDelete: true)
                .Index(t => t.CustomerGroupId);
            
            CreateTable(
                "dbo.CustomerLedgerDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomerLedgers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Reference = c.String(),
                        PostingDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        DocumentType = c.String(),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.Long(nullable: false),
                        UserCode = c.String(),
                        CompanyCode = c.String(),
                        Currency = c.String(),
                        Credit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Debit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomerShipToPartyMappings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CustomerId = c.Long(nullable: false),
                        ShipToPartyId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CustomerId)
                .ForeignKey("dbo.Users", t => t.ShipToPartyId)
                .Index(t => t.CustomerId)
                .Index(t => t.ShipToPartyId);
            
            CreateTable(
                "dbo.CustomerTruckCapacityMappings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        TruckCapacity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StorageTypeId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DateRanges",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FromRange1 = c.Int(nullable: false),
                        ToRange1 = c.Int(nullable: false),
                        FromRange2 = c.Int(nullable: false),
                        ToRange2 = c.Int(nullable: false),
                        FromRange3 = c.Int(nullable: false),
                        ToRange3 = c.Int(nullable: false),
                        FromRange4 = c.Int(nullable: false),
                        ToRange4 = c.Int(nullable: false),
                        ToRange5 = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DayOfWeekNames",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsHoliday = c.Boolean(nullable: false),
                        SortOrder = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DealerLocations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        StateId = c.Int(),
                        DistrictId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        Address = c.String(),
                        IsSAPData = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.DeliveryPriorities",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Code = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DepotCosts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DepotId = c.Long(nullable: false),
                        SalesOrganizationId = c.Long(nullable: false),
                        DistributionChannelId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        SkuId = c.Long(),
                        OilTypeId = c.Long(),
                        OilPackingTypeId = c.Long(),
                        RatePerMt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Depots", t => t.DepotId, cascadeDelete: true)
                .ForeignKey("dbo.Divisions", t => t.DivisionId, cascadeDelete: true)
                .ForeignKey("dbo.PackGroups", t => t.OilPackingTypeId)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .Index(t => t.DepotId)
                .Index(t => t.DivisionId)
                .Index(t => t.SkuId)
                .Index(t => t.OilTypeId)
                .Index(t => t.OilPackingTypeId);
            
            CreateTable(
                "dbo.DetentionCosts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DepotId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        RatePerMt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Depots", t => t.DepotId, cascadeDelete: true)
                .Index(t => t.DepotId);
            
            CreateTable(
                "dbo.DiscountGeographies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SalesOrganizationId = c.Long(nullable: false),
                        DistributionChannelId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        SkuId = c.Long(nullable: false),
                        ActualDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountReason = c.String(),
                        ZoneId = c.Long(nullable: false),
                        StateId = c.Long(nullable: false),
                        TerritoryId = c.Long(nullable: false),
                        DistrictId = c.Long(nullable: false),
                        CityId = c.Long(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ParentId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DistributionChannels", t => t.DistributionChannelId, cascadeDelete: true)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId)
                .ForeignKey("dbo.SalesOrganizations", t => t.SalesOrganizationId)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .Index(t => t.SalesOrganizationId)
                .Index(t => t.DistributionChannelId)
                .Index(t => t.DivisionId)
                .Index(t => t.OilTypeId)
                .Index(t => t.SkuId);
            
            CreateTable(
                "dbo.DiscountSkus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SkuId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        ActualDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RequestedDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SaudaBookingTypeId = c.Long(nullable: false),
                        Status = c.Boolean(nullable: false),
                        ApprovedBy = c.Long(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId)
                .ForeignKey("dbo.SaudaBookingTypes", t => t.SaudaBookingTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: true)
                .Index(t => t.SkuId)
                .Index(t => t.OilTypeId)
                .Index(t => t.SaudaBookingTypeId);
            
            CreateTable(
                "dbo.DiscountUsers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SalesOrganizationId = c.Long(nullable: false),
                        DistributionChannelId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        SkuId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        ActualDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RequestedDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountReason = c.String(),
                        SaudaBookingTypeId = c.Long(nullable: false),
                        Status = c.Boolean(nullable: false),
                        ApprovedBy = c.Long(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ParentId = c.Long(nullable: false),
                        ParentDiscountId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DistributionChannels", t => t.DistributionChannelId, cascadeDelete: true)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId)
                .ForeignKey("dbo.SalesOrganizations", t => t.SalesOrganizationId)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.SalesOrganizationId)
                .Index(t => t.DistributionChannelId)
                .Index(t => t.DivisionId)
                .Index(t => t.OilTypeId)
                .Index(t => t.SkuId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.EmailTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Template = c.String(nullable: false),
                        PlainTemplate = c.String(nullable: false),
                        SMSTemplate = c.String(),
                        SMSTemplateID = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FeedbackRequests",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        FeedbackTypeId = c.Long(nullable: false),
                        Details = c.String(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FeedbackTypes", t => t.FeedbackTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.FeedbackTypeId);
            
            CreateTable(
                "dbo.FeedbackTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FillerSkuBasedOnDealers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SkuId = c.Long(nullable: false),
                        PackTypeId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        BidQuantityInCases = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PackTypes", t => t.PackTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.SkuId)
                .Index(t => t.PackTypeId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.FinancialYears",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Year = c.String(nullable: false),
                        EffectiveFrom = c.DateTime(nullable: false),
                        EffectiveTo = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GPSTrackings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Gsts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DepotId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        SourceStateId = c.Int(nullable: false),
                        DestinationStateId = c.Int(nullable: false),
                        CGST = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SGST = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IGST = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ParentId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.States", t => t.DestinationStateId)
                .ForeignKey("dbo.States", t => t.SourceStateId)
                .Index(t => t.SourceStateId)
                .Index(t => t.DestinationStateId);
            
            CreateTable(
                "dbo.GuaranteePriceJumps",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(precision: 7, storeType: "datetime2"),
                        OilTypeId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        PackGroupId = c.Long(nullable: false),
                        StartValue = c.Int(nullable: false),
                        EndValue = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId, cascadeDelete: true)
                .ForeignKey("dbo.PackGroups", t => t.PackGroupId, cascadeDelete: true)
                .Index(t => t.OilTypeId)
                .Index(t => t.DivisionId)
                .Index(t => t.PackGroupId);
            
            CreateTable(
                "dbo.Holidays",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HolidayName = c.String(),
                        HolidayDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        Year = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.HoneycombCosts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PlantId = c.Long(),
                        DivisionId = c.Long(),
                        OilTypeId = c.Long(),
                        SkuId = c.Long(nullable: false),
                        TransportModeId = c.Long(nullable: false),
                        ZoneId = c.Long(nullable: false),
                        StateId = c.Int(),
                        RatePerMt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RatePerCase = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId)
                .ForeignKey("dbo.Depots", t => t.PlantId)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: true)
                .ForeignKey("dbo.States", t => t.StateId)
                .ForeignKey("dbo.TransportModes", t => t.TransportModeId, cascadeDelete: true)
                .ForeignKey("dbo.Zones", t => t.ZoneId, cascadeDelete: true)
                .Index(t => t.PlantId)
                .Index(t => t.DivisionId)
                .Index(t => t.OilTypeId)
                .Index(t => t.SkuId)
                .Index(t => t.TransportModeId)
                .Index(t => t.ZoneId)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.TransportModes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InvoiceDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ItemNo = c.String(),
                        InvoiceId = c.Long(nullable: false),
                        MaterialNumber = c.String(),
                        QuantityInCase = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SkuId = c.Long(nullable: false),
                        ActualBilledQuantity = c.Decimal(nullable: false, precision: 18, scale: 4),
                        OilTypeId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId, cascadeDelete: true)
                .Index(t => t.InvoiceId);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        LiftingRequestId = c.Long(nullable: false),
                        InvoiceDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        TotalInvoice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BillingDocument = c.String(),
                        SAPDocumentNo = c.String(),
                        UserCode = c.String(),
                        SalesOrganization = c.String(),
                        IsSAPDataSync = c.Boolean(nullable: false),
                        Status = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.IssueComments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SupportId = c.Long(nullable: false),
                        Comments = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.KeyPerformanceIndicators",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoleId = c.Long(nullable: false),
                        Content = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LiftingRequests",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LiftingRequestNumber = c.String(),
                        LiftingDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UserId = c.Long(nullable: false),
                        LiftingStatusId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        ApprovedBy = c.Long(nullable: false),
                        ApproverRemarks = c.String(),
                        CustomerRemarks = c.String(),
                        IsSAPDataSync = c.Boolean(nullable: false),
                        ShipToPartyId = c.Long(),
                        PlantId = c.Long(nullable: false),
                        SAPDocumentNo = c.String(),
                        SAPDeliveryNo = c.String(),
                        SAPInvoiceNo = c.String(),
                        QantityInCase = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ApproveDate = c.DateTime(nullable: false),
                        IsSAPSalesOrder = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.ShipToPartyId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ShipToPartyId);
            
            CreateTable(
                "dbo.LiftingRequestDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LiftingRequestId = c.Long(nullable: false),
                        SkuId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        LiftingQuantity = c.Decimal(nullable: false, precision: 18, scale: 4),
                        LiftingQuantityCase = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeliveryOrderNumber = c.String(),
                        ItemNo = c.String(),
                        StatusId = c.Int(nullable: false),
                        UomId = c.Long(nullable: false),
                        DOStatusId = c.Int(nullable: false),
                        Remarks = c.String(),
                        EnquiryNumber = c.String(),
                        EnquiryRemarks = c.String(),
                        ReprocessStatusId = c.Boolean(nullable: false),
                        EnquiryNumberSyncFromSap = c.Boolean(nullable: false),
                        SaudaOrderId = c.Long(nullable: false),
                        SaudaNumber = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LiftingRequests", t => t.LiftingRequestId, cascadeDelete: true)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .Index(t => t.LiftingRequestId)
                .Index(t => t.SkuId)
                .Index(t => t.OilTypeId);
            
            CreateTable(
                "dbo.LiftingRequestStatus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LoadCapacityConversions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SalesOrganizationId = c.Long(),
                        DistributionChannelId = c.Long(),
                        DivisionId = c.Long(),
                        OilTypeId = c.Long(),
                        SkuId = c.Long(nullable: false),
                        TransportModeId = c.Long(nullable: false),
                        LoadCapacity = c.Decimal(nullable: false, precision: 18, scale: 4),
                        LoadQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ActualLoadQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DistributionChannels", t => t.DistributionChannelId)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId)
                .ForeignKey("dbo.SalesOrganizations", t => t.SalesOrganizationId)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: true)
                .ForeignKey("dbo.TransportModes", t => t.TransportModeId, cascadeDelete: true)
                .Index(t => t.SalesOrganizationId)
                .Index(t => t.DistributionChannelId)
                .Index(t => t.DivisionId)
                .Index(t => t.OilTypeId)
                .Index(t => t.SkuId)
                .Index(t => t.TransportModeId);
            
            CreateTable(
                "dbo.MarketScenarios",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DealerId = c.Long(nullable: false),
                        Title = c.String(),
                        Remarks = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MaterialCosts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PlantId = c.Long(nullable: false),
                        SalesOrganizationId = c.Long(),
                        DistributionChannelId = c.Long(),
                        DivisionId = c.Long(),
                        OilTypeId = c.Long(nullable: false),
                        RatePerMt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DistributionChannels", t => t.DistributionChannelId)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Depots", t => t.PlantId, cascadeDelete: true)
                .ForeignKey("dbo.SalesOrganizations", t => t.SalesOrganizationId)
                .Index(t => t.PlantId)
                .Index(t => t.SalesOrganizationId)
                .Index(t => t.DistributionChannelId)
                .Index(t => t.DivisionId)
                .Index(t => t.OilTypeId);
            
            CreateTable(
                "dbo.MonthlyPlanDeviations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MonthlyTourPlanDetailsId = c.Long(nullable: false),
                        RevisedDate = c.DateTime(nullable: false),
                        Remarks = c.String(),
                        ApproverId = c.Long(nullable: false),
                        StatusId = c.Long(nullable: false),
                        ApproverRemarks = c.String(),
                        ReasonId = c.Long(nullable: false),
                        ToDealerId = c.Long(nullable: false),
                        ToDealer = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MonthlyTourPlanDetails", t => t.MonthlyTourPlanDetailsId, cascadeDelete: true)
                .Index(t => t.MonthlyTourPlanDetailsId);
            
            CreateTable(
                "dbo.MonthlyTourPlanDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MonthlyTourPlanId = c.Long(nullable: false),
                        Date = c.DateTime(nullable: false),
                        TownId = c.Int(nullable: false),
                        Area = c.String(),
                        DealerId = c.String(),
                        HeadquartersId = c.Long(nullable: false),
                        Remarks = c.String(),
                        InHQNoVisit = c.Int(nullable: false),
                        VisitRemarks = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Headquarters", t => t.HeadquartersId, cascadeDelete: true)
                .ForeignKey("dbo.MonthlyTourPlans", t => t.MonthlyTourPlanId, cascadeDelete: true)
                .ForeignKey("dbo.Cities", t => t.TownId, cascadeDelete: true)
                .Index(t => t.MonthlyTourPlanId)
                .Index(t => t.TownId)
                .Index(t => t.HeadquartersId);
            
            CreateTable(
                "dbo.MonthlyTourPlans",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MTPNumber = c.String(),
                        MonthlyTourPlanStatusId = c.Int(nullable: false),
                        Remarks = c.String(),
                        PJPId = c.Long(nullable: false),
                        MonthId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MonthlyTourPlanStatus", t => t.MonthlyTourPlanStatusId, cascadeDelete: true)
                .Index(t => t.MonthlyTourPlanStatusId);
            
            CreateTable(
                "dbo.MonthlyTourPlanStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MonthlyTourPlanApprovalInformations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MonthlyTourPlanId = c.Long(nullable: false),
                        MonthlyTourPlanStatusId = c.Int(nullable: false),
                        UserId = c.Long(nullable: false),
                        Remarks = c.String(),
                        ReasonId = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MonthlyTourPlans", t => t.MonthlyTourPlanId, cascadeDelete: true)
                .Index(t => t.MonthlyTourPlanId);
            
            CreateTable(
                "dbo.MonthlyPlanDeviationStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Months",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.NotificationHistories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        NotificationActionId = c.Long(nullable: false),
                        BiddingWindowId = c.Long(nullable: false),
                        CustomerGroupId = c.Long(nullable: false),
                        CustomerId = c.Long(nullable: false),
                        IsEmail = c.Boolean(nullable: false),
                        IsSms = c.Boolean(nullable: false),
                        IsPushNotification = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Request = c.String(),
                        RequestId = c.Long(nullable: false),
                        ReferenceId = c.Long(nullable: false),
                        Notification = c.String(),
                        StatusId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OilTransferCosts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OilTypeId = c.Long(nullable: false),
                        SourceId = c.Long(nullable: false),
                        DestinationId = c.Long(nullable: false),
                        SalesOrganizationId = c.Long(nullable: false),
                        DistributionChannelId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        SkuId = c.Long(),
                        RatePerMt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Depots", t => t.DestinationId, cascadeDelete: true)
                .ForeignKey("dbo.DistributionChannels", t => t.DistributionChannelId, cascadeDelete: true)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId)
                .ForeignKey("dbo.SalesOrganizations", t => t.SalesOrganizationId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .ForeignKey("dbo.Depots", t => t.SourceId)
                .Index(t => t.OilTypeId)
                .Index(t => t.SourceId)
                .Index(t => t.DestinationId)
                .Index(t => t.SalesOrganizationId)
                .Index(t => t.DistributionChannelId)
                .Index(t => t.DivisionId)
                .Index(t => t.SkuId);
            
            CreateTable(
                "dbo.OverduePayments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Reference = c.String(),
                        PostingDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        DocumentType = c.String(),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.Long(nullable: false),
                        UserCode = c.String(),
                        CompanyCode = c.String(),
                        Currency = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PackingCosts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DivisionId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        SkuId = c.Long(),
                        PlantId = c.Long(nullable: false),
                        ActualPackingCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesPackingCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Divisions", t => t.DivisionId, cascadeDelete: true)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId)
                .ForeignKey("dbo.Depots", t => t.PlantId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .Index(t => t.DivisionId)
                .Index(t => t.OilTypeId)
                .Index(t => t.SkuId)
                .Index(t => t.PlantId);
            
            CreateTable(
                "dbo.PendingContracts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        SaudaOrderId = c.Long(nullable: false),
                        SaudaNumber = c.String(),
                        MaterialCode = c.String(),
                        CustomerCode = c.String(),
                        CustomerName = c.String(),
                        ContractValidTo = c.DateTime(),
                        BasicRate = c.Decimal(nullable: false, precision: 18, scale: 3),
                        PendingQuantityInCase = c.Decimal(nullable: false, precision: 18, scale: 3),
                        SaudaQuantity = c.Decimal(nullable: false, precision: 18, scale: 3),
                        SalesOrgId = c.Long(nullable: false),
                        DistChnlId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        TotalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsSaudaExtended = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PendingSaudaRemarks",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DealerId = c.Long(nullable: false),
                        SaudaId = c.Long(nullable: false),
                        Remarks = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PercentileNumbers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DivisionId = c.Long(nullable: false),
                        PercentileNumbers = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        ValidFrom = c.DateTime(nullable: false),
                        ValidTo = c.DateTime(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Divisions", t => t.DivisionId, cascadeDelete: true)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "dbo.PercentileNumberDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PackGroupId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        PercentileNumberId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId, cascadeDelete: true)
                .ForeignKey("dbo.PackGroups", t => t.PackGroupId, cascadeDelete: true)
                .ForeignKey("dbo.PercentileNumbers", t => t.PercentileNumberId)
                .Index(t => t.PackGroupId)
                .Index(t => t.OilTypeId)
                .Index(t => t.PercentileNumberId);
            
            CreateTable(
                "dbo.PermanentJourneyPlanDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PermanentJourneyPlanId = c.Long(nullable: false),
                        RetailerId = c.String(nullable: false),
                        MonthId = c.Long(nullable: false),
                        StateId = c.Long(nullable: false),
                        TerritoryId = c.Long(nullable: false),
                        DistrictId = c.Long(nullable: false),
                        TownId = c.Long(nullable: false),
                        NoOfDirectDealer = c.String(),
                        NoofSubDealer = c.String(),
                        NoOfWholeSeller = c.String(),
                        NoOfVisit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InHQNoVisit = c.Int(nullable: false),
                        Remarks = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PermanentJourneyPlans", t => t.PermanentJourneyPlanId, cascadeDelete: true)
                .Index(t => t.PermanentJourneyPlanId);
            
            CreateTable(
                "dbo.PermanentJourneyPlans",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PJPNumber = c.String(),
                        PermanentJourneyPlanStatusId = c.Long(nullable: false),
                        FinancialYearId = c.Long(nullable: false),
                        Remarks = c.String(),
                        Isactive = c.Boolean(nullable: false),
                        EffectiveFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EffectiveTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PermanentJourneyPlanStatus", t => t.PermanentJourneyPlanStatusId, cascadeDelete: true)
                .ForeignKey("dbo.FinancialYears", t => t.FinancialYearId, cascadeDelete: true)
                .Index(t => t.PermanentJourneyPlanStatusId)
                .Index(t => t.FinancialYearId);
            
            CreateTable(
                "dbo.PermanentJourneyPlanApprovalInformations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PermanentJourneyPlanId = c.Long(nullable: false),
                        StatusId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        Remarks = c.String(),
                        ReasonId = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PermanentJourneyPlans", t => t.PermanentJourneyPlanId, cascadeDelete: true)
                .Index(t => t.PermanentJourneyPlanId);
            
            CreateTable(
                "dbo.PermanentJourneyPlanStatus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PickingPoints",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Code = c.String(nullable: false, maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlantDepotMappings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PlantId = c.Long(nullable: false),
                        DepotId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PremiumDiscounts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SkuId = c.Long(nullable: false),
                        RoleId = c.Long(nullable: false),
                        OilTypeId = c.Long(),
                        ActualDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RequestedDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SaudaBookingTypeId = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ApprovedBy = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.SaudaBookingTypes", t => t.SaudaBookingTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: true)
                .Index(t => t.SkuId)
                .Index(t => t.RoleId)
                .Index(t => t.OilTypeId)
                .Index(t => t.SaudaBookingTypeId);
            
            CreateTable(
                "dbo.PremiumGeographies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SkuId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        ActualPremium = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ZoneId = c.Long(nullable: false),
                        StateId = c.Long(nullable: false),
                        TerritoryId = c.Long(nullable: false),
                        DistrictId = c.Long(nullable: false),
                        CityId = c.Long(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ParentId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .Index(t => t.SkuId)
                .Index(t => t.OilTypeId);
            
            CreateTable(
                "dbo.PremiumUsers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SkuId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        ActualPremium = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RequestedPremium = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ParentPremiumId = c.Long(nullable: false),
                        ParentId = c.Long(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.SkuId)
                .Index(t => t.UserId)
                .Index(t => t.OilTypeId);
            
            CreateTable(
                "dbo.PriceGenerates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SaudaBookingTypeId = c.Long(nullable: false),
                        VerticalId = c.Long(nullable: false),
                        ExeStatusId = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SaudaBookingTypes", t => t.SaudaBookingTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Divisions", t => t.VerticalId, cascadeDelete: true)
                .Index(t => t.SaudaBookingTypeId)
                .Index(t => t.VerticalId);
            
            CreateTable(
                "dbo.PriceGenerateDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PriceGenerateId = c.Long(nullable: false),
                        OilTypeId = c.String(),
                        PackGroupId = c.String(),
                        PlantId = c.Long(nullable: false),
                        ZoneId = c.String(),
                        StateId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        TaskStatusId = c.Int(nullable: false),
                        IsPublish = c.Boolean(nullable: false),
                        StartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ErrorMessage = c.String(),
                        ErrorMessageCount = c.Int(nullable: false),
                        CounterBidLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BpCpJump = c.Decimal(nullable: false, precision: 18, scale: 2),
                        XMargin = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BiddingWindowId = c.Long(nullable: false),
                        CustomerGroupId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Depots", t => t.PlantId, cascadeDelete: true)
                .ForeignKey("dbo.PriceGenerates", t => t.PriceGenerateId, cascadeDelete: true)
                .Index(t => t.PriceGenerateId)
                .Index(t => t.PlantId);
            
            CreateTable(
                "dbo.PriceNotifyConfigurations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IncoTermId = c.String(),
                        ZoneId = c.String(),
                        StateId = c.String(),
                        TerritoryId = c.String(),
                        CityId = c.String(),
                        SkuId = c.String(),
                        IsSMS = c.Boolean(nullable: false),
                        IsEmail = c.Boolean(nullable: false),
                        IsPushNotification = c.Boolean(nullable: false),
                        NotificationDate = c.DateTime(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PricePublishes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        StatusId = c.Long(nullable: false),
                        OilTypeId = c.String(),
                        PlantId = c.Long(nullable: false),
                        IsPublish = c.Boolean(nullable: false),
                        SaudaBookingTypeId = c.Long(nullable: false),
                        ErrorMessage = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Depots", t => t.PlantId, cascadeDelete: true)
                .ForeignKey("dbo.SaudaBookingTypes", t => t.SaudaBookingTypeId, cascadeDelete: true)
                .Index(t => t.PlantId)
                .Index(t => t.SaudaBookingTypeId);
            
            CreateTable(
                "dbo.Pricings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SAPPricingCode = c.String(),
                        SkuId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        OilPackingTypeId = c.Long(nullable: false),
                        PlantId = c.Long(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrganizationId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        DistributionChannelId = c.Long(nullable: false),
                        ValidFrom = c.DateTime(nullable: false),
                        ValidTo = c.DateTime(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .Index(t => t.SkuId);
            
            CreateTable(
                "dbo.PricingBackups",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SAPPricingCode = c.String(),
                        SkuId = c.Long(nullable: false),
                        PlantId = c.Long(nullable: false),
                        DepotId = c.Long(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrganizationId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        DistributionChannelId = c.Long(nullable: false),
                        ValidFrom = c.DateTime(nullable: false),
                        ValidTo = c.DateTime(nullable: false),
                        PublishId = c.Long(),
                        IsPublish = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PricingUpdateFrequencies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PrimaryDiscountSkus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SkuId = c.Long(nullable: false),
                        ActualDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RequestedDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SaudaBookingTypeId = c.Long(nullable: false),
                        Status = c.Boolean(nullable: false),
                        ApprovedBy = c.Long(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SaudaBookingTypes", t => t.SaudaBookingTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: true)
                .Index(t => t.SkuId)
                .Index(t => t.SaudaBookingTypeId);
            
            CreateTable(
                "dbo.PrimaryFreights",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PlantId = c.Long(),
                        DepotId = c.Long(nullable: false),
                        VerticalId = c.Long(nullable: false),
                        TransportModeId = c.Long(nullable: false),
                        LoadCapacity = c.Decimal(nullable: false, precision: 18, scale: 4),
                        HireCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ActualFreight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesFreight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Depots", t => t.DepotId, cascadeDelete: true)
                .ForeignKey("dbo.Depots", t => t.PlantId)
                .ForeignKey("dbo.TransportModes", t => t.TransportModeId, cascadeDelete: true)
                .ForeignKey("dbo.Divisions", t => t.VerticalId, cascadeDelete: true)
                .Index(t => t.PlantId)
                .Index(t => t.DepotId)
                .Index(t => t.VerticalId)
                .Index(t => t.TransportModeId);
            
            CreateTable(
                "dbo.ProfitMargins",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DivisionId = c.Long(),
                        OilTypeId = c.Long(nullable: false),
                        SkuId = c.Long(),
                        OilPackingTypeId = c.Long(nullable: false),
                        CustomerCategoryWise = c.String(),
                        ZoneId = c.Long(nullable: false),
                        StateId = c.Int(nullable: false),
                        TerritoryId = c.Int(),
                        DistrictId = c.Int(),
                        CityId = c.Int(),
                        RatePerMt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Districts", t => t.DistrictId)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.PackGroups", t => t.OilPackingTypeId, cascadeDelete: true)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .ForeignKey("dbo.States", t => t.StateId, cascadeDelete: true)
                .ForeignKey("dbo.Territories", t => t.TerritoryId)
                .ForeignKey("dbo.Zones", t => t.ZoneId, cascadeDelete: true)
                .Index(t => t.DivisionId)
                .Index(t => t.OilTypeId)
                .Index(t => t.SkuId)
                .Index(t => t.OilPackingTypeId)
                .Index(t => t.ZoneId)
                .Index(t => t.StateId)
                .Index(t => t.TerritoryId)
                .Index(t => t.DistrictId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.ProspectiveDealers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Email = c.String(maxLength: 100),
                        MobileNumber = c.String(maxLength: 20),
                        StateId = c.Int(),
                        DistrictId = c.Int(),
                        CityId = c.Int(),
                        Pincode = c.String(maxLength: 10),
                        Address = c.String(maxLength: 4000),
                        IsActive = c.Boolean(nullable: false),
                        ProspectiveSales = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProspectiveInterestLevel = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BusinessPotentialPeryear = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DealerId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuantityTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RaMargins",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DivisionId = c.Long(),
                        OilTypeId = c.Long(nullable: false),
                        SkuId = c.Long(),
                        OilPackingTypeId = c.Long(nullable: false),
                        StateId = c.Int(nullable: false),
                        DistrictId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        ZoneId = c.Long(nullable: false),
                        TerritoryId = c.Int(nullable: false),
                        CustomerCategoryWise = c.String(),
                        RatePerMt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Districts", t => t.DistrictId)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.PackGroups", t => t.OilPackingTypeId, cascadeDelete: true)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .ForeignKey("dbo.States", t => t.StateId)
                .ForeignKey("dbo.Territories", t => t.TerritoryId)
                .ForeignKey("dbo.Zones", t => t.ZoneId, cascadeDelete: true)
                .Index(t => t.DivisionId)
                .Index(t => t.OilTypeId)
                .Index(t => t.SkuId)
                .Index(t => t.OilPackingTypeId)
                .Index(t => t.StateId)
                .Index(t => t.DistrictId)
                .Index(t => t.CityId)
                .Index(t => t.ZoneId)
                .Index(t => t.TerritoryId);
            
            CreateTable(
                "dbo.RAMaterialCosts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PlantId = c.Long(nullable: false),
                        DivisionId = c.Long(),
                        OilTypeId = c.Long(nullable: false),
                        RatePerMt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Depots", t => t.PlantId, cascadeDelete: true)
                .Index(t => t.PlantId)
                .Index(t => t.DivisionId)
                .Index(t => t.OilTypeId);
            
            CreateTable(
                "dbo.RaNotifications",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SMS = c.Boolean(nullable: false),
                        Email = c.Boolean(nullable: false),
                        InAppNotification = c.Boolean(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CautionNotificationTimes = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RaNotificationDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RaNotificationId = c.Long(nullable: false),
                        CustomerGroupId = c.Long(nullable: false),
                        DealerId = c.Long(nullable: false),
                        NotificationActionId = c.Long(nullable: false),
                        WindowVolumeCapacity = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerGroups", t => t.CustomerGroupId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.DealerId, cascadeDelete: true)
                .ForeignKey("dbo.RaNotifications", t => t.RaNotificationId, cascadeDelete: true)
                .Index(t => t.RaNotificationId)
                .Index(t => t.CustomerGroupId)
                .Index(t => t.DealerId);
            
            CreateTable(
                "dbo.RaSaudaConfigurations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        GuaranteePricePercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SaudaAllocationTime = c.Time(nullable: false, precision: 7),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reasons",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Reason = c.String(nullable: false),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegionName = c.String(),
                        TamilName = c.String(maxLength: 150),
                        SortOrder = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Remarks",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TableId = c.Long(nullable: false),
                        TableName = c.String(),
                        ReasonTypeId = c.Int(nullable: false),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleDiscounts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SkuId = c.Long(nullable: false),
                        RoleId = c.Long(nullable: false),
                        OilTypeId = c.Long(),
                        ActualDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RequestedDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SaudaBookingTypeId = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                        ApprovedBy = c.Long(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.SaudaBookingTypes", t => t.SaudaBookingTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: true)
                .Index(t => t.SkuId)
                .Index(t => t.RoleId)
                .Index(t => t.OilTypeId)
                .Index(t => t.SaudaBookingTypeId);
            
            CreateTable(
                "dbo.RoleHierarchies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 4000),
                        HierarchyId = c.Int(nullable: false),
                        RoleId = c.Long(nullable: false),
                        IsPrime = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SalesDocumentTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        Code = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SalesRegisters",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        InvoiceId = c.Long(nullable: false),
                        SkuId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        MaterialCode = c.String(),
                        CustomerCode = c.String(),
                        QuantityCase = c.Decimal(nullable: false, precision: 18, scale: 3),
                        QuantityMT = c.Decimal(nullable: false, precision: 18, scale: 3),
                        InvoiceType = c.String(),
                        InvoiceNumber = c.String(),
                        InvoiceDate = c.DateTime(nullable: false),
                        TotalGST = c.String(),
                        TotalAmount = c.String(),
                        SalesOrganization = c.String(),
                        DistributionChannel = c.String(),
                        Division = c.String(),
                        SalesOrganizationId = c.Long(nullable: false),
                        DistributionChannelId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SalesTourPlanMtpHistories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DealerId = c.Long(nullable: false),
                        CityId = c.Int(nullable: false),
                        Area = c.String(),
                        HeadquartersId = c.Long(nullable: false),
                        Remarks = c.String(),
                        MonthlyTourPlanDetailId = c.Long(nullable: false),
                        InHQNoVisit = c.Int(nullable: false),
                        TourDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsDataChanged = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.Headquarters", t => t.HeadquartersId, cascadeDelete: true)
                .Index(t => t.CityId)
                .Index(t => t.HeadquartersId);
            
            CreateTable(
                "dbo.SalesTourPlanPcpHistories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FinancialYearId = c.Long(nullable: false),
                        StateId = c.Int(nullable: false),
                        TerritoryId = c.Int(nullable: false),
                        DistrictId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        NoOfDirectDealer = c.String(),
                        NoofSubDealer = c.String(),
                        NoOfWholeSeller = c.String(),
                        NoOfVisit = c.Long(nullable: false),
                        PermanentJourneyPlanDetailId = c.Long(nullable: false),
                        DealerId = c.Long(nullable: false),
                        IsDataChanged = c.Boolean(nullable: false),
                        EffectiveFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EffectiveTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        InHQNoVisit = c.Int(nullable: false),
                        Remarks = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Districts", t => t.DistrictId)
                .ForeignKey("dbo.FinancialYears", t => t.FinancialYearId, cascadeDelete: true)
                .ForeignKey("dbo.States", t => t.StateId)
                .ForeignKey("dbo.Territories", t => t.TerritoryId)
                .Index(t => t.FinancialYearId)
                .Index(t => t.StateId)
                .Index(t => t.TerritoryId)
                .Index(t => t.DistrictId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Saudas",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        BiddingDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsSAPDataSync = c.Boolean(nullable: false),
                        IsSAPDataSyncApproval = c.Boolean(nullable: false),
                        SalesOrganizationId = c.Long(nullable: false),
                        DistributionChannelId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        SalesDocumentType = c.String(),
                        SaudaBookingTypeId = c.Long(nullable: false),
                        SaudaNumber = c.String(),
                        IsSapSauda = c.Boolean(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DistributionChannels", t => t.DistributionChannelId, cascadeDelete: true)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.SalesOrganizations", t => t.SalesOrganizationId, cascadeDelete: true)
                .ForeignKey("dbo.SaudaBookingTypes", t => t.SaudaBookingTypeId, cascadeDelete: true)
                .Index(t => t.SalesOrganizationId)
                .Index(t => t.DistributionChannelId)
                .Index(t => t.DivisionId)
                .Index(t => t.SaudaBookingTypeId);
            
            CreateTable(
                "dbo.SaudaApprovals",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SaudaId = c.Long(nullable: false),
                        RequestedBy = c.Long(nullable: false),
                        RequestedTo = c.Long(nullable: false),
                        ApprovedBy = c.Long(nullable: false),
                        StatusId = c.Long(nullable: false),
                        Remarks = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Saudas", t => t.SaudaId, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusId, cascadeDelete: true)
                .Index(t => t.SaudaId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.SaudaAudioFileMappings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        SaudaId = c.Long(nullable: false),
                        SaudaOrderId = c.Long(nullable: false),
                        SaudaNumber = c.String(),
                        AudioFileDetailsForActiveCustomersId = c.Long(),
                        MediaTypeId = c.Int(nullable: false),
                        ImagePath = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AudioFileDetailsForActiveCustomers", t => t.AudioFileDetailsForActiveCustomersId)
                .Index(t => t.AudioFileDetailsForActiveCustomersId);
            
            CreateTable(
                "dbo.SaudaBiddingCarts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BiddingWindowId = c.Long(nullable: false),
                        BiddingDateAndTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DealerId = c.Long(nullable: false),
                        IncotermId = c.Long(nullable: false),
                        PlantId = c.Long(nullable: false),
                        DepotId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        SkuId = c.Long(nullable: false),
                        GuarateedPricePerCase = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BidPricePerCase = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BidQuantityInCase = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BidQuantityInMT = c.Decimal(nullable: false, precision: 18, scale: 4),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ChanceNumber = c.Long(nullable: false),
                        TotalChance = c.Long(nullable: false),
                        StatusId = c.Long(nullable: false),
                        SchemeDiscount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        SchemeDiscountCase = c.Decimal(nullable: false, precision: 18, scale: 4),
                        SchemeDiscountType = c.Int(nullable: false),
                        VolumeDiscount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        VolumeDiscountCase = c.Decimal(nullable: false, precision: 18, scale: 4),
                        VolumeDiscountType = c.Int(nullable: false),
                        SkuDiscount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        SkuDiscountCase = c.Decimal(nullable: false, precision: 18, scale: 4),
                        SkuDiscountType = c.Int(nullable: false),
                        SaudaBiddingCartHeaderId = c.Long(nullable: false),
                        BaseRate = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CounterBidOffer = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CounterBidStatusId = c.Long(nullable: false),
                        IsSaudaAllocated = c.Boolean(nullable: false),
                        ValidFromDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidToDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        GPBenefitType = c.Int(nullable: false),
                        GPBenefitOrCategoryId = c.Long(nullable: false),
                        GPBenefitAppliedTypeId = c.Long(nullable: false),
                        GPBenefitDiscountInCase = c.Decimal(nullable: false, precision: 18, scale: 4),
                        GPBenefitDiscountOrDay = c.Decimal(nullable: false, precision: 18, scale: 4),
                        PricingId = c.Long(nullable: false),
                        BidPrice = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BaseBidQuantityInCase = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CounterBidPrice = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BiddingWindows", t => t.BiddingWindowId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.DealerId, cascadeDelete: true)
                .ForeignKey("dbo.IncoTerms", t => t.IncotermId, cascadeDelete: true)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId, cascadeDelete: true)
                .ForeignKey("dbo.SaudaBiddingCartHeaders", t => t.SaudaBiddingCartHeaderId)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .Index(t => t.BiddingWindowId)
                .Index(t => t.DealerId)
                .Index(t => t.IncotermId)
                .Index(t => t.OilTypeId)
                .Index(t => t.SkuId)
                .Index(t => t.SaudaBiddingCartHeaderId);
            
            CreateTable(
                "dbo.SaudaBiddingCartHeaders",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BiddingWindowId = c.Long(nullable: false),
                        DealerId = c.Long(nullable: false),
                        BiddingDateAndTime = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BiddingWindows", t => t.BiddingWindowId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.DealerId, cascadeDelete: true)
                .Index(t => t.BiddingWindowId)
                .Index(t => t.DealerId);
            
            CreateTable(
                "dbo.SaudaConversions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SaudaOrderId = c.Long(nullable: false),
                        DealerId = c.Long(nullable: false),
                        ExpiryDate = c.DateTime(),
                        ExtendToDate = c.DateTime(),
                        StatusId = c.Long(),
                        ExtensionStatusId = c.Long(),
                        IsConversion = c.Boolean(nullable: false),
                        IsExtension = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.DealerId, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.ExtensionStatusId)
                .ForeignKey("dbo.SaudaOrders", t => t.SaudaOrderId)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.SaudaOrderId)
                .Index(t => t.DealerId)
                .Index(t => t.StatusId)
                .Index(t => t.ExtensionStatusId);
            
            CreateTable(
                "dbo.SaudaOrders",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SaudaId = c.Long(nullable: false),
                        SkuId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        QuotedPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BidQuantity = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BidQuantityCase = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BidPriceBeforeDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BidPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BidPricePerCase = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SpecialRateRequestId = c.Long(nullable: false),
                        SaudaNumber = c.String(),
                        DiscountTypeId = c.Long(nullable: false),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StatusId = c.Int(nullable: false),
                        PricingId = c.Long(nullable: false),
                        Remarks = c.String(),
                        Incoterms1 = c.String(),
                        Incoterms2 = c.Long(nullable: false),
                        PlantId = c.Long(nullable: false),
                        DealerLocationId = c.Long(nullable: false),
                        BrokerId = c.Long(nullable: false),
                        SaudaBookingTypeId = c.Long(nullable: false),
                        IsSAPDataSyncApproval = c.Boolean(nullable: false),
                        IsSAPDataSync = c.Boolean(nullable: false),
                        ValidFromDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidToDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UomId = c.Long(nullable: false),
                        SaudaReleaseDate = c.DateTime(),
                        BaseRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsSapSauda = c.Boolean(nullable: false),
                        IsBaseSauda = c.Boolean(nullable: false),
                        BaseSaudaOrderId = c.Long(nullable: false),
                        IsLooseVerticalForAcceptedStatus = c.Boolean(nullable: false),
                        IsQuantityLimitForBookingSauda = c.Boolean(nullable: false),
                        BaseSkuBidPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsSaudaApprovalSyncConfirmation = c.Boolean(nullable: false),
                        IsSaudaApprovalStatusFromSap = c.Boolean(nullable: false),
                        IsSapSaudaNumberUpdateSync = c.Boolean(nullable: false),
                        SalesOrganizationId = c.Long(nullable: false),
                        DistributionChannelId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        SalesOrderQuantityCase = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InvoiceQuantityCase = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrderQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InvoiceQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DistributionChannels", t => t.DistributionChannelId, cascadeDelete: true)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId)
                .ForeignKey("dbo.SalesOrganizations", t => t.SalesOrganizationId, cascadeDelete: true)
                .ForeignKey("dbo.Saudas", t => t.SaudaId)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .Index(t => t.SaudaId)
                .Index(t => t.SkuId)
                .Index(t => t.OilTypeId)
                .Index(t => t.SalesOrganizationId)
                .Index(t => t.DistributionChannelId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "dbo.SaudaConversionOrders",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SaudaConversionId = c.Long(nullable: false),
                        SaudaId = c.Long(nullable: false),
                        SkuId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        QuotedPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BidQuantity = c.Decimal(nullable: false, precision: 18, scale: 4),
                        BidQuantityCase = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BidPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TradeTicketNumber = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId, cascadeDelete: true)
                .ForeignKey("dbo.SaudaConversions", t => t.SaudaConversionId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .Index(t => t.SaudaConversionId)
                .Index(t => t.SkuId)
                .Index(t => t.OilTypeId);
            
            CreateTable(
                "dbo.SaudaConversionSkuDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SaudaConversionSkuId = c.Long(nullable: false),
                        SaudaConversionUnitAndDifferenceRateDetailsId = c.Long(nullable: false),
                        ToSkuId = c.Long(nullable: false),
                        ToQuantityInSku = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ToQuantityInMt = c.Decimal(nullable: false, precision: 18, scale: 3),
                        ToSaudaOrderId = c.Long(),
                        ToSaudaNumber = c.String(),
                        ToBaseRate = c.Decimal(precision: 18, scale: 2),
                        TradeTicketNumber = c.String(),
                        Remarks = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SaudaConversionSkus", t => t.SaudaConversionSkuId, cascadeDelete: true)
                .Index(t => t.SaudaConversionSkuId);
            
            CreateTable(
                "dbo.SaudaConversionSkus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SkuId = c.Long(nullable: false),
                        QuantityInSku = c.Decimal(nullable: false, precision: 18, scale: 2),
                        QuantityInMt = c.Decimal(nullable: false, precision: 18, scale: 3),
                        OilTypeId = c.Long(nullable: false),
                        DealerId = c.Long(nullable: false),
                        PlantId = c.Long(nullable: false),
                        DepotId = c.Long(nullable: false),
                        SaudaOrderId = c.Long(),
                        SaudaNumber = c.String(),
                        SaudaConversionSkuHeaderId = c.Long(),
                        TradeTicketNumber = c.String(),
                        Remarks = c.String(maxLength: 1000),
                        BaseRate = c.Decimal(precision: 18, scale: 2),
                        IsSAPDataSync = c.Boolean(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        IsNotSyncToSAP = c.Boolean(nullable: false),
                        SaudaConversionUpdateFromSap = c.Boolean(nullable: false),
                        StatusId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SaudaConversionTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SaudaConversionUnitAndDifferenceRateDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SaudaConversionUnitAndDifferenceRateId = c.Long(nullable: false),
                        ToPackGroupId = c.Long(nullable: false),
                        ToSkuId = c.Long(nullable: false),
                        ToUnit = c.Decimal(nullable: false, precision: 18, scale: 3),
                        BasicRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SaudaConversionUnitAndDifferenceRates", t => t.SaudaConversionUnitAndDifferenceRateId, cascadeDelete: true)
                .Index(t => t.SaudaConversionUnitAndDifferenceRateId);
            
            CreateTable(
                "dbo.SaudaConversionUnitAndDifferenceRates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FromPackGroupId = c.Long(nullable: false),
                        FromSkuId = c.Long(nullable: false),
                        FromUnit = c.Decimal(nullable: false, precision: 18, scale: 3),
                        FromDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ToDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        SourceId = c.Long(nullable: false),
                        StateId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SaudaExtensions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OilTypeId = c.Long(nullable: false),
                        StateId = c.Int(nullable: false),
                        ExtensionDays = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        ValidFrom = c.DateTime(nullable: false),
                        ValidTo = c.DateTime(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId, cascadeDelete: true)
                .ForeignKey("dbo.States", t => t.StateId, cascadeDelete: true)
                .Index(t => t.OilTypeId)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.SaudaExtensionDetailsApprovals",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SaudaOrderId = c.Long(nullable: false),
                        PendingContractId = c.Long(nullable: false),
                        SaudaNumber = c.String(),
                        RequestDate = c.String(),
                        ExtentionDateCount = c.String(),
                        IsApproval = c.Boolean(nullable: false),
                        SaudaValidFrom = c.DateTime(nullable: false),
                        SaudaValidTo = c.DateTime(nullable: false),
                        BasicRate = c.Decimal(nullable: false, precision: 18, scale: 3),
                        SaudaQuantityMT = c.Decimal(nullable: false, precision: 18, scale: 3),
                        PendingQuantityCase = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PendingQuantityMT = c.Decimal(nullable: false, precision: 18, scale: 3),
                        SaudaQuantityCase = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SkuCode = c.String(),
                        UserCode = c.String(),
                        SAPRemarks = c.String(),
                        Remarks = c.String(),
                        IsSAPDataSync = c.Boolean(nullable: false),
                        SaudaRequestDate = c.DateTime(nullable: false),
                        SaudaExtensionUpdateFromSap = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SaudaLimits",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        UserCode = c.String(),
                        ActualLimit = c.Decimal(nullable: false, precision: 18, scale: 4),
                        RequestedLimit = c.Decimal(nullable: false, precision: 18, scale: 4),
                        StatusId = c.Long(nullable: false),
                        Remarks = c.String(),
                        IsSAPData = c.Boolean(nullable: false),
                        IsSAPDataSyncOrNot = c.Boolean(nullable: false),
                        PendingContract = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PendingDO = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PendingOBD = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Division = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        LimitQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UOM = c.String(),
                        TargetValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Currency = c.String(),
                        EndDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        OldQty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OldValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrganizationId = c.Long(nullable: false),
                        DistributionChannelId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SaudaLimitHistories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        OldSaudaLimit = c.Decimal(nullable: false, precision: 10, scale: 4),
                        NewSaudaLimit = c.Decimal(nullable: false, precision: 10, scale: 4),
                        Remarks = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SaudaOrderLiftingRequestMappings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SaudaId = c.Long(nullable: false),
                        SaudaOrderId = c.Long(nullable: false),
                        DeliveryOrderNumber = c.String(),
                        LiftingQuantity = c.Decimal(nullable: false, precision: 18, scale: 4),
                        LiftingQuantityCase = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UomId = c.Int(nullable: false),
                        LiftingRequestDetailId = c.Long(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SaudaQuantityConfigurations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OilTypeId = c.Long(nullable: false),
                        PackGroupId = c.Long(nullable: false),
                        MaximumPercentageQtyIncrease = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId, cascadeDelete: true)
                .ForeignKey("dbo.PackGroups", t => t.PackGroupId, cascadeDelete: true)
                .Index(t => t.OilTypeId)
                .Index(t => t.PackGroupId);
            
            CreateTable(
                "dbo.SaudaStatus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SaudaTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SchemeCosts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DivisionId = c.Long(),
                        OilTypeId = c.Long(nullable: false),
                        ZoneId = c.Long(nullable: false),
                        StateId = c.Int(nullable: false),
                        TerritoryId = c.Int(),
                        DistrictId = c.Int(),
                        CityId = c.Int(),
                        PackGroupId = c.Long(nullable: false),
                        SkuId = c.Long(nullable: false),
                        RatePerMt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Districts", t => t.DistrictId)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId, cascadeDelete: true)
                .ForeignKey("dbo.PackGroups", t => t.PackGroupId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .ForeignKey("dbo.States", t => t.StateId, cascadeDelete: true)
                .ForeignKey("dbo.Territories", t => t.TerritoryId)
                .ForeignKey("dbo.Zones", t => t.ZoneId, cascadeDelete: true)
                .Index(t => t.DivisionId)
                .Index(t => t.OilTypeId)
                .Index(t => t.ZoneId)
                .Index(t => t.StateId)
                .Index(t => t.TerritoryId)
                .Index(t => t.DistrictId)
                .Index(t => t.CityId)
                .Index(t => t.PackGroupId)
                .Index(t => t.SkuId);
            
            CreateTable(
                "dbo.SchemeDiscountGeographies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsEdited = c.Boolean(nullable: false),
                        TargetQuantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountReason = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SchemeDiscountGeographyMappings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SchemeDiscountGeographyId = c.Long(nullable: false),
                        SkuId = c.Long(nullable: false),
                        CityId = c.Int(nullable: false),
                        CustomerId = c.Long(nullable: false),
                        CustomerGroupId = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId)
                .ForeignKey("dbo.Users", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.CustomerGroups", t => t.CustomerGroupId, cascadeDelete: true)
                .ForeignKey("dbo.SchemeDiscountGeographies", t => t.SchemeDiscountGeographyId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .Index(t => t.SchemeDiscountGeographyId)
                .Index(t => t.SkuId)
                .Index(t => t.CityId)
                .Index(t => t.CustomerId)
                .Index(t => t.CustomerGroupId);
            
            CreateTable(
                "dbo.SchemeDiscountHistories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        DiscountId = c.Long(nullable: false),
                        DiscountType = c.Long(nullable: false),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SecondaryFreights",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DepotId = c.Long(nullable: false),
                        StateId = c.Int(),
                        ZoneId = c.Long(),
                        FreightZoneId = c.Long(),
                        FreightRouteId = c.Long(),
                        TransportModeId = c.Long(nullable: false),
                        VerticalId = c.Long(nullable: false),
                        ActualFreight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesFreight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Capacity = c.Decimal(nullable: false, precision: 18, scale: 4),
                        IsActive = c.Boolean(nullable: false),
                        IsPublished = c.Boolean(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Depots", t => t.DepotId, cascadeDelete: true)
                .ForeignKey("dbo.States", t => t.StateId)
                .ForeignKey("dbo.TransportModes", t => t.TransportModeId, cascadeDelete: true)
                .ForeignKey("dbo.Divisions", t => t.VerticalId, cascadeDelete: true)
                .ForeignKey("dbo.Zones", t => t.ZoneId)
                .Index(t => t.DepotId)
                .Index(t => t.StateId)
                .Index(t => t.ZoneId)
                .Index(t => t.TransportModeId)
                .Index(t => t.VerticalId);
            
            CreateTable(
                "dbo.SkuUomMappings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SkuId = c.Long(nullable: false),
                        UomId = c.Long(nullable: false),
                        RelationUomId = c.Long(nullable: false),
                        ConversionFactor = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ConversionFactor1 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ConversionFactor2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: true)
                .Index(t => t.SkuId);
            
            CreateTable(
                "dbo.SpecalityFatDiscountGeographies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SkuId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        ActualDiscount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ZoneId = c.Long(nullable: false),
                        StateId = c.Long(nullable: false),
                        TerritoryId = c.Long(nullable: false),
                        DistrictId = c.Long(nullable: false),
                        CityId = c.Long(nullable: false),
                        SaudaBookingTypeId = c.Long(nullable: false),
                        Status = c.Boolean(nullable: false),
                        ApprovedBy = c.Long(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ParentId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId, cascadeDelete: true)
                .ForeignKey("dbo.SaudaBookingTypes", t => t.SaudaBookingTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .Index(t => t.SkuId)
                .Index(t => t.OilTypeId)
                .Index(t => t.SaudaBookingTypeId);
            
            CreateTable(
                "dbo.SpecalityFatDiscountUsers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SkuId = c.Long(nullable: false),
                        UserId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        ActualDiscount = c.Decimal(nullable: false, precision: 18, scale: 4),
                        RequestedDiscount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ApprovedBy = c.Long(nullable: false),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ParentId = c.Long(nullable: false),
                        RemainingQuantity = c.Decimal(nullable: false, precision: 18, scale: 4),
                        ParentQuantityId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        SalesOrganizationId = c.Long(nullable: false),
                        DistributionChannelId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.SkuId)
                .Index(t => t.UserId)
                .Index(t => t.OilTypeId);
            
            CreateTable(
                "dbo.SpecialRates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        SkuId = c.Long(nullable: false),
                        PricingId = c.Long(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 4),
                        QuantityCase = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FinalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SpecialPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StatusId = c.Long(nullable: false),
                        Remarks = c.String(),
                        Incoterms1 = c.String(),
                        Incoterms2 = c.Long(nullable: false),
                        DepotId = c.Long(nullable: false),
                        IsLTD = c.Boolean(nullable: false),
                        BrokerId = c.Long(nullable: false),
                        SaudaLimitExceedRemarks = c.String(),
                        SalesOrganizationId = c.Long(nullable: false),
                        DistributionChannelId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Depots", t => t.DepotId, cascadeDelete: true)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId)
                .ForeignKey("dbo.Status", t => t.StatusId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.OilTypeId)
                .Index(t => t.SkuId)
                .Index(t => t.StatusId)
                .Index(t => t.DepotId);
            
            CreateTable(
                "dbo.SpecialRateApprovals",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SpecialRateId = c.Long(nullable: false),
                        RequestedBy = c.Long(nullable: false),
                        RequestedTo = c.Long(nullable: false),
                        ApprovedBy = c.Long(nullable: false),
                        StatusId = c.Long(),
                        Remarks = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SpecialRates", t => t.SpecialRateId, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.SpecialRateId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.SpecialRatePricingHistories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SAPPricingCode = c.String(),
                        SkuId = c.Long(nullable: false),
                        SkuCode = c.String(),
                        OilTypeId = c.Long(nullable: false),
                        OilPackingTypeId = c.Long(nullable: false),
                        PlantId = c.Long(nullable: false),
                        PlantCode = c.String(),
                        DepotCode = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrganization = c.String(),
                        SalesOrganizationId = c.Long(nullable: false),
                        DistributionChannel = c.String(),
                        DistributionChannelId = c.Long(nullable: false),
                        Division = c.String(),
                        DivisionId = c.Long(nullable: false),
                        ValidFrom = c.DateTime(nullable: false),
                        ValidTo = c.DateTime(nullable: false),
                        PricingReferneceId = c.Long(nullable: false),
                        PerUnit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SpecialtyFatQuantityRequests",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SkuId = c.Long(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 4),
                        StatusId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        SpecialtyFatQuantityLimitId = c.Long(nullable: false),
                        Remarks = c.String(),
                        DivisionId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusId, cascadeDelete: true)
                .Index(t => t.SkuId)
                .Index(t => t.StatusId)
                .Index(t => t.OilTypeId);
            
            CreateTable(
                "dbo.SpecialtyFatQuantityRequestUserDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        StatusId = c.Long(nullable: false),
                        SpecialtyFatQuantityRequestId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SupportAttachments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SupportId = c.Long(nullable: false),
                        FileName = c.String(),
                        MediaPath = c.String(),
                        MediaTypeId = c.Int(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MediaTypes", t => t.MediaTypeId)
                .ForeignKey("dbo.Supports", t => t.SupportId, cascadeDelete: true)
                .Index(t => t.SupportId)
                .Index(t => t.MediaTypeId);
            
            CreateTable(
                "dbo.Supports",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        IssueTypeId = c.Int(nullable: false),
                        SeverityTypeId = c.Int(nullable: false),
                        ModuleId = c.Int(nullable: false),
                        Feature = c.String(),
                        StatusId = c.Int(nullable: false),
                        DeviceId = c.Int(nullable: false),
                        StateId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Taluks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TalukName = c.String(nullable: false, maxLength: 150),
                        TamilName = c.String(maxLength: 150),
                        SortOrder = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tickers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Content = c.String(),
                        TickerDate = c.DateTime(nullable: false),
                        FromHours = c.Time(nullable: false, precision: 7),
                        ToHours = c.Time(nullable: false, precision: 7),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TodayPricings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SAPPricingCode = c.String(),
                        SkuId = c.Long(nullable: false),
                        SkuCode = c.String(),
                        OilTypeId = c.Long(nullable: false),
                        OilPackingTypeId = c.Long(nullable: false),
                        PlantId = c.Long(nullable: false),
                        PlantCode = c.String(),
                        DepotCode = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesOrganization = c.String(),
                        SalesOrganizationId = c.Long(nullable: false),
                        DistributionChannel = c.String(),
                        DistributionChannelId = c.Long(nullable: false),
                        Division = c.String(),
                        DivisionId = c.Long(nullable: false),
                        ValidFrom = c.DateTime(nullable: false),
                        ValidTo = c.DateTime(nullable: false),
                        PricingReferneceId = c.Long(nullable: false),
                        PerUnit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TPNotifications",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SMS = c.Boolean(nullable: false),
                        Email = c.Boolean(nullable: false),
                        InAppNotification = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TPNotificationDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TPNotificationId = c.Long(nullable: false),
                        DealerId = c.Long(nullable: false),
                        NotificationActionId = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.DealerId, cascadeDelete: true)
                .ForeignKey("dbo.TPNotifications", t => t.TPNotificationId, cascadeDelete: true)
                .Index(t => t.TPNotificationId)
                .Index(t => t.DealerId);
            
            CreateTable(
                "dbo.TradeTickets",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ContractTypeId = c.Int(nullable: false),
                        MaterialTypeId = c.Int(nullable: false),
                        BookingTypeId = c.Int(nullable: false),
                        DepotId = c.Long(nullable: false),
                        UomId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        TradeTicketNumber = c.String(),
                        ContractQuantity = c.Decimal(nullable: false, precision: 18, scale: 4),
                        UnitOfMeasure = c.String(),
                        OtherElement = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ContractDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidFrom = c.DateTime(precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(precision: 7, storeType: "datetime2"),
                        IsSAPDataSync = c.Boolean(nullable: false),
                        ContractType = c.String(),
                        BookingType = c.String(),
                        MaterialType = c.String(),
                        TotalOilCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalProcessCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OpenQuantityFromSap = c.Decimal(nullable: false, precision: 18, scale: 4),
                        TTStatus = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TradeTicketDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TradeTicketId = c.Long(nullable: false),
                        TradeTicketOilTypeId = c.Long(nullable: false),
                        ProcessCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Proportion = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OilCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TradeTickets", t => t.TradeTicketId, cascadeDelete: true)
                .ForeignKey("dbo.TradeTicketOilTypes", t => t.TradeTicketOilTypeId, cascadeDelete: true)
                .Index(t => t.TradeTicketId)
                .Index(t => t.TradeTicketOilTypeId);
            
            CreateTable(
                "dbo.TradeTicketOilTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OilTypeName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        SAPId = c.String(),
                        DivisionId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserAttendances",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        LoginTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        LogoutTime = c.DateTime(precision: 7, storeType: "datetime2"),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserCreditMasters",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        SalesOrgId = c.Long(nullable: false),
                        DistChnlId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        CreditLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreditExposure = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BillingDocumentValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeliveryValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OpenOrders = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Isactive = c.Boolean(nullable: false),
                        IsSAPData = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        CreditAccountNumber = c.String(),
                        RiskCat = c.String(),
                        Curr = c.String(),
                        SalesValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalReceivable = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SaudaDepC = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SecDepH = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BankGuarM = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AdvanceA = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DueToday = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TomorrowsDue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Overdue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NotDue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NextIntRev = c.String(),
                        Blocked = c.String(),
                        TotalLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IndividLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AvailableCreditLimit = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserCustomerMappings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        CustomerId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserCustomerSalesTargets",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AssignedFromId = c.Long(),
                        AssignedToId = c.Long(),
                        Quarter = c.Int(nullable: false),
                        MonthId = c.Int(nullable: false),
                        FinancialYearId = c.Long(nullable: false),
                        Year = c.Long(nullable: false),
                        DivisionId = c.Long(),
                        OilTypeId = c.Long(nullable: false),
                        Target = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AssignedFromId)
                .ForeignKey("dbo.Users", t => t.AssignedToId)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.FinancialYears", t => t.FinancialYearId, cascadeDelete: true)
                .ForeignKey("dbo.Months", t => t.MonthId, cascadeDelete: true)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId, cascadeDelete: true)
                .Index(t => t.AssignedFromId)
                .Index(t => t.AssignedToId)
                .Index(t => t.MonthId)
                .Index(t => t.FinancialYearId)
                .Index(t => t.DivisionId)
                .Index(t => t.OilTypeId);
            
            CreateTable(
                "dbo.UserCustomerSaudaTargets",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AssignedFromId = c.Long(),
                        AssignedToId = c.Long(),
                        Quarter = c.Int(nullable: false),
                        MonthId = c.Int(nullable: false),
                        FinancialYearId = c.Long(nullable: false),
                        Year = c.Long(nullable: false),
                        DivisionId = c.Long(),
                        OilTypeId = c.Long(nullable: false),
                        Target = c.Decimal(nullable: false, precision: 18, scale: 4),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        Dealer_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AssignedFromId)
                .ForeignKey("dbo.Users", t => t.AssignedToId)
                .ForeignKey("dbo.Users", t => t.Dealer_Id)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.FinancialYears", t => t.FinancialYearId, cascadeDelete: true)
                .ForeignKey("dbo.Months", t => t.MonthId, cascadeDelete: true)
                .ForeignKey("dbo.OilTypes", t => t.OilTypeId, cascadeDelete: true)
                .Index(t => t.AssignedFromId)
                .Index(t => t.AssignedToId)
                .Index(t => t.MonthId)
                .Index(t => t.FinancialYearId)
                .Index(t => t.DivisionId)
                .Index(t => t.OilTypeId)
                .Index(t => t.Dealer_Id);
            
            CreateTable(
                "dbo.UserCustomerTargets",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AssignedFromId = c.Long(),
                        AssignedToId = c.Long(),
                        Quarter = c.Int(nullable: false),
                        MonthId = c.Int(nullable: false),
                        FinancialYearId = c.Long(nullable: false),
                        Year = c.Long(nullable: false),
                        Target = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AssignedFromId)
                .ForeignKey("dbo.Users", t => t.AssignedToId)
                .ForeignKey("dbo.FinancialYears", t => t.FinancialYearId, cascadeDelete: true)
                .ForeignKey("dbo.Months", t => t.MonthId, cascadeDelete: true)
                .Index(t => t.AssignedFromId)
                .Index(t => t.AssignedToId)
                .Index(t => t.MonthId)
                .Index(t => t.FinancialYearId);
            
            CreateTable(
                "dbo.UserDepotMappings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        DepotId = c.Long(nullable: false),
                        IsSAPData = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Depots", t => t.DepotId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.DepotId);
            
            CreateTable(
                "dbo.UserDivisionMappings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        SalesOrganizationId = c.Long(nullable: false),
                        DistributionChannelId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        SaudaLimit = c.Decimal(precision: 18, scale: 4),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DistributionChannels", t => t.DistributionChannelId, cascadeDelete: true)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.SalesOrganizations", t => t.SalesOrganizationId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.SalesOrganizationId)
                .Index(t => t.DistributionChannelId)
                .Index(t => t.DivisionId);
            
            CreateTable(
                "dbo.UserIncoTerms",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        IncoTermsId = c.Long(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserOilTypeTargets",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        RoleId = c.Long(nullable: false),
                        IsSAPData = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.UserSkuTargets",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AssignedFromId = c.Long(nullable: false),
                        AssignedToId = c.Long(nullable: false),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        SkuId = c.Long(nullable: false),
                        Quarter = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        TargetQuanity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VehicleLodabilities",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ZoneId = c.Long(nullable: false),
                        StateId = c.Int(nullable: false),
                        VehicleSize = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.States", t => t.StateId, cascadeDelete: true)
                .ForeignKey("dbo.Zones", t => t.ZoneId, cascadeDelete: true)
                .Index(t => t.ZoneId)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.VolumeLoadabilities",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SkuId = c.Long(nullable: false),
                        PlantId = c.Long(nullable: false),
                        MaxAllowableMultiplesku = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaxAllowableSinglesku = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValidFrom = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ValidTo = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        IsActive = c.Boolean(nullable: false),
                        VehicleSize = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Depots", t => t.PlantId, cascadeDelete: true)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: true)
                .Index(t => t.SkuId)
                .Index(t => t.PlantId);
            
            CreateTable(
                "dbo.WholesellerBdoes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DealerId = c.Long(nullable: false),
                        Name = c.String(),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WholeSellerSalesDetails",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        WholesellerBdoId = c.Long(nullable: false),
                        SkuId = c.Long(nullable: false),
                        OilTypeId = c.Long(nullable: false),
                        QuantityPerMt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Skus", t => t.SkuId, cascadeDelete: true)
                .ForeignKey("dbo.WholesellerBdoes", t => t.WholesellerBdoId, cascadeDelete: true)
                .Index(t => t.WholesellerBdoId)
                .Index(t => t.SkuId);
            
            CreateTable(
                "dbo.ZoneStateMappings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ZoneId = c.Long(nullable: false),
                        StateId = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(),
                        ModifiedDate = c.DateTime(precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.States", t => t.StateId, cascadeDelete: true)
                .ForeignKey("dbo.Zones", t => t.ZoneId, cascadeDelete: true)
                .Index(t => t.ZoneId)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.MaterialTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 30),
                        SalesOrganizationId = c.Long(nullable: false),
                        DistributionChannelId = c.Long(nullable: false),
                        DivisionId = c.Long(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.Long(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DistributionChannels", t => t.DistributionChannelId, cascadeDelete: true)
                .ForeignKey("dbo.Divisions", t => t.DivisionId)
                .ForeignKey("dbo.SalesOrganizations", t => t.SalesOrganizationId, cascadeDelete: true)
                .Index(t => t.SalesOrganizationId)
                .Index(t => t.DistributionChannelId)
                .Index(t => t.DivisionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MaterialTypes", "SalesOrganizationId", "dbo.SalesOrganizations");
            DropForeignKey("dbo.MaterialTypes", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.MaterialTypes", "DistributionChannelId", "dbo.DistributionChannels");
            DropForeignKey("dbo.ZoneStateMappings", "ZoneId", "dbo.Zones");
            DropForeignKey("dbo.ZoneStateMappings", "StateId", "dbo.States");
            DropForeignKey("dbo.WholeSellerSalesDetails", "WholesellerBdoId", "dbo.WholesellerBdoes");
            DropForeignKey("dbo.WholeSellerSalesDetails", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.VolumeLoadabilities", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.VolumeLoadabilities", "PlantId", "dbo.Depots");
            DropForeignKey("dbo.VehicleLodabilities", "ZoneId", "dbo.Zones");
            DropForeignKey("dbo.VehicleLodabilities", "StateId", "dbo.States");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.UserDivisionMappings", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserDivisionMappings", "SalesOrganizationId", "dbo.SalesOrganizations");
            DropForeignKey("dbo.UserDivisionMappings", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.UserDivisionMappings", "DistributionChannelId", "dbo.DistributionChannels");
            DropForeignKey("dbo.UserDepotMappings", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserDepotMappings", "DepotId", "dbo.Depots");
            DropForeignKey("dbo.UserCustomerTargets", "MonthId", "dbo.Months");
            DropForeignKey("dbo.UserCustomerTargets", "FinancialYearId", "dbo.FinancialYears");
            DropForeignKey("dbo.UserCustomerTargets", "AssignedToId", "dbo.Users");
            DropForeignKey("dbo.UserCustomerTargets", "AssignedFromId", "dbo.Users");
            DropForeignKey("dbo.UserCustomerSaudaTargets", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.UserCustomerSaudaTargets", "MonthId", "dbo.Months");
            DropForeignKey("dbo.UserCustomerSaudaTargets", "FinancialYearId", "dbo.FinancialYears");
            DropForeignKey("dbo.UserCustomerSaudaTargets", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.UserCustomerSaudaTargets", "Dealer_Id", "dbo.Users");
            DropForeignKey("dbo.UserCustomerSaudaTargets", "AssignedToId", "dbo.Users");
            DropForeignKey("dbo.UserCustomerSaudaTargets", "AssignedFromId", "dbo.Users");
            DropForeignKey("dbo.UserCustomerSalesTargets", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.UserCustomerSalesTargets", "MonthId", "dbo.Months");
            DropForeignKey("dbo.UserCustomerSalesTargets", "FinancialYearId", "dbo.FinancialYears");
            DropForeignKey("dbo.UserCustomerSalesTargets", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.UserCustomerSalesTargets", "AssignedToId", "dbo.Users");
            DropForeignKey("dbo.UserCustomerSalesTargets", "AssignedFromId", "dbo.Users");
            DropForeignKey("dbo.UserCustomerMappings", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserCreditMasters", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserAttendances", "UserId", "dbo.Users");
            DropForeignKey("dbo.TradeTicketDetails", "TradeTicketOilTypeId", "dbo.TradeTicketOilTypes");
            DropForeignKey("dbo.TradeTicketDetails", "TradeTicketId", "dbo.TradeTickets");
            DropForeignKey("dbo.TPNotificationDetails", "TPNotificationId", "dbo.TPNotifications");
            DropForeignKey("dbo.TPNotificationDetails", "DealerId", "dbo.Users");
            DropForeignKey("dbo.SupportAttachments", "SupportId", "dbo.Supports");
            DropForeignKey("dbo.SupportAttachments", "MediaTypeId", "dbo.MediaTypes");
            DropForeignKey("dbo.SpecialtyFatQuantityRequestUserDetails", "UserId", "dbo.Users");
            DropForeignKey("dbo.SpecialtyFatQuantityRequests", "StatusId", "dbo.Status");
            DropForeignKey("dbo.SpecialtyFatQuantityRequests", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.SpecialtyFatQuantityRequests", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.SpecialRateApprovals", "StatusId", "dbo.Status");
            DropForeignKey("dbo.SpecialRateApprovals", "SpecialRateId", "dbo.SpecialRates");
            DropForeignKey("dbo.SpecialRates", "UserId", "dbo.Users");
            DropForeignKey("dbo.SpecialRates", "StatusId", "dbo.Status");
            DropForeignKey("dbo.SpecialRates", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.SpecialRates", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.SpecialRates", "DepotId", "dbo.Depots");
            DropForeignKey("dbo.SpecalityFatDiscountUsers", "UserId", "dbo.Users");
            DropForeignKey("dbo.SpecalityFatDiscountUsers", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.SpecalityFatDiscountUsers", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.SpecalityFatDiscountGeographies", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.SpecalityFatDiscountGeographies", "SaudaBookingTypeId", "dbo.SaudaBookingTypes");
            DropForeignKey("dbo.SpecalityFatDiscountGeographies", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.SkuUomMappings", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.SecondaryFreights", "ZoneId", "dbo.Zones");
            DropForeignKey("dbo.SecondaryFreights", "VerticalId", "dbo.Divisions");
            DropForeignKey("dbo.SecondaryFreights", "TransportModeId", "dbo.TransportModes");
            DropForeignKey("dbo.SecondaryFreights", "StateId", "dbo.States");
            DropForeignKey("dbo.SecondaryFreights", "DepotId", "dbo.Depots");
            DropForeignKey("dbo.SchemeDiscountGeographyMappings", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.SchemeDiscountGeographyMappings", "SchemeDiscountGeographyId", "dbo.SchemeDiscountGeographies");
            DropForeignKey("dbo.SchemeDiscountGeographyMappings", "CustomerGroupId", "dbo.CustomerGroups");
            DropForeignKey("dbo.SchemeDiscountGeographyMappings", "CustomerId", "dbo.Users");
            DropForeignKey("dbo.SchemeDiscountGeographyMappings", "CityId", "dbo.Cities");
            DropForeignKey("dbo.SchemeCosts", "ZoneId", "dbo.Zones");
            DropForeignKey("dbo.SchemeCosts", "TerritoryId", "dbo.Territories");
            DropForeignKey("dbo.SchemeCosts", "StateId", "dbo.States");
            DropForeignKey("dbo.SchemeCosts", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.SchemeCosts", "PackGroupId", "dbo.PackGroups");
            DropForeignKey("dbo.SchemeCosts", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.SchemeCosts", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.SchemeCosts", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.SchemeCosts", "CityId", "dbo.Cities");
            DropForeignKey("dbo.SaudaQuantityConfigurations", "PackGroupId", "dbo.PackGroups");
            DropForeignKey("dbo.SaudaQuantityConfigurations", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.SaudaLimitHistories", "UserId", "dbo.Users");
            DropForeignKey("dbo.SaudaLimits", "UserId", "dbo.Users");
            DropForeignKey("dbo.SaudaExtensions", "StateId", "dbo.States");
            DropForeignKey("dbo.SaudaExtensions", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.SaudaConversionUnitAndDifferenceRateDetails", "SaudaConversionUnitAndDifferenceRateId", "dbo.SaudaConversionUnitAndDifferenceRates");
            DropForeignKey("dbo.SaudaConversionSkuDetails", "SaudaConversionSkuId", "dbo.SaudaConversionSkus");
            DropForeignKey("dbo.SaudaConversionOrders", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.SaudaConversionOrders", "SaudaConversionId", "dbo.SaudaConversions");
            DropForeignKey("dbo.SaudaConversionOrders", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.SaudaConversions", "StatusId", "dbo.Status");
            DropForeignKey("dbo.SaudaConversions", "SaudaOrderId", "dbo.SaudaOrders");
            DropForeignKey("dbo.SaudaOrders", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.SaudaOrders", "SaudaId", "dbo.Saudas");
            DropForeignKey("dbo.SaudaOrders", "SalesOrganizationId", "dbo.SalesOrganizations");
            DropForeignKey("dbo.SaudaOrders", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.SaudaOrders", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.SaudaOrders", "DistributionChannelId", "dbo.DistributionChannels");
            DropForeignKey("dbo.SaudaConversions", "ExtensionStatusId", "dbo.Status");
            DropForeignKey("dbo.SaudaConversions", "DealerId", "dbo.Users");
            DropForeignKey("dbo.SaudaBiddingCarts", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.SaudaBiddingCarts", "SaudaBiddingCartHeaderId", "dbo.SaudaBiddingCartHeaders");
            DropForeignKey("dbo.SaudaBiddingCartHeaders", "DealerId", "dbo.Users");
            DropForeignKey("dbo.SaudaBiddingCartHeaders", "BiddingWindowId", "dbo.BiddingWindows");
            DropForeignKey("dbo.SaudaBiddingCarts", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.SaudaBiddingCarts", "IncotermId", "dbo.IncoTerms");
            DropForeignKey("dbo.SaudaBiddingCarts", "DealerId", "dbo.Users");
            DropForeignKey("dbo.SaudaBiddingCarts", "BiddingWindowId", "dbo.BiddingWindows");
            DropForeignKey("dbo.SaudaAudioFileMappings", "AudioFileDetailsForActiveCustomersId", "dbo.AudioFileDetailsForActiveCustomers");
            DropForeignKey("dbo.SaudaApprovals", "StatusId", "dbo.Status");
            DropForeignKey("dbo.SaudaApprovals", "SaudaId", "dbo.Saudas");
            DropForeignKey("dbo.Saudas", "SaudaBookingTypeId", "dbo.SaudaBookingTypes");
            DropForeignKey("dbo.Saudas", "SalesOrganizationId", "dbo.SalesOrganizations");
            DropForeignKey("dbo.Saudas", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.Saudas", "DistributionChannelId", "dbo.DistributionChannels");
            DropForeignKey("dbo.SalesTourPlanPcpHistories", "TerritoryId", "dbo.Territories");
            DropForeignKey("dbo.SalesTourPlanPcpHistories", "StateId", "dbo.States");
            DropForeignKey("dbo.SalesTourPlanPcpHistories", "FinancialYearId", "dbo.FinancialYears");
            DropForeignKey("dbo.SalesTourPlanPcpHistories", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.SalesTourPlanPcpHistories", "CityId", "dbo.Cities");
            DropForeignKey("dbo.SalesTourPlanMtpHistories", "HeadquartersId", "dbo.Headquarters");
            DropForeignKey("dbo.SalesTourPlanMtpHistories", "CityId", "dbo.Cities");
            DropForeignKey("dbo.RoleTypeClaims", "RoleHierarchy_Id", "dbo.RoleHierarchies");
            DropForeignKey("dbo.Roles", "RoleHierarchy_Id", "dbo.RoleHierarchies");
            DropForeignKey("dbo.RoleHierarchies", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RoleDiscounts", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.RoleDiscounts", "SaudaBookingTypeId", "dbo.SaudaBookingTypes");
            DropForeignKey("dbo.RoleDiscounts", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RoleDiscounts", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.RaNotificationDetails", "RaNotificationId", "dbo.RaNotifications");
            DropForeignKey("dbo.RaNotificationDetails", "DealerId", "dbo.Users");
            DropForeignKey("dbo.RaNotificationDetails", "CustomerGroupId", "dbo.CustomerGroups");
            DropForeignKey("dbo.RAMaterialCosts", "PlantId", "dbo.Depots");
            DropForeignKey("dbo.RAMaterialCosts", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.RAMaterialCosts", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.RaMargins", "ZoneId", "dbo.Zones");
            DropForeignKey("dbo.RaMargins", "TerritoryId", "dbo.Territories");
            DropForeignKey("dbo.RaMargins", "StateId", "dbo.States");
            DropForeignKey("dbo.RaMargins", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.RaMargins", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.RaMargins", "OilPackingTypeId", "dbo.PackGroups");
            DropForeignKey("dbo.RaMargins", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.RaMargins", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.RaMargins", "CityId", "dbo.Cities");
            DropForeignKey("dbo.ProfitMargins", "ZoneId", "dbo.Zones");
            DropForeignKey("dbo.ProfitMargins", "TerritoryId", "dbo.Territories");
            DropForeignKey("dbo.ProfitMargins", "StateId", "dbo.States");
            DropForeignKey("dbo.ProfitMargins", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.ProfitMargins", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.ProfitMargins", "OilPackingTypeId", "dbo.PackGroups");
            DropForeignKey("dbo.ProfitMargins", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.ProfitMargins", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.ProfitMargins", "CityId", "dbo.Cities");
            DropForeignKey("dbo.PrimaryFreights", "VerticalId", "dbo.Divisions");
            DropForeignKey("dbo.PrimaryFreights", "TransportModeId", "dbo.TransportModes");
            DropForeignKey("dbo.PrimaryFreights", "PlantId", "dbo.Depots");
            DropForeignKey("dbo.PrimaryFreights", "DepotId", "dbo.Depots");
            DropForeignKey("dbo.PrimaryDiscountSkus", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.PrimaryDiscountSkus", "SaudaBookingTypeId", "dbo.SaudaBookingTypes");
            DropForeignKey("dbo.Pricings", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.PricePublishes", "SaudaBookingTypeId", "dbo.SaudaBookingTypes");
            DropForeignKey("dbo.PricePublishes", "PlantId", "dbo.Depots");
            DropForeignKey("dbo.PriceGenerates", "VerticalId", "dbo.Divisions");
            DropForeignKey("dbo.PriceGenerates", "SaudaBookingTypeId", "dbo.SaudaBookingTypes");
            DropForeignKey("dbo.PriceGenerateDetails", "PriceGenerateId", "dbo.PriceGenerates");
            DropForeignKey("dbo.PriceGenerateDetails", "PlantId", "dbo.Depots");
            DropForeignKey("dbo.PremiumUsers", "UserId", "dbo.Users");
            DropForeignKey("dbo.PremiumUsers", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.PremiumUsers", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.PremiumGeographies", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.PremiumGeographies", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.PremiumDiscounts", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.PremiumDiscounts", "SaudaBookingTypeId", "dbo.SaudaBookingTypes");
            DropForeignKey("dbo.PremiumDiscounts", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.PremiumDiscounts", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.PermanentJourneyPlans", "FinancialYearId", "dbo.FinancialYears");
            DropForeignKey("dbo.PermanentJourneyPlans", "PermanentJourneyPlanStatusId", "dbo.PermanentJourneyPlanStatus");
            DropForeignKey("dbo.PermanentJourneyPlanDetails", "PermanentJourneyPlanId", "dbo.PermanentJourneyPlans");
            DropForeignKey("dbo.PermanentJourneyPlanApprovalInformations", "PermanentJourneyPlanId", "dbo.PermanentJourneyPlans");
            DropForeignKey("dbo.PercentileNumberDetails", "PercentileNumberId", "dbo.PercentileNumbers");
            DropForeignKey("dbo.PercentileNumberDetails", "PackGroupId", "dbo.PackGroups");
            DropForeignKey("dbo.PercentileNumberDetails", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.PercentileNumbers", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.PackingCosts", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.PackingCosts", "PlantId", "dbo.Depots");
            DropForeignKey("dbo.PackingCosts", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.PackingCosts", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.OilTransferCosts", "SourceId", "dbo.Depots");
            DropForeignKey("dbo.OilTransferCosts", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.OilTransferCosts", "SalesOrganizationId", "dbo.SalesOrganizations");
            DropForeignKey("dbo.OilTransferCosts", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.OilTransferCosts", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.OilTransferCosts", "DistributionChannelId", "dbo.DistributionChannels");
            DropForeignKey("dbo.OilTransferCosts", "DestinationId", "dbo.Depots");
            DropForeignKey("dbo.MonthlyPlanDeviations", "MonthlyTourPlanDetailsId", "dbo.MonthlyTourPlanDetails");
            DropForeignKey("dbo.MonthlyTourPlanDetails", "TownId", "dbo.Cities");
            DropForeignKey("dbo.MonthlyTourPlanDetails", "MonthlyTourPlanId", "dbo.MonthlyTourPlans");
            DropForeignKey("dbo.MonthlyTourPlanApprovalInformations", "MonthlyTourPlanId", "dbo.MonthlyTourPlans");
            DropForeignKey("dbo.MonthlyTourPlans", "MonthlyTourPlanStatusId", "dbo.MonthlyTourPlanStatus");
            DropForeignKey("dbo.MonthlyTourPlanDetails", "HeadquartersId", "dbo.Headquarters");
            DropForeignKey("dbo.MaterialCosts", "SalesOrganizationId", "dbo.SalesOrganizations");
            DropForeignKey("dbo.MaterialCosts", "PlantId", "dbo.Depots");
            DropForeignKey("dbo.MaterialCosts", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.MaterialCosts", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.MaterialCosts", "DistributionChannelId", "dbo.DistributionChannels");
            DropForeignKey("dbo.LoadCapacityConversions", "TransportModeId", "dbo.TransportModes");
            DropForeignKey("dbo.LoadCapacityConversions", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.LoadCapacityConversions", "SalesOrganizationId", "dbo.SalesOrganizations");
            DropForeignKey("dbo.LoadCapacityConversions", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.LoadCapacityConversions", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.LoadCapacityConversions", "DistributionChannelId", "dbo.DistributionChannels");
            DropForeignKey("dbo.LiftingRequestDetails", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.LiftingRequestDetails", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.LiftingRequestDetails", "LiftingRequestId", "dbo.LiftingRequests");
            DropForeignKey("dbo.LiftingRequests", "UserId", "dbo.Users");
            DropForeignKey("dbo.LiftingRequests", "ShipToPartyId", "dbo.Users");
            DropForeignKey("dbo.InvoiceDetails", "InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.Invoices", "UserId", "dbo.Users");
            DropForeignKey("dbo.HoneycombCosts", "ZoneId", "dbo.Zones");
            DropForeignKey("dbo.HoneycombCosts", "TransportModeId", "dbo.TransportModes");
            DropForeignKey("dbo.HoneycombCosts", "StateId", "dbo.States");
            DropForeignKey("dbo.HoneycombCosts", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.HoneycombCosts", "PlantId", "dbo.Depots");
            DropForeignKey("dbo.HoneycombCosts", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.HoneycombCosts", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.GuaranteePriceJumps", "PackGroupId", "dbo.PackGroups");
            DropForeignKey("dbo.GuaranteePriceJumps", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.GuaranteePriceJumps", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.Gsts", "SourceStateId", "dbo.States");
            DropForeignKey("dbo.Gsts", "DestinationStateId", "dbo.States");
            DropForeignKey("dbo.FillerSkuBasedOnDealers", "UserId", "dbo.Users");
            DropForeignKey("dbo.FillerSkuBasedOnDealers", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.FillerSkuBasedOnDealers", "PackTypeId", "dbo.PackTypes");
            DropForeignKey("dbo.FeedbackRequests", "UserId", "dbo.Users");
            DropForeignKey("dbo.FeedbackRequests", "FeedbackTypeId", "dbo.FeedbackTypes");
            DropForeignKey("dbo.DiscountUsers", "UserId", "dbo.Users");
            DropForeignKey("dbo.DiscountUsers", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.DiscountUsers", "SalesOrganizationId", "dbo.SalesOrganizations");
            DropForeignKey("dbo.DiscountUsers", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.DiscountUsers", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.DiscountUsers", "DistributionChannelId", "dbo.DistributionChannels");
            DropForeignKey("dbo.DiscountSkus", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.DiscountSkus", "SaudaBookingTypeId", "dbo.SaudaBookingTypes");
            DropForeignKey("dbo.DiscountSkus", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.DiscountGeographies", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.DiscountGeographies", "SalesOrganizationId", "dbo.SalesOrganizations");
            DropForeignKey("dbo.DiscountGeographies", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.DiscountGeographies", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.DiscountGeographies", "DistributionChannelId", "dbo.DistributionChannels");
            DropForeignKey("dbo.DetentionCosts", "DepotId", "dbo.Depots");
            DropForeignKey("dbo.DepotCosts", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.DepotCosts", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.DepotCosts", "OilPackingTypeId", "dbo.PackGroups");
            DropForeignKey("dbo.DepotCosts", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.DepotCosts", "DepotId", "dbo.Depots");
            DropForeignKey("dbo.DealerLocations", "UserId", "dbo.Users");
            DropForeignKey("dbo.CustomerShipToPartyMappings", "ShipToPartyId", "dbo.Users");
            DropForeignKey("dbo.CustomerShipToPartyMappings", "CustomerId", "dbo.Users");
            DropForeignKey("dbo.CustomerGroupMappings", "CustomerGroupId", "dbo.CustomerGroups");
            DropForeignKey("dbo.CushionMargins", "ZoneId", "dbo.Zones");
            DropForeignKey("dbo.CushionMargins", "TerritoryId", "dbo.Territories");
            DropForeignKey("dbo.CushionMargins", "StateId", "dbo.States");
            DropForeignKey("dbo.CushionMargins", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.CushionMargins", "SalesOrganizationId", "dbo.SalesOrganizations");
            DropForeignKey("dbo.CushionMargins", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.CushionMargins", "OilPackingTypeId", "dbo.PackGroups");
            DropForeignKey("dbo.CushionMargins", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.CushionMargins", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.CushionMargins", "DistributionChannelId", "dbo.DistributionChannels");
            DropForeignKey("dbo.CushionMargins", "CityId", "dbo.Cities");
            DropForeignKey("dbo.CreditNotes", "UserId", "dbo.Users");
            DropForeignKey("dbo.CounterBidJumps", "SalesOrganization_Id", "dbo.SalesOrganizations");
            DropForeignKey("dbo.CounterBidJumps", "PackGroupId", "dbo.PackGroups");
            DropForeignKey("dbo.CounterBidJumps", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.CounterBidJumps", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.CounterBidJumps", "DistributionChannel_Id", "dbo.DistributionChannels");
            DropForeignKey("dbo.ConversionFormulaDetails", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.ConversionFormulas", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.ConversionFormulas", "PackGroupId", "dbo.PackGroups");
            DropForeignKey("dbo.ConversionFormulas", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.ConversionFormulaDetails", "ConversionFormulaId", "dbo.ConversionFormulas");
            DropForeignKey("dbo.ConsentImageDetailsForCustomers", "MediaTypeId", "dbo.MediaTypes");
            DropForeignKey("dbo.CompetitorSkus", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.CompetitorSkus", "CompetitorId", "dbo.Competitors");
            DropForeignKey("dbo.CompetitorAnalysisDetails", "CompetitorAnalysisId", "dbo.CompetitorAnalysis");
            DropForeignKey("dbo.CompetitorAnalysisDetails", "CompetitorId", "dbo.Competitors");
            DropForeignKey("dbo.CompetitorAnalysisApprovals", "StatusId", "dbo.Status");
            DropForeignKey("dbo.CompetitorAnalysisApprovals", "CompetitorAnalysisId", "dbo.CompetitorAnalysis");
            DropForeignKey("dbo.CompetitorAnalysis", "StatusId", "dbo.Status");
            DropForeignKey("dbo.CompetitorAnalysis", "SkuId", "dbo.Skus");
            DropForeignKey("dbo.Skus", "UomId", "dbo.Uoms");
            DropForeignKey("dbo.Skus", "SubCategoryId", "dbo.SubCategories");
            DropForeignKey("dbo.Skus", "SalesOrganizationId", "dbo.SalesOrganizations");
            DropForeignKey("dbo.Skus", "PackTypeId", "dbo.PackTypes");
            DropForeignKey("dbo.Skus", "PackGroupId", "dbo.PackGroups");
            DropForeignKey("dbo.Skus", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.Skus", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.Skus", "DistributionChannelId", "dbo.DistributionChannels");
            DropForeignKey("dbo.CompetitorAnalysis", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.Competitors", "ZoneId", "dbo.Zones");
            DropForeignKey("dbo.Competitors", "StateId", "dbo.States");
            DropForeignKey("dbo.RoleTypeClaims", "RoleTypeId", "dbo.RoleTypes");
            DropForeignKey("dbo.RoleTypeClaims", "ClaimId", "dbo.Claims");
            DropForeignKey("dbo.Roles", "RoleTypeId", "dbo.RoleTypes");
            DropForeignKey("dbo.RoleClaims", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.RoleClaims", "ClaimId", "dbo.Claims");
            DropForeignKey("dbo.Bulletins", "ContentTypeId", "dbo.ContentTypes");
            DropForeignKey("dbo.BulletinMedias", "MediaTypeId", "dbo.MediaTypes");
            DropForeignKey("dbo.BulletinMedias", "BulletinId", "dbo.Bulletins");
            DropForeignKey("dbo.BiddingWindowVolumeCapacities", "BiddingWindowId", "dbo.BiddingWindows");
            DropForeignKey("dbo.BiddingWindowVolumeCapacities", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.BiddingWindowCustomerGroups", "BiddingWindowId", "dbo.BiddingWindows");
            DropForeignKey("dbo.BiddingWindowCustomerGroups", "CustomerGroupId", "dbo.CustomerGroups");
            DropForeignKey("dbo.Benefits", "BenefitTypeId", "dbo.BenefitTypes");
            DropForeignKey("dbo.BdoCompetitorSkus", "BdoCompetitorId", "dbo.BdoCompetitors");
            DropForeignKey("dbo.BaseGroupMargins", "PackGroupId", "dbo.PackGroups");
            DropForeignKey("dbo.BaseGroupMargins", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.DerivedGroupMargins", "BaseGroupMarginId", "dbo.BaseGroupMargins");
            DropForeignKey("dbo.DerivedGroupMargins", "CustomerGroupId", "dbo.CustomerGroups");
            DropForeignKey("dbo.BaseGroupMargins", "CustomerGroupId", "dbo.CustomerGroups");
            DropForeignKey("dbo.CustomerGroups", "SalesOrganizationId", "dbo.SalesOrganizations");
            DropForeignKey("dbo.CustomerGroups", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.CustomerGroups", "DistributionChannelId", "dbo.DistributionChannels");
            DropForeignKey("dbo.CustomerGroupDetails", "CustomerGroupId", "dbo.CustomerGroups");
            DropForeignKey("dbo.CustomerGroupDetails", "CustomerId", "dbo.Users");
            DropForeignKey("dbo.BaseGroupMarginStates", "BaseGroupMarginId", "dbo.BaseGroupMargins");
            DropForeignKey("dbo.AudioFileDetailsForActiveCustomers", "MediaTypeId", "dbo.MediaTypes");
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.AdditionalCosts", "PlantId", "dbo.Depots");
            DropForeignKey("dbo.AdditionalCosts", "OilTypeId", "dbo.OilTypes");
            DropForeignKey("dbo.OilTypes", "SalesOrganizationId", "dbo.SalesOrganizations");
            DropForeignKey("dbo.OilTypes", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.OilTypes", "DistributionChannelId", "dbo.DistributionChannels");
            DropForeignKey("dbo.AdditionalCosts", "DivisionId", "dbo.Divisions");
            DropForeignKey("dbo.Divisions", "SalesOrganizationId", "dbo.SalesOrganizations");
            DropForeignKey("dbo.Divisions", "DistributionChannelId", "dbo.DistributionChannels");
            DropForeignKey("dbo.DistributionChannels", "SalesOrganizationId", "dbo.SalesOrganizations");
            DropForeignKey("dbo.AccountStatements", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "ZoneId", "dbo.Zones");
            DropForeignKey("dbo.Users", "SaudaBookingTypeId", "dbo.SaudaBookingTypes");
            DropForeignKey("dbo.Users", "IncoTermsId", "dbo.IncoTerms");
            DropForeignKey("dbo.Users", "InActiveRemarkId", "dbo.DeleteListCreations");
            DropForeignKey("dbo.Users", "HeadquartersId", "dbo.Headquarters");
            DropForeignKey("dbo.Headquarters", "ZoneId", "dbo.Zones");
            DropForeignKey("dbo.Headquarters", "TerritoryId", "dbo.Territories");
            DropForeignKey("dbo.Territories", "StateId", "dbo.States");
            DropForeignKey("dbo.Headquarters", "StateId", "dbo.States");
            DropForeignKey("dbo.Headquarters", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.Headquarters", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Cities", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.Districts", "StateId", "dbo.States");
            DropForeignKey("dbo.States", "CountryId", "dbo.Countries");
            DropIndex("dbo.MaterialTypes", new[] { "DivisionId" });
            DropIndex("dbo.MaterialTypes", new[] { "DistributionChannelId" });
            DropIndex("dbo.MaterialTypes", new[] { "SalesOrganizationId" });
            DropIndex("dbo.ZoneStateMappings", new[] { "StateId" });
            DropIndex("dbo.ZoneStateMappings", new[] { "ZoneId" });
            DropIndex("dbo.WholeSellerSalesDetails", new[] { "SkuId" });
            DropIndex("dbo.WholeSellerSalesDetails", new[] { "WholesellerBdoId" });
            DropIndex("dbo.VolumeLoadabilities", new[] { "PlantId" });
            DropIndex("dbo.VolumeLoadabilities", new[] { "SkuId" });
            DropIndex("dbo.VehicleLodabilities", new[] { "StateId" });
            DropIndex("dbo.VehicleLodabilities", new[] { "ZoneId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserDivisionMappings", new[] { "DivisionId" });
            DropIndex("dbo.UserDivisionMappings", new[] { "DistributionChannelId" });
            DropIndex("dbo.UserDivisionMappings", new[] { "SalesOrganizationId" });
            DropIndex("dbo.UserDivisionMappings", new[] { "UserId" });
            DropIndex("dbo.UserDepotMappings", new[] { "DepotId" });
            DropIndex("dbo.UserDepotMappings", new[] { "UserId" });
            DropIndex("dbo.UserCustomerTargets", new[] { "FinancialYearId" });
            DropIndex("dbo.UserCustomerTargets", new[] { "MonthId" });
            DropIndex("dbo.UserCustomerTargets", new[] { "AssignedToId" });
            DropIndex("dbo.UserCustomerTargets", new[] { "AssignedFromId" });
            DropIndex("dbo.UserCustomerSaudaTargets", new[] { "Dealer_Id" });
            DropIndex("dbo.UserCustomerSaudaTargets", new[] { "OilTypeId" });
            DropIndex("dbo.UserCustomerSaudaTargets", new[] { "DivisionId" });
            DropIndex("dbo.UserCustomerSaudaTargets", new[] { "FinancialYearId" });
            DropIndex("dbo.UserCustomerSaudaTargets", new[] { "MonthId" });
            DropIndex("dbo.UserCustomerSaudaTargets", new[] { "AssignedToId" });
            DropIndex("dbo.UserCustomerSaudaTargets", new[] { "AssignedFromId" });
            DropIndex("dbo.UserCustomerSalesTargets", new[] { "OilTypeId" });
            DropIndex("dbo.UserCustomerSalesTargets", new[] { "DivisionId" });
            DropIndex("dbo.UserCustomerSalesTargets", new[] { "FinancialYearId" });
            DropIndex("dbo.UserCustomerSalesTargets", new[] { "MonthId" });
            DropIndex("dbo.UserCustomerSalesTargets", new[] { "AssignedToId" });
            DropIndex("dbo.UserCustomerSalesTargets", new[] { "AssignedFromId" });
            DropIndex("dbo.UserCustomerMappings", new[] { "UserId" });
            DropIndex("dbo.UserCreditMasters", new[] { "UserId" });
            DropIndex("dbo.UserAttendances", new[] { "UserId" });
            DropIndex("dbo.TradeTicketDetails", new[] { "TradeTicketOilTypeId" });
            DropIndex("dbo.TradeTicketDetails", new[] { "TradeTicketId" });
            DropIndex("dbo.TPNotificationDetails", new[] { "DealerId" });
            DropIndex("dbo.TPNotificationDetails", new[] { "TPNotificationId" });
            DropIndex("dbo.SupportAttachments", new[] { "MediaTypeId" });
            DropIndex("dbo.SupportAttachments", new[] { "SupportId" });
            DropIndex("dbo.SpecialtyFatQuantityRequestUserDetails", new[] { "UserId" });
            DropIndex("dbo.SpecialtyFatQuantityRequests", new[] { "OilTypeId" });
            DropIndex("dbo.SpecialtyFatQuantityRequests", new[] { "StatusId" });
            DropIndex("dbo.SpecialtyFatQuantityRequests", new[] { "SkuId" });
            DropIndex("dbo.SpecialRateApprovals", new[] { "StatusId" });
            DropIndex("dbo.SpecialRateApprovals", new[] { "SpecialRateId" });
            DropIndex("dbo.SpecialRates", new[] { "DepotId" });
            DropIndex("dbo.SpecialRates", new[] { "StatusId" });
            DropIndex("dbo.SpecialRates", new[] { "SkuId" });
            DropIndex("dbo.SpecialRates", new[] { "OilTypeId" });
            DropIndex("dbo.SpecialRates", new[] { "UserId" });
            DropIndex("dbo.SpecalityFatDiscountUsers", new[] { "OilTypeId" });
            DropIndex("dbo.SpecalityFatDiscountUsers", new[] { "UserId" });
            DropIndex("dbo.SpecalityFatDiscountUsers", new[] { "SkuId" });
            DropIndex("dbo.SpecalityFatDiscountGeographies", new[] { "SaudaBookingTypeId" });
            DropIndex("dbo.SpecalityFatDiscountGeographies", new[] { "OilTypeId" });
            DropIndex("dbo.SpecalityFatDiscountGeographies", new[] { "SkuId" });
            DropIndex("dbo.SkuUomMappings", new[] { "SkuId" });
            DropIndex("dbo.SecondaryFreights", new[] { "VerticalId" });
            DropIndex("dbo.SecondaryFreights", new[] { "TransportModeId" });
            DropIndex("dbo.SecondaryFreights", new[] { "ZoneId" });
            DropIndex("dbo.SecondaryFreights", new[] { "StateId" });
            DropIndex("dbo.SecondaryFreights", new[] { "DepotId" });
            DropIndex("dbo.SchemeDiscountGeographyMappings", new[] { "CustomerGroupId" });
            DropIndex("dbo.SchemeDiscountGeographyMappings", new[] { "CustomerId" });
            DropIndex("dbo.SchemeDiscountGeographyMappings", new[] { "CityId" });
            DropIndex("dbo.SchemeDiscountGeographyMappings", new[] { "SkuId" });
            DropIndex("dbo.SchemeDiscountGeographyMappings", new[] { "SchemeDiscountGeographyId" });
            DropIndex("dbo.SchemeCosts", new[] { "SkuId" });
            DropIndex("dbo.SchemeCosts", new[] { "PackGroupId" });
            DropIndex("dbo.SchemeCosts", new[] { "CityId" });
            DropIndex("dbo.SchemeCosts", new[] { "DistrictId" });
            DropIndex("dbo.SchemeCosts", new[] { "TerritoryId" });
            DropIndex("dbo.SchemeCosts", new[] { "StateId" });
            DropIndex("dbo.SchemeCosts", new[] { "ZoneId" });
            DropIndex("dbo.SchemeCosts", new[] { "OilTypeId" });
            DropIndex("dbo.SchemeCosts", new[] { "DivisionId" });
            DropIndex("dbo.SaudaQuantityConfigurations", new[] { "PackGroupId" });
            DropIndex("dbo.SaudaQuantityConfigurations", new[] { "OilTypeId" });
            DropIndex("dbo.SaudaLimitHistories", new[] { "UserId" });
            DropIndex("dbo.SaudaLimits", new[] { "UserId" });
            DropIndex("dbo.SaudaExtensions", new[] { "StateId" });
            DropIndex("dbo.SaudaExtensions", new[] { "OilTypeId" });
            DropIndex("dbo.SaudaConversionUnitAndDifferenceRateDetails", new[] { "SaudaConversionUnitAndDifferenceRateId" });
            DropIndex("dbo.SaudaConversionSkuDetails", new[] { "SaudaConversionSkuId" });
            DropIndex("dbo.SaudaConversionOrders", new[] { "OilTypeId" });
            DropIndex("dbo.SaudaConversionOrders", new[] { "SkuId" });
            DropIndex("dbo.SaudaConversionOrders", new[] { "SaudaConversionId" });
            DropIndex("dbo.SaudaOrders", new[] { "DivisionId" });
            DropIndex("dbo.SaudaOrders", new[] { "DistributionChannelId" });
            DropIndex("dbo.SaudaOrders", new[] { "SalesOrganizationId" });
            DropIndex("dbo.SaudaOrders", new[] { "OilTypeId" });
            DropIndex("dbo.SaudaOrders", new[] { "SkuId" });
            DropIndex("dbo.SaudaOrders", new[] { "SaudaId" });
            DropIndex("dbo.SaudaConversions", new[] { "ExtensionStatusId" });
            DropIndex("dbo.SaudaConversions", new[] { "StatusId" });
            DropIndex("dbo.SaudaConversions", new[] { "DealerId" });
            DropIndex("dbo.SaudaConversions", new[] { "SaudaOrderId" });
            DropIndex("dbo.SaudaBiddingCartHeaders", new[] { "DealerId" });
            DropIndex("dbo.SaudaBiddingCartHeaders", new[] { "BiddingWindowId" });
            DropIndex("dbo.SaudaBiddingCarts", new[] { "SaudaBiddingCartHeaderId" });
            DropIndex("dbo.SaudaBiddingCarts", new[] { "SkuId" });
            DropIndex("dbo.SaudaBiddingCarts", new[] { "OilTypeId" });
            DropIndex("dbo.SaudaBiddingCarts", new[] { "IncotermId" });
            DropIndex("dbo.SaudaBiddingCarts", new[] { "DealerId" });
            DropIndex("dbo.SaudaBiddingCarts", new[] { "BiddingWindowId" });
            DropIndex("dbo.SaudaAudioFileMappings", new[] { "AudioFileDetailsForActiveCustomersId" });
            DropIndex("dbo.SaudaApprovals", new[] { "StatusId" });
            DropIndex("dbo.SaudaApprovals", new[] { "SaudaId" });
            DropIndex("dbo.Saudas", new[] { "SaudaBookingTypeId" });
            DropIndex("dbo.Saudas", new[] { "DivisionId" });
            DropIndex("dbo.Saudas", new[] { "DistributionChannelId" });
            DropIndex("dbo.Saudas", new[] { "SalesOrganizationId" });
            DropIndex("dbo.SalesTourPlanPcpHistories", new[] { "CityId" });
            DropIndex("dbo.SalesTourPlanPcpHistories", new[] { "DistrictId" });
            DropIndex("dbo.SalesTourPlanPcpHistories", new[] { "TerritoryId" });
            DropIndex("dbo.SalesTourPlanPcpHistories", new[] { "StateId" });
            DropIndex("dbo.SalesTourPlanPcpHistories", new[] { "FinancialYearId" });
            DropIndex("dbo.SalesTourPlanMtpHistories", new[] { "HeadquartersId" });
            DropIndex("dbo.SalesTourPlanMtpHistories", new[] { "CityId" });
            DropIndex("dbo.RoleHierarchies", new[] { "RoleId" });
            DropIndex("dbo.RoleDiscounts", new[] { "SaudaBookingTypeId" });
            DropIndex("dbo.RoleDiscounts", new[] { "OilTypeId" });
            DropIndex("dbo.RoleDiscounts", new[] { "RoleId" });
            DropIndex("dbo.RoleDiscounts", new[] { "SkuId" });
            DropIndex("dbo.RaNotificationDetails", new[] { "DealerId" });
            DropIndex("dbo.RaNotificationDetails", new[] { "CustomerGroupId" });
            DropIndex("dbo.RaNotificationDetails", new[] { "RaNotificationId" });
            DropIndex("dbo.RAMaterialCosts", new[] { "OilTypeId" });
            DropIndex("dbo.RAMaterialCosts", new[] { "DivisionId" });
            DropIndex("dbo.RAMaterialCosts", new[] { "PlantId" });
            DropIndex("dbo.RaMargins", new[] { "TerritoryId" });
            DropIndex("dbo.RaMargins", new[] { "ZoneId" });
            DropIndex("dbo.RaMargins", new[] { "CityId" });
            DropIndex("dbo.RaMargins", new[] { "DistrictId" });
            DropIndex("dbo.RaMargins", new[] { "StateId" });
            DropIndex("dbo.RaMargins", new[] { "OilPackingTypeId" });
            DropIndex("dbo.RaMargins", new[] { "SkuId" });
            DropIndex("dbo.RaMargins", new[] { "OilTypeId" });
            DropIndex("dbo.RaMargins", new[] { "DivisionId" });
            DropIndex("dbo.ProfitMargins", new[] { "CityId" });
            DropIndex("dbo.ProfitMargins", new[] { "DistrictId" });
            DropIndex("dbo.ProfitMargins", new[] { "TerritoryId" });
            DropIndex("dbo.ProfitMargins", new[] { "StateId" });
            DropIndex("dbo.ProfitMargins", new[] { "ZoneId" });
            DropIndex("dbo.ProfitMargins", new[] { "OilPackingTypeId" });
            DropIndex("dbo.ProfitMargins", new[] { "SkuId" });
            DropIndex("dbo.ProfitMargins", new[] { "OilTypeId" });
            DropIndex("dbo.ProfitMargins", new[] { "DivisionId" });
            DropIndex("dbo.PrimaryFreights", new[] { "TransportModeId" });
            DropIndex("dbo.PrimaryFreights", new[] { "VerticalId" });
            DropIndex("dbo.PrimaryFreights", new[] { "DepotId" });
            DropIndex("dbo.PrimaryFreights", new[] { "PlantId" });
            DropIndex("dbo.PrimaryDiscountSkus", new[] { "SaudaBookingTypeId" });
            DropIndex("dbo.PrimaryDiscountSkus", new[] { "SkuId" });
            DropIndex("dbo.Pricings", new[] { "SkuId" });
            DropIndex("dbo.PricePublishes", new[] { "SaudaBookingTypeId" });
            DropIndex("dbo.PricePublishes", new[] { "PlantId" });
            DropIndex("dbo.PriceGenerateDetails", new[] { "PlantId" });
            DropIndex("dbo.PriceGenerateDetails", new[] { "PriceGenerateId" });
            DropIndex("dbo.PriceGenerates", new[] { "VerticalId" });
            DropIndex("dbo.PriceGenerates", new[] { "SaudaBookingTypeId" });
            DropIndex("dbo.PremiumUsers", new[] { "OilTypeId" });
            DropIndex("dbo.PremiumUsers", new[] { "UserId" });
            DropIndex("dbo.PremiumUsers", new[] { "SkuId" });
            DropIndex("dbo.PremiumGeographies", new[] { "OilTypeId" });
            DropIndex("dbo.PremiumGeographies", new[] { "SkuId" });
            DropIndex("dbo.PremiumDiscounts", new[] { "SaudaBookingTypeId" });
            DropIndex("dbo.PremiumDiscounts", new[] { "OilTypeId" });
            DropIndex("dbo.PremiumDiscounts", new[] { "RoleId" });
            DropIndex("dbo.PremiumDiscounts", new[] { "SkuId" });
            DropIndex("dbo.PermanentJourneyPlanApprovalInformations", new[] { "PermanentJourneyPlanId" });
            DropIndex("dbo.PermanentJourneyPlans", new[] { "FinancialYearId" });
            DropIndex("dbo.PermanentJourneyPlans", new[] { "PermanentJourneyPlanStatusId" });
            DropIndex("dbo.PermanentJourneyPlanDetails", new[] { "PermanentJourneyPlanId" });
            DropIndex("dbo.PercentileNumberDetails", new[] { "PercentileNumberId" });
            DropIndex("dbo.PercentileNumberDetails", new[] { "OilTypeId" });
            DropIndex("dbo.PercentileNumberDetails", new[] { "PackGroupId" });
            DropIndex("dbo.PercentileNumbers", new[] { "DivisionId" });
            DropIndex("dbo.PackingCosts", new[] { "PlantId" });
            DropIndex("dbo.PackingCosts", new[] { "SkuId" });
            DropIndex("dbo.PackingCosts", new[] { "OilTypeId" });
            DropIndex("dbo.PackingCosts", new[] { "DivisionId" });
            DropIndex("dbo.OilTransferCosts", new[] { "SkuId" });
            DropIndex("dbo.OilTransferCosts", new[] { "DivisionId" });
            DropIndex("dbo.OilTransferCosts", new[] { "DistributionChannelId" });
            DropIndex("dbo.OilTransferCosts", new[] { "SalesOrganizationId" });
            DropIndex("dbo.OilTransferCosts", new[] { "DestinationId" });
            DropIndex("dbo.OilTransferCosts", new[] { "SourceId" });
            DropIndex("dbo.OilTransferCosts", new[] { "OilTypeId" });
            DropIndex("dbo.MonthlyTourPlanApprovalInformations", new[] { "MonthlyTourPlanId" });
            DropIndex("dbo.MonthlyTourPlans", new[] { "MonthlyTourPlanStatusId" });
            DropIndex("dbo.MonthlyTourPlanDetails", new[] { "HeadquartersId" });
            DropIndex("dbo.MonthlyTourPlanDetails", new[] { "TownId" });
            DropIndex("dbo.MonthlyTourPlanDetails", new[] { "MonthlyTourPlanId" });
            DropIndex("dbo.MonthlyPlanDeviations", new[] { "MonthlyTourPlanDetailsId" });
            DropIndex("dbo.MaterialCosts", new[] { "OilTypeId" });
            DropIndex("dbo.MaterialCosts", new[] { "DivisionId" });
            DropIndex("dbo.MaterialCosts", new[] { "DistributionChannelId" });
            DropIndex("dbo.MaterialCosts", new[] { "SalesOrganizationId" });
            DropIndex("dbo.MaterialCosts", new[] { "PlantId" });
            DropIndex("dbo.LoadCapacityConversions", new[] { "TransportModeId" });
            DropIndex("dbo.LoadCapacityConversions", new[] { "SkuId" });
            DropIndex("dbo.LoadCapacityConversions", new[] { "OilTypeId" });
            DropIndex("dbo.LoadCapacityConversions", new[] { "DivisionId" });
            DropIndex("dbo.LoadCapacityConversions", new[] { "DistributionChannelId" });
            DropIndex("dbo.LoadCapacityConversions", new[] { "SalesOrganizationId" });
            DropIndex("dbo.LiftingRequestDetails", new[] { "OilTypeId" });
            DropIndex("dbo.LiftingRequestDetails", new[] { "SkuId" });
            DropIndex("dbo.LiftingRequestDetails", new[] { "LiftingRequestId" });
            DropIndex("dbo.LiftingRequests", new[] { "ShipToPartyId" });
            DropIndex("dbo.LiftingRequests", new[] { "UserId" });
            DropIndex("dbo.Invoices", new[] { "UserId" });
            DropIndex("dbo.InvoiceDetails", new[] { "InvoiceId" });
            DropIndex("dbo.HoneycombCosts", new[] { "StateId" });
            DropIndex("dbo.HoneycombCosts", new[] { "ZoneId" });
            DropIndex("dbo.HoneycombCosts", new[] { "TransportModeId" });
            DropIndex("dbo.HoneycombCosts", new[] { "SkuId" });
            DropIndex("dbo.HoneycombCosts", new[] { "OilTypeId" });
            DropIndex("dbo.HoneycombCosts", new[] { "DivisionId" });
            DropIndex("dbo.HoneycombCosts", new[] { "PlantId" });
            DropIndex("dbo.GuaranteePriceJumps", new[] { "PackGroupId" });
            DropIndex("dbo.GuaranteePriceJumps", new[] { "DivisionId" });
            DropIndex("dbo.GuaranteePriceJumps", new[] { "OilTypeId" });
            DropIndex("dbo.Gsts", new[] { "DestinationStateId" });
            DropIndex("dbo.Gsts", new[] { "SourceStateId" });
            DropIndex("dbo.FillerSkuBasedOnDealers", new[] { "UserId" });
            DropIndex("dbo.FillerSkuBasedOnDealers", new[] { "PackTypeId" });
            DropIndex("dbo.FillerSkuBasedOnDealers", new[] { "SkuId" });
            DropIndex("dbo.FeedbackRequests", new[] { "FeedbackTypeId" });
            DropIndex("dbo.FeedbackRequests", new[] { "UserId" });
            DropIndex("dbo.DiscountUsers", new[] { "UserId" });
            DropIndex("dbo.DiscountUsers", new[] { "SkuId" });
            DropIndex("dbo.DiscountUsers", new[] { "OilTypeId" });
            DropIndex("dbo.DiscountUsers", new[] { "DivisionId" });
            DropIndex("dbo.DiscountUsers", new[] { "DistributionChannelId" });
            DropIndex("dbo.DiscountUsers", new[] { "SalesOrganizationId" });
            DropIndex("dbo.DiscountSkus", new[] { "SaudaBookingTypeId" });
            DropIndex("dbo.DiscountSkus", new[] { "OilTypeId" });
            DropIndex("dbo.DiscountSkus", new[] { "SkuId" });
            DropIndex("dbo.DiscountGeographies", new[] { "SkuId" });
            DropIndex("dbo.DiscountGeographies", new[] { "OilTypeId" });
            DropIndex("dbo.DiscountGeographies", new[] { "DivisionId" });
            DropIndex("dbo.DiscountGeographies", new[] { "DistributionChannelId" });
            DropIndex("dbo.DiscountGeographies", new[] { "SalesOrganizationId" });
            DropIndex("dbo.DetentionCosts", new[] { "DepotId" });
            DropIndex("dbo.DepotCosts", new[] { "OilPackingTypeId" });
            DropIndex("dbo.DepotCosts", new[] { "OilTypeId" });
            DropIndex("dbo.DepotCosts", new[] { "SkuId" });
            DropIndex("dbo.DepotCosts", new[] { "DivisionId" });
            DropIndex("dbo.DepotCosts", new[] { "DepotId" });
            DropIndex("dbo.DealerLocations", new[] { "UserId" });
            DropIndex("dbo.CustomerShipToPartyMappings", new[] { "ShipToPartyId" });
            DropIndex("dbo.CustomerShipToPartyMappings", new[] { "CustomerId" });
            DropIndex("dbo.CustomerGroupMappings", new[] { "CustomerGroupId" });
            DropIndex("dbo.CushionMargins", new[] { "CityId" });
            DropIndex("dbo.CushionMargins", new[] { "DistrictId" });
            DropIndex("dbo.CushionMargins", new[] { "TerritoryId" });
            DropIndex("dbo.CushionMargins", new[] { "StateId" });
            DropIndex("dbo.CushionMargins", new[] { "ZoneId" });
            DropIndex("dbo.CushionMargins", new[] { "OilPackingTypeId" });
            DropIndex("dbo.CushionMargins", new[] { "OilTypeId" });
            DropIndex("dbo.CushionMargins", new[] { "DivisionId" });
            DropIndex("dbo.CushionMargins", new[] { "DistributionChannelId" });
            DropIndex("dbo.CushionMargins", new[] { "SalesOrganizationId" });
            DropIndex("dbo.CushionMargins", new[] { "SkuId" });
            DropIndex("dbo.CreditNotes", new[] { "UserId" });
            DropIndex("dbo.CounterBidJumps", new[] { "SalesOrganization_Id" });
            DropIndex("dbo.CounterBidJumps", new[] { "DistributionChannel_Id" });
            DropIndex("dbo.CounterBidJumps", new[] { "PackGroupId" });
            DropIndex("dbo.CounterBidJumps", new[] { "DivisionId" });
            DropIndex("dbo.CounterBidJumps", new[] { "OilTypeId" });
            DropIndex("dbo.ConversionFormulas", new[] { "SkuId" });
            DropIndex("dbo.ConversionFormulas", new[] { "PackGroupId" });
            DropIndex("dbo.ConversionFormulas", new[] { "OilTypeId" });
            DropIndex("dbo.ConversionFormulaDetails", new[] { "SkuId" });
            DropIndex("dbo.ConversionFormulaDetails", new[] { "ConversionFormulaId" });
            DropIndex("dbo.ConsentImageDetailsForCustomers", new[] { "MediaTypeId" });
            DropIndex("dbo.CompetitorSkus", new[] { "SkuId" });
            DropIndex("dbo.CompetitorSkus", new[] { "CompetitorId" });
            DropIndex("dbo.CompetitorAnalysisDetails", new[] { "CompetitorId" });
            DropIndex("dbo.CompetitorAnalysisDetails", new[] { "CompetitorAnalysisId" });
            DropIndex("dbo.CompetitorAnalysisApprovals", new[] { "StatusId" });
            DropIndex("dbo.CompetitorAnalysisApprovals", new[] { "CompetitorAnalysisId" });
            DropIndex("dbo.Skus", new[] { "SubCategoryId" });
            DropIndex("dbo.Skus", new[] { "UomId" });
            DropIndex("dbo.Skus", new[] { "PackGroupId" });
            DropIndex("dbo.Skus", new[] { "PackTypeId" });
            DropIndex("dbo.Skus", new[] { "OilTypeId" });
            DropIndex("dbo.Skus", new[] { "DivisionId" });
            DropIndex("dbo.Skus", new[] { "DistributionChannelId" });
            DropIndex("dbo.Skus", new[] { "SalesOrganizationId" });
            DropIndex("dbo.CompetitorAnalysis", new[] { "StatusId" });
            DropIndex("dbo.CompetitorAnalysis", new[] { "OilTypeId" });
            DropIndex("dbo.CompetitorAnalysis", new[] { "SkuId" });
            DropIndex("dbo.Competitors", new[] { "StateId" });
            DropIndex("dbo.Competitors", new[] { "ZoneId" });
            DropIndex("dbo.RoleTypeClaims", new[] { "RoleHierarchy_Id" });
            DropIndex("dbo.RoleTypeClaims", new[] { "ClaimId" });
            DropIndex("dbo.RoleTypeClaims", new[] { "RoleTypeId" });
            DropIndex("dbo.Roles", new[] { "RoleHierarchy_Id" });
            DropIndex("dbo.Roles", new[] { "RoleTypeId" });
            DropIndex("dbo.RoleClaims", new[] { "ClaimId" });
            DropIndex("dbo.RoleClaims", new[] { "RoleId" });
            DropIndex("dbo.BulletinMedias", new[] { "BulletinId" });
            DropIndex("dbo.BulletinMedias", new[] { "MediaTypeId" });
            DropIndex("dbo.Bulletins", new[] { "ContentTypeId" });
            DropIndex("dbo.BiddingWindowVolumeCapacities", new[] { "OilTypeId" });
            DropIndex("dbo.BiddingWindowVolumeCapacities", new[] { "BiddingWindowId" });
            DropIndex("dbo.BiddingWindowCustomerGroups", new[] { "CustomerGroupId" });
            DropIndex("dbo.BiddingWindowCustomerGroups", new[] { "BiddingWindowId" });
            DropIndex("dbo.Benefits", new[] { "BenefitTypeId" });
            DropIndex("dbo.BdoCompetitorSkus", new[] { "BdoCompetitorId" });
            DropIndex("dbo.DerivedGroupMargins", new[] { "CustomerGroupId" });
            DropIndex("dbo.DerivedGroupMargins", new[] { "BaseGroupMarginId" });
            DropIndex("dbo.CustomerGroupDetails", new[] { "CustomerId" });
            DropIndex("dbo.CustomerGroupDetails", new[] { "CustomerGroupId" });
            DropIndex("dbo.CustomerGroups", new[] { "DivisionId" });
            DropIndex("dbo.CustomerGroups", new[] { "DistributionChannelId" });
            DropIndex("dbo.CustomerGroups", new[] { "SalesOrganizationId" });
            DropIndex("dbo.BaseGroupMarginStates", new[] { "BaseGroupMarginId" });
            DropIndex("dbo.BaseGroupMargins", new[] { "CustomerGroupId" });
            DropIndex("dbo.BaseGroupMargins", new[] { "PackGroupId" });
            DropIndex("dbo.BaseGroupMargins", new[] { "OilTypeId" });
            DropIndex("dbo.AudioFileDetailsForActiveCustomers", new[] { "MediaTypeId" });
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            DropIndex("dbo.OilTypes", new[] { "DivisionId" });
            DropIndex("dbo.OilTypes", new[] { "DistributionChannelId" });
            DropIndex("dbo.OilTypes", new[] { "SalesOrganizationId" });
            DropIndex("dbo.DistributionChannels", new[] { "SalesOrganizationId" });
            DropIndex("dbo.Divisions", new[] { "DistributionChannelId" });
            DropIndex("dbo.Divisions", new[] { "SalesOrganizationId" });
            DropIndex("dbo.AdditionalCosts", new[] { "PlantId" });
            DropIndex("dbo.AdditionalCosts", new[] { "DivisionId" });
            DropIndex("dbo.AdditionalCosts", new[] { "OilTypeId" });
            DropIndex("dbo.Territories", new[] { "StateId" });
            DropIndex("dbo.States", new[] { "CountryId" });
            DropIndex("dbo.Districts", new[] { "StateId" });
            DropIndex("dbo.Cities", new[] { "DistrictId" });
            DropIndex("dbo.Headquarters", new[] { "CityId" });
            DropIndex("dbo.Headquarters", new[] { "DistrictId" });
            DropIndex("dbo.Headquarters", new[] { "TerritoryId" });
            DropIndex("dbo.Headquarters", new[] { "StateId" });
            DropIndex("dbo.Headquarters", new[] { "ZoneId" });
            DropIndex("dbo.Users", new[] { "InActiveRemarkId" });
            DropIndex("dbo.Users", new[] { "IncoTermsId" });
            DropIndex("dbo.Users", new[] { "SaudaBookingTypeId" });
            DropIndex("dbo.Users", new[] { "HeadquartersId" });
            DropIndex("dbo.Users", new[] { "ZoneId" });
            DropIndex("dbo.AccountStatements", new[] { "UserId" });
            DropTable("dbo.MaterialTypes");
            DropTable("dbo.ZoneStateMappings");
            DropTable("dbo.WholeSellerSalesDetails");
            DropTable("dbo.WholesellerBdoes");
            DropTable("dbo.VolumeLoadabilities");
            DropTable("dbo.VehicleLodabilities");
            DropTable("dbo.UserSkuTargets");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserOilTypeTargets");
            DropTable("dbo.UserIncoTerms");
            DropTable("dbo.UserDivisionMappings");
            DropTable("dbo.UserDepotMappings");
            DropTable("dbo.UserCustomerTargets");
            DropTable("dbo.UserCustomerSaudaTargets");
            DropTable("dbo.UserCustomerSalesTargets");
            DropTable("dbo.UserCustomerMappings");
            DropTable("dbo.UserCreditMasters");
            DropTable("dbo.UserAttendances");
            DropTable("dbo.TradeTicketOilTypes");
            DropTable("dbo.TradeTicketDetails");
            DropTable("dbo.TradeTickets");
            DropTable("dbo.TPNotificationDetails");
            DropTable("dbo.TPNotifications");
            DropTable("dbo.TodayPricings");
            DropTable("dbo.Tickers");
            DropTable("dbo.Taluks");
            DropTable("dbo.Supports");
            DropTable("dbo.SupportAttachments");
            DropTable("dbo.SpecialtyFatQuantityRequestUserDetails");
            DropTable("dbo.SpecialtyFatQuantityRequests");
            DropTable("dbo.SpecialRatePricingHistories");
            DropTable("dbo.SpecialRateApprovals");
            DropTable("dbo.SpecialRates");
            DropTable("dbo.SpecalityFatDiscountUsers");
            DropTable("dbo.SpecalityFatDiscountGeographies");
            DropTable("dbo.SkuUomMappings");
            DropTable("dbo.SecondaryFreights");
            DropTable("dbo.SchemeDiscountHistories");
            DropTable("dbo.SchemeDiscountGeographyMappings");
            DropTable("dbo.SchemeDiscountGeographies");
            DropTable("dbo.SchemeCosts");
            DropTable("dbo.SaudaTypes");
            DropTable("dbo.SaudaStatus");
            DropTable("dbo.SaudaQuantityConfigurations");
            DropTable("dbo.SaudaOrderLiftingRequestMappings");
            DropTable("dbo.SaudaLimitHistories");
            DropTable("dbo.SaudaLimits");
            DropTable("dbo.SaudaExtensionDetailsApprovals");
            DropTable("dbo.SaudaExtensions");
            DropTable("dbo.SaudaConversionUnitAndDifferenceRates");
            DropTable("dbo.SaudaConversionUnitAndDifferenceRateDetails");
            DropTable("dbo.SaudaConversionTypes");
            DropTable("dbo.SaudaConversionSkus");
            DropTable("dbo.SaudaConversionSkuDetails");
            DropTable("dbo.SaudaConversionOrders");
            DropTable("dbo.SaudaOrders");
            DropTable("dbo.SaudaConversions");
            DropTable("dbo.SaudaBiddingCartHeaders");
            DropTable("dbo.SaudaBiddingCarts");
            DropTable("dbo.SaudaAudioFileMappings");
            DropTable("dbo.SaudaApprovals");
            DropTable("dbo.Saudas");
            DropTable("dbo.SalesTourPlanPcpHistories");
            DropTable("dbo.SalesTourPlanMtpHistories");
            DropTable("dbo.SalesRegisters");
            DropTable("dbo.SalesDocumentTypes");
            DropTable("dbo.RoleHierarchies");
            DropTable("dbo.RoleDiscounts");
            DropTable("dbo.Remarks");
            DropTable("dbo.Regions");
            DropTable("dbo.Reasons");
            DropTable("dbo.RaSaudaConfigurations");
            DropTable("dbo.RaNotificationDetails");
            DropTable("dbo.RaNotifications");
            DropTable("dbo.RAMaterialCosts");
            DropTable("dbo.RaMargins");
            DropTable("dbo.QuantityTypes");
            DropTable("dbo.ProspectiveDealers");
            DropTable("dbo.ProfitMargins");
            DropTable("dbo.PrimaryFreights");
            DropTable("dbo.PrimaryDiscountSkus");
            DropTable("dbo.PricingUpdateFrequencies");
            DropTable("dbo.PricingBackups");
            DropTable("dbo.Pricings");
            DropTable("dbo.PricePublishes");
            DropTable("dbo.PriceNotifyConfigurations");
            DropTable("dbo.PriceGenerateDetails");
            DropTable("dbo.PriceGenerates");
            DropTable("dbo.PremiumUsers");
            DropTable("dbo.PremiumGeographies");
            DropTable("dbo.PremiumDiscounts");
            DropTable("dbo.PlantDepotMappings");
            DropTable("dbo.PickingPoints");
            DropTable("dbo.PermanentJourneyPlanStatus");
            DropTable("dbo.PermanentJourneyPlanApprovalInformations");
            DropTable("dbo.PermanentJourneyPlans");
            DropTable("dbo.PermanentJourneyPlanDetails");
            DropTable("dbo.PercentileNumberDetails");
            DropTable("dbo.PercentileNumbers");
            DropTable("dbo.PendingSaudaRemarks");
            DropTable("dbo.PendingContracts");
            DropTable("dbo.PackingCosts");
            DropTable("dbo.OverduePayments");
            DropTable("dbo.OilTransferCosts");
            DropTable("dbo.Notifications");
            DropTable("dbo.NotificationHistories");
            DropTable("dbo.Months");
            DropTable("dbo.MonthlyPlanDeviationStatus");
            DropTable("dbo.MonthlyTourPlanApprovalInformations");
            DropTable("dbo.MonthlyTourPlanStatus");
            DropTable("dbo.MonthlyTourPlans");
            DropTable("dbo.MonthlyTourPlanDetails");
            DropTable("dbo.MonthlyPlanDeviations");
            DropTable("dbo.MaterialCosts");
            DropTable("dbo.MarketScenarios");
            DropTable("dbo.LoadCapacityConversions");
            DropTable("dbo.LiftingRequestStatus");
            DropTable("dbo.LiftingRequestDetails");
            DropTable("dbo.LiftingRequests");
            DropTable("dbo.KeyPerformanceIndicators");
            DropTable("dbo.IssueComments");
            DropTable("dbo.Invoices");
            DropTable("dbo.InvoiceDetails");
            DropTable("dbo.TransportModes");
            DropTable("dbo.HoneycombCosts");
            DropTable("dbo.Holidays");
            DropTable("dbo.GuaranteePriceJumps");
            DropTable("dbo.Gsts");
            DropTable("dbo.GPSTrackings");
            DropTable("dbo.FinancialYears");
            DropTable("dbo.FillerSkuBasedOnDealers");
            DropTable("dbo.FeedbackTypes");
            DropTable("dbo.FeedbackRequests");
            DropTable("dbo.EmailTemplates");
            DropTable("dbo.DiscountUsers");
            DropTable("dbo.DiscountSkus");
            DropTable("dbo.DiscountGeographies");
            DropTable("dbo.DetentionCosts");
            DropTable("dbo.DepotCosts");
            DropTable("dbo.DeliveryPriorities");
            DropTable("dbo.DealerLocations");
            DropTable("dbo.DayOfWeekNames");
            DropTable("dbo.DateRanges");
            DropTable("dbo.CustomerTruckCapacityMappings");
            DropTable("dbo.CustomerShipToPartyMappings");
            DropTable("dbo.CustomerLedgers");
            DropTable("dbo.CustomerLedgerDetails");
            DropTable("dbo.CustomerGroupMappings");
            DropTable("dbo.CustomerGroupFives");
            DropTable("dbo.CushionMargins");
            DropTable("dbo.CreditNotes");
            DropTable("dbo.CounterBidNotifications");
            DropTable("dbo.CounterBidJumps");
            DropTable("dbo.ConversionFormulas");
            DropTable("dbo.ConversionFormulaDetails");
            DropTable("dbo.ContractTypes");
            DropTable("dbo.ConsentImageDetailsForCustomers");
            DropTable("dbo.Configurations");
            DropTable("dbo.ConfigurationForDivisionsAndEmails");
            DropTable("dbo.CompetitorSkus");
            DropTable("dbo.CompetitorAnalysisDetails");
            DropTable("dbo.CompetitorAnalysisApprovals");
            DropTable("dbo.Uoms");
            DropTable("dbo.SubCategories");
            DropTable("dbo.PackTypes");
            DropTable("dbo.Skus");
            DropTable("dbo.CompetitorAnalysis");
            DropTable("dbo.Competitors");
            DropTable("dbo.RoleTypeClaims");
            DropTable("dbo.RoleTypes");
            DropTable("dbo.Roles");
            DropTable("dbo.RoleClaims");
            DropTable("dbo.Claims");
            DropTable("dbo.ChequeInventoryDetails");
            DropTable("dbo.ContentTypes");
            DropTable("dbo.BulletinMedias");
            DropTable("dbo.Bulletins");
            DropTable("dbo.BookingTypes");
            DropTable("dbo.BiddingWindowTimings");
            DropTable("dbo.BiddingWindowStatus");
            DropTable("dbo.BiddingWindowNotificationTimings");
            DropTable("dbo.BiddingWindowVolumeCapacities");
            DropTable("dbo.BiddingWindowCustomerGroups");
            DropTable("dbo.BiddingWindows");
            DropTable("dbo.BenefitTypes");
            DropTable("dbo.Benefits");
            DropTable("dbo.BdoCompetitorSkus");
            DropTable("dbo.BdoCompetitors");
            DropTable("dbo.BdoChoosenDealerDetailsDuringCalls");
            DropTable("dbo.BaseSkuPrices");
            DropTable("dbo.BaseSkuPriceDetails");
            DropTable("dbo.PackGroups");
            DropTable("dbo.DerivedGroupMargins");
            DropTable("dbo.CustomerGroupDetails");
            DropTable("dbo.CustomerGroups");
            DropTable("dbo.BaseGroupMarginStates");
            DropTable("dbo.BaseGroupMargins");
            DropTable("dbo.Audits");
            DropTable("dbo.MediaTypes");
            DropTable("dbo.AudioFileDetailsForActiveCustomers");
            DropTable("dbo.Attachments");
            DropTable("dbo.Status");
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
            DropTable("dbo.Depots");
            DropTable("dbo.OilTypes");
            DropTable("dbo.SalesOrganizations");
            DropTable("dbo.DistributionChannels");
            DropTable("dbo.Divisions");
            DropTable("dbo.AdditionalCosts");
            DropTable("dbo.SaudaBookingTypes");
            DropTable("dbo.IncoTerms");
            DropTable("dbo.DeleteListCreations");
            DropTable("dbo.Zones");
            DropTable("dbo.Territories");
            DropTable("dbo.Countries");
            DropTable("dbo.States");
            DropTable("dbo.Districts");
            DropTable("dbo.Cities");
            DropTable("dbo.Headquarters");
            DropTable("dbo.Users");
            DropTable("dbo.AccountStatements");
        }
    }
}
