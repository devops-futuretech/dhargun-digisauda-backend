using Adani.Solution.API.App_Start;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service;
using Adani.Solution.Service.Common;
using GMCore.Authenticate;
using GMCore.Helper;
using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;

namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/reverseauction")]
    public class ReverseAuctionController : BaseApiController
    {
        private const string ServiceName = "Reverse Auction Controller";
        private readonly IReverseAuctionService _reverseAuctionService;
        private string _methodName;

        public ReverseAuctionController(IReverseAuctionService reverseAuctionService)
            : base(ServiceName)
        {
            _methodName = "Reverse Auction Controller";
            try
            {
                _reverseAuctionService = reverseAuctionService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        [HttpPost]
        [Route("bidwindowtiming/addorupdate")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddOrUpdateBiddingWindowTiming", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddOrUpdateBiddingWindowTiming([FromBody]string inputKey)
        {
            _methodName = "SaveBidWindowTiming";
            return Result(inputKey, _methodName, (BiddingWindowTimingDto x) => { return _reverseAuctionService.AddOrUpdateBiddingWindowTiming(x); });
        }

        [HttpPost]

        [Route("bidwindowtiming/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveBidWindowTiming", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBiddingWindowTiming([FromBody]string inputKey)
        {
            _methodName = "GetBiddingWindowTiming";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _reverseAuctionService.GetBiddingWindowTimingList(x); });
        }

        [HttpPost]
        [Route("bidwindowtiming/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveBidWindowTiming", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBiddingWindowTimingById([FromBody]string inputKey)
        {
            _methodName = "GetBiddingWindowTiming";
            return Result(inputKey, _methodName, (long x) => { return _reverseAuctionService.GetBiddingWindowTimingById(x); });
        }

        [HttpPost]
        [Route("ticker/addorupdate")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddOrUpdateBiddingWindowTiming", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddOrUpdateTicker([FromBody]string inputKey)
        {
            _methodName = "SaveBidWindowTiming";
            return Result(inputKey, _methodName, (TickerDto x) => { return _reverseAuctionService.AddOrUpdateTicker(x); });
        }

        [HttpPost]

        [Route("ticker/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveBidWindowTiming", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetTickerList([FromBody]string inputKey)
        {
            _methodName = "GetTickerList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _reverseAuctionService.GetTickerList(x); });
        }

        [HttpPost]
        [Route("ticker/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetTickerById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetTickerById([FromBody]string inputKey)
        {
            _methodName = "GetBiddingWindowTiming";
            return Result(inputKey, _methodName, (long x) => { return _reverseAuctionService.GetTickerById(x); });
        }

        [HttpPost]
        [Route("bidwindowtiming/ddl")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBiddingWindowTimingListddl", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBiddingWindowTimingListddl([FromBody]string inputKey)
        {
            _methodName = "GetBiddingWindowTimingListddl";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _reverseAuctionService.GetBiddingWindowTimingListddl(x); });
        }

        [HttpPost]
        [Route("bidwindowtimingddl/biddingdate")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBiddingWindowTimingListByDateddl", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBiddingWindowTimingListByDateddl([FromBody]string inputKey)
        {
            _methodName = "GetBiddingWindowTimingListByDateddl";
            return Result(inputKey, _methodName, (BiddingWindowInputDto x) => { return _reverseAuctionService.GetBiddingWindowTimingListByDateddl(x); });
        }

        

        
    }
}
