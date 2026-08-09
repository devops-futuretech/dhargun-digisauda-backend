using GMCore.Helper;
using Adani.Solution.DTO.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using GMCore.Logger;
using Adani.Solution.DTO;
using Adani.Solution.Service.Common;
using System.Net;
using Adani.Solution.DTO.Common;
using Newtonsoft.Json;
using Kendo.Mvc;

namespace Adani.Solution.API.Controllers
{
    public class BaseApiController : ApiController
    {
        public readonly ILogger _logger;
        private readonly string ServiceOrgin;
        public BaseApiController(string serviceName)
        {
            ServiceOrgin = serviceName;
            _logger = Logging.GetLogger(serviceName);
        }
        public BaseApiController()
        {

        }


        public bool IsAdmin => HttpContext.Current.User.IsInRole(EnumHelper.GetEnumDescription(Role.Admin));
        //public bool IsUser => HttpContext.Current.User.IsInRole(EnumHelper.GetEnumDescription(Role.DealerUser));

        public int UserId
        {
            get
            {
                var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
                return Convert.ToInt32(identity.FindFirst("UserId")?.Value);
            }
        }

        public List<int> RoleIds
        {
            get
            {
                var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
                return identity.FindFirst("RoleId").Value.Split(',').Select(int.Parse).ToList();
            }
        }

        public List<int> ClaimIds
        {
            get
            {
                var identity = (ClaimsIdentity)HttpContext.Current.User.Identity;
                return identity.FindFirst("ClaimId").Value.Split(',').Select(int.Parse).ToList();
            }
        }

        protected IHttpActionResult Result(string methodName, Func<ResultDto> delegatemethod)
        {
            var result = new ResultDto();
            var errorDto = new ErrorDto();
            var successDto = new SuccessDto();
            var contentDto = new ContentDto();
            try
            {
                _logger.Info($"{ServiceOrgin} Controller-Method {methodName}");
                result = delegatemethod.Invoke();
            }
            catch (Exception exception)
            {
                var message = $"{ServiceOrgin} Controller-Method {methodName} Exception: {exception}";
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

        protected IHttpActionResult Result<T>(string inputKey, string methodName, Func<T, ResultDto> delegatemethod)
        {
            {
                var result = new ResultDto();
                var errorDto = new ErrorDto();
                var successDto = new SuccessDto();
                var contentDto = new ContentDto();
                T input;
                try
                {
                    _logger.Info($"{ServiceOrgin} Controller-Method {methodName}");
                    string decryptedInput;
                    try
                    {
                        decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceOrgin} Controller-Method {methodName} Exception: {exception}";
                        _logger.Error(message);
                        errorDto.ErrorCode = Constants.InvalidRequest;
                        errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                        contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                        return Ok(contentDto);
                    }
                    try
                    {
                        input = JsonHelper.ConvertJSonToObject<T>(decryptedInput);
                        _logger.Info($"Json Input : {JsonConvert.SerializeObject(input)}");
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceOrgin} Controller-Method {methodName} Exception: {exception}";
                        _logger.Error(message);
                        errorDto.ErrorCode = Constants.Exception;
                        errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                        contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                        return Ok(contentDto);
                    }
                    result = delegatemethod.Invoke(input);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceOrgin} Controller-Method {methodName} Exception: {exception}";
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
                    successDto.Message = result.SuccessDto.Message;
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

        /// <summary>
        /// Kendo Grid Sorts,Filters,Groups,Aggregates and PageSize based on data filtered
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputKey"></param>
        /// <param name="methodName"></param>
        /// <param name="delegatemethod"></param>
        /// <returns <see cref="DataSourceResult">></returns>
        protected IHttpActionResult KendoGridResult<T>(string inputKey, string methodName, Func<T, ResultDto> delegatemethod)
        {
            {
                var result = new ResultDto();
                var errorDto = new ErrorDto();
                var successDto = new SuccessDto();
                var contentDto = new ContentDto();
                T input;
                try
                {
                    _logger.Info($"{ServiceOrgin} Controller-Method {methodName}");
                    string decryptedInput;
                    try
                    {
                        decryptedInput = EncryptDecryptHelper.Decrypt(inputKey, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceOrgin} Controller-Method {methodName} Exception: {exception}";
                        _logger.Error(message);
                        errorDto.ErrorCode = Constants.InvalidRequest;
                        errorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                        contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                        return Ok(contentDto);
                    }
                    try
                    {
                        var settings = new JsonSerializerSettings();
                        settings.Converters.Add(new DataSourceRequestConverter());
                        input = JsonConvert.DeserializeObject<T>(decryptedInput, settings);
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceOrgin} Controller-Method {methodName} Exception: {exception}";
                        _logger.Error(message);
                        errorDto.ErrorCode = Constants.Exception;
                        errorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                        contentDto.SXVI7XCEU = Utility.DtoEncrypt(errorDto);
                        return Ok(contentDto);
                    }
                    result = delegatemethod.Invoke(input);
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceOrgin} Controller-Method {methodName} Exception: {exception}";
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

    /// <summary>
    /// Interfact to Class to converter setting
    /// </summary>
    class DataSourceRequestConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IFilterDescriptor));
        }
        //Kendo.Mvc.CompositeFilterDescriptor
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize(reader, typeof(FilterDescriptor));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value, typeof(FilterDescriptor));
        }
    }
}
