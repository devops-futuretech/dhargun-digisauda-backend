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
    [RoutePrefix("api/zh")]
    public class ZonalHeadController : BaseApiController
    {
        private const string ServiceName = "Zonal Head Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IZonalHeadService _zonalHeadService;
        private string _methodName;

        public ZonalHeadController(IZonalHeadService zonalHeadService) : base(ServiceName)
        {
            _methodName = "Zonal Head Controller";
            try
            {
                _zonalHeadService = zonalHeadService;
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
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _zonalHeadService.DashboardWeekwiseOverallSauda(x); });
        }

        [HttpPost]
        [Route("chart/salestarget")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DashboardWeekwiseOverallSales", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DashboardWeekwiseOverallSales([FromBody]string inputKey)
        {
            _methodName = "DashboardWeekwiseOverallSales";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _zonalHeadService.DashboardWeekwiseOverallSales(x); });
        }

        [HttpPost]
        [Route("chart/saudatarget/overall")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DashboardOverallSauda", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DashboardOverallSauda([FromBody]string inputKey)
        {
            _methodName = "DashboardOverallSauda";
            return Result(inputKey, _methodName, (DashboardOverallSaudaInputDto x) => { return _zonalHeadService.DashboardOverallSauda(x); });
        }

        [HttpPost]
        [Route("chart/salestarget/overall")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DashboardOverallSales", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DashboardOverallSales([FromBody]string inputKey)
        {
            _methodName = "DashboardOverallSales";
            return Result(inputKey, _methodName, (DashboardOverallSaudaInputDto x) => { return _zonalHeadService.DashboardOverallSales(x); });
        }

        [HttpPost]
        [Route("bdo/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBDOList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBDOList([FromBody]string inputKey)
        {
            _methodName = "GetBDOList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _zonalHeadService.GetBDOList(x); });
        }

        [HttpPost]
        [Route("bdo/list/fortp")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBDOListForTp", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBDOListForTp([FromBody]string inputKey)
        {
            _methodName = "GetBDOListForTp";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _zonalHeadService.GetBDOListForTp(x); });
        }

        [HttpPost]
        [Route("oiltypewise/salestarget/chart")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "OverallSalesChart", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult OverallSalesChart([FromBody]string inputKey)
        {
            _methodName = "OverallSalesChart";
            return Result(inputKey, _methodName, (DashboardOverallSaudaInputDto x) => { return _zonalHeadService.OverallSalesChart(x); });
        }

        [HttpPost]
        [Route("salestarget/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "OverallSaleslistByDealers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult OverallSaleslistByDealers([FromBody]string inputKey)
        {
            _methodName = "OverallSaleslistByDealers";
            return Result(inputKey, _methodName, (DashboardOverallSaudaInputDto x) => { return _zonalHeadService.OverallSaleslistByDealers(x); });
        }

        [HttpPost]
        [Route("packwise/salestarget/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DashboardPackwiseSaleslist", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DashboardPackwiseSaleslist([FromBody]string inputKey)
        {
            _methodName = "DashboardPackwiseSaleslist";
            return Result(inputKey, _methodName, (DashboardOverallSaudaInputDto x) => { return _zonalHeadService.DashboardPackwiseSaleslist(x); });
        }

        [HttpPost]
        [Route("performance")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "OverallPerformanceByUser", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult OverallPerformanceByUser([FromBody]string inputKey)
        {
            _methodName = "OverallPerformanceByUser";
            return Result(inputKey, _methodName, (DashboardOverallSaudaInputDto x) => { return _zonalHeadService.OverallPerformanceByUser(x); });
        }

        [HttpPost]
        [Route("performance/rank/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "PerformanceRankingList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult PerformanceRankingList([FromBody]string inputKey)
        {
            _methodName = "PerformanceRankingList";
            return Result(inputKey, _methodName, (DashboardOverallSaudaInputDto x) => { return _zonalHeadService.PerformanceRankingList(x); });
        }

        [HttpPost]
        [Route("creditlimit/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCreditLimitList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCreditLimitList([FromBody]string inputKey)
        {
            _methodName = "GetCreditLimitList";
            return Result(inputKey, _methodName, (LoginZHId x) => { return _zonalHeadService.GetCreditLimitList(x); });
        }

        [HttpPost]
        [Route("duefortomorrow/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DueForTomorrowList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DueForTomorrowList([FromBody]string inputKey)
        {
            _methodName = "DueForTomorrowList";
            return Result(inputKey, _methodName, (LoginZHId x) => { return _zonalHeadService.DueForTomorrowList(x); });
        }

        [HttpPost]
        [Route("statistics")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetZHStatistics", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetZHStatistics([FromBody]string inputKey)
        {
            _methodName = "GetZHStatistics";
            return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _zonalHeadService.GetZHStatistics(x); });
        }

        [HttpPost]
        [Route("sales/statistics")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetTotalCreditLimit", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetTotalCreditLimit([FromBody]string inputKey)
        {
            _methodName = "GetTotalCreditLimit";
            return Result(inputKey, _methodName, (CreditLimitInputDto x) => { return _zonalHeadService.GetTotalCreditLimit(x); });
        }

        [HttpPost]
        [Route("stp/chart")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SalesTourPlanChart", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SalesTourPlanChart([FromBody]string inputKey)
        {
            _methodName = "SalesTourPlanChart";
            return Result(inputKey, _methodName, (SalesTourPlanInputDto x) => { return _zonalHeadService.SalesTourPlanChart(x); });
        }

        [HttpPost]
        [Route("PlantDepotDetailsByDealer")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ZHPlantDepotDetailsByDealer", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ZHPlantDepotDetailsByDealer([FromBody]string inputKey)
        {
            _methodName = "ZHPlantDepotDetailsByDealer";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _zonalHeadService.ZHPlantDepotDetailsByDealer(x); });
        }

        [HttpPost]
        [Route("DailyBookedSauda/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DailyBookedSaudaReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DailyBookedSaudaReport([FromBody]string inputKey)
        {
            _methodName = "DailyBookedSaudaReport";
            return Result(inputKey, _methodName, (DailyBookedSaudaInputDto x) => { return _zonalHeadService.DailyBookedSaudaReport(x); });
        }
        [HttpPost]
        [Route("SalesReport/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SalesReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SalesReport([FromBody]string inputKey)
        {
            _methodName = "SalesReport";
            return Result(inputKey, _methodName, (DailyBookedSaudaInputDto x) => { return _zonalHeadService.SalesReport(x); });
        }
    }
}
