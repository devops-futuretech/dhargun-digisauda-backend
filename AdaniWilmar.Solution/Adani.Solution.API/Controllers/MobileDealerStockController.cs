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
    [RoutePrefix("api/mobileDealerStock")]
    public class MobileDealerStockController : BaseApiController
    {
        private const string ServiceName = "Mobile Dealer Stock Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IMobileDealerStockService _dealerStockService;
        private string _methodName;

        public MobileDealerStockController(IMobileDealerStockService dealerStockService) : base(ServiceName)
        {
            _methodName = "Mobile Dealer Stock Controller";
            try
            {
                _dealerStockService = dealerStockService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        /// <summary>
        /// Method to get the Sku dropdown for the distributor stock entry screen,
        /// scoped to the distributor's sales organization/distribution channel/division
        /// combinations, with the case to MT conversion value per Sku.
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("skulist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSkuListForStockEntry", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuListForStockEntry([FromBody]string inputKey)
        {
            _methodName = "GetSkuListForStockEntry";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _dealerStockService.GetSkuListForStockEntry(x); });
        }

        /// <summary>
        /// Method to save one distributor stock entry (list of Skus with number of cases).
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("entry/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveDistributorStockEntry", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveDistributorStockEntry([FromBody]string inputKey)
        {
            _methodName = "SaveDistributorStockEntry";
            return Result(inputKey, _methodName, (DistributorStockEntrySaveDto x) => { return _dealerStockService.SaveDistributorStockEntry(x); });
        }

        /// <summary>
        /// Method to get the distributor's own stock entries, paginated - one item per
        /// entry with the reported Sku lines nested.
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("entry/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDistributorStockEntryList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDistributorStockEntryList([FromBody]string inputKey)
        {
            _methodName = "GetDistributorStockEntryList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _dealerStockService.GetDistributorStockEntryList(x); });
        }

        /// <summary>
        /// Method to get the latest reported stock per Sku of a selected distributor,
        /// for the State/Zonal/National trader views.
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("dealer/lateststock")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerLatestStockPerSku", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerLatestStockPerSku([FromBody]string inputKey)
        {
            _methodName = "GetDealerLatestStockPerSku";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _dealerStockService.GetDealerLatestStockPerSku(x); });
        }
    }
}
