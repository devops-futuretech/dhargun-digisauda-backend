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
    [RoutePrefix("api/ChatBot")]
    public class ChatBotController : BaseApiController
    {
        private const string ServiceName = "ChatBot Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IChatBotService _chatBotService ;
        private string _methodName;

        public ChatBotController(IChatBotService chatBotService) : base(ServiceName)
        {
            _methodName = "ChatBot Controller";
            try
            {
                _chatBotService = chatBotService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        [HttpPost]
        [Route("get/dealerId")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerIdByDealerCode", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public ResultDto GetDealerIdByDealerCode(DealerDto dealerDto)
        {
            _methodName = "GetDealerIdByDealerCode";
            return _chatBotService.GetDealerIdByDealerCode(dealerDto);
        }

        //[HttpPost]
        //[Route("get/PendingSaudaAndDue")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetPendingSaudaAndDue", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public ResultDto GetPendingSaudaAndDue(UserIdDto userIdDto)
        //{
        //    _methodName = "GetPendingSaudaAndDue";
        //    return _chatBotService.GetPendingSaudaAndDueDetails(userIdDto);
        //}

        [HttpPost]
        [Route("get/SpecialRateApproval")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialRateApprovalsList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public ResultDto GetSpecialRateApprovalsList(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetSpecialRateApprovalsList";
            return _chatBotService.GetSpecialRateApprovalsList(loginUserIdDto);
        }

        //[HttpPost]
        //[Route("OverallSales")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "OverallSales", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult OverallSales([FromBody]LoginUserIdDto inputKey)
        //{
        //    _methodName = "OverallSales";
        //     var result = _chatBotService.OverallSales(inputKey);
        //    return Ok(result);
        //}

     

        [HttpPost]
        [Route("oiltype/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetOilTypeList([FromBody]LoginUserIdDto inputKey)
        {
            _methodName = "GetOilTypeList";
            var result = _chatBotService.GetOilType(inputKey);
            return Ok(result);
        }

        [HttpGet]
        [Route("incoterm/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetIncoTermsList", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetIncoTermsList()
        {
            _methodName = "GetIncoTermsList";
            var result = _chatBotService.GetIncoTermsList();
            return Ok(result);
        }

        [HttpPost]
        [Route("BDOPlantDepotDetailsByDealer")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "BDOPlantDepotDetailsByDealer", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult BDOPlantDepotDetailsByDealer([FromBody]LoginUserIdDto inputKey)
        {
            _methodName = "BDOPlantDepotDetailsByDealer";
            var result = _chatBotService.BDOPlantDepotDetailsByDealer(inputKey);
            return Ok(result);
            
        }


        [HttpPost]
        [Route("get/limitenhancement")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetLimitEnhancementDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public ResultDto GetLimitEnhancementDetails(IdInputDto idInputDto)
        {
            _methodName = "GetLimitEnhancementDetails";
            return _chatBotService.GetLimitEnhancementDetails(idInputDto);
        }

        [HttpPost]
        [Route("get/dailyrate")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDailyRateDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public ResultDto GetDailyRateDetails(DailyRateInputDto idInputDto)
        {
            _methodName = "GetDailyRateDetails";
            return _chatBotService.GetDailyRateDetails(idInputDto);
        }
    }
}
