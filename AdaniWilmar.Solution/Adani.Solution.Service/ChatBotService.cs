using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using GMCore.Helper;
using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adani.Solution.DTO.Common;

namespace Adani.Solution.Service
{
    public interface IChatBotService
    {
        ResultDto GetDealerIdByDealerCode(DealerDto dealerDto);
        //ResultDto GetPendingSaudaAndDueDetails(UserIdDto userIdDto);
        ResultDto GetSpecialRateApprovalsList(LoginUserIdDto loginUserIdDto);
        ResultDto GetLimitEnhancementDetails(IdInputDto idInputDto);
        ResultDto GetDailyRateDetails(DailyRateInputDto inputDto);
        //ResultDto OverallSales(LoginUserIdDto inputDto);
        //ResultDto GetFrieghtRouteList();
        ResultDto GetOilType(LoginUserIdDto loginUserIdDto);
        ResultDto GetIncoTermsList();
        ResultDto BDOPlantDepotDetailsByDealer(LoginUserIdDto inputDto);
     }
    public class ChatBotService : IChatBotService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("ChatBot Service");
        private const string ServiceName = "ChatBot Service";
        private string _methodName;
        private readonly IResultService _resultService;
        private readonly INotificationService _notificationService;

        public ChatBotService(IAdaniContext salesContext, IResultService resultService, INotificationService notificationService)
        {
            try
            {
                _emamiContext = salesContext;
                _resultService = resultService;
                _notificationService = notificationService;
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for ChatBot Service", exception);
            }
        }

        //Get DealerId and Details By Dealer Code
        public ResultDto GetDealerIdByDealerCode(DealerDto dealerDto)
        {
            _methodName = "GetDealerIdByDealerCode";
            try
            {
                var resultDto = new ResultDto();
                if (dealerDto.UserId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (dealerDto.Code == null)
                {
                    return _resultService.ErrorMessage(Constants.DealerCodeMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().ToList();

                var isValidBDO = userContext.FirstOrDefault(f => f.Id == dealerDto.UserId && f.IsActive);
                if (isValidBDO == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var isValidDealer = userContext.FirstOrDefault(f => f.Code == dealerDto.Code && f.IsActive);
                if (isValidDealer == null)
                {
                    return _resultService.ErrorMessage(Constants.DealerNotFound);
                }

                var userCustomerMappingContext = _emamiContext.UserCustomerMapping.AsNoTracking().FirstOrDefault(f => f.UserId == isValidBDO.Id && f.CustomerId == isValidDealer.Id);
                if (userCustomerMappingContext == null)
                {
                    return _resultService.ErrorMessage(Constants.DeaerNotMappingToTheUser);
                }

                var dealerDetails = new DealerDto
                {
                    Id = isValidDealer.Id,
                    Name = isValidDealer.Name,
                    Code = isValidDealer.Code,
                    Email = isValidDealer.Email,
                    MobileNumber = isValidDealer.MobileNumber
                };
                return _resultService.SuccessObject(dealerDetails);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        //Pending Sauda,OverDue and Tomorrow's Due
        //public ResultDto GetPendingSaudaAndDueDetails(UserIdDto userIdDto)
        //{
        //    _methodName = "GetPendingSaudaAndDueDetails";
        //    try
        //    {
        //        var pendingSaudaAndDueDetails = new ChatBotPendingSaudaAndDueDto();

        //        var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //        if (userIdDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }

        //        if (userIdDto.UserId <= 0)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserIdMissing);
        //        }

        //        var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == userIdDto.UserId && _.IsActive);
        //        if (userContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserNotFound);
        //        }

        //        var roleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
        //        if (roleContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.RoleNotFound);
        //        }

        //        var PendingContractContext = _emamiContext.PendingContracts.AsNoTracking()
        //                                    .Join(_emamiContext.Users.AsNoTracking(), pc => pc.CustomerCode, us => us.Code, (sr, us) => new { PendingContract = sr, User = us })
        //                                    .Where(_ => _.PendingContract != null && userIdDto.UserId == _.User.Id).Select(_ => new { _.PendingContract }).ToList();

        //            if (PendingContractContext != null && PendingContractContext.Any())
        //            {
        //                pendingSaudaAndDueDetails.PendingSaudaQuantity = PendingContractContext.Sum(_ => _.PendingContract.PendingQuantityInMT);
        //            }

        //            var outStandingContextList = _emamiContext.PendingContracts.AsNoTracking()
        //                                 .Join(_emamiContext.Users.AsNoTracking(), pc => pc.CustomerCode, us => us.Code, (sr, us) => new { PendingContract = sr, User = us })
        //                                 .Where(_ => _.PendingContract != null && userIdDto.UserId == _.User.Id).Select(_ => new { _.PendingContract });

        //            if (outStandingContextList != null && outStandingContextList.Any())
        //            {
        //                var ExpiredContextList = outStandingContextList.Where(_ => DbFunctions.TruncateTime(_.PendingContract.ContractValidTo) < DbFunctions.TruncateTime(currentDate)).ToList();
        //                var NearExpiredContextList = outStandingContextList.Where(_ => DbFunctions.DiffDays(currentDate, _.PendingContract.ContractValidTo) < 5 && DbFunctions.DiffDays(currentDate, _.PendingContract.ContractValidTo) >= 1).ToList();
                        
        //                if (ExpiredContextList != null && ExpiredContextList.Any())
        //                {
        //                    pendingSaudaAndDueDetails.ExpiredSaudaQuantity = ExpiredContextList.Sum(_ => _.PendingContract.PendingQuantityInMT);
        //                }
        //                if (NearExpiredContextList != null && NearExpiredContextList.Any())
        //                {
        //                    pendingSaudaAndDueDetails.NearExpiredSaudaQuantity = NearExpiredContextList.Sum(_ => _.PendingContract.PendingQuantityInMT);
        //                }
        //            }
        //            var invoicesContext = _emamiContext.Invoices.AsNoTracking().Where(_ => userIdDto.UserId == _.UserId /*&& !_.PaymentStatus*/);
        //            if (invoicesContext != null && invoicesContext.Any())
        //            {
        //                var dueForTomoinvoicesContext = invoicesContext.Where(_ => _.InvoiceDate != null && DbFunctions.TruncateTime(_.InvoiceDueDate) == DbFunctions.TruncateTime(DbFunctions.AddDays(currentDate, 1)));
        //                if (dueForTomoinvoicesContext != null && dueForTomoinvoicesContext.Any())
        //                {
        //                    pendingSaudaAndDueDetails.TotalDueForTomorrow = dueForTomoinvoicesContext.Sum(_ => _.NetValue);
        //                }
        //                var overDueinvoicesContext = invoicesContext.Where(_ => _.InvoiceDueDate != null && DbFunctions.TruncateTime(_.InvoiceDueDate) < DbFunctions.TruncateTime(currentDate));
        //                if (overDueinvoicesContext != null && overDueinvoicesContext.Any())
        //                {
        //                    pendingSaudaAndDueDetails.TotalOverDue = overDueinvoicesContext.Sum(_ => _.NetValue);
        //                }

        //            }
                
        //        return _resultService.SuccessObject(pendingSaudaAndDueDetails);
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        //Get Special Rate Approvals
        public ResultDto GetSpecialRateApprovalsList(LoginUserIdDto loginUserIdDto)
        {
            var specialRateApprovalList = new List<ChatBotSpecialRateApprovalDto>();
            _methodName = "GetSpecialRateApprovalsList";
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == loginUserIdDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                var specialRateContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => _.UserId == loginUserIdDto.LoginUserId && DbFunctions.TruncateTime(_.CreatedDate) == DbFunctions.TruncateTime(currentDate)).Select(s =>s.Id).ToList();

                var specialRateApprovalListContext = _emamiContext.SpecialRateApproval.AsNoTracking().Where(_ => specialRateContext.Contains(_.SpecialRateId)
                    && _.SpecialRate != null && ((loginUserIdDto.VerticalId > 0 && _.SpecialRate.OilType != null) ? _.SpecialRate.OilType.DivisionId == loginUserIdDto.VerticalId : _.SpecialRate.OilType.DivisionId > 0))
                    .GroupBy(_ => _.SpecialRateId).Select(group =>
                          new
                          {
                              SpecialRateId = group.Key,
                              SpecialRateApprovals = group.OrderByDescending(_ => _.Id)
                          })
                    .Select(_ => _.SpecialRateApprovals.FirstOrDefault());

                var incotermsContext = _emamiContext.IncoTerms.AsNoTracking().ToList();

                if (specialRateApprovalListContext.Any())
                {
                    specialRateApprovalList = specialRateApprovalListContext.ToList().Select(_ => new ChatBotSpecialRateApprovalDto()
                    {
                        Date = _.SpecialRate.CreatedDate,
                        Status = _.SpecialRate.Status?.Name,
                        SkuName = _.SpecialRate.Sku?.SkuName,
                        IncoTerms = incotermsContext.FirstOrDefault(f => f.Id == _.SpecialRate.Incoterms2)?.Name,
                        //FreightRoute = _.SpecialRate.FreightRoute != null ? _.SpecialRate.FreightRoute.Name : string.Empty,
                        FinalPrice = _.SpecialRate.FinalPrice,
                        SpecialPrice = _.SpecialRate.SpecialPrice,
                    }).ToList();
                }

                return _resultService.SuccessObject(specialRateApprovalList);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetLimitEnhancementDetails(IdInputDto idInputDto)
        {
            _methodName = "DealerSaudaDetails";
            var dealerSaudaDetailsDto = new DealerSaudaDetailsDto();
            if (idInputDto == null)
            {
                return NotFoundResult();
            }
            try
            {
                if (idInputDto.SalesOrganizationId == 0)
                {
                    return _resultService.ErrorMessage(Constants.SalesOrganisationIsEmpty);
                }
                if (idInputDto.DistributionChannelId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DistributionChannelIsEmpty);
                }
                if (idInputDto.DivisionId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DivisionIsEmpty);
                }
                var overallStatus = Constants.OverallSaudaStatus;
                var overAllSaudaContext = (from s in _emamiContext.Sauda.AsNoTracking()
                                           join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                                           where s.UserId == idInputDto.Id
                                           && overallStatus.Contains(so.StatusId)
                                           select so
                                               ).ToList();

                //var SaudaLimitContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == idInputDto.Id);
                var userdivContext = _emamiContext.UserDivisionMappings.AsNoTracking()
                    .FirstOrDefault(_ => _.UserId == idInputDto.Id
                    && _.SalesOrganizationId == idInputDto.SalesOrganizationId && _.DistributionChannelId == idInputDto.DistributionChannelId
                    && _.DivisionId == idInputDto.DivisionId);

                dealerSaudaDetailsDto.TotalSaudaLimit = (decimal)userdivContext.SaudaLimit;
                dealerSaudaDetailsDto.AvailableSaudaLimit = (decimal)userdivContext.SaudaLimit;


                if (overAllSaudaContext != null && overAllSaudaContext.Any())
                {
                    var SaudaOutstanding = overAllSaudaContext.Sum(_ => _.BidQuantity);

                    decimal invoiceQuantity = 0;
                    decimal RtninvoiceQuantity = 0;
                    var existingSaudaQuantity = overAllSaudaContext.Sum(_ => _.BidQuantity);
                    var skuIds = overAllSaudaContext.Select(_ => _.SkuId).Distinct().ToList();
                    var invoiceContext = (from inv in _emamiContext.Invoices.AsNoTracking()
                                          join invDet in _emamiContext.InvoiceDetails.AsNoTracking() on inv.Id equals invDet.InvoiceId
                                          where inv.UserId == idInputDto.Id /*&& inv.SalesDocumentType != "ZHCR"*/
                                          && skuIds.Contains(invDet.SkuId)
                                          select invDet
                                              ).ToList();

                    var rtninvoiceContext = (from inv in _emamiContext.Invoices.AsNoTracking()
                                             join invDet in _emamiContext.InvoiceDetails.AsNoTracking() on inv.Id equals invDet.InvoiceId
                                             where inv.UserId == idInputDto.Id /*&& inv.SalesDocumentType == "ZHCR"*/
                                             && skuIds.Contains(invDet.SkuId)
                                             select invDet
                                              ).ToList();

                    if (invoiceContext != null && invoiceContext.Any())
                    {
                        invoiceQuantity = invoiceContext.Sum(_ => _.ActualBilledQuantity);
                    }
                    if (rtninvoiceContext != null && rtninvoiceContext.Any())
                    {
                        RtninvoiceQuantity = rtninvoiceContext.Sum(_ => _.ActualBilledQuantity);
                    }

                    dealerSaudaDetailsDto.AvailableSaudaLimit = (decimal)userdivContext.SaudaLimit - (existingSaudaQuantity - invoiceQuantity + RtninvoiceQuantity);
                }
                return SucessResult(dealerSaudaDetailsDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto GetDailyRateDetails(DailyRateInputDto inputDto)
        {
            _methodName = "GetDailyRateDetails";
            var resultDto = new ResultDto();
            try
            {

                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                //if (inputDto.IncotermId == 0)
                //{
                //    return _resultService.ErrorMessage(Constants.IncotermsMissing);
                //}

                //if (inputDto.FrieghtRouteId == 0)
                //{
                //    return _resultService.ErrorMessage(Constants.FrieghtRouteMissing);
                //}

                if (inputDto.PlantId == 0)
                {
                    return _resultService.ErrorMessage(Constants.PlantMissing);
                }

                var userContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == inputDto.LoginUserId);

                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var transportModeId = Constants.DefaultTransportModeId;
                var loadQuantity = Constants.DefaultLoadQuantity;
                var cityId = userContext.CityId;
                var stateId = userContext.StateId;
                var pricingContext = new List<Pricing>();
                var outputgroubyList = new List<Pricing>();
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                var currentDate1 = (DateHelper.UtcToIndia(DateTime.UtcNow) ).AddDays(-3);

                if (userContext.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess)
                {
                    
                        outputgroubyList = _emamiContext.Pricing.AsNoTracking().Where(_ =>  _.PlantId == inputDto.PlantId  ).ToList();
                    
                }
                
                if (outputgroubyList == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                var RecentPricings = from e in outputgroubyList
                                     group e by new { e.SkuId,
                                         //e.FrieghtRouteId, e.OilTypeId, e.TransportModeId, e.LoadQuantity, e.StateId 
                                     } into dptgrp
                                     let topsal = dptgrp.Max(x => x.Id)
                                     select new Pricing
                                     {
                                         //FrieghtRouteId = dptgrp.Key.FrieghtRouteId,
                                         //OilTypeId = dptgrp.Key.OilTypeId,
                                         //TransportModeId = dptgrp.Key.TransportModeId,
                                         //LoadQuantity = dptgrp.Key.LoadQuantity,
                                         //StateId = dptgrp.Key.StateId,
                                         SkuId = dptgrp.Key.SkuId,
                                         Id = dptgrp.First(y => y.Id == topsal).Id,
                                         //DepotId = dptgrp.First(y => y.Id == topsal).DepotId,
                                         PlantId = dptgrp.First(y => y.Id == topsal).PlantId,
                                         //SaudaBookingTypeId = dptgrp.First(y => y.Id == topsal).SaudaBookingTypeId,
                                         //OilTypeId = dptgrp.Key.OilTypeId,
                                         OilPackingTypeId = dptgrp.First(y => y.Id == topsal).OilPackingTypeId,
                                         //CityId = dptgrp.First(y => y.Id == topsal).CityId,
                                         //FrieghtZoneId = dptgrp.First(y => y.Id == topsal).TransportModeId,
                                         //BiddingWindowId = dptgrp.First(y => y.Id == topsal).BiddingWindowId,
                                         //BiddingDate = dptgrp.First(y => y.Id == topsal).BiddingDate,
                                         //MaterialCost = dptgrp.First(y => y.Id == topsal).MaterialCost,
                                         //PackingCost = dptgrp.First(y => y.Id == topsal).PackingCost,
                                         //PrimaryFrieght = dptgrp.First(y => y.Id == topsal).PrimaryFrieght,
                                         //SecondaryFrieght = dptgrp.First(y => y.Id == topsal).SecondaryFrieght,
                                         //DepotCost = dptgrp.First(y => y.Id == topsal).DepotCost,
                                         //DetentionCost = dptgrp.First(y => y.Id == topsal).DetentionCost,
                                         //HoneycombCost = dptgrp.First(y => y.Id == topsal).HoneycombCost,
                                         //Margin = dptgrp.First(y => y.Id == topsal).Margin,
                                         //CushionMargin = dptgrp.First(y => y.Id == topsal).CushionMargin,
                                         //SchemeCostRecovery = dptgrp.First(y => y.Id == topsal).SchemeCostRecovery,
                                         //Discount = dptgrp.First(y => y.Id == topsal).Discount,
                                         //Premium = dptgrp.First(y => y.Id == topsal).Premium,
                                         //ProcessCost = dptgrp.First(y => y.Id == topsal).ProcessCost,
                                         //SumOfIngredientCost = dptgrp.First(y => y.Id == topsal).SumOfIngredientCost,
                                         //TpPrice = dptgrp.First(y => y.Id == topsal).TpPrice,
                                         //RaMargin = dptgrp.First(y => y.Id == topsal).RaMargin,
                                         //BaseRate = dptgrp.First(y => y.Id == topsal).BaseRate,
                                         //XMargin = dptgrp.First(y => y.Id == topsal).XMargin,
                                         //FinalRate = dptgrp.First(y => y.Id == topsal).FinalRate,
                                         //ExPlantPrice = dptgrp.First(y => y.Id == topsal).ExPlantPrice,
                                         //ForDepotPrice = dptgrp.First(y => y.Id == topsal).ForDepotPrice,
                                         //ForPlantPrice = dptgrp.First(y => y.Id == topsal).ForPlantPrice,
                                         //ExDepotPrice = dptgrp.First(y => y.Id == topsal).ExDepotPrice,
                                         //ExRakePrice = dptgrp.First(y => y.Id == topsal).ExRakePrice,
                                         //ForRakePrice = dptgrp.First(y => y.Id == topsal).ForRakePrice,
                                         //ClearanceRate = dptgrp.First(y => y.Id == topsal).ClearanceRate,
                                         //CounterBidOffer = dptgrp.First(y => y.Id == topsal).CounterBidOffer,
                                         //CounterBidLimit = dptgrp.First(y => y.Id == topsal).CounterBidLimit,
                                         //BpCpJumb = dptgrp.First(y => y.Id == topsal).BpCpJumb,
                                         //IsActive = dptgrp.First(y => y.Id == topsal).IsActive,
                                         CreatedBy = dptgrp.First(y => y.Id == topsal).CreatedBy,
                                         CreatedDate = dptgrp.First(y => y.Id == topsal).CreatedDate,
                                         ModifiedBy = dptgrp.First(y => y.Id == topsal).ModifiedBy,
                                         ModifiedDate = dptgrp.First(y => y.Id == topsal).ModifiedDate,
                                     };



                var finalOutputDto = new List<Pricing>();
                var SkuDistinct = from a in RecentPricings.ToList()
                                  group a by new { a.SkuId, 
                                      //a.FrieghtRouteId, a.OilTypeId, a.TransportModeId, a.StateId 
                                  } into grp
                                  let topsku = grp.Max(X => X.Id)
                                  select new Pricing
                                  {
                                      SkuId = grp.Key.SkuId,
                                  };

                foreach (var item in SkuDistinct.ToList())
                {
                    var RecentPricingContext = (from a in RecentPricings.ToList()
                                                where a.SkuId == item.SkuId 
                                                //&& a.FrieghtRouteId == item.FrieghtRouteId && a.OilTypeId == item.OilTypeId && a.TransportModeId == item.TransportModeId && a.StateId == item.StateId
                                                select a).ToList();

                    if (RecentPricingContext != null && RecentPricingContext.Any())
                    {
                        if (RecentPricingContext.Count > 1)
                        {
                            finalOutputDto.AddRange(RecentPricingContext.Where(_ => _.SkuId == item.SkuId                                                                              ).ToList());
                        }
                        
                    }
                }

                pricingContext = finalOutputDto.ToList();

                var outputList = new List<DailyRateOutputDto>();
                foreach (var pricing in pricingContext)
                {
                    var finalPrice = pricing.Price;                    
                    if (finalPrice > 0)
                    {
                        if (!outputList.Any(_ => _.SkuId == pricing.SkuId))
                        {
                            var finalRate = new DailyRateOutputDto
                            {
                                SkuId = pricing.SkuId,
                                SkuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == pricing.SkuId) != null ? _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == pricing.SkuId).SkuName : string.Empty,
                                FinalPrice = finalPrice,
                            };

                            outputList.Add(finalRate);
                        }
                    }
                }

                var FinaloutputList = new List<DailyRateOutputDto>();
                if (outputList != null && outputList.Any())
                {
                    FinaloutputList = outputList
                                        .GroupBy(p => new { p.SkuId, p.SkuName, p.FinalPrice, p.PlantDepotId, p.PlantDepotName })
                                        .Select(g => g.First())
                                        .ToList();
                }
                return _resultService.SuccessObject(FinaloutputList);
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
        //public ResultDto OverallSales(LoginUserIdDto inputDto)
        //{
        //    _methodName = "OverallSales";
        //    var OverallsaudaOutpuDto = new OverallsaudaOutpuForChatBotDto();
        //    if (inputDto == null)
        //    {
        //        return NotFoundResult();
        //    }
        //    var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
        //    if (userContext == null)
        //    {
        //        return _resultService.ErrorMessage(Constants.UserNotFound);
        //    }
        //    try
        //    {
        //        var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //        DateTime yearStartDate;
        //        DateTime yearEndDate;
        //        DateTime MonthStartDate;
        //        DateTime MonthEndDate;
        //        List<DateTime> CurrentStartAndEndQuater;
        //        DateTime CurrentQuaterStartDate;
        //        DateTime CurrentQuaterEndDate;

        //        var financialyearList = _emamiContext.FinancialYears.AsNoTracking().FirstOrDefault(_ => _.IsActive && DbFunctions.TruncateTime(_.EffectiveFrom) <= DbFunctions.TruncateTime(currentDate) &&
        //        DbFunctions.TruncateTime(_.EffectiveTo) >= DbFunctions.TruncateTime(currentDate));

        //        if (financialyearList != null)
        //        {
        //            yearStartDate = financialyearList.EffectiveFrom;
        //            yearEndDate = financialyearList.EffectiveTo;
        //            CurrentStartAndEndQuater = CurrentQuater(yearStartDate);
        //            CurrentQuaterStartDate = CurrentStartAndEndQuater[0];
        //            CurrentQuaterEndDate = CurrentStartAndEndQuater[1];
        //        }
        //        else
        //        {
        //            yearStartDate = new DateTime(currentDate.Year, 01, 1);
        //            yearEndDate = new DateTime(currentDate.Year, 12, 31);
        //            CurrentStartAndEndQuater = CurrentQuater(yearStartDate);
        //            CurrentQuaterStartDate = CurrentStartAndEndQuater[0];
        //            CurrentQuaterEndDate = CurrentStartAndEndQuater[1];
        //        }

        //        MonthStartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
        //        MonthEndDate = MonthStartDate.AddMonths(1).AddDays(-1);
               
                
        //            var salescontext = _emamiContext.SalesRegister.AsNoTracking()
        //               .Join(_emamiContext.Users.AsNoTracking(), x => x.Payer, u => u.Code, (x, u) => new { x, u }).ToList();
        //            //yearly
        //            var salesContextForYearly = salescontext
        //                    .Where(_ => inputDto.LoginUserId == _.u.Id && _.x.BillingDate.Value.Date >= yearStartDate.Date &&
        //                    _.x.BillingDate.Value.Date <= yearEndDate.Date 
        //                    //&& _.u.DivisionId == userContext.DivisionId
        //                    ).OrderByDescending(_ => _.x.BillingDate).ToList();

        //            if (salesContextForYearly != null)
        //            {
        //                OverallsaudaOutpuDto.SaudaBookedForYearly = salesContextForYearly.Sum(_ => _.x.QuantityMT);
        //            }
        //            //monthly
        //            var salesContextForMonthly = salescontext
        //                .Where(_ => inputDto.LoginUserId == _.u.Id && _.x.BillingDate.Value.Date >= MonthStartDate.Date &&
        //                _.x.BillingDate.Value.Date <= MonthEndDate.Date 
        //                //&& _.u.DivisionId == userContext.DivisionId
        //                ).OrderByDescending(_ => _.x.BillingDate).ToList();

        //            if (salesContextForMonthly != null)
        //            {
        //                OverallsaudaOutpuDto.SaudaBookedForMonth = salesContextForMonthly.Sum(_ => _.x.QuantityMT);
        //            }
        //            //quaterly
        //            var salesContextForCurrentMonthQuater = salescontext.Where(_ => inputDto.LoginUserId == _.u.Id && (_.x.BillingDate.Value.Date >= CurrentQuaterStartDate.Date &&
        //            _.x.BillingDate.Value.Date <= CurrentQuaterEndDate.Date) 
        //            //&& _.u.DivisionId == userContext.DivisionId
        //            ).OrderByDescending(_ => _.x.BillingDate).ToList();

        //            if (salesContextForCurrentMonthQuater != null)
        //            {
        //                OverallsaudaOutpuDto.SaudaBookedForQuater = salesContextForCurrentMonthQuater.Sum(_ => _.x.QuantityMT);
        //            }

               
        //        return SucessResult(OverallsaudaOutpuDto);
        //    }
        //    catch (Exception exception)
        //    {
        //        return ExceptionResult(exception);
        //    }
        //}

        public List<DateTime> CurrentQuater(DateTime StartDate)
        {
            var startCal = StartDate;
            List<DateTime> StartAndEndDateOfMonth = new List<DateTime>();
            var customToday = DateTime.Now;
            List<DateTime> StartDates = new List<DateTime>();
            List<DateTime> EndDates = new List<DateTime>();

            var qtr = 0;
            var currentQtr = 0;
            for (int i = 0; i < 12; i++)
            {

                if (i % 3 == 0)
                {
                    if (currentQtr == 0) { StartDates.Clear(); EndDates.Clear(); }
                    qtr++;
                }
                if (currentQtr != 0 && qtr > currentQtr)
                {
                    break;
                }
                StartDates.Add(startCal);
                var DaysInMonth = DateTime.DaysInMonth(startCal.Year, startCal.Month);
                var lastDay = new DateTime(startCal.Year, startCal.Month, DaysInMonth);
                EndDates.Add(lastDay);
                if (startCal.Month == customToday.Month)
                {
                    currentQtr = qtr;
                }
                startCal = startCal.AddMonths(1);
            }            StartAndEndDateOfMonth.Add(StartDates[0]);            StartAndEndDateOfMonth.Add(EndDates[2]);            return StartAndEndDateOfMonth;

        }

        //public ResultDto GetFrieghtRouteList()
        //{
        //    _methodName = "GetFrieghtRouteList";
        //    var resultDto = new ResultDto();
        //    try
        //    {
        //        var frieghtRouteList = _emamiContext.FreightRoutes.AsNoTracking().Where(_ => _.IsActive)
        //            .Select(s => new FreightRouteDto()
        //            {
        //                Id = s.Id,
        //                Name = s.FreightZone.Name + " - " + s.Name,
        //                FreightZoneId = s.FreightZoneId,
        //                FreightZoneName = s.FreightZone.Name
        //            }).ToList();

        //        if (frieghtRouteList == null || !frieghtRouteList.Any())
        //        {
        //            return NotFoundResult();
        //        }

        //        return SucessResult(frieghtRouteList);
        //    }
        //    catch (Exception exception)
        //    {
                
        //        return ExceptionResult(exception);
        //    }
        //}


        public ResultDto GetOilType(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetOilType";
            var resultDto = new ResultDto();
            try
            {
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == loginUserIdDto.LoginUserId);
                var userDivisionContext = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == loginUserIdDto.LoginUserId).ToList(); ;
                var userDivisionIds = userDivisionContext.Select(_ => _.DivisionId).ToList();
                var userSalesOrgIds = userDivisionContext.Select(_ => _.SalesOrganizationId).ToList();
                var userDistChanIds = userDivisionContext.Select(_ => _.DistributionChannelId).ToList();
                IQueryable<OilType> oiltype;
                if (loginUserIdDto.IsToReturnInactiveData)
                {
                    if (userDivisionContext == null || !userDivisionContext.Any())
                    {
                        oiltype = _emamiContext.OilTypes.AsNoTracking();
                    }
                    else
                    {
                        oiltype = _emamiContext.OilTypes.AsNoTracking().Where(_ => userDivisionIds.Contains(_.DivisionId)
                        && userSalesOrgIds.Contains(_.SalesOrganizationId) && userDistChanIds.Contains(_.DistributionChannelId));
                    }
                }
                else
                {
                    if (userDivisionContext == null || !userDivisionContext.Any())
                    {
                        oiltype = _emamiContext.OilTypes.AsNoTracking();
                    }
                    else
                    {
                        oiltype = _emamiContext.OilTypes.AsNoTracking().Where(_ => userDivisionIds.Contains(_.DivisionId)
                        && userSalesOrgIds.Contains(_.SalesOrganizationId) && userDistChanIds.Contains(_.DistributionChannelId));
                    }
                }

                var oiltypeList = oiltype
                    .Select(s => new OilTypeDto()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        VerticalId = s.DivisionId,
                        VerticalName = s.Division.Name,
                      //  LitreConversion = s.LitreConversion,
                        IsActive = s.IsActive,
                        //IsRasoi = s.IsRasoi
                    }).ToList();

                return SucessResult(oiltypeList);
            }
            catch (Exception exception)
            {
               return ExceptionResult(exception);
            }
        }

        public ResultDto GetIncoTermsList()
        {
            _methodName = "GetIncoTermsList";
            var resultDto = new ResultDto();
            try
            {
                var incotermList = _emamiContext.IncoTerms.AsNoTracking().Where(_ => _.IsActive)
                    .Select(s => new IncoTermsDto()
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Code = s.Code,
                        IsActive = s.IsActive
                    }).ToList();

                return SucessResult(incotermList);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

        public ResultDto BDOPlantDepotDetailsByDealer(LoginUserIdDto inputDto)
        {
            _methodName = "BDOPlantDepotDetailsByDealer";
            var userMasterDto = new List<UserMasterDto>();
            var PlantDepotList = new List<DepotDto>();

            if (inputDto == null)
            {
                return NotFoundResult();
            }
            try
            {
                var LoginuserContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);

                var depotList =
                              (from depot in _emamiContext.Depots.AsNoTracking()
                               join depotMapping in _emamiContext.UserDepotMapping.AsNoTracking() on depot.Id equals depotMapping.DepotId
                               where depotMapping.UserId == inputDto.LoginUserId && depot.IsActive && depot.IsPlant
                               select new DepotDto
                               {
                                   Id = depot.Id,
                                   Name = depot.Name,
                                   Code = depot.Code,
                                   IsPlant = depot.IsPlant,
                                   IsActive = depot.IsActive
                               }).ToList();

                    foreach (var plant in depotList)
                    {
                        var depotContext = (from plantdepot in _emamiContext.PlantDepotMapping.AsNoTracking()
                                            join depot in _emamiContext.Depots.AsNoTracking() on plantdepot.DepotId equals depot.Id
                                            where plantdepot.PlantId == plant.Id && !depot.IsPlant && depot.StorageTypeId == (int)DTO.Enums.StorageType.Depot
                                            select new DepotDto
                                            {
                                                Id = depot.Id,
                                                Name = depot.Name,
                                                Code = depot.Code,
                                                IsPlant = depot.IsPlant,
                                                IsActive = depot.IsActive
                                            }).ToList();

                        plant.Depotlist = depotContext;


                        var rakeContext = (from plantdepot in _emamiContext.PlantDepotMapping.AsNoTracking()
                                           join depot in _emamiContext.Depots.AsNoTracking() on plantdepot.DepotId equals depot.Id
                                           where plantdepot.PlantId == plant.Id && !depot.IsPlant && depot.StorageTypeId == (int)DTO.Enums.StorageType.Rake
                                           select new DepotDto
                                           {
                                               Id = depot.Id,
                                               Name = depot.Name,
                                               Code = depot.Code,
                                               IsPlant = depot.IsPlant,
                                               IsActive = depot.IsActive
                                           }).ToList();

                        plant.Rakelist = rakeContext;
                    }

                    if (depotList != null && depotList.Any())
                    {
                        PlantDepotList.AddRange(depotList);
                    }
                

                List<DepotDto> list = null;
                if (PlantDepotList != null && PlantDepotList.Any())
                {
                    list = PlantDepotList
                    .GroupBy(a => a.Id)
                    .Select(g => g.First())
                    .ToList();

                }


                return SucessResult(list);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }
    }

}
