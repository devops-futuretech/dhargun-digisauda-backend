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
    [RoutePrefix("api/sauda")]
    public class SaudaController : BaseApiController
    {
        private const string ServiceName = "Sauda Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly ISaudaServices _saudaService;
        private string _methodName;

        public SaudaController(ISaudaServices saudaService) : base(ServiceName)
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

        /// <summary>
        /// Method to get sauda list for admin
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("admin/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaListForAdmin", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaListForAdmin([FromBody]string inputKey)
        {
            _methodName = "GetSaudaListForAdmin";
            return KendoGridResult(inputKey, _methodName, (SaudaListFilterDto x) => { return _saudaService.GetSaudaListForAdmin(x); });
        }

        /// <summary>
        /// Method to get sauda list for admin app
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("adminapp/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaListForAdmin", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaListForAdminApp([FromBody] string inputKey)
        {
            _methodName = "GetSaudaListForAdminApp";
            return Result(inputKey, _methodName, (SaudaListFilterDto x) => { return _saudaService.GetSaudaListForAdminMobile(x); });
        }

        [HttpGet]
        [Route("updateliftingSaudaOrderId")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaListForAdmin", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateLiftingSaudaOrderId()
        {
            _methodName = "UpdateLiftingSaudaOrderId";
            return Result(_methodName, () => { return _saudaService.UpdateLiftingSaudaOrderId(); });
        }

        /// <summary>
        /// Method to get sauda list
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("admin/saudhalist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetAllSaudaList([FromBody]string inputKey)
        {
            _methodName = "GetSaudaList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _saudaService.GetAllSaudaList(x); });
        }

        /// <summary>
        /// Method to get sauda list
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("admin/details")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaDetailsForAdmin", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaDetailsForAdmin([FromBody]string inputKey)
        {
            _methodName = "GetSaudaDetailsForAdmin";
            return Result(inputKey, _methodName, (SaudaDetailInputDto x) => { return _saudaService.GetSaudaDetailsForAdmin(x); });
        }

        /// <summary>
        /// Method to get sauda list
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("status/change")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ChangeSaudaStatus", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ChangeSaudaStatus([FromBody]string inputKey)
        {
            _methodName = "ChangeSaudaStatus";
            return Result(inputKey, _methodName, (SaudaUpdateDto x) => { return _saudaService.ChangeSaudaStatus(x); });
        }

        [HttpPost]
        [Route("status/changeForLoose")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ChangeSaudaStatusForLoose", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ChangeSaudaStatusForLoose([FromBody] string inputKey)
        {
            _methodName = "ChangeSaudaStatusForLoose";
            return Result(inputKey, _methodName, (SaudaUpdateDto x) => { return _saudaService.ChangeSaudaStatusForLoose(x); });
        }

        [HttpPost]
        [Route("saudaconversion/reprocess")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaudaConversionReprocess", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaConversionReprocess([FromBody] string inputKey)
        {
            _methodName = "SaudaConversionReprocess";
            return Result(inputKey, _methodName, (SaudaConversionReprocessDto x) => { return _saudaService.SaudaConversionReprocess(x); });
        }

        [HttpPost]
        [Route("saudaconversion/reject")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaudaConversionReject", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaConversionReject([FromBody] string inputKey)
        {
            _methodName = "SaudaConversionReject";
            return Result(inputKey, _methodName, (SaudaConversionReprocessDto x) => { return _saudaService.SaudaConversionReject(x); });
        }

        [HttpPost]
        [Route("saudaextension/reprocess")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaudaExtensionReprocess", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaudaExtensionReprocess([FromBody] string inputKey)
        {
            _methodName = "SaudaExtensionReprocess";
            return Result(inputKey, _methodName, (SaudaExtensionReprocessDto x) => { return _saudaService.SaudaExtensionReprocess(x); });
        }

        [HttpPost]
        [Route("lifting/reprocess")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "LiftingReprocess", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult LiftingReprocess([FromBody] string inputKey)
        {
            _methodName = "LiftingReprocess";
            return Result(inputKey, _methodName, (LiftingRequestReprocessDto x) => { return _saudaService.LiftingReprocess(x); });
        }

        //[HttpPost]
        //[Route("orderlist")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetSaudhaOrderList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetSaudhaOrderList([FromBody]string inputKey)
        //{
        //    _methodName = "GetSaudhaOrderList";
        //    return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _saudaService.GetSaudhaOrderList(x); });
        //}

        //[HttpPost]
        //[Route("tradeTicket/saudaOrdersList")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetTradeTicketSaudaOrdersMappingList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetTradeTicketSaudaOrdersMappingList([FromBody]string inputKey)
        //{
        //    _methodName = "GetTradeTicketSaudaOrdersMappingList";
        //    return Result(inputKey, _methodName, (TradeTicketSaudaSearchDto x) => { return _saudaService.GetTradeTicketSaudaOrdersMappingList(x); });
        //}

        //[HttpPost]
        //[Route("saudaOrdersMappingDetails")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetSaudaOrdersMappingDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetSaudaOrdersTradeTicketMappingDetails([FromBody]string inputKey)
        //{
        //    _methodName = "GetSaudaOrdersTradeTicketMappingDetails";
        //    return Result(inputKey, _methodName, (IdInputDto x) => { return _saudaService.GetSaudaOrdersTradeTicketMappingDetails(x); });
        //}

        //[HttpPost]
        //[Route("maptradeTickettosaudaorders")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "MapTradeTicketToSaudaOrders", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult MapTradeTicketToSaudaOrders([FromBody]string inputKey)
        //{
        //    _methodName = "MapTradeTicketToSaudaOrders";
        //    return Result(inputKey, _methodName, (TradeTicketMaptoSaudaOrderDto x) => { return _saudaService.MapTradeTicketToSaudaOrders(x); });
        //}

        //[HttpPost]
        //[Route("counterbid/notification")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "SendCounterBidNotification", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult SendCounterBidNotification([FromBody]string inputKey)
        //{
        //    _methodName = "SendCounterBidNotification";
        //    return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _saudaService.SendCounterBidNotification(x); });
        //}

        //[HttpPost]
        //[Route("hold_orders/reject")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "RejectSaudaOrdersInHold", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult RejectSaudaOrdersInHold([FromBody]string inputKey)
        //{
        //    _methodName = "RejectSaudaOrdersInHold";
        //    return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _saudaService.RejectSaudaOrdersInHold(x); });
        //}

        //[HttpPost]
        //[Route("status/notification")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "SendLatestSaudasStatusNotification", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult SendLatestSaudasStatusNotification([FromBody]string inputKey)
        //{
        //    _methodName = "SendLatestSaudasStatusNotification";
        //    return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _saudaService.SendLatestSaudasStatusNotification(x); });
        //}

        #region Sauda Limit
        //[HttpPost]
        //[Route("saudalimit/DealersBySalesPerson")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetDealersBySalesPerson", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetDealersBySalesPerson([FromBody]string inputKey)
        //{
        //    _methodName = "GetDealersBySalesPerson";
        //    return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _saudaService.GetDealersBySalesPerson(x); });
        //}

        [HttpPost]
        [Route("saudalimit/Update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateSaudaLimit", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateSaudaLimit([FromBody]string inputKey)
        {
            _methodName = "UpdateSaudaLimit";
            return Result(inputKey, _methodName, (SaudaLimitRequestHistoryDto x) => { return _saudaService.UpdateSaudaLimit(x); });
        }

        [HttpPost]
        [Route("saudalimit/SaudaLimitsRequestHistory")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaLimitsRequestHistory", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaLimitsRequestHistory([FromBody]string inputKey)
        {
            _methodName = "GetSaudaLimitsRequestHistory";
            return KendoGridResult(inputKey, _methodName, (SaudaLimitInputDto x) => { return _saudaService.GetSaudaLimitsRequestHistory(x); });
        }

        [HttpPost]
        [Route("saudalimit/ApproveorReject")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ApproveorRejectSaudaLimit", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ApproveorRejectSaudaLimit([FromBody]string inputKey)
        {
            _methodName = "ApproveorRejectSaudaLimit";
            return Result(inputKey, _methodName, (SaudaLimitRequestDto x) => { return _saudaService.ApproveorRejectSaudaLimit(x); });
        }
        #endregion

        #region SpecialRate

        [HttpPost]
        [Route("specialRate")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialRateApprovalList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialRateApprovalList([FromBody]string inputKey)
        {
            _methodName = "GetSpecialRateApprovalList";
            return Result(inputKey, _methodName, (SpecialRateAddInputDto x) => { return _saudaService.GetSpecialRateApprovalList(x); });
        }


        [HttpPost]
        [Route("specialRate/ApproveorReject")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ApproveorRejectSpecialRate", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ApproveorRejectSpecialRate([FromBody]string inputKey)
        {
            _methodName = "ApproveorRejectSpecialRate";
            return Result(inputKey, _methodName, (SpecialRateRequestDto x) => { return _saudaService.ApproveorRejectSpecialRate(x); });
        }

        #endregion

        #region Special Rate Approval

        [HttpPost]
        [Route("specialrate/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialRateApprovalListWithAccessPermission", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialRateApprovalListWithAccessPermission([FromBody]string inputKey)
        {
            _methodName = "GetSpecialRateApprovalListWithAccessPermission";
            return Result(inputKey, _methodName, (SpecialRateAddInputDto x) => { return _saudaService.GetSpecialRateApprovalListWithAccessPermission(x); });
        }


        [HttpPost]
        [Route("specialrate/approval")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SpecialRateApproval", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SpecialRateApproval([FromBody]string inputKey)
        {
            _methodName = "SpecialRateApproval";
            return Result(inputKey, _methodName, (SpecialRateApprovalDto x) => { return _saudaService.SpecialRateApproval(x); });
        }

        #endregion

        //ToDo: Now don't wnat use this below code
        #region Final Price
        /*
         [HttpPost]
         [Route("finalprice/admin")]
         [ResponseType(typeof(ContentDto))]
         [Throttle(Name = "SkuFinalpriceListForAdmin", Message = "The request has been declined for security reasons.", Seconds = 1)]
         public IHttpActionResult SkuFinalpriceListForAdmin([FromBody]string inputKey)
         {
             _methodName = "SkuFinalpriceListForAdmin";
             return Result(inputKey, _methodName, (SkuFinalpriceListInputDto x) => { return _saudaService.SkuFinalpriceListForAdmin(x); });
         }


         [HttpPost]
         [Route("finalprice/mobile")]
         [ResponseType(typeof(ContentDto))]
         [Throttle(Name = "SkuFinalpriceListForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
         public IHttpActionResult SkuFinalpriceListForMobile([FromBody]string inputKey)
         {
             _methodName = "SkuFinalpriceListForMobile";
             return Result(inputKey, _methodName, (FinalPriceInputDto x) => { return _saudaService.SkuFinalpriceListForMobile(x); });
         }

         [HttpPost]
         [Route("finalprice/traditional/save")]
         [ResponseType(typeof(ContentDto))]
         [Throttle(Name = "SaveTraditionalFinalPrice", Message = "The request has been declined for security reasons.", Seconds = 1)]
         public IHttpActionResult SaveTraditionalFinalPrice([FromBody]string inputKey)
         {
             _methodName = "SaveTraditionalFinalPrice";
             return Result(inputKey, _methodName, (SaveFinalPricngInputDto x) => { return _saudaService.SaveTraditionalFinalPrice(x); });
         }

         [HttpPost]
         [Route("finalprice/reverseauaction/save")]
         [ResponseType(typeof(ContentDto))]
         [Throttle(Name = "SaveReverseAuactionFinalPrice", Message = "The request has been declined for security reasons.", Seconds = 1)]
         public IHttpActionResult SaveReverseAuactionFinalPrice([FromBody]string inputKey)
         {
             _methodName = "SaveReverseAuactionfinalPrice";
             return Result(inputKey, _methodName, (SaveFinalPricngInputDto x) => { return _saudaService.SaveReverseAuactionFinalPrice(x); });
         }
         */
        #endregion

        #region Sauda Convertion

        [HttpPost]
        [Route("saudaconversion/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaConversionList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaConversionList([FromBody]string inputKey)
        {
            _methodName = "GetSaudaConversionList";
            return Result(inputKey, _methodName, (SaudaConvertionFilterDto x) => { return _saudaService.GetSaudaConversionList(x); });
        }

        [HttpPost]
        [Route("saudaconversion/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaConversionListForExport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaConversionListForExport([FromBody]string inputKey)
        {
            _methodName = "GetSaudaConversionListForExport";
            return Result(inputKey, _methodName, (SaudaConvertionFilterDto x) => { return _saudaService.GetSaudaConversionListForExport(x); });
        }


        [HttpPost]
        [Route("saudaconversiondetails/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaConversionDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaConversionDetails([FromBody]string inputKey)
        {
            _methodName = "GetSaudaConversionDetails";
            return Result(inputKey, _methodName, (SaudaConversionDetailInputDto x) => { return _saudaService.GetSaudaConversionDetails(x); });
        }

        [HttpPost]
        [Route("saudaconversiondetailall/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaConversionAllDetail", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaConversionAllDetail([FromBody]string inputKey)
        {
            _methodName = "GetSaudaConversionAllDetail";
            return Result(inputKey, _methodName, (SaudaConversionDetailInputDto x) => { return _saudaService.GetSaudaConversionAllDetail(x); });
        }

        [HttpPost]
        [Route("approvesaudaconversion")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ApproveSaudaConversion", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ApproveSaudaConversion([FromBody]string inputKey)
        {
            _methodName = "ApproveSaudaConversion";
            return Result(inputKey, _methodName, (SaudaConversionUpdateDto x) => { return _saudaService.ApproveSaudaConversion(x); });
        }

        #endregion

        #region View TP and RA

        [HttpPost]
        [Route("tpandrapricing/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetTPandRAPricingList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetTPandRAPricingList([FromBody]string inputKey)
        {
            _methodName = "GetTPandRAPricingList";
            return Result(inputKey, _methodName, (PricingTPandRAInputDto x) => { return _saudaService.GetTPandRAPricingList(x); });
        }

        #endregion

        #region Sauda Extension

        [HttpPost]
        [Route("saudaextension/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaExtensionList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaExtensionList([FromBody]string inputKey)
        {
            _methodName = "GetSaudaConversionList";
            return Result(inputKey, _methodName, (SaudaConvertionFilterDto x) => { return _saudaService.GetSaudaExtensionList(x); });
        }

        [HttpPost]
        [Route("saudaextension/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportSaudaExtensionList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportSaudaExtensionList([FromBody]string inputKey)
        {
            _methodName = "ExportSaudaExtensionList";
            return Result(inputKey, _methodName, (SaudaConvertionFilterDto x) => { return _saudaService.ExportSaudaExtensionList(x); });
        }

        [HttpPost]
        [Route("saudaextensiondetails/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaExtensionDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaExtensionDetails([FromBody]string inputKey)
        {
            _methodName = "GetSaudaConversionDetails";
            return Result(inputKey, _methodName, (SaudaConversionDetailInputDto x) => { return _saudaService.GetSaudaExtensionDetails(x); });
        }

        [HttpPost]
        [Route("saudaextensiondetailall/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaExtensionAllDetail", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaExtensionAllDetail([FromBody]string inputKey)
        {
            _methodName = "GetSaudaConversionAllDetail";
            return Result(inputKey, _methodName, (SaudaConversionDetailInputDto x) => { return _saudaService.GetSaudaExtensionAllDetail(x); });
        }

        [HttpPost]
        [Route("approvesaudaextension")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ApproveSaudaExtension", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ApproveSaudaExtension([FromBody]string inputKey)
        {
            _methodName = "ApproveSaudaConversion";
            return Result(inputKey, _methodName, (SaudaConversionUpdateDto x) => { return _saudaService.ApproveSaudaExtension(x); });
        }

        [HttpPost]
        [Route("saudaextensiondetails/new/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaExtensionDetailsnew", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaExtensionDetailsnew([FromBody]string inputKey)
        {
            _methodName = "GetSaudaExtensionDetailsnew";
            return Result(inputKey, _methodName, (SaudaConversionDetailInputDto x) => { return _saudaService.GetSaudaExtensionDetailsNew(x); });
        }
        #endregion

        [HttpPost]
        [Route("saudadetails/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateSaudaDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateSaudaDetails([FromBody]string inputKey)
        {
            _methodName = "UpdateSaudaDetails";
            return Result(inputKey, _methodName, (SaudaDetailOutputDto x) => { return _saudaService.UpdateSaudaDetails(x); });
        }



        #region Load Test

        //[HttpPost]
        //[Route("saudaOrdersMappingDetails/loadtest")]
        //[ResponseType(typeof(ContentDto))]
        ////[Throttle(Name = "GetSaudaOrdersMappingDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetSaudaOrdersTradeTicketMappingDetails([FromBody]TradeTicketDto inputKey)
        //{
        //    _methodName = "GetSaudaOrdersTradeTicketMappingDetails";
        //    var result = _saudaService.GetSaudaOrdersTradeTicketMappingDetailsLoadTest(inputKey);
        //    return Ok(result);
        //}


        //Load Test
        [HttpPost]
        [Route("saudastatus/update/loadtest")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult SaudaApproveLoadTest([FromBody]SaudaDto inputKey)
        {
            _methodName = "SaudaApproveLoadTest";
            // return Result(inputKey, _methodName, (SaudaDto x) => { return _saudaService.SaudaApproveLoadTest(x); });
            var result = _saudaService.SaudaApproveLoadTest(inputKey);
            return Ok(result);
        }

        [HttpPost]
        [Route("liftingstatus/update/loadtest")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult LiftingRequestApproveLoadTest([FromBody]SaudaDto inputKey)
        {
            _methodName = "LiftingRequestApproveLoadTest";
            //return Result(inputKey, _methodName, (SaudaDto x) => { return _saudaService.LiftingRequestApproveLoadTest(x); });            
            var result = _saudaService.LiftingRequestApproveLoadTest(inputKey);
            return Ok(result);
        }

        #endregion

        #region Cr for sauda extension
        [HttpPost]
        [Route("saudaextension/bookedSaudaWithextensionDetailsList")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBookedSaudaWithExtensionDetailsList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBookedSaudaWithExtensionDetailsList([FromBody]string inputKey)
        {
            _methodName = "GetBookedSaudaWithExtensionDetailsList";
            return Result(inputKey, _methodName, (SaudaExtensionFilterDto x) => { return _saudaService.GetBookedSaudaWithExtensionDetailsList(x); });
        }

        [HttpPost]
        [Route("saudaextension/pendingandapprovallist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaExtensionPendingAndApprovalList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaExtensionPendingAndApprovalList([FromBody]string inputKey)
        {
            _methodName = "GetSaudaExtensionPendingAndApprovalList";
            return Result(inputKey, _methodName, (SaudaExtensionFilterDto x) => { return _saudaService.GetSaudaExtensionPendingAndApprovalList(x); });
        }

        [HttpPost]
        [Route("saudaextension/pendingandapprovallistforbdo")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaExtensionPendingAndApprovalListForBdo", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaExtensionPendingAndApprovalListForBdo([FromBody]string inputKey)
        {
            _methodName = "GetSaudaExtensionPendingAndApprovalListForBdo";
            return Result(inputKey, _methodName, (SaudaExtensionFilterDto x) => { return _saudaService.GetSaudaExtensionPendingAndApprovalListForBdo(x); });
        }

        [HttpPost]
        [Route("saudaextension/pendingandapprovallistfordealer")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaExtensionPendingAndApprovalListForDealer", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaExtensionPendingAndApprovalListForDealer([FromBody]string inputKey)
        {
            _methodName = "GetSaudaExtensionPendingAndApprovalListForDealer";
            return Result(inputKey, _methodName, (SaudaExtensionFilterDto x) => { return _saudaService.GetSaudaExtensionPendingAndApprovalListForDealer(x); });
        }

        [HttpPost]
        [Route("saudaExtensionDetails/inweb")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetsaudaExtensionDetailsInWeb", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetsaudaExtensionDetailsInWeb([FromBody]string inputKey)
        {
            _methodName = "GetsaudaExtensionDetailsInWeb";
            return KendoGridResult(inputKey, _methodName, (SaudaExtensionFilterDtoForGrid x) => { return _saudaService.GetsaudaExtensionDetailsInWeb(x); });
        }


        [HttpPost]
        [Route("admin/saudaorderdetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaDetail", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaDetail([FromBody] string inputKey)
        {
            _methodName = "GetSaudaDetail";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _saudaService.GetSaudaDetail(x); });
        }
        #endregion


        #region CompetitorAnalysis        

        /// <summary>
        /// Method to Get CompetitorAnalysis List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("competitoranalysis/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCompetitorAnalysisList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCompetitorAnalysisList([FromBody] string inputKey)
        {
            _methodName = "GetCompetitorAnalysisList";
            return KendoGridResult(inputKey, _methodName, (LoginUserIdDto x) => { return _saudaService.GetCompetitorAnalysisList(x); });
        }

        /// <summary>
        /// Method to get Get CompetitorAnalysis Details By Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("competitoranalysis/details")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCompetitorAnalysisById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCompetitorAnalysisById([FromBody] string inputKey)
        {
            _methodName = "GetCompetitorAnalysisById";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _saudaService.GetCompetitorAnalysisById(x); });
        }

        /// <summary>
        /// Method to Update CompetitorAnalysis
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("competitoranalysis/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateCompetitorAnalysis", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateCompetitorAnalysis([FromBody] string inputKey)
        {
            _methodName = "UpdateCompetitorAnalysis";
            return Result(inputKey, _methodName, (CompetitorAnalysisAddDto x) => { return _saudaService.UpdateCompetitorAnalysis(x); });
        }


        /// <summary>
        /// Method to get Get CompetitorAnalysis Details By Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("competitoranalysisdetails/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCompetitorAnalysisDetailsListById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCompetitorAnalysisDetailsListById([FromBody] string inputKey)
        {
            _methodName = "GetCompetitorAnalysisDetailsListById";
            return Result(inputKey, _methodName, (long x) => { return _saudaService.GetCompetitorAnalysisDetailsListById(x); });
        }

        /// <summary>
        /// Method to Save CompetitorAnalysis
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("competitoranalysis/approval")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveCompetitorAnalysisApproval", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveCompetitorAnalysisApproval([FromBody] string inputKey)
        {
            _methodName = "SaveCompetitorAnalysisApproval";
            return Result(inputKey, _methodName, (CompetitorAnalysisApprovalDto x) => { return _saudaService.SaveCompetitorAnalysisApproval(x); });
        }

        #endregion

        
        #region Suada Booking Restriciton List

        [HttpPost]
        [Route("get/saudabooking/restrictionlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaBookingConfigurationList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaBookingConfigurationList([FromBody] string inputKey)
        {
            _methodName = "GetSaudaBookingConfigurationList";
            return Result(inputKey, _methodName, (UserIdDto x) => { return _saudaService.GetSaudaBookingConfigurationList(x); });
        }

        #endregion

        #region Sauda Sales Area Restriction List

        [HttpPost]
        [Route("get/saudabooking/salesarearestrictionlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaSalesAreaRestrictionConfigurationList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaSalesAreaRestrictionConfigurationList([FromBody] string inputKey)
        {
            _methodName = "GetSaudaSalesAreaRestrictionConfigurationList";
            return Result(inputKey, _methodName, (UserIdDto x) => { return _saudaService.GetSaudaSalesAreaRestrictionConfigurationList(x); });
        }

        #endregion

        #region

        [HttpPost]
        [Route("modification/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaModificationList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaModificationList([FromBody] string inputKey)
        {
            _methodName = "GetSaudaModificationList";
            return KendoGridResult(inputKey, _methodName, (SaudaListFilterDto x) => { return _saudaService.GetSaudaModificationList(x); });
        }

        [HttpPost]
        [Route("modification/detailsbyid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaModificationDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaModificationDetailsById([FromBody] string inputKey)
        {
            _methodName = "GetSaudaModificationDetailsById";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _saudaService.GetSaudaModificationDetailsById(x); });
        }

        [HttpPost]
        [Route("modification/details")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudhaModificationDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudhaModificationDetails([FromBody] string inputKey)
        {
            _methodName = "GetSaudhaModificationDetails";
            return Result(inputKey, _methodName, (SaudaDetailInputDto x) => { return _saudaService.GetSaudhaModificationDetails(x); });
        }

        [HttpPost]
        [Route("modification/report")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaModificationReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaModificationReport([FromBody] string inputKey)
        {
            _methodName = "GetSaudaModificationReport";
            return Result(inputKey, _methodName, (SaudaOrderReportInputputDto x) => { return _saudaService.GetSaudaModificationReport(x); });
        }
        #endregion

    }
}
