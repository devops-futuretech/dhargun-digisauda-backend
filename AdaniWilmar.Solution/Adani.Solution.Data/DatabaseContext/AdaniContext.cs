using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Adani.Solution.Data.DatabaseContextMigrations;
using Adani.Solution.Data.Entities;
using Adani.Solution.Data.Enum;
using Newtonsoft.Json;
using EntityFramework.BulkInsert.Extensions;
using EntityFramework.Extensions;
using System.Data.Entity.Infrastructure;
using Adani.Solution.DTO.Common;

namespace Adani.Solution.Data.DatabaseContext
{
    public class AdaniContext : DbContext, IAdaniContext
    {
        public AdaniContext() : base("DBContext")
        {
            Database.SetInitializer<AdaniContext>(new CheckAndMigrateDatabaseToLatestVersion<AdaniContext, MigrationConfiguration>());
            database = this.database;
            // Get the ObjectContext related to this DbContext
            var objectContext = (this as IObjectContextAdapter).ObjectContext;

            // Sets the command timeout for all the commands
            objectContext.CommandTimeout = 300;
        }

        public IDbSet<Audit> AuditLogs { get; set; }
        public IDbSet<Configuration> Configurations { get; set; }
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<UserDivisionMapping> UserDivisionMappings { get; set; }
        public IDbSet<DateRange> DateRanges { get; set; }
        public IDbSet<EmailTemplate> EmailTemplate { get; set; }
        public IDbSet<Country> Country { get; set; }
        public IDbSet<State> State { get; set; }
        public IDbSet<City> City { get; set; }
        public IDbSet<District> District { get; set; }
        public IDbSet<Taluk> Taluk { get; set; }
        public IDbSet<Division> Divisions { get; set; }
        public IDbSet<UserReportingToMapping> UserReportingToMappings { get; set; }
        //public IDbSet<DivisionDetail> DivisionDetails { get; set; }
        public IDbSet<OilType> OilTypes { get; set; }
        public IDbSet<TransportMode> TransportModes { get; set; }
        public IDbSet<PackGroup> OilPackingTypes { get; set; }
        public IDbSet<PackType> PackTypes { get; set; }
        public IDbSet<SaudaBookingType> SaudaBookingTypes { get; set; }
        public IDbSet<SaudaStatus> SaudaStatus { get; set; }
        public IDbSet<PickingPoint> PickingPoints { get; set; }
        public IDbSet<DeliveryPriority> DeliveryPriorities { get; set; }
        public IDbSet<IncoTerms> IncoTerms { get; set; }
        public IDbSet<ContractType> ContractTypes { get; set; }
        public IDbSet<TradeTicket> TradeTicket { get; set; }
        public IDbSet<TradeTicketDetails> TradeTicketDetails { get; set; }
        public IDbSet<Depot> Depots { get; set; }
        public IDbSet<SaudaType> SaudaTypes { get; set; }
        public IDbSet<Sku> Skus { get; set; }
        public IDbSet<MaterialCost> MaterialCosts { get; set; }
        public IDbSet<PackingCost> PackingCosts { get; set; }
        public IDbSet<PrimaryFreight> PrimaryFreights { get; set; }
        public IDbSet<SecondaryFreight> SecondaryFreights { get; set; }
        public IDbSet<DepotCost> DepotCosts { get; set; }
        public IDbSet<DetentionCost> DetentionCosts { get; set; }
        public IDbSet<HoneycombCost> HoneycombCosts { get; set; }
        public IDbSet<SchemeCost> SchemeCosts { get; set; }
        public IDbSet<ProfitMargin> ProfitMargins { get; set; }
        public IDbSet<CushionMargin> CushionMargins { get; set; }
        public IDbSet<LoadCapacityConversion> LoadCapacityConversion { get; set; }
        public IDbSet<Claim> Claims { get; set; }
        public IDbSet<RoleClaim> RoleClaims { get; set; }
        public IDbSet<RoleType> RoleTypes { get; set; }
        public IDbSet<RoleTypeClaim> RoleTypeClaims { get; set; }
        public IDbSet<Region> Region { get; set; }
        public IDbSet<PlantDepotMapping> PlantDepotMapping { get; set; }
        public IDbSet<DiscountUsers> DiscountUsers { get; set; }
        //public IDbSet<Competitor> Competitor { get; set; }
        //public IDbSet<CompetitorSku> CompetitorSku { get; set; }
        //public IDbSet<CompetitorAnalysis> CompetitorAnalysis { get; set; }
        public IDbSet<LiftingRequest> LiftingRequest { get; set; }
        public IDbSet<LiftingRequestDetails> LiftingRequestDetails { get; set; }
        public IDbSet<RaMargin> RaMargin { get; set; }
        public IDbSet<Sauda> Sauda { get; set; }
        public IDbSet<SaudaOrder> SaudaOrders { get; set; }
        public IDbSet<SaudaLimit> SaudaLimit { get; set; }
        public IDbSet<SkuUomMapping> SkuUomMapping { get; set; }
        public IDbSet<Uom> Uom { get; set; }
        public IDbSet<UserCustomerMapping> UserCustomerMapping { get; set; }
        public IDbSet<UserDepotMapping> UserDepotMapping { get; set; }
        public IDbSet<UserRole> UserRoles { get; set; }
        public IDbSet<SaudaLimitHistory> SaudaLimitHistory { get; set; }

        #region Sales Tour Plan
        public IDbSet<FinancialYear> FinancialYears { get; set; }
        public IDbSet<Month> Months { get; set; }
        public IDbSet<PermanentJourneyPlans> PermanentJourneyPlans { get; set; }
        public IDbSet<PermanentJourneyPlanDetails> PermanentJourneyPlanDetails { get; set; }
        public IDbSet<PermanentJourneyPlanStatus> PJPStatus { get; set; }
        public IDbSet<PermanentJourneyPlanApprovalInformation> PJPApprovalInformation { get; set; }
        public IDbSet<Headquarters> Headquarters { get; set; }
        public IDbSet<MonthlyTourPlans> MonthlyTourPlans { get; set; }
        public IDbSet<MonthlyTourPlanDetails> MonthlyTourPlanDetails { get; set; }
        public IDbSet<MonthlyTourPlanApprovalInformation> MonthlyTourPlanApprovalInformation { get; set; }
        public IDbSet<MonthlyTourPlanStatus> MonthlyTourPlanStatus { get; set; }
        public IDbSet<Reasons> Reasons { get; set; }
        public IDbSet<DayOfWeekName> DayOfWeekNames { get; set; }
        public IDbSet<MonthlyPlanDeviationStatus> MonthlyPlanDeviationStatus { get; set; }
        public IDbSet<MonthlyPlanDeviations> MonthlyPlanDeviation { get; set; }
        #endregion
        public IDbSet<SpecialRatePricingHistory> SpecialRatePricingHistory { get; set; }

        public IDbSet<Pricing> Pricing { get; set; }
        //public IDbSet<PricingLive> PricingLive { get; set; }
        public IDbSet<TodayPricing> TodayPricing { get; set; }
        public IDbSet<PricingBackup> PricingBackup { get; set; }
        public IDbSet<TodayPricingBackup> TodayPricingBackups { get; set; }
        //public IDbSet<IngredientPricing> IngredientPricing { get; set; }
        public IDbSet<BiddingWindowTiming> BiddingWindowTiming { get; set; }
        public IDbSet<DealerLocation> DealerLocation { get; set; }
        public IDbSet<QuantityType> QuantityType { get; set; }
        public IDbSet<RoleDiscount> RoleDiscount { get; set; }
        public IDbSet<PricingUpdateFrequency> PricingUpdateFrequency { get; set; }
        public IDbSet<LiftingRequestStatus> LiftingRequestStatus { get; set; }
        public IDbSet<Status> ApprovalStatus { get; set; }

        public IDbSet<Competitor> Competitor { get; set; }
        public IDbSet<CompetitorSku> CompetitorSku { get; set; }
        public IDbSet<CompetitorAnalysis> CompetitorAnalysis { get; set; }
        public IDbSet<CompetitorAnalysisDetails> CompetitorAnalysisDetails { get; set; }
        public IDbSet<CompetitorAnalysisApproval> CompetitorAnalysisApproval { get; set; }

        public IDbSet<Zone> Zones { get; set; }
        public IDbSet<ZoneStateMapping> ZoneStateMappings { get; set; }
        public IDbSet<DiscountSku> DiscountSku { get; set; }
        public IDbSet<Remarks> Remarks { get; set; }
        public IDbSet<PremiumDiscount> PremiumDiscount { get; set; }

        public IDbSet<PremiumUser> PremiumUser { get; set; }
        public IDbSet<PremiumGeography> PremiumGeography { get; set; }
        public IDbSet<PrimaryDiscountSku> PrimaryDiscountSku { get; set; }
        public IDbSet<UserIncoTerms> UserIncoTerms { get; set; }
        public IDbSet<Territory> Territory { get; set; }

        //public IDbSet<MaterialType> MaterialTypes { get; set; }
        public IDbSet<BookingType> BookingTypes { get; set; }
        public IDbSet<SaudaOrderLiftingRequestMapping> SaudaOrderLiftingRequestMapping { get; set; }
        public IDbSet<KeyPerformanceIndicator> KeyPerformanceIndicator { get; set; }
        public IDbSet<SaudaConversion> SaudaConversion { get; set; }
        public IDbSet<SaudaConversionOrder> SaudaConversionOrder { get; set; }
        public IDbSet<UserSkuTarget> UserTarget { get; set; }
        public IDbSet<DiscountGeography> DiscountGeography { get; set; }
        public IDbSet<SubCategory> SubCategory { get; set; }
        public IDbSet<PendingSaudaRemarks> PendingSaudaRemarks { get; set; }
        public IDbSet<MarketScenario> MarketScenario { get; set; }
        public IDbSet<BdoCompetitor> BdoCompetitor { get; set; }
        public IDbSet<BdoCompetitorSku> BdoCompetitorSku { get; set; }
        public IDbSet<ProspectiveDealer> ProspectiveDealer { get; set; }
        //public IDbSet<UserSalesSaudaTarget> UserSalesSaudaTarget { get; set; }
        public IDbSet<SpecialRate> SpecialRate { get; set; }
        public IDbSet<Invoice> Invoices { get; set; }
        public IDbSet<InvoiceDetail> InvoiceDetails { get; set; }

        public Database database { get; }

        public IDbSet<RoleHierarchy> RoleHierarchy { get; set; }
        public IDbSet<Ticker> Ticker { get; set; }
        public IDbSet<UserCreditMaster> UserCreditMaster { get; set; }
        public IDbSet<UserCustomerSalesTarget> UserCustomerSalesTarget { get; set; }
        public IDbSet<UserCustomerSaudaTarget> UserCustomerSaudaTarget { get; set; }
        public IDbSet<UserOilTypeTarget> UserOilTypeTarget { get; set; }

        //public IDbSet<CompetitorAnalysisDetails> CompetitorAnalysisDetails { get; set; }
        //public IDbSet<CompetitorAnalysisApproval> CompetitorAnalysisApproval { get; set; }

        public IDbSet<PriceNotifyConfiguration> PriceNotifyConfiguration { get; set; }
        public IDbSet<Holiday> Holiday { get; set; }

        public IDbSet<CreditNote> CreditNotes { get; set; }
        public IDbSet<AccountStatement> AccountStatements { get; set; }
        public IDbSet<FeedbackRequest> FeedbackRequests { get; set; }
        public IDbSet<FeedbackType> FeedbackTypes { get; set; }

        public IDbSet<Bulletin> Bulletin { get; set; }
        public IDbSet<BulletinMedia> BulletinMedia { get; set; }
        public IDbSet<ContentType> ContentType { get; set; }
        public IDbSet<MediaType> MediaType { get; set; }
        public IDbSet<Questions> Questions { get; set; }
        public IDbSet<Answers> Answers { get; set; }
        public IDbSet<CustomerLedger> CustomerLedgers { get; set; }
        public IDbSet<Notifications> Notifications { get; set; }
        public IDbSet<WholesellerBdo> WholesellerBdo { get; set; }
        public IDbSet<WholeSellerSalesDetail> WholeSellerSalesDetail { get; set; }

        public IDbSet<SpecialRateApproval> SpecialRateApproval { get; set; }
        public IDbSet<Attachment> Attachment { get; set; }
        public IDbSet<TradeTicketOilType> TradeTicketOilTypes { get; set; }
        public IDbSet<UserAttendance> UserAttendance { get; set; }
        public IDbSet<SpecalityFatDiscountUser> SpecalityFatDiscountUsers { get; set; }
        public IDbSet<SpecalityFatDiscountGeography> SpecalityFatDiscountGeographys { get; set; }
        public IDbSet<SpecialtyFatQuantityRequest> SpecialtyFatQuantityRequests { get; set; }
        public IDbSet<SpecialtyFatQuantityRequestUserDetail> SpecialtyFatQuantityRequestUserDetails { get; set; }
        public IDbSet<UserCustomerTarget> UserCustomerTarget { get; set; }

        public IDbSet<PricePublish> PricePublish { get; set; }

        public IDbSet<SalesTourPlanPcpHistory> SalesTourPlanPcpHistory { get; set; }
        public IDbSet<SalesTourPlanMtpHistory> SalesTourPlanMtpHistory { get; set; }

        public IDbSet<CustomerShipToPartyMapping> CustomerShipToPartyMappings { get; set; }

        public IDbSet<Support> Supports { get; set; }
        public IDbSet<SupportAttachment> SupportAttachments { get; set; }

        public IDbSet<PriceGenerate> PriceGenerate { get; set; }
        public IDbSet<PriceGenerateDetail> PriceGenerateDetail { get; set; }
        public IDbSet<VehicleLodability> VehicleLodability { get; set; }

        public IDbSet<PercentileNumber> PercentileNumber { get; set; }
        public IDbSet<CustomerGroupFive> CustomerGroupFive { get; set; }
        public IDbSet<PercentileNumberDetails> PercentileNumberDetails { get; set; }

        public IDbSet<CustomerGroupMappings> CustomerGroupMappings { get; set; }

        #region RA Version 2.0
        public IDbSet<VolumeLoadability> VolumeLoadability { get; set; }

        public IDbSet<CustomerGroups> CustomerGroups { get; set; }
        public IDbSet<CustomerGroupDetails> CustomerGroupDetails { get; set; } 
        public IDbSet<SchemeDiscountGeography> SchemeDiscountGeography { get; set; }
        public IDbSet<SchemeDiscountGeographyMapping> SchemeDiscountGeographyMappings { get; set; }      

        public IDbSet<Gst> Gst { get; set; }
        public IDbSet<RaNotification> RaNotification { get; set; }
        public IDbSet<RaNotificationDetails> RaNotificationDetails { get; set; }

        public IDbSet<BiddingWindow> BiddingWindow { get; set; }
        public IDbSet<BiddingWindowVolumeCapacity> BiddingWindowVolumeCapacity { get; set; }

        public IDbSet<ConversionFormula> ConversionFormulas { get; set; }
        public IDbSet<ConversionFormulaDetails> ConversionFormulaDetails { get; set; }

        public IDbSet<BenefitTypes> BenefitTypes { get; set; }
        public IDbSet<Benefits> Benefits { get; set; }

        public IDbSet<BaseGroupMargin> BaseGroupMargins { get; set; }
        public IDbSet<DerivedGroupMargin> DerivedGroupMargins { get; set; }
        public IDbSet<BaseGroupMarginStates> BaseGroupMarginStates { get; set; }

        public IDbSet<BaseSkuPrice> BaseSkuPrices { get; set; }
        public IDbSet<BaseSkuPriceDetails> BaseSkuPriceDetails { get; set; }

        public IDbSet<SaudaBiddingCart> SaudaBiddingCart { get; set; }
        public IDbSet<SaudaBiddingCartHeader> SaudaBiddingCartHeaders { get; set; }
        public IDbSet<GuaranteePriceJump> GuaranteePriceJump { get; set; }
        public IDbSet<CounterBidJump> CounterBidJump { get; set; }
        public IDbSet<BiddingWindowCustomerGroups> BiddingWindowCustomerGroups { get; set; }

        public IDbSet<CounterBidNotification> CounterBidNotifications { get; set; }
        public IDbSet<BiddingWindowStatus> BiddingWindowStatus { get; set; }
        public IDbSet<SaudaQuantityConfiguration> SaudaQuantityConfiguration { get; set; }
        public IDbSet<RaSaudaConfiguration> RaSaudaConfiguration { get; set; }
        public IDbSet<BiddingWindowNotificationTiming> BiddingWindowNotificationTiming { get; set; }
        public IDbSet<NotificationHistory> NotificationHistory { get; set; }
        public IDbSet<SchemeDiscountHistory> SchemeDiscountHistory { get; set; }

        #endregion


        public IDbSet<IssueComment> IssueComment { get; set; }
        public IDbSet<TPNotification> TPNotification { get; set; }
        public IDbSet<TPNotificationDetails> TPNotificationDetails { get; set; }
        public IDbSet<PendingContract> PendingContracts { get; set; }
        public IDbSet<SalesRegister> SalesRegister { get; set; }
        public IDbSet<SaudaConversionType> saudaConversionTypes { get; set; }
        public IDbSet<SaudaExtension> SaudaExtension { get; set; }

        public IDbSet<OilTransferCost> OilTransferCosts { get; set; }
        public IDbSet<AdditionalCost> AdditionalCosts { get; set; }

        public IDbSet<SaudaConversionSku> SaudaConversionSkus { get; set; }
        public IDbSet<SaudaConversionSkuDetail> SaudaConversionSkuDetails { get; set; }
        public IDbSet<SaudaExtensionDetailsApproval> SaudaExtensionDetailsApprovals { get; set; }
        public IDbSet<SaudaConversionUnitAndDifferenceRate> SaudaConversionUnitAndDifferenceRates { get; set; }
        public IDbSet<SaudaConversionUnitAndDifferenceRateDetail> SaudaConversionUnitAndDifferenceRateDetails { get; set; }
        public IDbSet<ChequeInventoryDetail> ChequeInventoryDetails { get; set; }
        public IDbSet<DeleteListCreation> DeleteListCreations { get; set; }
        public IDbSet<RAMaterialCost> RAMaterialCost { get; set; }
        public IDbSet<ConfigurationForDivisionsAndEmails> ConfigurationForDivisionsAndEmails { get; set; }
        public IDbSet<CustomerTruckCapacityMapping> CustomerTruckCapacityMapping { get; set; }
        public IDbSet<AudioFileDetailsForActiveCustomers> AudioFileDetailsForActiveCustomers { get; set; }
        public IDbSet<ConsentImageDetailsForCustomers> ConsentImageDetailsForCustomers { get; set; }

        public IDbSet<FillerSkuBasedOnDealer> FillerSkuBasedOnDealer { get; set; }
        public IDbSet<SaudaAudioFileMapping> SaudaAudioFileMapping { get; set; }
        public IDbSet<SalesOrganization> SalesOrganization { get; set; }
        public IDbSet<DistributionChannel> DistributionChannel { get; set; }
        public IDbSet<GPSTracking> GPSTrackings { get; set; }
        public IDbSet<SalesDocumentType> SalesDocumentType { get; set; }
        public IDbSet<OverduePayment> OverduePayment { get; set; }
        public IDbSet<SaudaApproval> SaudaApproval { get; set; }
        public IDbSet<CustomerLedgerDetails> CustomerLedgerDetails { get; set; }
        public IDbSet<BdoChoosenDealerDetailsDuringCall> BdoChoosenDealerDetailsDuringCall { get; set; }
        public IDbSet<SaudaBookingConfiguration> SaudaBookingConfiguration { get; set; }
        public IDbSet<Line> Line { get; set; }
        public IDbSet<CompletedDoNumber> CompletedDoNumbers { get; set; }
        public IDbSet<Entities.UserLoginHistory> UserLoginHistory { get; set; }
        public IDbSet<QpsDiscount> QpsDiscount { get; set; }
        public IDbSet<SlabDiscountDetails> SlabDiscountDetails { get; set; }
        public IDbSet<QPSDiscountSkuMapping> QPSDiscountSkuMapping { get; set; }
        public IDbSet<QPSSlabDetails> QPSSlabDetails { get; set; }
        public IDbSet<GamificationDashboard> GamificationDashboards { get; set; }
        public IDbSet<AnswerOption> AnswerOptions { get; set; }
        public IDbSet<Form> Forms { get; set; }
        public IDbSet<FormQuestion> FormQuestions { get; set; }
        public IDbSet<FormStatus> FormStatuses { get; set; }
        public IDbSet<FreightRoute> FreightRoutes { get; set; }
        public IDbSet<FreightZone> FreightZones { get; set; }
        public IDbSet<QuestionMaster> QuestionMasters { get; set; }
        public IDbSet<QuestionType> QuestionTypes { get; set; }
        public IDbSet<Retailer> Retailers { get; set; }
        public IDbSet<ScheduleDemoUser> ScheduleDemoUsers { get; set; }
        public IDbSet<SubmittedForm> SubmittedForms { get; set; }
        public IDbSet<SubmittedFormAnswerOption> SubmittedFormAnswerOptions { get; set; }
        public IDbSet<SubmittedFormQuestion> SubmittedFormQuestions { get; set; }
        public IDbSet<Vertical> Verticals { get; set; }
        public IDbSet<FormUser> FormUsers { get; set; }
        public IDbSet<SubmittedFormDetails> SubmittedFormDetails { get; set; }
        public IDbSet<ScheduleDemoUserMapping> ScheduleDemoUserMappings { get; set; }
        public IDbSet<CustomerAccountStatement> CustomerAccountStatement { get; set; }
        public IDbSet<Entities.SAPEmailStatement> SAPEmailStatement { get; set; }
        public IDbSet<Entities.DiscountGeographyImportStatus> DiscountGeographyImportStatus { get; set; }
        public IDbSet<Entities.SaudaConditionalBookingConfiguration> SaudaConditionalBookingConfigurations { get; set; }
        public IDbSet<Entities.SaudaConditionalBookingEssentialSkuMapping> SaudaConditionalBookingEssentialSkuMappings { get; set; }
        public IDbSet<Entities.SaudaConditionalBookingMandatorySkuMapping> SaudaConditionalBookingMandatorySkuMappings { get; set; }
        public IDbSet<Entities.SaudaConditionalBookingZoneStateMapping> SaudaConditionalBookingZoneStateMappings { get; set; }
        public IDbSet<UserDivisionDepotMapping> UserDivisionDepotMappings { get; set; }
        public IDbSet<SaudaSalesAreaRestriction> SaudaSalesAreaRestrictions { get; set; }

        public IDbSet<SaudaModification> SaudaModifications { get; set; }
        public IDbSet<SaudaModificationLine> SaudaModificationLines { get; set; }
        public IDbSet<SaudaModificationItem> SaudaModificationItems { get; set; }
        public IDbSet<SaudaModificationOldItem> SaudaModificationOldItems { get; set; }
        public IDbSet<SaudaModificationApproval> SaudaModificationApprovals { get; set; }

        public IDbSet<DistributorStockEntry> DistributorStockEntries { get; set; }
        public IDbSet<DistributorStockEntryDetail> DistributorStockEntryDetails { get; set; }


        public override int SaveChanges()
        {
            //Modified
            var modifiedEntities = ChangeTracker.Entries()
                .Where(t => t.Entity is Auditable && t.State == EntityState.Modified)
                .Select(t => new
                {
                    Original = t.OriginalValues.PropertyNames.ToDictionary(pn => pn, pn => t.OriginalValues[pn]),
                    Current = t.CurrentValues.PropertyNames.ToDictionary(pn => pn, pn => t.CurrentValues[pn]),
                    t.Entity
                }).ToList();

            if (modifiedEntities.Any())
            {
                foreach (var modifiedEntity in modifiedEntities)
                {
                    var theEntity = ((Auditable)modifiedEntity.Entity);
                    var resourceName = Convert.ToString(modifiedEntity.Entity.GetType().Name.Split('_')[0]);
                    if (resourceName.Length > 20)
                    {
                        resourceName = resourceName.Substring(0, 19);
                    }

                    if (theEntity != null && theEntity.ModifiedDate.HasValue)
                    {
                        var audit = new Audit
                        {
                            Action = ActionType.Modified,
                            PerformedBy = theEntity.ModifiedBy,
                            PerformedDate = theEntity.ModifiedDate.Value,
                            Changeset =
                                $"Old Value:{JsonConvert.SerializeObject(modifiedEntity.Original)}, New Value:{JsonConvert.SerializeObject(modifiedEntity.Current)}",
                            Resource = resourceName,
                        };
                        AuditLogs.Add(audit);
                    }
                }
            }

            //Added
            var addedEntities = ChangeTracker.Entries()
                .Where(t => t.Entity is Auditable && t.State == EntityState.Added)
                .Select(t => new
                {
                    Current = t.CurrentValues.PropertyNames.ToDictionary(pn => pn, pn => t.CurrentValues[pn]),
                    t.Entity
                }).ToList();

            if (addedEntities.Any())
            {

                foreach (var addedEntity in addedEntities)
                {
                    var theEntity = ((Auditable)addedEntity.Entity);

                    var resourceName = Convert.ToString(addedEntity.Entity.GetType().Name.Split('_')[0]);
                    if (resourceName.Length > 20)
                    {
                        resourceName = resourceName.Substring(0, 19);
                    }

                    if (theEntity != null)
                    {
                        var audit = new Audit
                        {
                            Action = ActionType.Created,
                            PerformedBy = theEntity.CreatedBy,
                            PerformedDate = theEntity.CreatedDate,
                            Changeset = $"New Value:{JsonConvert.SerializeObject(addedEntity.Current)}",
                            Resource = resourceName,
                        };
                        AuditLogs.Add(audit);
                    }
                }
            }

            //Deleted
            var deletedEntities = ChangeTracker.Entries()
                .Where(t => t.Entity is Auditable && t.State == EntityState.Deleted)
                .Select(t => new
                {
                    Original = t.OriginalValues.PropertyNames.ToDictionary(pn => pn, pn => t.OriginalValues[pn]),
                    t.Entity
                }).ToList();

            if (deletedEntities.Any())
            {

                foreach (var deletedEntity in deletedEntities)
                {
                    var theEntity = ((Auditable)deletedEntity.Entity);

                    var resourceName = Convert.ToString(deletedEntity.Entity.GetType().Name.Split('_')[0]);
                    if (resourceName.Length > 20)
                    {
                        resourceName = resourceName.Substring(0, 19);
                    }

                    if (theEntity != null && theEntity.ModifiedDate.HasValue)
                    {
                        var audit = new Audit
                        {
                            Action = ActionType.Deleted,
                            PerformedBy = theEntity.ModifiedBy,
                            PerformedDate = theEntity.ModifiedDate.Value,
                            Changeset = $"Deleted Value:{JsonConvert.SerializeObject(deletedEntity.Original)}",
                            Resource = resourceName,
                        };
                        AuditLogs.Add(audit);
                    }
                }
            }

            return base.SaveChanges();
        }

        public void BulkInsertProxy<T>(IEnumerable<T> entities) where T : class
        {
            //var enumerable = entities as IList<T> ?? entities.ToList();
            var enumerable = (entities != null && entities.Any()) ? entities.ToList() : entities as IList<T>;
            this.BulkInsert(enumerable);
        }

        public void BulkUpdateProxy(string entities, object[] parameters)
        {
            this.Database.ExecuteSqlCommand(entities, parameters);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new DecimalPrecisionAttributeConvention());

            modelBuilder.Entity<State>().HasRequired(s => s.Country).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<District>().HasRequired(s => s.State).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<City>().HasRequired(s => s.District).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<RaMargin>().HasRequired(s => s.District).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<SaudaOrder>().HasRequired(s => s.Sku).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<SurpriceDiscount>().HasRequired(s => s.Sku).WithMany().WillCascadeOnDelete(false);

           
            //modelBuilder.Entity<Depot>().HasRequired(s => s.State).WithMany().WillCascadeOnDelete(false);
            ////modelBuilder.Entity<Depot>().HasRequired(s => s.Territory).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<Depot>().HasRequired(s => s.District).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<Depot>().HasRequired(s => s.City).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<Retailer>().HasRequired(s => s.State).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<Retailer>().HasRequired(s => s.Territory).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<Retailer>().HasRequired(s => s.District).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<Retailer>().HasRequired(s => s.City).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<Competitor>().HasRequired(s => s.State).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<CushionMargin>().HasRequired(s => s.State).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<CushionMargin>().HasRequired(s => s.Territory).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<CushionMargin>().HasRequired(s => s.District).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<CushionMargin>().HasRequired(s => s.City).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<RaMargin>().HasRequired(s => s.State).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<RaMargin>().HasRequired(s => s.Territory).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<RaMargin>().HasRequired(s => s.District).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<RaMargin>().HasRequired(s => s.City).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<City>().HasRequired(s => s.Territory).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<DiscountUsers>().HasRequired(s => s.OilType).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<DiscountSku>().HasRequired(s => s.OilType).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<DiscountGeography>().HasRequired(s => s.Sku).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<DiscountGeography>().HasRequired(s => s.OilType).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Headquarters>().HasRequired(s => s.State).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Headquarters>().HasRequired(s => s.Territory).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Headquarters>().HasRequired(s => s.District).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Headquarters>().HasRequired(s => s.City).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<SpecialRate>().HasRequired(s => s.Sku).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<Retailer>().HasRequired(s => s.FreightZone).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<Retailer>().HasRequired(s => s.FreightRoute).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SaudaConversionOrder>().HasRequired(s => s.Sku).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<LiftingRequestDetails>().HasRequired(s => s.Sku).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<PremiumUser>().HasRequired(s => s.OilType).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SaudaConversion>().HasRequired(s => s.SaudaOrder).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<SpecalityFatDiscountGeography>().HasRequired(s => s.Sku).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SpecalityFatDiscountUser>().HasRequired(s => s.Sku).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SpecialtyFatQuantityRequest>().HasRequired(s => s.OilType).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<PremiumGeography>().HasRequired(s => s.Sku).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<SkuIngrediantPlant>().HasRequired(s => s.Sku).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<SkuIngrediantPlant>().HasRequired(s => s.Plant).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<SkuIngrediant>().HasRequired(s => s.SkuIngrediantPlant).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<IngredientCost>().HasRequired(s => s.Plant).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SchemeCost>().HasRequired(s => s.Sku).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<SalesTourPlanPcpHistory>().HasRequired(s => s.Territory).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<SalesTourPlanPcpHistory>().HasRequired(s => s.State).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<SalesTourPlanPcpHistory>().HasRequired(s => s.District).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<SalesTourPlanPcpHistory>().HasRequired(s => s.City).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<CustomerShipToPartyMapping>().HasRequired(s => s.Customer).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<CustomerShipToPartyMapping>().HasRequired(s => s.ShipToParty).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<SchemeDiscountUserMapping>().HasRequired<Sku>(s => s.Sku).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<SchemeDiscountUserMapping>().HasRequired<User>(s => s.Customer).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SchemeDiscountGeographyMapping>().HasRequired<Sku>(s => s.Sku).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SchemeDiscountGeographyMapping>().HasRequired<City>(s => s.City).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<VolumeDiscountUserMapping>().HasRequired<Sku>(s => s.Sku).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<VolumeDiscountUserMapping>().HasRequired<User>(s => s.Customer).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<VolumeDiscountGeography>().HasMany(s => s.VolumeDiscountGeographySlab)
            //    .WithRequired(w => w.VolumeDiscountGeography).WillCascadeOnDelete(false);
            //modelBuilder.Entity<VolumeDiscountGeographyMapping>().HasRequired<Sku>(s => s.Sku).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<VolumeDiscountGeographyMapping>().HasRequired<City>(s => s.City).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<SkuDiscountUserMapping>().HasRequired<Sku>(s => s.Sku).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<SkuDiscountGeographyMapping>().HasRequired<Sku>(s => s.Sku).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<SkuDiscountGeographyMapping>().HasRequired<City>(s => s.City).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<SurpriseBenefitUser>().HasRequired<BenefitTypes>(s => s.BenefitTypes).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<SurpriseBenefitUserMapping>().HasRequired<Sku>(s => s.Sku).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<SurpriseBenefitUserMapping>().HasRequired<CustomerGroups>(s => s.CustomerGroup).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<SurpriseBenefitGeographyMapping>().HasRequired<Sku>(s => s.Sku).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<SurpriseBenefitGeographyMapping>().HasRequired<City>(s => s.City).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<SurpriseBenefitGeographyMapping>().HasRequired<SaudaOrder>(s => s.SaudaOrder).WithMany().WillCascadeOnDelete(false);

            //modelBuilder.Entity<GPBenefitUser>().HasRequired<BenefitTypes>(s => s.BenefitTypes).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<GPBenefitUserMapping>().HasRequired<Sku>(s => s.Sku).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<GPBenefitGeographyMapping>().HasRequired<Sku>(s => s.Sku).WithMany().WillCascadeOnDelete(false);
            //modelBuilder.Entity<GPBenefitGeographyMapping>().HasRequired<City>(s => s.City).WithMany().WillCascadeOnDelete(false);

          
            modelBuilder.Entity<BiddingWindowCustomerGroups>().HasRequired<CustomerGroups>(s => s.CustomerGroup).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<BiddingWindow>().HasMany(s => s.BiddingWindowVolumeCapacity)
                .WithRequired(w => w.BiddingWindow).WillCascadeOnDelete(false);

            modelBuilder.Entity<BiddingWindowVolumeCapacity>().HasRequired<OilType>(s => s.OilType).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<ConversionFormula>().HasRequired<OilType>(s => s.OilType).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<ConversionFormula>().HasRequired<PackGroup>(s => s.PackGroup).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<ConversionFormula>().HasRequired<Sku>(s => s.Sku).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<ConversionFormula>().HasMany(h => h.ConversionFormulaDetails).WithRequired(w => w.ConversionFormula).WillCascadeOnDelete(false);
            modelBuilder.Entity<OilTransferCost>().HasRequired(s => s.OilType).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<OilTransferCost>().HasRequired(s => s.Source).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<OilTransferCost>().HasRequired(s => s.Division).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<AdditionalCost>().HasRequired(s => s.OilType).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<AdditionalCost>().HasRequired(s => s.Division).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<BaseGroupMargin>().HasMany(h => h.DerivedGroupMargins).WithRequired(w => w.BaseGroupMargin).WillCascadeOnDelete(false);
            modelBuilder.Entity<BaseGroupMargin>().HasRequired<OilType>(s => s.OilType).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<BaseGroupMargin>().HasRequired<PackGroup>(s => s.PackGroup).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<BaseGroupMargin>().HasRequired<CustomerGroups>(s => s.CustomerGroup).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<BaseGroupMargin>().HasMany(h => h.BaseGroupMarginStates).WithRequired().WillCascadeOnDelete(false);

            modelBuilder.Entity<DerivedGroupMargin>().HasRequired<CustomerGroups>(s => s.CustomerGroup).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<DerivedGroupMargin>().HasRequired<BaseGroupMargin>(s => s.BaseGroupMargin).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<ConversionFormulaDetails>().HasRequired<Sku>(s => s.Sku).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<SaudaBiddingCart>().HasRequired<SaudaBiddingCartHeader>(s => s.SaudaBiddingCartHeader).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SaudaBiddingCart>().HasRequired<Sku>(s => s.Sku).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<PercentileNumberDetails>().HasRequired<PercentileNumber>(s => s.PercentileNumber).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<GuaranteePriceJump>().HasRequired<Division>(s => s.Division).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<CounterBidJump>().HasRequired<Division>(s => s.Division).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<Gst>().HasRequired(s => s.SourceState).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Gst>().HasRequired(s => s.DestinationState).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<FillerSkuBasedOnDealer>().HasRequired(s => s.Sku).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Pricing>().HasRequired(s => s.Sku).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SaudaOrder>().HasRequired(s => s.OilType).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<UserDivisionMapping>().HasRequired(s => s.SalesOrganization).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<DiscountGeography>().HasRequired(s => s.SalesOrganization).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<DiscountUsers>().HasRequired(s => s.Division).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<DiscountUsers>().HasRequired(s => s.SalesOrganization).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<DistributionChannel>().HasRequired(s => s.SalesOrganization).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<OilType>().HasRequired(s => s.Division).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<CustomerGroups>().HasRequired(s => s.Division).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Sku>().HasRequired(s => s.Division).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<MaterialType>().HasRequired(s => s.Division).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<DiscountGeography>().HasRequired(s => s.Division).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<DiscountUsers>().HasRequired(s => s.Sku).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<PackingCost>().HasRequired(s => s.OilType).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SaudaOrder>().HasRequired(s => s.Division).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<UserDivisionMapping>().HasRequired(s => s.Division).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<Sauda>().HasRequired(s => s.Division).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SaudaOrder>().HasRequired(s => s.Sauda).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<SaudaConditionalBookingConfiguration>().HasRequired(s => s.SalesOrganization).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SaudaConditionalBookingConfiguration>().HasRequired(s => s.DistributionChannel).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SaudaConditionalBookingConfiguration>().HasRequired(s => s.Division).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SaudaConditionalBookingMandatorySkuMapping>().HasRequired(s => s.ConditionalBookingEssentialSkuMapping).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SaudaConditionalBookingEssentialSkuMapping>().HasRequired(s => s.SaudaConditionalConfiguration).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SaudaConditionalBookingZoneStateMapping>().HasRequired(s => s.SaudaConditionalConfiguration).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<SaudaSalesAreaRestriction>().HasRequired(s => s.SalesOrganization).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SaudaSalesAreaRestriction>().HasRequired(s => s.DistributionChannel).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SaudaSalesAreaRestriction>().HasRequired(s => s.Division).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<SaudaModificationLine>().HasRequired(s => s.SaudaModification).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SaudaModificationLine>().HasRequired(s => s.OilType).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<SaudaModificationItem>().HasRequired(s => s.SaudaModificationLine).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SaudaModificationItem>().HasRequired(s => s.Sku).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<SaudaModificationOldItem>().HasRequired(s => s.SaudaModificationLine).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SaudaModificationOldItem>().HasRequired(s => s.Sku).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<SaudaModificationApproval>().HasRequired(s => s.SaudaModification).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<SaudaModificationApproval>().HasRequired(s => s.Status).WithMany().WillCascadeOnDelete(false);

            modelBuilder.Entity<DistributorStockEntry>().HasRequired(s => s.User).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<DistributorStockEntryDetail>().HasRequired(s => s.DistributorStockEntry).WithMany().WillCascadeOnDelete(false);
            modelBuilder.Entity<DistributorStockEntryDetail>().HasRequired(s => s.Sku).WithMany().WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }

    public class CheckAndMigrateDatabaseToLatestVersion<TContext, TMigrationsConfiguration> : IDatabaseInitializer<TContext> where TContext : DbContext
        where TMigrationsConfiguration : DbMigrationsConfiguration<TContext>, new()
    {
        public virtual void InitializeDatabase(TContext context)
        {
            if (Utility.IsMigrationFileUpdate)
            {
                var migratorBase = ((MigratorBase)new DbMigrator(Activator.CreateInstance<TMigrationsConfiguration>()));
                if (migratorBase.GetPendingMigrations().Any())
                    migratorBase.Update();
            }
        }
    }
}

