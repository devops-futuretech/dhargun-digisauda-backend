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
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Adani.Solution.Service
{
    public interface IZonalHeadService
    {
        ResultDto DashboardWeekwiseOverallSauda(LoginUserIdDto loginUserIdDto);
        ResultDto DashboardWeekwiseOverallSales(LoginUserIdDto loginUserIdDto);
        ResultDto DashboardOverallSauda(DashboardOverallSaudaInputDto inputDto);
        ResultDto DashboardOverallSales(DashboardOverallSaudaInputDto inputDto);
        ResultDto GetBDOList(LoginUserIdDto inputDto);
        ResultDto OverallSalesChart(DashboardOverallSaudaInputDto inputDto);
        ResultDto OverallSaleslistByDealers(DashboardOverallSaudaInputDto inputDto);
        ResultDto DashboardPackwiseSaleslist(DashboardOverallSaudaInputDto inputDto);
        ResultDto OverallPerformanceByUser(DashboardOverallSaudaInputDto inputDto);
        ResultDto PerformanceRankingList(DashboardOverallSaudaInputDto inputDto);
        ResultDto GetCreditLimitList(LoginZHId inputDto);
        ResultDto DueForTomorrowList(LoginZHId inputDto);
        ResultDto GetZHStatistics(SaudaFilterDto inputDto);
        ResultDto GetTotalCreditLimit(CreditLimitInputDto inputDto);
        ResultDto SalesTourPlanChart(SalesTourPlanInputDto inputDto);
        ResultDto ZHPlantDepotDetailsByDealer(LoginUserIdDto inputDto);
        ResultDto DailyBookedSaudaReport(DailyBookedSaudaInputDto inputDto);
        ResultDto SalesReport(DailyBookedSaudaInputDto inputDto);
        ResultDto GetBDOListForTp(LoginUserIdDto inputDto);
    }
    public class ZonalHeadService : IZonalHeadService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Zonal Head Service");
        private const string ServiceName = "Zonal Head Service";
        private string _methodName;
        private readonly IResultService _resultService;
        private readonly INotificationService _notificationService;

        public ZonalHeadService(IAdaniContext emamiContext, IResultService resultService, INotificationService notificationService)
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
                _logger.Error("Error instantiating dependencies for sauda Service", exception);
            }
        }

        #region Dashboard Chart
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
                //var bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == loginUserIdDto.LoginUserId).Select(_ => _.Id).ToList();
                //if (bdoList != null && bdoList.Any())
                //{
                //    IQueryable<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId);
                //    if (dealersList != null && dealersList.Any())
                //    {
                //        //Weekwise report
                //        var divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == loginUserIdDto.LoginUserId)
                //        .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });


                IEnumerable<DashboardSauda> saudaContextList = new List<DashboardSauda>();
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    
                    saudaContextList = conn.Query<DashboardSauda>("ZH_GetWeekwiseOverallSauda", new
                    {
                        UserId = loginUserIdDto.LoginUserId,
                        StartDate = mStartDate,
                        EndDate = mEndDate,
                        Status = UtilityHelper.ConvertIntListToCommaSeparatedString(status)
                    },commandTimeout:300,commandType:CommandType.StoredProcedure);

                }

                //var saudaContextList = (from s in _emamiContext.Sauda.AsNoTracking()
                //                        join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                //                        join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                //                        equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                //                        //join dl in dealersList on s.UserId equals dl.CustomerId
                //                        where DbFunctions.TruncateTime(so.CreatedDate) >= DbFunctions.TruncateTime(mStartDate) &&
                //                        DbFunctions.TruncateTime(so.CreatedDate) <= DbFunctions.TruncateTime(mEndDate) && status.Contains(so.StatusId) 
                //                        && dealersList.Contains(s.UserId)
                //                        //&& (bdoList.Contains(s.BdoId) || s.BdoId==0)
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

                    var sauda = saudaContextList.Where(_ => _.Date.Date >= wStartDate.Date
                                            && _.Date.Date <= wEndDate.Date)
                                            .Select(s => s.Achievment).DefaultIfEmpty(0).Sum();

                    //var saudaContextList1 = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && dealersList.Any(a => a.CustomerId == _.Sauda.UserId) && status.Contains(_.StatusId)).ToList();

                    if (sauda > 0)
                    {
                        weekwiseTargetAchieved.WeekId = weekId;
                        weekwiseTargetAchieved.Week = "Week " + weekId;
                        weekwiseTargetAchieved.Achievement = sauda;
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
                var targetContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == loginUserIdDto.LoginUserId && _.MonthId == currentDate.Month && _.Year == currentDate.Year).ToList();
                if (targetContext != null)
                {
                    //Weekwise target
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
                //var bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == loginUserIdDto.LoginUserId).Select(_ => _.Id).ToList();
                //New Reporting to table change
                //var bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == loginUserIdDto.LoginUserId).Select(_ => _.UserId).ToList();

                //var divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == loginUserIdDto.LoginUserId)
                //  .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                //if (bdoList != null && bdoList.Any())
                //{
                //    IQueryable<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(s => s.CustomerId);
                //    if (dealersList != null && dealersList.Any())
                //    {


                //Weekwise report

                IEnumerable<DashboardSauda> sales = new List<DashboardSauda>();
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    //var sqlQuery = @"CREATE TABLE #BdoTemp(BdoId BIGINT)
                    //    CREATE TABLE #DealerTemp(DealerId BIGINT)
                    //    Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                    //    insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                    //    select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

                    //     insert into #BdoTemp(BdoId) select UserId from UserReportingToMappings 
                    //     where ReportingToUserId=@UserId
                    //     insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
                    //     where UserId in (select BdoId from #BdoTemp)

                    //     select 
                    //     s.InvoiceDate as Date,
                    //     s.QuantityMT as Achievment
                    //     from SalesRegisters s with(NOLOCK)
                    //     join Skus sku on s.MaterialCode=sku.SkuCode and s.SalesOrganizationId=sku.SalesOrganizationId
                    //     and s.DistributionChannelId=sku.DistributionChannelId and s.DivisionId=sku.DivisionId
                    //     join Users u on s.CustomerCode=u.Code
                    //     join #UserDivision ud on s.SalesOrganizationId=ud.SalesOrganizationId
                    //     and s.DistributionChannelId=ud.DistributionChannelId and s.DivisionId=ud.DivisionId
                    //     and Cast(s.InvoiceDate as date) >= Cast(@StartDate as date)
                    //      and Cast(s.InvoiceDate as date) <= Cast(@EndDate as date)
                    //      and u.Id in (select DealerId from #DealerTemp)

                    //      drop table #BdoTemp
                    //      drop table #DealerTemp
                    //      drop table #UserDivision
                    //                            ";

                    sales = conn.Query<DashboardSauda>("ZH_GetWeekwiseOverallSales",
                       new
                       {
                           UserId = loginUserIdDto.LoginUserId,
                           StartDate = mStartDate,
                           EndDate = mEndDate
                       }, commandType: CommandType.StoredProcedure,commandTimeout:300).ToList();
                    //sales = conn.Query<DashboardSauda>(sqlQuery, new
                    //        {
                    //            UserId = loginUserIdDto.LoginUserId,
                    //            StartDate = mStartDate,
                    //            EndDate = mEndDate
                    //        });

                }


                //var sales = (from s in _emamiContext.SalesRegister.AsNoTracking()
                //         join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                //         join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                //         join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                //         equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                //         //join dl in dealersList on u.Id equals dl.CustomerId
                //         where (DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(mStartDate) &&
                //         DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(mEndDate))
                //         && s.SalesOrganizationId == sku.SalesOrganizationId && s.DistributionChannelId == sku.DistributionChannelId
                //         && s.DivisionId == sku.DivisionId && dealersList.Contains(u.Id)
                //         select new { InvoiceDate = s.InvoiceDate, Quantity = s.QuantityMT });

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
                                    _.Date.Date <= wEndDate.Date).ToList();

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
                //var bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();

                //New Reporting to table change
                //var bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();

                //var divisionsloginWiseuser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                //.Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                //if (bdoList != null && bdoList.Any())
                //{
                //    IQueryable<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId);


                var target = _emamiContext.UserCustomerSaudaTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId);

                var status = Constants.OverallSaudaStatus;

                IEnumerable<DashboardSauda> sauda = new List<DashboardSauda>();
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    
                    sauda = conn.Query<DashboardSauda>("ZH_GetOverallSauda", new
                    {
                        UserId = inputDto.LoginUserId,
                        StartDate = inputDto.FromDate,
                        EndDate = inputDto.ToDate,
                        Status = UtilityHelper.ConvertIntListToCommaSeparatedString(status)
                    },commandType:CommandType.StoredProcedure,commandTimeout:300);

                }

                //var sauda = (from s in _emamiContext.Sauda.AsNoTracking()
                //             join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                //             join dm in divisionsloginWiseuser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                //             equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                //             //join dl in dealersList on s.UserId equals dl.CustomerId
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
                    outputDto.TotalTarget = target.Where(_ => _.MonthId == item.Id && _.Year == item.Year).Select(_ => _.Target).DefaultIfEmpty(0).Sum();
                    //if (targetContext != null)
                    //{
                    //    outputDto.TotalTarget = targetContext.Sum(_ => _.Target);
                    //}
                    outputDto.MonthId = item.Id;
                    outputDto.Month = new DateTime(DateTime.Now.Year, item.Id, 1).ToString("MMMM");
                    //if (dealersList != null && dealersList.Any())
                    //{
                    //var achievements = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => dealersList.Any(a => a.CustomerId == _.Sauda.UserId) && DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(item.StartDate) &&
                    //DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(item.EndDate) && status.Contains(_.StatusId)).OrderByDescending(_ => _.CreatedDate)
                    //.Select(_ => new AchievmentDetailsDto()
                    //{
                    //    UserId = _.CreatedBy,
                    //    Date = _.CreatedDate,
                    //    Achievment = _.BidQuantity
                    //}).ToList();

                    outputDto.OverallSauda = sauda.Where(_ => _.Date.Date >= item.StartDate.Date &&
                                            _.Date.Date <= item.EndDate.Date).Select(s => s.Achievment)
                                            .DefaultIfEmpty(0).Sum();

                    // outputDto.OverallSauda = achievements;
                    // outputDto.AchievmentDetailsDto.AddRange(achievements);
                    dashboardOverallsaudaOutpuDto.Add(outputDto);
                    //}
                }
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

        public ResultDto DashboardOverallSales(DashboardOverallSaudaInputDto inputDto)
        {
            _methodName = "DashboardOverallSales";
            var dashboardOverallsaudaOutpuDto = new List<DashboardOverallSalesOutpuDto>();

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
                //var bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                //New Reporting to table change
                //var bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();

                //if (bdoList != null && bdoList.Any())
                //{
                //IQueryable<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(s => s.CustomerId);
                List<MonthDto> months = new List<MonthDto>();
                months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);

                // var divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                //.Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                var target = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId).ToList();


                IEnumerable<DashboardSauda> salesContext = new List<DashboardSauda>();
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    //var sqlQuery = @"CREATE TABLE #BdoTemp(BdoId BIGINT)
                    //            CREATE TABLE #DealerTemp(DealerId BIGINT)
                    //            Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                    //            insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                    //            select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

                    //             insert into #BdoTemp(BdoId) select UserId from UserReportingToMappings 
                    //             where ReportingToUserId=@UserId
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
                    //              drop table #UserDivision";
                    salesContext = conn.Query<DashboardSauda>("ZH_GetOverallSales",
                       new
                       {
                           UserId = inputDto.LoginUserId,
                           StartDate = inputDto.FromDate,
                           EndDate = inputDto.ToDate
                       }, commandType: CommandType.StoredProcedure,commandTimeout:300).ToList();
                    //salesContext = conn.Query<DashboardSauda>(sqlQuery, new
                    //{
                    //    UserId = inputDto.LoginUserId,
                    //    StartDate = inputDto.FromDate,
                    //    EndDate = inputDto.ToDate
                    //});

                }

                //var salesContext = (from s in _emamiContext.SalesRegister.AsNoTracking()
                //                    join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                //                    join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                //                    join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                //                    equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                //                    //join dl in dealersList on s.UserId equals dl.CustomerId
                //                    where (DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                //                       DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                //                       && s.SalesOrganizationId == sku.SalesOrganizationId && s.DistributionChannelId == sku.DistributionChannelId
                //                       && s.DivisionId == sku.DivisionId
                //                       && dealersList.Contains(u.Id)
                //                    select new
                //                    {
                //                        Date = s.CreatedDate,
                //                        Achievment = s.QuantityMT,
                //                    });

                foreach (var item in months)
                {
                    var outputDto = new DashboardOverallSalesOutpuDto();
                    outputDto.TotalTarget = target.Where(_ => _.MonthId == item.Id && _.Year == item.Year).Select(_ => _.Target).DefaultIfEmpty(0).Sum();
                    //if (targetContext != null)
                    //{
                    //    outputDto.TotalTarget = targetContext.Sum(_ => _.Target);
                    //}
                    outputDto.MonthId = item.Id;
                    outputDto.Month = new DateTime(DateTime.Now.Year, item.Id, 1).ToString("MMMM");

                    //if (dealersList != null && dealersList.Any())
                    //{
                    outputDto.OverallSales = salesContext.Where(_ => _.Date.Date >= item.StartDate.Date &&
                        _.Date.Date <= item.EndDate.Date)
                        .Select(s => s.Achievment).DefaultIfEmpty(0).Sum();

                    // outputDto.OverallSales = achievements.Sum(_ => _.Achievment);
                    // outputDto.AchievmentDetailsDto.AddRange(achievements);
                    dashboardOverallsaudaOutpuDto.Add(outputDto);
                    //}
                }
                //}

                var resultData = new NewDashboardOverallSalesOutpuDto();
                resultData.SalesList = dashboardOverallsaudaOutpuDto;
                resultData.TotalTarget = dashboardOverallsaudaOutpuDto.Select(_ => _.TotalTarget).DefaultIfEmpty(0).Sum();
                resultData.OverallSales = dashboardOverallsaudaOutpuDto.Select(_ => _.OverallSales).DefaultIfEmpty(0).Sum();

                resultData.Quarter1 = new QuarterOverallSalesDto()
                {
                    OverallSales = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter1.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.OverallSales).DefaultIfEmpty(0).Sum(),
                    TotalTarget = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter1.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.TotalTarget).DefaultIfEmpty(0).Sum()
                };
                resultData.Quarter2 = new QuarterOverallSalesDto()
                {
                    OverallSales = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter2.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.OverallSales).DefaultIfEmpty(0).Sum(),
                    TotalTarget = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter2.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.TotalTarget).DefaultIfEmpty(0).Sum()
                };
                resultData.Quarter3 = new QuarterOverallSalesDto()
                {
                    OverallSales = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter3.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.OverallSales).DefaultIfEmpty(0).Sum(),
                    TotalTarget = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter3.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.TotalTarget).DefaultIfEmpty(0).Sum()
                };
                resultData.Quarter4 = new QuarterOverallSalesDto()
                {
                    OverallSales = dashboardOverallsaudaOutpuDto.Where(_ => Constants.Quarter4.Split(',').Select(Int64.Parse).Contains(_.MonthId)).Select(_ => _.OverallSales).DefaultIfEmpty(0).Sum(),
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
        #endregion

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

                //var bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.UserId).Select(_ => _.Id).ToList();
                //New Reporting to table change
                var bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.UserId).Select(_ => _.UserId).ToList();
                List<long> dealersList = new List<long>();
                if (bdoList != null && bdoList.Any())
                {
                    dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                }
                outputDto.DealersCount = dealersList.Count;

                var divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.UserId)
                 .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                if (dealersList != null && dealersList.Any())
                {

                    var PendingContractContext = new List<PendingContractStatistics>();
                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {
                        var sqlQuery = @"CREATE TABLE #DealerTemp(DealerId BIGINT)
                                    Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                                    insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                                    select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

                                    insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
                                    where UserId in (select UserId from UserReportingToMappings where ReportingToUserId=@UserId)
                                    select
                                    pc.SaudaQuantity as PendingQuantityInMT,
                                    pc.ContractValidTo
                                    from PendingContracts pc with(NOLOCK)
                                    join Skus sku on pc.MaterialCode=sku.SkuCode and pc.SalesOrgId=sku.SalesOrganizationId
                                    and pc.DistChnlId=sku.DistributionChannelId and pc.DivisionId=sku.DivisionId
                                    join #UserDivision ud on pc.SalesOrgId=ud.SalesOrganizationId
                                    and pc.DistChnlId=ud.DistributionChannelId and pc.DivisionId=ud.DivisionId
                                    where pc.UserId in (select DealerId from #DealerTemp)
                                    and pc.PendingQuantityInCase!=0
                                      drop table #DealerTemp
                                      drop table #UserDivision";

                        PendingContractContext = conn.Query<PendingContractStatistics>(sqlQuery, new
                        {
                            UserId = inputDto.UserId
                        }).ToList();

                    }
                    if (PendingContractContext != null && PendingContractContext.Any())
                    {
                        outputDto.PendingSaudaQuantity = PendingContractContext.Select(_ => _.PendingQuantityInMT).DefaultIfEmpty(0).Sum();
                    }
                    if (PendingContractContext != null && PendingContractContext.Any())
                    {
                        var ExpiredContextList = PendingContractContext.Where(_ => _.ContractValidTo.Date < currentDate.Date).ToList();
                        var NearExpiredContextList = PendingContractContext.Where(_ => (_.ContractValidTo.Date - currentDate.Date).Days < 5 && (_.ContractValidTo.Date - currentDate.Date).Days >= 1).ToList();

                        if (ExpiredContextList != null && ExpiredContextList.Any())
                        {
                            outputDto.AboveOutstandingSaudaQuantity = ExpiredContextList.Sum(_ => _.PendingQuantityInMT);
                        }
                        if (NearExpiredContextList != null && NearExpiredContextList.Any())
                        {
                            outputDto.BelowOutstandingSaudaQuantity = NearExpiredContextList.Sum(_ => _.PendingQuantityInMT);
                        }
                    }


                    //var PendingContractContext = _emamiContext.PendingContracts.AsNoTracking()
                    //                        .Join(_emamiContext.SaudaOrders.AsNoTracking(), pc => pc.SaudaOrderId, so => so.Id, (pc, so) => new { pc, so })
                    //                        .Join(_emamiContext.Sauda.AsNoTracking(), so => so.so.SaudaId, s => s.Id, (so, s) => new { so.pc, so, s })
                    //                        .Where(_ => _.pc != null && dealersList.Any(a => a == _.s.UserId)).Select(_ => new { _.pc }).ToList();

                    //var PendingContractContext = _emamiContext.PendingContracts.AsNoTracking()
                    //                        .Join(_emamiContext.Users.AsNoTracking(), pc => pc.UserId, us => us.Id, (sr, us) => new { PendingContract = sr, User = us })
                    //                        .Join(_emamiContext.Skus.AsNoTracking(), sr => sr.PendingContract.MaterialCode, sku => sku.SkuCode, (sr, sku) => new { PendingContract = sr.PendingContract, User = sr.User, Sku = sku })
                    //                        .Join(_emamiContext.Sauda.AsNoTracking(), sr => sr.PendingContract.SaudaNumber, sauda => sauda.SaudaNumber, (sr, sauda) => new { PendingContract = sr.PendingContract, User = sr.User, Sku = sr.Sku , Sauda = sauda })
                    //                        .Where(_ => _.PendingContract != null && dealersList.Any(a => a == _.User.Id)
                    //                        //&& _.Sku.DivisionId== _.User.DivisionId && _.User.DivisionId == userContext.DivisionId
                    //                        && _.PendingContract.SalesOrgId == _.Sku.SalesOrganizationId && _.PendingContract.DistChnlId == _.Sku.DistributionChannelId
                    //                      && _.PendingContract.DivisionId == _.Sku.DivisionId
                    //                        ).Select(_ => new { _.PendingContract }).ToList();

                    //var PendingContractContext = (from p in _emamiContext.PendingContracts.AsNoTracking()
                    //                              join sku in _emamiContext.Skus.AsNoTracking() on p.MaterialCode equals sku.SkuCode
                    //                             // join s in _emamiContext.Sauda.AsNoTracking() on p.SaudaNumber equals s.SaudaNumber
                    //                              join dm in divisionslogieduser on new { SalesOrganizationId = p.SalesOrgId, DistributionChannelId = p.DistChnlId, DivisionId = p.DivisionId }
                    //                              equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                    //                              where dealersList.Contains(p.UserId) 
                    //                              //&& (bdoList.Contains(s.BdoId) || s.BdoId==0)
                    //                              && p.SalesOrgId == sku.SalesOrganizationId
                    //                               //&& DbFunctions.TruncateTime(p.ContractValidTo) >= DbFunctions.TruncateTime(currentDate)
                    //                              && p.DistChnlId == sku.DistributionChannelId
                    //                                             && p.DivisionId == sku.DivisionId
                    //                              select new { SaudaQuantity = p.SaudaQuantity, ContractValidTo = p.ContractValidTo });

                    //if (PendingContractContext != null && PendingContractContext.Any())
                    //{
                    //    outputDto.PendingSaudaQuantity = PendingContractContext.Select(_ => _.SaudaQuantity).DefaultIfEmpty(0).Sum();
                    //}



                    //List<Sauda> saudaContextList = _emamiContext.Sauda.AsNoTracking().Where(_ => dealersList.Contains(_.UserId) && DbFunctions.TruncateTime(_.BiddingDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //    && DbFunctions.TruncateTime(_.BiddingDate) <= DbFunctions.TruncateTime(inputDto.ToDate)).ToList();
                    //IQueryable<SaudaOrder> saudaOrderContextList = null;
                    //if (saudaContextList != null && saudaContextList.Any())
                    //{
                    //    List<long> saudaContextListIds = saudaContextList.Select(_ => _.Id).ToList();
                    //    saudaOrderContextList = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => saudaContextListIds.Contains(_.SaudaId) && (_.StatusId == (int)DTO.Enums.Status.Approved
                    //  || _.StatusId == (int)DTO.Enums.Status.Pending));
                    //}
                    //if (saudaOrderContextList != null && saudaOrderContextList.Any())
                    //{
                    ////Approved and Pending sauda orders
                    //List<long> saudaOrderContextIds = saudaOrderContextList.Select(_ => _.Id).ToList();
                    ////Lifted orders count
                    ////List<LiftingRequestDetails> liftingDetailsListContext = _emamiContext.LiftingRequestDetails.AsNoTracking().Where(_ => saudaOrderContextIds.Contains(_.SaudaOrderId)).ToList();
                    //List<SaudaOrderLiftingRequestMapping> orderLiftMappingListContext = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => saudaOrderContextIds.Contains(_.SaudaOrderId)
                    //    && _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected).ToList();
                    //if (orderLiftMappingListContext != null && orderLiftMappingListContext.Any())
                    //{
                    //    //Pending orders count
                    //    outputDto.PendingSaudaQuantity = saudaOrderContextList.Sum(_ => _.BidQuantity) - orderLiftMappingListContext.Sum(_ => _.LiftingQuantity);
                    //}
                    //else
                    //{
                    //    outputDto.PendingSaudaQuantity = saudaOrderContextList.Sum(_ => _.BidQuantity);
                    //}


                    //IQueryable<SaudaOrder> outStandingContextList = saudaOrderContextList.Where(_ => DbFunctions.TruncateTime(_.ValidToDate) < DbFunctions.TruncateTime(currentDate));
                    //List<long> outStandingContextIds = outStandingContextList.Select(_ => _.Id).ToList();

                    //var outStandingContextList = _emamiContext.PendingContracts.AsNoTracking()
                    //                    .Join(_emamiContext.Users.AsNoTracking(), pc => pc.UserId, us => us.Id, (sr, us) => new { PendingContract = sr, User = us })
                    //                    .Join(_emamiContext.Skus.AsNoTracking(), sr => sr.PendingContract.MaterialCode, sku => sku.SkuCode, (sr, sku) => new { PendingContract = sr.PendingContract, User = sr.User, Sku = sku })
                    //                    .Join(_emamiContext.Sauda.AsNoTracking(), sr => sr.PendingContract.SaudaNumber, sauda => sauda.SaudaNumber, (sr, sauda) => new { PendingContract = sr.PendingContract, User = sr.User, Sku = sr.Sku, Sauda = sauda })
                    //                    .Where(_ => _.PendingContract != null && dealersList.Any(a => a == _.User.Id)
                    //                    //&& _.User.DivisionId == userContext.DivisionId && _.Sku.DivisionId== _.User.DivisionId
                    //                    && _.PendingContract.SalesOrgId == _.Sku.SalesOrganizationId && _.PendingContract.DistChnlId == _.Sku.DistributionChannelId
                    //                      && _.PendingContract.DivisionId == _.Sku.DivisionId
                    //                    ).Select(_ => new { _.PendingContract });
                    //var outStandingContextList = (from p in _emamiContext.PendingContracts.AsNoTracking()
                    //                              join sku in _emamiContext.Skus.AsNoTracking() on p.MaterialCode equals sku.SkuCode
                    //                              // join s in _emamiContext.Sauda.AsNoTracking() on p.SaudaNumber equals s.SaudaNumber
                    //                              join dm in divisionslogieduser on new { SalesOrganizationId = p.SalesOrgId, DistributionChannelId = p.DistChnlId, DivisionId = p.DivisionId }
                    //                              equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                    //                              where dealersList.Contains(p.UserId)
                    //                              //&& (bdoList.Contains(s.BdoId) || s.BdoId==0)
                    //                              && p.SalesOrgId == sku.SalesOrganizationId
                    //                              && p.DistChnlId == sku.DistributionChannelId
                    //                                             && p.DivisionId == sku.DivisionId
                    //                              select new { SaudaQuantity = p.SaudaQuantity, ContractValidTo = p.ContractValidTo });
                    //if (outStandingContextList != null && outStandingContextList.Any())
                    //{
                    //    var ExpiredContextList = outStandingContextList.Where(_ => DbFunctions.TruncateTime(_.ContractValidTo) < DbFunctions.TruncateTime(currentDate)).ToList();
                    //    var NearExpiredContextList = outStandingContextList.Where(_ => DbFunctions.DiffDays(currentDate, _.ContractValidTo) < 5 && DbFunctions.DiffDays(currentDate, _.ContractValidTo) >= 1).ToList();

                    //Expired
                    //List<SaudaOrderLiftingRequestMapping> outStandingLiftMappingListContext = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => outStandingContextIds.Contains(_.SaudaOrderId)
                    //    && _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected).ToList();
                    //if (outStandingLiftMappingListContext != null && outStandingLiftMappingListContext.Any())
                    //{
                    //    outputDto.AboveOutstandingSaudaQuantity = outStandingContextList.Sum(_ => _.BidQuantity) - outStandingLiftMappingListContext.Sum(_ => _.LiftingQuantity);
                    //}
                    //else
                    //{
                    //    outputDto.AboveOutstandingSaudaQuantity = outStandingContextList.Sum(_ => _.BidQuantity);
                    //}
                    //    if (ExpiredContextList != null && ExpiredContextList.Any())
                    //    {
                    //        outputDto.AboveOutstandingSaudaQuantity = ExpiredContextList.Sum(_ => _.SaudaQuantity);
                    //    }
                    //    if (NearExpiredContextList != null && NearExpiredContextList.Any())
                    //    {
                    //        outputDto.BelowOutstandingSaudaQuantity = NearExpiredContextList.Sum(_ => _.SaudaQuantity);
                    //    }
                    //}
                    //outStandingContextList = saudaOrderContextList.Where(_ => DbFunctions.DiffDays(currentDate, _.ValidToDate) < 5 && DbFunctions.DiffDays(currentDate, _.ValidToDate) >= 1);
                    //outStandingContextIds = outStandingContextList.Select(_ => _.Id).ToList();
                    //if (outStandingContextList != null && outStandingContextList.Any())
                    //{
                    //    //Near Expired
                    //    List<SaudaOrderLiftingRequestMapping> outStandingLiftMappingListContext = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => outStandingContextIds.Contains(_.SaudaOrderId)
                    //        && _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected).ToList();
                    //    if (outStandingLiftMappingListContext != null && outStandingLiftMappingListContext.Any())
                    //    {
                    //        outputDto.BelowOutstandingSaudaQuantity = outStandingContextList.Sum(_ => _.BidQuantity) - outStandingLiftMappingListContext.Sum(_ => _.LiftingQuantity);
                    //    }
                    //    else
                    //    {
                    //        outputDto.BelowOutstandingSaudaQuantity = outStandingContextList.Sum(_ => _.BidQuantity);
                    //    }
                    //}
                    //}

                    //var invoicesContext = _emamiContext.Invoices.AsNoTracking().Where(_ => dealersList.Any(a => a.CustomerId == _.UserId));
                    //var invoicesContext = _emamiContext.Invoices.AsNoTracking().Where(_ => dealersList.Contains(_.UserId) && !_.PaymentStatus);
                    //if (invoicesContext != null && invoicesContext.Any())
                    //{
                    //    var dueForTomoinvoicesContext = invoicesContext.Where(_ => _.InvoiceDueDate != null && DbFunctions.TruncateTime(_.InvoiceDueDate) == DbFunctions.TruncateTime(DbFunctions.AddDays(currentDate, 1)));
                    //    if (dueForTomoinvoicesContext != null && dueForTomoinvoicesContext.Any())
                    //    {
                    //        outputDto.TotalDueForTomorrow = dueForTomoinvoicesContext.Sum(_ => _.NetValue);
                    //    }
                    //    var overDueinvoicesContext = invoicesContext.Where(_ => _.InvoiceDueDate != null && DbFunctions.TruncateTime(_.InvoiceDueDate) < DbFunctions.TruncateTime(currentDate));
                    //    if (overDueinvoicesContext != null && overDueinvoicesContext.Any())
                    //    {
                    //        outputDto.TotalOverDue = overDueinvoicesContext.Sum(_ => _.NetValue);
                    //    }

                    //}

                    var overduePaymentContext = _emamiContext.OverduePayment.AsNoTracking().Where(_ => dealersList.Contains(_.UserId));
                    if (overduePaymentContext != null && overduePaymentContext.Any())
                    {
                        var tomDate = currentDate.AddDays(1);
                        outputDto.TotalDueForTomorrow = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) == DbFunctions.TruncateTime(tomDate)).DefaultIfEmpty().Sum(_ => (decimal?)_.Balance).GetValueOrDefault();
                        outputDto.TotalOverDue = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) <= DbFunctions.TruncateTime(currentDate)).DefaultIfEmpty().Sum(_ => (decimal?)_.Balance).GetValueOrDefault();
                    }

                    outputDto.TotalSpecialRateApproval = (from s in _emamiContext.Sauda.AsNoTracking()
                                                          join sr in _emamiContext.SpecialRate.AsNoTracking() on s.SpecialRateRequestIdInParentTable equals sr.Id
                                                          join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                                                          equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                                                          where dealersList.Contains(sr.UserId) && DbFunctions.TruncateTime(sr.CreatedDate) >= DbFunctions.TruncateTime(currentDate)
                                                          && DbFunctions.TruncateTime(sr.CreatedDate) <= DbFunctions.TruncateTime(currentDate) && sr.StatusId == (int)DTO.Enums.Status.Pending
                                                          select sr).Count();

                    //if (specialRatesContext != null && specialRatesContext.Any())
                    //{
                    //    outputDto.TotalSpecialRateApproval = specialRatesContext.Count();
                    //}

                }

                //var userRoleContext = (from user in _emamiContext.Users.AsNoTracking()
                //                       join role in _emamiContext.UserRoles.AsNoTracking() on user.Id equals role.UserId
                //                       where role.RoleId == (int)DTO.Enums.Role.ZonalTrader && user.IsActive
                //                       //&& user.DivisionId == userContext.DivisionId
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

                //        //var totalAchievement = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.Invoice != null && zhDealersList.Contains(_.Invoice.UserId) && DbFunctions.TruncateTime(_.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                //        //    DbFunctions.TruncateTime(_.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)).Select(_ => _.ActualBilledQuantity).DefaultIfEmpty(0).Sum();

                //        var totalAchievement = (from i in _emamiContext.Invoices.AsNoTracking()
                //         join inv in _emamiContext.InvoiceDetails.AsNoTracking() on i.Id equals inv.InvoiceId
                //         join dm in divisionslogieduser on new { SalesOrganizationId = inv.SalesOrganizationId, DistributionChannelId = inv.DistributionChannelId, DivisionId = inv.DivisionId }
                //         equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                //         where zhDealersList.Contains(i.UserId) && DbFunctions.TruncateTime(i.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                //        DbFunctions.TruncateTime(i.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                //         select inv.ActualBilledQuantity).DefaultIfEmpty(0).Sum();

                //        salesTarget.UserId = user.Id;
                //        salesTarget.AchievmentPercentage = (totalTarget > 0 && totalAchievement > 0) ? (totalAchievement / totalTarget) * 100 : 0;
                //        rankList.Add(salesTarget);
                //    }

                //}
                //if (rankList != null && rankList.Any())
                //{
                //    int rank = 1;
                //    rankList = rankList.OrderByDescending(o => o.AchievmentPercentage).ToList();
                //    //rankList.ForEach(_ => _.Rank = rank++);
                //    //outputDto.RankTotalUserCount = rankList.Count;
                //    //outputDto.LoginUserRank = rankList.FirstOrDefault(_ => _.UserId == inputDto.UserId) != null ? rankList.FirstOrDefault(_ => _.UserId == inputDto.UserId).Rank : 0;
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

        public ResultDto GetBDOList(LoginUserIdDto inputDto)
        {
            _methodName = "GetBDOList";
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
                //outputDto = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId && _.IsActive)
                //    .Select(_ => new DropDownDto()
                //    {
                //        Id = _.Id,
                //        Name = _.Name
                //    }).ToList();

                outputDto = (from u in _emamiContext.Users.AsNoTracking()
                             join urm in _emamiContext.UserReportingToMappings.AsNoTracking() on u.Id equals urm.UserId
                             where urm.ReportingToUserId == inputDto.LoginUserId && u.IsActive
                             select new DropDownDto()
                             {
                                 Id = u.Id,
                                 Name = u.Name
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

        public ResultDto GetBDOListForTp(LoginUserIdDto inputDto)
        {
            _methodName = "GetBDOListForTp";
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

                var cities = _emamiContext.City.AsNoTracking();
                var states = _emamiContext.State.AsNoTracking();

                //New Reporting to table change
                outputDto = (from u in _emamiContext.Users.AsNoTracking()
                             join urm in _emamiContext.UserReportingToMappings.AsNoTracking() on u.Id equals urm.UserId
                             where urm.ReportingToUserId == inputDto.LoginUserId && u.IsActive
                             select new DropDownDto()
                             {
                                 Id = u.Id,
                                 Name = string.Concat(u.Name + "-" + (cities.FirstOrDefault(c => c.Id == u.CityId).CityName != null ? cities.FirstOrDefault(c => c.Id == u.CityId).CityName : string.Empty) + "-" + (states.FirstOrDefault(s => s.Id == u.StateId).StateName != null ? states.FirstOrDefault(s => s.Id == u.StateId).StateName : string.Empty) + "-" + u.Code)
                             }).ToList();

                //outputDto = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId && _.IsActive /* && _.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess*/)
                //    .Select(_ => new DropDownDto()
                //    {
                //        Id = _.Id,
                //        Name = string.Concat(_.Name + "-" + (cities.FirstOrDefault(c => c.Id == _.CityId).CityName != null ? cities.FirstOrDefault(c => c.Id == _.CityId).CityName : string.Empty) + "-" + (states.FirstOrDefault(s => s.Id == _.StateId).StateName != null ? states.FirstOrDefault(s => s.Id == _.StateId).StateName : string.Empty) + "-" + _.Code)
                //    }).ToList();

                return _resultService.SuccessObject(outputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #region Sales

        public ResultDto OverallSalesChartOld(DashboardOverallSaudaInputDto inputDto)
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
                var bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                List<long> dealersList = new List<long>();
                if (bdoList != null && bdoList.Any())
                {
                    dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                }
                if (!inputDto.IsShowDealer)
                {
                    //foreach (var item in months)
                    // {
                    var dashboardOverallsaudaOutpuDto = new List<DashboardOverallSalesOutputDto>();

                    List<long> oilTypeIds = new List<long>();
                    List<long> targetOilTypeIds = new List<long>();
                    List<long> salesOilTypeIds = new List<long>();

                    var targetListContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId && _.MonthId == inputDto.FromDate.Month && _.Year == inputDto.FromDate.Year);
                    if (targetListContext != null && targetListContext.Any())
                    {
                        targetOilTypeIds = targetListContext.Where(_ => _.OilTypeId != 0).Select(_ => _.OilTypeId).ToList();
                        oilTypeIds.AddRange(targetOilTypeIds);
                    }

                    //var salesListContext = _emamiContext.Invoices.AsNoTracking()
                    //    .Join(_emamiContext.InvoiceDetails.AsNoTracking(), i => i.Id, ind => ind.InvoiceId, (i, ind) => new { i, ind })
                    //    .Join(_emamiContext.OilTypes.AsNoTracking(), x => x.ind.OilTypeId, o => o.Id, (x, o) => new { x.ind, x.i, OilTypeName = o.Name })
                    //    .Where(_ => _.i != null 
                    //    && dealersList.Contains(_.i.UserId) 
                    //    && DbFunctions.TruncateTime(_.i.InvoiceDate) >= DbFunctions.TruncateTime(item.StartDate) &&
                    //    DbFunctions.TruncateTime(_.i.InvoiceDate) <= DbFunctions.TruncateTime(item.EndDate))
                    //    .Select(_ => new { _.ind, _.OilTypeName }).ToList();

                    var salesListContext = _emamiContext.SalesRegister.AsNoTracking()
                        .Join(_emamiContext.Skus.AsNoTracking(), sr => sr.MaterialCode, sk => sk.SkuCode, (sr, sk) => new { SalesRegister = sr, Sku = sk })
                        .Join(_emamiContext.Users.AsNoTracking(), sr => sr.SalesRegister.CustomerCode, us => us.Code, (sr, us) => new { SalesRegister = sr.SalesRegister, Sku = sr.Sku, User = us })
                        .Where(_ => dealersList.Contains(_.User.Id)
                        && DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                        DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                        //&& _.User.DivisionId == userContext.DivisionId && _.Sku.DivisionId== _.User.DivisionId
                        && _.SalesRegister.SalesOrganizationId == _.Sku.SalesOrganizationId && _.SalesRegister.DistributionChannelId == _.Sku.DistributionChannelId
                        && _.SalesRegister.DivisionId == _.Sku.DivisionId
                        )
                        .Select(s => new
                        {
                            OilTypeId = s.Sku.OilTypeId ?? 0,
                            OilType = s.Sku.OilType.Name,
                            QuantityMT = s.SalesRegister.QuantityMT
                        }).ToList();

                    if (salesListContext != null && salesListContext.Any())
                    {
                        salesOilTypeIds = salesListContext.Where(_ => _.OilTypeId != 0)
                            .Select(_ => _.OilTypeId).ToList();
                        oilTypeIds.AddRange(salesOilTypeIds);
                    }

                    if (oilTypeIds != null && oilTypeIds.Any())
                    {
                        oilTypeIds = oilTypeIds.Distinct().ToList();
                    }
                    if (oilTypeIds != null && oilTypeIds.Any())
                    {
                        foreach (var oilTypeId in oilTypeIds)
                        {
                            var acheivment = new DashboardOverallSalesOutputDto();
                            if (targetListContext != null && targetListContext.Any())
                            {
                                var oilTypeTargetListContext = targetListContext.Where(_ => _.OilTypeId == oilTypeId);
                                if (oilTypeTargetListContext != null && oilTypeTargetListContext.Any())
                                {
                                    acheivment.OilTypeId = oilTypeTargetListContext.FirstOrDefault().OilTypeId;
                                    acheivment.OilType = oilTypeTargetListContext.FirstOrDefault().OilType.Name;
                                    acheivment.TotalTarget = oilTypeTargetListContext.Select(_ => _.Target).DefaultIfEmpty(0).Sum();
                                    //acheivment.MonthId = item.Id;
                                }
                            }
                            if (salesListContext != null && salesListContext.Any())
                            {
                                var oilTypeSalesListContext = salesListContext.Where(_ => _.OilTypeId == oilTypeId);
                                if (oilTypeSalesListContext != null && oilTypeSalesListContext.Any())
                                {
                                    acheivment.OilTypeId = oilTypeSalesListContext.FirstOrDefault().OilTypeId;
                                    acheivment.OilType = oilTypeSalesListContext.FirstOrDefault().OilType;
                                    // acheivment.MonthId = item.Id;
                                    acheivment.TotalAchievment = oilTypeSalesListContext.Select(_ => _.QuantityMT).DefaultIfEmpty(0).Sum();
                                }
                            }
                            dashboardOverallsaudaOutpuDto.Add(acheivment);
                        }
                        OutputDto.AddRange(dashboardOverallsaudaOutpuDto);
                    }

                    // }

                }
                else
                {
                    var bdoListByZH = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();

                    // foreach (var item in months)
                    //{
                    var bdoOverallsaudaOutpuDto = new List<DashboardOverallSalesOutputDto>();
                    foreach (var StateTrader in bdoListByZH)
                    {
                        List<long> dealersListBybdo = new List<long>();
                        dealersListBybdo = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == StateTrader).Select(_ => _.CustomerId).ToList();
                        var dealersOverallsaudaOutpuDto = new List<DashboardOverallSalesOutputDto>();
                        foreach (var dealer in dealersListBybdo)
                        {
                            var usercontext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == dealer);
                            var targetContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == dealer && _.MonthId == inputDto.FromDate.Month && _.Year == inputDto.FromDate.Year).ToList();
                            if (targetContext != null)
                            {
                                //var salesContext = (from invoice in _emamiContext.Invoices.AsNoTracking()
                                //                    join invdetail in _emamiContext.InvoiceDetails.AsNoTracking() on invoice.Id equals invdetail.InvoiceId
                                //                    where invoice.UserId == dealer
                                //                     && DbFunctions.TruncateTime(invoice.InvoiceDate) >= DbFunctions.TruncateTime(item.StartDate) &&
                                //                            DbFunctions.TruncateTime(invoice.InvoiceDate) <= DbFunctions.TruncateTime(item.EndDate)
                                //                    select invdetail
                                //                ).ToList();
                                var salesContext = (from SaleReg in _emamiContext.SalesRegister.AsNoTracking()
                                                    join user in _emamiContext.Users.AsNoTracking() on SaleReg.CustomerCode equals user.Code
                                                    join skus in _emamiContext.Skus.AsNoTracking() on SaleReg.MaterialCode equals skus.SkuCode
                                                    where SaleReg.CustomerCode == usercontext.Code
                                                     && DbFunctions.TruncateTime(SaleReg.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                            DbFunctions.TruncateTime(SaleReg.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                                                    //&& user.DivisionId == usercontext.DivisionId && skus.DivisionId == user.DivisionId
                                                    && SaleReg.SalesOrganizationId == skus.SalesOrganizationId && SaleReg.DistributionChannelId == skus.DistributionChannelId
                                          && SaleReg.DivisionId == skus.DivisionId
                                                    select SaleReg
                                                ).ToList();

                                var acheivment = new DashboardOverallSalesOutputDto
                                {
                                    DealerId = dealer,
                                    Dealer = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == dealer).Name,
                                    TotalTarget = targetContext.Sum(_ => _.Target),
                                    TotalAchievment = salesContext.Sum(_ => (decimal?)_.QuantityMT) ?? 0,
                                    AchievmentPercentage = targetContext.Sum(_ => _.Target) > 0 ? (salesContext.Sum(_ => (decimal?)_.QuantityMT) ?? 0 / targetContext.Sum(_ => _.Target)) * 100 : 0
                                };
                                dealersOverallsaudaOutpuDto.Add(acheivment);
                            }
                        }
                        var bdoAchievement = new DashboardOverallSalesOutputDto
                        {
                            DealerId = StateTrader,
                            Dealer = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == StateTrader).Name,
                            TotalTarget = dealersOverallsaudaOutpuDto.Sum(_ => _.TotalTarget),
                            TotalAchievment = dealersOverallsaudaOutpuDto.Sum(_ => (decimal?)_.TotalAchievment) ?? 0,
                            AchievmentPercentage = dealersOverallsaudaOutpuDto.Sum(_ => _.TotalTarget) > 0 ? (dealersOverallsaudaOutpuDto.Sum(_ => (decimal?)_.TotalAchievment) ?? 0 / dealersOverallsaudaOutpuDto.Sum(_ => _.TotalTarget)) * 100 : 0
                        };
                        bdoOverallsaudaOutpuDto.Add(bdoAchievement);
                    }
                    OutputDto.AddRange(bdoOverallsaudaOutpuDto);
                    // }
                }
                return _resultService.SuccessObject(OutputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

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
                        var chartResult = conn.Query<DashboardOverallSalesOutputDto>("Get_ZHSalesTargetChartsOilTypeWise",
                            new
                            {
                                LoginUserId = inputDto.LoginUserId,
                                StartDate = inputDto.FromDate,
                                EndDate = inputDto.ToDate,
                                MonthId = inputDto.FromDate.Month,
                                Year = inputDto.FromDate.Year
                            }, commandType: CommandType.StoredProcedure).ToList();

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
                        var chartResult = conn.Query<DashboardOverallSalesOutputDto>("Get_ZHSalesTargetChartsDealerWise",
                                    new
                                    {
                                        LoginUserId = inputDto.LoginUserId,
                                        StartDate = inputDto.FromDate,
                                        EndDate = inputDto.ToDate,
                                        MonthId = inputDto.FromDate.Month,
                                        Year = inputDto.FromDate.Year
                                    }, commandType: CommandType.StoredProcedure).ToList();

                        OutputDto.AddRange(chartResult);


                    }

                    #endregion


                }

                var result = new NewDashboardOverallSalesOutputDto();
                result.SalesList = OutputDto;
                //result.TotalTarget = OutputDto.Select(_ => _.TotalTarget).DefaultIfEmpty(0).Sum();
                //result.OverallSales = OutputDto.Select(_ => _.TotalAchievment).DefaultIfEmpty(0).Sum();

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
                if (inputDto.BDOId > 0)
                {
                    bdoList.Add(inputDto.BDOId);
                }
                else
                {
                    //New Reporting to table change
                    //bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                    bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
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


                var targetSum1 = (from ust in _emamiContext.UserCustomerSalesTarget.AsNoTracking()
                                  where dealersList.Contains((long)ust.AssignedToId)
                          && MonthIds.Contains(ust.MonthId)
                          && Years.Contains(ust.Year)
                                  group ust by ust.AssignedToId into target
                                  select new { dealerId = target.Key, Target = target.Sum(x => x.Target) }
                                );



                var invoiceSum1 = (from s in _emamiContext.SalesRegister.AsNoTracking()
                                   join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                                   join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                                          equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                   where DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                                   && DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                                   && dealersList.Contains(u.Id)
                                   group s by s.UserId into sales
                                   select new { dealerId = sales.Key, InvoiceSum = sales.Sum(x => x.QuantityMT) }
                                      );

                var outputDto = (from d in dealersList
                                 join u in _emamiContext.Users.AsNoTracking() on d equals u.Id
                                 join c in _emamiContext.City.AsNoTracking() on u.CityId equals c.Id into tmpCity
                                 join t in targetSum1 on d equals t.dealerId into tmptarget
                                 join i in invoiceSum1 on d equals i.dealerId into tmpsum
                                 from t in tmptarget.DefaultIfEmpty()
                                 from i in tmpsum.DefaultIfEmpty()
                                 from c in tmpCity.DefaultIfEmpty()
                                 where (t != null && t.Target > 0)
                                 && (i != null && i.InvoiceSum > 0)
                                 select new DashboardDetailsByDealersOutputDto()
                                 {
                                     Target = t != null ? t.Target : 0,
                                     Achievement = i != null ? i.InvoiceSum : 0,
                                     DealerId = d,
                                     Dealer = u.Name,
                                     TownName = c != null ? c.CityName : String.Empty
                                 }
                                 ).ToList();

                #region OldCode
                //foreach (var dealerId in dealersList)
                //{
                //    DashboardDetailsByDealersOutputDto salesTarget = new DashboardDetailsByDealersOutputDto();
                //    var targetSum = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => dealerId == _.AssignedToId && MonthIds.Contains(_.MonthId) && Years.Contains(_.Year)).Select(s => s.Target).DefaultIfEmpty(0).Sum();

                //    //var invoiceSum = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.Invoice != null && DbFunctions.TruncateTime(_.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                //    //    && DbFunctions.TruncateTime(_.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && _.Invoice.UserId == dealerId)
                //    //    .Select(_ => _.ActualBilledQuantity).DefaultIfEmpty(0).Sum();

                //    //var invoiceSum = _emamiContext.InvoiceDetails.AsNoTracking()
                //    //    .Join(_emamiContext.SalesRegister.AsNoTracking(), invd => invd.InvoiceId, sr => sr.InvoiceId, (invd, sr) => new { InvoiceDetails = invd, SalesRegister = sr })
                //    //    .Where(_ => _.InvoiceDetails.Invoice != null
                //    //    && DbFunctions.TruncateTime(_.InvoiceDetails.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                //    //    && DbFunctions.TruncateTime(_.InvoiceDetails.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                //    //    && _.InvoiceDetails.Invoice.UserId == dealerId)
                //    //    .Select(_ => _.SalesRegister.QuantityMT).DefaultIfEmpty(0).Sum();


                //    var invoiceSum = (from s in _emamiContext.SalesRegister.AsNoTracking()
                //                   join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                //                   join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                //                   join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                //                            equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                //                   where DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                //        && DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                //        && u.Id == dealerId
                //        && s.SalesOrganizationId == sku.SalesOrganizationId 
                //        && s.DistributionChannelId == sku.DistributionChannelId
                //        && s.DivisionId == sku.DivisionId
                //        && s.SkuId > 0
                //                   select s.QuantityMT
                //                   ).DefaultIfEmpty(0).Sum();

                //    //var invoiceSum = _emamiContext.SalesRegister.AsNoTracking()
                //    //    .Join(_emamiContext.Users.AsNoTracking(), sr => sr.CustomerCode, us => us.Code, (sr, us) => new { SalesRegister = sr, User = us })
                //    //    .Join(_emamiContext.Skus.AsNoTracking(), sr => sr.SalesRegister.MaterialCode, sku => sku.SkuCode, (sr, sku) => new { SalesRegister = sr.SalesRegister, User = sr.User, Sku = sku })
                //    //    .Where(_ => DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                //    //    && DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                //    //    && _.User.Id == dealerId
                //    //    //&& _.User.DivisionId == userContext.DivisionId && _.Sku.DivisionId== _.User.DivisionId
                //    //    && _.SalesRegister.SalesOrganizationId == _.Sku.SalesOrganizationId && _.SalesRegister.DistributionChannelId == _.Sku.DistributionChannelId
                //    //    && _.SalesRegister.DivisionId == _.Sku.DivisionId
                //    //    )
                //    //    .Select(_ => _.SalesRegister.QuantityMT).DefaultIfEmpty(0).Sum();

                //    var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == dealerId);
                //    if (dealerContext != null && invoiceSum > 0 && targetSum > 0)
                //    {
                //        salesTarget.DealerId = dealerId;
                //        salesTarget.Dealer = dealerContext.Name;
                //        salesTarget.TownName = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == dealerContext.CityId) != null ? _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == dealerContext.CityId).CityName : String.Empty; ;
                //        salesTarget.Target = targetSum;
                //        salesTarget.Achievement = invoiceSum;
                //    }
                //    if (salesTarget.Dealer != null)
                //    {
                //        dashboardDetailsByDealersOutputDto.Add(salesTarget);
                //    }
                //}
                #endregion
                return _resultService.SuccessObject(outputDto);
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
                if (inputDto.BDOId > 0)
                {
                    bdoList.Add(inputDto.BDOId);
                }
                else
                {
                    //New Reporting to table change
                    bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                    //bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
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
                var salesDetail = new List<DashboardDetailsByDealersOutputDto>();
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


                    //var invoiceDetailsContextList = (from s in _emamiContext.SalesRegister.AsNoTracking()
                    //                                 join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                    //                                 join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                    //                                 join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                    //                                           equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                    //                                 where DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //                && DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    //                && dealersList.Contains(u.Id)
                    //                && s.SalesOrganizationId == sku.SalesOrganizationId && s.DistributionChannelId == sku.DistributionChannelId
                    //                && s.DivisionId == sku.DivisionId
                    //                && s.SkuId > 0
                    //                                 select new
                    //                                 {
                    //                                     PackGroupId = sku.PackGroupId,
                    //                                     QuantityMT = s.QuantityMT,
                    //                                     UserId = u.Id,
                    //                                     SkuId = sku.Id
                    //                                 }
                    //              );



                    //if (invoiceDetailsContextList != null && invoiceDetailsContextList.Any())
                    //{
                    //    dealerIds = invoiceDetailsContextList.Select(_ => _.UserId).Distinct().ToList();

                    //    var dealerContextDatas = _emamiContext.Users.AsNoTracking().Where(_ => dealerIds.Contains(_.Id))
                    //    .Select(s => new { Id = s.Id, Name = s.Name, CityId = s.CityId }).ToList();

                    //    foreach (var dealerId in dealerIds)
                    //    {
                    //        var salesDetail = new DashboardDetailsByDealersOutputDto();
                    //        salesDetail.DealerId = dealerId;
                    //        var dealerContext = (dealerContextDatas != null && dealerContextDatas.Any())
                    //            ? dealerContextDatas.FirstOrDefault(_ => _.Id == dealerId) : null;
                    //        if (dealerContext != null)
                    //        {
                    //            salesDetail.Dealer = dealerContext.Name;
                    //            var cityContext = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == dealerContext.CityId);
                    //            salesDetail.TownName = cityContext != null ? cityContext.CityName : string.Empty;
                    //        }

                    //        salesDetail.Target = _emamiContext.UserCustomerSalesTarget.AsNoTracking()
                    //            .Where(_ => _.AssignedToId == dealerId
                    //            && MonthIds.Contains(_.MonthId)
                    //            && Years.Contains(_.Year))
                    //            .Select(_ => _.Target).DefaultIfEmpty(0).Sum();

                    //        if (inputDto.PackGroupId > 0)
                    //        {
                    //            salesDetail.Achievement = invoiceDetailsContextList.AsNoTracking()
                    //                .Where(_ => _.PackGroupId == inputDto.PackGroupId
                    //                && _.UserId == dealerId).Select(_ => _.QuantityMT).DefaultIfEmpty(0).Sum();
                    //        }
                    //        else
                    //        {
                    //            salesDetail.Achievement = invoiceDetailsContextList.AsNoTracking()
                    //                .Where(_ => _.UserId == dealerId).Select(_ => _.QuantityMT).DefaultIfEmpty(0).Sum();
                    //        }
                    //        dashboardOutputDto.Add(salesDetail);
                    //    }
                    //}

                    #endregion



                    #region 27-12-2019 Code Commented
                    //var invoiceDetailsContextList = _emamiContext.InvoiceDetails.AsNoTracking()
                    //    .Join(_emamiContext.SalesRegister.AsNoTracking(), invd => invd.InvoiceId, sr => sr.InvoiceId, (invd, sr) => new { InvoiceDetails = invd, SalesRegister = sr })
                    //    .Where(_ => _.InvoiceDetails.Invoice != null
                    //    && DbFunctions.TruncateTime(_.InvoiceDetails.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
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
                    #endregion
                    //DashboardSalesDto
                    IEnumerable<DashboardSalesDto> invoiceSum1 = new List<DashboardSalesDto>();
                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {

                        var sqlQuery = @"Create Table #DealerIdsTemp(DealerId bigint)
Create Table #BdoId(BdoId bigint)
Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)

insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

if(@BdoId >0)
begin
	insert into #BdoId(BdoId) select @BdoId
end
else
begin
	insert into #BdoId(BdoId) select UserId from UserReportingToMappings where ReportingToUserId=@UserId
end

insert into #DealerIdsTemp(DealerId) select CustomerId from UserCustomerMappings where UserId in (select BdoId from #BdoId)

select
u.Id as UserId,
Sum(s.QuantityMT) as QuantityMT
from SalesRegisters s with(NOLOCK)
join Skus sku with(NOLOCK) on s.MaterialCode=sku.SkuCode and s.SalesOrganizationId=sku.SalesOrganizationId
and s.DistributionChannelId=sku.DistributionChannelId and s.DivisionId=sku.DivisionId
join Users u with(NOLOCK) on s.CustomerCode=u.Code
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
drop table #UserDivision";
                        invoiceSum1 = conn.Query<DashboardSalesDto>(sqlQuery, new
                        {
                            UserId = inputDto.LoginUserId,
                            FromDate = inputDto.FromDate,
                            ToDate = inputDto.ToDate,
                            BdoId = inputDto.BDOId,
                            inputDto.PackGroupId
                        });

                    }
                    //var invoiceSum1 = (from s in _emamiContext.SalesRegister.AsNoTracking()
                    //                   join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                    //                   join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                    //                   //join d in dealersList on u.Id equals d
                    //                   join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                    //                          equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                    //                   where (
                    //                   DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //                   && DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    //                   && dealersList.Contains(u.Id)
                    //                   && s.SalesOrganizationId == sku.SalesOrganizationId && s.DistributionChannelId == sku.DistributionChannelId
                    //                   && s.DivisionId == sku.DivisionId
                    //                   //&& s.SkuId > 0
                    //                   && (inputDto.PackGroupId > 0 ? sku.PackGroupId == inputDto.PackGroupId : inputDto.PackGroupId == 0)
                    //                   )
                    //                   group s by s.UserId into sales
                    //                   select new { dealerId = sales.Key, InvoiceSum =sales.Sum(x => x.QuantityMT) }
                    //                 );





                    List<long> dealerIds = new List<long>();


                    if (invoiceSum1 != null && invoiceSum1.Any())
                    {


                        dealerIds = invoiceSum1.Select(_ => _.UserId).Distinct().ToList();

                        var targetSum1 = (from ust in _emamiContext.UserCustomerSalesTarget.AsNoTracking()
                                          where dealerIds.Contains((long)ust.AssignedToId)
                                  && MonthIds.Contains(ust.MonthId)
                                  && Years.Contains(ust.Year)
                                          group ust by ust.AssignedToId into target
                                          select new { dealerId = target.Key, Target = target.Sum(x => x.Target) }
                                 );

                        var dealerContextDatas = _emamiContext.Users.AsNoTracking().Where(_ => dealerIds.Contains(_.Id))
                        .Select(s => new { Id = s.Id, Name = s.Name, CityId = s.CityId }).ToList();


                        salesDetail = (from d in dealerContextDatas
                                       join c in _emamiContext.City.AsNoTracking() on d.CityId equals c.Id into tmpCity
                                       join t in targetSum1 on d.Id equals t.dealerId into tmptarget
                                       join i in invoiceSum1 on d.Id equals i.UserId into tmpsum
                                       from t in tmptarget.DefaultIfEmpty()
                                       from i in tmpsum.DefaultIfEmpty()
                                       from c in tmpCity.DefaultIfEmpty()
                                       select new DashboardDetailsByDealersOutputDto()
                                       {
                                           Target = t != null ? t.Target : 0,
                                           Achievement = i != null ? i.QuantityMT : 0,
                                           DealerId = d.Id,
                                           Dealer = d.Name,
                                           TownName = c != null ? c.CityName : String.Empty
                                       }
                                 ).ToList();


                    }


                }

                return _resultService.SuccessObject(salesDetail);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto OverallPerformanceByUser(DashboardOverallSaudaInputDto inputDto)
        {
            _methodName = "OverallPerformanceByUser";
            var outputDto = new OverallPerformanceByUserOutputDto();
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
                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }
                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
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
                    divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                 .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });
                }

                //target
                List<MonthDto> months = new List<MonthDto>();
                months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);
                List<int> MonthIds = months.Select(s => s.Id).Distinct().ToList();
                List<long> Years = months.Select(s => (long)s.Year).Distinct().ToList();

                var totalTarget = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId
                    && MonthIds.Contains(_.MonthId) && Years.Contains(_.Year)).Select(_ => _.Target).DefaultIfEmpty(0).Sum();
                //achievement
                //New Reporting to table change
                //var bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                var bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                List<long> dealersList = new List<long>();
                if (bdoList != null && bdoList.Any())
                {
                    dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                }

                var totalAchievement = (from i in _emamiContext.InvoiceDetails.AsNoTracking()
                                        join ud in divisionslogieduser on new { SalesOrganizationId = i.SalesOrganizationId, DistributionChannelId = i.DistributionChannelId, DivisionId = i.DivisionId }
                                             equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                        where i != null
                     && dealersList.Contains(i.Invoice.UserId)
                     && DbFunctions.TruncateTime(i.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                     && DbFunctions.TruncateTime(i.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                                        select i.ActualBilledQuantity
                                       ).DefaultIfEmpty(0).Sum();

                //var totalAchievement = _emamiContext.InvoiceDetails.AsNoTracking().
                //    Where(_ => _.Invoice != null 
                //    && dealersList.Contains(_.Invoice.UserId) 
                //    && DbFunctions.TruncateTime(_.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) 
                //    && DbFunctions.TruncateTime(_.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                //    .Select(_ => _.ActualBilledQuantity).DefaultIfEmpty(0).Sum();

                outputDto.UserTarget = totalTarget;
                outputDto.UserAchievment = totalAchievement;
                outputDto.UserId = userContext.Id;
                outputDto.Usercode = userContext.Code;
                outputDto.Username = userContext.Name;
                outputDto.AchievmentPercentage = (totalTarget > 0 && totalAchievement > 0) ? (totalAchievement / totalTarget) * 100 : 0;

                return _resultService.SuccessObject(outputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto PerformanceRankingList(DashboardOverallSaudaInputDto inputDto)
        {
            _methodName = "PerformanceRankingList";
            var outpuDto = new List<OverallPerformanceByUserOutputDto>();
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
                if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.FromDateEmpty);
                }
                if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
                {
                    return _resultService.ErrorMessage(Constants.ToDateEmpty);
                }

                var userRoleContext = (from user in _emamiContext.Users.AsNoTracking()
                                       join role in _emamiContext.UserRoles.AsNoTracking() on user.Id equals role.UserId
                                       where role.RoleId == (int)DTO.Enums.Role.ZonalTrader && user.IsActive
                                       //&& user.DivisionId == userContext.DivisionId
                                       select new UserMasterDto
                                       {
                                           Id = user.Id,
                                           EmployeeName = user.Name,
                                           EmployeeCode = user.Code
                                       }).ToList();

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

                List<MonthDto> months = new List<MonthDto>();
                months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);
                List<int> MonthIds = months.Select(s => s.Id).Distinct().ToList();
                List<long> Years = months.Select(s => (long)s.Year).Distinct().ToList();

                if (userRoleContext != null)
                {
                    foreach (var user in userRoleContext)
                    {
                        var salesTarget = new OverallPerformanceByUserOutputDto();
                        //target
                        var totalTarget = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == user.Id
                            && MonthIds.Contains(_.MonthId) && Years.Contains(_.Year)).Select(_ => _.Target).DefaultIfEmpty(0).Sum();

                        //achievement
                        //New Reporting to table change
                        //var bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == user.Id).Select(_ => _.Id).ToList();
                        var bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                        List<long> dealersList = new List<long>();
                        if (bdoList != null && bdoList.Any())
                        {
                            dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                        }

                        var totalAchievement = (from i in _emamiContext.InvoiceDetails.AsNoTracking()
                                                join ud in divisionslogieduser on new { SalesOrganizationId = i.SalesOrganizationId, DistributionChannelId = i.DistributionChannelId, DivisionId = i.DivisionId }
                                              equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                                where i.Invoice != null
                                && dealersList.Contains(i.Invoice.UserId)
                                && DbFunctions.TruncateTime(i.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                DbFunctions.TruncateTime(i.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                                                select i.ActualBilledQuantity
                                              ).DefaultIfEmpty(0).Sum();

                        //var totalAchievement = _emamiContext.InvoiceDetails.AsNoTracking()
                        //    .Where(_ => _.Invoice != null 
                        //    && dealersList.Contains(_.Invoice.UserId) 
                        //    && DbFunctions.TruncateTime(_.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                        //    DbFunctions.TruncateTime(_.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                        //    .Select(_ => _.ActualBilledQuantity).DefaultIfEmpty(0).Sum();

                        salesTarget.UserTarget = totalTarget;
                        salesTarget.UserAchievment = totalAchievement;
                        salesTarget.UserId = user.Id;
                        salesTarget.Usercode = user.EmployeeCode;
                        salesTarget.Username = user.EmployeeName;
                        salesTarget.AchievmentPercentage = (totalTarget > 0 && totalAchievement > 0) ? (totalAchievement / totalTarget) * 100 : 0;
                        outpuDto.Add(salesTarget);
                    }

                }
                if (outpuDto != null && outpuDto.Any())
                {
                    int rank = 1;
                    outpuDto = outpuDto.OrderByDescending(o => o.AchievmentPercentage).ToList();
                    outpuDto.ForEach(_ => _.Rank = rank++);
                }

                return _resultService.SuccessObject(outpuDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetCreditLimitList(LoginZHId inputDto)
        {
            var creditLimitListDto = new List<CreditLimitDto>();
            _methodName = "GetCreditLimitList";
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
                var roleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                if (roleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                List<long> bdoList = new List<long>();
                if (inputDto.BDOId > 0)
                {
                    bdoList.Add(inputDto.BDOId);
                }
                else
                {
                    bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                }
                List<long> dealersList = new List<long>();
                if (bdoList != null && bdoList.Any())
                {
                    dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
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
                    divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                 .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });
                }
                if (dealersList != null && dealersList.Any())
                {
                    var userCreditListContext = (from ucm in _emamiContext.UserCreditMaster.AsNoTracking()
                                                 join ud in divisionslogieduser on new { SalesOrganizationId = ucm.SalesOrgId, DistributionChannelId = ucm.DistChnlId, DivisionId = ucm.DivisionId }
                                              equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                                 where dealersList.Contains(ucm.UserId)
                                                 select ucm
                                               ).ToList();
                    //var userCreditListContext = _emamiContext.UserCreditMaster.AsNoTracking().Where(_ => dealersList.Contains(_.UserId) && _.Isactive).ToList();
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

        public ResultDto DueForTomorrowList(LoginZHId inputDto)
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
                if (inputDto.BDOIds == null && inputDto.DealerIds == null)
                {
                    bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                    dealerlist = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                }
                else if (inputDto.BDOIds.IsAny() && inputDto.DealerIds == null)
                {
                    bdoList.AddRange(inputDto.BDOIds);
                    dealerlist = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                }
                else if (inputDto.DealerIds.IsAny())
                {
                    dealerlist.AddRange(inputDto.DealerIds);
                }

                var overduePaymentContext = _emamiContext.OverduePayment.AsNoTracking().Where(_ => dealerlist.Contains(_.UserId));
                var userIdList = overduePaymentContext.Select(s => s.UserId).Distinct().ToList();
                var UserContextData = _emamiContext.Users.AsNoTracking()
                   .Where(_ => userIdList.Contains(_.Id))
                   .Select(s => new { Id = s.Id, Name = s.Name });
                if (overduePaymentContext != null && overduePaymentContext.Any())
                {
                    var tomDate = currentDate.AddDays(1);

                    if (inputDto.DueStatus == (int)DTO.Enums.DueStatus.OverDue)
                    {
                        //dashboardDetailsForPendingAndOverDueOutputDto.TotalBookedValueOverDue = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) < DbFunctions.TruncateTime(currentDate)).DefaultIfEmpty().Sum(_ => _.Balance);
                        //var userCreditMasterContext = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) < DbFunctions.TruncateTime(currentDate)).Select(s => new OverAndPendingDueWithDealerDetails()
                        //{
                        //    DealerCode = s.UserCode,
                        //    DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == s.UserId) != null ? _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == s.UserId).Name : string.Empty,
                        //    OverDue = s.Balance
                        //}).ToList();
                        var totalBookedValueOverDue = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) <= DbFunctions.TruncateTime(currentDate)).DefaultIfEmpty().Sum(_ => (decimal?)_.Balance).GetValueOrDefault();
                        if (totalBookedValueOverDue != null)
                        {
                            dashboardDetailsForPendingAndOverDueOutputDto.TotalBookedValueOverDue = totalBookedValueOverDue;
                        }
                        var userCreditMasterContext = new List<OverAndPendingDueWithDealerDetails>();
                        var userCreditMasterForOverDueList = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) <= DbFunctions.TruncateTime(currentDate)).ToList();
                        if (userCreditMasterForOverDueList != null)
                        {

                            foreach (var item in userCreditMasterForOverDueList)
                            {
                                var user = UserContextData.FirstOrDefault(_ => _.Id == item.UserId);
                                var overAndPendingDueWithDealerDetails = new OverAndPendingDueWithDealerDetails
                                {
                                    DealerCode = item.UserCode != null ? item.UserCode : string.Empty,
                                    DealerName = userContext != null ? userContext.Name : string.Empty,
                                    OverDue = item.Balance,
                                    ReferenceNo = item.Reference != null ? item.Reference : string.Empty,
                                    DueDate = item.DueDate
                                };

                                userCreditMasterContext.Add(overAndPendingDueWithDealerDetails);
                            }
                        }
                        dashboardDetailsForPendingAndOverDueOutputDto.OverAndPendingDueWithDealerDetails.AddRange(userCreditMasterContext);
                    }
                    else if (inputDto.DueStatus == (int)DTO.Enums.DueStatus.PendingDue)
                    {
                        //dashboardDetailsForPendingAndOverDueOutputDto.TotalBookedValuePendingDue = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) == DbFunctions.TruncateTime(tomDate)).DefaultIfEmpty().Sum(_ => _.Balance);
                        //var userCreditMasterContext = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) == DbFunctions.TruncateTime(tomDate)).Select(s => new OverAndPendingDueWithDealerDetails()
                        //{
                        //    DealerCode = s.UserCode,
                        //    DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == s.UserId) != null ? _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == s.UserId).Name : string.Empty,
                        //    PendingDue = s.Balance
                        //}).ToList();
                        var totalBookedValuePendingDue = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) == DbFunctions.TruncateTime(tomDate)).DefaultIfEmpty().Sum(_ => (decimal?)_.Balance).GetValueOrDefault();
                        if (totalBookedValuePendingDue != null)
                        {
                            dashboardDetailsForPendingAndOverDueOutputDto.TotalBookedValuePendingDue = totalBookedValuePendingDue;
                        }
                        var userCreditMasterContext = new List<OverAndPendingDueWithDealerDetails>();
                        var userCreditMasterForTommorrowDueList = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) == DbFunctions.TruncateTime(tomDate)).ToList();
                        if (userCreditMasterForTommorrowDueList != null)
                        {
                            foreach (var item in userCreditMasterForTommorrowDueList)
                            {
                                var user = UserContextData.FirstOrDefault(_ => _.Id == item.UserId);
                                var overAndPendingDueWithDealerDetails = new OverAndPendingDueWithDealerDetails
                                {
                                    DealerCode = item.UserCode != null ? item.UserCode : string.Empty,
                                    DealerName = userContext != null ? userContext.Name : string.Empty,
                                    PendingDue = item.Balance,
                                    ReferenceNo = item.Reference != null ? item.Reference : string.Empty,
                                    DueDate = item.DueDate
                                };

                                userCreditMasterContext.Add(overAndPendingDueWithDealerDetails);
                            }
                        }
                        dashboardDetailsForPendingAndOverDueOutputDto.OverAndPendingDueWithDealerDetails.AddRange(userCreditMasterContext);
                    }
                    else
                    {
                        var totalBookedValueOverDue = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) <= DbFunctions.TruncateTime(currentDate)).DefaultIfEmpty().Sum(_ => (decimal?)_.Balance).GetValueOrDefault();
                        if (totalBookedValueOverDue != null)
                        {
                            dashboardDetailsForPendingAndOverDueOutputDto.TotalBookedValueOverDue = totalBookedValueOverDue;
                        }
                        var totalBookedValuePendingDue = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) == DbFunctions.TruncateTime(tomDate)).DefaultIfEmpty().Sum(_ => (decimal?)_.Balance).GetValueOrDefault();
                        if (totalBookedValuePendingDue != null)
                        {
                            dashboardDetailsForPendingAndOverDueOutputDto.TotalBookedValuePendingDue = totalBookedValuePendingDue;
                        }

                        var userCreditMasterForOverDueContext = new List<OverAndPendingDueWithDealerDetails>();
                        var userCreditMasterForOverDueList = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) <= DbFunctions.TruncateTime(currentDate)).ToList();
                        if (userCreditMasterForOverDueList != null)
                        {

                            foreach (var item in userCreditMasterForOverDueList)
                            {
                                var user = UserContextData.FirstOrDefault(_ => _.Id == item.UserId);
                                var overAndPendingDueWithDealerDetails = new OverAndPendingDueWithDealerDetails
                                {
                                    DealerCode = item.UserCode != null ? item.UserCode : string.Empty,
                                    DealerName = userContext != null ? userContext.Name : string.Empty,
                                    OverDue = item.Balance,
                                    ReferenceNo = item.Reference != null ? item.Reference : string.Empty,
                                    DueDate = item.DueDate
                                };

                                userCreditMasterForOverDueContext.Add(overAndPendingDueWithDealerDetails);
                            }
                        }
                        dashboardDetailsForPendingAndOverDueOutputDto.OverAndPendingDueWithDealerDetails.AddRange(userCreditMasterForOverDueContext);
                        //var userCreditMasterForOverDueContext = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) < DbFunctions.TruncateTime(currentDate)).Select(s => new OverAndPendingDueWithDealerDetails()
                        //{
                        //    DealerCode = s.UserCode,
                        //    DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == s.UserId) != null ? _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == s.UserId).Name : string.Empty,
                        //    OverDue = s.Balance
                        //}).ToList();
                        var userCreditMasterForTommorrowDueContext = new List<OverAndPendingDueWithDealerDetails>();
                        var userCreditMasterForTommorrowDueList = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) == DbFunctions.TruncateTime(tomDate)).ToList();
                        if (userCreditMasterForTommorrowDueList != null)
                        {
                            foreach (var item in userCreditMasterForTommorrowDueList)
                            {
                                var user = UserContextData.FirstOrDefault(_ => _.Id == item.UserId);
                                var overAndPendingDueWithDealerDetails = new OverAndPendingDueWithDealerDetails
                                {
                                    DealerCode = item.UserCode != null ? item.UserCode : string.Empty,
                                    DealerName = userContext != null ? userContext.Name : string.Empty,
                                    PendingDue = item.Balance,
                                    ReferenceNo = item.Reference != null ? item.Reference : string.Empty,
                                    DueDate = item.DueDate
                                };

                                userCreditMasterForOverDueContext.Add(overAndPendingDueWithDealerDetails);
                            }
                        }
                        dashboardDetailsForPendingAndOverDueOutputDto.OverAndPendingDueWithDealerDetails.AddRange(userCreditMasterForTommorrowDueContext);
                        //var userCreditMasterForTommorrowDueContext = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) == DbFunctions.TruncateTime(tomDate)).Select(s => new OverAndPendingDueWithDealerDetails()
                        //{
                        //    DealerCode = s.UserCode,
                        //    DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == s.UserId) != null ? _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == s.UserId).Name : string.Empty,
                        //    PendingDue = s.Balance
                        //}).ToList();

                    }
                }
                else
                {
                    var resultDto = new ResultDto();
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                    return resultDto;
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

                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
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
                //New Reporting to table change
                //var bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                var bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                List<long> dealersList = new List<long>();
                if (bdoList != null && bdoList.Any())
                {
                    dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                }
                if (dealersList != null && dealersList.Any())
                {
                    var userCreditListContext = (from ucm in _emamiContext.UserCreditMaster.AsNoTracking()
                                                     //join ud in divisionslogieduser on new { SalesOrganizationId = ucm.SalesOrgId, DistributionChannelId = ucm.DistChnlId, DivisionId = ucm.DivisionId }
                                                     //       equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                                 where dealersList.Contains(ucm.UserId)
                                                 //select ucm
                                                 group ucm by ucm.UserId into ucredit
                                                 select new { Id = ucredit.Key, value = ucredit.OrderByDescending(_ => _.CreatedDate).FirstOrDefault() }
                                     );
                    //var userCreditListContext = _emamiContext.UserCreditMaster.AsNoTracking().Where(_ => dealersList.Contains(_.UserId)  /*&& _.CreditAccountNumber != null*/).ToList();
                    if (userCreditListContext != null && userCreditListContext.Any())
                    {
                        creditLimitTotalDto.DealersCount = userCreditListContext.Count();
                        creditLimitTotalDto.TotalCreditLimit = Math.Round((userCreditListContext.DefaultIfEmpty().Sum(_ => _.value.CreditLimit) / 100000), 2);
                        creditLimitTotalDto.TotalCreditExposure = Math.Round((userCreditListContext.DefaultIfEmpty().Sum(_ => _.value.CreditExposure) / 100000), 2);
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

                    //27-12-2019
                    //var invoiceDetailsContextList = _emamiContext.InvoiceDetails.AsNoTracking()
                    //    .Join(_emamiContext.SalesRegister.AsNoTracking(), invd => invd.InvoiceId, sr => sr.InvoiceId, (invd, sr) => new { InvoiceDetails = invd, SalesRegister = sr })
                    //    .Where(_ => _.InvoiceDetails.Invoice != null
                    //    && DbFunctions.TruncateTime(_.InvoiceDetails.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //    && DbFunctions.TruncateTime(_.InvoiceDetails.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    //    && dealersList.Contains(_.InvoiceDetails.Invoice.UserId));
                    //if (invoiceDetailsContextList != null && invoiceDetailsContextList.Any())
                    //{
                    //    var bulkInvoiceDetailsContextList = invoiceDetailsContextList
                    //        .Join(_emamiContext.Skus.AsNoTracking(), i => i.InvoiceDetails.SkuId, s => s.Id, (i, s) => new { i, s })
                    //     .Where(_ => _.s.PackGroupId == (int)DTO.Enums.PackGroupType.BulkPacking);
                    //    if (bulkInvoiceDetailsContextList != null && bulkInvoiceDetailsContextList.Any())
                    //    {
                    //        creditLimitTotalDto.TotalBulkPack = bulkInvoiceDetailsContextList.Select(s => s.i.SalesRegister.QuantityMT).DefaultIfEmpty(0).Sum();
                    //    }
                    //    var customInvoiceDetailsContextList = invoiceDetailsContextList
                    //        .Join(_emamiContext.Skus.AsNoTracking(), i => i.InvoiceDetails.SkuId, s => s.Id, (i, s) => new { i, s })
                    //     .Where(_ => _.s.PackGroupId == (int)DTO.Enums.PackGroupType.ConsumerPacking);
                    //    if (customInvoiceDetailsContextList != null && customInvoiceDetailsContextList.Any())
                    //    {
                    //        creditLimitTotalDto.TotalCustomPack = customInvoiceDetailsContextList.Select(s => s.i.SalesRegister.QuantityMT).DefaultIfEmpty(0).Sum();
                    //    }
                    //}

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
                                                     select
                                                     new { PackGroupId = sku.PackGroupId, QuantityMT = s.QuantityMT }
                                   );

                    //var invoiceDetailsContextList = _emamiContext.SalesRegister.AsNoTracking()
                    //    .Join(_emamiContext.Skus.AsNoTracking(), sr => sr.MaterialCode, sk => sk.SkuCode, (sr, sk) => new { SalesRegister = sr, Sku = sk })
                    //    .Join(_emamiContext.Users.AsNoTracking(), sr => sr.SalesRegister.CustomerCode, us => us.Code, (sr, us) => new { SalesRegister = sr.SalesRegister, Sku = sr.Sku, User = us })
                    //    .Where(w => DbFunctions.TruncateTime(w.SalesRegister.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //    && DbFunctions.TruncateTime(w.SalesRegister.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    //    && dealersList.Contains(w.User.Id)
                    //    && w.SalesRegister.SalesOrganizationId == w.Sku.SalesOrganizationId && w.SalesRegister.DistributionChannelId == w.Sku.DistributionChannelId
                    //       && w.SalesRegister.DivisionId == w.Sku.DivisionId
                    //    //&& w.User.DivisionId == userContext.DivisionId && w.Sku.DivisionId == w.User.DivisionId
                    //    )
                    //    .Select(s => new
                    //    {
                    //        PackGroupId = s.Sku.PackGroupId,
                    //        QuantityMT = s.SalesRegister.QuantityMT
                    //    });

                    if (invoiceDetailsContextList != null && invoiceDetailsContextList.Any())
                    {
                        if (inputDto.PackGroupId > 0)
                        {
                            var bulkInvoiceDetailsContextList = invoiceDetailsContextList
                             .Where(_ => _.PackGroupId == inputDto.PackGroupId);
                            if (bulkInvoiceDetailsContextList != null && bulkInvoiceDetailsContextList.Any())
                            {
                                creditLimitTotalDto.TotalPack = bulkInvoiceDetailsContextList.Select(s => s.QuantityMT).DefaultIfEmpty(0).Sum();
                            }
                        }
                        else
                        {
                            var customInvoiceDetailsContextList = invoiceDetailsContextList;
                            if (customInvoiceDetailsContextList != null && customInvoiceDetailsContextList.Any())
                            {
                                creditLimitTotalDto.TotalPack = customInvoiceDetailsContextList.Select(s => s.QuantityMT).DefaultIfEmpty(0).Sum();
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
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion

        #region STP
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
                bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();

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

        #endregion

        #region Plant, Depot list

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

                //var bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => new { Id = _.Id, /*VerticalId = _.DivisionId,*/ SaudaBookingTypeId = _.SaudaBookingTypeId }).ToList();
                //New Reporting to table change
                var bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => new { Id = _.UserId }).ToList();
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
                        //    var depotContext = (from plantdepot in _emamiContext.PlantDepotMapping.AsNoTracking()
                        //                        join depot in _emamiContext.Depots.AsNoTracking() on plantdepot.DepotId equals depot.Id
                        //                        where plantdepot.PlantId == plant.Id && !depot.IsPlant && depot.StorageTypeId == (int)DTO.Enums.StorageType.Depot
                        //                        select new DepotDto
                        //                        {
                        //                            Id = depot.Id,
                        //                            Name = depot.Name,
                        //                            Code = depot.Code,
                        //                            IsPlant = depot.IsPlant,
                        //                            IsActive = depot.IsActive
                        //                        }).ToList();

                        //    plant.Depotlist = depotContext;


                        //    var rakeContext = (from plantdepot in _emamiContext.PlantDepotMapping.AsNoTracking()
                        //                       join depot in _emamiContext.Depots.AsNoTracking() on plantdepot.DepotId equals depot.Id
                        //                       where plantdepot.PlantId == plant.Id && !depot.IsPlant && depot.StorageTypeId == (int)DTO.Enums.StorageType.Rake
                        //                       select new DepotDto
                        //                       {
                        //                           Id = depot.Id,
                        //                           Name = depot.Name,
                        //                           Code = depot.Code,
                        //                           IsPlant = depot.IsPlant,
                        //                           IsActive = depot.IsActive
                        //                       }).ToList();

                        //    plant.Rakelist = rakeContext;
                        //}

                        if (depotList != null && depotList.Any())
                        {
                            PlantDepotList.AddRange(depotList);
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

                var bdoList = new List<long>();
                if (inputDto.BDOs.IsAny())
                {
                    bdoList = inputDto.BDOs;
                }
                else
                {
                    //New Reporting to table change
                    //bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                    bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                }

                if (bdoList != null && bdoList.Any())
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

                    var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(a => a.CustomerId).ToList();
                    if (inputDto.PlantId == 0)
                    {
                        //plantIds = _emamiContext.UserDepotMapping.AsNoTracking().Where(w => dealerIds.Contains(w.UserId)).Select(s => s.DepotId).Distinct().ToList();
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
                        IEnumerable<DailyBookedSaudaOutputDto> saudaContext = new List<DailyBookedSaudaOutputDto>();
                        using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                        {
                            var sqlQuery = @"CREATE TABLE #DealerTemp(DealerId BIGINT)
CREATE TABLE #BdoTemp(BdoId BIGINT)
CREATE TABLE #PlantTemp(DealerId BIGINT)
Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)

insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

if(@BdoString!='')
begin
Insert Into #BdoTemp
	Select Data From dbo.Split(@BdoString,',')
	Insert into #DealerTemp select CustomerId from UserCustomerMappings where UserId in (select BdoId from #BdoTemp)
end
else
begin
	Insert into #BdoTemp select UserId from UserReportingToMappings where ReportingToUserId=@UserId
	Insert into #DealerTemp select CustomerId from UserCustomerMappings where UserId in (select BdoId from #BdoTemp)
end

if(@PlantId > 0)
begin 
 insert into #PlantTemp select DepotId from UserDepotMappings where UserId in (select DealerId from #DealerTemp) or DepotId=@PlantId
end

select 
s.UserId,
so.OilTypeId,
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
drop table #UserDivision";
                            var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                            saudaContext = conn.Query<DailyBookedSaudaOutputDto>(sqlQuery, new
                            {
                                UserId = inputDto.LoginUserId,
                                PlantId = inputDto.PlantId,
                                BdoString = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.BDOs),
                                FromDate = inputDto.FromDate,
                                ToDate = inputDto.ToDate
                            });

                        }

                        //var saudaContext = (from so in _emamiContext.SaudaOrders.AsNoTracking()
                        //                    join s in _emamiContext.Sauda.AsNoTracking() on so.SaudaId equals s.Id
                        //                    join ud in divisionslogieduser on new { SalesOrganizationId=s.SalesOrganizationId, DistributionChannelId=s.DistributionChannelId, DivisionId=s.DivisionId}
                        //                    equals new { SalesOrganizationId=ud.SalesOrganizationId, DistributionChannelId=ud.DistributionChannelId, DivisionId=ud.DivisionId}
                        //                 join sku in _emamiContext.Skus.AsNoTracking() on so.SkuId equals sku.Id
                        //                 join o in _emamiContext.OilTypes.AsNoTracking() on so.OilTypeId equals o.Id
                        //                 join u in _emamiContext.Users.AsNoTracking() on s.UserId equals u.Id
                        //                 where so != null 
                        //                 && s != null 
                        //                 && sku != null 
                        //                 && o != null
                        //                // && bdoList.Contains(s.BdoId)
                        //                   && DbFunctions.TruncateTime(so.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                        //                   && DbFunctions.TruncateTime(so.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                        //                   && dealerIds.Contains(s.UserId)
                        //                   && (plantIds.Any() ? plantIds.Contains(so.PlantId) : so.PlantId > 0)
                        //                   && so.StatusId != (int)DTO.Enums.Status.Rejected
                        //                   && so.SalesOrganizationId == sku.SalesOrganizationId && so.DistributionChannelId == sku.DistributionChannelId
                        //                   && so.DivisionId == sku.DivisionId
                        //                 select new {s,so,sku,o,u}
                        //                 );

                        //var saudaContext = _emamiContext.SaudaOrders.AsNoTracking().Join(_emamiContext.Sauda.AsNoTracking(), so => so.SaudaId, s => s.Id, (so, s) => new { so, s })
                        //    .Join(_emamiContext.Skus.AsNoTracking(), soi => soi.so.SkuId, sku => sku.Id, (soi, sku) => new { soi.so, soi.s, sku })
                        //    .Join(_emamiContext.OilTypes.AsNoTracking(), sos => sos.so.OilTypeId, o => o.Id, (sos, o) => new { sos.so, sos.s, sos.sku, o })
                        //    .Join(_emamiContext.Users.AsNoTracking(), souu => souu.s.UserId, u => u.Id, (souu, u) => new { souu.so, souu.s, souu.sku, souu.o, u })
                        //   .Where(_ => _.so != null && _.s != null && _.sku != null && _.o != null
                        //   && DbFunctions.TruncateTime(_.so.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                        //   && DbFunctions.TruncateTime(_.so.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                        //   && dealerIds.Contains(_.s.UserId)
                        //   && plantIds.Contains(_.so.PlantId)
                        //   && _.so.StatusId != (int)DTO.Enums.Status.Rejected
                        //   && _.so.SalesOrganizationId == _.sku.SalesOrganizationId && _.so.DistributionChannelId == _.sku.DistributionChannelId
                        //   && _.so.DivisionId == _.sku.DivisionId
                        //   );

                        if (saudaContext != null)
                        {
                            //if (inputDto.BDOs != null && inputDto.BDOs.Any())
                            //{
                            //    saudaContext = saudaContext.Where(_ => inputDto.BDOs.Contains(_.ucm.UserId));
                            //}
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
                                dailyBookedSaudaOutputDto = saudaContext.Select(_ => new DailyBookedSaudaOutputDto()
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
                                reportData.StateList = dailyBookedSaudaOutputDto.GroupBy(_ => new
                                {
                                    _.StateId,
                                    _.StateName,
                                    //_.ProductGroup,
                                    //_.ProductGroupId
                                }).Select(_ => new StateList()
                                {
                                    StateId = _.Key.StateId,
                                    StateName = _.Key.StateName,
                                    //ProductGroup = _.Key.ProductGroup,
                                    //ProductGroupId = _.Key.ProductGroupId,
                                    OilTypes = _.GroupBy(g => g.OilTypeId).Select(s => new OilTypeList()
                                    {
                                        OilType = s.FirstOrDefault().OilType,
                                        OilTypeId = s.FirstOrDefault().OilTypeId,
                                        QuantityInMT = s.Sum(q => q.QuantityInMT),
                                        QuantityCase = s.Sum(q => q.QuantityCase),
                                        SkuListReportDto = s.GroupBy(sku => sku.SkuId).Select(sk => new SkuListReportDto()
                                        {
                                            SkuName = sk.FirstOrDefault().SkuName,
                                            BidQuantity = sk.Sum(q => q.QuantityInMT),
                                            BidQuantityCase = sk.Sum(q => q.QuantityCase)
                                        }).ToList(),
                                    }).ToList(),
                                    QuantityInMT = _.Sum(s => s.QuantityInMT),
                                    QuantityCase = _.Sum(s => s.QuantityCase)
                                }).ToList();

                                //if (bookedSaudaoutput != null && bookedSaudaoutput.Any())
                                //{
                                //    foreach (var item in bookedSaudaoutput)
                                //    {
                                //        var dto = new BookedSaudaOutputDto();
                                //        var checkOilTypeExists = BookedSaudaOutputDto.FirstOrDefault(_ => _.OilTypeId == item.OilTypeId);
                                //        if (checkOilTypeExists == null)
                                //        {
                                //            dto.OilTypeId = item.OilTypeId;
                                //            dto.OilType = item.OilType;
                                //            BookedSaudaOutputDto.Add(dto);
                                //        }
                                //    }
                                //}
                                //if (BookedSaudaOutputDto != null && BookedSaudaOutputDto.Any())
                                //{
                                //    foreach (var item in BookedSaudaOutputDto)
                                //    {
                                //        var Dto = BookedSaudaOutputDto.FirstOrDefault(_ => _.OilTypeId == item.OilTypeId);
                                //        if (Dto != null)
                                //        {
                                //            if (item.PackGroupId > 0)
                                //            {
                                //                var BulkPack = bookedSaudaoutput.Where(_ => _.OilTypeId == item.OilTypeId && _.ProductGroupId == item.PackGroupId);
                                //                if (BulkPack != null && BulkPack.Any())
                                //                {
                                //                    Dto.QuantityInMT = BulkPack.Sum(_ => _.QuantityInMT);
                                //                }
                                //            }
                                //            else
                                //            {
                                //                var CustomPack = bookedSaudaoutput.Where(_ => _.OilTypeId == item.OilTypeId);
                                //                if (CustomPack != null && CustomPack.Any())
                                //                {
                                //                    Dto.QuantityInMT = CustomPack.Sum(_ => _.QuantityInMT);
                                //                }
                                //            }
                                //            //Dto.QuantityInMT = Dto.BPQuantityInMT + Dto.CPQuantityInMT;
                                //        }
                                //    }
                                //}
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.ZonalTrader);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                List<long> bdoList = new List<long>();
                if (inputDto.BDOs != null && inputDto.BDOs.Any())
                {
                    bdoList.AddRange(inputDto.BDOs);
                }
                else
                {
                    //New Reporting to table change
                    //bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                    bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                }
                if (bdoList != null && bdoList.Any())
                {
                    IQueryable<UserCustomerMapping> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId));
                    if (dealersList != null && dealersList.Any())
                    {
                        //var invoiceContext = _emamiContext.InvoiceDetails.AsNoTracking().Join(_emamiContext.Invoices.AsNoTracking(), id => id.InvoiceId, i => i.Id, (id, i) => new { id, i })
                        //   .Join(_emamiContext.Skus.AsNoTracking(), idi => idi.id.SkuId, sku => sku.Id, (idi, sku) => new { idi.id, idi.i, sku })
                        //   .Join(_emamiContext.OilTypes.AsNoTracking(), ids => ids.id.OilTypeId, o => o.Id, (ids, o) => new { ids.id, ids.i, ids.sku, o })
                        //   .Join(_emamiContext.Users.AsNoTracking(), idsu => idsu.i.UserId, u => u.Id, (idsu, u) => new { idsu.id, idsu.i, idsu.sku, idsu.o, u })
                        //   .Join(_emamiContext.SalesRegister.AsNoTracking(), idl => idl.id.InvoiceId, sr => sr.InvoiceId, (idl, sr) => new { idl.id, idl.i, idl.sku, idl.o, idl.u, sr })
                        //   .Where(_ => _.id != null && _.i != null && _.sku != null && _.o != null
                        //   && DbFunctions.TruncateTime(_.i.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                        //   && DbFunctions.TruncateTime(_.i.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                        //   && dealersList.Any(a => a.CustomerId == _.i.UserId)
                        //   //&& _.i.SalesDocumentType != "ZHCR"
                        //   );
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
                        IEnumerable<DailyBookedSaudaOutputDto> invoiceContext = new List<DailyBookedSaudaOutputDto>();
                        using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                        {
                            var sqlQuery = @"DECLARE @DealerTemp TABLE (DealerId BIGINT)
DECLARE @BdoTemp TABLE (BdoId BIGINT)
DECLARE @UserDivision TABLE (SalesOrganizationId BIGINT, DistributionChannelId BIGINT, DivisionId BIGINT)

insert into @UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

if(@BdoString!='')
begin
Insert Into @BdoTemp
 Select Data From dbo.Split(@BdoString,',')
 Insert into @DealerTemp select CustomerId from UserCustomerMappings where UserId in (select BdoId from @BdoTemp)
end
else
begin
 Insert into @BdoTemp select UserId from UserReportingToMappings where ReportingToUserId=@UserId
 Insert into @DealerTemp select CustomerId from UserCustomerMappings where UserId in (select BdoId from @BdoTemp)
end

select
u.Id as UserId,
s.InvoiceDate as BookedDate,
sku.OilTypeId,
sku.PackGroupId as ProductGroupId,
sku.OilPackGroupTypeId as OilPackGroupType,
s.QuantityMT as QuantityInMT,
s.QuantityCase as QuantityCase,
p.Name as ProductGroup,
u.StateId,
(u.Name+'-'+c.CityName+'/'+st.StateName) as PartyName,
(o.Name+'-'+sorg.Code+'/'+dist.Code+'/'+div.Code) as OilType
from SalesRegisters s with(NOLOCK)
join Users u on u.Code=s.CustomerCode
left join Cities c on u.CityId=c.Id
left join States st on u.StateId=st.Id
join Skus sku on s.MaterialCode=sku.SkuCode and s.SalesOrganizationId=sku.SalesOrganizationId
and s.DistributionChannelId=sku.DistributionChannelId and s.DivisionId=sku.DivisionId
join PackGroups p on p.Id=sku.PackGroupId
join OilTypes o on sku.OilTypeId=o.Id and o.SalesOrganizationId=s.SalesOrganizationId and o.DistributionChannelId=s.DistributionChannelId and o.DivisionId=s.DivisionId
join SalesOrganizations sorg on sorg.Id=o.SalesOrganizationId
join DistributionChannels dist on dist.Id=o.DistributionChannelId
join Divisions div on div.Id=o.DivisionId
join @UserDivision ud on ud.SalesOrganizationId=s.SalesOrganizationId
and ud.DistributionChannelId=s.DistributionChannelId and ud.DivisionId=s.DivisionId
where 
Cast(s.InvoiceDate as date) >= Cast(@FromDate as date)
and Cast(s.InvoiceDate as date) <= Cast(@ToDate as date)
and u.Id in (select DealerId from @DealerTemp)";
                            invoiceContext = conn.Query<DailyBookedSaudaOutputDto>(sqlQuery, new
                            {
                                UserId = inputDto.LoginUserId,
                                BdoString = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.BDOs),
                                FromDate = inputDto.FromDate,
                                ToDate = inputDto.ToDate
                            });

                        }


                        //var invoiceContext = (from s in _emamiContext.SalesRegister.AsNoTracking()
                        //                      join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                        //                      join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                        //                      join ud in divisionslogieduser on new { SalesOrganizationId=s.SalesOrganizationId, DistributionChannelId=s.DistributionChannelId, DivisionId=s.DivisionId } equals new { SalesOrganizationId=ud.SalesOrganizationId, DistributionChannelId=ud.DistributionChannelId, DivisionId=ud.DivisionId }
                        //                      where
                        //                      dealersList.Any(a => a.CustomerId == u.Id)
                        //    && DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                        //    && DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                        //    && s.SalesOrganizationId == sku.SalesOrganizationId && s.DistributionChannelId == sku.DistributionChannelId
                        //     && s.DivisionId == sku.DivisionId
                        //                      select new { s, sku, u }
                        //);

                        // var invoiceContext = _emamiContext.SalesRegister.AsNoTracking()
                        // .Join(_emamiContext.Skus.AsNoTracking(), sr => sr.MaterialCode, sk => sk.SkuCode, (sr, sk) => new { SalesRegister = sr, Sku = sk })
                        // .Join(_emamiContext.Users.AsNoTracking(), sr => sr.SalesRegister.CustomerCode, us => us.Code, (sr, us) => new { SalesRegister = sr.SalesRegister, Sku = sr.Sku, User = us })
                        // .Where(_ => dealersList.Any(a => a.CustomerId == _.User.Id)
                        //   && DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                        //   && DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                        //   && _.SalesRegister.SalesOrganizationId == _.Sku.SalesOrganizationId && _.SalesRegister.DistributionChannelId == _.Sku.DistributionChannelId
                        //    && _.SalesRegister.DivisionId == _.Sku.DivisionId
                        ////&& _.Sku.DivisionId== userContext.DivisionId
                        //);

                        var cityContext = _emamiContext.City.AsNoTracking();
                        var stateContext = _emamiContext.State.AsNoTracking();
                        if (invoiceContext != null)
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

                            var result = invoiceContext.GroupBy(_ => new { _.OilType, _.OilTypeId })
                                    .Select(s => new BookedSaudaOutputDto()
                                    {
                                        OilType = s.Key.OilType,
                                        OilTypeId = s.Key.OilTypeId,
                                        BakeryquantityInMT = s.Where(_ => _.OilTypeId == s.Key.OilTypeId && _.ProductGroupId == (int)DTO.Enums.PackGroupType.Bakery).Select(_ => _.QuantityInMT).DefaultIfEmpty(0).Sum(),
                                        LauricquantityInMT = s.Where(_ => _.OilTypeId == s.Key.OilTypeId && _.ProductGroupId == (int)DTO.Enums.PackGroupType.Lauric).Select(_ => _.QuantityInMT).DefaultIfEmpty(0).Sum(),
                                        premiumquantityInMT = s.Where(_ => _.OilTypeId == s.Key.OilTypeId && _.ProductGroupId == (int)DTO.Enums.PackGroupType.Premium).Select(_ => _.QuantityInMT).DefaultIfEmpty(0).Sum(),
                                        PopularquantityInMT = s.Where(_ => _.OilTypeId == s.Key.OilTypeId && _.ProductGroupId == (int)DTO.Enums.PackGroupType.Popular).Select(_ => _.QuantityInMT).DefaultIfEmpty(0).Sum(),
                                        QuantityInMT = s.Select(_ => _.QuantityInMT).DefaultIfEmpty(0).Sum(),
                                        PremiumQuantityCase = s.Where(_ => _.OilTypeId == s.Key.OilTypeId && _.ProductGroupId == (int)DTO.Enums.PackGroupType.Premium).Select(_ => _.QuantityCase).DefaultIfEmpty(0).Sum(),
                                        LauricQuantityCase = s.Where(_ => _.OilTypeId == s.Key.OilTypeId && _.ProductGroupId == (int)DTO.Enums.PackGroupType.Lauric).Select(_ => _.QuantityCase).DefaultIfEmpty(0).Sum(),
                                        PopularQuantityCase = s.Where(_ => _.OilTypeId == s.Key.OilTypeId && _.ProductGroupId == (int)DTO.Enums.PackGroupType.Popular).Select(_ => _.QuantityCase).DefaultIfEmpty(0).Sum(),
                                        BakeryQuantityCase = s.Where(_ => _.OilTypeId == s.Key.OilTypeId && _.ProductGroupId == (int)DTO.Enums.PackGroupType.Bakery).Select(_ => _.QuantityCase).DefaultIfEmpty(0).Sum(),
                                        QuantityCase = s.Select(_ => _.QuantityCase).DefaultIfEmpty(0).Sum()
                                    }).ToList();
                            return _resultService.SuccessObject(result);
                            #region OldCode
                            //if (invoiceContext != null)
                            //{

                            //    dailyBookedSaudaOutputDto = invoiceContext.Select(_ => new DailyBookedSaudaOutputDto()
                            //    {
                            //        BookedDate = _.BookedDate,
                            //        PartyName = _.PartyName,
                            //        OilType = _.OilType,
                            //        OilTypeId = _.OilTypeId,
                            //        ProductGroupId = _.ProductGroupId,
                            //        ProductGroup = _.ProductGroup,
                            //        QuantityInMT = _.QuantityInMT,
                            //        //SaleDocumentType = _.i.SalesDocumentType,
                            //        //MaterialType = _.Sku.MaterialType.Name
                            //    }).ToList();


                            //}

                            //if (dailyBookedSaudaOutputDto != null && dailyBookedSaudaOutputDto.Any())
                            //{


                            //    var bookedSaudaoutput = dailyBookedSaudaOutputDto.GroupBy(_ => new
                            //    {
                            //        _.OilType,
                            //        _.OilTypeId,
                            //        _.ProductGroup,
                            //        _.ProductGroupId,
                            //        //_.SaleDocumentType,
                            //        //_.MaterialType
                            //    }).Select(_ => new DailyBookedSaudaOutputDto()
                            //    {
                            //        OilType = _.Key.OilType,
                            //        OilTypeId = _.Key.OilTypeId,
                            //        ProductGroup = _.Key.ProductGroup,
                            //        ProductGroupId = _.Key.ProductGroupId,
                            //        //SaleDocumentType = _.Key.SaleDocumentType,
                            //        QuantityInMT = _.Sum(s => s.QuantityInMT),
                            //        //MaterialType = _.Key.MaterialType
                            //    }).ToList();

                            //    if (bookedSaudaoutput != null && bookedSaudaoutput.Any())
                            //    {
                            //        foreach (var item in bookedSaudaoutput)
                            //        {
                            //            var dto = new BookedSaudaOutputDto();
                            //            var checkOilTypeExists = BookedSaudaOutputDto.FirstOrDefault(_ => _.OilTypeId == item.OilTypeId);
                            //            if (checkOilTypeExists == null)
                            //            {
                            //                dto.OilTypeId = item.OilTypeId;
                            //                dto.OilType = item.OilType;
                            //                //dto.MaterialType = item.MaterialType;
                            //                BookedSaudaOutputDto.Add(dto);
                            //            }
                            //        }
                            //    }
                            //if (BookedSaudaOutputDto != null && BookedSaudaOutputDto.Any())
                            //{
                            //    foreach (var item in BookedSaudaOutputDto)
                            //    {
                            //        var Dto = BookedSaudaOutputDto.FirstOrDefault(_ => _.OilTypeId == item.OilTypeId);
                            //        if (Dto != null)
                            //        {
                            //            var PremiumPack = bookedSaudaoutput.Where(_ => _.OilTypeId == item.OilTypeId && _.ProductGroupId == (int)DTO.Enums.PackGroupType.Premium);
                            //            if (PremiumPack != null && PremiumPack.Any())
                            //            {
                            //                Dto.premiumquantityInMT = PremiumPack.Sum(_ => _.QuantityInMT);
                            //            }
                            //            var LauricPack = bookedSaudaoutput.Where(_ => _.OilTypeId == item.OilTypeId && _.ProductGroupId == (int)DTO.Enums.PackGroupType.Lauric);
                            //            if (LauricPack != null && LauricPack.Any())
                            //            {
                            //                Dto.LauricquantityInMT = LauricPack.Sum(_ => _.QuantityInMT);
                            //            }
                            //            var PopularPack = bookedSaudaoutput.Where(_ => _.OilTypeId == item.OilTypeId && _.ProductGroupId == (int)DTO.Enums.PackGroupType.Popular);
                            //            if (PopularPack != null && PopularPack.Any())
                            //            {
                            //                Dto.PopularquantityInMT = PopularPack.Sum(_ => _.QuantityInMT);
                            //            }
                            //            var BakeryPack = bookedSaudaoutput.Where(_ => _.OilTypeId == item.OilTypeId && _.ProductGroupId == (int)DTO.Enums.PackGroupType.Bakery);
                            //            if (BakeryPack != null && BakeryPack.Any())
                            //            {
                            //                Dto.BakeryquantityInMT = BakeryPack.Sum(_ => _.QuantityInMT);
                            //            }
                            //            Dto.QuantityInMT = Dto.premiumquantityInMT + Dto.LauricquantityInMT + Dto.PopularquantityInMT + Dto.BakeryquantityInMT;
                            //if (item.PackGroupId > 0)
                            //{
                            //    var BulkPack = bookedSaudaoutput.Where(_ => _.OilTypeId == item.OilTypeId && _.ProductGroupId == item.PackGroupId);
                            //    if (BulkPack != null && BulkPack.Any())
                            //    {
                            //        Dto.QuantityInMT = BulkPack.Sum(_ => _.QuantityInMT);
                            //    }
                            //}
                            //else
                            //{
                            //    var CustomPack = bookedSaudaoutput.Where(_ => _.OilTypeId == item.OilTypeId);
                            //    if (CustomPack != null && CustomPack.Any())
                            //    {
                            //        Dto.QuantityInMT = CustomPack.Sum(_ => _.QuantityInMT);
                            //    }
                            //}
                            //            //Dto.QuantityInMT = Dto.BPQuantityInMT + Dto.CPQuantityInMT;
                            //        }
                            //    }
                            //}
                            #endregion
                        }
                        //}
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
        #endregion

    }
}
