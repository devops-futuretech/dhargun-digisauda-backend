using Adani.Solution.API.App_Start;
using Adani.Solution.DTO;
using Adani.Solution.Service;
using GMCore.Authenticate;
using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/crossandupsell")]
    public class CrossAndUpsellController : BaseApiController
    {
        private const string ServiceName = "CrossAndUpsell Controller";
        private new readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly ICrossAndUpsellService _crossAndUpsellService;
        private string _methodName;
        public CrossAndUpsellController(ICrossAndUpsellService crossAndUpsellService): base(ServiceName)
        { 
            _methodName = "API Report Controller";
            try
            {
                _crossAndUpsellService = crossAndUpsellService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        [HttpPost]
        [Route("add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddCrossAndUpsellConfigurations", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddCrossAndUpsellConfigurations([FromBody] string inputKey)
        {
            _methodName = "AddCrossAndUpsellConfigurations";
            return Result(inputKey, _methodName, (CrossAndUpsellConfigurationDto x) => { return _crossAndUpsellService.AddCrossAndUpsellConfigurations(x); });
        }

        [HttpPost]
        [Route("update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddAndUpdateCrossAndUpsellConfigurations", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateCrossAndUpsellConfigurations([FromBody] string inputKey)
        {
            _methodName = "UpdateCrossAndUpsellConfigurations";
            return Result(inputKey, _methodName, (CrossAndUpsellConfigurationDto x) => { return _crossAndUpsellService.UpdateCrossAndUpsellConfigurations(x); });
        }

        [HttpPost]
        [Route("get/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCrossAndUpsellConfigurationList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCrossAndUpsellConfigurationList([FromBody] string inputKey)
        {
            _methodName = "GetCrossAndUpsellConfigurationList";
            return Result(inputKey, _methodName, (SuadaConditionalBookingInputDto x) => {return _crossAndUpsellService.GetCrossAndUpsellConfigurationList(x);});
        }

        [HttpPost]
        [Route("get/skus/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCrossAndUpsellConfigurationSkusList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCrossAndUpsellConfigurationSkusList([FromBody] string inputKey)
        {
            _methodName = "GetCrossAndUpsellConfigurationSkusList";
            return Result(inputKey, _methodName, (SuadaConditionalBookingInputDto x) => { return _crossAndUpsellService.GetCrossAndUpsellConfigurationSkusList(x); });
        }

        [HttpPost]
        [Route("get/details")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCrossAndUpsellConfigurationDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCrossAndUpsellConfigurationDetails([FromBody] string inputKey)
        {
            _methodName = "GetCrossAndUpsellConfigurationDetails";
            return Result(inputKey, _methodName, (SuadaConditionalBookingInputDto x) => { return _crossAndUpsellService.GetCrossAndUpsellConfigurationDetails(x); });
        }

        [HttpPost]
        [Route("get/mandatory/skus")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCrossAndUpsellMandatorySkusConfigurationDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCrossAndUpsellMandatorySkusConfigurationDetails([FromBody] string inputKey)
        {
            _methodName = "GetCrossAndUpsellMandatorySkusConfigurationDetails";
            return Result(inputKey, _methodName, (SuadaConditionalBookingSkusInputDto x) => { return _crossAndUpsellService.GetCrossAndUpsellMandatorySkusConfigurationDetails(x); });
        }

        [HttpPost]
        [Route("get/report/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCrossAndUpsellConfigurationListForReport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCrossAndUpsellConfigurationListForReport([FromBody] string inputKey)
        {
            _methodName = "GetCrossAndUpsellConfigurationSkusListForReport";
            return Result(inputKey, _methodName, (SuadaConditionalBookingInputDto x) => { return _crossAndUpsellService.GetCrossAndUpsellConfigurationListForReport(x); });
        }
    }
}