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
using System.Data;

namespace Adani.Solution.Service
{
    public interface ISalesTourPlanService
    {
        ResultDto AddPermanentJourneyPlan(PermanentJouneyPlanAddDto permanentJouneyPlanAddDto);
        ResultDto GetPermanentJourneyPlanDetails(PJPIdDto pJPIdDto);
        ResultDto GetPermanentJourneyPlanList(LoginUserIdDto loginUserIdDto);
        ResultDto UpdatePermanentJourneyPlan(PermanentJourneyPlanUpdateDto permanentJouneyPlanUpdateDto);
        ResultDto GetPendingPermanentJourneyPlanList(LoginUserIdDto loginUserIdDto);

        ResultDto AddMonthlyTourPlan(MonthlyTourPlanAddDto monthlyTourPlanAddDto);
        ResultDto UpdateMonthlyTourPlan(MonthlyTourPlanUpdateDto monthlyTourPlanUpdateDto);
        ResultDto GetMonthlyTourPlanDetails(MTPIdDto mtpIdDto);
        ResultDto GetMonthlyTourPlanList(LoginUserIdDto loginUserIdDto);
        ResultDto GetPendingMonthlyTourPlanList(LoginUserIdDto loginUserIdDto);

        ResultDto GetFinancialYear();
        ResultDto AddFinancialYear(FinancialYearAddDto financialYearAddDto);
        ResultDto UpdateFinancialYear(FinancialYearDto financialYearDto);
        ResultDto ViewFinancialYear(FinancialYearIdDto financialYearIdDto);
        ResultDto GetActiveFinancialYear();
        ResultDto GetCities();

        ResultDto GetHeadquarters();
        ResultDto AddHeadquarters(HeadquartersAddDto headquartersAddDto);
        ResultDto UpdateHeadquarters(HeadquartersUpdateDto headquartersupdateDto);
        ResultDto ViewHeadquarters(HeadquartersIdDto headquartersIdDto);
        ResultDto GetActiveHeadquarters();
        ResultDto ExportHeadQuarters(LoginUserIdDto loginUserIdDto);

        ResultDto GetReasons();
        ResultDto AddReason(ReasonAddDto reasonAddDto);
        ResultDto UpdateReason(ReasonUpdateDto reasonupdateDto);
        ResultDto ViewReasons(ReasonIdDto reasonIdDto);
        ResultDto GetActiveReasons();
        ResultDto GetDealers();

        ResultDto GetDateWeekDetails();
        ResultDto GetPJPMonths(FinancialYearIdDto financialYearIdDto);
        ResultDto ApprovedPermanentJourneyPlanByUser(LoginUserIdDto loginUserIdDto);
        ResultDto MonthsByUserPermanentJourneyPlan(PJPIdDto pJPIdDto);
        ResultDto DealersByUserPermanentJourneyPlan(PJPIdDto pJPIdDto);
        ResultDto MonthlyTourPlanDateCalendar(PermanentJourneyPlanDetailsDto permanentJourneyPlanDetailsDto);
        ResultDto CityByUserPermanentJourneyPlan(PermanentJourneyPlanDetailsDto permanentJourneyPlanDetailsDto);
        ResultDto NoVisitByUserPermanentJourneyPlan(PJPIdDto pJPIdDto);

        ResultDto ApprovedMonthlyTourPlanDetailsByUser(MTPIdDto MTPIdDto);
        ResultDto ApprovedMonthlyTourPlanByUser(LoginUserIdDto loginUserIdDto);
        ResultDto AddMonthlyPlanDeviation(AddMonthlyPlanDeviationDto addMonthlyPlanDeviationDto);
        ResultDto PendingMonthlyPlanDeviation(LoginUserIdDto loginUserIdDto);
        ResultDto ApprovedMonthlyPlanDeviation(LoginUserIdDto loginUserIdDto);
        ResultDto CheckMonthlyPlanDeviationApproveByLoginedUser(LoginUserIdDto loginUserIdDto);

        ResultDto UpdateMonthlyPlanDeviation(MonthlyPlanDeviationUpdateDto monthlyPlanDeviationUpdateDto);
        ResultDto GetApprovedOrRejectedPJPList(LoginUserIdDto loginUserIdDto);
        ResultDto GetApprovedOrRejectedMTPList(LoginUserIdDto loginUserIdDto);
        ResultDto GetApprovedPJPList(LoginUserIdDto loginUserIdDto);
        ResultDto GetRejectedPJPList(LoginUserIdDto loginUserIdDto);
        ResultDto GetPendingPJPList(LoginUserIdDto loginUserIdDto);

        ResultDto RejectedMonthlyPlanDeviationForMobile(LoginUserIdDto loginUserIdDto);
        ResultDto TodayActivities(TodayActivitiesInputDto todayActivitiesInputDto);
        ResultDto TodayActivitiesDealerList(TodayActivitiesInputDto todayActivitiesInputDto);
        ResultDto AddPendingSauda(AddPendingSaudaRemarksDto addPendingSaudaRemarksDto);
        ResultDto GetPendingSaudaList(PendingSaudaInputDto inputDto);
        ResultDto AddMarketScenario(AddMarketScenarioDto addMarketScenarioDto);
        ResultDto AddBDOCompetitorDetails(BdoCompetitorAddListDto bdoCompetitorAddDto);
        ResultDto AddProspectiveDealer(ProspectiveDealerAddListDto prospectiveDealerAddDto);
        ResultDto GetProspectiveDealerList(SalesTourPlanParamDto inputDto);
        ResultDto GetProspectiveDealerById(IdInputDto inputDto);
        ResultDto ViewMonthlyTourPlanDeviationDetails(IdInputDto idInputDto);

        ResultDto GetMonthsByFinancialYear(FinancialYearIdDto financialYearIdDto);
        ResultDto GetWholeSellerCompetitorsList(SalesTourPlanParamDto inputDto);
        ResultDto GetSecondarySalesDetails(WholesellerSecondarySalesInputDto secondarySalesInputDto);
        ResultDto GetSecondarySalesFortheDayByWholeseller(SecondarySalesInputDto secondarySalesInputDto);
        //ResultDto AddUserSalesSaudaTarget(UserSalesSaudaTargetDto userSalesSaudaTargetDto);
        //ResultDto UpdateUserSalesSaudaTarget(UserSalesSaudaTargetDto userSalesSaudaTargetDto);
        //ResultDto GetUserSalesSaudaTarget(IdInputDto idInputDto);
        //ResultDto GetUserSalesSaudaTargetList();
        //ResultDto GetUserSalesSaudaTargetDetailList(UserSalesSaudaTargetDto userSalesSaudaTargetDto);
        ResultDto GetSecondarySalesFortheDayByWholesellerForWeb(SecondarySalesInputDto secondarySalesInputDto);
        ResultDto GetFileAttachments(AttachmentInputDto inputDto);


        #region User Sales Target

        ResultDto GetMonthAndYearByFinancialYear(FinancialYearIdDto financialYearIdDto);

        //ResultDto GetOilTypeTargetList();
        //ResultDto GetOilTypeTargetDetailList(UserOilTypeTargetIdDto inputDto);
        //ResultDto AddUserOilTypeTarget(UserOilTypeTargetDto inputDto);
        //ResultDto UpdateUserOilTypeTarget(UserOilTypeTargetDto inputDto);
        //ResultDto GetUserOilTypeTargetDetailsById(UserOilTypeTargetIdDto inputDto);

        ResultDto GetUserCustomerSalesTargetList(LoginUserIdDto inputDto);
        ResultDto GetUserCustomerSalesTargetDetailList(UserTargetIdDto inputDto);
        ResultDto AddUserCustomerSalesTarget(UserCustomerSalesTargetDto inputDto);
        ResultDto UpdateUserCustomerSalesTarget(UserCustomerSalesTargetDto inputDto);
        ResultDto GetUserCustomerSalesTargetDetailsById(UserTargetIdDto inputDto);
        ResultDto SaveUserCustomerSalesTargetList(List<MapSalesTargetDetailDto> targetDetailDtoList);
        ResultDto GetAssignedSalesTargetList(LoginUserIdDto inputDto);
        ResultDto GetSalesTargetOilTypeList(UserTargetIdDto inputDto);

        ResultDto GetOilTypesBasedOnAssignedSalesTarget(UserTargetIdDto inputDto);
        #endregion

        #region UserCustomerSauda Target

        ResultDto GetUserCustomerSaudaTargetDetailList(UserTargetIdDto inputDto);
        ResultDto GetUserCustomerSaudaTargetList(LoginUserIdDto inputDto);
        ResultDto AddUserCustomerSaudaTarget(UserCustomerSaudaTargetDto inputDto);
        ResultDto UpdateUserCustomerSaudaTarget(UserCustomerSaudaTargetDto inputDto);
        ResultDto GetUserCustomerSaudaTargetDetailsById(UserTargetIdDto inputDto);
        ResultDto SaveUserCustomerSaudaTargetList(List<MapSaudaTargetDetailDto> targetDetailDtoList);
        ResultDto GetAssignedSaudaTargetList(LoginUserIdDto inputDto);
        ResultDto GetSaudaTargetOilTypeList(UserTargetIdDto inputDto);

        ResultDto GetOilTypesBasedOnAssignedSaudaTarget(UserTargetIdDto inputDto);
        #endregion

        ResultDto GetMTPDetailsForCurrentMonth(TodayActivitiesInputDto inputDto);

        #region Activity Details

        ResultDto GetProspectiveDealers(SalesTourPlanParamDto inputDto);
        ResultDto GetPendingSaudaRemarksList(SalesTourPlanParamDto inputDto);
        ResultDto GetMarketScenariosList(SalesTourPlanParamDto inputDto);
        ResultDto GetCompetitorsList(SalesTourPlanParamDto inputDto);
        ResultDto GetCompetitorSkuList(SalesTourPlanParamDto inputDto);

        #endregion

        ResultDto GetUserAttendence(UserAttendenceInputDto inputDto);

        #region User Customer Target

        ResultDto SaveUserCustomerTargetList(List<MapSalesTargetDetailDto> targetDetailDtoList);
        ResultDto GetUserCustomerTargetList(LoginUserIdDto inputDto);
        ResultDto GetUserCustomerTargetDetailList(UserTargetIdDto inputDto);
        ResultDto UpdateUserCustomerTarget(List<MapSalesTargetDetailDto> targetDetailDtoList);
        ResultDto GetUserCustomerTargetDetailsById(UserTargetIdDto inputDto);
        ResultDto GetAssignedTargetList(LoginUserIdDto inputDto);
        #endregion

        ResultDto GetSalesTourPlanPcpHistory(long id);
        ResultDto GetSalesTourPlanMtpHistory(long id);

    }

    public class SalesTourPlanService : ISalesTourPlanService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("SalesTourPlan Service");
        private const string ServiceName = "SalesTourPlan Service";
        private string _methodName;
        private readonly IResultService _resultService;
        private readonly INotificationService _notificationService;

        public SalesTourPlanService(IAdaniContext emamiContext, IResultService resultService, INotificationService notificationService)
        {
            try
            {
                _emamiContext = emamiContext;
                _resultService = resultService;
                _notificationService = notificationService;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for Lookup Service", exception);
            }
        }

        #region FinancialYear

        public ResultDto GetFinancialYear()
        {
            _methodName = "GetFinancialYear";
            var resultDto = new ResultDto();
            var financialYearListDto = new List<FinancialYearDto>();
            try
            {
                financialYearListDto = _emamiContext.FinancialYears.AsNoTracking().OrderByDescending(_ => _.Year).AsEnumerable().Select(c => new FinancialYearDto
                {
                    EncryptedId = UtilityHelper.ConvertToMd5(c.Id.ToString(), SecurityConstants.EncryptionKey),
                    Id = c.Id,
                    Year = c.Year,
                    EffectiveFrom = c.EffectiveFrom,
                    EffectiveTo = c.EffectiveTo,
                    IsActive = c.IsActive

                }).ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = financialYearListDto;
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

        public ResultDto AddFinancialYear(FinancialYearAddDto financialYearAddDto)
        {
            _methodName = "AddFinancialYear";
            var resultDto = new ResultDto();
            try
            {
                if (financialYearAddDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }
                if (financialYearAddDto.Year == "")
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.YearIsEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.YearIsEmpty, Utility.MessageLanguage);
                    return resultDto;
                }
                if (financialYearAddDto.EffectiveFrom == null || financialYearAddDto.EffectiveFrom == DateTime.MinValue)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.EmptyFromMonth;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.EmptyFromMonth, Utility.MessageLanguage);
                    return resultDto;
                }
                if (financialYearAddDto.EffectiveTo == null || financialYearAddDto.EffectiveTo == DateTime.MinValue)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.EmptyToMonth;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.EmptyToMonth, Utility.MessageLanguage);
                    return resultDto;
                }
                var financialyearContextExist = _emamiContext.FinancialYears.AsNoTracking().Count(_ => _.Year == financialYearAddDto.Year);
                if (financialyearContextExist > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.YearExist;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.YearExist, Utility.MessageLanguage);
                    return resultDto;
                }
                var financialYearContext = new FinancialYear
                {
                    Year = financialYearAddDto.Year,
                    EffectiveFrom = financialYearAddDto.EffectiveFrom,
                    EffectiveTo = financialYearAddDto.EffectiveTo,
                    IsActive = financialYearAddDto.IsActive,
                    CreatedBy = financialYearAddDto.CreatedBy,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                };
                _emamiContext.FinancialYears.Add(financialYearContext);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.GetMessage(Constants.RecordSaved, Utility.MessageLanguage);
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

        public ResultDto UpdateFinancialYear(FinancialYearDto financialYearDto)
        {
            _methodName = "UpdateFinancialYear";
            var resultDto = new ResultDto();
            try
            {
                if (financialYearDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }
                if (financialYearDto.Id == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.IdEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.IdEmpty, Utility.MessageLanguage);
                    return resultDto;
                }
                if (financialYearDto.Year == "")
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.YearIsEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.YearIsEmpty, Utility.MessageLanguage);
                    return resultDto;
                }
                if (financialYearDto.EffectiveFrom == null || financialYearDto.EffectiveFrom == DateTime.MinValue)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.EmptyFromMonth;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.EmptyFromMonth, Utility.MessageLanguage);
                    return resultDto;
                }
                if (financialYearDto.EffectiveTo == null || financialYearDto.EffectiveTo == DateTime.MinValue)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.EmptyToMonth;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.EmptyToMonth, Utility.MessageLanguage);
                    return resultDto;
                }
                var financialyearContextExist = _emamiContext.FinancialYears.AsNoTracking().Count(_ => _.Year == financialYearDto.Year && _.Id != financialYearDto.Id);
                if (financialyearContextExist > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.YearExist;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.YearExist, Utility.MessageLanguage);
                    return resultDto;
                }
                var financialyearContext = _emamiContext.FinancialYears.FirstOrDefault(_ => _.Id == financialYearDto.Id);
                if (financialyearContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Utility.MessageLanguage);
                    return resultDto;
                }
                financialyearContext.Year = financialYearDto.Year;
                financialyearContext.EffectiveFrom = financialYearDto.EffectiveFrom;
                financialyearContext.EffectiveTo = financialYearDto.EffectiveTo;
                financialyearContext.IsActive = financialYearDto.IsActive;
                financialyearContext.ModifiedBy = financialYearDto.LoginUserId;
                financialyearContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.GetMessage(Constants.RecordSaved, Utility.MessageLanguage);
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

        public ResultDto ViewFinancialYear(FinancialYearIdDto financialYearIdDto)
        {
            _methodName = "ViewFinancialYear";
            var resultDto = new ResultDto();
            var financialYearDto = new FinancialYearDto();
            try
            {
                var financialYearContext = _emamiContext.FinancialYears.AsNoTracking().FirstOrDefault(_ => _.Id == financialYearIdDto.FinancialYearid);
                if (financialYearContext != null)
                {
                    financialYearDto.Id = financialYearContext.Id;
                    financialYearDto.Year = financialYearContext.Year;
                    financialYearDto.EffectiveFrom = financialYearContext.EffectiveFrom;
                    financialYearDto.EffectiveTo = financialYearContext.EffectiveTo;
                    financialYearDto.IsActive = financialYearContext.IsActive;

                    int totalMonths = 12 * (financialYearDto.EffectiveTo.Year - financialYearDto.EffectiveFrom.Year) + financialYearDto.EffectiveTo.Month - financialYearDto.EffectiveFrom.Month;
                    List<int> months = new List<int>();
                    int startMonth = financialYearContext.EffectiveFrom.Month;
                    int endMonth = financialYearContext.EffectiveTo.Month;
                    int month = startMonth - 1;
                    for (var i = 0; i <= totalMonths; i++)
                    {
                        if (month == 12)
                        {
                            month = 0;
                        }
                        month = month + 1;
                        months.Add(month);
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = financialYearDto;
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

        public ResultDto GetPJPMonths(FinancialYearIdDto financialYearIdDto)
        {
            _methodName = "GetPJPMonths";
            var resultDto = new ResultDto();
            var financialYearDto = new FinancialYearDto();
            var pjpMonthList = new List<Month>();
            try
            {
                var financialYearContext = _emamiContext.FinancialYears.AsNoTracking().FirstOrDefault(_ => _.Id == financialYearIdDto.FinancialYearid);
                if (financialYearContext != null)
                {
                    financialYearDto.EffectiveFrom = financialYearContext.EffectiveFrom;
                    financialYearDto.EffectiveTo = financialYearContext.EffectiveTo;

                    int totalMonths = 12 * (financialYearDto.EffectiveTo.Year - financialYearDto.EffectiveFrom.Year) + financialYearDto.EffectiveTo.Month - financialYearDto.EffectiveFrom.Month;
                    List<Month> months = new List<Month>();
                    int startMonth = financialYearContext.EffectiveFrom.Month;
                    int endMonth = financialYearContext.EffectiveTo.Month;
                    int month = startMonth - 1;
                    for (var i = 0; i <= totalMonths; i++)
                    {
                        Month toaddmonth = new Month();
                        if (month == 12)
                        {
                            month = 0;
                        }
                        month = month + 1;
                        toaddmonth.Id = month;
                        months.Add(toaddmonth);
                    }

                    var monthlist = _emamiContext.Months.AsNoTracking().ToList();
                    var pjpmonths = (from m in monthlist
                                     join t in months on m.Id equals t.Id
                                     select m).ToList();
                    pjpMonthList = pjpmonths;
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = pjpMonthList;
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

        public ResultDto GetActiveFinancialYear()
        {
            _methodName = "GetActiveFinancialYear";
            var resultDto = new ResultDto();
            var financialYearListDto = new List<FinancialYearDto>();
            try
            {
                financialYearListDto = _emamiContext.FinancialYears.Where(_ => _.IsActive).AsNoTracking().OrderByDescending(_ => _.Year).Select(c => new FinancialYearDto
                {
                    Id = c.Id,
                    Year = c.Year,
                    EffectiveFrom = c.EffectiveFrom,
                    EffectiveTo = c.EffectiveTo,
                    IsActive = c.IsActive

                }).ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = financialYearListDto;
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

        #region Headquarters

        public ResultDto GetHeadquarters()
        {
            _methodName = "GetHeadquarters";
            var resultDto = new ResultDto();
            var headquartersDto = new List<HeadquartersDto>();
            try
            {
                headquartersDto = _emamiContext.Headquarters.AsNoTracking().OrderByDescending(_ => _.Name).Select(c => new HeadquartersDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Address = c.Address,
                    IsActive = c.IsActive,
                    Zone = c.Zone.Name,
                    State = c.State.StateName,
                    Territory = c.Territory.Name,
                    District = c.District.DistrictName,
                    City = c.City.CityName
                }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = headquartersDto;



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

        public ResultDto AddHeadquarters(HeadquartersAddDto headquartersAddDto)
        {
            _methodName = "AddHeadquarters";
            var resultDto = new ResultDto();
            try
            {
                if (headquartersAddDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (string.IsNullOrEmpty(headquartersAddDto.Name))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.NameIsEmpty;
                    return resultDto;
                }
                var nameExist = _emamiContext.Headquarters.AsNoTracking().Count(_ => _.CityId == headquartersAddDto.CityId && _.DistrictId == headquartersAddDto.DistrictId
                && _.TerritoryId == headquartersAddDto.TerritoryId && _.StateId == headquartersAddDto.StateId && _.ZoneId == headquartersAddDto.ZoneId);

                if (nameExist > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.NameExist;
                    return resultDto;
                }

                var headquartersContext = new Headquarters
                {
                    Name = headquartersAddDto.Name.Trim(),
                    Address = headquartersAddDto.Address.Trim(),
                    IsActive = headquartersAddDto.IsActive,
                    ZoneId = headquartersAddDto.ZoneId,
                    StateId = headquartersAddDto.StateId,
                    TerritoryId = headquartersAddDto.TerritoryId,
                    DistrictId = headquartersAddDto.DistrictId,
                    CityId = headquartersAddDto.CityId,
                    CreatedBy = headquartersAddDto.CreatedBy,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                };
                _emamiContext.Headquarters.Add(headquartersContext);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.GetMessage(Constants.RecordSaved, Utility.MessageLanguage);
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

        public ResultDto UpdateHeadquarters(HeadquartersUpdateDto headquartersupdateDto)
        {
            _methodName = "UpdateHeadquarters";
            var resultDto = new ResultDto();
            try
            {
                if (headquartersupdateDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (headquartersupdateDto.Id == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.IdEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.IdEmpty, Utility.MessageLanguage);
                    return resultDto;
                }
                if (string.IsNullOrEmpty(headquartersupdateDto.Name))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.NameIsEmpty;
                    return resultDto;
                }
                //var hqContextExist = _emamiContext.Headquarters.AsNoTracking().Count(_ => _.Name == headquartersupdateDto.Name && _.Id != headquartersupdateDto.Id);
                var nameExist = _emamiContext.Headquarters.AsNoTracking().Count(_ => _.CityId == headquartersupdateDto.CityId && _.DistrictId == headquartersupdateDto.DistrictId
              && _.TerritoryId == headquartersupdateDto.TerritoryId && _.StateId == headquartersupdateDto.StateId && _.ZoneId == headquartersupdateDto.ZoneId && _.Id != headquartersupdateDto.Id);

                if (nameExist > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.NameExist;
                    return resultDto;
                }
                var hqContext = _emamiContext.Headquarters.FirstOrDefault(_ => _.Id == headquartersupdateDto.Id);
                if (hqContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Utility.MessageLanguage);
                    return resultDto;
                }

                hqContext.Name = headquartersupdateDto.Name.Trim();
                hqContext.Address = headquartersupdateDto.Address.Trim();
                hqContext.IsActive = headquartersupdateDto.IsActive;
                hqContext.ZoneId = headquartersupdateDto.ZoneId;
                hqContext.StateId = headquartersupdateDto.StateId;
                hqContext.TerritoryId = headquartersupdateDto.TerritoryId;
                hqContext.DistrictId = headquartersupdateDto.DistrictId;
                hqContext.CityId = headquartersupdateDto.CityId;
                hqContext.ModifiedBy = headquartersupdateDto.ModifiedBy;
                hqContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.GetMessage(Constants.RecordSaved, Utility.MessageLanguage);
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

        public ResultDto ViewHeadquarters(HeadquartersIdDto headquartersIdDto)
        {
            _methodName = "ViewHeadquarters";
            var resultDto = new ResultDto();
            var headquartersDto = new HeadquartersDto();
            try
            {
                var hqContext = _emamiContext.Headquarters.AsNoTracking().FirstOrDefault(_ => _.Id == headquartersIdDto.HeadquartersId);
                if (hqContext != null)
                {
                    headquartersDto.Id = hqContext.Id;
                    headquartersDto.Name = hqContext.Name;
                    headquartersDto.Address = hqContext.Address;
                    headquartersDto.IsActive = hqContext.IsActive;
                    headquartersDto.ZoneId = hqContext.ZoneId;
                    headquartersDto.StateId = hqContext.StateId;
                    headquartersDto.TerritoryId = hqContext.TerritoryId;
                    headquartersDto.DistrictId = hqContext.DistrictId;
                    headquartersDto.CityId = hqContext.CityId;
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = headquartersDto;
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

        public ResultDto GetActiveHeadquarters()
        {
            _methodName = "GetActiveHeadquarters";
            var resultDto = new ResultDto();
            var financialYearListDto = new List<HeadquartersDto>();
            try
            {
                financialYearListDto = _emamiContext.Headquarters.Where(_ => _.IsActive).AsNoTracking().OrderByDescending(_ => _.Name).Select(c => new HeadquartersDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Address = c.Address,
                    IsActive = c.IsActive

                }).ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = financialYearListDto;
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

        public ResultDto ExportHeadQuarters(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "ExportHeadquarters";
            var resultDto = new ResultDto();
            var headquartersDto = new List<HeadquartersDto>();
            try
            {
                headquartersDto = _emamiContext.Headquarters.AsNoTracking().OrderByDescending(_ => _.Name).Select(c => new HeadquartersDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Address = c.Address,
                    IsActive = c.IsActive,
                    Zone = c.Zone.Name,
                    State = c.State.StateName,
                    Territory = c.Territory.Name,
                    District = c.District.DistrictName,
                    City = c.City.CityName
                }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = headquartersDto;
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

        #region Reasons

        public ResultDto GetReasons()
        {
            _methodName = "GetReasons";
            var resultDto = new ResultDto();
            var reasonDto = new List<ReasonDto>();
            try
            {
                reasonDto = _emamiContext.Reasons.AsNoTracking().OrderByDescending(_ => _.Reason).Select(c => new ReasonDto
                {
                    Id = c.Id,
                    Reason = c.Reason,
                    Description = c.Description,
                    IsActive = c.IsActive

                }).ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = reasonDto;
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

        public ResultDto AddReason(ReasonAddDto reasonAddDto)
        {
            _methodName = "AddReason";
            var resultDto = new ResultDto();
            try
            {
                if (reasonAddDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }
                if (string.IsNullOrEmpty(reasonAddDto.Reason))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.NameIsEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.NameIsEmpty, Utility.MessageLanguage);
                    return resultDto;
                }
                var reasonContext = _emamiContext.Reasons.AsNoTracking().Count(_ => _.Reason == reasonAddDto.Reason);
                if (reasonContext > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.NameExist;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.NameExist, Utility.MessageLanguage);
                    return resultDto;
                }
                var reasonsContext = new Reasons
                {
                    Reason = reasonAddDto.Reason.Trim(),
                    Description = reasonAddDto.Description.Trim(),
                    IsActive = reasonAddDto.IsActive,
                    CreatedBy = reasonAddDto.CreatedBy,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                };
                _emamiContext.Reasons.Add(reasonsContext);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.GetMessage(Constants.RecordSaved, Utility.MessageLanguage);
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

        public ResultDto UpdateReason(ReasonUpdateDto reasonupdateDto)
        {
            _methodName = "UpdateReason";
            var resultDto = new ResultDto();
            try
            {
                if (reasonupdateDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Utility.MessageLanguage);
                    return resultDto;
                }
                if (reasonupdateDto.Id == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.IdEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.IdEmpty, Utility.MessageLanguage);
                    return resultDto;
                }
                if (string.IsNullOrEmpty(reasonupdateDto.Reason))
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.NameIsEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.NameIsEmpty, Utility.MessageLanguage);
                    return resultDto;
                }
                var reasonContextExist = _emamiContext.Reasons.AsNoTracking().Count(_ => _.Reason == reasonupdateDto.Reason && _.Id != reasonupdateDto.Id);
                if (reasonContextExist > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.NameExist;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.NameExist, Utility.MessageLanguage);
                    return resultDto;
                }
                var reasonContext = _emamiContext.Reasons.FirstOrDefault(_ => _.Id == reasonupdateDto.Id);
                if (reasonContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Utility.MessageLanguage);
                    return resultDto;
                }
                reasonContext.Reason = reasonupdateDto.Reason.Trim();
                reasonContext.Description = reasonupdateDto.Description.Trim();
                reasonContext.IsActive = reasonupdateDto.IsActive;
                reasonContext.ModifiedBy = reasonupdateDto.ModifiedBy;
                reasonContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                _emamiContext.SaveChanges();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = Constants.GetMessage(Constants.RecordSaved, Utility.MessageLanguage);
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

        public ResultDto ViewReasons(ReasonIdDto reasonIdDto)
        {
            _methodName = "ViewReasons";
            var resultDto = new ResultDto();
            var reasonDto = new ReasonDto();
            try
            {
                var reasonContext = _emamiContext.Reasons.AsNoTracking().FirstOrDefault(_ => _.Id == reasonIdDto.ReasonId);
                if (reasonContext != null)
                {
                    reasonDto.Id = reasonContext.Id;
                    reasonDto.Reason = reasonContext.Reason;
                    reasonDto.Description = reasonContext.Description;
                    reasonDto.IsActive = reasonContext.IsActive;
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = reasonDto;
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

        public ResultDto GetActiveReasons()
        {
            _methodName = "GetActiveReasons";
            var resultDto = new ResultDto();
            var reasonListDto = new List<ReasonDto>();
            try
            {
                reasonListDto = _emamiContext.Reasons.Where(_ => _.IsActive).AsNoTracking().OrderByDescending(_ => _.Reason).Select(c => new ReasonDto
                {
                    Id = c.Id,
                    Reason = c.Reason,
                    Description = c.Description,
                    IsActive = c.IsActive

                }).ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = reasonListDto;
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

        public ResultDto GetDealers()
        {
            _methodName = "GetDealers";
            var resultDto = new ResultDto();
            var reasonListDto = new MTPDealerDto();
            try
            {
                var UserRolesContext = _emamiContext.UserRoles.Where(_ => _.RoleId == (int)DTO.Enums.Role.Dealer).AsNoTracking().Select(_ => _.UserId).ToList();
                foreach (var Dealer in UserRolesContext)
                {
                    var U = _emamiContext.Users.Where(_ => _.Id == Dealer).AsNoTracking().Select(c => new MTPDealerDetailDto
                    {
                        Id = c.Id,
                        Dealer = c.Name
                    }).ToList();
                    reasonListDto.MTPDealerDetail.AddRange(U);
                }
                
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = reasonListDto;
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

        public ResultDto GetCities()
        {
            _methodName = "GetCities";
            var cityList = new List<CityDto>();
            var resultDto = new ResultDto();
            try
            {
                var cityContext = new List<City>();
                var districtContextList = _emamiContext.City.AsNoTracking().Where(_ => _.IsActive).OrderBy(_ => _.SortOrder).AsQueryable();
                cityContext = districtContextList.OrderBy(_ => _.SortOrder).ToList();
                if (!cityContext.Any())
                {
                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = cityList;
                    return resultDto;
                }
                foreach (var district in cityContext)
                {
                    var districtDto = new CityDto
                    {
                        CityId = district.Id,
                        CityName = district.CityName,
                    };
                    cityList.Add(districtDto);
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = cityList;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Constants.EnglishLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        #endregion

        #region Permanent Journey Plan

        public ResultDto AddPermanentJourneyPlan(PermanentJouneyPlanAddDto permanentJouneyPlanAddDto)
        {
            _methodName = "AddPermanentJourneyPlan";
            var resultDto = new ResultDto();
            try
            {
                if (permanentJouneyPlanAddDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (permanentJouneyPlanAddDto.CreatedBy == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (!permanentJouneyPlanAddDto.PermanentJourneyPlanDetails.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.PJPDetailsEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.PJPDetailsEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }
                var userContext = _emamiContext.UserReportingToMappings.AsNoTracking().FirstOrDefault(_ => _.UserId == permanentJouneyPlanAddDto.CreatedBy);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }
                if (userContext.ReportingToUserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.PJPApprovalFlowEmpty;
                    resultDto.ErrorDto.Message = Constants.PJPApprovalFlowEmpty;
                    return resultDto;
                }

                DateTime FromDate = permanentJouneyPlanAddDto.PermanentJourneyPlanDetails[0].EffectiveFrom;
                DateTime ToDate = permanentJouneyPlanAddDto.PermanentJourneyPlanDetails[0].EffectiveTo;
                var pjpContextExist = _emamiContext.PermanentJourneyPlans.AsNoTracking().FirstOrDefault(_ => _.CreatedBy == permanentJouneyPlanAddDto.CreatedBy
                                                                           &&
                                                                           (_.PermanentJourneyPlanStatusId == (int)DTO.Enums.Status.Pending || _.PermanentJourneyPlanStatusId == (int)DTO.Enums.Status.Approved)
                                                                           &&
                                                                           //((DbFunctions.TruncateTime(_.EffectiveFrom) >= DbFunctions.TruncateTime(FromDate) &&
                                                                           //DbFunctions.TruncateTime(_.EffectiveFrom) <= DbFunctions.TruncateTime(ToDate))
                                                                           //||
                                                                           //(DbFunctions.TruncateTime(_.EffectiveTo) >= DbFunctions.TruncateTime(FromDate) &&
                                                                           //DbFunctions.TruncateTime(_.EffectiveTo) <= DbFunctions.TruncateTime(ToDate)))
                                                                           //);
                                                                           ((DbFunctions.TruncateTime(_.EffectiveFrom) <= DbFunctions.TruncateTime(FromDate) &&
                                                                           DbFunctions.TruncateTime(_.EffectiveTo) >= DbFunctions.TruncateTime(FromDate))
                                                                           ||
                                                                           (DbFunctions.TruncateTime(_.EffectiveFrom) <= DbFunctions.TruncateTime(ToDate) &&
                                                                           DbFunctions.TruncateTime(_.EffectiveTo) >= DbFunctions.TruncateTime(ToDate)))
                                                                           );

                if (pjpContextExist != null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.PJPDataAlreadyExiststhisDate;
                    resultDto.ErrorDto.Message = Constants.PJPDataAlreadyExiststhisDate;
                    return resultDto;
                }

                var MTPExistsContext = (from mtp in _emamiContext.MonthlyTourPlans.AsNoTracking()
                                        join mtpd in _emamiContext.MonthlyTourPlanDetails.AsNoTracking() on mtp.Id equals mtpd.MonthlyTourPlanId
                                        where mtp.CreatedBy == permanentJouneyPlanAddDto.CreatedBy
                                        &&
                                        (mtp.MonthlyTourPlanStatusId == (int)DTO.Enums.Status.Pending || mtp.MonthlyTourPlanStatusId == (int)DTO.Enums.Status.Approved)
                                        &&
                                        (DbFunctions.TruncateTime(mtpd.Date) >= DbFunctions.TruncateTime(FromDate) &&
                                         DbFunctions.TruncateTime(mtpd.Date) <= DbFunctions.TruncateTime(ToDate))
                                        select mtp
                                         ).ToList();
                if (MTPExistsContext != null && MTPExistsContext.Count > 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.MTPalreadyApproved;
                    resultDto.ErrorDto.Message = Constants.MTPalreadyApproved;
                    return resultDto;
                }

                var pjpContext = new PermanentJourneyPlans
                {
                    PermanentJourneyPlanStatusId = permanentJouneyPlanAddDto.StatusId,
                    FinancialYearId = permanentJouneyPlanAddDto.PermanentJourneyPlanDetails[0].FinancialYearId,
                    CreatedBy = permanentJouneyPlanAddDto.CreatedBy,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    EffectiveFrom = permanentJouneyPlanAddDto.PermanentJourneyPlanDetails[0].EffectiveFrom,
                    EffectiveTo = permanentJouneyPlanAddDto.PermanentJourneyPlanDetails[0].EffectiveTo,
                };
                _emamiContext.PermanentJourneyPlans.Add(pjpContext);
                _emamiContext.SaveChanges();
                pjpContext.PJPNumber = Utility.PermanentJourneyPlanNumberPrefix + pjpContext.Id;

                foreach (var details in permanentJouneyPlanAddDto.PermanentJourneyPlanDetails)
                {
                    var detailContext = new PermanentJourneyPlanDetails
                    {
                        PermanentJourneyPlanId = pjpContext.Id,
                        CreatedBy = permanentJouneyPlanAddDto.CreatedBy,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        RetailerId = details.RetailerId,
                        MonthId = details.MonthId,
                        StateId = details.StateId,
                        DistrictId = details.DistrictId,
                        TownId = details.CityId,
                        NoOfDirectDealer = details.NoOfDirectDealer,
                        NoOfWholeSeller = details.NoOfWholeSeller,
                        NoofSubDealer = details.NoOfSubDealer,
                        NoOfVisit = Convert.ToDecimal(details.NoOfVisit),
                        TerritoryId = details.TerritoryId,
                        InHQNoVisit = details.InHQNoVisitId,
                        Remarks = details.Remarks,
                    };
                    _emamiContext.PermanentJourneyPlanDetails.Add(detailContext);
                }

                var approvalContext = new PermanentJourneyPlanApprovalInformation
                {
                    PermanentJourneyPlanId = pjpContext.Id,
                    CreatedBy = permanentJouneyPlanAddDto.CreatedBy,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    UserId = userContext.ReportingToUserId,
                    StatusId = permanentJouneyPlanAddDto.StatusId
                };
                _emamiContext.PJPApprovalInformation.Add(approvalContext);
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = 0;
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

        public ResultDto UpdatePermanentJourneyPlan(PermanentJourneyPlanUpdateDto permanentJouneyPlanUpdateDto)
        {
            _methodName = "permanentJouneyPlanUpdateDto";
            var resultDto = new ResultDto();
            try
            {
                if (permanentJouneyPlanUpdateDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (permanentJouneyPlanUpdateDto.ModifiedBy == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (!permanentJouneyPlanUpdateDto.PermanentJourneyPlanDetails.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.PJPDetailsEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.PJPDetailsEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }
                var userContext = _emamiContext.UserReportingToMappings.AsNoTracking().FirstOrDefault(_ => _.UserId == permanentJouneyPlanUpdateDto.ModifiedBy);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (userContext.ReportingToUserId == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.PJPApprovalFlowEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.PJPApprovalFlowEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }

                var pjpContext = _emamiContext.PermanentJourneyPlans.FirstOrDefault(_ => _.Id == permanentJouneyPlanUpdateDto.PJPId);
                if (pjpContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                DateTime FromDate = permanentJouneyPlanUpdateDto.PermanentJourneyPlanDetails[0].EffectiveFrom;
                DateTime ToDate = permanentJouneyPlanUpdateDto.PermanentJourneyPlanDetails[0].EffectiveTo;
                var pjpContextExist = _emamiContext.PermanentJourneyPlans.AsNoTracking().FirstOrDefault(_ => _.CreatedBy == permanentJouneyPlanUpdateDto.ModifiedBy && _.Id != permanentJouneyPlanUpdateDto.PJPId
                                                                           &&
                                                                           (_.PermanentJourneyPlanStatusId == (int)DTO.Enums.Status.Pending || _.PermanentJourneyPlanStatusId == (int)DTO.Enums.Status.Approved)
                                                                           &&
                                                                           //((DbFunctions.TruncateTime(_.EffectiveFrom) >= DbFunctions.TruncateTime(FromDate) &&
                                                                           //DbFunctions.TruncateTime(_.EffectiveFrom) <= DbFunctions.TruncateTime(ToDate))
                                                                           //||
                                                                           //(DbFunctions.TruncateTime(_.EffectiveTo) >= DbFunctions.TruncateTime(FromDate) &&
                                                                           //DbFunctions.TruncateTime(_.EffectiveTo) <= DbFunctions.TruncateTime(ToDate)))
                                                                           ((DbFunctions.TruncateTime(_.EffectiveFrom) <= DbFunctions.TruncateTime(FromDate) &&
                                                                           DbFunctions.TruncateTime(_.EffectiveTo) >= DbFunctions.TruncateTime(FromDate))
                                                                           ||
                                                                           (DbFunctions.TruncateTime(_.EffectiveFrom) <= DbFunctions.TruncateTime(ToDate) &&
                                                                           DbFunctions.TruncateTime(_.EffectiveTo) >= DbFunctions.TruncateTime(ToDate)))
                                                                           );

                if (pjpContextExist != null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.PJPDataAlreadyExiststhisDate;
                    resultDto.ErrorDto.Message = Constants.PJPDataAlreadyExiststhisDate;
                    return resultDto;
                }

                if (pjpContext.PermanentJourneyPlanStatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Pending || pjpContext.PermanentJourneyPlanStatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Drafted)
                {
                    if (permanentJouneyPlanUpdateDto.IsEditedByAdmin == 0)
                    {
                        pjpContext.PermanentJourneyPlanStatusId = permanentJouneyPlanUpdateDto.StatusId;
                    }
                    pjpContext.FinancialYearId = permanentJouneyPlanUpdateDto.PermanentJourneyPlanDetails[0].FinancialYearId;
                    pjpContext.EffectiveFrom = permanentJouneyPlanUpdateDto.PermanentJourneyPlanDetails[0].EffectiveFrom;
                    pjpContext.EffectiveTo = permanentJouneyPlanUpdateDto.PermanentJourneyPlanDetails[0].EffectiveTo;
                    pjpContext.ModifiedBy = permanentJouneyPlanUpdateDto.ModifiedBy;
                    pjpContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    _emamiContext.SaveChanges();
                }

                var existingRecords = permanentJouneyPlanUpdateDto.PermanentJourneyPlanDetails.Where(_ => _.Id != 0).ToList();

                if (existingRecords != null && existingRecords.Any())
                {
                    var allRecords = _emamiContext.PermanentJourneyPlanDetails.Where(_ => _.PermanentJourneyPlanId == permanentJouneyPlanUpdateDto.PJPId).ToList();
                    var removeRecords = allRecords.Where(_ => !existingRecords.Select(s => s.Id).Contains(_.Id));
                    foreach (var removeRecord in removeRecords)
                    {
                        _emamiContext.PermanentJourneyPlanDetails.Remove(removeRecord);
                    }
                    _emamiContext.SaveChanges();
                }

                foreach (var details in existingRecords)
                {
                    var pjpexistrecordsContext = _emamiContext.PermanentJourneyPlanDetails.FirstOrDefault(_ => _.Id == details.Id);

                    #region STP History

                    if (!(((details.RetailerId == "0" && (pjpexistrecordsContext.RetailerId == null || pjpexistrecordsContext.RetailerId == "0")) || (details.RetailerId == pjpexistrecordsContext.RetailerId))
                        && details.FinancialYearId == permanentJouneyPlanUpdateDto.PermanentJourneyPlanDetails[0].FinancialYearId
                        && details.StateId == Convert.ToInt32(pjpexistrecordsContext.StateId)
                        && details.TerritoryId == Convert.ToInt32(pjpexistrecordsContext.TerritoryId)
                        && details.DistrictId == Convert.ToInt32(pjpexistrecordsContext.DistrictId)
                        && details.CityId == Convert.ToInt32(pjpexistrecordsContext.TownId)
                        && details.NoOfDirectDealer == pjpexistrecordsContext.NoOfDirectDealer
                        && details.NoOfSubDealer == pjpexistrecordsContext.NoofSubDealer
                        && details.NoOfWholeSeller == pjpexistrecordsContext.NoOfWholeSeller
                        && details.NoOfVisit == pjpexistrecordsContext.NoOfVisit.ToString()
                        && details.Id == pjpexistrecordsContext.Id
                        && details.EffectiveFrom.Date == permanentJouneyPlanUpdateDto.PermanentJourneyPlanDetails[0].EffectiveFrom.Date
                        && details.EffectiveTo.Date == permanentJouneyPlanUpdateDto.PermanentJourneyPlanDetails[0].EffectiveTo.Date
                        && details.InHQNoVisitId == pjpexistrecordsContext.InHQNoVisit
                        && details.Remarks == pjpexistrecordsContext.Remarks
                        ))
                    {
                        //Insert or Update History table
                        AddSalesTourPlanPcpHistory(pjpexistrecordsContext,
                    permanentJouneyPlanUpdateDto.PermanentJourneyPlanDetails[0].EffectiveFrom,
                    permanentJouneyPlanUpdateDto.PermanentJourneyPlanDetails[0].EffectiveTo,
                    permanentJouneyPlanUpdateDto.PermanentJourneyPlanDetails[0].FinancialYearId,
                    permanentJouneyPlanUpdateDto.ModifiedBy);

                        //Insert or Update Detail table
                        pjpexistrecordsContext.PermanentJourneyPlanId = pjpContext.Id;
                        pjpexistrecordsContext.ModifiedBy = permanentJouneyPlanUpdateDto.ModifiedBy;
                        pjpexistrecordsContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        pjpexistrecordsContext.RetailerId = details.RetailerId;
                        pjpexistrecordsContext.MonthId = details.MonthId;
                        pjpexistrecordsContext.StateId = details.StateId;
                        pjpexistrecordsContext.TerritoryId = details.TerritoryId;
                        pjpexistrecordsContext.DistrictId = details.DistrictId;
                        pjpexistrecordsContext.TownId = details.CityId;
                        pjpexistrecordsContext.NoOfDirectDealer = details.NoOfDirectDealer;
                        pjpexistrecordsContext.NoOfWholeSeller = details.NoOfWholeSeller;
                        pjpexistrecordsContext.NoofSubDealer = details.NoOfSubDealer;
                        pjpexistrecordsContext.NoOfVisit = Convert.ToDecimal(details.NoOfVisit);
                        pjpexistrecordsContext.InHQNoVisit = details.InHQNoVisitId;
                        pjpexistrecordsContext.Remarks = details.Remarks;
                    }

                    #endregion


                    _emamiContext.SaveChanges();
                }

                var newRecords = permanentJouneyPlanUpdateDto.PermanentJourneyPlanDetails.Where(_ => _.Id == 0).ToList();
                foreach (var details in newRecords)
                {
                    var detailContext = new PermanentJourneyPlanDetails
                    {
                        PermanentJourneyPlanId = pjpContext.Id,
                        CreatedBy = permanentJouneyPlanUpdateDto.ModifiedBy,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        RetailerId = details.RetailerId,
                        MonthId = details.MonthId,
                        StateId = details.StateId,
                        TerritoryId = details.TerritoryId,
                        DistrictId = details.DistrictId,
                        TownId = details.CityId,
                        NoOfDirectDealer = details.NoOfDirectDealer,
                        NoOfWholeSeller = details.NoOfWholeSeller,
                        NoofSubDealer = details.NoOfSubDealer,
                        NoOfVisit = UtilityHelper.IntTryToParse(details.NoOfVisit),
                        InHQNoVisit = details.InHQNoVisitId,
                        Remarks = details.Remarks,
                    };
                    _emamiContext.PermanentJourneyPlanDetails.Add(detailContext);
                }

                if (permanentJouneyPlanUpdateDto.IsEditedByAdmin == 0)
                {
                    if (permanentJouneyPlanUpdateDto.StatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Pending)
                    {
                        //New
                        var isApproverContext = _emamiContext.PJPApprovalInformation.Any(_ => _.PermanentJourneyPlanId == pjpContext.Id && _.StatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Pending);
                        if (!isApproverContext)
                        {
                            _emamiContext.PJPApprovalInformation.Add(new PermanentJourneyPlanApprovalInformation
                            {
                                PermanentJourneyPlanId = pjpContext.Id,
                                StatusId = (int)DTO.Enums.PermanentJourneyPlanStatus.Pending,
                                CreatedBy = permanentJouneyPlanUpdateDto.ModifiedBy,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                UserId = userContext.ReportingToUserId
                            });
                        }

                        //Old
                        //var approvalContext = new PermanentJourneyPlanApprovalInformation
                        //{
                        //    PermanentJourneyPlanId = pjpContext.Id,
                        //    CreatedBy = permanentJouneyPlanUpdateDto.ModifiedBy,
                        //    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        //    UserId = userContext.SalesReportingToId ?? 0,
                        //    StatusId = (int)DTO.Enums.PermanentJourneyPlanStatus.Pending
                        //};
                        //_emamiContext.PJPApprovalInformation.Add(approvalContext);
                    }
                    else
                    {
                        var approverContext = _emamiContext.PJPApprovalInformation.FirstOrDefault(_ => _.PermanentJourneyPlanId == pjpContext.Id && _.StatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Pending);
                        if (approverContext != null)
                        {
                            approverContext.StatusId = permanentJouneyPlanUpdateDto.StatusId;
                            approverContext.ModifiedBy = permanentJouneyPlanUpdateDto.ModifiedBy;
                            approverContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                            approverContext.Remarks = permanentJouneyPlanUpdateDto.Remarks;
                            approverContext.ReasonId = permanentJouneyPlanUpdateDto.ReasonIds;
                        }
                    }
                }

                _emamiContext.SaveChanges();

                try
                {
                    if (permanentJouneyPlanUpdateDto.StatusId != (int)DTO.Enums.PermanentJourneyPlanStatus.Pending)
                    {
                        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                        var CreatedByUser = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == pjpContext.CreatedBy);
                        var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.PCPApproval);
                        string PCPStatus = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == permanentJouneyPlanUpdateDto.StatusId).Name;
                        List<string> toUser = new List<string>();
                        toUser.Add(CreatedByUser.Email);
                        var emailSubject = Constants.PCPApprovalSubject;
                        var fromEmail = Constants.FromEmail;
                        var plainText = string.Empty;
                        if (emailTemplate != null)
                        {
                            var replaceEmailTemplate = emailTemplate.PlainTemplate.Replace(Constants.Name, CreatedByUser.Name).Replace(Constants.ApproveOrReject, PCPStatus);
                            var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, replaceEmailTemplate);
                            amazonNotificationService.SendEmail(toUser, emailSubject, plainText, htmlTemplate, true);
                        }
                        var smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.PCPApprovalSMS);
                        if (smsTemplate != null)
                        {
                            var replaceSmsTemplate = smsTemplate.PlainTemplate.Replace(Constants.Name, CreatedByUser.Name).Replace(Constants.ApproveOrReject, PCPStatus);
                            amazonNotificationService.SendMessage(replaceSmsTemplate, CreatedByUser.MobileNumber);

                            if (CreatedByUser != null && CreatedByUser.RegistrationTypeId != null && CreatedByUser.RegistrationTypeId > 0 && !string.IsNullOrEmpty(CreatedByUser.PushTokenKey))
                            {
                                PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                {
                                    PushTokenKey = CreatedByUser.PushTokenKey,
                                    RegistrationTypeId = CreatedByUser.RegistrationTypeId != null ? (int)CreatedByUser.RegistrationTypeId : 0,
                                    Title = emailSubject,
                                    Message = replaceSmsTemplate,
                                    //Id = pjpContext.Id,
                                };
                                _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = pjpContext.Id;
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

        public ResultDto GetPermanentJourneyPlanDetails(PJPIdDto pJPIdDto)
        {
            _methodName = "GetPermanentJourneyPlanDetails";
            var resultDto = new ResultDto();
            var permanentJourneyPlanDto = new PermanentJourneyPlanDto();
            try
            {
                if (pJPIdDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (pJPIdDto.PJPId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                var pjpContext = _emamiContext.PermanentJourneyPlans.FirstOrDefault(_ => _.Id == pJPIdDto.PJPId);
                var approverContext = _emamiContext.PJPApprovalInformation.Where(_ => _.PermanentJourneyPlanId == pJPIdDto.PJPId && _.StatusId != (int)DTO.Enums.PermanentJourneyPlanStatus.Pending).OrderByDescending(_ => _.Id).FirstOrDefault();
                if (pjpContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }
                permanentJourneyPlanDto.PJPId = pjpContext.Id;
                permanentJourneyPlanDto.StatusId = pjpContext.PermanentJourneyPlanStatusId;
                permanentJourneyPlanDto.Status = pjpContext.PJPStatusName.Status;
                permanentJourneyPlanDto.FinancialYearId = pjpContext.FinancialYearId;
                permanentJourneyPlanDto.FinancialYear = pjpContext.Year.Year.ToString();
                permanentJourneyPlanDto.CreatedBy = pjpContext.CreatedBy;
                permanentJourneyPlanDto.EffectiveFrom = pjpContext.EffectiveFrom;
                permanentJourneyPlanDto.EffectiveTo = pjpContext.EffectiveTo;
                if (approverContext != null)
                {
                    permanentJourneyPlanDto.Remarks = approverContext.Remarks;
                    permanentJourneyPlanDto.ReasonIds = approverContext.ReasonId;
                }
                var pjpDetailsList = new List<PermanentJourneyPlanDetailsDto>();
                if (pjpContext.PJPDetails.Any())
                {
                    foreach (var pjpdetails in pjpContext.PJPDetails)
                    {

                        var pjpDetails = new PermanentJourneyPlanDetailsDto
                        {
                            FinancialYearId = pjpContext.FinancialYearId,
                            PJPId = pjpContext.Id,
                            FinancialYear = _emamiContext.FinancialYears.FirstOrDefault(_ => _.Id == pjpContext.FinancialYearId)?.Year,
                            Id = pjpdetails.Id,
                            //MonthId = pjpdetails.MonthId,
                            //Month = _emamiContext.Months.FirstOrDefault(_ => _.Id == pjpdetails.MonthId).Name.ToString(),
                            StateId = pjpdetails.StateId,
                            State = _emamiContext.State.FirstOrDefault(_ => _.Id == pjpdetails.StateId)?.StateName,
                            DistrictId = pjpdetails.DistrictId,
                            District = _emamiContext.District.FirstOrDefault(_ => _.Id == pjpdetails.DistrictId)?.DistrictName,
                            CityId = pjpdetails.TownId,
                            City = _emamiContext.City.FirstOrDefault(_ => _.Id == pjpdetails.TownId)?.CityName,
                            RetailerId = pjpdetails.RetailerId,
                            NoOfDirectDealer = pjpdetails.NoOfDirectDealer,
                            NoOfSubDealer = pjpdetails.NoofSubDealer,
                            NoOfWholeSeller = pjpdetails.NoOfWholeSeller,
                            NoOfVisit = pjpdetails.NoOfVisit.ToString(),
                            TerritoryId = pjpdetails.TerritoryId,
                            Territory = _emamiContext.Territory.FirstOrDefault(_ => _.Id == pjpdetails.TerritoryId)?.Name,
                            EffectiveFrom = pjpContext.EffectiveFrom,
                            EffectiveTo = pjpContext.EffectiveTo,
                            InHQNoVisitId = pjpdetails.InHQNoVisit,
                            InHQNoVisitName = pjpdetails.InHQNoVisit != 0 ? Utility.GetEnumFromString<DTO.Enums.STPVisitType>(pjpdetails.InHQNoVisit) : string.Empty,
                            Remarks = pjpdetails.Remarks,
                        };

                        if (!string.IsNullOrEmpty(pjpDetails.RetailerId) && pjpDetails.RetailerId != "0")
                        {
                            var retailerIdsList = pjpdetails.RetailerId.Split(',');
                            var retailerNames = string.Empty;
                            foreach (var retailer in retailerIdsList)
                            {
                                var retailerId = long.Parse(retailer);
                                var retailerDetail = _emamiContext.Users.FirstOrDefault(_ => _.Id == retailerId);
                                retailerNames = retailerDetail == null ? string.Empty : retailerDetail.Name + "," + retailerNames;
                            }
                            pjpDetails.Retailers = retailerNames.Remove(retailerNames.Length - 1, 1);
                        }
                        else
                        {
                            pjpDetails.Retailers = string.Empty;
                        }

                        var isDataChange = _emamiContext.SalesTourPlanPcpHistory.AsNoTracking().FirstOrDefault(f => f.PermanentJourneyPlanDetailId == pjpdetails.Id)?.IsDataChanged;
                        pjpDetails.IsDataChanged = (isDataChange == null ? false : isDataChange);

                        pjpDetailsList.Add(pjpDetails);
                    }
                    permanentJourneyPlanDto.PermanentJourneyPlanDetails = pjpDetailsList;
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = permanentJourneyPlanDto;
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

        public ResultDto GetPermanentJourneyPlanList(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetPermanentJourneyPlanList";
            var resultDto = new ResultDto();
            var permanentJourneyPlansDto = new List<PermanentJourneyPlansDto>();
            try
            {
                var createdpjp = _emamiContext.PermanentJourneyPlans.Where(_ => _.CreatedBy == loginUserIdDto.LoginUserId).ToList();
                foreach (var c in createdpjp)
                {
                    var detailContext = new PermanentJourneyPlansDto
                    {
                        PJPId = c.Id,
                        EncryptedId = UtilityHelper.ConvertToMd5(c.Id.ToString(), SecurityConstants.EncryptionKey),
                        PJPNumber = c.PJPNumber,
                        FinancialYearId = c.FinancialYearId,
                        FinancialYear = _emamiContext.FinancialYears.FirstOrDefault(_ => _.Id == c.FinancialYearId)?.Year,
                        Remarks = c.Remarks,
                        CreatedBy = c.CreatedBy,
                        CreatedUser = _emamiContext.Users.FirstOrDefault(_ => _.Id == c.CreatedBy)?.Name,
                        StatusId = c.PermanentJourneyPlanStatusId,
                        Status = _emamiContext.PJPStatus.FirstOrDefault(_ => _.Id == c.PermanentJourneyPlanStatusId)?.Status,
                        CreatedDate = c.CreatedDate
                    };
                    permanentJourneyPlansDto.Add(detailContext);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = permanentJourneyPlansDto.ToList();
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

        public ResultDto GetPendingPermanentJourneyPlanList(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetPendingPermanentJourneyPlanList";
            var resultDto = new ResultDto();
            var permanentJourneyPlansDto = new List<PermanentJourneyPlansDto>();
            try
            {
                var toapprovepjp = (from ppj in _emamiContext.PermanentJourneyPlans
                                    join ppjai in _emamiContext.PJPApprovalInformation on ppj.Id equals ppjai.PermanentJourneyPlanId
                                    join ur in _emamiContext.UserReportingToMappings.AsNoTracking() on ppjai.CreatedBy equals ur.UserId
                                    where ur.ReportingToUserId == loginUserIdDto.LoginUserId && ppjai.StatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Pending
                                    && ppj.PermanentJourneyPlanStatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Pending
                                    select ppj
                                   ).Distinct().ToList();

                foreach (var c in toapprovepjp)
                {
                    var detailContext = new PermanentJourneyPlansDto
                    {
                        PJPId = c.Id,
                        EncryptedId = UtilityHelper.ConvertToMd5(c.Id.ToString(), SecurityConstants.EncryptionKey),
                        PJPNumber = c.PJPNumber,
                        FinancialYearId = c.FinancialYearId,
                        FinancialYear = _emamiContext.FinancialYears.FirstOrDefault(_ => _.Id == c.FinancialYearId)?.Year,
                        Remarks = c.Remarks,
                        CreatedBy = c.CreatedBy,
                        CreatedUser = _emamiContext.Users.FirstOrDefault(_ => _.Id == c.CreatedBy).Name,
                        StatusId = c.PermanentJourneyPlanStatusId,
                        Status = _emamiContext.PJPStatus.FirstOrDefault(_ => _.Id == c.PermanentJourneyPlanStatusId)?.Status,
                        CreatedDate = c.CreatedDate,
                    };
                    permanentJourneyPlansDto.Add(detailContext);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = permanentJourneyPlansDto.ToList();
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
        public ResultDto ApprovedPermanentJourneyPlanByUser(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "ApprovedPermanentJourneyPlanByUser";
            var resultDto = new ResultDto();
            var permanentJourneyPlansDto = new List<PermanentJourneyPlansDto>();
            DateTime currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            try
            {
                var approvedpjp = (from ppj in _emamiContext.PermanentJourneyPlans
                                   join ppjai in _emamiContext.PJPApprovalInformation on ppj.Id equals ppjai.PermanentJourneyPlanId
                                   where ppj.CreatedBy == loginUserIdDto.LoginUserId && ppj.PermanentJourneyPlanStatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Approved
                                   && DbFunctions.TruncateTime(ppj.EffectiveTo) >= DbFunctions.TruncateTime(currentDate)
                                   select ppj
                                   ).Distinct().ToList();

                if (approvedpjp != null)
                {
                    foreach (var c in approvedpjp)
                    {
                        var detailContext = new PermanentJourneyPlansDto
                        {
                            PJPId = c.Id,
                            PJPNumber = c.PJPNumber,
                        };
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
        public ResultDto MonthsByUserPermanentJourneyPlan(PJPIdDto pJPIdDto)
        {
            _methodName = "MonthsByUserPermanentJourneyPlan";
            var resultDto = new ResultDto();
            var pjpMonthList = new List<Month>();
            var financialYearDto = new FinancialYearDto();
            try
            {
                //var pjpmonths = (from ppj in _emamiContext.PermanentJourneyPlans
                //                   join ppjd in _emamiContext.PermanentJourneyPlanDetails on ppj.Id equals ppjd.PermanentJourneyPlanId
                //                   join month in _emamiContext.Months on ppjd.MonthId equals month.Id
                //                   where ppj.Id == pJPIdDto.PJPId 
                //                   select month
                //                   ).Distinct().ToList();

                var pjpContext = _emamiContext.PermanentJourneyPlans.FirstOrDefault(_ => _.Id == pJPIdDto.PJPId);

                //var financialYearContext = _emamiContext.FinancialYears.AsNoTracking().FirstOrDefault(_ => _.Id == pjpContext.FinancialYearId);
                if (pjpContext != null)
                {
                    financialYearDto.EffectiveFrom = pjpContext.EffectiveFrom;
                    financialYearDto.EffectiveTo = pjpContext.EffectiveTo;

                    int totalMonths = 12 * (financialYearDto.EffectiveTo.Year - financialYearDto.EffectiveFrom.Year) + financialYearDto.EffectiveTo.Month - financialYearDto.EffectiveFrom.Month;
                    List<Month> months = new List<Month>();
                    int startMonth = pjpContext.EffectiveFrom.Month;
                    int endMonth = pjpContext.EffectiveTo.Month;
                    int month = startMonth - 1;
                    for (var i = 0; i <= totalMonths; i++)
                    {
                        Month toaddmonth = new Month();
                        if (month == 12)
                        {
                            month = 0;
                        }
                        month = month + 1;
                        toaddmonth.Id = month;
                        months.Add(toaddmonth);
                    }
                    var monthlist = _emamiContext.Months.AsNoTracking().ToList();
                    var pjpmonths = (from m in monthlist
                                     join t in months on m.Id equals t.Id
                                     select m).ToList();
                    pjpMonthList = pjpmonths;
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = pjpMonthList;
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

        public ResultDto DealersByUserPermanentJourneyPlan(PJPIdDto pJPIdDto)
        {
            _methodName = "DealersByUserPermanentJourneyPlan";
            var resultDto = new ResultDto();
            var dealerDto = new List<DealerDto>();
            try
            {
                var dealers = new List<DealerDto>();
                var pjpContext = _emamiContext.PermanentJourneyPlans.FirstOrDefault(_ => _.Id == pJPIdDto.PJPId);
                if (pjpContext != null)
                {
                    if (pjpContext.PJPDetails.Any())
                    {
                        var pjpdetailscontext = pjpContext.PJPDetails.Where(_ => pJPIdDto.CityId > 0 ? _.TownId == pJPIdDto.CityId : pJPIdDto.CityId==0).ToList();
                        foreach (var pjpdetails in pjpdetailscontext)
                        {
                            if (!string.IsNullOrEmpty(pjpdetails.RetailerId) && pjpdetails.RetailerId != "0")
                            {
                                var retailerIdsList = pjpdetails.RetailerId.Split(',');
                                foreach (var retailer in retailerIdsList)
                                {
                                    DealerDto dealer = new DealerDto();
                                    var retailerId = long.Parse(retailer);
                                    var retailerDetail = _emamiContext.Users.FirstOrDefault(_ => _.Id == retailerId);
                                    dealer.Id = retailerDetail.Id;
                                    dealers.Add(dealer);
                                }

                                var userlist = _emamiContext.Users.ToList();
                                var dealerlist = (from user in userlist
                                                  join dealer in dealers on user.Id equals dealer.Id
                                                  select user
                                                   ).Distinct().ToList();

                                dealerDto = dealerlist.Select(c => new DealerDto
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                }).ToList();
                            }
                        }
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = dealerDto;
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

        public ResultDto MonthlyTourPlanDateCalendar(PermanentJourneyPlanDetailsDto permanentJourneyPlanDetailsDto)
        {
            _methodName = "MonthlyTourPlanDateCalendar";
            var resultDto = new ResultDto();
            var financialYearDto = new FinancialYearDto();
            try
            {
                var pjpContext = _emamiContext.PermanentJourneyPlans.FirstOrDefault(_ => _.Id == permanentJourneyPlanDetailsDto.PJPId);
                if (pjpContext != null)
                {
                    if (pjpContext.PJPDetails.Any())
                    {
                        var financialyearcontext = _emamiContext.FinancialYears.FirstOrDefault(_ => _.Id == pjpContext.FinancialYearId);

                        if (pjpContext.EffectiveFrom.Month == permanentJourneyPlanDetailsDto.MonthId)
                        {
                            financialYearDto.EffectiveFrom = pjpContext.EffectiveFrom;
                            if (pjpContext.EffectiveFrom < pjpContext.EffectiveFrom.AddMonths(1).AddDays(-1))
                            {
                                financialYearDto.EffectiveTo = pjpContext.EffectiveTo;
                            }
                            else
                            {
                                financialYearDto.EffectiveTo = pjpContext.EffectiveFrom.AddMonths(1).AddDays(-1);
                            }
                        }
                        else if (pjpContext.EffectiveTo.Month == permanentJourneyPlanDetailsDto.MonthId)
                        {
                            if (pjpContext.EffectiveFrom > new DateTime(pjpContext.EffectiveTo.Year, pjpContext.EffectiveTo.Month, 1))
                            {
                                financialYearDto.EffectiveFrom = pjpContext.EffectiveFrom;
                            }
                            else
                            {
                                financialYearDto.EffectiveFrom = new DateTime(pjpContext.EffectiveTo.Year, pjpContext.EffectiveTo.Month, 1);
                            }
                            financialYearDto.EffectiveTo = pjpContext.EffectiveTo;
                        }
                        else
                        {
                            int totalMonths = 12 * (pjpContext.EffectiveTo.Year - pjpContext.EffectiveFrom.Year) + pjpContext.EffectiveTo.Month - pjpContext.EffectiveFrom.Month;
                            int startMonth = pjpContext.EffectiveFrom.Month;
                            int endMonth = pjpContext.EffectiveTo.Month;
                            int month = startMonth - 1;
                            int year = pjpContext.EffectiveFrom.Year;
                            for (var i = 0; i <= totalMonths; i++)
                            {
                                if (month == 12)
                                {
                                    month = 0;
                                    year = year + 1;
                                }
                                month = month + 1;
                                if (month == permanentJourneyPlanDetailsDto.MonthId)
                                {
                                    financialYearDto.EffectiveFrom = new DateTime(year, month, 1);
                                    financialYearDto.EffectiveTo = financialYearDto.EffectiveFrom.AddMonths(1).AddDays(-1);
                                }
                            }
                        }
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = financialYearDto;
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

        public ResultDto CityByUserPermanentJourneyPlan(PermanentJourneyPlanDetailsDto permanentJourneyPlanDetailsDto)
        {
            _methodName = "CityByUserPermanentJourneyPlan";
            var resultDto = new ResultDto();
            var mtpCityList = new List<CityDto>();
            try
            {
                var pjpContext = _emamiContext.PermanentJourneyPlans.FirstOrDefault(_ => _.Id == permanentJourneyPlanDetailsDto.PJPId);
                if (pjpContext != null)
                {
                    if (pjpContext.PJPDetails.Any())
                    {
                        var citylist = (from ppjd in _emamiContext.PermanentJourneyPlanDetails
                                        join city in _emamiContext.City on ppjd.TownId equals city.Id
                                        where ppjd.PermanentJourneyPlanId == permanentJourneyPlanDetailsDto.PJPId //&& ppjd.MonthId == permanentJourneyPlanDetailsDto.MonthId
                                        select city
                                   ).Distinct().ToList();

                        foreach (var details in citylist)
                        {
                            var detailContext = new CityDto
                            {
                                CityId = details.Id,
                                CityName = details.CityName,
                            };
                            mtpCityList.Add(detailContext);
                        }
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = mtpCityList;
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

        public ResultDto GetApprovedOrRejectedPJPList(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetApprovedOrRejectedPJPList";
            var resultDto = new ResultDto();
            var permanentJourneyPlansDto = new List<PermanentJourneyPlansDto>();
            try
            {
                var toapprovepjp = (from ppj in _emamiContext.PermanentJourneyPlans
                                    join ppjai in _emamiContext.PJPApprovalInformation on ppj.Id equals ppjai.PermanentJourneyPlanId
                                    join ur in _emamiContext.UserReportingToMappings.AsNoTracking() on ppjai.CreatedBy equals ur.UserId
                                    where ur.ReportingToUserId == loginUserIdDto.LoginUserId && (ppj.PermanentJourneyPlanStatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Approved || ppj.PermanentJourneyPlanStatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Rejected)
                                    select ppj
                                   ).Distinct().ToList();

                foreach (var c in toapprovepjp)
                {
                    var detailContext = new PermanentJourneyPlansDto
                    {
                        PJPId = c.Id,
                        EncryptedId = UtilityHelper.ConvertToMd5(c.Id.ToString(), SecurityConstants.EncryptionKey),
                        PJPNumber = c.PJPNumber,
                        FinancialYearId = c.FinancialYearId,
                        FinancialYear = _emamiContext.FinancialYears.FirstOrDefault(_ => _.Id == c.FinancialYearId).Year.ToString(),
                        Remarks = c.Remarks,
                        CreatedBy = c.CreatedBy,
                        CreatedUser = _emamiContext.Users.FirstOrDefault(_ => _.Id == c.CreatedBy).Name,
                        StatusId = c.PermanentJourneyPlanStatusId,
                        Status = _emamiContext.PJPStatus.FirstOrDefault(_ => _.Id == c.PermanentJourneyPlanStatusId).Status,
                        CreatedDate = c.CreatedDate,
                    };
                    permanentJourneyPlansDto.Add(detailContext);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = permanentJourneyPlansDto.ToList();
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

        public ResultDto GetApprovedPJPList(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetApprovedPJPList";
            var resultDto = new ResultDto();
            var permanentJourneyPlansDto = new List<PermanentJourneyPlanDetailsDto>();
            try
            {
                var createdpjp = _emamiContext.PermanentJourneyPlans.Where(_ => _.CreatedBy == loginUserIdDto.LoginUserId && _.PermanentJourneyPlanStatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Approved).ToList();
                foreach (var c in createdpjp)
                {
                    var pjpdetails = _emamiContext.PermanentJourneyPlanDetails.Where(_ => _.PermanentJourneyPlanId == c.Id).ToList();
                    var detailContext = new PermanentJourneyPlanDetailsDto
                    {

                    };
                    permanentJourneyPlansDto.Add(detailContext);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = permanentJourneyPlansDto.ToList();
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
        public ResultDto GetRejectedPJPList(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetRejectedPJPList";
            var resultDto = new ResultDto();
            var permanentJourneyPlansDto = new List<PermanentJourneyPlansDto>();
            try
            {
                var toapprovepjp = (from ppj in _emamiContext.PermanentJourneyPlans
                                    join ppjai in _emamiContext.PJPApprovalInformation on ppj.Id equals ppjai.PermanentJourneyPlanId
                                    where ppjai.UserId == loginUserIdDto.LoginUserId && ppj.PermanentJourneyPlanStatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Rejected
                                    select ppj
                                   ).Distinct().ToList();

                foreach (var c in toapprovepjp)
                {
                    var detailContext = new PermanentJourneyPlansDto
                    {
                        PJPId = c.Id,
                        PJPNumber = c.PJPNumber,
                        FinancialYearId = c.FinancialYearId,
                        FinancialYear = _emamiContext.FinancialYears.FirstOrDefault(_ => _.Id == c.FinancialYearId).Year.ToString(),
                        Remarks = c.Remarks,
                        CreatedBy = c.CreatedBy,
                        CreatedUser = _emamiContext.Users.FirstOrDefault(_ => _.Id == c.CreatedBy).Name,
                        StatusId = c.PermanentJourneyPlanStatusId,
                        Status = _emamiContext.PJPStatus.FirstOrDefault(_ => _.Id == c.PermanentJourneyPlanStatusId).Status,
                    };
                    permanentJourneyPlansDto.Add(detailContext);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = permanentJourneyPlansDto.ToList();
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
        public ResultDto GetPendingPJPList(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetPendingPJPList";
            var resultDto = new ResultDto();
            var permanentJourneyPlansDto = new List<PermanentJourneyPlansDto>();
            try
            {
                var toapprovepjp = (from ppj in _emamiContext.PermanentJourneyPlans
                                    join ppjai in _emamiContext.PJPApprovalInformation on ppj.Id equals ppjai.PermanentJourneyPlanId
                                    join ur in _emamiContext.UserReportingToMappings.AsNoTracking() on ppjai.CreatedBy equals ur.UserId
                                    where ur.ReportingToUserId == loginUserIdDto.LoginUserId && ppj.PermanentJourneyPlanStatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Rejected
                                    select ppj
                                   ).Distinct().ToList();

                foreach (var c in toapprovepjp)
                {
                    var detailContext = new PermanentJourneyPlansDto
                    {
                        PJPId = c.Id,
                        PJPNumber = c.PJPNumber,
                        FinancialYearId = c.FinancialYearId,
                        FinancialYear = _emamiContext.FinancialYears.FirstOrDefault(_ => _.Id == c.FinancialYearId).Year.ToString(),
                        Remarks = c.Remarks,
                        CreatedBy = c.CreatedBy,
                        CreatedUser = _emamiContext.Users.FirstOrDefault(_ => _.Id == c.CreatedBy).Name,
                        StatusId = c.PermanentJourneyPlanStatusId,
                        Status = _emamiContext.PJPStatus.FirstOrDefault(_ => _.Id == c.PermanentJourneyPlanStatusId).Status,
                    };
                    permanentJourneyPlansDto.Add(detailContext);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = permanentJourneyPlansDto.ToList();
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

        #region Monthly Tour Plan
        public ResultDto AddMonthlyTourPlan(MonthlyTourPlanAddDto monthlyTourPlanAddDto)
        {
            _methodName = "AddMonthlyTourPlan";
            var resultDto = new ResultDto();
            try
            {
                if (monthlyTourPlanAddDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (monthlyTourPlanAddDto.CreatedBy == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (!monthlyTourPlanAddDto.MonthlyTourPlanDetails.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.MTPDetailsEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.MTPDetailsEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }
                var userContext = _emamiContext.UserReportingToMappings.AsNoTracking().FirstOrDefault(_ => _.UserId == monthlyTourPlanAddDto.CreatedBy);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (userContext.ReportingToUserId == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.PJPApprovalFlowEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.PJPApprovalFlowEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }
                List<string> existDateList = new List<string>();
                bool isError = false;
                foreach (var details in monthlyTourPlanAddDto.MonthlyTourPlanDetails)
                {
                    DateTime mtpDate = Convert.ToDateTime(details.Date);
                    if (!string.IsNullOrEmpty(details.DealerId) && details.DealerId != "0")
                    {
                        var noVisitExists = _emamiContext.MonthlyTourPlanDetails.AsNoTracking().FirstOrDefault(_ => DbFunctions.TruncateTime(_.Date) == DbFunctions.TruncateTime(mtpDate)
                            && _.InHQNoVisit != 0 && _.CreatedBy == monthlyTourPlanAddDto.CreatedBy && _.MonthlyTourPlan.MonthlyTourPlanStatusId != (int)DTO.Enums.MonthlyTourPlanStatus.Rejected);
                        if (noVisitExists != null)
                        {
                            isError = true;
                            existDateList.Add(details.Date);
                        }
                    }
                    if (details.InHQNoVisitId != 0)
                    {
                        var mtpExists = _emamiContext.MonthlyTourPlanDetails.AsNoTracking().FirstOrDefault(_ => DbFunctions.TruncateTime(_.Date) == DbFunctions.TruncateTime(mtpDate)
                            && _.CreatedBy == monthlyTourPlanAddDto.CreatedBy && _.MonthlyTourPlan.MonthlyTourPlanStatusId != (int)DTO.Enums.MonthlyTourPlanStatus.Rejected);
                        if (mtpExists != null)
                        {
                            isError = true;
                            existDateList.Add(details.Date);
                        }
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

                var mtpContext = new MonthlyTourPlans
                {
                    MonthlyTourPlanStatusId = Convert.ToInt32(monthlyTourPlanAddDto.StatusId),
                    CreatedBy = monthlyTourPlanAddDto.CreatedBy,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    PJPId = monthlyTourPlanAddDto.PJPId,
                    MonthId = monthlyTourPlanAddDto.MonthId,
                };
                _emamiContext.MonthlyTourPlans.Add(mtpContext);
                _emamiContext.SaveChanges();
                mtpContext.MTPNumber = Utility.MonthlyTourPlanNumberPrefix + mtpContext.Id;

                foreach (var details in monthlyTourPlanAddDto.MonthlyTourPlanDetails)
                {
                    var detailContext = new MonthlyTourPlanDetails
                    {
                        MonthlyTourPlanId = mtpContext.Id,
                        Date = Convert.ToDateTime(details.Date),
                        TownId = details.TownId,
                        Area = details.Area,
                        DealerId = details.DealerId,
                        HeadquartersId = details.HeadquartersId,
                        Remarks = details.Remarks,
                        CreatedBy = monthlyTourPlanAddDto.CreatedBy,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        InHQNoVisit = details.InHQNoVisitId
                    };
                    _emamiContext.MonthlyTourPlanDetails.Add(detailContext);
                }

                var approvalContext = new MonthlyTourPlanApprovalInformation
                {
                    MonthlyTourPlanId = mtpContext.Id,
                    CreatedBy = monthlyTourPlanAddDto.CreatedBy,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    UserId = userContext.ReportingToUserId,
                    MonthlyTourPlanStatusId = (int)monthlyTourPlanAddDto.StatusId
                };
                _emamiContext.MonthlyTourPlanApprovalInformation.Add(approvalContext);
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

        public ResultDto UpdateMonthlyTourPlan(MonthlyTourPlanUpdateDto monthlyTourPlanUpdateDto)
        {
            _methodName = "UpdateMonthlyTourPlan";
            var resultDto = new ResultDto();
            try
            {
                if (monthlyTourPlanUpdateDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (monthlyTourPlanUpdateDto.ModifiedBy == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (!monthlyTourPlanUpdateDto.MonthlyTourPlanDetails.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.PJPDetailsEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.PJPDetailsEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }
                var userContext = _emamiContext.UserReportingToMappings.AsNoTracking().FirstOrDefault(_ => _.UserId == monthlyTourPlanUpdateDto.ModifiedBy);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (userContext.ReportingToUserId == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.PJPApprovalFlowEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.PJPApprovalFlowEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }
                var mtpContext = _emamiContext.MonthlyTourPlans.FirstOrDefault(_ => _.Id == monthlyTourPlanUpdateDto.MTPId);
                if (mtpContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }
                List<string> existDateList = new List<string>();
                bool isError = false;
                foreach (var details in monthlyTourPlanUpdateDto.MonthlyTourPlanDetails)
                {
                    DateTime mtpDate = Convert.ToDateTime(details.Date);
                    if (!string.IsNullOrEmpty(details.DealerId) && details.DealerId != "0")
                    {
                        var noVisitExists = _emamiContext.MonthlyTourPlanDetails.AsNoTracking().FirstOrDefault(_ => DbFunctions.TruncateTime(_.Date) == DbFunctions.TruncateTime(mtpDate)
                            && _.InHQNoVisit != 0 && _.CreatedBy == mtpContext.CreatedBy && (details.Id != 0 ? _.Id != details.Id : true)
                            && _.MonthlyTourPlan.MonthlyTourPlanStatusId != (int)DTO.Enums.MonthlyTourPlanStatus.Rejected);
                        if (noVisitExists != null)
                        {
                            isError = true;
                            existDateList.Add(details.Date);
                        }
                    }
                    if (details.InHQNoVisitId != 0)
                    {
                        var mtpExists = _emamiContext.MonthlyTourPlanDetails.AsNoTracking().FirstOrDefault(_ => DbFunctions.TruncateTime(_.Date) == DbFunctions.TruncateTime(mtpDate)
                            && _.CreatedBy == mtpContext.CreatedBy && (details.Id != 0 ? _.Id != details.Id : true)
                            && _.MonthlyTourPlan.MonthlyTourPlanStatusId != (int)DTO.Enums.MonthlyTourPlanStatus.Rejected);
                        if (mtpExists != null)
                        {
                            isError = true;
                            existDateList.Add(details.Date);
                        }
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

                if (mtpContext.MonthlyTourPlanStatusId == (int)DTO.Enums.MonthlyTourPlanStatus.Pending)
                {
                    if (monthlyTourPlanUpdateDto.IsEditedByAdmin == 0)
                    {
                        mtpContext.MonthlyTourPlanStatusId = monthlyTourPlanUpdateDto.StatusId;
                    }
                    mtpContext.ModifiedBy = monthlyTourPlanUpdateDto.ModifiedBy;
                    mtpContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    _emamiContext.SaveChanges();
                }
                var existingRecords = monthlyTourPlanUpdateDto.MonthlyTourPlanDetails.Where(_ => _.Id != 0).ToList();
                foreach (var details in existingRecords)
                {
                    var mtpexistrecordsContext = _emamiContext.MonthlyTourPlanDetails.FirstOrDefault(_ => _.Id == details.Id);

                    #region STP Monthly History

                    if (!(mtpexistrecordsContext.Id == details.Id
                        && mtpexistrecordsContext.Date.Date == Convert.ToDateTime(details.Date)
                        && mtpexistrecordsContext.TownId == details.TownId
                        && mtpexistrecordsContext.Area == details.Area
                        && (((mtpexistrecordsContext.DealerId == null || mtpexistrecordsContext.DealerId == "0") && details.DealerId == "0") || mtpexistrecordsContext.DealerId == details.DealerId)
                        && mtpexistrecordsContext.HeadquartersId == details.HeadquartersId
                        && mtpexistrecordsContext.Remarks == details.Remarks
                        && mtpexistrecordsContext.InHQNoVisit == details.InHQNoVisitId))
                    {

                        AddSalesTourPlanMtpHistory(mtpexistrecordsContext, monthlyTourPlanUpdateDto.ModifiedBy);
                        mtpexistrecordsContext.MonthlyTourPlanId = mtpContext.Id;
                        mtpexistrecordsContext.ModifiedBy = monthlyTourPlanUpdateDto.ModifiedBy;
                        mtpexistrecordsContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        mtpexistrecordsContext.Date = Convert.ToDateTime(details.Date);
                        mtpexistrecordsContext.TownId = details.TownId;
                        mtpexistrecordsContext.Area = details.Area;
                        mtpexistrecordsContext.DealerId = details.DealerId;
                        mtpexistrecordsContext.HeadquartersId = details.HeadquartersId;
                        mtpexistrecordsContext.Remarks = details.Remarks;
                        mtpexistrecordsContext.InHQNoVisit = details.InHQNoVisitId;
                    }

                    #endregion

                    _emamiContext.SaveChanges();
                }

                var newRecords = monthlyTourPlanUpdateDto.MonthlyTourPlanDetails.Where(_ => _.Id == 0).ToList();
                foreach (var details in newRecords)
                {
                    var detailContext = new MonthlyTourPlanDetails
                    {
                        MonthlyTourPlanId = mtpContext.Id,
                        Date = Convert.ToDateTime(details.Date),
                        TownId = details.TownId,
                        Area = details.Area,
                        DealerId = details.DealerId,
                        HeadquartersId = details.HeadquartersId,
                        Remarks = details.Remarks,
                        CreatedBy = monthlyTourPlanUpdateDto.ModifiedBy,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        InHQNoVisit = details.InHQNoVisitId
                    };
                    _emamiContext.MonthlyTourPlanDetails.Add(detailContext);
                }

                if (monthlyTourPlanUpdateDto.IsEditedByAdmin == 0)
                {
                    if (monthlyTourPlanUpdateDto.StatusId == (int)DTO.Enums.MonthlyTourPlanStatus.Pending)
                    {
                        //New
                        var isApproverContext = _emamiContext.MonthlyTourPlanApprovalInformation.Any(_ => _.MonthlyTourPlanId == mtpContext.Id && _.MonthlyTourPlanStatusId == (int)DTO.Enums.MonthlyTourPlanStatus.Pending);
                        if (!isApproverContext)
                        {
                            _emamiContext.MonthlyTourPlanApprovalInformation.Add(new MonthlyTourPlanApprovalInformation
                            {
                                MonthlyTourPlanId = mtpContext.Id,
                                MonthlyTourPlanStatusId = (int)DTO.Enums.MonthlyTourPlanStatus.Pending,
                                CreatedBy = monthlyTourPlanUpdateDto.ModifiedBy,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                UserId = userContext.ReportingToUserId
                            });
                        }

                        var MTPapprover = _emamiContext.MonthlyTourPlans.FirstOrDefault(_ => _.Id == mtpContext.Id);
                        if (MTPapprover != null)
                        {
                            MTPapprover.MonthlyTourPlanStatusId = monthlyTourPlanUpdateDto.StatusId;
                        }

                        //Old
                        //var approvalContext = new MonthlyTourPlanApprovalInformation
                        //{
                        //    MonthlyTourPlanId = mtpContext.Id,
                        //    CreatedBy = monthlyTourPlanUpdateDto.ModifiedBy,
                        //    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        //    UserId = userContext.SalesReportingToId ?? 0,
                        //    MonthlyTourPlanStatusId = (int)DTO.Enums.MonthlyTourPlanStatus.Pending
                        //};
                        //_emamiContext.MonthlyTourPlanApprovalInformation.Add(approvalContext);
                    }
                    else
                    {
                        var approverContext = _emamiContext.MonthlyTourPlanApprovalInformation.FirstOrDefault(_ => _.MonthlyTourPlanId == mtpContext.Id && _.MonthlyTourPlanStatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Pending);

                        if (approverContext != null)
                        {
                            approverContext.MonthlyTourPlanStatusId = monthlyTourPlanUpdateDto.StatusId;
                            approverContext.ModifiedBy = monthlyTourPlanUpdateDto.ModifiedBy;
                            approverContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                            approverContext.Remarks = monthlyTourPlanUpdateDto.Remarks;
                            approverContext.ReasonId = monthlyTourPlanUpdateDto.ReasonIds;
                        }

                        var MTPapprover = _emamiContext.MonthlyTourPlans.FirstOrDefault(_ => _.Id == mtpContext.Id);
                        if (MTPapprover != null)
                        {
                            MTPapprover.MonthlyTourPlanStatusId = monthlyTourPlanUpdateDto.StatusId;
                        }
                    }
                }

                _emamiContext.SaveChanges();

                try
                {
                    AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                    var CreatedByUser = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == mtpContext.CreatedBy);
                    var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.MTPApproval);
                    string MTPStatus = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == monthlyTourPlanUpdateDto.StatusId).Name;
                    List<string> toUser = new List<string>();
                    toUser.Add(CreatedByUser.Email);
                    var emailSubject = Constants.MTPApprovalSubject;
                    var fromEmail = Constants.FromEmail;
                    var plainText = string.Empty;
                    if (emailTemplate != null)
                    {
                        var replaceEmailTemplate = emailTemplate.PlainTemplate.Replace(Constants.Name, CreatedByUser.Name).Replace(Constants.ApproveOrReject, MTPStatus);
                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, replaceEmailTemplate);
                        amazonNotificationService.SendEmail(toUser, emailSubject, plainText, htmlTemplate, true);
                    }
                    var smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.MTPApprovalSMS);
                    if (smsTemplate != null)
                    {
                        var replaceSmsTemplate = smsTemplate.PlainTemplate.Replace(Constants.Name, CreatedByUser.Name).Replace(Constants.ApproveOrReject, MTPStatus);
                        amazonNotificationService.SendMessage(replaceSmsTemplate, CreatedByUser.MobileNumber);

                        if (CreatedByUser != null && CreatedByUser.RegistrationTypeId != null && CreatedByUser.RegistrationTypeId > 0 && !string.IsNullOrEmpty(CreatedByUser.PushTokenKey))
                        {
                            PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                            {
                                PushTokenKey = CreatedByUser.PushTokenKey,
                                RegistrationTypeId = CreatedByUser.RegistrationTypeId != null ? (int)CreatedByUser.RegistrationTypeId : 0,
                                Title = emailSubject,
                                Message = replaceSmsTemplate,
                                //Id = mtpContext.Id,
                            };
                            _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                        }
                    }

                }
                catch (Exception ex)
                {

                }
                //}
                //else
                //{
                //    return _resultService.ErrorMessage(Constants.MonthlyTourPlanAlreadyUpdated);
                //}
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

        public ResultDto GetMonthlyTourPlanDetails(MTPIdDto mtpIdDto)
        {
            _methodName = "GetMonthlyTourPlanDetails";
            var resultDto = new ResultDto();
            var monthlyTourPlanDto = new MonthlyTourPlanDto();
            try
            {
                if (mtpIdDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (mtpIdDto.MTPId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                var mtpContext = _emamiContext.MonthlyTourPlans.FirstOrDefault(_ => _.Id == mtpIdDto.MTPId);
                var approverContext = _emamiContext.MonthlyTourPlanApprovalInformation.Where(_ => _.MonthlyTourPlanId == mtpIdDto.MTPId && _.MonthlyTourPlanStatusId != (int)DTO.Enums.MonthlyTourPlanStatus.Pending).OrderByDescending(_ => _.Id).FirstOrDefault();

                if (mtpContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }
                monthlyTourPlanDto.MTPId = mtpContext.Id;
                monthlyTourPlanDto.StatusId = mtpContext.MonthlyTourPlanStatusId;
                monthlyTourPlanDto.Status = mtpContext.MonthlyTourPlanStatus.Status;
                monthlyTourPlanDto.CreatedBy = mtpContext.CreatedBy;
                monthlyTourPlanDto.PJPId = mtpContext.PJPId;
                monthlyTourPlanDto.MonthId = mtpContext.MonthId;
                if (approverContext != null)
                {
                    monthlyTourPlanDto.Remarks = approverContext.Remarks;
                    monthlyTourPlanDto.ReasonIds = approverContext.ReasonId;
                }
                var mtpDetailsList = new List<MonthlyTourPlanDetailsDto>();
                if (mtpContext.MTPDetails.Any())
                {
                    foreach (var mtpdetails in mtpContext.MTPDetails)
                    {
                        var mtpDetails = new MonthlyTourPlanDetailsDto
                        {
                            MTPId = mtpContext.Id,
                            Id = mtpdetails.Id,
                            Date = mtpdetails.Date.ToString("dd-MMM-yyyy"),
                            Day = mtpdetails.Date.ToString("dddd"),
                            TownId = mtpdetails.TownId,
                            Town = _emamiContext.City.FirstOrDefault(_ => _.Id == mtpdetails.TownId)?.CityName,
                            Area = mtpdetails.Area,
                            DealerId = mtpdetails.DealerId,
                            HeadquartersId = mtpdetails.HeadquartersId,
                            Headquarters = _emamiContext.Headquarters.FirstOrDefault(_ => _.Id == mtpdetails.HeadquartersId)?.Name,
                            Remarks = mtpdetails.Remarks,
                            InHQNoVisitId = mtpdetails.InHQNoVisit,
                            InHQNoVisitName = mtpdetails.InHQNoVisit != 0 ? Utility.GetEnumFromString<DTO.Enums.STPVisitType>(mtpdetails.InHQNoVisit) : string.Empty
                        };

                        if (!string.IsNullOrEmpty(mtpDetails.DealerId) && mtpDetails.DealerId != "0")
                        {
                            var dealerIdsList = mtpdetails.DealerId.Split(',');
                            var dealerNames = string.Empty;
                            foreach (var dealer in dealerIdsList)
                            {
                                var dealerId = long.Parse(dealer);
                                var dealerDetail = _emamiContext.Users.FirstOrDefault(_ => _.Id == dealerId);
                                dealerNames = dealerDetail == null ? string.Empty : dealerDetail.Name + "," + dealerNames;
                            }
                            mtpDetails.Dealer = dealerNames.Remove(dealerNames.Length - 1, 1);
                        }
                        else
                        {
                            mtpDetails.Dealer = string.Empty;
                        }

                        mtpDetailsList.Add(mtpDetails);
                    }
                    monthlyTourPlanDto.MonthlyTourPlanDetailList = mtpDetailsList;
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = monthlyTourPlanDto;
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
        public ResultDto GetMonthlyTourPlanList(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetMonthlyTourList";
            var resultDto = new ResultDto();
            var monthlyTourPlansDto = new List<MonthlyTourPlanDto>();
            try
            {
                var createdmtp = _emamiContext.MonthlyTourPlans.Where(_ => _.CreatedBy == loginUserIdDto.LoginUserId).ToList();
                foreach (var c in createdmtp)
                {
                    var detailContext = new MonthlyTourPlanDto
                    {
                        MTPId = c.Id,
                        EncryptedId = UtilityHelper.ConvertToMd5(c.Id.ToString(), SecurityConstants.EncryptionKey),
                        MTPNumber = c.MTPNumber,
                        Remarks = c.Remarks,
                        CreatedBy = c.CreatedBy,
                        CreatedUser = _emamiContext.Users.FirstOrDefault(_ => _.Id == c.CreatedBy)?.Name,
                        StatusId = c.MonthlyTourPlanStatusId,
                        Status = _emamiContext.MonthlyTourPlanStatus.FirstOrDefault(_ => _.Id == c.MonthlyTourPlanStatusId)?.Status,
                        CreatedDate = c.CreatedDate,
                    };
                    monthlyTourPlansDto.Add(detailContext);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = monthlyTourPlansDto.ToList();
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

        public ResultDto GetPendingMonthlyTourPlanList(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetPendingMonthlyTourPlanList";
            var resultDto = new ResultDto();
            var monthlyTourPlanDto = new List<MonthlyTourPlanDto>();
            try
            {
                var toapprovepjp = (from ppj in _emamiContext.MonthlyTourPlans
                                    join ppjai in _emamiContext.MonthlyTourPlanApprovalInformation on ppj.Id equals ppjai.MonthlyTourPlanId
                                    join ur in _emamiContext.UserReportingToMappings on ppjai.CreatedBy equals ur.UserId
                                    where ur.ReportingToUserId == loginUserIdDto.LoginUserId && ppjai.MonthlyTourPlanStatusId == (int)DTO.Enums.MonthlyTourPlanStatus.Pending
                                    select ppj
                                   ).Distinct().ToList();

                foreach (var c in toapprovepjp)
                {
                    var detailContext = new MonthlyTourPlanDto
                    {
                        MTPId = c.Id,
                        EncryptedId = UtilityHelper.ConvertToMd5(c.Id.ToString(), SecurityConstants.EncryptionKey),
                        MTPNumber = c.MTPNumber,
                        Remarks = c.Remarks,
                        CreatedBy = c.CreatedBy,
                        CreatedUser = _emamiContext.Users.FirstOrDefault(_ => _.Id == c.CreatedBy).Name,
                        StatusId = c.MonthlyTourPlanStatusId,
                        Status = _emamiContext.MonthlyTourPlanStatus.FirstOrDefault(_ => _.Id == c.MonthlyTourPlanStatusId).Status,
                        CreatedDate = c.CreatedDate,
                    };
                    monthlyTourPlanDto.Add(detailContext);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = monthlyTourPlanDto.ToList();
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

        public ResultDto GetDateWeekDetails()
        {
            _methodName = "GetDateWeekDetails";
            var resultDto = new ResultDto();
            var dayOfWeekNameDto = new List<DayOfWeekNameDto>();
            try
            {
                dayOfWeekNameDto = _emamiContext.DayOfWeekNames.Where(_ => !_.IsHoliday).AsNoTracking().OrderBy(_ => _.Name).Select(c => new DayOfWeekNameDto
                {
                    DayOfWeekNameId = c.Id,
                    Name = c.Name,
                }).ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = dayOfWeekNameDto.OrderBy(_ => _.SortOrder).ToList();
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

        public ResultDto GetApprovedOrRejectedMTPList(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetApprovedOrRejectedMTPList";
            var resultDto = new ResultDto();
            var monthlyTourPlanDto = new List<MonthlyTourPlanDto>();
            try
            {
                var toapprovepjp = (from ppj in _emamiContext.MonthlyTourPlans
                                    join ppjai in _emamiContext.MonthlyTourPlanApprovalInformation on ppj.Id equals ppjai.MonthlyTourPlanId
                                    join ur in _emamiContext.UserReportingToMappings.AsNoTracking() on ppjai.CreatedBy equals ur.UserId
                                    where ur.ReportingToUserId== loginUserIdDto.LoginUserId && ppj.MonthlyTourPlanStatusId != (int)DTO.Enums.MonthlyTourPlanStatus.Pending
                                    select ppj
                                   ).Distinct().ToList();

                foreach (var c in toapprovepjp)
                {
                    var detailContext = new MonthlyTourPlanDto
                    {
                        EncryptedId = UtilityHelper.ConvertToMd5(c.Id.ToString(), SecurityConstants.EncryptionKey),
                        MTPId = c.Id,
                        MTPNumber = c.MTPNumber,
                        Remarks = c.Remarks,
                        CreatedBy = c.CreatedBy,
                        CreatedUser = _emamiContext.Users.FirstOrDefault(_ => _.Id == c.CreatedBy).Name,
                        StatusId = c.MonthlyTourPlanStatusId,
                        Status = _emamiContext.MonthlyTourPlanStatus.FirstOrDefault(_ => _.Id == c.MonthlyTourPlanStatusId).Status,
                        CreatedDate = c.CreatedDate,
                    };
                    monthlyTourPlanDto.Add(detailContext);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = monthlyTourPlanDto.ToList();
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

        #region Monthly Plan deviation
        public ResultDto ApprovedMonthlyTourPlanByUser(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "ApprovedMonthlyTourPlanByUser";
            var resultDto = new ResultDto();
            var permanentJourneyPlansDto = new List<MonthlyTourPlanDto>();
            try
            {
                var approvedmtp = (from mtp in _emamiContext.MonthlyTourPlans
                                   join mtpai in _emamiContext.MonthlyTourPlanApprovalInformation on mtp.Id equals mtpai.MonthlyTourPlanId
                                   where mtp.CreatedBy == loginUserIdDto.LoginUserId && mtpai.MonthlyTourPlanStatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Approved
                                   select mtp
                                   ).Distinct().ToList();

                if (approvedmtp != null)
                {
                    foreach (var c in approvedmtp)
                    {
                        var detailContext = new MonthlyTourPlanDto
                        {
                            MTPId = c.Id,
                            MTPNumber = c.MTPNumber,
                        };
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

        public ResultDto ApprovedMonthlyTourPlanDetailsByUser(MTPIdDto MTPIdDto)
        {
            _methodName = "ApprovedMonthlyTourPlanDetailsByUser";
            var resultDto = new ResultDto();
            var permanentJourneyPlansDto = new List<MonthlyTourPlanDeviationDto>();
            try
            {
                var approvedmtp = (from mtp in _emamiContext.MonthlyTourPlans
                                   join mtpd in _emamiContext.MonthlyTourPlanDetails on mtp.Id equals mtpd.MonthlyTourPlanId
                                   join pcp in _emamiContext.PermanentJourneyPlans.AsNoTracking() on mtp.PJPId equals pcp.Id
                                   where mtp.Id == MTPIdDto.MTPId && mtp.MonthlyTourPlanStatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Approved && mtpd != null && pcp != null
                                   select new { mtpd, pcp }
                                   ).Distinct().ToList();

                if (approvedmtp != null)
                {
                    foreach (var c in approvedmtp)
                    {
                        long statusId = 0;
                        var monthlyDeviationContext = _emamiContext.MonthlyPlanDeviation.AsNoTracking().Where(_ => _.MonthlyTourPlanDetailsId == c.mtpd.Id && _.StatusId != (int)DTO.Enums.MonthlyPlanDeviationStatus.Rejected).ToList().LastOrDefault();
                        if (monthlyDeviationContext != null)
                        {
                            statusId = monthlyDeviationContext.StatusId;
                        }
                        //var statusId = _emamiContext.MonthlyPlanDeviation.FirstOrDefault(_ => _.MonthlyTourPlanDetailsId == c.mtpd.Id) != null ? _emamiContext.MonthlyPlanDeviation.FirstOrDefault(_ => _.MonthlyTourPlanDetailsId == c.mtpd.Id).StatusId : 0;
                        //var monthlyDeviationContext = _emamiContext.MonthlyPlanDeviation.FirstOrDefault(_ => _.MonthlyTourPlanDetailsId == c.mtpd.Id && _.StatusId != (int)DTO.Enums.MonthlyPlanDeviationStatus.Rejected);
                        var detailContext = new MonthlyTourPlanDeviationDto
                        {
                            MTPId = c.mtpd.MonthlyTourPlanId,
                            MTPDetailId = c.mtpd.Id,
                            DealerId = c.mtpd.DealerId,
                            Town = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == c.mtpd.TownId) !=null ? _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == c.mtpd.TownId).CityName : String.Empty,
                            ActualDate = c.mtpd.Date.ToString("dd-MMM-yyyy"),
                            Status = statusId != 0 ? _emamiContext.MonthlyPlanDeviationStatus.FirstOrDefault(_ => _.Id == statusId).Status : string.Empty,
                            Remarks = monthlyDeviationContext != null ? monthlyDeviationContext.Remarks : string.Empty,
                            ApproverRemarks = monthlyDeviationContext != null ? monthlyDeviationContext.ApproverRemarks : string.Empty,
                            //RevisedDate = monthlyDeviationContext != null ? monthlyDeviationContext.RevisedDate.ToString("dd-MMM-yyyy") : c.mtpd.Date.ToString("dd-MMM-yyyy"),
                            Reasons = (monthlyDeviationContext != null && monthlyDeviationContext.ReasonId != 0) ? _emamiContext.Reasons.AsNoTracking().FirstOrDefault(_ => _.Id == monthlyDeviationContext.ReasonId)?.Reason : string.Empty,
                            ReasonId = monthlyDeviationContext != null ? monthlyDeviationContext.ReasonId : 0,
                            ToDealerId = monthlyDeviationContext != null ? monthlyDeviationContext.ToDealerId : 0,
                            ToDealer = (monthlyDeviationContext != null && monthlyDeviationContext.ToDealerId != 0) ? _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == monthlyDeviationContext.ToDealerId)?.Name : string.Empty,
                            InHQNoVisitId = c.mtpd.InHQNoVisit,
                            InHQNoVisitName = c.mtpd.InHQNoVisit != 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.STPVisitType)c.mtpd.InHQNoVisit) : string.Empty,
                            PCPValidFrom = c.pcp.EffectiveFrom.Date,
                            PCPValidTo = c.pcp.EffectiveTo.Date,
                            PCPValidFromString = c.pcp.EffectiveFrom.ToString("dd-MMM-yyyy"),
                            PCPValidToString = c.pcp.EffectiveTo.ToString("dd-MMM-yyyy"),
                        };

                        if (!string.IsNullOrEmpty(detailContext.DealerId) && detailContext.DealerId != "0")
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

        public ResultDto AddMonthlyPlanDeviation(AddMonthlyPlanDeviationDto addMonthlyPlanDeviationDto)
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
                var userContext = _emamiContext.UserReportingToMappings.AsNoTracking().FirstOrDefault(_ => _.UserId == addMonthlyPlanDeviationDto.CreatedBy);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }
                if (userContext.ReportingToUserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.PJPApprovalFlowEmpty;
                    resultDto.ErrorDto.Message = Constants.PJPApprovalFlowEmpty;
                    return resultDto;
                }

                List<string> existDateList = new List<string>();
                bool isError = false;
                foreach (var details in addMonthlyPlanDeviationDto.monthlyPlanDeviationListDto)
                {
                    DateTime mtpDate = Convert.ToDateTime(details.RevisedDate);
                    if (!string.IsNullOrEmpty(details.Dealer))
                    {
                        var noVisitExists = _emamiContext.MonthlyTourPlanDetails.AsNoTracking().FirstOrDefault(_ => DbFunctions.TruncateTime(_.Date) == DbFunctions.TruncateTime(mtpDate)
                            && _.InHQNoVisit != 0 && _.CreatedBy == addMonthlyPlanDeviationDto.CreatedBy && _.MonthlyTourPlan.MonthlyTourPlanStatusId != (int)DTO.Enums.MonthlyTourPlanStatus.Rejected);
                        if (noVisitExists != null)
                        {
                            isError = true;
                            existDateList.Add(details.RevisedDate);
                        }
                    }
                    if (!string.IsNullOrEmpty(details.InHQNoVisitName))
                    {
                        var mtpExists = _emamiContext.MonthlyTourPlanDetails.AsNoTracking().FirstOrDefault(_ => DbFunctions.TruncateTime(_.Date) == DbFunctions.TruncateTime(mtpDate)
                            && _.CreatedBy == addMonthlyPlanDeviationDto.CreatedBy && _.MonthlyTourPlan.MonthlyTourPlanStatusId != (int)DTO.Enums.MonthlyTourPlanStatus.Rejected);
                        if (mtpExists != null)
                        {
                            isError = true;
                            existDateList.Add(details.RevisedDate);
                        }
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

                foreach (var details in addMonthlyPlanDeviationDto.monthlyPlanDeviationListDto)
                {
                    var reasonContext = _emamiContext.Reasons.AsNoTracking().FirstOrDefault(_ => _.Id == details.ReasonId);
                    var mpdContext = new MonthlyPlanDeviations
                    {
                        MonthlyTourPlanDetailsId = details.MonthlyTourPlanDetailsId,
                        RevisedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        Remarks = details.Remarks,
                        ReasonId = reasonContext != null ? reasonContext.Id : 0,
                        ApproverId = userContext.ReportingToUserId,
                        StatusId = (int)DTO.Enums.MonthlyPlanDeviationStatus.Pending,
                        ToDealerId = details.ToDealerId,
                        CreatedBy = addMonthlyPlanDeviationDto.CreatedBy,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    };
                    _emamiContext.MonthlyPlanDeviation.Add(mpdContext);
                }
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

        public ResultDto PendingMonthlyPlanDeviation(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "PendingMonthlyPlanDeviation";
            var resultDto = new ResultDto();
            var permanentJourneyPlansDto = new List<MonthlyTourPlanDeviationDto>();
            try
            {
                var pendingmtp = (from mpd in _emamiContext.MonthlyPlanDeviation
                                  join mtpd in _emamiContext.MonthlyTourPlanDetails on mpd.MonthlyTourPlanDetailsId equals mtpd.Id
                                  join ur in _emamiContext.UserReportingToMappings.AsNoTracking() on mpd.CreatedBy equals ur.UserId
                                  where mpd.StatusId == (int)DTO.Enums.MonthlyPlanDeviationStatus.Pending
                                  && (ur.ReportingToUserId == loginUserIdDto.LoginUserId || mpd.CreatedBy == loginUserIdDto.LoginUserId)
                                  select new
                                  {
                                      MTPDetailId = mpd.MonthlyTourPlanDetailsId,
                                      DealerId = mtpd.DealerId,
                                      ActualDate = mtpd.Date,
                                      ToDealerId = mpd.ToDealerId,
                                      Remarks = mpd.Remarks,
                                      Id = mpd.Id,
                                      createdby = mpd.CreatedBy,
                                      ApproverId = mpd.ApproverId,
                                      StatusId=mpd.StatusId
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
                            //RevisedDate = c.RevisedDate.ToString("dd-MMM-yyyy"),
                            Id = c.Id,
                            CreatedBy = c.createdby,
                            CreatedByUser = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == c.createdby).Name,
                            ApprovedBy = c.ApproverId,
                            IsApprove = c.ApproverId > 0 ? true : false
                        };

                        if (!string.IsNullOrEmpty(detailContext.DealerId) && detailContext.DealerId != "0")
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

        public ResultDto ApprovedMonthlyPlanDeviation(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "ApprovedMonthlyPlanDeviation";
            var resultDto = new ResultDto();
            var permanentJourneyPlansDto = new List<MonthlyTourPlanDeviationDto>();
            try
            {
                var pendingmtp = (from mpd in _emamiContext.MonthlyPlanDeviation
                                  join mtpd in _emamiContext.MonthlyTourPlanDetails on mpd.MonthlyTourPlanDetailsId equals mtpd.Id
                                  join mtds in _emamiContext.MonthlyPlanDeviationStatus on mpd.StatusId equals mtds.Id
                                  join ur in _emamiContext.UserReportingToMappings.AsNoTracking() on mpd.CreatedBy equals ur.UserId
                                  where (ur.ReportingToUserId == loginUserIdDto.LoginUserId || mpd.CreatedBy == loginUserIdDto.LoginUserId) && mpd.StatusId != (int)DTO.Enums.MonthlyPlanDeviationStatus.Pending
                                  select new
                                  {
                                      MTPDetailId = mpd.MonthlyTourPlanDetailsId,
                                      DealerId = mtpd.DealerId,
                                      ActualDate = mtpd.Date,
                                      ToDealerId = mpd.ToDealerId,
                                      Remarks = mpd.Remarks,
                                      Status = mtds.Status,
                                      createdby = mpd.CreatedBy,
                                      ApproverId = mpd.ApproverId
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
                            //RevisedDate = c.RevisedDate.ToString("dd-MMM-yyyy"),
                            Status = c.Status,
                            CreatedBy = c.createdby,
                            CreatedByUser = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == c.createdby).Name,
                            ApprovedBy = c.ApproverId,
                            IsApprove = c.ApproverId > 0 ? true : false
                        };

                        if (!string.IsNullOrEmpty(detailContext.DealerId) && detailContext.DealerId != "0")
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

        public ResultDto RejectedMonthlyPlanDeviationForMobile(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "ApprovedMonthlyPlanDeviation";
            var resultDto = new ResultDto();
            var permanentJourneyPlansDto = new List<MonthlyTourPlanDeviationDto>();
            try
            {
                var pendingmtp = (from mpd in _emamiContext.MonthlyPlanDeviation
                                  join mtpd in _emamiContext.MonthlyTourPlanDetails on mpd.MonthlyTourPlanDetailsId equals mtpd.Id
                                  join mtds in _emamiContext.MonthlyPlanDeviationStatus on mpd.StatusId equals mtds.Id
                                  join ur in _emamiContext.UserReportingToMappings.AsNoTracking() on mpd.CreatedBy equals ur.UserId
                                  where ur.ReportingToUserId == loginUserIdDto.LoginUserId && mpd.StatusId == (int)DTO.Enums.MonthlyPlanDeviationStatus.Rejected
                                  select new
                                  {
                                      MTPDetailId = mpd.MonthlyTourPlanDetailsId,
                                      DealerId = mtpd.DealerId,
                                      ActualDate = mtpd.Date,
                                      //RevisedDate = mpd.RevisedDate,
                                      Remarks = mpd.Remarks,
                                      Status = mtds.Status

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
                            //RevisedDate = c.RevisedDate.ToString("dd-MMM-yyyy"),
                            Status = c.Status
                        };

                        if (!string.IsNullOrEmpty(detailContext.DealerId) && detailContext.DealerId != "0")
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

        public ResultDto UpdateMonthlyPlanDeviation(MonthlyPlanDeviationUpdateDto monthlyPlanDeviationUpdateDto)
        {
            _methodName = "UpdateMonthlyPlanDeviation";
            var resultDto = new ResultDto();
            try
            {
                if (monthlyPlanDeviationUpdateDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (monthlyPlanDeviationUpdateDto.ModifiedBy == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == monthlyPlanDeviationUpdateDto.ModifiedBy);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                foreach (var details in monthlyPlanDeviationUpdateDto.monthlyPlanDeviationListDto)
                {
                    if (details.StatusId != (int)DTO.Enums.MonthlyPlanDeviationStatus.Pending)
                    {
                        var mpdContext = _emamiContext.MonthlyPlanDeviation.FirstOrDefault(_ => _.Id == details.Id);
                        if (mpdContext != null && mpdContext.StatusId == (int)DTO.Enums.Status.Pending || mpdContext.StatusId == (int)DTO.Enums.Status.RequestForApproval)
                        {
                           // mpdContext.RevisedDate = Convert.ToDateTime(details.RevisedDate);
                            mpdContext.ToDealerId = details.ToDealerId;
                            mpdContext.StatusId = details.StatusId;
                            mpdContext.ModifiedBy = monthlyPlanDeviationUpdateDto.ModifiedBy;
                            mpdContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                            _emamiContext.SaveChanges();
                        }

                        try
                        {
                            AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                            var CreatedByUser = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == mpdContext.CreatedBy);
                            var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.MTPDeviationApproval);
                            string MTPStatus = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == details.StatusId).Name;
                            List<string> toUser = new List<string>();
                            toUser.Add(CreatedByUser.Email);
                            var emailSubject = Constants.MTPDeviationApprovalSubject;
                            var fromEmail = Constants.FromEmail;
                            var plainText = string.Empty;
                            if (emailTemplate != null)
                            {
                                var replaceEmailTemplate = emailTemplate.PlainTemplate.Replace(Constants.Name, CreatedByUser.Name).Replace(Constants.Date, details.RevisedDate).Replace(Constants.ApproveOrReject, MTPStatus);
                                var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, replaceEmailTemplate);
                                amazonNotificationService.SendEmail(toUser, emailSubject, plainText, htmlTemplate, true);
                            }
                            var smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.MTPDeviationApprovalSMS);
                            if (smsTemplate != null)
                            {
                                var replaceSmsTemplate = smsTemplate.PlainTemplate.Replace(Constants.Name, CreatedByUser.Name).Replace(Constants.Date, details.RevisedDate).Replace(Constants.ApproveOrReject, MTPStatus);
                                amazonNotificationService.SendMessage(replaceSmsTemplate, CreatedByUser.MobileNumber);

                                if (CreatedByUser != null && CreatedByUser.RegistrationTypeId != null && CreatedByUser.RegistrationTypeId > 0 && !string.IsNullOrEmpty(CreatedByUser.PushTokenKey))
                                {
                                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
                                    {
                                        PushTokenKey = CreatedByUser.PushTokenKey,
                                        RegistrationTypeId = CreatedByUser.RegistrationTypeId != null ? (int)CreatedByUser.RegistrationTypeId : 0,
                                        Title = emailSubject,
                                        Message = replaceSmsTemplate,
                                        //Id = mpdContext.Id,
                                    };
                                    _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            var message = $"{ServiceName} Service-Method {_methodName} Exception: {ex}";
                            _logger.Error(message);
                        }
                    }
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = monthlyPlanDeviationUpdateDto;
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

        public ResultDto ViewMonthlyTourPlanDeviationDetails(IdInputDto idInputDto)
        {
            _methodName = "ViewMonthlyTourPlanDeviationDetails";
            var resultDto = new ResultDto();
            var permanentJourneyPlansDto = new List<MonthlyTourPlanDeviationDto>();
            try
            {
                var pendingmtp = (from mpd in _emamiContext.MonthlyPlanDeviation
                                  join mtpd in _emamiContext.MonthlyTourPlanDetails on mpd.MonthlyTourPlanDetailsId equals mtpd.Id
                                  join mtds in _emamiContext.MonthlyPlanDeviationStatus on mpd.StatusId equals mtds.Id
                                  where mpd.Id == idInputDto.Id
                                  select new
                                  {
                                      MTPDetailId = mpd.MonthlyTourPlanDetailsId,
                                      DealerId = mtpd.DealerId,
                                      ActualDate = mtpd.Date,
                                      ToDealerId = mpd.ToDealerId,
                                      Remarks = mpd.Remarks,
                                      Status = mtds.Status,
                                      ReasonId = mpd.ReasonId,
                                      ApproverRemarks = mpd.ApproverRemarks

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
                            //RevisedDate = c.RevisedDate.ToString("dd-MMM-yyyy"),
                            Status = c.Status,
                            ReasonId = c.ReasonId,
                            ApproverRemarks = c.ApproverRemarks,
                            Reason = _emamiContext.Reasons.AsNoTracking().FirstOrDefault(_ => _.Id == c.ReasonId).Reason
                        };

                        if (!string.IsNullOrEmpty(detailContext.DealerId) && detailContext.DealerId != "0")
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

        public ResultDto CheckMonthlyPlanDeviationApproveByLoginedUser(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "CheckMonthlyPlanDeviationApproveByLoginedUser";
            var resultDto = new ResultDto();
            var monthlyTourPlanDeviationDto = new MonthlyTourPlanDeviationDto();
            try
            {
                var pendingmtp = (from mpd in _emamiContext.MonthlyPlanDeviation
                                  join mtpd in _emamiContext.MonthlyTourPlanDetails on mpd.MonthlyTourPlanDetailsId equals mtpd.Id
                                  where mpd.StatusId == (int)DTO.Enums.MonthlyPlanDeviationStatus.Pending
                                  && mpd.ApproverId == loginUserIdDto.LoginUserId
                                  select new
                                  {
                                      MTPDetailId = mpd.MonthlyTourPlanDetailsId,
                                      DealerId = mtpd.DealerId,
                                      ActualDate = mtpd.Date,
                                      RevisedDate = mpd.RevisedDate,
                                      Remarks = mpd.Remarks,
                                      Id = mpd.Id,
                                      createdby = mpd.CreatedBy,
                                      ApproverId = mpd.ApproverId

                                  }).Distinct().ToList();

                if (pendingmtp != null && pendingmtp.Any())
                {
                    monthlyTourPlanDeviationDto = pendingmtp.Select(c => new MonthlyTourPlanDeviationDto
                    {
                        ApprovedBy = c.ApproverId,
                        IsApprove = c.ApproverId > 0 ? true : false
                    }).FirstOrDefault();
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = monthlyTourPlanDeviationDto;
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

        #region Todays Activities
        public ResultDto TodayActivities(TodayActivitiesInputDto todayActivitiesInputDto)
        {
            _methodName = "TodayActivities";
            var resultDto = new ResultDto();
            var permanentJourneyPlansDto = new List<MonthlyTourPlanDetailsDto>();
            try
            {
                var approvedmtp = (from mtp in _emamiContext.MonthlyTourPlans
                                   join mtpd in _emamiContext.MonthlyTourPlanDetails on mtp.Id equals mtpd.MonthlyTourPlanId
                                   where DbFunctions.TruncateTime(mtpd.CreatedDate) == DbFunctions.TruncateTime(todayActivitiesInputDto.TodayDate) && mtp.MonthlyTourPlanStatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Approved
                                   && mtp.CreatedBy == todayActivitiesInputDto.LoginUserId
                                   select mtpd
                                   ).Distinct().ToList();

                if (approvedmtp != null)
                {
                    foreach (var c in approvedmtp)
                    {
                        var detailContext = new MonthlyTourPlanDetailsDto
                        {
                            MTPId = c.MonthlyTourPlanId,
                            Id = c.Id,
                            DealerId = c.DealerId,
                            EncryptedId = UtilityHelper.ConvertToMd5(c.DealerId.ToString(), SecurityConstants.EncryptionKey),
                            Area = c.Area,
                            //Headquarters = _emamiContext.Headquarters.AsNoTracking().FirstOrDefault(_ => _.Id == c.HeadquartersId).Name,
                            Town = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == c.TownId) != null ? _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == c.TownId).CityName : string.Empty,
                            TravelTo = string.Empty,
                            InHQNoVisitId = c.InHQNoVisit,
                            InHQNoVisitName = c.InHQNoVisit != 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.STPVisitType)c.InHQNoVisit) : string.Empty,
                            Remarks = c.Remarks,
                            VisitRemarks = c.VisitRemarks,
                        };

                        if (!string.IsNullOrEmpty(detailContext.DealerId) && detailContext.DealerId != "0")
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

        public ResultDto TodayActivitiesDealerList(TodayActivitiesInputDto inputDto)
        {
            _methodName = "TodayActivitiesDealerList";
            var resultDto = new ResultDto();
            if (inputDto == null)
            {
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.InvalidRequest;
                return resultDto;
            }
            if (inputDto.TodayDate == null || inputDto.TodayDate == DateTime.MinValue)
            {
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.InvalidDate;
                return resultDto;
            }
            if (inputDto.LoginUserId == 0)
            {
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.UserIdMissing;
                return resultDto;
            }

            var monthlyTourPlansDetailsDto = new List<MonthlyTourPlanDetailsDto>();
            try
            {
                var approvedmtp = (from mtp in _emamiContext.MonthlyTourPlans
                                   join mtpd in _emamiContext.MonthlyTourPlanDetails on mtp.Id equals mtpd.MonthlyTourPlanId
                                   where DbFunctions.TruncateTime(mtpd.Date) == DbFunctions.TruncateTime(inputDto.TodayDate) && mtp.MonthlyTourPlanStatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Approved
                                   && mtp.CreatedBy == inputDto.LoginUserId
                                   select mtpd
                                   ).Distinct().ToList();

                if (approvedmtp != null)
                {
                    foreach (var c in approvedmtp)
                    {
                        var dealerID = long.Parse(c.DealerId);
                        var detailContext = new MonthlyTourPlanDetailsDto
                        {
                            MTPId = c.MonthlyTourPlanId,
                            DealerId = c.DealerId,
                            Dealer = c.DealerId != null ? _emamiContext.Users.FirstOrDefault(_ => _.Id == dealerID).Name : string.Empty,
                            TownId = c.TownId,
                            Town = _emamiContext.City.FirstOrDefault(_ => _.Id == c.TownId).CityName,
                            Area = c.Area,
                            HeadquartersId = c.HeadquartersId,
                            Headquarters = _emamiContext.Headquarters.FirstOrDefault(_ => _.Id == c.HeadquartersId).Name,
                            Date = c.Date.ToLongDateString(),
                            Remarks = c.Remarks
                        };

                        monthlyTourPlansDetailsDto.Add(detailContext);
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = monthlyTourPlansDetailsDto;
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

        public ResultDto AddPendingSauda(AddPendingSaudaRemarksDto addPendingSaudaRemarksDto)
        {
            _methodName = "AddPendingSauda";
            var resultDto = new ResultDto();
            try
            {
                if (addPendingSaudaRemarksDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (addPendingSaudaRemarksDto.CreatedBy == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == addPendingSaudaRemarksDto.CreatedBy);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }

                //foreach (var details in addPendingSaudaRemarksDto)
                //{
                var psContext = new PendingSaudaRemarks
                {
                    DealerId = addPendingSaudaRemarksDto.DealerId,
                    SaudaId = addPendingSaudaRemarksDto.SaudaId,
                    Remarks = addPendingSaudaRemarksDto.Remarks,
                    CreatedBy = addPendingSaudaRemarksDto.CreatedBy,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                };
                _emamiContext.PendingSaudaRemarks.Add(psContext);
                //}
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = psContext.Id;
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

        public ResultDto GetPendingSaudaList(PendingSaudaInputDto inputDto)
        {
            _methodName = "GetPendingSaudaList";
            var resultDto = new ResultDto();
            var pendingSaudaDtoList = new List<PendingSaudaDto>();

            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                if (inputDto.DealerId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.DealerIdEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.DealerIdEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }
                pendingSaudaDtoList = _emamiContext.PendingSaudaRemarks.AsNoTracking().Where(_ => _.DealerId == inputDto.DealerId).Select(c => new PendingSaudaDto()
                {
                    Id = c.Id,
                    DealerId = c.DealerId,
                    SaudaId = c.SaudaId,
                    Remarks = c.Remarks
                }).ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = pendingSaudaDtoList;
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

        public ResultDto AddMarketScenario(AddMarketScenarioDto addMarketScenarioDto)
        {
            _methodName = "AddMarketScenario";
            var resultDto = new ResultDto();
            try
            {
                if (addMarketScenarioDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (addMarketScenarioDto.CreatedBy == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == addMarketScenarioDto.CreatedBy);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }

                //foreach (var details in addPendingSaudaRemarksDto)
                //{
                var marketScenarioContext = new MarketScenario
                {
                    DealerId = addMarketScenarioDto.DealerId,
                    Title = addMarketScenarioDto.Title,
                    Remarks = addMarketScenarioDto.Remarks,
                    CreatedBy = addMarketScenarioDto.CreatedBy,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                };
                _emamiContext.MarketScenario.Add(marketScenarioContext);
                //}
                _emamiContext.SaveChanges();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = marketScenarioContext.Id;
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

        public ResultDto AddBDOCompetitorDetails(BdoCompetitorAddListDto bdoCompetitorAddListDto)
        {
            _methodName = "AddBDOCompetitorDetails";
            var resultDto = new ResultDto();
            try
            {
                if (bdoCompetitorAddListDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (bdoCompetitorAddListDto.CreatedBy == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == bdoCompetitorAddListDto.CreatedBy);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }
                foreach (var bdoCompetitorAddDto in bdoCompetitorAddListDto.BdoCompetitorAddDto)
                {
                    var competitorContext = new BdoCompetitor
                    {
                        Name = bdoCompetitorAddDto.Name,
                        Remarks = bdoCompetitorAddDto.Remarks,
                        IsActive = bdoCompetitorAddDto.IsActive,
                        UserType = bdoCompetitorAddDto.UserType,
                        DealerId = bdoCompetitorAddDto.DealerId,
                        CreatedBy = bdoCompetitorAddDto.CreatedBy,
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
                            CreatedBy = details.CreatedBy,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };
                        _emamiContext.BdoCompetitorSku.Add(detailContext);
                    }
                    _emamiContext.SaveChanges();
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
        public ResultDto AddProspectiveDealer(ProspectiveDealerAddListDto prospectiveDealerAddListDto)
        {
            _methodName = "AddProspectiveDealer";
            var resultDto = new ResultDto();
            try
            {
                if (prospectiveDealerAddListDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (prospectiveDealerAddListDto.ProspectiveDealerAddDto.CreatedBy == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == prospectiveDealerAddListDto.ProspectiveDealerAddDto.CreatedBy);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }

                var prospectivedealerContext = new ProspectiveDealer
                {
                    Name = prospectiveDealerAddListDto.ProspectiveDealerAddDto.Name,
                    Email = prospectiveDealerAddListDto.ProspectiveDealerAddDto.Email,
                    MobileNumber = prospectiveDealerAddListDto.ProspectiveDealerAddDto.MobileNumber,
                    StateId = prospectiveDealerAddListDto.ProspectiveDealerAddDto.StateId,
                    DistrictId = prospectiveDealerAddListDto.ProspectiveDealerAddDto.DistrictId,
                    CityId = prospectiveDealerAddListDto.ProspectiveDealerAddDto.CityId,
                    Pincode = prospectiveDealerAddListDto.ProspectiveDealerAddDto.Pincode,
                    Address = prospectiveDealerAddListDto.ProspectiveDealerAddDto.Address,
                    IsActive = prospectiveDealerAddListDto.ProspectiveDealerAddDto.IsActive,
                    ProspectiveSales = prospectiveDealerAddListDto.ProspectiveDealerAddDto.ProspectiveSales,
                    ProspectiveInterestLevel = prospectiveDealerAddListDto.ProspectiveDealerAddDto.ProspectiveInterestLevel,
                    BusinessPotentialPeryear = prospectiveDealerAddListDto.ProspectiveDealerAddDto.BusinessPotentialPeryear,
                    DealerId = prospectiveDealerAddListDto.ProspectiveDealerAddDto.DealerId,
                    CreatedBy = prospectiveDealerAddListDto.ProspectiveDealerAddDto.CreatedBy,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                };
                _emamiContext.ProspectiveDealer.Add(prospectivedealerContext);
                _emamiContext.SaveChanges();

                foreach (var file in prospectiveDealerAddListDto.ProspectiveDealerAddDto.FileList)
                {
                    var attachmentContext = _emamiContext.Attachment.FirstOrDefault(_ => _.Id == file.Id);
                    if (attachmentContext != null)
                    {
                        attachmentContext.RecordId = prospectivedealerContext.Id;
                        _emamiContext.SaveChanges();
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

        public ResultDto GetProspectiveDealerList(SalesTourPlanParamDto inputDto)
        {
            _methodName = "GetProspectiveDealerList";
            var resultDto = new ResultDto();
            var prospectivedealerDto = new List<ProspectiveDealerDto>();
            if (inputDto == null)
            {
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                return resultDto;
            }
            try
            {
                var prospectivedealercontext = _emamiContext.ProspectiveDealer.AsNoTracking().Where(w => w.DealerId == inputDto.DealerId && DbFunctions.TruncateTime(w.CreatedDate) == DbFunctions.TruncateTime(inputDto.CreatedDate)).ToList();

                foreach (var c in prospectivedealercontext)
                {
                    var dto = new ProspectiveDealerDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Email = c.Email,
                        MobileNumber = c.MobileNumber,
                        Pincode = c.Pincode,
                        Address = c.Address,
                        IsActive = c.IsActive,
                        ProspectiveSales = c.ProspectiveSales,
                        ProspectiveInterestLevel = c.ProspectiveInterestLevel,
                        BusinessPotentialPeryear = c.BusinessPotentialPeryear,
                        DealerId = c.DealerId,
                        Dealer = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == c.DealerId).Name,
                        CreatedBy = c.CreatedBy,
                        CreatedDate = c.CreatedDate
                    };
                    prospectivedealerDto.Add(dto);
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = prospectivedealerDto;
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

        public ResultDto GetProspectiveDealerById(IdInputDto inputDto)
        {
            _methodName = "GetSurpriseDiscountById";
            var resultDto = new ResultDto();
            var prospectiveDealerDto = new ProspectiveDealerDto();
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
                resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                return resultDto;
            }
            try
            {
                var prospectivedealerContext = _emamiContext.ProspectiveDealer.FirstOrDefault(_ => _.Id == inputDto.Id);
                if (prospectivedealerContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }
                prospectiveDealerDto.Id = prospectivedealerContext.Id;
                prospectiveDealerDto.Name = prospectivedealerContext.Name;
                prospectiveDealerDto.Email = prospectivedealerContext.Email;
                prospectiveDealerDto.MobileNumber = prospectivedealerContext.MobileNumber;
                prospectiveDealerDto.Pincode = prospectivedealerContext.Pincode;
                prospectiveDealerDto.Address = prospectivedealerContext.Address;
                prospectiveDealerDto.IsActive = prospectivedealerContext.IsActive;
                prospectiveDealerDto.ProspectiveSales = prospectivedealerContext.ProspectiveSales;
                prospectiveDealerDto.ProspectiveInterestLevel = prospectivedealerContext.ProspectiveInterestLevel;
                prospectiveDealerDto.BusinessPotentialPeryear = prospectivedealerContext.BusinessPotentialPeryear;
                prospectiveDealerDto.DealerId = prospectivedealerContext.DealerId;
                prospectiveDealerDto.Dealer = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == prospectivedealerContext.DealerId).Name;
                prospectiveDealerDto.CreatedBy = prospectivedealerContext.CreatedBy;
                prospectiveDealerDto.CreatedDate = prospectivedealerContext.CreatedDate;

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = prospectiveDealerDto;
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

        #region User Sauda and Sales Target
        public ResultDto GetMonthsByFinancialYear(FinancialYearIdDto financialYearIdDto)
        {
            _methodName = "GetMonthsByFinancialYear";
            var resultDto = new ResultDto();
            var financialYearDto = new FinancialYearDto();
            var MonthList = new List<UserSalesSaudaTargetDetailDto>();
            try
            {
                var financialYearContext = _emamiContext.FinancialYears.AsNoTracking().FirstOrDefault(_ => _.Id == financialYearIdDto.FinancialYearid);
                if (financialYearContext != null)
                {
                    financialYearDto.EffectiveFrom = financialYearContext.EffectiveFrom;
                    financialYearDto.EffectiveTo = financialYearContext.EffectiveTo;

                    int totalMonths = 12 * (financialYearDto.EffectiveTo.Year - financialYearDto.EffectiveFrom.Year) + financialYearDto.EffectiveTo.Month - financialYearDto.EffectiveFrom.Month;
                    List<Month> months = new List<Month>();
                    int startMonth = financialYearContext.EffectiveFrom.Month;
                    int endMonth = financialYearContext.EffectiveTo.Month;
                    int month = startMonth - 1;
                    for (var i = 0; i <= totalMonths; i++)
                    {
                        Month toaddmonth = new Month();
                        if (month == 12)
                        {
                            month = 0;
                        }
                        month = month + 1;
                        toaddmonth.Id = month;
                        months.Add(toaddmonth);
                    }

                    var monthlist = _emamiContext.Months.AsNoTracking().ToList();
                    var tempmonths = (from m in monthlist
                                      join t in months on m.Id equals t.Id
                                      select m).ToList();

                    if (tempmonths.Any())
                    {
                        foreach (var details in tempmonths)
                        {
                            var pjpDetails = new UserSalesSaudaTargetDetailDto
                            {
                                MonthId = details.Id,
                                Month = _emamiContext.Months.FirstOrDefault(_ => _.Id == details.Id).Name,
                                SalesTarget = 0,
                                SaudaTarget = 0,
                            };
                            MonthList.Add(pjpDetails);
                        }
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = MonthList;
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

        //public ResultDto AddUserSalesSaudaTarget(UserSalesSaudaTargetDto userSalesSaudaTargetDto)
        //{
        //    _methodName = "AddUserSalesSaudaTarget";
        //    var resultDto = new ResultDto();
        //    try
        //    {
        //        if (userSalesSaudaTargetDto == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        if (userSalesSaudaTargetDto.CreatedBy == 0)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        if (!userSalesSaudaTargetDto.UserSalesSaudaTargetDetail.Any())
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.PJPDetailsEmpty;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.PJPDetailsEmpty, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == userSalesSaudaTargetDto.CreatedBy);
        //        if (userContext == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
        //            resultDto.ErrorDto.Message = Constants.UserNotFound;
        //            return resultDto;
        //        }

        //        foreach (var detail in userSalesSaudaTargetDto.UserSalesSaudaTargetDetail)
        //        {
        //            var detailContext = new UserSalesSaudaTarget
        //            {
        //                AssignedFromId = userSalesSaudaTargetDto.CreatedBy,
        //                AssignedToId = userSalesSaudaTargetDto.UserId,
        //                Month = detail.MonthId,
        //                Year = userSalesSaudaTargetDto.FinancialYearId,
        //                SalesTarget = detail.SalesTarget,
        //                SaudaTarget = detail.SaudaTarget,
        //                CreatedBy = userSalesSaudaTargetDto.CreatedBy,
        //                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        //            };
        //            _emamiContext.UserSalesSaudaTarget.Add(detailContext);
        //            _emamiContext.SaveChanges();
        //        }
        //        resultDto.IsSuccess = true;
        //        resultDto.SuccessDto.Response = userSalesSaudaTargetDto;
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

        //public ResultDto UpdateUserSalesSaudaTarget(UserSalesSaudaTargetDto userSalesSaudaTargetDto)
        //{
        //    _methodName = "UpdateUserSalesSaudaTarget";
        //    var resultDto = new ResultDto();
        //    try
        //    {
        //        if (userSalesSaudaTargetDto == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        if (userSalesSaudaTargetDto.CreatedBy == 0)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        if (!userSalesSaudaTargetDto.UserSalesSaudaTargetDetail.Any())
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.PJPDetailsEmpty;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.PJPDetailsEmpty, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == userSalesSaudaTargetDto.CreatedBy);
        //        if (userContext == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
        //            return resultDto;
        //        }


        //        foreach (var details in userSalesSaudaTargetDto.UserSalesSaudaTargetDetail)
        //        {
        //            var recordsContext = _emamiContext.UserSalesSaudaTarget.FirstOrDefault(_ => _.Id == details.Id);
        //            recordsContext.SaudaTarget = details.SaudaTarget;
        //            recordsContext.SalesTarget = details.SalesTarget;
        //            recordsContext.ModifiedBy = userSalesSaudaTargetDto.CreatedBy;
        //            recordsContext.ModifiedDate = DateTime.UtcNow;
        //            _emamiContext.SaveChanges();
        //        }
        //        resultDto.IsSuccess = true;
        //        resultDto.SuccessDto.Response = userSalesSaudaTargetDto;
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
        //public ResultDto GetUserSalesSaudaTarget(IdInputDto idInputDto)
        //{
        //    _methodName = "GetUserSalesSaudaTarget";
        //    var resultDto = new ResultDto();
        //    var usersalestargetDto = new UserSalesSaudaTargetDto();
        //    try
        //    {
        //        if (idInputDto == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        if (idInputDto.Id == 0)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        var targetContext = _emamiContext.UserSalesSaudaTarget.FirstOrDefault(_ => _.Year == idInputDto.Id && _.AssignedToId == idInputDto.LoginUserId);

        //        if (targetContext == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        usersalestargetDto.Id = targetContext.Id;
        //        usersalestargetDto.UserId = targetContext.AssignedToId;
        //        usersalestargetDto.FinancialYearId = targetContext.Year;

        //        var targetlistContext = _emamiContext.UserSalesSaudaTarget.AsNoTracking().Where(_ => _.AssignedToId == usersalestargetDto.UserId && _.Year == usersalestargetDto.FinancialYearId).ToList();

        //        var DetailsList = new List<UserSalesSaudaTargetDetailDto>();
        //        if (targetlistContext.Any())
        //        {
        //            foreach (var details in targetlistContext)
        //            {
        //                var pjpDetails = new UserSalesSaudaTargetDetailDto
        //                {
        //                    Id = details.Id,
        //                    MonthId = details.Month,
        //                    Month = _emamiContext.Months.FirstOrDefault(_ => _.Id == details.Month).Name,
        //                    SalesTarget = details.SalesTarget,
        //                    SaudaTarget = details.SaudaTarget,
        //                };
        //                DetailsList.Add(pjpDetails);
        //            }
        //            usersalestargetDto.UserSalesSaudaTargetDetail = DetailsList;
        //        }
        //        resultDto.IsSuccess = true;
        //        resultDto.SuccessDto.Response = usersalestargetDto;
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
        //public ResultDto GetUserSalesSaudaTargetDetailList(UserSalesSaudaTargetDto userSalesSaudaTargetDto)
        //{
        //    _methodName = "GetUserSalesSaudaTargetDetailList";
        //    var resultDto = new ResultDto();
        //    var userSalesSaudaTargetDetailDto = new List<UserSalesSaudaTargetDetailDto>();
        //    try
        //    {

        //        var targetlistContext = _emamiContext.UserSalesSaudaTarget.AsNoTracking().Where(_ => _.AssignedToId == userSalesSaudaTargetDto.UserId && _.Year == userSalesSaudaTargetDto.FinancialYearId).ToList();

        //        var DetailsList = new List<UserSalesSaudaTargetDetailDto>();
        //        if (targetlistContext.Any())
        //        {
        //            foreach (var details in targetlistContext)
        //            {
        //                var pjpDetails = new UserSalesSaudaTargetDetailDto
        //                {
        //                    MonthId = details.Month,
        //                    Month = _emamiContext.Months.FirstOrDefault(_ => _.Id == details.Month).Name,
        //                    SalesTarget = details.SalesTarget,
        //                    SaudaTarget = details.SaudaTarget,
        //                };
        //                DetailsList.Add(pjpDetails);
        //            }
        //            userSalesSaudaTargetDetailDto = DetailsList;
        //        }

        //        resultDto.IsSuccess = true;
        //        resultDto.SuccessDto.Response = userSalesSaudaTargetDetailDto;
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
        //public ResultDto GetUserSalesSaudaTargetList()
        //{
        //    _methodName = "GetUserSalesSaudaTargetList";
        //    var resultDto = new ResultDto();
        //    var userSalesSaudaTargetDto = new List<UserSalesSaudaTargetDto>();
        //    try
        //    {
        //        var userList = _emamiContext.UserSalesSaudaTarget.GroupBy(x => new { x.AssignedToId, x.Year }).Select(x => new
        //        {
        //            x.Key.AssignedToId,
        //            userid = x.Key.AssignedToId,
        //            financialyearid = x.Key.Year,
        //            user = _emamiContext.Users.FirstOrDefault(_ => _.Id == x.Key.AssignedToId).Name,
        //            financialyear = _emamiContext.FinancialYears.FirstOrDefault(_ => _.Id == x.Key.Year).Year,
        //        }).ToList();

        //        foreach (var list in userList)
        //        {
        //            var orderStatisticsDto = new UserSalesSaudaTargetDto
        //            {
        //                FinancialYearId = list.financialyearid,
        //                UserId = list.userid,
        //                FinancialYear = list.financialyear,
        //                User = list.user
        //            };
        //            userSalesSaudaTargetDto.Add(orderStatisticsDto);
        //        }

        //        resultDto.IsSuccess = true;
        //        resultDto.SuccessDto.Response = userSalesSaudaTargetDto;
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
        #endregion

        #region Financial Year
        public ResultDto GetMonthAndYearByFinancialYear(FinancialYearIdDto financialYearIdDto)
        {
            _methodName = "GetMonthAndYearByFinancialYear";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var financialYearDto = new FinancialYearDto();
            var MonthList = new List<UserTargetDetailDto>();
            try
            {
                var financialYearContext = _emamiContext.FinancialYears.AsNoTracking().FirstOrDefault(_ => _.Id == financialYearIdDto.FinancialYearid);
                if (financialYearContext != null)
                {
                    var dtStart = financialYearContext.EffectiveFrom;
                    var dtEnd = financialYearContext.EffectiveTo;
                    var monthSlist = _emamiContext.Months.AsNoTracking().ToList();
                    for (DateTime dt = dtStart; dt <= dtEnd; dt = dt.AddMonths(1))
                    {
                        var userSales = new UserTargetDetailDto
                        {
                            MonthId = monthSlist.FirstOrDefault(_ => _.Name == dt.ToString("MMMM")).Id,
                            Month = dt.ToString("MMMM"),
                            Year = dt.Year,
                            MonthAndYear = dt.ToString("MMMM yyyy"),
                            SalesTarget = 0,
                            SaudaTarget = 0,
                        };
                        MonthList.Add(userSales);
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = MonthList;
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

        //ToDo:  User Oiltype Target

        //#region User Oiltype Target


        //public ResultDto GetOilTypeTargetDetailList(UserOilTypeTargetIdDto inputDto)
        //{
        //    _methodName = "GetOiltypeTargetDetailList";
        //    _logger.Info($"{ServiceName} Controller-Method {_methodName}");
        //    var resultDto = new ResultDto();
        //    var resultOilTypeDto = new List<UserOiltypeTargetDetailDto>();
        //    try
        //    {

        //        var resultContext = _emamiContext.UserOilTypeTarget.AsNoTracking()
        //            .Where(_ => _.AssignedToId == inputDto.AssignedToUserId && _.FinancialYearId == inputDto.FinancialYearId).ToList();

        //        if (resultContext != null && resultContext.Any())
        //        {
        //            resultOilTypeDto = resultContext.Select(_ => new UserOiltypeTargetDetailDto
        //            {
        //                MonthId = _.MonthId,
        //                Month = _.Month?.Name,
        //                SalesTarget = _.Target
        //            }).ToList();
        //        }

        //        resultDto.IsSuccess = true;
        //        resultDto.SuccessDto.Response = resultOilTypeDto;
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

        //public ResultDto GetOilTypeTargetList()
        //{
        //    _methodName = "GetUserSalesSaudaTargetList";
        //    _logger.Info($"{ServiceName} Controller-Method {_methodName}");
        //    var resultDto = new ResultDto();
        //    var userSalesSaudaTargetDto = new List<UserOilTypeTargetDto>();
        //    try
        //    {
        //        userSalesSaudaTargetDto = _emamiContext.UserOilTypeTarget.AsNoTracking()
        //            .GroupBy(_ => new { _.AssignedToId, _.FinancialYearId }).ToList()
        //            .Select(_ => new UserOilTypeTargetDto
        //            {
        //                FinancialYearId = _.FirstOrDefault().FinancialYearId,
        //                AssignedFromUserId = _.FirstOrDefault()?.AssignedFromId,
        //                AssignedToUserId = _.FirstOrDefault().AssignedToId,
        //                AssignedToUser = _.FirstOrDefault()?.AssignedTo?.Name,
        //                FinancialYear = _.FirstOrDefault().FinancialYear.Year
        //            }).ToList();

        //        resultDto.IsSuccess = true;
        //        resultDto.SuccessDto.Response = userSalesSaudaTargetDto;
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

        //public ResultDto AddUserOilTypeTarget(UserOilTypeTargetDto inputDto)
        //{
        //    _methodName = "AddUserOilTypeTarget";
        //    _logger.Info($"{ServiceName} Controller-Method {_methodName}");
        //    var resultDto = new ResultDto();
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        if (inputDto.LoginUserId == 0)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        if (!inputDto.UserOiltypeTargetDetail.Any())
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.OiltypeTargetDetailsEmpty;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.OiltypeTargetDetailsEmpty, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
        //        if (userContext == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
        //            resultDto.ErrorDto.Message = Constants.UserNotFound;
        //            return resultDto;
        //        }

        //        var userOilTypeTargetContext = _emamiContext.UserOilTypeTarget.AsNoTracking().FirstOrDefault(_ => _.AssignedToId == inputDto.AssignedToUserId
        //        && _.FinancialYearId == inputDto.FinancialYearId);
        //        if (userOilTypeTargetContext != null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.RecordAlreadyExist;
        //            resultDto.ErrorDto.Message = Constants.RecordAlreadyExist;
        //            return resultDto;
        //        }

        //        foreach (var detail in inputDto.UserOiltypeTargetDetail)
        //        {
        //            var detailContext = new UserOilTypeTarget
        //            {
        //                AssignedFromId = inputDto.LoginUserId,
        //                AssignedToId = inputDto.AssignedToUserId,
        //                MonthId = detail.MonthId,
        //                FinancialYearId = inputDto.FinancialYearId,
        //                Target = detail.SalesTarget,
        //                Year = detail.Year,
        //                VerticalId = inputDto.VerticalId,
        //                OilTypeId = inputDto.OilTypeId,
        //                CreatedBy = inputDto.LoginUserId,
        //                CreatedDate = DateTime.UtcNow,
        //            };
        //            _emamiContext.UserOilTypeTarget.Add(detailContext);
        //            _emamiContext.SaveChanges();
        //        }
        //        resultDto.IsSuccess = true;
        //        resultDto.SuccessDto.Response = inputDto;
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

        //public ResultDto UpdateUserOilTypeTarget(UserOilTypeTargetDto inputDto)
        //{
        //    _methodName = "UpdateUserSalesSaudaTarget";
        //    _logger.Info($"{ServiceName} Controller-Method {_methodName}");
        //    var resultDto = new ResultDto();
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        if (inputDto.LoginUserId == 0)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        if (!inputDto.UserOiltypeTargetDetail.Any())
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.PJPDetailsEmpty;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.PJPDetailsEmpty, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
        //        if (userContext == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
        //            return resultDto;
        //        }


        //        foreach (var details in inputDto.UserOiltypeTargetDetail)
        //        {
        //            var recordsContext = _emamiContext.UserOilTypeTarget.FirstOrDefault(_ => _.Id == details.Id);
        //            recordsContext.Target = details.SalesTarget;
        //            recordsContext.ModifiedBy = inputDto.LoginUserId;
        //            recordsContext.ModifiedDate = DateTime.UtcNow;
        //            _emamiContext.SaveChanges();
        //        }

        //        resultDto.IsSuccess = true;
        //        resultDto.SuccessDto.Response = inputDto;
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

        //public ResultDto GetUserOilTypeTargetDetailsById(UserOilTypeTargetIdDto inputDto)
        //{
        //    _methodName = "GetUserOilTypeTargetDetailsById";
        //    _logger.Info($"{ServiceName} Controller-Method {_methodName}");
        //    var resultDto = new ResultDto();
        //    var outputDto = new UserOilTypeTargetDto();
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        if (inputDto.FinancialYearId == 0)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        var resultContext = _emamiContext.UserOilTypeTarget.AsNoTracking()
        //            .Where(_ => _.FinancialYearId == inputDto.FinancialYearId && _.AssignedToId == inputDto.AssignedToUserId).ToList();

        //        if (resultContext != null || resultContext.Any())
        //        {
        //            outputDto.Id = resultContext.FirstOrDefault().Id;
        //            outputDto.AssignedToUserId = resultContext.FirstOrDefault().AssignedToId;
        //            outputDto.FinancialYearId = resultContext.FirstOrDefault().FinancialYearId;
        //            outputDto.OilTypeId = resultContext.FirstOrDefault().OilTypeId;
        //            outputDto.VerticalId = resultContext.FirstOrDefault().VerticalId;

        //            outputDto.UserOiltypeTargetDetail = resultContext.Select(_ => new UserOiltypeTargetDetailDto
        //            {
        //                Id = _.Id,
        //                MonthId = _.MonthId,
        //                Month = _.Month.Name,
        //                MonthAndYear = _.Month.Name + " " + _.Year,
        //                SalesTarget = _.Target
        //            }).ToList();
        //        }
        //        else
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
        //            return resultDto;
        //        }

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

        //#endregion

        #region UserCustomerSalesTarget       

        public ResultDto GetUserCustomerSalesTargetDetailList(UserTargetIdDto inputDto)
        {
            _methodName = "GetUserCustomerSalesTargetDetailList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var resultOilTypeDto = new List<UserTargetDetailDto>();
            try
            {

                var resultContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking()
                    .Where(_ => _.AssignedToId == inputDto.AssignedToUserId && _.FinancialYearId == inputDto.FinancialYearId && _.OilTypeId == inputDto.OilTypeId).ToList();

                if (resultContext != null && resultContext.Any())
                {
                    resultOilTypeDto = resultContext.Select(_ => new UserTargetDetailDto
                    {
                        Id = _.Id,
                        MonthId = _.MonthId,
                        Month = _.Month?.Name,
                        SalesTarget = _.Target,
                        //SaudaTarget = _.SaudaTarget
                    }).ToList();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = resultOilTypeDto;
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

        public ResultDto GetUserCustomerSalesTargetList(LoginUserIdDto inputDto)
        {
            _methodName = "GetUserCustomerSalesTargetList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var userSalesSaudaTargetDto = new List<UserCustomerSalesTargetDto>();
            try
            {
                userSalesSaudaTargetDto = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedFromId == inputDto.LoginUserId
                && (inputDto.VerticalId > 0 ? _.DivisionId == inputDto.VerticalId : _.DivisionId > 0))
                    .GroupBy(_ => new { _.AssignedToId, _.FinancialYearId }).ToList()
                    .Select(_ => new UserCustomerSalesTargetDto
                    {
                        FinancialYearId = _.FirstOrDefault().FinancialYearId,
                        AssignedFromId = _.FirstOrDefault()?.AssignedFromId,
                        AssignedToId = _.FirstOrDefault().AssignedToId,
                        AssignedToUser = _.FirstOrDefault()?.AssignedTo?.Name,
                        FinancialYear = _.FirstOrDefault().FinancialYear.Year
                    }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = userSalesSaudaTargetDto;
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

        public ResultDto AddUserCustomerSalesTarget(UserCustomerSalesTargetDto inputDto)
        {
            _methodName = "AddUserCustomerSalesTarget";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
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
                if (!inputDto.UserTargetDetail.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UCSTargetDetailsEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UCSTargetDetailsEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }

                var UserCustomerSalesTargetContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().FirstOrDefault(_ => _.AssignedToId == inputDto.AssignedToId
               && _.FinancialYearId == inputDto.FinancialYearId);
                if (UserCustomerSalesTargetContext != null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordAlreadyExist;
                    resultDto.ErrorDto.Message = Constants.RecordAlreadyExist;
                    return resultDto;
                }

                foreach (var detail in inputDto.UserTargetDetail)
                {
                    var detailContext = new UserCustomerSalesTarget
                    {
                        AssignedFromId = inputDto.LoginUserId,
                        AssignedToId = inputDto.AssignedToId,
                        MonthId = Convert.ToInt32(detail.MonthId),
                        FinancialYearId = inputDto.FinancialYearId,
                        Target = detail.SalesTarget,
                        Year = detail.Year,
                        DivisionId = inputDto.VerticalId,
                        OilTypeId = inputDto.OilTypeId,
                        SalesOrganizationId=inputDto.SalesOrganizationId,
                        DistributionChannelId=inputDto.DistributionChannelId,
                        //OilTypeId = inputDto.DealerId,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    };
                    _emamiContext.UserCustomerSalesTarget.Add(detailContext);
                    _emamiContext.SaveChanges();
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = inputDto;
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

        public ResultDto UpdateUserCustomerSalesTarget(UserCustomerSalesTargetDto inputDto)
        {
            _methodName = "UpdateUserCustomerSalesTarget";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
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
                if (!inputDto.UserTargetDetail.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.PJPDetailsEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.PJPDetailsEmpty, Constants.EnglishLanguage);
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

                foreach (var details in inputDto.UserTargetDetail)
                {
                    var recordsContext = _emamiContext.UserCustomerSalesTarget.FirstOrDefault(_ => _.Id == details.Id);
                    recordsContext.Target = details.SalesTarget;
                    recordsContext.ModifiedBy = inputDto.LoginUserId;
                    recordsContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    _emamiContext.SaveChanges();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = inputDto;
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

        public ResultDto GetUserCustomerSalesTargetDetailsById(UserTargetIdDto inputDto)
        {
            _methodName = "GetUserCustomerSalesTargetDetailsById";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var outputDto = new UserCustomerSalesTargetDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (inputDto.FinancialYearId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                var resultContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking()
                    .Where(_ => _.FinancialYearId == inputDto.FinancialYearId && _.AssignedToId == inputDto.AssignedToUserId && _.OilTypeId == inputDto.OilTypeId).ToList();

                if (resultContext != null || resultContext.Any())
                {
                    outputDto.Id = resultContext.FirstOrDefault().Id;
                    outputDto.AssignedToId = resultContext.FirstOrDefault().AssignedToId;
                    outputDto.FinancialYearId = resultContext.FirstOrDefault().FinancialYearId;
                    outputDto.OilTypeId = resultContext.FirstOrDefault().OilTypeId;
                    outputDto.VerticalId = resultContext.FirstOrDefault()?.DivisionId;
                    outputDto.SalesOrganizationId = resultContext.FirstOrDefault().SalesOrganizationId!=null ? (long)resultContext.FirstOrDefault().SalesOrganizationId : 0;
                    outputDto.DistributionChannelId = resultContext.FirstOrDefault().DistributionChannelId != null ? (long)resultContext.FirstOrDefault().DistributionChannelId: 0;
                    outputDto.UserTargetDetail = resultContext.Select(_ => new UserTargetDetailDto
                    {
                        Id = _.Id,
                        MonthId = _.MonthId,
                        Month = _.Month.Name,
                        MonthAndYear = _.Month.Name + " " + _.Year,
                        SalesTarget = _.Target,
                    }).ToList();
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
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

        public ResultDto SaveUserCustomerSalesTargetList(List<MapSalesTargetDetailDto> targetDetailDtoList)
        {
            _methodName = "SaveUserCustomerSalesTargetList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            try
            {
                if (targetDetailDtoList == null || !targetDetailDtoList.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (targetDetailDtoList.FirstOrDefault().LoginUserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }
                var loginUserId = targetDetailDtoList.FirstOrDefault().LoginUserId;
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == loginUserId);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }
                //var oilTypes = targetDetailDtoList.GroupBy(_ => _.OilTypeId).Select(_ => new
                //{
                //    _.FirstOrDefault().OilTypeId,
                //    _.FirstOrDefault().AssignedFromId,
                //    _.FirstOrDefault().AssignedToId,
                //    _.FirstOrDefault().FinancialYearId,
                //    _.FirstOrDefault().OilType,
                //    _.FirstOrDefault().MonthId,
                //    _.FirstOrDefault().VerticalId
                //}).ToList();

                //var errorMessage = string.Empty;
                //var oilTypeList = targetDetailDtoList.Select(_ => new { _.OilTypeId, _.AssignedFromId, _.AssignedToId, _.FinancialYearId, _.OilType, _.MonthId, _.VerticalId }).Distinct().ToList();
                //foreach (var item in oilTypes)
                //{
                //    var salesTargetContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking()
                //            .FirstOrDefault(_ => _.AssignedToId == item.AssignedToId
                //            && _.FinancialYearId == item.FinancialYearId
                //            && _.OilTypeId == item.OilTypeId
                //             && _.VerticalId == item.VerticalId
                //            && _.MonthId == item.MonthId);
                //    if (salesTargetContext != null)
                //    {
                //        var message = "" + item.OilType + ". ";
                //        errorMessage = Constants.BindErrorMessage(message + " - ", errorMessage);
                //        var removed = targetDetailDtoList.RemoveAll(_ => _.OilTypeId == item.OilTypeId && _.MonthId == item.MonthId);
                //    }
                //}
                //if (!string.IsNullOrEmpty(errorMessage))
                //{
                //    var existMessage = "Record Exists for the following OilTypes: ";
                //    errorMessage = existMessage + errorMessage;
                //}

                foreach (var inputDto in targetDetailDtoList)
                {
                    var salesTargetContext = _emamiContext.UserCustomerSalesTarget
                            .FirstOrDefault(_ => _.AssignedFromId == inputDto.LoginUserId && _.AssignedToId == inputDto.AssignedToId
                            && _.FinancialYearId == inputDto.FinancialYearId
                            && _.OilTypeId == inputDto.OilTypeId
                             && _.DivisionId == inputDto.VerticalId
                             && _.SalesOrganizationId==inputDto.SalesOrganizationId
                             && _.DistributionChannelId==inputDto.DistributionChannelId
                            && _.MonthId == inputDto.MonthId);
                    if (salesTargetContext != null)
                    {
                        salesTargetContext.Target = inputDto.Target;
                    }
                    else
                    {
                        var detailContext = new UserCustomerSalesTarget
                        {
                            AssignedFromId = inputDto.LoginUserId,
                            AssignedToId = inputDto.AssignedToId,
                            MonthId = Convert.ToInt32(inputDto.MonthId),
                            FinancialYearId = inputDto.FinancialYearId,
                            Target = inputDto.Target,
                            Year = inputDto.Year,
                            DivisionId = inputDto.VerticalId,
                            OilTypeId = inputDto.OilTypeId,
                            CreatedBy = inputDto.LoginUserId,
                            SalesOrganizationId=inputDto.SalesOrganizationId,
                            DistributionChannelId=inputDto.DistributionChannelId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.UserCustomerSalesTarget.Add(detailContext);
                    }
                    _emamiContext.SaveChanges();
                }
                var outputDto = new UserCustomerSalesTargetDto();
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

        public ResultDto GetAssignedSalesTargetList(LoginUserIdDto inputDto)
        {
            _methodName = "GetAssignedSalesTargetList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var userSalesSaudaTargetDto = new List<UserCustomerSalesTargetDto>();
            try
            {
                userSalesSaudaTargetDto = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId
                && (inputDto.VerticalId > 0 ? _.DivisionId == inputDto.VerticalId : _.DivisionId > 0))
                    .GroupBy(_ => new { _.AssignedToId, _.FinancialYearId }).ToList()
                    .Select(_ => new UserCustomerSalesTargetDto
                    {
                        FinancialYearId = _.FirstOrDefault().FinancialYearId,
                        AssignedFromId = _.FirstOrDefault()?.AssignedFromId,
                        AssignedToId = _.FirstOrDefault().AssignedToId,
                        AssignedToUser = _.FirstOrDefault()?.AssignedTo?.Name,
                        FinancialYear = _.FirstOrDefault().FinancialYear.Year
                    }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = userSalesSaudaTargetDto;
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

        public ResultDto GetSalesTargetOilTypeList(UserTargetIdDto inputDto)
        {
            _methodName = "GetSalesTargetOilTypeList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var resultOilTypeDto = new List<UserCustomerSalesTargetDto>();
            try
            {
                var salesOrganization = _emamiContext.SalesOrganization.ToList();
                var distributionChannel = _emamiContext.DistributionChannel.ToList();
                var resultContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking()
                    .Where(_ => _.AssignedToId == inputDto.AssignedToUserId && _.FinancialYearId == inputDto.FinancialYearId)
                    .GroupBy(_ => new { _.AssignedToId, _.FinancialYearId, _.OilTypeId }).ToList();

                if (resultContext != null && resultContext.Any())
                {
                    resultOilTypeDto = resultContext.AsEnumerable().Select(_ => new UserCustomerSalesTargetDto
                    {
                        EncryptedId = UtilityHelper.ConvertToMd5(_.FirstOrDefault().AssignedToId.ToString(), SecurityConstants.EncryptionKey),
                        Id = _.FirstOrDefault().Id,
                        VerticalId = _.FirstOrDefault().DivisionId,
                        Vertical = _.FirstOrDefault().Division?.Name,
                        OilTypeId = _.FirstOrDefault().OilTypeId,
                        SalesOrganization = _.FirstOrDefault().SalesOrganizationId != null ? salesOrganization.FirstOrDefault(s => s.Id== _.FirstOrDefault().SalesOrganizationId).Name :String.Empty,
                        DistributionChannel = _.FirstOrDefault().DistributionChannelId != null ? distributionChannel.FirstOrDefault(s => s.Id== _.FirstOrDefault().DistributionChannelId).Name :String.Empty,
                        SalesOrganizationId = _.FirstOrDefault().SalesOrganizationId != null ? (long)_.FirstOrDefault().SalesOrganizationId : 0,
                        DistributionChannelId = _.FirstOrDefault().DistributionChannelId != null ? (long)_.FirstOrDefault().DistributionChannelId : 0,
                        OilType = _.FirstOrDefault().OilType != null ? _.FirstOrDefault().OilType.Name + "-" + _.FirstOrDefault().OilType.SalesOrganization.Code + "/" + _.FirstOrDefault().OilType.DistributionChannel.Code + "/" + _.FirstOrDefault().OilType.Division.Code : String.Empty,
                        //OilTypeCode = _.FirstOrDefault().OilType?.SAPCode,
                        FinancialYearId = _.FirstOrDefault().FinancialYearId,
                        AssignedFromId = _.FirstOrDefault()?.AssignedFromId,
                        AssignedToId = _.FirstOrDefault().AssignedToId,
                    }).ToList();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = resultOilTypeDto;
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

        public ResultDto GetOilTypesBasedOnAssignedSalesTarget(UserTargetIdDto inputDto)
        {
            _methodName = "GetOilTypesBasedOnAssignedSalesTarget";
            var resultDto = new ResultDto();
            var oiltypeList = new List<DropDownDto>();
            try
            {
                if (inputDto.RoleTypeId == (int)DTO.Enums.RoleType.NationalTrader
                    || inputDto.RoleTypeId == (int)DTO.Enums.RoleType.Admin
                    || inputDto.RoleTypeId == (int)DTO.Enums.RoleType.BusinessFinanceAdmin)
                {
                    if (inputDto.IsToReturnInactiveData)
                    {
                        oiltypeList = _emamiContext.OilTypes.AsNoTracking().Where(_ => _.DivisionId == inputDto.VerticalId)
                        .Select(s => new DropDownDto() { Id = s.Id, Name = s.Name + "-" + s.SalesOrganization.Code + "/" + s.DistributionChannel.Code + "/" + s.Division.Code }).ToList();
                    }
                    else
                    {
                        oiltypeList = _emamiContext.OilTypes.AsNoTracking().Where(_ => _.DivisionId == inputDto.VerticalId && _.IsActive)
                        .Select(s => new DropDownDto() { Id = s.Id, Name = s.Name + "-" + s.SalesOrganization.Code + "/" + s.DistributionChannel.Code + "/" + s.Division.Code }).ToList();
                    }
                }
                else
                {
                    List<long> oilTypeIds = _emamiContext.UserCustomerSalesTarget.AsNoTracking()
                   .Where(_ => _.AssignedToId == inputDto.AssignedToUserId && _.FinancialYearId == inputDto.FinancialYearId)
                   .GroupBy(_ => new { _.AssignedToId, _.FinancialYearId, _.OilTypeId }).Select(_ => _.FirstOrDefault().OilTypeId).ToList();

                    if (inputDto.IsToReturnInactiveData)
                    {
                        oiltypeList = _emamiContext.OilTypes.AsNoTracking().Where(_ => oilTypeIds.Contains(_.Id) && _.DivisionId == inputDto.VerticalId)
                        .Select(s => new DropDownDto() { Id = s.Id, Name =  s.Name + "-" + s.SalesOrganization.Code + "/" + s.DistributionChannel.Code + "/" + s.Division.Code }).ToList();
                    }
                    else
                    {
                        oiltypeList = _emamiContext.OilTypes.AsNoTracking().Where(_ => oilTypeIds.Contains(_.Id) && _.DivisionId == inputDto.VerticalId && _.IsActive)
                        .Select(s => new DropDownDto() { Id = s.Id, Name = s.Name + "-" + s.SalesOrganization.Code + "/" + s.DistributionChannel.Code + "/" + s.Division.Code }).ToList();
                    }
                }

                resultDto.SuccessDto.Response = oiltypeList;
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

        #region UserCustomerSaudaTarget       

        public ResultDto GetUserCustomerSaudaTargetDetailList(UserTargetIdDto inputDto)
        {
            _methodName = "GetUserCustomerSaudaTargetDetailList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var resultOilTypeDto = new List<UserTargetDetailDto>();
            try
            {

                var resultContext = _emamiContext.UserCustomerSaudaTarget.AsNoTracking()
                    .Where(_ => _.AssignedToId == inputDto.AssignedToUserId && _.FinancialYearId == inputDto.FinancialYearId && _.OilTypeId == inputDto.OilTypeId).ToList();

                if (resultContext != null && resultContext.Any())
                {
                    resultOilTypeDto = resultContext.Select(_ => new UserTargetDetailDto
                    {
                        Id = _.Id,
                        MonthId = _.MonthId,
                        Month = _.Month?.Name,
                        SaudaTarget = _.Target,
                        //SaudaTarget = _.SaudaTarget
                    }).ToList();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = resultOilTypeDto;
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

        public ResultDto GetUserCustomerSaudaTargetList(LoginUserIdDto inputDto)
        {
            _methodName = "GetUserCustomerSaudaTargetList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var userCustomerSaudaSaudaTargetDto = new List<UserCustomerSaudaTargetDto>();
            try
            {
                userCustomerSaudaSaudaTargetDto = _emamiContext.UserCustomerSaudaTarget.AsNoTracking().Where(_ => _.AssignedFromId == inputDto.LoginUserId
                && (inputDto.VerticalId > 0 ? _.DivisionId == inputDto.VerticalId : _.DivisionId > 0))
                    .GroupBy(_ => new { _.AssignedToId, _.FinancialYearId }).ToList()
                    .Select(_ => new UserCustomerSaudaTargetDto
                    {
                        FinancialYearId = _.FirstOrDefault().FinancialYearId,
                        AssignedFromId = _.FirstOrDefault()?.AssignedFromId,
                        AssignedToId = _.FirstOrDefault().AssignedToId,
                        AssignedToUser = _.FirstOrDefault()?.AssignedTo?.Name,
                        FinancialYear = _.FirstOrDefault().FinancialYear.Year
                    }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = userCustomerSaudaSaudaTargetDto;
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

        public ResultDto AddUserCustomerSaudaTarget(UserCustomerSaudaTargetDto inputDto)
        {
            _methodName = "AddUserCustomerSaudaTarget";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
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
                if (!inputDto.UserTargetDetail.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UCSTargetDetailsEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UCSTargetDetailsEmpty, Constants.EnglishLanguage);
                    return resultDto;
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }

                var UserCustomerSaudaTargetContext = _emamiContext.UserCustomerSaudaTarget.AsNoTracking().FirstOrDefault(_ => _.AssignedToId == inputDto.AssignedToId
                && _.FinancialYearId == inputDto.FinancialYearId);
                if (UserCustomerSaudaTargetContext != null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordAlreadyExist;
                    resultDto.ErrorDto.Message = Constants.RecordAlreadyExist;
                    return resultDto;
                }

                foreach (var detail in inputDto.UserTargetDetail)
                {
                    var detailContext = new UserCustomerSaudaTarget
                    {
                        AssignedFromId = inputDto.LoginUserId,
                        AssignedToId = inputDto.AssignedToId,
                        MonthId = Convert.ToInt32(detail.MonthId),
                        FinancialYearId = inputDto.FinancialYearId,
                        Target = detail.SaudaTarget,
                        Year = detail.Year,
                        DivisionId = inputDto.VerticalId,
                        OilTypeId = inputDto.OilTypeId,
                        SalesOrganizationId=inputDto.SalesOrganizationId,
                        DistributionChannelId=inputDto.DistributionChannelId,
                        //OilTypeId = inputDto.DealerId,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    };
                    _emamiContext.UserCustomerSaudaTarget.Add(detailContext);
                    _emamiContext.SaveChanges();
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = inputDto;
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

        public ResultDto UpdateUserCustomerSaudaTarget(UserCustomerSaudaTargetDto inputDto)
        {
            _methodName = "UpdateUserCustomerSaudaTarget";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
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
                if (!inputDto.UserTargetDetail.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.PJPDetailsEmpty;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.PJPDetailsEmpty, Constants.EnglishLanguage);
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

                foreach (var details in inputDto.UserTargetDetail)
                {
                    var recordsContext = _emamiContext.UserCustomerSaudaTarget.FirstOrDefault(_ => _.Id == details.Id);
                    recordsContext.Target = details.SaudaTarget;
                    recordsContext.ModifiedBy = inputDto.LoginUserId;
                    recordsContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    _emamiContext.SaveChanges();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = inputDto;
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

        public ResultDto GetUserCustomerSaudaTargetDetailsById(UserTargetIdDto inputDto)
        {
            _methodName = "GetUserCustomerSaudaTargetDetailsById";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var outputDto = new UserCustomerSaudaTargetDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (inputDto.FinancialYearId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                var resultContext = _emamiContext.UserCustomerSaudaTarget.AsNoTracking()
                    .Where(_ => _.FinancialYearId == inputDto.FinancialYearId && _.AssignedToId == inputDto.AssignedToUserId && _.OilTypeId == inputDto.OilTypeId).ToList();

                if (resultContext != null || resultContext.Any())
                {
                    outputDto.Id = resultContext.FirstOrDefault().Id;
                    outputDto.AssignedToId = resultContext.FirstOrDefault().AssignedToId;
                    outputDto.FinancialYearId = resultContext.FirstOrDefault().FinancialYearId;
                    outputDto.OilTypeId = resultContext.FirstOrDefault().OilTypeId;
                    outputDto.VerticalId = resultContext.FirstOrDefault()?.DivisionId;
                    outputDto.SalesOrganizationId = resultContext.FirstOrDefault().SalesOrganizationId !=null ? resultContext.FirstOrDefault().SalesOrganizationId : 0;
                    outputDto.DistributionChannelId = resultContext.FirstOrDefault().DistributionChannelId!=null ? resultContext.FirstOrDefault().DistributionChannelId : 0;
                    outputDto.UserTargetDetail = resultContext.Select(_ => new UserTargetDetailDto
                    {
                        Id = _.Id,
                        MonthId = _.MonthId,
                        Month = _.Month.Name,
                        MonthAndYear = _.Month.Name + " " + _.Year,
                        SaudaTarget = _.Target,
                    }).ToList();
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
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

        public ResultDto SaveUserCustomerSaudaTargetList(List<MapSaudaTargetDetailDto> targetDetailDtoList)
        {
            _methodName = "SaveUserCustomerSaudaTargetList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            try
            {
                if (targetDetailDtoList == null || !targetDetailDtoList.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (targetDetailDtoList.FirstOrDefault().LoginUserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }
                var loginUserId = targetDetailDtoList.FirstOrDefault().LoginUserId;
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == loginUserId);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }
                //var oilTypes = targetDetailDtoList.GroupBy(_ => _.OilTypeId).Select(_ => new
                //{
                //    _.FirstOrDefault().OilTypeId,
                //    _.FirstOrDefault().AssignedFromId,
                //    _.FirstOrDefault().AssignedToId,
                //    _.FirstOrDefault().FinancialYearId,
                //    _.FirstOrDefault().OilType,
                //    _.FirstOrDefault().MonthId,
                //    _.FirstOrDefault().VerticalId
                //}).ToList();

                //var errorMessage = string.Empty;
                //var oilTypeList = targetDetailDtoList.Select(_ => new { _.OilTypeId, _.AssignedFromId, _.AssignedToId, _.FinancialYearId, _.OilType, _.MonthId, _.VerticalId }).Distinct().ToList();
                //foreach (var item in oilTypes)
                //{
                //    var CustomerSaudaTargetContext = _emamiContext.UserCustomerSaudaTarget.AsNoTracking()
                //            .FirstOrDefault(_ => _.AssignedToId == item.AssignedToId
                //            && _.FinancialYearId == item.FinancialYearId
                //            && _.OilTypeId == item.OilTypeId
                //            && _.VerticalId == item.VerticalId
                //            && _.MonthId == item.MonthId);
                //    if (CustomerSaudaTargetContext != null)
                //    {
                //        var message = "" + item.OilType + ". ";
                //        errorMessage = Constants.BindErrorMessage(message + " - ", errorMessage);
                //        var removed = targetDetailDtoList.RemoveAll(_ => _.OilTypeId == item.OilTypeId && _.MonthId == item.MonthId);
                //    }
                //}

                //if (!string.IsNullOrEmpty(errorMessage))
                //{
                //    var existMessage = "Record Exists for the following OilTypes: ";
                //    errorMessage = existMessage + errorMessage;
                //}

                foreach (var inputDto in targetDetailDtoList)
                {
                    var CustomerSaudaTargetContext = _emamiContext.UserCustomerSaudaTarget
                            .FirstOrDefault(_ => _.AssignedFromId == inputDto.AssignedFromId && _.AssignedToId == inputDto.AssignedToId
                            && _.FinancialYearId == inputDto.FinancialYearId
                            && _.OilTypeId == inputDto.OilTypeId
                            && _.DivisionId == inputDto.VerticalId
                            && _.MonthId == inputDto.MonthId);
                    if (CustomerSaudaTargetContext != null)
                    {
                        CustomerSaudaTargetContext.Target = inputDto.Target;
                    }
                    else
                    {
                        var detailContext = new UserCustomerSaudaTarget
                        {
                            AssignedFromId = inputDto.LoginUserId,
                            AssignedToId = inputDto.AssignedToId,
                            MonthId = Convert.ToInt32(inputDto.MonthId),
                            FinancialYearId = inputDto.FinancialYearId,
                            Target = inputDto.Target,
                            Year = inputDto.Year,
                            DivisionId = inputDto.VerticalId,
                            SalesOrganizationId=inputDto.SalesOrganizationId,
                            DistributionChannelId=inputDto.DistributionChannelId,
                            OilTypeId = inputDto.OilTypeId,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        };
                        _emamiContext.UserCustomerSaudaTarget.Add(detailContext);
                    }
                    _emamiContext.SaveChanges();
                }
                var outputDto = new UserCustomerSaudaTargetDto();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Message = "Created Successfully";
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

        public ResultDto GetAssignedSaudaTargetList(LoginUserIdDto inputDto)
        {
            _methodName = "GetAssignedSaudaTargetList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var userCustomerSaudaSaudaTargetDto = new List<UserCustomerSaudaTargetDto>();
            try
            {
                userCustomerSaudaSaudaTargetDto = _emamiContext.UserCustomerSaudaTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId
                && (inputDto.VerticalId > 0 ? _.DivisionId == inputDto.VerticalId : _.DivisionId > 0))
                    .GroupBy(_ => new { _.AssignedToId, _.FinancialYearId }).ToList()
                    .Select(_ => new UserCustomerSaudaTargetDto
                    {
                        FinancialYearId = _.FirstOrDefault().FinancialYearId,
                        AssignedFromId = _.FirstOrDefault()?.AssignedFromId,
                        AssignedToId = _.FirstOrDefault().AssignedToId,
                        AssignedToUser = _.FirstOrDefault()?.AssignedTo?.Name,
                        FinancialYear = _.FirstOrDefault().FinancialYear.Year
                    }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = userCustomerSaudaSaudaTargetDto;
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

        public ResultDto GetSaudaTargetOilTypeList(UserTargetIdDto inputDto)
        {
            _methodName = "GetCustomerSaudaTargetOilTypeList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var resultOilTypeDto = new List<UserCustomerSaudaTargetDto>();
            try
            {
                var salesOrganization = _emamiContext.SalesOrganization.ToList();
                var distributionChannel = _emamiContext.DistributionChannel.ToList();
                var resultContext = _emamiContext.UserCustomerSaudaTarget.AsNoTracking()
                    .Where(_ => _.AssignedToId == inputDto.AssignedToUserId && _.FinancialYearId == inputDto.FinancialYearId)
                    .GroupBy(_ => new { _.AssignedToId, _.FinancialYearId, _.OilTypeId }).ToList();

                if (resultContext != null && resultContext.Any())
                {
                    resultOilTypeDto = resultContext.Select(_ => new UserCustomerSaudaTargetDto
                    {
                        EncryptedId = UtilityHelper.ConvertToMd5(_.FirstOrDefault().AssignedToId.ToString(), SecurityConstants.EncryptionKey),
                        Id = _.FirstOrDefault().Id,
                        VerticalId = _.FirstOrDefault().DivisionId,
                        Vertical = _.FirstOrDefault().Division?.Name,
                        SalesOrganization = _.FirstOrDefault().SalesOrganizationId != null ?salesOrganization.FirstOrDefault(s => s.Id== _.FirstOrDefault().SalesOrganizationId).Name : String.Empty,
                        DistributionChannel=_.FirstOrDefault().DistributionChannelId !=null ? distributionChannel.FirstOrDefault(s => s.Id==_.FirstOrDefault().DistributionChannelId).Name:String.Empty,
                        OilTypeId = _.FirstOrDefault().OilTypeId,
                        OilType = _.FirstOrDefault().OilType != null ? _.FirstOrDefault().OilType.Name + "-" + _.FirstOrDefault().OilType.SalesOrganization.Code + "/" + _.FirstOrDefault().OilType.DistributionChannel.Code + "/" + _.FirstOrDefault().OilType.Division.Code : String.Empty,
                        //OilTypeCode = _.FirstOrDefault().OilType?.SAPCode,
                        FinancialYearId = _.FirstOrDefault().FinancialYearId,
                        AssignedFromId = _.FirstOrDefault()?.AssignedFromId,
                        AssignedToId = _.FirstOrDefault().AssignedToId,
                    }).ToList();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = resultOilTypeDto;
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


        public ResultDto GetOilTypesBasedOnAssignedSaudaTarget(UserTargetIdDto inputDto)
        {
            _methodName = "GetOilTypesBasedOnAssignedSaudaTarget";
            var resultDto = new ResultDto();
            var oiltypeList = new List<DropDownDto>();

            try
            {
                if (inputDto.RoleTypeId == (int)DTO.Enums.RoleType.NationalTrader
                   || inputDto.RoleTypeId == (int)DTO.Enums.RoleType.Admin
                   || inputDto.RoleTypeId == (int)DTO.Enums.RoleType.BusinessFinanceAdmin)
                {
                    if (inputDto.IsToReturnInactiveData)
                    {
                        oiltypeList = _emamiContext.OilTypes.AsNoTracking().Where(_ => _.DivisionId == inputDto.VerticalId)
                        .Select(s => new DropDownDto() { Id = s.Id, Name = s.Name + "-"+s.SalesOrganization.Code+"/"+s.DistributionChannel.Code+"/"+s.Division.Code }).ToList();
                    }
                    else
                    {
                        oiltypeList = _emamiContext.OilTypes.AsNoTracking().Where(_ => _.DivisionId == inputDto.VerticalId && _.IsActive)
                        .Select(s => new DropDownDto() { Id = s.Id, Name =  s.Name + "-" + s.SalesOrganization.Code + "/" + s.DistributionChannel.Code + "/" + s.Division.Code }).ToList();
                    }
                }
                else
                {
                    List<long> oilTypeIds = _emamiContext.UserCustomerSaudaTarget.AsNoTracking()
                   .Where(_ => _.AssignedToId == inputDto.AssignedToUserId && _.FinancialYearId == inputDto.FinancialYearId)
                   .GroupBy(_ => new { _.AssignedToId, _.FinancialYearId, _.OilTypeId }).Select(_ => _.FirstOrDefault().OilTypeId).ToList();

                    if (inputDto.IsToReturnInactiveData)
                    {
                        oiltypeList = _emamiContext.OilTypes.AsNoTracking().Where(_ => oilTypeIds.Contains(_.Id) && _.DivisionId == inputDto.VerticalId)
                        .Select(s => new DropDownDto() { Id = s.Id, Name = s.Name + "-" + s.SalesOrganization.Code + "/" + s.DistributionChannel.Code + "/" + s.Division.Code }).ToList();
                    }
                    else
                    {
                        oiltypeList = _emamiContext.OilTypes.AsNoTracking().Where(_ => oilTypeIds.Contains(_.Id) && _.DivisionId == inputDto.VerticalId && _.IsActive)
                        .Select(s => new DropDownDto() { Id = s.Id, Name =s.Name + "-" + s.SalesOrganization.Code + "/" + s.DistributionChannel.Code + "/" + s.Division.Code }).ToList();
                    }
                }

                resultDto.SuccessDto.Response = oiltypeList;
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

        #region Monthly Tour Plan
        public ResultDto GetMTPDetailsForCurrentMonth(TodayActivitiesInputDto inputDto)
        {
            _methodName = "TodayActivities";
            var resultDto = new ResultDto();
            var mtpDateWiseResult = new List<MTPDateWiseDetailsOutputDto>();
            try
            {
                DateTime todayDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var monthlyTourPlanList = _emamiContext.MonthlyTourPlans
                    .Where(_ => _.MonthlyTourPlanStatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Approved && _.CreatedBy == inputDto.LoginUserId)
                    .Join(_emamiContext.MonthlyTourPlanDetails
                    .Where(_ => _.Date.Month == todayDate.Month && _.Date.Year == todayDate.Year), mtp => mtp.Id, mtpd => mtpd.MonthlyTourPlanId, (mtp, mtpd) => new { mtp, mtpd }).ToList();


                var dateList = monthlyTourPlanList.GroupBy(_ => new { _.mtpd.Date })
                    .Select(_ => new { Date = _.FirstOrDefault().mtpd.Date }).Select(_ => _.Date).ToList();

                var cityList = monthlyTourPlanList.GroupBy(_ => new { _.mtpd.Date })
                    .Select(_ => new MTPDateWiseCitiesDto
                    {
                        Date = _.FirstOrDefault().mtpd.Date,
                        TownId = _.FirstOrDefault().mtpd.TownId,
                        //Town = _.FirstOrDefault().mtpd.Town.CityName,
                    }).ToList();

                var dealerList = monthlyTourPlanList.GroupBy(_ => new { _.mtpd.Date,  _.mtpd.DealerId })
                    .Select(_ => new MTPDateWiseDealersDto
                    {
                        Date = _.FirstOrDefault().mtpd.Date,
                        TownId = _.FirstOrDefault().mtpd.TownId,
                        DealerId = _.FirstOrDefault().mtpd?.DealerId
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
                                    if (!string.IsNullOrEmpty(dealer.DealerId) && dealer.DealerId != "0")
                                    {
                                        var dealerId = long.Parse(dealer.DealerId);
                                        dealer.Dealer = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == dealerId)?.Name;
                                    }
                                }
                                cityDetailsDto.MTPDateWiseDealersDtos = dealers;
                                cityWiseDtos.Add(cityDetailsDto);
                            }
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
        #endregion

        #region Today Activity

        public ResultDto GetProspectiveDealers(SalesTourPlanParamDto inputDto)
        {
            _methodName = "GetProspectiveDealers";
            var resultDto = new ResultDto();
            try
            {
                var prospectiveDealers = _emamiContext.ProspectiveDealer.AsNoTracking().ToList()
                    .Select(s => new ProspectiveDealerVisitDto()
                    {
                        Name = s.Name,
                        MobileNumber = s.MobileNumber,
                        Email = s.Email,
                        Address = s.Address,
                        ProspectiveSales = s.ProspectiveSales,
                        ProspectiveInterestLevel = s.ProspectiveInterestLevel,
                        BusinessPotentialPeryear = s.BusinessPotentialPeryear,
                        DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == s.DealerId).Name
                    });
                return _resultService.SuccessObject(prospectiveDealers);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto GetPendingSaudaRemarksList(SalesTourPlanParamDto inputDto)
        {
            _methodName = "GetPendingSaudaRemarksList";
            var resultDto = new ResultDto();
            var outputdto = new List<PendingSaudaRemarksDto>();
            try
            {
                var pendingSaudaRemarks = _emamiContext.PendingSaudaRemarks.AsNoTracking()
                    .Where(w => w.DealerId == inputDto.DealerId && DbFunctions.TruncateTime(w.CreatedDate) == DbFunctions.TruncateTime(inputDto.CreatedDate)).ToList();
                foreach (var s in pendingSaudaRemarks)
                {
                    var dto = new PendingSaudaRemarksDto
                    {
                        SaudaId = s.SaudaId,
                        Status = s.SaudaId > 0 ? Enum.GetName(typeof(Adani.Solution.DTO.Enums.Status), _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(f => f.SaudaId == s.SaudaId).StatusId) : string.Empty,
                        Remarks = s.Remarks,
                        DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == s.DealerId).Name
                    };
                    outputdto.Add(dto);
                }
                return _resultService.SuccessObject(outputdto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto GetMarketScenariosList(SalesTourPlanParamDto inputDto)
        {
            _methodName = "GetMarketScenariosList";
            var resultDto = new ResultDto();
            var outputdto = new List<MarketScenariosDto>();
            try
            {
                var pendingSaudaRemarks = _emamiContext.MarketScenario.AsNoTracking()
                    .Where(w => w.DealerId == inputDto.DealerId && DbFunctions.TruncateTime(w.CreatedDate) == DbFunctions.TruncateTime(inputDto.CreatedDate)).ToList();
                foreach (var s in pendingSaudaRemarks)
                {
                    var dto = new MarketScenariosDto
                    {
                        DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == s.DealerId).Name,
                        Title = s.Title,
                        Remarks = s.Remarks
                    };
                    outputdto.Add(dto);
                }
                return _resultService.SuccessObject(outputdto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto GetCompetitorsList(SalesTourPlanParamDto inputDto)
        {
            _methodName = "GetCompetitorsList";
            var resultDto = new ResultDto();
            try
            {
                var pendingSaudaRemarks = _emamiContext.BdoCompetitor.AsNoTracking()
                    .Where(w => w.UserType == (int)DTO.Enums.UserType.Competitor && w.DealerId == inputDto.DealerId && DbFunctions.TruncateTime(w.CreatedDate) == DbFunctions.TruncateTime(inputDto.CreatedDate))
                    .Select(s => new BdoCompetitorsDto()
                    {
                        Id = s.Id,
                        CompetitorName = s.Name,
                        DealerName = string.Empty
                    });
                return _resultService.SuccessObject(pendingSaudaRemarks.ToList());
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto GetCompetitorSkuList(SalesTourPlanParamDto inputDto)
        {
            _methodName = "GetCompetitorSkuList";
            var resultDto = new ResultDto();
            try
            {
                var pendingSaudaRemarks = _emamiContext.BdoCompetitorSku.AsNoTracking()
                    .Where(w => w.BdoCompetitorId == inputDto.Id)
                    .Select(s => new BdoCompetitorSkusDto()
                    {
                        SkuName = s.SkuName,
                        QuanityPerMt = s.QuanityPerMt,
                        Price = s.Price
                    }).ToList();
                return _resultService.SuccessObject(pendingSaudaRemarks);
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

        #region Secondary Sales For the Day
        public ResultDto GetSecondarySalesFortheDayByWholeseller(SecondarySalesInputDto secondarySalesInputDto)
        {
            _methodName = "GetSecondarySalesFortheDay";
            var resultDto = new ResultDto();
            var wholesellerList = new List<WholesellerSecondarySalesDto>();
            try
            {
                if (secondarySalesInputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var SalesDetailContext = _emamiContext.WholeSellerSalesDetail.AsNoTracking().Where(_ => _.CreatedBy == secondarySalesInputDto.EmployeeId && DbFunctions.TruncateTime(_.CreatedDate) == DbFunctions.TruncateTime(secondarySalesInputDto.VisitDate))
                                            .Select(_ => DbFunctions.TruncateTime(_.CreatedDate)).Distinct().ToList();
                if (SalesDetailContext != null && SalesDetailContext.Any())
                {
                    foreach (var VisitDate in SalesDetailContext)
                    {
                        var wholesellerdto = new WholesellerSecondarySaleslistDto();
                        var SalesContext = (from sc in _emamiContext.WholeSellerSalesDetail
                                            where DbFunctions.TruncateTime(sc.CreatedDate) == DbFunctions.TruncateTime(VisitDate)
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
                                wholesellerList.Add(WholesellerSecondarySaleslistDto);
                            }
                        }
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

        public ResultDto GetSecondarySalesFortheDayByWholesellerForWeb(SecondarySalesInputDto secondarySalesInputDto)
        {
            _methodName = "GetSecondarySalesFortheDay";
            var resultDto = new ResultDto();
            var wholesellerList = new List<WholesellerSecondarySalesDto>();
            try
            {
                if (secondarySalesInputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var pendingSaudaRemarks = _emamiContext.WholesellerBdo.AsNoTracking()
                    .Where(w => w.DealerId == secondarySalesInputDto.EmployeeId && DbFunctions.TruncateTime(w.CreatedDate) == DbFunctions.TruncateTime(secondarySalesInputDto.VisitDate))
                    .ToList();

                foreach (var item in pendingSaudaRemarks)
                {
                    var dto = new WholesellerSecondarySalesDto
                    {
                        WholesellerId = item.Id,
                        Name = item.Name,
                        VisitDate = item.CreatedDate
                    };
                    wholesellerList.Add(dto);
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

        public ResultDto GetWholeSellerCompetitorsList(SalesTourPlanParamDto inputDto)
        {
            _methodName = "GetCompetitorsList";
            var resultDto = new ResultDto();
            var outputDto = new List<BdoCompetitorsDto>();
            try
            {
                var pendingSaudaRemarks = _emamiContext.BdoCompetitor.AsNoTracking()
                    .Where(w => w.UserType == (int)DTO.Enums.UserType.WholeSeller && w.DealerId == inputDto.DealerId && DbFunctions.TruncateTime(w.CreatedDate) == DbFunctions.TruncateTime(inputDto.CreatedDate))
                    .ToList();

                foreach (var item in pendingSaudaRemarks)
                {
                    var dto = new BdoCompetitorsDto
                    {
                        Id = item.Id,
                        CompetitorName = item.Name,
                        DealerName = _emamiContext.WholesellerBdo.AsNoTracking().FirstOrDefault(f => f.Id == item.BdoWholesellerId).Name
                    };
                    outputDto.Add(dto);
                }
                return _resultService.SuccessObject(outputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }
        public ResultDto GetFileAttachments(AttachmentInputDto inputDto)
        {
            _methodName = "GetSecondarySalesFortheDay";
            var resultDto = new ResultDto();
            var attachmentList = new List<AttachmentFileDto>();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var attachments = _emamiContext.Attachment.AsNoTracking()
                    .Where(w => w.RecordId == inputDto.RecordId && w.PageId == inputDto.PageId)
                    .ToList();
                if (inputDto.PageId == (int)DTO.Enums.PageType.Competitor)
                {
                    foreach (var item in attachments)
                    {
                        var dto = new AttachmentFileDto
                        {
                            RecordId = item.RecordId,
                            FileUrl = _resultService.GetCompetitorFilePath(item.Url)
                        };
                        attachmentList.Add(dto);
                    }
                }
                else if (inputDto.PageId == (int)DTO.Enums.PageType.ProspectiveDealer)
                {
                    foreach (var item in attachments)
                    {
                        var dto = new AttachmentFileDto
                        {
                            RecordId = item.RecordId,
                            FileUrl = _resultService.GetWholesellerFilePath(item.Url)
                        };
                        attachmentList.Add(dto);
                    }
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = attachmentList;
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

        public ResultDto GetUserAttendence(UserAttendenceInputDto inputDto)
        {
            _methodName = "GetMonthAndYearByFinancialYear";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var financialYearDto = new FinancialYearDto();
            var MonthList = new List<UserTargetDetailDto>();
            try
            {
                //var users = _emamiContext.UserAttendance.Where(s => s.User.ReportingToId == inputDto.LoginUserId).GroupBy(s => new { s.UserId, s.User.Name });
                var users = _emamiContext.UserAttendance
                    .Join(_emamiContext.UserReportingToMappings.AsNoTracking(),ua=>ua.UserId,ur=>ur.UserId,(ua,ur)=>new { ua,ur})
                    .Where(s => s.ur.ReportingToUserId == inputDto.LoginUserId).GroupBy(s => new { s.ua.UserId, s.ua.User.Name });
                var financialYear = _emamiContext.FinancialYears.FirstOrDefault(s => s.Id == inputDto.FinancialYear);
                var start = financialYear.EffectiveFrom;
                var end = financialYear.EffectiveTo;

                // set end-date to end of month
                end = new DateTime(end.Year, end.Month, DateTime.DaysInMonth(end.Year, end.Month));

                var year = Enumerable.Range(0, Int32.MaxValue)
                                     .Select(e => start.AddMonths(e))
                                     .TakeWhile(e => e <= end)
                                     .FirstOrDefault(s => s.Month == inputDto.Month).Year;

                DateTime firstOfNextMonth = new DateTime(year, inputDto.Month, 1).AddMonths(1).AddDays(-1);
                var processdate = new DateTime(year, inputDto.Month, 1).AddDays(-1);
                var dates = new List<DateTime>();
                DataTable dt = new DataTable();
                dt.Columns.Add("Name");
                while (firstOfNextMonth > processdate)
                {
                    processdate = processdate.AddDays(1);
                    dates.Add(processdate);
                    dt.Columns.Add(processdate.ToShortDateString());
                }

                foreach (var usr in users)
                {
                    var row = dt.NewRow();
                    row["Name"] = usr.Key.Name;
                    foreach (var rec in dates)
                    {
                        var loginInfos = usr.Where(s => s.ua.LoginTime.Value.Date == rec.Date).ToList();
                        TimeSpan timespan = new TimeSpan();
                        for (int i = 0; i < loginInfos.Count(); i++)
                        {
                            DateTime startTime = Convert.ToDateTime(loginInfos[i].ua.LoginTime);
                            //DateTime endTime = new DateTime();
                            var endTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                            if (loginInfos[i].ua.LogoutTime != null & loginInfos[i].ua.LogoutTime != DateTime.MinValue)
                            {
                                endTime = Convert.ToDateTime(loginInfos[i].ua.LogoutTime);
                                timespan = timespan.Add(endTime.Subtract(startTime));
                            }
                            else
                            {
                                var isNotAvailDuration = (i == loginInfos.Count() - 1);
                                if (isNotAvailDuration)
                                {
                                    if (rec.Date != endTime.Date)
                                    {
                                        endTime = Convert.ToDateTime(loginInfos[i].ua.LoginTime.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59));
                                    }
                                    timespan = timespan.Add(endTime.Subtract(startTime));
                                }
                            }
                        }
                        if ((int)timespan.TotalHours > 0)
                        {
                            row[rec.ToShortDateString()] = string.Format("{0}h {1}m", (int)timespan.TotalHours, timespan.Minutes);

                        }
                        else
                        {
                            row[rec.ToShortDateString()] = string.Format("{0}m", timespan.Minutes);

                        }

                    }
                    dt.Rows.Add(row);
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = dt;
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

        #region User Customer Target

        public ResultDto SaveUserCustomerTargetList(List<MapSalesTargetDetailDto> targetDetailDtoList)
        {
            _methodName = "SaveUserCustomerTargetList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            try
            {
                if (targetDetailDtoList == null || !targetDetailDtoList.Any())
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (targetDetailDtoList.FirstOrDefault().LoginUserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserIdMissing;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserIdMissing, Constants.EnglishLanguage);
                    return resultDto;
                }
                var loginUserId = targetDetailDtoList.FirstOrDefault().LoginUserId;
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == loginUserId);
                if (userContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }


                var errorMessage = string.Empty;

                var Templist = new List<MapSalesTargetDetailDto>();
                foreach (var item in targetDetailDtoList)
                {
                    var salesTargetContext = _emamiContext.UserCustomerTarget.AsNoTracking()
                            .FirstOrDefault(_ => _.AssignedToId == item.AssignedToId
                            && _.FinancialYearId == item.FinancialYearId
                            && _.MonthId == item.MonthId);
                    if (salesTargetContext != null)
                    {
                        var message = "" + _emamiContext.Months.AsNoTracking().FirstOrDefault(_ => _.Id == item.MonthId).Name + ", ";
                        errorMessage = Constants.BindErrorMessage(message + " - ", errorMessage);
                        //var removed = targetDetailDtoList.RemoveAll(_ => _.AssignedToId == item.AssignedToId && _.MonthId == item.MonthId);
                    }
                    else
                    {
                        Templist.Add(item);
                    }
                }
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    var existMessage = "Record Exists for the following Months: ";
                    errorMessage = existMessage + errorMessage;
                }

                foreach (var inputDto in Templist)
                {
                    var detailContext = new UserCustomerTarget
                    {
                        AssignedFromId = inputDto.LoginUserId,
                        AssignedToId = inputDto.AssignedToId,
                        MonthId = Convert.ToInt32(inputDto.MonthId),
                        FinancialYearId = inputDto.FinancialYearId,
                        Target = inputDto.Target,
                        Year = inputDto.Year,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    };
                    _emamiContext.UserCustomerTarget.Add(detailContext);
                    _emamiContext.SaveChanges();
                }
                var outputDto = new UserCustomerSalesTargetDto { ExistRecords = errorMessage };
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

        public ResultDto GetUserCustomerTargetList(LoginUserIdDto inputDto)
        {
            _methodName = "GetUserCustomerTargetList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var userSalesSaudaTargetDto = new List<UserCustomerSalesTargetDto>();
            try
            {
                userSalesSaudaTargetDto = _emamiContext.UserCustomerTarget.AsNoTracking().Where(_ => _.AssignedFromId == inputDto.LoginUserId)
                    .GroupBy(_ => new { _.AssignedToId, _.FinancialYearId }).ToList()
                    .Select(_ => new UserCustomerSalesTargetDto
                    {
                        FinancialYearId = _.FirstOrDefault().FinancialYearId,
                        AssignedFromId = _.FirstOrDefault()?.AssignedFromId,
                        AssignedToId = _.FirstOrDefault().AssignedToId,
                        AssignedToUser = _.FirstOrDefault()?.AssignedTo?.Name,
                        FinancialYear = _.FirstOrDefault().FinancialYear.Year
                    }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = userSalesSaudaTargetDto;
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

        public ResultDto GetUserCustomerTargetDetailList(UserTargetIdDto inputDto)
        {
            _methodName = "GetUserCustomerTargetDetailList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var resultOilTypeDto = new List<UserTargetDetailDto>();
            try
            {

                var resultContext = _emamiContext.UserCustomerTarget.AsNoTracking()
                    .Where(_ => _.AssignedToId == inputDto.AssignedToUserId && _.FinancialYearId == inputDto.FinancialYearId).ToList();

                if (resultContext != null && resultContext.Any())
                {
                    resultOilTypeDto = resultContext.Select(_ => new UserTargetDetailDto
                    {
                        Id = _.Id,
                        MonthId = _.MonthId,
                        Month = _.Month?.Name,
                        SalesTarget = _.Target,
                        //SaudaTarget = _.SaudaTarget
                    }).ToList();
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = resultOilTypeDto;
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

        public ResultDto UpdateUserCustomerTarget(List<MapSalesTargetDetailDto> targetDetailDtoList)
        {
            _methodName = "UpdateUserCustomerTarget";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            try
            {
                if (targetDetailDtoList == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                foreach (var details in targetDetailDtoList)
                {
                    var recordsContext = _emamiContext.UserCustomerTarget.FirstOrDefault(_ => _.Id == details.Id);
                    recordsContext.Target = details.Target;
                    recordsContext.ModifiedBy = details.LoginUserId;
                    recordsContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    _emamiContext.SaveChanges();
                }
                var outputDto = new UserCustomerSalesTargetDto { ExistRecords = string.Empty };
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

        public ResultDto GetUserCustomerTargetDetailsById(UserTargetIdDto inputDto)
        {
            _methodName = "GetUserCustomerTargetDetailsById";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var outputDto = new UserCustomerSalesTargetDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (inputDto.FinancialYearId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                var resultContext = _emamiContext.UserCustomerTarget.AsNoTracking()
                    .Where(_ => _.FinancialYearId == inputDto.FinancialYearId && _.AssignedToId == inputDto.AssignedToUserId).ToList();

                if (resultContext != null || resultContext.Any())
                {
                    outputDto.Id = resultContext.FirstOrDefault().Id;
                    outputDto.AssignedToId = resultContext.FirstOrDefault().AssignedToId;
                    outputDto.FinancialYearId = resultContext.FirstOrDefault().FinancialYearId;
                    outputDto.UserTargetDetail = resultContext.Select(_ => new UserTargetDetailDto
                    {
                        Id = _.Id,
                        MonthId = _.MonthId,
                        Month = _.Month.Name,
                        MonthAndYear = _.Month.Name + " " + _.Year,
                        SalesTarget = _.Target,
                    }).ToList();
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
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
        public ResultDto GetAssignedTargetList(LoginUserIdDto inputDto)
        {
            _methodName = "GetAssignedTargetList";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            var userSalesSaudaTargetDto = new List<UserCustomerSalesTargetDto>();
            try
            {
                userSalesSaudaTargetDto = _emamiContext.UserCustomerTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId)
                    .GroupBy(_ => new { _.AssignedToId, _.FinancialYearId }).ToList()
                    .Select(_ => new UserCustomerSalesTargetDto
                    {
                        FinancialYearId = _.FirstOrDefault().FinancialYearId,
                        AssignedFromId = _.FirstOrDefault()?.AssignedFromId,
                        AssignedToId = _.FirstOrDefault().AssignedToId,
                        AssignedToUser = _.FirstOrDefault()?.AssignedTo?.Name,
                        FinancialYear = _.FirstOrDefault().FinancialYear.Year
                    }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = userSalesSaudaTargetDto;
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

        #region STP History

        public ResultDto AddSalesTourPlanPcpHistory(PermanentJourneyPlanDetails pjPlan, DateTime effectiveFrom, DateTime effectiveTo, long finacialYearId, long modifiedBy)
        {
            _methodName = "AddSalesTourPlanPcpHistory";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var pcpHistory = _emamiContext.SalesTourPlanPcpHistory.FirstOrDefault(f => f.PermanentJourneyPlanDetailId == pjPlan.Id);

                if (pcpHistory == null)
                {
                    _emamiContext.SalesTourPlanPcpHistory.Add(new SalesTourPlanPcpHistory()
                    {
                        DealerId = !string.IsNullOrEmpty(pjPlan.RetailerId) ? Convert.ToInt64(pjPlan.RetailerId) : 0,
                        FinancialYearId = finacialYearId,
                        StateId = Convert.ToInt32(pjPlan.StateId),
                        TerritoryId = Convert.ToInt32(pjPlan.TerritoryId),
                        DistrictId = Convert.ToInt32(pjPlan.DistrictId),
                        CityId = Convert.ToInt32(pjPlan.TownId),
                        NoOfDirectDealer = pjPlan.NoOfDirectDealer,
                        NoofSubDealer = pjPlan.NoofSubDealer,
                        NoOfWholeSeller = pjPlan.NoOfWholeSeller,
                        NoOfVisit = Convert.ToInt64(pjPlan.NoOfVisit),
                        InHQNoVisit = pjPlan.InHQNoVisit,
                        PermanentJourneyPlanDetailId = pjPlan.Id,
                        EffectiveFrom = effectiveFrom,
                        EffectiveTo = effectiveTo,
                        IsDataChanged = true,
                        CreatedBy = modifiedBy,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        Remarks = pjPlan.Remarks,
                    });
                }
                else
                {
                    pcpHistory.DealerId = !string.IsNullOrEmpty(pjPlan.RetailerId) ? Convert.ToInt64(pjPlan.RetailerId) : 0;
                    pcpHistory.FinancialYearId = finacialYearId;
                    pcpHistory.StateId = Convert.ToInt32(pjPlan.StateId);
                    pcpHistory.TerritoryId = Convert.ToInt32(pjPlan.TerritoryId);
                    pcpHistory.DistrictId = Convert.ToInt32(pjPlan.DistrictId);
                    pcpHistory.CityId = Convert.ToInt32(pjPlan.TownId);
                    pcpHistory.NoOfDirectDealer = pjPlan.NoOfDirectDealer;
                    pcpHistory.NoofSubDealer = pjPlan.NoofSubDealer;
                    pcpHistory.NoOfWholeSeller = pjPlan.NoOfWholeSeller;
                    pcpHistory.NoOfVisit = Convert.ToInt64(pjPlan.NoOfVisit);
                    pcpHistory.InHQNoVisit = pjPlan.InHQNoVisit;
                    pcpHistory.PermanentJourneyPlanDetailId = pjPlan.Id;
                    pcpHistory.EffectiveFrom = effectiveFrom;
                    pcpHistory.EffectiveTo = effectiveTo;
                    pcpHistory.IsDataChanged = true;
                    pcpHistory.ModifiedBy = modifiedBy;
                    pcpHistory.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    pcpHistory.Remarks = pjPlan.Remarks;

                }
                _emamiContext.SaveChanges();
                return _resultService.SuccessMessage("success");
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.SuccessMessage(Constants.Exception);
            }
        }


        public ResultDto GetSalesTourPlanPcpHistory(long id)
        {
            _methodName = "GetSalesTourPlanPcpHistory";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var pcpHistoryDetails = new SalesTourPlanPcpHistoryDto();
            try
            {
                var pcpHistory = _emamiContext.SalesTourPlanPcpHistory.FirstOrDefault(f => f.PermanentJourneyPlanDetailId == id);
                
                if (pcpHistory != null)
                {

                    pcpHistoryDetails = new SalesTourPlanPcpHistoryDto()
                    {
                        DealerName = pcpHistory.DealerId != 0 ? _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == pcpHistory.DealerId)?.Name : string.Empty,
                        FinancialYear = pcpHistory.FinancialYear.Year,
                        State = _emamiContext.State.AsNoTracking().FirstOrDefault(_ => _.Id==pcpHistory.StateId) !=null ? _emamiContext.State.AsNoTracking().FirstOrDefault(_ => _.Id == pcpHistory.StateId).StateName:String.Empty,
                        //Territory = pcpHistory.Territory.Name,
                        District = _emamiContext.District.AsNoTracking().FirstOrDefault(_ => _.Id == pcpHistory.DistrictId) != null ? _emamiContext.District.AsNoTracking().FirstOrDefault(_ => _.Id == pcpHistory.DistrictId).DistrictName : String.Empty,
                        City = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == pcpHistory.CityId) != null ? _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == pcpHistory.CityId).CityName : String.Empty,
                        NoOfDirectDealer = pcpHistory.NoOfDirectDealer,
                        NoofSubDealer = pcpHistory.NoofSubDealer,
                        NoOfWholeSeller = pcpHistory.NoOfWholeSeller,
                        NoOfVisit = pcpHistory.NoOfVisit,
                        EffectiveFrom = pcpHistory.EffectiveFrom,
                        EffectiveTo = pcpHistory.EffectiveTo,
                        PostStatus = true,
                        InHQNoVisitId = pcpHistory.InHQNoVisit,
                        InHQNoVisitName = pcpHistory.InHQNoVisit != 0 ? Utility.GetEnumFromString<DTO.Enums.STPVisitType>(pcpHistory.InHQNoVisit) : string.Empty,
                        Remarks = pcpHistory.Remarks,
                    };

                    pcpHistory.IsDataChanged = false;
                    _emamiContext.SaveChanges();

                    return _resultService.SuccessObject(pcpHistoryDetails);
                }
                else
                {
                    return _resultService.SuccessObject(pcpHistoryDetails);
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.SuccessMessage(Constants.Exception);
            }
        }

        public ResultDto AddSalesTourPlanMtpHistory(MonthlyTourPlanDetails mtpPlan, long modifiedBy)
        {
            _methodName = "AddSalesTourPlanMtpHistory";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            try
            {
                var mtpHistory = _emamiContext.SalesTourPlanMtpHistory.FirstOrDefault(f => f.MonthlyTourPlanDetailId == mtpPlan.Id);

                if (mtpHistory == null)
                {
                    _emamiContext.SalesTourPlanMtpHistory.Add(new SalesTourPlanMtpHistory()
                    {
                        DealerId = !string.IsNullOrEmpty(mtpPlan.DealerId) ? Convert.ToInt64(mtpPlan.DealerId) : 0,
                        CityId = mtpPlan.TownId,
                        Area = mtpPlan.Area,
                        HeadquartersId = mtpPlan.HeadquartersId,
                        Remarks = mtpPlan.Remarks,
                        MonthlyTourPlanDetailId = mtpPlan.Id,
                        TourDate = mtpPlan.Date,
                        IsDataChanged = true,
                        CreatedBy = modifiedBy,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                        InHQNoVisit = mtpPlan.InHQNoVisit,
                    });
                }
                else
                {
                    mtpHistory.DealerId = !string.IsNullOrEmpty(mtpPlan.DealerId) ? Convert.ToInt64(mtpPlan.DealerId) : 0;
                    mtpHistory.CityId = mtpPlan.TownId;
                    mtpHistory.Area = mtpPlan.Area;
                    mtpHistory.HeadquartersId = mtpPlan.HeadquartersId;
                    mtpHistory.Remarks = mtpPlan.Remarks;
                    mtpHistory.MonthlyTourPlanDetailId = mtpPlan.Id;
                    mtpHistory.TourDate = mtpPlan.Date;
                    mtpHistory.IsDataChanged = true;
                    mtpHistory.ModifiedBy = modifiedBy;
                    mtpHistory.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    mtpHistory.InHQNoVisit = mtpPlan.InHQNoVisit;
                }
                _emamiContext.SaveChanges();
                return _resultService.SuccessMessage("success");
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.SuccessMessage(Constants.Exception);
            }
        }


        public ResultDto GetSalesTourPlanMtpHistory(long id)
        {
            _methodName = "GetSalesTourPlanMtpHistory";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var pcpHistoryDetails = new SalesTourPlanMtpHistoryDto();
            try
            {
                var mtpHistory = _emamiContext.SalesTourPlanMtpHistory.FirstOrDefault(f => f.MonthlyTourPlanDetailId == id);

                if (mtpHistory != null)
                {

                    pcpHistoryDetails = new SalesTourPlanMtpHistoryDto()
                    {
                        DealerName = mtpHistory.DealerId != 0 ? _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == mtpHistory.DealerId)?.Name : string.Empty,
                        City = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == mtpHistory.CityId) != null ? _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == mtpHistory.CityId).CityName : String.Empty,
                        Area = mtpHistory.Area,
                        //Headquarters = mtpHistory.Headquarters.Name,
                        Remarks = mtpHistory.Remarks,
                        TourDate = mtpHistory.TourDate,
                        PostStatus = true,
                        InHQNoVisitId = mtpHistory.InHQNoVisit,
                        InHQNoVisitName = mtpHistory.InHQNoVisit != 0 ? Utility.GetEnumFromString<DTO.Enums.STPVisitType>(mtpHistory.InHQNoVisit) : string.Empty
                    };
                    mtpHistory.IsDataChanged = false;
                    _emamiContext.SaveChanges();

                    return _resultService.SuccessObject(pcpHistoryDetails);
                }
                else
                {
                    return _resultService.SuccessObject(pcpHistoryDetails);
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.SuccessMessage(Constants.Exception);
            }
        }

        #endregion
    }
}
