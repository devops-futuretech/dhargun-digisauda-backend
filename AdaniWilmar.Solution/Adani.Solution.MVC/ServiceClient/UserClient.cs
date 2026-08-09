using GMCore.Helper;
using GMCore.Logger;
using Adani.Solution.DTO;
using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Adani.Solution.DTO.Enums;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Adani.Solution.MVC.ServiceClient
{
    public class UserClient : BaseClient
    {
        private const string ServiceName = "User Client";
        //private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;


        #region Login
        /// <summary>
        /// Method to check the user is valid or not
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        public async Task<LoginViewModel> ValidateUserAsync(LoginViewModel loginViewModel)
        {
            _methodName = "ValidateUserAsync";
            try
            {
                //_logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var webTokenResults = await Verify();
                if (webTokenResults.IsSuccess && !string.IsNullOrEmpty(webTokenResults.ErrorDto.Message))
                {
                    var mobileNumber = string.Empty;
                    var email = string.Empty;
                    var isNumber = long.TryParse(loginViewModel.Username, out long n);
                    if (isNumber)
                        mobileNumber = loginViewModel.Username;
                    else
                        email = loginViewModel.Username;

                    var apiUrl = ApiUrl.WebApiUrlPostValidateUser;
                    var inputDto = new AuthorizeInputDto { MobileNumber = mobileNumber, Password = loginViewModel.Password, Email = email, IsRequestFromWeb = true, VerticalId = loginViewModel.VerticalId };
                    var inputDtoJson = JsonHelper.ConvertObjectToJson<AuthorizeInputDto>(inputDto);
                    var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                    HttpResponseMessage response = PostAsync(apiUrl, inputSring, webTokenResults.ErrorDto.Message);
                    string responseData = await response.Content.ReadAsStringAsync();

                    responseData = UtilityHelper.TrimStartEnd(responseData);
                    var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                    if (response.IsSuccessStatusCode)
                    {
                        if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                            var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                            var loginDtoResult = JsonConvert.DeserializeObject<AuthorizeOutputDto>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
                            if (loginDtoResult != null)
                            {
                                loginViewModel.Authenticate.ProfileName = loginDtoResult.ProfileName;
                                loginViewModel.Authenticate.UserId = loginDtoResult.UserId;
                                loginViewModel.Authenticate.RoleId = loginDtoResult.RoleId;
                                loginViewModel.Authenticate.RoleTypeId = loginDtoResult.RoleTypeId;
                                loginViewModel.Authenticate.Name = loginDtoResult.Name;
                                loginViewModel.Authenticate.LoginToken = ja[0][Settings.ResponseWebToken].ToString();
                                loginViewModel.Authenticate.VerticalId = loginDtoResult.VerticalId;
                                loginViewModel.Authenticate.HeadquartersId = loginDtoResult.HeadquartersId;
                                loginViewModel.Authenticate.OrganizationReportingToId = loginDtoResult.OrganizationReportingToId;
                                loginViewModel.Authenticate.ProfilePath = loginDtoResult.ProfilePath;
                                loginViewModel.PostStatus = true;
                                return loginViewModel;
                            }
                        }
                        if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                            var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                            loginViewModel.PostStatus = false;
                            loginViewModel.PostMessage = errorDtoResult.Message;
                        }
                    }
                    else
                    {
                        loginViewModel.PostStatus = false;
                        loginViewModel.PostMessage = ja[0][Settings.ResponseMessage].ToString();
                    }

                }
                else
                {
                    loginViewModel.PostStatus = false;
                    loginViewModel.PostMessage = webTokenResults.ErrorDto.Message;
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                loginViewModel.PostStatus = false;
                loginViewModel.PostMessage = Helper.GetResourceString("msg_LoginError");
                // _logger.Error(message);
            }
            return loginViewModel;
        }
        #endregion



        #region Forgot Password
        /// <summary>
        /// Method to change the password
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ForgotPasswordViewModel> ChangePasswordAsync(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            _methodName = "ChangePasswordAsync";
            try
            {
                // _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var webTokenResults = await Verify();
                if (webTokenResults.IsSuccess && !string.IsNullOrEmpty(webTokenResults.ErrorDto.Message))
                {
                    forgotPasswordViewModel.NewPassword = forgotPasswordViewModel.Password;
                    var inputDto = new ForgotPasswordDto { UserName = forgotPasswordViewModel.Username, VerticalId = forgotPasswordViewModel.VerticalId };
                    var inputDtoJson = JsonHelper.ConvertObjectToJson<ForgotPasswordDto>(inputDto);
                    var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                    HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostChangePasswordOtpSent, inputSring, webTokenResults.ErrorDto.Message);
                    var responseData = await response.Content.ReadAsStringAsync();
                    responseData = UtilityHelper.TrimStartEnd(responseData);
                    var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                    if (response.IsSuccessStatusCode)
                    {
                        if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                            var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                            forgotPasswordViewModel.PostStatus = true;
                            forgotPasswordViewModel.UserId = UtilityHelper.LongTryToParse(jarray[0][Settings.Response].ToString());
                        }
                        if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                            var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                            forgotPasswordViewModel.PostStatus = false;
                            forgotPasswordViewModel.PostMessage = errorDtoResult.Message;
                        }

                    }
                    else
                    {
                        forgotPasswordViewModel.PostStatus = false;
                        forgotPasswordViewModel.PostMessage = ja[0][Settings.ResponseMessage].ToString();
                    }
                }
                else
                {
                    forgotPasswordViewModel.IsSuccess = false;
                    forgotPasswordViewModel.Message = webTokenResults.ErrorDto.Message;
                }


            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                forgotPasswordViewModel.PostStatus = false;
                forgotPasswordViewModel.PostMessage = Helper.GetResourceString("msg_ChangePasswordError");
                //_logger.Error(message);
            }
            return forgotPasswordViewModel;
        }

        /// <summary>
        /// Method to change the password
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ForgotPasswordViewModel> ChangePasswordOtpVerificationAsync(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            _methodName = "ChangePasswordOtpVerificationAsync";
            try
            {
                //_logger.Info($"{ServiceName} Controller-Method {_methodName}");
                forgotPasswordViewModel.NewPassword = forgotPasswordViewModel.Password;
                var webTokenResults = await Verify();
                if (webTokenResults.IsSuccess && !string.IsNullOrEmpty(webTokenResults.ErrorDto.Message))
                {
                    var inputDtoJson = JsonHelper.ConvertObjectToJson<ForgotPasswordViewModel>(forgotPasswordViewModel);
                    var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                    HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostChangePasswordOtpVerification, inputSring, webTokenResults.ErrorDto.Message);
                    var responseData = await response.Content.ReadAsStringAsync();
                    responseData = UtilityHelper.TrimStartEnd(responseData);
                    var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                    if (response.IsSuccessStatusCode)
                    {
                        if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                            var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                            forgotPasswordViewModel.PostStatus = true;
                            forgotPasswordViewModel.PostMessage = Helper.GetResourceString("msg_ChangePasswordSuccess");
                        }
                        if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                            var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                            forgotPasswordViewModel.PostStatus = false;
                            forgotPasswordViewModel.PostMessage = errorDtoResult.Message;
                        }

                    }
                    else
                    {
                        forgotPasswordViewModel.PostStatus = false;
                        forgotPasswordViewModel.PostMessage = ja[0][Settings.ResponseMessage].ToString();
                    }
                }
                else
                {
                    forgotPasswordViewModel.IsSuccess = false;
                    forgotPasswordViewModel.Message = webTokenResults.ErrorDto.Message;
                }


            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                forgotPasswordViewModel.PostStatus = false;
                forgotPasswordViewModel.PostMessage = Helper.GetResourceString("msg_ChangePasswordError");
                //_logger.Error(message);
            }
            return forgotPasswordViewModel;
        }


        /// <summary>
        /// Method to resend otp to user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ResultDto> ResendOtpAsync(long userId)
        {
            _methodName = "ResendOtpAsync";
            var result = new ResultDto();
            try
            {

                //_logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var webTokenResults = await Verify();
                if (webTokenResults.IsSuccess && !string.IsNullOrEmpty(webTokenResults.ErrorDto.Message))
                {
                    var inputDto = new UserIdDto()
                    {
                        UserId = userId
                    };
                    var inputDtoJson = JsonHelper.ConvertObjectToJson<UserIdDto>(inputDto);
                    var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                    HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostResendOtp, inputSring, webTokenResults.ErrorDto.Message);
                    var responseData = await response.Content.ReadAsStringAsync();
                    responseData = UtilityHelper.TrimStartEnd(responseData);
                    var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                    if (response.IsSuccessStatusCode)
                    {
                        if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                            var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                            //result = JsonConvert.DeserializeObject<ResultDto>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
                            result.ErrorDto.Message = Helper.GetResourceString("msg_OtpSentSuccessfully");
                            return result;
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
                        result.ErrorDto.Message = !string.IsNullOrEmpty(responseData) ? ja[0]["message"].ToString() : string.Empty;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.ErrorDto.Message = webTokenResults.ErrorDto.Message;
                }


            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                result.IsSuccess = false;
                result.ErrorDto.Message = Helper.GetResourceString("msg_ResendOtpError");
                // _logger.Error(message);
            }
            return result;
        }

        #endregion

        /// <summary>
        /// Get all order status
        /// </summary>
        /// <returns></returns>
        public List<DropDownDto> GetAllVerticals()
        {
            _methodName = "GetAllVerticals";
            var statusList = new List<DropDownDto>();
            try
            {
                foreach (var unitDetailsItem in Helper.EnumToList<Division>())
                {
                    var unitItem = new DropDownDto
                    {
                        Name = Helper.GetEnumDescription(unitDetailsItem),
                        Id = (int)unitDetailsItem
                    };
                    statusList.Add(unitItem);
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                
            }
            return statusList.Any() ? statusList.OrderBy(x => x.Id).ToList() : statusList;
        }

        public async Task<IList<DropDownDto>> GetVerticalListBasedOnUserAsync(string username)
        {
            try
            {
                _methodName = "GetVerticalListBasedOnUserAsync";
                var mobileNumber = string.Empty;
                var email = string.Empty;
                var isNumber = long.TryParse(username, out long n);
                if (isNumber)
                    mobileNumber = username;
                else
                    email = username;

                var inputDto = new AuthorizeInputDto {
                    MobileNumber = mobileNumber,
                    Email = email,
                    IsRequestFromWeb = true
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<AuthorizeInputDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetVerticalListBasedonUser, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<DropDownDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                
            }
            return new List<DropDownDto>();
        }

    }
}