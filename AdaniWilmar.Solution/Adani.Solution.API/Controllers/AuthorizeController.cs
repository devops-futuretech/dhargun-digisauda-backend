using GMCore.Authenticate;
using GMCore.Helper;
using GMCore.Logger;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service;
using Adani.Solution.Service.Common;
using System;
using System.Configuration;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using Adani.Solution.API.App_Start;

namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/authorize")]
    public class AuthorizeController : BaseApiController
    {
        private const string ServiceName = "Authorize Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IAuthorizeService _authorizeService;
        private string _methodName;

        public AuthorizeController(IAuthorizeService authorizeService) : base(ServiceName)
        {
            _methodName = "Constructor";
            try
            {
                _authorizeService = authorizeService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        //[HttpPost]
        //[Route("user")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "AuthorizeUser", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult AuthorizeUser([FromBody]string inputKey)
        //{
        //    _methodName = "AuthorizeUser";
        //    return Result(inputKey, _methodName, (AuthorizeInputDto x) => { return _authorizeService.AuthorizeUser(x); });
        //}

        [HttpPost]
        [Route("user")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "AuthorizeUser", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AuthorizeUser([FromBody]string inputKey)
        {
            _methodName = "AuthorizeUser";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            AuthorizeInputDto inputDto;
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
                    inputDto = JsonHelper.ConvertJSonToObject<AuthorizeInputDto>(decryptedInput);
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
                result = _authorizeService.AuthorizeUser(inputDto);
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
                var outputDto = (AuthorizeOutputDto)result.SuccessDto.Response;
                contentDto.E6DYES1Q2 = outputDto.LoginToken;
                outputDto.LoginToken = string.Empty;
                outputDto.TEG_AuthAPIUrl = ConfigurationManager.AppSettings["TEG_AuthAPIUrl"];
                outputDto.TEG_clientId = ConfigurationManager.AppSettings["TEG_clientId"];
                outputDto.TEG_clientSecret = ConfigurationManager.AppSettings["TEG_clientSecret"];
                successDto.Response = outputDto;
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
        //[Route("user/console")]
        //[ResponseType(typeof(ContentDto))]
        ////[Throttle(Name = "AuthorizeUser", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult AuthorizeConsoleUser([FromBody]string inputKey)
        //{
        //    //_methodName = "AuthorizeConsoleUser";
        //    //return Result(inputKey, _methodName, (AuthorizeInputDto x) => { return _authorizeService.AuthorizeUser(x); });

        //    _methodName = "AuthorizeConsoleUser";
        //    var result = new ResultDto();
        //    var errorDto = new ErrorDto();
        //    var successDto = new SuccessDto();
        //    var contentDto = new ContentDto();
        //    AuthorizeInputDto inputDto;
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
        //            inputDto = JsonHelper.ConvertJSonToObject<AuthorizeInputDto>(decryptedInput);
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
        //        result = _authorizeService.AuthorizeUser(inputDto);
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
        //        var outputDto = (AuthorizeOutputDto)result.SuccessDto.Response;
        //        contentDto.E6DYES1Q2 = outputDto.LoginToken;
        //        outputDto.LoginToken = string.Empty;
        //        successDto.Response = outputDto;
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
        [Route("user/forgotpassword/otp")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ForgotPasswordOtpSend", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ForgotPasswordOtpSend([FromBody]string inputKey)
        {
            _methodName = "ForgotPasswordOtpSend";
            return Result(inputKey, _methodName, (ForgotPasswordDto x) => { return _authorizeService.ForgotPasswordOtpSend(x); });
        }

        [HttpPost]
        [Route("user/resetpassword")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ResetPassword", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ResetPassword([FromBody]string inputKey)
        {
            _methodName = "ResetPassword";
            return Result(inputKey, _methodName, (ResetPasswordDto x) => { return _authorizeService.ResetPassword(x); });
        }

        [HttpPost]
        [Route("user/otp/resend")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "OtpReSend", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult OtpReSend([FromBody]string inputKey)
        {
            _methodName = "OtpReSend";
            return Result(inputKey, _methodName, (UserIdDto x) => { return _authorizeService.OtpReSend(x); });
        }

        [HttpPost]
        [Route("vertical/user")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetVerticalListBasedonUsername", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetVerticalListBasedonUsername([FromBody]string inputKey)
        {
            _methodName = "GetVerticalListBasedonUsername";
            return Result(inputKey, _methodName, (AuthorizeInputDto x) => { return _authorizeService.GetVerticalListBasedonUsername(x); });
        }

        [HttpPost]
        [Route("logout")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateLogOut", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateLogOut([FromBody]string inputKey)
        {
            _methodName = "UpdateLogOut";
            return Result(inputKey, _methodName, (UserIdDto x) => { return _authorizeService.UpdateLogOut(x); });
        }
        #region  counter Bid

        //[HttpPost]
        //[Route("counterbid/view")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetSaudaCounterBidDetails", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult GetSaudaCounterBidDetails([FromBody]string inputKey)
        //{
        //    _methodName = "GetSaudaCounterBidDetails";
        //    return Result(inputKey, _methodName, (SaudaDetailInputDto x) => { return _authorizeService.GetSaudaCounterBidDetails(x); });
        //}

        //[HttpPost]
        //[Route("counterbid/approve")]
        //[ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "ApproveCounterBid", Message = "The request has been declined for security reasons.", Seconds = 1)]
        //public IHttpActionResult ApproveCounterBid([FromBody]string inputKey)
        //{
        //    _methodName = "ApproveCounterBid";
        //    return Result(inputKey, _methodName, (CounterBidInputDto x) => { return _authorizeService.ApproveCounterBid(x); });
        //}

        #endregion


        #region Load Test

        [HttpPost]
        [Route("user/loadtest")]
        [ResponseType(typeof(ContentDto))]
        ////[Throttle(Name = "AuthorizeUserLoadTest", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AuthorizeUserLoadTest([FromBody]AuthorizeInputDto inputKey)
        {
            _methodName = "AuthorizeUserLoadTest";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            // AuthorizeInputDto inputDto;
            try
            {

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
                //    inputDto = JsonHelper.ConvertJSonToObject<AuthorizeInputDto>(decryptedInput);
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
                result = _authorizeService.AuthorizeUser(inputKey);
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
                var outputDto = (AuthorizeOutputDto)result.SuccessDto.Response;
                contentDto.E6DYES1Q2 = outputDto.LoginToken;
                outputDto.LoginToken = string.Empty;
                successDto.Response = outputDto;
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
        [Route("user1")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "AuthorizeUser", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AuthorizeUser1(AuthorizeInputDto inputKey)
        {
            _methodName = "AuthorizeUser";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            //AuthorizeInputDto inputDto;
            try
            {
                //_logger.Info($"{ServiceName} Controller-Method {_methodName}");
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
                //    inputDto = JsonHelper.ConvertJSonToObject<AuthorizeInputDto>(decryptedInput);
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
                result = _authorizeService.AuthorizeUser(inputKey);
                errorDto.ErrorCode = result.ErrorDto.ErrorCode;
                errorDto.Message = result.ErrorDto.Message;
                contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
               
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
            //if (result.ErrorDto.ErrorCode == Constants.Exception)
            //{
            //    return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(errorDto));
            //}
            //if (result.IsSuccess)
            //{
            //    var outputDto = (AuthorizeOutputDto)result.SuccessDto.Response;
            //    contentDto.E6DYES1Q2 = outputDto.LoginToken;
            //    outputDto.LoginToken = string.Empty;
            //    successDto.Response = outputDto;
            //    contentDto.Y77T3XP2B = Utility.DtoEncrypt(successDto);
            //    return Ok(contentDto);
            //}
            //else
            //{
            //    errorDto.ErrorCode = result.ErrorDto.ErrorCode;
            //    errorDto.Message = result.ErrorDto.Message;
            //    contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
            //    return Ok(contentDto);
            //}
        }

        [HttpPost]
        [Route("user/console")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "AuthorizeUser", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AuthorizeSapUser(AuthorizeInputDto inputdto)
        {
            //_methodName = "AuthorizeConsoleUser";
            //return Result(inputKey, _methodName, (AuthorizeInputDto x) => { return _authorizeService.AuthorizeUser(x); });
            _methodName = "AuthorizeSapUser";
            var result = new ResultDto();
            try
            {
               inputdto.IsRequestFromWeb = true;
               result = _authorizeService.AuthorizeUserSap(inputdto);
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

        [HttpPost]
        [Route("user/sap/new")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "AuthorizeUser", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AuthorizeSapUserNew(AuthorizeInputDto inputdto)
        {
            //_methodName = "AuthorizeConsoleUser";
            //return Result(inputKey, _methodName, (AuthorizeInputDto x) => { return _authorizeService.AuthorizeUser(x); });
            _methodName = "AuthorizeSapUserNew";
            var result = new ResultDto();
            try
            {
                inputdto.IsRequestFromWeb = true;
                result = _authorizeService.AuthorizeUserSapNew(inputdto);
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
