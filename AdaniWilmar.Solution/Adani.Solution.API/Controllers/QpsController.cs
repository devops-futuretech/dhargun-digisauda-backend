using Adani.Solution.DTO;
using GMCore.Authenticate;
using System.Web.Http;
using Adani.Solution.Service;
using GMCore.Logger;
using Adani.Solution.API.App_Start;
using Adani.Solution.DTO.QPSDiscount;
using System;
using System.Web.Http.Description;
using System.Collections.Generic;

namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [System.Web.Http.RoutePrefix("api/qps")]
    public class QpsController : BaseApiController
    {
        private const string ServiceName = "Qps Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IQpsService _QpsService;
        private string _methodName;

        public QpsController(IQpsService QpsService) : base(ServiceName)
        {
            _methodName = "qps Controller";
            try
            {
                _QpsService = QpsService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        [HttpPost]
        [Route("qpsdiscount/addorupdate")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "QpsAddOrUpdate", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult QpsAddOrUpdate([FromBody] string inputKey)
        {
            _methodName = "QpsAddOrUpdate";
            return Result(inputKey, _methodName, (QPSSchemeDiscountDto x) => { return _QpsService.QpsAddOrUpdate(x); });
        }

        [HttpPost]
        [Route("qpsdiscountlist/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "QpsList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult QpsList([FromBody] string inputKey)
        {
            _methodName = "QpsList";
            return Result(inputKey, _methodName, (QPSSchemeDiscountDto x) => { return _QpsService.QpsList(x); });
        }
        [HttpPost]
        [Route("qpsdiscountlist/getbyId")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "QpsList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult QpsGetById([FromBody] string inputKey)
        {
            _methodName = "QpsList";
            return Result(inputKey, _methodName, (long x) => { return _QpsService.GetQpsDiscountByIdnew(x); });
        }
        [HttpPost]
        [Route("QpsDiscount/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportQpsDiscount", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportQpsDiscount([FromBody] string inputKey)
        {
            _methodName = "ExportQpsDiscount";
            return KendoGridResult(inputKey, _methodName, (LoginUserIdDto x) => { return _QpsService.ExportQpsSchemeDiscount(x); });
        }
        [HttpPost]
        [Route("Qpsdiscountlist/listwithpagination")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetQpsDiscountListWithPagination([FromBody] string inputKey)
        {
            _methodName = "GetQpsDiscountListWithPagination";
            return KendoGridResult(inputKey, _methodName, (KendoGridResult x) => { return _QpsService.GetQpsDiscountListWithPagination(x); });
        }

        [HttpPost]
        [Route("QpsDiscountListWithSlab")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "QpsDiscountListWithSlab", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetQPSDiscountWithSlab([FromBody] string inputKey)
        {
            _methodName = "GetQPSDiscountWithSlab";
            return Result(inputKey, _methodName, (SkuQpsInputDto x) => { return _QpsService.GetQPSDiscountWithSlab(x); });
        }

        [HttpPost]
        [Route("GetQPSDiscountForQuantity")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetQPSDiscountForQuantity", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetQPSDiscountForQuantity([FromBody] string inputKey)
        {
            _methodName = "GetQPSDiscountForQuantity";
            return Result(inputKey, _methodName, (SkuQpsInputDto x) => { return _QpsService.GetQPSDiscountForQuantity(x); });
        }
    }
}