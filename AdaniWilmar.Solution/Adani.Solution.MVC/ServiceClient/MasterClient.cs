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
using System.Data.SqlClient;
using Dapper;
using System.Text;
using System.Data;
using System.Web;
using System.IO;
using System.Web.Http.Results;

namespace Adani.Solution.MVC.ServiceClient
{
    public class MasterClient : BaseClient
    {
        private const string ServiceName = "Master Client";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;
        static string connectionString = ConfigHelper.SPConnectionString;


        #region Delivery Type

        public async Task<IList<DeliveryTypeDto>> GetDeliveryDetailsAsync(DeliveryTypeInputDto inputDto)
        {
            try
            {
                _methodName = "GetDeliveryDetailsAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<DeliveryTypeInputDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetDeliveryDetails, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<DeliveryTypeDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<DeliveryTypeDto>();
        }

        public async Task<DeliveryTypeDto> AddOrUpdateDeliveryDetails(DeliveryTypeDto deliveryInputDto)
        {
            _methodName = "AddOrUpdateDeliveryDetails";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new DeliveryTypeDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                if (deliveryInputDto.Id > 0)
                { apiUrl = ApiUrl.WebApiUrlUpdateDeliveryDetails; }
                else { apiUrl = ApiUrl.WebApiUrlSaveDeliveryDetails; }

                inputDtoJson = JsonHelper.ConvertObjectToJson(deliveryInputDto);
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
                        result.PostMessage = deliveryInputDto.Id > 0 ? Helper.GetResourceString("msg_UpdatedSuccessFully") : Helper.GetResourceString("msg_SavedSuccessFully");
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

        public List<DropDownDto> GetDeliveryTypeList()
        {
            _logger.Info("MasterClient-GetDeliveryTypeList: Get Type Master List");
            var typeList = new List<DropDownDto>();
            foreach (var unitDetailsItem in Utility.EnumToList<MasterDataTypes>())
            {
                var unitItem = new DropDownDto
                {
                    Name = Utility.GetEnumDescription(unitDetailsItem),
                    Id = (int)unitDetailsItem
                };
                typeList.Add(unitItem);
            }
            return typeList.Any() ? typeList.OrderBy(x => x.Name).ToList() : typeList;
        }

        #endregion


        #region SalesOrganization

        public async Task<IList<SalesOrganizationDto>> GetSalesOrganizationListAsync()
        {
            try
            {
                _methodName = "GetSalesOrganizationListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetSalesOrganizationList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<SalesOrganizationDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<SalesOrganizationDto>();
        }

        public async Task<SalesOrganizationDto> AddOrUpdateSalesOrganization(SalesOrganizationDto inputDto)
        {

            _methodName = "AddOrUpdateSalesOrganization";

            var apiUrl = ApiUrl.WebApiUrlPostAddorUpdateSalesOrganization;
            var result = await AddOrUpdate<SalesOrganizationDto>(apiUrl, inputDto, inputDto.Id > 0 ? Helper.GetResourceString("msg_SalesOrganizationUpdate") : Helper.GetResourceString("msg_SalesOrganizationSave"), " ");
            return result;

        }

        public async Task<SalesOrganizationDto> GetSalesOrganizationDetailsById(string EncryptedId)
        {

            _methodName = "GetSalesOrganizationDetailsById";
            var apiUrl = ApiUrl.WebApiUrlGetSalesOrganizationDetailsById;
            var result = await GetByInputDto<SalesOrganizationDto>(apiUrl, EncryptedId);
            return result;
        }

        public List<SalesOrganizationExportDto> ExportSalesOrganization(LoginUserIdDto inputDto)
        {
            var result = new List<SalesOrganizationExportDto>();
            _methodName = "ExportSalesOrganization";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                result = connection.Query<SalesOrganizationExportDto>("select Code,Name,IsActive from SalesOrganizations").ToList();
            }
            return result;
        }

        public async Task<List<SalesOrganizationddlDto>> GetAllSalesOrganizationddl()
        {
            _methodName = "GetAllSalesOrganizationddl";
            var result = new List<SalesOrganizationddlDto>();
            _methodName = "GetAllSalesOrganizationddl";
            try
            {

                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetSalesOrganization);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<SalesOrganizationddlDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                    }
                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";

            }
            return result;
        }


        #endregion

        #region SalesDocumentTypes

        public async Task<List<SalesDocumentTypeddlDto>> GetSalesDocumentTypeddl()
        {
            _methodName = "GetSalesDocumentTypeddl";

            var result = await GetAsync<List<SalesDocumentTypeddlDto>>(ApiUrl.WebApiUrlGetSalesDocumentType);
            return result.ToList();
        }
        #endregion


        #region DistributionChannel

        public async Task<IList<DistributionChannelDto>> GetDistributionChannelListAsync()
        {
            try
            {
                _methodName = "GetDistributionChannelListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetDistributionChannelList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<DistributionChannelDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<DistributionChannelDto>();
        }

        public async Task<DistributionChannelDto> AddOrUpdateDistributionChannel(DistributionChannelDto inputDto)
        {

            _methodName = "AddOrUpdateDistributionChannel";

            var apiUrl = ApiUrl.WebApiUrlPostAddorUpdateDistributionChannel;
            var result = await AddOrUpdate<DistributionChannelDto>(apiUrl, inputDto, inputDto.Id > 0 ? Helper.GetResourceString("msg_SalesOrganizationUpdate") : Helper.GetResourceString("msg_SalesOrganizationSave"), " ");
            return result;

        }

        public async Task<DistributionChannelDto> GetDistributionChannelDetailsById(string DistributionChannelId)
        {

            _methodName = "GetDistributionChannelDetailsById";
            var apiUrl = ApiUrl.WebApiUrlGetDistributionChannelDetailsById;
            var result = await GetByEncryptId<DistributionChannelDto>(apiUrl, DistributionChannelId);
            return result;
        }

        public List<DistributionChannelExportDto> ExportDistributionChannel(LoginUserIdDto inputDto)
        {
            var result = new List<DistributionChannelExportDto>();
            _methodName = "ExportDistributionChannel";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                result = connection.Query<DistributionChannelExportDto>("SELECT DistributionChannels.Name,DistributionChannels.Code,DistributionChannels.IsActive,SalesOrganizations.Name as SalesOrganization FROM DistributionChannels INNER JOIN SalesOrganizations ON DistributionChannels.SalesOrganizationId = SalesOrganizations.Id").ToList();
            }
            return result;
        }

        public async Task<List<DistributionChannelddlDto>> GetAllDistributionChannelddl(int saleId)
        {
            var id = new IdInputDto
            {
                Id = saleId,
            };
            _methodName = "GetAllDistributionChannelddl";
            var result = new List<DistributionChannelddlDto>();
            _methodName = "GetAllDistributionChannelddl";

            try
            {
                var inputDtoJson = JsonHelper.ConvertObjectToJson(id);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetDistributionChannel, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<DistributionChannelddlDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                    }
                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";

            }
            return result;
        }


        #endregion

        #region Contract Type

        public async Task<IList<ContractTypeDto>> GetContractDetailsAsync(ContractTypeInputDto inputDto)
        {
            try
            {
                _methodName = "GetContractDetailsAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<ContractTypeInputDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetContractDetails, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<ContractTypeDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<ContractTypeDto>();
        }

        public async Task<IList<UserMasterDto>> GetUsersByRoleAsync(IdInputDto inputDto)
        {
            try
            {
                _methodName = "GetUsersByRoleAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<IdInputDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetUsersByRoleList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<UserMasterDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<UserMasterDto>();
        }

        public async Task<ContractTypeDto> AddOrUpdateContractDetails(ContractTypeDto contractTypeDto)
        {
            _methodName = "AddOrUpdateContractDetails";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new ContractTypeDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                apiUrl = ApiUrl.WebApiUrlPostAddOrUpdateContract;
                //if (contractTypeDto.Id > 0)
                //{ apiUrl = ApiUrl.WebApiUrlUpdateDeliveryDetails; }
                //else { apiUrl = ApiUrl.WebApiUrlSaveDeliveryDetails; }

                inputDtoJson = JsonHelper.ConvertObjectToJson(contractTypeDto);
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
                        result.PostMessage = contractTypeDto.Id > 0 ? Helper.GetResourceString("msg_UpdatedSuccessFully") : Helper.GetResourceString("msg_SavedSuccessFully");
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

        public List<DropDownDto> GetContractTypeList()
        {
            _logger.Info("MasterClient-GetContractTypeList: Get Type Master List");
            var typeList = new List<DropDownDto>();
            foreach (var unitDetailsItem in Utility.EnumToList<SeederDataType>())
            {
                var unitItem = new DropDownDto
                {
                    Name = Utility.GetEnumDescription(unitDetailsItem),
                    Id = (int)unitDetailsItem
                };
                typeList.Add(unitItem);
            }
            return typeList.Any() ? typeList.OrderBy(x => x.Name).ToList() : typeList;
        }

        #endregion

        #region Vertical Type

        public async Task<IList<VerticalDto>> GetVerticalDetailsAsync(LoginUserIdDto inputDto)
        {
            try
            {
                _methodName = "GetVerticalDetailsAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetVerticalDetails, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<VerticalDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<VerticalDto>();
        }

        public async Task<VerticalDto> AddOrUpdateVerticalDetails(VerticalDto verticalDto)
        {
            _methodName = "AddOrUpdateVerticalDetails";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new VerticalDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                apiUrl = ApiUrl.WebApiUrlPostAddOrUpdateVertical;
                inputDtoJson = JsonHelper.ConvertObjectToJson(verticalDto);
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
                        result.PostMessage = verticalDto.Id > 0 ? Helper.GetResourceString("msg_UpdatedSuccessFully") : Helper.GetResourceString("msg_SavedSuccessFully");
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

        public async Task<List<VerticalDto>> ExportVertical(LoginUserIdDto inputDto)
        {
            _methodName = "ExportVerticalAsync";
            var result = await GetListAsync<VerticalDto>(ApiUrl.WebApiUrlExportVertical, inputDto);
            return result.ToList();
        }

        #endregion

        #region Oil Type

        public async Task<IList<OilTypeDto>> GetOilTypeDetailsAsync(LoginUserIdDto inputDto)
        {
            try
            {
                _methodName = "GetOilTypeDetailsAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetOilTypeDetails, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<OilTypeDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<OilTypeDto>();
        }

        public async Task<OilTypeDto> AddOrUpdateOilTypeDetails(OilTypeDto oilTypeDto)
        {
            _methodName = "AddOrUpdateOilTypeDetails";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new OilTypeDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                apiUrl = ApiUrl.WebApiUrlPostAddOrUpdateOilType;
                //if (contractTypeDto.Id > 0)
                //{ apiUrl = ApiUrl.WebApiUrlUpdateDeliveryDetails; }
                //else { apiUrl = ApiUrl.WebApiUrlSaveDeliveryDetails; }

                inputDtoJson = JsonHelper.ConvertObjectToJson(oilTypeDto);
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
                        result.PostMessage = oilTypeDto.Id > 0 ? Helper.GetResourceString("msg_UpdatedSuccessFully") : Helper.GetResourceString("msg_SavedSuccessFully");
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

        public async Task<List<OilTypeDto>> ExportOilType(LoginUserIdDto inputDto)
        {
            _methodName = "ExportOilType";
            var result = await GetListAsync<OilTypeDto>(ApiUrl.WebApiUrlExportOilType, inputDto);
            return result.ToList();
        }

        #endregion

        #region Plant Master

        public async Task<List<DepotDto>> GetPlantDetailsAsync(LoginUserIdDto inputDto)
        {
            try
            {
                _methodName = "GetPlantDetailsAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetPlantDetails, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<DepotDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<DepotDto>();
        }

        public async Task<DepotDto> AddOrUpdatePlantDetails(DepotDto plantDto)
        {
            _methodName = "AddOrUpdatePlantDetails";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new DepotDto();
            try
            {
                var apiUrl = string.Empty; 
                var inputDtoJson = string.Empty;
                if (!String.IsNullOrEmpty(plantDto.EncryptedId))
                { apiUrl = ApiUrl.WebApiUrlPutPlantDetails; }
                else
                { apiUrl = ApiUrl.WebApiUrlPostPlantDetails; }

                inputDtoJson = JsonHelper.ConvertObjectToJson(plantDto);
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
                        result.PostMessage = plantDto.Id > 0 ? Helper.GetResourceString("msg_PlantUpdatedSuccessfully") : Helper.GetResourceString("msg_PlantSaveSuccessfully");
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

        public async Task<DepotDto> GetPlantDetailsByIdAsync(long userId, string plantId)
        {
            try
            {
                _methodName = "GetPlantDetailsByIdAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDto = new DepotDto() { UserId = userId, EncryptedId = plantId };
                var inputDtoJson = JsonHelper.ConvertObjectToJson<DepotDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetPlantDetailsById, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<DepotDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new DepotDto();
        }
        public async Task<List<DropDownDto>> GetPlantListddlByLoginUserId(LoginUserIdDto inputDto)
        {
            try
            {
                _methodName = "GetPlantDetailsByIdAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var apiUrl = "";

                if (inputDto.RoleId == (int)DTO.Enums.Role.NationalTrader)
                {
                    apiUrl = ApiUrl.WebApiUrlGetPlantListByNH;
                }
                else if (inputDto.RoleId == (int)DTO.Enums.Role.ZonalTrader)
                {
                    apiUrl = ApiUrl.WebApiUrlGetPlantListByZH;
                }
                else if (inputDto.RoleId == (int)DTO.Enums.Role.StateTrader)
                {
                    apiUrl = ApiUrl.WebApiUrlGetPlantListByST;
                }
                else
                {
                    return await GetPlantDetailsddlAsync();
                }

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
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
        public async Task<List<DropDownDto>> GetPlantDetailsddlAsync()
        {
            try
            {
                _methodName = "GetPlantDetailsddlAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetPlantDetailsddl);
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

        public async Task<List<DropDownDto>> GetPlantDetailsddl(PlantDDLDto plant)
        {
            try
            {
                _methodName = "GetPlantDetailsddlAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson<PlantDDLDto>(plant);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetPlantDetailbsCitysddl, inputSring);
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
        public async Task<List<DepotDto>> ExportPlant(LoginUserIdDto inputDto)
        {
            _methodName = "ExportPlant";
            var result = await GetListAsync<DepotDto>(ApiUrl.WebApiUrlExportPlant, inputDto);
            return result.ToList();
        }

        #endregion

        #region Depot Master

        public async Task<IList<DepotDto>> GetDepotDetailsAsync(LoginUserIdDto loginUserIdDto)
        {
            try
            {
                _methodName = "GetDepotDetailsAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(loginUserIdDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetDepotDetails, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<DepotDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<DepotDto>();
        }

        public async Task<IList<DepotDto>> GetDepotsAndPlantsAsync(LoginUserIdDto loginUserIdDto)
        {
            try
            {
                _methodName = "GetDepotsAndPlantsAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(loginUserIdDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetDepotsAndPlants, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<DepotDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<DepotDto>();
        }

        public async Task<DepotDto> AddOrUpdateDepotDetails(DepotDto depotDto)
        {
            _methodName = "AddOrUpdatePlantDetails";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new DepotDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                if (depotDto.Id > 0)
                { apiUrl = ApiUrl.WebApiUrlPutDepotDetails; }
                else { apiUrl = ApiUrl.WebApiUrlPostDepotDetails; }

                inputDtoJson = JsonHelper.ConvertObjectToJson(depotDto);
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
                        result.PostMessage = depotDto.Id > 0 ? Helper.GetResourceString("msg_DepotUpdatedSuccessfully") : Helper.GetResourceString("msg_DepotSaveSuccessfully");
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

        public async Task<DepotDto> GetDepotDetailByIdAsync(long userId, long depotId)
        {
            try
            {
                _methodName = "GetDepotDetailByIdAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDto = new DepotDto() { UserId = userId, Id = depotId };
                var inputDtoJson = JsonHelper.ConvertObjectToJson<DepotDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetDepotDetailsById, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<DepotDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new DepotDto();
        }

        public async Task<List<DepotDto>> ExportDepot(LoginUserIdDto inputDto)
        {
            _methodName = "ExportDepot";
            var result = await GetListAsync<DepotDto>(ApiUrl.WebApiUrlExportDepot, inputDto);
            return result.ToList();
        }

        #endregion

        #region Dealer

        /// <summary>
        /// Method to add or update dealers
        /// </summary>
        /// <param name="dealerInputDto"></param>
        /// <returns></returns>    
        public async Task<EmployeeDto> AddOrUpdateDealer(EmployeeDto inputDto, IEnumerable<HttpPostedFileBase> files)
        {
            _methodName = "AddOrUpdateDealer";

            List<SupportAttachmentDto> attachments = new List<SupportAttachmentDto>();
            if (files != null)
            {
                foreach (var file in files)
                {
                    MemoryStream target = new MemoryStream();
                    file.InputStream.CopyTo(target);
                    byte[] byteArray = target.ToArray();

                    SupportAttachmentDto attachment = new SupportAttachmentDto
                    {
                        FileName = file.FileName,
                        FileExtension = Path.GetExtension(file.FileName),
                        MediaTypeId = (int)MediaType.Image,
                        FileByteArray = byteArray
                    };
                    attachments.Add(attachment);
                }
                inputDto.Attachments = attachments;
            }

            var apiUrl = !String.IsNullOrEmpty(inputDto.EncryptedId) ? ApiUrl.WebApiUrlPostUpdateUser : ApiUrl.WebApiUrlPostSaveUser;

            if (!string.IsNullOrEmpty(inputDto.SelecteDealerBrokerIdsString))
            {
                inputDto.SelectedDealerBrokerIds = UtilityHelper.ConvertStringToLongList(inputDto.SelecteDealerBrokerIdsString);
            }
            if (!string.IsNullOrEmpty(inputDto.RemovedDealerBrokerIdsString))
            {
                inputDto.RemovedDealerBrokerIds = UtilityHelper.ConvertStringToLongList(inputDto.RemovedDealerBrokerIdsString);
            }

            if (!string.IsNullOrEmpty(inputDto.SelecteDealerIdsString))
            {
                inputDto.SelectedDealerIds = UtilityHelper.ConvertStringToLongList(inputDto.SelecteDealerIdsString);
            }
            if (!string.IsNullOrEmpty(inputDto.RemovedDealerBrokerIdsString))
            {
                inputDto.RemovedDealerIds = UtilityHelper.ConvertStringToLongList(inputDto.RemovedDealerIdsString);
            }

            var msg = !String.IsNullOrEmpty(inputDto.EncryptedId) ? Helper.GetResourceString("msg_DealerUpdateSuccess") : Helper.GetResourceString("msg_DealerSaveSuccess");
            var errMsg = Helper.GetResourceString("msg_DealerError");
            return await AddOrUpdate(apiUrl, inputDto, msg, errMsg);
        }

        public async Task<BulletinDto> DeleteConsentImageAsync(int consentImageId, long loginUserId)
        {
            var bulletinDto = new BulletinDto();
            _methodName = "DeleteConsentImageAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new BulletinInputDto
                {
                    BulletinMediaId = consentImageId,
                    LoginUserId = loginUserId
                };
                var inputDtoJson = JsonHelper.ConvertObjectToJson<BulletinInputDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlDeleteConsentImage, inputSring);
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

        /// <summary>
        /// Method to Get Dealer List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<List<DealerExportDto>> GetDealerListAsync(LoginUserIdDto inputDto)
        {
            var result = new List<DealerExportDto>();
            try
            {
                _methodName = "GetDealerListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<DealerExportDto>("GetDealerExport", new { 
                            VerticalId = inputDto.VerticalId 
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                //foreach (var data in result)
                //{
                //    data.Password = !string.IsNullOrEmpty(data.Password) ? UtilityHelper.ConvertMd5ToString(data.Password, SecurityConstants.EncryptionKey) : string.Empty;
                //}

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }
        public async Task<DateRangeDTO> AddDateRange(DateRangeDTO date)
        {

            var result = new DateRangeDTO();
            _methodName = "AddDateRange";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var apiUrl = ApiUrl.WebApiUrlAddDateRange;
                if (date != null)
                {
                    var inputDtoJson = JsonHelper.ConvertObjectToJson(date);
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
                            result = JsonConvert.DeserializeObject<DateRangeDTO>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<DateRangeDTO> GetDateRange(long DealerId)
        {
            _methodName = "GetDateRange";
            string apiUrl = ApiUrl.WebApiUrlGetDateRange;
            var result = await GetById<DateRangeDTO>(apiUrl, DealerId);
            return result;
        }

        /// <summary>
        /// Method to get Get Dealer Details By Id
        /// </summary>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        public async Task<EmployeeDto> GetDealerDetailsById(string dealerId)
        {
            var result = new EmployeeDto();
            _methodName = "GetDealerDetailsById";

            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string apiUrl = ApiUrl.WebApiUrlGetDealerDetailsById;
            result = await GetByEncryptId<EmployeeDto>(apiUrl, dealerId);
            return result;

        }

        /// <summary>
        /// Method to Get Dealer and Broker List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<DealerDto>> GetDealerBrokerListAsync(LoginUserIdDto inputDto)
        {
            try
            {
                _methodName = "GetDealerBrokerListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
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
                        var resultList = JsonConvert.DeserializeObject<List<DealerDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<DealerDto>();
        }

        /// <summary>
        /// Method to Get Dealer List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<DealerBrokerDto>> GetDealersBasedOnStateAsyn(LoginUserIdDto inputDto)
        {
            try
            {
                _methodName = "GetDealersBasedOnStateAsyn";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetDealersBasedOnState, inputSring);
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

        #region Broker

        /// <summary>
        /// Method to add or update Broker
        /// </summary>
        /// <param name="dealerInputDto"></param>
        /// <returns></returns>    
        public async Task<EmployeeDto> AddOrUpdateBroker(EmployeeDto inputDto, IEnumerable<HttpPostedFileBase> files)
        {
            _methodName = "AddOrUpdateBroker";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new EmployeeDto();
            try
            {
                List<SupportAttachmentDto> attachments = new List<SupportAttachmentDto>();
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        MemoryStream target = new MemoryStream();
                        file.InputStream.CopyTo(target);
                        byte[] byteArray = target.ToArray();

                        SupportAttachmentDto attachment = new SupportAttachmentDto
                        {
                            FileName = file.FileName,
                            FileExtension = Path.GetExtension(file.FileName),
                            MediaTypeId = (int)MediaType.Image,
                            FileByteArray = byteArray
                        };
                        attachments.Add(attachment);
                    }
                    inputDto.Attachments = attachments;
                }
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                if (!String.IsNullOrEmpty(inputDto.EncryptedId))
                { apiUrl = ApiUrl.WebApiUrlPostUpdateUser; }
                else { apiUrl = ApiUrl.WebApiUrlPostSaveUser; }

                if (!string.IsNullOrEmpty(inputDto.SelecteDealerIdsString))
                {
                    inputDto.SelectedDealerIds = UtilityHelper.ConvertStringToLongList(inputDto.SelecteDealerIdsString);
                }
                if (!string.IsNullOrEmpty(inputDto.RemovedDealerBrokerIdsString))
                {
                    inputDto.RemovedDealerIds = UtilityHelper.ConvertStringToLongList(inputDto.RemovedDealerIdsString);
                }

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
                        result.PostMessage = !String.IsNullOrEmpty(inputDto.EncryptedId) ? Helper.GetResourceString("msg_BrokerUpdateSuccess") : Helper.GetResourceString("msg_BrokerSaveSuccess");
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
                result.PostMessage = Helper.GetResourceString("msg_BrokerError");
                _logger.Error(message);
            }
            return result;
        }

        public async Task<EmployeeDto> UploadProfilePhoto(EmployeeDto inputDto, IEnumerable<HttpPostedFileBase> files)
        {
            _methodName = "UploadProfilePhoto";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new EmployeeDto();
            try
            {
                List<SupportAttachmentDto> attachments = new List<SupportAttachmentDto>();
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        MemoryStream target = new MemoryStream();
                        file.InputStream.CopyTo(target);
                        byte[] byteArray = target.ToArray();

                        SupportAttachmentDto attachment = new SupportAttachmentDto
                        {
                            FileName = file.FileName,
                            FileExtension = Path.GetExtension(file.FileName),
                            MediaTypeId = (int)MediaType.Image,
                            FileByteArray = byteArray
                        };
                        attachments.Add(attachment);
                    }
                    inputDto.Attachments = attachments;
                }
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                apiUrl = ApiUrl.WebApiUrlPostUpdateProfile;

               
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
                        result.PostMessage = !String.IsNullOrEmpty(inputDto.EncryptedId) ? Helper.GetResourceString("msg_BrokerUpdateSuccess") : Helper.GetResourceString("msg_BrokerSaveSuccess");
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
                result.PostMessage = Helper.GetResourceString("msg_BrokerError");
                _logger.Error(message);
            }
            return result;
        }
        /// <summary>
        /// Method to Get Broker List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<BrokerDto>> GetBrokerListAsync(LoginUserIdDto inputDto)
        {
            try
            {
                _methodName = "GetDealerListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetBrokerList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<BrokerDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<BrokerDto>();
        }

        /// <summary>
        /// Method to get Get Broker Details By Id
        /// </summary>
        /// <param name="brokerId"></param>
        /// <returns></returns>
        public async Task<EmployeeDto> GetBrokerDetailsById(string brokerId)
        {
            var result = new EmployeeDto();
            _methodName = "GetBrokerDetailsById";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");

            string apiUrl = ApiUrl.WebApiUrlGetBrokerDetailsById;
            result = await GetByEncryptId<EmployeeDto>(apiUrl, brokerId);
            return result;

        }

        /// <summary>
        /// Method to Get Broker List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        /// 

        public async Task<IList<DropDownDto>> GetBrokerListddlAsync(DealerBrokerParamDto idInputDto)
        {
            _methodName = "GetBrokerListddlAsync";
            string apiUrl = ApiUrl.WebApiUrlGetBrokerListddl;
            return await GetListAsync<DropDownDto>(apiUrl, idInputDto);
        }

        //public async Task<List<DropDownDto>> GetBrokerListddlAsync(IdInputDto idInputDto)
        //{
        //    try
        //    {
        //        _methodName = "GetBrokerListddlAsync";
        //        _logger.Info($"{ServiceName} Controller-Method {_methodName}");

        //        var inputDtoJson = JsonHelper.ConvertObjectToJson(idInputDto);

        //        HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetBrokerListddl);
        //        var responseData = await response.Content.ReadAsStringAsync();
        //        responseData = UtilityHelper.TrimStartEnd(responseData);
        //        var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
        //        if (response.IsSuccessStatusCode)
        //        {
        //            if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
        //            {
        //                var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
        //                var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
        //                var resultList = JsonConvert.DeserializeObject<List<DropDownDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
        //                return resultList;
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //    }
        //    return new List<DropDownDto>();
        //}
        #endregion

        #region StateTrader
        public async Task<IList<DropDownDto>> GetBDOListddlAsync(List<long> idInputDto)
        {
            _methodName = "GetBDOListddlAsync";
            string apiUrl = ApiUrl.WebApiUrlGetBDOListddl;
            return await GetListAsync<DropDownDto>(apiUrl, idInputDto);
        }

        public async Task<IList<DropDownDto>> GetOverallBDOListddlAsync(LoginUserIdDto idInputDto)
        {
            _methodName = "GetOverallBDOListddlAsync";
            string apiUrl = ApiUrl.WebApiUrlGetOverallBDOListddl;
            return await GetListAsync<DropDownDto>(apiUrl, idInputDto);
        }
        #endregion

        #region User

        /// <summary>
        /// Method to add or update users
        /// </summary>
        /// <param name="dealerInputDto"></param>
        /// <returns></returns>    
        public async Task<EmployeeDto> AddOrUpdateUser(EmployeeDto inputDto)
        {
            _methodName = "AddOrUpdateUser";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new EmployeeDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                if (!string.IsNullOrEmpty(inputDto.EncryptedId))
                { apiUrl = ApiUrl.WebApiUrlPostUpdateUser; }
                else { apiUrl = ApiUrl.WebApiUrlPostSaveUser; }

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
                        result.PostMessage = !string.IsNullOrEmpty(inputDto.EncryptedId) ? Helper.GetResourceString("msg_UserUpdateSuccess") : Helper.GetResourceString("msg_UserSaveSuccess");
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

        /// <summary>
        /// Method to get User Master List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<UserMasterDto>> GetUserMasterListAsync(LoginUserIdDto inputDto)
        {
            try
            {
                _methodName = "GetDealerListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetUserMasterList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<UserMasterDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<UserMasterDto>();
        }

        /// <summary>
        /// Method to get Get User Details By Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<EmployeeDto> GetUserDetailsById(string userId)
        {
            var result = new EmployeeDto();
            _methodName = "GetUserDetailsById";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            string apiUrl = ApiUrl.WebApiUrlGetUserDetailsById;
            result = await GetByEncryptId<EmployeeDto>(apiUrl, userId);
            return result;
           
        }

        #endregion

        #region Retailer

        /// <summary>
        /// Method to add or update Retailer
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<RetailerDto> AddOrUpdateRetailer(RetailerDto inputDto)
        {
            _methodName = "AddOrUpdateRetailer";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new RetailerDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                if (inputDto.Id > 0)
                { apiUrl = ApiUrl.WebApiUrlPostUpdateRetailer; }
                else { apiUrl = ApiUrl.WebApiUrlPostSaveRetailer; }

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
                        result.PostMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_RetailerUpdateSuccess") : Helper.GetResourceString("msg_RetailerSaveSuccess");
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
                result.PostMessage = Helper.GetResourceString("msg_RetailerError");
                _logger.Error(message);
            }
            return result;
        }

        /// <summary>
        /// Method to Get Retailer List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<RetailerDto>> GetRetailerListAsync(LoginUserIdDto inputDto)
        {
            try
            {
                _methodName = "GetRetailerListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetRetailerList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<RetailerDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<RetailerDto>();
        }

        /// <summary>
        /// Method to get Get Retailer Details By Id
        /// </summary>
        /// <param name="retailerId"></param>
        /// <returns></returns>
        public async Task<RetailerDto> GetRetailerDetailsById(long retailerId)
        {
            var result = new RetailerDto();
            _methodName = "GetRetailerDetailsById";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var apiUrl = ApiUrl.WebApiUrlGetRetailerDetailsById;
                if (retailerId != 0)
                {
                    var inputDtoJson = JsonHelper.ConvertObjectToJson(retailerId);
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
                            result = JsonConvert.DeserializeObject<RetailerDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        #region Sku

        /// <summary>
        /// Method to add or update Sku
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<SkuDto> AddOrUpdateSku(SkuDto inputDto)
        {
            _methodName = "AddOrUpdateSku";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new SkuDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                if (inputDto.Id > 0)
                { apiUrl = ApiUrl.WebApiUrlPostUpdateSku; }
                else { apiUrl = ApiUrl.WebApiUrlPostSaveSku; }

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
                        result.PostMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_SkuUpdateSuccess") : Helper.GetResourceString("msg_SkuSaveSuccess");
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
                result.PostMessage = Helper.GetResourceString("msg_SkuError");
                _logger.Error(message);
            }
            return result;
        }

        /// <summary>
        /// Method to Get Sku List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<SkuDto>> GetSkuListAsync(LoginUserIdDto inputDto)
        {
            try
            {
                _methodName = "GetSkuListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSkuList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<SkuDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<SkuDto>();
        }

        /// <summary>
        /// Method to get Get Sku Details By Id
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public async Task<SkuDto> GetSkuDetailsById(string skuId)
        {
            var result = new SkuDto();
            _methodName = "GetSkuDetailsById";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var apiUrl = ApiUrl.WebApiUrlGetSkuDetailsById;
                if (!string.IsNullOrEmpty(skuId))
                {
                    var inputDtoJson = JsonHelper.ConvertObjectToJson(skuId);
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
                            result = JsonConvert.DeserializeObject<SkuDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
                result.PostMessage = Helper.GetResourceString("msg_SomeErrorOccured");
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region  ZoneMapping

        /// <summary>
        /// Method to add or update Zone
        /// </summary>
        /// <param name="dealerInputDto"></param>
        /// <returns></returns>    
        public async Task<AddorUpdateZoneDto> AddOrUpdateZoneMapping(AddorUpdateZoneDto inputDto)
        {
            _methodName = "AddOrUpdateZoneMapping";
            var addOrUpdateMessage = !String.IsNullOrEmpty(inputDto.EncryptedId)? Helper.GetResourceString("lbl_UpdateZoneSuccess") : Helper.GetResourceString("lbl_CreateZoneSuccess");
            var apiUrl =String.IsNullOrEmpty(inputDto.EncryptedId) ? ApiUrl.WebApiUrlPostZone : ApiUrl.WebApiUrlPutZone;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, "Error");
        }

        /// <summary>
        /// Method to Get Zone Mapping List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<ZoneDto>> GetZoneMappingListAsync(LoginUserIdDto inputDto)
        {
            _methodName = "GetZoneMappingListAsync";
            string apiUrl = ApiUrl.WebApiUrlGetZoneList;
            var response = await GetListAsync<ZoneDto>(apiUrl, inputDto);
            return response;
        }
        public async Task<IList<RoleDto>> GetUserRoleListAsync(LoginUserIdDto inputDto)
        {
            _methodName = "GetUserRoleListAsync";
            string apiUrl = ApiUrl.WebApiUrlGetUserRoleList;
            var response = await GetListAsync<RoleDto>(apiUrl, inputDto);
            return response;
        }

        public async Task<IList<FormInputDto>> GetQuestionsForForm(LoginUserIdDto inputDto)
        {
            _methodName = "GetZoneMappingListAsync";
            string apiUrl = ApiUrl.WebApiUrlGetSubmitFormList;
            var response = await GetListAsync<FormInputDto>(apiUrl, inputDto);
            return response;
        }

        public async Task<IList<DropDownDto>> GetZoneListForDropdown(LoginUserIdDto inputDto)
        {
            _methodName = "GetQuestionsForForm";
            string apiUrl = ApiUrl.WebApiUrlGetZoneListForDropdown;
            var response = await GetListAsync<DropDownDto>(apiUrl, inputDto);
            return response;
        }

        public async Task<List<DropDownDto>> GetStateListByZoneIdForDropdown(long zoneId)
        {
            _methodName = "GetStateListByZoneIdForDropdown";
            string apiUrl = ApiUrl.WebApiUrlGetStateListByZoneIdForDropdown;
            var response = await GetListAsync<DropDownDto>(apiUrl, zoneId);
            return response.ToList();
        }

        /// <summary>
        /// Method to Get Zone Mapping List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<DropDownDto>> GetIncoTermListAsync(LoginUserIdDto inputDto)
        {
            _methodName = "GetIncoTermListAsync";
            string apiUrl = ApiUrl.WebApiUrlGetIncoTermList;
            var response = await GetListAsync<DropDownDto>(apiUrl, inputDto);
            return response;
        }


        /// <summary>
        /// Method to get Get  Zone Mapping Details By zone Id
        /// </summary>
        /// <param name="zoneId"></param>
        /// <returns></returns>
        public async Task<AddorUpdateZoneDto> GetZoneMappingDetailsById(string zoneId)
        {
            _methodName = "GetZoneMappingDetailsById";
            string apiUrl = ApiUrl.WebApiUrlGetZone;
            var result = await GetByEncryptId<AddorUpdateZoneDto>(apiUrl, zoneId);
            return result;
        }

        public async Task<AddorUpdateZoneDto> GetNewZoneStates()
        {
            _methodName = "GetNewZoneStates";
            string apiUrl = ApiUrl.WebApiUrlNewZone;
            var result = await GetAsync<AddorUpdateZoneDto>(apiUrl);
            return result;
        }

        /// <summary>
        /// Method to get Get  Zone Mapped State List By zone Id
        /// </summary>
        /// <param name="zoneId"></param>
        /// <returns></returns>
        public async Task<IList<DropDownDto>> GetZoneMappedStatesIds(List<long> zoneIds)
        {
            _methodName = "GetZoneMappedStatesIds";
            string apiUrl = ApiUrl.WebApiUrlGetStateListByZoneIds;
            return await GetListAsync<DropDownDto>(apiUrl, zoneIds);
        }


        public async Task<IList<StateDto>> GetZoneMappedStates(long zoneId)
        {
            _methodName = "GetZoneMappedStates";
            string apiUrl = ApiUrl.WebApiUrlGetZoneStateList;
            return await GetListAsync<StateDto>(apiUrl, zoneId);
        }

        public async Task<List<ZoneDto>> ExportZone(LoginUserIdDto inputDto)
        {
            _methodName = "ExportZone";
            var result = await GetListAsync<ZoneDto>(ApiUrl.WebApiUrlExportZone, inputDto);
            return result.ToList();
        }

        #endregion

        #region Sauda
        public async Task<List<BookingTypeDto>> GetBookingTypes()
        {
            _methodName = "GetBookingTypes";
            string apiUrl = ApiUrl.WebApiUrlGetSaudhaBookingTypes;
            return await GetAsync<List<BookingTypeDto>>(apiUrl);
        }

        public async Task<List<MaterialTypesDto>> GetMaterialTypes()
        {
            _methodName = "GetMaterialTypes";
            string apiUrl = ApiUrl.WebApiUrlGetMaterialTypes;
            return await GetAsync<List<MaterialTypesDto>>(apiUrl);
        }

        public async Task<List<OilTypesDto>> GetOilTypes()
        {
            _methodName = "GetOilTypes";
            string apiUrl = ApiUrl.WebApiUrlOilTypes;
            return await GetAsync<List<OilTypesDto>>(apiUrl);
        }

        #endregion

        #region State
        /// <summary>
        /// Method to add or update State
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<StateDto> AddOrUpdateState(StateDto inputDto)
        {
            _methodName = "AddOrUpdateState";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new StateDto();
            var addStateDto = new AddStateDto();
            var updateStateDto = new UpdateStateDto();
            try
            {
                var apiUrl = string.Empty;
                var inputDtoJson = string.Empty;
                if (!String.IsNullOrEmpty(inputDto.EncryptedId))
                {
                    updateStateDto = new UpdateStateDto
                    {
                        StateId = inputDto.StateId,
                        StateName = inputDto.StateName,
                        IsActive = inputDto.IsActive,
                        ModifiedBy = inputDto.LoginUserId
                    };
                    inputDtoJson = JsonHelper.ConvertObjectToJson<UpdateStateDto>(updateStateDto);
                    apiUrl = ApiUrl.WebApiUrlPostUpdateState;
                }
                else
                {
                    addStateDto = new AddStateDto
                    {
                        StateName = inputDto.StateName,
                        IsActive = inputDto.IsActive,
                        CreatedBy = inputDto.LoginUserId
                    };
                    inputDtoJson = JsonHelper.ConvertObjectToJson<AddStateDto>(addStateDto);
                    apiUrl = ApiUrl.WebApiUrlPostSaveState;
                }

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
                        result.PostMessage = !String.IsNullOrEmpty(inputDto.EncryptedId) ? Helper.GetResourceString("msg_StateUpdateSuccess") : Helper.GetResourceString("msg_StateSaveSuccess");
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
                result.PostMessage = Helper.GetResourceString("msg_SkuError");
                _logger.Error(message);
            }
            return result;
        }

        /// <summary>
        /// Method to Get State List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<StateDto>> GetStateListAsync()
        {
            try
            {
                _methodName = "GetStateListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetStateLists);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<StateDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<StateDto>();
        }

        /// <summary>
        /// Method to get Get Sku Details By Id
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public async Task<StateDto> GetStateDetailsById(string stateid)
        {
            var result = new StateDto();
            _methodName = "GetSkuDetailsById";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var updateStateDto = new UpdateStateDto();
                var inputDtoJson = string.Empty;

                updateStateDto = new UpdateStateDto
                {
                    EncryptedId = stateid
                };

                inputDtoJson = JsonHelper.ConvertObjectToJson<UpdateStateDto>(updateStateDto);

                var apiUrl = ApiUrl.WebApiUrlGetStateDetailsById;
                if (!String.IsNullOrEmpty(stateid))
                {
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
                            result = JsonConvert.DeserializeObject<StateDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
                result.PostMessage = Helper.GetResourceString("msg_DealerError");
                _logger.Error(message);
            }
            return result;
        }

        public async Task<List<StateDto>> ExportStates(LoginUserIdDto inputDto)
        {
            _methodName = "ExportStates";
            var result = await GetListAsync<StateDto>(ApiUrl.WebApiUrlExportState, inputDto);
            return result.ToList();
        }

        #endregion

        #region District

        /// <summary>
        /// Method to add or update District
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<DistrictDto> AddOrUpdateDistrict(DistrictDto inputDto)
        {
            _methodName = "AddOrUpdateDistrict";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new DistrictDto();
            var addDto = new AddDistrictDto();
            var updateDto = new UpdateDistrictDto();
            try
            {
                var apiUrl = string.Empty;
                var inputDtoJson = string.Empty;
                if (!String.IsNullOrEmpty(inputDto.EncryptedId))
                {
                    updateDto = new UpdateDistrictDto
                    {
                        EncryptedId=inputDto.EncryptedId,
                        DistrictId = inputDto.DistrictId,
                        DistrictName = inputDto.DistrictName,
                        StateId = inputDto.StateId,
                        TerritoryId = inputDto.TerritoryId,
                        IsActive = inputDto.IsActive,
                        ModifiedBy = inputDto.LoginUserId
                    };
                    inputDtoJson = JsonHelper.ConvertObjectToJson<UpdateDistrictDto>(updateDto);
                    apiUrl = ApiUrl.WebApiUrlPostUpdateDistrict;
                }
                else
                {
                    addDto = new AddDistrictDto
                    {
                        DistrictName = inputDto.DistrictName,
                        StateId = inputDto.StateId,
                        TerritoryId = inputDto.TerritoryId,
                        IsActive = inputDto.IsActive,
                        CreatedBy = inputDto.LoginUserId
                    };
                    inputDtoJson = JsonHelper.ConvertObjectToJson<AddDistrictDto>(addDto);
                    apiUrl = ApiUrl.WebApiUrlPostSaveDistrict;
                }

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
                        result.PostMessage = !String.IsNullOrEmpty(inputDto.EncryptedId) ? Helper.GetResourceString("msg_DistrictUpdateSuccess") : Helper.GetResourceString("msg_DistrictSaveSuccess");
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
                result.PostMessage = Helper.GetResourceString("msg_SkuError");
                _logger.Error(message);
            }
            return result;
        }

        /// <summary>
        /// Method to Get District List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<DistrictDto>> GetDistrictListAsync()
        {
            try
            {
                _methodName = "GetStateListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetDistrictLists);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<DistrictDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<DistrictDto>();
        }

        /// <summary>
        /// Method to get Get districts Details By Id
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public async Task<DistrictDto> GetDistrictDetailsById(string districtid)
        {
            var result = new DistrictDto();
            _methodName = "GetDistrictDetailsById";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var updateDistrictDto = new UpdateDistrictDto();
                var inputDtoJson = string.Empty;

                updateDistrictDto = new UpdateDistrictDto
                {
                    EncryptedId=districtid
                };

                inputDtoJson = JsonHelper.ConvertObjectToJson<UpdateDistrictDto>(updateDistrictDto);

                var apiUrl = ApiUrl.WebApiUrlGetDistrictDetailsById;
                if (!String.IsNullOrEmpty(districtid))
                {
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
                            result = JsonConvert.DeserializeObject<DistrictDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
                result.PostMessage = Helper.GetResourceString("msg_DealerError");
                _logger.Error(message);
            }
            return result;
        }

        public async Task<List<DistrictDto>> ExportDistrict(LoginUserIdDto inputDto)
        {
            _methodName = "ExportDistrict";
            var result = await GetListAsync<DistrictDto>(ApiUrl.WebApiUrlExportDistrict, inputDto);
            return result.ToList();
        }

        #endregion

        #region City

        /// <summary>
        /// Method to add or update City
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<CityDto> AddOrUpdateCity(CityDto inputDto)
        {
            _methodName = "AddOrUpdateCity";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new CityDto();
            var addDto = new AddCityDto();
            var updateDto = new UpdateCityDto();
            try
            {
                var apiUrl = string.Empty;
                var inputDtoJson = string.Empty;
                if (!String.IsNullOrEmpty(inputDto.EncryptedId))
                {
                    updateDto = new UpdateCityDto
                    {
                        EncryptedId=inputDto.EncryptedId,
                        CityId = inputDto.CityId,
                        CityName = inputDto.CityName,
                        DistrictId = inputDto.DistrictId,
                        TerritoryId = inputDto.TerritoryId,
                        IsActive = inputDto.IsActive,
                        ModifiedBy = inputDto.LoginUserId
                    };
                    inputDtoJson = JsonHelper.ConvertObjectToJson<UpdateCityDto>(updateDto);
                    apiUrl = ApiUrl.WebApiUrlPostUpdateCity;
                }
                else
                {
                    addDto = new AddCityDto
                    {
                        CityName = inputDto.CityName,
                        DistrictId = inputDto.DistrictId,
                        TerritoryId = inputDto.TerritoryId,
                        IsActive = inputDto.IsActive,
                        CreatedBy = inputDto.LoginUserId
                    };
                    inputDtoJson = JsonHelper.ConvertObjectToJson<AddCityDto>(addDto);
                    apiUrl = ApiUrl.WebApiUrlPostSaveCity;
                }

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
                        result.PostMessage = !String.IsNullOrEmpty(inputDto.EncryptedId) ? Helper.GetResourceString("msg_CityUpdateSuccess") : Helper.GetResourceString("msg_CitySaveSuccess");
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
                result.PostMessage = Helper.GetResourceString("msg_SkuError");
                _logger.Error(message);
            }
            return result;
        }

        /// <summary>
        /// Method to Get District List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<CityDto>> GetCityListAsync()
        {
            try
            {
                _methodName = "GetCityListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetCityLists);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<CityDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<CityDto>();
        }

        /// <summary>
        /// Method to get Get districts Details By Id
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        public async Task<CityDto> GetCityDetailsById(string cityid)
        {
            var result = new CityDto();
            _methodName = "GetCityDetailsById";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var updateDistrictDto = new UpdateCityDto();
                var inputDtoJson = string.Empty;

                updateDistrictDto = new UpdateCityDto
                {
                    EncryptedId = cityid
                };

                inputDtoJson = JsonHelper.ConvertObjectToJson<UpdateCityDto>(updateDistrictDto);

                var apiUrl = ApiUrl.WebApiUrlGetCityDetailsById;
                if (!String.IsNullOrEmpty(cityid))
                {
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
                            result = JsonConvert.DeserializeObject<CityDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
                result.PostMessage = Helper.GetResourceString("msg_DealerError");
                _logger.Error(message);
            }
            return result;
        }

        public async Task<List<CityDto>> ExportCity(LoginUserIdDto inputDto)
        {
            _methodName = "ExportCity";
            var result = await GetListAsync<CityDto>(ApiUrl.WebApiUrlExportCity, inputDto);
            return result.ToList();
        }

        #endregion

        #region FreightZone

        /// <summary>
        /// Method to add or update FreightZone
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<FreightZoneDto> AddOrUpdateFreightZone(FreightZoneDto inputDto)
        {
            _methodName = "AddOrUpdateFreightZone";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new FreightZoneDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                if (inputDto.Id > 0)
                { apiUrl = ApiUrl.WebApiUrlPostUpdateFreightZone; }
                else { apiUrl = ApiUrl.WebApiUrlPostSaveFreightZone; }

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
                        result.PostMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_FreightZoneUpdateSuccess") : Helper.GetResourceString("msg_FreightZoneSaveSuccess");
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
                result.PostMessage = Helper.GetResourceString("msg_FreightZoneError");
                _logger.Error(message);
            }
            return result;
        }

        /// <summary>
        /// Method to Get FreightZone List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<FreightZoneDto>> GetFreightZoneListAsync(LoginUserIdDto inputDto)
        {
            try
            {
                _methodName = "GetFreightZoneListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetFreightZoneList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<FreightZoneDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<FreightZoneDto>();
        }

        /// <summary>
        /// Method to get Get FreightZone Details By Id
        /// </summary>
        /// <param name="FreightZoneId"></param>
        /// <returns></returns>
        public async Task<FreightZoneDto> GetFreightZoneDetailsById(long FreightZoneId)
        {
            var result = new FreightZoneDto();
            _methodName = "GetFreightZoneDetailsById";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var apiUrl = ApiUrl.WebApiUrlGetFreightZoneDetailsById;
                if (FreightZoneId != 0)
                {
                    var inputDtoJson = JsonHelper.ConvertObjectToJson(FreightZoneId);
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
                            result = JsonConvert.DeserializeObject<FreightZoneDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
                result.PostMessage = Helper.GetResourceString("msg_DealerError");
                _logger.Error(message);
            }
            return result;
        }

        public async Task<List<FreightZoneDto>> ExportFreightZone(LoginUserIdDto inputDto)
        {
            _methodName = "ExportFreightZone";
            var result = await GetListAsync<FreightZoneDto>(ApiUrl.WebApiUrlExportFreightZone, inputDto);
            return result.ToList();
        }

        #endregion

        #region FreightRoute

        /// <summary>
        /// Method to add or update FreightRoute
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<FreightRouteDto> AddOrUpdateFreightRoute(FreightRouteDto inputDto)
        {
            _methodName = "AddOrUpdateFreightRoute";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new FreightRouteDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                if (inputDto.Id > 0)
                { apiUrl = ApiUrl.WebApiUrlPostUpdateFreightRoute; }
                else { apiUrl = ApiUrl.WebApiUrlPostSaveFreightRoute; }

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
                        result.PostMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_FreightRouteUpdateSuccess") : Helper.GetResourceString("msg_FreightRouteSaveSuccess");
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
                result.PostMessage = Helper.GetResourceString("msg_FreightRouteError");
                _logger.Error(message);
            }
            return result;
        }

        /// <summary>
        /// Method to Get FreightRoute List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<FreightRouteDto>> GetFreightRouteListAsync(LoginUserIdDto inputDto)
        {
            try
            {
                _methodName = "GetFreightRouteListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetFreightRouteList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<FreightRouteDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<FreightRouteDto>();
        }

        /// <summary>
        /// Method to get Get FreightRoute Details By Id
        /// </summary>
        /// <param name="FreightRouteId"></param>
        /// <returns></returns>
        public async Task<FreightRouteDto> GetFreightRouteDetailsById(long FreightRouteId)
        {
            var result = new FreightRouteDto();
            _methodName = "GetFreightRouteDetailsById";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var apiUrl = ApiUrl.WebApiUrlGetFreightRouteDetailsById;
                if (FreightRouteId != 0)
                {
                    var inputDtoJson = JsonHelper.ConvertObjectToJson(FreightRouteId);
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
                            result = JsonConvert.DeserializeObject<FreightRouteDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
                result.PostMessage = Helper.GetResourceString("msg_DealerError");
                _logger.Error(message);
            }
            return result;
        }


        /// <summary>
        /// Method to Get Freight Zone List
        /// </summary>       
        /// <returns></returns>
        public async Task<List<DropDownDto>> GetFreightZoneListByDepot(IdInputDto inputdto)
        {
            try
            {
                _methodName = "GetFreightZoneListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputdto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetFreightZoneListByDepot, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var result = JsonConvert.DeserializeObject<List<DropDownDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return result;
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

        public async Task<List<DropDownDto>> GetFreightZoneListByDepotIds(List<long> inputdto)
        {
            try
            {
                _methodName = "GetFreightZoneListByDepotIds";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputdto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetFreightZoneListByDepotIds, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var result = JsonConvert.DeserializeObject<List<DropDownDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return result;
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

        public async Task<List<DropDownDto>> GetFreightZoneListddlAsync()
        {
            var result = new List<DropDownDto>();
            _methodName = "GetFreightZoneListddlAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetFreightZoneListddl);
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

        public async Task<List<DropDownDto>> GetFreightZoneListddlByStateZoneAsync(FreightZoneInputDto inputDto)
        {
            var result = new List<DropDownDto>();
            _methodName = "GetFreightZoneListddlByStateZoneAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetFreightZoneListddlByStateZone, inputSring);
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

        public async Task<List<DropDownDto>> GetFreightZoneListByStateZoneIdsAsync(FreightZoneInputDto inputDto)
        {
            var result = new List<DropDownDto>();
            _methodName = "GetFreightZoneListddlByStateZoneAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetFreightZoneListddlByStateZoneIds, inputSring);
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
        /// <summary>
        /// Method to Get Freight Zone List
        /// </summary>
        /// <param name="inputdto"></param>
        /// <returns></returns>
        public async Task<List<DropDownDto>> GetFreightRouteListByZone(IdInputDto inputdto)
        {
            var result = new List<DropDownDto>();
            _methodName = "GetFreightRouteListddl";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputdto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetFreightRouteListByZone, inputSring);
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

        public async Task<List<FreightRouteDto>> ExportFreightRoute(LoginUserIdDto inputDto)
        {
            _methodName = "ExportFreightRoute";
            var result = await GetListAsync<FreightRouteDto>(ApiUrl.WebApiUrlExportFreightRoute, inputDto);
            return result.ToList();
        }
        #endregion

        #region Territory

        public async Task<TerritoryDto> AddOrUpdateTerritory(TerritoryDto territoryDto)
        {
            _methodName = "AddOrUpdateTerritory";
            var addOrUpdateMessage = territoryDto.Id > 0 ? Helper.GetResourceString("msg_TerritoryUpdateSuccess") : Helper.GetResourceString("msg_TerritorySaveSuccess");
            var errorMessage = Helper.GetResourceString("msg_TerritoryError");
            var apiUrl = territoryDto.Id > 0 ? ApiUrl.WebApiUrlPostUpdateTerritory : ApiUrl.WebApiUrlPostAddTerritory;
            return await AddOrUpdate(apiUrl, territoryDto, addOrUpdateMessage, errorMessage);
        }

        public async Task<IList<TerritoryDto>> GerTerritoryList(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GerTerritoryList";
            string apiUrl = ApiUrl.WebApiUrlGerTerritoryList;
            var response = await GetListAsync<TerritoryDto>(apiUrl, loginUserIdDto);
            return response;
        }

        public async Task<IList<TerritoryDto>> GerTerritoryMappedDistrict(TerritoryDistrictParam inputDto)
        {
            _methodName = "GerTerritoryMappedDistrict";
            string apiUrl = ApiUrl.WebApiUrlGerTerritoryMappedDistrict;
            var response = await GetListAsync<TerritoryDto>(apiUrl, inputDto);
            return response;
        }

        public async Task<TerritoryDto> GerTerritoryById(int territoryId)
        {
            _methodName = "GerTerritoryById";
            string apiUrl = ApiUrl.WebApiUrlGerTerritoryById;
            var result = await GetById<TerritoryDto>(apiUrl, territoryId);
            return result;
        }

        public async Task<IList<DropDownDto>> GerTerritoryStateBase(int stateId)
        {
            _methodName = "GerTerritoryStateBase";
            string apiUrl = ApiUrl.WebApiUrlGerTerritoryStateBase;
            var response = await GetListAsync<DropDownDto>(apiUrl, stateId);
            return response;
        }

        public async Task<List<TerritoryDto>> ExportTerritory(LoginUserIdDto inputDto)
        {
            _methodName = "ExportTerritory";
            var result = await GetListAsync<TerritoryDto>(ApiUrl.WebApiUrlExportTerritory, inputDto);
            return result.ToList();
        }

        #endregion

        #region SubCategory

        /// <summary>
        /// Method to add or update SubCategory
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<SubCategoryDto> AddOrUpdateSubCategory(SubCategoryDto inputDto)
        {
            _methodName = "AddOrUpdateSubCategory";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new SubCategoryDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                if (inputDto.Id > 0)
                { apiUrl = ApiUrl.WebApiUrlPostUpdateSubCategory; }
                else { apiUrl = ApiUrl.WebApiUrlPostSaveSubCategory; }

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
                        result.PostMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_SubCategoryUpdateSuccess") : Helper.GetResourceString("msg_SubCategorySaveSuccess");
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
                result.PostMessage = Helper.GetResourceString("msg_SubCategoryError");
                _logger.Error(message);
            }
            return result;
        }

        /// <summary>
        /// Method to Get SubCategory List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<DataSourceResult> GetSubCategoryListAsync(KendoGridResult inputDto)
        {
            var result = await GetKendoGridResultAsync<SubCategoryDto>(ApiUrl.WebApiUrlGetSubCategoryList, inputDto);
            return result;
        }

        /// <summary>
        /// Method to get Get SubCategory Details By Id
        /// </summary>
        /// <param name="subCategoryId"></param>
        /// <returns></returns>
        public async Task<SubCategoryDto> GetSubCategoryDetailsById(long subCategoryId)
        {
            var result = new SubCategoryDto();
            _methodName = "GetSubCategoryDetailsById";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var apiUrl = ApiUrl.WebApiUrlGetSubCategoryDetailsById;
                if (subCategoryId != 0)
                {
                    var inputDtoJson = JsonHelper.ConvertObjectToJson(subCategoryId);
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
                            result = JsonConvert.DeserializeObject<SubCategoryDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
                result.PostMessage = Helper.GetResourceString("msg_SomeErrorOccured");
                _logger.Error(message);
            }
            return result;
        }

        public async Task<List<SubCategoryDto>> ExportSubCategory(LoginUserIdDto inputDto)
        {
            _methodName = "ExportSubCategory";
            var result = await GetListAsync<SubCategoryDto>(ApiUrl.WebApiUrlExportSubCategory, inputDto);
            return result.ToList();
        }

        #endregion

        #region Lookup

        public async Task<IList<DropDownDto>> GetTransportModeBasedonDepotRake(IdInputDto inputDto)
        {
            _methodName = "GetTransportModeBasedonDepotRake";
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetTransportModeBasedonDepotRake, inputDto);
            return result;
        }

        public async Task<IList<StateDto>> GetStatesBasedOnZone(List<int> zoneId)
        {
            _methodName = "GetStatesBasedOnZone";
            string apiUrl = ApiUrl.WebApiUrlGetStatesBasedOnZone;
            return await GetListAsync<StateDto>(apiUrl, zoneId);
        }

        public async Task<IList<ZonalHeadMappingDto>> GetZonalHeadBasedonZoneState(ZonalHeadMappingDto inputDto)
        {
            _methodName = "GetZonalHeadBasedonZoneState";
            string apiUrl = ApiUrl.WebApiUrlGetZonalHeadBasedonZoneState;
            return await GetListAsync<ZonalHeadMappingDto>(apiUrl, inputDto);

        }

        public async Task<IList<OilTypeMappingDto>> GetOilTypeBasedonVerticals(OilTypeMappingDto inputDto)
        {
            _methodName = "GetOilTypeBasedonVerticals";
            string apiUrl = ApiUrl.WebApiUrlGetOilTypeBasedonVerticals;
            return await GetListAsync<OilTypeMappingDto>(apiUrl, inputDto);

        }

        public async Task<IList<DropDownDto>> GerTerritoryListByStateIdsForDropdown(List<int> stateIds)
        {
            _methodName = "GerTerritoryListByStateIdsForDropdown";
            string apiUrl = ApiUrl.WebApiUrlGerTerritoryBasedOnState;
            return await GetListAsync<DropDownDto>(apiUrl, stateIds);
        }

        /// <summary>
        /// Get all order status
        /// </summary>
        /// <returns></returns>
        public List<DropDownDto> GetAllStatus()
        {
            _methodName = "GetAllStatus";
            var statusList = new List<DropDownDto>();
            try
            {
                foreach (var unitDetailsItem in Helper.EnumToList<Status>())
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
                _logger.Error(message);
            }
            return statusList.Any() ? statusList.OrderBy(x => x.Id).ToList() : statusList;
        }

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
                _logger.Error(message);
            }
            return statusList.Any() ? statusList.OrderBy(x => x.Id).ToList() : statusList;
        }
        public async Task<IList<DropDownDto>> GerDistrictBasedOnTerritory(List<int> stateIds)
        {
            _methodName = "GerDistrictBasedOnTerritory";
            string apiUrl = ApiUrl.WebApiUrlGerDistrictBasedOnTerritory;
            return await GetListAsync<DropDownDto>(apiUrl, stateIds);
        }

        public async Task<IList<DropDownDto>> GetCityListBasedOnDistrict(List<int> stateIds)
        {
            _methodName = "GetCityListBasedOnDistrict";
            string apiUrl = ApiUrl.WebApiUrlGetCityListBasedOnDistrict;
            return await GetListAsync<DropDownDto>(apiUrl, stateIds);
        }

        public async Task<IList<DropDownDto>> GetFreightRouteByZone(List<int> stateIds)
        {
            _methodName = "GetFreightRouteByZone";
            string apiUrl = ApiUrl.WebApiUrlGetFreightRouteByZone;
            return await GetListAsync<DropDownDto>(apiUrl, stateIds);
        }

        public async Task<IList<UserMasterDto>> GetUserExcelExportList(LoginUserIdDto inputDto)
        {
            _methodName = "GetUserExcelExportList";
            string apiUrl = ApiUrl.WebApiUrlGetUserExcelExport;
            return await GetListAsync<UserMasterDto>(apiUrl, inputDto);
        }

        public async Task<IList<DropDownDto>> GetCustomerOnCity(List<int> CityIds)
        {
            _methodName = "GetCustomerOnCity";
            string apiUrl = ApiUrl.WebApiUrlGetCustomerByCityIds;
            return await GetListAsync<DropDownDto>(apiUrl, CityIds);
        }

        #endregion

        #region Rake

        public async Task<RakeDto> AddOrUpdateRake(RakeDto inputDto)
        {
            _methodName = "AddOrUpdateRake";
            var addOrUpdateMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_RakeUpdateSuccess") : Helper.GetResourceString("msg_RakeSaveSuccess");
            var errorMessage = Helper.GetResourceString("msg_RakeError");
            var apiUrl = inputDto.Id > 0 ? ApiUrl.WebApiUrlPostUpdateRake : ApiUrl.WebApiUrlPostAddRake;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, errorMessage);
        }

        public async Task<RakeDto> GetRakeById(IdInputDto inputDto)
        {
            _methodName = "GetRakeById";
            string apiUrl = ApiUrl.WebApiUrlGetRakeById;
            var result = await GetByInputDto<RakeDto>(apiUrl, inputDto);
            return result;
        }

        public async Task<IList<RakeDto>> GetRakeList(LoginUserIdDto inputDto)
        {
            _methodName = "GetRakeList";
            var result = await GetListAsync<RakeDto>(ApiUrl.WebApiUrlGetRakeList, inputDto);
            return result;
        }

        public async Task<IList<DepotRakeDto>> GetDepotRakeddList(IdInputDto inputDto, string apiUrl)
        {
            _methodName = "GetDepotRakeddList";
            var result = await GetListAsync<DepotRakeDto>(apiUrl, inputDto);
            return result;
        }

        public async Task<IList<DepotRakeDto>> GetDepotPlantddList(IdInputDto inputDto, string apiUrl)
        {
            _methodName = "GetDepotPlantddList";
            var result = await GetListAsync<DepotRakeDto>(apiUrl, inputDto);
            return result;
        }

        public async Task<IList<DropDownDto>> GetDepotListAsync(LoginUserIdDto inputDto)
        {
            _methodName = "GetDepotListAsync";
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetDepotListAsync, inputDto);
            return result;
        }

        public async Task<List<RakeDto>> ExportRake(LoginUserIdDto inputDto)
        {
            _methodName = "ExportRake";
            var result = await GetListAsync<RakeDto>(ApiUrl.WebApiUrlExportRake, inputDto);
            return result.ToList();
        }

        #endregion

        #region Pricing Grid Server Side paging        

        public async Task<DataSourceResult> GetKendoGridDataAsync<T>(KendoGridResult inputDto, string apiUrl) where T : class
        {
            var result = await GetKendoGridResultAsync<T>(apiUrl, inputDto);
            return result;
        }

        #endregion

        #region ShipToParty

        /// <summary>
        /// Method to add or update ShipToPartys
        /// </summary>
        /// <param name="ShipToPartyInputDto"></param>
        /// <returns></returns>    
        public async Task<EmployeeDto> AddOrUpdateShipToParty(EmployeeDto inputDto)
        {
            _methodName = "AddOrUpdateShipToParty";
            var apiUrl = !String.IsNullOrEmpty(inputDto.EncryptedId) ? ApiUrl.WebApiUrlPostUpdateUser : ApiUrl.WebApiUrlPostSaveUser;

            if (!string.IsNullOrEmpty(inputDto.SelecteDealerBrokerIdsString))
            {
                inputDto.SelectedDealerBrokerIds = UtilityHelper.ConvertStringToLongList(inputDto.SelecteDealerBrokerIdsString);
            }
            if (!string.IsNullOrEmpty(inputDto.RemovedDealerBrokerIdsString))
            {
                inputDto.RemovedDealerBrokerIds = UtilityHelper.ConvertStringToLongList(inputDto.RemovedDealerBrokerIdsString);
            }

            if (!string.IsNullOrEmpty(inputDto.SelecteDealerIdsString))
            {
                inputDto.SelectedDealerIds = UtilityHelper.ConvertStringToLongList(inputDto.SelecteDealerIdsString);
            }
            if (!string.IsNullOrEmpty(inputDto.RemovedDealerBrokerIdsString))
            {
                inputDto.RemovedDealerIds = UtilityHelper.ConvertStringToLongList(inputDto.RemovedDealerIdsString);
            }

            var msg = !String.IsNullOrEmpty(inputDto.EncryptedId) ? Helper.GetResourceString("msg_ShipToPartyUpdateSuccess") : Helper.GetResourceString("msg_ShipToPartySaveSuccess");
            var errMsg = Helper.GetResourceString("msg_ShipToPartyError");
            return await AddOrUpdate(apiUrl, inputDto, msg, errMsg);
        }

        /// <summary>
        /// Method to Get ShipToParty List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<ShipToPartyDto>> GetShipToPartyListAsync(LoginUserIdDto inputDto)
        {
            _methodName = "GetShipToPartyListAsync";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var result = await GetListAsync<ShipToPartyDto>(ApiUrl.WebApiUrlGetExcelExportShipToPartyList, inputDto);
                return result;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<ShipToPartyDto>();
        }

        public async Task<IList<ShipToPartyExportDto>> GetShipToPartyListAsync1(LoginUserIdDto inputDto)
        {
           
            var result = new List<ShipToPartyExportDto>();
            try
            {
                _methodName = "GetShipToPartyListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<ShipToPartyExportDto>("GetShipToPartyExport", new { VerticalId = inputDto.VerticalId }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                //foreach (var data in result)
                //{
                //    data.Password = !string.IsNullOrEmpty(data.Password) ? UtilityHelper.ConvertMd5ToString(data.Password, SecurityConstants.EncryptionKey) : string.Empty;
                //}

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        /// <summary>
        /// Method to get Get ShipToParty Details By Id
        /// </summary>
        /// <param name="ShipToPartyId"></param>
        /// <returns></returns>
        public async Task<EmployeeDto> GetShipToPartyDetailsById(string ShipToPartyId)
        {
            _methodName = "GetShipToPartyDetailsById";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var apiUrl = ApiUrl.WebApiUrlGetShipToPartyDetailsById;
            var result = await GetByEncryptId<EmployeeDto>(apiUrl, ShipToPartyId);
            return result;
        }

        /// <summary>
        /// Method to Get ShipToParty and Broker List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<ShipToPartyDto>> GetShipToPartyBrokerListAsync(LoginUserIdDto inputDto)
        {
            _methodName = "GetShipToPartyBrokerListAsync";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = await GetListAsync<ShipToPartyDto>(ApiUrl.WebApiUrlGetShipToPartyBrokerList, inputDto);
            return result;
        }


        public async Task<IList<DealerBrokerDto>> GetShipToPartyBasedOnVertical(LoginUserIdDto inputDto)
        {
            _methodName = "GetShipToPartyBasedOnVertical";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = await GetListAsync<DealerBrokerDto>(ApiUrl.WebApiUrlGetShipToPartyBasedOnState, inputDto);
            return result;
        }

        #endregion


        #region CustomerGroup5
        public async Task<IList<CustomerGroupFiveDto>> GetCustomerGroupFiveListAsync()
        {
            //_methodName = "GetCustomerGroupFiveListAsync";
            //var apiUrl = ApiUrl.WebApiUrlGetCustomerGroupFiveList;
            //var result = await GetListAsync<CustomerGroupFiveDto>(apiUrl);
            //return result;
            try
            {

                _methodName = "GetCustomerGroupFiveListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetCustomerGroupFiveList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<CustomerGroupFiveDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<CustomerGroupFiveDto>();
        }

        public async Task<CustomerGroupFiveDto> AddOrUpdateCustomerGroupFive(CustomerGroupFiveDto inputDto)
        {
            _methodName = "AddOrUpdateCustomerGroupFive";

            var apiUrl = ApiUrl.WebApiUrlPostAddorUpdateCustomerGroupFive;
            var result = await AddOrUpdate<CustomerGroupFiveDto>(apiUrl, inputDto, inputDto.Id > 0 ? Helper.GetResourceString("msg_CustomerGroupUpdate") : Helper.GetResourceString("msg_CustomerGroupSave"), " ");
            return result;

        }

        public async Task<CustomerGroupFiveDto> GetCustomerGroupFiveDetailsById(string customerGroupId)
        {
            var apiUrl = ApiUrl.WebApiUrlGetCustomerGroupFiveDetailsById;
            _methodName = "GetCustomerGroupFiveDetailsById";
            var result = await GetByEncryptId<CustomerGroupFiveDto>(apiUrl, customerGroupId);
            return result;

        }

        public List<CustomerGroupFiveExportDto> ExportCustomerGroupFive(LoginUserIdDto inputDto)
        {
            var result = new List<CustomerGroupFiveExportDto>();
            _methodName = "ExportCustomerGroupFive";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                result = connection.Query<CustomerGroupFiveExportDto>("select GroupCode,GroupName,IsActive from CustomerGroupFives").ToList();
            }
            return result;
        }

        public async Task<List<CustomerGroupFiveddlDto>> GetAllCustomerGroupFiveddl()
        {
            var result = new List<CustomerGroupFiveddlDto>();
            _methodName = "GetAllCustomerGroupFiveddl";
            try
            {

                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetCustomerGroupFive);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<CustomerGroupFiveddlDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                    }
                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";

            }
            return result;
        }

        #endregion

        #region CustomerGroup1 and CustomerGroup2 
        public async Task<CustomerGroupOneDto> AddOrUpdateCustomerGroupOne(CustomerGroupOneDto inputDto)
        {
            _methodName = "AddOrUpdateCustomerGroupOne";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new CustomerGroupOneDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                apiUrl = ApiUrl.WebApiUrlPostAddorUpdateCustomerGroupOne;

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
                        result.PostMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_CustomerGroupUpdate") : Helper.GetResourceString("msg_CustomerGroupSave");
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
                result.PostMessage = Helper.GetResourceString("msg_CustomerGroupError");
                _logger.Error(message);
            }
            return result;
        }

        public async Task<IList<CustomerGroupOneDto>> GetCustomerGroupOneListAsync()
        {
            try
            {
                _methodName = "GetCustomerGroupOneListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetCustomerGroupOneList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<CustomerGroupOneDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<CustomerGroupOneDto>();
        }

        public async Task<CustomerGroupOneDto> GetCustomerGroupOneDetailsById(long customerGroupId)
        {
            var result = new CustomerGroupOneDto();
            _methodName = "GetCustomerGroupOneDetailsById";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var apiUrl = ApiUrl.WebApiUrlGetCustomerGroupOneDetailsById;
                if (customerGroupId != 0)
                {
                    var inputDtoJson = JsonHelper.ConvertObjectToJson(customerGroupId);
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
                            result = JsonConvert.DeserializeObject<CustomerGroupOneDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
                result.PostMessage = Helper.GetResourceString("msg_SomeErrorOccured");
                _logger.Error(message);
            }
            return result;
        }

        public async Task<CustomerGroupOneDto> AddOrUpdateCustomerGroupTwo(CustomerGroupOneDto inputDto)
        {
            _methodName = "AddOrUpdateCustomerGroup1";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new CustomerGroupOneDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                apiUrl = ApiUrl.WebApiUrlPostAddorUpdateCustomerGroupTwo;

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
                        result.PostMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_CustomerGroupUpdate") : Helper.GetResourceString("msg_CustomerGroupSave");
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
                result.PostMessage = Helper.GetResourceString("msg_CustomerGroupError");
                _logger.Error(message);
            }
            return result;
        }

        public async Task<IList<CustomerGroupOneDto>> GetCustomerGroupTwoListAsync()
        {
            try
            {
                _methodName = "GetCustomerGroup2ListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetCustomerGroupTwoList);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<CustomerGroupOneDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<CustomerGroupOneDto>();
        }

        public async Task<CustomerGroupOneDto> GetCustomerGroupTwoDetailsById(long customerGroupId)
        {
            var result = new CustomerGroupOneDto();
            _methodName = "GetCustomerGroup2DetailsById";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var apiUrl = ApiUrl.WebApiUrlGetCustomerGroupTwoDetailsById;
                if (customerGroupId != 0)
                {
                    var inputDtoJson = JsonHelper.ConvertObjectToJson(customerGroupId);
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
                            result = JsonConvert.DeserializeObject<CustomerGroupOneDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
                result.PostMessage = Helper.GetResourceString("msg_SomeErrorOccured");
                _logger.Error(message);
            }
            return result;
        }
        #endregion

        #region 
        /// <summary>
        /// Method to Get Zonal Head List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<ZoneDto>> GetZonalHeadListAsync(LoginUserIdDto inputDto)
        {
            _methodName = "GetZonalHeadListAsync";
            string apiUrl = ApiUrl.WebApiUrlGetZonalHeadList;
            var response = await GetListAsync<ZoneDto>(apiUrl, inputDto);
            return response;
        }
        public async Task<IList<ZoneDto>> GetZonalHeadListAsyncNew(LoginUserIdDto inputDto)
        {
            _methodName = "GetZonalHeadListAsyncNew";
            string apiUrl = ApiUrl.WebApiUrlGetZonalHeadListNew;
            var response = await GetListAsync<ZoneDto>(apiUrl, inputDto);
            return response;
        }
        public async Task<IList<ZoneDto>> GetNationalHeadList(LoginUserIdDto inputDto)
        {
            _methodName = "GetNationalHeadList";
            string apiUrl = ApiUrl.WebApiUrlGetNationalHeadList;
            var response = await GetListAsync<ZoneDto>(apiUrl, inputDto);
            return response;
        }

        public async Task<List<ZoneDto>> GetZonalHeadListByNH(NationalHeadDto inputDto)
        {
            _methodName = "GetZonalHeadListByNH";
            string apiUrl = ApiUrl.WebApiUrlGetZonalHeadListByNH;
            var response = await GetListAsync<ZoneDto>(apiUrl, inputDto);
            return response.ToList();
        }

        public async Task<IList<ZoneDto>> GetZHBasedOnVertical(LoginUserIdDto inputDto)
        {
            _methodName = "GetZHBasedOnVertical";
            string apiUrl = ApiUrl.WebApiUrlGetZHBasedOnVertical;
            var response = await GetListAsync<ZoneDto>(apiUrl, inputDto);
            return response;
        }

        public async Task<IList<DropDownDto>> GetBDOBasedOnZonalhead(List<long> zonalheadId)
        {
            _methodName = "GetBDOBasedOnZonalhead";
            string apiUrl = ApiUrl.WebApiUrlGetBDOBasedOnZonalHead;
            return await GetListAsync<DropDownDto>(apiUrl, zonalheadId);
        }
        public async Task<IList<DropDownDto>> GetZonalHeadBasedNH(long zonalheadId)
        {
            _methodName = "GetZonalHeadBasedNH";
            string apiUrl = ApiUrl.WebApiUrlGetZonalHeadBasedonZH;
            return await GetListAsync<DropDownDto>(apiUrl, zonalheadId);
        }
        public async Task<IList<DropDownDto>> GetZonalHeadBasedNHComb(BookedSaudaInputDto zonalheadId)
        {
            _methodName = "GetZonalHeadBasedNH";
            string apiUrl = ApiUrl.WebApiUrlGetZonalHeadBasedonZHComb;
            return await GetListAsync<DropDownDto>(apiUrl, zonalheadId);
        }

        public async Task<IList<DropDownDto>> GetDealerBasedOnBdo(List<long> bdoIds)
        {
            _methodName = "GetDealerBasedOnBdo";
            string apiUrl = ApiUrl.WebApiUrlGetDealerBasedOnBdo;
            return await GetListAsync<DropDownDto>(apiUrl, bdoIds);
        }
        public async Task<IList<DropDownDto>> GetDealerCodeBasedOnBdo(List<long> bdoIds)
        {
            _methodName = "GetDealerBasedOnBdo";
            string apiUrl = ApiUrl.WebApiUrlGetDealerCodeBasedOnBdo;
            return await GetListAsync<DropDownDto>(apiUrl, bdoIds);
        }

        #endregion

        public async Task<List<CustomerGroupOneandTwoddlDto>> GetAllCustomerGroupOneddl()
        {
            var result = new List<CustomerGroupOneandTwoddlDto>();
            _methodName = "GetAllCustomerGroupOneddl";
            try
            {

                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetCustomerGroupOne);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<CustomerGroupOneandTwoddlDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                    }
                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";

            }
            return result;
        }
        public async Task<List<CustomerGroupOneandTwoddlDto>> GetAllCustomerGroupTwoddl()
        {
            var result = new List<CustomerGroupOneandTwoddlDto>();
            _methodName = "GetAllCustomerGroup2ddl";
            try
            {

                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetCustomerGroupTwo);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<CustomerGroupOneandTwoddlDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                    }
                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";

            }
            return result;
        }
        public List<CustomerGrouOneTwoExportDto> ExportCustomerGroupOne(LoginUserIdDto inputDto)
        {
            var result = new List<CustomerGrouOneTwoExportDto>();
            _methodName = "ExportCustomerGroupOne";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                result = connection.Query<CustomerGrouOneTwoExportDto>("select GroupCode,GroupName,IsActive from CustomerGroupOnes").ToList();
            }
            return result;
        }
        public List<CustomerGrouOneTwoExportDto> ExportCustomerGroupTwo(LoginUserIdDto inputDto)
        {
            var result = new List<CustomerGrouOneTwoExportDto>();
            _methodName = "ExportCustomerGroupTwo";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                result = connection.Query<CustomerGrouOneTwoExportDto>("select GroupCode,GroupName,IsActive from CustomerGroupTwoes").ToList();
            }
            return result;
        }


        #region  SmsSend

        public async Task<SmsInputDto> SendSms(SmsInputDto inputDto)
        {
            _methodName = "SendSms";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new SmsInputDto();
            try
            {
                var apiUrl = string.Empty;
                var inputDtoJson = string.Empty;
                apiUrl = ApiUrl.WebApiUrlPostSendSms;
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
                        var successDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        result.PostStatus = true;
                        result.PostMessage = successDtoResult.Response.ToString();
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
                result.PostMessage = "Notification sends some error occurred, Please try again";
                _logger.Error(message);
            }
            return result;
        }

        /// <summary>
        /// Get all order status
        /// </summary>
        /// <returns></returns>
        public List<DropDownDto> GetAllNotificationTypes()
        {
            _methodName = "GetAllNotificationTypes";
            var statusList = new List<DropDownDto>();
            try
            {
                foreach (var unitDetailsItem in Helper.EnumToList<NotificationTypeForms>())
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
                _logger.Error(message);
            }
            return statusList.Any() ? statusList.OrderBy(x => x.Id).ToList() : statusList;
        }

        /// <summary>
        /// Get all order status
        /// </summary>
        /// <returns></returns>
        public List<DropDownDto> GetGeneralNotificationTypes()
        {
            _methodName = "GetGeneralNotificationTypes";
            var statusList = new List<DropDownDto>();
            try
            {
                foreach (var unitDetailsItem in Helper.EnumToList<AppNotificationType>())
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
                _logger.Error(message);
            }
            return statusList.Any() ? statusList.OrderBy(x => x.Id).ToList() : statusList;
        }

        /// <summary>
        /// Get all order status
        /// </summary>
        /// <returns></returns>
        public List<DropDownDto> GetNotificationTypes()
        {
            _methodName = "GetNotificationTypes";
            var statusList = new List<DropDownDto>();
            try
            {
                foreach (var unitDetailsItem in Helper.EnumToList<LiveOrTesting>())
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
                _logger.Error(message);
            }
            return statusList.Any() ? statusList.OrderBy(x => x.Id).ToList() : statusList;
        }
        #endregion

        #region SchemeGeographyReport
        public async Task<IList<SchemeDiscountGeographyDto>> GetGeographySchemeBasedOnState(List<int> stateId)
        {
            _methodName = "GetGeographySchemeBasedOnState";
            string apiUrl = ApiUrl.WebApiUrlGetGeographySchemeBasedOnState;
            return await GetListAsync<SchemeDiscountGeographyDto>(apiUrl, stateId);
        }
        #endregion

        public async Task<UserProfileDto> GetProfileImageUrl(LoginUserIdDto inputDto)
        {
            _methodName = "GetProfileImageUrl";
            string apiUrl = ApiUrl.WebApiUrlProfileImage; 
            return await GetByInputDto<UserProfileDto>(apiUrl,inputDto);
        }

        #region SaudaBookingConfiguration

        public async Task<SaudaBookingConfigurationDto> SaudaBookingConfiguration(SaudaBookingConfigurationDto inputDto)
        {
            _methodName = "SaudaBookingConfiguration";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new SaudaBookingConfigurationDto();
            try
            {
                var apiUrl = string.Empty;
                var inputDtoJson = string.Empty;
                apiUrl = ApiUrl.WebApiUrlPostSaudaBookingConfiguration;
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
                        var successDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        result.PostStatus = true;
                        result.PostMessage = successDtoResult.Response.ToString();
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
                result.PostMessage = "Some error occurred, Please try again";
                _logger.Error(message);
            }
            return result;
        }


        #endregion

        #region SaudaSalesAreaRestrictionConfiguration

        public async Task<SaudaSalesAreaRestrictionDto> SaudaSalesAreaRestrictionConfiguration(SaudaSalesAreaRestrictionDto inputDto)
        {
            _methodName = "SaudaSalesAreaRestrictionConfiguration";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new SaudaSalesAreaRestrictionDto();
            try
            {
                var apiUrl = string.Empty;
                var inputDtoJson = string.Empty;
                apiUrl = ApiUrl.WebApiUrlPostSaudaSalesAreaRestrictionConfiguration;
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
                        var successDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        result.PostStatus = true;
                        result.PostMessage = successDtoResult.Response.ToString();
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
                result.PostMessage = "Some error occurred, Please try again";
                _logger.Error(message);
            }
            return result;
        }

        #endregion

        #region Line

        public async Task<AddAndUpdateLineDto> AddAndUpdateLineDetails(AddAndUpdateLineDto Inputmodel)
        {
            _methodName = "AddAndUpdateLineDetails";
            try
            {
                if(Inputmodel != null)
                {
                    var addOrUpdateMessage = !String.IsNullOrEmpty(Inputmodel.EncryptedId) ? Helper.GetResourceString("lbl_UpdateZoneSuccess") : Helper.GetResourceString("lbl_CreateZoneSuccess");
                    var apiUrl = String.IsNullOrEmpty(Inputmodel.EncryptedId) ? ApiUrl.WebApiUrlPostLine : ApiUrl.WebApiUrlPutLine;
                    return await AddOrUpdate(apiUrl, Inputmodel, addOrUpdateMessage, "Error");
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }

            return Inputmodel;
        }

        public async Task<List<LineddlDto>> GetLineListForddl()
        {
            _methodName = "GetLineListForddl";
            List<LineddlDto> lineListDto = new List<LineddlDto>();
            try
            {               
                var apiUrl = ApiUrl.WebApiUrlGetLineList;
                HttpResponseMessage response = GetAsync(apiUrl);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                {
                    var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                    var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                    lineListDto = JsonConvert.DeserializeObject<List<LineddlDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                    return await Task.FromResult(lineListDto);
                }
                return await Task.FromResult(lineListDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return await Task.FromResult(lineListDto);
            }
        }

        public async Task<List<LineGridDto>> GetLineListAsync()
        {
            _methodName = "GetLineListAsync";
            List<LineGridDto> lineListDto = new List<LineGridDto>();
            try
            {
                var apiUrl = ApiUrl.WebApiUrlGetLineListForGrid;
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
                        lineListDto = JsonConvert.DeserializeObject<List<LineGridDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return await Task.FromResult(lineListDto);
                    }
                }
                return await Task.FromResult(lineListDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return await Task.FromResult(lineListDto);
            }
        }

        public async Task<AddAndUpdateLineDto> GetLineMappingDetailsById(string lineId)
        {
            _methodName = "GetLineMappingDetailsById";
            string apiUrl = ApiUrl.WebApiUrlGetLineInfo;
            var result = await GetByEncryptId<AddAndUpdateLineDto>(apiUrl, lineId);
            return result;
        }

        public async Task<List<LineGridDto>> ExportLine(LoginUserIdDto inputDto)
        {
            _methodName = "ExportLine";
            var result = await GetListAsync<LineGridDto>(ApiUrl.WebApiUrlExportLine, inputDto);
            return result.ToList();
        }

        #endregion

        #region Dynamic Forms
        /// <summary>
        /// Method to get all master forms created
        /// </summary>
        /// <returns></returns>
        public async Task<List<FormDto>> GetMasterDynamicListAsync()
        {
            try
            {
                _methodName = "GetMasterDynamicListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetDynamicFormView);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<FormDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<FormDto>();
        }
        public async Task<DynamicFormQuestionDetailsViewModel> GetDynamicFormDetailsAsync(long id)
        {
            var result = new DynamicFormQuestionDetailsViewModel();
            _methodName = "GetDynamicFormDetailsAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDto = new FormIdDto
                {
                    FormId = id
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<FormIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetDynamicFormDetailsById, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<DynamicFormQuestionDetailsViewModel>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());

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

        #endregion
    }
}