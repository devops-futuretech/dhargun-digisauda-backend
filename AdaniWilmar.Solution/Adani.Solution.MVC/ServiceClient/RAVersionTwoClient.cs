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
using Adani.Solution.DTO.Common;
using Adani.Solution.DTO.Enums;
using Kendo.Mvc.UI;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Adani.Solution.MVC.ServiceClient
{
    public class RAVersionTwoClient : BaseClient
    {
        private const string ServiceName = "RAVersionTwo Client";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;

        static string connectionString = ConfigHelper.SPConnectionString;

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

        #region  Customer Group  

        /// <summary>
        /// Method to Get Customer Group List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<List<CustomerGroupDto>> GetCustomerGroupList(LoginUserIdDto inputDto)
        {
            _methodName = "GetCustomerGroupList";
            var result = await GetListAsync<CustomerGroupDto>(ApiUrl.WebApiUrlGetCustomerGroupList, inputDto);
            return result.ToList();
        }

        /// <summary>
        /// Method to Get Customer Group Detail List By Group Id
        /// </summary>
        /// <param name="customerGroupId"></param>
        /// <returns></returns>
        public async Task<CustomerGroupDto> GetCustomerGroupByGroupId(long customerGroupId)
        {
            _methodName = "GetCustomerGroupByGroupId";
            var result = await GetById<CustomerGroupDto>(ApiUrl.WebApiUrlGetCustomerGroupByGroupId, customerGroupId);
            return result;
        }

        /// <summary>
        /// Method to Get Kendo Grid Data Async
        /// </summary>
        /// <param name="customerGroupId"></param>
        /// <returns></returns>
        public async Task<List<CustomerGroupDetailDto>> GetCustomerGroupDetailListByGroupId(long customerGroupId)
        {
            _methodName = "GetCustomerGroupDetailListByGroupId";
            var result = await GetListAsync<CustomerGroupDetailDto>(ApiUrl.WebApiUrlGetCustomerGroupDetailListByGroupId, customerGroupId);
            return result.ToList();
        }

        /// <summary>
        /// Method to Add Or Update Customer Group
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<CustomerGroupDto> AddOrUpdateCustomerGroup(CustomerGroupDto inputDto)
        {
            _methodName = "AddOrUpdateCustomerGroup";
            var addOrUpdateMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_CustomerGroupUpdateSuccess") : Helper.GetResourceString("msg_CustomerGroupSaveSuccess");
            var errorMessage = Helper.GetResourceString("msg_CustomerGroupSaveError");
            var apiUrl = inputDto.Id > 0 ? ApiUrl.WebApiUrlPostUpdateCustomerGroup : ApiUrl.WebApiUrlPostSaveCustomerGroup;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, errorMessage);
        }

        /// <summary>
        /// Method to Get Customer List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<List<CustomerGroupDetailDto>> GetCustomerList(CustomerGroupInputDto inputDto)
        {
            _methodName = "GetCustomerList";
            var result = await GetListAsync<CustomerGroupDetailDto>(ApiUrl.WebApiUrlGetCustomerList, inputDto);
            return result.ToList();
        }

        /// <summary>
        /// Method to Get Mapped Customer List By Customer Group Id
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<DataSourceResult> GetMappedCustomerListByCustomerGroupId(CustomerGroupInputDto inputDto)
        {
            _methodName = "GetMappedCustomerListByCustomerGroupId";
            var result = await GetKendoGridResultAsync<CustomerGroupDetailDto>(ApiUrl.WebApiUrlGetMappedCustomerListByCustomerGroupId, inputDto);
            return result;
        }

        /// <summary>
        /// Method to Get Customer List By Customer Group Id And StateTrader For Dropdown
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<List<DropDownDto>> GetCustomerListByCustomerGroupIdAndBDOForDropdown(CustomerGroupInputDto inputDto)
        {
            _methodName = "GetCustomerListByCustomerGroupIdForDropdown";
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetCustomerListByCustomerGroupIdAndBDOForDropdown, inputDto);
            return result.ToList();
        }

        /// <summary>
        /// Method to Get Customer List By Customer Group Id And StateTrader For Dropdown
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public List<DropDownDto> GetCustomerListByCustomerGroupIdsBDOIdsAndPercentileForDropdown(SurpriseBenefitPercentileInputDto inputDto)
        {
            _methodName = "GetCustomerListByCustomerGroupIdsBDOIdsAndPercentileForDropdown";
            List<DropDownDto> customerDetails = new List<DropDownDto>();

            //var customerList = GetCustomerSaudasForSurpriseBenefits(inputDto);

            //if (customerList.IsAny())
            //{
            //    using (IDbConnection conn = new SqlConnection(connectionString))
            //    {
            //        var userIds = customerList.Select(s => s.CustomerId).Distinct().ToList();
            //        StringBuilder sb = new StringBuilder();
            //        sb.Append(" Select Id,Name From Users Where Id in  @UserIds");
            //        customerDetails = conn.Query<DropDownDto>(sb.ToString(),
            //       new
            //       {
            //           UserIds = userIds
            //       }).ToList();
            //    }
            //}
            return customerDetails;
        }

        public List<DropDownDto> GetCustomerListBasedOnCityIdsAndPercentileNumberForDropdown(SurpriseBenefitPercentileInputDto inputDto)
        {
            _methodName = "GetCustomerListBasedOnCityIdsAndPercentileNumberForDropdown";
            List<DropDownDto> customerDetails = new List<DropDownDto>();
            //var customerList = GetCityBasedCustomerSaudasForSurpriseBenefits(inputDto);

            //if (customerList.IsAny())
            //{
            //    using (IDbConnection conn = new SqlConnection(connectionString))
            //    {
            //        var userIds = customerList.Select(s => s.CustomerId).Distinct().ToList();
            //        StringBuilder sb = new StringBuilder();
            //        sb.Append(" Select Id,Name From Users Where Id in  @UserIds");
            //        customerDetails = conn.Query<DropDownDto>(sb.ToString(),
            //       new
            //       {
            //           UserIds = userIds
            //       }).ToList();
            //    }
            //}
            return customerDetails;
        }

        /// <summary>
        /// Method to Remove Customers From Customer Group
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<CustomerDeleteInputDto> RemoveCustomersFromCustomerGroup(CustomerDeleteInputDto inputDto)
        {
            _methodName = "DeleteCustomerFromCustomerGroup";
            var message = Helper.GetResourceString("msg_CustomerDeleteSuccess");
            var errorMessage = Helper.GetResourceString("msg_CustomerDeleteError");
            var apiUrl = ApiUrl.WebApiUrlPostRemoveCustomersFromCustomerGroup;
            return await AddOrUpdate(apiUrl, inputDto, message, errorMessage);
        }

        /// <summary>
        /// Method to Export Customer Group
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<List<CustomerGroupDto>> ExportCustomerGroup(LoginUserIdDto inputDto)
        {
            _methodName = "ExportCustomerGroup";
            var result = await GetListAsync<CustomerGroupDto>(ApiUrl.WebApiUrlExportCustomerGroup, inputDto);
            return result.ToList();
        }

        /// <summary>
        /// Method to Get Customer Group List By Vertical For Dropdown
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<List<DropDownDto>> GetCustomerGroupListByVerticalForDropdown(LoginUserIdDto inputDto)
        {
            _methodName = "GetCustomerGroupListByVerticalForDropdown";
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetCustomerGroupListByVerticalForDropdown, inputDto);
            return result.ToList();
        }

        public async Task<List<DropDownDto>> GetCustomerGroupListByVerticalIdsForDropdown(IdInputDto inputDto)
        {
            _methodName = "GetCustomerGroupListByVerticalIdsddl";
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetCustomerGroupListByVerticalIdsForDropdown, inputDto);
            return result.ToList();
        }

        public async Task<IList<DropDownDto>> GetBiddingWindowCustomerGroupListForddl(IdInputDto inputDto)
        {
            _methodName = "GetBiddingWindowCustomerGroupListForddl";
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetBiddingWindowCustomerGroupListForddl, inputDto);
            return result;
        }

        #endregion

        #region SchemeDiscount - GeographyBased 

        /// <summary>
        /// Method to Add Or Update Geography Based SchemeDiscount
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<SchemeDiscountGeographyDto> AddOrUpdateGeographyBasedSchemeDiscount(SchemeDiscountGeographyDto inputDto)
        {
            _methodName = "AddOrUpdateGeographyBasedSchemeDiscount";
            var addOrUpdateMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_SchemeDiscountUpdateSuccess") : Helper.GetResourceString("msg_SchemeDiscountSaveSuccess");
            var errorMessage = Helper.GetResourceString("msg_SchemeDiscountSaveError");
            var apiUrl = inputDto.Id > 0 ? ApiUrl.WebApiUrlPostUpdateGeographyBasedSchemeDiscount : ApiUrl.WebApiUrlPostSaveGeographyBasedSchemeDiscount;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, errorMessage);
        }

        /// <summary>
        /// Method to Get SchemeDiscount Geography Details By Id
        /// </summary>
        /// <param name="schemeDiscountId"></param>
        /// <returns></returns>
        public async Task<DataSourceResult> GetSchemeDiscountGeographyHierarchyListById(ListInputDto inputDto)
        {
            _methodName = "GetSchemeDiscountGeographyDetailsById";
            var result = await GetKendoGridResultAsync<SchemeDiscountGeographyMappingDto>(ApiUrl.WebApiUrlGetSchemeDiscountGeographyHierarchyListById, inputDto);
            return result;
        }

        /// <summary>
        /// Method to Get Geography Based SchemeDiscount By Discount Id
        /// </summary>
        /// <param name="schemeDiscountId"></param>
        /// <returns></returns>
        public async Task<SchemeDiscountGeographyDto> GetGeographyBasedSchemeDiscountByDiscountId(long schemeDiscountId)
        {
            _methodName = "GetGeographyBasedSchemeDiscountByDiscountId";
            var result = await GetById<SchemeDiscountGeographyDto>(ApiUrl.WebApiUrlGetGeographyBasedSchemeDiscountByDiscountId, schemeDiscountId);
            return result;
        }

        /// <summary>
        /// Method to Export Geography Based SchemeDiscount
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public List<SchemeDiscountGeographyExportDto> ExportSchemeDiscountGeography(ExcelReportFilterDto inputDto)
        {
            _methodName = "ExportSchemeDiscountGeography";
            var result = new List<SchemeDiscountGeographyExportDto>();
            try
            {
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    string reportQuery = @"SELECT CONVERT(VARCHAR(10), SD.ValidFrom, 101) as ValidFrom, CONVERT(VARCHAR(10), SD.ValidTo, 101) as ValidTo,SD.Name As SchemeName,D.Name As Division,SD.TargetQuantity,SDM.IsActive,Sku.SkuName,
                         Customer.Code UserCode,(OilType.Name+'-'+so.Code+'/'+dist.Code+'/'+div.Code ) as OilTypeName,City.CityName CityName,pg.Name as PackGroup, Sku.SkuCode,Customer.Name UserName
                         FROM SchemeDiscountGeographies SD 
                         LEFT JOIN SchemeDiscountGeographyMappings SDM ON SD.Id = SDM.SchemeDiscountGeographyId 
                         LEFT JOIN Skus Sku on Sku.Id = SDM.SkuId  
                         LEFT JOIN Divisions D ON D.Id = Sku.DivisionId
                         LEFT JOIN Cities City on City.Id = SDM.CityId  
                         LEFT JOIN OilTypes OilType on OilType.Id = Sku.OilTypeId 
						 left join SalesOrganizations so on so.Id=OilType.SalesOrganizationId
						 left join DistributionChannels dist on dist.Id=OilType.DistributionChannelId
						 left join Divisions div on div.Id =OilType.DivisionId
                         LEFT JOIN Users Customer ON Customer.Id = SDM.CustomerId
                         LEFT JOIN PackGroups pg ON pg.Id = Sku.PackGroupId
                         WHERE (DATEADD(dd, DATEDIFF(dd, 0, @FromDate),0) <=  DATEADD(dd, DATEDIFF(dd, 0, SD.CreatedDate),0))  
                         AND (DATEADD(dd, DATEDIFF(dd, 0, SD.CreatedDate),0) <=  DATEADD(dd, DATEDIFF(dd, 0, @ToDate),0))";

                    result = conn.Query<SchemeDiscountGeographyExportDto>(reportQuery,
                    new
                    {
                        inputDto.FromDate,
                        inputDto.ToDate,
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

        public async Task<IdDiscountAndBenefitInputDto> UpdateGeographyBasedSchemeDiscountByIsActive(IdDiscountAndBenefitInputDto inputDto)
        {
            _methodName = "UpdateGeographyBasedVolumeDiscountByIsActive";
            var updateMessage = Helper.GetResourceString("msg_BiddingWindowStopedSuccessfully");
            var errorMessage = Helper.GetResourceString("msg_BiddingWindowError");
            return await AddOrUpdate<IdDiscountAndBenefitInputDto>(ApiUrl.WebApiUrlPostUpdateSchemeDiscountGeographyListByIsActive, inputDto, updateMessage, errorMessage);
        }

        /// <summary>
        /// Method to Get Scheme Discount History Geography List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public List<SchemeDiscountGeographyDto> GetSchemeDiscountHistoryGeographyListAsync()
        {
            _methodName = "GetSchemeDiscountHistoryGeographyListAsync";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<SchemeDiscountGeographyDto> result = new List<SchemeDiscountGeographyDto>();

            using (IDbConnection conn = new SqlConnection(connectionString))
            {

                //string query = @"Select Distinct sd.DiscountId as Id,sd.Id as SchemeDiscountHistoryId,sd.Name,v.Name as Vertical,sd.Discount,sd.ValidFrom,sd.ValidTo
                //        From SchemeDiscountHistories sd
                //        Left Join SchemeDiscountGeographyMappings sdm on sd.DiscountId = sdm.SchemeDiscountGeographyId
                //        Left Join Skus sk on sk.Id = sdm.SkuId
                //        Left Join Verticals v on v.Id = sk.VerticalId
                //        where sd.DiscountType = @DiscountType";
                string query = @"Select Distinct sd.Id,sd.Name,d.Name as Vertical,sd.TargetQuantity,sd.ValidFrom,sd.ValidTo
                        ,(Select TOP 1 IsActive FROM SchemeDiscountGeographyMappings WHERE SchemeDiscountGeographyId = sd.Id ORDER BY IsActive DESC) as IsActive
                        From SchemeDiscountGeographies sd
                        Left Join SchemeDiscountGeographyMappings sdm on sd.Id = sdm.SchemeDiscountGeographyId
                        Left Join Skus sk on sk.Id = sdm.SkuId
                        Left Join Divisions d on d.Id = sk.DivisionId";

                result = conn.Query<SchemeDiscountGeographyDto>(query, new
                {
                    DiscountType = (int)DTO.Enums.RADiscountTypes.SchemeDiscountGeography
                }).ToList();
            }
            return result;
        }

        /// <summary>
        /// Method to Get Scheme Discount History User List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public List<SchemeDiscountUserDto> GetSchemeDiscountHistoryUserListAsync()
        {
            _methodName = "GetSchemeDiscountHistoryUserListAsync";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<SchemeDiscountUserDto> result = new List<SchemeDiscountUserDto>();

            using (IDbConnection conn = new SqlConnection(connectionString))
            {

                string query = @"Select Distinct sd.DiscountId as Id,sd.Id as SchemeDiscountHistoryId,sd.Name,v.Name as Vertical,sd.Discount,sd.ValidFrom,sd.ValidTo
                    From SchemeDiscountHistories sd
                    Left Join SchemeDiscountUserMappings sdm on sd.DiscountId = sdm.SchemeDiscountUserId
                    Left Join Skus sk on sk.Id = sdm.SkuId
                    Left Join Verticals v on v.Id = sk.VerticalId
                    where sd.DiscountType = @DiscountType";

                result = conn.Query<SchemeDiscountUserDto>(query, new
                {
                    DiscountType = (int)DTO.Enums.RADiscountTypes.SchemeDiscountUser
                }).ToList();
            }
            return result;
        }
        /// <summary>
        /// Method to Export Scheme Discount Geography History
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public List<SchemeDiscountGeographyExportDto> ExportSchemeDiscountGeographyHistory(ExcelReportFilterDto inputDto)
        {
            _methodName = "ExportSchemeDiscountGeographyHistory";
            var result = new List<SchemeDiscountGeographyExportDto>();
            try
            {
                using (IDbConnection conn = new SqlConnection(connectionString))
                {
                    //string reportQuery = @"Select Distinct sd.Id,sd.Name,d.Name as Vertical,sd.TargetQuantity,sd.ValidFrom,sd.ValidTo
                    //    ,(Select TOP 1 IsActive FROM SchemeDiscountGeographyMappings WHERE SchemeDiscountGeographyId = sd.Id ORDER BY IsActive DESC) as IsActive
                    //    From SchemeDiscountGeographies sd
                    //    Left Join SchemeDiscountGeographyMappings sdm on sd.Id = sdm.SchemeDiscountGeographyId
                    //    Left Join Skus sk on sk.Id = sdm.SkuId
                    //    Left Join Divisions d on d.Id = sk.DivisionId";
                    string reportQuery = @"  SELECT CONVERT(VARCHAR(10), SD.ValidFrom, 101) as ValidFrom, CONVERT(VARCHAR(10), SD.ValidTo, 101) as ValidTo,SD.Name SchemeName,SDM.IsActive,SD.Discount,Sku.SkuName,
                         Customer.Code UserCode,OilType.Name OilTypeName,CG.Name CustomerGroup,City.CityName CityName,pg.Name as PackGroup, Sku.SkuCode,Customer.Name UserName
                         FROM SchemeDiscountHistories SD 
                         LEFT JOIN SchemeDiscountGeographyMappings SDM ON SD.DiscountId = SDM.SchemeDiscountGeographyId 
                         LEFT JOIN Skus Sku on Sku.Id = SDM.SkuId  
                         LEFT JOIN Cities City on City.Id = SDM.CityId  
                         LEFT JOIN OilTypes OilType on OilType.Id = Sku.OilTypeId  
                         LEFT JOIN Users Customer ON Customer.Id = SDM.CustomerId
                         LEFT JOIN CustomerGroups CG ON CG.Id = SDM.CustomerGroupId
                         LEFT JOIN PackGroups pg ON pg.Id = Sku.PackGroupId
                         WHERE (DATEADD(dd, DATEDIFF(dd, 0, @FromDate),0) <=  DATEADD(dd, DATEDIFF(dd, 0, SD.CreatedDate),0))  
                         AND (DATEADD(dd, DATEDIFF(dd, 0, SD.CreatedDate),0) <=  DATEADD(dd, DATEDIFF(dd, 0, @ToDate),0))
                         AND DiscountType = @DiscountType";

                    result = conn.Query<SchemeDiscountGeographyExportDto>(reportQuery,
                    new
                    {
                        inputDto.FromDate,
                        inputDto.ToDate,
                        DiscountType = (int)DTO.Enums.RADiscountTypes.SchemeDiscountGeography
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

        #endregion

        #region Reporting To Users - Customer Group

        public async Task<List<DropDownDto>> GetRAZonalHeadUsersByCustomerGroup(CustomerGroupInputDto inputDto)
        {
            _methodName = "GetReportingToRAZonalHeadUsersByCustomerGroup";
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetReportingToRAZonalHeadUsersByCustomerGroup, inputDto);
            return result.ToList();
        }

        public async Task<List<DropDownDto>> GetRAZonalHeadUsersByCustomerGroupIdsAndVerticalIds(DropDownInputDto inputDto)
        {
            _methodName = "GetRAZonalHeadUsersByCustomerGroupIdsAndVerticalIds";
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetRAZonalHeadUsersByCustomerGroupIdsAndVerticalIds, inputDto);
            return result.ToList();
        }

        public async Task<List<DropDownDto>> GetRABDOUsersByZonalHeadIdsAndVerticalIds(DropDownInputDto inputDto)
        {
            _methodName = "GetRABDOUsersByZonalHeadIdsAndVerticalIds";
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetRABDOUsersByZonalHeadIdsAndVerticalIds, inputDto);
            return result.ToList();
        }

        /// <summary>
        /// Method to Get Customer List By Customer Group Id And StateTrader For Dropdown
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public async Task<List<DropDownDto>> GetCustomerListByCustomerGroupIdsAndBDOsForDropdown(DropDownInputDto inputDto)
        {
            _methodName = "GetCustomerListByCustomerGroupIdsAndBDOsForDropdown";
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetCustomerListByCustomerGroupIdsAndBDOsForDropdown, inputDto);
            return result.ToList();
        }

        public async Task<List<DropDownDto>> GetCustomerListByCustomerGroupIdsCityIdsForDropdown(DropDownInputDto inputDto)
        {
            _methodName = "GetCustomerListByCustomerGroupIdsCityIdsForDropdown";
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetCustomerListByCustomerGroupIdsCityIdsForDropdown, inputDto);
            return result.ToList();
        }

        #endregion

        #region CustomerGroupMappings

        public async Task<List<DropDownDto>> GetCustomerGroupListNotMappedInCustomerGroupMappings(long CustomerGroupMappingId)
        {
            _methodName = "GetCustomerGroupListNotMappedInCustomerGroupMappings";
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetCustomerGroupListNotMappedInCustomerGroupMappings, CustomerGroupMappingId);
            return result.ToList();
        }

        public async Task<List<DropDownDto>> GetDerivedCustomerGroupListNotMappedToderviedCustomerGroup(long CustomerGroupMappingId)
        {
            _methodName = "GetDerivedCustomerGroupListNotMappedToderviedCustomerGroup";
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetDerivedCustomerGroupListNotMappedInCustomerGroupMappings, CustomerGroupMappingId);
            return result.ToList();
        }

        public async Task<CustomerGroupMappingDto> AddOrUpdateCustomerGroupMappings(CustomerGroupMappingDto inputDto)
        {
            _methodName = "AddOrUpdateCustomerGroupMappings";
            var addOrUpdateMessage = inputDto.CustomerGroupId > 0 && inputDto.Id > 0 ? Helper.GetResourceString("msg_CustomerGroupMappingUpdate") : Helper.GetResourceString("msg_CustomerGroupMappingSave");
            var errorMessage = Helper.GetResourceString("msg_ErrorCustomerGroupMapping");
            var apiUrl = ApiUrl.WebApiUrlPostAddorUpdateCustomerGroupMapping;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, errorMessage);
        }

        public async Task<List<CustomerGroupMappingGridDto>> GetCustomerGroupMappingListDetailsById(long customergroupMappingId)
        {
            _methodName = "GetCustomerGroupMappingListDetailsById";
            var result = await GetListAsync<CustomerGroupMappingGridDto>(ApiUrl.WebApiUrlCustomerGroupMappingListDetailsById, customergroupMappingId);
            return result.ToList();
        }

        public async Task<CustomerGroupMappingDto> GetCustomerGroupMappingsByCustomerGroupMappingId(long customerGroupMappingId)
        {
            _methodName = "GetCustomerGroupMappingsByCustomerGroupMappingId";
            var result = await GetById<CustomerGroupMappingDto>(ApiUrl.WebApiUrlGetCustomerGroupMappingByCustomerGroupMappingId, customerGroupMappingId);
            return result;
        }

        #endregion

        #region RA2 Notification

        public async Task<RANotificationDto> AddOrUpdateNotification(RANotificationDto inputDto)
        {
            _methodName = "AddOrUpdateNotification";
            var addOrUpdateMessage = Helper.GetResourceString("msg_NotificationSavedSuccessfully");
            var errorMessage = Helper.GetResourceString("msg_NotificationSaveError");
            var apiUrl = ApiUrl.WebApiUrlPostAddRA2Notification;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, errorMessage);
        }
        public async Task<IdDiscountAndBenefitInputDto> UpdateRA2NotificationByIsActive(IdDiscountAndBenefitInputDto inputDto)
        {
            _methodName = "UpdateRA2NotificationByIsActive";
            var updateMessage = Helper.GetResourceString("msg_BiddingWindowStopedSuccessfully");
            var errorMessage = Helper.GetResourceString("msg_BiddingWindowError");
            return await AddOrUpdate<IdDiscountAndBenefitInputDto>(ApiUrl.WebApiUrlPostUpdateRA2NotificationListByIsActive, inputDto, updateMessage, errorMessage);
        }
        #endregion

    }
}