using Adani.Solution.DTO;
using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Helpers;
using GMCore.Helper;
using GMCore.Logger;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Adani.Solution.MVC.ServiceClient
{
    public class QPSSchemeDiscountClient : BaseClient
    {
        private const string ServiceName = "QPSSchemeDiscount Client";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;
        static string connectionString = ConfigHelper.SPConnectionString;


        public async Task<QPSSchemeDiscountDto> QpsAddOrUpdate(QPSSchemeDiscountDto qPSSchemeDiscountDto)
        {
            _methodName = "QpsAddOrUpdate";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new QPSSchemeDiscountDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                apiUrl = ApiUrl.WebApiUrlQpsDiscount;
                inputDtoJson = JsonHelper.ConvertObjectToJson(qPSSchemeDiscountDto);
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
                        result.PostStatus = true;
                        result.PostMessage = qPSSchemeDiscountDto.Id > 0 ? Helper.GetResourceString("msg_UpdatedSuccessFully") : Helper.GetResourceString("msg_SavedSuccessFully");
                    }
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        result.PostStatus = false;
                        result.PostMessage = errorDtoResult.Message;
                    }
                }
                else
                {
                    result.PostStatus = false;
                    result.PostMessage = ja[0][Settings.ResponseMessage].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                result.PostStatus = false;
                result.PostMessage = Helper.GetResourceString("msg_DealerError");
                _logger.Error(message);
            }
            return result;
        }

        public async Task<IList<QPSSchemeDiscountDto>> QpsListAsync()
        {
            try
            {
                _methodName = "QpsListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDto = new QPSSchemeDiscountDto();
                var inputDtoJson = JsonHelper.ConvertObjectToJson<QPSSchemeDiscountDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlQpsDiscountList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<QPSSchemeDiscountDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<QPSSchemeDiscountDto>();
        }

        public async Task<List<QPSSchemeDiscountDto>> ExportQPSSchemeDiscount(LoginUserIdDto inputDto)
        {
            _methodName = "ExportQPSSchemeDiscountAsync";
            var result = await GetListAsync<QPSSchemeDiscountDto>(ApiUrl.WebApiUrlExportQpsDiscount, inputDto);
            return result.ToList();
        }

        public async Task<DataSourceResult> GetKendoGridDataAsync<T>(KendoGridResult inputDto, string apiUrl) where T : class
        {
            var result = await GetKendoGridResultAsync<T>(apiUrl, inputDto);
            return result;
        }
        
        public async Task<QPSSchemeDiscountDto> GetQpsDiscountById(QPSSchemeDiscountDto inputDto)
        {
            _methodName = "GetQpsDiscountById";
            //var result = await GetById<QPSSchemeDiscountDto>("api/qps/qpsdiscountlist/getbyId", inputDto.Id);
            var result = await GetById<QPSSchemeDiscountDto>(ApiUrl.WebApiUrlGetQpsDiscountById, inputDto.Id);
            return result;
        }
    }
}