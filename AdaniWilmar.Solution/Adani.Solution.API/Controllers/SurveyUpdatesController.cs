using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GMCore.Logger;
using GMCore.Authenticate;
using GMCore.Helper;
using Adani.Solution.DTO;
using Adani.Solution.Service;
using Adani.Solution.Service.Common;
using Adani.Solution.DTO.Common;
using System.Reflection;
using System.Linq.Expressions;
using Adani.Solution.API.App_Start;

namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/surveyupdates")]
    public class SurveyUpdatesController : BaseApiController
    {
        private const string ServiceName = "Survey Updates Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly ISurveyUpdatesService _surveyUpdatesService;
        private string _methodName;

        public SurveyUpdatesController(ISurveyUpdatesService surveyUpdatesService)
           : base(ServiceName)
        {
            _methodName = "Survey Updates Controller";
            try
            {
                _surveyUpdatesService = surveyUpdatesService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        #region Question

        /// <summary>
        /// Method to Save Question
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("question/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveQuestion", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveQuestion([FromBody]string inputKey)
        {
            _methodName = "SaveQuestion";
            return Result(inputKey, _methodName, (QuestionDto x) => { return _surveyUpdatesService.SaveQuestion(x); });
        }


        /// <summary>
        /// Method to Get Question List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("question/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetQuestionList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetQuestionList([FromBody]string inputKey)
        {
            _methodName = "GetQuestionList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _surveyUpdatesService.GetQuestionList(x); });
        }

        /// <summary>
        /// Method to get Get Question Details By Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get/questionid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetQuestionDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetQuestionDetailsById([FromBody]string inputKey)
        {
            _methodName = "GetQuestionDetailsById";
            return Result(inputKey, _methodName, (long x) => { return _surveyUpdatesService.GetQuestionDetailsById(x); });
        }


        /// <summary>
        /// Method to get Get Question Survey Details By Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("question/survey")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetQuestionSurveyDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetQuestionSurveyDetailsById([FromBody]string inputKey)
        {
            _methodName = "GetQuestionSurveyDetailsById";
            return Result(inputKey, _methodName, (long x) => { return _surveyUpdatesService.GetQuestionSurveyDetailsById(x); });
        }

        /// <summary>
        /// Method to Update Question
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("question/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateQuestion", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateQuestion([FromBody]string inputKey)
        {
            _methodName = "UpdateQuestion";
            return Result(inputKey, _methodName, (QuestionDto x) => { return _surveyUpdatesService.UpdateQuestion(x); });
        }

        #endregion

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
            return Result(inputKey, _methodName, (BulletinInputDto x) => { return _surveyUpdatesService.GetBulletinList(x); });
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
            return Result(inputKey, _methodName, (BulletinInputDto x) => { return _surveyUpdatesService.GetBulletinDetailsById(x); });
        }

        /// <summary>
        /// Method to Save Bulletin
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("bulletin/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveBulletin", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveBulletin([FromBody]string inputKey)
        {
            _methodName = "SaveBulletin";
            return Result(inputKey, _methodName, (BulletinDto x) => { return _surveyUpdatesService.SaveBulletin(x); });
        }

        /// <summary>
        /// Method to Update Bulletin
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("bulletin/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateBulletin", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateBulletin([FromBody]string inputKey)
        {
            _methodName = "UpdateBulletin";
            return Result(inputKey, _methodName, (BulletinDto x) => { return _surveyUpdatesService.UpdateBulletin(x); });
        }


        /// <summary>
        /// Method to Update Bulletin
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("bulletin/media/delete")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DeleteBulletinMedia", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DeleteBulletinMedia([FromBody]string inputKey)
        {
            _methodName = "DeleteBulletinMedia";
            return Result(inputKey, _methodName, (BulletinInputDto x) => { return _surveyUpdatesService.DeleteBulletinMedia(x); });
        }
        #endregion

        #region Feedback
        [HttpPost]
        [Route("feedbackType/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetFeedbackTypeList([FromBody]string inputKey)
        {
            _methodName = "GetFeedbackTypeList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _surveyUpdatesService.GetFeedbackTypeList(x); });
        }

        /// <summary>
        /// Method to Get Feedback List
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("feedback/list")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetFeedbackList([FromBody]string inputKey)
        {
            _methodName = "GetFeedbackList";
            return Result(inputKey, _methodName, (FeedbackInputDto x) => { return _surveyUpdatesService.GetFeedbackList(x); });
        }

        #endregion
    }
}
