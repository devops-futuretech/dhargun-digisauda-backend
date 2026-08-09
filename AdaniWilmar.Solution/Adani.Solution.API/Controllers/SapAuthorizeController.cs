using System;
using GMCore.Authenticate;
using GMCore.Logger;
using Adani.Solution.DTO;
using Adani.Solution.Service;
using Adani.Solution.Service.Common;
using System.Web.Http;
using System.Web.Http.Description;

namespace Adani.Solution.API.Controllers
{
    [CustomException]
    [RoutePrefix("api/sap/authorize")]
    public class SapAuthorizeController : BaseApiController
    {
        private const string ServiceName = "SapAuthorize Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly ISapAuthorizeService _sapauthorizeService;
        private string _methodName;

        public SapAuthorizeController(ISapAuthorizeService sapauthorizeService) : base(ServiceName)
        {
            _methodName = "Constructor";
            try
            {
                _sapauthorizeService = sapauthorizeService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
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
                result = _sapauthorizeService.AuthorizeUserSap(inputdto);
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