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
    [RoutePrefix("api/mobilesauda")]
    public class MobileSaudaController : BaseApiController
    {
        private const string ServiceName = "Sauda Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IMobileSaudaServices _saudaService;
        private string _methodName;

        public MobileSaudaController(IMobileSaudaServices saudaService) : base(ServiceName)
        {
            _methodName = "Sauda Controller";
            try
            {
                _saudaService = saudaService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        [HttpPost]
        [Route("DealerSaudaDetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DealerSaudaDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DealerSaudaDetails([FromBody]string inputKey)
        {
            _methodName = "DealerSaudaDetails";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _saudaService.DealerSaudaDetails(x); });
        }

        [HttpPost]
        [Route("GetDealerSaudaDetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerSaudaDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerSaudaDetails([FromBody] string inputKey)
        {
            _methodName = "GetDealerSaudaDetails";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _saudaService.GetDealerSaudaDetails(x); });
        }

        [HttpPost]
        [Route("GetSalesOrderDataDetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSalesOrderDataDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSalesOrderDataDetails([FromBody] string inputKey)
        {
            _methodName = "GetSalesOrderDataDetails";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _saudaService.GetDealerSaudaDetails(x); });
        }

        [HttpPost]
        [Route("PendingSaudaList")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingSaudaListForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingSaudaListForMobile([FromBody]string inputKey)
        {
            _methodName = "GetPendingSaudaListForMobile";
            return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _saudaService.GetPendingSaudaListForMobile(x); });
        }

        [HttpPost]
        [Route("expired/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetExpiredSaudaListForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetExpiredSaudaListForMobile([FromBody]string inputKey)
        {
            _methodName = "GetExpiredSaudaListForMobile";
            return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _saudaService.GetExpiredSaudaListForMobile(x); });
        }

        [HttpPost]
        [Route("DealerLocationsById")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerLocationsByDealerId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerLocationsByDealerId([FromBody]string inputKey)
        {
            _methodName = "GetDealerLocationsByDealerId";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _saudaService.GetDealerLocationsByDealerId(x); });
        }

        [HttpPost]
        [Route("dealerDetail")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerDetail", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerDetail([FromBody]string inputKey)
        {
            _methodName = "GetDealerDetail";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _saudaService.GetDealerDetail(x); });
        }

        /// <summary>
        /// Method to create sauda
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaudaCreation", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaCreation([FromBody]string inputKey)
        {
            _methodName = "SaudaCreation";
            return Result(inputKey, _methodName, (SaudaInputDto x) => { return _saudaService.SaudaCreation(x); });
        }

        /// <summary>
        /// Method to get sauda list
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaList([FromBody]string inputKey)
        {
            _methodName = "GetSaudaList";
            return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _saudaService.GetSaudaList(x); });
        }

        [HttpPost]
        [Route("short/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaShortViewList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaShortViewList([FromBody]string inputKey)
        {
            _methodName = "GetSaudaShortViewList";
            return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _saudaService.GetSaudaShortViewList(x); });
        }

        [HttpPost]
        [Route("short/view")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaShortViewDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaShortViewDetails([FromBody]string inputKey)
        {
            _methodName = "GetSaudaShortViewDetails";
            return Result(inputKey, _methodName, (SaudaDetailInputDto x) => { return _saudaService.GetSaudaShortViewDetails(x); });
        }

        [HttpPost]
        [Route("sku/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkuListForIndentRequest", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuListForIndentRequest([FromBody]string inputKey)
        {
            _methodName = "GetSkuListForIndentRequest";
            return Result(inputKey, _methodName, (SkuInputDto x) => { return _saudaService.GetSkuListForIndentRequest(x); });
        }

        [HttpPost]
        [Route("skulist/basedonvehiclesize")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkuListBasedOnVehicleSize", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuListBasedOnVehicleSize([FromBody] string inputKey)
        {
            _methodName = "GetSkuListBasedOnVehicleSize";
            return Result(inputKey, _methodName, (SkuInputDto x) => { return _saudaService.GetSkuListBasedOnVehicleSize(x); });
        }
        /// <summary>
        /// Method to get sauda list
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("details")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaDetails([FromBody]string inputKey)
        {
            _methodName = "GetSaudaDetails";
            return Result(inputKey, _methodName, (SaudaDetailInputDto x) => { return _saudaService.GetSaudaDetails(x); });
        }

        [HttpPost]
        [Route("detailsTPNew")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaDetailsTPNew", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaDetailsTPNew([FromBody] string inputKey)
        {
            _methodName = "GetSaudaDetailsTPNew";
            return Result(inputKey, _methodName, (SaudaDetailInputDto x) => { return _saudaService.GetSaudaDetailsTPNew(x); });
        }

        [HttpPost]
        [Route("saudalimit/history")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaLimitRequestHistory", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaLimitRequestHistory([FromBody]string inputKey)
        {
            _methodName = "GetSaudaLimitRequestHistory";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _saudaService.GetSaudaLimitRequestHistory(x); });
        }

        [HttpPost]
        [Route("saudalimit/historydetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaLimitRequestHistoryDetail", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaLimitRequestHistoryDetail([FromBody]string inputKey)
        {
            _methodName = "GetSaudaLimitRequestHistoryDetail";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _saudaService.GetSaudaLimitRequestHistoryDetail(x); });
        }

        [HttpPost]
        [Route("saudalimit/Add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddSaudaLimitRequest", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddSaudaLimitRequest([FromBody]string inputKey)
        {
            _methodName = "GetSaudaLimitRequestHistoryDetail";
            return Result(inputKey, _methodName, (SaudaLimitRequestHistoryDto x) => { return _saudaService.AddSaudaLimitRequest(x); });
        }


        #region Sauda Amendment
        [HttpPost]
        [Route("list/foramendment")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaListForAmendment", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaListForAmendment([FromBody]string inputKey)
        {
            _methodName = "GetSaudaListForAmendment";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _saudaService.GetSaudaListForAmendment(x); });
        }

        /// Method to create sauda
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("saudaamendment/create")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaudaAmendmentCreation", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaAmendmentCreation([FromBody]string inputKey)
        {
            _methodName = "SaudaAmendmentCreation";
            return Result(inputKey, _methodName, (SaudaAmendmentInputDto x) => { return _saudaService.SaveSaudaAmendment(x); });
        }
        #endregion


        #region Sauda Chart
        [HttpPost]
        [Route("dealer/outstandingchart")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerOutstandingSaudaListForChart", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerOutstandingSaudaListForChart([FromBody]string inputKey)
        {
            _methodName = "GetDealerOutstandingSaudaListForChart";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _saudaService.GetDealerOutstandingSaudaListForChart(x); });
        }

        [HttpPost]
        [Route("StateTrader/outstandingchart")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBodOutstandingSaudaListForChart", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBodOutstandingSaudaListForChart([FromBody]string inputKey)
        {
            _methodName = "GetBodOutstandingSaudaListForChart";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _saudaService.GetBodOutstandingSaudaListForChart(x); });
        }

        #endregion

        #region OutStanding Sauda
        [HttpPost]
        [Route("OutStandingSaudaList")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetOutStandingSaudaList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetOutStandingSaudaList([FromBody]string inputKey)
        {
            _methodName = "GetOutStandingSaudaList";
            return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _saudaService.GetOutStandingSaudaList(x); });
        }



        [HttpPost]
        [Route("dealerSaudaLists")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerSaudaLists", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerSaudaLists([FromBody]string inputKey)
        {
            _methodName = "GetDealerSaudaLists";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _saudaService.GetDealerSaudaLists(x); });
        }

        [HttpPost]
        [Route("dealerSalesLists")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerSalesLists", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerSalesLists([FromBody]string inputKey)
        {
            _methodName = "GetDealerSalesLists";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _saudaService.GetDealerSalesLists(x); });
        }
        #endregion

        #region Special Rate Approval Request
        [HttpPost]
        [Route("specialrate/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddSpecialRateApprovalRequest", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddSpecialRateApprovalRequest([FromBody]string inputKey)
        {
            _methodName = "AddSpecialRateApprovalRequest";
            return Result(inputKey, _methodName, (SpecialRateApprovalAddDto x) => { return _saudaService.AddSpecialRateApprovalRequest(x); });
        }

        [HttpPost]
        [Route("specialrate/search")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialRateRequestList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialRateRequestList([FromBody]string inputKey)
        {
            _methodName = "GetSpecialRateRequestList";
            return Result(inputKey, _methodName, (SpecialRateInputDto x) => { return _saudaService.GetSpecialRateRequestList(x); });
        }

        [HttpPost]
        [Route("specialrate/view")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialRateRequestDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialRateRequestDetails([FromBody]string inputKey)
        {
            _methodName = "GetSpecialRateRequestDetails";
            return Result(inputKey, _methodName, (SpecialRateDetailInputDto x) => { return _saudaService.GetSpecialRateRequestDetails(x); });
        }

        /// <summary>
        /// Method to create sauda
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create/fromspecialrate")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaudaCreationFromSpecialRate", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaCreationFromSpecialRate([FromBody]string inputKey)
        {
            _methodName = "SaudaCreationFromSpecialRate";
            return Result(inputKey, _methodName, (SpecialRateSaudaDto x) => { return _saudaService.SaudaCreationFromSpecialRate(x); });
        }

        [HttpPost]
        [Route("specialratenew/search")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialRateRequestListNew", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialRateRequestListNew([FromBody] string inputKey)
        {
            _methodName = "GetSpecialRateRequestListNew";
            return Result(inputKey, _methodName, (SpecialRateInputDto x) => { return _saudaService.GetSpecialRateRequestListNew(x); });
        }

        [HttpPost]
        [Route("specialrate/approveorreject")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SpecialRateApproveOrReject", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SpecialRateApproveOrReject([FromBody] string inputKey)
        {
            _methodName = "SpecialRateApproveOrReject";
            return Result(inputKey, _methodName, (SpecialRateSaudaDto x) => { return _saudaService.SpecialRateApproveOrReject(x); });
        }

        #endregion


        [HttpPost]
        [Route("Dashboard/PendingSaudaChart")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingSaudaChartForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingSaudaChartForMobile([FromBody]string inputKey)
        {
            _methodName = "GetPendingSaudaChartForMobile";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _saudaService.GetPendingSaudaChartForMobile(x); });
        }

        [HttpPost]
        [Route("PendingSaudaChartDetail")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingSaudaChartDetailForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingSaudaChartDetailForMobile([FromBody]string inputKey)
        {
            _methodName = "GetPendingSaudaChartDetailForMobile";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _saudaService.GetPendingSaudaChartDetailForMobile(x); });
        }

        [HttpPost]
        [Route("BookedSauda")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBookedSauda", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBookedSauda([FromBody]string inputKey)
        {
            _methodName = "GetPendingSaudaChartDetailForMobile";
            return Result(inputKey, _methodName, (BookedSaudaInputDto x) => { return _saudaService.GetBookedSauda(x); });
        }

        /// <summary>
        /// Method to get sauda list
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Saudaorderdetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaorderdetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaorderdetails([FromBody]string inputKey)
        {
            _methodName = "GetSaudaDetails";
            return Result(inputKey, _methodName, (SaudaDetailInputDto x) => { return _saudaService.GetSaudaorderdetails(x); });
        }

        #region Sauda Conversion
        [HttpPost]
        [Route("conversion/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddSaudaConversionOrders", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddSaudaConversionOrders([FromBody]string inputKey)
        {
            _methodName = "AddSaudaConversionOrders";
            return Result(inputKey, _methodName, (SaudaConversionAddDto x) => { return _saudaService.AddSaudaConversionOrders(x); });
        }

        [HttpPost]
        [Route("conversion/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaConversionList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaConversionList([FromBody]string inputKey)
        {
            _methodName = "GetSaudaConversionList";
            return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _saudaService.GetSaudaConversionList(x); });
        }

        [HttpPost]
        [Route("conversion/view")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaConversionDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaConversionDetails([FromBody]string inputKey)
        {
            _methodName = "GetSaudaConversionDetails";
            return Result(inputKey, _methodName, (SaudaConversionDetailInputDto x) => { return _saudaService.GetSaudaConversionDetails(x); });
        }
        [HttpPost]
        [Route("conversion/unitdnddifferencerate/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddSaudaConversionUnitAndDifferenceRate", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddSaudaConversionUnitAndDifferenceRate([FromBody]string inputKey)
        {
            _methodName = "AddSaudaConversionUnitAndDifferenceRate";
            return Result(inputKey, _methodName, (SaudaConversionUnitAndDifferenceRateAddDto x) => { return _saudaService.AddSaudaConversionUnitAndDifferenceRate(x); });
        }
        #endregion

        #region Sauda Extension
        [HttpPost]
        [Route("extension/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddSaudaExtension", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddSaudaExtension([FromBody]string inputKey)
        {
            _methodName = "AddSaudaExtension";
            return Result(inputKey, _methodName, (SaudaExtensionAddDto x) => { return _saudaService.AddSaudaExtension(x); });
        }

        [HttpPost]
        [Route("extension/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaExtensionList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaExtensionList([FromBody]string inputKey)
        {
            _methodName = "GetSaudaExtensionList";
            return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _saudaService.GetSaudaExtensionList(x); });
        }
        #endregion

        [HttpPost]
        [Route("saudanumber/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaNumberList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaNumberList([FromBody]string inputKey)
        {
            _methodName = "GetSaudaNumberList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _saudaService.GetSaudaNumberList(x); });
        }

        #region Bid counter
        //[HttpPost]
        //[Route("counterbid/view")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetSaudaCounterBidDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetSaudaCounterBidDetails([FromBody]string inputKey)
        //{
        //    _methodName = "GetSaudaCounterBidDetails";
        //    return Result(inputKey, _methodName, (SaudaDetailInputDto x) => { return _saudaService.GetSaudaCounterBidDetails(x); });
        //}

        //[HttpPost]
        //[Route("counterbid/approve")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "ApproveCounterBid", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult ApproveCounterBid([FromBody]string inputKey)
        //{
        //    _methodName = "ApproveCounterBid";
        //    return Result(inputKey, _methodName, (CounterBidInputDto x) => { return _saudaService.ApproveCounterBid(x); });
        //}
        #endregion

        #region Load Test

        /// <summary>
        /// Method to create sauda
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("saudacreate/loadtest")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "SaudaCreationLoadTest", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaCreationLoadTest([FromBody]SaudaInputDto inputKey)
        {
            //return Result(inputKey, _methodName, (SaudaInputDto x) => { return _saudaService.SaudaCreation(x); });
            _methodName = "SaudaCreationLoadTest";
            var result = _saudaService.SaudaCreation(inputKey);
            return Ok(result);
        }

        [HttpPost]
        [Route("pendingcontract/chart")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingContractChartMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingContractChartMobile([FromBody]string inputKey)
        {
            _methodName = "GetPendingContractChartMobile";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _saudaService.GetPendingContractChartMobile(x); });
        }

        [HttpPost]
        [Route("ExpiredAndNearExpiredSauda")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetExpiredAndNearExpiredSaudaDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetExpiredAndNearExpiredSaudaDetails([FromBody]string inputKey)
        {
            _methodName = "GetExpiredAndNearExpiredSaudaDetails";
            return Result(inputKey, _methodName, (SaudaDetailInputDto x) => { return _saudaService.GetExpiredAndNearExpiredSaudaDetails(x); });
        }

        #endregion

        #region Push Notification Testing

        [HttpPost]
        [Route("pushnotification/testing")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "SaudaCreationLoadTest", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult PushNotificationTesting([FromBody]LoginUserIdDto inputKey)
        {
            _methodName = "SaudaCreationLoadTest";
            var result = _saudaService.PushNotificationTesting(inputKey);
            return Ok(result);
        }

        //[HttpPost]
        //[Route("counterbiddetails")]
        //[ResponseType(typeof(ContentDto))]
        ////[Throttle(Name = "SaudaCreationLoadTest", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetCounterBidDetails([FromBody]SaudaDetailInputDto inputKey)
        //{
        //    _methodName = "GetCounterBidDetails";
        //    var result = _saudaService.GetCounterBidDetails(inputKey);
        //    return Ok(result);
        //}

        #endregion

        //[HttpPost]
        //[Route("saudabiddingcart/id")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "SaudaCounterBidOfferDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult SaudaCounterBidOfferDetails([FromBody]string inputKey)
        //{
        //    _methodName = "SaudaCounterBidOfferDetails";
        //    return Result(inputKey, _methodName, (SaudaCounterBidOfferDetailsInputDto x) => { return _saudaService.SaudaCounterBidOfferDetails(x); });
        //}
        [HttpPost]
        [Route("Saudaorderdetails1")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaorderdetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaorderdetails1(SaudaDetailInputDto inputKey)
        {
            _methodName = "GetSaudaDetails";
            //return Result(inputKey, _methodName, (SaudaDetailInputDto x) => { return _saudaService.GetSaudaorderdetails(x); });
            var result = new ResultDto();
            //return Result(_methodName, () => { return _masterService.GetFrieghtRouteList(); });
            result = _saudaService.GetSaudaorderdetails1(inputKey);
            return Ok(result);
        }

        [HttpPost]
        [Route("saudalimit/history1")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaLimitRequestHistory", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaLimitRequestHistory1(IdInputDto inputKey)
        {
            _methodName = "GetSaudaLimitRequestHistory";
            //return Result(inputKey, _methodName, (IdInputDto x) => { return _saudaService.GetSaudaLimitRequestHistory(x); });
            var result = new ResultDto();
            result = _saudaService.GetSaudaLimitRequestHistory(inputKey);
            return Ok(result);
        }

        [HttpPost]
        [Route("skulist/packgroupid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkuListByPackGroupId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuListByPackGroupId([FromBody]string inputKey)
        {
            _methodName = "GetSkuListByPackGroupId";
            return Result(inputKey, _methodName, (SkuDropDownInputDto x) => { return _saudaService.GetSkuListByPackGroupId(x); });

        }

        #region ChequeStatusReport
        [HttpPost]
        [Route("chequestatus/getdetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetChequeStatusReportDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetChequeStatusReportDetails([FromBody]string inputKey)
        {
            _methodName = "GetChequeStatusReportDetails";
            return Result(inputKey, _methodName, (ChequeStatusReportInputDto x) => { return _saudaService.GetChequeStatusReportDetails(x); });
        }


        #endregion

        #region Filler Sku 

        [HttpPost]
        [Route("fillersku/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetFillerskuForIndentRequest", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetFillerskuForIndentRequest([FromBody] string inputKey)
        {
            _methodName = "GetFillerskuForIndentRequest";
            return Result(inputKey, _methodName, (FillerSkuInputDto x) => { return _saudaService.GetFillerskuForIndentRequest(x); });
        }

        #endregion

        #region Sauda Extension

        [HttpPost]
        [Route("saudaextensiondays")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "PostSaudaExtensionDays", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult PostSaudaExtensionDays([FromBody] string inputKey)
        {
            _methodName = "PostSaudaExtensionDays";
            return Result(inputKey, _methodName, (SaudaExtensionDaysDto x) => { return _saudaService.PostSaudaExtensionDays(x); });
        }

        #endregion

        [HttpPost]
        [Route("saudaextension/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSAPSaudaExtensionList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSAPSaudaExtensionList([FromBody] string inputKey)
        {
            _methodName = "GetSAPSaudaExtensionList";
            return Result(inputKey, _methodName, (SAPSaudaInputDto x) => { return _saudaService.GetSAPSaudaExtensionList(x); });
        }


        [HttpPost]
        [Route("saudarelease")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaudaReleaseToSAP", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaReleaseToSAP([FromBody] string inputKey)
        {
            _methodName = "SaudaReleaseToSAP";
            return Result(inputKey, _methodName, (SAPSaudaInputDto x) => { return _saudaService.SaudaReleaseToSAP(x); });
        }

        [HttpPost]
        [Route("saudarelease/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSAPSaudaReleaseList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSAPSaudaReleaseList([FromBody] string inputKey)
        {
            _methodName = "GetSAPSaudaReleaseList";
            return Result(inputKey, _methodName, (SAPSaudaInputDto x) => { return _saudaService.GetSAPSaudaReleaseList(x); });
        }

        [HttpPost]
        [Route("addsaudalimithistory")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddSaudaLimitHistory", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddSaudaLimitHistory([FromBody] string inputKey)
        {
            _methodName = "AddSaudaLimitHistory";
            return Result(inputKey, _methodName, (SaudaLimitHistoryDto x) => { return _saudaService.AddSaudaLimitHistory(x); });
        }


        [HttpPost]
        [Route("getsaudalimithistory/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaLimitHistoryList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaLimitHistoryList([FromBody] string inputKey)
        {
            _methodName = "GetSaudaLimitHistoryList";
            return Result(inputKey, _methodName, (SaudaLimitHistoryDto x) => { return _saudaService.GetSaudaLimitHistoryList(x); });
        }

        #region CompetitorAnalysis

        /// <summary>
        /// Method to Save CompetitorAnalysis
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("competitoranalysis/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveCompetitorAnalysis", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveCompetitorAnalysis([FromBody] string inputKey)
        {
            _methodName = "SaveCompetitorAnalysis";
            return Result(inputKey, _methodName, (CompetitorAnalysisInputDto x) => { return _saudaService.SaveCompetitorAnalysis(x); });
        }

        #endregion

        #region Sauda Modification

        [HttpPost]
        [Route("pendingcontract/dealerid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetValidPendingContractByDelaerId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetValidPendingContractByDelaerId([FromBody] string inputKey)
        {
            _methodName = "GetValidPendingContractByDelaerId";
            return Result(inputKey, _methodName, (UserIdDto x) => { return _saudaService.GetValidPendingContractByDelaerId(x); });
        }

        [HttpPost]
        [Route("oiltypesandskus/pendingcontractid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetOilTypesByPendingContractId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetOilTypesByPendingContractId([FromBody] string inputKey)
        {
            _methodName = "GetOilTypesByPendingContractId";
            return Result(inputKey, _methodName, (SaudaDetailInputDto x) => { return _saudaService.GetOilTypesByPendingContractId(x); });
        }

        [HttpPost]
        [Route("toskus/fromskuoiltype")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetToSkusBasedOnFromSkuOilType", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetToSkusBasedOnFromSkuOilType([FromBody] string inputKey)
        {
            _methodName = "GetToSkusBasedOnFromSkuOilType";
            return Result(inputKey, _methodName, (SaudaMofificationFromSkuInfoDto x) => { return _saudaService.GetToSkusBasedOnFromSkuOilType(x); });
        }

        [HttpPost]
        [Route("pendingcontractdetails/pendingcontract")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingContractDetailsByPendingContract", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingContractDetailsByPendingContract([FromBody] string inputKey)
        {
            _methodName = "GetOilTypesByPendingContractId";
            return Result(inputKey, _methodName, (SaudaDetailInputDto x) => { return _saudaService.GetPendingContractDetailsByPendingContract(x); });
        }

        [HttpPost]
        [Route("toskuslist/saudamodification")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetToSkusForSaudaModification", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetToSkusForSaudaModification([FromBody] string inputKey)
        {
            _methodName = "GetToSkusForSaudaModification";
            return Result(inputKey, _methodName, (SaudaMofificationFromSkuDetailsDto x) => { return _saudaService.GetToSkusForSaudaModification(x); });
        }

        [HttpPost]
        [Route("saudamodification/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveSaudaModification", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveSaudaModification([FromBody] string inputKey)
        {
            _methodName = "SaveSaudaModification";
            return Result(inputKey, _methodName, (SaudaModificationInputDTO x) => { return _saudaService.SaveSaudaModification(x); });
        }

        [HttpPost]
        [Route("saudamodification/pendingapprovedlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaModificationPendingApprovedList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaModificationPendingApprovedList([FromBody] string inputKey)
        {
            _methodName = "GetSaudaModificationPendingApprovedList";
            return Result(inputKey, _methodName, (SaudaReportFilterDto x) => { return _saudaService.GetSaudaModificationPendingApprovedList(x); });
        }

        [HttpPost]
        [Route("saudamodification/adminapp/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaModificationApprovalList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaModificationApprovalList([FromBody] string inputKey)
        {
            _methodName = "GetSaudaModificationApprovalList";
            return Result(inputKey, _methodName, (SaudaListFilterDto x) => { return _saudaService.GetSaudaModificationApprovalList(x); });
        }

        [HttpPost]
        [Route("saudamodification/details")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaModificationDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaModificationDetails([FromBody] string inputKey)
        {
            _methodName = "GetSaudaModificationDetails";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _saudaService.GetSaudaModificationDetails(x); });
        }

        [HttpPost]
        [Route("saudamodification/statuschange")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ChangeSaudaModificationStatus", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ChangeSaudaModificationStatus([FromBody] string inputKey)
        {
            _methodName = "ChangeSaudaModificationStatus";
            return Result(inputKey, _methodName, (SaudaModificationUpdateDto x) => { return _saudaService.ChangeSaudaModificationStatus(x); });
        }

        [HttpPost]
        [Route("saudamodification/statuschangeforloose")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ChangeSaudaModificationStatusForLoose", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ChangeSaudaModificationStatusForLoose([FromBody] string inputKey)
        {
            _methodName = "ChangeSaudaModificationStatusForLoose";
            return Result(inputKey, _methodName, (SaudaModificationUpdateDto x) => { return _saudaService.ChangeSaudaModificationStatusForLoose(x); });
        }

        #endregion
    }
}
