using Adani.Solution.DTO;
using Adani.Solution.MVC.Common;
using GMCore.Helper;
using GMCore.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Adani.Solution.MVC.ServiceClient
{
    public class VehicleClient : BaseClient
    {
        private const string ServiceName = "Report Client";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;

        public async Task<VehicleTrackingLoginResponseDto> GetToken()
        {
            VehicleTrackingLoginResponseDto tokenData = new VehicleTrackingLoginResponseDto();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var loginAPI = ConfigHelper.VehicleTrackStatusLoginAPI;

                    VehicleTrackingLoginDto body = new VehicleTrackingLoginDto
                    {
                        username = ConfigHelper.VehicleStatusUserName,
                        password = ConfigHelper.VehicleStatusPassword
                    };
                    
                    StringContent content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(loginAPI, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var jsonContent = JObject.Parse(responseBody);
                        tokenData = JsonConvert.DeserializeObject<VehicleTrackingLoginResponseDto>(jsonContent.ToString());
                        Console.WriteLine("API Response: " + responseBody);
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                    }
                }

                return tokenData;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return tokenData;
            }
        }

        public async Task<VehicleTrackingDto> GetVehicleTrackinStatusData(string token,string CustomerCodeList,string DoNumberList)
        {
            VehicleTrackingDto data = new VehicleTrackingDto();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var apiUrl = ConfigHelper.VehicleStatusDataAPI + "?customer_code=" + CustomerCodeList + "&do_numbers=" + DoNumberList + "&is_includes_all_dos=True";
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("token", token);
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        var jsonContent = JObject.Parse(responseBody);
                        data = JsonConvert.DeserializeObject<VehicleTrackingDto>(jsonContent.ToString());
                    }
                    else
                    {
                        Console.WriteLine("Error: " + response.StatusCode);
                    }
                }

                return data;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return data;
            }
        }

        public async Task<List<DONumberddlDto>> GetDONumberListByDistributorId(string selectedIds)
        {
            var IdList = selectedIds?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)?.ToList();

            var result = new List<DONumberddlDto>();
            _methodName = "GetDONumberListByDistributorId";

            try
            {
                var inputDtoJson = JsonHelper.ConvertObjectToJson(IdList);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetDONumber, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                {
                    var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                    var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                    result = JsonConvert.DeserializeObject<List<DONumberddlDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                }
                return result;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return result;
            }
        }

        public async Task<List<TrackSkuOutputDto>> GetSkuDataWithLiftingandDoNumber(LiftingSkuInputDto inputDto)
        {
            _methodName = "GetSkuDataWithLiftingandDoNumber";
            string apiUrl = ApiUrl.WebApiUrlGetSkuData;
            var response = await GetListAsync<TrackSkuOutputDto>(apiUrl, inputDto);
            return response.ToList();
        }

        protected new async Task<IList<T>> GetListAsync<T>(string apiUrl, object inputDto) where T : class
        {
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(apiUrl, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode 
                    && !string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                {
                    var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                    var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                    List<T> outPutDto = JsonConvert.DeserializeObject<List<T>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                    return outPutDto;
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }

            return new List<T>();
        }
    }
}