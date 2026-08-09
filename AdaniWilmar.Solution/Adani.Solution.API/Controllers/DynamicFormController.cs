using Adani.Solution.API.App_Start;
using Adani.Solution.DTO;
using Adani.Solution.Service;
using GMCore.Authenticate;
using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/dynamicform")]
    public class DynamicFormController : BaseApiController
    {
        private const string ServiceName = "Dynamic Form Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IDynamicFormService _dynamicformService;
        private string _methodName;

        public DynamicFormController(IDynamicFormService dynamicformService) : base(ServiceName)
        {
            _methodName = "Dynamic Form Controller";
            try
            {
                _dynamicformService = dynamicformService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        [HttpPost]
        [Route("form/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetFormsList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetFormsList([FromBody] string inputKey)
        {
            _methodName = "GetFormsList";
            return Result(inputKey, _methodName, (UserIdDto x) => { return _dynamicformService.GetFormsList(x); });
        }
        [HttpGet]
        [Route("form/View")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetFormsView", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetFormsView()
        {
            _methodName = "GetFormsView";
            return Result(_methodName, () => { return _dynamicformService.GetFormsView(); });
        }

        [HttpPost]
        [Route("form/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddFormAndQuestions", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddFormAndQuestions([FromBody] string inputKey)
        {
            _methodName = "AddFormAndQuestions";
            return Result(inputKey, _methodName, (FormAddDto x) => { return _dynamicformService.AddFormAndQuestions(x); });
        }

        [HttpPut]
        [Route("form/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateFormAndQuestions", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateFormAndQuestions([FromBody] string inputKey)
        {
            _methodName = "UpdateFormAndQuestions";
            return Result(inputKey, _methodName, (FormUpdateDto x) => { return _dynamicformService.UpdateFormAndQuestions(x); });
        }

        [HttpPost]
        [Route("form/view")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetFormQuestions", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetFormQuestions([FromBody] string inputKey)
        {
            _methodName = "GetFormQuestions";
            return Result(inputKey, _methodName, (FormIdDto x) => { return _dynamicformService.GetFormQuestions(x); });
        }

        [HttpGet]
        [Route("questiontypes")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetActiveQuestionTypes", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetActiveQuestionTypes()
        {
            _methodName = "GetActiveQuestionTypes";
            return Result(_methodName, () => { return _dynamicformService.GetActiveQuestionTypes(); });
        }

        [HttpPost]
        [Route("GetSubmittedDetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSubmittedDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSubmittedDetails([FromBody] string inputKey)
        {
            _methodName = "GetSubmittedDetails";
            return Result(inputKey, _methodName, (DynamicFormReportFilterInputDto x) => { return _dynamicformService.GetSubmittedDetailsList(x); });
        }
        [HttpPost]
        [Route("submittedForm/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportSupportIssues", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportSubmittedForm([FromBody] string inputKey)
        {
            _methodName = "ExportSubmittedForm";
            return KendoGridResult(inputKey, _methodName, (DynamicFormReportFilterInputDto x) => { return _dynamicformService.ExportSubmittedForm(x); });
        }


        [HttpPost]
        [Route("SubmittedFormDetailsbyId")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SubmittedFormDetailsbyId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SubmittedFormDetailsbyId([FromBody] string inputKey)
        {
            _methodName = "SubmittedFormDetailsbyId";
            return Result(inputKey, _methodName, (FormIdDto x) => { return _dynamicformService.SubmittedFormDetailsbyIdList(x); });
        }
     
        [HttpPost]
        [Route("sections/questions")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSectionQuestions", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSectionQuestions([FromBody] string inputKey)
        {
            _methodName = "GetSectionQuestions";
            return Result(inputKey, _methodName, (FormIdDto x) => { return _dynamicformService.GetSectionQuestions(x); });
        }

        
        [Route("sections/questionsList")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSectionQuestionsList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSectionQuestionsList([FromBody] string inputKey)
        {
            _methodName = "GetSectionQuestionsList";
            return Result(_methodName, () => { return _dynamicformService.GetSectionQuestionsList(); });
        }
        [Route("sections/questionsFormList")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSectionQuestionsFormList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSectionQuestionsFormList([FromBody] string inputKey)
        {
            _methodName = "GetSectionQuestionsFormList";
            return Result(_methodName, () => { return _dynamicformService.GetSectionQuestionsFormList(); });
        }

        [HttpPost]
        [Route("questions")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddQuestionAndAnswers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddQuestionAndAnswers([FromBody] string inputKey)
        {
            _methodName = "AddQuestionAndAnswers";
            return Result(inputKey, _methodName, (QuestionAddDto x) => { return _dynamicformService.AddQuestionAndAnswers(x); });
        }

        [HttpPut]
        [Route("questions/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateQuestionAndAnswers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateQuestionAndAnswers([FromBody] string inputKey)
        {
            _methodName = "UpdateQuestionAndAnswers";
            return Result(inputKey, _methodName, (QuestionAddDto x) => { return _dynamicformService.UpdateQuestionAndAnswers(x); });
        }

        //[HttpGet]
        //[Route("section/list")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetSectionsList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetSectionsList()
        //{
        //    _methodName = "GetSectionsList";
        //    return Result(_methodName, () => { return _dynamicformService.GetSectionsList(); });
        //}

        //[HttpPost]
        //[Route("section/details")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetSectionDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetSectionDetailsById([FromBody] string inputKey)
        //{
        //    _methodName = "GetSectionDetailsById";
        //    return Result(inputKey, _methodName, (SectionIdDto x) => { return _dynamicformService.GetSectionDetailsById(x); });
        //}

        [HttpPost]
        [Route("questions/view")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ViewQuestionDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ViewQuestionDetails([FromBody] string inputKey)
        {
            _methodName = "ViewQuestionDetails";
            return Result(inputKey, _methodName, (QuestionIdDto x) => { return _dynamicformService.ViewQuestionDetails(x); });
        }

        [HttpPost]
        [Route("submitform/view/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ViewSubmittedFormsListByDateRange", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ViewSubmittedFormsListByDateRange([FromBody] string inputKey)
        {
            _methodName = "ViewSubmittedFormsListByDateRange";
            return Result(inputKey, _methodName, (SubmittedFormsInputDto x) => { return _dynamicformService.ViewSubmittedFormsListByDateRange(x); });
        }

        [HttpPost]
        [Route("submitform/view")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ViewSubmittedFormDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ViewSubmittedFormDetails([FromBody] string inputKey)
        {
            _methodName = "ViewSubmittedFormDetails";
            return Result(inputKey, _methodName, (SubmittedFormIdDto x) => { return _dynamicformService.ViewSubmittedFormDetails(x); });
        }

        [HttpPost]
        [Route("submitform/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SubmitForm", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SubmitForm([FromBody] string inputKey)
        {
            _methodName = "SubmitForm";
            return Result(inputKey, _methodName, (FormInputDto x) => { return _dynamicformService.SubmitForm(x); });
        }

        //[HttpPost]
        //[Route("section/save")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "SaveSectionDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult SaveSectionDetails([FromBody] string inputKey)
        //{
        //    _methodName = "SaveSectionDetails";
        //    return Result(inputKey, _methodName, (SectionDto x) => { return _dynamicformService.SaveSectionDetails(x); });
        //}

        //[HttpPost]
        //[Route("section/update")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "UpdateSectionDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult UpdateSectionDetails([FromBody] string inputKey)
        //{
        //    _methodName = "UpdateSectionDetails";
        //    return Result(inputKey, _methodName, (SectionDto x) => { return _dynamicformService.UpdateSectionDetails(x); });
        //}

    }
}