using Adani.Solution.API.App_Start;
using Adani.Solution.DTO;
using Adani.Solution.Service;
using GMCore.Authenticate;
using GMCore.Logger;
using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/finalprice")]
    public class FinalPriceController : BaseApiController
    {
        private const string ServiceName = "FinalPrice Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IFinalPriceService _finalPriceService;
        private string _methodName;

        public FinalPriceController(IFinalPriceService finalPriceService) : base(ServiceName)
        {
            _methodName = "Final Price Controller";
            try
            {
                _finalPriceService = finalPriceService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        /// <summary>
        /// Method to final price list for sku
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("list/adminnew")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SkuFinalpriceListForAdminNew", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SkuFinalpriceListForAdminNew([FromBody]string inputKey)
        {
            _methodName = "SkuFinalpriceListForAdminNew";
            return Result(inputKey, _methodName, (SkuFinalpriceListInputDto x) => { return _finalPriceService.SkuFinalpriceListForAdminNew(x); });
        }

        /// <summary>
        /// Method to final price list for sku
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("list/adminupdatednew")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SkuFinalpriceListForAdminNew", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public void SkuFinalpriceListForAdminNew([FromBody]SkuFinalpriceListInputDto inputKey)
        {
            //var input = JsonHelper.ConvertJSonToObject<SkuFinalpriceListInputDto>(inputKey);
            _finalPriceService.SkuFinalpriceListForAdminUpdatedNew(inputKey);
            //_methodName = "SkuFinalpriceListForAdminNew";
            //return Result(inputKey, _methodName, (SkuFinalpriceListInputDto x) => { return _finalPriceService.SkuFinalpriceListForAdminUpdatedNew(x); });
        }

        /// <summary>
        /// Method to final price list for sku
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("list/admin")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SkuFinalpriceListForAdmin", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SkuFinalpriceListForAdmin([FromBody]string inputKey)
        {
            _methodName = "SkuFinalpriceListForAdmin";
            return Result(inputKey, _methodName, (SkuFinalpriceListInputDto x) => { return _finalPriceService.SkuFinalpriceListForAdmin(x); });
        }



        [HttpPost]
        [Route("traditional/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveTraditionalProcessFinalPrice", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveTraditionalProcessFinalPrice([FromBody]string inputKey)
        {
            _methodName = "SaveTraditionalProcessFinalPrice";
            return Result(inputKey, _methodName, (SaveFinalPricngInputDto x) => { return _finalPriceService.SaveTraditionalProcessFinalPrice(x); });
        }

        [HttpPost]
        [Route("reverseauction/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveReverseAuctionFinalPrice", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveReverseAuctionFinalPrice([FromBody]string inputKey)
        {
            _methodName = "SaveReverseAuctionFinalPrice";
            return Result(inputKey, _methodName, (SaveFinalPricngInputDto x) => { return _finalPriceService.SaveReverseAuctionFinalPrice(x); });
        }

        [HttpPost]
        [Route("publish")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "PublishFinalPrice", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult PublishFinalPrice([FromBody]string inputKey)
        {
            _methodName = "PublishFinalPrice";
            return Result(inputKey, _methodName, (FinalPricePublishDto x) => { return _finalPriceService.PublishFinalPrice(x); });
        }

        [HttpPost]
        [Route("price/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkuFinalPriceList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuFinalPriceList([FromBody]string inputKey)
        {
            _methodName = "GetSkuFinalPriceList";
            return Result(inputKey, _methodName, (FinalPricePublishDto x) => { return _finalPriceService.GetSkuFinalPriceList(x); });
        }


        #region Published Price

        /// <summary>
        /// Method to Get published price details
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("publishedprice/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPublishedPriceDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPublishedPriceDetails([FromBody]string inputKey)
        {
            _methodName = "GetPublishedPriceDetails";
            return Result(inputKey, _methodName, (PricePublishInputDto x) => { return _finalPriceService.GetPublishedPriceDetails(x); });
        }

        /// <summary>
        /// Method to final price list for sku
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pricegenerate/queue")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveFinalPrice", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveFinalPrice([FromBody]string inputKey)
        {
            _methodName = "SaveFinalPrice";
            return Result(inputKey, _methodName, (SkuFinalpriceListInputDto x) => { return _finalPriceService.SaveFinalPrice(x); });
        }

        /// <summary>
        /// Method to Get published price error details
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("publishedprice/errorlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPublishedPriceErrorDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPublishedPriceErrorDetails([FromBody]string inputKey)
        {
            _methodName = "GetPublishedPriceErrorDetails";
            return Result(inputKey, _methodName, (PricePublishInputDto x) => { return _finalPriceService.GetPublishedPriceErrorDetails(x); });
        }
        #endregion

        [HttpPost]
        [Route("pricingdata/backup")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "PricingDataBackup", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult PricingDataBackup([FromBody]string inputKey)
        {
            _methodName = "PricingDataBackup";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _finalPriceService.PricingDataBackup(x); });
        }

        #region New FinalPrice - State Based



        [HttpPost]
        [Route("getpricegenerate")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPriceGenerates", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPriceGenerates([FromBody]string inputKey)
        {
            _methodName = "GetPriceGenerates";
            return Result(inputKey, _methodName, (PricePublishInputDto x) => { return _finalPriceService.GetPriceGenerates(x); });
        }

        [HttpPost]
        [Route("getpricegenerate/details")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPriceGenerateDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPriceGenerateDetails([FromBody]string inputKey)
        {
            _methodName = "GetPriceGenerateDetails";
            return Result(inputKey, _methodName, (PricePublishInputDto x) => { return _finalPriceService.GetPriceGenerateDetails(x); });
        }

        [HttpPost]
        [Route("publish/state")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "StateBasePublishFinalPrice", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult StateBasePublishFinalPrice([FromBody]string inputKey)
        {
            _methodName = "StateBasePublishFinalPrice";
            return Result(inputKey, _methodName, (FinalPricePublishDto x) => { return _finalPriceService.FinalPriceBulkPublish(x); });
        }

        [HttpPost]
        [Route("price/state/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetStateBaseFinalPriceList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetStateBaseFinalPriceList([FromBody]string inputKey)
        {
            _methodName = "GetStateBaseFinalPriceList";
            return Result(inputKey, _methodName, (FinalPricePublishDto x) => { return _finalPriceService.GetStateBaseFinalPriceList(x); });
        }

        /// <summary>
        /// Method to Get published price details
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("publishedprice/state/errorlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetStateBasePublishedPriceDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetStateBasePublishedPriceDetails([FromBody]string inputKey)
        {
            _methodName = "GetStateBasePublishedPriceDetails";
            return Result(inputKey, _methodName, (PricePublishInputDto x) => { return _finalPriceService.GetStateBasePublishedPriceErrorDetails(x); });
        }

        #endregion

        #region RA2.0 Final Price

        [HttpPost]
        [Route("ra2reverseauction/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "RaFinalPricePriceGenerate", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult RaFinalPricePriceGenerate([FromBody]string inputKey)
        {
            _methodName = "RaFinalPricePriceGenerate";
            return Result(inputKey, _methodName, (RaFinalPriceGenerateInputDto x) => { return _finalPriceService.RaFinalPricePriceGenerate(x); });
        }

        [HttpPost]
        [Route("ra2getpricegenerate")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "RaGetFinalPriceGenerates", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult RaGetFinalPriceGenerates([FromBody]string inputKey)
        {
            _methodName = "GetPriceGenerates";
            return Result(inputKey, _methodName, (RaPricePublishInputDto x) => { return _finalPriceService.RaGetFinalPriceGenerates(x); });
        }

        [HttpPost]
        [Route("ra2getpricegenerate/details")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "RaGetFinalPriceGenerateDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult RaGetFinalPriceGenerateDetails([FromBody]string inputKey)
        {
            _methodName = "RaGetFinalPriceGenerateDetails";
            return Result(inputKey, _methodName, (RaPricePublishInputDto x) => { return _finalPriceService.RaGetFinalPriceGenerateDetails(x); });
        }

        #endregion

        #region

        [HttpPost]
        [Route("publishedprice/download")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ZoneBasedFinalPriceDownload", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ZoneBasedFinalPriceDownload([FromBody]string inputKey)
        {
            _methodName = "ZoneBasedFinalPriceDownload";
            return Result(inputKey, _methodName, (PriceDownloadInputDto x) => { return _finalPriceService.ZoneBasedFinalPriceDownload(x); });
        }

        [HttpPost]
        [Route("publishedprice/successdownload")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DownloadPriceGenerateSuccessList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DownloadPriceGenerateSuccessList([FromBody]string inputKey)
        {
            _methodName = "DownloadPriceGenerateSuccessList";
            return Result(inputKey, _methodName, (PriceDownloadInputDto x) => { return _finalPriceService.DownloadPriceGenerateSuccessList(x); });
        }

        [HttpPost]
        [Route("publishedprice/errordownload")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DownloadPriceGenerateErrorList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DownloadPriceGenerateErrorList([FromBody]string inputKey)
        {
            _methodName = "DownloadPriceGenerateErrorList";
            return Result(inputKey, _methodName, (PriceDownloadInputDto x) => { return _finalPriceService.DownloadPriceGenerateErrorList(x); });
        }

        #endregion

    }
}
