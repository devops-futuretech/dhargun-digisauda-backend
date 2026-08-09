
using Dapper;
using Adani.Solution.DTO;
using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.Models;
using GMCore.Helper;
using GMCore.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Kendo.Mvc.UI;

namespace Adani.Solution.MVC.ServiceClient
{
    public class SaudaClient : BaseClient
    {
        private const string ServiceName = "Sauda Client";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;

        static string connectionString = ConfigHelper.SPConnectionString;
        #region saudaLimit
        /// <summary>
        /// Method to Get Sauda Limit List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<DataSourceResult> GetSaudaLimitListAsync(SaudaLimitInputDto inputDto)
        {
            //List<SaudaLimitRequestHistoryDto> result = new List<SaudaLimitRequestHistoryDto>();
            //try
            //{
            _methodName = "GetSaudaLimitListAsync";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");

            var response = await GetKendoGridResultAsync<SaudaLimitRequestHistoryDto>(ApiUrl.WebApiUrlPostGetSaudaLimitRequestHistory, inputDto);

            //    var inputDtoJson = JsonHelper.ConvertObjectToJson<SaudaLimitInputDto>(inputDto);
            //    var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
            //    HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostGetSaudaLimitRequestHistory, inputSring);
            //    var responseData = await response.Content.ReadAsStringAsync();
            //    responseData = UtilityHelper.TrimStartEnd(responseData);
            //    var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
            //    if (response.IsSuccessStatusCode)
            //    {
            //        if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
            //        {
            //            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
            //            var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
            //            var resultList = JsonConvert.DeserializeObject<List<SaudaLimitRequestHistoryDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
            //            if (resultList[0] != null) resultList[0].PostStatus = true;
            //            return resultList;
            //        }
            //        if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
            //        {
            //            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
            //            var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
            //            var saudaLimitRequestHistoryDto = new SaudaLimitRequestHistoryDto();
            //            saudaLimitRequestHistoryDto.PostStatus = false;
            //            saudaLimitRequestHistoryDto.PostMessage = errorDtoResult.Message;
            //            result.Add(saudaLimitRequestHistoryDto);
            //        }

            //    }
            //}
            //catch (Exception exception)
            //{
            //    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
            //    _logger.Error(message);
            //}
            return response;
        }

        /// <summary>
        /// Method to ApproveSaudaLimit
        /// </summary>
        /// <param name="saudaLimitRequestDto"></param>
        /// <returns></returns>
        public async Task<SaudaApprovalViewModel> ApproveorRejectSaudaLimit(SaudaLimitRequestDto saudaLimitRequestDto)
        {
            var saudaApprovalViewModel = new SaudaApprovalViewModel();
            _methodName = "ApproveSaudaRequestLimitAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");


                var inputDtoJson = JsonHelper.ConvertObjectToJson(saudaLimitRequestDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlApproveorRejectSaudaLimitRequest, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var successDtoResult = JsonConvert.DeserializeObject<SuccessDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        saudaApprovalViewModel.PostStatus = true;
                        saudaApprovalViewModel.PostMessage = successDtoResult.Response.ToString();

                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        saudaApprovalViewModel.PostStatus = false;
                        saudaApprovalViewModel.PostMessage = errorDtoResult.Message;
                    }

                }
                else
                {
                    saudaApprovalViewModel.PostStatus = false;
                    saudaApprovalViewModel.PostMessage = ja[0]["message"].ToString();
                }


            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return saudaApprovalViewModel;
        }
        #endregion

        #region SpecialRate
        /// <summary>
        /// Method to Get Sauda Limit List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<List<SpecialRateApprovalOutputDto>> GetSpecialRateApprovalListAsync(SpecialRateAddInputDto inputDto)
        {
            List<SpecialRateApprovalOutputDto> result = new List<SpecialRateApprovalOutputDto>();
            try
            {
                _methodName = "GetSpecialRateListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<SpecialRateAddInputDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostGetSpecialRateRequestHistory, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<SpecialRateApprovalOutputDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        if (resultList[0] != null) resultList[0].PostStatus = true;
                        return resultList;
                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        var specialRateApprovalOutputDto = new SpecialRateApprovalOutputDto();
                        specialRateApprovalOutputDto.PostStatus = false;
                        specialRateApprovalOutputDto.PostMessage = errorDtoResult.Message;
                        result.Add(specialRateApprovalOutputDto);
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
        /// Method to ApproveSpecialRate
        /// </summary>
        /// <param name="SpecialRateRequestDto"></param>
        /// <returns></returns>
        public async Task<SpecialRateViewModel> ApproveorRejectSpecialRate(SpecialRateRequestDto SpecialRateRequestDto)
        {
            var specialRateViewModel = new SpecialRateViewModel();
            _methodName = "ApproveorRejectSpecialRate";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");


                var inputDtoJson = JsonHelper.ConvertObjectToJson(SpecialRateRequestDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlApproveorRejectSpecialRateRequest, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var successDtoResult = JsonConvert.DeserializeObject<SuccessDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        specialRateViewModel.PostStatus = true;
                        specialRateViewModel.PostMessage = successDtoResult.Response.ToString();

                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        specialRateViewModel.PostStatus = false;
                        specialRateViewModel.PostMessage = errorDtoResult.Message;
                    }

                }
                else
                {
                    specialRateViewModel.PostStatus = false;
                    specialRateViewModel.PostMessage = ja[0]["message"].ToString();
                }


            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return specialRateViewModel;
        }
        #endregion

        #region CompetitorAnalysis

        /// <summary>
        /// Method to add or update CompetitorAnalysis
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<CompetitorAnalysisDto> AddOrUpdateCompetitorAnalysis(CompetitorAnalysisInputDto inputDto)
        {
            _methodName = "AddOrUpdateCompetitorAnalysis";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new CompetitorAnalysisDto();
            try
            {
                var inputDtoJson = string.Empty;
                var apiUrl = ApiUrl.WebApiUrlPostSaveCompetitorAnalysis;

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
                        result.PostMessage = Helper.GetResourceString("msg_CompetitorAnalysisSaveSuccess");
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
                result.PostMessage = Helper.GetResourceString("msg_CompetitorAnalysisError");
                _logger.Error(message);
            }
            return result;
        }

        /// <summary>
        /// Method to Get CompetitorAnalysis List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<DataSourceResult> GetCompetitorAnalysisListAsync(LoginUserIdDto inputDto)
        {
            //try
            //{
            _methodName = "GetCompetitorAnalysisListAsync";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var response = await GetKendoGridResultAsync<List<CompetitorAnalysisViewDto>>(ApiUrl.WebApiUrlGetCompetitorAnalysisList, inputDto);
            return response;
            //    var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
            //    var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
            //    HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetCompetitorAnalysisList, inputSring);
            //    var responseData = await response.Content.ReadAsStringAsync();
            //    responseData = UtilityHelper.TrimStartEnd(responseData);
            //    var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
            //    if (response.IsSuccessStatusCode)
            //    {
            //        if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
            //        {
            //            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
            //            var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
            //            var resultList = JsonConvert.DeserializeObject<List<CompetitorAnalysisViewDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
            //            return resultList;
            //        }
            //    }
            //}
            //catch (Exception exception)
            //{
            //    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
            //    _logger.Error(message);
            //}
            //return new List<CompetitorAnalysisViewDto>();
        }

        /// <summary>
        /// Method to get Get CompetitorAnalysis Details By Id
        /// </summary>
        /// <param name="idInputDto"></param>
        /// <returns></returns>
        public async Task<CompetitorAnalysisViewDto> GetCompetitorAnalysisById(IdInputDto idInputDto)
        {
            var result = new CompetitorAnalysisViewDto();
            _methodName = "GetCompetitorAnalysisDetailsById";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var apiUrl = ApiUrl.WebApiUrlGetCompetitorAnalysisById;
                if (idInputDto.Id != 0)
                {
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
                            result = JsonConvert.DeserializeObject<CompetitorAnalysisViewDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        }
                        if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                            var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                            // result.PostStatus = false;
                            // result.PostMessage = errorDtoResult.Message;
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
        /// Method to get Get CompetitorAnalysis Details By Id
        /// </summary>
        /// <param name="competitorAnalysisId"></param>
        /// <returns></returns>
        public async Task<List<CompetitorAnalysisDetailsViewDto>> GetCompetitorAnalysisDetailsListById(long competitorAnalysisId)
        {
            var result = new List<CompetitorAnalysisDetailsViewDto>();
            _methodName = "GetCompetitorAnalysisDetailsById";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var apiUrl = ApiUrl.WebApiUrlGetCompetitorAnalysisDetailsListById;
                if (competitorAnalysisId != 0)
                {
                    var inputDtoJson = JsonHelper.ConvertObjectToJson(competitorAnalysisId);
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
                            result = JsonConvert.DeserializeObject<List<CompetitorAnalysisDetailsViewDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        }
                        if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                            var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                            // result.PostStatus = false;
                            // result.PostMessage = errorDtoResult.Message;
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

        public async Task<CompetitorAnalysisApprovalDto> SaveCompetitorAnalysisApproval(CompetitorAnalysisApprovalDto inputDto)
        {
            _methodName = "ProceedCompetitorAnalysisForApproval";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new CompetitorAnalysisApprovalDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                apiUrl = ApiUrl.WebApiUrlPostSaveCompetitorAnalysisApproval;

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
                        //result.PostMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_CompetitorAnalysisUpdateSuccess") : Helper.GetResourceString("msg_CompetitorAnalysisSaveSuccess");
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
                result.PostMessage = Helper.GetResourceString("msg_CompetitorAnalysisError");
                _logger.Error(message);
            }
            return result;
        }

        public async Task<IList<CompetitorDto>> GetCompetitorListAsync(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetCompetitorListAsync";
            string apiUrl = ApiUrl.WebApiUrlGetCompetitorList;
            var response = await GetListAsync<CompetitorDto>(apiUrl, loginUserIdDto);
            return response;
        }

        #endregion

        #region Sauda Convertion

        public async Task<IList<SaudaConversionListDto>> GetSaudaConversionList(SaudaConvertionFilterDto inputDto)
        {
            _methodName = "GetSaudaConversionList";
            var response = await GetListAsync<SaudaConversionListDto>(ApiUrl.WebApiUrlGetSaudaConversionList, inputDto);
            return response;
        }

        public async Task<IList<SaudaOrderDetails>> GetSaudaConversionDetails(SaudaConversionDetailInputDto inputDto)
        {
            _methodName = "GetSaudaConversionDetails";
            var response = await GetListAsync<SaudaOrderDetails>(ApiUrl.WebApiUrlGetSaudaConversionDetails, inputDto);
            return response;
        }

        public async Task<IList<SaudaOrderDetails>> GetSaudaConversionDetailsNew(SaudaConversionDetailInputDto inputDto)
        {
            _methodName = "GetSaudaConversionDetailsNew";
            var response = await GetListAsync<SaudaOrderDetails>(ApiUrl.WebApiUrlGetSaudaConversionNewDetails, inputDto);
            return response;
        }

        public async Task<IList<SaudaConversionListDto>> GetSaudaConversionListAsync(SaudaConvertionFilterDto inputDto)
        {
            _methodName = "GetSaudaConversionListAsync";
            var response = await GetListAsync<SaudaConversionListDto>(ApiUrl.WebApiUrlGetSaudaConversionListForExport, inputDto);
            return response;
        }

        public async Task<SaudaConversionDetailForAdminDto> WebApiUrlGetSaudaConversionAllDetail(SaudaConversionDetailInputDto inputDto)
        {
            _methodName = "WebApiUrlGetSaudaConversionAllDetail";
            var response = await GetByInputDto<SaudaConversionDetailForAdminDto>(ApiUrl.WebApiUrlGetSaudaConversionAllDetail, inputDto);
            return response;
        }

        public async Task<SaudaConversionUpdateDto> ApproveSaudaConversion(SaudaConversionUpdateDto inputDto)
        {
            _methodName = "ApproveSaudaConversion";
            var approveMsg = "Sauda Conversion approved successfully";
            var errorMessage = "Sauda Conversion Error";
            var apiUrl = ApiUrl.WebApiUrlApproveSaudaConversion;
            return await AddOrUpdate(apiUrl, inputDto, approveMsg, errorMessage);
        }

        #endregion

        #region TP and RA Pricing List

        public async Task<IList<PricingDto>> GetTPandRAPricingList(PricingTPandRAInputDto inputDto)
        {
            _methodName = "GetTPandRAPricingList";
            var response = await GetListAsync<PricingDto>(ApiUrl.WebApiUrlGetTPandRAPricingList, inputDto);
            return response;
        }

        #endregion

        #region Sauda Extension

        public async Task<IList<SaudaConversionListDto>> GetSaudaExtensionList(SaudaConvertionFilterDto inputDto)
        {
            _methodName = "GetSaudaExtensionList";
            var response = await GetListAsync<SaudaConversionListDto>(ApiUrl.WebApiUrlGetSaudaExtensionList, inputDto);
            return response;
        }

        public async Task<IList<SaudaConversionWithOrderDetailListDto>> ExportSaudaExtensionList(SaudaConvertionFilterDto inputDto)
        {
            _methodName = "ExportSaudaExtensionList";
            var response = await GetListAsync<SaudaConversionWithOrderDetailListDto>(ApiUrl.WebApiUrlExportSaudaExtensionList, inputDto);
            return response;
        }

        public async Task<IList<SaudaOrderDetails>> GetSaudaExtensionDetails(SaudaConversionDetailInputDto inputDto)
        {
            _methodName = "GetSaudaExtensionDetails";
            var response = await GetListAsync<SaudaOrderDetails>(ApiUrl.WebApiUrlGetSaudaExtensionDetails, inputDto);
            return response;
        }

        public async Task<SaudaConversionDetailForAdminDto> WebApiUrlGetSaudaExtensionAllDetail(SaudaConversionDetailInputDto inputDto)
        {
            _methodName = "WebApiUrlGetSaudaExtensionAllDetail";
            var response = await GetByInputDto<SaudaConversionDetailForAdminDto>(ApiUrl.WebApiUrlGetSaudaExtensionAllDetail, inputDto);
            return response;
        }

        public async Task<SaudaConversionUpdateDto> ApproveSaudaExtension(SaudaConversionUpdateDto inputDto)
        {
            _methodName = "ApproveSaudaExtension";
            var approveMsg = "Sauda Extension approved successfully";
            var errorMessage = "Sauda Extension Error";
            var apiUrl = ApiUrl.WebApiUrlApproveSaudaExtension;
            return await AddOrUpdate(apiUrl, inputDto, approveMsg, errorMessage);
        }

        #endregion

        #region Special Rate Approval

        public async Task<SpecialRateApprovalDto> SaveSpecialRateApproval(SpecialRateApprovalDto inputDto)
        {
            _methodName = "SaveSpecialRateApproval";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new SpecialRateApprovalDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                apiUrl = ApiUrl.WebApiUrlPostSaveSpecialRateApproval;

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
                        //result.PostMessage = inputDto.StatusId==(int)DTO.Enums.Status.Approved ? Helper.GetResourceString("msg_CompetitorAnalysisUpdateSuccess") : Helper.GetResourceString("msg_CompetitorAnalysisSaveSuccess");
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
                result.PostMessage = Helper.GetResourceString("msg_SpecialRateApprovalError");
                _logger.Error(message);
            }
            return result;
        }

        public async Task<List<SpecialRateApprovalOutputDto>> GetSpecialRateApprovalListWithAccessPermission(SpecialRateAddInputDto inputDto)
        {
            List<SpecialRateApprovalOutputDto> result = new List<SpecialRateApprovalOutputDto>();
            try
            {
                _methodName = "GetSpecialRateApprovalListWithAccessPermission";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<SpecialRateAddInputDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSpecialRateListWithAccessPermission, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<SpecialRateApprovalOutputDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return result;
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

        public async Task<List<DropDownDto>> GetSkuListByPackGroupId(SkuDropDownInputDto inputDto)
        {
            _methodName = "GetSkuListByPackGroupId";
            var response = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetSkuListByPackGrpId, inputDto);
            return response.ToList();
        }
        public async Task<SaudaConversionUnitAndDiffRateModel> AddSaudaConversionUnitandDiffRate(SaudaConversionUnitAndDiffRateModel inputDto)
        {
            _methodName = "AddSaudaConversionUnitandDiffRate";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new SaudaConversionUnitAndDiffRateModel();
            try
            {
                var apiUrl = string.Empty;
                var inputDtoJson = string.Empty;

                apiUrl = ApiUrl.WebApiUrlAddConversionUnitandDiffRate;

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
                        result.PostMessage = Helper.GetResourceString("msg_SavedSuccessFully");
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

        public async Task<List<SaudaConversionUnitAndDiffRateDto>> GetSaudaConversionUnitAndDiffRateList(SaudaConversionUnitAndDiffRateInputDto inputDto)
        {
            _methodName = "GetSaudaConversionUnitAndDiffRateList";
            var response = await GetListAsync<SaudaConversionUnitAndDiffRateDto>(ApiUrl.WebApiUrlGetSaudaConversionUnitAndDiffRateList, inputDto);
            return response.ToList();
        }

        /// <summary>
        /// Method to Export Sauda Conversion Unit And DiffRate
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public List<SaudaConversionUnitAndDiffRateExportDto> ExportSaudaConversionUnitAndDiffRate(ExcelExportInputDto inputDto)
        {
            _methodName = "ExportSaudaConversionUnitAndDiffRate";
            var result = new List<SaudaConversionUnitAndDiffRateExportDto>();
            var SuadaConversionList = new List<SaudaConversionUnitAndDiffRateDto>();
            try
            {
                string reportQuery = "";
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    if(inputDto.VerticalId > 0)
                    {
                        reportQuery = @"select s.FromUnit,sc.ToUnit as Unit,pf.Name as FromPackGroup,pt.Name as ToPackgroup,sf.SkuCode as FromSkuCode,sf.SkuName as FromSku,st.SkuCode as ToSkuCode,st.SkuName as ToSku,sc.BasicRate ,CONVERT(VARCHAR(10), s.FromDate, 103) as ValidFromInString,CONVERT(VARCHAR(10), s.ToDate, 103) as ValidToInString,sc.IsActive,d.Name as Source,state.StateName as State from SaudaConversionUnitAndDifferenceRates as s
                    join SaudaConversionUnitAndDifferenceRateDetails as sc on s.Id = sc.SaudaConversionUnitAndDifferenceRateId
                    join PackGroups as pf on s.FromPackGroupId = pf.Id
                    join PackGroups as pt on sc.ToPackGroupId = pt.Id
                    join Skus as sf on s.FromSkuId = sf.Id
                    join Skus as st on sc.ToSkuId = st.Id
                    join Depots as d on s.SourceId = d.Id
                    join States as state on s.StateId = state.Id
                    where Cast(@FromDate as Date) <= Cast(s.FromDate as Date)  and Cast(s.FromDate as Date) <= Cast(@ToDate as Date) and
                    Cast(@FromDate as Date) <= Cast(s.ToDate as Date) and  Cast(s.ToDate as Date) <= Cast(@ToDate as Date) and sf.VerticalId = @VerticalId";
                        SuadaConversionList = conn.Query<SaudaConversionUnitAndDiffRateDto>(reportQuery, new
                        {
                            FromDate = inputDto.StartDate,
                            ToDate = inputDto.EndDate,
                            VerticalId = inputDto.VerticalId
                        }).ToList();

                    }
                    else
                    {
                        reportQuery = @"select s.FromUnit,sc.ToUnit as Unit,pf.Name as FromPackGroup,pt.Name as ToPackgroup,sf.SkuCode as FromSkuCode,sf.SkuName as FromSku,st.SkuCode as ToSkuCode,st.SkuName as ToSku,sc.BasicRate ,CONVERT(VARCHAR(10), s.FromDate, 103) as ValidFromInString,CONVERT(VARCHAR(10), s.ToDate, 103) as ValidToInString,sc.IsActive,d.Name as Source,state.StateName as State from SaudaConversionUnitAndDifferenceRates as s
                    join SaudaConversionUnitAndDifferenceRateDetails as sc on s.Id = sc.SaudaConversionUnitAndDifferenceRateId
                    join PackGroups as pf on s.FromPackGroupId = pf.Id
                    join PackGroups as pt on sc.ToPackGroupId = pt.Id
                    join Skus as sf on s.FromSkuId = sf.Id
                    join Skus as st on sc.ToSkuId = st.Id
                    join Depots as d on s.SourceId = d.Id
                    join States as state on s.StateId = state.Id
                    where Cast(@FromDate as Date) <= Cast(s.FromDate as Date)  and Cast(s.FromDate as Date) <= Cast(@ToDate as Date) and
                    Cast(@FromDate as Date) <= Cast(s.ToDate as Date) and  Cast(s.ToDate as Date) <= Cast(@ToDate as Date)";
                        SuadaConversionList = conn.Query<SaudaConversionUnitAndDiffRateDto>(reportQuery, new
                        {
                            FromDate = inputDto.StartDate,
                            ToDate = inputDto.EndDate
                        }).ToList();


                    }

                    result = SuadaConversionList.Select(a => new SaudaConversionUnitAndDiffRateExportDto
                    {
                        FromUnit = string.Format("{0:0.000}", a.FromUnit),
                        FromPackGroup = a.FromPackGroup,
                        FromSkuCode = a.FromSkuCode,
                        FromSku = a.FromSku,
                        ToPackGroup = a.ToPackGroup,
                        ToSkuCode = a.ToSkuCode,
                        ToSku = a.ToSku,
                        ToUnit = string.Format("{0:0.000}", a.Unit),
                        ValidFrom = a.ValidFromInString,
                        ValidTo = a.ValidToInString,
                        IsActive = a.IsActive,
                        BaseRate = a.BasicRate,
                        Source = a.Source,
                        State = a.State
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

        /// <summary>
        /// Sauda Booking Restriction
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        #region Suada Booking Restriction
        public async Task<List<SaudaBookingConfigurationListDto>> GetSuadaBookingRestrictionListAsync(int UserId)
        {
            _methodName = "GetSuadaBookingRestrictionListAsync";
            UserIdDto userIdDto = new UserIdDto() { UserId = UserId };
            var response = await GetListAsync<SaudaBookingConfigurationListDto>(ApiUrl.WebApiUrlGetSaudaBookingRestrictionConfigurationList, userIdDto);
            return response.ToList();
        }

        public async Task<List<DropDownDto>> GetRoleListForSaudaBookingConfiguration()
        {
            _methodName = "GetRoleListForSaudaBookingConfiguration";
            var response = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlRolesForSaudaBookingConfigurationList);
            return response.ToList();
        }

        #endregion

        #region Sauda Sales Area Restriction

        public async Task<List<SaudaSalesAreaRestrictionListDto>> GetSuadaSalesAreaRestrictionListAsync(int UserId)
        {
            _methodName = "GetSuadaSalesAreaRestrictionListAsync";
            UserIdDto userIdDto = new UserIdDto() { UserId = UserId };
            var response = await GetListAsync<SaudaSalesAreaRestrictionListDto>(ApiUrl.WebApiUrlGetSaudaSalesAreaRestrictionConfigurationList, userIdDto);
            return response.ToList();
        }

        #endregion

        #region Sauda Modification

        public async Task<SaudaModificationUpdateDto> ChangeSaudaModificationStatus(SaudaModificationUpdateDto inputDto)
        {
            _methodName = "ChangeSaudaModificationStatus";
            var addOrUpdateMessage = Helper.GetResourceString("msg_SaudaStatusUpdatedSuccess");
            var apiUrl = ApiUrl.WebApiUrlChangeSaudaModificationStatus;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, Helper.GetResourceString("msg_SaudaStatusUpdatedError"));
        }

        #endregion
    }
}