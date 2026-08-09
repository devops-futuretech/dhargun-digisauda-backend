using Dapper;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Enums;
using Adani.Solution.MVC.Common;
using Adani.Solution.MVC.Helpers;
using Adani.Solution.MVC.Models;
using GMCore.Helper;
using GMCore.Logger;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Text;
using Adani.Solution.DTO.Common;
using Kendo.Mvc.Extensions;

namespace Adani.Solution.MVC.ServiceClient
{
    public class ReverseAuctionClient : BaseClient
    {
        private const string ServiceName = "Reverse Auction Client";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;
        static string connectionString = ConfigHelper.SPConnectionString;

        //protected async Task<T> GetById<T>(string apiUrl, long Id, T result) where T : IAPIInputDTO
        //{
        //    _logger.Info($"{ServiceName} Controller-Method {_methodName}");
        //    try
        //    {
        //        if (Id != 0)
        //        {
        //            var inputDtoJson = JsonHelper.ConvertObjectToJson(Id);
        //            var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
        //            HttpResponseMessage response = PostAsync(apiUrl, inputSring);
        //            var responseData = await response.Content.ReadAsStringAsync();
        //            responseData = UtilityHelper.TrimStartEnd(responseData);
        //            var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
        //            if (response.IsSuccessStatusCode)
        //            {
        //                if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
        //                {
        //                    var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
        //                    var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
        //                    result = JsonConvert.DeserializeObject<T>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());
        //                }
        //                if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
        //                {
        //                    var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
        //                    var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
        //                    result.PostStatus = false;
        //                    result.PostMessage = errorDtoResult.Message;
        //                }
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



        public async Task<BiddingWindowTimingDto> AddOrUpdateBidWindowTiming(BiddingWindowTimingDto inputDto)
        {
            _methodName = "AddOrUpdateBidWindowTiming";
            var addOrUpdateMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_BiddingWindowUpdateSuccess") : Helper.GetResourceString("msg_BiddingWindowCreateSuccess");
            return await AddOrUpdate(ApiUrl.WebApiUrlPostBidWindowTiming, inputDto, addOrUpdateMessage, "Error");
        }
        public async Task<BiddingWindowTimingDto> BidddingWindowTiming(long inputDto)
        {
            _methodName = "EditBidWIndowTiming";
            return await GetById<BiddingWindowTimingDto>(ApiUrl.WebApiUrGetBiddingWindowTimingById, inputDto);
        }

        public async Task<IList<BiddingWindowTimingDto>> GetBiddingWindowTimingList(LoginUserIdDto inputDto)
        {
            _methodName = "GetBiddingWindowTimingList";
            var response = await GetListAsync<BiddingWindowTimingDto>(ApiUrl.WebApiUrlGetBiddingWindowTimingList, inputDto);
            return response;
        }
        public async Task<IList<SaudaListOutputDto>> GetAllSaudhaList(LoginUserIdDto inputDto)
        {
            _methodName = "GetAllSaudhaList";
            var response = await GetListAsync<SaudaListOutputDto>(ApiUrl.WebApiUrlGetAllSaudhaList, inputDto);
            return response;
        }

        public async Task<IList<DropDownDto>> GetBiddingWindowTimingListddl(LoginUserIdDto inputDto)
        {
            _methodName = "GetBiddingWindowTimingListddl";
            var response = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetBiddingWindowTimingListddl, inputDto);
            return response;
        }


        public async Task<List<BidWindowListDto>> ExportBidWindow(BidWindowListSearchDto inputDto)
        {
            _methodName = "ExportBidWindow";
            var result = await GetListAsync<BidWindowListDto>(ApiUrl.WebApiUrlExportBidWindow, inputDto);
            return result.ToList();
        }


        public async Task<SaudaListDto> GetSaudhaDetails(SaudaDetailInputDto inputDto)
        {
            _methodName = "GetSaudhaDetail";
            var response = await GetByInputDto<SaudaListDto>(ApiUrl.WebApiUrlGetSaudhaDetails, inputDto);
            return response;
        }

        public async Task<DataSourceResult> GetLiftingRequestList(DealersLiftingRequestInputDto inputDto)
        {
            _methodName = "GetAllLiftingRequestList";

            List<LiftingRequestOutputDto> result = new List<LiftingRequestOutputDto>();
            List<long> statusIds = new List<long>();
            if (inputDto.StatusId > 0)
            {
                statusIds.Add(inputDto.StatusId);
            }
            else
            {
                statusIds = new List<long>() { (long)DTO.Enums.Status.Approved, (long)DTO.Enums.Status.Pending, (long)DTO.Enums.Status.Rejected };
            }
            var stateIds = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.StateIds);
            var status = UtilityHelper.ConvertLongListToCommaSeparatedString(statusIds);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    result = connection.Query<LiftingRequestOutputDto>("SalesOrderExport", new
                    {
                        @RoleId = inputDto.RoleId,
                        @LoginUserId = inputDto.LoginUserId,
                        @FromDate = inputDto.FromDate,
                        @ToDate = inputDto.ToDate,
                        @StateIds = stateIds,
                        @StatusIds = status,
                        @VerticalId = inputDto.VerticalId,
                        @SalesOrganizationId = inputDto.SalesOrganizationId,
                        @DistributionChannelId = inputDto.DistributionChannelId
                    }, commandType: System.Data.CommandType.StoredProcedure).ToList().OrderByDescending(_ => _.LiftingRequestId).ToList();
                    result.ForEach(item =>
                    {
                        string query = "select LRD.Id as Id,SKU.SkuName as SkuName,SKU.SkuCode as SkuCode,(O.Name + '-' + S.Code + '-' + DIST.Code + '-' + DIV.Code) as OilType,LRD.LiftingQuantity,LRD.LiftingQuantityCase,LRD.SaudaNumber from LiftingRequestDetails LRD join OilTypes O on LRD.OilTypeId = O.Id join Skus SKU on SKU.Id = LRD.SkuId join Divisions DIV on DIV.Id = O.DivisionId " +
                        "join DistributionChannels DIST on DIST.Id = O.DistributionChannelId join SalesOrganizations S on S.Id = O.SalesOrganizationId Where LRD.LiftingRequestId = @LiftingId";
                        item.EncryptedId = UtilityHelper.ConvertToMd5(item.LiftingRequestId.ToString(), SecurityConstants.EncryptionKey);
                        var result1 = connection.Query<LiftingRequestDetailsOutputDto>(query, new
                        {
                            LiftingId = item.LiftingRequestId
                        }).ToList();
                        item.RequestedQuantity = result1.Sum(s => s.LiftingQuantity);
                        item.RequestedQuantityInCase = result1.Sum(s => s.LiftingQuantityCase);
                        item.LiftingRequestDetails.AddRange(result1);
                    });

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
            var response = result.ToDataSourceResult(inputDto.DataSourceRequest);

            //var response = await GetKendoGridResultAsync<LiftingRequestOutputDto>(ApiUrl.WebApiUrlGetLiftingRequestWithoutEnquiryNumberList, inputDto);
            return response;
        }

        public async Task<IList<LiftingRequestExportDto>> GetLiftingRequestListForExport(DealersLiftingRequestInputDto inputDto)
        {
            _methodName = "GetLiftingRequestListForExport";
            var response = await GetListAsync<LiftingRequestExportDto>(ApiUrl.WebApiUrlGetLiftingRequestListForExport, inputDto);
            return response;
        }

        public async Task<LiftingRequestWebDto> GetLiftingDetails(IdInputDto inputDto)
        {
            _methodName = "GetAllLiftingRequestList";
            var response = await GetByInputDto<LiftingRequestWebDto>(ApiUrl.WebApiUrlGetLiftingRequestDetails, inputDto);
            return response;
        }

        public async Task<LiftingRequestStatusChangeDto> LiftingRequestStatusChange(LiftingRequestStatusChangeDto inputDto)
        {
            _methodName = "LiftingRequestStatusChange";
            var addOrUpdateMessage = inputDto.Id > 0 ? "Successfully added" : "Successfully Updated";
            return await AddOrUpdate(ApiUrl.WebApiUrlPostLiftingRequestStatusChange, inputDto, addOrUpdateMessage, "Error");
        }

        public async Task<IList<TradeTicketViewDto>> GetAllTradeTicket(TradeTicketParamDto inputDto)
        {
            _methodName = "GetTradeTicket";
            var response = await GetListAsync<TradeTicketViewDto>(ApiUrl.WebApiUrlListTradeTicket, inputDto);
            return response;
        }

        public async Task<TradeTicketViewDto> GetTradeTicket(TradeTicketInputDto inputDto)
        {
            _methodName = "GetTradeTicket";
            var response = await GetByEntityDto<TradeTicketViewDto>(ApiUrl.WebApiUrlGetTradeTicket, inputDto);
            return response;
        }

        public async Task<TradeTicketDeleteDto> DeleteTradeTicket(int tradeTicketId)
        {
            _methodName = "DeleteTradeTicket";
            try
            {
                var result = new TradeTicketDeleteDto();
                var inputDto = new TradeTicketDeleteDto() { TradeTicketId = tradeTicketId };
                var inputDtoJson = JsonHelper.ConvertObjectToJson(inputDto);
                var inputSring = EncryptDecryptHelper.Encrypt(inputDtoJson, SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                HttpResponseMessage response = PostAsync(ApiUrl.WebApiUrlDeleteTradeTicket, inputSring);
                var responseData = await response.Content.ReadAsStringAsync();
                responseData = UtilityHelper.TrimStartEnd(responseData);
                var ja = JArray.Parse(string.Join("", "[" + responseData + "]"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(ja[0]["Y77T3XP2B"].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0]["Y77T3XP2B"].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var jarray = JArray.Parse(string.Join("", "[" + decryptedString + "]"));
                        result = JsonConvert.DeserializeObject<TradeTicketDeleteDto>(jarray[0]["response"].ToString(), UtilityHelper.GetJsonSettings());

                    }
                    if (!string.IsNullOrEmpty(ja[0][Settings.ResponseError].ToString()))
                    {
                        var decryptedString = EncryptDecryptHelper.Decrypt(ja[0][Settings.ResponseError].ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
                        var errorDtoResult = JsonConvert.DeserializeObject<ErrorDto>(decryptedString, UtilityHelper.GetJsonSettings());
                        result.PostStatus = false;
                        result.PostMessage = errorDtoResult.Message;
                    }

                }
                return result;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return new TradeTicketDeleteDto() { PostStatus = false, PostMessage = "Failed to Process the Request", TradeTicketId = tradeTicketId };
            }
        }

        /// <summary>
        /// Update Sauda Status
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>       
        public async Task<SaudaUpdateDto> UpdateSaudaStatus(SaudaUpdateDto inputDto)
        {
            _methodName = "UpdateSaudaStatus";
            var addOrUpdateMessage = Helper.GetResourceString("msg_SaudaStatusUpdatedSuccess");
            var apiUrl = ApiUrl.WebApiUrlUpdateSaudaDetails;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, Helper.GetResourceString("msg_SaudaStatusUpdatedError"));
        }

        public async Task<SaudaUpdateDto> UpdateSaudhaStatusForLoose(SaudaUpdateDto inputDto)
        {
            _methodName = "UpdateSaudaStatus";
            var addOrUpdateMessage = Helper.GetResourceString("msg_SaudaStatusUpdatedSuccess");
            var apiUrl = ApiUrl.WebApiUrlUpdateSaudaForLoose;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, Helper.GetResourceString("msg_SaudaStatusUpdatedError"));
        }

        public async Task<SaudaConversionReprocessDto> ReprocessSaudaConversion(SaudaConversionReprocessDto inputDto)
        {
            _methodName = "ReprocessSaudaConversion";
            var addOrUpdateMessage = Helper.GetResourceString("msg_SuadaReprocess");
            var apiUrl = ApiUrl.WebApiUrlSaudaConversionReprocess;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, Helper.GetResourceString("msg_SaudaReprocessError"));
        }

        public async Task<SaudaConversionReprocessDto> RejectSaudaConversion(SaudaConversionReprocessDto inputDto)
        {
            _methodName = "RejectSaudaConversion";
            var addOrUpdateMessage = Helper.GetResourceString("msg_SuadaReprocess");
            var apiUrl = ApiUrl.WebApiUrlSaudaConversionReject;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, Helper.GetResourceString("msg_SaudaReprocessError"));
        }

        public async Task<SaudaExtensionReprocessDto> ReprocessSaudaExtension(SaudaExtensionReprocessDto inputDto)
        {
            _methodName = "ReprocessSaudaExtension";
            var addOrUpdateMessage = Helper.GetResourceString("msg_SuadaReprocess");
            var apiUrl = ApiUrl.WebApiUrlSaudaExtensionReprocess;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, Helper.GetResourceString("msg_SaudaReprocessError"));
        }

        public async Task<LiftingRequestReprocessDto> ReprocessLiftingRequest(LiftingRequestReprocessDto inputDto)
        {
            _methodName = "ReprocessLiftingRequest";
            var addOrUpdateMessage = Helper.GetResourceString("msg_SuadaReprocess");
            var apiUrl = ApiUrl.WebApiUrlLiftingRequestReprocess;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, Helper.GetResourceString("msg_SaudaReprocessError"));
        }
        public async Task<TradeTicketInputDto> AddOrUpdateTradeTciket(TradeTicketInputDto inputDto)
        {
            _methodName = "AddOrUpdateTradeTciket";
            var addOrUpdateMessage = inputDto.TradeTicketId > 0 ? Helper.GetResourceString("msg_TradeTicketupdatedsuccessfully") : Helper.GetResourceString("msg_TradeTicketsavedsuccessfully");
            var apiUrl = inputDto.TradeTicketId > 0 ? ApiUrl.WebApiUrlUpdateTradeTicket : ApiUrl.WebApiUrlCreateTradeTicket;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, Helper.GetResourceString("msg_TradeTicketError"));
        }

        public async Task<IList<DropDownDto>> TradeTicketDropDownList(LoginUserIdDto inputDto)
        {
            _methodName = "TradeTicketDropDownList";
            var response = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlTradeTicketDropDownList, inputDto);
            return response;
        }

        public async Task<IList<TradeTicketStatusListDto>> TradeTicketStatusList(TradeTicketStatusSearchDto inputDto)
        {
            _methodName = "TradeTicketStatusList";
            var response = await GetListAsync<TradeTicketStatusListDto>(ApiUrl.WebApiUrlTradeTicketStatusList, inputDto);
            return response;
        }

        public async Task<IList<SaudaOrderViewDto>> TradeTicketSaudaDetail(IdInputDto inputDto)
        {
            _methodName = "TradeTicketSaudaDetail";
            var response = await GetListAsync<SaudaOrderViewDto>(ApiUrl.WebApiUrlTradeTicketStatusDetail, inputDto);
            return response;
        }

        public async Task<IList<SaudaOrderViewDto>> SaudaOrderList(LoginUserIdDto inputDto)
        {
            _methodName = "SaudaOrdersList";
            var response = await GetListAsync<SaudaOrderViewDto>(ApiUrl.WebApiUrlSaudaOrderList, inputDto);
            return response;
        }

        public async Task<IList<SaudaOrderViewDto>> GetTradeTicketSaudaOrdersMappingList(TradeTicketSaudaSearchDto inputDto)
        {
            _methodName = "GetTradeTicketSaudaOrdersMappingList";
            var response = await GetListAsync<SaudaOrderViewDto>(ApiUrl.WebApiUrlGetTradeTicketSaudaOrdersMappingList, inputDto);
            //foreach(var item in response)
            //{
            //    //item.BookingDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(item.BookingDate, DateTimeKind.Unspecified),
            //    //    TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            //    //item.BookingDate = DateTime.SpecifyKind(item.BookingDate, DateTimeKind.Local).ToLocalTime();
            //}
            return response;
        }

        public async Task<TradeTicketSaudaMappingDto> GetSaudaOrdersTradeTicketMappingDetails(IdInputDto inputDto)
        {
            _methodName = "GetSaudaOrdersTradeTicketMappingDetails";
            var response = await GetByInputDto<TradeTicketSaudaMappingDto>(ApiUrl.WebApiUrlGetSaudaOrdersTradeTicketMappingDetails, inputDto);
            return response;
        }

        public async Task<TradeTicketMaptoSaudaOrderDto> MapTradeTicketToSaudaOrders(TradeTicketMaptoSaudaOrderDto inputDto)
        {
            _methodName = "MapTradeTicketToSaudaOrders";
            var response = await AddOrUpdate(ApiUrl.WebApiUrlMapTradeTicketToSaudaOrders, inputDto, Helper.GetResourceString("msg_TradeTicketSaudaMappedSuccessfully"), Helper.GetResourceString("msg_TradeTicketSaudaMappedError"));
            return response;
        }

        public async Task<DataSourceResult> GetSaudaListForAdminAsync(SaudaListFilterDto saudaFilterDto)
        {
            _methodName = "GetSaudaListForAdminAsync";
            var response = await GetKendoGridResultAsync<SaudaListDto>(ApiUrl.WebApiUrlGetAllSaudhaListForAdmin, saudaFilterDto);
            return response;
        }
        public async Task<List<SaudaExportDto>> GetSaudaListExport(SaudaListFilterDto saudaFilterDto)
        {
            _methodName = "GetSaudaListExport";
            var result = new List<SaudaExportDto>();
            var SaudaInnerList = new List<SaudaInnerList>();
            try
            {

                
                string reportQuery = "";
                using (IDbConnection conn = new SqlConnection(connectionString))
                {


                    result = conn.Query<SaudaExportDto>("GetSaudaListDataExport", new
                    {
                        @RoleId = saudaFilterDto.RoleId,
                        @LoginUserId = saudaFilterDto.LoginUserId,
                        @FromDate = saudaFilterDto.FromDate,
                        @ToDate = saudaFilterDto.ToDate,
                        @VerticalId = saudaFilterDto.VerticalId,
                        @SalesOrganizationId = saudaFilterDto.SalesOrganizationId,
                        @DistributionChannelId = saudaFilterDto.DistributionChannelId,
                        @OilTypeId=saudaFilterDto.OilTypeId,
                        @SkuId=saudaFilterDto.SkuId,
                        @StatusId=saudaFilterDto.StatusId
                    }, commandType: System.Data.CommandType.StoredProcedure,commandTimeout:300).OrderByDescending(_ => _.SaudaId).ToList();

                    #region oldCode
                    //if (saudaFilterDto.StatusId > 0)
                    //{
                    //    reportQuery = @"select s.CreatedDate as CreatedDate,so.Id as Id,s.Id as SaudaId,s.SaudaNumber as SaudaNumber,
                    //        s.BiddingDate as BiddingDate,s.UserId as UserId,dealer.Name as DealerName,createdby.Name as CreatedBy,
                    //        so.IsSAPDataSync as IsSAPDataSync,re.IsActive as IsActiveRemarks,so.IsSapSauda,so.IsSapSaudaNumberUpdateSync,so.StatusId from Saudas as s 
                    //        join SaudaOrders so on s.Id=so.SaudaId
                    //        left join Remarks re on so.Id=re.TableId 
                    //        join Users dealer on s.UserId=dealer.Id
                    //        join Users createdby on s.CreatedBy=createdby.Id
                    //        where  Cast(s.BiddingDate as Date) >= Cast(@FromDate as Date)  and Cast(s.BiddingDate as Date) <= Cast(@ToDate as Date) 
                    //        and s.StatusId=@StatusId and (s.SalesOrganizationId=@SalesOrganizationId or s.SalesOrganizationId > 0) and (s.DistributionChannelId=@DistributionChannelId or s.DistributionChannelId > 0 ) and (s.DivisionId=@DivisionId or s.DivisionId > 0) and (so.SkuId=@SkuId or so.SkuId > 0) and (so.OilTypeId=@OilTypeId or so.OilTypeId > 0)";
                    //    result = conn.Query<SaudaExportDto>(reportQuery, new
                    //    {
                    //        FromDate = saudaFilterDto.FromDate,
                    //        ToDate = saudaFilterDto.ToDate,
                    //        StatusId = saudaFilterDto.StatusId,
                    //        SalesOrganizationId = saudaFilterDto.SalesOrganizationId,
                    //        DistributionChannelId = saudaFilterDto.DistributionChannelId,
                    //        DivisionId = saudaFilterDto.DivisionId,
                    //        SkuId = saudaFilterDto.SkuId,
                    //        saudaFilterDto.OilTypeId
                    //    }).ToList();



                    //}
                    //else if (saudaFilterDto.FromDate.ToString("dd/MM/yyyy") == saudaFilterDto.ToDate.ToString("dd/MM/yyyy") && saudaFilterDto.FromDate.ToString("dd/MM/yyyy")==DateTime.Now.ToString("dd/MM/yyyy"))
                    //{
                    //    reportQuery = @"select s.CreatedDate as CreatedDate,so.Id as Id,s.Id as SaudaId,s.SaudaNumber as SaudaNumber,
                    //        s.BiddingDate as BiddingDate,s.UserId as UserId,dealer.Name as DealerName,createdby.Name as CreatedBy,
                    //        so.IsSAPDataSync as IsSAPDataSync,re.IsActive as IsActiveRemarks,so.IsSapSauda,so.IsSapSaudaNumberUpdateSync,so.StatusId from Saudas as s 
                    //        join SaudaOrders so on s.Id=so.SaudaId
                    //        left join Remarks re on so.Id=re.TableId 
                    //        join Users dealer on s.UserId=dealer.Id
                    //        join Users createdby on s.CreatedBy=createdby.Id
                    //        where  Cast(s.BiddingDate as Date) = Cast(@FromDate as Date)";

                    //    result = conn.Query<SaudaExportDto>(reportQuery, new
                    //    {
                    //        FromDate = saudaFilterDto.FromDate,

                    //    }).ToList();
                    //}
                    //else
                    //{
                    //    reportQuery = @"select s.CreatedDate as CreatedDate,so.Id as Id,s.Id as SaudaId,s.SaudaNumber as SaudaNumber,
                    //        s.BiddingDate as BiddingDate,s.UserId as UserId,dealer.Name as DealerName,createdby.Name as CreatedBy,
                    //        so.IsSAPDataSync as IsSAPDataSync,re.IsActive as IsActiveRemarks,so.IsSapSauda,so.IsSapSaudaNumberUpdateSync,so.StatusId from Saudas as s 
                    //        join SaudaOrders so on s.Id=so.SaudaId
                    //        left join Remarks re on so.Id=re.TableId 
                    //        join Users dealer on s.UserId=dealer.Id
                    //        join Users createdby on s.CreatedBy=createdby.Id
                    //        where  Cast(s.BiddingDate as Date) >= Cast(@FromDate as Date)  and Cast(s.BiddingDate as Date) <= Cast(@ToDate as Date) 
                    //        and (s.SalesOrganizationId=@SalesOrganizationId or s.SalesOrganizationId > 0) and (s.DistributionChannelId=@DistributionChannelId or s.DistributionChannelId > 0 ) and (s.DivisionId=@DivisionId or s.DivisionId > 0) and (so.SkuId=@SkuId or so.SkuId > 0) and (so.OilTypeId=@OilTypeId or so.OilTypeId > 0)";
                    //    result = conn.Query<SaudaExportDto>(reportQuery, new
                    //    {
                    //        FromDate = saudaFilterDto.FromDate,
                    //        ToDate = saudaFilterDto.ToDate,
                    //        SalesOrganizationId = saudaFilterDto.SalesOrganizationId,
                    //        DistributionChannelId = saudaFilterDto.DistributionChannelId,
                    //        DivisionId = saudaFilterDto.DivisionId,
                    //        SkuId=saudaFilterDto.SkuId,
                    //        saudaFilterDto.OilTypeId
                    //    }).ToList();



                    //}

                    //result = result.GroupBy(_ => _.SaudaId).Select(s => s.First()).OrderByDescending(_ => _.SaudaId).ToList();
                    //foreach(var item in result)
                    //{

                    //    item.InnerList = conn.Query<SaudaInnerList>(innerQuery, new { Id = item.SaudaId }).ToList();
                    //    foreach(var sauda in item.InnerList)
                    //    {
                    //        sauda.Status = sauda.StatusId == (int)DTO.Enums.Status.Pending ? "Accepted" : sauda.Status;
                    //        sauda.DiscountType = sauda.DiscountTypeId != 0 ? Enum.GetName(typeof(SaudaDiscountType), sauda.DiscountTypeId) : "";
                    //        sauda.SaudaBookingType = sauda.SaudaBookingTypeId != 0 ? Enum.GetName(typeof(SaudaBookingTypes), sauda.SaudaBookingTypeId) : "";
                    //        var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    //        TimeSpan difference = currentDate.Subtract(Convert.ToDateTime(sauda.ModifiedDate));

                    //        //if (bdoNewList.IsAny())
                    //        //{
                    //        //    var StateTrader = bdoNewList.FirstOrDefault(x => x.CustomerId == sauda.UserId);
                    //        //    sauda.BDOName = StateTrader != null ? StateTrader.BDOName : string.Empty;
                    //        //    sauda.BDOCode = StateTrader != null ? StateTrader.BDOCode : string.Empty;
                    //        //}


                    //    }
                    //}

                    #endregion

                    var innerQuery = @"select st.Name as Status,so.Id as Id,s.Id as SaudaId,so.SaudaNumber as SaudaNumber,sku.SkuName as SkuName,sku.SkuCode as SkuCode,
                        (ot.Name+'-'+sorg.Code+'/'+dist.Code+'/'+div.Code )as OiltypeName,so.QuotedPrice as QuotedPrice,so.BidQuantity,so.BidQuantityCase,so.BidPrice
                        ,s.BiddingDate,so.DiscountAmount,so.ValidFromDate,so.ValidToDate,i.Name as Incoterms1,
                        s.UserId,so.StatusId,so.SaudaBookingTypeId,so.DiscountTypeId,(d.Name+'-'+d.Code) as PlantName,
                        u.Name as DealerName,StateTrader.Name as CreatedBy,u.Code as DealerCode,state.StateName, 
                        so.IsLooseVerticalForAcceptedStatus,so.IsSAPDataSync,so.IsSAPDataSyncApproval,
                        so.IsSaudaApprovalSyncConfirmation,so.IsSapSauda,so.ModifiedDate,so.IsSapSaudaNumberUpdateSync,
                        so.IsSaudaApprovalStatusFromSap,so.SaudaId as SaudaBookedNumber,so.Incoterms2,r.Description as Remarks
                        ,StateTrader.Name as BDOName,StateTrader.Code as BDOCode,so.PRAmount as PRAmount from Saudas s 
                        join SaudaOrders so on s.Id= so.SaudaId
                        left join (select TableId,Max(Description) as Description from Remarks group by TableId) r on r.TableId=s.Id 
                        left join Status st on st.Id=s.StatusId
                        join Skus sku on so.SkuId=sku.Id and so.SalesOrganizationId=sku.SalesOrganizationId and so.DistributionChannelId=sku.DistributionChannelId and so.DivisionId=sku.DivisionId
                        join OilTypes ot on ot.Id=so.OilTypeId
						join SalesOrganizations sorg on sorg.Id=ot.SalesOrganizationId
						join DistributionChannels dist on ot.DistributionChannelId = dist.Id
						join Divisions div on ot.DivisionId=div.Id
                        join Depots d on so.PlantId=d.Id
                        join Users u on u.Id=s.UserId
                        join States state on u.StateId=state.Id
                        join Users StateTrader on StateTrader.Id=s.CreatedBy
                        join IncoTerms i on i.Id=so.Incoterms2
                                 where(so.SaudaId=@Id)";
                    
                    result.ForEach(item => {
                        item.InnerList = conn.Query<SaudaInnerList>(innerQuery, new { Id = item.SaudaId }).ToList();

                        if (item.InnerList.Any())
                        {
                            item.InnerList.ForEach(sauda =>
                            {
                                sauda.DiscountType = sauda.DiscountTypeId != 0 ? Enum.GetName(typeof(SaudaDiscountType), sauda.DiscountTypeId) : "";
                                sauda.SaudaBookingType = sauda.SaudaBookingTypeId != 0 ? Enum.GetName(typeof(SaudaBookingTypes), sauda.SaudaBookingTypeId) : "";
                                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                TimeSpan difference = currentDate.Subtract(Convert.ToDateTime(sauda.ModifiedDate));
                                if (sauda.BidPrice > 0 && sauda.BidQuantityCase > 0)
                                    sauda.BidPricePerCase = sauda.BidPrice / sauda.BidQuantityCase;
                                if (sauda.PRAmount > 0)
                                    sauda.BidPricePerCase = sauda.PRAmount;
                            });
                            
                        }
                    });


                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }

            //_methodName = "GetSaudaListExport";
            //var response = await GetListAsync<SaudaExportDto>(ApiUrl.WebApiUrlGetAllSaudhaListExport, saudaFilterDto);
            return result;
        }

        public async Task<TickerDto> AddOrUpdateTicker(TickerDto inputDto)
        {
            _methodName = "AddOrUpdateTicker";
            var addOrUpdateMessage = inputDto.Id > 0 ? "Successfully Updated" : "Successfully added";
            return await AddOrUpdate(ApiUrl.WebApiUrlPostTicker, inputDto, addOrUpdateMessage, "Error");
        }
        public async Task<TickerDto> GetTicker(long inputDto)
        {
            _methodName = "EditBidWIndowTiming";
            return await GetById<TickerDto>(ApiUrl.WebApiUrGetTickerById, inputDto);
        }

        public async Task<IList<TickerDto>> GetTickerList(LoginUserIdDto inputDto)
        {
            _methodName = "GetBiddingWindowTimingList";
            var response = await GetListAsync<TickerDto>(ApiUrl.WebApiUrlGetTickerList, inputDto);
            return response;
        }



        public async Task<IList<DropDownDto>> GetDealersListByStateIdAsync(List<int> id)
        {
            _methodName = "GetDealersListByStateIdAsync";
            string apiUrl = ApiUrl.WebApiUrlGetDealersListByStateId;
            return await GetListAsync<DropDownDto>(apiUrl, id);
        }

        #region Bidding Window 

        public async Task<IList<DropDownDto>> GetBiddingWindowTimingListByDateddl(BiddingWindowInputDto inputDto)
        {
            _methodName = "GetBiddingWindowTimingListByDateddl";
            var response = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetBiddingWindowTimingListByDateddl, inputDto);
            return response;
        }

        #endregion

        public async Task<LiftingRequestStatusChangeDto> LiftingRequestStatusChanges(LiftingRequestStatusChangeDto inputDto)
        {
            _methodName = "LiftingRequestStatusChanges";
            var addOrUpdateMessage = "Lifting Request updated successfully";
            return await AddOrUpdate(ApiUrl.WebApiUrlPostLiftingRequestAdminApprove, inputDto, addOrUpdateMessage, "Error");
        }

        public async Task<IList<LiftingRequestDetailsOutputDto>> GetSaudaOrderLiftingRequestDetails(IdInputDto inputDto)
        {
            _methodName = "GetSaudaOrderLiftingRequestDetails";
            var response = await GetListAsync<LiftingRequestDetailsOutputDto>(ApiUrl.WebApiUrlGetSaudaOrderLiftingRequestDetails, inputDto);
            return response;
        }

        public async Task<IList<LiftingRequestOutputDto>> GetSaudaOrderLiftingRequestExcelExport(DealersLiftingRequestInputDto inputDto)
        {
            _methodName = "GetSaudaOrderLiftingRequestExcelExport";

            List<LiftingRequestOutputDto> result = new List<LiftingRequestOutputDto>();
            List<long> statusIds = new List<long>();
            if (inputDto.StatusId > 0)
            {
                statusIds.Add(inputDto.StatusId);
            }
            else
            {
                statusIds = new List<long>() { (long)DTO.Enums.Status.Approved, (long)DTO.Enums.Status.Pending, (long)DTO.Enums.Status.Rejected };
            }
            var stateIds = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.StateIds);
            var status = UtilityHelper.ConvertLongListToCommaSeparatedString(statusIds);
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        result = connection.Query<LiftingRequestOutputDto>("SalesOrderExport", new
                        {
                            @RoleId=inputDto.RoleId,
                            @LoginUserId=inputDto.LoginUserId,
                            @FromDate = inputDto.FromDate,
                            @ToDate = inputDto.ToDate,
                            @StateIds = stateIds,
                            @StatusIds = status,
                            @VerticalId = inputDto.VerticalId,
                            @SalesOrganizationId=inputDto.SalesOrganizationId,
                            @DistributionChannelId=inputDto.DistributionChannelId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList().OrderByDescending(_ => _.LiftingRequestId).ToList();

                        result.ForEach(item =>
                        {
                            string query = "select LRD.Id as Id,SKU.SkuName as SkuName,SKU.SkuCode as SkuCode,(O.Name + '-' + S.Code + '-' + DIST.Code + '-' + DIV.Code) as OilType,LRD.LiftingQuantity,LRD.LiftingQuantityCase,LRD.SaudaNumber from LiftingRequestDetails LRD join OilTypes O on LRD.OilTypeId = O.Id join Skus SKU on SKU.Id = LRD.SkuId join Divisions DIV on DIV.Id = O.DivisionId " +
                            "join DistributionChannels DIST on DIST.Id = O.DistributionChannelId join SalesOrganizations S on S.Id = O.SalesOrganizationId Where LRD.LiftingRequestId = @LiftingId";

                            var result1 = connection.Query<LiftingRequestDetailsOutputDto>(query, new
                            {
                                LiftingId = item.LiftingRequestId
                            }).ToList();
                            item.RequestedQuantity = result1.Sum(s => s.LiftingQuantity);
                            item.RequestedQuantityInCase = result1.Sum(s => s.LiftingQuantityCase);
                            item.LiftingRequestDetails.AddRange(result1);
                        });
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
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;

            //var response = await GetListAsync<LiftingRequestOutputDto>(ApiUrl.WebApiUrlGetSaudaOrderLiftingRequestExcelExport, inputDto);
            //return response;
        }

        public async Task<SaudaDetailOutputDto> UpdateSaudaDetails(SaudaDetailOutputDto inputDto)
        {
            _methodName = "UpdateSaudaDetails";
            var message = "Sauda details updated successfully";
            return await AddOrUpdate(ApiUrl.WebApiUrlPostUpdateSaudaDetails, inputDto, message, "Sauda details update error");
        }

        public async Task<TradeTicketSaudaUnMappingDto> TradeTicketSaudaUnMapping(TradeTicketSaudaUnMappingDto inputDto)
        {
            _methodName = "TradeTicketSaudaUnMapping";
            return await AddOrUpdate(ApiUrl.WebApiUrlPostTicketSaudaUnmapping, inputDto, "", "Error");
        }



        #region Export Trade ticket

        public async Task<IList<TradeTicketExportDto>> ExcelExportTradeTicketStatus(TradeTicketSearchDto inputDto)
        {
            _methodName = "ExcelExportTradeTicketStatus";
            var response = await GetListAsync<TradeTicketExportDto>(ApiUrl.WebApiUrlExcelExportTradeTicketStatus, inputDto);
            return response;
        }

        public async Task<IList<TradeTicketExportAllDto>> ExportAllTradeTickets(TradeTicketSearchDto inputDto)
        {
            _methodName = "ExportAllTradeTickets";
            var response = await GetListAsync<TradeTicketExportAllDto>(ApiUrl.WebApiUrlExcelExportAllTradeTickets, inputDto);
            return response;
        }

        #endregion

        #region SAP Data Sync

        public List<DropDownDto> GetSAPDataSyncListForDropdown(string SyncTypeId)
        {
            _methodName = "GetSAPDataSyncListForDropdown";
            var outputDto = new List<DropDownDto>();
            try
            {
                if (SyncTypeId == "2")
                {
                    //outputDto = ((DTO.Enums.SAPDataSyncSAPToAPP[])Enum.GetValues(typeof(DTO.Enums.SAPDataSyncSAPToAPP)))
                    //.Select(c => new DropDownDto() { Id = (int)c, Name = c.Description().ToString() }).ToList();

                    //without TT
                    outputDto = ((DTO.Enums.SAPDataSyncAPPToSAPWithoutTT[])Enum.GetValues(typeof(DTO.Enums.SAPDataSyncAPPToSAPWithoutTT)))
                   .Select(c => new DropDownDto() { Id = (int)c, Name = c.Description().ToString() }).ToList();
                }
                else if (SyncTypeId == "1")
                {
                    //with TT
                    outputDto = ((DTO.Enums.SAPDataSyncAPPToSAP[])Enum.GetValues(typeof(DTO.Enums.SAPDataSyncAPPToSAP)))
                    .Select(c => new DropDownDto() { Id = (int)c, Name = c.Description().ToString() }).ToList();
                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }

            return outputDto;
        }

        public List<DropDownDto> GetSyncTypeForDropdown()
        {
            _methodName = "GetSyncTypeForDropdown";
            var outputDto = new List<DropDownDto>();
            try
            {
                outputDto.Add(new DropDownDto() { Id = 1, Name = "APP To SAP With Trade Ticket" });
                outputDto.Add(new DropDownDto() { Id = 2, Name = "APP To SAP Without Trade Ticket" });
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }

            return outputDto;
        }

        public SAPDataSyncInputDto SAPSyncData(SAPDataSyncInputDto inputDto)
        {
            _methodName = "SAPSyncData";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var addOrUpdateMessage = Helper.GetResourceString("msg_SyncSuccess");

            if (inputDto.DataSyncInputId == "Sauda HBC APP To SAP With Trade Ticket")
            {
                inputDto.VerticalId = (int)DTO.Enums.Division.Hbc;
                //With TT
                inputDto.TradeTicketWithOrWithoutId = 1;
                inputDto.PostMessage = SaudaSapMove(inputDto);
            }
            else if (inputDto.DataSyncInputId == "Sauda SPF APP To SAP With Trade Ticket")
            {
                inputDto.VerticalId = (int)DTO.Enums.Division.SpecialityFat;
                //With TT
                inputDto.TradeTicketWithOrWithoutId = 1;
                inputDto.PostMessage = SaudaSapMove(inputDto);
            }
            else if (inputDto.DataSyncInputId == "Sauda Loose APP To SAP With Trade Ticket")
            {
                inputDto.VerticalId = (int)DTO.Enums.LooseVertical.Loose;
                //With TT
                inputDto.TradeTicketWithOrWithoutId = 1;
                inputDto.PostMessage = SaudaSapMove(inputDto);
            }
            else if (inputDto.DataSyncInputId == "Sauda HBC APP To SAP Without Trade Ticket")
            {
                inputDto.VerticalId = (int)DTO.Enums.Division.Hbc;
                //Without TT
                inputDto.TradeTicketWithOrWithoutId = 2;
                inputDto.PostMessage = SaudaSapMove(inputDto);
            }
            else if (inputDto.DataSyncInputId == "Sauda SPF APP To SAP Without Trade Ticket")
            {
                inputDto.VerticalId = (int)DTO.Enums.Division.SpecialityFat;
                //Without TT
                inputDto.TradeTicketWithOrWithoutId = 2;
                inputDto.PostMessage = SaudaSapMove(inputDto);
            }
            else if (inputDto.DataSyncInputId == "Sauda Loose APP To SAP Without Trade Ticket")
            {
                inputDto.VerticalId = (int)DTO.Enums.LooseVertical.Loose;
                //Without TT
                inputDto.TradeTicketWithOrWithoutId = 2;
                inputDto.PostMessage = SaudaSapMove(inputDto);
            }
            else
            {
                inputDto.PostMessage = Settings.SAPExcInvoke(inputDto.DataSyncInputId);
            }

            return inputDto;
        }

        public string SaudaSapMove(SAPDataSyncInputDto inputDto)
        {
            _methodName = "SaudaSapMove";
            var result = PostAsync(ApiUrl.WebApiUrlSaudaSyncData, inputDto);
            var message = string.Concat(inputDto.DataSyncInputId, "", " sync successfully done");
            return message;
        }

        #endregion

        #region Bidding Window

        public async Task<BidWindowDto> AddOrUpdateBiddingWindows(BidWindowDto inputDto)
        {
            _methodName = "AddOrUpdateBiddingWindows";
            var addOrUpdateMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_BiddingWindowUpdatedSuccessfully") : Helper.GetResourceString("msg_BiddingWindowSavedSuccessfully");
            var errorMessage = Helper.GetResourceString("msg_BiddingWindowError");
            var apiUrl = inputDto.Id > 0 ? ApiUrl.WebApiUrlPostUpdateBiddingWindow : ApiUrl.WebApiUrlPostSaveBiddingWindow;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, errorMessage);
        }

        public async Task<BidWindowDto> GetBiddingWindowById(IdInputDto inputDto)
        {
            _methodName = "GetBiddingWindowById";
            string apiUrl = ApiUrl.WebApiUrlPostGetBiddingWindowDetailById;
            var result = await GetByInputDto<BidWindowDto>(apiUrl, inputDto);
            return result;
        }

        public async Task<IList<BidWindowListDto>> GetBiddingWindowDetails(BidWindowListSearchDto inputDto)
        {
            var result = await GetListAsync<BidWindowListDto>(ApiUrl.WebApiUrlPostGetBiddingWindowDetails, inputDto);
            return result;
        }

        public async Task<IList<BidWindowVolumeCapacityListDto>> GetBiddingWindowVolumeDetails(IdInputDto inputDto)
        {
            var result = await GetListAsync<BidWindowVolumeCapacityListDto>(ApiUrl.WebApiUrlPostGetBiddingWindowVolumeDetails, inputDto);
            return result;
        }

        public async Task<BidWindowInputDto> StopBidWindow(BidWindowInputDto inputDto)
        {
            _methodName = "StopBidWindow";
            var updateMessage = Helper.GetResourceString("msg_BiddingWindowStopedSuccessfully");
            var errorMessage = Helper.GetResourceString("msg_BiddingWindowError");
            return await AddOrUpdate<BidWindowInputDto>(ApiUrl.WebApiUrlPostStopBidWindow, inputDto, updateMessage, errorMessage);
        }

        /// <summary>
        /// Get all order status
        /// </summary>
        /// <returns></returns>
        public List<DropDownDto> GetAllBidWindowStatus()
        {
            _methodName = "GetAllBidWindowStatus";
            var statusList = new List<DropDownDto>();
            try
            {
                foreach (var unitDetailsItem in Helper.EnumToList<BiddWindowStatus>())
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

        public async Task<List<DropDownDto>> GetBiddingWindowListForddl(LoginUserIdDto inputDto)
        {
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlGetBiddingWindowListForddl, inputDto);
            return result.ToList();
        }


        //public List<BiddingWindowDashboardDto> BiddingWindowDashBoard(int statusId, DateTime searchDate)
        //{
        //    _methodName = "BiddingWindowDashBoard";
        //    _logger.Info($"{ServiceName} Controller-Method {_methodName}");
        //    List<BiddingWindowDashboardDto> result = new List<BiddingWindowDashboardDto>();


        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            result = connection.Query<BiddingWindowDashboardDto>("BiddingWindowDashboard", new
        //            {
        //                StatusId = statusId,
        //                SearchDate = searchDate
        //            }, commandType: System.Data.CommandType.StoredProcedure).ToList();
        //        }
        //        catch (Exception exception)
        //        {
        //            var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
        //            _logger.Error(message);
        //        }
        //    }
        //    return result;
        //}

        public List<BiddingWindowDashboardDto> BiddingWindowDashBoard(int statusId, DateTime searchDate)
        {
            _methodName = "BiddingWindowDashBoard";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<BiddingWindowDashboardDto> result = new List<BiddingWindowDashboardDto>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    result = connection.Query<BiddingWindowDashboardDto>("BiddingWindowDashboardDetailsInGrid", new
                    {
                        StatusId = statusId,
                        SearchDate = searchDate
                    }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                }
            }
            return result;
        }
        public List<BiddingWindowDashboardDto> GetBiddingWindowDashboardOilTypeDetails(long biddingWindowId)
        {
            _methodName = "GetBiddingWindowDashboardOilTypeDetails";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            List<BiddingWindowDashboardDto> result = new List<BiddingWindowDashboardDto>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    result = connection.Query<BiddingWindowDashboardDto>("BiddingWindowWithOilTypes", new
                    {
                        BiddingWindowId = biddingWindowId
                    }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                }
            }
            return result;
        }

        public BiddingWindowDashboardDto BiddingWindowDashBoardDetails(IdInputDto idInputDto)
        {
            _methodName = "BiddingWindowDashBoardDetails";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            BiddingWindowDashboardDto result = new BiddingWindowDashboardDto();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    result = connection.Query<BiddingWindowDashboardDto>("BiddingWindowDashboardDetails", new
                    {
                        BiddingWindowId = idInputDto.Id
                    }, commandType: System.Data.CommandType.StoredProcedure).FirstOrDefault();
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                }
            }
            return result;
        }

        public List<BiddingWindowDashboardDto> BiddingWindowDashBoardChartOverStateWiseAcceptedCountDetails(BiddingWindowDashboardDto biddingWindow)
        {
            var biddingWindowDashboardDetailsOverAllStatusWithOilType = new List<BiddingWindowDashboardDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        biddingWindowDashboardDetailsOverAllStatusWithOilType = connection.Query<BiddingWindowDashboardDto>("BiddingWindowStateWiseAcceptedDetails", new
                        {
                            BiddingWindowId = biddingWindow.BiddingWindowId,
                            AcceptedStatus = (int)Status.Approved,
                            StateId = biddingWindow.StateId

                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
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
            return biddingWindowDashboardDetailsOverAllStatusWithOilType;
        }

        public List<BiddingWindowDashboardChartVolumeCapacityDto> BiddingWindowDashBoardChartVolumeCapacityDetails(long biddingWindowId)
        {
            _methodName = "BiddingWindowDashBoardChartVolumeCapacityDetails";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new List<BiddingWindowDashboardChartVolumeCapacityDto>();


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    result = connection.Query<BiddingWindowDashboardChartVolumeCapacityDto>("BiddingWindowDashboardVolumeCapacitiesChart", new
                    {
                        BiddingWindowId = biddingWindowId
                    }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                }
            }
            return result;
        }

        public List<StateWiseDashboard> GetStatesOfUsersForSpecificBiddingWindow(long biddingWindowId)
        {
            _methodName = "GetOilTypeNamesForSpecificBiddingWindow";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new List<StateWiseDashboard>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(" select Distinct c.StateName as Name,b.StateId as Id from SaudaBiddingCarts as a join Users as b on a.DealerId = b.Id join States as c on b.StateId = c.Id  where a.BiddingWindowId =  @BiddingWindowId and a.StatusId = @AcceptedStatus");
                    result = connection.Query<StateWiseDashboard>(sb.ToString(), new { BiddingWindowId = biddingWindowId, AcceptedStatus = (int)Status.Approved }).ToList();

                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                }
            }
            return result;
        }

        public List<BiddingWindowDashboardChartDto> BiddingWindowDashBoardChartOverAllStatusCountDetails(IdInputDto inputDto)
        {
            _methodName = "BiddingWindowDashBoardChartDetails";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new List<BiddingWindowDashboardChartDto>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(" select OilTypeId as OilTypeId,StatusId as StatusId from SaudaBiddingCarts where BiddingWindowId = @BiddingWindowId ");
                    var StatusofOilTypes = connection.Query<BiddingWindowDashboardDto>(sb.ToString(), new { BiddingWindowId = inputDto.Id }).ToList();
                    if (StatusofOilTypes.IsAny())
                    {
                        result.Add(new BiddingWindowDashboardChartDto { Status = "Total", StatusCount = StatusofOilTypes.Count() });
                        result.Add(new BiddingWindowDashboardChartDto { Status = UtilityHelper.GetEnumDescription(DTO.Enums.Status.Approved), StatusCount = StatusofOilTypes.Count(a => a.StatusId == (int)DTO.Enums.Status.Approved) });
                        result.Add(new BiddingWindowDashboardChartDto { Status = UtilityHelper.GetEnumDescription(DTO.Enums.Status.Rejected), StatusCount = StatusofOilTypes.Count(a => a.StatusId == (int)DTO.Enums.Status.Rejected) });
                        result.Add(new BiddingWindowDashboardChartDto { Status = UtilityHelper.GetEnumDescription(DTO.Enums.Status.Pending), StatusCount = StatusofOilTypes.Count(a => a.StatusId == (int)DTO.Enums.Status.Pending) });
                    }
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                }
            }
            return result;
        }

        public List<BiddingWindowDashboardChartDto> BiddingWindowDashBoardChartOilTypeBasedDetails(BiddingWindowDashboardDto biddingWindowDashboardDto)
        {
            _methodName = "BiddingWindowDashBoardChartDetails";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var result = new List<BiddingWindowDashboardChartDto>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(" select OilTypeId as OilTypeId,StatusId as StatusId  from SaudaBiddingCarts where BiddingWindowId = @BiddingWindowId and OilTypeId = @OilTypeId");
                    var StatusofOilTypes = connection.Query<BiddingWindowDashboardDto>(sb.ToString(), new { BiddingWindowId = biddingWindowDashboardDto.BiddingWindowId, OilTypeId = biddingWindowDashboardDto.OilTypeId }).ToList();

                    if (StatusofOilTypes.IsAny())
                    {
                        result.Add(new BiddingWindowDashboardChartDto { Status = UtilityHelper.GetEnumDescription(DTO.Enums.Status.Pending), StatusCount = StatusofOilTypes.Count(a => a.StatusId == (int)DTO.Enums.Status.Pending) });
                        result.Add(new BiddingWindowDashboardChartDto { Status = UtilityHelper.GetEnumDescription(DTO.Enums.Status.Rejected), StatusCount = StatusofOilTypes.Count(a => a.StatusId == (int)DTO.Enums.Status.Rejected) });
                        result.Add(new BiddingWindowDashboardChartDto { Status = UtilityHelper.GetEnumDescription(DTO.Enums.Status.Approved), StatusCount = StatusofOilTypes.Count(a => a.StatusId == (int)DTO.Enums.Status.Approved) });
                    }
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                }
            }
            return result;
        }

        public List<BiddingWindowVolumeCapacityDto> BiddingWindowDashBoardOverAllStatusCountDetails(long biddingWindow)
        {
            var biddingWindowDashboardDetailsOverAllStatusWithOilType = new List<BiddingWindowVolumeCapacityDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        biddingWindowDashboardDetailsOverAllStatusWithOilType = connection.Query<BiddingWindowVolumeCapacityDto>("BiddingWindowOilWiseStatusCount", new
                        {
                            BiddingWindowId = biddingWindow,
                            AcceptedStatus = (int)Status.Approved,
                            RejectedStatus = (int)Status.Rejected,
                            PendingStatus = (int)Status.Pending
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
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
            return biddingWindowDashboardDetailsOverAllStatusWithOilType;
        }

        public List<BiddingWindowDashboardDto> BiddingWindowDashBoardChartOverAcceptedCountWDetails(BiddingWindowDashboardDto biddingWindow)
        {
            var biddingWindowDashboardDetailsOverAllStatusWithOilType = new List<BiddingWindowDashboardDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        biddingWindowDashboardDetailsOverAllStatusWithOilType = connection.Query<BiddingWindowDashboardDto>("BiddingWindowOilWiseStatusCount", new
                        {
                            BiddingWindowId = biddingWindow.BiddingWindowId,
                            AcceptedStatus = (int)Status.Approved,
                            RejectedStatus = 0,
                            PendingStatus = 0
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
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
            return biddingWindowDashboardDetailsOverAllStatusWithOilType;
        }

        public List<BiddingWindowDashboardReportDto> GetBiddingWindowDashboardDetailsReport(long biddindWindowId)
        {
            var biddingWindowDashboardDetailsReport = new List<BiddingWindowDashboardReportDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        biddingWindowDashboardDetailsReport = connection.Query<BiddingWindowDashboardReportDto>("BiddingWindowDashboardReport", new
                        {
                            BiddingWindowId = biddindWindowId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
                        if (biddingWindowDashboardDetailsReport.IsAny())
                        {
                            foreach (var order in biddingWindowDashboardDetailsReport)
                            {
                                decimal discountGstPercentage = 0;
                                decimal discountWithTax = 0;
                                decimal discountTaxAmount = 0;

                                decimal raTotalDiscount = order.VolumeDiscountCase +
                                order.SchemeDiscountCase +
                                order.SkuDiscountCase +
                                (order.GPBenefitAppliedTypeId == (int)DTO.Enums.BenefitType.NONSAP ? order.GPBenefitDiscountOrDay : 0);
                                decimal bidPricePerCause = (order.QuotedPrice / order.BidQuantityInCase) - raTotalDiscount;
                                switch (order.Incotermid)
                                {
                                    case (int)DTO.Enums.IncoTerms.ExPlant:
                                        discountGstPercentage = Utility.GetGstAmount(1, order.PlantGSTPercentage);
                                        discountWithTax = raTotalDiscount * discountGstPercentage;
                                        discountTaxAmount = discountWithTax - raTotalDiscount;
                                        order.BidPricePerCase = Utility.DecimalFormatTwo(bidPricePerCause);// - discountTaxAmount;
                                        break;
                                    case (int)DTO.Enums.IncoTerms.ForPlant:
                                        discountGstPercentage = Utility.GetGstAmount(1, order.PlantGSTPercentage);
                                        discountWithTax = raTotalDiscount * discountGstPercentage;
                                        discountTaxAmount = discountWithTax - raTotalDiscount;
                                        order.BidPricePerCase = Utility.DecimalFormatTwo(bidPricePerCause);// - discountTaxAmount;
                                        break;
                                    case (int)DTO.Enums.IncoTerms.ExDepot:
                                        discountGstPercentage = Utility.GetGstAmount(1, order.DepotGSTPercentage);
                                        discountWithTax = raTotalDiscount * discountGstPercentage;
                                        discountTaxAmount = discountWithTax - raTotalDiscount;
                                        order.BidPricePerCase = Utility.DecimalFormatTwo(bidPricePerCause);// - discountTaxAmount;
                                        break;
                                    case (int)DTO.Enums.IncoTerms.ForDepot:
                                        discountGstPercentage = Utility.GetGstAmount(1, order.DepotGSTPercentage);
                                        discountWithTax = raTotalDiscount * discountGstPercentage;
                                        discountTaxAmount = discountWithTax - raTotalDiscount;
                                        order.BidPricePerCase = Utility.DecimalFormatTwo(bidPricePerCause);// - discountTaxAmount;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
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
            return biddingWindowDashboardDetailsReport;
        }

        public List<BiddingWindowDashboardReportDto> GetBiddingWindowDashboardDetailsOverAllReport(DateTime searchDate)
        {
            var biddingWindowDashboardDetailsOverAllReport = new List<BiddingWindowDashboardReportDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        biddingWindowDashboardDetailsOverAllReport = connection.Query<BiddingWindowDashboardReportDto>("BiddingWindowDashboardOverAllReport", new
                        {
                            SearchDate = searchDate
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();
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
            return biddingWindowDashboardDetailsOverAllReport;
        }

        public List<BidWindowExportDto> ExportBiddingWindowList(DateTime searchDate, int statusId)
        {

            var result = new List<BidWindowExportDto>();
            try
            {
                var statusIds = new List<int>();
                if (statusId > 0)
                {
                    statusIds.Add(statusId);
                }
                else
                {
                    statusIds.Add((int)DTO.Enums.BiddWindowStatus.Pending);
                    statusIds.Add((int)DTO.Enums.BiddWindowStatus.Processing);
                    statusIds.Add((int)DTO.Enums.BiddWindowStatus.Stopped);
                    statusIds.Add((int)DTO.Enums.BiddWindowStatus.Completed);
                }
                using (IDbConnection conn = new SqlConnection(connectionString))
                {

                    string query = @"Select bw.Id,bw.Name,
                            (SELECT Substring((Select ', ' + cg.Name From CustomerGroups cg
                            Join BiddingWindowCustomerGroups bwc on cg.Id = bwc.CustomerGroupId
                            Where bwc.BiddingWindowId = bw.Id ORDER BY cg.Name FOR XML PATH('')),2,1000000000)) as CustomerGroupNames,
                            bw.StartTime,bw.EndTime,bw.NoOfAttemptsForBidding,bws.Name as WindowStatus,v.Name as Verticals,ot.Name as OilName,bwv.VolumeCapacity
                            From BiddingWindows bw
                            Left Join BiddingWindowVolumeCapacities bwv on bw.Id = bwv.BiddingWindowId
                            Left Join BiddingWindowStatus bws on bws.Id = bw.StatusId
                            Left Join OilTypes ot on ot.Id = bwv.OilTypeId
                            Left Join Verticals v on v.Id = ot.VerticalId
                            Where Convert(date, bw.CreatedDate) = Convert(date, @CreatedDate) And bw.StatusId In @StatusIds
                            Group By bw.Id,bw.Name,bw.StartTime,bw.EndTime,bw.NoOfAttemptsForBidding,bws.Name,v.Name,ot.Name,bwv.VolumeCapacity";

                    result = conn.Query<BidWindowExportDto>(query, new
                    {
                        CreatedDate = searchDate,
                        StatusIds = statusIds
                    }).ToList();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
            return result;
        }

        public int GetWindowTimeInterval()
        {
            _methodName = "GetWindowTimeInterval";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var windowTimeInterval = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Clear();
                    sb.Append(" Select Value From Configurations");
                    sb.Append(" Where Name in (@Name)");
                    windowTimeInterval = connection.QueryFirstOrDefault<int>(sb.ToString(),
                       new
                       {
                           Name = UtilityHelper.GetEnumDescription(DTO.Enums.Configuration.BiddingWindowTimeInterval)
                       });
                }
                catch (Exception exception)
                {
                    var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                }
            }
            return windowTimeInterval;
        }

        #endregion

        #region Conversion Formula

        public async Task<ConversionFormulaDto> AddOrUpdateConversionFormula(ConversionFormulaDto inputDto)
        {
            _methodName = "AddOrUpdateConversionFormula";
            var addOrUpdateMessage = inputDto.Id > 0 ? Helper.GetResourceString("msg_ConversionFormulaUpdateSuccess") : Helper.GetResourceString("msg_ConversionFormulaSaveSuccess");
            var errorMessage = Helper.GetResourceString("msg_ConversionFormulaError");
            var apiUrl = inputDto.Id > 0 ? ApiUrl.WebApiUrlPostUpdateConversionFormula : ApiUrl.WebApiUrlPostSaveConversionFormula;
            return await AddOrUpdate(apiUrl, inputDto, addOrUpdateMessage, errorMessage);
        }

        public async Task<ConversionFormulaDto> GetConversionFormulaById(IdInputDto inputDto)
        {
            _methodName = "GetConversionFormulaById";
            string apiUrl = ApiUrl.WebApiUrlPostGetConversionFormulaById;
            var result = await GetByInputDto<ConversionFormulaDto>(apiUrl, inputDto);
            return result;
        }

        public async Task<IList<ConversionFormulaGridDto>> GetConversionFormulaList(LoginUserIdDto inputDto)
        {
            var result = await GetListAsync<ConversionFormulaGridDto>(ApiUrl.WebApiUrlPostGetConversionDetails, inputDto);
            return result;
        }

        public async Task<IList<ConversionFormulaDetailsGridDto>> GetConversionFormulaDetails(IdInputDto inputDto)
        {
            var result = await GetListAsync<ConversionFormulaDetailsGridDto>(ApiUrl.WebApiUrlPostGetConversionFormulaDetails, inputDto);
            return result;
        }

        public async Task<IList<DropDownDto>> GetBaseSkuList(BaseSkuInputDto inputDto)
        {
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlPostGetBaseSku, inputDto);
            return result;
        }

        public async Task<IList<DropDownDto>> GetDerivedBaseSkuList(BaseSkuInputDto inputDto)
        {
            var result = await GetListAsync<DropDownDto>(ApiUrl.WebApiUrlPostGetDerivedSku, inputDto);
            return result;
        }

        public async Task<List<ConversionFormulaGridDto>> ExportConversionFormulaList(LoginUserIdDto inputDto)
        {
            _methodName = "ExportConversionFormulaList";
            var result = await GetListAsync<ConversionFormulaGridDto>(ApiUrl.WebApiUrlPostExport, inputDto);
            return result.ToList();
        }
        #endregion

        public List<BiddingWindowStatusWiseDetailsDto> BiddingWindowStatusWiseVolumeCount(long biddingWindow)
        {
            var biddingWindowDashboardDetailsOverAllStatusWithOilType = new List<BiddingWindowStatusWiseCountDto>();
            var result = new List<BiddingWindowStatusWiseDetailsDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        biddingWindowDashboardDetailsOverAllStatusWithOilType = connection.Query<BiddingWindowStatusWiseCountDto>("BiddingWindowStatusWiseCount", new
                        {
                            BiddingWindowId = biddingWindow,
                            AcceptedStatus = (int)Status.Approved,
                            RejectedStatus = (int)Status.Rejected,
                            PendingStatus = (int)Status.Pending
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();

                        if (biddingWindowDashboardDetailsOverAllStatusWithOilType.IsAny())
                        {
                            result.Add(new BiddingWindowStatusWiseDetailsDto
                            {
                                StatusName = "Total",
                                TotalBidding = biddingWindowDashboardDetailsOverAllStatusWithOilType.Select(s => s.TotalBidding).DefaultIfEmpty(0).Sum(),
                                TotalVolume = biddingWindowDashboardDetailsOverAllStatusWithOilType.Select(s => s.TotalVolume).DefaultIfEmpty(0).Sum()
                            });
                            result.Add(new BiddingWindowStatusWiseDetailsDto
                            {
                                StatusName = UtilityHelper.GetEnumDescription(DTO.Enums.Status.Approved),
                                TotalBidding = biddingWindowDashboardDetailsOverAllStatusWithOilType.FirstOrDefault(a => a.StatusId == (int)DTO.Enums.Status.Approved)?.ApprovedCount,
                                TotalVolume = biddingWindowDashboardDetailsOverAllStatusWithOilType.FirstOrDefault(a => a.StatusId == (int)DTO.Enums.Status.Approved)?.BidQuantityAccepted
                            });
                            result.Add(new BiddingWindowStatusWiseDetailsDto
                            {
                                StatusName = UtilityHelper.GetEnumDescription(DTO.Enums.Status.Rejected),
                                TotalBidding = biddingWindowDashboardDetailsOverAllStatusWithOilType.FirstOrDefault(a => a.StatusId == (int)DTO.Enums.Status.Rejected)?.RejectedCount,
                                TotalVolume = biddingWindowDashboardDetailsOverAllStatusWithOilType.FirstOrDefault(a => a.StatusId == (int)DTO.Enums.Status.Rejected)?.BidQuantityRejected
                            });
                            result.Add(new BiddingWindowStatusWiseDetailsDto
                            {
                                StatusName = UtilityHelper.GetEnumDescription(DTO.Enums.Status.Pending),
                                TotalBidding = biddingWindowDashboardDetailsOverAllStatusWithOilType.FirstOrDefault(a => a.StatusId == (int)DTO.Enums.Status.Pending)?.PendingCount,
                                TotalVolume = biddingWindowDashboardDetailsOverAllStatusWithOilType.FirstOrDefault(a => a.StatusId == (int)DTO.Enums.Status.Pending)?.BidQuantityPending
                            });
                        }
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

        public List<BiddingWindowStatusWiseDetailsDto> BiddingWindowStatusSateWiseCount(long biddingWindow, long stateId)
        {
            var biddingWindowDashboardDetailsOverAllStatusWithOilType = new List<BiddingWindowStatusWiseCountDto>();
            var result = new List<BiddingWindowStatusWiseDetailsDto>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        biddingWindowDashboardDetailsOverAllStatusWithOilType = connection.Query<BiddingWindowStatusWiseCountDto>("BiddingWindowStatusStateWiseCount", new
                        {
                            BiddingWindowId = biddingWindow,
                            AcceptedStatus = (int)Status.Approved,
                            RejectedStatus = (int)Status.Rejected,
                            PendingStatus = (int)Status.Pending,
                            StateId = stateId
                        }, commandType: System.Data.CommandType.StoredProcedure).ToList();

                        if (biddingWindowDashboardDetailsOverAllStatusWithOilType.IsAny())
                        {
                            result.Add(new BiddingWindowStatusWiseDetailsDto
                            {
                                StatusName = "Total",
                                TotalBidding = biddingWindowDashboardDetailsOverAllStatusWithOilType.FirstOrDefault()?.TotalBidding,
                                TotalVolume = biddingWindowDashboardDetailsOverAllStatusWithOilType.FirstOrDefault()?.TotalVolume,
                            });

                            foreach (var item in biddingWindowDashboardDetailsOverAllStatusWithOilType)
                            {
                                result.Add(new BiddingWindowStatusWiseDetailsDto
                                {
                                    StatusName = item.OilType,
                                    TotalBidding = item.ApprovedCount,
                                    TotalVolume = item.BidQuantityAccepted,
                                });
                            }
                            //result.Add(new BiddingWindowStatusWiseDetailsDto
                            //{
                            //    StatusName = "Total",
                            //    TotalBidding = biddingWindowDashboardDetailsOverAllStatusWithOilType.Select(s => s.TotalBidding).DefaultIfEmpty(0).Sum(),
                            //    TotalVolume = biddingWindowDashboardDetailsOverAllStatusWithOilType.Select(s => s.TotalVolume).DefaultIfEmpty(0).Sum()
                            //});
                            //result.Add(new BiddingWindowStatusWiseDetailsDto
                            //{
                            //    StatusName = UtilityHelper.GetEnumDescription(DTO.Enums.Status.Approved),
                            //    TotalBidding = biddingWindowDashboardDetailsOverAllStatusWithOilType.FirstOrDefault(a => a.StatusId == (int)DTO.Enums.Status.Approved).ApprovedCount,
                            //    TotalVolume = biddingWindowDashboardDetailsOverAllStatusWithOilType.FirstOrDefault(a => a.StatusId == (int)DTO.Enums.Status.Approved).BidQuantityAccepted
                            //});
                            //result.Add(new BiddingWindowStatusWiseDetailsDto
                            //{
                            //    StatusName = UtilityHelper.GetEnumDescription(DTO.Enums.Status.Rejected),
                            //    TotalBidding = biddingWindowDashboardDetailsOverAllStatusWithOilType.FirstOrDefault(a => a.StatusId == (int)DTO.Enums.Status.Rejected).RejectedCount,
                            //    TotalVolume = biddingWindowDashboardDetailsOverAllStatusWithOilType.FirstOrDefault(a => a.StatusId == (int)DTO.Enums.Status.Rejected).BidQuantityRejected
                            //});
                            //result.Add(new BiddingWindowStatusWiseDetailsDto
                            //{
                            //    StatusName = UtilityHelper.GetEnumDescription(DTO.Enums.Status.Pending),
                            //    TotalBidding = biddingWindowDashboardDetailsOverAllStatusWithOilType.FirstOrDefault(a => a.StatusId == (int)DTO.Enums.Status.Pending)?.PendingCount,
                            //    TotalVolume = biddingWindowDashboardDetailsOverAllStatusWithOilType.FirstOrDefault(a => a.StatusId == (int)DTO.Enums.Status.Pending)?.BidQuantityPending
                            //});
                        }
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

        public async Task<SaudaListsDto> GetSaudaDetails(IdInputDto inputDto)
        {
            _methodName = "GetSaudaDetails";
            var response = await GetByInputDto<SaudaListsDto>(ApiUrl.WebApiUrlGetSaudaRequestDetails, inputDto);
            return response;
        }
    }
}