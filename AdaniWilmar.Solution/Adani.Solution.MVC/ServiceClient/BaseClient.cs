using GMCore.Helper;
using GMCore.Logger;
using Adani.Solution.DTO;
using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Controllers;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Linq;
using Kendo.Mvc.UI;

namespace Adani.Solution.MVC.ServiceClient
{
    public class BaseClient : BaseController
    {
        public BaseController ControllerDelegate { get; set; }

        private const string ServiceName = "Base Client";
        private readonly ILogger _logger = Logging.GetLogger("BaseClient");
        private string _methodName;

        /// <summary>
        /// Method to verify token
        /// </summary>
        /// <returns></returns>
        public async Task<ResultDto> Verify()
        {
            var result = new ResultDto();
            _methodName = "Verify";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var apiUrl = ApiUrl.WebApiUrlPostVerifyToken;
                var inputDto = new KeyInputDto()
                {
                    ClientKey = ConfigHelper.WebKey,
                    ClientType = Settings.KeyType
                };
                var inputDtoJson = JsonHelper.ConvertObjectToJson<KeyInputDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                HttpResponseMessage response = PostAsync(apiUrl, inputSring);
                string responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result.IsSuccess = true;
                        result.ErrorDto.Message = jarray[0][Settings.Response].ToString();
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
                    result.ErrorDto.Message = ja[0]["Message"].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                result.IsSuccess = false;
                result.ErrorDto.Message = Helper.GetResourceFor("msg_GetWebKeyError");
                _logger.Error(message);
            }

            return result;
        }

        /// <summary>
        /// Method to get country list
        /// </summary>       
        /// <returns></returns>
        public async Task<List<CountryDto>> GetCountryListAsync()
        {
            var result = new List<CountryDto>();
            _methodName = "GetCountryListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetCountryList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<CountryDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get district list
        /// </summary>       
        /// <returns></returns>
        public async Task<List<DistrictDto>> GetDistrictListAsync(int stateId)
        {
            var result = new List<DistrictDto>();
            _methodName = "GetDistrictListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new DistrictInputDto
                {
                    StateId = stateId
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<DistrictInputDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetDistrictList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<DistrictDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get role list
        /// </summary>       
        /// <returns></returns>
        public async Task<List<RoleDto>> GetRoleListAsync()
        {
            var result = new List<RoleDto>();
            _methodName = "GetRoleListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetRoleList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<List<RoleDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        result.AddRange(responseResult.ToList());


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
        /// Method to get region list
        /// </summary>       
        /// <returns></returns>
        public async Task<List<RegionDto>> GetRegionListAsync()
        {
            var result = new List<RegionDto>();
            _methodName = "GetRegionListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetRegionList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<List<RegionDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        result.AddRange(responseResult.ToList());


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
        /// Method to save media file
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public ResultViewModel SaveMediaFile(IEnumerable<HttpPostedFileBase> files, string fileName, string folderName, IEnumerable<HttpPostedFileBase> video = null)
        {
            var result = new ResultViewModel();
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        fileName = files != null && files.Any() ? Guid.NewGuid().ToString() : string.Empty;
                    }
                    if (file == null || file.ContentLength <= 0) continue;

                    var fileSize = Math.Round((((decimal)file.ContentLength / (decimal)1024) / (decimal)1024), 1);

                    if (!file.ContentType.Contains(Settings.ImageFileContains))
                    {
                        result.IsSuccess = false;
                        result.ErrorDto.Message = Helper.GetResourceString("msg_ImageFileOnlyAccepted");
                        return result;
                    }

                    if (fileSize > Settings.ImageFileSize)
                    {
                        result.IsSuccess = false;
                        result.ErrorDto.Message = string.Format(Helper.GetResourceString("msg_ImageFileSizeExceed"), Settings.ImageFileSize);
                        return result;
                    }

                    var directory = Path.Combine(ControllerDelegate.Server.MapPath(ConfigurationManager.AppSettings["UploadMediaPath"]), folderName);
                    if (!Directory.Exists(directory))
                    {
                        result.IsSuccess = false;
                        result.ErrorDto.Message = Helper.GetResourceString("msg_FileDirectoryDoesNotExist");
                        return result;
                    }

                    var fileFullPath = Path.Combine(ControllerDelegate.Server.MapPath(ConfigHelper.UploadMediaPath), folderName, string.Concat(fileName, ConfigHelper.ImageExtension));
                    file.SaveAs(fileFullPath);

                    if (ConfigHelper.IsThumnailImageCreation)
                    {
                        var thumbFileFullPath = Path.Combine(ControllerDelegate.Server.MapPath(ConfigHelper.UploadMediaPath), folderName, string.Concat(fileName, Settings.ThumbString, ConfigHelper.ImageExtension));
                        var image = Image.FromFile(fileFullPath);
                        var thumb = image.GetThumbnailImage(Settings.ThumbnailWidth, Settings.ThumbnailHeight, () => false, IntPtr.Zero);
                        thumb.Save(thumbFileFullPath);
                        thumb.Dispose();
                        image.Dispose();
                    }

                    result.IsSuccess = true;
                    result.ImageFileList.Add(string.Concat(fileName, ConfigHelper.ImageExtension));
                    fileName = string.Empty;
                }

            }
            if (video != null)
            {
                foreach (var file in video)
                {
                    if (file == null || file.ContentLength <= 0) continue;

                    var fileSize = Math.Round((((decimal)file.ContentLength / (decimal)1024) / (decimal)1024), 1);

                    if (!file.ContentType.Contains(Settings.VideoFileContains))
                    {
                        result.IsSuccess = false;
                        result.ErrorDto.Message = Helper.GetResourceString("msg_VideoFileOnlyAccepted");
                        return result;
                    }

                    if (fileSize > Settings.VideoFileSize)
                    {
                        result.IsSuccess = false;
                        result.ErrorDto.Message = string.Format(Helper.GetResourceString("msg_VideoFileSizeExceed"), Settings.VideoFileSize);
                        return result;
                    }

                    var directory = Path.Combine(ControllerDelegate.Server.MapPath(ConfigurationManager.AppSettings["UploadMediaPath"]), folderName);
                    if (!Directory.Exists(directory))
                    {
                        result.IsSuccess = false;
                        result.ErrorDto.Message = Helper.GetResourceString("msg_FileDirectoryDoesNotExist");
                        return result;
                    }

                    var fileFullPath = Path.Combine(ControllerDelegate.Server.MapPath(ConfigHelper.UploadMediaPath), folderName, string.Concat(fileName, ""));
                    file.SaveAs(fileFullPath);

                    //if (ConfigHelper.IsThumnailImageCreation)
                    //{
                    //    var thumbFileFullPath = Path.Combine(ControllerDelegate.Server.MapPath(ConfigHelper.UploadMediaPath), folderName, string.Concat(fileName, Settings.ThumbString, ConfigHelper.ImageExtension));
                    //    var image = Image.FromFile(fileFullPath);
                    //    var thumb = image.GetThumbnailImage(Settings.ThumbnailWidth, Settings.ThumbnailHeight, () => false, IntPtr.Zero);
                    //    thumb.Save(thumbFileFullPath);
                    //    thumb.Dispose();
                    //    image.Dispose();
                    //}

                    result.IsSuccess = true;
                }
            }
            return result;
        }


        /// <summary>
        /// Method to delete file
        /// </summary>       
        /// <returns></returns>
        public void DeleteFile(string fileName, string folderName)
        {
            _methodName = "DeleteFile";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var filePath = Path.Combine(ControllerDelegate.Server.MapPath(ConfigHelper.UploadMediaPath), folderName, fileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                var thumbFilePath = Path.Combine(ControllerDelegate.Server.MapPath(ConfigHelper.UploadMediaPath), folderName, fileName.Replace(ConfigHelper.ImageExtension, string.Concat(Settings.ThumbString, ConfigHelper.ImageExtension)));
                if (System.IO.File.Exists(thumbFilePath))
                {
                    System.IO.File.Delete(thumbFilePath);
                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }

        }


        /// <summary>
        /// Method to get role type claims
        /// </summary>       
        /// <returns></returns>
        public async Task<List<RoleTypeDto>> GetRoleTypesAsync()
        {
            var result = new List<RoleTypeDto>();
            _methodName = "GetRoleTypesAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetRoleTypes);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<RoleTypeDto>>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get claim details
        /// </summary>       
        /// <returns></returns>
        public async Task<List<UserClaimsDto>> GetClaimDetailsByIdAsync(long userId, string webToken = "")
        {
            var result = new List<UserClaimsDto>();
            _methodName = "GetClaimDetailsByIdAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var categoryIdDto = new UserIdDto
                {
                    UserId = userId
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson(categoryIdDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = !string.IsNullOrEmpty(webToken) ? PostAsync(ApiUrl.WebApiUrlPostClaimDetails, inputSring, webToken) : PostAsync(ApiUrl.WebApiUrlPostClaimDetails, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<UserClaimsDto>>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get role type claims
        /// </summary>       
        /// <returns></returns>
        public async Task<List<RoleDto>> GetRolesAsync()
        {
            var result = new List<RoleDto>();
            _methodName = "GetRoleTypesAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlSaveType);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<RoleDto>>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
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

        #region Common
        protected async Task<U> GetAsync<U>(string apiUrl) where U : class, new()
        {
            U result = default(U);
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(apiUrl);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<U>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return result;
                    }
                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new U();
        }
        protected async Task<T> GetById<T>(string apiUrl, long Id) where T : IAPIInputDTO, new()
        {
            T result = default(T);
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {

                if (Id != 0)
                {
                    var inputDtoJson = JsonHelper.ConvertObjectToJson(Id);
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
                            result = JsonConvert.DeserializeObject<T>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        }
                        if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                            var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                            result = Activator.CreateInstance<T>();
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
            if (EqualityComparer<T>.Default.Equals(result, default(T)))
            {
                result = new T();
            }
            return result;
        }

        protected async Task<T> GetByEncryptId<T>(string apiUrl, string Id) where T : IAPIInputDTO, new()
        {
            T result = default(T);
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {

                if (!String.IsNullOrEmpty(Id))
                {
                    var inputDtoJson = JsonHelper.ConvertObjectToJson(Id);
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
                            result = JsonConvert.DeserializeObject<T>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        }
                        if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                            var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                            result = Activator.CreateInstance<T>();
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
            if (EqualityComparer<T>.Default.Equals(result, default(T)))
            {
                result = new T();
            }
            return result;
        }
        protected async Task<T> GetByInputDto<T>(string apiUrl, object dto) where T : IAPIInputDTO, new()
        {
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new T();
            try
            {

                var inputDtoJson = JsonHelper.ConvertObjectToJson(dto);
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
                        result = JsonConvert.DeserializeObject<T>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        protected async Task<T> GetByEntityDto<T>(string apiUrl, object dto) where T : EntityDto, new()
        {
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new T();
            try
            {

                var inputDtoJson = JsonHelper.ConvertObjectToJson(dto);
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
                        result = JsonConvert.DeserializeObject<T>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                    }
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        result.PostStatus = false;
                        result.PostStatusMessage = errorDtoResult.Message;
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
        protected async Task<IList<T>> GetListAsync<T>(string apiUrl, object inputDto) where T : class
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
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        List<T> outPutDto = JsonConvert.DeserializeObject<List<T>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return outPutDto;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<T>();
        }


        protected async Task<IList<T>> GetListAsync<T>(string apiUrl) where T : class
        {
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(apiUrl);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        List<T> outPutDto = JsonConvert.DeserializeObject<List<T>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return outPutDto;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<T>();
        }

        protected async Task<T> AddOrUpdate<T>(string apiUrl, T inputDto, string addOrupdateMessage, string errorMessage) where T : IAPIInputDTO
        {
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = inputDto;
            try
            {
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
                        result.PostStatus = true;
                        result.PostMessage = addOrupdateMessage;
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
                result.PostMessage = errorMessage;
                _logger.Error(message);
            }
            return result;
        }
        protected async Task<DataSourceResult> GetKendoGridResultAsync<T>(string apiUrl, object inputDto) where T : class
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
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<KendoDataSourceResult<T>>(jarray[0]["response"].ToString(), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
                        return KendoGridResult<T>.KendoDataSourceResult(resultList);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new DataSourceResult();
        }

        #region Grid Server Side paging        

        /// <summary>
        /// Method to Get Kendo Grid Data Async
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputDto"></param>
        /// <param name="apiUrl"></param>
        /// <returns></returns>
        public async Task<DataSourceResult> GetKendoGridDataAsync<T>(KendoGridResult inputDto, string apiUrl) where T : class
        {
            var result = await GetKendoGridResultAsync<T>(apiUrl, inputDto);
            return result;
        }

        #endregion

        #endregion


    }

    public static class KendoGridResult<T>
    {
        public static DataSourceResult KendoDataSourceResult(KendoDataSourceResult<T> resultList)
        {
            if (resultList != null)
                return new DataSourceResult()
                {
                    Data = resultList?.Data ?? null,
                    Total = resultList?.Total ?? 0,
                    AggregateResults = resultList?.AggregateResults ?? null,
                    Errors = resultList?.Errors ?? null
                };
            else
                return new DataSourceResult();
        }
    }
}