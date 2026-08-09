using GMCore.Authenticate;
using GMCore.Helper;
using GMCore.Logger;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service;
using Adani.Solution.Service.Common;
using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Adani.Solution.API.App_Start;

namespace Adani.Solution.API.Controllers
{
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/authenticate")]
    public class AuthenticateController : BaseApiController
    {
        private const string ServiceName = "Authenticate Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IAuthenticateService _authenticateService;
        private string _methodName;

        public AuthenticateController(IAuthenticateService authenticateService)
        {
            _methodName = "Constructor";
            try
            {
                _authenticateService = authenticateService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        [HttpPost]
        [Route("validate")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ValidateAppKey", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ValidateAppKey([FromBody]string inputKey)
        {
            _methodName = "ValidateAppKey";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            KeyInputDto inputDto;
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
                    inputDto = JsonHelper.ConvertJSonToObject<KeyInputDto>(decryptedInput);
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
                result = _authenticateService.ValidateAppKey(inputDto);
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
                contentDto.E6DYES1Q2 = Convert.ToString(result.SuccessDto.Response);
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
        [Route("verify")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "VerifyWebKey", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult VerifyWebKey([FromBody]string inputKey)
        {
            _methodName = "VerifyWebKey";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            KeyInputDto inputDto;
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
                    inputDto = JsonHelper.ConvertJSonToObject<KeyInputDto>(decryptedInput);
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
                result = _authenticateService.VerifyWebKey(inputDto);
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

        //[HttpPost]
        //[Route("verify/console")]
        //[ResponseType(typeof(ContentDto))]
        ////[Throttle(Name = "VerifyWebKey", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult VerifyConsoleWebKey([FromBody]string inputKey)
        //{
        //    _methodName = "VerifyWebKey";
        //    var result = new ResultDto();
        //    var errorDto = new ErrorDto();
        //    var successDto = new SuccessDto();
        //    var contentDto = new ContentDto();
        //    KeyInputDto inputDto;
        //    try
        //    {
        //        _logger.Info($"{ServiceName} Controller-Method {_methodName}");
        //        string decryptedInput;
        //        try
        //        {
        //            decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
        //        }
        //        catch (Exception exception)
        //        {
        //            var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
        //            _logger.Error(message);
        //            errorDto.ErrorCode = Constants.InvalidRequest;
        //            errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
        //            return Ok(contentDto);
        //        }
        //        try
        //        {
        //            inputDto = JsonHelper.ConvertJSonToObject<KeyInputDto>(decryptedInput);
        //        }
        //        catch (Exception exception)
        //        {
        //            var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
        //            _logger.Error(message);
        //            errorDto.ErrorCode = Constants.Exception;
        //            errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
        //            contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
        //            return Ok(contentDto);
        //        }
        //        result = _authenticateService.VerifyWebKey(inputDto);
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        result.IsSuccess = false;
        //        result.ErrorDto.ErrorCode = Constants.Exception;
        //        result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
        //    }
        //    if (result.ErrorDto.ErrorCode == Constants.Exception)
        //    {
        //        return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
        //    }
        //    if (result.IsSuccess)
        //    {
        //        successDto.Response = result.SuccessDto.Response;
        //        contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
        //        return Ok(contentDto);
        //    }
        //    else
        //    {
        //        errorDto.ErrorCode = result.ErrorDto.ErrorCode;
        //        errorDto.Message = result.ErrorDto.Message;
        //        contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
        //        return Ok(contentDto);
        //    }
        //}

        [HttpPost]
        [Route("validate1")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "ValidateAppKey", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ValidateAppKey1(KeyInputDto inputKey)
        {
            _methodName = "ValidateAppKey";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            //KeyInputDto inputDto;
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                //string decryptedInput;
                //try
                //{
                //    decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                //}
                //catch (Exception exception)
                //{
                //    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                //    _logger.Error(message);
                //    errorDto.ErrorCode = Constants.InvalidRequest;
                //    errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                //    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                //    return Ok(contentDto);
                //}
                //try
                //{
                //    inputDto = JsonHelper.ConvertJSonToObject<KeyInputDto>(decryptedInput);
                //}
                //catch (Exception exception)
                //{
                //    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                //    _logger.Error(message);
                //    errorDto.ErrorCode = Constants.Exception;
                //    errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                //    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                //    return Ok(contentDto);
                //}
                result = _authenticateService.ValidateAppKey(inputKey);
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
                contentDto.E6DYES1Q2 = Convert.ToString(result.SuccessDto.Response);
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
        [Route("verify/console")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "VerifyWebKey", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult VerifySap(KeyInputDto inputDto)
        {
            _methodName = "VerifySap";
            var result = new ResultDto();
            
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
               result = _authenticateService.VerifyWebKey(inputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
            }
              return Ok(result);
        }
    }
}
