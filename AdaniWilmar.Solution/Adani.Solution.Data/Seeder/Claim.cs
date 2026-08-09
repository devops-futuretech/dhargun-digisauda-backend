using System.Data.Entity.Migrations;
using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.DTO.Enums;
using GMCore.Helper;

namespace Adani.Solution.Data.Seeder
{
    public class Claim : ISeeder
    {
        public void Seed(AdaniContext context)
        {
            //SeedClaim(context);
        }
        private static void SeedClaim(IAdaniContext context)
        {
            context.Claims.AddOrUpdate(x => x.Id,
                new Entities.Claim
                {
                    Id = (int)Claims.ManageOrganization,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageOrganization),
                    Description = "Ability to manage Oraganzation. This is a Sales Track super admin feature.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageRoles,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageRoles),
                    Description = "Ability to manage roles.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageClaims,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageClaims),
                    Description = "Ability to manage claims.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageUser,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageUser),
                    Description = "Ability to add /update/view user.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewUser,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewUser),
                    Description = "Ability to view user.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageCustomer,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageCustomer),
                    Description = "Ability to  add /update /view customer.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewCustomer,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewCustomer),
                    Description = "Ability to view customer.",
                    IsActive = true
                },
                 new Entities.Claim
                 {
                     Id = (int)Claims.ManageBroker,
                     Name = UtilityHelper.GetEnumDescription(Claims.ManageBroker),
                     Description = "Ability to  add /update /view broker.",
                     IsActive = true
                 },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewBroker,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewBroker),
                    Description = "Ability to view broker.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageSku,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageSku),
                    Description = "Ability to  add /update /view sku.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewSku,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewSku),
                    Description = "Ability to add/update/view sku.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ApproveDiscount,
                    Name = UtilityHelper.GetEnumDescription(Claims.ApproveDiscount),
                    Description = "Ability to approve discount.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageSauda,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageSauda),
                    Description = "Ability to add/update/view sauda.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageTradeTicket,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageTradeTicket),
                    Description = "Ability to add/update/view trade tickets.",
                    IsActive = true
                },
                 new Entities.Claim
                 {
                     Id = (int)Claims.ManageLiftingRequest,
                     Name = UtilityHelper.GetEnumDescription(Claims.ManageLiftingRequest),
                     Description = "Ability to add/update/view lifting requests.",
                     IsActive = true
                 },
                 new Entities.Claim
                 {
                     Id = (int)Claims.ManageSalesPlan,
                     Name = UtilityHelper.GetEnumDescription(Claims.ManageSalesPlan),
                     Description = "Ability to add/update/view the Sales plans.",
                     IsActive = true
                 },
                 new Entities.Claim
                 {
                     Id = (int)Claims.ViewSalesPlan,
                     Name = UtilityHelper.GetEnumDescription(Claims.ViewSalesPlan),
                     Description = "Ability to view the sales plans.",
                     IsActive = true
                 },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewReports,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewReports),
                    Description = "Ability to view reports.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageSaudaLimit,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageSaudaLimit),
                    Description = "Ability to Approve SaudaLimit.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ApproveSpecialRate,
                    Name = UtilityHelper.GetEnumDescription(Claims.ApproveSpecialRate),
                    Description = "Ability to Approve Special Rate.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageGeographyDiscount,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageGeographyDiscount),
                    Description = "Ability to add the Geography Discount.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewGeographyDiscount,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewGeographyDiscount),
                    Description = "Ability to view Geography Discount.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageDiscounts,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageDiscounts),
                    Description = "Ability to add the Discount.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewDiscounts,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewDiscounts),
                    Description = "Ability to view Discount.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.AssignDiscount,
                    Name = UtilityHelper.GetEnumDescription(Claims.AssignDiscount),
                    Description = "Ability to Assign Discount.",
                    IsActive = true
                },
                 new Entities.Claim
                 {
                     Id = (int)Claims.ImportData,
                     Name = UtilityHelper.GetEnumDescription(Claims.ImportData),
                     Description = "Ability to Import Data.",
                     IsActive = true
                 },
                new Entities.Claim
                {
                    Id = (int)Claims.ApproveSaudaConversion,
                    Name = UtilityHelper.GetEnumDescription(Claims.ApproveSaudaConversion),
                    Description = "Ability to Approve Sauda Conversion.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewSaudaConversion,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewSaudaConversion),
                    Description = "Ability to View Sauda Conversion.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewAssignedDiscount,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewAssignedDiscount),
                    Description = "Ability to View Assigned Discount.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewSpecialRate,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewSpecialRate),
                    Description = "Ability to View Special Rate.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.RequestSpecialRate,
                    Name = UtilityHelper.GetEnumDescription(Claims.RequestSpecialRate),
                    Description = "Ability to Request Special Rate.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewBulletin,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewBulletin),
                    Description = "Ability to View Bulletin.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageBulletin,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageBulletin),
                    Description = "Ability to Manage Bulletin.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageDealer,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageDealer),
                    Description = "Ability to Manage Dealer.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewDealer,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewDealer),
                    Description = "Ability to View Dealer.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageMaster,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageMaster),
                    Description = "Ability to Manage Master.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewMaster,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewMaster),
                    Description = "Ability to View Master.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageSpecialityFat,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageSpecialityFat),
                    Description = "Ability to Manage Speciality Fat.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewSpecialityFat,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewSpecialityFat),
                    Description = "Ability to View Speciality Fat.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewTradeTicket,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewTradeTicket),
                    Description = "Ability to View Trade Ticket.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ApproveSaudaExtension,
                    Name = UtilityHelper.GetEnumDescription(Claims.ApproveSaudaExtension),
                    Description = "Ability to Approve Sauda Extension.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewSaudaExtension,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewSaudaExtension),
                    Description = "Ability to View Sauda Extension.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageLiftingList,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageLiftingList),
                    Description = "Ability to Manage Lifting List.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageBiddingWindow,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageBiddingWindow),
                    Description = "Ability to Manage BiddingWindow.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageUpdate,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageUpdate),
                    Description = "Ability to Manage Update.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageTarget,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageTarget),
                    Description = "Ability to Manage Target.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewTarget,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewTarget),
                    Description = "Ability to View Target.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageSalesTourPlan,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageSalesTourPlan),
                    Description = "Ability to Manage SalesTourPlan.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewSalesTourPlan,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewSalesTourPlan),
                    Description = "Ability to View SalesTourPlan.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageAttendance,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageAttendance),
                    Description = "Ability to Manage Attendance.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewAttendance,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewAttendance),
                    Description = "Ability to View Attendance.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManagePriceNotification,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManagePriceNotification),
                    Description = "Ability to Manage Price Notification.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.SpecialtyFatQtyApprove,
                    Name = UtilityHelper.GetEnumDescription(Claims.SpecialtyFatQtyApprove),
                    Description = "Ability to Manage Specialty Fat Qty Approve.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.SpecialtyFatQtyRequestForApprove,
                    Name = UtilityHelper.GetEnumDescription(Claims.SpecialtyFatQtyRequestForApprove),
                    Description = "Ability to Manage Specialty Fat Qty RequestForApprove.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.SpecialtyFatQtyReject,
                    Name = UtilityHelper.GetEnumDescription(Claims.SpecialtyFatQtyReject),
                    Description = "Ability to Manage Specialty Fat Qty Reject.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewSpecialtyFatQtyRequestStatus,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewSpecialtyFatQtyRequestStatus),
                    Description = "Ability to Manage Specialty Fat Qty Request View.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageSpecialtyFatQuantityGeography,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageSpecialtyFatQuantityGeography),
                    Description = "Ability to Manage Specialty Fat Qty Limit Geography.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageSpecialtyFatQuantityUser,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageSpecialtyFatQuantityUser),
                    Description = "Ability to Manage Specialty Fat Qty Limit User.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.SpecialtyFatQuantityAssign,
                    Name = UtilityHelper.GetEnumDescription(Claims.SpecialtyFatQuantityAssign),
                    Description = "Ability to Manage Specialty Fat Qty Limit Assign.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.SpecialtyFatQuantityCreate,
                    Name = UtilityHelper.GetEnumDescription(Claims.SpecialtyFatQuantityCreate),
                    Description = "Ability to Manage Specialty Fat Qty Limit Create.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewSpecialtyFatAssignedQuantity,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewSpecialtyFatAssignedQuantity),
                    Description = "Ability to View SpecialtyFat Assigned Quantity.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.AutoAllocation,
                    Name = UtilityHelper.GetEnumDescription(Claims.AutoAllocation),
                    Description = "Auto Allocation.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageCustomerTarget,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageCustomerTarget),
                    Description = "Manage Customer Target.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewCustomerTarget,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewCustomerTarget),
                    Description = "View Customer Target.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageTPFinalPrice,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageTPFinalPrice),
                    Description = "Manage Traditional Process Final Price.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageSupport,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageSupport),
                    Description = "Manage Support.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewSupportIssueList,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewSupportIssueList),
                    Description = "View Support Issue List.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageShipToParty,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageShipToParty),
                    Description = "Manage ShipToParty.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewShipToParty,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewShipToParty),
                    Description = "View ShipToParty.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.GST,
                    Name = UtilityHelper.GetEnumDescription(Claims.GST),
                    Description = "View GST",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.SaudaReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.SaudaReport),
                    Description = "Sauda Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.SaudaLimitReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.SaudaLimitReport),
                    Description = "Sauda Limit Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.IndentReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.IndentReport),
                    Description = "Indent Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.TargetVsAchievementReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.TargetVsAchievementReport),
                    Description = "Target Vs Achievement Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.PCPVsMTPDeviationReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.PCPVsMTPDeviationReport),
                    Description = "PCP Vs MTP Deviation Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.MTPVsDSRDeviationReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.MTPVsDSRDeviationReport),
                    Description = "MTP Vs DSR Deviation Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.DailyStatusReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.DailyStatusReport),
                    Description = "Daily Status Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.PriceReleaseAuditReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.PriceReleaseAuditReport),
                    Description = "Price Release Audit Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.SaudaExecutionAuditReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.SaudaExecutionAuditReport),
                    Description = "Sauda Execution Audit Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.MonthlyTourPlanReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.MonthlyTourPlanReport),
                    Description = "Monthly Tour Plan Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.PermanentCoveragePlanReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.PermanentCoveragePlanReport),
                    Description = "Permanent Coverage Plan Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.MonthlyReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.MonthlyReport),
                    Description = "Monthly Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.PendingContractReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.PendingContractReport),
                    Description = "Pending Contract Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.DSRReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.DSRReport),
                    Description = "DSR Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.PendingContracts,
                    Name = UtilityHelper.GetEnumDescription(Claims.PendingContracts),
                    Description = "Pending Contracts",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.PendingContractComparision,
                    Name = UtilityHelper.GetEnumDescription(Claims.PendingContractComparision),
                    Description = "Pending Contract Comparision",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.SalesRegister,
                    Name = UtilityHelper.GetEnumDescription(Claims.SalesRegister),
                    Description = "Sales Register",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.SalesRegisterComparision,
                    Name = UtilityHelper.GetEnumDescription(Claims.SalesRegisterComparision),
                    Description = "Sales Register Comparision",
                    IsActive = true
                }, new Entities.Claim
                {
                    Id = (int)Claims.ViewConfiguration,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewConfiguration),
                    Description = "View Configuration",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageConfiguration,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageConfiguration),
                    Description = "Manage Configuration",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.SaudaConversionReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.SaudaConversionReport),
                    Description = "Sauda Conversion Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.NewSaudaReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.NewSaudaReport),
                    Description = "New Sauda Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.CallRecordingReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.CallRecordingReport),
                    Description = "Call Recording Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.DailyBookingReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.DailyBookingReport),
                    Description = "Daily Booking Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.CreditLimitReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.CreditLimitReport),
                    Description = "Credit Limit Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.FillerSkuReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.FillerSkuReport),
                    Description = "Filler Sku Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.SkuPremiumAmountReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.SkuPremiumAmountReport),
                    Description = "Sku Premium Amount Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManagePricing,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManagePricing),
                    Description = UtilityHelper.GetEnumDescription(Claims.ManagePricing),
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewPricing,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewPricing),
                    Description = UtilityHelper.GetEnumDescription(Claims.ViewPricing),
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.SchemeGeographyReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.SchemeGeographyReport),
                    Description = "Scheme Geography Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.DemandPlanBillingReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.DemandPlanBillingReport),
                    Description = "Demand Plan Billing Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.CompetitorRateReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.CompetitorRateReport),
                    Description = "Competitor Rate Report",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewCompetitor,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewCompetitor),
                    Description = "Ability to View Competitor.",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ManageCompetitor,
                    Name = UtilityHelper.GetEnumDescription(Claims.ManageCompetitor),
                    Description = "Ability to add / update / competitor",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.ViewPriceDiscovery,
                    Name = UtilityHelper.GetEnumDescription(Claims.ViewPriceDiscovery),
                    Description = "Ability to view PriceDiscovery",
                    IsActive = true
                },
                new Entities.Claim
                {
                    Id = (int)Claims.GPSTrackingReport,
                    Name = UtilityHelper.GetEnumDescription(Claims.GPSTrackingReport),
                    Description = "Ability to view GPS Tracking Report",
                    IsActive = true
                },
                 new Entities.Claim
                 {
                     Id = (int)Claims.ApprovePriceDiscovery,
                     Name = UtilityHelper.GetEnumDescription(Claims.ApprovePriceDiscovery),
                     Description = "Ability to approve PriceDiscovery",
                     IsActive = true
                 },
                  new Entities.Claim
                  {
                      Id = (int)Claims.RequestPriceDiscovery,
                      Name = UtilityHelper.GetEnumDescription(Claims.RequestPriceDiscovery),
                      Description = "Ability to request PriceDiscovery",
                      IsActive = true
                  },
                  new Entities.Claim
                  {
                      Id = (int)Claims.UserLoginHistory,
                      Name = UtilityHelper.GetEnumDescription(Claims.UserLoginHistory),
                      Description = "User Login History",
                      IsActive = true
                  },
                  new Entities.Claim
                  {
                      Id = (int)Claims.QPSDiscount,
                      Name = UtilityHelper.GetEnumDescription(Claims.QPSDiscount),
                      Description = "QPS Discount",
                      IsActive = true
                  },
                  new Entities.Claim
                  {
                      Id = (int)Claims.DynamicForm,
                      Name = UtilityHelper.GetEnumDescription(Claims.DynamicForm),
                      Description = "Dynamic Form",
                      IsActive = true
                  },
                  new Entities.Claim
                  {
                      Id = (int)Claims.ManageSaudaSalesAreaRestriction,
                      Name = UtilityHelper.GetEnumDescription(Claims.ManageSaudaSalesAreaRestriction),
                      Description = "Manage Sauda Sales Area Restriction",
                      IsActive = true
                  },
                  new Entities.Claim
                  {
                      Id = (int)Claims.DistributorStockReport,
                      Name = UtilityHelper.GetEnumDescription(Claims.DistributorStockReport),
                      Description = "Ability to view/download distributor stock report.",
                      IsActive = true
                  }


                  );

        }
    }
}
