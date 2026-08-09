using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
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
using System.Web.Hosting;
using System.Web.Script.Serialization;
using System.IO;
using System.Net;
using System.Threading;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using Adani.Solution.DTO.QPSDiscount;
using MimeKit;
using System.Data.Entity.Migrations;
using System.Windows.Ink;
using System.Globalization;
using Newtonsoft.Json;

namespace Adani.Solution.Service
{
    public interface IMobileSaudaServices
    {
        ResultDto DealerSaudaDetails(IdInputDto inputDto);
        ResultDto GetDealerSaudaDetails(IdInputDto inputDto);
        ResultDto GetSalesOrderDataDetails(IdInputDto inputDto);
        ResultDto GetDealerDetail(IdInputDto IdDto);

        ResultDto SaudaCreation(SaudaInputDto inputDto);
        ResultDto GetSaudaList(SaudaFilterDto inputDto);
        ResultDto GetSaudaDetails(SaudaDetailInputDto inputDto);
        ResultDto GetSaudaShortViewList(SaudaFilterDto saudaFilterDto);
        ResultDto GetSaudaShortViewDetails(SaudaDetailInputDto inputDto);
        ResultDto GetSkuListForIndentRequest(SkuInputDto skuInputDto);
        ResultDto GetSaudaDetailsTPNew(SaudaDetailInputDto inputDto);

        //Sauda Limit
        ResultDto GetSaudaLimitRequestHistory(IdInputDto inputDto);
        ResultDto GetSaudaLimitRequestHistoryDetail(IdInputDto inputDto);
        ResultDto AddSaudaLimitRequest(SaudaLimitRequestHistoryDto saudaLimitRequestHistoryDto);
        ResultDto AddSaudaLimitHistory(SaudaLimitHistoryDto inputDto);
        ResultDto GetSaudaLimitHistoryList(SaudaLimitHistoryDto inputDto);

        //Sauda Amendment
        ResultDto GetSaudaListForAmendment(IdInputDto inputDto);
        ResultDto SaveSaudaAmendment(SaudaAmendmentInputDto inputDto);

        //Sauda Chart
        ResultDto GetDealerOutstandingSaudaListForChart(LoginUserIdDto inputDto);
        ResultDto GetBodOutstandingSaudaListForChart(LoginUserIdDto inputDto);
        ResultDto GetOutStandingSaudaList(SaudaFilterDto saudaFilterDto);

        ResultDto GetDealerSaudaLists(IdInputDto IdDto);
        ResultDto GetDealerSalesLists(IdInputDto IdDto);

        ResultDto GetPendingSaudaListForMobile(SaudaFilterDto saudaFilterDto);
        ResultDto GetExpiredSaudaListForMobile(SaudaFilterDto saudaFilterDto);
        ResultDto GetDealerLocationsByDealerId(IdInputDto inputDto);

        //Special Rate Approval Request
        ResultDto AddSpecialRateApprovalRequest(SpecialRateApprovalAddDto specialRateApprovalInputDto);
        ResultDto GetSpecialRateRequestList(SpecialRateInputDto specialRateInputDto);
        ResultDto GetSpecialRateRequestDetails(SpecialRateDetailInputDto specialRateDetailInputDto);
        ResultDto SaudaCreationFromSpecialRate(SpecialRateSaudaDto inputDto);
        ResultDto GetSpecialRateRequestListNew(SpecialRateInputDto specialRateInputDto);


        ResultDto GetPendingSaudaChartForMobile(LoginUserIdDto loginUserIdDto);

        //CompetitorAnalysis
        ResultDto SaveCompetitorAnalysis(CompetitorAnalysisInputDto competitorAnalysisInputDto);

        //Sauda Conversion
        ResultDto AddSaudaConversionOrders(SaudaConversionAddDto saudaConversionAddDto);
        ResultDto GetSaudaConversionList(SaudaFilterDto saudaFilterDto);
        ResultDto GetSaudaConversionDetails(SaudaConversionDetailInputDto inputDto);
        ResultDto AddSaudaConversionUnitAndDifferenceRate(SaudaConversionUnitAndDifferenceRateAddDto SaudaConversionUnitAndDifferenceRateAddDto);

        //Sauda Extension
        ResultDto AddSaudaExtension(SaudaExtensionAddDto saudaExtensionAddDto);
        ResultDto GetSaudaExtensionList(SaudaFilterDto saudaFilterDto);

        ResultDto GetPendingSaudaChartDetailForMobile(LoginUserIdDto loginUserIdDto);
        ResultDto GetBookedSauda(BookedSaudaInputDto loginUserIdDto);
        ResultDto GetSaudaorderdetails(SaudaDetailInputDto inputDto);

        ResultDto GetSaudaNumberList(LoginUserIdDto inputDto);
        //ResultDto SaudaCounterBidOfferDetails(SaudaCounterBidOfferDetailsInputDto inputDto);

        //Counter Bid
        //ResultDto GetSaudaCounterBidDetails(SaudaDetailInputDto inputDto);
        //ResultDto ApproveCounterBid(CounterBidInputDto inputDto);

        //PushNotification Testing
        ResultDto PushNotificationTesting(LoginUserIdDto inputDto);
        //ResultDto GetCounterBidDetails(SaudaDetailInputDto inputDto);
        ResultDto GetPendingContractChartMobile(LoginUserIdDto loginUserIdDto);

        ResultDto GetExpiredAndNearExpiredSaudaDetails(SaudaDetailInputDto inputDto);
        ResultDto GetSaudaorderdetails1(SaudaDetailInputDto inputDto);
        ResultDto GetSkuListByPackGroupId(SkuDropDownInputDto inputDto);

        //chequeStatus Report
        ResultDto GetChequeStatusReportDetails(ChequeStatusReportInputDto inputDto);

        ResultDto GetFillerskuForIndentRequest(FillerSkuInputDto inputDto);
        ResultDto SpecialRateApproveOrReject(SpecialRateSaudaDto inputDto);
        ResultDto GetSkuListBasedOnVehicleSize(SkuInputDto skuInputDto);

        #region Sauda Extension
        ResultDto PostSaudaExtensionDays(SaudaExtensionDaysDto inputDto);

        #endregion

        ResultDto GetSAPSaudaExtensionList(SAPSaudaInputDto inputDto);
        ResultDto SaudaReleaseToSAP(SAPSaudaInputDto inputDto);
        ResultDto GetSAPSaudaReleaseList(SAPSaudaInputDto inputDto);

        #region Sauda Modification

        ResultDto GetValidPendingContractByDelaerId(UserIdDto inputDto);

        ResultDto GetOilTypesByPendingContractId(SaudaDetailInputDto inputDto);

        ResultDto GetToSkusBasedOnFromSkuOilType(SaudaMofificationFromSkuInfoDto inputDto);

        ResultDto GetPendingContractDetailsByPendingContract(SaudaDetailInputDto inputDto);
        ResultDto GetToSkusForSaudaModification(SaudaMofificationFromSkuDetailsDto inputDto);

        ResultDto SaveSaudaModification(SaudaModificationInputDTO inputDto);
        ResultDto GetSaudaModificationPendingApprovedList(SaudaReportFilterDto inputDto);
        ResultDto GetSaudaModificationDetails(IdInputDto inputDto);
        ResultDto GetSaudaModificationApprovalList(SaudaListFilterDto inputDto);
        ResultDto ChangeSaudaModificationStatus(SaudaModificationUpdateDto inputDto);
        ResultDto ChangeSaudaModificationStatusForLoose(SaudaModificationUpdateDto inputDto);

        #endregion
    }

    public class MobileSaudaService : IMobileSaudaServices
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Sauda Service");
        private const string ServiceName = "Sauda Service";
        private string _methodName;
        private readonly IResultService _resultService;
        private readonly INotificationService _notificationService;
        private readonly ISAPIntegrationService _sapIntegrationService;
        private readonly IQpsService _qpsService;
        private readonly ILookupService _lookupService;

        public MobileSaudaService(IAdaniContext salesContext, IResultService resultService, INotificationService notificationService, ISAPIntegrationService sapIntegrationService, IQpsService qpsService, ILookupService lookupService)
        {
            try
            {
                _emamiContext = salesContext;
                _resultService = resultService;
                _notificationService = notificationService;
                _sapIntegrationService = sapIntegrationService;
                _qpsService = qpsService;
                _lookupService = lookupService;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for sauda Service", exception);
            }
        }

        public ResultDto GetDealerSaudaDetails(IdInputDto inputDto)
        {
            _methodName = "GetDealerSaudaDetails";
            var result = new ResultDto();
            var dealerSaudaDetailsDto = new DealerSaudaDataDto();
            if (inputDto == null)
            {
                return NotFoundResult();
            }
            try
            {
                var userRole = _emamiContext.UserRoles.Where(_ => _.UserId == inputDto.Id).FirstOrDefault();
                if (userRole != null)
                {
                    if (userRole.RoleId == (int)DTO.Enums.Role.ZonalTrader || userRole.RoleId == (int)DTO.Enums.Role.StateTrader || userRole.RoleId == (int)DTO.Enums.Role.Dealer)
                    {
                        var overallStatus = Constants.OverallSaudaStatus;
                        var overAllSaudaContext = (from s in _emamiContext.Sauda.AsNoTracking()
                                                   join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                                                   where s.CreatedBy == inputDto.Id
                                                   select new { so, s }
                                                       ).ToList();

                        if (overAllSaudaContext.IsAny())
                        {
                            var saudadetails = overAllSaudaContext.GroupBy(s => new { s.so.SalesOrganizationId, s.so.DistributionChannelId, s.so.DivisionId }).Select(s => new
                            {
                                count = s.Count(),
                                key = s.Key

                            }).OrderByDescending(s => s.count).FirstOrDefault();
                            var plantId = overAllSaudaContext.GroupBy(s => new { s.so.PlantId }).Select(s => new
                            {
                                count = s.Count(),
                                key = s.Key

                            }).OrderByDescending(s => s.count).FirstOrDefault();
                            var stateId = userRole.User.StateId;

                            dealerSaudaDetailsDto.StateId = stateId;
                            dealerSaudaDetailsDto.SalesOrganizationId = saudadetails.key.SalesOrganizationId;
                            dealerSaudaDetailsDto.DistrinbutionChannelId = saudadetails.key.DistributionChannelId;
                            dealerSaudaDetailsDto.DivisionId = saudadetails.key.DivisionId;
                            dealerSaudaDetailsDto.PlantId = plantId.key.PlantId;
                        }



                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.ErrorDto.Message = "Only Zonal Trader and State Trader login is Accessible";
                        return result;
                    }

                }
                else
                {
                    result.IsSuccess = false;
                    result.ErrorDto.Message = "User Not Found";
                    return result;
                }

                result.IsSuccess = true;
                result.SuccessDto.Response = dealerSaudaDetailsDto;
                result.SuccessDto.Message = "Success";
                return result;
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto GetSalesOrderDataDetails(IdInputDto inputDto)
        {
            _methodName = "GetSalesOrderDataDetails";
            var result = new ResultDto();
            var dealerSaudaDetailsDto = new DealerSaudaDataDto();
            if (inputDto == null)
            {
                return NotFoundResult();
            }
            try
            {
                var userRole = _emamiContext.UserRoles.Where(_ => _.UserId == inputDto.Id).FirstOrDefault();
                if (userRole != null)
                {
                    if (userRole.RoleId == (int)DTO.Enums.Role.ZonalTrader || userRole.RoleId == (int)DTO.Enums.Role.StateTrader)
                    {
                        var overallStatus = Constants.OverallSaudaStatus;
                        var overAllData = (from lr in _emamiContext.LiftingRequest.AsNoTracking()
                                           join lrd in _emamiContext.LiftingRequestDetails.AsNoTracking() on lr.Id equals lrd.LiftingRequestId
                                           join sku in _emamiContext.Skus.AsNoTracking() on lrd.SkuId equals sku.Id
                                           where lr.CreatedBy == inputDto.Id
                                           select new { lr, lrd, sku }
                                                       ).ToList();

                        if (overAllData.IsAny())
                        {
                            var saudadetails = overAllData.GroupBy(s => new { s.sku.SalesOrganizationId, s.sku.DistributionChannelId, s.sku.DivisionId }).Select(s => new
                            {
                                count = s.Count(),
                                key = s.Key

                            }).OrderByDescending(s => s.count).FirstOrDefault();
                            var plantId = overAllData.GroupBy(s => new { s.lr.PlantId }).Select(s => new
                            {
                                count = s.Count(),
                                key = s.Key

                            }).OrderByDescending(s => s.count).FirstOrDefault();
                            var stateId = userRole.User.StateId;

                            dealerSaudaDetailsDto.StateId = stateId;
                            dealerSaudaDetailsDto.SalesOrganizationId = saudadetails.key.SalesOrganizationId;
                            dealerSaudaDetailsDto.DistrinbutionChannelId = saudadetails.key.DistributionChannelId;
                            dealerSaudaDetailsDto.DivisionId = saudadetails.key.DivisionId;
                            dealerSaudaDetailsDto.PlantId = plantId.key.PlantId;
                        }


                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.ErrorDto.Message = "Only Zonal Trader and State Trader login is Accessible";
                        return result;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.ErrorDto.Message = "User Not Found";
                    return result;
                }

                result.IsSuccess = true;
                result.SuccessDto.Response = dealerSaudaDetailsDto;
                result.SuccessDto.Message = "Success";

                return result;
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }


        public ResultDto DealerSaudaDetails(IdInputDto inputDto)
        {
            _methodName = "DealerSaudaDetails";
            var dealerSaudaDetailsDto = new DealerSaudaDetailsDto();
            if (inputDto == null)
            {
                return NotFoundResult();
            }
            try
            {
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
                ////Multiplue combination changes
                //var divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                //   .Select(_ => new DivisionDetailsDto
                //   {
                //       SalesOrganizationId = _.SalesOrganizationId,
                //       DistributionChannelId = _.DistributionChannelId,
                //       DivisionId = _.DivisionId
                //   });
                //var overallStatus = Constants.OverallSaudaStatus;
                //var overAllSaudaContext = (from s in _emamiContext.Sauda.AsNoTracking()
                //                           join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                //                           join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                //                           equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                //                           where s.UserId == inputDto.Id && s.SalesOrganizationId == inputDto.SalesOrganizationId && s.DistributionChannelId == inputDto.DistributionChannelId && s.DivisionId == inputDto.DivisionId
                //                           && s.SaudaNumber == null && s.StatusId == (int)DTO.Enums.Status.Pending
                //                           select so).ToList();
                //Old Query
                var overAllSaudaContext = (from s in _emamiContext.Sauda.AsNoTracking()
                                           join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                                           where s.UserId == inputDto.Id && s.SalesOrganizationId == inputDto.SalesOrganizationId && s.DistributionChannelId == inputDto.DistributionChannelId && s.DivisionId == inputDto.DivisionId
                                           && s.SaudaNumber == null && s.StatusId == (int)DTO.Enums.Status.Pending
                                           select so).ToList();

                //var status = Constants.OutstandingSaudaStatus;

                var SaudaLimitContext = (from u in _emamiContext.Users.AsNoTracking().Where(_ => _.Id == inputDto.Id)
                                         join udm in _emamiContext.UserDivisionMappings.AsNoTracking()
                                         .Where(_ => _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId && _.DivisionId == inputDto.DivisionId) on u.Id equals udm.UserId
                                         select new { udm.SaudaValidityPeriod, udm.SaudaLimit, udm.DivisionId }).ToList();


                //var SaudaLimitContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.Id);
                //var SaudaOutstandingContext = (from s in _emamiContext.Sauda.AsNoTracking()
                //                               join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                //                               where s.UserId == inputDto.Id
                //                               && status.Contains(so.StatusId)
                //                               select so
                //                               ).ToList();

                //var SaudaOutstanding = SaudaOutstandingContext.Sum(_ => _.BidQuantity);

                dealerSaudaDetailsDto.TotalSaudaLimit = SaudaLimitContext.Sum(x => x.SaudaLimit ?? 0);
                //dealerSaudaDetailsDto.OutstandingSaudaLimit = SaudaOutstanding;
                //dealerSaudaDetailsDto.AvailableSaudaLimit = SaudaLimitContext.SaudaLimit - SaudaOutstanding;
                //dealerSaudaDetailsDto.AvailableSaudaLimit = SaudaLimitContext.Sum(x => x.SaudaLimit ?? 0) - overAllSaudaContext.Sum(_ => _.BidQuantity);



                //if (overAllSaudaContext != null && overAllSaudaContext.Any())
                //{
                var SaudaOutstanding = overAllSaudaContext.Sum(_ => _.BidQuantity);
                dealerSaudaDetailsDto.OutstandingSaudaLimit = SaudaOutstanding;

                //decimal invoiceQuantity = 0;
                //decimal RtninvoiceQuantity = 0;
                // var existingSaudaQuantity = overAllSaudaContext.Sum(_ => _.BidQuantity);
                //var skuIds = overAllSaudaContext.Select(_ => _.SkuId).Distinct().ToList();
                //var invoiceContext = (from inv in _emamiContext.Invoices.AsNoTracking()
                //                      join invDet in _emamiContext.InvoiceDetails.AsNoTracking() on inv.Id equals invDet.InvoiceId
                //                      where inv.UserId == inputDto.Id && inv.SalesDocumentType != "ZHCR"
                //                      && skuIds.Contains(invDet.SkuId)
                //                      select invDet
                //                          ).ToList();

                //var rtninvoiceContext = (from inv in _emamiContext.Invoices.AsNoTracking()
                //                         join invDet in _emamiContext.InvoiceDetails.AsNoTracking() on inv.Id equals invDet.InvoiceId
                //                         where inv.UserId == inputDto.Id && inv.SalesDocumentType == "ZHCR"
                //                         && skuIds.Contains(invDet.SkuId)
                //                         select invDet
                //                          ).ToList();

                //if (invoiceContext != null && invoiceContext.Any())
                //{
                //    invoiceQuantity = invoiceContext.Sum(_ => _.ActualBilledQuantity);
                //}
                //if (rtninvoiceContext != null && rtninvoiceContext.Any())
                //{
                //    RtninvoiceQuantity = rtninvoiceContext.Sum(_ => _.ActualBilledQuantity);
                //}
                var usersaudalimit = (SaudaLimitContext.Sum(x => x.SaudaLimit ?? 0));
                // var pendingContracttableValue = _emamiContext.PendingContracts.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.Id && _.SalesOrgId == inputDto.SalesOrganizationId && _.DistChnlId == inputDto.DistributionChannelId && _.DivisionId == inputDto.DivisionId) != null ? _emamiContext.PendingContracts.AsNoTracking().Where(_ => _.UserId == inputDto.Id && _.SalesOrgId == inputDto.SalesOrganizationId && _.DistChnlId == inputDto.DistributionChannelId && _.DivisionId == inputDto.DivisionId).Select(_ => _.SaudaQuantity).Sum() : 0;
                dealerSaudaDetailsDto.AvailableSaudaLimit = _resultService.AvailableSaudaLimit(inputDto.Id, usersaudalimit, inputDto.SalesOrganizationId, inputDto.DistributionChannelId, inputDto.DivisionId);
                //}

                var incoTermList =
                         (from incoterm in _emamiContext.IncoTerms.AsNoTracking()
                          join userIncoterm in _emamiContext.UserIncoTerms.AsNoTracking() on incoterm.Id equals userIncoterm.IncoTermsId
                          where userIncoterm.UserId == inputDto.Id && incoterm.IsActive
                          select new IncoTermsDto
                          {
                              Id = incoterm.Id,
                              Name = incoterm.Name,
                              Code = incoterm.Code,
                              Type = incoterm.Type,
                              IsActive = incoterm.IsActive
                          }).ToList();

                if (incoTermList != null && incoTermList.Any())
                {
                    dealerSaudaDetailsDto.IncoTermList = incoTermList;
                }

                //var depotList =
                //             (from depot in _emamiContext.Depots.AsNoTracking()
                //              join depotMapping in _emamiContext.UserDepotMapping.AsNoTracking() on depot.Id equals depotMapping.DepotId
                //              where depotMapping.UserId == inputDto.Id && depot.IsActive
                //              select new DepotDto
                //              {
                //                  Id = depot.Id,
                //                  Name = depot.Name,
                //                  Code = depot.Code,
                //                  IsPlant = depot.IsPlant,
                //                  IsActive = depot.IsActive
                //              }).ToList();

                //if (depotList != null && depotList.Any())
                //{
                //    dealerSaudaDetailsDto.PlantDepotList = depotList;
                //}


                var PlantDepotList = new List<DepotDto>();
                var plantList =
                            (from depot in _emamiContext.Depots.AsNoTracking()
                             join userDivisionDepotMapping in _emamiContext.UserDivisionDepotMappings.AsNoTracking() on depot.Id equals userDivisionDepotMapping.DepotId
                             join userDivisionMapping in _emamiContext.UserDivisionMappings.AsNoTracking() on userDivisionDepotMapping.UserDivisionId equals userDivisionMapping.Id
                             where userDivisionMapping.UserId == inputDto.Id && userDivisionMapping.SalesOrganizationId == inputDto.SalesOrganizationId &&
                             userDivisionMapping.DistributionChannelId == inputDto.DistributionChannelId && userDivisionMapping.DivisionId == inputDto.DivisionId
                             && depot.IsActive && depot.IsPlant
                             select new DepotDto
                             {
                                 Id = depot.Id,
                                 Name = depot.Name + "-" + depot.Code,
                                 Code = depot.Code,
                                 IsPlant = depot.IsPlant,
                                 IsActive = depot.IsActive
                             }).ToList();

                //var plantList =
                //            (from depot in _emamiContext.Depots.AsNoTracking()
                //             join depotMapping in _emamiContext.UserDepotMapping.AsNoTracking() on depot.Id equals depotMapping.DepotId
                //             where depotMapping.UserId == inputDto.Id && depot.IsActive && depot.IsPlant
                //             select new DepotDto
                //             {
                //                 Id = depot.Id,
                //                 Name = depot.Name + "-" + depot.Code,
                //                 Code = depot.Code,
                //                 IsPlant = depot.IsPlant,
                //                 IsActive = depot.IsActive
                //             }).ToList();

                if (plantList != null && plantList.Any())
                {
                    PlantDepotList.AddRange(plantList);
                }


                List<DepotDto> list = null;
                if (PlantDepotList != null && PlantDepotList.Any())
                {
                    list = PlantDepotList
                    .GroupBy(a => a.Id)
                    .Select(g => g.First())
                    .ToList();
                    dealerSaudaDetailsDto.PlantDepotListNew = list;
                }

                var brokerContext = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.CustomerId == inputDto.Id)
                    .Join(_emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.Broker), uc => uc.UserId, ur => ur.UserId, (uc, ur) => new { BrokerId = uc.UserId })
                    .Join(_emamiContext.Users.AsNoTracking(), x => x.BrokerId, u => u.Id, (x, u) => new { BrokerId = u.Id, BrokerName = u.Name }).Select(a => new DropDownDto
                    {
                        Id = a.BrokerId,
                        Name = a.BrokerName
                    }).ToList();
                if (brokerContext.IsAny())
                {
                    dealerSaudaDetailsDto.BrokerList = brokerContext;
                }
                dealerSaudaDetailsDto.SaudaValidityPeriod = SaudaLimitContext.Select(x => x.SaudaValidityPeriod).FirstOrDefault() ?? 0;
                //if (brokerContext != null)
                //{
                //    dealerSaudaDetailsDto.BrokerId = brokerContext.BrokerId;
                //    dealerSaudaDetailsDto.Broker = brokerContext.BrokerName;
                //
                return SucessResult(dealerSaudaDetailsDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto GetDealerDetail(IdInputDto IdDto)
        {
            _methodName = "GetDealerDetail";
            var resultDto = new ResultDto();
            var outputDto = new DealerHubDetailDto();
            try
            {
                if (IdDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (IdDto.Id == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == IdDto.LoginUserId);

                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }
                var roleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == IdDto.LoginUserId);

                if (roleContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }
                var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == IdDto.Id);

                if (dealerContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }
                IEnumerable<DivisionDetailsDto> divisionslogieduser = new List<DivisionDetailsDto>();
                if (roleContext.RoleId == (int)DTO.Enums.Role.Admin)
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
                    divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == IdDto.LoginUserId)
                 .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });
                }

                var bdoIds = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                              where ucm.CustomerId == IdDto.Id
                              select ucm.UserId
                           );


                //var userdivContext = _emamiContext.UserDivisionMappings.AsNoTracking()
                //    .Where(_ => _.UserId == IdDto.Id
                //    && (_.SalesOrganizationId == IdDto.SalesOrganizationId || IdDto.SalesOrganizationId == 0)
                //    && (_.DistributionChannelId == IdDto.DistributionChannelId || IdDto.DistributionChannelId == 0)
                //    && (_.DivisionId == IdDto.DivisionId || IdDto.DivisionId == 0));

                var status = Constants.OutstandingSaudaStatus;
                var SaudaOutstandingContext = (from s in _emamiContext.Sauda.AsNoTracking()
                                               join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                                               join ud in divisionslogieduser on new { s.SalesOrganizationId, s.DistributionChannelId, s.DivisionId }
                                               equals new { ud.SalesOrganizationId, ud.DistributionChannelId, ud.DivisionId }
                                               where s.UserId == IdDto.Id
                                               //&& status.Contains(so.StatusId)
                                               && so.StatusId != (int)DTO.Enums.Status.Rejected
                                               //&& bdoIds.Contains(s.BdoId)
                                               && (s.SalesOrganizationId == IdDto.SalesOrganizationId || IdDto.SalesOrganizationId == 0)
                                                && (s.DistributionChannelId == IdDto.DistributionChannelId || IdDto.DistributionChannelId == 0)
                                                && (s.DivisionId == IdDto.DivisionId || IdDto.DivisionId == 0)
                                               select so
                                               ).ToList();

                var SaudaOutstandingMT = SaudaOutstandingContext.Sum(_ => _.BidQuantityCase);
                var SaudaOutstanding = SaudaOutstandingContext.Sum(_ => _.BidQuantity);

                outputDto.DealerId = dealerContext.Id;
                outputDto.DealerName = dealerContext.Name;
                outputDto.DealerCode = dealerContext.Code;
                //outputDto.CurrentLimit = userdivContext != null && userdivContext.Any() ? userdivContext.Sum(_ => _.SaudaLimit ?? 0) : 0;
                outputDto.SaudaOutStatnding = SaudaOutstanding;
                outputDto.SaudaOutStandingMT = SaudaOutstandingMT;
                outputDto.Sales = (from s in _emamiContext.SalesRegister.AsNoTracking()
                                   join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                                   join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                                   join ud in divisionslogieduser on new { s.SalesOrganizationId, s.DistributionChannelId, s.DivisionId }
                                               equals new { ud.SalesOrganizationId, ud.DistributionChannelId, ud.DivisionId }
                                   where u.Id == IdDto.Id
                                   && s.SalesOrganizationId == sku.SalesOrganizationId
                                   && s.DistributionChannelId == sku.DistributionChannelId
                                   && s.DivisionId == sku.DivisionId
                                   select s.TotalAmount
                                 ).ToList().Select(s => Convert.ToDecimal(s)).DefaultIfEmpty(0).Sum();


                //outputDto.Sales = _emamiContext.SalesRegister.AsNoTracking()
                //    .Where(_ => _.UserId == IdDto.Id)
                //    .Select(_ => Convert.ToDecimal(_.TotalAmount)).DefaultIfEmpty(0).Sum();

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



        /// Method to create sauda
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        //public ResultDto SaudaCreation(SaudaInputDto inputDto)
        //{
        //    _methodName = "SaudaCreation";
        //    var resultDto = new ResultDto();
        //    try
        //    {
        //        var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }

        //        var dealerContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == inputDto.DealerId);

        //        if (dealerContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserNotFound);
        //        }

        //        decimal TotalQtyInMT = 0;
        //        foreach (var item in inputDto.SaudaOrders)
        //        {
        //            TotalQtyInMT = TotalQtyInMT + _resultService.ConvertCasetoMetricTon(item.BidQuantity, item.SkuId);
        //        }
        //        //var statuses = Constants.OutstandingSaudaStatus;
        //        var statuses = Constants.OverallSaudaStatus;
        //        var SaudaOutstandingContext = (from s in _emamiContext.Sauda.AsNoTracking()
        //                                       join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
        //                                       where s.UserId == inputDto.DealerId
        //                                       && statuses.Contains(so.StatusId)
        //                                       select so
        //                                       ).ToList();
        //        if (SaudaOutstandingContext != null && SaudaOutstandingContext.Any())
        //        {
        //            decimal invoiceQuantity = 0;
        //            var existingSaudaQuantity = SaudaOutstandingContext.Sum(_ => _.BidQuantity);
        //            var skuIds = SaudaOutstandingContext.Select(_ => _.SkuId).Distinct().ToList();
        //            var invoiceContext = (from inv in _emamiContext.Invoices.AsNoTracking()
        //                                  join invDet in _emamiContext.InvoiceDetails.AsNoTracking() on inv.Id equals invDet.InvoiceId
        //                                  where inv.UserId == inputDto.DealerId
        //                                  && skuIds.Contains(invDet.SkuId)
        //                                  select invDet
        //                                      ).ToList();

        //            if (invoiceContext != null && invoiceContext.Any())
        //            {
        //                invoiceQuantity = invoiceContext.Sum(_ => _.ActualBilledQuantity);
        //            }

        //            var SaudaOutstanding = existingSaudaQuantity + TotalQtyInMT;
        //            var SaudaLimit = dealerContext.SaudaLimit + invoiceQuantity;
        //            if (SaudaLimit < SaudaOutstanding)
        //            {
        //                return _resultService.ErrorMessage(Constants.SaudaLimitIsExceeds);
        //            }
        //        }


        //        if (inputDto.SaudaOrders != null && inputDto.SaudaOrders.Any())
        //        {
        //            var overallSaudaStatuses = Constants.OverallSaudaStatus;
        //            foreach (var item in inputDto.SaudaOrders)
        //            {

        //                var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == item.SkuId);
        //                if (skuContext != null && skuContext.DivisionId == (int)DTO.Enums.Vertical.SpecialityFat)
        //                {
        //                    //bool geoErrorFlag = false;
        //                    //bool bdoErrorFlag = false;
        //                    //decimal availableQuantityGeo = 0;
        //                    decimal availableQuantityBdo = 0;
        //                    //var geographicalLimitContext = _emamiContext.SpecalityFatDiscountGeographys.AsNoTracking().FirstOrDefault(_ => _.SkuId == item.SkuId && _.CityId == dealerContext.CityId
        //                    //&& DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(currentDate) && DbFunctions.TruncateTime(_.ValidTo) >= DbFunctions.TruncateTime(currentDate));
        //                    //if (geographicalLimitContext != null)
        //                    //{
        //                    //    IQueryable<SaudaOrder> saudaOrdersGeoContext = null;
        //                    //    List<long> dealerList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
        //                    //        .Where(_ => _.u.CityId == dealerContext.CityId && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.u.Id).ToList();
        //                    //    if (dealerList != null && dealerList.Any())
        //                    //    {
        //                    //        saudaOrdersGeoContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.SkuId == item.SkuId && dealerList.Contains(_.Sauda.UserId)
        //                    //              && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(geographicalLimitContext.ValidFrom) && 
        //                    //              DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(geographicalLimitContext.ValidTo) && overallSaudaStatuses.Contains(_.StatusId));
        //                    //    }
        //                    //    decimal requestedQuantityGeo = _resultService.ConvertCasetoMetricTon(item.BidQuantity, item.SkuId);
        //                    //    decimal orderedQuantityGeo = 0;
        //                    //    decimal totalQuantityGeo = requestedQuantityGeo;
        //                    //    if (saudaOrdersGeoContext != null && saudaOrdersGeoContext.Any())
        //                    //    {
        //                    //        orderedQuantityGeo = saudaOrdersGeoContext.Sum(_ => _.BidQuantity);
        //                    //        totalQuantityGeo = requestedQuantityGeo + orderedQuantityGeo;
        //                    //    }
        //                    //    if (totalQuantityGeo > geographicalLimitContext.ActualDiscount)
        //                    //    {
        //                    //        geoErrorFlag = true;
        //                    //        //return _resultService.ErrorMessage(Constants.SkuGeographicalLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName));
        //                    //        availableQuantityGeo = geographicalLimitContext.ActualDiscount - orderedQuantityGeo;
        //                    //        if (availableQuantityGeo < 0)
        //                    //        {
        //                    //            availableQuantityGeo = 0;
        //                    //        }
        //                    //        //else
        //                    //        //{
        //                    //        //    return _resultService.ErrorMessage(Constants.SkuGeographicalLimitReached.Replace(Constants.SkuName, skuContext.SkuName));
        //                    //        //}
        //                    //    }
        //                    //}
        //                    var bdoLimitContext = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
        //                                  && DbFunctions.TruncateTime(_.ValidTo) >= DbFunctions.TruncateTime(currentDate));
        //                    if (bdoLimitContext != null)
        //                    {
        //                        IQueryable<SaudaOrder> saudaOrdersBdoContext = null;
        //                        List<long> dealerList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
        //                            .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.u.Id, uc => uc.CustomerId, (x, uc) => new { DealerId = x.u.Id, x.ur, uc })
        //                            .Where(_ => _.uc.UserId == inputDto.LoginUserId && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.DealerId).ToList();
        //                        if (dealerList != null && dealerList.Any())
        //                        {
        //                            saudaOrdersBdoContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.SkuId == item.SkuId && dealerList.Contains(_.Sauda.UserId)
        //                                  && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(bdoLimitContext.ValidFrom)
        //                                  && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(bdoLimitContext.ValidTo) && overallSaudaStatuses.Contains(_.StatusId));
        //                        }
        //                        decimal requestedQuantityBdo = _resultService.ConvertCasetoMetricTon(item.BidQuantity, item.SkuId);
        //                        decimal orderedQuantityBdo = 0;
        //                        decimal totalQuantityBdo = requestedQuantityBdo;
        //                        if (saudaOrdersBdoContext != null && saudaOrdersBdoContext.Any())
        //                        {
        //                            orderedQuantityBdo = saudaOrdersBdoContext.Sum(_ => _.BidQuantity);
        //                            totalQuantityBdo = orderedQuantityBdo + requestedQuantityBdo;
        //                        }
        //                        if (totalQuantityBdo > bdoLimitContext.ActualDiscount)
        //                        {
        //                            //bdoErrorFlag = true;
        //                            //return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName));
        //                            availableQuantityBdo = bdoLimitContext.ActualDiscount - orderedQuantityBdo;
        //                            if (availableQuantityBdo < 0)
        //                            {
        //                                availableQuantityBdo = 0;
        //                                //return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName));
        //                            }
        //                            return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, Math.Round(availableQuantityBdo, 2).ToString()));
        //                            //if (availableQuantityBdo >= 0)
        //                            //{
        //                            //    return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, availableQuantityBdo.ToString()));
        //                            //}
        //                            //else
        //                            //{
        //                            //    return _resultService.ErrorMessage(Constants.SkuBdoLimitReached.Replace(Constants.SkuName, skuContext.SkuName));
        //                            //}
        //                        }

        //                    }
        //                    else
        //                    {
        //                        return _resultService.ErrorMessage(Constants.BDOLimitNotExists);
        //                    }
        //                    //if(geographicalLimitContext != null && bdoLimitContext != null && geoErrorFlag && bdoErrorFlag)
        //                    //{
        //                    //    return _resultService.ErrorMessage(Constants.SkuGeographicalBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.GeoLimitQuantity,Math.Round(availableQuantityGeo,2).ToString())
        //                    //        .Replace(Constants.BdoLimitQuantity, Math.Round(availableQuantityBdo, 2).ToString()));
        //                    //}
        //                    //else if(((geographicalLimitContext != null) != (bdoLimitContext != null)) && (geoErrorFlag || bdoErrorFlag))
        //                    //{
        //                    //    if (geographicalLimitContext != null)
        //                    //    {
        //                    //        return _resultService.ErrorMessage(Constants.SkuGeographicalLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, Math.Round(availableQuantityGeo,2).ToString()));
        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, Math.Round(availableQuantityBdo,2).ToString()));
        //                    //    }   
        //                    //}
        //                }


        //                //var pricingContext = _emamiContext.Pricing.AsNoTracking().FirstOrDefault(_ => _.Id == item.PricingId && DbFunctions.TruncateTime(_.BiddingDate) == DbFunctions.TruncateTime(currentDate) && _.IsActive);
        //                //if (pricingContext == null)
        //                //{
        //                //    return _resultService.ErrorMessage(Constants.PricingIdisnotValid);
        //                //}

        //                if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
        //                {
        //                    //var TodayBiddingWindowIds = _emamiContext.BiddingWindowTiming.AsNoTracking().Where(_ => _.BiddingDate == DbFunctions.TruncateTime(currentDate) && _.Id == pricingContext.BiddingWindowId && _.Isactive).ToList();
        //                    //if (TodayBiddingWindowIds == null)
        //                    //{
        //                    //    return _resultService.ErrorMessage(Constants.BiddingWindowisnotValid);
        //                    //}

        //                    int CounterBidAllowCount = 0;
        //                    var CounterBidAllowContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Id == (int)DTO.Enums.Configuration.CounterBidCount);
        //                    if (CounterBidAllowContext != null)
        //                    {
        //                        CounterBidAllowCount = Convert.ToInt32(CounterBidAllowContext.Value);
        //                    }
        //                    //var isSKuExistsContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.BiddingwindowId == pricingContext.BiddingWindowId && _.SkuId == item.SkuId
        //                    //    && _.OilTypeId == item.OilTypeId && _.Incoterms2 == item.IncotermsId && _.PlantId == item.PlantId).ToList();
        //                    //if (isSKuExistsContext != null && isSKuExistsContext.Count >= CounterBidAllowCount)
        //                    //{
        //                    //    return _resultService.ErrorMessage(Constants.SkuAlreadyBookedinBidding);
        //                    //}

        //                    var TodayBiddingIds = _emamiContext.BiddingWindowTiming.AsNoTracking().Where(_ => _.BiddingDate == DbFunctions.TruncateTime(currentDate) && _.Isactive);
        //                    if (TodayBiddingIds != null)
        //                    {
        //                        var SaudaContext = (from sauda in _emamiContext.Sauda
        //                                            join saudaorder in _emamiContext.SaudaOrders on sauda.Id equals saudaorder.SaudaId
        //                                            join biddings in TodayBiddingIds on saudaorder.BiddingwindowId equals biddings.Id
        //                                            where sauda.UserId == inputDto.DealerId && saudaorder.StatusId == (int)DTO.Enums.Status.Hold
        //                                            && saudaorder.SkuId == item.SkuId
        //                                            && saudaorder.OilTypeId == item.OilTypeId && saudaorder.Incoterms2 == item.IncotermsId && saudaorder.PlantId == item.PlantId
        //                                            select saudaorder
        //                                        ).ToList();

        //                        if (SaudaContext.Count > 1)
        //                        {
        //                            return _resultService.ErrorMessage(Constants.SaudaHoldMessage);
        //                        }
        //                    }
        //                }

        //            }
        //        }

        //        var statusId = (int)DTO.Enums.Status.Pending;
        //        //if (inputDto.DealerTypeId == (int)DTO.Enums.DealerType.Broker)
        //        //{
        //        //    statusId = (int)DTO.Enums.Status.WaitingForConfirmation;
        //        //}
        //        if (inputDto.SaudaOrders != null && inputDto.SaudaOrders.Any())
        //        {
        //            foreach (var item in inputDto.SaudaOrders)
        //            {
        //                var status = 0;
        //                var pricingContext = _emamiContext.Pricing.AsNoTracking().FirstOrDefault(_ => _.Id == item.PricingId);
        //                if (pricingContext != null)
        //                {
        //                    var cleranceRate = (decimal)0;
        //                    var baseRate = (decimal)0;
        //                    if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ExDepot)
        //                    {
        //                        cleranceRate = pricingContext.ExDepotPrice * pricingContext.CounterBidLimit;
        //                        baseRate = pricingContext.ExDepotPrice;
        //                    }
        //                    else if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ExPlant)
        //                    {
        //                        cleranceRate = pricingContext.ExPlantPrice * pricingContext.CounterBidLimit;
        //                        baseRate = pricingContext.ExPlantPrice;
        //                    }
        //                    else if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ForDepot)
        //                    {
        //                        cleranceRate = pricingContext.ForDepotPrice * pricingContext.CounterBidLimit;
        //                        baseRate = pricingContext.ForDepotPrice;
        //                    }
        //                    else if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ForPlant)
        //                    {
        //                        cleranceRate = pricingContext.ForPlantPrice * pricingContext.CounterBidLimit;
        //                        baseRate = pricingContext.ForPlantPrice;
        //                    }
        //                    else if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ExRake)
        //                    {
        //                        cleranceRate = pricingContext.ExRakePrice * pricingContext.CounterBidLimit;
        //                        baseRate = pricingContext.ExRakePrice;
        //                    }
        //                    else if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ForRake)
        //                    {
        //                        cleranceRate = pricingContext.ForRakePrice * pricingContext.CounterBidLimit;
        //                        baseRate = pricingContext.ForRakePrice;
        //                    }

        //                    //cleranceRate = item.BidQuantity * cleranceRate;
        //                    //baseRate = item.BidQuantity * baseRate;

        //                    if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
        //                    {
        //                        if (item.BidPrice < cleranceRate)
        //                            status = (int)DTO.Enums.Status.Rejected;
        //                        else if (item.BidPrice >= cleranceRate && item.BidPrice <= baseRate)
        //                            status = (int)DTO.Enums.Status.Hold;
        //                        else if (item.BidPrice > baseRate)
        //                            status = (int)DTO.Enums.Status.Pending;

        //                        //if (item.BidPrice < pricingContext.ClearanceRate)
        //                        //    status = (int)DTO.Enums.Status.Rejected;
        //                        //else if (item.BidPrice >= pricingContext.ClearanceRate && item.BidPrice <= pricingContext.BaseRate)
        //                        //    status = (int)DTO.Enums.Status.Hold;
        //                        //else if (item.BidPrice > pricingContext.BaseRate)
        //                        //    status = (int)DTO.Enums.Status.Pending;
        //                    }
        //                    else
        //                    {
        //                        status = (int)DTO.Enums.Status.Pending;
        //                    }
        //                    item.StatusId = status;
        //                }
        //            }

        //            if (inputDto.SaudaOrders.Any(_ => _.StatusId == (int)DTO.Enums.Status.Hold))
        //                statusId = (int)DTO.Enums.Status.Hold;
        //            else if (inputDto.SaudaOrders.Any(_ => _.StatusId == (int)DTO.Enums.Status.Rejected))
        //                statusId = (int)DTO.Enums.Status.Rejected;

        //        }

        //        long DealerTypeId = 0;
        //        string IncotermsType = string.Empty;
        //        long BrokerId = 0;
        //        var dealerRole = _emamiContext.UserRoles.FirstOrDefault(_ => _.UserId == inputDto.DealerId);
        //        if (dealerRole != null)
        //        {
        //            DealerTypeId = dealerRole.RoleId == (int)DTO.Enums.Role.Broker ? (int)DTO.Enums.DealerType.Broker : (int)DTO.Enums.DealerType.Direct;
        //            if (dealerRole.RoleId == (int)DTO.Enums.Role.Broker)
        //            {
        //                BrokerId = inputDto.DealerId;
        //            }
        //            else
        //            {
        //                var BrokerContext = (from ucm in _emamiContext.UserCustomerMapping
        //                                     join ur in _emamiContext.UserRoles on ucm.UserId equals ur.UserId
        //                                     where ur.RoleId == (int)DTO.Enums.Role.Broker
        //                                     && ucm.CustomerId == inputDto.DealerId
        //                                     select new
        //                                     {
        //                                         BrokerId = ucm.UserId
        //                                     }).FirstOrDefault();

        //                if (BrokerContext != null)
        //                {
        //                    BrokerId = BrokerContext.BrokerId;
        //                }
        //            }
        //        }


        //        var saudaContext = new Sauda
        //        {

        //            BiddingDate = currentDate,
        //            UserId = inputDto.DealerId,

        //            SaudaBookingTypeId = inputDto.SaudaBookingTypeId,

        //            CreatedBy = inputDto.LoginUserId,
        //            CreatedDate = currentDate,
        //            IsSAPDataSync = false,
        //            IsSAPDataSyncApproval = false

        //        };

        //        _emamiContext.Sauda.Add(saudaContext);
        //        _emamiContext.SaveChanges();

        //        if (inputDto.SaudaOrders != null && inputDto.SaudaOrders.Any())
        //        {
        //            foreach (var item in inputDto.SaudaOrders)
        //            {
        //                DateTime? saudaValidFromDate = currentDate;
        //                long? depotIdForRake = 0;
        //                if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ExRake || item.IncotermsId == (int)DTO.Enums.IncoTerms.ExRake)
        //                {
        //                    depotIdForRake = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == item.PlantId && !_.IsPlant)?.DepotId;
        //                    if (item.SaudaValidFromDate != null)
        //                        saudaValidFromDate = item.SaudaValidFromDate;
        //                }


        //                var IncotermContext = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == item.IncotermsId).Name;
        //                IncotermsType = IncotermContext.ToLower().Contains("for") ? "For" : "Ex";


        //                item.DiscountAmount = item.BidQuantity * item.DiscountAmount;
        //                decimal itemquotedprice = item.BidQuantity * item.QuotedPrice;
        //                item.QuotedPrice = itemquotedprice;
        //                item.BidPrice = inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction ?
        //                    item.BidQuantity * item.BidPrice : itemquotedprice;


        //                if (item.DiscountTypeId == (int)DTO.Enums.SaudaDiscountType.Discount)
        //                {
        //                    item.BidPrice = item.BidPrice - item.DiscountAmount;
        //                }
        //                else
        //                {
        //                    item.BidPrice = item.BidPrice + item.DiscountAmount;
        //                }

        //                //var pricingContext = _emamiContext.Pricing.AsNoTracking().FirstOrDefault(_ => _.Id == item.PricingId);
        //                //if (pricingContext != null)
        //                //{
        //                var saudaOrder = new SaudaOrder
        //                {
        //                    SaudaId = saudaContext.Id,
        //                    SkuId = item.SkuId,
        //                    OilTypeId = item.OilTypeId,
        //                    BidPrice = item.BidPrice,
        //                    DiscountTypeId = item.DiscountTypeId,
        //                    DiscountAmount = item.DiscountAmount,
        //                    BidQuantity = _resultService.ConvertCasetoMetricTon(item.BidQuantity, item.SkuId),
        //                    BidQuantityCase = item.BidQuantity,
        //                    QuotedPrice = item.QuotedPrice,
        //                    CreatedBy = inputDto.LoginUserId,
        //                    CreatedDate = currentDate,
        //                    BiddingwindowId = item.BiddingwindowId,
        //                    SaudaBookingTypeId = inputDto.SaudaBookingTypeId,
        //                    PricingId = item.PricingId,
        //                    DealerTypeId = DealerTypeId,
        //                    Incoterms1 = IncotermsType,
        //                    PlantId = item.PlantId,
        //                    DealerLocationId = Convert.ToInt64(dealerContext.FreightRouteId),
        //                    CustomerPONumber = dealerContext.Code + currentDate.ToShortDateString(),
        //                    ValidFromDate = saudaValidFromDate.Value,
        //                    ValidToDate = saudaValidFromDate.Value.AddDays(Convert.ToDouble(dealerContext.SaudaValidityPeriod > 0 ? dealerContext.SaudaValidityPeriod : Config.DefaultSaudaValidity)),
        //                    StatusId = statusId,
        //                    SaudaStatusId = (int)DTO.Enums.SaudaStatus.NotReleased,
        //                    Incoterms2 = item.IncotermsId,
        //                    BrokerId = BrokerId,
        //                    IsSAPDataSync = false,
        //                    IsSAPDataSyncApproval = false,
        //                    DepotIdForRake = depotIdForRake.Value
        //                };
        //                _emamiContext.SaudaOrders.Add(saudaOrder);
        //                _emamiContext.SaveChanges();
        //                //}
        //                //if (inputDto.SaudaBookingTypeId != (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
        //                //{
        //                try
        //                {
        //                    var usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == inputDto.LoginUserId || _.Id == inputDto.DealerId);
        //                    var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrder.Id);
        //                    if (usersContext != null && usersContext.Any() && saudaOrderContext != null)
        //                    {
        //                        List<string> toUsers = new List<string>();
        //                        var createdBy = usersContext.FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
        //                        var dealer = usersContext.FirstOrDefault(_ => _.Id == inputDto.DealerId);
        //                        string dealerName = string.Empty;
        //                        if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
        //                        {
        //                            toUsers.Add(createdBy.Email);
        //                        }
        //                        if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
        //                        {
        //                            dealerName = dealer.Name;
        //                            toUsers.Add(dealer.Email);
        //                        }
        //                        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
        //                        string emailSubject = string.Empty;
        //                        if (_resultService.IsEmail() && toUsers != null && toUsers.Any())
        //                        {
        //                            var fromEmail = Constants.FromEmail;
        //                            var plainText = string.Empty;
        //                            EmailTemplate emailTemplate = new EmailTemplate();
        //                            if (saudaOrder.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess)
        //                            {
        //                                emailSubject = Constants.SaudaBookedSubject;
        //                                emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationEmail);
        //                            }
        //                            else
        //                            {
        //                                if (item.StatusId == (int)DTO.Enums.Status.Pending)
        //                                {
        //                                    emailSubject = Constants.SaudaCreationRAFlowSubject;
        //                                    emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationRAFlowEmail);
        //                                }
        //                                else if (item.StatusId == (int)DTO.Enums.Status.Hold)
        //                                {
        //                                    emailSubject = Constants.SaudaOnHoldSubject;
        //                                    emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderHoldNotificationEmail);
        //                                }
        //                                else if (item.StatusId == (int)DTO.Enums.Status.Rejected)
        //                                {
        //                                    emailSubject = Constants.SaudaRejectedSubject;
        //                                    emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderRejectNotificationEmail);
        //                                }
        //                            }
        //                            if (emailTemplate != null)
        //                            {
        //                                var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
        //                                    .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round((saudaOrderContext.BidPrice / saudaOrderContext.BidQuantityCase), 2)).ToString())
        //                                    .Replace(Constants.BY_FOR, Constants.FOR).Replace(Constants.UserName, dealerName);
        //                                var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
        //                                amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
        //                            }

        //                        }
        //                        var smsPlainTemplate = string.Empty;
        //                        if (_resultService.IsSMS())
        //                        {
        //                            var smsMessage = string.Empty;
        //                            EmailTemplate smsTemplate = new EmailTemplate();
        //                            if (saudaOrder.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess)
        //                            {
        //                                smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationSMS);
        //                            }
        //                            else
        //                            {
        //                                if (saudaOrder.StatusId == (int)DTO.Enums.Status.Pending)
        //                                {
        //                                    smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationRAFlowSMS);


        //                                }
        //                                else if (saudaOrder.StatusId == (int)DTO.Enums.Status.Hold)
        //                                {
        //                                    smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderHoldNotificationSMS);
        //                                }
        //                                else if (saudaOrder.StatusId == (int)DTO.Enums.Status.Rejected)
        //                                {
        //                                    smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderRejectNotificationSMS);
        //                                }

        //                                var statusContext = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrder.StatusId);
        //                                var notificationContext = new Notifications
        //                                {
        //                                    Request = DTO.Enums.NotificationRequest.Sauda.ToString(),
        //                                    RequestId = (int)DTO.Enums.NotificationRequest.Sauda,
        //                                    ReferenceId = saudaOrder.Id,
        //                                    Notification = statusContext != null ? statusContext.Name : string.Empty,
        //                                    StatusId = saudaOrder.StatusId,
        //                                    CreatedBy = saudaOrder.CreatedBy,
        //                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        //                                };
        //                                _emamiContext.Notifications.Add(notificationContext);
        //                                _emamiContext.SaveChanges();
        //                            }
        //                            if (smsTemplate != null)
        //                            {
        //                                smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
        //                                    .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round((saudaOrderContext.BidPrice / saudaOrderContext.BidQuantityCase), 2)).ToString())
        //                                    .Replace(Constants.BY_FOR, Constants.FOR).Replace(Constants.UserName, dealerName);
        //                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
        //                                try
        //                                {
        //                                    if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
        //                                    {
        //                                        amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber);
        //                                    }
        //                                    if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
        //                                    {
        //                                        amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber);
        //                                    }
        //                                }
        //                                catch (Exception ex)
        //                                {

        //                                }
        //                            }
        //                        }
        //                        if (_resultService.IsPushNotification() && saudaOrder.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
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

        //                //}

        //            }
        //        }


        //        resultDto.IsSuccess = true;
        //        resultDto.SuccessDto.Response = saudaContext.Id;
        //        return resultDto;
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        resultDto.IsSuccess = false;
        //        resultDto.ErrorDto.ErrorCode = Constants.Exception;
        //        resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
        //        _logger.Error(message);
        //        return resultDto;
        //    }

        //}

        public ResultDto GetDealerLocationsByDealerId(IdInputDto inputDto)
        {
            _methodName = "GetdealerLocationByDealerId";
            var userMasterDto = new List<DealerLocationDto>();
            if (inputDto == null)
            {
                return NotFoundResult();
            }
            try
            {
                var dealerlist = _emamiContext.DealerLocation.AsNoTracking().Where(_ => _.UserId == inputDto.Id).Select(s => new DealerLocationDto()
                {
                    Id = s.Id,
                    Address = s.Address,
                    CityId = s.CityId,
                    DistrictId = s.DistrictId,
                    StateId = s.StateId,
                    UserId = s.UserId,
                    City = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == s.CityId).CityName,
                    District = _emamiContext.District.AsNoTracking().FirstOrDefault(_ => _.Id == s.DistrictId).DistrictName,
                    State = _emamiContext.State.AsNoTracking().FirstOrDefault(_ => _.Id == s.StateId).StateName
                }).ToList();


                return SucessResult(dealerlist);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        /// <summary>
        /// Method to get sauda list
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetSaudaList(SaudaFilterDto saudaFilterDto)
        {
            _methodName = "GetSaudaList";
            var resultDto = new ResultDto();
            var saudaListDto = new List<SaudaListDto>();
            try
            {
                if (saudaFilterDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                if (saudaFilterDto.UserId <= 0)
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
                var saudaList = _emamiContext.Sauda.AsNoTracking().Where(_ => _.UserId == saudaFilterDto.UserId && DbFunctions.TruncateTime(_.BiddingDate) >= DbFunctions.TruncateTime(saudaFilterDto.FromDate) &&
                DbFunctions.TruncateTime(_.BiddingDate) <= DbFunctions.TruncateTime(saudaFilterDto.ToDate)).OrderByDescending(_ => _.CreatedDate).AsQueryable();

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
                    var saudaDto = new SaudaListDto
                    {
                        Id = sauda.Id,
                        //SaudaNumber = sauda.SaudaNumber,
                        SaudaNumber = sauda.Id.ToString(),
                        User = _emamiContext.Users.FirstOrDefault(_ => _.Id == sauda.UserId).Name,
                        BiddingDate = sauda.BiddingDate,
                        TotalBidPrice = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == sauda.Id).Sum(_ => (decimal?)_.BidPrice) ?? 0,
                        TotalBidQuantity = totalBidQuantity
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
        /// Method to get sauda list
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetPendingSaudaListForMobile(SaudaFilterDto saudaFilterDto)
        {
            _methodName = "GetPendingSaudaListForMobile";
            var resultDto = new ResultDto();
            var saudaListDto = new List<SaudaListDto>();
            try
            {
                if (saudaFilterDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
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
                IQueryable<Sauda> saudaList;
                if (saudaFilterDto.DealerId != 0 && saudaFilterDto.OilTypeId != 0)
                {
                    saudaList = _emamiContext.Sauda.AsNoTracking().Join(_emamiContext.SaudaOrders.AsNoTracking(), s => s.Id, so => so.SaudaId, (s, so) => new { s, so })
                        .Where(_ => _.s != null && _.so != null && DbFunctions.TruncateTime(_.s.BiddingDate) >= DbFunctions.TruncateTime(saudaFilterDto.FromDate) &&
                        DbFunctions.TruncateTime(_.s.BiddingDate) <= DbFunctions.TruncateTime(saudaFilterDto.ToDate) && _.s.UserId == saudaFilterDto.DealerId && _.so.OilTypeId == saudaFilterDto.OilTypeId)
                        .Select(_ => _.s).Distinct().OrderByDescending(_ => _.CreatedDate).AsQueryable();
                }
                else
                {
                    saudaList = _emamiContext.Sauda.AsNoTracking().Where(_ => DbFunctions.TruncateTime(_.BiddingDate) >= DbFunctions.TruncateTime(saudaFilterDto.FromDate) &&
                    DbFunctions.TruncateTime(_.BiddingDate) <= DbFunctions.TruncateTime(saudaFilterDto.ToDate)).OrderByDescending(_ => _.CreatedDate).AsQueryable();
                }
                if (!saudaList.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }
                var list = saudaList.ToList();
                foreach (var sauda in saudaList.ToList())
                {
                    var liftingQuantity = 0;

                    var totalBidQuantity = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == sauda.Id).Sum(_ => (int?)_.BidQuantity) ?? 0;
                    //var CityId = _emamiContext.DealerLocation.FirstOrDefault(_ => _.Id == sauda.DealerLocationId).CityId;
                    //var liftingRequestContext = _emamiContext.LiftingRequest.AsNoTracking().FirstOrDefault(_ => _.SaudaId == sauda.Id);
                    //if (liftingRequestContext != null)
                    //{
                    //    liftingQuantity = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.LiftingRequestId == liftingRequestContext.Id &&
                    //    _.Status == (int)DTO.Enums.LiftingRequestStatus.Inprogress).Sum(_ => (int?)_.LiftingQuantity) ?? 0;
                    //}
                    var saudaDto = new SaudaListDto
                    {
                        Id = sauda.Id,
                        UserId = sauda.UserId,
                        User = _emamiContext.Users.FirstOrDefault(_ => _.Id == sauda.UserId).Name,
                        //City = _emamiContext.City.FirstOrDefault(_ => _.Id == CityId).CityName,
                        BiddingDate = sauda.BiddingDate,
                        TotalBidPrice = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == sauda.Id).Sum(_ => (decimal?)_.BidPrice) ?? 0,
                        TotalBidQuantity = totalBidQuantity
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

        public ResultDto GetExpiredSaudaListForMobile(SaudaFilterDto saudaFilterDto)
        {
            _methodName = "GetExpiredSaudaListForMobile";
            var saudaDetailsList = new List<SaudaOrderDetails>();
            try
            {
                if (saudaFilterDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (saudaFilterDto.UserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaFilterDto.UserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (saudaFilterDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }
                if (saudaFilterDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }
                IQueryable<UserCustomerMapping> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == saudaFilterDto.UserId);
                if (dealersList != null && dealersList.Any())
                {
                    List<Sauda> saudaContextList = _emamiContext.Sauda.AsNoTracking().Where(_ => dealersList.Any(a => a.CustomerId == _.UserId) && DbFunctions.TruncateTime(_.BiddingDate) >= DbFunctions.TruncateTime(saudaFilterDto.FromDate)
                    && DbFunctions.TruncateTime(_.BiddingDate) <= DbFunctions.TruncateTime(saudaFilterDto.ToDate)).ToList();
                    IQueryable<SaudaOrder> saudaOrderContextList = null;
                    if (saudaContextList != null && saudaContextList.Any())
                    {
                        List<long> saudaContextListIds = saudaContextList.Select(_ => _.Id).ToList();
                        saudaOrderContextList = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => saudaContextListIds.Contains(_.SaudaId) && (_.StatusId == (int)DTO.Enums.Status.Approved
                      || _.StatusId == (int)DTO.Enums.Status.Pending));
                    }
                    var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    if (saudaOrderContextList != null && saudaOrderContextList.Any())
                    {
                        IQueryable<SaudaOrder> outStandingContextList = null;
                        if (saudaFilterDto.IsExpired)
                        {
                            outStandingContextList = saudaOrderContextList.Where(_ => DbFunctions.TruncateTime(_.ValidToDate) < DbFunctions.TruncateTime(currentDate));
                        }
                        else
                        {
                            outStandingContextList = saudaOrderContextList.Where(_ => DbFunctions.DiffDays(currentDate, _.ValidToDate) < 25 && DbFunctions.DiffDays(currentDate, _.ValidToDate) >= 1);
                        }
                        if (outStandingContextList != null && outStandingContextList.Any())
                        {
                            var outStandContextList = outStandingContextList.Join(_emamiContext.Sauda.AsNoTracking(), so => so.SaudaId, s => s.Id, (so, s) => new { so, s })
                                .Join(_emamiContext.Users.AsNoTracking(), sos => sos.s.UserId, u => u.Id, (sos, u) => new { sos, u })
                                .GroupJoin(_emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected)
                                , sosu => sosu.sos.so.Id, lr => lr.SaudaOrderId, (sosu, lr) => new { sosu.sos.so, sosu.sos.s, sosu.u, lr })
                                .Where(_ => _.s != null && _.so != null && _.u != null);
                            saudaDetailsList = outStandContextList.Select(_ => new SaudaOrderDetails
                            {
                                SaudaId = _.so.Id,
                                SaudaOrderId = _.so.Id,
                                DealerId = _.s.UserId,
                                DealerName = _.u.Name,
                                BookedDate = _.s.BiddingDate,
                                BidQuantity = _.so.BidQuantity - (_.lr.Select(s => s.LiftingQuantity).DefaultIfEmpty(0).Sum()),
                                BidQuantityCases = _.so.BidQuantityCase - (_.lr.Select(s => s.LiftingQuantityCase).DefaultIfEmpty(0).Sum()),
                                BidPrice = _.so.BidPrice,
                                //FrieghtRoute = _.u.FreightRoute != null ? _.u.FreightRoute.Name : string.Empty,
                                OilTypeId = _.so.OilTypeId,
                                OilTypeName = _.so.OilType != null ? _.so.OilType.Name : string.Empty,
                            }).Distinct().ToList();

                            //foreach (var saudaOrderContext in outStandingContextList.ToList())
                            //{
                            //    var saudaDetails = new SaudaOrderDetails();
                            //    if (saudaOrderContext.Sauda != null)
                            //    {
                            //        saudaDetails.SaudaId = saudaOrderContext.Sauda.Id;

                            //        var dealerContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == saudaOrderContext.Sauda.UserId);
                            //        if (dealerContext != null)
                            //        {
                            //            saudaDetails.DealerId = dealerContext.Id;
                            //            saudaDetails.DealerName = dealerContext.Name;
                            //        }
                            //        saudaDetails.BookedDate = saudaOrderContext.Sauda.BiddingDate;

                            //    }
                            //    saudaDetails.BidQuantity = saudaOrderContext.BidQuantity;
                            //    saudaDetails.BidPrice = saudaOrderContext.BidPrice;
                            //    var frieghtRouteContext = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.DealerLocationId);
                            //    if (frieghtRouteContext != null)
                            //    {
                            //        saudaDetails.FrieghtRoute = frieghtRouteContext.Name;
                            //    }

                            //    saudaDetailsList.Add(saudaDetails);
                            //}
                        }
                    }
                }

                if (saudaDetailsList != null && saudaDetailsList.Any())
                {
                    return _resultService.SuccessObject(saudaDetailsList);
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
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }


        /// <summary>
        /// Method to get sauda details
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetSaudaDetails(SaudaDetailInputDto inputDto)
        {
            _methodName = "GetSaudaDetails";
            var resultDto = new ResultDto();
            var saudaDetails = new SaudaDetailOutputDto();
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
                    resultDto.ErrorDto.ErrorCode = Constants.DealerIdEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.DealerIdEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.UserId);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                var saudaContext = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SaudaId);
                if (saudaContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id);

                if (inputDto.SalesOrganizationId > 0 && inputDto.DistributionChannelId > 0 && inputDto.DivisionId > 0)
                {
                    saudaOrderContext = saudaOrderContext.Where(_ => _.SalesOrganizationId == inputDto.SalesOrganizationId
                    && _.DistributionChannelId == inputDto.DistributionChannelId
                    && _.DivisionId == inputDto.DivisionId);
                }

                if (saudaOrderContext != null && saudaOrderContext.Any())
                {
                    var totalBidAmount = saudaOrderContext.Sum(_ => (decimal?)_.BidPrice) ?? 0;
                    var totalBidQuantity = saudaOrderContext.Sum(_ => (decimal?)_.BidQuantityCase) ?? 0;
                    var totalBidQuantityInMT = saudaOrderContext.Sum(_ => (decimal?)_.BidQuantity) ?? 0;

                    saudaDetails.TotalAmount = totalBidAmount;
                    saudaDetails.TotalQuantity = totalBidQuantity;
                    saudaDetails.TotalQuantityInMT = totalBidQuantityInMT;

                    saudaDetails.BrokerId = saudaOrderContext.Where(_ => _.SaudaId == saudaContext.Id).FirstOrDefault() != null
                        ? saudaOrderContext.Where(_ => _.SaudaId == saudaContext.Id).FirstOrDefault().BrokerId : 0;
                    if (saudaDetails.BrokerId > 0)
                    {
                        saudaDetails.BrokerName = saudaOrderContext.Where(_ => _.SaudaId == saudaContext.Id).FirstOrDefault() != null
                             ?
                            _emamiContext.Users.FirstOrDefault(_ => _.Id == saudaOrderContext.Where(s => s.SaudaId == saudaContext.Id).FirstOrDefault().BrokerId).Name : string.Empty;

                    }
                    //var BrokerContext = saudaOrderContext.Where(_ => _.SaudaId == saudaContext.Id).FirstOrDefault();
                    //if (BrokerContext != null)
                    //{
                    //    saudaDetails.BrokerId = BrokerContext.BrokerId;
                    //    saudaDetails.BrokerName = BrokerContext.BrokerId != 0 ? _emamiContext.Users.FirstOrDefault(_ => _.Id == BrokerContext.BrokerId).Name : string.Empty;
                    //}

                }

                saudaDetails.SaudaId = saudaContext.Id;
                saudaDetails.SaudaNumber = saudaContext.SaudaNumber != null ? saudaContext.SaudaNumber.ToString() : "";
                saudaDetails.BiddingDate = saudaContext.BiddingDate;
                saudaDetails.DealerId = saudaContext.UserId;
                saudaDetails.StatusId = saudaOrderContext.Where(_ => _.SaudaId == saudaContext.Id).FirstOrDefault().StatusId;
                saudaDetails.Status = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.Where(s => s.SaudaId == saudaContext.Id).FirstOrDefault().StatusId).Name;
                saudaDetails.DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaContext.UserId).Name;
                //saudaDetails.Incoterm = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == saudaContext.Incoterms2).Name;
                saudaDetails.SaudaValidityDays = Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity;
                saudaDetails.SaudaExpireDays = (DateHelper.UtcToIndia(DateTime.UtcNow) - saudaContext.BiddingDate).Days;
                saudaDetails.ExpiryDate = saudaContext.BiddingDate.AddDays(Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity);
                saudaDetails.Remarks = _emamiContext.Remarks.AsNoTracking().OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.TableId == saudaContext.Id && _.IsActive) != null ? _emamiContext.Remarks.AsNoTracking().OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.TableId == saudaContext.Id && _.IsActive).Description : string.Empty;

                var saudaAudioFileMappingContext = _emamiContext.SaudaAudioFileMapping.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id);
                var key = UtilityHelper.GetEnumDescription((DTO.Enums.Configuration.CallRecordMappingReattachBufferTime));
                var reattachBufferTime = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Key == key)?.Value ?? "0";
                var BufferTimeToAdd = Convert.ToDouble(reattachBufferTime);
                if (!saudaAudioFileMappingContext.IsAny())
                {
                    saudaDetails.CanSubmitAudioMapping = true;
                }
                else if (saudaAudioFileMappingContext.IsAny())
                {
                    var ImageCreatedDate = saudaAudioFileMappingContext.FirstOrDefault(_ => _.SaudaId == saudaContext.Id && (_.MediaTypeId == (int)DTO.Enums.MediaType.Audio || _.MediaTypeId == (int)DTO.Enums.MediaType.Image)).CreatedDate;
                    var timeUntilReattachmentDone = ImageCreatedDate.AddMinutes(BufferTimeToAdd);
                    if (DateHelper.UtcToIndia(DateTime.UtcNow) <= timeUntilReattachmentDone)
                    {
                        saudaDetails.CanSubmitAudioMapping = true;
                    }
                }

                saudaDetails.AudiofileDetailIds = saudaAudioFileMappingContext.Where(_ => _.MediaTypeId == (int)DTO.Enums.MediaType.Audio).Select(s => s.AudioFileDetailsForActiveCustomersId ?? 0).ToList();

                var imageNames = saudaAudioFileMappingContext.FirstOrDefault(_ => _.SaudaId == saudaContext.Id && _.MediaTypeId == (int)DTO.Enums.MediaType.Image) != null ? saudaAudioFileMappingContext.FirstOrDefault(_ => _.SaudaId == saudaContext.Id && _.MediaTypeId == (int)DTO.Enums.MediaType.Image).ImagePath : string.Empty;
                if (imageNames != string.Empty)
                {
                    saudaDetails.ImagePaths = imageNames.Split(',').ToList();
                    string folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.ImagesSaudaMappingwithCallRecording);
                    string mediapath = Config.MobileImagePath + Path.Combine(ConfigurationManager.AppSettings["UploadMediaPaths"], folderName);

                    if (saudaDetails.ImagePaths.IsAny())
                    {
                        saudaDetails.ImagePaths = saudaDetails.ImagePaths.Select(filename => Path.Combine(mediapath, filename)).ToList();
                    }
                }


                var saudaOrders = new List<SaudaOrderDetails>();
                //var saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id);

                //if (inputDto.SalesOrganizationId > 0 && inputDto.DistributionChannelId > 0 && inputDto.DivisionId > 0)
                //{
                //    saudaOrderContext = saudaOrderContext.Where(_ => _.SalesOrganizationId == inputDto.SalesOrganizationId
                //    && _.DistributionChannelId == inputDto.DistributionChannelId
                //    && _.DivisionId == inputDto.DivisionId);
                //}

                var skuIds = saudaOrderContext.Select(_ => _.SkuId).ToList();
                var plantIds = saudaOrderContext.Select(_ => _.PlantId).ToList();
                var skuContext = _emamiContext.Skus.AsNoTracking().Where(_ => skuIds.Contains(_.Id));
                var plantContext = _emamiContext.Depots.AsNoTracking().Where(_ => plantIds.Contains(_.Id));

                var saudaorderList = saudaOrderContext.ToList();
                foreach (var order in saudaorderList)
                {
                    var saudaOrderItem = new SaudaOrderDetails
                    {
                        SaudaId = order.SaudaId,
                        SaudaOrderId = order.Id,
                        SkuId = order.SkuId,
                        SkuName = skuContext.FirstOrDefault(_ => _.Id == order.SkuId).SkuName,
                        BidPrice = order.BidPrice,
                        BidQuantity = order.BidQuantity,
                        BidQuantityCases = order.BidQuantityCase,
                        IncoTerms = order.Incoterms1,
                        Discount = order.DiscountAmount,
                        PlantDepot = plantContext.FirstOrDefault(_ => _.Id == order.PlantId)?.Name,
                        //FrieghtRoute = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == order.DealerLocationId).Name,
                        DiscountTypeId = order.DiscountTypeId,
                        StatusId = order.StatusId,
                        //Status = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == order.StatusId).Name,
                        Status = order.StatusId == (int)DTO.Enums.Status.Pending ? Constants.Accepted : _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == order.StatusId).Name,
                        SaudaNumber = order.SaudaNumber != null ? order.SaudaNumber : string.Empty,
                        BidPricePerCase = order.BidPrice / order.BidQuantityCase
                    };
                    saudaOrders.Add(saudaOrderItem);
                }
                saudaDetails.SaudaOrders = saudaOrders;

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

        public ResultDto GetSaudaDetailsOld(SaudaDetailInputDto inputDto)
        {
            _methodName = "GetSaudaDetails";
            var resultDto = new ResultDto();
            var saudaDetails = new SaudaDetailOutputDto();
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
                    resultDto.ErrorDto.ErrorCode = Constants.DealerIdEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.DealerIdEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.UserId);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                var saudaContext = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SaudaId);
                if (saudaContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                var totalBidAmount = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id).Sum(_ => (decimal?)_.BidPrice) ?? 0;
                var totalBidQuantity = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id).Sum(_ => (decimal?)_.BidQuantity) ?? 0;
                var BrokerContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.SaudaId == saudaContext.Id);

                saudaDetails.SaudaNumber = saudaContext.Id.ToString();
                saudaDetails.SaudaDate = saudaContext.BiddingDate;
                saudaDetails.DealerId = saudaContext.UserId;
                saudaDetails.DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaContext.UserId).Name;
                //saudaDetails.Incoterm = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == saudaContext.Incoterms2).Name;
                saudaDetails.TotalAmount = totalBidAmount;
                saudaDetails.TotalQuantity = totalBidQuantity;
                saudaDetails.SaudaValidityDays = Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity;
                saudaDetails.SaudaExpireDays = (DateHelper.UtcToIndia(DateTime.UtcNow) - saudaContext.BiddingDate).Days;
                saudaDetails.ExpiryDate = saudaContext.BiddingDate.AddDays(Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity);
                saudaDetails.BrokerId = BrokerContext.BrokerId;
                if (BrokerContext != null)
                {
                    saudaDetails.BrokerName = BrokerContext.BrokerId != 0 ? _emamiContext.Users.FirstOrDefault(_ => _.Id == BrokerContext.BrokerId).Name : string.Empty;
                }

                var saudaOrders = new List<SaudaOrderDetails>();

                var saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id).ToList();
                foreach (var order in saudaOrderListContext)
                {
                    var saudaOrderItem = new SaudaOrderDetails
                    {
                        SkuId = order.SkuId,
                        SkuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == order.SkuId).SkuName,
                        BidPrice = order.BidPrice,
                        BidQuantity = order.BidQuantity,
                        BidQuantityCases = order.BidQuantityCase,
                        IncoTerms = order.Incoterms1,
                        Discount = order.DiscountAmount,
                        PlantDepot = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == order.PlantId).Name,
                        //FrieghtRoute = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == order.DealerLocationId).Name,
                        DiscountTypeId = order.DiscountTypeId,
                        StatusId = order.StatusId,
                        //Status = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == order.StatusId).Name,
                        Status = order.StatusId == (int)DTO.Enums.Status.Pending ? Constants.Accepted : _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == order.StatusId).Name,
                        SaudaNumber = order.SaudaNumber != null ? order.SaudaNumber : string.Empty
                    };
                    saudaOrders.Add(saudaOrderItem);
                }
                saudaDetails.SaudaOrders = saudaOrders;



                //var LiftingRequestList = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => _.SaudaId == inputDto.SaudaId).AsQueryable();
                //foreach (var liftingRequest in LiftingRequestList.ToList())
                //{
                //    var liftingRequestDetailList = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.LiftingRequestId == liftingRequest.Id).AsQueryable();


                //    var LiftingList = liftingRequestDetailList.GroupBy(x => new { x.OilTypeId, x.SkuId, x.SaudaOrderId }).Select(x => new
                //    {
                //        x.Key.OilTypeId,
                //        OilType = _emamiContext.OilTypes.FirstOrDefault(_ => _.Id == x.Key.OilTypeId).Name,
                //        SKU = _emamiContext.Skus.FirstOrDefault(_ => _.Id == x.Key.SkuId).SkuName,
                //        SkuCount = x.Count(),
                //        LiftedQty = x.Sum(_ => _.LiftingQuantity),
                //        TotalQty = _emamiContext.SaudaOrders.Where(_ => _.Id == x.Key.SaudaOrderId && _.OilTypeId == x.Key.OilTypeId && _.SkuId == x.Key.SkuId).Sum(_ => _.BidQuantity)
                //    }).OrderBy(_ => _.OilType).ToList();

                //    foreach (var list in LiftingList)
                //    {
                //        var orderStatisticsDto = new LiftingDetailGroupingDto
                //        {
                //            OilType = list.OilType,
                //            SKUName = list.SKU,
                //            TotalQty = list.TotalQty,
                //            LiftedQty = list.LiftedQty,
                //            PendingQty = list.TotalQty - list.LiftedQty
                //        };
                //        saudaDetails.LiftingDetailGrouping.Add(orderStatisticsDto);
                //    }
                //}

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

        public ResultDto GetSaudaDetailsTPNew(SaudaDetailInputDto inputDto)
        {
            _methodName = "GetSaudaDetailsTPNew";
            var resultDto = new ResultDto();
            var saudaDetails = new List<SaudaDetailOutputDto>();
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
                    resultDto.ErrorDto.ErrorCode = Constants.DealerIdEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.DealerIdEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.UserId);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                var saudaContext = _emamiContext.Sauda.AsNoTracking().Where(_ => _.UserId == inputDto.UserId && DbFunctions.TruncateTime(_.BiddingDate) == DbFunctions.TruncateTime(inputDto.BiddingDate)).ToList();
                if (saudaContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }
                foreach (var sauda in saudaContext)
                {
                    var saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == sauda.Id && inputDto.SkuIds.Contains(_.SkuId)).ToList();
                    foreach (var order in saudaOrderListContext)
                    {
                        var saudaOrders = new List<SaudaOrderDetails>();
                        var saudaDetail = new SaudaDetailOutputDto();
                        var totalBidAmount = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == sauda.Id).Sum(_ => (decimal?)_.BidPrice) ?? 0;
                        var totalBidQuantity = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == sauda.Id).Sum(_ => (decimal?)_.BidQuantity) ?? 0;
                        var BrokerContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.SaudaId == sauda.Id);

                        saudaDetail.SaudaNumber = sauda.Id.ToString();
                        saudaDetail.SaudaDate = sauda.BiddingDate;
                        saudaDetail.DealerId = sauda.UserId;
                        saudaDetail.DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == sauda.UserId).Name;
                        //saudaDetails.Incoterm = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == saudaContext.Incoterms2).Name;
                        saudaDetail.TotalAmount = totalBidAmount;
                        saudaDetail.TotalQuantity = totalBidQuantity;
                        saudaDetail.SaudaValidityDays = Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity;
                        saudaDetail.SaudaExpireDays = (DateHelper.UtcToIndia(DateTime.UtcNow) - sauda.BiddingDate).Days;
                        saudaDetail.ExpiryDate = sauda.BiddingDate.AddDays(Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity);
                        saudaDetail.BrokerId = BrokerContext.BrokerId;
                        saudaDetail.SaudaOrderId = order.Id;
                        if (BrokerContext != null)
                        {
                            saudaDetail.BrokerName = BrokerContext.BrokerId != 0 ? _emamiContext.Users.FirstOrDefault(_ => _.Id == BrokerContext.BrokerId).Name : string.Empty;
                        }
                        var saudaOrderItem = new SaudaOrderDetails
                        {
                            SkuId = order.SkuId,
                            SkuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == order.SkuId).SkuName,
                            BidPrice = order.BidPrice,
                            BidQuantity = order.BidQuantity,
                            BidQuantityCases = order.BidQuantityCase,
                            IncoTerms = order.Incoterms1,
                            Discount = order.DiscountAmount,
                            PlantDepot = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == order.PlantId).Name,
                            //FrieghtRoute = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == order.DealerLocationId).Name,
                            DiscountTypeId = order.DiscountTypeId,
                            StatusId = order.StatusId,
                            //Status = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == order.StatusId).Name,
                            Status = order.StatusId == (int)DTO.Enums.Status.Pending ? Constants.Accepted : _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == order.StatusId).Name,
                            SaudaNumber = order.SaudaNumber != null ? order.SaudaNumber : string.Empty
                        };
                        saudaOrders.Add(saudaOrderItem);
                        saudaDetail.SaudaOrders = saudaOrders;
                        saudaDetails.Add(saudaDetail);
                    }



                    //var LiftingRequestList = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => _.SaudaId == inputDto.SaudaId).AsQueryable();
                    //foreach (var liftingRequest in LiftingRequestList.ToList())
                    //{
                    //    var liftingRequestDetailList = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.LiftingRequestId == liftingRequest.Id).AsQueryable();


                    //    var LiftingList = liftingRequestDetailList.GroupBy(x => new { x.OilTypeId, x.SkuId, x.SaudaOrderId }).Select(x => new
                    //    {
                    //        x.Key.OilTypeId,
                    //        OilType = _emamiContext.OilTypes.FirstOrDefault(_ => _.Id == x.Key.OilTypeId).Name,
                    //        SKU = _emamiContext.Skus.FirstOrDefault(_ => _.Id == x.Key.SkuId).SkuName,
                    //        SkuCount = x.Count(),
                    //        LiftedQty = x.Sum(_ => _.LiftingQuantity),
                    //        TotalQty = _emamiContext.SaudaOrders.Where(_ => _.Id == x.Key.SaudaOrderId && _.OilTypeId == x.Key.OilTypeId && _.SkuId == x.Key.SkuId).Sum(_ => _.BidQuantity)
                    //    }).OrderBy(_ => _.OilType).ToList();

                    //    foreach (var list in LiftingList)
                    //    {
                    //        var orderStatisticsDto = new LiftingDetailGroupingDto
                    //        {
                    //            OilType = list.OilType,
                    //            SKUName = list.SKU,
                    //            TotalQty = list.TotalQty,
                    //            LiftedQty = list.LiftedQty,
                    //            PendingQty = list.TotalQty - list.LiftedQty
                    //        };
                    //        saudaDetails.LiftingDetailGrouping.Add(orderStatisticsDto);
                    //    }
                    //}

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

        public ResultDto GetSaudaShortViewList(SaudaFilterDto saudaFilterDto)
        {
            _methodName = "GetSaudaShortViewList";
            var saudaOrderListDto = new List<SaudaOrderListDto>();
            try
            {
                if (saudaFilterDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (saudaFilterDto.UserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaFilterDto.UserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (saudaFilterDto.DealerId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DealerMissing);
                }
                var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaFilterDto.DealerId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.DealerNotFound);
                }

                if (saudaFilterDto.IsConversion)
                {
                    var saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().Join(_emamiContext.Sauda.AsNoTracking(), so => so.SaudaId, s => s.Id, (so, s) => new { so, s })
                        .Join(_emamiContext.Users.AsNoTracking(), sos => sos.s.UserId, u => u.Id, (sos, u) => new { sos.so, sos.s, u })
                        .GroupJoin(_emamiContext.SaudaConversion.AsNoTracking(), sosu => sosu.so.Id, sc => sc.SaudaOrderId, (sosu, sc) => new { sosu.so, sosu.s, sosu.u, sc })
                        .GroupJoin(_emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected)
                        , sosusc => sosusc.so.Id, lr => lr.SaudaOrderId, (sosusc, lr) => new { sosusc.so, sosusc.s, sosusc.u, sosusc.sc, lr })
                        .Where(_ => _.s.UserId == saudaFilterDto.DealerId && _.so.StatusId == (int)DTO.Enums.Status.Approved && _.so.SaudaNumber != ""
                        && _.so.SaudaNumber != null && _.sc.FirstOrDefault().IsConversion != true && _.s != null && _.so != null && _.u != null);

                    if (saudaOrderListContext != null && saudaOrderListContext.Any())
                    {
                        saudaOrderListDto = saudaOrderListContext.ToList().Select(_ => new SaudaOrderListDto
                        {
                            SaudaId = _.so != null ? _.so.Id : 0,
                            SaudaOrderId = _.so != null ? _.so.Id : 0,
                            SaudaNumber = _.so != null ? _.so.SaudaNumber : string.Empty,
                            SkuName = _.so != null && _.so.Sku != null ? _.so.Sku.SkuName : string.Empty,
                            OilTypeName = _.so != null && _.so.OilType != null ? _.so.OilType.Name : string.Empty,
                            BidQuantity = (_.so != null ? _.so.BidQuantity : 0) - (_.lr.Select(s => s.LiftingQuantity).DefaultIfEmpty(0).Sum()),
                            BidQuantityCase = (_.so != null ? _.so.BidQuantityCase : 0) - (_.lr.Select(s => s.LiftingQuantityCase).DefaultIfEmpty(0).Sum()),
                            BookedDate = _.s.BiddingDate,
                            DealerId = _.u != null ? _.u.Id : 0,
                            DealerName = _.u != null ? _.u.Name : string.Empty,
                        }).Distinct().ToList();
                    }
                }
                else
                {
                    var saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().Join(_emamiContext.Sauda.AsNoTracking(), so => so.SaudaId, s => s.Id, (so, s) => new { so, s })
                        .Join(_emamiContext.Users.AsNoTracking(), sos => sos.s.UserId, u => u.Id, (sos, u) => new { sos.so, sos.s, u })
                        .GroupJoin(_emamiContext.SaudaConversion.AsNoTracking(), sosu => sosu.so.Id, sc => sc.SaudaOrderId, (sosu, sc) => new { sosu.so, sosu.s, sosu.u, sc })
                        .GroupJoin(_emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected)
                        , sosusc => sosusc.so.Id, lr => lr.SaudaOrderId, (sosusc, lr) => new { sosusc.so, sosusc.s, sosusc.u, sosusc.sc, lr })
                        .Where(_ => _.s.UserId == saudaFilterDto.DealerId && _.so.StatusId == (int)DTO.Enums.Status.Approved && _.so.SaudaNumber != ""
                        && _.so.SaudaNumber != null && _.sc.FirstOrDefault().IsExtension != true && _.s != null && _.so != null && _.u != null);

                    if (saudaOrderListContext != null && saudaOrderListContext.Any())
                    {
                        saudaOrderListDto = saudaOrderListContext.Select(_ => new SaudaOrderListDto
                        {
                            SaudaId = _.so != null ? _.so.Id : 0,
                            SaudaOrderId = _.so != null ? _.so.Id : 0,
                            SaudaNumber = _.so != null ? _.so.SaudaNumber : string.Empty,
                            SkuName = _.so != null && _.so.Sku != null ? _.so.Sku.SkuName : string.Empty,
                            OilTypeName = _.so != null && _.so.OilType != null ? _.so.OilType.Name : string.Empty,
                            BidQuantity = (_.so != null ? _.so.BidQuantity : 0) - (_.lr.Select(s => s.LiftingQuantity).DefaultIfEmpty(0).Sum()),
                            BidQuantityCase = (_.so != null ? _.so.BidQuantityCase : 0) - (_.lr.Select(s => s.LiftingQuantityCase).DefaultIfEmpty(0).Sum()),
                            BookedDate = _.s.BiddingDate,
                            DealerId = _.u != null ? _.u.Id : 0,
                            DealerName = _.u != null ? _.u.Name : string.Empty,
                        }).Distinct().ToList();
                    }
                }

                if (saudaOrderListDto != null && saudaOrderListDto.Any())
                {
                    return _resultService.SuccessObject(saudaOrderListDto);
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
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetSkuListForIndentRequest(SkuInputDto skuInputDto)
        {
            _methodName = "GetSkuListForIndentRequest";
            var skuAfterDetectionList = new List<NewIndentSkuListDto>();
            try
            {
                if (skuInputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (skuInputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == skuInputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (skuInputDto.DealerId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DealerMissing);
                }
                var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == skuInputDto.DealerId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.DealerNotFound);
                }
                //var skuListContext = _emamiContext.SaudaOrders.AsNoTracking().Join(_emamiContext.Sauda.AsNoTracking(), so => so.SaudaId, s => s.Id, (so, s) => new { so, s })
                //    .GroupJoin(_emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking(), sos => sos.so.Id, lr => lr.SaudaOrderId, (sos, lr) => new { sos.so, sos.s, lr })
                //    .Where(_ => _.so.StatusId == (int)DTO.Enums.Status.Approved && _.so.SaudaNumber != "" && _.so.SaudaNumber != null && _.so.OilTypeId == skuInputDto.OilTypeId
                //    && _.s.UserId == skuInputDto.DealerId && _.so != null && _.s != null);
                //var skuList = new List<NewIndentSkuListDto>();
                //if (skuListContext != null && skuListContext.Any())
                //{
                //    skuList = skuListContext.Select(_ => new NewIndentSkuListDto()
                //    {
                //        SkuId = _.so.SkuId,
                //        SkuName = _.so.Sku != null ? _.so.Sku.SkuName : string.Empty,
                //        BidQuantityCase = _.so.BidQuantityCase - (_.lr.Any() ? _.lr.Select(s => s.LiftingQuantityCase).DefaultIfEmpty(0).Sum() : 0),
                //    }).Where(_ => _.BidQuantityCase != 0).ToList();
                //}

                //var skuListContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda.UserId == skuInputDto.DealerId
                //&& _.OilTypeId == skuInputDto.OilTypeId &&
                //(_.StatusId == (int)DTO.Enums.Status.Approved || _.StatusId == (int)DTO.Enums.Status.Completed)).ToList();

                var skuListContext = _emamiContext.Skus.AsNoTracking().Where(_ => _.OilTypeId == skuInputDto.OilTypeId && _.IsActive).ToList();

                var skuList = new List<NewIndentSkuListDto>();
                if (skuListContext != null && skuListContext.Any())
                {
                    skuList = skuListContext.Select(_ => new NewIndentSkuListDto()
                    {
                        SkuId = _.Id,
                        SkuName = _.SkuName ?? string.Empty,
                        SkuCode = _.SkuCode ?? string.Empty
                    }).ToList();
                }

                if (skuList != null && skuList.Any())
                {
                    skuList = skuList.GroupBy(_ => _.SkuId).Select(_ => new NewIndentSkuListDto()
                    {
                        SkuId = _.FirstOrDefault().SkuId,
                        SkuName = _.FirstOrDefault().SkuName,
                        BidQuantityCase = _.Sum(su => su.BidQuantityCase),
                        SkuCode = _.FirstOrDefault().SkuCode
                    }).ToList();

                    var SkuContext = _emamiContext.Skus.AsNoTracking();
                    var volumeContext = _emamiContext.VolumeLoadability.AsNoTracking();
                    var vehicleCapacityKey = UtilityHelper.GetEnumDescription((DTO.Enums.Configuration.MaximumVehicleCapacityinPercent));
                    var volumeCapacityKey = UtilityHelper.GetEnumDescription((DTO.Enums.Configuration.MaximumVolumeCapacityinPercent));
                    var vehicleCapacity = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Key == vehicleCapacityKey)?.Value ?? "0";
                    var vehicleCapacityDisplay = Convert.ToDecimal(vehicleCapacity);
                    var volumeCapacity = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Key == volumeCapacityKey)?.Value ?? "0";
                    var volumeCapacityDisplay = Convert.ToDecimal(volumeCapacity);
                    foreach (var sku in skuList)
                    {
                        var volumeLoadabilityContext = volumeContext.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.SkuId == sku.SkuId && _.IsActive && _.VehicleSize == skuInputDto.VehicleSize && _.PlantId == skuInputDto.PlantId);
                        var skuAfterDetection = new NewIndentSkuListDto();
                        skuAfterDetection.SkuId = sku.SkuId;
                        skuAfterDetection.SkuName = sku.SkuName + "-" + sku.SkuCode;
                        skuAfterDetection.SkuCode = sku.SkuCode;
                        skuAfterDetection.CaseToMetricTonValue = _resultService.ConvertCasetoMetricTon(1, sku.SkuId);
                        skuAfterDetection.MaxAllowableCasesSingleSku = volumeLoadabilityContext != null ? volumeLoadabilityContext.MaxAllowableSinglesku : 0;
                        skuAfterDetection.MaxAllowableCasesMultipleSku = volumeLoadabilityContext != null ? volumeLoadabilityContext.MaxAllowableMultiplesku : 0;
                        skuAfterDetection.GrossWeight = (SkuContext.FirstOrDefault(_ => _.Id == sku.SkuId) != null) ? SkuContext.FirstOrDefault(_ => _.Id == sku.SkuId).GrossWeight : 0;
                        skuAfterDetection.MaximumVehicleCapacityInPercent = vehicleCapacityDisplay;
                        skuAfterDetection.MaximumVolumeCapacityInPercent = volumeCapacityDisplay;
                        skuAfterDetectionList.Add(skuAfterDetection);
                        //var liftingRequestListContext = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.SkuId == sku.SkuId 
                        //&& _.LiftingRequest.UserId == dealerContext.Id
                        //      && _.LiftingRequest.StatusId == (int)DTO.Enums.Status.Pending);

                        // var liftingRequestListContext = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.SkuId == sku.SkuId
                        //&& _.LiftingRequest.UserId == dealerContext.Id
                        //      && _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected);

                        // if (liftingRequestListContext != null && liftingRequestListContext.Any())
                        // {
                        //     decimal test = liftingRequestListContext.Sum(_ => _.LiftingQuantityCase);
                        //     if (sku.BidQuantityCase - liftingRequestListContext.Sum(_ => _.LiftingQuantityCase) > 0)
                        //     {
                        //         skuAfterDetection.BidQuantityCase = sku.BidQuantityCase - liftingRequestListContext.Sum(_ => _.LiftingQuantityCase);
                        //         skuAfterDetectionList.Add(skuAfterDetection);
                        //     }
                        // }
                        // else
                        // {
                        //     skuAfterDetection.BidQuantityCase = sku.BidQuantityCase;
                        //     skuAfterDetectionList.Add(skuAfterDetection);
                        // }
                    }


                    return _resultService.SuccessObject(skuAfterDetectionList);
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
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetSkuListBasedOnVehicleSize(SkuInputDto skuInputDto)
        {
            _methodName = "GetSkuListBasedOnVehicleSize";
            var skuAfterDetectionList = new List<NewIndentSkuListDto>();
            try
            {
                if (skuInputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (skuInputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == skuInputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (skuInputDto.VehicleSize == 0)
                {
                    return _resultService.ErrorMessage(Constants.VehicleSizeNotFound);
                }
                //var skuListContext = _emamiContext.SaudaOrders.AsNoTracking().Join(_emamiContext.Sauda.AsNoTracking(), so => so.SaudaId, s => s.Id, (so, s) => new { so, s })
                //    .GroupJoin(_emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking(), sos => sos.so.Id, lr => lr.SaudaOrderId, (sos, lr) => new { sos.so, sos.s, lr })
                //    .Where(_ => _.so.StatusId == (int)DTO.Enums.Status.Approved && _.so.SaudaNumber != "" && _.so.SaudaNumber != null && _.so.OilTypeId == skuInputDto.OilTypeId
                //    && _.s.UserId == skuInputDto.DealerId && _.so != null && _.s != null);
                //var skuList = new List<NewIndentSkuListDto>();
                //if (skuListContext != null && skuListContext.Any())
                //{
                //    skuList = skuListContext.Select(_ => new NewIndentSkuListDto()
                //    {
                //        SkuId = _.so.SkuId,
                //        SkuName = _.so.Sku != null ? _.so.Sku.SkuName : string.Empty,
                //        BidQuantityCase = _.so.BidQuantityCase - (_.lr.Any() ? _.lr.Select(s => s.LiftingQuantityCase).DefaultIfEmpty(0).Sum() : 0),
                //    }).Where(_ => _.BidQuantityCase != 0).ToList();
                //}

                //var skuListContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda.UserId == skuInputDto.DealerId
                //&& _.OilTypeId == skuInputDto.OilTypeId &&
                //(_.StatusId == (int)DTO.Enums.Status.Approved || _.StatusId == (int)DTO.Enums.Status.Completed)).ToList();

                var skuListContext = _emamiContext.Skus.AsNoTracking().Where(_ => skuInputDto.SkuIds.Contains(_.Id) && _.IsActive).ToList();

                var skuList = new List<NewIndentSkuListDto>();

                if (skuListContext != null && skuListContext.Any())
                {
                    var SkuContext = _emamiContext.Skus.AsNoTracking();
                    var volumeContext = _emamiContext.VolumeLoadability.AsNoTracking();
                    var vehicleCapacityKey = UtilityHelper.GetEnumDescription((DTO.Enums.Configuration.MaximumVehicleCapacityinPercent));
                    var volumeCapacityKey = UtilityHelper.GetEnumDescription((DTO.Enums.Configuration.MaximumVolumeCapacityinPercent));
                    var vehicleCapacity = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Key == vehicleCapacityKey)?.Value ?? "0";
                    var vehicleCapacityDisplay = Convert.ToDecimal(vehicleCapacity);
                    var volumeCapacity = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Key == volumeCapacityKey)?.Value ?? "0";
                    var volumeCapacityDisplay = Convert.ToDecimal(volumeCapacity);
                    foreach (var sku in skuListContext)
                    {
                        var volumeLoadabilityContext = volumeContext.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.SkuId == sku.Id && _.IsActive && _.VehicleSize == skuInputDto.VehicleSize && _.PlantId == skuInputDto.PlantId);
                        var skuAfterDetection = new NewIndentSkuListDto();
                        skuAfterDetection.SkuId = sku.Id;
                        skuAfterDetection.SkuName = sku.SkuName;
                        skuAfterDetection.SkuCode = sku.SkuCode;
                        skuAfterDetection.CaseToMetricTonValue = _resultService.ConvertCasetoMetricTon(1, sku.Id);
                        skuAfterDetection.MaxAllowableCasesSingleSku = volumeLoadabilityContext != null ? volumeLoadabilityContext.MaxAllowableSinglesku : 0;
                        skuAfterDetection.MaxAllowableCasesMultipleSku = volumeLoadabilityContext != null ? volumeLoadabilityContext.MaxAllowableMultiplesku : 0;
                        skuAfterDetection.GrossWeight = (SkuContext.FirstOrDefault(_ => _.Id == sku.Id) != null) ? SkuContext.FirstOrDefault(_ => _.Id == sku.Id).GrossWeight : 0;
                        skuAfterDetection.MaximumVehicleCapacityInPercent = vehicleCapacityDisplay;
                        skuAfterDetection.MaximumVolumeCapacityInPercent = volumeCapacityDisplay;
                        skuAfterDetection.OilTypeId = sku.OilTypeId ?? 0;
                        skuAfterDetectionList.Add(skuAfterDetection);
                        //var liftingRequestListContext = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.SkuId == sku.SkuId 
                        //&& _.LiftingRequest.UserId == dealerContext.Id
                        //      && _.LiftingRequest.StatusId == (int)DTO.Enums.Status.Pending);

                        // var liftingRequestListContext = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.SkuId == sku.SkuId
                        //&& _.LiftingRequest.UserId == dealerContext.Id
                        //      && _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected);

                        // if (liftingRequestListContext != null && liftingRequestListContext.Any())
                        // {
                        //     decimal test = liftingRequestListContext.Sum(_ => _.LiftingQuantityCase);
                        //     if (sku.BidQuantityCase - liftingRequestListContext.Sum(_ => _.LiftingQuantityCase) > 0)
                        //     {
                        //         skuAfterDetection.BidQuantityCase = sku.BidQuantityCase - liftingRequestListContext.Sum(_ => _.LiftingQuantityCase);
                        //         skuAfterDetectionList.Add(skuAfterDetection);
                        //     }
                        // }
                        // else
                        // {
                        //     skuAfterDetection.BidQuantityCase = sku.BidQuantityCase;
                        //     skuAfterDetectionList.Add(skuAfterDetection);
                        // }
                    }


                    return _resultService.SuccessObject(skuAfterDetectionList);
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
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetSaudaNumberList(LoginUserIdDto inputDto)
        {
            _methodName = "GetSaudaNumberList";
            var saudaOrderListDto = new List<SaudaOrderListDto>();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                IQueryable<UserCustomerMapping> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId);
                if (dealersList != null && dealersList.Any())
                {
                    var saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && dealersList.Any(a => a.CustomerId == _.Sauda.UserId)
                        && _.StatusId == (int)DTO.Enums.Status.Approved && _.SaudaNumber != "" && _.SaudaNumber != null).ToList();
                    if (saudaOrderListContext != null && saudaOrderListContext.Any())
                    {
                        saudaOrderListDto = saudaOrderListContext.Select(_ => new SaudaOrderListDto()
                        {
                            SaudaId = _.Id,
                            SaudaOrderId = _.Id,
                            SaudaNumber = _.Sauda.SaudaNumber != null ? _.Sauda.SaudaNumber : String.Empty,
                        }).ToList();
                    }
                }

                if (saudaOrderListDto != null && saudaOrderListDto.Any())
                {
                    return _resultService.SuccessObject(saudaOrderListDto);
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
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetSaudaShortViewDetails(SaudaDetailInputDto inputDto)
        {
            _methodName = "GetSaudaShortViewDetails";
            try
            {
                var saudaDetails = new SaudaOrderDetails();
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.UserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.UserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                IQueryable<UserCustomerMapping> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == inputDto.UserId);
                if (dealersList != null && dealersList.Any())
                {

                    SaudaOrder saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SaudaId && _.Sauda != null && dealersList.Any(a => a.CustomerId == _.Sauda.UserId));
                    if (saudaOrderContext != null)
                    {
                        if (saudaOrderContext.Sauda != null)
                        {
                            var dealerContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == saudaOrderContext.Sauda.UserId);
                            if (dealerContext != null)
                            {
                                saudaDetails.DealerId = dealerContext.Id;
                                saudaDetails.DealerName = dealerContext.Name;
                            }
                            saudaDetails.BookedDate = saudaOrderContext.Sauda.BiddingDate;

                        }
                        saudaDetails.SaudaId = saudaOrderContext.Id;
                        saudaDetails.SaudaOrderId = saudaOrderContext.Id;
                        saudaDetails.SaudaNumber = saudaOrderContext.SaudaNumber;
                        saudaDetails.ValidToDate = saudaOrderContext.ValidToDate;
                        saudaDetails.OilTypeId = saudaOrderContext.OilTypeId;
                        saudaDetails.OilTypeName = saudaOrderContext.OilType != null ? saudaOrderContext.OilType.Name : string.Empty;
                        saudaDetails.SkuId = saudaOrderContext.SkuId;
                        saudaDetails.SkuName = saudaOrderContext.Sku != null ? saudaOrderContext.Sku.SkuName : string.Empty;
                        IQueryable<SaudaOrderLiftingRequestMapping> liftingReqOrderContextList = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => _.SaudaOrderId == saudaOrderContext.Id
                            && _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected);
                        if (liftingReqOrderContextList != null && liftingReqOrderContextList.Any())
                        {
                            saudaDetails.BidQuantity = saudaOrderContext.BidQuantity - liftingReqOrderContextList.Sum(_ => _.LiftingQuantity);
                            saudaDetails.BidQuantityCases = saudaOrderContext.BidQuantityCase - liftingReqOrderContextList.Sum(_ => _.LiftingQuantityCase);
                        }
                        else
                        {
                            saudaDetails.BidQuantity = saudaOrderContext.BidQuantity;
                            saudaDetails.BidQuantityCases = saudaOrderContext.BidQuantityCase;
                        }
                        saudaDetails.BidPrice = saudaOrderContext.BidPrice;
                        saudaDetails.BidPricePerCase = Math.Round((saudaOrderContext.BidPrice != 0 && saudaOrderContext.BidQuantityCase != 0 ? (saudaOrderContext.BidPrice / saudaOrderContext.BidQuantityCase) : 0), 2);
                        saudaDetails.IncoTerms = saudaOrderContext.Incoterms1;
                        var plantContext = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.PlantId);
                        if (plantContext != null)
                        {
                            saudaDetails.PlantDepot = plantContext.Name;
                        }
                        //var freightRouteContext = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.DealerLocationId);
                        //if (freightRouteContext != null)
                        //{
                        //    saudaDetails.FrieghtRoute = freightRouteContext.Name;
                        //}

                        saudaDetails.PlantId = saudaOrderContext.PlantId;
                        // saudaDetails.DepotId = saudaOrderContext.DepotIdForRake;
                        saudaDetails.IncoTermId = saudaOrderContext.Incoterms2;
                    }
                    return _resultService.SuccessObject(saudaDetails);
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
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #region Sauda Limit Request


        public ResultDto GetSaudaLimitRequestHistory(IdInputDto inputDto)
        {
            _methodName = "GetSaudaLimitRequestHistory";
            var saudalimitHistoryDto = new List<SaudaLimitRequestHistoryDto>();
            if (inputDto == null)
            {
                return NotFoundResult();
            }
            try
            {
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.Id && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                List<long> inputIds = new List<long>();
                var userDealerRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.Dealer);
                if (userDealerRoleContext != null)
                {
                    inputIds.Add(userDealerRoleContext.UserId);
                }
                var userBDORoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userBDORoleContext != null)
                {
                    var BDODealerContext = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == userContext.Id).Select(_ => _.CustomerId).ToList();
                    if (BDODealerContext != null && BDODealerContext.Any())
                    {
                        inputIds.Add(userContext.Id);
                        inputIds.AddRange(BDODealerContext.ToList());
                    }
                }
                var userZHRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.ZonalTrader);
                if (userZHRoleContext != null)
                {
                    var ZHBDOContext = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == userContext.Id).Select(_ => _.Id).ToList();
                    if (ZHBDOContext != null && ZHBDOContext.Any())
                    {
                        var BDODealerContext = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => ZHBDOContext.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                        if (BDODealerContext != null && BDODealerContext.Any())
                        {
                            inputIds.Add(userContext.Id);
                            inputIds.AddRange(ZHBDOContext.ToList());
                            inputIds.AddRange(BDODealerContext.ToList());
                        }
                    }
                }
                var userNHRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.NationalTrader);
                if (userNHRoleContext != null)
                {
                    var userZHDRoleContext = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == userContext.Id).Select(_ => _.Id).ToList();
                    if (userZHDRoleContext != null && userZHDRoleContext.Any())
                    {
                        var ZHBDOContext = _emamiContext.Users.AsNoTracking().Where(_ => userZHDRoleContext.Contains(_.ReportingToId ?? 0)).Select(_ => _.Id).ToList();
                        if (ZHBDOContext != null && ZHBDOContext.Any())
                        {
                            var BDODealerContext = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => ZHBDOContext.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                            if (BDODealerContext != null && BDODealerContext.Any())
                            {
                                inputIds.Add(userContext.Id);
                                inputIds.AddRange(userZHDRoleContext.ToList());
                                inputIds.AddRange(ZHBDOContext.ToList());
                                inputIds.AddRange(BDODealerContext.ToList());
                            }
                        }
                    }
                }

                saudalimitHistoryDto = _emamiContext.SaudaLimit.AsNoTracking().Where(_ => inputIds.Contains(_.CreatedBy)).AsNoTracking().Select(c => new SaudaLimitRequestHistoryDto
                {
                    Id = c.Id,
                    LimitRequestNo = c.Id.ToString(),
                    Remarks = c.Remarks != null ? c.Remarks : string.Empty,
                    RequestDate = c.CreatedDate,
                    RequestQuantityLimit = c.RequestedLimit,
                    StatusId = c.StatusId,
                    Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == c.StatusId).Name,
                    DealerId = c.UserId,
                    DealerName = _emamiContext.Users.FirstOrDefault(_ => _.Id == c.UserId).Name

                }).ToList();
                var ouputDto = saudalimitHistoryDto.GroupBy(_ => _.DealerId).Select(c => new SaudaLimitGroupDto()
                {
                    DealerId = (long)c.Key,
                    DealerName = _emamiContext.Users.FirstOrDefault(_ => _.Id == c.Key).Name,
                    saudahistory = c.Select(s => new SaudaLimitOutputDto()
                    {
                        Id = s.Id,
                        LimitRequestNo = s.Id.ToString(),
                        Remarks = s.Remarks,
                        RequestDate = s.RequestDate,
                        RequestQuantityLimit = s.RequestQuantityLimit,
                        StatusId = s.StatusId,
                        Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == s.StatusId).Name,

                    }).ToList()
                }).ToList();

                return SucessResult(ouputDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto GetSaudaLimitRequestHistoryDetail(IdInputDto inputDto)
        {
            _methodName = "GetSaudaLimitRequestHistory";
            var saudalimitHistoryDto = new SaudaLimitRequestHistoryDto();
            if (inputDto == null)
            {
                return NotFoundResult();
            }
            try
            {
                var saudalimitHistoryContext = _emamiContext.SaudaLimit.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.Id);
                if (saudalimitHistoryContext == null)
                {
                    saudalimitHistoryDto.Id = saudalimitHistoryContext.Id;
                    saudalimitHistoryDto.LimitRequestNo = saudalimitHistoryContext.Id.ToString();
                    saudalimitHistoryDto.Remarks = saudalimitHistoryContext.Remarks;
                    saudalimitHistoryDto.RequestDate = saudalimitHistoryContext.CreatedDate;
                    saudalimitHistoryDto.RequestQuantityLimit = saudalimitHistoryContext.RequestedLimit;
                    saudalimitHistoryDto.StatusId = saudalimitHistoryContext.StatusId;
                    saudalimitHistoryDto.Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == saudalimitHistoryContext.StatusId).Name;
                    saudalimitHistoryDto.DealerId = saudalimitHistoryContext.UserId;
                    saudalimitHistoryDto.DealerName = saudalimitHistoryContext.User.Name;
                }
                return SucessResult(saudalimitHistoryDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto AddSaudaLimitRequest(SaudaLimitRequestHistoryDto inputDto)
        {
            _methodName = "AddSaudaLimitRequest";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.CreatedBy == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
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

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.CreatedBy);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }

                var userdivContext = _emamiContext.UserDivisionMappings.AsNoTracking()
                    .FirstOrDefault(_ => _.UserId == inputDto.DealerId
                    && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId
                    && _.DivisionId == inputDto.DivisionId);
                if (userdivContext.SaudaLimit == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.AdminContactMessage;
                    resultDto.ErrorDto.Message = Constants.AdminContactMessage;
                    return resultDto;
                }

                var saudalimitContext = new SaudaLimit
                {
                    UserId = inputDto.DealerId,
                    ActualLimit = inputDto.ActualLimit,
                    RequestedLimit = inputDto.RequestQuantityLimit,
                    StatusId = (int)DTO.Enums.Status.Pending,
                    Remarks = inputDto.Remarks,
                    CreatedBy = inputDto.CreatedBy,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    IsSAPDataSyncOrNot = false,
                    SalesOrganizationId = inputDto.SalesOrganizationId,
                    DistributionChannelId = inputDto.DistributionChannelId,
                    DivisionId = inputDto.DivisionId
                };
                _emamiContext.SaudaLimit.Add(saudalimitContext);
                _emamiContext.SaveChanges();

                try
                {
                    var users = _emamiContext.Users.AsNoTracking();
                    var usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == inputDto.CreatedBy || _.Id == inputDto.DealerId);
                    var saudaLimit = _emamiContext.SaudaLimit.AsNoTracking().FirstOrDefault(_ => _.Id == saudalimitContext.Id);
                    if (usersContext != null && usersContext.Any() && saudaLimit != null)
                    {
                        decimal actualLimit = saudaLimit.ActualLimit;
                        decimal extendedLimit = saudaLimit.ActualLimit + saudaLimit.RequestedLimit;
                        List<string> toUsers = new List<string>();
                        var createdBy = usersContext.FirstOrDefault(_ => _.Id == inputDto.CreatedBy);
                        var dealer = usersContext.FirstOrDefault(_ => _.Id == inputDto.DealerId);
                        var CreatedByRole = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.CreatedBy).RoleId;
                        var reportingContext = new User();
                        if (CreatedByRole == (int)DTO.Enums.Role.StateTrader)
                        {
                            var zh = users.FirstOrDefault(_ => _.Id == inputDto.CreatedBy).ReportingToId;
                            var reportingId = users.FirstOrDefault(_ => _.Id == zh).ReportingToId;
                            reportingContext = users.FirstOrDefault(_ => _.Id == reportingId);
                        }
                        else if (CreatedByRole == (int)DTO.Enums.Role.ZonalTrader)
                        {
                            var reportingId = users.FirstOrDefault(_ => _.Id == inputDto.CreatedBy).ReportingToId;
                            reportingContext = users.FirstOrDefault(_ => _.Id == reportingId);
                        }

                        if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
                        {
                            toUsers.Add(createdBy.Email);
                        }
                        if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                        {
                            toUsers.Add(dealer.Email);
                        }
                        bool isEmail = false;
                        var DealerNotificationContext = _emamiContext.TPNotification.AsNoTracking().
                                                        Join(_emamiContext.TPNotificationDetails.AsNoTracking(), TPN => TPN.Id, TPND => TPND.TPNotificationId, (TPN, TPND) => new { TPN, TPND })
                                                        .Where(_ => _.TPND.DealerId == inputDto.DealerId && _.TPND.NotificationActionId == (int)DTO.Enums.NotificationActionTP.LimitEnhancementRequestCreation && _.TPND.IsActive).ToList();

                        var DealerEmailNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.Email);
                        if (DealerEmailNotificationEnabled != null && DealerEmailNotificationEnabled.Any())
                            isEmail = true;
                        else
                            isEmail = false;
                        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                        if (isEmail && toUsers != null && toUsers.Any())
                        {
                            var fromEmail = Constants.FromEmail;
                            var emailSubject = Constants.SaudaLimitExtensionCreationSubject;
                            var plainText = string.Empty;
                            var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaLimitExtensionCreationEmail);
                            if (emailTemplate != null)
                            {
                                var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.ContractQty, actualLimit.ToString()).Replace(Constants.Quantity, extendedLimit.ToString()).Replace(Constants.CustomerName, dealer.Name);
                                var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                            }

                            if (reportingContext != null)
                            {
                                var maillist = new List<string> { reportingContext.Email };
                                var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, Constants.SaudaLimitRequestForApproval);
                                amazonNotificationService.SendEmail(maillist, emailSubject, plainText, htmlTemplate, true);
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
                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaLimitExtensionCreationSMS);
                            if (smsTemplate != null)
                            {
                                smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.ContractQty, actualLimit.ToString()).Replace(Constants.Quantity, extendedLimit.ToString()).Replace(Constants.CustomerName, dealer.Name);
                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                                {
                                    amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber);
                                }
                                if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                {
                                    amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber);
                                }
                            }
                        }
                        //if (_resultService.IsPushNotification())
                        //{
                        //    if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
                        //    {
                        //        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                        //        {
                        //            PushTokenKey = createdBy.PushTokenKey,
                        //            RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
                        //            Title = Constants.SaudaLimitExtensionCreationSubject,
                        //            Message = smsPlainTemplate,
                        //            //Id = saudaLimit.Id,
                        //        };
                        //        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                        //    }
                        //    if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
                        //    {
                        //        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                        //        {
                        //            PushTokenKey = dealer.PushTokenKey,
                        //            RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
                        //            Title = Constants.SaudaLimitExtensionCreationSubject,
                        //            Message = smsPlainTemplate,
                        //            //Id = saudaLimit.Id,
                        //        };
                        //        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                        //    }
                        //}
                    }
                }
                catch (Exception ex)
                {

                    var message = $"{ServiceName} Service-Method {_methodName} Exception: {ex}";
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.Exception;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                    _logger.Error(message);
                    return resultDto;
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Message = "Sauda Limit Added Successfully";
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



        #endregion


        #region Sauda Amendment

        /// <summary>
        /// Method used to get the sauda list for amendment
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetSaudaListForAmendment(IdInputDto inputDto)
        {
            _methodName = "GetSaudaListForAmendment";
            var resultDto = new ResultDto();
            var saudaListDto = new List<SaudaListOutputDto>();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.LoginUserId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }

                var saudaList = _emamiContext.Sauda.AsNoTracking().Where(_ => _.UserId == inputDto.Id).OrderByDescending(_ => _.CreatedDate).AsQueryable();

                if (saudaList == null || !saudaList.Any())
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                foreach (var sauda in saudaList.ToList())
                {
                    var saudaDto = new SaudaListOutputDto
                    {
                        SaudaId = sauda.Id,
                        BiddingDate = sauda.BiddingDate,
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
        /// 
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto SaveSaudaAmendment(SaudaAmendmentInputDto inputDto)
        {
            _methodName = "SaveSaudaAmendment";

            if (inputDto == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }

            var resultDto = new ResultDto();
            var saoInputDto = inputDto.saudaAmedmantOrdersInputDto;
            var outputDto = new SaudaAmendmentInputDto();

            using (var transaction = _emamiContext.Database.BeginTransaction())
            {
                try
                {
                    var saEntity = new SaudaConversion()
                    {
                        SaudaOrderId = inputDto.SaudaId,
                        DealerId = inputDto.DealerId,
                        ExtendToDate = inputDto.ToDate,
                        StatusId = (int)DTO.Enums.Status.Pending,
                        ExpiryDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                    };

                    _emamiContext.SaudaConversion.Add(saEntity);
                    _emamiContext.SaveChanges();

                    var saoEntity = new SaudaConversionOrder()
                    {
                        SaudaConversionId = saEntity.Id,
                        SaudaId = inputDto.SaudaId,
                        SkuId = saoInputDto.SkuId,
                        OilTypeId = saoInputDto.OilTypeId,
                        QuotedPrice = saoInputDto.QuotedPrice,
                        BidPrice = saoInputDto.BidPrice,
                        BidQuantity = saoInputDto.BidQuantity
                    };

                    _emamiContext.SaudaConversionOrder.Add(saoEntity);
                    _emamiContext.SaveChanges();
                    transaction.Commit();

                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = saEntity.Id;
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.Exception;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                    _logger.Error(message);
                    return resultDto;
                }
            }
            return resultDto;
        }

        #endregion

        #region Sauda Chart

        /// <summary>
        /// Method used to sauda outstanding chart for dealer app
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetDealerOutstandingSaudaListForChart(LoginUserIdDto inputDto)
        {
            _methodName = "GetOutstandingSaudaForChart";
            var resultDto = new ResultDto();
            var saudaListDto = new List<SaudaListOutputDto>();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.LoginUserId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }

                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var saudaStatus = Constants.OutstandingSaudaStatus;
                var saudaDateGroupbyList = from sauda in _emamiContext.Sauda.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                                           join saudaOrder in _emamiContext.SaudaOrders.AsNoTracking().Where(_ => saudaStatus.Contains(_.StatusId)) on sauda.Id equals saudaOrder.SaudaId
                                           group saudaOrder by new { sauda.Id, sauda.BiddingDate } into saudaGroup
                                           select new { Id = saudaGroup.Key.Id, Days = (currentDate - saudaGroup.Key.BiddingDate).Days, SaudaList = saudaGroup.Sum(_ => _.BidQuantity) };



                if (saudaDateGroupbyList == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                var saudaGroupbyList = saudaDateGroupbyList.GroupBy(_ => _.Days).Select(g => new SaudaOutstandingChartOutputDto { Days = g.Key, Quantity = g.Sum(x => x.SaudaList) });

                if (saudaGroupbyList == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }


                return _resultService.SuccessMessageWitObject(saudaGroupbyList, Constants.PriceDetailsSavedSuccessfully);
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

        public ResultDto GetBodOutstandingSaudaListForChart(LoginUserIdDto inputDto)
        {
            _methodName = "GetBodOutstandingSaudaListForChart";
            var resultDto = new ResultDto();
            var saudaListDto = new List<SaudaListOutputDto>();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.LoginUserId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }

                var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId).Select(_ => _.CustomerId).ToList();

                if (dealerIds == null || dealerIds.Count == 0)
                {
                    return _resultService.ErrorMessage(Constants.DeaerNotMappingToTheUser);
                }

                var saudaStatus = Constants.OutstandingSaudaStatus;
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var saudaDateGroupbyList = from sauda in _emamiContext.Sauda.AsNoTracking().Where(_ => dealerIds.Contains(_.UserId))
                                           join saudaOrder in _emamiContext.SaudaOrders.AsNoTracking().Where(_ => saudaStatus.Contains(_.StatusId)) on sauda.Id equals saudaOrder.SaudaId
                                           group saudaOrder by new { sauda.Id, sauda.BiddingDate } into saudaGroup
                                           select new { Id = saudaGroup.Key.Id, Days = (currentDate - saudaGroup.Key.BiddingDate).Days, SaudaList = saudaGroup.Sum(_ => _.BidQuantity) };

                if (saudaDateGroupbyList == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                var saudaGroupbyList = saudaDateGroupbyList.GroupBy(_ => _.Days).Select(g => new SaudaOutstandingChartOutputDto { Days = g.Key, Quantity = g.Sum(x => x.SaudaList) });

                if (saudaGroupbyList == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }


                return _resultService.SuccessMessageWitObject(saudaGroupbyList, Constants.PriceDetailsSavedSuccessfully);
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

        /// <summary>
        /// Method to get Get OutStanding Sauda List
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetOutStandingSaudaList(SaudaFilterDto saudaFilterDto)
        {
            _methodName = "GetSaudaList";
            var resultDto = new ResultDto();
            var outstandingsaudaListDto = new List<OutstandingSaudaDto>();
            try
            {
                if (saudaFilterDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
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

                var dealerlist = new List<UserMasterDto>();
                var status = Constants.OutstandingSaudaStatus;
                if (saudaFilterDto.DealerId == 0)
                {
                    dealerlist = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                  join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                                  where ucm.UserId == saudaFilterDto.UserId
                                  select new UserMasterDto
                                  {
                                      Id = u.Id,
                                      EmployeeName = u.Name,
                                      EmployeeCode = u.Code
                                  }).ToList();
                }
                else
                {
                    dealerlist = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                  join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                                  where ucm.UserId == saudaFilterDto.UserId && ucm.CustomerId == saudaFilterDto.DealerId
                                  select new UserMasterDto
                                  {
                                      Id = u.Id,
                                      EmployeeName = u.Name,
                                      EmployeeCode = u.Code
                                  }).ToList();
                }

                var saudaList = (from sauda in _emamiContext.Sauda.AsNoTracking()
                                 join saudaOrder in _emamiContext.SaudaOrders.AsNoTracking() on sauda.Id equals saudaOrder.SaudaId
                                 join dealer in dealerlist on sauda.UserId equals dealer.Id
                                 where status.Contains(saudaOrder.StatusId)
                                 && DbFunctions.TruncateTime(sauda.BiddingDate) >= DbFunctions.TruncateTime(saudaFilterDto.FromDate)
                                 && DbFunctions.TruncateTime(sauda.BiddingDate) <= DbFunctions.TruncateTime(saudaFilterDto.ToDate)
                                 select sauda
                                 ).AsQueryable();

                if (!saudaList.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                foreach (var sauda in saudaList.ToList())
                {
                    var saudaDto = new OutstandingSaudaDto
                    {
                        SaudaId = sauda.Id,
                        BiddingDate = sauda.BiddingDate,
                        DealerId = sauda.UserId,
                        DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == sauda.UserId).Name,
                        BiddingPrice = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == sauda.Id).Sum(_ => (decimal?)_.BidPrice) ?? 0,
                    };
                    outstandingsaudaListDto.Add(saudaDto);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outstandingsaudaListDto;
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

        public ResultDto GetDealerSaudaLists(IdInputDto IdDto)
        {
            _methodName = "GetDealerSaudaLists";
            var resultDto = new ResultDto();
            var outputDto = new List<SaudaListGroupedOutputDto>();
            try
            {
                if (IdDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (IdDto.Id == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                //var saudaContext = _emamiContext.Sauda.AsNoTracking().Where(_ => _.UserId == IdDto.Id && _.SaudaStatusId != (int)DTO.Enums.SaudaStatus.Processed);

                var usercontext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == IdDto.LoginUserId);
                if (usercontext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }


                IEnumerable<DivisionDetailsDto> divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == IdDto.LoginUserId)
                .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });


                var bdoIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.CustomerId == IdDto.Id).Select(s => s.UserId).ToList();

                //var saudaContext = _emamiContext.Sauda.AsNoTracking().Where(_ => _.UserId == IdDto.Id);

                var saudaContext = (from s in _emamiContext.Sauda.AsNoTracking()
                                    join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                                        equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                    where s.UserId == IdDto.Id
                                    && DbFunctions.TruncateTime(s.CreatedDate) <= DbFunctions.TruncateTime(IdDto.ToDate)
                                    && DbFunctions.TruncateTime(s.CreatedDate) >= DbFunctions.TruncateTime(IdDto.FromDate)
                                    //&& bdoIds.Contains(s.BdoId)
                                    select s
                                    ).ToList();



                if (!saudaContext.IsAny())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }
                var saudaIdList = saudaContext.Select(s => s.Id).Distinct().ToList();
                var saudaOrderContextData = _emamiContext.SaudaOrders.AsNoTracking()
                   .Where(_ => saudaIdList.Contains(_.SaudaId))
                   .Select(s => new { SaudaId = s.SaudaId, BidPrice = s.BidPrice, SaudaNumber = s.SaudaNumber, QuantityInMT = s.BidQuantity });

                outputDto = saudaContext.OrderByDescending(_ => _.BiddingDate).GroupBy(s => s.BiddingDate.Date).Select(a => new SaudaListGroupedOutputDto()
                {
                    BiddingDate = a.Key,
                    saudaListOutputs = a.Select(sauda => new SaudaListOutputDto()
                    {
                        SaudaId = sauda.Id,
                        SaudaNo = sauda.Id.ToString(),
                        SaudaOrderId = sauda.Id,
                        BiddingDate = sauda.BiddingDate,
                        TotalAmt = saudaOrderContextData.Where(_ => _.SaudaId == sauda.Id).Sum(price => price.BidPrice),
                        TotalQty = saudaOrderContextData.Where(_ => _.SaudaId == sauda.Id).Sum(qty => qty.QuantityInMT),
                        SaudaNumber = sauda.SaudaNumber != null ? sauda.SaudaNumber : string.Empty
                    }).ToList()
                }).ToList();

                //foreach (var sauda in saudaContext)
                //{
                //    var saudaoutputDto = new SaudaListOutputDto
                //    {
                //        SaudaId = sauda.Id,
                //        SaudaNo = sauda.Id.ToString(),
                //        SaudaOrderId = sauda.Id,
                //        BiddingDate = sauda.BiddingDate
                //    };
                //    var saudaOrderContext = saudaOrderContextData.Where(_ => _.SaudaId == sauda.Id).ToList();
                //    saudaoutputDto.TotalAmt = saudaOrderContext.Sum(_ => _.BidPrice);
                //    saudaoutputDto.TotalQty = saudaOrderContext.Sum(_ => _.QuantityInMT);
                //    outputDto.Add(saudaoutputDto);
                //}
                // outputDto = outputDto.OrderByDescending(_ => _.BiddingDate).ToList();
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


        public ResultDto GetDealerSalesLists(IdInputDto IdDto)
        {
            _methodName = "GetDealerSaudaLists";
            var resultDto = new ResultDto();
            var outputDto = new List<SaudaListOutputDto>();
            try
            {
                if (IdDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (IdDto.Id == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == IdDto.LoginUserId);

                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                IEnumerable<DivisionDetailsDto> divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == IdDto.LoginUserId)
                 .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });


                var saudaContext = (from i in _emamiContext.SalesRegister.AsNoTracking()
                                    join sku in _emamiContext.Skus.AsNoTracking() on i.MaterialCode equals sku.SkuCode
                                    join u in _emamiContext.Users.AsNoTracking() on i.CustomerCode equals u.Code
                                    // join skus in _emamiContext.Skus.AsNoTracking() on i.SkuId equals skus.Id
                                    join ud in divisionslogieduser on new { SalesOrganizationId = i.SalesOrganizationId, DistributionChannelId = i.DistributionChannelId, DivisionId = i.DivisionId }
                                             equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                    where u.Id == IdDto.Id
                                    && i.SalesOrganizationId == sku.SalesOrganizationId && i.DistributionChannelId == sku.DistributionChannelId
                                    && i.DivisionId == sku.DivisionId
                                    && DbFunctions.TruncateTime(i.InvoiceDate) >= DbFunctions.TruncateTime(IdDto.FromDate)
                                    && DbFunctions.TruncateTime(i.InvoiceDate) <= DbFunctions.TruncateTime(IdDto.ToDate)
                                    //&& i.SkuId != 0
                                    select i
                                   ).ToList();

                // var invoicelist = saudaContext.Distinct().ToList();
                // var saudaContext = _emamiContext.Invoices.AsNoTracking().Where(_ => _.UserId == IdDto.Id);

                if (!saudaContext.IsAny())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                foreach (var invoice in saudaContext)
                {
                    var saudaoutputDto = new SaudaListOutputDto
                    {
                        SaudaId = invoice.Id,
                        SaudaNo = invoice.Id.ToString(),
                        BiddingDate = invoice.InvoiceDate,
                        TotalAmt = Convert.ToDecimal(invoice.TotalAmount),
                        TotalQty = invoice.QuantityMT
                    };
                    outputDto.Add(saudaoutputDto);
                }
                outputDto = outputDto.OrderByDescending(_ => _.BiddingDate).ToList();

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

        #region Special Rate Approval Request

        public ResultDto AddSpecialRateApprovalRequest(SpecialRateApprovalAddDto inputDto)
        {
            _methodName = "AddSpecialRateApprovalRequest";
            try
            {
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                string errorMessageList = string.Empty;
                var errorFlag = true;
                User userContext = new User();
                User dealerContext = new User();
                var incotermsContext = new IncoTerms();
                var requestTo = 0L;
                var todayPricings = new List<long>();
                if (inputDto == null || inputDto.SpecialRateApprovals == null || !inputDto.SpecialRateApprovals.Any())
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                List<SpecialRateAddInputDto> specialRateApprovalInputListDto = inputDto.SpecialRateApprovals.ToList();
                var specialRateDto = specialRateApprovalInputListDto.FirstOrDefault();
                if (specialRateDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                else
                {
                    userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == specialRateDto.LoginUserId);
                    if (userContext == null)
                    {
                        return _resultService.ErrorMessage(Constants.UserNotFound);
                    }
                    //var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                    //if (userRoleContext == null)
                    //{
                    //    return _resultService.ErrorMessage(Constants.UserNotFound);
                    //}
                }

                if (specialRateDto.SalesOrganizationId == 0)
                {
                    return _resultService.ErrorMessage(Constants.SalesOrganisationIsEmpty);
                }
                if (specialRateDto.DistributionChannelId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DistributionChannelIsEmpty);
                }
                if (specialRateDto.DivisionId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DivisionIsEmpty);
                }
                var userdivContext = _emamiContext.UserDivisionMappings.AsNoTracking()
                    .FirstOrDefault(_ => _.UserId == specialRateDto.LoginUserId
                    && _.SalesOrganizationId == specialRateDto.SalesOrganizationId && _.DistributionChannelId == specialRateDto.DistributionChannelId
                    && _.DivisionId == specialRateDto.DivisionId);

                decimal overallSaudaLimit = 0;
                decimal orderedQuantity = 0;
                decimal liftingQuantity = 0;
                decimal availableQuantity = 0;

                foreach (var specialRateApprovalInputDto in specialRateApprovalInputListDto)
                {

                    var errorMessage = string.Empty;

                    if (specialRateApprovalInputDto.SkuId == 0)
                    {
                        errorMessage = Constants.SKUMissing;
                        errorFlag = false;
                    }
                    else
                    {
                        var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == specialRateApprovalInputDto.SkuId);
                        if (skuContext == null)
                        {
                            errorMessage = Constants.BindErrorMessage(Constants.SKUNotFound, errorMessage);
                            errorFlag = false;
                        }
                        else
                        {
                            errorMessage = skuContext.SkuName;
                            if (specialRateApprovalInputDto.UserId == 0)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.DealerMissing, errorMessage);
                                errorFlag = false;
                            }
                            else
                            {
                                dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == specialRateApprovalInputDto.UserId);
                                if (dealerContext == null)
                                {
                                    errorMessage = Constants.BindErrorMessage(Constants.DealerNotFound, errorMessage);
                                    errorFlag = false;
                                }
                            }

                            if (specialRateApprovalInputDto.SalesOrganizationId == 0 || specialRateApprovalInputDto.DistributionChannelId == 0 || specialRateApprovalInputDto.DivisionId == 0)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.SalesOrgMissing, errorMessage);
                                errorFlag = false;
                            }

                            if (specialRateApprovalInputDto.IncotermsId == 0)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.IncotermsMissing, errorMessage);
                                errorFlag = false;
                            }
                            else
                            {
                                incotermsContext = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == specialRateApprovalInputDto.IncotermsId);
                                if (incotermsContext == null)
                                {
                                    errorMessage = Constants.BindErrorMessage(Constants.IncotermsNotFound, errorMessage);
                                    errorFlag = false;
                                }
                            }
                            if (specialRateApprovalInputDto.PlantId == 0)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.PlantMissing, errorMessage);
                                errorFlag = false;
                            }
                            else
                            {
                                var plantContext = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == specialRateApprovalInputDto.PlantId);
                                if (plantContext == null)
                                {
                                    errorMessage = Constants.BindErrorMessage(Constants.PlantNotFound, errorMessage);
                                    errorFlag = false;
                                }
                            }
                            //if (specialRateApprovalInputDto.DealerLocationId == 0)
                            //{
                            //    errorMessage = Constants.BindErrorMessage(Constants.DealerLocationMissing, errorMessage);
                            //    errorFlag = false;
                            //}
                            //else
                            //{
                            //    var dealerLocationContext = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == specialRateApprovalInputDto.DealerLocationId);
                            //    if (dealerLocationContext == null)
                            //    {
                            //        errorMessage = Constants.BindErrorMessage(Constants.DealerLocationNotFound , errorMessage);
                            //        errorFlag = false;
                            //    }
                            //}
                            if (specialRateApprovalInputDto.OilTypeId == 0)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.OilTypeMissing, errorMessage);
                                errorFlag = false;
                            }
                            else
                            {
                                var oilTypeContext = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == specialRateApprovalInputDto.OilTypeId);
                                if (oilTypeContext == null)
                                {
                                    errorMessage = Constants.BindErrorMessage(Constants.OilTypeNotFound, errorMessage);
                                    errorFlag = false;
                                }
                            }
                            //if (specialRateApprovalInputDto.Quantity == 0)
                            //{
                            //    errorMessage = Constants.BindErrorMessage(Constants.QuantityEmpty, errorMessage);
                            //    errorFlag = false;
                            //}

                            if (specialRateApprovalInputDto.SpecialPrice == 0)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.SpecialPriceEmpty, errorMessage);
                                errorFlag = false;
                            }
                        }

                        overallSaudaLimit = userdivContext.SaudaLimit ?? 0;

                        //IQueryable<SaudaOrder> saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.Sauda.UserId == dealerContext.Id
                        //    && (_.SaudaNumber == null) && _.StatusId == (int)DTO.Enums.Status.Pending);

                        var QuantityLimitForBookingSaudaName = Utility.GetEnumDescription(DTO.Enums.Configuration.QuantityLimitforBookingSaudaEnabled);
                        var configurationContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Name == QuantityLimitForBookingSaudaName);
                        bool IsQuantityLimitForBookingSauda = Convert.ToBoolean(configurationContext.Value);
                        var overallSaudaStatuses = Constants.OverallSaudaStatus;
                        //if (saudaOrderListContext != null && saudaOrderListContext.Any())
                        //{
                        // var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == specialRateApprovalInputDto.SkuId);
                        if (skuContext != null)
                        {

                            decimal availableQuantityBdo = 0;

                            if (configurationContext != null && IsQuantityLimitForBookingSauda)
                            {
                                var bdoLimitContext = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                                .FirstOrDefault(_ => _.UserId == specialRateDto.LoginUserId
                                && _.SkuId == specialRateApprovalInputDto.SkuId
                                && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
                                && DbFunctions.TruncateTime(_.ValidTo) >= DbFunctions.TruncateTime(currentDate));
                                if (bdoLimitContext != null)
                                {
                                    IQueryable<SaudaOrder> saudaOrdersBdoContext = null;
                                    List<long> dealerList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                                        .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.u.Id, uc => uc.CustomerId, (x, uc) => new { DealerId = x.u.Id, x.ur, uc })
                                        .Where(_ => _.uc.UserId == specialRateDto.LoginUserId && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.DealerId).ToList();
                                    if (dealerList != null && dealerList.Any())
                                    {
                                        saudaOrdersBdoContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.SkuId == specialRateApprovalInputDto.SkuId && dealerList.Contains(_.Sauda.UserId)
                                              && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(bdoLimitContext.ValidFrom)
                                              && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(bdoLimitContext.ValidTo) && overallSaudaStatuses.Contains(_.StatusId) && _.IsQuantityLimitForBookingSauda);
                                    }
                                    decimal requestedQuantityBdo = _resultService.ConvertCasetoMetricTon(specialRateApprovalInputDto.Quantity, specialRateApprovalInputDto.SkuId);
                                    decimal orderedQuantityBdo = 0;
                                    decimal totalQuantityBdo = requestedQuantityBdo;
                                    if (saudaOrdersBdoContext != null && saudaOrdersBdoContext.Any())
                                    {
                                        orderedQuantityBdo = saudaOrdersBdoContext.Sum(_ => _.BidQuantity);
                                        totalQuantityBdo = orderedQuantityBdo + requestedQuantityBdo;
                                    }
                                    if (totalQuantityBdo > bdoLimitContext.ActualDiscount)
                                    {
                                        availableQuantityBdo = bdoLimitContext.ActualDiscount - orderedQuantityBdo;
                                        if (availableQuantityBdo < 0)
                                        {
                                            availableQuantityBdo = 0;
                                        }
                                        errorMessage = Constants.BindErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, Math.Round(availableQuantityBdo, 2).ToString()), errorMessage);
                                        errorFlag = false;
                                        //return _resultService.ErrorMessage();
                                    }
                                }
                                else
                                {
                                    errorMessage = Constants.BindErrorMessage(Constants.BDOLimitNotExists, errorMessage);
                                    errorFlag = false;
                                    //return _resultService.ErrorMessage(Constants.BDOLimitNotExists);
                                }
                            }
                        }

                        //decimal invoiceQuantity = 0;
                        //var existingSaudaQuantity = saudaOrderListContext.Sum(_ => _.BidQuantity);
                        //var skuIds = saudaOrderListContext.Select(_ => _.SkuId).Distinct().ToList();

                        var SaudaLimitContext = (from u in _emamiContext.Users.AsNoTracking().Where(_ => _.Id == dealerContext.Id)
                                                 join udm in _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.SalesOrganizationId == specialRateDto.SalesOrganizationId && _.DistributionChannelId == specialRateDto.DistributionChannelId && _.DivisionId == specialRateDto.DivisionId) on u.Id equals udm.UserId
                                                 select new { u.SaudaValidityPeriod, udm.SaudaLimit, udm.DivisionId }).ToList();

                        var usersaudalimit = SaudaLimitContext.Sum(x => x.SaudaLimit ?? 0);
                        // var pendingContracttablevalue = _emamiContext.PendingContracts.AsNoTracking().Where(_ => _.UserId == dealerContext.Id && _.SalesOrgId == specialRateDto.SalesOrganizationId && _.DistChnlId == specialRateDto.DistributionChannelId && _.DivisionId == specialRateDto.DivisionId).ToList().IsAny() ? _emamiContext.PendingContracts.AsNoTracking().Where(_ => _.UserId == dealerContext.Id && _.SalesOrgId == specialRateDto.SalesOrganizationId && _.DistChnlId == specialRateDto.DistributionChannelId && _.DivisionId == specialRateDto.DivisionId).Select(_ => _.SaudaQuantity).Sum() : 0;

                        availableQuantity = _resultService.AvailableSaudaLimit(dealerContext.Id, usersaudalimit, specialRateDto.SalesOrganizationId, specialRateDto.DistributionChannelId, specialRateDto.DivisionId);

                        if (availableQuantity < specialRateApprovalInputListDto.Where(_ => _.UserId == dealerContext.Id).Sum(_ => _resultService.ConvertCasetoMetricTon(_.Quantity, _.SkuId)))
                        {
                            errorMessage = Constants.BindErrorMessage(Constants.SaudaLimitExceeds, errorMessage);
                            errorFlag = false;
                            //  return _resultService.ErrorMessage(Constants.SaudaLimitExceeds);
                        }
                        //}
                    }

                    if (!errorFlag)
                    {
                        if (!string.IsNullOrEmpty(errorMessageList))
                        {
                            errorMessageList = Constants.BindErrorMessage(System.Environment.NewLine + errorMessage, errorMessageList);
                        }
                        else
                        {
                            errorMessageList = Constants.BindErrorMessage(errorMessage, errorMessageList);
                        }
                    }

                }

                //var users = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == specialRateDto.LoginUserId);
                var users = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.UserId == specialRateDto.LoginUserId).Select(_ => _.ReportingToUserId);
                if (users != null && users.Any())
                {
                    var requestedTo = users.FirstOrDefault();
                    if (requestedTo != null)
                    {
                        requestTo = (long)requestedTo;
                    }
                    else
                    {
                        return _resultService.ErrorMessage(Constants.InvalidRequestToUser);
                    }
                }

                if (errorFlag)
                {

                    foreach (var specialRateApprovalInputDto in specialRateApprovalInputListDto)
                    {
                        var specialRate = new SpecialRate
                        {
                            UserId = specialRateApprovalInputDto.UserId,
                            OilTypeId = specialRateApprovalInputDto.OilTypeId,
                            SkuId = specialRateApprovalInputDto.SkuId,
                            QuantityCase = specialRateApprovalInputDto.Quantity,
                            PricingId = specialRateApprovalInputDto.PricingId,
                            Quantity = _resultService.ConvertCasetoMetricTon(specialRateApprovalInputDto.Quantity, specialRateApprovalInputDto.SkuId),
                            FinalPrice = specialRateApprovalInputDto.FinalPrice,
                            SpecialPrice = specialRateApprovalInputDto.SpecialPrice,
                            StatusId = (int)DTO.Enums.Status.Pending,
                            Incoterms2 = specialRateApprovalInputDto.IncotermsId,
                            Incoterms1 = incotermsContext.Name.ToLower().Contains("for") ? "For" : "Ex",
                            //FreightRouteId = dealerContext.FreightRouteId != null ? (long)dealerContext.FreightRouteId : 0,
                            DepotId = specialRateApprovalInputDto.PlantId,
                            CreatedBy = specialRateApprovalInputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            IsLTD = specialRateApprovalInputDto.IsLTD,
                            BrokerId = specialRateApprovalInputDto.BrokerId > 0 ? specialRateApprovalInputDto.BrokerId : 0,
                            SalesOrganizationId = specialRateApprovalInputDto.SalesOrganizationId,
                            DistributionChannelId = specialRateApprovalInputDto.DistributionChannelId,
                            DivisionId = specialRateApprovalInputDto.DivisionId
                        };
                        _emamiContext.SpecialRate.Add(specialRate);
                        _emamiContext.SaveChanges();

                        var specialRateApproval = new SpecialRateApproval
                        {
                            SpecialRateId = specialRate.Id,
                            RequestedBy = specialRateApprovalInputDto.LoginUserId,
                            RequestedTo = requestTo,
                            StatusId = (int)DTO.Enums.Status.Pending,
                            CreatedBy = specialRateApprovalInputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.SpecialRateApproval.Add(specialRateApproval);
                        _emamiContext.SaveChanges();
                        todayPricings.Add(specialRateApprovalInputDto.PricingId);
                        if (_resultService.IsPushNotification())
                        {
                            var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == specialRateApprovalInputDto.SkuId);
                            var reportingToContextList = _emamiContext.Users.AsNoTracking().Where(_ => users.Contains(_.Id));
                            if (reportingToContextList != null && reportingToContextList.Any())
                            {
                                foreach (var reportingToContext in reportingToContextList)
                                {
                                    if (reportingToContext != null && reportingToContext.RegistrationTypeId != null && reportingToContext.RegistrationTypeId > 0 && !string.IsNullOrEmpty(reportingToContext.PushTokenKey))
                                    {
                                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                        {
                                            PushTokenKey = reportingToContext.PushTokenKey,
                                            RegistrationTypeId = reportingToContext.RegistrationTypeId != null ? (int)reportingToContext.RegistrationTypeId : 0,
                                            Title = Constants.SpecialRateRequestCreation,
                                            Message = Constants.SpecialRateRequestCreationNotification.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, Math.Round(specialRate.Quantity, 2).ToString()).Replace(Constants.Price, Math.Round(specialRate.SpecialPrice, 2).ToString())
                                        };
                                        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    }
                                }
                            }

                        }
                    }

                    SpecialRatePricingHistory(todayPricings);
                }


                if (!errorFlag)
                {
                    return _resultService.ErrorMessage(errorMessageList);
                }
                else
                {
                    return _resultService.SuccessMessage(Constants.SpecialRateApprovalSuccess);
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetSpecialRateRequestList(SpecialRateInputDto specialRateInputDto)
        {
            var specialRateListDto = new List<SpecialRateResultDto>();
            _methodName = "GetSpecialRateRequestList";
            try
            {
                if (specialRateInputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                //Sales person
                if (specialRateInputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                if (specialRateInputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }
                if (specialRateInputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == specialRateInputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                IQueryable<UserCustomerMapping> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == specialRateInputDto.LoginUserId);

                IQueryable<SpecialRate> specialRateListContext;
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
                    divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == specialRateInputDto.LoginUserId)
                 .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });
                }
                var specialRateApproval = (from sp in _emamiContext.SpecialRateApproval.AsNoTracking()
                                           join s in _emamiContext.SpecialRate.AsNoTracking() on sp.SpecialRateId equals s.Id
                                           join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                                            equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                           where dealersList.Any(a => a.CustomerId == sp.CreatedBy) || sp.CreatedBy == specialRateInputDto.LoginUserId
                                           select sp
                                           );

                //var specialRateApproval = _emamiContext.SpecialRateApproval.AsNoTracking().Where(_ => _.RequestedTo == specialRateInputDto.LoginUserId || _.CreatedBy == specialRateInputDto.LoginUserId);

                List<long> specialRateIds = specialRateApproval.Select(_ => _.SpecialRateId).Distinct().ToList();

                //if (specialRateInputDto.DealerId != null && specialRateInputDto.OilTypeId != null && specialRateInputDto.FromDate.HasValue && specialRateInputDto.ToDate.HasValue)

                //specialRateListContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => _.CreatedDate >= specialRateInputDto.FromDate && _.CreatedDate <= specialRateInputDto.ToDate && specialRateIds.Contains(_.Id));

                //else if ((specialRateInputDto.DealerId != 0 && specialRateInputDto.DealerId != null) || (specialRateInputDto.OilTypeId != 0 && specialRateInputDto.OilTypeId != null)
                //    || (specialRateInputDto.FromDate.HasValue && specialRateInputDto.FromDate != DateTime.MinValue) || (specialRateInputDto.ToDate.HasValue && specialRateInputDto.ToDate != DateTime.MinValue))
                //{
                //    return _resultService.ErrorMessage(Constants.RecordNotFound);
                //}
                //else
                //{

                if (dealersList != null && dealersList.Any())
                {
                    specialRateListContext = _emamiContext.SpecialRate.AsNoTracking()
                    .Where(_ => dealersList.Any(a => a.CustomerId == _.UserId)
                    && DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(specialRateInputDto.FromDate)
                    && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(specialRateInputDto.ToDate)
                    && specialRateIds.Contains(_.Id));
                }
                else
                {
                    specialRateListContext = null;
                }
                //}
                if (specialRateListContext != null && specialRateListContext.Any())
                {
                    var cityContext = _emamiContext.City.AsNoTracking();
                    var stateContext = _emamiContext.State.AsNoTracking();
                    var specialRateList = specialRateListContext.Join(_emamiContext.Users.AsNoTracking(), sr => sr.UserId, u => u.Id, (sr, u) => new { sr, u })
                        .Join(_emamiContext.UserRoles.AsNoTracking(), sru => sru.u.Id, ur => ur.UserId, (sru, ur) => new { sru.sr, sru.u, ur }).Where(_ => _.sr != null && _.u != null && _.ur != null).ToList();

                    specialRateListDto = specialRateList.GroupBy(_ => _.sr.UserId)
                        .Select(s => new SpecialRateResultDto
                        {
                            DealerId = s.Key
                        }).ToList();

                    foreach (var dealer in specialRateListDto)
                    {
                        var specialRates = specialRateList.Where(_ => _.sr.UserId == dealer.DealerId).ToList();
                        foreach (var specialRateContext in specialRates)
                        {
                            dealer.DealerName = string.Concat((specialRateContext.sr.User != null ? specialRateContext.sr.User.Name : string.Empty) + "-" + (cityContext.FirstOrDefault(c => c.Id == specialRateContext.sr.User.CityId).CityName != null ? cityContext.FirstOrDefault(c => c.Id == specialRateContext.sr.User.CityId).CityName : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == specialRateContext.sr.User.StateId) != null ? stateContext.FirstOrDefault(s => s.Id == specialRateContext.sr.User.StateId).StateName : string.Empty) + "-" + (specialRateContext.sr.User != null ? specialRateContext.sr.User.Code : string.Empty));

                            var specialRateOutputDto = new SpecialRateOutputDto();
                            specialRateOutputDto.SpecialRateId = specialRateContext.sr.Id;
                            specialRateOutputDto.DealerId = specialRateContext.sr.UserId;
                            specialRateOutputDto.DealerName = string.Concat((specialRateContext.sr.User != null ? specialRateContext.sr.User.Name : string.Empty) + "-" + (cityContext.FirstOrDefault(c => c.Id == specialRateContext.sr.User.CityId).CityName != null ? cityContext.FirstOrDefault(c => c.Id == specialRateContext.sr.User.CityId).CityName : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == specialRateContext.sr.User.StateId) != null ? stateContext.FirstOrDefault(s => s.Id == specialRateContext.sr.User.StateId).StateName : string.Empty) + "-" + (specialRateContext.sr.User != null ? specialRateContext.sr.User.Code : string.Empty));
                            specialRateOutputDto.RequestDate = specialRateContext.sr.CreatedDate;
                            specialRateOutputDto.StatusId = specialRateContext.sr.StatusId;
                            specialRateOutputDto.StatusName = specialRateContext.sr.Status != null ? specialRateContext.sr.Status.Name : string.Empty;
                            specialRateOutputDto.IsBroker = specialRateContext.ur.RoleId == (int)DTO.Enums.Role.Broker ? true : false;
                            specialRateOutputDto.IsLTD = specialRateContext.sr.IsLTD;
                            var specialRateOilTypeListDto = new List<SpecialRateOilTypeDto>();

                            var specialRateOilTypeDto = new SpecialRateOilTypeDto();
                            specialRateOilTypeDto.OilTypeId = specialRateContext.sr.OilTypeId;
                            specialRateOilTypeDto.OilTypeName = specialRateContext.sr.OilType != null ? specialRateContext.sr.OilType.Name : string.Empty;
                            specialRateOilTypeDto.SkuCount = 1;
                            specialRateOilTypeDto.SkuId = specialRateContext.sr.SkuId;
                            specialRateOilTypeDto.SkuName = specialRateContext.sr.Sku != null ? specialRateContext.sr.Sku.SkuName : string.Empty;

                            specialRateOilTypeListDto.Add(specialRateOilTypeDto);
                            specialRateOutputDto.OilTypeList = specialRateOilTypeListDto;
                            dealer.SpecialRateList.Add(specialRateOutputDto);
                            dealer.SpecialRateList.OrderByDescending(_ => _.SpecialRateId).ToList();
                        }
                    }
                }
                if (specialRateListDto != null && specialRateListDto.Any())
                {
                    return _resultService.SuccessObject(specialRateListDto);
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
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetSpecialRateRequestDetails(SpecialRateDetailInputDto specialRateDetailInputDto)
        {
            _methodName = "GetSpecialRateRequestDetails";
            try
            {
                var specialRateDetailsDto = new SpecialRateDetailsDto();
                if (specialRateDetailInputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                //Sales person
                if (specialRateDetailInputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == specialRateDetailInputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id /*&& _.RoleId == (int)DTO.Enums.Role.StateTrader*/);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (specialRateDetailInputDto.DealerId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DealerMissing);
                }
                if (specialRateDetailInputDto.SpecialRateId == 0)
                {
                    return _resultService.ErrorMessage(Constants.SpecialRateRequestIdMissing);
                }
                //if (specialRateDetailInputDto.RequestDate == DateTime.MinValue)
                //{
                //    return _resultService.ErrorMessage(Constants.SpecialRateRequestDateMissing);
                //}
                //if (specialRateDetailInputDto.StatusId == 0)
                //{
                //    return _resultService.ErrorMessage(Constants.StatusMissing);
                //}

                //IQueryable<SpecialRate> specialRateListContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => _.UserId == specialRateDetailInputDto.DealerId
                //&& DbFunctions.TruncateTime(_.CreatedDate) == DbFunctions.TruncateTime(specialRateDetailInputDto.RequestDate) && _.StatusId == specialRateDetailInputDto.StatusId);
                IQueryable<SpecialRate> specialRateListContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => _.UserId == specialRateDetailInputDto.DealerId
                && _.Id == specialRateDetailInputDto.SpecialRateId);

                if (specialRateListContext != null && specialRateListContext.Any())
                {
                    var cityContext = _emamiContext.City.AsNoTracking();
                    var stateContext = _emamiContext.State.AsNoTracking();

                    SpecialRate specialRateDetailContext = specialRateListContext.FirstOrDefault();
                    specialRateDetailsDto.DealerId = specialRateDetailContext.UserId;
                    specialRateDetailsDto.DealerName = string.Concat((specialRateDetailContext.User != null ? specialRateDetailContext.User.Name : string.Empty) + "-" + (cityContext.FirstOrDefault(c => c.Id == specialRateDetailContext.User.CityId).CityName != null ? cityContext.FirstOrDefault(c => c.Id == specialRateDetailContext.User.CityId).CityName : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == specialRateDetailContext.User.StateId).StateName != null ? stateContext.FirstOrDefault(s => s.Id == specialRateDetailContext.User.StateId).StateName : string.Empty) + "-" + (specialRateDetailContext.User != null ? specialRateDetailContext.User.Code : string.Empty));
                    //specialRateDetailsDto.DealerCode = cityContext.FirstOrDefault(c => c.Id == specialRateDetailContext.User.CityId) != null ? cityContext.FirstOrDefault(c => c.Id == specialRateDetailContext.User.CityId).CityName : string.Empty + "-" + stateContext.FirstOrDefault(s => s.Id == specialRateDetailContext.User.StateId) != null ? stateContext.FirstOrDefault(s => s.Id == specialRateDetailContext.User.StateId).StateName : string.Empty + "-" + specialRateDetailContext.User != null ? specialRateDetailContext.User.Code : string.Empty;
                    specialRateDetailsDto.RequestDate = specialRateDetailContext.CreatedDate;
                    specialRateDetailsDto.Remarks = specialRateDetailContext.Remarks != null ? specialRateDetailContext.Remarks : string.Empty;
                    specialRateDetailsDto.SaudaLimitExceedRemarks = specialRateDetailContext.SaudaLimitExceedRemarks != null ? specialRateDetailContext.SaudaLimitExceedRemarks : string.Empty;
                    var specialRateApprovalContext = _emamiContext.SpecialRateApproval.AsNoTracking().Where(_ => _.SpecialRateId == specialRateDetailInputDto.SpecialRateId && _.SpecialRate != null).OrderByDescending(_ => _.Id).FirstOrDefault();
                    specialRateDetailsDto.IsAccessToApprove = (specialRateApprovalContext.RequestedTo == specialRateDetailInputDto.LoginUserId) ? true : false;
                    var statusContext = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == specialRateDetailContext.StatusId);
                    if (statusContext != null)
                    {
                        specialRateDetailsDto.Status = statusContext.Name;
                        specialRateDetailsDto.StatusId = statusContext.Id;
                    }
                    specialRateDetailsDto.SkuList = specialRateListContext.ToList().Select(_ => new SkuShortViewOutputDto
                    {
                        SpecialRateId = _.Id,
                        SkuId = _.SkuId,
                        SkuName = _.Sku != null ? _.Sku.SkuName : string.Empty,
                        Quantity = _.Quantity,
                        QuantityCase = _.QuantityCase,
                        FinalPrice = _.FinalPrice,
                        SpecialPrice = _.SpecialPrice,
                        IncotermsName = _.Incoterms1,
                        IsRake = (_.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake || _.Incoterms2 == (int)DTO.Enums.IncoTerms.ForRake) ? true : false,
                        //DealerLocationName = _.FreightRoute != null ? _.FreightRoute.Name : string.Empty,
                        PlantName = _.Depot != null ? _.Depot.Name : string.Empty,
                        CaseToMetricTonValue = _resultService.ConvertCasetoMetricTon(1, _.SkuId),
                        IsLTD = _.IsLTD,
                    }).ToList();
                }

                if (specialRateDetailsDto != null && specialRateDetailsDto.SkuList != null && specialRateDetailsDto.SkuList.Any())
                {
                    return _resultService.SuccessObject(specialRateDetailsDto);
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
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }



        /// Method to create sauda from special Rate
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        public ResultDto SaudaCreationFromSpecialRate(SpecialRateSaudaDto inputDto)
        {
            _methodName = "SaudaCreationFromSpecialRate";
            var resultDto = new ResultDto();
            try
            {

                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                //var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                //if (userRoleContext == null)
                //{
                //    return _resultService.ErrorMessage(Constants.UserNotFound);
                //}

                var dealerContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == inputDto.DealerId);

                if (dealerContext == null)
                {
                    return _resultService.ErrorMessage(Constants.DealerNotFound);
                }

                if (inputDto.SpecialRateIdInfo == null || !inputDto.SpecialRateIdInfo.Any())
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
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
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var SplrateIdInfo = inputDto.SpecialRateIdInfo.ToList();
                var SpecialRatelist = _emamiContext.SpecialRate.Where(_ => _.StatusId == (int)DTO.Enums.Status.Approved).ToList();
                var specialRateListContext = SpecialRatelist
                                             .Join(SplrateIdInfo, sr => sr.Id, srId => srId.SpecialRateIds, (sr, srId) => new { sr, srId })
                                            .ToList();

                if (specialRateListContext == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }
                var userdivContext = _emamiContext.UserDivisionMappings.AsNoTracking()
                   .FirstOrDefault(_ => _.UserId == inputDto.DealerId
                   && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId
                   && _.DivisionId == inputDto.DivisionId);

                decimal overallSaudaLimit = 0;
                decimal orderedQuantity = 0;
                decimal liftingQuantity = 0;
                decimal availableQuantity = 0;

                overallSaudaLimit = userdivContext.SaudaLimit ?? 0;

                //IQueryable<SaudaOrder> saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.Sauda.UserId == dealerContext.Id
                //    && (_.StatusId == (int)DTO.Enums.Status.Pending || _.StatusId == (int)DTO.Enums.Status.Approved ));
                IQueryable<SaudaOrder> saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.Sauda.UserId == dealerContext.Id
                    && (_.SaudaNumber == null) && _.StatusId == (int)DTO.Enums.Status.Pending);

                var QuantityLimitForBookingSaudaName = Utility.GetEnumDescription(DTO.Enums.Configuration.QuantityLimitforBookingSaudaEnabled);
                var configurationContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Name == QuantityLimitForBookingSaudaName);
                bool IsQuantityLimitForBookingSauda = Convert.ToBoolean(configurationContext.Value);
                var overallSaudaStatuses = Constants.OverallSaudaStatus;
                int i = 0;
                foreach (var item in specialRateListContext)
                {
                    if (saudaOrderListContext != null && saudaOrderListContext.Any())
                    {
                        var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == item.sr.SkuId);
                        if (skuContext != null && (skuContext.DivisionId == (int)DTO.Enums.Division.SpecialityFat || skuContext.DivisionId == (int)DTO.Enums.Division.Hbc))
                        {

                            decimal availableQuantityBdo = 0;

                            if (configurationContext != null && IsQuantityLimitForBookingSauda)
                            {
                                var bdoLimitContext = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                                .FirstOrDefault(_ => _.UserId == inputDto.LoginUserId
                                && _.SkuId == item.sr.SkuId
                                && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
                                && DbFunctions.TruncateTime(_.ValidTo) >= DbFunctions.TruncateTime(currentDate));
                                if (bdoLimitContext != null)
                                {
                                    IQueryable<SaudaOrder> saudaOrdersBdoContext = null;
                                    List<long> dealerList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                                        .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.u.Id, uc => uc.CustomerId, (x, uc) => new { DealerId = x.u.Id, x.ur, uc })
                                        .Where(_ => _.uc.UserId == inputDto.LoginUserId && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.DealerId).ToList();
                                    if (dealerList != null && dealerList.Any())
                                    {
                                        saudaOrdersBdoContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.SkuId == item.sr.SkuId && dealerList.Contains(_.Sauda.UserId)
                                              && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(bdoLimitContext.ValidFrom)
                                              && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(bdoLimitContext.ValidTo) && overallSaudaStatuses.Contains(_.StatusId) && _.IsQuantityLimitForBookingSauda);
                                    }
                                    decimal requestedQuantityBdo = _resultService.ConvertCasetoMetricTon(item.srId.QuantityInCases, item.sr.SkuId);
                                    decimal orderedQuantityBdo = 0;
                                    decimal totalQuantityBdo = requestedQuantityBdo;
                                    if (saudaOrdersBdoContext != null && saudaOrdersBdoContext.Any())
                                    {
                                        orderedQuantityBdo = saudaOrdersBdoContext.Sum(_ => _.BidQuantity);
                                        totalQuantityBdo = orderedQuantityBdo + requestedQuantityBdo;
                                    }
                                    if (totalQuantityBdo > bdoLimitContext.ActualDiscount)
                                    {
                                        //bdoErrorFlag = true;
                                        //return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName));
                                        availableQuantityBdo = bdoLimitContext.ActualDiscount - orderedQuantityBdo;
                                        if (availableQuantityBdo < 0)
                                        {
                                            availableQuantityBdo = 0;
                                            //return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName));
                                        }
                                        return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, Math.Round(availableQuantityBdo, 2).ToString()));
                                        //if (availableQuantityBdo >= 0)
                                        //{
                                        //    return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, availableQuantityBdo.ToString()));
                                        //}
                                        //else
                                        //{
                                        //    return _resultService.ErrorMessage(Constants.SkuBdoLimitReached.Replace(Constants.SkuName, skuContext.SkuName));
                                        //}
                                    }
                                }
                                else
                                {
                                    return _resultService.ErrorMessage(Constants.BDOLimitNotExists);
                                }
                            }

                        }
                        decimal invoiceQuantity = 0;
                        var existingSaudaQuantity = saudaOrderListContext.Sum(_ => _.BidQuantity);
                        var skuIds = saudaOrderListContext.Select(_ => _.SkuId).Distinct().ToList();

                        var saudaLimitTableValue = _emamiContext.SaudaLimit.AsNoTracking().FirstOrDefault(_ => _.UserId == dealerContext.Id);
                        var saudaLimitTableValueTotal = saudaLimitTableValue != null ? (saudaLimitTableValue.PendingContract + saudaLimitTableValue.PendingDO + saudaLimitTableValue.PendingOBD) : 0;
                        availableQuantity = overallSaudaLimit - saudaLimitTableValueTotal - existingSaudaQuantity;

                        if (availableQuantity < specialRateListContext.Where(_ => _.sr.UserId == dealerContext.Id && _.sr.Id == item.srId.SpecialRateIds).Sum(_ => _resultService.ConvertCasetoMetricTon(_.srId.QuantityInCases, _.sr.SkuId)))
                        {
                            return _resultService.ErrorMessage(Constants.SaudaLimitExceeds);
                        }
                    }
                    else
                    {
                        var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == item.sr.SkuId);
                        if (skuContext != null && (skuContext.DivisionId == (int)DTO.Enums.Division.SpecialityFat || skuContext.DivisionId == (int)DTO.Enums.Division.Hbc))
                        {

                            decimal availableQuantityBdo = 0;

                            if (configurationContext != null && IsQuantityLimitForBookingSauda)
                            {
                                var bdoLimitContext = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                                .FirstOrDefault(_ => _.UserId == inputDto.LoginUserId
                                && _.SkuId == item.sr.SkuId
                                && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
                                && DbFunctions.TruncateTime(_.ValidTo) >= DbFunctions.TruncateTime(currentDate));
                                if (bdoLimitContext != null)
                                {
                                    IQueryable<SaudaOrder> saudaOrdersBdoContext = null;
                                    List<long> dealerList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                                        .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.u.Id, uc => uc.CustomerId, (x, uc) => new { DealerId = x.u.Id, x.ur, uc })
                                        .Where(_ => _.uc.UserId == inputDto.LoginUserId && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.DealerId).ToList();
                                    if (dealerList != null && dealerList.Any())
                                    {
                                        saudaOrdersBdoContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.SkuId == item.sr.SkuId && dealerList.Contains(_.Sauda.UserId)
                                              && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(bdoLimitContext.ValidFrom)
                                              && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(bdoLimitContext.ValidTo) && overallSaudaStatuses.Contains(_.StatusId) && _.IsQuantityLimitForBookingSauda);
                                    }
                                    decimal requestedQuantityBdo = _resultService.ConvertCasetoMetricTon(item.srId.QuantityInCases, item.sr.SkuId);
                                    decimal orderedQuantityBdo = 0;
                                    decimal totalQuantityBdo = requestedQuantityBdo;
                                    if (saudaOrdersBdoContext != null && saudaOrdersBdoContext.Any())
                                    {
                                        orderedQuantityBdo = saudaOrdersBdoContext.Sum(_ => _.BidQuantity);
                                        totalQuantityBdo = orderedQuantityBdo + requestedQuantityBdo;
                                    }
                                    if (totalQuantityBdo > bdoLimitContext.ActualDiscount)
                                    {
                                        //bdoErrorFlag = true;
                                        //return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName));
                                        availableQuantityBdo = bdoLimitContext.ActualDiscount - orderedQuantityBdo;
                                        if (availableQuantityBdo < 0)
                                        {
                                            availableQuantityBdo = 0;
                                            //return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName));
                                        }
                                        return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, Math.Round(availableQuantityBdo, 2).ToString()));
                                        //if (availableQuantityBdo >= 0)
                                        //{
                                        //    return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, availableQuantityBdo.ToString()));
                                        //}
                                        //else
                                        //{
                                        //    return _resultService.ErrorMessage(Constants.SkuBdoLimitReached.Replace(Constants.SkuName, skuContext.SkuName));
                                        //}
                                    }
                                }
                                else
                                {
                                    return _resultService.ErrorMessage(Constants.BDOLimitNotExists);
                                }
                            }
                        }
                        decimal invoiceQuantity = 0;
                        //var existingSaudaQuantity = saudaOrderListContext.Sum(_ => _.BidQuantity);
                        //var skuIds = saudaOrderListContext.Select(_ => _.SkuId).Distinct().ToList();

                        var saudaLimitTableValue = _emamiContext.SaudaLimit.AsNoTracking().FirstOrDefault(_ => _.UserId == dealerContext.Id);
                        var saudaLimitTableValueTotal = saudaLimitTableValue != null ? (saudaLimitTableValue.PendingContract + saudaLimitTableValue.PendingDO + saudaLimitTableValue.PendingOBD) : 0;
                        availableQuantity = overallSaudaLimit - saudaLimitTableValueTotal /*- existingSaudaQuantity*/;

                        if (availableQuantity < specialRateListContext.Where(_ => _.sr.UserId == dealerContext.Id && _.sr.Id == item.srId.SpecialRateIds).Sum(_ => _resultService.ConvertCasetoMetricTon(_.srId.QuantityInCases, _.sr.SkuId)))
                        {
                            return _resultService.ErrorMessage(Constants.SaudaLimitExceeds);
                        }
                    }

                    long BrokerId = 0;
                    var dealerRole = _emamiContext.UserRoles.FirstOrDefault(_ => _.UserId == inputDto.DealerId);
                    if (dealerRole != null)
                    {
                        if (dealerRole.RoleId == (int)DTO.Enums.Role.Broker)
                        {
                            BrokerId = inputDto.DealerId;
                        }
                        else
                        {
                            var BrokerContext = (from ucm in _emamiContext.UserCustomerMapping
                                                 join ur in _emamiContext.UserRoles on ucm.UserId equals ur.UserId
                                                 where ur.RoleId == (int)DTO.Enums.Role.Broker
                                                 && ucm.CustomerId == inputDto.DealerId
                                                 select new
                                                 {
                                                     BrokerId = ucm.UserId
                                                 }).FirstOrDefault();

                            if (BrokerContext != null)
                            {
                                BrokerId = BrokerContext.BrokerId;
                            }
                        }
                    }

                    var saudaContext = new Sauda();
                    if (specialRateListContext != null && specialRateListContext.Any())
                    {
                        saudaContext = new Sauda
                        {

                            BiddingDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            UserId = inputDto.DealerId,

                            SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,

                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            IsSAPDataSync = false,
                            IsSAPDataSyncApproval = false

                        };

                        _emamiContext.Sauda.Add(saudaContext);
                        _emamiContext.SaveChanges();

                        List<long> saudaOrderIds = new List<long>();
                        //  foreach (var item in specialRateListContext)
                        //{
                        DateTime? saudaValidFromDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        long? depotIdForRake = 0;
                        if (item.sr.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake || item.sr.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake)
                        {
                            depotIdForRake = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == item.sr.DepotId && !_.IsPlant)?.DepotId;
                            if (item.srId.SaudaValidFromDate != null)
                                saudaValidFromDate = item.srId.SaudaValidFromDate;

                        }

                        #region PricingLive to Pricing DataInsert and Rearrange the Pricing Data
                        ///Pricing Live is contain Current day Pricing
                        ///So, we insert the Pricing Live data into Pricing table for Sauda booked records only
                        /// Daily we cleanup and fresh data insert into the pricing live table
                        var pricingLiveContext = _emamiContext.TodayPricing.FirstOrDefault(_ => _.Id == item.sr.PricingId);
                        //var pricingContext = default(Pricing);
                        long pricingId = 0;
                        if (pricingLiveContext == null)
                        {
                            return _resultService.ErrorMessage(Constants.PricingIdisnotValid);
                        }
                        if (pricingLiveContext.PricingReferneceId == 0)
                        {
                            var pricing = new Pricing()
                            {
                                SkuId = pricingLiveContext.SkuId,
                                OilTypeId = pricingLiveContext.OilTypeId,
                                OilPackingTypeId = pricingLiveContext.OilPackingTypeId,
                                PlantId = pricingLiveContext.PlantId,
                                Price = pricingLiveContext.Price,
                                SalesOrganizationId = pricingLiveContext.SalesOrganizationId,
                                DistributionChannelId = pricingLiveContext.DistributionChannelId,
                                DivisionId = pricingLiveContext.DivisionId,
                                SAPPricingCode = pricingLiveContext.SAPPricingCode,
                                ValidFrom = pricingLiveContext.ValidFrom,
                                ValidTo = pricingLiveContext.ValidTo,
                                CreatedBy = pricingLiveContext.CreatedBy,
                                CreatedDate = pricingLiveContext.CreatedDate,
                                ModifiedBy = pricingLiveContext.ModifiedBy,
                                ModifiedDate = pricingLiveContext.ModifiedDate,
                            };
                            _emamiContext.Pricing.Add(pricing);
                            _emamiContext.SaveChanges();
                            pricingId = pricing.Id;
                            pricingLiveContext.PricingReferneceId = pricing.Id;
                            _emamiContext.SaveChanges();
                        }
                        else
                        {
                            pricingId = pricingLiveContext.PricingReferneceId;
                        }
                        item.sr.PricingId = pricingId;
                        #endregion

                        i = i + 10;
                        var saudaOrder = new SaudaOrder
                        {
                            SaudaId = saudaContext.Id,
                            SaudaNumber = i.ToString(),
                            SkuId = item.sr.SkuId,
                            OilTypeId = item.sr.OilTypeId,
                            BidPrice = (item.sr.SpecialPrice * item.srId.QuantityInCases),
                            BidQuantity = _resultService.ConvertCasetoMetricTon(item.srId.QuantityInCases, item.sr.SkuId),
                            BidQuantityCase = item.srId.QuantityInCases,
                            QuotedPrice = (item.sr.FinalPrice * item.sr.QuantityCase),
                            //BidPriceForDailyReport = (item.sr.SpecialPrice * item.srId.QuantityInCases),
                            //BidQuantityForDailyReport = _resultService.ConvertCasetoMetricTon(item.srId.QuantityInCases, item.sr.SkuId),
                            //BidQuantityCaseForDailyReport = item.srId.QuantityInCases,
                            //QuotedPriceForDailyReport = (item.sr.FinalPrice * item.sr.QuantityCase),
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            PricingId = item.sr.PricingId,
                            // DealerTypeId = (int)DTO.Enums.DealerType.Direct,
                            SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                            Incoterms1 = item.sr.Incoterms1,
                            PlantId = item.sr.DepotId,
                            //CustomerPONumber = dealerContext.Code + DateHelper.UtcToIndia(DateTime.UtcNow),
                            //CustomerPONumberForDailyReport = dealerContext.Code + DateHelper.UtcToIndia(DateTime.UtcNow),
                            ValidFromDate = saudaValidFromDate.Value,
                            ValidToDate = saudaValidFromDate.Value.AddDays(Convert.ToDouble(dealerContext.SaudaValidityPeriod > 0 ? dealerContext.SaudaValidityPeriod : Config.DefaultSaudaValidity)),
                            StatusId = (int)DTO.Enums.Status.Pending,
                            //SaudaStatusId = (int)DTO.Enums.SaudaStatus.NotReleased,
                            //StatusIdForDailyReport = (int)DTO.Enums.Status.Pending,
                            //SaudaStatusIdForDailyReport = (int)DTO.Enums.SaudaStatus.NotReleased,
                            Incoterms2 = item.sr.Incoterms2,
                            BrokerId = BrokerId,
                            // BrokerIdForDailyReport = BrokerId,
                            SpecialRateRequestId = item.sr.Id,
                            IsSAPDataSync = false,
                            IsSAPDataSyncApproval = false,
                            //DepotIdForRake = depotIdForRake.Value,
                            IsQuantityLimitForBookingSauda = IsQuantityLimitForBookingSauda,
                            QuotedPriceBeforeSAPDiscount = item.sr.SpecialPrice
                        };
                        _emamiContext.SaudaOrders.Add(saudaOrder);
                        _emamiContext.SaveChanges();

                        //if (dealerContext.DivisionId == (int)DTO.Enums.LooseVertical.Loose)
                        //{
                        //    saudaOrderIds.Add(saudaOrder.Id);
                        //}

                        try
                        {
                            var usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == inputDto.LoginUserId || _.Id == inputDto.DealerId);
                            var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrder.Id);
                            if (usersContext != null && usersContext.Any() && saudaOrderContext != null)
                            {
                                List<string> toUsers = new List<string>();
                                var createdBy = usersContext.FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                                var dealer = usersContext.FirstOrDefault(_ => _.Id == inputDto.DealerId);
                                string dealerName = string.Empty;
                                if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
                                {
                                    toUsers.Add(createdBy.Email);
                                }
                                if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                                {
                                    dealerName = dealer.Name;
                                    toUsers.Add(dealer.Email);
                                }
                                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                if (_resultService.IsEmail() && toUsers != null && toUsers.Any())
                                {
                                    var fromEmail = Constants.FromEmail;
                                    var emailSubject = Constants.SaudaBookedSubject;
                                    var plainText = string.Empty;
                                    var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationEmail);
                                    if (emailTemplate != null)
                                    {
                                        var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
                                            .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round((saudaOrderContext.BidPrice / saudaOrderContext.BidQuantityCase), 2)).ToString())
                                            .Replace(Constants.BY_FOR, Constants.FOR).Replace(Constants.UserName, dealerName);
                                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                        amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                    }

                                }
                                var smsPlainTemplate = string.Empty;
                                if (_resultService.IsSMS())
                                {
                                    var smsMessage = string.Empty;
                                    EmailTemplate smsTemplate = new EmailTemplate();
                                    smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationSMS);
                                    if (smsTemplate != null)
                                    {
                                        smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
                                            .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round((saudaOrderContext.BidPrice / saudaOrderContext.BidQuantityCase), 2)).ToString());
                                        smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                        if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                                        {
                                            amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber);
                                        }
                                        if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                        {
                                            amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber);
                                        }
                                    }
                                }
                                //if (_resultService.IsPushNotification())
                                //{
                                //    if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
                                //    {
                                //        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                //        {
                                //            PushTokenKey = createdBy.PushTokenKey,
                                //            RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
                                //            Title = Constants.SaudaCreationSubject,
                                //            Message = smsPlainTemplate,
                                //            //Id = saudaOrderContext.Id,
                                //        };
                                //        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                //    }
                                //    if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
                                //    {
                                //        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                //        {
                                //            PushTokenKey = dealer.PushTokenKey,
                                //            RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
                                //            Title = Constants.SaudaCreationSubject,
                                //            Message = smsPlainTemplate,
                                //            //Id = saudaOrderContext.Id,
                                //        };
                                //        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                //    }
                                //}
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        // }

                        specialRateListContext.ForEach(_ => _.sr.StatusId = (int)DTO.Enums.Status.Completed);
                        _emamiContext.SaveChanges();

                        //if (dealerContext.VerticalId == (int)DTO.Enums.LooseVertical.Loose)
                        //{
                        //    //method to sync Loose sauda from APP to SAP 
                        //    _sapIntegrationService.GetSaudaDetailsForLooseVertical(saudaOrderIds);
                        //}
                    }
                    else
                    {
                        return _resultService.ErrorMessage(Constants.SpecialRateNotFoundWithApproval);
                    }
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = saudaContext.Id;
                }

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

        public ResultDto SpecialRateApproveOrReject(SpecialRateSaudaDto inputDto)
        {
            _methodName = "SpecialRateApproveOrReject";
            var resultDto = new ResultDto();
            try
            {
                //var todayPricings = new List<long>();
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                //var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                //if (userRoleContext == null)
                //{
                //    return _resultService.ErrorMessage(Constants.UserNotFound);
                //}

                if (inputDto.SpecialRateIdInfo == null || !inputDto.SpecialRateIdInfo.Any())
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                //var SplrateIdInfo = inputDto.SpecialRateIdInfo.ToList();
                //var SpecialRatelist = _emamiContext.SpecialRate.Where(_ => _.StatusId == (int)DTO.Enums.Status.Approved).ToList();
                //var specialRateListContext = SpecialRatelist
                //                             .Join(SplrateIdInfo, sr => sr.Id, srId => srId.SpecialRateIds, (sr, srId) => new { sr, srId })
                //                            .ToList();

                //if (specialRateListContext == null)
                //{
                //    return _resultService.ErrorMessage(Constants.RecordNotFound);
                //}



                decimal overallSaudaLimit = 0;
                decimal orderedQuantity = 0;
                decimal liftingQuantity = 0;
                decimal availableQuantity = 0;

                int i = 0;
                foreach (var item in inputDto.SpecialRateIdInfo)
                {

                    var specialContext = _emamiContext.SpecialRate.FirstOrDefault(_ => _.Id == item.SpecialRateIds);



                    var specialRateapprovalContext = _emamiContext.SpecialRateApproval.FirstOrDefault(_ => _.SpecialRateId == item.SpecialRateIds);
                    if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                    {
                        //special rate approval
                        specialRateapprovalContext.StatusId = inputDto.StatusId;
                        specialRateapprovalContext.Remarks = inputDto.Remarks;
                        specialRateapprovalContext.ModifiedBy = inputDto.LoginUserId;
                        specialRateapprovalContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        //special rate 
                        specialContext.StatusId = inputDto.StatusId;
                        specialContext.Remarks = inputDto.Remarks;
                        specialContext.ModifiedBy = inputDto.LoginUserId;
                        specialContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                        var reportingTo = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == specialRateapprovalContext.RequestedBy);
                        if (_resultService.IsPushNotification())
                        {
                            if (reportingTo != null && reportingTo.RegistrationTypeId != null && reportingTo.RegistrationTypeId > 0 && !string.IsNullOrEmpty(reportingTo.PushTokenKey))
                            {
                                PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                {
                                    PushTokenKey = reportingTo.PushTokenKey,
                                    RegistrationTypeId = reportingTo.RegistrationTypeId != null ? (int)reportingTo.RegistrationTypeId : 0,
                                    Title = Constants.SpecialRateRejectSubject,
                                    Message = Constants.SpecialRateRejectSubject,
                                };
                                _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                            }
                        }
                    }
                    else if (inputDto.StatusId == (int)DTO.Enums.Status.RequestForApproval)
                    {
                        //special rate approval
                        specialRateapprovalContext.StatusId = inputDto.StatusId;
                        // specialRateapprovalContext.Remarks = inputDto.Remarks;
                        specialRateapprovalContext.ModifiedBy = inputDto.LoginUserId;
                        specialRateapprovalContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        //special rate 
                        specialContext.StatusId = inputDto.StatusId;
                        // specialContext.Remarks = inputDto.Remarks;
                        specialContext.ModifiedBy = inputDto.LoginUserId;
                        specialContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                        //var reportingToId = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == specialRateapprovalContext.RequestedTo).ReportingToId;
                        var reportingToId = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.UserId == specialRateapprovalContext.RequestedTo).Select(_ => _.ReportingToUserId).ToList();
                        var reportingToList = _emamiContext.Users.AsNoTracking().Where(_ => reportingToId.Contains(_.Id)).ToList();
                        if (_resultService.IsPushNotification())
                        {
                            if (reportingToList != null && reportingToList.Any())
                            {
                                foreach (var reportingTo in reportingToList)
                                {
                                    if (reportingTo != null && reportingTo.RegistrationTypeId != null && reportingTo.RegistrationTypeId > 0 && !string.IsNullOrEmpty(reportingTo.PushTokenKey))
                                    {
                                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                        {
                                            PushTokenKey = reportingTo.PushTokenKey,
                                            RegistrationTypeId = reportingTo.RegistrationTypeId != null ? (int)reportingTo.RegistrationTypeId : 0,
                                            Title = Constants.SpecialRateApprovalSubject,
                                            Message = Constants.SpecialRateRequestForApprovalMessage,
                                            //Id = saudaOrderContext.Id,
                                        };
                                        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    }
                                }
                            }

                        }
                    }

                    _emamiContext.SaveChanges();

                    if ((specialContext.StatusId == (int)DTO.Enums.Status.Pending || specialContext.StatusId == (int)DTO.Enums.Status.RequestForApproval) && inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                    {
                        var dealerContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == specialContext.UserId);

                        if (dealerContext == null)
                        {
                            return _resultService.ErrorMessage(Constants.DealerNotFound);
                        }

                        //special rate approval
                        specialRateapprovalContext.StatusId = inputDto.StatusId;
                        specialRateapprovalContext.ModifiedBy = inputDto.LoginUserId;
                        specialRateapprovalContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                        //special rate 
                        specialContext.StatusId = inputDto.StatusId;
                        specialContext.ModifiedBy = inputDto.LoginUserId;
                        specialContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);


                        _emamiContext.SaveChanges();
                        var userdivContext = _emamiContext.UserDivisionMappings.AsNoTracking()
                            .FirstOrDefault(_ => _.UserId == item.DealerId
                            && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId
                            && _.DivisionId == inputDto.DivisionId);

                        overallSaudaLimit = userdivContext.SaudaLimit ?? 0;

                        //IQueryable<SaudaOrder> saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.Sauda.UserId == dealerContext.Id
                        //    && (_.StatusId == (int)DTO.Enums.Status.Pending || _.StatusId == (int)DTO.Enums.Status.Approved ));
                        //IQueryable<SaudaOrder> saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.Sauda.UserId == dealerContext.Id
                        //    && (_.Sauda.SaudaNumber == null) && _.Sauda.StatusId == (int)DTO.Enums.Status.Pending);

                        var QuantityLimitForBookingSaudaName = Utility.GetEnumDescription(DTO.Enums.Configuration.QuantityLimitforBookingSaudaEnabled);
                        var configurationContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Name == QuantityLimitForBookingSaudaName);
                        bool IsQuantityLimitForBookingSauda = Convert.ToBoolean(configurationContext.Value);
                        var overallSaudaStatuses = Constants.OverallSaudaStatus;
                        //if (saudaOrderListContext != null && saudaOrderListContext.Any())
                        //{

                        var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == specialContext.SkuId);
                        if (skuContext != null /*&& (skuContext.DivisionId == (int)DTO.Enums.Division.SpecialityFat || skuContext.DivisionId == (int)DTO.Enums.Division.Hbc)*/)
                        {
                            decimal availableQuantityBdo = 0;
                            if (configurationContext != null && IsQuantityLimitForBookingSauda)
                            {
                                var bdoLimitContext = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                                .FirstOrDefault(_ => _.UserId == inputDto.LoginUserId
                                && _.SkuId == specialContext.SkuId
                                && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
                                && DbFunctions.TruncateTime(_.ValidTo) >= DbFunctions.TruncateTime(currentDate));
                                if (bdoLimitContext != null)
                                {
                                    IQueryable<SaudaOrder> saudaOrdersBdoContext = null;
                                    List<long> dealerList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                                        .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.u.Id, uc => uc.CustomerId, (x, uc) => new { DealerId = x.u.Id, x.ur, uc })
                                        .Where(_ => _.uc.UserId == inputDto.LoginUserId && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.DealerId).ToList();
                                    if (dealerList != null && dealerList.Any())
                                    {
                                        saudaOrdersBdoContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.SkuId == specialContext.SkuId && dealerList.Contains(_.Sauda.UserId)
                                              && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(bdoLimitContext.ValidFrom)
                                              && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(bdoLimitContext.ValidTo) && overallSaudaStatuses.Contains(_.StatusId) && _.IsQuantityLimitForBookingSauda);
                                    }
                                    decimal requestedQuantityBdo = _resultService.ConvertCasetoMetricTon(specialContext.QuantityCase, specialContext.SkuId);
                                    decimal orderedQuantityBdo = 0;
                                    decimal totalQuantityBdo = requestedQuantityBdo;
                                    if (saudaOrdersBdoContext != null && saudaOrdersBdoContext.Any())
                                    {
                                        orderedQuantityBdo = saudaOrdersBdoContext.Sum(_ => _.BidQuantity);
                                        totalQuantityBdo = orderedQuantityBdo + requestedQuantityBdo;
                                    }
                                    if (totalQuantityBdo > bdoLimitContext.ActualDiscount)
                                    {
                                        //bdoErrorFlag = true;
                                        //return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName));
                                        availableQuantityBdo = bdoLimitContext.ActualDiscount - orderedQuantityBdo;
                                        if (availableQuantityBdo < 0)
                                        {
                                            availableQuantityBdo = 0;
                                            //return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName));
                                        }
                                        specialContext.SaudaLimitExceedRemarks = Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, Math.Round(availableQuantityBdo, 2).ToString());
                                        specialContext.StatusId = (int)DTO.Enums.Status.Pending;
                                        specialContext.ModifiedBy = inputDto.LoginUserId;
                                        specialContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                        _emamiContext.SaveChanges();
                                        return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, Math.Round(availableQuantityBdo, 2).ToString()));
                                        //if (availableQuantityBdo >= 0)
                                        //{
                                        //    return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, availableQuantityBdo.ToString()));
                                        //}
                                        //else
                                        //{
                                        //    return _resultService.ErrorMessage(Constants.SkuBdoLimitReached.Replace(Constants.SkuName, skuContext.SkuName));
                                        //}
                                    }
                                }
                                else
                                {
                                    specialContext.SaudaLimitExceedRemarks = Constants.BDOLimitNotExists;
                                    specialContext.StatusId = (int)DTO.Enums.Status.Pending;
                                    specialContext.ModifiedBy = inputDto.LoginUserId;
                                    specialContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                    _emamiContext.SaveChanges();
                                    return _resultService.ErrorMessage(Constants.BDOLimitNotExists);
                                }
                            }
                        }

                        //decimal invoiceQuantity = 0;
                        //var existingSaudaQuantity = saudaOrderListContext.Sum(_ => _.BidQuantity);
                        //var skuIds = saudaOrderListContext.Select(_ => _.SkuId).Distinct().ToList();

                        // var pendingContracttablevalue = _emamiContext.PendingContracts.AsNoTracking().Where(_ => _.UserId == inputDto.DealerId && _.SalesOrgId == inputDto.SalesOrganizationId && _.DistChnlId == inputDto.DistributionChannelId && _.DivisionId == inputDto.DivisionId).ToList().IsAny() ? _emamiContext.PendingContracts.AsNoTracking().Where(_ => _.UserId == inputDto.DealerId && _.SalesOrgId == inputDto.SalesOrganizationId && _.DistChnlId == inputDto.DistributionChannelId && _.DivisionId == inputDto.DivisionId).Select(_ => _.SaudaQuantity).Sum() : 0;                           
                        availableQuantity = _resultService.AvailableSaudaLimit(item.DealerId, overallSaudaLimit, inputDto.SalesOrganizationId, inputDto.DistributionChannelId, inputDto.DivisionId);


                        var specialrateAfterApproveContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => _.StatusId == (int)DTO.Enums.Status.Approved).ToList();
                        if (availableQuantity < specialrateAfterApproveContext.Where(_ => _.UserId == dealerContext.Id && _.Id == item.SpecialRateIds).Sum(_ => _resultService.ConvertCasetoMetricTon(_.QuantityCase, _.SkuId)))
                        {
                            specialContext.SaudaLimitExceedRemarks = Constants.SaudaLimitExceeds + " for " + dealerContext.Name;
                            specialContext.StatusId = (int)DTO.Enums.Status.Pending;
                            specialContext.ModifiedBy = inputDto.LoginUserId;
                            specialContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                            _emamiContext.SaveChanges();
                            return _resultService.ErrorMessage(specialContext.SaudaLimitExceedRemarks);
                        }
                        //}


                        long BrokerId = 0;
                        var dealerRole = _emamiContext.UserRoles.FirstOrDefault(_ => _.UserId == specialContext.UserId);
                        if (dealerRole != null)
                        {
                            if (dealerRole.RoleId == (int)DTO.Enums.Role.Broker)
                            {
                                BrokerId = specialContext.UserId;
                            }
                            else
                            {
                                var BrokerContext = (from ucm in _emamiContext.UserCustomerMapping
                                                     join ur in _emamiContext.UserRoles on ucm.UserId equals ur.UserId
                                                     where ur.RoleId == (int)DTO.Enums.Role.Broker
                                                     && ucm.CustomerId == specialContext.UserId
                                                     select new
                                                     {
                                                         BrokerId = ucm.UserId
                                                     }).FirstOrDefault();

                                if (BrokerContext != null)
                                {
                                    BrokerId = BrokerContext.BrokerId;
                                }
                            }
                        }
                        var divisionContext = _emamiContext.Divisions.FirstOrDefault(_ => _.Id == inputDto.DivisionId);

                        var saudaContext = new Sauda();

                        saudaContext = new Sauda
                        {
                            BiddingDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            UserId = specialContext.UserId,
                            StatusId = (int)DTO.Enums.Status.Approved,
                            SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                            SalesOrganizationId = inputDto.SalesOrganizationId,
                            SalesDocumentType = divisionContext != null ? divisionContext.SalesDocumentType : string.Empty,
                            DistributionChannelId = inputDto.DistributionChannelId,
                            DivisionId = inputDto.DivisionId,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            IsSAPDataSync = false,
                            IsSAPDataSyncApproval = false,
                            SpecialRateRequestIdInParentTable = specialContext.Id,
                            BdoId = specialContext.CreatedBy
                        };

                        _emamiContext.Sauda.Add(saudaContext);
                        _emamiContext.SaveChanges();

                        ////Sauda approval save
                        //var saudaapprovalContext = new SaudaApproval
                        //{
                        //    RequestedBy = inputDto.LoginUserId,
                        //    RequestedTo = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId).ReportingToId ?? 0,
                        //    CreatedBy = inputDto.LoginUserId,
                        //    CreatedDate = currentDate,
                        //    StatusId = (int)DTO.Enums.Status.Pending,
                        //    SaudaId = saudaContext.Id
                        //};
                        //_emamiContext.SaudaApproval.Add(saudaapprovalContext);
                        //_emamiContext.SaveChanges();

                        List<long> saudaOrderIds = new List<long>();

                        DateTime? saudaValidFromDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        long? depotIdForRake = 0;
                        //if (specialContext.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake || specialContext.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake)
                        //{
                        //    depotIdForRake = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == specialContext.DepotId && !_.IsPlant)?.DepotId;
                        if (item.SaudaValidFromDate != null)
                            saudaValidFromDate = item.SaudaValidFromDate;

                        //}

                        #region PricingLive to Pricing DataInsert and Rearrange the Pricing Data
                        ///Pricing Live is contain Current day Pricing
                        ///So, we insert the Pricing Live data into Pricing table for Sauda booked records only
                        /// Daily we cleanup and fresh data insert into the pricing live table
                        var CurrentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        var pricingLiveContext = _emamiContext.SpecialRatePricingHistory.FirstOrDefault(_ => _.Id == specialContext.PricingId /*&& DbFunctions.TruncateTime(_.CreatedDate) == DbFunctions.TruncateTime(CurrentDate)*/);

                        //var pricingContext = default(Pricing);
                        long pricingId = 0;
                        if (pricingLiveContext == null)
                        {
                            specialContext.StatusId = (int)DTO.Enums.Status.Pending;
                            specialContext.ModifiedBy = inputDto.LoginUserId;
                            specialContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                            specialContext.Remarks = Constants.PricingIdisnotValid;

                            _emamiContext.SaveChanges();
                            return _resultService.ErrorMessage(Constants.PricingIdisnotValid);
                        }
                        // todayPricings.Add(pricingLiveContext.Id);
                        if (pricingLiveContext.PricingReferneceId == 0)
                        {
                            var pricing = new Pricing()
                            {
                                SkuId = pricingLiveContext.SkuId,
                                OilTypeId = pricingLiveContext.OilTypeId,
                                OilPackingTypeId = pricingLiveContext.OilPackingTypeId,
                                PlantId = pricingLiveContext.PlantId,
                                Price = pricingLiveContext.Price,
                                SalesOrganizationId = pricingLiveContext.SalesOrganizationId,
                                DistributionChannelId = pricingLiveContext.DistributionChannelId,
                                DivisionId = pricingLiveContext.DivisionId,
                                SAPPricingCode = pricingLiveContext.SAPPricingCode,
                                ValidFrom = pricingLiveContext.ValidFrom,
                                ValidTo = pricingLiveContext.ValidTo,
                                CreatedBy = pricingLiveContext.CreatedBy,
                                CreatedDate = pricingLiveContext.CreatedDate,
                                ModifiedBy = pricingLiveContext.ModifiedBy,
                                ModifiedDate = pricingLiveContext.ModifiedDate,
                            };
                            _emamiContext.Pricing.Add(pricing);
                            _emamiContext.SaveChanges();
                            pricingId = pricing.Id;
                            /// Update pricingLive Record Pricing Reference Id
                            //var pricingLiveRecord = _emamiContext.TodayPricing.FirstOrDefault(s => s.Id == pricingLiveContext.Id);
                            pricingLiveContext.PricingReferneceId = pricing.Id;
                            _emamiContext.SaveChanges();
                            //pricingContext = pricing;
                        }
                        else
                        {
                            pricingId = pricingLiveContext.PricingReferneceId;
                            //pricingContext = _emamiContext.Pricing.FirstOrDefault(s => s.Id == pricingLiveContext.PricingReferneceId);
                        }
                        specialContext.PricingId = pricingId;
                        _emamiContext.SaveChanges();
                        #endregion

                        i = i + 10;
                        var saudaOrder = new SaudaOrder
                        {
                            SaudaId = saudaContext.Id,
                            SaudaNumber = i.ToString(),
                            SkuId = specialContext.SkuId,
                            OilTypeId = specialContext.OilTypeId,
                            BidPrice = (specialContext.SpecialPrice * specialContext.QuantityCase),
                            SalesOrganizationId = specialContext.SalesOrganizationId,
                            DistributionChannelId = specialContext.DistributionChannelId,
                            DivisionId = specialContext.DivisionId,
                            BidQuantity = _resultService.ConvertCasetoMetricTon(specialContext.QuantityCase, specialContext.SkuId),
                            BidQuantityCase = specialContext.QuantityCase,
                            QuotedPrice = (specialContext.FinalPrice * specialContext.QuantityCase),
                            //BidPriceForDailyReport = (specialContext.SpecialPrice * specialContext.QuantityCase),
                            //BidQuantityForDailyReport = _resultService.ConvertCasetoMetricTon(specialContext.QuantityCase, specialContext.SkuId),
                            //BidQuantityCaseForDailyReport = specialContext.QuantityCase,
                            //QuotedPriceForDailyReport = (specialContext.FinalPrice * specialContext.QuantityCase),
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            PricingId = specialContext.PricingId,
                            //DealerTypeId = (int)DTO.Enums.DealerType.Direct,
                            SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                            Incoterms1 = specialContext.Incoterms1,
                            PlantId = specialContext.DepotId,
                            //DealerLocationId = specialContext.FreightRouteId,
                            //CustomerPONumber = dealerContext.Code + DateHelper.UtcToIndia(DateTime.UtcNow),
                            //CustomerPONumberForDailyReport = dealerContext.Code + DateHelper.UtcToIndia(DateTime.UtcNow),
                            ValidFromDate = saudaValidFromDate.Value,
                            ValidToDate = saudaValidFromDate.Value.AddDays(Convert.ToDouble(dealerContext.SaudaValidityPeriod > 0 ? dealerContext.SaudaValidityPeriod : Config.DefaultSaudaValidity)),
                            StatusId = (int)DTO.Enums.Status.Approved,
                            //SaudaStatusId = (int)DTO.Enums.SaudaStatus.NotReleased,
                            //StatusIdForDailyReport = (int)DTO.Enums.Status.Pending,
                            //SaudaStatusIdForDailyReport = (int)DTO.Enums.SaudaStatus.NotReleased,
                            Incoterms2 = specialContext.Incoterms2,
                            BrokerId = BrokerId,
                            // BrokerIdForDailyReport = BrokerId,
                            SpecialRateRequestId = specialContext.Id,
                            IsSAPDataSync = false,
                            IsSAPDataSyncApproval = false,
                            //DepotIdForRake = depotIdForRake.Value,
                            IsQuantityLimitForBookingSauda = IsQuantityLimitForBookingSauda,
                            QuotedPriceBeforeSAPDiscount = specialContext.SpecialPrice
                        };
                        _emamiContext.SaudaOrders.Add(saudaOrder);
                        _emamiContext.SaveChanges();

                        //if (dealerContext.DivisionId == (int)DTO.Enums.LooseVertical.Loose)
                        //{
                        //    saudaOrderIds.Add(saudaOrder.Id);
                        //}

                        specialContext.StatusId = (int)DTO.Enums.Status.Completed;
                        specialContext.ModifiedBy = inputDto.LoginUserId;
                        specialContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        _emamiContext.SaveChanges();

                        if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                        {
                            var Ids = new List<long> { saudaContext.Id };
                            //method to sync sauda approval from APP to SAP 
                            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                            {
                                _sapIntegrationService.GetSaudaDetails(Ids, false);
                            });
                        }

                        try
                        {
                            var usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == inputDto.LoginUserId || _.Id == specialContext.UserId);
                            var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrder.Id);
                            if (usersContext != null && usersContext.Any() && saudaOrderContext != null)
                            {
                                List<string> toUsers = new List<string>();
                                var createdBy = usersContext.FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                                var dealer = usersContext.FirstOrDefault(_ => _.Id == specialContext.UserId);
                                string dealerName = string.Empty;
                                var userrole = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == createdBy.Id).RoleId;
                                if (userrole != (int)DTO.Enums.Role.NationalTrader)
                                {
                                    if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
                                    {
                                        toUsers.Add(createdBy.Email);
                                    }
                                }
                                if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                                {
                                    dealerName = dealer.Name;
                                    toUsers.Add(dealer.Email);
                                }
                                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                if (_resultService.IsEmail() && toUsers != null && toUsers.Any())
                                {
                                    var fromEmail = Constants.FromEmail;
                                    var emailSubject = Constants.SaudaBookedSubject;
                                    var plainText = string.Empty;
                                    var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationEmail);
                                    if (emailTemplate != null)
                                    {
                                        var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
                                            .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round((saudaOrderContext.BidPrice / saudaOrderContext.BidQuantityCase), 2)).ToString())
                                            .Replace(Constants.BY_FOR, Constants.FOR).Replace(Constants.UserName, dealerName);
                                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                        amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                    }

                                }
                                var smsPlainTemplate = string.Empty;
                                if (_resultService.IsSMS())
                                {
                                    var smsMessage = string.Empty;
                                    EmailTemplate smsTemplate = new EmailTemplate();
                                    smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationSMS);
                                    if (smsTemplate != null)
                                    {
                                        smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
                                            .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round((saudaOrderContext.BidPrice / saudaOrderContext.BidQuantityCase), 2)).ToString())
                                            .Replace(Constants.BY_FOR, Constants.FOR).Replace(Constants.UserName, dealerName);
                                        smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                        if (userrole != (int)DTO.Enums.Role.NationalTrader)
                                        {
                                            if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                                            {
                                                amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber, smsTemplate.SMSTemplateID);
                                            }
                                        }
                                        if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                        {
                                            amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber, smsTemplate.SMSTemplateID);
                                        }
                                    }

                                }
                                if (_resultService.IsPushNotification())
                                {
                                    if (createdBy != null && createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
                                    {
                                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                        {
                                            PushTokenKey = createdBy.PushTokenKey,
                                            RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
                                            Title = Constants.SaudaCreationSubject,
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
                                            Title = Constants.SaudaCreationSubject,
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
                //if (dealerContext.VerticalId == (int)DTO.Enums.LooseVertical.Loose)
                //{
                //    //method to sync Loose sauda from APP to SAP 
                //    _sapIntegrationService.GetSaudaDetailsForLooseVertical(saudaOrderIds);
                //}
                //}
                //else
                //{
                //    return _resultService.ErrorMessage(Constants.SpecialRateNotFoundWithApproval);
                //}




                return _resultService.SuccessMessage(Constants.SpecialRateApprovalSuccess);
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

        public void SpecialRatePricingHistory(List<long> todayPricings)
        {
            _methodName = "SpecialRatePricingHistory";
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DBContext"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string SP_Name = "TodayPricingBackupForSpecialRate";
                    SqlCommand cmd = new SqlCommand(SP_Name, conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    var Ids = string.Join(",", todayPricings);
                    cmd.Parameters.AddWithValue("@PricingIds", Ids);
                    cmd.CommandTimeout = 0;
                    var rdr = cmd.ExecuteReader();
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
            }
        }


        #endregion


        /// <summary>
        /// Method to get sauda list
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetPendingSaudaChartForMobile(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetPendingSaudaChartForMobile";
            var resultDto = new ResultDto();
            var saudaOrdersContext = new List<PendingSaudaChartOutputDto>();
            try
            {
                if (loginUserIdDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (loginUserIdDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == loginUserIdDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                #region NewCode

                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    var sqlQuery = @"CREATE TABLE #DealerTemp(DealerId BIGINT)
                                    Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                                    insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                                    select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId
                                     insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings where UserId=@UserId
                                    select 
                                    (Case when pc.ContractValidFrom is null then Cast('0001-01-01T00:00:00' as datetime2) else pc.ContractValidFrom end) as BiddingDate,
                                    u.Id as UserId,
                                    pc.SaudaQuantity as BidQuantity
                                    
                                    from PendingContracts pc with(NOLOCK)
                                    join Users u on pc.UserId=u.Id
                                    left join Saudas s on pc.SaudaNumber=s.SaudaNumber
                                    join Skus sku on pc.MaterialCode=sku.SkuCode and pc.SalesOrgId=sku.SalesOrganizationId and pc.DistChnlId=sku.DistributionChannelId
                                    and pc.DivisionId=sku.DivisionId
                                    join #UserDivision udiv on udiv.SalesOrganizationId=pc.SalesOrgId and udiv.DistributionChannelId=pc.DistChnlId
                                    and pc.DivisionId=udiv.DivisionId
                                    where  pc.UserId in (select DealerId from #DealerTemp)
                                    and pc.PendingQuantityInCase > 0.99
                                    drop table #UserDivision
                                    drop table #DealerTemp";
                    saudaOrdersContext = conn.Query<PendingSaudaChartOutputDto>(sqlQuery, new
                    {
                        UserId = loginUserIdDto.LoginUserId,

                    }).ToList();

                }
                #endregion

                #region OldCode
                //var dealerlist = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                //                  join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                //                  where ucm.UserId == loginUserIdDto.LoginUserId
                //                  select ucm.CustomerId).ToList();

                //var divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == userContext.Id)
                //    .Select(_ => new { _.SalesOrganizationId, _.DistributionChannelId, _.DivisionId }).ToList();

                //Func<long, long, long, bool> ValidUser = (long salesOrganizationId, long DistributionChannelId, long DivisionId) =>
                //{
                //    foreach (var item in divisionslogieduser)
                //    {
                //        if (item.SalesOrganizationId == salesOrganizationId || item.DistributionChannelId == DistributionChannelId || item.DivisionId == DivisionId)
                //        {
                //            return true;
                //        }
                //    }
                //    return false;
                //};
                //Multiple User Changes
                //var divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == loginUserIdDto.LoginUserId)
                //   .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, 
                //       DistributionChannelId = _.DistributionChannelId, 
                //       DivisionId = _.DivisionId });
                //var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                //var saudaContext = _emamiContext.Sauda.AsQueryable();

                //saudaOrdersContext = (from pct in _emamiContext.PendingContracts.AsNoTracking()
                //                          where pct.PendingQuantityInCase!=0 select pct into pc
                //                          //join sauda in _emamiContext.Sauda.AsNoTracking() on pc.SaudaNumber equals sauda.SaudaNumber
                //                          join dm in divisionslogieduser on new { SalesOrganizationId = pc.SalesOrgId, DistributionChannelId = pc.DistChnlId, DivisionId = pc.DivisionId }
                //                          equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                //                          join ud in _emamiContext.Users.AsNoTracking() on pc.UserId equals ud.Id
                //                          join sku in _emamiContext.Skus.AsNoTracking() on pc.MaterialCode equals sku.SkuCode
                //                          where pc.SalesOrgId == sku.SalesOrganizationId && pc.DistChnlId == sku.DistributionChannelId && pc.DivisionId == sku.DivisionId
                //                           /*into saudadb from sd in saudadb.DefaultIfEmpty()*/
                //                          where pc.PendingQuantityInCase != 0 && dealerlist.Contains(pc.UserId)
                //                           //&& DbFunctions.TruncateTime(pc.ContractValidTo) >= DbFunctions.TruncateTime(currentDate)
                //                          //&& sauda.BdoId == loginUserIdDto.LoginUserId
                //                          select new PendingSaudaChartOutputDto()
                //                          { UserId = pc.UserId, BidQuantity = pc.SaudaQuantity, BiddingDate = (saudaContext.FirstOrDefault(_ => _.SaudaNumber == pc.SaudaNumber) != null ? saudaContext.FirstOrDefault(_ => _.SaudaNumber == pc.SaudaNumber).BiddingDate : DateTime.Now) }).ToList();

                //Old Query
                //var saudaOrdersContext1 = (from pc in _emamiContext.PendingContracts.AsNoTracking()
                //                          join ud in _emamiContext.Users.AsNoTracking() on pc.UserId equals ud.Id
                //                          join sku in _emamiContext.Skus.AsNoTracking() on pc.MaterialCode equals sku.SkuCode where pc.SalesOrgId == sku.SalesOrganizationId && pc.DistChnlId == sku.DistributionChannelId && pc.DivisionId == sku.DivisionId
                //                          join sauda in _emamiContext.Sauda.AsNoTracking() on pc.SaudaNumber equals sauda.SaudaNumber /*into saudadb from sd in saudadb.DefaultIfEmpty()*/
                //                          where pc.PendingQuantityInCase != 0 && dealerlist.Contains(pc.UserId)
                //                          select new PendingSaudaChartOutputDto()
                //                          { UserId = pc.UserId, BidQuantity = pc.SaudaQuantity, BiddingDate = (sauda.BiddingDate!=null ? sauda.BiddingDate : DateTime.Now) }).ToList();

                #endregion

                if (saudaOrdersContext != null && saudaOrdersContext.Any())
                {
                    return _resultService.SuccessObject(saudaOrdersContext);
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }
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

        public ResultDto GetPendingSaudaChartDetailForMobile(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetPendingSaudaChartDetailForMobile";
            var resultDto = new ResultDto();
            var saudaListDto = new List<SaudaListDto>();
            try
            {
                if (loginUserIdDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (loginUserIdDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == loginUserIdDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var dealerlist = (loginUserIdDto.DealerId > 0) ? new List<long> { loginUserIdDto.DealerId }
                                  : (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                     join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                                     where ucm.UserId == loginUserIdDto.LoginUserId
                                     select ucm.CustomerId).ToList();

                var divisionsloginWiseuser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == loginUserIdDto.LoginUserId)
                .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                var city = _emamiContext.City.AsQueryable();
                var saudaContext = _emamiContext.Sauda.AsQueryable();

                //saudaListDto = _emamiContext.PendingContracts.AsNoTracking()
                //    .Join(_emamiContext.Users.AsNoTracking(), x => x.UserId, u => u.Id, (x, u) => new { x, u })
                //    //.Join(_emamiContext.City.AsNoTracking(), x => x.u.CityId, c => c.Id, (x, c) => new { x.x, DealerName = x.u.Name, CityName = c.CityName, DealerId = x.u.Id/*, VerticalId = x.u.DivisionId */})
                //    .Join(_emamiContext.Skus.AsNoTracking(), s => s.x.MaterialCode, ss => ss.SkuCode, (s, ss) => new { s.x, ss, DealerName=s.u.Name, CityId=s.u.CityId, DealerId=s.u.Id/*, s.VerticalId*/ })
                //    .Join(_emamiContext.Sauda.AsNoTracking(), sauda => sauda.x.SaudaNumber , sa => sa.SaudaNumber , (sauda , sa) => new { sauda , sa})
                //    .Where(_ => _.sauda.x.PendingQuantityInCase != 0 && dealerlist.Contains(_ .sauda.DealerId)
                //    && _.sauda.x.SalesOrgId == _.sauda.ss.SalesOrganizationId && _.sauda.x.DistChnlId == _.sauda.ss.DistributionChannelId
                //    && _.sauda.x.DivisionId == _.sauda.ss.DivisionId
                //    ).Select(a => new SaudaListDto {
                //        Id = a.sauda.x.Id,
                //        SaudaOrderId = a.sauda.x.Id,
                //        UserId = a.sauda.DealerId,
                //        User = a.sauda.DealerName,
                //        City = city.FirstOrDefault(_ => _.Id==a.sauda.CityId)!=null? city.FirstOrDefault(_ => _.Id == a.sauda.CityId).CityName : String.Empty,
                //        BiddingDate = a.sa.BiddingDate != null ? a.sa.BiddingDate : DateTime.Today,
                //        TotalBidPrice = a.sauda.x.BasicRate,
                //        TotalBidQuantity = a.sauda.x.SaudaQuantity,
                //        OiltypeName = a.sauda.ss.OilType.Name+"-"+ a.sauda.ss.OilType.SalesOrganization.Code+"/"+ a.sauda.ss.OilType.DistributionChannel.Code+"/"+ a.sauda.ss.OilType.Division.Code,
                //        OilTypeId = a.sauda.ss.OilType.Id,
                //        ValidToDate = a.sauda.x.ContractValidTo ?? DateTime.Today,
                //        BidQuantity = a.sauda.x.SaudaQuantity,
                //        BidQuantityCase = a.sauda.x.PendingQuantityInCase
                //    }).ToList();

                IEnumerable<SaudaListDto> invoiceListContext = new List<SaudaListDto>();
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    var sqlQuery = @"CREATE TABLE #DealerTemp(DealerId BIGINT)

                        Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                        insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                        select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

                        if(@CustomerId >0)
                        begin
	                        insert into #DealerTemp select @CustomerId
                        end
                        else
                        begin 
	                        insert into #DealerTemp select CustomerId from UserCustomerMappings where UserId=@UserId
                        end


                        select
                        p.Id,
                        (Case when s.Id is null then 0 else s.Id end) as SaudaOrderId,
                        u.Id as UserId,
                        u.Name as [User],
                        (Case when c.CityName is null then c.CityName else '' end) as City,
                        (Case when p.ContractValidFrom is null then Cast('0001-01-01T00:00:00' as datetime2) else p.ContractValidFrom end) as BiddingDate,
                        p.BasicRate as TotalBidPrice,
                        p.SaudaQuantity as TotalBidQuantity,
                        (o.Name+'-'+sorg.Code+'/'+dist.Code+'/'+div.Code) as OiltypeName,
                        o.Id as OilTypeId,
                        (Case when p.ContractValidTo is null then Cast('0001-01-01T00:00:00' as datetime2) else p.ContractValidTo end) as ValidToDate,
                        p.SaudaQuantity as BidQuantity,
                        p.PendingQuantityInCase as BidQuantityCase,
                        p.SaudaNumber
                        from PendingContracts p with(NOLOCK)
                        left join Saudas s with(NOLOCK) on p.SaudaNumber=s.SaudaNumber
                        join Users u on p.UserId=u.Id
                        left join Cities c on u.CityId=c.Id
                        join Skus sku on p.MaterialCode=sku.SkuCode and p.SalesOrgId=sku.SalesOrganizationId
                        and p.DistChnlId=sku.DistributionChannelId and p.DivisionId=sku.DivisionId
                        join OilTypes o on sku.OilTypeId=o.Id
                        join SalesOrganizations sorg on o.SalesOrganizationId=sorg.Id
                        join DistributionChannels dist on dist.Id=o.DistributionChannelId
                        join Divisions div on o.DivisionId=div.Id
                        join #UserDivision ud on ud.SalesOrganizationId=p.SalesOrgId and ud.DistributionChannelId=p.DistChnlId
                        and p.DivisionId=ud.DivisionId
                        where p.PendingQuantityInCase > 0.99
                        and u.Id in (select DealerId from #DealerTemp)
                        order by p.Id desc
                        drop table #DealerTemp
                        drop table #UserDivision";
                    saudaListDto = conn.Query<SaudaListDto>(sqlQuery, new
                    {
                        UserId = loginUserIdDto.LoginUserId,
                        CustomerId = loginUserIdDto.DealerId
                    }).ToList();

                }

                //saudaListDto = (from pct in _emamiContext.PendingContracts.AsNoTracking()
                //                where pct.PendingQuantityInCase !=0 select pct into pc
                //                join ud in _emamiContext.Users.AsNoTracking() on pc.UserId equals ud.Id
                //                join sku in _emamiContext.Skus.AsNoTracking() on pc.MaterialCode equals sku.SkuCode
                //                where pc.SalesOrgId == sku.SalesOrganizationId && pc.DistChnlId == sku.DistributionChannelId && pc.DivisionId == sku.DivisionId
                //                join o in _emamiContext.OilTypes.AsNoTracking() on sku.OilTypeId equals o.Id
                //                join sorg in _emamiContext.SalesOrganization.AsNoTracking() on o.SalesOrganizationId equals sorg.Id
                //                join dist in _emamiContext.DistributionChannel.AsNoTracking() on o.DistributionChannelId equals dist.Id
                //                join div in _emamiContext.Divisions.AsNoTracking() on o.DivisionId equals div.Id                      
                //                //join sauda in _emamiContext.Sauda.AsNoTracking() on pc.SaudaNumber equals sauda.SaudaNumber
                //                join dm in divisionsloginWiseuser on new { SalesOrganizationId = pc.SalesOrgId, DistributionChannelId = pc.DistChnlId, DivisionId = pc.DivisionId }
                //                equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId } /*into saudadb from sd in saudadb.DefaultIfEmpty()*/
                //                where  dealerlist.Contains(pc.UserId)
                //                 //&& DbFunctions.TruncateTime(pc.ContractValidTo) >= DbFunctions.TruncateTime(currentDate)
                //                //&& sauda.BdoId == loginUserIdDto.LoginUserId
                //                select new SaudaListDto()
                //                {
                //                    Id = pc.Id,
                //                    SaudaOrderId = saudaContext.FirstOrDefault(_ => _.SaudaNumber == pc.SaudaNumber) != null ? saudaContext.FirstOrDefault(_ => _.SaudaNumber == pc.SaudaNumber).Id : 0,//sauda table Id
                //                    UserId = ud.Id,
                //                    User = ud.Name,
                //                    City = city.FirstOrDefault(_ => _.Id == ud.CityId) != null ? city.FirstOrDefault(_ => _.Id == ud.CityId).CityName : String.Empty,
                //                    BiddingDate = saudaContext.FirstOrDefault(_ => _.SaudaNumber == pc.SaudaNumber) != null ? saudaContext.FirstOrDefault(_ => _.SaudaNumber == pc.SaudaNumber).BiddingDate : DateTime.Today,
                //                    TotalBidPrice = pc.BasicRate,
                //                    TotalBidQuantity = pc.SaudaQuantity,
                //                    OiltypeName = o.Name + "-" + sorg.Code + "/" + dist.Code + "/" + div.Code,
                //                    OilTypeId = o.Id,
                //                    ValidToDate = pc.ContractValidTo ?? DateTime.Today,
                //                    BidQuantity = pc.SaudaQuantity,
                //                    BidQuantityCase = pc.PendingQuantityInCase
                //                }).ToList();

                if (saudaListDto != null && saudaListDto.Any())
                {
                    if (loginUserIdDto.IsPendingSauda)
                    {
                        var data = saudaListDto.OrderByDescending(s => s.BiddingDate).GroupBy(s => s.BiddingDate.Date).Select(a => new SaudaListGroupedOutputDto()
                        {
                            BiddingDate = a.Key,
                            saudaListOutputs = a.Select(sauda => new SaudaListOutputDto()
                            {
                                SaudaId = sauda.Id,
                                SaudaNo = sauda.Id.ToString(),
                                SaudaOrderId = sauda.SaudaOrderId,
                                BiddingDate = sauda.BiddingDate,
                                TotalQty = sauda.BidQuantity,
                                SaudaNumber = sauda.SaudaNumber != null ? sauda.SaudaNumber : string.Empty,
                                DealerName = sauda.User,
                                DealerId = sauda.UserId
                            }).ToList()
                        }).ToList();
                        return _resultService.SuccessObject(data);
                    }
                    else
                    {
                        saudaListDto = saudaListDto.OrderBy(a => a.User).ToList();
                        return _resultService.SuccessObject(saudaListDto);
                    }
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }
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




        public ResultDto GetBookedSauda(BookedSaudaInputDto inputDto)
        {
            _methodName = "GetBookedSauda";
            var resultDto = new ResultDto();
            var saudaListDto = new List<BookedSaudaDetailsDto>();

            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }

                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }

                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }

                List<long> dealerList = new List<long>();

                if (inputDto.DealerId > 0)
                {
                    dealerList.Add(inputDto.DealerId);
                }
                else
                {
                    dealerList = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                  join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                                  where ucm.UserId == inputDto.LoginUserId
                                  select ucm.CustomerId).Distinct().ToList();
                }

                var loginUserdata = _emamiContext.Users.AsNoTracking()
                    .FirstOrDefault(f => f.Id == inputDto.LoginUserId);

                var divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking()
                    .Where(_ => _.UserId == inputDto.LoginUserId)
                    .Select(_ => new DivisionDetailsDto
                    {
                        SalesOrganizationId = _.SalesOrganizationId,
                        DistributionChannelId = _.DistributionChannelId,
                        DivisionId = _.DivisionId
                    });

                IEnumerable<BookedSDto> saudatableContext = new List<BookedSDto>();

                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {
                    var sqlQuery = @"CREATE TABLE #DealerTemp(DealerId BIGINT)

                CREATE TABLE #UserDivision
                (
                    SalesOrganizationId BIGINT,
                    DistributionChannelId BIGINT,
                    DivisionId BIGINT
                )

                INSERT INTO #UserDivision
                SELECT SalesOrganizationId, DistributionChannelId, DivisionId
                FROM UserDivisionMappings
                WHERE UserId = @UserId

                IF(@CustomerId > 0)
                BEGIN
                    INSERT INTO #DealerTemp SELECT @CustomerId
                END
                ELSE
                BEGIN
                    INSERT INTO #DealerTemp
                    SELECT CustomerId FROM UserCustomerMappings WHERE UserId = @UserId
                END

                SELECT 
                    s.Id,
                    s.UserId,
                    s.StatusId,
                    s.BiddingDate,
                    s.SaudaNumber
                FROM Saudas s WITH(NOLOCK)
                JOIN #UserDivision ud 
                    ON s.SalesOrganizationId = ud.SalesOrganizationId
                    AND s.DistributionChannelId = ud.DistributionChannelId
                    AND s.DivisionId = ud.DivisionId
                WHERE s.UserId IN (SELECT DealerId FROM #DealerTemp)
                  AND CAST(s.BiddingDate AS DATE) >= CAST(@FromDate AS DATE)
                  AND CAST(s.BiddingDate AS DATE) <= CAST(@ToDate AS DATE)";

                    saudatableContext = conn.Query<BookedSDto>(sqlQuery, new
                    {
                        UserId = inputDto.LoginUserId,
                        FromDate = inputDto.FromDate,
                        ToDate = inputDto.ToDate,
                        CustomerId = inputDto.DealerId
                    });
                }

                var saudaId = saudatableContext.Select(s => s.Id).Distinct().ToList();

                var saudaOrdersContext = _emamiContext.SaudaOrders.AsNoTracking()
                    .Where(w => saudaId.Contains(w.SaudaId));

                if (inputDto.SalesOrganizationId > 0 &&
                    inputDto.DistributionChannelId > 0 &&
                    inputDto.DivisionId > 0)
                {
                    saudaOrdersContext = saudaOrdersContext.Where(_ =>
                        _.SalesOrganizationId == inputDto.SalesOrganizationId &&
                        _.DistributionChannelId == inputDto.DistributionChannelId &&
                        _.DivisionId == inputDto.DivisionId);
                }

                var saudaOrderContext = saudaOrdersContext.Select(s => new
                {
                    Id = s.Id,
                    SaudaId = s.SaudaId,
                    OilTypeId = s.Sku.OilTypeId,
                    SkuId = s.SkuId,
                    StatusId = s.StatusId,
                    BidQuantity = s.BidQuantity
                });

                var userContext = _emamiContext.Users.AsNoTracking()
                    .Where(w => dealerList.Contains(w.Id))
                    .Select(s => new
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Code = s.Code,
                        s.CityId,
                        s.StateId
                    }).ToList();

                var oilTypesContext = _emamiContext.OilTypes.AsNoTracking()
                    .Select(s => new
                    {
                        Id = s.Id,
                        Name = s.Name + "-" +
                               s.SalesOrganization.Code + "/" +
                               s.DistributionChannel.Code + "/" +
                               s.Division.Code
                    }).ToList();

                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().ToList();
                var cityContext = _emamiContext.City.AsNoTracking();
                var stateContext = _emamiContext.State.AsNoTracking();

                // --- NEW: fetch latest approval per SaudaId and approval user names (only added logic) ---
                var latestApprovals = _emamiContext.SaudaApproval.AsNoTracking()
                    .Where(a => saudaId.Contains(a.SaudaId))
                    .GroupBy(a => a.SaudaId)
                    .Select(g => g.OrderByDescending(x => x.Id).FirstOrDefault())
                    .ToList();

                var approvalUserIds = latestApprovals.Where(a => a != null && a.RequestedTo > 0)
                                                     .Select(a => a.RequestedTo)
                                                     .Distinct()
                                                     .ToList();

                var approvalUsersDict = new Dictionary<long, string>();
                if (approvalUserIds.Any())
                {
                    var approvalUsers = _emamiContext.Users.AsNoTracking()
                        .Where(u => approvalUserIds.Contains(u.Id))
                        .Select(u => new { u.Id, u.Name })
                        .ToList();

                    approvalUsersDict = approvalUsers.ToDictionary(u => u.Id, u => u.Name);
                }

                // map SaudaId -> RequestedTo user id
                var approvalBySauda = latestApprovals.Where(a => a != null)
                                                     .ToDictionary(a => a.SaudaId, a => a.RequestedTo);

                saudaListDto = saudatableContext
                    .GroupBy(_ => _.UserId)
                    .Select(_ => new BookedSaudaDetailsDto
                    {
                        DealerId = _.First().UserId
                    }).ToList();

                foreach (var dealer in saudaListDto)
                {
                    var saudaContext = saudatableContext.Where(_ => _.UserId == dealer.DealerId).ToList();
                    if (saudaContext != null)
                    {
                        foreach (var sauda in saudaContext)
                        {
                            var SaudaDetailContext = saudaOrderContext.Where(_ => _.SaudaId == sauda.Id).ToList();
                            var Dealer = userContext.FirstOrDefault(_ => _.Id == sauda.UserId).Name;
                            var DealerCode = userContext.FirstOrDefault(_ => _.Id == sauda.UserId).Code;
                            var CityId = userContext.FirstOrDefault(_ => _.Id == sauda.UserId).CityId;
                            var StateId = userContext.FirstOrDefault(_ => _.Id == sauda.UserId).StateId;
                            dealer.Dealer = string.Concat(Dealer + "-" + (cityContext.FirstOrDefault(c => c.Id == CityId) != null ? cityContext.FirstOrDefault(c => c.Id == CityId).CityName : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == StateId) != null ? stateContext.FirstOrDefault(s => s.Id == StateId).StateName : string.Empty) + "-" + DealerCode);

                            var saudaDto = new BookedSaudaDto
                            {
                                SaudaId = sauda.Id,
                                DealerId = sauda.UserId,
                                Dealer = string.Concat(Dealer + "-" + (cityContext.FirstOrDefault(c => c.Id == CityId) != null ? cityContext.FirstOrDefault(c => c.Id == CityId).CityName : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == StateId) != null ? stateContext.FirstOrDefault(s => s.Id == StateId).StateName : string.Empty) + "-" + DealerCode),
                                SaudaBookedDate = sauda.BiddingDate,
                                IsBroker = userRoleContext.Any(_ => _.UserId == sauda.UserId && _.RoleId == (int)DTO.Enums.Role.Broker) ? true : false,
                                SaudaNumber = string.IsNullOrEmpty(sauda.SaudaNumber) ? sauda.Id.ToString() : sauda.SaudaNumber,
                                StatusId = SaudaDetailContext.FirstOrDefault() != null ? SaudaDetailContext.FirstOrDefault().StatusId : 0,
                                Status = SaudaDetailContext.FirstOrDefault() != null ? UtilityHelper.GetEnumDescription((DTO.Enums.Status)SaudaDetailContext.FirstOrDefault().StatusId) : string.Empty,
                                Location = string.Concat((cityContext.FirstOrDefault(c => c.Id == CityId) != null ? cityContext.FirstOrDefault(c => c.Id == CityId).CityName : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == StateId) != null ? stateContext.FirstOrDefault(s => s.Id == StateId).StateName : string.Empty)),
                                TotalQuantity = SaudaDetailContext.IsAny() ? SaudaDetailContext.Sum(_ => _.BidQuantity) : 0,
                                ApprovalUser = string.Empty // default, may be populated below
                            };

                            // populate ApprovalUser if present (preserve the added behavior)
                            if (approvalBySauda.TryGetValue(sauda.Id, out var requestedTo) && requestedTo > 0)
                            {
                                if (approvalUsersDict.TryGetValue(requestedTo, out var approverName))
                                {
                                    saudaDto.ApprovalUser = approverName;
                                }
                            }

                            var results = SaudaDetailContext.GroupBy(
                                p => p.OilTypeId,
                                p => p.SkuId,
                                (key, g) => new { OilTypeId = key, Skus = g.ToList() }).ToList();

                            foreach (var detail in results)
                            {
                                var DetailDto = new BookedSaudaDetailDto
                                {
                                    OilTypeId = (long)detail.OilTypeId,
                                    OilType = oilTypesContext.FirstOrDefault(_ => _.Id == detail.OilTypeId).Name,
                                    SkuCount = detail.Skus.Count
                                };
                                saudaDto.BookedSaudaDetailDto.Add(DetailDto);
                            }

                            dealer.BookedSaudaList.Add(saudaDto);
                        }
                    }
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



        //var saudatableContext = (from s in _emamiContext.Sauda.AsNoTracking()
        //                         join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
        //                         equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
        //                         where dealerList.Contains(s.UserId)
        //                         //&& (s.BdoId == inputDto.LoginUserId || s.BdoId==0)
        //                && (DbFunctions.TruncateTime(s.BiddingDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
        //                && DbFunctions.TruncateTime(s.BiddingDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
        //                select new BookedSDto()
        //                {
        //                    Id = s.Id,
        //                    UserId = s.UserId,
        //                    StatusId = s.StatusId,                                    
        //                    BiddingDate = s.BiddingDate,
        //                    SaudaNumber = s.SaudaNumber
        //                }).ToList();

        //Old Query
        //var saudatableContext1 = _emamiContext.Sauda.AsNoTracking()
        //    .Where(w => dealerList.Contains(w.UserId)
        //    //&& w.SaudaBookingTypeId == loginUserdata.SaudaBookingTypeId 
        //    && (DbFunctions.TruncateTime(w.BiddingDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
        //    && DbFunctions.TruncateTime(w.BiddingDate) <= DbFunctions.TruncateTime(inputDto.ToDate)))
        //    .Select(s => new
        //    {
        //        Id = s.Id,
        //        UserId = s.UserId,
        //        StatusId = s.StatusId,
        //        //SaudaBookingTypeId = s.SaudaBookingTypeId,
        //        BiddingDate = s.BiddingDate,
        //        SaudaNumber = s.SaudaNumber
        //    }).ToList();



        //var sauda = saudaListsDto.ToList();
        //saudaListDto = saudatableContext
        //   .GroupBy(_ => _.UserId).Select(_ => new BookedSaudaDto
        //   {
        //       DealerId = _.FirstOrDefault().UserId,
        //       //Dealer = string.Concat(userContext.FirstOrDefault(u => u.Id == _.FirstOrDefault().UserId).Name + "-" + (cityContext.FirstOrDefault(c => c.Id == userContext.FirstOrDefault(u => u.Id == _.FirstOrDefault().UserId).CityId).CityName != null ? cityContext.FirstOrDefault(c => c.Id == userContext.FirstOrDefault(u => u.Id == _.FirstOrDefault().UserId).CityId).CityName : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == userContext.FirstOrDefault(u => u.Id == _.FirstOrDefault().UserId).StateId).StateName != null ? stateContext.FirstOrDefault(s => s.Id == userContext.FirstOrDefault(u => u.Id == _.FirstOrDefault().UserId).StateId).StateName : string.Empty) + "-" + userContext.FirstOrDefault(u => u.Id == _.FirstOrDefault().UserId).Code),
        //       SaudaId = _.FirstOrDefault().Id,
        //       SaudaBookedDate = _.FirstOrDefault().BiddingDate,
        //       IsBroker = userRoleContext.Any(ur => ur.UserId == _.FirstOrDefault().UserId && ur.RoleId == (int)DTO.Enums.Role.Broker) ? true : false,
        //       SaudaNumber = _.FirstOrDefault().Id.ToString(),
        //       BookedSaudaDetailDto = saudaOrderContext.Where(a => a.SaudaId == _.FirstOrDefault().Id)
        //       .GroupBy(
        //                p => p.OilTypeId,
        //                p => p.SkuId,
        //                (key, g) => new { OilTypeId = key, Skus = g.ToList() })
        //       .Select(detail => new BookedSaudaDetailDto
        //       {
        //           OilTypeId = detail.OilTypeId,
        //           OilType = oilTypesContext.FirstOrDefault(o => o.Id == detail.OilTypeId).Name,
        //           SkuCount = detail.Skus.Count
        //       }).ToList(),
        //   }).ToList();



        public ResultDto GetBookedSaudaOld(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetBookedSaudaOld";
            var resultDto = new ResultDto();
            var saudaListDto = new List<BookedSaudaDto>();
            try
            {
                if (loginUserIdDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (loginUserIdDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                List<long> dealerList = new List<long>();
                if (loginUserIdDto.DealerId > 0)
                {
                    dealerList.Add(loginUserIdDto.DealerId);
                }
                else
                {
                    dealerList = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                  join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                                  where ucm.UserId == loginUserIdDto.LoginUserId
                                  select ucm.CustomerId).Distinct().ToList();
                }
                var saudatableContext = _emamiContext.Sauda.AsNoTracking().ToList();
                var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().ToList();
                var userContext = _emamiContext.Users.AsNoTracking().ToList();
                var oilTypesContext = _emamiContext.OilTypes.AsNoTracking().ToList();
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().ToList();
                foreach (var dealer in dealerList)
                {
                    var saudaContext = saudatableContext.Where(_ => _.UserId == dealer).ToList();
                    if (saudaContext != null)
                    {
                        foreach (var sauda in saudaContext)
                        {
                            var SaudaDetailContext = saudaOrderContext.Where(_ => _.SaudaId == sauda.Id).ToList();
                            var Dealer = userContext.FirstOrDefault(_ => _.Id == sauda.UserId).Name;

                            //if (sauda.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                            //{
                            //    var saudaDto = new BookedSaudaDto
                            //    {
                            //        DealerId = sauda.UserId,
                            //        Dealer = Dealer,
                            //        SaudaBookedDate = sauda.BiddingDate,
                            //        IsBroker = userRoleContext.Any(_ => _.UserId == sauda.UserId && _.RoleId == (int)DTO.Enums.Role.Broker) ? true : false,
                            //        SaudaNumber = sauda.Id.ToString()
                            //    };
                            //    var results = SaudaDetailContext.GroupBy(
                            //        p => p.OilTypeId,
                            //        p => p.SkuId,
                            //        (key, g) => new { OilTypeId = key, Skus = g.ToList() }).ToList();
                            //    foreach (var detail in results)
                            //    {
                            //        var DetailDto = new BookedSaudaDetailDto
                            //        {
                            //            OilTypeId = detail.OilTypeId,
                            //            OilType = oilTypesContext.FirstOrDefault(_ => _.Id == detail.OilTypeId).Name,
                            //            SkuCount = detail.Skus.Count
                            //        };
                            //        saudaDto.BookedSaudaDetailDto.Add(DetailDto);
                            //    }
                            //    saudaListDto.Add(saudaDto);
                            //    /*
                            //    long BiddingWindowId = SaudaDetailContext.FirstOrDefault(_ => _.SaudaId == sauda.Id) != null ? SaudaDetailContext.FirstOrDefault(_ => _.SaudaId == sauda.Id).BiddingwindowId : 0;
                            //    if (BiddingWindowId != 0)
                            //    {
                            //        var BiddingWindowContext = _emamiContext.BiddingWindowTiming.AsNoTracking().FirstOrDefault(_ => _.Id == BiddingWindowId);
                            //        if (BiddingWindowContext != null)
                            //        {
                            //            DateTime BiddingWindowDate = BiddingWindowContext.BiddingDate;
                            //            TimeSpan BiddingEndTime = BiddingWindowContext.ToHours;
                            //            DateTime BiddingWindowEndDateTime = BiddingWindowDate + BiddingEndTime;
                            //            string configBufferMinutes = _emamiContext.Configurations.FirstOrDefault(_ => _.Id == (int)DTO.Enums.Configuration.BidStatusTime) != null ? _emamiContext.Configurations.FirstOrDefault(_ => _.Id == (int)DTO.Enums.Configuration.BidStatusTime).Value : string.Empty;
                            //            if (configBufferMinutes != string.Empty)
                            //            {
                            //                long bufferMinutes = Convert.ToInt32(configBufferMinutes);
                            //                BiddingWindowEndDateTime.AddMinutes(bufferMinutes);
                            //            }
                            //            if (DateHelper.UtcToIndia(DateTime.UtcNow) > BiddingWindowEndDateTime)
                            //            {
                            //                var saudaDto = new BookedSaudaDto
                            //                {
                            //                    DealerId = sauda.UserId,
                            //                    Dealer = Dealer.Name,
                            //                    SaudaBookedDate = sauda.BiddingDate,
                            //                    IsBroker = _emamiContext.UserRoles.AsNoTracking().Any(_ => _.UserId == sauda.UserId && _.RoleId == (int)DTO.Enums.Role.Broker) ? true : false,
                            //                    SaudaNumber = sauda.Id.ToString()
                            //                };
                            //                var results = SaudaDetailContext.GroupBy(
                            //                    p => p.OilTypeId,
                            //                    p => p.SkuId,
                            //                    (key, g) => new { OilTypeId = key, Skus = g.ToList() }).ToList();
                            //                foreach (var detail in results)
                            //                {
                            //                    var DetailDto = new BookedSaudaDetailDto
                            //                    {
                            //                        OilTypeId = detail.OilTypeId,
                            //                        OilType = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == detail.OilTypeId).Name,
                            //                        SkuCount = detail.Skus.Count
                            //                    };
                            //                    saudaDto.BookedSaudaDetailDto.Add(DetailDto);
                            //                }
                            //                saudaListDto.Add(saudaDto);
                            //            }
                            //        }
                            //    }
                            //    */
                            //}
                            //else
                            //{
                            var saudaDto = new BookedSaudaDto
                            {
                                DealerId = sauda.UserId,
                                Dealer = Dealer,
                                SaudaBookedDate = sauda.BiddingDate,
                                IsBroker = userRoleContext.Any(_ => _.UserId == sauda.UserId && _.RoleId == (int)DTO.Enums.Role.Broker) ? true : false,
                                SaudaNumber = sauda.Id.ToString()
                            };
                            var results = SaudaDetailContext.GroupBy(
                                p => p.OilTypeId,
                                p => p.SkuId,
                                (key, g) => new { OilTypeId = key, Skus = g.ToList() }).ToList();
                            foreach (var detail in results)
                            {
                                var DetailDto = new BookedSaudaDetailDto
                                {
                                    OilTypeId = detail.OilTypeId,
                                    OilType = oilTypesContext.FirstOrDefault(_ => _.Id == detail.OilTypeId).Name,
                                    SkuCount = detail.Skus.Count
                                };
                                saudaDto.BookedSaudaDetailDto.Add(DetailDto);
                            }
                            saudaListDto.Add(saudaDto);
                            //}
                        }
                    }
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
        /// Method to Get Sauda order details
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetSaudaorderdetails(SaudaDetailInputDto inputDto)
        {
            _methodName = "GetSaudaorderdetails";
            var resultDto = new ResultDto();
            var saudaDetails = new SaudaDetailOutputDto();
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
                    resultDto.ErrorDto.ErrorCode = Constants.DealerIdEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.DealerIdEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.UserId);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                var saudaContext = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SaudaId);
                if (saudaContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id);

                if (inputDto.SalesOrganizationId > 0 && inputDto.DistributionChannelId > 0 && inputDto.DivisionId > 0)
                {
                    saudaOrderContext = saudaOrderContext.Where(_ => _.SalesOrganizationId == inputDto.SalesOrganizationId
                    && _.DistributionChannelId == inputDto.DistributionChannelId
                    && _.DivisionId == inputDto.DivisionId);
                }

                if (saudaOrderContext != null && saudaOrderContext.Any())
                {
                    var totalBidAmount = saudaOrderContext.Sum(_ => (decimal?)_.BidPrice) ?? 0;
                    var totalBidQuantity = saudaOrderContext.Sum(_ => (decimal?)_.BidQuantity) ?? 0;

                    saudaDetails.TotalAmount = totalBidAmount;
                    saudaDetails.TotalQuantity = totalBidQuantity;

                    saudaDetails.BrokerId = saudaOrderContext.Where(_ => _.SaudaId == saudaContext.Id).FirstOrDefault() != null
                        ? saudaOrderContext.Where(_ => _.SaudaId == saudaContext.Id).FirstOrDefault().BrokerId : 0;
                    if (saudaDetails.BrokerId > 0)
                    {
                        saudaDetails.BrokerName = saudaOrderContext.Where(_ => _.SaudaId == saudaContext.Id).FirstOrDefault() != null
                             ?
                            _emamiContext.Users.FirstOrDefault(_ => _.Id == saudaOrderContext.Where(s => s.SaudaId == saudaContext.Id).FirstOrDefault().BrokerId).Name : string.Empty;

                    }
                    //var BrokerContext = saudaOrderContext.Where(_ => _.SaudaId == saudaContext.Id).FirstOrDefault();
                    //if (BrokerContext != null)
                    //{
                    //    saudaDetails.BrokerId = BrokerContext.BrokerId;
                    //    saudaDetails.BrokerName = BrokerContext.BrokerId != 0 ? _emamiContext.Users.FirstOrDefault(_ => _.Id == BrokerContext.BrokerId).Name : string.Empty;
                    //}

                }

                saudaDetails.SaudaId = saudaContext.Id;
                saudaDetails.SaudaNumber = saudaContext.SaudaNumber != null ? saudaContext.SaudaNumber.ToString() : "";
                saudaDetails.BiddingDate = saudaContext.BiddingDate;
                saudaDetails.DealerId = saudaContext.UserId;
                saudaDetails.StatusId = saudaContext.StatusId;
                saudaDetails.Status = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == saudaContext.StatusId).Name;
                saudaDetails.DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaContext.UserId).Name;
                //saudaDetails.Incoterm = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == saudaContext.Incoterms2).Name;
                saudaDetails.SaudaValidityDays = Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity;
                saudaDetails.SaudaExpireDays = (DateHelper.UtcToIndia(DateTime.UtcNow) - saudaContext.BiddingDate).Days;
                saudaDetails.ExpiryDate = saudaContext.BiddingDate.AddDays(Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity);
                saudaDetails.Remarks = _emamiContext.Remarks.AsNoTracking().OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.TableId == saudaContext.Id && _.IsActive) != null ? _emamiContext.Remarks.AsNoTracking().OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.TableId == saudaContext.Id && _.IsActive).Description : string.Empty;

                var saudaAudioFileMappingContext = _emamiContext.SaudaAudioFileMapping.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id);
                var key = UtilityHelper.GetEnumDescription((DTO.Enums.Configuration.CallRecordMappingReattachBufferTime));
                var reattachBufferTime = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Key == key)?.Value ?? "0";
                var BufferTimeToAdd = Convert.ToDouble(reattachBufferTime);
                if (!saudaAudioFileMappingContext.IsAny())
                {
                    saudaDetails.CanSubmitAudioMapping = true;
                }
                else if (saudaAudioFileMappingContext.IsAny())
                {
                    var ImageCreatedDate = saudaAudioFileMappingContext.FirstOrDefault(_ => _.SaudaId == saudaContext.Id && (_.MediaTypeId == (int)DTO.Enums.MediaType.Audio || _.MediaTypeId == (int)DTO.Enums.MediaType.Image)).CreatedDate;
                    var timeUntilReattachmentDone = ImageCreatedDate.AddMinutes(BufferTimeToAdd);
                    if (DateHelper.UtcToIndia(DateTime.UtcNow) <= timeUntilReattachmentDone)
                    {
                        saudaDetails.CanSubmitAudioMapping = true;
                    }
                }

                saudaDetails.AudiofileDetailIds = saudaAudioFileMappingContext.Where(_ => _.MediaTypeId == (int)DTO.Enums.MediaType.Audio).Select(s => s.AudioFileDetailsForActiveCustomersId ?? 0).ToList();

                var imageNames = saudaAudioFileMappingContext.FirstOrDefault(_ => _.SaudaId == saudaContext.Id && _.MediaTypeId == (int)DTO.Enums.MediaType.Image) != null ? saudaAudioFileMappingContext.FirstOrDefault(_ => _.SaudaId == saudaContext.Id && _.MediaTypeId == (int)DTO.Enums.MediaType.Image).ImagePath : string.Empty;
                if (imageNames != string.Empty)
                {
                    saudaDetails.ImagePaths = imageNames.Split(',').ToList();
                    string folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.ImagesSaudaMappingwithCallRecording);
                    string mediapath = Config.MobileImagePath + Path.Combine(ConfigurationManager.AppSettings["UploadMediaPaths"], folderName);

                    if (saudaDetails.ImagePaths.IsAny())
                    {
                        saudaDetails.ImagePaths = saudaDetails.ImagePaths.Select(filename => Path.Combine(mediapath, filename)).ToList();
                    }
                }


                var saudaOrders = new List<SaudaOrderDetails>();
                //var saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id);

                //if (inputDto.SalesOrganizationId > 0 && inputDto.DistributionChannelId > 0 && inputDto.DivisionId > 0)
                //{
                //    saudaOrderContext = saudaOrderContext.Where(_ => _.SalesOrganizationId == inputDto.SalesOrganizationId
                //    && _.DistributionChannelId == inputDto.DistributionChannelId
                //    && _.DivisionId == inputDto.DivisionId);
                //}

                var skuIds = saudaOrderContext.Select(_ => _.SkuId).ToList();
                var plantIds = saudaOrderContext.Select(_ => _.PlantId).ToList();
                var skuContext = _emamiContext.Skus.AsNoTracking().Where(_ => skuIds.Contains(_.Id));
                var plantContext = _emamiContext.Depots.AsNoTracking().Where(_ => plantIds.Contains(_.Id));

                var saudaorderList = saudaOrderContext.ToList();
                foreach (var order in saudaorderList)
                {
                    var saudaOrderItem = new SaudaOrderDetails
                    {
                        SaudaId = order.SaudaId,
                        SaudaOrderId = order.Id,
                        SkuId = order.SkuId,
                        SkuName = skuContext.FirstOrDefault(_ => _.Id == order.SkuId).SkuName,
                        BidPrice = order.BidPrice,
                        BidQuantity = order.BidQuantity,
                        BidQuantityCases = order.BidQuantityCase,
                        IncoTerms = order.Incoterms1,
                        Discount = order.DiscountAmount,
                        PlantDepot = plantContext.FirstOrDefault(_ => _.Id == order.PlantId)?.Name,
                        //FrieghtRoute = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == order.DealerLocationId).Name,
                        DiscountTypeId = order.DiscountTypeId,
                        StatusId = order.StatusId,
                        //Status = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == order.StatusId).Name,
                        Status = order.StatusId == (int)DTO.Enums.Status.Pending ? Constants.Accepted : _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == order.StatusId).Name,
                        SaudaNumber = order.SaudaNumber != null ? order.SaudaNumber : string.Empty,
                        BidPricePerCase = order.BidPrice / order.BidQuantityCase
                    };
                    saudaOrders.Add(saudaOrderItem);
                }
                saudaDetails.SaudaOrders = saudaOrders;

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

        #region Sauda Conversion
        public ResultDto AddSaudaConversionOrders(SaudaConversionAddDto saudaConversionAddDto)
        {
            _methodName = "AddSaudaConversionOrders";
            try
            {
                var errorMessageList = string.Empty;
                var errorFlag = true;
                if (saudaConversionAddDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (saudaConversionAddDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaConversionAddDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (saudaConversionAddDto.SaudaId == 0)
                {
                    return _resultService.ErrorMessage(Constants.SaudaMissing);
                }
                var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == saudaConversionAddDto.SaudaId && _.StatusId == (int)DTO.Enums.Status.Approved);
                if (saudaOrderContext == null)
                {
                    return _resultService.ErrorMessage(Constants.SaudaNotFound);
                }
                var saudaConversionContext = _emamiContext.SaudaConversion.FirstOrDefault(_ => _.SaudaOrderId == saudaConversionAddDto.SaudaId);
                if (saudaConversionContext != null && saudaConversionContext.IsConversion)
                {
                    return _resultService.ErrorMessage(Constants.SaudaAlreadyConverted);
                }
                else
                {
                    if (saudaConversionAddDto.SaudaConversionOrders != null && saudaConversionAddDto.SaudaConversionOrders.Any())
                    {
                        //if (saudaOrderContext != null)
                        //{

                        //    decimal availableQuantity = saudaOrderContext.BidQuantity;
                        //    IQueryable<SaudaOrderLiftingRequestMapping> liftingReqOrderContextList = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => _.SaudaOrderId == saudaOrderContext.Id);
                        //    if (liftingReqOrderContextList != null && liftingReqOrderContextList.Any())
                        //    {
                        //        availableQuantity = saudaOrderContext.BidQuantity - liftingReqOrderContextList.Sum(_ => _.LiftingQuantity);
                        //    }
                        //    decimal conversionQuantityMT = 0;
                        //    foreach (var saudaConversionOrder in saudaConversionAddDto.SaudaConversionOrders)
                        //    {
                        //        conversionQuantityMT += _resultService.ConvertCasetoMetricTon(saudaConversionOrder.BidQuantityCase, saudaConversionOrder.SkuId);
                        //    }

                        //    if (availableQuantity != conversionQuantityMT)
                        //    {
                        //        return _resultService.ErrorMessage(Constants.SaudaConversionQuantityMismatch);
                        //    }
                        //}

                        foreach (var saudaConversionOrder in saudaConversionAddDto.SaudaConversionOrders)
                        {
                            var errorMessage = string.Empty;
                            if (saudaConversionOrder.SkuId == 0)
                            {
                                errorMessage = Constants.SKUMissing;
                                errorFlag = false;
                            }
                            else
                            {
                                var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == saudaConversionOrder.SkuId);
                                if (skuContext == null)
                                {
                                    errorMessage = Constants.BindErrorMessage(Constants.SKUNotFound, errorMessage);
                                    errorFlag = false;
                                }
                                else
                                {
                                    errorMessage = skuContext.SkuName;
                                    if (saudaConversionOrder.OilTypeId == 0)
                                    {
                                        errorMessage = Constants.BindErrorMessage(Constants.OilTypeMissing, errorMessage);
                                        errorFlag = false;
                                    }
                                    else
                                    {
                                        var oilTypeContext = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == saudaConversionOrder.OilTypeId);
                                        if (oilTypeContext == null)
                                        {
                                            errorMessage = Constants.BindErrorMessage(Constants.OilTypeNotFound, errorMessage);
                                            errorFlag = false;
                                        }
                                    }
                                    if (saudaConversionOrder.BidQuantityCase == 0)
                                    {
                                        errorMessage = Constants.BindErrorMessage(Constants.QuantityEmpty, errorMessage);
                                        errorFlag = false;
                                    }
                                }
                            }
                            if (!errorFlag)
                            {
                                if (!string.IsNullOrEmpty(errorMessageList))
                                {
                                    errorMessageList = Constants.BindErrorMessage(System.Environment.NewLine + errorMessage, errorMessageList);
                                }
                                else
                                {
                                    errorMessageList = Constants.BindErrorMessage(errorMessage, errorMessageList);
                                }
                            }
                        }
                        if (errorFlag)
                        {
                            if (saudaConversionContext != null)
                            {
                                saudaConversionContext.StatusId = (int)DTO.Enums.Status.Pending;
                                saudaConversionContext.IsConversion = true;
                                saudaConversionContext.ModifiedBy = saudaConversionAddDto.LoginUserId;
                                saudaConversionContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                            }
                            else
                            {
                                saudaConversionContext = new SaudaConversion()
                                {
                                    SaudaOrderId = saudaConversionAddDto.SaudaId,
                                    DealerId = saudaOrderContext.Sauda != null ? saudaOrderContext.Sauda.UserId : 0,
                                    StatusId = (int)DTO.Enums.Status.Pending,
                                    IsConversion = true,
                                    CreatedBy = saudaConversionAddDto.LoginUserId,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                };
                                _emamiContext.SaudaConversion.Add(saudaConversionContext);
                            }
                            _emamiContext.SaveChanges();
                            var sauda = _emamiContext.SaudaConversion.Include(_ => _.SaudaOrder).Include(_ => _.SaudaOrder.Sku).FirstOrDefault(f => f.SaudaOrderId == saudaConversionContext.SaudaOrderId);

                            foreach (var saudaConversionOrder in saudaConversionAddDto.SaudaConversionOrders)
                            {

                                var saudaConversionOrderContext = new SaudaConversionOrder()
                                {
                                    SaudaConversionId = saudaConversionContext.Id,
                                    SaudaId = saudaConversionContext.SaudaOrderId,
                                    OilTypeId = saudaConversionOrder.OilTypeId,
                                    SkuId = saudaConversionOrder.SkuId,
                                    BidQuantityCase = saudaConversionOrder.BidQuantityCase,
                                    BidQuantity = _resultService.ConvertCasetoMetricTon(saudaConversionOrder.BidQuantityCase, saudaConversionOrder.SkuId),
                                    CreatedBy = saudaConversionAddDto.LoginUserId,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                };
                                _emamiContext.SaudaConversionOrder.Add(saudaConversionOrderContext);

                            }
                            _emamiContext.SaveChanges();

                            List<string> newSkuNameList = _emamiContext.SaudaConversionOrder.Where(w => w.SaudaConversionId == saudaConversionContext.Id && w.Sku != null).Select(_ => _.Sku.SkuName).DefaultIfEmpty("").ToList();
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
                            if (saudaConversionContext != null && sauda.SaudaOrder != null && sauda.SaudaOrder.Sku != null)
                            {
                                oldSku = saudaConversionContext.SaudaOrder.Sku.SkuName;
                            }

                            var usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == saudaConversionAddDto.LoginUserId || _.Id == saudaOrderContext.Sauda.UserId);
                            if (usersContext != null && usersContext.Any() && saudaOrderContext != null)
                            {
                                bool isEmail = false;
                                var DealerNotificationContext = _emamiContext.TPNotification.AsNoTracking().
                                                                Join(_emamiContext.TPNotificationDetails.AsNoTracking(), TPN => TPN.Id, TPND => TPND.TPNotificationId, (TPN, TPND) => new { TPN, TPND })
                                                                .Where(_ => _.TPND.DealerId == saudaOrderContext.Sauda.UserId && _.TPND.NotificationActionId == (int)DTO.Enums.NotificationActionTP.SaudaConversionRequest && _.TPND.IsActive).ToList();

                                var DealerEmailNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.Email);
                                if (DealerEmailNotificationEnabled != null && DealerEmailNotificationEnabled.Any())
                                    isEmail = true;
                                else
                                    isEmail = false;

                                List<string> toUsers = new List<string>();
                                var createdBy = usersContext.FirstOrDefault(_ => _.Id == saudaConversionAddDto.LoginUserId);
                                var dealer = usersContext.FirstOrDefault(_ => _.Id == saudaOrderContext.Sauda.UserId);
                                if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
                                {
                                    toUsers.Add(createdBy.Email);
                                }
                                if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                                {
                                    toUsers.Add(dealer.Email);
                                }
                                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                if (isEmail && toUsers != null && toUsers.Any())
                                {
                                    var fromEmail = Constants.FromEmail;
                                    var emailSubject = Constants.SaudaConversionRequestSubject;
                                    var plainText = string.Empty;
                                    var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaConversionRequestEmail);
                                    if (emailTemplate != null)
                                    {
                                        var plainTemplate = emailTemplate.PlainTemplate
                                            .Replace(Constants.SkuOld, oldSku)
                                            .Replace(Constants.SkuNew, newSku)
                                            .Replace(Constants.CustomerName, dealer.Name);
                                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                        amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                    }

                                }
                                bool isSms = false;
                                var DealerSMSNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.SMS);
                                if (DealerSMSNotificationEnabled != null && DealerSMSNotificationEnabled.Any())
                                    isSms = true;
                                else
                                    isSms = false;
                                if (isSms)
                                {
                                    var smsPlainTemplate = string.Empty;
                                    var smsMessage = string.Empty;
                                    EmailTemplate smsTemplate = new EmailTemplate();
                                    smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaConversionRequestSMS);
                                    if (smsTemplate != null)
                                    {
                                        smsPlainTemplate = smsTemplate.PlainTemplate
                                            .Replace(Constants.SkuOld, oldSku)
                                            .Replace(Constants.SkuNew, newSku)
                                            .Replace(Constants.CustomerName, dealer.Name);
                                        smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                        if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                                        {
                                            amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber);
                                        }
                                        if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                        {
                                            amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        return _resultService.ErrorMessage(Constants.RecordNotFound);
                    }
                }
                if (!errorFlag)
                {
                    return _resultService.ErrorMessage(errorMessageList);
                }
                else
                {
                    return _resultService.SuccessMessage(Constants.SaudaConversionSuccess);
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetSaudaConversionList(SaudaFilterDto saudaFilterDto)
        {
            _methodName = "GetSaudaConversionList";

            try
            {
                var saudaConversionListDto = new List<SaudaShortViewDto>();
                if (saudaFilterDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (saudaFilterDto.UserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaFilterDto.UserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                IQueryable<UserCustomerMapping> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == saudaFilterDto.UserId);
                if (dealersList != null && dealersList.Any())
                {
                    IQueryable<SaudaConversionOrder> saudaConvOrderListContext = _emamiContext.SaudaConversionOrder.AsNoTracking().Where(_ => _.SaudaConversion != null && dealersList.Any(a => a.CustomerId == _.SaudaConversion.DealerId));
                    if (saudaConvOrderListContext != null && saudaConvOrderListContext.Any())
                    {
                        saudaConversionListDto = saudaConvOrderListContext.GroupBy(_ => _.SaudaConversionId).Select(_ => new SaudaShortViewDto
                        {
                            SaudaConversionId = _.FirstOrDefault().SaudaConversionId,
                            SaudaId = _.FirstOrDefault().SaudaId,
                            SaudaOrderId = _.FirstOrDefault().SaudaId,
                            BookedDate = _.FirstOrDefault().SaudaConversion != null ? _.FirstOrDefault().SaudaConversion.CreatedDate : DateTime.MinValue,
                            TotalQuantity = _.Sum(s => s.BidQuantityCase),
                            StatusId = _.FirstOrDefault().SaudaConversion != null ? _.FirstOrDefault().SaudaConversion.StatusId : 0,
                            StatusName = _.FirstOrDefault().SaudaConversion != null && _.FirstOrDefault().SaudaConversion.Status != null ? _.FirstOrDefault().SaudaConversion.Status.Name : string.Empty,
                            DealerId = _.FirstOrDefault().SaudaConversion != null ? _.FirstOrDefault().SaudaConversion.DealerId : 0,
                            DealerName = _.FirstOrDefault().SaudaConversion != null && _.FirstOrDefault().SaudaConversion.Dealer != null ? _.FirstOrDefault().SaudaConversion.Dealer.Name : string.Empty,
                            OilTypes = _.GroupBy(g => g.OilTypeId).Select(s => new SpecialRateOilTypeDto
                            {
                                OilTypeId = s.FirstOrDefault().OilTypeId,
                                OilTypeName = s.FirstOrDefault().OilType != null ? s.FirstOrDefault().OilType.Name : string.Empty,
                                SkuCount = s.Count(),
                            }).ToList(),
                        }).ToList();

                        foreach (var saudaConversion in saudaConversionListDto)
                        {
                            var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == saudaConversion.SaudaOrderId);
                            if (saudaOrderContext != null)
                            {
                                saudaConversion.TotalAmount = saudaOrderContext.BidPrice;
                            }
                        }
                    }
                }
                if (saudaConversionListDto != null && saudaConversionListDto.Any())
                {
                    return _resultService.SuccessObject(saudaConversionListDto);
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
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetSaudaConversionDetails(SaudaConversionDetailInputDto inputDto)
        {
            _methodName = "GetSaudaConversionDetails";
            try
            {
                var saudaConversionDetails = new SaudaConversionDetailDto();
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.UserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.UserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                //var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                //if (userRoleContext == null)
                //{
                //    return _resultService.ErrorMessage(Constants.UserNotFound);
                //}

                var saudaConversionContext = _emamiContext.SaudaConversion.FirstOrDefault(_ => _.Id == inputDto.SaudaConversionId);
                if (saudaConversionContext != null)
                {
                    saudaConversionDetails = new SaudaConversionDetailDto()
                    {
                        SaudaConversionId = saudaConversionContext.Id,
                        SaudaId = saudaConversionContext.SaudaOrderId,
                        SaudaOrderId = saudaConversionContext.SaudaOrderId,
                        DealerId = saudaConversionContext.DealerId,
                        ConversionDate = saudaConversionContext.CreatedDate,
                        ExtendToDate = saudaConversionContext.ExtendToDate,
                    };
                    if (saudaConversionContext.SaudaOrder != null && saudaConversionContext.SaudaOrder.Sauda != null)
                    {
                        saudaConversionDetails.BookedDate = saudaConversionContext.SaudaOrder.Sauda.CreatedDate;
                    }
                    var dealerContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == saudaConversionContext.DealerId);
                    if (dealerContext != null)
                    {
                        saudaConversionDetails.DealerName = dealerContext.Name;
                    }
                    var saudaOrderContext = _emamiContext.SaudaOrders.FirstOrDefault(_ => _.Id == saudaConversionDetails.SaudaOrderId);
                    if (saudaOrderContext != null)
                    {
                        saudaConversionDetails.SaudaNumber = saudaOrderContext.SaudaNumber;
                    }
                    saudaConversionDetails.Remarks = _emamiContext.Remarks.AsNoTracking().ToList().LastOrDefault(_ => _.TableId == saudaConversionContext.Id)?.Description;
                    //if (remarks!=null && remarks.Any())
                    //{
                    //    saudaConversionDetails.Remarks = remarks.LastOrDefault().Description;
                    //}
                    if (inputDto.isConversion == true && saudaConversionContext.IsConversion == true)
                    {
                        IQueryable<SaudaConversionOrder> saudaConversionOrderContextList = _emamiContext.SaudaConversionOrder.AsNoTracking().Where(_ => _.SaudaConversionId == inputDto.SaudaConversionId);
                        if (saudaConversionOrderContextList != null && saudaConversionOrderContextList.Any())
                        {
                            saudaConversionDetails.TotalQuantity = saudaConversionOrderContextList.Sum(_ => _.BidQuantity);
                            saudaConversionDetails.TotalQuantityCase = saudaConversionOrderContextList.Sum(_ => _.BidQuantityCase);
                            saudaConversionDetails.SaudaConversionOrders = saudaConversionOrderContextList.Select(_ => new SaudaOrderDetails
                            {
                                SkuId = _.SkuId,
                                SkuName = _.Sku != null ? _.Sku.SkuName : string.Empty,
                                BidQuantity = _.BidQuantity,
                                BidQuantityCases = _.BidQuantityCase,
                                BidPrice = _.BidPrice,
                                BidPricePerCase = Math.Round((_.BidPrice != 0 && _.BidQuantityCase != 0 ? (_.BidPrice / _.BidQuantityCase) : 0), 2),
                            }).ToList();
                        }
                    }
                    else if (inputDto.isConversion == false && saudaConversionContext.IsExtension == true)
                    {
                        IQueryable<SaudaOrder> saudaOrderContextList = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == saudaConversionContext.SaudaOrderId);
                        if (saudaOrderContextList != null && saudaOrderContextList.Any())
                        {
                            saudaConversionDetails.TotalQuantity = saudaOrderContextList.Sum(_ => _.BidQuantity);
                            saudaConversionDetails.TotalQuantityCase = saudaOrderContextList.Sum(_ => _.BidQuantityCase);
                            saudaConversionDetails.SaudaConversionOrders = saudaOrderContextList.Select(_ => new SaudaOrderDetails
                            {
                                SkuId = _.SkuId,
                                SkuName = _.Sku != null ? _.Sku.SkuName : string.Empty,
                                BidQuantity = _.BidQuantity,
                                BidQuantityCases = _.BidQuantityCase,
                                BidPrice = _.BidPrice,
                                BidPricePerCase = Math.Round((_.BidPrice != 0 && _.BidQuantityCase != 0 ? (_.BidPrice / _.BidQuantityCase) : 0), 2),
                            }).ToList();
                        }
                    }
                    return _resultService.SuccessObject(saudaConversionDetails);
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
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto AddSaudaConversionUnitAndDifferenceRate(SaudaConversionUnitAndDifferenceRateAddDto inputDto)
        {
            _methodName = "AddSaudaConversionUnitAndDifferenceRate";
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (inputDto.FromSkuId == 0)
                {
                    return _resultService.ErrorMessage(Constants.FromSkuNotFound);
                }
                if (inputDto.FromUnit == 0)
                {
                    return _resultService.ErrorMessage(Constants.FromUnitNotFound);
                }
                if (inputDto.FromPackGroupId == 0)
                {
                    return _resultService.ErrorMessage(Constants.FromPackGroupNotFound);
                }
                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }
                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }
                if (inputDto.StateIds == null)
                {
                    return _resultService.ErrorMessage(Constants.StateEmpty);
                }
                if (inputDto.SourceIds == null)
                {
                    return _resultService.ErrorMessage(Constants.PlantOrDepotEmpty);
                }
                if (inputDto.SaudaConversionUnitAndDifferenceRateDetailsList == null || !inputDto.SaudaConversionUnitAndDifferenceRateDetailsList.Any())
                {
                    return _resultService.ErrorMessage(Constants.ToSkuList);
                }

                var existingFromSku = _emamiContext.SaudaConversionUnitAndDifferenceRates.AsNoTracking()
                    .Where(_ => _.FromSkuId == inputDto.FromSkuId && _.FromPackGroupId == inputDto.FromPackGroupId && inputDto.SourceIds.Contains(_.SourceId) && inputDto.StateIds.Contains(_.StateId) &&
                    ((DbFunctions.TruncateTime(_.FromDate) <= DbFunctions.TruncateTime(inputDto.FromDate) && DbFunctions.TruncateTime(inputDto.FromDate) <= DbFunctions.TruncateTime(_.ToDate)) ||
                    (DbFunctions.TruncateTime(_.FromDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && DbFunctions.TruncateTime(inputDto.ToDate) <= DbFunctions.TruncateTime(_.ToDate))))
                    .Select(_ => _.Id).ToList();

                var saudaConversionUnitAndDifferenceRateContext = new SaudaConversionUnitAndDifferenceRate();
                foreach (var state in inputDto.StateIds)
                {
                    foreach (var source in inputDto.SourceIds)
                    {
                        saudaConversionUnitAndDifferenceRateContext = new SaudaConversionUnitAndDifferenceRate
                        {
                            FromDate = inputDto.FromDate,
                            ToDate = inputDto.ToDate,
                            FromPackGroupId = inputDto.FromPackGroupId,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            FromSkuId = inputDto.FromSkuId,
                            FromUnit = inputDto.FromUnit,
                            SourceId = source,
                            StateId = state
                        };
                        _emamiContext.SaudaConversionUnitAndDifferenceRates.Add(saudaConversionUnitAndDifferenceRateContext);
                        _emamiContext.SaveChanges();
                        var saudaConversionUnitAndDifferenceRateDetailList = new List<SaudaConversionUnitAndDifferenceRateDetail>();
                        foreach (var item in inputDto.SaudaConversionUnitAndDifferenceRateDetailsList)
                        {
                            var Errorflag = true;
                            if (item.ToSkuId == 0)
                            {
                                Errorflag = false;
                            }
                            if (item.ToUnit == 0)
                            {
                                Errorflag = false;
                            }
                            if (item.ToPackGroupId == 0)
                            {
                                Errorflag = false;
                            }
                            if (item.BasicRate == 0)
                            {
                                Errorflag = false;
                            }
                            if (Errorflag)
                            {
                                var ExistingConversionDetail = _emamiContext.SaudaConversionUnitAndDifferenceRateDetails
                                    .Where(_ => existingFromSku.Contains(_.SaudaConversionUnitAndDifferenceRateId) && _.ToSkuId == item.ToSkuId && _.IsActive);

                                if (ExistingConversionDetail != null)
                                {
                                    foreach (var ToSkuitem in ExistingConversionDetail)
                                    {
                                        ToSkuitem.IsActive = false;
                                        ToSkuitem.ModifiedBy = inputDto.LoginUserId;
                                        ToSkuitem.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                    }

                                }

                                var SaudaConversionUnitAndDifferenceRateDetailContext = new SaudaConversionUnitAndDifferenceRateDetail
                                {
                                    ToUnit = item.ToUnit,
                                    BasicRate = item.BasicRate,
                                    ToPackGroupId = item.ToPackGroupId,
                                    CreatedBy = inputDto.LoginUserId,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                    ToSkuId = item.ToSkuId,
                                    IsActive = true,
                                    SaudaConversionUnitAndDifferenceRateId = saudaConversionUnitAndDifferenceRateContext.Id,
                                };
                                saudaConversionUnitAndDifferenceRateDetailList.Add(SaudaConversionUnitAndDifferenceRateDetailContext);
                            }
                        }
                        _emamiContext.BulkInsertProxy(saudaConversionUnitAndDifferenceRateDetailList);
                    }
                }
                _emamiContext.SaveChanges();

                return _resultService.SuccessObject(saudaConversionUnitAndDifferenceRateContext.Id);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }
        #endregion

        #region Sauda Extension
        public ResultDto AddSaudaExtension(SaudaExtensionAddDto saudaExtensionAddDto)
        {
            _methodName = "AddSaudaExtension";
            try
            {
                if (saudaExtensionAddDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (saudaExtensionAddDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaExtensionAddDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (saudaExtensionAddDto.SaudaId == 0)
                {
                    return _resultService.ErrorMessage(Constants.SaudaMissing);
                }
                var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == saudaExtensionAddDto.SaudaId);
                if (saudaOrderContext == null)
                {
                    return _resultService.ErrorMessage(Constants.SaudaNotFound);
                }
                var saudaConversionContext = _emamiContext.SaudaConversion.FirstOrDefault(_ => _.SaudaOrderId == saudaExtensionAddDto.SaudaId);
                if (saudaConversionContext != null && saudaConversionContext.IsExtension)
                {
                    return _resultService.ErrorMessage(Constants.SaudaAlreadyExtended);
                }
                else
                {
                    if (saudaExtensionAddDto.ExtendToDate == DateTime.MinValue)
                    {
                        return _resultService.ErrorMessage(Constants.SaudaExtendToDateMissing);
                    }
                    if (saudaConversionContext != null)
                    {
                        saudaConversionContext.ExtensionStatusId = (int)DTO.Enums.Status.Pending;
                        saudaConversionContext.IsExtension = true;
                        saudaConversionContext.ExpiryDate = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.Id) != null ?
                            _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.Id).ValidToDate : DateTime.MinValue;
                        saudaConversionContext.ExtendToDate = saudaExtensionAddDto.ExtendToDate;
                        saudaConversionContext.ModifiedBy = saudaExtensionAddDto.LoginUserId;
                        saudaConversionContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    }
                    else
                    {
                        var saudaConversion = new SaudaConversion()
                        {
                            SaudaOrderId = saudaExtensionAddDto.SaudaId,
                            DealerId = saudaOrderContext.Sauda != null ? saudaOrderContext.Sauda.UserId : 0,
                            ExpiryDate = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.Id) != null ?
                        _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.Id).ValidToDate : DateTime.MinValue,
                            ExtendToDate = saudaExtensionAddDto.ExtendToDate,
                            ExtensionStatusId = (int)DTO.Enums.Status.Pending,
                            IsExtension = true,
                            CreatedBy = saudaExtensionAddDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),

                        };
                        _emamiContext.SaudaConversion.Add(saudaConversion);
                    }
                    _emamiContext.SaveChanges();

                    if (saudaOrderContext.ValidToDate != null && saudaExtensionAddDto.ExtendToDate != null && saudaOrderContext.ValidToDate != DateTime.MinValue && saudaExtensionAddDto.ExtendToDate != DateTime.MinValue)
                    {
                        string noOfDays = saudaExtensionAddDto.ExtendToDate.Date.Subtract(saudaOrderContext.ValidToDate.Date).TotalDays.ToString();
                        var DealerId = saudaOrderContext.Sauda != null ? saudaOrderContext.Sauda.UserId : 0;
                        var usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == saudaExtensionAddDto.LoginUserId || _.Id == DealerId);
                        if (usersContext != null && usersContext.Any() && saudaOrderContext != null)
                        {
                            bool isEmail = false;
                            var DealerNotificationContext = _emamiContext.TPNotification.AsNoTracking().
                                                            Join(_emamiContext.TPNotificationDetails.AsNoTracking(), TPN => TPN.Id, TPND => TPND.TPNotificationId, (TPN, TPND) => new { TPN, TPND })
                                                            .Where(_ => _.TPND.DealerId == saudaOrderContext.Sauda.UserId && _.TPND.NotificationActionId == (int)DTO.Enums.NotificationActionTP.SaudaExtensionRequest && _.TPND.IsActive).ToList();

                            var DealerEmailNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.Email);
                            if (DealerEmailNotificationEnabled != null && DealerEmailNotificationEnabled.Any())
                                isEmail = true;
                            else
                                isEmail = false;
                            List<string> toUsers = new List<string>();
                            var createdBy = usersContext.FirstOrDefault(_ => _.Id == saudaExtensionAddDto.LoginUserId);
                            var dealer = usersContext.FirstOrDefault(_ => _.Id == DealerId);
                            if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
                            {
                                toUsers.Add(createdBy.Email);
                            }
                            if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                            {
                                toUsers.Add(dealer.Email);
                            }
                            AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                            if (isEmail && toUsers != null && toUsers.Any())
                            {
                                var fromEmail = Constants.FromEmail;
                                var emailSubject = Constants.SaudaExtensionRequestSubject;
                                var plainText = string.Empty;
                                var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaExtensionRequestNotificationEmail);
                                if (emailTemplate != null)
                                {
                                    var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.NoOfDays, noOfDays.ToString())
                                                       .Replace(Constants.CustomerName, dealer.Name);
                                    var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                    amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                }

                            }

                            bool isSms = false;
                            var DealerSMSNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.SMS);
                            if (DealerSMSNotificationEnabled != null && DealerSMSNotificationEnabled.Any())
                                isSms = true;
                            else
                                isSms = false;
                            if (isSms)
                            {
                                var smsPlainTemplate = string.Empty;
                                var smsMessage = string.Empty;
                                EmailTemplate smsTemplate = new EmailTemplate();
                                smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaExtensionRequestNotificationSMS);
                                if (smsTemplate != null)
                                {
                                    smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.NoOfDays, noOfDays.ToString())
                                                       .Replace(Constants.CustomerName, dealer.Name);
                                    smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                    try
                                    {
                                        if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                                        {
                                            amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber, smsTemplate.SMSTemplateID);
                                        }
                                        if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                        {
                                            amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber, smsTemplate.SMSTemplateID);
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                        }
                    }

                    return _resultService.SuccessMessage(Constants.SaudaExtensionSuccess);
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetSaudaExtensionList(SaudaFilterDto saudaFilterDto)
        {
            _methodName = "GetSaudaExtensionList";

            try
            {
                var saudaExtensionListDto = new List<SaudaExtensionListDto>();
                if (saudaFilterDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (saudaFilterDto.UserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaFilterDto.UserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                IQueryable<UserCustomerMapping> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == saudaFilterDto.UserId);
                if (dealersList != null && dealersList.Any())
                {
                    var saudaConversionListContext = _emamiContext.SaudaConversion.AsNoTracking()
                        .Join(_emamiContext.SaudaOrders.AsNoTracking(), sc => sc.SaudaOrderId, so => so.Id, (sc, so) => new { sc, so })
                        .Where(_ => dealersList.Any(a => a.CustomerId == _.sc.DealerId)
                          && _.sc.IsExtension == true);
                    if (saudaConversionListContext != null && saudaConversionListContext.Any())
                    {
                        saudaExtensionListDto = saudaConversionListContext.Select(_ => new SaudaExtensionListDto
                        {
                            SaudaConversionId = _.sc != null ? _.sc.Id : 0,
                            SaudaId = _.sc != null ? _.sc.SaudaOrderId : 0,
                            SaudaOrderId = _.sc != null ? _.sc.SaudaOrderId : 0,
                            SaudaNumber = _.so != null ? _.so.SaudaNumber : string.Empty,
                            ExpiryDate = _.sc != null ? _.sc.ExpiryDate : DateTime.MinValue,
                            ExtendToDate = _.sc != null ? _.sc.ExtendToDate : DateTime.MinValue,
                            StatusId = _.sc != null ? _.sc.ExtensionStatusId : 0,
                            StatusName = _.sc != null && _.sc.ExtensionStatusId != null ? _.sc.ExtensionStatus.Name : string.Empty,
                            DealerId = _.sc != null ? _.sc.DealerId : 0,
                            DealerName = _.sc != null && _.sc.Dealer != null ? _.sc.Dealer.Name : string.Empty,
                        }).Distinct().ToList();
                    }
                }
                if (saudaExtensionListDto != null && saudaExtensionListDto.Any())
                {
                    return _resultService.SuccessObject(saudaExtensionListDto);
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
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }
        #endregion

        #region Counter Bid
        //public ResultDto GetSaudaCounterBidDetails(SaudaDetailInputDto inputDto)
        //{
        //    _methodName = "GetSaudaCounterBidDetails";
        //    try
        //    {
        //        var saudaOrderDetails = new SaudaOrderDetails();
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }

        //        if (inputDto.UserId == 0)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidUser);
        //        }

        //        var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.UserId);
        //        if (userContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserNotFound);
        //        }
        //        var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
        //        if (userRoleContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserNotFound);
        //        }

        //        var dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == inputDto.UserId);
        //        if (dealersList != null && dealersList.Any())
        //        {
        //            SaudaOrder saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SaudaOrderId && _.Sauda != null && dealersList.Any(a => a.CustomerId == _.Sauda.UserId));
        //            if (saudaOrderContext != null)
        //            {
        //                if (saudaOrderContext.Sauda != null)
        //                {
        //                    var dealerContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == saudaOrderContext.Sauda.UserId);
        //                    if (dealerContext != null)
        //                    {
        //                        saudaOrderDetails.DealerId = dealerContext.Id;
        //                        saudaOrderDetails.DealerName = dealerContext.Name;
        //                    }
        //                    saudaOrderDetails.BookedDate = saudaOrderContext.Sauda.BiddingDate;
        //                }
        //                saudaOrderDetails.SaudaId = saudaOrderContext.Id;
        //                saudaOrderDetails.SaudaOrderId = saudaOrderContext.Id;
        //                saudaOrderDetails.SaudaNumber = saudaOrderContext.SaudaNumber;
        //                saudaOrderDetails.ValidToDate = saudaOrderContext.ValidToDate;
        //                saudaOrderDetails.OilTypeId = saudaOrderContext.OilTypeId;
        //                saudaOrderDetails.OilTypeName = saudaOrderContext.OilType != null ? saudaOrderContext.OilType.Name : string.Empty;
        //                saudaOrderDetails.SkuId = saudaOrderContext.SkuId;
        //                saudaOrderDetails.SkuName = saudaOrderContext.Sku != null ? saudaOrderContext.Sku.SkuName : string.Empty;
        //                saudaOrderDetails.StatusId = saudaOrderContext.StatusId;
        //                var statusContext = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.StatusId);
        //                if (statusContext != null)
        //                {
        //                    saudaOrderDetails.Status = statusContext.Name;
        //                }
        //                IQueryable<SaudaOrderLiftingRequestMapping> liftingReqOrderContextList = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => _.SaudaOrderId == saudaOrderContext.Id
        //                    && _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected);
        //                if (liftingReqOrderContextList != null && liftingReqOrderContextList.Any())
        //                {
        //                    saudaOrderDetails.BidQuantity = saudaOrderContext.BidQuantity - liftingReqOrderContextList.Sum(_ => _.LiftingQuantity);
        //                    saudaOrderDetails.BidQuantityCases = saudaOrderContext.BidQuantityCase - liftingReqOrderContextList.Sum(_ => _.LiftingQuantityCase);
        //                }
        //                else
        //                {
        //                    saudaOrderDetails.BidQuantity = saudaOrderContext.BidQuantity;
        //                    saudaOrderDetails.BidQuantityCases = saudaOrderContext.BidQuantityCase;
        //                }
        //                saudaOrderDetails.BidPrice = saudaOrderContext.BidPrice;
        //                saudaOrderDetails.BidPricePerCase = Math.Round((saudaOrderContext.BidPrice != 0 && saudaOrderContext.BidQuantityCase != 0 ? (saudaOrderContext.BidPrice / saudaOrderContext.BidQuantityCase) : 0), 2);
        //                saudaOrderDetails.IncoTerms = saudaOrderContext.Incoterms1;
        //                var plantContext = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.PlantId);
        //                if (plantContext != null)
        //                {
        //                    saudaOrderDetails.PlantDepot = plantContext.Name;
        //                }
        //                //var freightRouteContext = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.DealerLocationId);
        //                //if (freightRouteContext != null)
        //                //{
        //                //    saudaOrderDetails.FrieghtRoute = freightRouteContext.Name;
        //                //}
        //                saudaOrderDetails.CounterBidOffer = saudaOrderContext.CounterBidOffer;
        //                saudaOrderDetails.CounterBidOfferDate = saudaOrderContext.CounterBidOfferDate != null ? saudaOrderContext.CounterBidOfferDate.Value : DateTime.MinValue;
        //            }
        //            return _resultService.SuccessObject(saudaOrderDetails);
        //        }
        //        else
        //        {
        //            return _resultService.ErrorMessage(Constants.RecordNotFound);

        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        //public ResultDto ApproveCounterBid(CounterBidInputDto inputDto)
        //{
        //    _methodName = "ApproveCounterBid";
        //    try
        //    {
        //        string responseMessage = string.Empty;
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }
        //        if (inputDto.LoginUserId == 0)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidUser);
        //        }

        //        var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
        //        if (userContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserNotFound);
        //        }
        //        var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
        //        if (userRoleContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserNotFound);
        //        }
        //        IQueryable<UserCustomerMapping> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId);
        //        if (dealersList != null && dealersList.Any())
        //        {
        //            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //            SaudaOrder saudaOrderContext = _emamiContext.SaudaOrders.FirstOrDefault(_ => _.Id == inputDto.SaudaOrderId && _.Sauda != null && dealersList.Any(a => a.CustomerId == _.Sauda.UserId)
        //                && DbFunctions.TruncateTime(_.Sauda.BiddingDate) == DbFunctions.TruncateTime(currentDate) && _.CounterBidOffer != 0 && _.CounterBidOfferDate != null);
        //            if (saudaOrderContext == null)
        //            {
        //                return _resultService.ErrorMessage(Constants.SaudaNotFound);
        //            }
        //            else
        //            {
        //                decimal couterBidOffer = 0;
        //                if (inputDto.IsAccept)
        //                {
        //                    var configContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Id == (int)DTO.Enums.Configuration.CounterBidBufferTime);
        //                    if (configContext != null)
        //                    {
        //                        var bufferTime = TimeSpan.FromMinutes(Convert.ToInt32(configContext.Value));
        //                        var timeLimit = saudaOrderContext.CounterBidOfferDate.Value.TimeOfDay + bufferTime;
        //                        if (timeLimit < currentDate.TimeOfDay)
        //                        {
        //                            return _resultService.ErrorMessage(Constants.CounterBidOfferTimeLimitExceeds);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        return _resultService.ErrorMessage(Constants.RecordNotFound);
        //                    }
        //                    saudaOrderContext.StatusId = (int)DTO.Enums.Status.Pending;
        //                    couterBidOffer = saudaOrderContext.CounterBidOffer;
        //                    saudaOrderContext.CounterBidOffer = saudaOrderContext.BidPrice;
        //                    saudaOrderContext.BidPrice = couterBidOffer;
        //                    saudaOrderContext.ModifiedBy = inputDto.LoginUserId;
        //                    saudaOrderContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //                    _emamiContext.SaveChanges();
        //                    responseMessage = Constants.CounterBidSuccess;

        //                }
        //                else
        //                {
        //                    couterBidOffer = saudaOrderContext.CounterBidOffer;
        //                    saudaOrderContext.StatusId = (int)DTO.Enums.Status.Rejected;
        //                    saudaOrderContext.ModifiedBy = inputDto.LoginUserId;
        //                    saudaOrderContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //                    _emamiContext.SaveChanges();
        //                    responseMessage = Constants.CounterBidReject;
        //                }
        //                try
        //                {
        //                    var usersContext = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == inputDto.LoginUserId || _.Id == saudaOrderContext.Sauda.UserId);
        //                    if (usersContext != null && usersContext.Any() && saudaOrderContext != null)
        //                    {
        //                        List<string> toUsers = new List<string>();
        //                        var createdBy = usersContext.FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
        //                        var dealer = usersContext.FirstOrDefault(_ => _.Id == saudaOrderContext.Sauda.UserId);
        //                        string dealerName = string.Empty;
        //                        if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
        //                        {
        //                            toUsers.Add(createdBy.Email);
        //                        }
        //                        if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
        //                        {
        //                            dealerName = dealer.Name;
        //                            toUsers.Add(dealer.Email);
        //                        }
        //                        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
        //                        string emailSubject = string.Empty;
        //                        if (_resultService.IsEmail() && toUsers != null && toUsers.Any())
        //                        {
        //                            var fromEmail = Constants.FromEmail;
        //                            var plainText = string.Empty;
        //                            EmailTemplate emailTemplate = new EmailTemplate();
        //                            if (inputDto.IsAccept)
        //                            {
        //                                emailSubject = Constants.CounterBidAcceptSubject;
        //                                emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationRAFlowEmail);
        //                            }
        //                            else
        //                            {
        //                                emailSubject = Constants.CounterBidRejectSubject;
        //                                emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderRejectNotificationEmail);
        //                            }

        //                            if (emailTemplate != null)
        //                            {
        //                                var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
        //                                    .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(couterBidOffer, 2)).ToString())
        //                                    .Replace(Constants.BY_FOR, Constants.FOR).Replace(Constants.UserName, dealerName);
        //                                var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
        //                                amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
        //                            }

        //                        }
        //                        var smsPlainTemplate = string.Empty;
        //                        if (_resultService.IsSMS())
        //                        {
        //                            var smsMessage = string.Empty;
        //                            EmailTemplate smsTemplate = new EmailTemplate();
        //                            if (inputDto.IsAccept)
        //                            {
        //                                smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationRAFlowSMS);
        //                            }
        //                            else
        //                            {
        //                                smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderRejectNotificationSMS);
        //                            }
        //                            if (smsTemplate != null)
        //                            {
        //                                smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
        //                                    .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(couterBidOffer, 2)).ToString())
        //                                    .Replace(Constants.BY_FOR, Constants.FOR).Replace(Constants.UserName, dealerName);
        //                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
        //                                try
        //                                {
        //                                    if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
        //                                    {
        //                                        amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber);
        //                                    }
        //                                    if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
        //                                    {
        //                                        amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber);
        //                                    }
        //                                }
        //                                catch (Exception ex)
        //                                {

        //                                }
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
        //                return _resultService.SuccessMessage(responseMessage);
        //            }

        //        }
        //        else
        //        {
        //            return _resultService.ErrorMessage(Constants.DealerNotFound);
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}
        #endregion

        #region Sauda Create


        /// Method to create sauda
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        public ResultDto SaudaCreation(SaudaInputDto inputDto)
        {
            _methodName = "SaudaCreation";
            var resultDto = new ResultDto();
            try
            {
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                List<long> reportingToMappingLimitIdsList = new List<long>();
                List<long> userLimitContextLimitIdsLimitIdsList = new List<long>();
                SpecalityFatDiscountUser reportingToMappingLimitContext = null;
                SpecalityFatDiscountUser userLimitContext = null;
                decimal availableQuantityBdo = 0;
                decimal actualDiscountQuantityBdo = 0;
                decimal orderedQuantityBdo = 0;
                decimal totalQuantityBdo = 0;
                decimal requestedQuantityBdo = 0;
                decimal saudaBidQuantity = 0;
                var stateIsauantity = false;
                var stateNotQuantity = false;
                var IsReportingtoAllocation = false;
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);

                var SaudaLimitContext = (from u in _emamiContext.Users.AsNoTracking().Where(_ => _.Id == inputDto.DealerId)
                                         join udm in _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId && _.DivisionId == inputDto.DivisionId) on u.Id equals udm.UserId
                                         select new { u.SaudaValidityPeriod, udm.SaudaLimit, udm.DivisionId }).ToList();
                var userrolecontext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                if (dealerContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (userrolecontext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var isSalesAreaBookingValid = _resultService.IsSalesAreaBookingValid(inputDto);

                if (!isSalesAreaBookingValid)
                {
                    if (userrolecontext.RoleId == (int)DTO.Enums.Role.ZonalTrader)
                    {
                        return _resultService.ErrorMessage(Constants.SaudaSalesAreaRestricitedZonalTrader);
                    }
                    else
                    {
                        return _resultService.ErrorMessage(Constants.SaudaSalesAreaRestricitedStateTrader);
                    }
                }

                var saudaConditionData = _resultService.IsSaudaConditionalBookingValid(inputDto);
                if (!saudaConditionData.Item1)
                {
                    return _resultService.ErrorMessage(saudaConditionData.Item2);
                }

                decimal TotalQtyInMT = 0;
                foreach (var item in inputDto.SaudaOrders)
                {
                    TotalQtyInMT = TotalQtyInMT + _resultService.ConvertCasetoMetricTon(item.BidQuantity, item.SkuId);
                }

                var statuses = Constants.OverallSaudaStatus;
                var SaudaOutstanding = TotalQtyInMT;
                var usersaudalimit = SaudaLimitContext.Sum(x => x.SaudaLimit ?? 0);
                var SaudaLimit = _resultService.AvailableSaudaLimit(inputDto.DealerId, usersaudalimit, inputDto.SalesOrganizationId, inputDto.DistributionChannelId, inputDto.DivisionId);

                if (SaudaLimit < SaudaOutstanding)
                {
                    return _resultService.ErrorMessage(Constants.SaudaLimitIsExceeds);
                }

                var QuantityLimitForBookingSaudaName = Utility.GetEnumDescription(DTO.Enums.Configuration.QuantityLimitforBookingSaudaEnabled);
                var configurationContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Name == QuantityLimitForBookingSaudaName);
                bool IsQuantityLimitForBookingSauda = Convert.ToBoolean(configurationContext.Value);
                if (inputDto.SaudaOrders != null && inputDto.SaudaOrders.Any())
                {
                    var overallSaudaStatuses = Constants.OverallSaudaStatus;
                    string oilTypeBasedError = string.Empty;
                    string oilTypeExpiredError = string.Empty;
                    string userLimitNotExistError = string.Empty;
                    string oilTypeBasedRestrictionError = string.Empty;
                    var checkedOiltypeIdsForRestrict = new List<long>();

                    foreach (var item in inputDto.SaudaOrders)
                    {
                        requestedQuantityBdo = _resultService.ConvertCasetoMetricTon(item.BidQuantity, item.SkuId);

                        var skusContext = _emamiContext.Skus.AsNoTracking().Where(_ => _.IsActive);
                        var skuContext = skusContext.FirstOrDefault(_ => _.Id == item.SkuId);

                        if (skuContext != null)
                        {
                            var oiltypecontext = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == skuContext.OilTypeId);

                            if (oiltypecontext != null)
                            {
                                userLimitContext = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.OilTypeId == oiltypecontext.Id
                                && _.ParentId == 0 && _.ValidFrom <= currentDate && _.ValidTo >= currentDate && _.SalesOrganizationId == inputDto.SalesOrganizationId
                                && _.DistributionChannelId == inputDto.DistributionChannelId && _.DivisionId == inputDto.DivisionId);

                                reportingToMappingLimitContext = (from s in _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                                                                  join u in _emamiContext.UserReportingToMappings on s.UserId equals u.ReportingToUserId
                                                                  where u.UserId == inputDto.LoginUserId
                                                                  && s.OilTypeId == oiltypecontext.Id
                                                                  //&& s.ParentId == 0
                                                                  && s.ValidFrom <= currentDate
                                                                  && s.ValidTo >= currentDate
                                                                  && s.SalesOrganizationId == inputDto.SalesOrganizationId
                                                                  && s.DistributionChannelId == inputDto.DistributionChannelId
                                                                  && s.DivisionId == inputDto.DivisionId
                                                                  select s).FirstOrDefault();

                                var reportingToMappingLimitIds = (from s in _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                                                                  join u in _emamiContext.UserReportingToMappings on s.UserId equals u.ReportingToUserId
                                                                  where u.UserId == inputDto.LoginUserId
                                                                  && s.OilTypeId == oiltypecontext.Id
                                                                  //&& s.ParentId == 0
                                                                  && s.ValidFrom <= currentDate
                                                                  && s.ValidTo >= currentDate
                                                                  && s.SalesOrganizationId == inputDto.SalesOrganizationId
                                                                  && s.DistributionChannelId == inputDto.DistributionChannelId
                                                                  && s.DivisionId == inputDto.DivisionId
                                                                  select s.Id).FirstOrDefault();

                                reportingToMappingLimitIdsList.Add(reportingToMappingLimitIds);

                                if (userLimitContext != null)
                                {
                                    userLimitContext = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId
                                        && _.OilTypeId == oiltypecontext.Id
                                        && _.ParentId == 0
                                        && _.ValidFrom <= currentDate
                                        && _.ValidTo >= currentDate && _.SalesOrganizationId == inputDto.SalesOrganizationId
                                        && _.DistributionChannelId == inputDto.DistributionChannelId && _.DivisionId == inputDto.DivisionId);

                                    if (userLimitContext != null)
                                    {
                                        userLimitContextLimitIdsLimitIdsList.Add(userLimitContext.Id);
                                    }

                                    stateIsauantity = true;
                                    var dealerlist = new List<long>();
                                    if (userrolecontext.RoleId == (int)DTO.Enums.Role.ZonalTrader)
                                    {
                                        var bdoIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(s => s.UserId).ToList();

                                        if (bdoIds.Any())
                                        {
                                            dealerlist = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                                            .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.u.Id, uc => uc.CustomerId, (x, uc) => new { DealerId = x.u.Id, x.ur, uc })
                                            .Where(_ => bdoIds.Contains(_.uc.UserId) && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.DealerId).ToList();
                                        }
                                    }
                                    else
                                    {
                                        dealerlist = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                                        .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.u.Id, uc => uc.CustomerId, (x, uc) => new { DealerId = x.u.Id, x.ur, uc })
                                        .Where(_ => _.uc.UserId == inputDto.LoginUserId && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.DealerId).ToList();
                                    }

                                    if (dealerlist != null && dealerlist.Any() && currentDate >= userLimitContext.ValidFrom
                                        && currentDate <= userLimitContext.ValidTo)
                                    {
                                        saudaBidQuantity = _resultService.GetSaudaBookedQuantityForCurrentDateByDealersByDateRangeIsNotReportingtoAllocation(inputDto, (long)skuContext.OilTypeId, dealerlist, userLimitContext.ValidFrom, userLimitContext.ValidTo);
                                    }

                                    if (userLimitContext.RequestedDiscount > 0 && userLimitContext.RequestedDiscountDate.HasValue && userLimitContext.RequestedDiscountDate.Value.Date != DateHelper.UtcToIndia(DateTime.UtcNow).Date)
                                    {
                                        orderedQuantityBdo = saudaBidQuantity;
                                        actualDiscountQuantityBdo = userLimitContext.ActualDiscount - orderedQuantityBdo;
                                        var actualAvailableQuantityBdo = actualDiscountQuantityBdo - userLimitContext.RequestedDiscount;
                                        if (actualAvailableQuantityBdo < 0)
                                        {
                                            actualAvailableQuantityBdo = 0;
                                        }
                                        if (actualAvailableQuantityBdo <= requestedQuantityBdo)
                                        {
                                            totalQuantityBdo = requestedQuantityBdo;
                                            if (saudaBidQuantity != 0)
                                            {
                                                orderedQuantityBdo = saudaBidQuantity;
                                                totalQuantityBdo = orderedQuantityBdo + requestedQuantityBdo;
                                            }
                                        }
                                        else
                                        {
                                            oilTypeExpiredError = oilTypeExpiredError + Constants.QuantityLimitExpired.Replace(Constants.OiltypeName, oiltypecontext.Name) + Environment.NewLine;
                                        }
                                    }
                                    else
                                    {
                                        //decimal orderedQuantityBdo = 0;
                                        totalQuantityBdo = requestedQuantityBdo;
                                        if (saudaBidQuantity != 0)
                                        {
                                            orderedQuantityBdo = saudaBidQuantity;
                                            totalQuantityBdo = userLimitContext.ActualDiscount - orderedQuantityBdo;
                                        }
                                        if (totalQuantityBdo > userLimitContext.ActualDiscount || totalQuantityBdo > userLimitContext.RemainingQuantity)// here Actual discount is limit
                                        {
                                            availableQuantityBdo = userLimitContext.ActualDiscount - orderedQuantityBdo;
                                            if (availableQuantityBdo < 0)
                                            {
                                                availableQuantityBdo = 0;
                                            }
                                            oilTypeBasedError = oilTypeBasedError + Constants.OilTypeLimitExceeds.Replace(Constants.OiltypeName, oiltypecontext.Name).Replace(Constants.Quantity, userLimitContext.RemainingQuantity.ToString()) + Environment.NewLine;
                                            //return _resultService.ErrorMessage(Constants.OilTypeBdoLimitExceeds.Replace(Constants.OiltypeName, oiltypecontext.Name).Replace(Constants.Quantity, Math.Round(availableQuantityBdo, 2).ToString()));
                                        }
                                    }
                                }

                                if (userLimitContext == null && reportingToMappingLimitContext != null)
                                {
                                    stateNotQuantity = true;
                                    IsReportingtoAllocation = true;
                                    var dealerlist = new List<long>();
                                    if (userrolecontext.RoleId == (int)DTO.Enums.Role.ZonalTrader)
                                    {
                                        var bdoIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(s => s.UserId).ToList();

                                        if (bdoIds.Any())
                                        {
                                            dealerlist = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                                             .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.u.Id, uc => uc.CustomerId, (x, uc) => new { DealerId = x.u.Id, x.ur, uc })
                                             .Where(_ => bdoIds.Contains(_.uc.UserId) && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.DealerId).ToList();
                                        }
                                    }
                                    else
                                    {
                                        var zoneIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                                            .Join(_emamiContext.UserDivisionMappings.AsNoTracking(), urm => urm.ReportingToUserId, ud => ud.UserId, (urm, ud) => new { urm, ud }).
                                            Where(_ => _.ud.SalesOrganizationId == inputDto.SalesOrganizationId && _.ud.DistributionChannelId == inputDto.DistributionChannelId && _.ud.DivisionId == inputDto.DivisionId)
                                            .Select(_ => _.urm.ReportingToUserId).Distinct().ToList();

                                        if (zoneIds.Any())
                                        {
                                            var stateIds = _emamiContext.UserReportingToMappings.AsNoTracking()
                                                .Where(_ => zoneIds.Contains(_.ReportingToUserId)).Select(s => s.UserId).Distinct().ToList();

                                            var excludedIds = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                                                .Where(s => s.ValidFrom <= currentDate && s.ValidTo >= currentDate)
                                                .Select(s => s.UserId).ToList();

                                            // Exclude the IDs found in SpecalityFatDiscountUsers from the stateIds
                                            var bdoIds = stateIds.Where(id => !excludedIds.Contains(id)).ToList();

                                            if (bdoIds.Any())
                                            {
                                                dealerlist = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                                                   .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.u.Id, uc => uc.CustomerId, (x, uc) => new { DealerId = x.u.Id, x.ur, uc })
                                                   .Where(_ => bdoIds.Contains(_.uc.UserId) && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.DealerId).ToList();
                                            }
                                        }
                                    }

                                    if (dealerlist != null && dealerlist.Any() && currentDate >= reportingToMappingLimitContext.ValidFrom
                                        && currentDate <= reportingToMappingLimitContext.ValidTo)
                                    {
                                        saudaBidQuantity = _resultService.GetSaudaBookedQuantityForCurrentDateByDealersByDateRangeIsReportingtoAllocation(inputDto, (long)skuContext.OilTypeId, dealerlist, reportingToMappingLimitContext.ValidFrom, reportingToMappingLimitContext.ValidTo);
                                    }

                                    if (reportingToMappingLimitContext.RequestedDiscount > 0 && reportingToMappingLimitContext.RequestedDiscountDate.HasValue && reportingToMappingLimitContext.RequestedDiscountDate.Value.Date != DateHelper.UtcToIndia(DateTime.UtcNow).Date)
                                    {
                                        orderedQuantityBdo = saudaBidQuantity;
                                        actualDiscountQuantityBdo = reportingToMappingLimitContext.ActualDiscount - orderedQuantityBdo;
                                        var actualAvailableQuantityBdo = actualDiscountQuantityBdo - reportingToMappingLimitContext.RequestedDiscount;
                                        if (actualAvailableQuantityBdo < 0)
                                        {
                                            actualAvailableQuantityBdo = 0;
                                        }

                                        if (actualAvailableQuantityBdo <= requestedQuantityBdo)
                                        {
                                            totalQuantityBdo = requestedQuantityBdo;
                                            if (saudaBidQuantity != 0)
                                            {
                                                orderedQuantityBdo = saudaBidQuantity;
                                                totalQuantityBdo = orderedQuantityBdo + requestedQuantityBdo;
                                            }
                                        }
                                        else
                                        {
                                            oilTypeExpiredError = oilTypeExpiredError + Constants.QuantityLimitExpired.Replace(Constants.OiltypeName, oiltypecontext.Name) + Environment.NewLine;
                                        }
                                    }
                                    else
                                    {
                                        if (saudaBidQuantity != 0)
                                        {
                                            orderedQuantityBdo = saudaBidQuantity;
                                            totalQuantityBdo = orderedQuantityBdo + requestedQuantityBdo;
                                        }
                                        else if (saudaBidQuantity == 0)
                                        {
                                            totalQuantityBdo = requestedQuantityBdo;
                                        }
                                        if (totalQuantityBdo > reportingToMappingLimitContext.ActualDiscount || requestedQuantityBdo > reportingToMappingLimitContext.RemainingQuantity)// here Actual discount is limit
                                        {
                                            availableQuantityBdo = reportingToMappingLimitContext.ActualDiscount - orderedQuantityBdo;
                                            if (availableQuantityBdo < 0)
                                            {
                                                availableQuantityBdo = 0;
                                            }
                                            oilTypeBasedError = oilTypeBasedError + Constants.OilTypeLimitExceeds.Replace(Constants.OiltypeName, oiltypecontext.Name).Replace(Constants.Quantity, availableQuantityBdo.ToString()) + Environment.NewLine;
                                            //return _resultService.ErrorMessage(Constants.OilTypeBdoLimitExceeds.Replace(Constants.OiltypeName, oiltypecontext.Name).Replace(Constants.Quantity, Math.Round(availableQuantityBdo, 2).ToString()));
                                        }
                                    }
                                }
                                else if (userLimitContext == null && reportingToMappingLimitContext == null && SaudaLimit == 0)
                                {
                                    userLimitNotExistError = userLimitNotExistError + Constants.UserLimitNotExists.Replace(Constants.OiltypeName, oiltypecontext.Name) + Environment.NewLine;
                                }

                                // Checking Sauda booking restriction based on Oiltype 
                                if (checkedOiltypeIdsForRestrict.Count == 0 || !checkedOiltypeIdsForRestrict.Contains(item.OilTypeId))
                                {
                                    var restrictionInput = new UserInputDto()
                                    {
                                        LoginUserId = inputDto.LoginUserId,
                                        SkuId = item.SkuId,
                                        SalesOrganizationId = inputDto.SalesOrganizationId,
                                        DistributionChannelId = inputDto.DistributionChannelId,
                                        DivisionId = inputDto.DivisionId
                                    };
                                    checkedOiltypeIdsForRestrict.Add(item.OilTypeId);
                                    var saudaRestrictionResult = _lookupService.SaudaBookingConfigurationRolewise(restrictionInput);
                                    var restrictedOiltypeData = new SaudaBoookingConfig();
                                    if (saudaRestrictionResult.IsSuccess)
                                    {
                                        restrictedOiltypeData = (SaudaBoookingConfig)saudaRestrictionResult.SuccessDto.Response;
                                        if (restrictedOiltypeData.Message != null)
                                        {
                                            oilTypeBasedRestrictionError = oilTypeBasedRestrictionError + restrictedOiltypeData.Message + Environment.NewLine;
                                        }
                                    }
                                }
                            }
                        }

                        decimal calculatedDiscount = (decimal)0;

                        if (item.DiscountTypeId == (int)DTO.Enums.SaudaDiscountType.Discount)
                        {
                            var userdiscount = _emamiContext.DiscountUsers.AsNoTracking().OrderByDescending(s => s.Id).FirstOrDefault(a => a.SkuId == item.SkuId && a.ParentId != 0 && a.UserId == inputDto.LoginUserId && a.StateId == dealerContext.StateId &&
                            currentDate >= a.ValidFrom && currentDate <= a.ValidTo);
                            if (userdiscount != null)
                            {
                                if (item.DiscountAmountPerCase > userdiscount.ActualDiscount)
                                {
                                    //var geodiscount = _emamiContext.DiscountGeography.AsNoTracking().OrderByDescending(s => s.Id).FirstOrDefault(a => a.SkuId == item.SkuId &&
                                    // currentDate >= a.ValidFrom && currentDate <= a.ValidTo && a.CityId == dealerContext.CityId);

                                    //var geodiscount = _emamiContext.DiscountGeography.AsNoTracking().OrderByDescending(s => s.Id).FirstOrDefault(a => a.SkuId == item.SkuId &&
                                    // currentDate >= a.ValidFrom && currentDate <= a.ValidTo && a.CityId == dealerContext.CityId);

                                    //if (geodiscount == null)
                                    //{
                                    //    geodiscount = _emamiContext.DiscountGeography.AsNoTracking().OrderByDescending(s => s.Id).FirstOrDefault(a => a.OilTypeId == item.OilTypeId &&
                                    // currentDate >= a.ValidFrom && currentDate <= a.ValidTo && a.CityId == dealerContext.CityId);
                                    //}


                                    //if (geodiscount != null)
                                    //{
                                    //    if (skuContext.OilPackGroupTypeId != null)
                                    //    {
                                    //        if (skuContext.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                    //        {
                                    //            calculatedDiscount = geodiscount.ActualDiscount;
                                    //        }
                                    //        else if (skuContext.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                                    //        {
                                    //            calculatedDiscount = _resultService.CalculateAutomatedDiscount(geodiscount.ActualDiscount, geodiscount.SkuId, item.SkuId);
                                    //        }
                                    //    }

                                    //    if (Math.Round(item.DiscountAmountPerCase, 2) > Math.Round(calculatedDiscount, 2))
                                    //    {
                                    //        return _resultService.ErrorMessage(item.DiscountAmountPerCase + " " + Constants.DiscountNotExists);
                                    //    }
                                    //}
                                    //else if (item.DiscountAmountPerCase > 0)
                                    //{
                                    //    return _resultService.ErrorMessage(item.DiscountAmountPerCase + " " + Constants.DiscountNotExists);
                                    //}

                                    // Direct SkuId match (unchanged query — simple CityId filter only)
                                    var geodiscount = _emamiContext.DiscountGeography.AsNoTracking()
                                        .OrderByDescending(s => s.Id)
                                        .FirstOrDefault(a => a.SkuId == item.SkuId &&
                                            currentDate >= a.ValidFrom && currentDate <= a.ValidTo &&
                                            a.CityId == dealerContext.CityId);

                                    if (geodiscount != null)
                                    {
                                        // Direct sku match → no conversion
                                        calculatedDiscount = geodiscount.ActualDiscount;
                                    }
                                    else
                                    {
                                        // Fallback: same OilType AND same OilPackGroupType (join Skus)
                                        geodiscount = (from a in _emamiContext.DiscountGeography.AsNoTracking()
                                                       join s in _emamiContext.Skus.AsNoTracking() on a.SkuId equals s.Id
                                                       where a.OilTypeId == item.OilTypeId
                                                          && currentDate >= a.ValidFrom && currentDate <= a.ValidTo
                                                          && a.CityId == dealerContext.CityId
                                                          && s.OilPackGroupTypeId == skuContext.OilPackGroupTypeId
                                                       orderby a.Id descending
                                                       select a).FirstOrDefault();

                                        if (geodiscount != null && skuContext.OilPackGroupTypeId != null)
                                        {
                                            if (skuContext.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                            {
                                                calculatedDiscount = geodiscount.ActualDiscount;
                                            }
                                            else if (skuContext.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                                            {
                                                calculatedDiscount = _resultService.CalculateAutomatedDiscount(
                                                    geodiscount.ActualDiscount, geodiscount.SkuId, item.SkuId);
                                            }
                                        }
                                    }

                                    // Existing post-lookup validation stays as-is:
                                    if (geodiscount != null)
                                    {
                                        if (Math.Round(item.DiscountAmountPerCase, 2) > Math.Round(calculatedDiscount, 2))
                                        {
                                            return _resultService.ErrorMessage(item.DiscountAmountPerCase + " " + Constants.DiscountNotExists);
                                        }
                                    }
                                    else if (item.DiscountAmountPerCase > 0)
                                    {
                                        return _resultService.ErrorMessage(item.DiscountAmountPerCase + " " + Constants.DiscountNotExists);
                                    }
                                }
                            }
                            else
                            {
                                //var geodiscount = _emamiContext.DiscountGeography.AsNoTracking().OrderByDescending(s => s.Id).FirstOrDefault(a => a.SkuId == item.SkuId &&
                                //        currentDate >= a.ValidFrom && currentDate <= a.ValidTo && ((a.CityId == dealerContext.CityId || a.CityId == 0) && (a.DistrictId == dealerContext.DistrictId || a.DistrictId == 0) 
                                //        && (a.StateId == dealerContext.StateId || a.StateId == 0) && a.ZoneId == dealerContext.ZoneId));

                                //var geodiscount = _emamiContext.DiscountGeography.AsNoTracking().OrderByDescending(s => s.Id).FirstOrDefault(a => a.SkuId == item.SkuId &&
                                //        currentDate >= a.ValidFrom && currentDate <= a.ValidTo && ((a.CityId == dealerContext.CityId || a.CityId == 0) && (a.DistrictId == dealerContext.DistrictId || a.DistrictId == 0)
                                //        && (a.StateId == dealerContext.StateId || a.StateId == 0) && a.ZoneId == dealerContext.ZoneId));

                                //if (geodiscount == null)
                                //{
                                //    geodiscount = _emamiContext.DiscountGeography.AsNoTracking().OrderByDescending(s => s.Id).FirstOrDefault(a => a.OilTypeId == item.OilTypeId &&
                                //        currentDate >= a.ValidFrom && currentDate <= a.ValidTo && ((a.CityId == dealerContext.CityId || a.CityId == 0) && (a.DistrictId == dealerContext.DistrictId || a.DistrictId == 0)
                                //        && (a.StateId == dealerContext.StateId || a.StateId == 0) && a.ZoneId == dealerContext.ZoneId));
                                //}



                                //if (geodiscount != null)
                                //{
                                //    if (skuContext.OilPackGroupTypeId != null)
                                //    {
                                //        if (skuContext.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                //        {
                                //            calculatedDiscount = geodiscount.ActualDiscount;
                                //        }
                                //        else if (skuContext.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                                //        {
                                //            calculatedDiscount = _resultService.CalculateAutomatedDiscount(geodiscount.ActualDiscount, geodiscount.SkuId, item.SkuId);
                                //        }
                                //    }

                                //    if (Math.Round(item.DiscountAmountPerCase, 2) > Math.Round(calculatedDiscount, 2))
                                //    {
                                //        return _resultService.ErrorMessage(item.DiscountAmountPerCase + " " + Constants.DiscountNotExists);
                                //    }
                                //}
                                //else if (item.DiscountAmountPerCase > 0)
                                //{
                                //    return _resultService.ErrorMessage(item.DiscountAmountPerCase + " " + Constants.DiscountNotExists);
                                //}

                                // Direct SkuId match (unchanged query — full multi-tier geography filter)
                                var geodiscount = _emamiContext.DiscountGeography.AsNoTracking()
                                    .OrderByDescending(s => s.Id)
                                    .FirstOrDefault(a => a.SkuId == item.SkuId &&
                                        currentDate >= a.ValidFrom && currentDate <= a.ValidTo &&
                                        ((a.CityId == dealerContext.CityId || a.CityId == 0) &&
                                         (a.DistrictId == dealerContext.DistrictId || a.DistrictId == 0) &&
                                         (a.StateId == dealerContext.StateId || a.StateId == 0) &&
                                         a.ZoneId == dealerContext.ZoneId));

                                if (geodiscount != null)
                                {
                                    // Direct sku match → no conversion
                                    calculatedDiscount = geodiscount.ActualDiscount;
                                }
                                else
                                {
                                    // Fallback: same OilType AND same OilPackGroupType (join Skus)
                                    geodiscount = (from a in _emamiContext.DiscountGeography.AsNoTracking()
                                                   join s in _emamiContext.Skus.AsNoTracking() on a.SkuId equals s.Id
                                                   where a.OilTypeId == item.OilTypeId
                                                      && currentDate >= a.ValidFrom && currentDate <= a.ValidTo
                                                      && (a.CityId == dealerContext.CityId || a.CityId == 0)
                                                      && (a.DistrictId == dealerContext.DistrictId || a.DistrictId == 0)
                                                      && (a.StateId == dealerContext.StateId || a.StateId == 0)
                                                      && a.ZoneId == dealerContext.ZoneId
                                                      && s.OilPackGroupTypeId == skuContext.OilPackGroupTypeId
                                                   orderby a.Id descending
                                                   select a).FirstOrDefault();

                                    if (geodiscount != null && skuContext.OilPackGroupTypeId != null)
                                    {
                                        if (skuContext.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                        {
                                            calculatedDiscount = geodiscount.ActualDiscount;
                                        }
                                        else if (skuContext.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                                        {
                                            calculatedDiscount = _resultService.CalculateAutomatedDiscount(
                                                geodiscount.ActualDiscount, geodiscount.SkuId, item.SkuId);
                                        }
                                    }
                                }

                                // Existing post-lookup validation stays as-is:
                                if (geodiscount != null)
                                {
                                    if (Math.Round(item.DiscountAmountPerCase, 2) > Math.Round(calculatedDiscount, 2))
                                    {
                                        return _resultService.ErrorMessage(item.DiscountAmountPerCase + " " + Constants.DiscountNotExists);
                                    }
                                }
                                else if (item.DiscountAmountPerCase > 0)
                                {
                                    return _resultService.ErrorMessage(item.DiscountAmountPerCase + " " + Constants.DiscountNotExists);
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(oilTypeExpiredError))
                    {
                        return _resultService.ErrorMessage(oilTypeExpiredError);
                    }
                    if (!string.IsNullOrEmpty(userLimitNotExistError))
                    {
                        return _resultService.ErrorMessage(userLimitNotExistError);
                    }
                    if (!string.IsNullOrEmpty(oilTypeBasedError))
                    {
                        return _resultService.ErrorMessage(oilTypeBasedError);
                    }
                    if (!string.IsNullOrEmpty(oilTypeBasedRestrictionError))
                    {
                        return _resultService.ErrorMessage(oilTypeBasedRestrictionError);
                    }
                }

                var statusId = (int)DTO.Enums.Status.Pending;

                long DealerTypeId = 0;
                string IncotermsType = string.Empty;
                long BrokerId = 0;
                var dealerRole = _emamiContext.UserRoles.FirstOrDefault(_ => _.UserId == inputDto.DealerId);
                if (dealerRole != null)
                {
                    DealerTypeId = dealerRole.RoleId == (int)DTO.Enums.Role.Broker ? (int)DTO.Enums.DealerType.Broker : (int)DTO.Enums.DealerType.Direct;
                    if (dealerRole.RoleId == (int)DTO.Enums.Role.Broker)
                    {
                        BrokerId = inputDto.DealerId;
                    }
                    else
                    {
                        BrokerId = inputDto.BrokerId;
                    }
                }

                var divisionContext = _emamiContext.Divisions.FirstOrDefault(_ => _.Id == inputDto.DivisionId);

                var saudaContext = new Sauda
                {
                    BiddingDate = currentDate,
                    BdoId = inputDto.LoginUserId,
                    UserId = inputDto.DealerId,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = currentDate,
                    IsSAPDataSync = false,
                    SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                    IsSAPDataSyncApproval = false,
                    SalesOrganizationId = inputDto.SalesOrganizationId,
                    DistributionChannelId = inputDto.DistributionChannelId,
                    DivisionId = inputDto.DivisionId,
                    SalesDocumentType = divisionContext != null ? divisionContext.SalesDocumentType : string.Empty,
                    StatusId = (int)DTO.Enums.Status.Pending,
                    SaudaType = inputDto.SaudaType,
                    IsCrossAndUpsellContract = inputDto.IsCrossAndUpsellContract
                };
                _emamiContext.Sauda.Add(saudaContext);
                _emamiContext.SaveChanges();

                var requestedToUser = (from uc in _emamiContext.UserReportingToMappings.AsNoTracking()
                                       join udiv in _emamiContext.UserDivisionMappings.AsNoTracking() on uc.UserId equals udiv.UserId
                                       where
                                       udiv.SalesOrganizationId == inputDto.SalesOrganizationId
                                       && udiv.DistributionChannelId == inputDto.DistributionChannelId
                                       && udiv.DivisionId == inputDto.DivisionId
                                       && uc.UserId == inputDto.LoginUserId
                                       select uc.ReportingToUserId

                                     ).FirstOrDefault();


                //Sauda approval save
                var saudaapprovalContext = new SaudaApproval
                {
                    RequestedBy = inputDto.LoginUserId,
                    RequestedTo = requestedToUser > 0 ? requestedToUser : 0,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = currentDate,
                    StatusId = (int)DTO.Enums.Status.Pending,
                    SaudaId = saudaContext.Id
                };
                _emamiContext.SaudaApproval.Add(saudaapprovalContext);
                _emamiContext.SaveChanges();

                List<long> saudaOrderIds = new List<long>();
                List<SaudaCreateNotificationDto> saudaCreateEmailList = new List<SaudaCreateNotificationDto>();
                if (inputDto.SaudaOrders != null && inputDto.SaudaOrders.Any())
                {
                    int i = 0;
                    var skuIds = inputDto.SaudaOrders.Select(s => s.SkuId).ToList();
                    var skuUomMappingData = _emamiContext.SkuUomMapping.AsNoTracking()
                                         .Where(_ => skuIds.Contains(_.SkuId))
                                         .Select(s => new SkuUomMappingDto
                                         {
                                             Id = s.Id,
                                             ConversionFactor = s.ConversionFactor,
                                             ConversionFactor1 = s.ConversionFactor1,
                                             ConversionFactor2 = s.ConversionFactor2,
                                             SkuId = s.SkuId,
                                             UomId = s.UomId
                                         }).ToList();

                    var tpIds = inputDto.SaudaOrders.Select(s => s.PricingId).ToList();
                    var todayPricingContext = _emamiContext.TodayPricing.AsNoTracking().Where(tp => tpIds.Contains(tp.Id)).ToList();
                    #region QPS Discount Fetch
                    var skuQpsInputDto = new SkuQpsInputDto { DealerId = inputDto.DealerId, SkuDetails = new List<SkuQpsDiscountDto>() };
                    foreach (var order in inputDto.SaudaOrders)
                    {
                        var skuDetail = new SkuQpsDiscountDto
                        {
                            SkuId = order.SkuId,
                            Quantity = order.BidQuantity
                        };
                        skuQpsInputDto.SkuDetails.Add(skuDetail);
                    }
                    var qpsResult = _qpsService.GetQPSDiscountForQuantity(skuQpsInputDto);
                    var qpsDiscountResult = new List<MultipleSkuQpsDiscountResultDto>();
                    if (qpsResult.IsSuccess)
                    {
                        qpsDiscountResult = (List<MultipleSkuQpsDiscountResultDto>)qpsResult.SuccessDto.Response;
                    }

                    #endregion

                    foreach (var item in inputDto.SaudaOrders)
                    {

                        DateTime? saudaValidFromDate = currentDate;
                        // long? depotIdForRake = 0;
                        if (item.SaudaValidFromDate != null)
                            saudaValidFromDate = item.SaudaValidFromDate;
                        //if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ExRake || item.IncotermsId == (int)DTO.Enums.IncoTerms.ExRake)
                        //{
                        //    depotIdForRake = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == item.PlantId && !_.IsPlant)?.DepotId;
                        //}

                        var IncotermContext = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == item.IncotermsId).Name;
                        IncotermsType = IncotermContext.ToLower().Contains("for") ? "For" : "Ex";

                        //if (!isDiscountAppliedZero)
                        //{
                        //    item.DiscountAmount = 0;
                        //}
                        //else
                        //{
                        // item.DiscountAmount = item.BidQuantity * item.DiscountAmountPerCase;
                        //}

                        #region PricingLive to Pricing DataInsert and Rearrange the Pricing Data
                        ///Pricing Live is contain Current day Pricing
                        ///So, we insert the Pricing Live data into Pricing table for Sauda booked records only
                        /// Daily we cleanup and fresh data insert into the pricing live table
                        var pricingLiveContext = todayPricingContext.FirstOrDefault(_ => _.Id == item.PricingId);
                        //var pricingContext = default(Pricing);
                        long pricingId = 0;
                        decimal bidPrice = 0;
                        if (pricingLiveContext == null)
                        {
                            return _resultService.ErrorMessage(Constants.PricingIdisnotValid);
                        }
                        if (pricingLiveContext.PricingReferneceId == 0)
                        {
                            var pricing = new Pricing()
                            {
                                SkuId = pricingLiveContext.SkuId,
                                OilTypeId = pricingLiveContext.OilTypeId,
                                OilPackingTypeId = pricingLiveContext.OilPackingTypeId,
                                PlantId = pricingLiveContext.PlantId,
                                Price = pricingLiveContext.Price,
                                SalesOrganizationId = pricingLiveContext.SalesOrganizationId,
                                DistributionChannelId = pricingLiveContext.DistributionChannelId,
                                DivisionId = pricingLiveContext.DivisionId,
                                SAPPricingCode = pricingLiveContext.SAPPricingCode,
                                CreatedBy = pricingLiveContext.CreatedBy,
                                CreatedDate = pricingLiveContext.CreatedDate,
                                ModifiedBy = pricingLiveContext.ModifiedBy,
                                ModifiedDate = pricingLiveContext.ModifiedDate,
                                ValidFrom = (DateTime)pricingLiveContext.ValidFrom,
                                ValidTo = (DateTime)pricingLiveContext.ValidTo,
                            };
                            _emamiContext.Pricing.Add(pricing);
                            _emamiContext.SaveChanges();
                            pricingId = pricing.Id;
                            pricingLiveContext.PricingReferneceId = pricing.Id;
                            _emamiContext.SaveChanges();
                            bidPrice = pricing.Price;
                        }
                        else
                        {
                            pricingId = pricingLiveContext.PricingReferneceId;
                            bidPrice = pricingLiveContext.Price;
                        }

                        #endregion

                        #region Updated code
                        decimal itemquotedprice = item.BidQuantity * bidPrice; // Here QuotedPrice is with Discount or Premium applied for BasePrice so only below formulas for discount and premium
                        item.QuotedPrice = itemquotedprice;
                        item.BidPrice = itemquotedprice;

                        decimal qpsDiscount = 0;
                        string qpsId = string.Empty;
                        string individualQPSDiscount = string.Empty;
                        if (qpsDiscountResult != null && qpsDiscountResult.Any())
                        {
                            qpsDiscount = qpsDiscountResult.FirstOrDefault(q => q.SkuId == item.SkuId).Discount;
                            qpsDiscount = qpsDiscount * item.BidQuantity; // Calculating QPSDiscount based on quantity.
                            qpsId = qpsDiscountResult.FirstOrDefault(q => q.SkuId == item.SkuId).QpsId;
                            individualQPSDiscount = qpsDiscountResult.FirstOrDefault(q => q.SkuId == item.SkuId).IndividualQPSDiscount;
                        }

                        if (item.DiscountTypeId == (int)DTO.Enums.SaudaDiscountType.Discount)
                        {
                            item.BidPrice = item.QuotedPrice - item.DiscountAmount - qpsDiscount;  // Discount
                        }
                        else
                        {
                            item.BidPrice = (item.QuotedPrice + item.DiscountAmount) - qpsDiscount;  // Premium
                        }
                        #endregion

                        i = i + 10;
                        var saudaOrder = new SaudaOrder
                        {
                            SaudaId = saudaContext.Id,
                            SaudaNumber = i.ToString(),
                            SkuId = item.SkuId,
                            OilTypeId = item.OilTypeId,
                            BidPrice = item.BidPrice,
                            // BidPriceForDailyReport = item.BidPrice,
                            DiscountTypeId = item.DiscountTypeId,
                            //DiscountTypeIdForDailyReport = item.DiscountTypeId,
                            DiscountAmount = item.DiscountAmount,
                            QPSDiscount = qpsDiscount,
                            QpsId = qpsId,
                            IndividualQPSDiscount = individualQPSDiscount,
                            //DiscountAmountForDailyReport = item.DiscountAmount,
                            BidQuantity = _resultService.ConvertCasetoMetricTonWithoutDB(item.BidQuantity, item.SkuId, skuUomMappingData),
                            // BidQuantityForDailyReport = _resultService.ConvertCasetoMetricTon(item.BidQuantity, item.SkuId),
                            BidQuantityCase = item.BidQuantity,
                            // BidQuantityCaseForDailyReport = item.BidQuantity,
                            QuotedPrice = item.QuotedPrice,
                            // QuotedPriceForDailyReport = item.QuotedPrice,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = currentDate,
                            //BiddingwindowId = item.BiddingwindowId,
                            SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                            PricingId = pricingId, // pricingContext.Id,
                                                   // DealerTypeId = DealerTypeId,
                            Incoterms1 = IncotermsType,
                            PlantId = item.PlantId,
                            //DealerLocationId = Convert.ToInt64(dealerContext.FreightRouteId),
                            // CustomerPONumber = dealerContext.Code + currentDate.ToShortDateString(),
                            // CustomerPONumberForDailyReport = dealerContext.Code + currentDate.ToShortDateString(),
                            ValidFromDate = saudaValidFromDate.Value,
                            ValidToDate = item.SaudaValidToDate != null ? item.SaudaValidToDate.Value : saudaValidFromDate.Value.AddDays(Convert.ToDouble(dealerContext.SaudaValidityPeriod > 0 ? dealerContext.SaudaValidityPeriod : Config.DefaultSaudaValidity)),
                            StatusId = statusId,
                            //SaudaStatusId = (int)DTO.Enums.SaudaStatus.NotReleased,
                            //StatusIdForDailyReport = statusId,
                            //SaudaStatusIdForDailyReport = (int)DTO.Enums.SaudaStatus.NotReleased,
                            Incoterms2 = item.IncotermsId,
                            BrokerId = BrokerId,
                            //BrokerIdForDailyReport = BrokerId,
                            IsSAPDataSync = false,
                            IsSAPDataSyncApproval = false,
                            IsReportingtoAllocation = IsReportingtoAllocation,
                            // DepotIdForRake = depotIdForRake.Value,
                            IsQuantityLimitForBookingSauda = IsQuantityLimitForBookingSauda,
                            SalesOrganizationId = inputDto.SalesOrganizationId,
                            DistributionChannelId = inputDto.DistributionChannelId,
                            DivisionId = inputDto.DivisionId,
                            IsMandatorySku = item.IsMandatorySku,
                            EmployeeSkuDiscountId = item.DiscountId == 0 ? _resultService.GetDiscountId(inputDto, item) : item.DiscountId,
                            QuotedPriceBeforeSAPDiscount = item.BidQuantity == 0 ? 0m : item.BidPrice / item.BidQuantity
                        }
                    ;
                        var overallSaudaStatuses = Constants.OverallSaudaStatus;
                        var LimitIdsList = new List<long>();
                        var dealerlist = new List<long>();
                        if (userrolecontext.RoleId == (int)DTO.Enums.Role.ZonalTrader)
                        {
                            var bdoIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(s => s.UserId).ToList();

                            if (bdoIds.Any())
                            {
                                dealerlist = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                            .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.u.Id, uc => uc.CustomerId, (x, uc) => new { DealerId = x.u.Id, x.ur, uc })
                            .Where(_ => bdoIds.Contains(_.uc.UserId) && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.DealerId).ToList();
                            }
                        }
                        else
                        {
                            var zoneIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                                            .Join(_emamiContext.UserDivisionMappings.AsNoTracking(), urm => urm.ReportingToUserId, ud => ud.UserId, (urm, ud) => new { urm, ud }).
                                            Where(_ => _.ud.SalesOrganizationId == inputDto.SalesOrganizationId && _.ud.DistributionChannelId == inputDto.DistributionChannelId && _.ud.DivisionId == inputDto.DivisionId)
                                            .Select(_ => _.urm.ReportingToUserId).Distinct().ToList();

                            if (zoneIds.Any())
                            {
                                var stateIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => zoneIds.Contains(_.ReportingToUserId)).Select(s => s.UserId).Distinct().ToList();
                                var excludedIds = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                                    .Where(s => s.ValidFrom <= currentDate && s.ValidTo >= currentDate)
                                    .Select(s => s.UserId).ToList();

                                var bdoIds = stateIds.Where(id => !excludedIds.Contains(id)).ToList();
                                if (bdoIds.Any())
                                {
                                    dealerlist = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                                .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.u.Id, uc => uc.CustomerId, (x, uc) => new { DealerId = x.u.Id, x.ur, uc })
                                .Where(_ => bdoIds.Contains(_.uc.UserId) && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.DealerId).ToList();
                                }
                            }
                            //dealerlist = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                            //.Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.u.Id, uc => uc.CustomerId, (x, uc) => new { DealerId = x.u.Id, x.ur, uc })
                            //.Where(_ => _.uc.UserId == inputDto.LoginUserId && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.DealerId).ToList();
                        }
                        requestedQuantityBdo = _resultService.ConvertCasetoMetricTonWithoutDB(item.BidQuantity, item.SkuId, skuUomMappingData);

                        if (stateIsauantity)
                        {
                            var userStateLimitContext = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId
                                        && _.OilTypeId == item.OilTypeId
                                        && _.ParentId == 0
                                        && _.ValidFrom <= currentDate
                                        && _.ValidTo >= currentDate && _.SalesOrganizationId == inputDto.SalesOrganizationId
                                && _.DistributionChannelId == inputDto.DistributionChannelId && _.DivisionId == inputDto.DivisionId);
                            decimal saudaBidQtySt = 0;
                            if (userStateLimitContext != null)
                            {
                                LimitIdsList.Add(userStateLimitContext.Id);
                                if (dealerlist != null && dealerlist.Any())
                                {
                                    saudaBidQtySt = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.OilTypeId == item.OilTypeId && dealerlist.Contains(_.Sauda.UserId)
                                          && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(userStateLimitContext.ValidFrom) /*&& _.CreatedBy == inputDto.LoginUserId*/ && !_.IsReportingtoAllocation
                                          && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(userStateLimitContext.ValidTo) && overallSaudaStatuses.Contains(_.StatusId) && _.IsQuantityLimitForBookingSauda
                                          && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId
                                          && _.DivisionId == inputDto.DivisionId)
                                          .Select(s => s.BidQuantity).DefaultIfEmpty(0).Sum();
                                }
                            }

                            var totalQty = saudaBidQtySt + requestedQuantityBdo;
                            if (userStateLimitContext != null && userStateLimitContext.ActualDiscount >= totalQty)
                            {
                                decimal availableUpdateQuantityBdo = 0;
                                if (saudaBidQtySt == 0)
                                {
                                    availableUpdateQuantityBdo = userStateLimitContext.RemainingQuantity - requestedQuantityBdo;
                                }
                                else
                                {
                                    availableUpdateQuantityBdo = userStateLimitContext.ActualDiscount - totalQty;
                                }

                                foreach (var id in LimitIdsList)
                                {
                                    var remainingQuantityUpdate = "UPDATE SpecalityFatDiscountUsers SET RemainingQuantity = @availableUpdateQuantityBdo, CreatedDate = @currentDate WHERE Id = @Id OR ParentId = @Id";

                                    var remainingQuantityParameters = new[]
                                    {
                                                    new SqlParameter("@availableUpdateQuantityBdo", availableUpdateQuantityBdo),
                                                    new SqlParameter("@currentDate", DateTime.Now),
                                                    new SqlParameter("@Id", id)
                                                };

                                    _emamiContext.BulkUpdateProxy(remainingQuantityUpdate, remainingQuantityParameters);
                                }
                            }
                        }
                        else if (stateNotQuantity)
                        {
                            var nonStateLimitContext = (from s in _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                                                        join u in _emamiContext.UserReportingToMappings on s.UserId equals u.ReportingToUserId
                                                        where u.UserId == inputDto.LoginUserId
                                                        && s.OilTypeId == item.OilTypeId
                                                        //&& s.ParentId == 0
                                                        && s.ValidFrom <= currentDate
                                                        && s.ValidTo >= currentDate
                                                        && s.SalesOrganizationId == inputDto.SalesOrganizationId
                                                        && s.DistributionChannelId == inputDto.DistributionChannelId
                                                        && s.DivisionId == inputDto.DivisionId
                                                        select s).FirstOrDefault();
                            decimal saudaBidQtyNonSt = 0;
                            if (nonStateLimitContext != null)
                            {
                                LimitIdsList.Add(nonStateLimitContext.Id);
                                if (dealerlist != null && dealerlist.Any())
                                {
                                    saudaBidQtyNonSt = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.OilTypeId == item.OilTypeId && dealerlist.Contains(_.Sauda.UserId)
                                          && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(nonStateLimitContext.ValidFrom) /*&& _.CreatedBy == inputDto.LoginUserId*/ && _.IsReportingtoAllocation
                                          && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(nonStateLimitContext.ValidTo) && overallSaudaStatuses.Contains(_.StatusId) && _.IsQuantityLimitForBookingSauda
                                          && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId
                                          && _.DivisionId == inputDto.DivisionId)
                                          .Select(s => s.BidQuantity).DefaultIfEmpty(0).Sum();
                                }
                            }

                            var totalQty = saudaBidQtyNonSt + requestedQuantityBdo;
                            if (nonStateLimitContext != null && nonStateLimitContext.ActualDiscount >= totalQty)
                            {
                                decimal availableUpdateQuantityBdo = 0;
                                if (saudaBidQuantity == 0)
                                {
                                    availableUpdateQuantityBdo = nonStateLimitContext.RemainingQuantity - requestedQuantityBdo;
                                }
                                else
                                {
                                    availableUpdateQuantityBdo = nonStateLimitContext.ActualDiscount - totalQty;
                                }

                                foreach (var id in LimitIdsList)
                                {
                                    var remainingQuantityUpdate = "UPDATE SpecalityFatDiscountUsers SET RemainingQuantity = @availableUpdateQuantityBdo, CreatedDate = @currentDate WHERE Id = @Id OR ParentId = @Id";

                                    var remainingQuantityParameters = new[]
                                    {
                                                    new SqlParameter("@availableUpdateQuantityBdo", availableUpdateQuantityBdo),
                                                    new SqlParameter("@currentDate", DateTime.Now),
                                                    new SqlParameter("@Id", id)
                                                };

                                    _emamiContext.BulkUpdateProxy(remainingQuantityUpdate, remainingQuantityParameters);
                                }
                            }
                        }
                        _emamiContext.SaudaOrders.Add(saudaOrder);
                        _emamiContext.SaveChanges();

                        //if (dealerContext.DivisionId == (int)DTO.Enums.LooseVertical.Loose)
                        //{
                        //    saudaOrderIds.Add(saudaOrder.Id);
                        //}

                        saudaCreateEmailList.Add(new SaudaCreateNotificationDto()
                        {
                            StatusId = item.StatusId,
                            SaudaOrderId = saudaOrder.Id,
                            SaudaBookingTypeId = saudaOrder.SaudaBookingTypeId,
                            SaudaOrderStatusId = saudaOrder.StatusId,
                            LoginUserId = inputDto.LoginUserId,
                            DealerId = inputDto.DealerId
                        });
                    }

                    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => SaudaCreateNotificationAsync(saudaCreateEmailList, cancellationToken));

                    //if (dealerContext.VerticalId == (int)DTO.Enums.LooseVertical.Loose)
                    //{
                    //    //method to sync Loose sauda from APP to SAP 
                    //    _sapIntegrationService.GetSaudaDetailsForLooseVertical(saudaOrderIds);
                    //}

                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = saudaContext.Id;
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


        public void SaudaCreateNotificationAsync(List<SaudaCreateNotificationDto> inputDto, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            try
            {
                using (AdaniContext _context = new AdaniContext())
                {
                    if (inputDto != null && inputDto.Any())
                    {
                        foreach (var saudaData in inputDto)
                        {
                            bool isEmail = false;

                            var DealerNotificationContext = _context.TPNotification.AsNoTracking().
                                                            Join(_context.TPNotificationDetails.AsNoTracking(), TPN => TPN.Id, TPND => TPND.TPNotificationId, (TPN, TPND) => new { TPN, TPND })
                                                            .Where(_ => _.TPND.DealerId == saudaData.DealerId && _.TPND.NotificationActionId == (int)DTO.Enums.NotificationActionTP.SaudaCreation && _.TPND.IsActive).ToList();

                            var DealerEmailNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.Email);
                            if (DealerEmailNotificationEnabled != null && DealerEmailNotificationEnabled.Any())
                                isEmail = true;
                            else
                                isEmail = false;

                            var usersContext = _context.Users.AsNoTracking().Where(_ => _.Id == saudaData.LoginUserId || _.Id == saudaData.DealerId);
                            var saudaOrderContext = _context.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == saudaData.SaudaOrderId);
                            if (usersContext != null && usersContext.Any() && saudaOrderContext != null)
                            {
                                List<string> toUsers = new List<string>();
                                var createdBy = usersContext.FirstOrDefault(_ => _.Id == saudaData.LoginUserId);
                                var dealer = _context.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaData.DealerId);
                                var ReportingId = _context.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaData.LoginUserId).ReportingToId;
                                var reportingTo = _context.Users.AsNoTracking().FirstOrDefault(_ => _.Id == ReportingId);
                                string dealerName = string.Empty;
                                if (createdBy != null && !string.IsNullOrEmpty(createdBy.Email))
                                {
                                    toUsers.Add(createdBy.Email);
                                }
                                if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                                {
                                    dealerName = dealer.Name;
                                    toUsers.Add(dealer.Email);
                                }
                                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                string emailSubject = string.Empty;

                                if (isEmail && toUsers != null && toUsers.Any())
                                {
                                    var fromEmail = Constants.FromEmail;
                                    var plainText = string.Empty;
                                    EmailTemplate emailTemplate = new EmailTemplate();
                                    if (saudaData.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess)
                                    {
                                        emailSubject = Constants.SaudaBookedSubject;
                                        emailTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationEmail);
                                    }
                                    else
                                    {
                                        if (saudaData.StatusId == (int)DTO.Enums.Status.Pending)
                                        {
                                            emailSubject = Constants.SaudaCreationRAFlowSubject;
                                            emailTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationRAFlowEmail);
                                        }
                                        else if (saudaData.StatusId == (int)DTO.Enums.Status.Hold)
                                        {
                                            emailSubject = Constants.SaudaOnHoldSubject;
                                            emailTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderHoldNotificationEmail);
                                        }
                                        else if (saudaData.StatusId == (int)DTO.Enums.Status.Rejected)
                                        {
                                            emailSubject = Constants.SaudaRejectedSubject;
                                            emailTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderRejectNotificationEmail);
                                        }
                                    }
                                    if (emailTemplate != null)
                                    {
                                        var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
                                            .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round((saudaOrderContext.BidPrice / saudaOrderContext.BidQuantityCase), 2)).ToString())
                                            .Replace(Constants.BY_FOR, Constants.FOR).Replace(Constants.UserName, dealerName);
                                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                        amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                    }
                                }
                                var smsPlainTemplate = string.Empty;

                                bool isSms = false;
                                //var IsSMS = _context.Configurations.AsNoTracking().Where(_ => _.Id == (int)DTO.Enums.Configuration.IsSMS).Select(_ => _.Value).Single();
                                var DealerSMSNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.SMS);
                                if (DealerSMSNotificationEnabled != null && DealerSMSNotificationEnabled.Any())
                                    isSms = true;
                                else
                                    isSms = false;

                                if (isSms)
                                {
                                    var smsMessage = string.Empty;
                                    EmailTemplate smsTemplate = new EmailTemplate();
                                    if (saudaData.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess)
                                    {
                                        smsTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationSMS);
                                    }
                                    else
                                    {
                                        if (saudaData.SaudaOrderStatusId == (int)DTO.Enums.Status.Pending)
                                        {
                                            smsTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationRAFlowSMS);


                                        }
                                        else if (saudaData.SaudaOrderStatusId == (int)DTO.Enums.Status.Hold)
                                        {
                                            smsTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderHoldNotificationSMS);
                                        }
                                        else if (saudaData.SaudaOrderStatusId == (int)DTO.Enums.Status.Rejected)
                                        {
                                            smsTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderRejectNotificationSMS);
                                        }

                                        var statusContext = _context.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == saudaData.SaudaOrderStatusId);
                                        var notificationContext = new Notifications
                                        {
                                            Request = DTO.Enums.NotificationRequest.Sauda.ToString(),
                                            RequestId = (int)DTO.Enums.NotificationRequest.Sauda,
                                            ReferenceId = saudaData.SaudaOrderId,
                                            Notification = statusContext != null ? statusContext.Name : string.Empty,
                                            StatusId = saudaData.SaudaOrderStatusId,
                                            CreatedBy = saudaData.SaudaOrderCreatedBy,
                                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                        };
                                        _context.Notifications.Add(notificationContext);
                                        _context.SaveChanges();
                                    }
                                    if (smsTemplate != null)
                                    {
                                        smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
                                            .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round((saudaOrderContext.BidPrice / saudaOrderContext.BidQuantityCase), 2)).ToString())
                                            .Replace(Constants.BY_FOR, Constants.FOR).Replace(Constants.UserName, dealerName);
                                        smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                        try
                                        {
                                            if (createdBy != null && !string.IsNullOrEmpty(createdBy.MobileNumber))
                                            {
                                                amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber, smsTemplate.SMSTemplateID);
                                            }
                                            if (dealer != null && !string.IsNullOrEmpty(dealer.MobileNumber))
                                            {
                                                amazonNotificationService.SendMessage(smsMessage, dealer.MobileNumber, smsTemplate.SMSTemplateID);
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }

                                bool isPushNotification = false;
                                //var IsPushNotification = _context.Configurations.AsNoTracking().Where(_ => _.Id == (int)DTO.Enums.Configuration.IsPushNotification).Select(_ => _.Value).Single();
                                var DealerPushNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.InAppNotification);
                                if (DealerPushNotificationEnabled != null && DealerPushNotificationEnabled.Any())
                                    isPushNotification = true;
                                else
                                    isPushNotification = false;


                                //if (isPushNotification && saudaData.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                //{
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
                                        //Id = saudaOrderContext.Id,
                                    };
                                    //_notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                }
                                if (reportingTo != null && reportingTo.RegistrationTypeId != null && reportingTo.RegistrationTypeId > 0 && !string.IsNullOrEmpty(reportingTo.PushTokenKey))
                                {
                                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                    {
                                        PushTokenKey = reportingTo.PushTokenKey,
                                        RegistrationTypeId = reportingTo.RegistrationTypeId != null ? (int)reportingTo.RegistrationTypeId : 0,
                                        Title = Constants.ApprovalRequest,
                                        Message = Constants.ApprovalRequestMessage,
                                        //Id = saudaOrderContext.Id,
                                    };
                                    //_notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                }
                                //}
                            }
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
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Push Notification Testing

        public ResultDto PushNotificationTesting(LoginUserIdDto inputDto)
        {
            try
            {
                if (true)
                {
                    SaudaDetailInputDto saudaDetailInputDto = new SaudaDetailInputDto() { SaudaOrderId = 295 };
                    var dealer = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                    if (dealer != null)
                    {
                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                        {
                            PushTokenKey = dealer.PushTokenKey,
                            RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
                            Title = "Test CounterBid Notification",
                            Message = "CounterBid Notification",
                            Id = "230"
                        };
                        _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                        return _resultService.SuccessMessageWitObject(saudaDetailInputDto, Constants.SuccessMessage);
                    }
                }
                return _resultService.SuccessMessage(Constants.EmployeeIsEmpty);
            }
            catch (Exception ex)
            {
                return _resultService.ErrorMessage(ex.Message);
            }
        }

        //public ResultDto GetCounterBidDetails(SaudaDetailInputDto inputDto)
        //{
        //    var saudaOrderDetails = new SaudaOrderDetails();
        //    try
        //    {

        //        SaudaOrder saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SaudaOrderId && _.Sauda != null);
        //        saudaOrderDetails.DealerId = saudaOrderContext.Sauda.UserId;
        //        saudaOrderDetails.DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == saudaOrderContext.Sauda.UserId)?.Name;
        //        saudaOrderDetails.BookedDate = saudaOrderContext.Sauda.BiddingDate;
        //        saudaOrderDetails.SaudaId = saudaOrderContext.Id;
        //        saudaOrderDetails.SaudaOrderId = saudaOrderContext.Id;
        //        saudaOrderDetails.SaudaNumber = saudaOrderContext.SaudaNumber;
        //        saudaOrderDetails.ValidToDate = saudaOrderContext.ValidToDate;
        //        saudaOrderDetails.OilTypeId = saudaOrderContext.OilTypeId;
        //        saudaOrderDetails.OilTypeName = saudaOrderContext.OilType != null ? saudaOrderContext.OilType.Name : string.Empty;
        //        saudaOrderDetails.SkuId = saudaOrderContext.SkuId;
        //        saudaOrderDetails.SkuName = saudaOrderContext.Sku != null ? saudaOrderContext.Sku.SkuName : string.Empty;
        //        saudaOrderDetails.StatusId = saudaOrderContext.StatusId;
        //        var statusContext = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.StatusId);
        //        if (statusContext != null)
        //        {
        //            saudaOrderDetails.Status = statusContext.Name;
        //        }
        //        IQueryable<SaudaOrderLiftingRequestMapping> liftingReqOrderContextList = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => _.SaudaOrderId == saudaOrderContext.Id
        //            && _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected);
        //        if (liftingReqOrderContextList != null && liftingReqOrderContextList.Any())
        //        {
        //            saudaOrderDetails.BidQuantity = saudaOrderContext.BidQuantity - liftingReqOrderContextList.Sum(_ => _.LiftingQuantity);
        //            saudaOrderDetails.BidQuantityCases = saudaOrderContext.BidQuantityCase - liftingReqOrderContextList.Sum(_ => _.LiftingQuantityCase);
        //        }
        //        else
        //        {
        //            saudaOrderDetails.BidQuantity = saudaOrderContext.BidQuantity;
        //            saudaOrderDetails.BidQuantityCases = saudaOrderContext.BidQuantityCase;
        //        }
        //        saudaOrderDetails.BidPrice = saudaOrderContext.BidPrice;
        //        saudaOrderDetails.BidPricePerCase = Math.Round((saudaOrderContext.BidPrice != 0 && saudaOrderContext.BidQuantityCase != 0 ? (saudaOrderContext.BidPrice / saudaOrderContext.BidQuantityCase) : 0), 2);
        //        saudaOrderDetails.IncoTerms = saudaOrderContext.Incoterms1;
        //        var plantContext = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.PlantId);
        //        if (plantContext != null)
        //        {
        //            saudaOrderDetails.PlantDepot = plantContext.Name;
        //        }
        //        //var freightRouteContext = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.DealerLocationId);
        //        //if (freightRouteContext != null)
        //        //{
        //        //    saudaOrderDetails.FrieghtRoute = freightRouteContext.Name;
        //        //}
        //        saudaOrderDetails.CounterBidOffer = saudaOrderContext.CounterBidOffer;
        //        saudaOrderDetails.CounterBidOfferDate = saudaOrderContext.CounterBidOfferDate != null ? saudaOrderContext.CounterBidOfferDate.Value : DateTime.MinValue;

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return _resultService.SuccessMessageWitObject(saudaOrderDetails, "");
        //}

        #endregion

        //public ResultDto SaudaCounterBidOfferDetails(SaudaCounterBidOfferDetailsInputDto inputDto)
        //{
        //    _methodName = "GetDealerSaudaLists";
        //    var resultDto = new ResultDto();
        //    var outputDto = new SaudaCounterBidOfferDetailsDto();
        //    var currentdate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        if (inputDto.Id == 0)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.SaudaBiddingCartIdMissing;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        var SaudaBiddingCartContext = _emamiContext.SaudaBiddingCart.AsNoTracking().FirstOrDefault(f => f.Id == inputDto.Id);
        //        if (SaudaBiddingCartContext == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.SaudaBiddingCartId;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            return resultDto;
        //        }

        //        var biddingWindow = _emamiContext.BiddingWindow.AsNoTracking().FirstOrDefault(f => f.Id == SaudaBiddingCartContext.BiddingWindowId);

        //        outputDto.Id = SaudaBiddingCartContext.Id;
        //        outputDto.SkuName = SaudaBiddingCartContext.Sku.SkuName ?? string.Empty;
        //        outputDto.DealerName = SaudaBiddingCartContext.Dealer.Name;
        //        outputDto.OilTypeName = SaudaBiddingCartContext.OilType.Name ?? string.Empty;
        //        outputDto.BidQuantity = SaudaBiddingCartContext.BidQuantityInMT;
        //        outputDto.BidQuantityCases = SaudaBiddingCartContext.BidQuantityInCase;
        //        //outputDto.FrieghtRoute = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(f => f.Id == SaudaBiddingCartContext.Dealer.FreightRouteId)?.Name;
        //        outputDto.PlantDepot = _emamiContext.Depots.AsNoTracking().FirstOrDefault(f => f.Id == SaudaBiddingCartContext.PlantId).Name;
        //        outputDto.IncoTerms = SaudaBiddingCartContext.Incoterm.Name ?? string.Empty;
        //        outputDto.CounterBidOffer = SaudaBiddingCartContext.CounterBidOffer;
        //        outputDto.BidPricePerCase = SaudaBiddingCartContext.CounterBidPrice;
        //        outputDto.SaudaId = SaudaBiddingCartContext.SaudaBiddingCartHeaderId;
        //        outputDto.SaudaOrderId = SaudaBiddingCartContext.Id;
        //        outputDto.BiddingWindowId = SaudaBiddingCartContext.BiddingWindowId;
        //        outputDto.StatusId = SaudaBiddingCartContext.StatusId;
        //        outputDto.BiddingWindowStatusId = biddingWindow != null ? biddingWindow.StatusId : 0;
        //        outputDto.BiddingWindowStatus = biddingWindow != null ? Utility.GetEnumFromString<DTO.Enums.BiddWindowStatus>(biddingWindow.StatusId) : string.Empty;

        //        //var saudaAllocationTime = $"{string.Format("{0:HH:mm tt}", biddingWindow.SaudaAllocationStartTime)}  -  {string.Format("{0:HH:mm tt}", biddingWindow.SaudaAllocationEndTime)}";
        //        var saudaAllocationTime = _emamiContext.RaSaudaConfiguration.AsNoTracking().FirstOrDefault(f => f.IsActive).SaudaAllocationTime;
        //        var saudaAllocation = new DateTime(currentdate.Year, currentdate.Month, currentdate.Day, saudaAllocationTime.Hours, saudaAllocationTime.Minutes, saudaAllocationTime.Seconds, saudaAllocationTime.Milliseconds);
        //        var saudaAllocationTimeFormat = string.Format("{0:HH:mm tt}", saudaAllocation);
        //        outputDto.SaudaAllocationTime = saudaAllocationTimeFormat;
        //        outputDto.DealerId = SaudaBiddingCartContext.Dealer.Id;

        //        resultDto.IsSuccess = true;
        //        resultDto.SuccessDto.Response = outputDto;
        //        return resultDto;
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        resultDto.IsSuccess = false;
        //        resultDto.ErrorDto.ErrorCode = Constants.Exception;
        //        resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
        //        _logger.Error(message);
        //        return resultDto;
        //    }
        //}

        public ResultDto GetPendingContractChartMobile(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetPendingContractChartMobile";
            var resultDto = new ResultDto();
            var saudaListDto = new List<SaudaListDto>();
            try
            {
                if (loginUserIdDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (loginUserIdDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == loginUserIdDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var dealerlist = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                  join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                                  where ucm.UserId == loginUserIdDto.LoginUserId
                                  select ucm.CustomerId).ToList();

                var saudaStatus = Constants.OutstandingSaudaStatus;
                //var saudaOrdersContext = _emamiContext.SaudaOrders.AsNoTracking().Join(_emamiContext.Sauda.AsNoTracking(), so => so.SaudaId, s => s.Id, (so, s) => new { so, s })
                //    .Join(_emamiContext.Users.AsNoTracking(), x => x.s.UserId, u => u.Id, (x, u) => new { x.so, x.s, u })
                //    .Join(_emamiContext.PendingContracts.AsNoTracking(), x => x.so.Id, pc => pc.SaudaOrderId, (x, pc) => new { x.so, x.s, x.u, pc })
                //    .Join(_emamiContext.City.AsNoTracking(), x => x.u.CityId, c => c.Id, (x, c) => new { x.so, x.s, x.pc, DealerName = x.u.Name, CityName = c.CityName })
                //    .Where(_ => dealerlist.Contains(_.s.UserId) && saudaStatus.Contains(_.so.StatusId) && _.s != null && _.so != null && _.so.OilType != null).ToList();

                var saudaOrdersContext = _emamiContext.PendingContracts.AsNoTracking()
                          .Join(_emamiContext.Users.AsNoTracking(), x => x.CustomerCode, u => u.Code, (x, u) => new { x, u })
                          .Join(_emamiContext.City.AsNoTracking(), x => x.u.CityId, c => c.Id, (x, c) => new { x.x, x.u, DealerName = x.u.Name, CityName = c.CityName })
                          .Where(_ => dealerlist.Contains(_.u.Id)
                          //&& _.u.DivisionId == userContext.DivisionId
                          ).ToList();

                if (saudaOrdersContext != null && saudaOrdersContext.Any())
                {
                    foreach (var item in saudaOrdersContext)
                    {
                        var outputdto = new SaudaListDto()
                        {
                            Id = _emamiContext.SaudaOrders.Where(s => s.SaudaNumber == item.x.SaudaNumber).FirstOrDefault() != null
                                    && _emamiContext.SaudaOrders.Where(s => s.SaudaNumber == item.x.SaudaNumber).FirstOrDefault().SaudaNumber != null ? _emamiContext.SaudaOrders.Where(s => s.SaudaNumber == item.x.SaudaNumber).FirstOrDefault().Id : 0,
                            //SaudaOrderId = _emamiContext.SaudaOrders.FirstOrDefault(s => s.SaudaNumber == _.x.SaudaNumber).SaudaNumber != null ? _emamiContext.SaudaOrders.FirstOrDefault(s => s.SaudaNumber == _.x.SaudaNumber).Id : 0,
                            SaudaOrderId = _emamiContext.SaudaOrders.Where(s => s.SaudaNumber == item.x.SaudaNumber).FirstOrDefault() != null
                                    && _emamiContext.SaudaOrders.Where(s => s.SaudaNumber == item.x.SaudaNumber).FirstOrDefault().SaudaNumber != null ? _emamiContext.SaudaOrders.Where(s => s.SaudaNumber == item.x.SaudaNumber).FirstOrDefault().Id : 0,
                            UserId = item.u.Id,
                            User = item.DealerName != null ? item.DealerName : string.Empty,
                            City = item.CityName != null ? item.CityName : string.Empty,
                            // BiddingDate = item.x.SaudaDate ?? DateTime.Now,
                            TotalBidPrice = item.x.BasicRate,
                            TotalBidQuantity = item.x.SaudaQuantity,
                            // OiltypeName = item.x.MaterialGroup4 != null ? item.x.MaterialGroup4 : string.Empty
                        };
                        saudaListDto.Add(outputdto);
                    }
                }

                if (saudaListDto != null && saudaListDto.Any())
                {
                    return _resultService.SuccessObject(saudaListDto);
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }
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
        /// Method to Get Sauda order details
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetExpiredAndNearExpiredSaudaDetails(SaudaDetailInputDto inputDto)
        {
            _methodName = "GetExpiredAndNearExpiredSaudaDetails";
            var resultDto = new ResultDto();
            var saudaDetails = new SaudaDetailOutputDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var saudaContext = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SaudaOrderId);

                if (saudaContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id).ToList();  // inputDto.SaudaOrderId is Sauda table Id
                if (!saudaOrderContext.IsAny())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaContext.UserId);
                //if (PendingContractContext == null)
                //{
                saudaDetails.SaudaNumber = saudaContext.SaudaNumber;
                saudaDetails.SaudaDate = saudaContext.BiddingDate;
                saudaDetails.DealerName = userContext.Name;
                saudaDetails.TotalAmount = saudaOrderContext.Sum(s => s.BidPrice);
                saudaDetails.TotalQuantity = saudaOrderContext.Sum(s => s.BidQuantityCase);
                saudaDetails.TotalQuantityInMT = saudaOrderContext.Sum(s => s.BidQuantity);
                saudaDetails.SaudaValidityDays = Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity;
                saudaDetails.BrokerId = saudaOrderContext.Where(_ => _.SaudaId == saudaContext.Id).FirstOrDefault() != null
                    ? saudaOrderContext.Where(_ => _.SaudaId == saudaContext.Id).FirstOrDefault().BrokerId : 0;
                if (saudaDetails.BrokerId > 0)
                {
                    saudaDetails.BrokerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaDetails.BrokerId).Name;

                }
                saudaDetails.IsFromSAPData = saudaContext.IsSAPDataSync;
                saudaDetails.SaudaExpireDays = Math.Abs((int)(DateTime.Now - saudaContext.BiddingDate).TotalDays);
                saudaDetails.ExpiryDate = saudaOrderContext.FirstOrDefault().ValidToDate;

                var saudaOrders = new List<SaudaOrderDetails>();
                foreach (var data in saudaOrderContext)
                {
                    var saudaOrderItem = new SaudaOrderDetails
                    {
                        SkuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == data.SkuId).SkuName,
                        BidPrice = data.BidPrice,
                        BidQuantity = data.BidQuantity,
                        BidQuantityCases = data.BidQuantityCase,
                        PlantName = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == data.PlantId)?.Name,
                        BidPricePerCase = data.BidPrice / data.BidQuantityCase,
                        BookedDate = data.CreatedDate
                    };
                    saudaOrders.Add(saudaOrderItem);
                }

                saudaDetails.SaudaOrders = saudaOrders;

                if (saudaContext != null)
                {
                    //Dispatch status

                    var saudaorderDetails = saudaOrderContext.Where(_ => _.SaudaId == saudaContext.Id).ToList();
                    var saudaIDs = saudaorderDetails.Select(_ => _.Id).ToList();
                    var salesOrderDerails = _emamiContext.LiftingRequest.AsNoTracking()
                        .Join(_emamiContext.LiftingRequestDetails.AsNoTracking(), l => l.Id, ld => ld.LiftingRequestId, (l, ld) => new { l, ld }).Where(_ => saudaIDs.Contains(_.ld.SaudaOrderId));

                    var salesorderId = salesOrderDerails.Select(s => s.l.Id).Distinct().ToList();
                    var salesorderQty = salesOrderDerails.Select(s => s.ld.LiftingQuantity).DefaultIfEmpty(0).Sum();
                    var salesorderQtyCase = salesOrderDerails.Select(s => s.ld.LiftingQuantityCase).DefaultIfEmpty(0).Sum();
                    var invoiceQty = _emamiContext.InvoiceDetails.AsNoTracking()
                        .Join(_emamiContext.Invoices.AsNoTracking(), invoicedetails => invoicedetails.InvoiceId, invoice => invoice.Id, (invoicedetails, invoice) => new { invoicedetails, invoice })
                        .Where(_ => salesorderId.Contains(_.invoice.LiftingRequestId)).Select(s => s.invoicedetails.ActualBilledQuantity).DefaultIfEmpty(0).Sum();
                    var inprogressQuantity = salesorderQty - invoiceQty;

                    if (saudaorderDetails != null && saudaorderDetails.Any())
                    {
                        var liftingDetailView = new LiftingDetailViewDto();

                        liftingDetailView.CompletedQuantity = invoiceQty;
                        liftingDetailView.InprogressQuantity = /*inprogressQuantity > 0 ?*/ inprogressQuantity /*: 0;*/;
                        //liftingDetailView.CompletedQuantity = saudaorderDetails.Sum(_ => _.InvoiceQuantityCase);
                        liftingDetailView.PendingQuantity = saudaorderDetails.Sum(_ => _.BidQuantity) - salesorderQty;
                        liftingDetailView.PendingQuantityCase = saudaorderDetails.Sum(_ => _.BidQuantityCase) - salesorderQtyCase;

                        liftingDetailView.LiftedSkus = salesOrderDerails.Select(_ => new SaudaOrderDetails
                        {
                            SkuId = _.ld != null ? _.ld.SkuId : 0,
                            SkuName = _.ld != null && _.ld.Sku != null ? _.ld.Sku.SkuName : string.Empty,
                            BidQuantity = _.ld != null ? _.ld.LiftingQuantity : 0,
                            BidQuantityCases = _.ld != null ? _.ld.LiftingQuantityCase : 0,
                            LiftedDate = _.l != null ? _.l.CreatedDate : DateTime.MinValue,
                        }).ToList();
                        if (liftingDetailView != null && liftingDetailView.LiftedSkus != null && liftingDetailView.LiftedSkus.Any())
                        {
                            saudaDetails.LiftingDetails = liftingDetailView;
                        }
                    }

                }
                //}

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

        public ResultDto GetSaudaorderdetails1(SaudaDetailInputDto inputDto)
        {
            _methodName = "GetSaudaorderdetails";
            var resultDto = new ResultDto();
            var saudaDetails = new SaudaDetailOutputDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.SaudaNumber == inputDto.SaudaNumber);
                if (saudaOrderListContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                var saudaContext = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderListContext.SaudaId);
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaContext.UserId);
                var totalBidAmount = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id).Sum(_ => (decimal?)_.BidPrice) ?? 0;
                var totalBidQuantity = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id).Sum(_ => (decimal?)_.BidQuantity) ?? 0;
                var totalQuotedAmount = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id).Sum(_ => (decimal?)_.QuotedPrice) ?? 0;
                var BrokerContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.SaudaId == saudaContext.Id);

                saudaDetails.SaudaNumber = saudaContext.Id.ToString();
                saudaDetails.SaudaDate = saudaContext.BiddingDate;
                saudaDetails.DealerId = saudaContext.UserId;
                saudaDetails.DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaContext.UserId).Name;
                saudaDetails.TotalAmount = totalBidAmount;
                saudaDetails.TotalQuantity = totalBidQuantity;
                saudaDetails.ImpactMargin = totalQuotedAmount - totalBidAmount;
                saudaDetails.SaudaValidityDays = Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity;
                saudaDetails.SaudaExpireDays = (DateHelper.UtcToIndia(DateTime.UtcNow) - saudaContext.BiddingDate).Days;
                saudaDetails.ExpiryDate = saudaContext.BiddingDate.AddDays(Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity);
                saudaDetails.BrokerId = BrokerContext.BrokerId;
                if (BrokerContext != null)
                {
                    saudaDetails.BrokerName = BrokerContext.BrokerId != 0 ? _emamiContext.Users.FirstOrDefault(_ => _.Id == BrokerContext.BrokerId).Name : string.Empty;
                }

                var saudaOrders = new List<SaudaOrderDetails>();

                var saudaOrderItem = new SaudaOrderDetails
                {
                    SkuId = saudaOrderListContext.SkuId,
                    SkuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderListContext.SkuId)?.SkuName,
                    BidPrice = saudaOrderListContext.BidPrice,
                    BidQuantity = saudaOrderListContext.BidQuantity,
                    BidQuantityCases = saudaOrderListContext.BidQuantityCase,
                    IncoTerms = saudaOrderListContext.Incoterms1,
                    Discount = saudaOrderListContext.DiscountAmount,
                    PlantDepot = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderListContext.PlantId)?.Name,
                    //FrieghtRoute = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderListContext.DealerLocationId)?.Name,
                    DiscountTypeId = saudaOrderListContext.DiscountTypeId,
                    StatusId = saudaOrderListContext.StatusId,
                    Status = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderListContext.StatusId)?.Name,
                    SaudaConversionId = _emamiContext.SaudaConversion.AsNoTracking().FirstOrDefault(_ => _.SaudaOrderId == saudaOrderListContext.Id) != null ? _emamiContext.SaudaConversion.AsNoTracking().FirstOrDefault(_ => _.SaudaOrderId == saudaOrderListContext.Id).Id : 0,
                    Remarks = saudaOrderListContext.Remarks,
                    SaudaNumber = saudaOrderListContext.SaudaNumber,
                    BookedDate = saudaOrderListContext.CreatedDate,
                    OilTypeName = saudaOrderListContext.OilType.Name
                };
                saudaOrders.Add(saudaOrderItem);

                saudaDetails.SaudaOrders = saudaOrders;


                if (saudaOrderListContext != null)
                {
                    //Dispatch status

                    IQueryable<SaudaOrderLiftingRequestMapping> liftingReqOrderContextList = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => _.SaudaOrderId == saudaOrderListContext.Id
                        && _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected);
                    if (liftingReqOrderContextList != null && liftingReqOrderContextList.Any())
                    {

                        //var ReturnsInvoiceContext = (from i in _emamiContext.Invoices.AsNoTracking()
                        //                             join id in _emamiContext.InvoiceDetails.AsNoTracking() on i.Id equals id.InvoiceId
                        //                             where i.SalesDocumentType == "ZHCR"
                        //                             && id.SaudaOrderId == saudaOrderListContext.Id
                        //                             select id).ToList();

                        //decimal InvoiceBilledQuantity = 0;
                        //decimal InvoiceBilledQuantityInCase = 0;
                        //if (ReturnsInvoiceContext != null && ReturnsInvoiceContext.Any())
                        //{
                        //    InvoiceBilledQuantity = ReturnsInvoiceContext.Sum(_ => _.ActualBilledQuantity);
                        //    InvoiceBilledQuantityInCase = ReturnsInvoiceContext.Sum(_ => _.QuantityInCase);
                        //}
                        var liftingDetailView = new LiftingDetailViewDto();
                        liftingDetailView.CompletedQuantity = liftingReqOrderContextList.Sum(_ => _.LiftingQuantity);
                        liftingDetailView.PendingQuantity = saudaOrderListContext.BidQuantity - liftingDetailView.CompletedQuantity /*+ InvoiceBilledQuantity*/;
                        liftingDetailView.PendingQuantityCase = saudaOrderListContext.BidQuantityCase - liftingReqOrderContextList.Sum(_ => _.LiftingQuantityCase) /*+ InvoiceBilledQuantityInCase*/;
                        liftingDetailView.LiftedSkus = liftingReqOrderContextList.Join(_emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaNumber == inputDto.SaudaNumber),
                            lr => lr.SaudaOrderId, so => so.Id, (lr, so) => new { lr, so }).Select(_ => new SaudaOrderDetails
                            {
                                SkuId = _.so != null ? _.so.SkuId : 0,
                                SkuName = _.so != null && _.so.Sku != null ? _.so.Sku.SkuName : string.Empty,
                                BidQuantity = _.lr != null ? _.lr.LiftingQuantity : 0,
                                BidQuantityCases = _.lr != null ? _.lr.LiftingQuantityCase : 0,
                                LiftedDate = _.lr != null ? _.lr.CreatedDate : DateTime.MinValue,
                            }).ToList();
                        if (liftingDetailView != null && liftingDetailView.LiftedSkus != null && liftingDetailView.LiftedSkus.Any())
                        {
                            saudaDetails.LiftingDetails = liftingDetailView;
                        }
                    }

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

        public ResultDto GetSkuListByPackGroupId(SkuDropDownInputDto inputDto)
        {
            _methodName = "GetSkuListByPackGroupId";
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

                var skuContext = _emamiContext.Skus.Where(_ => _.OilTypeId == inputDto.OilTypeId && _.PackGroupId == inputDto.PackGroupId && _.IsActive);

                if (!string.IsNullOrEmpty(inputDto.SearchText))
                {
                    skuContext = skuContext.Where(_ => _.SkuName.ToLower().Trim().Contains(inputDto.SearchText.ToLower().Trim()) || _.SkuCode.ToString().ToLower().Trim().Contains(inputDto.SearchText.ToLower().Trim()));
                }

                var skuList = new List<DropDownDto>();

                if (skuContext != null && skuContext.Any())
                {
                    skuList = skuContext
                        .Select(_ => new DropDownDto()
                        {
                            Id = _.Id,
                            Name = _.SkuCode + " - " + _.SkuName
                        }).ToList();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = skuList;
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


        #region ChequeStatus Report

        public ResultDto GetChequeStatusReportDetails(ChequeStatusReportInputDto inputDto)
        {
            _methodName = "GetChequeStatusReportDetails";
            var resultDto = new ResultDto();
            var chequeReportDetails = new List<ChequeStatusReportOutputDto>();
            try
            {
                var UserContext = _emamiContext.Users.AsNoTracking().ToList();
                var UserCustomerMappingContext = _emamiContext.UserCustomerMapping.AsNoTracking().ToList();
                var chequeStatusContext = _emamiContext.ChequeInventoryDetails.AsNoTracking().ToList();

                //for zonal head login
                if (inputDto.ZonalHeadId != 0)
                {
                    var bdoContext = UserContext.Where(_ => _.ReportingToId == inputDto.ZonalHeadId).Select(_ => _.Id).ToList();
                    if ((inputDto.BdoIds == null) && (inputDto.DealerIds == null))
                    {
                        var dealerIdsContext = UserCustomerMappingContext
                                            .Where(_ => bdoContext.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();

                        chequeReportDetails = chequeStatusContext.Where(_ => dealerIdsContext.Contains(_.UserId)).Select(a => new ChequeStatusReportOutputDto
                        {
                            DealerCode = a.UserCode,
                            DealerName = a.UserName,
                            ChequeNo = a.ChequeNo,
                            BankName = a.NameOfBank,
                            BranchName = a.BranchName,
                            BdoCode = UserCustomerMappingContext.FirstOrDefault(_ => _.CustomerId == a.UserId).User != null ? UserCustomerMappingContext.FirstOrDefault(_ => _.CustomerId == a.UserId).User.Code : string.Empty,
                            BdoName = UserCustomerMappingContext.FirstOrDefault(_ => _.CustomerId == a.UserId).User != null ? UserCustomerMappingContext.FirstOrDefault(_ => _.CustomerId == a.UserId).User.Name : string.Empty,
                            CreatedDate = a.CreatedDate
                        }).ToList();
                    }
                    else if ((inputDto.BdoIds != null) && (inputDto.DealerIds == null))
                    {
                        var dealerIdsContext = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => inputDto.BdoIds.Contains(_.UserId)).Select(a => a.CustomerId).ToList();

                        chequeReportDetails = chequeStatusContext.Where(_ => dealerIdsContext.Contains(_.UserId)).Select(a => new ChequeStatusReportOutputDto
                        {
                            DealerCode = a.UserCode,
                            DealerName = a.UserName,
                            ChequeNo = a.ChequeNo,
                            BankName = a.NameOfBank,
                            BranchName = a.BranchName,
                            BdoCode = UserCustomerMappingContext.FirstOrDefault(_ => _.CustomerId == a.UserId).User != null ? UserCustomerMappingContext.FirstOrDefault(_ => _.CustomerId == a.UserId).User.Code : string.Empty,
                            BdoName = UserCustomerMappingContext.FirstOrDefault(_ => _.CustomerId == a.UserId).User != null ? UserCustomerMappingContext.FirstOrDefault(_ => _.CustomerId == a.UserId).User.Name : string.Empty,
                            CreatedDate = a.CreatedDate
                        }).ToList();
                    }
                    else
                    {
                        chequeReportDetails = chequeStatusContext.Where(_ => inputDto.DealerIds.Contains(_.UserId)).Select(a => new ChequeStatusReportOutputDto
                        {
                            DealerCode = a.UserCode,
                            DealerName = a.UserName,
                            ChequeNo = a.ChequeNo,
                            BankName = a.NameOfBank,
                            BranchName = a.BranchName,
                            BdoCode = UserCustomerMappingContext.FirstOrDefault(_ => _.CustomerId == a.UserId).User != null ? UserCustomerMappingContext.FirstOrDefault(_ => _.CustomerId == a.UserId).User.Code : string.Empty,
                            BdoName = UserCustomerMappingContext.FirstOrDefault(_ => _.CustomerId == a.UserId).User != null ? UserCustomerMappingContext.FirstOrDefault(_ => _.CustomerId == a.UserId).User.Name : string.Empty,
                            CreatedDate = a.CreatedDate
                        }).ToList();
                    }
                }
                //StateTrader login
                else if (inputDto.ZonalHeadId == 0 && inputDto.BdoIds != null)
                {
                    if (inputDto.DealerIds == null)
                    {
                        var dealerIdsContext = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => inputDto.BdoIds.Contains(_.UserId)).Select(a => a.CustomerId).ToList();

                        chequeReportDetails = chequeStatusContext.Where(_ => dealerIdsContext.Contains(_.UserId)).Select(a => new ChequeStatusReportOutputDto
                        {
                            DealerCode = a.UserCode,
                            DealerName = a.UserName,
                            ChequeNo = a.ChequeNo,
                            BankName = a.NameOfBank,
                            BranchName = a.BranchName,
                            CreatedDate = a.CreatedDate
                        }).ToList();
                    }
                    else
                    {
                        chequeReportDetails = chequeStatusContext.Where(_ => inputDto.DealerIds.Contains(_.UserId)).Select(a => new ChequeStatusReportOutputDto
                        {
                            DealerCode = a.UserCode,
                            DealerName = a.UserName,
                            ChequeNo = a.ChequeNo,
                            BankName = a.NameOfBank,
                            BranchName = a.BranchName,
                            CreatedDate = a.CreatedDate
                        }).ToList();
                    }
                }
                //Dealer login
                else
                {
                    chequeReportDetails = chequeStatusContext.Where(_ => inputDto.DealerIds.Contains(_.UserId)).Select(a => new ChequeStatusReportOutputDto
                    {
                        DealerCode = a.UserCode,
                        DealerName = a.UserName,
                        ChequeNo = a.ChequeNo,
                        BankName = a.NameOfBank,
                        BranchName = a.BranchName,
                        CreatedDate = a.CreatedDate
                    }).ToList();
                }
                return SucessResult(chequeReportDetails);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        #endregion

        #region Filler Sku

        public ResultDto GetFillerskuForIndentRequest(FillerSkuInputDto inputDto)
        {
            _methodName = "GetFillerskuForIndentRequest";
            var FillerSkuList = new List<FillerSkuOutputDto>();
            //var errorFlag = true;
            //string errorMessageList = string.Empty;
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (inputDto.DealerId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DealerMissing);
                }
                var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.DealerNotFound);
                }

                var PackTypesForJarPet = new List<int> { (int)DTO.Enums.PackTypes.Bottles, (int)DTO.Enums.PackTypes.Jars };
                var PackTypesForPouchTin = new List<int> { (int)DTO.Enums.PackTypes.Pouches, (int)DTO.Enums.PackTypes.Tins, (int)DTO.Enums.PackTypes.BIB, (int)DTO.Enums.PackTypes.Box };
                var fillerSkuContext = _emamiContext.FillerSkuBasedOnDealer.AsNoTracking().Where(_ => _.UserId == inputDto.DealerId).Select(s => new FillerSkuOutputDto
                {
                    SkuId = s.SkuId,
                    SkuName = s.Sku.SkuName,
                    SkuCode = s.Sku.SkuCode,
                    PackTypeId = s.PackTypeId,
                    BidedCases = s.BidQuantityInCases
                });

                if (fillerSkuContext != null && fillerSkuContext.Any())
                {
                    var fillerSkuForJarAndPet = fillerSkuContext.Where(_ => _.PackTypeId == (int)DTO.Enums.PackTypes.Jars || _.PackTypeId == (int)DTO.Enums.PackTypes.Bottles).OrderByDescending(_ => _.BidedCases).Take(3).ToList();
                    var fillerSkuForTinAndPouch = fillerSkuContext.Where(_ => _.PackTypeId == (int)DTO.Enums.PackTypes.Tins || _.PackTypeId == (int)DTO.Enums.PackTypes.Pouches || _.PackTypeId == (int)DTO.Enums.PackTypes.BIB || _.PackTypeId == (int)DTO.Enums.PackTypes.Box).OrderByDescending(_ => _.BidedCases).Take(3).ToList();
                    fillerSkuForJarAndPet.AddRange(fillerSkuForTinAndPouch);
                    foreach (var data in fillerSkuForJarAndPet)
                    {
                        var errorMessage = string.Empty;
                        decimal maxallowable = _emamiContext.VolumeLoadability.AsNoTracking().OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.SkuId == data.SkuId && _.IsActive && _.VehicleSize == inputDto.VehicleSize && _.PlantId == inputDto.PlantId) != null ? _emamiContext.VolumeLoadability.AsNoTracking().OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.SkuId == data.SkuId && _.IsActive && _.VehicleSize == inputDto.VehicleSize && _.PlantId == inputDto.PlantId).MaxAllowableMultiplesku : 0;
                        decimal grossWeight = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == data.SkuId) != null ? _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == data.SkuId).GrossWeight : 0;
                        var skuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == data.SkuId).SkuName;
                        //if (maxallowable == 0)
                        //{
                        //    errorMessage = Constants.MaxAllowableMissing + skuName;
                        //    errorFlag = false;
                        //}
                        //if (grossWeight == 0)
                        //{
                        //    errorMessage = Constants.GrossWeightMissing + skuName;
                        //    errorFlag = false;
                        //}
                        //if (errorFlag)
                        //{
                        decimal casesCanBeFilledToVolume = ((100 - inputDto.Volumepercentage) / 100) * maxallowable;
                        decimal casesCanBeFilledToWeight = ((100 - inputDto.Weightpercentage) / 100) * (inputDto.VehicleSize / grossWeight);
                        var output = new FillerSkuOutputDto()
                        {
                            SkuName = data.SkuName,
                            SkuCode = data.SkuCode,
                            SuggestedQuantity = Math.Floor(Math.Min(casesCanBeFilledToVolume, casesCanBeFilledToWeight)),
                            SkuId = data.SkuId,
                            CaseToMetricTon = _resultService.ConvertCasetoMetricTon(1, data.SkuId),
                            MaxAllowableSingleSku = _emamiContext.VolumeLoadability.AsNoTracking().OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.SkuId == data.SkuId && _.IsActive && _.VehicleSize == inputDto.VehicleSize && _.PlantId == inputDto.PlantId) != null ? _emamiContext.VolumeLoadability.AsNoTracking().OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.SkuId == data.SkuId && _.IsActive && _.VehicleSize == inputDto.VehicleSize && _.PlantId == inputDto.PlantId).MaxAllowableSinglesku : 0,
                            MaxAllowableMultipleSku = _emamiContext.VolumeLoadability.AsNoTracking().OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.SkuId == data.SkuId && _.IsActive && _.VehicleSize == inputDto.VehicleSize && _.PlantId == inputDto.PlantId) != null ? _emamiContext.VolumeLoadability.AsNoTracking().OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.SkuId == data.SkuId && _.IsActive && _.VehicleSize == inputDto.VehicleSize && _.PlantId == inputDto.PlantId).MaxAllowableMultiplesku : 0,
                            GrossWeight = grossWeight,
                            PackTypeId = data.PackTypeId,
                            OilTypeId = data.SkuId > 0 ? (_emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == data.SkuId).OilTypeId ?? 0) : 0
                        };
                        FillerSkuList.Add(output);
                        // }

                        //if (!errorFlag)
                        //{
                        //    if (!string.IsNullOrEmpty(errorMessageList))
                        //    {
                        //        errorMessageList = Constants.BindErrorMessage(System.Environment.NewLine + errorMessage, errorMessageList);
                        //    }
                        //    else
                        //    {
                        //        errorMessageList = Constants.BindErrorMessage(errorMessage, errorMessageList);
                        //    }
                        //}
                    }

                    //if (!errorFlag)
                    //{
                    //    return _resultService.ErrorMessage(errorMessageList);
                    //}
                }
                return _resultService.SuccessObject(FillerSkuList);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetSpecialRateRequestListNew(SpecialRateInputDto specialRateInputDto)
        {
            var specialRateListDto = new List<SpecialRateOutputDto>();
            _methodName = "GetSpecialRateRequestListNew";
            try
            {
                if (specialRateInputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                //Sales person
                if (specialRateInputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == specialRateInputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                IQueryable<SpecialRate> specialRateListContext;

                var specialRateApproval = _emamiContext.SpecialRateApproval.AsNoTracking().Where(_ => _.RequestedTo == specialRateInputDto.LoginUserId || _.CreatedBy == specialRateInputDto.LoginUserId);
                List<long> specialRateIds = specialRateApproval.Select(_ => _.SpecialRateId).Distinct().ToList();

                if (specialRateInputDto.DealerId != null && specialRateInputDto.OilTypeId != null && specialRateInputDto.FromDate.HasValue && specialRateInputDto.ToDate.HasValue)
                {
                    specialRateListContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => _.UserId == specialRateInputDto.DealerId
                            && _.OilTypeId == specialRateInputDto.OilTypeId && _.CreatedDate >= specialRateInputDto.FromDate && _.CreatedDate <= specialRateInputDto.ToDate && specialRateIds.Contains(_.Id));
                }
                else if ((specialRateInputDto.DealerId != 0 && specialRateInputDto.DealerId != null) || (specialRateInputDto.OilTypeId != 0 && specialRateInputDto.OilTypeId != null)
                    || (specialRateInputDto.FromDate.HasValue && specialRateInputDto.FromDate != DateTime.MinValue) || (specialRateInputDto.ToDate.HasValue && specialRateInputDto.ToDate != DateTime.MinValue))
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }
                else
                {
                    IQueryable<UserCustomerMapping> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == specialRateInputDto.LoginUserId);
                    if (dealersList != null && dealersList.Any())
                    {
                        specialRateListContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => dealersList.Any(a => a.CustomerId == _.UserId) && specialRateIds.Contains(_.Id));
                    }
                    else
                    {
                        specialRateListContext = null;
                    }
                }
                if (specialRateListContext != null && specialRateListContext.Any())
                {
                    var cityContext = _emamiContext.City.AsNoTracking().ToList();
                    var stateContext = _emamiContext.State.AsNoTracking().ToList();

                    var specialRateList = specialRateListContext.Join(_emamiContext.Users.AsNoTracking(), sr => sr.UserId, u => u.Id, (sr, u) => new { sr, u })
                        .Join(_emamiContext.UserRoles.AsNoTracking(), sru => sru.u.Id, ur => ur.UserId, (sru, ur) => new { sru.sr, sru.u, ur }).Where(_ => _.sr != null && _.u != null && _.ur != null).ToList();
                    foreach (var specialRateContext in specialRateList)
                    {
                        var specialRateOutputDto = new SpecialRateOutputDto();
                        specialRateOutputDto.SpecialRateId = specialRateContext.sr.Id;
                        specialRateOutputDto.DealerId = specialRateContext.sr.UserId;
                        specialRateOutputDto.DealerName = string.Concat((specialRateContext.sr.User != null ? specialRateContext.sr.User.Name : string.Empty) + "-" + (cityContext.FirstOrDefault(c => c.Id == specialRateContext.sr.User.CityId).CityName != null ? cityContext.FirstOrDefault(c => c.Id == specialRateContext.sr.User.CityId).CityName : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == specialRateContext.sr.User.StateId).StateName != null ? stateContext.FirstOrDefault(s => s.Id == specialRateContext.sr.User.StateId).StateName : string.Empty) + "-" + (specialRateContext.sr.User != null ? specialRateContext.sr.User.Code : string.Empty));
                        specialRateOutputDto.RequestDate = specialRateContext.sr.CreatedDate;
                        specialRateOutputDto.StatusId = specialRateContext.sr.StatusId;
                        specialRateOutputDto.StatusName = specialRateContext.sr.Status != null ? specialRateContext.sr.Status.Name : string.Empty;
                        specialRateOutputDto.IsBroker = specialRateContext.ur.RoleId == (int)DTO.Enums.Role.Broker ? true : false;
                        specialRateOutputDto.IsLTD = specialRateContext.sr.IsLTD;
                        specialRateOutputDto.SkuId = specialRateContext.sr.SkuId;
                        specialRateOutputDto.SkuName = specialRateContext.sr.Sku != null ? specialRateContext.sr.Sku.SkuName + "-" + specialRateContext.sr.Sku.SkuCode : string.Empty;
                        specialRateOutputDto.SpecialPrice = specialRateContext.sr.SpecialPrice;
                        specialRateOutputDto.Quantity = specialRateContext.sr.QuantityCase;

                        specialRateListDto.Add(specialRateOutputDto);
                    }
                }
                if (specialRateListDto != null && specialRateListDto.Any())
                {
                    return _resultService.SuccessObject(specialRateListDto);
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
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion

        #region SaudaExtensionDays

        public ResultDto PostSaudaExtensionDays(SaudaExtensionDaysDto inputDto)
        {
            _methodName = "PostSaudaExtensionDays";
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
                if (string.IsNullOrEmpty(inputDto.SaudaNumbers))//list of long
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.SaudaNumbersMissing, Constants.EnglishLanguage);
                    return resultDto;
                }

                var extensionDaysKey = UtilityHelper.GetEnumDescription((DTO.Enums.Configuration.SaudaExtensionDays));
                var extensionDays = Convert.ToInt64(_emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Key == extensionDaysKey)?.Value ?? "0");

                List<string> saudaNumbersList = new List<string>();
                if (!string.IsNullOrEmpty(inputDto.SaudaNumbers))
                {
                    saudaNumbersList = inputDto.SaudaNumbers.Split(',').ToList();
                }
                resultDto = _sapIntegrationService.SaudaExtenstionAPPToSAP(saudaNumbersList, extensionDays);
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

        public ResultDto GetSAPSaudaExtensionList(SAPSaudaInputDto inputDto)
        {
            _methodName = "GetSAPSaudaExtensionList";
            var resultDto = new ResultDto();
            var outputDto = new SAPSaudaExtensionOutputDto();
            outputDto.SAPSaudaExtensionList = new List<SAPSaudaExtensionDto>();
            try
            {
                var saudaExtension = _emamiContext.SaudaExtensionDetailsApprovals.AsNoTracking()
                    .Where(_ => _.IsSAPDataSync
                    && DbFunctions.TruncateTime(_.ModifiedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                        && DbFunctions.TruncateTime(_.ModifiedDate) <= DbFunctions.TruncateTime(inputDto.ToDate));



                if (inputDto.StatusId > 0)
                {
                    saudaExtension = saudaExtension.Where(_ => _.IsApproval == (inputDto.StatusId == 1 ? false : true));
                }

                var pageSize = Constants.PageSize;
                var skip = pageSize * inputDto.PageNo;

                outputDto.ListCount = saudaExtension.Count();
                outputDto.SAPSaudaExtensionList = saudaExtension.ToList()
                    .Select(_ => new SAPSaudaExtensionDto()
                    {
                        Id = _.Id,
                        SaudaNumber = _.SaudaNumber,
                        StatusId = _.IsApproval == true ? 2 : 1,
                        Status = _.IsApproval == true ? "Approved" : "Pending",
                        SAPRemarks = _.SAPRemarks,
                    }).OrderByDescending(_ => _.Id).ToList().Skip(skip).Take(pageSize).ToList();
                resultDto.SuccessDto.Response = outputDto;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        #endregion

        #region SaudaReleaseToSAP

        public ResultDto SaudaReleaseToSAP(SAPSaudaInputDto inputDto)
        {
            _methodName = "SaudaReleaseToSAP";
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
                if (string.IsNullOrEmpty(inputDto.SaudaNumbers))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.SaudaNumbersMissing, Constants.EnglishLanguage);
                    return resultDto;
                }

                if (!string.IsNullOrEmpty(inputDto.SaudaNumbers))
                {
                    List<string> saudaNumbersList = inputDto.SaudaNumbers.Split(',').ToList();
                    resultDto = _sapIntegrationService.SaudaReleaseAPPToSAP(saudaNumbersList);
                }
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

        public ResultDto GetSAPSaudaReleaseList(SAPSaudaInputDto inputDto)
        {
            _methodName = "GetSAPSaudaReleaseList";
            var resultDto = new ResultDto();
            var outputDto = new SAPSaudaExtensionOutputDto();
            outputDto.SAPSaudaExtensionList = new List<SAPSaudaExtensionDto>();
            try
            {
                var saudaExtension = _emamiContext.SaudaOrders.AsNoTracking()
                    .Join(_emamiContext.Remarks.AsNoTracking(), so => so.Id, r => r.TableId, (so, r) => new { so, r })
                    .Where(_ => DbFunctions.TruncateTime(_.so.SaudaReleaseDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                        && DbFunctions.TruncateTime(_.so.SaudaReleaseDate) <= DbFunctions.TruncateTime(inputDto.ToDate));

                if (inputDto.StatusId > 0)
                {
                    saudaExtension = saudaExtension.Where(_ => _.so.StatusId == inputDto.StatusId);
                }
                var pageSize = Constants.PageSize;
                var skip = pageSize * inputDto.PageNo;

                outputDto.ListCount = saudaExtension.Count();
                outputDto.SAPSaudaExtensionList = saudaExtension.ToList()
                    .Select(_ => new SAPSaudaExtensionDto()
                    {
                        Id = _.so.Id,
                        SaudaNumber = _.so.SaudaNumber,
                        StatusId = _.so.StatusId,
                        Status = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(f => f.Id == _.so.StatusId)?.Name,
                        SAPRemarks = _.r.Description,
                    }).OrderByDescending(_ => _.Id).ToList().Skip(skip).Take(pageSize).ToList();

                resultDto.SuccessDto.Response = outputDto;
                resultDto.IsSuccess = true;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto AddSaudaLimitHistory(SaudaLimitHistoryDto inputDto)
        {
            _methodName = "AddSaudaLimitHistory";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto != null)
                {
                    var userdivContext = _emamiContext.UserDivisionMappings.AsNoTracking()
                              .FirstOrDefault(_ => _.UserId == inputDto.DealerId
                              && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId
                              && _.DivisionId == inputDto.DivisionId);

                    //var userDto = _emamiContext.Users.FirstOrDefault(user => user.Id == inputDto.DealerId);
                    if (userdivContext == null)
                    {
                        return _resultService.ErrorMessage(Constants.RecordNotFound);
                    }
                    else
                    {
                        userdivContext.SaudaLimit = inputDto.NewSaudaLimit;
                        userdivContext.ModifiedBy = inputDto.LoginUserId;
                        userdivContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.Now);
                        _emamiContext.SaveChanges();
                    }

                    var saudalimithistory = new SaudaLimitHistory
                    {
                        UserId = inputDto.DealerId,
                        OldSaudaLimit = inputDto.OldSaudaLimit,
                        NewSaudaLimit = inputDto.NewSaudaLimit,
                        Remarks = inputDto.Remarks,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.Now)
                    };
                    _emamiContext.SaudaLimitHistory.Add(saudalimithistory);
                    _emamiContext.SaveChanges();
                    resultDto.IsSuccess = true;
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
            }
            return resultDto;
        }

        public ResultDto GetSaudaLimitHistoryList(SaudaLimitHistoryDto inputDto)
        {
            var resultDto = new ResultDto();
            var saudalimithistoryLisyt = new List<SaudaLimitHistoryDto>();
            try
            {
                if (inputDto.DealerId == 0)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }
                else
                {
                    var saudaList = _emamiContext.SaudaLimitHistory.AsNoTracking().Where(sauda => sauda.UserId == inputDto.DealerId).ToList();

                    saudalimithistoryLisyt = saudaList
                        .Select(_ => new SaudaLimitHistoryDto()
                        {
                            Id = _.Id,
                            OldSaudaLimit = _.OldSaudaLimit,
                            NewSaudaLimit = _.NewSaudaLimit,
                            CreatedDate = _.CreatedDate
                        }).OrderByDescending(_ => _.Id).Take(3).ToList();
                }

                if (saudalimithistoryLisyt != null && saudalimithistoryLisyt.Any())
                {
                    return _resultService.SuccessObject(saudalimithistoryLisyt);
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion

        #region CompetitorAnalysis - Price Discovery

        /// <summary>
        /// Method to Save CompetitorAnalysis
        /// </summary>
        /// <param name="employeeDto"></param>
        /// <returns></returns>
        public ResultDto SaveCompetitorAnalysis(CompetitorAnalysisInputDto competitorAnalysisInputDto)
        {
            _methodName = "SaveCompetitorAnalysis";
            var resultDto = new ResultDto();
            try
            {

                var errorMessageList = string.Empty;
                var errorFlag = false;
                if (competitorAnalysisInputDto == null || competitorAnalysisInputDto.CompetitorAnalysisList == null || !competitorAnalysisInputDto.CompetitorAnalysisList.Any())
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                List<CompetitorAnalysisAddDto> inputDtoList = competitorAnalysisInputDto.CompetitorAnalysisList.ToList();
                int loginUserId = inputDtoList.FirstOrDefault() != null ? inputDtoList.FirstOrDefault().LoginUserId : 0;
                if (loginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == loginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var requestTo = 0L;

                var requestedTo = userContext.ReportingToId;
                if (requestedTo != null)
                {
                    requestTo = (long)requestedTo;
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequestToUser);
                }

                foreach (var inputDto in inputDtoList)
                {
                    var errorMessage = string.Empty;
                    if (inputDto.SkuId == 0)
                    {
                        errorMessage = Constants.SKUMissing;
                        errorFlag = true;
                    }
                    else
                    {
                        var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SkuId);
                        if (skuContext == null)
                        {
                            errorMessage = Constants.BindErrorMessage(Constants.SKUNotFound, errorMessage);
                            errorFlag = true;
                        }
                        else
                        {
                            errorMessage = skuContext.SkuName;
                            if (inputDto.OilTypeId == 0)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.OilTypeMissing, errorMessage);
                                errorFlag = true;
                            }
                            var oilTypeContext = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.OilTypeId);
                            if (oilTypeContext == null)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.OilTypeNotFound, errorMessage);
                                errorFlag = true;
                            }
                            if (inputDto.EmamiPrice == 0)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.EmamiPriceMissing, errorMessage);
                                errorFlag = true;
                            }
                            if (inputDto.WorkableQuantity == 0)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.WorkableQuantityMissing, errorMessage);
                                errorFlag = true;
                            }
                            if (inputDto.WorkablePrice == 0)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.WorkablePriceMissing, errorMessage);
                                errorFlag = true;
                            }
                            if (inputDto.CompetitorAnalysisDetailsList == null || !inputDto.CompetitorAnalysisDetailsList.Any())
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.CompetitorAnalysDetailsMissing, errorMessage);
                                errorFlag = true;
                            }
                            foreach (var inputDetails in inputDto.CompetitorAnalysisDetailsList)
                            {
                                if (inputDetails.CompetitorId == 0)
                                {
                                    errorMessage = Constants.BindErrorMessage(Constants.CompetitorMissing, errorMessage);
                                    errorFlag = true;
                                }
                                else
                                {
                                    var competitorContext = _emamiContext.Competitor.AsNoTracking().FirstOrDefault(_ => _.Id == inputDetails.CompetitorId);
                                    if (competitorContext == null)
                                    {
                                        errorMessage = Constants.BindErrorMessage(Constants.CompetitorNotFound, errorMessage);
                                        errorFlag = true;
                                    }
                                    else
                                    {
                                        errorMessage = Constants.BindErrorMessage(competitorContext.Name, errorMessage);
                                        if (inputDetails.SaudaRate == 0)
                                        {
                                            errorMessage = Constants.BindErrorMessage(Constants.SaudaRateMissing, errorMessage);
                                            errorFlag = true;
                                        }
                                        if (inputDetails.MarketOperatingPrice == 0)
                                        {
                                            errorMessage = Constants.BindErrorMessage(Constants.MarketOperatingPriceMissing, errorMessage);
                                            errorFlag = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (errorFlag)
                    {
                        if (!string.IsNullOrEmpty(errorMessageList))
                        {
                            errorMessageList = Constants.BindErrorMessage(System.Environment.NewLine + errorMessage, errorMessageList);
                        }
                        else
                        {
                            errorMessageList = Constants.BindErrorMessage(errorMessage, errorMessageList);
                        }
                    }
                }
                foreach (var inputDto in inputDtoList)
                {
                    if (!errorFlag)
                    {
                        var input = new CompetitorAnalysis
                        {
                            SkuId = inputDto.SkuId,
                            OilTypeId = inputDto.OilTypeId,
                            StatusId = (int)DTO.Enums.Status.Pending,
                            Margin = inputDto.Margin,
                            EmamiPrice = inputDto.EmamiPrice,
                            WorkableQuantity = inputDto.WorkableQuantity,
                            WorkablePrice = inputDto.WorkablePrice,
                            Remarks = inputDto.Remarks,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.CompetitorAnalysis.Add(input);
                        _emamiContext.SaveChanges();

                        if (inputDto.CompetitorAnalysisDetailsList != null && inputDto.CompetitorAnalysisDetailsList.Any())
                        {
                            foreach (var inputDetails in inputDto.CompetitorAnalysisDetailsList)
                            {
                                var context = new CompetitorAnalysisDetails
                                {
                                    CompetitorAnalysisId = input.Id,
                                    CompetitorId = inputDetails.CompetitorId,
                                    SaudaRate = inputDetails.SaudaRate,
                                    MarketOperatingPrice = inputDetails.MarketOperatingPrice,
                                    CreatedBy = inputDto.LoginUserId,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                                };
                                _emamiContext.CompetitorAnalysisDetails.Add(context);
                            }
                            _emamiContext.SaveChanges();
                        }


                        var competitorAnalysisApproval = new CompetitorAnalysisApproval
                        {
                            CompetitorAnalysisId = input.Id,
                            RequestedBy = inputDto.LoginUserId,
                            RequestedTo = requestTo,
                            StatusId = (int)DTO.Enums.Status.Pending,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.CompetitorAnalysisApproval.Add(competitorAnalysisApproval);
                        _emamiContext.SaveChanges();
                    }
                }
                if (errorFlag)
                {
                    return _resultService.ErrorMessage(errorMessageList);
                }
                else
                {
                    return _resultService.SuccessMessage(Constants.CompetitorAnalysisSaveSuccess);
                }
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

        public ResultDto GetValidPendingContractByDelaerId(UserIdDto inputDto)
        {
            _methodName = "GetValidPendingContractByDelaerId";
            var validPendingContracts = new List<ContractNoListDto>();
            var resultDto = new ResultDto();

            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow).Date;

            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var delaerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.UserId);
                if (delaerContext == null)
                {
                    return _resultService.ErrorMessage(Constants.DealerNotFound);
                }

                var rawData =
                (
                    from pc in _emamiContext.PendingContracts.AsNoTracking()
                    join sauda in _emamiContext.Sauda.AsNoTracking()
                        on pc.SaudaNumber equals sauda.SaudaNumber
                    where
                        pc.UserId == inputDto.UserId &&
                        pc.SaudaNumber != null &&
                        (pc.ContractValidFrom == null || DbFunctions.TruncateTime(pc.ContractValidFrom) <= currentDate) &&
                        (pc.ContractValidTo == null || DbFunctions.TruncateTime(pc.ContractValidTo) >= currentDate)
                    select new
                    {
                        sauda.Id,
                        sauda.SaudaNumber,
                        sauda.BiddingDate
                    }
                ).ToList();

                validPendingContracts = rawData
                .GroupBy(s => s.SaudaNumber)                     // DISTINCT by SaudaNumber
                .Select(g => g.OrderByDescending(x => x.BiddingDate).First()) // pick latest BiddingDate
                .OrderBy(x => x.BiddingDate)           // ORDER final list by BiddingDate
                .Select(s => new ContractNoListDto               // build DTO after grouping
                {
                    Id = s.Id,
                    SaudaNumber = s.SaudaNumber != null && s.BiddingDate != null
                        ? s.SaudaNumber + "-" + s.BiddingDate.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture)
                        : string.Empty
                })
                .ToList();


                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = validPendingContracts;
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

        public ResultDto GetOilTypesByPendingContractId(SaudaDetailInputDto inputDto)
        {
            _methodName = "GetOilTypesByPendingContractId";
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

                var pendingContractContext = _emamiContext.PendingContracts.AsNoTracking().Where(_ => _.SaudaNumber == inputDto.SaudaNumber);
                if (pendingContractContext == null)
                {
                    return _resultService.ErrorMessage(Constants.SaudaNotFound);
                }

                var SkuContext = _emamiContext.Skus.AsNoTracking().ToList();
                var oilTypesContext = _emamiContext.OilTypes.AsNoTracking().ToList();

                var pendingMaterials = pendingContractContext
                .Select(x => new PendingContractMaterialInfoDTO
                {
                    MaterialCode = x.MaterialCode,
                    SalesOrgId = x.SalesOrgId,
                    DistChnlId = x.DistChnlId,
                    DivisionId = x.DivisionId,
                    BasicRate = x.BasicRate,
                    PendingQuantityInCase = x.PendingQuantityInCase,
                    SaudaQuantity = x.SaudaQuantity
                })
                .Distinct()
                .ToList();

                var skuDetails = (from p in pendingMaterials
                                  join sku in SkuContext on new
                                  {
                                      MaterialCode = p.MaterialCode,
                                      SalesOrganizationId = p.SalesOrgId,
                                      DistributionChannelId = p.DistChnlId,
                                      DivisionId = p.DivisionId
                                  }
                                  equals new
                                  {
                                      MaterialCode = sku.SkuCode,
                                      SalesOrganizationId = sku.SalesOrganizationId,
                                      DistributionChannelId = sku.DistributionChannelId,
                                      DivisionId = sku.DivisionId
                                  }
                                  select new SaudaModificaitonFromSkuInfoDTO
                                  {
                                      Id = sku.Id,
                                      SkuCode = sku.SkuCode,
                                      SkuName = sku.SkuName,
                                      OilTypeId = sku.OilTypeId,
                                      PendingQuantityInCase = p.PendingQuantityInCase,
                                      BasicRate = p.BasicRate,
                                      SaudaQuantity = p.SaudaQuantity
                                  }).ToList();

                foreach (var sku in skuDetails)
                {
                    sku.CaseToMetricTonValue = _resultService.ConvertCasetoMetricTon(1, sku.Id);
                }


                var result = skuDetails
                    .Where(x => x.OilTypeId != null)
                    .GroupBy(x => x.OilTypeId)
                    .Select(g => new
                    {
                        OilTypeId = g.Key,
                        OilTypeName = oilTypesContext
                            .First(mt => mt.Id == g.Key).Name,
                        Materials = g.Select(x => new
                        {
                            x.Id,
                            x.SkuCode,
                            x.SkuName,
                            x.PendingQuantityInCase,
                            x.SaudaQuantity,
                            x.CaseToMetricTonValue
                        }).ToList()
                    })
                    .ToList();


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

        public ResultDto GetToSkusBasedOnFromSkuOilType(SaudaMofificationFromSkuInfoDto inputDto)
        {
            _methodName = "GetToSkusBasedOnFromSkuOilType";
            var skuList = new List<SkuDropDown>();
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

                var fromSkuContext = _emamiContext.Skus.AsNoTracking().Where(_ => _.Id == inputDto.SkuId).FirstOrDefault();
                if (fromSkuContext == null)
                {
                    return _resultService.ErrorMessage(Constants.FromSkuNotFound);
                }

                if (inputDto.IsToReturnInactiveData)
                {
                    skuList = _emamiContext.Skus.AsNoTracking().Where(w => w.OilTypeId == inputDto.OilTypeId && w.PackTypeId == fromSkuContext.PackTypeId && w.Id != fromSkuContext.Id)
                    .Select(s => new SkuDropDown() { SkuId = s.Id, SkuName = s.SkuName + "-" + s.SkuCode, Code = s.SkuCode }).ToList();
                }
                else
                {
                    skuList = _emamiContext.Skus.AsNoTracking().Where(w => w.OilTypeId == inputDto.OilTypeId && w.PackTypeId == fromSkuContext.PackTypeId && w.IsActive && w.Id != fromSkuContext.Id) 
                    .Select(s => new SkuDropDown() { SkuId = s.Id, SkuName = s.SkuName + "-" + s.SkuCode, Code = s.SkuCode }).ToList();
                }

                foreach (var sku in skuList)
                {
                    sku.CaseToMetricTonValue = _resultService.ConvertCasetoMetricTon(1, sku.SkuId);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = skuList;
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

        public ResultDto GetPendingContractDetailsByPendingContract(SaudaDetailInputDto inputDto)
        {
            _methodName = "GetPendingContractDetailsByPendingContract";
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

                var pendingMaterials =
                (
                    from pc in _emamiContext.PendingContracts.AsNoTracking()

                    join s in _emamiContext.Sauda.AsNoTracking()
                        on pc.SaudaNumber equals s.SaudaNumber

                    join so in _emamiContext.SaudaOrders.AsNoTracking()
                        on s.Id equals so.SaudaId

                    join sku in _emamiContext.Skus.AsNoTracking()
                        on so.SkuId equals sku.Id

                    where pc.SaudaNumber == inputDto.SaudaNumber
                          && sku.SkuCode == pc.MaterialCode

                    select new PendingContractMaterialInfoDTO
                    {
                        MaterialCode = pc.MaterialCode,
                        SalesOrgId = pc.SalesOrgId,
                        DistChnlId = pc.DistChnlId,
                        DivisionId = pc.DivisionId,

                        BasicRate = so.QuotedPriceBeforeSAPDiscount > 0
                          ? so.QuotedPriceBeforeSAPDiscount
                          : (so.BidQuantityCase > 0 ? so.BidPrice / so.BidQuantityCase : 0),

                        PendingQuantityInCase = pc.OpenSalesOrderQuantity,
                        SaudaQuantity = pc.PendingQuantityInCase > 0
                             ? (pc.SaudaQuantity / pc.PendingQuantityInCase) * pc.OpenSalesOrderQuantity
                             : 0m
                    }
                )
                .ToList();

                if (!pendingMaterials.Any())
                {
                    return _resultService.ErrorMessage(Constants.SaudaNotFound);
                }

                var SkuContext = _emamiContext.Skus.AsNoTracking().ToList();
                var oilTypesContext = _emamiContext.OilTypes.AsNoTracking().ToList();

                var skuDetails = (from p in pendingMaterials
                                  join sku in SkuContext on new
                                  {
                                      MaterialCode = p.MaterialCode,
                                      SalesOrganizationId = p.SalesOrgId,
                                      DistributionChannelId = p.DistChnlId,
                                      DivisionId = p.DivisionId
                                  }
                                  equals new
                                  {
                                      MaterialCode = sku.SkuCode,
                                      SalesOrganizationId = sku.SalesOrganizationId,
                                      DistributionChannelId = sku.DistributionChannelId,
                                      DivisionId = sku.DivisionId
                                  }
                                  select new SaudaModificaitonFromSkuInfoDTO
                                  {
                                      Id = sku.Id,
                                      SkuCode = sku.SkuCode,
                                      SkuName = sku.SkuName,
                                      OilTypeId = sku.OilTypeId,
                                      BasicRate = p.BasicRate,
                                      OilPackGroupTypeId = sku.OilPackGroupTypeId,
                                      PendingQuantityInCase = p.PendingQuantityInCase,
                                      SaudaQuantity = p.SaudaQuantity
                                  }).ToList();

                foreach (var sku in skuDetails)
                {
                    sku.CaseToMetricTonValue = _resultService.ConvertCasetoMetricTon(1, sku.Id);
                }


                //var result = skuDetails
                //    .Where(x => x.OilTypeId != null)
                //    .GroupBy(x => x.OilTypeId)
                //    .Select(g => new
                //    {
                //        OilTypeId = g.Key,
                //        OilTypeName = oilTypesContext
                //            .First(mt => mt.Id == g.Key).Name,
                //        Materials = g.Select(x => new
                //        {
                //            x.Id,
                //            x.SkuCode,
                //            x.SkuName,
                //            x.PendingQuantityInCase,
                //            x.SaudaQuantity,
                //            x.CaseToMetricTonValue
                //        }).ToList()
                //    })
                //    .ToList();

                var result = new PendingContractDetails
                {
                    OilTypes = skuDetails
                    .GroupBy(s => s.OilTypeId)
                    .Select(oilGroup => new PendingContractOilTypeDetails
                    {
                        OilTypeId = oilGroup.Key,
                        OilTypeName = oilTypesContext
                            .FirstOrDefault(o => o.Id == oilGroup.Key)?.Name ?? "Unknown",

                        PackTypes = oilGroup
                            .GroupBy(p => p.OilPackGroupTypeId)
                            .Select(packGroup => new PendingContractPackTypeDetails
                            {
                                PackTypeId = packGroup.Key,
                                PackTypeName = (packGroup.Key == null ? "Unknown" : (packGroup.Key == (int)DTO.Enums.BpCpType.BP ? "BP" : (packGroup.Key == (int)DTO.Enums.BpCpType.CP ? "CP" : "Unknown"))),

                                OriginalMT = packGroup.Sum(x => x.SaudaQuantity),
                                ModifiedMT = packGroup.Sum(x => x.SaudaQuantity),
                                DifferenceMT = packGroup.Sum(x => x.SaudaQuantity) - packGroup.Sum(x => x.SaudaQuantity),

                                Skus = packGroup.Select(s => new PendingContractSkuDetails
                                {
                                    SkuId = s.Id,
                                    SkuName = s.SkuName,
                                    SkuCode = s.SkuCode,
                                    PendingQuantityInCase = s.PendingQuantityInCase,
                                    SaudaQuantity = s.SaudaQuantity,
                                    BasicRate = s.BasicRate,
                                    CaseToMetricTonValue = s.CaseToMetricTonValue
                                }).ToList()
                            })
                            .ToList()
                    })
                    .ToList()
                };



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

        public ResultDto GetToSkusForSaudaModification(SaudaMofificationFromSkuDetailsDto inputDto)
        {
            _methodName = "GetToSkusForSaudaModification";
            var resultDto = new ResultDto();
            var outputDto = new List<FinalPriceSkuOutputDto>();
            List<string> LineIds = new List<string>();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var cityId = 0;
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                cityId = Convert.ToInt32(userContext.CityId);

                var saudaContext = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.SaudaNumber == inputDto.SaudaNumber);
                if (saudaContext == null)
                {
                    return _resultService.ErrorMessage(Constants.SaudaNotFound);
                }

                var plantId = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.SaudaId == saudaContext.Id).PlantId;

                if (plantId == 0)
                    return _resultService.ErrorMessage(Constants.PlantMissing);

                var userrole = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(user => user.UserId == inputDto.LoginUserId).RoleId;
                var skuDatas = _emamiContext.Skus.AsNoTracking()
                    .Select(s => new
                    {
                        Id = s.Id,
                        Name = s.SkuName + "-" + s.SkuCode + "-" + s.PackGroup.Name,
                        Code = s.SkuCode,
                        OilType = s.OilTypeId,
                        Quantity = s.Quantity,
                        UomId = s.UomId,
                        s.PremiumAmount,
                        s.StorageLocation,
                        OilPackGroupTypeId = s.OilPackGroupTypeId
                    }).ToList();

                var tempoutput = _emamiContext.TodayPricingBackups.AsNoTracking().Join(_emamiContext.Skus.AsNoTracking(), t => t.SkuId, s => s.Id, (t, s) => new { t, s }).Where(_ =>
                       _.t.PlantId == plantId && _.t.OilTypeId == inputDto.OilTypeId && _.s.OilPackGroupTypeId == inputDto.OilPackGroupTypeId
                       && _.t.SkuId != 0
                       && (DbFunctions.TruncateTime(saudaContext.BiddingDate) >= DbFunctions.TruncateTime(_.t.ValidFrom)
                       && DbFunctions.TruncateTime(saudaContext.BiddingDate) <= DbFunctions.TruncateTime(_.t.ValidTo)) && _.s.IsActive)/*.Take(2000000)*/.AsQueryable();

                if (tempoutput == null)
                    return _resultService.ErrorMessage(Constants.SkuMissingInTodayPricing);

                outputDto = tempoutput.Select(s => s.t).OrderByDescending(_ => _.Id).ToList()
                        .Select(_ => new FinalPriceSkuOutputDto
                        {
                            PricingId = _.Id,
                            SkuId = _.SkuId,
                            SkuName = skuDatas.FirstOrDefault(s => s.Id == _.SkuId) != null ? skuDatas.FirstOrDefault(s => s.Id == _.SkuId).Name : "",
                            PlantId = _.PlantId,
                            Price = _.Price,
                            DistributionChannelId = _.DistributionChannelId,
                            DivisionId = _.DivisionId,
                            SalesOrganizationId = _.SalesOrganizationId,
                            OilTypeId = skuDatas.FirstOrDefault(s => s.Id == _.SkuId) != null ? skuDatas.FirstOrDefault(s => s.Id == _.SkuId).OilType.GetValueOrDefault() : 0,
                            OilPackGroupTypeId = skuDatas.FirstOrDefault(s => s.Id == _.SkuId) != null ? skuDatas.FirstOrDefault(s => s.Id == _.SkuId).OilPackGroupTypeId : null,
                        }).ToList();
                LineIds = userContext.LineId != null ? userContext.LineId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() : null;

                var RecentPricings = from e in outputDto
                                     group e by new { e.SkuId, e.PlantId, e.SalesOrganizationId, e.DistributionChannelId, e.DivisionId } into dptgrp
                                     let topsal = dptgrp.Max(x => x.PricingId)
                                     select new FinalPriceSkuOutputDto
                                     {
                                         SkuId = dptgrp.Key.SkuId,
                                         PlantId = dptgrp.Key.PlantId,
                                         Price = dptgrp.First(y => y.PricingId == topsal).Price,
                                         PricingId = dptgrp.First(y => y.PricingId == topsal).PricingId,
                                         SkuName = dptgrp.First(y => y.PricingId == topsal).SkuName,
                                         OilTypeId = dptgrp.First(y => y.PricingId == topsal).OilTypeId,
                                         DistributionChannelId = dptgrp.Key.DistributionChannelId,
                                         DivisionId = dptgrp.Key.DivisionId,
                                         SalesOrganizationId = dptgrp.Key.SalesOrganizationId,
                                         OilPackGroupTypeId = dptgrp.First(y => y.PricingId == topsal).OilPackGroupTypeId
                                     };
                outputDto = RecentPricings.ToList();


                if (outputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                var finalOutputDto = new List<FinalPriceSkuOutputDto>();

                var SkuDistinct = from a in outputDto.ToList()
                                  group a by new { a.SkuId, a.PlantId } into grp
                                  let topsku = grp.Max(X => X.PricingId)
                                  select new FinalPriceSkuOutputDto
                                  {
                                      SkuId = grp.Key.SkuId,
                                      PlantId = grp.Key.PlantId,
                                  };


                foreach (var item in SkuDistinct.ToList())
                {
                    var RecentPricingContext = (from a in outputDto.ToList()
                                                where a.SkuId == item.SkuId && a.PlantId == item.PlantId
                                                select a).ToList();

                    if (RecentPricingContext != null && RecentPricingContext.Any())
                    {
                        if (RecentPricingContext.Count > 1)
                        {
                            finalOutputDto.AddRange(RecentPricingContext.Where(_ => _.SkuId == item.SkuId && _.PlantId == item.PlantId).ToList());
                        }
                        else
                        {
                            finalOutputDto.AddRange(RecentPricingContext.Where(_ => _.SkuId == item.SkuId && _.PlantId == item.PlantId).ToList());
                        }
                    }
                }

                if (finalOutputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                #region Get Common Data's

                var skuIds = finalOutputDto.Select(s => s.SkuId).Distinct().ToList();
                //var discountGeographyDatas = _emamiContext.DiscountGeography.AsNoTracking()
                //    .Where(_ => saudaContext.BiddingDate >= _.ValidFrom
                //    && saudaContext.BiddingDate <= _.ValidTo
                //    && ((_.CityId == cityId || _.CityId == 0) && (_.DistrictId == userContext.DistrictId || _.DistrictId == 0) && (_.StateId == userContext.StateId || _.StateId == 0) && _.ZoneId == userContext.ZoneId)
                //    && skuIds.Contains(_.SkuId) && _.IsActive)
                //    .Select(s => new
                //    {
                //        Id = s.Id,
                //        CityId = s.CityId,
                //        ActualDiscount = s.ActualDiscount,
                //        SkuId = s.SkuId,
                //        OilTypeId = s.OilTypeId
                //    }).ToList();

                var discountGeographyDatas = (
                  from dg in _emamiContext.DiscountGeography.AsNoTracking()
                  join sku in _emamiContext.Skus.AsNoTracking()
                      on dg.SkuId equals sku.Id into skuGroup
                  from sku in skuGroup.DefaultIfEmpty()
                  where saudaContext.BiddingDate >= dg.ValidFrom
                      && saudaContext.BiddingDate <= dg.ValidTo
                      && ((dg.CityId == cityId || dg.CityId == 0)
                          && (dg.DistrictId == userContext.DistrictId || dg.DistrictId == 0)
                          && (dg.StateId == userContext.StateId || dg.StateId == 0)
                          && dg.ZoneId == userContext.ZoneId)
                      && skuIds.Contains(dg.SkuId) && dg.IsActive
                  select new
                  {
                      Id = dg.Id,
                      CityId = dg.CityId,
                      ActualDiscount = dg.ActualDiscount,
                      SkuId = dg.SkuId,
                      OilTypeId = dg.OilTypeId,
                      OilPackGroupTypeId = sku != null ? sku.OilPackGroupTypeId : null
                  }).ToList();

                var premiumGeographyDatas = _emamiContext.PremiumGeography.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(saudaContext.BiddingDate) >= DbFunctions.TruncateTime(_.ValidFrom)
                    && DbFunctions.TruncateTime(saudaContext.BiddingDate) <= DbFunctions.TruncateTime(_.ValidTo)
                    && _.CityId == cityId && skuIds.Contains(_.SkuId))
                    .Select(s => new
                    {
                        Id = s.Id,
                        CityId = s.CityId,
                        ActualPremium = s.ActualPremium,
                        SkuId = s.SkuId
                    }).ToList();

                var discountUserDatas = _emamiContext.DiscountUsers.AsNoTracking()
                    .Where(_ => _.ParentId != 0 && saudaContext.BiddingDate >= _.ValidFrom
                    && saudaContext.BiddingDate <= _.ValidTo
                    && _.UserId == inputDto.LoginUserId && skuIds.Contains(_.SkuId))
                    .Select(s => new
                    {
                        Id = s.Id,
                        UserId = s.UserId,
                        ActualDiscount = s.ActualDiscount,
                        SkuId = s.SkuId,
                        StateId = s.StateId
                    }).ToList();

                var premiumUserDatas = _emamiContext.PremiumUser.AsNoTracking()
                    .Where(_ => _.ParentId != 0 && DbFunctions.TruncateTime(saudaContext.BiddingDate) >= DbFunctions.TruncateTime(_.ValidFrom)
                    && DbFunctions.TruncateTime(saudaContext.BiddingDate) <= DbFunctions.TruncateTime(_.ValidTo)
                    && _.UserId == inputDto.LoginUserId && skuIds.Contains(_.SkuId))
                    .Select(s => new
                    {
                        Id = s.Id,
                        UserId = s.UserId,
                        ActualPremium = s.ActualPremium,
                        SkuId = s.SkuId
                    }).ToList();

                var skuUomMappingDatas = _emamiContext.SkuUomMapping.AsNoTracking()
                    .Where(_ => skuIds.Contains(_.SkuId))
                    .Select(s => new
                    {
                        SkuId = s.SkuId,
                        UomId = s.UomId,
                        RelationUomId = s.RelationUomId,
                        ConversionFactor1 = s.ConversionFactor1,
                        ConversionFactor2 = s.ConversionFactor2,
                    });

                var uomList = _emamiContext.Uom.AsNoTracking();

                var saudaDiscountLookup = _emamiContext.SaudaOrders
                .AsNoTracking()
                .Where(x => x.SaudaId == saudaContext.Id && x.PRAmount > 0)
                .Join(_emamiContext.Skus.AsNoTracking(),
                      so => so.SkuId,
                      s => s.Id,
                      (so, s) => new
                      {
                          so.SkuId,
                          s.OilTypeId,
                          s.OilPackGroupTypeId,
                          Discount = so.QuotedPriceBeforeSAPDiscount - so.PRAmount
                      })
                .GroupBy(x => new { x.OilTypeId, x.OilPackGroupTypeId })
                .Select(g => g
                    .OrderByDescending(x => x.Discount)
                    .FirstOrDefault())
                .ToDictionary(
                    x => (OilTypeId: x.OilTypeId, OilPackGroupTypeId: x.OilPackGroupTypeId),
                    x => new
                    {
                        x.SkuId,
                        x.Discount
                    }
                );

                #endregion

                foreach (var pricing in finalOutputDto)
                {
                    pricing.SkuName = skuDatas.FirstOrDefault(x => x.Id == pricing.SkuId) != null ? skuDatas.FirstOrDefault(x => x.Id == pricing.SkuId).Name : string.Empty;
                    var skuId = pricing.SkuId;
                    var oilTypeId = pricing.OilTypeId;
                    var uomId = 0L;

                    var discount = (decimal)0;
                    var premium = (decimal)0;


                    var skuContext = skuDatas.FirstOrDefault(_ => _.Id == skuId);
                    if (skuContext == null)
                    {
                        return _resultService.ErrorMessage(Constants.RecordNotFound);
                    }
                    var skuUomdata = skuUomMappingDatas.FirstOrDefault(_ => _.SkuId == skuId);
                    if (skuUomdata != null)
                    {
                        uomId = skuUomdata.UomId;
                        pricing.UOMId = uomId;
                        pricing.UOM = uomList.FirstOrDefault(_ => _.Id == uomId).SAPName;
                        pricing.CaseToMetricTonValue = _resultService.ConvertCasetoMetricTon(1, skuId);
                    }

                    if (premiumGeographyDatas != null && premiumGeographyDatas.Any())
                    {
                        var premiumGeographySkuContext = premiumGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.CityId == cityId && _.SkuId == skuId);
                        if (premiumGeographySkuContext != null)
                        {
                            var geoGraphyPremium = premiumGeographySkuContext.ActualPremium;
                            premium = premium + geoGraphyPremium;
                        }
                    }

                    if (discountUserDatas != null && discountUserDatas.Any())
                    {
                        if (userrole == (int)DTO.Enums.Role.ZonalTrader || userrole == (int)DTO.Enums.Role.StateTrader)
                        {
                            var discountLoginUserContext = discountUserDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.SkuId == skuId && _.StateId == userContext.StateId);
                            if (discountLoginUserContext != null && discountLoginUserContext.ActualDiscount > 0)
                            {
                                pricing.EmployeeSkuDiscount = discountLoginUserContext.ActualDiscount;
                            }
                            else
                            {
                                //if (discountGeographyDatas != null && discountGeographyDatas.Any())
                                //{
                                //    var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.SkuId == skuId);

                                //    if (discountGeographySkuContext == null)
                                //    {
                                //        discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.OilTypeId == oilTypeId);
                                //    }

                                //    if (discountGeographySkuContext != null)
                                //    {
                                //        if (pricing.OilPackGroupTypeId != null)
                                //        {
                                //            if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                //            {
                                //                pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                //            }
                                //            else if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                                //            {
                                //                pricing.EmployeeSkuDiscount = _resultService.CalculateAutomatedDiscount(discountGeographySkuContext.ActualDiscount, discountGeographySkuContext.SkuId, pricing.SkuId);
                                //            }
                                //        }
                                //    }
                                //}
                                if (discountGeographyDatas != null && discountGeographyDatas.Any())
                                {
                                    var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id)
                                        .FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.SkuId == skuId);

                                    if (discountGeographySkuContext != null)
                                    {
                                        pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                        pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                    }
                                    else
                                    {
                                        discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id)
                                            .FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.OilTypeId == oilTypeId && _.OilPackGroupTypeId ==
                                pricing.OilPackGroupTypeId);

                                        if (discountGeographySkuContext != null && pricing.OilPackGroupTypeId != null)
                                        {
                                            if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                            {
                                                pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                                pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                            }
                                            else if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                                            {
                                                pricing.EmployeeSkuDiscount = _resultService.CalculateAutomatedDiscount(
                                                    discountGeographySkuContext.ActualDiscount,
                                                    discountGeographySkuContext.SkuId,
                                                    pricing.SkuId);
                                                pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var discountLoginUserContext = discountUserDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.SkuId == skuId);
                            if (discountLoginUserContext != null && discountLoginUserContext.ActualDiscount > 0)
                            {
                                pricing.EmployeeSkuDiscount = discountLoginUserContext.ActualDiscount;
                            }
                            else
                            {
                                //if (discountGeographyDatas != null && discountGeographyDatas.Any())
                                //{
                                //    var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.SkuId == skuId);

                                //    if (discountGeographySkuContext == null)
                                //    {
                                //        discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.OilTypeId == oilTypeId);
                                //    }

                                //    if (discountGeographySkuContext != null)
                                //    {
                                //        if (pricing.OilPackGroupTypeId != null)
                                //        {
                                //            if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                //            {
                                //                pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                //            }
                                //            else if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                                //            {
                                //                pricing.EmployeeSkuDiscount = _resultService.CalculateAutomatedDiscount(discountGeographySkuContext.ActualDiscount, discountGeographySkuContext.SkuId, pricing.SkuId);
                                //            }
                                //        }
                                //    }
                                //}

                                if (discountGeographyDatas != null && discountGeographyDatas.Any())
                                {
                                    var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id)
                                        .FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.SkuId == skuId);

                                    if (discountGeographySkuContext != null)
                                    {
                                        pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                        pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                    }
                                    else
                                    {
                                        discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id)
                                            .FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.OilTypeId == oilTypeId && _.OilPackGroupTypeId ==
                                pricing.OilPackGroupTypeId);

                                        if (discountGeographySkuContext != null && pricing.OilPackGroupTypeId != null)
                                        {
                                            if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                            {
                                                pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                                pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                            }
                                            else if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                                            {
                                                pricing.EmployeeSkuDiscount = _resultService.CalculateAutomatedDiscount(
                                                    discountGeographySkuContext.ActualDiscount,
                                                    discountGeographySkuContext.SkuId,
                                                    pricing.SkuId);
                                                pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        //if (discountGeographyDatas != null && discountGeographyDatas.Any())
                        //{
                        //    var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.SkuId == skuId);

                        //    if (discountGeographySkuContext == null)
                        //    {
                        //        discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.OilTypeId == oilTypeId);
                        //    }

                        //    if (discountGeographySkuContext != null)
                        //    {
                        //        if (pricing.OilPackGroupTypeId != null)
                        //        {
                        //            if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                        //            {
                        //                pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                        //            }
                        //            else if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                        //            {
                        //                pricing.EmployeeSkuDiscount = _resultService.CalculateAutomatedDiscount(discountGeographySkuContext.ActualDiscount, discountGeographySkuContext.SkuId, pricing.SkuId);
                        //            }
                        //        }
                        //    }
                        //}
                        if (discountGeographyDatas != null && discountGeographyDatas.Any())
                        {
                            var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id)
                                .FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.SkuId == skuId);

                            if (discountGeographySkuContext != null)
                            {
                                pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                            }
                            else
                            {
                                discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id)
                                    .FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.OilTypeId == oilTypeId && _.OilPackGroupTypeId ==
                        pricing.OilPackGroupTypeId);

                                if (discountGeographySkuContext != null && pricing.OilPackGroupTypeId != null)
                                {
                                    if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                    {
                                        pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                        pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                    }
                                    else if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                                    {
                                        pricing.EmployeeSkuDiscount = _resultService.CalculateAutomatedDiscount(
                                            discountGeographySkuContext.ActualDiscount,
                                            discountGeographySkuContext.SkuId,
                                            pricing.SkuId);
                                        pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                    }
                                }
                            }
                        }
                    }

                    if(pricing.EmployeeSkuDiscount == 0)
                    {
                        var key = (pricing.OilTypeId, pricing.OilPackGroupTypeId);

                        if (saudaDiscountLookup.TryGetValue(key, out var discountData))
                        {
                            if (pricing.OilPackGroupTypeId != null)
                            {
                                if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                {
                                    pricing.EmployeeSkuDiscount = discountData.Discount;
                                }
                                else if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                                {
                                    pricing.EmployeeSkuDiscount = _resultService.CalculateAutomatedDiscount(discountData.Discount, discountData.SkuId, pricing.SkuId);
                                }
                            }
                        }
                    }

                    if (premiumUserDatas != null && premiumUserDatas.Any())
                    {
                        var premiumLoginUserContext = premiumUserDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.SkuId == skuId);
                        if (premiumLoginUserContext != null)
                        {
                            pricing.EmployeeSkuPremium = premiumLoginUserContext.ActualPremium;
                        }
                    }
                }

                if (LineIds != null && LineIds.Any())
                {
                    List<long> mappingSkuIds = new List<long>();

                    foreach (var id in LineIds.Distinct())
                    {
                        if (_emamiContext.Skus.Where(_ => _.LineId != null && _.LineId != string.Empty).Count() > 0)
                        {
                            var skuContextList = _emamiContext.Skus.Where(_ => _.LineId != null && _.LineId != string.Empty).ToList();
                            var skuIdList = skuContextList.Where(_ => _.LineId.Split(',').ToList().Contains(id)).Select(_ => _.Id).ToList();
                            mappingSkuIds.AddRange(skuIdList);
                        }
                    }

                    if (mappingSkuIds != null && mappingSkuIds.Any())
                    {
                        finalOutputDto = finalOutputDto.Where(_ => mappingSkuIds.Distinct().Contains(_.SkuId)).ToList();
                    }
                }
                return _resultService.SuccessObject(finalOutputDto);
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

        public ResultDto SaveSaudaModification(SaudaModificationInputDTO inputDto)
        {
            _methodName = "SaveSaudaModification";
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputDto)}");
            var resultDto = new ResultDto();
            try
            {

                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.DealerId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DealerMissing);
                }
                var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
                if (dealerContext == null)
                {
                    return _resultService.ErrorMessage(Constants.DealerNotFound);
                }

                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var saudaContext = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.SaudaNumber == inputDto.SaudaNumber);
                if (saudaContext == null)
                {
                    return _resultService.ErrorMessage(Constants.SaudaNotFound);
                }

                var pendingContractContext = _emamiContext.PendingContracts.AsNoTracking().Where(_ => _.SaudaNumber == inputDto.SaudaNumber);
                if (pendingContractContext == null)
                {
                    return _resultService.ErrorMessage(Constants.SaudaMissing);
                }

                // Check if there is already a pending modification for this SaudaNumber
                //var existingPendingModification = _emamiContext.SaudaModifications.AsNoTracking()
                //    .Any(_ => _.SaudaNumber == inputDto.SaudaNumber
                //              && _.StatusId == (int)DTO.Enums.Status.Pending);
                //if (existingPendingModification)
                //{
                //    return _resultService.ErrorMessage(Constants.SaudaModificationPendingExists);
                //}

                var existingPendingModificationId = _emamiContext.SaudaModifications
                .AsNoTracking()
                .Where(x => x.SaudaNumber == inputDto.SaudaNumber
                         && x.StatusId == (int)DTO.Enums.Status.Pending)
                .Select(x => x.Id)
                .FirstOrDefault();

                if (existingPendingModificationId > 0)
                {
                    return _resultService.ErrorMessage(
                        Constants.SaudaModificationPendingExists + existingPendingModificationId.ToString()
                    );
                }

                var SkuContext = _emamiContext.Skus.AsNoTracking().ToList();
                var oilTypesContext = _emamiContext.OilTypes.AsNoTracking().ToList();

                var pendingMaterials = pendingContractContext
                .Select(x => new PendingContractMaterialInfoDTO
                {
                    MaterialCode = x.MaterialCode,
                    SalesOrgId = x.SalesOrgId,
                    DistChnlId = x.DistChnlId,
                    DivisionId = x.DivisionId,
                    BasicRate = x.BasicRate,
                    PendingQuantityInCase = x.PendingQuantityInCase,
                    SaudaQuantity = x.SaudaQuantity
                })
                .Distinct()
                .ToList();

                var skuDetails = (from p in pendingMaterials
                                  join sku in SkuContext on new
                                  {
                                      MaterialCode = p.MaterialCode,
                                      SalesOrganizationId = p.SalesOrgId,
                                      DistributionChannelId = p.DistChnlId,
                                      DivisionId = p.DivisionId
                                  }
                                  equals new
                                  {
                                      MaterialCode = sku.SkuCode,
                                      SalesOrganizationId = sku.SalesOrganizationId,
                                      DistributionChannelId = sku.DistributionChannelId,
                                      DivisionId = sku.DivisionId
                                  }
                                  select new SaudaModificaitonFromSkuInfoDTO
                                  {
                                      Id = sku.Id,
                                      SkuCode = sku.SkuCode,
                                      SkuName = sku.SkuName,
                                      OilTypeId = sku.OilTypeId,
                                      BasicRate = p.BasicRate,
                                      OilPackGroupTypeId = sku.OilPackGroupTypeId,
                                      PendingQuantityInCase = p.PendingQuantityInCase,
                                      SaudaQuantity = p.SaudaQuantity
                                  }).ToList();

                var oldItems = new PendingContractDetails
                {
                    OilTypes = skuDetails
                    .GroupBy(s => s.OilTypeId)
                    .Select(oilGroup => new PendingContractOilTypeDetails
                    {
                        OilTypeId = oilGroup.Key,
                        OilTypeName = oilTypesContext
                            .FirstOrDefault(o => o.Id == oilGroup.Key)?.Name ?? "Unknown",

                        PackTypes = oilGroup
                            .GroupBy(p => p.OilPackGroupTypeId)
                            .Select(packGroup => new PendingContractPackTypeDetails
                            {
                                PackTypeId = packGroup.Key,
                                PackTypeName = (packGroup.Key == null ? "Unknown" : (packGroup.Key == (int)DTO.Enums.BpCpType.BP ? "BP" : (packGroup.Key == (int)DTO.Enums.BpCpType.CP ? "CP" : "Unknown"))),

                                OriginalMT = packGroup.Sum(x => x.SaudaQuantity),
                                ModifiedMT = packGroup.Sum(x => x.SaudaQuantity),
                                DifferenceMT = packGroup.Sum(x => x.SaudaQuantity) - packGroup.Sum(x => x.SaudaQuantity),

                                Skus = packGroup.Select(s => new PendingContractSkuDetails
                                {
                                    SkuId = s.Id,
                                    SkuName = s.SkuName,
                                    SkuCode = s.SkuCode,
                                    PendingQuantityInCase = s.PendingQuantityInCase,
                                    SaudaQuantity = s.SaudaQuantity,
                                    BasicRate = s.BasicRate,
                                    CaseToMetricTonValue = s.CaseToMetricTonValue
                                }).ToList()
                            })
                            .ToList()
                    })
                    .ToList()
                };

                foreach (var oldOilType in oldItems.OilTypes)
                {
                    foreach (var oldPack in oldOilType.PackTypes)
                    {
                        var newOilType = inputDto.OilTypes
                            .FirstOrDefault(x => x.OilTypeId == oldOilType.OilTypeId);

                        if (newOilType == null ||
                            !newOilType.PackTypes.Any(p => p.PackTypeId == oldPack.PackTypeId))
                        {
                            return _resultService.ErrorMessage(Constants.SaudaModificationQuantityMismatch);
                        }
                    }
                }

                bool isAnyModificationDone = false;

                if (inputDto.OilTypes
                .GroupBy(x => x.OilTypeId)
                .Any(g => g.Key == null || g.Count() > 1))
                {
                    return _resultService.ErrorMessage(Constants.SaudaModificationDuplicateOrInvalidOilTypeFound);
                }


                foreach (var newOilType in inputDto.OilTypes)
                {

                    if(newOilType.OilTypeId == 0 || !oilTypesContext.Where(x => x.Id == newOilType.OilTypeId).Any())
                    {
                        return _resultService.ErrorMessage(Constants.OilTypeEmpty);
                    }

                    if (newOilType.PackTypes
                    .GroupBy(p => p.PackTypeId)
                    .Any(g => g.Key == null || g.Count() > 1))
                    {
                        return _resultService.ErrorMessage(Constants.SaudaModificationDuplicateOrInvalidPackTypeFound);
                    }

                    foreach (var newOilTypePack in newOilType.PackTypes)
                    {
                        if(newOilTypePack.PackTypeId == 0)
                        {
                            return _resultService.ErrorMessage(Constants.PackTypeMissing);
                        }

                        var originalOilTypeInfo = oldItems.OilTypes.Where(x => x.OilTypeId == newOilType.OilTypeId).FirstOrDefault();
                        if (originalOilTypeInfo == null)
                        {
                            return _resultService.ErrorMessage(Constants.SaudaModificationQuantityMismatch);
                        }
                        var originalOilTypePackInfo = originalOilTypeInfo.PackTypes.Where(x => x.PackTypeId == newOilTypePack.PackTypeId).FirstOrDefault();
                        if (originalOilTypePackInfo == null)
                        {
                            return _resultService.ErrorMessage(Constants.SaudaModificationQuantityMismatch);
                        }

                        foreach (var sku in newOilTypePack.Skus)
                        {
                            var originalSku = originalOilTypePackInfo.Skus.Where(x => x.SkuId == sku.SkuId).FirstOrDefault();

                            if(originalSku == null || originalSku.SaudaQuantity != sku.SaudaQuantity)
                            {
                                isAnyModificationDone = true;
                                break;
                            }
                        }

                        var originalSkuMT = originalOilTypePackInfo.Skus.Sum(x => x.SaudaQuantity);
                        var newSkuMT = newOilTypePack.Skus.Sum(x => x.SaudaQuantity);

                        decimal toleranceInMT = GetSaudaModificationToleranceInMT();

                        if (Math.Abs(originalSkuMT - newSkuMT) > toleranceInMT)
                        {
                            return _resultService.ErrorMessage(Constants.SaudaModificationQuantityMismatch);
                        }
                    }

                }

                if(!isAnyModificationDone)
                {
                    return _resultService.ErrorMessage(Constants.SaudaModificationNoModification);
                }

                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                long createdSaudaModificationId = 0;

                using (var dbContextTransaction = _emamiContext.Database.BeginTransaction())
                {
                    try
                    {

                        var saudaModification = new SaudaModification
                        {
                            SaudaNumber = inputDto.SaudaNumber,
                            StatusId = (int)DTO.Enums.Status.Pending,
                            IsSentToSAP = false,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = currentDate
                        };
                        _emamiContext.SaudaModifications.Add(saudaModification);
                        _emamiContext.SaveChanges();



                        foreach (var newOilType in inputDto.OilTypes)
                        {

                            foreach (var newOilTypePack in newOilType.PackTypes)
                            {

                                var originalSkuItems = oldItems.OilTypes.Where(x => x.OilTypeId == newOilType.OilTypeId).FirstOrDefault()
                                    .PackTypes.Where(x => x.PackTypeId == newOilTypePack.PackTypeId).FirstOrDefault().Skus.ToList();

                                var saudaModificationLine = new SaudaModificationLine
                                {
                                    SaudaModificationId = saudaModification.Id,
                                    OilTypeId = newOilType.OilTypeId.GetValueOrDefault(),
                                    OilPackGroupTypeId = newOilTypePack.PackTypeId.GetValueOrDefault(),
                                    TotalOriginalPendingQty = originalSkuItems.Sum(x => x.SaudaQuantity),
                                    TotalModifiedQty = newOilTypePack.Skus.Sum(x => x.SaudaQuantity),
                                    CreatedBy = inputDto.LoginUserId,
                                    CreatedDate = currentDate
                                };

                                _emamiContext.SaudaModificationLines.Add(saudaModificationLine);
                                _emamiContext.SaveChanges();

                                foreach(var originalItem in originalSkuItems)
                                {
                                    var saudaModificationOldItem = new SaudaModificationOldItem
                                    {
                                        SaudaModificationLineId = saudaModificationLine.Id,
                                        skuId = originalItem.SkuId,
                                        QuantityInCase = originalItem.PendingQuantityInCase,
                                        SaudaQuantity = originalItem.SaudaQuantity,
                                        CreatedBy = inputDto.LoginUserId,
                                        CreatedDate = currentDate
                                    };
                                    _emamiContext.SaudaModificationOldItems.Add(saudaModificationOldItem);
                                }
                                _emamiContext.SaveChanges();

                                foreach (var newItem in newOilTypePack.Skus)
                                {
                                    var saudaModificationItems = new SaudaModificationItem
                                    {
                                        SaudaModificationLineId = saudaModificationLine.Id,
                                        skuId = newItem.SkuId,
                                        QuantityInCase = newItem.PendingQuantityInCase,
                                        SaudaQuantity = newItem.SaudaQuantity,
                                        CreatedBy = inputDto.LoginUserId,
                                        CreatedDate = currentDate,
                                        Price = newItem.Price,
                                        Discount = newItem.EmployeeSkuDiscount
                                    };
                                    _emamiContext.SaudaModificationItems.Add(saudaModificationItems);
                                }
                                _emamiContext.SaveChanges();

                            }

                        }

                        var saudaModificationApprovalContext = new SaudaModificationApproval
                        {
                            RequestedBy = inputDto.LoginUserId,
                            RequestedTo = _emamiContext.UserReportingToMappings.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId).ReportingToUserId,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = currentDate,
                            StatusId = (int)DTO.Enums.Status.Pending,
                            SaudaModificationId = saudaModification.Id
                        };
                        _emamiContext.SaudaModificationApprovals.Add(saudaModificationApprovalContext);
                        _emamiContext.SaveChanges();

                        createdSaudaModificationId = saudaModification.Id;

                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        var message = $"{ServiceName} Service-Method {_methodName} Exception: {ex}";
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.ErrorCode = Constants.Exception;
                        resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                        _logger.Error(message);
                        return resultDto;
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Message = Constants.SaudaModificationBookedSuccessfully+createdSaudaModificationId.ToString();
                return resultDto;
            }
            catch(Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetSaudaModificationPendingApprovedList(SaudaReportFilterDto inputDto)
        {
            _methodName = "GetSaudaModificationPendingApprovedList";
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.LoginUserId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }

                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }

                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var roleIds = _emamiContext.UserRoles.AsNoTracking()
                    .Where(_ => _.UserId == userContext.Id)
                    .Select(_ => _.RoleId)
                    .ToList();

                var dealerIds = new List<long>();

                // Dealer / Distributor
                if (roleIds.Contains((int)DTO.Enums.Role.Dealer))
                {
                    dealerIds.Add(userContext.Id);
                }

                // State Head (StateTrader) - dealers mapped directly
                if (roleIds.Contains((int)DTO.Enums.Role.StateTrader))
                {
                    var stateTraderDealers = _emamiContext.UserCustomerMapping.AsNoTracking()
                        .Where(_ => _.UserId == userContext.Id)
                        .Select(_ => _.CustomerId)
                        .ToList();
                    dealerIds.AddRange(stateTraderDealers);
                }

                // Zonal Head (ZonalTrader) - BDOs -> dealers
                if (roleIds.Contains((int)DTO.Enums.Role.ZonalTrader))
                {
                    var bdoIds = _emamiContext.Users.AsNoTracking()
                        .Where(_ => _.ReportingToId == userContext.Id)
                        .Select(_ => _.Id)
                        .ToList();

                    if (bdoIds != null && bdoIds.Any())
                    {
                        var zonalDealers = _emamiContext.UserCustomerMapping.AsNoTracking()
                            .Where(_ => bdoIds.Contains(_.UserId))
                            .Select(_ => _.CustomerId)
                            .ToList();
                        dealerIds.AddRange(zonalDealers);
                    }
                }

                // National Head (NationalTrader) - ZH -> BDO -> dealers
                if (roleIds.Contains((int)DTO.Enums.Role.NationalTrader))
                {
                    var zonalHeadIds = _emamiContext.Users.AsNoTracking()
                        .Where(_ => _.ReportingToId == userContext.Id)
                        .Select(_ => _.Id)
                        .ToList();

                    if (zonalHeadIds != null && zonalHeadIds.Any())
                    {
                        var bdoIds = _emamiContext.Users.AsNoTracking()
                            .Where(_ => zonalHeadIds.Contains(_.ReportingToId ?? 0))
                            .Select(_ => _.Id)
                            .ToList();

                        if (bdoIds != null && bdoIds.Any())
                        {
                            var nationalDealers = _emamiContext.UserCustomerMapping.AsNoTracking()
                                .Where(_ => bdoIds.Contains(_.UserId))
                                .Select(_ => _.CustomerId)
                                .ToList();
                            dealerIds.AddRange(nationalDealers);
                        }
                    }
                }

                dealerIds = dealerIds.Distinct().ToList();

                if (dealerIds == null || !dealerIds.Any())
                {
                    return _resultService.ErrorMessage(Constants.DealerNotFound);
                }

                var saudaContext = _emamiContext.Sauda.AsNoTracking()
                    .Where(_ => dealerIds.Contains(_.UserId))
                    .Select(_ => new { _.UserId, _.SaudaNumber })
                    .ToList();

                if (saudaContext == null || !saudaContext.Any())
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                var saudaNumbers = saudaContext.Select(_ => _.SaudaNumber).Distinct().ToList();

                var saudaModifications = _emamiContext.SaudaModifications.AsNoTracking()
                    .Where(_ => saudaNumbers.Contains(_.SaudaNumber)
                                && _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected
                                && DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                                && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                    .ToList();

                if (saudaModifications == null || !saudaModifications.Any())
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                //var approvalStatuses = _emamiContext.ApprovalStatus.AsNoTracking().ToList();
                //var users = _emamiContext.Users.AsNoTracking().ToList();

                //var pendingList = saudaModifications
                //    .Where(_ => _.StatusId == (int)DTO.Enums.Status.Pending)
                //    .Join(saudaContext, sm => sm.SaudaNumber, s => s.SaudaNumber, (sm, s) => new { sm, s })
                //    .Select(_ => new SaudaModificationListItemDto
                //    {
                //        Id = _.sm.Id,
                //        DealerName = users.FirstOrDefault(u => u.Id == _.s.UserId)?.Name ?? string.Empty,
                //        CreatedByName = users.FirstOrDefault(u => u.Id == _.sm.CreatedBy)?.Name ?? string.Empty,
                //        ModificationDate = _.sm.CreatedDate,
                //        SaudaNumber = _.sm.SaudaNumber,
                //        Status = approvalStatuses.FirstOrDefault(st => st.Id == _.sm.StatusId)?.Name ?? string.Empty
                //    })
                //    .OrderByDescending(_ => _.ModificationDate)
                //    .ToList();

                //var approvedList = saudaModifications
                //    .Where(_ => _.StatusId == (int)DTO.Enums.Status.Approved)
                //    .Join(saudaContext, sm => sm.SaudaNumber, s => s.SaudaNumber, (sm, s) => new { sm, s })
                //    .Select(_ => new SaudaModificationListItemDto
                //    {
                //        Id = _.sm.Id,
                //        DealerName = users.FirstOrDefault(u => u.Id == _.s.UserId)?.Name ?? string.Empty,
                //        CreatedByName = users.FirstOrDefault(u => u.Id == _.sm.CreatedBy)?.Name ?? string.Empty,
                //        ModificationDate = _.sm.CreatedDate,
                //        SaudaNumber = _.sm.SaudaNumber,
                //        Status = approvalStatuses.FirstOrDefault(st => st.Id == _.sm.StatusId)?.Name ?? string.Empty
                //    })
                //    .OrderByDescending(_ => _.ModificationDate)
                //    .ToList();

                var saudaModificationIds = saudaModifications
                .Select(sm => sm.Id)
                .Distinct()
                .ToList();

                var latestApprovals = _emamiContext.SaudaModificationApprovals
                .AsNoTracking()
                .Where(a => saudaModificationIds.Contains(a.SaudaModificationId))
                .GroupBy(a => a.SaudaModificationId)
                .Select(g => g
                    .OrderByDescending(x => x.CreatedDate) // or CreatedId if that's the correct column
                    .Select(x => new
                    {
                        x.SaudaModificationId,
                        x.RequestedTo
                    })
                    .FirstOrDefault()
                )
                .ToList();

                var approvalLookup = latestApprovals
                .Where(a => a != null)
                .ToDictionary(a => a.SaudaModificationId, a => a.RequestedTo);

                var approvalStatuses = _emamiContext.ApprovalStatus.AsNoTracking().ToList();
                var users = _emamiContext.Users.AsNoTracking().ToList();

                var flatList =
                saudaModifications
                .Join(
                    saudaContext,
                    sm => sm.SaudaNumber,
                    s => s.SaudaNumber,
                    (sm, s) => new
                    {
                        DealerId = s.UserId,
                        Item = new SaudaModificationListItemDto
                        {
                            Id = sm.Id,
                            DealerId = s.UserId, // add if missing
                            DealerName = users.FirstOrDefault(u => u.Id == s.UserId)?.Name ?? string.Empty,
                            CreatedByName = users.FirstOrDefault(u => u.Id == sm.CreatedBy)?.Name ?? string.Empty,
                            ModificationDate = sm.CreatedDate,
                            SaudaNumber = sm.SaudaNumber,
                            StatusId = sm.StatusId,
                            Status = approvalStatuses.FirstOrDefault(st => st.Id == sm.StatusId)?.Name ?? string.Empty,
                            ApprovalRejectedByName =
                            sm.StatusId == (int)DTO.Enums.Status.Approved
                            ? string.Empty
                            : approvalLookup.TryGetValue(sm.Id, out var approverUserId)
                                ? users.FirstOrDefault(u => u.Id == approverUserId)?.Name ?? string.Empty
                                : string.Empty
                        }
                    }
                )
                .ToList();

                var pendingGrouped =
                flatList
                .Where(x => x.Item.StatusId == (int)DTO.Enums.Status.Pending)
                .GroupBy(x => x.DealerId)
                .Select(g => new DealerGroupedSaudaModificationDto
                {
                    DealerId = g.Key,
                    DealerName = g.First().Item.DealerName,
                    Items = g
                        .Select(x => x.Item)
                        .OrderByDescending(x => x.ModificationDate)
                        .ToList()
                })
                .OrderBy(x => x.DealerName)
                .ToList();

                var approvedGrouped =
                flatList
                .Where(x => x.Item.StatusId == (int)DTO.Enums.Status.Approved)
                .GroupBy(x => x.DealerId)
                .Select(g => new DealerGroupedSaudaModificationDto
                {
                    DealerId = g.Key,
                    DealerName = g.First().Item.DealerName,
                    Items = g
                        .Select(x => x.Item)
                        .OrderByDescending(x => x.ModificationDate)
                        .ToList()
                })
                .OrderBy(x => x.DealerName)
                .ToList();

                var result = new SaudaModificationStatusListDto
                {
                    PendingList = pendingGrouped,
                    ApprovedList = approvedGrouped
                };

                return _resultService.SuccessObject(result);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetSaudaModificationDetails(IdInputDto inputDto)
        {
            _methodName = "GetSaudaModificationDetails";
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.Id <= 0)
                {
                    return _resultService.ErrorMessage(Constants.IdEmpty);
                }

                var modification = _emamiContext.SaudaModifications.AsNoTracking()
                    .FirstOrDefault(_ => _.Id == inputDto.Id);

                if (modification == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                var statusName = _emamiContext.ApprovalStatus.AsNoTracking()
                    .FirstOrDefault(_ => _.Id == modification.StatusId)?.Name ?? string.Empty;

                var createdByName = _emamiContext.Users.AsNoTracking()
                    .FirstOrDefault(_ => _.Id == modification.CreatedBy)?.Name ?? string.Empty;

                var lines = _emamiContext.SaudaModificationLines.AsNoTracking()
                    .Where(_ => _.SaudaModificationId == modification.Id)
                    .ToList();

                if (lines == null || !lines.Any())
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                var lineIds = lines.Select(_ => _.Id).ToList();

                var newItems = _emamiContext.SaudaModificationItems.AsNoTracking()
                    .Where(_ => lineIds.Contains(_.SaudaModificationLineId))
                    .ToList();

                var oldItems = _emamiContext.SaudaModificationOldItems.AsNoTracking()
                    .Where(_ => lineIds.Contains(_.SaudaModificationLineId))
                    .ToList();

                var oilTypeIds = lines.Select(_ => _.OilTypeId).Distinct().ToList();
                var oilTypes = _emamiContext.OilTypes.AsNoTracking()
                    .Where(_ => oilTypeIds.Contains(_.Id))
                    .Select(_ => new { _.Id, _.Name })
                    .ToList();

                var skuIds = newItems.Select(_ => _.skuId)
                    .Concat(oldItems.Select(_ => _.skuId))
                    .Distinct()
                    .ToList();

                var skuLookup = _emamiContext.Skus.AsNoTracking()
                    .Where(_ => skuIds.Contains(_.Id))
                    .Select(_ => new { _.Id, _.SkuName })
                    .ToList();

                var result = new SaudaModificationDetailsDto
                {
                    Id = modification.Id,
                    SaudaNumber = modification.SaudaNumber,
                    StatusId = modification.StatusId,
                    Status = statusName,
                    CreatedDate = modification.CreatedDate,
                    CreatedByName = createdByName
                };

                foreach (var line in lines)
                {
                    var lineDto = new SaudaModificationDetailLineDto
                    {
                        Id = line.Id,
                        OilTypeId = line.OilTypeId,
                        OilTypeName = oilTypes.FirstOrDefault(_ => _.Id == line.OilTypeId)?.Name ?? string.Empty,
                        OilPackGroupTypeId = line.OilPackGroupTypeId,
                        OilPackGroupTypeName = Enum.IsDefined(typeof(DTO.Enums.BpCpType), (int)line.OilPackGroupTypeId)
                            ? UtilityHelper.GetEnumDescription((DTO.Enums.BpCpType)line.OilPackGroupTypeId)
                            : string.Empty,
                        TotalOriginalPendingQty = line.TotalOriginalPendingQty,
                        TotalModifiedQty = line.TotalModifiedQty
                    };

                    lineDto.NewItems = newItems.Where(_ => _.SaudaModificationLineId == line.Id)
                        .Select(item => new SaudaModificationDetailItemDto
                        {
                            SkuId = item.skuId,
                            SkuName = skuLookup.FirstOrDefault(_ => _.Id == item.skuId)?.SkuName ?? string.Empty,
                            QuantityInCase = item.QuantityInCase,
                            SaudaQuantity = item.SaudaQuantity
                        }).ToList();

                    lineDto.OldItems = oldItems.Where(_ => _.SaudaModificationLineId == line.Id)
                        .Select(item => new SaudaModificationDetailItemDto
                        {
                            SkuId = item.skuId,
                            SkuName = skuLookup.FirstOrDefault(_ => _.Id == item.skuId)?.SkuName ?? string.Empty,
                            QuantityInCase = item.QuantityInCase,
                            SaudaQuantity = item.SaudaQuantity
                        }).ToList();

                    result.Lines.Add(lineDto);
                }

                return _resultService.SuccessObject(result);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetSaudaModificationApprovalList(SaudaListFilterDto inputDto)
        {
            _methodName = "GetSaudaModificationApprovalList";
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.LoginUserId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }

                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }

                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }

                var userContext = _emamiContext.Users.AsNoTracking()
                    .FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var approvalsQuery = from sma in _emamiContext.SaudaModificationApprovals.AsNoTracking()
                                     join sm in _emamiContext.SaudaModifications.AsNoTracking() on sma.SaudaModificationId equals sm.Id
                                     join s in _emamiContext.Sauda.AsNoTracking() on sm.SaudaNumber equals s.SaudaNumber
                                     join dealer in _emamiContext.Users.AsNoTracking() on s.UserId equals dealer.Id
                                     join createdBy in _emamiContext.Users.AsNoTracking() on sm.CreatedBy equals createdBy.Id into cb
                                     from createdBy in cb.DefaultIfEmpty()
                                     where sma.RequestedTo == inputDto.LoginUserId
                                           && DbFunctions.TruncateTime(sma.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                                           && DbFunctions.TruncateTime(sma.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                                           && sma.StatusId == (int)DTO.Enums.Status.Pending
                                     select new
                                     {
                                         Approval = sma,
                                         Modification = sm,
                                         Sauda = s,
                                         Dealer = dealer,
                                         CreatedBy = createdBy
                                     };


                if (inputDto.SalesOrganizationId > 0)
                {
                    approvalsQuery = approvalsQuery.Where(_ => _.Sauda.SalesOrganizationId == inputDto.SalesOrganizationId);
                }

                if (inputDto.DistributionChannelId > 0)
                {
                    approvalsQuery = approvalsQuery.Where(_ => _.Sauda.DistributionChannelId == inputDto.DistributionChannelId);
                }

                if (inputDto.DivisionId > 0)
                {
                    approvalsQuery = approvalsQuery.Where(_ => _.Sauda.DivisionId == inputDto.DivisionId);
                }

                var output = new SaudaModificationApprovalListDto();
                output.ListCount = approvalsQuery.Count();

                output.Items = approvalsQuery
                    .OrderByDescending(_ => _.Approval.CreatedDate)
                    .ToList()
                    .Select(_ => new SaudaModificationListItemDto
                    {
                        Id = _.Modification.Id,
                        DealerName = _.Dealer != null ? _.Dealer.Name : string.Empty,
                        CreatedByName = _.CreatedBy != null ? _.CreatedBy.Name : string.Empty,
                        ModificationDate = _.Modification.CreatedDate,
                        SaudaNumber = _.Modification.SaudaNumber,
                        Status = string.Empty,
                        BiddingDate = _.Sauda.BiddingDate
                    })
                    .ToList();

                return _resultService.SuccessObject(output);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto ChangeSaudaModificationStatus(SaudaModificationUpdateDto inputDto)
        {
            _methodName = "ChangeSaudaModificationStatus";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            try
            {

                if (inputDto == null || (inputDto.SaudaModificationIds == null || !inputDto.SaudaModificationIds.Any()) && inputDto.LoginUserId < 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var loginUserRole = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                //var saudaModificationsContext = _emamiContext.SaudaModifications.Where(_ => inputDto.SaudaModificationIds.Contains(_.Id)).ToList();

                var saudaModificationsWithSauda = (
                    from sm in _emamiContext.SaudaModifications
                    join s in _emamiContext.Sauda.AsNoTracking()
                        on sm.SaudaNumber equals s.SaudaNumber
                    where inputDto.SaudaModificationIds.Contains(sm.Id)
                    select new
                    {
                        SaudaModification = sm,
                        Sauda = s
                    }
                ).ToList();

                if (inputDto.SaudaModificationIds != null && inputDto.SaudaModificationIds.Any())
                {
                    int roleId = 0;

                    //if (loginUserRole.RoleId == (int)DTO.Enums.Role.NationalTrader)
                    //{
                    //    roleId = (int)DTO.Enums.Role.NationalTrader;
                    //}
                    //else
                    //{
                    //    roleId = (int)DTO.Enums.Role.ZonalTrader;
                    //}

                    roleId = (int)DTO.Enums.Role.NationalTrader;

                    if (inputDto.StatusId == (int)DTO.Enums.Status.Approved && loginUserRole != null && loginUserRole.RoleId == roleId)
                    {
                        saudaModificationsWithSauda.Select(x=> x.SaudaModification).ToList().ForEach(a =>
                        {
                            a.StatusId = inputDto.StatusId;
                            a.ModifiedBy = inputDto.ModifiedBy;
                            a.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        });
                        _emamiContext.SaveChanges();
                    }
                    else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                    {
                        saudaModificationsWithSauda.Select(x => x.SaudaModification).ToList().ForEach(a =>
                        {
                            a.StatusId = inputDto.StatusId;
                            a.ModifiedBy = inputDto.ModifiedBy;
                            a.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                            if (!string.IsNullOrEmpty(inputDto.Remarks))
                            {
                                var entity = new Remarks()
                                {
                                    TableId = a.Id,
                                    TableName = "SaudaModifications",
                                    ReasonTypeId = inputDto.StatusId,
                                    Description = inputDto.Remarks,
                                    CreatedBy = inputDto.ModifiedBy,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                    IsActive = true
                                };
                                _emamiContext.Remarks.Add(entity);
                            }
                        });
                        _emamiContext.SaveChanges();
                    }
                    else
                    {
                        saudaModificationsWithSauda.Select(x => x.SaudaModification).ToList().ForEach(a =>
                        {
                            a.StatusId = (int)DTO.Enums.Status.Pending;
                            a.ModifiedBy = inputDto.ModifiedBy;
                            a.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        });
                        _emamiContext.SaveChanges();
                    }

                    var saudaModificationApprovalContextlist = _emamiContext.SaudaModificationApprovals.Where(_ => inputDto.SaudaModificationIds.Contains(_.SaudaModificationId)).ToList();
                    saudaModificationApprovalContextlist.ForEach(a =>
                    {
                        a.StatusId = inputDto.StatusId;
                        //a.Remarks = inputDto.Remarks;
                        a.ModifiedBy = inputDto.ModifiedBy;
                        a.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    });
                    _emamiContext.SaveChanges();

                    if (inputDto.StatusId == (int)DTO.Enums.Status.Approved && loginUserRole != null && loginUserRole.RoleId != roleId)
                    {
                        //saudaModificationsContext.ForEach(a =>
                        //{

                        //    var saudaContext = _emamiContext.Sauda.AsNoTracking().Where(_ => _.SaudaNumber == a.SaudaNumber).FirstOrDefault();

                        //    var requestedToUser = (from uc in _emamiContext.UserReportingToMappings.AsNoTracking()
                        //                           join udiv in _emamiContext.UserDivisionMappings.AsNoTracking() on uc.UserId equals udiv.UserId
                        //                           where
                        //                           udiv.SalesOrganizationId == saudaContext.SalesOrganizationId
                        //                           && udiv.DistributionChannelId == saudaContext.DistributionChannelId
                        //                           && udiv.DivisionId == saudaContext.DivisionId
                        //                           && uc.UserId == inputDto.LoginUserId
                        //                           select uc.ReportingToUserId
                        //             ).FirstOrDefault();

                        //    //Sauda approval save
                        //    var saudaModificationApprovalContext = new SaudaModificationApproval
                        //    {
                        //        RequestedBy = inputDto.LoginUserId,
                        //        RequestedTo = requestedToUser > 0 ? requestedToUser : 0,
                        //        CreatedBy = inputDto.LoginUserId,
                        //        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        //        StatusId = (int)DTO.Enums.Status.Pending,
                        //        ApprovedBy = inputDto.LoginUserId,
                        //        SaudaModificationId = a.Id,
                        //        Remarks = inputDto.Remarks
                        //    };
                        //    _emamiContext.SaudaModificationApprovals.Add(saudaModificationApprovalContext);
                        //});
                        foreach (var item in saudaModificationsWithSauda)
                        {
                            var saudaModification = item.SaudaModification;
                            var saudaContext = item.Sauda; // never null (inner join)

                            var requestedToUser = (
                                from uc in _emamiContext.UserReportingToMappings.AsNoTracking()
                                join udiv in _emamiContext.UserDivisionMappings.AsNoTracking()
                                    on uc.UserId equals udiv.UserId
                                where
                                    udiv.SalesOrganizationId == saudaContext.SalesOrganizationId &&
                                    udiv.DistributionChannelId == saudaContext.DistributionChannelId &&
                                    udiv.DivisionId == saudaContext.DivisionId &&
                                    uc.UserId == inputDto.LoginUserId
                                select uc.ReportingToUserId
                            ).FirstOrDefault();

                            var saudaModificationApprovalContext = new SaudaModificationApproval
                            {
                                RequestedBy = inputDto.LoginUserId,
                                RequestedTo = requestedToUser > 0 ? requestedToUser : 0,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                StatusId = (int)DTO.Enums.Status.Pending,
                                ApprovedBy = inputDto.LoginUserId,
                                SaudaModificationId = saudaModification.Id,
                                Remarks = inputDto.Remarks
                            };

                            _emamiContext.SaudaModificationApprovals.Add(saudaModificationApprovalContext);
                        }
                        _emamiContext.SaveChanges();
                    }

                    if (inputDto.StatusId == (int)DTO.Enums.Status.Approved && loginUserRole != null && loginUserRole.RoleId == roleId)
                    {
                        //method to sync sauda approval from APP to SAP 
                        HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                        {
                            _sapIntegrationService.SendSaudaModificationInfoToSAP(inputDto.SaudaModificationIds, true);
                        });
                    }
                }
                else { return _resultService.ErrorMessage(Constants.RecordNotFound); }

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

        public ResultDto ChangeSaudaModificationStatusForLoose(SaudaModificationUpdateDto inputDto)
        {
            _methodName = "ChangeSaudaModificationStatusForLoose";
            _logger.Info($"SAP Service : {ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            try
            {

                if (inputDto == null || (inputDto.SaudaModificationIds == null || !inputDto.SaudaModificationIds.Any()) && inputDto.LoginUserId < 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var loginUserRole = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                //var saudaModificationsContext = _emamiContext.SaudaModifications.Where(_ => inputDto.SaudaModificationIds.Contains(_.Id)).ToList();

                var saudaModificationsWithSauda = (
                    from sm in _emamiContext.SaudaModifications
                    join s in _emamiContext.Sauda.AsNoTracking()
                        on sm.SaudaNumber equals s.SaudaNumber
                    where inputDto.SaudaModificationIds.Contains(sm.Id)
                    select new
                    {
                        SaudaModification = sm,
                        Sauda = s
                    }
                ).ToList();

                if (inputDto.SaudaModificationIds != null && inputDto.SaudaModificationIds.Any())
                {
                    int roleId = 0;

                    //if (loginUserRole.RoleId == (int)DTO.Enums.Role.NationalTrader)
                    //{
                    //    roleId = (int)DTO.Enums.Role.NationalTrader;
                    //}
                    //else
                    //{
                    //    roleId = (int)DTO.Enums.Role.ZonalTrader;
                    //}

                    roleId = (int)DTO.Enums.Role.NationalTrader;

                    if (inputDto.StatusId == (int)DTO.Enums.Status.Approved && loginUserRole != null && loginUserRole.RoleId == roleId)
                    {
                        saudaModificationsWithSauda.Select(x => x.SaudaModification).ToList().ForEach(a =>
                        {
                            a.StatusId = inputDto.StatusId;
                            a.ModifiedBy = inputDto.ModifiedBy;
                            a.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        });
                        _emamiContext.SaveChanges();
                    }
                    else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                    {
                        saudaModificationsWithSauda.Select(x => x.SaudaModification).ToList().ForEach(a =>
                        {
                            a.StatusId = inputDto.StatusId;
                            a.ModifiedBy = inputDto.ModifiedBy;
                            a.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                            if (!string.IsNullOrEmpty(inputDto.Remarks))
                            {
                                var entity = new Remarks()
                                {
                                    TableId = a.Id,
                                    TableName = "SaudaModifications",
                                    ReasonTypeId = inputDto.StatusId,
                                    Description = inputDto.Remarks,
                                    CreatedBy = inputDto.ModifiedBy,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                    IsActive = true
                                };
                                _emamiContext.Remarks.Add(entity);
                            }
                        });
                        _emamiContext.SaveChanges();
                    }
                    else
                    {
                        saudaModificationsWithSauda.Select(x => x.SaudaModification).ToList().ForEach(a =>
                        {
                            a.StatusId = (int)DTO.Enums.Status.Pending;
                            a.ModifiedBy = inputDto.ModifiedBy;
                            a.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        });
                        _emamiContext.SaveChanges();
                    }

                    var saudaModificationApprovalContextlist = _emamiContext.SaudaModificationApprovals.Where(_ => inputDto.SaudaModificationIds.Contains(_.SaudaModificationId)).ToList();
                    saudaModificationApprovalContextlist.ForEach(a =>
                    {
                        a.StatusId = inputDto.StatusId;
                        //a.Remarks = inputDto.Remarks;
                        a.ModifiedBy = inputDto.ModifiedBy;
                        a.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    });
                    _emamiContext.SaveChanges();

                    if (inputDto.StatusId == (int)DTO.Enums.Status.Approved && loginUserRole != null && loginUserRole.RoleId != roleId)
                    {
                        //saudaModificationsContext.ForEach(a =>
                        //{

                        //    var saudaContext = _emamiContext.Sauda.AsNoTracking().Where(_ => _.SaudaNumber == a.SaudaNumber).FirstOrDefault();

                        //    var requestedToUser = (from uc in _emamiContext.UserReportingToMappings.AsNoTracking()
                        //                           join udiv in _emamiContext.UserDivisionMappings.AsNoTracking() on uc.UserId equals udiv.UserId
                        //                           where
                        //                           udiv.SalesOrganizationId == saudaContext.SalesOrganizationId
                        //                           && udiv.DistributionChannelId == saudaContext.DistributionChannelId
                        //                           && udiv.DivisionId == saudaContext.DivisionId
                        //                           && uc.UserId == inputDto.LoginUserId
                        //                           select uc.ReportingToUserId
                        //             ).FirstOrDefault();

                        //    //Sauda approval save
                        //    var saudaModificationApprovalContext = new SaudaModificationApproval
                        //    {
                        //        RequestedBy = inputDto.LoginUserId,
                        //        RequestedTo = requestedToUser > 0 ? requestedToUser : 0,
                        //        CreatedBy = inputDto.LoginUserId,
                        //        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        //        StatusId = (int)DTO.Enums.Status.Pending,
                        //        ApprovedBy = inputDto.LoginUserId,
                        //        SaudaModificationId = a.Id,
                        //        Remarks = inputDto.Remarks
                        //    };
                        //    _emamiContext.SaudaModificationApprovals.Add(saudaModificationApprovalContext);
                        //});
                        foreach (var item in saudaModificationsWithSauda)
                        {
                            var saudaModification = item.SaudaModification;
                            var saudaContext = item.Sauda; // never null (inner join)

                            var requestedToUser = (
                                from uc in _emamiContext.UserReportingToMappings.AsNoTracking()
                                join udiv in _emamiContext.UserDivisionMappings.AsNoTracking()
                                    on uc.UserId equals udiv.UserId
                                where
                                    udiv.SalesOrganizationId == saudaContext.SalesOrganizationId &&
                                    udiv.DistributionChannelId == saudaContext.DistributionChannelId &&
                                    udiv.DivisionId == saudaContext.DivisionId &&
                                    uc.UserId == inputDto.LoginUserId
                                select uc.ReportingToUserId
                            ).FirstOrDefault();

                            var saudaModificationApprovalContext = new SaudaModificationApproval
                            {
                                RequestedBy = inputDto.LoginUserId,
                                RequestedTo = requestedToUser > 0 ? requestedToUser : 0,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                StatusId = (int)DTO.Enums.Status.Pending,
                                ApprovedBy = inputDto.LoginUserId,
                                SaudaModificationId = saudaModification.Id,
                                Remarks = inputDto.Remarks
                            };

                            _emamiContext.SaudaModificationApprovals.Add(saudaModificationApprovalContext);
                        }
                        _emamiContext.SaveChanges();
                    }

                    if (inputDto.StatusId == (int)DTO.Enums.Status.Approved && loginUserRole != null && loginUserRole.RoleId == roleId)
                    {
                        //method to sync sauda approval from APP to SAP 
                        HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                        {
                            _sapIntegrationService.SendSaudaModificationInfoToSAP(inputDto.SaudaModificationIds, true);
                        });
                    }
                }
                else { return _resultService.ErrorMessage(Constants.RecordNotFound); }

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

        private static decimal GetSaudaModificationToleranceInMT()
        {
            try
            {
                var raw = ConfigurationManager.AppSettings["SaudaModificationToleranceInMT"];
                if (!string.IsNullOrWhiteSpace(raw)
                    && decimal.TryParse(raw, NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out var parsed)
                    && parsed >= 0m)
                {
                    return parsed;
                }
            }
            catch
            {
                // intentional: swallow exceptions and fall back to default
            }

            return 0.03m;// 30 kg = 0.03 MT
        }

        #endregion
    }
}
