using Adani.Solution.DTO;
using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Helpers;
using GMCore.Helper;
using GMCore.Logger;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Threading.Tasks;

namespace Adani.Solution.MVC.ServiceClient
{
    public class CrossAndUpsellClient : BaseClient
    {
        private const string ServiceName = "CrossAndUpsell Client";
        private readonly ILogger _logger = Logging.GetLogger("CrossAndUpsellClient");
        private string _methodName;
       
       public async Task<ResultDto> AddAndUpdateCrossAndUpsellConfiguration(CrossAndUpsellConfigurationDto inputDto)
       {
            _methodName = "AddAndUpdateCrossAndUpsellConfiguration";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new ResultDto();

            try
            {
                var apiUrl = inputDto.Id > 0 ? ApiUrl.WebApiUrlUpdateCrossAndUpsell : ApiUrl.WebApiUrlAddCrossAndUpsell;
                var inputDtoJson = string.Empty;
                inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(apiUrl, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var successDto = JsonConvert.DeserializeObject<SuccessDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        result.IsSuccess = true;
                        result.SuccessDto.Message = successDto.Message;
                    }
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        result.IsSuccess = false;
                        result.ErrorDto.Message = errorDtoResult.Message;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.ErrorDto.Message = ja[0][Settings.ResponseMessage].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                result.IsSuccess = false;
                result.ErrorDto.Message = Helper.GetResourceString("msg_CrossAndUpsellError");
                _logger.Error(message);
            }

            return result;
       }
        public async Task<List<SaudaConditionalBookingConfigurationListDto>> GetSaudaConditionalBokkingConfigurationList(SuadaConditionalBookingInputDto inputDto)
        {
            _methodName = "GetSaudaConditionalBokkingConfigurationList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new List<SaudaConditionalBookingConfigurationListDto>();

            try
            {
                var inputDtoJson = string.Empty;
                inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSuadaConditionalBookingConfigurationList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<SaudaConditionalBookingConfigurationListDto>>(jarray[0]["response"].ToString(),
                            UtilityHelper.GetJsonSettings());
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }

            return result;
        }

        public async Task<List<SaudaConditionalBookingSkuMappingListDto>> GetSaudaConditionalBokkingConfigurationSkusList(SuadaConditionalBookingInputDto inputDto)
        {
            _methodName = "GetSaudaConditionalBokkingConfigurationSkusList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new List<SaudaConditionalBookingSkuMappingListDto>();

            try
            {
                var inputDtoJson = string.Empty;
                inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSuadaConditionalBookingConfigurationSkusList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<SaudaConditionalBookingSkuMappingListDto>>(jarray[0]["response"].ToString(),
                            UtilityHelper.GetJsonSettings());
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }

            return result;
        }

        public async Task<CrossAndUpsellConfigurationDto> GetSaudaConditionalBokkingConfigurationDetails(SuadaConditionalBookingInputDto inputDto)
        {
            _methodName = "GetSaudaConditionalBokkingConfigurationDetails";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new CrossAndUpsellConfigurationDto();

            try
            {
                var inputDtoJson = string.Empty;
                inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSuadaConditionalBookingConfigurationDetails, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<CrossAndUpsellConfigurationDto> (jarray[0]["response"].ToString(),
                            UtilityHelper.GetJsonSettings());
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }

            return result;
        }

        public async Task<List<CrossAndUpsellConfigurationReportDto>> GetSaudaConditionalBokkingConfigurationListForReport(long UserId)
        {
            _methodName = "GetSaudaConditionalBokkingConfigurationListForReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new List<CrossAndUpsellConfigurationReportDto>();

            try
            {
                var inputDto = new SuadaConditionalBookingInputDto { LoginUserId = UserId };
                var inputDtoJson = string.Empty;
                inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSuadaConditionalBookingConfigurationListForReport, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<CrossAndUpsellConfigurationReportDto>>(jarray[0]["response"].ToString(),
                            UtilityHelper.GetJsonSettings());
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }

            return result;
        }
    }
}