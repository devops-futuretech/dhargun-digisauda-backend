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
    [RoutePrefix("api/mobileDealersales")]
    public class MobileDealerSaleController : BaseApiController
    {
        private const string ServiceName = "Mobile Dealer Sale Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IMobileDealerSalesService _dealersalesService;
        private string _methodName;

        public MobileDealerSaleController(IMobileDealerSalesService salesService) : base(ServiceName)
        {
            _methodName = "Sales Controller";
            try
            {
                _dealersalesService = salesService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }
        #region Credit Limit
        [HttpPost]
        [Route("creditlimit/total")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetTotalCreditLimit", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetTotalCreditLimit([FromBody]string inputKey)
        {
            _methodName = "GetTotalCreditLimit";
            return Result(inputKey, _methodName, (CreditLimitInputDto x) => { return _dealersalesService.GetTotalCreditLimit(x); });
        }

        [HttpPost]
        [Route("creditlimit/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCreditLimitList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCreditLimitList([FromBody]string inputKey)
        {
            _methodName = "GetCreditLimitList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _dealersalesService.GetCreditLimitList(x); });
        }
        #endregion
        [HttpPost]
        [Route("chart/overallsales")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DashboardOverallSales", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DashboardOverallSales([FromBody]string inputKey)
        {
            _methodName = "DashboardOverallSales";
            return Result(inputKey, _methodName, (DashboardOverallSaudaInputDto x) => { return _dealersalesService.DashboardOverallSales(x); });
        }

        [HttpPost]
        [Route("Performance")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "OverallPerformanceByUser", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult OverallPerformanceByUser([FromBody]string inputKey)
        {
            _methodName = "OverallPerformanceByUser";
            return Result(inputKey, _methodName, (DashboardOverallSaudaInputDto x) => { return _dealersalesService.OverallPerformanceByUser(x); });
        }

        [HttpPost]
        [Route("sku")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetTotalSkuSales", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetTotalSkuSales([FromBody]string inputKey)
        {
            _methodName = "GetTotalSkuSales";
            return Result(inputKey, _methodName, (SkuSalesFilterDto x) => { return _dealersalesService.GetTotalSkuSales(x); });
        }

        [HttpPost]
        [Route("PerformanceRankingList")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "PerformanceRankingList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult PerformanceRankingList([FromBody]string inputKey)
        {
            _methodName = "PerformanceRankingList";
            return Result(inputKey, _methodName, (DashboardOverallSaudaInputDto x) => { return _dealersalesService.PerformanceRankingList(x); });
        }

        #region Load Test

        #endregion
    }
}
