using System.Configuration;
using System;
using System.Data;
using System.Reflection;
using System.Collections.Generic;

namespace Adani.Solution.Service.Common
{
    public class Constants
    {
        public const int NumberOfDaysTakenForNotification = -6;
        public const int NumberOfDaysAddedTogetPreviousDate = -1;
        public const int NumberOfDaysAddedToGetNextDate = 1;
        public const string DefaultDecimalPlacesForLabel = "{0:0.00}";
        public const string DefaultDecimalPlacesForMT = "{0:0.0000}";

        //Paging
        public const int PageSize = 10;
        public const int GeoMinimumCityCount = 5;

        public const string VehicleAlreadyExists = "Vechicle with the specified size already exists for ";
        public const string MaterialTypeExists = "Material Type already exists with this vertical";
        public const string VehicleSize = "Vechicle size should be greater than zero";
        public const string Exception = "Internal Server Error";
        public const string InValidClientKey = "Invalid client key";
        public const string ClientKeyCantBeEmpty = "Client key cant be empty";
        public const string InvalidRequest = "Invalid request";
        public const string InvalidupdateQuantityRequest = "Please entry valid quantity";
        public const string QuantityLimitRequest = "You don't have the quantity limit";
        public const string QuantityLimitExpired = "Your quantity limit expired for OilType : [OiltypeName].";
        public const string UserNotFound = "User not found";
        public const string InValidOtpNumber = "Invalid OTP number";
        public const string UserIdMissing = "User missing";
        public const string QuantityRequestExists = "Quantity Request Already Exists";
        public const string NationalHeadIdMissing = "User is not NationalTrader";
        public const string PublishIdMissing = "Publish id missing";
        public const string Unauthorised = "Unauthorised";
        public const string InvalidLoginCredential = "Invalid login credential";
        public const string InActiveUser = "Inactive user";
        public const string RecordNotFound = "No record found";
        public const string CustomerRecordNotFound = "No Customer Record found";
        public const string SaudaOrderCantApprove = "The following SaudaOrders does not have Sauda Number so can't be approved ,";
        public const string RecordDeleted = "Record Deleted Successfully";
        public const string RoleEmpty = "Role empty";
        public const string RoleTypeEmpty = "Role type empty";
        public const string FromDateEmpty = "From date is empty";
        public const string ToDateEmpty = "To date is empty";
        public const string FromDateInvalid = "Invalid from date";
        public const string ToDateInvalid = "Invalid to date";
        public const string DiscountInvalid = "Discount is invalid";
        public const string SaudaHoldMessage = "You cannot create sauda, already more than one Material is on Hold";
        public const string InvalidRequestToUser = "User not found to assign for next level of approval";
        public const string MediaSavedSuccessfully = "Media saved successfully";
        public const string UserDontHavePermission = "user dont have permission to access this.";
        public const int LoginBaseHour = 6;
        public const string SkuEmpty = "Material is empty";
        public const string StockReportedSuccessfully = "Stock reported successfully";
        public const string InvalidStockQuantity = "Quantity in case should be greater than zero";
        public const string EmployeeEmpty = "Employees is empty";
        public const string QuantityLimitEmpty = "Quantity Limit is empty";
        public const string OiltypeIsEmpty = "Oiltype is empty";
        public const string SkuAlreadyMappedToUser = "Material mapped to user.";
        public const string QuantityLimitError = "Assigned quantity is ";
        public const string EmployeeIsEmpty = "Employees is empty";
        public const string SkuIsEmpty = "Material is empty";
        public const string SpecialityFatDiscountId = "SpecialityFatDiscount Id is empty";
        public const string InvalidSpecialityFatDiscountId = "SpecialityFatDiscount Id is invalid";
        public const string DiscountMissing = "Discount is empty";
        public const string SkuIngredientPlantInsertError = "Material ingredient plant insert error";
        public const string SkuIngredientPlantUpdateError = "Material ingredient plant insert error";
        public const string PasswordExpired = "Password is Expired, Please reset the Password";
        public const string BiddingCartIdIsMissing = "Bidding cart Id is missing for ";
        public const string BiddingCartIsMissing = "Bidding cart is missing for ";
        public const string UserBased = "User Based";
        public const string GeographyBased = "Geography Based";
        public const string FromSkuNotFound = "From Material not found";
        public const string FromUnitNotFound = "From Unit not found";
        public const string FromPackGroupNotFound = "From Pack Group not found";
        public const string ToSkuList = "To Material List is empty";
        public const string NotificationError = "Notification sends some error occurred, Please try again.";
        public const string StateEmpty = "State is empty";
        public const string PlantOrDepotEmpty = "Plant/Depot is empty";
        public const string DiscountLimitExceed = "Discount limit exceeded";
        public const string SalesOrderNoIsEmpty = "{0} Sales Order no is empty";
        public const string SaudaBookingConfiguration = "Sauda Booking Configuration successfully done";
        public const string SaudaBookingConfigurationUpdate = "Sauda Booking Configuration successfully Updated";
        public const string SaudaBookingConfigurationCombinationExits = "The selected Sauda Booking Configuration combination already exists.";
        public const string SaudaSalesAreaRestricitonConfiguration = "Sauda Sales Area Restriciton Configuration successfully done";
        public const string SaudaSalesAreaRestricitonConfigurationUpdate = "Sauda Sales Area Restriciton Configuration successfully Updated";
        public const string SaudaSalesAreaRestrictionConfigurationCombinationExits = "The selected Sauda Sales Area Restriction Configuration combination already exists.";
        public const string SaudaSalesAreaRestricitedDistributor = "Booking not accepted — Sauda time is over. Contact with your State Trader for further assistance";
        public const string SaudaSalesAreaRestricitedStateTrader = "Sauda time is over. Contact with your Zonal Trader for further assistance";
        public const string SaudaSalesAreaRestricitedZonalTrader = "Sauda time is over. Contact with your National Trader for further assistance";

        public const string AdminContactMessage = "Total Sauda Limit is “0 MT”, Kindly Contact Admin ";
        public const string UploadedMedias = "UploadMedias";
        public const string DealerRoleOnlyAccepted = "Dealer role only accepted";
        public const string BdoRoleOnlyAccepted = "StateTrader role only accepted";
        public const string UserNotMappedToCustomerGroup = "User not mapped to customer group";
        public const string UserNotMappingToBdo = "User not mapped to StateTrader";
        public const string NoBiddingWindows = "No Bidding Windows";
        public const string BiddingWindowStatusChanged = "Bidding Window has been ";
        public const string SaudaAllocationhasbeen = "Sauda Allocation has been ";
        public const string SaudaAllocationTimeHasBeenExceeds = "Sauda allocation time has been completed";
        public const string InvalidDealer = "Dealer Code does not exists for above filter values.";

        public const string DealerIdEmpty = "Dealer Id is empty";
        public const string PriceAlreadyPublished = "Price already published";
        public const string UserRoleMappingNotExists = "User role mapping not exists";
        public const string PushTokenEmpty = "Push Token is empty";
        public const string DiscountShouldBeLessthen = "Discount should be less then or equal to ";
        public const string PremiumShouldBeLessthen = "Premium should be less then or equal to ";
        public const string LiftingSkuInsufficientQuantity = "Lifting Material is Insufficient Quantity";
        public const string GroupAlreadyExists = "Group Already Exists";
        public const string BeniftAreadyExists = "Benefit with specfic category already exists";
        public const string SaudaAcceptedMessage = "(in counterbid)";
        public const string ReportingToUserNotSelected = "Please Select Reporting To User";
        public const string ReportingToUserNotExists = "Reporting to user not exists";
        public const string DivisionNotMappedFound = "Division not mapped for that user ";

        //Pricing
        public const string CostAlreadyExistiInThisDate = "Cost already exist in this date.";
        public const string PriceDetailsSavedSuccessfully = "Price details saved successfully.";
        public const string PriceDetailsPublishedSuccessfully = "Price details published successfully.";
        public const string CostAlreadyExistiInThisCity = "Cost already exist in this city : ";
        public const string CostAlreadyExistiInThisCities= "Costs are already configured for the selected cities.";
        public const string CostAlreadyExistiInThisUser = "Cost already exist in this user : ";
        public const string PricingIdisnotValid = "Pricing Id is not Valid";
        public const string BiddingWindowisnotValid = "Bidding Window is not Valid";
        public const string PremiumAlreadyExistiInThisUser = "Premium already exist in this user : ";
        public const string DiscountAlreadyExistiInThisUser = "Discount already exist in this user : ";
        public const string SkuAlreadyBookedinBidding = "Material already booked in this Bidding Window";
        public const string QtyLimitAlreadyExistiInThisUser = "Quantity limit already exist in this date for user : ";
        public const string QtyLimitValidation = "Quantity limit should be less then or equalto Assigned quantity : ";
        public const string QtyLimitExceeded = "Assigned quantity limit is exceeded. If you need extra quantity raise the request.";
        public const string QtyLimitReduceExceed = "Updated quantity limit is exceed from remaining quantity.";
        public const string PriceCalculationCompleted = "Price calculation has been completed. Count : [COUNT] Start Time : [START_TIME] End Time : [END_TIME]";
        public const string PriceCalculationSuccess = "Price calculation has been completed";
        public const string PriceCalculationInprocess = "Price calculation is in process";
        public const string Count = "[COUNT]";
        public const string StartTime = "[START_TIME]";
        public const string EndTime = "[END_TIME]";
        public const string PriceCalculationFailed = "Price calculation has been failed. Please check the web portal for more detail.";
        public const string PricingNotifyConfigurationAlreadyExistiInThisCity = "Price Notify Configuration already exist in selected cities";
        public const string DiscountAlreadyProcessed = "Discount already processed";
        public const string PremiumAlreadyProcessed = "Premium already processed";
        public const string DiscountAlreadyExist = "Discount already exist";

        public const string RecordAlreadyExist = "Record already exists.";
        public const string PushNotificationTitleMissing = "Push notification title missing";
        public const string PushNotifcationMessageMissing = "Push notification message missing";
        public const string ContentTypeIdMissing = "Content Type Id missing";
        public const string BulletinIdMissing = "Bulletin Id missing";

        public const string EmamiPriceMissing = "Emami price missing";
        public const string WorkableQuantityMissing = "Workable quantity missing";
        public const string WorkablePriceMissing = "Workable price missing";
        public const string CompetitorMissing = "Competitor missing";
        public const string CompetitorNotFound = "Competitor not found";
        public const string SaudaRateMissing = "Sauda rate missing";
        public const string MarketOperatingPriceMissing = "Market operating price missing";
        public const string CompetitorAnalysisSaveSuccess = "Competitor analysis added successfully";
        public const string CompetitorAnalysDetailsMissing = "Competitor analysis details missing";
        public const string CityListEmpty = "Please select any cities";
        public const string MaxAllowableMissing = "MaxAllowable is not added for Material ";
        public const string GrossWeightMissing = "GrossWeight is not added for Material ";

        //Send Sauda Extension days to SAP
        public const string SentSaudaExtensionDaysToSAP = "Sauda Extension APP to SAP successfully";
        public const string ExtentionDaysMissing = "Extention days missing";
        public const string SaudaNumbersMissing = "Sauda Numbers missing";
        public const string SaudaReleasedToSAP = "Sauda Released to SAP successfully";

        //Role
        public const string RoleNameExist = "Role name exist";
        public const string ClaimEmpty = "Claim empty";
        public const string ClaimNameExist = "Claim name exist";
        public const string RoleTypeNameExist = "Role type name exist";
        public const string RoleCannotDelete = "Role cannot delete";
        public const string RoleNotFound = "Role not found";
        public const string RoleTypeNotFound = "Role type not found";
        public const string RoleTypeCannotDelete = "Role type cannot delete";
        public const string InvalidUser = "Invalid user";
        public const string RoleTypeAndClaimCannotDelete = "Role type and claim cannot delete";
        public const string PrimeRoleTypeCannotDelete = "Prime role type cannot delete";
        public const string RoleTypeMappedToRole = "Role type mapped to role";
        public const string PrimeRoleCannotDelete = "Prime role cannot delete";
        public const string SaudaAllocationSuccessfully = "Sauda Allocation Saved Successfully";

        //Auto Allocation
        public const string SpecalityFatDiscountAlreadyExistiInThisDate = "SpecalityFatDiscount already exist in this date.";

        public const string InvalidPincode = "Please enter valid pincode";
        public const string InvalidDistrictForPincode = "Invalid District For Pincode";

        //Plant
        public const string PlantNameEmpty = "Plant Name is empty";
        public const string PlantCodeEmpty = "Plant Code is empty";

        public const string EmailEmpty = "Email is empty";
        public const string PlantNameExists = "Plant Name already exist";
        public const string PlantCodeExists = "Plant Code already exist";
        public const string EmailExists = "Email already exist";
        public const string FromToDateValidateErrorMsg = "Valid To date should be greater then or equal to Valid From date";
        public const string FromAndToDateAlreadyExist = "Oil Type and Material Valid From and Valid To date already exist";

        //depot
        public const string DepotNameEmpty = "Depot Name is empty";
        public const string DepotCodeEmpty = "Depot Code is empty";
        public const string DepotNameExists = "Depot Name already exist";
        public const string DepotCodeExists = "Depot Code already exist";

        public const string SupportIdMissing = "Id is missing";


        //Rake
        public const string RakeNameEmpty = "Rake Name is empty";
        public const string RakeCodeEmpty = "Rake Code is empty";
        public const string RakeNameExists = "Rake Name already exist";
        public const string RakeCodeExists = "Rake Code already exist";

        //Ingredients
        public const string IngredientNameExists = "Ingredient Name already exist";
        public const string SkuIngredientNameExists = "Material - Ingredient Name already exist";
        public const string SkuIngredientPercentage = "Ingredients Percentage Should be 100";

        public const string RoleDiscountSkuNameExists = "Material Name already exist";
        public const string CompetitorNameExists = "Competitor Name already exist";
        public const string OilTypeNameExists = "Oil Name already exist";
        public const string OilTypeExists = "Oil already exist";
        public const string OilTypeCodeExists = "Oil Code already exist";
        public const string VerticaleNameExists = "Vertical Name already exist";
        public const string TerritoryNameExists = "Territory Name already exist";
        public const string LastBiddingWindowClosed = "Already  last window enabled for the day. Please unselect the old one";
        public const string LimitRequestMissing = "Please select a limit request";
        public const string LimitStatusUpdated = "Sauda Limit Status Updated Successfully";
        public const string QuantityUpdated = "Quantity Updated Successfully";
        public const string SpecialRateRequestMissing = "Please select a special rate request to proceed";
        public const string SpecialRateStatusUpdated = "Special Rate Status has been updated Successfully";
        public const string ApprovalRequest = "Approval Request";
        public const string ApprovalRequestMessage = "Dear Sir , Sauda Booking has been raised. you can proceed with Approval Flow.";

        public const string SpecialRateStatusAlreadyUpdated = "Special Rate has been already approved/rejected";
        public const string SaudaStatusAlreadyUpdated = "Sauda has been already approved/rejected";
        public const string PriceDiscoveryStatusAlreadyUpdated = "Price Discovery has been already approved/rejected";
        public const string SaudaLimitStatusAlreadyUpdated = "Sauda Limit has been already approved/rejected";
        public const string PermanentJourneyPlanAlreadyUpdated = "Permanent Journey Plan has been already approved/rejected";
        public const string MonthlyTourPlanAlreadyUpdated = "Monthly Tour Plan has been already approved/rejected";
        public const string MTPDeviationAlreadyUpdated = "MTP Deviation has been already approved/rejected";
        public const string SaudaConversionAlreadyUpdated = "Sauda Conversion has been already approved/rejected";
        public const string SaudaExtensionAlreadyUpdated = "Sauda Extension has been already approved/rejected";
        public const string SpecialtyFatQuantityRequestAlreadyUpdated = "Specialty Fat Quantity Request has been already approved/rejected";


        public const string SaudaOrderIsEmpty = "Sauda orders is empty ";
        public const string SpecialRateNotFoundWithApproval = "No special rate approval requests found with approval status for today";
        public const string SaudaLimitIsExceeds = "Sauda Limit is Exceeds";
        public const string IndentRequestIsExceeds = "Indent request exceeds. Available quantity is ";
        public const string IndentRequestSuccess = "Indent request done successfully";
        public const string SpecialRateRequestIdMissing = "Special rate request id missing";
        public const string SkuGeographicalLimitExceeds = "Geographical limit exceeds for Sku : [SKU_NAME]. Available quantity is [QUANTITY]MT";
        public const string SkuBdoLimitExceeds = "StateTrader limit exceeds for Sku : [SKU_NAME]. Available quantity is [QUANTITY]MT";
        public const string OilTypeBdoLimitExceeds = "StateTrader limit exceeds for OilType : [OiltypeName]. Available quantity is [QUANTITY]MT";
        public const string OilTypeLimitExceeds = "limit exceeds for OilType : [OiltypeName]. Available quantity is [QUANTITY]MT";
        public const string SkuGeographicalLimitReached = "Geographical limit reached for Sku : [SKU_NAME].";
        public const string SkuBdoLimitReached = "StateTrader limit reached for Sku : [SKU_NAME].";
        public const string SkuGeographicalBdoLimitExceeds = "Geographical limit and StateTrader limit exceeds for Sku : [SKU_NAME]. Available quantity for geographical limit is [GEO_QUANTITY]MT and StateTrader limit is [BDO_QUANTITY]MT";
        public const string SpecialRateSaudaMessage = "BidPrice ({0}) should be less than or equal to the QuotedPrice ({1}). Special Rate based Converted Sauda discount only allowed not premium";

        public const string UserMappedWithBDO = "Sauda booking type could not be changed.User is mapped With StateTrader ";
        public const string BiddingWindowExistsFromToHours = "Bidding Window already exist";
        public const string BDONotMapped = "StateTrader is not mapped for the dealer.";
        public const string BDOLimitNotExists = "StateTrader Limit is Not Exists";
        public const string UserLimitNotExists = "User Limit is Not Exists for OilType : [OiltypeName], Contact your Reporting Manager.";
        public const string QuantityExceedstheLimit = "Quantity Exceeds the Limit";
        public const string SaudaDetailUpdateError = "Sauda Details update some error occured";
        public const string NoSaudaToUpdate = "There is no Sauda to Update for the selected Customers";
        public const string SqlDefualtDatetime = "01-01-1900 00:00:00";

        public const string SpecialRateRequestCreationNotification = "Special Rate approval with Sku: [SKU_NAME] for [QUANTITY]Case(s) at Rs.[PRICE] is been requested.";
        public const string SpecialRateRequestCreation = "Special Rate requested created";
        public const string SpecialRateApproveNotification = "Special Rate approval with Sku: [SKU_NAME] for [QUANTITY]Case(s) at Rs.[PRICE] is been approved.";
        public const string SpecialRateRejectNotification = "Special Rate approval with Sku: [SKU_NAME] for [QUANTITY]Case(s) at Rs.[PRICE] is been rejected.";
        public const string SpecialRateRequestForApproval = "Special Rate request for approval";
        public const string SaudaLimitRequestForApproval = "Dear Sir ,Sauda Limit Enhancement request is raised. You can proced with approval flow.";

        //Trade Ticket
        public const string TradeTicketIdMissing = "Trade Ticket id is missing";

        public const string BiddingErrorMessage = "Bidding Window is still processing so you should edit sauda in Bidding Status";

        //Sauda
        public const string DeaerNotMappingToTheUser = "Dealer not mapping to the user";
        public const string SaudaMissing = "Sauda is missing";
        public const string SaudaNotFound = "Sauda not found";
        public const string InvalidSauda = "Invalid Sauda";
        public const string CounterBidOfferTimeLimitExceeds = "Time limit to approve counter bid offer exceeds";
        public const string SaudaOrderReject = "Sauda orders rejected successfully";
        public const string TaxPaidValue = "1.05";
        public const string OutOfState = "Out of [STATE_NAME]";
        public const string StateName = "[STATE_NAME]";
        public const string SaudaCounterBitOffer = "Sauda counterbit offer approved successfully";

        //SaudaBiddingCart
        public const string SaudaBiddingCartId = "Sauda BiddingCart Id is Invalid";
        public const string SaudaBiddingCartIdMissing = "Sauda BiddingCart Id is Missing";

        //Sauda Conversion
        public const string SaudaConversionSuccess = "Sauda conversion successfully done";
        public const string SaudaConversionQuantityMismatch = "Sauda conversion quantity is mismatch";
        public const string SaudaAlreadyConverted = "Sauda conversion already done";
        public const string SaudaAlreadyExtended = "Sauda extension already done";

        //Sauda Modificaiton

        public const string SaudaModificationBookedSuccessfully = "Sauda Modificaiton Booked Successfully. Sauda Modification Id : ";
        public const string SaudaModificationQuantityMismatch = "Sauda Modificaiton Quantity Mismatch";
        public const string SaudaModificationNoModification = "No Modification Done";
        public const string SaudaModificationDuplicateOrInvalidOilTypeFound = "Duplicate or Invalid Oiltype Found";
        public const string SaudaModificationDuplicateOrInvalidPackTypeFound = "Duplicate or Invalid Packtype Found";
        public const string SaudaModificationPendingExists = "Sauda modification is already pending for this Sauda number. Sauda Modification Id : ";


        //Sauda Extension
        public const string SaudaExtensionSuccess = "Sauda extension successfully done";
        public const string SaudaExtendToDateMissing = "Sauda extend to date missing";

        public const string FeedbackSuccess = "Feedback added successfully";
        public const string AnswerSuccess = "Answer added successfully";

        public const string SkuIngredientAlreadyExists = "Ingredient's are already added to this Material. Please select any other.";

        //SpecialtyFatQuantityRequests
        public const string SkuMissing = "Material is missing";
        public const string SpecialtyFatQuantityRequestsSuccess = "Specialty fat quantity request added successfully";
        public const string SpecialtyFatQuantityRequestsSuccessUpdated = "Specialty fat quantity request updated successfully";
        public const string SpecialtyFatQuantityRequestsNotFound = "Specialty fat quantity request not found";

        //Key Performence
        public const string KeyPerformenceContentDuplicate = "Content already exists";

        //Benefit
        public const string BenefitIsMapped = "Benefit already exist and mapped.";

        public const string RecordSaved = "Record saved";

        //Bidding Window
        public const string BiddingWindowProcessing = "Bidding Window is Processing";
        public const string BiddingWindowExists = "Bidding window already exists";
        public const string BiddingWindowNameExists = "Bidding window name already exists";
        //Competitive analysis
        public const string PriceDiscoveryMissing = "Price discovery missing";
        public const string DealerMobileNumberMissing = "Dealer mobile number missing";
        public const string BDOMobileNumberMissing = "BDO mobile number missing";
        public const string DealerIdMissing = "Dealer id missing";
        public const string BDOIdMissing = "BDO id missing";
        public const string DataExpired = "Data Expired";

        public const string EnglishLanguage = "EN";
        public const string DateFormat = "dd-MMM-yyyy";
        public const string TimeFormat = "hh:mm tt";
        public const string ProductSkuPrefix = "GMS";
        public const string Separator = "/";
        public const string DefaultPassword = "JXJK14rJK/nCUGdsaZIc2w==";
        public const int DefaultTransportModeId = 1;
        public static readonly decimal DefaultLoadQuantity = Convert.ToDecimal(ConfigurationManager.AppSettings["DefaultLoadQuantityMT"].ToString());

        public const decimal SFNoOfPiiceConstant = (decimal)1.114;
        public const decimal SFKgtoLtrConstant = (decimal)1.11421;

        //Support
        public const string SupportDescriptionMissing = "Issue description missing";
        public const string IssueTypeMissing = "Issue type missing";
        public const string ModuleMissing = "Module of issue missing";
        public const string ComponentMissing = "Component is missing";
        public const string SeverityMissing = "Severity of issue missing";
        public const string IssueStatusMissing = "Issue status missing";
        public const string SupportSavedSuccess = "Support request successfully done";
        public const string CallRecordedSavedSuccess = "Audio file saved successfully done";
        public const string CallRecordedSavedFailed = "Audio file save failed";
        public const string SaudaDetailsAgainstCallRecordedSavedSuccess = "Sauda Details Mapped Successfully Against Audio file";

        //Image upload
        public const string MaxFileSize = "File upload allowed upto 2MB";

        public const string SalesOrgAlreadyExists = "Sales Organization Already Exists";
        public const string DistributionChannelAlreadyExists = "Distribution Channel Already Exists";

        //Report
        public const string SalesPersonMissing = "Sales person missing";
        public const string ZonalHeadIsMissing = "Zonal Head Missing";
        //public const string BDOMissing = "StateTrader missing";
        public const string ZoneNameMissing = "Zone Name missing";
        public const string DiscountNotExists = "this discount amount not assigned to you";

        //Bulletin
        public const string BulletinSavedSuccessfully = "Bulletin saved successfully.";
        public const string BulletinCreationError = "Only one bulletin can be active in Latest Update Bulletin Type.Please inactive the existing to create this";
        //Notification Constatnts
        public static readonly string SmsUsername = "(c56xgkuzi8btdnpq9y073,";
        public static readonly string SmsPassword = "*2mvh14r_feao)jsl";
        //Notification Constants Updated
        public static readonly string AirtelSmsUserName = "AWL_AGRI_B_67LYfm3VbuhCD2h8q0o8";
        public static readonly string AirtelSmsPassword = ">)d)D!G[}O$fyij4Yr7iD%";
        public static readonly string SmsSourceAddress = "AWLSMS";
        public static readonly string SmsMessageType = "SERVICE_IMPLICIT";
        //public static readonly string SmsSenderId = "JKFDLR";
        public static readonly string SmsApiUrl = "http://bulksmsservice.co.in/httpapi/v1/sendsms?api-token=";
        public static readonly string SmsApiMethod = "GET";
        public static readonly int SmsRouteValue = 2;
        public static readonly string SmsApiContentType = "application/x-www-form-urlencoded";
        public static readonly string SmsStaticMessage = "&message=";
        public static readonly string SmsStaticUsername = "username=";
        public static readonly string SmsStaticPassword = "&password=";
        public static readonly string SmsStaticServiceType = "&smsservicetype=";
        public static readonly string SmsStaticContent = "&content=";
        public static readonly string SmsStaticMobileNumber = "&mobiles=";
        public static readonly int EtaDefaultDays = 8;
        public static readonly string SmsSender = "&sender=";
        public static readonly string SmsRoute = "&route=";
        public static readonly string SmsNumbers = "&numbers=";
        public static readonly string SmsEncrypt = "192215Adrp1X9tX5a546222";
        public static readonly string SmsCountry = "&Country=";
        public static readonly int SmsCountryId = 0;
        public static readonly string SmsAuthkey = "&authkey=";
        public static readonly string SmsMessage = "Sent my OTP value : 1234";
        public static readonly string SmsOtpApiUrl = "http://control.msg91.com/api/sendotp.php?";
        public static readonly string SaudaApprovalSubject = "Sauda Approved";
        public static readonly string SaudaCreationSubject = "Sauda Creation";
        public static readonly string Saudalimit = "[CUSTOMER_NAME] has exceeded its allotted quantity [OiltypeName]";
        public static readonly string SpecialRateApprovalSubject = "Special Rate Approval";
        public static readonly string SpecialRateRequestForApprovalMessage = "Special Rate has been requested for Approval. you can proceed with Approval flow.";
        public static readonly string SpecialRateRejectSubject = "Special Rate Rejected";
        public static readonly string SaudaExtensionSubject = "Sauda Extension";
        public static readonly string SaudaExtensionMessage = "Contract has been extended for Contract Number - ";
        public static readonly string LiftingRequestCreationSubject = "Sales Order Request Creation";
        public static readonly string SkuFinalRateSubject = "Material Final Rate";
        public static readonly string CounterBidOfferSubject = "Counter Bid Offer";
        public static readonly string TradeTicketQuantityIncreaseSub = "Trade Ticket Quantity Increase ";
        public static readonly string CounterBidWebsiteUrl = "MailRedirect/ApproveSaudaCounterBid?saudaOrderEncryptedId=";
        public const string SaudaBookedSuccessMessage = "Your sauda has been booked. Please complete SKU allocation within ##SAUDAALLOCATIONTIME## time";
        public const string SaudaBookedErrorMessage = "Sauda request received";
        public static readonly string SaudaExpiryNoificationSubject = "Sauda Expiry Noification";
        public static readonly string OverDueNotificationSubject = "Over Due Noification";
        public const string DialerMobileNumberMissing = "Dialer mobile number missing";
        public const string ReceiverMobileNumberMissing = "Receiver mobile number missing";
        public const string ReceiverIdMissing = "Receiver id missing";
        public const string DialerIdMissing = "Dialer id missing";
        public const string DialerMobileNumberNotInDeal = "Dialer mobile mumber is not in Deal App";

        public static readonly string SpecalityFatDiscountUserSubject = "Specality Fat Discount User";
        public const string SpecalityFatDiscountUserEmail = "SpecalityFatDiscountUserEmail";
        public const string SpecalityFatDiscountUserSMS = "SpecalityFatDiscountUserSMS";
        public const string SpecalityFatDiscountUserSaveEmail = "SpecalityFatDiscountUserSaveEmail";
        public const string SpecalityFatDiscountUserSaveSMS = "SpecalityFatDiscountUserSaveSMS";

        //ChatBot
        public const string DealerCodeMissing = "Dealer Code is Empty";

        public const string OtpEmail = "OtpEmail";
        public const string OtpSms = "OtpSMS";
        public const string ForgotPasswordEmail = "ForgotPasswordEmail";
        public const string ForgotPasswordSms = "ForgotPasswordSMS";
        public const string ReachUsEmail = "ReachUsEmail";
        public const string ConversionUnitAndDiffRateEmail = "ConversionUnitAndDiffRateEmail";
        public const string SuccessMessage = "Success";
        public const string SyncErrorMessage = "While Error occured {0} data sync,Please check failed details.";
        public const string SyncSuccessMessage = "Successfully {0} data synced";
        public const string SapSyncSuccessMessage = "Successfully  data synced";
        public const string ServiceErrorMessage = "While Error occured from service,Please check failed details. ";
        public const string ErrorSapMessage = "Error occured Please check file for failed details.";
        public const string ReplaceMainContent = "##MailContent##";
        public const string ReplaceValueContent = "Value";
        public const string Name = "[NAME]";
        public const string RoleName = "[ROLE_NAME]";
        public const string Content = "[CONTENT]";
        public const string Message = "[MESSAGE]";
        public const string OtpValue = "[OTP_VALUE]";
        public const string ConversionUnitAndDiffRateEmailFromTableContent = "[TableContent1]";
        public const string ConversionUnitAndDiffRateEmailToTableContent = "[TableContent2]";
        public const string DataResult = "[DATA_RESULT]";
        public const string ApplicationTitle = "[APP_LINK]";
        public const string Password = "[PASSWORD]";
        public const string EmailContentId = "logo";
        public const string Spike = "Spike";
        public const string NotSpike = "Not Spike";
        public const string MTA = "MTA";
        public const string MTO = "MTO";
        public const string ResendOtpEmailSubject = "Resend OTP for forgot password";
        public const string ConversionUnitAndDiffRatSubject = "Sauda conversion unit and Difference rate";
        public const string SaudaCreationEmail = "SaudaCreationEmail";
        public const string SaudaCreationSMS = "SaudaCreationSMS";
        public const string SaudaApprovalEmail = "SaudaApprovalEmail";
        public const string SaudaApprovalSMS = "SaudaApprovalSMS";
        public const string SaudaBiddingEmail = "SaudaBiddingEmail";
        public const string SpecialRateApprovalEmail = "SpecialRateApprovalEmail";
        public const string SpecialRateApprovalSMS = "SpecialRateApprovalSMS";
        public const string SpecialRateRejectEmail = "SpecialRateRejectEmail";
        public const string SpecialRateRejectSMS = "SpecialRateRejectSMS";
        public const string SaudaLimitApprovalEmail = "SaudaLimitApprovalEmail";
        public const string SaudaLimitApprovalSMS = "SaudaLimitApprovalSMS";
        public const string SaudaLimitRejectEmail = "SaudaLimitRejectEmail";
        public const string SaudaLimitRejectSMS = "SaudaLimitRejectSMS";
        public const string LiftingRequestCreationEmail = "LiftingRequestCreationEmail";
        public const string LiftingRequestCreationSMS = "LiftingRequestCreationSMS";
        public const string LiftingRequestApprovalEmail = "LiftingRequestApprovalEmail";
        public const string LiftingRequestApprovalSMS = "LiftingRequestApprovalSMS";
        public const string LiftingRequestApprovalSubject = "Sales Order Request Approval";
        public const string BiddingWindowStopedEmail = "BiddingWindowStopedEmail";
        public const string BiddingWindowStopedSms = "BiddingWindowStopedSms";
        public const string UserIncotermsEmail = "UserIncotermsEmail";
        public const string OverDueNotificationEmail = "OverDueNotification";
        public const string SaudaExpiryNoificationEmail = "SaudaExpiryNoification";
        public const string SaudaNumber = "[SAUDA_NUMBER]";
        public const string RequestNumber = "[REQUEST_NUMBER]";
        public const string SkuPricing = "[SKU_PRICING]";
        public const string SaudaLimitExceeds = "Sauda limit exceeds";
        public const string CounterBidSuccess = "Counter bid has been approved successfully";
        public const string CounterBidReject = "Counter bid has been rejected";
        public const string CounterBidUpdateSuccess = "Counter bid updated and notification sent successfully";
        public const string NotificationSuccess = "Notification sent successfully";
        public const string TradeTicketQuantityIncrease = "TradeTicketQuantityIncrease";
        public const string PCPApproval = "PCPApproval";
        public const string PCPApprovalSMS = "PCPApprovalSMS";
        public const string MTPApproval = "MTPApproval";
        public const string MTPApprovalSMS = "MTPApprovalSMS";
        public const string PCPApprovalSubject = "PCP Approval Status";
        public const string MTPApprovalSubject = "MTP Approval Status";
        public const string MTPDeviationApproval = "MTPDeviationApproval";
        public const string MTPDeviationApprovalSMS = "MTPDeviationApprovalSMS";
        public const string MTPDeviationApprovalSubject = "MTP Deviation Approval Status";
        public const string SaudaLimitExtensionCreationEmail = "SaudaLimitExtensionCreationEmail";
        public const string SaudaLimitExtensionCreationSMS = "SaudaLimitExtensionCreationSMS";
        public const string SaudaLimitExtensionCreationSubject = "Sauda Limit Enhancement";
        public const string SaudaLimitApprovalSubject = "Sauda Limit Approval";
        public const string SaudaLimitRejectSubject = "Sauda Limit Rejected";
        public const string SaudaApprovalTPFlowEmail = "SaudaApprovalTPFlowEmail";
        public const string SaudaApprovalTPFlowSMS = "SaudaApprovalTPFlowSMS";
        public const string SaudaHoldTPFlowNotificationEmail = "SaudaHoldTPFlowNotificationEmail";
        public const string SaudaHoldTPFlowNotificationSMS = "SaudaHoldTPFlowNotificationSMS";
        public const string SaudaRejectTPFlowNotificationEmail = "SaudaRejectTPFlowNotificationEmail";
        public const string SaudaRejectTPFlowNotificationSMS = "SaudaRejectTPFlowNotificationSMS";
        public const string PriceConfigFinalPricePublishEmail = "PriceConfigFinalPricePublishEmail";
        public const string PriceConfigFinalPricePublishSMS = "PriceConfigFinalPricePublishSMS";
        public const string SpecalityFatDiscountAcceptEmail = "SpecalityFatDiscountAcceptEmail";
        public const string SpecalityFatDiscountAcceptSMS = "SpecalityFatDiscountAcceptSMS";
        public const string SpecalityFatDiscountRejectEmail = "SpecalityFatDiscountRejectEmail";
        public const string SpecalityFatDiscountRejectSMS = "SpecalityFatDiscountRejectSMS";
        public const string SpecialityFatLimitApprovalSubject = "Speciality Fat Limit Request Approval";
        public const string SpecialityFatLimitRejectSubject = "Speciality Fat Limit Request Rejected";
        public const string SoldToPartyNotFound = "{0} Sold to party not found";
        public const string SupportIssueSubmittedSubject = "Support - Issue Register";
        public const string SupportIssueSubmittedEmail = "SupportIssueSubmittedEmail";
        public const string SupportIssueSubmittedSMS = "SupportIssueSubmittedSMS";

        public const string WindowCompletedNotParticipatedEmail = "WindowCompletedNotParticipatedEmail";
        public const string WindowCompletedNotParticipatedSMS = "WindowCompletedNotParticipatedSMS";

        public const string WindowCompletedEmail = "WindowCompletedEmail";
        public const string WindowCompletedSMS = "WindowCompletedSMS";

        //Sauda Extension policy
        public const string InvalidOilTypeId = "Invalid Oil type";
        public const string InvalidState = "Invalid State";
        public const string InvalidExtensionDays = "Invalid Extension Days";

        public const string RemarksMissing = "Remarks Missing";

        public const string BiddingQty = "[BIDDING_QTY]";
        public const string ContractQty = "[CONTRACT_QTY]";
        public const string Accepted = "Accepted";
        public const string SkuPricings = "[SKU_PRICINGS]";
        public const string SkuPriceFormat = "<p style=\"margin-bottom:5px;\">Plant : [PLANT_NAME] Depot : [DEPOT_NAME] Sku : [SKU_PRICE] </p>";
        public const string SkuPrice = "[SKU_PRICE]";
        public const string PlantName = "[PLANT_NAME]";
        public const string DepotName = "[DEPOT_NAME]";
        public const string SkuPriceFormatMobile = "Plant : [PLANT_NAME] Depot : [DEPOT_NAME] Sku : [SKU_PRICE]";

        public const string IsSMSUrl = "IsSMS";
        public const string IsEMAILUrl = "IsEMAIL";
        public const string IsPushNotificationUrl = "IsPushNotification";

        public const string SaudaConversionApprovalEmail = "SaudaConversionApprovalEmail";
        public const string SaudaConversionApprovalSMS = "SaudaConversionApprovalSMS";
        public const string FinalRateNotificationEmail = "FinalRateNotificationEmail";
        public const string FinalRateNotificationSMS = "FinalRateNotificationSMS";
        public const string NotificationEmail = "NotificationEmail";
        public const string FirebaseSenderId = "FirebaseSenderId";
        public const string PushNotifyServerkey = "PushNotifyServerkey";
        public const string PushNotifyUrl = "PushNotifyUrl";
        public const string CounterBidOfferNotificationEmail = "CounterBidOfferNotificationEmail";
        public const string CounterBidOfferNotificationSMS = "CounterBidOfferNotificationSMS";
        public const string CounterBidOffer = "CounterBidOffer";
        public const string SaudaOrderPendingNotificationEmail = "SaudaOrderPendingNotificationEmail";
        public const string SaudaOrderPendingNotificationSMS = "SaudaOrderPendingNotificationSMS";
        public const string SaudaOrderHoldNotificationEmail = "SaudaOrderHoldNotificationEmail";
        public const string SaudaOrderHoldNotificationSMS = "SaudaOrderHoldNotificationSMS";
        public const string SaudaOrderRejectNotificationEmail = "SaudaOrderRejectNotificationEmail";
        public const string SaudaOrderRejectNotificationSMS = "SaudaOrderRejectNotificationSMS";
        public const string SaudaExtensionRequestNotificationEmail = "SaudaExtensionRequestNotificationEmail";
        public const string SaudaExtensionRequestNotificationSMS = "SaudaExtensionRequestNotificationSMS";
        public const string SaudaExtensionApprovalNotificationEmail = "SaudaExtensionApprovalNotificationEmail";
        public const string SaudaExtensionApprovalNotificationSMS = "SaudaExtensionApprovalNotificationSMS";
        public const string SaudaExtensionRejectNotificationEmail = "SaudaExtensionRejectNotificationEmail";
        public const string SaudaExtensionRejectNotificationSMS = "SaudaExtensionRejectNotificationSMS";
        public const string SaudaConversionRequestEmail = "SaudaConversionRequestEmail";
        public const string SaudaConversionRequestSMS = "SaudaConversionRequestSMS";
        public const string SaudaConversionRejectEmail = "SaudaConversionRejectEmail";
        public const string SaudaConversionRejectSMS = "SaudaConversionRejectSMS";
        public const string FinalPricePublishNotificationEmail = "FinalPricePublishNotificationEmail";
        public const string FinalPricePublishNotificationSMS = "FinalPricePublishNotificationSMS";
        public const string RAFinalPricePublishNotificationEmail = "RAFinalPricePublishNotificationEmail";
        public const string RAFinalPricePublishNotificationSMS = "RAFinalPricePublishNotificationSMS";
        public const string CustomerDetailsChangeNotificationEmail = "CustomerDetailsChangeNotificationEmail";
        public const string CustomerDetailsChangeNotificationSMS = "CustomerDetailsChangeNotificationSMS";
        public const string SaudaBookedSubject = "Sauda Booked";
        public const string SaudaOnHoldSubject = "Sauda on hold";
        public const string SaudaRejectedSubject = "Sauda Rejected";
        public const string SaudaBiddingDetails = "Sauda Bidding";
        public const string SaudaExtensionRequestSubject = "Sauda extension request";
        public const string SaudaExtensionApprovalSubject = "Sauda extension approval";
        public const string SaudaExtensionRejectSubject = "Sauda extension reject";
        public const string ServiceNotificationEmail = "ServiceNotificationEmail";
        public const string ServiceNotificationSMS = "ServiceNotificationSMS";
        public const string SaudaConversionRequestSubject = "Sauda conversion request";
        public const string SaudaConversionRejectSubject = "Sauda conversion reject";
        public const string SaudaConversionApprovalSubject = "Sauda conversion approval";
        public const string FinalPricePublishSubject = "Final pricing published";
        public const string ServiceNotificationSubject = "Service Notification";
        public const string CustomerDetailsChangeSubject = "Customer details changed";
        public const string SaudaCreationRAFlowEmail = "SaudaCreationRAFlowEmail";
        public const string SaudaCreationRAFlowSMS = "SaudaCreationRAFlowSMS";
        public const string SaudaCreationRAFlowSubject = "Sauda Accepted";
        public const string SaudaBiddingApprovedSubject = "Sauda Bidding Approved";
        public const string CounterBidAcceptSubject = "Counter Bid Accepted";
        public const string CounterBidRejectSubject = "Counter Bid Rejected";
        public const string BiddingWindowStopedSubject = "Bidding Window Stopped";
        public const string SpecialityFatLimitSubject = "Speciality Fat Limit";
        public const string FreightRouteNotFount = "Freight Route is empty";
        public const string TransportModeNotFount = "Transport Mode is empty";
        public const string FreightRouteNotAvailable = "Freight Routes not available";
        public const string TransportModeNotAvailable = "Transport Modes not available";
        public const string SaudaBiddingApprovedNotificationEmail = "SaudaBiddingApprovedNotificationEmail";
        public const string SaudaBiddingApprovedNotificationSMS = "SaudaBiddingApprovedNotificationSMS";
        public const string InCounterBidOffer = "In Counter Bid Offer";
        //Push Notification
        public const string ComplaintApprovalStatus = "Form Approval Status updated";
        public const string ComplaintFormStatus = "Form Status updated";
        public const string UnderstandingFormSubmit = "New understanding form submitted";
        public const string ComplaintFormSubmit = "New Complaint form submitted";
        public const string ComplaintFormSubmitName = "ComplaintFormSubmit";
        public const string NewFormAssigned = "New Form assigned";
        public const string Demoscheduled = "Demo scheduled";
        public const string DemoRescheduled = "Demo rescheduled";

        public static readonly string PriceDiscoverySubject = "Price Discovery";
        public const string PriceDiscoveryEmail = "PriceDiscoveryEmail";
        public const string PriceDiscoverySMS = "PriceDiscoverySMS";
        public const string SkuName = "[SKU_NAME]";
        public const string OiltypeName = "[OiltypeName]";
        public const string SkuOld = "[SKU_OLD]";
        public const string SkuNew = "[SKU_NEW]";
        public const string ApproveOrReject = "[APPROVE_REJECT]";
        public const string FinalRate = "[FINAL_RATE]";
        public const string Status = "[STATUS]";
        public const string CounterBidOfferPrice = "[COUNTER_BID_Offer]";
        public const string Quantity = "[QUANTITY]";
        public const string Price = "[PRICE]";
        public const string NoOfDays = "[NO_OF_Days]";
        public const string Date = "[Date]";
        public const string URL = "[URL]";
        public const string MethodName = "[METHOD_NAME]";
        public const string UserName = "[USER_NAME]";
        public const string CustomerName = "[CUSTOMER_NAME]";
        public const string GeoLimitQuantity = "[GEO_QUANTITY]";
        public const string BdoLimitQuantity = "[BDO_QUANTITY]";
        public const string BiddingWindowName = "[BIDDINGWINDOW_NAME]";
        public const string BiddingWindowStartTime = "[BIDDINGWINDOW_STARTTIME]";
        public const string BiddingWindowEndTime = "[BIDDINGWINDOW_ENDTIME]";
        public const string BiddingWindowSaudaAllocationStartTime = "[BIDDINGWINDOW_SAUDAALLOCATIONSTARTTIME]";
        public const string BiddingWindowSaudaAllocationEndTime = "[BIDDINGWINDOW_SAUDAALLOCATIONENDTIME]";
        public const string OilType = "[BIDDINGWINDOW_OILTYPE]";
        public const string TotalVolumeCapaciy = "[BIDDINGWINDOW_TOTALVOLUMECAPACITY]";
        public const string RemainingVolumeCapacity = "[BIDDINGWINDOW_REMAININGVOLUMECAPACITY]";
        public const string DISCOUNT_OR_DAYS = "[DISCOUNT_OR_DAYS]";
        public const string PERCASE_OR_DAYS = "[PERCASE_OR_DAYS]";
        public const string PERCASE = "PERCASE";
        public const string DAYS = "DAYS";
        public const string BENEFIT_TYPE = "[BENEFIT_TYPE]";
        public const string BENEFIT = "[BENEFIT]";
        public const string SAUDAALLOCATIONTIME = "[SAUDAALLOCATIONTIME]";
        public const string DISCOUNTORDAYS = "[DISCOUNTORDAYS]";
        public const string BiddingWindowTimeToEnd = "[BiddingWindowTimeToEnd]";
        public const string OrderNumber = "[OrderNumber]";
        public const string Expirydate = "[Expirydate]";


        public const string IncoTerms = "[INCO_TERMS]";
        public const string NewIncoTerms = "[NEW_INCO_TERMS]";
        public const string RemovedIncoTerms = "[REMOVED_INCO_TERMS]";
        public const string IncoTerms_MobileNo = "[INCOTERM_MOBILE_NO]";
        public const string FROM_TIME = "[FROM_TIME]";
        public const string TO_TIME = "[TO_TIME]";
        public const string FromDate = "[FROM_DATE]";
        public const string ToDate = "[TO_DATE]";
        public const string BY_FOR = "[BY_FOR]";
        public const string BY = "by";
        public const string FOR = "for";
        public const string ActualDiscount = "[ACTUAL_DISCOUNT]";
        public const string LiftingRequestNumber = "[LIFTING_REQUEST_NUMBER]";
        public const string FormId = "[FORMID]";
        public const string FormName = "[FORMNAME]";
        public const string Amount = "[Amount]";

        public const string UserAlreadyExist = "User already exist";
        public const string NameExist = "Name already exist";
        public const string BDOCompNotExist = "Distributor Combination Not Matched in State Trader";
        public const string BDOCompExist = "Distributor Combination Already exist with Another State Trader";
        public const string UserCompExist = "Users Not Mapped, Combination Already exist with Another Reporting User or Combination not matched with Report User";
        public const string NameIsEmpty = "Name is empty";
        public const string InActiveRemarksIsEmpty = "Please select InActiveRemarks";
        public const string CodeExist = "Code already exist";
        public const string BranchNameIsEmpty = "Branch name is empty";
        public const string BranchCodeIsEmpty = "Branch code is empty";
        public const string BrandCodeIsEmpty = "Brand code is empty";
        public const string BrandNameIsEmpty = "Brand name is empty";
        public const string ProductCodeIsEmpty = "Product code is empty";
        public const string ProductNameIsEmpty = "Product name is empty";
        public const string BranchNameExist = "Branch name already exist";
        public const string BranchCodeExist = "Branch code already exist";
        public const string BrandCodeExist = "Brand code already exist";
        public const string BrandNameExist = "Brand name already exist";
        public const string ProductCodeExist = "Product code already exist";
        public const string ProductNameExist = "Product name already exist";
        public const string CustomerNameIsEmpty = "Customer name is empty";
        public const string CustomerCodeIsEmpty = "Customer code is empty";
        public const string CustomerCodeExist = "Customer code already exist";
        public const string TradeTicketNumberIsEmpty = "Trade ticket number is empty";
        public const string MaterialTypeIsEmpty = "Material Type is empty";
        public const string BookingTypeIsEmpty = "Booking Type is empty";
        public const string CityNameIsEmpty = "City is empty";
        public const string StateNameIsEmpty = "State is empty";
        public const string DistrictNameIsEmpty = "District is empty";
        public const string CityNameNotMatch = "City is not exist in DS Portal.";
        public const string StateNameNotMatch = "State is not exist in DS Portal.";
        public const string DistrictNameNotMatch = "District is not exist in DS Portal.";
        public const string DeportNotExistEmpty = "Deport{0} is not exist.So can not mapping to this customer code - {1}.";
        public const string SaudaNumberIsEmpty = "Sauda number is empty";
        public const string CountryNameIsEmpty = "Country is empty";
        public const string CountryNameNotMatch = "Country is not exist in DS Portal.";
        public const string TerritoryNameIsEmpty = "Territory is empty";
        public const string CountryNameIsMissing = "Country is missing to save state";
        public const string StateNameIsMissing = "State is missing to save territory";
        public const string TerritoryStateNameMissing = "Territory and State are mandatory to save district";
        public const string DistrictTerritoryStateNameMissing = "District, Territory and State are mandatory to save city";
        public const string DeliveryOrderNumberIsEmpty = "Delivery order number is empty";
        public const string BillingDocumentIsEmpty = "Billing document is empty";
        public const string CustomerCodeNotExist = "{0} Customer code does not exist in DS Portal. ";
        public const string PlantNotExist = "{0} Plant does not exist in DS Portal";
        public const string SaudaLimitIsEmpty = "Sauda limit quantity is not zero.";
        public const string SkuCodeIsEmpty = "Material code is empty";
        public const string SkuDetailsIsEmpty = "{0} Material code not in DS Portal. ";
        public const string UOMIsEmpty = "UOM is not zero.";
        public const string CreditLimitAndCreditExposureIsEmpty = "Credit limit and credit exposure is not zero.";
        public const string DepotCodeIsEmpty = "Depot code is empty. ";
        public const string VerticalCodeIsEmpty = "{0} Vertical code is not in DS Portal. ";
        public const string OilTypeCodeIsEmpty = "{0} Oil Type code is empty or not in DS Portal or not active in DS Portal. ";
        public const string PackTypeTypeCodeIsEmpty = "{0} Pack Type code is empty or not in DS Portal. ";
        public const string PackGroupTypeCodeIsEmpty = "{0} Pack Group is empty or not in DS Portal. ";
        public const string MaterialTypesCodeIsEmpty = "{0} Material Types is empty or not in DS Portal. ";
        public const string UOMCodeIsEmpty = "UOM is empty. ";
        public const string ConvertionTypeIsEmpty = "{0} Convertion type (UOM) is empty or not in DS Portal. ";
        public const string UOMCodeIsNotEmpty = "{0} UOM is empty or not in DS Portal. ";
        public const string SaudaNumberIsNotEmpty = "{0} SaudaNumber is empty or not in DS Portal. ";
        public const string SoldToPartyIsNotEmpty = "{0} SoldToParty is empty or not in DS Portal. ";
        public const string DepotCodeSaudaIsEmpty = "{0} Depot code is empty or not in DS Portal. ";
        public const string ForSpecificTT = " for TT ";
        public const string ShipToPartyIsNotEmpty = "{0} ShipToParty is empty or not in DS Portal. ";
        public const string DONumberIsNotEmpty = "{0} DO Number is empty or not in DS Portal. ";
        public const string ContractTypesIsEmpty = "{0} Contract Types is empty or not in DS Portal. ";
        public const string MaterialTypesIsEmpty = "{0} Material Types is empty or not in DS Portal. ";
        public const string BookingTypesIsEmpty = "{0} BookingTypes is empty or not in DS Portal. ";
        public const string SaudaBookingTypeIsEmpty = "{0} Sauda booking type is not mapping to user in DS Portal. ";
        public const string TradeTicketIsEmpty = "{0} Trade Ticket is empty or not in DS Portal. ";
        public const string FreightZoneExist = "FreightZone already exist";
        public const string FreightRouteExist = "FreightRoute already exist";
        public const string InvoiceNumberDeleteIsNotEmpty = "{0} Invoice Number is empty or not in DS Portal.So We cannot delete Invoice";
        public const string TransportModeIsEmpty = "{0} TransportMode is not mapping to user in DS Portal.";
        public const string StatusIsEmpty = "Status is empty";
        public const string InvoiceNumberNotFound = "{0} Invoice Number not found";
        public const string LiftingRequestDetailIdNotFound = "{0} Lifting Request Detail Id not found";
        public const string EnquiryNumberNotFound = "{0} Enquiry Number not found in DS Portal.";
        public const string CustomerGroupIsEmpty = "CustomerGroup is empty";
        public const string BiddingWindowMovedToInprogressState = "Bidding Window has gone to Inprogress State";
        public const string IncoTermsIsEmpty = "{0} IncoTerms is empty or not in DS Portal. ";
        public const string BatchNumberIsNotEmpty = "{0} Batch Number is empty or not in DS Portal(Invoice Number {1}). ";
        public const string CustomerGroupInSaudaIsEmpty = "{0} This Sauda not sync to SFTP because of CustomerGroup is not mapped to Dealer(Code : {1}) ";
        public const string VerticalIsEmpty = "{0}  Vertical is empty or not in DS Portal. ";
        public const string InquiryNumberIsNotEmpty = "{0} Inquiry Number is empty or not in DS Portal. ";
        public const string CustomerGroupNotMapped = "Customer group is not mapped for corresponding dealer";
        public const string SubCategoryIsEmpty = "{0} Sub Category is empty or not in DS Portal. ";
        public const string PackSizeIsEmpty = "{0} Pack Size is empty or not in DS Portal. ";
        public const string SalesOrganisationIsEmpty = "{0} Sales Organisation is empty or not in DS Portal. ";
        public const string DistributionChannelIsEmpty = "{0} Distribution Channel is empty or not in DS Portal. ";
        public const string DivisionIsEmpty = "{0} Division is empty or not in DS Portal. ";
        public const string SAPDocumentNoIsEmpty = "SAP Documnet Number is empty";
        public const string InvoiceIsEmpty = "Invoice number is empty";
        public const string SapDocumentNoIsEmpty = "Sap document no is empty";
        public const string BrokerIsNotEmpty = "{0} Broker is empty or not in DS Portal. ";
        public const string RefNumberIsEmpty = "Reference Number is empty or null";
        public const string UserCodeIsEmpty = "User code is empty";
        public const string SAPDeliveryNoIsMissing = "{0} SAP delivery no is empty";
        public const string OpenContractQuantityIsZero = "{0} {1} {2} Open contract Quantity is zero";
        public const string SkuIsNotInPortal = "Sauda Number:{0} SkuCode: {1} , {2} , {3} , {4} , {5} Sku is not in portal";
        public const string OpenContractListIsEmpty = "Open contract list is empty";



        //Sales Tour Plan
        public const string JourneyPlanIdMissing = "Journey plan id missing";
        public const string JourneyPlanNotFound = "Journey plan not found";
        public const string YearIsEmpty = "Year is empty";
        public const string EmptyFromMonth = "From Date Empty";
        public const string EmptyToMonth = "To Date Empty";
        public const string YearExist = "Year already exists";
        public const string PJPDetailsEmpty = "Permanent Journey Plan Details are Empty";
        public const string PJPApprovalFlowEmpty = "Permanent Journey Plan Approval Flow is Empty";
        public const string MTPNoVisitPerDay = "Only one no visit (staying in headquarters) per day allowed";
        public const string MTPDetailsEmpty = "Monthly Tour Plan Details are Empty";
        public const string MTPApprovalFlowEmpty = "Monthly Tour Plan Approval Flow is Empty";
        public const string MTPAlreadyExists = "Monthly Tour Plan already exists for the date(s) - [MTP_EXISTING_DATES]";
        public const string MTPExistingDates = "[MTP_EXISTING_DATES]";
        public const string IdEmpty = "Id is empty";
        public const string CompetitorDetailsEmpty = "Competitor Details are Empty";
        public const string OiltypeTargetDetailsEmpty = "User Oiltype Target Details are Empty";
        public const string UCSTargetDetailsEmpty = "User Customer Sales Target Details are Empty";
        public const string DealerDetailsEmpty = "Dealer Details are Empty";
        public const string PJPDataAlreadyExiststhisDate = "Permanent Journey Plan already exists in this date range.";
        public const string MTPalreadyApproved = "Monthly Tour Plan already approved for this date range.";
        public const string PJPIdMissing = "Permanent coverage plan missing";
        public const string PJPNotFound = "Permanent coverage plan not found";
        public const string MTPIdMissing = "Monthly tour plan missing";
        public const string MTPNotFound = "Monthly tour plan not found";
        public const string MTPDeviationNotFound = "Monthly tour plan deviation not found";
        public const string DiscountQuantityOver = "Quantity allocated with discount is over, so you can't apply your discount now.";


        public const string BenefitExist = "Benefit already exist";
        public const string BenefitIsEmpty = "Benefit is empty";
        public const string BenefitCategoryIsEmpty = "Benefit Category is empty";

        //Notification 
        public static readonly string FromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
        public static readonly string AWSEmailAccessKey = ConfigurationManager.AppSettings["AWSAccessKey"];
        public static readonly string AWSEmailSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
        public static readonly string BucketName = ConfigurationManager.AppSettings["Bucketname"];
        public static readonly string applicationARNForPushNotification = ConfigurationManager.AppSettings["applicationARNForPushNotification"];
        public static readonly string AWSRegionName = ConfigurationManager.AppSettings["AWSRegionName"];
        public static readonly string AWSTopic = ConfigurationManager.AppSettings["AWSTopic"];
        public const string EmailSendSuccessfully = "Email Sent Successfully";
        public const string EmailSendError = "Email Sent Error";
        public static string SapDataSyncEmailIds = ConfigurationManager.AppSettings["SapDataSyncEmailIds"];
        public static string SapDataSyncMobileNumbers = ConfigurationManager.AppSettings["SapDataSyncMobileNumbers"];
        public static string ServiceNotificationEmailIds = ConfigurationManager.AppSettings["ServiceNotificationEmailIds"];
        public static string ServiceNotificationMobileNumbers = ConfigurationManager.AppSettings["ServiceNotificationMobileNumbers"];
        public static readonly string CCEmail = ConfigurationManager.AppSettings["CCEmail"].ToString();

        //Smtp Credentials
        public static string SmtpHostServerName = ConfigurationManager.AppSettings["SmtpHostServerName"];
        public static string SmtpNetworkCredentialUserName = ConfigurationManager.AppSettings["SmtpNetworkCredentialUserName"];
        public static string SmtpNetworkCredentialPassword = ConfigurationManager.AppSettings["SmtpNetworkCredentialPassword"];
        public static int SmtpNetworkCredentialPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpNetworkCredentialPort"]);
        public static string SmtpFromMailAddress = ConfigurationManager.AppSettings["SmtpFromMailAddress"];
        public static bool SmtpEnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SmtpEnableSsl"]);

        public const string DataSyncEmail = "DataSyncEmail";
        public const string DataSyncSms = "DataSyncSMS";
        public const string DataSyncValue = "[DATA_RESULT]";
        public const string DataSyncEmailSubject = "SAP Data Sync";
        public const string FromDisplayName = "No Reply";

        //Sms Credentials
        public static string SmsCodeZUserName = ConfigurationManager.AppSettings["SmsCodeZUserName"];
        public static string SmsCodeZPassword = ConfigurationManager.AppSettings["SmsCodeZPassword"];
        public static string PEID = ConfigurationManager.AppSettings["PEID"];
        public static string SmsSenderId = ConfigurationManager.AppSettings["SmsSenderId"];
        public static string SmsAuthKey = ConfigurationManager.AppSettings["SmsAuthKey"];

        public static string SmsSecretKey = ConfigurationManager.AppSettings["SmsSecretKey"];
        public static string SmsGatewayKey = ConfigurationManager.AppSettings["SmsGatewayKey"];
        public static string Smsentity_id = ConfigurationManager.AppSettings["Smsentity_id"];
        public static string SmsSenderTxt = ConfigurationManager.AppSettings["SmsSender"];
        public static string SmsOtpUrl = ConfigurationManager.AppSettings["SmsOTPUrl"];

        public static readonly bool AwsEmail = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSEmail"].ToString());

        public static readonly bool IsFinalPriceGenerateOld = Convert.ToBoolean(ConfigurationManager.AppSettings["IsFinalPriceGenerateOld"]);

        //Quarter
        public static readonly string Quarter1 = "4,5,6";
        public static readonly string Quarter2 = "7,8,9";
        public static readonly string Quarter3 = "10,11,12";
        public static readonly string Quarter4 = "1,2,3";

        //Special Rate Approval
        public const string SalesOrgMissing = "SalesOrganization , DistributionalChannel , Division one of these is missing";
        public const string DealerMissing = "Dealer is missing";
        public const string DealerNotFound = "Dealer not found";
        public const string ShipToPartyNotFound = "Ship to party not found";
        public const string OilTypeMissing = "Oil type is missing";
        public const string PackTypeMissing = "pack type is missing";
        public const string OilTypeNotFound = "Oil type not found";
        public const string SKUMissing = "Material name is missing";
        public const string SKUNotFound = "Material name not found";
        public const string SKUsNotAvailable = "Materials not available";
        public const string DepotsNotAvailable = "Depots not available";
        public const string QuantityEmpty = "Quantity is empty";
        public const string RequestedPriceEmpty = "Requested price is empty";
        public const string SpecialPriceEmpty = "Special price is empty";
        public const string SpecialRateApprovalSuccess = "Special rate approval request successfully done";
        public const string SpecialRateRequestDateMissing = "Special rate approval request date is missing";
        public const string StatusMissing = "Special rate approval request status is missing";
        public const string IncotermsMissing = "Incoterms is missing";
        public const string IncotermsNotFound = "Incoterms not found";
        public const string DealerLocationMissing = "Dealer Location is missing";
        public const string DealerLocationNotFound = "Dealer Location not found";
        public const string PlantMissing = "Plant is missing";
        public const string SkuMissingInTodayPricing = "Material Missing In Today Pricing";
        public const string DepotMissing = "Depot is missing";
        public const string SkuUomIdRecordNotFound = "No matching skuUomId found";
        public const string PlantNotFound = "Plant not found";
        public const string FrieghtRouteMissing = "Frieght Route is missing";
        public const string BookingTypeMissing = "Booking type is missing";
        public const string BaseSkuEmpty = "Base Material is empty";

        public const string RetailerLocationUpdated = "Retailer Location updated";
        public const string MobileNumberExist = "Mobile number already exist";
        public const string VehicleSizeNotFound = "Vehicle Size not found";
        public const string KindlyincreaseQuantity = "Kindly increase the quantity to utilise full truck load capacity";
        public const string KindlyincreaseQuantityToFillVolume = "Kindly increase the quantity to volume capacity";
        public const string ExceededRequestedQuantity = "The requested quantity is more than vehicle capacity";
        public const string WeightLoadabilityPercentageIsZero = "Weight Loadability percentage value is zero or not configured";
        public const string VolumeLoadabilityPercentageIsZero = "Volume Loadability percentage value is zero or not configured";
        public const string CustomerRemarksNotFound = "Customer Remarks not found";

        //Updates
        public const string QuestionMissing = "Question missing";
        public const string QuestionNotFound = "Question not found";
        public const string AnswerMissing = "Answer missing";

        //Sku final price calculation
        public const string DataMissingToCalculate = "Data missing to calculate";
        public const string DataMissingToMaterialCost = "Material Cost";
        public const string DataMissingToPackingCost = "Packing Cost";
        public const string DataMissingToPrimaryFrieght = "Primary Frieght";
        public const string DataMissingToSecondaryFrieght = "Secondary Frieght";
        public const string DataMissingToSecondaryFrieghtForPlant = "Plant Secondary Frieght";
        public const string DataMissingToDepoCost = "Depo Cost";
        public const string DataMissingToDetentionCost = "Detention Cost";
        public const string DataMissingToHoneyCombCost = "HoneyComb Cost";
        public const string DataMissingToMarginCost = "Margin Cost";
        public const string DataMissingToCushionMarginCost = "Cushion Margin Cost";
        public const string DataMissingToSchemeCost = "Scheme Cost";
        public const string DataMissingToIngredientCost = "Ingredient Cost";
        public const string DataMissingToRAMarginCost = "RA Margin Cost";
        public const string DataMissingToLoadCapacity = "Load Capacity";
        public const string TransportModesMissing = "Transport modes missing";
        public const string LoadCapacityMissing = "Load capacity missing";
        public const string BaseCustomerGroupMarginMissing = "Customer Group Margin missing";
        public const string DerivedCustomerGroupMarginMissing = "Customer Group Margin missing";
        public const string DataMissingToPlantGST = "Plant GST";
        public const string DataMissingToDepotGST = "Depot GST";

        public const string MissingSkuRequiredField = "Need to update required fields";
        public const string MissingSkuQuantityField = "Pack Size";
        public const string MissingSkuPackSizeQuantityField = "Pack Size Quantity";
        public const string MissingSkuProcessCostField = "Process Cost";
        public const string MissingSkuUom2Field = "Case to number conversion";
        public const string MissingSkuUom3Field = "MT to number conversion";
        public const string SkuIngredientNotAdded = "Ingredient not added";

        public const string CostAlreadyExistsForSku = "Cost already Exists in this date for Material: ";
        public const string VerticalCodeEmpty = "Vertical code is empty. ";
        public const string VerticalNotExists = "Vertical not exists. ";
        public const string SaudaBookingTypeEmpty = "Sauda BookingType is empty. ";
        public const string SaudaBookingTypeNotExists = "Sauda BookingType not exists. ";

        //Bidding Window
        public const string BiddingWindowProcessingError = " : Bidding window already processing";
        public const string BiddingWindowisMissing = "Bidding Window is Missing";


        //Today Activities
        public const string InvalidDate = "Invalid Date";

        //Notification
        public const string NotificationSauda = "Sauda";
        public const string NotificationIndent = "Indent";
        public const string NotificationCounterBid = "CounterBid";
        public const string NotificationSaudaLimit = "Sauda Limit";
        public const string NotificationSpecialRate = "Special Rate";
        public const string RasoiOilTypeId = "RasoiOilTypeId";

        public const string SaudaAllocationTimeFormat = "{0:hh:mm tt}";

        //Sauda Conversion Sku CR
        public const string SaudaConversionDetailsMissing = "Sauda conversion details missing";
        public const string SaudaConversionSkuSaved = "Sauda Conversion Material details saved successfully";
        public const string SaudaConversionSkuIdMissing = "Sauda Conversion Id for Material missing";
        public const string SaudaConversionSkuIdNotfound = "Sauda Conversion Id for Material found";

        //Complaint Management System
        public const string SectionIdMissing = "Section Id is missing";
        public const string SectionNameEmpty = "Section Name empty";
        public const string SectionNameExist = "Section Name exist";
        public const string SectionSavedSuccessfully = "Section Saved Successfully";
        public const string SectionUpdatedSuccessfully = "Section Updated Successfully";

        public const string QuestionEmpty = "Question Empty";
        public const string QuestionTypeIdMissing = "QuestionType Id is missing";
        public const string QuestionAnswerOptionMissing = "Question Answer Option Missing";
        public const string AnswerOptionWrong = "Answer Option Wrong";
        public const string DuplicateAnswerOptions = "Duplicate Answer Options";
        public const string QuestionExist = "Question Exist";
        public const string QuestionSavedSuccessfully = "Question Saved Successfully";
        public const string QuestionIdMissing = "QuestionId Missing";
        public const string AnswerOptionExist = "Answer Option Exist";
        public const string QuestionUsedInForm = "Question Used In Form";
        public const string QuestionAnswerOptionDeleted = "Question Answer Option Deleted";

        public const string FormNameEmpty = "Form name is empty";
        public const string FormQuestionsEmpty = "Form Questions are empty";
        public const string FormNameExist = "Form name already exist";
        public const string FormSavedSuccessfully = "Form saved successfully";
        public const string FormSubmittedSavedSuccessfully = "Form Submitted successfully";
        public const string FormUpdatedSuccessfully = "Form updated successfully";
        public const string FormIdMissing = "Form Id is missing";
        public const string SubmittedFormIdMissing = "Submitted form Id is missing";
        public const string SubmittedFormCannotBeEdited = "Submitted form cannot be edited";
        public const string FormsNotAssignedToUser = "Forms Not Assigned To User";
        public const string StartIndexExceedTotalRecords = "Start index should be less than total records";

        public const string DemoDateEmpty = "Demo date choosen is empty";
        public const string DemoDateCannotBePast = "Demo date cannot be past date";
        public const string PastDemoCannotBeEdited = "Past scheduled demo cannot be edited, schedule new demo to continue.";
        public const string AlreadyActiveDemoPresent = "Already an active Demo Present for this complaint";
        public const string DemoScheduledSuccessfully = "Demo Scheduled Successfully";
        public const string DemoDataEmpty = "Demo details missing";
        public const string DemoScheduleUpdatedSuccessfully = "Demo Schedule Updated Successfully";
        public const string DemoIdMissing = "Demo Id is missing";
        public const string TripNotCreated = "Trip not created/found.";
        public const string DemoCompleted = "Demo is completed already and UnderstandingForm is submitted on ";

        //AccountSatement
        public const string InvalidAccountSatementId = "Invalid Account statement id";
        public const string AccountSatementStatus = "Status updated successfully";

        //SuadaConditionalBookingConfigration
        public const string SaudaConditionalBookingConfigAddSuccessfully = "Sauda Conditional Booking Configuration Data Saved Successfully";
        public const string SaudaConditionalBookingConfigUpdateSuccessfully = "Sauda Conditional Booking Configuration Data Updated Successfully";
        public const string MandatorySkusNotFoundOnBooking = "Mandatory SKUs are not included in the booking. Please ensure they are added before proceeding.";
        public const string MandatorySkusNotMatch = "The selected SKUs do not match the configured mandatory SKUs.";
        public const string MandatoryQuantityNotExits = "Quantity not found for mandatory SKU {0}";
        public const string MandatoryQuantityNotMatch = "Mandatory SKU {0} quantity is below the configured {1}% threshold.";


        public const string Yes = "Yes";
        public const string No = "No";
        //CSV Files
        //Csv name
        public const string BrokerCsv = "BROK_MAS_";
        public const string CustomerCsv = "CUST_MAS_";
        public const string PartnerFunctionBroker = "AG";
        public const string PartnerFunctionCustomer = "RE";

        public const string CsvDelimiter = "#";
        public const char SapDelimiter = '\t';

        public static int[] OutstandingSaudaStatus = new int[] { (int)DTO.Enums.Status.Pending, (int)DTO.Enums.Status.Approved, (int)DTO.Enums.Status.WaitingForConfirmation };
        public static int[] OverallSaudaStatus = new int[] { (int)DTO.Enums.Status.Pending, (int)DTO.Enums.Status.Approved, (int)DTO.Enums.Status.WaitingForConfirmation, (int)DTO.Enums.Status.Completed };

        //SKU Final Price
        public const string DepotNotMappingThisPlant = "Depot not mapping to this plant";

        //Lifting Request Mail Datetime Format
        public const string LiftingRequestMailDatetimeFormat = "{0:dd.MM.yyyy hh:mm tt}";

        //Darvin
        public const string ActiveUserAPIKey = "8e721206f456eb25dfe62f687a798a1ebdba6f8f5183fc62dd4608c670b0adbe55b1239b1e87dbbf3d2390cf534eb39dff477216490a3db5a75912a0f0e4fde6";
        public const string ActiveUserDatasetKey = "dcadbfa39ae97b67d21f812db17639ab9ddabab9967d9992fb1ed109e171f43918cff60546c5abe95d7d1bc3e764cb05ae4c244a8b10481cf78eb16f70852cbd";

        public const string InActiveUserAPIKey = "b593947666399100f3bda7236b230edff053bd07d9a352f4d4639e1ac487431a5c9e4c6524ec85e9235712a9feacae3257fb8ab6e137227d1ab73fdd72619b66";
        public const string InActiveUserDatasetKey = "e421768ee554649dda24a561a3d3991eca8fb8f22fe6cc74b4fb72b5c40daace8584020ecb93e22086eed2669a229f1ec4673d6761d1a8673f45d7f58718a0d4";
        public const string ExistingCombination = "QPS Discount Combination Already Exists";
        public const string OilTypeEmpty = "Oil Type is Empty";
        public const string DateRangeInvalid = "Date Range Invalid";
        public const string StateIdEmpty = "State is Empty";
        public const string SkuIdEmpty = "SkuId is Empty";
        public const string ZoneEmpty = "State is Empty";
        public const string QPSdiscountNotFound = "QPSdiscount Not Found";
        public const string QPSDiscountNotAvailable = "QPS Discount not available.";
        public const string SlabEmpty = "Slab Details is empty";

        //GamificationDashboard
        public const string GamificationDashboard = "Gamification Dashboard successfully done";
        public const string DistributorCodeExist = "DistributorCode already exist";
        public const string TannumberalreadyExist = "TAN Number already exist";
        public const string TannumberExist = "TAN Number already exist for . {0}";
        public const string AccountStatementDays = "AccountStatementDays";
        public const string AccountStatementHitCount = "AccountStatementHitCount";
        public const string GeographyDiscountInprogress= "In-Progress";

        public static string GetMessage(string errorCode, string language)
        {
            if (string.IsNullOrEmpty(language) || language != "EN" || language != "en" || language != "TA" ||
                language != "ta")
                language = "EN";
            switch (language)
            {
                default:
                    return GetMessage(errorCode + "English");
                case "TA":
                case "ta":
                    return GetMessage(errorCode + "Tamil");
            }
        }

        public static string BindErrorMessage(string errorMessage, string messages)
        {
            if (string.IsNullOrEmpty(messages))
            {
                messages = errorMessage;
            }
            else
            {
                messages = string.Concat(messages, " - ", errorMessage);
            }
            return messages;
        }


        private static string GetMessage(string errorCode)
        {
            switch (errorCode)
            {
                case "gmsP100English":
                    return "No record found";
                case "gmsP100Tamil":
                    return "No record found";
                case "gmsP101English":
                    return "Record saved successfully";
                case "gmsP101Tamil":
                    return "Record saved successfully";
                case "gmsE000English":
                    return "Internal Server Error";
                case "gmsE001English":
                    return "Not a valid client key";
                case "gmsE002English":
                    return "Client key cannot be empty";
                case "gmsE004Englisg":
                    return "Mobile number is empty";
                case "gmsE004Tamil":
                    return "Mobile number is empty";
                case "gmsE005English":
                    return "User already exist";
                case "gmsE005Tamil":
                    return "User already exist";
                case "gmsE006English":
                    return "Please select Country";
                case "gmsE006Tamil":
                    return "Please select Country";
                case "gmsE008English":
                    return "Email id empty";
                case "gmsE008Tamil":
                    return "Email id empty";
                case "gmsE009English":
                    return "Name is empty";
                case "gmsE009Tamil":
                    return "Name is empty";
                case "gmsE012English":
                    return "Password is empty";
                case "gmsE012Tamil":
                    return "Password is empty";
                case "gmsE013English":
                    return "Invalid request";
                case "gmsE013Tamil":
                    return "Invalid request";
                case "gmsE014English":
                    return "User not found";
                case "gmsE014Tamil":
                    return "User not found";
                case "gmsE015English":
                    return "Invalid OTP number";
                case "gmsE015Tamil":
                    return "Invalid OTP number";
                case "gmsE016English":
                    return "User id missing";
                case "gmsE016Tamil":
                    return "User id missing";
                case "gmsE116English":
                    return "This is prime Role cannot be deleted";
                case "gmsE116Tamil":
                    return "This is prime Role cannot be deleted";
                case "gmsE017English":
                    return "Not authorized";
                case "gmsE017Tamil":
                    return "Not authorized";
                case "gmsE018English":
                    return "Invalid login credential";
                case "gmsE018Tamil":
                    return "Invalid login credential";
                case "gmsE019English":
                    return "User is inactive";
                case "gmsE019Tamil":
                    return "User is inactive";
                case "gmsE020English":
                    return "No role to the user";
                case "gmsE020Tamil":
                    return "No role to the user";
                case "gmsE021English":
                    return "Invalid Super admin mobile number";
                case "gmsE021Tamil":
                    return "Invalid Super admin mobile number";
                case "gmsE022English":
                    return "Invalid Admin mobile number";
                case "gmsE022Tamil":
                    return "Invalid Admin mobile number";
                case "gmsE023English":
                    return "Password has been sent to your registered email address";
                case "gmsE023Tamil":
                    return "Password has been sent to your registered email address";
                case "gmsE024English":
                    return "Password has been sent to your registered mobile number";
                case "gmsE024Tamil":
                    return "Password has been sent to your registered mobile number";
                case "gmsE025English":
                    return "OTP Pending user, please login and verify OTP";
                case "gmsE025Tamil":
                    return "OTP Pending user, please login and verify OTP";
                case "gmsE026English":
                    return "No record found";
                case "gmsE026Tamil":
                    return "No record found";
                case "gmsE028English":
                    return "Role name is empty";
                case "gmsE028Tamil":
                    return "Role name is empty";
                case "gmsE050English":
                    return "Claim name is empty";
                case "gmsE050Tamil":
                    return "Claim name is empty";
                case "gmsE029English":
                    return "Role type is empty";
                case "gmsE029Tamil":
                    return "Role type is empty";
                case "gmsE049English":
                    return "Role name already exist";
                case "gmsE049Tamil":
                    return "Role name already exist";
                case "gmsE051English":
                    return "Claim name already exist";
                case "gmsE051Tamil":
                    return "Claim name already exist";
                case "gmsE052English":
                    return "Role type name already exist";
                case "gmsE052Tamil":
                    return "Role type name already exist";
                case "gmsE053English":
                    return "Users are associated with '{0}' role. Cannot be deleted this time";
                case "gmsE053Tamil":
                    return "Users are associated with '{0}' role. Cannot be deleted this time";
                case "gmsE054English":
                    return "Role not found";
                case "gmsE054Tamil":
                    return "Role not found";
                case "gmsE055English":
                    return "Role type not found";
                case "gmsE055Tamil":
                    return "Role type not found";
                case "gmsE056English":
                    return "Users are associated with '{0}' role type. Cannot be deleted this time";
                case "gmsE056Tamil":
                    return "Users are associated with '{0}' role type. Cannot be deleted this time";
                case "gmsE110English":
                    return "Invalid user";
                case "gmsE110Tamil":
                    return "Invalid user";
                case "gmsE112English":
                    return "Roletype claim is associated with '{0}' role claim. Cannot be deleted this time";
                case "gmsE112Tamil":
                    return "Roletype claim is associated with '{0}' role claim. Cannot be deleted this time";
                case "gmsE113English":
                    return "This is prime Roletype cannot be deleted";
                case "gmsE113Tamil":
                    return "This is prime Roletype cannot be deleted";
                case "gmsE114English":
                    return "Roletype is associated with '{0}' role. Cannot be deleted this time";
                case "gmsE114Tamil":
                    return "Roletype is associated with '{0}' role. Cannot be deleted this time";
            }
            return string.Empty;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}
