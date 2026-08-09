using GMCore.Logger;
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
using Adani.Solution.API.App_Start;

namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/roles")]
    public class RoleController : BaseApiController
    {
        private const string ServiceName = "Role Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IAdminService _adminService;
        private readonly IRoleService _roleService;
        private string _methodName;

        public RoleController(IAdminService adminService, IRoleService roleService)
          : base(ServiceName)
        {
            _methodName = "Role Controller";
            try
            {
                _adminService = adminService;
                _roleService = roleService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }

        #region Roles & Claims

        [HttpGet]
        [Route("claims")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetClaims", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetClaims()
        {
            _methodName = "GetClaims";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                result = _adminService.GetClaims();
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
        [Route("roletypes")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetRoleTypes", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetRoleTypes()
        {
            _methodName = "GetRoleTypes";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                result = _adminService.GetRoleTypes();
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
        [Route("")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetRoles", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetRoles()
        {
            _methodName = "GetRoles";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                result = _adminService.GetRoles();
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
        [Route("roletypeclaims")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetAllRoleTypeClaims", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetAllRoleTypeClaims()
        {
            _methodName = "GetAllRoleTypeClaims";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                result = _adminService.GetAllRoleTypeClaims();
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
        [Route("roletypeclaims")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetRoleTypeClaims", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetRoleTypeClaims([FromBody]string inputKey)
        {
            _methodName = "GetRoleTypeClaims";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            RoleTypeUsersDto inputDto;
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
                    inputDto = JsonHelper.ConvertJSonToObject<RoleTypeUsersDto>(decryptedInput);
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
                result = _adminService.GetRoleTypeClaims(inputDto);
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
        [Route("roletypeclaims/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddRoleTypeClaim", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddRoleTypeClaim([FromBody]string inputKey)
        {
            _methodName = "AddRoleTypeClaim";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            RoleTypeClaimDto inputDto;
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
                    inputDto = JsonHelper.ConvertJSonToObject<RoleTypeClaimDto>(decryptedInput);
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
                result = _adminService.AddRoleTypeClaim(inputDto);
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
        [Route("roletypeclaims/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateRoleTypeClaims", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateRoleTypeClaims([FromBody]string inputKey)
        {
            _methodName = "UpdateRoleTypeClaims";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            RoleTypeClaimUpdateDto inputDto;
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
                    inputDto = JsonHelper.ConvertJSonToObject<RoleTypeClaimUpdateDto>(decryptedInput);
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
                result = _adminService.UpdateRoleTypeClaims(inputDto);
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
        [Route("roleclaims")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetAllRoleClaims", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetAllRoleClaims()
        {
            _methodName = "GetAllRoleClaims";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                result = _adminService.GetAllRoleClaims();
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
        [Route("roleclaims/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddRoleClaim", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddRoleClaim([FromBody]string inputKey)
        {
            _methodName = "AddRoleClaim";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            RoleClaimDto inputDto;
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
                    inputDto = JsonHelper.ConvertJSonToObject<RoleClaimDto>(decryptedInput);
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

                result = _adminService.AddRoleClaim(inputDto);
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
        [Route("roleclaims/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateRoleClaims", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateRoleClaims([FromBody]string inputKey)
        {
            _methodName = "UpdateRoleClaims";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            RoleClaimUpdateDto inputDto;
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
                    inputDto = JsonHelper.ConvertJSonToObject<RoleClaimUpdateDto>(decryptedInput);
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
                result = _adminService.UpdateRoleClaims(inputDto);
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
        [Route("roletype/hierarchy")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateRoleTypeHierarchy", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateRoleTypeHierarchy([FromBody]string inputKey)
        {
            _methodName = "UpdateRoleTypeHierarchy";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            RoleTypeHierarchyDto inputDto;
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
                    inputDto = JsonHelper.ConvertJSonToObject<RoleTypeHierarchyDto>(decryptedInput);
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

                result = _adminService.UpdateRoleTypeHierarchy(inputDto);
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
        [Route("orghierarchy")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetOrganizationHierarchy", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetOrganizationHierarchy()
        {
            _methodName = "GetOrganizationHierarchy";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                result = _adminService.GetOrganizationHierarchy();
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
        [Route("roletypeclaims/delete")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DeleteRoleTypeAndClaims", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DeleteRoleTypeAndClaims([FromBody]string inputKey)
        {
            _methodName = "DeleteRoleTypeAndClaims";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            RoleTypeIdDto inputDto;
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
                    inputDto = JsonHelper.ConvertJSonToObject<RoleTypeIdDto>(decryptedInput);
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
                result = _adminService.DeleteRoleTypeAndClaims(inputDto);
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
        [Route("roleclaims/delete")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DeleteRoleAndClaims", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DeleteRoleAndClaims([FromBody]string inputKey)
        {
            _methodName = "DeleteRoleAndClaims";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            RoleIdDto inputDto;
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
                    inputDto = JsonHelper.ConvertJSonToObject<RoleIdDto>(decryptedInput);
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
                result = _adminService.DeleteRoleAndClaims(inputDto);
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
        [Route("reportingtoroles")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetReportingToRoles", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetReportingToRoles([FromBody]string inputKey)
        {
            _methodName = "GetReportingToRoles";
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
                result = _adminService.GetReportingToRoles(inputDto);
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

        #region Hierarchy

        /// <summary>
        /// Method to Get FreightZone List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("hierarchy/processid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetRoleHierarchyByProcess", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetRoleHierarchyByProcess([FromBody]string inputKey)
        {
            _methodName = "GetRoleHierarchyByProcess";
            return Result(inputKey, _methodName, (RoleHierarchyParamDto x) => { return _adminService.GetRoleHierarchyByProcess(x); });
        }

        [HttpPost]
        [Route("hierarchy/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddOrUpdateRoleHierarchy", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddOrUpdateRoleHierarchy([FromBody]string inputKey)
        {
            _methodName = "AddOrUpdateRoleHierarchy";
            return Result(inputKey, _methodName, (RoleHierarchyDto x) => { return _adminService.AddOrUpdateRoleHierarchy(x); });
        }

        #endregion

        #region Reporting To Users

        [HttpPost]
        [Route("reportingtouser/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetReportingToUsersByRole", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetReportingToUsersByRole([FromBody]string inputKey)
        {
            _methodName = "GetReportingToUsersByRole";
            return Result(inputKey, _methodName, (ReportingUsersInputDto x) => { return _adminService.GetReportingToUsersByRole(x); });
        }

        [HttpPost]
        [Route("reportingtousers/userid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetReportingToUsersByUserId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetReportingToUsersByUserId([FromBody]string inputKey)
        {
            _methodName = "GetReportingToUsersByUserId";
            return Result(inputKey, _methodName, (ReportingUsersInputDto x) => { return _adminService.GetOrganizationReportingToUsersByUserId(x); });
        }

        [HttpPost]
        [Route("salesreportingtousers/userid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSalesReportingToUsersByUserId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSalesReportingToUsersByUserId([FromBody]string inputKey)
        {
            _methodName = "GetSalesReportingToUsersByUserId";
            return Result(inputKey, _methodName, (ReportingUsersInputDto x) => { return _adminService.GetSalesReportingToUsersByUserId(x); });
        }

        [HttpPost]
        [Route("salesreportingtousers/bycityid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSalesReportingToUsersByCityId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSalesReportingToUsersByCityId([FromBody]string inputKey)
        {
            _methodName = "GetSalesReportingToUsersByCityId";
            return Result(inputKey, _methodName, (ReportingUsersInputDto x) => { return _adminService.GetSalesReportingToUsersByCityId(x); });
        }

        [HttpPost]
        [Route("salesreportingtousers/bycitydistrictstate")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSalesReportingToUsersByCityId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSalesReportingToUsersByCityStateDistrict([FromBody] string inputKey)
        {
            _methodName = "GetSalesReportingToUsersByCityStateDistrict";
            return Result(inputKey, _methodName, (ReportingUsersInputDto x) => { return _adminService.GetSalesReportingToUsersByCityStateDistrict(x); });
        }

        [HttpPost]
        [Route("reportingToZonalHeadUsers/userid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetReportingToZonalHeadUsersByUserId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetReportingToZonalHeadUsersByUserId([FromBody]string inputKey)
        {
            _methodName = "GetReportingToUsersByUserId";
            return Result(inputKey, _methodName, (ReportingUsersInputDto x) => { return _adminService.GetReportingToZonalHeadUsersByUserId(x); });
        }

        [HttpPost]
        [Route("reportingToBDOUsers/userid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetReportingToBDOUsersByUserId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetReportingToBDOUsersByUserId([FromBody]string inputKey)
        {
            _methodName = "GetReportingToBDOUsersByUserId";
            return Result(inputKey, _methodName, (ReportingUsersInputDto x) => { return _adminService.GetReportingToBDOUsersByUserId(x); });
        }

        [HttpPost]
        [Route("reportingToRABDOUsers/userid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetReportingToRABDOUsersByUserId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetReportingToRABDOUsersByUserId([FromBody]string inputKey)
        {
            _methodName = "GetReportingToRABDOUsersByUserId";
            return Result(inputKey, _methodName, (ReportingUsersInputDto x) => { return _adminService.GetReportingToRABDOUsersByUserId(x); });
        }

        [HttpPost]
        [Route("getclaimbyroleid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetClaimsByRoleId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetClaimsByRoleId([FromBody] string inputKey)
        {
            _methodName = "GetClaimsByRoleId";
            return Result(inputKey, _methodName, (RoleIdDto x) => { return _adminService.GetClaimsByRoleId(x); });
        }

        [HttpPost]
        [Route("getclaimbyroletypeid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetClaimsByRoleTypeId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetClaimsByRoleTypeId([FromBody] string inputKey)
        {
            _methodName = "GetClaimsByRoleTypeId";
            return Result(inputKey, _methodName, (RoleIdDto x) => { return _adminService.GetClaimsByRoleTypeId(x); });
        }
        #endregion

        #region Booking Restriction Roles

        [HttpGet]
        [Route("get/booking/restrictionroleids")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetBookingRestrictionRoleIds", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetBookingRestrictionRoleIds()
        {
            _methodName = "GetBookingRestrictionRoleIds";
            return Result(_methodName, () => { return _roleService.GetSuadaBookingRestrictionRoleIds(); });
        }

        #endregion
    }
}
