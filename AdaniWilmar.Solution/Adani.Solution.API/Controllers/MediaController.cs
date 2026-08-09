using GMCore.Authenticate;
using Adani.Solution.DTO;
using Adani.Solution.Service;
using System;
using System.Web.Http;
using System.Web.Http.Description;
using GMCore.Helper;
using Adani.Solution.Service.Common;
using System.Net;
using Adani.Solution.DTO.Common;
using System.Collections.Generic;
using GMCore.Logger;
using System.Threading.Tasks;
using System.Net.Http;
using Adani.Solution.API.Models;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Configuration;
using System.Linq;
using Adani.Solution.API.App_Start;

namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/media")]
    public class MediaController : BaseApiController
    {
        private const string ServiceName = "Media Controller";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;
        private readonly IMediaService _mediaService;

        public MediaController(IMediaService mediaService)
        {
            _methodName = "Media Controller";
            try
            {
                _mediaService = mediaService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        [HttpPost]
        [Route("upload/competitor")]
        [Throttle(Name = "CompetitorImageUpload", Message = "The request has been declined for security reasons.", Seconds = 12)]
        public async Task<IHttpActionResult> CompetitorMediaUpload([FromUri]long key)
        {
            _methodName = "CompetitorMediaUpload";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            long recordId;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                if (key <= 0)
                {
                    result.IsSuccess = false;
                    result.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    result.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    recordId = key;
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
                var file = HttpContext.Current.Request.Files[0];
                var pageId = (int)DTO.Enums.PageType.Competitor;
                result =await _mediaService.UploadMedia(file, file.FileName, pageId, recordId);
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
        [Route("upload/prospectivedealer")]
        [Throttle(Name = "UserImageUpload", Message = "The request has been declined for security reasons.", Seconds = 12)]
        public async Task<IHttpActionResult> ProspectiveDealerMediaUpload([FromUri]long key)
        {
            _methodName = "ProspectiveDealerMediaUpload";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            long recordId;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                
                if (key <= 0)
                {
                    result.IsSuccess = false;
                    result.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    result.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    recordId = key;
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
                var file = HttpContext.Current.Request.Files[0];
                var pageId = (int)DTO.Enums.PageType.ProspectiveDealer;
                result = await _mediaService.UploadMedia(file, file.FileName, pageId, recordId);
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
        [Route("upload/dealer")]
        [Throttle(Name = "DealerMediaUpload", Message = "The request has been declined for security reasons.", Seconds = 12)]
        public async Task<IHttpActionResult> DealerMediaUpload([FromUri]long key)
        {
            _methodName = "DealerMediaUpload";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            long recordId;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
               
                if (key <= 0)
                {
                    result.IsSuccess = false;
                    result.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    result.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                    return Ok(contentDto);
                }
                try
                {
                    recordId = key;
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
                var file = HttpContext.Current.Request.Files[0];
                var pageId = (int)DTO.Enums.PageType.Dealer;
                result = await _mediaService.UploadMedia(file, file.FileName, pageId, recordId);
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
        [Route("upload/supportFiles")]
        [Throttle(Name = "SupportAttachmentsUpload", Message = "The request has been declined for security reasons.", Seconds = 12)]
        public async Task<IHttpActionResult> SupportAttachmentsUpload([FromUri]long key)
        {
            _methodName = "SupportAttachmentsUpload";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var file = HttpContext.Current.Request.Files[0];
                var pageId = (int)DTO.Enums.PageType.Support;
                result = await _mediaService.UploadMediaAndReturnFileName(file, file.FileName, pageId);
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
        [Route("upload/dynamicformattachment")]
        [Throttle(Name = "DynamicFormAttachment", Message = "The request has been declined for security reasons.", Seconds = 12)]
        public async Task<IHttpActionResult> DynamicFormAttachment()
        {
            _methodName = "DynamicFormAttachment";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var file = HttpContext.Current.Request.Files[0];
                var pageId = (int)DTO.Enums.PageType.DynamicFormAttachments;
                result = await _mediaService.UploadMediaAndReturnFileName(file, file.FileName, pageId);
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
        [Route("upload/audioFilesForCallToCustomers")]
        [Throttle(Name = "AudioFilesForCallToCustomers", Message = "The request has been declined for security reasons.", Seconds = 12)]
        public async Task<IHttpActionResult> AudioFilesForCallToCustomers([FromUri] long key)
        {
            _methodName = "AudioFilesForCallToCustomers";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var file = HttpContext.Current.Request.Files[0];
                var pageId = (int)DTO.Enums.PageType.AudioFiles;
                result = await _mediaService.UploadMediaAndReturnFileName(file, file.FileName, pageId);
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
        [Route("upload/imageForSaudaCallRecordMapping")]
        [Throttle(Name = "ImageForSaudaCallRecordMapping", Message = "The request has been declined for security reasons.", Seconds = 12)]
        public async Task<IHttpActionResult> ImageForSaudaCallRecordMapping([FromUri] long key)
        {
            _methodName = "ImageForSaudaCallRecordMapping";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var file = HttpContext.Current.Request.Files[0];
                var pageId = (int)DTO.Enums.PageType.ImagesSaudaMappingwithCallRecording;
                result = await _mediaService.UploadMediaAndReturnFileName(file, file.FileName, pageId);
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
    }
}