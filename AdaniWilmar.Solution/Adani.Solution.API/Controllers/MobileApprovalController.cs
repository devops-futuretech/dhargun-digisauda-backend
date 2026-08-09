using Adani.Solution.API.App_Start;
using Adani.Solution.DTO;
using Adani.Solution.Service;
using GMCore.Authenticate;
using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/mobileapproval")]
    public class MobileApprovalController : BaseApiController
    {
        private const string ServiceName = "Mobile Approval Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IMobileApprovalService _approvalService;
        private string _methodName;

        public MobileApprovalController(IMobileApprovalService approvalService) : base(ServiceName)
        {
            _methodName = "Mobile Approval Controller";
            try
            {
                _approvalService = approvalService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }
        //[HttpPost]
        //[Route("SpecialRate/ApprovalList")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetSpecialRateApprovalList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetSpecialRateApprovalList([FromBody]string inputKey)
        //{
        //    _methodName = "GetSpecialRateApprovalList";
        //    return Result(inputKey, _methodName, (SpecialRateAddInputDto x) => { return _approvalService.GetSpecialRateApprovalList(x); });
        //}

        //[HttpPost]
        //[Route("SpecialRate/details")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetSpecialRateRequestDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetSpecialRateRequestDetails([FromBody]string inputKey)
        //{
        //    _methodName = "GetSpecialRateRequestDetails";
        //    return Result(inputKey, _methodName, (SpecialRateDetailInputDto x) => { return _approvalService.GetSpecialRateRequestDetails(x); });
        //}

        [HttpPost]
        [Route("SpecialRate/approval")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SpecialRateApproval", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SpecialRateApproval([FromBody]string inputKey)
        {
            _methodName = "SpecialRateApproval";
            return Result(inputKey, _methodName, (SpecialRateApprovalDto x) => { return _approvalService.SpecialRateApproval(x); });
        }

        [HttpPost]
        [Route("specialrateapproval/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialRateApprovals", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialRateApprovals([FromBody]string inputKey)
        {
            _methodName = "GetSpecialRateApprovals";
            return Result(inputKey, _methodName, (SpecialRateAddInputDto x) => { return _approvalService.GetSpecialRateApprovals(x); });
        }

        [HttpPost]
        [Route("specialrateapproval/detail")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialRateApprovalDetail", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialRateApprovalDetail([FromBody]string inputKey)
        {
            _methodName = "GetSpecialRateApprovalDetail";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _approvalService.GetSpecialRateApprovalDetail(x); });
        }

        [HttpPost]
        [Route("specialraterequest/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialRateRequestList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialRateRequestList([FromBody]string inputKey)
        {
            _methodName = "GetSpecialRateRequestList";
            return Result(inputKey, _methodName, (SpecialRateInputDto x) => { return _approvalService.GetSpecialRateRequestList(x); });
        }

        [HttpPost]
        [Route("specialraterequestnew/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialRateRequestNewList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialRateRequestNewList([FromBody] string inputKey)
        {
            _methodName = "GetSpecialRateRequestNewList";
            return Result(inputKey, _methodName, (SpecialRateInputDto x) => { return _approvalService.GetSpecialRateRequestNewList(x); });
        }

        [HttpPost]
        [Route("specialraterequest/detail")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialRateRequestDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialRateRequestDetails([FromBody]string inputKey)
        {
            _methodName = "GetSpecialRateRequestDetails";
            return Result(inputKey, _methodName, (SpecialRateDetailInputDto x) => { return _approvalService.GetSpecialRateRequestDetails(x); });
        }

        [HttpPost]
        [Route("sauda/create/fromspecialrate")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaudaCreationFromSpecialRate", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaCreationFromSpecialRate([FromBody]string inputKey)
        {
            _methodName = "SaudaCreationFromSpecialRate";
            return Result(inputKey, _methodName, (SpecialRateSaudaDto x) => { return _approvalService.SaudaCreationFromSpecialRate(x); });
        }



        [HttpPost]
        [Route("pcp/pending/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingPermanentJourneyPlanList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingPermanentJourneyPlanList([FromBody]string inputKey)
        {
            _methodName = "GetPendingPermanentJourneyPlanList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _approvalService.GetPendingPermanentJourneyPlanList(x); });
        }

        [HttpPost]
        [Route("pcp/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPermanentJourneyPlanList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPermanentJourneyPlanList([FromBody]string inputKey)
        {
            _methodName = "GetPermanentJourneyPlanList";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _approvalService.GetPermanentJourneyPlanList(x); });
        }

        [HttpPost]
        [Route("pcp/total/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetTotalPCPByUsers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetTotalPCPByUsers([FromBody]string inputKey)
        {
            _methodName = "GetTotalPCPByUsers";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _approvalService.GetTotalPCPByUsers(x); });
        }

        [HttpPost]
        [Route("pcp/view")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPermanentJourneyPlanDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPermanentJourneyPlanDetails([FromBody]string inputKey)
        {
            _methodName = "GetPermanentJourneyPlanDetails";
            return Result(inputKey, _methodName, (PJPIdDto x) => { return _approvalService.GetPermanentJourneyPlanDetails(x); });
        }

        [HttpPost]
        [Route("pcp/approval")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "PermanentJourneyPlanApproval", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult PermanentJourneyPlanApproval([FromBody]string inputKey)
        {
            _methodName = "PermanentJourneyPlanApproval";
            return Result(inputKey, _methodName, (PermanentJourneyPlanUpdateDto x) => { return _approvalService.PermanentJourneyPlanApproval(x); });
        }

        [HttpPost]
        [Route("mtp/pending/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingMonthlyTourPlanList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingMonthlyTourPlanList([FromBody]string inputKey)
        {
            _methodName = "GetPendingMonthlyTourPlanList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _approvalService.GetPendingMonthlyTourPlanList(x); });
        }

        [HttpPost]
        [Route("mtp/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetMonthlyTourPlanList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetMonthlyTourPlanList([FromBody]string inputKey)
        {
            _methodName = "GetMonthlyTourPlanList";
            return Result(inputKey, _methodName, (MTPDateWiseDetailsInputDto x) => { return _approvalService.GetMonthlyTourPlanList(x); });
        }

        [HttpPost]
        [Route("mtp/view")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetMonthlyTourPlanDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetMonthlyTourPlanDetails([FromBody]string inputKey)
        {
            _methodName = "GetMonthlyTourPlanDetails";
            return Result(inputKey, _methodName, (MTPIdDto x) => { return _approvalService.GetMonthlyTourPlanDetails(x); });
        }

        [HttpPost]
        [Route("mtp/approval")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "MonthlyTourPlanApproval", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult MonthlyTourPlanApproval([FromBody]string inputKey)
        {
            _methodName = "MonthlyTourPlanApproval";
            return Result(inputKey, _methodName, (MonthlyTourPlanUpdateDto x) => { return _approvalService.MonthlyTourPlanApproval(x); });
        }

        [HttpPost]
        [Route("deviation/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "MonthlyPlanDeviationList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult MonthlyPlanDeviationList([FromBody]string inputKey)
        {
            _methodName = "MonthlyPlanDeviationList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _approvalService.MonthlyPlanDeviationList(x); });
        }

        [HttpPost]
        [Route("deviation/approval")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "MonthlyPlanDeviationApproval", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult MonthlyPlanDeviationApproval([FromBody]string inputKey)
        {
            _methodName = "MonthlyPlanDeviationApproval";
            return Result(inputKey, _methodName, (MonthlyPlanDeviationDto x) => { return _approvalService.MonthlyPlanDeviationApproval(x); });
        }

        [HttpPost]
        [Route("sauda/approval")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaudaApproval", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaApproval([FromBody]string inputKey)
        {
            _methodName = "SaudaApproval";
            return Result(inputKey, _methodName, (SaudaApproveInputDto x) => { return _approvalService.SaudaApproval(x); });
        }

        [HttpPost]
        [Route("pendingsauda/chart")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingSaudaChartForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingSaudaChartForMobile([FromBody]string inputKey)
        {
            _methodName = "GetPendingSaudaChartForMobile";
            return Result(inputKey, _methodName, (LoginZHId x) => { return _approvalService.GetPendingSaudaChartForMobile(x); });
        }

        [HttpPost]
        [Route("pendingsauda/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingSaudaChartDetailForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingSaudaChartDetailForMobile([FromBody]string inputKey)
        {
            _methodName = "GetPendingSaudaChartDetailForMobile";
            return Result(inputKey, _methodName, (LoginZHId x) => { return _approvalService.GetPendingSaudaChartDetailForMobile(x); });
        }

        [HttpPost]
        [Route("bookedsauda/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBookedSauda", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBookedSauda([FromBody]string inputKey)
        {
            _methodName = "GetBookedSauda";
            return Result(inputKey, _methodName, (LoginZHId x) => { return _approvalService.GetBookedSauda(x); });
        }

        [HttpPost]
        [Route("sauda/create")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaudaCreation", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaCreation([FromBody]string inputKey)
        {
            _methodName = "SaudaCreation";
            return Result(inputKey, _methodName, (SaudaInputDto x) => { return _approvalService.SaudaCreation(x); });
        }

        [HttpPost]
        [Route("sauda/details")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaorderdetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaorderdetails([FromBody]string inputKey)
        {
            _methodName = "GetSaudaorderdetails";
            return Result(inputKey, _methodName, (SaudaDetailInputDto x) => { return _approvalService.GetSaudaorderdetails(x); });
        }

        [HttpPost]
        [Route("liftingrequest/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetLiftingRequestCountList", Message = "The request has been declinCed for security reasons.", Seconds = 1)]
        public IHttpActionResult GetLiftingRequestCountList([FromBody]string inputKey)
        {
            _methodName = "GetLiftingRequestCountList";
            return Result(inputKey, _methodName, (LiftingRequestListInputDto x) => { return _approvalService.GetLiftingRequestCountList(x); });
        }

        [HttpPost]
        [Route("liftingrequest/StateTrader/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetLiftingRequestListByBDO", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetLiftingRequestListByBDO([FromBody]string inputKey)
        {
            _methodName = "GetLiftingRequestListByBDO";
            return Result(inputKey, _methodName, (LiftingRequestListInputDto x) => { return _approvalService.GetLiftingRequestListByBDO(x); });
        }

        [HttpPost]
        [Route("liftingrequest/dealerliftingrequestlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealersLiftingRequestList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealersLiftingRequestList([FromBody]string inputKey)
        {
            _methodName = "GetDealersLiftingRequestList";
            return Result(inputKey, _methodName, (DealersLiftingRequestInputDto x) => { return _approvalService.GetDealersLiftingRequestList(x); });
        }

        [HttpPost]
        [Route("liftingrequest/approval")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "LiftingRequestApproval", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult LiftingRequestApproval([FromBody]string inputKey)
        {
            _methodName = "LiftingRequestApproval";
            return Result(inputKey, _methodName, (LiftingRequestStatusChangeDto x) => { return _approvalService.LiftingRequestApproval(x); });
        }

        [HttpPost]
        [Route("discount/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetEmployeeAndUserDiscountList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetEmployeeAndUserDiscountList([FromBody]string inputKey)
        {
            _methodName = "GetEmployeeAndUserDiscountList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _approvalService.GetEmployeeAndUserDiscountList(x); });
        }

        [HttpPost]
        [Route("discount/assign")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddEmployeeAndUserDiscount", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddEmployeeAndUserDiscount([FromBody]string inputKey)
        {
            _methodName = "AddEmployeeAndUserDiscount";
            return Result(inputKey, _methodName, (EmployeeUserDiscountDto x) => { return _approvalService.AddEmployeeAndUserDiscount(x); });
        }

        [HttpPost]
        [Route("assigneddiscount/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateDiscountUsers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateDiscountUsers([FromBody]string inputKey)
        {
            _methodName = "UpdateDiscountUsers";
            return Result(inputKey, _methodName, (DiscountUserDto x) => { return _approvalService.UpdateDiscountUsers(x); });
        }

        [HttpPost]
        [Route("assigneddiscount/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDiscountUserList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDiscountUserList([FromBody]string inputKey)
        {
            _methodName = "GetDiscountUserList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _approvalService.GetDiscountUserList(x); });
        }

        [HttpPost]
        [Route("discount/multiselect/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetMultiselectDiscountList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetMultiselectDiscountList([FromBody]string inputKey)
        {
            _methodName = "GetMultiselectDiscountList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _approvalService.GetMultiselectDiscountList(x); });
        }

        [HttpPost]
        [Route("discount/multiselect/assign")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AssignMultiselectDiscount", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AssignMultiselectDiscount([FromBody]string inputKey)
        {
            _methodName = "AssignMultiselectDiscount";
            return Result(inputKey, _methodName, (DiscountUserDto x) => { return _approvalService.AssignMultiselectDiscount(x); });
        }

        [HttpPost]
        [Route("assigneddiscount/multiselect/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateMultiselectDiscountUsers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateMultiselectDiscountUsers([FromBody]string inputKey)
        {
            _methodName = "UpdateMultiselectDiscountUsers";
            return Result(inputKey, _methodName, (DiscountUserDto x) => { return _approvalService.UpdateMultiselectDiscountUsers(x); });
        }

        [HttpPost]
        [Route("assigneddiscount/multiselect/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AssignedMultiselectDiscountList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AssignedMultiselectDiscountList([FromBody]string inputKey)
        {
            _methodName = "AssignedMultiselectDiscountList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _approvalService.AssignedMultiselectDiscountList(x); });
        }

        [HttpPost]
        [Route("saudalimit/approval")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaudaLimitApproval", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaLimitApproval([FromBody]string inputKey)
        {
            _methodName = "SaudaLimitApproval";
            return Result(inputKey, _methodName, (SaudaLimitRequestInputDto x) => { return _approvalService.SaudaLimitApproval(x); });
        }

        [HttpPost]
        [Route("secondarysales/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSecondarySalesFortheDay", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSecondarySalesFortheDay([FromBody]string inputKey)
        {
            _methodName = "GetSecondarySalesFortheDay";
            return Result(inputKey, _methodName, (LoginZHId x) => { return _approvalService.GetSecondarySalesFortheDay(x); });
        }

        [HttpPost]
        [Route("sauda/conversion/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaConversionList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaConversionList([FromBody]string inputKey)
        {
            _methodName = "GetSaudaConversionList";
            return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _approvalService.GetSaudaConversionList(x); });
        }

        [HttpPost]
        [Route("sauda/conversion/approval")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaudaConversionApproval", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaConversionApproval([FromBody]string inputKey)
        {
            _methodName = "SaudaConversionApproval";
            return Result(inputKey, _methodName, (SaudaConversionApprovalInputDto x) => { return _approvalService.SaudaConversionApproval(x); });
        }

        [HttpPost]
        [Route("sauda/extension/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaExtensionList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaExtensionList([FromBody]string inputKey)
        {
            _methodName = "GetSaudaExtensionList";
            return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _approvalService.GetSaudaExtensionList(x); });
        }

        [HttpPost]
        [Route("sauda/extension/approval")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaudaExtensionApproval", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaExtensionApproval([FromBody]string inputKey)
        {
            _methodName = "SaudaExtensionApproval";
            return Result(inputKey, _methodName, (SaudaConversionApprovalInputDto x) => { return _approvalService.SaudaExtensionApproval(x); });
        }

        [HttpPost]
        [Route("premium/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPremiumList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPremiumList([FromBody]string inputKey)
        {
            _methodName = "GetPremiumList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _approvalService.GetPremiumList(x); });
        }

        [HttpPost]
        [Route("premium/assign")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AssignPremium", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AssignPremium([FromBody]string inputKey)
        {
            _methodName = "AssignPremium";
            return Result(inputKey, _methodName, (EmployeeUserPremiumDto x) => { return _approvalService.AssignPremium(x); });
        }

        [HttpPost]
        [Route("assignedpremium/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdatePremium", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdatePremium([FromBody]string inputKey)
        {
            _methodName = "UpdatePremium";
            return Result(inputKey, _methodName, (PremiumUserDto x) => { return _approvalService.UpdatePremium(x); });
        }

        [HttpPost]
        [Route("assignedpremium/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetAssignedPremiumList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetAssignedPremiumList([FromBody]string inputKey)
        {
            _methodName = "GetAssignedPremiumList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _approvalService.GetAssignedPremiumList(x); });
        }

        [HttpPost]
        [Route("premium/multiselect/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetMultiselectPremiumList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetMultiselectPremiumList([FromBody]string inputKey)
        {
            _methodName = "GetMultiselectPremiumList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _approvalService.GetMultiselectPremiumList(x); });
        }

        [HttpPost]
        [Route("premium/multiselect/assign")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AssignMultiselectPremium", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AssignMultiselectPremium([FromBody]string inputKey)
        {
            _methodName = "AssignMultiselectPremium";
            return Result(inputKey, _methodName, (PremiumUserDto x) => { return _approvalService.AssignMultiselectPremium(x); });
        }

        [HttpPost]
        [Route("specialityfat/quantitylimit/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialityFatQuantityLimitList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialityFatQuantityLimitList([FromBody]string inputKey)
        {
            _methodName = "GetSpecialityFatQuantityLimitList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _approvalService.GetSpecialityFatQuantityLimitList(x); });
        }

        [HttpPost]
        [Route("specialityfat/quantitylimit/assign")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AssignSpecialityFatQuantityLimit", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AssignSpecialityFatQuantityLimit([FromBody]string inputKey)
        {
            _methodName = "AssignSpecialityFatQuantityLimit";
            return Result(inputKey, _methodName, (SpecialityFatEmployeeDiscountDto x) => { return _approvalService.AssignSpecialityFatQuantityLimit(x); });
        }

        [HttpPost]
        [Route("specialityfat/assignedquantitylimit/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateAssignedSpecialityFatQuantityLimit", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateAssignedSpecialityFatQuantityLimit([FromBody]string inputKey)
        {
            _methodName = "UpdateAssignedSpecialityFatQuantityLimit";
            return Result(inputKey, _methodName, (SpecialityFatDiscountUserDto x) => { return _approvalService.UpdateAssignedSpecialityFatQuantityLimit(x); });
        }

        [HttpPost]
        [Route("specialityfat/assignedquantitylimit/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetAssignedSpecialityFatQuantityLimitList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetAssignedSpecialityFatQuantityLimitList([FromBody]string inputKey)
        {
            _methodName = "GetAssignedSpecialityFatQuantityLimitList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _approvalService.GetAssignedSpecialityFatQuantityLimitList(x); });
        }

        [HttpPost]
        [Route("specialityfat/quantitylimit/request")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddSpecialtyFatQuantityRequests", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddSpecialtyFatQuantityRequests([FromBody]string inputKey)
        {
            _methodName = "AddSpecialtyFatQuantityRequests";
            return Result(inputKey, _methodName, (SpecialtyFatQuantityRequestDto x) => { return _approvalService.AddSpecialtyFatQuantityRequests(x); });
        }

        [HttpPost]
        [Route("specialityfat/requestedquantitylimit/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialtyFatQuantityRequestsList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialtyFatQuantityRequestsList([FromBody]string inputKey)
        {
            _methodName = "GetSpecialtyFatQuantityRequestsList";
            return Result(inputKey, _methodName, (SpecialtyFatQuantityRequestSearchDto x) => { return _approvalService.GetSpecialtyFatQuantityRequestsList(x); });
        }

        [HttpPost]
        [Route("specialityfat/quantitylimitrequest/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialtyFatQuantityRequestsListByOrganizationReportingToId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialtyFatQuantityRequestsListByOrganizationReportingToId([FromBody]string inputKey)
        {
            _methodName = "GetSpecialtyFatQuantityRequestsListByOrganizationReportingToId";
            return Result(inputKey, _methodName, (SpecialtyFatQuantityRequestSearchDto x) => { return _approvalService.GetSpecialtyFatQuantityRequestsListByOrganizationReportingToId(x); });
        }

        [HttpPost]
        [Route("specialityfat/quantitylimit/approval")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateSpecialtyFatQuantityLimit", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateSpecialtyFatQuantityLimit([FromBody]string inputKey)
        {
            _methodName = "UpdateSpecialtyFatQuantityLimit";
            return Result(inputKey, _methodName, (SpecialtyFatQuantityRequestDto x) => { return _approvalService.UpdateSpecialtyFatQuantityLimit(x); });
        }

        [HttpPost]
        [Route("pendingcontract/chart")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingContractChartMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingContractChartMobile([FromBody]string inputKey)
        {
            _methodName = "GetPendingContractChartMobile";
            return Result(inputKey, _methodName, (LoginZHId x) => { return _approvalService.GetPendingContractChartMobile(x); });
        }

        [HttpPost]
        [Route("creditlimitandcreditexposure/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCreditLimitAndCreditExposureList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCreditLimitAndCreditExposureList([FromBody] string inputKey)
        {
            _methodName = "GetCreditLimitAndCreditExposureList";
            return Result(inputKey, _methodName, (CreditLimitAndCreditExposureInputDto x) => { return _approvalService.GetCreditLimitAndCreditExposureList(x); });
        }

        [HttpPost]
        [Route("contactlistforactivecalltocustomers")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetContactListForActiveCallToCustomers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetContactListForActiveCallToCustomers([FromBody] string inputKey)
        {
            _methodName = "GetContactListForActiveCallToCustomers";
            return Result(inputKey, _methodName, (ContactListForActiveCallInputDto x) => { return _approvalService.GetContactListForActiveCallToCustomers(x); });
        }

        [HttpPost]
        [Route("saveCallRecordingOfCustomers")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveCallRecordingOfCustomers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveCallRecordingOfCustomers([FromBody] string inputKey)
        {
            _methodName = "SaveCallRecordingOfCustomers";
            return Result(inputKey, _methodName, (ContactListForActiveCallInputDto x) => { return _approvalService.SaveCallRecordingOfCustomers(x); });
        }

        [HttpPost]
        [Route("audiofilesagainstcustomers")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetAudioFilesListAgainstCustomers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetAudioFilesListAgainstCustomers([FromBody] string inputKey)
        {
            _methodName = "GetAudioFilesListAgainstCustomers";
            return Result(inputKey, _methodName, (ContactListForActiveCallInputDto x) => { return _approvalService.GetAudioFilesListAgainstCustomers(x); });
        }

        [HttpPost]
        [Route("savesaudadetails/mappedagainstaudiofiles")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveSaudadetailsMappedAgainstAudiofiles", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveSaudadetailsMappedAgainstAudiofiles([FromBody] string inputKey)
        {
            _methodName = "SaveSaudadetailsMappedAgainstAudiofiles";
            return Result(inputKey, _methodName, (ContactListForActiveCallInputDto x) => { return _approvalService.SaveSaudadetailsMappedAgainstAudiofiles(x); });
        }

        [HttpPost]
        [Route("creditlimitandcreditexposure/list/app")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCreditLimitAndCreditExposureListAPP", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCreditLimitAndCreditExposureListAPP([FromBody] string inputKey)
        {
            _methodName = "GetCreditLimitAndCreditExposureListAPP";
            return Result(inputKey, _methodName, (CreditLimitAndCreditExposureInputDto x) => { return _approvalService.GetCreditLimitAndCreditExposureListAPP(x); });
        }

        #region Competitor

        [HttpPost]
        [Route("competitoranalysis/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCompetitorAnalysisList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCompetitorAnalysisList([FromBody] string inputKey)
        {
            _methodName = "GetCompetitorAnalysisList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _approvalService.GetCompetitorAnalysisList(x); });
        }

        [HttpPost]
        [Route("competitoranalysis/details")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCompetitorAnalysisById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCompetitorAnalysisById([FromBody] string inputKey)
        {
            _methodName = "GetCompetitorAnalysisById";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _approvalService.GetCompetitorAnalysisById(x); });
        }

        [HttpPost]
        [Route("competitoranalysisdetails/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCompetitorAnalysisDetailsListById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCompetitorAnalysisDetailsListById([FromBody] string inputKey)
        {
            _methodName = "GetCompetitorAnalysisDetailsListById";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _approvalService.GetCompetitorAnalysisDetailsListById(x); });
        }

        [HttpPost]
        [Route("competitoranalysis/approval")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveCompetitorAnalysisApproval", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveCompetitorAnalysisApproval([FromBody] string inputKey)
        {
            _methodName = "SaveCompetitorAnalysisApproval";
            return Result(inputKey, _methodName, (CompetitorAnalysisApprovalDto x) => { return _approvalService.SaveCompetitorAnalysisApproval(x); });
        }


        #endregion

        [HttpGet]
        [Route("daterange/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDateRangeList", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetDateRangeList()
        {
            _methodName = "GetDateRangeList";
            return Result(_methodName, (() => { return _approvalService.GetDateRangeList(); }));
        }

        [HttpPost]
        [Route("savedealerdetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "savedealerdetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveDealerDetails([FromBody] string inputKey)
        {
            _methodName = "SaveDealerDetails";
            return Result(inputKey, _methodName, (SaveDealerDetails x) => { return _approvalService.SaveDealerDetails(x); });
        }
    }
}
