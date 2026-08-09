using System;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net;
using System.Collections.Generic;
using GMCore.Logger;
using GMCore.Authenticate;
using GMCore.Helper;
using Adani.Solution.DTO;
using Adani.Solution.Service;
using Adani.Solution.Service.Common;
using Adani.Solution.DTO.Common;

namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [RoutePrefix("api/imports")]
    public class ImportController : BaseApiController
    {
        private const string ServiceName = "Import Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IImportService _importService;
        private string _methodName;

        public ImportController(IImportService importService) : base(ServiceName)
        {
            _methodName = "Import Controller";
            try
            {
                _importService = importService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        [HttpPost]
        [Route("address")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ImportAddresses", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ImportAddresses([FromBody]string inputKey)
        {
            _methodName = "ImportAddresses";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
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
                result = _importService.ImportAddresses(decryptedInput);
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
                return Content(HttpStatusCode.InternalServerError, Utility.DtoEncrypt(result.ErrorDto));
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
