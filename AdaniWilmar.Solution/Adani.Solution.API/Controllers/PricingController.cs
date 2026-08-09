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
using System.Reflection;
using System.Linq.Expressions;
using System.Linq;
using Adani.Solution.API.App_Start;

namespace Adani.Solution.API.Controllers
{
    [AuthorizeUser]
    [CustomException]
    [AuditLogFilter]
    [RoutePrefix("api/pricing")]
    public class PricingController : BaseApiController
    {
        private const string ServiceName = "Pricing Controller";
        private readonly new ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IPricingService _pricingService;
        private string _methodName;

        public PricingController(IPricingService pricingService)
           : base(ServiceName)
        {
            _methodName = "Pricing Controller";
            try
            {
                _pricingService = pricingService;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
            }
        }




        #region Discount

        [HttpPost]
        [Route("update/rolediscount")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "UpdateRoleBasedDiscount", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateRoleBasedDiscount([FromBody]string inputKey)
        {
            _methodName = "UpdateRoleBasedDiscount";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            RoleDiscountDto inputDto;
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
                    inputDto = JsonHelper.ConvertJSonToObject<RoleDiscountDto>(decryptedInput);
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
                result = _pricingService.UpdateRoleBasedDiscount(inputDto);
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
        [Route("get/rolediscountall")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetRoleBasedDiscounts", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetRoleBasedDiscounts([FromBody]string inputKey)
        {
            _methodName = "GetRoleBasedDiscounts";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            RoleDiscountDto inputDto;
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
                    inputDto = JsonHelper.ConvertJSonToObject<RoleDiscountDto>(decryptedInput);
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
                result = _pricingService.GetRoleBasedDiscounts(inputDto);
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
        [Route("get/rolediscountbyid")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetRoleBasedDiscountById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetRoleBasedDiscountById([FromBody]string inputKey)
        {
            _methodName = "GetRoleBasedDiscountById";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            RoleDiscountDto inputDto;
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
                    inputDto = JsonHelper.ConvertJSonToObject<RoleDiscountDto>(decryptedInput);
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
                result = _pricingService.GetRoleBasedDiscountById(inputDto);
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
        [Route("add/skudepotdiscount")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddDiscount", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddDiscount([FromBody]string inputKey)
        {
            _methodName = "AddDiscount";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            SkuDepotDiscountDto inputDto;
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
                    inputDto = JsonHelper.ConvertJSonToObject<SkuDepotDiscountDto>(decryptedInput);
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
                result = _pricingService.AddDiscount(inputDto);
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
        [Route("update/skudepotdiscount")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "UpdateSkuDepotBasedDiscount", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateSkuDepotBasedDiscount([FromBody]string inputKey)
        {
            _methodName = "UpdateSkuDepotBasedDiscount";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            SkuDepotDiscountDto inputDto;
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
                    inputDto = JsonHelper.ConvertJSonToObject<SkuDepotDiscountDto>(decryptedInput);
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
                result = _pricingService.UpdateDiscount(inputDto);
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
        [Route("get/skudepotdiscountall")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetSkuDepotBasedDiscounts", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuDepotBasedDiscounts([FromBody]string inputKey)
        {
            _methodName = "GetSkuDepotBasedDiscounts";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            CustomerDiscountinputDto inputDto;
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
                    inputDto = JsonHelper.ConvertJSonToObject<CustomerDiscountinputDto>(decryptedInput);
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
                result = _pricingService.GetDiscountList(inputDto);
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
        [Route("get/skudepotdiscountbyid")]
        [ResponseType(typeof(ContentDto))]
        //[Throttle(Name = "GetSkuDepotBasedDiscountById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSkuDepotBasedDiscountById([FromBody]string inputKey)
        {
            _methodName = "GetSkuDepotBasedDiscountById";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            CustomerDiscountinputDto inputDto;
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
                    inputDto = JsonHelper.ConvertJSonToObject<CustomerDiscountinputDto>(decryptedInput);
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
                result = _pricingService.GetSkuDepotBasedDiscountById(inputDto);
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

        #region Dropdown

        [HttpPost]
        [Route("getoiltypedetailsddl")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetOilTypeDetailsddl([FromBody]string inputKey)
        {
            _methodName = "GetOilTypeDetailsddl";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            OilTypeDto inputDto;
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
                    inputDto = JsonHelper.ConvertJSonToObject<OilTypeDto>(decryptedInput);
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
                result = _pricingService.GetOilTypeDetailsddl(inputDto);
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
        [Route("getdepotdetailsddl")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetDepotDetailsddl([FromBody]string inputKey)
        {
            _methodName = "GetDepotDetailsddl";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            DepotDto inputDto;
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
                    inputDto = JsonHelper.ConvertJSonToObject<DepotDto>(decryptedInput);
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
                result = _pricingService.GetDepotDetailsddl(inputDto);
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
        [Route("getuserdetailsddl")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetUserDetailsddl([FromBody]string inputKey)
        {
            _methodName = "GetUserDetailsddl";
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
                result = _pricingService.GetUserDetailsddl(inputDto);
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
        [Route("getskudetailsddl")]
        [ResponseType(typeof(ContentDto))]
        public IHttpActionResult GetSkuDetailsddl([FromBody]string inputKey)
        {
            _methodName = "GetSkuDetailsddl";
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            OilTypeDto inputDto;
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
                    inputDto = JsonHelper.ConvertJSonToObject<OilTypeDto>(decryptedInput);
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
                result = _pricingService.GetSkuDetailsddl(inputDto);
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

        #region Role Discount

        [HttpPost]
        [Route("post/adminrolediscount")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddRoleDiscount", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddRoleDiscount([FromBody]string inputKey)
        {
            _methodName = "AddRoleDiscount";
            return Result(inputKey, _methodName, (RoleDisocuntDto x) => { return _pricingService.AddRoleDiscount(x); });
        }

        [HttpPost]
        [Route("update/adminrolediscount")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateRoleDiscount", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateRoleDiscount([FromBody]string inputKey)
        {
            _methodName = "UpdateRoleDiscount";
            return Result(inputKey, _methodName, (RoleDisocuntDto x) => { return _pricingService.UpdateRoleDiscount(x); });
        }

        [HttpPost]
        [Route("get/adminrolediscountbyid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetRoleDiscountbyId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetRoleDiscountbyId([FromBody]string inputKey)
        {
            _methodName = "GetRoleDiscountbyId";
            return Result(inputKey, _methodName, (RoleDisocuntInputDto x) => { return _pricingService.GetRoleDiscountbyId(x); });
        }

        [HttpPost]
        [Route("get/adminrolediscountall")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetRoleDiscountsAll", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetRoleDiscountsAll([FromBody]string inputKey)
        {
            _methodName = "GetRoleDiscountsAll";
            return Result(inputKey, _methodName, (RoleDisocuntInputDto x) => { return _pricingService.GetRoleDiscountsAll(x); });
        }

        #endregion

        #region Request Discount

        [HttpPost]
        [Route("get/requestdiscountall")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetRequestDiscountsAll", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetRequestDiscountsAll([FromBody]string inputKey)
        {
            _methodName = "GetRequestDiscountsAll";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _pricingService.GetRequestDiscountsAll(x); });
        }

        [HttpPost]
        [Route("get/requestdiscountlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetRequestDiscountList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetRequestDiscountList([FromBody]string inputKey)
        {
            _methodName = "GetRequestDiscountList";
            return Result(inputKey, _methodName, (RequestDisocuntInputDto x) => { return _pricingService.GetRequestDiscountList(x); });
        }

        [HttpPost]
        [Route("get/requestdiscountdetails")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetRequestDiscountbyId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetRequestDiscountbyId([FromBody]string inputKey)
        {
            _methodName = "GetRequestDiscountbyId";
            return Result(inputKey, _methodName, (RequestDisocuntInputDto x) => { return _pricingService.GetRequestDiscountbyId(x); });
        }

        [HttpPost]
        [Route("update/requestdiscount")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateRequestDiscount", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateRequestDiscount([FromBody]string inputKey)
        {
            _methodName = "UpdateRequestDiscount";
            return Result(inputKey, _methodName, (RequestDisocuntUpdateDto x) => { return _pricingService.UpdateRequestDiscount(x); });
        }

        [HttpPost]
        [Route("get/requestdiscountdetailsbyid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetRequestDiscountDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetRequestDiscountDetailsById([FromBody]string inputKey)
        {
            _methodName = "GetRequestDiscountDetailsById";
            return Result(inputKey, _methodName, (RequestDisocuntInputDto x) => { return _pricingService.GetRequestDiscountDetailsById(x); });
        }

        [HttpPost]
        [Route("get/getrequesteddiscounts")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetRequestedDiscounts", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetRequestedDiscounts([FromBody]string inputKey)
        {
            _methodName = "GetRequestedDiscounts";
            return Result(inputKey, _methodName, (RequestDisocuntInputDto x) => { return _pricingService.GetRequestedDiscounts(x); });
        }

        [HttpPost]
        [Route("post/approverequestdiscount")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ApproveRequestedDiscount", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ApproveRequestedDiscount([FromBody]string inputKey)
        {
            _methodName = "ApproveRequestedDiscount";
            return Result(inputKey, _methodName, (ApproveRequestDiscountDto x) => { return _pricingService.ApproveRequestedDiscount(x); });
        }

        #endregion

        #region Approve Pending Request

        [HttpPost]
        [Route("premium/approvepremiumrequestlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPremiumDiscountForPending", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPremiumDiscountForPending([FromBody]string inputKey)
        {
            _methodName = "GetPremiumDiscountForPending";
            return Result(inputKey, _methodName, (PremiumDisocuntRequestInputDto x) => { return _pricingService.GetPremiumDiscountForPending(x); });
        }

        [HttpPost]
        [Route("premium/approvepremiumrequestupdate")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateApprovePremiumDiscount", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ApprovePremiumDiscountUpdate([FromBody]string inputKey)
        {
            _methodName = "UpdateApprovePremiumDiscount";
            return Result(inputKey, _methodName, (ApprovePremiunDiscountRequestDto x) => { return _pricingService.UpdateApprovePremiumDiscount(x); });
        }

        #endregion

        #region Primary Discount Users

        [HttpPost]
        [Route("premiumuser/addpremiumuser")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddPrimaryDiscountForUser", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddPrimaryDiscountForUser([FromBody]string inputKey)
        {
            _methodName = "AddPrimaryDiscountForUser";
            return Result(inputKey, _methodName, (PrimaryDiscountUserDto x) => { return _pricingService.AddPrimaryDiscountForUser(x); });
        }

        [HttpPost]
        [Route("premiumuser/updatepremiumuser")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdatePrimaryDiscountForUser", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdatePrimaryDiscountForUser([FromBody]string inputKey)
        {
            _methodName = "UpdatePrimaryDiscountForUser";
            return Result(inputKey, _methodName, (PrimaryDiscountUserDto x) => { return _pricingService.UpdatePrimaryDiscountForUser(x); });
        }

        [HttpPost]
        [Route("premiumuser/premiumuserlist")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPrimaryDiscountForUserList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPrimaryDiscountForUserList([FromBody]string inputKey)
        {
            _methodName = "GetPrimaryDiscountForUserList";
            return Result(inputKey, _methodName, (PrimaryDiscountUserInputDto x) => { return _pricingService.GetPrimaryDiscountForUserList(x); });
        }

        [HttpPost]
        [Route("premiumuser/premiumuserbyid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPrimaryDiscountForUserById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPrimaryDiscountForUserById([FromBody]string inputKey)
        {
            _methodName = "GetPrimaryDiscountForUserById";
            return Result(inputKey, _methodName, (PrimaryDiscountUserInputDto x) => { return _pricingService.GetPrimaryDiscountForUserById(x); });
        }

        #endregion

        #region Reverse Auction Margin

        /// <summary>
        /// Method to Save RaMargin
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ramargin/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveRaMargin", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveRaMargin([FromBody]string inputKey)
        {
            _methodName = "SaveRaMargin";
            return Result(inputKey, _methodName, (RaMarginDto x) => { return _pricingService.SaveRaMargin(x); });
        }

        /// <summary>
        /// Method to Get RaMargin List
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ramargin/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetRaMarginList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetRaMarginList([FromBody]string inputKey)
        {
            _methodName = "GetRaMarginList";
            //return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _pricingService.GetRaMarginList(x); });
            return KendoGridResult(inputKey, _methodName, (KendoGridResult x) => { return _pricingService.GetRaMarginListWithPaging(x); });
        }

        [HttpPost]
        [Route("ramargin/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "ExportRaMargin", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult ExportRaMargin([FromBody]string inputKey)
        {
            _methodName = "ExportRaMargin";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _pricingService.ExportRaMargin(x); });
        }

        /// <summary>
        /// Method to get Get RaMargin Details By Id
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get/ramarginid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetRaMarginDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetRaMarginDetailsById([FromBody]string inputKey)
        {
            _methodName = "GetRaMarginDetailsById";
            return Result(inputKey, _methodName, (long x) => { return _pricingService.GetRaMarginDetailsById(x); });
        }

        /// <summary>
        /// Method to Update RaMargin
        /// </summary>
        /// <param name="inputKey"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("ramargin/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateRaMargin", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateRaMargin([FromBody]string inputKey)
        {
            _methodName = "UpdateRaMargin";
            return Result(inputKey, _methodName, (RaMarginDto x) => { return _pricingService.UpdateRaMargin(x); });
        }

        #endregion  

        #region Geography Discount

        [HttpPost]
        [Route("geographydiscount/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddDiscountGeography", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddDiscountGeography([FromBody]string inputKey)
        {
            _methodName = "AddDiscountGeography";
            return Result(inputKey, _methodName, (DiscountInputDto x) => { return _pricingService.AddDiscountGeography(x); });
        }

        [HttpPost]
        [Route("getcitydetails/territoryids")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCityDetailsBasedOnTerritory", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCityDetailsBasedOnTerritory([FromBody]string inputKey)
        {
            _methodName = "GetCityDetailsBasedOnTerritory";
            return Result(inputKey, _methodName, (TerritoryId x) => { return _pricingService.GetCityDetailsBasedOnTerritory(x); });
        }

        [HttpPost]
        [Route("getgeography/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetGeographyList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetGeographyList([FromBody]string inputKey)
        {
            _methodName = "GetGeographyList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _pricingService.GetGeographyList(x); });
        }

        [HttpPost]
        [Route("getgeographycity/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetGeographyCityList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetGeographyCityList([FromBody]string inputKey)
        {
            _methodName = "GetGeographyCityList";
            return Result(inputKey, _methodName, (GeographyCityListParam x) => { return _pricingService.GetGeographyCityList(x); });
        }

        [HttpPost]
        [Route("getgeographycitymobile/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetGeographyCityListMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetGeographyCityListMobile([FromBody] string inputKey)
        {
            _methodName = "GetGeographyCityList";
            return Result(inputKey, _methodName, (GeographyDiscountCityListParam x) => { return _pricingService.GetGeographyCityListMobile(x); });
        }

        [HttpPost]
        [Route("getgeography/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetGeographyDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetGeographyDetailsById([FromBody]string inputKey)
        {
            _methodName = "GetGeographyDetailsById";
            return Result(inputKey, _methodName, (long x) => { return _pricingService.GetGeographyDetailsById(x); });
        }

        [HttpPost]
        [Route("geographydiscount/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateDiscountGeography", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateDiscountGeography([FromBody]string inputKey)
        {
            _methodName = "UpdateDiscountGeography";
            return Result(inputKey, _methodName, (DiscountInputDto x) => { return _pricingService.UpdateDiscountGeography(x); });
        }

        #endregion

        #region Discount User

        [HttpPost]
        [Route("discountuser/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddDiscountUsers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddDiscountUsers([FromBody]string inputKey)
        {
            _methodName = "AddDiscountUsers";
            return Result(inputKey, _methodName, (DiscountUserDto x) => { return _pricingService.AddDiscountUsers(x); });
        }

        [HttpPost]
        [Route("discountuser/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDiscountUserList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDiscountUserList([FromBody]string inputKey)
        {
            _methodName = "GetDiscountUserList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _pricingService.GetDiscountUserList(x); });
        }
        [HttpPost]
        [Route("discountuser/list/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "DiscountUserExport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult DiscountUserExport([FromBody] string inputKey)
        {
            _methodName = "DiscountUserExport";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _pricingService.DiscountUserExport(x); });
        }
        [HttpPost]
        [Route("discountuser/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDiscountUserById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDiscountUserById([FromBody]string inputKey)
        {
            _methodName = "GetDiscountUserById";
            return Result(inputKey, _methodName, (long x) => { return _pricingService.GetDiscountUserById(x); });
        }

        [HttpPost]
        [Route("discountuser/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateDiscountUsers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateDiscountUsers([FromBody]string inputKey)
        {
            _methodName = "UpdateDiscountUsers";
            return Result(inputKey, _methodName, (DiscountUserDto x) => { return _pricingService.UpdateDiscountUsers(x); });
        }

        [HttpPost]
        [Route("discountuserdetails/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetDiscountUserDetailList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetDiscountUserDetailList([FromBody]string inputKey)
        {
            _methodName = "GetDiscountUserDetailList";
            return Result(inputKey, _methodName, (GeographyCityListParam x) => { return _pricingService.GetDiscountUserDetailList(x); });
        }

        [HttpPost]
        [Route("employeeuserdiscount/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetEmployeeAndUserDiscountList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetEmployeeAndUserDiscountList([FromBody]string inputKey)
        {
            _methodName = "GetEmployeeAndUserDiscountList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _pricingService.GetEmployeeAndUserDiscountList(x); });
        }

        [HttpPost]
        [Route("employeeuserdiscount/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetEmployeeAndUserDiscountById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetEmployeeAndUserDiscountById([FromBody]string inputKey)
        {
            _methodName = "GetEmployeeAndUserDiscountById";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _pricingService.GetEmployeeAndUserDiscountById(x); });
        }

        [HttpPost]
        [Route("employeeuserdiscount/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddEmployeeAndUserDiscount", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddEmployeeAndUserDiscount([FromBody]string inputKey)
        {
            _methodName = "AddEmployeeAndUserDiscount";
            return Result(inputKey, _methodName, (EmployeeUserDiscountDto x) => { return _pricingService.AddEmployeeAndUserDiscount(x); });
        }
        #endregion

        #region PriceNotifyConfiguration

        [HttpPost]
        [Route("pricenotifyconfiguration/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddorUpdatePriceNotifyConfiguration", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddorUpdatePriceNotifyConfiguration([FromBody]string inputKey)
        {
            _methodName = "AddorUpdatePriceNotifyConfiguration";
            return Result(inputKey, _methodName, (PriceNotifyConfigurationDto x) => { return _pricingService.AddorUpdatePriceNotifyConfiguration(x); });
        }


        [HttpPost]
        [Route("pricenotifyconfiguration/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPriceNotifyConfigurationList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPriceNotifyConfigurationList([FromBody]string inputKey)
        {
            _methodName = "GetPriceNotifyConfigurationList";
            return Result(inputKey, _methodName, (SaudaLimitInputDto x) => { return _pricingService.GetPriceNotifyConfigurationList(x); });
        }

        [HttpPost]
        [Route("getpricenotifyconfigurationcity/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPriceNotifyConfigurationCityList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPriceNotifyConfigurationCityList([FromBody]string inputKey)
        {
            _methodName = "GetPriceNotifyConfigurationCityList";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _pricingService.GetPriceNotifyConfigurationCityList(x); });
        }

        [HttpPost]
        [Route("getpricenotifyconfiguration/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetPriceNotifyconfigurationDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetPriceNotifyconfigurationDetailsById([FromBody]string inputKey)
        {
            _methodName = "GetPriceNotifyconfigurationDetailsById";
            return Result(inputKey, _methodName, (long x) => { return _pricingService.GetPriceNotifyconfigurationDetailsById(x); });
        }

        [HttpPost]
        [Route("pricenotifyconfiguration/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdatePriceNotifyconfiguration", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdatePriceNotifyconfiguration([FromBody]string inputKey)
        {
            _methodName = "UpdatePriceNotifyconfiguration";
            return Result(inputKey, _methodName, (PriceNotifyConfigurationDto x) => { return _pricingService.UpdatePriceNotifyconfiguration(x); });
        }

        #endregion

        #region SpecialtyFat Geography Discount

        [HttpPost]
        [Route("geographydiscount/SpecialtyFat/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddSpecialityFatDiscountGeography", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddSpecialityFatDiscountGeography([FromBody]string inputKey)
        {
            _methodName = "AddSpecialityFatDiscountGeography";
            return Result(inputKey, _methodName, (SpecialityFatDiscountInputDto x) => { return _pricingService.AddSpecialityFatDiscountGeography(x); });
        }

        [HttpPost]
        [Route("geographydiscount/SpecialtyFat/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateSpecialityFatDiscountGeography", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateSpecialityFatDiscountGeography([FromBody]string inputKey)
        {
            _methodName = "UpdateSpecialityFatDiscountGeography";
            return Result(inputKey, _methodName, (SpecialityFatDiscountInputDto x) => { return _pricingService.UpdateSpecialityFatDiscountGeography(x); });
        }

        [HttpPost]
        [Route("getgeography/SpecialtyFat/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialityFatGeographyDetailsById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialityFatGeographyDetailsById([FromBody]string inputKey)
        {
            _methodName = "GetSpecialityFatGeographyDetailsById";
            return Result(inputKey, _methodName, (long x) => { return _pricingService.GetSpecialityFatGeographyDetailsById(x); });
        }

        [HttpPost]
        [Route("getgeography/SpecialtyFat/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialityFatGeographyList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialityFatGeographyList([FromBody]string inputKey)
        {
            _methodName = "GetSpecialityFatGeographyList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _pricingService.GetSpecialityFatGeographyList(x); });
        }

        [HttpPost]
        [Route("getgeographycity/SpecialtyFat/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialityFatGeographyCityList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialityFatGeographyCityList([FromBody]string inputKey)
        {
            _methodName = "GetSpecialityFatGeographyCityList";
            return Result(inputKey, _methodName, (GeographyCityListParam x) => { return _pricingService.GetSpecialityFatGeographyCityList(x); });
        }

        [HttpPost]
        [Route("specialtyfat/getgeographycitylist/cityidterritory")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetCityDetailsBasedOnTerritoryAndCity", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetCityDetailsBasedOnTerritoryAndCity([FromBody]string inputKey)
        {
            _methodName = "GetCityDetailsBasedOnTerritoryAndCity";
            return Result(inputKey, _methodName, (TerritoryId x) => { return _pricingService.GetCityDetailsBasedOnTerritoryAndCity(x); });
        }

        #endregion

        #region SpecialityFat Discount User List

        [HttpPost]
        [Route("specialityfat/discountuser/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddSpecialityFatDiscountUsers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddSpecialityFatDiscountUsers([FromBody]string inputKey)
        {
            _methodName = "AddSpecialityFatDiscountUsers";
            return Result(inputKey, _methodName, (SpecialityFatDiscountUserDto x) => { return _pricingService.AddSpecialityFatDiscountUsers(x); });
        }

        [HttpPost]
        [Route("specialityfat/discountuser/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialityFatDiscountUserList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialityFatDiscountUserList([FromBody]string inputKey)
        {
            _methodName = "GetSpecialityFatDiscountUserList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _pricingService.GetSpecialityFatDiscountUserList(x); });
        }
        [HttpPost]
        [Route("specialityfat/discountuser/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialityFatDiscountUserExport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialityFatDiscountUserExport([FromBody] string inputKey)
        {
            _methodName = "GetSpecialityFatDiscountUserExport";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _pricingService.GetSpecialityFatDiscountUserExport(x); });
        }

        [HttpPost]
        [Route("specialityfat/discountuser/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialityFatDiscountUserById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialityFatDiscountUserById([FromBody]string inputKey)
        {
            _methodName = "GetSpecialityFatDiscountUserById";
            return Result(inputKey, _methodName, (long x) => { return _pricingService.GetSpecialityFatDiscountUserById(x); });
        }

        [HttpPost]
        [Route("specialityfat/discountuser/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateSpecialityFatDiscountUsers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateSpecialityFatDiscountUsers([FromBody]string inputKey)
        {
            _methodName = "UpdateSpecialityFatDiscountUsers";
            return Result(inputKey, _methodName, (SpecialityFatDiscountUserDto x) => { return _pricingService.UpdateSpecialityFatDiscountUsers(x); });
        }

        [HttpPost]
        [Route("specialityfat/discountuserdetails/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialityFatDiscountUserDetailList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialityFatDiscountUserDetailList([FromBody]string inputKey)
        {
            _methodName = "GetSpecialityFatDiscountUserDetailList";
            return Result(inputKey, _methodName, (GeographyCityListParam x) => { return _pricingService.GetSpecialityFatDiscountUserDetailList(x); });
        }

        [HttpPost]
        [Route("specialityfat/assigneddiscount/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialityFatEmployeeDiscountList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialityFatEmployeeDiscountList([FromBody]string inputKey)
        {
            _methodName = "GetSpecialityFatEmployeeDiscountList";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _pricingService.GetSpecialityFatEmployeeDiscountList(x); });
        }

        [HttpPost]
        [Route("specialityfat/assigneddiscount/export")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialityFatEmployeeDiscountExport", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialityFatEmployeeDiscountExport([FromBody] string inputKey)
        {
            _methodName = "GetSpecialityFatEmployeeDiscountExport";
            return Result(inputKey, _methodName, (LoginUserIdDto x) => { return _pricingService.GetSpecialityFatEmployeeDiscountExport(x); });
        }

        [HttpPost]
        [Route("specialityfat/assigneddiscountuserdetails/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialityFatDiscountEmployeeDetailList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialityFatDiscountEmployeeDetailList([FromBody] string inputKey)
        {
            _methodName = "GetSpecialityFatDiscountEmployeeDetailList";
            return Result(inputKey, _methodName, (GeographyCityListParam x) => { return _pricingService.GetSpecialityFatDiscountEmployeeDetailList(x); });
        }

        [HttpPost]
        [Route("specialityfat/assigneddiscount/id")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialityFatEmployeeDiscountById", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialityFatEmployeeDiscountById([FromBody]string inputKey)
        {
            _methodName = "GetSpecialityFatEmployeeDiscountById";
            return Result(inputKey, _methodName, (IdInputDto x) => { return _pricingService.GetSpecialityFatEmployeeDiscountById(x); });
        }

        [HttpPost]
        [Route("specialityfat/assigneddiscount/employee")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddSpecialityFatEmployeeDiscount", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddSpecialityFatEmployeeDiscount([FromBody]string inputKey)
        {
            _methodName = "AddSpecialityFatEmployeeDiscount";
            return Result(inputKey, _methodName, (SpecialityFatEmployeeDiscountDto x) => { return _pricingService.AddSpecialityFatEmployeeDiscount(x); });
        }

        #endregion

        #region SpecialtyFat Quantity Requests

        [HttpPost]
        [Route("specialtyfat/quantityrequest/add")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "AddSpecialtyFatQuantityRequests", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult AddSpecialtyFatQuantityRequests([FromBody]string inputKey)
        {
            _methodName = "AddSpecialtyFatQuantityRequests";
            return Result(inputKey, _methodName, (SpecialtyFatQuantityRequestDto x) => { return _pricingService.AddSpecialtyFatQuantityRequests(x); });
        }

        [HttpPost]
        [Route("specialtyfat/quantityrequest/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateSpecialtyFatQuantityRequests", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateSpecialtyFatQuantityRequests([FromBody]string inputKey)
        {
            _methodName = "UpdateSpecialtyFatQuantityRequests";
            return Result(inputKey, _methodName, (SpecialtyFatQuantityRequestDto x) => { return _pricingService.UpdateSpecialtyFatQuantityRequests(x); });
        }

        [HttpPost]
        [Route("specialtyfat/quantityrequest/organizationreportingtoId/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialtyFatQuantityRequestsListByOrganizationReportingToId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialtyFatQuantityRequestsListByOrganizationReportingToId([FromBody]string inputKey)
        {
            _methodName = "GetSpecialtyFatQuantityRequestsListByOrganizationReportingToId";
            return Result(inputKey, _methodName, (SpecialtyFatQuantityRequestSearchDto x) => { return _pricingService.GetSpecialtyFatQuantityRequestsListByOrganizationReportingToId(x); });
        }
        [HttpPost]
        [Route("mobile/quantityrequest/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialtyFatQuantityRequestsListForMobile", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialtyFatQuantityRequestsListForMobile([FromBody] string inputKey)
        {
            _methodName = "GetSpecialtyFatQuantityRequestsListForMobile";
            return Result(inputKey, _methodName, (SpecialtyFatQuantityRequestSearchDto x) => { return _pricingService.GetSpecialtyFatQuantityRequestsListForMobile(x); });
        }

        [HttpPost]
        [Route("specialtyfat/quantityrequest/list")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetSpecialtyFatQuantityRequestsList", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetSpecialtyFatQuantityRequestsList([FromBody]string inputKey)
        {
            _methodName = "GetSpecialtyFatQuantityRequestsList";
            return Result(inputKey, _methodName, (SpecialtyFatQuantityRequestSearchDto x) => { return _pricingService.GetSpecialtyFatQuantityRequestsList(x); });
        }

        [HttpPost]
        [Route("specialtyfat/quantitylimit/update")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "UpdateSpecialtyFatQuantityRequests", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult UpdateSpecialtyFatQuantityLimit([FromBody]string inputKey)
        {
            _methodName = "UpdateSpecialtyFatQuantityRequests";
            return Result(inputKey, _methodName, (SpecialtyFatQuantityRequestDto x) => { return _pricingService.UpdateSpecialtyFatQuantityLimit(x); });
        }
        #endregion

        #region  Auto Allocation

        [HttpPost]
        [Route("autoallocationlist/roleids")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetAutoAllocationUserListByRoleIds", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetAutoAllocationUserListByRoleIds([FromBody]string inputKey)
        {
            _methodName = "GetAutoAllocationUserListByRoleIds";
            return Result(inputKey, _methodName, (AutoAllocationInputDto x) => { return _pricingService.GetAutoAllocationUserListByRoleIds(x); });
        }

        [HttpPost]
        [Route("autoallocationdetails/userid")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "GetAutoAllocationDetailsByUserId", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult GetAutoAllocationDetailsByUserId([FromBody]string inputKey)
        {
            _methodName = "GetAutoAllocationDetailsByUserId";
            return Result(inputKey, _methodName, (AutoAllocationInputDto x) => { return _pricingService.GetAutoAllocationDetailsByUserId(x); });
        }

        [HttpPost]
        [Route("specalityfatdiscountusers/save")]
        [ResponseType(typeof(ContentDto))]
        [Throttle(Name = "SaveSpecalityFatDiscountUsers", Message = "The request has been declined for security reasons.", Seconds = 1)]
        public IHttpActionResult SaveSpecalityFatDiscountUsers([FromBody]string inputKey)
        {
            _methodName = "SaveSpecalityFatDiscountUsers";
            return Result(inputKey, _methodName, (SaveAutoAllocationDetailDto x) => { return _pricingService.SaveSpecalityFatDiscountUsers(x); });
        }

        #endregion


    }
}
