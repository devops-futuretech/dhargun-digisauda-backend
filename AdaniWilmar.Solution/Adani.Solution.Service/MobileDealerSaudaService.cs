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
using System.IO;
using System.Web.Hosting;
using System.Globalization;
using System.Web.Script.Serialization;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using MimeKit;
using Adani.Solution.DTO.Enums;
using System.Net.NetworkInformation;
using Adani.Solution.Data.Seeder;
using System.Security.Policy;
using Adani.Solution.DTO.QPSDiscount;
using Amazon.Auth.AccessControlPolicy;

namespace Adani.Solution.Service
{
    public interface IMobileDealerSaudaService
    {
        ResultDto DealerSaudaDetails(IdInputDto inputDto);
        ResultDto GetDealerDetail(IdInputDto IdDto);

        ResultDto SaudaCreation(SaudaInputDto inputDto);
        ResultDto GetSaudaList(SaudaFilterDto inputDto);
        ResultDto GetSaudaDetails(SaudaDetailInputDto inputDto);
        ResultDto GetSaudaShortViewList(LoginUserIdCoversionDto loginUserIdDto);
        ResultDto GetSaudaShortViewDetails(SaudaDetailInputDto inputDto);
        ResultDto GetExpiredSaudaListForMobile(SaudaFilterDto saudaFilterDto);
        ResultDto GetSkuListForIndentRequest(SkuInputDto skuInputDto);

        //Sauda Limit
        ResultDto GetSaudaLimitRequestHistory(IdInputDto inputDto);
        ResultDto GetSaudaLimitRequestHistoryDetail(IdInputDto inputDto);
        ResultDto AddSaudaLimitRequest(SaudaLimitRequestHistoryDto saudaLimitRequestHistoryDto);

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
        ResultDto GetDealerLocationsByDealerId(IdInputDto inputDto);

        //Special Rate Approval Request
        ResultDto AddSpecialRateApprovalRequest(SpecialRateApprovalAddDto inputDto);
        ResultDto GetSpecialRateRequestList(SpecialRateInputDto specialRateInputDto);
        ResultDto GetSpecialRateRequestDetails(SpecialRateDetailInputDto specialRateDetailInputDto);
        ResultDto SaudaCreationFromSpecialRate(SpecialRateSaudaDto inputDto);

        ResultDto GetPendingSaudaChartForMobile(LoginUserIdDto loginUserIdDto);

        //Sales Credit Limit
        ResultDto GetTotalCreditLimit(LoginUserIdDto loginUserIdDto);
        ResultDto GetCreditLimitList(LoginUserIdDto loginUserIdDto);

        //CompetitorAnalysis
        ResultDto SaveCompetitorAnalysis(CompetitorAnalysisAddDto inputDto);

        //Sauda Conversion
        ResultDto AddSaudaConversionOrders(SaudaConversionAddDto saudaConversionAddDto);
        ResultDto GetSaudaConversionList(SaudaFilterDto saudaFilterDto);
        ResultDto GetSaudaConversionDetails(SaudaConversionDetailInputDto inputDto);

        //Sauda Extension
        ResultDto AddSaudaExtension(SaudaExtensionAddDto saudaExtensionAddDto);
        ResultDto GetSaudaExtensionList(SaudaFilterDto saudaFilterDto);
        ResultDto NewAddSaudaExtension(SaudaExtensionNewAddDto saudaExtensionAddDto);

        ResultDto GetPendingSaudaChartDetailForMobile(LoginUserIdDto loginUserIdDto);
        ResultDto GetBookedSauda(LoginUserIdDto loginUserIdDto);
        ResultDto GetSaudaorderdetails(SaudaDetailInputDto inputDto);

        //Counter Bid
        //ResultDto GetSaudaCounterBidDetails(SaudaDetailInputDto inputDto);
        //ResultDto ApproveCounterBid(CounterBidInputDto inputDto);
        ResultDto GetPendingContractChartMobile(LoginUserIdDto loginUserIdDto);

        #region New Change Sauda Conversion CR
        ResultDto GetSKUListForSaudaConversion(SaudaConversionSKUInputDto inputDto);
        ResultDto GetDealerPlantDepotList(UserIdDto inputDto);
        ResultDto SaveSaudaConversionSkuDetails(SaudaConversionSKUInputDto inputDto);
        ResultDto GetSaudaConversionPendingAndApprovedList(SaudaReportFilterDto inputDto);
        ResultDto GetZonalHeadSaudaConversionPendingApprovedList(SaudaReportFilterDto inputDto);
        ResultDto GetBDOSaudaConversionPendingApprovedList(SaudaReportFilterDto inputDto);
        ResultDto GetDealerSaudaConversionPendingApprovedList(SaudaReportFilterDto inputDto);
        ResultDto GetSaudaConversionSkuDetailsById(SaudaConversionSKUInputDto inputDto);
        ResultDto GetSaudaConversionUnitAndBaseRateList(SaudaConversionUnitAndDiffRateInputDto inputDto);
        ResultDto GetSaudaConversionReport(SaudaConversionReportInputDto inputDto);
        #endregion

        ResultDto GetSapSyncPendingSaudaConversionList(SaudaConversionInputDto inputDto);

        ResultDto GetSaudaConversionListMobile(SaudaConversionInputDTO inputDto);

        ResultDto GetContractNumberList(ContractNoInputDto inputDto);
        ResultDto GetSkuListByContractNumber(ContractNoInputDto inputDto);

    }
    public class MobileDealerSaudaService : IMobileDealerSaudaService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Mobile Dealer Sauda Service");
        private const string ServiceName = "Mobile Dealer Sauda Service";
        private string _methodName;
        private readonly IResultService _resultService;
        private readonly INotificationService _notificationService;
        private readonly ISAPIntegrationService _sapIntegrationService;
        private readonly IQpsService _qpsService;
        private readonly ILookupService _lookupService;


        public MobileDealerSaudaService(IAdaniContext salesContext, IResultService resultService, INotificationService notificationService, ISAPIntegrationService sapIntegrationService, IQpsService qpsService, ILookupService lookupService)
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
        public ResultDto DealerSaudaDetails(IdInputDto inputDto)
        {
            _methodName = "DealerSaudaDetails";
            var dealerSaudaDetailsDto = new DealerSaudaDetailsDto();
            var _userDivisionSaudaLimitList = new List<UserDivisionSaudaLimitDto>();

            if (inputDto == null)
            {
                return NotFoundResult();
            }

            try
            {
                var overallStatus = Constants.OverallSaudaStatus;

                var overAllSaudaContext = (from s in _emamiContext.Sauda.AsNoTracking()
                                           join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                                           where s.UserId == inputDto.Id &&
                                                 s.SalesOrganizationId == inputDto.SalesOrganizationId &&
                                                 s.DistributionChannelId == inputDto.DistributionChannelId &&
                                                 s.DivisionId == inputDto.DivisionId &&
                                                 s.SaudaNumber == null &&
                                                 s.StatusId == (int)DTO.Enums.Status.Pending
                                           select so).ToList();
                // * var status = Constants.OutstandingSaudaStatus;

                var SaudaLimitContext = (from u in _emamiContext.Users.AsNoTracking().Where(_ => _.Id == inputDto.Id)
                                         join udm in _emamiContext.UserDivisionMappings.AsNoTracking()
                                             .Where(_ => _.SalesOrganizationId == inputDto.SalesOrganizationId &&
                                                         _.DistributionChannelId == inputDto.DistributionChannelId &&
                                                         _.DivisionId == inputDto.DivisionId)
                                         on u.Id equals udm.UserId
                                         select new
                                         {
                                             u.SaudaValidityPeriod,
                                             udm.SaudaLimit,
                                             udm.DivisionId
                                         }).ToList();

                // * var SaudaLimitContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.Id);
                // * var SaudaOutstandingContext = (from s in _emamiContext.Sauda.AsNoTracking()
                // *                               join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                // *                               where s.UserId == inputDto.Id
                // *                               && status.Contains(so.StatusId)
                // *                               select so).ToList();

                // * var SaudaOutstanding = SaudaOutstandingContext.Sum(_ => _.BidQuantity);


                dealerSaudaDetailsDto.TotalSaudaLimit = SaudaLimitContext.Sum(x => x.SaudaLimit ?? 0);
                // * dealerSaudaDetailsDto.OutstandingSaudaLimit = SaudaOutstanding;
                // * dealerSaudaDetailsDto.AvailableSaudaLimit = SaudaLimitContext.SaudaLimit - SaudaOutstanding;

                dealerSaudaDetailsDto.AvailableSaudaLimit = SaudaLimitContext.Sum(x => x.SaudaLimit ?? 0);
                //if (overAllSaudaContext != null && overAllSaudaContext.Any())
                //{
                var SaudaOutstanding = overAllSaudaContext.Sum(_ => _.BidQuantity);
                dealerSaudaDetailsDto.OutstandingSaudaLimit = SaudaOutstanding;
                //decimal invoiceQuantity = 0;
                //decimal RtninvoiceQuantity = 0;
                //var existingSaudaQuantity = overAllSaudaContext.Sum(_ => _.BidQuantity);
                //var skuIds = overAllSaudaContext.Select(_ => _.SkuId).Distinct().ToList();
                //var invoiceContext = (from inv in _emamiContext.Invoices.AsNoTracking()
                //                      join invDet in _emamiContext.InvoiceDetails.AsNoTracking() on inv.Id equals invDet.InvoiceId
                //                      where inv.UserId == inputDto.Id && inv.SalesDocumentType != "ZHCR"
                //                      && skuIds.Contains(invDet.SkuId)
                //                      select invDet
                //                          ).ToList();
                //var rtninvoiceContext = (from inv in _emamiContext.Invoices.AsNoTracking()
                //                      join invDet in _emamiContext.InvoiceDetails.AsNoTracking() on inv.Id equals invDet.InvoiceId
                //                      where inv.UserId == inputDto.Id && inv.SalesDocumentType == "ZHCR"
                //                      && skuIds.Contains(invDet.SkuId)
                //                      select invDet
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

                if (inputDto.SalesOrganizationId != 0 &&
                    inputDto.DistributionChannelId != 0 &&
                    inputDto.DivisionId != 0 &&
                    usersaudalimit != 0)
                {
                    dealerSaudaDetailsDto.AvailableSaudaLimit =
                        _resultService.AvailableSaudaLimit(inputDto.Id, usersaudalimit,
                                                           inputDto.SalesOrganizationId,
                                                           inputDto.DistributionChannelId,
                                                           inputDto.DivisionId);
                }
                //}

                var incoTermList =
                    (from incoterm in _emamiContext.IncoTerms.AsNoTracking()
                     join userIncoterm in _emamiContext.UserIncoTerms.AsNoTracking()
                         on incoterm.Id equals userIncoterm.IncoTermsId
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

                // 2. Fetch depots for all mapped divisions
                // Step 1: Get all UserDivisionIds for the given user
                var userDivisionIds = _emamiContext.UserDivisionMappings
                    .Where(ud => ud.UserId == inputDto.Id)
                    .Select(ud => ud.Id)
                    .ToList();

                // Step 2: Fetch depots mapped to those UserDivisionIds
                var plantList = (from depot in _emamiContext.Depots.AsNoTracking()
                                 join uddm in _emamiContext.UserDivisionDepotMappings.AsNoTracking()
                                     on depot.Id equals uddm.DepotId
                                 where userDivisionIds.Contains(uddm.UserDivisionId)
                                       && depot.IsActive
                                       && depot.IsPlant
                                 select new DepotDto
                                 {
                                     Id = depot.Id,
                                     Name = depot.Name + "-" + depot.Code,
                                     Code = depot.Code,
                                     IsPlant = depot.IsPlant,
                                     IsActive = depot.IsActive
                                 })
                                 .GroupBy(d => d.Id)
                .Select(g => g.FirstOrDefault())  // Use FirstOrDefault instead of First
                .Where(d => d != null)            // Filter out nulls just in case
                .ToList();

                // Assign to your DTO
                dealerSaudaDetailsDto.PlantDepotListNew = plantList;

                //if (brokerContext != null)
                //{
                //    dealerSaudaDetailsDto.BrokerId = brokerContext.BrokerId;
                //    dealerSaudaDetailsDto.Broker = brokerContext.BrokerName;
                //

                var brokerContext = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.CustomerId == inputDto.Id)
                    .Join(_emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.Broker),
                          uc => uc.UserId,
                          ur => ur.UserId,
                          (uc, ur) => new { BrokerId = uc.UserId })
                    .Join(_emamiContext.Users.AsNoTracking(),
                          x => x.BrokerId,
                          u => u.Id,
                          (x, u) => new { BrokerId = u.Id, BrokerName = u.Name })
                    .Select(a => new DropDownDto
                    {
                        Id = a.BrokerId,
                        Name = a.BrokerName
                    }).ToList();

                if (brokerContext.IsAny())
                {
                    dealerSaudaDetailsDto.BrokerList = brokerContext;
                }

                // 3. Fetch correct SaudaValidityPeriod from Users table directly
                dealerSaudaDetailsDto.SaudaValidityPeriod =
     _emamiContext.UserDivisionMappings
     .Where(udm => udm.UserId == inputDto.Id
                   && udm.SalesOrganizationId == inputDto.SalesOrganizationId
                   && udm.DistributionChannelId == inputDto.DistributionChannelId
                   && udm.DivisionId == inputDto.DivisionId)
     .Select(udm => udm.SaudaValidityPeriod)
     .FirstOrDefault() ?? 0;




                var overAllSaudaDivisionContext = (from sdc in overAllSaudaContext
                                                   group sdc by sdc.DivisionId into g
                                                   select new UserDivisionSaudaLimitDto
                                                   {
                                                       DivisionId = g.First().DivisionId,
                                                       SaudaOrderQty = g.Sum(x => x.BidQuantity)
                                                   }).ToList();

                var pcContext = _emamiContext.PendingContracts.AsNoTracking()
                                .Where(x => x.UserId == inputDto.Id).ToList();

                var pendingContractDivisionContext = (from pc in pcContext
                                                      group pc by pc.DivisionId into g
                                                      select new UserDivisionSaudaLimitDto
                                                      {
                                                          DivisionId = g.First().DivisionId,
                                                          PendingContractQty = g.Sum(x => x.SaudaQuantity)
                                                      }).ToList();

                var _userDivisionSaudaLimitList1 =
                    (from sc in SaudaLimitContext
                     select new UserDivisionSaudaLimitDto
                     {
                         DivisionId = sc.DivisionId,
                         SaudaLimit = sc.SaudaLimit ?? 0
                     }).ToList();

                overAllSaudaDivisionContext.AddRange(
                    _userDivisionSaudaLimitList1
                    .Where(x => !overAllSaudaDivisionContext.Any(y => y.DivisionId == x.DivisionId)));

                pendingContractDivisionContext.AddRange(
                    _userDivisionSaudaLimitList1
                    .Where(x => !pendingContractDivisionContext.Any(y => y.DivisionId == x.DivisionId)));

                _userDivisionSaudaLimitList =
                    (from udsl in _userDivisionSaudaLimitList1
                     join sc in overAllSaudaDivisionContext on udsl.DivisionId equals sc.DivisionId
                     join pc in pendingContractDivisionContext on sc.DivisionId equals pc.DivisionId
                     select new UserDivisionSaudaLimitDto
                     {
                         DivisionId = udsl.DivisionId,
                         SaudaLimit = udsl.SaudaLimit,
                         SaudaOrderQty = sc.SaudaOrderQty,
                         PendingContractQty = pc.PendingContractQty,
                         AvailableSaudaLimit = udsl.SaudaLimit - sc.SaudaOrderQty - pc.PendingContractQty
                     }).ToList();

                dealerSaudaDetailsDto.UserDivisionSaudaLimitList = _userDivisionSaudaLimitList;

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

                if (IdDto.SalesOrganizationId == 0)
                {
                    return _resultService.ErrorMessage(Constants.SalesOrganisationIsEmpty);
                }
                if (IdDto.DistributionChannelId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DistributionChannelIsEmpty);
                }
                if (IdDto.DivisionId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DivisionIsEmpty);
                }
                var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == IdDto.Id);

                if (dealerContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                var status = Constants.OutstandingSaudaStatus;
                var SaudaOutstandingContext = (from s in _emamiContext.Sauda.AsNoTracking()
                                               join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                                               where s.UserId == IdDto.Id
                                               && status.Contains(so.StatusId)
                                               select so
                                               ).ToList();

                var SaudaOutstanding = SaudaOutstandingContext.Sum(_ => _.BidQuantity);

                outputDto.DealerId = dealerContext.Id;
                outputDto.DealerName = dealerContext.Name;
                outputDto.DealerCode = dealerContext.Code;

                var userdivContext = _emamiContext.UserDivisionMappings.AsNoTracking()
                            .FirstOrDefault(_ => _.UserId == IdDto.DealerId
                            && _.SalesOrganizationId == IdDto.SalesOrganizationId
                            && _.DistributionChannelId == IdDto.DistributionChannelId
                            && _.DivisionId == IdDto.DivisionId);
                outputDto.CurrentLimit = userdivContext.SaudaLimit ?? 0;
                outputDto.SaudaOutStatnding = SaudaOutstanding;

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
        public ResultDto SaudaCreation(SaudaInputDto inputDto)
        {
            _methodName = "SaudaCreation";
            var resultDto = new ResultDto();
            try
            {
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                List<long> reportingToMappingLimitIdsList = new List<long>();
                List<long> userqntyLimitIdsList = new List<long>();
                SpecalityFatDiscountUser reporttoqnty = null;
                SpecalityFatDiscountUser userqnty = null;
                decimal orderedQuantityBdo = 0;
                decimal totalQuantityBdo = 0;
                decimal requestedQuantityBdo = 0;
                decimal saudaBidQuantity = 0;
                decimal actualDiscountQuantityBdo = 0;
                var delarNotQuantity = false;
                var delarIsQuantity = false;
                var IsReportingtoAllocation = false;
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                var isSalesAreaBookingValid = _resultService.IsSalesAreaBookingValid(inputDto);

                if(!isSalesAreaBookingValid)
                {
                    return _resultService.ErrorMessage(Constants.SaudaSalesAreaRestricitedDistributor);
                }

                var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);

                if (dealerContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var saudaConditionData = _resultService.IsSaudaConditionalBookingValid(inputDto);

                if(!saudaConditionData.Item1)
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
                var userdivContext = _emamiContext.UserDivisionMappings.AsNoTracking()
                        .FirstOrDefault(_ => _.UserId == inputDto.DealerId
                        && _.SalesOrganizationId == inputDto.SalesOrganizationId
                        && _.DistributionChannelId == inputDto.DistributionChannelId
                        && _.DivisionId == inputDto.DivisionId);

                var SalesTrader = (from ucm in _emamiContext.UserCustomerMapping
                                   join u in _emamiContext.Users on ucm.UserId equals u.Id
                                   join ud in _emamiContext.UserDivisionMappings on ucm.UserId equals ud.UserId
                                   join ur in _emamiContext.UserRoles on u.Id equals ur.UserId
                                   where ucm.CustomerId == inputDto.DealerId && ud.SalesOrganizationId == inputDto.SalesOrganizationId
                                   && ud.DistributionChannelId == inputDto.DistributionChannelId
                                   && ud.DivisionId == inputDto.DivisionId && ur.RoleId == (int)DTO.Enums.Role.StateTrader
                                   select new
                                   {
                                       Distributor = ucm.CustomerId,
                                       SalesTrader = ucm.UserId,
                                       Salestrader = u.Name,
                                       PushTokenKey = u.PushTokenKey,
                                       RegistrationTypeId = u.RegistrationTypeId,
                                       SaudaLimit = ud.SaudaLimit
                                   }).FirstOrDefault();

                var ReportingToUserId = (from urm in _emamiContext.UserReportingToMappings
                                         where urm.UserId == SalesTrader.SalesTrader
                                         select urm.ReportingToUserId).FirstOrDefault();

                var saudaLimitUser = userdivContext.SaudaLimit ?? 0;
                var SaudaLimit = _resultService.AvailableSaudaLimit(inputDto.DealerId, saudaLimitUser, inputDto.SalesOrganizationId, inputDto.DistributionChannelId, inputDto.DivisionId);
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
                    var skucontext = _emamiContext.Skus.AsNoTracking();
                    string oilTypeBasedError = string.Empty;
                    string oilTypeExpiredError = string.Empty;
                    string userLimitNotExistError = string.Empty;
                    string oilTypeBasedRestrictionError = string.Empty;
                    var checkedOiltypeIdsForRestrict = new List<long>();

                    foreach (var item in inputDto.SaudaOrders)
                    {
                        var skudata = skucontext.FirstOrDefault(_ => _.Id == item.SkuId);

                        if (skudata != null)
                        {
                            var oiltypeContext = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == skudata.OilTypeId);

                            if (oiltypeContext != null)
                            {
                                item.OilTypeId = oiltypeContext.Id;
                                decimal availableQuantityBdo = 0;

                                userqnty = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().FirstOrDefault(_ => _.UserId == SalesTrader.SalesTrader && _.OilTypeId == skudata.OilTypeId && _.ParentId == 0 &&
                                _.ValidFrom <= currentDate && _.ValidTo >= currentDate && _.SalesOrganizationId == inputDto.SalesOrganizationId &&
                                _.DistributionChannelId == inputDto.DistributionChannelId && _.DivisionId == inputDto.DivisionId);

                                if (userqnty != null)
                                {
                                    userqntyLimitIdsList.Add(userqnty.Id);
                                }

                                if (userqnty == null)
                                {
                                    reporttoqnty = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().FirstOrDefault(_ => _.UserId == ReportingToUserId && _.OilTypeId == skudata.OilTypeId && _.ParentId == 0 &&
                                       _.ValidFrom <= currentDate && _.ValidTo >= currentDate && _.SalesOrganizationId == inputDto.SalesOrganizationId &&
                                       _.DistributionChannelId == inputDto.DistributionChannelId && _.DivisionId == inputDto.DivisionId);

                                    var reportToQntyId = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().Where(_ => _.UserId == ReportingToUserId && _.OilTypeId == skudata.OilTypeId && _.ParentId == 0 &&
                                       _.ValidFrom <= currentDate && _.ValidTo >= currentDate && _.SalesOrganizationId == inputDto.SalesOrganizationId &&
                                       _.DistributionChannelId == inputDto.DistributionChannelId && _.DivisionId == inputDto.DivisionId)
                                        .Select(_ => _.Id).FirstOrDefault();

                                    if (reportToQntyId > 0)
                                    {
                                        reportingToMappingLimitIdsList.Add(reportToQntyId);
                                    }
                                }

                                if (userqnty != null)
                                {
                                    delarIsQuantity = true;

                                    if (currentDate >= userqnty.ValidFrom && currentDate <= userqnty.ValidTo)
                                    {
                                        saudaBidQuantity = _resultService.GetSaudaBookedQuantityForCurrentDateByDateRangeIsNotReportingtoAllocation(inputDto, item.OilTypeId, userqnty.ValidFrom, userqnty.ValidTo);
                                    }

                                    requestedQuantityBdo = _resultService.ConvertCasetoMetricTon(item.BidQuantity, item.SkuId);

                                    if (userqnty.RequestedDiscount > 0 && userqnty.RequestedDiscountDate.HasValue && userqnty.RequestedDiscountDate.Value.Date != DateHelper.UtcToIndia(DateTime.UtcNow).Date)
                                    {
                                        orderedQuantityBdo = saudaBidQuantity;
                                        actualDiscountQuantityBdo = userqnty.ActualDiscount - orderedQuantityBdo;
                                        var actualAvailableQuantityBdo = actualDiscountQuantityBdo - userqnty.RequestedDiscount;
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
                                            oilTypeExpiredError = oilTypeExpiredError + Constants.QuantityLimitExpired.Replace(Constants.OiltypeName, oiltypeContext.Name) + Environment.NewLine;
                                        }
                                    }
                                    else
                                    {
                                        totalQuantityBdo = requestedQuantityBdo;
                                        if (saudaBidQuantity != 0)
                                        {
                                            orderedQuantityBdo = saudaBidQuantity;
                                            totalQuantityBdo = userqnty.ActualDiscount - orderedQuantityBdo;
                                        }
                                        if (totalQuantityBdo > userqnty.ActualDiscount || totalQuantityBdo > userqnty.RemainingQuantity)
                                        {
                                            availableQuantityBdo = userqnty.ActualDiscount - orderedQuantityBdo;
                                            if (availableQuantityBdo < 0)
                                            {
                                                availableQuantityBdo = 0;
                                            }
                                            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => SaudaCreateLimitNotificationAsync(inputDto.DealerId, inputDto.SalesOrganizationId, inputDto.DistributionChannelId, inputDto.DivisionId, oiltypeContext.Name, cancellationToken));
                                            oilTypeBasedError = oilTypeBasedError + Constants.OilTypeLimitExceeds.Replace(Constants.OiltypeName, oiltypeContext.Name).Replace(Constants.Quantity, userqnty.RemainingQuantity.ToString()) + "\n";
                                        }
                                    }
                                }

                                if (userqnty == null && reporttoqnty != null)
                                {
                                    delarNotQuantity = true;
                                    IsReportingtoAllocation = true;

                                    if (currentDate >= reporttoqnty.ValidFrom && currentDate <= reporttoqnty.ValidTo)
                                    {
                                        saudaBidQuantity = _resultService.GetSaudaBookedQuantityForCurrentDateByDateRangeIsReportingtoAllocation(inputDto, item.OilTypeId, reporttoqnty.ValidFrom, reporttoqnty.ValidTo);
                                    }

                                    requestedQuantityBdo = _resultService.ConvertCasetoMetricTon(item.BidQuantity, item.SkuId);

                                    if (reporttoqnty.RequestedDiscount > 0 && reporttoqnty.RequestedDiscountDate.HasValue && reporttoqnty.RequestedDiscountDate.Value.Date != DateHelper.UtcToIndia(DateTime.UtcNow).Date)
                                    {
                                        orderedQuantityBdo = saudaBidQuantity;
                                        actualDiscountQuantityBdo = reporttoqnty.ActualDiscount - orderedQuantityBdo;
                                        var actualAvailableQuantityBdo = (actualDiscountQuantityBdo - reporttoqnty.RequestedDiscount);

                                        if (actualAvailableQuantityBdo < 0) { actualAvailableQuantityBdo = 0; }                                           

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
                                            oilTypeExpiredError = oilTypeExpiredError + Constants.QuantityLimitExpired.Replace(Constants.OiltypeName, oiltypeContext.Name) + Environment.NewLine;
                                        }
                                    }
                                    else
                                    {
                                        totalQuantityBdo = requestedQuantityBdo;
                                        if (saudaBidQuantity != 0)
                                        {
                                            orderedQuantityBdo = saudaBidQuantity;
                                            totalQuantityBdo = orderedQuantityBdo + requestedQuantityBdo;
                                        }
                                        else if (saudaBidQuantity == 0)
                                        {
                                            totalQuantityBdo = requestedQuantityBdo;
                                        }
                                        if (totalQuantityBdo > reporttoqnty.ActualDiscount || requestedQuantityBdo > reporttoqnty.RemainingQuantity)
                                        {
                                            availableQuantityBdo = reporttoqnty.ActualDiscount - orderedQuantityBdo;
                                            if (availableQuantityBdo < 0)
                                            {
                                                availableQuantityBdo = 0;
                                            }
                                            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => SaudaCreateLimitNotificationAsync(inputDto.DealerId, inputDto.SalesOrganizationId, inputDto.DistributionChannelId, inputDto.DivisionId, oiltypeContext.Name, cancellationToken));
                                            oilTypeBasedError = oilTypeBasedError + Constants.OilTypeLimitExceeds.Replace(Constants.OiltypeName, oiltypeContext.Name).Replace(Constants.Quantity, reporttoqnty.RemainingQuantity.ToString()) + "\n";
                                        }
                                    }
                                }
                                else if (userqnty == null && reporttoqnty == null && SaudaLimit == 0)
                                {
                                    userLimitNotExistError = userLimitNotExistError + Constants.UserLimitNotExists.Replace(Constants.OiltypeName, oiltypeContext.Name) + Environment.NewLine;
                                }
                                
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
                            var userdiscount = _emamiContext.DiscountUsers.AsNoTracking().OrderByDescending(s => s.Id).FirstOrDefault(a => a.SkuId == item.SkuId && a.ParentId != 0 && a.UserId == inputDto.LoginUserId &&
                            currentDate >= a.ValidFrom && currentDate <= a.ValidTo);
                            if (userdiscount != null)
                            {
                                if (item.DiscountAmountPerCase > userdiscount.ActualDiscount)
                                {
                                    //var geodiscount = _emamiContext.DiscountGeography.AsNoTracking().OrderByDescending(s => s.Id).FirstOrDefault(a => a.SkuId == item.SkuId &&
                                    //    currentDate >= a.ValidFrom && currentDate <= a.ValidTo && ((a.CityId == dealerContext.CityId || a.CityId == 0) && (a.DistrictId == dealerContext.DistrictId || a.DistrictId == 0)
                                    //    && (a.StateId == dealerContext.StateId || a.StateId == 0) && a.ZoneId == dealerContext.ZoneId));

                                    //var geodiscount = _emamiContext.DiscountGeography.AsNoTracking().OrderByDescending(s => s.Id).FirstOrDefault(a => a.SkuId == item.SkuId &&
                                    //    currentDate >= a.ValidFrom && currentDate <= a.ValidTo && ((a.CityId == dealerContext.CityId || a.CityId == 0) && (a.DistrictId == dealerContext.DistrictId || a.DistrictId == 0)
                                    //    && (a.StateId == dealerContext.StateId || a.StateId == 0) && a.ZoneId == dealerContext.ZoneId));

                                    //if(geodiscount == null)
                                    //{
                                    //    geodiscount = _emamiContext.DiscountGeography.AsNoTracking().OrderByDescending(s => s.Id).FirstOrDefault(a => a.OilTypeId == item.OilTypeId &&
                                    //    currentDate >= a.ValidFrom && currentDate <= a.ValidTo && ((a.CityId == dealerContext.CityId || a.CityId == 0) && (a.DistrictId == dealerContext.DistrictId || a.DistrictId == 0)
                                    //    && (a.StateId == dealerContext.StateId || a.StateId == 0) && a.ZoneId == dealerContext.ZoneId));
                                    //}


                                    //if (geodiscount != null)
                                    //{
                                    //    if (skudata.OilPackGroupTypeId != null)
                                    //    {
                                    //        if (skudata.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                    //        {
                                    //            calculatedDiscount = geodiscount.ActualDiscount;
                                    //        }
                                    //        else if (skudata.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                                    //        {
                                    //            calculatedDiscount = _resultService.CalculateAutomatedDiscount(geodiscount.ActualDiscount, geodiscount.SkuId, item.SkuId);
                                    //        }
                                    //    }

                                    //    if (Math.Round(item.DiscountAmountPerCase, 2) > Math.Round(calculatedDiscount, 2))
                                    //    {
                                    //        return _resultService.ErrorMessage(item.DiscountAmountPerCase + " " + Constants.DiscountNotExists);
                                    //    }
                                    //}
                                    //else if(item.DiscountAmountPerCase > 0)
                                    //{
                                    //    return _resultService.ErrorMessage(item.DiscountAmountPerCase + " " + Constants.DiscountNotExists);
                                    //}

                                    // Direct SkuId match (unchanged query)
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
                                        // Fallback: same OilType AND same OilPackGroupType (join Skus to access OilPackGroupTypeId)
                                        geodiscount = (from a in _emamiContext.DiscountGeography.AsNoTracking()
                                                       join s in _emamiContext.Skus.AsNoTracking() on a.SkuId equals s.Id
                                                       where a.OilTypeId == item.OilTypeId
                                                          && currentDate >= a.ValidFrom && currentDate <= a.ValidTo
                                                          && (a.CityId == dealerContext.CityId || a.CityId == 0)
                                                          && (a.DistrictId == dealerContext.DistrictId || a.DistrictId == 0)
                                                          && (a.StateId == dealerContext.StateId || a.StateId == 0)
                                                          && a.ZoneId == dealerContext.ZoneId
                                                          && s.OilPackGroupTypeId == skudata.OilPackGroupTypeId
                                                       orderby a.Id descending
                                                       select a).FirstOrDefault();

                                        if (geodiscount != null && skudata.OilPackGroupTypeId != null)
                                        {
                                            if (skudata.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                            {
                                                calculatedDiscount = geodiscount.ActualDiscount;
                                            }
                                            else if (skudata.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
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

                                //if(geodiscount == null)
                                //{
                                //    geodiscount = _emamiContext.DiscountGeography.AsNoTracking().OrderByDescending(s => s.Id).FirstOrDefault(a => a.OilTypeId == item.OilTypeId &&
                                //        currentDate >= a.ValidFrom && currentDate <= a.ValidTo && ((a.CityId == dealerContext.CityId || a.CityId == 0) && (a.DistrictId == dealerContext.DistrictId || a.DistrictId == 0)
                                //        && (a.StateId == dealerContext.StateId || a.StateId == 0) && a.ZoneId == dealerContext.ZoneId));
                                //}


                                //if (geodiscount != null)
                                //{
                                //    if (skudata.OilPackGroupTypeId != null)
                                //    {
                                //        if (skudata.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                //        {
                                //            calculatedDiscount = geodiscount.ActualDiscount;
                                //        }
                                //        else if (skudata.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
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

                                // Direct SkuId match (unchanged query)
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
                                    // Fallback: same OilType AND same OilPackGroupType (join Skus to access OilPackGroupTypeId)
                                    geodiscount = (from a in _emamiContext.DiscountGeography.AsNoTracking()
                                                   join s in _emamiContext.Skus.AsNoTracking() on a.SkuId equals s.Id
                                                   where a.OilTypeId == item.OilTypeId
                                                      && currentDate >= a.ValidFrom && currentDate <= a.ValidTo
                                                      && (a.CityId == dealerContext.CityId || a.CityId == 0)
                                                      && (a.DistrictId == dealerContext.DistrictId || a.DistrictId == 0)
                                                      && (a.StateId == dealerContext.StateId || a.StateId == 0)
                                                      && a.ZoneId == dealerContext.ZoneId
                                                      && s.OilPackGroupTypeId == skudata.OilPackGroupTypeId
                                                   orderby a.Id descending
                                                   select a).FirstOrDefault();

                                    if (geodiscount != null && skudata.OilPackGroupTypeId != null)
                                    {
                                        if (skudata.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                        {
                                            calculatedDiscount = geodiscount.ActualDiscount;
                                        }
                                        else if (skudata.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
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

                var requestedToUser = (from uc in _emamiContext.UserCustomerMapping.AsNoTracking()
                                       join ur in _emamiContext.UserRoles.AsNoTracking() on uc.UserId equals ur.UserId
                                       join udiv in _emamiContext.UserDivisionMappings.AsNoTracking() on uc.UserId equals udiv.UserId
                                       where ur.RoleId == (int)DTO.Enums.Role.StateTrader
                                       && udiv.SalesOrganizationId == inputDto.SalesOrganizationId
                                       && udiv.DistributionChannelId == inputDto.DistributionChannelId
                                       && udiv.DivisionId == inputDto.DivisionId
                                       && uc.CustomerId == inputDto.LoginUserId
                                       select uc.UserId

                                     ).FirstOrDefault();

                var saudaContext = new Sauda
                {
                    BiddingDate = currentDate,
                    UserId = inputDto.DealerId,
                    BdoId = 0,
                    SaudaBookingTypeId = inputDto.SaudaBookingTypeId,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = currentDate,
                    IsSAPDataSync = false,
                    IsSAPDataSyncApproval = false,
                    SalesOrganizationId = inputDto.SalesOrganizationId,
                    DistributionChannelId = inputDto.DistributionChannelId,
                    DivisionId = inputDto.DivisionId,
                    SalesDocumentType = divisionContext != null ? divisionContext.SalesDocumentType : string.Empty,
                    StatusId = (int)DTO.Enums.Status.Pending,
                    SaudaType=inputDto.SaudaType
                };
                _emamiContext.Sauda.Add(saudaContext);
                _emamiContext.SaveChanges();

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

                        if (item.SaudaValidFromDate != null)
                            saudaValidFromDate = item.SaudaValidFromDate;

                        var IncotermContext = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == item.IncotermsId).Name;
                        IncotermsType = IncotermContext.ToLower().Contains("for") ? "For" : "Ex";

                        #region PricingLive to Pricing DataInsert and Rearrange the Pricing Data
                     
                        var pricingLiveContext = todayPricingContext.FirstOrDefault(_ => _.Id == item.PricingId);
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
                        decimal itemquotedprice = item.BidQuantity * bidPrice;
                        item.QuotedPrice = itemquotedprice;
                        item.BidPrice = itemquotedprice;

                        decimal qpsDiscount = 0;
                        string qpsId = string.Empty;
                        string individualQPSDiscount = string.Empty;
                        if (qpsDiscountResult != null && qpsDiscountResult.Any())
                        {
                            qpsDiscount = qpsDiscountResult.FirstOrDefault(q => q.SkuId == item.SkuId).Discount;
                            qpsDiscount = qpsDiscount * item.BidQuantity;
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
                            DiscountTypeId = item.DiscountTypeId,
                            DiscountAmount = item.DiscountAmount,
                            QPSDiscount = qpsDiscount,
                            QpsId = qpsId,
                            IndividualQPSDiscount = individualQPSDiscount,
                            BidQuantity = _resultService.ConvertCasetoMetricTonWithoutDB(item.BidQuantity, item.SkuId, skuUomMappingData),
                            BidQuantityCase = item.BidQuantity,
                            QuotedPrice = item.QuotedPrice,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = currentDate,
                            //BiddingwindowId = item.BiddingwindowId,
                            SaudaBookingTypeId = inputDto.SaudaBookingTypeId,
                            PricingId = pricingId,
                            //DealerTypeId = DealerTypeId,
                            Incoterms1 = IncotermsType,
                            PlantId = item.PlantId,
                            //DealerLocationId = Convert.ToInt64(dealerContext.FreightRouteId),
                            // CustomerPONumber = dealerContext.Code + currentDate.ToShortDateString(),
                            ValidFromDate = saudaValidFromDate.Value,
                            ValidToDate = item.SaudaValidToDate != null ? item.SaudaValidToDate.Value : saudaValidFromDate.Value.AddDays(Convert.ToDouble(dealerContext.SaudaValidityPeriod > 0 ? dealerContext.SaudaValidityPeriod : Config.DefaultSaudaValidity)),
                            StatusId = statusId,
                            // SaudaStatusId = (int)DTO.Enums.SaudaStatus.NotReleased,
                            Incoterms2 = item.IncotermsId,
                            BrokerId = BrokerId,
                            IsSAPDataSync = false,
                            IsSAPDataSyncApproval = false,
                            IsReportingtoAllocation = IsReportingtoAllocation,
                            //DepotIdForRake = depotIdForRake.Value,
                            IsQuantityLimitForBookingSauda = IsQuantityLimitForBookingSauda,
                            SalesOrganizationId = inputDto.SalesOrganizationId,
                            DistributionChannelId = inputDto.DistributionChannelId,
                            DivisionId = inputDto.DivisionId,
                            IsMandatorySku = item.IsMandatorySku,
                            EmployeeSkuDiscountId = item.DiscountId == 0 ? _resultService.GetDiscountId(inputDto, item) : item.DiscountId,
                            QuotedPriceBeforeSAPDiscount = item.BidQuantity == 0 ? 0m : item.BidPrice / item.BidQuantity
                        };
                        var overallSaudaStatuses = Constants.OverallSaudaStatus;
                        var LimitIdsList = new List<long>();
                        requestedQuantityBdo = _resultService.ConvertCasetoMetricTonWithoutDB(item.BidQuantity, item.SkuId, skuUomMappingData);
                        if (delarIsQuantity)
                        {
                            var dealerLimitContext = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().FirstOrDefault(_ => _.UserId == SalesTrader.SalesTrader && _.OilTypeId == item.OilTypeId && _.ParentId == 0 &&
                                _.ValidFrom <= currentDate && _.ValidTo >= currentDate && _.SalesOrganizationId == inputDto.SalesOrganizationId &&
                                _.DistributionChannelId == inputDto.DistributionChannelId && _.DivisionId == inputDto.DivisionId);
                            decimal saudaBidQtydl = 0;
                            if (dealerLimitContext != null)
                            {
                                LimitIdsList.Add(dealerLimitContext.Id);
                                saudaBidQtydl = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.OilTypeId == item.OilTypeId && inputDto.DealerId == _.Sauda.UserId
                                             && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(dealerLimitContext.ValidFrom) /*&& _.CreatedBy == inputDto.DealerId*/ && !_.IsReportingtoAllocation
                                             && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(dealerLimitContext.ValidTo) && overallSaudaStatuses.Contains(_.StatusId) && _.IsQuantityLimitForBookingSauda
                                             && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId
                                             && _.DivisionId == inputDto.DivisionId)
                                             .Select(s => s.BidQuantity).DefaultIfEmpty(0).Sum();
                            }

                            var totalQty = saudaBidQtydl + requestedQuantityBdo;
                            if (dealerLimitContext != null && dealerLimitContext.ActualDiscount >= totalQty)
                            {
                                decimal availableUpdateQuantityBdo = 0;
                                if (saudaBidQtydl == 0)
                                {
                                    availableUpdateQuantityBdo = dealerLimitContext.RemainingQuantity - requestedQuantityBdo;
                                }
                                else
                                {
                                    availableUpdateQuantityBdo = dealerLimitContext.ActualDiscount - totalQty;
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
                        else if (delarNotQuantity)
                        {
                            var nonDealerLimitContext = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().FirstOrDefault(_ => _.UserId == ReportingToUserId && _.OilTypeId == item.OilTypeId && _.ParentId == 0 &&
                                    _.ValidFrom <= currentDate && _.ValidTo >= currentDate && _.SalesOrganizationId == inputDto.SalesOrganizationId &&
                                _.DistributionChannelId == inputDto.DistributionChannelId && _.DivisionId == inputDto.DivisionId);

                            decimal saudaBidQtyNondl = 0;
                            if (nonDealerLimitContext != null)
                            {
                                LimitIdsList.Add(nonDealerLimitContext.Id);
                                saudaBidQtyNondl = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.OilTypeId == item.OilTypeId && inputDto.DealerId == _.Sauda.UserId
                                             && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(nonDealerLimitContext.ValidFrom) /*&& _.CreatedBy == inputDto.DealerId*/ && _.IsReportingtoAllocation
                                             && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(nonDealerLimitContext.ValidTo) && overallSaudaStatuses.Contains(_.StatusId) && _.IsQuantityLimitForBookingSauda
                                             && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId
                                             && _.DivisionId == inputDto.DivisionId)
                                             .Select(s => s.BidQuantity).DefaultIfEmpty(0).Sum();
                            }

                            var totalQty = saudaBidQtyNondl + requestedQuantityBdo;
                            if (nonDealerLimitContext != null && nonDealerLimitContext.ActualDiscount >= totalQty)
                            {
                                decimal availableUpdateQuantityBdo = 0;
                                if (saudaBidQtyNondl == 0)
                                {
                                    availableUpdateQuantityBdo = nonDealerLimitContext.RemainingQuantity - requestedQuantityBdo;
                                }
                                else
                                {
                                    availableUpdateQuantityBdo = nonDealerLimitContext.ActualDiscount - totalQty;
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
        public void SaudaCreateLimitNotificationAsync(long DealerId,long SalesOrganizationId,long DistributionChannelId,long DivisionId, string OilTypeName, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            try
            {
                using (AdaniContext _emamiContext = new AdaniContext())
                {
                    var SalesTrader = (from ucm in _emamiContext.UserCustomerMapping
                                 //join urm in _emamiContext.UserReportingToMappings on ucm.UserId equals urm.UserId
                                 join u in _emamiContext.Users on ucm.UserId equals u.Id
                                 join ud in _emamiContext.UserDivisionMappings on ucm.UserId equals ud.UserId 
                                 join ur in _emamiContext.Users on DealerId equals ur.Id  
                                 where ucm.CustomerId == DealerId && ud.SalesOrganizationId == SalesOrganizationId
                                 && ud.DistributionChannelId == DistributionChannelId
                                 && ud.DivisionId == DivisionId
                                 select new
                                 {
                                     Distributor = ucm.CustomerId,
                                     SalesTrader = ucm.UserId,
                                     Salestrader = u.Name,
                                     UserName = ur.Name,
                                     PushTokenKey = u.PushTokenKey,
                                     RegistrationTypeId = u.RegistrationTypeId,
                                     SaudaLimit = ud.SaudaLimit
                                 }).FirstOrDefault();
                    var Zonetrader = (from ucm in _emamiContext.UserCustomerMapping
                                      join urm in _emamiContext.UserReportingToMappings on ucm.UserId equals urm.UserId
                                      join u in _emamiContext.Users on urm.ReportingToUserId equals u.Id
                                      join ur in _emamiContext.Users on DealerId equals ur.Id
                                      join ud in _emamiContext.UserDivisionMappings on ucm.UserId equals ud.UserId
                                      where ucm.CustomerId == DealerId && ud.SalesOrganizationId == SalesOrganizationId
                                        && ud.DistributionChannelId == DistributionChannelId
                                        && ud.DivisionId == DivisionId
                                      select new
                                      {
                                          PushTokenKey = u.PushTokenKey,
                                          RegistrationTypeId = u.RegistrationTypeId,
                                          Zonetradername = u.Name,
                                          UserName = ur.Name
                                      }).FirstOrDefault();


                    if (SalesTrader.RegistrationTypeId != null && SalesTrader.RegistrationTypeId > 0 && !string.IsNullOrEmpty(SalesTrader.PushTokenKey))
                    {
                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                        {
                            PushTokenKey = SalesTrader.PushTokenKey,
                            RegistrationTypeId = SalesTrader.RegistrationTypeId != null ? (int)SalesTrader.RegistrationTypeId : 0,
                            Title = Constants.Saudalimit.Replace(Constants.CustomerName,SalesTrader.UserName.ToString()).Replace(Constants.OiltypeName, OilTypeName).ToString(),
                            Message = "Sauda Booking Limit Exceeds",
                            //Id = saudaOrderContext.Id,
                        };
                        //_notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                        SendPushNotificationThroughFirebase(pushNotificationInputDto);
                    }

                    if (Zonetrader.RegistrationTypeId != null && Zonetrader.RegistrationTypeId > 0 && !string.IsNullOrEmpty(Zonetrader.PushTokenKey))
                    {
                        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                        {
                            PushTokenKey = Zonetrader.PushTokenKey,
                            RegistrationTypeId = Zonetrader.RegistrationTypeId != null ? (int)Zonetrader.RegistrationTypeId : 0,
                            Title = Constants.Saudalimit.Replace(Constants.CustomerName, Zonetrader.UserName.ToString()).Replace(Constants.OiltypeName, OilTypeName).ToString(),
                            Message = "Sauda Booking Limit Exceeds",
                            //Id = saudaOrderContext.Id,
                        };
                        //_notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                        SendPushNotificationThroughFirebase(pushNotificationInputDto);
                    }
                
                #region Push Notification Nested Method
                void SendPushNotificationThroughFirebase(PushNotificationInputDto pushNotificationInputDto)
                {
                    try
                    {
                        var firebaseSenderId = _emamiContext.Configurations.FirstOrDefault(_ => _.Key == Constants.FirebaseSenderId).Value;
                        var pushNotifyServerkey = _emamiContext.Configurations.FirstOrDefault(_ => _.Key == Constants.PushNotifyServerkey).Value;
                        var pushNotifyUrl = _emamiContext.Configurations.FirstOrDefault(_ => _.Key == Constants.PushNotifyUrl).Value;

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
                }
                #endregion
            }
            catch (Exception ex)
            {
            }
        }


        public void SaudaCreateNotificationAsync(List<SaudaCreateNotificationDto> inputDto, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        {
            // string mStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            long loginUserId = 0;
            try
            {
                using (AdaniContext _context = new AdaniContext())
                {

                    //emailStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",
                    //               CultureInfo.InvariantCulture);
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

                            loginUserId = saudaData.LoginUserId;
                            int bdoRegistratioTypeId = 0;
                            string bdoPushtokenKey = "";
                            var usersContext = _context.Users.AsNoTracking().Where(_ => _.Id == saudaData.LoginUserId || _.Id == saudaData.DealerId);
                            var saudaOrderContext = _context.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == saudaData.SaudaOrderId);
                            var createdBy = usersContext.FirstOrDefault(_ => _.Id == saudaData.LoginUserId);
                            var dealer = usersContext.FirstOrDefault(_ => _.Id == saudaData.DealerId);
                            var userCustomerMapping = _context.UserCustomerMapping.AsNoTracking().FirstOrDefault(_ => _.CustomerId == saudaData.DealerId);
                            if (userCustomerMapping != null)
                            {
                                var bdoContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == userCustomerMapping.UserId);
                                bdoRegistratioTypeId = bdoContext.RegistrationTypeId ?? 0;
                                bdoPushtokenKey = bdoContext.PushTokenKey;
                            }

                            string dealerName = string.Empty;
                            if (usersContext != null && saudaOrderContext != null)
                            {
                                List<string> toUsers = new List<string>();
                                if (!string.IsNullOrEmpty(createdBy.Email))
                                {
                                    toUsers.Add(createdBy.Email);
                                }
                                if (dealer != null && !string.IsNullOrEmpty(dealer.Email))
                                {
                                    dealerName = dealer.Name;
                                    toUsers.Add(dealer.Email);
                                }
                                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();

                                if (isEmail && toUsers != null && toUsers.Any())
                                {
                                    var fromEmail = Constants.FromEmail;
                                    string emailSubject = string.Empty;
                                    var plainText = string.Empty;
                                    Data.Entities.EmailTemplate emailTemplate = new Data.Entities.EmailTemplate();
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
                                            .Replace(Constants.BY_FOR, Constants.BY).Replace(Constants.UserName, createdBy.Name);
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
                                    Data.Entities.EmailTemplate smsTemplate = new Data.Entities.EmailTemplate();
                                    if (saudaData.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess)
                                    {
                                        smsTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationSMS);
                                    }
                                    else
                                    {
                                        if (saudaData.StatusId == (int)DTO.Enums.Status.Pending)
                                        {
                                            smsTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationRAFlowSMS);
                                        }
                                        else if (saudaData.StatusId == (int)DTO.Enums.Status.Hold)
                                        {
                                            smsTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderHoldNotificationSMS);
                                        }
                                        else if (saudaData.StatusId == (int)DTO.Enums.Status.Rejected)
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
                                            StatusId = saudaData.StatusId,
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
                                            .Replace(Constants.BY_FOR, Constants.BY).Replace(Constants.UserName, createdBy.Name);
                                        smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                        if (!string.IsNullOrEmpty(createdBy.MobileNumber))
                                        {
                                            amazonNotificationService.SendMessage(smsMessage, createdBy.MobileNumber, smsTemplate.SMSTemplateID);
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
                                if (createdBy.RegistrationTypeId != null && createdBy.RegistrationTypeId > 0 && !string.IsNullOrEmpty(createdBy.PushTokenKey))
                                {
                                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                    {
                                        PushTokenKey = createdBy.PushTokenKey,
                                        RegistrationTypeId = createdBy.RegistrationTypeId != null ? (int)createdBy.RegistrationTypeId : 0,
                                        Title = Constants.SaudaCreationSubject,
                                        Message = smsPlainTemplate,
                                        //Id = saudaOrderContext.Id,
                                    };
                                    //_notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                    SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                }

                                if (bdoRegistratioTypeId > 0 && !string.IsNullOrEmpty(bdoPushtokenKey))
                                {
                                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                    {
                                        PushTokenKey = bdoPushtokenKey,
                                        RegistrationTypeId = bdoRegistratioTypeId,
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

                    //emailEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff",
                    //              CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
            }
            //string mEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            //TimeSpan timeSpan = Convert.ToDateTime(mEndTime) - Convert.ToDateTime(mStartTime);
            //int mTotalMilliSeconds = (int)timeSpan.TotalMilliseconds;
            //string logData = $"EmailSaudaCreation, StartTime, {mStartTime}, EndTime, {mEndTime}, EmailSendTotalTime, {mTotalMilliSeconds}, LoginUserId, {loginUserId}";
            //string serverFoloderPath = HostingEnvironment.MapPath("~/LogFiles/");
            //string filePath = Path.Combine(serverFoloderPath + "SaudaCreateEmail.txt");
            //File.AppendAllText(filePath, logData + Environment.NewLine);
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
        //                if (skuContext != null && skuContext.VerticalId == (int)DTO.Enums.Vertical.SpecialityFat)
        //                {
        //                    //bool geoErrorFlag = false;
        //                    //bool bdoErrorFlag = false;
        //                    //decimal availableQuantityGeo = 0;
        //                    decimal availableQuantityBdo = 0;
        //                    //var geographicalLimitContext = _emamiContext.SpecalityFatDiscountGeographys.AsNoTracking().FirstOrDefault(_ => _.SkuId == item.SkuId && _.CityId == dealerContext.CityId
        //                    //    && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(currentDate) && DbFunctions.TruncateTime(_.ValidTo) >= DbFunctions.TruncateTime(currentDate));
        //                    //if (geographicalLimitContext != null)
        //                    //{
        //                    //    IQueryable<SaudaOrder> saudaOrdersGeoContext = null;
        //                    //    List<long> dealerList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
        //                    //        .Where(_ => _.u.CityId == dealerContext.CityId && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.u.Id).ToList();
        //                    //    if (dealerList != null && dealerList.Any())
        //                    //    {
        //                    //        saudaOrdersGeoContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.SkuId == item.SkuId && dealerList.Contains(_.Sauda.UserId)
        //                    //              && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(geographicalLimitContext.ValidFrom)
        //                    //              && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(geographicalLimitContext.ValidTo) && overallSaudaStatuses.Contains(_.StatusId));
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
        //                    //        availableQuantityGeo = geographicalLimitContext.ActualDiscount - orderedQuantityGeo;
        //                    //        if (availableQuantityGeo < 0)
        //                    //        {
        //                    //            availableQuantityGeo = 0;
        //                    //        }
        //                    //        //return _resultService.ErrorMessage(Constants.SkuGeographicalLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName));
        //                    //        //availableQuantityGeo = geographicalLimitContext.ActualDiscount - saudaOrdersGeoContext.Sum(_ => _.BidQuantity);
        //                    //        //if (availableQuantityGeo > 0)
        //                    //        //{
        //                    //        //    return _resultService.ErrorMessage(Constants.SkuGeographicalLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, availableQuantityGeo.ToString()));
        //                    //        //}
        //                    //        //else
        //                    //        //{
        //                    //        //    return _resultService.ErrorMessage(Constants.SkuGeographicalLimitReached.Replace(Constants.SkuName, skuContext.SkuName));
        //                    //        //}
        //                    //    }
        //                    //}

        //                    var bdoContext = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.CustomerId == inputDto.DealerId)
        //                                     .Join(_emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.StateTrader), u => u.CustomerId, ur => ur.UserId, (u, ur) => new { u, ur }).ToList();

        //                    var bdoId = bdoContext.FirstOrDefault(_ => _.u.CustomerId == inputDto.DealerId)?.u.UserId;

        //                    SpecalityFatDiscountUser bdoLimitContext = null;
        //                    if (bdoId != null)
        //                    {
        //                        bdoLimitContext = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().FirstOrDefault(_ => _.UserId == bdoId && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
        //                                  && DbFunctions.TruncateTime(_.ValidTo) >= DbFunctions.TruncateTime(currentDate));
        //                        if (bdoLimitContext != null)
        //                        {
        //                            IQueryable<SaudaOrder> saudaOrdersBdoContext = null;
        //                            List<long> dealerList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
        //                                .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.u.Id, uc => uc.CustomerId, (x, uc) => new { DealerId = x.u.Id, x.ur, uc })
        //                                .Where(_ => _.uc.UserId == bdoId && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.DealerId).ToList();
        //                            if (dealerList != null && dealerList.Any())
        //                            {
        //                                saudaOrdersBdoContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.SkuId == item.SkuId && dealerList.Contains(_.Sauda.UserId)
        //                                      && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(bdoLimitContext.ValidFrom)
        //                                      && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(bdoLimitContext.ValidTo) && overallSaudaStatuses.Contains(_.StatusId));
        //                            }
        //                            decimal requestedQuantityBdo = _resultService.ConvertCasetoMetricTon(item.BidQuantity, item.SkuId);
        //                            decimal orderedQuantityBdo = 0;
        //                            decimal totalQuantityBdo = requestedQuantityBdo;
        //                            if (saudaOrdersBdoContext != null && saudaOrdersBdoContext.Any())
        //                            {
        //                                orderedQuantityBdo = saudaOrdersBdoContext.Sum(_ => _.BidQuantity);
        //                                totalQuantityBdo = orderedQuantityBdo + requestedQuantityBdo;
        //                            }
        //                            if (totalQuantityBdo > bdoLimitContext.ActualDiscount)
        //                            {
        //                                //bdoErrorFlag = true;
        //                                availableQuantityBdo = bdoLimitContext.ActualDiscount - orderedQuantityBdo;
        //                                if (availableQuantityBdo < 0)
        //                                {
        //                                    availableQuantityBdo = 0;
        //                                }
        //                                return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, availableQuantityBdo.ToString()));
        //                                //return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName));
        //                                //availableQuantityBdo = bdoLimitContext.ActualDiscount - saudaOrdersBdoContext.Sum(_ => _.BidQuantity);
        //                                //if (availableQuantityBdo > 0)
        //                                //{
        //                                //    return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, availableQuantityBdo.ToString()));
        //                                //}
        //                                //else
        //                                //{
        //                                //    return _resultService.ErrorMessage(Constants.SkuBdoLimitReached.Replace(Constants.SkuName, skuContext.SkuName));
        //                                //}
        //                            }
        //                        }
        //                        else
        //                        {
        //                            return _resultService.ErrorMessage(Constants.BDOLimitNotExists);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        return _resultService.ErrorMessage(Constants.BDONotMapped);
        //                    }
        //                    //if (geographicalLimitContext != null && bdoLimitContext != null && bdoId != null && geoErrorFlag && bdoErrorFlag)
        //                    //{
        //                    //    return _resultService.ErrorMessage(Constants.SkuGeographicalBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.GeoLimitQuantity, Math.Round(availableQuantityGeo, 2).ToString())
        //                    //        .Replace(Constants.BdoLimitQuantity, Math.Round(availableQuantityBdo, 2).ToString()));
        //                    //}
        //                    //else if (((geographicalLimitContext != null) != (bdoId != null && bdoLimitContext != null)) && (geoErrorFlag || bdoErrorFlag))
        //                    //{
        //                    //    if (geographicalLimitContext != null)
        //                    //    {
        //                    //        return _resultService.ErrorMessage(Constants.SkuGeographicalLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, Math.Round(availableQuantityGeo, 2).ToString()));
        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, Math.Round(availableQuantityBdo, 2).ToString()));
        //                    //    }
        //                    //}
        //                }

        //                //var pricingContext = _emamiContext.Pricing.AsNoTracking().FirstOrDefault(_ => _.Id == item.PricingId && DbFunctions.TruncateTime(_.CreatedDate) == DbFunctions.TruncateTime(currentDate));
        //                //if (pricingContext == null)
        //                //{
        //                //    return _resultService.ErrorMessage(Constants.PricingIdisnotValid);
        //                //}

        //                if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
        //                {
        //                    //var TodayBiddingWindowIds = _emamiContext.BiddingWindowTiming.AsNoTracking().Where(_ => _.BiddingDate == DbFunctions.TruncateTime(currentDate) && _.Id == pricingContext.BiddingWindowId).ToList();
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
        //                    var isSKuExistsContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.BiddingwindowId == item.BiddingwindowId && _.SkuId == item.SkuId
        //                        && _.OilTypeId == item.OilTypeId && _.Incoterms2 == item.IncotermsId && _.PlantId == item.PlantId).ToList();
        //                    if (isSKuExistsContext != null && isSKuExistsContext.Count >= CounterBidAllowCount)
        //                    {
        //                        return _resultService.ErrorMessage(Constants.SkuAlreadyBookedinBidding);
        //                    }

        //                    var TodayBiddingIds = _emamiContext.BiddingWindowTiming.AsNoTracking().Where(_ => _.BiddingDate == DbFunctions.TruncateTime(currentDate));
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

        //                if (item.DiscountTypeId == 1)
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

        //                //if (inputDto.SaudaBookingTypeId != (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
        //                //{
        //                try
        //                {
        //                    var usersContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
        //                    var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrder.Id);
        //                    if (usersContext != null && saudaOrderContext != null)
        //                    {
        //                        List<string> toUsers = new List<string>();
        //                        if (!string.IsNullOrEmpty(usersContext.Email))
        //                        {
        //                            toUsers.Add(usersContext.Email);
        //                        }
        //                        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
        //                        if (_resultService.IsEmail() && toUsers != null && toUsers.Any())
        //                        {
        //                            var fromEmail = Constants.FromEmail;
        //                            string emailSubject = string.Empty;
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
        //                                    .Replace(Constants.BY_FOR, Constants.BY).Replace(Constants.UserName, usersContext.Name);
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
        //                                    .Replace(Constants.BY_FOR, Constants.BY).Replace(Constants.UserName, usersContext.Name);
        //                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
        //                                if (!string.IsNullOrEmpty(usersContext.MobileNumber))
        //                                {
        //                                    amazonNotificationService.SendMessage(smsMessage, usersContext.MobileNumber);
        //                                }
        //                            }
        //                        }
        //                        if (_resultService.IsPushNotification() && saudaOrder.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
        //                        {
        //                            if (usersContext.RegistrationTypeId != null && usersContext.RegistrationTypeId > 0 && !string.IsNullOrEmpty(usersContext.PushTokenKey))
        //                            {
        //                                PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                                {
        //                                    PushTokenKey = usersContext.PushTokenKey,
        //                                    RegistrationTypeId = usersContext.RegistrationTypeId != null ? (int)usersContext.RegistrationTypeId : 0,
        //                                    Title = Constants.SaudaCreationSubject,
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
        //                //}
        //            }



        //            //if (inputDto.DealerTypeId == (int)DTO.Enums.DealerType.Broker)
        //            //{
        //            //    List<string> toBroker = new List<string>();
        //            //    var smsMessage = smsTemplate.Template.Replace(Constants.DataResult, saudaContext.Id.ToString());
        //            //    var Broker = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.BrokerId);
        //            //    toBroker.Add(Broker.Email);
        //            //    if (emailTemplate != null)
        //            //    {
        //            //        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, saudaContext.Id.ToString());
        //            //        htmlTemplate = htmlTemplate + "URL";
        //            //        amazonNotificationService.SendEmail(toBroker, emailSubject, plainText, htmlTemplate, true);
        //            //        amazonNotificationService.SendMessage(smsMessage, Broker.MobileNumber);
        //            //    }
        //            //}
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
                var saudaList = _emamiContext.Sauda.AsNoTracking().Where(_ => DbFunctions.TruncateTime(_.BiddingDate) >= DbFunctions.TruncateTime(saudaFilterDto.FromDate) &&
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

                var totalBidAmount = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id).Sum(_ => (decimal?)_.BidPrice) ?? 0;
                var totalBidQuantity = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id).Sum(_ => (decimal?)_.BidQuantityCase) ?? 0;
                var totalBidQuantityInMT = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id).Sum(_ => (decimal?)_.BidQuantity) ?? 0;
                var BrokerContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.SaudaId == saudaContext.Id);

                saudaDetails.SaudaNumber = saudaContext.Id.ToString();
                saudaDetails.SaudaDate = saudaContext.BiddingDate;
                saudaDetails.BiddingDate = saudaContext.BiddingDate;
                saudaDetails.DealerId = saudaContext.UserId;
                saudaDetails.DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaContext.UserId).Name;
                //saudaDetails.Incoterm = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == saudaContext.Incoterms2).Name;
                saudaDetails.TotalAmount = totalBidAmount;
                saudaDetails.TotalQuantity = totalBidQuantity;
                saudaDetails.TotalQuantityInMT = totalBidQuantityInMT;
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
                        DiscountTypeId = order.DiscountTypeId,
                        PlantDepot = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == order.PlantId).Name,
                        //FrieghtRoute = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == order.DealerLocationId).Name,
                        StatusId = order.StatusId,
                        //Status = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == order.StatusId).Name,
                        Status = order.StatusId == (int)DTO.Enums.Status.Pending ? Constants.Accepted : _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == order.StatusId).Name,
                        SaudaNumber = order.SaudaNumber != null ? order.SaudaNumber : string.Empty,
                        BidPricePerCase = order.BidPrice / order.BidQuantityCase
                    };
                    saudaOrders.Add(saudaOrderItem);
                }
                saudaDetails.SaudaOrders = saudaOrders;

                if (saudaOrderListContext != null && saudaOrderListContext.Any())
                {
                    List<long> saudaOrderIds = saudaOrderListContext.Select(s => s.Id).ToList();
                    /*
                     //Dispatch status

                    IQueryable<LiftingRequestDetails> liftingReqOrderContextList = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => saudaOrderIds.Contains(_.SaudaOrderId));
                    if(liftingReqOrderContextList!=null && liftingReqOrderContextList.Any())
                    {
                        var liftingDetailView = new LiftingDetailViewDto();
                        liftingDetailView.CompletedQuantity = liftingReqOrderContextList.Sum(_ => _.LiftingQuantity);
                        liftingDetailView.PendingQuantity = saudaOrderListContext.Sum(_ => _.BidQuantity)- liftingDetailView.CompletedQuantity;
                        liftingDetailView.LiftedSkus = liftingReqOrderContextList.Select(_ => new SaudaOrderDetails
                        {
                            SkuId = _.SkuId,
                            SkuName = _.Sku != null ? _.Sku.SkuName : string.Empty,
                            BidQuantity = _.LiftingQuantity,
                            BidQuantityCases = _.LiftingQuantityCase,
                            BidPricePerCase = Math.Round((_.SaudaOrder!=null && _.SaudaOrder.BidPrice != 0 && _.SaudaOrder.BidQuantityCase != 0 ? (_.SaudaOrder.BidPrice / _.SaudaOrder.BidQuantityCase) : 0), 2),
                            BidPrice = Math.Round((_.SaudaOrder != null && _.SaudaOrder.BidPrice != 0 && _.SaudaOrder.BidQuantityCase != 0 && _.LiftingQuantityCase!=0 ? (_.SaudaOrder.BidPrice / _.SaudaOrder.BidQuantityCase)*_.LiftingQuantityCase : 0), 2),
                            LiftedDate =_.CreatedDate,
                        }).ToList();
                        if(liftingDetailView!=null && liftingDetailView.LiftedSkus!=null && liftingDetailView.LiftedSkus.Any())
                        {
                            saudaDetails.LiftingDetails = liftingDetailView;
                        }
                    }
                    */
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

        public ResultDto GetSaudaShortViewList(LoginUserIdCoversionDto loginUserIdDto)
        {
            _methodName = "GetSaudaShortViewList";
            var saudaOrderListDto = new List<SaudaOrderListDto>();
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (loginUserIdDto.IsConversion)
                {
                    var saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().Join(_emamiContext.Sauda.AsNoTracking(), so => so.SaudaId, s => s.Id, (so, s) => new { so, s })
                        .Join(_emamiContext.Users.AsNoTracking(), sos => sos.s.UserId, u => u.Id, (sos, u) => new { sos.so, sos.s, u })
                        .GroupJoin(_emamiContext.SaudaConversion.AsNoTracking(), sosu => sosu.so.Id, sc => sc.SaudaOrderId, (sosu, sc) => new { sosu.so, sosu.s, sosu.u, sc })
                        .GroupJoin(_emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected)
                        , sosusc => sosusc.so.Id, lr => lr.SaudaOrderId, (sosusc, lr) => new { sosusc.so, sosusc.s, sosusc.u, sosusc.sc, lr })
                        .Where(_ => _.s.UserId == loginUserIdDto.LoginUserId && _.so.StatusId == (int)DTO.Enums.Status.Approved && _.so.SaudaNumber != ""
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
                        .Where(_ => _.s.UserId == loginUserIdDto.LoginUserId && _.so.StatusId == (int)DTO.Enums.Status.Approved && _.so.SaudaNumber != ""
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                SaudaOrder saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SaudaId && _.Sauda != null && _.Sauda.UserId == inputDto.UserId);
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
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

                List<Sauda> saudaContextList = _emamiContext.Sauda.AsNoTracking().Where(_ => _.UserId == saudaFilterDto.UserId && DbFunctions.TruncateTime(_.BiddingDate) >= DbFunctions.TruncateTime(saudaFilterDto.FromDate)
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
                        //    saudaDetailsList.Add(saudaDetails);
                        //}
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }



                //var skuListContext = _emamiContext.SaudaOrders.AsNoTracking().Join(_emamiContext.Sauda.AsNoTracking(), so => so.SaudaId, s => s.Id, (so, s) => new { so, s })
                //    .GroupJoin(_emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking(), sos => sos.so.Id, lr => lr.SaudaOrderId, (sos, lr) => new { sos.so, sos.s, lr })
                //    .Where(_ => _.so.StatusId == (int)DTO.Enums.Status.Approved && _.so.SaudaNumber != "" && _.so.SaudaNumber != null && _.so.OilTypeId == skuInputDto.OilTypeId
                //    && _.s.UserId == skuInputDto.LoginUserId && _.so != null && _.s != null);
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

                //var skuListContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda.UserId == skuInputDto.LoginUserId
                //&& _.OilTypeId == skuInputDto.OilTypeId &&
                //(_.StatusId == (int)DTO.Enums.Status.Approved || _.StatusId == (int)DTO.Enums.Status.Completed)).ToList();

                var skuListContext = _emamiContext.Skus.AsNoTracking().Where(_ => _.OilTypeId == skuInputDto.OilTypeId && _.IsActive).ToList();
                var skuList = new List<NewIndentSkuListDto>();
                if (skuListContext != null && skuListContext.Any())
                {
                    skuList = skuListContext.Select(_ => new NewIndentSkuListDto()
                    {
                        SkuId = _.Id,
                        SkuName = _.SkuName ?? string.Empty
                    }).ToList();
                }

                if (skuList != null && skuList.Any())
                {
                    skuList = skuList.GroupBy(_ => _.SkuId).Select(_ => new NewIndentSkuListDto()
                    {
                        SkuId = _.FirstOrDefault().SkuId,
                        SkuName = _.FirstOrDefault().SkuName,
                        BidQuantityCase = _.Sum(su => su.BidQuantityCase),
                    }).ToList();

                    var SkuContext = _emamiContext.Skus.AsNoTracking();
                    var volumeContext = _emamiContext.VolumeLoadability.AsNoTracking();

                    foreach (var sku in skuList)
                    {
                        var volumeLoadabilityContext = volumeContext.FirstOrDefault(_ => _.SkuId == sku.SkuId);
                        var skuAfterDetection = new NewIndentSkuListDto();
                        skuAfterDetection.SkuId = sku.SkuId;
                        skuAfterDetection.SkuName = sku.SkuName;
                        skuAfterDetection.CaseToMetricTonValue = _resultService.ConvertCasetoMetricTon(1, sku.SkuId);
                        skuAfterDetection.MaxAllowableCasesSingleSku = volumeLoadabilityContext != null ? volumeLoadabilityContext.MaxAllowableSinglesku : 0;
                        skuAfterDetection.MaxAllowableCasesMultipleSku = volumeLoadabilityContext != null ? volumeLoadabilityContext.MaxAllowableMultiplesku : 0;
                        skuAfterDetection.GrossWeight = (SkuContext.FirstOrDefault(_ => _.Id == sku.SkuId) != null) ? SkuContext.FirstOrDefault(_ => _.Id == sku.SkuId).GrossWeight : 0;
                        skuAfterDetectionList.Add(skuAfterDetection);
                        //var liftingRequestListContext = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.SkuId == sku.SkuId && _.LiftingRequest.UserId == skuInputDto.LoginUserId
                        //      && _.LiftingRequest.StatusId == (int)DTO.Enums.Status.Pending);

                        //   var liftingRequestListContext = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => _.SkuId == sku.SkuId
                        //&& _.LiftingRequest.UserId == skuInputDto.LoginUserId
                        //      && _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected);


                        //   if (liftingRequestListContext != null && liftingRequestListContext.Any())
                        //   {
                        //       if (sku.BidQuantityCase - liftingRequestListContext.Sum(_ => _.LiftingQuantityCase) > 0)
                        //       {
                        //           skuAfterDetection.BidQuantityCase = sku.BidQuantityCase - liftingRequestListContext.Sum(_ => _.LiftingQuantityCase);
                        //           skuAfterDetectionList.Add(skuAfterDetection);
                        //       }
                        //   }
                        //   else
                        //   {
                        //       skuAfterDetection.BidQuantityCase = sku.BidQuantityCase;
                        //       skuAfterDetectionList.Add(skuAfterDetection);
                        //   }
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
                saudalimitHistoryDto = _emamiContext.SaudaLimit.AsNoTracking().Where(_ => _.UserId == inputDto.Id).AsNoTracking().Select(c => new SaudaLimitRequestHistoryDto
                {
                    Id = c.Id,
                    LimitRequestNo = c.Id.ToString(),
                    Remarks = c.Remarks,
                    RequestDate = c.CreatedDate,
                    RequestQuantityLimit = c.RequestedLimit,
                    StatusId = c.StatusId,
                    Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == c.StatusId).Name
                }).ToList();

                return SucessResult(saudalimitHistoryDto);
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
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (inputDto.CreatedBy == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
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
                          .FirstOrDefault(_ => _.UserId == inputDto.CreatedBy
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
                    bool isEmail = false;
                    var DealerNotificationContext = _emamiContext.TPNotification.AsNoTracking().
                                                    Join(_emamiContext.TPNotificationDetails.AsNoTracking(), TPN => TPN.Id, TPND => TPND.TPNotificationId, (TPN, TPND) => new { TPN, TPND })
                                                    .Where(_ => _.TPND.DealerId == inputDto.DealerId && _.TPND.NotificationActionId == (int)DTO.Enums.NotificationActionTP.LimitEnhancementRequestCreation && _.TPND.IsActive).ToList();

                    var DealerEmailNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.Email);
                    if (DealerEmailNotificationEnabled != null && DealerEmailNotificationEnabled.Any())
                        isEmail = true;
                    else
                        isEmail = false;

                    var usersContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.CreatedBy);
                    var saudaLimit = _emamiContext.SaudaLimit.AsNoTracking().FirstOrDefault(_ => _.Id == saudalimitContext.Id);
                    if (usersContext != null && saudaLimit != null)
                    {
                        decimal actualLimit = saudaLimit.ActualLimit;
                        decimal extendedLimit = saudaLimit.ActualLimit + saudaLimit.RequestedLimit;
                        List<string> toUsers = new List<string>();
                        if (!string.IsNullOrEmpty(usersContext.Email))
                        {
                            toUsers.Add(usersContext.Email);
                        }
                        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                        if (isEmail && toUsers != null && toUsers.Any())
                        {
                            var fromEmail = Constants.FromEmail;
                            var emailSubject = Constants.SaudaLimitExtensionCreationSubject;
                            var plainText = string.Empty;
                            var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaLimitExtensionCreationEmail);
                            if (emailTemplate != null)
                            {
                                var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.ContractQty, Math.Round(actualLimit, 2).ToString()).Replace(Constants.Quantity, Math.Round(extendedLimit, 2).ToString()).Replace(Constants.CustomerName, usersContext.Name);
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
                            Data.Entities.EmailTemplate smsTemplate = new Data.Entities.EmailTemplate();
                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaLimitExtensionCreationSMS);
                            if (smsTemplate != null)
                            {
                                smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.ContractQty, Math.Round(actualLimit, 2).ToString()).Replace(Constants.Quantity, Math.Round(extendedLimit, 2).ToString()).Replace(Constants.CustomerName, usersContext.Name);
                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                if (!string.IsNullOrEmpty(usersContext.MobileNumber))
                                {
                                    amazonNotificationService.SendMessage(smsMessage, usersContext.MobileNumber);
                                }
                            }
                        }
                        //if (_resultService.IsPushNotification())
                        //{
                        //    if (usersContext.RegistrationTypeId != null && usersContext.RegistrationTypeId > 0 && !string.IsNullOrEmpty(usersContext.PushTokenKey))
                        //    {
                        //        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                        //        {
                        //            PushTokenKey = usersContext.PushTokenKey,
                        //            RegistrationTypeId = usersContext.RegistrationTypeId != null ? (int)usersContext.RegistrationTypeId : 0,
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

                }

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
                _resultService.ErrorMessage(Constants.InvalidRequest);
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
                //var saudaContext = _emamiContext.Sauda.AsNoTracking().Where(_ => _.UserId == IdDto.Id && _.SaudaStatusId != (int)DTO.Enums.SaudaStatus.Processed);
                var saudaContext = _emamiContext.Sauda.AsNoTracking().Where(_ => _.UserId == IdDto.Id);
                if (saudaContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                foreach (var sauda in saudaContext.ToList())
                {
                    var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == sauda.Id);

                    var saudaoutputDto = new SaudaListOutputDto
                    {
                        SaudaId = sauda.Id,
                        SaudaNo = sauda.Id.ToString(),
                        BiddingDate = sauda.BiddingDate,
                        TotalAmt = saudaOrderContext.Sum(_ => _.BidPrice),
                        TotalQty = saudaOrderContext.Sum(_ => _.BidQuantity)
                    };
                    outputDto.Add(saudaoutputDto);
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
                //var saudaContext = _emamiContext.Sauda.AsNoTracking().Where(_ => _.UserId == IdDto.Id && _.SaudaStatusId == (int)DTO.Enums.SaudaStatus.Processed);
                var saudaContext = _emamiContext.Sauda.AsNoTracking().Where(_ => _.UserId == IdDto.Id);
                if (saudaContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                foreach (var sauda in saudaContext.ToList())
                {
                    var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == sauda.Id);

                    if (saudaOrderContext != null)
                    {
                        var saudaoutputDto = new SaudaListOutputDto
                        {
                            SaudaId = sauda.Id,
                            SaudaNo = sauda.Id.ToString(),
                            BiddingDate = sauda.BiddingDate,
                            TotalAmt = saudaOrderContext.Sum(_ => _.BidPrice),
                            TotalQty = saudaOrderContext.Sum(_ => _.BidQuantity)
                        };
                        outputDto.Add(saudaoutputDto);
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
                var errorMessageList = string.Empty;
                var errorFlag = true;
                Data.Entities.User userContext = new Data.Entities.User();
                var incotermsContext = new Data.Entities.IncoTerms();
                var requestTo = 0L;
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
                    var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                    if (userRoleContext == null)
                    {
                        return _resultService.ErrorMessage(Constants.UserNotFound);
                    }
                }

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
                            //        errorMessage = Constants.BindErrorMessage(Constants.DealerLocationNotFound, errorMessage);
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
                            if (specialRateApprovalInputDto.Quantity == 0)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.QuantityEmpty, errorMessage);
                                errorFlag = false;
                            }
                            if (specialRateApprovalInputDto.SpecialPrice == 0)
                            {
                                errorMessage = Constants.BindErrorMessage(Constants.SpecialPriceEmpty, errorMessage);
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

                var userDetail = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.CustomerId == specialRateDto.LoginUserId)
                    .Join(_emamiContext.UserRoles.AsNoTracking()
                    .Where(_ => _.RoleId == (int)DTO.Enums.Role.StateTrader), uc => uc.UserId, ur => ur.UserId, (UserCustomerMapping, UserRoles) => new { UserCustomerMapping })
                    .Select(_ => _.UserCustomerMapping).ToList();
                if (userDetail != null && userDetail.Any())
                {
                    var requestedTo = userDetail.FirstOrDefault()?.UserId;
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
                    var reportingToContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == requestTo);
                    foreach (var specialRateApprovalInputDto in specialRateApprovalInputListDto)
                    {
                        var specialRate = new SpecialRate
                        {
                            UserId = specialRateApprovalInputDto.LoginUserId,
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
                            //FreightRouteId = userContext.FreightRouteId != null ? (long)userContext.FreightRouteId : 0,
                            DepotId = specialRateApprovalInputDto.PlantId,
                            CreatedBy = specialRateApprovalInputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            IsLTD = inputDto.IsLTD,
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

                        if (_resultService.IsPushNotification())
                        {
                            var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == specialRateApprovalInputDto.SkuId);
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
            var specialRateListDto = new List<SpecialRateOutputDto>();
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == specialRateInputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                IQueryable<SpecialRate> specialRateListContext;
                if (specialRateInputDto.OilTypeId != null && specialRateInputDto.FromDate.HasValue && specialRateInputDto.ToDate.HasValue)
                {
                    specialRateListContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => _.UserId == specialRateInputDto.LoginUserId
                            && _.OilTypeId == specialRateInputDto.OilTypeId && _.CreatedDate >= specialRateInputDto.FromDate && _.CreatedDate <= specialRateInputDto.ToDate);
                }
                //else if ((specialRateInputDto.OilTypeId != 0 && specialRateInputDto.OilTypeId != null) || (specialRateInputDto.FromDate.HasValue
                //    && specialRateInputDto.FromDate != DateTime.MinValue) || (specialRateInputDto.ToDate.HasValue && specialRateInputDto.ToDate != DateTime.MinValue))
                //{
                //    return _resultService.ErrorMessage(Constants.RecordNotFound);
                //}
                else
                {
                    specialRateListContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => _.UserId == specialRateInputDto.LoginUserId);
                }
                if (specialRateListContext != null && specialRateListContext.Any())
                {

                    var specialRateList = specialRateListContext.Join(_emamiContext.Users.AsNoTracking(), sr => sr.UserId, u => u.Id, (sr, u) => new { sr, u })
                        .Join(_emamiContext.UserRoles.AsNoTracking(), sru => sru.u.Id, ur => ur.UserId, (sru, ur) => new { sru.sr, sru.u, ur }).Where(_ => _.sr != null && _.u != null && _.ur != null).ToList();
                    foreach (var specialRateContext in specialRateList)
                    {
                        var specialRateOutputDto = new SpecialRateOutputDto();
                        specialRateOutputDto.SpecialRateId = specialRateContext.sr.Id;
                        specialRateOutputDto.DealerId = specialRateContext.sr.UserId;
                        specialRateOutputDto.DealerName = specialRateContext.sr.User != null ? specialRateContext.sr.User.Name : string.Empty;
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
                        specialRateListDto.Add(specialRateOutputDto);
                    }
                    //var specialRateList = specialRateListContext.Join(_emamiContext.Users.AsNoTracking(), sr => sr.UserId, u => u.Id, (sr, u) => new { sr, u })
                    //    .Join(_emamiContext.UserRoles.AsNoTracking(), sru => sru.u.Id, ur => ur.UserId, (sru, ur) => new { sru.sr, sru.u, ur }).Where(_ => _.sr != null && _.u != null && _.ur != null).ToList()
                    //    .GroupBy(_ => new { _.sr.CreatedDate.Date, _.sr.StatusId });
                    //specialRateListDto = specialRateList.Select(_ => new SpecialRateOutputDto
                    //{
                    //    DealerId = _.FirstOrDefault().sr.UserId,
                    //    DealerName = _.FirstOrDefault().sr.User != null ? _.FirstOrDefault().sr.User.Name : string.Empty,
                    //    RequestDate = _.FirstOrDefault().sr.CreatedDate,
                    //    StatusId = _.FirstOrDefault().sr.StatusId,
                    //    StatusName = _.FirstOrDefault().sr.Status != null ? _.FirstOrDefault().sr.Status.Name : string.Empty,
                    //    IsBroker = _.FirstOrDefault().ur.RoleId == (int)DTO.Enums.Role.Broker ? true : false,
                    //    OilTypeList = _.GroupBy(g => g.sr.OilTypeId).Select(s => new SpecialRateOilTypeDto
                    //    {
                    //        OilTypeId = s.FirstOrDefault().sr.OilTypeId,
                    //        OilTypeName = s.FirstOrDefault().sr.OilType != null ? s.FirstOrDefault().sr.OilType.Name : string.Empty,
                    //        SkuCount = s.Count(),
                    //    }).ToList(),
                    //}).ToList();
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
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

                //IQueryable<SpecialRate> specialRateListContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => _.UserId == specialRateDetailInputDto.LoginUserId
                //&& DbFunctions.TruncateTime(_.CreatedDate) == DbFunctions.TruncateTime(specialRateDetailInputDto.RequestDate) && _.StatusId == specialRateDetailInputDto.StatusId);
                IQueryable<SpecialRate> specialRateListContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => _.UserId == specialRateDetailInputDto.LoginUserId
                && _.Id == specialRateDetailInputDto.SpecialRateId);
                if (specialRateListContext != null && specialRateListContext.Any())
                {
                    SpecialRate specialRateDetailContext = specialRateListContext.FirstOrDefault();
                    specialRateDetailsDto.DealerId = specialRateDetailContext.UserId;
                    specialRateDetailsDto.DealerName = specialRateDetailContext.User != null ? specialRateDetailContext.User.Name : string.Empty;
                    specialRateDetailsDto.RequestDate = specialRateDetailContext.CreatedDate;
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
                //var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                //if (userRoleContext == null)
                //{
                //    return _resultService.ErrorMessage(Constants.UserNotFound);
                //}
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

                decimal overallSaudaLimit = 0;
                decimal orderedQuantity = 0;
                decimal liftingQuantity = 0;
                decimal availableQuantity = 0;
                var userdivContext = _emamiContext.UserDivisionMappings.AsNoTracking()
                           .FirstOrDefault(_ => _.UserId == inputDto.LoginUserId
                           && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId
                           && _.DivisionId == inputDto.DivisionId);
                overallSaudaLimit = userdivContext.SaudaLimit ?? 0;

                //IQueryable<SaudaOrder> saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.Sauda.UserId == userContext.Id
                //    && (_.StatusId == (int)DTO.Enums.Status.Pending || _.StatusId == (int)DTO.Enums.Status.Approved));
                IQueryable<SaudaOrder> saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.Sauda.UserId == userContext.Id
                   && (_.SaudaNumber == null) && _.StatusId == (int)DTO.Enums.Status.Pending);

                var QuantityLimitForBookingSaudaName = Utility.GetEnumDescription(DTO.Enums.Configuration.QuantityLimitforBookingSaudaEnabled);
                var configurationContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Name == QuantityLimitForBookingSaudaName);
                bool IsQuantityLimitForBookingSauda = Convert.ToBoolean(configurationContext.Value);
                if (saudaOrderListContext != null && saudaOrderListContext.Any())
                {
                    var overallSaudaStatuses = Constants.OverallSaudaStatus;
                    foreach (var item in specialRateListContext)
                    {

                        var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == item.sr.SkuId);
                        if (skuContext != null && (skuContext.DivisionId == (int)DTO.Enums.Division.SpecialityFat || skuContext.DivisionId == (int)DTO.Enums.Division.Hbc))
                        {
                            //bool geoErrorFlag = false;
                            //bool bdoErrorFlag = false;
                            //decimal availableQuantityGeo = 0;
                            decimal availableQuantityBdo = 0;
                            //var geographicalLimitContext = _emamiContext.SpecalityFatDiscountGeographys.AsNoTracking().FirstOrDefault(_ => _.SkuId == item.SkuId && _.CityId == userContext.CityId
                            //    && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(currentDate) && DbFunctions.TruncateTime(_.ValidTo) >= DbFunctions.TruncateTime(currentDate));
                            //if (geographicalLimitContext != null)
                            //{
                            //    IQueryable<SaudaOrder> saudaOrdersGeoContext = null;
                            //    List<long> dealerList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                            //        .Where(_ => _.u.CityId == userContext.CityId && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.u.Id).ToList();
                            //    if (dealerList != null && dealerList.Any())
                            //    {
                            //        saudaOrdersGeoContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.SkuId == item.SkuId && dealerList.Contains(_.Sauda.UserId)
                            //              && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(geographicalLimitContext.ValidFrom)
                            //              && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(geographicalLimitContext.ValidTo) && overallSaudaStatuses.Contains(_.StatusId));
                            //    }
                            //    decimal requestedQuantityGeo = _resultService.ConvertCasetoMetricTon(item.BidQuantity, item.SkuId);
                            //    decimal orderedQuantityGeo = 0;
                            //    decimal totalQuantityGeo = requestedQuantityGeo;
                            //    if (saudaOrdersGeoContext != null && saudaOrdersGeoContext.Any())
                            //    {
                            //        orderedQuantityGeo = saudaOrdersGeoContext.Sum(_ => _.BidQuantity);
                            //        totalQuantityGeo = requestedQuantityGeo + orderedQuantityGeo;
                            //    }
                            //    if (totalQuantityGeo > geographicalLimitContext.ActualDiscount)
                            //    {
                            //        geoErrorFlag = true;
                            //        availableQuantityGeo = geographicalLimitContext.ActualDiscount - orderedQuantityGeo;
                            //        if (availableQuantityGeo < 0)
                            //        {
                            //            availableQuantityGeo = 0;
                            //        }
                            //        //return _resultService.ErrorMessage(Constants.SkuGeographicalLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName));
                            //        //availableQuantityGeo = geographicalLimitContext.ActualDiscount - saudaOrdersGeoContext.Sum(_ => _.BidQuantity);
                            //        //if (availableQuantityGeo > 0)
                            //        //{
                            //        //    return _resultService.ErrorMessage(Constants.SkuGeographicalLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, availableQuantityGeo.ToString()));
                            //        //}
                            //        //else
                            //        //{
                            //        //    return _resultService.ErrorMessage(Constants.SkuGeographicalLimitReached.Replace(Constants.SkuName, skuContext.SkuName));
                            //        //}
                            //    }
                            //}
                            if (configurationContext != null && IsQuantityLimitForBookingSauda)
                            {
                                var bdoContext = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.CustomerId == inputDto.DealerId)
                                            .Join(_emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.StateTrader), u => u.CustomerId, ur => ur.UserId, (u, ur) => new { u, ur }).ToList();

                                var bdoId = bdoContext.FirstOrDefault(_ => _.u.CustomerId == inputDto.DealerId)?.u.UserId;

                                SpecalityFatDiscountUser bdoLimitContext = null;
                                if (bdoId != null)
                                {
                                    bdoLimitContext = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                                        .FirstOrDefault(_ => _.UserId == bdoId
                                        && _.SkuId == item.sr.SkuId
                                        && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
                                        && DbFunctions.TruncateTime(_.ValidTo) >= DbFunctions.TruncateTime(currentDate));
                                    if (bdoLimitContext != null)
                                    {
                                        IQueryable<SaudaOrder> saudaOrdersBdoContext = null;
                                        List<long> dealerList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                                            .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.u.Id, uc => uc.CustomerId, (x, uc) => new { DealerId = x.u.Id, x.ur, uc })
                                            .Where(_ => _.uc.UserId == bdoId && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.DealerId).ToList();
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
                                            availableQuantityBdo = bdoLimitContext.ActualDiscount - orderedQuantityBdo;
                                            if (availableQuantityBdo < 0)
                                            {
                                                availableQuantityBdo = 0;
                                            }
                                            return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, availableQuantityBdo.ToString()));
                                            //return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName));
                                            //availableQuantityBdo = bdoLimitContext.ActualDiscount - saudaOrdersBdoContext.Sum(_ => _.BidQuantity);
                                            //if (availableQuantityBdo > 0)
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
                                else
                                {
                                    return _resultService.ErrorMessage(Constants.BDONotMapped);
                                }
                            }

                            //if (geographicalLimitContext != null && bdoLimitContext != null && bdoId != null && geoErrorFlag && bdoErrorFlag)
                            //{
                            //    return _resultService.ErrorMessage(Constants.SkuGeographicalBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.GeoLimitQuantity, Math.Round(availableQuantityGeo, 2).ToString())
                            //        .Replace(Constants.BdoLimitQuantity, Math.Round(availableQuantityBdo, 2).ToString()));
                            //}
                            //else if (((geographicalLimitContext != null) != (bdoId != null && bdoLimitContext != null)) && (geoErrorFlag || bdoErrorFlag))
                            //{
                            //    if (geographicalLimitContext != null)
                            //    {
                            //        return _resultService.ErrorMessage(Constants.SkuGeographicalLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, Math.Round(availableQuantityGeo, 2).ToString()));
                            //    }
                            //    else
                            //    {
                            //        return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, Math.Round(availableQuantityBdo, 2).ToString()));
                            //    }
                            //}
                        }
                    }
                    //orderedQuantity = saudaOrderListContext.Sum(_ => _.BidQuantity);
                    //IQueryable<SaudaOrderLiftingRequestMapping> liftingReqOrderContextList = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => saudaOrderListContext.Any(a => a.Id == _.SaudaOrderId));
                    //if (liftingReqOrderContextList != null && liftingReqOrderContextList.Any())
                    //{
                    //    liftingQuantity = liftingReqOrderContextList.Sum(_ => _.LiftingQuantity);
                    //}
                    //availableQuantity = overallSaudaLimit - (orderedQuantity - liftingQuantity);

                    decimal invoiceQuantity = 0;
                    var existingSaudaQuantity = saudaOrderListContext.Sum(_ => _.BidQuantity);
                    var skuIds = saudaOrderListContext.Select(_ => _.SkuId).Distinct().ToList();
                    //var invoiceContext = (from inv in _emamiContext.Invoices.AsNoTracking()
                    //                      join invDet in _emamiContext.InvoiceDetails.AsNoTracking() on inv.Id equals invDet.InvoiceId
                    //                      where inv.UserId == userContext.Id
                    //                      && skuIds.Contains(invDet.SkuId)
                    //                      select invDet
                    //                          ).ToList();

                    //if (invoiceContext != null && invoiceContext.Any())
                    //{
                    //    invoiceQuantity = invoiceContext.Sum(_ => _.ActualBilledQuantity);
                    //}
                    var saudaLimitTableValue = _emamiContext.SaudaLimit.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                    var saudaLimitTableValueTotal = saudaLimitTableValue != null ? (saudaLimitTableValue.PendingContract + saudaLimitTableValue.PendingDO + saudaLimitTableValue.PendingOBD) : 0;
                    availableQuantity = overallSaudaLimit - saudaLimitTableValueTotal - existingSaudaQuantity;

                    if (availableQuantity < specialRateListContext.Sum(_ => _resultService.ConvertCasetoMetricTon(_.srId.QuantityInCases, _.sr.SkuId)))
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
                        UserId = inputDto.LoginUserId,

                        SaudaBookingTypeId = //userContext.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess
                                             // ? 
                        (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                        // : (int)DTO.Enums.SaudaBookingTypes.ReverseAuction,

                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        IsSAPDataSync = false,
                        IsSAPDataSyncApproval = false

                    };

                    _emamiContext.Sauda.Add(saudaContext);
                    _emamiContext.SaveChanges();

                    List<long> saudaOrderIds = new List<long>();
                    int i = 0;
                    foreach (var item in specialRateListContext)
                    {
                        DateTime? saudaValidFromDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        long depotIdForRake = 0;
                        if (item.sr.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake || item.sr.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake)
                        {
                            var depotContext = _emamiContext.Depots.AsNoTracking();
                            depotIdForRake = depotContext.FirstOrDefault(_ => _.Id == item.sr.DepotId && !_.IsPlant) == null ? 0 : depotContext.FirstOrDefault(_ => _.Id == item.sr.DepotId && !_.IsPlant).DepotId;
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
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            PricingId = item.sr.PricingId,
                            // DealerTypeId = (int)DTO.Enums.DealerType.Direct,
                            SaudaBookingTypeId = //userContext.SaudaBookingTypeId == 
                            (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                            //? (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess : (int)DTO.Enums.SaudaBookingTypes.ReverseAuction,

                            Incoterms1 = item.sr.Incoterms1,
                            PlantId = item.sr.DepotId,
                            //DealerLocationId = item.sr.FreightRouteId,
                            // CustomerPONumber = userContext.Code + DateHelper.UtcToIndia(DateTime.UtcNow).ToShortDateString(),
                            ValidFromDate = saudaValidFromDate.Value,
                            ValidToDate = saudaValidFromDate.Value.AddDays(Convert.ToDouble(userContext.SaudaValidityPeriod > 0 ? userContext.SaudaValidityPeriod : Config.DefaultSaudaValidity)),
                            StatusId = (int)DTO.Enums.Status.Pending,
                            // SaudaStatusId = (int)DTO.Enums.SaudaStatus.NotReleased,

                            Incoterms2 = item.sr.Incoterms2,
                            BrokerId = BrokerId,
                            SpecialRateRequestId = item.sr.Id,
                            IsSAPDataSync = false,
                            IsSAPDataSyncApproval = false,
                            // DepotIdForRake = depotIdForRake,
                            IsQuantityLimitForBookingSauda = IsQuantityLimitForBookingSauda,
                            QuotedPriceBeforeSAPDiscount = item.sr.SpecialPrice
                        };
                        _emamiContext.SaudaOrders.Add(saudaOrder);
                        _emamiContext.SaveChanges();

                        //if (userContext.DivisionId == (int)DTO.Enums.LooseVertical.Loose)
                        //{
                        //    saudaOrderIds.Add(saudaOrder.Id);
                        //}

                        try
                        {
                            var usersContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                            var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrder.Id);
                            if (usersContext != null && saudaOrderContext != null)
                            {
                                List<string> toUsers = new List<string>();
                                if (!string.IsNullOrEmpty(usersContext.Email))
                                {
                                    toUsers.Add(usersContext.Email);
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
                                            .Replace(Constants.BY_FOR, Constants.BY).Replace(Constants.UserName, userContext.Name);
                                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                        amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                    }

                                }
                                var smsPlainTemplate = string.Empty;
                                if (_resultService.IsSMS())
                                {
                                    var smsMessage = string.Empty;
                                    Data.Entities.EmailTemplate smsTemplate = new Data.Entities.EmailTemplate();
                                    smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationSMS);
                                    if (smsTemplate != null)
                                    {
                                        smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
                                            .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round((saudaOrderContext.BidPrice / saudaOrderContext.BidQuantityCase), 2)).ToString())
                                            .Replace(Constants.BY_FOR, Constants.BY).Replace(Constants.UserName, userContext.Name);
                                        smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                        if (!string.IsNullOrEmpty(usersContext.MobileNumber))
                                        {
                                            amazonNotificationService.SendMessage(smsMessage, usersContext.MobileNumber);
                                        }
                                    }
                                }
                                //if (_resultService.IsPushNotification())
                                //{
                                //    if (usersContext.RegistrationTypeId != null && usersContext.RegistrationTypeId > 0 && !string.IsNullOrEmpty(usersContext.PushTokenKey))
                                //    {
                                //        PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                //        {
                                //            PushTokenKey = usersContext.PushTokenKey,
                                //            RegistrationTypeId = usersContext.RegistrationTypeId != null ? (int)usersContext.RegistrationTypeId : 0,
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
                    }
                    specialRateListContext.ForEach(_ => _.sr.StatusId = (int)DTO.Enums.Status.Completed);
                    _emamiContext.SaveChanges();

                    //if (userContext.VerticalId == (int)DTO.Enums.LooseVertical.Loose)
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

        #region Credit Limit
        public ResultDto GetTotalCreditLimit(LoginUserIdDto loginUserIdDto)
        {
            var creditLimitTotalDto = new CreditLimitTotalDto();
            _methodName = "GetTotalCreditLimit";
            try
            {
                if (loginUserIdDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                //Sales person
                if (loginUserIdDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == loginUserIdDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userCreditListContext = _emamiContext.UserCreditMaster.AsNoTracking().Where(_ => _.CreatedBy == loginUserIdDto.LoginUserId && _.Isactive).ToList();
                if (userCreditListContext != null && userCreditListContext.Any())
                {
                    creditLimitTotalDto.DealersCount = userCreditListContext.Count();
                    creditLimitTotalDto.TotalCreditLimit = userCreditListContext.Sum(_ => _.CreditLimit);
                    creditLimitTotalDto.TotalCreditExposure = userCreditListContext.Sum(_ => _.CreditExposure);
                }
                if (creditLimitTotalDto != null)
                {
                    return _resultService.SuccessObject(creditLimitTotalDto);
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

        public ResultDto GetCreditLimitList(LoginUserIdDto loginUserIdDto)
        {
            var creditLimitListDto = new List<CreditLimitDto>();
            _methodName = "GetCreditLimitList";
            try
            {
                if (loginUserIdDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                //Sales person
                if (loginUserIdDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == loginUserIdDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userCreditListContext = _emamiContext.UserCreditMaster.AsNoTracking().Where(_ => _.CreatedBy == loginUserIdDto.LoginUserId && _.Isactive).ToList();
                if (userCreditListContext != null && userCreditListContext.Any())
                {
                    creditLimitListDto = userCreditListContext.Select(_ => new CreditLimitDto
                    {
                        DealerId = _.UserId,
                        DealerName = _.User != null ? _.User.Name : string.Empty,
                        CreditLimit = _.CreditLimit,
                        CreditExposure = _.CreditExposure,
                    }).ToList();
                }
                if (creditLimitListDto != null && creditLimitListDto.Any())
                {
                    return _resultService.SuccessObject(creditLimitListDto);
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




        /// <summary>
        /// Method to get sauda list
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetPendingSaudaChartForMobile(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetPendingSaudaChartForMobile";
            var resultDto = new ResultDto();
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var saudaOrdersContext = new List<PendingSaudaChartOutputDto>();
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {
                    var sqlQuery = @"select
                            u.Id as UserId,
                            pc.SaudaQuantity as BidQuantity,
                            (Case when pc.ContractValidFrom is null then Cast('0001-01-01T00:00:00' as datetime2) else pc.ContractValidFrom end) as BiddingDate
                            from PendingContracts pc with(NOLOCK)
                            join Users u on  pc.UserId=u.Id
                            join Skus sku on pc.MaterialCode=sku.SkuCode and pc.SalesOrgId=sku.SalesOrganizationId
                            and pc.DistChnlId=sku.DistributionChannelId and pc.DivisionId=sku.DivisionId
                            where u.Id=@UserId and pc.PendingQuantityInCase > 0.99 ";

                    saudaOrdersContext = conn.Query<PendingSaudaChartOutputDto>(sqlQuery, new
                    {
                        UserId = loginUserIdDto.LoginUserId
                    }).ToList();

                }
                //var saudaOrdersContext = (from pct in _emamiContext.PendingContracts.AsNoTracking()
                //                          where pct.PendingQuantityInCase != 0 select pct into pc
                //                          join u in _emamiContext.Users.AsNoTracking() on pc.UserId equals u.Id
                //                          join sku in _emamiContext.Skus.AsNoTracking() on pc.MaterialCode equals sku.SkuCode where pc.SalesOrgId == sku.SalesOrganizationId && pc.DistChnlId == sku.DistributionChannelId && pc.DivisionId == sku.DivisionId
                //                          //join sauda in _emamiContext.Sauda.AsNoTracking() on pc.SaudaNumber equals sauda.SaudaNumber /*into saudadb from sd in saudadb.DefaultIfEmpty()*/
                //                          where  u.Id == loginUserIdDto.LoginUserId
                //                           //&& DbFunctions.TruncateTime(pc.ContractValidTo) >= DbFunctions.TruncateTime(currentDate)
                //                          select new PendingSaudaChartOutputDto()
                //                          { UserId = u.Id,
                //                              BidQuantity = pc.SaudaQuantity,
                //                              BiddingDate = (_emamiContext.Sauda.AsQueryable().FirstOrDefault(_ => _.SaudaNumber == pc.SaudaNumber) != null ? _emamiContext.Sauda.AsQueryable().FirstOrDefault(_ => _.SaudaNumber == pc.SaudaNumber).BiddingDate : DateTime.MinValue) }).ToList();

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
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetPendingSaudaChartDetailForMobile(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetPendingSaudaChartDetailForMobile";
            var saudaListDto = new List<PendingContractOutputDtoDealer>();
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var city = _emamiContext.City.AsQueryable();
                var saudaContext = _emamiContext.Sauda.AsQueryable();

                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {
                    //var sqlQuery = @"select 
                    //            p.SaudaQuantity as BidQuantity,
                    //            u.Id as UserId,
                    //            p.SalesOrgId as SalesOrganizationId,
                    //            p.DistChnlId as DistributionChannelId,
                    //            p.DivisionId as DivisionId
                    //            from PendingContracts p with(NOLOCK)
                    //            join Users u on p.PendingQuantityInCase!=0 and p.UserId=u.Id";
                    var sqlQuery = @"select 
                                    pc.Id,
                                    (Case when s.Id is null then 0 else s.Id end) as SaudaOrderId,
                                    u.Id as UserId,
                                    u.Name as [User],
                                    (Case when c.CityName is null then '' else c.CityName end) as City,
                                    (Case when pc.ContractValidFrom is null then Cast('0001-01-01T00:00:00' as datetime2) else pc.ContractValidFrom end) as BiddingDate,
                                    pc.TotalValue as TotalBidPrice,
                                    pc.SaudaQuantity as TotalBidQuantity,
                                    (o.Name+'-'+sorg.Code+'/'+dist.Code+'/'+div.Code) as OiltypeName,
                                    sku.OilTypeId,
                                    pc.SaudaNumber
                                    from PendingContracts pc with(NOLOCK)
                                    join Users u on pc.UserId=u.Id
                                    left join Cities c on u.CityId=c.Id
                                    left join Saudas s with(NOLOCK) on pc.SaudaNumber=s.SaudaNumber
                                    join Skus sku on pc.MaterialCode=sku.SkuCode and pc.SalesOrgId=sku.SalesOrganizationId
                                    left join OilTypes o on sku.OilTypeId=o.Id
                                    join SalesOrganizations sorg on o.SalesOrganizationId=sorg.Id
                                    join DistributionChannels dist on o.DistributionChannelId=dist.Id
                                    join Divisions div on o.DivisionId=div.Id
                                    and pc.DistChnlId=sku.DistributionChannelId and pc.DivisionId=sku.DivisionId
                                    where u.Id=@Userid and pc.PendingQuantityInCase > 0.99
                                    order by pc.Id desc";
                    var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    saudaListDto = conn.Query<PendingContractOutputDtoDealer>(sqlQuery, new
                    {
                        UserId = loginUserIdDto.LoginUserId
                    }).ToList();

                }

                //var saudaOrdersContext = _emamiContext.PendingContracts.AsNoTracking().Where(_ => _.PendingQuantityInCase!=0)
                //    //.Join(_emamiContext.Sauda.AsNoTracking(), so => so.SaudaNumber, s => s.SaudaNumber, (so, s) => new { x=so, s })
                //    .Join(_emamiContext.Users.AsNoTracking(), x => x.UserId, u => u.Id, (x, u) => new { x = x, u })
                //    //.Join(_emamiContext.City.AsNoTracking(), x => x.u.CityId, c => c.Id, (x, c) => new { x.x, DealerName = x.u.Name, CityName = c.CityName,sauda=x.sauda, DealerId = x.u.Id/*, VerticalId = x.u.DivisionId*/ })
                //    .Join(_emamiContext.Skus.AsNoTracking(), s => s.x.MaterialCode, ss => ss.SkuCode, (s, ss) => new { s.x, ss, DealerName=s.u.Name, DealerId=s.u.Id,CityId=s.u.CityId/*, s.VerticalId*/ })
                //    .Where(_ => _.DealerId == loginUserIdDto.LoginUserId
                //     //&& DbFunctions.TruncateTime(_.x.ContractValidTo) >= DbFunctions.TruncateTime(currentDate)
                //    && _.x.SalesOrgId == _.ss.SalesOrganizationId && _.x.DistChnlId == _.ss.DistributionChannelId
                //       && _.x.DivisionId == _.ss.DivisionId 
                //    //&& _.ss.DivisionId == _.VerticalId
                //    ).Select(s => new {
                //        Id = s.x.Id,
                //        SaudaOrderId = saudaContext.FirstOrDefault(_ => _.SaudaNumber == s.x.SaudaNumber) != null ? saudaContext.FirstOrDefault(_ => _.SaudaNumber == s.x.SaudaNumber).Id : 0,
                //        UserId = s.DealerId,
                //        User = s.DealerName,
                //        City = city.FirstOrDefault(_ => _.Id==s.CityId) !=null ? city.FirstOrDefault(_ => _.Id==s.CityId).CityName:String.Empty,
                //        BiddingDate = saudaContext.FirstOrDefault(_ => _.SaudaNumber == s.x.SaudaNumber)  != null ? saudaContext.FirstOrDefault(_ => _.SaudaNumber == s.x.SaudaNumber).BiddingDate : DateTime.MinValue,
                //        TotalBidPrice = s.x.TotalValue,
                //        TotalBidQuantity = s.x.SaudaQuantity,
                //        OiltypeName = s.ss.OilType.Name+"-"+ s.ss.OilType.SalesOrganization.Code+"/"+ s.ss.OilType.DistributionChannel.Code+"/"+ s.ss.OilType.Division.Code,
                //        OilTypeId = s.ss.OilType.Id,
                //    }).ToList();

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
                                TotalQty = sauda.TotalBidQuantity,
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
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }


        public ResultDto GetBookedSauda(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetPendingSaudaChartDetailForMobile";
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

                var saudaContext = _emamiContext.Sauda.AsNoTracking().Where(_ => _.UserId == loginUserIdDto.LoginUserId &&
                DbFunctions.TruncateTime(loginUserIdDto.FromDate) <= DbFunctions.TruncateTime(_.BiddingDate) && DbFunctions.TruncateTime(loginUserIdDto.ToDate) >= DbFunctions.TruncateTime(_.BiddingDate)).ToList();

                if (saudaContext != null && saudaContext.IsAny())
                {
                    // Prepare approval mapping: latest approval per SaudaId -> approver user name
                    var saudaIds = saudaContext.Select(s => s.Id).Distinct().ToList();
                    var latestApprovals = _emamiContext.SaudaApproval.AsNoTracking()
                        .Where(a => saudaIds.Contains(a.SaudaId))
                        .GroupBy(a => a.SaudaId)
                        .Select(g => g.OrderByDescending(x => x.Id).FirstOrDefault())
                        .ToList();

                    var approvalDict = latestApprovals.Where(a => a != null)
                                                      .ToDictionary(a => a.SaudaId, a => a);

                    var approverIds = latestApprovals.Where(a => a != null && a.RequestedTo > 0)
                                                    .Select(a => a.RequestedTo)
                                                    .Distinct()
                                                    .ToList();

                    var approverNames = new Dictionary<long, string>();
                    if (approverIds.Any())
                    {
                        approverNames = _emamiContext.Users.AsNoTracking()
                                         .Where(u => approverIds.Contains(u.Id))
                                         .ToDictionary(u => u.Id, u => u.Name);
                    }

                    foreach (var sauda in saudaContext)
                    {
                        var SaudaDetailContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == sauda.Id).ToList();
                        var Dealer = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == sauda.UserId);
                        var saudaDto = new BookedSaudaDto
                        {
                            SaudaId = sauda.Id,
                            DealerId = sauda.UserId,
                            Dealer = Dealer != null ? Dealer.Name : string.Empty,
                            SaudaBookedDate = sauda.BiddingDate,
                            SaudaNumber = sauda.SaudaNumber != null ? sauda.SaudaNumber : String.Empty,
                            StatusId = sauda.StatusId,
                            Status = !SaudaDetailContext.IsAny() ? String.Empty : SaudaDetailContext.FirstOrDefault().StatusId == (int)DTO.Enums.Status.Pending ? UtilityHelper.GetEnumDescription(DTO.Enums.Status.Pending) : SaudaDetailContext.FirstOrDefault().StatusId == (int)DTO.Enums.Status.Approved ? UtilityHelper.GetEnumDescription(DTO.Enums.Status.Approved) : SaudaDetailContext.FirstOrDefault().StatusId == (int)DTO.Enums.Status.Completed ? UtilityHelper.GetEnumDescription(DTO.Enums.Status.Completed) : SaudaDetailContext.FirstOrDefault().StatusId == (int)DTO.Enums.Status.Rejected ? UtilityHelper.GetEnumDescription(DTO.Enums.Status.Rejected) : "",
                            TotalQuantity = SaudaDetailContext.IsAny() ? SaudaDetailContext.Sum(_ => _.BidQuantity) : 0,
                        };

                        // attach approval info (ApprovalUser, IsApprovalView) using latest approval entry
                        if (approvalDict.TryGetValue(sauda.Id, out var approval) && approval != null)
                        {
                            saudaDto.ApprovalUser = (approval.RequestedTo > 0 && approverNames.ContainsKey(approval.RequestedTo)) ? approverNames[approval.RequestedTo] : string.Empty;
                        }
                        else
                        {
                            saudaDto.ApprovalUser = string.Empty;
                        }

                        var results = SaudaDetailContext.GroupBy(
                            p => p.Sku.OilTypeId,
                            p => p.SkuId,
                            (key, g) => new { OilTypeId = key, Skus = g.ToList() }).ToList();

                        foreach (var detail in results)
                        {
                            var DetailDto = new BookedSaudaDetailDto
                            {
                                OilTypeId = (long)detail.OilTypeId,
                                OilType = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == detail.OilTypeId)?.Name ?? string.Empty,
                                SkuCount = detail.Skus.Count
                            };
                            saudaDto.BookedSaudaDetailDto.Add(DetailDto);
                        }
                        saudaListDto.Add(saudaDto);
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

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.UserId);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                var saudaOrderListContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SaudaOrderId);
                if (saudaOrderListContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                var saudaContext = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderListContext.SaudaId);
                var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == saudaOrderListContext.SaudaId);
                var totalBidAmount = saudaOrderContext.Where(_ => _.SaudaId == saudaContext.Id).Sum(_ => (decimal?)_.BidPrice) ?? 0;
                var totalBidQuantity = saudaOrderContext.Where(_ => _.SaudaId == saudaContext.Id).Sum(_ => (decimal?)_.BidQuantity) ?? 0;
                var BrokerContext = saudaOrderContext.FirstOrDefault(_ => _.SaudaId == saudaContext.Id);

                saudaDetails.SaudaNumber = saudaContext.Id.ToString();
                saudaDetails.SaudaDate = saudaContext.BiddingDate;
                saudaDetails.DealerId = saudaContext.UserId;
                saudaDetails.DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaContext.UserId).Name;
                saudaDetails.TotalAmount = totalBidAmount;
                saudaDetails.TotalQuantity = totalBidQuantity;
                saudaDetails.SaudaValidityDays = Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity;
                saudaDetails.SaudaExpireDays = (DateHelper.UtcToIndia(DateTime.UtcNow) - saudaContext.BiddingDate).Days;
                saudaDetails.ExpiryDate = BrokerContext.ValidToDate;
                saudaDetails.BrokerId = BrokerContext.BrokerId;
                if (BrokerContext != null)
                {
                    saudaDetails.BrokerName = BrokerContext.BrokerId != 0 ? _emamiContext.Users.FirstOrDefault(_ => _.Id == BrokerContext.BrokerId).Name : string.Empty;
                }

                var saudaOrders = new List<SaudaOrderDetails>();

                var saudaOrderItem = new SaudaOrderDetails
                {
                    SkuId = saudaOrderListContext.SkuId,
                    SkuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderListContext.SkuId).SkuName,
                    BidPrice = saudaOrderListContext.BidPrice,
                    BidQuantity = saudaOrderListContext.BidQuantity,
                    BidQuantityCases = saudaOrderListContext.BidQuantityCase,
                    IncoTerms = saudaOrderListContext.Incoterms1,
                    Discount = saudaOrderListContext.DiscountAmount,
                    PlantDepot = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderListContext.PlantId).Name,
                    //FrieghtRoute = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderListContext.DealerLocationId).Name,
                    DiscountTypeId = saudaOrderListContext.DiscountTypeId,
                    StatusId = saudaOrderListContext.StatusId,
                    Status = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderListContext.StatusId).Name,
                    SaudaConversionId = _emamiContext.SaudaConversionOrder.AsNoTracking().FirstOrDefault(_ => _.SaudaId == saudaOrderListContext.Id) != null ? _emamiContext.SaudaConversionOrder.AsNoTracking().FirstOrDefault(_ => _.SaudaId == saudaOrderListContext.Id).SaudaConversionId : 0
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
                        //if (ReturnsInvoiceContext != null && ReturnsInvoiceContext.Any())
                        //{
                        //    InvoiceBilledQuantity = ReturnsInvoiceContext.Sum(_ => _.ActualBilledQuantity);
                        //}

                        var liftingDetailView = new LiftingDetailViewDto();
                        liftingDetailView.CompletedQuantity = liftingReqOrderContextList.Sum(_ => _.LiftingQuantity);
                        liftingDetailView.PendingQuantity = saudaOrderListContext.BidQuantity - liftingDetailView.CompletedQuantity /*+ InvoiceBilledQuantity*/;
                        liftingDetailView.PendingQuantityCase = saudaOrderListContext.BidQuantityCase - liftingReqOrderContextList.Sum(_ => _.LiftingQuantityCase);
                        liftingDetailView.LiftedSkus = liftingReqOrderContextList.Join(_emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Id == inputDto.SaudaOrderId),
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
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
                            if (saudaConversionContext != null && saudaConversionContext.SaudaOrder != null && saudaConversionContext.SaudaOrder.Sku != null)
                            {
                                oldSku = saudaConversionContext.SaudaOrder.Sku.SkuName;
                            }

                            var usersContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaConversionAddDto.LoginUserId);
                            if (usersContext != null && saudaOrderContext != null)
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
                                if (!string.IsNullOrEmpty(usersContext.Email))
                                {
                                    toUsers.Add(usersContext.Email);
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
                                        var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuOld, oldSku).Replace(Constants.SkuNew, newSku).Replace(Constants.CustomerName, usersContext.Name);
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
                                    Data.Entities.EmailTemplate smsTemplate = new Data.Entities.EmailTemplate();
                                    smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaConversionRequestSMS);
                                    if (smsTemplate != null)
                                    {
                                        smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuOld, oldSku).Replace(Constants.SkuNew, newSku).Replace(Constants.CustomerName, usersContext.Name);
                                        smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                        if (!string.IsNullOrEmpty(usersContext.MobileNumber))
                                        {
                                            amazonNotificationService.SendMessage(smsMessage, usersContext.MobileNumber);
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                IQueryable<SaudaConversionOrder> saudaConvOrderListContext = _emamiContext.SaudaConversionOrder.AsNoTracking().Where(_ => _.SaudaConversion != null && _.SaudaConversion.DealerId == saudaFilterDto.UserId);
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var saudaConversionContext = _emamiContext.SaudaConversion.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SaudaConversionId);
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
                        IQueryable<SaudaOrder> saudaOrderContextList = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Id == saudaConversionContext.SaudaOrderId);
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
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
                        var usersContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaExtensionAddDto.LoginUserId);
                        if (usersContext != null && saudaOrderContext != null)
                        {
                            List<string> toUsers = new List<string>();
                            if (!string.IsNullOrEmpty(usersContext.Email))
                            {
                                toUsers.Add(usersContext.Email);
                            }
                            bool isEmail = false;
                            var DealerNotificationContext = _emamiContext.TPNotification.AsNoTracking().
                                                            Join(_emamiContext.TPNotificationDetails.AsNoTracking(), TPN => TPN.Id, TPND => TPND.TPNotificationId, (TPN, TPND) => new { TPN, TPND })
                                                            .Where(_ => _.TPND.DealerId == saudaOrderContext.Sauda.UserId && _.TPND.NotificationActionId == (int)DTO.Enums.NotificationActionTP.SaudaExtensionRequest && _.TPND.IsActive).ToList();

                            var DealerEmailNotificationEnabled = DealerNotificationContext.Where(_ => _.TPN.Email);
                            if (DealerEmailNotificationEnabled != null && DealerEmailNotificationEnabled.Any())
                                isEmail = true;
                            else
                                isEmail = false;
                            AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                            if (isEmail && toUsers != null && toUsers.Any())
                            {
                                var fromEmail = Constants.FromEmail;
                                var emailSubject = Constants.SaudaExtensionRequestSubject;
                                var plainText = string.Empty;
                                var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaExtensionRequestNotificationEmail);
                                if (emailTemplate != null)
                                {
                                    var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.NoOfDays, noOfDays.ToString());
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
                                Data.Entities.EmailTemplate smsTemplate = new Data.Entities.EmailTemplate();
                                smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaExtensionRequestNotificationSMS);
                                if (smsTemplate != null)
                                {
                                    smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.NoOfDays, noOfDays.ToString());
                                    smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                    if (!string.IsNullOrEmpty(usersContext.MobileNumber))
                                    {
                                        try
                                        {
                                            amazonNotificationService.SendMessage(smsMessage, usersContext.MobileNumber, smsTemplate.SMSTemplateID);
                                        }
                                        catch (Exception ex)
                                        {

                                        }
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
        public ResultDto NewAddSaudaExtension(SaudaExtensionNewAddDto saudaExtensionAddDto)
        {
            _methodName = "NewAddSaudaExtension";
            try
            {
                var saudaExtensionApprovalList = new List<SaudaExtensionDetailsApproval>();
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

                var saudaPendingContracts = (from s in _emamiContext.PendingContracts.AsNoTracking().ToList()
                                             join se in saudaExtensionAddDto.SaudaExtensionList on s.SaudaNumber equals se.SaudaNumber
                                             select new
                                             {
                                                 s.SaudaOrderId,
                                                 PendingContractId = s.Id,
                                                 s.SaudaNumber,
                                                 SaudaValidFrom = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.SaudaNumber == s.SaudaNumber) != null ? _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.SaudaNumber == s.SaudaNumber).BiddingDate : DateTime.MinValue,
                                                 SaudaValidTo = s.ContractValidTo,
                                                 BasicRate = s.BasicRate,
                                                 s.SaudaQuantity,
                                                 SkuCode = s.MaterialCode,
                                                 UserCode = s.CustomerCode,
                                                 PendingQuantityInMT = s.SaudaQuantity,
                                                 s.PendingQuantityInCase,
                                                 s.MaterialCode,
                                                 s.SalesOrgId,
                                                 s.DistChnlId,
                                                 s.DivisionId
                                             }).ToList();

                if (saudaExtensionAddDto.SaudaExtensionList != null && saudaExtensionAddDto.SaudaExtensionList.Any())
                {
                    foreach (var sauda in saudaExtensionAddDto.SaudaExtensionList)
                    {
                        decimal quantityMT = 0;
                        var saudaDetailsContext = saudaPendingContracts.ToList();
                        foreach (var saudaDetails in saudaDetailsContext)
                        {
                            if (saudaDetails != null)
                            {
                                //var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.SkuCode == saudaDetails.SkuCode
                                //&& saudaDetails.SalesOrgId == _.SalesOrganizationId && saudaDetails.DistChnlId == _.DistributionChannelId
                                //&& saudaDetails.DivisionId == _.DivisionId);

                                //if (skuContext != null)
                                //{
                                //    quantityMT = _resultService.ConvertCasetoMetricTon(saudaDetails.SaudaQuantity, skuContext.Id);
                                //}
                                var saudaExtensionApproval = new SaudaExtensionDetailsApproval
                                {
                                    SaudaNumber = saudaDetails.SaudaNumber,
                                    RequestDate = sauda.RequestDate,
                                    ExtentionDateCount = sauda.ExtentionDateCount,
                                    PendingContractId = sauda.PendingContractId,
                                    SaudaOrderId = saudaDetails.SaudaOrderId,
                                    IsApproval = true,
                                    CreatedBy = userContext.Id,
                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                    BasicRate = saudaDetails.BasicRate,
                                    SkuCode = saudaDetails.SkuCode,
                                    UserCode = saudaDetails.UserCode,
                                    SaudaValidFrom = saudaDetails.SaudaValidFrom,
                                    SaudaValidTo = saudaDetails.SaudaValidTo.Value,
                                    SaudaQuantityMT = saudaDetails.SaudaQuantity,
                                    PendingQuantityCase = saudaDetails.PendingQuantityInCase,
                                    Remarks = saudaExtensionAddDto.Remarks,
                                    SaudaRequestDate = Convert.ToDateTime(sauda.RequestDate)
                                };
                                if (saudaExtensionApproval != null)
                                {
                                    saudaExtensionApprovalList.Add(saudaExtensionApproval);
                                }

                                var saudaId = _emamiContext.Sauda.FirstOrDefault(_ => _.SaudaNumber == saudaDetails.SaudaNumber).Id;
                                var saudaOrderContextList = _emamiContext.SaudaOrders.Where(_ => _.SaudaId == saudaId).ToList();
                                if (saudaOrderContextList != null)
                                {
                                    foreach (var item in saudaOrderContextList)
                                    {
                                        var saudaOrderContext = _emamiContext.SaudaOrders.FirstOrDefault(_ => _.Id == item.Id);
                                        saudaOrderContext.ValidToDate = Convert.ToDateTime(sauda.RequestDate);
                                        saudaOrderContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                        saudaOrderContext.ModifiedBy = saudaExtensionAddDto.LoginUserId;
                                        _emamiContext.SaveChanges();
                                    }

                                    var UserId = _emamiContext.Sauda.FirstOrDefault(_ => _.SaudaNumber == saudaDetails.SaudaNumber).UserId;
                                    var dealer = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == UserId);
                                    if (_resultService.IsPushNotification())
                                    {
                                        if (dealer != null && dealer.RegistrationTypeId != null && dealer.RegistrationTypeId > 0 && !string.IsNullOrEmpty(dealer.PushTokenKey))
                                        {
                                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                            {
                                                PushTokenKey = dealer.PushTokenKey,
                                                RegistrationTypeId = dealer.RegistrationTypeId != null ? (int)dealer.RegistrationTypeId : 0,
                                                Title = Constants.SaudaExtensionSubject,
                                                Message = Constants.SaudaExtensionMessage + saudaDetails.SaudaNumber + "for ValidTo date " + Convert.ToDateTime(sauda.RequestDate),
                                            };
                                            _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                        }
                                    }
                                }

                            }
                        }
                        // var saudaNumber = string.Concat(saudaDetails.SaudaNumber, "_", saudaDetails.MaterialCode);

                    }

                    if (null != saudaExtensionApprovalList && saudaExtensionApprovalList.Any())
                    {
                        _emamiContext.BulkInsertProxy(saudaExtensionApprovalList);

                        List<string> SaudaNumbers = new List<string>();
                        foreach (var extendedSauda in saudaExtensionApprovalList)
                        {
                            var pendingContractContextList = _emamiContext.PendingContracts.Where(_ => _.SaudaNumber == extendedSauda.SaudaNumber).ToList();

                            if (pendingContractContextList != null)
                            {
                                foreach (var item in pendingContractContextList)
                                {
                                    var pendingContractContext = _emamiContext.PendingContracts.FirstOrDefault(_ => _.Id == item.Id);
                                    pendingContractContext.IsSaudaExtended = true;
                                }
                            }


                            //var saudaNumber = string.Concat(pendingContractContext.SaudaNumber, "_", pendingContractContext.MaterialCode);
                            SaudaNumbers.Add(extendedSauda.SaudaNumber);
                        }
                        _emamiContext.SaveChanges();

                        var saudaIds = _emamiContext.Sauda.AsNoTracking().Where(_ => SaudaNumbers.Contains(_.SaudaNumber)).Select(_ => _.Id).Distinct().ToList();

                        //empty values passed
                        bool IsReprocess = false;

                        //if (ConsoleSettings.IsInboundDirectSyncToSapAllowed)
                        //{
                        HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                        {
                            _sapIntegrationService.GetSaudaDetails(saudaIds, false);
                        });

                        //}
                    }
                }
                return _resultService.SuccessMessage(Constants.SaudaExtensionSuccess);
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var saudaConversionListContext = _emamiContext.SaudaConversion.AsNoTracking()
                    .Join(_emamiContext.SaudaOrders.AsNoTracking(), sc => sc.SaudaOrderId, so => so.Id, (sc, so) => new { sc, so })
                    .Where(_ => _.sc.DealerId == saudaFilterDto.UserId && _.sc.IsExtension == true);
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
        //        var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
        //        if (userRoleContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserNotFound);
        //        }
        //        SaudaOrder saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SaudaOrderId && _.Sauda != null && _.Sauda.UserId == inputDto.UserId);
        //        if (saudaOrderContext != null)
        //        {
        //            if (saudaOrderContext.Sauda != null)
        //            {
        //                var dealerContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == saudaOrderContext.Sauda.UserId);
        //                if (dealerContext != null)
        //                {
        //                    saudaOrderDetails.DealerId = dealerContext.Id;
        //                    saudaOrderDetails.DealerName = dealerContext.Name;
        //                }
        //                saudaOrderDetails.BookedDate = saudaOrderContext.Sauda.BiddingDate;
        //            }
        //            saudaOrderDetails.SaudaId = saudaOrderContext.Id;
        //            saudaOrderDetails.SaudaOrderId = saudaOrderContext.Id;
        //            saudaOrderDetails.SaudaNumber = saudaOrderContext.SaudaNumber;
        //            saudaOrderDetails.ValidToDate = saudaOrderContext.ValidToDate;
        //            saudaOrderDetails.OilTypeId = saudaOrderContext.OilTypeId;
        //            saudaOrderDetails.OilTypeName = saudaOrderContext.OilType != null ? saudaOrderContext.OilType.Name : string.Empty;
        //            saudaOrderDetails.SkuId = saudaOrderContext.SkuId;
        //            saudaOrderDetails.SkuName = saudaOrderContext.Sku != null ? saudaOrderContext.Sku.SkuName : string.Empty;
        //            saudaOrderDetails.StatusId = saudaOrderContext.StatusId;
        //            var statusContext = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.StatusId);
        //            if (statusContext != null)
        //            {
        //                saudaOrderDetails.Status = statusContext.Name;
        //            }
        //            IQueryable<SaudaOrderLiftingRequestMapping> liftingReqOrderContextList = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => _.SaudaOrderId == saudaOrderContext.Id
        //                && _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected);
        //            if (liftingReqOrderContextList != null && liftingReqOrderContextList.Any())
        //            {
        //                saudaOrderDetails.BidQuantity = saudaOrderContext.BidQuantity - liftingReqOrderContextList.Sum(_ => _.LiftingQuantity);
        //                saudaOrderDetails.BidQuantityCases = saudaOrderContext.BidQuantityCase - liftingReqOrderContextList.Sum(_ => _.LiftingQuantityCase);
        //            }
        //            else
        //            {
        //                saudaOrderDetails.BidQuantity = saudaOrderContext.BidQuantity;
        //                saudaOrderDetails.BidQuantityCases = saudaOrderContext.BidQuantityCase;
        //            }
        //            saudaOrderDetails.BidPrice = saudaOrderContext.BidPrice;
        //            saudaOrderDetails.BidPricePerCase = Math.Round((saudaOrderContext.BidPrice != 0 && saudaOrderContext.BidQuantityCase != 0 ? (saudaOrderContext.BidPrice / saudaOrderContext.BidQuantityCase) : 0), 2);
        //            saudaOrderDetails.IncoTerms = saudaOrderContext.Incoterms1;
        //            var plantContext = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.PlantId);
        //            if (plantContext != null)
        //            {
        //                saudaOrderDetails.PlantDepot = plantContext.Name;
        //            }
        //            //var freightRouteContext = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.DealerLocationId);
        //            //if (freightRouteContext != null)
        //            //{
        //            //    saudaOrderDetails.FrieghtRoute = freightRouteContext.Name;
        //            //}
        //            saudaOrderDetails.CounterBidOffer = saudaOrderContext.CounterBidOffer;
        //            saudaOrderDetails.CounterBidOfferDate = saudaOrderContext.CounterBidOfferDate != null ? saudaOrderContext.CounterBidOfferDate.Value : DateTime.MinValue;
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
        //        var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
        //        if (userRoleContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserNotFound);
        //        }
        //        var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //        SaudaOrder saudaOrderContext = _emamiContext.SaudaOrders.FirstOrDefault(_ => _.Id == inputDto.SaudaOrderId && _.Sauda != null && _.Sauda.UserId == inputDto.LoginUserId
        //            && DbFunctions.TruncateTime(_.Sauda.BiddingDate) == DbFunctions.TruncateTime(currentDate) && _.CounterBidOffer != 0 && _.CounterBidOfferDate != null);
        //        if (saudaOrderContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.SaudaNotFound);
        //        }
        //        else
        //        {
        //            decimal couterBidOffer = 0;
        //            if (inputDto.IsAccept)
        //            {
        //                var configContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Id == (int)DTO.Enums.Configuration.CounterBidBufferTime);
        //                if (configContext != null)
        //                {
        //                    var bufferTime = TimeSpan.FromMinutes(Convert.ToInt32(configContext.Value));
        //                    var timeLimit = saudaOrderContext.CounterBidOfferDate.Value.TimeOfDay + bufferTime;
        //                    if (timeLimit < currentDate.TimeOfDay)
        //                    {
        //                        return _resultService.ErrorMessage(Constants.CounterBidOfferTimeLimitExceeds);
        //                    }
        //                }
        //                else
        //                {
        //                    return _resultService.ErrorMessage(Constants.RecordNotFound);
        //                }
        //                saudaOrderContext.StatusId = (int)DTO.Enums.Status.Pending;
        //                couterBidOffer = saudaOrderContext.CounterBidOffer;
        //                saudaOrderContext.CounterBidOffer = saudaOrderContext.BidPrice;
        //                saudaOrderContext.BidPrice = couterBidOffer;
        //                saudaOrderContext.ModifiedBy = inputDto.LoginUserId;
        //                saudaOrderContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //                _emamiContext.SaveChanges();
        //                responseMessage = Constants.CounterBidSuccess;
        //            }
        //            else
        //            {
        //                couterBidOffer = saudaOrderContext.CounterBidOffer;
        //                saudaOrderContext.StatusId = (int)DTO.Enums.Status.Rejected;
        //                saudaOrderContext.ModifiedBy = inputDto.LoginUserId;
        //                saudaOrderContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //                _emamiContext.SaveChanges();
        //                responseMessage = Constants.CounterBidReject;
        //            }
        //            try
        //            {
        //                var usersContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
        //                if (usersContext != null && saudaOrderContext != null)
        //                {
        //                    List<string> toUsers = new List<string>();
        //                    if (!string.IsNullOrEmpty(usersContext.Email))
        //                    {
        //                        toUsers.Add(usersContext.Email);
        //                    }
        //                    AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
        //                    string emailSubject = string.Empty;
        //                    if (_resultService.IsEmail() && toUsers != null && toUsers.Any())
        //                    {
        //                        var fromEmail = Constants.FromEmail;
        //                        var plainText = string.Empty;
        //                        EmailTemplate emailTemplate = new EmailTemplate();
        //                        if (inputDto.IsAccept)
        //                        {
        //                            emailSubject = Constants.CounterBidAcceptSubject;
        //                            emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationRAFlowEmail);
        //                        }
        //                        else
        //                        {
        //                            emailSubject = Constants.CounterBidRejectSubject;
        //                            emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderRejectNotificationEmail);
        //                        }

        //                        if (emailTemplate != null)
        //                        {
        //                            var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
        //                                .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(couterBidOffer, 2)).ToString())
        //                                .Replace(Constants.BY_FOR, Constants.BY).Replace(Constants.UserName, usersContext.Name);
        //                            var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
        //                            amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
        //                        }
        //                    }
        //                    var smsPlainTemplate = string.Empty;
        //                    if (_resultService.IsSMS())
        //                    {
        //                        var smsMessage = string.Empty;
        //                        EmailTemplate smsTemplate = new EmailTemplate();
        //                        if (inputDto.IsAccept)
        //                        {
        //                            emailSubject = Constants.CounterBidAcceptSubject;
        //                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationRAFlowSMS);
        //                        }
        //                        else
        //                        {
        //                            emailSubject = Constants.CounterBidRejectSubject;
        //                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaOrderRejectNotificationSMS);
        //                        }
        //                        if (smsTemplate != null)
        //                        {
        //                            smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, saudaOrderContext.Sku.SkuName)
        //                                .Replace(Constants.Quantity, (Math.Round(saudaOrderContext.BidQuantityCase, 0)).ToString()).Replace(Constants.Price, (Math.Round(couterBidOffer, 2)).ToString())
        //                                .Replace(Constants.BY_FOR, Constants.BY).Replace(Constants.UserName, usersContext.Name);
        //                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
        //                            if (!string.IsNullOrEmpty(usersContext.MobileNumber))
        //                            {
        //                                amazonNotificationService.SendMessage(smsMessage, usersContext.MobileNumber);
        //                            }
        //                        }
        //                    }
        //                    if (_resultService.IsPushNotification())
        //                    {
        //                        if (usersContext.RegistrationTypeId != null && usersContext.RegistrationTypeId > 0 && !string.IsNullOrEmpty(usersContext.PushTokenKey))
        //                        {
        //                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                            {
        //                                PushTokenKey = usersContext.PushTokenKey,
        //                                RegistrationTypeId = usersContext.RegistrationTypeId != null ? (int)usersContext.RegistrationTypeId : 0,
        //                                Title = emailSubject,
        //                                Message = smsPlainTemplate,
        //                                //Id = saudaOrderContext.Id,
        //                            };
        //                            _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                        }
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {

        //            }
        //            return _resultService.SuccessMessage(responseMessage);
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
        public ResultDto GetPendingContractChartMobile(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetPendingContractChartMobile";
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var saudaStatus = Constants.OutstandingSaudaStatus;
                //var saudaOrdersContext = _emamiContext.SaudaOrders.AsNoTracking().Join(_emamiContext.Sauda.AsNoTracking(), so => so.SaudaId, s => s.Id, (so, s) => new { so, s })
                //    .Join(_emamiContext.Users.AsNoTracking(), x => x.s.UserId, u => u.Id, (x, u) => new { x.so, x.s, u })
                //    .Join(_emamiContext.PendingContracts.AsNoTracking(), x => x.so.Id, pc => pc.SaudaOrderId, (x, pc) => new { x.so, x.s, x.u, pc })
                //    .Join(_emamiContext.City.AsNoTracking(), x => x.u.CityId, c => c.Id, (x, c) => new { x.so, x.s, x.pc, DealerName = x.u.Name, CityName = c.CityName })
                //    .Where(_ => _.s.UserId == loginUserIdDto.LoginUserId && saudaStatus.Contains(_.so.StatusId) && _.s != null && _.so != null && _.so.OilType != null).ToList();

                var saudaOrdersContext = _emamiContext.PendingContracts.AsNoTracking()
                           .Join(_emamiContext.Users.AsNoTracking(), x => x.CustomerCode, u => u.Code, (x, u) => new { x, u })
                           .Join(_emamiContext.City.AsNoTracking(), x => x.u.CityId, c => c.Id, (x, c) => new { x.x, x.u, DealerName = x.u.Name, CityName = c.CityName })
                           .Where(_ => _.u.Id == loginUserIdDto.LoginUserId
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
                            User = item.DealerName,
                            City = item.CityName,
                            //  BiddingDate = item.x.SaudaDate ?? DateTime.Now,
                            TotalBidPrice = item.x.BasicRate,
                            TotalBidQuantity = item.x.SaudaQuantity,
                            // OiltypeName = item.x.MaterialGroup4
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
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #region New Change Sauda Conversion CR

        public ResultDto GetSKUListForSaudaConversion(SaudaConversionSKUInputDto inputDto)
        {
            _methodName = "GetSKUListForSaudaConversion";
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.SkuId == 0)
                {
                    return _resultService.ErrorMessage(Constants.SkuMissing);
                }
                if (inputDto.DealerId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DealerIdEmpty);
                }
                if (inputDto.PlantOrDepotId == 0)
                {
                    return _resultService.ErrorMessage(Constants.PlantOrDepotEmpty);
                }

                var StateId = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId).StateId;
                var CurrentDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                var result = new List<SaudaConversionSKUOutputDto>();

                var existingSkuData = _emamiContext.Skus.AsNoTracking().FirstOrDefault(s => s.Id == inputDto.SkuId);
                if (existingSkuData != null)
                {
                    //To get the Conversion pack groups from the Sku's Packgroup
                    var ConversionPackgroups = GetSaudaConversionPackgroups(existingSkuData.PackGroupId);

                    if (ConversionPackgroups != null && ConversionPackgroups.Count > 0)
                    {
                        var ConversionUnitandDiffRate = _emamiContext.SaudaConversionUnitAndDifferenceRates.AsNoTracking().
                            Where(_ => _.FromPackGroupId == existingSkuData.PackGroupId && _.FromSkuId == existingSkuData.Id
                            && DbFunctions.TruncateTime(_.FromDate) <= DbFunctions.TruncateTime(CurrentDateTime) && DbFunctions.TruncateTime(_.ToDate) >= DbFunctions.TruncateTime(CurrentDateTime) && _.SourceId == inputDto.PlantOrDepotId && _.StateId == StateId)
                            .Select(_ => _.Id).ToList();

                        result = _emamiContext.Skus.AsNoTracking()
                            .Join(_emamiContext.SaudaConversionUnitAndDifferenceRateDetails.AsNoTracking(), sku => sku.Id, skuunit => skuunit.ToSkuId, (sku, skuunit) => new { skus = sku, skuunits = skuunit })
                            .Where(_ => ConversionUnitandDiffRate.Contains(_.skuunits.SaudaConversionUnitAndDifferenceRateId) && _.skuunits.IsActive && ConversionPackgroups.Contains(_.skuunits.ToPackGroupId) && _.skus.Id != inputDto.SkuId && _.skus.OilTypeId == existingSkuData.OilTypeId)
                            .Select(_ => new SaudaConversionSKUOutputDto()
                            {
                                SkuId = _.skus.Id,
                                SkuName = _.skus.SkuName + "-" + _.skus.SkuCode,
                                BasicRateDifference = _.skuunits.BasicRate,
                                Unit = _.skuunits.ToUnit,
                                SaudaConversionUnitAndDifferenceRateDetailsId = _.skuunits.Id
                            }).ToList();

                        var ToSkuIds = result.Select(_ => _.SkuId).ToList();
                        var MissingSkuresult = _emamiContext.Skus.AsNoTracking()
                                                .Where(_ => ConversionPackgroups.Contains(_.PackGroupId ?? 0) &&
                                                _.Id != existingSkuData.Id &&
                                                !ToSkuIds.Contains(_.Id) &&
                                                _.OilTypeId == existingSkuData.OilTypeId)
                                                .Select(sku => new SkuOutputDto
                                                {
                                                    SkuId = sku.Id,
                                                    Name = sku.SkuName + "-" + sku.SkuCode,
                                                    Code = sku.SkuCode
                                                }).ToList();

                        var SaudaConversionMinvalue = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Id == (int)DTO.Enums.Configuration.SaudaconversionMinValue);
                        var SaudaConversionMaxvalue = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Id == (int)DTO.Enums.Configuration.SaudaconversionMaxValue);

                        result.AddRange(MissingSkuresult.Select(_ => new SaudaConversionSKUOutputDto()
                        {
                            SkuId = _.SkuId,
                            SkuName = _.Name,
                            SkuCode = _.Code
                        }).ToList());

                        //Case to Metric ton value conversion
                        if (result != null && result.Count > 0)
                        {
                            foreach (var sku in result)
                            {
                                sku.CaseToMetricTonValue = _resultService.ConvertCasetoMetricTon(1, sku.SkuId);
                                sku.SaudaConversionMax = SaudaConversionMaxvalue != null ? Convert.ToDecimal(SaudaConversionMaxvalue.Value) : 0;
                                sku.SaudaConversionMin = SaudaConversionMinvalue != null ? Convert.ToDecimal(SaudaConversionMinvalue.Value) : 0;
                            }
                        }

                        #region Notify admin about missing sku


                        var amazonNotificationService = new AmazonNotificationService();

                        var toUser = new List<string>();

                        var NotificationEmailIds = _emamiContext.Configurations.AsNoTracking()
                            .FirstOrDefault(_ => _.Id == (int)DTO.Enums.Configuration.NotificationEmail);

                        if (NotificationEmailIds != null)
                        {
                            toUser = NotificationEmailIds.Value.Split(',').ToList();
                        }

                        var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.ConversionUnitAndDiffRateEmail);
                        if (_resultService.IsEmail())
                        {
                            if (emailTemplate != null && toUser.Any() && MissingSkuresult != null && MissingSkuresult.Any())
                            {
                                StringBuilder Fromsku = new StringBuilder();

                                Fromsku.Append("<tr><td width=30% style='padding: 10px;'>" + existingSkuData.SkuCode + "</td><td width=70% style='padding: 10px;'><p>" + existingSkuData.SkuName + "</p></td></tr>");

                                StringBuilder Tosku = new StringBuilder();

                                foreach (var missingItem in MissingSkuresult)
                                {
                                    Tosku.Append("<tr><td width=30% style='padding: 10px;'>" + missingItem.Code + "</td><td width=70% style='padding: 10px;'><p>" + missingItem.Name + "</p></td></tr>");
                                }

                                var replaceEmailTemplate = emailTemplate.PlainTemplate
                                    .Replace(Constants.ConversionUnitAndDiffRateEmailFromTableContent, Fromsku.ToString())
                                    .Replace(Constants.ConversionUnitAndDiffRateEmailToTableContent, Tosku.ToString());

                                var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, replaceEmailTemplate);

                                amazonNotificationService.SendEmail(toUser, Constants.ConversionUnitAndDiffRatSubject, string.Empty, htmlTemplate, true);
                            }
                        }
                        #endregion

                    }
                }
                return SucessResult(result);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        private List<long> GetSaudaConversionPackgroups(long? FromPackgroupId)
        {
            List<long> PackConversionIds = new List<long>();
            List<long> SaudaConversionIds = new List<long>();
            var saudaConversionTypes = _emamiContext.saudaConversionTypes.AsNoTracking();

            //switch (FromPackgroupId)
            //{
            //    case (int)DTO.Enums.PackGroupType.Premium:
            //        PackConversionIds = saudaConversionTypes.Where(_ => (_.Id == (int)DTO.Enums.SaudaConversionType.BPToBP || _.Id == (int)DTO.Enums.SaudaConversionType.BPToCP) && _.IsActive).Select(_ => _.Id).ToList();
            //        break;
            //    case (int)DTO.Enums.PackGroupType.Bakery:
            //        PackConversionIds = saudaConversionTypes.Where(_ => (_.Id == (int)DTO.Enums.SaudaConversionType.CPToCP || _.Id == (int)DTO.Enums.SaudaConversionType.CPToBP) && _.IsActive).Select(_ => _.Id).ToList();
            //        break;
            //    default:
            //        break;
            //}

            //foreach (var packgroupId in PackConversionIds)
            //{
            //    // BP
            //    if (packgroupId == (int)DTO.Enums.SaudaConversionType.BPToBP || packgroupId == (int)DTO.Enums.SaudaConversionType.CPToBP)
            //    {
            //        SaudaConversionIds.Add((int)DTO.Enums.PackGroupType.Premium);
            //    }
            //    //CP
            //    else if (packgroupId == (int)DTO.Enums.SaudaConversionType.CPToCP || packgroupId == (int)DTO.Enums.SaudaConversionType.BPToCP)
            //    {
            //        SaudaConversionIds.Add((int)DTO.Enums.PackGroupType.Bakery);
            //    }
            //}

            return SaudaConversionIds;
        }

        public ResultDto GetDealerPlantDepotList(UserIdDto inputDto)
        {
            _methodName = "GetDealerPlantDepotList";
            try
            {
                var userPlantList = _emamiContext.UserDepotMapping.AsNoTracking()
                                     .Join(_emamiContext.Depots.AsNoTracking(), ud => ud.DepotId, d => d.Id, (ud, d) => new { UserDepot = ud, Depot = d })
                                     .Where(w => w.UserDepot.UserId == inputDto.UserId && w.Depot.StorageTypeId == (int)DTO.Enums.StorageType.Plant)
                                     .Select(s => new DepotDto
                                     {
                                         Id = s.Depot.Id,
                                         Name = s.Depot.Name,
                                         Code = s.Depot.Code,
                                         IsPlant = s.Depot.IsPlant,
                                         IsActive = s.Depot.IsActive
                                     }).ToList();

                if (userPlantList != null)
                {
                    foreach (var plant in userPlantList)
                    {
                        plant.Depotlist = _emamiContext.UserDepotMapping.AsNoTracking()
                           .Join(_emamiContext.Depots.AsNoTracking(), ud => ud.DepotId, d => d.Id, (ud, d) => new
                           {
                               UserDepot = ud,
                               Depot = d
                           })
                           .Join(_emamiContext.PlantDepotMapping.AsNoTracking(), ud => ud.Depot.Id, pd => pd.DepotId, (ud, pd) => new
                           {
                               UserDepot = ud.UserDepot,
                               Depot = ud.Depot,
                               PlantDepot = pd
                           })
                           .Where(w => w.PlantDepot.PlantId == plant.Id
                           && w.UserDepot.UserId == inputDto.UserId
                           && w.Depot.StorageTypeId == (int)DTO.Enums.StorageType.Depot)
                           .Select(s => new DepotDto
                           {
                               Id = s.Depot.Id,
                               Name = s.Depot.Name,
                               Code = s.Depot.Code,
                               IsPlant = s.Depot.IsPlant,
                               IsActive = s.Depot.IsActive
                           }).ToList();

                        plant.Rakelist = _emamiContext.UserDepotMapping.AsNoTracking()
                                                 .Join(_emamiContext.Depots.AsNoTracking(), ud => ud.DepotId, d => d.Id, (ud, d) => new { UserDepot = ud, Depot = d })
                                                 .Join(_emamiContext.PlantDepotMapping.AsNoTracking(), ud => ud.Depot.Id, pd => pd.DepotId, (ud, pd) => new { UserDepot = ud.UserDepot, Depot = ud.Depot, PlantDepot = pd })
                                                 .Where(w => w.PlantDepot.PlantId == plant.Id
                                                  && w.UserDepot.UserId == inputDto.UserId
                                                 && w.Depot.StorageTypeId == (int)DTO.Enums.StorageType.Rake)
                                                 .Select(s => new DepotDto
                                                 {
                                                     Id = s.Depot.Id,
                                                     Name = s.Depot.Name,
                                                     Code = s.Depot.Code,
                                                     IsPlant = s.Depot.IsPlant,
                                                     IsActive = s.Depot.IsActive
                                                 }).ToList();
                    }
                }
                return SucessResult(userPlantList);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto SaveSaudaConversionSkuDetails(SaudaConversionSKUInputDto inputDto)
        {
            _methodName = "SaveSaudaConversionSkuDetails";
            _logger.Info($"Json Input : {JsonConvert.SerializeObject(inputDto)}");
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.SkuId == 0)
                {
                    return _resultService.ErrorMessage(Constants.SkuMissing);
                }
                var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SkuId);
                if (skuContext == null)
                {
                    return _resultService.ErrorMessage(Constants.SKUNotFound);
                }
                if (inputDto.OilTypeId == 0)
                {
                    return _resultService.ErrorMessage(Constants.OilTypeMissing);
                }
                var oilTypeContext = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.OilTypeId);
                if (oilTypeContext == null)
                {
                    return _resultService.ErrorMessage(Constants.OilTypeNotFound);
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
                if (inputDto.SaudaConvertedToSkuList == null || inputDto.SaudaConvertedToSkuList.Count <= 0)
                {
                    return _resultService.ErrorMessage(Constants.SaudaConversionDetailsMissing);
                }

                var input = new SaudaConversionSku
                {
                    SkuId = inputDto.SkuId,
                    QuantityInSku = inputDto.QuantityInSku,
                    QuantityInMt = inputDto.QuantityInMt,
                    OilTypeId = inputDto.OilTypeId,
                    DealerId = inputDto.DealerId,
                    PlantId = inputDto.PlantId,
                    DepotId = inputDto.DepotId,
                    Remarks = inputDto.Remarks,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                };
                _emamiContext.SaudaConversionSkus.Add(input);
                _emamiContext.SaveChanges();

                foreach (var toSku in inputDto.SaudaConvertedToSkuList)
                {
                    var skuConversionDetails = new SaudaConversionSkuDetail
                    {
                        SaudaConversionSkuId = input.Id,
                        ToSkuId = toSku.SkuId,
                        ToQuantityInSku = toSku.QuantityInSku,
                        ToQuantityInMt = toSku.QuantityInMt,
                        SaudaConversionUnitAndDifferenceRateDetailsId = toSku.SaudaConversionUnitAndDifferenceRateDetailsId
                    };
                    _emamiContext.SaudaConversionSkuDetails.Add(skuConversionDetails);
                }
                _emamiContext.SaveChanges();

                List<long> SaudaConversionId = new List<long>();
                SaudaConversionId.Add(input.Id);
                if (ConsoleSettings.IsInboundDirectSyncToSapAllowed)
                {
                    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken =>
                    {
                        _sapIntegrationService.GetSaudaConversionDetails(SaudaConversionId);
                    });
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.SaudaConversionSkuSaved;
                return resultDto;
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        /// <summary>
        /// Sauda Conversion Sku Pending and Approved list based on status
        /// </summary>
        public ResultDto GetSaudaConversionPendingAndApprovedList(SaudaReportFilterDto inputDto)
        {
            _methodName = "GetSaudaConversionPendingAndApprovedList";
            try
            {
                var result = new SaudaConversionSKUStatusListDto();
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
                List<SaudaConversionSkusDetail> saudaConversionSkusDetails = new List<SaudaConversionSkusDetail>();
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var description = UtilityHelper.GetEnumDescription(DTO.Enums.Configuration.InboundInterfacenotSyncedToSAPMinutes);
                var configurationContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Key == description).Value;
                if (inputDto.StatusId == (int)DTO.Enums.Status.Pending)
                {
                    bool IsSapSyncReceivedFoSaudaConversionUpdate = false;
                    string remarks = string.Empty;
                    //Get Pending Dealer SaudaConversions Sku from table
                    var PendingSaudaConversionDetailsList = _emamiContext.SaudaConversionSkus.AsNoTracking()
                                                                .Join(_emamiContext.Skus.AsNoTracking(), so => so.SkuId, sku => sku.Id, (so, sku) => new { so, sku })
                                                                .Where(_ => /* string.IsNullOrEmpty(_.SaudaNumber)*/ /*&& string.IsNullOrEmpty(_.Remarks)*/ !_.so.IsApproved && _.so.StatusId != (int)DTO.Enums.Status.Rejected &&
                                                                DbFunctions.TruncateTime(_.so.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                DbFunctions.TruncateTime(_.so.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && (inputDto.VerticalId > 0 ? _.sku.DivisionId == inputDto.VerticalId : _.sku.DivisionId > 0))
                                                                .OrderByDescending(_ => _.so.CreatedDate).Select(s => s.so).ToList();
                    if (PendingSaudaConversionDetailsList != null && PendingSaudaConversionDetailsList.Count > 0)
                    {
                        var pendingSaudaConversionList = GetSkuConversionDetails(PendingSaudaConversionDetailsList);

                        foreach (var saudaDetail in pendingSaudaConversionList)
                        {
                            TimeSpan difference = currentDate.Subtract(Convert.ToDateTime(saudaDetail.ConversionModifiedDate));

                            if (difference.TotalMinutes > Convert.ToDouble(configurationContext) && saudaDetail.IsSapDataSync)
                            {
                                if (saudaDetail.SaudaConversionUpdateFromSap)
                                {
                                    IsSapSyncReceivedFoSaudaConversionUpdate = true;
                                    remarks = saudaDetail.Remarks;
                                }
                                else
                                {
                                    IsSapSyncReceivedFoSaudaConversionUpdate = false;
                                    remarks = "Sauda Conversion Sync From Sap not Received";
                                }
                            }
                            else
                            {
                                IsSapSyncReceivedFoSaudaConversionUpdate = saudaDetail.SaudaConversionUpdateFromSap;
                                remarks = saudaDetail.Remarks;
                            }
                            var bdoDetail = GetBDONameFromDealerId(saudaDetail.DealerId);
                            var detail = new SaudaConversionSkusDetail()
                            {
                                ZonalHeadName = GetZonalHeadNameFromBDOId(bdoDetail.BDOId),
                                BdoName = bdoDetail.BDOName,
                                DealerName = GetUserName(saudaDetail.DealerId),
                                SkuConversionId = saudaDetail.SkuConversionId,
                                ConversionCreatedDate = saudaDetail.ConversionCreatedDate,
                                SkuName = saudaDetail.SkuName,
                                SaudaQuantityInMT = saudaDetail.SaudaQuantityInMT,
                                Remarks = remarks,
                                SaudaQuantityInSku = saudaDetail.SaudaQuantityInSku,
                                PlantOrDepotCode = saudaDetail.PlantOrDepotCode,
                                PlantOrDepotName = saudaDetail.PlantOrDepotName,
                                SaudaConversionUpdateFromSap = IsSapSyncReceivedFoSaudaConversionUpdate,
                                ReprocessStatus = saudaDetail.ReprocessStatus,
                                IsSapDataSync = saudaDetail.IsSapDataSync,
                                StatusId = saudaDetail.StatusId
                            };
                            saudaConversionSkusDetails.Add(detail);
                        }
                    }
                }
                else if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                {
                    bool IsSapSyncReceivedFoSaudaConversionUpdate = false;
                    string remarks = string.Empty;
                    //Get Approved Dealer SaudaConversions Sku from table
                    var ApprovedSaudaConversionDetailsList = _emamiContext.SaudaConversionSkus.AsNoTracking()
                                                                .Join(_emamiContext.Skus.AsNoTracking(), so => so.SkuId, sku => sku.Id, (so, sku) => new { so, sku })
                                                                .Where(_ => /*!string.IsNullOrEmpty(_.SaudaNumber) &&*/ /*!string.IsNullOrEmpty(_.Remarks)*/  _.so.IsApproved && _.so.StatusId != (int)DTO.Enums.Status.Rejected &&
                                                                DbFunctions.TruncateTime(_.so.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                DbFunctions.TruncateTime(_.so.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && (inputDto.VerticalId > 0 ? _.sku.DivisionId == inputDto.VerticalId : _.sku.DivisionId > 0))
                                                                .OrderByDescending(_ => _.so.CreatedDate).Select(s => s.so).ToList();


                    if (ApprovedSaudaConversionDetailsList != null && ApprovedSaudaConversionDetailsList.Count > 0)
                    {
                        var approvedSaudaConversionList = GetSkuConversionDetails(ApprovedSaudaConversionDetailsList);

                        foreach (var saudaDetail in approvedSaudaConversionList)
                        {
                            TimeSpan difference = currentDate.Subtract(Convert.ToDateTime(saudaDetail.ConversionModifiedDate));

                            if (difference.TotalMinutes > Convert.ToDouble(configurationContext) && saudaDetail.IsSapDataSync)
                            {
                                if (saudaDetail.SaudaConversionUpdateFromSap)
                                {
                                    IsSapSyncReceivedFoSaudaConversionUpdate = true;
                                    remarks = saudaDetail.Remarks;
                                }
                                else
                                {
                                    IsSapSyncReceivedFoSaudaConversionUpdate = false;
                                    remarks = "Sauda Conversion Sync From Sap not Received";
                                }
                            }
                            else
                            {
                                IsSapSyncReceivedFoSaudaConversionUpdate = saudaDetail.SaudaConversionUpdateFromSap;
                                remarks = saudaDetail.Remarks;
                            }
                            var bdoDetail = GetBDONameFromDealerId(saudaDetail.DealerId);
                            var detail = new SaudaConversionSkusDetail()
                            {
                                ZonalHeadName = GetZonalHeadNameFromBDOId(bdoDetail.BDOId),
                                BdoName = bdoDetail.BDOName,
                                DealerName = GetUserName(saudaDetail.DealerId),
                                SkuConversionId = saudaDetail.SkuConversionId,
                                ConversionCreatedDate = saudaDetail.ConversionCreatedDate,
                                SkuName = saudaDetail.SkuName,
                                SaudaQuantityInMT = saudaDetail.SaudaQuantityInMT,
                                Remarks = remarks,
                                SaudaQuantityInSku = saudaDetail.SaudaQuantityInSku,
                                PlantOrDepotCode = saudaDetail.PlantOrDepotCode,
                                PlantOrDepotName = saudaDetail.PlantOrDepotName,
                                SaudaConversionUpdateFromSap = IsSapSyncReceivedFoSaudaConversionUpdate,
                                ReprocessStatus = saudaDetail.ReprocessStatus,
                                IsSapDataSync = saudaDetail.IsSapDataSync,
                                StatusId = saudaDetail.StatusId
                            };
                            saudaConversionSkusDetails.Add(detail);
                        }
                    }
                }
                else if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                {

                    //Get Approved Dealer SaudaConversions Sku from table
                    var RejectedSaudaConversionDetailsList = _emamiContext.SaudaConversionSkus.AsNoTracking()
                                                                .Join(_emamiContext.Skus.AsNoTracking(), so => so.SkuId, sku => sku.Id, (so, sku) => new { so, sku })
                                                                .Where(_ => _.so.StatusId == (int)DTO.Enums.Status.Rejected &&
                                                                DbFunctions.TruncateTime(_.so.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                DbFunctions.TruncateTime(_.so.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && (inputDto.VerticalId > 0 ? _.sku.DivisionId == inputDto.VerticalId : _.sku.DivisionId > 0))
                                                                .OrderByDescending(_ => _.so.CreatedDate).Select(s => s.so).ToList();

                    if (RejectedSaudaConversionDetailsList != null && RejectedSaudaConversionDetailsList.Count > 0)
                    {
                        var rejectedSaudaConversionList = GetSkuConversionDetails(RejectedSaudaConversionDetailsList);

                        foreach (var saudaDetail in rejectedSaudaConversionList)
                        {

                            var bdoDetail = GetBDONameFromDealerId(saudaDetail.DealerId);
                            var detail = new SaudaConversionSkusDetail()
                            {
                                ZonalHeadName = GetZonalHeadNameFromBDOId(bdoDetail.BDOId),
                                BdoName = bdoDetail.BDOName,
                                DealerName = GetUserName(saudaDetail.DealerId),
                                SkuConversionId = saudaDetail.SkuConversionId,
                                ConversionCreatedDate = saudaDetail.ConversionCreatedDate,
                                SkuName = saudaDetail.SkuName,
                                SaudaQuantityInMT = saudaDetail.SaudaQuantityInMT,
                                Remarks = saudaDetail.Remarks,
                                SaudaQuantityInSku = saudaDetail.SaudaQuantityInSku,
                                PlantOrDepotCode = saudaDetail.PlantOrDepotCode,
                                PlantOrDepotName = saudaDetail.PlantOrDepotName,
                                SaudaConversionUpdateFromSap = saudaDetail.SaudaConversionUpdateFromSap,
                                ReprocessStatus = saudaDetail.ReprocessStatus,
                                IsSapDataSync = saudaDetail.IsSapDataSync,
                                StatusId = saudaDetail.StatusId
                            };
                            saudaConversionSkusDetails.Add(detail);
                        }
                    }
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.StatusIsEmpty);
                }
                return SucessResult(saudaConversionSkusDetails);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        /// <summary>
        /// Sauda Conversion Sku Pending and Approved list for Zonal Head 
        /// </summary>
        public ResultDto GetZonalHeadSaudaConversionPendingApprovedList(SaudaReportFilterDto inputDto)
        {
            _methodName = "GetZonalHeadSaudaConversionPendingApprovedList";
            try
            {
                var result = new SaudaConversionSKUStatusListDto();
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.LoginUserId <= 0)
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.ZonalHeadIsMissing);
                }
                else
                {
                    //Common for Pending and Approved List
                    // Get StateTrader list under zonal head   
                    var BDOList = _emamiContext.Users.AsNoTracking()
                                                     .Where(_ => _.ReportingToId == userContext.Id)
                                                     .Select(_ => new SaudaConversionSkuBDOUserList { BDOId = _.Id, BDOName = _.Name, BDOAddress = _.Address1 })
                                                     .ToList();

                    //Get Pending Dealer SaudaConversions Sku from table
                    var PendingSaudaConversionDetailsList = _emamiContext.SaudaConversionSkus.AsNoTracking()
                                                                .Where(_ => /*string.IsNullOrEmpty(_.SaudaNumber) && string.IsNullOrEmpty(_.Remarks) &&*/ !_.IsApproved && _.StatusId != (int)DTO.Enums.Status.Rejected &&
                                                                DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                                                                .OrderByDescending(_ => _.CreatedDate).ToList();
                    if (PendingSaudaConversionDetailsList != null && PendingSaudaConversionDetailsList.Count > 0)
                    {
                        //Get Dealer list under StateTrader
                        foreach (var StateTrader in BDOList)
                        {
                            var BDODealerList = _emamiContext.UserCustomerMapping.AsNoTracking()
                                                                                .Where(_ => _.UserId == StateTrader.BDOId)
                                                                                .Select(_ => _.CustomerId).ToList();
                            var saudaDealers = PendingSaudaConversionDetailsList.Select(_ => _.DealerId).Distinct().ToList();

                            var dealerUserContext = (from dealer in _emamiContext.Users.AsNoTracking()
                                                     join saudaDealer in saudaDealers
                                                     on dealer.Id equals saudaDealer
                                                     where BDODealerList.Contains(dealer.Id) && saudaDealer == dealer.Id
                                                     select new SaudaConversionSkuDealerUserList
                                                     {
                                                         DealerId = dealer.Id,
                                                         DealerName = dealer.Name,
                                                         DealerAddress = dealer.Address1
                                                     }).ToList();
                            if (dealerUserContext != null && dealerUserContext.Count > 0)
                            {
                                StateTrader.BDODealerUsersList.AddRange(dealerUserContext);
                            }
                        }
                        result.PendingSaudaConversionList = GetSkuConversionDetailsZonalHead(BDOList, PendingSaudaConversionDetailsList);
                    }

                    //Get Approved Dealer SaudaConversions Sku from table
                    var ApprovedSaudaConversionDetailsList = _emamiContext.SaudaConversionSkus.AsNoTracking()
                                                                .Where(_ => /*!string.IsNullOrEmpty(_.SaudaNumber) &&*/ /*!string.IsNullOrEmpty(_.Remarks)*/ _.IsApproved &&
                                                                DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                                                                .OrderByDescending(_ => _.CreatedDate).ToList();
                    if (ApprovedSaudaConversionDetailsList != null && ApprovedSaudaConversionDetailsList.Count > 0)
                    {
                        //Get Dealer list under StateTrader
                        foreach (var StateTrader in BDOList)
                        {
                            var BDODealerList = _emamiContext.UserCustomerMapping.AsNoTracking()
                                                                                .Where(_ => _.UserId == StateTrader.BDOId)
                                                                                .Select(_ => _.CustomerId).ToList();
                            var saudaDealers = ApprovedSaudaConversionDetailsList.Select(_ => _.DealerId).Distinct().ToList();

                            if (StateTrader.BDODealerUsersList.Count(_ => saudaDealers.Contains(_.DealerId)) <= 0)
                            {
                                var dealerUserContext = (from dealer in _emamiContext.Users.AsNoTracking()
                                                         join saudaDealer in saudaDealers
                                                         on dealer.Id equals saudaDealer
                                                         where BDODealerList.Contains(dealer.Id) && saudaDealer == dealer.Id
                                                         select new SaudaConversionSkuDealerUserList
                                                         {
                                                             DealerId = dealer.Id,
                                                             DealerName = dealer.Name,
                                                             DealerAddress = dealer.Address1
                                                         }).ToList();
                                if (dealerUserContext != null && dealerUserContext.Count > 0)
                                {
                                    StateTrader.BDODealerUsersList.AddRange(dealerUserContext);
                                }
                            }
                        }
                        result.ApprovedSaudaConversionList = GetSkuConversionDetailsZonalHead(BDOList, ApprovedSaudaConversionDetailsList);
                    }
                }

                return SucessResult(result);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        /// <summary>
        /// Sauda Conversion Sku Pending and Approved list for StateTrader - Sales person
        /// </summary>
        public ResultDto GetBDOSaudaConversionPendingApprovedList(SaudaReportFilterDto inputDto)
        {
            _methodName = "GetBDOSaudaConversionPendingApprovedList";
            try
            {
                var result = new SaudaConversionSKUStatusListDto();
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.LoginUserId <= 0)
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.SalesPersonMissing);
                }
                else
                {
                    //Common for Pending and Approved List                    
                    //Get Dealer list under StateTrader

                    var BDODealerList = _emamiContext.UserCustomerMapping.AsNoTracking()
                                                                        .Where(_ => _.UserId == userContext.Id)
                                                                        .Select(_ => _.CustomerId).ToList();


                    //Get Pending Dealer SaudaConversions Sku from table
                    var PendingSaudaConversionDetailsList = _emamiContext.SaudaConversionSkus.AsNoTracking()
                                                                .Where(_ => /*string.IsNullOrEmpty(_.SaudaNumber) && string.IsNullOrEmpty(_.Remarks) &&*/ !_.IsApproved && _.StatusId != (int)DTO.Enums.Status.Rejected &&
                                                                DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                                                                .OrderByDescending(_ => _.CreatedDate).ToList();
                    if (PendingSaudaConversionDetailsList != null && PendingSaudaConversionDetailsList.Count > 0)
                    {
                        var saudaDealers = PendingSaudaConversionDetailsList.Select(_ => _.DealerId).Distinct().ToList();
                        var BDOdealerUserContext = _emamiContext.Users.AsNoTracking()
                                                                .Where(_ => BDODealerList.Contains(_.Id) && saudaDealers.Contains(_.Id))
                                                                .Select(_ => new SaudaConversionSkuDealerUserList
                                                                {
                                                                    DealerId = _.Id,
                                                                    DealerName = _.Name,
                                                                    DealerAddress = _.Address1
                                                                }).ToList();

                        result.PendingSaudaConversionList = GetSkuConversionDetailsBDO(BDOdealerUserContext, PendingSaudaConversionDetailsList);
                    }

                    //Get Approved Dealer SaudaConversions Sku from table
                    var ApprovedSaudaConversionDetailsList = _emamiContext.SaudaConversionSkus.AsNoTracking()
                                                                .Where(_ => /*!string.IsNullOrEmpty(_.SaudaNumber) &&*/ _.IsApproved &&
                                                                DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                                                                .OrderByDescending(_ => _.CreatedDate).ToList();
                    if (ApprovedSaudaConversionDetailsList != null && ApprovedSaudaConversionDetailsList.Count > 0)
                    {
                        var saudaDealers = ApprovedSaudaConversionDetailsList.Select(_ => _.DealerId).Distinct().ToList();
                        var BDOdealerUserContext = _emamiContext.Users.AsNoTracking()
                                                                .Where(_ => BDODealerList.Contains(_.Id) && saudaDealers.Contains(_.Id))
                                                                .Select(_ => new SaudaConversionSkuDealerUserList
                                                                {
                                                                    DealerId = _.Id,
                                                                    DealerName = _.Name,
                                                                    DealerAddress = _.Address1
                                                                }).ToList();

                        result.ApprovedSaudaConversionList = GetSkuConversionDetailsBDO(BDOdealerUserContext, ApprovedSaudaConversionDetailsList);
                    }
                }

                return SucessResult(result);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        /// <summary>
        /// Sauda Conversion Sku Pending and Approved list for Dealer 
        /// </summary>
        public ResultDto GetDealerSaudaConversionPendingApprovedList(SaudaReportFilterDto inputDto)
        {
            _methodName = "GetDealerSaudaConversionPendingApprovedList";
            try
            {
                var result = new SaudaConversionSKUStatusListDto();
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.LoginUserId <= 0)
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.DealerMissing);
                }
                else
                {
                    //Get Pending Dealer SaudaConversions Sku from table
                    var PendingSaudaConversionDetailsList = _emamiContext.SaudaConversionSkus.AsNoTracking()
                                                                .Where(_ => /*string.IsNullOrEmpty(_.SaudaNumber) && string.IsNullOrEmpty(_.Remarks) &&*/ !_.IsApproved && _.StatusId != (int)DTO.Enums.Status.Rejected &&
                                                                DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                                                                .OrderByDescending(_ => _.CreatedDate).ToList();
                    if (PendingSaudaConversionDetailsList != null && PendingSaudaConversionDetailsList.Count > 0)
                    {
                        result.PendingSaudaConversionList = GetSkuConversionDetailsDealer(userContext.Id, PendingSaudaConversionDetailsList);
                    }

                    //Get Approved Dealer SaudaConversions Sku from table
                    var ApprovedSaudaConversionDetailsList = _emamiContext.SaudaConversionSkus.AsNoTracking()
                                                                .Where(_ => /*!string.IsNullOrEmpty(_.SaudaNumber) &&*/ /*!string.IsNullOrEmpty(_.Remarks) &&*/ _.IsApproved &&
                                                                DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                                                                .OrderByDescending(_ => _.CreatedDate).ToList();
                    if (ApprovedSaudaConversionDetailsList != null && ApprovedSaudaConversionDetailsList.Count > 0)
                    {
                        result.ApprovedSaudaConversionList = GetSkuConversionDetailsDealer(userContext.Id, ApprovedSaudaConversionDetailsList);
                    }
                }

                return SucessResult(result);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        private SaudaConversionSkuStatusWiseDetailsDto GetSkuConversionDetailsZonalHead(List<SaudaConversionSkuBDOUserList> BDOList, List<SaudaConversionSku> SaudaConversionDetailsList)
        {
            SaudaConversionSkuStatusWiseDetailsDto saudaConversionSkuDetails = new SaudaConversionSkuStatusWiseDetailsDto();
            var bdolist = new List<SaudaConversionSkuBDOUserList>();

            foreach (var StateTrader in BDOList)
            {
                var SaudaConversionSkuBDOUserList = new SaudaConversionSkuBDOUserList()
                {
                    BDOId = StateTrader.BDOId,
                    BDOName = StateTrader.BDOName,
                    BDOAddress = StateTrader.BDOAddress
                };
                if (StateTrader.BDODealerUsersList != null && StateTrader.BDODealerUsersList.Count > 0)
                {
                    foreach (var dealer in StateTrader.BDODealerUsersList)
                    {
                        var dealerdetails = new SaudaConversionSkuDealerUserList();
                        var saudaDetailsWithNullContext = SaudaConversionDetailsList.Where(_ => _.DealerId == dealer.DealerId && _.SaudaConversionSkuHeaderId == null)
                                                             .ToList()
                                                             .Select(_ => new SaudaConversionSkusDetail()
                                                             {
                                                                 SkuConversionId = _.Id,
                                                                 SkuConversionHeaderId = _.SaudaConversionSkuHeaderId,
                                                                 SkuName = GetSKUName(_.SkuId),
                                                                 SaudaNumber = _.SaudaNumber,
                                                                 SaudaQuantityInMT = _.QuantityInMt,
                                                                 ConversionCreatedDate = _.CreatedDate,
                                                                 ConversionModifiedDate = _.ModifiedDate ?? DateTime.MinValue
                                                             }).OrderByDescending(_ => _.ConversionCreatedDate).ToList();

                        var saudaDetailsWithHeaderIdContext = SaudaConversionDetailsList.Where(_ => _.DealerId == dealer.DealerId && _.SaudaConversionSkuHeaderId != null)
                                                             .ToList()
                                                             .Select(_ => new SaudaConversionSkusDetail()
                                                             {
                                                                 SkuConversionId = _.Id,
                                                                 SkuConversionHeaderId = _.SaudaConversionSkuHeaderId,
                                                                 SkuName = GetSKUName(_.SkuId),
                                                                 SaudaNumber = _.SaudaNumber,
                                                                 SaudaQuantityInMT = _.QuantityInMt,
                                                                 ConversionCreatedDate = _.CreatedDate,
                                                                 ConversionModifiedDate = _.ModifiedDate ?? DateTime.MinValue
                                                             }).OrderByDescending(_ => _.ConversionCreatedDate).ToList();

                        if ((saudaDetailsWithNullContext != null && saudaDetailsWithNullContext.Count > 0) || (saudaDetailsWithHeaderIdContext != null && saudaDetailsWithHeaderIdContext.Count > 0))
                        {
                            dealerdetails.DealerId = dealer.DealerId;
                            dealerdetails.DealerName = dealer.DealerName;
                            dealerdetails.DealerAddress = dealer.DealerAddress;
                            if (saudaDetailsWithNullContext != null && saudaDetailsWithNullContext.Count > 0)
                            {
                                dealerdetails.SaudaConversionSkuDetails.AddRange(saudaDetailsWithNullContext);
                            }
                            if (saudaDetailsWithHeaderIdContext != null && saudaDetailsWithHeaderIdContext.Count > 0)
                            {
                                var saudaDetailsWithHeaderContext = (from header in saudaDetailsWithHeaderIdContext
                                                                     join child in saudaDetailsWithHeaderIdContext
                                                                     on header.SkuConversionId equals child.SkuConversionHeaderId
                                                                     where header.SkuConversionHeaderId == 0
                                                                     select new SaudaConversionSkusDetail()
                                                                     {
                                                                         SkuConversionId = header.SkuConversionId,
                                                                         SkuConversionHeaderId = header.SkuConversionHeaderId,
                                                                         SkuName = header.SkuName,
                                                                         SaudaNumber = header.SaudaNumber + "-" + child.SaudaNumber,
                                                                         SaudaQuantityInMT = header.SaudaQuantityInMT + child.SaudaQuantityInMT,
                                                                         ConversionCreatedDate = header.ConversionCreatedDate,
                                                                         ConversionModifiedDate = header.ConversionModifiedDate
                                                                     }).ToList();

                                if (saudaDetailsWithHeaderContext != null && saudaDetailsWithHeaderContext.Count > 0)
                                {
                                    dealerdetails.SaudaConversionSkuDetails.AddRange(saudaDetailsWithHeaderContext);
                                }
                            }
                            SaudaConversionSkuBDOUserList.BDODealerUsersList.Add(dealerdetails);
                        }
                    }
                    if (SaudaConversionSkuBDOUserList.BDODealerUsersList != null && SaudaConversionSkuBDOUserList.BDODealerUsersList.Count > 0)
                    {
                        bdolist.Add(SaudaConversionSkuBDOUserList);
                    }
                }
            }
            saudaConversionSkuDetails.BDOUsersList.AddRange(bdolist);
            return saudaConversionSkuDetails;
        }

        private SaudaConversionSkuStatusWiseDetailsDto GetSkuConversionDetailsBDO(List<SaudaConversionSkuDealerUserList> DealerList, List<SaudaConversionSku> SaudaConversionDetailsList)
        {
            SaudaConversionSkuStatusWiseDetailsDto saudaConversionSkuDetails = new SaudaConversionSkuStatusWiseDetailsDto();
            var dealerList = new List<SaudaConversionSkuDealerUserList>();
            foreach (var dealer in DealerList)
            {
                var dealerdetail = new SaudaConversionSkuDealerUserList()
                {
                    DealerId = dealer.DealerId,
                    DealerName = dealer.DealerName,
                    DealerAddress = dealer.DealerAddress
                };

                var saudaDetailsWithNullContext = SaudaConversionDetailsList.Where(_ => _.DealerId == dealer.DealerId && _.SaudaConversionSkuHeaderId == null)
                                                    .ToList()
                                                    .Select(_ => new SaudaConversionSkusDetail()
                                                    {
                                                        SkuConversionId = _.Id,
                                                        SkuConversionHeaderId = _.SaudaConversionSkuHeaderId,
                                                        SkuName = GetSKUName(_.SkuId),
                                                        SaudaNumber = _.SaudaNumber,
                                                        SaudaQuantityInMT = _.QuantityInMt,
                                                        ConversionCreatedDate = _.CreatedDate,
                                                        ConversionModifiedDate = _.ModifiedDate ?? DateTime.MinValue
                                                    }).OrderByDescending(_ => _.ConversionCreatedDate).ToList();
                var saudaDetailsWithHeaderIdContext = SaudaConversionDetailsList.Where(_ => _.DealerId == dealer.DealerId && _.SaudaConversionSkuHeaderId != null)
                                                    .ToList()
                                                    .Select(_ => new SaudaConversionSkusDetail()
                                                    {
                                                        SkuConversionId = _.Id,
                                                        SkuConversionHeaderId = _.SaudaConversionSkuHeaderId,
                                                        SkuName = GetSKUName(_.SkuId),
                                                        SaudaNumber = _.SaudaNumber,
                                                        SaudaQuantityInMT = _.QuantityInMt,
                                                        ConversionCreatedDate = _.CreatedDate,
                                                        ConversionModifiedDate = _.ModifiedDate ?? DateTime.MinValue
                                                    }).OrderByDescending(_ => _.ConversionCreatedDate).ToList();

                if ((saudaDetailsWithNullContext != null && saudaDetailsWithNullContext.Count > 0) || (saudaDetailsWithHeaderIdContext != null && saudaDetailsWithHeaderIdContext.Count > 0))
                {
                    if (saudaDetailsWithNullContext != null && saudaDetailsWithNullContext.Count > 0)
                    {
                        dealerdetail.SaudaConversionSkuDetails.AddRange(saudaDetailsWithNullContext);
                    }
                    if (saudaDetailsWithHeaderIdContext != null && saudaDetailsWithHeaderIdContext.Count > 0)
                    {
                        var saudaDetailsWithHeaderContext = (from header in saudaDetailsWithHeaderIdContext
                                                             join child in saudaDetailsWithHeaderIdContext
                                                             on header.SkuConversionId equals child.SkuConversionHeaderId
                                                             where header.SkuConversionHeaderId == 0
                                                             select new SaudaConversionSkusDetail()
                                                             {
                                                                 SkuConversionId = header.SkuConversionId,
                                                                 SkuConversionHeaderId = header.SkuConversionHeaderId,
                                                                 SkuName = header.SkuName,
                                                                 SaudaNumber = header.SaudaNumber + "-" + child.SaudaNumber,
                                                                 SaudaQuantityInMT = header.SaudaQuantityInMT + child.SaudaQuantityInMT,
                                                                 ConversionCreatedDate = header.ConversionCreatedDate,
                                                                 ConversionModifiedDate = header.ConversionModifiedDate
                                                             }).ToList();

                        if (saudaDetailsWithHeaderContext != null && saudaDetailsWithHeaderContext.Count > 0)
                        {
                            dealerdetail.SaudaConversionSkuDetails.AddRange(saudaDetailsWithHeaderContext);
                        }
                    }
                }
                if (dealerdetail.SaudaConversionSkuDetails != null && dealerdetail.SaudaConversionSkuDetails.Count > 0)
                {
                    dealerList.Add(dealerdetail);
                }
            }

            saudaConversionSkuDetails.BDODealerUsersList.AddRange(dealerList);
            return saudaConversionSkuDetails;
        }

        private SaudaConversionSkuStatusWiseDetailsDto GetSkuConversionDetailsDealer(long DealerId, List<SaudaConversionSku> SaudaConversionDetailsList)
        {
            SaudaConversionSkuStatusWiseDetailsDto saudaConversionSkuDetails = new SaudaConversionSkuStatusWiseDetailsDto();
            var saudaDetailsWithNullContext = SaudaConversionDetailsList.Where(_ => _.DealerId == DealerId && _.SaudaConversionSkuHeaderId == null)
                                                    .ToList()
                                                    .Select(_ => new SaudaConversionSkusDetail()
                                                    {
                                                        SkuConversionId = _.Id,
                                                        SkuConversionHeaderId = _.SaudaConversionSkuHeaderId,
                                                        SkuName = GetSKUName(_.SkuId),
                                                        SaudaNumber = _.SaudaNumber,
                                                        SaudaQuantityInMT = _.QuantityInMt,
                                                        ConversionCreatedDate = _.CreatedDate,
                                                        ConversionModifiedDate = _.ModifiedDate ?? DateTime.MinValue
                                                    }).OrderByDescending(_ => _.ConversionCreatedDate).ToList();

            var saudaDetailsWithHeaderIdContext = SaudaConversionDetailsList.Where(_ => _.DealerId == DealerId && _.SaudaConversionSkuHeaderId != null)
                                                    .ToList()
                                                    .Select(_ => new SaudaConversionSkusDetail()
                                                    {
                                                        SkuConversionId = _.Id,
                                                        SkuConversionHeaderId = _.SaudaConversionSkuHeaderId,
                                                        SkuName = GetSKUName(_.SkuId),
                                                        SaudaNumber = _.SaudaNumber,
                                                        SaudaQuantityInMT = _.QuantityInMt,
                                                        ConversionCreatedDate = _.CreatedDate,
                                                        ConversionModifiedDate = _.ModifiedDate ?? DateTime.MinValue
                                                    }).OrderByDescending(_ => _.ConversionCreatedDate).ToList();

            if ((saudaDetailsWithNullContext != null && saudaDetailsWithNullContext.Count > 0) || (saudaDetailsWithHeaderIdContext != null && saudaDetailsWithHeaderIdContext.Count > 0))
            {
                if (saudaDetailsWithNullContext != null && saudaDetailsWithNullContext.Count > 0)
                {
                    saudaConversionSkuDetails.SaudaConversionSkuDetails.AddRange(saudaDetailsWithNullContext);
                }
                if (saudaDetailsWithHeaderIdContext != null && saudaDetailsWithHeaderIdContext.Count > 0)
                {
                    var saudaDetailsWithHeaderContext = (from header in saudaDetailsWithHeaderIdContext
                                                         join child in saudaDetailsWithHeaderIdContext
                                                         on header.SkuConversionId equals child.SkuConversionHeaderId
                                                         where header.SkuConversionHeaderId == 0
                                                         select new SaudaConversionSkusDetail()
                                                         {
                                                             SkuConversionId = header.SkuConversionId,
                                                             SkuConversionHeaderId = header.SkuConversionHeaderId,
                                                             SkuName = header.SkuName,
                                                             SaudaNumber = header.SaudaNumber + "-" + child.SaudaNumber,
                                                             SaudaQuantityInMT = header.SaudaQuantityInMT + child.SaudaQuantityInMT,
                                                             ConversionCreatedDate = header.ConversionCreatedDate,
                                                             ConversionModifiedDate = header.ConversionModifiedDate
                                                         }).ToList();

                    if (saudaDetailsWithHeaderContext != null && saudaDetailsWithHeaderContext.Count > 0)
                    {
                        saudaConversionSkuDetails.SaudaConversionSkuDetails.AddRange(saudaDetailsWithHeaderContext);
                    }
                }
            }
            return saudaConversionSkuDetails;
        }

        private List<SaudaConversionSkusDetail> GetSkuConversionDetails(List<SaudaConversionSku> SaudaConversionDetailsList)
        {
            List<SaudaConversionSkusDetail> saudaConversionSkuDetails = new List<SaudaConversionSkusDetail>();
            var saudaDetailsWithNullContext = SaudaConversionDetailsList.Where(_ => _.SaudaConversionSkuHeaderId == null)
                                                    .ToList()
                                                    .Select(_ => new SaudaConversionSkusDetail()
                                                    {
                                                        SkuConversionId = _.Id,
                                                        SkuConversionHeaderId = _.SaudaConversionSkuHeaderId,
                                                        DealerId = _.DealerId,
                                                        SkuName = GetSKUName(_.SkuId),
                                                        SaudaNumber = _.SaudaNumber,
                                                        SaudaQuantityInMT = _.QuantityInMt,
                                                        ConversionCreatedDate = _.CreatedDate,
                                                        ConversionModifiedDate = _.ModifiedDate ?? DateTime.MinValue,
                                                        Remarks = _.Remarks,
                                                        SaudaQuantityInSku = _.QuantityInSku,
                                                        PlantOrDepotName = _.PlantId == 0 ? GetPlantOrDepotName(_.DepotId) : GetPlantOrDepotName(_.PlantId),
                                                        PlantOrDepotCode = _.PlantId == 0 ? GetPlantOrDepotCode(_.DepotId) : GetPlantOrDepotCode(_.PlantId),
                                                        SaudaConversionUpdateFromSap = _.SaudaConversionUpdateFromSap,
                                                        ReprocessStatus = _.IsApproved,
                                                        IsSapDataSync = _.IsSAPDataSync,
                                                        StatusId = _.StatusId
                                                    }).OrderByDescending(_ => _.ConversionCreatedDate).ToList();

            var saudaDetailsWithHeaderIdContext = SaudaConversionDetailsList.Where(_ => _.SaudaConversionSkuHeaderId != null)
                                                    .ToList()
                                                    .Select(_ => new SaudaConversionSkusDetail()
                                                    {
                                                        SkuConversionId = _.Id,
                                                        SkuConversionHeaderId = _.SaudaConversionSkuHeaderId,
                                                        DealerId = _.DealerId,
                                                        SkuName = GetSKUName(_.SkuId),
                                                        SaudaNumber = _.SaudaNumber,
                                                        SaudaQuantityInMT = _.QuantityInMt,
                                                        ConversionCreatedDate = _.CreatedDate,
                                                        ConversionModifiedDate = _.ModifiedDate ?? DateTime.MinValue,
                                                        Remarks = _.Remarks,
                                                        SaudaQuantityInSku = _.QuantityInSku,
                                                        PlantOrDepotName = _.PlantId == 0 ? GetPlantOrDepotName(_.DepotId) : GetPlantOrDepotName(_.PlantId),
                                                        PlantOrDepotCode = _.PlantId == 0 ? GetPlantOrDepotCode(_.DepotId) : GetPlantOrDepotCode(_.PlantId),
                                                        SaudaConversionUpdateFromSap = _.SaudaConversionUpdateFromSap,
                                                        ReprocessStatus = _.IsApproved,
                                                        IsSapDataSync = _.IsSAPDataSync,
                                                        StatusId = _.StatusId
                                                    }).OrderByDescending(_ => _.ConversionCreatedDate).ToList();

            if ((saudaDetailsWithNullContext != null && saudaDetailsWithNullContext.Count > 0) || (saudaDetailsWithHeaderIdContext != null && saudaDetailsWithHeaderIdContext.Count > 0))
            {
                if (saudaDetailsWithNullContext != null && saudaDetailsWithNullContext.Count > 0)
                {
                    saudaConversionSkuDetails.AddRange(saudaDetailsWithNullContext);
                }
                if (saudaDetailsWithHeaderIdContext != null && saudaDetailsWithHeaderIdContext.Count > 0)
                {
                    var saudaDetailsWithHeaderContext = (from header in saudaDetailsWithHeaderIdContext
                                                         join child in saudaDetailsWithHeaderIdContext
                                                         on header.SkuConversionId equals child.SkuConversionHeaderId
                                                         where header.SkuConversionHeaderId == 0
                                                         select new SaudaConversionSkusDetail()
                                                         {
                                                             SkuConversionId = header.SkuConversionId,
                                                             SkuConversionHeaderId = header.SkuConversionHeaderId,
                                                             DealerId = header.DealerId,
                                                             SkuName = header.SkuName,
                                                             SaudaNumber = header.SaudaNumber + "-" + child.SaudaNumber,
                                                             SaudaQuantityInMT = header.SaudaQuantityInMT + child.SaudaQuantityInMT,
                                                             ConversionCreatedDate = header.ConversionCreatedDate,
                                                             ConversionModifiedDate = header.ConversionModifiedDate,
                                                             Remarks = header.Remarks,
                                                             SaudaQuantityInSku = header.SaudaQuantityInSku + child.SaudaQuantityInSku,
                                                             PlantOrDepotCode = header.PlantOrDepotCode,
                                                             PlantOrDepotName = header.PlantOrDepotName,
                                                             SaudaConversionUpdateFromSap = header.SaudaConversionUpdateFromSap,
                                                             ReprocessStatus = header.ReprocessStatus,
                                                             IsSapDataSync = header.IsSapDataSync,
                                                             StatusId = header.StatusId
                                                         }).ToList();

                    if (saudaDetailsWithHeaderContext != null && saudaDetailsWithHeaderContext.Count > 0)
                    {
                        saudaConversionSkuDetails.AddRange(saudaDetailsWithHeaderContext);
                    }
                }
            }
            return saudaConversionSkuDetails;
        }

        public ResultDto GetSaudaConversionSkuDetailsById(SaudaConversionSKUInputDto inputDto)
        {
            _methodName = "GetSaudaConversionSkuDetailsById";
            try
            {
                var result = new SaudaConversionDetailsBySkuId();
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.SaudaConversionId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.SaudaConversionSkuIdMissing);
                }
                var saudaConversionSkuContext = _emamiContext.SaudaConversionSkus.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SaudaConversionId);
                if (saudaConversionSkuContext == null)
                {
                    return _resultService.ErrorMessage(Constants.SaudaConversionSkuIdNotfound);
                }
                else
                {
                    result.SkuConversionId = saudaConversionSkuContext.Id;
                    result.ConversionCreatedDate = saudaConversionSkuContext.CreatedDate;
                    result.ConversionModifiedDate = saudaConversionSkuContext.ModifiedDate ?? DateTime.MinValue;
                    result.SaudaConversionStatus = (saudaConversionSkuContext.StatusId == (int)DTO.Enums.Status.Rejected) ? UtilityHelper.GetEnumDescription(DTO.Enums.Status.Rejected) : !saudaConversionSkuContext.IsApproved ? UtilityHelper.GetEnumDescription(DTO.Enums.Status.Pending) : UtilityHelper.GetEnumDescription(DTO.Enums.Status.Approved);
                    result.SaudaConversionStatusId = string.IsNullOrEmpty(saudaConversionSkuContext.SaudaNumber) ? (int)DTO.Enums.Status.Pending : (int)DTO.Enums.Status.Approved;

                    var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaConversionSkuContext.DealerId);
                    if (dealerContext != null)
                    {
                        result.DealerName = dealerContext.Name;
                        var bdoContext = (from user in _emamiContext.Users.AsNoTracking()
                                          join usermap in _emamiContext.UserCustomerMapping.AsNoTracking()
                                          on user.Id equals usermap.UserId
                                          join ur in _emamiContext.UserRoles.AsNoTracking()
                                          on user.Id equals ur.UserId
                                          where usermap.CustomerId == dealerContext.Id && ur.RoleId == (int)DTO.Enums.RoleType.StateTrader
                                          select user).ToList();
                        if (bdoContext != null & bdoContext.Count > 0)
                        {
                            result.BDOName = bdoContext.FirstOrDefault() != null ? bdoContext.FirstOrDefault().Name : string.Empty;
                        }

                    }
                    var SaudaConversionSkus = _emamiContext.SaudaConversionSkus.AsNoTracking().ToList();
                    var fromSkusList = SaudaConversionSkus
                                                    .Where(_ => _.SaudaConversionSkuHeaderId == saudaConversionSkuContext.Id || _.Id == saudaConversionSkuContext.Id)
                                                    .ToList()
                                                    .Select(_ => new SaudaConversionSkuDetailOutput
                                                    {
                                                        SaudaConversionId = _.Id,
                                                        SkuName = GetSKUName(_.SkuId),
                                                        BaseRate = _.BaseRate,
                                                        SaudaNumber = _.SaudaNumber,
                                                        SaudaQuantityInMT = _.QuantityInMt,
                                                        SaudaQuantityInSku = _.QuantityInSku,
                                                        Remarks = _.Remarks
                                                    }).ToList();
                    result.FromSkus.AddRange(fromSkusList);

                    foreach (var saudaConversionSku in fromSkusList)
                    {
                        var toSkusList = _emamiContext.SaudaConversionSkuDetails.AsNoTracking()
                                                      .Where(_ => _.SaudaConversionSkuId == saudaConversionSku.SaudaConversionId)
                                                      .ToList()
                                                      .Select(_ => new SaudaConversionSkuDetailOutput
                                                      {
                                                          SaudaConversionId = _.SaudaConversionSkuId,
                                                          SaudaConversionDetailId = _.Id,
                                                          BaseRate = _.ToBaseRate,
                                                          SaudaNumber = _.ToSaudaNumber,
                                                          SaudaQuantityInMT = _.ToQuantityInMt,
                                                          SaudaQuantityInSku = _.ToQuantityInSku,
                                                          SkuName = GetSKUName(_.ToSkuId),
                                                          Remarks = _.Remarks
                                                      }).ToList();
                        result.ToSkus.AddRange(toSkusList);
                    }
                }

                return SucessResult(result);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        private string GetSKUName(long SkuId)
        {
            var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == SkuId);
            if (skuContext != null)
            {
                return skuContext.SkuName;
            }
            return string.Empty;
        }

        private string GetPlantOrDepotName(long PlantorDepotId)
        {
            var depotContext = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == PlantorDepotId);
            if (depotContext != null)
            {
                return depotContext.Name;
            }
            return string.Empty;
        }

        private string GetPlantOrDepotCode(long PlantorDepotId)
        {
            var depotContext = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == PlantorDepotId);
            if (depotContext != null)
            {
                return depotContext.Code;
            }
            return string.Empty;
        }

        private string GetUserName(long UserId)
        {
            if (UserId != 0)
            {
                var usercontext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == UserId);
                if (usercontext != null)
                {
                    return usercontext.Name;
                }
            }
            return string.Empty;
        }

        private SaudaConversionSkuBDOUserList GetBDONameFromDealerId(long UserId)
        {
            var bdoUserDetails = new SaudaConversionSkuBDOUserList();
            if (UserId != 0)
            {
                var usercustomerContext = _emamiContext.UserCustomerMapping.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), ucm => ucm.UserId, ur => ur.UserId, (ucm, ur) => new { ucm, ur }).FirstOrDefault(_ => _.ucm.CustomerId == UserId && _.ur.RoleId == (int)DTO.Enums.RoleType.StateTrader);
                if (usercustomerContext != null)
                {
                    var bdoName = GetUserName(usercustomerContext.ucm.UserId);

                    bdoUserDetails.BDOId = usercustomerContext.ucm.UserId;
                    bdoUserDetails.BDOName = bdoName;
                }
            }
            return bdoUserDetails;
        }

        private string GetZonalHeadNameFromBDOId(long UserId)
        {
            var bdoUserContext = (from StateTrader in _emamiContext.Users.AsNoTracking()
                                  join ZonalTrader in _emamiContext.Users.AsNoTracking()
                                  on StateTrader.ReportingToId equals ZonalTrader.Id
                                  where StateTrader.Id == UserId
                                  select ZonalTrader.Name).FirstOrDefault();

            return bdoUserContext ?? string.Empty;
        }

        public ResultDto GetSaudaConversionUnitAndBaseRateList(SaudaConversionUnitAndDiffRateInputDto inputDto)
        {
            _methodName = "GetSaudaConversionUnitAndBaseRateList";
            var resultDto = new ResultDto();
            try
            {
                var result = new List<SaudaConversionUnitAndDiffRateDto>();
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.LoginUserId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }

                result = _emamiContext.SaudaConversionUnitAndDifferenceRates.AsNoTracking()
                    .Join(_emamiContext.SaudaConversionUnitAndDifferenceRateDetails.AsNoTracking(), s => s.Id, sd => sd.SaudaConversionUnitAndDifferenceRateId, (s, sd) => new { sauda = s, saudaDetail = sd })
                    .Join(_emamiContext.Skus.AsNoTracking(), x => x.sauda.FromSkuId, sku => sku.Id, (x, sku) => new { SaudaConversion = x, sku })
                    .Where(_ => DbFunctions.TruncateTime(inputDto.Fromdate) <= DbFunctions.TruncateTime(_.SaudaConversion.sauda.FromDate) && DbFunctions.TruncateTime(_.SaudaConversion.sauda.FromDate) <= DbFunctions.TruncateTime(inputDto.Todate) &&
                    DbFunctions.TruncateTime(inputDto.Fromdate) <= DbFunctions.TruncateTime(_.SaudaConversion.sauda.ToDate) && DbFunctions.TruncateTime(_.SaudaConversion.sauda.ToDate) <= DbFunctions.TruncateTime(inputDto.Todate) && (inputDto.VerticalId > 0 ? _.sku.DivisionId == inputDto.VerticalId : _.sku.DivisionId > 0))
                    .Select(_ => new SaudaConversionUnitAndDiffRateDto()
                    {
                        ConversionId = _.SaudaConversion.saudaDetail.Id,
                        FromPackGroupId = _.SaudaConversion.sauda.FromPackGroupId,
                        FromSkuId = _.SaudaConversion.sauda.FromSkuId,
                        FromUnit = _.SaudaConversion.sauda.FromUnit,
                        ValidFrom = _.SaudaConversion.sauda.FromDate,
                        ValidTo = _.SaudaConversion.sauda.ToDate,
                        ToPackGroupId = _.SaudaConversion.saudaDetail.ToPackGroupId,
                        ToSkuId = _.SaudaConversion.saudaDetail.ToSkuId,
                        Unit = _.SaudaConversion.saudaDetail.ToUnit,
                        BasicRate = _.SaudaConversion.saudaDetail.BasicRate,
                        IsActive = _.SaudaConversion.saudaDetail.IsActive,
                        SourceId = _.SaudaConversion.sauda.SourceId,
                        StateId = _.SaudaConversion.sauda.StateId
                    }).ToList();

                var packgroupList = _emamiContext.OilPackingTypes.AsNoTracking();
                var skusList = _emamiContext.Skus.AsNoTracking();
                var stateList = _emamiContext.State.AsNoTracking();
                var plantOrDepotList = _emamiContext.Depots.AsNoTracking();
                foreach (var item in result)
                {
                    item.FromPackGroup = packgroupList.FirstOrDefault(_ => _.Id == item.FromPackGroupId)?.Name;
                    item.ToPackGroup = packgroupList.FirstOrDefault(_ => _.Id == item.ToPackGroupId)?.Name;

                    var fromSku = skusList.FirstOrDefault(_ => _.Id == item.FromSkuId);
                    item.FromSku = fromSku != null ? fromSku.SkuName : string.Empty;
                    item.FromSkuCode = fromSku != null ? fromSku.SkuCode : string.Empty;

                    var toSku = skusList.FirstOrDefault(_ => _.Id == item.ToSkuId);
                    item.ToSku = toSku != null ? toSku.SkuName : string.Empty;
                    item.ToSkuCode = toSku != null ? toSku.SkuCode : string.Empty;

                    var state = stateList.FirstOrDefault(_ => _.Id == item.StateId);
                    item.State = state != null ? state.StateName : string.Empty;

                    var source = plantOrDepotList.FirstOrDefault(_ => _.Id == item.SourceId);
                    item.Source = source != null ? source.Name : string.Empty;
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = result;
                return resultDto;
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto GetSaudaConversionReport(SaudaConversionReportInputDto inputDto)
        {
            _methodName = "GetSaudaConversionReport";
            try
            {
                var result = new List<SaudaConversionDetailsBySkuId>();
                var SaudaConversionDetailsList = new List<SaudaConversionSku>();
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }
                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }
                if (inputDto.StatusIds.Count == 0)
                {
                    var SaudaConversionDetailswithSkuList = _emamiContext.SaudaConversionSkus.AsNoTracking()
                                                                 .Join(_emamiContext.Skus.AsNoTracking(), so => so.SkuId, sku => sku.Id, (so, sku) => new { so, sku })
                                                                .Where(_ =>
                                                                DbFunctions.TruncateTime(_.so.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                DbFunctions.TruncateTime(_.so.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                                                                .OrderByDescending(_ => _.so.CreatedDate).Select(s => new { s.so, s.sku }).ToList();
                    if (inputDto.VerticalId > 0)
                    {
                        SaudaConversionDetailsList = SaudaConversionDetailswithSkuList.Where(_ => _.sku.DivisionId == inputDto.VerticalId && _.sku.SalesOrganizationId == inputDto.SalesOrganizationId && _.sku.DistributionChannelId == inputDto.DistributionChannelId).Select(a => a.so).ToList();
                    }
                    else
                    {
                        SaudaConversionDetailsList = SaudaConversionDetailswithSkuList.Select(a => a.so).ToList();
                    }
                    if (SaudaConversionDetailsList != null && SaudaConversionDetailsList.Count > 0)
                    {
                        var saudaConversionDetailsList = GetSkuConversionDetails(SaudaConversionDetailsList);

                        foreach (var saudaDetail in saudaConversionDetailsList)
                        {
                            var bdoDetail = GetBDONameFromDealerId(saudaDetail.DealerId);
                            var detail = new SaudaConversionDetailsBySkuId()
                            {
                                ZonalHeadName = GetZonalHeadNameFromBDOId(bdoDetail.BDOId),
                                BDOName = bdoDetail.BDOName,
                                DealerName = GetUserName(saudaDetail.DealerId),
                                SkuConversionId = saudaDetail.SkuConversionId,
                                ConversionCreatedDate = saudaDetail.ConversionCreatedDate,
                                SkuName = saudaDetail.SkuName,
                                SaudaQuantityInMT = saudaDetail.SaudaQuantityInMT,
                                Remarks = saudaDetail.Remarks,
                                SaudaQuantityInSku = saudaDetail.SaudaQuantityInSku,
                                PlantOrDepotCode = saudaDetail.PlantOrDepotCode,
                                PlantOrDepotName = saudaDetail.PlantOrDepotName
                            };
                            var SaudaConversionSkus = _emamiContext.SaudaConversionSkus.AsNoTracking().ToList();
                            var fromSkusList = SaudaConversionSkus
                                                            .Where(_ => _.SaudaConversionSkuHeaderId == detail.SkuConversionId || _.Id == detail.SkuConversionId)
                                                            .ToList()
                                                            .Select(_ => new SaudaConversionSkuDetailOutput
                                                            {
                                                                SaudaConversionId = _.Id,
                                                                SkuName = GetSKUName(_.SkuId),
                                                                BaseRate = _.BaseRate,
                                                                SaudaNumber = _.SaudaNumber,
                                                                SaudaQuantityInMT = _.QuantityInMt,
                                                                SaudaQuantityInSku = _.QuantityInSku,
                                                                Remarks = _.Remarks
                                                            }).ToList();
                            detail.FromSkus.AddRange(fromSkusList);

                            foreach (var saudaConversionSku in fromSkusList)
                            {
                                var toSkusList = _emamiContext.SaudaConversionSkuDetails.AsNoTracking()
                                                              .Where(_ => _.SaudaConversionSkuId == saudaConversionSku.SaudaConversionId)
                                                              .ToList()
                                                              .Select(_ => new SaudaConversionSkuDetailOutput
                                                              {
                                                                  SaudaConversionId = _.SaudaConversionSkuId,
                                                                  SaudaConversionDetailId = _.Id,
                                                                  BaseRate = _.ToBaseRate,
                                                                  SaudaNumber = _.ToSaudaNumber,
                                                                  SaudaQuantityInMT = _.ToQuantityInMt,
                                                                  SaudaQuantityInSku = _.ToQuantityInSku,
                                                                  SkuName = GetSKUName(_.ToSkuId),
                                                                  Remarks = _.Remarks
                                                              }).ToList();
                                detail.ToSkus.AddRange(toSkusList);
                            }
                            result.Add(detail);
                        }
                    }
                }
                if (inputDto.StatusIds.Contains((int)DTO.Enums.Status.Pending))
                {
                    //Get Pending Dealer SaudaConversions Sku from table
                    var PendingSaudaConversionDetailswithSkusList = _emamiContext.SaudaConversionSkus.AsNoTracking()
                                                                 .Join(_emamiContext.Skus.AsNoTracking(), so => so.SkuId, sku => sku.Id, (so, sku) => new { so, sku })
                                                                .Where(_ => string.IsNullOrEmpty(_.so.SaudaNumber) && string.IsNullOrEmpty(_.so.Remarks) &&
                                                                DbFunctions.TruncateTime(_.so.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                DbFunctions.TruncateTime(_.so.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                                                                .OrderByDescending(_ => _.so.CreatedDate).Select(s => new { s.so, s.sku }).ToList();

                    if (inputDto.VerticalId > 0)
                    {
                        SaudaConversionDetailsList = PendingSaudaConversionDetailswithSkusList.Where(_ => _.sku.DivisionId == inputDto.VerticalId && _.sku.SalesOrganizationId == inputDto.SalesOrganizationId && _.sku.DistributionChannelId == inputDto.DistributionChannelId).Select(a => a.so).ToList();
                    }
                    else
                    {
                        SaudaConversionDetailsList = PendingSaudaConversionDetailswithSkusList.Select(a => a.so).ToList();
                    }

                    if (SaudaConversionDetailsList != null && SaudaConversionDetailsList.Count > 0)
                    {
                        var pendingSaudaConversionList = GetSkuConversionDetails(SaudaConversionDetailsList);

                        foreach (var saudaDetail in pendingSaudaConversionList)
                        {
                            var bdoDetail = GetBDONameFromDealerId(saudaDetail.DealerId);
                            var detail = new SaudaConversionDetailsBySkuId()
                            {
                                ZonalHeadName = GetZonalHeadNameFromBDOId(bdoDetail.BDOId),
                                BDOName = bdoDetail.BDOName,
                                DealerName = GetUserName(saudaDetail.DealerId),
                                SkuConversionId = saudaDetail.SkuConversionId,
                                ConversionCreatedDate = saudaDetail.ConversionCreatedDate,
                                SkuName = saudaDetail.SkuName,
                                SaudaQuantityInMT = saudaDetail.SaudaQuantityInMT,
                                Remarks = saudaDetail.Remarks,
                                SaudaQuantityInSku = saudaDetail.SaudaQuantityInSku,
                                PlantOrDepotCode = saudaDetail.PlantOrDepotCode,
                                PlantOrDepotName = saudaDetail.PlantOrDepotName
                            };
                            var SaudaConversionSkus = _emamiContext.SaudaConversionSkus.AsNoTracking().ToList();
                            var fromSkusList = SaudaConversionSkus
                                                            .Where(_ => _.SaudaConversionSkuHeaderId == detail.SkuConversionId || _.Id == detail.SkuConversionId)
                                                            .ToList()
                                                            .Select(_ => new SaudaConversionSkuDetailOutput
                                                            {
                                                                SaudaConversionId = _.Id,
                                                                SkuName = GetSKUName(_.SkuId),
                                                                BaseRate = _.BaseRate,
                                                                SaudaNumber = _.SaudaNumber,
                                                                SaudaQuantityInMT = _.QuantityInMt,
                                                                SaudaQuantityInSku = _.QuantityInSku,
                                                                Remarks = _.Remarks
                                                            }).ToList();
                            detail.FromSkus.AddRange(fromSkusList);

                            foreach (var saudaConversionSku in fromSkusList)
                            {
                                var toSkusList = _emamiContext.SaudaConversionSkuDetails.AsNoTracking()
                                                              .Where(_ => _.SaudaConversionSkuId == saudaConversionSku.SaudaConversionId)
                                                              .ToList()
                                                              .Select(_ => new SaudaConversionSkuDetailOutput
                                                              {
                                                                  SaudaConversionId = _.SaudaConversionSkuId,
                                                                  SaudaConversionDetailId = _.Id,
                                                                  BaseRate = _.ToBaseRate,
                                                                  SaudaNumber = _.ToSaudaNumber,
                                                                  SaudaQuantityInMT = _.ToQuantityInMt,
                                                                  SaudaQuantityInSku = _.ToQuantityInSku,
                                                                  SkuName = GetSKUName(_.ToSkuId),
                                                                  Remarks = _.Remarks
                                                              }).ToList();
                                detail.ToSkus.AddRange(toSkusList);
                            }
                            result.Add(detail);
                        }
                    }
                }
                else if (inputDto.StatusIds.Contains((int)DTO.Enums.Status.Approved))
                {
                    //Get Approved Dealer SaudaConversions Sku from table
                    var ApprovedSaudaConversionDetailsList = _emamiContext.SaudaConversionSkus.AsNoTracking()
                                                                .Join(_emamiContext.Skus.AsNoTracking(), so => so.SkuId, sku => sku.Id, (so, sku) => new { so, sku })
                                                                .Where(_ => /*!string.IsNullOrEmpty(_.SaudaNumber) &&*/ !string.IsNullOrEmpty(_.so.Remarks) &&
                                                                DbFunctions.TruncateTime(_.so.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                DbFunctions.TruncateTime(_.so.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                                                                .OrderByDescending(_ => _.so.CreatedDate).Select(s => new { s.so, s.sku }).ToList();
                    if (inputDto.VerticalId > 0)
                    {
                        SaudaConversionDetailsList = ApprovedSaudaConversionDetailsList.Where(_ => _.sku.DivisionId == inputDto.VerticalId && _.sku.SalesOrganizationId == inputDto.SalesOrganizationId && _.sku.DistributionChannelId == inputDto.DistributionChannelId).Select(a => a.so).ToList();
                    }
                    else
                    {
                        SaudaConversionDetailsList = ApprovedSaudaConversionDetailsList.Select(a => a.so).ToList();
                    }


                    if (ApprovedSaudaConversionDetailsList != null && ApprovedSaudaConversionDetailsList.Count > 0)
                    {
                        var approvedSaudaConversionList = GetSkuConversionDetails(SaudaConversionDetailsList);

                        foreach (var saudaDetail in approvedSaudaConversionList)
                        {
                            var bdoDetail = GetBDONameFromDealerId(saudaDetail.DealerId);
                            var detail = new SaudaConversionDetailsBySkuId()
                            {
                                ZonalHeadName = GetZonalHeadNameFromBDOId(bdoDetail.BDOId),
                                BDOName = bdoDetail.BDOName,
                                DealerName = GetUserName(saudaDetail.DealerId),
                                SkuConversionId = saudaDetail.SkuConversionId,
                                ConversionCreatedDate = saudaDetail.ConversionCreatedDate,
                                SkuName = saudaDetail.SkuName,
                                SaudaQuantityInMT = saudaDetail.SaudaQuantityInMT,
                                Remarks = saudaDetail.Remarks,
                                SaudaQuantityInSku = saudaDetail.SaudaQuantityInSku,
                                PlantOrDepotCode = saudaDetail.PlantOrDepotCode,
                                PlantOrDepotName = saudaDetail.PlantOrDepotName
                            };

                            var SaudaConversionSkus = _emamiContext.SaudaConversionSkus.AsNoTracking().ToList();
                            var fromSkusList = SaudaConversionSkus
                                                            .Where(_ => _.SaudaConversionSkuHeaderId == detail.SkuConversionId || _.Id == detail.SkuConversionId)
                                                            .ToList()
                                                            .Select(_ => new SaudaConversionSkuDetailOutput
                                                            {
                                                                SaudaConversionId = _.Id,
                                                                SkuName = GetSKUName(_.SkuId),
                                                                BaseRate = _.BaseRate,
                                                                SaudaNumber = _.SaudaNumber,
                                                                SaudaQuantityInMT = _.QuantityInMt,
                                                                SaudaQuantityInSku = _.QuantityInSku,
                                                                Remarks = _.Remarks
                                                            }).ToList();
                            detail.FromSkus.AddRange(fromSkusList);

                            foreach (var saudaConversionSku in fromSkusList)
                            {
                                var toSkusList = _emamiContext.SaudaConversionSkuDetails.AsNoTracking()
                                                              .Where(_ => _.SaudaConversionSkuId == saudaConversionSku.SaudaConversionId)
                                                              .ToList()
                                                              .Select(_ => new SaudaConversionSkuDetailOutput
                                                              {
                                                                  SaudaConversionId = _.SaudaConversionSkuId,
                                                                  SaudaConversionDetailId = _.Id,
                                                                  BaseRate = _.ToBaseRate,
                                                                  SaudaNumber = _.ToSaudaNumber,
                                                                  SaudaQuantityInMT = _.ToQuantityInMt,
                                                                  SaudaQuantityInSku = _.ToQuantityInSku,
                                                                  SkuName = GetSKUName(_.ToSkuId),
                                                                  Remarks = _.Remarks
                                                              }).ToList();
                                detail.ToSkus.AddRange(toSkusList);
                            }
                            result.Add(detail);
                        }
                    }
                }

                return SucessResult(result);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }

        }

        public ResultDto GetSaudaConversionUnitAndBaseRateListForAdminApp(SaudaConversionUnitAndDiffRateInputDto inputDto)
        {
            _methodName = "GetSaudaConversionUnitAndBaseRateListForAdminApp";
            var resultDto = new ResultDto();
            try
            {
                var result = new List<SaudaConversionUnitAndDiffRateDto>();
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.LoginUserId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }

                var pageSize = Constants.PageSize;
                long skip = pageSize * inputDto.PageNo;

                result = _emamiContext.SaudaConversionUnitAndDifferenceRates.AsNoTracking()
                    .Join(_emamiContext.SaudaConversionUnitAndDifferenceRateDetails.AsNoTracking(), s => s.Id, sd => sd.SaudaConversionUnitAndDifferenceRateId, (s, sd) => new { sauda = s, saudaDetail = sd })
                    .Join(_emamiContext.Skus.AsNoTracking(), x => x.sauda.FromSkuId, sku => sku.Id, (x, sku) => new { SaudaConversion = x, sku })
                    .Where(_ => DbFunctions.TruncateTime(inputDto.Fromdate) <= DbFunctions.TruncateTime(_.SaudaConversion.sauda.FromDate) && DbFunctions.TruncateTime(_.SaudaConversion.sauda.FromDate) <= DbFunctions.TruncateTime(inputDto.Todate) &&
                    DbFunctions.TruncateTime(inputDto.Fromdate) <= DbFunctions.TruncateTime(_.SaudaConversion.sauda.ToDate) && DbFunctions.TruncateTime(_.SaudaConversion.sauda.ToDate) <= DbFunctions.TruncateTime(inputDto.Todate) && (inputDto.VerticalId > 0 ? (_.sku.DivisionId == inputDto.VerticalId && _.sku.SalesOrganizationId == inputDto.SalesOrganizationId && _.sku.DistributionChannelId == inputDto.DistributionChannelId) : _.sku.DivisionId > 0))
                    .Select(_ => new SaudaConversionUnitAndDiffRateDto()
                    {
                        ConversionId = _.SaudaConversion.saudaDetail.Id,
                        FromSkuId = _.SaudaConversion.sauda.FromSkuId,
                        FromUnit = _.SaudaConversion.sauda.FromUnit,
                        ValidFrom = _.SaudaConversion.sauda.FromDate,
                        ValidTo = _.SaudaConversion.sauda.ToDate,
                        ToSkuId = _.SaudaConversion.saudaDetail.ToSkuId,
                        Unit = _.SaudaConversion.saudaDetail.ToUnit,
                        BasicRate = _.SaudaConversion.saudaDetail.BasicRate,
                        IsActive = _.SaudaConversion.saudaDetail.IsActive,
                    }).OrderByDescending(_ => _.ConversionId).Skip((int)skip).Take(pageSize).ToList();

                var packgroupList = _emamiContext.OilPackingTypes.AsNoTracking();
                var skusList = _emamiContext.Skus.AsNoTracking();
                var stateList = _emamiContext.State.AsNoTracking();
                var plantOrDepotList = _emamiContext.Depots.AsNoTracking();
                foreach (var item in result)
                {
                    var fromSku = skusList.FirstOrDefault(_ => _.Id == item.FromSkuId);
                    if (fromSku != null)
                        item.FromSku = string.Concat(fromSku.SkuName, "-", fromSku.SkuCode);

                    var toSku = skusList.FirstOrDefault(_ => _.Id == item.ToSkuId);
                    if (toSku != null)
                        item.FromSku = string.Concat(toSku.SkuName, "-", toSku.SkuCode);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = result;
                return resultDto;
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }
        #endregion

        #region Get Sap Sync Pending Sauda Conversion List

        public ResultDto GetSapSyncPendingSaudaConversionList(SaudaConversionInputDto inputDto)
        {
            _methodName = "GetSapSyncPendingSaudaConversionList";
            try
            {
                var result = new SaudaConversionSKUStatusListDto();
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.LoginUserId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                List<SaudaConversionSkusDetail> saudaConversionSkusDetails = new List<SaudaConversionSkusDetail>();
                List<Data.Entities.User> userContext = _emamiContext.Users.ToList();

                if (inputDto.BdoIds.IsAny())
                {
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => inputDto.BdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    userContext = userContext.Where(_ => dealerIds.Contains(_.Id)).ToList();
                }
                else if (inputDto.ZonalHeadIds.IsAny())
                {
                    var bdoIds = userContext.Where(_ => _.ReportingToId != null && inputDto.ZonalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    userContext = userContext.Where(_ => dealerIds.Contains(_.Id)).ToList();
                }
                else if (inputDto.NationalHeadIds.IsAny())
                {
                    var zonalHeadIds = userContext.Where(_ => _.ReportingToId != null && inputDto.NationalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var bdoIds = userContext.Where(_ => _.ReportingToId != null && zonalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    userContext = userContext.Where(_ => dealerIds.Contains(_.Id)).ToList();
                }

                //Get Pending Dealer SaudaConversions Sku from table
                var PendingSaudaConversionDetailsList = _emamiContext.SaudaConversionSkus.AsNoTracking()
                    .Join(_emamiContext.Skus.AsNoTracking(), so => so.SkuId, sku => sku.Id, (so, sku) => new { so, sku })
                    .Where(_ => !_.so.IsApproved && _.so.StatusId == (int)DTO.Enums.Status.Pending
                    && (inputDto.VerticalId > 0 ? (_.sku.DivisionId == inputDto.VerticalId) : _.sku.DivisionId > 0))
                    .OrderByDescending(_ => _.so.CreatedDate).Select(s => s.so).ToList();
                if (PendingSaudaConversionDetailsList != null && PendingSaudaConversionDetailsList.Count > 0)
                {
                    var pendingSaudaConversionList = GetSkuConversionDetails(PendingSaudaConversionDetailsList);
                    pendingSaudaConversionList = pendingSaudaConversionList.Where(_ => _.SaudaConversionUpdateFromSap).ToList();

                    foreach (var saudaDetail in pendingSaudaConversionList)
                    {
                        var bdoDetail = GetBDONameFromDealerId(saudaDetail.DealerId);
                        var detail = new SaudaConversionSkusDetail()
                        {
                            ZonalHeadName = GetZonalHeadNameFromBDOId(bdoDetail.BDOId),
                            BdoName = bdoDetail.BDOName,
                            DealerName = GetUserName(saudaDetail.DealerId),
                            SkuConversionId = saudaDetail.SkuConversionId,
                            ConversionCreatedDate = saudaDetail.ConversionCreatedDate,
                            SkuName = saudaDetail.SkuName,
                            SaudaQuantityInMT = saudaDetail.SaudaQuantityInMT,
                            Remarks = saudaDetail.Remarks,
                            SaudaQuantityInSku = saudaDetail.SaudaQuantityInSku,
                            PlantOrDepotCode = saudaDetail.PlantOrDepotCode,
                            PlantOrDepotName = saudaDetail.PlantOrDepotName,
                            SaudaConversionUpdateFromSap = saudaDetail.SaudaConversionUpdateFromSap,
                            ReprocessStatus = saudaDetail.ReprocessStatus,
                            IsSapDataSync = saudaDetail.IsSapDataSync,
                            StatusId = saudaDetail.StatusId
                        };
                        saudaConversionSkusDetails.Add(detail);
                    }
                }

                return SucessResult(saudaConversionSkusDetails);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion

        public ResultDto GetSaudaConversionListMobile(SaudaConversionInputDTO inputDto)
        {
            _methodName = "GetSaudaConversionListMobile";
            var resultDto = new ResultDto();
            var outputDto = new SaudaConversionMobileOutputDTO();
            outputDto.SaudaConversionList = new List<SaudaConversionMobileListDTO>();
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
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                bool IsSapSyncReceivedFoSaudaConversionUpdate = false;
                string remarks = string.Empty;
                var description = UtilityHelper.GetEnumDescription(DTO.Enums.Configuration.InboundInterfacenotSyncedToSAPMinutes);
                var configurationContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Key == description).Value;

                var saudaConversionList = _emamiContext.SaudaConversionSkus.AsNoTracking()
                    .Join(_emamiContext.Skus.AsNoTracking(), so => so.SkuId, sku => sku.Id, (so, sku) => new { so, sku })
                    .Where(_ => (inputDto.VerticalId > 0 ? _.sku.DivisionId == inputDto.VerticalId : _.sku.DivisionId > 0))
                    .OrderByDescending(_ => _.so.CreatedDate).Select(s => s.so);

                if (inputDto.FromDate != null && inputDto.ToDate != null)
                {
                    saudaConversionList = saudaConversionList
                        .Where(_ => DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                        DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate));
                }
                IQueryable<Data.Entities.User> userContext = _emamiContext.Users.Where(_ => _.IsActive);

                if (inputDto.DealerIds != null && inputDto.DealerIds.Any())
                {
                    saudaConversionList = saudaConversionList.Where(_ => inputDto.DealerIds.Contains(_.DealerId));
                }
                else if (inputDto.BdoIds.IsAny())
                {
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => inputDto.BdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    saudaConversionList = saudaConversionList.Where(_ => dealerIds.Contains(_.DealerId));
                }
                else if (inputDto.ZonalHeadIds.IsAny())
                {
                    var bdoIds = userContext.Where(_ => _.ReportingToId != null && inputDto.ZonalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    saudaConversionList = saudaConversionList.Where(_ => dealerIds.Contains(_.DealerId));
                }
                else if (inputDto.NationalHeadIds.IsAny())
                {
                    var zonalHeadIds = userContext.Where(_ => _.ReportingToId != null && inputDto.NationalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var bdoIds = userContext.Where(_ => _.ReportingToId != null && zonalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    saudaConversionList = saudaConversionList.Where(_ => dealerIds.Contains(_.DealerId));
                }

                if (inputDto.StatusId == (int)DTO.Enums.Status.Pending)
                {
                    saudaConversionList = saudaConversionList
                        .Where(_ => !_.IsApproved && _.StatusId != (int)DTO.Enums.Status.Rejected);
                }

                if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                {
                    saudaConversionList = saudaConversionList
                        .Where(_ => _.IsApproved && _.StatusId != (int)DTO.Enums.Status.Rejected);
                }

                if (inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                {
                    saudaConversionList = saudaConversionList.Where(_ => _.StatusId == (int)DTO.Enums.Status.Rejected);
                }

                var pageSize = Constants.PageSize;
                var skip = pageSize * inputDto.PageNo;


                var saudaConversionListFinal = GetSkuConversionDetails(saudaConversionList.ToList());

                outputDto.ListCount = saudaConversionListFinal.Count();
                var saudaConversionLists = saudaConversionListFinal.OrderByDescending(_ => _.SkuConversionId).Skip(skip).Take(pageSize).ToList();

                foreach (var saudaDetail in saudaConversionLists)
                {
                    TimeSpan difference = currentDate.Subtract(Convert.ToDateTime(saudaDetail.ConversionModifiedDate));

                    if (difference.TotalMinutes > Convert.ToDouble(configurationContext) && saudaDetail.IsSapDataSync)
                    {
                        if (saudaDetail.SaudaConversionUpdateFromSap)
                        {
                            IsSapSyncReceivedFoSaudaConversionUpdate = true;
                            remarks = saudaDetail.Remarks;
                        }
                        else
                        {
                            IsSapSyncReceivedFoSaudaConversionUpdate = false;
                            remarks = "Sauda Conversion Sync From Sap not Received";
                        }
                    }
                    else
                    {
                        IsSapSyncReceivedFoSaudaConversionUpdate = saudaDetail.SaudaConversionUpdateFromSap;
                        remarks = saudaDetail.Remarks;
                    }
                    var bdoDetail = GetBDONameFromDealerId(saudaDetail.DealerId);
                    var detail = new SaudaConversionMobileListDTO()
                    {
                        ZonalHeadName = GetZonalHeadNameFromBDOId(bdoDetail.BDOId),
                        BdoName = bdoDetail.BDOName,
                        DealerName = GetUserName(saudaDetail.DealerId),
                        SkuConversionId = saudaDetail.SkuConversionId,
                        ConversionCreatedDate = saudaDetail.ConversionCreatedDate,
                        SkuName = saudaDetail.SkuName,
                        SaudaQuantityInMT = saudaDetail.SaudaQuantityInMT,
                        Remarks = remarks,
                        SaudaQuantityInSku = saudaDetail.SaudaQuantityInSku,
                        PlantOrDepotCode = saudaDetail.PlantOrDepotCode,
                        PlantOrDepotName = saudaDetail.PlantOrDepotName,
                        SaudaConversionUpdateFromSap = IsSapSyncReceivedFoSaudaConversionUpdate,
                        ReprocessStatus = saudaDetail.ReprocessStatus,
                        IsSapDataSync = saudaDetail.IsSapDataSync,
                        StatusId = saudaDetail.StatusId,
                        IsReprocessed = (saudaDetail.StatusId == 0 && IsSapSyncReceivedFoSaudaConversionUpdate && !saudaDetail.ReprocessStatus)
                        || (saudaDetail.StatusId == 0 && !saudaDetail.ReprocessStatus && !saudaDetail.IsSapDataSync && !string.IsNullOrEmpty(remarks))
                        || (saudaDetail.StatusId == 0 && saudaDetail.IsSapDataSync && !saudaDetail.SaudaConversionUpdateFromSap && !string.IsNullOrEmpty(remarks)) ? false : true,
                    };
                    outputDto.SaudaConversionList.Add(detail);
                }


                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
                return resultDto;

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }


        #region CompetitorAnalysis - Price Discovery

        /// <summary>
        /// Method to Save CompetitorAnalysis
        /// </summary>
        /// <param name="employeeDto"></param>
        /// <returns></returns>
        public ResultDto SaveCompetitorAnalysis(CompetitorAnalysisAddDto inputDto)
        {
            _methodName = "SaveCompetitorAnalysis";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                var requestTo = 0L;
                var users = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == inputDto.LoginUserId);
                if (users != null && users.Any())
                {
                    var requestedTo = users.FirstOrDefault()?.ReportingToId;
                    if (requestedTo != null)
                    {
                        requestTo = (long)requestedTo;
                    }
                    else
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.InvalidRequestToUser;
                        return resultDto;
                    }
                }

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

        #endregion


        public ResultDto GetContractNumberList(ContractNoInputDto inputDto)
        {
            _methodName = "GetContractNumberList";
            var resultDto = new ResultDto();
            var result = new List<ContractNoListDto>();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.DealerId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.DealerIdEmpty);
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                var pendingContext = _emamiContext.PendingContracts.AsNoTracking();
                var skucontextCode = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id==inputDto.SkuId)!=null ? _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SkuId).SkuCode:string.Empty;

                _logger.Info($"{skucontextCode}");
                var saudaList = _emamiContext.PendingContracts.AsNoTracking()
                     .Join(_emamiContext.Sauda.AsNoTracking(), p => p.SaudaNumber, sa => sa.SaudaNumber, (p, sa) => new { p, sa })
                     .Where(_ => _.p.SalesOrgId == _.sa.SalesOrganizationId && _.p.DistChnlId == _.sa.DistributionChannelId && _.p.DivisionId == _.sa.DivisionId && inputDto.DealerId == _.p.UserId && _.p.MaterialCode==skucontextCode)
                     .Select(s => s.sa);

                //var saudaList = _emamiContext.Sauda.AsNoTracking()
                //    .Where(_ => _.UserId == inputDto.DealerId && _.StatusId == (int)DTO.Enums.Status.Approved && _.SaudaNumber != null);

                if (inputDto.SalesOrganizationId != 0)
                {
                    saudaList = saudaList
                    .Where(_ => _.SaudaNumber != null && _.SalesOrganizationId == inputDto.SalesOrganizationId);
                }

                var saudaIds = new List<long>();
                if (ConsoleSettings.SalesOrderDateCheck)
                {
                    var date = DateTime.Parse(ConsoleSettings.SalesOrderDate);
                    saudaIds = saudaList.Where(s => DbFunctions.TruncateTime(s.BiddingDate) >= DbFunctions.TruncateTime(date)).Select(_ => _.Id).ToList();
                }
                else
                {
                    saudaIds = saudaList.Select(_ => _.Id).ToList();
                }

                var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking()
                    .Where(_ => saudaIds.Contains(_.SaudaId) && _.SkuId == inputDto.SkuId).ToList();

                var saudaIdsApproved = saudaOrderContext.Select(_ => _.SaudaId).Distinct().ToList();

                var saudas = saudaList.Where(_ => saudaIdsApproved.Contains(_.Id)).OrderBy(_ => _.Id).ToList();
                if (inputDto.SkuId == 0)
                {
                    return _resultService.ErrorMessage(Constants.SkuEmpty);
                }
                if (saudas.IsAny())
                {
                    var qtyInCase = Convert.ToDecimal(0.99);
                    var saudaresult = saudas
                        .Select(_ => new ContractNoListDto
                        {
                            Id = _.Id !=null ? _.Id : 0,
                            UserId = _.Id!=null ? _.UserId:0,
                            SaudaNumber = _.SaudaNumber != null && _.BiddingDate != null ? _.SaudaNumber + "-" + _.BiddingDate.ToString("dd/MMM/yyyy") : string.Empty,
                            //AvailableQuantity = _resultService.SaudaAvailableQuantityCheck(_.SaudaNumber, inputDto.SkuId),
                            AvailableQuantity = _.SaudaNumber !=null ? pendingContext.FirstOrDefault(p => p.SaudaNumber ==_.SaudaNumber && p.MaterialCode== skucontextCode) !=null ? pendingContext.FirstOrDefault(p => p.SaudaNumber == _.SaudaNumber && p.MaterialCode == skucontextCode).PendingQuantityInCase:0:0,
                            soOpenQuantity = _.SaudaNumber !=null ? pendingContext.FirstOrDefault(p => p.SaudaNumber ==_.SaudaNumber && p.MaterialCode== skucontextCode) !=null ? pendingContext.FirstOrDefault(p => p.SaudaNumber == _.SaudaNumber && p.MaterialCode == skucontextCode).OpenSalesOrderQuantity:0:0,
                            SaudaOrderId = saudaOrderContext.FirstOrDefault(s => s.SaudaId == _.Id && s.SkuId == inputDto.SkuId) != null ? saudaOrderContext.FirstOrDefault(s => s.SaudaId == _.Id && s.SkuId == inputDto.SkuId).Id : 0,
                            SalesOrganizationId = _.SalesOrganizationId,
                            DistributionChannelId = _.DistributionChannelId,
                            DivisionId = _.DivisionId
                        }).ToList();
                    result = saudaresult.Where(_ => _.AvailableQuantity > qtyInCase).Select(s => s).ToList();
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
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

        public ResultDto GetSkuListByContractNumber(ContractNoInputDto inputDto)
        {
            _methodName = "GetSkuListByContractNumber";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.DealerId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.DealerIdEmpty);
                }

                //if (string.IsNullOrEmpty(inputDto.SaudaNumber))
                //{
                //    return _resultService.ErrorMessage(Constants.SaudaNumberIsEmpty);
                //}

                List<SaudaSkuDetailsDto> skulist = new List<SaudaSkuDetailsDto>();

                //= _resultService.SaudaAvailableQuantityCheck(inputDto.SaudaNumber);
                var pendingList = _emamiContext.PendingContracts.AsNoTracking()
                    .Where(_ => _.UserId == inputDto.DealerId && _.SaudaNumber != null);

                skulist = _emamiContext.Skus.AsNoTracking().Join(pendingList, s => s.SkuCode, p => p.MaterialCode, (s, p) => new { s, p })
                    .Where(_ => _.p.SalesOrgId == _.s.SalesOrganizationId && _.p.DistChnlId == _.s.DistributionChannelId && _.p.DivisionId == _.s.DivisionId && _.s.IsActive).GroupBy(_ => new { _.s.SkuName, _.s.Id, _.s.SkuCode }).Select(s => new SaudaSkuDetailsDto()
                    {
                        SkuName = s.Key.SkuName + "_" + s.Key.SkuCode,
                        SkuId = s.Key.Id
                    }).ToList();

                foreach(var sku in skulist)
                {
                    sku.CaseToMetricTonValue = _resultService.ConvertCasetoMetricTon(1, sku.SkuId);
                }


                //var saudaList = _emamiContext.Sauda.AsNoTracking()
                //   .Where(_ => _.UserId == inputDto.DealerId && _.StatusId == (int)DTO.Enums.Status.Approved && _.SaudaNumber != null).Select(a => a.Id).ToList();

                //var skuIds = _emamiContext.SaudaOrders.AsNoTracking()
                //    .Where(_ => saudaList.Contains(_.SaudaId) && _.StatusId == (int)DTO.Enums.Status.Approved).Select(so => so.SkuId).ToList();

                //skulist = _emamiContext.Skus.AsNoTracking().Where(sku => skuIds.Contains(sku.Id) && sku.IsActive).Select(s => new SaudaSkuDetailsDto
                //{
                //    SkuName = s.SkuName + "_" + s.SkuCode,
                //    SkuId = s.Id
                //}).ToList();

                //var volumeLoadabilityContext = volumeContext.OrderByDescending(_ => _.Id)
                //    .FirstOrDefault(_ => _.SkuId == sku.SkuId && _.IsActive
                //    && _.VehicleSize == inputDto.VehicleSize && _.PlantId == inputDto.PlantId);

                //sku.MaxAllowableCasesSingleSku = /*volumeLoadabilityContext != null ? volumeLoadabilityContext.MaxAllowableSinglesku : */0;
                //sku.MaxAllowableCasesMultipleSku = /*volumeLoadabilityContext != null ? volumeLoadabilityContext.MaxAllowableMultiplesku :*/ 0;
                //sku.GrossWeight = (skuContext.FirstOrDefault(_ => _.Id == sku.Id) != null) ? skuContext.FirstOrDefault(_ => _.Id == sku.Id).GrossWeight : 0;
                //sku.MaximumVehicleCapacityInPercent = 0;
                //sku.MaximumVolumeCapacityInPercent = 0;


                // sku.SkuUomId = skuUomContext.FirstOrDefault(s => s.SkuId == sku.SkuId) != null ? skuUomContext.FirstOrDefault(s => s.SkuId == sku.SkuId).UomId : 0;
                // sku.SkuUomName = uomContext.FirstOrDefault(u => u.Id == (skuUomContext.FirstOrDefault(s => s.SkuId == sku.SkuId) != null ? skuUomContext.FirstOrDefault(s => s.SkuId == sku.SkuId).UomId : 0)) != null ? uomContext.FirstOrDefault(u => u.Id == (skuUomContext.FirstOrDefault(s => s.SkuId == sku.SkuId) != null ? skuUomContext.FirstOrDefault(s => s.SkuId == sku.SkuId).UomId : 0)).SAPName : string.Empty;
                // sku.AvailableQuantity = sku.AvailableQuantity > 0 ? sku.AvailableQuantity : 0;

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = skulist;
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
    }
}
