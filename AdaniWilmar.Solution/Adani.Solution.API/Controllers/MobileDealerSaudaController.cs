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
    [RoutePrefix("api/mobileDealersauda")]
    public class MobileDealerSaudaController : BaseApiController
    {
        private const string ServiceName = "Mobile Dealer Sauda Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IMobileDealerSaudaService _dealersaudaService;
        private string _methodName;

        public MobileDealerSaudaController(IMobileDealerSaudaService saudaService) : base(ServiceName)
        {
            _methodName = "Mobile Dealer Sauda Controller";
            try
            {
                _dealersaudaService = saudaService;
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
            return Result(inputKey, _methodName, (IdInputDto x) => { return _dealersaudaService.DealerSaudaDetails(x); });
        }

        [HttpPost]
        [Route("PendingSaudaList")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingSaudaListForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingSaudaListForMobile([FromBody]string inputKey)
        {
            _methodName = "GetPendingSaudaListForMobile";
            return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _dealersaudaService.GetPendingSaudaListForMobile(x); });
        }

        [HttpPost]
        [Route("DealerLocationsById")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerLocationsByDealerId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerLocationsByDealerId([FromBody]string inputKey)
        {
            _methodName = "GetDealerLocationsByDealerId";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _dealersaudaService.GetDealerLocationsByDealerId(x); });
        }

        [HttpPost]
        [Route("dealerDetail")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerDetail", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerDetail([FromBody]string inputKey)
        {
            _methodName = "GetDealerDetail";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _dealersaudaService.GetDealerDetail(x); });
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
            return Result(inputKey, _methodName, (SaudaInputDto x) => { return _dealersaudaService.SaudaCreation(x); });
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
            return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _dealersaudaService.GetSaudaList(x); });
        }

        [HttpPost]
        [Route("short/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaShortViewList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaShortViewList([FromBody]string inputKey)
        {
            _methodName = "GetSaudaShortViewList";
            return Result(inputKey, _methodName, (LoginUserIdCoversionDto x) => { return _dealersaudaService.GetSaudaShortViewList(x); });
        }

        [HttpPost]
        [Route("short/view")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaShortViewDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaShortViewDetails([FromBody]string inputKey)
        {
            _methodName = "GetSaudaShortViewDetails";
            return Result(inputKey, _methodName, (SaudaDetailInputDto x) => { return _dealersaudaService.GetSaudaShortViewDetails(x); });
        }

        [HttpPost]
        [Route("expired/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetExpiredSaudaListForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetExpiredSaudaListForMobile([FromBody]string inputKey)
        {
            _methodName = "GetExpiredSaudaListForMobile";
            return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _dealersaudaService.GetExpiredSaudaListForMobile(x); });
        }

        [HttpPost]
        [Route("sku/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkuListForIndentRequest", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuListForIndentRequest([FromBody]string inputKey)
        {
            _methodName = "GetSkuListForIndentRequest";
            return Result(inputKey, _methodName, (SkuInputDto x) => { return _dealersaudaService.GetSkuListForIndentRequest(x); });
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
            return Result(inputKey, _methodName, (SaudaDetailInputDto x) => { return _dealersaudaService.GetSaudaDetails(x); });
        }

        [HttpPost]
        [Route("saudalimit/history")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaLimitRequestHistory", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaLimitRequestHistory([FromBody]string inputKey)
        {
            _methodName = "GetSaudaLimitRequestHistory";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _dealersaudaService.GetSaudaLimitRequestHistory(x); });
        }

        [HttpPost]
        [Route("saudalimit/historydetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaLimitRequestHistoryDetail", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaLimitRequestHistoryDetail([FromBody]string inputKey)
        {
            _methodName = "GetSaudaLimitRequestHistoryDetail";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _dealersaudaService.GetSaudaLimitRequestHistoryDetail(x); });
        }

        [HttpPost]
        [Route("saudalimit/Add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddSaudaLimitRequest", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddSaudaLimitRequest([FromBody]string inputKey)
        {
            _methodName = "GetSaudaLimitRequestHistoryDetail";
            return Result(inputKey, _methodName, (SaudaLimitRequestHistoryDto x) => { return _dealersaudaService.AddSaudaLimitRequest(x); });
        }


        #region Sauda Amendment
        [HttpPost]
        [Route("list/foramendment")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaListForAmendment", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaListForAmendment([FromBody]string inputKey)
        {
            _methodName = "GetSaudaListForAmendment";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _dealersaudaService.GetSaudaListForAmendment(x); });
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
            return Result(inputKey, _methodName, (SaudaAmendmentInputDto x) => { return _dealersaudaService.SaveSaudaAmendment(x); });
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
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _dealersaudaService.GetDealerOutstandingSaudaListForChart(x); });
        }

        [HttpPost]
        [Route("StateTrader/outstandingchart")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBodOutstandingSaudaListForChart", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBodOutstandingSaudaListForChart([FromBody]string inputKey)
        {
            _methodName = "GetBodOutstandingSaudaListForChart";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _dealersaudaService.GetBodOutstandingSaudaListForChart(x); });
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
            return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _dealersaudaService.GetOutStandingSaudaList(x); });
        }



        [HttpPost]
        [Route("dealerSaudaLists")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerSaudaLists", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerSaudaLists([FromBody]string inputKey)
        {
            _methodName = "GetDealerSaudaLists";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _dealersaudaService.GetDealerSaudaLists(x); });
        }

        [HttpPost]
        [Route("dealerSalesLists")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerSalesLists", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerSalesLists([FromBody]string inputKey)
        {
            _methodName = "GetDealerSalesLists";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _dealersaudaService.GetDealerSalesLists(x); });
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
            return Result(inputKey, _methodName, (SpecialRateApprovalAddDto x) => { return _dealersaudaService.AddSpecialRateApprovalRequest(x); });
        }

        [HttpPost]
        [Route("specialrate/search")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialRateRequestList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialRateRequestList([FromBody]string inputKey)
        {
            _methodName = "GetSpecialRateRequestList";
            return Result(inputKey, _methodName, (SpecialRateInputDto x) => { return _dealersaudaService.GetSpecialRateRequestList(x); });
        }

        [HttpPost]
        [Route("specialrate/view")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialRateRequestDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialRateRequestDetails([FromBody]string inputKey)
        {
            _methodName = "GetSpecialRateRequestDetails";
            return Result(inputKey, _methodName, (SpecialRateDetailInputDto x) => { return _dealersaudaService.GetSpecialRateRequestDetails(x); });
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
            return Result(inputKey, _methodName, (SpecialRateSaudaDto x) => { return _dealersaudaService.SaudaCreationFromSpecialRate(x); });
        }

        #endregion

        #region Credit Limit
        [HttpPost]
        [Route("creditlimit/total")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetTotalCreditLimit", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetTotalCreditLimit([FromBody]string inputKey)
        {
            _methodName = "GetTotalCreditLimit";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _dealersaudaService.GetTotalCreditLimit(x); });
        }

        [HttpPost]
        [Route("creditlimit/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCreditLimitList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCreditLimitList([FromBody]string inputKey)
        {
            _methodName = "GetCreditLimitList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _dealersaudaService.GetCreditLimitList(x); });
        }
        #endregion

        [HttpPost]
        [Route("Dashboard/PendingSaudaChart")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingSaudaChartForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingSaudaChartForMobile([FromBody]string inputKey)
        {
            _methodName = "GetPendingSaudaChartForMobile";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _dealersaudaService.GetPendingSaudaChartForMobile(x); });
        }

        [HttpPost]
        [Route("PendingSaudaChartDetail")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingSaudaChartDetailForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingSaudaChartDetailForMobile([FromBody]string inputKey)
        {
            _methodName = "GetPendingSaudaChartDetailForMobile";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _dealersaudaService.GetPendingSaudaChartDetailForMobile(x); });
        }

        [HttpPost]
        [Route("BookedSauda")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBookedSauda", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBookedSauda([FromBody]string inputKey)
        {
            _methodName = "GetBookedSauda";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _dealersaudaService.GetBookedSauda(x); });
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
            _methodName = "GetSaudaorderdetails";
            return Result(inputKey, _methodName, (SaudaDetailInputDto x) => { return _dealersaudaService.GetSaudaorderdetails(x); });
        }
        #region Sauda Conversion
        [HttpPost]
        [Route("conversion/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddSaudaConversionOrders", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddSaudaConversionOrders([FromBody]string inputKey)
        {
            _methodName = "AddSaudaConversionOrders";
            return Result(inputKey, _methodName, (SaudaConversionAddDto x) => { return _dealersaudaService.AddSaudaConversionOrders(x); });
        }

        [HttpPost]
        [Route("conversion/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaConversionList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaConversionList([FromBody]string inputKey)
        {
            _methodName = "GetSaudaConversionList";
            return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _dealersaudaService.GetSaudaConversionList(x); });
        }

        [HttpPost]
        [Route("conversion/view")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaConversionDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaConversionDetails([FromBody]string inputKey)
        {
            _methodName = "GetSaudaConversionDetails";
            return Result(inputKey, _methodName, (SaudaConversionDetailInputDto x) => { return _dealersaudaService.GetSaudaConversionDetails(x); });
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
            return Result(inputKey, _methodName, (SaudaExtensionAddDto x) => { return _dealersaudaService.AddSaudaExtension(x); });
        }

        [HttpPost]
        [Route("extension/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaExtensionList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaExtensionList([FromBody]string inputKey)
        {
            _methodName = "GetSaudaExtensionList";
            return Result(inputKey, _methodName, (SaudaFilterDto x) => { return _dealersaudaService.GetSaudaExtensionList(x); });
        }

        [HttpPost]
        [Route("extension/sauda/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "NewAddSaudaExtension", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult NewAddSaudaExtension([FromBody]string inputKey)
        {
            _methodName = "NewAddSaudaExtension";
            return Result(inputKey, _methodName, (SaudaExtensionNewAddDto x) => { return _dealersaudaService.NewAddSaudaExtension(x); });
        }
        #endregion

        #region Bid counter
        //[HttpPost]
        //[Route("counterbid/view")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetSaudaCounterBidDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetSaudaCounterBidDetails([FromBody]string inputKey)
        //{
        //    _methodName = "GetSaudaCounterBidDetails";
        //    return Result(inputKey, _methodName, (SaudaDetailInputDto x) => { return _dealersaudaService.GetSaudaCounterBidDetails(x); });
        //}

        //[HttpPost]
        //[Route("counterbid/approve")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "ApproveCounterBid", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult ApproveCounterBid([FromBody]string inputKey)
        //{
        //    _methodName = "ApproveCounterBid";
        //    return Result(inputKey, _methodName, (CounterBidInputDto x) => { return _dealersaudaService.ApproveCounterBid(x); });
        //}
        #endregion

        #region Load Test

        [HttpPost]
        [Route("create/loadtest")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "SaudaCreationLoadTest", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaCreationLoadTest([FromBody]SaudaInputDto inputKey)
        {
            _methodName = "SaudaCreationLoadTest";
            var result = _dealersaudaService.SaudaCreation(inputKey);
            return Ok(result);
        }

        #endregion

        [HttpPost]
        [Route("pendingcontract/chart")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingContractChartMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingContractChartMobile([FromBody]string inputKey)
        {
            _methodName = "GetPendingContractChartMobile";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _dealersaudaService.GetPendingContractChartMobile(x); });
        }

        #region New Change Sauda Conversion CR
        [HttpPost]
        [Route("saudaconversion/get/skulist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSKUListForSaudaConversion", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSKUListForSaudaConversion([FromBody]string inputKey)
        {
            _methodName = "GetSKUListForSaudaConversion";
            return Result(inputKey, _methodName, (SaudaConversionSKUInputDto x) => { return _dealersaudaService.GetSKUListForSaudaConversion(x); });            
        }

        [HttpPost]
        [Route("saudaconversion/get/dealerplantdepot/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerPlantDepotList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerPlantDepotList([FromBody]string inputKey)
        {
            _methodName = "GetDealerPlantDepotList";
            return Result(inputKey, _methodName, (UserIdDto x) => { return _dealersaudaService.GetDealerPlantDepotList(x); });            
        }

        [HttpPost]
        [Route("saudaconversion/sku/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveSaudaConversionSkuDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveSaudaConversionSkuDetails([FromBody]string inputKey)
        {
            _methodName = "SaveSaudaConversionSkuDetails";
            return Result(inputKey, _methodName, (SaudaConversionSKUInputDto x) => { return _dealersaudaService.SaveSaudaConversionSkuDetails(x); });            
        }

        [HttpPost]
        [Route("saudaconversion/pendingapprovedlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaConversionPendingAndApprovedList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaConversionPendingAndApprovedList([FromBody]string inputKey)
        {
            _methodName = "GetSaudaConversionPendingAndApprovedList";
            return Result(inputKey, _methodName, (SaudaReportFilterDto x) => { return _dealersaudaService.GetSaudaConversionPendingAndApprovedList(x); });
        }

        [HttpPost]
        [Route("saudaconversion/pendingapprovedlist/ZonalTrader")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetZonalHeadSaudaConversionPendingApprovedList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetZonalHeadSaudaConversionPendingApprovedList([FromBody]string inputKey)
        {
            _methodName = "GetZonalHeadSaudaConversionPendingApprovedList";
            return Result(inputKey, _methodName, (SaudaReportFilterDto x) => { return _dealersaudaService.GetZonalHeadSaudaConversionPendingApprovedList(x); });
        }

        [HttpPost]
        [Route("saudaconversion/pendingapprovedlist/StateTrader")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBDOSaudaConversionPendingApprovedList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBDOSaudaConversionPendingApprovedList([FromBody]string inputKey)
        {
            _methodName = "GetBDOSaudaConversionPendingApprovedList";
            return Result(inputKey, _methodName, (SaudaReportFilterDto x) => { return _dealersaudaService.GetBDOSaudaConversionPendingApprovedList(x); });
        }

        [HttpPost]
        [Route("saudaconversion/pendingapprovedlist/dealer")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerSaudaConversionPendingApprovedList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerSaudaConversionPendingApprovedList([FromBody]string inputKey)
        {
            _methodName = "GetDealerSaudaConversionPendingApprovedList";
            return Result(inputKey, _methodName, (SaudaReportFilterDto x) => { return _dealersaudaService.GetDealerSaudaConversionPendingApprovedList(x); });
        }

        [HttpPost]
        [Route("saudaconversion/get/skudetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaConversionSkuDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaConversionSkuDetailsById([FromBody]string inputKey)
        {
            _methodName = "GetSaudaConversionSkuDetailsById";
            return Result(inputKey, _methodName, (SaudaConversionSKUInputDto x) => { return _dealersaudaService.GetSaudaConversionSkuDetailsById(x); });
        }

        [HttpPost]
        [Route("saudaconversion/get/unitandbasicrate")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaConversionUnitandBasicRateList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaConversionUnitandBasicRateList([FromBody]string inputKey)
        {
            _methodName = "GetSaudaConversionUnitandBasicRateList";
            return Result(inputKey, _methodName, (SaudaConversionUnitAndDiffRateInputDto x) => { return _dealersaudaService.GetSaudaConversionUnitAndBaseRateList(x); });
        }

        [HttpPost]
        [Route("saudaConversion")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaConversionReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaConversionReport([FromBody]string inputKey)
        {
            _methodName = "GetSaudaConversionReport";
            return Result(inputKey, _methodName, (SaudaConversionReportInputDto x) => { return _dealersaudaService.GetSaudaConversionReport(x); });
        }

        #endregion

        #region Get Sap Sync Pending Sauda Conversion List

        [HttpPost]
        [Route("sapsyncpendingsaudaConversionlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSapSyncPendingSaudaConversionList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSapSyncPendingSaudaConversionList([FromBody]string inputKey)
        {
            _methodName = "GetSapSyncPendingSaudaConversionList";
            return Result(inputKey, _methodName, (SaudaConversionInputDto x) => { return _dealersaudaService.GetSapSyncPendingSaudaConversionList(x); });
        }

        #endregion

        [HttpPost]
        [Route("saudaconversion/pendingapprovedlist/mobile")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaConversionListMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaConversionListMobile([FromBody]string inputKey)
        {
            _methodName = "GetSaudaConversionListMobile";
            return Result(inputKey, _methodName, (SaudaConversionInputDTO x) => { return _dealersaudaService.GetSaudaConversionListMobile(x); });
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
            return Result(inputKey, _methodName, (CompetitorAnalysisAddDto x) => { return _dealersaudaService.SaveCompetitorAnalysis(x); });
        }

        #endregion

        #region Pending Contract - Sales Order 

        [HttpPost]
        [Route("contractnumberlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetContractNumberList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetContractNumberList([FromBody] string inputKey)
        {
            _methodName = "GetContractNumberList";
            return Result(inputKey, _methodName, (ContractNoInputDto x) => { return _dealersaudaService.GetContractNumberList(x); });
        }


        [HttpPost]
        [Route("skulistwithqty/contractnumber")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkuListByContractNumber", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuListByContractNumber([FromBody] string inputKey)
        {
            _methodName = "GetSkuListByContractNumber";
            return Result(inputKey, _methodName, (ContractNoInputDto x) => { return _dealersaudaService.GetSkuListByContractNumber(x); });
        }


        #endregion
    }
}
