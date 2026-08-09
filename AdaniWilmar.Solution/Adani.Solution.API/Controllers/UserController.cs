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
    [RoutePrefix("api/user")]
    public class UserController : BaseApiController
    {
        private const string ServiceName = "User Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IUserService _userService;
        private string _methodName;

        public UserController(IUserService userService) : base(ServiceName)
        {
            _methodName = "User Controller";
            try
            {
                _userService = userService;
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
        [Route("dealerlist/userid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerListByUserid", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerListByUserid([FromBody]string inputKey)
        {
            _methodName = "GetDealerListByUserid";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _userService.GetDealerListByUserid(x); });
        }

        [HttpPost]
        [Route("dealerlist/all")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerListAll", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerListAll([FromBody]string inputKey)
        {
            _methodName = "GetDealerListAll";
            return Result(inputKey, _methodName, (DealerListAllFilterDto x) => { return _userService.GetDealerListAll(x); });
        }
        /// <summary>
        /// Method to get sauda list for admin
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("shiptoparty/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetShipToPartyListByCustomerId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetShipToPartyListByCustomerId([FromBody]string inputKey)
        {
            _methodName = "GetShipToPartyListByCustomerId";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _userService.GetShipToPartyListByCustomerId(x); });
        }

        /// <summary>
        /// Method to get sauda list
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("brokerlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBrokerList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBrokerList([FromBody]string inputKey)
        {
            _methodName = "GetBrokerList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _userService.GetBrokerList(x); });
        }

        /// <summary>
        /// Method to get sauda list
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pushtoken/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddDevicePushToken", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddDevicePushToken([FromBody]string inputKey)
        {
            _methodName = "AddDevicePushToken";
            return Result(inputKey, _methodName, (PushTokenInputDto x) => { return _userService.AddDevicePushToken(x); });
        }

        /// <summary>
        /// Method to get sauda list
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("pushtoken/Exists")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "CheckDevicePushTokenExists", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult CheckDevicePushTokenExists([FromBody]string inputKey)
        {
            _methodName = "CheckDevicePushTokenExists";
            return Result(inputKey, _methodName, (PushTokenInputDto x) => { return _userService.CheckDevicePushTokenExists(x); });
        }

        /// <summary>
        /// Method to save login details
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("logintime/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveUserLoginTime", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveUserLoginTime([FromBody]string inputKey)
        {
            _methodName = "SaveUserLoginTime";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _userService.SaveUserLoginTime(x); });
        }

        [HttpPost]
        [Route("StateTrader/ddl")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBDOListByStates", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBDOListByStates([FromBody]string inputKey)
        {
            _methodName = "GetBDOListByStates";
            return Result(inputKey, _methodName, (List<long> x) => { return _userService.GetBDOListByStates(x); });
        }

        [HttpPost]
        [Route("StateTrader/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBDOList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBDOList([FromBody]string inputKey)
        {
            _methodName = "GetBDOList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _userService.GetBDOList(x); });
        }

        [HttpPost]
        [Route("GetUserLoginHistory/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserLoginHistory", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserLoginHistory([FromBody] string inputKey)
        {
            _methodName = "GetUserLoginHistory";
            return Result(inputKey, _methodName, (UserLoginHistoryDto x) => { return _userService.GetUserLoginHistory(x); });
        }

        [HttpPost]
        [Route("dealerlist/pendingcontractsanduserid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealerListByPendingContractsAndUserid", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealerListByPendingContractsAndUserid([FromBody] string inputKey)
        {
            _methodName = "GetDealerListByPendingContractsAndUserid";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _userService.GetDealerListByPendingContractsAndUserid(x); });
        }
    }
}
