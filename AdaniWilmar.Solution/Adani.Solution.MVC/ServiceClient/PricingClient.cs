using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using GMCore.Helper;
using GMCore.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Adani.Solution.DTO;
using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.Models;
using Adani.Solution.DTO.Enums;
using Kendo.Mvc.UI;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Dapper;

namespace Adani.Solution.MVC.ServiceClient
{
    public class PricingClient : BaseClient
    {
        private const string ServiceName = "Pricing Client";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;
        static string connectionString = ConfigHelper.SPConnectionString;

        #region User Role Discount

        /// <summary>
        /// Method to update Role based Discount
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<DepotCostDto> UpdateRoleBasedDiscount(RoleDiscountDto roleDiscountDto)
        {
            _methodName = "UpdateRoleBasedDiscount";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new DepotCostDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                apiUrl = ApiUrl.WebApiUrlPutRoleDiscount;

                inputDtoJson = JsonHelper.ConvertObjectToJson(roleDiscountDto);
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
                        result.PostMessage = Helper.GetResourceString("msg_DiscountUpdatedSuccessfully");
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
                result.PostMessage = Helper.GetResourceString("msg_DepotCostError");
                _logger.Error(message);
            }
            return result;
        }

        /// <summary>
        /// Method to Get Role based Discount List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<RoleDiscountDto>> GetRoleBasedDiscounts(RoleDiscountDto roleDiscountDto)
        {
            try
            {
                _methodName = "GetRoleBasedDiscounts";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<RoleDiscountDto>(roleDiscountDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetRoleDiscountAll, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<RoleDiscountDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<RoleDiscountDto>();
        }

        /// <summary>
        /// Method to Get Role based Discount details basedon Id
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<RoleDiscountDto> GetRoleBasedDiscountById(long roleDiscountId)
        {
            var result = new RoleDiscountDto();
            try
            {
                _methodName = "GetRoleBasedDiscountById";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                RoleDiscountDto roleDiscountDto = new RoleDiscountDto() { Id = roleDiscountId };
                var inputDtoJson = JsonHelper.ConvertObjectToJson<RoleDiscountDto>(roleDiscountDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetRoleDiscountById, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<RoleDiscountDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        #endregion

        #region Sku,Depot Based Discount

        /// <summary>
        /// Method to update Sku based Discount
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<DepotCostDto> AddOrUpdateSkuDepotDiscount(SkuDepotDiscountDto skuDepotDiscountDto)
        {
            _methodName = "AddOrUpdateSkuDepotDiscount";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new DepotCostDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                if (skuDepotDiscountDto.Id > 0)
                { apiUrl = ApiUrl.WebApiUrlUpdateSkuDepotDiscount; }
                else { apiUrl = ApiUrl.WebApiUrlPostSkuDepotDiscount; }

                inputDtoJson = JsonHelper.ConvertObjectToJson(skuDepotDiscountDto);
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
                        result.PostMessage = skuDepotDiscountDto.Id > 0 ? Helper.GetResourceString("msg_CustomerDiscountUpdatedSuccessfully") : Helper.GetResourceString("msg_CustomerDiscountSavedSuccessfully"); ;
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
                result.PostMessage = Helper.GetResourceString("msg_DepotCostError");
                _logger.Error(message);
            }
            return result;
        }

        /// <summary>
        /// Method to Get Sku based Discount List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<SkuDepotDiscountDto>> GetSkuDepotBasedDiscounts(CustomerDiscountinputDto discountinputDto)
        {
            try
            {
                _methodName = "GetSkuDepotBasedDiscounts";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<CustomerDiscountinputDto>(discountinputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSkuDepotDiscountAll, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<SkuDepotDiscountDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<SkuDepotDiscountDto>();
        }

        /// <summary>
        /// Method to Get Sku based Discount details basedon Id
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<SkuDepotDiscountDto> GetSkuDepotBasedDiscountById(CustomerDiscountinputDto discountDto)
        {
            var result = new SkuDepotDiscountDto();
            try
            {
                _methodName = "GetSkuDepotBasedDiscountById";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson<CustomerDiscountinputDto>(discountDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSkuDepotDiscountById, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<SkuDepotDiscountDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<IList<DropDownDto>> GetOilTypeDetailsddl(OilTypeDto inputDto)
        {
            try
            {
                _methodName = "GetOilTypeDetailsddl";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<OilTypeDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetOilTypeDetailsddl, inputSring);
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

        public async Task<IList<DropDownDto>> GetDepotDetailsddl(DepotDto inputDto)
        {
            try
            {
                _methodName = "GetDepotDetailsddl";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<DepotDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetDepotDetailsddl, inputSring);
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

        public async Task<IList<DropDownDto>> GetUserDetailsddl(LoginUserIdDto inputDto)
        {
            try
            {
                _methodName = "GetUserDetailsddl";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetUserDetailsddl, inputSring);
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

        public async Task<IList<DropDownDto>> GetSkuDetailsddl(OilTypeDto inputDto)
        {
            try
            {
                _methodName = "GetSkuDetailsddl";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<OilTypeDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSkuDetailsddl, inputSring);
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

        #region HoneycombCost

        public decimal ConvertRatePerMetricToRatePerCase(long skuId, decimal ratePerMt)
        {
            var noofPiecesperCase = (decimal)0;
            var litreConversion = (decimal)0;
            var quantity = (decimal)0;
            var uomId = 0L;
            var costPerCase = (decimal)0;


            using (SqlConnection query = new SqlConnection(ConfigHelper.SPConnectionString))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append("Select ConversionFactor from SkuUomMappings where SkuId = @SkuId and UomId = @UomId  and  RelationUomId = @RelationUomId ");
                    var skuUomContext = query.Query<SkuUomConversionDto>(sb.ToString(), new { SkuId = skuId, UomId = (int)DTO.Enums.Uom.Case, RelationUomId = (int)DTO.Enums.Uom.Nos, ConfigHelper.SPConnectionString }).FirstOrDefault();
                    if (skuUomContext != null)
                    {
                        noofPiecesperCase = skuUomContext.ConversionFactor;
                    }

                    sb.Clear();
                    sb.Append("select  a.Quantity , b.LitreConversion , a.UomId from Skus as a join OilTypes as b  on a.OilTypeId = b.Id where a.Id = @SkuId ");
                    var sku = query.Query<SkuUomConversionDto>(sb.ToString(), new { SkuId = skuId, ConfigHelper.SPConnectionString }).FirstOrDefault();
                    if (sku != null)
                    {
                        litreConversion = sku.LitreConversion;


                        uomId = Convert.ToInt64(sku.UomId);
                        quantity = sku.Quantity;
                        costPerCase = GetSkuQuanityRate(uomId, quantity, ratePerMt, litreConversion);
                        costPerCase = noofPiecesperCase * costPerCase;
                    }



                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);

                }
                finally
                {
                    query.Close();
                }

            }
            //var skuUomContext = _emamiContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);


            return costPerCase;
        }


        public decimal GetSkuQuanityRate(long quantityTypeId, decimal quantity, decimal ratePerMt, decimal litreConversion)
        {
            var quantityRate = (decimal)0;
            if (quantityTypeId == (int)DTO.Enums.Uom.Ltr)
            {
                //Litre conversion From SKU 
                //var oneLrRate = ratePerMt / litreConversion;

                //1000 kg x 1 L / 0.92 kg = 1087 L approx.
                //litreConversion = Convert.ToDecimal(1000 / 0.91);  //Oiltype KgConversion
                var oneLitreRate = ratePerMt / litreConversion;
                quantityRate = quantity * oneLitreRate;
            }
            else
            {
                var oneKgRate = ratePerMt / 1000;
                quantityRate = quantity * oneKgRate;
            }
            return quantityRate;
        }



        public DataTable RAMaterialCostExportToList(ExcelExportInputDto excelExportInputDto)
        {
            _methodName = "RAMaterialCostExportToList";
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigHelper.SPConnectionString);
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("RAMaterialCostExport", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", excelExportInputDto.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", excelExportInputDto.EndDate);
                cmd.Parameters.AddWithValue("@VerticalId", excelExportInputDto.VerticalId);
                cmd.Parameters.AddWithValue("@IsActiveStatus", excelExportInputDto.IsActiveStatus);
                conn.Open();
                rdr = cmd.ExecuteReader();
                dataTable.Load(rdr);
            }
            catch (Exception e)
            {

                _logger.Error($"{ServiceName} Service-Method {_methodName} Exception: {e}");

                return dataTable;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
            return dataTable;

        }

        #endregion

        #region Ad-Role Discount

        public async Task<RoleDisocuntDto> AddOrUpdateRoleDiscount(RoleDisocuntDto roleDisocuntDto)
        {
            _methodName = "AddOrUpdateRoleDiscount";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new RoleDisocuntDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                if (roleDisocuntDto.Id > 0)
                { apiUrl = ApiUrl.WebApiUrlUpdateAdminRoleDiscount; }
                else { apiUrl = ApiUrl.WebApiUrlPostAdminRoleDiscount; }

                inputDtoJson = JsonHelper.ConvertObjectToJson<RoleDisocuntDto>(roleDisocuntDto);
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
                        result.PostMessage = roleDisocuntDto.Id > 0 ? Helper.GetResourceString("msg_RoleDiscountUpdateSuccess") : Helper.GetResourceString("msg_RoleDiscountSavedSuccess"); ;
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
                result.PostMessage = Helper.GetResourceString("msg_RoleDiscountError");
                _logger.Error(message);
            }
            return result;
        }

        public async Task<IList<RoleDisocuntDto>> GetRoleDiscountsAll(RoleDisocuntInputDto roleDisocuntInputDto)
        {
            try
            {
                _methodName = "GetIngredientsCostAll";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<RoleDisocuntInputDto>(roleDisocuntInputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetAdminRolediscountall, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<IList<RoleDisocuntDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<RoleDisocuntDto>();
        }

        public async Task<RoleDisocuntDto> GetRoleDiscountbyId(RoleDisocuntInputDto roleDisocuntInputDto)
        {
            var result = new RoleDisocuntDto();
            try
            {
                _methodName = "GetRoleDiscountbyId";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson<RoleDisocuntInputDto>(roleDisocuntInputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetAdminDiscountById, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<RoleDisocuntDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        #endregion

        #region Request Discount

        public async Task<RequestDisocuntDto> GetRequestDiscountbyId(long roleDiscountId)
        {
            var result = new RequestDisocuntDto();
            try
            {
                _methodName = "GetRequestDiscountbyId";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputParam = new RequestDisocuntInputDto()
                {
                    Id = roleDiscountId
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<RequestDisocuntInputDto>(inputParam);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetRequestDiscountbyId, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<RequestDisocuntDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<RequestDisocuntDto> UpdateRequestDiscount(RequestDisocuntDto roleDisocuntDto)
        {
            _methodName = "UpdateRequestDiscount";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new RequestDisocuntDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;

                var inputdata = new RequestDisocuntUpdateDto()
                {
                    Id = roleDisocuntDto.Id,
                    LoginUserId = roleDisocuntDto.LoginUserId,
                    RequestedDiscount = roleDisocuntDto.RequestedDiscount
                };
                inputDtoJson = JsonHelper.ConvertObjectToJson<RequestDisocuntUpdateDto>(inputdata);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlUpdateRequestDiscount, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();

                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        roleDisocuntDto.PostStatus = true;
                        roleDisocuntDto.PostMessage = "Request Discount updated successfully";
                    }
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        roleDisocuntDto.PostStatus = false;
                        roleDisocuntDto.PostMessage = errorDtoResult.Message;
                    }
                }
                else
                {
                    roleDisocuntDto.PostStatus = false;
                    roleDisocuntDto.PostMessage = ja[0][Settings.ResponseMessage].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                roleDisocuntDto.PostStatus = false;
                roleDisocuntDto.PostMessage = Helper.GetResourceString("msg_SomeErrorOccured");
                _logger.Error(message);
            }
            return roleDisocuntDto;
        }

        public async Task<IList<RequestDisocuntDto>> GetRequestDiscountsAll(LoginUserIdDto loginUserIdDto)
        {
            try
            {
                _methodName = "GetRequestDiscountsAll";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<LoginUserIdDto>(loginUserIdDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetRequestDiscountsAll, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<IList<RequestDisocuntDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<RequestDisocuntDto>();
        }

        public async Task<IList<RequestDisocuntDto>> GetRequestDiscountList(RequestDisocuntInputDto requestDisocuntInputDto)
        {
            try
            {
                _methodName = "GetRequestDiscountList";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<RequestDisocuntInputDto>(requestDisocuntInputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetRequestDiscountList, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<IList<RequestDisocuntDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<RequestDisocuntDto>();
        }

        public async Task<RequestDisocuntDto> GetRequestDiscountDetailsById(RequestDisocuntInputDto requestDisocuntInputDto)
        {
            var result = new RequestDisocuntDto();
            try
            {
                _methodName = "GetRequestDiscountbyId";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<RequestDisocuntInputDto>(requestDisocuntInputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetRequestDiscountDetailsById, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<RequestDisocuntDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        result.PostStatus = true;
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
        #endregion

        #region Approve Discounts

        public async Task<IList<RequestDisocuntDto>> GetRequestedDiscounts(RequestDisocuntInputDto requestDisocuntInputDto)
        {
            try
            {
                _methodName = "GetRequestedDiscounts";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<RequestDisocuntInputDto>(requestDisocuntInputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetRequestedDiscounts, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<IList<RequestDisocuntDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<RequestDisocuntDto>();
        }

        public async Task<ApproveRequestDiscountDto> ApproveRequestedDiscount(ApproveRequestDiscountDto approveRequestDiscountDto)
        {
            _methodName = "ApproveRequestedDiscount";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new ApproveRequestDiscountDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                inputDtoJson = JsonHelper.ConvertObjectToJson<ApproveRequestDiscountDto>(approveRequestDiscountDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostApproveDiscount, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();

                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        approveRequestDiscountDto.PostStatus = true;
                        if (approveRequestDiscountDto.ReasonType == 1)
                            approveRequestDiscountDto.PostMessage = "Request Discount approved successfully";
                        if (approveRequestDiscountDto.ReasonType == 2)
                            approveRequestDiscountDto.PostMessage = "Request Discount canceled successfully";
                    }
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        approveRequestDiscountDto.PostStatus = false;
                        approveRequestDiscountDto.PostMessage = errorDtoResult.Message;
                    }
                }
                else
                {
                    approveRequestDiscountDto.PostStatus = false;
                    approveRequestDiscountDto.PostMessage = ja[0][Settings.ResponseMessage].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                approveRequestDiscountDto.PostStatus = false;
                approveRequestDiscountDto.PostMessage = Helper.GetResourceString("msg_SomeErrorOccured");
                _logger.Error(message);
            }
            return approveRequestDiscountDto;
        }
        #endregion

        #region Premium Discount

        public async Task<PremiumDisocuntDto> AddOrUpdatePremiumDiscount(PremiumDisocuntDto premiumDisocuntDto)
        {
            _methodName = "AddOrUpdatePremiumDiscount";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new PremiumDisocuntDto();
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                if (premiumDisocuntDto.Id > 0)
                { apiUrl = ApiUrl.WebApiUrlUpdatePremiumDiscount; }
                else { apiUrl = ApiUrl.WebApiUrlPostPremiumDiscount; }

                inputDtoJson = JsonHelper.ConvertObjectToJson<PremiumDisocuntDto>(premiumDisocuntDto);
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
                        result.PostMessage = premiumDisocuntDto.Id > 0 ? Helper.GetResourceString("msg_PremiumDiscountUpdateSuccess") : Helper.GetResourceString("msg_PremiumDiscountSavedSuccess"); ;
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
                result.PostMessage = Helper.GetResourceString("msg_PremiumDiscountError");
                _logger.Error(message);
            }
            return result;
        }

        public async Task<IList<PremiumDisocuntDto>> GetPremiumDiscountsAll(PremiumDisocuntInputDto premiumDisocuntInputDto)
        {
            try
            {
                _methodName = "GetPremiumDiscountsAll";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<PremiumDisocuntInputDto>(premiumDisocuntInputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetPremiumall, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<IList<PremiumDisocuntDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        return resultList;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new List<PremiumDisocuntDto>();
        }

        public async Task<PremiumDisocuntDto> GetPremiumDiscountbyId(PremiumDisocuntInputDto premiumDisocuntInputDto)
        {
            var result = new PremiumDisocuntDto();
            try
            {
                _methodName = "GetPremiumDiscountbyId";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson<PremiumDisocuntInputDto>(premiumDisocuntInputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetPremiumById, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<PremiumDisocuntDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        #endregion

        #region Premium Discount Request

        public async Task<IList<PremiumDisocuntRequestDto>> GetPremiumRequestDiscountList(PremiumDisocuntRequestInputDto premiumDisocuntInputDto)
        {
            _methodName = "GetPremiumDiscountsAll";
            string apiUrl = ApiUrl.WebApiUrlGetPremiumRequestall;
            var response = await GetListAsync<PremiumDisocuntRequestDto>(apiUrl, premiumDisocuntInputDto);
            return response;
        }

        public async Task<PremiumDisocuntRequestDto> UpdatePremiumRequestDiscount(PremiumDisocuntRequestDto premiumDisocuntDto)
        {
            _methodName = "UpdatePremiumRequestDiscount";
            var addOrUpdateMessage = Helper.GetResourceString("msg_PremiumRequestUpdatedMessage");
            var errorMessage = Helper.GetResourceString("msg_SomeErrorOccured");
            var apiUrl = ApiUrl.WebApiUrlUpdatePremiumRequestDiscount;
            return await AddOrUpdate(apiUrl, premiumDisocuntDto, addOrUpdateMessage, errorMessage);
        }

        public async Task<PremiumDisocuntRequestDto> GetPremiumRequestDiscountDetailsById(PremiumDisocuntRequestInputDto requestDisocuntInputDto)
        {
            var result = new PremiumDisocuntRequestDto();
            try
            {
                _methodName = "GetPremiumRequestDiscountDetailsById";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<PremiumDisocuntRequestInputDto>(requestDisocuntInputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetPremiumRequestById, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<PremiumDisocuntRequestDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        result.PostStatus = true;
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

        public async Task<PremiumDisocuntRequestDto> GetSkuPremiumDiscountRequestById(PremiumDisocuntRequestInputDto requestDisocuntInputDto)
        {
            var result = new PremiumDisocuntRequestDto();
            try
            {
                _methodName = "GetRequestDiscountbyId";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<PremiumDisocuntRequestInputDto>(requestDisocuntInputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSkuPremiumDiscountRequestById, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<PremiumDisocuntRequestDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        result.PostStatus = true;
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

        #endregion

        #region Approve Pending Request

        public async Task<IList<PremiumDisocuntRequestDto>> GetApprovePremiumRequestList(PremiumDisocuntRequestInputDto premiumDisocuntInputDto)
        {
            _methodName = "GetApprovePremiumRequestList";
            string apiUrl = ApiUrl.WebApiUrlGetPremiumDiscountForPending;
            var response = await GetListAsync<PremiumDisocuntRequestDto>(apiUrl, premiumDisocuntInputDto);
            return response;
        }

        public async Task<ApprovePremiunDiscountRequestDto> ApprovePremiumDiscountUpdate(ApprovePremiunDiscountRequestDto premiumDisocuntDto)
        {
            _methodName = "ApprovePremiumDiscountUpdate";
            var addOrUpdateMessage = Helper.GetResourceString("msg_PremiumRequestUpdatedMessage");
            var errorMessage = Helper.GetResourceString("msg_SomeErrorOccured");
            var apiUrl = ApiUrl.WebApiUrlPostApprovePremiumDiscount;
            return await AddOrUpdate(apiUrl, premiumDisocuntDto, addOrUpdateMessage, errorMessage);
        }

        #endregion

        #region Primary Discount Users

        public async Task<PrimaryDiscountUserDto> AddOrUpdatePrimaryDiscountForUser(PrimaryDiscountUserDto premiumDisocuntDto)
        {
            _methodName = "AddOrUpdatePrimaryDiscountForUser";
            var addOrUpdateMessage = premiumDisocuntDto.Id > 0 ? Helper.GetResourceString("msg_PremiumDiscountUsersUpdateSuccess") : Helper.GetResourceString("msg_PremiumDiscountUsersSaveSuccess");
            var errorMessage = Helper.GetResourceString("msg_SomeErrorOccured");
            var apiUrl = premiumDisocuntDto.Id > 0 ? ApiUrl.WebApiUrlPostUpdatePrimaryDiscountForUser : ApiUrl.WebApiUrlPostAddPrimaryDiscountForUser;
            return await AddOrUpdate(apiUrl, premiumDisocuntDto, addOrUpdateMessage, errorMessage);
        }

        public async Task<IList<PrimaryDiscountUserDto>> GetPrimaryDiscountForUserList(PrimaryDiscountUserInputDto premiumDisocuntInputDto)
        {
            _methodName = "GetPrimaryDiscountForUserList";
            string apiUrl = ApiUrl.WebApiUrlGetGetPrimaryDiscountForUserList;
            var response = await GetListAsync<PrimaryDiscountUserDto>(apiUrl, premiumDisocuntInputDto);
            return response;
        }

        public async Task<PrimaryDiscountUserDto> GetPrimaryDiscountForUserById(PrimaryDiscountUserInputDto primaryDiscountUserInputDto)
        {
            var result = new PrimaryDiscountUserDto();
            try
            {
                _methodName = "GetPrimaryDiscountForUserById";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<PrimaryDiscountUserInputDto>(primaryDiscountUserInputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetGetPrimaryDiscountForUserById, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<PrimaryDiscountUserDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        result.PostStatus = true;
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

        #endregion

        #region  Final Pricing

        public async Task<IList<SkuFinalpriceListOutputDto>> SearchFinalPrice(SkuFinalpriceListInputDto dto)
        {
            _methodName = "SearchFinalPricing";
            var result = await GetListAsync<SkuFinalpriceListOutputDto>(ApiUrl.WebApiUrlFinalPrice, dto);
            return result;
        }

        public async Task<FinalpriceListOutputDto> SearchFinalPricing(SkuFinalpriceListInputDto inputDto)
        {
            try
            {
                _methodName = "SearchFinalPricing";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var result = new FinalpriceListOutputDto();

                var inputDtoJson = JsonHelper.ConvertObjectToJson<SkuFinalpriceListInputDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlFinalPrice, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        //var resultList = JsonConvert.DeserializeObject<IList<SkuFinalpriceListOutputDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        //jarray[0]["response"]["successDto"].ToString()
                        var resultList = JsonConvert.DeserializeObject<IList<SkuFinalpriceListOutputDto>>(jarray[0]["response"]["successDto"]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        var errorResult = JsonConvert.DeserializeObject<List<string>>(jarray[0]["response"]["errorDto"]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        result.SkuFinalpriceList = resultList;
                        result.ErrorMessage = errorResult;
                        return result;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new FinalpriceListOutputDto();
        }

        public async Task<FinalpriceListOutputDto> SearchFinalPricingNew(SkuFinalpriceListInputDto inputDto)
        {
            try
            {
                _methodName = "SearchFinalPricingNew";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var result = new FinalpriceListOutputDto();

                var inputDtoJson = JsonHelper.ConvertObjectToJson<SkuFinalpriceListInputDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlFinalPriceNew, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                _logger.Info("Final Price MVC :" + DateHelper.UtcToIndia(DateTime.UtcNow));
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<IList<SkuFinalpriceListOutputDto>>(jarray[0]["response"]["successDto"]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        var errorResult = JsonConvert.DeserializeObject<List<string>>(jarray[0]["response"]["errorDto"]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        result.PostStatus = true;
                        result.SkuFinalpriceList = resultList;
                        result.ErrorMessage = errorResult;
                        return result;
                    }
                    else
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var errorResult = jarray[0]["message"].ToString();
                        result.PostStatus = false;
                        result.PostMessage = errorResult;
                        return result;
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return new FinalpriceListOutputDto();
        }

        public async Task<SaveFinalPricngInputDto> SaveTraditionalFinalPricing(SaveFinalPricngInputDto dto)
        {
            var result = await AddOrUpdate(ApiUrl.WebApiUrlSaveTraditionaFinalPrice, dto, Helper.GetResourceString("msg_TPPublishSuccess"), Helper.GetResourceString("msg_TPPublishFailure"));
            return result;
        }

        public async Task<SaveFinalPricngInputDto> SaveReverseAucationFinalPricing(SaveFinalPricngInputDto dto)
        {
            var result = await AddOrUpdate(ApiUrl.WebApiUrlSaveReverseAucationFinalPrice, dto, Helper.GetResourceString("msg_RAPublishSuccess"), Helper.GetResourceString("msg_RAPublishFailure"));
            return result;
        }

        #endregion

        #region Geography Discount

        public async Task<DiscountInputDto> AddDiscountGeography(DiscountInputDto dto)
        {
            var apiUrl = dto.Id > 0 ? ApiUrl.WebApiUrlUpdateDiscountGeography : ApiUrl.WebApiUrlAddDiscountGeography;
            var addOrUpdateMessage = dto.Id > 0 ? Helper.GetResourceString("msg_GeographyDiscountUpdate") : Helper.GetResourceString("msg_GeographyDiscountSave");
            var errorMessage = Helper.GetResourceString("msg_GeographyDiscount");
            var result = await AddOrUpdate(apiUrl, dto, addOrUpdateMessage, errorMessage);
            return result;
        }

        public async Task<IList<CityDetails>> GetCityDetailsBasedOnTerritory(TerritoryId territoryId)
        {
            var result = await GetListAsync<CityDetails>(ApiUrl.WebApiUrlGetCityDetailsBasedOnTerritory, territoryId);
            return result;
        }

        public async Task<DataSourceResult> GetGeographyList(LoginUserIdDto inputDto)
        {
            var kendoGridResult = new KendoGridResult
            {
                LoginUserId = inputDto.LoginUserId,
                IsToReturnInactiveData = inputDto.IsToReturnInactiveData,
                Date = inputDto.Date,
                DataSourceRequest = inputDto.DataSourceRequest,
                ZoneIds = inputDto.ZoneIds,
                StateIds = inputDto.StateIds,
                DistrictIds= inputDto.DistrictIds,
                CityIds= inputDto.CityIds,
                Status = inputDto.Status
            };
            var result = await GetKendoGridResultAsync<DiscountOutputDto>(ApiUrl.WebApiUrlGetGeographyList, inputDto);
            //var result = await GetListAsync<DiscountOutputDto>(ApiUrl.WebApiUrlGetGeographyList, inputDto);
            return result;
        }

        public async Task<IList<CityDetails>> GetGeographyCityList(GeographyCityListParam inputDto)
        {
            var result = await GetListAsync<CityDetails>(ApiUrl.WebApiUrlGetGeographyCityList, inputDto);
            return result;
        }

        public async Task<DiscountInputDto> GetGeographyDetailsById(long geographyId)
        {
            _methodName = "GetGeographyDetailsById";
            string apiUrl = ApiUrl.WebApiUrlGetGeographyDetailsById;
            var result = await GetById<DiscountInputDto>(apiUrl, geographyId);
            return result;
        }

        /// <summary>
        /// Method to Export Geography Discount
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public List<GeographyDiscountExportDto> ExportGeographyDiscount(ExcelReportFilterDto inputDto)
        {
            _methodName = "ExportGeographyDiscount";
            var result = new List<GeographyDiscountExportDto>();
            try
            {
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    string reportQuery = @"SELECT 
                                            DG.ParentId AS DiscountId,
                                            DG.ActualDiscount AS Discount, 
                                            CONVERT(VARCHAR(10), DG.ValidFrom, 101) as ValidFrom,
                                            CONVERT(VARCHAR(10), DG.ValidTo, 101) as ValidTo,
                                            Sku.SkuName, 
                                            Sku.SkuCode AS MaterialCode, 
                                            Sku.PackTypeId AS PackTypeId, 
                                            City.CityName AS City, 
                                            district.DistrictName AS District, 
                                            state.StateName AS State,
                                            zone.Name AS Zone FROM DiscountGeographies DG 
                                            LEFT JOIN Skus Sku on Sku.Id = DG.SkuId  
                                            LEFT JOIN Cities City on City.Id = DG.CityId
                                            LEFT JOIN Districts district on district.Id = DG.DistrictId
                                            LEFT JOIN States state on state.Id = DG.StateId
                                            LEFT JOIN Zones zone on zone.Id = DG.ZoneId
                                            --WHERE CAST(DG.CreatedDate AS DATE) = CAST(@FromDate AS DATE) 
                                            WHERE CAST(@FromDate AS DATE) BETWEEN CAST(DG.ValidFrom AS DATE) AND CAST(DG.ValidTo AS DATE) AND DG.ParentId <> 0";

                    result = conn.Query<GeographyDiscountExportDto>(reportQuery, 
                        new
                        {
                            inputDto.FromDate
                        },commandType:System.Data.CommandType.Text).ToList();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }


        #endregion

        #region Discount User

        public async Task<DiscountUserDto> AddOrUpdateDiscountUser(DiscountUserDto inputDto)
        {
            _methodName = "AddOrUpdateDiscountUser";
            var addOrUpdateMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_CustomerDiscountUpdatedSuccessfully") : Helper.GetResourceString("msg_CustomerDiscountSavedSuccessfully");
            var errorMessage = Helper.GetResourceString("msg_CustomerDiscountError");
            var apiUrl = inputDto.Id > 0 ? ApiUrl.WebApiUrlPostUpdateDiscountUsers : ApiUrl.WebApiUrlPostAddDiscountUsers;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, errorMessage);
        }

        public async Task<DiscountUserDto> GetDiscountUserById(long discountId)
        {
            _methodName = "GetDiscountUserById";
            string apiUrl = ApiUrl.WebApiUrlGetDiscountUsersById;
            var result = await GetById<DiscountUserDto>(apiUrl, discountId);
            return result;
        }

        public async Task<IList<DiscountUserDto>> GetDiscountUserList(LoginUserIdDto inputDto)
        {
            var result = await GetListAsync<DiscountUserDto>(ApiUrl.WebApiUrlGetDiscountUsersList, inputDto);
            return result;
        }
        public async Task<IList<DiscountExportDto>> DiscountUserExport(LoginUserIdDto inputDto)
        {
            var result = await GetListAsync<DiscountExportDto>(ApiUrl.WebApiUrlDiscountUsersExport, inputDto);
            return result;
        }


        public async Task<IList<DiscountUserQuantityOutput>> GetDiscountUserDetailList(GeographyCityListParam inputDto)
        {
            var result = await GetListAsync<DiscountUserQuantityOutput>(ApiUrl.WebApiUrlGetDiscountUsersDetailList, inputDto);
            return result;
        }

        public async Task<IList<DiscountUserDto>> GetEmployeeAndUserDiscountList(LoginUserIdDto inputDto)
        {
            var result = await GetListAsync<DiscountUserDto>(ApiUrl.WebApiUrlGetAssignedDiscountList, inputDto);
            return result;
        }

        public async Task<EmployeeUserDiscountDto> GetEmployeeAndUserDiscountById(IdInputDto inputDto)
        {
            _methodName = "GetEmployeeAndUserDiscountById";
            string apiUrl = ApiUrl.WebApiUrlGetAssignedDiscountById;
            var result = await GetByInputDto<EmployeeUserDiscountDto>(apiUrl, inputDto);
            return result;
        }

        public async Task<EmployeeUserDiscountDto> AddEmployeeAndUserDiscount(EmployeeUserDiscountDto inputDto)
        {
            _methodName = "AddEmployeeAndUserDiscount";
            var addOrUpdateMessage = Helper.GetResourceString("msg_CustomerDiscountAssignedSuccessfully");
            var errorMessage = Helper.GetResourceString("msg_CustomerDiscountError");
            var apiUrl = ApiUrl.WebApiUrlPostEmployeeAndUserDiscount;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, errorMessage);
        }

        #endregion

        #region PriceNotifyConfiguration

        public async Task<PriceNotifyConfigurationDto> AddorUpdatePriceNotifyConfiguration(PriceNotifyConfigurationDto dto)
        {
            var apiUrl = dto.Id > 0 ? ApiUrl.WebApiUrlUpdatePriceNotifyConfiguration : ApiUrl.WebApiUrlAddPriceNotifyConfiguration;
            var addOrUpdateMessage = dto.Id > 0 ? Helper.GetResourceString("msg_PriceNotifyConfigurationUpdate") : Helper.GetResourceString("msg_PriceNotifyConfigurationSave");
            var errorMessage = Helper.GetResourceString("msg_PriceNotifyConfiguration");
            var result = await AddOrUpdate(apiUrl, dto, addOrUpdateMessage, errorMessage);
            return result;
        }


        /// <summary>
        /// Method to Get Price Notify Configuration List Async
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<IList<PriceNotifyConfigurationDto>> GetPriceNotifyConfigurationListAsync(SaudaLimitInputDto inputDto)
        {
            var result = await GetListAsync<PriceNotifyConfigurationDto>(ApiUrl.WebApiUrlGetPriceNotifyConfigurationList, inputDto);
            return result;
        }


        public async Task<IList<CityDetails>> GetPriceNotifyConfigurationCityList(IdInputDto inputDto)
        {
            var result = await GetListAsync<CityDetails>(ApiUrl.WebApiUrlGetPriceNotifyConfigurationCityList, inputDto);
            return result;
        }

        public async Task<PriceNotifyConfigurationDto> GetPriceNotifyConfiguratioDetailsById(long geographyId)
        {
            _methodName = "GetPriceNotifyConfiguratioDetailsById";
            string apiUrl = ApiUrl.WebApiUrlGetPriceNotifyConfigurationById;
            var result = await GetById<PriceNotifyConfigurationDto>(apiUrl, geographyId);
            return result;
        }

        #endregion

        #region Premium

        public async Task<PremiumUserDto> AddOrUpdatePremiumUser(PremiumUserDto inputDto)
        {
            _methodName = "AddOrUpdatePremiumUser";
            var addOrUpdateMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_PremiumUpdateSuccess") : Helper.GetResourceString("msg_PremiumSavedSuccess");
            var errorMessage = Helper.GetResourceString("msg_PremiumDiscountError");
            var apiUrl = inputDto.Id > 0 ? ApiUrl.WebApiUrlPostUpdatePremium : ApiUrl.WebApiUrlPostAddPremium;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, errorMessage);
        }

        public async Task<PremiumUserDto> GetPremiumUserById(IdInputDto inputDto)
        {
            _methodName = "GetPremiumUserById";
            string apiUrl = ApiUrl.WebApiUrlGetPremiumUserById;
            var result = await GetByInputDto<PremiumUserDto>(apiUrl, inputDto);
            return result;
        }

        public async Task<IList<PremiumUserDto>> GetPremiumUserList(LoginUserIdDto inputDto)
        {
            _methodName = "GetPremiumUserList";
            var result = await GetListAsync<PremiumUserDto>(ApiUrl.WebApiUrlGetPremiumUserList, inputDto);
            return result;
        }

        public async Task<IList<PremiumUserQuantityOutput>> GetPremiumUserDetailList(PremiumUserListParam inputDto)
        {
            _methodName = "GetPremiumUserDetailList";
            var result = await GetListAsync<PremiumUserQuantityOutput>(ApiUrl.WebApiUrlGetPremiumUserDetailList, inputDto);
            return result;
        }

        #endregion

        #region Assigned Premium List

        public async Task<IList<PremiumUserDto>> GetAssignedPremiumList(LoginUserIdDto inputDto)
        {
            var result = await GetListAsync<PremiumUserDto>(ApiUrl.WebApiUrlGetAssignedPremiumList, inputDto);
            return result;
        }

        public async Task<EmployeeUserPremiumDto> GetAssignPremiumById(IdInputDto inputDto)
        {
            _methodName = "GetAssignPremiumById";
            string apiUrl = ApiUrl.WebApiUrlGetAssignedPremiumById;
            var result = await GetByInputDto<EmployeeUserPremiumDto>(apiUrl, inputDto);
            return result;
        }

        public async Task<EmployeeUserPremiumDto> AddEmployeeAndUserPremium(EmployeeUserPremiumDto inputDto)
        {
            _methodName = "AddEmployeeAndUserPremium";
            var addOrUpdateMessage = Helper.GetResourceString("msg_PremiumAssignedSuccessfully");
            var errorMessage = Helper.GetResourceString("msg_PremiumAssignError");
            var apiUrl = ApiUrl.WebApiUrlPostAddEmployeeAndUserPremium;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, errorMessage);
        }

        #endregion

        #region Premium Geography

        public async Task<PremiumInputDto> AddOrUpdatePremiumGeography(PremiumInputDto dto)
        {
            var apiUrl = dto.Id > 0 ? ApiUrl.WebApiUrlPostUpdatePremiumGeography : ApiUrl.WebApiUrlPostAddPremiumGeography;
            var addOrUpdateMessage = dto.Id > 0 ? Helper.GetResourceString("msg_PremiumUpdateSuccess") : Helper.GetResourceString("msg_PremiumSavedSuccess");
            var errorMessage = Helper.GetResourceString("msg_PremiumError");
            var result = await AddOrUpdate(apiUrl, dto, addOrUpdateMessage, errorMessage);
            return result;
        }

        public async Task<IList<PremiumOutputDto>> GetPremiumGeographyList(LoginUserIdDto inputDto)
        {
            var result = await GetListAsync<PremiumOutputDto>(ApiUrl.WebApiUrlGetPremiumGeographyList, inputDto);
            return result;
        }

        public async Task<PremiumInputDto> GetPremiumGeographyDetailsById(IdInputDto inputDto)
        {
            var result = await GetByInputDto<PremiumInputDto>(ApiUrl.WebApiUrlGetPremiumGeographyDetailsById, inputDto);
            return result;
        }

        public async Task<IList<CityDetails>> GetPremiumGeographyCityList(GeographyCityListParam inputDto)
        {
            var result = await GetListAsync<CityDetails>(ApiUrl.WebApiUrlGetPremiumGeographyCityList, inputDto);
            return result;
        }

        #endregion

        #region SpecialtyFat Geography Discount

        public async Task<SpecialityFatDiscountInputDto> AddSpecialtyFatDiscountGeography(SpecialityFatDiscountInputDto dto)
        {
            var apiUrl = dto.Id > 0 ? ApiUrl.WebApiUrlUpdateSpecialtyFatDiscountGeography : ApiUrl.WebApiUrlAddSpecialtyFatDiscountGeography;
            var addOrUpdateMessage = dto.Id > 0 ? Helper.GetResourceString("msg_SpecialtyFatGeographyquantitylimitupdate") : Helper.GetResourceString("msg_SpecialtyFatGeographyquantitylimitsave");
            var errorMessage = Helper.GetResourceString("msg_SpecialtyFatGeographyquantitylimiterror");
            var result = await AddOrUpdate(apiUrl, dto, addOrUpdateMessage, errorMessage);
            return result;
        }

        public async Task<IList<SpecialityFatDiscountOutputDto>> GetSpecialtyFatGeographyList(LoginUserIdDto inputDto)
        {
            var result = await GetListAsync<SpecialityFatDiscountOutputDto>(ApiUrl.WebApiUrlGetSpecialtyFatGeographyList, inputDto);
            return result;
        }

        public async Task<IList<CityDetails>> GetSpecialtyFatGeographyCityList(GeographyCityListParam inputDto)
        {
            var result = await GetListAsync<CityDetails>(ApiUrl.WebApiUrlGetSpecialtyFatGeographyCityList, inputDto);
            return result;
        }

        public async Task<SpecialityFatDiscountInputDto> GetSpecialtyFatGeographyDetailsById(long geographyId)
        {
            _methodName = "GetSpecialtyFatGeographyDetailsById";
            string apiUrl = ApiUrl.WebApiUrlGetSpecialtyFatGeographyDetailsById;
            var result = await GetById<SpecialityFatDiscountInputDto>(apiUrl, geographyId);
            return result;
        }

        public async Task<IList<CityDetails>> GetSpecialtyFatCityDetailsBasedOnCityTerritory(TerritoryId territoryId)
        {
            var result = await GetListAsync<CityDetails>(ApiUrl.WebApiUrlGetSpecialtyFatCityBasedOnCityTerritory, territoryId);
            return result;
        }

        #endregion

        #region SpecialityFat Discount User

        public async Task<SpecialityFatDiscountUserDto> AddOrUpdateSpecialityFatDiscountUser(SpecialityFatDiscountUserDto inputDto)
        {
            _methodName = "AddOrUpdateSpecialityFatDiscountUser";
            var addOrUpdateMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_SpecialtyFatQuantityLimitUpdate") : Helper.GetResourceString("msg_SpecialtyFatQuantityLimitSave");
            var errorMessage = Helper.GetResourceString("msg_SpecialtyFatQuantityLimitError");
            var apiUrl = inputDto.Id > 0 ? ApiUrl.WebApiUrlPostUpdateSpecialityFatDiscountUsers : ApiUrl.WebApiUrlPostAddSpecialityFatDiscountUsers;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, errorMessage);
        }

        public async Task<SpecialityFatDiscountUserDto> GetSpecialityFatDiscountUserById(long discountId)
        {
            _methodName = "GetSpecialityFatDiscountUserById";
            string apiUrl = ApiUrl.WebApiUrlGetSpecialityFatDiscountUsersById;
            var result = await GetById<SpecialityFatDiscountUserDto>(apiUrl, discountId);
            return result;
        }

        public async Task<IList<SpecialityFatDiscountUserDto>> GetSpecialityFatDiscountUserList(LoginUserIdDto inputDto)
        {
            var result = await GetListAsync<SpecialityFatDiscountUserDto>(ApiUrl.WebApiUrlGetSpecialityFatDiscountUsersList, inputDto);
            return result;
        }

        public async Task<IList<SpecialityFatEmployeeExportDto>> SpecialityFatDiscountUserExportAsync(LoginUserIdDto inputDto)
        {
            var result = await GetListAsync<SpecialityFatEmployeeExportDto>(ApiUrl.WebApiUrlGetSpecialityFatDiscountUsersExportList, inputDto);
            return result;
        }

        public async Task<IList<SpecialityFatEmployeeDto>> GetSpecialityFatDiscountUserDetailList(GeographyCityListParam inputDto)
        {
            var result = await GetListAsync<SpecialityFatEmployeeDto>(ApiUrl.WebApiUrlGetSpecialityFatDiscountUsersDetailList, inputDto);
            return result;
        }


        public async Task<IList<SpecialityFatDiscountUserDto>> GetSpecialityFatEmployeeDiscountList(LoginUserIdDto inputDto)
        {
            var result = await GetListAsync<SpecialityFatDiscountUserDto>(ApiUrl.WebApiUrlGetSpecialityFatAssignedDiscountList, inputDto);
            return result;
        }
        public async Task<IList<SpecialityFatDiscountUserExportDto>> GetSpecialityFatEmployeeDiscountExport(LoginUserIdDto inputDto)
        {
            var result = await GetListAsync<SpecialityFatDiscountUserExportDto>(ApiUrl.WebApiUrlGetSpecialityFatAssignedDiscountExport, inputDto);
            return result;
        }


        public async Task<IList<SpecialityFatDiscountUserDto>> GetSpecialityFatDiscountEmployeeDetailListAsynx(GeographyCityListParam inputDto)
        {
            var result = await GetListAsync<SpecialityFatDiscountUserDto>(ApiUrl.WebApiUrlGetSpecialityFatAssignedDiscountUserDetailsList, inputDto);
            return result;
        }

        public async Task<SpecialityFatEmployeeDiscountDto> GetSpecialityFatEmployeeDiscountById(IdInputDto inputDto)
        {
            _methodName = "GetSpecialityFatEmployeeDiscountById";
            string apiUrl = ApiUrl.WebApiUrlGetSpecialityFatDiscountUsersDetailId;
            var result = await GetByInputDto<SpecialityFatEmployeeDiscountDto>(apiUrl, inputDto);
            return result;
        }

        public async Task<SpecialityFatEmployeeDiscountDto> AddSpecialityFatEmployeeDiscount(SpecialityFatEmployeeDiscountDto inputDto)
        {
            _methodName = "AddSpecialityFatEmployeeDiscount";
            var addOrUpdateMessage = Helper.GetResourceString("msg_SpecialtyFatQtyUpdatedSuccessfully");
            var errorMessage = Helper.GetResourceString("msg_SpecialtyFatQuantityLimitSaveError");
            var apiUrl = ApiUrl.WebApiUrlGetSpecialityFatAssignedDiscountToUser;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, errorMessage);
        }

        public async Task<SpecialtyFatQuantityRequestDto> SaveRequestQuantityLimit(SpecialtyFatQuantityRequestDto inputDto)
        {
            _methodName = "SaveRequestQuantityLimit";
            var addOrUpdateMessage = Helper.GetResourceString("msg_RequestQuantitySuccess");
            var errorMessage = Helper.GetResourceString("msg_RequestQuantityError");
            var apiUrl = ApiUrl.WebApiUrlPostUpdateRequestQuantityLimit;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, errorMessage);
        }

        #endregion

        #region SpecalityFat Quantity Request
        /// <summary>
        /// Method to Get SpecalityFat Quantity Request List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<List<SpecialtyFatQuantityRequestDto>> GetSpecalityFatQuantityRequestListAsync(SpecialtyFatQuantityRequestSearchDto inputDto)
        {
            var result = new List<SpecialtyFatQuantityRequestDto>();
            try
            {
                _methodName = "GetSpecalityFatQuantityRequestListAsync";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<SpecialtyFatQuantityRequestSearchDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostGetSpecalityFatQuantityRequest, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var resultList = JsonConvert.DeserializeObject<List<SpecialtyFatQuantityRequestDto>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        if (resultList[0] != null && resultList.Any()) resultList[0].PostStatus = true;
                        return resultList;
                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        var saudaLimitRequestHistoryDto = new SpecialtyFatQuantityRequestDto();
                        saudaLimitRequestHistoryDto.PostStatus = false;
                        saudaLimitRequestHistoryDto.PostMessage = errorDtoResult.Message;
                        result.Add(saudaLimitRequestHistoryDto);
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
        /// Method to Approve SpecalityFat Quantity Request limit
        /// </summary>
        /// <param name="specialtyFatQuantityRequestDto"></param>
        /// <returns></returns>
        public async Task<SaudaApprovalViewModel> ApproveorRejectSpecalityFatQuantityRequest(SpecialtyFatQuantityRequestDto specialtyFatQuantityRequestDto)
        {
            var saudaApprovalViewModel = new SaudaApprovalViewModel();
            _methodName = "ApproveorRejectSpecalityFatQuantityRequest";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");


                var inputDtoJson = JsonHelper.ConvertObjectToJson(specialtyFatQuantityRequestDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostUpdateSpecalityFatQuantityRequest, inputSring);
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

        public async Task<IList<SpecialtyFatQuantityRequestDto>> GetSpecialtyFatQuantityRequestStatusAsync(SpecialtyFatQuantityRequestSearchDto inputDto)
        {
            var result = await GetListAsync<SpecialtyFatQuantityRequestDto>(ApiUrl.WebApiUrlGetSpecialtyFatQuantityRequestStatus, inputDto);
            return result;
        }

        #endregion

        #region  Auto Allocation

        public async Task<List<AutoAllocationDto>> GetAutoAllocationUserListByRoleIds(AutoAllocationInputDto inputDto)
        {
            _methodName = "GetAutoAllocationUserListByRoleIds";
            var result = await GetListAsync<AutoAllocationDto>(ApiUrl.WebApiUrlGetAutoAllocationUserListByRoleIds, inputDto);
            return result.ToList();
        }

        public async Task<List<AutoAllocationDetailDto>> GetAutoAllocationDetailsByUserId(AutoAllocationInputDto inputDto)
        {
            _methodName = "GetAutoAllocationDetailsByUserId";
            var result = await GetListAsync<AutoAllocationDetailDto>(ApiUrl.WebApiUrlGetAutoAllocationDetailsByUserId, inputDto);
            return result.ToList();
        }
        public async Task<SaveAutoAllocationDetailDto> SaveAutoAllocation(List<AutoAllocationDetailDto> inputDto)
        {
            _methodName = "GetAutoAllocationDetailsByUserId";
            SaveAutoAllocationDetailDto saveautoAllocationDetailDto = new SaveAutoAllocationDetailDto();
            var addOrUpdateMessage = Helper.GetResourceString("msg_SaveAutoAllocation");
            var errorMessage = Helper.GetResourceString("msg_SomeErrorOccured");
            var apiUrl = ApiUrl.WebApiUrlPostSaveAutoAllocation;
            saveautoAllocationDetailDto.autoAllocationDetailDtos = inputDto;
            return await AddOrUpdate(apiUrl, saveautoAllocationDetailDto, addOrUpdateMessage, errorMessage);
        }

        #endregion

        #region Published Price

        public async Task<List<PricePublishesDto>> GetPublishedPriceDetails(PricePublishInputDto inputDto)
        {
            _methodName = "GetPublishedPriceDetails";
            var result = await GetListAsync<PricePublishesDto>(ApiUrl.WebApiUrlGetPublishedPriceDetails, inputDto);
            return result.ToList();
        }

        public async Task<SkuFinalpriceListInputDto> SearchFinalPriceList(SkuFinalpriceListInputDto inputDto)
        {
            _methodName = "SearchFinalPriceList";
            var addOrUpdateMessage = "Final price generate processing. <br>Final price generate is completed will sent notification for mobile number";
            var errorMessage = "Final price generate error";
            var apiUrl = ApiUrl.WebApiUrlFinalPriceGenerate;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, errorMessage);
        }

        public async Task<FinalPricePublishDto> PublishFinalPrice(FinalPricePublishDto inputDto)
        {
            _methodName = "PublishFinalPrice";
            var successMessage = inputDto.BookingTypeId == (int)SaudaBookingTypes.TraditionalProcess ? "Traditional Process Final price published successfully" : "Reverse Auction Final price published successfully";
            var errorMessage = inputDto.BookingTypeId == (int)SaudaBookingTypes.TraditionalProcess ? "Traditional Process Final price publish error" : "Reverse Auction Final price publish error";
            return await AddOrUpdate(ApiUrl.WebApiUrlPublishFinalPrice, inputDto, successMessage, errorMessage);
        }

        public async Task<List<PricingDto>> GetPublishedFinalPriceListAsync(FinalPricePublishDto inputDto)
        {
            _methodName = "GetPublishedFinalPriceListAsync";
            var result = await GetListAsync<PricingDto>(ApiUrl.WebApiUrlGetPublishedFinalPriceList, inputDto);
            return result.ToList();
        }

        public async Task<SkuFinalpriceListInputDto> GenerateFinalPriceListAsync(SkuFinalpriceListInputDto inputDto)
        {
            _methodName = "SearchFinalPriceList";
            System.Text.StringBuilder msg = new System.Text.StringBuilder();
            msg.Append("Final price generate processing.");
            if (!string.IsNullOrEmpty(inputDto.MobileNoList))
            {
                msg.Append("<br> Final price generate process is completed will sent the notification for this mobile number : " + inputDto.MobileNoList);
            }
            var errorMessage = "Final price generate error";
            return await AddOrUpdate(ApiUrl.WebApiUrlFinalPriceGenerate, inputDto, msg.ToString(), errorMessage);
        }

        public async Task<List<PricingDto>> GetPublishedPriceErrorDetails(PricePublishInputDto inputDto)
        {
            _methodName = "GetPublishedPriceErrorDetails";
            var result = await GetListAsync<PricingDto>(ApiUrl.WebApiUrlGetPublishedFinalPriceErrorList, inputDto);
            return result.ToList();
        }

        #endregion

        #region Pricing Grid Server Side paging        

        public async Task<DataSourceResult> GetKendoGridDataAsync<T>(KendoGridResult inputDto, string apiUrl) where T : class
        {
            var result = await GetKendoGridResultAsync<T>(apiUrl, inputDto);
            return result;
        }


        public async Task<DataSourceResult> GetKendoGridDataExportAsync<T>(KendoGridResultExport inputDto, string apiUrl) where T : class
        {
            var result = await GetKendoGridResultAsync<T>(apiUrl, inputDto);
            return result;
        }

        #endregion

        #region New FinalPrice - State Based

        public async Task<List<FinalPriceGenerateOutputDto>> GetGeneratedPriceAsync(PricePublishInputDto inputDto)
        {
            _methodName = "GetGeneratedPriceAsync";
            var result = await GetListAsync<FinalPriceGenerateOutputDto>(ApiUrl.WebApiUrlGetGeneratedPriceAsync, inputDto);
            //foreach (var item in result)
            //{
            //    item.PricingDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(item.PricingDate, DateTimeKind.Unspecified),
            //     TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            //    //item.BookingDate = DateTime.SpecifyKind(item.BookingDate, DateTimeKind.Local).ToLocalTime();
            //}
            return result.ToList();
        }

        public async Task<DataSourceResult> GetGeneratedPriceList(PricePublistInputDataDto inputDto)
        {
            _methodName = "GetGeneratedPriceAsync";
            var result = await GetKendoGridResultAsync<FinalPriceGenerateListDto>(ApiUrl.WebApiUrlGetGeneratedPriceList, inputDto);
            //foreach (var item in result)
            //{
            //    item.PricingDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(item.PricingDate, DateTimeKind.Unspecified),
            //     TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            //    //item.BookingDate = DateTime.SpecifyKind(item.BookingDate, DateTimeKind.Local).ToLocalTime();
            //}
            return result;
        }

        public async Task<List<FinalPriceGenerateExportDto>> GetGeneratedPriceList1(PricePublistInputDataDto inputDto)
        {
            _methodName = "GetGeneratedPriceAsync";
            var result = new List<FinalPriceGenerateExportDto>();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        result = (await connection.QueryAsync<FinalPriceGenerateExportDto>("GetPricingData", new
                        {
                            StartDate = inputDto.StartDate,
                            EndDate = inputDto.EndDate,
                            DivisionId = inputDto.DivisionId,
                            SalesOrganizationId = inputDto.SalesOrganizationId,
                            DistributionChannelId = inputDto.DistributionChannelId,
                            OilTypeId = inputDto.OilTypeId,
                            PlantId = inputDto.PlantId,
                            LoginUserId = inputDto.LoginUserId,
                            RoleId = inputDto.RoleId
                        }, commandType: System.Data.CommandType.StoredProcedure)).ToList();
                    }
                    catch (Exception exception)
                    {
                        var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                        _logger.Error(message);
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


        public async Task<List<FinalPriceGenerateDetailOutputDto>> GetGeneratedPriceDetailsAsync(PricePublishInputDto inputDto)
        {
            _methodName = "GetGeneratedPriceDetailsAsync";
            var result = await GetListAsync<FinalPriceGenerateDetailOutputDto>(ApiUrl.WebApiUrlGetGetPriceGenerateDetails, inputDto);
            //foreach (var item in result)
            //{
            //    item.StartDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(item.StartDate, DateTimeKind.Unspecified),
            //     TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            //    item.EndDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(item.EndDate, DateTimeKind.Unspecified),
            //     TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            //    //item.BookingDate = DateTime.SpecifyKind(item.BookingDate, DateTimeKind.Local).ToLocalTime();
            //}
            return result.ToList();
        }

        public async Task<FinalPricePublishDto> StateBasePublishFinalPrice(FinalPricePublishDto inputDto)
        {
            _methodName = "StateBasePublishFinalPrice";
            var successMessage = inputDto.BookingTypeId == (int)SaudaBookingTypes.TraditionalProcess ? "Traditional Process Final price published successfully" : "Reverse Auction Final price published successfully";
            var errorMessage = inputDto.BookingTypeId == (int)SaudaBookingTypes.TraditionalProcess ? "Traditional Process Final price publish error" : "Reverse Auction Final price publish error";
            return await AddOrUpdate(ApiUrl.WebApiUrlStateBasePublishFinalPrice, inputDto, successMessage, errorMessage);
        }

        public async Task<List<PricingDto>> GetStateBasePublishedFinalPriceListAsync(FinalPricePublishDto inputDto)
        {
            _methodName = "GetStateBasePublishedFinalPriceListAsync";
            var result = await GetListAsync<PricingDto>(ApiUrl.WebApiUrlGetStateBasePublishedFinalPriceList, inputDto);
            return result.ToList();
        }

        public async Task<List<PricePublishesDto>> GetStateBasePublishedPriceErrorDetails(PricePublishInputDto inputDto)
        {
            _methodName = "GetStateBasePublishedPriceErrorDetails";
            var result = await GetListAsync<PricePublishesDto>(ApiUrl.WebApiUrlStateBaseGetPublishedFinalPriceErrorList, inputDto);
            return result.ToList();
        }

        #endregion

        #region ExportForPricing

        public DataTable MaterialCostExportToList(ExcelExportInputDto excelExportInputDto)
        {
            _methodName = "MaterialCostExportToList";
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigHelper.SPConnectionString);
            SqlDataReader rdr = null;
            try
            {
                excelExportInputDto.EndDate = excelExportInputDto.StartDate;
                SqlCommand cmd = new SqlCommand("MaterialCostExport", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@StartDate", excelExportInputDto.StartDate);
                //cmd.Parameters.AddWithValue("@EndDate", excelExportInputDto.EndDate);
                cmd.Parameters.AddWithValue("@VerticalId", excelExportInputDto.VerticalId);
                cmd.Parameters.AddWithValue("@IsActiveStatus", excelExportInputDto.IsActiveStatus);
                conn.Open();
                rdr = cmd.ExecuteReader();
                dataTable.Load(rdr);
            }
            catch (Exception e)
            {

                _logger.Error($"{ServiceName} Service-Method {_methodName} Exception: {e}");

                return dataTable;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
            return dataTable;

        }


        public DataTable PackingCostExport(ExcelExportInputDto excelExportInputDto)
        {
            _methodName = "PackingCostExport";

            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigHelper.SPConnectionString);
            SqlDataReader rdr = null;
            try
            {
                excelExportInputDto.EndDate = excelExportInputDto.StartDate;
                SqlCommand cmd = new SqlCommand("PackingCostExport", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@StartDate", excelExportInputDto.StartDate);
                //cmd.Parameters.AddWithValue("@EndDate", excelExportInputDto.EndDate);
                cmd.Parameters.AddWithValue("@VerticalId", excelExportInputDto.VerticalId);
                cmd.Parameters.AddWithValue("@IsActiveStatus", excelExportInputDto.IsActiveStatus);
                conn.Open();
                rdr = cmd.ExecuteReader();
                dataTable.Load(rdr);
            }
            catch (Exception e)
            {
                _logger.Error($"{ServiceName} Service-Method {_methodName} Exception: {e}");
                return dataTable;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
            return dataTable;

        }

        public DataTable PrimaryFreightExport(ExcelExportInputDto excelExportInputDto)
        {
            _methodName = "PrimaryFreightExport";

            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigHelper.SPConnectionString);
            SqlDataReader rdr = null;

            try
            {
                excelExportInputDto.EndDate = excelExportInputDto.StartDate;
                SqlCommand cmd = new SqlCommand("PrimaryFreightExport", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@StartDate", excelExportInputDto.StartDate);
                //cmd.Parameters.AddWithValue("@EndDate", excelExportInputDto.EndDate);
                cmd.Parameters.AddWithValue("@VerticalId", excelExportInputDto.VerticalId);
                cmd.Parameters.AddWithValue("@IsActiveStatus", excelExportInputDto.IsActiveStatus);
                conn.Open();
                rdr = cmd.ExecuteReader();
                dataTable.Load(rdr);
            }
            catch (Exception e)
            {
                _logger.Error($"{ServiceName} Service-Method {_methodName} Exception: {e}");
                return dataTable;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
            return dataTable;

        }


        public DataTable SecondaryFreightExport(ExcelExportInputDto excelExportInputDto)
        {
            _methodName = "SecondaryFreightExportToList";


            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigHelper.SPConnectionString);
            SqlDataReader rdr = null;
            try
            {
                excelExportInputDto.EndDate = excelExportInputDto.StartDate;
                SqlCommand cmd = new SqlCommand("SecondaryFreightExport", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@StartDate", excelExportInputDto.StartDate);
                //cmd.Parameters.AddWithValue("@EndDate", excelExportInputDto.EndDate);
                cmd.Parameters.AddWithValue("@VerticalId", excelExportInputDto.VerticalId);
                cmd.Parameters.AddWithValue("@IsActiveStatus", excelExportInputDto.IsActiveStatus);
                conn.Open();
                rdr = cmd.ExecuteReader();
                dataTable.Load(rdr);
            }
            catch (Exception e)
            {
                _logger.Error($"{ServiceName} Service-Method {_methodName} Exception: {e}");
                return dataTable;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
            return dataTable;

        }

        public DataTable DepotCostExport(ExcelExportInputDto excelExportInputDto)
        {
            _methodName = "DepotCostExport";
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigHelper.SPConnectionString);
            SqlDataReader rdr = null;

            try
            {
                excelExportInputDto.EndDate = excelExportInputDto.StartDate;
                SqlCommand cmd = new SqlCommand("DepotCostExport", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@StartDate", excelExportInputDto.StartDate);
                //cmd.Parameters.AddWithValue("@EndDate", excelExportInputDto.EndDate);
                cmd.Parameters.AddWithValue("@VerticalId", excelExportInputDto.VerticalId);
                cmd.Parameters.AddWithValue("@IsActiveStatus", excelExportInputDto.IsActiveStatus);
                conn.Open();
                rdr = cmd.ExecuteReader();
                dataTable.Load(rdr);
            }
            catch (Exception e)
            {
                _logger.Error($"{ServiceName} Service-Method {_methodName} Exception: {e}");
                return dataTable;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
            return dataTable;


        }


        public DataTable DetentionCostExport(ExcelExportInputDto excelExportInputDto)
        {
            _methodName = "DetentionCostExport";
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigHelper.SPConnectionString);
            SqlDataReader rdr = null;

            try
            {
                excelExportInputDto.EndDate = excelExportInputDto.StartDate;
                SqlCommand cmd = new SqlCommand("DetentionCostExport", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@StartDate", excelExportInputDto.StartDate);
                //cmd.Parameters.AddWithValue("@EndDate", excelExportInputDto.EndDate);
                cmd.Parameters.AddWithValue("@VerticalId", excelExportInputDto.VerticalId);
                cmd.Parameters.AddWithValue("@IsActiveStatus", excelExportInputDto.IsActiveStatus);
                conn.Open();
                rdr = cmd.ExecuteReader();
                dataTable.Load(rdr);
            }
            catch (Exception e)
            {
                _logger.Error($"{ServiceName} Service-Method {_methodName} Exception: {e}");
                return dataTable;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
            return dataTable;
        }


        public DataTable CushionMarginExport(ExcelExportInputDto excelExportInputDto)
        {
            _methodName = "CushionMarginExport";
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigHelper.SPConnectionString);
            SqlDataReader rdr = null;

            try
            {
                excelExportInputDto.EndDate = excelExportInputDto.StartDate;
                SqlCommand cmd = new SqlCommand("CushionMarginExport", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@StartDate", excelExportInputDto.StartDate);
                //cmd.Parameters.AddWithValue("@EndDate", excelExportInputDto.EndDate);
                cmd.Parameters.AddWithValue("@VerticalId", excelExportInputDto.VerticalId);
                cmd.Parameters.AddWithValue("@IsActiveStatus", excelExportInputDto.IsActiveStatus);
                conn.Open();
                rdr = cmd.ExecuteReader();
                dataTable.Load(rdr);
            }
            catch (Exception e)
            {
                _logger.Error($"{ServiceName} Service-Method {_methodName} Exception: {e}");
                return dataTable;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }

            return dataTable;
        }

        public DataTable ProfitMarginExport(ExcelExportInputDto excelExportInputDto)
        {
            _methodName = "ProfitMarginExport";
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigHelper.SPConnectionString);
            SqlDataReader rdr = null;
            try
            {
                excelExportInputDto.EndDate = excelExportInputDto.StartDate;
                SqlCommand cmd = new SqlCommand("ProfitMarginExport", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@StartDate", excelExportInputDto.StartDate);
                //cmd.Parameters.AddWithValue("@EndDate", excelExportInputDto.EndDate);
                cmd.Parameters.AddWithValue("@VerticalId", excelExportInputDto.VerticalId);
                cmd.Parameters.AddWithValue("@IsActiveStatus", excelExportInputDto.IsActiveStatus);
                conn.Open();
                rdr = cmd.ExecuteReader();
                dataTable.Load(rdr);
            }
            catch (Exception e)
            {
                _logger.Error($"{ServiceName} Service-Method {_methodName} Exception: {e}");
                return dataTable;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
            return dataTable;

        }


        public DataTable SchemeCostExport(ExcelExportInputDto excelExportInputDto)
        {
            _methodName = "SchemeCostExport";
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigHelper.SPConnectionString);
            SqlDataReader rdr = null;

            try
            {
                excelExportInputDto.EndDate = excelExportInputDto.StartDate;
                SqlCommand cmd = new SqlCommand("SchemeCostExport", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@StartDate", excelExportInputDto.StartDate);
                //cmd.Parameters.AddWithValue("@EndDate", excelExportInputDto.EndDate);
                cmd.Parameters.AddWithValue("@VerticalId", excelExportInputDto.VerticalId);
                cmd.Parameters.AddWithValue("@IsActiveStatus", excelExportInputDto.IsActiveStatus);
                conn.Open();
                rdr = cmd.ExecuteReader();
                dataTable.Load(rdr);
            }
            catch (Exception e)
            {
                _logger.Error($"{ServiceName} Service-Method {_methodName} Exception: {e}");
                return dataTable;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
            return dataTable;

        }


        public DataTable ExportLoadCapacity(ExcelExportInputDto excelExportInputDto)
        {
            _methodName = "ExportLoadCapacity";
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigHelper.SPConnectionString);
            SqlDataReader rdr = null;
            try
            {
                excelExportInputDto.EndDate = excelExportInputDto.StartDate;
                SqlCommand cmd = new SqlCommand("LoadCapacityExport", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@StartDate", excelExportInputDto.StartDate);
                //cmd.Parameters.AddWithValue("@EndDate", excelExportInputDto.EndDate);
                cmd.Parameters.AddWithValue("@VerticalId", excelExportInputDto.VerticalId);
                cmd.Parameters.AddWithValue("@IsActiveStatus", excelExportInputDto.IsActiveStatus);
                conn.Open();
                rdr = cmd.ExecuteReader();
                dataTable.Load(rdr);
            }
            catch (Exception e)
            {
                _logger.Error($"{ServiceName} Service-Method {_methodName} Exception: {e}");
                return dataTable;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
            return dataTable;

        }


        public DataTable ExportRAMargin(ExcelExportInputDto excelExportInputDto)
        {
            _methodName = "ExportRAMargin";
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigHelper.SPConnectionString);
            SqlDataReader rdr = null;

            try
            {
                excelExportInputDto.EndDate = excelExportInputDto.StartDate;
                SqlCommand cmd = new SqlCommand("RaMarginExport", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@StartDate", excelExportInputDto.StartDate);
                //cmd.Parameters.AddWithValue("@EndDate", excelExportInputDto.EndDate);
                cmd.Parameters.AddWithValue("@VerticalId", excelExportInputDto.VerticalId);
                cmd.Parameters.AddWithValue("@IsActiveStatus", excelExportInputDto.IsActiveStatus);
                conn.Open();
                rdr = cmd.ExecuteReader();
                dataTable.Load(rdr);
            }
            catch (Exception e)
            {
                _logger.Error($"{ServiceName} Service-Method {_methodName} Exception: {e}");
                return dataTable;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                if (rdr != null)
                {
                    rdr.Close();
                }
            }
            return dataTable;

        }


        #endregion

        #region RAMaterialCost

        /// <summary>
        /// Method to add or update RAMaterialCost
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<RAMaterialCostDto> AddOrUpdateRAMaterialCost(RAMaterialCostDto inputDto)
        {
            _methodName = "AddOrUpdateRAMaterialCost";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var apiUrl = string.Empty; var inputDtoJson = string.Empty;
                if (inputDto.Id > 0)
                { apiUrl = ApiUrl.WebApiUrlPostUpdateRAMaterialCost; }
                else { apiUrl = ApiUrl.WebApiUrlPostSaveRAMaterialCost; }

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
                        inputDto.PostStatus = true;
                        inputDto.PostMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_RAMaterialCostUpdateSuccess") : Helper.GetResourceString("msgRAMaterialCostSaveSuccess");
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
                inputDto.PostStatus = false;
                inputDto.PostMessage = Helper.GetResourceString("msg_MaterialCostError");
                _logger.Error(message);
            }
            return inputDto;
        }


        /// <summary>
        /// Method to get Get MaterialCost Details By Id
        /// </summary>
        /// <param name="materialCostId"></param>
        /// <returns></returns>
        public async Task<RAMaterialCostDto> GetRAMaterialCostDetailsById(long ramaterialCostId)
        {
            var result = new RAMaterialCostDto();
            _methodName = "GetRAMaterialCostDetailsById";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var apiUrl = ApiUrl.WebApiUrlGetRAMaterialCostDetailsById;
                if (ramaterialCostId != 0)
                {
                    var inputDtoJson = JsonHelper.ConvertObjectToJson(ramaterialCostId);
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
                            result = JsonConvert.DeserializeObject<RAMaterialCostDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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
    }
}