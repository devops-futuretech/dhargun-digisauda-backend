//using Dapper;
using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using Dapper;
using GMCore.Helper;
using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Adani.Solution.Service
{
    public interface INationalHeadService
    {
        ResultDto DashboardWeekwiseOverallSauda(LoginUserIdDto loginUserIdDto);
        ResultDto DashboardWeekwiseOverallSales(LoginUserIdDto loginUserIdDto);
        ResultDto DashboardOverallSauda(DashboardOverallSaudaInputDto inputDto);
        ResultDto DashboardOverallSales(DashboardOverallSaudaInputDto inputDto);
        ResultDto GetZonalHeadList(LoginUserIdDto inputDto);
        ResultDto GetZHStatistics(SaudaFilterDto inputDto);
        ResultDto DueForTomorrowList(LoginNHId inputDto);
        ResultDto OverallSalesChart(DashboardOverallSaudaInputDto inputDto);
        ResultDto GetSpecialRateRequestList(SpecialRateInputDto inputDto);
        ResultDto GetBookedSauda(LoginNHId inputDto);
        ResultDto GetLiftingRequestCountList(LiftingRequestListInputDto inputDto);
        ResultDto DashboardPackwiseSaleslist(DashboardOverallSaudaInputDto inputDto);
        ResultDto SalesTourPlanChart(SalesTourPlanInputDto inputDto);
        ResultDto GetSecondarySalesFortheDay(LoginZHId inputDto);
        ResultDto GetPendingSaudaChartForMobile(LoginZHId inputDto);
        ResultDto OverallSaleslistByDealers(DashboardOverallSaudaInputDto inputDto);
        ResultDto GetSpecialityFatQuantityLimitList(LoginUserIdDto inputDto);
        ResultDto GetAssignedSpecialityFatQuantityLimitList(LoginUserIdDto inputDto);
        ResultDto AssignSpecialityFatQuantityLimit(SpecialityFatEmployeeDiscountDto inputDto);
        ResultDto UpdateAssignedSpecialityFatQuantityLimit(SpecialityFatDiscountUserDto inputDto);
        ResultDto AddSpecialtyFatQuantityRequests(SpecialtyFatQuantityRequestDto inputDto);
        ResultDto GetSpecialtyFatQuantityRequestsListByOrganizationReportingToId(SpecialtyFatQuantityRequestSearchDto inputDto);
        ResultDto GetSpecialtyFatQuantityRequestsList(SpecialtyFatQuantityRequestSearchDto inputDto);
        ResultDto UpdateSpecialtyFatQuantityLimit(SpecialtyFatQuantityRequestDto inputDto);
        ResultDto GetMultiselectDiscountList(LoginUserIdDto inputDto);
        ResultDto GetMultiselectPremiumList(LoginUserIdDto inputDto);
        ResultDto GetEmployeeAndUserDiscountList(LoginUserIdDto inputDto);
        ResultDto GetPremiumList(LoginUserIdDto inputDto);
        ResultDto GetDiscountUserList(LoginUserIdDto inputDto);
        ResultDto GetAssignedPremiumList(LoginUserIdDto inputDto);
        ResultDto AssignMultiselectPremium(PremiumUserDto inputDto);
        ResultDto AssignMultiselectDiscount(DiscountUserDto inputDto);
        ResultDto UpdateDiscountUsers(DiscountUserDto inputDto);
        ResultDto AddEmployeeAndUserDiscount(EmployeeUserDiscountDto inputDto);
        ResultDto UpdatePremium(PremiumUserDto inputDto);
        ResultDto AssignPremium(EmployeeUserPremiumDto inputDto);
        ResultDto GetTotalCreditLimit(CreditLimitInputDto inputDto);
        ResultDto ZHPlantDepotDetailsByDealer(LoginUserIdDto inputDto);
        ResultDto DailyBookedSaudaReport(DailyBookedSaudaInputDto inputDto);
        ResultDto SpecialityFatDiscountUsersList(LoginNHId inputDto);
        ResultDto SpecialityFatDiscountUpdate(SpecialityFatDiscountUpdateInputDto inputDto);
        ResultDto SalesReport(DailyBookedSaudaInputDto inputDto);
        ResultDto GetPendingSaudaChartDetailForMobile(LoginNHId inputDto);
        ResultDto GetPendingContractChartMobile(LoginNHId inputDto);
        ResultDto GetSpecialRateRequestListNew(SpecialRateInputDto inputDto);
    }
    public class NationalHeadService : INationalHeadService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("National Head Service");
        private const string ServiceName = "National Head";
        private string _methodName;
        private readonly IResultService _resultService;

        public NationalHeadService(IAdaniContext emamiContext, IResultService resultService)
        {
            try
            {
                _emamiContext = emamiContext;
                _resultService = resultService;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for National Head Service", exception);
            }
        }

        #region Dashboard
        public ResultDto DashboardWeekwiseOverallSauda(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "DashboardWeekwiseOverallSauda";
            try
            {
                var overallSauda = new DashboardWeekwiseOverallSaudaDto();
                if (loginUserIdDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
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
                //New Reporting to table change
                //var ZHList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == loginUserIdDto.LoginUserId).Select(_ => _.UserId).ToList();
                //var ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == loginUserIdDto.LoginUserId).Select(_ => _.Id).ToList();
                var targetContext = _emamiContext.UserCustomerSaudaTarget.AsNoTracking().Where(_ => _.AssignedToId == loginUserIdDto.LoginUserId && _.MonthId == currentDate.Month && _.Year == currentDate.Year).ToList();
                if (targetContext != null)
                {
                    //Weekwise target
                    overallSauda.TotalTarget = targetContext.Sum(_ => _.Target);
                }
                var status = Constants.OverallSaudaStatus;
                DateTime now = currentDate;
                DateTime mStartDate = new DateTime(now.Year, now.Month, 1);
                DateTime mEndDate = mStartDate.AddMonths(1).AddDays(-1);
                DateTime wStartDate = mStartDate;
                DateTime wEndDate = DateTime.MinValue;
                int weekId = 1;
                var weekNo = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(mStartDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                //var bdoList = _emamiContext.Users.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToId ?? 0)).Select(_ => _.Id).ToList();

                IEnumerable<DashboardSauda> saudaContextList = new List<DashboardSauda>();
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    
                    saudaContextList = conn.Query<DashboardSauda>("GetNHWeekwiseOverallSauda", new
                    {
                        UserId = loginUserIdDto.LoginUserId,
                        StartDate = mStartDate,
                        EndDate = mEndDate,
                        Status = UtilityHelper.ConvertIntListToCommaSeparatedString(status)
                    },commandTimeout:300,commandType:CommandType.StoredProcedure);

                }

                //var bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();
                //if (bdoList != null && bdoList.Any())
                //{
                //    IQueryable<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(s => s.CustomerId);
                //    if (dealersList != null && dealersList.Any())
                //    {
                //Weekwise report
                //var divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == loginUserIdDto.LoginUserId)
                //.Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });


                //var saudaContextList = (from s in _emamiContext.Sauda.AsNoTracking()
                //                        join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                //                        join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                //                        equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                //                        //join dl in dealersList on s.UserId equals dl.CustomerId
                //                        where DbFunctions.TruncateTime(so.CreatedDate) >= DbFunctions.TruncateTime(mStartDate) &&
                //                        DbFunctions.TruncateTime(so.CreatedDate) <= DbFunctions.TruncateTime(mEndDate) && status.Contains(so.StatusId)
                //                        //&& (bdoList.Contains(s.BdoId) || s.BdoId==0)
                //                        && dealersList.Contains(s.UserId)
                //                        select new { CreatedDate = so.CreatedDate, BidQuantity = so.BidQuantity });

                do
                {
                    var weekwiseTargetAchieved = new OverallSaudaWeekWiseAchievementDto();

                    if (weekId < 4)
                    {
                        wEndDate = wStartDate.AddDays(6);
                    }
                    else
                    {
                        wEndDate = mEndDate;
                    }

                    var sauda = saudaContextList.Where(_ => _.Date.Date >= wStartDate.Date &&
                    _.Date.Date <= wEndDate.Date).ToList();

                    //  var saudaContextList1 = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && dealersList.Any(a => a.CustomerId == _.Sauda.UserId) && status.Contains(_.StatusId)).ToList();

                    if (sauda != null && sauda.Any())
                    {
                        weekwiseTargetAchieved.WeekId = weekId;
                        weekwiseTargetAchieved.Week = "Week " + weekId;
                        weekwiseTargetAchieved.Achievement = sauda.Sum(_ => _.Achievment);
                        weekwiseTargetAchieved.Target = overallSauda.TotalTarget / 4;
                    }
                    else
                    {
                        weekwiseTargetAchieved.WeekId = weekId;
                        weekwiseTargetAchieved.Week = "Week " + weekId;
                        weekwiseTargetAchieved.Achievement = 0;
                        weekwiseTargetAchieved.Target = overallSauda.TotalTarget / 4;
                    }
                    wStartDate = wEndDate.AddDays(1);
                    weekId++;

                    overallSauda.OverallSauda = overallSauda.OverallSauda + weekwiseTargetAchieved.Achievement;
                    overallSauda.OverallWeekWiseAchievements.Add(weekwiseTargetAchieved);
                }
                while (weekId <= 4);
                return _resultService.SuccessObject(overallSauda);
                //    }
                //    else
                //    {
                //        return _resultService.ErrorMessage(Constants.RecordNotFound);
                //    }
                //}
                //else
                //{
                //    return _resultService.ErrorMessage(Constants.RecordNotFound);
                //}

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }
        public ResultDto DashboardWeekwiseOverallSales(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "DashboardWeekwiseOverallSales";
            try
            {
                var overallSales = new DashboardWeekwiseOverallSalesDto();
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

                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                //New Reporting to table change
                //var ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == loginUserIdDto.LoginUserId).Select(_ => _.Id).ToList();
                //var ZHList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == loginUserIdDto.LoginUserId).Select(_ => _.UserId).ToList();
                var targetContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == loginUserIdDto.LoginUserId && _.MonthId == currentDate.Month && _.Year == currentDate.Year).ToList();
                if (targetContext != null)
                {
                    //Overall target
                    overallSales.TotalTarget = targetContext.Sum(_ => _.Target);
                }
                var status = Constants.OverallSaudaStatus;
                DateTime now = currentDate;
                DateTime mStartDate = new DateTime(now.Year, now.Month, 1);
                DateTime mEndDate = mStartDate.AddMonths(1).AddDays(-1);
                DateTime wStartDate = mStartDate;
                DateTime wEndDate = DateTime.MinValue;
                int weekId = 1;
                var weekNo = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(mStartDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
                //var bdoList = _emamiContext.Users.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToId ?? 0)).Select(_ => _.Id).ToList();
                //var bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();

                IEnumerable<DashboardSauda> sales = new List<DashboardSauda>();
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    //var sqlQuery = @"CREATE TABLE #ZHTemp(ZHId BIGINT)
                    //            CREATE TABLE #BdoTemp(BdoId BIGINT)
                    //            CREATE TABLE #DealerTemp(DealerId BIGINT)
                    //            Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                    //            insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                    //            select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

                    //            insert into #ZHTemp(ZHId) select UserId from UserReportingToMappings where ReportingToUserId=@UserId
                    //             insert into #BdoTemp(BdoId) select UserId from UserReportingToMappings 
                    //             where ReportingToUserId in (select ZHId from #ZHTemp)
                    //             insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
                    //             where UserId in (select BdoId from #BdoTemp)

                    //             select 
                    //             s.InvoiceDate as Date,
                    //             s.QuantityMT as Achievment
                    //             from SalesRegisters s with(NOLOCK)
                    //             join Skus sku on s.MaterialCode=sku.SkuCode and s.SalesOrganizationId=sku.SalesOrganizationId
                    //             and s.DistributionChannelId=sku.DistributionChannelId and s.DivisionId=sku.DivisionId
                    //             join Users u on s.CustomerCode=u.Code
                    //             join #UserDivision ud on s.SalesOrganizationId=ud.SalesOrganizationId
                    //             and s.DistributionChannelId=ud.DistributionChannelId and s.DivisionId=ud.DivisionId
                    //             and Cast(s.InvoiceDate as date) >= Cast(@StartDate as date)
                    //              and Cast(s.InvoiceDate as date) <= Cast(@EndDate as date)
                    //              and u.Id in (select DealerId from #DealerTemp)

                    //              drop table #BdoTemp
                    //              drop table #DealerTemp
                    //              drop table #UserDivision
                    //              drop table #ZHTemp
                    //    ";
                    sales = conn.Query<DashboardSauda>("NHGetWeekwiseOverallSales",
                        new
                        {
                            UserId = loginUserIdDto.LoginUserId,
                            StartDate = mStartDate,
                            EndDate = mEndDate
                        }, commandType: CommandType.StoredProcedure,commandTimeout:300).ToList();
                    //sales = conn.Query<DashboardSauda>(sqlQuery, new
                    //{
                    //    UserId = loginUserIdDto.LoginUserId,
                    //    StartDate = mStartDate,
                    //    EndDate = mEndDate
                    //});

                }

                //if (bdoList != null && bdoList.Any())
                //{
                //    var divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == loginUserIdDto.LoginUserId)
                //  .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                //    List<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                //    if (dealersList != null && dealersList.Any())
                //    {
                //Weekwise report
                //var sales = (from s in _emamiContext.SalesRegister.AsNoTracking()
                //             join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                //             join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                //             equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                //             join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                //             //join dl in dealersList on u.Id equals dl.CustomerId
                //             where (DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(mStartDate) &&
                //             DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(mEndDate)) && dealersList.Contains(u.Id)
                //             && s.SalesOrganizationId == sku.SalesOrganizationId && s.DistributionChannelId == sku.DistributionChannelId
                //             && s.DivisionId == sku.DivisionId
                //             select new { InvoiceDate = s.InvoiceDate, Quantity = s.QuantityMT });

                do
                {
                    var weekwiseTargetAchieved = new OverallSaudaWeekWiseAchievementDto();

                    if (weekId < 4)
                    {
                        wEndDate = wStartDate.AddDays(6);
                    }
                    else
                    {
                        wEndDate = mEndDate;
                    }

                    var salesContextList = sales.Where(_ => _.Date.Date >= wStartDate.Date &&
                                     _.Date <= wEndDate.Date).ToList();

                    if (salesContextList != null && salesContextList.Any())
                    {
                        weekwiseTargetAchieved.WeekId = weekId;
                        weekwiseTargetAchieved.Week = "Week " + weekId;
                        weekwiseTargetAchieved.Achievement = salesContextList.Sum(_ => _.Achievment);
                        weekwiseTargetAchieved.Target = overallSales.TotalTarget / 4;
                    }
                    else
                    {
                        weekwiseTargetAchieved.WeekId = weekId;
                        weekwiseTargetAchieved.Week = "Week " + weekId;
                        weekwiseTargetAchieved.Achievement = 0;
                        weekwiseTargetAchieved.Target = overallSales.TotalTarget / 4;
                    }
                    wStartDate = wEndDate.AddDays(1);
                    weekId++;

                    overallSales.OverallSales = overallSales.OverallSales + weekwiseTargetAchieved.Achievement;
                    overallSales.OverallWeekWiseAchievements.Add(weekwiseTargetAchieved);
                }
                while (weekId <= 4);
                return _resultService.SuccessObject(overallSales);
                //    }
                //    else
                //    {
                //        return _resultService.ErrorMessage(Constants.RecordNotFound);
                //    }
                //}
                //else
                //{
                //    return _resultService.ErrorMessage(Constants.RecordNotFound);
                //}
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto DashboardOverallSauda(DashboardOverallSaudaInputDto inputDto)
        {
            _methodName = "DashboardOverallSauda";
            var dashboardOverallsaudaOutpuDto = new List<DashboardOverallsaudaOutpuDto>();
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
                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }
                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }

                List<MonthDto> months = new List<MonthDto>();
                months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);
                //New Reporting to table change
                //var ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                //var ZHList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                //if (ZHList != null && ZHList.Any())
                //{
                //    var bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();
                //    //var bdoList = _emamiContext.Users.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToId ?? 0)).Select(_ => _.Id).ToList();
                //    if (bdoList != null && bdoList.Any())
                //    {
                //        var divisionsloginWiseuser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                //        .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                //        IQueryable<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId);

                var target = _emamiContext.UserCustomerSaudaTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId);

                var status = Constants.OverallSaudaStatus;

                IEnumerable<DashboardSauda> sauda = new List<DashboardSauda>();
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    
                    sauda = conn.Query<DashboardSauda>("GetNHOverallSauda", new
                    {
                        UserId = inputDto.LoginUserId,
                        StartDate = inputDto.FromDate,
                        EndDate = inputDto.ToDate,
                        Status = UtilityHelper.ConvertIntListToCommaSeparatedString(status)
                    },commandType:CommandType.StoredProcedure,commandTimeout:300); ;

                }


                //var sauda = (from s in _emamiContext.Sauda.AsNoTracking()
                //             join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                //             join dm in divisionsloginWiseuser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                //             equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                //         //join dl in dealersList on s.UserId equals dl.CustomerId
                //             where DbFunctions.TruncateTime(so.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                //             DbFunctions.TruncateTime(so.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && status.Contains(so.StatusId)
                //             //&& (bdoList.Contains(s.BdoId) || s.BdoId==0)
                //             && dealersList.Contains(s.UserId)
                //             orderby so.CreatedDate descending
                //             select new AchievmentDetailsDto()
                //             {
                //                 Date = so.CreatedDate,
                //                 Achievment = so.BidQuantity
                //             });

                foreach (var item in months)
                {
                    var outputDto = new DashboardOverallsaudaOutpuDto();
                    outputDto.TotalTarget = target.Where(_ => _.MonthId == item.Id && _.Year == item.Year).Select(s => s.Target).DefaultIfEmpty(0).Sum();
                    //if (targetContext != null)
                    //{
                    //    outputDto.TotalTarget = targetContext.Sum(_ => _.Target);
                    //}
                    outputDto.MonthId = item.Id;
                    outputDto.Month = new DateTime(DateTime.Now.Year, item.Id, 1).ToString("MMMM");
                    //if (dealersList != null && dealersList.Any())
                    //{
                    outputDto.OverallSauda = sauda.Where(_ => _.Date >= item.StartDate.Date &&
                                        _.Date.Date <= item.EndDate.Date).Select(s => s.Achievment)
                                        .DefaultIfEmpty(0).Sum();
                    //outputDto.AchievmentDetailsDto.AddRange(achievements);
                    // outputDto.OverallSauda = achievements.Sum(_ => _.Achievment);
                    dashboardOverallsaudaOutpuDto.Add(outputDto);
                    //}
                }
                //    }
                //}

                var resultData = new NewDashboardOverallSaudaOutputDto();
                resultData.SaudaList = dashboardOverallsaudaOutpuDto;
                resultData.TotalTarget = dashboardOverallsaudaOutpuDto.Select(_ => _.TotalTarget).DefaultIfEmpty(0).Sum();
                resultData.OverallSauda = dashboardOverallsaudaOutpuDto.Select(_ => _.OverallSauda).DefaultIfEmpty(0).Sum();

                resultData.Quarter1 = new QuarterOverallSaudaDto()
                {
                    OverallSauda = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter1.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.OverallSauda).DefaultIfEmpty(0).Sum(),
                    TotalTarget = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter1.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.TotalTarget).DefaultIfEmpty(0).Sum()
                };
                resultData.Quarter2 = new QuarterOverallSaudaDto()
                {
                    OverallSauda = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter2.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.OverallSauda).DefaultIfEmpty(0).Sum(),
                    TotalTarget = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter2.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.TotalTarget).DefaultIfEmpty(0).Sum()
                };
                resultData.Quarter3 = new QuarterOverallSaudaDto()
                {
                    OverallSauda = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter3.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.OverallSauda).DefaultIfEmpty(0).Sum(),
                    TotalTarget = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter3.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.TotalTarget).DefaultIfEmpty(0).Sum()
                };
                resultData.Quarter4 = new QuarterOverallSaudaDto()
                {
                    OverallSauda = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter4.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.OverallSauda).DefaultIfEmpty(0).Sum(),
                    TotalTarget = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter4.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.TotalTarget).DefaultIfEmpty(0).Sum()
                };

                return _resultService.SuccessObject(resultData);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

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

        public ResultDto DueForTomorrowList(LoginNHId inputDto)
        {
            _methodName = "DueForTomorrowList";
            var dashboardDetailsForPendingAndOverDueOutputDto = new DashboardDetailsForPendingAndOverDueOutputDto();
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
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                List<long> bdoList = new List<long>();
                List<long> dealerlist = new List<long>();
                if (inputDto.ZHIds == null && inputDto.BDOIds == null && inputDto.DealerIds == null)
                {
                    //var ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                    //bdoList = _emamiContext.Users.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToId ?? 0)).Select(_ => _.Id).ToList();
                    var ZHList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                    bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();

                    dealerlist = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                }
                else if (inputDto.ZHIds.IsAny() && inputDto.BDOIds == null && inputDto.DealerIds == null)
                {
                    //bdoList = _emamiContext.Users.AsNoTracking().Where(_ => inputDto.ZHIds.Contains(_.ReportingToId ?? 0)).Select(_ => _.Id).ToList();
                    bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => inputDto.ZHIds.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();
                    dealerlist = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                }
                else if (inputDto.BDOIds.IsAny() && inputDto.DealerIds == null)
                {
                    dealerlist = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => inputDto.BDOIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                }
                else if (inputDto.DealerIds.IsAny())
                {
                    dealerlist.AddRange(inputDto.DealerIds);
                }

                var userCreditMaster = _emamiContext.UserCreditMaster.AsNoTracking().Where(_ => _.Isactive && dealerlist.Contains(_.UserId)).ToList();

                if (userCreditMaster.IsAny())
                {
                    if (inputDto.DueStatus == (int)DTO.Enums.DueStatus.OverDue)
                    {
                        dashboardDetailsForPendingAndOverDueOutputDto.TotalBookedValueOverDue = userCreditMaster.DefaultIfEmpty().Sum(s => s.Overdue);
                        var userCreditMasterContext = userCreditMaster.Select(s => new OverAndPendingDueWithDealerDetails()
                        {
                            DealerCode = s.User.Code,
                            DealerName = s.User.Name,
                            OverDue = s.Overdue
                        }).ToList();
                        dashboardDetailsForPendingAndOverDueOutputDto.OverAndPendingDueWithDealerDetails.AddRange(userCreditMasterContext);
                    }
                    else if (inputDto.DueStatus == (int)DTO.Enums.DueStatus.PendingDue)
                    {
                        var todayDue = userCreditMaster.DefaultIfEmpty().Sum(s => s.DueToday);
                        var tomorrowsDue = userCreditMaster.DefaultIfEmpty().Sum(s => s.TomorrowsDue);
                        dashboardDetailsForPendingAndOverDueOutputDto.TotalBookedValuePendingDue = todayDue + tomorrowsDue;
                        var userCreditMasterContext = userCreditMaster.Select(s => new OverAndPendingDueWithDealerDetails()
                        {
                            DealerCode = s.User.Code,
                            DealerName = s.User.Name,
                            PendingDue = s.DueToday + s.TomorrowsDue
                        }).ToList();
                        dashboardDetailsForPendingAndOverDueOutputDto.OverAndPendingDueWithDealerDetails.AddRange(userCreditMasterContext);
                    }
                    else
                    {
                        dashboardDetailsForPendingAndOverDueOutputDto.TotalBookedValueOverDue = userCreditMaster.DefaultIfEmpty().Sum(s => s.Overdue);
                        var todayDue = userCreditMaster.DefaultIfEmpty().Sum(s => s.DueToday);
                        var tomorrowsDue = userCreditMaster.DefaultIfEmpty().Sum(s => s.TomorrowsDue);
                        dashboardDetailsForPendingAndOverDueOutputDto.TotalBookedValuePendingDue = todayDue + tomorrowsDue;
                        var userCreditMasterContext = userCreditMaster.Select(s => new OverAndPendingDueWithDealerDetails()
                        {
                            DealerCode = s.User.Code,
                            DealerName = s.User.Name,
                            OverDue = s.Overdue,
                            PendingDue = s.DueToday + s.TomorrowsDue
                        }).ToList();
                        dashboardDetailsForPendingAndOverDueOutputDto.OverAndPendingDueWithDealerDetails.AddRange(userCreditMasterContext);
                    }
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                return _resultService.SuccessObject(dashboardDetailsForPendingAndOverDueOutputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto DashboardOverallSales(DashboardOverallSaudaInputDto inputDto)
        {
            _methodName = "DashboardOverallSales";
            var dashboardOverallsaudaOutpuDto = new List<DashboardOverallsaudaOutpuDto>();

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
                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }
                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }
                //New Reporting to table change
                //var ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                var ZHList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                if (ZHList != null && ZHList.Any())
                {
                    //var bdoList = _emamiContext.Users.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToId ?? 0)).Select(_ => _.Id).ToList();
                    var bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();
                    if (bdoList != null && bdoList.Any())
                    {
                        IQueryable<UserCustomerMapping> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId));
                        List<MonthDto> months = new List<MonthDto>();
                        months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);

                        //var divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                        //.Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                        var target = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId).ToList();

//                        var sqlquery = @"Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
//Create Table #ZHTemp(ZHId bigint)
//Create Table #BdoTemp(BdoId bigint)
//Create Table #DealerTemp(DealerId bigint)

//insert into #ZHTemp(ZHId) select UserId from UserReportingToMappings where ReportingToUserId=@UserId
//insert into #BdoTemp(BdoId) select UserId from UserReportingToMappings where ReportingToUserId in (select ZHId from #ZHTemp)
//insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings where UserId in (select BdoId from #BdoTemp)

//insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
//                                            select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId
//select 
                                    
//                                    s.QuantityMT as Achievment,
//                                    s.InvoiceDate as Date
//                                    from SalesRegisters s with(NOLOCK)
//                                    join Skus sk on s.MaterialCode=sk.SkuCode
//                                    join Users u on s.CustomerCode=u.Code
//									join #UserDivision ud on ud.SalesOrganizationId=s.SalesOrganizationId 
//									and ud.DistributionChannelId=s.DistributionChannelId and ud.DivisionId=s.DivisionId
//                                    where (Cast(s.InvoiceDate as date)>=Cast(@FromDate as date) and Cast(s.InvoiceDate as date)<=Cast(@ToDate as date))
//                                    and s.SalesOrganizationId=sk.SalesOrganizationId and s.DistributionChannelId=sk.DistributionChannelId 
//                                    and s.DivisionId=sk.DivisionId
//									and u.Id in (select DealerId from #DealerTemp)
//drop table #BdoTemp
//drop table #DealerTemp
//drop table #UserDivision
//drop table #ZHTemp";
                        IEnumerable<DashboardSauda> salesContext = new List<DashboardSauda>();
                        using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                        {
                            salesContext = conn.Query<DashboardSauda>("NHGetOverallSales",
                               new
                               {
                                   UserId = inputDto.LoginUserId,
                                   StartDate = inputDto.FromDate,
                                   EndDate = inputDto.ToDate
                               }, commandType: CommandType.StoredProcedure,commandTimeout:300).ToList();
                            //var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                            //salesContext = conn.Query<DashboardSauda>(sqlquery, new
                            //{
                            //    UserId=inputDto.LoginUserId,
                            //    FromDate = inputDto.FromDate,
                            //    ToDate = inputDto.ToDate,
                            //});

                        }

                        //var salesContext = (from s in saleslist
                        //                    join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                        //                    equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                        //                    join dl in dealersList on s.UserId equals dl.CustomerId
                        //                    select new
                        //                    {
                        //                        Date = s.InvoiceDate,
                        //                        Achievment = s.QuantityMT,
                        //                    }
                        //                   );
                        //var salesContext1 = (from s in _emamiContext.SalesRegister.AsNoTracking()
                        //                    join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                        //                    join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                        //                    equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                        //                    join dl in dealersList on s.UserId equals dl.CustomerId
                        //                    where (DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                        //                       DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                        //                       && s.SalesOrganizationId == sku.SalesOrganizationId && s.DistributionChannelId == sku.DistributionChannelId
                        //                       && s.DivisionId == sku.DivisionId
                        //                    select new
                        //                    {
                        //                        UserId = s.CreatedBy,
                        //                        Date = s.CreatedDate,
                        //                        Achievment = s.QuantityMT,
                        //                        SalesRegisterId = s.Id
                        //                    });

                        foreach (var item in months)
                        {
                            var outputDto = new DashboardOverallsaudaOutpuDto();
                            outputDto.TotalTarget = target.Where(_ => _.MonthId == item.Id && _.Year == item.Year).Select(_ => _.Target).DefaultIfEmpty(0).Sum();
                            //if (targetContext != null)
                            //{
                            //    outputDto.TotalTarget = targetContext.Sum(_ => _.Target);
                            //}
                            outputDto.MonthId = item.Id;
                            outputDto.Month = new DateTime(DateTime.Now.Year, item.Id, 1).ToString("MMMM");

                            if (dealersList != null && dealersList.Any())
                            {
                                //var achievements1 = (from s in salesContext
                                //                     where DbFunctions.TruncateTime(s.Date) >= DbFunctions.TruncateTime(item.StartDate) &&
                                //            DbFunctions.TruncateTime(s.Date) <= DbFunctions.TruncateTime(item.EndDate)
                                //            group s by s.SalesRegisterId into sales
                                //            select new AchievmentDetailsDto()
                                //            {
                                //                UserId = sales.FirstOrDefault().UserId,
                                //                Date = sales.FirstOrDefault().Date,
                                //                Achievment = sales.Sum(s => s.Achievment)
                                //            }
                                //         ).ToList();

                                outputDto.OverallSauda = salesContext.Where(_ => _.Date.Date >= item.StartDate.Date &&
                                            _.Date.Date <= item.EndDate.Date)
                                            .Select(s => s.Achievment).DefaultIfEmpty(0).Sum();

                                //outputDto.AchievmentDetailsDto.AddRange(achievements);
                                //outputDto.OverallSauda =  achievements.Sum(_ => _.Achievment);
                                dashboardOverallsaudaOutpuDto.Add(outputDto);
                            }
                        }
                    }
                }

                var resultData = new NewDashboardOverallSaudaOutputDto();
                resultData.SaudaList = dashboardOverallsaudaOutpuDto;
                resultData.TotalTarget = dashboardOverallsaudaOutpuDto.Select(_ => _.TotalTarget).DefaultIfEmpty(0).Sum();
                resultData.OverallSauda = dashboardOverallsaudaOutpuDto.Select(_ => _.OverallSauda).DefaultIfEmpty(0).Sum();

                resultData.Quarter1 = new QuarterOverallSaudaDto()
                {
                    OverallSauda = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter1.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.OverallSauda).DefaultIfEmpty(0).Sum(),
                    TotalTarget = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter1.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.TotalTarget).DefaultIfEmpty(0).Sum()
                };
                resultData.Quarter2 = new QuarterOverallSaudaDto()
                {
                    OverallSauda = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter2.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.OverallSauda).DefaultIfEmpty(0).Sum(),
                    TotalTarget = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter2.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.TotalTarget).DefaultIfEmpty(0).Sum()
                };
                resultData.Quarter3 = new QuarterOverallSaudaDto()
                {
                    OverallSauda = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter3.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.OverallSauda).DefaultIfEmpty(0).Sum(),
                    TotalTarget = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter3.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.TotalTarget).DefaultIfEmpty(0).Sum()
                };
                resultData.Quarter4 = new QuarterOverallSaudaDto()
                {
                    OverallSauda = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter4.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.OverallSauda).DefaultIfEmpty(0).Sum(),
                    TotalTarget = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter4.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.TotalTarget).DefaultIfEmpty(0).Sum()
                };

                return _resultService.SuccessObject(resultData);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto ZHPlantDepotDetailsByDealer(LoginUserIdDto inputDto)
        {
            _methodName = "BDOPlantDepotDetailsByDealer";
            var userMasterDto = new List<UserMasterDto>();
            var PlantDepotList = new List<DepotDto>();

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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                //var ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                var ZHList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                if (ZHList != null && ZHList.Any())
                {
                    //var bdoList = _emamiContext.Users.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToId ?? 0)).Select(_ => new { Id = _.Id/*, VerticalId = _.DivisionId*/, SaudaBookingTypeId = _.SaudaBookingTypeId }).ToList();
                    var bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToUserId)).Select(_ => new { Id = _.UserId }).ToList();
                    foreach (var StateTrader in bdoList)
                    {
                        userMasterDto = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                         join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                                         where ucm.UserId == StateTrader.Id && u.IsActive
                                         //&& u.DivisionId == StateTrader.VerticalId
                                         //&& u.SaudaBookingTypeId == StateTrader.SaudaBookingTypeId
                                         select new UserMasterDto
                                         {
                                             Id = u.Id,
                                             EmployeeName = u.Name,
                                             EmployeeCode = u.Code,
                                             //FrieghtRoute = u.FreightRoute.Name,
                                             //FrieghtZone = u.FreightZone.Name,
                                             Loadability = u.Loadability,
                                             DepotLoadability = u.DepotLoadability,
                                             //VerticalId = u.DivisionId != null ? u.DivisionId.Value : 0,
                                             SaudaBookingTypeId = u.SaudaBookingTypeId != null ? u.SaudaBookingTypeId.Value : 0
                                         }).ToList();

                        foreach (var user in userMasterDto)
                        {
                            var depotList =
                                      (from depot in _emamiContext.Depots.AsNoTracking()
                                       join depotMapping in _emamiContext.UserDepotMapping.AsNoTracking() on depot.Id equals depotMapping.DepotId
                                       where depotMapping.UserId == user.Id && depot.IsActive && depot.IsPlant
                                       select new DepotDto
                                       {
                                           Id = depot.Id,
                                           Name = depot.Name + "-" + depot.Code,
                                           Code = depot.Code,
                                           IsPlant = depot.IsPlant,
                                           IsActive = depot.IsActive
                                       }).ToList();

                            //foreach (var plant in depotList)
                            //{
                            //var depotContext = (from plantdepot in _emamiContext.PlantDepotMapping.AsNoTracking()
                            //                    join depot in _emamiContext.Depots.AsNoTracking() on plantdepot.DepotId equals depot.Id
                            //                    where plantdepot.PlantId == plant.Id && !depot.IsPlant && depot.StorageTypeId == (int)DTO.Enums.StorageType.Depot
                            //                    select new DepotDto
                            //                    {
                            //                        Id = depot.Id,
                            //                        Name = depot.Name,
                            //                        Code = depot.Code,
                            //                        IsPlant = depot.IsPlant,
                            //                        IsActive = depot.IsActive
                            //                    }).ToList();

                            //plant.Depotlist = depotContext;


                            //var rakeContext = (from plantdepot in _emamiContext.PlantDepotMapping.AsNoTracking()
                            //                   join depot in _emamiContext.Depots.AsNoTracking() on plantdepot.DepotId equals depot.Id
                            //                   where plantdepot.PlantId == plant.Id && !depot.IsPlant && depot.StorageTypeId == (int)DTO.Enums.StorageType.Rake
                            //                   select new DepotDto
                            //                   {
                            //                       Id = depot.Id,
                            //                       Name = depot.Name,
                            //                       Code = depot.Code,
                            //                       IsPlant = depot.IsPlant,
                            //                       IsActive = depot.IsActive
                            //                   }).ToList();

                            //plant.Rakelist = rakeContext;
                            //   }

                            if (depotList != null && depotList.Any())
                            {
                                PlantDepotList.AddRange(depotList);
                            }
                        }
                    }
                }

                List<DepotDto> list = null;
                if (PlantDepotList != null && PlantDepotList.Any())
                {
                    list = PlantDepotList
                    .GroupBy(a => a.Id)
                    .Select(g => g.First())
                    .ToList();

                }

                return _resultService.SuccessObject(list);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }
        #endregion
        public ResultDto GetZonalHeadList(LoginUserIdDto inputDto)
        {
            _methodName = "GetZonalHeadList";
            var outputDto = new List<DropDownDto>();
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

                //New Reporting to table change
                //outputDto = _emamiContext.Users.Join(_emamiContext.UserRoles, u => u.Id, ur => ur.UserId, (u, ur) => new { Users = u, UserRoles = ur })
                //    .Where(w => (w.UserRoles.RoleId == (int)DTO.Enums.Role.ZonalTrader && w.Users.ReportingToId == inputDto.LoginUserId && w.Users.IsActive))
                //    .Select(s => new DropDownDto()
                //    {
                //        Id = s.Users.Id,
                //        Name = s.Users.Name
                //    }).ToList();

                outputDto = (from u in _emamiContext.Users.AsNoTracking()
                                 //join ur in _emamiContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                             join urm in _emamiContext.UserReportingToMappings.AsNoTracking() on u.Id equals urm.UserId
                             where urm.ReportingToUserId == inputDto.LoginUserId && urm.RoleId == (int)DTO.Enums.Role.ZonalTrader && u.IsActive
                             select new DropDownDto()
                             {
                                 Id = u.Id,
                                 Name = u.Name
                             }).OrderBy(s => s.Name).ToList();

                return _resultService.SuccessObject(outputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #region ZH Dashboard Statistics

        public ResultDto GetZHStatistics(SaudaFilterDto inputDto)
        {
            _methodName = "GetZHStatistics";
            var outputDto = new UserStatisticsOutputDto();
            try
            {
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.UserId <= 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.UserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                if (inputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }
                if (inputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }


                //New Reporting to table change
                //var ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.UserId).Select(_ => _.Id).ToList();
                var ZHList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.UserId).Select(_ => _.UserId).ToList();
                if (ZHList != null && ZHList.Any())
                {
                    var bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();
                    //var bdoList = _emamiContext.Users.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToId ?? 0)).Select(_ => _.Id).ToList();
                    List<long> dealersList = new List<long>();
                    if (bdoList != null && bdoList.Any())
                    {
                        dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    }
                    outputDto.DealersCount = dealersList.Count;

                    var divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.UserId)
                    .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                    var outStandingContextList = new List<PendingContractStatistics>();
                    var NearExpiredContextList = new List<decimal>();
                    var ExpiredContextList = new List<decimal>();
                    if (dealersList != null && dealersList.Any())
                    {
                        #region OldCode

                        //var skipCount = 0;
                        //var takeCount = Config.InConditionTakeCount;
                        //var rowCount = Math.Ceiling(Convert.ToDecimal(dealersList.Count / Convert.ToDecimal(takeCount)));
                        //for (int i = 0; i < rowCount; i++)
                        //{
                        //    var userIds = dealersList.Skip(skipCount).Take(takeCount);
                        //    outputDto.PendingSaudaQuantity += (from p in _emamiContext.PendingContracts.AsNoTracking()
                        //                                       join sku in _emamiContext.Skus.AsNoTracking() on p.MaterialCode equals sku.SkuCode
                        //                                      // join s in _emamiContext.Sauda.AsNoTracking() on p.SaudaNumber equals s.SaudaNumber
                        //                                       join dm in divisionslogieduser on new { SalesOrganizationId = p.SalesOrgId, DistributionChannelId = p.DistChnlId, DivisionId = p.DivisionId }
                        //                                       equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                        //                                       where userIds.Contains(p.UserId)
                        //                                        //&& DbFunctions.TruncateTime(p.ContractValidTo) >= DbFunctions.TruncateTime(currentDate)
                        //                                       //&& (bdoList.Contains(s.BdoId) || s.BdoId == 0)
                        //                                       && p.SalesOrgId == sku.SalesOrganizationId 
                        //                                       && p.DistChnlId == sku.DistributionChannelId
                        //                                                      && p.DivisionId == sku.DivisionId
                        //                                       select p.SaudaQuantity).DefaultIfEmpty(0).Sum();


                        //    var outStandingList = (from p in _emamiContext.PendingContracts.AsNoTracking()
                        //                           join sku in _emamiContext.Skus.AsNoTracking() on p.MaterialCode equals sku.SkuCode
                        //                           //join s in _emamiContext.Sauda.AsNoTracking() on p.SaudaNumber equals s.SaudaNumber
                        //                           join dm in divisionslogieduser on new { SalesOrganizationId = p.SalesOrgId, DistributionChannelId = p.DistChnlId, DivisionId = p.DivisionId }
                        //                           equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                        //                           where userIds.Contains(p.UserId) 
                        //                           //&& (bdoList.Contains(s.BdoId) || s.BdoId==0)
                        //                           && p.SalesOrgId == sku.SalesOrganizationId
                        //                           && p.DistChnlId == sku.DistributionChannelId
                        //                           && p.DivisionId == sku.DivisionId
                        //                           select new { SaudaQuantity = p.SaudaQuantity, ContractValidTo = p.ContractValidTo });
                        //    //.Select(_ => new PendingContractStatistics
                        //    //{
                        //    //    ContractValidTo = _.PendingContract.ContractValidTo,
                        //    //    PendingQuantityInMT = _.PendingContract.SaudaQuantity
                        //    //}).ToList();
                        //    if (outStandingList.IsAny())
                        //    {
                        //       var ExpiredContextdata = outStandingList.Where(_ => DbFunctions.TruncateTime(_.ContractValidTo) < DbFunctions.TruncateTime(currentDate)).Select(a => a.SaudaQuantity).ToList();
                        //       var NearExpiredContextdata = outStandingList.Where(_ => DbFunctions.DiffDays(currentDate, _.ContractValidTo) < 5 && DbFunctions.DiffDays(currentDate, _.ContractValidTo) >= 1).Select(a => a.SaudaQuantity).ToList();
                        //        ExpiredContextList.AddRange(ExpiredContextdata);
                        //        NearExpiredContextList.AddRange(NearExpiredContextdata);
                        //    }
                        //    skipCount += takeCount;
                        //}

                        //    if (ExpiredContextList != null && ExpiredContextList.Any())
                        //    {
                        //        outputDto.AboveOutstandingSaudaQuantity = ExpiredContextList.DefaultIfEmpty(0).Sum();
                        //    }
                        //    if (NearExpiredContextList != null && NearExpiredContextList.Any())
                        //    {
                        //        outputDto.BelowOutstandingSaudaQuantity = NearExpiredContextList.DefaultIfEmpty(0).Sum();
                        //    }

                        #endregion


                        #region NewCode
                        using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                        {
                            var sqlQuery = @"Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                        insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                        select SalesOrganizationId,DistributionChannelId,DivisionId 
                        from UserDivisionMappings where UserId=@UserId
                        select pc.SaudaQuantity as PendingQuantityInMT,pc.ContractValidTo 
                        from PendingContracts pc
                        join Skus sku on  pc.MaterialCode=sku.SkuCode
                        join #UserDivision ud on pc.SalesOrgId=ud.SalesOrganizationId and pc.DistChnlId=ud.DistributionChannelId and pc.DivisionId=ud.DivisionId
                        where 
                        pc.SalesOrgId=sku.SalesOrganizationId and pc.DistChnlId=sku.DistributionChannelId and pc.DivisionId=sku.DivisionId
                        and pc.UserId in (select distinct CustomerId from UserCustomerMappings where UserId in(
                        select UserId from UserReportingToMappings where ReportingToUserId in(
                        select UserId from UserReportingToMappings where ReportingToUserId=@UserId)))
                        drop table #UserDivision";

                            outStandingContextList = conn.Query<PendingContractStatistics>(sqlQuery, new
                            {
                                UserId = inputDto.UserId
                            }).ToList();

                        }
                        outputDto.PendingSaudaQuantity = outStandingContextList.Select(s => s.PendingQuantityInMT).DefaultIfEmpty(0).Sum();
                        outputDto.AboveOutstandingSaudaQuantity = outStandingContextList.Where(_ => _.ContractValidTo.Date < currentDate.Date).Select(a => a.PendingQuantityInMT).DefaultIfEmpty(0).Sum();
                        outputDto.BelowOutstandingSaudaQuantity = outStandingContextList.Where(_ => (_.ContractValidTo.Date - currentDate.Date).Days < 5 && (_.ContractValidTo.Date - currentDate.Date).Days >= 1).Select(a => a.PendingQuantityInMT).DefaultIfEmpty(0).Sum();

                        #endregion
                        var overduePaymentContext = _emamiContext.OverduePayment.AsNoTracking().Where(_ => dealersList.Contains(_.UserId));
                        if (overduePaymentContext != null && overduePaymentContext.Any())
                        {
                            var tomDate = currentDate.AddDays(1);
                            outputDto.TotalDueForTomorrow = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) == DbFunctions.TruncateTime(tomDate)).DefaultIfEmpty().Sum(_ => (decimal?)_.Balance).GetValueOrDefault();
                            outputDto.TotalOverDue = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) <= DbFunctions.TruncateTime(currentDate)).DefaultIfEmpty().Sum(_ => (decimal?)_.Balance).GetValueOrDefault();
                        }


                        //var specialRatesContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => dealersList.Contains(_.UserId) && DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(currentDate)
                        //    && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(currentDate) && _.StatusId == (int)DTO.Enums.Status.Pending).ToList();
                        //if (specialRatesContext != null && specialRatesContext.Any())
                        //{
                        //    outputDto.TotalSpecialRateApproval = specialRatesContext.Count();
                        //}

                        outputDto.TotalSpecialRateApproval = (from s in _emamiContext.Sauda.AsNoTracking()
                                                              join sr in _emamiContext.SpecialRate.AsNoTracking() on s.SpecialRateRequestIdInParentTable equals sr.Id
                                                              join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                                                              equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                                                              where dealersList.Contains(sr.UserId) && DbFunctions.TruncateTime(sr.CreatedDate) >= DbFunctions.TruncateTime(currentDate)
                                                              && DbFunctions.TruncateTime(sr.CreatedDate) <= DbFunctions.TruncateTime(currentDate) && sr.StatusId == (int)DTO.Enums.Status.Pending
                                                              select sr).Count();

                    }
                    //var userRoleContext = (from user in _emamiContext.Users.AsNoTracking()
                    //                       join role in _emamiContext.UserRoles.AsNoTracking() on user.Id equals role.UserId
                    //                       where role.RoleId == (int)DTO.Enums.Role.ZonalTrader
                    //                       select new UserMasterDto
                    //                       {
                    //                           Id = user.Id,
                    //                           EmployeeName = user.Name,
                    //                           EmployeeCode = user.Code
                    //                       }).ToList();

                    //List<MonthDto> months = new List<MonthDto>();
                    //months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);
                    //List<int> MonthIds = months.Select(s => s.Id).Distinct().ToList();
                    //List<long> Years = months.Select(s => (long)s.Year).Distinct().ToList();
                    //var rankList = new List<OverallPerformanceByUserOutputDto>();
                    //if (userRoleContext != null)
                    //{
                    //    foreach (var user in userRoleContext)
                    //    {
                    //        var salesTarget = new OverallPerformanceByUserOutputDto();
                    //        //target
                    //        var totalTarget = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == user.Id
                    //            && MonthIds.Contains(_.MonthId) && Years.Contains(_.Year)).Select(_ => _.Target).DefaultIfEmpty(0).Sum();

                    //        //achievement
                    //        var zhDBOList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == user.Id).Select(_ => _.Id).ToList();
                    //        List<long> zhDealersList = new List<long>();
                    //        if (zhDBOList != null && zhDBOList.Any())
                    //        {
                    //            zhDealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => zhDBOList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    //        }

                    //        var totalAchievement = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.Invoice != null && zhDealersList.Contains(_.Invoice.UserId) && DbFunctions.TruncateTime(_.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                    //            DbFunctions.TruncateTime(_.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)).Select(_ => _.ActualBilledQuantity).DefaultIfEmpty(0).Sum();

                    //        salesTarget.UserId = user.Id;
                    //        salesTarget.AchievmentPercentage = (totalTarget > 0 && totalAchievement > 0) ? (totalAchievement / totalTarget) * 100 : 0;
                    //        rankList.Add(salesTarget);
                    //    }

                    //}
                    //if (rankList != null && rankList.Any())
                    //{
                    //    int rank = 1;
                    //    rankList = rankList.OrderByDescending(o => o.AchievmentPercentage).ToList();
                    //    rankList.ForEach(_ => _.Rank = rank++);
                    //    outputDto.RankTotalUserCount = rankList.Count;
                    //    //outputDto.LoginUserRank = rankList.FirstOrDefault(_ => _.UserId == inputDto.UserId).Rank;
                    //}
                    outputDto.CurrentDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                    //bool isApplySpecialityFatDiscount = false;
                    //var applySpecialityFatDiscount = Utility.GetEnumDescription(DTO.Enums.Configuration.IsApplySpecialityFatDiscount);
                    //var configurationSpecialityFatDiscountContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Name == applySpecialityFatDiscount);
                    //if (configurationSpecialityFatDiscountContext != null)
                    //{
                    //    isApplySpecialityFatDiscount = Convert.ToBoolean(configurationSpecialityFatDiscountContext.Value);
                    //}
                    //outputDto.IsApplySpecialityFatDiscount = isApplySpecialityFatDiscount;
                }
                if (outputDto != null)
                {
                    return _resultService.SuccessObject(outputDto);
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

        #region Sales
        public ResultDto OverallSalesChart(DashboardOverallSaudaInputDto inputDto)
        {
            _methodName = "OverallSalesChart";
            var OutputDto = new List<DashboardOverallSalesOutputDto>();
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
                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }
                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }
                //List<MonthDto> months = new List<MonthDto>();
                //months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);
                //var ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.OrganizationReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                //var bdoList = _emamiContext.Users.AsNoTracking().Where(_ => ZHList.Contains(_.OrganizationReportingToId ?? 0)).Select(_ => _.Id).ToList();
                //List<long> dealersList = new List<long>();
                //if (bdoList != null && bdoList.Any())
                //{
                //    dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                //}

                if (!inputDto.IsShowDealer)
                {
                    //foreach (var item in months)
                    //{
                    //    var dashboardOverallsaudaOutpuDto = new List<DashboardOverallSalesOutputDto>();

                    //    List<long> oilTypeIds = new List<long>();
                    //    List<long> targetOilTypeIds = new List<long>();
                    //    List<long> salesOilTypeIds = new List<long>();
                    //    var targetListContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking()
                    //        .Where(_ => _.AssignedToId == inputDto.LoginUserId
                    //        && _.MonthId == item.Id
                    //        && _.Year == item.Year);
                    //    if (targetListContext != null && targetListContext.Any())
                    //    {
                    //        targetOilTypeIds = targetListContext.Where(_ => _.OilTypeId != 0).Select(_ => _.OilTypeId).ToList();
                    //        oilTypeIds.AddRange(targetOilTypeIds);
                    //    }

                    //    var salesListContext = _emamiContext.Invoices.AsNoTracking()
                    //        .Join(_emamiContext.InvoiceDetails.AsNoTracking(), i => i.Id, ind => ind.InvoiceId, (i, ind) => new { i, ind })
                    //        .Join(_emamiContext.SalesRegister.AsNoTracking(), i => i.ind.InvoiceId, sr => sr.InvoiceId, (i, sr) => new { i = i.i, ind = i.ind, SR = sr })
                    //        .Join(_emamiContext.OilTypes.AsNoTracking(), x => x.ind.OilTypeId, o => o.Id, (x, o) => new { x.ind, x.i, OilTypeName = o.Name, SR = x.SR })
                    //        .Where(_ => _.i != null && dealersList.Contains(_.i.UserId)
                    //        && DbFunctions.TruncateTime(_.i.InvoiceDate) >= DbFunctions.TruncateTime(item.StartDate)
                    //        && DbFunctions.TruncateTime(_.i.InvoiceDate) <= DbFunctions.TruncateTime(item.EndDate))
                    //        .Select(_ => new { _.ind, _.OilTypeName, SR = _.SR.QuantityMT }).ToList();

                    //    if (salesListContext != null && salesListContext.Any())
                    //    {
                    //        salesOilTypeIds = salesListContext.Where(_ => _.ind.OilTypeId != 0).Select(_ => _.ind.OilTypeId).ToList();
                    //        oilTypeIds.AddRange(salesOilTypeIds);
                    //    }

                    //    if (oilTypeIds != null && oilTypeIds.Any())
                    //    {
                    //        oilTypeIds = oilTypeIds.Distinct().ToList();
                    //    }
                    //    if (oilTypeIds != null && oilTypeIds.Any())
                    //    {
                    //        foreach (var oilTypeId in oilTypeIds)
                    //        {
                    //            var acheivment = new DashboardOverallSalesOutputDto();
                    //            if (targetListContext != null && targetListContext.Any())
                    //            {
                    //                var oilTypeTargetListContext = targetListContext.Where(_ => _.OilTypeId == oilTypeId);
                    //                if (oilTypeTargetListContext != null && oilTypeTargetListContext.Any())
                    //                {
                    //                    acheivment.OilTypeId = oilTypeTargetListContext.FirstOrDefault().OilTypeId;
                    //                    acheivment.OilType = oilTypeTargetListContext.FirstOrDefault().OilType.Name;
                    //                    acheivment.TotalTarget = oilTypeTargetListContext.Select(_ => _.Target).DefaultIfEmpty(0).Sum();
                    //                    acheivment.MonthId = item.Id;
                    //                }
                    //            }
                    //            if (salesListContext != null && salesListContext.Any())
                    //            {
                    //                var oilTypeSalesListContext = salesListContext.Where(_ => _.ind.OilTypeId == oilTypeId);
                    //                if (oilTypeSalesListContext != null && oilTypeSalesListContext.Any())
                    //                {
                    //                    acheivment.OilTypeId = oilTypeSalesListContext.FirstOrDefault().ind.OilTypeId;
                    //                    acheivment.OilType = oilTypeSalesListContext.FirstOrDefault().OilTypeName;
                    //                    acheivment.MonthId = item.Id;
                    //                    acheivment.TotalAchievment = oilTypeSalesListContext.Select(_ => _.ind.ActualBilledQuantity).DefaultIfEmpty(0).Sum();
                    //                }
                    //            }
                    //            dashboardOverallsaudaOutpuDto.Add(acheivment);
                    //        }
                    //    }
                    //}

                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {
                        var chartResult = conn.Query<DashboardOverallSalesOutputDto>("Get_NHSalesTargetChartsOilTypeWise",
                            new
                            {
                                LoginUserId = inputDto.LoginUserId,
                                StartDate = inputDto.FromDate,
                                EndDate = inputDto.ToDate,
                                MonthId = inputDto.FromDate.Month,
                                Year = inputDto.FromDate.Year
                            }, commandType: CommandType.StoredProcedure,commandTimeout:300).ToList();

                        OutputDto.AddRange(chartResult);
                    }
                }
                else
                {

                    #region oldCode
                    //var ZHLists = _emamiContext.Users.AsNoTracking().Where(_ => _.OrganizationReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();

                    //foreach (var item in months)
                    //{
                    //    var bdoOverallsaudaOutpuDto = new List<DashboardOverallSalesOutputDto>();
                    //    foreach (var zh in ZHLists)
                    //    {
                    //        var bdoListByZH = _emamiContext.Users.AsNoTracking().Where(_ => _.OrganizationReportingToId == zh).Select(_ => _.Id).ToList();
                    //        if (bdoListByZH != null && bdoListByZH.Any())
                    //        {
                    //            List<long> dealersListBybdo = new List<long>();
                    //            dealersListBybdo = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoListByZH.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    //            var dealersOverallsaudaOutpuDto = new List<DashboardOverallSalesOutputDto>();
                    //            foreach (var dealer in dealersListBybdo)
                    //            {
                    //                var targetContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == dealer && _.MonthId == inputDto.FromDate.Month && _.Year == inputDto.FromDate.Year).ToList();
                    //                if (targetContext != null)
                    //                {
                    //                    var salesContext = (from invoice in _emamiContext.Invoices.AsNoTracking()
                    //                                        join invdetail in _emamiContext.InvoiceDetails.AsNoTracking() on invoice.Id equals invdetail.InvoiceId
                    //                                        where invoice.UserId == dealer
                    //                                         && DbFunctions.TruncateTime(invoice.InvoiceDate) >= DbFunctions.TruncateTime(item.StartDate) &&
                    //                                                DbFunctions.TruncateTime(invoice.InvoiceDate) <= DbFunctions.TruncateTime(item.EndDate)
                    //                                        select invdetail
                    //                                    ).ToList();

                    //                    var acheivment = new DashboardOverallSalesOutputDto
                    //                    {
                    //                        DealerId = dealer,
                    //                        Dealer = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == dealer).Name,
                    //                        TotalTarget = targetContext.Sum(_ => _.Target),
                    //                        TotalAchievment = salesContext.Sum(_ => (decimal?)_.ActualBilledQuantity) ?? 0,
                    //                        AchievmentPercentage = targetContext.Sum(_ => _.Target) > 0 ? (salesContext.Sum(_ => (decimal?)_.ActualBilledQuantity) ?? 0 / targetContext.Sum(_ => _.Target)) * 100 : 0
                    //                    };
                    //                    dealersOverallsaudaOutpuDto.Add(acheivment);
                    //                }
                    //            }
                    //            var bdoAchievement = new DashboardOverallSalesOutputDto
                    //            {
                    //                DealerId = zh,
                    //                Dealer = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == zh).Name,
                    //                TotalTarget = dealersOverallsaudaOutpuDto.Sum(_ => _.TotalTarget),
                    //                TotalAchievment = dealersOverallsaudaOutpuDto.Sum(_ => (decimal?)_.TotalAchievment) ?? 0,
                    //                AchievmentPercentage = dealersOverallsaudaOutpuDto.Sum(_ => _.TotalTarget) > 0 ? (dealersOverallsaudaOutpuDto.Sum(_ => (decimal?)_.TotalAchievment) ?? 0 / dealersOverallsaudaOutpuDto.Sum(_ => _.TotalTarget)) * 100 : 0
                    //            };
                    //            bdoOverallsaudaOutpuDto.Add(bdoAchievement);
                    //        }
                    //    }
                    //    OutputDto.AddRange(bdoOverallsaudaOutpuDto);
                    //}



                    //string query = @"Select DISTINCT nh.Id as ZonalHeadId,nh.Name as ZonalTrader,cus.Id as DealerId,cus.Name as Dealer
                    //                From Users nh
                    //                INNER JOIN Users zh ON nh.Id = zh.OrganizationReportingToId
                    //                INNER JOIN UserCustomerMappings ucm ON ucm.UserId = zh.Id
                    //                INNER JOIN Users cus ON ucm.CustomerId = cus.Id
                    //                Where nh.OrganizationReportingToId = @OrganizationReportingToId
                    //                And zh.Id IS NOT NULL
                    //                And ucm.CustomerId IS NOT NULL";
                    //var userDetails = conn.Query<ZhUserDetailDto>(query,
                    //            new
                    //            {
                    //                OrganizationReportingToId = inputDto.LoginUserId
                    //            }).ToList();

                    //if (userDetails != null && userDetails.Any())
                    //{

                    //    var zonalHeadDatas = userDetails.GroupBy(g => new { g.ZonalHeadId, g.ZonalTrader }).ToList();

                    //    foreach (var item in months)
                    //    {
                    //        var bdoOverallsaudaOutpuDto = new List<DashboardOverallSalesOutputDto>();
                    //        foreach (var zh in zonalHeadDatas)
                    //        {
                    //            var dealersOverallsaudaOutpuDto = new List<DashboardOverallSalesOutputDto>();
                    //            foreach (var dealer in zh)
                    //            {

                    //                string salesTargetQuery = @"Select ISNULL(SUM(Target),0) As SumOfTarget  From UserCustomerSalesTargets 
                    //                                        Where AssignedToId = @AssignedToId
                    //                                        AND MonthId = @MonthId
                    //                                        AND Year = @Year";
                    //                var targetContext = conn.QueryFirstOrDefault<decimal>(salesTargetQuery,
                    //                    new
                    //                    {
                    //                        AssignedToId = dealer.DealerId,
                    //                        MonthId = inputDto.FromDate.Month,
                    //                        Year = inputDto.FromDate.Year
                    //                    });

                    //                string invoiceTargetQuery = @"Select ISNULL(Sum(ActualBilledQuantity),0) As ActualBilledQuantity From Invoices inv 
                    //                                    JOIN InvoiceDetails invd ON inv.Id = invd.InvoiceId
                    //                                    Where inv.UserId = @DealerId
                    //                                    AND CONVERT(VARCHAR, inv.InvoiceDate, 111) >= CONVERT(VARCHAR, @StartDate, 111)
                    //                                    AND CONVERT(VARCHAR, inv.InvoiceDate, 111) <= CONVERT(VARCHAR, @EndDate, 111)";
                    //                var salesContext = conn.QueryFirstOrDefault<decimal>(invoiceTargetQuery,
                    //                    new
                    //                    {
                    //                        DealerId = dealer.DealerId,
                    //                        StartDate = item.StartDate,
                    //                        EndDate = item.EndDate
                    //                    });

                    //                var acheivment = new DashboardOverallSalesOutputDto
                    //                {
                    //                    DealerId = dealer.DealerId,
                    //                    Dealer = dealer.Dealer,
                    //                    TotalTarget = targetContext,
                    //                    TotalAchievment = salesContext,
                    //                    AchievmentPercentage = targetContext > 0 ? (salesContext / targetContext) * 100 : 0
                    //                };
                    //                dealersOverallsaudaOutpuDto.Add(acheivment);
                    //            }

                    //            var bdoAchievement = new DashboardOverallSalesOutputDto
                    //            {
                    //                DealerId = zh.Key.ZonalHeadId,
                    //                Dealer = zh.Key.ZonalTrader,
                    //                TotalTarget = dealersOverallsaudaOutpuDto.Sum(_ => _.TotalTarget),
                    //                TotalAchievment = dealersOverallsaudaOutpuDto.Sum(_ => (decimal?)_.TotalAchievment) ?? 0,
                    //                AchievmentPercentage = dealersOverallsaudaOutpuDto.Sum(_ => _.TotalTarget) > 0 ? (dealersOverallsaudaOutpuDto.Sum(_ => (decimal?)_.TotalAchievment) ?? 0 / dealersOverallsaudaOutpuDto.Sum(_ => _.TotalTarget)) * 100 : 0
                    //            };
                    //            bdoOverallsaudaOutpuDto.Add(bdoAchievement);
                    //        }
                    //        OutputDto.AddRange(bdoOverallsaudaOutpuDto);
                    //    }
                    //}

                    #endregion
                    #region New Code

                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {
                        var chartResult = conn.Query<DashboardOverallSalesOutputDto>("Get_NHSalesTargetChartsDealerWise",
                                    new
                                    {
                                        LoginUserId = inputDto.LoginUserId,
                                        StartDate = inputDto.FromDate,
                                        EndDate = inputDto.ToDate,
                                        MonthId = inputDto.FromDate.Month,
                                        Year = inputDto.FromDate.Year
                                    }, commandType: CommandType.StoredProcedure, commandTimeout: 300).ToList();

                        OutputDto.AddRange(chartResult);


                    }

                    #endregion


                }

                var result = new NewDashboardOverallSalesOutputDto();
                result.SalesList = OutputDto;
                result.TotalTarget = OutputDto.FirstOrDefault().OverallTarget;
                result.OverallSales = OutputDto.FirstOrDefault().OverallAchievment;

                return _resultService.SuccessObject(result);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto DashboardPackwiseSaleslist(DashboardOverallSaudaInputDto inputDto)
        {
            var dashboardOutputDto = new List<DashboardDetailsByDealersOutputDto>();
            _methodName = "DashboardPackwiseSaleslist";
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                //Sales person
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var roleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                if (roleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }
                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }
                List<long> bdoList = new List<long>();
                if (inputDto.ZHId > 0)
                {
                    //New Reporting to table change
                    var bdos = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.ZHId).Select(_ => _.UserId).ToList();
                    //var bdos = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.ZHId).Select(_ => _.Id).ToList();
                    bdoList.AddRange(bdos);
                }
                else
                {
                    //New Reporting to table change
                    var ZHList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                    bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();

                    //var ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                    //bdoList = _emamiContext.Users.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToId ?? 0)).Select(_ => _.Id).ToList();
                }
                List<long> dealersList = new List<long>();
                if (bdoList != null && bdoList.Any())
                {
                    dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                }
                List<MonthDto> months = new List<MonthDto>();
                months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);
                List<int> MonthIds = months.Select(s => s.Id).Distinct().ToList();
                List<long> Years = months.Select(s => (long)s.Year).Distinct().ToList();
                if (dealersList != null && dealersList.Any())
                {


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
                        divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                     .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });
                    }


                    #region OldCode
                    //var invoiceDetailsContextList = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.Invoice != null && DbFunctions.TruncateTime(_.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                    //  DbFunctions.TruncateTime(_.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && dealersList.Contains(_.Invoice.UserId));
                    //List<long> dealerIds = new List<long>();
                    //if (invoiceDetailsContextList != null && invoiceDetailsContextList.Any())
                    //{
                    //    dealerIds = invoiceDetailsContextList.Select(_ => _.Invoice.UserId).Distinct().ToList();
                    //}
                    //foreach (var dealerId in dealerIds)
                    //{
                    //    var salesDetail = new DashboardDetailsByDealersOutputDto();
                    //    salesDetail.DealerId = dealerId;
                    //    var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == dealerId);
                    //    if (dealerContext != null)
                    //    {
                    //        salesDetail.Dealer = dealerContext.Name;
                    //        var cityContext = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == dealerContext.CityId);
                    //        if (cityContext != null)
                    //        {
                    //            salesDetail.TownName = cityContext.CityName;
                    //        }
                    //    }

                    //    var totalTarget = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == dealerId
                    //        && MonthIds.Contains(_.MonthId) && Years.Contains(_.Year)).Select(_ => _.Target).DefaultIfEmpty(0).Sum();

                    //    salesDetail.Target = totalTarget;

                    //    var invoiceTotal = invoiceDetailsContextList.AsNoTracking().Join(_emamiContext.Skus.AsNoTracking(), i => i.SkuId, s => s.Id, (i, s) => new { i, s })
                    //        .Where(_ => (inputDto.IsBulkPack == true ? _.s.PackGroupId == (int)DTO.Enums.PackGroupType.BulkPacking : _.s.PackGroupId == (int)DTO.Enums.PackGroupType.ConsumerPacking)
                    //        && _.i.Invoice != null && _.i.Invoice.UserId == dealerId).Select(_ => _.i.ActualBilledQuantity).DefaultIfEmpty(0).Sum();

                    //    salesDetail.Achievement = invoiceTotal;

                    //    dashboardOutputDto.Add(salesDetail);
                    //}
                    #endregion

                    #region 27-12-2019 Code Comment
                    //var invoiceDetailsContextList = _emamiContext.InvoiceDetails.AsNoTracking()
                    //    .Join(_emamiContext.SalesRegister.AsNoTracking(), invd => invd.InvoiceId, sr => sr.InvoiceId, (invd, sr) => new { InvoiceDetails = invd, SalesRegister = sr })
                    //    .Where(_ => _.InvoiceDetails.Invoice != null && DbFunctions.TruncateTime(_.InvoiceDetails.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //    && DbFunctions.TruncateTime(_.InvoiceDetails.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    //    && dealersList.Contains(_.InvoiceDetails.Invoice.UserId));
                    //List<long> dealerIds = new List<long>();
                    //if (invoiceDetailsContextList != null && invoiceDetailsContextList.Any())
                    //{
                    //    dealerIds = invoiceDetailsContextList.Select(_ => _.InvoiceDetails.Invoice.UserId).Distinct().ToList();
                    //}
                    //foreach (var dealerId in dealerIds)
                    //{
                    //    var salesDetail = new DashboardDetailsByDealersOutputDto();
                    //    salesDetail.DealerId = dealerId;
                    //    var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == dealerId);
                    //    if (dealerContext != null)
                    //    {
                    //        salesDetail.Dealer = dealerContext.Name;
                    //        var cityContext = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == dealerContext.CityId);
                    //        if (cityContext != null)
                    //        {
                    //            salesDetail.TownName = cityContext.CityName;
                    //        }
                    //    }

                    //    var totalTarget = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == dealerId
                    //        && MonthIds.Contains(_.MonthId) && Years.Contains(_.Year)).Select(_ => _.Target).DefaultIfEmpty(0).Sum();

                    //    salesDetail.Target = totalTarget;

                    //    var invoiceTotal = invoiceDetailsContextList.AsNoTracking().Join(_emamiContext.Skus.AsNoTracking(), i => i.InvoiceDetails.SkuId, s => s.Id, (i, s) => new { i, s })
                    //        .Where(_ => (inputDto.IsBulkPack == true ? _.s.PackGroupId == (int)DTO.Enums.PackGroupType.BulkPacking : _.s.PackGroupId == (int)DTO.Enums.PackGroupType.ConsumerPacking)
                    //        && _.i.InvoiceDetails.Invoice != null && _.i.InvoiceDetails.Invoice.UserId == dealerId).Select(_ => _.i.SalesRegister.QuantityMT).DefaultIfEmpty(0).Sum();

                    //    salesDetail.Achievement = invoiceTotal;

                    //    dashboardOutputDto.Add(salesDetail);
                    //} 

                    //var invoiceDetailsContextList = _emamiContext.SalesRegister.AsNoTracking()
                    //    .Join(_emamiContext.Skus.AsNoTracking(), sr => sr.MaterialCode, sk => sk.SkuCode, (sr, sk) => new { SalesRegister = sr, Sku = sk })
                    //    .Join(_emamiContext.Users.AsNoTracking(), sr => sr.SalesRegister.CustomerCode, us => us.Code, (sr, us) => new { SalesRegister = sr.SalesRegister, Sku = sr.Sku, User = us })
                    //    .Where(w => DbFunctions.TruncateTime(w.SalesRegister.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //    && DbFunctions.TruncateTime(w.SalesRegister.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    //    && dealersList.Contains(w.User.Id)
                    //    && w.SalesRegister.SalesOrganizationId == w.Sku.SalesOrganizationId && w.SalesRegister.DistributionChannelId == w.Sku.DistributionChannelId
                    //    && w.SalesRegister.DivisionId == w.Sku.DivisionId
                    //    //&& w.User.DivisionId == userContext.DivisionId
                    //    )
                    //    .Select(s => new
                    //    {
                    //        PackGroupId = s.Sku.PackGroupId,
                    //        QuantityMT = s.SalesRegister.QuantityMT,
                    //        UserId = s.User.Id,
                    //        SkuId = s.Sku.Id
                    //    });
                    #endregion
                    #region 24/11/2022

                    //    var invoiceDetailsContextList = (from s in _emamiContext.SalesRegister.AsNoTracking()
                    //                                     join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                    //                                     join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                    //                                     join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                    //                                              equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                    //                                     where DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //                 && DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    //                 && dealersList.Contains(u.Id)
                    //                 && s.SalesOrganizationId == sku.SalesOrganizationId && s.DistributionChannelId == sku.DistributionChannelId
                    //                 && s.DivisionId == sku.DivisionId
                    //                 && s.SkuId > 0

                    //                                     select new
                    //                                     {
                    //                                         PackGroupId = sku.PackGroupId,
                    //                                         QuantityMT = s.QuantityMT,
                    //                                         UserId = u.Id,
                    //                                         SkuId = sku.Id
                    //                                     }
                    //                  );



                    //    List<long> dealerIds = new List<long>();
                    //    if (invoiceDetailsContextList != null && invoiceDetailsContextList.Any())
                    //    {
                    //        dealerIds = invoiceDetailsContextList.Select(_ => _.UserId).Distinct().ToList();

                    //        var dealerContextDatas = _emamiContext.Users.AsNoTracking().Where(_ => dealerIds.Contains(_.Id))
                    //        .Select(s => new { Id = s.Id, Name = s.Name, CityId = s.CityId }).ToList();

                    //        if (dealerIds != null && dealerIds.Any())
                    //        {
                    //            foreach (var dealerId in dealerIds)
                    //            {
                    //                var salesDetail = new DashboardDetailsByDealersOutputDto();
                    //                salesDetail.DealerId = dealerId;
                    //                var dealerContext = (dealerContextDatas != null && dealerContextDatas.Any()) ?
                    //                    dealerContextDatas.FirstOrDefault(_ => _.Id == dealerId) : null;
                    //                if (dealerContext != null)
                    //                {
                    //                    salesDetail.Dealer = dealerContext.Name;
                    //                    var cityContext = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == dealerContext.CityId);
                    //                    if (cityContext != null)
                    //                    {
                    //                        salesDetail.TownName = cityContext.CityName;
                    //                    }
                    //                }

                    //                salesDetail.Target = _emamiContext.UserCustomerSalesTarget.AsNoTracking()
                    //                    .Where(_ => _.AssignedToId == dealerId
                    //                    && MonthIds.Contains(_.MonthId)
                    //                    && Years.Contains(_.Year))
                    //                    .Select(_ => _.Target).DefaultIfEmpty(0).Sum();

                    //                salesDetail.Achievement = invoiceDetailsContextList
                    //                    .Where(_ => (inputDto.PackGroupId > 0 ? _.PackGroupId == inputDto.PackGroupId : _.PackGroupId == _.PackGroupId)
                    //                   && _.UserId == dealerId)
                    //                   .Select(_ => _.QuantityMT).DefaultIfEmpty(0).Sum();

                    //                dashboardOutputDto.Add(salesDetail);
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        return _resultService.ErrorMessage(Constants.RecordNotFound);
                    //    }
                    //}
                    #endregion


                    IEnumerable<DashboardSalesDto> invoiceDetailsContextList = new List<DashboardSalesDto>();
                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {

                        var sqlQuery = @"Create Table #DealerIdsTemp(DealerId bigint)
Create Table #BdoId(BdoId bigint)
Create Table #ZHId(ZHId bigint)
Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)

insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

if(@ZHId >0)
begin
	insert into #ZHId(ZHId) select @ZHId

	insert into #BdoId(BdoId) select UserId from UserReportingToMappings where ReportingToUserId in (select ZHId from #ZHId)
end
else
begin
	insert into #ZHId(ZHId) select UserId from UserReportingToMappings where ReportingToUserId=@UserId

	insert into #BdoId(BdoId) select UserId from UserReportingToMappings where ReportingToUserId in (select ZHId from #ZHId)
end



insert into #DealerIdsTemp(DealerId) select CustomerId from UserCustomerMappings where UserId in (select BdoId from #BdoId)

select
u.Id as UserId,
Sum(s.QuantityMT) as QuantityMT
from SalesRegisters s with(NOLOCK)
join Skus sku on s.MaterialCode=sku.SkuCode and s.SalesOrganizationId=sku.SalesOrganizationId
and s.DistributionChannelId=sku.DistributionChannelId and s.DivisionId=sku.DivisionId
join Users u on s.CustomerCode=u.Code
join #UserDivision ud on s.SalesOrganizationId=ud.SalesOrganizationId and s.DistributionChannelId=ud.DistributionChannelId
and s.DivisionId=ud.DivisionId
where 
u.Id in (select DealerId from #DealerIdsTemp)
and Cast(s.InvoiceDate as date) <= Cast(@ToDate as date)
and Cast(s.InvoiceDate as date) >= Cast(@FromDate as date)
and ((@PackGroupId > 0 and sku.PackGroupId=@PackGroupId) or @PackGroupId=0)
group by u.Id

drop table #BdoId
drop table #DealerIdsTemp
drop table #UserDivision
drop table #ZHId";
                        invoiceDetailsContextList = conn.Query<DashboardSalesDto>(sqlQuery, new
                        {
                            UserId = inputDto.LoginUserId,
                            FromDate = inputDto.FromDate,
                            ToDate = inputDto.ToDate,
                            ZHId = inputDto.ZHId,
                            inputDto.PackGroupId
                        });

                    }

                    //var invoiceDetailsContextList = (from s in _emamiContext.SalesRegister.AsNoTracking()
                    //                                 join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                    //                                 join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                    //                                 join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                    //                                          equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                    //                                 where DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //             && DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    //             && dealersList.Contains(u.Id)
                    //             && s.SalesOrganizationId == sku.SalesOrganizationId && s.DistributionChannelId == sku.DistributionChannelId
                    //             && s.DivisionId == sku.DivisionId
                    //             && s.SkuId > 0
                    //             && (inputDto.PackGroupId > 0 ? sku.PackGroupId == inputDto.PackGroupId : inputDto.PackGroupId == 0)
                    //                                 group s by s.UserId into sales
                    //                                 select new { dealerId = sales.Key, InvoiceSum = sales.Sum(x => x.QuantityMT) }
                    //              );

                    var ilist = invoiceDetailsContextList.ToList();


                    var targetSum = (from ust in _emamiContext.UserCustomerSalesTarget.AsNoTracking()
                                     where dealersList.Contains((long)ust.AssignedToId)
                             && MonthIds.Contains(ust.MonthId)
                             && Years.Contains(ust.Year)
                                     group ust by ust.AssignedToId into target
                                     select new { dealerId = target.Key, Target = target.Sum(x => x.Target) }
                                );

                    var salesDetail = (from i in invoiceDetailsContextList
                                       join u in _emamiContext.Users.AsNoTracking() on i.UserId equals u.Id
                                       join c in _emamiContext.City.AsNoTracking() on u.CityId equals c.Id into tmpCity
                                       //join i in invoiceDetailsContextList on d equals i.dealerId into tmpsum
                                       join t in targetSum on i.UserId equals t.dealerId into tmptarget
                                       from t in tmptarget.DefaultIfEmpty()
                                       from c in tmpCity.DefaultIfEmpty()
                                       select new DashboardDetailsByDealersOutputDto()
                                       {
                                           Target = t != null ? t.Target : 0,
                                           Achievement = i != null ? i.QuantityMT : 0,
                                           DealerId = i.UserId,
                                           Dealer = u.Name,
                                           TownName = c != null ? c.CityName : String.Empty
                                       }
                                 ).OrderBy(s => s.Dealer).ToList();
                    return _resultService.SuccessObject(salesDetail);

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

        public ResultDto OverallSaleslistByDealers(DashboardOverallSaudaInputDto inputDto)
        {
            _methodName = "OverallSaleslistByDealers";
            var dashboardDetailsByDealersOutputDto = new List<DashboardDetailsByDealersOutputDto>();
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
                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }
                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }
                List<long> bdoList = new List<long>();
                if (inputDto.ZHId > 0)
                {
                    //New Reporting to table change
                    //var bdos = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.ZHId).Select(_ => _.Id).ToList();
                    var bdos = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.ZHId).Select(_ => _.UserId).ToList();
                    bdoList.AddRange(bdos);
                }
                else
                {
                    //New Reporting to table change
                    //var ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                    //bdoList = _emamiContext.Users.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToId ?? 0)).Select(_ => _.Id).ToList();

                    var ZHList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                    bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();
                }
                List<long> dealersList = new List<long>();
                if (bdoList != null && bdoList.Any())
                {
                    dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
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
                    divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                 .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });
                }

                List<MonthDto> months = new List<MonthDto>();
                months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);
                List<int> MonthIds = months.Select(s => s.Id).Distinct().ToList();
                List<long> Years = months.Select(s => (long)s.Year).Distinct().ToList();

                var targetSum1 = (from ust in _emamiContext.UserCustomerSalesTarget.AsNoTracking()
                                  where dealersList.Contains((long)ust.AssignedToId)
                          && MonthIds.Contains(ust.MonthId)
                          && Years.Contains(ust.Year)
                                  group ust by ust.AssignedToId into target
                                  select new { dealerId = target.Key, Target = target.Sum(x => x.Target) }
                                );



                var invoiceSum1 = (from s in _emamiContext.SalesRegister.AsNoTracking()
                                   join sk in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sk.SkuCode
                                   join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                                   join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                                          equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                   where DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                                   && DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                                   && dealersList.Contains(u.Id)
                                   && sk.SalesOrganizationId == s.SalesOrganizationId
                                   && sk.DistributionChannelId == s.DistributionChannelId
                                   && sk.DivisionId == s.DivisionId
                                   group new { s, u } by u.Id into sales
                                   select new { dealerId = sales.Key, InvoiceSum = sales.Sum(x => x.s.QuantityMT) }
                                      );

                var outputDto = (from d in dealersList
                                 join u in _emamiContext.Users.AsNoTracking() on d equals u.Id
                                 join c in _emamiContext.City.AsNoTracking() on u.CityId equals c.Id into tmpCity
                                 join t in targetSum1 on d equals t.dealerId into tmptarget
                                 join i in invoiceSum1 on d equals i.dealerId into tmpsum
                                 from t in tmptarget.DefaultIfEmpty()
                                 from i in tmpsum.DefaultIfEmpty()
                                 from c in tmpCity.DefaultIfEmpty()
                                 select new DashboardDetailsByDealersOutputDto()
                                 {
                                     Target = t != null ? t.Target : 0,
                                     Achievement = i != null ? i.InvoiceSum : 0,
                                     DealerId = d,
                                     Dealer = u.Name,
                                     TownName = c != null ? c.CityName : String.Empty
                                 }
                                 ).ToList();
                #region oldCode
                //foreach (var dealerId in dealersList)
                //{
                //    DashboardDetailsByDealersOutputDto salesTarget = new DashboardDetailsByDealersOutputDto();
                //    var targetSum = _emamiContext.UserCustomerSalesTarget.AsNoTracking()
                //        .Where(_ => dealerId == _.AssignedToId 
                //        && MonthIds.Contains(_.MonthId) 
                //        && Years.Contains(_.Year))
                //        .Select(s => s.Target).DefaultIfEmpty(0).Sum();
                //    salesTarget.Target = targetSum;



                //    var invoiceSum = (from s in _emamiContext.SalesRegister.AsNoTracking()
                //                      join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                //                      join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                //                             equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                //                      where DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                //      && DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                //      && u.Id == dealerId
                //      select s.QuantityMT
                //                      ).DefaultIfEmpty(0).Sum();
                //    #region OldCode
                //    //var invoiceSum = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.Invoice != null && DbFunctions.TruncateTime(_.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                //    //    && DbFunctions.TruncateTime(_.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && _.Invoice.UserId == dealerId)
                //    //    .Select(_ => _.ActualBilledQuantity).DefaultIfEmpty(0).Sum();

                //    //var invoiceSum = _emamiContext.InvoiceDetails.AsNoTracking()
                //    //    .Join(_emamiContext.SalesRegister.AsNoTracking(), invd => invd.InvoiceId, sr => sr.InvoiceId, (invd, sr) => new { InvoiceDetails = invd, SalesRegister = sr })
                //    //    .Where(_ => _.InvoiceDetails.Invoice != null
                //    //    && DbFunctions.TruncateTime(_.InvoiceDetails.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                //    //  && DbFunctions.TruncateTime(_.InvoiceDetails.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                //    //  && _.InvoiceDetails.Invoice.UserId == dealerId
                //    //  && _.InvoiceDetails.Invoice.UserId == dealerId)
                //    //  .Select(_ => _.SalesRegister.QuantityMT).DefaultIfEmpty(0).Sum();

                //    //var invoiceSum1 = _emamiContext.SalesRegister.AsNoTracking()
                //    //    .Join(_emamiContext.Users.AsNoTracking(), sr => sr.CustomerCode, us => us.Code, (sr, us) => new { SalesRegister = sr, User = us })
                //    //    .Where(_ => DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                //    //  && DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                //    //  && _.User.Id == dealerId 
                //    //  //&& _.User.DivisionId == userContext.DivisionId
                //    //  )
                //    //  .Select(_ => _.SalesRegister.QuantityMT).DefaultIfEmpty(0).Sum();
                //    #endregion

                //salesTarget.Achievement = invoiceSum;
                //var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == dealerId);
                //if (dealerContext != null)
                //{
                //    salesTarget.DealerId = dealerId;
                //    salesTarget.Dealer = dealerContext.Name;
                //    salesTarget.TownName = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == dealerContext.Id) != null ? _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == dealerContext.Id).CityName : String.Empty;
                //}
                //dashboardDetailsByDealersOutputDto.Add(salesTarget);
                //}
                #endregion

                //cityContext.FirstOrDefault(_ => _.Id == u.CityId) != null ? cityContext.FirstOrDefault(_ => _.Id == u.CityId).CityName : String.Empty
                return _resultService.SuccessObject(outputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetTotalCreditLimit(CreditLimitInputDto inputDto)
        {
            var creditLimitTotalDto = new CreditLimitTotalDto();
            _methodName = "GetTotalCreditLimit";
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                //Sales person
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }
                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }

                var rolesContext = _emamiContext.UserRoles;
                long roleId = rolesContext.FirstOrDefault(_ => _.UserId == inputDto.LoginUserId).RoleId;

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

                List<long> ZHList = new List<long>();
                if (inputDto.NationalHeadIds.IsAny())
                {
                    //New Reporting to table change
                    //ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId != null && inputDto.NationalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    ZHList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => inputDto.NationalHeadIds.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();
                }
                else
                {

                    if (roleId == (int)DTO.Enums.Role.Admin)
                    {
                        var nationalHeadIds = rolesContext.Where(_ => _.RoleId == (int)DTO.Enums.Role.NationalTrader).Select(_ => _.UserId).ToList();
                        //New Reporting to table change
                        //ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId != null && nationalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                        ZHList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => nationalHeadIds.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();
                    }
                }

                //var ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.OrganizationReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                if (ZHList != null && ZHList.Any())
                {
                    //New Reporting to table change
                    //var bdoList = _emamiContext.Users.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToId ?? 0)).Select(_ => _.Id).ToList();
                    var bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();

                    List<long> dealersList = new List<long>();
                    if (bdoList != null && bdoList.Any())
                    {
                        dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    }
                    if (dealersList != null && dealersList.Any())
                    {

                        var userCreditListContext = (from ucm in _emamiContext.UserCreditMaster.AsNoTracking()
                                                     join ud in divisionslogieduser on new { SalesOrganizationId = ucm.SalesOrgId, DistributionChannelId = ucm.DistChnlId, DivisionId = ucm.DivisionId }
                                                       equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                                     where dealersList.Contains(ucm.UserId)
                                                     //&& ucm.Isactive
                                                     && ucm.CreditAccountNumber != null
                                                     select ucm
                                          );
                        //var userCreditListContext = _emamiContext.UserCreditMaster.AsNoTracking().Where(_ => dealersList.Contains(_.UserId) 
                        //&& _.Isactive && _.CreditAccountNumber != null).ToList();
                        if (userCreditListContext != null && userCreditListContext.Any())
                        {
                            creditLimitTotalDto.DealersCount = userCreditListContext.Count();
                            creditLimitTotalDto.TotalCreditLimit = userCreditListContext.Sum(_ => _.CreditLimit);
                            creditLimitTotalDto.TotalCreditExposure = userCreditListContext.Sum(_ => _.CreditExposure);
                        }
                        //var invoiceDetailsContextList = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.Invoice != null && DbFunctions.TruncateTime(_.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                        // DbFunctions.TruncateTime(_.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && dealersList.Contains(_.Invoice.UserId));
                        //if (invoiceDetailsContextList != null && invoiceDetailsContextList.Any())
                        //{
                        //    var bulkInvoiceDetailsContextList = invoiceDetailsContextList.Join(_emamiContext.Skus.AsNoTracking(), i => i.SkuId, s => s.Id, (i, s) => new { i, s })
                        //     .Where(_ => _.s.PackGroupId == (int)DTO.Enums.PackGroupType.BulkPacking);
                        //    if (bulkInvoiceDetailsContextList != null && bulkInvoiceDetailsContextList.Any())
                        //    {
                        //        creditLimitTotalDto.TotalBulkPack = bulkInvoiceDetailsContextList.Sum(_ => _.i.ActualBilledQuantity);
                        //    }
                        //    var customInvoiceDetailsContextList = invoiceDetailsContextList.Join(_emamiContext.Skus.AsNoTracking(), i => i.SkuId, s => s.Id, (i, s) => new { i, s })
                        //     .Where(_ => _.s.PackGroupId == (int)DTO.Enums.PackGroupType.ConsumerPacking);
                        //    if (customInvoiceDetailsContextList != null && customInvoiceDetailsContextList.Any())
                        //    {
                        //        creditLimitTotalDto.TotalCustomPack = customInvoiceDetailsContextList.Sum(_ => _.i.ActualBilledQuantity);
                        //    }
                        //}

                        #region Comment Date 27-12-2019
                        //var invoiceDetailsContextList = _emamiContext.InvoiceDetails.AsNoTracking()
                        //    .Join(_emamiContext.SalesRegister.AsNoTracking(), invd => invd.InvoiceId, sr => sr.InvoiceId, (invd, sr) => new { InvoiceDetails = invd, SalesRegister = sr })
                        //    .Where(_ => _.InvoiceDetails.Invoice != null
                        //    && DbFunctions.TruncateTime(_.InvoiceDetails.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                        //    && DbFunctions.TruncateTime(_.InvoiceDetails.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                        //    && dealersList.Contains(_.InvoiceDetails.Invoice.UserId));

                        //if (invoiceDetailsContextList != null && invoiceDetailsContextList.Any())
                        //{
                        //    var bulkInvoiceDetailsContextList = invoiceDetailsContextList.Join(_emamiContext.Skus.AsNoTracking(), i => i.InvoiceDetails.SkuId, s => s.Id, (i, s) => new { i, s })
                        //     .Where(_ => _.s.PackGroupId == (int)DTO.Enums.PackGroupType.BulkPacking);
                        //    if (bulkInvoiceDetailsContextList != null && bulkInvoiceDetailsContextList.Any())
                        //    {
                        //        creditLimitTotalDto.TotalBulkPack = bulkInvoiceDetailsContextList.Select(_ => _.i.SalesRegister.QuantityMT).DefaultIfEmpty(0).Sum();
                        //    }
                        //    var customInvoiceDetailsContextList = invoiceDetailsContextList.Join(_emamiContext.Skus.AsNoTracking(), i => i.InvoiceDetails.SkuId, s => s.Id, (i, s) => new { i, s })
                        //     .Where(_ => _.s.PackGroupId == (int)DTO.Enums.PackGroupType.ConsumerPacking);
                        //    if (customInvoiceDetailsContextList != null && customInvoiceDetailsContextList.Any())
                        //    {
                        //        creditLimitTotalDto.TotalCustomPack = customInvoiceDetailsContextList.Select(_ => _.i.SalesRegister.QuantityMT).DefaultIfEmpty(0).Sum();
                        //    }
                        //} 
                        #endregion

                        var invoiceDetailsContextList = (from s in _emamiContext.SalesRegister.AsNoTracking()
                                                         join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                                                         join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                                                         join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                                                              equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                                         where DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                                          && DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                                          && dealersList.Contains(u.Id)
                                          && s.SalesOrganizationId == sku.SalesOrganizationId && s.DistributionChannelId == sku.DistributionChannelId
                                          && s.DivisionId == sku.DivisionId
                                          && s.SkuId > 0
                                                         select new { PackGroupId = sku.PackGroupId, QuantityMT = s.QuantityMT }
                                       );

                        //var invoiceDetailsContextList = _emamiContext.SalesRegister.AsNoTracking()
                        //.Join(_emamiContext.Skus.AsNoTracking(), sr => sr.MaterialCode, sk => sk.SkuCode, (sr, sk) => new { SalesRegister = sr, Sku = sk })
                        //.Join(_emamiContext.Users.AsNoTracking(), sr => sr.SalesRegister.CustomerCode, us => us.Code, (sr, us) => new { SalesRegister = sr.SalesRegister, Sku = sr.Sku, User = us })
                        //.Where(w => DbFunctions.TruncateTime(w.SalesRegister.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                        //&& DbFunctions.TruncateTime(w.SalesRegister.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                        //&& dealersList.Contains(w.User.Id)
                        //&& w.SalesRegister.SalesOrganizationId == w.Sku.SalesOrganizationId && w.SalesRegister.DistributionChannelId == w.Sku.DistributionChannelId
                        //&& w.SalesRegister.DivisionId == w.Sku.DivisionId
                        //)
                        //.Select(s => new
                        //{
                        //    PackGroupId = s.Sku.PackGroupId,
                        //    QuantityMT = s.SalesRegister.QuantityMT
                        //});

                        if (invoiceDetailsContextList != null && invoiceDetailsContextList.Any())
                        {
                            if (inputDto.PackGroupId > 0)
                            {
                                var bulkInvoiceDetailsContextList = invoiceDetailsContextList
                                 .Where(_ => _.PackGroupId == inputDto.PackGroupId);
                                if (bulkInvoiceDetailsContextList != null && bulkInvoiceDetailsContextList.Any())
                                {
                                    creditLimitTotalDto.TotalPack = bulkInvoiceDetailsContextList.Select(_ => _.QuantityMT).DefaultIfEmpty(0).Sum();
                                }
                            }
                            else
                            {
                                var customInvoiceDetailsContextList = invoiceDetailsContextList;
                                if (customInvoiceDetailsContextList != null && customInvoiceDetailsContextList.Any())
                                {
                                    creditLimitTotalDto.TotalPack = customInvoiceDetailsContextList.Select(_ => _.QuantityMT).DefaultIfEmpty(0).Sum();
                                }
                            }
                        }

                        return _resultService.SuccessObject(creditLimitTotalDto);
                    }
                    else
                    {
                        return _resultService.ErrorMessage(Constants.RecordNotFound);
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
        #endregion

        #region Special Rate
        public ResultDto GetSpecialRateRequestList(SpecialRateInputDto inputDto)
        {
            var specialRateListDto = new List<SpecialRateOutputDto>();
            _methodName = "GetSpecialRateRequestList";
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                //Sales person
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                IQueryable<SpecialRate> specialRateListContext = null;

                var specialRateApproval = _emamiContext.SpecialRateApproval.AsNoTracking().Where(_ => _.RequestedTo == inputDto.LoginUserId || _.CreatedBy == inputDto.LoginUserId);
                List<long> specialRateIds = specialRateApproval.Select(_ => _.SpecialRateId).Distinct().ToList();

                if (inputDto.DealerId != null && inputDto.OilTypeId != null && inputDto.FromDate.HasValue && inputDto.ToDate.HasValue)
                {
                    specialRateListContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => _.UserId == inputDto.DealerId
                            && _.OilTypeId == inputDto.OilTypeId && _.CreatedDate >= inputDto.FromDate && _.CreatedDate <= inputDto.ToDate && specialRateIds.Contains(_.Id));
                }
                //else if ((inputDto.DealerId != 0 && inputDto.DealerId != null) || (inputDto.OilTypeId != 0 && inputDto.OilTypeId != null)
                //    || (inputDto.FromDate.HasValue && inputDto.FromDate != DateTime.MinValue) || (inputDto.ToDate.HasValue && inputDto.ToDate != DateTime.MinValue))
                //{
                //    return _resultService.ErrorMessage(Constants.RecordNotFound);
                //}
                else
                {
                    List<long> bdoList = new List<long>();
                    if (inputDto.ZHId > 0)
                    {
                        var bdos = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.ZHId).Select(_ => _.Id).ToList();
                        bdoList.AddRange(bdos);
                    }
                    else
                    {
                        var ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                        bdoList = _emamiContext.Users.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToId ?? 0)).Select(_ => _.Id).ToList();
                    }
                    if (bdoList != null && bdoList.Any())
                    {
                        List<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                        if (dealersList != null && dealersList.Any())
                        {
                            if (dealersList != null && dealersList.Any())
                            {
                                specialRateListContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => dealersList.Contains(_.UserId) && DbFunctions.TruncateTime(_.CreatedDate) >= inputDto.FromDate && DbFunctions.TruncateTime(_.CreatedDate) <= inputDto.ToDate && specialRateIds.Contains(_.Id));
                            }
                        }
                    }
                }
                if (specialRateListContext != null && specialRateListContext.Any())
                {
                    var specialRateList = specialRateListContext.Join(_emamiContext.Users.AsNoTracking(), sr => sr.UserId, u => u.Id, (sr, u) => new { sr, u })
                        .Join(_emamiContext.UserRoles.AsNoTracking(), sru => sru.u.Id, ur => ur.UserId, (sru, ur) => new { sru.sr, sru.u, ur }).Where(_ => _.sr != null && _.u != null && _.ur != null)
                        .OrderByDescending(o => o.sr.CreatedDate).ToList();
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

        public ResultDto SpecialRateApproval(SpecialRateApprovalDto inputDto)
        {
            _methodName = "SpecialRateApproval";
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
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                var result = _emamiContext.SpecialRate.FirstOrDefault(_ => _.Id == inputDto.Id);
                if (result == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                if (inputDto.StatusId == (int)DTO.Enums.Status.Approved || inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                {
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
                if (result != null && (result.StatusId == (int)DTO.Enums.Status.Pending || result.StatusId == (int)DTO.Enums.Status.RequestForApproval))
                {
                    var input = new SpecialRateApproval
                    {
                        SpecialRateId = inputDto.Id,
                        RequestedBy = inputDto.LoginUserId,
                        RequestedTo = inputDto.RequestedTo,
                        ApprovedBy = inputDto.LoginUserId,
                        StatusId = inputDto.StatusId,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    };
                    _emamiContext.SpecialRateApproval.Add(input);
                    _emamiContext.SaveChanges();

                    result.StatusId = inputDto.StatusId;
                    result.Remarks = inputDto.Remarks;
                    _emamiContext.SaveChanges();

                    #region Send Email and SMS

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
                            AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                            var emailSubject = string.Empty;
                            if (_resultService.IsEmail())
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
                                if (emailTemplate != null)
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
                            if (_resultService.IsSMS())
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
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    #endregion
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.SpecialRateStatusAlreadyUpdated);
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

        public ResultDto GetSpecialRateRequestListNew(SpecialRateInputDto inputDto)
        {
            var specialRateListDto = new List<SpecialRateOutputDto>();
            _methodName = "GetSpecialRateRequestListNew";
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                //Sales person
                if (inputDto.LoginUserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var roleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                if (roleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                IQueryable<SpecialRate> specialRateListContext = null;

                //var ZHIds = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                var ZHIds = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();

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
                    divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                 .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });
                }

                List<long> bdoList = new List<long>();
                List<long> zhList = new List<long>();
                if (inputDto.ZHId > 0)
                {
                    zhList.Add(inputDto.ZHId);
                    var bdos = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.ZHId).Select(_ => _.Id).ToList();
                    bdoList.AddRange(bdos);
                }
                else
                {
                    zhList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                    bdoList = _emamiContext.Users.AsNoTracking().Where(_ => zhList.Contains(_.ReportingToId ?? 0)).Select(_ => _.Id).ToList();
                }

                var specialRateApproval = (from spa in _emamiContext.SpecialRateApproval.AsNoTracking()
                                           join sr in _emamiContext.SpecialRate.AsNoTracking() on spa.SpecialRateId equals sr.Id
                                           join ud in divisionslogieduser on new { SalesOrganizationId = sr.SalesOrganizationId, DistributionChannelId = sr.DistributionChannelId, DivisionId = sr.DivisionId }
                                           equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                           where zhList.Contains(spa.CreatedBy)
                                           || spa.CreatedBy == inputDto.LoginUserId
                                           || ZHIds.Contains(spa.RequestedTo)
                                           select spa
                                         );

                //var specialRateApproval = _emamiContext.SpecialRateApproval.AsNoTracking().Where(_ => _.RequestedTo == inputDto.LoginUserId || _.CreatedBy == inputDto.LoginUserId || ZHIds.Contains(_.RequestedTo));
                List<long> specialRateIds = specialRateApproval.Select(_ => _.SpecialRateId).Distinct().ToList();

                var key = UtilityHelper.GetEnumDescription((DTO.Enums.Configuration.DiscountAmountforSpecialRateApproval));
                var discountAmountForSpecialRateApproval = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Key == key)?.Value ?? "0";
                var amountInDecimal = Convert.ToDecimal(discountAmountForSpecialRateApproval);

                if (inputDto.DealerId != null && inputDto.OilTypeId != null && inputDto.FromDate.HasValue && inputDto.ToDate.HasValue)
                {
                    specialRateListContext = (from sr in _emamiContext.SpecialRate.AsNoTracking()
                                              join ud in divisionslogieduser on new { SalesOrganizationId = sr.SalesOrganizationId, DistributionChannelId = sr.DistributionChannelId, DivisionId = sr.DivisionId }
                                              equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                              where sr.UserId == inputDto.DealerId
                              && sr.OilTypeId == inputDto.OilTypeId
                              && sr.CreatedDate >= inputDto.FromDate
                              && sr.CreatedDate <= inputDto.ToDate
                              && specialRateIds.Contains(sr.Id)
                              && (sr.FinalPrice - sr.SpecialPrice) > amountInDecimal
                                              select sr
                                            );
                    //specialRateListContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => _.UserId == inputDto.DealerId
                    //        && _.OilTypeId == inputDto.OilTypeId && _.CreatedDate >= inputDto.FromDate && _.CreatedDate <= inputDto.ToDate 
                    //        && specialRateIds.Contains(_.Id) && (_.FinalPrice - _.SpecialPrice) > amountInDecimal);
                }
                //else if ((inputDto.DealerId != 0 && inputDto.DealerId != null) || (inputDto.OilTypeId != 0 && inputDto.OilTypeId != null)
                //    || (inputDto.FromDate.HasValue && inputDto.FromDate != DateTime.MinValue) || (inputDto.ToDate.HasValue && inputDto.ToDate != DateTime.MinValue))
                //{
                //    return _resultService.ErrorMessage(Constants.RecordNotFound);
                //}
                else
                {
                    if (bdoList != null && bdoList.Any())
                    {
                        List<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                        if (dealersList != null && dealersList.Any())
                        {
                            if (dealersList != null && dealersList.Any())
                            {
                                specialRateListContext = (from sr in _emamiContext.SpecialRate.AsNoTracking()
                                                          join s in _emamiContext.Sauda.AsNoTracking() on sr.Id equals s.SpecialRateRequestIdInParentTable
                                                          where dealersList.Contains(sr.UserId)
                                  && DbFunctions.TruncateTime(sr.CreatedDate) >= inputDto.FromDate
                                  && DbFunctions.TruncateTime(sr.CreatedDate) <= inputDto.ToDate
                                  && specialRateIds.Contains(sr.Id)
                                  && ((sr.FinalPrice - sr.SpecialPrice) > amountInDecimal)
                                                          select sr
                                );

                                //specialRateListContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => dealersList.Contains(_.UserId) 
                                //&& DbFunctions.TruncateTime(_.CreatedDate) >= inputDto.FromDate && DbFunctions.TruncateTime(_.CreatedDate) <= inputDto.ToDate 
                                //&& specialRateIds.Contains(_.Id) && (_.FinalPrice - _.SpecialPrice) > amountInDecimal);
                            }
                        }
                    }
                }
                if (specialRateListContext != null && specialRateListContext.Any())
                {
                    var specialRateList = specialRateListContext.Join(_emamiContext.Users.AsNoTracking(), sr => sr.UserId, u => u.Id, (sr, u) => new { sr, u })
                        .Join(_emamiContext.UserRoles.AsNoTracking(), sru => sru.u.Id, ur => ur.UserId, (sru, ur) => new { sru.sr, sru.u, ur }).Where(_ => _.sr != null && _.u != null && _.ur != null)
                        .OrderByDescending(o => o.sr.CreatedDate).ToList();

                    var zhCreatedSRAIds = specialRateList.Where(_ => ZHIds.Contains(_.sr.CreatedBy)).Select(s => s.sr.Id).ToList();
                    var zhCreatedSRA = specialRateList.Where(_ => zhCreatedSRAIds.Contains(_.sr.Id)).ToList();
                    var otherSRA = specialRateList.Where(_ => !zhCreatedSRAIds.Contains(_.sr.Id) && _.sr.StatusId != (int)DTO.Enums.Status.Pending);
                    specialRateList = zhCreatedSRA;
                    specialRateList.AddRange(otherSRA);

                    var cityContext = _emamiContext.City.AsNoTracking();
                    var stateContext = _emamiContext.State.AsNoTracking();

                    foreach (var specialRateContext in specialRateList)
                    {
                        var specialRateOutputDto = new SpecialRateOutputDto();
                        specialRateOutputDto.SpecialRateId = specialRateContext.sr.Id;
                        specialRateOutputDto.DealerId = specialRateContext.sr.UserId;
                        specialRateOutputDto.DealerName = string.Concat((specialRateContext.sr.User != null ? specialRateContext.sr.User.Name : string.Empty) + "-" + (cityContext.FirstOrDefault(c => c.Id == specialRateContext.sr.User.CityId).CityName != null ? cityContext.FirstOrDefault(c => c.Id == specialRateContext.sr.User.CityId).CityName : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == specialRateContext.sr.User.StateId).StateName != null ? stateContext.FirstOrDefault(s => s.Id == specialRateContext.sr.User.StateId).StateName : string.Empty) + "-" + (specialRateContext.sr.User != null ? specialRateContext.sr.User.Code : string.Empty));
                        //specialRateOutputDto.DealerCode = cityContext.FirstOrDefault(c => c.Id == specialRateContext.sr.User.CityId) != null ? cityContext.FirstOrDefault(c => c.Id == specialRateContext.sr.User.CityId).CityName : string.Empty + "-" + stateContext.FirstOrDefault(s => s.Id == specialRateContext.sr.User.StateId) != null ? stateContext.FirstOrDefault(s => s.Id == specialRateContext.sr.User.StateId).StateName : string.Empty + "-" + specialRateContext.sr.User != null ? specialRateContext.sr.User.Code : string.Empty;
                        specialRateOutputDto.RequestDate = specialRateContext.sr.CreatedDate;
                        specialRateOutputDto.StatusId = specialRateContext.sr.StatusId;
                        specialRateOutputDto.StatusName = specialRateContext.sr.Status != null ? specialRateContext.sr.Status.Name : string.Empty;
                        specialRateOutputDto.IsBroker = specialRateContext.ur.RoleId == (int)DTO.Enums.Role.Broker ? true : false;
                        specialRateOutputDto.IsLTD = specialRateContext.sr.IsLTD;
                        specialRateOutputDto.SkuId = specialRateContext.sr.SkuId;
                        specialRateOutputDto.SkuName = specialRateContext.sr.Sku != null ? specialRateContext.sr.Sku.SkuName : string.Empty;
                        specialRateOutputDto.SpecialPrice = specialRateContext.sr.SpecialPrice;
                        specialRateOutputDto.Quantity = specialRateContext.sr.QuantityCase;
                        specialRateOutputDto.DiscountOrPremium = specialRateContext.sr.FinalPrice - specialRateContext.sr.SpecialPrice;
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

        #region Sauda
        public ResultDto GetBookedSauda(LoginNHId inputDto)
        {
            _methodName = "GetBookedSauda";
            var resultDto = new ResultDto();
            var saudaListDto = new List<BookedSaudaDto>();
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

                List<long> bdoList = new List<long>();
                if (inputDto.ZHId > 0)
                {
                    var bdos = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.ZHId).Select(_ => _.Id).ToList();
                    bdoList.AddRange(bdos);
                }
                else
                {
                    var ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                    bdoList = _emamiContext.Users.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToId ?? 0)).Select(_ => _.Id).ToList();
                }
                if (bdoList != null && bdoList.Any())
                {
                    List<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    if (dealersList != null && dealersList.Any())
                    {
                        saudaListDto = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null).Join(_emamiContext.Users.AsNoTracking(), so => so.Sauda.UserId, u => u.Id, (so, u) =>
                                new { Sauda = so.Sauda, OilTypeId = so.Sku.OilTypeId, OilTypeName = so.OilType.Name, StatusId = so.StatusId, SaudaOrderId = so.Id, DealerId = u.Id, DealerName = u.Name })
                            .Join(_emamiContext.UserRoles.AsNoTracking(), x => x.DealerId, ur => ur.UserId, (x, ur) =>
                            new { x.Sauda, x.OilTypeId, x.OilTypeName, x.DealerId, x.DealerName, x.StatusId, x.SaudaOrderId, IsBroker = (ur.RoleId == (int)DTO.Enums.Role.Broker ? true : false) })
                            .Join(_emamiContext.ApprovalStatus.AsNoTracking(), x => x.StatusId, ss => ss.Id, (x, ss) => new { x.Sauda, x.OilTypeId, x.OilTypeName, x.DealerId, x.DealerName, x.StatusId, x.SaudaOrderId, x.IsBroker, StatusName = (x.StatusId == (int)DTO.Enums.Status.Pending ? Constants.Accepted : ss.Name) })
                            .Where(_ => dealersList.Contains(_.Sauda.UserId))
                            .Select(_ => new BookedSaudaDto()
                            {
                                SaudaOrderId = _.SaudaOrderId,
                                DealerId = _.DealerId,
                                Dealer = _.DealerName,
                                SaudaBookedDate = _.Sauda.BiddingDate,
                                IsBroker = _.IsBroker,
                                SaudaNumber = _.Sauda.Id.ToString(),
                                OilTypeId = (long)_.OilTypeId,
                                OilType = _.OilTypeName,
                                StatusId = _.StatusId,
                                Status = _.StatusName,
                            }).ToList();
                    }
                }
                return _resultService.SuccessObject(saudaListDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetPendingSaudaChartForMobile(LoginZHId inputDto)
        {
            _methodName = "GetPendingSaudaChartForMobile";
            var resultDto = new ResultDto();
            var saudaListDto = new List<PendingSaudaChartOutputDto>();
            //var saudaSPDto = new List<PendingSaudaChartSPDto>();
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



                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {
                    var sqlQuery = @"Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                                            insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                                            select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId
                                            select 
                                            p.SaudaQuantity as BidQuantity,
                                            u.Id as UserId,
                                            (Case when p.ContractValidFrom is null then Cast('0001-01-01T00:00:00' as datetime2) else p.ContractValidFrom end) as BiddingDate
                                            from PendingContracts p with(NOLOCK)
                                            join Users u with(NOLOCK) on p.PendingQuantityInCase > 0.99 and p.UserId=u.Id
                                            --left join Saudas s with(NOLOCK) on p.SaudaNumber=s.SaudaNumber
                                            join #UserDivision ud on ud.SalesOrganizationId=p.SalesOrgId and ud.DistributionChannelId=p.DistChnlId
											and ud.DivisionId=p.DivisionId
                                            where u.Id in (select distinct CustomerId from UserCustomerMappings where UserId in(
                                            select UserId from UserReportingToMappings where ReportingToUserId in(
                                            select UserId from UserReportingToMappings where ReportingToUserId=@UserId)))
                                            drop table #UserDivision";
                    var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    saudaListDto = conn.Query<PendingSaudaChartOutputDto>(sqlQuery, new
                    {
                        UserId = inputDto.LoginUserId
                    }).ToList();

                }


                //List<long> bdoList = new List<long>();

                ////New Reporting to table change
                //var ZHList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                //bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();

                //var ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                //bdoList = _emamiContext.Users.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToId ?? 0)).Select(_ => _.Id).ToList();

                //Multiplue combination changes
                //var divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                //   .Select(_ => new DivisionDetailsDto
                //   {
                //       SalesOrganizationId = _.SalesOrganizationId,
                //       DistributionChannelId = _.DistributionChannelId,
                //       DivisionId = _.DivisionId
                //   });
                //var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                //_emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
                //if (bdoList != null && bdoList.Any())
                //{
                //    var saudaContext = _emamiContext.Sauda.AsQueryable();
                //    List<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                //if (dealersList != null && dealersList.Any())
                //{





                //saudaListDto = (from pc in saudaSPDto
                //                join dm in divisionslogieduser on new { SalesOrganizationId = pc.SalesOrganizationId, DistributionChannelId = pc.DistributionChannelId, DivisionId = pc.DivisionId }
                //                          equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                //                          // join sauda in _emamiContext.Sauda.AsNoTracking() on pc.SaudaNumber equals sauda.SaudaNumber
                //                where dealersList.Contains(pc.UserId)
                //                //&& DbFunctions.TruncateTime(pc.ContractValidTo) >= DbFunctions.TruncateTime(currentDate)
                //                //&& bdoList.Contains(sauda.BdoId)
                //                select new PendingSaudaChartOutputDto()
                //                {
                //                    UserId = pc.UserId,
                //                    BidQuantity = pc.BidQuantity,
                //                    BiddingDate = saudaContext.FirstOrDefault(_ => _.SaudaNumber == pc.SaudaNumber) != null ? saudaContext.FirstOrDefault(_ => _.SaudaNumber == pc.SaudaNumber).BiddingDate : DateTime.Today
                //                }).ToList();

                //old query
                //var saudaListDto1 = (from pc in _emamiContext.PendingContracts.AsNoTracking()
                //                join u in _emamiContext.Users.AsNoTracking() on pc.UserId equals u.Id
                //                join sauda in _emamiContext.Sauda.AsNoTracking() on pc.SaudaNumber equals sauda.SaudaNumber
                //                where pc.PendingQuantityInCase != 0 && dealersList.Contains(u.Id)
                //        //                select new PendingSaudaChartOutputDto() { UserId = u.Id, BidQuantity = pc.SaudaQuantity, BiddingDate = sauda.BiddingDate }).ToList();
                //    }
                //}

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
        #endregion

        #region Quantity Allocation
        public ResultDto GetSpecialityFatQuantityLimitList(LoginUserIdDto inputDto)
        {
            _methodName = "GetSpecialityFatQuantityList";
            var quantityLimitList = new List<SpecialityFatDiscountUserDto>();
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
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                quantityLimitList = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().Where(w => w.UserId == inputDto.LoginUserId && w.ParentQuantityId == 0
                    && w.Sku != null && w.OilType != null
                    && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
                    && DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(currentDate)
                    //&& w.DivisionId == userContext.DivisionId.Value
                    ).OrderByDescending(o => o.CreatedDate).Select(s => new SpecialityFatDiscountUserDto()
                    {
                        Id = s.Id,
                        SkuId = s.SkuId,
                        SkuName = s.Sku.SkuName,
                        SkuCode = s.Sku.SkuCode,
                        OilTypeId = s.OilTypeId,
                        OilTypeName = s.OilType.Name,
                        QuantityLimit = s.ActualDiscount,
                        ValidFrom = s.ValidFrom,
                        ValidTo = s.ValidTo,
                        RemainingQuantity = s.RemainingQuantity
                    }).ToList();
                return _resultService.SuccessObject(quantityLimitList);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetAssignedSpecialityFatQuantityLimitList(LoginUserIdDto inputDto)
        {
            _methodName = "GetAssignedSpecialityFatQuantityLimitList";
            var result = new List<SpecialityFatQuantityLimitParentChildDto>();
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

                result = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().Where(w => w.UserId == inputDto.LoginUserId
                //&& w.DivisionId == userContext.DivisionId.Value
                )
                //.Join(_emamiContext.SpecalityFatDiscountUsers.AsNoTracking(), x => x.ParentQuantityId, p => p.Id, (x, p) => new { child = x, parent = p })
                .OrderByDescending(o => o.CreatedDate)
                .Select(s => new SpecialityFatQuantityLimitParentChildDto()
                {
                    Id = s.Id,
                    SkuId = s.SkuId,
                    SkuName = s.Sku.SkuName,
                    SkuCode = s.Sku.SkuCode,
                    OilTypeId = s.OilTypeId,
                    OilTypeName = s.OilType != null ? s.OilType.Name : string.Empty,
                    ActualQuantity = s.ActualDiscount,
                    RemainingQuantity = s.RemainingQuantity,
                    ValidFrom = s.ValidFrom,
                    ValidTo = s.ValidTo,
                    ChildActualQuantity = s.ActualDiscount,
                    ChildValidFrom = s.ValidFrom,
                    ChildValidTo = s.ValidTo,
                    EmployeeId = s.UserId,
                    EmployeeName = s.User != null ? s.User.Name : string.Empty,
                    Email = s.User.Email,
                    MobileNumber = s.User != null ? s.User.MobileNumber : string.Empty,
                }).ToList();


                //result = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().Where(w => w.CreatedBy == inputDto.LoginUserId && w.
                ////&& w.DivisionId == userContext.DivisionId.Value
                //)
                //.Join(_emamiContext.SpecalityFatDiscountUsers.AsNoTracking(), x => x.ParentQuantityId, p => p.Id, (x, p) => new { child = x, parent = p })
                //.OrderByDescending(o => o.parent.CreatedDate)
                //.Select(s => new SpecialityFatQuantityLimitParentChildDto()
                //{
                //    Id = s.child.Id,
                //    SkuId = s.child.SkuId,
                //    SkuName = s.child.Sku.SkuName,
                //    SkuCode = s.child.Sku.SkuCode,
                //    OilTypeId = s.child.OilTypeId,
                //    OilTypeName = s.child.OilType != null ? s.child.OilType.Name : string.Empty,
                //    ActualQuantity = s.parent.ActualDiscount,
                //    RemainingQuantity = s.parent.RemainingQuantity,
                //    ValidFrom = s.parent.ValidFrom,
                //    ValidTo = s.parent.ValidTo,
                //    ChildActualQuantity = s.child.ActualDiscount,
                //    ChildValidFrom = s.child.ValidFrom,
                //    ChildValidTo = s.child.ValidTo,
                //    EmployeeId = s.child.UserId,
                //    EmployeeName = s.child.User != null ? s.child.User.Name : string.Empty,
                //    Email = s.child.User.Email,
                //    MobileNumber = s.child.User != null ? s.child.User.MobileNumber : string.Empty,
                //}).ToList();

                return _resultService.SuccessObject(result);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto AssignSpecialityFatQuantityLimit(SpecialityFatEmployeeDiscountDto inputDto)
        {
            _methodName = "AssignSpecialityFatQuantityLimit";
            var resultDto = new ResultDto();
            bool isFirstRecord = false;
            long parentId = 0;
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
                if (inputDto.CustomerId == null || !inputDto.CustomerId.Any())
                {
                    return _resultService.ErrorMessage(Constants.EmployeeIsEmpty);
                }

                var discountData = _emamiContext.SpecalityFatDiscountUsers.FirstOrDefault(f => f.Id == inputDto.Id);

                if (discountData != null)
                {
                    #region Validation

                    var userId = inputDto.CustomerId;
                    var details = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                    .Where(w => w.OilTypeId == inputDto.OilTypeId && w.SkuId == inputDto.SkuId // && (w.Id == inputDto.Id && w.ParentId == inputDto.Id)
                    && userId.Contains(w.UserId)
                    && ((DbFunctions.TruncateTime(w.ValidFrom) >= DbFunctions.TruncateTime(inputDto.EmpValidFrom)
                    && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(inputDto.EmpValidTo))
                    || (DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(inputDto.EmpValidFrom)
                    && DbFunctions.TruncateTime(w.ValidTo) <= DbFunctions.TruncateTime(inputDto.EmpValidTo))));

                    var notWithinCurrentDiscount = details.Where(w => w.Id != inputDto.Id && w.ParentId != inputDto.Id).Select(s => s.UserId).ToList();
                    if (notWithinCurrentDiscount != null && notWithinCurrentDiscount.Any() && notWithinCurrentDiscount.Count > 0)
                    {
                        var userName = _emamiContext.Users.AsNoTracking().Where(w => notWithinCurrentDiscount.Any(a => a == w.Id)).Select(s => s.Name).ToList();
                        return _resultService.ErrorMessage(Constants.QtyLimitAlreadyExistiInThisUser + string.Join(",", userName.Select(s => s)));
                    }

                    #endregion

                    if (!(inputDto.EmpValidFrom.Date >= discountData.ValidFrom.Date && inputDto.EmpValidFrom.Date <= discountData.ValidTo.Date
                        && inputDto.EmpValidTo.Date <= discountData.ValidTo.Date && inputDto.EmpValidTo.Date >= discountData.ValidFrom.Date))
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = "Please select a Valid From and Valid To date";
                        return resultDto;
                    }
                    decimal totalQuantity = 0;
                    totalQuantity = inputDto.EmpActualDiscount * inputDto.CustomerId.Count();
                    if (!(totalQuantity <= discountData.ActualDiscount))
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = "Actual quantity limit is " + discountData.ActualDiscount + ". Please enter less than or equal to quantity";
                        return resultDto;
                    }

                    foreach (var userid in inputDto.CustomerId)
                    {
                        var result = new SpecalityFatDiscountUser()
                        {
                            OilTypeId = inputDto.OilTypeId,
                            SkuId = inputDto.SkuId,
                            UserId = userid,
                            ActualDiscount = inputDto.EmpActualDiscount,
                            ParentId = parentId,
                            ParentQuantityId = discountData.Id,
                            RemainingQuantity = inputDto.EmpActualDiscount,
                            ValidFrom = inputDto.EmpValidFrom,
                            ValidTo = inputDto.EmpValidTo,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                        };

                        _emamiContext.SpecalityFatDiscountUsers.Add(result);
                        if (!isFirstRecord)
                        {
                            isFirstRecord = true;
                            _emamiContext.SaveChanges();
                            parentId = result.Id;
                        }
                    }

                    //Update remaining quantity
                    discountData.RemainingQuantity = discountData.ActualDiscount - totalQuantity;

                    _emamiContext.SaveChanges();

                    try
                    {
                        var input = new SpecialityFatDiscountUserDto()
                        {
                            CustomerId = inputDto.CustomerId,
                            SkuId = inputDto.SkuId,
                            QuantityLimit = inputDto.EmpActualDiscount,
                            ValidFrom = inputDto.EmpValidFrom,
                            ValidTo = inputDto.EmpValidTo,
                        };
                        SpecialityFatLimitNotification(input);
                    }
                    catch (Exception ex)
                    {
                    }
                    resultDto.IsSuccess = true;
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                }
                return _resultService.SuccessObject(resultDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto UpdateAssignedSpecialityFatQuantityLimit(SpecialityFatDiscountUserDto inputDto)
        {
            _methodName = "UpdateAssignedSpecialityFatQuantityLimit";
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
                decimal assignedQuantity = 0;
                assignedQuantity = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking()
                            .Where(w => w.ParentQuantityId == inputDto.Id).Select(s => s.ActualDiscount).DefaultIfEmpty(0).Sum();

                var specalityFatData = _emamiContext.SpecalityFatDiscountUsers.FirstOrDefault(w => w.Id == inputDto.Id);
                if (specalityFatData != null)
                {

                    if (specalityFatData.ParentQuantityId == 0)
                    {
                        if (inputDto.QuantityLimit >= assignedQuantity)
                        {
                            specalityFatData.ActualDiscount = inputDto.QuantityLimit;
                            specalityFatData.RemainingQuantity = inputDto.QuantityLimit - assignedQuantity;
                            specalityFatData.ModifiedBy = inputDto.LoginUserId;
                            specalityFatData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                            _emamiContext.SaveChanges();
                        }
                        else
                        {
                            return _resultService.ErrorMessage(specalityFatData.User.Name + " Total quantity is " + specalityFatData.ActualDiscount + ". Used quantity is " + assignedQuantity + ". Total quantity is should be greater then or equal to assigned quantity");
                        }
                    }
                    else
                    {
                        var parentAssignedQuantity = _emamiContext.SpecalityFatDiscountUsers.FirstOrDefault(w => w.Id == specalityFatData.ParentQuantityId);

                        var extraQuantity = inputDto.QuantityLimit - specalityFatData.ActualDiscount;

                        bool positive = extraQuantity > 0;
                        bool negative = extraQuantity < 0;

                        if (positive)
                        {
                            if (extraQuantity <= parentAssignedQuantity.RemainingQuantity)
                            {
                                parentAssignedQuantity.RemainingQuantity = parentAssignedQuantity.RemainingQuantity - extraQuantity;
                                parentAssignedQuantity.ModifiedBy = inputDto.LoginUserId;
                                parentAssignedQuantity.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                                specalityFatData.ActualDiscount = inputDto.QuantityLimit;
                                specalityFatData.RemainingQuantity = specalityFatData.RemainingQuantity + extraQuantity;
                                specalityFatData.ModifiedBy = inputDto.LoginUserId;
                                specalityFatData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                _emamiContext.SaveChanges();
                            }
                            else
                            {
                                return _resultService.ErrorMessage(Constants.QtyLimitExceeded);
                            }
                        }
                        else
                        {
                            if (inputDto.QuantityLimit >= assignedQuantity)
                            {
                                parentAssignedQuantity.RemainingQuantity = parentAssignedQuantity.RemainingQuantity - extraQuantity;
                                parentAssignedQuantity.ModifiedBy = inputDto.LoginUserId;
                                parentAssignedQuantity.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                                specalityFatData.ActualDiscount = inputDto.QuantityLimit;
                                specalityFatData.RemainingQuantity = specalityFatData.RemainingQuantity + extraQuantity;
                                specalityFatData.ModifiedBy = inputDto.LoginUserId;
                                specalityFatData.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                _emamiContext.SaveChanges();
                            }
                            else
                            {
                                return _resultService.ErrorMessage(specalityFatData.User.Name + " Total quantity is " + specalityFatData.ActualDiscount + ". Used quantity is " + assignedQuantity + ". Total quantity is should be greater then or equal to assigned quantity");
                            }
                        }
                    }
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                try
                {
                    SpecialityFatLimitNotification(inputDto);
                }
                catch (Exception)
                {
                }
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

        public void SpecialityFatLimitNotification(SpecialityFatDiscountUserDto inputDto)
        {
            try
            {
                var skuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SkuId)?.SkuName;
                {
                    var usersContext = _emamiContext.Users.AsNoTracking().Where(_ => inputDto.CustomerId.Contains(_.Id)).ToList();
                    if (usersContext != null && usersContext.Any())
                    {
                        List<string> toUsers = new List<string>();
                        toUsers.AddRange(usersContext.Select(_ => _.Email));
                        string fromDate = inputDto.ValidFrom.ToString("MMM dd,yyyy");
                        string toDate = inputDto.ValidTo.ToString("MMM dd,yyyy");

                        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                        if (_resultService.IsEmail())
                        {
                            var fromEmail = Constants.FromEmail;
                            EmailTemplate emailTemplate = new EmailTemplate();
                            var plainText = string.Empty;
                            var emailSubject = Constants.SpecialityFatLimitSubject;
                            emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecalityFatDiscountUserEmail);
                            if (emailTemplate != null)
                            {
                                var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, skuName).Replace(Constants.FromDate, fromDate).Replace(Constants.ToDate, toDate)
                                    .Replace(Constants.Quantity, inputDto.QuantityLimit.ToString());
                                var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                            }
                        }
                        var smsPlainTemplate = string.Empty;
                        if (_resultService.IsSMS())
                        {
                            var smsMessage = string.Empty;
                            EmailTemplate smsTemplate = new EmailTemplate();
                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecalityFatDiscountUserSMS);
                            if (smsTemplate != null)
                            {
                                smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, skuName).Replace(Constants.FromDate, fromDate).Replace(Constants.ToDate, toDate)
                                    .Replace(Constants.Quantity, inputDto.QuantityLimit.ToString());
                                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                try
                                {
                                    foreach (var mobileNumber in usersContext.Select(_ => _.MobileNumber).ToList())
                                    {
                                        amazonNotificationService.SendMessage(smsMessage, mobileNumber);
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public ResultDto AddSpecialtyFatQuantityRequests(SpecialtyFatQuantityRequestDto inputDto)
        {
            _methodName = "AddSpecialtyFatQuantityRequests";
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
                var entity = new SpecialtyFatQuantityRequest
                {
                    SkuId = inputDto.SkuId,
                    Quantity = inputDto.Quantity,
                    OilTypeId = inputDto.OiltypeId,
                    StatusId = (int)DTO.Enums.Status.Pending,
                    SpecialtyFatQuantityLimitId = inputDto.SpecialtyFatQuantityLimitId,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = DateTime.Now,
                    //DivisionId = userContext.DivisionId.Value
                };
                _emamiContext.SpecialtyFatQuantityRequests.Add(entity);
                _emamiContext.SaveChanges();

                var specialtyFatQuantityRequestUserDetailEntity = new SpecialtyFatQuantityRequestUserDetail
                {
                    UserId = inputDto.LoginUserId,
                    SpecialtyFatQuantityRequestId = entity.Id,
                    StatusId = (int)DTO.Enums.Status.Pending,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                };
                _emamiContext.SpecialtyFatQuantityRequestUserDetails.Add(specialtyFatQuantityRequestUserDetailEntity);
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

        public ResultDto GetSpecialtyFatQuantityRequestsListByOrganizationReportingToId(SpecialtyFatQuantityRequestSearchDto inputDto)
        {
            _methodName = "GetSpecialtyFatQuantityRequestsListByOrganizationReportingToId";
            var resultDto = new ResultDto();
            var specialtyFatQuantityRequestsList = new List<SpecialtyFatQuantityRequestDto>();
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

                var userList = _emamiContext.Users.AsNoTracking().Where(w => w.ReportingToId == inputDto.LoginUserId).ToList();
                specialtyFatQuantityRequestsList = (from us in userList
                                                    join sfu in _emamiContext.SpecialtyFatQuantityRequestUserDetails on us.Id equals sfu.UserId
                                                    join sf in _emamiContext.SpecialtyFatQuantityRequests on sfu.SpecialtyFatQuantityRequestId equals sf.Id
                                                    join createus in _emamiContext.Users on sf.CreatedBy equals createus.Id
                                                    join sfd in _emamiContext.SpecalityFatDiscountUsers.AsNoTracking() on sf.SpecialtyFatQuantityLimitId equals sfd.Id
                                                    join parentDiscount in _emamiContext.SpecalityFatDiscountUsers.AsNoTracking() on sfd.ParentQuantityId equals parentDiscount.Id
                                                    //where sf.DivisionId == userContext.DivisionId.Value
                                                    orderby sf.Id
                                                    select new SpecialtyFatQuantityRequestDto
                                                    {
                                                        Id = sf.Id,
                                                        UserId = sfu.UserId,
                                                        UserName = sfu.User.Name,
                                                        SkuId = sf.SkuId,
                                                        SkuName = sf.Sku.SkuName,
                                                        SkuCode = sf.Sku.SkuCode,
                                                        Quantity = sf.Quantity,
                                                        Status = sf.Status != null ? sf.Status.Name : string.Empty,
                                                        StatusId = sf.StatusId,
                                                        OiltypeId = sf.OilTypeId,
                                                        OilTypeName = sf.OilType.Name,
                                                        CreatedBy = createus.Name,
                                                        SpecialtyFatQuantityRequestId = sfu.SpecialtyFatQuantityRequestId,
                                                        IsRequestedUser = ((sf.Id == sfu.SpecialtyFatQuantityRequestId && inputDto.LoginUserId == sfu.UserId) ? true : false),
                                                        IsApprove = parentDiscount.RemainingQuantity > sf.Quantity ? true : false,
                                                    }).OrderByDescending(dto => dto.Id).ToList();

                return _resultService.SuccessObject(specialtyFatQuantityRequestsList);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto GetSpecialtyFatQuantityRequestsList(SpecialtyFatQuantityRequestSearchDto inputDto)
        {
            _methodName = "GetSpecialtyFatQuantityRequestsList";
            var resultDto = new ResultDto();
            List<SpecialtyFatQuantityRequestDto> outputDto = new List<SpecialtyFatQuantityRequestDto>();
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

                outputDto = (from sfu in _emamiContext.SpecialtyFatQuantityRequestUserDetails
                             join sf in _emamiContext.SpecialtyFatQuantityRequests on sfu.SpecialtyFatQuantityRequestId equals sf.Id
                             join us in _emamiContext.Users on sf.CreatedBy equals us.Id
                             where sfu.UserId == inputDto.LoginUserId //&& sf.DivisionId == userContext.DivisionId.Value
                             orderby sf.Id
                             select new SpecialtyFatQuantityRequestDto
                             {
                                 Id = sf.Id,
                                 UserId = sfu.UserId,
                                 UserName = sfu.User.Name,
                                 SkuId = sf.SkuId,
                                 SkuName = sf.Sku.SkuName,
                                 SkuCode = sf.Sku.SkuCode,
                                 Quantity = sf.Quantity,
                                 Status = sf.Status != null ? sf.Status.Name : string.Empty,
                                 StatusId = sf.StatusId,
                                 OiltypeId = sf.OilTypeId,
                                 OilTypeName = sf.OilType.Name,
                                 CreatedBy = us.Name,
                             }).OrderByDescending(dto => dto.Id).ToList();

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

        public ResultDto UpdateSpecialtyFatQuantityLimit(SpecialtyFatQuantityRequestDto inputDto)
        {
            _methodName = "UpdateSpecialtyFatQuantityLimit";
            var resultDto = new ResultDto();
            var errorMessage = new StringBuilder();
            decimal remainingQuantity = 0;
            bool isValid = false;

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
                var usersContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (usersContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var specialtyFatQuantityRequests = _emamiContext.SpecialtyFatQuantityRequests.FirstOrDefault(w => w.Id == inputDto.Id);
                if (specialtyFatQuantityRequests == null)
                {
                    return _resultService.ErrorMessage(Constants.SpecialtyFatQuantityRequestsNotFound);
                }
                // else
                //{
                if (specialtyFatQuantityRequests.StatusId == (int)DTO.Enums.Status.Pending || specialtyFatQuantityRequests.StatusId == (int)DTO.Enums.Status.RequestForApproval)
                {
                    var specalityFatDiscountUsers = _emamiContext.SpecalityFatDiscountUsers
                        .FirstOrDefault(w => w.Id == specialtyFatQuantityRequests.SpecialtyFatQuantityLimitId);
                    if (specalityFatDiscountUsers != null)
                    {
                        if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                        {
                            var specalityFatRemainingQty = _emamiContext.SpecalityFatDiscountUsers
                        .FirstOrDefault(w => w.Id == specalityFatDiscountUsers.ParentQuantityId);
                            if (specalityFatRemainingQty != null)
                            {
                                remainingQuantity = specalityFatRemainingQty.RemainingQuantity;
                                if (remainingQuantity == 0 && specalityFatRemainingQty.ParentQuantityId > 0)
                                {
                                    if (inputDto.RoleId == (int)(DTO.Enums.Role.NationalTrader))
                                    {
                                        specalityFatRemainingQty = _emamiContext.SpecalityFatDiscountUsers
                            .FirstOrDefault(w => w.Id == specalityFatRemainingQty.ParentQuantityId);
                                    }
                                }

                                if (specialtyFatQuantityRequests.Quantity <= specalityFatRemainingQty.RemainingQuantity)
                                {
                                    if (specalityFatDiscountUsers != null && inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                    {
                                        specalityFatDiscountUsers.ActualDiscount = specalityFatDiscountUsers.ActualDiscount + specialtyFatQuantityRequests.Quantity;
                                        specalityFatDiscountUsers.RemainingQuantity = specalityFatDiscountUsers.RemainingQuantity + specialtyFatQuantityRequests.Quantity;
                                        specalityFatRemainingQty.RemainingQuantity = specalityFatRemainingQty.RemainingQuantity - specialtyFatQuantityRequests.Quantity;
                                    }

                                    specialtyFatQuantityRequests.StatusId = inputDto.StatusId;
                                    specialtyFatQuantityRequests.Remarks = inputDto.Remarks;
                                    specialtyFatQuantityRequests.ModifiedBy = inputDto.LoginUserId;
                                    specialtyFatQuantityRequests.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                    //_emamiContext.SaveChanges();
                                    var specialtyFatQuantityRequestUserDetailEntity = new SpecialtyFatQuantityRequestUserDetail
                                    {
                                        UserId = inputDto.LoginUserId,
                                        SpecialtyFatQuantityRequestId = specialtyFatQuantityRequests.Id,
                                        StatusId = inputDto.StatusId,
                                        CreatedBy = inputDto.LoginUserId,
                                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                                    };
                                    _emamiContext.SpecialtyFatQuantityRequestUserDetails.Add(specialtyFatQuantityRequestUserDetailEntity);
                                    _emamiContext.SaveChanges();
                                }
                                else
                                {
                                    var userName = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == specialtyFatQuantityRequests.CreatedBy).Name;
                                    errorMessage.Append("USER : " + userName + " | SKU : " + specialtyFatQuantityRequests.Sku.SkuName + "<br>");
                                }
                            }
                            else
                            {
                                resultDto = _resultService.ErrorMessage(Constants.RecordNotFound);
                            }
                        }
                        else
                        {
                            //var specalityFatRemainingQty = _emamiContext.SpecalityFatDiscountUsers
                            //.FirstOrDefault(w => w.Id == specalityFatDiscountUsers.ParentQuantityId);
                            if (specalityFatDiscountUsers != null && inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                            {
                                specalityFatDiscountUsers.ActualDiscount = (specalityFatDiscountUsers.ActualDiscount + specialtyFatQuantityRequests.Quantity);
                                specalityFatDiscountUsers.RemainingQuantity = (specalityFatDiscountUsers.RemainingQuantity + specialtyFatQuantityRequests.Quantity);
                            }

                            specialtyFatQuantityRequests.StatusId = inputDto.StatusId;
                            specialtyFatQuantityRequests.Remarks = inputDto.Remarks;
                            specialtyFatQuantityRequests.ModifiedBy = inputDto.LoginUserId;
                            specialtyFatQuantityRequests.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                            //_emamiContext.SaveChanges();
                            var specialtyFatQuantityRequestUserDetailEntity = new SpecialtyFatQuantityRequestUserDetail
                            {
                                UserId = inputDto.LoginUserId,
                                SpecialtyFatQuantityRequestId = specialtyFatQuantityRequests.Id,
                                StatusId = inputDto.StatusId,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
                            };
                            _emamiContext.SpecialtyFatQuantityRequestUserDetails.Add(specialtyFatQuantityRequestUserDetailEntity);
                            _emamiContext.SaveChanges();
                        }
                    }
                    else
                    {
                        resultDto = _resultService.ErrorMessage(Constants.RecordNotFound);
                    }
                    //resultDto = _resultService.SuccessMessage(Constants.SpecialtyFatQuantityRequestsSuccessUpdated);
                    //}



                    if (!string.IsNullOrEmpty(errorMessage.ToString()))
                    {
                        errorMessage.Append("Above users not approved. Your remaining quantity is " + remainingQuantity + ".</br>");
                        errorMessage.Append("User requested quantity is greater then for your remaining quantity. so can't approve. Please raise the request");
                        resultDto = _resultService.ErrorMessage(errorMessage.ToString());
                    }
                    else
                    {
                        resultDto = _resultService.SuccessMessage(Constants.SpecialtyFatQuantityRequestsSuccessUpdated);
                    }

                    if (inputDto.StatusId == (int)DTO.Enums.Status.Approved || inputDto.StatusId == (int)DTO.Enums.Status.Rejected)
                    {
                        try
                        {
                            var requestedLimitContext = _emamiContext.SpecialtyFatQuantityRequests.AsNoTracking().FirstOrDefault(w => w.Id == inputDto.Id);
                            var allocatedLimitContext = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().FirstOrDefault(w => w.Id == requestedLimitContext.SpecialtyFatQuantityLimitId);
                            var skuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == allocatedLimitContext.SkuId)?.SkuName;
                            if (requestedLimitContext != null && allocatedLimitContext != null && skuName != null)
                            {
                                decimal limit = 0;
                                if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                {
                                    limit = requestedLimitContext.Quantity;
                                }
                                else
                                {
                                    limit = allocatedLimitContext.ActualDiscount;
                                }

                                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == allocatedLimitContext.UserId);
                                if (userContext != null)
                                {
                                    List<string> toUsers = new List<string>();
                                    toUsers.Add(userContext.Email);
                                    string fromDate = allocatedLimitContext.ValidFrom.ToString("MMM dd,yyyy");
                                    string toDate = allocatedLimitContext.ValidTo.ToString("MMM dd,yyyy");

                                    AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                                    if (_resultService.IsEmail())
                                    {
                                        var fromEmail = Constants.FromEmail;
                                        EmailTemplate emailTemplate = new EmailTemplate();
                                        var plainText = string.Empty;
                                        var emailSubject = string.Empty;
                                        if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                        {
                                            emailSubject = Constants.SpecialityFatLimitApprovalSubject;
                                            emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecalityFatDiscountAcceptEmail);
                                        }
                                        else
                                        {
                                            emailSubject = Constants.SpecialityFatLimitRejectSubject;
                                            emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecalityFatDiscountRejectEmail);
                                        }
                                        if (emailTemplate != null)
                                        {
                                            var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, skuName).Replace(Constants.FromDate, fromDate).Replace(Constants.ToDate, toDate)
                                                .Replace(Constants.Quantity, Math.Round(limit, 0).ToString());
                                            var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                            amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                        }
                                    }
                                    var smsPlainTemplate = string.Empty;
                                    if (_resultService.IsSMS())
                                    {
                                        var smsMessage = string.Empty;
                                        EmailTemplate smsTemplate = new EmailTemplate();
                                        if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
                                        {
                                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecalityFatDiscountAcceptSMS);
                                        }
                                        else
                                        {
                                            smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SpecalityFatDiscountRejectSMS);
                                        }
                                        if (smsTemplate != null)
                                        {
                                            smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, skuName).Replace(Constants.FromDate, fromDate).Replace(Constants.ToDate, toDate)
                                                .Replace(Constants.Quantity, Math.Round(limit, 0).ToString());
                                            smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                            try
                                            {
                                                amazonNotificationService.SendMessage(smsMessage, userContext.MobileNumber);
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                    }
                                    resultDto.IsSuccess = true;
                                    resultDto.SuccessDto.Response = 1;
                                }
                            }
                            return resultDto;
                        }
                        catch (Exception ex)
                        {

                        }

                    }

                }
                else
                {
                    return _resultService.ErrorMessage(Constants.SpecialtyFatQuantityRequestAlreadyUpdated);
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
        #region Lifting Request
        public ResultDto GetLiftingRequestCountList(LiftingRequestListInputDto inputDto)
        {
            _methodName = "GetLiftingRequestCountList";
            var outputDto = new List<LiftingRequestCountDto>();
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

                List<long> bdoList = new List<long>();
                if (inputDto.BDOId > 0)
                {
                    var bdos = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.ZHId).Select(_ => _.Id).ToList();
                    bdoList.AddRange(bdos);
                }
                else
                {
                    var ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                    bdoList = _emamiContext.Users.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToId ?? 0)).Select(_ => _.Id).ToList();
                }
                if (bdoList != null && bdoList.Any())
                {
                    List<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                    if (dealersList != null && dealersList.Any())
                    {
                        outputDto = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => dealersList.Contains(_.UserId) && _.StatusId == inputDto.StatusId)
                            .Join(_emamiContext.Users.AsNoTracking(), lr => lr.UserId, u => u.Id, (lr, u) => new { DealerId = lr.UserId, Dealer = u.Name })
                            .GroupBy(_ => _.DealerId)
                            .Select(_ => new LiftingRequestCountDto()
                            {
                                Dealer = _.FirstOrDefault().Dealer,
                                DealerId = _.FirstOrDefault().DealerId,
                                TotalLiftingCount = _.Count()
                            }).ToList();
                    }
                }
                return _resultService.SuccessObject(outputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }
        #endregion



        #region Sale Tour Plan
        public ResultDto SalesTourPlanChart(SalesTourPlanInputDto inputDto)
        {
            _methodName = "SalesTourPlanChart";
            var outputDto = new List<SalesTourPlanOutputDto>();
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
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                if (inputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }
                if (inputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }
                List<long> bdoList = new List<long>();
                var ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                bdoList = _emamiContext.Users.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToId ?? 0)).Select(_ => _.Id).ToList();

                List<MonthDto> months = new List<MonthDto>();
                months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);

                foreach (var month in months)
                {
                    List<long> MTPDetailsIds = _emamiContext.PermanentJourneyPlans.AsNoTracking()
                    .Join(_emamiContext.MonthlyTourPlanDetails.AsNoTracking(), pjp => pjp.Id, mtpd => mtpd.MonthlyTourPlan.PJPId, (pjp, mtpd) => new { pjp, mtpd })
                    .Where(_ => bdoList.Contains(_.pjp.CreatedBy) && _.pjp.FinancialYearId == inputDto.FinancialYearId && _.pjp.PermanentJourneyPlanStatusId == (int)DTO.Enums.PermanentJourneyPlanStatus.Approved
                    && _.mtpd.MonthlyTourPlan != null && _.mtpd.MonthlyTourPlan.MonthId == month.Id && _.mtpd.MonthlyTourPlan.MonthlyTourPlanStatusId == (int)DTO.Enums.MonthlyTourPlanStatus.Approved)
                    .Select(_ => _.mtpd.Id).ToList();

                    var MTPDeviationCount = _emamiContext.MonthlyPlanDeviation.AsNoTracking().Where(_ => _.StatusId == (int)DTO.Enums.MonthlyPlanDeviationStatus.Approved && MTPDetailsIds.Contains(_.MonthlyTourPlanDetailsId)).Count();

                    var ActualVisitCount = _emamiContext.MarketScenario.AsNoTracking().Where(_ => bdoList.Contains(_.CreatedBy) && DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(month.StartDate)
                        && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(month.EndDate)).Distinct().Count();

                    var chartDetail = new SalesTourPlanOutputDto()
                    {
                        Month = month.Id,
                        PlannedVisit = MTPDetailsIds.Count(),
                        DeviatedVisit = MTPDeviationCount,
                        ActualVisit = ActualVisitCount
                    };

                    outputDto.Add(chartDetail);
                }
                return _resultService.SuccessObject(outputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetSecondarySalesFortheDay(LoginZHId inputDto)
        {
            _methodName = "GetSecondarySalesFortheDay";
            var resultDto = new ResultDto();
            var outputDto = new List<WholesellerSecondarySaleslistDto>();
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
                if (!_resultService.UserIsAcive(inputDto.LoginUserId))
                {
                    return _resultService.ErrorMessage(Constants.InvalidUser);
                }
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                List<long> bdoList = new List<long>();
                if (inputDto.ZHId > 0)
                {
                    var bdos = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.ZHId).Select(_ => _.Id).ToList();
                    bdoList.AddRange(bdos);
                }
                else
                {
                    var ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                    bdoList = _emamiContext.Users.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToId ?? 0)).Select(_ => _.Id).ToList();
                }

                outputDto = _emamiContext.WholeSellerSalesDetail.AsNoTracking()
                    .Join(_emamiContext.Users.AsNoTracking(), ws => ws.WholesellerBdo.DealerId, u => u.Id, (ws, u) => new { Sales = ws, DealerName = u.Name })
                    .Where(_ => _.Sales.WholesellerBdo != null && bdoList.Contains(_.Sales.CreatedBy) && _.Sales.CreatedDate.Month == currentDate.Month
                     && _.Sales.CreatedDate.Year == currentDate.Year)
                    .Select(s => new { s.Sales, s.DealerName, Date = DbFunctions.TruncateTime(s.Sales.CreatedDate) }).GroupBy(g => g.Date)
                     .Select(_ => new WholesellerSecondarySaleslistDto()
                     {
                         VisitDate = _.FirstOrDefault().Date,
                         WholesellerSecondarySales = _.GroupBy(g => g.Sales.WholesellerBdoId).Select(s => new WholesellerSecondarySalesDto()
                         {
                             DealerId = s.FirstOrDefault().Sales.WholesellerBdo.DealerId,
                             Dealer = s.FirstOrDefault().DealerName,
                             WholesellerId = s.FirstOrDefault().Sales.WholesellerBdoId,
                             Name = s.FirstOrDefault().Sales.WholesellerBdo.Name,
                             TotalPrice = s.Sum(t => t.Sales.Price),
                             TotalQuantity = s.Sum(t => t.Sales.QuantityPerMt),
                             VisitDate = _.FirstOrDefault().Date,
                         }).ToList(),
                     }).ToList();

                return _resultService.SuccessObject(outputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }
        #endregion

        #region Premium and Discount Allocation
        public ResultDto GetMultiselectDiscountList(LoginUserIdDto inputDto)
        {
            _methodName = "GetMultiselectDiscountList";
            var discountUsers = new List<DiscountUserDto>();
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
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                discountUsers = _emamiContext.DiscountUsers.AsNoTracking()
                    .Where(w => w.UserId == inputDto.LoginUserId //&& w.ParentId != 0 
                    && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
                    && DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(currentDate)).OrderByDescending(o => o.CreatedDate)
                    .GroupBy(_ => _.ParentId)
                    .Select(s => new DiscountUserDto()
                    {
                        Id = s.FirstOrDefault().ParentId,
                        OilTypeId = s.FirstOrDefault().OilTypeId,
                        OilTypeName = s.FirstOrDefault().OilType != null ? s.FirstOrDefault().OilType.Name : "",
                        ActualDiscount = s.FirstOrDefault().ActualDiscount,
                        ValidFrom = s.FirstOrDefault().ValidFrom,
                        ValidTo = s.FirstOrDefault().ValidTo,
                        SkuDetails = s.Select(_ => new SkuOutputDto()
                        {
                            SkuId = _.SkuId,
                            Name = _.Sku != null ? _.Sku.SkuName : string.Empty,
                            PackGroupId = (_.Sku != null && _.Sku.PackGroupId != null) ? (long)_.Sku.PackGroupId : 0,
                            PackGroupName = (_.Sku != null && _.Sku.PackGroup != null) ? _.Sku.PackGroup.Name : string.Empty,
                            ParentId = _.Id,
                        }).ToList(),
                    }).ToList();

                return _resultService.SuccessObject(discountUsers);

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetMultiselectPremiumList(LoginUserIdDto inputDto)
        {
            _methodName = "GetMultiselectPremiumList";
            var premiumUsers = new List<PremiumUserDto>();
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
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                premiumUsers = _emamiContext.PremiumUser.AsNoTracking()
                    .Where(w => w.UserId == inputDto.LoginUserId //&& w.ParentId != 0 
                    && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
                    && DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(currentDate)).OrderByDescending(o => o.CreatedDate)
                    .GroupBy(_ => _.ParentId)
                    .Select(s => new PremiumUserDto()
                    {
                        Id = s.FirstOrDefault().ParentId,
                        OilTypeId = s.FirstOrDefault().OilTypeId,
                        OilTypeName = s.FirstOrDefault().OilType != null ? s.FirstOrDefault().OilType.Name : "",
                        ActualPremium = s.FirstOrDefault().ActualPremium,
                        ValidFrom = s.FirstOrDefault().ValidFrom,
                        ValidTo = s.FirstOrDefault().ValidTo,
                        SkuDetails = s.Select(_ => new SkuOutputDto()
                        {
                            SkuId = _.SkuId,
                            Name = _.Sku != null ? _.Sku.SkuName : string.Empty,
                            PackGroupId = (_.Sku != null && _.Sku.PackGroupId != null) ? (long)_.Sku.PackGroupId : 0,
                            PackGroupName = (_.Sku != null && _.Sku.PackGroup != null) ? _.Sku.PackGroup.Name : string.Empty,
                            ParentId = _.Id,
                        }).ToList(),
                    }).ToList();

                return _resultService.SuccessObject(premiumUsers);

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetEmployeeAndUserDiscountList(LoginUserIdDto inputDto)
        {
            _methodName = "GetEmployeeAndUserDiscountList";
            var discountUsers = new List<DiscountUserDto>();
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
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                discountUsers = _emamiContext.DiscountUsers.AsNoTracking()
                    .Where(w => w.UserId == inputDto.LoginUserId //&& w.ParentId != 0 
                    && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
                    && DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(currentDate)).OrderByDescending(o => o.CreatedDate)
                    .Select(s => new DiscountUserDto()
                    {
                        Id = s.Id,
                        SkuId = s.SkuId,
                        SkuName = s.Sku != null ? s.Sku.SkuName : "",
                        OilTypeId = s.OilTypeId,
                        OilTypeName = s.OilType != null ? s.OilType.Name : "",
                        ActualDiscount = s.ActualDiscount,
                        ValidFrom = s.ValidFrom,
                        ValidTo = s.ValidTo
                    }).ToList();

                return _resultService.SuccessObject(discountUsers);

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetPremiumList(LoginUserIdDto inputDto)
        {
            _methodName = "GetPremiumList";
            var premiumList = new List<PremiumUserDto>();
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
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                premiumList = _emamiContext.PremiumUser.AsNoTracking()
                    .Where(w => //w.ParentId != 0 && 
                    w.UserId == inputDto.LoginUserId && DbFunctions.TruncateTime(w.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
                    && DbFunctions.TruncateTime(w.ValidTo) >= DbFunctions.TruncateTime(currentDate)).OrderByDescending(o => o.CreatedDate)
                    .Select(s => new PremiumUserDto()
                    {
                        Id = s.Id,
                        SkuId = s.SkuId,
                        SkuName = s.Sku != null ? s.Sku.SkuName : "",
                        OilTypeId = s.OilTypeId,
                        OilTypeName = s.OilType != null ? s.OilType.Name : "",
                        ActualPremium = s.ActualPremium,
                        ValidFrom = s.ValidFrom,
                        ValidTo = s.ValidTo
                    }).ToList();

                return _resultService.SuccessObject(premiumList);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetDiscountUserList(LoginUserIdDto inputDto)
        {
            _methodName = "GetDiscountUserList";
            var result = new List<DiscountUserParentChildDto>();
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
                result = _emamiContext.DiscountUsers.AsNoTracking().Where(w => w.Status && w.ParentId == 0
                && w.CreatedBy == inputDto.LoginUserId)
                .GroupJoin(_emamiContext.DiscountUsers.AsNoTracking().GroupJoin(_emamiContext.DiscountUsers.AsNoTracking(), x => x.Id, gc => gc.ParentDiscountId, (x, gc) => new { child = x, grandChildCount = gc.Count() }), x => x.Id, du => du.child.ParentId, (x, du) => new { parent = x, child = du, })
                //.Join(_emamiContext.DiscountUsers.AsNoTracking(), x => x.parent.ParentDiscountId, p => p.Id, (x, p) => new { x.parent, x.child, grandparent = p })
                .OrderByDescending(o => o.parent.CreatedDate)
                .Select(s => new DiscountUserParentChildDto()
                {
                    Id = s.parent.Id,
                    SkuId = s.parent.SkuId,
                    SkuName = s.parent.Sku.SkuName,
                    SkuCode = s.parent.Sku.SkuCode,
                    OilTypeId = s.parent.OilTypeId,
                    OilTypeName = s.parent.OilType != null ? s.parent.OilType.Name : string.Empty,
                    //ActualDiscount = s.grandparent.ActualDiscount,
                    //ValidFrom = s.grandparent.ValidFrom,
                    //ValidTo = s.grandparent.ValidTo,
                    ChildActualDiscount = s.parent.ActualDiscount,
                    ChildValidFrom = s.parent.ValidFrom,
                    ChildValidTo = s.parent.ValidTo,
                    AssignedUserDiscountList = s.child.Select(_ => new DiscountUserQuantityOutput()
                    {
                        Id = _.child.Id,
                        EmployeeId = _.child.UserId,
                        EmployeeName = _.child.User.Name,
                        Email = _.child.User.Email,
                        MobileNumber = _.child.User.MobileNumber,
                    }).ToList(),
                    IsProcessed = s.child.Where(w => w.grandChildCount > 0).Any(),
                }).ToList();

                return _resultService.SuccessObject(result);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetAssignedPremiumList(LoginUserIdDto inputDto)
        {
            _methodName = "GetAssignedPremiumList";
            var result = new List<PremiumUserParentChildDto>();
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

                result = _emamiContext.PremiumUser.AsNoTracking().Where(w => w.ParentId == 0 &&
                w.CreatedBy == inputDto.LoginUserId)
                .GroupJoin(_emamiContext.PremiumUser.AsNoTracking().
                GroupJoin(_emamiContext.PremiumUser.AsNoTracking(), x => x.Id, gc => gc.ParentPremiumId, (x, gc) => new { child = x, grandChildCount = gc.Count() }), x => x.Id, du => du.child.ParentId, (x, du) => new { parent = x, child = du })
                //.Join(_emamiContext.PremiumUser.AsNoTracking(), x => x.parent.ParentPremiumId, p => p.Id, (x, p) => new { x.parent, x.child, grandparent = p })
                .OrderByDescending(o => o.parent.CreatedDate)
                .Select(s => new PremiumUserParentChildDto()
                {
                    Id = s.parent.Id,
                    SkuId = s.parent.SkuId,
                    SkuName = s.parent.Sku.SkuName,
                    SkuCode = s.parent.Sku.SkuCode,
                    OilTypeId = s.parent.OilTypeId,
                    OilTypeName = s.parent.OilType != null ? s.parent.OilType.Name : string.Empty,
                    //ActualPremium = s.grandparent.ActualPremium,
                    //ValidFrom = s.grandparent.ValidFrom,
                    //ValidTo = s.grandparent.ValidTo,
                    ChildActualPremium = s.parent.ActualPremium,
                    ChildValidFrom = s.parent.ValidFrom,
                    ChildValidTo = s.parent.ValidTo,
                    AssignedUserPremiumList = s.child.Select(_ => new PremiumUserQuantityOutput()
                    {
                        Id = _.child.Id,
                        EmployeeId = _.child.UserId,
                        EmployeeName = _.child.User.Name,
                        Email = _.child.User.Email,
                        MobileNumber = _.child.User.MobileNumber,
                    }).ToList(),
                    IsProcessed = s.child.Where(w => w.grandChildCount > 0).Any(),
                }).ToList();

                return _resultService.SuccessObject(result);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto AssignMultiselectDiscount(DiscountUserDto inputDto)
        {
            _methodName = "AssignMultiselectDiscount";
            var resultDto = new ResultDto();
            bool isFirstRecord = false;
            long parentId = 0;
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
                if (inputDto.CustomerId == null || !inputDto.CustomerId.Any())
                {
                    return _resultService.ErrorMessage(Constants.EmployeeIsEmpty);
                }
                if (inputDto.SkuDetails == null || !inputDto.SkuDetails.Any())
                {
                    return _resultService.ErrorMessage(Constants.SkuMissing);
                }

                var discountData = _emamiContext.DiscountUsers.AsNoTracking().FirstOrDefault(f => f.Id == inputDto.Id);

                if (discountData != null)
                {
                    if (!(inputDto.ValidFrom.Date >= discountData.ValidFrom.Date && inputDto.ValidFrom.Date <= discountData.ValidTo.Date
                        && inputDto.ValidFrom.Date <= discountData.ValidTo.Date && inputDto.ValidFrom.Date >= discountData.ValidFrom.Date))
                    {
                        return _resultService.ErrorMessage("Please select a Valid From and To date");
                    }

                    if (!(inputDto.ActualDiscount <= discountData.ActualDiscount))
                    {
                        return _resultService.ErrorMessage("Discount limit is " + discountData.ActualDiscount + ". Please enter less than or equal to discount");
                    }

                    foreach (var sku in inputDto.SkuDetails)
                    {
                        isFirstRecord = false;
                        parentId = 0;
                        if (!isFirstRecord)
                        {
                            var parentDiscount = new DiscountUsers()
                            {
                                OilTypeId = inputDto.OilTypeId,
                                SkuId = sku.SkuId,
                                UserId = inputDto.CustomerId.FirstOrDefault(),
                                SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                                Status = true,
                                ActualDiscount = inputDto.ActualDiscount,
                                ValidFrom = inputDto.ValidFrom,
                                ValidTo = inputDto.ValidTo,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                ParentId = parentId,
                                ParentDiscountId = sku.ParentId
                            };
                            _emamiContext.DiscountUsers.Add(parentDiscount);
                            _emamiContext.SaveChanges();

                            parentId = parentDiscount.Id;
                            isFirstRecord = true;
                        }
                        foreach (var userid in inputDto.CustomerId)
                        {
                            var discount = new DiscountUsers()
                            {
                                OilTypeId = inputDto.OilTypeId,
                                SkuId = sku.SkuId,
                                UserId = userid,
                                SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                                Status = true,
                                ActualDiscount = inputDto.ActualDiscount,
                                ValidFrom = inputDto.ValidFrom,
                                ValidTo = inputDto.ValidTo,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                ParentId = parentId,
                                ParentDiscountId = sku.ParentId,
                            };
                            _emamiContext.DiscountUsers.Add(discount);
                        }
                        _emamiContext.SaveChanges();
                    }

                    resultDto.IsSuccess = true;
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                }
                return _resultService.SuccessObject(resultDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto AssignMultiselectPremium(PremiumUserDto inputDto)
        {
            _methodName = "AssignMultiselectPremium";
            var resultDto = new ResultDto();
            bool isFirstRecord = false;
            long parentId = 0;
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
                if (inputDto.CustomerId == null || !inputDto.CustomerId.Any())
                {
                    return _resultService.ErrorMessage(Constants.EmployeeIsEmpty);
                }
                if (inputDto.SkuDetails == null || !inputDto.SkuDetails.Any())
                {
                    return _resultService.ErrorMessage(Constants.SkuMissing);
                }

                var discountData = _emamiContext.PremiumUser.AsNoTracking().FirstOrDefault(f => f.Id == inputDto.Id);

                if (discountData != null)
                {
                    if (!(inputDto.ValidFrom.Date >= discountData.ValidFrom.Date && inputDto.ValidFrom.Date <= discountData.ValidTo.Date
                        && inputDto.ValidFrom.Date <= discountData.ValidTo.Date && inputDto.ValidFrom.Date >= discountData.ValidFrom.Date))
                    {
                        return _resultService.ErrorMessage("Please select a Valid From and To date");
                    }

                    if (!(inputDto.ActualPremium <= discountData.ActualPremium))
                    {
                        return _resultService.ErrorMessage("Discount limit is " + discountData.ActualPremium + ". Please enter less than or equal to discount");
                    }

                    foreach (var sku in inputDto.SkuDetails)
                    {
                        isFirstRecord = false;
                        parentId = 0;
                        if (!isFirstRecord)
                        {
                            var parentPremium = new PremiumUser()
                            {
                                OilTypeId = inputDto.OilTypeId,
                                SkuId = sku.SkuId,
                                UserId = inputDto.CustomerId.FirstOrDefault(),
                                ActualPremium = inputDto.ActualPremium,
                                ValidFrom = inputDto.ValidFrom,
                                ValidTo = inputDto.ValidTo,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                ParentId = parentId,
                                ParentPremiumId = sku.ParentId
                            };
                            _emamiContext.PremiumUser.Add(parentPremium);
                            _emamiContext.SaveChanges();

                            parentId = parentPremium.Id;
                            isFirstRecord = true;
                        }
                        foreach (var userid in inputDto.CustomerId)
                        {
                            var premium = new PremiumUser()
                            {
                                OilTypeId = inputDto.OilTypeId,
                                SkuId = sku.SkuId,
                                UserId = userid,
                                ActualPremium = inputDto.ActualPremium,
                                ValidFrom = inputDto.ValidFrom,
                                ValidTo = inputDto.ValidTo,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                ParentId = parentId,
                                ParentPremiumId = sku.ParentId,
                            };
                            _emamiContext.PremiumUser.Add(premium);
                        }
                        _emamiContext.SaveChanges();
                    }

                    resultDto.IsSuccess = true;
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                }
                return _resultService.SuccessObject(resultDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto UpdateDiscountUsers(DiscountUserDto inputDto)
        {
            _methodName = "UpdateDiscountUsers";
            var resultDto = new ResultDto();
            bool isFirstRecord = false;
            var isExistsData = false;

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
                if (inputDto.CustomerId == null || !inputDto.CustomerId.Any())
                {
                    return _resultService.ErrorMessage(Constants.EmployeeIsEmpty);
                }

                var discountUserData = _emamiContext.DiscountUsers.AsNoTracking().Where(f => f.ParentDiscountId == inputDto.Id);
                if (discountUserData != null && discountUserData.Any())
                {
                    return _resultService.ErrorMessage(Constants.DiscountAlreadyProcessed);
                }

                //var parentDiscountId = _emamiContext.DiscountUsers.FirstOrDefault(f => f.Id == inputDto.Id).ParentDiscountId;
                //var discountData = _emamiContext.DiscountUsers.FirstOrDefault(f => f.Id == parentDiscountId);

                //if (inputDto.ActualDiscount > discountData.ActualDiscount)
                //{
                //    return _resultService.ErrorMessage("Discount limit is " + discountData.ActualDiscount + ". Please enter less then or equal to discount");
                //}

                //if (!(inputDto.ValidFrom.Date >= discountData.ValidFrom.Date && inputDto.ValidFrom.Date <= discountData.ValidTo.Date
                //&& inputDto.ValidTo.Date <= discountData.ValidTo.Date && inputDto.ValidTo.Date >= discountData.ValidFrom.Date))
                //{
                //    return _resultService.ErrorMessage("Discount date range is " + discountData.ValidFrom.ToString("dd-MMM-yyyy") + " - " + discountData.ValidTo.ToString("dd-MMM-yyyy") + ". Please select dates between the range");
                //}

                var discountDatas = _emamiContext.DiscountUsers.Where(f => f.ParentId == inputDto.Id).Select(s => new { SkuIds = s.SkuId, UserIds = s.UserId }).ToList();
                var dbEmployess = (discountDatas != null && discountDatas.Any()) ? discountDatas.Select(s => s.UserIds).Distinct().ToList() : null;

                //Get Removed Employees
                var removedEmployees = (dbEmployess != null && dbEmployess.Any() && inputDto.CustomerId != null && inputDto.CustomerId.Any())
                    ? dbEmployess.Where(w => !inputDto.CustomerId.Contains(w)).Distinct().ToList() : null;

                if (removedEmployees != null && removedEmployees.Any())
                {
                    var removedData = _emamiContext.DiscountUsers.Where(f => removedEmployees.Contains(f.UserId) && f.ParentId == inputDto.Id);
                    if (removedData != null)
                    {
                        removedData.ToList().ForEach(f => _emamiContext.DiscountUsers.Remove(f));
                        _emamiContext.SaveChanges();
                    }
                }

                var newEmployees = (dbEmployess != null && dbEmployess.Any() && inputDto.CustomerId != null && inputDto.CustomerId.Any())
                    ? inputDto.CustomerId.Where(w => !dbEmployess.Contains(w)) : null;

                if (newEmployees != null && newEmployees.Any())
                {
                    //foreach (var skuId in inputDto.SkuIds)
                    //{
                    foreach (var userID in newEmployees)
                    {
                        if (!isFirstRecord)
                        {
                            var entity = new DiscountUsers()
                            {
                                SkuId = inputDto.SkuId,
                                UserId = userID,
                                SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                                Status = true,
                                ActualDiscount = inputDto.ActualDiscount,
                                ValidFrom = inputDto.ValidFrom,
                                ValidTo = inputDto.ValidTo,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                OilTypeId = inputDto.OilTypeId,
                                ParentId = inputDto.Id,
                                //ParentDiscountId = parentDiscountId
                            };
                            _emamiContext.DiscountUsers.Add(entity);
                            _emamiContext.SaveChanges();
                        }
                    }
                    //}
                }


                var discounts = _emamiContext.DiscountUsers.Where(f => f.ParentId == inputDto.Id || f.Id == inputDto.Id).ToList();
                if (discounts != null && discounts.Any())
                {
                    foreach (var discount in discounts)
                    {
                        discount.ActualDiscount = inputDto.ActualDiscount;
                        discount.ValidFrom = inputDto.ValidFrom;
                        discount.ValidTo = inputDto.ValidTo;
                        discount.ModifiedBy = inputDto.LoginUserId;
                        discount.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        _emamiContext.SaveChanges();
                    }
                }

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

        public ResultDto AddEmployeeAndUserDiscount(EmployeeUserDiscountDto inputDto)
        {
            _methodName = "AddEmployeeAndUserDiscount";
            var resultDto = new ResultDto();
            bool isFirstRecord = false;
            long parentId = 0;
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
                if (inputDto.CustomerId == null || !inputDto.CustomerId.Any())
                {
                    return _resultService.ErrorMessage(Constants.EmployeeIsEmpty);
                }

                var discountData = _emamiContext.DiscountUsers.AsNoTracking().FirstOrDefault(f => f.Id == inputDto.Id);

                if (discountData != null)
                {
                    if (!(inputDto.ValidFrom.Date >= discountData.ValidFrom.Date && inputDto.ValidFrom.Date <= discountData.ValidTo.Date
                        && inputDto.ValidTo.Date <= discountData.ValidTo.Date && inputDto.ValidTo.Date >= discountData.ValidFrom.Date))
                    {
                        return _resultService.ErrorMessage("Please select a Valid From and To date");
                    }

                    if (!(inputDto.ActualDiscount <= discountData.ActualDiscount))
                    {
                        return _resultService.ErrorMessage("Discount limit is " + discountData.ActualDiscount + ". Please enter less than or equal to discount");
                    }

                    foreach (var userid in inputDto.CustomerId)
                    {
                        if (!isFirstRecord)
                        {
                            var parentDiscount = new DiscountUsers()
                            {
                                OilTypeId = inputDto.OilTypeId,
                                SkuId = inputDto.SkuId,
                                UserId = userid,
                                SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                                Status = true,
                                ActualDiscount = inputDto.ActualDiscount,
                                ValidFrom = inputDto.ValidFrom,
                                ValidTo = inputDto.ValidTo,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                ParentId = parentId,
                                ParentDiscountId = inputDto.Id
                            };
                            _emamiContext.DiscountUsers.Add(parentDiscount);
                            _emamiContext.SaveChanges();

                            parentId = parentDiscount.Id;
                            isFirstRecord = true;
                        }
                        if (isFirstRecord)
                        {
                            var discount = new DiscountUsers()
                            {
                                OilTypeId = inputDto.OilTypeId,
                                SkuId = inputDto.SkuId,
                                UserId = userid,
                                SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess,
                                Status = true,
                                ActualDiscount = inputDto.ActualDiscount,
                                ValidFrom = inputDto.ValidFrom,
                                ValidTo = inputDto.ValidTo,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                ParentId = parentId,
                                ParentDiscountId = inputDto.Id
                            };
                            _emamiContext.DiscountUsers.Add(discount);
                        }
                    }
                    _emamiContext.SaveChanges();
                    resultDto.IsSuccess = true;
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                }
                return _resultService.SuccessObject(resultDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto UpdatePremium(PremiumUserDto inputDto)
        {
            _methodName = "UpdatePremium";
            var resultDto = new ResultDto();
            bool isFirstRecord = false;
            bool isExistsData = false;
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
                if (inputDto.CustomerId == null || !inputDto.CustomerId.Any())
                {
                    return _resultService.ErrorMessage(Constants.EmployeeIsEmpty);
                }

                var premiumUserData = _emamiContext.PremiumUser.AsNoTracking().Where(f => f.ParentPremiumId == inputDto.Id);
                if (premiumUserData != null && premiumUserData.Any())
                {
                    return _resultService.ErrorMessage(Constants.PremiumAlreadyProcessed);
                }

                //var parentPremiumId = _emamiContext.PremiumUser.FirstOrDefault(f => f.Id == inputDto.Id).ParentPremiumId;
                //var premiumData = _emamiContext.PremiumUser.FirstOrDefault(f => f.Id == parentPremiumId);

                //if (!(inputDto.ValidFrom.Date >= premiumData.ValidFrom.Date && inputDto.ValidFrom.Date <= premiumData.ValidTo.Date
                //&& inputDto.ValidTo.Date <= premiumData.ValidTo.Date && inputDto.ValidTo.Date >= premiumData.ValidFrom.Date))
                //{
                //    return _resultService.ErrorMessage("Discount date range is " + premiumData.ValidFrom.ToString("dd-MMM-yyyy") + " - " + premiumData.ValidTo.ToString("dd-MMM-yyyy") + ". Please select dates between the range");
                //}

                //if (inputDto.ActualPremium <= premiumData.ActualPremium)
                //{
                var discountDatas = _emamiContext.PremiumUser.Where(f => f.ParentId == inputDto.Id).Select(s => new { SkuIds = s.SkuId, UserIds = s.UserId }).ToList();
                var dbEmployess = (discountDatas != null && discountDatas.Any()) ? discountDatas.Select(s => s.UserIds).Distinct().ToList() : null;

                //Get Removed Employees
                var removedEmployees = (dbEmployess != null && dbEmployess.Any() && inputDto.CustomerId != null && inputDto.CustomerId.Any())
                    ? dbEmployess.Where(w => !inputDto.CustomerId.Contains(w)).Distinct().ToList() : null;

                if (removedEmployees != null && removedEmployees.Any())
                {
                    var removedData = _emamiContext.PremiumUser.Where(f => removedEmployees.Contains(f.UserId) && f.ParentId == inputDto.Id);
                    if (removedData != null)
                    {
                        removedData.ToList().ForEach(f => _emamiContext.PremiumUser.Remove(f));
                        _emamiContext.SaveChanges();
                    }
                }

                var newEmployees = (dbEmployess != null && dbEmployess.Any() && inputDto.CustomerId != null && inputDto.CustomerId.Any())
                    ? inputDto.CustomerId.Where(w => !dbEmployess.Contains(w)) : null;

                if (newEmployees != null && newEmployees.Any())
                {
                    foreach (var userID in newEmployees)
                    {
                        if (!isFirstRecord)
                        {
                            var entity = new PremiumUser()
                            {
                                SkuId = inputDto.SkuId,
                                UserId = userID,
                                ActualPremium = inputDto.ActualPremium,
                                ValidFrom = inputDto.ValidFrom,
                                ValidTo = inputDto.ValidTo,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                OilTypeId = inputDto.OilTypeId,
                                ParentId = inputDto.Id,
                                //ParentPremiumId = parentPremiumId
                            };
                            _emamiContext.PremiumUser.Add(entity);
                            _emamiContext.SaveChanges();
                        }
                    }
                }
                //}
                //else
                //{
                //    return _resultService.ErrorMessage("Premium limit is " + premiumData.ActualPremium + ". Please enter less then or equal to discount");
                //}

                var premiums = _emamiContext.PremiumUser.Where(f => f.ParentId == inputDto.Id || f.Id == inputDto.Id).ToList();
                if (premiums != null && premiums.Any())
                {
                    foreach (var discount in premiums)
                    {
                        discount.ActualPremium = inputDto.ActualPremium;
                        discount.ValidFrom = inputDto.ValidFrom;
                        discount.ValidTo = inputDto.ValidTo;
                        discount.ModifiedBy = inputDto.LoginUserId;
                        discount.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        _emamiContext.SaveChanges();
                    }
                }


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

        public ResultDto AssignPremium(EmployeeUserPremiumDto inputDto)
        {
            _methodName = "AssignPremium";
            var resultDto = new ResultDto();
            bool isFirstRecord = false;
            long parentId = 0;
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
                if (inputDto.CustomerId == null || !inputDto.CustomerId.Any())
                {
                    return _resultService.ErrorMessage(Constants.EmployeeIsEmpty);
                }

                var discountData = _emamiContext.PremiumUser.AsNoTracking().FirstOrDefault(f => f.Id == inputDto.Id);

                if (discountData != null)
                {

                    if (!(inputDto.ValidFrom.Date >= discountData.ValidFrom.Date && inputDto.ValidFrom.Date <= discountData.ValidTo.Date
                        && inputDto.ValidTo.Date <= discountData.ValidTo.Date && inputDto.ValidTo.Date >= discountData.ValidFrom.Date))
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = "Please select a Valid From and To date";
                        return resultDto;
                    }

                    if (!(inputDto.ActualPremium <= discountData.ActualPremium))
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = "Premium limit is " + discountData.ActualPremium + ". Please enter less than or equal to premium";
                        return resultDto;
                    }

                    foreach (var userid in inputDto.CustomerId)
                    {
                        if (!isFirstRecord)
                        {
                            var parentDiscount = new PremiumUser()
                            {
                                OilTypeId = inputDto.OilTypeId,
                                SkuId = inputDto.SkuId,
                                UserId = userid,
                                ActualPremium = inputDto.ActualPremium,
                                ValidFrom = inputDto.ValidFrom,
                                ValidTo = inputDto.ValidTo,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                ParentId = parentId,
                                ParentPremiumId = inputDto.Id
                            };
                            _emamiContext.PremiumUser.Add(parentDiscount);
                            _emamiContext.SaveChanges();

                            parentId = parentDiscount.Id;
                            isFirstRecord = true;
                        }
                        if (isFirstRecord)
                        {
                            var discount = new PremiumUser()
                            {
                                OilTypeId = inputDto.OilTypeId,
                                SkuId = inputDto.SkuId,
                                UserId = userid,
                                ActualPremium = inputDto.ActualPremium,
                                ValidFrom = inputDto.ValidFrom,
                                ValidTo = inputDto.ValidTo,
                                CreatedBy = inputDto.LoginUserId,
                                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                ParentId = parentId,
                                ParentPremiumId = inputDto.Id
                            };
                            _emamiContext.PremiumUser.Add(discount);
                        }
                    }
                    _emamiContext.SaveChanges();
                    resultDto.IsSuccess = true;
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                }
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

        #region Daily Booked Sauda Report
        public ResultDto DailyBookedSaudaReport(DailyBookedSaudaInputDto inputDto)
        {
            var dailyBookedSaudaOutputDto = new List<DailyBookedSaudaOutputDto>();
            var BookedSaudaOutputDto = new List<BookedSaudaOutputDto>();
            var plantIds = new List<long>();
            var reportData = new SaudaReportDtoNH();
            _methodName = "DailyBookedSaudaReport";
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                List<long> ZHList = new List<long>();

                if (inputDto.NationalHeadIds.IsAny())
                {
                    //New Reporting to change
                    //ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId != null && inputDto.NationalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                    ZHList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => inputDto.NationalHeadIds.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();
                }
                else
                {
                    //long roleId = rolesContext.FirstOrDefault(_ => _.UserId == inputDto.LoginUserId).RoleId;
                    if (userRoleContext.RoleId == (int)DTO.Enums.Role.Admin)
                    {
                        var nationalHeadIds = _emamiContext.UserRoles.Where(_ => _.RoleId == (int)DTO.Enums.Role.NationalTrader).Select(_ => _.UserId).ToList();
                        //New Reporting to change
                        //ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId != null && nationalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                        ZHList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => nationalHeadIds.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();
                    }
                }
                //var ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.OrganizationReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                if (ZHList != null && ZHList.Any())
                {
                    //New Reporting to change
                    //var bdoList = _emamiContext.Users.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToId ?? 0)).Select(_ => _.Id).ToList();
                    var bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();
                    if (bdoList != null && bdoList.Any())
                    {
                        var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(a => a.CustomerId).ToList();
                        if (inputDto.PlantId == 0)
                        {
                            plantIds = _emamiContext.UserDepotMapping.AsNoTracking().Where(w => dealerIds.Contains(w.UserId)).Select(s => s.DepotId).Distinct().ToList();
                            // plantIds = _emamiContext.Depots.AsNoTracking()/*.Where(w => w.IsPlant)*/.Select(s => s.Id).ToList();
                        }
                        else
                        {
                            plantIds = _emamiContext.UserDepotMapping.AsNoTracking()
                                  .Join(_emamiContext.Depots.AsNoTracking(), ud => ud.DepotId, d => d.Id, (ud, d) => new
                                  {
                                      UserDepot = ud,
                                      Depot = d
                                  })
                                  //.Join(_emamiContext.PlantDepotMapping.AsNoTracking(), ud => ud.Depot.Id, pd => pd.DepotId, (ud, pd) => new
                                  //{
                                  //    UserDepot = ud.UserDepot,
                                  //    Depot = ud.Depot,
                                  //    PlantDepot = pd
                                  //})
                                  .Where(w => w.Depot.Id == inputDto.PlantId
                                  && dealerIds.Contains(w.UserDepot.UserId))
                                  .Select(s => s.Depot.Id).Distinct().ToList();
                            plantIds.Add(inputDto.PlantId);
                        }
                        if (dealerIds != null && dealerIds.Any())
                        {

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
                                divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                             .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });
                            }

                            IEnumerable<DailyBookedSaudaOutputDto> saudaContext = new List<DailyBookedSaudaOutputDto>();
                            using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                            {
                                var sqlQuery = @"CREATE TABLE #DealerTemp(DealerId BIGINT)
                                    CREATE TABLE #BdoTemp(BdoId BIGINT)
                                    CREATE TABLE #ZHTemp(ZHId BIGINT)
                                    CREATE TABLE #PlantTemp(DealerId BIGINT)
                                    Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                                    Declare @RoleId bigint
                                    insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                                    select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

                                    if(@NTString!='')
                                    begin
	                                    Insert Into #ZHTemp select UserId from UserReportingToMappings where ReportingToUserId in (
	                                    Select Data From dbo.Split(@NTString,','))
	                                    insert into #BdoTemp select UserId from UserReportingToMappings where ReportingToUserId in (select ZHId from #ZHTemp)
	                                    Insert into #DealerTemp select CustomerId from UserCustomerMappings where UserId in (select BdoId from #BdoTemp)
                                    end
                                    else
                                    begin
	                                    select @RoleId=RoleId from UserRoles where UserId=@UserId
	                                    if(@RoleId=1)
	                                    begin
	                                    Insert into #ZHTemp select UserId from UserReportingToMappings urp where ReportingToUserId in (select UserId from UserRoles where RoleId=12)
	                                    Insert into #BdoTemp select UserId from UserReportingToMappings where ReportingToUserId in (select ZHId from #ZHTemp)
	                                    Insert into #DealerTemp select CustomerId from UserCustomerMappings where UserId in (select BdoId from #BdoTemp)
	                                    end
	
                                    end

                                    if(@PlantId > 0)
                                    begin 
                                     insert into #PlantTemp select DepotId from UserDepotMappings where UserId in (select DealerId from #DealerTemp) or DepotId=@PlantId
                                    end

                                    select 
                                    s.UserId,
                                    o.Id as OilTypeId,
                                    sku.PackGroupId as ProductGroupId,
                                    sku.OilPackGroupTypeId as OilPackGroupType,
                                    so.CreatedDate as BookedDate,
                                    --o.Name as OilType,
                                    p.Name as ProductGroup,
                                    so.BidQuantity as QuantityInMT,
                                    so.BidQuantityCase as QuantityCase,
                                    u.StateId as StateId,
                                    (o.Name+'-'+sorg.Code+'/'+dist.Code+'/'+div.Code) as OilType,
                                    (sku.SkuName +'-'+sku.SkuCode) as SkuName,
                                    sku.Id as SkuId,
                                    state.StateName
                                    from Saudas s with(NOLOCK)
                                    join SaudaOrders so with(NOLOCK) on s.Id=so.SaudaId
                                    join Users u on s.UserId=u.Id
                                    join #UserDivision ud on ud.SalesOrganizationId=s.SalesOrganizationId
                                    and ud.DistributionChannelId=s.DistributionChannelId and ud.DivisionId=s.DivisionId
                                    join Skus sku on so.SkuId=sku.Id
                                    join PackGroups p on sku.PackGroupId=p.Id
                                    join OilTypes o on sku.OilTypeId=o.Id
                                    join SalesOrganizations sorg on o.SalesOrganizationId=sorg.Id
                                    join DistributionChannels dist on o.DistributionChannelId=dist.Id
                                    join Divisions div on o.DivisionId=div.Id
                                    join States as state on u.StateId = state.Id
                                    where 
                                    Cast(s.BiddingDate as date) >= Cast(@FromDate as date)
                                    and Cast(s.BiddingDate as date) <= Cast(@ToDate as date)
                                    and s.UserId in (select DealerId from #DealerTemp)
                                    and ((@PlantId > 0 and so.PlantId in (select PlantId from #PlantTemp)) or @PlantId=0)
                                    and so.StatusId !=3 --Rejected StatusId=3


                                     drop table #DealerTemp
                                     drop table #BdoTemp
                                     drop table #PlantTemp
                                    drop table #UserDivision
                                    drop table #ZHTemp";
                                var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                                saudaContext = conn.Query<DailyBookedSaudaOutputDto>(sqlQuery, new
                                {
                                    UserId = inputDto.LoginUserId,
                                    PlantId = inputDto.PlantId,
                                    NTString = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.NationalHeadIds),
                                    FromDate = inputDto.FromDate,
                                    ToDate = inputDto.ToDate
                                });

                            }
                           
                            if (saudaContext != null)
                            {                                
                                if (inputDto.OilTypes != null && inputDto.OilTypes.Any())
                                {
                                    saudaContext = saudaContext.Where(_ => inputDto.OilTypes.Contains(_.OilTypeId));
                                }
                                if (inputDto.PackTypes != null && inputDto.PackTypes.Any())
                                {
                                    saudaContext = saudaContext.Where(_ => inputDto.PackTypes.Contains(_.ProductGroupId));
                                }
                                if (inputDto.StateIds != null && inputDto.StateIds.Any())
                                {
                                    saudaContext = saudaContext.Where(_ => inputDto.StateIds.Contains(_.StateId));
                                }
                                if (inputDto.OilPackGroupTypes > 0)
                                {
                                    if (inputDto.OilPackGroupTypes == (int)DTO.Enums.BpCpType.BP)
                                    {
                                        saudaContext = saudaContext.Where(_ => _.OilPackGroupType == (int)DTO.Enums.BpCpType.BP);
                                    }
                                    if (inputDto.OilPackGroupTypes == (int)DTO.Enums.BpCpType.CP)
                                    {
                                        saudaContext = saudaContext.Where(_ => _.OilPackGroupType == (int)DTO.Enums.BpCpType.CP);
                                    }
                                }
                                if (saudaContext != null)
                                {
                                    dailyBookedSaudaOutputDto = saudaContext.ToList().Select(_ => new DailyBookedSaudaOutputDto()
                                    {
                                        BookedDate = _.BookedDate,
                                        UserId = _.UserId,
                                        OilType = _.OilType,
                                        OilTypeId = _.OilTypeId,
                                        ProductGroupId = _.ProductGroupId,
                                        OilPackGroupType = _.OilPackGroupType,
                                        ProductGroup = _.ProductGroup,
                                        QuantityInMT = _.QuantityInMT,
                                        QuantityCase = _.QuantityCase,
                                        SkuName = _.SkuName,
                                        SkuId = _.SkuId,
                                        StateId = _.StateId,
                                        StateName = _.StateName
                                    }).ToList();
                                }

                                if (dailyBookedSaudaOutputDto != null && dailyBookedSaudaOutputDto.Any())
                                {
                                    reportData.QuantityInMT = dailyBookedSaudaOutputDto.Sum(s => s.QuantityInMT);
                                    reportData.QuantityCase = dailyBookedSaudaOutputDto.Sum(s => s.QuantityCase);
                                    reportData.StateList = dailyBookedSaudaOutputDto
                                        .GroupBy(x => new { x.StateId })
                                        .Select(stateGroup => new StateList
                                        {
                                            StateId = stateGroup.Key.StateId,
                                            StateName = stateGroup.FirstOrDefault().StateName,
                                            QuantityInMT = stateGroup.Sum(s => s.QuantityInMT),
                                            QuantityCase = stateGroup.Sum(s => s.QuantityCase),
                                            OilTypes = stateGroup
                                                .GroupBy(o => new { o.OilTypeId })
                                                .Select(oilGroup => new OilTypeList
                                                {
                                                    OilTypeId = oilGroup.Key.OilTypeId,
                                                    OilType = oilGroup.FirstOrDefault().OilType,
                                                    QuantityInMT = oilGroup.Sum(q => q.QuantityInMT),
                                                    QuantityCase = oilGroup.Sum(q => q.QuantityCase),
                                                    SkuListReportDto = oilGroup
                                                        .GroupBy(s => new { s.SkuId})
                                                        .Select(sku => new SkuListReportDto
                                                        {
                                                            SkuName = sku.FirstOrDefault().SkuName,
                                                            BidQuantity = sku.Sum(_ => _.QuantityInMT),
                                                            BidQuantityCase = sku.Sum(_ => _.QuantityCase)
                                                        })
                                                        .ToList()
                                                }).ToList()
                                        }).ToList();
                                }
                            }
                        }
                        else
                        {
                            return _resultService.ErrorMessage(Constants.RecordNotFound);
                        }
                    }
                    else
                    {
                        return _resultService.ErrorMessage(Constants.RecordNotFound);
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
            return _resultService.SuccessObject(reportData);
        }

        public ResultDto SalesReport(DailyBookedSaudaInputDto inputDto)
        {
            _methodName = "SalesReport";
            var dailyBookedSaudaOutputDto = new List<DailyBookedSaudaOutputDto>();
            var BookedSaudaOutputDto = new List<BookedSaudaOutputDto>();
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                //var usersContext = _emamiContext.Users;
                var usersDivisionContext = _emamiContext.UserDivisionMappings;
                var rolesContext = _emamiContext.UserRoles;
                List<long> ZHList = new List<long>();
                List<long> VerticalList = new List<long>();
                long roleId = rolesContext.FirstOrDefault(_ => _.UserId == inputDto.LoginUserId).RoleId;
                if (inputDto.ZHs != null && inputDto.ZHs.Any())
                {
                    ZHList.AddRange(inputDto.ZHs);
                }
                else
                {
                    if (inputDto.NationalHeadIds.IsAny())
                    {
                        //New Reporting to table change
                        //ZHList = usersContext.Where(_ => _.ReportingToId != null && inputDto.NationalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                        ZHList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => inputDto.NationalHeadIds.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();
                        //VerticalList = usersDivisionContext.Where(_ =>  inputDto.NationalHeadIds.Contains(_.UserId)).Select(_ => (long)_.DivisionId).Distinct().ToList();
                    }
                    else
                    {

                        if (roleId == (int)DTO.Enums.Role.Admin)
                        {
                            var nationalHeadIds = rolesContext.Where(_ => _.RoleId == (int)DTO.Enums.Role.NationalTrader).Select(_ => _.UserId).ToList();
                            //New Reporting to table change
                            //ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId != null && nationalHeadIds.Contains(_.ReportingToId.Value)).Select(_ => _.Id).ToList();
                            ZHList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => nationalHeadIds.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();
                        }
                    }
                    //ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.OrganizationReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                }

                if (ZHList != null && ZHList.Any())
                {

                    IEnumerable<DailyBookedSaudaOutputDto> invoiceContext = new List<DailyBookedSaudaOutputDto>();
                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {
                        invoiceContext = conn.Query<DailyBookedSaudaOutputDto>("GetSalesReport", new
                        {
                            UserId = inputDto.LoginUserId,
                            ZHString = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.ZHs),
                            NTString = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.NationalHeadIds),
                            FromDate = inputDto.FromDate,
                            ToDate = inputDto.ToDate
                        }, commandType: CommandType.StoredProcedure, commandTimeout: 300);

                    }
                    var cityContext = _emamiContext.City.AsNoTracking();
                    var stateContext = _emamiContext.State.AsNoTracking();
                    if (invoiceContext != null && invoiceContext.Any())
                    {
                        if (inputDto.Dealers != null && inputDto.Dealers.Any())
                        {
                            invoiceContext = invoiceContext.Where(_ => inputDto.Dealers.Contains(_.UserId));
                        }
                        
                        if (inputDto.OilTypes != null && inputDto.OilTypes.Any())
                        {
                            invoiceContext = invoiceContext.Where(_ => inputDto.OilTypes.Contains(_.OilTypeId));
                        }
                        if (inputDto.PackTypes != null && inputDto.PackTypes.Any())
                        {
                            invoiceContext = invoiceContext.Where(_ => inputDto.PackTypes.Contains(_.ProductGroupId));
                        }
                        if (inputDto.OilPackGroupTypes > 0)
                        {
                            if (inputDto.OilPackGroupTypes == (int)DTO.Enums.BpCpType.BP) 
                            {
                                invoiceContext = invoiceContext.Where(_ => _.OilPackGroupType == (int)DTO.Enums.BpCpType.BP);
                            }
                            if (inputDto.OilPackGroupTypes == (int)DTO.Enums.BpCpType.CP) 
                            {
                                invoiceContext = invoiceContext.Where(_ => _.OilPackGroupType == (int)DTO.Enums.BpCpType.CP);
                            }
                        }
                        if (inputDto.StateIds != null && inputDto.StateIds.Any())
                        {
                            invoiceContext = invoiceContext.Where(_ => inputDto.StateIds.Contains(_.StateId));
                        }
                        if (invoiceContext != null)
                        {
                            dailyBookedSaudaOutputDto = invoiceContext.Select(_ => new DailyBookedSaudaOutputDto()
                            {
                                BookedDate = _.BookedDate,
                                OilType = _.OilType,
                                OilTypeId = _.OilTypeId,
                                ProductGroupId = _.ProductGroupId,
                                OilPackGroupType = _.OilPackGroupType,
                                ProductGroup = _.ProductGroup,
                                QuantityInMT = _.QuantityInMT,
                                QuantityCase = _.QuantityCase
                            }).ToList();
                        }

                        if (dailyBookedSaudaOutputDto != null && dailyBookedSaudaOutputDto.Any())
                        {
                            var bookedSaudaoutput = dailyBookedSaudaOutputDto.GroupBy(_ => new
                            {
                                _.OilType,
                                _.OilTypeId,
                                _.ProductGroup,
                                _.ProductGroupId,
                                _.OilPackGroupType
                            }).Select(_ => new DailyBookedSaudaOutputDto()
                            {
                                OilType = _.Key.OilType,
                                OilTypeId = _.Key.OilTypeId,
                                ProductGroup = _.Key.ProductGroup,
                                ProductGroupId = _.Key.ProductGroupId,
                                OilPackGroupType = _.Key.OilPackGroupType,
                                QuantityInMT = _.Sum(s => s.QuantityInMT),
                                QuantityCase = _.Sum(s => s.QuantityCase)
                            }).ToList();

                            if (bookedSaudaoutput != null && bookedSaudaoutput.Any())
                            {
                                foreach (var item in bookedSaudaoutput)
                                {
                                    var dto = new BookedSaudaOutputDto();
                                    var checkOilTypeExists = BookedSaudaOutputDto.FirstOrDefault(_ => _.OilTypeId == item.OilTypeId);
                                    if (checkOilTypeExists == null)
                                    {
                                        dto.OilTypeId = item.OilTypeId;
                                        dto.OilType = item.OilType;
                                        //dto.MaterialType = item.MaterialType;
                                        BookedSaudaOutputDto.Add(dto);
                                    }
                                }
                            }
                            if (BookedSaudaOutputDto != null && BookedSaudaOutputDto.Any())
                            {
                                foreach (var item in BookedSaudaOutputDto)
                                {
                                    var Dto = BookedSaudaOutputDto.FirstOrDefault(_ => _.OilTypeId == item.OilTypeId);
                                    if (Dto != null)
                                    {
                                        var PremiumPack = bookedSaudaoutput.Where(_ => _.OilTypeId == item.OilTypeId && _.ProductGroupId == (int)DTO.Enums.PackGroupType.Premium);
                                        if (PremiumPack != null && PremiumPack.Any())
                                        {
                                            Dto.premiumquantityInMT = PremiumPack.Sum(_ => _.QuantityInMT);
                                            Dto.PremiumQuantityCase = PremiumPack.Sum(_ => _.QuantityCase);
                                        }
                                        var LauricPack = bookedSaudaoutput.Where(_ => _.OilTypeId == item.OilTypeId && _.ProductGroupId == (int)DTO.Enums.PackGroupType.Lauric);
                                        if (LauricPack != null && LauricPack.Any())
                                        {
                                            Dto.LauricquantityInMT = LauricPack.Sum(_ => _.QuantityInMT);
                                            Dto.LauricQuantityCase = LauricPack.Sum(_ => _.QuantityCase);
                                        }
                                        var PopularPack = bookedSaudaoutput.Where(_ => _.OilTypeId == item.OilTypeId && _.ProductGroupId == (int)DTO.Enums.PackGroupType.Popular);
                                        if (PopularPack != null && PopularPack.Any())
                                        {
                                            Dto.PopularquantityInMT = PopularPack.Sum(_ => _.QuantityInMT);
                                            Dto.PopularQuantityCase = PopularPack.Sum(_ => _.QuantityCase);
                                        }
                                        var BakeryPack = bookedSaudaoutput.Where(_ => _.OilTypeId == item.OilTypeId && _.ProductGroupId == (int)DTO.Enums.PackGroupType.Bakery);
                                        if (BakeryPack != null && BakeryPack.Any())
                                        {
                                            Dto.BakeryquantityInMT = BakeryPack.Sum(_ => _.QuantityInMT);
                                            Dto.BakeryQuantityCase = BakeryPack.Sum(_ => _.QuantityCase);
                                        }
                                        Dto.QuantityInMT = Dto.premiumquantityInMT + Dto.LauricquantityInMT + Dto.PopularquantityInMT + Dto.BakeryquantityInMT;
                                        Dto.QuantityCase = Dto.PremiumQuantityCase + Dto.LauricQuantityCase + Dto.PopularQuantityCase + Dto.BakeryQuantityCase;
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
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
            return _resultService.SuccessObject(BookedSaudaOutputDto);
        }

        public ResultDto GetPendingSaudaChartDetailForMobile(LoginNHId inputDto)
        {
            _methodName = "GetPendingSaudaChartDetailForMobile";
            var resultDto = new ResultDto();
            var saudaListDto = new List<SaudaPendinglistOutputDto>();
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

                #region New Code

                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    var sqlQuery = @"CREATE TABLE #ZHTemp(ZHId BIGINT)
                        CREATE TABLE #BdoTemp(BdoId BIGINT)
                        CREATE TABLE #DealerTemp(DealerId BIGINT)
                        Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                        insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                        select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId
                        if(@ZHId=0)
                        begin
                         insert into #ZHTemp(ZHId) select UserId from UserReportingToMappings where ReportingToUserId=@UserId
                         insert into #BdoTemp(BdoId) select UserId from UserReportingToMappings 
                         where ReportingToUserId in (select ZHId from #ZHTemp)
                         insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
                         where UserId in (select BdoId from #BdoTemp)
                        end
                        else
                        begin
                        insert into #ZHTemp(ZHId) select @ZHId
	                        insert into #BdoTemp(BdoId) select  UserId from UserReportingToMappings where ReportingToUserId in (select ZHId from #ZHTemp)
	                         insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
                         where UserId in (select BdoId from #BdoTemp)
                        end
                        select 
                        pc.Id,
                        (Case when s.SaudaNumber is null then 0 else s.Id end) as SaudaOrderId,
                        (Case when pc.ContractValidFrom is null then Cast('0001-01-01T00:00:00' as datetime2) else pc.ContractValidFrom end) as BiddingDate,
                        u.Id as UserId,
                        u.Name as [User],
                        (Case when c.CityName is null then '' else c.CityName end) as City,
                        pc.BasicRate as TotalBidPrice,
                        pc.SaudaQuantity as TotalBidQuantity,
                        (o.Name+'-'+sorg.Code+'/'+dist.Code+'/'+div.Code) as OilTypename,
                        o.Id as OilTypeId,
                        pc.SaudaNumber
                        from PendingContracts pc with(NOLOCK)
                        join Users u on pc.UserId=u.Id
                        left join Saudas s on pc.SaudaNumber=s.SaudaNumber
                        left join Cities c on u.CityId=c.Id
                        join Skus sku on pc.MaterialCode=sku.SkuCode and pc.SalesOrgId=sku.SalesOrganizationId and pc.DistChnlId=sku.DistributionChannelId
                        and pc.DivisionId=sku.DivisionId
                        join OilTypes o on sku.OilTypeId=o.Id
                        join SalesOrganizations sorg on o.SalesOrganizationId=sorg.Id
                        join DistributionChannels dist on o.DistributionChannelId=dist.Id
                        join Divisions div on o.DivisionId=div.Id
                        join #UserDivision udiv on udiv.SalesOrganizationId=pc.SalesOrgId and udiv.DistributionChannelId=pc.DistChnlId
                        and pc.DivisionId=udiv.DivisionId
                        where 
                        pc.UserId in (select DealerId from #DealerTemp)
                        and pc.PendingQuantityInCase > 0.99
                        order by pc.Id desc
                        drop table #UserDivision
                        drop table #BdoTemp
                        drop table #ZhTemp
                        drop table #DealerTemp";
                    saudaListDto = conn.Query<SaudaPendinglistOutputDto>(sqlQuery, new
                    {
                        UserId = inputDto.LoginUserId,
                        ZHId = inputDto.ZHId
                    }).ToList();

                }


                #endregion

                #region OldCode

                //List<long> ZHList = new List<long>();
                //if (inputDto.ZHId > 0)
                //{
                //    ZHList.Add(inputDto.ZHId);
                //}
                //else
                //{
                //    //ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                //    ZHList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                //}
                //if (ZHList != null && ZHList.Any())
                //{
                //    //var bdoList = _emamiContext.Users.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToId ?? 0)).Select(_ => _.Id).ToList();
                //    var bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();
                //    if (bdoList != null && bdoList.Any())
                //    {
                //        List<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();

                //        var divisionsloginWiseuser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                //        .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });
                //        var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                //        if (dealersList != null && dealersList.Any())
                //        {
                //            var city = _emamiContext.City.AsQueryable();
                //            var saudacontext = _emamiContext.Sauda.AsQueryable();
                //            // saudaListDto = _emamiContext.PendingContracts.AsNoTracking().Join(_emamiContext.Sauda.AsNoTracking(), so => so.SaudaNumber, s => s.SaudaNumber, (so, s) => new { so, s })
                //            // .Join(_emamiContext.Users.AsNoTracking(), x => x.so.UserId, u => u.Id, (x, u) => new { x, u })
                //            ////.Join(_emamiContext.City.AsNoTracking(), x => x.u.CityId, c => c.Id, (x, c) => new { x.x, DealerName = x.u.Name, CityName = c.CityName, DealerId = x.u.Id })
                //            //.Join(_emamiContext.Skus.AsNoTracking(), s => s.x.so.MaterialCode, ss => ss.SkuCode, (s, ss) => new { s.x, ss, DealerName=s.u.Name, CityId=s.u.CityId, DealerId=s.u.Id })
                //            //.Join(_emamiContext.Sauda.AsNoTracking(), s => s.x.so.SaudaNumber, sauda => sauda.SaudaNumber, (s, sauda) => new { s.x,s.ss, s.DealerName, s.CityId, s.DealerId , sauda })
                //            //.Where(_ => _.x.so.PendingQuantityInCase != 0 && dealersList.Contains(_.DealerId)
                //            //&& _.x.so.SalesOrgId == _.ss.SalesOrganizationId && _.x.so.DistChnlId == _.ss.DistributionChannelId
                //            // && _.x.so.DivisionId == _.ss.DivisionId).Select(item => new SaudaListDto()
                //            // {
                //            //     Id = item.x.so.Id,
                //            //     SaudaOrderId = item.x.s.Id, //Sauda table Id
                //            //     UserId = item.DealerId,
                //            //     User = item.DealerName,
                //            //     City = city.FirstOrDefault(_ => _.Id==item.CityId)!=null ? city.FirstOrDefault(_ => _.Id==item.CityId).CityName :String.Empty,
                //            //     BiddingDate = item.x.s.BiddingDate,
                //            //     TotalBidPrice = item.x.so.BasicRate,
                //            //     TotalBidQuantity = item.x.so.SaudaQuantity,
                //            //     OiltypeName = item.ss.OilType.Name+"-"+ item.ss.OilType.SalesOrganization.Code+"/"+ item.ss.OilType.DistributionChannel.Code+"/"+ item.ss.OilType.Division.Code,
                //            //     OilTypeId = item.ss.OilType.Id,
                //            //     StatusId = 0,
                //            //     Status = string.Empty,
                //            // }).ToList();

                //            saudaListDto = (from pct in _emamiContext.PendingContracts.AsNoTracking()
                //                            where pct.PendingQuantityInCase != 0
                //                            select pct into pc
                //                            join ud in _emamiContext.Users.AsNoTracking() on pc.UserId equals ud.Id
                //                            join sku in _emamiContext.Skus.AsNoTracking() on pc.MaterialCode equals sku.SkuCode
                //                            where pc.SalesOrgId == sku.SalesOrganizationId && pc.DistChnlId == sku.DistributionChannelId && pc.DivisionId == sku.DivisionId
                //                            join o in _emamiContext.OilTypes.AsNoTracking() on sku.OilTypeId equals o.Id
                //                            join sorg in _emamiContext.SalesOrganization.AsNoTracking() on o.SalesOrganizationId equals sorg.Id
                //                            join dist in _emamiContext.DistributionChannel.AsNoTracking() on o.DistributionChannelId equals dist.Id
                //                            join div in _emamiContext.Divisions.AsNoTracking() on o.DivisionId equals div.Id
                //                            //join sauda in _emamiContext.Sauda.AsNoTracking() on pc.SaudaNumber equals sauda.SaudaNumber
                //                            join dm in divisionsloginWiseuser on new { SalesOrganizationId = pc.SalesOrgId, DistributionChannelId = pc.DistChnlId, DivisionId = pc.DivisionId }
                //                             equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId } /*into saudadb from sd in saudadb.DefaultIfEmpty()*/
                //                            where dealersList.Contains(pc.UserId)
                //                            //&& DbFunctions.TruncateTime(pc.ContractValidTo) >= DbFunctions.TruncateTime(currentDate)
                //                            //&& bdoList.Contains(sauda.BdoId)
                //                            select new SaudaPendinglistOutputDto()
                //                            {
                //                                Id = pc.Id,
                //                                SaudaOrderId = saudacontext.FirstOrDefault(_ => _.SaudaNumber == pc.SaudaNumber) != null ? saudacontext.FirstOrDefault(_ => _.SaudaNumber == pc.SaudaNumber).Id : 0,//sauda table Id
                //                                UserId = ud.Id,
                //                                User = ud.Name,
                //                                City = city.FirstOrDefault(_ => _.Id == ud.CityId) != null ? city.FirstOrDefault(_ => _.Id == ud.CityId).CityName : String.Empty,
                //                                BiddingDate = saudacontext.FirstOrDefault(_ => _.SaudaNumber == pc.SaudaNumber) != null ? saudacontext.FirstOrDefault(_ => _.SaudaNumber == pc.SaudaNumber).BiddingDate : DateTime.Today,
                //                                TotalBidPrice = pc.BasicRate,
                //                                TotalBidQuantity = pc.SaudaQuantity,
                //                                OilTypeName = o.Name + "-" + sorg.Code + "/" + dist.Code + "/" + div.Code,
                //                                OilTypeId = o.Id,
                //                                StatusId = 0,
                //                                Status = string.Empty,
                //                            }).ToList();
                //        }
                //    }
                //}
                #endregion


                if (saudaListDto != null && saudaListDto.Any())
                {
                    if (inputDto.IsPendingSauda)
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

        public ResultDto GetPendingContractChartMobile(LoginNHId inputDto)
        {
            _methodName = "GetPendingContractChartMobile";
            var resultDto = new ResultDto();
            var saudaListDto = new List<SaudaListDto>();
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
                string descru = EncryptDecryptHelper.Decrypt("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", SecurityConstants.EncryptionKey, SecurityConstants.VectorKey);

                List<long> ZHList = new List<long>();
                if (inputDto.ZHId > 0)
                {
                    ZHList.Add(inputDto.ZHId);
                }
                else
                {
                    ZHList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                }
                if (ZHList != null && ZHList.Any())
                {
                    var bdoList = _emamiContext.Users.AsNoTracking().Where(_ => ZHList.Contains(_.ReportingToId ?? 0)).Select(_ => _.Id).ToList();
                    if (bdoList != null && bdoList.Any())
                    {
                        List<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                        if (dealersList != null && dealersList.Any())
                        {
                            var saudaStatus = Constants.OutstandingSaudaStatus;
                            //var saudaOrdersContext = _emamiContext.SaudaOrders.AsNoTracking().Join(_emamiContext.Sauda.AsNoTracking(), so => so.SaudaId, s => s.Id, (so, s) => new { so, s })
                            //    .Join(_emamiContext.Users.AsNoTracking(), x => x.s.UserId, u => u.Id, (x, u) => new { x.so, x.s, u })
                            //    .Join(_emamiContext.PendingContracts.AsNoTracking(), x => x.so.Id, pc => pc.SaudaOrderId, (x, pc) => new { x.so, x.s, x.u, pc })
                            //    .Join(_emamiContext.City.AsNoTracking(), x => x.u.CityId, c => c.Id, (x, c) => new { x.so, x.s, x.pc, DealerName = x.u.Name, CityName = c.CityName })
                            //    .Join(_emamiContext.ApprovalStatus.AsNoTracking(), x => x.so.StatusId, ss => ss.Id, (x, ss) => new { x.so, x.s, x.pc, x.DealerName, x.CityName, StatusName = ss.Name })
                            //    .Where(_ => dealersList.Contains(_.s.UserId) && saudaStatus.Contains(_.so.StatusId) && _.s != null && _.so != null && _.so.OilType != null);
                            var saudaOrdersContext = _emamiContext.PendingContracts.AsNoTracking()
                            .Join(_emamiContext.Users.AsNoTracking(), x => x.CustomerCode, u => u.Code, (x, u) => new { x, u })
                            .Join(_emamiContext.City.AsNoTracking(), x => x.u.CityId, c => c.Id, (x, c) => new { x.x, x.u, DealerName = x.u.Name, CityName = c.CityName })
                            .Join(_emamiContext.Skus.AsNoTracking(), x => x.x.MaterialCode, sku => sku.SkuCode, (x, sku) => new { x.x, x.u, DealerName = x.u.Name, CityName = x.CityName, sku })
                            .Where(_ => dealersList.Contains(_.u.Id)
                            //&& _.u.DivisionId == userContext.DivisionId && _.sku.DivisionId == _.VerticalId
                            && _.x.SalesOrgId == _.sku.SalesOrganizationId && _.x.DistChnlId == _.sku.DistributionChannelId
                            && _.x.DivisionId == _.sku.DivisionId).ToList();
                            // .Where(_ => dealersList.Contains(_.u.Id) && _.u.VerticalId == userContext.VerticalId).ToList();

                            if (saudaOrdersContext != null && saudaOrdersContext.Any())
                            {
                                saudaListDto = saudaOrdersContext.Select(_ => new SaudaListDto()
                                {
                                    Id = _emamiContext.SaudaOrders.Where(s => s.SaudaNumber == _.x.SaudaNumber).FirstOrDefault() != null
                                    && _emamiContext.SaudaOrders.Where(s => s.SaudaNumber == _.x.SaudaNumber).FirstOrDefault().SaudaNumber != null ? _emamiContext.SaudaOrders.Where(s => s.SaudaNumber == _.x.SaudaNumber).FirstOrDefault().Id : 0,
                                    //SaudaOrderId = _emamiContext.SaudaOrders.FirstOrDefault(s => s.SaudaNumber == _.x.SaudaNumber).SaudaNumber != null ? _emamiContext.SaudaOrders.FirstOrDefault(s => s.SaudaNumber == _.x.SaudaNumber).Id : 0,
                                    SaudaOrderId = _emamiContext.SaudaOrders.Where(s => s.SaudaNumber == _.x.SaudaNumber).FirstOrDefault() != null
                                    && _emamiContext.SaudaOrders.Where(s => s.SaudaNumber == _.x.SaudaNumber).FirstOrDefault().SaudaNumber != null ? _emamiContext.SaudaOrders.Where(s => s.SaudaNumber == _.x.SaudaNumber).FirstOrDefault().Id : 0,
                                    UserId = _.u.Id,
                                    User = _.DealerName,
                                    City = _.CityName,
                                    // BiddingDate = _.x?.SaudaDate ?? DateTime.Now,
                                    TotalBidPrice = _.x.BasicRate,
                                    TotalBidQuantity = _.x.PendingQuantityInCase,
                                    // OiltypeName = _.x?.MaterialGroup4
                                }).ToList();

                                //var saudaList = new List<SaudaListDto>();
                                //foreach (var item in saudaOrdersContext)
                                //{
                                //    var so = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.SaudaNumber == item.x.SaudaNumber);
                                //    var sauda = new SaudaListDto();
                                //    if(so != null)
                                //    {
                                //        sauda.Id = _emamiContext.SaudaOrders.FirstOrDefault(s => s.SaudaNumber == item.x.SaudaNumber).SaudaNumber != null ? _emamiContext.SaudaOrders.FirstOrDefault(s => s.SaudaNumber == item.x.SaudaNumber).Id : 0;
                                //        sauda.SaudaOrderId = _emamiContext.SaudaOrders.FirstOrDefault(s => s.SaudaNumber == item.x.SaudaNumber).SaudaNumber != null ? _emamiContext.SaudaOrders.FirstOrDefault(s => s.SaudaNumber == item.x.SaudaNumber).Id : 0;
                                //    }
                                //    sauda.UserId = item.u.Id;
                                //    sauda.User = item.DealerName;
                                //    sauda.City = item.CityName;
                                //    sauda.BiddingDate = item.x.SaudaDate ?? DateTime.Now;
                                //    sauda.TotalBidPrice = item.x.BasicRate;
                                //    sauda.TotalBidQuantity = item.x.PendingQuantityInMT;
                                //    sauda.OiltypeName = item.x.MaterialGroup4;

                                //    saudaList.Add(sauda);
                                //}
                            }
                        }
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
        #endregion

        public ResultDto SpecialityFatDiscountUsersList(LoginNHId inputDto)
        {
            _methodName = "SpecialityFatDiscountUsersList";
            var OutputDto = new List<SpecialityFatDiscountUserDto>();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.ZHId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var UserContext = _emamiContext.Users.FirstOrDefault(f => f.Id == inputDto.ZHId && f.IsActive);
                if (UserContext == null)
                {
                    return _resultService.ErrorMessage(Constants.InActiveUser);
                }
                var UserRoleContext = _emamiContext.UserRoles.FirstOrDefault(f => f.UserId == inputDto.ZHId && f.RoleId == (int)DTO.Enums.RoleType.NationalTrader);
                if (UserRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.NationalHeadIdMissing);
                }
                OutputDto = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().Where(w => w.CreatedBy == inputDto.ZHId
                //&& w.DivisionId == UserContext.DivisionId.Value
                ).OrderByDescending(o => o.CreatedDate).Select(s => new SpecialityFatDiscountUserDto()
                {
                    Id = s.Id,
                    SkuId = s.SkuId,
                    SkuName = s.Sku.SkuName,
                    SkuCode = s.Sku.SkuCode,
                    OilTypeId = s.OilTypeId,
                    OilTypeName = s.OilType.Name,
                    QuantityLimit = s.ActualDiscount,
                    ValidFrom = s.ValidFrom,
                    ValidTo = s.ValidTo,
                    RemainingQuantity = s.RemainingQuantity,
                    EmployeeId = s.UserId,
                    EmployeeName = s.User.Name
                }).ToList();

                return _resultService.SuccessObject(OutputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto SpecialityFatDiscountUpdate(SpecialityFatDiscountUpdateInputDto inputDto)
        {
            _methodName = "SpecialityFatDiscountUpdate";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.SpecialityFatDiscountId == 0)
                {
                    return _resultService.ErrorMessage(Constants.SpecialityFatDiscountId);
                }
                if (inputDto.ActualDiscount == 0)
                {
                    return _resultService.ErrorMessage(Constants.DiscountMissing);
                }
                var SpecialityFatDiscountContext = _emamiContext.SpecalityFatDiscountUsers.FirstOrDefault(f => f.Id == inputDto.SpecialityFatDiscountId);
                if (SpecialityFatDiscountContext == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidSpecialityFatDiscountId);
                }
                else
                {
                    SpecialityFatDiscountContext.ActualDiscount = inputDto.ActualDiscount;
                    _emamiContext.SaveChanges();
                }
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
    }
}
