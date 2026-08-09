using Adani.Solution.API.App_Start;
using Adani.Solution.Data;
using Adani.Solution.DTO;
using Adani.Solution.Service;
using GMCore.Authenticate;
using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/mobilefinalprice")]
    public class MobileFinalPriceController : BaseApiController
    {
        private const string ServiceName = "Sauda Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IMobileFinalPriceService _finalPriceService;
        private string _methodName;

        public MobileFinalPriceController(IMobileFinalPriceService finalPriceService) : base(ServiceName)
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
        /// Method to get final price for single sku
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("skunamelist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "FinalPriceSkuNameListForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult FinalPriceSkuNameListForMobile([FromBody]string inputKey)
        {
            _methodName = "FinalPriceSkuNameListForMobile";
            return Result(inputKey, _methodName, (FinalPriceSkuInputDto x) => { return _finalPriceService.FinalPriceSkuNameListForMobile(x); });
        }

        /// <summary>
        /// Method to get final price for single sku
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("specialrateskunamelist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "FinalPriceSkuNameListForSpecialRateMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult FinalPriceSkuNameListForSpecialRateMobile([FromBody]string inputKey)
        {
            _methodName = "FinalPriceSkuNameListForSpecialRateMobile";
            return Result(inputKey, _methodName, (FinalPriceSkuInputDto x) => { return _finalPriceService.FinalPriceSkuNameListForSpecialRateMobile(x); });
        }

        /// <summary>
        /// Method to get final price for single sku
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("skufinalprice")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkuFinalPriceWithBdoDiscountPremiumForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuFinalPriceWithBdoDiscountPremiumForMobile([FromBody]string inputKey)
        {
            _methodName = "GetSkuFinalPriceWithBdoDiscountPremiumForMobile";
            return Result(inputKey, _methodName, (FinalPriceInputDto x) => { return _finalPriceService.GetSkuFinalPriceWithBdoDiscountPremiumForMobile(x); });
        }
    }
}
