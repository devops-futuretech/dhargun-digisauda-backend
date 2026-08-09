using GMCore.Helper;
using GMCore.Logger;
using Adani.Solution.DTO;
using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Adani.Solution.DTO.Common;
using Adani.Solution.DTO.Enums;

namespace Adani.Solution.MVC.ServiceClient
{
    public class TargetClient : BaseClient
    {
        private const string ServiceName = "Target Client";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;

        public async Task<List<UserTargetDetailDto>> GetMonthAndYearByFinancialYear(FinancialYearIdDto inputdto)
        {
            var result = new List<UserTargetDetailDto>();
            _methodName = "GetMonthAndYearByFinancialYear";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputdto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetMonthAndYearByFinancialYear, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<UserTargetDetailDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        #region UserCustomerSalesTarget

        public async Task<UserCustomerSalesTargetDto> AddUserSalesTarget(UserCustomerSalesTargetDto inputDto)
        {
            _methodName = "AddUserCustomerSalesTarget";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var inputDtoJson = string.Empty;
                inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = inputDto.Id == 0 ? PostAsync(ApiUrl.WebApiUrlPostAddUserSalesTarget, inputSring) : PostAsync(ApiUrl.WebApiUrlPostUpdateUserSalesTarget, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var successMessage = inputDto.Id == 0 ? Helper.GetResourceString("msg_UserCustomerSalesTargetSaveSuccess") : Helper.GetResourceString("msg_UserCustomerSalesTargetUpdateSuccess");
                        inputDto.PostStatus = true;
                        inputDto.PostMessage = successMessage;
                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        inputDto.PostStatus = false;
                        inputDto.PostMessage = errorDtoResult.Message;
                    }
                }
                else
                {
                    inputDto.PostStatus = false;
                    inputDto.PostMessage = ja[0]["message"].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                inputDto.PostStatus = false;
                inputDto.PostMessage = Helper.GetResourceString("msg_UserCustomerSalesTargetError");
                _logger.Error(message);
            }
            return inputDto;
        }

        public async Task<List<UserCustomerSalesTargetDto>> UserSalesTargetList(LoginUserIdDto inputDto)
        {
            var result = new List<UserCustomerSalesTargetDto>();
            _methodName = "UserCustomerSalesTargetList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetUserSalesTargetList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<UserCustomerSalesTargetDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<List<UserTargetDetailDto>> UserSalesTargetDetailList(long userid, int financialyearid, int oiltypeId)
        {
            var result = new List<UserTargetDetailDto>();
            _methodName = "UserCustomerSalesTargetDetail";
            try
            {
                var inputdto = new UserTargetIdDto
                {
                    FinancialYearId = financialyearid,
                    AssignedToUserId = userid,
                    OilTypeId = oiltypeId
                };
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputdto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetUserSalesTargetDetailList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<UserTargetDetailDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<UserCustomerSalesTargetDto> GetUserSalesTargetdetailbyId(UserTargetIdDto idInputDto)
        {
            var result = new UserCustomerSalesTargetDto();
            _methodName = "UserCustomerSalesTargetdetailbyId";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(idInputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostGetUserSalesTargetdetailbyId, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<UserCustomerSalesTargetDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<UserCustomerSalesTargetDto> SaveUserSalesTargetList(List<MapSalesTargetDetailDto> mapSalesTargetDetailDtoList)
        {
            var resultDto = new UserCustomerSalesTargetDto();
            _methodName = "SaveUserCustomerSalesTargetList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var inputDtoJson = string.Empty;
                inputDtoJson = JsonHelper.ConvertObjectToJson(mapSalesTargetDetailDtoList);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostSaveUserSalesTargetList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        resultDto = JsonConvert.DeserializeObject<UserCustomerSalesTargetDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        //var successMessage = Helper.GetResourceString("msg_UserCustomerSalesTargetSaveSuccess");
                        resultDto.PostStatus = true;
                        resultDto.PostMessage = Helper.GetResourceString("msg_UserCustomerSalesTargetSaveSuccess");
                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        resultDto.PostStatus = false;
                        resultDto.PostMessage = errorDtoResult.Message;
                    }
                }
                else
                {
                    resultDto.PostStatus = false;
                    resultDto.PostMessage = ja[0]["message"].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                resultDto.PostStatus = false;
                resultDto.PostMessage = Helper.GetResourceString("msg_UserCustomerSalesTargetError");
                _logger.Error(message);
            }
            return resultDto;
        }

        public async Task<List<UserCustomerSalesTargetDto>> GetAssignedSalesTargetListAsync(LoginUserIdDto inputDto)
        {
            var result = new List<UserCustomerSalesTargetDto>();
            _methodName = "GetAssignedSalesTargetListAsync";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetAssignedSalesTargetList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<UserCustomerSalesTargetDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<List<UserCustomerSalesTargetDto>> GetSalesTargetOilTypeListAsync(long userid, int financialyearid)
        {
            var result = new List<UserCustomerSalesTargetDto>();
            _methodName = "GetSalesTargetOilTypeListAsync";
            try
            {
                var inputdto = new UserTargetIdDto
                {
                    FinancialYearId = financialyearid,
                    AssignedToUserId = userid
                };
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputdto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSalesTargetOilTypeList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<UserCustomerSalesTargetDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<UserCustomerSalesTargetDto> UpdateUserSalesTarget(UserCustomerSalesTargetDto inputDto)
        {
            _methodName = "UpdateUserSalesTarget";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var inputDtoJson = string.Empty;
                inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostUpdateUserSalesTarget, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var successMessage = inputDto.Id == 0 ? Helper.GetResourceString("msg_UserCustomerSalesTargetSaveSuccess") : Helper.GetResourceString("msg_UserCustomerSalesTargetUpdateSuccess");
                        inputDto.PostStatus = true;
                        inputDto.PostMessage = successMessage;
                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        inputDto.PostStatus = false;
                        inputDto.PostMessage = errorDtoResult.Message;
                    }
                }
                else
                {
                    inputDto.PostStatus = false;
                    inputDto.PostMessage = ja[0]["message"].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                inputDto.PostStatus = false;
                inputDto.PostMessage = Helper.GetResourceString("msg_UserCustomerSalesTargetError");
                _logger.Error(message);
            }
            return inputDto;
        }

        public async Task<List<DropDownDto>> GetOilTypesBasedOnAssignedSalesTarget(UserTargetIdDto inputdto)
        {
            var result = new List<DropDownDto>();
            _methodName = "GetOilTypesBasedOnAssignedSalesTarget";
            try
            {
              
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputdto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetOilTypesBasedOnAssignedSalesTarget, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<DropDownDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        #endregion

        #region UserCustomerSaudaTarget

        public async Task<UserCustomerSaudaTargetDto> AddUserCustomerSaudaTarget(UserCustomerSaudaTargetDto inputDto)
        {
            _methodName = "AddUserCustomerSaudaTarget";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var inputDtoJson = string.Empty;
                inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = inputDto.Id == 0 ? PostAsync(ApiUrl.WebApiUrlPostAddUserCustomerSaudaTarget, inputSring) : PostAsync(ApiUrl.WebApiUrlPostUpdateUserCustomerSaudaTarget, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var successMessage = inputDto.Id == 0 ? Helper.GetResourceString("msg_UserCustomerSaudaTargetSaveSuccess") : Helper.GetResourceString("msg_UserCustomerCustomerSaudaTargetUpdateSuccess");
                        inputDto.PostStatus = true;
                        inputDto.PostMessage = successMessage;
                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        inputDto.PostStatus = false;
                        inputDto.PostMessage = errorDtoResult.Message;
                    }
                }
                else
                {
                    inputDto.PostStatus = false;
                    inputDto.PostMessage = ja[0]["message"].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                inputDto.PostStatus = false;
                inputDto.PostMessage = Helper.GetResourceString("msg_UserCustomerSaudaTargetError");
                _logger.Error(message);
            }
            return inputDto;
        }

        public async Task<List<UserCustomerSaudaTargetDto>> UserCustomerSaudaTargetList(LoginUserIdDto inputDto)
        {
            var result = new List<UserCustomerSaudaTargetDto>();
            _methodName = "UserCustomerSaudaTargetList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetUserCustomerSaudaTargetList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<UserCustomerSaudaTargetDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<List<UserTargetDetailDto>> UserCustomerSaudaTargetDetailList(long userid, int financialyearid, int oiltypeId)
        {
            var result = new List<UserTargetDetailDto>();
            _methodName = "UserCustomerSaudaTargetDetail";
            try
            {
                var inputdto = new UserTargetIdDto
                {
                    FinancialYearId = financialyearid,
                    AssignedToUserId = userid,
                    OilTypeId = oiltypeId
                };
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputdto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetUserCustomerSaudaTargetDetailList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<UserTargetDetailDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<UserCustomerSaudaTargetDto> GetUserCustomerSaudaTargetdetailbyId(UserTargetIdDto idInputDto)
        {
            var result = new UserCustomerSaudaTargetDto();
            _methodName = "UserCustomerSaudaTargetdetailbyId";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(idInputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostGetUserCustomerSaudaTargetdetailbyId, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<UserCustomerSaudaTargetDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<UserCustomerSaudaTargetDto> SaveUserCustomerSaudaTargetList(List<MapSaudaTargetDetailDto> mapCustomerSaudaTargetDetailDtoList)
        {
            var resultDto = new UserCustomerSaudaTargetDto();
            _methodName = "SaveUserCustomerSaudaTargetList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var inputDtoJson = string.Empty;
                inputDtoJson = JsonHelper.ConvertObjectToJson(mapCustomerSaudaTargetDetailDtoList);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostSaveUserCustomerSaudaTargetList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        resultDto = JsonConvert.DeserializeObject<UserCustomerSaudaTargetDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        //var successMessage = Helper.GetResourceString("msg_UserCustomerCustomerSaudaTargetSaveSuccess");
                        resultDto.PostStatus = true;
                        resultDto.PostMessage = Helper.GetResourceString("msg_UserCustomerSaudaTargetSaveSuccess");
                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        resultDto.PostStatus = false;
                        resultDto.PostMessage = errorDtoResult.Message;
                    }
                }
                else
                {
                    resultDto.PostStatus = false;
                    resultDto.PostMessage = ja[0]["message"].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                resultDto.PostStatus = false;
                resultDto.PostMessage = Helper.GetResourceString("msg_UserCustomerSaudaTargetError");
                _logger.Error(message);
            }
            return resultDto;
        }

        public async Task<List<UserCustomerSaudaTargetDto>> GetAssignedSaudaTargetListAsync(LoginUserIdDto inputDto)
        {
            var result = new List<UserCustomerSaudaTargetDto>();
            _methodName = "GetAssignedSaudaTargetListAsync";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetAssignedSaudaTargetList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<UserCustomerSaudaTargetDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<List<UserCustomerSaudaTargetDto>> GetSaudaTargetOilTypeListAsync(long userid, int financialyearid)
        {
            var result = new List<UserCustomerSaudaTargetDto>();
            _methodName = "GetSaudaTargetOilTypeListAsync";
            try
            {
                var inputdto = new UserTargetIdDto
                {
                    FinancialYearId = financialyearid,
                    AssignedToUserId = userid
                };
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputdto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSaudaTargetOilTypeList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<UserCustomerSaudaTargetDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<UserCustomerSaudaTargetDto> UpdateUserCustomerSaudaTarget(UserCustomerSaudaTargetDto inputDto)
        {
            _methodName = "UpdateUserCustomerSaudaTarget";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var inputDtoJson = string.Empty;
                inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostUpdateUserCustomerSaudaTarget, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var successMessage = inputDto.Id == 0 ? Helper.GetResourceString("msg_UserCustomerSaudaTargetSaveSuccess") : Helper.GetResourceString("msg_UserCustomerCustomerSaudaTargetUpdateSuccess");
                        inputDto.PostStatus = true;
                        inputDto.PostMessage = successMessage;
                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        inputDto.PostStatus = false;
                        inputDto.PostMessage = errorDtoResult.Message;
                    }
                }
                else
                {
                    inputDto.PostStatus = false;
                    inputDto.PostMessage = ja[0]["message"].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                inputDto.PostStatus = false;
                inputDto.PostMessage = Helper.GetResourceString("msg_UserCustomerSaudaTargetError");
                _logger.Error(message);
            }
            return inputDto;
        }

        public async Task<List<DropDownDto>> GetOilTypesBasedOnAssignedSaudaTarget(UserTargetIdDto inputdto)
        {
            var result = new List<DropDownDto>();
            _methodName = "GetOilTypesBasedOnAssignedSaudaTarget";
            try
            {

                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputdto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetOilTypesBasedOnAssignedSaudaTarget, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<DropDownDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        #endregion

        #region User Customer Target
        public async Task<UserCustomerTargetDto> SaveUserCustomerTarget(List<MapSalesTargetDetailDto> mapSalesTargetDetailDtoList)
        {
            var resultDto = new UserCustomerTargetDto();
            _methodName = "SaveUserCustomerTarget";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var list = new MapSalesTargetDetailDto();
                list = mapSalesTargetDetailDtoList.FirstOrDefault();
                string url = string.Empty;
                if(list != null)
                {
                    if (list.Id > 0)
                    {
                        url = ApiUrl.WebApiUrlPostUpdateUserCustomerTargetList;
                    }
                    else
                    {
                        url = ApiUrl.WebApiUrlPostSaveUserCustomerTargetList;
                    }
                }
                var inputDtoJson = string.Empty;
                inputDtoJson = JsonHelper.ConvertObjectToJson(mapSalesTargetDetailDtoList);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(url, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        resultDto = JsonConvert.DeserializeObject<UserCustomerTargetDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        var successMessage = Helper.GetResourceString("msg_UserCustomerTargetSaveSuccess");
                        resultDto.PostStatus = true;
                        resultDto.PostMessage = successMessage;
                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        resultDto.PostStatus = false;
                        resultDto.PostMessage = errorDtoResult.Message;
                    }
                }
                else
                {
                    resultDto.PostStatus = false;
                    resultDto.PostMessage = ja[0]["message"].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                resultDto.PostStatus = false;
                resultDto.PostMessage = Helper.GetResourceString("msg_UserCustomerSalesTargetError");
                _logger.Error(message);
            }
            return resultDto;
        }
        public async Task<List<UserCustomerSalesTargetDto>> UserTargetList(LoginUserIdDto inputDto)
        {
            var result = new List<UserCustomerSalesTargetDto>();
            _methodName = "UserTargetList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetUserTargetLists, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<UserCustomerSalesTargetDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<List<UserTargetDetailDto>> UserTargetDetailList(long userid, int financialyearid)
        {
            var result = new List<UserTargetDetailDto>();
            _methodName = "UserTargetDetailList";
            try
            {
                var inputdto = new UserTargetIdDto
                {
                    FinancialYearId = financialyearid,
                    AssignedToUserId = userid
                };
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputdto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetUserTargetDetailList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<UserTargetDetailDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        public async Task<List<UserCustomerSalesTargetDto>> GetAssignedTargetListAsync(LoginUserIdDto inputDto)
        {
            var result = new List<UserCustomerSalesTargetDto>();
            _methodName = "GetAssignedSalesTargetListAsync";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetAssignedTargetList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<UserCustomerSalesTargetDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        public async Task<UserCustomerTargetDto> GetUserTargetdetailbyId(UserTargetIdDto idInputDto)
        {
            var result = new UserCustomerTargetDto();
            _methodName = "GetUserTargetdetailbyId";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(idInputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostGetUserTargetdetailbyId, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<UserCustomerTargetDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        #endregion
    }
}