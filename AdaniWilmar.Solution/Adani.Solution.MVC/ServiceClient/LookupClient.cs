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
using System.Data;
using Dapper;
using System.Data.SqlClient;

namespace Adani.Solution.MVC.ServiceClient
{
    public class LookupClient : BaseClient
    {
        private const string ServiceName = "Lookup Client";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;
        static string connectionString = ConfigHelper.SPConnectionString;

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
        protected async Task<T> GetById<T>(string apiUrl, long Id) where T : IAPIInputDTO
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
                result = Activator.CreateInstance<T>();
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
        protected async Task<T> GetByInputDto<T>(string apiUrl, object dto) where T : IAPIInputDTO, new()
        {
            var result = new T();
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
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

        public async Task<DataSourceResult> GetKendoGridDataAsync<T>(KendoGridResult inputDto, string apiUrl) where T : class
        {
            var result = await GetKendoGridResultAsync<T>(apiUrl, inputDto);
            return result;
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
        protected async Task<IList<T>> GetListWithoutInputAsync<T>(string apiUrl) where T : class
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

        #region Lookup

        /// <summary>
        /// Method to get address by pincode
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <param name="pincode"></param>
        /// <returns></returns>
        public async Task<PincodeAddressDto> GetAddressByPincode(int loginUserId, string pincode)
        {
            var result = new PincodeAddressDto();
            _methodName = "GetAddressByPincode";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDto = new PincodeInputDto()
                {
                    LoginUserId = loginUserId,
                    Pincode = pincode
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetAddressByPincode, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<PincodeAddressDto>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
                        result.PostStatus = true;
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

        /// <summary>
        /// Method to get state list
        /// </summary>       
        /// <returns></returns>
        public async Task<List<StateDto>> GetStateListAsync()
        {
            var result = new List<StateDto>();
            _methodName = "GetStateListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetStateList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<List<StateDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<IList<StateDto>> GetStateListByEmployeeIdsAsync(LoginUserIdDto inputDto)
        {
            var result = await GetListAsync<StateDto>(ApiUrl.WebApiUrlGetStateListByEmployees, inputDto);
            return result;
        }

        /// <summary>
        /// Method to get Oil Packing Type list
        /// </summary>       
        /// <returns></returns>
        public async Task<List<OilPackingTypeDto>> GetOilPackingTypeListAsync()
        {
            var result = new List<OilPackingTypeDto>();
            _methodName = "GetOilPackingTypeListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetOilPackingTypeList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<List<OilPackingTypeDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get Oil Packing Group Type
        /// </summary>
        /// <returns></returns>
        public async Task<List<OilPackingTypeDto>> GetOilPackingGroupListAsync()
        {
            var result = new List<OilPackingTypeDto>();
            _methodName = "GetOilPackingGroupListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetOilPackingGroupTypeList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<List<OilPackingTypeDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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



        public async Task<List<DropDownDto>> GetPackGroupListBySkuId(IdInputDto idInputDto)
        {
            var result = new List<DropDownDto>();
            _methodName = "GetOilPackingTypeListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var apiUrl = ApiUrl.WebApiUrlGetPackGroupListBySkuId;
                var inputDtoJson = JsonHelper.ConvertObjectToJson(idInputDto);
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
                        var responseResult = JsonConvert.DeserializeObject<List<DropDownDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get city list
        /// </summary>       
        /// <returns></returns>
        public async Task<List<CityDto>> GetCityListAsync()
        {
            var result = new List<CityDto>();
            _methodName = "GetCityListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetCityList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<List<CityDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Get District List By StateId
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public async Task<List<DistrictDto>> GetDistrictListByStateIdAsync(int stateId)
        {
            var result = new List<DistrictDto>();
            _methodName = "GetDistrictListByStateIdAsync";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var apiUrl = ApiUrl.WebApiUrlGetDistrictListByStateId;
                if (stateId != 0)
                {
                    var inputDtoJson = JsonHelper.ConvertObjectToJson(stateId);
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
                            result = JsonConvert.DeserializeObject<List<DistrictDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        }
                        if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                            var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
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
        /// Method to Get City List By DistrictName
        /// </summary>
        /// <param name="districtName"></param>
        /// <returns></returns>
        public async Task<List<CityDto>> GetCityListByDistrictName(string districtName)
        {
            var result = new List<CityDto>();
            _methodName = "GetCityListByDistrictName";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var apiUrl = ApiUrl.WebApiUrlGetCityListByDistrictId;
                if (!string.IsNullOrEmpty(districtName))
                {
                    var inputDtoJson = JsonHelper.ConvertObjectToJson(districtName);
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
                            result = JsonConvert.DeserializeObject<List<CityDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        }
                        if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                            var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
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
        /// Method to Get City List By DistrictName
        /// </summary>
        /// <param name="CitiestName"></param>
        /// <returns></returns>
        public async Task<List<CityDto>> GetCityListByStateName(string stateName)
        {
            var result = new List<CityDto>();
            _methodName = "GetCityListByStateName";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var apiUrl = ApiUrl.WebApiUrlGetCityListByStateId;
                if (!string.IsNullOrEmpty(stateName))
                {
                    var inputDtoJson = JsonHelper.ConvertObjectToJson(stateName);
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
                            result = JsonConvert.DeserializeObject<List<CityDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        }
                        if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                            var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
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


        public async Task<IList<IngredientDownDto>> GetIngredientCostddl(LoginUserIdDto inputDto)
        {
            try
            {
                _methodName = "GetIngredientCostddl";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetIngredientCostddl, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<IngredientDownDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<IngredientDownDto>();
        }

        public async Task<List<DropDownDto>> GetOilTypesBasedOnVerticalId(IdInputDto inputDto)
        {
            try
            {
                _methodName = "GetOilTypesBasedOnVerticalId";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetOilTypesBasedOnVerticalId, inputSring);
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
                _logger.Error(message);
            }
            return new List<DropDownDto>();
        }

        //get oiltypes based on vertical if there is vertical id or gets all oiltypes
        public async Task<IList<DropDownDto>> GetOilTypesBasedOnVertical(LoginUserIdDto inputDto)
        {
            _methodName = "GetOilTypesBasedOnVertical";
            string apiUrl = ApiUrl.WebApiUrlGetOilTypesBasedOnVertical;
            var response = await GetListAsync<DropDownDto>(apiUrl, inputDto);
            return response;
        }

        public async Task<List<DropDownDto>> GetStatesBasedOnCustomerGroupId(IdInputDto inputDto)
        {
            try
            {
                _methodName = "GetStatesBasedOnCustomerGroupId";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetStatesBasedOnCustomerGroupId, inputSring);
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
                _logger.Error(message);
            }
            return new List<DropDownDto>();
        }

        public async Task<List<DropDownDto>> GetSkusBasedOnOilTypeId(IdInputDto inputDto)
        {
            try
            {
                _methodName = "GetSkusBasedOnOilTypeId";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSkusBasedOnOilTypeId, inputSring);
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
                _logger.Error(message);
            }
            return new List<DropDownDto>();
        }

        public async Task<IList<DropDownDto>> GetCityListByDistrictIdForDropdown(int districtId)
        {
            _methodName = "GetCityListByDistrictIdForDropdown";
            string apiUrl = ApiUrl.WebApiUrlGetCityListByDistrictIdForDropdown;
            var result = await GetListAsync<DropDownDto>(apiUrl, districtId);
            return result;
        }

        #endregion

        #region Verticle Oiltype Sku

        public async Task<IList<DropDownDto>> GetOilTypesBasedOnVerticle(OilTypeInputDto oilTypeInput)
        {
            try
            {
                _methodName = "GetOilTypesBasedOnVerticle";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<OilTypeInputDto>(oilTypeInput);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetOilTypesBasedOnVerticle, inputSring);
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
                _logger.Error(message);
            }
            return new List<DropDownDto>();
        }

        public async Task<List<SkuDropDown>> GetSkusBasedOnOilType(SkuInputDto skuInputDto)
        {
            try
            {
                _methodName = "GetSkusBasedOnOilType";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<SkuInputDto>(skuInputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSkusBasedOnOilType, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<SkuDropDown>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<SkuDropDown>();
        }

        public async Task<List<DropDownDto>> GetSkusBasedOnEmployeeDiscount(SkuInputDto skuInputDto)
        {
            try
            {
                _methodName = "GetSkusBasedOnEmployeeDiscount";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<SkuInputDto>(skuInputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSkusBasedOnEmployeeDiscount, inputSring);
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
                _logger.Error(message);
            }
            return new List<DropDownDto>();
        }

        #endregion
        /// <summary>
        /// Method to get uom list
        /// </summary>       
        /// <returns></returns>
        public async Task<List<UomDto>> GetUomListAsync()
        {
            var result = new List<UomDto>();
            _methodName = "GetUomListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetUomList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<List<UomDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        #region Dealer And Broker Details

        public async Task<List<DealerBrokerDto>> GetDealerAndBrokerDetails(ReportingUsersInputDto inputDto)
        {
            try
            {
                _methodName = "GetDealerAndBrokerDetails";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<ReportingUsersInputDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetDealerBrokerList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<DealerBrokerDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<DealerBrokerDto>();
        }

        public async Task<List<DealerBrokerDto>> GetDealerAndBrokerListForBDO(ReportingUsersInputDto inputDto)
        {
            try
            {
                _methodName = "GetDealerBrokerListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<ReportingUsersInputDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetDealerAndBrokerListForBDO, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<DealerBrokerDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<DealerBrokerDto>();
        }

        #endregion

        public async Task<List<DropDownDto>> GetZoneddl(LoginUserIdDto inputDto)
        {
            try
            {
                _methodName = "GetZoneddl";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetOilTypesBasedOnVerticalId, inputSring);
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
                _logger.Error(message);
            }
            return new List<DropDownDto>();
        }

        #region Competitors

        public async Task<CompetitorDto> AddOrUpdateCompetitor(CompetitorDto raMarginDto)
        {
            _methodName = "AddOrUpdateCompetitor";

            if (!string.IsNullOrEmpty(raMarginDto.SelecteSkuId))
            {
                raMarginDto.SelectedSkuIds = UtilityHelper.ConvertStringToLongList(raMarginDto.SelecteSkuId);
            }
            if (!string.IsNullOrEmpty(raMarginDto.RemovedSkuId))
            {
                raMarginDto.RemovedSkuIds = UtilityHelper.ConvertStringToLongList(raMarginDto.RemovedSkuId);
            }

            var addOrUpdateMessage = raMarginDto.Id > 0 ? Helper.GetResourceString("msg_CompetitorUpdateSuccess") : Helper.GetResourceString("msg_CompetitorSaveSuccess");
            var errorMessage = Helper.GetResourceString("msg_CompetitorSaveError");
            var apiUrl = !String.IsNullOrEmpty(raMarginDto.EncryptedId) ? ApiUrl.WebApiUrlUpdateCompetitor : ApiUrl.WebApiUrlSaveCompetitor;
            return await AddOrUpdate(apiUrl, raMarginDto, addOrUpdateMessage, errorMessage);
        }

        public async Task<IList<CompetitorDto>> GetCompetitorListAsync(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetCompetitorListAsync";
            string apiUrl = ApiUrl.WebApiUrlGetCompetitorList;
            var response = await GetListAsync<CompetitorDto>(apiUrl, loginUserIdDto);
            return response;
        }

        public async Task<CompetitorDto> GetCompetitorDetailsById(string competitorId)
        {
            _methodName = "GetCompetitorDetailsById";
            string apiUrl = ApiUrl.WebApiUrlGetCompetitorById;
            var result = await GetByEncryptId<CompetitorDto>(apiUrl, competitorId);
            return result;
        }

        /// <summary>
        /// Method to get Competitor Skus
        /// </summary>       
        /// <returns></returns>
        public async Task<IList<SkuDto>> GetSkuBasedOnOilTypesAsync(CompetitorSkuInputDto competitorSkuInputDto)
        {
            _methodName = "GetSkuBasedOnOilTypesAsync";
            string apiUrl = ApiUrl.WebApiUrlGetSkuBasedOilTypeList;
            var result = await GetListAsync<SkuDto>(apiUrl, competitorSkuInputDto);
            return result;
        }

        public async Task<List<CompetitorDto>> ExportCompetitor(LoginUserIdDto inputDto)
        {
            _methodName = "ExportCompetitor";
            var result = await GetListAsync<CompetitorDto>(ApiUrl.WebApiUrlExportCompetitor, inputDto);
            return result.ToList();
        }

        #endregion

        #region Dealer details

        public async Task<List<DealerBrokerDto>> GetDealerDetails(DealerBrokerParamDto inputDto)
        {
            try
            {
                _methodName = "GetDealerDetails";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<DealerBrokerParamDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetDealerDetailsByVertical, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<DealerBrokerDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<DealerBrokerDto>();
        }

        #endregion

        #region Lookup

        public IList<DropDownDto> GetProcessListForHierarchy()
        {
            _methodName = "GetProcessListForHierarchy";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");

            var resultList = new List<DropDownDto>();
            foreach (var item in Settings.EnumToList<HierarchyProcess>())
            {
                var aleTypeItem = new DropDownDto
                {
                    Name = Settings.GetEnumDescription(item),
                    Id = (int)item
                };
                resultList.Add(aleTypeItem);
            }
            return resultList;
        }

        public async Task<List<DropDownDto>> GetUsersByRoleIdddl(IdInputDto inputDto)
        {
            try
            {
                _methodName = "GetReportingToRoles";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetReportingToUsersByRole, inputSring);
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
                _logger.Error(message);
            }
            return new List<DropDownDto>();
        }

        public async Task<List<DropDownDto>> GetSubCategoryList()
        {
            var result = new List<DropDownDto>();
            _methodName = "GetSubCategoryList";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetSubCategoryListddl);
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

        public async Task<List<DropDownDto>> GetDepotsByPlantId(IdInputDto inputDto)
        {
            try
            {
                _methodName = "GetReportingToRoles";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetDepotsByPlantId, inputSring);
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
                _logger.Error(message);
            }
            return new List<DropDownDto>();
        }

        /// <summary>
        /// Method to get Competitor Skus
        /// </summary>       
        /// <returns></returns>
        public async Task<IList<DistrictDto>> GetDistrictBasedOnTerritory(int territoryId)
        {
            _methodName = "GetDistrictBasedOnTerritory";
            string apiUrl = ApiUrl.WebApiUrlGetDistrictBasedTerritory;
            var result = await GetListAsync<DistrictDto>(apiUrl, territoryId);
            return result;
        }


        /// <summary>
        /// Method to get Competitor Skus
        /// </summary>       
        /// <returns></returns>
        public async Task<IList<DistrictDto>> GetDistrictBasedOnState(int territoryId)
        {
            _methodName = "GetDistrictBasedOnTerritory";
            string apiUrl = ApiUrl.WebApiUrlGetDistrictBasedTerritory;
            var result = await GetListAsync<DistrictDto>(apiUrl, territoryId);
            return result;
        }

        /// <summary>
        /// Method to get Competitor Skus
        /// </summary>       
        /// <returns></returns>
        public async Task<IList<DistrictDto>> GetUnMappedDistrictListByStateId(int stateId)
        {
            _methodName = "GetUnMappedDistrictListByStateId";
            string apiUrl = ApiUrl.WebApiUrlGetUnMappedDistrictListByStateId;
            var result = await GetListAsync<DistrictDto>(apiUrl, stateId);
            return result;
        }

        /// <summary>
        /// Method to get uom list
        /// </summary>       
        /// <returns></returns>
        public async Task<ConfigurationViewModel> GetConfigurationList()
        {
            var result = new ConfigurationViewModel();
            _methodName = "GetConfigurationList";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetConfigurationList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<List<ConfigurationDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());

                        result.Configurations = responseResult.ToList();
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

        public async Task<ConfigurationViewModel> UpdateConfiguration(ConfigurationViewModel configurationViewModel)
        {
            _methodName = "UpdateConfiguration";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var UpdateMessage = Helper.GetResourceString("msg_ConfigurationUpdateSuccess");
            var errorMessage = Helper.GetResourceString("msg_ConfigurationSaveError");
            var result = configurationViewModel;
            try
            {
                var inputDtoJson = string.Empty;
                inputDtoJson = JsonHelper.ConvertObjectToJson(configurationViewModel.Configurations);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostUpdateConfigurationList, inputSring);
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
                        result.PostMessage = UpdateMessage;
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

        public async Task<IList<DropDownDto>> GetSkuListByPackGroupIdAsync(SkuDropDownInputDto inputDto)
        {
            _methodName = "GetSkuListByPackGroupIdAsync";
            string apiUrl = ApiUrl.WebApiUrlGetSkuListByPackGroupId;
            var result = await GetListAsync<DropDownDto>(apiUrl, inputDto);
            return result;
        }

        #endregion

        #region Key Performance Indicator

        public async Task<KeyPerformanceDto> AddOrUpdateKeyPerformance(KeyPerformanceDto inputDto)
        {
            _methodName = "AddOrUpdateKeyPerformance";
            var addOrUpdateMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_KeyPerformanceIndicatorUpdateSuccess") : Helper.GetResourceString("msg_KeyPerformanceIndicatorSaveSuccess");
            var errorMessage = Helper.GetResourceString("msg_KeyPerformanceSaveError");
            var apiUrl = inputDto.Id > 0 ? ApiUrl.WebApiUrlPostUpdateKeyPerformance : ApiUrl.WebApiUrlPostAddKeyPerformance;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, errorMessage);
        }

        public async Task<IList<KeyPerformanceDto>> GetKeyPerformanceListAsync(LoginUserIdDto inputDto)
        {
            _methodName = "GetKeyPerformanceListAsync";
            string apiUrl = ApiUrl.WebApiUrlGetKeyPerformanceList;
            var response = await GetListAsync<KeyPerformanceDto>(apiUrl, inputDto);
            return response;
        }

        public async Task<KeyPerformanceDto> GetKeyPerformanceById(IdInputDto inputDto)
        {
            _methodName = "GetKeyPerformanceById";
            string apiUrl = ApiUrl.WebApiUrlGetKeyPerformanceById;
            var result = await GetByInputDto<KeyPerformanceDto>(apiUrl, inputDto);
            return result;
        }

        #endregion

        public async Task<IList<DropDownDto>> GetUsersByRoleId(IdInputDto inputDto)
        {
            _methodName = "GetSkuListByPackGroupIdAsync";
            string apiUrl = ApiUrl.WebApiUrlGetUsersByRoleIdddl;
            var result = await GetListAsync<DropDownDto>(apiUrl, inputDto);
            return result;
        }

        public async Task<IList<DropDownDto>> GetTradeTicketOilTypes(IdInputDto inputDto)
        {
            _methodName = "GetTradeTicketOilTypes";
            string apiUrl = ApiUrl.WebApiUrlGetTradeTicketOilTypes;
            var result = await GetListAsync<DropDownDto>(apiUrl, inputDto);
            return result;
        }

        /// <summary>
        /// Get Depts Based on Plant Ids
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<DropDownDto>> GetDepotsByPlantIds(DepotDropDownParam inputDto)
        {
            _methodName = "GetDepotsByPlantIds";
            string apiUrl = ApiUrl.WebApiUrlGetDepotsByPlantIds;
            var result = await GetListAsync<DropDownDto>(apiUrl, inputDto);
            return result;
        }

        #region Sku Ingredient OilTypes

        public async Task<IList<DropDownDto>> GetSkuIngredienOilTypes(IdInputDto inputDto)
        {
            _methodName = "GetSkuIngredienOilTypes";
            string apiUrl = ApiUrl.WebApiUrlGetSkuIngredienOilTypes;
            var result = await GetListAsync<DropDownDto>(apiUrl, inputDto);
            return result;
        }

        #endregion

        #region Material Cost Oiltypes

        public async Task<IList<DropDownDto>> MaterialCostOilTypesBasedOnVerticalId(IdInputDto inputDto)
        {
            _methodName = "MaterialCostOilTypesBasedOnVerticalId";
            string apiUrl = ApiUrl.WebApiUrlMaterialCostOilTypes;
            var result = await GetListAsync<DropDownDto>(apiUrl, inputDto);
            return result;
        }

        #endregion
        public async Task<OilTypeDto> GetOilTypesById(string inputDto)
        {
            _methodName = "GetOilTypesById";
            var result = await GetByEncryptId<OilTypeDto>(ApiUrl.WebApiUrlGetOilTypesById, inputDto);
            return result;
        }
        public async Task<VerticalDto> GetVerticalById(string inputDto)
        {
            _methodName = "GetVerticalById";
            var result = await GetByEncryptId<VerticalDto>(ApiUrl.WebApiUrlGetVerticalById, inputDto);
            return result;
        }
        #region Lookup

        public async Task<IList<DropDownDto>> GetSkuBasedOnOilTypeSubCategory(SkuDropDownInputDto inputDto)
        {
            _methodName = "GetSkuBasedOnOilTypeSubCategory";
            string apiUrl = ApiUrl.WebApiUrlGetSkuBasedOnOilTypeSubCategory;
            var result = await GetListAsync<DropDownDto>(apiUrl, inputDto);
            return result;
        }

        public async Task<IList<DropDownDto>> GetSkulistBasedOnCombination(LoginUserIdDto inputDto)
        {
            _methodName = "GetSkulistBasedOnCombination";
            string apiUrl = ApiUrl.WebApiUrlGetSkuBasedOnCombination;
            var result = await GetListAsync<DropDownDto>(apiUrl, inputDto);
            return result;
        }

        public async Task<OilTypeNameDto> GetOilTypeIsRasoiOrNot(IdInputDto inputDto)
        {
            _methodName = "GetOilTypeIsRasoiOrNot";
            string apiUrl = ApiUrl.WebApiUrlGetOilTypeIsRasoiOrNot;
            var result = await GetByInputDto<OilTypeNameDto>(apiUrl, inputDto);
            return result;
        }

        public async Task<IList<DropDownDto>> GetOilTypesByVerticalId(IdInputDto inputDto)
        {
            _methodName = "GetOilTypesByVerticalId";
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetOilTypesByVerticalId, inputDto);
            return result;
        }

        public async Task<IList<DropDownDto>> GetPlantDepotRakeByStateId(IdInputDto inputDto)
        {
            _methodName = "GetPlantDepotRakeByStateId";
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetPlantDepotRakeByStateId, inputDto);
            return result;
        }

        public async Task<IList<DropDownDto>> GetFreightZoneByStateId(IdInputDto inputDto)
        {
            _methodName = "GetFreightZoneByStateId";
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetFreightZoneByStateId, inputDto);
            return result;
        }

        public async Task<List<DropDownDto>> GetSkuListBasedOnOilTypeIdSubCategoryIdPackGroupIdForDropdown(SkuDropDownInputDto inputDto)
        {
            _methodName = "GetSkuListBasedOnOilTypeIdSubCategoryIdPackGroupIdForDropdown";
            string apiUrl = ApiUrl.WebApiUrlGetSkuListBasedOnOilTypeIdSubCategoryIdPackGroupIdForDropdown;
            var result = await GetListAsync<DropDownDto>(apiUrl, inputDto);
            return result.ToList();
        }
        #endregion
        public async Task<List<DropDownDto>> GetDealerDetailsddl(FreightZoneAndRouteDropDownInputDto inputDto)
        {
            var result = new List<DropDownDto>();
            _methodName = "GetDealerDetailsddl";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetDealersList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<List<DropDownDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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


        #region VehicleLoadabilities

        public async Task<VehicleLoadabilitiesDto> AddOrUpdateVehicleLoadabilities(VehicleLoadabilitiesDto inputDto)
        {
            _methodName = "AddOrUpdateVehicleLoadabilities";
            var addOrUpdateMessage = inputDto.Id > 0 ? "Successfully Updated" : "Successfully added";

            return await AddOrUpdate(ApiUrl.WebApiUrlAddVehicleLoadabilities, inputDto, addOrUpdateMessage, "Error");
        }


        public async Task<List<VehicleLoadabilitiesGridDataDto>> GetVehicleLoadabilitiesListAsync()
        {
            var result = new List<VehicleLoadabilitiesGridDataDto>();
            _methodName = "GetVehicleLoadabilitiesListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetAllVehicleLoadabilities);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<List<VehicleLoadabilitiesGridDataDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<VehicleLoadabilitiesDto> GetVehicleLoadabilitiesByIdAsync(long userId, long vehicleLoadabilitiesId)
        {
            try
            {
                _methodName = "GetVehicleLoadabilitiesByIdAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDto = new VehicleLoadabilitiesDto() { UserId = userId, Id = vehicleLoadabilitiesId };
                var inputDtoJson = JsonHelper.ConvertObjectToJson<VehicleLoadabilitiesDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetVehicleLoadabilitiesById, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<VehicleLoadabilitiesDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new VehicleLoadabilitiesDto();
        }

        public async Task<List<VehicleLoadabilitiesDto>> ExportVehicleLoadabilities(LoginUserIdDto inputDto)
        {
            _methodName = "ExportVehicleLoadabilities";
            var result = await GetListAsync<VehicleLoadabilitiesDto>(ApiUrl.WebApiUrlExportVehicleLoadabilitiesList, inputDto);
            return result.ToList();
        }

        public async Task<List<DropDownDto>> GetOilTypeListByVerticalIdsForDropDown(IdInputDto inputDto)
        {
            _methodName = "GetOilTypeListByVerticalIdsForDropDown";
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetOilTypeListByVerticalIdListForDropDown, inputDto);
            return result.ToList();
        }

        public async Task<List<DropDownDto>> GetOilPackingTypeListForDropdown()
        {
            _methodName = "GetOilPackingTypeListForDropdown";
            var result = await GetListWithoutInputAsync<DropDownDto>(ApiUrl.WebApiUrlGetOilPackingTypeListForDropdown);
            return result.ToList();
        }

        public async Task<List<DropDownDto>> GetVerticalListForDropdown(LoginUserIdDto inputDto)
        {
            _methodName = "GetVerticalListForDropdown";
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetVerticalListForDropdown, inputDto);
            return result.ToList();
        }


        public async Task<IList<DropDownDto>> GetSkuListByOilTypeIdsPackGroupIdsForDropdown(DropDownInputDto inputDto)
        {
            _methodName = "GetSkuListByOilTypeIdsPackGroupIdsForDropdown";
            string apiUrl = ApiUrl.WebApiUrlGetSkuListByOilTypeIdsPackGroupIdsForDropdown;
            var result = await GetListAsync<DropDownDto>(apiUrl, inputDto);
            return result;
        }


        #endregion


        #region TPNotification

        public async Task<IList<DropDownDto>> GetBdoddlAsync(LoginUserIdDto inputDto)
        {
            _methodName = "GetCustomerGroupddlAsync";
            string apiUrl = ApiUrl.WebApiUrlGetBdoddlList;
            var response = await GetListAsync<DropDownDto>(apiUrl, inputDto);
            return response;
        }

        public async Task<NotificationsDto> AddOrUpdateNotification(NotificationsDto inputDto)
        {
            _methodName = "AddOrUpdateNotification";
            var addOrUpdateMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_NotificationUpdatedSuccessfully") : Helper.GetResourceString("msg_NotificationSavedSuccessfully");
            var errorMessage = Helper.GetResourceString("msg_NotificationError");
            var apiUrl = inputDto.Id > 0 ? ApiUrl.WebApiUrlPostUpdateNotification : ApiUrl.WebApiUrlPostAddNotification;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, errorMessage);
        }
        public async Task<IList<NotificationsDto>> GetTPNotificationListAsync(LoginUserIdDto inputDto)
        {
            _methodName = "GetTPNotificationListAsync";
            string apiUrl = ApiUrl.WebApiUrlGetTPNotificationList;
            var response = await GetListAsync<NotificationsDto>(apiUrl, inputDto);
            return response;
        }
        public async Task<List<NotificationDetailDto>> GetTPNotificationDetailsById(long tpNotificationId)
        {
            _methodName = "GetTPNotificationDetailsById";
            var result = await GetListAsync<NotificationDetailDto>(ApiUrl.WebApiUrlGetTPNotificationDetails, tpNotificationId);
            return result.ToList();
        }
        public async Task<NotificationsDto> GetTPNotificationById(IdInputDto inputDto)
        {
            _methodName = "GetRANotificationById";
            string apiUrl = ApiUrl.WebApiUrlGetTPNotificationById;
            var result = await GetByInputDto<NotificationsDto>(apiUrl, inputDto);
            return result;
        }
        public async Task<Kendo.Mvc.UI.DataSourceResult> GetMappedDealerListByTPNotificationId(NotificationGridInputDto inputDto)
        {
            _methodName = "GetMappedCustomerListByRaNotificationId";
            var result = await GetKendoGridResultAsync<NotificationDetailDto>(ApiUrl.WebApiUrlGetMappedDealerListByRaNotificationId, inputDto);
            return result;
        }
        public async Task<List<NotificationsDto>> ExportTPNotificationList(LoginUserIdDto inputDto)
        {
            _methodName = "ExportTPNotificationList";
            var result = await GetListAsync<NotificationsDto>(ApiUrl.WebApiUrlGetTPNotificationExport, inputDto);
            return result.ToList();
        }
        #endregion

        #region Sauda conversion type
        public async Task<SaudaConversionTypeViewModel> GetSaudaConversionList()
        {
            var result = new SaudaConversionTypeViewModel();
            _methodName = "GetSaudaConversionList";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetSaudaConversionTypeList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<List<SaudaConversionTypeDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());

                        result.ConversionTypes = responseResult.ToList();
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
        public async Task<SaudaConversionTypeViewModel> UpdateSaudaConversionType(SaudaConversionTypeViewModel saudaConversionTypeViewModel)
        {
            _methodName = "UpdateSaudaConversionType";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var UpdateMessage = Helper.GetResourceString("msg_SaudaConversionTypeUpdated");
            var errorMessage = Helper.GetResourceString("msg_SaudaConversionTypeSaveError");
            var result = saudaConversionTypeViewModel;
            try
            {
                var inputDtoJson = string.Empty;
                inputDtoJson = JsonHelper.ConvertObjectToJson(saudaConversionTypeViewModel.ConversionTypes);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostUpdateSaudaConversionTypeList, inputSring);
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
                        result.PostMessage = UpdateMessage;
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
        public async Task<List<SaudaConversionSkusDetail>> GetSaudaConversionDetailList(SaudaConversionHistoryInputDto inputDto)
        {
            /*_methodName = "GetSaudaConversionDetailList";
            var result = new List<SaudaConversionHistoryDto>();
            var response = await GetListAsync<SaudaConversionSKUStatusListDto>(ApiUrl.WebApiUrlPostSaudaConversionDetailList, inputDto);
            return result.ToList();*/

            var result = new List<SaudaConversionSkusDetail>();
            var responseoutput = new SaudaConversionSKUStatusListModel();
            _methodName = "GetSaudaConversionDetailList";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostSaudaConversionDetailList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<SaudaConversionSkusDetail>>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
                    }
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        responseoutput.PostStatus = false;
                        responseoutput.PostStatusMessage = errorDtoResult.Message;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result.ToList();
        }

        public async Task<SaudaConversionDetailViewModel> GetSaudaConversionDetailsById(long conversionId)
        {
            _methodName = "GetSaudaConversionDetailsById";
            string apiUrl = ApiUrl.WebApiUrlPostSaudaConversionDetailsById;

            SaudaConversionSKUInputDto inputDto = new SaudaConversionSKUInputDto()
            {
                SaudaConversionId = conversionId
            };
            var result = await GetByInputDto<SaudaConversionDetailViewModel>(apiUrl, inputDto);
            return result;
        }

        /// <summary>
        /// Method to Export SaudaConversion 
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public List<SaudaConversionSkusDetailExportDto> ExportSaudaConversion(SaudaConversionHistoryInputDto inputDto)
        {
            _methodName = "ExportSaudaConversion";
            var saudaConversionDetails = new List<SaudaConversionSkusDetail>();
            var plantOrDepotDetails = new List<SaudaConversionPantAndDepotDetails>();
            var result = new List<SaudaConversionSkusDetailExportDto>();
            try
            {
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    string reportQuery = "";
                    string PlantOrDepotIds = "";

                    if (inputDto.StatusId == (int)DTO.Enums.Status.Pending)
                    {
                        reportQuery = @"select s.Id as SkuConversionId ,CONVERT(VARCHAR(10), s.CreatedDate, 103) as ConversionCreatedDateInString,s.QuantityInMt as SaudaQuantityInMT,
                        s.QuantityInSku as SaudaQuantityInSku,s.Remarks,sku.SkuName,s.PlantId,s.DepotId
                        ,dealer.Name as DealerName,StateTrader.Name as BdoName,ZonalTrader.Name as ZonalheadName from SaudaConversionSkus as s
                        join Skus as sku on s.SkuId = sku.Id
                        join Users as dealer on s.DealerId = dealer.Id
                        join UserCustomerMappings as ucm on s.DealerId = ucm.CustomerId
                        join Users as StateTrader  on ucm.UserId = StateTrader.Id
                        join Users as ZonalTrader on StateTrader.OrganizationReportingToId = ZonalTrader.Id
                        where s.IsApproved != 1 and Cast(s.CreatedDate as Date) >= Cast(@ValidFrom as Date) and Cast(s.CreatedDate as Date) <= Cast(@ValidTo as Date)
                        and s.SaudaConversionSkuHeaderId is null and s.StatusId != 3 and (sku.VerticalId = @VerticalId or @VerticalId = 0)
                        order by s.CreatedDate desc ";
                    }
                    else if(inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                    {
                        reportQuery = @"select s.Id as SkuConversionId ,CONVERT(VARCHAR(10), s.CreatedDate, 103) as ConversionCreatedDateInstring,s.QuantityInMt as SaudaQuantityInMT,
                        s.QuantityInSku as SaudaQuantityInSku,s.Remarks,sku.SkuName,dealer.Name as DealerName,s.PlantId,s.DepotId,
                        StateTrader.Name as BdoName,ZonalTrader.Name as ZonalheadName from SaudaConversionSkus as s
                        join Skus as sku on s.SkuId = sku.Id
                        join Users as dealer on s.DealerId = dealer.Id
                        join UserCustomerMappings as ucm on s.DealerId = ucm.CustomerId
                        join Users as StateTrader  on ucm.UserId = StateTrader.Id
                        join Users as ZonalTrader on StateTrader.OrganizationReportingToId = ZonalTrader.Id
                        where s.IsApproved = 1 and Cast(s.CreatedDate as Date) >= Cast(@ValidFrom as Date) and Cast(s.CreatedDate as Date) <= Cast(@ValidTo as Date)
                        and ( s.SaudaConversionSkuHeaderId is null or s.SaudaConversionSkuHeaderId = 0 ) and s.StatusId != 3 and (sku.VerticalId = @VerticalId or @VerticalId = 0)
                        order by s.CreatedDate desc  ";
                    }
                    else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                    {
                        reportQuery = @"select s.Id as SkuConversionId ,CONVERT(VARCHAR(10), s.CreatedDate, 103) as ConversionCreatedDateInstring,s.QuantityInMt as SaudaQuantityInMT,
                        s.QuantityInSku as SaudaQuantityInSku,s.Remarks,sku.SkuName,dealer.Name as DealerName,s.PlantId,s.DepotId,
                        StateTrader.Name as BdoName,ZonalTrader.Name as ZonalheadName from SaudaConversionSkus as s
                        join Skus as sku on s.SkuId = sku.Id
                        join Users as dealer on s.DealerId = dealer.Id
                        join UserCustomerMappings as ucm on s.DealerId = ucm.CustomerId
                        join Users as StateTrader  on ucm.UserId = StateTrader.Id
                        join Users as ZonalTrader on StateTrader.OrganizationReportingToId = ZonalTrader.Id
                        where s.StatusId = 3 and Cast(s.CreatedDate as Date) >= Cast(@ValidFrom as Date) and Cast(s.CreatedDate as Date) <= Cast(@ValidTo as Date)
                        and ( s.SaudaConversionSkuHeaderId is null) and (sku.VerticalId = @VerticalId or @VerticalId = 0)
                        order by s.CreatedDate desc  ";
                    }

                    saudaConversionDetails = conn.Query<SaudaConversionSkusDetail>(reportQuery, new
                    {
                        ValidFrom = inputDto.FromDate.Date,
                        ValidTo = inputDto.ToDate.Date,
                        VerticalId = inputDto.VerticalId
                    }).ToList();

                    PlantOrDepotIds = @"select Id as PlantOrDepotId, Code as PlantOrDepotCode , Name as PlantOrDepotName from Depots";
                    plantOrDepotDetails = conn.Query<SaudaConversionPantAndDepotDetails>(PlantOrDepotIds).ToList();

                    foreach (var data in saudaConversionDetails)
                    {
                        data.PlantOrDepotCode = data.PlantId == 0 ? plantOrDepotDetails.FirstOrDefault(_ => _.PlantOrDepotId == data.DepotId) != null ? plantOrDepotDetails.FirstOrDefault(_ => _.PlantOrDepotId == data.DepotId).PlantOrDepotCode : string.Empty :
                            plantOrDepotDetails.FirstOrDefault(_ => _.PlantOrDepotId == data.PlantId) != null ? plantOrDepotDetails.FirstOrDefault(_ => _.PlantOrDepotId == data.PlantId).PlantOrDepotCode : string.Empty;
                        data.PlantOrDepotName = data.PlantId == 0 ? plantOrDepotDetails.FirstOrDefault(_ => _.PlantOrDepotId == data.DepotId) != null ? plantOrDepotDetails.FirstOrDefault(_ => _.PlantOrDepotId == data.DepotId).PlantOrDepotName : string.Empty
                            : plantOrDepotDetails.FirstOrDefault(_ => _.PlantOrDepotId == data.PlantId) != null ? plantOrDepotDetails.FirstOrDefault(_ => _.PlantOrDepotId == data.PlantId).PlantOrDepotName : string.Empty;
                    }


                    result = saudaConversionDetails.Select(a => new SaudaConversionSkusDetailExportDto
                    {
                        ConversionQuantityInCase = string.Format("{0:0.00}", a.SaudaQuantityInSku),
                        ConversionQuantityInMT = string.Format("{0:0.000}", a.SaudaQuantityInMT),
                        SkuConversionId = a.SkuConversionId,
                        Remarks = a.Remarks,
                        ConversionCreatedDate = a.ConversionCreatedDateInstring,
                        SkuName = a.SkuName,
                        PlantOrDepotCode = a.PlantOrDepotCode,
                        PlantOrDepotName = a.PlantOrDepotName,
                        Dealer = a.DealerName,
                        StateTrader = a.BdoName,
                        ZonalTrader = a.ZonalHeadName
                    }).ToList();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        #endregion Sauda conversion type

        #region Sauda Extention policy

        public async Task<List<DropDownDto>> GetActiveStateListBasedOnZonalHeadIdsAsync(List<long> zonalHeadIds)
        {
            var result = new List<DropDownDto>();
            _methodName = "GetActiveStateListBasedOnZonalHeadIdsAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(zonalHeadIds);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetActiveStateListBasedOnZonalHeadIds, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<List<DropDownDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        public async Task<IList<OilTypeDto>> GetActiveOilTypeListAsync(LoginUserIdDto inputDto)
        {
            _methodName = "GetActiveOilTypeListAsync";
            string apiUrl = ApiUrl.WebApiUrlGetOilTypeList;
            var response = await GetListAsync<OilTypeDto>(apiUrl, inputDto);
            return response;
        }
        //public async Task<List<OilTypeDto>> GetActiveOilTypeListAsync()
        //{
        //    var result = new List<OilTypeDto>();
        //    _methodName = "GetActiveOilTypeListAsync";
        //    try
        //    {
        //        _logger.Info($"{ServiceName} Controller-Method {_methodName}");
        //        HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetOilTypeList);
        //        var responseData = await response.Content.ReadAsStringAsync();
        //        responseData = UtilityHelper.TrimStartEnd(responseData);
        //        var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
        //        if (response.IsSuccessStatusCode)
        //        {
        //            if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
        //            {
        //                var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

        //                var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
        //                var responseResult = JsonConvert.DeserializeObject<List<OilTypeDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
        //                result.AddRange(responseResult.ToList());
        //            }
        //        }

        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //    }
        //    return result;
        //}

        public async Task<SaudaExtensionPolicyAddDto> AddSaudaExtensionPolicy(SaudaExtensionPolicyAddDto inputDto)
        {
            _methodName = "AddSaudaExtensionPolicy";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new SaudaExtensionPolicyAddDto();
            try
            {
                var apiUrl = string.Empty;
                var inputDtoJson = string.Empty;

                apiUrl = ApiUrl.WebApiUrlAddSaudaExtensionPolicy;

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
                        result.PostMessage = Helper.GetResourceString("msg_SaudaExtensionAdded");
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
                _logger.Error(message);
            }
            return result;
        }
        public async Task<List<SaudaExtensionPolicyViewDto>> GetSaudaExtensionListClient(long verticalId)
        {
            var result = new List<SaudaExtensionPolicyViewDto>();
            _methodName = "GetSaudaExtensionListClient";

            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = string.Empty;
                inputDtoJson = JsonHelper.ConvertObjectToJson(verticalId);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlListSaudaExtensionPolicy, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<List<SaudaExtensionPolicyViewDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        result.AddRange(responseResult.ToList());
                        return result;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<SaudaExtensionPolicyViewDto>();
        }

        /// <summary>
        /// Method to Export Extension Policy 
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public List<SaudaExtensionPolicyExportDto> ExportExtensionPolicy(long verticalId)
        {
            _methodName = "ExportExtensionPolicy";
            var result = new List<SaudaExtensionPolicyExportDto>();
            try
            {
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    string reportQuery = "";
                    if (verticalId > 0)
                    {
                        reportQuery = @"select (o.Name+'-'+so.Code+'/'+d.Code+'/'+div.Code) as OilType ,state.StateName as State,s.IsActive
                            ,s.ExtensionDays as Days,CONVERT(VARCHAR(10), s.ValidFrom, 103) as ValidFrom,CONVERT(VARCHAR(10), s.ValidTo, 103) as ValidTo from SaudaExtensions as s
                             join OilTypes as o on s.OilTypeId = o.Id
                             join States as state on s.StateId = state.Id
                             join SalesOrganizations so on o.SalesOrganizationId=so.Id
							 join DistributionChannels d on o.DistributionChannelId=d.Id
							 join Divisions div on o.DivisionId=div.Id
                             order by s.CreatedDate desc";

                        result = conn.Query<SaudaExtensionPolicyExportDto>(reportQuery, new
                        {
                            VerticalId = verticalId
                        }).ToList();
                    }
                    else
                    {
                        reportQuery = @"select (o.Name+'-'+so.Code+'/'+d.Code+'/'+div.Code) as OilType ,state.StateName as State,s.IsActive
                            ,s.ExtensionDays as Days,CONVERT(VARCHAR(10), s.ValidFrom, 103) as ValidFrom,CONVERT(VARCHAR(10), s.ValidTo, 103) as ValidTo from SaudaExtensions as s
                             join OilTypes as o on s.OilTypeId = o.Id
                             join States as state on s.StateId = state.Id
                             join SalesOrganizations so on o.SalesOrganizationId=so.Id
							 join DistributionChannels d on o.DistributionChannelId=d.Id
							 join Divisions div on o.DivisionId=div.Id
                             order by s.CreatedDate desc";
                        result = conn.Query<SaudaExtensionPolicyExportDto>(reportQuery).ToList();
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        /// <summary>
        /// Method to Export SaudaExtension 
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public List<SaudaBookedSaudaWithExtensionDetailsExportDto> ExportSaudaExtension(SaudaExtensionFilterDtoForGrid inputDto)
        {
            _methodName = "ExportSaudaExtension";
            var saudaExtensionDetails = new List<SaudaBookedSaudaWithExtensionDetailsListDto>();
            var result = new List<SaudaBookedSaudaWithExtensionDetailsExportDto>();
            try
            {
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    string reportQuery = "";
                    if (inputDto.statusId == (int)DTO.Enums.Status.Pending)
                    {
                        reportQuery = @"select s.SaudaNumber,CONVERT(VARCHAR(10), s.SaudaValidFrom, 103) as ValidFrom,CONVERT(VARCHAR(10), s.SaudaValidTo, 103) as ValidTo,
                        s.ExtentionDateCount as SaudaExtendedDays,s.RequestDate as SaudaRequestDate,s.BasicRate,
                        s.SaudaQuantityMT,s.SaudaQuantityCase,s.PendingQuantityMT,s.PendingQuantityCase,
                        s.Remarks,s.SAPRemarks,s.IsApproval,sku.SkuName as BookedSku,u.Name as DealerName,StateTrader.Name as BdoName,ZonalTrader.Name as zonalHeadName,s.Id,s.SaudaExtensionUpdateFromSap,s.ModifiedDate,s.IsSapDataSync
                        from SaudaExtensionDetailsApprovals as s
                        join skus as sku on s.SkuCode = sku.SkuCode
                        join Users as u on s.UserCode = u.Code And sku.VerticalId = u.VerticalId
                        join UserCustomerMappings as ucm on u.Id = ucm.CustomerId
                        join UserRoles as ur on ucm.UserId = ur.UserId and RoleId = 7  
                        join Users as StateTrader on ucm.UserId = StateTrader.Id
                        join Users as ZonalTrader on StateTrader.OrganizationReportingToId = ZonalTrader.Id
                        where s.IsApproval = 0 and Cast(s.CreatedDate as Date) >= @ValidFrom and Cast(s.CreatedDate as Date) <= @ValidTo and 
                        u.SaudaBookingTypeId = 1  and (sku.VerticalId = @VerticalId or @VerticalId = 0)";
                    }
                    else
                    {
                        reportQuery = @"select s.SaudaNumber,CONVERT(VARCHAR(10), s.SaudaValidFrom, 103) as ValidFrom,CONVERT(VARCHAR(10), s.SaudaValidTo, 103) as ValidTo,
                        s.ExtentionDateCount as SaudaExtendedDays,s.RequestDate as SaudaRequestDate,s.BasicRate,
                        s.SaudaQuantityMT,s.SaudaQuantityCase,s.PendingQuantityMT,s.PendingQuantityCase,
                        s.Remarks,s.SAPRemarks,s.IsApproval,sku.SkuName as BookedSku,u.Name as DealerName,StateTrader.Name as BdoName,ZonalTrader.Name as zonalHeadName,s.Id,s.SaudaExtensionUpdateFromSap,s.ModifiedDate,s.IsSapDataSync
                        from SaudaExtensionDetailsApprovals as s
                        join skus as sku on s.SkuCode = sku.SkuCode
                        join Users as u on s.UserCode = u.Code And sku.VerticalId = u.VerticalId
                        join UserCustomerMappings as ucm on u.Id = ucm.CustomerId
                        join UserRoles as ur on ucm.UserId = ur.UserId and RoleId = 7  
                        join Users as StateTrader on ucm.UserId = StateTrader.Id
                        join Users as ZonalTrader on StateTrader.OrganizationReportingToId = ZonalTrader.Id
                        where s.IsApproval = 1 and Cast(s.CreatedDate as Date) >= @ValidFrom and Cast(s.CreatedDate as Date) <= @ValidTo and 
                        u.SaudaBookingTypeId = 1 and (sku.VerticalId = @VerticalId or @VerticalId = 0)";
                    }

                    saudaExtensionDetails = conn.Query<SaudaBookedSaudaWithExtensionDetailsListDto>(reportQuery,new {
                        ValidFrom = inputDto.ValidFrom.Date,
                        ValidTo = inputDto.ValidTo.Date,
                        VerticalId = inputDto.VerticalId
                    }).ToList();

                    var description = UtilityHelper.GetEnumDescription(DTO.Enums.Configuration.InboundInterfacenotSyncedToSAPMinutes);
                    var configuration = @"select value from Configurations where name = @key";
                    var configurationContext = conn.QueryFirstOrDefault<string>(configuration, new
                    {
                        key = description
                    });

                    result = saudaExtensionDetails.Select(a => new SaudaBookedSaudaWithExtensionDetailsExportDto
                    {
                        SaudaNumber = a.SaudaNumber,
                        ExtendedDate = a.SaudaRequestDate,
                        ExtendedDays = a.SaudaExtendedDays,
                        ValidFrom = a.ValidFrom,
                        ValidTo = a.ValidTo,
                        BaseRate = a.BasicRate,
                        SaudaQuantityCase = string.Format("{0:0.00}", a.SaudaQuantityCase),
                        SaudaQuantityMT = string.Format("{0:0.000}", a.SaudaQuantityMT),
                        PendingQuantityCase = string.Format("{0:0.00}", a.PendingQuantityCase),
                        PendingQuantityMT = string.Format("{0:0.000}", a.PendingQuantityMT),
                        Remarks = a.Remarks,
                        SAPRemarks =a.SAPRemarks,
                        BookedSku = a.BookedSku,
                        Dealer = a.DealerName,
                        StateTrader = a.BdoName,
                        ZonalTrader = a.zonalHeadName,
                        Id = a.Id,
                        SaudaExtensionUpdateFromSap = a.SaudaExtensionUpdateFromSap,
                        IsApproval = a.IsApproval,
                        ModifiedDate = a.ModifiedDate,
                        IsSapDataSync = a.IsSapDataSync
                    }).ToList();

                    foreach (var detail in result)
                    {
                        var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        TimeSpan difference = currentDate.Subtract(Convert.ToDateTime(detail.ModifiedDate));
                        bool IsSapSyncReceivedForSaudaExtensionUpdate = false;
                        string remarks = string.Empty;
                        if (difference.TotalMinutes > Convert.ToDouble(configurationContext))
                        {
                            if (detail.SaudaExtensionUpdateFromSap)
                            {
                                IsSapSyncReceivedForSaudaExtensionUpdate = true;
                                remarks = detail.SAPRemarks;
                            }
                            else
                            {
                                IsSapSyncReceivedForSaudaExtensionUpdate = false;
                                remarks = "Sauda Extension Update Sync not received from sap";
                            }
                        }
                        else
                        {
                            IsSapSyncReceivedForSaudaExtensionUpdate = detail.SaudaExtensionUpdateFromSap;
                            remarks = detail.SAPRemarks;
                        }
                        detail.SAPRemarks = remarks;
                        detail.SaudaExtensionUpdateFromSap = IsSapSyncReceivedForSaudaExtensionUpdate;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        #endregion Sauda Extention policy
        #region Delete List Creation
        public async Task<List<DeleteListCreateDto>> GetDeleteRemarksList(IdInputDto inputDto)
        {
            _methodName = "GetDeleteRemarksList";
            string apiUrl = ApiUrl.WebApiUrlListDeleteRemark;
            var result = await GetListAsync<DeleteListCreateDto>(apiUrl, inputDto);
            return result.ToList();
        }
        public async Task<AddDeleteListRemarks> AddDeleteListRemarksAsync(AddDeleteListRemarks inputDto)
        {
            _methodName = "AddDeleteListRemarksAsync";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new AddDeleteListRemarks();
            try
            {
                var apiUrl = string.Empty;
                var inputDtoJson = string.Empty;

                apiUrl = ApiUrl.WebApiUrlAddDeleteListRemark;

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
                        result.PostMessage = Helper.GetResourceString("msg_UpdateSuccess");
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
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Sauda validity and Sauda report email configuration

        public async Task<SaudaValidityAndSaudaReportMailConfigurationDto> SaudaValidityAndSaudaReportMailConfiguration(SaudaValidityAndSaudaReportMailConfigurationDto inputDto)
        {
            _methodName = "SaudaValidityAndSaudaReportMailConfiguration";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostSaudaValidityAndSaudaReportMailConfiguration, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();

                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        inputDto.PostStatus = true;
                        inputDto.PostMessage = Helper.GetResourceString("msg_SaudaValidityAndSaudaReportConfiguration");
                    }
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        inputDto.PostStatus = false;
                        inputDto.PostMessage = errorDtoResult.Message;
                    }
                }
                else
                {
                    inputDto.PostStatus = false;
                    inputDto.PostMessage = ja[0][Settings.ResponseMessage].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return inputDto;
        }

        public async Task<SaudaValidityAndSaudaReportMailConfigurationDto> GetVerticalListBasedOnSaudaValidity()
        {
            var result = new SaudaValidityAndSaudaReportMailConfigurationDto();
            _methodName = "GetVerticalListBasedOnSaudaValidity";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetVerticalListBasedOnSaudaValidity);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<List<long>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());

                        result.VerticalsBasedOnSaudaValidity = responseResult.ToList();
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


        public async Task<SaudaValidityAndSaudaReportMailConfigurationDto> GetVerticalListAndMailIds(long verticalId)
        {
            var result = new SaudaValidityAndSaudaReportMailConfigurationDto();
            _methodName = "GetVerticalListAndMailIds";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var apiUrl = ApiUrl.WebApiUrlGetVerticalListAndMails;
                if (verticalId != 0)
                {
                    var inputDtoJson = JsonHelper.ConvertObjectToJson(verticalId);
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
                            result = JsonConvert.DeserializeObject<SaudaValidityAndSaudaReportMailConfigurationDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        #region MaterialTypes

        public async Task<MaterialTypeDto> AddOrUpdateMaterialType(MaterialTypeDto inputDto)
        {
            _methodName = "AddOrUpdateMaterialType";
            var addOrUpdateMessage = inputDto.Id > 0 ? "Successfully Updated" : "Successfully added";

            return await AddOrUpdate(ApiUrl.WebApiUrlAddOrUpdateMaterialType, inputDto, addOrUpdateMessage, "Error");
        }


        public async Task<List<MaterialTypesGridDataDto>> GetMaterialTypeListAsync()
        {
            var result = new List<MaterialTypesGridDataDto>();
            _methodName = "GetMaterialTypeListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetMaterialTypeList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<List<MaterialTypesGridDataDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<MaterialTypeDto> GetMaterialTypeByIdAsync(long userId, long materialTypeId)
        {
            try
            {
                _methodName = "GetMaterialTypeByIdAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDto = new MaterialTypeDto() { UserId = userId, Id = materialTypeId };
                var inputDtoJson = JsonHelper.ConvertObjectToJson<MaterialTypeDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlMaterialTypeById, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<MaterialTypeDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new MaterialTypeDto();
        }

        public async Task<List<MaterialTypeDto>> ExportMaterialType(LoginUserIdDto inputDto)
        {
            _methodName = "ExportMaterialType";
            var result = await GetListAsync<MaterialTypeDto>(ApiUrl.WebApiUrlExportMaterialType, inputDto);
            return result.ToList();
        }
        #endregion

        #region Volume Loadability

        public async Task<VolumeLoadability> AddOrUpdateVolumeLoadability(VolumeLoadability inputDto)
        {
            _methodName = "AddOrUpdateVolumeLoadability";
            var addOrUpdateMessage = inputDto.Id > 0 ? "Successfully Updated" : "Successfully added";

            return await AddOrUpdate(ApiUrl.WebApiUrlAddOrUpdateVolumeLoadability, inputDto, addOrUpdateMessage, "Error");
        }


        public async Task<List<VolumeLoadabilityGridDataDto>> GetVolumeLoadabilityListAsync()
        {
            var result = new List<VolumeLoadabilityGridDataDto>();
            _methodName = "GetVolumeLoadabilityListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetVolumeLoadabilityList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<List<VolumeLoadabilityGridDataDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<VolumeLoadability> GetVolumeLoadabilityByIdAsync(long userId, long volumeloadabilityId)
        {
            try
            {
                _methodName = "GetVolumeLoadabilityByIdAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDto = new VolumeLoadability() { LoginUserId = userId, Id = volumeloadabilityId };
                var inputDtoJson = JsonHelper.ConvertObjectToJson<VolumeLoadability>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlVolumeLoadabilityById, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<VolumeLoadability>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new VolumeLoadability();
        }

        public async Task<List<VolumeLoadabilityGridDataDto>> ExportVolumeLoadability(LoginUserIdDto inputDto)
        {
            _methodName = "ExportVolumeLoadability";
            var result = await GetListAsync<VolumeLoadabilityGridDataDto>(ApiUrl.WebApiUrlExportVolumeLoadability, inputDto);
            return result.ToList();
        }
        #endregion

        public async Task<SaudaBookingConfigurationDto> GetSaudaBookingConfigurationDetails(string EncryptedId)
        {
            var result = new SaudaBookingConfigurationDto();
            _methodName = "GetSaudaBookingConfigurationDetails";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(EncryptedId);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSaudaBookingConfigurationDetails, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<SaudaBookingConfigurationDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());

                        result = responseResult;
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

        #region  GamificationDashboard

        public async Task<GamificationDashboardDto> GetGamificationDashboardListAsync(string inputDto)
        {
            _methodName = "GetGamificationDashboardListAsync";
            var result = await GetByEncryptId<GamificationDashboardDto>(ApiUrl.WebApiUrlGetGamificationDashboardList, inputDto);
            return result;
        }

        public async Task<GamificationDashboardDto> AddOrUpdateGamificationDashboardDetails(GamificationDashboardDto gamificationDashboardDto)
        {
            _methodName = "AddOrUpdateGamificationDashboardDetails";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new GamificationDashboardDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                apiUrl = ApiUrl.WebApiUrlPostAddOrUpdateGamificationDashboard;
                //if (contractTypeDto.Id > 0)
                //{ apiUrl = ApiUrl.WebApiUrlUpdateDeliveryDetails; }
                //else { apiUrl = ApiUrl.WebApiUrlSaveDeliveryDetails; }

                inputDtoJson = JsonHelper.ConvertObjectToJson(gamificationDashboardDto);
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
                        result.PostMessage = gamificationDashboardDto.Id > 0 ? Helper.GetResourceString("msg_UpdatedSuccessFully") : Helper.GetResourceString("msg_SavedSuccessFully");
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
        #endregion

        #region Complaint management
        public async Task<DynamicFormQuestionDetailsViewModel> SaveDynamicFormDetailsAsync(DynamicFormQuestionDetailsViewModel formQuestionDetailsViewModel)
        {
            _methodName = "SaveDynamicFormDetailsAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var formAddDto = new FormAddDto();
                var formUpdateDto = new FormUpdateDto();
                var inputDtoJson = string.Empty;

                if (formQuestionDetailsViewModel.FormId == 0)
                {
                    //if (formQuestionDetailsViewModel.SectionQuestionsList != null && formQuestionDetailsViewModel.SectionQuestionsList.Count > 0)
                    //{
                    //    foreach (var section in formQuestionDetailsViewModel.SectionQuestionsList)
                    //    {
                    //        var selectedQuestions = UtilityHelper.ConvertStringToLongList(section.SelectedQuestionsString);
                    //        //Add multiple sections and questions list
                    //        foreach (var question in selectedQuestions)
                    //        {
                    //            var sectionQuestionAddDto = new FormQuestionAddDto();
                    //            sectionQuestionAddDto.QuestionId = question;
                    //            sectionQuestionAddDto.SectionId = section.SectionId;
                    //            //sectionQuestionAddDto.OrderNo = question.orderno;
                    //            formQuestionDetailsViewModel.AddedQuestions.Add(sectionQuestionAddDto);
                    //        }
                    //    }
                    //}
                    formQuestionDetailsViewModel.SectionQuestions = formQuestionDetailsViewModel.SectionQuestionsList;
                    if (!string.IsNullOrEmpty(formQuestionDetailsViewModel.FormUserString))
                    {
                        formQuestionDetailsViewModel.FormUsers = UtilityHelper.ConvertStringToLongList(formQuestionDetailsViewModel.FormUserString);
                    }

                    formAddDto = new FormAddDto
                    {
                        FormName = formQuestionDetailsViewModel.FormName,
                        ParentFormId = formQuestionDetailsViewModel.DependentFormId,
                        FormUsers = formQuestionDetailsViewModel.FormUsers,
                      //  SectionQuestions = formQuestionDetailsViewModel.AddedQuestions,
                        IsActive = formQuestionDetailsViewModel.IsActive,
                        IsFormStatus = formQuestionDetailsViewModel.IsFormStatus,
                        LoginUserId = formQuestionDetailsViewModel.LoginUserId,
                        RoleIds = formQuestionDetailsViewModel.RoleIds,
                        SectionQuestions = formQuestionDetailsViewModel.SectionQuestions

                    };
                    inputDtoJson = JsonHelper.ConvertObjectToJson<FormAddDto>(formAddDto);
                }
                else
                {
                    if (formQuestionDetailsViewModel.SectionQuestionsList != null && formQuestionDetailsViewModel.SectionQuestionsList.Count > 0)
                    {
                        foreach (var section in formQuestionDetailsViewModel.SectionQuestionsList)
                        {
                            //var resultremoved = section.UnselectedQuestionsString.Split(new char[] { ',' }).Except(section.SelectedQuestionsString.Split(new char[] { ',' })).ToList();
                            //var resultadded = section.SelectedQuestionsString.Split(new char[] { ',' }).Except(section.UnselectedQuestionsString.Split(new char[] { ',' })).ToList();
                            //var selectedList = UtilityHelper.ConvertStringToLongList(section.SelectedQuestionsString);
                            var unselectedList = UtilityHelper.ConvertStringToLongList(section.UnselectedQuestionsString);
                            
                            //List<long> removedList = resultremoved.Select(s => long.Parse(s)).ToList();
                            //List<long> addedList = resultadded.Select(s => long.Parse(s)).ToList();

                            var addedList = UtilityHelper.ConvertStringToLongList(section.SelectedQuestionsString);

                            foreach (var questionId in addedList)
                            {
                                var sectionQuestionAddDto = new FormQuestionAddDto
                                {
                                    QuestionId = questionId,
                                    SectionId = section.SectionId
                                };
                                //sectionQuestionAddDto.QuestionId = question;
                                //sectionQuestionAddDto.SectionId = section.SectionId;
                                //sectionQuestionAddDto.OrderNo = question.orderno;
                                formQuestionDetailsViewModel.AddedQuestions.Add(sectionQuestionAddDto);
                            }

                            //foreach (var question in removedList)
                            //{
                            //    var sectionQuestionRemoveDto = new FormQuestionAddDto();
                            //    sectionQuestionRemoveDto.QuestionId = question;
                            //    sectionQuestionRemoveDto.SectionId = section.SectionId;
                            //    formQuestionDetailsViewModel.RemovedQuestions.Add(sectionQuestionRemoveDto);
                            //}
                            foreach (var questionId in unselectedList)
                            {
                                var sectionQuestionRemoveDto = new FormQuestionAddDto
                                {
                                    QuestionId = questionId,
                                    SectionId = section.SectionId
                                };
                                formQuestionDetailsViewModel.RemovedQuestions.Add(sectionQuestionRemoveDto);
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(formQuestionDetailsViewModel.FormUserString))
                    {
                        formQuestionDetailsViewModel.FormUsers = UtilityHelper.ConvertStringToLongList(formQuestionDetailsViewModel.FormUserString);
                    }

                    formUpdateDto = new FormUpdateDto
                    {
                        FormId = formQuestionDetailsViewModel.FormId,
                        FormName = formQuestionDetailsViewModel.FormName,
                        ParentFormId = formQuestionDetailsViewModel.DependentFormId,
                        NewQuestions = formQuestionDetailsViewModel.AddedQuestions,
                        RemovedQuestions = formQuestionDetailsViewModel.RemovedQuestions,
                        NewUsers = formQuestionDetailsViewModel.FormUsers,
                        IsActive = formQuestionDetailsViewModel.IsActive,
                        IsFormStatus = formQuestionDetailsViewModel.IsFormStatus,
                        LoginUserId = formQuestionDetailsViewModel.LoginUserId,
                        RoleIds = formQuestionDetailsViewModel.RoleIds,
                        SectionQuestions = formQuestionDetailsViewModel.SectionQuestionsList
                    };
                    inputDtoJson = JsonHelper.ConvertObjectToJson<FormUpdateDto>(formUpdateDto);
                }

                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                HttpResponseMessage response = formQuestionDetailsViewModel.FormId == 0 ? PostAsync(ApiUrl.WebApiUrlPostSaveDynamicForm, inputSring) : PutAsync(ApiUrl.WebApiUrlPutUpdateDynamicForm, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var successMessage = formQuestionDetailsViewModel.FormId == 0 ? Helper.GetResourceString("msg_DynamicFormSaveSuccess") : Helper.GetResourceString("msg_DynamicFormUpdateSuccess");

                        formQuestionDetailsViewModel.PostStatus = true;
                        formQuestionDetailsViewModel.PostMessage = successMessage;
                    }
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        formQuestionDetailsViewModel.PostStatus = false;
                        formQuestionDetailsViewModel.PostMessage = errorDtoResult.Message;
                    }
                }
                else
                {
                    formQuestionDetailsViewModel.PostStatus = false;
                    formQuestionDetailsViewModel.PostMessage = ja[0]["message"].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                formQuestionDetailsViewModel.PostStatus = false;
                formQuestionDetailsViewModel.PostMessage = Helper.GetResourceString("msg_DynamicFormSaveError");
                _logger.Error(message);
            }

            return formQuestionDetailsViewModel;
        }


        public async Task<List<QuestionTypeDto>> GetActiveQuestionTypeAsync()
        {
            var result = new List<QuestionTypeDto>();
            _methodName = "GetActiveQuestionTypeAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetQuestionTypeList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<List<QuestionTypeDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<List<FormInputDto>> GetSubmittedDetailsAsync(DynamicFormReportFilterInputDto inputDto)
        {
            var result = new List<FormInputDto>();
            _methodName = "GetSubmittedDetailsAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var encryptedInput = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSubmittedDetailsList, encryptedInput);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<List<FormInputDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        result = responseResult.ToList();
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

        public async Task<List<QuestionAnswerInput>> GetSubmittedFormDetailsbyId(long FormId)
        {
            var result = new List<QuestionAnswerInput>();
            _methodName = "GetSubmittedFormDetailsbyId";

            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson(new { FormId });
                var encryptedInput = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSubmittedFormDetailsbyId, encryptedInput);

                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);

                var jsonArray = JArray.Parse($"[{responseData}]");

                if (response.IsSuccessStatusCode)
                {
                    var encryptedResponse = jsonArray[0]["Y77T3XP2B"]?.ToString();
                    if (!string.IsNullOrEmpty(encryptedResponse))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(encryptedResponse, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var dataArray = JArray.Parse($"[{decryptedString}]");
                        var responseResult = JsonConvert.DeserializeObject<List<QuestionAnswerInput>>(dataArray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());

                        result = responseResult;
                    }
                    else
                    {
                        _logger.Warn($"{ServiceName} Controller-Method {_methodName} - No data found in the response.");
                    }
                }
                else
                {
                    _logger.Warn($"{ServiceName} Controller-Method {_methodName} - Response status code is not successful: {response.StatusCode}");
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }

            return result;
        }

        public async Task<SectionModel> GetSectionDetailsById(long SectionId)
        {
            _methodName = "GetSectionDetailsById";
            string apiUrl = ApiUrl.WebApiUrlGetSectionDetail;

            SectionIdDto inputDto = new SectionIdDto()
            {
                SectionId = SectionId
            };
            var result = await GetByInputDto<SectionModel>(apiUrl, inputDto);
            return result;
        }

        public async Task<CMSQuestionModel> GetQuestionDetailsById(long QuestionId)
        {
            _methodName = "GetQuestionDetailsById";
            string apiUrl = ApiUrl.WebApiUrlGetQuestionDetail;

            QuestionIdDto inputDto = new QuestionIdDto()
            {
                QuestionId = QuestionId
            };
            var result = await GetByInputDto<CMSQuestionModel>(apiUrl, inputDto);
            return result;
        }

        public async Task<SectionModel> AddOrUpdateCMSSection(SectionModel inputDto)
        {
            _methodName = "AddOrUpdateCMSSection";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new SectionModel();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                if (inputDto.SectionId > 0)
                { apiUrl = ApiUrl.WebApiUrlSectionUpdate; }
                else { apiUrl = ApiUrl.WebApiUrlSectionSave; }


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
                        result.PostMessage = inputDto.SectionId > 0 ? Helper.GetResourceString("msg_SectionUpdatedSuccess") : Helper.GetResourceString("msg_SectionSavedSuccess");
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
                result.PostMessage = Helper.GetResourceString("msg_UserError");
                _logger.Error(message);
            }
            return result;
        }

        public async Task<CMSQuestionModel> AddOrUpdateCMSQuestion(CMSQuestionModel questionViewModel)
        {
            _methodName = "AddOrUpdateCMSSection";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new CMSQuestionModel();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                if (questionViewModel.QuestionId > 0)
                { apiUrl = ApiUrl.WebApiUrlQuestionUpdate; }
                else { apiUrl = ApiUrl.WebApiUrlQuestionSave; }

                if (questionViewModel.QuestionId == 0)
                {
                    if ((questionViewModel.QuestionTypeId == (int)DTO.Enums.QuestionType.MultipleChoice) || ((questionViewModel.QuestionTypeId == (int)DTO.Enums.QuestionType.SingleChoice)))
                    {
                        var Object = questionViewModel.AnswerOptionsDto.Replace("\"", "'");
                        questionViewModel.AnswerOptions = JsonConvert.DeserializeObject<List<AnswerOptionDto>>(Object, UtilityHelper.GetJsonSettings());
                    }
                    else
                    {
                        questionViewModel.AnswerOptions.Clear();
                    }
                    var addQuestionInputDto = new QuestionAddDto
                    {
                        SectionId = questionViewModel.SectionId,
                        QuestionTypeId = questionViewModel.QuestionTypeId,
                        Query = questionViewModel.Query,
                        Textlength = questionViewModel.Textlength,
                        LoginUserId = questionViewModel.LoginUserId,
                        AnswerOptions = questionViewModel.AnswerOptions,
                        IsActive = questionViewModel.IsActive,
                        Description = questionViewModel.Description,
                        IsMandatory = questionViewModel.IsMandatory
                    };
                    inputDtoJson = JsonHelper.ConvertObjectToJson<QuestionAddDto>(addQuestionInputDto);
                }
                else
                {
                    if ((questionViewModel.QuestionTypeEdit == (int)DTO.Enums.QuestionType.MultipleChoice) || ((questionViewModel.QuestionTypeEdit == (int)DTO.Enums.QuestionType.SingleChoice)))
                    {
                        var Object = questionViewModel.AnswerOptionsDto.Replace("\"", "'");
                        questionViewModel.AnswerOptions = JsonConvert.DeserializeObject<List<AnswerOptionDto>>(Object, UtilityHelper.GetJsonSettings());
                        questionViewModel.RemovedAnswerIds = UtilityHelper.ConvertStringToLongList(questionViewModel.RemovedOptionIds);
                    }
                    else
                    {
                        questionViewModel.AnswerOptions.Clear();
                    }
                    var updateQuestionInputDto = new QuestionAddDto
                    {
                        QuestionId = questionViewModel.QuestionId,
                        SectionId = questionViewModel.SectionId,
                        QuestionTypeId = questionViewModel.QuestionTypeEdit,
                        Query = questionViewModel.Query,
                        Textlength = questionViewModel.Textlength,
                        LoginUserId = questionViewModel.LoginUserId,
                        AnswerOptions = questionViewModel.AnswerOptions,
                        RemovedAnswerIds = questionViewModel.RemovedAnswerIds,
                        IsActive = questionViewModel.IsActive,
                        //Description = questionViewModel.Description,
                        IsMandatory = questionViewModel.IsMandatory
                    };
                    inputDtoJson = JsonHelper.ConvertObjectToJson<QuestionAddDto>(updateQuestionInputDto);
                }




                //inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = questionViewModel.QuestionId > 0 ? PutAsync(apiUrl, inputSring) : PostAsync(apiUrl, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();

                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        questionViewModel.PostStatus = true;
                        questionViewModel.PostMessage = questionViewModel.QuestionId > 0 ? Helper.GetResourceString("msg_questionUpdatedSuccess") : Helper.GetResourceString("msg_questionSavedSuccess");
                    }
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        questionViewModel.PostStatus = false;
                        questionViewModel.PostMessage = errorDtoResult.Message;
                    }
                }
                else
                {
                    questionViewModel.PostStatus = false;
                    questionViewModel.PostMessage = ja[0][Settings.ResponseMessage].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                questionViewModel.PostStatus = false;
                questionViewModel.PostMessage = Helper.GetResourceString("msg_UserError");
                _logger.Error(message);
            }
            return questionViewModel;
        }

        public async Task<List<SectionModel>> GetSectionList()
        {
            var result = new List<SectionModel>();
            _methodName = "GetSectionList";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetSectionList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<List<SectionModel>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());

                        result = responseResult.ToList();
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

        public async Task<List<QuestionsViewDto>> GetSectionQuestionList()
        {
            var result = new SectionQuestionsViewDto();
            _methodName = "GetSectionQuestionList";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                //var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                //var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetSectionQuestionList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<SectionQuestionsViewDto>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
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
            return result.Questions.ToList();
        }

        public async Task<List<QuestionsViewDto>> GetSectionFormQuestionList()
        {
            var result = new SectionQuestionsViewDto();
            _methodName = "GetSectionFormQuestionList";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                //var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                //var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetSectionFromQuestionList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<SectionQuestionsViewDto>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
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
            return result.Questions.ToList();
        }
        #endregion

        #region View Submitted Forms
        /// <summary>
        /// Method to get all Completed Submitted Forms List
        /// </summary>
        /// <returns></returns>
        public async Task<SubmittedFormsListViewDto> GetAllSubmittedFormsListForGridAsync(SubmittedFormsInputDto submittedFormsInputDto)
        {
            _methodName = "GetAllSubmittedFormsListForGridAsync";
            SubmittedFormsListViewDto submittedFormsListViewDto = new SubmittedFormsListViewDto();
            var result = new List<SubmittedFormShortViewDto>();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(submittedFormsInputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostViewSubmittedFormList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<SubmittedFormShortViewDto>>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());

                        submittedFormsListViewDto.SubmittedFormsShortView = result;
                        submittedFormsListViewDto.PostStatus = true;
                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        submittedFormsListViewDto.PostStatus = false;
                        submittedFormsListViewDto.PostMessage = errorDtoResult.Message;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return submittedFormsListViewDto;
        }
        public async Task<SubmittedFormViewDto> GetSubmittedFormDetailsByIdAsync(long id)
        {
            var result = new SubmittedFormViewDto();
            _methodName = "GetSubmittedFormDetailsByIdAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new SubmittedFormIdDto
                {
                    SubmittedFormId = id
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<SubmittedFormIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostViewSubmittedFormDetails, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<SubmittedFormViewDto>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());

                        if (result == null) return result;
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

        public async Task<List<FormInputDto>> ExportSubmittedFormDetails(DynamicFormReportFilterInputDto inputDto)
        {
            _methodName = "ExportSupportIssues";
            var result = await GetListAsync<FormInputDto>(ApiUrl.WebApiUrlExportsubmittedForm, inputDto);
            return result.ToList();
        }
        #endregion

        #region Pack Type

        /// <summary>
        /// Get pack type dropdown
        /// </summary>
        /// <returns></returns>
        public async Task<List<OilPackingTypeDto>> GetPackTypeddl()
        {
            _methodName = "GetPackTypeddl";
            var result = await GetListWithoutInputAsync<OilPackingTypeDto>(ApiUrl.WebApiUrlGetOilPackingGroupTypeList);
            return result.ToList();
        }

        #endregion

        #region SaudaSalesAreaRestrictionConfiguration

        public async Task<SaudaSalesAreaRestrictionDto> GetSaudaSalesAreaRestrictionConfigurationDetails(string EncryptedId)
        {
            var result = new SaudaSalesAreaRestrictionDto();
            _methodName = "GetSaudaSalesAreaRestrictionConfigurationDetails";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(EncryptedId);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSaudaSalesAreaRestrictionConfigurationDetails, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<SaudaSalesAreaRestrictionDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());

                        result = responseResult;
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

        public async Task<DataSourceResult> GetSaudaModificationListAsync(SaudaListFilterDto saudaFilterDto)
        {
            _methodName = "GetSaudaModificationListAsync";
            var response = await GetKendoGridResultAsync<SaudaListDto>(ApiUrl.WebApiUrlGetSaudhaModificationList, saudaFilterDto);
            return response;
        }

        public async Task<SaudaModificationsListsDto> SaudaModificationsDetailsById(IdInputDto inputDto)
        {
            _methodName = "SaudaModificationsDetailsById";
            var response = await GetByInputDto<SaudaModificationsListsDto>(ApiUrl.WebApiUrlGetSaudhaModificationDetailsById, inputDto);
            return response;
        }

        public async Task<SaudaListDto> GetSaudhaModificationDetails(SaudaDetailInputDto inputDto)
        {
            _methodName = "GetSaudhaModificationDetails";
            var response = await GetByInputDto<SaudaListDto>(ApiUrl.WebApiUrlGetSaudhaModificationDetails, inputDto);
            return response;
        }

        public async Task<SaudaUpdateDto> UpdateSaudhaModificationStatus(SaudaUpdateDto inputDto)
        {
            _methodName = "UpdateSaudaStatus";
            var addOrUpdateMessage = Helper.GetResourceString("msg_SaudaStatusUpdatedSuccess");
            var apiUrl = ApiUrl.WebApiUrlChangeSaudaModificationStatus;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, Helper.GetResourceString("msg_SaudaStatusUpdatedError"));
        }

        public async Task<SaudaUpdateDto> UpdateSaudhaModificationStatusForLoose(SaudaUpdateDto inputDto)
        {
            _methodName = "UpdateSaudaStatus";
            var addOrUpdateMessage = Helper.GetResourceString("msg_SaudaStatusUpdatedSuccess");
            var apiUrl = ApiUrl.WebApiUrlChangeSaudaModificationStatusForLoose;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, Helper.GetResourceString("msg_SaudaStatusUpdatedError"));
        }
    }
}