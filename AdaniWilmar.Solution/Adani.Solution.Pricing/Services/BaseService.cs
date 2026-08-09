using Adani.Solution.Console.Common;
using Adani.Solution.DTO;
using Adani.Solution.Model;
using GMCore.Helper;
using GMCore.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
namespace Adani.Solution.Pricing.Services
{
    public class BaseService
    {
        private const string ServiceName = "Base Service";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;

        #region Common
        public LoginResult ValidateUserAsync(LoginResult loginResult)
        {
            _methodName = "ValidateUserAsync";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var webTokenResults = Verify();
                if (webTokenResults.IsSuccess && webTokenResults.SuccessDto != null)
                {
                    var mobileNumber = string.Empty;
                    var email = string.Empty;
                    var isNumber = long.TryParse(loginResult.Username, out long n);
                    if (isNumber)
                        mobileNumber = loginResult.Username;
                    else
                        email = loginResult.Username;

                    var apiUrl = WebConfig.WebApiUrlPostValidateUser;
                    var inputDto = new AuthorizeInputDto { MobileNumber = mobileNumber, Email = email, Password = loginResult.Password, IsRequestFromWeb = true };
                    var inputDtoJson = JsonHelper.ConvertObjectToJson<AuthorizeInputDto>(inputDto);
                    var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, WebConfig.EncryptionKey, WebConfig.VectorKey);

                    HttpResponseMessage response = PostAsync(apiUrl, inputDto, webTokenResults.SuccessDto.Response.ToString());
                    string responseData = response.Content.ReadAsStringAsync().Result;

                    responseData = UtilityHelper.TrimStartEnd(responseData);
                    //var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                    var resultdto = JsonConvert.DeserializeObject<ResultDto>(responseData);
                    if (response.IsSuccessStatusCode)
                    {

                        //if (!string.IsNullOrEmpty(ja[0][ConsoleSettings.ResponseSuccess].ToString()))
                        //{
                        //    _logger.Info($"{ServiceName} Controller-Method {_methodName} Message:ValidateUserSuccess");
                        //    var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][ConsoleSettings.ResponseSuccess].ToString(), ConsoleSettings.EncryptionKey, ConsoleSettings.VectorKey);

                        //    var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        //    var loginDtoResult = JsonConvert.DeserializeObject<AuthorizeOutputDto>(jarray[0][ConsoleSettings.Response].ToString(), UtilityHelper.GetJsonSettings());
                        loginResult.PostStatus = true;
                        loginResult.Authenticate.LoginToken = resultdto.SuccessDto.Response.ToString();


                        //}

                        //if (!string.IsNullOrEmpty(ja[0][ConsoleSettings.ResponseError].ToString()))
                        //{
                        //    var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][ConsoleSettings.ResponseError].ToString(), ConsoleSettings.EncryptionKey, ConsoleSettings.VectorKey);
                        //    var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        //    loginResult.PostStatus = false;
                        //    loginResult.PostMessage = errorDtoResult.Message;
                        //    var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {errorDtoResult.Message}";
                        //    _logger.Error(message);
                        //}
                    }
                    else
                    {
                        loginResult.PostStatus = false;
                        // loginResult.PostMessage = ja[0][ConsoleSettings.ResponseMessage].ToString();
                        var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {loginResult.PostMessage}";
                        _logger.Error(message);
                    }

                }
                else
                {
                    loginResult.PostStatus = false;
                    //loginResult.PostMessage = webTokenResults.ErrorDto.Message;
                    var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {loginResult.PostMessage}";
                    _logger.Error(message);
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                loginResult.PostStatus = false;
                loginResult.PostMessage = exception.ToString();
                _logger.Error(message);
            }
            return loginResult;
        }

        public ResultDto Verify()
        {
            var result = new ResultDto();
            _methodName = "Verify";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var apiUrl = WebConfig.WebApiUrlPostVerifyToken;
                var inputDto = new KeyInputDto()
                {
                    ClientKey = WebConfig.WebKey,
                    ClientType = WebConfig.KeyType
                };
                //var inputDtoJson = JsonHelper.ConvertObjectToJson<KeyInputDto>(inputDto);
                //var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, ConsoleSettings.EncryptionKey, ConsoleSettings.VectorKey);

                HttpResponseMessage response = PostAsync(apiUrl, inputDto);
                string responseData = response.Content.ReadAsStringAsync().Result;
                responseData = UtilityHelper.TrimStartEnd(responseData);
                _logger.Info($"{ServiceName} Controller-Method {_methodName} responseData: {responseData}");
                // var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                var resultJson = JsonConvert.DeserializeObject(responseData);
                var resultdto = JsonConvert.DeserializeObject<ResultDto>(responseData);
                if (response.IsSuccessStatusCode)
                {
                    //if (!string.IsNullOrEmpty(ja[0][ConsoleSettings.ResponseSuccess].ToString()))
                    //{
                    //    _logger.Info($"{ServiceName} Controller-Method {_methodName} Message:VerifySuccess");
                    //    var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][ConsoleSettings.ResponseSuccess].ToString(), ConsoleSettings.EncryptionKey, ConsoleSettings.VectorKey);
                    //    var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                    result.IsSuccess = true;
                    result.SuccessDto.Response = resultdto.SuccessDto.Response;
                    //}
                    //if (!string.IsNullOrEmpty(ja[0][ConsoleSettings.ResponseError].ToString()))
                    //{
                    //    var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][ConsoleSettings.ResponseError].ToString(), ConsoleSettings.EncryptionKey, ConsoleSettings.VectorKey);
                    //    var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                    //    result.IsSuccess = false;
                    //    result.ErrorDto.Message = errorDtoResult.Message;
                    //    var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {errorDtoResult.Message}";
                    //    _logger.Error(message);
                    //}
                }
                else
                {
                    result.IsSuccess = false;
                    // result.ErrorDto.Message = ja[0]["Message"].ToString();
                    var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {result.ErrorDto.Message}";
                    _logger.Error(message);
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Instantiating Dependencies Exception: {exception}";
                _logger.Error(message);
                result.IsSuccess = false;
                result.ErrorDto.Message = message;
            }

            return result;
        }

        public HttpResponseMessage PostAsync(string functionUrl, object model, string webToken)
        {
            using (var client = new HttpClient())
            {
                SetClientTimeout(client);
                if (!string.IsNullOrEmpty(webToken))
                {
                    SetBearerToken(webToken, client);
                }

                var webApiUrl = new Uri(WebConfig.ApiUrl, functionUrl);
                var responseMessage = client.PostAsJsonAsync(webApiUrl, model).Result;
                return ValidateResponseMessage(responseMessage);
            }

        }

        public HttpResponseMessage PostAsync(string functionUrl, object model)
        {
            using (var client = new HttpClient())
            {
                SetClientTimeout(client);
                //if (AccessToken != null)
                //{
                //    SetBearerToken(AccessToken, client);
                //}

                var webApiUrl = new Uri(WebConfig.ApiUrl, functionUrl);
                var responseMessage = client.PostAsJsonAsync(webApiUrl, model).Result;

                return ValidateResponseMessage(responseMessage);
            }

        }

        public HttpResponseMessage GetAsync(string functionUrl, string loginAccesstoken)
        {
            using (var client = new HttpClient())
            {
                SetClientTimeout(client);
                if (!string.IsNullOrEmpty(loginAccesstoken))
                {
                    SetBearerToken(loginAccesstoken, client);
                }

                var requestUri = new Uri(WebConfig.ApiUrl, functionUrl);
                var responseMessage = client.GetAsync(requestUri).Result;
                //RefreshAuthCookie(responseMessage);

                return ValidateResponseMessage(responseMessage);
            }
        }

        public void SetClientTimeout(HttpClient client)
        {
            client.Timeout = new TimeSpan(10, 0, 0);
        }

        public static void SetBearerToken(string accessToken, HttpClient client)
        {
            // Add the Token header in the request message
            var header = new AuthenticationHeaderValue("Bearer", accessToken);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = header;
        }

        public HttpResponseMessage ValidateResponseMessage(HttpResponseMessage responseMessage)
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                return responseMessage;
            }
            else if (responseMessage.StatusCode != HttpStatusCode.OK)
            {
                var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                var data = "{ \"$id\":\"1\",\"Message\":" + responseData + ",\"IsSuccess\":false,\"ErrorCode\":\"gmsE000\",\"Response\":\"\"}";
                responseMessage.Content = new StringContent(data, Encoding.UTF8, "application/json");
                return responseMessage;
            }
            return responseMessage;
        }

        public HttpResponseMessage PostAsyncWithBaicAuthentication(string functionUrl, object model)
        {
            var responseMessage = new HttpResponseMessage();
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(model)}");
            try
            {

                using (var httpClientHandler = new HttpClientHandler())
                {
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                    using (var client = new HttpClient(httpClientHandler))
                    {
                        _logger.Info($"Url : {functionUrl}");
                        client.DefaultRequestHeaders.Add("ContentType", "application/json");
                        SetBasicAuthenticationHeaderValue(client);
                        string json = JsonConvert.SerializeObject(model);
                        var content = new StringContent(json.ToString(), System.Text.Encoding.UTF8, "application/json");
                        var webApiUrl = new Uri(WebConfig.ApiUrl, functionUrl);                         
                        responseMessage = client.PostAsync(webApiUrl, content).Result;
                        _logger.Info($"Response : {responseMessage.ToString()}");

                    }
                }
            }
            catch (Exception e)
            {
                _logger.Info($"Exception : {e.ToString()}");
            }
            return responseMessage;
        }

        private static void SetBasicAuthenticationHeaderValue(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            string credentials = string.Empty;
            credentials = $"{WebConfig.UserNameString}:{WebConfig.PasswordString}";
            var byteArray = Encoding.ASCII.GetBytes(credentials);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }
        #endregion

        #region App to SAP Data Send

        public void GetDataAsync(string apiUrl)
        {
          
            _logger.Info($"{ServiceName} Controller-Method GetDataAsync");
            var sapDataSyncResultDto = new SapDataSyncResultDto();
            sapDataSyncResultDto.SyncStartedDateTime = DateTime.Now;           
            try
            {
                LoginResult loginResult = ValidateUserAsync(new LoginResult());
                if (loginResult.PostStatus)
                {
                    _logger.Info($"{ServiceName} Controller-Method {_methodName} Message:Login Success");
                    HttpResponseMessage response = GetAsync(apiUrl, loginResult.Authenticate.LoginToken);
                    var responseData = response.Content.ReadAsStringAsync().Result;
                    responseData = UtilityHelper.TrimStartEnd(responseData);
                    var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                    if (response.IsSuccessStatusCode)
                    {
                        //if (!string.IsNullOrEmpty(ja[0][ConsoleSettings.ResponseSuccess].ToString()))
                        //{
                        //    _logger.Info($"{ServiceName} Controller-Method {_methodName} Message:Get Data From API Success Data: {ja[0][ConsoleSettings.ResponseSuccess].ToString()}");
                        //    var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][ConsoleSettings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        //    _logger.Info($"{ServiceName} Controller-Method {_methodName} SyncFolder {syncFolder} Message: DecryptedString Success Data: DecryptedString: {decryptedString} ");
                        //    //GenerateCsvDataAsync(decryptedString, syncFolder, sapDataSyncResultDto, subject, csvFileName);
                        //}
                    }
                }
                else
                {
                    //SendNotification(subject, loginResult.PostMessage, syncFolder, null, null, true);
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception:{exception}";
                _logger.Error(message);
                sapDataSyncResultDto.SyncCompletedDateTime = DateTime.Now;
                //SendNotification(subject, message, syncFolder, sapDataSyncResultDto, null, true);
            }
        }
        #endregion
    }
}
