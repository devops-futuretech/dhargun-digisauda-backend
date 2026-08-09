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
    [RoutePrefix("api/NationalHead")]
    public class NationalHeadController : BaseApiController
    {
        private const string ServiceName = "National Head Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly INationalHeadService _nationalHeadService;
        private string _methodName;

        public NationalHeadController(INationalHeadService nationalHeadService) : base(ServiceName)
        {
            _methodName = "National Head Controller";
            try
            {
                _nationalHeadService = nationalHeadService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        [HttpPost]
        [Route("chart/saudatarget")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DashboardWeekwiseOverallSauda", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DashboardWeekwiseOverallSauda([FromBody]string inputKey)
        {
            _methodName = "DashboardWeekwiseOverallSauda";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _nationalHeadService.DashboardWeekwiseOverallSauda(x); });
        }
        [HttpPost]
        [Route("chart/salestarget")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DashboardWeekwiseOverallSales", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DashboardWeekwiseOverallSales([FromBody]string inputKey)
        {
            _methodName = "DashboardWeekwiseOverallSales";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _nationalHeadService.DashboardWeekwiseOverallSales(x); });
        }

        [HttpPost]
        [Route("chart/saudatarget/overall")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DashboardOverallSauda", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DashboardOverallSauda([FromBody]string inputKey)
        {
            _methodName = "DashboardOverallSauda";
            return Result(inputKey, _methodName, (DashboardOverallSaudaInputDto x) => { return _nationalHeadService.DashboardOverallSauda(x); });
        }

        [HttpPost]
        [Route("chart/salestarget/overall")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DashboardOverallSales", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DashboardOverallSales([FromBody]string inputKey)
        {
            _methodName = "DashboardOverallSales";
            return Result(inputKey, _methodName, (DashboardOverallSaudaInputDto x) => { return _nationalHeadService.DashboardOverallSales(x); });
        }

        [HttpPost]
        [Route("ZH/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetZonalHeadList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetZonalHeadList([FromBody]string inputKey)
        {
            _methodName = "GetZonalHeadList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _nationalHeadService.GetZonalHeadList(x); });
        }

        [HttpPost]
        [Route("statistics")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetZHStatistics", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetZHStatistics([FromBody]string inputKey)
        {
            _methodName = "GetZHStatistics";
            return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _nationalHeadService.GetZHStatistics(x); });
        }

        [HttpPost]
        [Route("duefortomorrow/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DueForTomorrowList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DueForTomorrowList([FromBody]string inputKey)
        {
            _methodName = "DueForTomorrowList";
            return Result(inputKey, _methodName, (LoginNHId x) => { return _nationalHeadService.DueForTomorrowList(x); });
        }

        [HttpPost]
        [Route("oiltypewise/salestarget/chart")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "OverallSalesChart", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult OverallSalesChart([FromBody]string inputKey)
        {
            _methodName = "OverallSalesChart";
            return Result(inputKey, _methodName, (DashboardOverallSaudaInputDto x) => { return _nationalHeadService.OverallSalesChart(x); });
        }

        [HttpPost]
        [Route("specialraterequest/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialRateRequestList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialRateRequestList([FromBody]string inputKey)
        {
            _methodName = "GetSpecialRateRequestList";
            return Result(inputKey, _methodName, (SpecialRateInputDto x) => { return _nationalHeadService.GetSpecialRateRequestList(x); });
        }

        [HttpPost]
        [Route("bookedsauda/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBookedSauda", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBookedSauda([FromBody]string inputKey)
        {
            _methodName = "GetBookedSauda";
            return Result(inputKey, _methodName, (LoginNHId x) => { return _nationalHeadService.GetBookedSauda(x); });
        }

        [HttpPost]
        [Route("liftingrequest/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetLiftingRequestCountList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetLiftingRequestCountList([FromBody]string inputKey)
        {
            _methodName = "GetLiftingRequestCountList";
            return Result(inputKey, _methodName, (LiftingRequestListInputDto x) => { return _nationalHeadService.GetLiftingRequestCountList(x); });
        }

        [HttpPost]
        [Route("packwise/salestarget/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DashboardPackwiseSaleslist", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DashboardPackwiseSaleslist([FromBody]string inputKey)
        {
            _methodName = "DashboardPackwiseSaleslist";
            return Result(inputKey, _methodName, (DashboardOverallSaudaInputDto x) => { return _nationalHeadService.DashboardPackwiseSaleslist(x); });
        }

        [HttpPost]
        [Route("stp/chart")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SalesTourPlanChart", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SalesTourPlanChart([FromBody]string inputKey)
        {
            _methodName = "SalesTourPlanChart";
            return Result(inputKey, _methodName, (SalesTourPlanInputDto x) => { return _nationalHeadService.SalesTourPlanChart(x); });
        }

        [HttpPost]
        [Route("secondarysales/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSecondarySalesFortheDay", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSecondarySalesFortheDay([FromBody]string inputKey)
        {
            _methodName = "GetSecondarySalesFortheDay";
            return Result(inputKey, _methodName, (LoginZHId x) => { return _nationalHeadService.GetSecondarySalesFortheDay(x); });
        }

        [HttpPost]
        [Route("pendingsauda/chart")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingSaudaChartForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingSaudaChartForMobile([FromBody]string inputKey)
        {
            _methodName = "GetPendingSaudaChartForMobile";
            return Result(inputKey, _methodName, (LoginZHId x) => { return _nationalHeadService.GetPendingSaudaChartForMobile(x); });
        }

        [HttpPost]
        [Route("salestarget/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "OverallSaleslistByDealers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult OverallSaleslistByDealers([FromBody]string inputKey)
        {
            _methodName = "OverallSaleslistByDealers";
            return Result(inputKey, _methodName, (DashboardOverallSaudaInputDto x) => { return _nationalHeadService.OverallSaleslistByDealers(x); });
        }

        [HttpPost]
        [Route("specialityfat/quantitylimit/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialityFatQuantityLimitList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialityFatQuantityLimitList([FromBody]string inputKey)
        {
            _methodName = "GetSpecialityFatQuantityLimitList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _nationalHeadService.GetSpecialityFatQuantityLimitList(x); });
        }

        [HttpPost]
        [Route("specialityfat/assignedquantitylimit/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetAssignedSpecialityFatQuantityLimitList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetAssignedSpecialityFatQuantityLimitList([FromBody]string inputKey)
        {
            _methodName = "GetAssignedSpecialityFatQuantityLimitList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _nationalHeadService.GetAssignedSpecialityFatQuantityLimitList(x); });
        }

        [HttpPost]
        [Route("specialityfat/quantitylimit/assign")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AssignSpecialityFatQuantityLimit", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AssignSpecialityFatQuantityLimit([FromBody]string inputKey)
        {
            _methodName = "AssignSpecialityFatQuantityLimit";
            return Result(inputKey, _methodName, (SpecialityFatEmployeeDiscountDto x) => { return _nationalHeadService.AssignSpecialityFatQuantityLimit(x); });
        }

        [HttpPost]
        [Route("specialityfat/assignedquantitylimit/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateAssignedSpecialityFatQuantityLimit", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateAssignedSpecialityFatQuantityLimit([FromBody]string inputKey)
        {
            _methodName = "UpdateAssignedSpecialityFatQuantityLimit";
            return Result(inputKey, _methodName, (SpecialityFatDiscountUserDto x) => { return _nationalHeadService.UpdateAssignedSpecialityFatQuantityLimit(x); });
        }

        [HttpPost]
        [Route("specialityfat/quantitylimit/request")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddSpecialtyFatQuantityRequests", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddSpecialtyFatQuantityRequests([FromBody]string inputKey)
        {
            _methodName = "AddSpecialtyFatQuantityRequests";
            return Result(inputKey, _methodName, (SpecialtyFatQuantityRequestDto x) => { return _nationalHeadService.AddSpecialtyFatQuantityRequests(x); });
        }

        [HttpPost]
        [Route("specialityfat/quantitylimitrequest/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialtyFatQuantityRequestsListByOrganizationReportingToId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialtyFatQuantityRequestsListByOrganizationReportingToId([FromBody]string inputKey)
        {
            _methodName = "GetSpecialtyFatQuantityRequestsListByOrganizationReportingToId";
            return Result(inputKey, _methodName, (SpecialtyFatQuantityRequestSearchDto x) => { return _nationalHeadService.GetSpecialtyFatQuantityRequestsListByOrganizationReportingToId(x); });
        }

        [HttpPost]
        [Route("specialityfat/requestedquantitylimit/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialtyFatQuantityRequestsList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialtyFatQuantityRequestsList([FromBody]string inputKey)
        {
            _methodName = "GetSpecialtyFatQuantityRequestsList";
            return Result(inputKey, _methodName, (SpecialtyFatQuantityRequestSearchDto x) => { return _nationalHeadService.GetSpecialtyFatQuantityRequestsList(x); });
        }

        [HttpPost]
        [Route("specialityfat/quantitylimit/approval")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateSpecialtyFatQuantityLimit", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateSpecialtyFatQuantityLimit([FromBody]string inputKey)
        {
            _methodName = "UpdateSpecialtyFatQuantityLimit";
            return Result(inputKey, _methodName, (SpecialtyFatQuantityRequestDto x) => { return _nationalHeadService.UpdateSpecialtyFatQuantityLimit(x); });
        }
        [HttpPost]
        [Route("discount/multiselect/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetMultiselectDiscountList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetMultiselectDiscountList([FromBody]string inputKey)
        {
            _methodName = "GetMultiselectDiscountList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _nationalHeadService.GetMultiselectDiscountList(x); });
        }

        [HttpPost]
        [Route("premium/multiselect/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetMultiselectPremiumList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetMultiselectPremiumList([FromBody]string inputKey)
        {
            _methodName = "GetMultiselectPremiumList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _nationalHeadService.GetMultiselectPremiumList(x); });
        }

        [HttpPost]
        [Route("discount/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetEmployeeAndUserDiscountList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetEmployeeAndUserDiscountList([FromBody]string inputKey)
        {
            _methodName = "GetEmployeeAndUserDiscountList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _nationalHeadService.GetEmployeeAndUserDiscountList(x); });
        }

        [HttpPost]
        [Route("premium/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPremiumList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPremiumList([FromBody]string inputKey)
        {
            _methodName = "GetPremiumList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _nationalHeadService.GetPremiumList(x); });
        }

        [HttpPost]
        [Route("assigneddiscount/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDiscountUserList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDiscountUserList([FromBody]string inputKey)
        {
            _methodName = "GetDiscountUserList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _nationalHeadService.GetDiscountUserList(x); });
        }

        [HttpPost]
        [Route("assignedpremium/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetAssignedPremiumList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetAssignedPremiumList([FromBody]string inputKey)
        {
            _methodName = "GetAssignedPremiumList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _nationalHeadService.GetAssignedPremiumList(x); });
        }

        [HttpPost]
        [Route("discount/multiselect/assign")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AssignMultiselectDiscount", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AssignMultiselectDiscount([FromBody]string inputKey)
        {
            _methodName = "AssignMultiselectDiscount";
            return Result(inputKey, _methodName, (DiscountUserDto x) => { return _nationalHeadService.AssignMultiselectDiscount(x); });
        }

        [HttpPost]
        [Route("premium/multiselect/assign")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AssignMultiselectPremium", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AssignMultiselectPremium([FromBody]string inputKey)
        {
            _methodName = "AssignMultiselectPremium";
            return Result(inputKey, _methodName, (PremiumUserDto x) => { return _nationalHeadService.AssignMultiselectPremium(x); });
        }

        [HttpPost]
        [Route("assigneddiscount/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateDiscountUsers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateDiscountUsers([FromBody]string inputKey)
        {
            _methodName = "UpdateDiscountUsers";
            return Result(inputKey, _methodName, (DiscountUserDto x) => { return _nationalHeadService.UpdateDiscountUsers(x); });
        }

        [HttpPost]
        [Route("discount/assign")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddEmployeeAndUserDiscount", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddEmployeeAndUserDiscount([FromBody]string inputKey)
        {
            _methodName = "AddEmployeeAndUserDiscount";
            return Result(inputKey, _methodName, (EmployeeUserDiscountDto x) => { return _nationalHeadService.AddEmployeeAndUserDiscount(x); });
        }

        [HttpPost]
        [Route("assignedpremium/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdatePremium", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdatePremium([FromBody]string inputKey)
        {
            _methodName = "UpdatePremium";
            return Result(inputKey, _methodName, (PremiumUserDto x) => { return _nationalHeadService.UpdatePremium(x); });
        }

        [HttpPost]
        [Route("premium/assign")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AssignPremium", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AssignPremium([FromBody]string inputKey)
        {
            _methodName = "AssignPremium";
            return Result(inputKey, _methodName, (EmployeeUserPremiumDto x) => { return _nationalHeadService.AssignPremium(x); });
        }

        [HttpPost]
        [Route("sales/statistics")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetTotalCreditLimit", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetTotalCreditLimit([FromBody]string inputKey)
        {
            _methodName = "GetTotalCreditLimit";
            return Result(inputKey, _methodName, (CreditLimitInputDto x) => { return _nationalHeadService.GetTotalCreditLimit(x); });
        }

        [HttpPost]
        [Route("PlantDepotDetailsByDealer")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ZHPlantDepotDetailsByDealer", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ZHPlantDepotDetailsByDealer([FromBody]string inputKey)
        {
            _methodName = "ZHPlantDepotDetailsByDealer";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _nationalHeadService.ZHPlantDepotDetailsByDealer(x); });
        }

        [HttpPost]
        [Route("DailyBookedSauda/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DailyBookedSaudaReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DailyBookedSaudaReport([FromBody]string inputKey)
        {
            _methodName = "DailyBookedSaudaReport";
            return Result(inputKey, _methodName, (DailyBookedSaudaInputDto x) => { return _nationalHeadService.DailyBookedSaudaReport(x); });
        }

        [HttpPost]
        [Route("specialityfat/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SpecialityFatDiscountUsersList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SpecialityFatDiscountUsersList([FromBody]string inputKey)
        {
            _methodName = "SpecialityFatDiscountUsersList";
            return Result(inputKey, _methodName, (LoginNHId x) => { return _nationalHeadService.SpecialityFatDiscountUsersList(x); });
        }

        [HttpPost]
        [Route("specialityfat/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SpecialityFatDiscountUpdate", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SpecialityFatDiscountUpdate([FromBody]string inputKey)
        {
            _methodName = "SpecialityFatDiscountUpdate";
            return Result(inputKey, _methodName, (SpecialityFatDiscountUpdateInputDto x) => { return _nationalHeadService.SpecialityFatDiscountUpdate(x); });
        }

        [HttpPost]
        [Route("SalesReport/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SalesReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SalesReport([FromBody]string inputKey)
        {
            _methodName = "SalesReport";
            return Result(inputKey, _methodName, (DailyBookedSaudaInputDto x) => { return _nationalHeadService.SalesReport(x); });
        }
        [HttpPost]
        [Route("pendingsauda/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingSaudaChartDetailForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingSaudaChartDetailForMobile([FromBody]string inputKey)
        {
            _methodName = "GetPendingSaudaChartDetailForMobile";
            return Result(inputKey, _methodName, (LoginNHId x) => { return _nationalHeadService.GetPendingSaudaChartDetailForMobile(x); });
        }
        [HttpPost]
        [Route("pendingcontract/chart")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingContractChartMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingContractChartMobile([FromBody]string inputKey)
        {
            _methodName = "GetPendingContractChartMobile";
            return Result(inputKey, _methodName, (LoginNHId x) => { return _nationalHeadService.GetPendingContractChartMobile(x); });
        }

        [HttpPost]
        [Route("specialraterequestnew/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialRateRequestListNew", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialRateRequestListNew([FromBody] string inputKey)
        {
            _methodName = "GetSpecialRateRequestListNew";
            return Result(inputKey, _methodName, (SpecialRateInputDto x) => { return _nationalHeadService.GetSpecialRateRequestListNew(x); });
        }
    }
}