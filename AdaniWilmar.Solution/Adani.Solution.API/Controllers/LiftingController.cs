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
using System.Web.Http;
using System.Web.Http.Description;
namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/lifting")]
    public class LiftingController : BaseApiController
    {
        private const string ServiceName = "Lifting Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly ILiftingService _liftingService;
        private readonly ISAPIntegrationService _sapIntegrationService;
        private string _methodName;

        public LiftingController(ILiftingService liftingService, ISAPIntegrationService sapIntegrationService) : base(ServiceName)
        {
            _methodName = "Lifting Controller";
            try
            {
                _liftingService = liftingService;
                _sapIntegrationService = sapIntegrationService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        /// <summary>
        /// Method to Get Lifting Request List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("liftingRequest/lists")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetLiftingRequestLists", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetLiftingRequestLists([FromBody]string inputKey)
        {
            _methodName = "GetLiftingRequestLists";
            return Result(inputKey, _methodName, (DealersLiftingRequestInputDto x) => { return _liftingService.GetLiftingRequestList(x); });
        }

        /// <summary>
        /// Method to Get Lifting Request detail
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("liftingRequest/detail")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetLiftingRequestDetail", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetLiftingRequestDetail([FromBody]string inputKey)
        {
            _methodName = "GetLiftingRequestDetail";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _liftingService.GetLiftingRequestDetail(x); });
        }

        /// <summary>
        /// Method to Lifting Request Status Change
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("liftingRequest/statuschange")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "LiftingRequestStatusChange", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult LiftingRequestStatusChange([FromBody]string inputKey)
        {
            _methodName = "LiftingRequestStatusChange";
            return Result(inputKey, _methodName, (LiftingRequestStatusChangeDto x) => { return _liftingService.LiftingRequestStatusChange(x); });
        }

        /// <summary>
        /// Method to Lifting Request Creation
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("liftingRequest/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "LiftingRequestCreation", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult LiftingRequestCreation([FromBody]string inputKey)
        {
            _methodName = "LiftingRequestCreation";
            return Result(inputKey, _methodName, (LiftingRequestInputDto x) => { return _liftingService.LiftingRequestCreation(x); });
        }

        ///// <summary>
        ///// Method to Get Confirmed Lifting Request Lists
        ///// </summary>
        ///// <param name="inputKey"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("liftingRequest/ConfirmList")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetConfirmedLiftingRequestLists", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetConfirmedLiftingRequestLists([FromBody]string inputKey)
        //{
        //    _methodName = "GetConfirmedLiftingRequestLists";
        //    return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _liftingService.GetConfirmedLiftingRequestLists(x); });
        //}

        ///// <summary>
        ///// Method to Get InProgress Lifting Request Lists
        ///// </summary>
        ///// <param name="inputKey"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("liftingRequest/InprogressList")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetInProgressLiftingRequestLists", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetInProgressLiftingRequestLists([FromBody]string inputKey)
        //{
        //    _methodName = "GetInProgressLiftingRequestLists";
        //    return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _liftingService.GetInProgressLiftingRequestLists(x); });
        //}

        /// <summary>
        /// Method to Get Lifting Request Lists
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("liftingRequest/List")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetLiftingRequestCountList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetLiftingRequestCountList([FromBody]string inputKey)
        {
            _methodName = "GetLiftingRequestCountList";
            return Result(inputKey, _methodName, (LiftingRequestListInputDto x) => { return _liftingService.GetLiftingRequestCountList(x); });
        }

        /// <summary>
        /// Method to Get Dealers Lifting Request List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("liftingRequest/DealersLiftingRequestList")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetDealersLiftingRequestList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealersLiftingRequestList([FromBody]string inputKey)
        {
            _methodName = "GetDealersLiftingRequestList";
            return Result(inputKey, _methodName, (DealersLiftingRequestInputDto x) => { return _liftingService.GetDealersLiftingRequestList(x); });
        }

        [HttpPost]
        [Route("liftingRequest/VehicleLodabilityList")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetDealersLiftingRequestList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetVehicleLodabilityList([FromBody]string inputKey)
        {
            _methodName = "GetVehicleLodabilityList";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _liftingService.GetVehicleLodabilityList(x); });
        }

        #region Lifting Request - Web

        [HttpPost]
        [Route("liftingRequestWeb/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetLiftingRequestListForWeb", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetLiftingRequestListForWeb([FromBody]string inputKey)
        {
            _methodName = "GetLiftingRequestListForWeb";
            return Result(inputKey, _methodName, (DealersLiftingRequestInputDto x) => { return _liftingService.GetLiftingRequestListForWeb(x); });
        }

        [HttpPost]
        [Route("liftingRequestWeb/WithoutEnquiryNumber/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetLiftingRequestWithoutEnquiryNumberListForWeb", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetLiftingRequestWithoutEnquiryNumberListForWeb([FromBody]string inputKey)
        {
            _methodName = "GetLiftingRequestWithoutEnquiryNumberListForWeb";
            return KendoGridResult(inputKey, _methodName, (DealersLiftingRequestInputDto x) => { return _liftingService.GetLiftingRequestWithoutEnquiryNumberListForWeb(x); });
        }
       
        [HttpPost]
        [Route("liftingRequestWeb/detail")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetLiftingRequestDetailsForWeb", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetLiftingRequestDetailsForWeb([FromBody]string inputKey)
        {
            _methodName = "GetLiftingRequestDetailsForWeb";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _liftingService.GetLiftingRequestDetailsForWeb(x); });
        }

        /// <summary>
        /// Method to Lifting Request Status Change
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("liftingRequest/statuschanges")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "LiftingRequestStatusChanges", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult LiftingRequestStatusChangeLiftingRequestStatusChanges([FromBody]string inputKey)
        {
            _methodName = "LiftingRequestStatusChanges";
            return Result(inputKey, _methodName, (LiftingRequestStatusChangeDto x) => { return _liftingService.LiftingRequestStatusChanges(x); });
        }

        /// <summary>
        /// Method to Lifting Request Status Change
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("liftingRequest/admin/approve")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "LiftingRequestApproveForAdmin", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult LiftingRequestApproveForAdmin([FromBody]string inputKey)
        {
            _methodName = "LiftingRequestApproveForAdmin";
            return Result(inputKey, _methodName, (LiftingRequestStatusChangeDto x) => { return _liftingService.LiftingRequestApproveForAdmin(x); });
        }

        [HttpPost]
        [Route("liftingRequestWeb/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetLiftingRequestListForExport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetLiftingRequestListForExport([FromBody]string inputKey)
        {
            _methodName = "GetLiftingRequestListForExport";
            return Result(inputKey, _methodName, (DealersLiftingRequestInputDto x) => { return _liftingService.GetLiftingRequestListForExport(x); });
        }

        #endregion

        #region Sauda Order Lifting Request

        [HttpPost]
        [Route("saudaorder/liftingrequest")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaOrderLiftingRequestDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaOrderLiftingRequestDetails([FromBody]string inputKey)
        {
            _methodName = "GetSaudaOrderLiftingRequestDetails";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _liftingService.GetSaudaOrderLiftingRequestDetails(x); });
        }

        [HttpPost]
        [Route("saudaorder/liftingrequestexcelexport")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaOrderLiftingRequestExcelExport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaOrderLiftingRequestExcelExport([FromBody]string inputKey)
        {
            _methodName = "GetSaudaOrderLiftingRequestExcelExport";
            return Result(inputKey, _methodName, (DealersLiftingRequestInputDto x) => { return _liftingService.GetSaudaOrderLiftingRequestExcelExport(x); });
        }

        #endregion

        #region Load Test

        [HttpPost]
        [Route("liftingRequest/add/loadtest")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "LoadTestLiftingRequestCreation", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult LoadTestLiftingRequestCreation([FromBody]LiftingRequestInputDto inputKey)
        {
            //return Result(inputKey, _methodName, (LiftingRequestInputDto x) => { return _liftingService.LiftingRequestCreation(x); });
            _methodName = "LoadTestLiftingRequestCreation";
            var result = _liftingService.LiftingRequestCreation(inputKey);
            return Ok(result);
        }

        #endregion

        #region Lifting/Indent List - Mobile

        [HttpPost]
        [Route("liftingRequest/list/mobile")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetLiftingRequestListForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetLiftingRequestListForMobile([FromBody] string inputKey)
        {
            _methodName = "GetLiftingRequestListForMobile";
            return Result(inputKey, _methodName, (LiftingRequestListsInputDto x) => { return _liftingService.GetLiftingRequestListForMobile(x); });
        }

        [HttpPost]
        [Route("liftingRequest/detail/mobile")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetLiftingRequesListForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetLiftingRequestSODetailsForMobile([FromBody] string inputKey)
        {
            _methodName = "GetLiftingRequestSODetailsForMobile";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _liftingService.GetLiftingRequestSODetailsForMobile(x); });
        }

        #endregion

        [HttpPost]
        [Route("liftingRequest/detailforpopup")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetLiftingRequestDetailForPopup", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetLiftingRequestDetailForPopup([FromBody] string inputKey)
        {
            _methodName = "GetLiftingRequestDetailForPopup";
            return Result(inputKey, _methodName, (SalesOrderInputDto x) => { return _liftingService.GetLiftingRequestDetailForPopup(x); });
        }


        [Route("PendingContractTrigger")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "PendingContractTrigger", Message = "The request has been declined for security reasons.", Seconds = 3)]
        public IHttpActionResult PendingContractTrigger([FromBody] string inputKey)
        {
            _methodName = "PendingContractTrigger";
            return Result(inputKey, _methodName, (OpenContractRequestDTOList x) => { return _sapIntegrationService.ContractTrigger(x); });
        }

    }
}
