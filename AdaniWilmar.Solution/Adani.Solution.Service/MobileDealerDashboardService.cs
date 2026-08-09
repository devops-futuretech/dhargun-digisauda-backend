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
using Kendo.Mvc.Extensions;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Adani.Solution.Service
{
    public interface IMobileDealerDashboardServices
    {
        ResultDto DashboardOverallSauda(DashboardOverallSaudaInputDto inputDto);
        ResultDto DashboardWeekwiseOverallSauda(LoginUserIdDto loginUserIdDto);
        ResultDto DashboardOverallSales(DashboardOverallSaudaInputDto inputDto);
        ResultDto DashboardWeekwiseOverallSales(LoginUserIdDto loginUserIdDto);
        ResultDto DashboardSaudalistByDealers(DashboardOverallSaudaInputDto inputDto);
        ResultDto DashboardSaleslistByDealers(DashboardOverallSaudaInputDto inputDto);
        ResultDto DashboardPackwiseSaleslist(DashboardSaudaDetailsByDealersInputDto inputDto);
        ResultDto DashboardSaudaDetailsByDealers(DashboardSaudaDetailsByDealersInputDto inputDto);
        ResultDto InvoicesByDealers(DashboardSaudaDetailsByDealersInputDto inputDto);
        ResultDto InvoiceDetailsByDealers(IdInputDto inputDto);
        ResultDto DueForTomorrowList(LoginUserIdDto inputDto);
        ResultDto GetTickerListForToday();
        ResultDto GetCreditNote(LoginUserIdDto loginUserIdDto);
        ResultDto GetAccountStatement(LoginUserIdDto loginUserIdDto);
        ResultDto GetDailyRate(DailyRateInputDto inputDto);
        ResultDto GetDealerStatistics(SaudaFilterDto saudaFilterDto);
        ResultDto GetCustomerLedger(LoginUserIdDto inputDto);
        ResultDto PackwiseInvoicesByDealer(DashboardSaudaDetailsByDealersInputDto inputDto);
        ResultDto PackwiseInvoiceDetailsByDealer(IdInputDto inputDto);
        ResultDto DealerPlantDepotDetails(LoginUserIdDto inputDto);
        ResultDto GetDailyRateNew(DailyRateInputDto inputDto);
        ResultDto GetDailyRateWeb(PricePublistInputDataDto inputDto);
        ResultDto GetCustomerLedgerRolewise(LoginUserIdDto inputDto);
    }
    public class MobileDealerDashboardService : IMobileDealerDashboardServices
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Mobile Dashboard Dealer Services");
        private const string ServiceName = "Mobile Dashboard Dealer Services";
        private string _methodName;
        private readonly IResultService _resultService;

        public MobileDealerDashboardService(IAdaniContext salesContext, IResultService resultService)
        {
            try
            {
                _emamiContext = salesContext;
                _resultService = resultService;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for sauda Service", exception);
            }
        }
        private ResultDto NotFoundResult()
        {
            var resultDto = new ResultDto();
            resultDto.IsSuccess = false;
            resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
            resultDto.ErrorDto.Message = Constants.RecordNotFound;
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
        public ResultDto DashboardOverallSauda(DashboardOverallSaudaInputDto inputDto)
        {
            _methodName = "DashboardOverallSauda";
            var dashboardOverallsaudaOutpuDto = new List<DashboardOverallsaudaOutpuDto>();
            if (inputDto == null)
            {
                return NotFoundResult();
            }
            try
            {
                List<MonthDto> months = new List<MonthDto>();
                months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);
                var status = Constants.OverallSaudaStatus;
                IEnumerable<DashboardSauda> sauda= new List<DashboardSauda>();
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {
                    
                    var sqlQuery = @"select 
                            so.CreatedDate as Date,
                            so.BidQuantity as Achievment
                            from Saudas s with(NOLOCK)
                            join SaudaOrders so with(NOLOCK) on s.Id=so.SaudaId 
                            where s.UserId=@UserId
                            and Cast(so.CreatedDate as date) >= Cast(@FromDate as date)
                            and Cast(so.CreatedDate as date) <= Cast(@ToDate as date)
                            and so.StatusId in @StatusId";
                    sauda = conn.Query<DashboardSauda>(sqlQuery, new
                    {
                        UserId = inputDto.LoginUserId,
                        FromDate=inputDto.FromDate,
                        ToDate=inputDto.ToDate,
                        StatusId= status
                    });

                }

                //var sauda1 = _emamiContext.Sauda.AsNoTracking().Join(_emamiContext.SaudaOrders.AsNoTracking(), s => s.Id, so => so.SaudaId, (s, so) => new { s, so })
                //    .Where(_ => _.s.UserId == inputDto.LoginUserId && DbFunctions.TruncateTime(_.so.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                //    DbFunctions.TruncateTime(_.so.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && status.Contains(_.so.StatusId))
                //    .Select(s => new
                //    {
                //        Date = s.so.CreatedDate,
                //        Achievment = s.so.BidQuantity
                //    });
                var target = _emamiContext.UserCustomerSaudaTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId);
                foreach (var item in months)
                {
                    var outputDto = new DashboardOverallsaudaOutpuDto();
                    var targetContext = target.Where(_ =>  _.MonthId == item.Id && _.Year == item.Year).ToList();
                    if (targetContext != null)
                    {
                        outputDto.TotalTarget = targetContext.Sum(_ => _.Target);
                    }
                    outputDto.MonthId = item.Id;
                    outputDto.Month = new DateTime(DateTime.Now.Year, item.Id, 1).ToString("MMMM");

                    //  if (saudaContext != null && saudaContext.Any())
                    //  {
                    //var saudaIds = saudaContext.Select(_ => _.Id).ToList();
                    //foreach (var detail in saudaContext)
                    //{
                    // var saudadetail = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => saudaIds.Contains(_.SaudaId)  && status.Contains(_.StatusId));
                    //  if (saudadetail != null && saudadetail.Any())
                    //  {
                    //var acheivment = new AchievmentDetailsDto
                    //{
                    //    UserId = detail.CreatedBy,
                    //    Date = detail.CreatedDate,
                    //    Achievment = saudadetail.Sum(_ => _.BidQuantity)
                    //};
                    //  outputDto.AchievmentDetailsDto.Add(acheivment);
                    outputDto.OverallSauda = sauda.Where(_ => _.Date.Date >= item.StartDate.Date &&
                                                    _.Date.Date <= item.EndDate.Date).Select(s => s.Achievment)
                                                    .DefaultIfEmpty(0).Sum();
                      //  }
                        //}
                       
                  //  }
                    dashboardOverallsaudaOutpuDto.Add(outputDto);
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

                return SucessResult(resultData);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }

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

                //Weekwise report

                //var query = from sauda in _emamiContext.Sauda.AsNoTracking().Where(_ => dealersList.Any(a => a.CustomerId == _.UserId)).ToList()
                //            join saudaorders in _emamiContext.SaudaOrders.Where(_ => _.CreatedDate.Month == DateTime.UtcNow.Month).ToList() on sauda.Id equals saudaorders.SaudaId
                //            group saudaorders by new { Week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(saudaorders.CreatedDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday), sauda.UserId } into g
                //            select new { Weeks = g.Key, Sum = g.Sum(p => p.BidQuantity) };
                //var s = query.ToList();
                IEnumerable<DashboardSauda> sauda = new List<DashboardSauda>();
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {
                    
                    var sqlQuery = @"select 
                            so.CreatedDate as Date,
                            so.BidQuantity as Achievment
                            from Saudas s with(NOLOCK)
                            join SaudaOrders so with(NOLOCK) on s.Id=so.SaudaId 
                            where s.UserId=@UserId
                            and so.StatusId in @StatusId";
                    sauda = conn.Query<DashboardSauda>(sqlQuery, new
                    {
                        UserId = loginUserIdDto.LoginUserId,
                        StatusId = status
                    });

                }
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

                    var saudaContextList = sauda.Where(_ =>  _.Date.Date >= wStartDate.Date &&
                    _.Date.Date <= wEndDate.Date ).ToList();
                    if (saudaContextList != null && saudaContextList.Any())
                    {
                        weekwiseTargetAchieved.WeekId = weekId;
                        weekwiseTargetAchieved.Week = "Week " + weekId;
                        weekwiseTargetAchieved.Achievement = saudaContextList.Sum(_ => _.Achievment);
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
            if (inputDto == null)
            {
                return NotFoundResult();
            }
            var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
            if (userContext == null)
            {
                return _resultService.ErrorMessage(Constants.UserNotFound);
            }
            try
            {
                List<MonthDto> months = new List<MonthDto>();
                months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);

                //var salesContext = _emamiContext.SalesRegister.AsNoTracking()
                //        .Join(_emamiContext.Skus.AsNoTracking(), sr => sr.MaterialCode, sk => sk.SkuCode, (sr, sk) => new { SalesRegister = sr, Sku = sk })
                //        .Join(_emamiContext.Users.AsNoTracking(), sr => sr.SalesRegister.CustomerCode, us => us.Code, (sr, us) => new { SalesRegister = sr.SalesRegister, Sku = sr.Sku, User = us })
                //        .Where(_ => _.User.Id == userContext.Id && DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                //                DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                //                && _.SalesRegister.SalesOrganizationId == _.Sku.SalesOrganizationId && _.SalesRegister.DistributionChannelId == _.Sku.DistributionChannelId
                //                && _.SalesRegister.DivisionId == _.Sku.DivisionId
                //          ).Select(s => new {
                //              Date = s.SalesRegister.InvoiceDate != null ? s.SalesRegister.InvoiceDate : DateTime.Now,
                //              Achievment = s.SalesRegister.QuantityMT
                //          });

                foreach (var item in months)
                {
                    var outputDto = new DashboardOverallSalesOutpuDto();
                    var targetContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId && _.MonthId == item.Id && _.Year == item.Year).ToList();
                    if (targetContext != null)
                    {
                        outputDto.TotalTarget = targetContext.Sum(_ => _.Target);
                    }
                    outputDto.MonthId = item.Id;
                    outputDto.Month = new DateTime(DateTime.Now.Year, item.Id, 1).ToString("MMMM");

                    IEnumerable<DashboardSauda> salesContext = new List<DashboardSauda>();
                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {
                        
                        var sqlQuery = @"select 
                            InvoiceDate as Date,
                            QuantityMT as Achievment
                            from SalesRegisters s with(NOLOCK)
							join Skus sku on s.MaterialCode=sku.SkuCode and s.SalesOrganizationId=sku.SalesOrganizationId
							and s.DistributionChannelId=sku.DistributionChannelId and s.DivisionId=sku.DivisionId
                            join Users u with(NOLOCK) on s.CustomerCode=u.Code
                            where u.Id=@UserId";
                        salesContext = conn.Query<DashboardSauda>(sqlQuery, new
                        {
                            UserId = userContext.Id,
                            FromDate=inputDto.FromDate,
                            ToDate=inputDto.ToDate
                        });

                    }

                    //var salesContext = _emamiContext.SalesRegister.AsNoTracking()
                    //                         .Where(_ => _.UserId == userContext.Id
                    //                         && (DbFunctions.TruncateTime(_.InvoiceDate) >= DbFunctions.TruncateTime(item.StartDate) &&
                    //                         DbFunctions.TruncateTime(_.InvoiceDate) <= DbFunctions.TruncateTime(item.EndDate)) 
                    //                         //&& _.SkuId != 0
                    //                         //&& _.User.DivisionId == userContext.DivisionId
                    //                         ).ToList();

                    outputDto.OverallSales = salesContext.Where(_ => (_.Date.Date >= item.StartDate.Date &&
                            _.Date.Date <= item.EndDate.Date)
                            ).OrderByDescending(_ => _.Date).Select(a => a.Achievment).DefaultIfEmpty(0).Sum();

                    //if (salesContext != null && salesContext.Any())
                    //{
                        //foreach (var details in salesContext)
                        //{
                            //var InvoiceDetailsContext = _emamiContext.SalesRegister.AsNoTracking().Where(_ => _.InvoiceNumber == details.InvoiceNumber).ToList();
                            //var acheivment = new AchievmentDetailsDto
                            //{
                            //    //UserId = details.UserId,
                            //    //Date = details.InvoiceDate,
                            //    Achievment = salesContext.Sum(_ => (decimal?)_.QuantityMT) ?? 0
                            //};
                          //  outputDto.AchievmentDetailsDto.Add(acheivment);
                        //}
                      //  outputDto.OverallSales = salesContext.Sum(_ => (decimal?)_.SalesRegister.QuantityMT) ?? 0;
                   // }
                    dashboardOverallsaudaOutpuDto.Add(outputDto);
                }

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

                return SucessResult(resultData);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
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

                //Weekwise report

                //var query = from sauda in _emamiContext.Sauda.AsNoTracking().Where(_ => dealersList.Any(a => a.CustomerId == _.UserId)).ToList()
                //            join saudaorders in _emamiContext.SaudaOrders.Where(_ => _.CreatedDate.Month == DateTime.UtcNow.Month).ToList() on sauda.Id equals saudaorders.SaudaId
                //            group saudaorders by new { Week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(saudaorders.CreatedDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday), sauda.UserId } into g
                //            select new { Weeks = g.Key, Sum = g.Sum(p => p.BidQuantity) };
                //var s = query.ToList();
                IEnumerable<DashboardSauda> sales = new List<DashboardSauda>();
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {
                   
                    var sqlQuery = @"select 
                            InvoiceDate as Date,
                            QuantityMT as Achievment
                            from SalesRegisters s with(NOLOCK)
                            join Skus sku on s.MaterialCode=sku.SkuCode and s.SalesOrganizationId=sku.SalesOrganizationId
							and s.DistributionChannelId=sku.DistributionChannelId and s.DivisionId=sku.DivisionId
                            join Users u with(NOLOCK) on s.CustomerCode=u.Code
                            where u.Id=@UserId";
                    sales = conn.Query<DashboardSauda>(sqlQuery, new
                    {
                        UserId = userContext.Id
                    });

                }

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

                    var salesContextList = sales.Where(_ =>  _.Date.Date >= wStartDate.Date &&
                                             _.Date.Date <= wEndDate.Date).ToList();
                    //var salesContextList = _emamiContext.SalesRegister.AsNoTracking()
                    //                   .Join(_emamiContext.Users.AsNoTracking(), sr => sr.CustomerCode, us => us.Code, (sr, us) => new { SalesRegister = sr, User = us })
                    //                        .Where(_ => _.User.Id == userContext.Id
                    //                        && (DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) >= DbFunctions.TruncateTime(wStartDate) &&
                    //                        DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) <= DbFunctions.TruncateTime(wEndDate))
                    //                        //&& _.SalesRegister.SkuId != 0
                    //                        //&& _.User.DivisionId == userContext.DivisionId
                    //                        ).ToList();
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
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto DashboardSaudalistByDealers(DashboardOverallSaudaInputDto inputDto)
        {
            _methodName = "DashboardSaudalistByDealers";
            var dashboardDetailsByDealersOutputDto = new List<DashboardDetailsByDealersOutputDto>();
            if (inputDto == null)
            {
                return NotFoundResult();
            }
            try
            {
                var saudaContext = _emamiContext.Sauda.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId && DbFunctions.TruncateTime(_.BiddingDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                    DbFunctions.TruncateTime(_.BiddingDate) <= DbFunctions.TruncateTime(inputDto.ToDate)).OrderByDescending(_ => _.BiddingDate).ToList();

                List<MonthDto> months = new List<MonthDto>();
                months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);
                decimal TotalTarget = 0;
                decimal TotalAcheivement = 0;
                var outputDto = new DashboardDetailsByDealersOutputDto();
                foreach (var month in months)
                {
                    var targetContext = _emamiContext.UserCustomerSaudaTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId && _.MonthId == month.Id && _.FinancialYearId == month.Year).ToList();
                    if (targetContext != null)
                    {
                        TotalTarget = TotalTarget + targetContext.Sum(_ => _.Target);
                    }
                    var saudaByUserContext = saudaContext.Where(_ => _.UserId == inputDto.LoginUserId).ToList();
                    if (saudaByUserContext != null)
                    {
                        foreach (var sauda in saudaByUserContext)
                        {
                            var saudaorderContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == sauda.Id).ToList();
                            if (saudaorderContext != null)
                            {
                                TotalAcheivement = TotalAcheivement + saudaorderContext.Sum(_ => _.BidQuantity);
                            }
                        }
                    }
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                outputDto.DealerId = inputDto.LoginUserId;
                outputDto.Dealer = userContext.Name;
                outputDto.Dealer = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == userContext.CityId).CityName;
                outputDto.Target = TotalTarget;
                outputDto.Achievement = TotalAcheivement;
                dashboardDetailsByDealersOutputDto.Add(outputDto);


                return SucessResult(dashboardDetailsByDealersOutputDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }
        public ResultDto DashboardSaleslistByDealers(DashboardOverallSaudaInputDto inputDto)
        {
            _methodName = "DashboardSaleslistByDealers";
            var dashboardDetailsByDealersOutputDto = new List<DashboardDetailsByDealersOutputDto>();
            if (inputDto == null)
            {
                return NotFoundResult();
            }
            try
            {
                var invoiceContext = _emamiContext.Invoices.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId && DbFunctions.TruncateTime(_.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                    DbFunctions.TruncateTime(_.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)).OrderByDescending(_ => _.InvoiceDate).ToList();



                List<MonthDto> months = new List<MonthDto>();
                months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);
                decimal TotalTarget = 0;
                decimal TotalAcheivement = 0;
                var outputDto = new DashboardDetailsByDealersOutputDto();
                foreach (var month in months)
                {
                    var targetContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId && _.MonthId == month.Id && _.FinancialYearId == month.Year).ToList();
                    if (targetContext != null)
                    {
                        TotalTarget = TotalTarget + targetContext.Sum(_ => _.Target);
                    }

                    var invoiceByUserContext = invoiceContext.Where(_ => _.UserId == inputDto.LoginUserId).ToList();
                    if (invoiceByUserContext != null)
                    {
                        foreach (var invoice in invoiceByUserContext)
                        {
                            var invoicedetailContext = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.InvoiceId == invoice.Id).ToList();
                            if (invoicedetailContext != null)
                            {
                                TotalAcheivement = TotalAcheivement + invoicedetailContext.Sum(_ => _.ActualBilledQuantity);
                            }
                        }
                    }
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                outputDto.DealerId = inputDto.LoginUserId;
                outputDto.Dealer = userContext.Name;
                outputDto.TownName = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == userContext.CityId).CityName;
                outputDto.Target = TotalTarget;
                outputDto.Achievement = TotalAcheivement;
                dashboardDetailsByDealersOutputDto.Add(outputDto);


                return SucessResult(dashboardDetailsByDealersOutputDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }


        public ResultDto DashboardSaudaDetailsByDealers(DashboardSaudaDetailsByDealersInputDto inputDto)
        {
            _methodName = "DashboardSaudaDetailsByDealers";
            var dashboardDetailsByDealersOutputDto = new DashboardSaudaDetailsByDealersOutputDto();
            if (inputDto == null)
            {
                return NotFoundResult();
            }
            try
            {
                var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);

                var saudaContext = _emamiContext.Sauda.AsNoTracking().Where(_ => _.UserId == inputDto.DealerId && DbFunctions.TruncateTime(_.BiddingDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                        DbFunctions.TruncateTime(_.BiddingDate) <= DbFunctions.TruncateTime(inputDto.ToDate)).ToList();

                decimal TotalQuantity = 0;
                decimal TotalBookedValue = 0;
                foreach (var item in saudaContext)
                {
                    var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == item.Id).ToList();
                    var dispatchedQuantityContext = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => _.SaudaId == item.Id
                        && _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected).ToList();
                    var DashboardSaudaDetails = new DashboardSaudaDetailsOutputDto()
                    {
                        SaudaId = item.Id,
                        //SaudaNumber = item.SaudaNumber,
                        SaudaNumber = item.Id.ToString(),
                        SaudaBookedQuantity = saudaOrderContext.Sum(_ => _.BidQuantity),
                        DispatchedQuantity = dispatchedQuantityContext.Sum(_ => _.LiftingQuantity)
                    };
                    TotalQuantity = TotalQuantity + saudaOrderContext.Sum(_ => _.BidQuantity);
                    TotalBookedValue = TotalBookedValue + saudaOrderContext.Sum(_ => _.BidPrice);
                }
                dashboardDetailsByDealersOutputDto.DealerId = dealerContext.Id;
                dashboardDetailsByDealersOutputDto.Dealer = dealerContext.Name;
                dashboardDetailsByDealersOutputDto.TotalQuantity = TotalQuantity;
                dashboardDetailsByDealersOutputDto.TotalBookedSaudaValue = TotalBookedValue;
                return SucessResult(dashboardDetailsByDealersOutputDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }
        public ResultDto InvoicesByDealers(DashboardSaudaDetailsByDealersInputDto inputDto)
        {
            _methodName = "DashboardSalesDetailsByDealers";
            var dashboardDetailsByDealersOutputDto = new DashboardSalesDetailsByDealersOutputDto();
            if (inputDto == null)
            {
                return NotFoundResult();
            }
            try
            {
                var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);

                if (dealerContext != null)
                {
                    var invoiceContext = _emamiContext.SalesRegister.AsNoTracking()
                        .Join(_emamiContext.Users.AsNoTracking(), sr => sr.CustomerCode, us => us.Code, (sr, us) => new { SalesRegister = sr, User = us })
                        .Join(_emamiContext.Skus.AsNoTracking(), sr => sr.SalesRegister.MaterialCode, sk => sk.SkuCode, (sr, sk) => new { SalesRegister = sr, Sku = sk })
                        .Where(_ => _.SalesRegister.User.Id == dealerContext.Id && DbFunctions.TruncateTime(_.SalesRegister.SalesRegister.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                    DbFunctions.TruncateTime(_.SalesRegister.SalesRegister.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    //&& _.Sku.DivisionId == _.SalesRegister.User.DivisionId
                    && _.SalesRegister.SalesRegister.SalesOrganizationId == _.Sku.SalesOrganizationId && _.SalesRegister.SalesRegister.DistributionChannelId == _.Sku.DistributionChannelId
                        && _.SalesRegister.SalesRegister.DivisionId == _.Sku.DivisionId
                    ).ToList();

                    decimal TotalQuantity = 0;
                    decimal TotalInvoiceValue = 0;
                    if (invoiceContext != null)
                    {
                        foreach (var item in invoiceContext)
                        {
                            //var invoicedetailsContext = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.InvoiceId == item.Id).ToList();
                            //var DashboardSaudaDetails = new DashboardSalesDetailsOutputDto()
                            //{
                            //    InvoiceId = item.Id,
                            //    InvoiceNumber = item.BillingDocument,
                            //    InvoiceValue = item.NetValue,
                            //    InvoiceDate = item.InvoiceDate,
                            //    TotalQuantity = invoicedetailsContext.Sum(_ => _.ActualBilledQuantity)
                            //};
                            //TotalQuantity = TotalQuantity + invoicedetailsContext.Sum(_ => _.ActualBilledQuantity);
                            //TotalInvoiceValue = TotalInvoiceValue + item.NetValue;
                            //dashboardDetailsByDealersOutputDto.DashboardSalesDetails.Add(DashboardSaudaDetails);

                            var invoicedetailsContext = _emamiContext.SalesRegister.AsNoTracking().Where(_ => _.Id == item.SalesRegister.SalesRegister.Id).Select(s => s.QuantityMT).DefaultIfEmpty(0).Sum();
                            var DashboardSaudaDetails = new DashboardSalesDetailsOutputDto()
                            {
                                InvoiceId = item.SalesRegister.SalesRegister.Id,
                                InvoiceNumber = item.SalesRegister.SalesRegister.InvoiceNumber,
                                InvoiceValue = Convert.ToDecimal(item.SalesRegister.SalesRegister.TotalAmount),
                                InvoiceDate = item.SalesRegister.SalesRegister.InvoiceDate,
                                PackGroupId = (long)item.Sku.PackGroupId,
                                TotalQuantity = invoicedetailsContext
                            };
                            TotalQuantity = TotalQuantity + invoicedetailsContext;
                            TotalInvoiceValue = TotalInvoiceValue + Convert.ToDecimal(item.SalesRegister.SalesRegister.TotalAmount);
                            dashboardDetailsByDealersOutputDto.DashboardSalesDetails.Add(DashboardSaudaDetails);

                        }
                        dashboardDetailsByDealersOutputDto.DealerId = dealerContext.Id;
                        dashboardDetailsByDealersOutputDto.Dealer = dealerContext.Name;
                        dashboardDetailsByDealersOutputDto.TownName = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == dealerContext.CityId)?.CityName;
                        dashboardDetailsByDealersOutputDto.TotalQuantity = TotalQuantity;
                        dashboardDetailsByDealersOutputDto.TotalBookedInvoiceValue = TotalInvoiceValue;
                    }
                }
                return SucessResult(dashboardDetailsByDealersOutputDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }
        public ResultDto DashboardPackwiseSaleslist(DashboardSaudaDetailsByDealersInputDto inputDto)
        {
            var invoiceList = new DashboardDetailsByDealersOutputDto();
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                //var invoiceDetailsContextList = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.Invoice != null && DbFunctions.TruncateTime(_.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                //  DbFunctions.TruncateTime(_.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && _.Invoice.UserId == inputDto.LoginUserId);
                //if (invoiceDetailsContextList != null && invoiceDetailsContextList.Any())
                //{

                //    invoiceList.DealerId = inputDto.LoginUserId;

                //    invoiceList.Dealer = userContext.Name;
                //    var cityContext = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == userContext.CityId);
                //    if (cityContext != null)
                //    {
                //        invoiceList.TownName = cityContext.CityName;
                //    }

                //    if (inputDto.IsBulkPack == true)
                //    {
                //        var bulkPackContextList = invoiceDetailsContextList.AsNoTracking().Join(_emamiContext.Skus.AsNoTracking(), i => i.SkuId, s => s.Id, (i, s) => new { i, s })
                //            .Where(_ => _.s.PackGroupId == (int)DTO.Enums.PackGroupType.BulkPacking && _.i.Invoice != null && _.i.Invoice.UserId == inputDto.LoginUserId);
                //        if (bulkPackContextList != null && bulkPackContextList.Any())
                //        {
                //            invoiceList.TotalQuantity = bulkPackContextList.Sum(_ => _.i.ActualBilledQuantity);
                //            invoiceList.TotalBookedInvoiceValue = bulkPackContextList.Sum(_ => _.i.Invoice.NetValue);
                //            invoiceList.DashboardSalesDetails = bulkPackContextList.GroupBy(_ => _.i.InvoiceId).Select(_ => new DashboardSalesDetailsOutputDto()
                //            {
                //                InvoiceId = _.FirstOrDefault().i.InvoiceId,
                //                InvoiceNumber = _.FirstOrDefault().i.Invoice.BillingDocument,
                //                InvoiceValue = _.FirstOrDefault().i.Invoice.NetValue,
                //                InvoiceDate = _.FirstOrDefault().i.Invoice.InvoiceDate,
                //                TotalQuantity = _.Sum(s => s.i.ActualBilledQuantity),
                //            }).ToList();

                //        }
                //    }
                //    if (inputDto.IsBulkPack == false)
                //    {
                //        var customPackContextList = invoiceDetailsContextList.AsNoTracking().Join(_emamiContext.Skus.AsNoTracking(), i => i.SkuId, s => s.Id, (i, s) => new { i, s })
                //            .Where(_ => _.s.PackGroupId == (int)DTO.Enums.PackGroupType.ConsumerPacking && _.i.Invoice != null && _.i.Invoice.UserId == inputDto.LoginUserId);
                //        if (customPackContextList != null && customPackContextList.Any())
                //        {
                //            invoiceList.TotalQuantity = customPackContextList.Sum(_ => _.i.ActualBilledQuantity);
                //            invoiceList.TotalBookedInvoiceValue = customPackContextList.Sum(_ => _.i.Invoice.NetValue);
                //            invoiceList.DashboardSalesDetails = customPackContextList.GroupBy(_ => _.i.InvoiceId).Select(_ => new DashboardSalesDetailsOutputDto()
                //            {
                //                InvoiceId = _.FirstOrDefault().i.InvoiceId,
                //                InvoiceNumber = _.FirstOrDefault().i.Invoice.BillingDocument,
                //                InvoiceValue = _.FirstOrDefault().i.Invoice.NetValue,
                //                InvoiceDate = _.FirstOrDefault().i.Invoice.InvoiceDate,
                //                TotalQuantity = _.Sum(s => s.i.ActualBilledQuantity),
                //            }).ToList();
                //        }
                //    }
                //    return _resultService.SuccessObject(invoiceList);
                //}
                //else
                //{
                //    return _resultService.ErrorMessage(Constants.RecordNotFound);
                //}

                //var invoiceDetailsContextList = _emamiContext.InvoiceDetails.AsNoTracking()
                //    .Join(_emamiContext.SalesRegister.AsNoTracking(), invd => invd.InvoiceId, sr => sr.InvoiceId, (invd, sr) => new { InvoiceDetails = invd, SalesRegister = sr })
                //    .Where(_ => _.InvoiceDetails.Invoice != null && DbFunctions.TruncateTime(_.InvoiceDetails.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                //  DbFunctions.TruncateTime(_.InvoiceDetails.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && _.InvoiceDetails.Invoice.UserId == inputDto.LoginUserId);

                var invoiceDetailsContextList = new List<SalesRegisterDataDto>();
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    var sqlQuery = @"select 
                                sku.PackGroupId as Id,
                                u.Id as UserId,
                                s.QuantityMT
                                from SalesRegisters s with(NOLOCK)
                                join Users u with(NOLOCK) on u.Code=s.CustomerCode
                                join Skus sku with(NOLOCK) on s.MaterialCode=sku.SkuCode and s.SalesOrganizationId=sku.SalesOrganizationId
                                and s.DistributionChannelId=sku.DistributionChannelId and s.DivisionId=sku.DivisionId
                                where
                                u.Id=@UserId
                                and Cast(s.InvoiceDate as date)>=Cast(@StartDate as date)
                                and Cast(s.InvoiceDate as date)<=Cast(@EndDate as date)";
                    var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    invoiceDetailsContextList = conn.Query<SalesRegisterDataDto>(sqlQuery, new
                    {
                        UserId = inputDto.LoginUserId,
                        StartDate=inputDto.FromDate,
                        EndDate=inputDto.ToDate
                    }).ToList();

                }
                //var invoiceDetailsContextList = _emamiContext.SalesRegister.AsNoTracking()
                //    .Join(_emamiContext.Users.AsNoTracking(), sr => sr.CustomerCode, us => us.Code, (sr, us) => new { SalesRegister = sr, User = us })
                //    .Join(_emamiContext.Skus.AsNoTracking(), i => i.SalesRegister.MaterialCode, s => s.SkuCode, (i, s) => new { SalesRegister = i.SalesRegister, User = i.User, Sku = s })
                //    .Where(_ => DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                //  DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && _.User.Id == userContext.Id
                //  //&& _.User.DivisionId == userContext.DivisionId && _.Sku.DivisionId == _.User.DivisionId
                //  && _.SalesRegister.SalesOrganizationId == _.Sku.SalesOrganizationId && _.SalesRegister.DistributionChannelId == _.Sku.DistributionChannelId
                //        && _.SalesRegister.DivisionId == _.Sku.DivisionId
                //  ).Select(s => new
                //  {
                //      UserId = s.User.Id,
                //      QuantityMT = s.SalesRegister.QuantityMT,
                //      PackGroupId = s.Sku.PackGroupId
                //  }).ToList();
                var usersalesTrgt = _emamiContext.UserCustomerSalesTarget.AsNoTracking()
                    .Where(_ => _.AssignedToId == inputDto.LoginUserId && _.MonthId<=inputDto.ToDate.Month && _.MonthId>=inputDto.FromDate.Month);


                if (invoiceDetailsContextList != null && invoiceDetailsContextList.Any())
                {

                    invoiceList.DealerId = inputDto.LoginUserId;

                    invoiceList.Dealer = userContext.Name;
                    var cityContext = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == userContext.CityId);
                    invoiceList.TownName = cityContext !=null ? cityContext.CityName : string.Empty;

                    if (inputDto.PackGroupId > 0)
                    {
                        var bulkPackContextList = invoiceDetailsContextList
                            .Where(_ => _.Id == inputDto.PackGroupId && _.UserId == userContext.Id);
                        if (bulkPackContextList != null && bulkPackContextList.Any())
                        {
                            invoiceList.Achievement = bulkPackContextList.Sum(_ => _.QuantityMT);
                        }
                        if (usersalesTrgt.Any())
                        {
                            invoiceList.Target = usersalesTrgt.Sum(_ => _.Target);
                        }

                    }
                    else
                    {
                        var bulkPackContextList = invoiceDetailsContextList.Where(_ => _.UserId == userContext.Id);
                        if (bulkPackContextList != null && bulkPackContextList.Any())
                        {
                            invoiceList.Achievement = bulkPackContextList.Sum(_ => _.QuantityMT);
                        }

                        if (usersalesTrgt.Any())
                        {
                            invoiceList.Target = usersalesTrgt.Sum(_ => _.Target);
                        }
                    }
                    return _resultService.SuccessObject(invoiceList);
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
        public ResultDto InvoiceDetailsByDealers(IdInputDto inputDto)
        {
            _methodName = "DashboardSalesDetailsByDealers";
            var invoiceDetailsOutputDto = new InvoiceDetailsOutputDto();
            if (inputDto == null)
            {
                return NotFoundResult();
            }
            try
            {
                var invoiceContext = _emamiContext.SalesRegister.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.Id);
                if (invoiceContext != null)
                {
                    invoiceDetailsOutputDto.InvoiceId = invoiceContext.Id;
                    invoiceDetailsOutputDto.InvoiceNumber = invoiceContext.InvoiceNumber;
                    invoiceDetailsOutputDto.InvoiceDate = invoiceContext.InvoiceDate;
                    invoiceDetailsOutputDto.TotalInvoiceValue = Convert.ToDecimal(invoiceContext.TotalAmount);
                    //var invoicedetailContext = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.InvoiceId == inputDto.Id).ToList();
                    //if (invoicedetailContext != null)
                    //{
                    //    decimal TotalInvoiceQuantity = 0;
                    //    foreach (var item in invoicedetailContext)
                    //    {
                    //        var InvoiceSKUDetails = new InvoiceSKUDetailsOutputDto()
                    //        {
                    //            OilTypeId = item.OilTypeId,
                    //            SkuId = item.SkuId,
                    //            Quantity = item.ActualBilledQuantity,
                    //            //QuantityInCase = _resultService.ConvertCasetoMetricTon(item.ActualBilledQuantity, item.SkuId),
                    //            QuantityInCase = item.QuantityInCase,
                    //            QunatityPrice = item.SKUInvoiceTax,
                    //            OilType = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == item.OilTypeId).Name,
                    //            sku = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == item.SkuId).SkuName,
                    //        };
                    //        TotalInvoiceQuantity = TotalInvoiceQuantity + item.ActualBilledQuantity;
                    //        invoiceDetailsOutputDto.InvoiceSKUDetails.Add(InvoiceSKUDetails);
                    //    }
                    //    invoiceDetailsOutputDto.InvoiceQuantity = TotalInvoiceQuantity;
                    //}
                    var invoicedetailContext = _emamiContext.SalesRegister.AsNoTracking().Where(_ => _.Id == inputDto.Id).ToList();
                    var skuIds = invoicedetailContext.Select(_ => _.SkuId).ToList();
                    var skulist = _emamiContext.Skus.AsNoTracking().Where(_ => skuIds.Contains(_.Id)).ToList();
                    if (invoicedetailContext != null)
                    {
                        decimal TotalInvoiceQuantity = 0;
                        foreach (var item in invoicedetailContext)
                        {
                            var InvoiceSKUDetails = new InvoiceSKUDetailsOutputDto()
                            {
                                OilTypeId = 0,
                                SkuId = 0,
                                Quantity = item.QuantityMT,
                                //QuantityInCase = _resultService.ConvertCasetoMetricTon(item.ActualBilledQuantity, item.SkuId),
                                QuantityInCase = item.QuantityCase,
                                QunatityPrice = Convert.ToDecimal(item.TotalAmount),
                                OilType = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == item.SkuId).Name,
                                sku = skulist.FirstOrDefault(_ => _.Id == item.SkuId).SkuCode,
                            };
                            TotalInvoiceQuantity = TotalInvoiceQuantity + item.QuantityMT;
                            invoiceDetailsOutputDto.InvoiceSKUDetails.Add(InvoiceSKUDetails);
                        }
                        invoiceDetailsOutputDto.InvoiceQuantity = TotalInvoiceQuantity;
                    }
                }
                return SucessResult(invoiceDetailsOutputDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }
        public ResultDto DueForTomorrowList(LoginUserIdDto inputDto)
        {
            _methodName = "DashboardSalesDetailsByDealers";
            var dashboardDetailsForPendingAndOverDueOutputDto = new DashboardDetailsForPendingAndOverDueOutputDto();
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
            if (inputDto == null)
            {
                return NotFoundResult();
            }
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

                var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (dealerContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }


                //var invoiceContext = _emamiContext.Invoices.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId && _.PaymentStatus == false).ToList();


                var overduePaymentContext = _emamiContext.OverduePayment.AsNoTracking().Where(_ =>  _.UserId == inputDto.LoginUserId);

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
                            var overAndPendingDueWithDealerDetails = new OverAndPendingDueWithDealerDetails
                            {
                                DealerCode = dealerContext != null ? dealerContext.Code : string.Empty,
                                DealerName = dealerContext != null ? dealerContext.Name : string.Empty,
                                OverDue = totalBookedValueOverDue
                            };

                            dashboardDetailsForPendingAndOverDueOutputDto.OverAndPendingDueWithDealerDetails.Add(overAndPendingDueWithDealerDetails);
                        }
                        
                        //if (totalBookedValueOverDue != null)
                        //{

                            //foreach (var item in userCreditMasterForOverDueList)
                            //{
                            //    var userContext = UserContextData.FirstOrDefault(_ => _.Id == item.UserId);
                                
                            //}
                        //}
                        //dashboardDetailsForPendingAndOverDueOutputDto.OverAndPendingDueWithDealerDetails.AddRange(userCreditMasterContext);
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

                            var overAndPendingDueWithDealerDetails = new OverAndPendingDueWithDealerDetails
                            {
                                DealerCode = dealerContext != null ? dealerContext.Code : string.Empty,
                                DealerName = dealerContext != null ? dealerContext.Name : string.Empty,
                                PendingDue = totalBookedValuePendingDue
                            };

                            dashboardDetailsForPendingAndOverDueOutputDto.OverAndPendingDueWithDealerDetails.Add(overAndPendingDueWithDealerDetails);
                        }
                        //var userCreditMasterContext = new List<OverAndPendingDueWithDealerDetails>();
                        //var userCreditMasterForTommorrowDueList = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) == DbFunctions.TruncateTime(tomDate)).ToList();
                        //if (userCreditMasterForTommorrowDueList != null)
                        //{
                            //foreach (var item in userCreditMasterForTommorrowDueList)
                            //{
                            //    var userContext = UserContextData.FirstOrDefault(_ => _.Id == item.UserId);
                               
                            //}
                        //}
                      //  dashboardDetailsForPendingAndOverDueOutputDto.OverAndPendingDueWithDealerDetails.AddRange(userCreditMasterContext);
                    }
                    else
                    {
                        var totalBookedValueOverDue = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) <= DbFunctions.TruncateTime(currentDate)).DefaultIfEmpty().Sum(_ => (decimal?)_.Balance).GetValueOrDefault();
                        if (totalBookedValueOverDue != null)
                        {
                            dashboardDetailsForPendingAndOverDueOutputDto.TotalBookedValueOverDue = totalBookedValueOverDue;
                            var overAndPendingDueWithDealerDetails = new OverAndPendingDueWithDealerDetails
                            {
                                DealerCode = dealerContext != null ? dealerContext.Code : string.Empty,
                                DealerName = dealerContext != null ? dealerContext.Name : string.Empty,
                                OverDue = totalBookedValueOverDue
                            };

                            dashboardDetailsForPendingAndOverDueOutputDto.OverAndPendingDueWithDealerDetails.Add(overAndPendingDueWithDealerDetails);
                        }
                        var totalBookedValuePendingDue = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) == DbFunctions.TruncateTime(tomDate)).DefaultIfEmpty().Sum(_ => (decimal?)_.Balance).GetValueOrDefault();
                        if (totalBookedValuePendingDue != null)
                        {
                            dashboardDetailsForPendingAndOverDueOutputDto.TotalBookedValuePendingDue = totalBookedValuePendingDue;
                            var overAndPendingDueWithDealerDetails = new OverAndPendingDueWithDealerDetails
                            {
                                DealerCode = dealerContext != null ? dealerContext.Code : string.Empty,
                                DealerName = dealerContext != null ? dealerContext.Name : string.Empty,
                                PendingDue = totalBookedValuePendingDue
                            };
                            dashboardDetailsForPendingAndOverDueOutputDto.OverAndPendingDueWithDealerDetails.Add(overAndPendingDueWithDealerDetails);
                        }

                        //var userCreditMasterForOverDueContext = new List<OverAndPendingDueWithDealerDetails>();
                        //var userCreditMasterForOverDueList = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) <= DbFunctions.TruncateTime(currentDate)).ToList();
                        //if (userCreditMasterForOverDueList != null)
                        //{

                        //    foreach (var item in userCreditMasterForOverDueList)
                        //    {
                        //        var userContext = UserContextData.FirstOrDefault(_ => _.Id == item.UserId);
                               
                            //}
                       // }
                        //dashboardDetailsForPendingAndOverDueOutputDto.OverAndPendingDueWithDealerDetails.AddRange(userCreditMasterForOverDueContext);
                        //var userCreditMasterForOverDueContext = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) < DbFunctions.TruncateTime(currentDate)).Select(s => new OverAndPendingDueWithDealerDetails()
                        //{
                        //    DealerCode = s.UserCode,
                        //    DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == s.UserId) != null ? _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == s.UserId).Name : string.Empty,
                        //    OverDue = s.Balance
                        //}).ToList();
                       // var userCreditMasterForTommorrowDueContext = new List<OverAndPendingDueWithDealerDetails>();
                        //var userCreditMasterForTommorrowDueList = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) == DbFunctions.TruncateTime(tomDate)).ToList();
                        //if (userCreditMasterForTommorrowDueList != null)
                        //{
                        //    foreach (var item in userCreditMasterForTommorrowDueList)
                        //    {
                        //        var userContext = UserContextData.FirstOrDefault(_ => _.Id == item.UserId);
                                

                        //        userCreditMasterForOverDueContext.Add(overAndPendingDueWithDealerDetails);
                        //    }
                        //}
                        //dashboardDetailsForPendingAndOverDueOutputDto.OverAndPendingDueWithDealerDetails.AddRange(userCreditMasterForTommorrowDueContext);
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
                    return NotFoundResult();
                }
                //decimal TotalQuantity = 0;
                //decimal TotalInvoiceValue = 0;
                //foreach (var detail in invoiceContext)
                //{
                //    var invoicedetailsContext = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.InvoiceId == detail.Id).ToList();
                //    var DashboardSaudaDetails = new DashboardSalesDetailsOutputDto()
                //    {
                //        InvoiceId = detail.Id,
                //        InvoiceNumber = detail.BillingDocument,
                //        InvoiceDate = detail.InvoiceDate != null ? detail.InvoiceDate.Date : detail.InvoiceDate,
                //        TotalQuantity = invoicedetailsContext.Sum(_ => _.ActualBilledQuantity)
                //    };
                //    var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                //    if (detail.InvoiceDueDate != null && detail.InvoiceDueDate.Value.Date < currentDate.Date)
                //    {
                //        DashboardSaudaDetails.StatusId = (int)DTO.Enums.DueStatus.OverDue;
                //        DashboardSaudaDetails.StatusName = UtilityHelper.GetEnumDescription(DTO.Enums.DueStatus.OverDue);
                //        DashboardSaudaDetails.InvoiceValue = userCreditMasterContext.Sum(due => due.Overdue);
                //        TotalQuantity = TotalQuantity + invoicedetailsContext.Sum(_ => _.ActualBilledQuantity);
                //        TotalInvoiceValue = TotalInvoiceValue + userCreditMasterContext.Sum(due => due.Overdue);
                //        objdetailoutput.DashboardSalesDetails.Add(DashboardSaudaDetails);
                //    }
                //    else
                //    {
                //        if (detail.InvoiceDueDate != null && (detail.InvoiceDueDate.Value.Date == currentDate.Date || detail.InvoiceDueDate.Value.Date == currentDate.Date.AddDays(1)))
                //        {
                //            DashboardSaudaDetails.StatusId = (int)DTO.Enums.DueStatus.PendingDue;
                //            DashboardSaudaDetails.StatusName = UtilityHelper.GetEnumDescription(DTO.Enums.DueStatus.PendingDue);
                //            DashboardSaudaDetails.InvoiceValue = userCreditMasterContext.Sum(due => due.TomorrowsDue);
                //            TotalQuantity = TotalQuantity + invoicedetailsContext.Sum(_ => _.ActualBilledQuantity);
                //            TotalInvoiceValue = TotalInvoiceValue + userCreditMasterContext.Sum(due => due.TomorrowsDue);
                //            objdetailoutput.DashboardSalesDetails.Add(DashboardSaudaDetails);
                //        }
                //    }
                //}
                //objdetailoutput.DealerId = dealerContext.Id;
                //objdetailoutput.Dealer = dealerContext.Name;
                //objdetailoutput.TotalQuantity = TotalQuantity;
                //objdetailoutput.TotalBookedInvoiceValue = TotalInvoiceValue;

                return SucessResult(dashboardDetailsForPendingAndOverDueOutputDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }


        /// Method to create sauda from special rate
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        public ResultDto SaudaCreationFromSpecialRate(SaudaInputDto inputDto)
        {
            _methodName = "SaudaCreationFromSpecialRate";
            var resultDto = new ResultDto();
            try
            {

                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                var dealerContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == inputDto.DealerId);

                if (dealerContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                if (inputDto.SaudaOrders == null || !inputDto.SaudaOrders.Any())
                {
                    return _resultService.ErrorMessage(Constants.SaudaOrderIsEmpty);
                }


                var statusId = (int)DTO.Enums.Status.Pending;


                long DealerTypeId = 0;
                string IncotermsType = string.Empty;
                if (inputDto.DealerTypeId == 1)
                {
                    DealerTypeId = (int)DTO.Enums.DealerType.Direct;
                }
                else
                {
                    DealerTypeId = (int)DTO.Enums.DealerType.Broker;
                }




                var saudaContext = new Sauda
                {

                    BiddingDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    UserId = inputDto.DealerId,


                    SaudaBookingTypeId = inputDto.SaudaBookingTypeId,

                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                    IsSAPDataSync = false,
                    IsSAPDataSyncApproval = false,

                };

                _emamiContext.Sauda.Add(saudaContext);
                _emamiContext.SaveChanges();

                if (inputDto.SaudaOrders != null && inputDto.SaudaOrders.Any())
                {
                    int i = 0;
                    foreach (var item in inputDto.SaudaOrders)
                    {
                        var IncotermContext = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == item.IncotermsId).Name;
                        IncotermsType = IncotermContext.ToLower().Contains("for") ? "For" : "Ex";

                        //var pricingContext = _emamiContext.Pricing.AsNoTracking().FirstOrDefault(_ => _.Id == item.PricingId);
                        //if (pricingContext != null)
                        //{
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
                            BidQuantity = _resultService.ConvertCasetoMetricTon(item.BidQuantity, item.SkuId),
                            BidQuantityCase = item.BidQuantity,
                            QuotedPrice = item.QuotedPrice,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),

                            //BiddingwindowId = item.BiddingwindowId,
                            PricingId = item.PricingId,
                            //DealerTypeId = DealerTypeId,
                            Incoterms1 = IncotermsType,
                            PlantId = item.PlantId,
                            DealerLocationId = item.DealerLocationId,
                            ValidFromDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            ValidToDate = DateHelper.UtcToIndia(DateTime.UtcNow).AddDays(Convert.ToDouble(dealerContext.SaudaValidityPeriod > 0 ? dealerContext.SaudaValidityPeriod : Config.DefaultSaudaValidity)),
                            StatusId = statusId,
                           // SaudaStatusId = (int)DTO.Enums.SaudaStatus.NotReleased,
                            Incoterms2 = item.IncotermsId,
                            BrokerId = inputDto.BrokerId,
                           // CustomerPONumber = dealerContext.Code + DateHelper.UtcToIndia(DateTime.UtcNow),
                            IsSAPDataSync = false,
                            IsSAPDataSyncApproval = false,
                            QuotedPriceBeforeSAPDiscount = item.BidQuantity == 0 ? 0m : item.BidPrice / item.BidQuantity
                        };
                        _emamiContext.SaudaOrders.Add(saudaOrder);
                        _emamiContext.SaveChanges();
                        //}
                    }
                }

                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                var CreatedByUser = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaContext.CreatedBy);
                var User = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaContext.UserId);
                var emailTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationEmail);
                if (_resultService.IsEmail())
                {
                    List<string> toUser = new List<string>();
                    toUser.Add(CreatedByUser.Email);
                    toUser.Add(User.Email);
                    var emailSubject = Constants.SaudaCreationSubject;
                    var fromEmail = Constants.FromEmail;
                    var plainText = string.Empty;
                    if (emailTemplate != null)
                    {
                        var replaceEmailTemplate = emailTemplate.PlainTemplate.Replace(Constants.Name, User.Name).Replace(Constants.RequestNumber, saudaContext.Id.ToString());
                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, replaceEmailTemplate);
                        amazonNotificationService.SendEmail(toUser, emailSubject, plainText, htmlTemplate, true);
                    }
                }
                if (_resultService.IsSMS())
                {
                    var smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaCreationSMS);
                    if (smsTemplate != null)
                    {
                        var replaceSmsTemplate = smsTemplate.PlainTemplate.Replace(Constants.Name, User.Name).Replace(Constants.RequestNumber, saudaContext.Id.ToString());
                        amazonNotificationService.SendMessage(replaceSmsTemplate, CreatedByUser.MobileNumber);
                        amazonNotificationService.SendMessage(replaceSmsTemplate, User.MobileNumber);
                    }
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

        public ResultDto GetTickerListForToday()
        {
            _methodName = "GetTickerListForToday";
            try
            {
                var todayTickerListDto = new List<TodayTickerListDto>();
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                IQueryable<Ticker> tickerContextList = _emamiContext.Ticker.AsNoTracking().Where(_ => DbFunctions.TruncateTime(_.CreatedDate) == DbFunctions.TruncateTime(currentDate) && _.IsActive);
                if (tickerContextList != null && tickerContextList.Any())
                {
                    todayTickerListDto = tickerContextList.Select(_ => new TodayTickerListDto
                    {
                        Content = _.Content,
                        FromHours = _.FromHours,
                        ToHours = _.ToHours,
                    }).ToList();
                }
                if (todayTickerListDto != null && todayTickerListDto.Any())
                {
                    return _resultService.SuccessObject(todayTickerListDto);
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

        public ResultDto GetCreditNote(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetCreditNote";
            try
            {
                var creditNote = new CreditNoteDto();
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
                var creditNoteContext = _emamiContext.CreditNotes.AsNoTracking().FirstOrDefault(_ => _.UserId == loginUserIdDto.LoginUserId && _.IsActive);
                if (creditNoteContext != null)
                {
                    creditNote.CreditNoteDate = creditNoteContext.CreditNoteDate;
                    creditNote.Number = creditNoteContext.Number;
                    creditNote.Amount = creditNoteContext.Amount;
                    return _resultService.SuccessObject(creditNote);
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

        public ResultDto GetAccountStatement(LoginUserIdDto loginUserIdDto)
        {
            _methodName = "GetAccountStatement";
            try
            {
                var accountStatement = new AccountStatementDto();
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
                var accountStatementContext = _emamiContext.AccountStatements.AsNoTracking().FirstOrDefault(_ => _.UserId == loginUserIdDto.LoginUserId && _.IsActive);
                if (accountStatementContext != null)
                {
                    accountStatement.StatementDate = accountStatementContext.StatementDate;
                    accountStatement.DurationDate = accountStatementContext.DurationDate;
                    accountStatement.ClosingBalance = accountStatementContext.ClosingBalance;
                    accountStatement.DepositAmount = accountStatementContext.DepositAmount;
                    accountStatement.BankGuarantee = accountStatementContext.BankGuarantee;
                    return _resultService.SuccessObject(accountStatement);
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

        /// Method to get daily rate
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        public ResultDto GetDailyRate(DailyRateInputDto inputDto)
        {
            _methodName = "GetDailyRate";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (inputDto.OilTypeId == 0)
                {
                    return _resultService.ErrorMessage(Constants.OilTypeMissing);
                }
                var userContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == inputDto.LoginUserId);

                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var pricingContext = new List<Pricing>();
                var outputgroubyList = new List<Pricing>();
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                if (userContext.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess)
                {
                    outputgroubyList = _emamiContext.Pricing.AsNoTracking().Where(_ =>
                                         _.OilTypeId == inputDto.OilTypeId).ToList();
                }

                if (outputgroubyList == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                var RecentPricings = from e in outputgroubyList
                                     group e by new
                                     {
                                         e.SkuId,
                                     } into dptgrp
                                     let topsal = dptgrp.Max(x => x.Id)
                                     select new Pricing
                                     {
                                         SkuId = dptgrp.Key.SkuId,
                                         Id = dptgrp.First(y => y.Id == topsal).Id,
                                         PlantId = dptgrp.First(y => y.Id == topsal).PlantId,
                                     };

                pricingContext = RecentPricings.ToList();
                var outputList = new List<DailyRateOutputDto>();
                foreach (var pricing in pricingContext)
                {
                    var finalPrice = pricing.Price;
                    if (finalPrice > 0)
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
                var FinaloutputList = new List<DailyRateOutputDto>();
                if (outputList != null && outputList.Any())
                {
                    FinaloutputList = outputList
                                        .GroupBy(p => new { p.SkuId, p.SkuName, p.FinalPrice })
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


        /// Method to get daily rate
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        public ResultDto GetDailyRateWeb(PricePublistInputDataDto inputDto)
        {
            _methodName = "GetDailyRate";
            var resultDto = new ResultDto();
            var priceGenerateList = new List<FinalPriceGenerateListDto>();
            try
            {
                var todayPricing = _emamiContext.TodayPricing.AsNoTracking();
                var salesOrganization = _emamiContext.SalesOrganization.AsNoTracking();
                var distributionChannel = _emamiContext.DistributionChannel.AsNoTracking();
                var divisions = _emamiContext.Divisions.AsNoTracking();
                var depots = _emamiContext.Depots.AsNoTracking();
                var skus = _emamiContext.Skus.AsNoTracking();
                var oilTypes = _emamiContext.OilTypes.AsNoTracking();
                var oilPackingTypes = _emamiContext.OilPackingTypes.AsNoTracking();

                if (inputDto.StartDate != null && inputDto.EndDate != null)
                {
                    //todayPricing = todayPricing.Where(_ => _.ValidTo <= inputDto.EndDate && _.ValidFrom >= inputDto.StartDate);
                    todayPricing = _emamiContext.TodayPricing.AsNoTracking().Where(_ => DbFunctions.TruncateTime(inputDto.StartDate) >= DbFunctions.TruncateTime(_.ValidFrom) 
                    && DbFunctions.TruncateTime(inputDto.StartDate) <= DbFunctions.TruncateTime(_.ValidTo)
                        && DbFunctions.TruncateTime(inputDto.EndDate) <= DbFunctions.TruncateTime(_.ValidTo) 
                        && DbFunctions.TruncateTime(inputDto.EndDate) >= DbFunctions.TruncateTime(_.ValidFrom));
                }
                if (inputDto.DivisionId != 0)
                {
                    todayPricing = todayPricing.Where(_ => _.DivisionId == inputDto.DivisionId && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId);
                }
                if (inputDto.OilTypeId != 0)
                {
                    todayPricing = todayPricing.Where(_ => _.OilTypeId == inputDto.OilTypeId);
                }
                if (inputDto.PlantId != 0)
                {
                    todayPricing = todayPricing.Where(_ => _.PlantId == inputDto.PlantId);
                }
                              

                if (todayPricing != null && todayPricing.Any())
                {
                    priceGenerateList = todayPricing.Select(_ => new FinalPriceGenerateListDto()
                    {
                        SAPPricingCode = _.SAPPricingCode,
                        DivisionId = _.DivisionId,
                        SalesOrganizationId = _.SalesOrganizationId,
                        DistributionChannelId = _.DistributionChannelId,
                        OilTypeId = _.OilTypeId,
                        PlantId = _.PlantId,
                        Price = _.Price,
                        ValidFrom = _.ValidFrom,
                        ValidTo = _.ValidTo,
                        CreatedDate = _.CreatedDate,
                        SalesOrganizationName = salesOrganization.Where(s => s.Id == _.SalesOrganizationId).FirstOrDefault().Name,
                        DistributionChannelName = distributionChannel.Where(d => d.Id == _.DistributionChannelId).FirstOrDefault().Name,
                        DivisionName = divisions.Where(d => d.Id == _.DivisionId).FirstOrDefault().Name,
                        PlantName = depots.Where(d => d.Id == _.PlantId && d.IsPlant).FirstOrDefault().Name,
                        PlantCode = depots.Where(d => d.Id == _.PlantId && d.IsPlant).FirstOrDefault().Code,
                        SkuCode = skus.Where(d => d.Id == _.SkuId).FirstOrDefault().SkuCode,
                        SkuName = skus.Where(d => d.Id == _.SkuId).FirstOrDefault().SkuName,
                        //OilTypeCode = oilTypes.Where(d => d.Id == _.OilTypeId).FirstOrDefault().SAPCode,
                        OilTypeName = _.OilTypeId > 0 ? oilTypes.Where(d => d.Id == _.OilTypeId).FirstOrDefault().Name +"-"+ salesOrganization.Where(s => s.Id == _.SalesOrganizationId).FirstOrDefault().Code+"/"+ distributionChannel.Where(d => d.Id == _.DistributionChannelId).FirstOrDefault().Code+"/"+ divisions.Where(d => d.Id == _.DivisionId).FirstOrDefault().Code : string.Empty,
                        OilPackingType = oilPackingTypes.Where(d => d.Id == _.OilPackingTypeId).FirstOrDefault().Name,

                    }).ToList();

                }
                else
                {
                    return _resultService.SuccessMessageWitObject(priceGenerateList, "List is Empty");
                }
                var datasourceResult = priceGenerateList.ToDataSourceResult(inputDto.DataSourceRequest);

                return _resultService.SuccessMessageWitObject(datasourceResult, Constants.SuccessMessage);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }

        }

        public ResultDto GetDailyRateNew(DailyRateInputDto inputDto)
        {
            _methodName = "GetDailyRate";
            var resultDto = new ResultDto();
            try
            {

                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.PlantId == 0)
                {
                    return _resultService.ErrorMessage(Constants.PlantMissing);
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
                var userContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var outputgroubyList = new List<TodayPricing>();
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                outputgroubyList = _emamiContext.TodayPricing.AsNoTracking().Join(_emamiContext.Skus.AsNoTracking(), t => t.SkuId , s => s.Id , (t,s) => new { t ,s})
                    .Where(_ => _.t.PlantId == inputDto.PlantId && _.t.SkuId != 0  && (inputDto.OilTypeId > 0 ? _.t.OilTypeId == inputDto.OilTypeId : _.t.OilTypeId > 0) && (DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.t.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.t.ValidTo)) && _.t.SalesOrganizationId == inputDto.SalesOrganizationId && _.t.DistributionChannelId == inputDto.DistributionChannelId && _.t.DivisionId == inputDto.DivisionId && _.s.IsActive).Select(p => p.t).OrderByDescending(_ => _.Id).ToList();

                if (outputgroubyList == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                var RecentPricings = from e in outputgroubyList
                                     group e by new
                                     {
                                         e.SkuId
                                     } into dptgrp
                                     let topsal = dptgrp.Max(x => x.Id)
                                     select new Pricing
                                     {
                                         SkuId = dptgrp.Key.SkuId,
                                         Id = dptgrp.FirstOrDefault(y => y.Id == topsal).Id,
                                         Price = dptgrp.FirstOrDefault(y => y.Id == topsal).Price,
                                     };

                var finalOutputDto = new List<Pricing>();
                var SkuDistinct = from a in RecentPricings.ToList()
                                  group a by new
                                  {
                                      a.SkuId
                                  } into grp
                                  let topsku = grp.Max(X => X.Id)
                                  select new Pricing
                                  {
                                      SkuId = grp.Key.SkuId,
                                      Price = grp.FirstOrDefault(y => y.Id == topsku).Price,
                                  };

                foreach (var item in SkuDistinct.ToList())
                {
                    var RecentPricingContext = (from a in RecentPricings.ToList()
                                                where a.SkuId == item.SkuId
                                                select a).ToList();

                    if (RecentPricingContext != null && RecentPricingContext.Any())
                    {
                        finalOutputDto.AddRange(RecentPricingContext.Where(_ => _.SkuId == item.SkuId
                                                                            ).ToList());
                    }
                }

                var pricingContext = finalOutputDto.ToList();


                var outputList = new List<DailyRateOutputDto>();
                foreach (var pricing in pricingContext)
                {
                    var finalPrice = pricing.Price;
                    if (finalPrice > 0)
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


        public ResultDto DealerPlantDepotDetails(LoginUserIdDto inputDto)
        {
            _methodName = "DealerPlantDepotDetails";
            var PlantDepotList = new List<DepotDto>();

            if (inputDto == null)
            {
                return NotFoundResult();
            }
            try
            {
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
                    var plantContext = (from plantdepot in _emamiContext.PlantDepotMapping.AsNoTracking()
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

                    plant.Depotlist = plantContext;

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

        public ResultDto GetDealerStatistics(SaudaFilterDto saudaFilterDto)
        {
            _methodName = "GetDealerStatistics";
            try
            {
                var DealerStatistics = new UserStatisticsOutputDto();
                if (saudaFilterDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                if (saudaFilterDto.UserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaFilterDto.UserId && _.IsActive);
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
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                #region New Code
                var outStandingContextList = new List<PendingContractStatistics>();
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {
                    var sqlQuery = @"select pc.SaudaQuantity as PendingQuantityInMT,pc.ContractValidTo 
                        from PendingContracts pc with(NOLOCK)
						join Users u on pc.UserId=u.Id
                        join Skus sku on  pc.MaterialCode=sku.SkuCode
                        where 
                        pc.SalesOrgId=sku.SalesOrganizationId and pc.DistChnlId=sku.DistributionChannelId and pc.DivisionId=sku.DivisionId
                        and u.Id= @UserId";

                    outStandingContextList = conn.Query<PendingContractStatistics>(sqlQuery, new
                    {
                        UserId = saudaFilterDto.UserId
                    }).ToList();

                }
                if (outStandingContextList != null && outStandingContextList.Any())
                {
                    DealerStatistics.PendingSaudaQuantity = outStandingContextList.Sum(_ => _.PendingQuantityInMT);
                }
                if (outStandingContextList != null && outStandingContextList.Any())
                {
                    var ExpiredContextList = outStandingContextList.Where(_ => _.ContractValidTo.Date < currentDate.Date).ToList();
                    var NearExpiredContextList = outStandingContextList.Where(_ => ( _.ContractValidTo.Date-currentDate.Date).Days < 5 &&  (_.ContractValidTo.Date-currentDate.Date).Days >= 1).ToList();
                    
                    if (ExpiredContextList != null && ExpiredContextList.Any())
                    {
                        DealerStatistics.AboveOutstandingSaudaQuantity = ExpiredContextList.Sum(_ => _.PendingQuantityInMT);
                    }
                    if (NearExpiredContextList != null && NearExpiredContextList.Any())
                    {
                        DealerStatistics.BelowOutstandingSaudaQuantity = NearExpiredContextList.Sum(_ => _.PendingQuantityInMT);
                    }
                }

                #endregion

                #region Old Code
                //var PendingContractContext = _emamiContext.PendingContracts.AsNoTracking()
                //                            .Join(_emamiContext.SaudaOrders.AsNoTracking(), pc => pc.SaudaOrderId, so => so.Id, (pc, so) => new { pc, so })
                //                            .Join(_emamiContext.Sauda.AsNoTracking(), so => so.so.SaudaId, s => s.Id, (so, s) => new { so.pc, so, s })
                //                            .Where(_ => _.pc != null && _.s.UserId == saudaFilterDto.UserId).Select(_ => new { _.pc }).ToList();

                //var PendingContractContext = _emamiContext.PendingContracts.AsNoTracking()
                //                            .Join(_emamiContext.Users.AsNoTracking(), pc => pc.UserId, us => us.Id, (sr, us) => new { PendingContract = sr, User = us })
                //                            .Join(_emamiContext.Skus.AsNoTracking(), pc => pc.PendingContract.MaterialCode, sku => sku.SkuCode, (pc, sku) => new { PendingContract = pc.PendingContract, User = pc.User, Sku = sku })
                //                            //.Join(_emamiContext.Sauda.AsNoTracking(), pc => pc.PendingContract.SaudaNumber, sauda => sauda.SaudaNumber, (pc, sauda) => new { PendingContract = pc.PendingContract, User = pc.User, Sku = pc.Sku, Sauda = sauda })
                //                            .Where(_ => _.PendingContract != null && _.User.Id == saudaFilterDto.UserId
                //                            //&& _.User.DivisionId == userContext.DivisionId && _.Sku.DivisionId == _.User.DivisionId
                //                            //&& DbFunctions.TruncateTime(_.PendingContract.ContractValidTo) >= DbFunctions.TruncateTime(currentDate)
                //                            && _.PendingContract.SalesOrgId == _.Sku.SalesOrganizationId && _.PendingContract.DistChnlId == _.Sku.DistributionChannelId
                //                            && _.PendingContract.DivisionId == _.Sku.DivisionId
                //                            ).Select(_ => new { _.PendingContract }).ToList();

                //if (outStandingContextList != null && outStandingContextList.Any())
                //{
                //    DealerStatistics.PendingSaudaQuantity = outStandingContextList.Sum(_ => _.PendingQuantityInMT);
                //}

                //List<Sauda> saudaContextList = _emamiContext.Sauda.AsNoTracking().Where(_ => _.UserId == saudaFilterDto.UserId
                // && DbFunctions.TruncateTime(_.BiddingDate) >= DbFunctions.TruncateTime(saudaFilterDto.FromDate)
                //&& DbFunctions.TruncateTime(_.BiddingDate) <= DbFunctions.TruncateTime(saudaFilterDto.ToDate)
                //).ToList();
                //IQueryable<SaudaOrder> saudaOrderContextList = null;
                //if (saudaContextList != null && saudaContextList.Any())
                //{
                //    List<long> saudaContextListIds = saudaContextList.Select(_ => _.Id).ToList();
                //    saudaOrderContextList = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => saudaContextListIds.Contains(_.SaudaId) && (_.StatusId == (int)DTO.Enums.Status.Approved
                //  || _.StatusId == (int)DTO.Enums.Status.Pending || _.StatusId == (int)DTO.Enums.Status.Completed));
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
                //    decimal ReturnedQuantity = 0;
                //    var ReturnInvoiceContext = _emamiContext.Invoices.AsNoTracking().Where(_ => _.UserId == saudaFilterDto.UserId && _.SalesDocumentType == "ZHCR");
                //    if (ReturnInvoiceContext != null && ReturnInvoiceContext.Any())
                //    {
                //        var ReturnedQtyContext = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => ReturnInvoiceContext.Any(a => a.Id == _.InvoiceId) && saudaOrderContextIds.Contains(_.SaudaOrderId)).ToList();
                //        if (ReturnedQtyContext != null && ReturnedQtyContext.Any())
                //        {
                //            ReturnedQuantity = ReturnedQtyContext.Sum(_ => _.ActualBilledQuantity);
                //        }
                //    }
                //    //Pending orders count
                //    DealerStatistics.PendingSaudaQuantity = saudaOrderContextList.Sum(_ => _.BidQuantity) - orderLiftMappingListContext.Sum(_ => _.LiftingQuantity) + ReturnedQuantity;
                //}
                //else
                //{
                //    DealerStatistics.PendingSaudaQuantity = saudaOrderContextList.Sum(_ => _.BidQuantity);
                //}
                //IQueryable<SaudaOrder> outStandingContextList = saudaOrderContextList.Where(_ => DbFunctions.TruncateTime(_.ValidToDate) < DbFunctions.TruncateTime(currentDate));
                //List<long> outStandingContextIds = outStandingContextList.Select(_ => _.Id).ToList();
                //var outStandingContextList = _emamiContext.PendingContracts.AsNoTracking()
                //                        .Join(_emamiContext.Users.AsNoTracking(), pc => pc.UserId, us => us.Id, (sr, us) => new { PendingContract = sr, User = us })
                //                        .Join(_emamiContext.Skus.AsNoTracking(), pc => pc.PendingContract.MaterialCode, sku => sku.SkuCode, (pc, sku) => new { PendingContract = pc.PendingContract, User = pc.User, Sku = sku })
                //                       // .Join(_emamiContext.Sauda.AsNoTracking(), pc => pc.PendingContract.SaudaNumber, sauda => sauda.SaudaNumber, (pc, sauda) => new { PendingContract = pc.PendingContract, User = pc.User, Sku = pc.Sku , Sauda = sauda })
                //                        .Where(_ => _.PendingContract != null && _.User.Id == saudaFilterDto.UserId
                //                        //&& _.User.DivisionId == userContext.DivisionId && _.Sku.DivisionId == _.User.DivisionId
                //                        && _.PendingContract.SalesOrgId == _.Sku.SalesOrganizationId && _.PendingContract.DistChnlId == _.Sku.DistributionChannelId
                //                            && _.PendingContract.DivisionId == _.Sku.DivisionId
                //                        ).Select(_ => new { _.PendingContract });

                //if (outStandingContextList != null && outStandingContextList.Any())
                //{
                //    var ExpiredContextList = outStandingContextList.Where(_ => DbFunctions.TruncateTime(_.ContractValidTo) < DbFunctions.TruncateTime(currentDate)).ToList();
                //    var NearExpiredContextList = outStandingContextList.Where(_ => DbFunctions.DiffDays(currentDate, _.ContractValidTo) < 5 && DbFunctions.DiffDays(currentDate, _.ContractValidTo) >= 1).ToList();
                //Expired
                //List<SaudaOrderLiftingRequestMapping> outStandingLiftMappingListContext = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => outStandingContextIds.Contains(_.SaudaOrderId)
                //    && _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected).ToList();
                //if (outStandingLiftMappingListContext != null && outStandingLiftMappingListContext.Any())
                //{
                //    DealerStatistics.AboveOutstandingSaudaQuantity = outStandingContextList.Sum(_ => _.BidQuantity) - outStandingLiftMappingListContext.Sum(_ => _.LiftingQuantity);
                //}
                //else
                //{
                //    DealerStatistics.AboveOutstandingSaudaQuantity = outStandingContextList.Sum(_ => _.BidQuantity);
                //}
                //    if (ExpiredContextList != null && ExpiredContextList.Any())
                //    {
                //        DealerStatistics.AboveOutstandingSaudaQuantity = ExpiredContextList.Sum(_ => _.PendingQuantityInMT);
                //    }
                //    if (NearExpiredContextList != null && NearExpiredContextList.Any())
                //    {
                //        DealerStatistics.BelowOutstandingSaudaQuantity = NearExpiredContextList.Sum(_ => _.PendingQuantityInMT);
                //    }
                //}
                //if (outStandingContextList != null && outStandingContextList.Any())
                //{
                //    //Near Expired
                //    //List<SaudaOrderLiftingRequestMapping> outStandingLiftMappingListContext = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => outStandingContextIds.Contains(_.SaudaOrderId)
                //    //    && _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected).ToList();
                //    //if (outStandingLiftMappingListContext != null && outStandingLiftMappingListContext.Any())
                //    //{
                //    //    DealerStatistics.BelowOutstandingSaudaQuantity = outStandingContextList.Sum(_ => _.BidQuantity) - outStandingLiftMappingListContext.Sum(_ => _.LiftingQuantity);
                //    //}
                //    //else
                //    //{
                //    //    DealerStatistics.BelowOutstandingSaudaQuantity = outStandingContextList.Sum(_ => _.BidQuantity);
                //    //}
                //    outStandingContextList = outStandingContextList.Where(_ => DbFunctions.DiffDays(currentDate, _.PendingContract.ContractValidTo) < 5 && DbFunctions.DiffDays(currentDate, _.PendingContract.ContractValidTo) >= 1).ToList();
                //}
                //}
                //var invoicesContext = _emamiContext.Invoices.AsNoTracking().Where(_ => _.UserId == saudaFilterDto.UserId);
                //if (invoicesContext != null && invoicesContext.Any())
                //{
                //    var dueForTomoinvoicesContext = invoicesContext.Where(_ => _.InvoiceDueDate != null && DbFunctions.TruncateTime(_.InvoiceDueDate) == DbFunctions.TruncateTime(DbFunctions.AddDays(currentDate, 1)));
                //    if (dueForTomoinvoicesContext != null && dueForTomoinvoicesContext.Any())
                //    {
                //        DealerStatistics.TotalDueForTomorrow = dueForTomoinvoicesContext.Sum(_ => _.NetValue);
                //    }
                //    var overDueinvoicesContext = invoicesContext.Where(_ => _.InvoiceDueDate != null && DbFunctions.TruncateTime(_.InvoiceDueDate) < DbFunctions.TruncateTime(currentDate));
                //    if (overDueinvoicesContext != null && overDueinvoicesContext.Any())
                //    {
                //        DealerStatistics.TotalOverDue = overDueinvoicesContext.Sum(_ => _.NetValue);
                //    }
                //}

                //var userCreditMasterContext = _emamiContext.UserCreditMaster.AsNoTracking().Where(_ => _.UserId == saudaFilterDto.UserId && _.Isactive);
                //if (userCreditMasterContext != null && userCreditMasterContext.Any())
                //{
                //    DealerStatistics.TotalDueForTomorrow = userCreditMasterContext.DefaultIfEmpty().Sum(_ => _.TomorrowsDue);
                //    DealerStatistics.TotalOverDue = userCreditMasterContext.DefaultIfEmpty().Sum(_ => _.Overdue);
                //}

                #endregion
                
                var overduePaymentContext = _emamiContext.OverduePayment.AsNoTracking().Where(_ => _.UserId == saudaFilterDto.UserId);
                if (overduePaymentContext != null && overduePaymentContext.Any())
                {
                    var tomDate = currentDate.AddDays(1);
                    decimal TotalDueForTomorrow = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) == DbFunctions.TruncateTime(tomDate)).DefaultIfEmpty().Sum(_ => (decimal?)_.Balance).GetValueOrDefault();
                    decimal TotalOverDue = overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) <= DbFunctions.TruncateTime(currentDate)).DefaultIfEmpty().Sum(_ => (decimal?)_.Balance).GetValueOrDefault();
                    DealerStatistics.TotalDueForTomorrow = TotalDueForTomorrow;
                    DealerStatistics.TotalOverDue = TotalOverDue;
                }


                var specialRatesContext = _emamiContext.SpecialRate.AsNoTracking().Where(_ => _.UserId == saudaFilterDto.UserId && DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(saudaFilterDto.FromDate)
                    && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(saudaFilterDto.ToDate)).ToList();
                if (specialRatesContext != null && specialRatesContext.Any())
                {
                    DealerStatistics.TotalSpecialRateApproval = specialRatesContext.ToList().Count();
                }

                DealerStatistics.CurrentDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
                if (DealerStatistics != null)
                {
                    return _resultService.SuccessObject(DealerStatistics);
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

        public ResultDto GetCustomerLedger(LoginUserIdDto inputDto)
        {
            _methodName = "GetCustomerLedger";
            var customerLedgerList = new CustomerLedgerDto();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (string.IsNullOrEmpty(inputDto.DealerCode))
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Code == inputDto.DealerCode);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var customerLedgerDaysCount = ConsoleSettings.CustomerLedgerDaysCount;
                //DateTime CurrentMonthStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                //DateTime CurrentMonthEndDate = CurrentMonthStartDate.AddMonths(1).AddDays(-1);
                List<CustomerLedger> customerLedgerListContext = _emamiContext.CustomerLedgers.AsNoTracking().Where(_ => _.UserCode == inputDto.DealerCode).OrderByDescending(_ => _.PostingDate)/*.Take(customerLedgerDaysCount)*/.ToList();
                if (customerLedgerListContext != null && customerLedgerListContext.Any())
                {
                    //customerLedgerListContext = customerLedgerListContext.OrderByDescending(_ => _.PostingDate);
                    //if (customerLedgerListContext.ToList().Count >= customerLedgerDaysCount)
                    //{
                    //    customerLedgerListContext = customerLedgerListContext;
                    //}

                    customerLedgerList.CurrentBalance = _emamiContext.CustomerLedgerDetails.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id).Balance;

                    var totalDebit = customerLedgerListContext.Select(s => s.Debit).Sum();
                    var totalCredit = customerLedgerListContext.Select(s => s.Credit).Sum();


                    customerLedgerList.TransactionType = (totalDebit > (totalCredit * -1)) ? (int)DTO.Enums.TransactionType.Debit : (int)DTO.Enums.TransactionType.Credit;

                    customerLedgerList.customerLedger = customerLedgerListContext.Select(_ => new CustomerLedgerlist()
                    {
                        // PdfUrl = _resultService.GetCustomerLedgerPath(_.PdfUrl),
                        TransactionAmount = (_.Credit == 0) ? _.Debit : _.Credit,
                        TransactionType = (_.Credit == 0) ? (int)DTO.Enums.TransactionType.Debit  : (int)DTO.Enums.TransactionType.Credit,   
                        PostingDate = _.PostingDate.ToString() == Constants.SqlDefualtDatetime ? (DateTime?)null : _.PostingDate,
                        DueDate = _.DueDate.ToString() == Constants.SqlDefualtDatetime ? (DateTime?)null : _.DueDate,
                        Reference = _.Reference != null ? _.Reference : string.Empty
                    }).OrderByDescending(_ => _.PostingDate)
                        //.OrderByDescending(_ => _.TransactionType == (int)DTO.Enums.TransactionType.Debit).ThenBy(t => t.TransactionType == (int)DTO.Enums.TransactionType.Debit ? -t.TransactionAmount : t.TransactionAmount)
                    .ToList(); 
                }
                if (customerLedgerList != null)
                {
                    return _resultService.SuccessObject(customerLedgerList);
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

        public ResultDto GetCustomerLedgerRolewise(LoginUserIdDto inputDto)
        {
            _methodName = "GetCustomerLedgerRolewise";
            var customerLedgerList = new CustomerLedgerRolewiseDto();
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

                var customerLedgerDaysCount = ConsoleSettings.CustomerLedgerDaysCount;

                var users = _emamiContext.UserReportingToMappings.AsNoTracking()
                    .Join(_emamiContext.Users.AsNoTracking(), ur => ur.UserId, u => u.Id, (ur, u) => new { ur, u });

                var dealerscontext = _emamiContext.UserCustomerMapping.AsNoTracking()
                    .Join(_emamiContext.Users.AsNoTracking(), ur => ur.CustomerId, u => u.Id, (ur, u) => new { ur, u });
                var customerledgercontext = _emamiContext.CustomerLedgers.AsNoTracking();
                //var customerledgerdetailscontext = _emamiContext.CustomerLedgerDetails.AsNoTracking();
                if (userRoleContext.RoleId == (int)DTO.Enums.Role.NationalTrader)
                {
                    decimal totalOutStandingBalance = 0;
                    var ztList = users.Where(user => user.ur.ReportingToUserId == userContext.Id && user.u.IsActive)
                        .Select(s => new { s.u.Id , s.u.Name , s.u.Code }).ToList();

                   
                    decimal totalCreadit = 0;
                    decimal totalDebit = 0;
                    foreach(var data in ztList)
                    {
                        var stList = users.Where(user => user.ur.ReportingToUserId == data.Id && user.u.IsActive).Select(s => s.u.Id).ToList();
                        var dealers = dealerscontext.Where(user => stList.Contains(user.ur.UserId)  && user.u.IsActive).Select(s => new { s.u.Code, s.u.Id }).ToList();

                        var dealercode = dealers.Select(a => a.Code).ToList();
                        var dealerIds = dealers.Select(a => a.Id).ToList();
                        List<CustomerLedger> customerLedgerListContext = customerledgercontext.Where(_ => dealercode.Contains(_.UserCode)).OrderByDescending(_ => _.PostingDate).ToList();
                        if (customerLedgerListContext != null && customerLedgerListContext.Any())
                        {
                            var currentbalance = customerLedgerListContext.Where(_ => dealerIds.Contains(_.UserId)).Sum(a => a.Balance);
                            var debit = customerLedgerListContext.Where(_ => dealerIds.Contains(_.UserId)).Sum(a => a.Debit);
                            var credit = customerLedgerListContext.Where(_ => dealerIds.Contains(_.UserId)).Sum(a => a.Credit);

                            totalCreadit = totalCreadit + credit;
                            totalDebit = totalDebit + debit;

                            totalOutStandingBalance = totalOutStandingBalance + currentbalance;

                            customerLedgerList.customerLedger.Add(new CustomerLedgerUsersList()
                            {
                                CustomerLedgerUserId = data.Id,
                                CustomerLedgerUserName = data.Name,
                                CustomerLedgerUserCode = data.Code,
                                UserOutStandingBalance = currentbalance,
                                TransactionType = (debit > (credit * -1)) ? (int)DTO.Enums.TransactionType.Debit : (int)DTO.Enums.TransactionType.Credit,
                            });
                        }
                    }
                    customerLedgerList.TransactionType = (totalDebit > (totalCreadit * -1)) ? (int)DTO.Enums.TransactionType.Debit : (int)DTO.Enums.TransactionType.Credit;
                    customerLedgerList.TotalOutStandingBalance = totalOutStandingBalance;

                }
                else if (userRoleContext.RoleId == (int)DTO.Enums.Role.ZonalTrader)
                {
                    decimal totalOutStandingBalance = 0;
                    var stList = users.Where(user => user.ur.ReportingToUserId == userContext.Id && user.u.IsActive).Select(s => new { s.u.Id, s.u.Name , s.u.Code}).ToList();
                    decimal totalCreadit = 0;
                    decimal totalDebit = 0;
                    var stIds = stList.Select(s => s.Id).ToList();
                    
                    var dealerIdsforbalance = dealerscontext.Where(user => stIds.Contains(user.ur.UserId) && user.u.IsActive).Select(s => s.u.Id).ToList();

                    totalOutStandingBalance = customerledgercontext.Where(_ => dealerIdsforbalance.Contains(_.UserId)).OrderByDescending(_ => _.PostingDate).Sum(a => a.Balance);

                    foreach (var data in stList)
                    {
                        var dealers = dealerscontext.Where(user =>user.ur.UserId == data.Id && user.u.IsActive).Select(s => new { s.u.Code, s.u.Id }).ToList();

                        var dealercode = dealers.Select(a => a.Code).ToList();
                        var dealerIds = dealers.Select(a => a.Id).ToList();
                        List<CustomerLedger> customerLedgerListContext = customerledgercontext.Where(_ => dealercode.Contains(_.UserCode)).OrderByDescending(_ => _.PostingDate).ToList();
                        if (customerLedgerListContext != null && customerLedgerListContext.Any())
                        {
                            var currentbalance = customerLedgerListContext.Where(_ => dealerIds.Contains(_.UserId)).Sum(a => a.Balance);
                            //totalOutStandingBalance = totalOutStandingBalance + currentbalance;
                            var debit = customerLedgerListContext.Where(_ => dealerIds.Contains(_.UserId)).Sum(a => a.Debit);
                            var credit = customerLedgerListContext.Where(_ => dealerIds.Contains(_.UserId)).Sum(a => a.Credit);

                            totalCreadit = totalCreadit + credit;
                            totalDebit = totalDebit + debit;

                            customerLedgerList.customerLedger.Add(new CustomerLedgerUsersList()
                            {
                                CustomerLedgerUserId = data.Id,
                                CustomerLedgerUserName = data.Name,
                                CustomerLedgerUserCode = data.Code,
                                UserOutStandingBalance = currentbalance,
                                TransactionType = (debit > (credit * -1)) ? (int)DTO.Enums.TransactionType.Debit : (int)DTO.Enums.TransactionType.Credit,
                            });
                        }
                    }
                    customerLedgerList.TransactionType = (totalDebit > (totalCreadit * -1)) ? (int)DTO.Enums.TransactionType.Debit : (int)DTO.Enums.TransactionType.Credit;
                    customerLedgerList.TotalOutStandingBalance = totalOutStandingBalance;

                }
                else if (userRoleContext.RoleId == (int)DTO.Enums.Role.StateTrader)
                {
                    decimal totalOutStandingBalance = 0;
                    var dealers = dealerscontext.Where(user => user.ur.UserId == userContext.Id && user.u.IsActive).Select(s => new { s.u.Code, s.u.Id , s.u.Name}).ToList();
                    var dealerIds = dealers.Select(a => a.Id).ToList();
                    decimal totalCreadit = 0;
                    decimal totalDebit = 0;
                    totalOutStandingBalance = customerledgercontext.Where(_ => dealerIds.Contains(_.UserId)).Sum(a => a.Balance);
                    foreach (var data in dealers)
                    {
                        List<CustomerLedger> customerLedgerListContext = customerledgercontext.Where(_ => _.UserCode == data.Code).OrderByDescending(_ => _.PostingDate).ToList();
                        if (customerLedgerListContext != null && customerLedgerListContext.Any())
                        {
                            var currentbalance = customerLedgerListContext.Where(_ => _.UserId == data.Id).Sum(a => a.Balance);
                            //totalOutStandingBalance = totalOutStandingBalance + currentbalance;
                            var debit = customerLedgerListContext.Where(_ => _.UserId == data.Id).Sum(a => a.Debit);
                            var credit = customerLedgerListContext.Where(_ => _.UserId == data.Id).Sum(a => a.Credit);

                            totalCreadit = totalCreadit + credit;
                            totalDebit = totalDebit + debit;

                            customerLedgerList.customerLedger.Add(new CustomerLedgerUsersList()
                            {
                                CustomerLedgerUserId = data.Id,
                                CustomerLedgerUserName = data.Name,
                                CustomerLedgerUserCode = data.Code,
                                UserOutStandingBalance = currentbalance,
                                TransactionType = (debit > (credit * -1)) ? (int)DTO.Enums.TransactionType.Debit : (int)DTO.Enums.TransactionType.Credit,
                            });
                        }
                    }
                    customerLedgerList.TransactionType = (totalDebit > (totalCreadit * -1)) ? (int)DTO.Enums.TransactionType.Debit : (int)DTO.Enums.TransactionType.Credit;

                    customerLedgerList.TotalOutStandingBalance = totalOutStandingBalance;
                }

                customerLedgerList.customerLedger = customerLedgerList.customerLedger.OrderByDescending(_ => _.TransactionType == (int)DTO.Enums.TransactionType.Debit).ThenBy(t => t.TransactionType == (int)DTO.Enums.TransactionType.Debit ? -t.UserOutStandingBalance : t.UserOutStandingBalance)
                    .ToList();
                //DateTime CurrentMonthStartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                //DateTime CurrentMonthEndDate = CurrentMonthStartDate.AddMonths(1).AddDays(-1);

                if (customerLedgerList != null)
                {
                    return _resultService.SuccessObject(customerLedgerList);
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

        public ResultDto PackwiseInvoicesByDealer(DashboardSaudaDetailsByDealersInputDto inputDto)
        {
            var dashboardDetailsByDealersOutputDto = new DashboardSalesDetailsByDealersOutputDto();
            _methodName = "PackwiseInvoicesByDealer";
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var invoiceListContext = _emamiContext.SalesRegister.AsNoTracking()
                       .Join(_emamiContext.Users.AsNoTracking(), i => i.CustomerCode, u => u.Code, (i, u) => new { i, u })
                       .Join(_emamiContext.Skus.AsNoTracking(), iu => iu.i.MaterialCode, s => s.SkuCode, (iu, s) => new { iu.i , iu.u, s })
                       .Where(_ => DbFunctions.TruncateTime(_.i.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                       && DbFunctions.TruncateTime(_.i.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && _.u.Id == inputDto.LoginUserId &&  _.i != null && _.s != null && _.u != null
                       && _.i.SalesOrganizationId == _.s.SalesOrganizationId && _.i.DistributionChannelId == _.s.DistributionChannelId && _.i.DivisionId == _.s.DivisionId);

                if (invoiceListContext != null && invoiceListContext.Any())
                {
                    if (inputDto.PackGroupId > 0)
                    {
                        invoiceListContext = invoiceListContext.AsNoTracking().Where(_ => _.s.PackGroupId == inputDto.PackGroupId);
                    }
                }
                if (invoiceListContext != null && invoiceListContext.Any())
                {
                    dashboardDetailsByDealersOutputDto.DealerId = inputDto.LoginUserId;
                    dashboardDetailsByDealersOutputDto.Dealer = invoiceListContext.FirstOrDefault().u.Name;
                    var cityContext = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == invoiceListContext.FirstOrDefault().u.CityId);
                    //if (cityContext != null)
                    //{
                        dashboardDetailsByDealersOutputDto.TownName = cityContext != null ? cityContext.CityName : string.Empty;
                    //}
                    dashboardDetailsByDealersOutputDto.TotalQuantity = invoiceListContext.Select(_ => _.i.QuantityMT).DefaultIfEmpty(0).Sum();
                   // dashboardDetailsByDealersOutputDto.TotalBookedInvoiceValue = invoiceListContext.Select(_ => _.ivd.SKUInvoiceTax).DefaultIfEmpty(0).Sum();
                    //dashboardDetailsByDealersOutputDto.IsBulkPack = inputDto.IsBulkPack;
                    dashboardDetailsByDealersOutputDto.DashboardSalesDetails = invoiceListContext.GroupBy(g => g.i.Id).Select(_ => new DashboardSalesDetailsOutputDto()
                    {
                        InvoiceId = _.FirstOrDefault().i.Id,
                        InvoiceNumber = _.FirstOrDefault().i.InvoiceNumber,
                        InvoiceDate = _.FirstOrDefault().i.InvoiceDate,
                        TotalQuantity = _.FirstOrDefault().i.QuantityMT,
                      //  InvoiceValue = _.Select(s => s.ivd.SKUInvoiceTax).DefaultIfEmpty(0).Sum(),
                    }).ToList();
                    return _resultService.SuccessObject(dashboardDetailsByDealersOutputDto);
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

        public ResultDto PackwiseInvoiceDetailsByDealer(IdInputDto inputDto)
        {
            var invoiceDetailsOutputDto = new InvoiceDetailsOutputDto();
            _methodName = "PackwiseInvoiceDetailsByDealer";
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                //var invoiceContext = _emamiContext.InvoiceDetails.AsNoTracking().Join(_emamiContext.Invoices.AsNoTracking(), ivd => ivd.InvoiceId, i => i.Id, (ivd, i) => new { ivd, i })
                //       .Join(_emamiContext.Skus.AsNoTracking(), ivdi => ivdi.ivd.SkuId, s => s.Id, (ivdi, s) => new { ivdi.ivd, ivdi.i, s })
                //       .Join(_emamiContext.OilTypes.AsNoTracking(), ivdis => ivdis.ivd.OilTypeId, o => o.Id, (ivdis, o) => new { ivdis.ivd, ivdis.i, ivdis.s, o })
                //       .Where(_ => _.i.Id == inputDto.Id && _.ivd != null && _.i != null && _.s != null && _.o != null);

                //if (invoiceContext != null && invoiceContext.Any())
                //{
                //    if (inputDto.IsBulkPack)
                //    {
                //        invoiceContext = invoiceContext.AsNoTracking().Where(_ => _.s.PackGroupId == (int)DTO.Enums.PackGroupType.BulkPacking);
                //    }
                //    else
                //    {
                //        invoiceContext = invoiceContext.AsNoTracking().Where(_ => _.s.PackGroupId == (int)DTO.Enums.PackGroupType.ConsumerPacking);
                //    }
                //}
                //if (invoiceContext != null && invoiceContext.Any())
                //{
                //    invoiceDetailsOutputDto.InvoiceId = invoiceContext.FirstOrDefault().i.Id;
                //    invoiceDetailsOutputDto.InvoiceNumber = invoiceContext.FirstOrDefault().i.BillingDocument;
                //    invoiceDetailsOutputDto.InvoiceDate = invoiceContext.FirstOrDefault().i.InvoiceDate;
                //    invoiceDetailsOutputDto.TotalInvoiceValue = invoiceContext.Select(_ => _.ivd.SKUInvoiceTax).DefaultIfEmpty(0).Sum();
                //    invoiceDetailsOutputDto.InvoiceQuantity = invoiceContext.Select(_ => _.ivd.ActualBilledQuantity).DefaultIfEmpty(0).Sum();

                //    invoiceDetailsOutputDto.InvoiceSKUDetails = invoiceContext.Select(_ => new InvoiceSKUDetailsOutputDto()
                //    {
                //        OilTypeId = _.ivd.OilTypeId,
                //        SkuId = _.ivd.SkuId,
                //        Quantity = _.ivd.ActualBilledQuantity,
                //        QuantityInCase = _.ivd.QuantityInCase,
                //        QunatityPrice = _.ivd.SKUInvoiceTax,
                //        OilType = _.o.Name,
                //        sku = _.s.SkuName,
                //    }).ToList();
                //    return _resultService.SuccessObject(invoiceDetailsOutputDto);
                //}
                //else
                //{
                //    return _resultService.ErrorMessage(Constants.RecordNotFound);
                //}

                //var invoiceContext = _emamiContext.InvoiceDetails.AsNoTracking().Join(_emamiContext.Invoices.AsNoTracking(), ivd => ivd.InvoiceId, i => i.Id, (ivd, i) => new { ivd, i })
                //       .Join(_emamiContext.Skus.AsNoTracking(), ivdi => ivdi.ivd.SkuId, s => s.Id, (ivdi, s) => new { ivdi.ivd, ivdi.i, s })
                //       .Join(_emamiContext.OilTypes.AsNoTracking(), ivdis => ivdis.ivd.OilTypeId, o => o.Id, (ivdis, o) => new { ivdis.ivd, ivdis.i, ivdis.s, o })
                //       .Join(_emamiContext.SalesRegister.AsNoTracking(), ivdis => ivdis.ivd.InvoiceId, sr => sr.InvoiceId, (ivdis, sr) => new { ivdis.ivd, ivdis.i, ivdis.s, ivdis.o, SalesRegister = sr })
                //       .Where(_ => _.i.Id == inputDto.Id && _.ivd != null && _.i != null && _.s != null && _.o != null);

                var invoiceContext = _emamiContext.SalesRegister.AsNoTracking()
                        .Join(_emamiContext.Skus.AsNoTracking(), sr => sr.MaterialCode, s => s.SkuCode, (sr, s) => new { sr, s })
                       .Where(_ => _.sr.Id == inputDto.Id
                       && _.sr.SalesOrganizationId == _.s.SalesOrganizationId && _.sr.DistributionChannelId == _.s.DistributionChannelId
                       && _.sr.DivisionId == _.s.DivisionId).ToList();

                if (invoiceContext != null && invoiceContext.Any())
                {
                    if (inputDto.PackGroupId > 0)
                    {
                        invoiceContext = invoiceContext.Where(_ => _.s.PackGroupId == inputDto.PackGroupId).ToList();
                    }
                }
                if (invoiceContext != null && invoiceContext.Any())
                {
                    invoiceDetailsOutputDto.InvoiceId = invoiceContext.FirstOrDefault().sr.Id;
                    invoiceDetailsOutputDto.InvoiceNumber = invoiceContext.FirstOrDefault().sr.InvoiceNumber;
                    invoiceDetailsOutputDto.InvoiceDate = invoiceContext.FirstOrDefault().sr.InvoiceDate;
                    invoiceDetailsOutputDto.TotalInvoiceValue = invoiceContext.Select(_ => Convert.ToDecimal(_.sr.TotalAmount)).DefaultIfEmpty(0).Sum();
                    invoiceDetailsOutputDto.InvoiceQuantity = invoiceContext.Select(_ => _.sr.QuantityMT).DefaultIfEmpty(0).Sum();

                    var skuIds = invoiceContext.Select(_ => _.sr.SkuId).ToList();
                    var skulist = _emamiContext.Skus.AsNoTracking().Where(_ => skuIds.Contains(_.Id)).ToList();

                    invoiceDetailsOutputDto.InvoiceSKUDetails = invoiceContext.Select(_ => new InvoiceSKUDetailsOutputDto()
                    {
                        OilTypeId = 0,
                        SkuId = 0,
                        Quantity = _.sr.QuantityMT,
                        QuantityInCase = _.sr.QuantityCase,
                        QunatityPrice = Convert.ToDecimal(_.sr.TotalAmount),
                        OilType = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(oilType => oilType.Id == _.sr.SkuId ).Name,
                        sku = skulist.FirstOrDefault(sku => sku.Id == _.sr.SkuId).SkuCode,
                    }).ToList();
                    return _resultService.SuccessObject(invoiceDetailsOutputDto);
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

    }
}
