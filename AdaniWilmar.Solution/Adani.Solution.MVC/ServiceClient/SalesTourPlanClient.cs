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
using System.Data;

namespace Adani.Solution.MVC.ServiceClient
{
    public class SalesTourPlanClient : BaseClient
    {
        private const string ServiceName = "SalesTourPlan Client";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;

        #region Financial Year
        /// <summary>
        /// Method to post Financial Year
        /// </summary>
        /// <param name="FinancialYearViewModel"></param>
        /// <returns></returns>
        public async Task<FinancialYearViewModel> SaveFinancialYearAsync(FinancialYearViewModel financialYearViewModel)
        {
            _methodName = "SaveFinancialYearAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<FinancialYearViewModel>(financialYearViewModel);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = financialYearViewModel.Id > 0 ? PutAsync(ApiUrl.WebApiUrlPutFinancialYear, inputSring) : PostAsync(ApiUrl.WebApiUrlPostFinancialYear, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        financialYearViewModel.PostStatus = true;
                        financialYearViewModel.PostMessage = financialYearViewModel.Id > 0 ? Helpers.Helper.GetResourceString("msg_FinancialYearUpdateSucess") : Helpers.Helper.GetResourceString("msg_FinancialYearSaveSucess");

                    }
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        financialYearViewModel.PostStatus = false;
                        financialYearViewModel.PostMessage = errorDtoResult.Message;
                    }

                }
                else
                {
                    financialYearViewModel.PostStatus = false;
                    financialYearViewModel.PostMessage = ja[0]["message"].ToString();
                }


            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                financialYearViewModel.PostStatus = false;
                financialYearViewModel.PostMessage = Helper.GetResourceString("msg_FinancialYearSaveError");
                _logger.Error(message);
            }
            return financialYearViewModel;
        }
        /// <summary>
        /// Method to get unit of sale deatails
        /// </summary>       
        /// <returns></returns>
        public async Task<FinancialYearViewModel> GetFinancialYearDetailsAsync(long id)
        {
            var result = new FinancialYearViewModel();
            _methodName = "GetFinancialYearDetailsAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new FinancialYearIdDto
                {
                    FinancialYearid = id
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<FinancialYearIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlViewFinancialYear, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<FinancialYearViewModel>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get unit of sale list
        /// </summary>       
        /// <returns></returns>
        public async Task<IList<FinancialYearViewModel>> GetFinancialYearListAsync()
        {
            _methodName = "GetFinancialYearListAsync";

            var result = new List<FinancialYearViewModel>();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetFinancialYearList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<FinancialYearViewModel>>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
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

        #region HeadQuarters
        /// <summary>
        /// Method to post HeadQuartersAsync
        /// </summary>
        /// <param name="HeadQuartersViewModel"></param>
        /// <returns></returns>
        public async Task<HeadQuartersViewModel> HeadQuartersAsync(HeadQuartersViewModel hqViewModel)
        {
            _methodName = "HeadQuartersAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var hqAddDto = new HeadquartersAddDto();
                var hqupdateDto = new HeadquartersUpdateDto();
                var inputDtoJson = string.Empty;
                if (hqViewModel.Id == 0)
                {
                    hqAddDto = new HeadquartersAddDto
                    {
                        Name = hqViewModel.Name,
                        Address = hqViewModel.Address,
                        IsActive = hqViewModel.IsActive,
                        CreatedBy = hqViewModel.CreatedBy,
                        ZoneId = hqViewModel.ZoneId,
                        StateId = hqViewModel.StateId,
                        TerritoryId = hqViewModel.TerritoryId,
                        DistrictId = hqViewModel.DistrictId,
                        CityId = hqViewModel.CityId
                    };
                    inputDtoJson = JsonHelper.ConvertObjectToJson<HeadquartersAddDto>(hqAddDto);
                }
                else
                {
                    hqupdateDto = new HeadquartersUpdateDto
                    {
                        Name = hqViewModel.Name,
                        Address = hqViewModel.Address,
                        Id = hqViewModel.Id,
                        IsActive = hqViewModel.IsActive,
                        ModifiedBy = hqViewModel.CreatedBy,
                        ZoneId = hqViewModel.ZoneId,
                        StateId = hqViewModel.StateId,
                        TerritoryId = hqViewModel.TerritoryId,
                        DistrictId = hqViewModel.DistrictId,
                        CityId = hqViewModel.CityId
                    };
                    inputDtoJson = JsonHelper.ConvertObjectToJson<HeadquartersUpdateDto>(hqupdateDto);
                }

                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = hqViewModel.Id == 0 ? PostAsync(ApiUrl.WebApiUrlPostHeadQuarters, inputSring) : PutAsync(ApiUrl.WebApiUrlPutHeadQuarters, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var successMessage = hqViewModel.Id == 0 ? Helper.GetResourceString("msg_HeadQuartersSucess") : Helper.GetResourceString("msg_HeadQuartersUpdated");
                        hqViewModel = new HeadQuartersViewModel();
                        hqViewModel.PostStatus = true;
                        hqViewModel.PostMessage = successMessage;

                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        hqViewModel.PostStatus = false;
                        hqViewModel.PostMessage = errorDtoResult.Message;
                    }

                }
                else
                {
                    hqViewModel.PostStatus = false;
                    hqViewModel.PostMessage = ja[0]["message"].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                hqViewModel.PostStatus = false;
                hqViewModel.PostMessage = Helper.GetResourceString("msg_CategoryError");
                _logger.Error(message);
            }
            return hqViewModel;
        }

        /// <summary>
        /// Method to get Headquarter details
        /// </summary>       
        /// <returns></returns>
        public async Task<HeadQuartersViewModel> GetHeadquarterDetailsByIdAsync(long headQuarterId)
        {
            var result = new HeadQuartersViewModel();
            _methodName = "GetHeadquarterDetailsByIdAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                if (headQuarterId != 0)
                {
                    var headquartersIdDtoDto = new HeadquartersIdDto
                    {
                        HeadquartersId = headQuarterId
                    };

                    var inputDtoJson = JsonHelper.ConvertObjectToJson(headquartersIdDtoDto);
                    var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                    HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlViewHeadQuarters, inputSring);
                    var responseData = await response.Content.ReadAsStringAsync();
                    responseData = UtilityHelper.TrimStartEnd(responseData);
                    var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                    if (response.IsSuccessStatusCode)
                    {
                        if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                            var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                            result = JsonConvert.DeserializeObject<HeadQuartersViewModel>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get Headquarters all
        /// </summary>       
        /// <returns></returns>
        public async Task<List<HeadquartersDto>> GetAllHeadQuartersListAsync()
        {
            var result = new List<HeadquartersDto>();
            _methodName = "GetAllHeadQuartersListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetHeadQuarters);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<HeadquartersDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get Headquarters all Active
        /// </summary>       
        /// <returns></returns>
        public async Task<List<HeadquartersDto>> GetActiveHeadQuartersListAsync()
        {
            var result = new List<HeadquartersDto>();
            _methodName = "GetActiveHeadQuartersListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetActiveHeadQuarters);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<HeadquartersDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<List<HeadquartersDto>> ExportHeadQuarters(LoginUserIdDto inputDto)
        {
            _methodName = "ExportHeadQuarters";
            var result = await GetListAsync<HeadquartersDto>(ApiUrl.WebApiUrlExportHeadQuarters, inputDto);
            return result.ToList();
        }

        #endregion

        #region Reasons
        /// <summary>
        /// Method to post Reasons Async
        /// </summary>
        /// <param name="reasonViewModel"></param>
        /// <returns></returns>
        public async Task<ReasonsViewModel> ReasonsAsync(ReasonsViewModel reasonViewModel)
        {
            _methodName = "ReasonsAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var reasonAddDto = new ReasonAddDto();
                var reasonupdateDto = new ReasonUpdateDto();
                var inputDtoJson = string.Empty;
                if (reasonViewModel.Id == 0)
                {
                    reasonAddDto = new ReasonAddDto
                    {
                        Reason = reasonViewModel.Reason,
                        Description = reasonViewModel.Description,
                        IsActive = reasonViewModel.IsActive,
                        CreatedBy = reasonViewModel.CreatedBy
                    };
                    inputDtoJson = JsonHelper.ConvertObjectToJson<ReasonAddDto>(reasonAddDto);
                }
                else
                {
                    reasonupdateDto = new ReasonUpdateDto
                    {
                        Reason = reasonViewModel.Reason,
                        Description = reasonViewModel.Description,
                        Id = reasonViewModel.Id,
                        IsActive = reasonViewModel.IsActive,
                        ModifiedBy = reasonViewModel.CreatedBy
                    };
                    inputDtoJson = JsonHelper.ConvertObjectToJson<ReasonUpdateDto>(reasonupdateDto);
                }


                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = reasonViewModel.Id == 0 ? PostAsync(ApiUrl.WebApiUrlPostReasons, inputSring) : PutAsync(ApiUrl.WebApiUrlPutReasons, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var successMessage = reasonViewModel.Id == 0 ? Helper.GetResourceString("msg_HeadQuartersSucess") : Helper.GetResourceString("msg_HeadQuartersUpdated");
                        reasonViewModel = new ReasonsViewModel();
                        reasonViewModel.PostStatus = true;
                        reasonViewModel.PostMessage = successMessage;

                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        reasonViewModel.PostStatus = false;
                        reasonViewModel.PostMessage = errorDtoResult.Message;
                    }

                }
                else
                {
                    reasonViewModel.PostStatus = false;
                    reasonViewModel.PostMessage = ja[0]["message"].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                reasonViewModel.PostStatus = false;
                reasonViewModel.PostMessage = Helper.GetResourceString("msg_CategoryError");
                _logger.Error(message);
            }
            return reasonViewModel;
        }

        /// <summary>
        /// Method to get reason details
        /// </summary>       
        /// <returns></returns>
        public async Task<ReasonsViewModel> GetReasonDetailsByIdAsync(long reasonId)
        {
            var result = new ReasonsViewModel();
            _methodName = "GetReasonDetailsByIdAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                if (reasonId != 0)
                {
                    var headquartersIdDtoDto = new ReasonIdDto
                    {
                        ReasonId = reasonId
                    };

                    var inputDtoJson = JsonHelper.ConvertObjectToJson(headquartersIdDtoDto);
                    var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                    HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlViewReasons, inputSring);
                    var responseData = await response.Content.ReadAsStringAsync();
                    responseData = UtilityHelper.TrimStartEnd(responseData);
                    var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                    if (response.IsSuccessStatusCode)
                    {
                        if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                            var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                            result = JsonConvert.DeserializeObject<ReasonsViewModel>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get reason all
        /// </summary>       
        /// <returns></returns>
        public async Task<List<ReasonDto>> GetAllReasonsListAsync()
        {
            var result = new List<ReasonDto>();
            _methodName = "GetAllReasonsListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetReasons);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<ReasonDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get reason all Active
        /// </summary>       
        /// <returns></returns>
        public async Task<List<ReasonDto>> GetActiveReasonsListAsync()
        {
            var result = new List<ReasonDto>();
            _methodName = "GetActiveReasonsListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetActiveReasons);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<ReasonDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<MTPDealerDto> GetDealerListAsync()
        {
            var result = new MTPDealerDto();
            _methodName = "GetDealerListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetDealer);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<MTPDealerDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        #region Permanent Journey Plan
        /// <summary>
        /// Method to get Active Financial Year list
        /// </summary>       
        /// <returns></returns>
        public async Task<List<FinancialYearDto>> GetActiveFinancialYearListAsync()
        {
            var result = new List<FinancialYearDto>();
            _methodName = "GetActiveFinancialYearListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetActiveFinancialYearList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<FinancialYearDto>>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to post Permanent Journey Plan
        /// </summary>
        /// <param name="PermanentJouneyPlanViewModel"></param>
        /// <returns></returns>
        public async Task<PermanentJouneyPlanViewModel> PermanentJourneyPlanAsync(PermanentJouneyPlanViewModel journeyPlanViewModel)
        {
            _methodName = "PermanentJourneyPlanAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var permanentJouneyPlanAddDto = new PermanentJouneyPlanAddDto();
                var permanentJouneyPlanUpdateDto = new PermanentJourneyPlanUpdateDto();
                var inputDtoJson = string.Empty;

                if (journeyPlanViewModel.PJPId == 0)
                {
                    permanentJouneyPlanAddDto = new PermanentJouneyPlanAddDto
                    {
                        FinancialYearId = journeyPlanViewModel.FinancialYearId,
                        CreatedBy = journeyPlanViewModel.LoginUserId,
                        StatusId = journeyPlanViewModel.StatusId,
                        PermanentJourneyPlanDetails = journeyPlanViewModel.PermanentJourneyPlanDetailList
                    };
                    inputDtoJson = JsonHelper.ConvertObjectToJson(permanentJouneyPlanAddDto);
                }
                else
                {
                    permanentJouneyPlanUpdateDto = new PermanentJourneyPlanUpdateDto
                    {
                        FinancialYearId = journeyPlanViewModel.FinancialYearId,
                        ModifiedBy = journeyPlanViewModel.LoginUserId,
                        PermanentJourneyPlanDetails = journeyPlanViewModel.PermanentJourneyPlanDetailList,
                        PJPId = journeyPlanViewModel.PJPId,
                        StatusId = journeyPlanViewModel.StatusId,
                        Remarks = journeyPlanViewModel.Remarks,
                        ReasonIds = journeyPlanViewModel.ReasonIds,
                        IsEditedByAdmin = journeyPlanViewModel.IsEditableForAdmin
                    };
                    inputDtoJson = JsonHelper.ConvertObjectToJson(permanentJouneyPlanUpdateDto);
                }


                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = journeyPlanViewModel.PJPId == 0 ? PostAsync(ApiUrl.WebApiUrlPostAddPermanentJourneyPlan, inputSring) : PutAsync(ApiUrl.WebApiUrlPostUpdatePermanentJourneyPlan, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var successMessage = Helper.GetResourceString("msg_PermanentJourneyPlanSucess");
                        journeyPlanViewModel = new PermanentJouneyPlanViewModel();
                        journeyPlanViewModel.PostStatus = true;
                        journeyPlanViewModel.PostMessage = successMessage;
                    }
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        journeyPlanViewModel.PostStatus = false;
                        journeyPlanViewModel.PostMessage = errorDtoResult.Message;
                    }
                }
                else
                {
                    journeyPlanViewModel.PostStatus = false;
                    journeyPlanViewModel.PostMessage = ja[0]["message"].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                journeyPlanViewModel.PostStatus = false;
                journeyPlanViewModel.PostMessage = Helper.GetResourceString("msg_JourneyPlanError");
                _logger.Error(message);
            }
            return journeyPlanViewModel;
        }


        /// <summary>
        /// Method to get unit of sale deatails
        /// </summary>       
        /// <returns></returns>
        public async Task<List<PermanentJourneyPlansDto>> GetPermanentJourneyPlanList(int CreatedBy)
        {
            var result = new List<PermanentJourneyPlansDto>();
            _methodName = "GetPermanentJourneyPlanList";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new LoginUserIdDto
                {
                    LoginUserId = CreatedBy
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostPermanentJourneyPlanList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<PermanentJourneyPlansDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        public async Task<FinancialYearDto> GetCurrenntFinancialYearAsync()
        {
            var result = new FinancialYearDto();
            _methodName = "GetCurrenntFinancialYearAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetCurrenntFinancialYear);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var responseResult = JsonConvert.DeserializeObject<FinancialYearDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        /// <summary>
        /// Method to get unit of sale deatails
        /// </summary>       
        /// <returns></returns>
        public async Task<PermanentJouneyPlanViewModel> GetPermanentJourneyPlanDetailsAsync(long id)
        {
            var result = new PermanentJouneyPlanViewModel();
            _methodName = "GetPermanentJourneyPlanDetailsAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new PJPIdDto
                {
                    PJPId = id
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<PJPIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostPermanentJourneyPlanDetails, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var result1 = JsonConvert.DeserializeObject<PermanentJourneyPlanDto>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());

                        result.PermanentJourneyPlanDetailList = result1.PermanentJourneyPlanDetails;
                        result.PJPId = result1.PJPId;
                        result.CreatedBy = result1.CreatedBy;
                        result.StatusId = result1.StatusId;
                        result.Status = result1.Status;
                        result.Remarks = result1.Remarks;
                        result.ReasonIds = result1.ReasonIds;
                        result.EffectiveFrom = result1.EffectiveFrom;
                        result.EffectiveTo = result1.EffectiveTo;
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
        /// Method to get Get Pending PermanentJourneyPlan List
        /// </summary>       
        /// <returns></returns>
        public async Task<List<PermanentJourneyPlansDto>> GetPendingPermanentJourneyPlanList(int CreatedBy)
        {
            var result = new List<PermanentJourneyPlansDto>();
            _methodName = "GetPendingPermanentJourneyPlanList";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new LoginUserIdDto
                {
                    LoginUserId = CreatedBy
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostPendingPermanentJourneyPlanList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<PermanentJourneyPlansDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get all retailer list
        /// </summary>       
        /// <returns></returns>
        public async Task<List<RetailerViewDto>> GetAllDealerListForDropdownAsync()
        {
            var result = new List<RetailerViewDto>();
            _methodName = "GetAllRetailerListForDropdownAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDto = new LoginUserIdDto()
                {
                    IsToReturnInactiveData = true
                };
                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);


                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetDealerList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<RetailerViewDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

                var inputDtoJson = JsonHelper.ConvertObjectToJson<int>(stateId);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetDistrictListByStateId, inputSring);
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
        /// Method to get district list
        /// </summary>       
        /// <returns></returns>
        public async Task<List<CityDto>> GetCityListAsync()
        {
            var result = new List<CityDto>();
            _methodName = "GetDistrictListAsync";
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
                        result = JsonConvert.DeserializeObject<List<CityDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get Date Week Details list
        /// </summary>       
        /// <returns></returns>
        public async Task<List<DayOfWeekNameDto>> GetDateWeekDetailsListAsync()
        {
            var result = new List<DayOfWeekNameDto>();
            _methodName = "GetSalesPersonListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrldateweekdetails);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<DayOfWeekNameDto>>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get unit of sale deatails
        /// </summary>       
        /// <returns></returns>
        public async Task<List<MonthDto>> GetPJPMonths(long id)
        {
            var result = new List<MonthDto>();
            _methodName = "GetPJPMonths";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new FinancialYearIdDto
                {
                    FinancialYearid = id
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<FinancialYearIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetPJPMonthList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<MonthDto>>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get Approved PermanentJourneyPlan By User
        /// </summary>       
        /// <returns></returns>
        public async Task<List<PermanentJourneyPlansDto>> ApprovedPermanentJourneyPlanByUser(long id)
        {
            var result = new List<PermanentJourneyPlansDto>();
            _methodName = "ApprovedPermanentJourneyPlanByUser";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new LoginUserIdDto
                {
                    LoginUserId = id
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostApprovedPermanentJourneyPlanByUser, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<PermanentJourneyPlansDto>>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get Months By User PermanentJourneyPlan
        /// </summary>       
        /// <returns></returns>
        public async Task<List<MonthDto>> MonthsByUserPermanentJourneyPlan(long id)
        {
            var result = new List<MonthDto>();
            _methodName = "MonthsByUserPermanentJourneyPlan";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new PJPIdDto
                {
                    PJPId = id
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<PJPIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostMonthsByUserPermanentJourneyPlan, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<MonthDto>>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get Dealers By User PermanentJourneyPlan
        /// </summary>       
        /// <returns></returns>
        public async Task<List<DealerDto>> DealersByUserPermanentJourneyPlan(long id, long CityId)
        {
            var result = new List<DealerDto>();
            _methodName = "DealersByUserPermanentJourneyPlan";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new PJPIdDto
                {
                    PJPId = id,
                    CityId = CityId
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<PJPIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostDealersByUserPermanentJourneyPlan, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<DealerDto>>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<List<DealerDto>> GetNoVisitListByPJP(long id)
        {
            var result = new List<DealerDto>();
            _methodName = "GetNoVisitListByPJP";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new PJPIdDto
                {
                    PJPId = id
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<PJPIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostNoVisitByUserPermanentJourneyPlan, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<DealerDto>>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get Get Pending PermanentJourneyPlan List
        /// </summary>       
        /// <returns></returns>
        public async Task<List<PermanentJourneyPlansDto>> GetApprovedOrRejectedPJPList(int CreatedBy)
        {
            var result = new List<PermanentJourneyPlansDto>();
            _methodName = "GetApprovedOrRejectedPJPList";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new LoginUserIdDto
                {
                    LoginUserId = CreatedBy
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetApprovedOrRejectedPJPList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<PermanentJourneyPlansDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        #region Monthly Tour Plan
        /// <summary>
        /// Method to post Monthly Tour Plan
        /// </summary>
        /// <param name="MonthlyTourPlanViewModel"></param>
        /// <returns></returns>
        public async Task<MonthlyTourPlanViewModel> MonthlyTourPlanAsync(MonthlyTourPlanViewModel monthlyTourPlanViewModel)
        {
            _methodName = "MonthlyTourPlanAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var monthlyTourPlanAddDto = new MonthlyTourPlanAddDto();
                var monthlyTourPlanUpdateDto = new MonthlyTourPlanUpdateDto();
                var inputDtoJson = string.Empty;

                if (monthlyTourPlanViewModel.MTPId == 0)
                {
                    monthlyTourPlanAddDto = new MonthlyTourPlanAddDto
                    {
                        CreatedBy = monthlyTourPlanViewModel.LoginUserId,
                        PJPId = monthlyTourPlanViewModel.PJPId,
                        MonthId = monthlyTourPlanViewModel.MonthId,
                        StatusId = monthlyTourPlanViewModel.StatusId,
                        MonthlyTourPlanDetails = monthlyTourPlanViewModel.MonthlyTourPlanDetailList
                    };
                    inputDtoJson = JsonHelper.ConvertObjectToJson(monthlyTourPlanAddDto);
                }
                else
                {
                    monthlyTourPlanUpdateDto = new MonthlyTourPlanUpdateDto
                    {
                        ModifiedBy = monthlyTourPlanViewModel.LoginUserId,
                        MonthlyTourPlanDetails = monthlyTourPlanViewModel.MonthlyTourPlanDetailList,
                        MTPId = monthlyTourPlanViewModel.MTPId,
                        StatusId = monthlyTourPlanViewModel.StatusId,
                        Remarks = monthlyTourPlanViewModel.Remarks,
                        ReasonIds = monthlyTourPlanViewModel.ReasonIds,
                        IsEditedByAdmin = monthlyTourPlanViewModel.IsEditableForAdmin
                    };
                    inputDtoJson = JsonHelper.ConvertObjectToJson(monthlyTourPlanUpdateDto);
                }


                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = monthlyTourPlanViewModel.MTPId == 0 ? PostAsync(ApiUrl.WebApiUrlPostAddMonthlyTourPlan, inputSring) : PutAsync(ApiUrl.WebApiUrlPostUpdateMonthlyTourPlan, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var successMessage = Helper.GetResourceString("msg_MonthlyTourPlanSucess");
                        monthlyTourPlanViewModel = new MonthlyTourPlanViewModel();
                        monthlyTourPlanViewModel.PostStatus = true;
                        monthlyTourPlanViewModel.PostMessage = successMessage;
                    }
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        monthlyTourPlanViewModel.PostStatus = false;
                        monthlyTourPlanViewModel.PostMessage = errorDtoResult.Message;
                    }
                }
                else
                {
                    monthlyTourPlanViewModel.PostStatus = false;
                    monthlyTourPlanViewModel.PostMessage = ja[0]["message"].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                monthlyTourPlanViewModel.PostStatus = false;
                monthlyTourPlanViewModel.PostMessage = Helper.GetResourceString("msg_JourneyPlanError");
                _logger.Error(message);
            }
            return monthlyTourPlanViewModel;
        }

        /// <summary>
        /// Method to get MonthlyTourPlan details
        /// </summary>       
        /// <returns></returns>
        public async Task<MonthlyTourPlanViewModel> GetMonthlyTourPlanDetailsAsync(long id)
        {
            var result = new MonthlyTourPlanViewModel();
            _methodName = "GetMonthlyTourPlanDetailsAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new MTPIdDto
                {
                    MTPId = id
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<MTPIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostMonthlyTourPlanDetails, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var result1 = JsonConvert.DeserializeObject<MonthlyTourPlanDto>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());

                        result.MonthlyTourPlanDetailList = result1.MonthlyTourPlanDetailList;
                        result.MTPId = result1.MTPId;
                        result.LoginUserId = result1.CreatedBy;
                        result.StatusId = result1.StatusId;
                        result.Status = result1.Status;
                        result.Remarks = result1.Remarks;
                        result.PJPId = result1.PJPId;
                        result.MonthId = result1.MonthId;
                        result.ReasonIds = result1.ReasonIds;
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
        /// Method to get Monthly Tour Plan created list
        /// </summary>       
        /// <returns></returns>
        public async Task<List<MonthlyTourPlanDto>> GetMonthlyTourPlanList(long CreatedBy)
        {
            var result = new List<MonthlyTourPlanDto>();
            _methodName = "GetMonthlyTourPlanList";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new LoginUserIdDto
                {
                    LoginUserId = CreatedBy
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostMonthlyTourPlanList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<MonthlyTourPlanDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get Get Pending Monthly Tour Plan List
        /// </summary>       
        /// <returns></returns>
        public async Task<List<MonthlyTourPlanDto>> GetPendingMonthlyTourPlanList(long CreatedBy)
        {
            var result = new List<MonthlyTourPlanDto>();
            _methodName = "GetPendingMonthlyTourPlanList";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new LoginUserIdDto
                {
                    LoginUserId = CreatedBy
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostPendingMonthlyTourPlanList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<MonthlyTourPlanDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get Months By User PermanentJourneyPlan
        /// </summary>       
        /// <returns></returns>
        public async Task<FinancialYearDto> MonthlyTourPlanDateCalendar(PermanentJourneyPlanDetailsDto permanentJourneyPlanDetailsDto)
        {
            var result = new FinancialYearDto();
            _methodName = "MonthlyTourPlanDateCalendar";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new PermanentJourneyPlanDetailsDto
                {
                    MonthId = permanentJourneyPlanDetailsDto.MonthId,
                    PJPId = permanentJourneyPlanDetailsDto.PJPId
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<PermanentJourneyPlanDetailsDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostMonthlyTourPlanDateCalendar, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<FinancialYearDto>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get Months By User PermanentJourneyPlan
        /// </summary>       
        /// <returns></returns>
        public async Task<List<CityDto>> CityByUserPermanentJourneyPlan(PermanentJourneyPlanDetailsDto permanentJourneyPlanDetailsDto)
        {
            var result = new List<CityDto>();
            _methodName = "CityByUserPermanentJourneyPlan";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new PermanentJourneyPlanDetailsDto
                {
                    MonthId = permanentJourneyPlanDetailsDto.MonthId,
                    PJPId = permanentJourneyPlanDetailsDto.PJPId
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<PermanentJourneyPlanDetailsDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostCityByUserPermanentJourneyPlan, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<CityDto>>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get Get Pending Monthly Tour Plan List
        /// </summary>       
        /// <returns></returns>
        public async Task<List<MonthlyTourPlanDto>> GetApprovedOrRejectedMTPList(long CreatedBy)
        {
            var result = new List<MonthlyTourPlanDto>();
            _methodName = "GetApprovedOrRejectedMTPList";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new LoginUserIdDto
                {
                    LoginUserId = CreatedBy
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetApprovedOrRejectedMTPList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<MonthlyTourPlanDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        #region Monthly Plan deviation
        /// <summary>
        /// Method to get Approved MonthlyTourPlan Details By User
        /// </summary>       
        /// <returns></returns>
        public async Task<List<MonthlyTourPlanDto>> ApprovedMonthlyTourPlanByUserDDL(long CreatedBy)
        {
            var result = new List<MonthlyTourPlanDto>();
            _methodName = "ApprovedMonthlyTourPlanDetailsByUser";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new LoginUserIdDto
                {
                    LoginUserId = CreatedBy
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostApprovedMonthlyTourPlanByUser, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<MonthlyTourPlanDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get Approved MonthlyTourPlan Details By User
        /// </summary>       
        /// <returns></returns>
        public async Task<List<MonthlyTourPlanDeviationDto>> ApprovedMonthlyTourPlanDetailsByUserAsync(long MTPId)
        {
            var result = new List<MonthlyTourPlanDeviationDto>();
            _methodName = "ApprovedMonthlyTourPlanDetailsByUserAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new MTPIdDto
                {
                    MTPId = MTPId
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<MTPIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostApprovedMonthlyTourPlanDetailsByUser, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<MonthlyTourPlanDeviationDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to post Permanent Journey Plan
        /// </summary>
        /// <param name="PermanentJouneyPlanViewModel"></param>
        /// <returns></returns>
        public async Task<MonthlyPlanDeviationViewModel> AddMonthlyPlanDeviationAsync(MonthlyPlanDeviationViewModel monthlyPlanDeviationViewModel)
        {
            _methodName = "AddMonthlyPlanDeviationAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var addMonthlyPlanDeviationDto = new AddMonthlyPlanDeviationDto();
                var inputDtoJson = string.Empty;

                addMonthlyPlanDeviationDto = new AddMonthlyPlanDeviationDto
                {
                    CreatedBy = monthlyPlanDeviationViewModel.CreatedBy,
                    monthlyPlanDeviationListDto = monthlyPlanDeviationViewModel.MonthlyPlanDeviationListDto
                };
                inputDtoJson = JsonHelper.ConvertObjectToJson(addMonthlyPlanDeviationDto);


                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostAddMonthlyPlanDeviation, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var successMessage = Helper.GetResourceString("msg_MonthlyTourPlanDeviationSuccess");
                        monthlyPlanDeviationViewModel = new MonthlyPlanDeviationViewModel();
                        monthlyPlanDeviationViewModel.PostStatus = true;
                        monthlyPlanDeviationViewModel.PostMessage = successMessage;
                    }
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        monthlyPlanDeviationViewModel.PostStatus = false;
                        monthlyPlanDeviationViewModel.PostMessage = errorDtoResult.Message;
                    }
                }
                else
                {
                    monthlyPlanDeviationViewModel.PostStatus = false;
                    monthlyPlanDeviationViewModel.PostMessage = ja[0]["message"].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                monthlyPlanDeviationViewModel.PostStatus = false;
                monthlyPlanDeviationViewModel.PostMessage = Helper.GetResourceString("msg_JourneyPlanError");
                _logger.Error(message);
            }
            return monthlyPlanDeviationViewModel;
        }

        /// <summary>
        /// Method to get Approved MonthlyTourPlan Details By User
        /// </summary>       
        /// <returns></returns>
        public async Task<List<MonthlyTourPlanDeviationDto>> PendingMonthlyPlanDeviation(long UserId)
        {
            var result = new List<MonthlyTourPlanDeviationDto>();
            _methodName = "PendingMonthlyPlanDeviation";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new LoginUserIdDto
                {
                    LoginUserId = UserId
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostPendingMonthlyPlanDeviation, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<MonthlyTourPlanDeviationDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        result.Select(w => w.Approval = MonthlyPlanDeviationStatus.Pending.ToString()).ToList();
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
        /// Method to get Approved MonthlyTourPlan Details By User
        /// </summary>       
        /// <returns></returns>
        public async Task<List<MonthlyTourPlanDeviationDto>> ApprovedMonthlyPlanDeviation(long UserId)
        {
            var result = new List<MonthlyTourPlanDeviationDto>();
            _methodName = "ApprovedMonthlyPlanDeviation";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new LoginUserIdDto
                {
                    LoginUserId = UserId
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostApprovedMonthlyPlanDeviation, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<MonthlyTourPlanDeviationDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to post Permanent Journey Plan
        /// </summary>
        /// <param name="monthlyPlanDeviationViewModel"></param>
        /// <returns></returns>
        public async Task<MonthlyPlanDeviationViewModel> UpdateMonthlyPlanDeviationAsync(MonthlyPlanDeviationViewModel monthlyPlanDeviationViewModel)
        {
            _methodName = "UpdateMonthlyPlanDeviationAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var updateMonthlyPlanDeviationDto = new MonthlyPlanDeviationUpdateDto();
                var inputDtoJson = string.Empty;

                foreach (var item in monthlyPlanDeviationViewModel.MonthlyPlanDeviationListDto)
                {
                    if (item.Approval == UtilityHelper.GetEnumDescription(MonthlyPlanDeviationStatus.Pending))
                    {
                        item.StatusId = (int)MonthlyPlanDeviationStatus.Pending;
                    }
                    else if (item.Approval == UtilityHelper.GetEnumDescription(MonthlyPlanDeviationStatus.Approved))
                    {
                        item.StatusId = (int)MonthlyPlanDeviationStatus.Approved;
                    }
                    else if (item.Approval == UtilityHelper.GetEnumDescription(MonthlyPlanDeviationStatus.Rejected))
                    {
                        item.StatusId = (int)MonthlyPlanDeviationStatus.Rejected;
                    }
                }

                updateMonthlyPlanDeviationDto = new MonthlyPlanDeviationUpdateDto
                {
                    ModifiedBy = monthlyPlanDeviationViewModel.CreatedBy,
                    monthlyPlanDeviationListDto = monthlyPlanDeviationViewModel.MonthlyPlanDeviationListDto
                };

                inputDtoJson = JsonHelper.ConvertObjectToJson(updateMonthlyPlanDeviationDto);


                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PutAsync(ApiUrl.WebApiUrlPostUpdateMonthlyPlanDeviation, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var successMessage = Helper.GetResourceString("msg_PermanentJourneyPlanSucess");
                        monthlyPlanDeviationViewModel = new MonthlyPlanDeviationViewModel();
                        monthlyPlanDeviationViewModel.PostStatus = true;
                        monthlyPlanDeviationViewModel.PostMessage = successMessage;
                    }
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        monthlyPlanDeviationViewModel.PostStatus = false;
                        monthlyPlanDeviationViewModel.PostMessage = errorDtoResult.Message;
                    }
                }
                else
                {
                    monthlyPlanDeviationViewModel.PostStatus = false;
                    monthlyPlanDeviationViewModel.PostMessage = ja[0]["message"].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                monthlyPlanDeviationViewModel.PostStatus = false;
                monthlyPlanDeviationViewModel.PostMessage = Helper.GetResourceString("msg_JourneyPlanError");
                _logger.Error(message);
            }
            return monthlyPlanDeviationViewModel;
        }

        public async Task<MonthlyPlanDeviationViewModel> CheckMonthlyPlanDeviationApproveByLoginedUser(long userId)
        {
            var result = new MonthlyPlanDeviationViewModel();
            _methodName = "CheckMonthlyPlanDeviationApproveByLoginedUser";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new LoginUserIdDto
                {
                    LoginUserId = userId
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlCheckMonthlyPlanDeviationApproveByLoginedUser, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var result1 = JsonConvert.DeserializeObject<MonthlyTourPlanDeviationDto>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());

                        result.IsApprovar = result1.IsApprove;
                        result.ApprovedBy = result1.ApprovedBy;
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

        #region User Target
        /// <summary>
        /// Method to Add or update user target
        /// </summary>       
        /// <returns></returns>
        public async Task<UserTargetDto> AddOrUpdateUserTarget(UserTargetDto userTargetDto)
        {
            _methodName = "AddOrUpdateUserTarget";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var usertargetAddDto = new AddUserTargetDto();
                var usertargetupdateDto = new UpdateUserTargetDto();
                var inputDtoJson = string.Empty;
                if (userTargetDto.Id == 0)
                {
                    usertargetAddDto = new AddUserTargetDto
                    {
                        AssignedFromId = userTargetDto.AssignedFromId,
                        AssignedToId = userTargetDto.AssignedToId,
                        FromDate = userTargetDto.FromDate,
                        ToDate = userTargetDto.ToDate,
                        OilTypeId = userTargetDto.OilTypeId,
                        SkuId = userTargetDto.SkuId,
                        TargetQuanity = userTargetDto.TargetQuanity,
                        SchemeQuanity = userTargetDto.SchemeQuanity,
                        IsActive = userTargetDto.IsActive,
                        CreatedBy = userTargetDto.LoginUserId
                    };
                    inputDtoJson = JsonHelper.ConvertObjectToJson<AddUserTargetDto>(usertargetAddDto);
                }
                else
                {
                    usertargetupdateDto = new UpdateUserTargetDto
                    {
                        Id = userTargetDto.Id,
                        AssignedFromId = userTargetDto.AssignedFromId,
                        AssignedToId = userTargetDto.AssignedToId,
                        FromDate = userTargetDto.FromDate,
                        ToDate = userTargetDto.ToDate,
                        OilTypeId = userTargetDto.OilTypeId,
                        SkuId = userTargetDto.SkuId,
                        TargetQuanity = userTargetDto.TargetQuanity,
                        SchemeQuanity = userTargetDto.SchemeQuanity,
                        IsActive = userTargetDto.IsActive,
                        ModifiedBy = userTargetDto.LoginUserId
                    };
                    inputDtoJson = JsonHelper.ConvertObjectToJson<UpdateUserTargetDto>(usertargetupdateDto);
                }

                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = userTargetDto.Id == 0 ? PostAsync(ApiUrl.WebApiUrlSaveUserTarget, inputSring) : PostAsync(ApiUrl.WebApiUrlUpdateUserTarget, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var successMessage = userTargetDto.Id == 0 ? Helper.GetResourceString("msg_TargetSucess") : Helper.GetResourceString("msg_TargetUpdated");
                        userTargetDto = new UserTargetDto();
                        userTargetDto.PostStatus = true;
                        userTargetDto.PostMessage = successMessage;

                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        userTargetDto.PostStatus = false;
                        userTargetDto.PostMessage = errorDtoResult.Message;
                    }

                }
                else
                {
                    userTargetDto.PostStatus = false;
                    userTargetDto.PostMessage = ja[0]["message"].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                userTargetDto.PostStatus = false;
                userTargetDto.PostMessage = Helper.GetResourceString("msg_CategoryError");
                _logger.Error(message);
            }
            return userTargetDto;
        }

        /// <summary>
        /// Method to get Target detail by id
        /// </summary>       
        /// <returns></returns>
        public async Task<UserTargetDto> GetTargetDetailsById(IdInputDto inputdto)
        {
            var result = new UserTargetDto();
            _methodName = "GetTargetDetailsById";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputdto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetUserTargetById, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<UserTargetDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get Target List
        /// </summary>       
        /// <returns></returns>
        public async Task<List<UserTargetDto>> GetUserTargetList(IdInputDto inputdto)
        {
            var result = new List<UserTargetDto>();
            _methodName = "GetUserTargetList";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputdto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetUserTargetList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<UserTargetDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get Assigned To list
        /// </summary>       
        /// <returns></returns>
        public async Task<List<UserMasterDto>> GetUserAssignedToList(IdInputDto inputdto)
        {
            var result = new List<UserMasterDto>();
            _methodName = "GetUserAssignedToList";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputdto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetUserAssignedToList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<UserMasterDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get Assigned To list
        /// </summary>       
        /// <returns></returns>
        public async Task<List<UserSalesSaudaTargetDetailDto>> UserSaleTargetDetail(FinancialYearIdDto inputdto)
        {
            var result = new List<UserSalesSaudaTargetDetailDto>();
            _methodName = "UserSaleTargetDetail";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputdto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostMonthsByFinancialYear, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<UserSalesSaudaTargetDetailDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to post Reasons Async
        /// </summary>
        /// <param name="userSalesSaudaTargetDto"></param>
        /// <returns></returns>
        public async Task<UserSalesSaudaTargetDto> AddUserSalesSaudaTargetAsync(UserSalesSaudaTargetDto userSalesSaudaTargetDto)
        {
            _methodName = "ReasonsAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = string.Empty;

                inputDtoJson = JsonHelper.ConvertObjectToJson<UserSalesSaudaTargetDto>(userSalesSaudaTargetDto);


                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = userSalesSaudaTargetDto.Id == 0 ? PostAsync(ApiUrl.WebApiUrlPostAddSaudaSalesTarget, inputSring) : PostAsync(ApiUrl.WebApiUrlPostUpdateSaudaSalesTarget, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var successMessage = userSalesSaudaTargetDto.Id == 0 ? Helper.GetResourceString("msg_HeadQuartersSucess") : Helper.GetResourceString("msg_HeadQuartersUpdated");
                        userSalesSaudaTargetDto = new UserSalesSaudaTargetDto();
                        userSalesSaudaTargetDto.PostStatus = true;
                        userSalesSaudaTargetDto.PostMessage = successMessage;

                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        userSalesSaudaTargetDto.PostStatus = false;
                        userSalesSaudaTargetDto.PostMessage = errorDtoResult.Message;
                    }

                }
                else
                {
                    userSalesSaudaTargetDto.PostStatus = false;
                    userSalesSaudaTargetDto.PostMessage = ja[0]["message"].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                userSalesSaudaTargetDto.PostStatus = false;
                userSalesSaudaTargetDto.PostMessage = Helper.GetResourceString("msg_CategoryError");
                _logger.Error(message);
            }
            return userSalesSaudaTargetDto;
        }

        /// <summary>
        /// Method to get Headquarters all
        /// </summary>       
        /// <returns></returns>
        public async Task<List<UserSalesSaudaTargetDto>> UserSalesSaudaTargetList()
        {
            var result = new List<UserSalesSaudaTargetDto>();
            _methodName = "UserSalesSaudaTargetList";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetListaudaSalesTarget);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<UserSalesSaudaTargetDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get Assigned To list
        /// </summary>       
        /// <returns></returns>
        public async Task<List<UserSalesSaudaTargetDetailDto>> UserSalesSaudaTargetDetailList(long userid, int financialyearid)
        {
            var result = new List<UserSalesSaudaTargetDetailDto>();
            _methodName = "UserSaleTargetDetail";
            try
            {
                var inputdto = new UserSalesSaudaTargetDto
                {
                    FinancialYearId = financialyearid,
                    UserId = userid
                };
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputdto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostListDetailSaudaSalesTarget, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<UserSalesSaudaTargetDetailDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get unit of sale deatails
        /// </summary>       
        /// <returns></returns>
        public async Task<UserSalesSaudaTargetDto> UserSalesSaudaTargetdetailbyId(IdInputDto idInputDto)
        {
            var result = new UserSalesSaudaTargetDto();
            _methodName = "UserSalesSaudaTargetdetailbyId";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson<IdInputDto>(idInputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostViewSaudaSalesTarget, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<UserSalesSaudaTargetDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        #region OilType Target

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

        public async Task<UserOilTypeTargetDto> AddUserOiltypeTarget(UserOilTypeTargetDto inputDto)
        {
            _methodName = "AddUserOiltypeTarget";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var inputDtoJson = string.Empty;
                inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = inputDto.Id == 0 ? PostAsync(ApiUrl.WebApiUrlPostAddOilTypeTarget, inputSring) : PostAsync(ApiUrl.WebApiUrlPostUpdateUserOilTypeTarget, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var successMessage = inputDto.Id == 0 ? Helper.GetResourceString("msg_UserOilTypeTargetSaveSuccess") : Helper.GetResourceString("msg_UserOilTypeTargetUpdateSuccess");
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
                inputDto.PostMessage = Helper.GetResourceString("msg_UserOilTypeTargetError");
                _logger.Error(message);
            }
            return inputDto;
        }

        public async Task<List<UserOilTypeTargetDto>> UserOiltypeTargetList()
        {
            var result = new List<UserOilTypeTargetDto>();
            _methodName = "UserOiltypeTargetList";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetOilTypeTargetList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<UserOilTypeTargetDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<List<UserTargetDetailDto>> UserOiltypeTargetDetailList(long userid, int financialyearid)
        {
            var result = new List<UserTargetDetailDto>();
            _methodName = "UserOiltypeTargetDetail";
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
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetOilTypeTargetDetailList, inputSring);
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

        public async Task<UserOilTypeTargetDto> GetUserOiltypeTargetdetailbyId(UserTargetIdDto idInputDto)
        {
            var result = new UserOilTypeTargetDto();
            _methodName = "UserOiltypeTargetdetailbyId";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(idInputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostGetUserOiltypeTargetdetailbyId, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<UserOilTypeTargetDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        #region Today Activities
        /// <summary>
        /// Method to get Get Pending Monthly Tour Plan List
        /// </summary>       
        /// <returns></returns>
        public async Task<List<MonthlyTourPlanDetailsDto>> GetTodayActivitiesListAsync(TodayActivitiesInputDto todayActivitiesInputDto)
        {
            var result = new List<MonthlyTourPlanDetailsDto>();
            _methodName = "GetTodayActivitiesListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");


                var inputDtoJson = JsonHelper.ConvertObjectToJson<TodayActivitiesInputDto>(todayActivitiesInputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostTodayActivitiesList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<MonthlyTourPlanDetailsDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to get Get Pending Sauda List
        /// </summary>       
        /// <returns></returns>
        public async Task<List<PendingSaudaDto>> GetPendingSaudaListAsync(PendingSaudaInputDto pendingSaudaInputDto)
        {
            var result = new List<PendingSaudaDto>();
            _methodName = "GetPendingSaudaListAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");


                var inputDtoJson = JsonHelper.ConvertObjectToJson<PendingSaudaInputDto>(pendingSaudaInputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostGetPendingSaudaList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<PendingSaudaDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        #region Today Activity

        public async Task<IList<ProspectiveDealerVisitDto>> GetProspectiveDealers(SalesTourPlanParamDto inputDto)
        {
            var result = await GetListAsync<ProspectiveDealerVisitDto>(ApiUrl.WebApiUrlGetProspectiveDealers, inputDto);
            return result;
        }

        public async Task<IList<PendingSaudaRemarksDto>> GetPendingSaudaRemarksList(SalesTourPlanParamDto inputDto)
        {
            var result = await GetListAsync<PendingSaudaRemarksDto>(ApiUrl.WebApiUrlGetPendingSaudaRemarksList, inputDto);
            return result;
        }

        public async Task<IList<MarketScenariosDto>> GetMarketScenariosList(SalesTourPlanParamDto inputDto)
        {
            var result = await GetListAsync<MarketScenariosDto>(ApiUrl.WebApiUrlGetMarketScenariosList, inputDto);
            return result;
        }

        public async Task<IList<BdoCompetitorsDto>> GetCompetitorsList(SalesTourPlanParamDto inputDto)
        {
            var result = await GetListAsync<BdoCompetitorsDto>(ApiUrl.WebApiUrlGetCompetitorsList, inputDto);
            return result;
        }

        public async Task<IList<BdoCompetitorsDto>> GetWholesellerCompetitorsList(SalesTourPlanParamDto inputDto)
        {
            var result = await GetListAsync<BdoCompetitorsDto>(ApiUrl.WebApiUrlGetWholesellerCompetitorsList, inputDto);
            return result;
        }

        public async Task<IList<WholesellerSecondarySalesDto>> GetSecondarySalesFortheDayByWholesellerForWeb(SecondarySalesInputDto inputDto)
        {
            var result = await GetListAsync<WholesellerSecondarySalesDto>(ApiUrl.WebApiUrlGetListWholesellerForWeb, inputDto);
            return result;
        }

        public async Task<IList<ProspectiveDealerDto>> GetProspectiveDealerList(SalesTourPlanParamDto inputDto)
        {
            var result = await GetListAsync<ProspectiveDealerDto>(ApiUrl.WebApiUrlGetProspectiveDealerList, inputDto);
            return result;
        }

        public async Task<IList<BdoCompetitorSkusDto>> GetCompetitorSkuList(SalesTourPlanParamDto inputDto)
        {
            var result = await GetListAsync<BdoCompetitorSkusDto>(ApiUrl.WebApiUrlGetCompetitorSkuList, inputDto);
            return result;
        }

        public async Task<IList<WholesellerSecondarySalesDetailOutputDto>> GetSecondarySalesDetails(WholesellerSecondarySalesInputDto inputDto)
        {
            var result = await GetListAsync<WholesellerSecondarySalesDetailOutputDto>(ApiUrl.WebApiUrlGetListWholesellerSalesDetails, inputDto);
            return result;
        }

        public async Task<IList<MonthlyTourPlanDetailsDto>> GetTodayActivitiesList(TodayActivitiesInputDto inputDto)
        {
            var result = await GetListAsync<MonthlyTourPlanDetailsDto>(ApiUrl.WebApiUrlGetTodayActivityList, inputDto);
            return result;
        }
        public async Task<IList<AttachmentFileDto>> GetFileAttachments(AttachmentInputDto inputDto)
        {
            var result = await GetListAsync<AttachmentFileDto>(ApiUrl.WebApiUrlGetFileAttachmentsList, inputDto);
            return result;
        }

        #endregion

        public async Task<DataTable> GetUserAttendence(UserAttendenceInputDto inputDto)
        {
            var result = new DataTable();
            _methodName = "GetUserAttendence";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetUserAttendenceList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<DataTable>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        #region STP History

        public async Task<SalesTourPlanPcpHistoryDto> GetSalesTourPlanPcpHistory(long id)
        {
            _methodName = "GetSalesTourPlanPcpHistory";
            var result = await GetById<SalesTourPlanPcpHistoryDto>(ApiUrl.WebApiUrlGetSalesTourPlanPcpHistory, id);
            return result;
        }

        public async Task<SalesTourPlanMtpHistoryDto> GetSalesTourPlanMtpHistory(long id)
        {
            _methodName = "GetSalesTourPlanMtpHistory";
            var result = await GetById<SalesTourPlanMtpHistoryDto>(ApiUrl.WebApiUrlGetSalesTourPlanMtpHistory, id);
            return result;
        }

        public IList<DropDownDto> GetSTPVisitType()
        {
            _methodName = "GetSTPVisitType";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");

            var resultList = new List<DropDownDto>();
            foreach (var item in Settings.EnumToList<STPVisitType>())
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

        #endregion

    }
}