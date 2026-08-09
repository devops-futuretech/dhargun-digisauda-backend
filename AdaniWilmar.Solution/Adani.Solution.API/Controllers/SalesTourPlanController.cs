using System;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net;
using GMCore.Logger;
using GMCore.Authenticate;
using GMCore.Helper;
using Adani.Solution.DTO;
using Adani.Solution.Service;
using Adani.Solution.Service.Common;
using Adani.Solution.DTO.Common;
using System.Collections.Generic;
using Adani.Solution.API.App_Start;

namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/SalesTourPlan")]
    public class SalesTourPlanController : BaseApiController
    {
        private const string ServiceName = "SalesTourPlan Controller";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly ISalesTourPlanService _salesTourPlanService;
        private string _methodName;

        public SalesTourPlanController(ISalesTourPlanService salesTourPlanService) : base(ServiceName)
        {
            _methodName = "SalesTourPlan Controller";
            try
            {
                _salesTourPlanService = salesTourPlanService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        #region Masters

        [HttpGet]
        [Route("dateweekdetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDateWeekDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDateWeekDetails()
        {
            _methodName = "GetDateWeekDetails";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                result = _salesTourPlanService.GetDateWeekDetails();
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpGet]
        [Route("cities")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "Getcities", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult Getcities()
        {
            _methodName = "Getcities";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            DistrictInputDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                result = _salesTourPlanService.GetCities();
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        #endregion

        #region FinancialYear

        [HttpGet]
        [Route("financialyear")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetFinancialYear", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetFinancialYear()
        {
            _methodName = "GetFinancialYear";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                result = _salesTourPlanService.GetFinancialYear();
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("financialyear/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddFinancialYear", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddFinancialYear([FromBody]string inputKey)
        {
            _methodName = "AddFinancialYear";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            FinancialYearAddDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<FinancialYearAddDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.AddFinancialYear(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPut]
        [Route("financialyear/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateFinancialYear", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateFinancialYear([FromBody]string inputKey)
        {
            _methodName = "UpdateFinancialYear";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            FinancialYearDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<FinancialYearDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.UpdateFinancialYear(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("financialyear/view")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ViewFinancialYear", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ViewFinancialYear([FromBody]string inputKey)
        {
            _methodName = "ViewFinancialYear";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            FinancialYearIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<FinancialYearIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.ViewFinancialYear(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }
        
        [HttpGet]
        [Route("financialyear/active")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetActiveFinancialYear", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetActiveFinancialYear()
        {
            _methodName = "GetActiveFinancialYear";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                result = _salesTourPlanService.GetActiveFinancialYear();
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        #endregion

        #region HeadQuarters

        [HttpGet]
        [Route("HeadQuarters")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "Getheadquarters", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult Getheadquarters()
        {
            _methodName = "Getheadquarters";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                result = _salesTourPlanService.GetHeadquarters();
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("headquarters/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "Addheadquarters", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult Addheadquarters([FromBody]string inputKey)
        {
            _methodName = "Addheadquarters";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            HeadquartersAddDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<HeadquartersAddDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.AddHeadquarters(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPut]
        [Route("headquarters/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "Updateheadquarters", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult Updateheadquarters([FromBody]string inputKey)
        {
            _methodName = "Updateheadquarters";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            HeadquartersUpdateDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<HeadquartersUpdateDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.UpdateHeadquarters(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("headquarters/view")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "Viewheadquarters", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult Viewheadquarters([FromBody]string inputKey)
        {
            _methodName = "Viewheadquarters";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            HeadquartersIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<HeadquartersIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.ViewHeadquarters(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpGet]
        [Route("headquarters/active")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetActiveheadquarters", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetActiveheadquarters()
        {
            _methodName = "GetActiveheadquarters";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                result = _salesTourPlanService.GetActiveHeadquarters();
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("headquarters/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportHeadQuarters", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportHeadQuarters([FromBody]string inputKey)
        {
            _methodName = "ExportHeadQuarters";
            return KendoGridResult(inputKey, _methodName, (LoginUserIdDto x) => { return _salesTourPlanService.ExportHeadQuarters(x); });
        }

        #endregion

        #region Reasons

        [HttpGet]
        [Route("reasons")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetReasons", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetReasons()
        {
            _methodName = "GetReasons";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                result = _salesTourPlanService.GetReasons();
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }
        [HttpPost]
        [Route("reasons/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddReason", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddReason([FromBody]string inputKey)
        {
            _methodName = "AddReason";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            ReasonAddDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<ReasonAddDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.AddReason(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPut]
        [Route("reasons/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateReasons", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateReasons([FromBody]string inputKey)
        {
            _methodName = "UpdateReasons";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            ReasonUpdateDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<ReasonUpdateDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.UpdateReason(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("reasons/view")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ViewReason", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ViewReason([FromBody]string inputKey)
        {
            _methodName = "ViewReason";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            ReasonIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<ReasonIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.ViewReasons(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpGet]
        [Route("reasons/active")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetActiveReasons", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetActiveReasons()
        {
            _methodName = "GetActiveReasons";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                result = _salesTourPlanService.GetActiveReasons();
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpGet]
        [Route("dealer/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDealers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDealers()
        {
            _methodName = "GetDealers";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                result = _salesTourPlanService.GetDealers();
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        #endregion

        #region Permanent Journey Plan

        [HttpPost]
        [Route("PJP/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddPermanentJourneyPlan", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddPermanentJourneyPlan([FromBody]string inputKey)
        {
            _methodName = "AddPermanentJourneyPlan";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            PermanentJouneyPlanAddDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<PermanentJouneyPlanAddDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.AddPermanentJourneyPlan(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPut]
        [Route("PJP/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdatePermanentJourneyPlan", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdatePermanentJourneyPlan([FromBody]string inputKey)
        {
            _methodName = "UpdatePermanentJourneyPlan";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            PermanentJourneyPlanUpdateDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<PermanentJourneyPlanUpdateDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.UpdatePermanentJourneyPlan(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("PJP/PermanentJourneyPlanList")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPermanentJourneyPlanList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPermanentJourneyPlanList([FromBody]string inputKey)
        {
            _methodName = "GetPermanentJourneyPlanList";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            LoginUserIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<LoginUserIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.GetPermanentJourneyPlanList(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("PJP/PermanentJourneyPlanDetail")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPermanentJourneyPlanDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPermanentJourneyPlanDetails([FromBody]string inputKey)
        {
            _methodName = "GetPermanentJourneyPlanDetails";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            PJPIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<PJPIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.GetPermanentJourneyPlanDetails(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("PJP/PendingPermanentJourneyPlanList")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingPermanentJourneyPlanList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingPermanentJourneyPlanList([FromBody]string inputKey)
        {
            _methodName = "GetPendingPermanentJourneyPlanList";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            LoginUserIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<LoginUserIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.GetPendingPermanentJourneyPlanList(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("PJP/Months")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPJPMonths", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPJPMonths([FromBody]string inputKey)
        {
            _methodName = "GetPJPMonths";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            FinancialYearIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<FinancialYearIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.GetPJPMonths(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("PJP/ApprovedPermanentJourneyPlanByUser")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ApprovedPermanentJourneyPlanByUser", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ApprovedPermanentJourneyPlanByUser([FromBody]string inputKey)
        {
            _methodName = "ApprovedPermanentJourneyPlanByUser";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            LoginUserIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<LoginUserIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.ApprovedPermanentJourneyPlanByUser(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }
        [HttpPost]
        [Route("PJP/MonthsByUserPermanentJourneyPlan")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "MonthsByUserPermanentJourneyPlan", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult MonthsByUserPermanentJourneyPlan([FromBody]string inputKey)
        {
            _methodName = "MonthsByUserPermanentJourneyPlan";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            PJPIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<PJPIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.MonthsByUserPermanentJourneyPlan(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }
        [HttpPost]
        [Route("PJP/DealersByUserPermanentJourneyPlan")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DealersByUserPermanentJourneyPlan", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DealersByUserPermanentJourneyPlan([FromBody]string inputKey)
        {
            _methodName = "DealersByUserPermanentJourneyPlan";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            PJPIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<PJPIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.DealersByUserPermanentJourneyPlan(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("PJP/NoVisitByUserPermanentJourneyPlan")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "NoVisitByUserPermanentJourneyPlan", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult NoVisitByUserPermanentJourneyPlan([FromBody]string inputKey)
        {
            _methodName = "NoVisitByUserPermanentJourneyPlan";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            PJPIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<PJPIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.NoVisitByUserPermanentJourneyPlan(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("PJP/GetApprovedOrRejectedPJPList")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetApprovedOrRejectedPJPList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetApprovedOrRejectedPJPList([FromBody]string inputKey)
        {
            _methodName = "GetApprovedOrRejectedPJPList";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            LoginUserIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<LoginUserIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.GetApprovedOrRejectedPJPList(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("PJP/GetApprovedPJPList")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetApprovedPJPList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetApprovedPJPList([FromBody]string inputKey)
        {
            _methodName = "GetApprovedOrRejectedPJPList";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            LoginUserIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<LoginUserIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.GetApprovedPJPList(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("PJP/GetRejectedPJPList")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetRejectedPJPList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetRejectedPJPList([FromBody]string inputKey)
        {
            _methodName = "GetApprovedOrRejectedPJPList";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            LoginUserIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<LoginUserIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.GetRejectedPJPList(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }
        #endregion

        #region Monthly Tour Plan

        [HttpPost]
        [Route("MTP/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddMonthlyTourPlan", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddMonthlyTourPlan([FromBody]string inputKey)
        {
            _methodName = "AddMonthlyTourPlan";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            MonthlyTourPlanAddDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<MonthlyTourPlanAddDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.AddMonthlyTourPlan(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPut]
        [Route("MTP/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateMonthlyTourPlan", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateMonthlyTourPlan([FromBody]string inputKey)
        {
            _methodName = "UpdateMonthlyTourPlan";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            MonthlyTourPlanUpdateDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<MonthlyTourPlanUpdateDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.UpdateMonthlyTourPlan(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("MTP/MonthlyTourPlanList")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetMonthlyTourPlanList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetMonthlyTourPlanList([FromBody]string inputKey)
        {
            _methodName = "GetMonthlyTourPlanList";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            LoginUserIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<LoginUserIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.GetMonthlyTourPlanList(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("MTP/MonthlyTourPlanDetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetMonthlyTourPlanDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetMonthlyTourPlanDetails([FromBody]string inputKey)
        {
            _methodName = "GetMonthlyTourPlanDetails";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            MTPIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<MTPIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.GetMonthlyTourPlanDetails(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("MTP/PendingMonthlyTourPlanList")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingMonthlyTourPlanList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingMonthlyTourPlanList([FromBody]string inputKey)
        {
            _methodName = "GetPendingMonthlyTourPlanList";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            LoginUserIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<LoginUserIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.GetPendingMonthlyTourPlanList(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }
        [HttpPost]
        [Route("MTP/MonthlyTourPlanDateCalendar")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "MonthlyTourPlanDateCalendar", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult MonthlyTourPlanDateCalendar([FromBody]string inputKey)
        {
            _methodName = "DealersByUserPermanentJourneyPlan";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            PermanentJourneyPlanDetailsDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<PermanentJourneyPlanDetailsDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.MonthlyTourPlanDateCalendar(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }
        [HttpPost]
        [Route("MTP/CityByUserPermanentJourneyPlan")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "CityByUserPermanentJourneyPlan", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult CityByUserPermanentJourneyPlan([FromBody]string inputKey)
        {
            _methodName = "CityByUserPermanentJourneyPlan";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            PermanentJourneyPlanDetailsDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<PermanentJourneyPlanDetailsDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.CityByUserPermanentJourneyPlan(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }
        [HttpPost]
        [Route("PJP/GetApprovedOrRejectedMTPList")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetApprovedOrRejectedMTPList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetApprovedOrRejectedMTPList([FromBody]string inputKey)
        {
            _methodName = "GetApprovedOrRejectedMTPList";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            LoginUserIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<LoginUserIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.GetApprovedOrRejectedMTPList(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }
        #endregion

        #region Monthly Plan deviation
        [HttpPost]
        [Route("MTPDeviation/ApprovedMonthlyTourPlanDetailsByUser")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ApprovedMonthlyTourPlanDetailsByUser", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ApprovedMonthlyTourPlanDetailsByUser([FromBody]string inputKey)
        {
            _methodName = "ApprovedMonthlyTourPlanDetailsByUser";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            MTPIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<MTPIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.ApprovedMonthlyTourPlanDetailsByUser(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("MTPDeviation/ApprovedMonthlyTourPlanByUser")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ApprovedMonthlyTourPlanByUser", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ApprovedMonthlyTourPlanByUser([FromBody]string inputKey)
        {
            _methodName = "ApprovedMonthlyTourPlanByUser";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            LoginUserIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<LoginUserIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.ApprovedMonthlyTourPlanByUser(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("MTPDeviation/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddMonthlyPlanDeviation", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddMonthlyPlanDeviation([FromBody]string inputKey)
        {
            _methodName = "AddMonthlyPlanDeviation";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            AddMonthlyPlanDeviationDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<AddMonthlyPlanDeviationDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.AddMonthlyPlanDeviation(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("MTPDeviation/PendingMonthlyPlanDeviation")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "PendingMonthlyPlanDeviation", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult PendingMonthlyPlanDeviation([FromBody]string inputKey)
        {
            _methodName = "PendingMonthlyPlanDeviation";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            LoginUserIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<LoginUserIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.PendingMonthlyPlanDeviation(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("MTPDeviation/ApprovedMonthlyPlanDeviation")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ApprovedMonthlyPlanDeviation", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ApprovedMonthlyPlanDeviation([FromBody]string inputKey)
        {
            _methodName = "ApprovedMonthlyTourPlanByUser";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            LoginUserIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<LoginUserIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.ApprovedMonthlyPlanDeviation(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("MTPDeviation/CheckApproveUserMonthlyPlanDeviation")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "CheckMonthlyPlanDeviationApproveByLoginedUser", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult CheckMonthlyPlanDeviationApproveByLoginedUser([FromBody]string inputKey)
        {
            _methodName = "CheckMonthlyPlanDeviationApproveByLoginedUser";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _salesTourPlanService.CheckMonthlyPlanDeviationApproveByLoginedUser(x); });
        }

        [HttpPut]
        [Route("MTPDeviation/UpdateMonthlyPlanDeviation")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateMonthlyPlanDeviation", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateMonthlyPlanDeviation([FromBody]string inputKey)
        {
            _methodName = "UpdateMonthlyPlanDeviation";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            MonthlyPlanDeviationUpdateDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<MonthlyPlanDeviationUpdateDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.UpdateMonthlyPlanDeviation(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }
        [HttpPost]
        [Route("MTPDeviation/RejectedMonthlyPlanDeviationForMobile")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "RejectedMonthlyPlanDeviationForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult RejectedMonthlyPlanDeviationForMobile([FromBody]string inputKey)
        {
            _methodName = "RejectedMonthlyPlanDeviationForMobile";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            LoginUserIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<LoginUserIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.RejectedMonthlyPlanDeviationForMobile(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }
        
        #endregion

        #region Today Activities
        [HttpPost]
        [Route("TodayActivities/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "TodayActivities", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult TodayActivities([FromBody]string inputKey)
        {
            _methodName = "TodayActivities";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            TodayActivitiesInputDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<TodayActivitiesInputDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.TodayActivities(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("TodayActivities/AddPendingSauda")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddPendingSauda", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddPendingSauda([FromBody]string inputKey)
        {
            _methodName = "AddPendingSauda";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            AddPendingSaudaRemarksDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<AddPendingSaudaRemarksDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.AddPendingSauda(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("TodayActivities/AddMarketScenario")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddMarketScenario", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddMarketScenario([FromBody]string inputKey)
        {
            _methodName = "AddMarketScenario";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            AddMarketScenarioDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<AddMarketScenarioDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.AddMarketScenario(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("TodayActivities/AddBDOCompetitorDetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddBDOCompetitorDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddBDOCompetitorDetails([FromBody]string inputKey)
        {
            _methodName = "AddBDOCompetitorDetails";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            BdoCompetitorAddListDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<BdoCompetitorAddListDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.AddBDOCompetitorDetails(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("TodayActivities/AddProspectiveDealer")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddProspectiveDealer", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddProspectiveDealer([FromBody]string inputKey)
        {
            _methodName = "AddProspectiveDealer";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            ProspectiveDealerAddListDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<ProspectiveDealerAddListDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.AddProspectiveDealer(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("TodayActivities/GetProspectiveDealerList")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetProspectiveDealerList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetProspectiveDealerList([FromBody]string inputKey)
        {
            _methodName = "GetProspectiveDealerList";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            SalesTourPlanParamDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<SalesTourPlanParamDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.GetProspectiveDealerList(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("TodayActivities/GetProspectiveDealerById")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetProspectiveDealerById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetProspectiveDealerById([FromBody]string inputKey)
        {
            _methodName = "GetProspectiveDealerById";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            IdInputDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<IdInputDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.GetProspectiveDealerById(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("TodayActivities/ViewMonthlyTourPlanDeviationDetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ViewMonthlyTourPlanDeviationDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ViewMonthlyTourPlanDeviationDetails([FromBody]string inputKey)
        {
            _methodName = "ViewMonthlyTourPlanDeviationDetails";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            IdInputDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<IdInputDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.ViewMonthlyTourPlanDeviationDetails(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("TodayActivities/dealer/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "TodayActivitiesDealerList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult TodayActivitiesDealerList([FromBody]string inputKey)
        {
            _methodName = "TodayActivitiesDealerList";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            TodayActivitiesInputDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<TodayActivitiesInputDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.TodayActivitiesDealerList(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("PendingSaudaRemarks/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingSaudaList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingSaudaList([FromBody]string inputKey)
        {
            _methodName = "GetPendingSaudaList";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            PendingSaudaInputDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<PendingSaudaInputDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.GetPendingSaudaList(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }


        #endregion

        #region User Sauda and Sales Target

        [HttpPost]
        [Route("SaudaSalesTarget/MonthsByFinancialYear")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetMonthsByFinancialYear", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetMonthsByFinancialYear([FromBody]string inputKey)
        {
            _methodName = "GetMonthsByFinancialYear";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            FinancialYearIdDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<FinancialYearIdDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                result = _salesTourPlanService.GetMonthsByFinancialYear(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("SaudaSalesTarget/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddUserSalesSaudaTarget", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddUserSalesSaudaTarget([FromBody]string inputKey)
        {
            _methodName = "AddUserSalesSaudaTarget";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            UserSalesSaudaTargetDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<UserSalesSaudaTargetDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                //result = _salesTourPlanService.AddUserSalesSaudaTarget(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("SaudaSalesTarget/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateUserSalesSaudaTarget", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateUserSalesSaudaTarget([FromBody]string inputKey)
        {
            _methodName = "AddUserSalesSaudaTarget";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            UserSalesSaudaTargetDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<UserSalesSaudaTargetDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                //result = _salesTourPlanService.UpdateUserSalesSaudaTarget(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("SaudaSalesTarget/view")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserSalesSaudaTarget", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserSalesSaudaTarget([FromBody]string inputKey)
        {
            _methodName = "GetUserSalesSaudaTarget";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            IdInputDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<IdInputDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                //result = _salesTourPlanService.GetUserSalesSaudaTarget(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpGet]
        [Route("SaudaSalesTarget/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserSalesSaudaTargetList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserSalesSaudaTargetList()
        {
            _methodName = "GetUserSalesSaudaTargetList";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            IdInputDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                //result = _salesTourPlanService.GetUserSalesSaudaTargetList();
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }

        [HttpPost]
        [Route("SaudaSalesTarget/listdetail")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserSalesSaudaTargetDetailList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserSalesSaudaTargetDetailList([FromBody]string inputKey)
        {
            _methodName = "GetUserSalesSaudaTargetDetailList";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            UserSalesSaudaTargetDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                string decryptedInput;
                try
                {
                    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.InvalidRequest;
                    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    inputDto = JsonHelper.ConvertJSonToObject<UserSalesSaudaTargetDto>(decryptedInput);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    errorDto.ErrorCode = Constants.Exception;
                    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                //result = _salesTourPlanService.GetUserSalesSaudaTargetDetailList(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
            if (result.ErrorDto.ErrorCode == Constants.Exception)
            {
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            }
            if (result.IsSuccess)
            {
                successDto.Response = result.SuccessDto.Response;
                contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
                return Ok(contentDto);
            }
            else
            {
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                return Ok(contentDto);
            }
        }
        #endregion

        [HttpPost]
        [Route("oiltypetarget/monthandyear")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetMonthAndYearByFinancialYear", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetMonthAndYearByFinancialYear([FromBody]string inputKey)
        {
            _methodName = "GetMonthAndYearByFinancialYear";
            return Result(inputKey, _methodName, (FinancialYearIdDto x) => { return _salesTourPlanService.GetMonthAndYearByFinancialYear(x); });
        }

        //ToDo: User Oiltype Target
        //#region User Oiltype Target

        //[Route("oiltypetarget/list")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetOilTypeTargetList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetOilTypeTargetList()
        //{
        //    _methodName = "GetOilTypeTargetDetailList";
        //    return Result(_methodName, () => { return _salesTourPlanService.GetOilTypeTargetList(); });
        //}

        //[HttpPost]
        //[Route("oiltypetarget/listdetail")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetOilTypeTargetDetailList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetOilTypeTargetDetailList([FromBody]string inputKey)
        //{
        //    _methodName = "GetOilTypeTargetDetailList";
        //    return Result(inputKey, _methodName, (UserOilTypeTargetIdDto x) => { return _salesTourPlanService.GetOilTypeTargetDetailList(x); });
        //}

        //[HttpPost]
        //[Route("oiltypetarget/add")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "AddUserOilTypeTarget", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult AddUserOilTypeTarget([FromBody]string inputKey)
        //{
        //    _methodName = "AddUserOilTypeTarget";
        //    return Result(inputKey, _methodName, (UserOilTypeTargetDto x) => { return _salesTourPlanService.AddUserOilTypeTarget(x); });
        //}

        //[HttpPost]
        //[Route("oiltypetarget/update")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "UpdateUserOilTypeTarget", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult UpdateUserOilTypeTarget([FromBody]string inputKey)
        //{
        //    _methodName = "UpdateUserOilTypeTarget";
        //    return Result(inputKey, _methodName, (UserOilTypeTargetDto x) => { return _salesTourPlanService.UpdateUserOilTypeTarget(x); });
        //}

        //[HttpPost]
        //[Route("oiltypetarget/details")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetUserOilTypeTargetDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetUserOilTypeTargetDetailsById([FromBody]string inputKey)
        //{
        //    _methodName = "GetUserSalesSaudaTarget";
        //    return Result(inputKey, _methodName, (UserOilTypeTargetIdDto x) => { return _salesTourPlanService.GetUserOilTypeTargetDetailsById(x); });
        //}

        //#endregion

        #region User Sales Target

        [HttpPost]
        [Route("usercustomersalestarget/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserCustomerSalesTargetList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserCustomerSalesTargetList([FromBody]string inputKey)
        {
            _methodName = "GetUserCustomerSalesTargetList";           
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _salesTourPlanService.GetUserCustomerSalesTargetList(x); });
        }

        [HttpPost]
        [Route("usercustomersalestarget/listdetail")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserSalesTargetDetailList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserSalesTargetDetailList([FromBody]string inputKey)
        {
            _methodName = "GetUserSalesTargetDetailList";
            return Result(inputKey, _methodName, (UserTargetIdDto x) => { return _salesTourPlanService.GetUserCustomerSalesTargetDetailList(x); });
        }

        [HttpPost]
        [Route("usercustomersalestarget/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddUserSalesTarget", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddUserSalesTarget([FromBody]string inputKey)
        {
            _methodName = "AddUserSalesTarget";
            return Result(inputKey, _methodName, (UserCustomerSalesTargetDto x) => { return _salesTourPlanService.AddUserCustomerSalesTarget(x); });
        }

        [HttpPost]
        [Route("usercustomersalestarget/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateUserSalesTarget", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateUserSalesTarget([FromBody]string inputKey)
        {
            _methodName = "UpdateUserSalesTarget";
            return Result(inputKey, _methodName, (UserCustomerSalesTargetDto x) => { return _salesTourPlanService.UpdateUserCustomerSalesTarget(x); });
        }

        [HttpPost]
        [Route("usercustomersalestarget/details")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserSalesTargetDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserSalesTargetDetailsById([FromBody]string inputKey)
        {
            _methodName = "GetUserSalesTargetDetailsById";
            return Result(inputKey, _methodName, (UserTargetIdDto x) => { return _salesTourPlanService.GetUserCustomerSalesTargetDetailsById(x); });
        }

        [HttpPost]
        [Route("usercustomersalestarget/addlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveUserSalesTargetList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveUserSalesTargetList([FromBody]string inputKey)
        {
            _methodName = "SaveUserSalesTargetList";
            return Result(inputKey, _methodName, (List<MapSalesTargetDetailDto> x) => { return _salesTourPlanService.SaveUserCustomerSalesTargetList(x); });
        }

        [HttpPost]
        [Route("usercustomersalestarget/assignedlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetAssignedSalesTargetList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetAssignedSalesTargetList([FromBody]string inputKey)
        {
            _methodName = "GetAssignedSalesTargetList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _salesTourPlanService.GetAssignedSalesTargetList(x); });
        }

        [HttpPost]
        [Route("usercustomersalestarget/oiltypelist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSalesTargetOilTypeList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSalesTargetOilTypeList([FromBody]string inputKey)
        {
            _methodName = "GetSalesTargetOilTypeList";
            return Result(inputKey, _methodName, (UserTargetIdDto x) => { return _salesTourPlanService.GetSalesTargetOilTypeList(x); });
        }

        [HttpPost]
        [Route("salestarget/oiltypelist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetOilTypesBasedOnAssignedSalesTarget", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetOilTypesBasedOnAssignedSalesTarget([FromBody]string inputKey)
        {
            _methodName = "GetOilTypesBasedOnAssignedSalesTarget";
            return Result(inputKey, _methodName, (UserTargetIdDto x) => { return _salesTourPlanService.GetOilTypesBasedOnAssignedSalesTarget(x); });
        }

        #endregion

        #region User CustomerSauda Target

        [HttpPost]
        [Route("usercustomersaudatarget/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserCustomerSaudaTargetList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserCustomerSaudaTargetList([FromBody]string inputKey)
        {
            _methodName = "GetUserCustomerSaudaTargetList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _salesTourPlanService.GetUserCustomerSaudaTargetList(x); });
        }

        [HttpPost]
        [Route("usercustomersaudatarget/listdetail")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserCustomerSaudaTargetDetailList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserCustomerSaudaTargetDetailList([FromBody]string inputKey)
        {
            _methodName = "GetUserCustomerSaudaTargetDetailList";
            return Result(inputKey, _methodName, (UserTargetIdDto x) => { return _salesTourPlanService.GetUserCustomerSaudaTargetDetailList(x); });
        }

        [HttpPost]
        [Route("usercustomersaudatarget/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddUserCustomerSaudaTarget", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddUserCustomerSaudaTarget([FromBody]string inputKey)
        {
            _methodName = "AddUserCustomerSaudaTarget";
            return Result(inputKey, _methodName, (UserCustomerSaudaTargetDto x) => { return _salesTourPlanService.AddUserCustomerSaudaTarget(x); });
        }

        [HttpPost]
        [Route("usercustomersaudatarget/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateUserCustomerSaudaTarget", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateUserCustomerSaudaTarget([FromBody]string inputKey)
        {
            _methodName = "UpdateUserCustomerSaudaTarget";
            return Result(inputKey, _methodName, (UserCustomerSaudaTargetDto x) => { return _salesTourPlanService.UpdateUserCustomerSaudaTarget(x); });
        }

        [HttpPost]
        [Route("usercustomersaudatarget/details")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserCustomerSaudaTargetDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserCustomerSaudaTargetDetailsById([FromBody]string inputKey)
        {
            _methodName = "GetUserCustomerSaudaTargetDetailsById";
            return Result(inputKey, _methodName, (UserTargetIdDto x) => { return _salesTourPlanService.GetUserCustomerSaudaTargetDetailsById(x); });
        }

        [HttpPost]
        [Route("usercustomersaudatarget/addlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveUserCustomerSaudaTargetList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveUserCustomerSaudaTargetList([FromBody]string inputKey)
        {
            _methodName = "SaveUserCustomerSaudaTargetList";
            return Result(inputKey, _methodName, (List<MapSaudaTargetDetailDto> x) => { return _salesTourPlanService.SaveUserCustomerSaudaTargetList(x); });
        }

        [HttpPost]
        [Route("usercustomersaudatarget/assignedlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetAssignedSaudaTargetList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetAssignedSaudaTargetList([FromBody]string inputKey)
        {
            _methodName = "GetAssignedSaudaTargetList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _salesTourPlanService.GetAssignedSaudaTargetList(x); });
        }

        [HttpPost]
        [Route("usercustomersaudatarget/oiltypelist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSaudaTargetOilTypeList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSaudaTargetOilTypeList([FromBody]string inputKey)
        {
            _methodName = "GetSaudaTargetOilTypeList";
            return Result(inputKey, _methodName, (UserTargetIdDto x) => { return _salesTourPlanService.GetSaudaTargetOilTypeList(x); });
        }

        [HttpPost]
        [Route("saudatarget/oiltypelist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetOilTypesBasedOnAssignedSaudaTarget", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetOilTypesBasedOnAssignedSaudaTarget([FromBody]string inputKey)
        {
            _methodName = "GetOilTypesBasedOnAssignedSaudaTarget";
            return Result(inputKey, _methodName, (UserTargetIdDto x) => { return _salesTourPlanService.GetOilTypesBasedOnAssignedSaudaTarget(x); });
        }
        #endregion

        #region MTP Current Month

        [HttpPost]
        [Route("MTP/currentmonth")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetMTPDetailsForCurrentMonth", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetMTPDetailsForCurrentMonth([FromBody]string inputKey)
        {
            _methodName = "GetMTPDetailsForCurrentMonth";
            return Result(inputKey,_methodName, (TodayActivitiesInputDto x) => { return _salesTourPlanService.GetMTPDetailsForCurrentMonth(x); });
        }

        #endregion

        #region Today Activity

        [HttpPost]
        [Route("todayactivity/prospectivedealers")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetProspectiveDealers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetProspectiveDealers([FromBody]string inputKey)
        {
            _methodName = "GetProspectiveDealers";
            return Result(inputKey, _methodName, (SalesTourPlanParamDto x) => { return _salesTourPlanService.GetProspectiveDealers(x); });
        }

        [HttpPost]
        [Route("todayactivity/pendingsauda")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPendingSaudaRemarksList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPendingSaudaRemarksList([FromBody]string inputKey)
        {
            _methodName = "GetPendingSaudaRemarksList";
            return Result(inputKey, _methodName, (SalesTourPlanParamDto x) => { return _salesTourPlanService.GetPendingSaudaRemarksList(x); });
        }

        [HttpPost]
        [Route("todayactivity/salesdiscussion")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetMarketScenariosList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetMarketScenariosList([FromBody]string inputKey)
        {
            _methodName = "GetMarketScenariosList";
            return Result(inputKey, _methodName, (SalesTourPlanParamDto x) => { return _salesTourPlanService.GetMarketScenariosList(x); });
        }

        [HttpPost]
        [Route("todayactivity/competitors")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCompetitorsList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCompetitorsList([FromBody]string inputKey)
        {
            _methodName = "GetCompetitorsList";
            return Result(inputKey, _methodName, (SalesTourPlanParamDto x) => { return _salesTourPlanService.GetCompetitorsList(x); });
        }

        [HttpPost]
        [Route("todayactivity/competitorssku")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCompetitorSkuList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCompetitorSkuList([FromBody]string inputKey)
        {
            _methodName = "GetCompetitorSkuList";
            return Result(inputKey, _methodName, (SalesTourPlanParamDto x) => { return _salesTourPlanService.GetCompetitorSkuList(x); });
        }
        [HttpPost]
        [Route("todayactivity/SecondarySalesWholeseller")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSecondarySalesFortheDayByWholeseller", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSecondarySalesFortheDayByWholeseller([FromBody]string inputKey)
        {
            _methodName = "GetSecondarySalesFortheDayByWholeseller";
            return Result(inputKey, _methodName, (SecondarySalesInputDto x) => { return _salesTourPlanService.GetSecondarySalesFortheDayByWholeseller(x); });
        }
        [HttpPost]
        [Route("todayactivity/wholeseller/SalesDetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSecondarySalesDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSecondarySalesDetails([FromBody]string inputKey)
        {
            _methodName = "GetSecondarySalesDetails";
            return Result(inputKey, _methodName, (WholesellerSecondarySalesInputDto x) => { return _salesTourPlanService.GetSecondarySalesDetails(x); });
        }
        [HttpPost]
        [Route("todayactivity/WholeSellerCompetitors")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetWholeSellerCompetitorsList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetWholeSellerCompetitorsList([FromBody]string inputKey)
        {
            _methodName = "GetWholeSellerCompetitorsList";
            return Result(inputKey, _methodName, (SalesTourPlanParamDto x) => { return _salesTourPlanService.GetWholeSellerCompetitorsList(x); });
        }
        [HttpPost]
        [Route("todayactivity/WholesellerForWeb")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSecondarySalesFortheDayByWholesellerForWeb", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSecondarySalesFortheDayByWholesellerForWeb([FromBody]string inputKey)
        {
            _methodName = "GetSecondarySalesFortheDayByWholesellerForWeb";
            return Result(inputKey, _methodName, (SecondarySalesInputDto x) => { return _salesTourPlanService.GetSecondarySalesFortheDayByWholesellerForWeb(x); });
        }
        [HttpPost]
        [Route("FileAttachments")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetFileAttachments", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetFileAttachments([FromBody]string inputKey)
        {
            _methodName = "GetFileAttachments";
            return Result(inputKey, _methodName, (AttachmentInputDto x) => { return _salesTourPlanService.GetFileAttachments(x); });
        }
        #endregion

        [HttpPost]
        [Route("user/Attendence")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetMonthAndYearByFinancialYear", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserAttendence([FromBody]string inputKey)
        {
            _methodName = "GetMonthAndYearByFinancialYear";
            return Result(inputKey, _methodName, (UserAttendenceInputDto x) => { return _salesTourPlanService.GetUserAttendence(x); });
        }

        #region User Customer Target

        [HttpPost]
        [Route("usercustomertarget/addlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveUserCustomerTargetList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveUserCustomerTargetList([FromBody]string inputKey)
        {
            _methodName = "SaveUserCustomerTargetList";
            return Result(inputKey, _methodName, (List<MapSalesTargetDetailDto> x) => { return _salesTourPlanService.SaveUserCustomerTargetList(x); });
        }
        [HttpPost]
        [Route("usercustomertarget/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserCustomerSalesTargetList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserCustomerTargetList([FromBody]string inputKey)
        {
            _methodName = "GetUserCustomerSalesTargetList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _salesTourPlanService.GetUserCustomerTargetList(x); });
        }
        [HttpPost]
        [Route("usercustomertarget/listdetail")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserSalesTargetDetailList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserTargetDetailList([FromBody]string inputKey)
        {
            _methodName = "GetUserSalesTargetDetailList";
            return Result(inputKey, _methodName, (UserTargetIdDto x) => { return _salesTourPlanService.GetUserCustomerTargetDetailList(x); });
        }
        [HttpPost]
        [Route("usercustomertarget/assignedlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetAssignedSalesTargetList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetAssignedTargetList([FromBody]string inputKey)
        {
            _methodName = "GetAssignedSalesTargetList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _salesTourPlanService.GetAssignedTargetList(x); });
        }

        [HttpPost]
        [Route("usercustomertarget/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateUserCustomerTarget", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateUserCustomerTarget([FromBody]string inputKey)
        {
            _methodName = "UpdateUserCustomerTarget";
            return Result(inputKey, _methodName, (List<MapSalesTargetDetailDto> x) => { return _salesTourPlanService.UpdateUserCustomerTarget(x); });
        }

        [HttpPost]
        [Route("usercustomertarget/details")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetUserCustomerTargetDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetUserCustomerTargetDetailsById([FromBody]string inputKey)
        {
            _methodName = "GetUserCustomerTargetDetailsById";
            return Result(inputKey, _methodName, (UserTargetIdDto x) => { return _salesTourPlanService.GetUserCustomerTargetDetailsById(x); });
        }


        #endregion

        #region STP History

        [HttpPost]
        [Route("pcp/history")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSalesTourPlanPcpHistory", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSalesTourPlanPcpHistory([FromBody]string inputKey)
        {
            _methodName = "GetSalesTourPlanPcpHistory";
            return Result(inputKey, _methodName, (long x) => { return _salesTourPlanService.GetSalesTourPlanPcpHistory(x); });
        }

        [HttpPost]
        [Route("mtp/history")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSalesTourPlanMtpHistory", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSalesTourPlanMtpHistory([FromBody]string inputKey)
        {
            _methodName = "GetSalesTourPlanMtpHistory";
            return Result(inputKey, _methodName, (long x) => { return _salesTourPlanService.GetSalesTourPlanMtpHistory(x); });
        }

        #endregion
    }
}