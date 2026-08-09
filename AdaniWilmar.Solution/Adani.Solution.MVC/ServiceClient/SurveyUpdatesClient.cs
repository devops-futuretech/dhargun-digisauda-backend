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
using Kendo.Mvc.UI;

namespace Adani.Solution.MVC.ServiceClient
{
    public class SurveyUpdatesClient : BaseClient
    {
        private const string ServiceName = "Updates Client";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;

        #region Question

        /// <summary>
        /// Method to add or update Question
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<QuestionDto> AddOrUpdateQuestion(QuestionDto inputDto)
        {
            _methodName = "AddOrUpdateQuestion";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new QuestionDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                if (inputDto.Id > 0)
                { apiUrl = ApiUrl.WebApiUrlPostUpdateQuestion; }
                else { apiUrl = ApiUrl.WebApiUrlPostSaveQuestion; }

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
                        result.PostStatus = true;
                        result.PostMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_QuestionUpdateSuccess") : Helper.GetResourceString("msg_QuestionSaveSuccess");
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
                result.PostMessage = Helper.GetResourceString("msg_QuestionError");
                _logger.Error(message);
            }
            return result;
        }

        /// <summary>
        /// Method to Get Question List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<QuestionDto>> GetQuestionListAsync(LoginUserIdDto inputDto)
        {
            try
            {
                _methodName = "GetQuestionListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetQuestionList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<QuestionDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<QuestionDto>();
        }

        /// <summary>
        /// Method to get Get Question Details By Id
        /// </summary>
        /// <param name="QuestionId"></param>
        /// <returns></returns>
        public async Task<QuestionDto> GetQuestionDetailsById(long QuestionId)
        {
            var result = new QuestionDto();
            _methodName = "GetQuestionDetailsById";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var apiUrl = ApiUrl.WebApiUrlGetQuestionDetailsById;
                if (QuestionId != 0)
                {
                    var inputDtoJson = JsonHelper.ConvertObjectToJson(QuestionId);
                    var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                    HttpResponseMessage response = PostAsync(apiUrl, inputSring);
                    var responseData = await response.Content.ReadAsStringAsync();
                    responseData = UtilityHelper.TrimStartEnd(responseData);
                    var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                    if (response.IsSuccessStatusCode)
                    {
                        if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                            var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                            result = JsonConvert.DeserializeObject<QuestionDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        /// <summary>
        /// Method to get Get Question Survey Details By Id
        /// </summary>
        /// <param name="QuestionId"></param>
        /// <returns></returns>
        public async Task<QuestionDto> GetQuestionSurveyDetailsById(long QuestionId)
        {
            var result = new QuestionDto();
            _methodName = "GetQuestionSurveyDetailsById";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var apiUrl = ApiUrl.WebApiUrlGetQuestionSurveyDetailsById;
                if (QuestionId != 0)
                {
                    var inputDtoJson = JsonHelper.ConvertObjectToJson(QuestionId);
                    var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                    HttpResponseMessage response = PostAsync(apiUrl, inputSring);
                    var responseData = await response.Content.ReadAsStringAsync();
                    responseData = UtilityHelper.TrimStartEnd(responseData);
                    var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                    if (response.IsSuccessStatusCode)
                    {
                        if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                            var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                            result = JsonConvert.DeserializeObject<QuestionDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Bulletin
        /// <summary>
        /// Method to get Bulletin List
        /// </summary>
        /// <param name="BulletinViewModel"></param>
        /// <returns></returns>
        public async Task<IList<BulletinDto>> GetBulletinListAsync([DataSourceRequest] DataSourceRequest dataSourceRequest, long[] totalRecordsFound, BulletinInputDto bulletinInputDto)
        {
            var inputDtoJson = JsonHelper.ConvertObjectToJson<BulletinInputDto>(bulletinInputDto);
            var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
            HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetAllBulletins, inputSring);
            var responseData = await response.Content.ReadAsStringAsync();
            responseData = UtilityHelper.TrimStartEnd(responseData);
            var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
            if (response.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                {
                    var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                    var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                    var resultList = JsonConvert.DeserializeObject<List<BulletinDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                    if (!resultList.Any()) return resultList;
                    var total = resultList.Count;
                    totalRecordsFound[0] = total;
                    return resultList;
                }
            }
            return new List<BulletinDto>();
        }

        /// <summary>
        /// Method to get Bulletin Details By Id
        /// </summary>
        /// <param name="BulletinDto"></param>
        /// <returns></returns>
        public async Task<BulletinDto> GetBulletinDetailsByIdAsync(int BulletinId, long loginUserId)
        {
            var result = new BulletinDto();
            _methodName = "GetBulletinDetailsByIdAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                if (BulletinId != 0)
                {
                    var BulletinInputDto = new BulletinInputDto
                    {
                        BulletinId = BulletinId,
                        LoginUserId = loginUserId
                    };

                    var inputDtoJson = JsonHelper.ConvertObjectToJson(BulletinInputDto);
                    var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                    HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetBulletinDetailById, inputSring);
                    var responseData = await response.Content.ReadAsStringAsync();
                    responseData = UtilityHelper.TrimStartEnd(responseData);
                    var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                    if (response.IsSuccessStatusCode)
                    {
                        if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                            var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                            result = JsonConvert.DeserializeObject<BulletinDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                           

                            result.PostMessage = string.Empty;
                            result.PostStatus = true;
                            return result;
                        }
                        else
                        {
                            var errorCode = ja[0]["ErrorCode"].ToString();
                            result.PostStatus = false;
                            result.PostMessage = Helper.GetResourceFor(errorCode, "msg_VerifyGetBulletinDetailsError");
                        }
                    }
                    else
                    {
                        var errorCode = ja[0]["ErrorCode"].ToString();
                        result.PostStatus = false;
                        result.PostMessage = Helper.GetResourceFor(errorCode, "msg_VerifyGetBulletinDetailsError");
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

        /// <summary>
        /// Method to add or update Question
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<BulletinDto> AddOrUpdateBulletin(BulletinDto inputDto, List<MediaDto> mediaResult)
        {
            _methodName = "AddOrUpdateQuestion";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new BulletinDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;

                //Media 
                var bulletinContentList = new List<BulletinMediaDto>();
                if (mediaResult != null)
                {
                    foreach (var item in mediaResult)
                    {
                        var bulletinContent = new BulletinMediaDto
                        {
                            MediaPath = item.FileName,
                            MediaTypeId = item.MediaTypeId
                        };
                        bulletinContentList.Add(bulletinContent);
                    }
                }
                if (bulletinContentList.Any())
                    inputDto.MediaList = bulletinContentList;

                if (inputDto.BulletinId > 0)
                { apiUrl = ApiUrl.WebApiUrlPostUpdateBulletin; }
                else { apiUrl = ApiUrl.WebApiUrlPostAddBulletin; }

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
                        result.PostStatus = true;
                        result.PostMessage = inputDto.BulletinId > 0 ? Helper.GetResourceString("msg_BulletinUpdateSuccess") : Helper.GetResourceString("msg_BulletinSaveSuccess");
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
                result.PostMessage = Helper.GetResourceString("msg_BulletinError");
                _logger.Error(message);
            }
            return result;
        }


        public async Task<BulletinDto> DeleteBulletinMediaAsync(int bulletinMediaId, long loginUserId)
        {
            var bulletinDto = new BulletinDto();
            _methodName = "DeleteBulletinMediaAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new BulletinInputDto
                {
                    BulletinMediaId = bulletinMediaId,
                    LoginUserId = loginUserId
                };
                var inputDtoJson = JsonHelper.ConvertObjectToJson<BulletinInputDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlDeleteBulletinsMedia, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));

                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var successMessage = Helper.GetResourceString("msg_DeleteMediaSuccessful");
                        bulletinDto.PostStatus = true;
                        bulletinDto.PostMessage = successMessage;
                    }
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        bulletinDto.PostStatus = false;
                        bulletinDto.PostMessage = errorDtoResult.Message;
                    }
                }
                else
                {
                    bulletinDto.PostStatus = false;
                    bulletinDto.PostMessage = ja[0]["message"].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                bulletinDto.PostStatus = false;
                bulletinDto.PostMessage = Helper.GetResourceString("msg_MediaError");
                _logger.Error(message);
            }
            return bulletinDto;

        }

        public async Task<IList<FeedbackTypeDto>> GetFeedbackTypeddl(LoginUserIdDto inputDto)
        {
            try
            {
                _methodName = "GetFeedbackTypeddl";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetFeedbackTypeddl, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<FeedbackTypeDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<FeedbackTypeDto>();
        }

        /// <summary>
        /// Method to get Bulletin List
        /// </summary>
        /// <param name="BulletinViewModel"></param>
        /// <returns></returns>
        public async Task<List<FeedbackRequestDto>> GetFeedbackListAsync(FeedbackInputDto feedbackInputDto)
        {
            var inputDtoJson = JsonHelper.ConvertObjectToJson<FeedbackInputDto>(feedbackInputDto);
            var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
            HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetAllFeedback, inputSring);
            var responseData = await response.Content.ReadAsStringAsync();
            responseData = UtilityHelper.TrimStartEnd(responseData);
            var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
            if (response.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                {
                    var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                    var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                    var resultList = JsonConvert.DeserializeObject<List<FeedbackRequestDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                    if (!resultList.Any()) return resultList;
                    return resultList;
                }
            }
            return new List<FeedbackRequestDto>();
        }
        #endregion

    }
}