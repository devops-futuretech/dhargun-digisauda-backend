using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using GMCore.Logger;
using GMCore.Helper;
using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using System.Globalization;

namespace Adani.Solution.Service
{
    public interface IMobileSTPService
    {
        ResultDto GetMTPDetailsForCurrentOrUpcomingMonth(MTPDateWiseDetailsInputDto inputDto);
        ResultDto SaveUpcomingMonthlyTourPlan(MTPInputDto inputDto);
        ResultDto NoVisitByUserPermanentJourneyPlan(PJPIdDto pJPIdDto);
        ResultDto SaveMTPNoVisitRemarks(MonthlyTourPlanUpdateDto inputDto);
        ResultDto GetHolidayList(IdInputDto year);
        ResultDto GetTotalPCPByUsers(IdInputDto idInputDto);
        ResultDto AddDealerVisit(AddDealerVisitDto addDealerVisitDto);
        ResultDto PendingMonthlyPlanDeviationForMobile(LoginUserIdDto loginUserIdDto);
        ResultDto ApprovedMonthlyPlanDeviationForMobile(LoginUserIdDto loginUserIdDto);
        ResultDto AddMonthlyPlanDeviation(MonthlyPlanDeviationListDto addMonthlyPlanDeviationDto);
        ResultDto SalesTourPlanChart(SalesTourPlanInputDto inputDto);
        ResultDto SaveCompetitorImageName(ImageNameAddDto imageNameAddDto);
        ResultDto AddWholeSellerVisit(AddWholeSellerVisitDto addWholeSellerVisitDto);
        ResultDto GetWholeSellerList(IdInputDto idInputDto);
        ResultDto GetSecondarySalesFortheDay(LoginUserIdDto loginUserIdDto);
        ResultDto GetSecondarySalesDetails(WholesellerSecondarySalesInputDto secondarySalesInputDto);
    }

    public class MobileSTPService : IMobileSTPService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Mobile SalesTourPlan Service");
        private const string ServiceName = "Mobile SalesTourPlan Service";
        private string _methodName;
        private readonly IResultService _resultService;

        public MobileSTPService(IAdaniContext emamiContext, IResultService resultService)
        {
            try
            {
                _emamiContext = emamiContext;
                _resultService = resultService;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for Lookup Service", exception);
            }
        }


        #region MTP Mobile

        public ResultDto SaveUpcomingMonthlyTourPlan(MTPInputDto inputDto)
        {
            _methodName = "SaveUpcomingMonthlyTourPlan";
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
                if (inputDto.LoginUserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }
                if ((inputDto.DealerIds == null || !inputDto.DealerIds.Any()) && inputDto.InHQNoVisit == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (userContext.ReportingToId == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.PJPApprovalFlowEmpty;
                    resultDto.ErrorDto.Message = Constants.PJPApprovalFlowEmpty;
                    return resultDto;
                }
                if (inputDto.DealerIds != null && inputDto.DealerIds.Any())
                {
                    var noVisitExists = _emamiContext.MonthlyTourPlanDetails.AsNoTracking().FirstOrDefault(_ => DbFunctions.TruncateTime(_.Date) == DbFunctions.TruncateTime(inputDto.Date)
                    && _.InHQNoVisit != 0 && _.CreatedBy == inputDto.LoginUserId && _.MonthlyTourPlan.MonthlyTourPlanStatusId != (int)DTO.Enums.MonthlyTourPlanStatus.Rejected);
                    if (noVisitExists != null)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.ErrorCode = Constants.MTPNoVisitPerDay;
                        resultDto.ErrorDto.Message = Constants.MTPNoVisitPerDay;
                        return resultDto;
                    }
                }
                if (inputDto.InHQNoVisit != 0)
                {
                    var mtpExists = _emamiContext.MonthlyTourPlanDetails.AsNoTracking().FirstOrDefault(_ => DbFunctions.TruncateTime(_.Date) == DbFunctions.TruncateTime(inputDto.Date)
                    && _.CreatedBy == inputDto.LoginUserId && _.MonthlyTourPlan.MonthlyTourPlanStatusId != (int)DTO.Enums.MonthlyTourPlanStatus.Rejected);
                    if (mtpExists != null)
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.ErrorCode = Constants.MTPNoVisitPerDay;
                        resultDto.ErrorDto.Message = Constants.MTPNoVisitPerDay;
                        return resultDto;
                    }
                }

                var monthId = inputDto.Date.Month;
                var monthDetails = _emamiContext.Months.FirstOrDefault(_ => _.Id == inputDto.Date.Month);
                if (monthDetails != null)
                {
                    inputDto.MonthId = monthDetails.Id;
                }
                var mtpContext = new MonthlyTourPlans
                {
                    MonthlyTourPlanStatusId = (int)DTO.Enums.MonthlyTourPlanStatus.Pending,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    PJPId = inputDto.PJPId,
                    MonthId = inputDto.MonthId,
                };
                _emamiContext.MonthlyTourPlans.Add(mtpContext);
                _emamiContext.SaveChanges();

                mtpContext.MTPNumber = Utility.MonthlyTourPlanNumberPrefix + mtpContext.Id;

                if (inputDto.DealerIds != null && inputDto.DealerIds.Any())
                {
                    foreach (var dealerId in inputDto.DealerIds)
                    {
                        var detailContext = new MonthlyTourPlanDetails
                        {
                            MonthlyTourPlanId = mtpContext.Id,
                            Date = Convert.ToDateTime(inputDto.Date),
                            TownId = inputDto.TownId,
                            Area = inputDto.Area,
                            DealerId = dealerId.ToString(),
                         //   HeadquartersId = userContext.Headquarters.Id,
                            Remarks = string.Empty,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.MonthlyTourPlanDetails.Add(detailContext);
                    }
                }
                else if (inputDto.InHQNoVisit != 0)
                {
                    var detailContext = new MonthlyTourPlanDetails
                    {
                        MonthlyTourPlanId = mtpContext.Id,
                        Date = Convert.ToDateTime(inputDto.Date),
                        TownId = inputDto.TownId,
                        Area = inputDto.Area,
                      //  HeadquartersId = userContext.Headquarters.Id,
                        Remarks = string.Empty,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        InHQNoVisit = inputDto.InHQNoVisit,
                    };
                    _emamiContext.MonthlyTourPlanDetails.Add(detailContext);
                }

                var approvalContext = new MonthlyTourPlanApprovalInformation
                {
                    MonthlyTourPlanId = mtpContext.Id,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    UserId = userContext.ReportingToId ?? 0,
                    MonthlyTourPlanStatusId = (int)DTO.Enums.MonthlyTourPlanStatus.Pending
                };
                _emamiContext.MonthlyTourPlanApprovalInformation.Add(approvalContext);
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = mtpContext.Id;
                resultDto.SuccessDto.Message = "success";
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

        public ResultDto GetMTPDetailsForCurrentOrUpcomingMonth(MTPDateWiseDetailsInputDto inputDto)
        {
            _methodName = "GetMTPDetailsForCurrentOrUpcomingMonth";
            var resultDto = new ResultDto();
            var mtpDateWiseResult = new List<MTPDateWiseDetailsOutputDto>();
            try
            {
                List<MonthlyTourPlanDetails> monthlyTourPlanDetails = new List<MonthlyTourPlanDetails>();
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                if (inputDto.IsUpcoming)
                {
                    DateTime nextMonthDate = currentDate.AddMonths(1);
                    monthlyTourPlanDetails = _emamiContext.MonthlyTourPlans
                        .Where(_ => _.MonthlyTourPlanStatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Approved && _.CreatedBy == inputDto.LoginUserId)
                        .Join(_emamiContext.MonthlyTourPlanDetails
                        .Where(_ => _.Date.Month == nextMonthDate.Month && _.Date.Year == nextMonthDate.Year), mtp => mtp.Id, mtpd => mtpd.MonthlyTourPlanId, (mtp, mtpd) => new { mtpd }).Select(_ => _.mtpd).ToList();
                }
                else
                {
                    DateTime todayDate = currentDate;
                    monthlyTourPlanDetails = _emamiContext.MonthlyTourPlans
                      .Where(_ => _.MonthlyTourPlanStatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Approved && _.CreatedBy == inputDto.LoginUserId)
                      .Join(_emamiContext.MonthlyTourPlanDetails
                      .Where(_ => _.Date.Month == todayDate.Month && _.Date.Year == todayDate.Year), mtp => mtp.Id, mtpd => mtpd.MonthlyTourPlanId, (mtp, mtpd) => new { mtpd }).Select(_ => _.mtpd).ToList();

                }
                var dateList = monthlyTourPlanDetails.GroupBy(_ => new { _.Date })
                    .Select(_ => new { Date = _.FirstOrDefault().Date }).Select(_ => _.Date).ToList();
                
                var cityList = monthlyTourPlanDetails.GroupBy(_ => new { _.Date  })
                    .Select(_ => new MTPDateWiseCitiesDto
                    {
                        Date = _.FirstOrDefault().Date,
                        TownId = _.FirstOrDefault().TownId,
                        Town = _emamiContext.City.AsNoTracking().FirstOrDefault(s => s.Id==_.FirstOrDefault().TownId)!= null ?  _emamiContext.City.AsNoTracking().FirstOrDefault(s => s.Id == _.FirstOrDefault().TownId).CityName:String.Empty
                    }).ToList();

                var dealerList = monthlyTourPlanDetails.Where(_ => _.DealerId != null && _.DealerId!="0").GroupBy(_ => new { _.Date, _.DealerId })
                    .Select(_ => new MTPDateWiseDealersDto
                    {
                        Date = _.FirstOrDefault().Date,
                        TownId = _.FirstOrDefault().TownId,
                        DealerId = _.FirstOrDefault()?.DealerId,

                    }).ToList();

                var NoVisitList = monthlyTourPlanDetails.Where(_ => _.InHQNoVisit != 0).GroupBy(_ => new { _.Date,  _.InHQNoVisit })
                    .Select(_ => new MTPDateWiseNoVisitDto
                    {
                        Date = _.FirstOrDefault().Date,
                        TownId = _.FirstOrDefault().TownId,
                        NoVisitHQId = _.FirstOrDefault()?.InHQNoVisit,

                    }).ToList();

                foreach (var date in dateList)
                {
                    var cities = cityList.Where(_ => _.Date.Date == date.Date).ToList();
                    if (cities != null && cities.Any())
                    {
                        List<MTPDateWiseCitiesDto> cityWiseDtos = new List<MTPDateWiseCitiesDto>();
                        foreach (var city in cities)
                        {
                            var cityDetailsDto = new MTPDateWiseCitiesDto();
                            cityDetailsDto.TownId = city.TownId;
                            cityDetailsDto.Town = city.Town;
                            cityDetailsDto.Date = city.Date;
                            var dealers = dealerList.Where(_ => _.Date == date && _.TownId == city.TownId).ToList();
                            if (dealers != null && dealers.Any())
                            {
                                foreach (var dealer in dealers)
                                {
                                    if (!string.IsNullOrEmpty(dealer.DealerId) && dealer.DealerId!="0")
                                    {
                                        var dealerId = long.Parse(dealer.DealerId);
                                        dealer.Dealer = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == dealerId)?.Name;
                                    }
                                }
                                cityDetailsDto.MTPDateWiseDealersDtos = dealers;
                            }
                            var noVisit = NoVisitList.FirstOrDefault(_ => _.Date == date && _.TownId == city.TownId);
                            if (noVisit != null)
                            {
                                cityDetailsDto.NoVisitHQ = UtilityHelper.GetEnumDescription((DTO.Enums.STPVisitType)noVisit.NoVisitHQId);
                            }
                            cityWiseDtos.Add(cityDetailsDto);
                        }
                        var linearMtpDateWise = new MTPDateWiseDetailsOutputDto
                        {
                            Date = date,
                            MTPDateWiseCitiesDtos = cityWiseDtos
                        };
                        mtpDateWiseResult.Add(linearMtpDateWise);
                    }
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = mtpDateWiseResult;
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

        public ResultDto NoVisitByUserPermanentJourneyPlan(PJPIdDto pJPIdDto)
        {
            _methodName = "NoVisitByUserPermanentJourneyPlan";
            var resultDto = new ResultDto();
            var noVisitDto = new List<DealerDto>();
            try
            {
                var noVisits = new List<DealerDto>();
                var pjpContext = _emamiContext.PermanentJourneyPlans.FirstOrDefault(_ => _.Id == pJPIdDto.PJPId);
                if (pjpContext != null)
                {
                    if (pjpContext.PJPDetails.Any())
                    {
                        var pjpdetailscontext = pjpContext.PJPDetails.Where(_ => _.InHQNoVisit != 0).Select(_ => _.InHQNoVisit).Distinct().ToList();

                        noVisitDto = pjpdetailscontext.Select(c => new DealerDto
                        {
                            Id = c,
                            Name = UtilityHelper.GetEnumDescription((DTO.Enums.STPVisitType)c),
                        }).ToList();

                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = noVisitDto;
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

        public ResultDto SaveMTPNoVisitRemarks(MonthlyTourPlanUpdateDto inputDto)
        {
            _methodName = "SaveUpcomingMonthlyTourPlan";
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                if(string.IsNullOrEmpty(inputDto.Remarks))
                {
                    return _resultService.ErrorMessage(Constants.RemarksMissing);
                }

                var mtpDetailsContext = _emamiContext.MonthlyTourPlanDetails.FirstOrDefault(_ => _.Id==inputDto.MTPId);
                if(mtpDetailsContext==null)
                {
                    return _resultService.ErrorMessage(Constants.MTPNotFound);
                }
                mtpDetailsContext.VisitRemarks = inputDto.Remarks;
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
                return _resultService.SuccessObject(resultDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion

        #region Holiday

        public ResultDto GetHolidayList(IdInputDto inputDto)
        {
            _methodName = "GetHolidayList";
            var resultDto = new ResultDto();
            var holiDayDto = new HolidayDto();
            var holiDayListDto = new List<HolidayDto>();
            var holiDayDetailsDto = new List<HolidayDetailListDto>();
            try
            {
                var holiDayList = _emamiContext.Holiday.Where(w => w.Year == inputDto.Id).ToList()
                    .OrderBy(o => o.HolidayDate)
                    .GroupBy(g => new { MonthInt = g.HolidayDate.Month, YearInt = g.HolidayDate.Year })
                    .Select(s => new
                    {
                        Month = s.Key.MonthInt,
                        Count = s.Count(),
                        HolidayList = s.ToList()
                    });

                if (holiDayList != null && holiDayList.Any())
                {
                    foreach (var item in holiDayList)
                    {
                        holiDayDto = new HolidayDto();
                        holiDayDto.MonthName = new DateTime((int)inputDto.Id, item.Month, 1).ToString("MMMM");
                        holiDayDto.HolidayCount = item.Count;
                        if (item.HolidayList != null && item.HolidayList.Any())
                        {
                            holiDayDetailsDto = new List<HolidayDetailListDto>();
                            foreach (var holiday in item.HolidayList)
                            {
                                var holidayDetails = new HolidayDetailListDto()
                                {
                                    HolidayDate = holiday.HolidayDate,
                                    Day = holiday.HolidayDate.ToString("dddd"),
                                    Remarks = holiday.Description
                                };

                                holiDayDetailsDto.Add(holidayDetails);
                                holiDayDto.HolidayDetails = holiDayDetailsDto;
                            }
                        }
                        holiDayListDto.Add(holiDayDto);
                    }
                    return _resultService.SuccessObject(holiDayListDto);
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.RoleNotFound);
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        #endregion

        #region Permanent Coverage Plan
        public ResultDto GetTotalPCPByUsers(IdInputDto idInputDto)
        {
            _methodName = "GetTotalPCPByUsers";
            var resultDto = new ResultDto();
            var totalPCPlistDto = new List<TotalPCPDto>();
            try
            {
                if (idInputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var pcpContext = _emamiContext.PermanentJourneyPlans.AsNoTracking().Where(_ => _.FinancialYearId == idInputDto.Id && _.CreatedBy == idInputDto.LoginUserId &&
                _.PermanentJourneyPlanStatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Approved).ToList();

                if (pcpContext != null)
                {
                    foreach (var data in pcpContext)
                    {
                     var pcpCityContext = _emamiContext.PermanentJourneyPlanDetails.AsNoTracking().Where(_ => _.PermanentJourneyPlanId == data.Id).Distinct().Select(_ => new PermanentJourneyPlanDetailsDto()
                    {
                        CityId = _.TownId
                    }).Distinct().ToList();
                    if (pcpCityContext != null)
                    {
                        foreach (var item in pcpCityContext)
                        {
                            var pcpDetails = _emamiContext.PermanentJourneyPlanDetails.AsNoTracking().Where(_ => _.PermanentJourneyPlanId == data.Id && _.TownId == item.CityId).ToList();
                            string dealerNames = string.Empty;
                            if (pcpDetails != null)
                            {
                                foreach (var detail in pcpDetails)
                                {
                                    if (!string.IsNullOrEmpty(detail.RetailerId) && detail.RetailerId != "0")
                                    {
                                        var dealerId = long.Parse(detail.RetailerId);
                                        var dealerDetail = _emamiContext.Users.FirstOrDefault(_ => _.Id == dealerId);
                                        dealerNames = dealerDetail == null ? string.Empty : dealerDetail.Name + "," + dealerNames;
                                    }
                                }
                                var TotalPCPDto = new TotalPCPDto
                                {
                                    CityId = item.CityId,
                                    City = _emamiContext.City.FirstOrDefault(_ => _.Id == item.CityId).CityName,
                                    Dealers = dealerNames.Length > 0 ? dealerNames.Remove(dealerNames.Length - 1, 1) : string.Empty,
                                    NoOfDealers = pcpDetails.Count(),
                                    NoOfVisit = pcpDetails.Where(_ => _.InHQNoVisit == 0).Sum(_ => _.NoOfVisit),
                                    HQVisitCount = pcpDetails.Where(_ => _.InHQNoVisit != 0).Sum(_ => _.NoOfVisit)
                                };
                                totalPCPlistDto.Add(TotalPCPDto);
                            }
                        }
                    }
                  }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = totalPCPlistDto;
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

        #region Today Activities
        public ResultDto AddDealerVisit(AddDealerVisitDto addDealerVisitDto)
        {
            _methodName = "AddDealerVisit";
            var resultDto = new ResultDto();
            try
            {
                if (addDealerVisitDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (addDealerVisitDto.CreatedBy == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == addDealerVisitDto.CreatedBy);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }

                if (addDealerVisitDto.AddPendingSaudaRemarksDto != null)
                {
                    foreach (var addPendingSaudaRemarksDto in addDealerVisitDto.AddPendingSaudaRemarksDto)
                    {
                        var psContext = new PendingSaudaRemarks
                        {
                            DealerId = addPendingSaudaRemarksDto.DealerId,
                            SaudaId = addPendingSaudaRemarksDto.SaudaId,
                            Remarks = addPendingSaudaRemarksDto.Remarks,
                            CreatedBy = addDealerVisitDto.CreatedBy,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.PendingSaudaRemarks.Add(psContext);
                    }
                    _emamiContext.SaveChanges();
                }

                if (addDealerVisitDto.AddMarketScenarioDto != null)
                {
                    foreach (var addMarketScenarioDto in addDealerVisitDto.AddMarketScenarioDto)
                    {
                        var marketScenarioContext = new MarketScenario
                        {
                            DealerId = addMarketScenarioDto.DealerId,
                            Title = addMarketScenarioDto.Title,
                            Remarks = addMarketScenarioDto.Remarks,
                            CreatedBy = addDealerVisitDto.CreatedBy,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.MarketScenario.Add(marketScenarioContext);
                    }
                    _emamiContext.SaveChanges();
                }

                if (addDealerVisitDto.BdoCompetitorAddDto != null)
                {
                    foreach (var bdoCompetitorAddDto in addDealerVisitDto.BdoCompetitorAddDto)
                    {
                        var competitorContext = new BdoCompetitor
                        {
                            Name = bdoCompetitorAddDto.Name,
                            Remarks = bdoCompetitorAddDto.Remarks,
                            IsActive = bdoCompetitorAddDto.IsActive,
                            UserType = bdoCompetitorAddDto.UserType,
                            DealerId = bdoCompetitorAddDto.DealerId,
                            CreatedBy = addDealerVisitDto.CreatedBy,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.BdoCompetitor.Add(competitorContext);
                        _emamiContext.SaveChanges();

                        foreach (var details in bdoCompetitorAddDto.BdoCompetitorSkuDetails)
                        {
                            var detailContext = new BdoCompetitorSku
                            {
                                BdoCompetitorId = competitorContext.Id,
                                SkuName = details.SkuName,
                                QuanityPerMt = details.QuanityPerMt,
                                Price = details.Price,
                                CreatedBy = addDealerVisitDto.CreatedBy,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                            };
                            _emamiContext.BdoCompetitorSku.Add(detailContext);
                        }
                        _emamiContext.SaveChanges();

                        foreach (var file in bdoCompetitorAddDto.FileList)
                        {
                            var attachmentContext = _emamiContext.Attachment.FirstOrDefault(_ => _.Id == file.Id);
                            if (attachmentContext != null)
                            {
                                attachmentContext.RecordId = competitorContext.Id;
                                _emamiContext.SaveChanges();
                            }
                        }
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.SuccessMessage;
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

        #region Monthly Plan Deviation
        public ResultDto PendingMonthlyPlanDeviationForMobile(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "ApprovedMonthlyPlanDeviation";
            var resultDto = new ResultDto();
            var permanentJourneyPlansDto = new List<MonthlyTourPlanDeviationDto>();
            try
            {
                var pendingmtp = (from mpd in _emamiContext.MonthlyPlanDeviation
                                  join mtpd in _emamiContext.MonthlyTourPlanDetails on mpd.MonthlyTourPlanDetailsId equals mtpd.Id
                                  join mtds in _emamiContext.MonthlyPlanDeviationStatus on mpd.StatusId equals mtds.Id
                                  where mpd.CreatedBy == loginUserIdDto.LoginUserId && mpd.StatusId == (int)DTO.Enums.MonthlyPlanDeviationStatus.Pending
                                  select new
                                  {
                                      MTPDetailId = mpd.MonthlyTourPlanDetailsId,
                                      DealerId = mtpd.DealerId,
                                      ActualDate = mtpd.Date,
                                      RevisedDate = mpd.RevisedDate,
                                      ToDealerId = mpd.ToDealerId,
                                      Remarks = mpd.Remarks != null ? mpd.Remarks : string.Empty,
                                      Status = mtds.Status,
                                      TownId = mtpd.TownId,
                                      ReasonId = mpd.ReasonId,
                                      InHQNoVisitId = mtpd.InHQNoVisit,
                                  }
                                   ).Distinct().ToList();

                if (pendingmtp != null)
                {
                    foreach (var c in pendingmtp)
                    {
                        var detailContext = new MonthlyTourPlanDeviationDto
                        {
                            MTPDetailId = c.MTPDetailId,
                            DealerId = c.DealerId,
                            ActualDate = c.ActualDate.ToString("dd-MMM-yyyy"),
                            Remarks = c.Remarks,
                            ToDealerId = c.ToDealerId,
                            RevisedDate = c.RevisedDate.ToString("dd-MMM-yyyy"),
                            Status = c.Status,
                            Town = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == c.TownId).CityName,
                            Reason = _emamiContext.Reasons.AsNoTracking().FirstOrDefault(_ => _.Id == c.ReasonId)?.Reason,
                            InHQNoVisitId = c.InHQNoVisitId,
                            InHQNoVisitName = c.InHQNoVisitId != 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.STPVisitType)c.InHQNoVisitId) : string.Empty,
                        };

                        if (!string.IsNullOrEmpty(detailContext.DealerId) && detailContext.DealerId!="0")
                        {
                            var dealerIdsList = detailContext.DealerId.Split(',');
                            var dealerNames = string.Empty;
                            foreach (var dealer in dealerIdsList)
                            {
                                var dealerId = long.Parse(dealer);
                                var dealerDetail = _emamiContext.Users.FirstOrDefault(_ => _.Id == dealerId);
                                dealerNames = dealerDetail == null ? string.Empty : dealerDetail.Name + "," + dealerNames;
                            }
                            detailContext.Dealer = dealerNames.Remove(dealerNames.Length - 1, 1);
                        }
                        else
                        {
                            detailContext.Dealer = string.Empty;
                        }

                        permanentJourneyPlansDto.Add(detailContext);
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = permanentJourneyPlansDto;
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

        public ResultDto ApprovedMonthlyPlanDeviationForMobile(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "ApprovedMonthlyPlanDeviation";
            var resultDto = new ResultDto();
            var permanentJourneyPlansDto = new List<MonthlyTourPlanDeviationDto>();
            try
            {
                var pendingmtp = (from mpd in _emamiContext.MonthlyPlanDeviation
                                  join mtpd in _emamiContext.MonthlyTourPlanDetails on mpd.MonthlyTourPlanDetailsId equals mtpd.Id
                                  join mtds in _emamiContext.MonthlyPlanDeviationStatus on mpd.StatusId equals mtds.Id
                                  where mpd.CreatedBy == loginUserIdDto.LoginUserId && mpd.StatusId == (int)DTO.Enums.MonthlyPlanDeviationStatus.Approved
                                  select new
                                  {
                                      MTPDetailId = mpd.MonthlyTourPlanDetailsId,
                                      DealerId = mtpd.DealerId,
                                      ActualDate = mtpd.Date,
                                      ToDealerId = mpd.ToDealerId,
                                      RevisedDate = mpd.RevisedDate,
                                      Remarks = mpd.Remarks,
                                      Status = mtds.Status,
                                      TownId = mtpd.TownId,
                                      ReasonId = mpd.ReasonId,
                                      InHQNoVisitId = mtpd.InHQNoVisit,
                                  }
                                   ).Distinct().ToList();

                if (pendingmtp != null)
                {
                    foreach (var c in pendingmtp)
                    {
                        var detailContext = new MonthlyTourPlanDeviationDto
                        {
                            MTPDetailId = c.MTPDetailId,
                            DealerId = c.DealerId,
                            ActualDate = c.ActualDate.ToString("dd-MMM-yyyy"),
                            Remarks = c.Remarks,
                            ToDealerId = c.ToDealerId,
                            RevisedDate = c.RevisedDate.ToString("dd-MMM-yyyy"),
                            Status = c.Status,
                            Town = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == c.TownId).CityName,
                            Reason = _emamiContext.Reasons.AsNoTracking().FirstOrDefault(_ => _.Id == c.ReasonId)?.Reason,
                            InHQNoVisitId = c.InHQNoVisitId,
                            InHQNoVisitName = c.InHQNoVisitId != 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.STPVisitType)c.InHQNoVisitId) : string.Empty,
                        };

                        if (!string.IsNullOrEmpty(detailContext.DealerId) && detailContext.DealerId!="0")
                        {
                            var dealerIdsList = detailContext.DealerId.Split(',');
                            var dealerNames = string.Empty;
                            foreach (var dealer in dealerIdsList)
                            {
                                var dealerId = long.Parse(dealer);
                                var dealerDetail = _emamiContext.Users.FirstOrDefault(_ => _.Id == dealerId);
                                dealerNames = dealerDetail == null ? string.Empty : dealerDetail.Name + "," + dealerNames;
                            }
                            detailContext.Dealer = dealerNames.Remove(dealerNames.Length - 1, 1);
                        }
                        else
                        {
                            detailContext.Dealer = string.Empty;
                        }

                        permanentJourneyPlansDto.Add(detailContext);
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = permanentJourneyPlansDto;
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

        public ResultDto AddMonthlyPlanDeviation(MonthlyPlanDeviationListDto addMonthlyPlanDeviationDto)
        {
            _methodName = "AddMonthlyPlanDeviation";
            var resultDto = new ResultDto();
            try
            {
                if (addMonthlyPlanDeviationDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (addMonthlyPlanDeviationDto.CreatedBy == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == addMonthlyPlanDeviationDto.CreatedBy);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }
                if (userContext.ReportingToId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.PJPApprovalFlowEmpty;
                    resultDto.ErrorDto.Message = Constants.PJPApprovalFlowEmpty;
                    return resultDto;
                }

                var monthlyPlanDetailContext = _emamiContext.MonthlyTourPlanDetails.AsNoTracking().FirstOrDefault(_ => _.Id == addMonthlyPlanDeviationDto.MonthlyTourPlanDetailsId);
                if(monthlyPlanDetailContext==null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.MTPNotFound;
                    resultDto.ErrorDto.Message = Constants.MTPNotFound;
                    return resultDto;
                }
                List<string> existDateList = new List<string>();
                bool isError = false;

                DateTime mtpDate = Convert.ToDateTime(addMonthlyPlanDeviationDto.RevisedDate);
                if (!string.IsNullOrEmpty(monthlyPlanDetailContext.DealerId) && monthlyPlanDetailContext.DealerId!="0")
                {
                    var noVisitExists = _emamiContext.MonthlyTourPlanDetails.AsNoTracking().FirstOrDefault(_ => DbFunctions.TruncateTime(_.Date) == DbFunctions.TruncateTime(mtpDate)
                        && _.InHQNoVisit != 0 && _.CreatedBy == addMonthlyPlanDeviationDto.CreatedBy && _.MonthlyTourPlan.MonthlyTourPlanStatusId != (int)DTO.Enums.MonthlyTourPlanStatus.Rejected);
                    if (noVisitExists != null)
                    {
                        isError = true;
                        existDateList.Add(addMonthlyPlanDeviationDto.RevisedDate);
                    }
                }
                if (monthlyPlanDetailContext.InHQNoVisit!=0)
                {
                    var mtpExists = _emamiContext.MonthlyTourPlanDetails.AsNoTracking().FirstOrDefault(_ => DbFunctions.TruncateTime(_.Date) == DbFunctions.TruncateTime(mtpDate)
                        && _.CreatedBy == addMonthlyPlanDeviationDto.CreatedBy && _.MonthlyTourPlan.MonthlyTourPlanStatusId != (int)DTO.Enums.MonthlyTourPlanStatus.Rejected);
                    if (mtpExists != null)
                    {
                        isError = true;
                        existDateList.Add(addMonthlyPlanDeviationDto.RevisedDate);
                    }
                }

                if (isError)
                {
                    existDateList = existDateList.Distinct().ToList();
                    string existDates = string.Join(",", existDateList);
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.MTPAlreadyExists.Replace(Constants.MTPExistingDates, existDates);
                    resultDto.ErrorDto.Message = Constants.MTPAlreadyExists.Replace(Constants.MTPExistingDates, existDates);
                    return resultDto;
                }

                var reasonContext = _emamiContext.Reasons.AsNoTracking().FirstOrDefault(_ => _.Reason == addMonthlyPlanDeviationDto.Reasons);
                var mpdContext = new MonthlyPlanDeviations
                {
                    MonthlyTourPlanDetailsId = addMonthlyPlanDeviationDto.MonthlyTourPlanDetailsId,
                    ToDealerId = addMonthlyPlanDeviationDto.ToDealerId,
                    RevisedDate = Convert.ToDateTime(addMonthlyPlanDeviationDto.RevisedDate),
                    Remarks = addMonthlyPlanDeviationDto.Remarks,
                    ReasonId = reasonContext.Id,
                    ApproverId = userContext.ReportingToId ?? 0,
                    StatusId = (int)DTO.Enums.MonthlyPlanDeviationStatus.Pending,
                    CreatedBy = addMonthlyPlanDeviationDto.CreatedBy,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                };

                _emamiContext.MonthlyPlanDeviation.Add(mpdContext);
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = addMonthlyPlanDeviationDto;
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
        #region Chart

        public List<MonthDto> GetMonthListfromInput(DateTime FromDate, DateTime ToDate)
        {
            _methodName = "GetMonthListfromInput";
            try
            {
                int totalMonths = 12 * (ToDate.Year - FromDate.Year) + ToDate.Month - FromDate.Month;
                List<MonthDto> months = new List<MonthDto>();
                int startMonth = FromDate.Month;
                int endMonth = ToDate.Month;
                int month = startMonth - 1;
                int year = FromDate.Year;
                for (var i = 0; i <= totalMonths; i++)
                {
                    MonthDto toaddmonth = new MonthDto();
                    if (month == 12)
                    {
                        month = 0;
                        year = year + 1;
                    }
                    month = month + 1;
                    toaddmonth.Id = month;
                    toaddmonth.Year = year;
                    toaddmonth.StartDate = new DateTime(year, month, 1);
                    toaddmonth.EndDate = toaddmonth.StartDate.AddMonths(1).AddDays(-1);
                    months.Add(toaddmonth);
                }
                return months;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ResultDto SalesTourPlanChart(SalesTourPlanInputDto inputDto)
        {
            _methodName = "SalesTourPlanChart";
            var resultDto = new ResultDto();
            var salesTourPlanOutputDto = new List<SalesTourPlanOutputDto>();
            if (inputDto == null)
            {
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                return resultDto;
            }
            try
            {
                var pjpContext = _emamiContext.PermanentJourneyPlans.AsNoTracking().Where(_ => _.CreatedBy == inputDto.LoginUserId && _.FinancialYearId == inputDto.FinancialYearId && _.PermanentJourneyPlanStatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Approved).ToList();
                if (pjpContext != null && pjpContext.Count > 0)
                {
                    List<MonthDto> months = new List<MonthDto>();
                    months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);
                    foreach (var month in months)
                    {
                        long PlannedVisit = 0;
                        long DeviatedVisit = 0;
                        long ActualVisit = 0;
                        foreach (var pjp in pjpContext)
                        {
                            var mtpContext = _emamiContext.MonthlyTourPlans.AsNoTracking().FirstOrDefault(_ => _.PJPId == pjp.Id && _.MonthId == month.Id && _.MonthlyTourPlanStatusId == (int)DTO.Enums.MonthlyTourPlanStatus.Approved);

                            if (mtpContext != null)
                            {
                                var PlannedContext = _emamiContext.MonthlyTourPlanDetails.AsNoTracking().Where(_ => _.MonthlyTourPlanId == mtpContext.Id).Count();
                                var DeviatedContext = (from mtpd in _emamiContext.MonthlyTourPlanDetails.AsNoTracking()
                                                       join mtp in _emamiContext.MonthlyTourPlans.AsNoTracking() on mtpd.MonthlyTourPlanId equals mtp.Id
                                                       join mpd in _emamiContext.MonthlyPlanDeviation.AsNoTracking() on mtpd.Id equals mpd.MonthlyTourPlanDetailsId
                                                       where mtpd.CreatedBy == inputDto.LoginUserId && mpd.StatusId == (int)DTO.Enums.MonthlyPlanDeviationStatus.Approved
                                                       && mtp.MonthId == month.Id
                                                       && mtp.Id == mtpContext.Id
                                                       select mpd
                                                        ).Count();

                                var ActualVisitContext = _emamiContext.MarketScenario.AsNoTracking().Where(_ => _.CreatedBy == inputDto.LoginUserId && DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(month.StartDate) && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(month.EndDate)).Distinct().Select(_ => new
                                {
                                    CreatedDate = DbFunctions.TruncateTime(_.CreatedDate)
                                }).Distinct().ToList();

                                PlannedVisit = PlannedVisit + PlannedContext;
                                DeviatedVisit = DeviatedVisit + DeviatedContext;
                                ActualVisit = ActualVisit + ActualVisitContext.Count();
                            }
                        }
                        if (PlannedVisit > 0)
                        {
                            var outputdto = new SalesTourPlanOutputDto
                            {
                                Month = month.Id,
                                PlannedVisit = PlannedVisit,
                                DeviatedVisit = DeviatedVisit,
                                ActualVisit = ActualVisit
                            };
                            salesTourPlanOutputDto.Add(outputdto);
                        }
                        else
                        {
                            var outputdto = new SalesTourPlanOutputDto
                            {
                                Month = month.Id,
                                PlannedVisit = 0,
                                DeviatedVisit = 0,
                                ActualVisit = 0
                            };
                            salesTourPlanOutputDto.Add(outputdto);
                        }
                    }
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = salesTourPlanOutputDto;
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


        #region Attachments
        public ResultDto SaveCompetitorImageName(ImageNameAddDto imageNameAddDto)
        {
            _methodName = "SaveIssueImageName";
            try
            {
                if (imageNameAddDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                //if (imageNameAddDto.RecordId == 0)
                //{
                //    return _resultService.ErrorMessage(Constants.IssueIdMissing);
                //}
                //if (string.IsNullOrEmpty(imageNameAddDto.Url))
                //{
                //    return _resultService.ErrorMessage(Constants.ImageNameEmpty);
                //}

                var issueImageContext = new Attachment
                {
                    PageId = imageNameAddDto.PageId,
                    Url = imageNameAddDto.Url,
                    CreatedBy = imageNameAddDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                };
                _emamiContext.Attachment.Add(issueImageContext);
                _emamiContext.SaveChanges();
                return _resultService.SuccessMessageWitObject(issueImageContext.Id, Constants.MediaSavedSuccessfully);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }

        }

        #endregion

        #region WholeSeller
        public ResultDto AddWholeSellerVisit(AddWholeSellerVisitDto addWholeSellerVisitDto)
        {
            _methodName = "AddWholeSellerVisit";
            var resultDto = new ResultDto();
            try
            {
                if (addWholeSellerVisitDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (addWholeSellerVisitDto.CreatedBy == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == addWholeSellerVisitDto.CreatedBy);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }

                if (addWholeSellerVisitDto.WholeSellerId == 0)
                {
                    var psContext = new WholesellerBdo
                    {
                        DealerId = addWholeSellerVisitDto.DealerId,
                        Name = addWholeSellerVisitDto.WholeSellerName,
                        CreatedBy = addWholeSellerVisitDto.CreatedBy,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    };
                    _emamiContext.WholesellerBdo.Add(psContext);
                    _emamiContext.SaveChanges();
                    addWholeSellerVisitDto.WholeSellerId = psContext.Id;
                }
                if (addWholeSellerVisitDto.WholeSellerSalesDetailDto != null && addWholeSellerVisitDto.WholeSellerSalesDetailDto.Any())
                {
                    foreach (var item in addWholeSellerVisitDto.WholeSellerSalesDetailDto)
                    {
                        var salesDetail = new WholeSellerSalesDetail
                        {
                            WholesellerBdoId = addWholeSellerVisitDto.WholeSellerId,
                            OilTypeId = item.OilTypeId,
                            SkuId = item.SkuId,
                            QuantityPerMt = item.QuantityPerMt,
                            Price = item.Price,
                            CreatedBy = addWholeSellerVisitDto.CreatedBy,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        _emamiContext.WholeSellerSalesDetail.Add(salesDetail);
                        _emamiContext.SaveChanges();
                    }
                }
                if (addWholeSellerVisitDto.BdoCompetitorAddDto != null && addWholeSellerVisitDto.BdoCompetitorAddDto.Any())
                {
                    foreach (var bdoCompetitorAddDto in addWholeSellerVisitDto.BdoCompetitorAddDto)
                    {
                        var competitorContext = new BdoCompetitor
                        {
                            Name = bdoCompetitorAddDto.Name,
                            Remarks = bdoCompetitorAddDto.Remarks,
                            IsActive = bdoCompetitorAddDto.IsActive,
                            UserType = bdoCompetitorAddDto.UserType,
                            DealerId = bdoCompetitorAddDto.DealerId,
                            CreatedBy = addWholeSellerVisitDto.CreatedBy,
                            BdoWholesellerId = addWholeSellerVisitDto.WholeSellerId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.BdoCompetitor.Add(competitorContext);
                        _emamiContext.SaveChanges();

                        foreach (var details in bdoCompetitorAddDto.BdoCompetitorSkuDetails)
                        {
                            var detailContext = new BdoCompetitorSku
                            {
                                BdoCompetitorId = competitorContext.Id,
                                SkuName = details.SkuName,
                                QuanityPerMt = details.QuanityPerMt,
                                Price = details.Price,
                                CreatedBy = addWholeSellerVisitDto.CreatedBy,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                            };
                            _emamiContext.BdoCompetitorSku.Add(detailContext);
                        }
                        _emamiContext.SaveChanges();

                        foreach (var file in bdoCompetitorAddDto.FileList)
                        {
                            var attachmentContext = _emamiContext.Attachment.FirstOrDefault(_ => _.Id == file.Id);
                            if (attachmentContext != null)
                            {
                                attachmentContext.RecordId = competitorContext.Id;
                                _emamiContext.SaveChanges();
                            }
                        }
                    }
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.SuccessMessage;
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
        public ResultDto GetWholeSellerList(IdInputDto idInputDto)
        {
            _methodName = "GetWholeSellerList";
            var resultDto = new ResultDto();
            var wholesellerList = new List<WholesellerDto>();
            try
            {
                if (idInputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var WholesellerContext = _emamiContext.WholesellerBdo.AsNoTracking().Where(_ => _.DealerId == idInputDto.Id).ToList();
                if (WholesellerContext != null && WholesellerContext.Any())
                {
                    foreach (var item in WholesellerContext)
                    {
                        var wholeseller = new WholesellerDto
                        {
                            Id = item.Id,
                            Name = item.Name
                        };
                        wholesellerList.Add(wholeseller);
                    }
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = wholesellerList;
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
        #region Secondary Sales For the Day
        public ResultDto GetSecondarySalesFortheDay(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetSecondarySalesFortheDay";
            var resultDto = new ResultDto();
            var wholesellerList = new List<WholesellerSecondarySaleslistDto>();
            try
            {
                if (loginUserIdDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                DateTime StartDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                DateTime EndDate = new DateTime(currentDate.Year, currentDate.Month, CultureInfo.CurrentCulture.Calendar.GetDaysInMonth(currentDate.Year, currentDate.Month));
                var SalesDetailContext = _emamiContext.WholeSellerSalesDetail.AsNoTracking().Where(_ => _.CreatedBy == loginUserIdDto.LoginUserId && DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(StartDate) && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(EndDate))
                                            .Select(_ => DbFunctions.TruncateTime(_.CreatedDate)).Distinct().ToList();
                if (SalesDetailContext != null && SalesDetailContext.Any())
                {
                    foreach (var VisitDate in SalesDetailContext)
                    {
                        var wholesellerdto = new WholesellerSecondarySaleslistDto();
                        wholesellerdto.VisitDate = VisitDate;
                        var SalesContext = (from sc in _emamiContext.WholeSellerSalesDetail
                                            where DbFunctions.TruncateTime(sc.CreatedDate) == DbFunctions.TruncateTime(VisitDate)
                                            && sc.CreatedBy == loginUserIdDto.LoginUserId
                                            group sc by sc.WholesellerBdoId into groupResult
                                            select new
                                            {
                                                WholeSellerId = groupResult.Key,
                                                TotalQuantity = groupResult.Sum(f => f.QuantityPerMt),
                                                TotalPrice = groupResult.Sum(f => f.Price),
                                            }).ToList();
                        if (SalesContext != null && SalesContext.Any())
                        {
                            foreach (var item in SalesContext)
                            {
                                var WholesellerContext = _emamiContext.WholesellerBdo.AsNoTracking().FirstOrDefault(_ => _.Id == item.WholeSellerId);
                                var WholesellerSecondarySaleslistDto = new WholesellerSecondarySalesDto
                                {
                                    DealerId = WholesellerContext.DealerId,
                                    Dealer = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == WholesellerContext.DealerId).Name,
                                    WholesellerId = WholesellerContext.Id,
                                    Name = WholesellerContext.Name,
                                    TotalPrice = item.TotalPrice,
                                    TotalQuantity = item.TotalQuantity,
                                    VisitDate = VisitDate
                                };
                                wholesellerdto.WholesellerSecondarySales.Add(WholesellerSecondarySaleslistDto);
                            }
                        }
                        wholesellerList.Add(wholesellerdto);
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = wholesellerList;
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

        public ResultDto GetSecondarySalesDetails(WholesellerSecondarySalesInputDto secondarySalesInputDto)
        {
            _methodName = "GetSecondarySalesFortheDay";
            var resultDto = new ResultDto();
            var wholesellerList = new List<WholesellerSecondarySalesDetailOutputDto>();
            try
            {
                if (secondarySalesInputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var salesDetailContext = _emamiContext.WholeSellerSalesDetail.AsNoTracking().Where(_ => DbFunctions.TruncateTime(_.CreatedDate) == DbFunctions.TruncateTime(secondarySalesInputDto.VisitDate) && _.WholesellerBdoId == secondarySalesInputDto.WholesellerId).ToList();
                if (salesDetailContext != null && salesDetailContext.Any())
                {
                    foreach (var item in salesDetailContext)
                    {
                        var wholesellerDto = new WholesellerSecondarySalesDetailOutputDto()
                        {
                            WholesellerId = item.WholesellerBdoId,
                            OilTypeId = item.OilTypeId,
                            SkuId = item.SkuId,
                            OilType = _emamiContext.OilTypes.FirstOrDefault(_ => _.Id == item.OilTypeId).Name,
                            SkuName = item.Sku.SkuName,
                            Price = item.Price,
                            Quantity = item.QuantityPerMt,
                            Wholeseller = _emamiContext.WholesellerBdo.FirstOrDefault(_ => _.Id == item.WholesellerBdoId).Name
                        };
                        wholesellerList.Add(wholesellerDto);
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = wholesellerList;
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
