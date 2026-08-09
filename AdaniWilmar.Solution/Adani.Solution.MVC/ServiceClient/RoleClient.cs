using GMCore.Helper;
using GMCore.Logger;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Enums;
using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Sys = System.Configuration;

namespace Adani.Solution.MVC.ServiceClient
{
    public class RoleClient : BaseClient
    {
        private const string ServiceName = "Role Client";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;

        #region RoleType 

        /// <summary>
        /// Method to post role type
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<RoleTypeClaimViewModel> RoleTypeAsync(RoleTypeClaimViewModel roleTypeClaimViewModel)
        {
            _methodName = "RoleTypeAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var roleTypeClaim = roleTypeClaimViewModel.ClaimDto.Where(m => m.IsClaim == true).ToList();
                if (roleTypeClaim != null && roleTypeClaim.Any())
                {
                    var roleTypeClaimDto = new RoleTypeClaimDto
                    {
                        RoleType = roleTypeClaimViewModel.RoleType,
                        ClaimIds = roleTypeClaim.Select(x => x.ClaimId).ToList(),
                        LoginUserId = roleTypeClaimViewModel.LoginUserId
                    };
                    var inputDtoJson = JsonHelper.ConvertObjectToJson<RoleTypeClaimDto>(roleTypeClaimDto);
                    var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                    HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostRoleType, inputSring);
                    var responseData = await response.Content.ReadAsStringAsync();
                    responseData = UtilityHelper.TrimStartEnd(responseData);
                    var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                    if (response.IsSuccessStatusCode)
                    {
                        if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                            var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                            roleTypeClaimViewModel = new RoleTypeClaimViewModel();
                            roleTypeClaimViewModel.PostStatus = true;
                            roleTypeClaimViewModel.PostMessage = Helpers.Helper.GetResourceString("msg_RoleTypeSucess");

                        }
                        if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                            var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                            roleTypeClaimViewModel.PostStatus = false;
                            roleTypeClaimViewModel.PostMessage = errorDtoResult.Message;
                        }

                    }
                    else
                    {
                        roleTypeClaimViewModel.PostStatus = false;
                        roleTypeClaimViewModel.PostMessage = ja[0]["message"].ToString();
                    }
                }
                else
                {
                    roleTypeClaimViewModel.PostStatus = false;
                    roleTypeClaimViewModel.PostMessage = Helper.GetResourceString("msg_RoleTypeCliamSelect");
                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                roleTypeClaimViewModel.PostStatus = false;
                roleTypeClaimViewModel.PostMessage = Helper.GetResourceString("msg_RoleTypeError");
                _logger.Error(message);
            }
            return roleTypeClaimViewModel;
        }

        /// <summary>
        /// Method to delete role type
        /// </summary>
        /// <param name="roleTypeIdDto"></param>
        /// <returns></returns>
        public async Task<RoleTypeClaimUpdateViewModel> RoleTypeDeleteAsync(RoleTypeIdDto roleTypeIdDto)
        {
            var roleTypeClaimViewModel = new RoleTypeClaimUpdateViewModel();
            _methodName = "RoleTypeDeleteAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");


                var inputDtoJson = JsonHelper.ConvertObjectToJson(roleTypeIdDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlDeleteRoleType, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        roleTypeClaimViewModel = new RoleTypeClaimUpdateViewModel();
                        roleTypeClaimViewModel.PostStatus = true;
                        roleTypeClaimViewModel.PostMessage = Helpers.Helper.GetResourceString("msg_RoleTypeDeleteSucess");

                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        roleTypeClaimViewModel.PostStatus = false;
                        roleTypeClaimViewModel.PostMessage = errorDtoResult.Message;
                    }

                }
                else
                {
                    roleTypeClaimViewModel.PostStatus = false;
                    roleTypeClaimViewModel.PostMessage = ja[0]["message"].ToString();
                }


            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                roleTypeClaimViewModel.PostStatus = false;
                roleTypeClaimViewModel.PostMessage = Helper.GetResourceString("msg_RoleTypeError");
                _logger.Error(message);
            }
            return roleTypeClaimViewModel;
        }

        #endregion

        #region UpdateRoleType 

        /// <summary>
        /// Method to post role type
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<RoleTypeClaimUpdateViewModel> UpdateRoleTypeAsync(RoleTypeClaimUpdateViewModel roleTypeClaimUpdateViewModel, long UserId)
        {
            _methodName = "UpdateRoleTypeAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                //list of tuples
                List<Tuple<long, string, bool, List<int>>> roleTypeClaimDetails = new List<Tuple<long, string, bool, List<int>>>();
                //foreach (var item in roleTypeClaimUpdateViewModel.RoleTypeUpdate)
                //{
                //    var claimIds = item.ClaimDto.Where(m => m.IsClaim == true).Select(x => x.ClaimId).ToList();
                roleTypeClaimDetails.Add(new Tuple<long, string, bool, List<int>>(roleTypeClaimUpdateViewModel.RoleTypeId, roleTypeClaimUpdateViewModel.RoleTypeName, false, roleTypeClaimUpdateViewModel.ClaimIds));
                //}

                var roleTypeClaimDto = new RoleTypeClaimUpdateDto
                {
                    RoleTypeClaimIds = roleTypeClaimDetails,
                    LoginUserId = UserId
                };
                var inputDtoJson = JsonHelper.ConvertObjectToJson<RoleTypeClaimUpdateDto>(roleTypeClaimDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPutRoleType, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        roleTypeClaimUpdateViewModel = new RoleTypeClaimUpdateViewModel();
                        roleTypeClaimUpdateViewModel.PostStatus = true;
                        roleTypeClaimUpdateViewModel.PostMessage = Helpers.Helper.GetResourceString("msg_RoleTypeUpdateSucess");

                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        roleTypeClaimUpdateViewModel.PostStatus = false;
                        roleTypeClaimUpdateViewModel.PostMessage = errorDtoResult.Message;
                    }

                }
                else
                {
                    roleTypeClaimUpdateViewModel.PostStatus = false;
                    roleTypeClaimUpdateViewModel.PostMessage = ja[0]["message"].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                roleTypeClaimUpdateViewModel.PostStatus = false;
                roleTypeClaimUpdateViewModel.PostMessage = Helper.GetResourceString("msg_RoleTypeUpdateError");
                _logger.Error(message);
            }
            return roleTypeClaimUpdateViewModel;
        }

        /// <summary>
        /// Method to get role type claims
        /// </summary>       
        /// <returns></returns>
        public async Task<SystemRoleTypeClaimsDto> GetRoleTypeClaimsAsync()
        {
            var result = new SystemRoleTypeClaimsDto();
            _methodName = "GetRoleTypeClaimsAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetRoleTypeClaims);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<SystemRoleTypeClaimsDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());

                        //var twe = systemRoleTypeClaimsDto.SystemClaims.ToList();
                        //var test = systemRoleTypeClaimsDto.SystemRoleTypes.ToList();                        
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

        #region Role
        /// <summary>
        /// Method to post role type
        /// </summary>
        /// <param name="roleViewModel"></param>
        /// <returns></returns>
        public async Task<RoleViewModel> RoleAsync(RoleViewModel roleViewModel)
        {
            _methodName = "RoleAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var roleTypeClaim = roleViewModel.ClaimDto.Where(m => m.IsClaim == true).ToList();
                if (roleTypeClaim != null && roleTypeClaim.Any())
                {

                    var roleDto = new RoleDto
                    {
                        Name = roleViewModel.Name,
                        RoleTypeId = Convert.ToInt32(roleViewModel.RoleType)
                    };
                    var roleClaimDto = new RoleClaimDto
                    {
                        Role = roleDto,
                        ClaimIds = roleTypeClaim.Select(x => x.ClaimId).ToList(),
                        LoginUserId = roleViewModel.LoginUserId
                    };
                    var inputDtoJson = JsonHelper.ConvertObjectToJson<RoleClaimDto>(roleClaimDto);
                    var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                    HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostRole, inputSring);
                    var responseData = await response.Content.ReadAsStringAsync();
                    responseData = UtilityHelper.TrimStartEnd(responseData);
                    var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                    if (response.IsSuccessStatusCode)
                    {
                        if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                            var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                            roleViewModel = new RoleViewModel();
                            roleViewModel.PostStatus = true;
                            roleViewModel.PostMessage = Helpers.Helper.GetResourceString("msg_RoleSucess");

                        }
                        if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                        {
                            var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                            var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                            roleViewModel.PostStatus = false;
                            roleViewModel.PostMessage = errorDtoResult.Message;
                        }

                    }
                    else
                    {
                        roleViewModel.PostStatus = false;
                        roleViewModel.PostMessage = ja[0]["message"].ToString();
                    }
                }
                else
                {
                    roleViewModel.PostStatus = false;
                    roleViewModel.PostMessage = Helper.GetResourceString("msg_RoleTypeCliamSelect");
                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                roleViewModel.PostStatus = false;
                roleViewModel.PostMessage = Helper.GetResourceString("msg_RoleError");
                _logger.Error(message);
            }
            return roleViewModel;
        }

        /// <summary>
        /// Method to get role type claims
        /// </summary>       
        /// <returns></returns>
        public async Task<List<ClaimDto>> GetRoleTypeClaimsDetailsAsync(int roleTypeId)
        {
            var result = new List<ClaimDto>();
            _methodName = "GetRoleTypeClaimsDetailsAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var roleTypeIdDto = new RoleTypeUsersDto
                {
                    RoleTypeId = roleTypeId
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson<RoleTypeUsersDto>(roleTypeIdDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetRoleTypeClaims, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var systemRoleTypeClaimsDto = JsonConvert.DeserializeObject<SystemRoleTypeClaimsDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        foreach (var roleType in systemRoleTypeClaimsDto.SystemRoleTypes)
                        {
                            foreach (var claims in roleType.Claims)
                            {
                                var claim = new ClaimDto { ClaimId = claims.ClaimId };
                                result.Add(claim);
                            }
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
        /// Method to delete role 
        /// </summary>
        /// <param name="roleIdDto"></param>
        /// <returns></returns>
        public async Task<RoleClaimUpdateViewModel> RoleDeleteAsync(RoleIdDto roleIdDto)
        {
            var roleClaimViewModel = new RoleClaimUpdateViewModel();
            _methodName = "RoleDeleteAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");


                var inputDtoJson = JsonHelper.ConvertObjectToJson(roleIdDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlDeleteRole, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        roleClaimViewModel = new RoleClaimUpdateViewModel();
                        roleClaimViewModel.PostStatus = true;
                        roleClaimViewModel.PostMessage = Helpers.Helper.GetResourceString("msg_RoleDeleteSucess");

                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        roleClaimViewModel.PostStatus = false;
                        roleClaimViewModel.PostMessage = errorDtoResult.Message;
                    }

                }
                else
                {
                    roleClaimViewModel.PostStatus = false;
                    roleClaimViewModel.PostMessage = ja[0]["message"].ToString();
                }


            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                roleClaimViewModel.PostStatus = false;
                roleClaimViewModel.PostMessage = Helper.GetResourceString("msg_RoleTypeError");
                _logger.Error(message);
            }
            return roleClaimViewModel;
        }



        #endregion

        #region UpdateRole

        /// <summary>
        /// Method to put role 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<RoleClaimUpdateViewModel> UpdateRoleAsync(RoleClaimUpdateViewModel roleTypeClaimUpdateViewModel, long UserId)
        {
            _methodName = "UpdateRoleAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                //list of tuples
                List<Tuple<long, string, bool, List<int>>> roleTypeClaimDetails = new List<Tuple<long, string, bool, List<int>>>();
                //foreach (var item in roleTypeClaimUpdateViewModel.RoleTypeUpdate)
                //{
                // var claimIds = item.ClaimDto.Where(m => m.IsClaim == true).Select(x => x.ClaimId).ToList();
                roleTypeClaimDetails.Add(new Tuple<long, string, bool, List<int>>(roleTypeClaimUpdateViewModel.RoleId, roleTypeClaimUpdateViewModel.RoleName, false, roleTypeClaimUpdateViewModel.ClaimIds));
                //}

                var roleTypeClaimDto = new RoleClaimUpdateDto
                {
                    RoleClaimIds = roleTypeClaimDetails,
                    LoginUserId = UserId
                };
                var inputDtoJson = JsonHelper.ConvertObjectToJson<RoleClaimUpdateDto>(roleTypeClaimDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPutRole, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        roleTypeClaimUpdateViewModel = new RoleClaimUpdateViewModel();
                        roleTypeClaimUpdateViewModel.PostStatus = true;
                        roleTypeClaimUpdateViewModel.PostMessage = Helpers.Helper.GetResourceString("msg_RoleUpdateSucess");

                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        roleTypeClaimUpdateViewModel.PostStatus = false;
                        roleTypeClaimUpdateViewModel.PostMessage = errorDtoResult.Message;
                    }

                }
                else
                {
                    roleTypeClaimUpdateViewModel.PostStatus = false;
                    roleTypeClaimUpdateViewModel.PostMessage = ja[0]["message"].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                roleTypeClaimUpdateViewModel.PostStatus = false;
                roleTypeClaimUpdateViewModel.PostMessage = Helper.GetResourceString("msg_RoleUpdateError");
                _logger.Error(message);
            }
            return roleTypeClaimUpdateViewModel;
        }

        /// <summary>
        /// Method to get role type claims
        /// </summary>       
        /// <returns></returns>
        public async Task<RoleClaimViewDto> GetRoleClaimsAsync()
        {
            var result = new RoleClaimViewDto();
            _methodName = "GetRoleClaimsAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetRoleClaims);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<RoleClaimViewDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        #region OrganizationHierarchy

        /// <summary>
        /// Method to put role 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<RoleTypeHierarchyViewModel> OrganizationHierarchyAsync(List<int> roleTypeDto)
        {
            _methodName = "OrganizationHierarchyAsync";
            var roleTypeHierarchyViewModel = new RoleTypeHierarchyViewModel();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                //list of tuples
                var roleTpyeHierarchyNoList = new Collection<KeyValuePair<int, int>>();
                var indexLevel = 0;
                foreach (var item in roleTypeDto)
                {
                    indexLevel++;
                    var roleTpyeHierarchy = new KeyValuePair<int, int>(item, indexLevel);
                    roleTpyeHierarchyNoList.Add(roleTpyeHierarchy);
                }

                var roleTypeHierarchyDto = new RoleTypeHierarchyDto
                {
                    RoleTpyeHierarchyNo = roleTpyeHierarchyNoList
                };
                var inputDtoJson = JsonHelper.ConvertObjectToJson<RoleTypeHierarchyDto>(roleTypeHierarchyDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostOrgHierarchy, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));

                        roleTypeHierarchyViewModel.PostStatus = true;
                        roleTypeHierarchyViewModel.PostMessage = Helpers.Helper.GetResourceString("msg_HierarchyUpdateSucess");

                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        roleTypeHierarchyViewModel.PostStatus = false;
                        roleTypeHierarchyViewModel.PostMessage = errorDtoResult.Message;
                    }

                }
                else
                {
                    roleTypeHierarchyViewModel.PostStatus = false;
                    roleTypeHierarchyViewModel.PostMessage = ja[0]["message"].ToString();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                roleTypeHierarchyViewModel.PostStatus = false;
                roleTypeHierarchyViewModel.PostMessage = Helper.GetResourceString("msg_RoleUpdateError");
                _logger.Error(message);
            }
            return roleTypeHierarchyViewModel;
        }

        /// <summary>
        /// Method to get role type claims
        /// </summary>       
        /// <returns></returns>
        public async Task<List<CorporateData>> GetHierarchyChartAsync()
        {
            var corporateData = new List<CorporateData>();
            _methodName = "GetHierarchyChartAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetOrgHierarchy);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        var result = JsonConvert.DeserializeObject<OrganizationHierarchyDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
                        if (result != null)
                        {
                            var cData = new List<CorporateData>();

                            foreach (var item in result.OrganizationHierarchy)
                            {
                                corporateData.Add(new CorporateData(item.Item4, "", item.Item1, item.Item5, "#10c4bb", item.Item2, item.Item3));
                            }
                            var roleTypeId = new List<int>();
                            for (var i = result.OrganizationHierarchy.Count - 1; i >= 0; i--)
                            {
                                var roleId = result.OrganizationHierarchy[i].Item2;
                                var subRoleDetails = corporateData.Where(e => e.HierarchyId == roleId).ToList();
                                if (subRoleDetails != null && subRoleDetails.Any())
                                {
                                    corporateData.FirstOrDefault(e => e.HierarchyId == result.OrganizationHierarchy[i].Item3).Items.AddRange(subRoleDetails);
                                    corporateData.RemoveAll(e => subRoleDetails.Any(a => a.Title == e.Title));
                                }

                            }

                        }
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return corporateData;
        }


        #endregion

        /// <summary>
        /// Method to get claim details
        /// </summary>       
        /// <returns></returns>
        public async Task<List<RoleTypeViewModel>> GetClaimDetailsAsync()
        {
            var result = new List<RoleTypeViewModel>();
            _methodName = "GetClaimDetailsAsync";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                HttpResponseMessage response = GetAsync(ApiUrl.WebApiUrlGetClaims);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<RoleTypeViewModel>>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
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

        public async Task<List<DropDownDto>> GetReportingToRoles(IdInputDto idInputDto)
        {
            try
            {
                _methodName = "GetReportingToRoles";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson<IdInputDto>(idInputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetReportinToRoles, inputSring);
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


        #region Process/Role Hierarchy

        public async Task<List<RoleHierarchyDto>> GetRoleHierarchyByProcess(RoleHierarchyParamDto inputDto)
        {
            var result = new List<RoleHierarchyDto>();
            _methodName = "GetRoleHierarchyByProcess";
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");
                var inputDtoJson = JsonHelper.ConvertObjectToJson<RoleHierarchyParamDto>(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetRoleHierarchyByProcess, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseSuccess].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseSuccess].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<List<RoleHierarchyDto>>(jarray[0][Settings.Response].ToString(), UtilityHelper.GetJsonSettings());
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
        /// Method to put role 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<RoleHierarchyViewModel> RoleHierarchyAsync(List<int> roleTypeDto, long loginUserId)
        {
            _methodName = "RoleHierarchyAsync";
            var roleHierarchyViewModel = new RoleHierarchyViewModel();
            try
            {
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                //list of tuples
                var roleHierarchyNoList = new Collection<KeyValuePair<int, int>>();
                var indexLevel = 0;
                foreach (var item in roleTypeDto)
                {
                    indexLevel++;
                    var roleTpyeHierarchy = new KeyValuePair<int, int>(item, indexLevel);
                    roleHierarchyNoList.Add(roleTpyeHierarchy);
                }

                var roleHierarchyDto = new RoleHierarchyDto
                {
                    RoleHierarchyNo = roleHierarchyNoList,
                    LoginUserId = loginUserId
                };

                var inputDtoJson = JsonHelper.ConvertObjectToJson(roleHierarchyDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlPostRoleHierarchy, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));

                        roleHierarchyViewModel.PostStatus = true;
                        roleHierarchyViewModel.PostMessage = Helpers.Helper.GetResourceString("msg_HierarchyUpdateSucess");

                    }
                    if (!string.IsNullOrEmpty(ja[0]["SXVI7XCEU"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["SXVI7XCEU"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        roleHierarchyViewModel.PostStatus = false;
                        roleHierarchyViewModel.PostMessage = errorDtoResult.Message;
                    }

                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                roleHierarchyViewModel.PostStatus = false;
                roleHierarchyViewModel.PostMessage = Helper.GetResourceString("msg_RoleUpdateError");
                _logger.Error(message);
            }
            return roleHierarchyViewModel;
        }

        public async Task<List<DropDownDto>> GetReportingToUsersByRole(ReportingUsersInputDto inputDto)
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

        #endregion

        #region Reporting To users

        public async Task<List<DropDownDto>> GetOrganizationReportingToUsersByUserId(ReportingUsersInputDto inputDto)
        {
            try
            {
                _methodName = "GetReportingToUsersByUserId";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetOrganizationReportingToUsersByUserId, inputSring);
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

        public async Task<List<DropDownDto>> GetSalesReportingToUsersByUserId(ReportingUsersInputDto inputDto)
        {
            try
            {
                _methodName = "GetReportingToUsersByUserId";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSalesReportingToUsersByUserId, inputSring);
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
        public async Task<List<DropDownDto>> GetSalesReportingToUsersByCityId(ReportingUsersInputDto inputDto)
        {
            try
            {
                _methodName = "GetReportingToUsersByUserId";
                _logger.Info($"{ServiceName} Controller-Method {_methodName}");

                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlGetSalesReportingToUsersByCityId, inputSring);
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

        public async Task<List<DropDownDto>> GetSalesReportingToUsersByCityStateDistrict(ReportingUsersInputDto inputDto)
        {

            _methodName = "GetReportingToUsersByUserId";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");

            return (List<DropDownDto>)await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetSalesReportingToUsersByCityDistrictState, inputDto);
            
        }

        public async Task<List<DropDownDto>> GetReportingToZonalHeadUsersByUserId(LoginUserIdDto inputDto)
        {
            _methodName = "GetReportingToZonalHeadUsersByUserId";
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetReportingToZonalHeadUsersByUserId, inputDto);
            return result.ToList();
        }

        public async Task<List<DropDownDto>> GetReportingToBDOUsersByUserId(LoginUserIdDto inputDto)
        {
            _methodName = "GetReportingToBDOUsersByUserId";
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetReportingToBDOUsersByUserId, inputDto);
            return result.ToList();
        }

        public async Task<List<DropDownDto>> GetReportingToRABDOUsersByUserId(LoginUserIdDto inputDto)
        {
            _methodName = "GetReportingToRABDOUsersByUserId";
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetReportingToRABDOUsersByUserId, inputDto);
            return result.ToList();
        }

        public async Task<List<ClaimDto>> GetClaimsbyRoleId(RoleIdDto inputDto)
        {
            _methodName = "GetClaimsbyRoleId";
            var result = await GetListAsync<ClaimDto>(ApiUrl.WebApiUrlGetClaimsByRoleId, inputDto);
            return result.ToList();
        }

        public async Task<List<ClaimDto>> GetClaimsbyRoleTypeId(RoleIdDto inputDto)
        {
            _methodName = "GetClaimsbyRoleTypeId";
            var result = await GetListAsync<ClaimDto>(ApiUrl.WebApiUrlGetClaimsbyRoleTypeId, inputDto);
            return result.ToList();
        }

        #endregion
    }
}