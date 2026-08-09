using System;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net;
using System.Collections.Generic;
using GMCore.Logger;
using GMCore.Authenticate;
using GMCore.Helper;
using Adani.Solution.DTO;
using Adani.Solution.Service;
using Adani.Solution.Service.Common;
using Adani.Solution.DTO.Common;
using Adani.Solution.API.App_Start;

namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/mobileDealerDashboard")]
    public class MobileDealerDashboardController : BaseApiController
    {
        private const string ServiceName = "Mobile Dashboard Dealer Controller";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IMobileDealerDashboardServices _mobileDashboardServices;
        private string _methodName;

        public MobileDealerDashboardController(IMobileDealerDashboardServices mobileDashboardServices) : base(ServiceName)
        {
            _methodName = "SalesTourPlan Controller";
            try
            {
                _mobileDashboardServices = mobileDashboardServices;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }
        [HttpPost]
        [Route("Chart/OverallSauda")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DashboardOverallSauda", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DashboardOverallSauda([FromBody]string inputKey)
        {
            _methodName = "DashboardOverallSauda";
            return Result(inputKey, _methodName, (DashboardOverallSaudaInputDto x) => { return _mobileDashboardServices.DashboardOverallSauda(x); });
        }

        [HttpPost]
        [Route("Chart/WeekwiseOverallSauda")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DashboardWeekwiseOverallSauda", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DashboardWeekwiseOverallSauda([FromBody]string inputKey)
        {
            _methodName = "DashboardWeekwiseOverallSauda";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _mobileDashboardServices.DashboardWeekwiseOverallSauda(x); });
        }

        [HttpPost]
        [Route("Chart/OverallSales")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DashboardOverallSales", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DashboardOverallSales([FromBody]string inputKey)
        {
            _methodName = "DashboardOverallSales";
            return Result(inputKey, _methodName, (DashboardOverallSaudaInputDto x) => { return _mobileDashboardServices.DashboardOverallSales(x); });
        }

        [HttpPost]
        [Route("Chart/WeekwiseOverallSales")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DashboardWeekwiseOverallSales", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DashboardWeekwiseOverallSales([FromBody]string inputKey)
        {
            _methodName = "DashboardWeekwiseOverallSales";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _mobileDashboardServices.DashboardWeekwiseOverallSales(x); });
        }

        [HttpPost]
        [Route("SaudalistByDealers")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DashboardSaudalistByDealers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DashboardSaudalistByDealers([FromBody]string inputKey)
        {
            _methodName = "DashboardOverallSales";
            return Result(inputKey, _methodName, (DashboardOverallSaudaInputDto x) => { return _mobileDashboardServices.DashboardSaudalistByDealers(x); });
        }
        [HttpPost]
        [Route("SaleslistByDealers")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DashboardSaleslistByDealers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DashboardSaleslistByDealers([FromBody]string inputKey)
        {
            _methodName = "DashboardOverallSales";
            return Result(inputKey, _methodName, (DashboardOverallSaudaInputDto x) => { return _mobileDashboardServices.DashboardSaleslistByDealers(x); });
        }
        [HttpPost]
        [Route("packgroupwise/sales")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DashboardPackwiseSaleslist", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DashboardPackwiseSaleslist([FromBody]string inputKey)
        {
            _methodName = "DashboardPackwiseSaleslist";
            return Result(inputKey, _methodName, (DashboardSaudaDetailsByDealersInputDto x) => { return _mobileDashboardServices.DashboardPackwiseSaleslist(x); });
        }
        [HttpPost]
        [Route("SaudaDetailsByDealers")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DashboardSaudaDetailsByDealers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DashboardSaudaDetailsByDealers([FromBody]string inputKey)
        {
            _methodName = "DashboardOverallSales";
            return Result(inputKey, _methodName, (DashboardSaudaDetailsByDealersInputDto x) => { return _mobileDashboardServices.DashboardSaudaDetailsByDealers(x); });
        }
        [HttpPost]
        [Route("InvoicesByDealers")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "InvoicesByDealers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult InvoicesByDealers([FromBody]string inputKey)
        {
            _methodName = "DashboardOverallSales";
            return Result(inputKey, _methodName, (DashboardSaudaDetailsByDealersInputDto x) => { return _mobileDashboardServices.InvoicesByDealers(x); });
        }
        [HttpPost]
        [Route("InvoiceDetailsByDealers")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "InvoiceDetailsByDealers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult InvoiceDetailsByDealers([FromBody]string inputKey)
        {
            _methodName = "InvoiceDetailsByDealers";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _mobileDashboardServices.InvoiceDetailsByDealers(x); });
        }
        [HttpPost]
        [Route("DueForTomorrowList")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DueForTomorrowList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DueForTomorrowList([FromBody]string inputKey)
        {
            _methodName = "DueForTomorrowList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _mobileDashboardServices.DueForTomorrowList(x); });
        }

        [HttpGet]
        [Route("ticker/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetTickerListForToday", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetTickerListForToday()
        {
            _methodName = "GetTickerListForToday";
            return Result(_methodName, () => { return _mobileDashboardServices.GetTickerListForToday(); });
        }

        [HttpPost]
        [Route("creditnote")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCreditNote", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCreditNote([FromBody]string inputKey)
        {
            _methodName = "GetCreditNote";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _mobileDashboardServices.GetCreditNote(x); });
        }

        [HttpPost]
        [Route("accountstatement")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetAccountStatement", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetAccountStatement([FromBody]string inputKey)
        {
            _methodName = "GetAccountStatement";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _mobileDashboardServices.GetAccountStatement(x); });
        }

        [HttpPost]
        [Route("dailyrate")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDailyRate", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDailyRate([FromBody]string inputKey)
        {
            _methodName = "GetDailyRate";
            return Result(inputKey, _methodName, (DailyRateInputDto x) => { return _mobileDashboardServices.GetDailyRate(x); });
        }

        [HttpPost]
        [Route("statistics")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerStatistics", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerStatistics([FromBody]string inputKey)
        {
            _methodName = "GetDealerStatistics";
            return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _mobileDashboardServices.GetDealerStatistics(x); });
        }

        [HttpPost]
        [Route("ledger")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCustomerLedger", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCustomerLedger([FromBody]string inputKey)
        {
            _methodName = "GetCustomerLedger";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _mobileDashboardServices.GetCustomerLedger(x); });
        }

        [HttpPost]
        [Route("ledger/rolewise")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCustomerLedgerRolewise", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCustomerLedgerRolewise([FromBody] string inputKey)
        {
            _methodName = "GetCustomerLedgerRolewise";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _mobileDashboardServices.GetCustomerLedgerRolewise(x); });
        }

        [HttpPost]
        [Route("packgroupwise/invoicesByDealers")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "PackwiseInvoicesByDealer", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult PackwiseInvoicesByDealer([FromBody]string inputKey)
        {
            _methodName = "PackwiseInvoicesByDealer";
            return Result(inputKey, _methodName, (DashboardSaudaDetailsByDealersInputDto x) => { return _mobileDashboardServices.PackwiseInvoicesByDealer(x); });
        }

        [HttpPost]
        [Route("packgroupwise/InvoiceDetailsByDealers")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "PackwiseInvoiceDetailsByDealer", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult PackwiseInvoiceDetailsByDealer([FromBody]string inputKey)
        {
            _methodName = "PackwiseInvoiceDetailsByDealer";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _mobileDashboardServices.PackwiseInvoiceDetailsByDealer(x); });
        }

        [HttpPost]
        [Route("DealerPlantDepotDetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DealerPlantDepotDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult BDOPlantDepotDetailsByDealer([FromBody]string inputKey)
        {
            _methodName = "DealerPlantDepotDetails";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _mobileDashboardServices.DealerPlantDepotDetails(x); });
        }
        [HttpPost]
        [Route("dailyrate/new")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDailyRateNew", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDailyRateNew([FromBody]string inputKey)
        {
            _methodName = "GetDailyRateNew";
            return Result(inputKey, _methodName, (DailyRateInputDto x) => { return _mobileDashboardServices.GetDailyRateNew(x); });
        }
        [HttpPost]
        [Route("dailyrate/web")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDailyRateWeb", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDailyRateWeb([FromBody] string inputKey)
        {
            _methodName = "GetDailyRateWeb";
            return KendoGridResult(inputKey, _methodName, (PricePublistInputDataDto x) => { return _mobileDashboardServices.GetDailyRateWeb(x); });
        }
    }
}
