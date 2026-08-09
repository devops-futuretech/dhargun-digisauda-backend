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
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/support")]
    public class SupportController : BaseApiController
    {
        private const string ServiceName = "Support Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly ISupportService _supportService;
        private string _methodName;

        public SupportController(ISupportService supportService) : base(ServiceName)
        {
            _methodName = "Support Controller";
            try
            {
                _supportService = supportService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        [HttpPost]
        [Route("category/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCategoriesForSupport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCategoriesForSupport([FromBody]string inputKey)
        {
            _methodName = "GetCategoriesForSupport";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _supportService.GetCategoriesForSupport(x); });
        }

        [HttpPost]
        [Route("add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddSupportMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddSupportMobile([FromBody]string inputKey)
        {
            _methodName = "AddSupportMobile";
            return Result(inputKey, _methodName, (SupportAddInputDto x) => { return _supportService.AddSupportMobile(x); });
        }

        #region Support - Web

        [HttpPost]
        [Route("issue/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "IssueRegisterForWeb", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult IssueRegisterForWeb([FromBody]string inputKey)
        {
            _methodName = "IssueRegisterForWeb";
            return Result(inputKey, _methodName, (IssueRegisterDto x) => { return _supportService.IssueRegisterForWeb(x); });
        }

        [HttpPost]
        [Route("issue/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetIssueList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetIssueList([FromBody]string inputKey)
        {
            _methodName = "GetIssueList";
            return KendoGridResult(inputKey, _methodName, (SupportFilterInputDto x) => { return _supportService.GetIssueListWithPaging(x); });
        }

        [HttpPost]
        [Route("issue/listwithcomments")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetIssueListWithCmts", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetIssueListWithCmts([FromBody]string inputKey)
        {
            _methodName = "GetIssueListWithCmts";
            return KendoGridResult(inputKey, _methodName, (SupportFilterInputDto x) => { return _supportService.GetIssueListWithCmts(x); });
        }

        [HttpPost]
        [Route("issuedetails/supportid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetIssueDetailsBySupportId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetIssueDetailsBySupportId([FromBody]string inputKey)
        {
            _methodName = "GetIssueDetailsBySupportId";
            return KendoGridResult(inputKey, _methodName, (IssueDetailInputDto x) => { return _supportService.GetIssueDetailsBySupportId(x); });
        }

        [HttpPost]
        [Route("status/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateSupportIssueStatus", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateSupportIssueStatus([FromBody]string inputKey)
        {
            _methodName = "UpdateSupportIssueStatus";
            return KendoGridResult(inputKey, _methodName, (IssueStatusUpdateDto x) => { return _supportService.UpdateSupportIssueStatus(x); });
        }

        [HttpPost]
        [Route("issuedetails/comments")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetIssueCommentsList", Message = "The request has been declined for security reasons.", Seconds = 5)]
        public IHttpActionResult GetIssueCommentsList([FromBody] string inputKey)
        {
            _methodName = "GetIssueCommentsList";
            return Result(inputKey, _methodName, ((int n) => { return _supportService.GetIssueCommentsList(n); }));
        }


        [HttpPost]
        [Route("issue/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportSupportIssues", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportSupportIssues([FromBody]string inputKey)
        {
            _methodName = "ExportSupportIssues";
            return KendoGridResult(inputKey, _methodName, (SupportFilterInputDto x) => { return _supportService.ExportSupportIssues(x); });
        }

        [HttpPost]
        [Route("issue/featurelist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetFeatureList ", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetFeatureList()
        {
            _methodName = "GetFeatureList ";
            return Result(_methodName, () => { return _supportService.GetFeatureList(); });
        }

        [HttpPost]
        [Route("issue/queryfromlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetQueryFromList ", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetQueryFromList()
        {
            _methodName = "GetQueryFromList ";
            return Result(_methodName, () => { return _supportService.GetQueryFromList(); });
        }

        #endregion
    }
}
