using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Adani.Solution.API.App_Start;
using Adani.Solution.DTO;
using Adani.Solution.Service;
using GMCore.Authenticate;
using GMCore.Logger;

namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/mobileupdate")]
    public class MobileUpdateController : BaseApiController
    {

        private const string ServiceName = "Mobile Update Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IMobileUpdateService _updateService;
        private string _methodName;

        public MobileUpdateController(IMobileUpdateService updateService) : base(ServiceName)
        {
            _methodName = "Mobile update Controller";
            try
            {
                _updateService = updateService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        [HttpGet]
        [Route("feedbacktype/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetFeedbackTypeList", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetFeedbackTypeList()
        {
            _methodName = "GetFeedbackTypeList";
            return Result(_methodName, (() => { return _updateService.GetFeedbackTypeList(); }));
        }

        [HttpPost]
        [Route("feedback/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveFeedback", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult SaveFeedback([FromBody] string inputKey)
        {
            _methodName = "SaveFeedback";
            return Result(inputKey, _methodName, ((FeedbackRequestInputDto z) => { return _updateService.SaveFeedback(z); }));
        }

        [HttpPost]
        [Route("question/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetQuestionList", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetQuestionList([FromBody] string inputKey)
        {
            _methodName = "GetQuestionList";
            return Result(inputKey, _methodName, ((LoginUserIdDto z) => { return _updateService.GetQuestionList(z); }));
        }

        [HttpPost]
        [Route("answer/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddAnswer", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult AddAnswer([FromBody] string inputKey)
        {
            _methodName = "AddAnswer";
            return Result(inputKey, _methodName, ((QuestionSurveyDto z) => { return _updateService.AddAnswer(z); }));
        }

        #region Bulletin
        /// <summary>
        /// Method to Get Bulletin List
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("bulletin/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetBulletinList([FromBody]string inputKey)
        {
            _methodName = "GetBulletinList";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _updateService.GetBulletinList(x); });
        }

        /// <summary>
        /// Method to get Get Bulletin By Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("bulletin/get/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBulletinDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBulletinDetailsById([FromBody]string inputKey)
        {
            _methodName = "GetBulletinDetailsById";
            return Result(inputKey, _methodName, (BulletinInputDto x) => { return _updateService.GetBulletinDetailsById(x); });
        }

        /// <summary>
        /// Method to get Get Latest Update - Bulletin 
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("bulletin/get/latestupdate")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetLatestUpdateBulletin", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetLatestUpdateBulletin([FromBody]string inputKey)
        {
            _methodName = "GetLatestUpdateBulletin";
            return Result(_methodName, () => { return _updateService.GetLatestUpdateBulletin(); });
        }
        #endregion
    }
}