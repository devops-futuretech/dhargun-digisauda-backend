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
using System.Data.SqlClient;

namespace Adani.Solution.MVC.ServiceClient
{
    public class RALookupClient : BaseClient
    {
        private const string ServiceName = "RALookup Client";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;

        

        #region RA Notification
        public async Task<IList<CustomerGroupDto>> GetCustomerGroupddlAsync(LoginUserIdDto inputDto)
        {
            _methodName = "GetCustomerGroupddlAsync";
            string apiUrl = ApiUrl.WebApiUrlPostCustomerGroupDDL;
            var response = await GetListAsync<CustomerGroupDto>(apiUrl, inputDto);
            return response;
        }

        public async Task<RANotificationDto> AddOrUpdateNotification(RANotificationDto inputDto)
        {
            _methodName = "AddOrUpdateNotification";
            var addOrUpdateMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_NotificationUpdatedSuccessfully") : Helper.GetResourceString("msg_NotificationSavedSuccessfully");
            var errorMessage = Helper.GetResourceString("msg_NotificationSaveError");
            var apiUrl = inputDto.Id > 0 ? ApiUrl.WebApiUrlPostUpdateRANotification : ApiUrl.WebApiUrlPostAddRANotification;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, errorMessage);
        }

        public async Task<RANotificationDto> GetRANotificationById(IdInputDto inputDto)
        {
            _methodName = "GetRANotificationById";
            string apiUrl = ApiUrl.WebApiUrlGetRANotificationById;
            var result = await GetByInputDto<RANotificationDto>(apiUrl, inputDto);
            return result;
        }
        public async Task<IList<RANotificationDto>> GetRANotificationListAsync(LoginUserIdDto inputDto)
        {
            _methodName = "GetRANotificationListAsync";
            string apiUrl = ApiUrl.WebApiUrlGetRANotificationList;
            var response = await GetListAsync<RANotificationDto>(apiUrl, inputDto);
            return response;
        }
        public async Task<List<RaNotificationDetailDto>> GetRaNotificationDetailsById(long raNotificationId)
        {
            _methodName = "GetRaNotificationDetailsById";
            var result = await GetListAsync<RaNotificationDetailDto>(ApiUrl.WebApiUrlGetRANotificationDetails, raNotificationId);
            return result.ToList();
        }

        public async Task<Kendo.Mvc.UI.DataSourceResult> GetMappedCustomerListByRaNotificationId(RANotificationGridInputDto inputDto)
        {
            _methodName = "GetMappedCustomerListByRaNotificationId";
            var result = await GetKendoGridResultAsync<RaNotificationDetailDto>(ApiUrl.WebApiUrlGetMappedCustomerListByRaNotificationId, inputDto);
            return result;
        }
        public async Task<List<RANotificationDto>> ExportRaNotificationList(LoginUserIdDto inputDto)
        {
            _methodName = "ExportRaNotificationList";
            var result = await GetListAsync<RANotificationDto>(ApiUrl.WebApiUrlGetRANotificationExport, inputDto);
            return result.ToList();
        }
        #endregion

            
        #region Reverse Auction Final Price

        public DataTable GetReverseAuctionFinalPriceDownload(long PublishId, long CustomerGroupId, long BiddingWindowId,DateTime SearchDate)
        {
            DataTable dataTable = new DataTable();
            SqlConnection conn = new SqlConnection(ConfigHelper.SPConnectionString);
            SqlDataReader rdr = null;
            try
            {
                SqlCommand cmd = new SqlCommand("GetReverseAuctionFinalPriceDatas", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PublishId", PublishId);
                cmd.Parameters.AddWithValue("@CustomerGroupId", CustomerGroupId);
                cmd.Parameters.AddWithValue("@BiddingWindowId", BiddingWindowId);
                cmd.Parameters.AddWithValue("@SearchDate", SearchDate);
                conn.Open();
                rdr = cmd.ExecuteReader();
                dataTable.Load(rdr);
            }
            catch (Exception)
            {
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


        #region CoutrerBid Jump

        public async Task<IList<CounterBidJumpDto>> GetCounterBidJumpListAsync(LoginUserIdDto inputDto)
        {
            _methodName = "GetCounterBidJumpListAsync";
            string apiUrl = ApiUrl.WebApiUrlGetCounterBidJumpList;
            var response = await GetListAsync<CounterBidJumpDto>(apiUrl, inputDto);
            return response;
        }
        public async Task<CounterBidJumpDto> AddorUpdateCounterBidJump(CounterBidJumpDto inputDto)
        {
            _methodName = "AddorUpdateCounterBidJump";
            var addOrUpdateMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_CounterBidJumpUpdateSuccess") : Helper.GetResourceString("msg_CounterBidJumpSaveSuccess");
            var errorMessage = Helper.GetResourceString("msg_CounterBidJumpError");
            var apiUrl = inputDto.Id > 0 ? ApiUrl.WebApiUrlPostUpdateCounterBidJump : ApiUrl.WebApiUrlPostSaveCounterBidJump;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, errorMessage);
        }

        public async Task<CounterBidJumpDto> GetCounterBidJumpById(IdInputDto inputDto)
        {
            _methodName = "GetCounterBidJumpById";
            var result = await GetByInputDto<CounterBidJumpDto>(ApiUrl.WebApiUrlGetCounterBidJumpById, inputDto);
            return result;
        }
        public async Task<List<CounterBidJumpDto>> ExportCounterBidJumpList(LoginUserIdDto inputDto)
        {
            _methodName = "ExportCounterBidJumpList";
            var result = await GetListAsync<CounterBidJumpDto>(ApiUrl.WebApiUrlExportCounterBidJump, inputDto);
            return result.ToList();
        }

        #endregion

        public async Task<SaudaQuantitySaveDto> AddSaudaQuantity(SaudaQuantitySaveDto inputDto)
        {
            _methodName = "AddorUpdateCounterBidJump";
            var addOrUpdateMessage = "Sauda Quantity saved successfully";
            var errorMessage = "An error occurred while save sauda quantity, Please retry.";
            var apiUrl = ApiUrl.WebApiUrlPostSaveSaudaQuantityList;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, errorMessage);
        }

        #region RA Sauda Confoguration

        public async Task<RaSaudaAllocationListDto> AddorUpdateRASaudaConfoguration(RaSaudaAllocationListDto inputDto)
        {
            _methodName = "AddorUpdateRASaudaConfoguration";
            var addOrUpdateMessage = inputDto.Id > 0 ? "Sauda configuration updated successfully" : "Sauda configuration saved successfully";
            var errorMessage = "An error occurred while save sauda configuration, Please retry.";
            var apiUrl = inputDto.Id > 0 ? ApiUrl.WebApiUrlPostUpdateRaSaudaConfiguration : ApiUrl.WebApiUrlPostSaveRaSaudaConfiguration;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, errorMessage);
        }

        public async Task<RaSaudaAllocationListDto> GetRASaudaConfogurationById(IdInputDto inputDto)
        {
            _methodName = "GetRASaudaConfogurationById";
            var result = await GetByInputDto<RaSaudaAllocationListDto>(ApiUrl.WebApiUrlPostGetRaSaudaConfigurationById, inputDto);
            return result;
        }

        #endregion
    }
}