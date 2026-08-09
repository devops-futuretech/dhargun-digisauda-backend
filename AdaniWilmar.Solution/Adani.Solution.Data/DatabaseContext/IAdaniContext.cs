using System;
using System.Collections.Generic;
using System.Data.Entity;
using Adani.Solution.Data.Entities;

namespace Adani.Solution.Data.DatabaseContext
{
    public interface IAdaniContext
    {
        Database Database { get; }
        int SaveChanges();
        void BulkInsertProxy<T>(IEnumerable<T> entities) where T : class;
        void BulkUpdateProxy(string entities, object[] parameters);

        IDbSet<Audit> AuditLogs { get; set; }
        IDbSet<Configuration> Configurations { get; set; }
        IDbSet<Role> Roles { get; set; }
        IDbSet<User> Users { get; set; }
        IDbSet<UserDivisionMapping> UserDivisionMappings { get; set; }
        IDbSet<EmailTemplate> EmailTemplate { get; set; }
        IDbSet<Division> Divisions { get; set; }
        IDbSet<UserReportingToMapping> UserReportingToMappings { get; set; }
        IDbSet<DateRange> DateRanges { get; set; }
        //IDbSet<DivisionDetail> DivisionDetails { get; set; }
        IDbSet<OilType> OilTypes { get; set; }
        IDbSet<TransportMode> TransportModes { get; set; }
        IDbSet<PackGroup> OilPackingTypes { get; set; }
        IDbSet<PackType> PackTypes { get; set; }
        IDbSet<SaudaBookingType> SaudaBookingTypes { get; set; }
        IDbSet<SaudaStatus> SaudaStatus { get; set; }
        IDbSet<PickingPoint> PickingPoints { get; set; }
        IDbSet<DeliveryPriority> DeliveryPriorities { get; set; }
        IDbSet<IncoTerms> IncoTerms { get; set; }
        IDbSet<ContractType> ContractTypes { get; set; }
        IDbSet<Country> Country { get; set; }
        IDbSet<State> State { get; set; }
        IDbSet<City> City { get; set; }
        IDbSet<District> District { get; set; }
        IDbSet<Taluk> Taluk { get; set; }
        IDbSet<TradeTicket> TradeTicket { get; set; }
        IDbSet<TradeTicketDetails> TradeTicketDetails { get; set; }
        IDbSet<Depot> Depots { get; set; }
        IDbSet<SaudaType> SaudaTypes { get; set; }
        IDbSet<Sku> Skus { get; set; }
        IDbSet<MaterialCost> MaterialCosts { get; set; }
        IDbSet<PackingCost> PackingCosts { get; set; }
        IDbSet<PrimaryFreight> PrimaryFreights { get; set; }
        IDbSet<SecondaryFreight> SecondaryFreights { get; set; }
        IDbSet<DepotCost> DepotCosts { get; set; }
        IDbSet<DetentionCost> DetentionCosts { get; set; }
        IDbSet<HoneycombCost> HoneycombCosts { get; set; }
        IDbSet<SchemeCost> SchemeCosts { get; set; }
        IDbSet<ProfitMargin> ProfitMargins { get; set; }
        IDbSet<CushionMargin> CushionMargins { get; set; }
        IDbSet<LoadCapacityConversion> LoadCapacityConversion { get; set; }
        IDbSet<Claim> Claims { get; set; }
        IDbSet<RoleClaim> RoleClaims { get; set; }
        IDbSet<RoleType> RoleTypes { get; set; }
        IDbSet<RoleTypeClaim> RoleTypeClaims { get; set; }
        IDbSet<Region> Region { get; set; }
        IDbSet<PlantDepotMapping> PlantDepotMapping { get; set; }
        IDbSet<Invoice> Invoices { get; set; }
        IDbSet<InvoiceDetail> InvoiceDetails { get; set; }

        IDbSet<VolumeLoadability> VolumeLoadability { get; set; }
        IDbSet<SpecialRatePricingHistory> SpecialRatePricingHistory { get; set; }

        //IDbSet<Competitor> Competitor { get; set; }
        //IDbSet<CompetitorSku> CompetitorSku { get; set; }
        //IDbSet<CompetitorAnalysis> CompetitorAnalysis { get; set; }
        IDbSet<LiftingRequest> LiftingRequest { get; set; }
        IDbSet<LiftingRequestDetails> LiftingRequestDetails { get; set; }
        IDbSet<RaMargin> RaMargin { get; set; }
        IDbSet<Sauda> Sauda { get; set; }
        IDbSet<SaudaOrder> SaudaOrders { get; set; }
        IDbSet<SaudaLimit> SaudaLimit { get; set; }
        IDbSet<SkuUomMapping> SkuUomMapping { get; set; }
        IDbSet<Uom> Uom { get; set; }
        IDbSet<UserCustomerMapping> UserCustomerMapping { get; set; }
        IDbSet<UserDepotMapping> UserDepotMapping { get; set; }
        IDbSet<UserRole> UserRoles { get; set; }
        IDbSet<SaudaLimitHistory> SaudaLimitHistory { get; set; }
        IDbSet<DiscountUsers> DiscountUsers { get; set; }
        IDbSet<Competitor> Competitor { get; set; }
        IDbSet<CompetitorSku> CompetitorSku { get; set; }
        IDbSet<CompetitorAnalysis> CompetitorAnalysis { get; set; }
        IDbSet<CompetitorAnalysisDetails> CompetitorAnalysisDetails { get; set; }
        IDbSet<CompetitorAnalysisApproval> CompetitorAnalysisApproval { get; set; }
        IDbSet<Pricing> Pricing { get; set; }
        //IDbSet<PricingLive> PricingLive { get; set; }
        IDbSet<TodayPricing> TodayPricing { get; set; }
        IDbSet<PricingBackup> PricingBackup { get; set; }
        IDbSet<TodayPricingBackup> TodayPricingBackups { get; set; }
        //IDbSet<IngredientPricing> IngredientPricing { get; set; }
        IDbSet<BiddingWindowTiming> BiddingWindowTiming { get; set; }
        IDbSet<DealerLocation> DealerLocation { get; set; }
        IDbSet<QuantityType> QuantityType { get; set; }
        IDbSet<RoleDiscount> RoleDiscount { get; set; }
        IDbSet<PricingUpdateFrequency> PricingUpdateFrequency { get; set; }
        IDbSet<LiftingRequestStatus> LiftingRequestStatus { get; set; }
        IDbSet<Status> ApprovalStatus { get; set; }

        IDbSet<DiscountSku> DiscountSku { get; set; }
        IDbSet<Remarks> Remarks { get; set; }

        #region Sales Tour Plan
        IDbSet<FinancialYear> FinancialYears { get; set; }
        IDbSet<Month> Months { get; set; }
        IDbSet<PermanentJourneyPlans> PermanentJourneyPlans { get; set; }
        IDbSet<PermanentJourneyPlanDetails> PermanentJourneyPlanDetails { get; set; }
        IDbSet<PermanentJourneyPlanStatus> PJPStatus { get; set; }
        IDbSet<PermanentJourneyPlanApprovalInformation> PJPApprovalInformation { get; set; }
        IDbSet<Headquarters> Headquarters { get; set; }
        IDbSet<MonthlyTourPlans> MonthlyTourPlans { get; set; }
        IDbSet<MonthlyTourPlanDetails> MonthlyTourPlanDetails { get; set; }
        IDbSet<MonthlyTourPlanApprovalInformation> MonthlyTourPlanApprovalInformation { get; set; }
        IDbSet<MonthlyTourPlanStatus> MonthlyTourPlanStatus { get; set; }
        IDbSet<Reasons> Reasons { get; set; }
        IDbSet<DayOfWeekName> DayOfWeekNames { get; set; }
        IDbSet<MonthlyPlanDeviationStatus> MonthlyPlanDeviationStatus { get; set; }
        IDbSet<MonthlyPlanDeviations> MonthlyPlanDeviation { get; set; }
        #endregion

        IDbSet<Zone> Zones { get; set; }
        IDbSet<ZoneStateMapping> ZoneStateMappings { get; set; }
        IDbSet<PremiumDiscount> PremiumDiscount { get; set; }
        IDbSet<UserIncoTerms> UserIncoTerms { get; set; }
        IDbSet<Territory> Territory { get; set; }

        IDbSet<PremiumUser> PremiumUser { get; set; }
        IDbSet<PremiumGeography> PremiumGeography { get; set; }
        IDbSet<PrimaryDiscountSku> PrimaryDiscountSku { get; set; }
        //IDbSet<MaterialType> MaterialTypes { get; set; }
        IDbSet<BookingType> BookingTypes { get; set; }
        IDbSet<SaudaOrderLiftingRequestMapping> SaudaOrderLiftingRequestMapping { get; set; }
        IDbSet<KeyPerformanceIndicator> KeyPerformanceIndicator { get; set; }
        IDbSet<SaudaConversion> SaudaConversion { get; set; }
        IDbSet<SaudaConversionOrder> SaudaConversionOrder { get; set; }
        IDbSet<UserSkuTarget> UserTarget { get; set; }
        IDbSet<DiscountGeography> DiscountGeography { get; set; }
        IDbSet<SubCategory> SubCategory { get; set; }

        IDbSet<RoleHierarchy> RoleHierarchy { get; set; }
        IDbSet<PendingSaudaRemarks> PendingSaudaRemarks { get; set; }
        IDbSet<MarketScenario> MarketScenario { get; set; }
        IDbSet<BdoCompetitor> BdoCompetitor { get; set; }
        IDbSet<BdoCompetitorSku> BdoCompetitorSku { get; set; }
        IDbSet<ProspectiveDealer> ProspectiveDealer { get; set; }
        //IDbSet<UserSalesSaudaTarget> UserSalesSaudaTarget { get; set; }
        IDbSet<Ticker> Ticker { get; set; }
        IDbSet<UserCreditMaster> UserCreditMaster { get; set; }
        IDbSet<SpecialRate> SpecialRate { get; set; }
        //IDbSet<CompetitorAnalysisDetails> CompetitorAnalysisDetails { get; set; }
        IDbSet<UserCustomerSalesTarget> UserCustomerSalesTarget { get; set; }
        IDbSet<UserCustomerSaudaTarget> UserCustomerSaudaTarget { get; set; }
        IDbSet<UserOilTypeTarget> UserOilTypeTarget { get; set; }
        //IDbSet<CompetitorAnalysisApproval> CompetitorAnalysisApproval { get; set; }
        IDbSet<PriceNotifyConfiguration> PriceNotifyConfiguration { get; set; }

        IDbSet<CreditNote> CreditNotes { get; set; }
        IDbSet<AccountStatement> AccountStatements { get; set; }
        IDbSet<FeedbackRequest> FeedbackRequests { get; set; }
        IDbSet<FeedbackType> FeedbackTypes { get; set; }

        IDbSet<Holiday> Holiday { get; set; }

        IDbSet<Bulletin> Bulletin { get; set; }
        IDbSet<BulletinMedia> BulletinMedia { get; set; }
        IDbSet<ContentType> ContentType { get; set; }
        IDbSet<MediaType> MediaType { get; set; }
        IDbSet<Questions> Questions { get; set; }
        IDbSet<Answers> Answers { get; set; }
        IDbSet<CustomerLedger> CustomerLedgers { get; set; }

        IDbSet<SpecialRateApproval> SpecialRateApproval { get; set; }
        IDbSet<Notifications> Notifications { get; set; }
        IDbSet<Attachment> Attachment { get; set; }
        IDbSet<WholesellerBdo> WholesellerBdo { get; set; }
        IDbSet<WholeSellerSalesDetail> WholeSellerSalesDetail { get; set; }
        IDbSet<TradeTicketOilType> TradeTicketOilTypes { get; set; }
        IDbSet<UserAttendance> UserAttendance { get; set; }

        IDbSet<SpecalityFatDiscountUser> SpecalityFatDiscountUsers { get; set; }
        IDbSet<SpecalityFatDiscountGeography> SpecalityFatDiscountGeographys { get; set; }
        IDbSet<SpecialtyFatQuantityRequest> SpecialtyFatQuantityRequests { get; set; }
        IDbSet<SpecialtyFatQuantityRequestUserDetail> SpecialtyFatQuantityRequestUserDetails { get; set; }
        IDbSet<UserCustomerTarget> UserCustomerTarget { get; set; }

        IDbSet<PricePublish> PricePublish { get; set; }

        IDbSet<SalesTourPlanPcpHistory> SalesTourPlanPcpHistory { get; set; }
        IDbSet<SalesTourPlanMtpHistory> SalesTourPlanMtpHistory { get; set; }

        IDbSet<CustomerShipToPartyMapping> CustomerShipToPartyMappings { get; set; }

        IDbSet<Support> Supports { get; set; }
        IDbSet<SupportAttachment> SupportAttachments { get; set; }

        IDbSet<PriceGenerate> PriceGenerate { get; set; }
        IDbSet<PriceGenerateDetail> PriceGenerateDetail { get; set; }

        IDbSet<VehicleLodability> VehicleLodability { get; set; }
        IDbSet<CustomerGroupFive> CustomerGroupFive { get; set; }

        IDbSet<PercentileNumberDetails> PercentileNumberDetails { get; set; }

        IDbSet<CustomerGroupMappings> CustomerGroupMappings { get; set; }


        #region RA Version 2.0

        IDbSet<CustomerGroups> CustomerGroups { get; set; }
        IDbSet<CustomerGroupDetails> CustomerGroupDetails { get; set; }
        IDbSet<SchemeDiscountGeography> SchemeDiscountGeography { get; set; }
        IDbSet<SchemeDiscountGeographyMapping> SchemeDiscountGeographyMappings { get; set; }
        IDbSet<Gst> Gst { get; set; }

        IDbSet<RaNotification> RaNotification { get; set; }
        IDbSet<RaNotificationDetails> RaNotificationDetails { get; set; }

        IDbSet<BiddingWindow> BiddingWindow { get; set; }
        IDbSet<BiddingWindowVolumeCapacity> BiddingWindowVolumeCapacity { get; set; }

        IDbSet<ConversionFormula> ConversionFormulas { get; set; }
        IDbSet<ConversionFormulaDetails> ConversionFormulaDetails { get; set; }

        IDbSet<BenefitTypes> BenefitTypes { get; set; }
        IDbSet<Benefits> Benefits { get; set; }

        IDbSet<BaseGroupMargin> BaseGroupMargins { get; set; }
        IDbSet<DerivedGroupMargin> DerivedGroupMargins { get; set; }
        IDbSet<BaseGroupMarginStates> BaseGroupMarginStates { get; set; }

        IDbSet<PercentileNumber> PercentileNumber { get; set; }

        IDbSet<BaseSkuPrice> BaseSkuPrices { get; set; }
        IDbSet<BaseSkuPriceDetails> BaseSkuPriceDetails { get; set; }

        IDbSet<SaudaBiddingCart> SaudaBiddingCart { get; set; }
        IDbSet<SaudaBiddingCartHeader> SaudaBiddingCartHeaders { get; set; }
        IDbSet<GuaranteePriceJump> GuaranteePriceJump { get; set; }
        IDbSet<CounterBidJump> CounterBidJump { get; set; }
        IDbSet<BiddingWindowCustomerGroups> BiddingWindowCustomerGroups { get; set; }
        IDbSet<CounterBidNotification> CounterBidNotifications { get; set; }
        IDbSet<BiddingWindowStatus> BiddingWindowStatus { get; set; }
        IDbSet<SaudaQuantityConfiguration> SaudaQuantityConfiguration { get; set; }
        IDbSet<RaSaudaConfiguration> RaSaudaConfiguration { get; set; }
        IDbSet<BiddingWindowNotificationTiming> BiddingWindowNotificationTiming { get; set; }
        IDbSet<NotificationHistory> NotificationHistory { get; set; }
        IDbSet<SchemeDiscountHistory> SchemeDiscountHistory { get; set; }
        #endregion

        IDbSet<IssueComment> IssueComment { get; set; }
        IDbSet<TPNotification> TPNotification { get; set; }
        IDbSet<TPNotificationDetails> TPNotificationDetails { get; set; }
        IDbSet<PendingContract> PendingContracts { get; set; }
        IDbSet<SalesRegister> SalesRegister { get; set; }
        IDbSet<SaudaConversionType> saudaConversionTypes { get; set; }

        IDbSet<OilTransferCost> OilTransferCosts { get; set; }
        IDbSet<AdditionalCost> AdditionalCosts { get; set; }
        IDbSet<SaudaExtension> SaudaExtension { get; set; }

        IDbSet<SaudaConversionSku> SaudaConversionSkus { get; set; }
        IDbSet<SaudaConversionSkuDetail> SaudaConversionSkuDetails { get; set; }
        IDbSet<SaudaExtensionDetailsApproval> SaudaExtensionDetailsApprovals { get; set; }
        IDbSet<SaudaConversionUnitAndDifferenceRate> SaudaConversionUnitAndDifferenceRates { get; set; }
        IDbSet<SaudaConversionUnitAndDifferenceRateDetail> SaudaConversionUnitAndDifferenceRateDetails { get; set; }
        IDbSet<ChequeInventoryDetail> ChequeInventoryDetails { get; set; }
        IDbSet<DeleteListCreation> DeleteListCreations { get; set; }
        IDbSet<RAMaterialCost> RAMaterialCost { get; set; }
        IDbSet<ConfigurationForDivisionsAndEmails> ConfigurationForDivisionsAndEmails { get; set; }
        IDbSet<CustomerTruckCapacityMapping> CustomerTruckCapacityMapping { get; set; }
        IDbSet<ConsentImageDetailsForCustomers> ConsentImageDetailsForCustomers { get; set; }
        IDbSet<AudioFileDetailsForActiveCustomers> AudioFileDetailsForActiveCustomers { get; set; }
        IDbSet<SaudaAudioFileMapping> SaudaAudioFileMapping { get; set; }
        IDbSet<FillerSkuBasedOnDealer> FillerSkuBasedOnDealer { get; set; }

        IDbSet<SalesOrganization> SalesOrganization { get; set; }
        IDbSet<DistributionChannel> DistributionChannel { get; set; }
        IDbSet<GPSTracking> GPSTrackings { get; set; }
        IDbSet<SalesDocumentType> SalesDocumentType { get; set; }
        IDbSet<OverduePayment> OverduePayment { get; set; }
        IDbSet<SaudaApproval> SaudaApproval { get; set; }
        IDbSet<CustomerLedgerDetails> CustomerLedgerDetails { get; set; }
        IDbSet<BdoChoosenDealerDetailsDuringCall> BdoChoosenDealerDetailsDuringCall { get; set; }
        IDbSet<SaudaBookingConfiguration> SaudaBookingConfiguration { get; set; }
        IDbSet<Line> Line { get; set; }
        IDbSet<CompletedDoNumber> CompletedDoNumbers { get; set; }
        IDbSet<Entities.UserLoginHistory> UserLoginHistory { get; set; }
        IDbSet<QpsDiscount> QpsDiscount { get; set; }
        IDbSet<SlabDiscountDetails> SlabDiscountDetails { get; set; }
        IDbSet<QPSDiscountSkuMapping> QPSDiscountSkuMapping { get; set; }
        IDbSet<QPSSlabDetails> QPSSlabDetails { get; set; }
        IDbSet<GamificationDashboard> GamificationDashboards { get; set; }
        IDbSet<CustomerAccountStatement> CustomerAccountStatement { get; set; }

        #region Complaint management
        IDbSet<AnswerOption> AnswerOptions { get; set; }
        IDbSet<Form> Forms { get; set; }
        IDbSet<FormQuestion> FormQuestions { get; set; }
        IDbSet<FormStatus> FormStatuses { get; set; }
        IDbSet<FreightRoute> FreightRoutes { get; set; }
        IDbSet<FreightZone> FreightZones { get; set; }
        IDbSet<QuestionMaster> QuestionMasters { get; set; }
        IDbSet<QuestionType> QuestionTypes { get; set; }
        IDbSet<Retailer> Retailers { get; set; }
        IDbSet<ScheduleDemoUser> ScheduleDemoUsers { get; set; }
        IDbSet<SubmittedForm> SubmittedForms { get; set; }
        IDbSet<SubmittedFormAnswerOption> SubmittedFormAnswerOptions { get; set; }
        IDbSet<SubmittedFormQuestion> SubmittedFormQuestions { get; set; }
        IDbSet<Vertical> Verticals { get; set; }
        IDbSet<FormUser> FormUsers { get; set; }
        IDbSet<SubmittedFormDetails> SubmittedFormDetails { get; set; }
        IDbSet<ScheduleDemoUserMapping> ScheduleDemoUserMappings { get; set; }

        #endregion
        IDbSet<Entities.SAPEmailStatement> SAPEmailStatement { get; set; }
        IDbSet<Entities.DiscountGeographyImportStatus> DiscountGeographyImportStatus { get; set; }
        IDbSet<Entities.SaudaConditionalBookingConfiguration> SaudaConditionalBookingConfigurations { get; set; }
        IDbSet<Entities.SaudaConditionalBookingEssentialSkuMapping> SaudaConditionalBookingEssentialSkuMappings { get; set; }
        IDbSet<Entities.SaudaConditionalBookingMandatorySkuMapping> SaudaConditionalBookingMandatorySkuMappings { get; set; }
        IDbSet<Entities.SaudaConditionalBookingZoneStateMapping> SaudaConditionalBookingZoneStateMappings { get; set; }

        IDbSet<UserDivisionDepotMapping> UserDivisionDepotMappings { get; set; }

        IDbSet<SaudaSalesAreaRestriction> SaudaSalesAreaRestrictions { get; set; }

        IDbSet<SaudaModification> SaudaModifications { get; set; }
        IDbSet<SaudaModificationLine> SaudaModificationLines { get; set; }
        IDbSet<SaudaModificationItem> SaudaModificationItems { get; set; }
        IDbSet<SaudaModificationOldItem> SaudaModificationOldItems { get; set; }
        IDbSet<SaudaModificationApproval> SaudaModificationApprovals { get; set; }

        IDbSet<DistributorStockEntry> DistributorStockEntries { get; set; }
        IDbSet<DistributorStockEntryDetail> DistributorStockEntryDetails { get; set; }





    }
}
