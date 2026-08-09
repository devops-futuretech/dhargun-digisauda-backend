using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Enums;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMCore.Helper;
using Kendo.Mvc.Extensions;
using System.Threading;
using System.Web.Hosting;
using System.Net;
using System.Web.Script.Serialization;
using System.IO;
using Kendo.Mvc.UI;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace Adani.Solution.Service
{
    public interface ISaudaServices
    {
        ResultDto GetSaudaListForAdmin(SaudaListFilterDto saudaFilterDto);
        ResultDto GetSaudaListForAdminApp(SaudaListAdminAppFilterDto saudaFilterDto);
        ResultDto GetAllSaudaList(LoginUserIdDto inputDto);

        ResultDto GetSaudaDetailsForAdmin(SaudaDetailInputDto inputDto);
        ResultDto ChangeSaudaStatus(SaudaUpdateDto inputDto);
        ResultDto UpdateSaudaDetails(SaudaUpdateDto saudaUpdateDto);

        //ResultDto GetSaudhaOrderList(LoginUserIdDto inputDto);
        //ResultDto MapTradeTicketToSaudaOrders(TradeTicketMaptoSaudaOrderDto inputDto);

        //ResultDto GetDealersBySalesPerson(LoginUserIdDto inputDto);
        ResultDto UpdateSaudaLimit(SaudaLimitRequestHistoryDto saudaLimitRequestHistoryDto);
        //ResultDto GetTradeTicketSaudaOrdersMappingList(TradeTicketSaudaSearchDto inputDto);
        //ResultDto GetSaudaOrdersTradeTicketMappingDetails(IdInputDto inputDto);

        ResultDto GetSaudaLimitsRequestHistory(SaudaLimitInputDto inputDto);
        ResultDto ApproveorRejectSaudaLimit(SaudaLimitRequestDto saudaLimitRequestDto);
        ResultDto GetSpecialRateApprovalList(SpecialRateAddInputDto inputDto);
        //Not used
        ResultDto ApproveorRejectSpecialRate(SpecialRateRequestDto specialRateRequestDto);

        //Service Notification
        //ResultDto SendCounterBidNotification(LoginUserIdDto inputDto);
        //ResultDto RejectSaudaOrdersInHold(LoginUserIdDto inputDto);
        //ResultDto SendLatestSaudasStatusNotification(LoginUserIdDto inputDto);

        //ToDO: Now don't want use this function
        //ResultDto GetSkuFinalprice(FinalPriceInputDto inputDto);
        //ResultDto SkuFinalpriceListForAdmin(SkuFinalpriceListInputDto inputDto);
        //ResultDto SkuFinalpriceListForMobile(FinalPriceInputDto inputDto);
        //ResultDto SaveTraditionalFinalPrice(SaveFinalPricngInputDto dto);
        //ResultDto SaveReverseAuactionFinalPrice(SaveFinalPricngInputDto dto);

        //CompetitorAnalysis

        ResultDto GetCompetitorAnalysisList(LoginUserIdDto inputDto);
        ResultDto GetCompetitorAnalysisById(IdInputDto CompetitorAnalysisId);
        ResultDto GetCompetitorAnalysisDetailsListById(long competitorAnalysisId);
        ResultDto UpdateCompetitorAnalysis(CompetitorAnalysisAddDto inputDto);
        ResultDto SaveCompetitorAnalysisApproval(CompetitorAnalysisApprovalDto inputDto);

        //Sauda Convertion
        ResultDto GetSaudaConversionList(SaudaConvertionFilterDto inputDto);
        ResultDto GetSaudaConversionDetails(SaudaConversionDetailInputDto inputDto);
        ResultDto GetSaudaConversionAllDetail(SaudaConversionDetailInputDto inputDto);
        ResultDto ApproveSaudaConversion(SaudaConversionUpdateDto inputDto);
        ResultDto GetSaudaConversionListForExport(SaudaConvertionFilterDto inputDto);
        ResultDto ExportSaudaExtensionList(SaudaConvertionFilterDto inputDto);

        //TP and RA Pricing List
        ResultDto GetTPandRAPricingList(PricingTPandRAInputDto inputDto);

        //SaudaExtension
        ResultDto GetSaudaExtensionList(SaudaConvertionFilterDto inputDto);
        ResultDto GetSaudaExtensionDetails(SaudaConversionDetailInputDto inputDto);
        ResultDto GetSaudaExtensionAllDetail(SaudaConversionDetailInputDto inputDto);
        ResultDto ApproveSaudaExtension(SaudaConversionUpdateDto inputDto);
        ResultDto GetSaudaExtensionDetailsNew(SaudaConversionDetailInputDto inputDto);
        //Cr for sauda extension
        ResultDto GetBookedSaudaWithExtensionDetailsList(SaudaExtensionFilterDto inputDto);
        ResultDto GetSaudaExtensionPendingAndApprovalList(SaudaExtensionFilterDto inputDto);
        ResultDto GetsaudaExtensionDetailsInWeb(SaudaExtensionFilterDtoForGrid saudaFilterDto);
        ResultDto GetSaudaExtensionPendingAndApprovalListForBdo(SaudaExtensionFilterDto inputDto);
        ResultDto GetSaudaExtensionPendingAndApprovalListForDealer(SaudaExtensionFilterDto inputDto);
        #region Special Rate Approval

        ResultDto GetSpecialRateApprovalListWithAccessPermission(SpecialRateAddInputDto inputDto);
        ResultDto SpecialRateApproval(SpecialRateApprovalDto inputDto);

        #endregion

        ResultDto UpdateSaudaDetails(SaudaDetailOutputDto inputDto);
        void SaudaApproveRejectEmailSmsQueueWorkItem(CancellationToken cancellationToken, SaudaUpdateDto inputDto);

        //ResultDto GetSaudaOrdersTradeTicketMappingDetailsLoadTest(TradeTicketDto input);
        ResultDto SaudaApproveLoadTest(SaudaDto inputDto);
        ResultDto LiftingRequestApproveLoadTest(SaudaDto inputDto);
        ResultDto ChangeSaudaStatusForLoose(SaudaUpdateDto inputDto);
        ResultDto SaudaConversionReprocess(SaudaConversionReprocessDto inputDto);
        ResultDto SaudaExtensionReprocess(SaudaExtensionReprocessDto inputDto);
        ResultDto LiftingReprocess(LiftingRequestReprocessDto inputDto);
        ResultDto SaudaConversionReject(SaudaConversionReprocessDto inputDto);

        ResultDto GetSaudaListForAdminMobile(SaudaListFilterDto saudaFilterDto);
        ResultDto UpdateLiftingSaudaOrderId();
        ResultDto GetSaudaDetail(IdInputDto idInputDto);
        ResultDto GetSaudaBookingConfigurationList(UserIdDto inputDto);
        ResultDto GetSaudaSalesAreaRestrictionConfigurationList(UserIdDto inputDto);

        ResultDto GetSaudaModificationList(SaudaListFilterDto saudaFilterDto);

        ResultDto GetSaudaModificationDetailsById(IdInputDto idInputDto);
        ResultDto GetSaudhaModificationDetails(SaudaDetailInputDto inputDto);
        ResultDto GetSaudaModificationReport(SaudaOrderReportInputputDto inputDto);

    }

    public class SaudaServices : ISaudaServices
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Sauda Service");
        private const string ServiceName = "Sauda Service";
        private string _methodName;
        private readonly IResultService _resultService;
        private readonly INotificationService _notificationService;
        private readonly ISAPIntegrationService _sapIntegrationService;

        public SaudaServices(IAdaniContext salesContext, IResultService resultService, INotificationService notificationService, ISAPIntegrationService sapIntegrationService)
        {
            try
            {
                _emamiContext = salesContext;
                _resultService = resultService;
                _notificationService = notificationService;
                _sapIntegrationService = sapIntegrationService;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for sauda Service", exception);
            }
        }

        /// <summary>
        /// Method to get sauda list for admin
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetSaudaListForAdmin(SaudaListFilterDto saudaFilterDto)
        {
            _methodName = "GetSaudaListForAdmin";
            var resultDto = new ResultDto();
            //IEnumerable<SaudaListDto> saudaListDtos = null;

            try
            {
                if (saudaFilterDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                if (saudaFilterDto.LoginUserId <= 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.DealerIdEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.DealerIdEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }

                if (saudaFilterDto.FromDate == null || saudaFilterDto.FromDate == DateTime.MinValue)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.FromDateEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.FromDateEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (saudaFilterDto.ToDate == null || saudaFilterDto.ToDate == DateTime.MinValue)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.ToDateEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.ToDateEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }
                var roleId = _emamiContext.UserRoles.Where(_ => _.UserId == saudaFilterDto.LoginUserId).FirstOrDefault().RoleId;
                //var bdoList = new List<UserRoleIdDto>();
                //var bdoIds = new List<long>();
                //var dealerIds = new List<long>();
                //var ZHIds = new List<long>();



                //if (roleId == (int)DTO.Enums.Role.NationalTrader)
                //{
                //    ZHIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == saudaFilterDto.LoginUserId).Select(s => s.UserId).ToList();
                //    bdoIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => ZHIds.Contains(_.ReportingToUserId)).Select(s => s.UserId).ToList();

                //ZHIds = _emamiContext.Users.Where(_ => _.ReportingToId == saudaFilterDto.LoginUserId).Select(s => s.Id).ToList();
                //bdoIds = _emamiContext.Users.Where(_ => ZHIds.Contains((long)_.ReportingToId)).Select(s => s.Id).ToList();
                //}
                //if (roleId == (int)DTO.Enums.Role.ZonalTrader)
                //{
                //    ZHIds.Add(saudaFilterDto.LoginUserId);
                //    bdoIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == saudaFilterDto.LoginUserId).Select(s => s.UserId).ToList();

                //bdoIds = _emamiContext.Users.Where(_ => _.ReportingToId == saudaFilterDto.LoginUserId).Select(s => s.Id).ToList();
                //}
                //if (roleId == (int)DTO.Enums.Role.StateTrader)
                //{
                //    bdoIds.Add(saudaFilterDto.LoginUserId);
                //bdoList = (from u in _emamiContext.Users.AsNoTracking()
                //               join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                //               where ur.RoleId == (int)DTO.Enums.Role.StateTrader && ur.UserId==saudaFilterDto.LoginUserId
                //               select new UserRoleIdDto()
                //               {
                //                   Username = u.Name,
                //                   UserId = u.Id,
                //                   RoleId = ur.RoleId,
                //                   Code = u.Code
                //               }).ToList();
                //}
                //else
                //{
                //    if (!bdoIds.IsAny())
                //    {
                //        bdoIds = (from u in _emamiContext.Users.AsNoTracking()
                //                  join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                //                  where ur.RoleId == (int)DTO.Enums.Role.StateTrader
                //                  select u.Id).ToList();
                //    }
                //}

                //IEnumerable<SaudaListDto> saudaListContext;


                //var dealerList = (from u in _emamiContext.Users.AsNoTracking()
                //                  join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                //                  join uc in _emamiContext.UserCustomerMapping.AsNoTracking() on u.Id equals uc.CustomerId
                //                  where ur.RoleId == (int)DTO.Enums.Role.Dealer
                //                  select new
                //                  {
                //                      Username = u.Name,
                //                      UserId = u.Id,
                //                      RoleId = ur.RoleId,
                //                      CustometId = uc.UserId
                //                  }).ToList();

                //var bdoNewList = (from d in dealerList
                //                  join StateTrader in bdoList on d.CustometId equals StateTrader.UserId
                //                  select new
                //                  {
                //                      BDOName = StateTrader.Username,
                //                      BdoId = StateTrader.UserId,
                //                      CustomerId = d.UserId,
                //                      BDOCode = StateTrader.Code,
                //                  }).ToList();
                //bdoIds = bdoList.Select(s => s.UserId).ToList();
                //if (ZHIds.IsAny())
                //{
                //    bdoIds.AddRange(ZHIds);
                //}
                //if (bdoIds.IsAny())
                //{
                //    dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(s => s.CustomerId).ToList();
                //}

                IEnumerable<DivisionDetailsDto> divisionslogieduser = new List<DivisionDetailsDto>();
                if (roleId == (int)DTO.Enums.Role.Admin)
                {
                    divisionslogieduser = _emamiContext.Divisions.AsNoTracking().Select(s => new DivisionDetailsDto()
                    {
                        SalesOrganizationId = s.SalesOrganizationId,
                        DistributionChannelId = s.DistributionChannelId,
                        DivisionId = s.Id
                    });
                }
                else
                {
                    divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == saudaFilterDto.LoginUserId)
                 .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });
                }

                var saudaIds = new List<long>();
                var createdBy = new List<long>();
                var loginUserRole = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == saudaFilterDto.LoginUserId);
                if (loginUserRole != null)
                {
                    if (loginUserRole.RoleId == (int)DTO.Enums.Role.NationalTrader || loginUserRole.RoleId == (int)DTO.Enums.Role.ZonalTrader || loginUserRole.RoleId == (int)DTO.Enums.Role.StateTrader)
                    {
                        var ReportingUsers = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == saudaFilterDto.LoginUserId).ToList();
                        if (loginUserRole.RoleId == (int)DTO.Enums.Role.NationalTrader)
                        {
                            //createdBy = _emamiContext.Users.AsNoTracking().Where(user => user.ReportingToId != null ? user.ReportingToId == saudaFilterDto.LoginUserId : saudaFilterDto.LoginUserId > 0).Select(_ => _.Id).ToList();
                            createdBy = _emamiContext.UserReportingToMappings.AsNoTracking().Where(user => user.ReportingToUserId == saudaFilterDto.LoginUserId).Select(_ => _.UserId).ToList();

                        }
                        if (loginUserRole.RoleId == (int)DTO.Enums.Role.ZonalTrader)
                        {

                            createdBy = _emamiContext.UserReportingToMappings.AsNoTracking().Where(user => user.ReportingToUserId == saudaFilterDto.LoginUserId).Select(_ => _.UserId).ToList();
                            //createdBy = _emamiContext.Users.AsNoTracking().Where(user => user.ReportingToId != null ? user.ReportingToId == saudaFilterDto.LoginUserId : saudaFilterDto.LoginUserId > 0).Select(_ => _.Id).ToList();
                        }
                        if (loginUserRole.RoleId == (int)DTO.Enums.Role.StateTrader)
                        {
                            createdBy = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == saudaFilterDto.LoginUserId).Select(s => s.CustomerId).ToList();
                        }
                        var saudabeforeCobinationCheck = _emamiContext.SaudaApproval.AsNoTracking().
                             Where(_ => createdBy.Contains(_.CreatedBy) && _.StatusId == (int)DTO.Enums.Status.Pending).Select(_ => _.SaudaId).ToList();

                        saudaIds = (from s in _emamiContext.Sauda.AsNoTracking()
                                    join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                                    equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                                    where saudabeforeCobinationCheck.Contains(s.Id)
                                    select s.Id).ToList();
                    }

                }

                var description = UtilityHelper.GetEnumDescription(DTO.Enums.Configuration.InboundInterfacenotSyncedToSAPMinutes);
                var configurationContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Key == description).Value;
                // var looseVerticalId = _emamiContext.Verticals.AsNoTracking().FirstOrDefault(vertical => vertical.Id == (int)DTO.Enums.LooseVertical.Loose).Id;
                var remarksContext = _emamiContext.Remarks.AsNoTracking();

                //var saudacontext = (from s in _emamiContext.Sauda.AsNoTracking()
                //                    join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                //                    join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                //                    equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                //                    join dl in dealersList on s.UserId equals dl.CustomerId
                //                    where DbFunctions.TruncateTime(so.CreatedDate) >= DbFunctions.TruncateTime(mStartDate) &&
                //                    DbFunctions.TruncateTime(so.CreatedDate) <= DbFunctions.TruncateTime(mEndDate) && status.Contains(so.StatusId) && s.BdoId == loginUserIdDto.LoginUserId
                //                    select new { CreatedDate = so.CreatedDate, BidQuantity = so.BidQuantity }).ToList();

                IEnumerable<SaudaListDto> saudaqueryContext = new List<SaudaListDto>();
                if (saudaFilterDto.StatusId > 0)
                {
                    var saudaQuery = @"CREATE TABLE #DealerIdsTemp(DealerId BIGINT) 
                    IF(@RoleId = 12) -- NH  
                    BEGIN  
                    INSERT INTO #DealerIdsTemp(DealerId)  
                    Select DISTINCT cus.Id as DealerId  
                    From UserReportingToMappings zh with(NOLOCK)
                    INNER JOIN UserReportingToMappings bdo with(NOLOCK) ON zh.UserId = bdo.ReportingToUserId  
                    INNER JOIN UserCustomerMappings ucm with(NOLOCK) ON ucm.UserId = bdo.UserId  
                    INNER JOIN Users cus with(NOLOCK) ON ucm.CustomerId = cus.Id  
                    Where zh.ReportingToUserId = @LoginUserId 
                    END  
                    ELSE IF(@RoleId = 9) -- ZH  
                    BEGIN  
                    INSERT INTO #DealerIdsTemp(DealerId)  
                    Select DISTINCT 
                    cus.Id as DealerId From UserReportingToMappings bdo  
                    INNER JOIN UserCustomerMappings ucm with(NOLOCK) ON ucm.UserId = bdo.UserId  
                    INNER JOIN Users cus with(NOLOCK) ON ucm.CustomerId = cus.Id  
                    Where bdo.ReportingToUserId = @LoginUserId 
                    END  
                    ELSE IF(@RoleId = 7) --BDO  
                    BEGIN  
                    INSERT INTO #DealerIdsTemp(DealerId)  
                    Select DISTINCT cus.Id as DealerId   
                    From UserCustomerMappings ucm with(NOLOCK) 
                    JOIN Users cus with(NOLOCK) ON ucm.CustomerId = cus.Id  
                    Where ucm.UserId = @LoginUserId 
                    END
                    ELSE -- Admin  
                    BEGIN  
                    INSERT INTO #DealerIdsTemp(DealerId)  
                    select u.Id from Users u
                    join UserRoles ur on u.Id=ur.UserId and ur.RoleId=5
                    join UserCustomerMappings uc on u.Id=uc.CustomerId
                    join Users bdo with(NOLOCK) on uc.UserId=bdo.Id
                    join UserRoles urb with(NOLOCK) on urb.UserId=bdo.Id
                    where urb.RoleId=7  
                    END  

                    Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                    if(@RoleId = 1)
                    begin
	                    insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) Select SalesOrganizationId,DistributionChannelId,Id as DivisionId from Divisions 
                    end
                    else
                    begin
	                    insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@LoginUserId
                    end


                    Select s.SaudaType as SaudaTypeId,s.CreatedDate,so.Id,s.Id as SaudaId,s.SaudaNumber,s.BiddingDate,s.UserId,
                    dealer.Name as DealerName,created.Name as CreatedBy,so.IsSAPDataSync,r.IsActive as IsActiveRemarks,
                    so.IsSapSauda,so.IsSapSaudaNumberUpdateSync,so.StatusId,s.SalesOrganizationId,s.DistributionChannelId,s.DivisionId,so.ModifiedDate,
                    z.Name as Zones,
                    state.StateName As States,
                    dist.DistrictName As Districts,
                    city.CityName As cities
                    from Saudas s with(NOLOCK) 
                    join SaudaOrders so with(NOLOCK) on s.Id = so.SaudaId
                    join #UserDivision ud on s.SalesOrganizationId=ud.SalesOrganizationId and s.DistributionChannelId=ud.DistributionChannelId
                    and s.DivisionId=ud.DivisionId join Users dealer with(NOLOCK) on s.UserId = dealer.Id
                    left join Remarks r with(NOLOCK) on so.Id = r.TableId and r.IsActive = 1
                    join Users created with(NOLOCK) on s.CreatedBy = created.Id 
                    join zones z with(NOLOCK) on z.Id = dealer.ZoneId
                    join States state On state.Id = dealer.StateId
                    join Districts dist On dealer.DistrictId = dist.Id
                    join Cities city On city.Id = dealer.CityId --and city.DistrictId = dist.Id
                    where
                    s.UserId in (select DealerId from #DealerIdsTemp)
                    and Cast(s.BiddingDate as date) >= Cast(@FromDate as Date)
                    and Cast(s.BiddingDate as date) <= Cast(@ToDate as Date) 
                    and((@SalesOrganizationId > 0 and s.SalesOrganizationId = @SalesOrganizationId) or @SalesOrganizationId = 0)
                    and((@DistributionChannelId > 0 and s.DistributionChannelId = @DistributionChannelId) or @DistributionChannelId = 0)
                    and((@DivisionId > 0 and s.DivisionId = @DivisionId) or @DivisionId = 0) 
                    and((@SkuId > 0 and so.SkuId = @SkuId) or @SkuId = 0)
                    and((@OilTypeId > 0 and so.OilTypeId = @OilTypeId) or @OilTypeId = 0)
                    AND ((@ZoneId > 0 AND z.Id = @ZoneId) OR @ZoneId = 0)  
                    AND ((@StateId > 0 AND state.Id = @StateId) OR @StateId = 0)  
                    AND ((@DistrictId > 0 AND dist.Id = @DistrictId) OR @DistrictId = 0)
                    AND ((@CityId > 0 AND city.Id = @CityId) OR @CityId = 0)
                    and so.StatusId=@StatusId
                    drop table #DealerIdsTemp
                    drop table #UserDivision";
                    using (SqlConnection connection = new SqlConnection(Config.DBConnectionString))
                    {
                        try
                        {
                            connection.Open();

                            saudaqueryContext = connection.Query<SaudaListDto>(
                                            saudaQuery,
                                             new
                                             {
                                                 LoginUserId = saudaFilterDto.LoginUserId,
                                                 RoleId = roleId,
                                                 SalesOrganizationId = saudaFilterDto.SalesOrganizationId,
                                                 DistributionChannelId = saudaFilterDto.DistributionChannelId,
                                                 DivisionId = saudaFilterDto.DivisionId,
                                                 FromDate = saudaFilterDto.FromDate,
                                                 ToDate = saudaFilterDto.ToDate,
                                                 SkuId = saudaFilterDto.SkuId,
                                                 OilTypeId = saudaFilterDto.OilTypeId,
                                                 saudaIds = saudaIds,
                                                 StatusId = saudaFilterDto.StatusId,
                                                 ZoneId = saudaFilterDto.ZoneId,
                                                 StateId = saudaFilterDto.StateId,
                                                 DistrictId = saudaFilterDto.DistrictId,
                                                 CityId = saudaFilterDto.CityId
                                                 //dealerIds = dealerIds
                                             }
                                            ).AsEnumerable();

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

                    //saudaListContext = (from s in saudaqueryContext
                    //                    join dm in divisionslogieduser on new
                    //                    {
                    //                        SalesOrganizationId = s.SalesOrganizationId,
                    //                        DistributionChannelId = s.DistributionChannelId,
                    //                        DivisionId = s.DivisionId
                    //                    }
                    //                    equals new
                    //                    {
                    //                        SalesOrganizationId = dm.SalesOrganizationId,
                    //                        DistributionChannelId = dm.DistributionChannelId,
                    //                        DivisionId = dm.DivisionId
                    //                    }
                    //                    where dealerIds.Contains(s.UserId)
                    //                    select s
                    //                       ).ToList();

                    //var saud = saudaqueryContext.ToList();

                    //saudaListContext = (from s in _emamiContext.Sauda.AsNoTracking()
                    //                    join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                    //                    join dealer in _emamiContext.Users.AsNoTracking() on s.UserId equals dealer.Id
                    //                    join createUseer in _emamiContext.Users.AsNoTracking() on s.CreatedBy equals createUseer.Id
                    //                    join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                    //                         equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                    //                         where DbFunctions.TruncateTime(s.BiddingDate) >= DbFunctions.TruncateTime(saudaFilterDto.FromDate)
                    //                         && DbFunctions.TruncateTime(s.BiddingDate) <= DbFunctions.TruncateTime(saudaFilterDto.ToDate)
                    //                         && dealerIds.Contains(s.UserId)
                    //                         && so.StatusId == saudaFilterDto.StatusId
                    //                         && (saudaFilterDto.SalesOrganizationId > 0 ? s.SalesOrganizationId == saudaFilterDto.SalesOrganizationId : s.SalesOrganizationId > 0)
                    //                         && (saudaFilterDto.DistributionChannelId > 0 ? s.DistributionChannelId == saudaFilterDto.DistributionChannelId : s.DistributionChannelId > 0)
                    //                         && (saudaFilterDto.DivisionId > 0 ? s.DivisionId == saudaFilterDto.DivisionId : s.DivisionId > 0)
                    //                         && (saudaFilterDto.SkuId > 0 ? so.SkuId == saudaFilterDto.SkuId : so.SkuId > 0)
                    //                         && (saudaFilterDto.OilTypeId > 0 ? so.OilTypeId == saudaFilterDto.OilTypeId : so.OilTypeId > 0)
                    //                         && (saudaIds.Contains(s.Id) || s.Id > 0)
                    //                         //&& bdoIds.Contains(s.BdoId)
                    //                    select new SaudaListDto()
                    //                    {
                    //                        CreatedDate = s.CreatedDate,
                    //                        Id = so.Id,
                    //                        //EncryptedId = UtilityHelper.ConvertToMd5(s.Id.ToString(), SecurityConstants.EncryptionKey),
                    //                        SaudaId = s.Id,
                    //                        SaudaNumber = s.SaudaNumber,
                    //                        BiddingDate = s.BiddingDate,
                    //                        UserId = s.UserId,
                    //                        DealerName = dealer.Name,
                    //                        CreatedBy = createUseer.Name,
                    //                        IsSAPDataSync = so.IsSAPDataSync,
                    //                        IsActiveRemarks = remarksContext.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.TableId == so.Id && _.IsActive) != null ? remarksContext.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.TableId == so.Id && _.IsActive).IsActive : false,
                    //                        IsSapSauda = so.IsSapSauda,
                    //                        IsSapSaudaNumberUpdateSync = so.IsSapSaudaNumberUpdateSync,
                    //                        StatusId = so.StatusId
                    //                    }
                    //                   ).ToList();



                }
                else if (saudaFilterDto.FromDate.ToString("dd/MM/yyyy") == saudaFilterDto.ToDate.ToString("dd/MM/yyyy") && saudaFilterDto.FromDate.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy"))
                {
                    var saudaQuery = @"CREATE TABLE #DealerIdsTemp(DealerId BIGINT) 
                    IF(@RoleId = 12) -- NH  
                    BEGIN  
                    INSERT INTO #DealerIdsTemp(DealerId)  
                    Select DISTINCT cus.Id as DealerId  
                    From UserReportingToMappings zh with(NOLOCK)
                    INNER JOIN UserReportingToMappings bdo with(NOLOCK) ON zh.UserId = bdo.ReportingToUserId  
                    INNER JOIN UserCustomerMappings ucm with(NOLOCK) ON ucm.UserId = bdo.UserId  
                    INNER JOIN Users cus with(NOLOCK) ON ucm.CustomerId = cus.Id  
                    Where zh.ReportingToUserId = @LoginUserId 
                    END  
                    ELSE IF(@RoleId = 9) -- ZH  
                    BEGIN  
                    INSERT INTO #DealerIdsTemp(DealerId)  
                    Select DISTINCT 
                    cus.Id as DealerId From UserReportingToMappings bdo  
                    INNER JOIN UserCustomerMappings ucm with(NOLOCK) ON ucm.UserId = bdo.UserId  
                    INNER JOIN Users cus with(NOLOCK) ON ucm.CustomerId = cus.Id  
                    Where bdo.ReportingToUserId = @LoginUserId 
                    END  
                    ELSE IF(@RoleId = 7) --BDO  
                    BEGIN  
                    INSERT INTO #DealerIdsTemp(DealerId)  
                    Select DISTINCT cus.Id as DealerId   
                    From UserCustomerMappings ucm with(NOLOCK) 
                    JOIN Users cus with(NOLOCK) ON ucm.CustomerId = cus.Id  
                    Where ucm.UserId = @LoginUserId 
                    END
                    ELSE -- Admin  
                    BEGIN  
                    INSERT INTO #DealerIdsTemp(DealerId)  
                    select u.Id from Users u
                    join UserRoles ur on u.Id=ur.UserId and ur.RoleId=5
                    join UserCustomerMappings uc on u.Id=uc.CustomerId
                    join Users bdo with(NOLOCK) on uc.UserId=bdo.Id
                    join UserRoles urb with(NOLOCK) on urb.UserId=bdo.Id
                    where urb.RoleId=7  
                    END  

                    Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                    if(@RoleId = 1)
                    begin
	                    insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) Select SalesOrganizationId,DistributionChannelId,Id as DivisionId from Divisions 
                    end
                    else
                    begin
	                    insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@LoginUserId
                    end


                    Select s.SaudaType as SaudaTypeId,
                    s.CreatedDate,so.Id,s.Id as SaudaId,s.SaudaNumber,s.BiddingDate,s.UserId,
                    dealer.Name as DealerName,created.Name as CreatedBy,so.IsSAPDataSync,
                    r.IsActive as IsActiveRemarks,so.IsSapSauda,so.IsSapSaudaNumberUpdateSync,so.StatusId,s.SalesOrganizationId,s.DistributionChannelId,s.DivisionId,so.ModifiedDate,
                    z.Name as Zones,
                    state.StateName As States,
                    dist.DistrictName As Districts,
                    city.CityName As cities
                    from Saudas s with(NOLOCK) join SaudaOrders so with(NOLOCK) on s.Id = so.SaudaId
                    join #UserDivision ud on s.SalesOrganizationId=ud.SalesOrganizationId and s.DistributionChannelId=ud.DistributionChannelId
                    and s.DivisionId=ud.DivisionId
                    join Users dealer with(NOLOCK) on s.UserId = dealer.Id 
                    left join Remarks r with(NOLOCK) on so.Id = r.TableId and r.IsActive = 1
                    join Users created with(NOLOCK) on s.CreatedBy = created.Id 
                    join zones z with(NOLOCK) on z.Id = dealer.ZoneId
                    join States state On state.Id = dealer.StateId
                    join Districts dist On dealer.DistrictId = dist.Id
                    join Cities city On city.Id = dealer.CityId --and city.DistrictId = dist.Id
                    where 
                    s.UserId in (select DealerId from #DealerIdsTemp)
                    and Cast(s.BiddingDate as date) = Cast(@FromDate as Date)
                    and((@SalesOrganizationId > 0 and s.SalesOrganizationId = @SalesOrganizationId) or @SalesOrganizationId = 0)
                    and((@DistributionChannelId > 0 and s.DistributionChannelId = @DistributionChannelId) or @DistributionChannelId = 0)
                    and((@DivisionId > 0 and s.DivisionId = @DivisionId) or @DivisionId = 0) 
                    and((@SkuId > 0 and so.SkuId = @SkuId) or @SkuId = 0)
                    and((@OilTypeId > 0 and so.OilTypeId = @OilTypeId) or @OilTypeId = 0)
                    AND ((@ZoneId > 0 AND z.Id = @ZoneId) OR @ZoneId = 0)  
                    AND ((@StateId > 0 AND state.Id = @StateId) OR @StateId = 0)  
                    AND ((@DistrictId > 0 AND dist.Id = @DistrictId) OR @DistrictId = 0)
                    AND ((@CityId > 0 AND city.Id = @CityId) OR @CityId = 0)
                    drop table #DealerIdsTemp
                    drop table #UserDivision";

                    using (SqlConnection connection = new SqlConnection(Config.DBConnectionString))
                    {
                        try
                        {
                            connection.Open();

                            saudaqueryContext = connection.Query<SaudaListDto>(
                                            saudaQuery,
                                             new
                                             {
                                                 LoginUserId = saudaFilterDto.LoginUserId,
                                                 RoleId = roleId,
                                                 SalesOrganizationId = saudaFilterDto.SalesOrganizationId,
                                                 DistributionChannelId = saudaFilterDto.DistributionChannelId,
                                                 DivisionId = saudaFilterDto.DivisionId,
                                                 FromDate = saudaFilterDto.FromDate,
                                                 //ToDate = saudaFilterDto.ToDate,
                                                 SkuId = saudaFilterDto.SkuId,
                                                 OilTypeId = saudaFilterDto.OilTypeId,
                                                 saudaIds = saudaIds,
                                                 ZoneId = saudaFilterDto.ZoneId,
                                                 StateId = saudaFilterDto.StateId,
                                                 DistrictId = saudaFilterDto.DistrictId,
                                                 CityId = saudaFilterDto.CityId
                                                 //StatusId = saudaFilterDto.StatusId
                                                 //dealerIds = dealerIds
                                             }
                                            ).AsEnumerable();

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

                    //saudaListContext = (from s in saudaqueryContext
                    //                    join dm in divisionslogieduser on new
                    //                    {
                    //                        SalesOrganizationId = s.SalesOrganizationId,
                    //                        DistributionChannelId = s.DistributionChannelId,
                    //                        DivisionId = s.DivisionId
                    //                    }
                    //                    equals new
                    //                    {
                    //                        SalesOrganizationId = dm.SalesOrganizationId,
                    //                        DistributionChannelId = dm.DistributionChannelId,
                    //                        DivisionId = dm.DivisionId
                    //                    }
                    //                    where dealerIds.Contains(s.UserId)
                    //                    select s
                    //                       ).ToList();

                    //saudaListContext = (from s in _emamiContext.Sauda.AsNoTracking()
                    //                    join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                    //                    join dealer in _emamiContext.Users.AsNoTracking() on s.UserId equals dealer.Id
                    //                    join createUseer in _emamiContext.Users.AsNoTracking() on s.CreatedBy equals createUseer.Id
                    //                    join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                    //                         equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                    //                    where (
                    //                    DbFunctions.TruncateTime(s.BiddingDate) == DbFunctions.TruncateTime(saudaFilterDto.FromDate))
                    //                    && dealerIds.Contains(s.UserId)
                    //                    && (saudaFilterDto.SalesOrganizationId > 0 ? s.SalesOrganizationId == saudaFilterDto.SalesOrganizationId : s.SalesOrganizationId > 0)
                    //                    && (saudaFilterDto.DistributionChannelId > 0 ? s.DistributionChannelId == saudaFilterDto.DistributionChannelId : s.DistributionChannelId > 0)
                    //                    && (saudaFilterDto.DivisionId > 0 ? s.DivisionId == saudaFilterDto.DivisionId : s.DivisionId > 0)
                    //                    && (saudaFilterDto.SkuId > 0 ? so.SkuId == saudaFilterDto.SkuId : so.SkuId > 0)
                    //                    && (saudaFilterDto.OilTypeId > 0 ? so.OilTypeId == saudaFilterDto.OilTypeId : so.OilTypeId > 0)
                    //                    && (saudaIds.Contains(s.Id) || s.Id > 0)
                    //                    //&& bdoIds.Contains(s.BdoId)
                    //                    select new SaudaListDto()
                    //                    {
                    //                        CreatedDate = s.CreatedDate,
                    //                        Id = so.Id,
                    //                        //EncryptedId = UtilityHelper.ConvertToMd5(s.Id.ToString(), SecurityConstants.EncryptionKey),
                    //                        SaudaId = s.Id,
                    //                        SaudaNumber = s.SaudaNumber,
                    //                        BiddingDate = s.BiddingDate,
                    //                        UserId = s.UserId,
                    //                        DealerName = dealer.Name,
                    //                        CreatedBy = createUseer.Name,
                    //                        IsSAPDataSync = so.IsSAPDataSync,
                    //                        IsActiveRemarks = remarksContext.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.TableId == so.Id && _.IsActive) != null ? remarksContext.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.TableId == so.Id && _.IsActive).IsActive : false,
                    //                        IsSapSauda = so.IsSapSauda,
                    //                        IsSapSaudaNumberUpdateSync = so.IsSapSaudaNumberUpdateSync,
                    //                        StatusId = so.StatusId
                    //                    }
                    //                 ).ToList();


                }
                else
                {
                    var saudaQuery = @"CREATE TABLE #DealerIdsTemp(DealerId BIGINT) 
                            IF(@RoleId = 12) -- NH  
                            BEGIN  
                            INSERT INTO #DealerIdsTemp(DealerId)  
                            Select DISTINCT cus.Id as DealerId  
                            From UserReportingToMappings zh with(NOLOCK)
                            INNER JOIN UserReportingToMappings bdo with(NOLOCK) ON zh.UserId = bdo.ReportingToUserId  
                            INNER JOIN UserCustomerMappings ucm with(NOLOCK) ON ucm.UserId = bdo.UserId  
                            INNER JOIN Users cus with(NOLOCK) ON ucm.CustomerId = cus.Id  
                            Where zh.ReportingToUserId = @LoginUserId 
                            END  
                            ELSE IF(@RoleId = 9) -- ZH  
                            BEGIN  
                            INSERT INTO #DealerIdsTemp(DealerId)  
                            Select DISTINCT 
                            cus.Id as DealerId From UserReportingToMappings bdo  
                            INNER JOIN UserCustomerMappings ucm with(NOLOCK) ON ucm.UserId = bdo.UserId  
                            INNER JOIN Users cus with(NOLOCK) ON ucm.CustomerId = cus.Id  
                            Where bdo.ReportingToUserId = @LoginUserId 
                            END  
                            ELSE IF(@RoleId = 7) --BDO  
                            BEGIN  
                            INSERT INTO #DealerIdsTemp(DealerId)  
                            Select DISTINCT cus.Id as DealerId   
                            From UserCustomerMappings ucm with(NOLOCK) 
                            JOIN Users cus with(NOLOCK) ON ucm.CustomerId = cus.Id  
                            Where ucm.UserId = @LoginUserId 
                            END
                            ELSE -- Admin  
                            BEGIN  
                            INSERT INTO #DealerIdsTemp(DealerId)  
                            select u.Id from Users u
                            join UserRoles ur on u.Id=ur.UserId and ur.RoleId=5
                            join UserCustomerMappings uc on u.Id=uc.CustomerId
                            join Users bdo with(NOLOCK) on uc.UserId=bdo.Id
                            join UserRoles urb with(NOLOCK) on urb.UserId=bdo.Id
                            where urb.RoleId=7     
                            END  
                            
                            Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                            if(@RoleId = 1)
                            begin
                            	insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) Select SalesOrganizationId,DistributionChannelId,Id as DivisionId from Divisions 
                            end
                            else
                            begin
                            	insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@LoginUserId
                            end
                            
                            
                            Select s.SaudaType as SaudaTypeId,
                            s.CreatedDate,
                            so.Id,
                            s.Id as SaudaId,
                            s.SaudaNumber,
                            s.BiddingDate,
                            s.UserId,
                            dealer.Name as DealerName,
                            created.Name as CreatedBy,so.IsSAPDataSync,r.IsActive as IsActiveRemarks,so.IsSapSauda,so.IsSapSaudaNumberUpdateSync,so.StatusId,s.SalesOrganizationId,s.DistributionChannelId,s.DivisionId,so.ModifiedDate,
                            z.Name as Zones,
                            state.StateName As States,
                            dist.DistrictName As Districts,
                            city.CityName As cities
                            from Saudas s with(NOLOCK) join SaudaOrders so with(NOLOCK) on s.Id = so.SaudaId
                            join #UserDivision ud on ud.SalesOrganizationId=s.SalesOrganizationId and ud.DistributionChannelId=s.DistributionChannelId
                            and ud.DivisionId=s.DivisionId
                            join Users dealer with(NOLOCK) on s.UserId = dealer.Id 
                            left join Remarks r with(NOLOCK)  on so.Id = r.TableId and r.IsActive = 1
                            join Users created with(NOLOCK) on s.CreatedBy = created.Id 
                            join zones z with(NOLOCK) on z.Id = dealer.ZoneId
                            join States state On state.Id = dealer.StateId
                            join Districts dist On dealer.DistrictId = dist.Id
                            join Cities city On city.Id = dealer.CityId --and city.DistrictId = dist.Id
                            where 
                            s.UserId in (select DealerId from #DealerIdsTemp)
                            and Cast(s.BiddingDate as date) >= Cast(@FromDate as Date)
                            and Cast(s.BiddingDate as date) <= Cast(@ToDate as Date)  and((@SalesOrganizationId > 0 and s.SalesOrganizationId = @SalesOrganizationId) or @SalesOrganizationId = 0)
                            and((@DistributionChannelId > 0 and s.DistributionChannelId = @DistributionChannelId) or @DistributionChannelId = 0)
                            and((@DivisionId > 0 and s.DivisionId = @DivisionId) or @DivisionId = 0) and((@SkuId > 0 and so.SkuId = @SkuId) or @SkuId = 0)
                            and((@OilTypeId > 0 and so.OilTypeId = @OilTypeId) or @OilTypeId = 0) 
                            AND ((@ZoneId > 0 AND z.Id = @ZoneId) OR @ZoneId = 0)  
                            AND ((@StateId > 0 AND state.Id = @StateId) OR @StateId = 0)  
                            AND ((@DistrictId > 0 AND dist.Id = @DistrictId) OR @DistrictId = 0)
                            AND ((@CityId > 0 AND city.Id = @CityId) OR @CityId = 0)
                            drop table #DealerIdsTemp
                            drop table #UserDivision";
                    using (SqlConnection connection = new SqlConnection(Config.DBConnectionString))
                    {
                        try
                        {
                            connection.Open();

                            saudaqueryContext = connection.Query<SaudaListDto>(
                                            saudaQuery,
                                             new
                                             {
                                                 LoginUserId = saudaFilterDto.LoginUserId,
                                                 RoleId = roleId,
                                                 SalesOrganizationId = saudaFilterDto.SalesOrganizationId,
                                                 DistributionChannelId = saudaFilterDto.DistributionChannelId,
                                                 DivisionId = saudaFilterDto.DivisionId,
                                                 FromDate = saudaFilterDto.FromDate,
                                                 ToDate = saudaFilterDto.ToDate,
                                                 SkuId = saudaFilterDto.SkuId,
                                                 OilTypeId = saudaFilterDto.OilTypeId,
                                                 saudaIds = saudaIds,
                                                 ZoneId = saudaFilterDto.ZoneId,
                                                 StateId = saudaFilterDto.StateId,
                                                 DistrictId = saudaFilterDto.DistrictId,
                                                 CityId = saudaFilterDto.CityId
                                                 //StatusId = saudaFilterDto.StatusId
                                                 //dealerIds = dealerIds
                                             }
                                            ).AsEnumerable();

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

                    //saudaListContext = (from s in saudaqueryContext
                    //                    join dm in divisionslogieduser on new
                    //                    {
                    //                        SalesOrganizationId = s.SalesOrganizationId,
                    //                        DistributionChannelId = s.DistributionChannelId,
                    //                        DivisionId = s.DivisionId
                    //                    }
                    //                    equals new
                    //                    {
                    //                        SalesOrganizationId = dm.SalesOrganizationId,
                    //                        DistributionChannelId = dm.DistributionChannelId,
                    //                        DivisionId = dm.DivisionId
                    //                    }
                    //                    where dealerIds.Contains(s.UserId)
                    //                    select s
                    //                       ).ToList();

                    //saudaListContext = (from s in _emamiContext.Sauda.AsNoTracking()
                    //                    join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                    //                    join dealer in _emamiContext.Users.AsNoTracking() on s.UserId equals dealer.Id
                    //                    join createUseer in _emamiContext.Users.AsNoTracking() on s.CreatedBy equals createUseer.Id
                    //                    join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                    //                         equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                    //                    where (
                    //                    DbFunctions.TruncateTime(s.BiddingDate) >= DbFunctions.TruncateTime(saudaFilterDto.FromDate)
                    //                    && DbFunctions.TruncateTime(s.BiddingDate) <= DbFunctions.TruncateTime(saudaFilterDto.ToDate))
                    //                    && dealerIds.Contains(s.UserId)
                    //                    && (saudaFilterDto.SalesOrganizationId > 0 ? s.SalesOrganizationId == saudaFilterDto.SalesOrganizationId : s.SalesOrganizationId > 0)
                    //                    && (saudaFilterDto.DistributionChannelId > 0 ? s.DistributionChannelId == saudaFilterDto.DistributionChannelId : s.DistributionChannelId > 0)
                    //                    && (saudaFilterDto.DivisionId > 0 ? s.DivisionId == saudaFilterDto.DivisionId : s.DivisionId > 0)
                    //                    && (saudaFilterDto.SkuId > 0 ? so.SkuId == saudaFilterDto.SkuId : so.SkuId > 0)
                    //                    && (saudaFilterDto.OilTypeId > 0 ? so.OilTypeId == saudaFilterDto.OilTypeId : so.OilTypeId > 0)
                    //                    && (saudaIds.Contains(s.Id) || s.Id > 0)
                    //                    //&& bdoIds.Contains(s.BdoId)
                    //                    select new SaudaListDto()
                    //                    {
                    //                        CreatedDate = s.CreatedDate,
                    //                        Id = so.Id,
                    //                        //EncryptedId = UtilityHelper.ConvertToMd5(s.Id.ToString(), SecurityConstants.EncryptionKey),
                    //                        SaudaId = s.Id,
                    //                        SaudaNumber = s.SaudaNumber,
                    //                        BiddingDate = s.BiddingDate,
                    //                        UserId = s.UserId,
                    //                        DealerName = dealer.Name,
                    //                        CreatedBy = createUseer.Name,
                    //                        IsSAPDataSync = so.IsSAPDataSync,
                    //                        IsActiveRemarks = remarksContext.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.TableId == so.Id && _.IsActive) != null ? remarksContext.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.TableId == so.Id && _.IsActive).IsActive : false,
                    //                        IsSapSauda = so.IsSapSauda,
                    //                        IsSapSaudaNumberUpdateSync = so.IsSapSaudaNumberUpdateSync,
                    //                        StatusId = so.StatusId
                    //                    }
                    //                 ).AsEnumerable();

                }

                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                //Data Filter
                List<SaudaListDto> saudaList = new List<SaudaListDto>();
                if (saudaFilterDto.DataFilter == (int)DTO.Enums.Status.Approved && saudaqueryContext.IsAny())
                {
                    saudaList = saudaqueryContext.Where(_ => _.StatusId == (int)DTO.Enums.Status.Pending && string.IsNullOrEmpty(_.SaudaNumber) && !_.IsSAPDataSync && saudaIds.Contains(_.SaudaId)).ToList();
                }
                else if (saudaFilterDto.DataFilter == 0 && saudaqueryContext.IsAny()) //Reprocess after sauda approval
                {
                    saudaList = saudaqueryContext.Where(_ => _.StatusId != (int)DTO.Enums.Status.Rejected && (string.IsNullOrEmpty(_.SaudaNumber) 
                    && _.IsSapSaudaNumberUpdateSync) || (_.IsSAPDataSync && !_.IsSapSaudaNumberUpdateSync 
                    && !_.IsSapSauda && currentDate.Subtract(Convert.ToDateTime(_.ModifiedDate)).TotalMinutes > Convert.ToDouble(configurationContext))).ToList();
                }
                else
                {
                    saudaList = saudaqueryContext.ToList();
                }

                var totalcount = saudaList.Count;

                var saudaList1 = saudaList.OrderByDescending(_ => _.Id).GroupBy(_ => _.SaudaId).Select(s => s.First()).ToList();
                var datasourceresult = saudaList1.ToDataSourceResult(saudaFilterDto.DataSourceRequest);
                List<SaudaListDto> saudaListDtos = new List<SaudaListDto>();
                var saudaListafterPagination = datasourceresult.Data as List<SaudaListDto>;


                if (saudaListafterPagination.IsAny())
                {
                    // --- NEW: populate ApprovalUser for each Sauda in the current page ---

                    var pageSaudaIds = saudaListafterPagination.Select(x => x.SaudaId).Where(id => id > 0).Distinct().ToList();
                    if (pageSaudaIds.Any())
                    {
                        var latestApprovals = _emamiContext.SaudaApproval.AsNoTracking()
                            .Where(a => pageSaudaIds.Contains(a.SaudaId))
                            .GroupBy(a => a.SaudaId)
                            .Select(g => g.OrderByDescending(x => x.Id).FirstOrDefault())
                            .ToList();

                        var approvalBySauda = latestApprovals.Where(a => a != null)
                                                             .ToDictionary(a => a.SaudaId, a => a.RequestedTo);

                        var approverIds = approvalBySauda.Values.Where(v => v > 0).Distinct().ToList();
                        Dictionary<long, string> approverNames = new Dictionary<long, string>();
                        if (approverIds.Any())
                        {
                            approverNames = _emamiContext.Users.AsNoTracking()
                                .Where(u => approverIds.Contains(u.Id))
                                .ToDictionary(u => u.Id, u => u.Name);
                        }

                        // assign ApprovalUser on each DTO in the current page
                        foreach (var s in saudaListafterPagination)
                        {
                            if (approvalBySauda.TryGetValue(s.SaudaId, out var approverId) && approverId > 0)
                            {
                                approverNames.TryGetValue(approverId, out var name);
                                s.ApprovalUser = name ?? string.Empty;
                            }
                            else
                            {
                                s.ApprovalUser = string.Empty;
                            }
                        }
                    }
                    // --- END NEW ---

                    var BdoContextData = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                          join ur in _emamiContext.UserRoles.AsNoTracking() on ucm.UserId equals ur.UserId
                                          join udivm in _emamiContext.UserDivisionMappings.AsNoTracking() on ucm.UserId equals udivm.UserId
                                          where ur.RoleId == (int)DTO.Enums.Role.StateTrader
                                          select new
                                          {
                                              CustomerId = ucm.CustomerId,
                                              BdoId = ucm.UserId,
                                              SalesOrganizationId = udivm.SalesOrganizationId,
                                              DistributionChannelId = udivm.DistributionChannelId,
                                              DivisionId = udivm.DivisionId
                                          }).ToList();
                    var stateTraderList = (from u in _emamiContext.Users.AsNoTracking()
                                           join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                                           where ur.RoleId == (int)DTO.Enums.Role.StateTrader
                                           select new
                                           {
                                               BdoId = u.Id,
                                               BDOName = u.Name
                                           }).ToList();
                    foreach (var sauda in saudaListafterPagination)
                    {
                        sauda.EncryptedId = UtilityHelper.ConvertToMd5(sauda.SaudaId.ToString(), SecurityConstants.EncryptionKey);
                        sauda.DiscountType = sauda.DiscountTypeId != 0 ? Enum.GetName(typeof(SaudaDiscountType), sauda.DiscountTypeId) : "";
                        sauda.SaudaBookingType = sauda.SaudaBookingTypeId != 0 ? Enum.GetName(typeof(SaudaBookingTypes), sauda.SaudaBookingTypeId) : "";
                        sauda.SaudaType = sauda.SaudaTypeId == 0 ? string.Empty : ((DTO.Enums.SaudaType)sauda.SaudaTypeId).ToString();
                        TimeSpan difference = currentDate.Subtract(Convert.ToDateTime(sauda.ModifiedDate));

                        if (sauda.IsSapSauda)
                        {
                            var stateTrader = BdoContextData.FirstOrDefault(s => s.SalesOrganizationId == sauda.SalesOrganizationId &&
                            s.DistributionChannelId == sauda.DistributionChannelId && s.DivisionId == sauda.DivisionId && s.CustomerId == sauda.UserId);

                            var stId = stateTrader != null ? stateTrader.BdoId : 0;
                            var stContext = stateTraderList.FirstOrDefault(statetrader => statetrader.BdoId == stId);
                            sauda.CreatedBy = stContext != null ? stContext.BDOName : string.Empty;
                        }

                        if (sauda.IsSAPDataSync && !sauda.IsSapSauda && difference.TotalMinutes > Convert.ToDouble(configurationContext) && !sauda.IsSapSaudaNumberUpdateSync)
                        {
                            sauda.IsSapSyncNotReceivedForSaudaNumber = true;
                            sauda.Remarks = "Sauda Number Update Sync not Received From Sap";
                        }

                        saudaListDtos.Add(sauda);
                    }

                    datasourceresult.Data = saudaListDtos;
                }

                if (!saudaListDtos.IsAny())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = datasourceresult;
                return resultDto;
                //saudaListDtos != null ? saudaListDtos.OrderByDescending(_ => _.Id).ToList() : saudaListDtos;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }
        public ResultDto GetAllSaudaList(LoginUserIdDto inputDto)
        {
            _methodName = "GetSaudaList";
            var resultDto = new ResultDto();
            var saudaListDto = new List<SaudaListOutputDto>();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                if (inputDto.LoginUserId <= 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.DealerIdEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.DealerIdEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }

                var saudaList = _emamiContext.Sauda.AsNoTracking().OrderByDescending(_ => _.CreatedDate).AsQueryable();

                if (!saudaList.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }
                foreach (var sauda in saudaList.ToList())
                {
                    var liftingQuantity = 0;

                    var totalBidQuantity = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == sauda.Id).Sum(_ => (int?)_.BidQuantity) ?? 0;

                    //var liftingRequestContext = _emamiContext.LiftingRequest.AsNoTracking().FirstOrDefault(_ => _.SaudaId == sauda.Id);
                    //if (liftingRequestContext != null)
                    //{
                    //    liftingQuantity = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.LiftingRequestId == liftingRequestContext.Id &&
                    //    _.Status == (int)DTO.Enums.LiftingRequestStatus.Inprogress).Sum(_ => (int?)_.LiftingQuantity) ?? 0;
                    //}
                    var DealerName = _emamiContext.Users.FirstOrDefault(s => s.Id == sauda.UserId).Code;
                    var saudaDto = new SaudaListOutputDto
                    {
                        SaudaId = sauda.Id,
                        BiddingDate = sauda.BiddingDate,
                        TotalAmt = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == sauda.Id).Sum(_ => (decimal?)_.BidPrice) ?? 0,
                        TotalQty = totalBidQuantity,
                        DeliveryLocation = "",
                        IncoTerms = "",
                        PlantOrDepot = "",
                        //PendingliftQuantity = totalBidQuantity - liftingQuantity
                        DealerName = DealerName,
                        SaudaNo = sauda.Id.ToString()
                    };
                    saudaListDto.Add(saudaDto);
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = saudaListDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to get sauda details for admin
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetSaudaDetailsForAdmin(SaudaDetailInputDto inputDto)
        {
            _methodName = "GetSaudaDetailsForAdmin";
            var resultDto = new ResultDto();
            var saudaDetails = new SaudaListDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                var saudaListContext = _emamiContext.Sauda.AsNoTracking()
                       .Join(_emamiContext.SaudaOrders.AsNoTracking(), s => s.Id, so => so.SaudaId, (s, so) => new { Sauda = s, SaudaOrders = so })
                       .Join(_emamiContext.ApprovalStatus.AsNoTracking(), x => x.SaudaOrders.StatusId, a => a.Id, (x, a) => new { x.SaudaOrders, x.Sauda, ApprovalStatus = a.Name })
                       .Join(_emamiContext.Depots.AsNoTracking(), x => x.SaudaOrders.PlantId, p => p.Id, (x, p) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, Depots = p.Name })
                       .Join(_emamiContext.Users.AsNoTracking(), x => x.Sauda.UserId, u => u.Id, (x, u) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, DealerName = u.Name, DealerCode = u.Code, ZoneId = u.ZoneId, StateId = u.StateId, CityId = u.CityId, DistrictId = u.DistrictId })
                       .Join(_emamiContext.Zones.AsNoTracking(), x => x.ZoneId, z => z.Id, (x, z) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.DealerCode, x.StateId, x.CityId, x.DistrictId, ZoneName = z.Name })
                       .Join(_emamiContext.State.AsNoTracking(), x => x.StateId, s => s.Id, (x, s) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.DealerCode, x.CityId, x.DistrictId, x.ZoneName, StateName = s.StateName })
                       .Join(_emamiContext.District.AsNoTracking(), x => x.DistrictId, d => d.Id, (x, d) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.DealerCode, x.ZoneName, x.StateName, DistrictName = d.DistrictName, x.CityId }) // Moved before City join
                       .Join(_emamiContext.City.AsNoTracking(), x => x.CityId, c => c.Id, (x, c) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.DealerCode, x.ZoneName, x.StateName, x.DistrictName, CityName = c.CityName }) // Moved after District join
                       .Join(_emamiContext.Users.AsNoTracking(), x => x.Sauda.CreatedBy, u => u.Id, (x, u) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, CreatedByName = u.Name, x.DealerCode, x.ZoneName, x.StateName, x.CityName, x.DistrictName })
                       .Join(_emamiContext.IncoTerms.AsNoTracking(), x => x.SaudaOrders.Incoterms2, i => i.Id, (x, i) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.CreatedByName, IncoTermsName = i.Name, x.DealerCode, x.ZoneName, x.StateName, x.CityName, x.DistrictName })
                       .Where(_ => _.Sauda.Id == inputDto.SaudaId)
                       .Select(se => new SaudaListDto()
                       {
                           CreatedDate = se.Sauda.CreatedDate,
                           Id = se.SaudaOrders.Id,
                           SaudaId = se.Sauda.Id,
                           SaudaNumber = se.Sauda.SaudaNumber,
                           SaudaBookedNumber = se.Sauda.Id,
                           BiddingDate = se.Sauda.BiddingDate,
                           UserId = se.Sauda.UserId,
                           DealerName = se.DealerName,
                           CreatedBy = se.CreatedByName,
                           IsSAPDataSync = se.SaudaOrders.IsSAPDataSync,
                           //IsActiveRemarks = remarksContext.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.TableId == se.SaudaOrders.Id && _.IsActive) != null ? remarksContext.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.TableId == se.SaudaOrders.Id && _.IsActive).IsActive : false,
                           IsSapSauda = se.SaudaOrders.IsSapSauda,
                           IsSapSaudaNumberUpdateSync = se.SaudaOrders.IsSapSaudaNumberUpdateSync,
                           StatusId = se.SaudaOrders.StatusId,
                           SalesOrganizationId = se.Sauda.SalesOrganizationId,
                           DistributionChannelId = se.Sauda.DistributionChannelId,
                           DivisionId = se.Sauda.DivisionId,
                           Zones = se.ZoneName,
                           States = se.StateName,
                           Districts = se.DistrictName,
                           Cities = se.CityName
                       }).ToList();

                var BdoContextData = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                      join ur in _emamiContext.UserRoles.AsNoTracking() on ucm.UserId equals ur.UserId
                                      join udivm in _emamiContext.UserDivisionMappings.AsNoTracking() on ucm.UserId equals udivm.UserId
                                      where ur.RoleId == (int)DTO.Enums.Role.StateTrader
                                      select new
                                      {
                                          CustomerId = ucm.CustomerId,
                                          BdoId = ucm.UserId,
                                          SalesOrganizationId = udivm.SalesOrganizationId,
                                          DistributionChannelId = udivm.DistributionChannelId,
                                          DivisionId = udivm.DivisionId
                                      }).ToList();
                var stateTraderList = (from u in _emamiContext.Users.AsNoTracking()
                                       join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                                       where ur.RoleId == (int)DTO.Enums.Role.StateTrader
                                       select new
                                       {
                                           BdoId = u.Id,
                                           BDOName = u.Name
                                       }).ToList();

                if (saudaListContext.FirstOrDefault().IsSapSauda)
                {
                    var stateTrader = BdoContextData.FirstOrDefault(s => s.SalesOrganizationId == saudaListContext.FirstOrDefault().SalesOrganizationId &&
                    s.DistributionChannelId == saudaListContext.FirstOrDefault().DistributionChannelId && s.DivisionId == saudaListContext.FirstOrDefault().DivisionId && s.CustomerId == saudaListContext.FirstOrDefault().UserId);

                    var stId = stateTrader != null ? stateTrader.BdoId : 0;
                    var stContext = stateTraderList.FirstOrDefault(statetrader => statetrader.BdoId == stId);
                    saudaListContext.FirstOrDefault().CreatedBy = stContext != null ? stContext.BDOName : string.Empty;
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = saudaListContext.FirstOrDefault() != null ? saudaListContext.FirstOrDefault() : new SaudaListDto();
                return resultDto;




            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to change the sauda status
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto ChangeSaudaStatus(SaudaUpdateDto inputDto)
        {
            _methodName = "ChangeSaudaStatus";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var saudaDetails = new SaudaDetailOutputDto();
            List<long> SaudaOrdersWithoutSaudaNumber = new List<long>();
            try
            {
                if (inputDto == null || (inputDto.SaudaOrderIds == null || !inputDto.SaudaOrderIds.Any()) && inputDto.LoginUserId < 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var loginUserRole = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                var saudaContext = _emamiContext.Sauda.Where(_ => inputDto.SaudaOrderIds.Contains(_.Id)).ToList();
                var configName = UtilityHelper.GetEnumDescription(DTO.Enums.Configuration.SaudaCreationNationalTraderApproval);
                var configurationsForNT = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(a => a.Name == configName).Value;
                if (inputDto.SaudaOrderIds != null && inputDto.SaudaOrderIds.Any())
                {
                    var sudaorderContext = _emamiContext.SaudaOrders.AsNoTracking();
                    int roleId = 0;
                    if (configurationsForNT == "True")
                    {
                        roleId = (int)DTO.Enums.Role.NationalTrader;
                    }
                    else
                    {
                        if (loginUserRole.RoleId == (int)DTO.Enums.Role.NationalTrader)
                        {
                            roleId = (int)DTO.Enums.Role.NationalTrader;
                        }
                        else
                        {
                            roleId = (int)DTO.Enums.Role.ZonalTrader;
                        }
                    }

                    if (inputDto.StatusId == (int)DTO.Enums.Status.Approved && loginUserRole != null && loginUserRole.RoleId == roleId)
                    {
                        saudaContext.ForEach(a =>
                        {
                            a.StatusId = inputDto.StatusId;
                            a.ModifiedBy = inputDto.ModifiedBy;
                            a.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        });
                        _emamiContext.SaveChanges();
                    }
                    else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                    {
                        saudaContext.ForEach(a =>
                        {
                            a.StatusId = inputDto.StatusId;
                            a.ModifiedBy = inputDto.ModifiedBy;
                            a.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                            if (!string.IsNullOrEmpty(inputDto.Remarks))
                            {
                                var entity = new Remarks()
                                {
                                    TableId = a.Id,
                                    TableName = "Saudas",
                                    ReasonTypeId = inputDto.StatusId,
                                    Description = inputDto.Remarks,
                                    CreatedBy = inputDto.ModifiedBy,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                    IsActive = true
                                };
                                InsertReason(entity);
                            }
                        });
                        _emamiContext.SaveChanges();
                    }
                    else
                    {
                        saudaContext.ForEach(a =>
                        {
                            a.StatusId = (int)DTO.Enums.Status.Pending;
                            a.ModifiedBy = inputDto.ModifiedBy;
                            a.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        });
                        _emamiContext.SaveChanges();
                    }


                    var saudaapprovalContextlist = _emamiContext.SaudaApproval.Where(_ => inputDto.SaudaOrderIds.Contains(_.SaudaId)).ToList();
                    saudaapprovalContextlist.ForEach(a =>
                    {
                        a.StatusId = inputDto.StatusId;
                        // a.Remarks = inputDto.Remarks;
                        a.ModifiedBy = inputDto.ModifiedBy;
                        a.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    });
                    _emamiContext.SaveChanges();

                    if (inputDto.StatusId == (int)DTO.Enums.Status.Approved && loginUserRole != null && loginUserRole.RoleId != roleId)
                    {
                        saudaContext.ForEach(a =>
                        {

                            var requestedToUser = (from uc in _emamiContext.UserReportingToMappings.AsNoTracking()
                                                   join udiv in _emamiContext.UserDivisionMappings.AsNoTracking() on uc.UserId equals udiv.UserId
                                                   where
                                                   udiv.SalesOrganizationId == a.SalesOrganizationId
                                                   && udiv.DistributionChannelId == a.DistributionChannelId
                                                   && udiv.DivisionId == a.DivisionId
                                                   && uc.UserId == inputDto.LoginUserId
                                                   select uc.ReportingToUserId
                                     ).FirstOrDefault();

                            //Sauda approval save
                            var saudaapprovalContext = new SaudaApproval
                            {
                                RequestedBy = inputDto.LoginUserId,
                                RequestedTo = requestedToUser > 0 ? requestedToUser : 0,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                StatusId = (int)DTO.Enums.Status.Pending,
                                ApprovedBy = inputDto.LoginUserId,
                                SaudaId = a.Id,
                                Remarks = inputDto.Remarks
                            };
                            _emamiContext.SaudaApproval.Add(saudaapprovalContext);
                        });
                        _emamiContext.SaveChanges();
                    }

                    foreach (var sauda in inputDto.SaudaOrderIds)
                    {

                        var saudaOrderlist = sudaorderContext.Where(f => f.SaudaId == sauda).Select(_ => _.Id).ToList();
                        //var remarksContext = _emamiContext.Remarks.Where(_ => saudaOrderlist.Contains(_.TableId)).ToList();
                        //if (remarksContext.IsAny())
                        //{
                        //    remarksContext.ForEach(a =>
                        //    {
                        //        a.IsActive = false;
                        //        a.ModifiedBy = inputDto.ModifiedBy;
                        //        a.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        //    });
                        //    _emamiContext.SaveChanges();
                        //}
                        foreach (var saudaorderId in saudaOrderlist)
                        {
                            var saudaOrder = _emamiContext.SaudaOrders.FirstOrDefault(f => f.Id == saudaorderId);
                            if (saudaOrder != null)
                            {
                                if (inputDto.StatusId == (int)DTO.Enums.Status.Approved && loginUserRole != null && loginUserRole.RoleId == roleId)
                                {
                                    saudaOrder.StatusId = inputDto.StatusId;
                                    saudaOrder.SaudaReleaseDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                                }
                                else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                                {
                                    saudaOrder.StatusId = inputDto.StatusId;
                                }
                                else
                                {
                                    saudaOrder.StatusId = (int)DTO.Enums.Status.Pending;
                                }
                                saudaOrder.ModifiedBy = inputDto.ModifiedBy;
                                saudaOrder.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                saudaOrder.Remarks = inputDto.Remarks;

                                if (inputDto.SkuList != null && inputDto.SkuList.Any())
                                {
                                    var matchedSku = inputDto.SkuList.FirstOrDefault(s => s.SkuId == saudaOrder.SkuId);
                                    if (matchedSku != null)
                                    {
                                        saudaOrder.BidQuantityCase = matchedSku.Quantity;
                                    }
                                }
                                _emamiContext.SaveChanges();

                                //var SaudaContext = saudaOrder.Sauda;
                                //if (SaudaContext != null)
                                //{
                                //    long RA2BookingId = SaudaContext.RABookingId;
                                //    if (RA2BookingId > 0)
                                //    {
                                //        var BiddingHeaderContext = _emamiContext.SaudaBiddingCartHeaders.FirstOrDefault(_ => _.Id == RA2BookingId);
                                //        if (BiddingHeaderContext != null)
                                //        {
                                //            var BiddingDetailContext = _emamiContext.SaudaBiddingCart.FirstOrDefault(_ => _.SaudaBiddingCartHeaderId == BiddingHeaderContext.Id && _.SkuId == saudaOrder.SkuId);
                                //            if (BiddingDetailContext != null)
                                //            {
                                //                BiddingDetailContext.StatusId = inputDto.StatusId;
                                //                BiddingDetailContext.ModifiedBy = inputDto.ModifiedBy;
                                //                BiddingDetailContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                //            }
                                //            _emamiContext.SaveChanges();
                                //        }
                                //    }
                                //}

                                //if (!string.IsNullOrEmpty(inputDto.Remarks))
                                //{
                                //    var entity = new Remarks()
                                //    {
                                //        TableId = saudaorderId,
                                //        TableName = "SaudaOrders",
                                //        ReasonTypeId = inputDto.StatusId,
                                //        Description = inputDto.Remarks,
                                //        CreatedBy = inputDto.ModifiedBy,
                                //        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                //        IsActive = true
                                //    };
                                //    InsertReason(entity);
                                //}

                            }
                            else
                            { return _resultService.ErrorMessage(Constants.RecordNotFound); }
                        }

                    }
                    _emamiContext.SaveChanges();

                    if (inputDto.StatusId == (int)DTO.Enums.Status.Approved && loginUserRole != null && loginUserRole.RoleId == roleId)
                    {
                        //method to sync sauda approval from APP to SAP 
                        HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                        {
                            _sapIntegrationService.GetSaudaDetails(inputDto.SaudaOrderIds, true);
                        });
                    }
                    //var usercontextVerticalId = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.ModifiedBy).VerticalId;
                    //if(usercontextVerticalId == (int)DTO.Enums.LooseVertical.Loose)
                    //{
                    //    //method to sync Loose sauda from APP to SAP 
                    //    _sapIntegrationService.GetSaudaDetailsForLooseVertical();
                    //}

                    //Email and SMS Schedule Background Jobs
                    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                    {
                        SaudaApproveRejectEmailSmsQueueWorkItem(cancellationToken, inputDto);
                    });
                }
                else { return _resultService.ErrorMessage(Constants.RecordNotFound); }


                //if (SaudaOrdersWithoutSaudaNumber.IsAny())
                //{
                //    return _resultService.ErrorMessage(Constants.SaudaOrderCantApprove + string.Join(",", SaudaOrdersWithoutSaudaNumber));
                //}

                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto ChangeSaudaStatusForLoose(SaudaUpdateDto inputDto)
        {
            _methodName = "ChangeSaudaStatusForLoose";
            var resultDto = new ResultDto();
            var saudaDetails = new SaudaDetailOutputDto();
            try
            {
                if (inputDto == null || (inputDto.SaudaOrderIds == null) && inputDto.LoginUserId < 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var loginUserRole = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                var saudaContext = _emamiContext.Sauda.Where(_ => inputDto.SaudaOrderIds.Contains(_.Id)).ToList();
                var configName = UtilityHelper.GetEnumDescription(DTO.Enums.Configuration.SaudaCreationNationalTraderApproval);
                var configurationsForNT = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(a => a.Name == configName).Value;
                int roleId = 0;
                if (configurationsForNT == "True")
                {
                    roleId = (int)DTO.Enums.Role.NationalTrader;
                }
                else
                {
                    if (loginUserRole.RoleId == (int)DTO.Enums.Role.NationalTrader)
                    {
                        roleId = (int)DTO.Enums.Role.NationalTrader;
                    }
                    else
                    {
                        roleId = (int)DTO.Enums.Role.ZonalTrader;
                    }
                }


                if (inputDto.StatusId == (int)DTO.Enums.Status.Approved && loginUserRole != null && loginUserRole.RoleId == roleId)
                {
                    saudaContext.ForEach(a =>
                    {
                        a.StatusId = inputDto.StatusId;
                        a.ModifiedBy = inputDto.ModifiedBy;
                        a.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    });
                    _emamiContext.SaveChanges();
                }
                else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                {
                    saudaContext.ForEach(a =>
                    {
                        a.StatusId = inputDto.StatusId;
                        a.ModifiedBy = inputDto.ModifiedBy;
                        a.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                        if (!string.IsNullOrEmpty(inputDto.Remarks))
                        {
                            var entity = new Remarks()
                            {
                                TableId = a.Id,
                                TableName = "Saudas",
                                ReasonTypeId = inputDto.StatusId,
                                Description = inputDto.Remarks,
                                CreatedBy = inputDto.ModifiedBy,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                IsActive = true
                            };
                            InsertReason(entity);
                        }
                    });
                    _emamiContext.SaveChanges();
                }
                else
                {
                    saudaContext.ForEach(a =>
                    {
                        a.StatusId = (int)DTO.Enums.Status.Pending;
                        a.ModifiedBy = inputDto.ModifiedBy;
                        a.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    });
                    _emamiContext.SaveChanges();
                }


                var saudaapprovalContextlist = _emamiContext.SaudaApproval.Where(_ => inputDto.SaudaOrderIds.Contains(_.SaudaId)).ToList();
                saudaapprovalContextlist.ForEach(a =>
                {
                    a.StatusId = inputDto.StatusId;
                    // a.Remarks = inputDto.Remarks;
                    a.ModifiedBy = inputDto.ModifiedBy;
                    a.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                });
                _emamiContext.SaveChanges();

                if (inputDto.StatusId == (int)DTO.Enums.Status.Approved && loginUserRole != null && loginUserRole.RoleId != roleId)
                {
                    saudaContext.ForEach(a =>
                    {
                        var requestedToUser = (from uc in _emamiContext.UserReportingToMappings.AsNoTracking()
                                               join udiv in _emamiContext.UserDivisionMappings.AsNoTracking() on uc.UserId equals udiv.UserId
                                               where
                                               udiv.SalesOrganizationId == a.SalesOrganizationId
                                               && udiv.DistributionChannelId == a.DistributionChannelId
                                               && udiv.DivisionId == a.DivisionId
                                               && uc.UserId == inputDto.LoginUserId
                                               select uc.ReportingToUserId
                                     ).FirstOrDefault();
                        //Sauda approval save
                        var saudaapprovalContext = new SaudaApproval
                        {
                            RequestedBy = inputDto.LoginUserId,
                            RequestedTo = requestedToUser > 0 ? requestedToUser : 0,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            StatusId = (int)DTO.Enums.Status.Pending,
                            SaudaId = a.Id,
                            ApprovedBy = inputDto.LoginUserId,
                            Remarks = inputDto.Remarks
                        };
                        _emamiContext.SaudaApproval.Add(saudaapprovalContext);
                    });
                    _emamiContext.SaveChanges();
                }

                var sudaorderContext = _emamiContext.SaudaOrders;
                foreach (var sauda in inputDto.SaudaOrderIds)
                {
                    var saudaOrderlist = sudaorderContext.Where(f => f.SaudaId == sauda).ToList();
                    var saudaorderids = sudaorderContext.Where(f => f.SaudaId == sauda).Select(_ => _.Id).ToList();

                    foreach (var id in saudaorderids)
                    {
                        var saudaOrder = _emamiContext.SaudaOrders.FirstOrDefault(f => f.Id == id);
                        if (saudaOrder != null)
                        {
                            if (inputDto.StatusId == (int)DTO.Enums.Status.Approved && loginUserRole != null && loginUserRole.RoleId == roleId)
                            {
                                saudaOrder.StatusId = inputDto.StatusId;
                                saudaOrder.SaudaReleaseDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                            }
                            else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                            {
                                saudaOrder.StatusId = inputDto.StatusId;
                            }
                            else
                            {
                                saudaOrder.StatusId = (int)DTO.Enums.Status.Pending;
                            }
                            saudaOrder.ModifiedBy = inputDto.ModifiedBy;
                            saudaOrder.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                            saudaOrder.Remarks = inputDto.Remarks;
                            _emamiContext.SaveChanges();
                        }
                    }

                    if (saudaOrderlist.IsAny())
                    {
                        saudaOrderlist.ForEach(a =>
                        {
                            a.IsSAPDataSync = false;
                            a.IsSapSaudaNumberUpdateSync = false;
                            a.ModifiedBy = inputDto.ModifiedBy;
                            a.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        });
                        _emamiContext.SaveChanges();
                    }
                }

                if (inputDto.StatusId == (int)DTO.Enums.Status.Approved && loginUserRole != null && loginUserRole.RoleId == roleId)
                {
                    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                    {
                        _sapIntegrationService.GetSaudaDetails(inputDto.SaudaOrderIds, false);
                    });
                }
                //var usercontextVerticalId = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.ModifiedBy).VerticalId;
                //if(usercontextVerticalId > 0)
                //{
                //method to sync sauda from APP to SAP - reprocess for all verticals
                //HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                //{
                //    _sapIntegrationService.GetSaudaDetailsForLooseVertical(saudaOrderIds);
                //});
                // }

                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto SaudaConversionReprocess(SaudaConversionReprocessDto inputDto)
        {
            _methodName = "SaudaConversionReprocess";
            var resultDto = new ResultDto();
            var saudaDetails = new SaudaDetailOutputDto();
            try
            {
                if (inputDto == null || (inputDto.SaudaConversionIds == null))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var saudaContext = _emamiContext.SaudaConversionSkus.Where(_ => inputDto.SaudaConversionIds.Contains(_.Id)).ToList();
                if (saudaContext.IsAny())
                {
                    saudaContext.ForEach(a =>
                    {
                        a.IsSAPDataSync = false;
                        a.SaudaConversionUpdateFromSap = false;
                        a.ModifiedBy = inputDto.ModifiedBy;
                        a.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    });
                    _emamiContext.SaveChanges();
                }

                //method to sync sauda Conversion from APP to SAP 
                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.GetSaudaConversionDetails(inputDto.SaudaConversionIds);
                });
                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto SaudaConversionReject(SaudaConversionReprocessDto inputDto)
        {
            _methodName = "SaudaConversionReject";
            var resultDto = new ResultDto();
            var saudaDetails = new SaudaDetailOutputDto();
            try
            {
                if (inputDto == null || (inputDto.SaudaConversionIds == null))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var saudaContext = _emamiContext.SaudaConversionSkus.Where(_ => inputDto.SaudaConversionIds.Contains(_.Id)).ToList();
                if (saudaContext.IsAny())
                {
                    saudaContext.ForEach(a =>
                    {
                        a.StatusId = (int)DTO.Enums.Status.Rejected; // For Sauda Conversions rejected saudas status alone maintained in StatusId Column
                        a.ModifiedBy = inputDto.ModifiedBy;
                        a.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    });
                    _emamiContext.SaveChanges();
                }

                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto SaudaExtensionReprocess(SaudaExtensionReprocessDto inputDto)
        {
            _methodName = "SaudaExtensionReprocess";
            var resultDto = new ResultDto();
            var saudaDetails = new SaudaDetailOutputDto();
            try
            {
                if (inputDto == null || (inputDto.SaudaExtensionIds == null))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var saudaContext = _emamiContext.SaudaExtensionDetailsApprovals.Where(_ => inputDto.SaudaExtensionIds.Contains(_.Id)).ToList();
                if (saudaContext.IsAny())
                {
                    saudaContext.ForEach(a =>
                    {
                        a.IsSAPDataSync = false;
                        a.SaudaExtensionUpdateFromSap = false;
                        a.ModifiedBy = inputDto.ModifiedBy;
                        a.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    });
                    _emamiContext.SaveChanges();
                }

                //method to sync sauda Extension from APP to SAP 
                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.GetSaudaDetails(inputDto.SaudaExtensionIds, false);
                });
                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto LiftingReprocess(LiftingRequestReprocessDto inputDto)
        {
            _methodName = "LiftingReprocess";
            var resultDto = new ResultDto();
            var saudaDetails = new SaudaDetailOutputDto();
            try
            {
                if (inputDto == null || !inputDto.LiftingIds.IsAny())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var liftingRequest = _emamiContext.LiftingRequest.Where(_ => inputDto.LiftingIds.Contains(_.Id)).ToList();
                liftingRequest.ForEach(_ =>
                {
                    _.StatusId = (int)DTO.Enums.Status.Approved;
                    _.ModifiedBy = inputDto.ModifiedBy;
                    _.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    _.IsSAPDataSync = true;
                });

                _emamiContext.SaveChanges();

                var liftingRequestDetails = _emamiContext.LiftingRequestDetails.Where(_ => inputDto.LiftingIds.Contains(_.LiftingRequestId)).ToList();
                liftingRequestDetails.ForEach(_ =>
                {
                    _.EnquiryNumberSyncFromSap = false;
                    _.ModifiedBy = inputDto.ModifiedBy;
                    _.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                });

                _emamiContext.SaveChanges();

                List<long> Ids = new List<long>();
                Ids.AddRange(inputDto.LiftingIds);
                //method to sync Liftng from APP to SAP 
                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                {
                    _sapIntegrationService.GetLiftingRequestEnquiryNumberOutboundDetails(Ids, inputDto.IsReprocess);
                });

                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public void SaudaApproveRejectEmailSmsQueueWorkItem(CancellationToken cancellationToken, SaudaUpdateDto inputDto)
        {
            using (var _context = new AdaniContext())
            {
                foreach (var saudaorderId in inputDto.SaudaOrderIds)
                {
                    //var saudaOrder = _context.SaudaOrders.FirstOrDefault(f => f.Id == saudaorderId);
                    var saudaOrder = _context.SaudaOrders.FirstOrDefault(f => f.SaudaId == saudaorderId);
                    var saudaContext = _context.Sauda.FirstOrDefault(_ => _.Id == saudaOrder.SaudaId);
                    try
                    {
                        bool isEmail = false;

                        var DealerNotificationContext = _context.TPNotification.AsNoTracking().
                                                        Join(_context.TPNotificationDetails.AsNoTracking(), TPN => TPN.Id, TPND => TPND.TPNotificationId, (TPN, TPND) => new { TPN, TPND })
                                                        .Where(_ => _.TPND.DealerId == saudaContext.UserId && _.TPND.NotificationActionId == (int)DTO.Enums.NotificationActionTP.SaudaApproval && _.TPND.IsActive).ToList();

                        var DealerEmailNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.Email);
                        if (DealerEmailNotificationEnabled != null && DealerEmailNotificationEnabled.Any())
                            isEmail = true;
                        else
                            isEmail = false;

                        List<User> usersContext = new List<User>();
                        List<string> toUsers = new List<string>();
                        User createdBy = new User();
                        User dealer = new User();
                        if (saudaOrder.CreatedBy == saudaOrder.Sauda.UserId)
                        {
                            createdBy = _context.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrder.CreatedBy);
                            if (createdBy != null)
                            {
                                toUsers.Add(createdBy.Email);
                            }
                        }
                        else
                        {
                            usersContext = _context.Users.AsNoTracking().Where(_ => _.Id == saudaOrder.CreatedBy || _.Id == saudaOrder.Sauda.UserId).ToList();
                            if (usersContext != null && usersContext.Any())
                            {
                                createdBy = usersContext.FirstOrDefault(_ => _.Id == saudaOrder.CreatedBy);
                                dealer = usersContext.FirstOrDefault(_ => _.Id == saudaOrder.Sauda.UserId);
                                if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
                                {
                                    toUsers.Add(createdBy.Email);
                                }
                                if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                                {
                                    toUsers.Add(dealer.Email);
                                }
                            }
                        }
                        if ((usersContext != null && usersContext.Any()) || createdBy != null)
                        {
                            var emailSubject = string.Empty;
                            AmazonNotificationService amazonNotificationService = new AmazonNotificationService();

                            if (isEmail && toUsers != null && toUsers.Any())
                            {
                                var fromEmail = Constants.FromEmail;
                                var plainText = string.Empty;
                                EmailTemplate emailTemplate = new EmailTemplate();
                                if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                {
                                    //if (saudaOrder.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                    //{
                                    //    emailTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaApprovalEmail);
                                    //}
                                    //else
                                    //{
                                    emailTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaApprovalTPFlowEmail);
                                    //}
                                    emailSubject = Constants.SaudaApprovalSubject;
                                }
                                else if (inputDto.StatusId == (int)DTO.Enums.Status.Hold)
                                {
                                    //if (saudaOrder.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                    //{
                                    //    emailTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderHoldNotificationEmail);
                                    //}
                                    //else
                                    //{
                                    emailTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaHoldTPFlowNotificationEmail);
                                    //}
                                    emailSubject = Constants.SaudaOnHoldSubject;
                                }
                                else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                                {
                                    //if (saudaOrder.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                    //{
                                    //    emailTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderRejectNotificationEmail);
                                    //}
                                    //else
                                    //{
                                    emailTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaRejectTPFlowNotificationEmail);
                                    //}
                                    emailSubject = Constants.SaudaRejectedSubject;
                                }
                                if (emailTemplate != null)
                                {
                                    string plainTemplate = string.Empty;
                                    string htmlTemplate = string.Empty;
                                    if (toUsers.Count > 1)
                                    {
                                        plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrder.Sku.SkuName)
                                        .Replace(Constants.Quantity, (Math.Round(saudaOrder.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(saudaOrder.BidPrice, 2)).ToString())
                                        .Replace(Constants.BY_FOR, Constants.FOR).Replace(Constants.UserName, dealer.Name);
                                    }
                                    else
                                    {
                                        plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrder.Sku.SkuName)
                                        .Replace(Constants.Quantity, (Math.Round(saudaOrder.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(saudaOrder.BidPrice, 2)).ToString())
                                        .Replace(Constants.BY_FOR, Constants.BY).Replace(Constants.UserName, createdBy.Name);
                                    }
                                    htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                    amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                }
                            }
                            var smsPlainTemplate = string.Empty;

                            bool isSms = false;
                            //var IsSMS = _context.Configurations.AsNoTracking().Where(_ => _.Id == (int)DTO.Enums.Configuration.IsSMS).Select(_ => _.Value).Single();
                            //if (IsSMS.Equals("1") || IsSMS.Equals("True"))
                            //    isSms = true;
                            //else
                            //    isSms = false;
                            var DealerSMSNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.SMS);
                            if (DealerSMSNotificationEnabled != null && DealerSMSNotificationEnabled.Any())
                                isSms = true;
                            else
                                isSms = false;

                            bool isPushNotification = false;
                            //var IsPushNotification = _context.Configurations.AsNoTracking().Where(_ => _.Id == (int)DTO.Enums.Configuration.IsPushNotification).Select(_ => _.Value).Single();
                            //if (IsPushNotification.Equals("1") || IsPushNotification.Equals("True"))
                            //    isPushNotification = true;
                            //else
                            //    isPushNotification = false;
                            var DealerPushNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.InAppNotification);
                            if (DealerPushNotificationEnabled != null && DealerPushNotificationEnabled.Any())
                                isPushNotification = true;
                            else
                                isPushNotification = false;

                            if (isSms || isPushNotification)
                            {
                                var smsMessage = string.Empty;
                                EmailTemplate smsTemplate = new EmailTemplate();
                                if (saudaOrder.StatusId == (int)DTO.Enums.Status.Approved)
                                {
                                    //if (saudaOrder.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                    //{
                                    //    smsTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaApprovalSMS);
                                    //}
                                    //else
                                    //{
                                    smsTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaApprovalTPFlowSMS);
                                    // }
                                }
                                else if (saudaOrder.StatusId == (int)DTO.Enums.Status.Hold)
                                {
                                    //if (saudaOrder.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                    //{
                                    //    smsTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderHoldNotificationSMS);
                                    //}
                                    //else
                                    //{
                                    smsTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaHoldTPFlowNotificationSMS);
                                    // }
                                }
                                else if (saudaOrder.StatusId == (int)DTO.Enums.Status.Rejected)
                                {
                                    //if (saudaOrder.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                    //{
                                    //    smsTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderRejectNotificationSMS);
                                    //}
                                    //else
                                    //{
                                    smsTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaRejectTPFlowNotificationSMS);
                                    //}
                                }
                                if (smsTemplate != null)
                                {
                                    if (toUsers.Count > 1)
                                    {
                                        smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrder.Sku.SkuName)
                                        .Replace(Constants.Quantity, (Math.Round(saudaOrder.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(saudaOrder.BidPrice, 2)).ToString())
                                        .Replace(Constants.BY_FOR, Constants.FOR).Replace(Constants.UserName, dealer.Name);
                                    }
                                    else
                                    {
                                        smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrder.Sku.SkuName)
                                        .Replace(Constants.Quantity, (Math.Round(saudaOrder.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(saudaOrder.BidPrice, 2)).ToString())
                                        .Replace(Constants.BY_FOR, Constants.BY).Replace(Constants.UserName, createdBy.Name);
                                    }
                                    if (isSms)
                                    {
                                        smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                        try
                                        {
                                            var smsTemplateID = smsTemplate.SMSTemplateID == null ? string.Empty : smsTemplate.SMSTemplateID;
                                            if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                                            {
                                                amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber, smsTemplateID);
                                            }
                                            if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                            {
                                                amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber, smsTemplateID);
                                            }
                                        }
                                        catch (Exception e) { }
                                    }
                                }
                            }



                            if (isPushNotification)
                            {
                                if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
                                {
                                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                    {
                                        PushTokenKey = createdBy.PushTokenKey,
                                        RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
                                        Title = emailSubject,
                                        Message = smsPlainTemplate,
                                        //Id = saudaOrder.Id,
                                    };
                                    //_notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                }
                                if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
                                {
                                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                    {
                                        PushTokenKey = dealer.PushTokenKey,
                                        RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
                                        Title = emailSubject,
                                        Message = smsPlainTemplate,
                                        //Id = saudaOrder.Id,
                                    };
                                    //_notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                }

                                #region Push Notification Nested Method
                                void SendPushNotificationThroughFirebase(PushNotificationInputDto pushNotificationInputDto)
                                {
                                    try
                                    {
                                        var firebaseSenderId = _context.Configurations.FirstOrDefault(_ => _.Key == Constants.FirebaseSenderId).Value;
                                        var pushNotifyServerkey = _context.Configurations.FirstOrDefault(_ => _.Key == Constants.PushNotifyServerkey).Value;
                                        var pushNotifyUrl = _context.Configurations.FirstOrDefault(_ => _.Key == Constants.PushNotifyUrl).Value;

                                        WebRequest tRequest = WebRequest.Create(pushNotifyUrl);
                                        tRequest.Method = "post";
                                        tRequest.ContentType = "application/json";
                                        var json = new JavaScriptSerializer().Serialize(string.Empty);
                                        if (pushNotificationInputDto.RegistrationTypeId == (int)DTO.Enums.RegistrationType.Android)
                                        {
                                            var data = new
                                            {
                                                to = pushNotificationInputDto.PushTokenKey,
                                                data = new
                                                {
                                                    sound = "default",
                                                    message = pushNotificationInputDto.Message,
                                                    title = pushNotificationInputDto.Title,
                                                    id = pushNotificationInputDto.Id,
                                                },
                                                priority = "high"
                                            };
                                            json = new JavaScriptSerializer().Serialize(data);
                                        }
                                        else if (pushNotificationInputDto.RegistrationTypeId == (int)DTO.Enums.RegistrationType.IOS)
                                        {
                                            var data = new
                                            {
                                                to = pushNotificationInputDto.PushTokenKey,
                                                data = new
                                                {
                                                    sound = "default",
                                                    message = pushNotificationInputDto.Message,
                                                    title = pushNotificationInputDto.Title,
                                                    id = pushNotificationInputDto.Id,
                                                },
                                                notification = new
                                                {
                                                    title = pushNotificationInputDto.Title,
                                                    body = pushNotificationInputDto.Message,
                                                    id = pushNotificationInputDto.Id,
                                                    sound = "default",
                                                },
                                                priority = "high"
                                            };
                                            json = new JavaScriptSerializer().Serialize(data);
                                        }

                                        Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                                        tRequest.Headers.Add(string.Format("Authorization: key={0}", pushNotifyServerkey));
                                        tRequest.Headers.Add(string.Format("Sender: id={0}", firebaseSenderId));
                                        tRequest.ContentLength = byteArray.Length;
                                        using (Stream dataStream = tRequest.GetRequestStream())
                                        {
                                            dataStream.Write(byteArray, 0, byteArray.Length);
                                            using (WebResponse tResponse = tRequest.GetResponse())
                                            {
                                                using (Stream dataStreamResponse = tResponse.GetResponseStream())
                                                {
                                                    using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                                    {
                                                        String sResponseFromServer = tReader.ReadToEnd();
                                                        string str = sResponseFromServer;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception exception)
                                    {
                                        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                                        _logger.Error(message);
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    catch (Exception ex) { }
                }
            }
        }

        public ResultDto UpdateSaudaDetails(SaudaUpdateDto saudaUpdateDto)
        {
            _methodName = "UpdateSaudaDetails";
            var resultDto = new ResultDto();
            try
            {
                if (saudaUpdateDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (saudaUpdateDto.ModifiedBy == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaUpdateDto.ModifiedBy);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                var mtpContext = _emamiContext.Sauda.FirstOrDefault(_ => _.Id == saudaUpdateDto.SaudaId);
                if (mtpContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                var saudaOrders = _emamiContext.SaudaOrders.Where(_ => _.SaudaId == mtpContext.Id);

                if (saudaOrders == null || !saudaOrders.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }

                //mtpContext.StatusId = saudaUpdateDto.StatusId;
                //mtpContext.Remarks = saudaUpdateDto.Remarks;
                mtpContext.ModifiedBy = saudaUpdateDto.ModifiedBy;
                mtpContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                //Sauda Order table status update
                foreach (var saudaorder in saudaOrders)
                {
                    saudaorder.StatusId = saudaUpdateDto.StatusId;
                    saudaorder.ModifiedBy = saudaUpdateDto.ModifiedBy;
                    saudaorder.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    saudaorder.Remarks = saudaUpdateDto.Remarks;
                }
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = mtpContext.Id;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        private ResultDto NotFoundResult()
        {
            var resultDto = new ResultDto();
            resultDto.IsSuccess = false;
            resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
            return resultDto;
        }

        private ResultDto ExceptionResult(Exception exception)
        {
            var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
            var resultDto = new ResultDto();
            resultDto.IsSuccess = false;
            resultDto.ErrorDto.ErrorCode = Constants.Exception;
            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
            _logger.Error(message);
            return resultDto;
        }

        private ResultDto SucessResult(Object obj)
        {
            var resultDto = new ResultDto();
            resultDto.IsSuccess = true;
            resultDto.SuccessDto.Response = obj;
            return resultDto;
        }

        #region Trade Ticket - Sauda

        //public ResultDto GetSaudhaOrderList(LoginUserIdDto inputDto)
        //{
        //    _methodName = "GetSaudhaOrderList";
        //    if (inputDto == null)
        //    {
        //        return NotFoundResult();
        //    }
        //    try
        //    {
        //        var data = _emamiContext.SaudaOrders.Where(_ => _.TradeTicketNumber == null).Include(s => s.Sku).Include(s => s.Sauda).Include(s => s.OilType).Select(s => new SaudaOrderViewDto()
        //        {
        //            SaudaOrderId = s.Id,
        //            SaudhaId = s.SaudaId,
        //            Oiltype = s.OilType.Name,
        //            OilTypeId = s.OilTypeId,
        //            BidQuantity = s.BidQuantity,
        //            BiddingDate = s.Sauda.BiddingDate,
        //            BidPrice = s.BidPrice,
        //            Sku = s.Sku.SkuName,
        //            SkuId = s.SkuId,
        //            TradeTicketNumber = string.Empty
        //        });
        //        return SucessResult(data);
        //    }
        //    catch (Exception exception)
        //    {
        //        return ExceptionResult(exception);
        //    }
        //}

        //public ResultDto GetSaudaOrdersTradeTicketMappingDetails(IdInputDto inputDto)
        //{
        //    _methodName = "GetSaudaOrdersTradeTicketMappingDetails";
        //    var outputDto = new TradeTicketSaudaMappingDto();

        //    if (inputDto == null)
        //    {
        //        return NotFoundResult();
        //    }
        //    try
        //    {
        //        var tradeTicketContext = _emamiContext.TradeTicket.AsNoTracking().Where(_ => _.Id == inputDto.Id).FirstOrDefault();
        //        if (tradeTicketContext != null)
        //        {
        //            var tradeTicketDetails = _emamiContext.TradeTicketDetails.AsNoTracking().Where(_ => _.TradeTicketId == inputDto.Id).ToList();
        //            long plantId = tradeTicketContext.DepotId;
        //            var materialTypeId = tradeTicketContext.MaterialTypeId;
        //            var depotIds = _emamiContext.PlantDepotMapping.AsNoTracking().Where(f => f.PlantId == plantId).Select(_ => _.DepotId).ToList();

        //            var saudaQuantity = 0m;
        //            var saudaOrderList = _emamiContext.SaudaOrders.AsNoTracking()
        //                .Where(_ => _.TradeTicketNumber == tradeTicketContext.TradeTicketNumber
        //                && (_.StatusId == (int)DTO.Enums.Status.Pending || _.StatusId == (int)Adani.Solution.DTO.Enums.Status.Approved)).ToList();
        //            if (saudaOrderList != null && saudaOrderList.Any())
        //            {
        //                saudaQuantity = saudaOrderList.Sum(_ => _.BidQuantity);
        //            }

        //            var saudaTotalQuantity = 0m;
        //            var allSaudaOrdersList = _emamiContext.Sauda.AsNoTracking()
        //                    .Join(_emamiContext.SaudaOrders.AsNoTracking().Where(w => (w.PlantId == plantId || depotIds.Contains(w.PlantId))
        //                    && w.StatusId == (int)Adani.Solution.DTO.Enums.Status.Pending && (w.TradeTicketNumber == string.Empty
        //                    || w.TradeTicketNumber == null)), s => s.Id, so => so.SaudaId, (s, so) => new { Sauda = s, SaudaOrders = so })
        //                    .Join(_emamiContext.Skus.AsNoTracking()
        //                    //.Where(w => w.MaterialTypeId == materialTypeId)
        //                    , so => so.SaudaOrders.SkuId, sk => sk.Id, (so, sk) => new { so.Sauda, so.SaudaOrders, Skus = sk })
        //                    .ToList();
        //            if (allSaudaOrdersList != null && allSaudaOrdersList.Any())
        //            {
        //                saudaTotalQuantity = allSaudaOrdersList.Sum(_ => _.SaudaOrders.BidQuantity);
        //            }

        //            var plantOrDepot = _emamiContext.Depots.Where(a => a.Id == plantId).FirstOrDefault();

        //            outputDto.TradeTicketOilTypes = string.Join(",", tradeTicketDetails.Select(_ => _.TradeTicketOilType.OilTypeName));
        //            outputDto.RatePerMT = tradeTicketContext.TotalCost / tradeTicketContext.ContractQuantity;
        //            outputDto.SaudaTotalQuantity = saudaTotalQuantity;
        //            outputDto.SaudaQuantity = saudaQuantity;
        //            outputDto.OpenQuantity = tradeTicketContext.OpenQuantityFromSap > 0 ? tradeTicketContext.OpenQuantityFromSap : tradeTicketContext.ContractQuantity - saudaQuantity;
        //            outputDto.SAPCreationDate = tradeTicketContext.ContractDate;
        //            outputDto.PlantName = plantOrDepot != null ? plantOrDepot.Name : string.Empty;
        //            outputDto.TradeTicketId = inputDto.Id;
        //            //outputDto.MaterialType = (tradeTicketContext.MaterialTypeId != 0 && tradeTicketContext.DivisionId != 0 && _emamiContext.MaterialTypes.AsNoTracking().FirstOrDefault(_ => _.Id == tradeTicketContext.MaterialTypeId && _.DivisionId == tradeTicketContext.DivisionId) != null) ? _emamiContext.MaterialTypes.AsNoTracking().FirstOrDefault(_ => _.Id == tradeTicketContext.MaterialTypeId && _.DivisionId == tradeTicketContext.DivisionId).Name : string.Empty;
        //            return SucessResult(outputDto);
        //        }
        //        else
        //        {
        //            return _resultService.ErrorMessage(Constants.RecordNotFound);
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        return ExceptionResult(exception);
        //    }
        //}

        //public ResultDto GetTradeTicketSaudaOrdersMappingList(TradeTicketSaudaSearchDto inputDto)
        //{
        //    _methodName = "GetTradeTicketSaudaOrdersMappingList";
        //    var UserIdMatchingInputState = new List<long>();
        //    var outputDto = new List<SaudaOrderViewDto>(); /*TradeTicketSaudaMappingDto();*/
        //    if (inputDto == null)
        //    {
        //        return NotFoundResult();
        //    }

        //    try
        //    {
        //        var tradeTicketContext = _emamiContext.TradeTicket.AsNoTracking().Where(_ => _.Id == inputDto.TradeTicketId).FirstOrDefault();
        //        if (tradeTicketContext != null)
        //        {
        //            var tradeTicketDetails = _emamiContext.TradeTicketDetails.AsNoTracking().Where(_ => _.TradeTicketId == inputDto.TradeTicketId).ToList();
        //            long plantId = tradeTicketContext.DepotId;
        //            var materialTypeId = tradeTicketContext.MaterialTypeId;
        //            var depotIds = _emamiContext.PlantDepotMapping.AsNoTracking().Where(f => f.PlantId == plantId).Select(_ => _.DepotId).ToList();

        //            if (inputDto.StateId != null)
        //            {
        //                UserIdMatchingInputState = _emamiContext.Users.AsNoTracking().Where(_ => inputDto.StateId.Contains(_.StateId)).Select(_ => _.Id).ToList();
        //                outputDto = _emamiContext.Sauda.AsNoTracking()
        //                       .Join(_emamiContext.SaudaOrders.AsNoTracking()
        //                       .Where(w => (w.PlantId == plantId || depotIds.Contains(w.PlantId))
        //                       && DbFunctions.TruncateTime(inputDto.FromDate) <= DbFunctions.TruncateTime(w.CreatedDate)
        //                       && DbFunctions.TruncateTime(w.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
        //                       && w.StatusId == (int)Adani.Solution.DTO.Enums.Status.Pending && (w.TradeTicketNumber == string.Empty || w.TradeTicketNumber == null)), s => s.Id, so => so.SaudaId, (s, so) => new { Sauda = s, SaudaOrders = so })
        //                       .Join(_emamiContext.Skus.AsNoTracking()
        //                       //.Where(w => w.MaterialTypeId == materialTypeId)
        //                       , so => so.SaudaOrders.SkuId, sk => sk.Id, (so, sk) => new { so.Sauda, so.SaudaOrders, Skus = sk })
        //                       .Join(_emamiContext.SkuUomMapping.AsNoTracking().Where(_ => _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos), x => x.SaudaOrders.SkuId, s => s.SkuId, (x, s) => new { x.SaudaOrders, x.Sauda, x.Skus,/* NoOfSkusPerCase = s.ConversionFactor */})
        //                       .Join(_emamiContext.Pricing.AsNoTracking(), so => so.SaudaOrders.PricingId, p => p.Id, (so, p) => new { so.SaudaOrders, so.Sauda, so.Skus, /*so.NoOfSkusPerCase*/ PricingPlantId = p.PlantId })
        //                       .Where(_ => inputDto.DealerId.Count != 0 ? inputDto.DealerId.Contains(_.Sauda.UserId) : UserIdMatchingInputState.Contains(_.Sauda.UserId)
        //                       && _.PricingPlantId == plantId)
        //                       .Select(s => new SaudaOrderViewDto
        //                       {
        //                           SaudhaId = s.Sauda.Id,
        //                           SaudaOrderId = s.SaudaOrders.Id,
        //                           SaudhaNumber = s.SaudaOrders.SaudaNumber,
        //                           OilTypeId = s.SaudaOrders.OilTypeId,
        //                           Oiltype = s.SaudaOrders.OilType.Name,
        //                           SkuId = s.SaudaOrders.SkuId,
        //                           Sku = s.SaudaOrders.Sku.SkuName,
        //                           BidQuantity = s.SaudaOrders.BidQuantity,
        //                           BidQuantityCase = s.SaudaOrders.BidQuantityCase,
        //                           BidPrice = s.SaudaOrders.BidPrice,
        //                           PlantName = (_emamiContext.Depots.FirstOrDefault(_ => _.Id == s.SaudaOrders.PlantId && _.IsPlant).IsPlant)
        //                                    ? _emamiContext.Depots.FirstOrDefault(_ => _.Id == s.SaudaOrders.PlantId).Name
        //                                    : _emamiContext.PlantDepotMapping.FirstOrDefault(_ => _.DepotId == s.SaudaOrders.PlantId).PlantId > 0 ?
        //                                    _emamiContext.Depots.FirstOrDefault(_ => _.Id == _emamiContext.PlantDepotMapping.FirstOrDefault(m => m.DepotId == s.SaudaOrders.PlantId).PlantId).Name
        //                                    : string.Empty,
        //                           BookingDate = (DateTime)DbFunctions.TruncateTime(s.SaudaOrders.CreatedDate),
        //                           BidPricePerSku = (s.SaudaOrders.BidPrice / s.SaudaOrders.BidQuantityCase) /*/ s.NoOfSkusPerCase*/,
        //                           DealerName = _emamiContext.Users.FirstOrDefault(_ => _.Id == s.Sauda.UserId).Name,
        //                           StateName = _emamiContext.Users.FirstOrDefault(_ => _.Id == s.Sauda.UserId).StateId != 0 ? _emamiContext.State.FirstOrDefault(_ => _.Id == _emamiContext.Users.FirstOrDefault(x => x.Id == s.Sauda.UserId).StateId).StateName : string.Empty,
        //                       }).ToList();
        //                if (outputDto.IsAny())
        //                {
        //                    outputDto.ForEach(f => { f.BidQuantity = Utility.DecimalFormatThree(f.BidQuantity); });
        //                }
        //                return SucessResult(outputDto != null ? outputDto.OrderBy(_ => _.BookingDate).ToList() : outputDto);
        //            }
        //            else
        //            {
        //                outputDto = _emamiContext.Sauda.AsNoTracking()
        //                        .Join(_emamiContext.SaudaOrders.AsNoTracking()
        //                        .Where(w => (w.PlantId == plantId || depotIds.Contains(w.PlantId))
        //                        && DbFunctions.TruncateTime(inputDto.FromDate) <= DbFunctions.TruncateTime(w.CreatedDate)
        //                        && DbFunctions.TruncateTime(w.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
        //                        && w.StatusId == (int)Adani.Solution.DTO.Enums.Status.Pending && (w.TradeTicketNumber == string.Empty || w.TradeTicketNumber == null)), s => s.Id, so => so.SaudaId, (s, so) => new { Sauda = s, SaudaOrders = so })
        //                        .Join(_emamiContext.Skus.AsNoTracking()
        //                        //.Where(w => w.MaterialTypeId == materialTypeId)
        //                        , so => so.SaudaOrders.SkuId, sk => sk.Id, (so, sk) => new { so.Sauda, so.SaudaOrders, Skus = sk })
        //                        .Join(_emamiContext.SkuUomMapping.AsNoTracking().Where(_ => _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos), x => x.SaudaOrders.SkuId, s => s.SkuId, (x, s) => new { x.SaudaOrders, x.Sauda, x.Skus, /*NoOfSkusPerCase = s.ConversionFactor*/ })
        //                        .Join(_emamiContext.Pricing.AsNoTracking(), so => so.SaudaOrders.PricingId, p => p.Id, (so, p) => new { so.SaudaOrders, so.Sauda, so.Skus, /*o.NoOfSkusPerCase*/ PricingPlantId = p.PlantId })
        //                        .Where(_ => _.PricingPlantId == plantId)
        //                        .Select(s => new SaudaOrderViewDto
        //                        {
        //                            SaudhaId = s.Sauda.Id,
        //                            SaudaOrderId = s.SaudaOrders.Id,
        //                            SaudhaNumber = s.SaudaOrders.SaudaNumber,
        //                            OilTypeId = s.SaudaOrders.OilTypeId,
        //                            Oiltype = s.SaudaOrders.OilType.Name,
        //                            SkuId = s.SaudaOrders.SkuId,
        //                            Sku = s.SaudaOrders.Sku.SkuName,
        //                            BidQuantity = s.SaudaOrders.BidQuantity,
        //                            BidQuantityCase = s.SaudaOrders.BidQuantityCase,
        //                            BidPrice = s.SaudaOrders.BidPrice,
        //                            PlantName = (_emamiContext.Depots.FirstOrDefault(_ => _.Id == s.SaudaOrders.PlantId && _.IsPlant).IsPlant)
        //                                    ? _emamiContext.Depots.FirstOrDefault(_ => _.Id == s.SaudaOrders.PlantId).Name
        //                                    : _emamiContext.PlantDepotMapping.FirstOrDefault(_ => _.DepotId == s.SaudaOrders.PlantId).PlantId > 0 ?
        //                                    _emamiContext.Depots.FirstOrDefault(_ => _.Id == _emamiContext.PlantDepotMapping.FirstOrDefault(m => m.DepotId == s.SaudaOrders.PlantId).PlantId).Name
        //                                    : string.Empty,
        //                            BookingDate = (DateTime)DbFunctions.TruncateTime(s.SaudaOrders.CreatedDate),
        //                            BidPricePerSku = (s.SaudaOrders.BidPrice / s.SaudaOrders.BidQuantityCase) /*/ s.NoOfSkusPerCase*/,
        //                            DealerName = _emamiContext.Users.FirstOrDefault(_ => _.Id == s.Sauda.UserId).Name,
        //                            StateName = _emamiContext.Users.FirstOrDefault(_ => _.Id == s.Sauda.UserId).StateId != 0 ? _emamiContext.State.FirstOrDefault(_ => _.Id == _emamiContext.Users.FirstOrDefault(x => x.Id == s.Sauda.UserId).StateId).StateName : string.Empty,
        //                        }).ToList();
        //                if (outputDto.IsAny())
        //                {
        //                    outputDto.ForEach(f => { f.BidQuantity = Utility.DecimalFormatThree(f.BidQuantity); });
        //                }
        //                return SucessResult(outputDto != null ? outputDto.OrderBy(_ => _.BookingDate).ToList() : outputDto);
        //            }
        //        }
        //        else
        //        {
        //            return _resultService.ErrorMessage(Constants.RecordNotFound);
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        return ExceptionResult(exception);
        //    }
        //}

        //public ResultDto MapTradeTicketToSaudaOrders(TradeTicketMaptoSaudaOrderDto inputDto)
        //{
        //    _methodName = "MapTradeTicketToSaudaOrders";
        //    if (inputDto == null)
        //    {
        //        return NotFoundResult();
        //    }
        //    try
        //    {
        //        //var soBidQuantity = _emamiContext.SaudaOrders.AsNoTracking().Where(w => w.TradeTicketNumber == inputDto.TradeTicketNumber).Select(s => s.BidQuantity).DefaultIfEmpty(0).Sum();
        //        //var mappingBidQuantity = _emamiContext.SaudaOrders.AsNoTracking().Where(w => inputDto.SaudaOrders.Contains(w.Id)).Select(s => s.BidQuantity).DefaultIfEmpty(0).Sum();
        //        //var totalBidQty = soBidQuantity + mappingBidQuantity;
        //        var contractQty = _emamiContext.TradeTicket.AsNoTracking().FirstOrDefault(f => f.TradeTicketNumber == inputDto.TradeTicketNumber).ContractQuantity;
        //        //var soBidQuantity = _emamiContext.SaudaOrders.AsNoTracking().Where(w => w.TradeTicketNumber == inputDto.TradeTicketNumber || inputDto.SaudaOrders.Contains(w.Id)).Select(s => s.BidQuantity).DefaultIfEmpty(0).Sum();
        //        var inputQuanitity = _emamiContext.SaudaOrders.AsNoTracking().Where(w => inputDto.SaudaOrders.Contains(w.Id)).Select(s => s.BidQuantity).DefaultIfEmpty(0).Sum();
        //        var soBookedQuantity = _emamiContext.SaudaOrders.AsNoTracking().Where(w => w.TradeTicketNumber == inputDto.TradeTicketNumber && (w.StatusId != (int)DTO.Enums.Status.Rejected)).Select(s => s.BidQuantity).DefaultIfEmpty(0).Sum();

        //        var availableQuanitity = contractQty - soBookedQuantity;
        //        if (inputDto.OpenQuantity == availableQuanitity)
        //        {
        //            if (!(inputQuanitity <= availableQuanitity))
        //            {
        //                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
        //                var User = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == 1);
        //                var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.TradeTicketQuantityIncrease);
        //                List<string> toUser = new List<string>();
        //                var fromEmail = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(f => f.Key == Constants.NotificationEmail).Value;
        //                var emailSubject = Constants.TradeTicketQuantityIncreaseSub;
        //                toUser = fromEmail.Split(',').ToList();
        //                var plainText = string.Empty;
        //                if (emailTemplate != null)
        //                {
        //                    var htmlPlainTemplate = emailTemplate.PlainTemplate.Replace(Constants.ContractQty, contractQty.ToString()).Replace(Constants.BiddingQty, soBookedQuantity.ToString());
        //                    var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, htmlPlainTemplate);
        //                    amazonNotificationService.SendEmail(toUser, emailSubject, plainText, htmlTemplate, true);
        //                }
        //                //var smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.TradeTicketQuantityIncrease);
        //                //if (smsTemplate != null)
        //                //{
        //                //    var htmlPlainTemplate = emailTemplate.PlainTemplate.Replace(Constants.ContractQty, contractQty.ToString()).Replace(Constants.BiddingQty, soBidQuantity.ToString());
        //                //    amazonNotificationService.SendMessage(htmlPlainTemplate, User.MobileNumber);
        //                //}

        //                //return _resultService.ErrorMessage("Trade Ticket ContractQuantity is " + "<b>" + contractQty + "</b>" + "<br>" + "Mapping Sauda Total BidQuantity is " + "<b>" + soBookedQuantity + "<b>" + "<br>" + "Total BidQuantity should be less then or equal to ContractQuantity");
        //                return _resultService.ErrorMessage("Your Sauda Mapping Quanitity : " + "<b>" + inputQuanitity + "<b>" + " Available Quanitity : " + "<b>" + availableQuanitity + "<b>" + "<br>" + " Sauda Mapping Quanitity should be less then or equal to Available Quantity");
        //            }
        //        }
        //        else
        //        {
        //            var tradeTicket = _emamiContext.TradeTicket.FirstOrDefault(f => f.TradeTicketNumber == inputDto.TradeTicketNumber);
        //            tradeTicket.OpenQuantityFromSap = inputDto.OpenQuantity;
        //            _emamiContext.SaveChanges();
        //            if (!(inputQuanitity <= inputDto.OpenQuantity))
        //            {
        //                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
        //                var User = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == 1);
        //                var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.TradeTicketQuantityIncrease);
        //                List<string> toUser = new List<string>();
        //                var fromEmail = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(f => f.Key == Constants.NotificationEmail).Value;
        //                var emailSubject = Constants.TradeTicketQuantityIncreaseSub;
        //                toUser = fromEmail.Split(',').ToList();
        //                var plainText = string.Empty;
        //                if (emailTemplate != null)
        //                {
        //                    var htmlPlainTemplate = emailTemplate.PlainTemplate.Replace(Constants.ContractQty, contractQty.ToString()).Replace(Constants.BiddingQty, soBookedQuantity.ToString());
        //                    var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, htmlPlainTemplate);
        //                    amazonNotificationService.SendEmail(toUser, emailSubject, plainText, htmlTemplate, true);
        //                }
        //                //var smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.TradeTicketQuantityIncrease);
        //                //if (smsTemplate != null)
        //                //{
        //                //    var htmlPlainTemplate = emailTemplate.PlainTemplate.Replace(Constants.ContractQty, contractQty.ToString()).Replace(Constants.BiddingQty, soBidQuantity.ToString());
        //                //    amazonNotificationService.SendMessage(htmlPlainTemplate, User.MobileNumber);
        //                //}

        //                //return _resultService.ErrorMessage("Trade Ticket ContractQuantity is " + "<b>" + contractQty + "</b>" + "<br>" + "Mapping Sauda Total BidQuantity is " + "<b>" + soBookedQuantity + "<b>" + "<br>" + "Total BidQuantity should be less then or equal to ContractQuantity");
        //                return _resultService.ErrorMessage("Your Sauda Mapping Quanitity : " + "<b>" + inputQuanitity + "<b>" + " Available Quanitity : " + "<b>" + inputDto.OpenQuantity + "<b>" + "<br>" + " Sauda Mapping Quanitity should be less then or equal to Available Quantity");
        //            }
        //        }


        //        var saudaOrders = _emamiContext.SaudaOrders.Where(_ => inputDto.SaudaOrders.Contains(_.Id));
        //        foreach (var orders in saudaOrders)
        //        {
        //            orders.TradeTicketNumber = inputDto.TradeTicketNumber;
        //            orders.SaudaTTAttachedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //            orders.ModifiedBy = inputDto.LoginUserId;
        //            orders.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //        }
        //        _emamiContext.SaveChanges();

        //        if (inputDto.OpenQuantity != availableQuanitity)
        //        {
        //            var tradeTicketcontext = _emamiContext.TradeTicket.FirstOrDefault(f => f.TradeTicketNumber == inputDto.TradeTicketNumber);
        //            tradeTicketcontext.OpenQuantityFromSap = tradeTicketcontext.OpenQuantityFromSap - inputQuanitity;
        //            _emamiContext.SaveChanges();
        //        }

        //        return SucessResult(inputDto);
        //    }
        //    catch (Exception exception)
        //    {
        //        return ExceptionResult(exception);
        //    }
        //}

        #endregion

        //public ResultDto GetDealersBySalesPerson(LoginUserIdDto inputDto)
        //{
        //    _methodName = "GetDealersBySalesPerson";
        //    var userMasterDto = new List<UserMasterDto>();
        //    if (inputDto == null)
        //    {
        //        return NotFoundResult();
        //    }
        //    try
        //    {
        //        var dealerlist = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
        //                          join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
        //                          where ucm.UserId == inputDto.LoginUserId
        //                          select new UserMasterDto
        //                          {
        //                              Id = u.Id,
        //                              EmployeeName = u.Name,
        //                              EmployeeCode = u.Code,
        //                              FrieghtRoute = u.FreightZone.Name,
        //                              FrieghtZone = u.FreightRoute.Name
        //                              //DealerLocation = _emamiContext.DealerLocation.AsNoTracking().Where(_ => _.UserId == u.Id).Select(s => new DealerLocationDto()
        //                              //{
        //                              //    Address = s.Address,
        //                              //    CityId = s.CityId,
        //                              //    DistrictId = s.DistrictId,
        //                              //    StateId = s.StateId,
        //                              //    UserId = s.UserId,
        //                              //    City = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == s.CityId).CityName,
        //                              //    District = _emamiContext.District.AsNoTracking().FirstOrDefault(_ => _.Id == s.DistrictId).DistrictName,
        //                              //    State = _emamiContext.State.AsNoTracking().FirstOrDefault(_ => _.Id == s.StateId).StateName
        //                              //}
        //                              //).ToList()
        //                          }).ToList();


        //        return SucessResult(dealerlist);
        //    }
        //    catch (Exception exception)
        //    {
        //        return ExceptionResult(exception);
        //    }
        //}

        public ResultDto UpdateSaudaLimit(SaudaLimitRequestHistoryDto saudaLimitRequestHistoryDto)
        {
            _methodName = "UpdateSaudaLimit";
            var resultDto = new ResultDto();
            try
            {
                if (saudaLimitRequestHistoryDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (saudaLimitRequestHistoryDto.CreatedBy == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaLimitRequestHistoryDto.CreatedBy);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                var saudalimitContext = _emamiContext.SaudaLimit.AsNoTracking().FirstOrDefault(_ => _.Id == saudaLimitRequestHistoryDto.Id);
                if (saudalimitContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                saudalimitContext.StatusId = saudaLimitRequestHistoryDto.StatusId;
                saudalimitContext.Remarks = saudaLimitRequestHistoryDto.Remarks;
                saudalimitContext.ModifiedBy = saudaLimitRequestHistoryDto.CreatedBy;
                saudalimitContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = saudalimitContext.Id;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto ApproveorRejectSaudaLimit(SaudaLimitRequestDto inputDto)
        {
            _methodName = "ApproveSaudaLimit";
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (!inputDto.LimitRequest.Any())
                {
                    return _resultService.ErrorMessage(Constants.LimitRequestMissing);
                }
                //if (inputDto.SalesOrganizationId == 0)
                //{
                //    return _resultService.ErrorMessage(Constants.SalesOrganisationIsEmpty);
                //}
                //if (inputDto.DistributionChannelId == 0)
                //{
                //    return _resultService.ErrorMessage(Constants.DistributionChannelIsEmpty);
                //}
                //if (inputDto.DivisionId == 0)
                //{
                //    return _resultService.ErrorMessage(Constants.DivisionIsEmpty);
                //}
                foreach (var limitRequestId in inputDto.LimitRequest)
                {
                    var limitContext = _emamiContext.SaudaLimit.FirstOrDefault(_ => _.Id == limitRequestId.Id);
                    if (limitContext != null)
                    {
                        if (limitContext.StatusId == (int)DTO.Enums.Status.Pending || limitContext.StatusId == (int)DTO.Enums.Status.RequestForApproval)
                        {
                            limitContext.Remarks = inputDto.Remark;
                            limitContext.StatusId = inputDto.Status;
                            limitContext.ModifiedBy = limitContext.CreatedBy;
                            limitContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        }
                        var userContext = _emamiContext.Users.Where(_ => _.Id == limitContext.UserId).FirstOrDefault();
                        var userdivContext = _emamiContext.UserDivisionMappings
                              .FirstOrDefault(_ => _.UserId == limitContext.UserId
                              && _.SalesOrganizationId == limitContext.SalesOrganizationId && _.DistributionChannelId == limitContext.DistributionChannelId
                              && _.DivisionId == limitContext.DivisionId);

                        if (limitContext.StatusId == (int)DTO.Enums.Status.Approved)
                        {
                            limitContext.ActualLimit = userdivContext.SaudaLimit ?? 0;
                            limitContext.RequestedLimit = limitRequestId.RequestedLimitRequest;
                            userdivContext.SaudaLimit = limitContext.ActualLimit + limitRequestId.RequestedLimitRequest;
                        }
                        _emamiContext.SaveChanges();
                        try
                        {
                            List<User> usersContext = new List<User>();
                            List<string> toUsers = new List<string>();
                            User createdBy = new User();
                            User dealer = new User();
                            bool isEmail = false;
                            var DealerNotificationContext = _emamiContext.TPNotification.AsNoTracking().
                                                       Join(_emamiContext.TPNotificationDetails.AsNoTracking(), TPN => TPN.Id, TPND => TPND.TPNotificationId, (TPN, TPND) => new { TPN, TPND })
                                                       .Where(_ => _.TPND.DealerId == limitContext.UserId && _.TPND.NotificationActionId == (int)DTO.Enums.NotificationActionTP.LimitEnhancementRequestApproval && _.TPND.IsActive).ToList();
                            if (limitContext.CreatedBy == limitContext.UserId)
                            {
                                createdBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == limitContext.CreatedBy);
                                if (createdBy != null)
                                {
                                    toUsers.Add(createdBy.Email);
                                    isEmail = true;
                                }
                            }
                            else
                            {
                                usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == limitContext.CreatedBy || _.Id == limitContext.UserId).ToList();
                                if (usersContext != null && usersContext.Any())
                                {
                                    createdBy = usersContext.FirstOrDefault(_ => _.Id == limitContext.CreatedBy);
                                    dealer = usersContext.FirstOrDefault(_ => _.Id == limitContext.UserId);
                                    if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
                                    {
                                        toUsers.Add(createdBy.Email);
                                        isEmail = true;
                                    }
                                    if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                                    {
                                        toUsers.Add(dealer.Email);
                                        if (DealerNotificationContext != null)
                                        {
                                            var DealerEmailNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.Email);
                                            if (DealerEmailNotificationEnabled != null && DealerEmailNotificationEnabled.Any())
                                                isEmail = true;
                                            else
                                                isEmail = false;
                                        }
                                    }
                                }
                            }

                            if ((usersContext != null && usersContext.Any()) || createdBy != null)
                            {
                                decimal actualLimit = limitContext.ActualLimit;
                                decimal extendedLimit = limitContext.ActualLimit + limitContext.RequestedLimit;
                                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                var emailSubject = string.Empty;

                                if (isEmail && toUsers != null && toUsers.Any())
                                {
                                    var fromEmail = Constants.FromEmail;
                                    var plainText = string.Empty;

                                    EmailTemplate emailTemplate = new EmailTemplate();
                                    if (inputDto.Status == (int)DTO.Enums.Status.Approved)
                                    {
                                        emailSubject = Constants.SaudaLimitApprovalSubject;
                                        emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaLimitApprovalEmail);
                                    }
                                    else if (inputDto.Status == (int)DTO.Enums.Status.Rejected)
                                    {
                                        emailSubject = Constants.SaudaLimitRejectSubject;
                                        emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaLimitRejectEmail);
                                    }
                                    if (emailTemplate != null)
                                    {
                                        var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.ContractQty, actualLimit.ToString()).Replace(Constants.Quantity, extendedLimit.ToString()).Replace(Constants.CustomerName, dealer.Name);
                                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                        amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                    }
                                }
                                var smsPlainTemplate = string.Empty;
                                bool isSms = false;
                                if (DealerNotificationContext != null)
                                {
                                    var DealerSMSNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.SMS);
                                    if (DealerSMSNotificationEnabled != null && DealerSMSNotificationEnabled.Any())
                                        isSms = true;
                                    else
                                        isSms = false;
                                }
                                if (isSms)
                                {

                                    var smsMessage = string.Empty;
                                    EmailTemplate smsTemplate = new EmailTemplate();
                                    if (inputDto.Status == (int)DTO.Enums.Status.Approved)
                                    {
                                        smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaLimitApprovalSMS);
                                    }
                                    else if (inputDto.Status == (int)DTO.Enums.Status.Rejected)
                                    {
                                        smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaLimitRejectSMS);
                                    }
                                    if (smsTemplate != null)
                                    {
                                        smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.ContractQty, actualLimit.ToString()).Replace(Constants.Quantity, extendedLimit.ToString()).Replace(Constants.CustomerName, dealer.Name);
                                        smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                        if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                                        {
                                            amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber, smsTemplate.SMSTemplateID);
                                        }
                                        if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                        {
                                            amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber, smsTemplate.SMSTemplateID);
                                        }
                                    }
                                }
                                bool IsPushNotification = false;
                                var DealerPushNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.InAppNotification);
                                if (DealerPushNotificationEnabled != null && DealerPushNotificationEnabled.Any())
                                    IsPushNotification = true;
                                else
                                    IsPushNotification = false;
                                if (IsPushNotification)
                                {
                                    if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
                                    {
                                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                        {
                                            PushTokenKey = createdBy.PushTokenKey,
                                            RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
                                            Title = emailSubject,
                                            Message = smsPlainTemplate,
                                            //Id = limitContext.Id,
                                        };
                                        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    }
                                    if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
                                    {
                                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                        {
                                            PushTokenKey = dealer.PushTokenKey,
                                            RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
                                            Title = emailSubject,
                                            Message = smsPlainTemplate,
                                            //Id = limitContext.Id,
                                        };
                                        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                _emamiContext.SaveChanges();

                //try
                //{
                //    foreach (var limitRequestId in saudaLimitRequestDto.LimitRequest)
                //    {
                //        var limitContext = _emamiContext.SaudaLimit.FirstOrDefault(_ => _.Id == limitRequestId);
                //        if (limitContext != null)
                //        {
                //            AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                //            var User = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == limitContext.UserId);
                //            var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaLimitApprovalEmail);
                //            if (_resultService.IsEmail())
                //            {
                //                List<string> toUser = new List<string>();
                //                toUser.Add(User.Email);
                //                var emailSubject = Constants.SpecialRateApprovalSubject;
                //                var fromEmail = Constants.FromEmail;
                //                var plainText = string.Empty;

                //                if (emailTemplate != null)
                //                {
                //                    var htmlPlainTemplate = emailTemplate.PlainTemplate.Replace(Constants.Name, User.Name).Replace(Constants.Status, Enum.GetName(typeof(DTO.Enums.Status), limitContext.StatusId)).Replace(Constants.ContractQty, User.SaudaLimit.ToString()).Replace(Constants.Quantity, (User.SaudaLimit + limitContext.RequestedLimit).ToString());
                //                    var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, htmlPlainTemplate);
                //                    amazonNotificationService.SendEmail(toUser, emailSubject, plainText, htmlTemplate, true);
                //                }
                //            }
                //            if (_resultService.IsSMS())
                //            {
                //                var smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaLimitApprovalSMS);
                //                if (smsTemplate != null)
                //                {
                //                    var htmlPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.Name, User.Name).Replace(Constants.Status, Enum.GetName(typeof(DTO.Enums.Status), limitContext.StatusId)).Replace(Constants.ContractQty, User.SaudaLimit.ToString()).Replace(Constants.Quantity, (User.SaudaLimit + limitContext.RequestedLimit).ToString());
                //                    amazonNotificationService.SendMessage(htmlPlainTemplate, User.MobileNumber);
                //                }
                //            }

                //        }
                //    }
                //}
                //catch(Exception ex)
                //{

                //}
                return _resultService.SuccessMessage(Constants.LimitStatusUpdated);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetSaudaLimitsRequestHistory(SaudaLimitInputDto inputDto)
        {
            _methodName = "GetSaudaLimitsRequestHistory";
            var saudalimitHistoryDto = new List<SaudaLimitRequestHistoryDto>();
            if (inputDto == null)
            {
                return NotFoundResult();
            }
            if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
            {
                return _resultService.ErrorMessage(Constants.FromDateEmpty);
            }
            if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
            {
                return _resultService.ErrorMessage(Constants.ToDateEmpty);
            }
            if (inputDto.FromDate > inputDto.ToDate)
            {
                return _resultService.ErrorMessage(Constants.FromDateInvalid);
            }


            if (inputDto.LoginUserId == 0)
            {
                return _resultService.ErrorMessage(Constants.UserIdMissing);
            }
            if (!_resultService.UserIsAcive(inputDto.LoginUserId))
            {
                return _resultService.ErrorMessage(Constants.InvalidUser);
            }

            var roleId = _emamiContext.UserRoles.Where(_ => _.UserId == inputDto.LoginUserId).FirstOrDefault().RoleId;

            var bdoIds = new List<long>();
            var ZHIds = new List<long>();
            if (roleId == (int)DTO.Enums.Role.NationalTrader)
            {

                ZHIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(s => s.UserId).ToList();
                bdoIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => ZHIds.Contains(_.ReportingToUserId)).Select(s => s.UserId).ToList();

                //ZHIds = _emamiContext.Users.Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(s => s.Id).ToList();
                //bdoIds = _emamiContext.Users.Where(_ => ZHIds.Contains((long)_.ReportingToId)).Select(s => s.Id).ToList();
            }
            if (roleId == (int)DTO.Enums.Role.ZonalTrader)
            {
                ZHIds.Add(inputDto.LoginUserId);
                bdoIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(s => s.UserId).ToList();

                bdoIds = _emamiContext.Users.Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(s => s.Id).ToList();
            }
            if (roleId == (int)DTO.Enums.Role.StateTrader)
            {
                bdoIds.Add(inputDto.LoginUserId);
            }

            try
            {
                IEnumerable<DivisionDetailsDto> divisionslogieduser = new List<DivisionDetailsDto>();
                if (roleId == (int)DTO.Enums.Role.Admin)
                {
                    divisionslogieduser = _emamiContext.Divisions.AsNoTracking().Select(s => new DivisionDetailsDto()
                    {
                        SalesOrganizationId = s.SalesOrganizationId,
                        DistributionChannelId = s.DistributionChannelId,
                        DivisionId = s.Id
                    });
                }
                else
                {
                    divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                 .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });
                }


                var saudaLimitHistoryQueryContext = (from s in _emamiContext.SaudaLimit.AsNoTracking()
                                                     join ud in divisionslogieduser on new { s.SalesOrganizationId, s.DistributionChannelId, s.DivisionId } equals new { ud.SalesOrganizationId, ud.DistributionChannelId, ud.DivisionId }
                                                     join u in _emamiContext.Users.AsNoTracking() on s.CreatedBy equals u.Id
                                                     where DbFunctions.TruncateTime(s.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                                                     && DbFunctions.TruncateTime(s.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                                                     && !s.IsSAPData
                                                     && (inputDto.SalesOrganizationId > 0 ? s.SalesOrganizationId == inputDto.SalesOrganizationId : s.SalesOrganizationId > 0)
                                                     && (inputDto.DistributionChannelId > 0 ? s.DistributionChannelId == inputDto.DistributionChannelId : s.DistributionChannelId > 0)
                                                     && (inputDto.DivisionId > 0 ? s.DivisionId == inputDto.DivisionId : s.DivisionId > 0)
                                                     select new { s, u }
                    );
                //var saudaLimitHistoryQueryContext = _emamiContext.SaudaLimit.AsNoTracking().Where(_ => 
                //DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                // && !_.IsSAPData && (inputDto.SalesOrganizationId > 0 ? _.SalesOrganizationId == inputDto.SalesOrganizationId : _.SalesOrganizationId > 0) && (inputDto.DistributionChannelId > 0 ? _.DistributionChannelId == inputDto.DistributionChannelId : _.DistributionChannelId > 0) && (inputDto.DivisionId > 0 ? _.DivisionId == inputDto.DivisionId : _.DivisionId > 0)
                //).AsNoTracking().AsQueryable();

                var saudaLimt = new List<SaudaLimitRequestHistoryDto>();
                if (ZHIds.IsAny())
                {
                    saudaLimt.AddRange(saudaLimitHistoryQueryContext.Where(_ => ZHIds.Contains(_.s.CreatedBy)).Select(s => new SaudaLimitRequestHistoryDto()
                    {
                        Id = s.s.Id,
                        LimitRequestNo = s.s.Id.ToString(),
                        Remarks = s.s.Remarks,
                        RequestDate = s.s.CreatedDate,
                        ActualLimit = s.s.ActualLimit,
                        RequestQuantityLimit = s.s.RequestedLimit,
                        StatusId = s.s.StatusId,
                        CreatedByName = s.u.Name,
                        DealerName = s.s.User.Name,
                        Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == s.s.StatusId).Name,
                    }));
                }
                if (bdoIds.IsAny())
                {
                    saudaLimt.AddRange(saudaLimitHistoryQueryContext.Where(_ => bdoIds.Contains(_.s.CreatedBy)).Select(s => new SaudaLimitRequestHistoryDto()
                    {
                        Id = s.s.Id,
                        LimitRequestNo = s.s.Id.ToString(),
                        Remarks = s.s.Remarks,
                        RequestDate = s.s.CreatedDate,
                        ActualLimit = s.s.ActualLimit,
                        RequestQuantityLimit = s.s.RequestedLimit,
                        StatusId = s.s.StatusId,
                        CreatedByName = s.u.Name,
                        DealerName = s.s.User.Name,
                        Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == s.s.StatusId).Name,
                    }));
                    //saudaLimt.AddRange(saudaLimitHistoryQueryContext.Where(_ => bdoIds.Contains(_.CreatedBy)));
                }
                if (ZHIds.IsNotAny() && bdoIds.IsNotAny())
                {
                    saudaLimt.AddRange(saudaLimitHistoryQueryContext.Select(s => new SaudaLimitRequestHistoryDto()
                    {
                        Id = s.s.Id,
                        LimitRequestNo = s.s.Id.ToString(),
                        Remarks = s.s.Remarks,
                        RequestDate = s.s.CreatedDate,
                        ActualLimit = s.s.ActualLimit,
                        RequestQuantityLimit = s.s.RequestedLimit,
                        StatusId = s.s.StatusId,
                        CreatedByName = s.u.Name,
                        DealerName = s.s.User.Name,
                        Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == s.s.StatusId).Name,
                    }));
                }
                if (inputDto.StatusId > 0)
                {
                    saudaLimt = saudaLimt.Where(_ => _.StatusId == inputDto.StatusId).ToList();
                    //saudaLimt = saudaLimt.Where(_ => _.StatusId == inputDto.StatusId).ToList();
                }



                //saudalimitHistoryDto = saudaLimt.Select(c => new SaudaLimitRequestHistoryDto
                //{
                //    Id = c.Id,
                //    LimitRequestNo = c.Id.ToString(),
                //    Remarks = c.Remarks,
                //    RequestDate = c.CreatedDate,
                //    ActualLimit = c.ActualLimit,
                //    RequestQuantityLimit = c.RequestedLimit,
                //    StatusId = c.StatusId,
                //    DealerName = c.User.Name,
                //    Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == c.StatusId).Name,
                //}).ToList();
                var datasourceResult = saudaLimt != null ? saudaLimt.OrderByDescending(_ => _.Id).ToDataSourceResult(inputDto.DataSourceRequest) : saudaLimt.ToDataSourceResult(inputDto.DataSourceRequest);



                //return SucessResult(saudalimitHistoryDto != null ? saudalimitHistoryDto.OrderByDescending(_ => _.Id).ToList() : saudalimitHistoryDto);
                return SucessResult(datasourceResult);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto ApproveorRejectSpecialRate(SpecialRateRequestDto specialRateRequestDto)
        {
            _methodName = "ApproveorRejectSpecialRate";
            try
            {
                if (specialRateRequestDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (!specialRateRequestDto.SpecialRateRequest.Any())
                {
                    return _resultService.ErrorMessage(Constants.SpecialRateRequestMissing);
                }
                if (specialRateRequestDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                if (!_resultService.UserIsAcive(specialRateRequestDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                var userContext = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.Id == specialRateRequestDto.LoginUserId && _.RoleId == (int)DTO.Enums.Role.NationalTrader).FirstOrDefault();
                foreach (var specialRateRequestId in specialRateRequestDto.SpecialRateRequest)
                {
                    var specialRateContext = _emamiContext.SpecialRate.FirstOrDefault(_ => _.Id == specialRateRequestId);
                    if (specialRateContext != null)
                    {
                        specialRateContext.Remarks = specialRateRequestDto.Remark;
                        specialRateContext.StatusId = userContext != null ? specialRateRequestDto.Status : ((specialRateRequestDto.Status == (int)DTO.Enums.Status.Approved) ? (int)DTO.Enums.Status.WaitingForApproval : specialRateRequestDto.Status);
                    }

                }
                _emamiContext.SaveChanges();

                foreach (var specialRateRequestId in specialRateRequestDto.SpecialRateRequest)
                {
                    var specialRateContext = _emamiContext.SpecialRate.FirstOrDefault(_ => _.Id == specialRateRequestId);
                    if (specialRateContext != null)
                    {
                        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                        var User = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == specialRateContext.UserId);
                        var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecialRateApprovalEmail);
                        if (_resultService.IsEmail())
                        {
                            List<string> toUser = new List<string>();
                            toUser.Add(User.Email);
                            var emailSubject = Constants.SpecialRateApprovalSubject;
                            var fromEmail = Constants.FromEmail;
                            var plainText = string.Empty;
                            if (emailTemplate != null)
                            {
                                var htmlPlainTemplate = emailTemplate.PlainTemplate.Replace(Constants.Name, User.Name).Replace(Constants.Status, specialRateContext.Status.Name);
                                var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, htmlPlainTemplate);
                                amazonNotificationService.SendEmail(toUser, emailSubject, plainText, htmlTemplate, true);
                            }
                        }
                        if (_resultService.IsSMS())
                        {
                            var smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecialRateApprovalSMS);
                            if (smsTemplate != null)
                            {
                                var htmlPlainTemplate = emailTemplate.PlainTemplate.Replace(Constants.Name, User.Name).Replace(Constants.Status, specialRateContext.Status.Name);
                                amazonNotificationService.SendMessage(htmlPlainTemplate, User.MobileNumber, smsTemplate.SMSTemplateID);
                            }
                        }
                    }
                }
                return _resultService.SuccessMessage(Constants.SpecialRateStatusUpdated);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetSpecialRateApprovalList(SpecialRateAddInputDto inputDto)
        {
            _methodName = "GetSpecialRateApprovalList";
            var specialRateApprovalOutputDto = new List<SpecialRateApprovalOutputDto>();
            if (inputDto == null)
            {
                return NotFoundResult();
            }
            if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
            {
                return _resultService.ErrorMessage(Constants.FromDateEmpty);
            }
            if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
            {
                return _resultService.ErrorMessage(Constants.ToDateEmpty);
            }
            if (inputDto.FromDate > inputDto.ToDate)
            {
                return _resultService.ErrorMessage(Constants.FromDateInvalid);
            }
            if (inputDto.LoginUserId == 0)
            {
                return _resultService.ErrorMessage(Constants.UserIdMissing);
            }
            if (!_resultService.UserIsAcive(inputDto.LoginUserId))
            {
                return _resultService.ErrorMessage(Constants.InvalidUser);
            }

            try
            {
                var userContext = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId && (_.RoleId == (int)DTO.Enums.Role.NationalTrader || _.RoleId == (int)DTO.Enums.Role.Admin)).FirstOrDefault();
                var specialRateQueryable = _emamiContext.SpecialRate.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate) &&
                    (inputDto.VerticalId > 0 ? _.OilType.DivisionId == inputDto.VerticalId : _.OilType.DivisionId > 0)).AsQueryable();
                if (userContext == null)
                {
                    specialRateQueryable = specialRateQueryable.Where(_ => _.User.ReportingToId == inputDto.LoginUserId);
                }
                if (inputDto.StatusId > 0)
                {
                    specialRateQueryable = specialRateQueryable.Where(_ => _.StatusId == inputDto.StatusId);
                }

                specialRateApprovalOutputDto = specialRateQueryable.AsNoTracking().Select(c => new SpecialRateApprovalOutputDto
                {
                    Id = c.Id,
                    FinalPrice = c.FinalPrice,
                    OilTypeId = c.OilTypeId,
                    Quantity = c.Quantity,
                    SpecialPrice = c.SpecialPrice,
                    SkuName = c.Sku.SkuName,
                    SkuCode = c.Sku.SkuCode,
                    StatusId = (int)c.StatusId,
                    DealerName = c.User.Name,
                    Remarks = c.Remarks,
                    //FreightRoute = c.FreightRoute != null ? c.FreightRoute.Name : string.Empty,
                    IncoTerms = c.Incoterms1 + "," + c.Incoterms2,
                    CreatedBy = _emamiContext.Users.FirstOrDefault(_ => _.Id == c.CreatedBy).Name,
                    OilTypeName = _emamiContext.OilTypes.FirstOrDefault(_ => _.Id == c.OilTypeId).Name,
                    Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == c.StatusId).Name,
                    IsLTD = c.IsLTD,
                    LTD_SR = c.IsLTD == true ? UtilityHelper.GetEnumDescription(DTO.Enums.LTDSR.LTD) : UtilityHelper.GetEnumDescription(DTO.Enums.LTDSR.SpecialRate)
                }).ToList();


                return SucessResult(specialRateApprovalOutputDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        /// <summary>
        /// Method to update counter bid offer and send notification
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        //public ResultDto SendCounterBidNotification(LoginUserIdDto inputDto)
        //{
        //    _methodName = "SendCounterBidNotification";
        //    var resultDto = new ResultDto();
        //    AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
        //    List<CounterBidNotificationSku> notificationSku = new List<CounterBidNotificationSku>();
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }
        //        if (inputDto.LoginUserId == 0)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidUser);
        //        }
        //        var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
        //        if (userContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserNotFound);
        //        }
        //        var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //        var currentTime = new TimeSpan(currentDate.Hour, currentDate.Minute, currentDate.Second);
        //        var biddingWindowContext = _emamiContext.BiddingWindowTiming.AsNoTracking().FirstOrDefault(_ => _.IsLastWindowPerDay == true && _.ToHours < currentTime
        //            && DbFunctions.TruncateTime(_.BiddingDate) == DbFunctions.TruncateTime(currentDate));
        //        if (biddingWindowContext == null)
        //        {
        //            SendServiceNotification("Counter Bid Notification");
        //            return _resultService.ErrorMessage(Constants.RecordNotFound);
        //        }

        //        var saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().Join(_emamiContext.Sauda.AsNoTracking(), so => so.SaudaId, s => s.Id, (so, s) => new { so, s })
        //                                .Join(_emamiContext.Pricing.AsNoTracking(), sos => sos.so.PricingId, p => p.Id, (sos, p) => new { sos.so, sos.s, p })
        //                                .Where(_ => _.so.StatusId == (int)DTO.Enums.Status.Hold && DbFunctions.TruncateTime(_.s.BiddingDate) == DbFunctions.TruncateTime(currentDate)
        //                                && _.so.CounterBidOffer == 0 && _.so != null && _.s != null && _.p != null).ToList();
        //        if (saudaOrderListContext != null && saudaOrderListContext.Any())
        //        {
        //            foreach (var saudaOrderContext in saudaOrderListContext)
        //            {
        //                var counterBidOffer = (decimal)0;
        //                //if (saudaOrderContext.so.Incoterms2 == (int)DTO.Enums.IncoTerms.ExDepot)
        //                //{
        //                //    counterBidOffer = saudaOrderContext.p.ExDepotPrice + saudaOrderContext.p.BpCpJumb;
        //                //}
        //                //else if (saudaOrderContext.so.Incoterms2 == (int)DTO.Enums.IncoTerms.ExPlant)
        //                //{
        //                //    counterBidOffer = saudaOrderContext.p.ExPlantPrice + saudaOrderContext.p.BpCpJumb;
        //                //}
        //                //else if (saudaOrderContext.so.Incoterms2 == (int)DTO.Enums.IncoTerms.ForDepot)
        //                //{
        //                //    counterBidOffer = saudaOrderContext.p.ForDepotPrice + saudaOrderContext.p.BpCpJumb;
        //                //}
        //                //else if (saudaOrderContext.so.Incoterms2 == (int)DTO.Enums.IncoTerms.ForPlant)
        //                //{
        //                //    counterBidOffer = saudaOrderContext.p.ForPlantPrice + saudaOrderContext.p.BpCpJumb;
        //                //}
        //                //else if (saudaOrderContext.so.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake)
        //                //{
        //                //    counterBidOffer = saudaOrderContext.p.ExRakePrice + saudaOrderContext.p.BpCpJumb;
        //                //}
        //                //else if (saudaOrderContext.so.Incoterms2 == (int)DTO.Enums.IncoTerms.ForRake)
        //                //{
        //                //    counterBidOffer = saudaOrderContext.p.ForRakePrice + saudaOrderContext.p.BpCpJumb;
        //                //}

        //                var soContext = _emamiContext.SaudaOrders.FirstOrDefault(_ => _.Id == saudaOrderContext.so.Id);
        //                soContext.CounterBidOffer = counterBidOffer;
        //                soContext.CounterBidOfferDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //                soContext.ModifiedBy = inputDto.LoginUserId;
        //                soContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //                _emamiContext.SaveChanges();

        //                var smsPlainTemplate = string.Empty;
        //                var smsMessage = string.Empty;
        //                var smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.CounterBidOfferNotificationSMS);
        //                if (smsTemplate != null)
        //                {
        //                    smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.so.Sku.SkuName)
        //                        .Replace(Constants.CounterBidOfferPrice, Math.Round(counterBidOffer / saudaOrderContext.so.BidQuantityCase, 2).ToString());
        //                    smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
        //                }

        //                var notificationContext = new Notifications
        //                {
        //                    Request = Constants.NotificationCounterBid,
        //                    RequestId = (int)DTO.Enums.NotificationRequest.CounterBid,
        //                    ReferenceId = saudaOrderContext.so.Id,
        //                    Notification = smsMessage,
        //                    StatusId = saudaOrderContext.so.StatusId,
        //                    CreatedBy = saudaOrderContext.so.CreatedBy,
        //                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        //                };
        //                _emamiContext.Notifications.Add(notificationContext);
        //                _emamiContext.SaveChanges();
        //                try
        //                {
        //                    List<User> usersContext = new List<User>();
        //                    List<string> toUsers = new List<string>();
        //                    User createdBy = new User();
        //                    User dealer = new User();
        //                    if (saudaOrderContext.so.CreatedBy == saudaOrderContext.s.UserId)
        //                    {
        //                        createdBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.so.CreatedBy);
        //                        if (createdBy != null)
        //                        {
        //                            toUsers.Add(createdBy.Email);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == saudaOrderContext.so.CreatedBy || _.Id == saudaOrderContext.s.UserId).ToList();
        //                        if (usersContext != null && usersContext.Any() && saudaOrderContext != null)
        //                        {
        //                            createdBy = usersContext.FirstOrDefault(_ => _.Id == saudaOrderContext.so.CreatedBy);
        //                            dealer = usersContext.FirstOrDefault(_ => _.Id == saudaOrderContext.s.UserId);
        //                            if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
        //                            {
        //                                toUsers.Add(createdBy.Email);
        //                            }
        //                            if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
        //                            {
        //                                toUsers.Add(dealer.Email);
        //                            }
        //                        }
        //                    }

        //                    if (((usersContext != null && usersContext.Any()) || createdBy != null) && saudaOrderContext != null)
        //                    {
        //                        var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.CounterBidOfferNotificationEmail);
        //                        var emailSubject = Constants.CounterBidOffer;
        //                        var fromEmail = Constants.FromEmail;
        //                        var plainText = string.Empty;
        //                        if (_resultService.IsEmail() && toUsers != null && toUsers.Any())
        //                        {
        //                            if (emailTemplate != null)
        //                            {
        //                                var encryptedSaudaOrderId = EncryptDecryptHelper.Encrypt(saudaOrderContext.so.Id.ToString(), SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);
        //                                var counterBidWebsiteUrl = Config.WebSiteUrl + Constants.CounterBidWebsiteUrl + encryptedSaudaOrderId;
        //                                var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.so.Sku?.SkuName)
        //                                    .Replace(Constants.CounterBidOfferPrice, Math.Round(counterBidOffer / saudaOrderContext.so.BidQuantityCase, 2).ToString())
        //                                    .Replace(Constants.URL, counterBidWebsiteUrl);
        //                                var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
        //                                amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
        //                            }
        //                        }

        //                        if (_resultService.IsSMS())
        //                        {
        //                            if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber) && !string.IsNullOrEmpty(smsMessage))
        //                            {
        //                                //amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber);
        //                                var dto = new CounterBidNotificationSku
        //                                {
        //                                    UserId = saudaOrderContext.so.CreatedBy,
        //                                    SkuId = saudaOrderContext.so.Sku.Id,
        //                                    Sku = saudaOrderContext.so.Sku?.SkuName,
        //                                    counterBidOffer = Math.Round(counterBidOffer / saudaOrderContext.so.BidQuantityCase, 2),
        //                                    MobileNumber = createdBy.MobileNumber
        //                                };
        //                                notificationSku.Add(dto);
        //                            }
        //                            if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber) && !string.IsNullOrEmpty(smsMessage))
        //                            {
        //                                //amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber);
        //                                var dto = new CounterBidNotificationSku
        //                                {
        //                                    UserId = dealer.Id,
        //                                    SkuId = saudaOrderContext.so.Sku.Id,
        //                                    Sku = saudaOrderContext.so.Sku?.SkuName,
        //                                    counterBidOffer = Math.Round(counterBidOffer / saudaOrderContext.so.BidQuantityCase, 2),
        //                                    MobileNumber = createdBy.MobileNumber
        //                                };
        //                                notificationSku.Add(dto);
        //                            }
        //                        }
        //                        if (_resultService.IsPushNotification())
        //                        {
        //                            if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
        //                            {
        //                                PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                                {
        //                                    PushTokenKey = createdBy.PushTokenKey,
        //                                    RegistrationTypeId = (int)createdBy.RegistrationTypeId,
        //                                    Title = Constants.CounterBidOfferSubject,
        //                                    Message = smsPlainTemplate,
        //                                    Id = Convert.ToString(saudaOrderContext.so.Id),
        //                                };
        //                                _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                            }
        //                            if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
        //                            {
        //                                PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                                {
        //                                    PushTokenKey = dealer.PushTokenKey,
        //                                    RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
        //                                    Title = Constants.CounterBidOfferSubject,
        //                                    Message = smsPlainTemplate,
        //                                    Id = Convert.ToString(saudaOrderContext.so.Id),
        //                                };
        //                                _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                            }
        //                        }
        //                    }
        //                    else
        //                    {
        //                        return _resultService.ErrorMessage(Constants.UserNotFound);
        //                    }
        //                }
        //                catch (Exception ex)
        //                {

        //                }
        //            }
        //            if (notificationSku != null && notificationSku.Any())
        //            {
        //                var DistinctUser = notificationSku.Select(row => new
        //                {
        //                    UserId = row.UserId,
        //                    MobileNumber = row.MobileNumber
        //                }).Distinct().ToList();

        //                if (DistinctUser != null && DistinctUser.Any())
        //                {
        //                    foreach (var item in DistinctUser)
        //                    {
        //                        var SkubyUser = notificationSku.Where(_ => _.UserId == item.UserId).ToList();
        //                        string skuConcat = string.Empty;
        //                        foreach (var sku in SkubyUser)
        //                        {
        //                            skuConcat = skuConcat + sku.Sku + "@" + sku.counterBidOffer + ";";
        //                        }
        //                        string smsMessage = string.Empty;
        //                        smsMessage = "Following booking, you have placed today are in counter, with the counter rates " + skuConcat;
        //                        amazonNotificationService.SendMessage(smsMessage, item.MobileNumber);
        //                    }
        //                }
        //            }
        //            SendServiceNotification("Counter Bid Notification");
        //            return _resultService.SuccessMessage(Constants.CounterBidUpdateSuccess);
        //        }
        //        else
        //        {
        //            SendServiceNotification("Counter Bid Notification");
        //            return _resultService.ErrorMessage(Constants.RecordNotFound);
        //        }

        //    }
        //    catch (Exception exception)
        //    {
        //        SendServiceNotification("Counter Bid Notification");
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        //public ResultDto RejectSaudaOrdersInHold(LoginUserIdDto inputDto)
        //{
        //    _methodName = "RejectSaudaOrdersInHold";
        //    var resultDto = new ResultDto();
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }
        //        if (inputDto.LoginUserId == 0)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidUser);
        //        }
        //        var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
        //        if (userContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserNotFound);
        //        }

        //        var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);

        //        var saudaOrderListContext = _emamiContext.SaudaOrders.Where(_ => _.StatusId == (int)DTO.Enums.Status.Hold && _.CounterBidOffer != 0 && _.Sauda != null
        //            && DbFunctions.TruncateTime(_.Sauda.BiddingDate) == DbFunctions.TruncateTime(currentDate)).ToList();
        //        if (saudaOrderListContext != null && saudaOrderListContext.Any())
        //        {
        //            foreach (var saudaOrderContext in saudaOrderListContext)
        //            {
        //                saudaOrderContext.StatusId = (int)DTO.Enums.Status.Rejected;
        //                saudaOrderContext.ModifiedBy = inputDto.LoginUserId;
        //                saudaOrderContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

        //                try
        //                {
        //                    List<User> usersContext = new List<User>();
        //                    List<string> toUsers = new List<string>();
        //                    User createdBy = new User();
        //                    User dealer = new User();
        //                    if (saudaOrderContext.CreatedBy == saudaOrderContext.Sauda.UserId)
        //                    {
        //                        createdBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.CreatedBy);
        //                        if (createdBy != null)
        //                        {
        //                            toUsers.Add(createdBy.Email);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == saudaOrderContext.CreatedBy || _.Id == saudaOrderContext.Sauda.UserId).ToList();
        //                        if (usersContext != null && usersContext.Any())
        //                        {
        //                            createdBy = usersContext.FirstOrDefault(_ => _.Id == saudaOrderContext.CreatedBy);
        //                            dealer = usersContext.FirstOrDefault(_ => _.Id == saudaOrderContext.Sauda.UserId);
        //                            if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
        //                            {
        //                                toUsers.Add(createdBy.Email);
        //                            }
        //                            if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
        //                            {
        //                                toUsers.Add(dealer.Email);
        //                            }
        //                        }
        //                    }
        //                    if (((usersContext != null && usersContext.Any()) || createdBy != null) && saudaOrderContext != null)
        //                    {
        //                        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
        //                        var emailSubject = string.Empty;
        //                        if (_resultService.IsEmail() && toUsers != null && toUsers.Any())
        //                        {
        //                            var fromEmail = Constants.FromEmail;

        //                            var plainText = string.Empty;
        //                            EmailTemplate emailTemplate = new EmailTemplate();

        //                            emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderRejectNotificationEmail);
        //                            emailSubject = Constants.SaudaRejectedSubject;

        //                            if (emailTemplate != null)
        //                            {
        //                                var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
        //                                    .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(saudaOrderContext.BidPrice, 2)).ToString());
        //                                var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
        //                                amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
        //                            }
        //                        }
        //                        var smsPlainTemplate = string.Empty;
        //                        if (_resultService.IsSMS())
        //                        {
        //                            var smsMessage = string.Empty;
        //                            EmailTemplate smsTemplate = new EmailTemplate();
        //                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderRejectNotificationSMS);

        //                            if (smsTemplate != null)
        //                            {
        //                                smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
        //                                    .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(saudaOrderContext.BidPrice, 2)).ToString());
        //                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
        //                            }
        //                            if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
        //                            {
        //                                amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber);
        //                            }
        //                            if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
        //                            {
        //                                amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber);
        //                            }
        //                        }
        //                        if (_resultService.IsPushNotification())
        //                        {
        //                            if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
        //                            {
        //                                PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                                {
        //                                    PushTokenKey = createdBy.PushTokenKey,
        //                                    RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
        //                                    Title = emailSubject,
        //                                    Message = smsPlainTemplate,
        //                                    //Id = saudaOrderContext.Id,
        //                                };
        //                                _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                            }
        //                            if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
        //                            {
        //                                PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                                {
        //                                    PushTokenKey = dealer.PushTokenKey,
        //                                    RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
        //                                    Title = emailSubject,
        //                                    Message = smsPlainTemplate,
        //                                    //Id = saudaOrderContext.Id,
        //                                };
        //                                _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                            }
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {

        //                }
        //            }
        //            _emamiContext.SaveChanges();

        //            SendServiceNotification("Hold sauda moved to rejected status");
        //            return _resultService.SuccessMessage(Constants.SaudaOrderReject);
        //        }
        //        else
        //        {
        //            SendServiceNotification("Hold sauda moved to rejected status");
        //            return _resultService.ErrorMessage(Constants.RecordNotFound);
        //        }

        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        SendServiceNotification("Hold sauda moved to rejected status");
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        //public ResultDto SendLatestSaudasStatusNotification(LoginUserIdDto inputDto)
        //{
        //    _methodName = "SendLatestSaudasStatusNotification";
        //    var resultDto = new ResultDto();
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }
        //        if (inputDto.LoginUserId == 0)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidUser);
        //        }
        //        var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
        //        if (userContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserNotFound);
        //        }

        //        var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //        var currentTime = new TimeSpan(currentDate.Hour, currentDate.Minute, currentDate.Second);
        //        var biddingWindowContext = _emamiContext.BiddingWindowTiming.AsNoTracking().Where(_ => _.ToHours < currentTime
        //            && DbFunctions.TruncateTime(_.BiddingDate) == DbFunctions.TruncateTime(currentDate)).OrderByDescending(_ => _.ToHours).FirstOrDefault();
        //        if (biddingWindowContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.RecordNotFound);
        //        }

        //        var saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && DbFunctions.TruncateTime(_.Sauda.BiddingDate) == DbFunctions.TruncateTime(currentDate)
        //                && _.BiddingwindowId == biddingWindowContext.Id && _.StatusId != (int)DTO.Enums.Status.Approved).ToList();
        //        if (saudaOrderListContext != null && saudaOrderListContext.Any())
        //        {
        //            foreach (var saudaOrderContext in saudaOrderListContext)
        //            {
        //                List<User> usersContext = new List<User>();
        //                List<string> toUsers = new List<string>();
        //                User createdBy = new User();
        //                User dealer = new User();
        //                if (saudaOrderContext.CreatedBy == saudaOrderContext.Sauda.UserId)
        //                {
        //                    createdBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.CreatedBy);
        //                    if (createdBy != null)
        //                    {
        //                        toUsers.Add(createdBy.Email);
        //                    }
        //                }
        //                else
        //                {
        //                    usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == saudaOrderContext.CreatedBy || _.Id == saudaOrderContext.Sauda.UserId).ToList();
        //                    if (usersContext != null && usersContext.Any() && saudaOrderContext != null)
        //                    {
        //                        createdBy = usersContext.FirstOrDefault(_ => _.Id == saudaOrderContext.CreatedBy);
        //                        dealer = usersContext.FirstOrDefault(_ => _.Id == saudaOrderContext.Sauda.UserId);
        //                        if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
        //                        {
        //                            toUsers.Add(createdBy.Email);
        //                        }
        //                        if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
        //                        {
        //                            toUsers.Add(dealer.Email);
        //                        }
        //                    }
        //                }
        //                if (((usersContext != null && usersContext.Any()) || createdBy != null) && saudaOrderContext != null)
        //                {
        //                    AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
        //                    var emailSubject = string.Empty;
        //                    if (_resultService.IsEmail() && toUsers != null && toUsers.Any())
        //                    {
        //                        var fromEmail = Constants.FromEmail;
        //                        var plainText = string.Empty;
        //                        EmailTemplate emailTemplate = new EmailTemplate();
        //                        if (saudaOrderContext.StatusId == (int)DTO.Enums.Status.Pending)
        //                        {
        //                            emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderPendingNotificationEmail);
        //                            emailSubject = Constants.SaudaBookedSubject;
        //                        }
        //                        else if (saudaOrderContext.StatusId == (int)DTO.Enums.Status.Hold)
        //                        {
        //                            emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderHoldNotificationEmail);
        //                            emailSubject = Constants.SaudaOnHoldSubject;
        //                        }
        //                        else if (saudaOrderContext.StatusId == (int)DTO.Enums.Status.Rejected)
        //                        {
        //                            emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderRejectNotificationEmail);
        //                            emailSubject = Constants.SaudaRejectedSubject;
        //                        }
        //                        if (emailTemplate != null)
        //                        {
        //                            var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
        //                                .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(saudaOrderContext.BidPrice, 2)).ToString());
        //                            var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
        //                            amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
        //                        }
        //                    }
        //                    var smsPlainTemplate = string.Empty;
        //                    if (_resultService.IsSMS())
        //                    {
        //                        var smsMessage = string.Empty;
        //                        EmailTemplate smsTemplate = new EmailTemplate();
        //                        if (saudaOrderContext.StatusId == (int)DTO.Enums.Status.Pending)
        //                        {
        //                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderPendingNotificationSMS);
        //                        }
        //                        else if (saudaOrderContext.StatusId == (int)DTO.Enums.Status.Hold)
        //                        {
        //                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderHoldNotificationSMS);
        //                        }
        //                        else if (saudaOrderContext.StatusId == (int)DTO.Enums.Status.Rejected)
        //                        {
        //                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderRejectNotificationSMS);
        //                        }
        //                        if (smsTemplate != null)
        //                        {
        //                            smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
        //                                .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(saudaOrderContext.BidPrice, 2)).ToString());
        //                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
        //                        }
        //                        if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
        //                        {
        //                            amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber);
        //                        }
        //                        if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
        //                        {
        //                            amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber);
        //                        }
        //                    }
        //                    if (_resultService.IsPushNotification())
        //                    {
        //                        if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
        //                        {
        //                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                            {
        //                                PushTokenKey = createdBy.PushTokenKey,
        //                                RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
        //                                Title = emailSubject,
        //                                Message = smsPlainTemplate,
        //                                //Id = saudaOrderContext.Id,
        //                            };
        //                            _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                        }
        //                        if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
        //                        {
        //                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                            {
        //                                PushTokenKey = dealer.PushTokenKey,
        //                                RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
        //                                Title = emailSubject,
        //                                Message = smsPlainTemplate,
        //                                //Id = saudaOrderContext.Id,
        //                            };
        //                            _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                        }
        //                    }
        //                }

        //                else
        //                {
        //                    return _resultService.ErrorMessage(Constants.UserNotFound);
        //                }
        //            }
        //            SendServiceNotification("RA Booking Status");
        //            return _resultService.SuccessMessage(Constants.NotificationSuccess);
        //        }
        //        else
        //        {
        //            SendServiceNotification("RA Booking Status");
        //            return _resultService.ErrorMessage(Constants.RecordNotFound);
        //        }

        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        SendServiceNotification("RA Booking Status");
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        public void SendServiceNotification(string methodName)
        {
            try
            {
                var amazonNotificationService = new AmazonNotificationService();
                if (!string.IsNullOrEmpty(Constants.ServiceNotificationEmailIds))
                {
                    var emailIdsList = Constants.ServiceNotificationEmailIds.Split(',');
                    var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.ServiceNotificationEmail);
                    if (emailTemplate != null)
                    {
                        var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.MethodName, methodName).ToString();
                        plainTemplate = plainTemplate.Replace("##date##", DateHelper.UtcToIndia(DateTime.UtcNow).ToLongDateString() + " at " + String.Format("{0:hh:mm tt}", DateTime.Now));
                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                        amazonNotificationService.SendEmail(emailIdsList.ToList(), Constants.ServiceNotificationSubject, string.Empty, htmlTemplate, true);
                    }
                }

                if (!string.IsNullOrEmpty(Constants.ServiceNotificationMobileNumbers))
                {
                    var mobilenumbersList = Constants.ServiceNotificationMobileNumbers.Split(',');
                    var smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.ServiceNotificationSMS);
                    var smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.MethodName, methodName).ToString();
                    foreach (var mobileNumber in mobilenumbersList)
                    {
                        if (smsTemplate != null && !string.IsNullOrEmpty(mobileNumber))
                        {
                            smsPlainTemplate = smsPlainTemplate.Replace("##date##", DateHelper.UtcToIndia(DateTime.UtcNow).ToLongDateString() + " at " + String.Format("{0:hh:mm tt}", DateTime.Now));
                            amazonNotificationService.SendMessage(smsPlainTemplate, mobileNumber);
                        }
                    }
                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
        }

        public void InsertReason(Remarks remarks)
        {
            try
            {
                _emamiContext.Remarks.Add(remarks);
                // _emamiContext.SaveChanges();
            }
            catch (Exception)
            {
            }
        }



        #region Special Rate Approval

        public ResultDto GetSpecialRateApprovalListWithAccessPermission(SpecialRateAddInputDto inputDto)
        {
            _methodName = "GetSpecialRateApprovalListWithAccessPermission";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");

            var outputDto = new List<SpecialRateApprovalOutputDto>();
            try
            {
                if (inputDto == null)
                {
                    return NotFoundResult();
                }
                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }
                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }
                if (inputDto.FromDate > inputDto.ToDate)
                {
                    return _resultService.ErrorMessage(Constants.FromDateInvalid);
                }
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }

                var specialRateApproval = _emamiContext.SpecialRateApproval.AsNoTracking().Where(_ => _.RequestedTo == inputDto.LoginUserId || _.CreatedBy == inputDto.LoginUserId);
                List<long> specialRateIds = specialRateApproval.Select(_ => _.SpecialRateId).Distinct().ToList();

                var specialRateQueryable = _emamiContext.SpecialRate.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && specialRateIds.Contains(_.Id) &&
                    (inputDto.VerticalId > 0 ? _.OilType.DivisionId == inputDto.VerticalId : _.OilType.DivisionId > 0)).AsQueryable();

                if (inputDto.StatusId > 0)
                {
                    specialRateQueryable = specialRateQueryable.Where(_ => _.StatusId == inputDto.StatusId);
                }

                outputDto = specialRateQueryable.ToList().Select(c => new SpecialRateApprovalOutputDto
                {
                    Id = c.Id,
                    FinalPrice = c.FinalPrice,
                    OilTypeId = c.OilTypeId,
                    Quantity = c.Quantity,
                    SpecialPrice = c.SpecialPrice,
                    SkuName = c.Sku?.SkuName,
                    SkuCode = c.Sku?.SkuCode,
                    CreatedDate = c.CreatedDate,
                    StatusId = (int)c.StatusId,
                    DealerName = c.User.Name,
                    Remarks = c.Remarks,
                    //FreightRoute = c.FreightRoute != null ? c.FreightRoute.Name : string.Empty,
                    IncoTerms = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == c.Incoterms2)?.Name,
                    OilTypeName = c.OilType?.Name,
                    Status = c.Status?.Name,
                    CreatedById = c.CreatedBy,
                    IsLTD = c.IsLTD,
                    LTD_SR = c.IsLTD == true ? UtilityHelper.GetEnumDescription(DTO.Enums.LTDSR.LTD) : UtilityHelper.GetEnumDescription(DTO.Enums.LTDSR.SpecialRate)
                }).ToList();


                foreach (var item in outputDto)
                {
                    var specialRatedata = _emamiContext.SpecialRateApproval.AsNoTracking().Where(_ => _.SpecialRateId == item.Id).ToList();
                    if (specialRatedata != null && specialRatedata.Any())
                    {
                        var specialrateDetail = specialRatedata.OrderByDescending(_ => _.Id).FirstOrDefault();
                        if (specialrateDetail != null)
                        {
                            var requestTo = specialrateDetail.RequestedTo;
                            if (requestTo == inputDto.LoginUserId)
                            {
                                item.HasAccessToProceed = true;
                                item.ApprovalsCount = specialRatedata.Count();
                                //item.RequestedBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == specialRatedata.FirstOrDefault().RequestedBy).Name;
                            }
                            item.CreatedBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == item.CreatedById)?.Name;
                            item.ApprovedBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == specialrateDetail.ApprovedBy)?.Name;
                            item.RequestedBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == specialrateDetail.RequestedBy)?.Name;
                            item.RequestedTo = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == specialrateDetail.RequestedTo)?.Name;
                        }
                    }
                }

                return SucessResult(outputDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto SpecialRateApproval(SpecialRateApprovalDto inputDto)
        {
            _methodName = "SpecialRateApproval";
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (!inputDto.SpecialRateIds.Any())
                {
                    return _resultService.ErrorMessage(Constants.SpecialRateRequestMissing);
                }
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                foreach (var specialRateId in inputDto.SpecialRateIds)
                {
                    var result = _emamiContext.SpecialRate.FirstOrDefault(_ => _.Id == specialRateId);

                    if (result != null && (result.StatusId == (int)DTO.Enums.Status.Pending || result.StatusId == (int)DTO.Enums.Status.RequestForApproval))
                    {
                        if (specialRateId > 0)
                        {
                            if (inputDto.StatusId == (int)DTO.Enums.Status.Approved || inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                            {
                                inputDto.ApprovedBy = inputDto.LoginUserId;
                                inputDto.RequestedTo = 0;
                            }
                            else
                            {
                                var users = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == inputDto.LoginUserId);
                                if (users != null && users.Any())
                                {
                                    inputDto.RequestedTo = (long)users.FirstOrDefault().ReportingToId;
                                }
                            }

                            var input = new SpecialRateApproval
                            {
                                SpecialRateId = specialRateId,
                                RequestedBy = inputDto.LoginUserId,
                                RequestedTo = inputDto.RequestedTo,
                                ApprovedBy = inputDto.ApprovedBy,
                                StatusId = inputDto.StatusId,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            };
                            _emamiContext.SpecialRateApproval.Add(input);
                            _emamiContext.SaveChanges();

                            result.StatusId = inputDto.StatusId;
                            result.Remarks = inputDto.Remarks;
                            result.ModifiedBy = inputDto.LoginUserId;
                            result.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                            _emamiContext.SaveChanges();

                            #region Notification

                            try
                            {
                                List<User> usersContext = new List<User>();
                                User createdBy = new User(); createdBy = null;
                                User dealer = new User(); dealer = null;
                                if (result.CreatedBy == result.UserId)
                                {
                                    createdBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == result.UserId);
                                }
                                else
                                {
                                    usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == result.CreatedBy || _.Id == result.UserId).ToList();
                                    if (usersContext != null && usersContext.Any())
                                    {
                                        createdBy = usersContext.FirstOrDefault(_ => _.Id == result.CreatedBy);
                                        dealer = usersContext.FirstOrDefault(_ => _.Id == result.UserId);
                                    }
                                }

                                if ((usersContext != null && usersContext.Any()) || createdBy != null)
                                {
                                    bool isEmail = false;
                                    var DealerNotificationContext = _emamiContext.TPNotification.AsNoTracking().
                                                                    Join(_emamiContext.TPNotificationDetails.AsNoTracking(), TPN => TPN.Id, TPND => TPND.TPNotificationId, (TPN, TPND) => new { TPN, TPND })
                                                                    .Where(_ => _.TPND.DealerId == result.UserId && _.TPND.NotificationActionId == (int)DTO.Enums.NotificationActionTP.SpecialRateApproval && _.TPND.IsActive).ToList();

                                    var DealerEmailNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.Email);
                                    if (DealerEmailNotificationEnabled != null && DealerEmailNotificationEnabled.Any())
                                        isEmail = true;
                                    else
                                        isEmail = false;
                                    AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                    var emailSubject = string.Empty;
                                    if (isEmail)
                                    {
                                        var fromEmail = Constants.FromEmail;
                                        var plainText = string.Empty;
                                        EmailTemplate emailTemplate = new EmailTemplate();
                                        if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                        {
                                            emailSubject = Constants.SpecialRateApprovalSubject;
                                            emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecialRateApprovalEmail);
                                        }
                                        else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                                        {
                                            emailSubject = Constants.SpecialRateRejectSubject;
                                            emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecialRateRejectEmail);
                                        }
                                        if (!string.IsNullOrEmpty(emailTemplate.PlainTemplate) && !string.IsNullOrEmpty(emailTemplate.Template))
                                        {
                                            if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email) && dealer != null && !string.IsNullOrEmpty(dealer.Email))
                                            {
                                                List<string> toUsers = new List<string>();
                                                toUsers.Add(createdBy.Email);
                                                var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.UserName, createdBy.Name).Replace(Constants.CustomerName, dealer.Name)
                                                    .Replace(Constants.SkuName, result.Sku != null ? result.Sku.SkuName : "")
                                                    .Replace(Constants.Quantity, (Math.Round(result.QuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(result.SpecialPrice, 2)).ToString());
                                                var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                                amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                                toUsers = new List<string>();
                                                toUsers.Add(dealer.Email);
                                                plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.UserName, dealer.Name).Replace(Constants.CustomerName, dealer.Name)
                                                    .Replace(Constants.SkuName, result.Sku != null ? result.Sku.SkuName : "")
                                                    .Replace(Constants.Quantity, (Math.Round(result.QuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(result.SpecialPrice, 2)).ToString());
                                                htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                                amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                            }
                                            if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email) && dealer == null)
                                            {
                                                List<string> toUsers = new List<string>();
                                                toUsers.Add(createdBy.Email);
                                                var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.UserName, createdBy.Name).Replace(Constants.CustomerName, createdBy.Name)
                                                    .Replace(Constants.SkuName, result.Sku != null ? result.Sku.SkuName : "")
                                                    .Replace(Constants.Quantity, (Math.Round(result.QuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(result.SpecialPrice, 2)).ToString());
                                                var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                                amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                            }
                                        }
                                    }
                                    var smsPlainTemplateCreatedBy = string.Empty;
                                    var smsPlainTemplateDealer = string.Empty;
                                    bool isSms = false;
                                    var DealerSMSNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.SMS);
                                    if (DealerSMSNotificationEnabled != null && DealerSMSNotificationEnabled.Any())
                                        isSms = true;
                                    else
                                        isSms = false;
                                    if (isSms)
                                    {
                                        var smsMessage = string.Empty;
                                        EmailTemplate smsTemplate = new EmailTemplate();
                                        if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                        {
                                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecialRateApprovalSMS);
                                        }
                                        else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                                        {
                                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecialRateRejectSMS);
                                        }
                                        if (smsTemplate != null)
                                        {
                                            if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber) && dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                            {
                                                smsPlainTemplateCreatedBy = smsTemplate.PlainTemplate.Replace(Constants.UserName, createdBy.Name).Replace(Constants.CustomerName, dealer.Name)
                                                    .Replace(Constants.SkuName, result.Sku != null ? result.Sku.SkuName : "")
                                                    .Replace(Constants.Quantity, (Math.Round(result.QuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(result.SpecialPrice, 2)).ToString());
                                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplateCreatedBy);
                                                amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber, smsTemplate.SMSTemplateID);

                                                smsPlainTemplateDealer = smsTemplate.PlainTemplate.Replace(Constants.UserName, dealer.Name).Replace(Constants.CustomerName, dealer.Name)
                                                    .Replace(Constants.SkuName, result.Sku != null ? result.Sku.SkuName : "")
                                                    .Replace(Constants.Quantity, (Math.Round(result.QuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(result.SpecialPrice, 2)).ToString());
                                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplateDealer);
                                                amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber, smsTemplate.SMSTemplateID);
                                            }
                                            if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber) && dealer == null)
                                            {
                                                smsPlainTemplateCreatedBy = smsTemplate.PlainTemplate.Replace(Constants.UserName, createdBy.Name).Replace(Constants.CustomerName, createdBy.Name)
                                                    .Replace(Constants.SkuName, result.Sku != null ? result.Sku.SkuName : "")
                                                    .Replace(Constants.Quantity, (Math.Round(result.QuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(result.SpecialPrice, 2)).ToString());
                                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplateCreatedBy);
                                                amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber, smsTemplate.SMSTemplateID);
                                            }

                                        }
                                    }
                                    bool IsPushNotification = false;
                                    var DealerPushNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.InAppNotification);
                                    if (DealerPushNotificationEnabled != null && DealerPushNotificationEnabled.Any())
                                        IsPushNotification = true;
                                    else
                                        IsPushNotification = false;
                                    if (IsPushNotification)
                                    {
                                        if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
                                        {
                                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                            {
                                                PushTokenKey = createdBy.PushTokenKey,
                                                RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
                                                Title = emailSubject,
                                                Message = smsPlainTemplateCreatedBy,
                                                //Id = result.Id,
                                            };
                                            _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                        }
                                        if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
                                        {
                                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                            {
                                                PushTokenKey = dealer.PushTokenKey,
                                                RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
                                                Title = emailSubject,
                                                Message = smsPlainTemplateDealer,
                                                //Id = result.Id,
                                            };
                                            _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }

                            #endregion
                        }
                    }
                    else
                    {
                        return _resultService.ErrorMessage(Constants.SpecialRateStatusAlreadyUpdated);
                    }
                }
                return _resultService.SuccessMessage(Constants.SpecialRateStatusUpdated);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion

        #region Sauda Convertion

        public ResultDto GetSaudaConversionList(SaudaConvertionFilterDto inputDto)
        {
            _methodName = "GetSaudaConversionList";
            var resultDto = new ResultDto();
            try
            {

                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.FromDateEmpty;
                    return resultDto;
                }
                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.ToDateEmpty;
                    return resultDto;
                }

                var saudaConvertionContextList = new List<SaudaConversion>();

                if (inputDto.StatusId > 0)
                {
                    saudaConvertionContextList = _emamiContext.SaudaConversion.AsNoTracking()
                        .Join(_emamiContext.SaudaOrders.AsNoTracking(), sc => sc.SaudaOrderId, so => so.Id, (sc, so) => new { SaudaConversion = sc, SaudaOrders = so })
                    .Where(w => DbFunctions.TruncateTime(w.SaudaConversion.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    && DbFunctions.TruncateTime(w.SaudaConversion.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    && w.SaudaConversion.StatusId == inputDto.StatusId && w.SaudaConversion.IsConversion
                    && (inputDto.VerticalId > 0 ? w.SaudaOrders.OilType.DivisionId == inputDto.VerticalId : w.SaudaOrders.OilType.DivisionId > 0))
                    .Select(s => s.SaudaConversion).ToList();
                }
                else
                {
                    saudaConvertionContextList = _emamiContext.SaudaConversion.AsNoTracking()
                        .Join(_emamiContext.SaudaOrders.AsNoTracking(), sc => sc.SaudaOrderId, so => so.Id, (sc, so) => new { SaudaConversion = sc, SaudaOrders = so })
                    .Where(w => DbFunctions.TruncateTime(w.SaudaConversion.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    && DbFunctions.TruncateTime(w.SaudaConversion.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    && w.SaudaConversion.IsConversion
                    && (inputDto.VerticalId > 0 ? w.SaudaOrders.OilType.DivisionId == inputDto.VerticalId : w.SaudaOrders.OilType.DivisionId > 0))
                    .Select(s => s.SaudaConversion).ToList();
                }

                var saudaConvertionList = saudaConvertionContextList
                      .OrderByDescending(o => o.CreatedDate)
                      .Select(s => new SaudaConversionListDto()
                      {
                          Id = s.Id,
                          DealerId = s.DealerId,
                          DealerName = _emamiContext.Users.AsNoTracking().ToList().FirstOrDefault(f => f.Id == s.DealerId).Name,
                          ExpiryDate = s.ExpiryDate,
                          ExtendToDate = s.ExtendToDate,
                          StatusId = s.StatusId,
                          StatusName = s.Status.Name,
                          SaudaId = s.SaudaOrderId,
                          IsConversion = s.IsConversion,
                          //IsExtension = s.IsExtension
                      }).ToList();
                return SucessResult(saudaConvertionList);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto GetSaudaConversionListForExport(SaudaConvertionFilterDto inputDto)
        {
            _methodName = "GetSaudaConversionListForExport";
            var resultDto = new ResultDto();
            try
            {

                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.FromDateEmpty;
                    return resultDto;
                }
                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.ToDateEmpty;
                    return resultDto;
                }

                var saudaConvertionContextList = new List<SaudaConversion>();

                if (inputDto.StatusId > 0)
                {
                    //saudaConvertionContextList = _emamiContext.SaudaConversion.AsNoTracking()
                    //.Where(w => DbFunctions.TruncateTime(w.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                    //DbFunctions.TruncateTime(w.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && w.StatusId == inputDto.StatusId && w.IsConversion).ToList();
                    saudaConvertionContextList = _emamiContext.SaudaConversion.AsNoTracking()
                       .Join(_emamiContext.SaudaOrders.AsNoTracking(), sc => sc.SaudaOrderId, so => so.Id, (sc, so) => new { SaudaConversion = sc, SaudaOrders = so })
                   .Where(w => DbFunctions.TruncateTime(w.SaudaConversion.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                   && DbFunctions.TruncateTime(w.SaudaConversion.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                   && w.SaudaConversion.StatusId == inputDto.StatusId && w.SaudaConversion.IsConversion
                   && (inputDto.VerticalId > 0 ? w.SaudaOrders.OilType.DivisionId == inputDto.VerticalId : w.SaudaOrders.OilType.DivisionId > 0))
                   .Select(s => s.SaudaConversion).ToList();
                }
                else
                {
                    //saudaConvertionContextList = _emamiContext.SaudaConversion.AsNoTracking()
                    //.Where(w => DbFunctions.TruncateTime(w.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                    //DbFunctions.TruncateTime(w.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && w.IsConversion).ToList();
                    saudaConvertionContextList = _emamiContext.SaudaConversion.AsNoTracking()
                        .Join(_emamiContext.SaudaOrders.AsNoTracking(), sc => sc.SaudaOrderId, so => so.Id, (sc, so) => new { SaudaConversion = sc, SaudaOrders = so })
                    .Where(w => DbFunctions.TruncateTime(w.SaudaConversion.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    && DbFunctions.TruncateTime(w.SaudaConversion.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    && w.SaudaConversion.IsConversion
                    && (inputDto.VerticalId > 0 ? w.SaudaOrders.OilType.DivisionId == inputDto.VerticalId : w.SaudaOrders.OilType.DivisionId > 0))
                    .Select(s => s.SaudaConversion).ToList();
                }

                var saudaConvertionList = saudaConvertionContextList
                      .OrderByDescending(o => o.CreatedDate)
                      .Select(s => new SaudaConversionListDto()
                      {
                          Id = s.Id,
                          DealerId = s.DealerId,
                          DealerName = _emamiContext.Users.AsNoTracking().ToList().FirstOrDefault(f => f.Id == s.DealerId).Name,
                          ExpiryDate = s.ExpiryDate,
                          ExtendToDate = s.ExtendToDate,
                          StatusId = s.StatusId,
                          StatusName = s.Status.Name,
                          SaudaId = s.SaudaOrderId,
                          IsConversion = s.IsConversion
                      }).ToList();

                foreach (var saudaConversion in saudaConvertionList)
                {
                    var saudaConvertionDetailList = _emamiContext.SaudaConversionOrder.AsNoTracking().Where(w => w.SaudaConversionId == saudaConversion.Id)
                    .Select(s => new SaudaOrderDetails()
                    {
                        SkuId = s.SkuId,
                        SkuName = s.Sku.SkuName,
                        QuotedPrice = s.QuotedPrice,
                        BidQuantity = s.BidQuantity,
                        BidQuantityCases = s.BidQuantityCase,
                        BidPrice = s.BidPrice,
                        BidPricePerCase = Math.Round((s.BidPrice != 0 && s.BidQuantityCase != 0 ? (s.BidPrice / s.BidQuantityCase) : 0), 2)
                    }).ToList();
                    saudaConversion.SaudaOrderDetailsList = saudaConvertionDetailList;
                }
                return SucessResult(saudaConvertionList);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto GetSaudaConversionDetails(SaudaConversionDetailInputDto inputDto)
        {
            _methodName = "GetSaudaConversionDetails";
            try
            {
                var saudaConvertionList = _emamiContext.SaudaConversionOrder.AsNoTracking().Where(w => w.SaudaConversionId == inputDto.SaudaConversionId)
                    .Select(s => new SaudaOrderDetails()
                    {
                        SkuId = s.SkuId,
                        SkuName = s.Sku.SkuName,
                        SkuCode = s.Sku.SkuCode,
                        QuotedPrice = s.QuotedPrice,
                        BidQuantity = s.BidQuantity,
                        BidQuantityCases = s.BidQuantityCase,
                        BidPrice = s.BidPrice,
                        BidPricePerCase = Math.Round((s.BidPrice != 0 && s.BidQuantityCase != 0 ? (s.BidPrice / s.BidQuantityCase) : 0), 2)
                    }).ToList();
                return SucessResult(saudaConvertionList);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto GetSaudaExtensionDetailsNew(SaudaConversionDetailInputDto inputDto)
        {
            _methodName = "GetSaudaConversionDetails";
            try
            {
                var saudaConvertion = new List<SaudaOrderDetails>();
                var SaudaConversionContext = _emamiContext.SaudaConversion.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SaudaConversionId);
                var saudaConvertionList = _emamiContext.SaudaOrders.AsNoTracking().Where(w => w.Id == SaudaConversionContext.SaudaOrderId).ToList();
                if (saudaConvertionList != null && saudaConvertionList.Any())
                {
                    foreach (var s in saudaConvertionList)
                    {
                        var saudaContext = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.Id == s.SaudaId);
                        var TotalSkuLiftedContext = (from sauda in _emamiContext.Sauda.AsNoTracking()
                                                     join saudaorder in _emamiContext.SaudaOrders.AsNoTracking() on sauda.Id equals saudaorder.SaudaId
                                                     join lifting in _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking() on saudaorder.Id equals lifting.SaudaOrderId
                                                     where lifting.SaudaOrderId == s.Id && lifting.StatusId != (int)DTO.Enums.Status.Deleted && lifting.StatusId != (int)DTO.Enums.Status.Rejected
                                                     select lifting
                                                    ).ToList();

                        var dto = new SaudaOrderDetails()
                        {
                            SkuId = s.SkuId,
                            SkuName = s.Sku.SkuName,
                            SkuCode = s.Sku.SkuCode,
                            QuotedPrice = s.QuotedPrice,
                            BidQuantity = s.BidQuantity,
                            BidQuantityCases = s.BidQuantityCase,
                            BidPrice = s.BidPrice,
                            BidPricePerCase = Math.Round((s.BidPrice != 0 && s.BidQuantityCase != 0 ? (s.BidPrice / s.BidQuantityCase) : 0), 2),
                            PendingQuantityCases = s.BidQuantityCase - TotalSkuLiftedContext.Sum(_ => _.LiftingQuantityCase),
                            PendingQuantity = s.BidQuantity - TotalSkuLiftedContext.Sum(_ => _.LiftingQuantity)
                        };
                        saudaConvertion.Add(dto);
                    }
                }
                return SucessResult(saudaConvertion);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto GetSaudaConversionAllDetail(SaudaConversionDetailInputDto inputDto)
        {
            _methodName = "GetSaudaConversionAllDetail";
            try
            {
                var result = new SaudaConversionDetailForAdminDto();

                var saudaConversion = new SaudaConversionDetailForAdminDto();
                var saudaConvertionData = _emamiContext.SaudaConversion.AsNoTracking().FirstOrDefault(w => w.Id == inputDto.SaudaConversionId);

                if (saudaConvertionData != null)
                {
                    saudaConversion.SaudaConversionId = saudaConvertionData.Id;
                    saudaConversion.DealerId = saudaConvertionData.DealerId;
                    saudaConversion.DealerName = _emamiContext.Users.AsNoTracking().ToList().FirstOrDefault(f => f.Id == saudaConvertionData.DealerId).Name;
                    saudaConversion.SaudaId = saudaConvertionData.SaudaOrderId;
                    saudaConversion.SaudaNumber = saudaConvertionData.SaudaOrder != null ? saudaConvertionData.SaudaOrder.SaudaNumber.ToString() : string.Empty;
                    saudaConversion.ExpiryDate = saudaConvertionData.ExpiryDate;
                    saudaConversion.ExtendToDate = saudaConvertionData.ExtendToDate;
                    saudaConversion.ConversionDate = saudaConvertionData.CreatedDate;
                    saudaConversion.StatusId = saudaConvertionData.StatusId;
                    saudaConversion.StatusName = saudaConvertionData.Status.Name;
                    saudaConversion.SaudaId = saudaConvertionData.SaudaOrderId;
                    saudaConversion.IsConversion = saudaConvertionData.IsConversion;
                    //saudaConversion.IsExtension = saudaConvertionData.IsExtension;
                    saudaConversion.SaudaConversionOrders = GetSaudaConversionDetails(inputDto).SuccessDto.Response as List<SaudaOrderDetails>;
                }
                return SucessResult(saudaConversion);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto ApproveSaudaConversion(SaudaConversionUpdateDto inputDto)
        {
            _methodName = "ApproveSaudaConversion";
            var resultDto = new ResultDto();
            try
            {

                if (inputDto == null || inputDto.SaudaIds == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (inputDto.SaudaIds != null && inputDto.SaudaIds.Any())
                {
                    foreach (var saudatConvertionId in inputDto.SaudaIds)
                    {
                        var saudaConvertion = _emamiContext.SaudaConversion.FirstOrDefault(w => w.Id == saudatConvertionId);
                        if (saudaConvertion != null)
                        {
                            if (saudaConvertion.StatusId == (int)DTO.Enums.Status.Pending)
                            {
                                saudaConvertion.StatusId = inputDto.StatusId;
                                saudaConvertion.ModifiedBy = inputDto.ModifiedBy;
                                saudaConvertion.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                                #region Reason
                                if (!string.IsNullOrEmpty(inputDto.Remarks))
                                {
                                    var entity = new Remarks()
                                    {
                                        TableId = saudaConvertion.Id,
                                        TableName = "SaudaConversion",
                                        ReasonTypeId = inputDto.StatusId,
                                        Description = inputDto.Remarks,
                                        ModifiedBy = inputDto.ModifiedBy,
                                        ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                                    };
                                    InsertReason(entity);
                                }
                                #endregion
                            }
                        }
                    }
                    _emamiContext.SaveChanges();

                    foreach (var saudatConvertionId in inputDto.SaudaIds)
                    {
                        var saudaConvertion = _emamiContext.SaudaConversion.FirstOrDefault(w => w.Id == saudatConvertionId);
                        if (saudaConvertion != null)
                        {
                            List<string> newSkuNameList = _emamiContext.SaudaConversionOrder.Where(w => w.SaudaConversionId == saudatConvertionId && w.Sku != null).Select(_ => _.Sku.SkuName).DefaultIfEmpty("").ToList();
                            string newSku = string.Empty;
                            string oldSku = string.Empty;
                            if (newSkuNameList != null && newSkuNameList.Any())
                            {
                                foreach (var newSkuName in newSkuNameList)
                                {
                                    if (!string.IsNullOrEmpty(newSkuName) && string.IsNullOrEmpty(newSku))
                                    {
                                        newSku = newSkuName;
                                    }
                                    else if (!string.IsNullOrEmpty(newSkuName))
                                    {
                                        newSku = newSku + ", " + newSkuName;
                                    }
                                }
                            }
                            if (saudaConvertion != null && saudaConvertion.SaudaOrder != null && saudaConvertion.SaudaOrder.Sku != null)
                            {
                                oldSku = saudaConvertion.SaudaOrder.Sku.SkuName;
                            }

                            try
                            {
                                List<User> usersContext = new List<User>();
                                List<string> toUsers = new List<string>();
                                User createdBy = new User();
                                User dealer = new User();

                                if (saudaConvertion.CreatedBy == saudaConvertion.DealerId)
                                {
                                    createdBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaConvertion.CreatedBy);
                                    if (createdBy != null)
                                    {
                                        toUsers.Add(createdBy.Email);
                                    }
                                }
                                else
                                {
                                    usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == saudaConvertion.CreatedBy || _.Id == saudaConvertion.DealerId).ToList();
                                    if (usersContext != null && usersContext.Any())
                                    {
                                        createdBy = usersContext.FirstOrDefault(_ => _.Id == saudaConvertion.CreatedBy);
                                        dealer = usersContext.FirstOrDefault(_ => _.Id == saudaConvertion.DealerId);
                                        if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
                                        {
                                            toUsers.Add(createdBy.Email);
                                        }
                                        if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                                        {
                                            toUsers.Add(dealer.Email);
                                        }
                                    }
                                }

                                if ((usersContext != null && usersContext.Any()) || createdBy != null)
                                {
                                    bool isEmail = false;
                                    var DealerNotificationContext = _emamiContext.TPNotification.AsNoTracking().
                                                                    Join(_emamiContext.TPNotificationDetails.AsNoTracking(), TPN => TPN.Id, TPND => TPND.TPNotificationId, (TPN, TPND) => new { TPN, TPND })
                                                                    .Where(_ => _.TPND.DealerId == saudaConvertion.DealerId && _.TPND.NotificationActionId == (int)DTO.Enums.NotificationActionTP.SaudaConversionApproval && _.TPND.IsActive).ToList();

                                    var DealerEmailNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.Email);
                                    if (DealerEmailNotificationEnabled != null && DealerEmailNotificationEnabled.Any())
                                        isEmail = true;
                                    else
                                        isEmail = false;

                                    AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                    var emailSubject = string.Empty;
                                    if (isEmail && toUsers != null && toUsers.Any())
                                    {
                                        var fromEmail = Constants.FromEmail;
                                        var plainText = string.Empty;
                                        EmailTemplate emailTemplate = new EmailTemplate();
                                        if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                        {
                                            emailSubject = Constants.SaudaConversionApprovalSubject;
                                            emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaConversionApprovalEmail);
                                        }
                                        else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                                        {
                                            emailSubject = Constants.SaudaConversionRejectSubject;
                                            emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaConversionRejectEmail);
                                        }
                                        if (emailTemplate != null)
                                        {
                                            var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuOld, oldSku).Replace(Constants.SkuNew, newSku).Replace(Constants.CustomerName, dealer.Name);
                                            var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                            amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                        }
                                    }
                                    var smsPlainTemplate = string.Empty;
                                    bool isSms = false;
                                    var DealerSMSNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.SMS);
                                    if (DealerSMSNotificationEnabled != null && DealerSMSNotificationEnabled.Any())
                                        isSms = true;
                                    else
                                        isSms = false;
                                    if (isSms)
                                    {
                                        var smsMessage = string.Empty;
                                        EmailTemplate smsTemplate = new EmailTemplate();
                                        if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                        {
                                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaConversionApprovalSMS);
                                        }
                                        else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                                        {
                                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaConversionRejectSMS);
                                        }
                                        if (smsTemplate != null)
                                        {
                                            smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuOld, oldSku).Replace(Constants.SkuNew, newSku).Replace(Constants.CustomerName, dealer.Name);
                                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                            try
                                            {
                                                if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                                                {
                                                    amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber);
                                                }
                                                if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                                {
                                                    amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber);
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                    }
                                    bool IsPushNotification = false;
                                    var DealerPushNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.InAppNotification);
                                    if (DealerPushNotificationEnabled != null && DealerPushNotificationEnabled.Any())
                                        IsPushNotification = true;
                                    else
                                        IsPushNotification = false;
                                    if (IsPushNotification)
                                    {
                                        if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
                                        {
                                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                            {
                                                PushTokenKey = createdBy.PushTokenKey,
                                                RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
                                                Title = emailSubject,
                                                Message = smsPlainTemplate,
                                                //Id = saudaOrderContext.Id,
                                            };
                                            _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                        }
                                        if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
                                        {
                                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                            {
                                                PushTokenKey = dealer.PushTokenKey,
                                                RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
                                                Title = emailSubject,
                                                Message = smsPlainTemplate,
                                                //Id = saudaOrderContext.Id,
                                            };
                                            _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }


                    resultDto.IsSuccess = true;
                    return resultDto;
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        #endregion

        #region View TP and RA

        /// <summary>
        /// Method to Get Traditional and Reverse Auction Pricing List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        public ResultDto GetTPandRAPricingList(PricingTPandRAInputDto inputDto)
        {
            _methodName = "GetTraditionalProcessPricingList";
            var resultDto = new ResultDto();
            var pricingEntityList = new List<Pricing>();
            try
            {
                if (inputDto.SaudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess)
                {
                    pricingEntityList = _emamiContext.Pricing.AsNoTracking()
                    //.Where(w => w.SaudaBookingTypeId == inputDto.SaudaBookingTypeId
                    //&& DbFunctions.TruncateTime(w.BiddingDate) == DbFunctions.TruncateTime(inputDto.CreatedDate)
                    //&& (inputDto.VerticalId > 0 ? w.OilType.DivisionId == inputDto.VerticalId : w.OilType.DivisionId > 0)
                    //)
                    .ToList();
                }
                else
                {
                    pricingEntityList = _emamiContext.Pricing.AsNoTracking()
                    //.Where(w => w.SaudaBookingTypeId == inputDto.SaudaBookingTypeId
                    //&& DbFunctions.TruncateTime(w.BiddingDate) == DbFunctions.TruncateTime(inputDto.BiddingDate) && w.BiddingWindowId == inputDto.BiddingWindowId
                    //&& (inputDto.VerticalId > 0 ? w.OilType.DivisionId == inputDto.VerticalId : w.OilType.DivisionId > 0)
                    //)
                    .ToList();
                }

                if (pricingEntityList != null && pricingEntityList.Any())
                {
                    var result = pricingEntityList.Select(s => new PricingDto()
                    {
                        Id = s.Id,
                        SkuName = s.Sku?.SkuName,
                        SkuCode = s.Sku?.SkuCode,
                        //OilTypeName = s.OilType.Name,
                        //OilPackingType = s.OilPackingType.Name,
                        //State = _emamiContext.State.AsNoTracking().FirstOrDefault(f => f.Id == s.StateId)?.StateName,
                        //City = _emamiContext.City.AsNoTracking().FirstOrDefault(f => f.Id == s.CityId)?.CityName,
                        //TransportMode = s.TransportMode.Name,
                        //Loadability = s.LoadQuantity,
                        Plant = _emamiContext.Depots.AsNoTracking().FirstOrDefault(f => f.Id == s.PlantId)?.Name,
                        //Depot = _emamiContext.Depots.AsNoTracking().FirstOrDefault(f => f.Id == s.DepotId)?.Name,
                        //FrieghtZone = _emamiContext.FreightZones.AsNoTracking().FirstOrDefault(f => f.Id == s.FrieghtZoneId)?.Name,
                        //FrieghtRoute = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(f => f.Id == s.FrieghtRouteId)?.Name,
                        //BiddingDate = s.BiddingDate,
                        //MaterialCost = s.MaterialCost,
                        //PackingCost = s.PackingCost,
                        //PrimaryFrieght = s.PrimaryFrieght,
                        //SecondaryFrieght = s.SecondaryFrieght,
                        //PlantSecondaryFrieght = s.PlantSecondaryFrieght,
                        //DepotCost = s.DepotCost,
                        //DetentionCost = s.DetentionCost,
                        //HoneycombCost = s.HoneycombCost,
                        //Margin = s.Margin,
                        //CushionMargin = s.CushionMargin,
                        //SchemeCostRecovery = s.SchemeCostRecovery,
                        //Discount = s.Discount,
                        //Premium = s.Premium,
                        //ProcessCost = s.ProcessCost,
                        //SumOfIngredientCost = s.SumOfIngredientCost,
                        //TpPrice = s.TpPrice,
                        //RaMargin = s.RaMargin,
                        //BaseRate = s.BaseRate,
                        //XMargin = s.XMargin,
                        //FinalRate = s.FinalRate,
                        //ExPlantPrice = s.ExPlantPrice + s.XMargin,
                        //ForDepotPrice = s.ForDepotPrice + s.XMargin,
                        //ForPlantPrice = s.ForPlantPrice + s.XMargin,
                        //ExDepotPrice = s.ExDepotPrice + s.XMargin,
                        //ClearanceRate = s.ClearanceRate,
                        //CounterBidOffer = s.CounterBidOffer,
                        //CounterBidLimit = s.CounterBidLimit,
                        //BpCpJumb = s.BpCpJumb,
                        //ExRakePrice = s.ExRakePrice + s.XMargin,
                        //ForRakePrice = s.ForRakePrice + s.XMargin
                    });

                    return _resultService.SuccessObject(result);
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(exception.Message);
            }
        }

        #endregion

        #region Sauda Extension

        public ResultDto GetSaudaExtensionList(SaudaConvertionFilterDto inputDto)
        {
            _methodName = "GetSaudaExtensionList";
            var resultDto = new ResultDto();
            try
            {

                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.FromDateEmpty;
                    return resultDto;
                }
                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.ToDateEmpty;
                    return resultDto;
                }

                var saudaConvertionContextList = new List<SaudaConversion>();

                if (inputDto.StatusId > 0)
                {
                    //saudaConvertionContextList = _emamiContext.SaudaConversion.AsNoTracking()
                    //.Where(w => DbFunctions.TruncateTime(w.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                    //DbFunctions.TruncateTime(w.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && w.StatusId == inputDto.StatusId && w.IsExtension).ToList();
                    saudaConvertionContextList = _emamiContext.SaudaConversion.AsNoTracking()
                    .Join(_emamiContext.SaudaOrders.AsNoTracking(), sc => sc.SaudaOrderId, so => so.Id, (sc, so) => new { SaudaConversion = sc, SaudaOrders = so })
                .Where(w => DbFunctions.TruncateTime(w.SaudaConversion.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                && DbFunctions.TruncateTime(w.SaudaConversion.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                && w.SaudaConversion.ExtensionStatusId == inputDto.StatusId && w.SaudaConversion.IsExtension
                && (inputDto.VerticalId > 0 ? w.SaudaOrders.OilType.DivisionId == inputDto.VerticalId : w.SaudaOrders.OilType.DivisionId > 0))
                .Select(s => s.SaudaConversion).ToList();
                }
                else
                {
                    //saudaConvertionContextList = _emamiContext.SaudaConversion.AsNoTracking()
                    //.Where(w => DbFunctions.TruncateTime(w.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                    //DbFunctions.TruncateTime(w.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && w.IsExtension).ToList();
                    saudaConvertionContextList = _emamiContext.SaudaConversion.AsNoTracking()
                    .Join(_emamiContext.SaudaOrders.AsNoTracking(), sc => sc.SaudaOrderId, so => so.Id, (sc, so) => new { SaudaConversion = sc, SaudaOrders = so })
                .Where(w => DbFunctions.TruncateTime(w.SaudaConversion.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                && DbFunctions.TruncateTime(w.SaudaConversion.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                && w.SaudaConversion.IsExtension
                && (inputDto.VerticalId > 0 ? w.SaudaOrders.OilType.DivisionId == inputDto.VerticalId : w.SaudaOrders.OilType.DivisionId > 0))
                .Select(s => s.SaudaConversion).ToList();
                }

                var saudaConvertionList = saudaConvertionContextList
                      .OrderByDescending(o => o.Id)
                      .Select(s => new SaudaConversionListDto()
                      {
                          Id = s.Id,
                          DealerId = s.DealerId,
                          DealerName = s.Dealer.Name,
                          CityName = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == s.Dealer.CityId).CityName,
                          PlantName = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == s.SaudaOrder.PlantId).Name,
                          ValidFrom = s.SaudaOrder.ValidFromDate,
                          IncoTerm = s.SaudaOrder.Incoterms1,
                          //DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == s.DealerId) != null ?
                          //_emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == s.DealerId).Name : string.Empty,
                          ExpiryDate = s.ExpiryDate,
                          ExtendToDate = s.ExtendToDate,
                          StatusId = s.ExtensionStatusId,
                          StatusName = s.ExtensionStatus.Name,
                          SaudaId = s.SaudaOrderId,
                          IsExtension = s.IsExtension,
                          SaudaNumber = s.SaudaOrder.SaudaNumber
                      }).ToList();
                return SucessResult(saudaConvertionList);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto ExportSaudaExtensionList(SaudaConvertionFilterDto inputDto)
        {
            _methodName = "ExportSaudaExtensionList";
            var resultDto = new ResultDto();
            try
            {

                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.FromDateEmpty;
                    return resultDto;
                }
                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.ToDateEmpty;
                    return resultDto;
                }

                var saudaConvertionList = new List<SaudaConversionWithOrderDetailListDto>();

                var saudaConvertionContextList = _emamiContext.SaudaConversion.AsNoTracking()
                    .Join(_emamiContext.SaudaOrders.AsNoTracking(), sc => sc.SaudaOrderId, so => so.Id, (sc, so) => new { SaudaConversion = sc, SaudaOrders = so })
                    .Join(_emamiContext.City.AsNoTracking(), x => x.SaudaConversion.Dealer.CityId, c => c.Id, (x, c) => new { x.SaudaConversion, x.SaudaOrders, City = c.CityName })
                    .Join(_emamiContext.Depots.AsNoTracking(), x => x.SaudaOrders.PlantId, d => d.Id, (x, d) => new { x.SaudaConversion, x.SaudaOrders, x.City, Plant = d.Name })
                    .GroupJoin(_emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking(), x => x.SaudaOrders.Id, lr => lr.SaudaOrderId, (x, lr) => new
                    {
                        x.SaudaConversion,
                        x.SaudaOrders,
                        x.City,
                        x.Plant,
                        PendingQuantity = lr.Select(_ => x.SaudaOrders.BidQuantity - _.LiftingQuantity),
                        PendingQuantityCase = lr.Select(_ => x.SaudaOrders.BidQuantityCase - _.LiftingQuantityCase)
                    })
                    .Where(w => DbFunctions.TruncateTime(w.SaudaConversion.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    && DbFunctions.TruncateTime(w.SaudaConversion.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    && (inputDto.StatusId > 0 ? w.SaudaConversion.ExtensionStatusId == inputDto.StatusId : true) && w.SaudaConversion.IsExtension
                    && (inputDto.VerticalId > 0 ? w.SaudaOrders.OilType.DivisionId == inputDto.VerticalId : w.SaudaOrders.OilType.DivisionId > 0));

                saudaConvertionList = saudaConvertionContextList
                  .OrderBy(o => o.SaudaConversion.Id)
                  .Select(s => new SaudaConversionWithOrderDetailListDto()
                  {
                      Id = s.SaudaConversion.Id,
                      DealerId = s.SaudaConversion.DealerId,
                      DealerName = s.SaudaConversion.Dealer != null ? s.SaudaConversion.Dealer.Name : string.Empty,
                      CityName = s.City,
                      PlantName = s.Plant,
                      ValidFrom = s.SaudaOrders.ValidFromDate,
                      IncoTerm = s.SaudaOrders.Incoterms1,
                      //DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == s.DealerId) != null ?
                      //_emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == s.DealerId).Name : string.Empty,
                      ExpiryDate = s.SaudaConversion.ExpiryDate,
                      ExtendToDate = s.SaudaConversion.ExtendToDate,
                      StatusId = s.SaudaConversion.ExtensionStatusId,
                      StatusName = s.SaudaConversion.ExtensionStatus.Name,
                      SaudaId = s.SaudaConversion.SaudaOrderId,
                      IsExtension = s.SaudaConversion.IsExtension,
                      SaudaNumber = s.SaudaOrders.SaudaNumber,
                      SkuId = s.SaudaOrders.SkuId,
                      SkuName = s.SaudaOrders.Sku.SkuName,
                      SkuCode = s.SaudaOrders.Sku.SkuCode,
                      QuotedPrice = s.SaudaOrders.QuotedPrice,
                      BidQuantity = s.SaudaOrders.BidQuantity,
                      BidQuantityCases = s.SaudaOrders.BidQuantityCase,
                      BidPrice = s.SaudaOrders.BidPrice,
                      BidPricePerCase = s.SaudaOrders.BidPrice != 0 && s.SaudaOrders.BidQuantityCase != 0 ? (s.SaudaOrders.BidPrice / s.SaudaOrders.BidQuantityCase) : 0,
                      PendingQuantity = s.PendingQuantity.FirstOrDefault() == 0 ? s.SaudaOrders.BidQuantity : s.PendingQuantity.FirstOrDefault(),
                      PendingQuantityCases = s.PendingQuantityCase.FirstOrDefault() == 0 ? s.SaudaOrders.BidQuantityCase : s.PendingQuantityCase.FirstOrDefault()
                  }).ToList();

                return SucessResult(saudaConvertionList);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto GetSaudaExtensionDetails(SaudaConversionDetailInputDto inputDto)
        {
            _methodName = "GetSaudaExtensionDetails";
            try
            {
                var saudaConvertionList = _emamiContext.SaudaConversionOrder.AsNoTracking().Where(w => w.SaudaConversionId == inputDto.SaudaConversionId)
                    .Select(s => new SaudaOrderDetails()
                    {
                        SkuId = s.SkuId,
                        SkuName = s.Sku.SkuName,
                        QuotedPrice = s.QuotedPrice,
                        BidQuantity = s.BidQuantity,
                        BidQuantityCases = s.BidQuantityCase,
                        BidPrice = s.BidPrice,
                        BidPricePerCase = Math.Round((s.BidPrice != 0 && s.BidQuantityCase != 0 ? (s.BidPrice / s.BidQuantityCase) : 0), 2)
                    }).ToList();
                return SucessResult(saudaConvertionList);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto GetSaudaExtensionAllDetail(SaudaConversionDetailInputDto inputDto)
        {
            _methodName = "GetSaudaExtensionAllDetail";
            try
            {
                var result = new SaudaConversionDetailForAdminDto();

                var saudaConversion = new SaudaConversionDetailForAdminDto();
                var saudaConvertionData = _emamiContext.SaudaConversion.AsNoTracking().FirstOrDefault(w => w.Id == inputDto.SaudaConversionId && w.IsExtension);

                if (saudaConvertionData != null)
                {
                    saudaConversion.SaudaConversionId = saudaConvertionData.Id;
                    saudaConversion.DealerId = saudaConvertionData.DealerId;
                    saudaConversion.DealerName = _emamiContext.Users.AsNoTracking().ToList().FirstOrDefault(f => f.Id == saudaConvertionData.DealerId).Name;
                    saudaConversion.SaudaId = saudaConvertionData.SaudaOrderId;
                    saudaConversion.SaudaNumber = saudaConvertionData.SaudaOrder != null ? saudaConvertionData.SaudaOrder.SaudaNumber.ToString() : string.Empty;
                    saudaConversion.ExpiryDate = saudaConvertionData.ExpiryDate;
                    saudaConversion.ExtendToDate = saudaConvertionData.ExtendToDate;
                    saudaConversion.ConversionDate = saudaConvertionData.CreatedDate;
                    saudaConversion.StatusId = saudaConvertionData.ExtensionStatusId;
                    saudaConversion.StatusName = saudaConvertionData.ExtensionStatus.Name;
                    saudaConversion.SaudaId = saudaConvertionData.SaudaOrderId;
                    saudaConversion.IsExtension = saudaConvertionData.IsExtension;
                    saudaConversion.SaudaConversionOrders = GetSaudaExtensionDetailsNew(inputDto).SuccessDto.Response as List<SaudaOrderDetails>;
                }
                return SucessResult(saudaConversion);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto ApproveSaudaExtension(SaudaConversionUpdateDto inputDto)
        {
            _methodName = "ApproveSaudaConversion";
            var resultDto = new ResultDto();
            try
            {

                if (inputDto == null || inputDto.SaudaIds == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (inputDto.SaudaIds != null && inputDto.SaudaIds.Any())
                {
                    foreach (var saudatConvertionId in inputDto.SaudaIds)
                    {
                        var saudaConvertion = _emamiContext.SaudaConversion.FirstOrDefault(w => w.Id == saudatConvertionId);
                        if (saudaConvertion.ExtensionStatusId == (int)DTO.Enums.Status.Pending)
                        {
                            saudaConvertion.ExtensionStatusId = inputDto.StatusId;
                            saudaConvertion.ModifiedBy = inputDto.ModifiedBy;
                            saudaConvertion.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                            #region Reason
                            if (!string.IsNullOrEmpty(inputDto.Remarks))
                            {
                                var entity = new Remarks()
                                {
                                    TableId = saudaConvertion.Id,
                                    TableName = "SaudaConversion",
                                    ReasonTypeId = inputDto.StatusId,
                                    Description = inputDto.Remarks,
                                    ModifiedBy = inputDto.ModifiedBy,
                                    ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                                };
                                InsertReason(entity);
                            }
                            #endregion
                        }
                    }
                    _emamiContext.SaveChanges();

                    foreach (var saudatConvertionId in inputDto.SaudaIds)
                    {
                        var saudaConvertion = _emamiContext.SaudaConversion.FirstOrDefault(w => w.Id == saudatConvertionId);
                        var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == saudaConvertion.SaudaOrderId);

                        try
                        {
                            if (saudaOrderContext != null && saudaOrderContext.ValidToDate != null && saudaConvertion.ExtendToDate != null && saudaOrderContext.ValidToDate != DateTime.MinValue && saudaConvertion.ExtendToDate != DateTime.MinValue)
                            {
                                List<User> usersContext = new List<User>();
                                List<string> toUsers = new List<string>();
                                User createdBy = new User();
                                User dealer = new User();
                                if (saudaConvertion.CreatedBy == saudaConvertion.DealerId)
                                {
                                    createdBy = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaConvertion.CreatedBy);
                                    if (createdBy != null)
                                    {
                                        toUsers.Add(createdBy.Email);
                                    }
                                }
                                else
                                {
                                    usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == saudaConvertion.CreatedBy || _.Id == saudaConvertion.DealerId).ToList();
                                    if (usersContext != null && usersContext.Any())
                                    {
                                        createdBy = usersContext.FirstOrDefault(_ => _.Id == saudaConvertion.CreatedBy);
                                        dealer = usersContext.FirstOrDefault(_ => _.Id == saudaConvertion.DealerId);
                                        if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
                                        {
                                            toUsers.Add(createdBy.Email);
                                        }
                                        if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                                        {
                                            toUsers.Add(dealer.Email);
                                        }
                                    }
                                }

                                if ((usersContext != null && usersContext.Any()) || createdBy != null)
                                {
                                    string noOfDays = (saudaConvertion.ExtendToDate?.Date - saudaOrderContext.ValidToDate.Date).Value.TotalDays.ToString();
                                    bool isEmail = false;
                                    var DealerNotificationContext = _emamiContext.TPNotification.AsNoTracking().
                                                                    Join(_emamiContext.TPNotificationDetails.AsNoTracking(), TPN => TPN.Id, TPND => TPND.TPNotificationId, (TPN, TPND) => new { TPN, TPND })
                                                                    .Where(_ => _.TPND.DealerId == saudaConvertion.DealerId && _.TPND.NotificationActionId == (int)DTO.Enums.NotificationActionTP.SaudaExtensionApproval && _.TPND.IsActive).ToList();

                                    var DealerEmailNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.Email);
                                    if (DealerEmailNotificationEnabled != null && DealerEmailNotificationEnabled.Any())
                                        isEmail = true;
                                    else
                                        isEmail = false;
                                    AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                    var emailSubject = string.Empty;
                                    if (isEmail && toUsers != null && toUsers.Any())
                                    {
                                        var fromEmail = Constants.FromEmail;
                                        var plainText = string.Empty;
                                        EmailTemplate emailTemplate = new EmailTemplate();
                                        if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                        {
                                            emailSubject = Constants.SaudaExtensionApprovalSubject;
                                            emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaExtensionApprovalNotificationEmail);
                                        }
                                        else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                                        {
                                            emailSubject = Constants.SaudaExtensionRejectSubject;
                                            emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaExtensionRequestNotificationEmail);
                                        }
                                        if (emailTemplate != null)
                                        {
                                            var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.NoOfDays, noOfDays);
                                            var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                            amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                        }
                                    }
                                    var smsPlainTemplate = string.Empty;
                                    bool isSms = false;
                                    var DealerSMSNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.SMS);
                                    if (DealerSMSNotificationEnabled != null && DealerSMSNotificationEnabled.Any())
                                        isSms = true;
                                    else
                                        isSms = false;
                                    if (isSms)
                                    {
                                        var smsMessage = string.Empty;
                                        EmailTemplate smsTemplate = new EmailTemplate();
                                        if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                        {
                                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaExtensionApprovalNotificationSMS);
                                        }
                                        else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                                        {
                                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaExtensionRejectNotificationSMS);
                                        }
                                        if (smsTemplate != null)
                                        {
                                            smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.NoOfDays, noOfDays);
                                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                            try
                                            {
                                                if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                                                {
                                                    amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber);
                                                }
                                                if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                                {
                                                    amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber);
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                    }
                                    bool IsPushNotification = false;
                                    var DealerPushNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.InAppNotification);
                                    if (DealerPushNotificationEnabled != null && DealerPushNotificationEnabled.Any())
                                        IsPushNotification = true;
                                    else
                                        IsPushNotification = false;
                                    if (IsPushNotification)
                                    {
                                        if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
                                        {
                                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                            {
                                                PushTokenKey = createdBy.PushTokenKey,
                                                RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
                                                Title = emailSubject,
                                                Message = smsPlainTemplate,
                                                //Id = saudaOrderContext.Id,
                                            };
                                            _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                        }
                                        if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
                                        {
                                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                            {
                                                PushTokenKey = dealer.PushTokenKey,
                                                RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
                                                Title = emailSubject,
                                                Message = smsPlainTemplate,
                                                //Id = saudaOrderContext.Id,
                                            };
                                            _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    resultDto.IsSuccess = true;
                    return resultDto;
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }



        #endregion

        #region Sauda Details Update

        public ResultDto UpdateSaudaDetails(SaudaDetailOutputDto inputDto)
        {
            _methodName = "UpdateSaudaDetails";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            try
            {
                decimal discountOrPremiumAmount = 0;
                decimal bidPrice = 0;
                decimal quotedPrice = 0;
                bool isChanged = false;

                if (inputDto.SalesOrganizationId == 0)
                {
                    return _resultService.ErrorMessage(Constants.SalesOrganisationIsEmpty);
                }
                if (inputDto.DistributionChannelId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DistributionChannelIsEmpty);
                }
                if (inputDto.DivisionId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DivisionIsEmpty);
                }
                var saudaDetail = _emamiContext.SaudaOrders.FirstOrDefault(f => f.Id == inputDto.SaudaOrderId);
                if (saudaDetail != null)
                {

                    #region Sauda Validation

                    // var statuses = Constants.OverallSaudaStatus;
                    //var SaudaOutstandingContext = (from s in _emamiContext.Sauda.AsNoTracking()
                    //                               join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                    //                               where s.UserId == inputDto.DealerId
                    //                               && so.StatusId == (int)DTO.Enums.Status.Pending
                    //                               && so.Id != inputDto.SaudaOrderId &&
                    //                               s.SalesOrganizationId == inputDto.SalesOrganizationId && s.DistributionChannelId == inputDto.DistributionChannelId && s.DivisionId == inputDto.DivisionId
                    //                               select so).ToList();

                    //decimal invoiceQuantity = 0;
                    //decimal RtninvoiceQuantity = 0;
                    //decimal existingSaudaQuantity = 0;
                    //if (SaudaOutstandingContext != null && SaudaOutstandingContext.Any())
                    //{
                    //    existingSaudaQuantity = SaudaOutstandingContext.Sum(s => s.BidQuantity);
                    //    //var skuIds = SaudaOutstandingContext.Select(s => s.SkuId).Distinct().ToList();
                    //    //var invoiceContext = (from inv in _emamiContext.Invoices.AsNoTracking()
                    //    //                      join invDet in _emamiContext.InvoiceDetails.AsNoTracking() on inv.Id equals invDet.InvoiceId
                    //    //                      where inv.UserId == inputDto.DealerId
                    //    //                      && skuIds.Contains(invDet.SkuId)
                    //    //                      select invDet).ToList();

                    //    //var rtninvoiceContext = (from inv in _emamiContext.Invoices.AsNoTracking()
                    //    //                         join invDet in _emamiContext.InvoiceDetails.AsNoTracking() on inv.Id equals invDet.InvoiceId
                    //    //                         where inv.UserId == inputDto.DealerId /*&& inv.SalesDocumentType == "ZHCR"*/
                    //    //                         && skuIds.Contains(invDet.SkuId)
                    //    //                         select invDet
                    //    //                      ).ToList();

                    //    //if (invoiceContext != null && invoiceContext.Any())
                    //    //{
                    //    //    invoiceQuantity = invoiceContext.Sum(s => s.ActualBilledQuantity);
                    //    //}
                    //    //if (rtninvoiceContext != null && rtninvoiceContext.Any())
                    //    //{
                    //    //    RtninvoiceQuantity = rtninvoiceContext.Sum(_ => _.ActualBilledQuantity);
                    //    //}
                    //}

                    var dealerContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == inputDto.DealerId);

                    var userdivContext = _emamiContext.UserDivisionMappings.AsNoTracking()
                           .FirstOrDefault(_ => _.UserId == inputDto.DealerId
                          && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId
                           && _.DivisionId == inputDto.DivisionId);
                    var saudaLimitExist = userdivContext.SaudaLimit ?? 0;
                    // var pendingContracttablevalue = _emamiContext.PendingContracts.AsNoTracking().Where(_ => _.UserId == inputDto.DealerId && _.SalesOrgId == inputDto.SalesOrganizationId && _.DistChnlId == inputDto.DistributionChannelId && _.DivisionId == inputDto.DivisionId).ToList().IsAny() ? _emamiContext.PendingContracts.AsNoTracking().Where(_ => _.UserId == inputDto.DealerId && _.SalesOrgId == inputDto.SalesOrganizationId && _.DistChnlId == inputDto.DistributionChannelId && _.DivisionId == inputDto.DivisionId).Select(_ => _.SaudaQuantity).Sum() : 0;

                    var SaudaLimit = _resultService.AvailableSaudaLimit(inputDto.DealerId, saudaLimitExist, inputDto.SalesOrganizationId, inputDto.DistributionChannelId, inputDto.DivisionId);
                    if (inputDto.BidQuantityCase > saudaDetail.BidQuantityCase)
                    {
                        var SaudaOutstanding = (_resultService.ConvertCasetoMetricTon(inputDto.BidQuantityCase, saudaDetail.SkuId)) - saudaDetail.BidQuantity;
                        //var SaudaLimit = saudaLimitExist - existingSaudaQuantity - pendingContracttablevalue;
                        if (SaudaLimit < SaudaOutstanding)
                        {
                            return _resultService.ErrorMessage(Constants.SaudaLimitIsExceeds);
                        }
                    }

                    #endregion

                    bidPrice = inputDto.BidQuantityCase * inputDto.BidPricePerCase; // Here bid price per case is Discount or premium applied so below formula used for Discount and premium
                    quotedPrice = bidPrice;

                    //decimal itemquotedprice = inputDto.BidQuantity * item.QuotedPrice;
                    //item.QuotedPrice = itemquotedprice;
                    //item.BidPrice = itemquotedprice;



                    if (inputDto.DiscountTypeId == (int)DTO.Enums.SaudaDiscountType.Discount)
                    {
                        quotedPrice = quotedPrice + inputDto.DiscountAmount; // Discount
                    }
                    else
                    {
                        quotedPrice = quotedPrice - inputDto.DiscountAmount; // Premium
                    }
                    if (!bidPrice.Equals(saudaDetail.BidPrice) || !inputDto.BidQuantityCase.Equals(saudaDetail.BidQuantityCase))
                    {
                        if (saudaDetail.SaudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess)
                        {
                            saudaDetail.BidQuantityCase = inputDto.BidQuantityCase;
                            saudaDetail.BidQuantity = _resultService.ConvertCasetoMetricTon(inputDto.BidQuantityCase, inputDto.SkuId);
                            //if (string.IsNullOrEmpty(saudaDetail.SaudaNumber))
                            //{
                            //    saudaDetail.BidQuantityCaseForDailyReport = inputDto.BidQuantityCase;
                            //    saudaDetail.BidQuantityForDailyReport = _resultService.ConvertCasetoMetricTon(inputDto.BidQuantityCase, inputDto.SkuId);
                            //}


                            //if (bidPrice > quotedPrice)
                            //{
                            //discountOrPremiumAmount = bidPrice - quotedPrice;
                            saudaDetail.BidQuantityCase = inputDto.BidQuantityCase;
                            saudaDetail.BidQuantity = _resultService.ConvertCasetoMetricTon(inputDto.BidQuantityCase, inputDto.SkuId);
                            saudaDetail.BidPrice = bidPrice;
                            saudaDetail.QuotedPrice = quotedPrice;
                            //saudaDetail.DiscountAmount = discountOrPremiumAmount;
                            //saudaDetail.DiscountTypeId = (int)SaudaDiscountType.Premium;
                            isChanged = true;
                            //if (string.IsNullOrEmpty(saudaDetail.SaudaNumber))
                            //{
                            //    saudaDetail.BidQuantityCaseForDailyReport = inputDto.BidQuantityCase;
                            //    saudaDetail.BidQuantityForDailyReport = _resultService.ConvertCasetoMetricTon(inputDto.BidQuantityCase, inputDto.SkuId);
                            //    saudaDetail.BidPriceForDailyReport = bidPrice;
                            //    saudaDetail.QuotedPriceForDailyReport = quotedPrice;
                            //    saudaDetail.DiscountAmountForDailyReport = discountOrPremiumAmount;
                            //    saudaDetail.DiscountTypeIdForDailyReport = (int)SaudaDiscountType.Premium;
                            //}
                            //}
                            //else
                            //{

                            //    discountOrPremiumAmount = quotedPrice - bidPrice;
                            //    saudaDetail.BidQuantityCase = inputDto.BidQuantityCase;
                            //    saudaDetail.BidQuantity = _resultService.ConvertCasetoMetricTon(inputDto.BidQuantityCase, inputDto.SkuId);
                            //    saudaDetail.BidPrice = bidPrice;
                            //    saudaDetail.QuotedPrice = quotedPrice;
                            //    saudaDetail.DiscountAmount = discountOrPremiumAmount;
                            //    saudaDetail.DiscountTypeId = (int)SaudaDiscountType.Discount;
                            //    isChanged = true;
                            //    //if (string.IsNullOrEmpty(saudaDetail.SaudaNumber))
                            //    //{
                            //    //    saudaDetail.BidQuantityCaseForDailyReport = inputDto.BidQuantityCase;
                            //    //    saudaDetail.BidQuantityForDailyReport = _resultService.ConvertCasetoMetricTon(inputDto.BidQuantityCase, inputDto.SkuId);
                            //    //    saudaDetail.BidPriceForDailyReport = bidPrice;
                            //    //    saudaDetail.QuotedPriceForDailyReport = quotedPrice;
                            //    //    saudaDetail.DiscountAmountForDailyReport = discountOrPremiumAmount;
                            //    //    saudaDetail.DiscountTypeIdForDailyReport = (int)SaudaDiscountType.Discount;
                            //    //}
                            //}
                        }
                        //else if (saudaDetail.SaudaBookingTypeId == (int)SaudaBookingTypes.ReverseAuction)
                        //{
                        //    var biddingWindowStatusId = _emamiContext.BiddingWindow.FirstOrDefault(_ => _.Id == saudaDetail.BiddingwindowId).StatusId;
                        //    if ((biddingWindowStatusId == (int)DTO.Enums.BiddWindowStatus.Completed) || (biddingWindowStatusId == (int)DTO.Enums.BiddWindowStatus.Stopped))
                        //    {
                        //        saudaDetail.SkuDiscount = inputDto.BidQuantityCase * saudaDetail.SkuDiscountCase;
                        //        saudaDetail.SchemeDiscount = inputDto.BidQuantityCase * saudaDetail.SchemeDiscountCase;
                        //        saudaDetail.VolumeDiscount = inputDto.BidQuantityCase * saudaDetail.VolumeDiscountCase;
                        //        if (string.IsNullOrEmpty(saudaDetail.SaudaNumber))
                        //        {
                        //            saudaDetail.SkuDiscountForDailyReport = inputDto.BidQuantityCase * saudaDetail.SkuDiscountCase;
                        //            saudaDetail.SchemeDiscountForDailyReport = inputDto.BidQuantityCase * saudaDetail.SchemeDiscountCase;
                        //            saudaDetail.VolumeDiscountForDailyReport = inputDto.BidQuantityCase * saudaDetail.VolumeDiscountCase;
                        //        }
                        //        if (saudaDetail.GPBenefitType == (int)DTO.Enums.BenefitType.NONSAP)
                        //        {
                        //            saudaDetail.GPBenefitDiscountOrDay = inputDto.BidQuantityCase * saudaDetail.GPBenefitDiscountInCase;
                        //            if (string.IsNullOrEmpty(saudaDetail.SaudaNumber))
                        //            {
                        //                saudaDetail.GPBenefitDiscountOrDayForDailyReport = inputDto.BidQuantityCase * saudaDetail.GPBenefitDiscountInCase;
                        //            }
                        //        }
                        //        if (saudaDetail.SurpriseBenefitType == (int)DTO.Enums.BenefitType.NONSAP)
                        //        {
                        //            saudaDetail.SurpriseBenefitDiscountOrDay = inputDto.BidQuantityCase * saudaDetail.SurpriseBenefitDiscountInCase;
                        //            if (string.IsNullOrEmpty(saudaDetail.SaudaNumber))
                        //            {
                        //                saudaDetail.SurpriseBenefitDiscountOrDayForDailyReport = inputDto.BidQuantityCase * saudaDetail.SurpriseBenefitDiscountInCase;
                        //            }
                        //        }
                        //        var totalDiscount = saudaDetail.SkuDiscount + saudaDetail.SchemeDiscount + saudaDetail.VolumeDiscount + saudaDetail.GPBenefitDiscountOrDay + saudaDetail.SurpriseBenefitDiscountOrDay;
                        //        quotedPrice = (inputDto.BidQuantityCase * inputDto.BidPricePerCase) + totalDiscount;
                        //        bidPrice = inputDto.BidQuantityCase * inputDto.BidPricePerCase;

                        //        saudaDetail.BidPrice = bidPrice;
                        //        saudaDetail.QuotedPrice = quotedPrice;
                        //        saudaDetail.BidQuantityCase = inputDto.BidQuantityCase;
                        //        saudaDetail.BidQuantity = _resultService.ConvertCasetoMetricTon(inputDto.BidQuantityCase, inputDto.SkuId);
                        //        isChanged = true;
                        //        if (string.IsNullOrEmpty(saudaDetail.SaudaNumber))
                        //        {
                        //            saudaDetail.BidPriceForDailyReport = bidPrice;
                        //            saudaDetail.QuotedPriceForDailyReport = quotedPrice;
                        //            saudaDetail.BidQuantityCaseForDailyReport = inputDto.BidQuantityCase;
                        //            saudaDetail.BidQuantityForDailyReport = _resultService.ConvertCasetoMetricTon(inputDto.BidQuantityCase, inputDto.SkuId);
                        //        }
                        //    }
                        //    else if (biddingWindowStatusId == (int)DTO.Enums.BiddWindowStatus.Processing)
                        //    {
                        //        resultDto.IsSuccess = false;
                        //        resultDto.ErrorDto.Message = Constants.BiddingErrorMessage;
                        //        return resultDto;
                        //    }
                        //    //resultDto.IsSuccess = false;
                        //    //resultDto.ErrorDto.Message = "Some error occured";
                        //    //return resultDto;
                        //}
                    }
                    if (isChanged)
                    {
                        saudaDetail.ModifiedBy = inputDto.LoginUserId;
                        saudaDetail.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        _emamiContext.SaveChanges();
                        //var Id = new List<long> { saudaDetail.SaudaId };
                        //_sapIntegrationService.GetSaudaDetails(Id);
                    }
                    resultDto.IsSuccess = true;
                }
                else
                {
                    resultDto.IsSuccess = false;
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
            }
            return resultDto;
        }

        #endregion


        #region Load Test

        //public ResultDto GetSaudaOrdersTradeTicketMappingDetailsLoadTest(TradeTicketDto input)
        //{

        //    //string mStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",
        //    //                               CultureInfo.InvariantCulture);
        //    _methodName = "GetSaudaOrdersTradeTicketMappingDetailsLoadTest";
        //    var startDateTime = DateTime.Now;
        //    var resultDto = new ResultDto();
        //    _logger.Info($"{_methodName} Process-StartDateTime {startDateTime}");

        //    var outputDto = new TradeTicketSaudaMappingDto();
        //    var saudaOrderIds = new List<long>();
        //    var tradeTicketContexts = _emamiContext.TradeTicket.AsNoTracking().Where(_ => _.TradeTicketNumber == input.TradeticketNumber).FirstOrDefault();
        //    if (tradeTicketContexts == null)
        //    {
        //        return NotFoundResult();
        //    }
        //    IdInputDto inputDto = new IdInputDto() { Id = tradeTicketContexts.Id };
        //    if (inputDto == null)
        //    {
        //        return NotFoundResult();
        //    }
        //    try
        //    {
        //        var tradeTicketContext = _emamiContext.TradeTicket.AsNoTracking().Where(_ => _.Id == inputDto.Id).FirstOrDefault();
        //        if (tradeTicketContext != null)
        //        {
        //            var tradeTicketDetails = _emamiContext.TradeTicketDetails.AsNoTracking().Where(_ => _.TradeTicketId == inputDto.Id).ToList();
        //            long plantId = tradeTicketContext.DepotId;
        //            var materialTypeId = tradeTicketContext.MaterialTypeId;
        //            var depotIds = _emamiContext.PlantDepotMapping.AsNoTracking().Where(f => f.PlantId == plantId).Select(_ => _.DepotId).ToList();

        //            var saudaQuantity = 0m;
        //            var saudaOrderList = _emamiContext.SaudaOrders.AsNoTracking()
        //                .Where(_ => _.TradeTicketNumber == tradeTicketContext.TradeTicketNumber && (_.StatusId == (int)DTO.Enums.Status.Pending || _.StatusId == (int)Adani.Solution.DTO.Enums.Status.Approved)).ToList();
        //            if (saudaOrderList != null && saudaOrderList.Any())
        //            {
        //                saudaQuantity = saudaOrderList.Sum(_ => _.BidQuantity);
        //            }

        //            var saudaTotalQuantity = 0m;
        //            var allSaudaOrdersList = _emamiContext.Sauda.AsNoTracking()
        //                    .Join(_emamiContext.SaudaOrders.AsNoTracking().Where(w => (w.PlantId == plantId || depotIds.Contains(w.PlantId))
        //                    && w.StatusId == (int)Adani.Solution.DTO.Enums.Status.Pending && (w.TradeTicketNumber == string.Empty || w.TradeTicketNumber == null)), s => s.Id, so => so.SaudaId, (s, so) => new { Sauda = s, SaudaOrders = so })
        //                    .Join(_emamiContext.Skus.AsNoTracking()
        //                    //.Where(w => w.MaterialTypeId == materialTypeId)
        //                    , so => so.SaudaOrders.SkuId, sk => sk.Id, (so, sk) => new { so.Sauda, so.SaudaOrders, Skus = sk })
        //                    .Where(w => DbFunctions.TruncateTime(w.SaudaOrders.CreatedDate) == DbFunctions.TruncateTime(DateTime.Now)).ToList();
        //            _logger.Info("All SaudaOrdersList Start");
        //            if (allSaudaOrdersList != null && allSaudaOrdersList.Any())
        //            {
        //                saudaTotalQuantity = allSaudaOrdersList.Sum(_ => _.SaudaOrders.BidQuantity);
        //                saudaOrderIds = allSaudaOrdersList.Select(s => s.SaudaOrders.Id).ToList();
        //            }
        //            _logger.Info($"{saudaOrderIds} All SaudaOrdersList Completed");

        //            if (saudaOrderIds != null && saudaOrderIds.Any())
        //            {
        //                var saudaOrders = _emamiContext.SaudaOrders
        //                    .Where(w => saudaOrderIds.Contains(w.Id) && DbFunctions.TruncateTime(w.CreatedDate) == DbFunctions.TruncateTime(DateTime.Now)).ToList();
        //                int count = 0;
        //                _logger.Info($"Count {count}");
        //                if (saudaOrders != null && saudaOrders.Any())
        //                {
        //                    //string dbStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",
        //                    //                CultureInfo.InvariantCulture);
        //                    foreach (var orders in saudaOrders)
        //                    {
        //                        count++;
        //                        orders.TradeTicketNumber = input.TradeticketNumber;
        //                        orders.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //                    }
        //                    _emamiContext.SaveChanges();
        //                    ////string dbEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",
        //                    ////                CultureInfo.InvariantCulture);
        //                    //string mEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",
        //                    //               CultureInfo.InvariantCulture);
        //                    //StringBuilder sb = new StringBuilder();
        //                    //TimeSpan timeSpan = Convert.ToDateTime(mEndTime) - Convert.ToDateTime(mStartTime);
        //                    //int mTotalMilliSeconds = (int)timeSpan.TotalMilliseconds;
        //                    //TimeSpan timeSpan2 = Convert.ToDateTime(dbEndTime) - Convert.ToDateTime(dbStartTime);
        //                    //int mTotalMilliSeconds2 = (int)timeSpan2.TotalMilliseconds;
        //                    //sb.Append($"LoginUserId, {inputDto.LoginUserId}, SaudaOrdersTradeTicketMapping, StartTime, {mStartTime} ,EndTime, {mEndTime}, TotalSaudaTTMappingTime, {mTotalMilliSeconds}, DBOperation, SaudaOrders ,StartTime, {dbStartTime} ,EndTime, {dbEndTime}, TotalSaudaOrderTime, {mTotalMilliSeconds2}");
        //                    //string serverFoloderPath = HostingEnvironment.MapPath("~/LogFiles/");
        //                    //string filePath = Path.Combine(serverFoloderPath + "SaudaTTMapping.txt");
        //                    //File.AppendAllText(filePath, sb.ToString() + Environment.NewLine);

        //                    return _resultService.SuccessMessage($"{count} Tradeticket Sauda Mapped Successfully");
        //                }
        //                else
        //                {
        //                    return _resultService.ErrorMessage($"Tradeticket Sauda Mapped Error");
        //                }
        //            }
        //            else
        //            {
        //                return _resultService.ErrorMessage($"Sauda Order Id Is Empty");
        //            }
        //        }
        //        else
        //        {
        //            return _resultService.ErrorMessage(Constants.RecordNotFound);
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.Info($"{_methodName} ProcessStartDateTime {startDateTime}  EndDateTime {DateTime.Now}");
        //        return ExceptionResult(exception);
        //    }
        //}




        public ResultDto SaudaApproveLoadTest(SaudaDto inputDto)
        {
            _methodName = "SaudaApproveLoadTest";
            var resultDto = new ResultDto();
            try
            {
                var message = "No records found";
                var statusId = (int)DTO.Enums.Status.Pending;

                var saudaIds = _emamiContext.Sauda.AsNoTracking()
                    .Where(w => DbFunctions.TruncateTime(w.BiddingDate) == DbFunctions.TruncateTime(DateTime.Now)).Select(s => s.Id).ToList();

                if (saudaIds != null && saudaIds.Any())
                {
                    var saudaOrders = _emamiContext.SaudaOrders.Where(w => saudaIds.Any(s => s == w.SaudaId)
                    && !string.IsNullOrEmpty(w.SaudaNumber)
                    && w.StatusId == statusId)
                    .Select(s => s).ToList();

                    if (saudaOrders != null && saudaOrders.Any())
                    {
                        saudaOrders.ForEach(f => f.StatusId = inputDto.StatusId);
                        _emamiContext.SaveChanges();
                        message = "Sauda status updated successfully";
                    }
                    else
                    {
                        message = "Sauda Order no records found";
                    }
                }
                return _resultService.SuccessMessage(message);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.CostAlreadyExistiInThisDate);
            }
        }

        public ResultDto LiftingRequestApproveLoadTest(SaudaDto inputDto)
        {
            _methodName = "LiftingRequestApproveLoadTest";
            var message = "No records found";
            var resultDto = new ResultDto();
            try
            {
                var statusId = (int)DTO.Enums.Status.Pending;

                var liftingContext = _emamiContext.LiftingRequest
                    .Where(w => DbFunctions.TruncateTime(w.LiftingDate) == DbFunctions.TruncateTime(DateTime.Now)
                    && w.StatusId == statusId).ToList();

                if (liftingContext != null && liftingContext.Any())
                {
                    liftingContext.ForEach(f => f.StatusId = inputDto.StatusId);
                    _emamiContext.SaveChanges();
                    message = "Lifting status updated successfully";
                }
                else
                {
                    message = "Lifting no records found";
                }

                return _resultService.SuccessMessage(message);
            }
            catch (Exception exception)
            {
                var messages = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(messages);
                return _resultService.ErrorMessage(Constants.CostAlreadyExistiInThisDate);
            }
        }

        #endregion

        #region Cr for sauda extension
        public ResultDto GetBookedSaudaWithExtensionDetailsList(SaudaExtensionFilterDto inputDto)
        {
            _methodName = "GetBookedSaudaWithExtensionDetailsList";
            var resultDto = new ResultDto();
            var BookedSaudaWithExtensionDetailsList = new List<SaudaBookedSaudaWithExtensionDetailsListDto>();
            var StateList = new List<String>();
            try
            {
                var CurrentDate = DateHelper.UtcToIndia(DateTime.UtcNow.Date).Date;
                var previousDate = CurrentDate.AddDays(Constants.NumberOfDaysAddedTogetPreviousDate);
                var UserContext = _emamiContext.Users.AsNoTracking().ToList();
                if (inputDto.OilTypeIds == null)
                {
                    var OilTypeIds = _emamiContext.OilTypes.AsNoTracking().Select(_ => _.Id).ToList();
                    inputDto.OilTypeIds = OilTypeIds.Select(i => (long?)i).ToList();
                }

                IEnumerable<DivisionDetailsDto> divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.UserId)
                .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                var pendingContractsContext = (from p in _emamiContext.PendingContracts.AsNoTracking()
                                               join ud in divisionslogieduser on new { SalesOrganizationId = p.SalesOrgId, DistributionChannelId = p.DistChnlId, DivisionId = p.DivisionId }
                                              equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                               where !p.IsSaudaExtended
                                               select p
                                             ).ToList();

                //var pendingContractsContext = _emamiContext.PendingContracts.AsNoTracking().Where(_ => !_.IsSaudaExtended).ToList();
                var skuContext = _emamiContext.Skus.AsNoTracking().ToList();
                var saudaExtensionContext = _emamiContext.SaudaExtension.AsNoTracking().Where(_ => _.IsActive && DbFunctions.TruncateTime(CurrentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(CurrentDate) <= DbFunctions.TruncateTime(_.ValidTo)).ToList();
                var role = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.UserId).RoleId;
                var bdoContext = new List<long>();
                if (role == (int)DTO.Enums.Role.StateTrader)
                {
                    bdoContext.Add(inputDto.UserId);
                }
                else
                {
                    //New Reporting to table change
                    bdoContext = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.UserId).Select(_ => _.UserId).ToList();
                    //bdoContext = UserContext.Where(_ => _.ReportingToId == inputDto.UserId).Select(_ => _.Id).ToList();
                }



                var StateContext = _emamiContext.State.AsNoTracking().ToList();
                var StatesinSaudaExtensionContext = saudaExtensionContext.Where(_ => _.IsActive && CurrentDate >= _.ValidFrom && CurrentDate <= _.ValidTo).Select(_ => _.StateId).ToList();
                if ((inputDto.BdoIds == null) && (inputDto.DealerIds == null))
                {
                    var dealerIdsContext = _emamiContext.UserCustomerMapping.AsNoTracking()
                                        .Where(_ => bdoContext.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    var dealerCodeContext = UserContext.Where(_ => dealerIdsContext.Contains(_.Id)).Select(a => a.Code).ToList();

                    BookedSaudaWithExtensionDetailsList = pendingContractsContext.Join(UserContext, p => p.CustomerCode, u => u.Code, (p, u) => new { p, u })
                        .Join(skuContext, a => a.p.MaterialCode, sku => sku.SkuCode, (a, sku) => new { a, sku })
                        .Join(saudaExtensionContext, b => b.sku.OilTypeId, saudaextension => saudaextension.OilTypeId, (b, saudaextension) => new { b, saudaextension })
                        .Where(_ => dealerCodeContext.Contains(_.b.a.p.CustomerCode) && inputDto.OilTypeIds.Contains(_.b.sku.OilTypeId)
                        && _.b.a.p.ContractValidTo.Value.Date <= previousDate &&
                            _.saudaextension.StateId == _.b.a.u.StateId
                            //&& _.b.a.u.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess 
                            && _.b.a.p.PendingQuantityInCase > 0
                            //&& _.b.sku.DivisionId == _.b.a.u.DivisionId
                            ).Select(s => new SaudaBookedSaudaWithExtensionDetailsListDto
                            {
                                SaudaOrderId = s.b.a.p.SaudaOrderId,
                                PendingContractId = s.b.a.p.Id,
                                SaudaBookedDate = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.SaudaNumber == s.b.a.p.SaudaNumber) != null ? _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.SaudaNumber == s.b.a.p.SaudaNumber).BiddingDate : DateTime.MinValue,
                                PendingQuantityCase = s.b.a.p.PendingQuantityInCase,
                                SaudaQuantityCase = s.b.a.p.SaudaQuantity,
                                SaudaValidToDate = s.b.a.p.ContractValidTo,
                                SaudaNumber = s.b.a.p.SaudaNumber,
                                BookedSku = s.b.sku.SkuName,
                                SaudaExtendedDays = s.saudaextension.ExtensionDays.ToString() + "days",
                                SaudaExtendedToDate = AddBusinessDays(s.b.a.p.ContractValidTo, s.saudaextension.ExtensionDays),
                                BasicRate = s.b.a.p.BasicRate,
                                DivisionId = s.b.a.p.DivisionId,
                                SalesOrganizationId = s.b.a.p.SalesOrgId,
                                DistributionChannelId = s.b.a.p.DistChnlId,
                                SkuCode = s.b.a.p.MaterialCode
                            }).Distinct().ToList();

                    var StateIds = pendingContractsContext.Join(UserContext, p => p.CustomerCode, u => u.Code, (p, u) => new { p, u })
                        .Join(skuContext, a => a.p.MaterialCode, sku => sku.SkuCode, (a, sku) => new { a, sku })
                        .Where(_ => dealerCodeContext.Contains(_.a.p.CustomerCode) && inputDto.OilTypeIds.Contains(_.sku.OilTypeId) && _.a.p.ContractValidTo.Value.Date <= previousDate &&
                                !StatesinSaudaExtensionContext.Contains(_.a.u.StateId) && _.a.u.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess
                                //&& _.sku.DivisionId == _.a.u.DivisionId
                                ).Select(a => a.a.u.StateId).Distinct().ToList();

                    StateList = StateContext.Where(_ => StateIds.Contains(_.Id)).Select(a => a.StateName).ToList();

                }
                else if ((inputDto.BdoIds != null && inputDto.BdoIds.Any()) && (inputDto.DealerIds == null))
                {
                    var dealerIdsContext = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => inputDto.BdoIds.Contains(_.UserId)).Select(a => a.CustomerId).ToList();
                    var dealerCodeContext = UserContext.Where(_ => dealerIdsContext.Contains(_.Id)).Select(a => a.Code).ToList();

                    BookedSaudaWithExtensionDetailsList = pendingContractsContext.Join(UserContext, p => p.CustomerCode, u => u.Code, (p, u) => new { p, u })
                        .Join(skuContext, a => a.p.MaterialCode, sku => sku.SkuCode, (a, sku) => new { a, sku })
                        .Join(saudaExtensionContext, b => b.sku.OilTypeId, saudaextension => saudaextension.OilTypeId, (b, saudaextension) => new { b, saudaextension })
                        .Where(_ => dealerCodeContext.Contains(_.b.a.p.CustomerCode) && inputDto.OilTypeIds.Contains(_.b.sku.OilTypeId) && _.b.a.p.ContractValidTo.Value.Date <= previousDate &&
                            _.saudaextension.StateId == _.b.a.u.StateId && _.b.a.u.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess && _.b.a.p.PendingQuantityInCase > 0
                            //&& _.b.sku.DivisionId == _.b.a.u.DivisionId
                            ).Select(s => new SaudaBookedSaudaWithExtensionDetailsListDto
                            {
                                SaudaOrderId = s.b.a.p.SaudaOrderId,
                                PendingContractId = s.b.a.p.Id,
                                SaudaBookedDate = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.SaudaNumber == s.b.a.p.SaudaNumber) != null ? _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.SaudaNumber == s.b.a.p.SaudaNumber).BiddingDate : DateTime.MinValue,
                                PendingQuantityCase = s.b.a.p.PendingQuantityInCase,
                                SaudaQuantityCase = s.b.a.p.SaudaQuantity,
                                SaudaValidToDate = s.b.a.p.ContractValidTo,
                                SaudaNumber = s.b.a.p.SaudaNumber,
                                BookedSku = s.b.sku.SkuName,
                                SaudaExtendedDays = s.saudaextension.ExtensionDays.ToString() + "days",
                                SaudaExtendedToDate = AddBusinessDays(s.b.a.p.ContractValidTo, s.saudaextension.ExtensionDays),
                                BasicRate = s.b.a.p.BasicRate,
                                DivisionId = s.b.a.p.DivisionId,
                                SalesOrganizationId = s.b.a.p.SalesOrgId,
                                DistributionChannelId = s.b.a.p.DistChnlId,
                                SkuCode = s.b.a.p.MaterialCode
                            }).Distinct().ToList();

                    var StateIds = pendingContractsContext.Join(UserContext, p => p.CustomerCode, u => u.Code, (p, u) => new { p, u })
                        .Join(skuContext, a => a.p.MaterialCode, sku => sku.SkuCode, (a, sku) => new { a, sku })
                        .Where(_ => dealerCodeContext.Contains(_.a.p.CustomerCode) && inputDto.OilTypeIds.Contains(_.sku.OilTypeId) && _.a.p.ContractValidTo.Value.Date <= previousDate &&
                                !StatesinSaudaExtensionContext.Contains(_.a.u.StateId)
                                //&& _.a.u.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess 
                                //&& _.sku.DivisionId == _.a.u.DivisionId
                                ).Select(a => a.a.u.StateId).Distinct().ToList();

                    StateList = StateContext.Where(_ => StateIds.Contains(_.Id)).Select(a => a.StateName).ToList();
                }
                else
                {
                    var dealerCodeContext = UserContext.Where(_ => inputDto.DealerIds.Contains(_.Id)).Select(a => a.Code).ToList();

                    BookedSaudaWithExtensionDetailsList = pendingContractsContext.Join(UserContext, p => p.CustomerCode, u => u.Code, (p, u) => new { p, u })
                   .Join(skuContext, a => a.p.MaterialCode, sku => sku.SkuCode, (a, sku) => new { a, sku })
                   .Join(saudaExtensionContext, b => b.sku.OilTypeId, saudaextension => saudaextension.OilTypeId, (b, saudaextension) => new { b, saudaextension })
                   .Where(_ => dealerCodeContext.Contains(_.b.a.p.CustomerCode) && inputDto.OilTypeIds.Contains(_.b.sku.OilTypeId) && _.b.a.p.ContractValidTo.Value.Date <= previousDate &&
                    _.saudaextension.StateId == _.b.a.u.StateId
                    //&& _.b.a.u.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess 
                    && _.b.a.p.PendingQuantityInCase > 0
                    //&& _.b.sku.DivisionId == _.b.a.u.DivisionId
                    ).Select(s => new SaudaBookedSaudaWithExtensionDetailsListDto
                    {
                        SaudaOrderId = s.b.a.p.SaudaOrderId,
                        PendingContractId = s.b.a.p.Id,
                        SaudaBookedDate = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.SaudaNumber == s.b.a.p.SaudaNumber) != null ? _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.SaudaNumber == s.b.a.p.SaudaNumber).BiddingDate : DateTime.MinValue,
                        PendingQuantityCase = s.b.a.p.PendingQuantityInCase,
                        SaudaQuantityCase = s.b.a.p.SaudaQuantity,
                        SaudaValidToDate = s.b.a.p.ContractValidTo,
                        SaudaNumber = s.b.a.p.SaudaNumber,
                        BookedSku = s.b.sku.SkuName,
                        SaudaExtendedDays = s.saudaextension.ExtensionDays.ToString() + "days",
                        SaudaExtendedToDate = AddBusinessDays(s.b.a.p.ContractValidTo, s.saudaextension.ExtensionDays),
                        BasicRate = s.b.a.p.BasicRate,
                        DivisionId = s.b.a.p.DivisionId,
                        SalesOrganizationId = s.b.a.p.SalesOrgId,
                        DistributionChannelId = s.b.a.p.DistChnlId,
                        SkuCode = s.b.a.p.MaterialCode
                    }).Distinct().ToList();

                    var StateIds = pendingContractsContext.Join(UserContext, p => p.CustomerCode, u => u.Code, (p, u) => new { p, u })
                        .Join(skuContext, a => a.p.MaterialCode, sku => sku.SkuCode, (a, sku) => new { a, sku })
                        .Where(_ => dealerCodeContext.Contains(_.a.p.CustomerCode) && inputDto.OilTypeIds.Contains(_.sku.OilTypeId) && _.a.p.ContractValidTo.Value.Date <= previousDate &&
                                !StatesinSaudaExtensionContext.Contains(_.a.u.StateId)
                                //&& _.a.u.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess 
                                //&& _.sku.DivisionId == _.a.u.DivisionId
                                ).Select(a => a.a.u.StateId).Distinct().ToList();

                    StateList = StateContext.Where(_ => StateIds.Contains(_.Id)).Select(a => a.StateName).ToList();
                }

                HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => NotificationAsyncForStatesNotExtended(StateList, cancellationToken));
                var saudaBookedSaudaWithExtensionDetailsList = new List<SaudaBookedSaudaWithExtensionDetailsListDto>();
                if (BookedSaudaWithExtensionDetailsList != null && BookedSaudaWithExtensionDetailsList.Any())
                {
                    var saudaNumberList = BookedSaudaWithExtensionDetailsList.Select(s => s.SaudaNumber).Distinct().ToList();
                    foreach (var saudaNumber in saudaNumberList)
                    {
                        var se = BookedSaudaWithExtensionDetailsList.FirstOrDefault(_ => _.SaudaNumber == saudaNumber);
                        var saudaBookedSaudaWithExtensionDetails = new SaudaBookedSaudaWithExtensionDetailsListDto
                        {
                            SaudaOrderId = se.SaudaOrderId,
                            PendingContractId = se.PendingContractId,
                            SaudaBookedDate = se.SaudaBookedDate,
                            PendingQuantityCase = se.PendingQuantityCase,
                            SaudaQuantityCase = se.SaudaQuantityCase,
                            SaudaValidToDate = se.SaudaValidToDate,
                            SaudaNumber = se.SaudaNumber,
                            BookedSku = se.BookedSku,
                            SaudaExtendedDays = se.SaudaExtendedDays,
                            SaudaExtendedToDate = se.SaudaExtendedToDate,
                            BasicRate = se.BasicRate
                        };


                        var saudaContext = BookedSaudaWithExtensionDetailsList.Where(_ => _.SaudaNumber == saudaNumber);
                        if (saudaContext != null && saudaContext.Any())
                        {
                            var skuList = new List<SaudaExtensionSkuListDto>();
                            foreach (var item in saudaContext)
                            {
                                var skuDetails = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.SkuCode == item.SkuCode && _.SalesOrganizationId == item.SalesOrganizationId && _.DivisionId == item.DivisionId && _.DistributionChannelId == item.DistributionChannelId);
                                if (skuDetails != null)
                                {
                                    var Sku = new SaudaExtensionSkuListDto
                                    {
                                        SkuName = skuDetails.SkuName,
                                        SkuCode = skuDetails.SkuCode,
                                        SkuId = skuDetails.Id
                                    };
                                    saudaBookedSaudaWithExtensionDetails.SkuList.Add(Sku);
                                }
                            }
                        }


                        saudaBookedSaudaWithExtensionDetailsList.Add(saudaBookedSaudaWithExtensionDetails);


                    }
                }
                return SucessResult(saudaBookedSaudaWithExtensionDetailsList);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public DateTime? AddBusinessDays(DateTime? saudaValidToDate, long ExtendedDays)
        {

            for (int i = 1; i <= ExtendedDays; i++)
            {
                saudaValidToDate = saudaValidToDate.Value.AddDays(Constants.NumberOfDaysAddedToGetNextDate);
                if (saudaValidToDate.Value.DayOfWeek == DayOfWeek.Sunday)
                {
                    saudaValidToDate = saudaValidToDate.Value.AddDays(Constants.NumberOfDaysAddedToGetNextDate);
                }
            }
            return saudaValidToDate;
        }

        public void NotificationAsyncForStatesNotExtended(List<string> StateList, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
            if (StateList != null && StateList.Any())
            {
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(" Select Value as Email From Configurations");
                    sb.Append(" Where Name in (@Name)");
                    var mailIds = conn.QueryFirstOrDefault<string>(sb.ToString(),
                    new
                    {
                        Name = UtilityHelper.GetEnumDescription(DTO.Enums.Configuration.NotificationEmail)
                    });

                    if (mailIds != null && !string.IsNullOrEmpty(mailIds))
                    {
                        var mailIdsList = mailIds.Split(',').ToList();
                        mailIdsList.RemoveAll(x => string.IsNullOrEmpty(x));
                        sb.Clear();
                        sb.Append(" Select Template From EmailTemplates");
                        sb.Append(" Where Name in (@Name)");
                        var emailTemplate = conn.QueryFirstOrDefault<string>(sb.ToString(),
                        new
                        {
                            Name = "SaudaExtensionEmail"
                        });

                        string emailSubject = string.Empty;
                        var fromEmail = Constants.FromEmail;
                        var plainText = string.Empty;
                        emailSubject = "Sauda Extension";

                        sb.Clear();
                        sb.Append("<p>Dear Admin,The following states have not been mapped in the master screen(In Menu - Masters-> Extension Policy).So some of the Saudas can't be extended.</p><br>");
                        sb.Append("<p><br></p><div style='padding-bottom: 50px;'><table text-align=left border=1  width=100% align=center cellpadding=10 style='border-collapse:collapse'><tr><td><b><center>State</center></b></td></tr>");
                        foreach (var data in StateList)
                        {
                            sb.Append("<tr><td width=50% style='padding: 10px;'><center>" + data + "</center></td></tr>");
                        }
                        sb.Append("</table></div></p>");

                        var htmlTemplate = emailTemplate.Replace(Constants.ReplaceMainContent, sb.ToString());
                        amazonNotificationService.SendEmail(mailIdsList, emailSubject, plainText, htmlTemplate, true);
                    }
                }
            }
        }

        public ResultDto GetSaudaExtensionPendingAndApprovalList(SaudaExtensionFilterDto inputDto)
        {
            _methodName = "GetSaudaExtensionPendingAndApprovalList";
            try
            {
                var result = new SaudaExtensionPendingAndApprovedDto();
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.UserId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.ZonalHeadIsMissing);
                }

                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }
                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }
                var UserContext = _emamiContext.Users.AsNoTracking().ToList();
                var userIdContext = UserContext.FirstOrDefault(_ => _.Id == inputDto.UserId);
                if (userIdContext == null)
                {
                    return _resultService.ErrorMessage(Constants.ZonalHeadIsMissing);
                }

                var UserCustomerMappingContext = _emamiContext.UserCustomerMapping.AsNoTracking().ToList();
                //New Reporting to table change
                var BdoContext = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.UserId).Select(_ => _.UserId).ToList();
                //var BdoContext = UserContext.Where(_ => _.ReportingToId == inputDto.UserId).Select(a => a.Id).ToList();
                var dealerIdsContext = UserCustomerMappingContext.Where(_ => BdoContext.Contains(_.UserId)).Select(a => a.CustomerId).ToList();
                var dealerCodeContext = UserContext.Where(_ => dealerIdsContext.Contains(_.Id)).Select(a => a.Code).ToList();



                IEnumerable<DivisionDetailsDto> divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.UserId)
                 .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                var SaudaExtension = (from sed in _emamiContext.SaudaExtensionDetailsApprovals.AsNoTracking()
                                      join s in _emamiContext.Sauda.AsNoTracking() on sed.SaudaNumber equals s.SaudaNumber
                                      join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                                              equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                      join d in _emamiContext.Users.AsNoTracking() on sed.UserCode equals d.Code
                                      join ucm in _emamiContext.UserCustomerMapping.AsNoTracking() on d.Id equals ucm.CustomerId
                                      join bdo in _emamiContext.Users.AsNoTracking() on ucm.UserId equals bdo.Id
                                      join ur in _emamiContext.UserRoles.AsNoTracking() on ucm.UserId equals ur.UserId
                                      where dealerCodeContext.Contains(sed.UserCode)
                                         && DbFunctions.TruncateTime(sed.SaudaRequestDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                                         && DbFunctions.TruncateTime(sed.SaudaRequestDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                                         && ur.RoleId == (int)DTO.Enums.Role.StateTrader
                                      select new { SaudaExtension = sed, Dealer = d, UserCustomer = ucm, UserRole = ur, bdo }
                                    );

                //var SaudaExtension = _emamiContext.SaudaExtensionDetailsApprovals.AsNoTracking()
                //    .Join(_emamiContext.Users.AsNoTracking(), se => se.UserCode, d => d.Code, (se, d) => new { SaudaExtension = se,  Dealer = d })
                //    .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), se => se.Dealer.Id, uc => uc.CustomerId, (se, uc) => new { SaudaExtension = se.SaudaExtension,  Dealer = se.Dealer, UserCustomer = uc})
                //    .Join(_emamiContext.Users.AsNoTracking(), se =>se.UserCustomer.UserId,bdo=>bdo.Id,(se,bdo) => new { SaudaExtension = se.SaudaExtension,  se.Dealer,se.UserCustomer ,bdo})
                //    .Join(_emamiContext.UserRoles.AsNoTracking(), uc => uc.UserCustomer.UserId, ur => ur.UserId, (uc, ur) => new { SaudaExtension = uc.SaudaExtension, Dealer = uc.Dealer, UserCustomer = uc.UserCustomer, UserRole = ur,uc.bdo })
                //    .Where(_ => dealerCodeContext.Contains(_.SaudaExtension.UserCode) 
                //   && DbFunctions.TruncateTime(_.SaudaExtension.SaudaRequestDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                //    DbFunctions.TruncateTime(_.SaudaExtension.SaudaRequestDate) <= DbFunctions.TruncateTime(inputDto.ToDate) &&
                //     _.UserRole.RoleId == (int)DTO.Enums.Role.StateTrader);

                var saudaSkus = SaudaExtension.Select(s => s.SaudaExtension.SkuCode).ToList();
                var skucontext = _emamiContext.Skus.AsNoTracking()
                    .Join(_emamiContext.PendingContracts.AsNoTracking(), sku => sku.SkuCode, p => p.MaterialCode, (sku, p) => new { sku, p })
                    .Where(_ => saudaSkus.Contains(_.sku.SkuCode) && (_.p.SalesOrgId == _.sku.SalesOrganizationId && _.p.DistChnlId == _.sku.DistributionChannelId && _.p.DivisionId == _.sku.DivisionId)).Select(s => new
                    {
                        Id = s.sku.Id,
                        SaudaNumber = s.p.SaudaNumber,
                        SkuCode = s.sku.SkuCode,
                        SkuName = s.sku.SkuName
                    }).ToList();
                result.PendingList = SaudaExtension.AsEnumerable().Where(_ => !_.SaudaExtension.IsApproval)
                    .GroupBy(_ => _.Dealer.Id).Select(_ => new SaudaBookedListDto
                    {
                        DealerId = _.FirstOrDefault().Dealer.Id,
                        DealerName = _.FirstOrDefault().Dealer.Name,
                        DealerCode = _.FirstOrDefault().Dealer.Code,
                        SaudaBookedList = _.Where(g => g.Dealer.Id == _.Key).Select(s => new SaudaBookedSaudaWithExtensionDetailsListDto
                        {
                            BdoId = s.bdo.Id,
                            BdoName = s.bdo.Name,
                            DealerId = s.Dealer.Id,
                            SaudaNumber = (s.SaudaExtension != null) ? s.SaudaExtension.SaudaNumber : string.Empty,
                            SaudaValidToDate = DateTime.Parse(s.SaudaExtension.RequestDate),
                            SaudaValidFromDate = s.SaudaExtension.SaudaValidFrom,
                            SaudaExtendedDays = s.SaudaExtension.ExtentionDateCount,
                            SaudaRequestDate = s.SaudaExtension.RequestDate,
                            BasicRate = s.SaudaExtension.BasicRate,
                            SaudaQuantityInMt = s.SaudaExtension.SaudaQuantityMT,
                            //SaudaQuantityCase = s.SaudaExtension.SaudaQuantityCase,
                            PendingQuantityCase = s.SaudaExtension.PendingQuantityCase,
                            //PendingQuantityMT = s.SaudaExtension.PendingQuantityMT,
                            BookedSku = skucontext.FirstOrDefault(d => d.SkuCode == s.SaudaExtension.SaudaNumber).SkuName,
                            DealerName = s.Dealer.Name,
                            DealerAddress = s.Dealer.Address1,
                            IsApproval = s.SaudaExtension.IsApproval,
                        }).GroupBy(g => g.SaudaNumber)
                      .Select(s => new SaudaBookedSaudaWithExtensionDetailsListDto()
                      {
                          BdoId = s.FirstOrDefault().BdoId,
                          BdoName = s.FirstOrDefault().BdoName,
                          DealerId = s.FirstOrDefault().Id,
                          SaudaNumber = (s.FirstOrDefault() != null) ? s.FirstOrDefault().SaudaNumber : string.Empty,
                          SaudaValidToDate = s.FirstOrDefault().SaudaValidToDate,
                          SaudaValidFromDate = s.FirstOrDefault().SaudaValidFromDate,
                          SaudaExtendedDays = s.FirstOrDefault().SaudaExtendedDays,
                          SaudaRequestDate = s.FirstOrDefault().SaudaRequestDate,
                          BasicRate = s.FirstOrDefault().BasicRate,
                          SaudaQuantityInMt = s.FirstOrDefault().SaudaQuantityMT,
                          //SaudaQuantityCase = s.SaudaExtension.SaudaQuantityCase,
                          PendingQuantityCase = s.FirstOrDefault().PendingQuantityCase,
                          //PendingQuantityMT = s.SaudaExtension.PendingQuantityMT,
                          DealerName = s.FirstOrDefault().DealerName,
                          DealerAddress = s.FirstOrDefault().DealerAddress,
                          IsApproval = s.FirstOrDefault().IsApproval,
                          SkuList = skucontext.Where(sku => (s.FirstOrDefault() != null) ? sku.SaudaNumber == s.FirstOrDefault().SaudaNumber : sku.SaudaNumber == string.Empty).Select(sku => new SaudaExtensionSkuListDto()
                          {
                              SkuCode = sku.SkuCode,
                              SkuName = sku.SkuName,
                          }).GroupBy(d => d.SkuCode).Select(c => c.FirstOrDefault()).ToList()
                      }).ToList(),
                    }).ToList();
                result.ApprovedList = SaudaExtension.AsEnumerable().Where(_ => _.SaudaExtension.IsApproval)
                    .GroupBy(_ => (_.Dealer.Id)).Select(_ => new SaudaBookedListDto
                    {
                        DealerId = _.FirstOrDefault().Dealer.Id,
                        DealerName = _.FirstOrDefault().Dealer.Name,
                        DealerCode = _.FirstOrDefault().Dealer.Code,
                        SaudaBookedList = _.Where(g => g.Dealer.Id == _.Key).Select(s => new SaudaBookedSaudaWithExtensionDetailsListDto
                        {
                            BdoId = s.bdo.Id,
                            BdoName = s.bdo.Name,
                            DealerId = s.Dealer.Id,
                            SaudaNumber = (s.SaudaExtension != null) ? s.SaudaExtension.SaudaNumber : string.Empty,
                            SaudaValidToDate = DateTime.Parse(s.SaudaExtension.RequestDate),
                            SaudaValidFromDate = s.SaudaExtension.SaudaValidFrom,
                            SaudaExtendedDays = s.SaudaExtension.ExtentionDateCount,
                            SaudaRequestDate = s.SaudaExtension.RequestDate,
                            BasicRate = s.SaudaExtension.BasicRate,
                            SaudaQuantityInMt = s.SaudaExtension.SaudaQuantityMT,
                            SaudaQuantityCase = s.SaudaExtension.SaudaQuantityCase,
                            PendingQuantityCase = s.SaudaExtension.PendingQuantityCase,
                            PendingQuantityMT = s.SaudaExtension.PendingQuantityMT,
                            BookedSku = skucontext.FirstOrDefault(d => d.SkuCode == s.SaudaExtension.SkuCode).SkuName,
                            DealerName = s.Dealer.Name,
                            DealerAddress = s.Dealer.Address1,
                            IsApproval = s.SaudaExtension.IsApproval,
                        }).GroupBy(g => g.SaudaNumber).Select(s => new SaudaBookedSaudaWithExtensionDetailsListDto()
                        {
                            BdoId = (s.FirstOrDefault() != null) ? s.FirstOrDefault().BdoId : 0,
                            BdoName = (s.FirstOrDefault() != null) ? s.FirstOrDefault().BdoName : string.Empty,
                            DealerId = (s.FirstOrDefault() != null) ? s.FirstOrDefault().Id : 0,
                            SaudaNumber = (s.FirstOrDefault() != null) ? s.FirstOrDefault().SaudaNumber : string.Empty,
                            SaudaValidToDate = (s.FirstOrDefault() != null) ? s.FirstOrDefault().SaudaValidToDate : DateTime.Now,
                            SaudaValidFromDate = (s.FirstOrDefault() != null) ? s.FirstOrDefault().SaudaValidFromDate : DateTime.Now,
                            SaudaExtendedDays = (s.FirstOrDefault() != null) ? s.FirstOrDefault().SaudaExtendedDays : string.Empty,
                            SaudaRequestDate = (s.FirstOrDefault() != null) ? s.FirstOrDefault().SaudaRequestDate : string.Empty,
                            BasicRate = (s.FirstOrDefault() != null) ? s.FirstOrDefault().BasicRate : 0,
                            SaudaQuantityInMt = (s.FirstOrDefault() != null) ? s.FirstOrDefault().SaudaQuantityMT : 0,
                            //SaudaQuantityCase = s.SaudaExtension.SaudaQuantityCase,
                            PendingQuantityCase = (s.FirstOrDefault() != null) ? s.FirstOrDefault().PendingQuantityCase : 0,
                            //PendingQuantityMT = s.SaudaExtension.PendingQuantityMT,
                            DealerName = (s.FirstOrDefault() != null) ? s.FirstOrDefault().DealerName : string.Empty,
                            DealerAddress = (s.FirstOrDefault() != null) ? s.FirstOrDefault().DealerAddress : string.Empty,
                            IsApproval = (s.FirstOrDefault() != null) ? s.FirstOrDefault().IsApproval : false,
                            SkuList = skucontext.Where(sku => (s.FirstOrDefault() != null) ? sku.SaudaNumber == s.FirstOrDefault().SaudaNumber : sku.SaudaNumber == string.Empty).Select(sku => new SaudaExtensionSkuListDto()
                            {
                                SkuCode = sku.SkuCode,
                                SkuName = sku.SkuName,
                            }).GroupBy(d => d.SkuCode).Select(c => c.FirstOrDefault()).ToList()
                        }
                        ).ToList(),
                    }).ToList();

                //result.PendingList = SaudaExtension.Where(_ => !_.SaudaExtension.IsApproval).Select(a => new SaudaBookedSaudaWithExtensionDetailsListDto
                //{
                //    SaudaNumber = a.SaudaExtension.SaudaNumber,
                //    SaudaValidToDate = a.SaudaExtension.SaudaValidTo,
                //    SaudaValidFromDate = a.SaudaExtension.SaudaValidFrom,
                //    SaudaExtendedDays = a.SaudaExtension.ExtentionDateCount,
                //    SaudaRequestDate = a.SaudaExtension.RequestDate,
                //    BasicRate = a.SaudaExtension.BasicRate,
                //    DealerName = a.Dealer.Name,
                //    DealerId = a.Dealer.Id,
                //    DealerAddress = a.Dealer.Address1,
                //    BdoName = a.UserCustomer.User.Name,
                //    BdoId = a.UserCustomer.User.Id,
                //    BdoAddress = a.UserCustomer.User.Address1,
                //    SaudaQuantityCase = a.SaudaExtension.SaudaQuantityCase,
                //    SaudaQuantityInMt = a.SaudaExtension.SaudaQuantityMT,
                //    PendingQuantityCase = a.SaudaExtension.PendingQuantityCase,
                //    PendingQuantityMT = a.SaudaExtension.PendingQuantityMT,
                //    BookedSku = a.Skus.SkuName
                //}).ToList();

                //result.ApprovedList = SaudaExtension.Where(_ => _.SaudaExtension.IsApproval).Select(a => new SaudaBookedSaudaWithExtensionDetailsListDto
                //{
                //    SaudaNumber = a.SaudaExtension.SaudaNumber,
                //    SaudaValidToDate = a.SaudaExtension.SaudaValidTo,
                //    SaudaValidFromDate = a.SaudaExtension.SaudaValidFrom,
                //    SaudaExtendedDays = a.SaudaExtension.ExtentionDateCount,
                //    SaudaRequestDate = a.SaudaExtension.RequestDate,
                //    BasicRate = a.SaudaExtension.BasicRate,
                //    DealerName = a.Dealer.Name,
                //    DealerId = a.Dealer.Id,
                //    DealerAddress = a.Dealer.Address1,
                //    BdoName = a.UserCustomer.User.Name,
                //    BdoId = a.UserCustomer.User.Id,
                //    BdoAddress = a.UserCustomer.User.Address1,
                //    SaudaQuantityCase = a.SaudaExtension.SaudaQuantityCase,
                //    SaudaQuantityInMt = a.SaudaExtension.SaudaQuantityMT,
                //    PendingQuantityCase = a.SaudaExtension.PendingQuantityCase,
                //    PendingQuantityMT = a.SaudaExtension.PendingQuantityMT,
                //    BookedSku = a.Skus.SkuName
                //}).ToList();
                return SucessResult(result);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto GetsaudaExtensionDetailsInWeb(SaudaExtensionFilterDtoForGrid inputDto)
        {
            _methodName = "GetsaudaExtensionDetailsInWeb";
            var resultDto = new ResultDto();
            DataSourceResult result = new DataSourceResult();

            try
            {
                if (inputDto.statusId == (int)DTO.Enums.Status.Pending)
                {
                    result = _emamiContext.SaudaExtensionDetailsApprovals.AsNoTracking()
                        .Join(_emamiContext.Skus.AsNoTracking(), se => se.SkuCode, sk => sk.SkuCode, (se, sk) => new { SaudaExtension = se, Skus = sk })
                        .Join(_emamiContext.Users.AsNoTracking(), se => se.SaudaExtension.UserCode, d => d.Code, (se, d) => new { SaudaExtension = se.SaudaExtension, Skus = se.Skus, Dealer = d })
                        .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), se => se.Dealer.Id, uc => uc.CustomerId, (se, uc) => new { SaudaExtension = se.SaudaExtension, Skus = se.Skus, Dealer = se.Dealer, UserCustomer = uc })
                        .Join(_emamiContext.Users.AsNoTracking(), se => se.UserCustomer.User.ReportingToId, uc => uc.Id, (se, uc) => new { SaudaExtension = se.SaudaExtension, Skus = se.Skus, Dealer = se.Dealer, UserCustomer = se.UserCustomer, ZonalTrader = uc })
                        .Where(_ => !_.SaudaExtension.IsApproval
                        && _.Dealer.SaudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess
                        && (DbFunctions.TruncateTime(_.SaudaExtension.CreatedDate) >= inputDto.ValidFrom
                        && DbFunctions.TruncateTime(_.SaudaExtension.CreatedDate) <= inputDto.ValidTo)
                        //&& _.Skus.DivisionId == _.Dealer.DivisionId
                        )
                        .Select(s => new SaudaBookedSaudaWithExtensionDetailsListDto
                        {
                            SaudaOrderId = s.SaudaExtension.SaudaOrderId,
                            SaudaNumber = s.SaudaExtension.SaudaNumber,
                            SaudaValidToDate = s.SaudaExtension.SaudaValidTo,
                            SaudaValidFromDate = s.SaudaExtension.SaudaValidFrom,
                            SaudaExtendedDays = s.SaudaExtension.ExtentionDateCount,
                            SaudaRequestDate = s.SaudaExtension.RequestDate,
                            BasicRate = s.SaudaExtension.BasicRate,
                            SaudaQuantityInMt = s.SaudaExtension.SaudaQuantityMT,
                            SaudaQuantityMT = s.SaudaExtension.SaudaQuantityMT,
                            SaudaQuantityCase = s.SaudaExtension.SaudaQuantityCase,
                            PendingQuantityCase = s.SaudaExtension.PendingQuantityCase,
                            PendingQuantityMT = s.SaudaExtension.PendingQuantityMT,
                            BookedSku = s.Skus.SkuName,
                            DealerName = s.Dealer.Name,
                            Remarks = s.SaudaExtension.Remarks,
                            SAPRemarks = s.SaudaExtension.SAPRemarks,
                            IsApproval = s.SaudaExtension.IsApproval,
                            BdoName = s.UserCustomer.User.Name,
                            zonalHeadName = s.ZonalTrader.Name
                        }).ToDataSourceResult(inputDto.DataSourceRequest);
                }
                else
                {
                    result = _emamiContext.SaudaExtensionDetailsApprovals.AsNoTracking()
                             .Join(_emamiContext.Skus.AsNoTracking(), se => se.SkuCode, sk => sk.SkuCode, (se, sk) => new { SaudaExtension = se, Skus = sk })
                             .Join(_emamiContext.Users.AsNoTracking(), se => se.SaudaExtension.UserCode, d => d.Code, (se, d) => new { SaudaExtension = se.SaudaExtension, Skus = se.Skus, Dealer = d })
                             .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), se => se.Dealer.Id, uc => uc.CustomerId, (se, uc) => new { SaudaExtension = se.SaudaExtension, Skus = se.Skus, Dealer = se.Dealer, UserCustomer = uc })
                             .Join(_emamiContext.Users.AsNoTracking(), se => se.UserCustomer.User.ReportingToId, uc => uc.Id, (se, uc) => new { SaudaExtension = se.SaudaExtension, Skus = se.Skus, Dealer = se.Dealer, UserCustomer = se.UserCustomer, ZonalTrader = uc })
                             .Where(_ => _.SaudaExtension.IsApproval
                             && _.Dealer.SaudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess
                             && (DbFunctions.TruncateTime(_.SaudaExtension.CreatedDate) >= inputDto.ValidFrom
                             && DbFunctions.TruncateTime(_.SaudaExtension.CreatedDate) <= inputDto.ValidTo)
                             //&& _.Skus.DivisionId == _.Dealer.DivisionId
                             )
                             .Select(s => new SaudaBookedSaudaWithExtensionDetailsListDto
                             {
                                 SaudaOrderId = s.SaudaExtension.SaudaOrderId,
                                 SaudaNumber = s.SaudaExtension.SaudaNumber,
                                 SaudaValidToDate = s.SaudaExtension.SaudaValidTo,
                                 SaudaValidFromDate = s.SaudaExtension.SaudaValidFrom,
                                 SaudaExtendedDays = s.SaudaExtension.ExtentionDateCount,
                                 SaudaRequestDate = s.SaudaExtension.RequestDate,
                                 BasicRate = s.SaudaExtension.BasicRate,
                                 SaudaQuantityInMt = s.SaudaExtension.SaudaQuantityMT,
                                 SaudaQuantityMT = s.SaudaExtension.SaudaQuantityMT,
                                 SaudaQuantityCase = s.SaudaExtension.SaudaQuantityCase,
                                 PendingQuantityCase = s.SaudaExtension.PendingQuantityCase,
                                 PendingQuantityMT = s.SaudaExtension.PendingQuantityMT,
                                 BookedSku = s.Skus.SkuName,
                                 DealerName = s.Dealer.Name,
                                 Remarks = s.SaudaExtension.Remarks,
                                 SAPRemarks = s.SaudaExtension.SAPRemarks,
                                 IsApproval = s.SaudaExtension.IsApproval,
                                 BdoName = s.UserCustomer.User.Name,
                                 zonalHeadName = s.ZonalTrader.Name
                             }).ToDataSourceResult(inputDto.DataSourceRequest);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = result;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetSaudaExtensionPendingAndApprovalListForBdo(SaudaExtensionFilterDto inputDto)
        {
            _methodName = "GetSaudaExtensionPendingAndApprovalListForBdo";
            try
            {
                var result = new SaudaExtensionPendingAndApprovedDto();
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.UserId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.SalesPersonMissing);
                }
                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }
                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }
                var UserContext = _emamiContext.Users.AsNoTracking().ToList();
                var userIdContext = UserContext.FirstOrDefault(_ => _.Id == inputDto.UserId);
                if (userIdContext == null)
                {
                    return _resultService.ErrorMessage(Constants.SalesPersonMissing);
                }

                var UserCustomerMappingContext = _emamiContext.UserCustomerMapping.AsNoTracking().ToList();
                var dealerIdsContext = UserCustomerMappingContext.Where(_ => _.UserId == inputDto.UserId).Select(a => a.CustomerId).ToList();
                var dealerCodeContext = UserContext.Where(_ => dealerIdsContext.Contains(_.Id)).Select(a => a.Code).ToList();

                IEnumerable<DivisionDetailsDto> divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.UserId)
                 .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });


                var saudaExtensionDetailsApprovalsContext = (from sed in _emamiContext.SaudaExtensionDetailsApprovals.AsNoTracking()
                                                             join s in _emamiContext.Sauda.AsNoTracking() on sed.SaudaNumber equals s.SaudaNumber
                                                             join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                                                              equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                                             join u in _emamiContext.Users.AsNoTracking() on sed.UserCode equals u.Code
                                                             where dealerCodeContext.Contains(sed.UserCode)
                                                             && DbFunctions.TruncateTime(sed.SaudaRequestDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                                                             && DbFunctions.TruncateTime(sed.SaudaRequestDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                                                             select new { sed, u }
                                                           );

                //var saudaExtensionDetailsApprovalsContext = _emamiContext.SaudaExtensionDetailsApprovals.AsNoTracking()
                //    //.Join(_emamiContext.Skus.AsNoTracking(), se => se.SkuCode, sk => sk.SkuCode, (se, sk) => new { SaudaExtension = se, Skus = sk })
                //    .Join(_emamiContext.Users.AsNoTracking(), se => se.UserCode, d => d.Code, (se, d) => new { SaudaExtension = se,  Dealer = d })
                //    .Where(_ => dealerCodeContext.Contains(_.SaudaExtension.UserCode)
                //    //_.Dealer.SaudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess 
                //    //&& _.Skus.DivisionId == _.Dealer.DivisionId 
                //    && DbFunctions.TruncateTime(_.SaudaExtension.SaudaRequestDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                //    DbFunctions.TruncateTime(_.SaudaExtension.SaudaRequestDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                //    //&& _.Dealer.DivisionId == userIdContext.DivisionId
                //    );

                //result.PendingList = saudaExtensionDetailsApprovalsContext.Where(_ => !_.SaudaExtension.IsApproval)
                //    .Select(a => new SaudaBookedSaudaWithExtensionDetailsListDto
                //{
                //    SaudaNumber = a.SaudaExtension.SaudaNumber,
                //    SaudaValidToDate = a.SaudaExtension.SaudaValidTo,
                //    SaudaValidFromDate = a.SaudaExtension.SaudaValidFrom,
                //    SaudaExtendedDays = a.SaudaExtension.ExtentionDateCount,
                //    SaudaRequestDate = a.SaudaExtension.RequestDate,
                //    BasicRate = a.SaudaExtension.BasicRate,
                //    SaudaQuantityInMt = a.SaudaExtension.SaudaQuantityMT,
                //    SaudaQuantityCase = a.SaudaExtension.SaudaQuantityCase,
                //    PendingQuantityCase = a.SaudaExtension.PendingQuantityCase,
                //    PendingQuantityMT = a.SaudaExtension.PendingQuantityMT,
                //    BookedSku = a.Skus.SkuName,
                //    DealerName = a.Dealer.Name,
                //    DealerId = a.Dealer.Id,
                //    DealerAddress = a.Dealer.Address1
                //}).ToList();

                //var ss = saudaExtensionDetailsApprovalsContext.Where(_ => !_.SaudaExtension.IsApproval) != null
                //    && saudaExtensionDetailsApprovalsContext.Where(_ => !_.SaudaExtension.IsApproval).Any() ?
                //    saudaExtensionDetailsApprovalsContext.Where(_ => !_.SaudaExtension.IsApproval).ToList().GroupBy(_ => _.Dealer.Id);
                var saudaSkus = saudaExtensionDetailsApprovalsContext.Select(s => s.sed.SkuCode).ToList();
                var skucontext = _emamiContext.Skus.AsNoTracking().Where(_ => saudaSkus.Contains(_.SkuCode)).Select(s => new
                {
                    SkuCode = s.SkuCode,
                    SkuName = s.SkuName
                }).ToList();
                result.PendingList = saudaExtensionDetailsApprovalsContext.AsEnumerable().Where(_ => !_.sed.IsApproval)
                    .GroupBy(_ => _.u.Id).Select(_ => new SaudaBookedListDto
                    {
                        DealerId = _.FirstOrDefault().u.Id,
                        DealerName = _.FirstOrDefault().u.Name,
                        DealerCode = _.FirstOrDefault().u.Code,
                        SaudaBookedList = _.Where(g => g.u.Id == _.Key).Select(s => new SaudaBookedSaudaWithExtensionDetailsListDto
                        {
                            DealerId = s.u.Id,
                            SaudaNumber = (s.sed != null) ? s.sed.SaudaNumber : string.Empty,
                            SaudaValidToDate = DateTime.Parse(s.sed.RequestDate),
                            SaudaValidFromDate = s.sed.SaudaValidFrom,
                            SaudaExtendedDays = s.sed.ExtentionDateCount,
                            SaudaRequestDate = s.sed.RequestDate,
                            BasicRate = s.sed.BasicRate,
                            SaudaQuantityInMt = s.sed.SaudaQuantityMT,
                            SaudaQuantityCase = s.sed.SaudaQuantityCase,
                            PendingQuantityCase = s.sed.PendingQuantityCase,
                            PendingQuantityMT = s.sed.PendingQuantityMT,
                            BookedSku = skucontext.FirstOrDefault(d => d.SkuCode == s.sed.SkuCode).SkuName,
                            DealerName = s.u.Name,
                            DealerAddress = s.u.Address1,
                            IsApproval = s.sed.IsApproval,
                        }).ToList(),
                    }).ToList();

                result.ApprovedList = saudaExtensionDetailsApprovalsContext.AsEnumerable().Where(_ => _.sed.IsApproval)
                    .GroupBy(_ => _.u.Id).Select(_ => new SaudaBookedListDto
                    {
                        DealerId = _.FirstOrDefault().u.Id,
                        DealerName = _.FirstOrDefault().u.Name,
                        DealerCode = _.FirstOrDefault().u.Code,
                        SaudaBookedList = _.Where(g => g.u.Id == _.Key).Select(s => new SaudaBookedSaudaWithExtensionDetailsListDto
                        {
                            DealerId = s.u.Id,
                            SaudaNumber = (s.sed != null) ? s.sed.SaudaNumber : string.Empty,
                            SaudaValidToDate = DateTime.Parse(s.sed.RequestDate),
                            SaudaValidFromDate = s.sed.SaudaValidFrom,
                            SaudaExtendedDays = s.sed.ExtentionDateCount,
                            SaudaRequestDate = s.sed.RequestDate,
                            BasicRate = s.sed.BasicRate,
                            SaudaQuantityInMt = s.sed.SaudaQuantityMT,
                            SaudaQuantityCase = s.sed.SaudaQuantityCase,
                            PendingQuantityCase = s.sed.PendingQuantityCase,
                            PendingQuantityMT = s.sed.PendingQuantityMT,
                            BookedSku = skucontext.FirstOrDefault(d => d.SkuCode == s.sed.SkuCode).SkuName,
                            DealerName = s.u.Name,
                            DealerAddress = s.u.Address1,
                            IsApproval = s.sed.IsApproval,
                        }).ToList(),
                    }).ToList();

                return SucessResult(result);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }
        public ResultDto GetSaudaExtensionPendingAndApprovalListForDealer(SaudaExtensionFilterDto inputDto)
        {
            _methodName = "GetSaudaExtensionPendingAndApprovalListForDealer";
            try
            {
                var result = new SaudaExtensionPendingAndApprovedListDto();
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.UserId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.DealerIdEmpty);
                }
                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }
                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }
                var userIdContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.UserId);
                if (userIdContext == null)
                {
                    return _resultService.ErrorMessage(Constants.DealerMissing);
                }

                var dealerCodeContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == inputDto.UserId).Select(a => a.Code).ToList();
                var saudaExtensionDetailsApprovalsContext = _emamiContext.SaudaExtensionDetailsApprovals.AsNoTracking()
                    //.Join(_emamiContext.Skus.AsNoTracking(), se => se.SkuCode, sk => sk.SkuCode, (se, sk) => new { SaudaExtension = se, Skus = sk })
                    .Join(_emamiContext.Users.AsNoTracking(), se => se.UserCode, d => d.Code, (se, d) => new { SaudaExtension = se, Dealer = d })
                    .Where(_ => dealerCodeContext.Contains(_.SaudaExtension.UserCode) &&
                    //&& _.Dealer.SaudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess
                    //&& _.Skus.DivisionId == _.Dealer.DivisionId 
                    DbFunctions.TruncateTime(_.SaudaExtension.SaudaRequestDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                    DbFunctions.TruncateTime(_.SaudaExtension.SaudaRequestDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    //&& _.Dealer.DivisionId == userIdContext.DivisionId
                    );
                var saudaSkus = saudaExtensionDetailsApprovalsContext.Select(s => s.SaudaExtension.SkuCode).ToList();
                var skucontext = _emamiContext.Skus.AsNoTracking().Where(_ => saudaSkus.Contains(_.SkuCode)).Select(s => new
                {
                    SkuCode = s.SkuCode,
                    SkuName = s.SkuName
                }).ToList();
                result.PendingList = saudaExtensionDetailsApprovalsContext.AsEnumerable().Where(_ => !_.SaudaExtension.IsApproval).Select(a => new SaudaBookedSaudaWithExtensionDetailsListDto
                {
                    SaudaNumber = a.SaudaExtension.SaudaNumber,
                    SaudaValidToDate = DateTime.Parse(a.SaudaExtension.RequestDate),
                    SaudaValidFromDate = a.SaudaExtension.SaudaValidFrom,
                    SaudaExtendedDays = a.SaudaExtension.ExtentionDateCount,
                    SaudaRequestDate = a.SaudaExtension.RequestDate,
                    BasicRate = a.SaudaExtension.BasicRate,
                    SaudaQuantityInMt = a.SaudaExtension.SaudaQuantityMT,
                    SaudaQuantityCase = a.SaudaExtension.SaudaQuantityCase,
                    PendingQuantityCase = a.SaudaExtension.PendingQuantityCase,
                    PendingQuantityMT = a.SaudaExtension.PendingQuantityMT,
                    BookedSku = skucontext.FirstOrDefault(_ => _.SkuCode == a.SaudaExtension.SkuCode).SkuName,
                }).ToList();

                result.ApprovedList = saudaExtensionDetailsApprovalsContext.AsEnumerable().Where(_ => _.SaudaExtension.IsApproval).Select(a => new SaudaBookedSaudaWithExtensionDetailsListDto
                {
                    SaudaNumber = a.SaudaExtension.SaudaNumber,
                    SaudaValidToDate = DateTime.Parse(a.SaudaExtension.RequestDate),
                    SaudaValidFromDate = a.SaudaExtension.SaudaValidFrom,
                    SaudaExtendedDays = a.SaudaExtension.ExtentionDateCount,
                    SaudaRequestDate = a.SaudaExtension.RequestDate,
                    SaudaQuantityInMt = a.SaudaExtension.SaudaQuantityMT,
                    SaudaQuantityCase = a.SaudaExtension.SaudaQuantityCase,
                    PendingQuantityCase = a.SaudaExtension.PendingQuantityCase,
                    PendingQuantityMT = a.SaudaExtension.PendingQuantityMT,
                    BasicRate = a.SaudaExtension.BasicRate,
                    BookedSku = skucontext.FirstOrDefault(_ => _.SkuCode == a.SaudaExtension.SkuCode).SkuName,
                }).ToList();
                return SucessResult(result);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }
        #endregion

        /// <summary>
        /// Method to get sauda list for admin app
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetSaudaListForAdminApp(SaudaListAdminAppFilterDto saudaFilterDto)
        {
            _methodName = "GetSaudaListForAdminApp";
            var resultDto = new ResultDto();
            var outputDto = new SaudaListsDto();
            outputDto.SaudaList = new List<SaudaListDto>();
            try
            {
                if (saudaFilterDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                if (saudaFilterDto.LoginUserId <= 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.DealerIdEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.DealerIdEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }

                if (saudaFilterDto.FromDate == null || saudaFilterDto.FromDate == DateTime.MinValue)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.FromDateEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.FromDateEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (saudaFilterDto.ToDate == null || saudaFilterDto.ToDate == DateTime.MinValue)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.ToDateEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.ToDateEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }

                IEnumerable<dynamic> saudaListContext;
                var bdoList = (from u in _emamiContext.Users.AsNoTracking()
                               join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                               where ur.RoleId == (int)DTO.Enums.Role.StateTrader
                               select new
                               {
                                   Username = u.Name,
                                   UserId = u.Id,
                                   RoleId = ur.RoleId,
                                   Code = u.Code
                               }).ToList();

                var dealerList = (from u in _emamiContext.Users.AsNoTracking()
                                  join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                                  join uc in _emamiContext.UserCustomerMapping.AsNoTracking() on u.Id equals uc.CustomerId
                                  where ur.RoleId == (int)DTO.Enums.Role.Dealer
                                  select new
                                  {
                                      Username = u.Name,
                                      UserId = u.Id,
                                      RoleId = ur.RoleId,
                                      CustometId = uc.UserId
                                  }).ToList();

                var bdoNewList = (from d in dealerList
                                  join StateTrader in bdoList on d.CustometId equals StateTrader.UserId
                                  select new
                                  {
                                      BDOName = StateTrader.Username,
                                      BdoId = StateTrader.UserId,
                                      CustomerId = d.UserId,
                                      BDOCode = StateTrader.Code,
                                  }).ToList();

                var dealerIds = new List<long>();
                //Dealer 
                if (saudaFilterDto.DealerIds.IsAny())
                {
                    dealerIds = saudaFilterDto.DealerIds;
                }
                //StateTrader
                else if (saudaFilterDto.DealerIds == null && saudaFilterDto.BdoIds.IsAny())
                {
                    dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(usercustomer => saudaFilterDto.BdoIds.Contains(usercustomer.UserId)).Select(customer => customer.CustomerId).ToList();
                }
                //ZonalTrader
                else if (saudaFilterDto.DealerIds == null && saudaFilterDto.BdoIds == null)
                {
                    var bdoIds = _emamiContext.Users.AsNoTracking().Where(user => user.ReportingToId == saudaFilterDto.LoginUserId).Select(a => a.Id).ToList();
                    dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(usercustomer => bdoIds.Contains(usercustomer.UserId)).Select(customer => customer.CustomerId).ToList();
                }
                else if (saudaFilterDto.DealerIds == null && saudaFilterDto.BdoIds == null && saudaFilterDto.ZonalHeadIds.IsAny())
                {
                    var bdoIds = _emamiContext.Users.AsNoTracking().Where(user => saudaFilterDto.ZonalHeadIds.Contains(user.ReportingToId ?? 0)).Select(a => a.Id).ToList();
                    dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(usercustomer => bdoIds.Contains(usercustomer.UserId)).Select(customer => customer.CustomerId).ToList();
                }
                else if (saudaFilterDto.DealerIds == null && saudaFilterDto.BdoIds == null && saudaFilterDto.ZonalHeadIds == null && saudaFilterDto.NationalHeadIds.IsAny())
                {
                    var zonalHeadIds = _emamiContext.Users.AsNoTracking().Where(user => saudaFilterDto.NationalHeadIds.Contains(user.ReportingToId ?? 0)).Select(a => a.Id).ToList();
                    var bdoIds = _emamiContext.Users.AsNoTracking().Where(user => zonalHeadIds.Contains(user.ReportingToId ?? 0)).Select(a => a.Id).ToList();
                    dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(usercustomer => bdoIds.Contains(usercustomer.UserId)).Select(customer => customer.CustomerId).ToList();
                }
                else if (saudaFilterDto.DealerIds == null && saudaFilterDto.BdoIds == null && saudaFilterDto.ZonalHeadIds == null && saudaFilterDto.NationalHeadIds == null)
                {
                    var nationalHeadIds = _emamiContext.UserRoles.AsNoTracking().Where(roles => roles.RoleId == (int)DTO.Enums.Role.NationalTrader).Select(a => a.UserId).ToList();
                    var zonalHeadIds = _emamiContext.Users.AsNoTracking().Where(user => nationalHeadIds.Contains(user.ReportingToId ?? 0)).Select(a => a.Id).ToList();
                    var bdoIds = _emamiContext.Users.AsNoTracking().Where(user => zonalHeadIds.Contains(user.ReportingToId ?? 0)).Select(a => a.Id).ToList();
                    dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(usercustomer => bdoIds.Contains(usercustomer.UserId)).Select(customer => customer.CustomerId).ToList();
                }

                var pageSize = Constants.PageSize;
                long skip = pageSize * saudaFilterDto.PageNo;

                if (saudaFilterDto.StatusId > 0)
                {
                    if (saudaFilterDto.SaudaBookingTypeId > 0)
                    {
                        var saudaList = _emamiContext.Sauda.AsNoTracking()
                           .Join(_emamiContext.SaudaOrders.AsNoTracking(), s => s.Id, so => so.SaudaId, (s, so) => new { Sauda = s, SaudaOrders = so })
                           .Join(_emamiContext.ApprovalStatus.AsNoTracking(), x => x.SaudaOrders.StatusId, a => a.Id, (x, a) => new { x.SaudaOrders, x.Sauda, ApprovalStatus = a.Name })
                           .Join(_emamiContext.Depots.AsNoTracking(), x => x.SaudaOrders.PlantId, p => p.Id, (x, p) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, Depots = p.Name })
                           //.Join(_emamiContext.FreightRoutes.AsNoTracking(), x => x.SaudaOrders.DealerLocationId, f => f.Id, (x, f) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, FreightRoutes = f.Name })
                           .Join(_emamiContext.Users.AsNoTracking(), x => x.Sauda.UserId, u => u.Id, (x, u) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, DealerName = u.Name, DealerCode = u.Code, StateId = u.StateId })
                           .Join(_emamiContext.State.AsNoTracking(), x => x.StateId, s => s.Id, (x, s) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.DealerCode, x.StateId, StateName = s.StateName })
                           .Join(_emamiContext.Users.AsNoTracking(), x => x.Sauda.CreatedBy, u => u.Id, (x, u) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, CreatedByName = u.Name, x.DealerCode, x.StateName })
                           .Join(_emamiContext.IncoTerms.AsNoTracking(), x => x.SaudaOrders.Incoterms2, i => i.Id, (x, i) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.CreatedByName, IncoTermsName = i.Name, x.DealerCode, x.StateName })
                           .Join(_emamiContext.SkuUomMapping.AsNoTracking().Where(_ => _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos), x => x.SaudaOrders.SkuId, s => s.SkuId, (x, s) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.CreatedByName, x.IncoTermsName, NoOfSkusPerCase = s.ConversionFactor, x.DealerCode, x.StateName })
                           .Join(_emamiContext.Pricing.AsNoTracking(), x => x.SaudaOrders.PricingId, s => s.Id, (x, Pricing) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.CreatedByName, x.IncoTermsName, x.NoOfSkusPerCase, Pricing, x.DealerCode, x.StateName })
                           .Where(w => (DbFunctions.TruncateTime(w.Sauda.BiddingDate) >= DbFunctions.TruncateTime(saudaFilterDto.FromDate) && dealerIds.Contains(w.Sauda.UserId) &&
                           DbFunctions.TruncateTime(w.Sauda.BiddingDate) <= DbFunctions.TruncateTime(saudaFilterDto.ToDate)) && w.SaudaOrders.StatusId == saudaFilterDto.StatusId
                           && w.SaudaOrders.SaudaBookingTypeId == saudaFilterDto.SaudaBookingTypeId && (saudaFilterDto.VerticalId > 0 ? w.SaudaOrders.OilType.DivisionId == saudaFilterDto.VerticalId : w.SaudaOrders.OilType.DivisionId > 0))
                          .OrderByDescending(_ => _.SaudaOrders.Id);
                        outputDto.ListCount = saudaList.Count();
                        saudaListContext = saudaList.Skip((int)skip).Take(pageSize).ToList();
                    }
                    else
                    {
                        var saudaList = _emamiContext.Sauda.AsNoTracking()
                        .Join(_emamiContext.SaudaOrders.AsNoTracking(), s => s.Id, so => so.SaudaId, (s, so) => new { Sauda = s, SaudaOrders = so })
                        .Join(_emamiContext.ApprovalStatus.AsNoTracking(), x => x.SaudaOrders.StatusId, a => a.Id, (x, a) => new { x.SaudaOrders, x.Sauda, ApprovalStatus = a.Name })
                        .Join(_emamiContext.Depots.AsNoTracking(), x => x.SaudaOrders.PlantId, p => p.Id, (x, p) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, Depots = p.Name })
                        //.Join(_emamiContext.FreightRoutes.AsNoTracking(), x => x.SaudaOrders.DealerLocationId, f => f.Id, (x, f) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, FreightRoutes = f.Name })
                        .Join(_emamiContext.Users.AsNoTracking(), x => x.Sauda.UserId, u => u.Id, (x, u) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, DealerName = u.Name, DealerCode = u.Code, StateId = u.StateId })
                        .Join(_emamiContext.State.AsNoTracking(), x => x.StateId, s => s.Id, (x, s) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.DealerCode, x.StateId, StateName = s.StateName })
                        .Join(_emamiContext.Users.AsNoTracking(), x => x.Sauda.CreatedBy, u => u.Id, (x, u) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, CreatedByName = u.Name, x.DealerCode, x.StateName })
                        .Join(_emamiContext.IncoTerms.AsNoTracking(), x => x.SaudaOrders.Incoterms2, i => i.Id, (x, i) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.CreatedByName, IncoTermsName = i.Name, x.DealerCode, x.StateName })
                        .Join(_emamiContext.SkuUomMapping.AsNoTracking().Where(_ => _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos), x => x.SaudaOrders.SkuId, s => s.SkuId, (x, s) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.CreatedByName, x.IncoTermsName, NoOfSkusPerCase = s.ConversionFactor, x.DealerCode, x.StateName })
                        .Join(_emamiContext.Pricing.AsNoTracking(), x => x.SaudaOrders.PricingId, s => s.Id, (x, Pricing) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.CreatedByName, x.IncoTermsName, x.NoOfSkusPerCase, Pricing, x.DealerCode, x.StateName })
                        .Where(w => (DbFunctions.TruncateTime(w.Sauda.BiddingDate) >= DbFunctions.TruncateTime(saudaFilterDto.FromDate) && dealerIds.Contains(w.Sauda.UserId) &&
                        DbFunctions.TruncateTime(w.Sauda.BiddingDate) <= DbFunctions.TruncateTime(saudaFilterDto.ToDate)) && w.SaudaOrders.StatusId == saudaFilterDto.StatusId
                        && (saudaFilterDto.VerticalId > 0 ? w.SaudaOrders.OilType.DivisionId == saudaFilterDto.VerticalId : w.SaudaOrders.OilType.DivisionId > 0)).OrderByDescending(_ => _.SaudaOrders.Id);

                        outputDto.ListCount = saudaList.Count();
                        saudaListContext = saudaList.Skip((int)skip).Take(pageSize).ToList();
                    }
                }
                else
                {
                    if (saudaFilterDto.SaudaBookingTypeId > 0)
                    {
                        var saudaList = _emamiContext.Sauda.AsNoTracking()
                         .Join(_emamiContext.SaudaOrders.AsNoTracking(), s => s.Id, so => so.SaudaId, (s, so) => new { Sauda = s, SaudaOrders = so })
                         .Join(_emamiContext.ApprovalStatus.AsNoTracking(), x => x.SaudaOrders.StatusId, a => a.Id, (x, a) => new { x.SaudaOrders, x.Sauda, ApprovalStatus = a.Name })
                         .Join(_emamiContext.Depots.AsNoTracking(), x => x.SaudaOrders.PlantId, p => p.Id, (x, p) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, Depots = p.Name })
                         //.Join(_emamiContext.FreightRoutes.AsNoTracking(), x => x.SaudaOrders.DealerLocationId, f => f.Id, (x, f) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, FreightRoutes = f.Name })
                         .Join(_emamiContext.Users.AsNoTracking(), x => x.Sauda.UserId, u => u.Id, (x, u) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, DealerName = u.Name, DealerCode = u.Code, StateId = u.StateId })
                         .Join(_emamiContext.State.AsNoTracking(), x => x.StateId, s => s.Id, (x, s) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.DealerCode, x.StateId, StateName = s.StateName })
                         .Join(_emamiContext.Users.AsNoTracking(), x => x.Sauda.CreatedBy, u => u.Id, (x, u) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.DealerCode, x.StateName, CreatedByName = u.Name })
                         .Join(_emamiContext.IncoTerms.AsNoTracking(), x => x.SaudaOrders.Incoterms2, i => i.Id, (x, i) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.DealerCode, x.CreatedByName, x.StateName, IncoTermsName = i.Name })
                         .Join(_emamiContext.SkuUomMapping.AsNoTracking().Where(_ => _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos), x => x.SaudaOrders.SkuId, s => s.SkuId, (x, s) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.CreatedByName, x.IncoTermsName, NoOfSkusPerCase = s.ConversionFactor, x.DealerCode, x.StateName })
                         .Join(_emamiContext.Pricing.AsNoTracking(), x => x.SaudaOrders.PricingId, s => s.Id, (x, Pricing) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.CreatedByName, x.IncoTermsName, x.NoOfSkusPerCase, Pricing, x.DealerCode, x.StateName })
                         .Where(w => (DbFunctions.TruncateTime(w.Sauda.BiddingDate) >= DbFunctions.TruncateTime(saudaFilterDto.FromDate) && dealerIds.Contains(w.Sauda.UserId) &&
                         DbFunctions.TruncateTime(w.Sauda.BiddingDate) <= DbFunctions.TruncateTime(saudaFilterDto.ToDate))
                         && w.SaudaOrders.SaudaBookingTypeId == saudaFilterDto.SaudaBookingTypeId && (saudaFilterDto.VerticalId > 0 ? w.SaudaOrders.OilType.DivisionId == saudaFilterDto.VerticalId : w.SaudaOrders.OilType.DivisionId > 0))
                         .OrderByDescending(_ => _.SaudaOrders.Id);

                        outputDto.ListCount = saudaList.Count();
                        saudaListContext = saudaList.Skip((int)skip).Take(pageSize).ToList();
                    }
                    else
                    {
                        var saudaList = _emamiContext.Sauda.AsNoTracking()
                        .Join(_emamiContext.SaudaOrders.AsNoTracking(), s => s.Id, so => so.SaudaId, (s, so) => new { Sauda = s, SaudaOrders = so })
                        .Join(_emamiContext.ApprovalStatus.AsNoTracking(), x => x.SaudaOrders.StatusId, a => a.Id, (x, a) => new { x.SaudaOrders, x.Sauda, ApprovalStatus = a.Name })
                        .Join(_emamiContext.Depots.AsNoTracking(), x => x.SaudaOrders.PlantId, p => p.Id, (x, p) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, Depots = p.Name })
                        //.Join(_emamiContext.FreightRoutes.AsNoTracking(), x => x.SaudaOrders.DealerLocationId, f => f.Id, (x, f) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, FreightRoutes = f.Name })
                        .Join(_emamiContext.Users.AsNoTracking(), x => x.Sauda.UserId, u => u.Id, (x, u) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, DealerName = u.Name, DealerCode = u.Code, StateId = u.StateId })
                        .Join(_emamiContext.State.AsNoTracking(), x => x.StateId, s => s.Id, (x, s) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.DealerCode, x.StateId, StateName = s.StateName })
                        .Join(_emamiContext.Users.AsNoTracking(), x => x.Sauda.CreatedBy, u => u.Id, (x, u) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.DealerCode, x.StateId, CreatedByName = u.Name, x.StateName })
                        .Join(_emamiContext.IncoTerms.AsNoTracking(), x => x.SaudaOrders.Incoterms2, i => i.Id, (x, i) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.DealerCode, x.CreatedByName, x.StateName, IncoTermsName = i.Name })
                        .Join(_emamiContext.SkuUomMapping.AsNoTracking().Where(_ => _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos), x => x.SaudaOrders.SkuId, s => s.SkuId, (x, s) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.CreatedByName, x.IncoTermsName, NoOfSkusPerCase = s.ConversionFactor, x.DealerCode, x.StateName })
                        .Join(_emamiContext.Pricing.AsNoTracking(), x => x.SaudaOrders.PricingId, s => s.Id, (x, Pricing) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.CreatedByName, x.IncoTermsName, x.NoOfSkusPerCase, Pricing, x.DealerCode, x.StateName })
                        .Where(w => (DbFunctions.TruncateTime(w.Sauda.BiddingDate) >= DbFunctions.TruncateTime(saudaFilterDto.FromDate) && dealerIds.Contains(w.Sauda.UserId) &&
                        DbFunctions.TruncateTime(w.Sauda.BiddingDate) <= DbFunctions.TruncateTime(saudaFilterDto.ToDate))
                        && (saudaFilterDto.VerticalId > 0 ? w.SaudaOrders.OilType.DivisionId == saudaFilterDto.VerticalId : w.SaudaOrders.OilType.DivisionId > 0)).OrderByDescending(_ => _.SaudaOrders.Id);

                        outputDto.ListCount = saudaList.Count();
                        saudaListContext = saudaList.Skip((int)skip).Take(pageSize).ToList();
                    }
                }
                var description = UtilityHelper.GetEnumDescription(DTO.Enums.Configuration.InboundInterfacenotSyncedToSAPMinutes);
                var configurationContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Key == description).Value;
                var remarksContext = _emamiContext.Remarks.AsNoTracking();
                outputDto.SaudaList = new List<SaudaListDto>();
                SaudaListDto sauda = new SaudaListDto();
                if (saudaListContext != null && saudaListContext.Any())
                {
                    foreach (var se in saudaListContext)
                    {
                        sauda = new SaudaListDto();
                        sauda.Id = se.SaudaOrders.Id;
                        sauda.TradeTicketNumber = se.SaudaOrders.TradeTicketNumber;
                        sauda.SaudaId = se.Sauda.Id;
                        sauda.SaudaNumber = se.SaudaOrders.SaudaNumber;
                        sauda.SkuName = se.SaudaOrders.Sku.SkuName;
                        sauda.SkuCode = se.SaudaOrders.Sku.SkuCode;
                        sauda.Vertical = se.SaudaOrders.Sku.Vertical.Id > 0 /*== looseVerticalId*/ ? se.SaudaOrders.Sku.Vertical.Name : string.Empty;
                        sauda.OiltypeName = se.SaudaOrders.OilType.Name;
                        sauda.QuotedPrice = se.SaudaOrders.QuotedPrice;
                        sauda.BidQuantity = se.SaudaOrders.BidQuantity;
                        sauda.BidQuantityCase = se.SaudaOrders.BidQuantityCase;
                        sauda.BidPrice = se.SaudaOrders.BidPrice;
                        sauda.BiddingDate = se.Sauda.BiddingDate;
                        sauda.DiscountAmount = se.SaudaOrders.DiscountAmount;
                        sauda.ValidFromDate = se.SaudaOrders.ValidFromDate;
                        sauda.ValidToDate = se.SaudaOrders.ValidToDate;
                        sauda.Incoterms1 = se.IncoTermsName;
                        sauda.UserId = se.Sauda.UserId;
                        sauda.StatusId = se.SaudaOrders.StatusId;
                        sauda.DiscountType = se.SaudaOrders.DiscountTypeId != 0 ? Enum.GetName(typeof(SaudaDiscountType), se.SaudaOrders.DiscountTypeId) : "";
                        sauda.SaudaBookingType = se.SaudaOrders.SaudaBookingTypeId != 0 ? Enum.GetName(typeof(SaudaBookingTypes), se.SaudaOrders.SaudaBookingTypeId) : "";
                        sauda.SaudaBookingTypeId = se.SaudaOrders.SaudaBookingTypeId;
                        sauda.Status = se.SaudaOrders.StatusId == (int)DTO.Enums.Status.Pending ? "Accepted" : se.ApprovalStatus;
                        sauda.PlantName = se.Depots;
                        sauda.DealerName = se.DealerName;
                        sauda.DealerLocation = se.FreightRoutes;
                        sauda.CreatedBy = se.CreatedByName;
                        sauda.CounterBidOffer = se.SaudaOrders.CounterBidOffer;
                        sauda.DealerCode = se.DealerCode;
                        sauda.StateName = se.StateName;
                        sauda.IsLooseVerticalForAcceptedStatus = se.SaudaOrders.IsLooseVerticalForAcceptedStatus;
                        sauda.Remarks = remarksContext.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.TableId == sauda.Id && _.IsActive) != null ? remarksContext.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.TableId == sauda.Id && _.IsActive).Description : string.Empty;
                        sauda.IsSAPDataSync = se.SaudaOrders.IsSAPDataSync;
                        sauda.IsActiveRemarks = remarksContext.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.TableId == sauda.Id && _.IsActive) != null ? remarksContext.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.TableId == sauda.Id && _.IsActive).IsActive : false;
                        sauda.IsSAPDataSyncApproval = se.SaudaOrders.IsSAPDataSyncApproval;
                        sauda.IsSaudaApprovalSyncConfirmation = se.SaudaOrders.IsSaudaApprovalSyncConfirmation;
                        sauda.IsSapSauda = se.SaudaOrders.IsSapSauda;
                        sauda.ModifiedDate = se.SaudaOrders.ModifiedDate;
                        sauda.IsSapSaudaNumberUpdateSync = se.SaudaOrders.IsSapSaudaNumberUpdateSync;
                        sauda.IsSaudaApprovalStatusFromSap = se.SaudaOrders.IsSaudaApprovalStatusFromSap;
                        var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        TimeSpan difference = currentDate.Subtract(Convert.ToDateTime(se.SaudaOrders.ModifiedDate));

                        if (sauda.IsSAPDataSync && !sauda.IsSapSauda && !sauda.IsSAPDataSyncApproval && difference.TotalMinutes > Convert.ToDouble(configurationContext) && !sauda.IsSapSaudaNumberUpdateSync)
                        {
                            sauda.IsSapSyncNotReceivedForSaudaNumber = true;
                            sauda.Remarks = "Sauda Number Update Sync not Received From Sap";
                        }

                        if (sauda.IsSAPDataSync && !sauda.IsSapSauda && sauda.IsSAPDataSyncApproval && !string.IsNullOrEmpty(sauda.SaudaNumber) && difference.TotalMinutes > Convert.ToDouble(configurationContext) && !sauda.IsSaudaApprovalSyncConfirmation)
                        {
                            sauda.IsSapSyncNotReceivedForSaudaApprovalConfirmation = true;
                            sauda.Remarks = "Sauda Approval Confirmation Sync not Received From Sap";
                        }

                        if (bdoNewList.IsAny())
                        {
                            var StateTrader = bdoNewList.FirstOrDefault(x => x.CustomerId == se.Sauda.UserId);
                            sauda.BDOName = StateTrader != null ? StateTrader.BDOName : string.Empty;
                            sauda.BDOCode = StateTrader != null ? StateTrader.BDOCode : string.Empty;
                        }

                        if (se.SaudaOrders.BidPrice > 0 && se.SaudaOrders.BidQuantityCase > 0)
                            sauda.BidPricePerCase = se.SaudaOrders.BidPrice / se.SaudaOrders.BidQuantityCase;

                        if (se.SaudaOrders.BidPrice > 0 && se.SaudaOrders.BidQuantityCase > 0)
                            sauda.BidPricePerSku = (se.SaudaOrders.BidPrice / se.SaudaOrders.BidQuantityCase) / se.NoOfSkusPerCase;

                        sauda.BasePricePerCase = se.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExDepot
                             ? se.Pricing.ExDepotPrice
                             : se.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExPlant
                             ? se.Pricing.ExPlantPrice
                             : se.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake
                             ? se.Pricing.ExRakePrice
                             : se.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForDepot
                             ? se.Pricing.ForDepotPrice
                             : se.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForPlant
                             ? se.Pricing.ForPlantPrice
                             : se.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForRake
                             ? se.Pricing.ForRakePrice : 0;

                        if (se.SaudaOrders.BidQuantityCase > 0)
                        {
                            sauda.BasePricePerSku = ((se.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExDepot
                         ? se.Pricing.ExDepotPrice
                         : se.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExPlant
                         ? se.Pricing.ExPlantPrice
                         : se.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake
                         ? se.Pricing.ExRakePrice
                         : se.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForDepot
                         ? se.Pricing.ForDepotPrice
                         : se.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForPlant
                         ? se.Pricing.ForPlantPrice
                         : se.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForRake
                         ? se.Pricing.ForRakePrice : 0) / se.SaudaOrders.BidQuantityCase) / se.NoOfSkusPerCase;
                        }


                        sauda.SaudaBookedNumber = se.SaudaOrders.SaudaId;

                        outputDto.SaudaList.Add(sauda);
                    }
                }
                if (!outputDto.SaudaList.IsAny())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetSaudaListForAdminMobile(SaudaListFilterDto saudaFilterDto)
        {
            _methodName = "GetSaudaListForAdminMobile";
            var resultDto = new ResultDto();
            var outputDto = new SaudaListsDto();
            outputDto.SaudaList = new List<SaudaListDto>();

            try
            {
                if (saudaFilterDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                if (saudaFilterDto.LoginUserId <= 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.DealerIdEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.DealerIdEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }

                if (saudaFilterDto.FromDate == null || saudaFilterDto.FromDate == DateTime.MinValue)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.FromDateEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.FromDateEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (saudaFilterDto.ToDate == null || saudaFilterDto.ToDate == DateTime.MinValue)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.ToDateEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.ToDateEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }

                IEnumerable<dynamic> saudaListContext;
                var bdoList = (from u in _emamiContext.Users.AsNoTracking()
                               join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                               where ur.RoleId == (int)DTO.Enums.Role.StateTrader
                               select new
                               {
                                   Username = u.Name,
                                   UserId = u.Id,
                                   RoleId = ur.RoleId,
                                   Code = u.Code
                               }).ToList();

                var dealerList = (from u in _emamiContext.Users.AsNoTracking()
                                  join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                                  join uc in _emamiContext.UserCustomerMapping.AsNoTracking() on u.Id equals uc.CustomerId
                                  where ur.RoleId == (int)DTO.Enums.Role.Dealer
                                  select new
                                  {
                                      Username = u.Name,
                                      UserId = u.Id,
                                      RoleId = ur.RoleId,
                                      CustometId = uc.UserId
                                  }).ToList();

                var bdoNewList = (from d in dealerList
                                  join StateTrader in bdoList on d.CustometId equals StateTrader.UserId
                                  select new
                                  {
                                      BDOName = StateTrader.Username,
                                      BdoId = StateTrader.UserId,
                                      CustomerId = d.UserId,
                                      BDOCode = StateTrader.Code,
                                  }).ToList();

                var pageSize = Constants.PageSize;
                long skip = pageSize * saudaFilterDto.PageNo;

                var saudaIds = new List<long>();
                var createdBy = new List<long>();
                var loginUserRole = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == saudaFilterDto.LoginUserId);

                var divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == saudaFilterDto.LoginUserId)
                    .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                if (loginUserRole != null)
                {
                    if (loginUserRole.RoleId == (int)DTO.Enums.Role.NationalTrader || loginUserRole.RoleId == (int)DTO.Enums.Role.ZonalTrader || loginUserRole.RoleId == (int)DTO.Enums.Role.StateTrader)
                    {
                        if (loginUserRole.RoleId == (int)DTO.Enums.Role.NationalTrader)
                        {
                            createdBy = _emamiContext.UserReportingToMappings.AsNoTracking().Where(user => user.ReportingToUserId == saudaFilterDto.LoginUserId).Select(_ => _.UserId).ToList();
                        }
                        if (loginUserRole.RoleId == (int)DTO.Enums.Role.ZonalTrader)
                        {
                            createdBy = _emamiContext.UserReportingToMappings.AsNoTracking().Where(user => user.ReportingToUserId == saudaFilterDto.LoginUserId).Select(_ => _.UserId).ToList();
                        }
                        if (loginUserRole.RoleId == (int)DTO.Enums.Role.StateTrader)
                        {
                            createdBy = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == saudaFilterDto.LoginUserId).Select(s => s.CustomerId).ToList();
                        }

                        var saudabeforeCobinationCheck = _emamiContext.SaudaApproval.AsNoTracking().
                            Where(_ => createdBy.Contains(_.CreatedBy) && _.StatusId == (int)DTO.Enums.Status.Pending).Select(_ => _.SaudaId).ToList();

                        saudaIds = (from s in _emamiContext.Sauda.AsNoTracking()
                                    join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                                    equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                                    where saudabeforeCobinationCheck.Contains(s.Id)
                                    select s.Id).ToList();
                    }

                }

                if (saudaFilterDto.DivisionId == 0 && saudaFilterDto.SalesOrganizationId == 0 && saudaFilterDto.DistributionChannelId == 0)
                {
                    var saudaList = _emamiContext.Sauda.AsNoTracking()
                    .Join(_emamiContext.ApprovalStatus.AsNoTracking(), x => x.StatusId, a => a.Id, (x, a) => new { Sauda = x, ApprovalStatus = a.Name })
                    .Join(_emamiContext.Users.AsNoTracking(), x => x.Sauda.UserId, u => u.Id, (x, u) => new { x.Sauda, x.ApprovalStatus, DealerName = u.Name, DealerCode = u.Code, StateId = u.StateId })
                    .Join(_emamiContext.State.AsNoTracking(), x => x.StateId, s => s.Id, (x, s) => new { x.Sauda, x.ApprovalStatus, x.DealerName, x.DealerCode, x.StateId, StateName = s.StateName })
                    .Join(_emamiContext.Users.AsNoTracking(), x => x.Sauda.CreatedBy, u => u.Id, (x, u) => new { x.Sauda, x.ApprovalStatus, x.DealerName, CreatedByName = u.Name, x.DealerCode, x.StateName })
                    .Where(w => (DbFunctions.TruncateTime(w.Sauda.BiddingDate) >= DbFunctions.TruncateTime(saudaFilterDto.FromDate) &&
                    DbFunctions.TruncateTime(w.Sauda.BiddingDate) <= DbFunctions.TruncateTime(saudaFilterDto.ToDate)) /*&& w.Sauda.StatusId == (int)DTO.Enums.Status.Pending */&& (saudaIds.Contains(w.Sauda.Id))
                    ).ToList();

                    outputDto.ListCount = saudaList.Count();
                    saudaListContext = saudaList.OrderByDescending(_ => _.Sauda.Id).Skip((int)skip).Take(pageSize).ToList();
                }
                else
                {
                    var saudaList = _emamiContext.Sauda.AsNoTracking()
                    .Join(_emamiContext.ApprovalStatus.AsNoTracking(), x => x.StatusId, a => a.Id, (x, a) => new { Sauda = x, ApprovalStatus = a.Name })//.Join(_emamiContext.Depots.AsNoTracking(), x => x.SaudaOrders.PlantId, p => p.Id, (x, p) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, Depots = p.Name })
                    .Join(_emamiContext.Users.AsNoTracking(), x => x.Sauda.UserId, u => u.Id, (x, u) => new { x.Sauda, x.ApprovalStatus, DealerName = u.Name, DealerCode = u.Code, StateId = u.StateId })
                    .Join(_emamiContext.State.AsNoTracking(), x => x.StateId, s => s.Id, (x, s) => new { x.Sauda, x.ApprovalStatus, x.DealerName, x.DealerCode, x.StateId, StateName = s.StateName })
                    .Join(_emamiContext.Users.AsNoTracking(), x => x.Sauda.CreatedBy, u => u.Id, (x, u) => new { x.Sauda, x.ApprovalStatus, x.DealerName, CreatedByName = u.Name, x.DealerCode, x.StateName })
                    .Where(w => (DbFunctions.TruncateTime(w.Sauda.BiddingDate) >= DbFunctions.TruncateTime(saudaFilterDto.FromDate) &&
                    DbFunctions.TruncateTime(w.Sauda.BiddingDate) <= DbFunctions.TruncateTime(saudaFilterDto.ToDate)) /*&& w.Sauda.StatusId == (int)DTO.Enums.Status.Pending*/
                    && w.Sauda.SalesOrganizationId == saudaFilterDto.SalesOrganizationId && w.Sauda.DistributionChannelId == saudaFilterDto.DistributionChannelId &&
                       w.Sauda.DivisionId == saudaFilterDto.DivisionId && (saudaIds.Contains(w.Sauda.Id))
                    ).ToList();

                    outputDto.ListCount = saudaList.Count();
                    saudaListContext = saudaList.OrderByDescending(_ => _.Sauda.Id).Skip((int)skip).Take(pageSize).ToList();
                }

                var description = UtilityHelper.GetEnumDescription(DTO.Enums.Configuration.InboundInterfacenotSyncedToSAPMinutes);
                var configurationContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Key == description).Value;
                var remarksContext = _emamiContext.Remarks.AsNoTracking();
                outputDto.SaudaList = new List<SaudaListDto>();
                SaudaListDto sauda = new SaudaListDto();
                if (saudaListContext != null && saudaListContext.Any())
                {
                    foreach (var se in saudaListContext)
                    {
                        sauda = new SaudaListDto();
                        var remarks = remarksContext.Where(_ => _.TableId == sauda.SaudaId && _.IsActive).OrderByDescending(_ => _.Id).FirstOrDefault();
                        sauda.Id = se.Sauda.Id;
                        sauda.SaudaId = se.Sauda.Id;
                        sauda.DealerId = se.Sauda.UserId;
                        //sauda.PlantName = se.Depots;
                        sauda.DealerName = se.DealerName != null ? se.DealerName : string.Empty;
                        sauda.CreatedBy = se.CreatedByName != null ? se.CreatedByName : string.Empty;
                        sauda.DealerCode = se.DealerCode != null ? se.DealerCode : string.Empty;
                        sauda.Remarks = remarks != null ? remarks.Description : string.Empty;
                        sauda.IsError = remarksContext.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.TableId == sauda.SaudaId && _.IsActive) != null ? true : false;
                        var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        if (bdoNewList.IsAny())
                        {
                            var StateTrader = bdoNewList.FirstOrDefault(x => x.CustomerId == se.Sauda.UserId);
                            sauda.BDOName = StateTrader != null ? StateTrader.BDOName : string.Empty;
                            sauda.BDOCode = StateTrader != null ? StateTrader.BDOCode : string.Empty;
                        }

                        sauda.SaudaBookedNumber = se.Sauda.Id;


                        var saudaOrder = _emamiContext.SaudaOrders.Include(_ => _.Sku).AsNoTracking().Where(_ => _.SaudaId == sauda.Id);
                        sauda.BidQuantity = saudaOrder.Sum(_ => _.BidQuantity);
                        sauda.BidQuantityCase = saudaOrder.Sum(_ => _.BidQuantityCase);
                        sauda.BidPrice = saudaOrder.Sum(_ => _.QuotedPrice);

                        if (saudaOrder != null && saudaOrder.Any())
                        {
                            sauda.SkuList = saudaOrder.ToList().Select(a => new SkuList
                            {
                                SkuId = a.SkuId,
                                SkuName = a.Sku.SkuName,
                                Quantity = a.BidQuantityCase,
                                QuantityInMT = a.BidQuantity,
                                PricePercase = a.BidPrice / a.BidQuantityCase
                            }).ToList();

                            var singleSaudaOrder = saudaOrder.FirstOrDefault();
                            if (singleSaudaOrder != null)
                            {
                                sauda.SaudaNumber = singleSaudaOrder.Sauda.SaudaNumber != null ? singleSaudaOrder.Sauda.SaudaNumber : string.Empty;
                                sauda.SkuName = singleSaudaOrder.Sku.SkuName;
                                sauda.SkuCode = singleSaudaOrder.Sku.SkuCode;
                                sauda.PlantName = _emamiContext.Depots.FirstOrDefault(_ => _.Id == singleSaudaOrder.PlantId)?.Name;
                            }
                        }
                        outputDto.SaudaList.Add(sauda);
                    }
                }
                outputDto.SaudaListGroup = outputDto.SaudaList.GroupBy(_ => _.DealerId).Select(s => new SaudaListGroupDto()
                {
                    DealerId = s.FirstOrDefault().DealerId,
                    DealerName = s.FirstOrDefault().DealerName,
                    DealerCode = s.FirstOrDefault().DealerCode,
                    SaudaList = s.ToList()
                }).ToList();
                outputDto.SaudaList = new List<SaudaListDto>();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }


        public ResultDto UpdateLiftingSaudaOrderId()
        {
            _methodName = "UpdateLiftingSaudaOrderId";
            var resultDto = new ResultDto();
            var outputDto = new SaudaListsDto();
            outputDto.SaudaList = new List<SaudaListDto>();

            try
            {
                var saudaNumberlist = _emamiContext.LiftingRequestDetails.AsNoTracking()
                    .Where(_ => _.SaudaOrderId == 0)
                    .Select(s => new { SaudaNumber = s.SaudaNumber }).ToList();

                var saudacontext = _emamiContext.Sauda.AsNoTracking().ToList();
                var saudaOrdercontext = _emamiContext.SaudaOrders.AsNoTracking().ToList();

                var liftingDetails = _emamiContext.LiftingRequestDetails.Where(_ => _.SaudaOrderId == 0).AsEnumerable();
                foreach (var lifting in liftingDetails)
                {
                    lifting.SaudaOrderId = saudacontext
                        .Join(saudaOrdercontext, s => s.Id, so => so.SaudaId, (s, so) => new { s, so })
                        .Where(_ => _.s.SaudaNumber == lifting.SaudaNumber && lifting.SkuId == _.so.SkuId).Select(s => s.so.Id).FirstOrDefault();
                }
                _emamiContext.SaveChanges();
                foreach (var s in saudaNumberlist)
                {
                    _sapIntegrationService.ContractAvilableLimitCalculate(s.SaudaNumber);
                }
                //_sapIntegrationService.ContractAvilableLimitCalculate();

                outputDto.SaudaList = new List<SaudaListDto>();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        #region CompetitorAnalysis      

        /// <summary>
        /// Method to Get CompetitorAnalysis List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        public ResultDto GetCompetitorAnalysisList(LoginUserIdDto inputDto)
        {
            _methodName = "GetCompetitorAnalysisList";
            var resultDto = new ResultDto();
            var outputDto = new List<CompetitorAnalysisViewDto>();
            try
            {
                IQueryable<CompetitorAnalysis> resultContext;

                var competitorAnalysisApproval = _emamiContext.CompetitorAnalysisApproval.AsNoTracking().Where(_ => _.RequestedTo == inputDto.LoginUserId || _.CreatedBy == inputDto.LoginUserId);
                List<long> competitorAnalysisIds = competitorAnalysisApproval.Select(_ => _.CompetitorAnalysisId).ToList();

                resultContext = _emamiContext.CompetitorAnalysis.AsNoTracking().Where(_ => competitorAnalysisIds.Contains(_.Id)
                && (inputDto.VerticalId > 0 ? _.OilType.DivisionId == inputDto.VerticalId : _.OilType.DivisionId > 0));

                if (resultContext != null && resultContext.Any())
                {
                    outputDto = resultContext.Select(c => new CompetitorAnalysisViewDto
                    {
                        Id = c.Id,
                        SkuId = c.SkuId,
                        SkuName = c.Sku != null ? c.Sku.SkuName : string.Empty,
                        SkuCode = c.Sku != null ? c.Sku.SkuCode : string.Empty,
                        OilTypeId = c.OilTypeId,
                        OilType = c.OilType != null ? c.OilType.Name + "-" + c.OilType.SalesOrganization.Code + "/" + c.OilType.DistributionChannel.Code + "/" + c.OilType.Division.Code : string.Empty,
                        //OilTypeCode = c.OilType != null ? c.OilType.SAPCode : string.Empty,
                        StatusId = c.StatusId,
                        Status = c.Status != null ? c.Status.Name : string.Empty,
                        Margin = c.Margin,
                        EmamiPrice = c.EmamiPrice,
                        Remarks = c.Remarks,
                        WorkableQuantity = c.WorkableQuantity,
                        WorkablePrice = c.WorkablePrice
                    }).ToList();
                }
                var result = outputDto != null ? outputDto.OrderByDescending(_ => _.Id).ToDataSourceResult(inputDto.DataSourceRequest) : outputDto.ToDataSourceResult(inputDto.DataSourceRequest);
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = result;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to get Get CompetitorAnalysis Details By Id
        /// </summary>
        /// <param name="competitorAnalysisId"></param>
        /// <returns></returns>
        public ResultDto GetCompetitorAnalysisById(IdInputDto inputDto)
        {
            _methodName = "GetCompetitorAnalysisDetailsById";
            var resultDto = new ResultDto();
            var outputDto = new CompetitorAnalysisViewDto();
            try
            {
                var resultContext = _emamiContext.CompetitorAnalysis.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.Id);
                if (resultContext != null)
                {
                    outputDto.Id = resultContext.Id;
                    outputDto.SkuId = resultContext.SkuId;
                    outputDto.SkuName = resultContext.Sku != null ? resultContext.Sku.SkuName : string.Empty;
                    outputDto.OilTypeId = resultContext.OilTypeId;
                    outputDto.OilType = resultContext.OilType != null ? resultContext.OilType.Name + "-" + resultContext.OilType.SalesOrganization.Code + "/" + resultContext.OilType.DistributionChannel.Code + "/" + resultContext.OilType.Division.Code : string.Empty;
                    outputDto.StatusId = resultContext.StatusId;
                    outputDto.Status = resultContext.Status != null ? resultContext.Status.Name : string.Empty;
                    outputDto.Margin = resultContext.Margin;
                    outputDto.EmamiPrice = resultContext.EmamiPrice;
                    outputDto.WorkableQuantity = resultContext.WorkableQuantity;
                    outputDto.WorkablePrice = resultContext.WorkablePrice;
                    outputDto.Remarks = resultContext.Remarks;

                    var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    var userDetails = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == resultContext.CreatedBy);
                    if (userDetails != null)
                    {
                        var cityId = userDetails.CityId;
                        var stateId = userDetails.StateId;

                        //var profitMargin = _emamiContext.ProfitMargins.AsNoTracking().FirstOrDefault(_ => _.SkuId == resultContext.SkuId
                        //&& _.StateId == stateId
                        //&& DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom)
                        //&& DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                        //if (profitMargin != null)
                        //{
                        //    outputDto.ProfitMargin = profitMargin.RatePerMt;
                        //}

                        //var cushionMargin = _emamiContext.CushionMargins.AsNoTracking().FirstOrDefault(_ => _.SkuId == resultContext.SkuId && _.CityId == cityId
                        // && DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom)
                        //&& DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                        //if (cushionMargin != null)
                        //{
                        //    outputDto.CushionMargin = cushionMargin.RatePerMt;
                        //}

                        var oilTypeId = 0L; var litreConversion = (decimal)0;
                        var oilPackingTypeId = 0L;
                        var uomId = 0L; var quantity = (decimal)0;
                        var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == resultContext.SkuId);
                        if (skuContext != null)
                        {
                            oilTypeId = Convert.ToInt64(skuContext.OilTypeId);
                            oilPackingTypeId = Convert.ToInt64(skuContext.PackGroupId);
                            uomId = Convert.ToInt64(skuContext.UomId);
                            quantity = skuContext.Quantity;
                            //  litreConversion = skuContext.OilType.LitreConversion;
                        }

                        //var oilTypeContext = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == oilTypeId);
                        //if (oilTypeContext != null)
                        //{
                        //    litreConversion = oilTypeContext.LitreConversion;
                        //}


                        //var noofPiecesperCase = (decimal)0; ;
                        //var skuUomContext = _emamiContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == resultContext.SkuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                        //if (skuUomContext != null)
                        //{
                        //    noofPiecesperCase = skuUomContext.ConversionFactor;
                        //}

                        ////Cushion Margin Cost
                        //var cushionMarginCostContext = _emamiContext.CushionMargins.AsNoTracking().FirstOrDefault(_ => _.SkuId == resultContext.SkuId && _.CityId == cityId
                        // && DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo));
                        //if (cushionMarginCostContext != null)
                        //{
                        //    var cushionMarginCostMT = _resultService.GetSkuQuanityRate(uomId, quantity, cushionMarginCostContext.RatePerMt, litreConversion);
                        //    outputDto.CushionMargin = noofPiecesperCase * cushionMarginCostMT; //Case
                        //}

                        //outputDto.TotalCushionProfitMargin = outputDto.CushionMargin + outputDto.ProfitMargin;
                        var priceDifference = outputDto.EmamiPrice - outputDto.WorkablePrice;
                        if (priceDifference > 0)
                        {
                            outputDto.CalculatedFinalMargin = priceDifference;
                        }
                    }

                    var competitorApprovals = _emamiContext.CompetitorAnalysisApproval.AsNoTracking().Where(_ => _.CompetitorAnalysisId == inputDto.Id).OrderByDescending(_ => _.CreatedDate);
                    if (competitorApprovals != null && competitorApprovals.Any())
                    {
                        var requestTo = competitorApprovals.FirstOrDefault().RequestedTo;
                        if (requestTo == inputDto.LoginUserId)
                        {
                            outputDto.HasAccessToProceed = true;
                            outputDto.ApprovalsCount = competitorApprovals.Count();
                        }
                    }

                    var details = _emamiContext.CompetitorAnalysisDetails.AsNoTracking().Where(_ => _.CompetitorAnalysisId == inputDto.Id);
                    if (details != null && details.Any())
                    {
                        outputDto.CompetitorAnalysisDetailsDtoList = details
                            .Select(_ => new CompetitorAnalysisDetailsViewDto
                            {
                                CompetitorAnalysisId = _.CompetitorAnalysisId,
                                CompetitorId = _.CompetitorId,
                                CompetitorName = _.Competitor != null ? _.Competitor.Name : string.Empty,
                                SaudaRate = _.SaudaRate,
                                MarketOperatingPrice = _.MarketOperatingPrice,
                            })
                            .ToList();
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to get Get CompetitorAnalysis Details By Id
        /// </summary>
        /// <param name="competitorAnalysisId"></param>
        /// <returns></returns>
        public ResultDto GetCompetitorAnalysisDetailsListById(long competitorAnalysisId)
        {
            _methodName = "GetCompetitorAnalysisDetailsById";
            var resultDto = new ResultDto();
            var outputDto = new List<CompetitorAnalysisDetailsViewDto>();
            try
            {
                var resultContext = _emamiContext.CompetitorAnalysisDetails.AsNoTracking().Where(_ => _.CompetitorAnalysisId == competitorAnalysisId);
                if (resultContext != null && resultContext.Any())
                {
                    outputDto = resultContext
                    .Select(_ => new CompetitorAnalysisDetailsViewDto
                    {
                        CompetitorAnalysisId = _.CompetitorAnalysisId,
                        CompetitorId = _.CompetitorId,
                        CompetitorName = _.Competitor != null ? _.Competitor.Name : string.Empty,
                        SaudaRate = _.SaudaRate,
                        MarketOperatingPrice = _.MarketOperatingPrice,
                    })
                    .ToList();
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to Update CompetitorAnalysis
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto UpdateCompetitorAnalysis(CompetitorAnalysisAddDto inputDto)
        {
            _methodName = "UpdateCompetitorAnalysis";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }
                var result = _emamiContext.CompetitorAnalysis.FirstOrDefault(_ => _.Id == inputDto.Id);
                result.SkuId = inputDto.SkuId;
                result.OilTypeId = inputDto.OilTypeId;
                result.StatusId = inputDto.StatusId;
                result.Margin = inputDto.Margin;
                result.EmamiPrice = inputDto.EmamiPrice;
                result.Remarks = inputDto.Remarks;
                result.ModifiedBy = inputDto.LoginUserId;
                result.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        /// <summary>
        /// Method to Proceed Competitor Analysis For Approval
        /// </summary>
        /// <param name="employeeDto"></param>
        /// <returns></returns>
        public ResultDto SaveCompetitorAnalysisApproval(CompetitorAnalysisApprovalDto inputDto)
        {
            _methodName = "SaveCompetitorAnalysisApproval";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }
                var result = _emamiContext.CompetitorAnalysis.FirstOrDefault(_ => _.Id == inputDto.CompetitorAnalysisId);
                if (result.StatusId == (int)DTO.Enums.Status.Pending)
                {
                    if (inputDto.StatusId == (int)DTO.Enums.Status.Approved || inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                    {
                        inputDto.RequestedTo = 0;
                    }
                    else
                    {
                        var user = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId).FirstOrDefault();
                        if (user != null)
                        {
                            inputDto.RequestedTo = user.ReportingToUserId;
                        }
                        //var users = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == inputDto.LoginUserId);
                        //if (users != null && users.Any() && users.FirstOrDefault().ReportingToId != null)
                        //{
                        //    inputDto.RequestedTo = (long)users.FirstOrDefault().ReportingToId;
                        //}
                    }

                    var input = new CompetitorAnalysisApproval
                    {
                        CompetitorAnalysisId = inputDto.CompetitorAnalysisId,
                        RequestedBy = inputDto.LoginUserId,
                        RequestedTo = inputDto.RequestedTo,
                        ApprovedBy = inputDto.ApprovedBy,
                        StatusId = inputDto.StatusId,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    };
                    _emamiContext.CompetitorAnalysisApproval.Add(input);
                    _emamiContext.SaveChanges();

                    result.StatusId = inputDto.StatusId;
                    result.Margin = inputDto.Margin;
                    _emamiContext.SaveChanges();

                    #region Send Email and SMS

                    try
                    {
                        if (inputDto.StatusId == (int)DTO.Enums.Status.Approved || inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                        {
                            var approveOrRejectStatus = string.Empty;
                            if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                            {
                                approveOrRejectStatus = DTO.Enums.Status.Approved.ToString();
                            }
                            if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                            {
                                approveOrRejectStatus = DTO.Enums.Status.Rejected.ToString();
                            }

                            var competitorAnalysisApprovalList = _emamiContext.CompetitorAnalysisApproval.AsNoTracking()
                            .Where(_ => _.CompetitorAnalysisId == inputDto.CompetitorAnalysisId);

                            //&& _.RequestedTo != inputDto.LoginUserId && _.CreatedBy != inputDto.LoginUserId
                            //competitorAnalysisApprovalList = competitorAnalysisApprovalList.Where(_ => _.CreatedBy != inputDto.LoginUserId);
                            List<long> toUserList = new List<long>();
                            foreach (var item in competitorAnalysisApprovalList)
                            {
                                if (item.RequestedTo != inputDto.LoginUserId)
                                {
                                    toUserList.Add(item.RequestedTo);
                                }

                                if (item.CreatedBy != inputDto.LoginUserId)
                                {
                                    toUserList.Add(item.CreatedBy);
                                }
                            }
                            //toUserList.AddRange(competitorAnalysisApprovalList.Select(_ => _.RequestedTo));
                            //toUserList.AddRange(competitorAnalysisApprovalList.Select(_ => _.CreatedBy));

                            List<string> toUserEmails = new List<string>();
                            var sendNotifyUsers = _emamiContext.Users.AsNoTracking().Where(_ => toUserList.Contains(_.Id));
                            toUserEmails.AddRange(sendNotifyUsers.Select(_ => _.Email.ToString()));

                            List<string> toUserMobileNumbers = new List<string>();
                            toUserMobileNumbers.AddRange(sendNotifyUsers.Select(_ => _.MobileNumber));

                            //if (sendNotifyUsers != null && sendNotifyUsers.Any())
                            //{
                            //    foreach (var item in sendNotifyUsers)
                            //    {
                            //        toUserEmails.Add(item.Email.ToString());
                            //    }
                            //}

                            AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                            var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.PriceDiscoveryEmail);
                            if (_resultService.IsEmail())
                            {
                                var emailSubject = Constants.PriceDiscoverySubject;
                                var fromEmail = Constants.FromEmail;
                                var plainText = string.Empty;
                                if (emailTemplate != null)
                                {
                                    var replaceEmailTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, result.Sku?.SkuName).Replace(Constants.ApproveOrReject, approveOrRejectStatus);
                                    var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, replaceEmailTemplate);
                                    amazonNotificationService.SendEmail(toUserEmails, emailSubject, plainText, htmlTemplate, true);
                                }
                            }
                            if (_resultService.IsSMS())
                            {
                                var smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.PriceDiscoverySMS);
                                if (smsTemplate != null)
                                {
                                    var smsMessage = smsTemplate.PlainTemplate.Replace(Constants.SkuName, result.Sku?.SkuName).Replace(Constants.ApproveOrReject, approveOrRejectStatus);
                                    foreach (var mobile in toUserMobileNumbers)
                                    {
                                        amazonNotificationService.SendMessage(smsMessage, mobile);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.PriceDiscoveryStatusAlreadyUpdated);
                }
                resultDto.IsSuccess = true;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetSaudaDetail(IdInputDto idInputDto)
        {
            _methodName = "GetSaudaDetail";
            var resultDto = new ResultDto();
            var outputDto = new SaudaListsDto();
            try
            {
                if (idInputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                List<SaudaListDto> saudaList;
                var bdomappings = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                   join ur in _emamiContext.UserRoles.AsNoTracking() on ucm.UserId equals ur.UserId
                                   join udivm in _emamiContext.UserDivisionMappings.AsNoTracking() on ucm.UserId equals udivm.UserId
                                   where ur.RoleId == (int)DTO.Enums.Role.StateTrader
                                   select new
                                   {
                                       CustomerId = ucm.CustomerId,
                                       BdoId = ucm.UserId,
                                       SalesOrganizationId = udivm.SalesOrganizationId,
                                       DistributionChannelId = udivm.DistributionChannelId,
                                       DivisionId = udivm.DivisionId
                                   }).ToList();

                var bdoNewList = (from u in _emamiContext.Users.AsNoTracking()
                                  join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                                  where ur.RoleId == (int)DTO.Enums.Role.StateTrader
                                  select new
                                  {
                                      Username = u.Name,
                                      UserId = u.Id,
                                      RoleId = ur.RoleId,
                                      Code = u.Code
                                  }).ToList();

                var description = UtilityHelper.GetEnumDescription(DTO.Enums.Configuration.InboundInterfacenotSyncedToSAPMinutes);
                var configurationContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Key == description).Value;
                // var looseVerticalId = _emamiContext.Verticals.AsNoTracking().FirstOrDefault(vertical => vertical.Id == (int)DTO.Enums.LooseVertical.Loose).Id;
                var remarksContext = _emamiContext.Remarks.AsNoTracking();

                saudaList = _emamiContext.Sauda.AsNoTracking()
                        .Join(_emamiContext.SaudaOrders.AsNoTracking(), s => s.Id, so => so.SaudaId, (s, so) => new { Sauda = s, SaudaOrders = so })
                        .Join(_emamiContext.ApprovalStatus.AsNoTracking(), x => x.SaudaOrders.StatusId, a => a.Id, (x, a) => new { x.SaudaOrders, x.Sauda, ApprovalStatus = a.Name })
                        .Join(_emamiContext.Depots.AsNoTracking(), x => x.SaudaOrders.PlantId, p => p.Id, (x, p) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, Depots = p.Name + "-" + p.Code })
                        //.Join(_emamiContext.FreightRoutes.AsNoTracking(), x => x.SaudaOrders.DealerLocationId, f => f.Id, (x, f) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, FreightRoutes = f.Name })
                        .Join(_emamiContext.Users.AsNoTracking(), x => x.Sauda.UserId, u => u.Id, (x, u) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, DealerName = u.Name, DealerId = u.Id, DealerCode = u.Code, StateId = u.StateId })
                        .Join(_emamiContext.State.AsNoTracking(), x => x.StateId, s => s.Id, (x, s) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.DealerCode, x.StateId, StateName = s.StateName, x.DealerId })
                        .Join(_emamiContext.Users.AsNoTracking(), x => x.Sauda.CreatedBy, u => u.Id, (x, u) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, CreatedByName = u.Name, x.DealerCode, x.StateName, x.DealerId })
                        .Join(_emamiContext.UserDivisionMappings.AsNoTracking(), x => x.DealerId, ud => ud.UserId, (x, ud) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, CreatedByName = x.CreatedByName, x.DealerCode, x.StateName, DealerId = x.DealerId, ud })
                        .Join(_emamiContext.OilTypes.AsNoTracking(), x => x.SaudaOrders.Sku.OilTypeId, o => o.Id, (x, o) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.CreatedByName, x.DealerCode, x.StateName, x.ud, x.DealerId, oiltype = o })
                        .Join(_emamiContext.IncoTerms.AsNoTracking(), x => x.SaudaOrders.Incoterms2, i => i.Id, (x, i) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.CreatedByName, IncoTermsName = i.Name, x.DealerCode, x.StateName, x.ud, x.DealerId,x.oiltype})
                         //.Join(_emamiContext.Pricing.AsNoTracking(), x => x.SaudaOrders.PricingId, s => s.Id, (x, Pricing) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.CreatedByName, x.IncoTermsName, x.NoOfSkusPerCase, Pricing, x.DealerCode, x.StateName })
                         //.Join(_emamiContext.UserCustomerMapping.AsNoTracking(),x=>x.Sauda.UserId,uc=>uc.CustomerId,(x,uc)=>new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots,  x.DealerName, x.CreatedByName, x.IncoTermsName, x.NoOfSkusPerCase, x.Pricing, x.DealerCode, x.StateName, BDOId=uc.UserId })
                         //.Join(_emamiContext.Users.AsNoTracking(),x=>x.BDOId,u=>u.Id,(x,u)=>new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots,  x.DealerName, x.CreatedByName, x.IncoTermsName, x.NoOfSkusPerCase, x.Pricing, x.DealerCode, x.StateName, x.BDOId, BDOName=u.Name })
                         .Where(_ => _.SaudaOrders.SaudaId == idInputDto.Id && _.SaudaOrders.DivisionId == _.ud.DivisionId).Select(se => new SaudaListDto()
                         {

                             Id = se.SaudaOrders.Id,
                             SalesOrganizationId = se.ud.SalesOrganizationId,
                             DealerId = se.DealerId,
                             DistributionChannelId = se.ud.DistributionChannelId,
                             DivisionId = se.ud.DivisionId,
                             //TradeTicketNumber = se.SaudaOrders.TradeTicketNumber,
                             SaudaId = se.Sauda.Id,
                             SaudaNumber = se.SaudaOrders.SaudaNumber,
                             SaudaOrderId = se.SaudaOrders.Id,
                             SkuName = se.SaudaOrders.Sku.SkuName,
                             SkuId = se.SaudaOrders.Sku.Id,
                             SkuCode = se.SaudaOrders.Sku.SkuCode,
                             //Vertical = se.SaudaOrders.Sku.Division.Id > 0 /*== looseVerticalId*/ ? se.SaudaOrders.Sku.Division.Name : string.Empty,
                             OiltypeName = se.oiltype.Name + "-" + se.SaudaOrders.OilType.SalesOrganization.Code + "/" + se.SaudaOrders.OilType.DistributionChannel.Code + "/" + se.SaudaOrders.OilType.Division.Code,
                             //OiltypeCode = se.SaudaOrders.OilType.SAPCode,
                             QuotedPrice = se.SaudaOrders.QuotedPrice,
                             BidQuantity = se.SaudaOrders.BidQuantity,
                             BidQuantityCase = se.SaudaOrders.BidQuantityCase,
                             BidPrice = se.SaudaOrders.BidPrice,
                             BiddingDate = se.Sauda.BiddingDate,
                             DiscountAmount = se.SaudaOrders.DiscountAmount,
                             QPSDiscount = se.SaudaOrders.QPSDiscount,
                             ValidFromDate = se.SaudaOrders.ValidFromDate,
                             ValidToDate = se.SaudaOrders.ValidToDate,
                             Incoterms1 = se.IncoTermsName,
                             UserId = se.Sauda.UserId,
                             StatusId = se.SaudaOrders.StatusId,
                             //DiscountType = se.SaudaOrders.DiscountTypeId != 0 ? Enum.GetName(typeof(SaudaDiscountType), se.SaudaOrders.DiscountTypeId) : "",
                             //SaudaBookingType = se.SaudaOrders.SaudaBookingTypeId != 0 ? Enum.GetName(typeof(SaudaBookingTypes), se.SaudaOrders.SaudaBookingTypeId) : "",
                             SaudaBookingTypeId = se.SaudaOrders.SaudaBookingTypeId,
                             DiscountTypeId = se.SaudaOrders.DiscountTypeId,
                             Status = se.SaudaOrders.StatusId == (int)DTO.Enums.Status.Pending ? "Accepted" : se.ApprovalStatus,
                             PlantName = se.Depots,
                             DealerName = se.DealerName,
                             //DealerLocation = se.FreightRoutes,
                             CreatedBy = se.CreatedByName,
                             //CounterBidOffer = se.SaudaOrders.CounterBidOffer,
                             DealerCode = se.DealerCode,
                             StateName = se.StateName,
                             IsLooseVerticalForAcceptedStatus = se.SaudaOrders.IsLooseVerticalForAcceptedStatus,
                             Remarks = remarksContext.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.TableId == se.Sauda.Id && _.IsActive) != null ? remarksContext.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.TableId == se.Sauda.Id && _.IsActive).Description : string.Empty,
                             IsSAPDataSync = se.SaudaOrders.IsSAPDataSync,
                             IsActiveRemarks = remarksContext.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.TableId == se.Sauda.Id && _.IsActive) != null ? remarksContext.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.TableId == se.Sauda.Id && _.IsActive).IsActive : false,
                             IsSAPDataSyncApproval = se.SaudaOrders.IsSAPDataSyncApproval,
                             IsSaudaApprovalSyncConfirmation = se.SaudaOrders.IsSaudaApprovalSyncConfirmation,
                             IsSapSauda = se.SaudaOrders.IsSapSauda,
                             ModifiedDate = se.SaudaOrders.ModifiedDate,
                             IsSapSaudaNumberUpdateSync = se.SaudaOrders.IsSapSaudaNumberUpdateSync,
                             IsSaudaApprovalStatusFromSap = se.SaudaOrders.IsSaudaApprovalStatusFromSap,
                             SaudaBookedNumber = se.SaudaOrders.SaudaId,
                             IncotermsTwo = se.SaudaOrders.Incoterms2,
                             ApproverRemarks = se.SaudaOrders.Remarks,
                             PRAmount = se.SaudaOrders.PRAmount
                             //NoOfSkusPerCase = se.NoOfSkusPerCase
                         }).Distinct().ToList();

                if (saudaList.IsAny())
                {
                    outputDto.SaudaList = new List<SaudaListDto>();
                    foreach (var sauda in saudaList)
                    {
                        sauda.DiscountType = sauda.DiscountTypeId != 0 ? Enum.GetName(typeof(SaudaDiscountType), sauda.DiscountTypeId) : "";
                        sauda.SaudaBookingType = sauda.SaudaBookingTypeId != 0 ? Enum.GetName(typeof(SaudaBookingTypes), sauda.SaudaBookingTypeId) : "";
                        var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        TimeSpan difference = currentDate.Subtract(Convert.ToDateTime(sauda.ModifiedDate));

                        if (bdoNewList.IsAny())
                        {
                            var StateTrader = bdomappings.FirstOrDefault(x => x.SalesOrganizationId == sauda.SalesOrganizationId && x.DistributionChannelId == sauda.DistributionChannelId && x.DivisionId == sauda.DivisionId && x.CustomerId == sauda.UserId) != null ? bdomappings.FirstOrDefault(x => x.SalesOrganizationId == sauda.SalesOrganizationId && x.DistributionChannelId == sauda.DistributionChannelId && x.DivisionId == sauda.DivisionId && x.CustomerId == sauda.UserId).BdoId : 0;
                            var stateTraderDetails = bdoNewList.FirstOrDefault(st => st.UserId == StateTrader);
                            sauda.BDOName = stateTraderDetails != null ? stateTraderDetails.Username : string.Empty;
                            sauda.BDOCode = stateTraderDetails != null ? stateTraderDetails.Code : string.Empty;
                        }

                        if (sauda.QuotedPrice > 0 && sauda.BidQuantityCase > 0)
                            sauda.BidPricePerCase = sauda.BidPrice / sauda.BidQuantityCase;

                        if (sauda.PRAmount > 0)
                            sauda.BidPricePerCase = sauda.PRAmount;

                        sauda.Remarks = string.IsNullOrEmpty(sauda.Remarks) && (sauda.IsSAPDataSync && difference.TotalMinutes > Convert.ToDouble(configurationContext) && !sauda.IsSapSaudaNumberUpdateSync) ? "Sauda Number Update Sync not Received From Sap" : sauda.Remarks;
                        //if (sauda.BidPrice > 0 && sauda.BidQuantityCase > 0)
                        //    sauda.BidPricePerSku = (sauda.BidPrice / sauda.BidQuantityCase) / sauda.NoOfSkusPerCase;

                        //sauda.BasePricePerCase = sauda.IncotermsTwo == (int)DTO.Enums.IncoTerms.ExDepot
                        //     ? sauda.ExDepotPrice
                        //     : sauda.IncotermsTwo == (int)DTO.Enums.IncoTerms.ExPlant
                        //     ? sauda.ExPlantPrice
                        //     : sauda.IncotermsTwo == (int)DTO.Enums.IncoTerms.ExRake
                        //     ? sauda.ExRakePrice
                        //     : sauda.IncotermsTwo == (int)DTO.Enums.IncoTerms.ForDepot
                        //     ? sauda.ForDepotPrice
                        //     : sauda.IncotermsTwo == (int)DTO.Enums.IncoTerms.ForPlant
                        //     ? sauda.ForPlantPrice
                        //     : sauda.IncotermsTwo == (int)DTO.Enums.IncoTerms.ForRake
                        //     ? sauda.ForRakePrice : 0;

                        //if (sauda.BidQuantityCase > 0)
                        //{
                        //    sauda.BasePricePerSku = ((sauda.IncotermsTwo == (int)DTO.Enums.IncoTerms.ExDepot
                        // ? sauda.ExDepotPrice
                        // : sauda.IncotermsTwo == (int)DTO.Enums.IncoTerms.ExPlant
                        // ? sauda.ExPlantPrice
                        // : sauda.IncotermsTwo == (int)DTO.Enums.IncoTerms.ExRake
                        // ? sauda.ExRakePrice
                        // : sauda.IncotermsTwo == (int)DTO.Enums.IncoTerms.ForDepot
                        // ? sauda.ForDepotPrice
                        // : sauda.IncotermsTwo == (int)DTO.Enums.IncoTerms.ForPlant
                        // ? sauda.ForPlantPrice
                        // : sauda.IncotermsTwo == (int)DTO.Enums.IncoTerms.ForRake
                        // ? sauda.ForRakePrice : 0) / sauda.BidQuantityCase) / sauda.NoOfSkusPerCase;
                        //}
                        outputDto.SaudaList.Add(sauda);
                    }

                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }
        #endregion

        #region Sauda Booking Congiguration List


        public ResultDto GetSaudaBookingConfigurationList(UserIdDto inputDto)
        {
            _methodName = "GetSaudaBookingRestrictionConfigurationList";
            var resultDto = new ResultDto();

            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                if (inputDto.UserId <= 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }

                var userRoleContext = _emamiContext.UserRoles.FirstOrDefault(_ => _.UserId == inputDto.UserId);

                IEnumerable<DivisionDetailsDto> divisionslogieduser = new List<DivisionDetailsDto>();
                if (userRoleContext.RoleId == (int)DTO.Enums.Role.Admin)
                {
                    divisionslogieduser = _emamiContext.Divisions.AsNoTracking().Select(s => new DivisionDetailsDto()
                    {
                        SalesOrganizationId = s.SalesOrganizationId,
                        DistributionChannelId = s.DistributionChannelId,
                        DivisionId = s.Id
                    });
                }
                else
                {
                    divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.UserId)
                 .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });
                }

                var roleLookup = _emamiContext.Roles.AsNoTracking().ToDictionary(r => r.Id, r => r.Name);
                var saudaBookingConfigList = _emamiContext.SaudaBookingConfiguration.AsNoTracking().ToList();

                if(!saudaBookingConfigList.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }

                var saudaBookingRestrictionList = saudaBookingConfigList.Select(config => new SaudaBookingConfigurationListDto
                {
                    Id = config.Id,
                    EncryptedId = UtilityHelper.ConvertToMd5(config.Id.ToString(), SecurityConstants.EncryptionKey),
                    RoleId = config.RoleId,
                    RoleName = roleLookup.ContainsKey(config.RoleId) ? roleLookup[config.RoleId] : string.Empty,
                    StartDate = (DateTime)config.StartDate,
                    OilTypeIds = config.OilTypeIds
                        .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => Convert.ToInt64(x)).ToList(),
                    UserIds = config.UserIds
                        .Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => Convert.ToInt64(x)).ToList(),
                    IsActive = config.IsActive
                }).OrderByDescending(_ => _.Id).ToList();


                if (saudaBookingRestrictionList.Any())
                {
                    saudaBookingRestrictionList.ForEach(_ =>
                    {
                        _.OilTypeNames = string.Join(",", _emamiContext.OilTypes.AsNoTracking().Where(x => _.OilTypeIds.Contains(x.Id)).Select(y => y.Name).ToList());
                        _.UserNames = string.Join(",", _emamiContext.Users.AsNoTracking().Where(x => _.UserIds.Contains(x.Id)).Select(y => y.Name).ToList());
                    });

                    // Get the sales areas of logged in user
                    var userSalesAreas = divisionslogieduser.ToList();

                    // Create a lookup for OilTypes
                    //var oilTypeLookup = _emamiContext.OilTypes
                    //    .AsNoTracking()
                    //    .Where(x => saudaBookingRestrictionList.SelectMany(y => y.OilTypeIds).Contains(x.Id))
                    //    .Select(x => new
                    //    {
                    //        x.Id,
                    //        x.SalesOrganizationId,
                    //        x.DistributionChannelId,
                    //        x.DivisionId
                    //    })
                    //    .ToDictionary(x => x.Id);

                    //var allOilTypeIds = saudaBookingRestrictionList
                    //                    .SelectMany(y => y.OilTypeIds)
                    //                    .Distinct()
                    //                    .ToList();

                    var allOilTypeIds = new HashSet<long>(
                                        saudaBookingRestrictionList
                                            .SelectMany(y => y.OilTypeIds)
                                            .Distinct()
                                    );

                    var oilTypeLookup = _emamiContext.OilTypes
                                        .AsNoTracking()
                                        .Where(x => allOilTypeIds.Contains(x.Id))
                                        .Select(x => new
                                        {
                                            x.Id,
                                            x.SalesOrganizationId,
                                            x.DistributionChannelId,
                                            x.DivisionId
                                        })
                                        .ToDictionary(x => x.Id);

                    saudaBookingRestrictionList = saudaBookingRestrictionList
                    .Where(config =>
                    {
                        if (config.OilTypeIds == null || !config.OilTypeIds.Any())
                            return true;

                        //// Since all oil types belong to same sales area, take first one
                        //var firstOilTypeId = config.OilTypeIds.First();

                        //if (!oilTypeLookup.TryGetValue(firstOilTypeId, out var oilTypeArea))
                        //    return false;

                        //// Match Sales Area
                        //return userSalesAreas.Any(u =>
                        //    u.SalesOrganizationId == oilTypeArea.SalesOrganizationId &&
                        //    u.DistributionChannelId == oilTypeArea.DistributionChannelId &&
                        //    u.DivisionId == oilTypeArea.DivisionId
                        //);

                        // Keep the record if ANY oil type matches user sales area
                        return config.OilTypeIds.Any(oilTypeId =>
                        {
                            if (!oilTypeLookup.TryGetValue(oilTypeId, out var oilTypeArea))
                                return false;

                            return userSalesAreas.Any(u =>
                                u.SalesOrganizationId == oilTypeArea.SalesOrganizationId &&
                                u.DistributionChannelId == oilTypeArea.DistributionChannelId &&
                                u.DivisionId == oilTypeArea.DivisionId
                            );
                        });
                    })

                    .ToList();
                }



                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = saudaBookingRestrictionList == null ? new List<SaudaBookingConfigurationListDto> () : saudaBookingRestrictionList;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        #endregion

        #region Sauda Sales Area Restriction Configuration List

        public ResultDto GetSaudaSalesAreaRestrictionConfigurationList(UserIdDto inputDto)
        {
            _methodName = "GetSaudaSalesAreaRestrictionConfigurationList";
            var resultDto = new ResultDto();

            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                if (inputDto.UserId <= 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }

                var userRoleContext = _emamiContext.UserRoles.Where(_ => _.UserId == inputDto.UserId).FirstOrDefault();

                if (userRoleContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserRoleMappingNotExists;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserRoleMappingNotExists, Constants.EnglishLanguage);
                    return resultDto;
                }

                IEnumerable<DivisionDetailsDto> divisionslogieduser = new List<DivisionDetailsDto>();
                if (userRoleContext.RoleId == (int)DTO.Enums.Role.Admin)
                {
                    divisionslogieduser = _emamiContext.Divisions.AsNoTracking().Select(s => new DivisionDetailsDto()
                    {
                        SalesOrganizationId = s.SalesOrganizationId,
                        DistributionChannelId = s.DistributionChannelId,
                        DivisionId = s.Id
                    });
                }
                else
                {
                    divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.UserId)
                 .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });
                }

                var saudaSalesAreaRestrictionConfigList = (from ssar in _emamiContext.SaudaSalesAreaRestrictions.AsNoTracking()
                                                            join dm in divisionslogieduser on new { SalesOrganizationId = ssar.SalesOrganizationId, DistributionChannelId = ssar.DistributionChannelId, DivisionId = ssar.DivisionId }
                                                            equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                                                           select ssar)
                                                            .ToList();

                if (!saudaSalesAreaRestrictionConfigList.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }

                var saudaSalesAreaRestrictionList = saudaSalesAreaRestrictionConfigList.Select(config => new SaudaSalesAreaRestrictionListDto
                {
                    Id = config.Id,
                    EncryptedId = UtilityHelper.ConvertToMd5(config.Id.ToString(), SecurityConstants.EncryptionKey),
                    SalesOrganizationName = config.SalesOrganization.Name,
                    DistributionChannelName = config.DistributionChannel.Name,
                    DivisionName = config.Division.Name,
                    TimeRestriction = config.TimeRestriction,
                    ValidFrom = (DateTime)config.ValidFrom,
                    ValidTo = (DateTime)config.ValidTo,
                    IsActive = config.IsActive
                }).OrderByDescending(_ => _.Id).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = saudaSalesAreaRestrictionList == null ? new List<SaudaSalesAreaRestrictionListDto>() : saudaSalesAreaRestrictionList;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        #endregion

        #region Sauda Modification

        public ResultDto GetSaudaModificationList(SaudaListFilterDto saudaFilterDto)
        {
            _methodName = "GetSaudaModificationList";
            var resultDto = new ResultDto();

            try
            {
                if (saudaFilterDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                if (saudaFilterDto.LoginUserId <= 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.DealerIdEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.DealerIdEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }

                if (saudaFilterDto.FromDate == null || saudaFilterDto.FromDate == DateTime.MinValue)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.FromDateEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.FromDateEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (saudaFilterDto.ToDate == null || saudaFilterDto.ToDate == DateTime.MinValue)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.ToDateEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.ToDateEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }
                var roleId = _emamiContext.UserRoles.Where(_ => _.UserId == saudaFilterDto.LoginUserId).FirstOrDefault().RoleId;

                IEnumerable<DivisionDetailsDto> divisionslogieduser = new List<DivisionDetailsDto>();
                if (roleId == (int)DTO.Enums.Role.Admin)
                {
                    divisionslogieduser = _emamiContext.Divisions.AsNoTracking().Select(s => new DivisionDetailsDto()
                    {
                        SalesOrganizationId = s.SalesOrganizationId,
                        DistributionChannelId = s.DistributionChannelId,
                        DivisionId = s.Id
                    });
                }
                else
                {
                    divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == saudaFilterDto.LoginUserId)
                 .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });
                }

                var saudaIds = new List<long>();
                var createdBy = new List<long>();
                var loginUserRole = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == saudaFilterDto.LoginUserId);
                if (loginUserRole != null)
                {
                    if (loginUserRole.RoleId == (int)DTO.Enums.Role.NationalTrader || loginUserRole.RoleId == (int)DTO.Enums.Role.ZonalTrader || loginUserRole.RoleId == (int)DTO.Enums.Role.StateTrader)
                    {
                        var ReportingUsers = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == saudaFilterDto.LoginUserId).ToList();
                        if (loginUserRole.RoleId == (int)DTO.Enums.Role.NationalTrader)
                        {
                            createdBy = _emamiContext.UserReportingToMappings.AsNoTracking().Where(user => user.ReportingToUserId == saudaFilterDto.LoginUserId).Select(_ => _.UserId).ToList();
                        }
                        if (loginUserRole.RoleId == (int)DTO.Enums.Role.ZonalTrader)
                        {
                            createdBy = _emamiContext.UserReportingToMappings.AsNoTracking().Where(user => user.ReportingToUserId == saudaFilterDto.LoginUserId).Select(_ => _.UserId).ToList();
                        }
                        if (loginUserRole.RoleId == (int)DTO.Enums.Role.StateTrader)
                        {
                            createdBy = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == saudaFilterDto.LoginUserId).Select(s => s.CustomerId).ToList();
                        }
                        var saudabeforeCobinationCheck = _emamiContext.SaudaModificationApprovals.AsNoTracking().
                             Where(_ => createdBy.Contains(_.CreatedBy) && _.StatusId == (int)DTO.Enums.Status.Pending).Select(_ => _.SaudaModification.SaudaNumber).ToList();

                        saudaIds = (from s in _emamiContext.Sauda.AsNoTracking()
                                    join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                                    equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                                    where saudabeforeCobinationCheck.Contains(s.SaudaNumber)
                                    select s.Id).ToList();
                    }

                }

                var description = UtilityHelper.GetEnumDescription(DTO.Enums.Configuration.InboundInterfacenotSyncedToSAPMinutes);
                var configurationContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Key == description).Value;
                var remarksContext = _emamiContext.Remarks.AsNoTracking();

                IEnumerable<SaudaListDto> saudaqueryContext = new List<SaudaListDto>();
                if (saudaFilterDto.StatusId > 0)
                {
                    var saudaQuery = @"CREATE TABLE #DealerIdsTemp(DealerId BIGINT) 
                    IF(@RoleId = 12) -- NH  
                    BEGIN  
                    INSERT INTO #DealerIdsTemp(DealerId)  
                    Select DISTINCT cus.Id as DealerId  
                    From UserReportingToMappings zh with(NOLOCK)
                    INNER JOIN UserReportingToMappings bdo with(NOLOCK) ON zh.UserId = bdo.ReportingToUserId  
                    INNER JOIN UserCustomerMappings ucm with(NOLOCK) ON ucm.UserId = bdo.UserId  
                    INNER JOIN Users cus with(NOLOCK) ON ucm.CustomerId = cus.Id  
                    Where zh.ReportingToUserId = @LoginUserId 
                    END  
                    ELSE IF(@RoleId = 9) -- ZH  
                    BEGIN  
                    INSERT INTO #DealerIdsTemp(DealerId)  
                    Select DISTINCT 
                    cus.Id as DealerId From UserReportingToMappings bdo  
                    INNER JOIN UserCustomerMappings ucm with(NOLOCK) ON ucm.UserId = bdo.UserId  
                    INNER JOIN Users cus with(NOLOCK) ON ucm.CustomerId = cus.Id  
                    Where bdo.ReportingToUserId = @LoginUserId 
                    END  
                    ELSE IF(@RoleId = 7) --BDO  
                    BEGIN  
                    INSERT INTO #DealerIdsTemp(DealerId)  
                    Select DISTINCT cus.Id as DealerId   
                    From UserCustomerMappings ucm with(NOLOCK) 
                    JOIN Users cus with(NOLOCK) ON ucm.CustomerId = cus.Id  
                    Where ucm.UserId = @LoginUserId 
                    END
                    ELSE -- Admin  
                    BEGIN  
                    INSERT INTO #DealerIdsTemp(DealerId)  
                    select u.Id from Users u
                    join UserRoles ur on u.Id=ur.UserId and ur.RoleId=5
                    join UserCustomerMappings uc on u.Id=uc.CustomerId
                    join Users bdo with(NOLOCK) on uc.UserId=bdo.Id
                    join UserRoles urb with(NOLOCK) on urb.UserId=bdo.Id
                    where urb.RoleId=7  
                    END  

                    Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                    if(@RoleId = 1)
                    begin
	                    insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) Select SalesOrganizationId,DistributionChannelId,Id as DivisionId from Divisions 
                    end
                    else
                    begin
	                    insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@LoginUserId
                    end


                    Select s.SaudaType as SaudaTypeId,s.CreatedDate,so.Id,s.Id as SaudaId,sm.Id as SaudaModificationId,s.SaudaNumber,sm.CreatedDate as BiddingDate,s.UserId,
                    dealer.Name as DealerName,created.Name as CreatedBy,sm.IsSentToSAP as IsSAPDataSync,r.IsActive as IsActiveRemarks,
                    so.IsSapSauda,so.IsSapSaudaNumberUpdateSync,sm.StatusId,s.SalesOrganizationId,s.DistributionChannelId,s.DivisionId,so.ModifiedDate,
                    z.Name as Zones,
                    state.StateName As States,
                    dist.DistrictName As Districts,
                    city.CityName As cities
                    from SaudaModifications sm with(NOLOCK)
                    join Saudas s with(NOLOCK) on sm.SaudaNumber = s.SaudaNumber
                    join SaudaOrders so with(NOLOCK) on s.Id = so.SaudaId
                    join #UserDivision ud on s.SalesOrganizationId=ud.SalesOrganizationId and s.DistributionChannelId=ud.DistributionChannelId
                    and s.DivisionId=ud.DivisionId join Users dealer with(NOLOCK) on s.UserId = dealer.Id
                    left join Remarks r with(NOLOCK) on so.Id = r.TableId and r.IsActive = 1
                    join Users created with(NOLOCK) on sm.CreatedBy = created.Id 
                    join zones z with(NOLOCK) on z.Id = dealer.ZoneId
                    join States state On state.Id = dealer.StateId
                    join Districts dist On dealer.DistrictId = dist.Id
                    join Cities city On city.Id = dealer.CityId --and city.DistrictId = dist.Id
                    where
                    s.UserId in (select DealerId from #DealerIdsTemp)
                    and Cast(sm.CreatedDate as date) >= Cast(@FromDate as Date)
                    and Cast(sm.CreatedDate as date) <= Cast(@ToDate as Date) 
                    and((@SalesOrganizationId > 0 and s.SalesOrganizationId = @SalesOrganizationId) or @SalesOrganizationId = 0)
                    and((@DistributionChannelId > 0 and s.DistributionChannelId = @DistributionChannelId) or @DistributionChannelId = 0)
                    and((@DivisionId > 0 and s.DivisionId = @DivisionId) or @DivisionId = 0) 
                    and((@SkuId > 0 and so.SkuId = @SkuId) or @SkuId = 0)
                    and((@OilTypeId > 0 and so.OilTypeId = @OilTypeId) or @OilTypeId = 0)
                    AND ((@ZoneId > 0 AND z.Id = @ZoneId) OR @ZoneId = 0)  
                    AND ((@StateId > 0 AND state.Id = @StateId) OR @StateId = 0)  
                    AND ((@DistrictId > 0 AND dist.Id = @DistrictId) OR @DistrictId = 0)
                    AND ((@CityId > 0 AND city.Id = @CityId) OR @CityId = 0)
                    and sm.StatusId=@StatusId
                    drop table #DealerIdsTemp
                    drop table #UserDivision";
                    using (SqlConnection connection = new SqlConnection(Config.DBConnectionString))
                    {
                        try
                        {
                            connection.Open();

                            saudaqueryContext = connection.Query<SaudaListDto>(
                                            saudaQuery,
                                             new
                                             {
                                                 LoginUserId = saudaFilterDto.LoginUserId,
                                                 RoleId = roleId,
                                                 SalesOrganizationId = saudaFilterDto.SalesOrganizationId,
                                                 DistributionChannelId = saudaFilterDto.DistributionChannelId,
                                                 DivisionId = saudaFilterDto.DivisionId,
                                                 FromDate = saudaFilterDto.FromDate,
                                                 ToDate = saudaFilterDto.ToDate,
                                                 SkuId = saudaFilterDto.SkuId,
                                                 OilTypeId = saudaFilterDto.OilTypeId,
                                                 saudaIds = saudaIds,
                                                 StatusId = saudaFilterDto.StatusId,
                                                 ZoneId = saudaFilterDto.ZoneId,
                                                 StateId = saudaFilterDto.StateId,
                                                 DistrictId = saudaFilterDto.DistrictId,
                                                 CityId = saudaFilterDto.CityId
                                             }
                                            ).AsEnumerable();

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
                else if (saudaFilterDto.FromDate.ToString("dd/MM/yyyy") == saudaFilterDto.ToDate.ToString("dd/MM/yyyy") && saudaFilterDto.FromDate.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy"))
                {
                    var saudaQuery = @"CREATE TABLE #DealerIdsTemp(DealerId BIGINT) 
                    IF(@RoleId = 12) -- NH  
                    BEGIN  
                    INSERT INTO #DealerIdsTemp(DealerId)  
                    Select DISTINCT cus.Id as DealerId  
                    From UserReportingToMappings zh with(NOLOCK)
                    INNER JOIN UserReportingToMappings bdo with(NOLOCK) ON zh.UserId = bdo.ReportingToUserId  
                    INNER JOIN UserCustomerMappings ucm with(NOLOCK) ON ucm.UserId = bdo.UserId  
                    INNER JOIN Users cus with(NOLOCK) ON ucm.CustomerId = cus.Id  
                    Where zh.ReportingToUserId = @LoginUserId 
                    END  
                    ELSE IF(@RoleId = 9) -- ZH  
                    BEGIN  
                    INSERT INTO #DealerIdsTemp(DealerId)  
                    Select DISTINCT 
                    cus.Id as DealerId From UserReportingToMappings bdo  
                    INNER JOIN UserCustomerMappings ucm with(NOLOCK) ON ucm.UserId = bdo.UserId  
                    INNER JOIN Users cus with(NOLOCK) ON ucm.CustomerId = cus.Id  
                    Where bdo.ReportingToUserId = @LoginUserId 
                    END  
                    ELSE IF(@RoleId = 7) --BDO  
                    BEGIN  
                    INSERT INTO #DealerIdsTemp(DealerId)  
                    Select DISTINCT cus.Id as DealerId   
                    From UserCustomerMappings ucm with(NOLOCK) 
                    JOIN Users cus with(NOLOCK) ON ucm.CustomerId = cus.Id  
                    Where ucm.UserId = @LoginUserId 
                    END
                    ELSE -- Admin  
                    BEGIN  
                    INSERT INTO #DealerIdsTemp(DealerId)  
                    select u.Id from Users u
                    join UserRoles ur on u.Id=ur.UserId and ur.RoleId=5
                    join UserCustomerMappings uc on u.Id=uc.CustomerId
                    join Users bdo with(NOLOCK) on uc.UserId=bdo.Id
                    join UserRoles urb with(NOLOCK) on urb.UserId=bdo.Id
                    where urb.RoleId=7  
                    END  

                    Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                    if(@RoleId = 1)
                    begin
	                    insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) Select SalesOrganizationId,DistributionChannelId,Id as DivisionId from Divisions 
                    end
                    else
                    begin
	                    insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@LoginUserId
                    end


                    Select s.SaudaType as SaudaTypeId,
                    s.CreatedDate,so.Id,s.Id as SaudaId,sm.Id as SaudaModificationId,s.SaudaNumber,sm.CreatedDate as BiddingDate,s.UserId,
                    dealer.Name as DealerName,created.Name as CreatedBy,sm.IsSentToSAP as IsSAPDataSync,
                    r.IsActive as IsActiveRemarks,so.IsSapSauda,so.IsSapSaudaNumberUpdateSync,sm.StatusId,s.SalesOrganizationId,s.DistributionChannelId,s.DivisionId,so.ModifiedDate,
                    z.Name as Zones,
                    state.StateName As States,
                    dist.DistrictName As Districts,
                    city.CityName As cities
                    from SaudaModifications sm with(NOLOCK)
                    join Saudas s with(NOLOCK) on sm.SaudaNumber = s.SaudaNumber 
                    join SaudaOrders so with(NOLOCK) on s.Id = so.SaudaId
                    join #UserDivision ud on s.SalesOrganizationId=ud.SalesOrganizationId and s.DistributionChannelId=ud.DistributionChannelId
                    and s.DivisionId=ud.DivisionId
                    join Users dealer with(NOLOCK) on s.UserId = dealer.Id 
                    left join Remarks r with(NOLOCK) on so.Id = r.TableId and r.IsActive = 1
                    join Users created with(NOLOCK) on sm.CreatedBy = created.Id 
                    join zones z with(NOLOCK) on z.Id = dealer.ZoneId
                    join States state On state.Id = dealer.StateId
                    join Districts dist On dealer.DistrictId = dist.Id
                    join Cities city On city.Id = dealer.CityId --and city.DistrictId = dist.Id
                    where 
                    s.UserId in (select DealerId from #DealerIdsTemp)
                    and Cast(sm.CreatedDate as date) = Cast(@FromDate as Date)
                    and((@SalesOrganizationId > 0 and s.SalesOrganizationId = @SalesOrganizationId) or @SalesOrganizationId = 0)
                    and((@DistributionChannelId > 0 and s.DistributionChannelId = @DistributionChannelId) or @DistributionChannelId = 0)
                    and((@DivisionId > 0 and s.DivisionId = @DivisionId) or @DivisionId = 0) 
                    and((@SkuId > 0 and so.SkuId = @SkuId) or @SkuId = 0)
                    and((@OilTypeId > 0 and so.OilTypeId = @OilTypeId) or @OilTypeId = 0)
                    AND ((@ZoneId > 0 AND z.Id = @ZoneId) OR @ZoneId = 0)  
                    AND ((@StateId > 0 AND state.Id = @StateId) OR @StateId = 0)  
                    AND ((@DistrictId > 0 AND dist.Id = @DistrictId) OR @DistrictId = 0)
                    AND ((@CityId > 0 AND city.Id = @CityId) OR @CityId = 0)
                    drop table #DealerIdsTemp
                    drop table #UserDivision";

                    using (SqlConnection connection = new SqlConnection(Config.DBConnectionString))
                    {
                        try
                        {
                            connection.Open();

                            saudaqueryContext = connection.Query<SaudaListDto>(
                                            saudaQuery,
                                             new
                                             {
                                                 LoginUserId = saudaFilterDto.LoginUserId,
                                                 RoleId = roleId,
                                                 SalesOrganizationId = saudaFilterDto.SalesOrganizationId,
                                                 DistributionChannelId = saudaFilterDto.DistributionChannelId,
                                                 DivisionId = saudaFilterDto.DivisionId,
                                                 FromDate = saudaFilterDto.FromDate,
                                                 SkuId = saudaFilterDto.SkuId,
                                                 OilTypeId = saudaFilterDto.OilTypeId,
                                                 saudaIds = saudaIds,
                                                 ZoneId = saudaFilterDto.ZoneId,
                                                 StateId = saudaFilterDto.StateId,
                                                 DistrictId = saudaFilterDto.DistrictId,
                                                 CityId = saudaFilterDto.CityId
                                             }
                                            ).AsEnumerable();

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
                else
                {
                    var saudaQuery = @"CREATE TABLE #DealerIdsTemp(DealerId BIGINT) 
                            IF(@RoleId = 12) -- NH  
                            BEGIN  
                            INSERT INTO #DealerIdsTemp(DealerId)  
                            Select DISTINCT cus.Id as DealerId  
                            From UserReportingToMappings zh with(NOLOCK)
                            INNER JOIN UserReportingToMappings bdo with(NOLOCK) ON zh.UserId = bdo.ReportingToUserId  
                            INNER JOIN UserCustomerMappings ucm with(NOLOCK) ON ucm.UserId = bdo.UserId  
                            INNER JOIN Users cus with(NOLOCK) ON ucm.CustomerId = cus.Id  
                            Where zh.ReportingToUserId = @LoginUserId 
                            END  
                            ELSE IF(@RoleId = 9) -- ZH  
                            BEGIN  
                            INSERT INTO #DealerIdsTemp(DealerId)  
                            Select DISTINCT 
                            cus.Id as DealerId From UserReportingToMappings bdo  
                            INNER JOIN UserCustomerMappings ucm with(NOLOCK) ON ucm.UserId = bdo.UserId  
                            INNER JOIN Users cus with(NOLOCK) ON ucm.CustomerId = cus.Id  
                            Where bdo.ReportingToUserId = @LoginUserId 
                            END  
                            ELSE IF(@RoleId = 7) --BDO  
                            BEGIN  
                            INSERT INTO #DealerIdsTemp(DealerId)  
                            Select DISTINCT cus.Id as DealerId   
                            From UserCustomerMappings ucm with(NOLOCK) 
                            JOIN Users cus with(NOLOCK) ON ucm.CustomerId = cus.Id  
                            Where ucm.UserId = @LoginUserId 
                            END
                            ELSE -- Admin  
                            BEGIN  
                            INSERT INTO #DealerIdsTemp(DealerId)  
                            select u.Id from Users u
                            join UserRoles ur on u.Id=ur.UserId and ur.RoleId=5
                            join UserCustomerMappings uc on u.Id=uc.CustomerId
                            join Users bdo with(NOLOCK) on uc.UserId=bdo.Id
                            join UserRoles urb with(NOLOCK) on urb.UserId=bdo.Id
                            where urb.RoleId=7     
                            END  
                            
                            Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                            if(@RoleId = 1)
                            begin
                            	insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) Select SalesOrganizationId,DistributionChannelId,Id as DivisionId from Divisions 
                            end
                            else
                            begin
                            	insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@LoginUserId
                            end
                            
                            
                            Select s.SaudaType as SaudaTypeId,
                            s.CreatedDate,
                            so.Id,
                            s.Id as SaudaId,
                            sm.Id as SaudaModificationId,
                            s.SaudaNumber,
                            sm.CreatedDate as BiddingDate,
                            s.UserId,
                            dealer.Name as DealerName,
                            created.Name as CreatedBy,sm.IsSentToSAP as IsSAPDataSync,r.IsActive as IsActiveRemarks,so.IsSapSauda,so.IsSapSaudaNumberUpdateSync,sm.StatusId,s.SalesOrganizationId,s.DistributionChannelId,s.DivisionId,so.ModifiedDate,
                            z.Name as Zones,
                            state.StateName As States,
                            dist.DistrictName As Districts,
                            city.CityName As cities
                            from SaudaModifications sm with(NOLOCK)
                            join Saudas s with(NOLOCK) on sm.SaudaNumber = s.SaudaNumber
                            join SaudaOrders so with(NOLOCK) on s.Id = so.SaudaId
                            join #UserDivision ud on ud.SalesOrganizationId=s.SalesOrganizationId and ud.DistributionChannelId=s.DistributionChannelId
                            and ud.DivisionId=s.DivisionId
                            join Users dealer with(NOLOCK) on s.UserId = dealer.Id 
                            left join Remarks r with(NOLOCK)  on so.Id = r.TableId and r.IsActive = 1
                            join Users created with(NOLOCK) on sm.CreatedBy = created.Id 
                            join zones z with(NOLOCK) on z.Id = dealer.ZoneId
                            join States state On state.Id = dealer.StateId
                            join Districts dist On dealer.DistrictId = dist.Id
                            join Cities city On city.Id = dealer.CityId --and city.DistrictId = dist.Id
                            where 
                            s.UserId in (select DealerId from #DealerIdsTemp)
                            and Cast(sm.CreatedDate as date) >= Cast(@FromDate as Date)
                            and Cast(sm.CreatedDate as date) <= Cast(@ToDate as Date)  and((@SalesOrganizationId > 0 and s.SalesOrganizationId = @SalesOrganizationId) or @SalesOrganizationId = 0)
                            and((@DistributionChannelId > 0 and s.DistributionChannelId = @DistributionChannelId) or @DistributionChannelId = 0)
                            and((@DivisionId > 0 and s.DivisionId = @DivisionId) or @DivisionId = 0) and((@SkuId > 0 and so.SkuId = @SkuId) or @SkuId = 0)
                            and((@OilTypeId > 0 and so.OilTypeId = @OilTypeId) or @OilTypeId = 0) 
                            AND ((@ZoneId > 0 AND z.Id = @ZoneId) OR @ZoneId = 0)  
                            AND ((@StateId > 0 AND state.Id = @StateId) OR @StateId = 0)  
                            AND ((@DistrictId > 0 AND dist.Id = @DistrictId) OR @DistrictId = 0)
                            AND ((@CityId > 0 AND city.Id = @CityId) OR @CityId = 0)
                            drop table #DealerIdsTemp
                            drop table #UserDivision";
                    using (SqlConnection connection = new SqlConnection(Config.DBConnectionString))
                    {
                        try
                        {
                            connection.Open();

                            saudaqueryContext = connection.Query<SaudaListDto>(
                                            saudaQuery,
                                             new
                                             {
                                                 LoginUserId = saudaFilterDto.LoginUserId,
                                                 RoleId = roleId,
                                                 SalesOrganizationId = saudaFilterDto.SalesOrganizationId,
                                                 DistributionChannelId = saudaFilterDto.DistributionChannelId,
                                                 DivisionId = saudaFilterDto.DivisionId,
                                                 FromDate = saudaFilterDto.FromDate,
                                                 ToDate = saudaFilterDto.ToDate,
                                                 SkuId = saudaFilterDto.SkuId,
                                                 OilTypeId = saudaFilterDto.OilTypeId,
                                                 saudaIds = saudaIds,
                                                 ZoneId = saudaFilterDto.ZoneId,
                                                 StateId = saudaFilterDto.StateId,
                                                 DistrictId = saudaFilterDto.DistrictId,
                                                 CityId = saudaFilterDto.CityId
                                             }
                                            ).AsEnumerable();

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

                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                //Data Filter
                List<SaudaListDto> saudaList = new List<SaudaListDto>();
                if (saudaFilterDto.DataFilter == (int)DTO.Enums.Status.Approved && saudaqueryContext.IsAny())
                {
                    saudaList = saudaqueryContext.Where(_ => _.StatusId == (int)DTO.Enums.Status.Pending && !_.IsSAPDataSync && saudaIds.Contains(_.SaudaId)).ToList();
                }
                else if (saudaFilterDto.DataFilter == 0 && saudaqueryContext.IsAny()) //Reprocess after sauda approval
                {
                    saudaList = saudaqueryContext.Where(_ => _.StatusId != (int)DTO.Enums.Status.Rejected && 
                    (_.IsSAPDataSync && currentDate.Subtract(Convert.ToDateTime(_.ModifiedDate)).TotalMinutes > Convert.ToDouble(configurationContext))).ToList();
                }
                else
                {
                    saudaList = saudaqueryContext.ToList();
                }

                var saudaList1 = saudaList.OrderByDescending(_ => _.Id).GroupBy(_ => _.SaudaId).Select(s => s.First()).ToList();
                var datasourceresult = saudaList1.ToDataSourceResult(saudaFilterDto.DataSourceRequest);
                List<SaudaListDto> saudaListDtos = new List<SaudaListDto>();
                var saudaListafterPagination = datasourceresult.Data as List<SaudaListDto>;
                if (saudaListafterPagination.IsAny())
                {

                    var saudaModificationIds = saudaListafterPagination
                    .Select(x => x.SaudaModificationId)
                    .Distinct()
                    .ToList();

                    var latestApprovals = _emamiContext.SaudaModificationApprovals
                    .AsNoTracking()
                    .Where(a => saudaModificationIds.Contains(a.SaudaModificationId))
                    .GroupBy(a => a.SaudaModificationId)
                    .Select(g => g
                        .OrderByDescending(x => x.CreatedDate)
                        .Select(x => new
                        {
                            x.SaudaModificationId,
                            x.RequestedTo
                        })
                        .FirstOrDefault()
                    )
                    .Where(x => x != null)
                    .ToList();

                    var approverUserIds = latestApprovals
                    .Select(x => x.RequestedTo)
                    .Distinct()
                    .ToList();

                    var users = _emamiContext.Users
                        .AsNoTracking()
                        .Where(u => approverUserIds.Contains(u.Id))
                        .Select(u => new { u.Id, u.Name })
                        .ToList();

                    var approvalLookup = latestApprovals
                        .ToDictionary(x => x.SaudaModificationId, x => x.RequestedTo);

                    var userLookup = users
                        .ToDictionary(x => x.Id, x => x.Name);

                    foreach (var sauda in saudaListafterPagination)
                    {
                        sauda.EncryptedId = UtilityHelper.ConvertToMd5(sauda.SaudaModificationId.ToString(), SecurityConstants.EncryptionKey);
                        sauda.DiscountType = sauda.DiscountTypeId != 0 ? Enum.GetName(typeof(SaudaDiscountType), sauda.DiscountTypeId) : "";
                        sauda.SaudaBookingType = sauda.SaudaBookingTypeId != 0 ? Enum.GetName(typeof(SaudaBookingTypes), sauda.SaudaBookingTypeId) : "";
                        sauda.SaudaType = sauda.SaudaTypeId == 0 ? string.Empty : ((DTO.Enums.SaudaType)sauda.SaudaTypeId).ToString();
                        TimeSpan difference = currentDate.Subtract(Convert.ToDateTime(sauda.ModifiedDate));

                        if (sauda.IsSAPDataSync && !sauda.IsSapSauda && difference.TotalMinutes > Convert.ToDouble(configurationContext) && !sauda.IsSapSaudaNumberUpdateSync)
                        {
                            sauda.IsSapSyncNotReceivedForSaudaNumber = true;
                            sauda.Remarks = "Update Sync not Received From Sap";
                        }

                        sauda.ApprovalUser =
                        approvalLookup.TryGetValue(sauda.SaudaModificationId, out var approverUserId)
                        && userLookup.TryGetValue(approverUserId, out var approverName)
                            ? approverName
                            : string.Empty;


                        saudaListDtos.Add(sauda);
                    }

                    datasourceresult.Data = saudaListDtos;
                }

                if (!saudaListDtos.IsAny())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = datasourceresult;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetSaudaModificationDetailsById(IdInputDto idInputDto)
        {
            _methodName = "GetSaudaModificationDetailsById";
            var resultDto = new ResultDto();
            var outputDto = new SaudaModificationsListsDto();
            try
            {
                if (idInputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var modificationLines = _emamiContext.SaudaModificationLines
                .Where(l => l.SaudaModificationId == idInputDto.Id)
                .Include(l => l.OilType)
                .ToList();

                if (modificationLines == null || modificationLines.Count == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                outputDto.SaudaModificationNewItemsList = new List<SaudaModificationNewItemDto>();

                foreach (var line in modificationLines)
                {
                    var oilTypeName = line.OilType != null ? line.OilType.Name : string.Empty;
                    var oilPackGroupTypeName = (line.OilPackGroupTypeId == 0 ? "Unknown" : (line.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP ? "BP" : (line.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP ? "CP" : "Unknown")));

                    var newItems = _emamiContext.SaudaModificationItems
                        .Where(i => i.SaudaModificationLineId == line.Id)
                        .Include(i => i.Sku)
                        .ToList();

                    foreach (var item in newItems)
                    {
                        var newItemDto = new SaudaModificationNewItemDto
                        {
                            OilTypeName = oilTypeName,
                            OilPackGroupTypeName = oilPackGroupTypeName,
                            MaterialName = item.Sku != null ? item.Sku.SkuName : string.Empty,
                            MaterialCode = item.Sku != null ? item.Sku.SkuCode : string.Empty,
                            QuantityInCase = item.QuantityInCase,
                            QuantityInMT = item.SaudaQuantity,
                            Price = item.Price,
                            Discount = item.Discount
                        };

                        outputDto.SaudaModificationNewItemsList.Add(newItemDto);
                    }
                }


                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetSaudhaModificationDetails(SaudaDetailInputDto inputDto)
        {
            _methodName = "GetSaudhaModificationDetails";
            var resultDto = new ResultDto();
            var saudaDetails = new SaudaListDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var saudaModification = _emamiContext.SaudaModifications.AsNoTracking()
                    .Where(sm => sm.Id == inputDto.SaudaId)
                    .FirstOrDefault();

                if (saudaModification == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                // Get the original Sauda to get dealer information
                var sauda = _emamiContext.Sauda.AsNoTracking()
                    .Where(s => s.SaudaNumber == saudaModification.SaudaNumber)
                    .FirstOrDefault();

                if (sauda != null)
                {
                    var dealer = _emamiContext.Users.AsNoTracking()
                        .Where(u => u.Id == sauda.UserId)
                        .FirstOrDefault();

                    var createdByUser = _emamiContext.Users.AsNoTracking()
                        .Where(u => u.Id == saudaModification.CreatedBy)
                        .FirstOrDefault();

                    var zone = dealer != null && dealer.ZoneId.HasValue ? _emamiContext.Zones.AsNoTracking()
                        .Where(z => z.Id == dealer.ZoneId.Value)
                        .FirstOrDefault() : null;

                    var state = dealer != null ? _emamiContext.State.AsNoTracking()
                        .Where(s => s.Id == dealer.StateId)
                        .FirstOrDefault() : null;

                    var district = dealer != null ? _emamiContext.District.AsNoTracking()
                        .Where(d => d.Id == dealer.DistrictId)
                        .FirstOrDefault() : null;

                    var city = dealer != null ? _emamiContext.City.AsNoTracking()
                        .Where(c => c.Id == dealer.CityId)
                        .FirstOrDefault() : null;

                    saudaDetails = new SaudaListDto()
                    {
                        SaudaId = saudaModification.Id,
                        SaudaNumber = saudaModification.SaudaNumber,
                        SaudaBookedNumber = saudaModification.Id,
                        BiddingDate = saudaModification.ModifiedDate ?? saudaModification.CreatedDate, // Use ModifiedDate as "Modification Date"
                        CreatedDate = saudaModification.CreatedDate,
                        UserId = sauda != null ? sauda.UserId : 0,
                        DealerName = dealer != null ? dealer.Name : string.Empty,
                        CreatedBy = createdByUser != null ? createdByUser.Name : string.Empty, // Sauda Modification Created By
                        StatusId = saudaModification.StatusId,
                        SalesOrganizationId = sauda != null ? sauda.SalesOrganizationId : 0,
                        DistributionChannelId = sauda != null ? sauda.DistributionChannelId : 0,
                        DivisionId = sauda != null ? sauda.DivisionId : 0,
                        Zones = zone != null ? zone.Name : string.Empty,
                        States = state != null ? state.StateName : string.Empty,
                        Districts = district != null ? district.DistrictName : string.Empty,
                        Cities = city != null ? city.CityName : string.Empty
                    };
                }
                else
                {
                    // If Sauda not found, still return basic modification info
                    var createdByUser = _emamiContext.Users.AsNoTracking()
                        .Where(u => u.Id == saudaModification.CreatedBy)
                        .FirstOrDefault();

                    saudaDetails = new SaudaListDto()
                    {
                        SaudaId = saudaModification.Id,
                        SaudaNumber = saudaModification.SaudaNumber,
                        SaudaBookedNumber = 0,
                        BiddingDate = saudaModification.ModifiedDate ?? saudaModification.CreatedDate,
                        CreatedDate = saudaModification.CreatedDate,
                        CreatedBy = createdByUser != null ? createdByUser.Name : string.Empty,
                        StatusId = saudaModification.StatusId
                    };
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = saudaDetails;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetSaudaModificationReport(SaudaOrderReportInputputDto inputDto)
        {
            _methodName = "GetSaudaModificationReport";
            var resultDto = new ResultDto();
            var reportList = new List<SaudaModificationReportOutputDto>();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var roleId = _emamiContext.UserRoles.Where(_ => _.UserId == inputDto.LoginUserId).FirstOrDefault()?.RoleId ?? 0;
                var stateIds = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.StateIds);
                var statusIds = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.StatusIds);

                using (var connection = new System.Data.SqlClient.SqlConnection(Config.DBConnectionString))
                {
                    connection.Open();
                    reportList = connection.Query<SaudaModificationReportOutputDto>("GetSaudaModificationReport", new
                    {
                        @RoleId = roleId,
                        @LoginUserId = inputDto.LoginUserId,
                        @FromDate = inputDto.FromDate,
                        @ToDate = inputDto.ToDate,
                        @StateIds = stateIds,
                        @StatusIds = statusIds,
                        @VerticalId = inputDto.VerticalId,
                        @SalesOrganizationId = inputDto.SalesOrganizationId,
                        @DistributionChannelId = inputDto.DistributionChannelId
                    }, commandType: System.Data.CommandType.StoredProcedure, commandTimeout: 0).ToList();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = reportList;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        #endregion
    }
}
