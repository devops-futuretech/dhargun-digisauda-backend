using Adani.Solution.DTO;
using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Helpers;
using GMCore.Helper;
using GMCore.Logger;
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
    public class MailRedirectClient : BaseClient
    {
        private const string ServiceName = "Mail Redirect Client";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;


        public async Task<SaudaOrderDetails> GetSaudaCounterBidDetails(SaudaDetailInputDto inputDto)
        {
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new SaudaOrderDetails();
            try
            {

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSaudaCounterBidDetails, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<SaudaOrderDetails>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                    }
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        result.PostStatus = false;
                        result.PostMessage = errorDtoResult.Message;
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

        public async Task<SaudaOrderDetails> ApproveCounterBid(CounterBidInputDto inputDto)
        {
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new SaudaOrderDetails();
            try
            {
                var inputDtoJson = string.Empty;
                inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostApproveCounterBid, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();

                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultDto = JsonConvert.DeserializeObject<SuccessDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        result.PostStatus = true;
                        result.PostMessage = resultDto.Response.ToString();
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
                result.PostMessage = Helper.GetResourceString("msg_CounterBidStatusUpdatedError");
                _logger.Error(message);
            }
            return result;           
        }
    }
}