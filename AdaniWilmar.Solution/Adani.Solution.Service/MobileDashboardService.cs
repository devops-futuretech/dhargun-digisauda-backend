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
    public interface IMobileDashboardServices
    {
        ResultDto DashboardOverallSauda(DashboardOverallSaudaInputDto inputDto);
        ResultDto DashboardWeekwiseOverallSauda(LoginUserIdDto loginUserIdDto);
        ResultDto DashboardOverallSales(DashboardOverallSaudaInputDto inputDto);
        ResultDto DashboardWeekwiseOverallSales(LoginUserIdDto loginUserIdDto);
        ResultDto DashboardSaudalistByDealers(DashboardOverallSaudaInputDto inputDto);
        ResultDto DashboardSaleslistByDealers(DashboardOverallSaudaInputDto inputDto);
        ResultDto DashboardPackwiseSaleslist(DashboardOverallSaudaInputDto inputDto);
        ResultDto DashboardSaudaDetailsByDealers(DashboardSaudaDetailsByDealersInputDto inputDto);
        ResultDto InvoicesByDealers(DashboardSaudaDetailsByDealersInputDto inputDto);
        ResultDto InvoiceDetailsByDealers(IdInputDto inputDto);
        ResultDto DueForTomorrowList(LoginUserIdDto inputDto);
        ResultDto GetTickerListForToday();
        ResultDto GetDailyRate(DailyRateInputDto inputDto);
        ResultDto GetDailyRateNew(DailyRateInputDto inputDto);
        ResultDto PackwiseInvoicesByDealer(DashboardSaudaDetailsByDealersInputDto inputDto);
        ResultDto PackwiseInvoiceDetailsByDealer(IdInputDto inputDto);
        ResultDto GetDailySFQuantityAllocation(QuantityAllocationInputDto inputDto);
        ResultDto BDOPlantDepotDetailsByDealer(LoginUserIdDto inputDto);
        ResultDto DailyBookedSaudaReport(DailyBookedSaudaInputDto inputDto);
        ResultDto SalesReport(DailyBookedSaudaInputDto inputDto);
        ResultDto GetDailyRateForManager(DailyRateInputDto inputDto);
        ResultDto InvoiceDetailsByDealer(IdInputDto inputDto);
        ResultDto GetSalesOrderDetails(IdInputDto inputDto);
        ResultDto UpdateSalesOrderDetails(LiftingUpdateDto inputDto);
    }
    public class MobileDashboardService : IMobileDashboardServices
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Mobile Dashboard Service");
        private const string ServiceName = "Mobile Dashboard Service";
        private string _methodName;
        private readonly IResultService _resultService;

        public MobileDashboardService(IAdaniContext salesContext, IResultService resultService)
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

                //IQueryable<UserCustomerMapping> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId);

                //IQueryable<long> dealersList = (inputDto.DealerId > 0) ? dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId && _.CustomerId == inputDto.DealerId).Select(s => s.CustomerId)
                //: dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId).Select(s => s.CustomerId);

                //var divisionsloginWiseuser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                // .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                var status = Constants.OverallSaudaStatus;


                IEnumerable<DashboardSauda> sauda = new List<DashboardSauda>();
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    
                    sauda = conn.Query<DashboardSauda>("GetDealerOverallSauda", new
                    {
                        UserId = inputDto.LoginUserId,
                        StartDate = inputDto.FromDate,
                        EndDate = inputDto.ToDate,
                        Status = UtilityHelper.ConvertIntListToCommaSeparatedString(status),
                        CustomerId=inputDto.DealerId
                    },commandType:CommandType.StoredProcedure,commandTimeout:300);

                }

                //var sauda = (from s in _emamiContext.Sauda.AsNoTracking()
                //             join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                //             join dm in divisionsloginWiseuser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                //             equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                //                             //join dl in dealersList on s.UserId equals dl.CustomerId
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
                    var targetContext = _emamiContext.UserCustomerSaudaTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId && _.MonthId == item.Id && _.Year == item.Year).ToList();
                    if (targetContext != null)
                    {
                        outputDto.TotalTarget = targetContext.Sum(_ => _.Target);
                    }
                    outputDto.MonthId = item.Id;
                    outputDto.Month = new DateTime(DateTime.Now.Year, item.Id, 1).ToString("MMMM");
                    //if (dealersList != null && dealersList.Any())
                    //{
                        //var saudaContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => dealersList.Any(a => a.CustomerId == _.Sauda.UserId) && DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(item.StartDate) &&
                        //DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(item.EndDate) && status.Contains(_.StatusId)).OrderByDescending(_ => _.CreatedDate).Select(s => new AchievmentDetailsDto()
                        //{
                        //    UserId = s.CreatedBy,
                        //    Date = s.CreatedDate,
                        //    Achievment = s.BidQuantity
                        //}).ToList();

                        outputDto.OverallSauda = sauda.Where(_ => _.Date.Date >= item.StartDate.Date &&
                                                    _.Date.Date <= item.EndDate.Date).Select(s => s.Achievment)
                                                    .DefaultIfEmpty(0).Sum();

                        //   outputDto.OverallSauda = outputDto.OverallSauda + saudaContext.Sum(_ => _.Achievment);
                        //    outputDto.AchievmentDetailsDto = saudaContext.ToList();
                        dashboardOverallsaudaOutpuDto.Add(outputDto);
                    //}
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var targetContext = _emamiContext.UserCustomerSaudaTarget.AsNoTracking().Where(_ => _.AssignedToId == loginUserIdDto.LoginUserId && _.MonthId == currentDate.Month && _.Year == currentDate.Year).ToList();
                if (targetContext != null)
                {
                    //OverAll target
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

               

                IEnumerable<DashboardSauda> saudaContextList = new List<DashboardSauda>();
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    var sqlQuery = @"CREATE TABLE #DealerTemp(DealerId BIGINT)
                                Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                                insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                                select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

                                if(@CustomerId>0)
                                begin
                                insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
                                 where UserId=@UserId and CustomerId=@CustomerId
                                end
                                else
                                begin
                                 insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
                                 where UserId=@UserId
                                end

                                 select 
                                  so.CreatedDate as Date,
                                  so.BidQuantity as Achievment
                                 from Saudas s with(NOLOCK)
                                 join SaudaOrders so with(NOLOCK) on s.Id=so.SaudaId
                                 join #UserDivision ud on ud.SalesOrganizationId=s.SalesOrganizationId
                                 and ud.DistributionChannelId=s.DistributionChannelId and ud.DivisionId=s.DivisionId
                                 where Cast(so.CreatedDate as date) >= Cast(@StartDate as date)
                                 and Cast(so.CreatedDate as date) <= Cast(@EndDate as date)
                                 and s.UserId in (select DealerId from #DealerTemp)
                                 and so.StatusId in @Status
                                  drop table #DealerTemp
                                  drop table #UserDivision
                        ";
                    saudaContextList = conn.Query<DashboardSauda>(sqlQuery, new
                    {
                        UserId = loginUserIdDto.LoginUserId,
                        StartDate = mStartDate,
                        EndDate = mEndDate,
                        Status = status,
                        CustomerId = loginUserIdDto.DealerId
                    });

                }

                //var saudaContextList = (from s in _emamiContext.Sauda.AsNoTracking()
                //                    join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                //                    join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                //                    equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                //                    //join dl in dealersList on s.UserId equals dl.CustomerId
                //                    where DbFunctions.TruncateTime(so.CreatedDate) >= DbFunctions.TruncateTime(mStartDate) &&
                //                    DbFunctions.TruncateTime(so.CreatedDate) <= DbFunctions.TruncateTime(mEndDate) && status.Contains(so.StatusId) 
                //                    && dealersList.Contains(s.UserId)
                //                    //&& (s.BdoId == loginUserIdDto.LoginUserId || s.BdoId==0)
                //                    select new { CreatedDate = so.CreatedDate , BidQuantity = so.BidQuantity });

                //if (dealersList != null && dealersList.Any())
                //{
                    //Weekwise report

                    //var query = from sauda in _emamiContext.Sauda.AsNoTracking().Where(_ => dealersList.Any(a => a.CustomerId == _.UserId)).ToList()
                    //            join saudaorders in _emamiContext.SaudaOrders.Where(_ => _.CreatedDate.Month == DateTime.UtcNow.Month).ToList() on sauda.Id equals saudaorders.SaudaId
                    //            group saudaorders by new { Week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(saudaorders.CreatedDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday), sauda.UserId } into g
                    //            select new { Weeks = g.Key, Sum = g.Sum(p => p.BidQuantity) };
                    //var s = query.ToList();
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

                        var saudalist = saudaContextList.Where(_ => _.Date.Date >= wStartDate.Date &&
                        _.Date.Date <= wEndDate.Date).ToList();
                        if (saudalist != null && saudalist.Any())
                        {
                            weekwiseTargetAchieved.WeekId = weekId;
                            weekwiseTargetAchieved.Week = "Week " + weekId;
                            weekwiseTargetAchieved.Achievement = saudalist.Sum(_ => _.Achievment);
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

                //var dealerlist = (inputDto.DealerId > 0) ? (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                //                                            join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                //                                            where ucm.UserId == inputDto.LoginUserId && ucm.CustomerId == inputDto.DealerId
                //                                            select ucm.CustomerId).ToList()
                //                      : (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                //                         join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                //                         where ucm.UserId == inputDto.LoginUserId
                //                         select ucm.CustomerId).ToList();

                //var divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                //   .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                var target = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId).ToList();

                IEnumerable<DashboardSauda> salesContext = new List<DashboardSauda>();
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    var sqlQuery = @"CREATE TABLE #DealerTemp(DealerId BIGINT)
                                Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                                insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                                select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

                                if(@CustomerId>0)
                                begin
                                insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
                                 where UserId=@UserId and CustomerId=@CustomerId
                                end
                                else
                                begin
                                 insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
                                 where UserId=@UserId
                                end

                                 select 
                                 s.InvoiceDate as Date,
                                 s.QuantityMT as Achievment
                                 from SalesRegisters s with(NOLOCK)
                                 join Skus sku with(NOLOCK) on s.MaterialCode=sku.SkuCode and s.SalesOrganizationId=sku.SalesOrganizationId
                                 and s.DistributionChannelId=sku.DistributionChannelId and s.DivisionId=sku.DivisionId
                                 join Users u with(NOLOCK) on s.CustomerCode=u.Code
                                 join #UserDivision ud on s.SalesOrganizationId=ud.SalesOrganizationId
                                 and s.DistributionChannelId=ud.DistributionChannelId and s.DivisionId=ud.DivisionId
                                 and Cast(s.InvoiceDate as date) >= Cast(@StartDate as date)
                                  and Cast(s.InvoiceDate as date) <= Cast(@EndDate as date)
                                  and u.Id in (select DealerId from #DealerTemp)
                                  drop table #DealerTemp
                                  drop table #UserDivision
                        ";
                    salesContext = conn.Query<DashboardSauda>(sqlQuery, new
                    {
                        UserId = inputDto.LoginUserId,
                        StartDate = inputDto.FromDate,
                        EndDate = inputDto.ToDate,
                        CustomerId = inputDto.DealerId
                    });

                }

                //var salesContext = (from s in _emamiContext.SalesRegister.AsNoTracking()
                //         join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                //         join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                //         join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                //         equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                //         where dealerlist.Contains(u.Id) && (DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                //            DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                //            && s.SalesOrganizationId == sku.SalesOrganizationId && s.DistributionChannelId == sku.DistributionChannelId
                //            && s.DivisionId == sku.DivisionId
                //          select new
                //          {
                //              Date = s.InvoiceDate != null ? s.InvoiceDate : DateTime.Now,
                //              Achievment = s.QuantityMT
                //          });

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

                    

                    //if (inputDto.DealerId > 0)
                    //    dealerlist = dealerlist.Select(_ => ).ToList();

                    //if (dealerlist.IsAny())
                    //{
                        outputDto.OverallSales = salesContext.Where(_ => (_.Date.Date >= item.StartDate.Date &&
                            _.Date.Date <= item.EndDate.Date)
                            ).OrderByDescending(_ => _.Date).Select(a => a.Achievment).DefaultIfEmpty(0).Sum();

                        //if (salesContext.IsAny())
                        //{
                            //foreach (var details in salesContext)
                            //{
                            //    // var InvoiceDetailsContext = _emamiContext.SalesRegister.AsNoTracking().Where(_ => _. == details.x.Id).ToList();

                            //    var acheivment = new AchievmentDetailsDto
                            //    {
                            //        UserId = details.c.u.Id,
                            //        Date = details.c.x.InvoiceDate != null ? details.c.x.InvoiceDate : DateTime.Now,
                            //        Achievment = details.c.x.QuantityMT
                            //    };
                            //    outputDto.AchievmentDetailsDto.Add(acheivment);
                            //}
                           // outputDto.OverallSales = outputDto.OverallSales + outputDto.AchievmentDetailsDto.Sum(_ => _.Achievment);
                       // }
                    //}
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
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

                //IQueryable<UserCustomerMapping> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == loginUserIdDto.LoginUserId);

                //IQueryable<UserCustomerMapping> dealersList = (loginUserIdDto.DealerId > 0) ? dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == loginUserIdDto.LoginUserId && _.CustomerId == loginUserIdDto.DealerId)
                //   : dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == loginUserIdDto.LoginUserId);

                //var divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == loginUserIdDto.LoginUserId)
                //   .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                
                //if (dealersList != null && dealersList.Any())
                //{


                    IEnumerable<DashboardSauda> salesContextList = new List<DashboardSauda>();
                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {

                        var sqlQuery = @"CREATE TABLE #DealerTemp(DealerId BIGINT)
                                Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                                insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                                select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

                                if(@CustomerId>0)
                                begin
                                insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
                                 where UserId=@UserId and CustomerId=@CustomerId
                                end
                                else
                                begin
                                 insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
                                 where UserId=@UserId
                                end

                                 select 
                                 s.InvoiceDate as Date,
                                 s.QuantityMT as Achievment
                                 from SalesRegisters s with(NOLOCK)
                                 join Skus sku on s.MaterialCode=sku.SkuCode and s.SalesOrganizationId=sku.SalesOrganizationId
                                 and s.DistributionChannelId=sku.DistributionChannelId and s.DivisionId=sku.DivisionId
                                 join Users u on s.CustomerCode=u.Code
                                 join #UserDivision ud on s.SalesOrganizationId=ud.SalesOrganizationId
                                 and s.DistributionChannelId=ud.DistributionChannelId and s.DivisionId=ud.DivisionId
                                 and Cast(s.InvoiceDate as date) >= Cast(@StartDate as date)
                                  and Cast(s.InvoiceDate as date) <= Cast(@EndDate as date)
                                  and u.Id in (select DealerId from #DealerTemp)
                                  drop table #DealerTemp
                                  drop table #UserDivision
                        ";
                        salesContextList = conn.Query<DashboardSauda>(sqlQuery, new
                        {
                            UserId = loginUserIdDto.LoginUserId,
                            StartDate = mStartDate,
                            EndDate = mEndDate,
                            CustomerId= loginUserIdDto.DealerId
                        });

                    }

                    //var salesContextList = (from s in _emamiContext.SalesRegister.AsNoTracking()
                    //                        join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                    //                        join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                    //                        join dm in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                    //                        equals new { SalesOrganizationId = dm.SalesOrganizationId, DistributionChannelId = dm.DistributionChannelId, DivisionId = dm.DivisionId }
                    //                        join dl in dealersList on u.Id equals dl.CustomerId
                    //                        where (DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(mStartDate) &&
                    //                        DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(mEndDate))
                    //                        && s.SalesOrganizationId == sku.SalesOrganizationId && s.DistributionChannelId == sku.DistributionChannelId
                    //                        && s.DivisionId == sku.DivisionId
                    //                        select new DashboardSauda (){ Date = s.InvoiceDate, Achievment = s.QuantityMT }).ToList();

                    //Weekwise report

                    //var query = from sauda in _emamiContext.Sauda.AsNoTracking().Where(_ => dealersList.Any(a => a.CustomerId == _.UserId)).ToList()
                    //            join saudaorders in _emamiContext.SaudaOrders.Where(_ => _.CreatedDate.Month == DateTime.UtcNow.Month).ToList() on sauda.Id equals saudaorders.SaudaId
                    //            group saudaorders by new { Week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(saudaorders.CreatedDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday), sauda.UserId } into g
                    //            select new { Weeks = g.Key, Sum = g.Sum(p => p.BidQuantity) };
                    //var s = query.ToList();
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

                        var sales = salesContextList.Where(_ => _.Date.Date >= wStartDate.Date &&
                                             _.Date.Date <= wEndDate.Date).ToList();

                        if (sales != null && sales.Any())
                        {
                            weekwiseTargetAchieved.WeekId = weekId;
                            weekwiseTargetAchieved.Week = "Week " + weekId;
                            weekwiseTargetAchieved.Achievement = sales.Sum(_ => _.Achievment);
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

                //}
                //else
                //{
                //    return _resultService.ErrorMessage(Constants.RecordNotFound);
                //}
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
                var saudaContext = _emamiContext.Sauda.AsNoTracking().Where(_ => _.CreatedBy == inputDto.LoginUserId && DbFunctions.TruncateTime(_.BiddingDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                    DbFunctions.TruncateTime(_.BiddingDate) <= DbFunctions.TruncateTime(inputDto.ToDate)).OrderByDescending(_ => _.BiddingDate).ToList();

                var userlistContext = saudaContext.Select(row => new
                {
                    userId = row.UserId
                }).Distinct().ToList();

                if (userlistContext != null)
                {
                    foreach (var item in userlistContext)
                    {
                        List<MonthDto> months = new List<MonthDto>();
                        months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);
                        decimal TotalTarget = 0;
                        decimal TotalAcheivement = 0;
                        var outputDto = new DashboardDetailsByDealersOutputDto();
                        foreach (var month in months)
                        {
                            var targetContext = _emamiContext.UserCustomerSaudaTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId && _.MonthId == month.Id && _.Year == month.Year).ToList();
                            if (targetContext != null)
                            {
                                TotalTarget = TotalTarget + targetContext.Sum(_ => _.Target);
                            }
                            var saudaByUserContext = saudaContext.Where(_ => _.UserId == item.userId).ToList();
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
                        var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == item.userId);
                        outputDto.DealerId = item.userId;
                        outputDto.Dealer = userContext.Name;
                        outputDto.Dealer = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == userContext.CityId).CityName;
                        outputDto.Target = TotalTarget;
                        outputDto.Achievement = TotalAcheivement;
                        dashboardDetailsByDealersOutputDto.Add(outputDto);
                    }
                }
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
            var userContexts = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
            if (userContexts == null)
            {
                return _resultService.ErrorMessage(Constants.UserNotFound);
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

            try
            {
                IQueryable<UserCustomerMapping> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId);
                if (dealersList != null && dealersList.Any())
                {
                    //var invoiceContext = _emamiContext.Invoices.AsNoTracking().Where(_ => dealersList.Any(a => a.CustomerId == _.UserId)
                    //&& DbFunctions.TruncateTime(_.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                    //DbFunctions.TruncateTime(_.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)).OrderByDescending(_ => _.InvoiceDate).ToList();

                    var invoiceContext = (from s in _emamiContext.SalesRegister.AsNoTracking()
                                   join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                                   join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                                             equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                   where DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                        && DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                        && dealersList.Any(a => a.CustomerId == u.Id) && s.SkuId > 0
                        select new
                        {
                            UserId=u.Id,
                            DealerName=u.Name,
                            CityId=u.CityId,
                            QuantityMT=s.QuantityMT,
                            InvoiceId = s.Id
                        }
                                   );

                    //var invoiceContext = _emamiContext.SalesRegister.AsNoTracking()
                    //    .Join(_emamiContext.Users.AsNoTracking(), sr => sr.CustomerCode, us => us.Code, (sr, us) => new { SalesRegister = sr, User = us })
                    //    //.Join(_emamiContext.Skus.AsNoTracking(), sr => sr.SalesRegister.MaterialCode, sku => sku.SkuCode, (sr, sku) => new { SalesRegister = sr.SalesRegister, User = sr.User, Sku = sku })
                    //    .Where(w => DbFunctions.TruncateTime(w.SalesRegister.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //    && DbFunctions.TruncateTime(w.SalesRegister.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    //    && dealersList.Any(a => a.CustomerId == w.User.Id) && w.SalesRegister.SkuId != 0 
                    //    //&& w.SalesRegister.SalesOrganizationId == w.Sku.SalesOrganizationId && w.SalesRegister.DistributionChannelId == w.Sku.DistributionChannelId
                    //    //&& w.SalesRegister.DivisionId = w.Sku.DivisionId
                    //    )
                    //    .Select(s => new
                    //    {
                    //        UserId = s.User.Id,
                    //        DealerName = s.User.Name,
                    //        CityId = s.User.CityId,
                    //        QuantityMT = s.SalesRegister.QuantityMT,
                    //        InvoiceId = s.SalesRegister.Id
                    //    });

                    if (invoiceContext != null && invoiceContext.Any())
                    {
                        var userlistContext = invoiceContext.Select(row => new
                        {
                            userId = row.UserId,
                            userName = row.DealerName,
                            CityId = row.CityId
                        }).Distinct().ToList();

                        foreach (var item in userlistContext)
                        {
                            List<MonthDto> months = new List<MonthDto>();
                            months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);
                            decimal TotalTarget = 0;
                            decimal TotalAcheivement = 0;
                            var outputDto = new DashboardDetailsByDealersOutputDto();
                            foreach (var month in months)
                            {
                                //var targetContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == item.userId && _.MonthId == month.Id && _.FinancialYearId == month.Year).ToList();
                                //if (targetContext != null)
                                //{
                                //    TotalTarget = TotalTarget + targetContext.Sum(_ => _.Target);
                                //}

                                //var invoiceByUserContext = invoiceContext.Where(_ => _.UserId == item.userId).ToList();
                                //if (invoiceByUserContext != null)
                                //{
                                //    foreach (var invoice in invoiceByUserContext)
                                //    {
                                //        //var invoicedetailContext = _emamiContext.InvoiceDetails.AsNoTracking()
                                //        //    .Where(_ => _.InvoiceId == invoice.Id).ToList();
                                //        var invoicedetailContext = _emamiContext.InvoiceDetails.AsNoTracking()
                                //            .Join(_emamiContext.SalesRegister.AsNoTracking(), invd => invd.InvoiceId, sr => sr.InvoiceId, (invd, sr) => new { InvoiceDetails = invd, SalesRegister = sr })
                                //            .Where(_ => _.InvoiceDetails.InvoiceId == invoice.Id).Select(s => s.SalesRegister.QuantityMT).ToList();
                                //        if (invoicedetailContext != null && invoicedetailContext.Any())
                                //        {
                                //            TotalAcheivement = TotalAcheivement + invoicedetailContext.DefaultIfEmpty(0).Sum();
                                //        }
                                //    }
                                //}
                                var targetContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == item.userId && _.MonthId == month.Id && _.FinancialYearId == month.Year).ToList();
                                if (targetContext != null)
                                {
                                    TotalTarget = TotalTarget + targetContext.Sum(_ => _.Target);
                                }

                                var invoiceByUserContext = invoiceContext.Where(_ => _.UserId == item.userId).ToList();
                                if (invoiceByUserContext != null)
                                {
                                    var invoiceIds = invoiceByUserContext.Select(s => s.InvoiceId).ToList();
                                    //var invoiceDetails = _emamiContext.InvoiceDetails.AsNoTracking()
                                    //.Join(_emamiContext.SalesRegister.AsNoTracking(), invd => invd.InvoiceId, sr => sr.InvoiceId, (invd, sr) => new { InvoiceDetails = invd, SalesRegister = sr })
                                    //.Where(_ => invoiceIds.Contains(_.InvoiceDetails.InvoiceId)).Select(s => new { InvoiceId = s.InvoiceDetails.InvoiceId, QuantityMT = s.SalesRegister.QuantityMT }).ToList();

                                    TotalAcheivement = TotalAcheivement + invoiceContext.Where(_ => invoiceIds.Contains(_.InvoiceId)).Select(s => s.QuantityMT).DefaultIfEmpty(0).Sum();

                                    //foreach (var invoice in invoiceByUserContext)
                                    //{
                                    //    var invoicedetailContext = invoiceContext.Where(_ => _.InvoiceId == invoice.InvoiceId).Select(s => s.QuantityMT).ToList();
                                    //    if (invoicedetailContext != null && invoicedetailContext.Any())
                                    //    {
                                    //        TotalAcheivement = TotalAcheivement + invoicedetailContext.DefaultIfEmpty(0).Sum();
                                    //    }
                                    //}

                                }
                            }

                            outputDto.DealerId = item.userId;
                            outputDto.Dealer = item.userName;
                            outputDto.TownName = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == item.CityId) !=null ? _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == item.CityId).CityName : String.Empty;
                            outputDto.Target = TotalTarget;
                            outputDto.Achievement = TotalAcheivement;
                            dashboardDetailsByDealersOutputDto.Add(outputDto);
                        }
                    }
                }
                return SucessResult(dashboardDetailsByDealersOutputDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                IQueryable<UserCustomerMapping> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId);

                #region oldCode
                //if (dealersList != null && dealersList.Any())
                //{
                //    var invoiceDetailsContextList = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.Invoice != null && DbFunctions.TruncateTime(_.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                //      DbFunctions.TruncateTime(_.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && dealersList.Any(a => a.CustomerId == _.Invoice.UserId));
                //    List<long> dealerIds = new List<long>();
                //    if (invoiceDetailsContextList != null && invoiceDetailsContextList.Any())
                //    {
                //        dealerIds = invoiceDetailsContextList.Select(_ => _.Invoice.UserId).Distinct().ToList();
                //    }
                //    foreach (var dealerId in dealerIds)
                //    {
                //        var salesDetail = new DashboardDetailsByDealersOutputDto();
                //        salesDetail.DealerId = dealerId;
                //        var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == dealerId);
                //        if (dealerContext != null)
                //        {
                //            salesDetail.Dealer = dealerContext.Name;
                //            var cityContext = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == dealerContext.CityId);
                //            if (cityContext != null)
                //            {
                //                salesDetail.TownName = cityContext.CityName;
                //            }
                //        }
                //        List<MonthDto> months = new List<MonthDto>();
                //        months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);
                //        decimal TotalTarget = 0;
                //        foreach (var month in months)
                //        {
                //            var targetContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedFromId == inputDto.LoginUserId && _.AssignedToId == dealerId && _.MonthId == month.Id).ToList();
                //            if (targetContext != null)
                //            {
                //                TotalTarget = TotalTarget + targetContext.Sum(_ => _.Target);
                //            }
                //        }
                //        salesDetail.Target = TotalTarget;
                //        if (inputDto.IsBulkPack == true)
                //        {
                //            var bulkPackContextList = invoiceDetailsContextList.AsNoTracking().Join(_emamiContext.Skus.AsNoTracking(), i => i.SkuId, s => s.Id, (i, s) => new { i, s })
                //                .Where(_ => _.s.PackGroupId == (int)DTO.Enums.PackGroupType.BulkPacking && _.i.Invoice != null && _.i.Invoice.UserId == dealerId);
                //            if (bulkPackContextList != null && bulkPackContextList.Any())
                //            {
                //                salesDetail.Achievement = bulkPackContextList.Sum(_ => _.i.ActualBilledQuantity);
                //            }
                //        }
                //        if (inputDto.IsBulkPack == false)
                //        {
                //            var customPackContextList = invoiceDetailsContextList.AsNoTracking().Join(_emamiContext.Skus.AsNoTracking(), i => i.SkuId, s => s.Id, (i, s) => new { i, s })
                //                .Where(_ => _.s.PackGroupId == (int)DTO.Enums.PackGroupType.ConsumerPacking && _.i.Invoice != null && _.i.Invoice.UserId == dealerId);
                //            if (customPackContextList != null && customPackContextList.Any())
                //            {
                //                salesDetail.Achievement = customPackContextList.Sum(_ => _.i.ActualBilledQuantity);
                //            }
                //        }
                //        dashboardOutputDto.Add(salesDetail);
                //    }
                //}
                #endregion

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



                if (dealersList != null && dealersList.Any())
                {
                    IEnumerable<DashboardSalesDto> invoiceDetailsContextList = new List<DashboardSalesDto>();
                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {
                        var sqlQuery = @"Create Table #DealerIdsTemp(DealerId bigint)
Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

insert into #DealerIdsTemp select CustomerId from UserCustomerMappings where UserId=@UserId

select 
s.QuantityMT,
u.Id as UserId,
sku.PackGroupId
from SalesRegisters s with(NOLOCK)
join Skus sku on s.MaterialCode=sku.SkuCode and s.DivisionId=sku.DivisionId
and s.SalesOrganizationId=sku.SalesOrganizationId and s.DistributionChannelId=sku.DistributionChannelId
join Users u on s.CustomerCode=u.Code
join #UserDivision ud on ud.SalesOrganizationId=s.SalesOrganizationId
and ud.DistributionChannelId=s.DistributionChannelId and ud.DivisionId=s.DivisionId
where Cast(s.InvoiceDate as date) >= Cast(@FromDate as date)
and Cast(s.InvoiceDate as date) <= Cast(@ToDate as date)
and u.Id in (select DealerId from #DealerIdsTemp)
drop table #UserDivision
drop table #DealerIdsTemp
";

                        invoiceDetailsContextList = conn.Query<DashboardSalesDto>(sqlQuery, new
                        {
                            UserId = inputDto.LoginUserId,
                            FromDate = inputDto.FromDate,
                            ToDate = inputDto.ToDate,
                        }).ToList();
                    }

                    //var invoiceDetailsContextList = (from s in _emamiContext.SalesRegister.AsNoTracking()
                    //              join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                    //              join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                    //              join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                    //                        equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                    //                        where DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //    && DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    //    && dealersList.Any(a => a.CustomerId == u.Id) && s.SalesOrganizationId == sku.SalesOrganizationId 
                    //    && s.DistributionChannelId == sku.DistributionChannelId
                    //    && s.DivisionId == sku.DivisionId
                    //    && s.SkuId > 0
                    //    select new
                    //    {
                    //        UserId=u.Id,
                    //        QuantityMT=s.QuantityMT,
                    //        PackGroupId = sku.PackGroupId
                    //    });


                    //var invoiceDetailsContextList = _emamiContext.SalesRegister.AsNoTracking()
                    //    .Join(_emamiContext.Skus.AsNoTracking(), sr => sr.MaterialCode, sk => sk.SkuCode, (sr, sk) => new { SalesRegister = sr, Sku = sk })
                    //    .Join(_emamiContext.Users.AsNoTracking(), sr => sr.SalesRegister.CustomerCode, us => us.Code, (sr, us) => new { SalesRegister = sr.SalesRegister, Sku = sr.Sku, User = us })
                    //    .Where(_ => DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //    && DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    //    && dealersList.Any(a => a.CustomerId == _.User.Id) && _.SalesRegister.SalesOrganizationId == _.Sku.SalesOrganizationId && _.SalesRegister.DistributionChannelId == _.Sku.DistributionChannelId
                    //    && _.SalesRegister.DivisionId == _.Sku.DivisionId
                    //    //&& _.User.DivisionId == userContext.DivisionId && _.Sku.DivisionId == _.User.DivisionId
                    //    )
                    //    .Select(s => new
                    //    {
                    //        UserId = s.User.Id,
                    //        QuantityMT = s.SalesRegister.QuantityMT,
                    //        PackGroupId = s.Sku.PackGroupId
                    //    }).ToList();

                    List<long> dealerIds = new List<long>();
                    if (invoiceDetailsContextList != null && invoiceDetailsContextList.Any())
                    {
                        dealerIds = invoiceDetailsContextList.Select(_ => _.UserId).Distinct().ToList();
                    }
                    foreach (var dealerId in dealerIds)
                    {
                        var salesDetail = new DashboardDetailsByDealersOutputDto();
                        salesDetail.DealerId = dealerId;
                        var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == dealerId);
                        if (dealerContext != null)
                        {
                            salesDetail.Dealer = dealerContext.Name;
                            var cityContext = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == dealerContext.CityId);                          
                            salesDetail.TownName = cityContext !=null ? cityContext.CityName : string.Empty;
                        }
                        List<MonthDto> months = new List<MonthDto>();
                        months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);
                        decimal TotalTarget = 0;
                        foreach (var month in months)
                        {
                            var targetContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedFromId == inputDto.LoginUserId && _.AssignedToId == dealerId && _.MonthId == month.Id).ToList();
                            if (targetContext != null)
                            {
                                TotalTarget = TotalTarget + targetContext.Sum(_ => _.Target);
                            }
                        }
                        salesDetail.Target = TotalTarget;
                        if (inputDto.PackGroupId > 0)
                        {
                            var bulkPackContextList = invoiceDetailsContextList
                                .Where(_ => _.PackGroupId == inputDto.PackGroupId && _.UserId == dealerId);
                            if (bulkPackContextList != null && bulkPackContextList.Any())
                            {
                                salesDetail.Achievement = bulkPackContextList.Sum(_ => _.QuantityMT);
                            }
                        }
                        else
                        {
                            var bulkPackContextList = invoiceDetailsContextList.Where(_ => _.UserId == dealerId);
                            if (bulkPackContextList != null && bulkPackContextList.Any())
                            {
                                salesDetail.Achievement = bulkPackContextList.Sum(_ => _.QuantityMT);
                            }
                        }
                        //if (inputDto.IsBulkPack == false)
                        //{
                        //    var customPackContextList = invoiceDetailsContextList
                        //        .Where(_ => _.PackGroupId == (int)DTO.Enums.PackGroupType.Bakery
                        //        && _.UserId == dealerId);
                        //    if (customPackContextList != null && customPackContextList.Any())
                        //    {
                        //        salesDetail.Achievement = customPackContextList.Sum(_ => _.QuantityMT);
                        //    }
                        //}
                        dashboardOutputDto.Add(salesDetail);
                    }
                }

                //if (dashboardOutputDto != null && dashboardOutputDto.Any())
                //{
                return _resultService.SuccessObject(dashboardOutputDto.OrderBy(_ => _.Dealer).ToList());
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
            var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
            if (userContext == null)
            {
                return _resultService.ErrorMessage(Constants.UserNotFound);
            }
            try
            {
                var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);

                if (dealerContext != null)
                {
                    //var invoiceContext = _emamiContext.Invoices.AsNoTracking().Where(_ => _.UserId == inputDto.DealerId && DbFunctions.TruncateTime(_.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                    //DbFunctions.TruncateTime(_.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)).ToList();

                    decimal TotalQuantity = 0;
                    decimal TotalInvoiceValue = 0;
                    //if (invoiceContext != null)
                    //{
                    //    foreach (var item in invoiceContext)
                    //    {
                    //        //var invoicedetailsContext = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.InvoiceId == item.Id).ToList();
                    //        var invoicedetailsContext = _emamiContext.InvoiceDetails.AsNoTracking()
                    //            .Join(_emamiContext.SalesRegister.AsNoTracking(), invd => invd.InvoiceId, sr => sr.InvoiceId, (invd, sr) => new { InvoiceDetails = invd, SalesRegister = sr })
                    //            .Where(_ => _.InvoiceDetails.InvoiceId == item.Id).Select(s => s.SalesRegister.QuantityMT).ToList();

                    //        var DashboardSaudaDetails = new DashboardSalesDetailsOutputDto()
                    //        {
                    //            InvoiceId = item.Id,
                    //            InvoiceNumber = item.BillingDocument,
                    //            InvoiceValue = item.NetValue,
                    //            InvoiceDate = item.InvoiceDate,
                    //            TotalQuantity = invoicedetailsContext.DefaultIfEmpty(0).Sum() //invoicedetailsContext.Sum(_ => _.ActualBilledQuantity)
                    //        };
                    //        TotalQuantity = TotalQuantity + invoicedetailsContext.DefaultIfEmpty(0).Sum();  //invoicedetailsContext.Sum(_ => _.ActualBilledQuantity)
                    //        TotalInvoiceValue = TotalInvoiceValue + item.NetValue;
                    //        dashboardDetailsByDealersOutputDto.DashboardSalesDetails.Add(DashboardSaudaDetails);
                    //    }
                    //    dashboardDetailsByDealersOutputDto.DealerId = dealerContext.Id;
                    //    dashboardDetailsByDealersOutputDto.Dealer = dealerContext.Name;
                    //    dashboardDetailsByDealersOutputDto.TotalQuantity = TotalQuantity;
                    //    dashboardDetailsByDealersOutputDto.TotalBookedInvoiceValue = TotalInvoiceValue;
                    //    var cityContext = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == dealerContext.CityId);
                    //    if (cityContext != null)
                    //    {
                    //        dashboardDetailsByDealersOutputDto.TownName = cityContext.CityName;
                    //    }
                    //}

                    #region 27-12-2019
                    //if (invoiceContext != null)
                    //{
                    //    var invoiceIds = invoiceContext.Select(s => s.Id).Distinct().ToList();
                    //    var invoiceDetails = _emamiContext.InvoiceDetails.AsNoTracking()
                    //           .Join(_emamiContext.SalesRegister.AsNoTracking(), invd => invd.InvoiceId, sr => sr.InvoiceId, (invd, sr) => new { InvoiceDetails = invd, SalesRegister = sr })
                    //           .Where(_ => invoiceIds.Contains(_.InvoiceDetails.InvoiceId))
                    //           .Select(s => new { InvoiceId = s.InvoiceDetails.InvoiceId, QuantityMT = s.SalesRegister.QuantityMT }).ToList();

                    //    if (invoiceDetails != null && invoiceDetails.Any())
                    //    {
                    //        foreach (var item in invoiceContext)
                    //        {
                    //            //var invoicedetailsContext = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.InvoiceId == item.Id).ToList();
                    //            var invoicedetailsContext = invoiceDetails.Where(_ => _.InvoiceId == item.Id).Select(s => s.QuantityMT).ToList();

                    //            var DashboardSaudaDetails = new DashboardSalesDetailsOutputDto()
                    //            {
                    //                InvoiceId = item.Id,
                    //                InvoiceNumber = item.BillingDocument,
                    //                InvoiceValue = item.NetValue,
                    //                InvoiceDate = item.InvoiceDate,
                    //                TotalQuantity = invoicedetailsContext.DefaultIfEmpty(0).Sum() //invoicedetailsContext.Sum(_ => _.ActualBilledQuantity)
                    //            };
                    //            TotalQuantity = TotalQuantity + invoicedetailsContext.DefaultIfEmpty(0).Sum();  //invoicedetailsContext.Sum(_ => _.ActualBilledQuantity)
                    //            TotalInvoiceValue = TotalInvoiceValue + item.NetValue;
                    //            dashboardDetailsByDealersOutputDto.DashboardSalesDetails.Add(DashboardSaudaDetails);
                    //        }
                    //    }
                    //    dashboardDetailsByDealersOutputDto.DealerId = dealerContext.Id;
                    //    dashboardDetailsByDealersOutputDto.Dealer = dealerContext.Name;
                    //    dashboardDetailsByDealersOutputDto.TotalQuantity = TotalQuantity;
                    //    dashboardDetailsByDealersOutputDto.TotalBookedInvoiceValue = TotalInvoiceValue;
                    //    var cityContext = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == dealerContext.CityId);
                    //    if (cityContext != null)
                    //    {
                    //        dashboardDetailsByDealersOutputDto.TownName = cityContext.CityName;
                    //    }
                    //} 
                    #endregion

                    var invoiceContext = _emamiContext.SalesRegister.AsNoTracking()
                        .Join(_emamiContext.Skus.AsNoTracking(), sr => sr.MaterialCode, sk => sk.SkuCode, (sr, sk) => new { SalesRegister = sr, Sku = sk })
                        .Join(_emamiContext.Users.AsNoTracking(), sr => sr.SalesRegister.CustomerCode, us => us.Code, (sr, us) => new { SalesRegister = sr.SalesRegister, Sku = sr.Sku, User = us })
                        .Where(w => DbFunctions.TruncateTime(w.SalesRegister.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                        && DbFunctions.TruncateTime(w.SalesRegister.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                        && w.User.Id == inputDto.DealerId
                        && w.SalesRegister.SalesOrganizationId == w.Sku.SalesOrganizationId && w.SalesRegister.DistributionChannelId == w.Sku.DistributionChannelId
                        && w.SalesRegister.DivisionId == w.Sku.DivisionId
                        //&& w.User.DivisionId == userContext.DivisionId
                        )
                        .Select(s => new
                        {
                            InvoiveId = s.SalesRegister.Id,
                            PackGroupId = s.Sku.PackGroupId,
                            QuantityMT = s.SalesRegister.QuantityMT,
                            BillNumber = s.SalesRegister.InvoiceNumber,
                            BillingDate = s.SalesRegister.InvoiceDate,
                            NetValue = s.SalesRegister.TotalAmount,
                        }).ToList();

                    if (invoiceContext != null)
                    {
                        var invoiceIds = invoiceContext.Select(s => s.InvoiveId).Distinct().ToList();

                        if (invoiceContext != null && invoiceContext.Any())
                        {
                            foreach (var item in invoiceContext)
                            {
                                //var invoicedetailsContext = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.InvoiceId == item.Id).ToList();
                                var invoicedetailsContext = invoiceContext.Where(_ => _.InvoiveId == item.InvoiveId).Select(s => s.QuantityMT).ToList();

                                var DashboardSaudaDetails = new DashboardSalesDetailsOutputDto()
                                {
                                    InvoiceId = item.InvoiveId,
                                    InvoiceNumber = item.BillNumber,
                                    InvoiceValue = Convert.ToDecimal(item.NetValue),
                                    InvoiceDate = (DateTime)item.BillingDate,
                                    PackgroupId = (long)item.PackGroupId,
                                    //IsBulkPack = item.PackGroupId == (int)DTO.Enums.PackGroupType.Premium ? true : false,
                                    TotalQuantity = invoicedetailsContext.DefaultIfEmpty(0).Sum() //invoicedetailsContext.Sum(_ => _.ActualBilledQuantity)
                                };
                                TotalQuantity = TotalQuantity + invoicedetailsContext.DefaultIfEmpty(0).Sum();  //invoicedetailsContext.Sum(_ => _.ActualBilledQuantity)
                                TotalInvoiceValue = TotalInvoiceValue + Convert.ToDecimal(item.NetValue);
                                dashboardDetailsByDealersOutputDto.DashboardSalesDetails.Add(DashboardSaudaDetails);
                            }
                        }
                        dashboardDetailsByDealersOutputDto.DealerId = dealerContext.Id;
                        dashboardDetailsByDealersOutputDto.Dealer = dealerContext.Name;
                        dashboardDetailsByDealersOutputDto.TotalQuantity = TotalQuantity;
                        dashboardDetailsByDealersOutputDto.TotalBookedInvoiceValue = TotalInvoiceValue;
                        var cityContext = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == dealerContext.CityId);
                        if (cityContext != null)
                        {
                            dashboardDetailsByDealersOutputDto.TownName = cityContext.CityName;
                        }
                    }
                }
                return SucessResult(dashboardDetailsByDealersOutputDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
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
            var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
            if (userContext == null)
            {
                return _resultService.ErrorMessage(Constants.UserNotFound);
            }
            try
            {
                #region 27-12-2019
                //var invoiceContext = _emamiContext.Invoices.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.Id);
                //if (invoiceContext != null)
                //{
                //    invoiceDetailsOutputDto.InvoiceId = invoiceContext.Id;
                //    invoiceDetailsOutputDto.InvoiceNumber = invoiceContext.BillingDocument;
                //    invoiceDetailsOutputDto.InvoiceDate = invoiceContext.InvoiceDate;
                //    invoiceDetailsOutputDto.TotalInvoiceValue = invoiceContext.NetValue;
                //    //var invoicedetailContext = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.InvoiceId == inputDto.Id).ToList();
                //    //if (invoicedetailContext != null)
                //    //{
                //    //    decimal TotalInvoiceQuantity = 0;
                //    //    foreach (var item in invoicedetailContext)
                //    //    {
                //    //        var InvoiceSKUDetails = new InvoiceSKUDetailsOutputDto()
                //    //        {
                //    //            OilTypeId = item.OilTypeId,
                //    //            SkuId = item.SkuId,
                //    //            Quantity = item.ActualBilledQuantity,
                //    //            //QuantityInCase = _resultService.ConvertCasetoMetricTon(item.ActualBilledQuantity, item.SkuId),
                //    //            QuantityInCase = item.QuantityInCase,
                //    //            QunatityPrice = item.SKUInvoiceTax,
                //    //            OilType = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == item.OilTypeId).Name,
                //    //            sku = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == item.SkuId).SkuName,
                //    //        };
                //    //        TotalInvoiceQuantity = TotalInvoiceQuantity + item.ActualBilledQuantity;
                //    //        invoiceDetailsOutputDto.InvoiceSKUDetails.Add(InvoiceSKUDetails);
                //    //    }
                //    //    invoiceDetailsOutputDto.InvoiceQuantity = TotalInvoiceQuantity;
                //    //}
                //    var invoicedetailContext = _emamiContext.InvoiceDetails.AsNoTracking()
                //        .Join(_emamiContext.SalesRegister.AsNoTracking(), invd => invd.InvoiceId, sr => sr.InvoiceId, (invd, sr) => new { InvoiceDetails = invd, SalesRegister = sr })
                //        .Where(_ => _.InvoiceDetails.InvoiceId == inputDto.Id).ToList();
                //    if (invoicedetailContext != null)
                //    {
                //        decimal TotalInvoiceQuantity = 0;
                //        foreach (var item in invoicedetailContext)
                //        {
                //            var InvoiceSKUDetails = new InvoiceSKUDetailsOutputDto()
                //            {
                //                OilTypeId = item.InvoiceDetails.OilTypeId,
                //                SkuId = item.InvoiceDetails.SkuId,
                //                Quantity = item.SalesRegister.QuantityMT,
                //                //QuantityInCase = _resultService.ConvertCasetoMetricTon(item.ActualBilledQuantity, item.SkuId),
                //                QuantityInCase = item.SalesRegister.QuantityCase,
                //                QunatityPrice = item.InvoiceDetails.SKUInvoiceTax,
                //                OilType = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == item.InvoiceDetails.OilTypeId).Name,
                //                sku = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == item.InvoiceDetails.SkuId).SkuName,
                //            };
                //            TotalInvoiceQuantity = TotalInvoiceQuantity + item.SalesRegister.QuantityMT;
                //            invoiceDetailsOutputDto.InvoiceSKUDetails.Add(InvoiceSKUDetails);
                //        }
                //        invoiceDetailsOutputDto.InvoiceQuantity = TotalInvoiceQuantity;
                //    }
                //} 

                //var invoiceContext = _emamiContext.Invoices.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.Id);

                var invoiceDetailsContextList = _emamiContext.SalesRegister.AsNoTracking()
                        .Join(_emamiContext.Skus.AsNoTracking(), sr => sr.MaterialCode, sk => sk.SkuCode, (sr, sk) => new { SalesRegister = sr, Sku = sk })
                        .Join(_emamiContext.Users.AsNoTracking(), sr => sr.SalesRegister.CustomerCode, us => us.Code, (sr, us) => new { SalesRegister = sr.SalesRegister, Sku = sr.Sku, User = us })
                        .Join(_emamiContext.UserRoles.AsNoTracking(), us => us.User.Id, ur => ur.UserId, (us, ur) => new { SalesRegister = us.SalesRegister, Sku = us.Sku, User = us.User, UserRoles = ur })
                        .Where(w => w.SalesRegister.Id == inputDto.Id && w.UserRoles.RoleId == (int)DTO.Enums.Role.Dealer
                        //&& w.User.DivisionId == userContext.DivisionId
                        && w.SalesRegister.SalesOrganizationId == w.Sku.SalesOrganizationId && w.SalesRegister.DistributionChannelId == w.Sku.DistributionChannelId
                            && w.SalesRegister.DivisionId == w.Sku.DivisionId
                        )
                        .Select(s => new
                        {
                            PackGroupId = s.Sku.PackGroupId,
                            QuantityMT = s.SalesRegister.QuantityMT,
                            QuantityCase = s.SalesRegister.QuantityCase,
                            SkuId = s.Sku.Id,
                            Sku = s.Sku.SkuName,
                            OilTypeId = s.Sku.OilType.Id,
                            OilType = s.Sku.OilType.Name,
                            TotalGST = s.SalesRegister.TotalGST,
                            InvoiceId = s.SalesRegister.InvoiceId,
                            BillNumber = s.SalesRegister.InvoiceNumber,
                            BillDate = s.SalesRegister.InvoiceDate,
                            NetValue = s.SalesRegister.TotalAmount
                        }).ToList();



                if (invoiceDetailsContextList != null && invoiceDetailsContextList.Any())
                {
                    var invoiceContext = invoiceDetailsContextList.FirstOrDefault();
                    invoiceDetailsContextList.FirstOrDefault();
                    invoiceDetailsOutputDto.InvoiceId = invoiceContext.InvoiceId;
                    invoiceDetailsOutputDto.InvoiceNumber = invoiceContext.BillNumber;
                    invoiceDetailsOutputDto.InvoiceDate = Convert.ToDateTime(invoiceContext.BillDate);
                    invoiceDetailsOutputDto.TotalInvoiceValue = Convert.ToDecimal(invoiceContext.NetValue);
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
                    //var invoicedetailContext = _emamiContext.InvoiceDetails.AsNoTracking()
                    //    .Join(_emamiContext.SalesRegister.AsNoTracking(), invd => invd.InvoiceId, sr => sr.InvoiceId, (invd, sr) => new { InvoiceDetails = invd, SalesRegister = sr })
                    //    .Where(_ => _.InvoiceDetails.InvoiceId == inputDto.Id).ToList();

                    decimal TotalInvoiceQuantity = 0;
                    foreach (var item in invoiceDetailsContextList)
                    {
                        var InvoiceSKUDetails = new InvoiceSKUDetailsOutputDto()
                        {
                            OilTypeId = item.OilTypeId,
                            SkuId = item.SkuId,
                            Quantity = item.QuantityMT,
                            //QuantityInCase = _resultService.ConvertCasetoMetricTon(item.ActualBilledQuantity, item.SkuId),
                            QuantityInCase = item.QuantityCase,
                            QunatityPrice = Convert.ToDecimal(item.NetValue),
                            OilType = item.OilType, // _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == item.InvoiceDetails.OilTypeId).Name,
                            sku = item.Sku // _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == item.InvoiceDetails.SkuId).SkuName,
                        };
                        TotalInvoiceQuantity = TotalInvoiceQuantity + item.QuantityMT;
                        invoiceDetailsOutputDto.InvoiceSKUDetails.Add(InvoiceSKUDetails);
                    }
                    invoiceDetailsOutputDto.InvoiceQuantity = TotalInvoiceQuantity;

                }
                #endregion
                return SucessResult(invoiceDetailsOutputDto);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }
        public ResultDto DueForTomorrowList(LoginUserIdDto inputDto)
        {
            _methodName = "DueForTomorrowList";
            var dashboardDetailsForPendingAndOverDueOutputDto = new DashboardDetailsForPendingAndOverDueOutputDto();
            if (inputDto == null)
            {
                return NotFoundResult();
            }
            try
            {
                var dealerlist = new List<long>();
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                if (inputDto.DealerIds == null)
                {
                    dealerlist = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                  join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                                  where ucm.UserId == inputDto.LoginUserId
                                  select ucm.CustomerId).Distinct().ToList();
                }
                else
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
                                var userContext = UserContextData.FirstOrDefault(_ => _.Id == item.UserId);
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
                                var userContext = UserContextData.FirstOrDefault(_ => _.Id == item.UserId);
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
                                var userContext = UserContextData.FirstOrDefault(_ => _.Id == item.UserId);
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
                                var userContext = UserContextData.FirstOrDefault(_ => _.Id == item.UserId);
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
                //        DashboardSaudaDetails.InvoiceValue = userCreditMasterContext.Overdue;
                //        TotalQuantity = TotalQuantity + invoicedetailsContext.Sum(_ => _.ActualBilledQuantity);
                //        TotalInvoiceValue = TotalInvoiceValue + userCreditMasterContext.Overdue;
                //        objdetailoutput.DashboardSalesDetails.Add(DashboardSaudaDetails);
                //    }
                //    else
                //    {
                //        if (detail.InvoiceDueDate != null && (detail.InvoiceDueDate.Value.Date == currentDate.Date || detail.InvoiceDueDate.Value.Date == currentDate.Date.AddDays(1)))
                //        {
                //            DashboardSaudaDetails.StatusId = (int)DTO.Enums.DueStatus.PendingDue;
                //            DashboardSaudaDetails.StatusName = UtilityHelper.GetEnumDescription(DTO.Enums.DueStatus.PendingDue);
                //            DashboardSaudaDetails.InvoiceValue = userCreditMasterContext.TomorrowsDue;
                //            TotalQuantity = TotalQuantity + invoicedetailsContext.Sum(_ => _.ActualBilledQuantity);
                //            TotalInvoiceValue = TotalInvoiceValue + userCreditMasterContext.TomorrowsDue;
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
                            // DealerTypeId = DealerTypeId,
                            Incoterms1 = IncotermsType,
                            PlantId = item.PlantId,
                            DealerLocationId = item.DealerLocationId,
                            ValidFromDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                            ValidToDate = DateHelper.UtcToIndia(DateTime.UtcNow).AddDays(Convert.ToDouble(dealerContext.SaudaValidityPeriod > 0 ? dealerContext.SaudaValidityPeriod : Config.DefaultSaudaValidity)),
                            StatusId = statusId,
                            // SaudaStatusId = (int)DTO.Enums.SaudaStatus.NotReleased,
                            Incoterms2 = item.IncotermsId,
                            BrokerId = inputDto.BrokerId,
                            //   CustomerPONumber = dealerContext.Code + DateHelper.UtcToIndia(DateTime.UtcNow),
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
                        var smsMessage = smsTemplate.Template.Replace(Constants.ReplaceMainContent, replaceSmsTemplate);
                        amazonNotificationService.SendMessage(smsMessage, CreatedByUser.MobileNumber, smsTemplate.SMSTemplateID);
                        amazonNotificationService.SendMessage(smsMessage, User.MobileNumber, smsTemplate.SMSTemplateID);
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
                IQueryable<Ticker> tickerContextList = _emamiContext.Ticker.AsNoTracking().Where(_ => DbFunctions.TruncateTime(_.TickerDate) == DbFunctions.TruncateTime(currentDate) && _.IsActive);
                if (tickerContextList != null && tickerContextList.Any())
                {
                    todayTickerListDto = tickerContextList.Select(_ => new TodayTickerListDto
                    {
                        Content = _.Content,
                        ColorCode = _.ColorCode,
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

                //if (inputDto.IncotermId == 0)
                //{
                //    return _resultService.ErrorMessage(Constants.IncotermsMissing);
                //}

                //if (inputDto.FrieghtRouteId == 0)
                //{
                //    return _resultService.ErrorMessage(Constants.FrieghtRouteMissing);
                //}

                var userContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var loadQuantity = Constants.DefaultLoadQuantity;
                var cityId = userContext.CityId;
                var stateId = userContext.StateId;
                var pricingContext = new List<Pricing>();
                var outputgroubyList = new List<Pricing>();
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                outputgroubyList = _emamiContext.Pricing.AsNoTracking().ToList();


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
                                         OilTypeId = dptgrp.First(y => y.Id == topsal).OilTypeId,
                                         Id = dptgrp.First(y => y.Id == topsal).Id,
                                         PlantId = dptgrp.First(y => y.Id == topsal).PlantId,
                                         Price = dptgrp.First(y => y.Id == topsal).Price,
                                         CreatedBy = dptgrp.First(y => y.Id == topsal).CreatedBy,
                                         CreatedDate = dptgrp.First(y => y.Id == topsal).CreatedDate,
                                         ModifiedBy = dptgrp.First(y => y.Id == topsal).ModifiedBy,
                                         ModifiedDate = dptgrp.First(y => y.Id == topsal).ModifiedDate,
                                     };

                pricingContext = RecentPricings.ToList();

                var outputList = new List<DailyRateOutputDto>();
                foreach (var pricing in pricingContext)
                {
                    var plantDepotId = 0L;
                    var plantDepotName = string.Empty;
                    var finalPrice = (decimal)0;
                    finalPrice = pricing.Price;

                    if (finalPrice > 0)
                    {
                        var finalRate = new DailyRateOutputDto
                        {
                            SkuId = pricing.SkuId,
                            SkuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == pricing.SkuId) != null ? _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == pricing.SkuId).SkuName : string.Empty,
                            FinalPrice = finalPrice,
                            //PlantDepotId = plantDepotId,
                            //PlantDepotName = plantDepotName
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


        /// Method to get daily rate
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
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

                var userContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.FirstOrDefault(_ => _.UserId == userContext.Id);

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

                var pricingContext = new List<Pricing>();
                var outputgroubyList = new List<TodayPricing>();
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                outputgroubyList = _emamiContext.TodayPricing.AsNoTracking()
                    .Join(_emamiContext.Skus.AsNoTracking(), t => t.SkuId , s=> s.Id , (t,s) => new { t ,s})
                    .Where(_ =>
                _.t.PlantId == inputDto.PlantId && _.t.SkuId != 0 && (inputDto.OilTypeId > 0 ? _.t.OilTypeId == inputDto.OilTypeId : _.t.OilTypeId > 0) && (DbFunctions.TruncateTime(currentDate) >=  DbFunctions.TruncateTime(_.t.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.t.ValidTo)) &&
                (inputDto.SalesOrganizationId > 0 ? _.t.SalesOrganizationId == inputDto.SalesOrganizationId : _.t.SalesOrganizationId > 0) && (inputDto.DistributionChannelId > 0 ? _.t.DistributionChannelId == inputDto.DistributionChannelId : _.t.DistributionChannelId > 0) && (inputDto.DivisionId > 0 ? _.t.DivisionId == inputDto.DivisionId : _.t.DivisionId > 0) && _.s.IsActive)
                    .Select(p => p.t).OrderByDescending(_ => _.Id).ToList();
                if (outputgroubyList == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                var RecentPricings = from e in outputgroubyList
                                     join ud in divisionslogieduser on new { SalesOrganizationId=e.SalesOrganizationId,DistributionChannelId=e.DistributionChannelId,DivisionId=e.DivisionId}
                                     equals new { SalesOrganizationId=ud.SalesOrganizationId,DistributionChannelId=ud.DistributionChannelId,DivisionId=ud.DivisionId}
                                     group e by new
                                     {
                                         e.SkuId
                                     } into dptgrp
                                     let topsal = dptgrp.Max(x => x.Id)
                                     select new Pricing
                                     {
                                         Id = dptgrp.First(y => y.Id == topsal).Id,
                                         SkuId = dptgrp.First(y => y.Id == topsal).SkuId,
                                         Price = dptgrp.First(y => y.Id == topsal).Price,
                                     };

                var finalOutputDto = new List<Pricing>();
                var SkuDistinct = from a in RecentPricings.ToList()
                                  group a by new
                                  {
                                      a.SkuId,
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
                                                select a).ToList();

                    if (RecentPricingContext != null && RecentPricingContext.Any())
                    {
                        finalOutputDto.AddRange(RecentPricingContext.Where(_ => _.SkuId == item.SkuId
                        ).ToList());
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
                                        .GroupBy(p => new { p.SkuId, p.SkuName, p.FinalPrice })
                                        .Select(g => g.First())
                                        .ToList();
                }

                var lineIds = userContext.LineId != null ? userContext.LineId?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList() : null;

                if (lineIds != null && lineIds.Any())
                {
                    List<long> mappingSkuIds = new List<long>();

                    foreach (var id in lineIds.Distinct())
                    {
                        if (_emamiContext.Skus.Where(_ => _.LineId != null && _.LineId != string.Empty).Count() > 0)
                        {
                            var skuContextList = _emamiContext.Skus.Where(_ => _.LineId != null && _.LineId != string.Empty).ToList();
                            var skuIds = skuContextList.Where(_ => _.LineId.Split(',').ToList().Contains(id)).Select(_ => _.Id).ToList();
                            mappingSkuIds.AddRange(skuIds);
                        }
                    }

                    if (mappingSkuIds != null && mappingSkuIds.Any())
                    {
                        FinaloutputList = FinaloutputList.Where(_ => mappingSkuIds.Distinct().Contains(_.SkuId)).ToList();
                    }
                    //else
                    //{
                    //    FinaloutputList = new List<DailyRateOutputDto>();
                    //}
                }
                //else
                //{
                //    FinaloutputList = new List<DailyRateOutputDto>();
                //}

                //if (userRoleContext.RoleId == (int)DTO.Enums.Role.StateTrader)
                //{
                //    List<long> mappingLineIds = new List<long>();
                //    List<long> mappingSkuIds = new List<long>();

                //    var dealerList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == userContext.Id).Select(_ => _.CustomerId).ToList();

                //    if (dealerList != null && dealerList.Any())
                //    {
                //        foreach (var dealer in dealerList)
                //        {
                //            var lineId = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == dealer).Select(_ => _.LineId).FirstOrDefault();
                //            var lineIds = lineId?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                //            if (lineIds != null)
                //            {
                //                foreach (var id in lineIds.Distinct())
                //                {
                //                    if (_emamiContext.Skus.Where(_ => _.LineId != null && _.LineId != string.Empty).Count() > 0)
                //                    {
                //                        var skuContextList = _emamiContext.Skus.Where(_ => _.LineId != null && _.LineId != string.Empty).ToList();
                //                        var skuIds = skuContextList.Where(_ => _.LineId.Split(',').ToList().Contains(id)).Select(_ => _.Id).ToList();
                //                        mappingSkuIds.AddRange(skuIds);
                //                    }
                //                }
                //            }
                //        }

                //        if (mappingSkuIds != null)
                //        {
                //            FinaloutputList = FinaloutputList.Where(_ => mappingSkuIds.Distinct().Contains(_.SkuId)).ToList();
                //        }
                //    }
                //}
                //else if(userRoleContext.RoleId == (int)DTO.Enums.Role.Dealer)
                //{
                //    var lineIds = userContext.LineId != null ? userContext.LineId?.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList() : null;

                //    if (lineIds != null && lineIds.Any())
                //    {
                //        List<long> mappingSkuIds = new List<long>();

                //        foreach (var id in lineIds.Distinct())
                //        {
                //            if (_emamiContext.Skus.Where(_ => _.LineId != null && _.LineId != string.Empty).Count() > 0)
                //            {
                //                var skuContextList = _emamiContext.Skus.Where(_ => _.LineId != null && _.LineId != string.Empty).ToList();
                //                var skuIds = skuContextList.Where(_ => _.LineId.Split(',').ToList().Contains(id)).Select(_ => _.Id).ToList();
                //                mappingSkuIds.AddRange(skuIds);
                //            }
                //        }

                //        if (mappingSkuIds != null)
                //        {
                //            FinaloutputList = FinaloutputList.Where(_ => mappingSkuIds.Distinct().Contains(_.SkuId)).ToList();
                //        }
                //    }
                //}       

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
        public ResultDto GetDailyRateForManager(DailyRateInputDto inputDto)
        {
            _methodName = "GetDailyRateForManager";
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
                if (inputDto.PlantId == 0)
                {
                    return _resultService.ErrorMessage(Constants.PlantMissing);
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
                                         _.OilTypeId == inputDto.OilTypeId
                                         && _.PlantId == inputDto.PlantId).ToList();
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
                var userDetails = _emamiContext.Users.AsNoTracking().Where(_ => _.Id == inputDto.LoginUserId)
                    .Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur }).FirstOrDefault();
                if (userDetails != null)
                {
                    if (userDetails.ur.RoleId == (int)DTO.Enums.Role.StateTrader)
                    {
                        //var LoginuserContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == user.LoginUserId);
                        userMasterDto = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                         join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                                         where ucm.UserId == userDetails.u.Id && u.IsActive
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
                                       where depotMapping.UserId == user.Id && depot.IsActive 
                                       select new DepotDto
                                       {
                                           Id = depot.Id,
                                           Name = depot.Name + "-" + depot.Code,
                                           Code = depot.Code,
                                           IsPlant = depot.IsPlant,
                                           IsActive = depot.IsActive
                                       }).ToList();
                            if (depotList != null && depotList.Any())
                            {
                                PlantDepotList.AddRange(depotList);
                            }
                        }
                        
                    }
                    else if (userDetails.ur.RoleId == (int)DTO.Enums.Role.Dealer)
                    {
                        PlantDepotList =
                              (from depot in _emamiContext.Depots.AsNoTracking()
                               join depotMapping in _emamiContext.UserDepotMapping.AsNoTracking() on depot.Id equals depotMapping.DepotId
                               where depotMapping.UserId == userDetails.u.Id && depot.IsActive 
                               select new DepotDto
                               {
                                   Id = depot.Id,
                                   Name = depot.Name + "-" + depot.Code,
                                   Code = depot.Code,
                                   IsPlant = depot.IsPlant,
                                   IsActive = depot.IsActive
                               }).ToList();
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

                return SucessResult(list);
            }
            catch (Exception exception)
            {
                return ExceptionResult(exception);
            }
        }


        public ResultDto PackwiseInvoicesByDealer(DashboardSaudaDetailsByDealersInputDto inputDto)
        {
            var dashboardDetailsByDealersOutputDto = new DashboardSalesOutputDto();
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
                if (inputDto.DealerId == 0)
                {
                    return _resultService.ErrorMessage(Constants.DealerMissing);
                }
                var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId && _.IsActive);
                if (dealerContext == null)
                {
                    return _resultService.ErrorMessage(Constants.DealerNotFound);
                }
                var dealerRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == dealerContext.Id);
                if (dealerRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.DealerNotFound);
                }
                #region OldCode
                //if (invoiceListContext != null && invoiceListContext.Any())
                //{
                //    if (inputDto.IsBulkPack)
                //    {
                //        invoiceListContext = invoiceListContext.AsNoTracking().Where(_ => _.s.PackGroupId == (int)DTO.Enums.PackGroupType.BulkPacking);
                //    }
                //    else
                //    {
                //        invoiceListContext = invoiceListContext.AsNoTracking().Where(_ => _.s.PackGroupId == (int)DTO.Enums.PackGroupType.ConsumerPacking);
                //    }
                //}
                //if (invoiceListContext != null && invoiceListContext.Any())
                //{
                //    dashboardDetailsByDealersOutputDto.DealerId = inputDto.DealerId;
                //    dashboardDetailsByDealersOutputDto.Dealer = invoiceListContext.FirstOrDefault().u.Name;
                //    var cityContext = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == invoiceListContext.FirstOrDefault().u.CityId);
                //    if (cityContext != null)
                //    {
                //        dashboardDetailsByDealersOutputDto.TownName = cityContext.CityName;
                //    }
                //    dashboardDetailsByDealersOutputDto.TotalQuantity = invoiceListContext.Select(_ => _.ivd.ActualBilledQuantity).DefaultIfEmpty(0).Sum();
                //    dashboardDetailsByDealersOutputDto.TotalBookedInvoiceValue = invoiceListContext.Select(_ => _.ivd.SKUInvoiceTax).DefaultIfEmpty(0).Sum();
                //    dashboardDetailsByDealersOutputDto.IsBulkPack = inputDto.IsBulkPack;
                //    dashboardDetailsByDealersOutputDto.DashboardSalesDetails = invoiceListContext.GroupBy(g => g.i.Id).Select(_ => new DashboardSalesDetailsOutputDto()
                //    {
                //        InvoiceId = _.FirstOrDefault().i.Id,
                //        InvoiceNumber = _.FirstOrDefault().i.BillingDocument,
                //        InvoiceDate = _.FirstOrDefault().i.InvoiceDate,
                //        TotalQuantity = _.Select(s => s.ivd.ActualBilledQuantity).DefaultIfEmpty(0).Sum(),
                //        InvoiceValue = _.Select(s => s.ivd.SKUInvoiceTax).DefaultIfEmpty(0).Sum(),
                //    }).ToList();
                //    //return _resultService.SuccessObject(dashboardDetailsByDealersOutputDto);
                //}

                #region 27-12-2019
                //var invoiceListContext = _emamiContext.Invoices.AsNoTracking()
                //    .Join(_emamiContext.InvoiceDetails.AsNoTracking(), inv => inv.Id, invd => invd.InvoiceId, (inv, invd) => new { Invoices = inv, InvoiceDetails = invd })
                //      .Join(_emamiContext.SalesRegister.AsNoTracking(), invd => invd.InvoiceDetails.InvoiceId, sr => sr.InvoiceId, (invd, sr) => new { Invoices = invd.Invoices, InvoiceDetails = invd.InvoiceDetails, SalesRegister = sr })
                //         .Join(_emamiContext.Users.AsNoTracking(), ivdi => ivdi.Invoices.UserId, u => u.Id, (ivdi, u) => new { Invoices = ivdi.Invoices, InvoiceDetails = ivdi.InvoiceDetails, SalesRegister = ivdi.SalesRegister, Users = u })
                //         .Join(_emamiContext.Skus.AsNoTracking(), ivdiu => ivdiu.InvoiceDetails.SkuId, s => s.Id, (ivdiu, s) => new { Invoices = ivdiu.Invoices, InvoiceDetails = ivdiu.InvoiceDetails, SalesRegister = ivdiu.SalesRegister, Users = ivdiu.Users, Skus = s })
                //         .Where(_ => DbFunctions.TruncateTime(_.Invoices.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                //         && DbFunctions.TruncateTime(_.Invoices.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && _.Invoices.UserId == inputDto.DealerId
                //         && _.InvoiceDetails != null
                //         && _.Invoices != null
                //         && _.SalesRegister != null
                //         && _.Users != null);

                //if (invoiceListContext != null && invoiceListContext.Any())
                //{
                //    if (inputDto.IsBulkPack)
                //    {
                //        invoiceListContext = invoiceListContext.AsNoTracking().Where(_ => _.Skus.PackGroupId == (int)DTO.Enums.PackGroupType.BulkPacking);
                //    }
                //    else
                //    {
                //        invoiceListContext = invoiceListContext.AsNoTracking().Where(_ => _.Skus.PackGroupId == (int)DTO.Enums.PackGroupType.ConsumerPacking);
                //    }
                //}
                //if (invoiceListContext != null && invoiceListContext.Any())
                //{
                //    dashboardDetailsByDealersOutputDto.DealerId = inputDto.DealerId;
                //    dashboardDetailsByDealersOutputDto.Dealer = invoiceListContext.FirstOrDefault().Users.Name;
                //    var cityContext = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == invoiceListContext.FirstOrDefault().Users.CityId);
                //    if (cityContext != null)
                //    {
                //        dashboardDetailsByDealersOutputDto.TownName = cityContext.CityName;
                //    }
                //    dashboardDetailsByDealersOutputDto.TotalQuantity = invoiceListContext.Select(_ => _.SalesRegister.QuantityMT).DefaultIfEmpty(0).Sum();
                //    dashboardDetailsByDealersOutputDto.TotalBookedInvoiceValue = invoiceListContext.Select(_ => _.InvoiceDetails.SKUInvoiceTax).DefaultIfEmpty(0).Sum();
                //    dashboardDetailsByDealersOutputDto.IsBulkPack = inputDto.IsBulkPack;
                //    dashboardDetailsByDealersOutputDto.DashboardSalesDetails = invoiceListContext.GroupBy(g => g.Invoices.Id).Select(_ => new DashboardSalesDetailsOutputDto()
                //    {
                //        InvoiceId = _.FirstOrDefault().Invoices.Id,
                //        InvoiceNumber = _.FirstOrDefault().Invoices.BillingDocument,
                //        InvoiceDate = _.FirstOrDefault().Invoices.InvoiceDate,
                //        TotalQuantity = _.Select(s => s.SalesRegister.QuantityMT).DefaultIfEmpty(0).Sum(),
                //        InvoiceValue = _.Select(s => s.InvoiceDetails.SKUInvoiceTax).DefaultIfEmpty(0).Sum(),
                //    }).ToList();
                //    //return _resultService.SuccessObject(dashboardDetailsByDealersOutputDto);
                //} 
                #endregion

                //var invoiceListContext = _emamiContext.Invoices.AsNoTracking()
                //   .Join(_emamiContext.InvoiceDetails.AsNoTracking(), inv => inv.Id, invd => invd.InvoiceId, (inv, invd) => new { Invoices = inv, InvoiceDetails = invd })
                //     .Join(_emamiContext.SalesRegister.AsNoTracking(), invd => invd.InvoiceDetails.InvoiceId, sr => sr.InvoiceId, (invd, sr) => new { Invoices = invd.Invoices, InvoiceDetails = invd.InvoiceDetails, SalesRegister = sr })
                //        .Join(_emamiContext.Users.AsNoTracking(), ivdi => ivdi.Invoices.UserId, u => u.Id, (ivdi, u) => new { Invoices = ivdi.Invoices, InvoiceDetails = ivdi.InvoiceDetails, SalesRegister = ivdi.SalesRegister, Users = u })
                //        .Join(_emamiContext.Skus.AsNoTracking(), ivdiu => ivdiu.InvoiceDetails.SkuId, s => s.Id, (ivdiu, s) => new { Invoices = ivdiu.Invoices, InvoiceDetails = ivdiu.InvoiceDetails, SalesRegister = ivdiu.SalesRegister, Users = ivdiu.Users, Skus = s })
                //        .Where(_ => DbFunctions.TruncateTime(_.Invoices.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                //        && DbFunctions.TruncateTime(_.Invoices.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && _.Invoices.UserId == inputDto.DealerId
                //        && _.InvoiceDetails != null
                //        && _.Invoices != null
                //        && _.SalesRegister != null
                //        && _.Users != null);
                #endregion

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

                IEnumerable<SalesRegisterDashboardDto> invoiceListContext = new List<SalesRegisterDashboardDto>();
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    var sqlQuery = @"Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                            insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                            select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

                            
                            select 
                            sku.PackGroupId,
                            sr.QuantityMT,
                            u.Name,
                            u.CityId,
                            sr.TotalGST,
                            sr.Id as InvoiceId,
                            sr.InvoiceDate as BillingDate,
                            sr.InvoiceNumber as BillNumber,
                            sr.TotalAmount
                            from SalesRegisters sr with(NOLOCK)
                            join Skus sku on sr.MaterialCode=sku.SkuCode and sr.SalesOrganizationId=sku.SalesOrganizationId
                            and sr.DistributionChannelId=sku.DistributionChannelId and sr.DivisionId=sku.DivisionId
                            join Users u on sr.CustomerCode=u.Code
                            join #UserDivision ud on ud.SalesOrganizationId=sr.SalesOrganizationId
                            and ud.DistributionChannelId=sr.DistributionChannelId and ud.DivisionId=sr.DivisionId
                            where Cast(sr.InvoiceDate as date) >= Cast(@FromDate as date)
                            and Cast(sr.InvoiceDate as date) <= Cast(@ToDate as date)
                            and u.Id=@CustomerId
                        ";
                    invoiceListContext = conn.Query<SalesRegisterDashboardDto>(sqlQuery, new
                    {
                        UserId = inputDto.LoginUserId,
                        FromDate = inputDto.FromDate,
                        ToDate = inputDto.ToDate,
                        CustomerId = inputDto.DealerId
                    });

                }
                
                //var invoiceListContext = (from s in _emamiContext.SalesRegister.AsNoTracking()
                //              join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                //              join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                //              join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                //                            equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                //                            where DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                //        && DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                //        && u.Id == inputDto.DealerId
                //        && s.SalesOrganizationId == sku.SalesOrganizationId && s.DistributionChannelId == sku.DistributionChannelId
                //        && s.DivisionId == sku.DivisionId
                //        && s.SkuId > 0
                //        select new
                //        {
                //            PackGroupId = sku.PackGroupId,
                //            QuantityMT = s.QuantityMT,
                //            Name = u.Name,
                //            CityId = u.CityId,
                //            TotalGST = s.TotalGST,
                //            InvoiceId = s.Id,
                //            BillingDate = s.InvoiceDate,
                //            BillNumber = s.InvoiceNumber,
                //            TotalAmount = s.TotalAmount
                //        }
                //              ).ToList();
                
                //var invoiceListContext = _emamiContext.SalesRegister.AsNoTracking()
                //        .Join(_emamiContext.Skus.AsNoTracking(), sr => sr.MaterialCode, sk => sk.SkuCode, (sr, sk) => new { SalesRegister = sr, Sku = sk })
                //        .Join(_emamiContext.Users.AsNoTracking(), sr => sr.SalesRegister.CustomerCode, us => us.Code, (sr, us) => new { SalesRegister = sr.SalesRegister, Sku = sr.Sku, User = us })
                //        .Where(w => DbFunctions.TruncateTime(w.SalesRegister.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                //        && DbFunctions.TruncateTime(w.SalesRegister.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                //        && w.User.Id == inputDto.DealerId
                //        && w.SalesRegister.SalesOrganizationId == w.Sku.SalesOrganizationId && w.SalesRegister.DistributionChannelId == w.Sku.DistributionChannelId
                //        && w.SalesRegister.DivisionId == w.Sku.DivisionId)
                //        //&& w.User.DivisionId == userContext.DivisionId
                //        .Select(s => new
                //        {
                //            PackGroupId = s.Sku.PackGroupId,
                //            QuantityMT = s.SalesRegister.QuantityMT,
                //            Name = s.User.Name,
                //            CityId = s.User.CityId,
                //            TotalGST = s.SalesRegister.TotalGST,
                //            InvoiceId = s.SalesRegister.Id,
                //            BillingDate = s.SalesRegister.InvoiceDate,
                //            BillNumber = s.SalesRegister.InvoiceNumber,
                //            TotalAmount = s.SalesRegister.TotalAmount
                //        }).ToList();

                if (invoiceListContext != null && invoiceListContext.Any())
                {
                    if (inputDto.PackGroupId > 0)
                    {
                        invoiceListContext = invoiceListContext.Where(_ => _.PackGroupId == inputDto.PackGroupId).ToList();
                    }
                }
                if (invoiceListContext != null && invoiceListContext.Any())
                {
                    dashboardDetailsByDealersOutputDto.DealerId = inputDto.DealerId;
                    dashboardDetailsByDealersOutputDto.Dealer = invoiceListContext.FirstOrDefault().Name;
                    var CityId = invoiceListContext.FirstOrDefault().CityId;
                    var cityContext = _emamiContext.City.AsNoTracking().FirstOrDefault(_ => _.Id == CityId);
                    if (cityContext != null)
                    {
                        dashboardDetailsByDealersOutputDto.TownName = cityContext.CityName;
                    }
                    else
                    {
                        dashboardDetailsByDealersOutputDto.TownName = string.Empty;
                    }
                    dashboardDetailsByDealersOutputDto.TotalQuantity = invoiceListContext.Select(_ => _.QuantityMT).DefaultIfEmpty(0).Sum();
                    dashboardDetailsByDealersOutputDto.TotalBookedInvoiceValue = invoiceListContext.Select(_ => string.IsNullOrEmpty(_.TotalAmount) ? 0 : Convert.ToDecimal(_.TotalAmount)).DefaultIfEmpty(0).Sum();
                    //dashboardDetailsByDealersOutputDto.IsBulkPack = inputDto.IsBulkPack;

                    if(inputDto.IsPendingSauda)
                    {
                        var data = invoiceListContext.GroupBy(_ => _.BillingDate).Select(s => new { BillingDate = s.Key, Details = s });
                        dashboardDetailsByDealersOutputDto.DashboardSalesDetails
                            = data.Select(_ => new DashboardSalesDetailsOuterListDto()
                            {
                                InvoiceDate = _.BillingDate,
                                TotalInvoiceQuantity = _.Details.Sum(qty => qty.QuantityMT),
                                TotalInvoiceValue = _.Details.Sum(amt => string.IsNullOrEmpty(amt.TotalAmount) ? 0 : Convert.ToDecimal(amt.TotalAmount)),
                                InvoiceList = _.Details.Select(a => new InvoiceDetailsInnerListDto
                                {
                                    InvoiceDate = Convert.ToDateTime(a.BillingDate),
                                    InvoiceQuantity = a.QuantityMT,
                                    InvoiceId = a.InvoiceId,
                                    InvoiceValue = string.IsNullOrEmpty(a.TotalAmount) ? 0 : Convert.ToDecimal(a.TotalAmount),
                                    InvoiceNumber = a.BillNumber
                                }).ToList()
                            }).ToList();
                    }
                    else
                    {
                        var data = invoiceListContext.GroupBy(_ => _.BillNumber).Select(s => new { BillNumber = s.Key, Details = s });
                        dashboardDetailsByDealersOutputDto.DashboardSalesDetails
                            = data.Select(_ => new DashboardSalesDetailsOuterListDto()
                            {
                                InvoiceNumber = _.BillNumber,
                                TotalInvoiceQuantity = _.Details.Sum(qty => qty.QuantityMT),
                                TotalInvoiceValue = _.Details.Sum(amt => string.IsNullOrEmpty(amt.TotalAmount) ? 0 : Convert.ToDecimal(amt.TotalAmount)),
                                InvoiceList = _.Details.Select(a => new InvoiceDetailsInnerListDto
                                {
                                    InvoiceDate = Convert.ToDateTime(a.BillingDate),
                                    InvoiceQuantity = a.QuantityMT,
                                    InvoiceId = a.InvoiceId,
                                    InvoiceValue = string.IsNullOrEmpty(a.TotalAmount) ? 0 : Convert.ToDecimal(a.TotalAmount),
                                    InvoiceNumber = a.BillNumber
                                }).ToList()
                            }).ToList();
                    }
                   
                    //return _resultService.SuccessObject(dashboardDetailsByDealersOutputDto);
                }

                return _resultService.SuccessObject(dashboardDetailsByDealersOutputDto);
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

                var roleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                if (roleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                #region OldCode

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

                //var invoiceContext = _emamiContext.InvoiceDetails.AsNoTracking()
                //       .Join(_emamiContext.Invoices.AsNoTracking(), ivd => ivd.InvoiceId, i => i.Id, (ivd, i) => new { ivd, i })
                //       .Join(_emamiContext.Skus.AsNoTracking(), ivdi => ivdi.ivd.SkuId, s => s.Id, (ivdi, s) => new { ivdi.ivd, ivdi.i, s })
                //       .Join(_emamiContext.OilTypes.AsNoTracking(), ivdis => ivdis.ivd.OilTypeId, o => o.Id, (ivdis, o) => new { ivdis.ivd, ivdis.i, ivdis.s, o })
                //       .Join(_emamiContext.SalesRegister.AsNoTracking(), ivdis => ivdis.ivd.InvoiceId, sr => sr.InvoiceId, (ivdis, sr) => new { ivdis.ivd, ivdis.i, ivdis.s, ivdis.o, SalesRegister = sr })
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
                //    invoiceDetailsOutputDto.InvoiceQuantity = invoiceContext.Select(_ => _.SalesRegister.QuantityMT).DefaultIfEmpty(0).Sum();

                //    invoiceDetailsOutputDto.InvoiceSKUDetails = invoiceContext.Select(_ => new InvoiceSKUDetailsOutputDto()
                //    {
                //        OilTypeId = _.ivd.OilTypeId,
                //        SkuId = _.ivd.SkuId,
                //        Quantity = _.SalesRegister.QuantityMT,
                //        QuantityInCase = _.ivd.QuantityInCase,
                //        QunatityPrice = _.ivd.SKUInvoiceTax,
                //        OilType = _.o.Name,
                //        sku = _.s.SkuName,
                //    }).ToList();

                #endregion

                #region 27-12-2019
                //var invoiceContext = _emamiContext.InvoiceDetails.AsNoTracking()
                //       .Join(_emamiContext.Invoices.AsNoTracking(), ivd => ivd.InvoiceId, i => i.Id, (ivd, i) => new { ivd, i })
                //       .Join(_emamiContext.Skus.AsNoTracking(), ivdi => ivdi.ivd.SkuId, s => s.Id, (ivdi, s) => new { ivdi.ivd, ivdi.i, s })
                //       .Join(_emamiContext.OilTypes.AsNoTracking(), ivdis => ivdis.ivd.OilTypeId, o => o.Id, (ivdis, o) => new { ivdis.ivd, ivdis.i, ivdis.s, o })
                //       .Join(_emamiContext.SalesRegister.AsNoTracking(), ivdis => ivdis.ivd.InvoiceId, sr => sr.InvoiceId, (ivdis, sr) => new { ivdis.ivd, ivdis.i, ivdis.s, ivdis.o, SalesRegister = sr })
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
                //    invoiceDetailsOutputDto.InvoiceQuantity = invoiceContext.Select(_ => _.SalesRegister.QuantityMT).DefaultIfEmpty(0).Sum();

                //    invoiceDetailsOutputDto.InvoiceSKUDetails = invoiceContext.Select(_ => new InvoiceSKUDetailsOutputDto()
                //    {
                //        OilTypeId = _.ivd.OilTypeId,
                //        SkuId = _.ivd.SkuId,
                //        Quantity = _.SalesRegister.QuantityMT,
                //        QuantityInCase = _.ivd.QuantityInCase,
                //        QunatityPrice = _.ivd.SKUInvoiceTax,
                //        OilType = _.o.Name,
                //        sku = _.s.SkuName,
                //    }).ToList(); 
                #endregion

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

                var invoiceContext = (from s in _emamiContext.SalesRegister.AsNoTracking()
                                      join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                                      join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                                      join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                                            equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                      where s.Id == inputDto.Id
                        && s.SalesOrganizationId == sku.SalesOrganizationId 
                        && s.DistributionChannelId == sku.DistributionChannelId
                        && s.DivisionId == sku.DivisionId
                        && s.SkuId > 0
                        select new
                        {
                            PackGroupId = sku.PackGroupId,
                            QuantityMT = s.QuantityMT,
                            QuantityInCase = s.QuantityCase,
                            OilTypeId = sku.OilTypeId,
                            OilType = sku.OilType.Name,
                            SkuId = sku.Id,
                            Sku = sku.SkuName,
                            BillNumber = s.InvoiceNumber,
                            BillDate = s.InvoiceDate,
                            InvoiceId = s.InvoiceId,
                            TotalAmount = s.TotalAmount
                        }
                                      ).ToList();

                //var invoiceContext = _emamiContext.SalesRegister.AsNoTracking()
                //        .Join(_emamiContext.Skus.AsNoTracking(), sr => sr.MaterialCode, sk => sk.SkuCode, (sr, sk) => new { SalesRegister = sr, Sku = sk })
                //        .Join(_emamiContext.Users.AsNoTracking(), sr => sr.SalesRegister.CustomerCode, us => us.Code, (sr, us) => new { SalesRegister = sr.SalesRegister, Sku = sr.Sku, User = us })
                //        .Join(_emamiContext.UserRoles.AsNoTracking(), us => us.User.Id, userroles => userroles.UserId, (us, userroles) => new { SalesRegister = us.SalesRegister, Sku = us.Sku, User = us, UserRoles = userroles })
                //        .Where(w => w.SalesRegister.Id == inputDto.Id
                //        //&& w.User.User.DivisionId == userContext.DivisionId 
                //        && w.UserRoles.RoleId != (int)DTO.Enums.Role.ShipToParty
                //        && w.SalesRegister.SalesOrganizationId == w.Sku.SalesOrganizationId && w.SalesRegister.DistributionChannelId == w.Sku.DistributionChannelId
                //        && w.SalesRegister.DivisionId == w.Sku.DivisionId)
                //        .Select(s => new
                //        {
                //            PackGroupId = s.Sku.PackGroupId,
                //            QuantityMT = s.SalesRegister.QuantityMT,
                //            QuantityInCase = s.SalesRegister.QuantityCase,
                //            OilTypeId = s.Sku.OilTypeId,
                //            OilType = s.Sku.OilType.Name,
                //            SkuId = s.Sku.Id,
                //            Sku = s.Sku.SkuName,
                //            BillNumber = s.SalesRegister.InvoiceNumber,
                //            BillDate = s.SalesRegister.InvoiceDate,
                //            InvoiceId = s.SalesRegister.InvoiceId,
                //            TotalAmount = s.SalesRegister.TotalAmount
                //        }).ToList();

                if (invoiceContext != null && invoiceContext.Any())
                {
                    if (inputDto.PackGroupId > 0)
                    {
                        invoiceContext = invoiceContext.Where(_ => _.PackGroupId == inputDto.PackGroupId).ToList();
                    }
                }
                if (invoiceContext != null && invoiceContext.Any())
                {
                    invoiceDetailsOutputDto.InvoiceId = invoiceContext.FirstOrDefault().InvoiceId;
                    invoiceDetailsOutputDto.InvoiceNumber = invoiceContext.FirstOrDefault().BillNumber;
                    invoiceDetailsOutputDto.InvoiceDate = Convert.ToDateTime(invoiceContext.FirstOrDefault().BillDate);
                    invoiceDetailsOutputDto.TotalInvoiceValue = invoiceContext.Select(_ => string.IsNullOrEmpty(_.TotalAmount) ? 0 : Convert.ToDecimal(_.TotalAmount)).DefaultIfEmpty(0).Sum(); //string.IsNullOrEmpty(_.TotalGST) ? 0 : Convert.ToDecimal(_.TotalGST)).DefaultIfEmpty(0).Sum();
                    invoiceDetailsOutputDto.InvoiceQuantity = invoiceContext.Select(_ => _.QuantityMT).DefaultIfEmpty(0).Sum();

                    invoiceDetailsOutputDto.InvoiceSKUDetails = invoiceContext.Select(_ => new InvoiceSKUDetailsOutputDto()
                    {
                        OilTypeId = (int)_.OilTypeId,
                        SkuId = _.SkuId,
                        Quantity = _.QuantityMT,
                        QuantityInCase = _.QuantityInCase,
                        QunatityPrice = string.IsNullOrEmpty(_.TotalAmount) ? 0 : Convert.ToDecimal(_.TotalAmount), //  _.TotalGST,
                        OilType = _.OilType,
                        sku = _.Sku,
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


        /// <summary>
        /// Method to get quantity allocation list
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto GetDailySFQuantityAllocation(QuantityAllocationInputDto inputDto)
        {
            _methodName = "GetDailySFQuantityAllocation";
            var resultDto = new ResultDto();
            var outputDto = new List<QuantityAllocationOutputDto>();
            try
            {
                //var loginUserVerticalId = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId)?.DivisionId;
                var userDivisionMappings = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.Id == inputDto.LoginUserId).ToList();
                var divisionIds = userDivisionMappings.Select(_ => _.DivisionId).ToList();

                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var allocationList = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().
                    Where(_ => _.UserId == inputDto.LoginUserId && _.OilTypeId == inputDto.OilTypeId && _.ParentQuantityId != 0
                    && DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom)
                    && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && divisionIds.Contains(_.DivisionId)).ToList();

                if (allocationList == null || !allocationList.Any())
                    return _resultService.ErrorMessage(Constants.RecordNotFound);

                outputDto = allocationList.Select(c => new QuantityAllocationOutputDto
                {
                    Id = c.Id,
                    SkuId = c.SkuId,
                    SkuName = c.Sku.SkuName,
                    SkuCode = c.Sku.SkuCode,
                    Quantity = c.ActualDiscount
                }).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderBy(_ => _.SkuName).ToList() : outputDto;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.Exception;
                _logger.Error(message);
                return resultDto;
            }
        }

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
                // var roleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId = inputDto.LoginUserId);
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                List<long> dealersList = new List<long>();
                List<long> bdolist = new List<long>();

                

                if (inputDto.Dealers != null && inputDto.Dealers.Any())
                {
                    dealersList.AddRange(inputDto.Dealers);
                }
                else
                {
                    dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId).Select(_ => _.CustomerId).OrderBy(_ => _).ToList();
                }
                if (userRoleContext.RoleId == (int)DTO.Enums.Role.StateTrader)
                {
                    bdolist.Add(inputDto.LoginUserId);
                }
                if (inputDto.BDOs.IsAny())
                {
                    bdolist.AddRange(inputDto.BDOs);
                }
                else
                {
                    var bdoIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => dealersList.Contains(_.CustomerId)).Select(_ => _.UserId).ToList();
                    bdolist.AddRange(bdoIds);
                }
                if (inputDto.PlantId == 0)
                {
                    //plantIds = _emamiContext.Depots.AsNoTracking().Where(w => w.IsPlant).Select(s => s.Id).ToList();
                    //plantIds = _emamiContext.UserDepotMapping.AsNoTracking().Where(w => dealersList.Contains(w.UserId))
                    //    .Select(s => s.DepotId).Distinct().ToList();
                }
                else
                {
                    plantIds = _emamiContext.UserDepotMapping.AsNoTracking()
                          .Join(_emamiContext.Depots.AsNoTracking(), ud => ud.DepotId, d => d.Id, (ud, d) => new
                          {
                              UserDepot = ud,
                              Depot = d
                          })
                          .Where(w => w.Depot.Id == inputDto.PlantId
                          && dealersList.Contains(w.UserDepot.UserId))
                          .Select(s => s.Depot.Id).Distinct().ToList();
                    plantIds.Add(inputDto.PlantId);
                }
                plantIds = plantIds.OrderBy(_ => _).ToList();
                //IQueryable<UserCustomerMapping> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId);
                if (dealersList != null && dealersList.Any())
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
                        var sqlQuery = @"DECLARE @DealerTemp TABLE (DealerId BIGINT)
                        DECLARE @PlantTemp TABLE (DealerId BIGINT)
                        DECLARE @UserDivision TABLE (SalesOrganizationId BIGINT, DistributionChannelId BIGINT, DivisionId BIGINT)

                        insert into @UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                        select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

                        if(@DealerString!='')
                        begin
                        Insert Into @DealerTemp
                         Select Data From dbo.Split(@DealerString,',')
                        end
                        else
                        begin
                         insert into @DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
                         where UserId=@UserId
                        end

                        if(@PlantId > 0)
                        begin 
                         insert into @PlantTemp select DepotId from UserDepotMappings where UserId in (select DealerId from @DealerTemp) or DepotId=@PlantId
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
                        (o.Name+'-'+sorg.Code+'/'+dist.Code+'/'+div.Code) as OilType,
                        (sku.SkuName +'-'+sku.SkuCode) as SkuName,
                        sku.Id as SkuId,
                        state.StateName,
                        u.StateId as StateId
                        from Saudas s  with(NOLOCK)
                        join SaudaOrders so with(NOLOCK) on s.Id=so.SaudaId
                        join @UserDivision ud on ud.SalesOrganizationId=s.SalesOrganizationId
                        and ud.DistributionChannelId=s.DistributionChannelId and ud.DivisionId=s.DivisionId
                        join Skus sku on so.SkuId=sku.Id
                        join PackGroups p on sku.PackGroupId=p.Id
                        join OilTypes o on sku.OilTypeId=o.Id
                        join SalesOrganizations sorg on o.SalesOrganizationId=sorg.Id
                        join DistributionChannels dist on o.DistributionChannelId=dist.Id
                        join Divisions div on o.DivisionId=div.Id
                        join Users u on s.UserId=u.Id
                        join States as state on u.StateId = state.Id
                        where 
                        Cast(s.BiddingDate as date) >= Cast(@FromDate as date)
                        and Cast(s.BiddingDate as date) <= Cast(@ToDate as date)
                        and s.UserId in (select DealerId from @DealerTemp)
                        and ((@PlantId > 0 and so.PlantId in (select PlantId from @PlantTemp)) or @PlantId=0)
                        and so.StatusId !=3 --Rejected StatusId=3";
                        var modifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                        saudaContext = conn.Query<DailyBookedSaudaOutputDto>(sqlQuery, new
                        {
                            UserId = inputDto.LoginUserId,
                            PlantId = inputDto.PlantId,
                            DealerString = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.Dealers),
                            FromDate=inputDto.FromDate,
                            ToDate=inputDto.ToDate
                        }); 

                    }

                    //saudaContext = (from so in _emamiContext.SaudaOrders.AsNoTracking()
                    //                    join s in _emamiContext.Sauda.AsNoTracking() on so.SaudaId equals s.Id
                    //                    join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                    //                          equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                    //                    join sku in _emamiContext.Skus.AsNoTracking() on so.SkuId equals sku.Id
                    //                    join o in _emamiContext.OilTypes.AsNoTracking() on so.OilTypeId equals o.Id
                    //                    where so != null
                    //                    && s != null
                    //                    && sku != null
                    //                    && o != null
                    //                  //  && bdolist.Contains(s.BdoId)
                    //                    && DbFunctions.TruncateTime(so.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //     && DbFunctions.TruncateTime(so.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    //     && dealersList.Contains(s.UserId)
                    //     && (plantIds.Any() ?  plantIds.Contains(so.PlantId) : so.PlantId > 0)
                    //     && so.StatusId != (int)DTO.Enums.Status.Rejected
                    //                    select new { s, so, o, sku }
                    //                  ) ;

                    
                    //var saudaContext = _emamiContext.SaudaOrders.AsNoTracking()
                    //    .Join(_emamiContext.Sauda.AsNoTracking(), so => so.SaudaId, s => s.Id, (so, s) => new { so, s })
                    //    .Join(_emamiContext.Skus.AsNoTracking(), soi => soi.so.SkuId, sku => sku.Id, (soi, sku) => new { soi.so, soi.s, sku })
                    //    .Join(_emamiContext.OilTypes.AsNoTracking(), sos => sos.so.OilTypeId, o => o.Id, (sos, o) => new { sos.so, sos.s, sos.sku, o })
                    //   .Where(_ => _.so != null && _.s != null && _.sku != null && _.o != null
                    //   && DbFunctions.TruncateTime(_.so.CreatedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //   && DbFunctions.TruncateTime(_.so.CreatedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    //   && dealersList.Contains(_.s.UserId)
                    //   && plantIds.Contains(_.so.PlantId) 
                    //   && _.so.StatusId != (int)DTO.Enums.Status.Rejected
                    //   );

                    if (saudaContext != null)
                    {
                        if (inputDto.Dealers != null && inputDto.Dealers.Any())
                        {
                            saudaContext = saudaContext.Where(_ => inputDto.Dealers.Contains(_.UserId));
                        }
                        if (inputDto.OilTypes != null && inputDto.OilTypes.Any())
                        {
                            saudaContext = saudaContext.Where(_ => inputDto.OilTypes.Contains(_.OilTypeId));
                        }
                        if (inputDto.PackTypes != null && inputDto.PackTypes.Any())
                        {
                            saudaContext = saudaContext.Where(_ => inputDto.PackTypes.Contains(_.ProductGroupId));
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
                        if (inputDto.StateIds != null && inputDto.StateIds.Any() && userRoleContext.RoleId == (int)DTO.Enums.Role.StateTrader)
                        {
                            saudaContext = saudaContext.Where(_ => inputDto.StateIds.Contains(_.StateId));
                        }
                        var cityContext = _emamiContext.City.AsNoTracking();
                        var stateContext = _emamiContext.State.AsNoTracking();
                        var userlistContext = _emamiContext.Users.AsNoTracking();

                        if (saudaContext != null)
                        {
                            dailyBookedSaudaOutputDto = saudaContext.Select(_ => new DailyBookedSaudaOutputDto()
                            {
                                BookedDate = _.BookedDate,
                                PartyName = string.Concat((userlistContext.FirstOrDefault(u => u.Id == _.UserId).Name) + "-" + (cityContext.FirstOrDefault(c => c.Id == userlistContext.FirstOrDefault(u => u.Id == _.UserId).CityId) != null ? cityContext.FirstOrDefault(c => c.Id == userlistContext.FirstOrDefault(u => u.Id == _.UserId).CityId).CityName : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == userlistContext.FirstOrDefault(u => u.Id == _.UserId).StateId) != null ? stateContext.FirstOrDefault(s => s.Id == userlistContext.FirstOrDefault(u => u.Id == _.UserId).StateId).StateName : string.Empty) + "-" + (userlistContext.FirstOrDefault(u => u.Id == _.UserId) != null ? userlistContext.FirstOrDefault(u => u.Id == _.UserId).Code : string.Empty)),
                                OilType = _.OilType,
                                OilTypeId = _.OilTypeId,
                                ProductGroupId = _.ProductGroupId,
                                OilPackGroupType = _.OilPackGroupType,
                                ProductGroup = _.ProductGroup,
                                QuantityInMT = _.QuantityInMT,
                                QuantityCase = _.QuantityCase,
                                SkuName = _.SkuName,
                                SkuId=_.SkuId,
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
                            //            //if (item.PackGroupId > 0)
                            //            //{
                            //            //    var BulkPack = bookedSaudaoutput.Where(_ => _.OilTypeId == item.OilTypeId && _.ProductGroupId == item.PackGroupId);
                            //            //    if (BulkPack != null && BulkPack.Any())
                            //            //    {
                            //            //        Dto.BPQuantityInMT = BulkPack.Sum(_ => _.QuantityInMT);
                            //            //    }
                            //            //}
                            //            //else
                            //            //{
                            //            //    var BulkPack = bookedSaudaoutput.Where(_ => _.OilTypeId == item.OilTypeId);
                            //            //    if (BulkPack != null && BulkPack.Any())
                            //            //    {
                            //            //        Dto.CPQuantityInMT = BulkPack.Sum(_ => _.QuantityInMT);
                            //            //    }
                            //            //}
                            //            //var PremiumPack = bookedSaudaoutput.Where(_ => _.OilTypeId == item.OilTypeId && _.ProductGroupId == (int)DTO.Enums.PackGroupType.Premium);
                            //            //if (PremiumPack != null && PremiumPack.Any())
                            //            //{
                            //            //    Dto.premiumquantityInMT = PremiumPack.Sum(_ => _.QuantityInMT);
                            //            //}
                            //            //var LauricPack = bookedSaudaoutput.Where(_ => _.OilTypeId == item.OilTypeId && _.ProductGroupId == (int)DTO.Enums.PackGroupType.Lauric);
                            //            //if (LauricPack != null && LauricPack.Any())
                            //            //{
                            //            //    Dto.LauricquantityInMT = LauricPack.Sum(_ => _.QuantityInMT);
                            //            //}
                            //            //var PopularPack = bookedSaudaoutput.Where(_ => _.OilTypeId == item.OilTypeId && _.ProductGroupId == (int)DTO.Enums.PackGroupType.Popular);
                            //            //if (PopularPack != null && PopularPack.Any())
                            //            //{
                            //            //    Dto.PopularquantityInMT = PopularPack.Sum(_ => _.QuantityInMT);
                            //            //}
                            //            //var BakeryPack = bookedSaudaoutput.Where(_ => _.OilTypeId == item.OilTypeId && _.ProductGroupId == (int)DTO.Enums.PackGroupType.Bakery);
                            //            //if (BakeryPack != null && BakeryPack.Any())
                            //            //{
                            //            //    Dto.BakeryquantityInMT = BakeryPack.Sum(_ => _.QuantityInMT);
                            //            //}
                            //            Dto.QuantityInMT = item.QuantityInMT;
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
                var role = _emamiContext.UserRoles.FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                long roleId = 0;
                if (role != null)
                {
                    roleId = role.RoleId;
                }

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
                //var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                //if (userRoleContext == null)
                //{
                //    return _resultService.ErrorMessage(Constants.UserNotFound);
                //}
                List<long> dealersList = new List<long>();
                if (inputDto.Dealers != null && inputDto.Dealers.Any())
                {
                    dealersList.AddRange(inputDto.Dealers);
                }
                else
                {
                    dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId).Select(_ => _.CustomerId).ToList();
                }
                if (dealersList != null && dealersList.Any())
                {
                    //var invoiceContext = _emamiContext.InvoiceDetails.AsNoTracking().Join(_emamiContext.Invoices.AsNoTracking(), id => id.InvoiceId, i => i.Id, (id, i) => new { id, i })
                    //   .Join(_emamiContext.Skus.AsNoTracking(), idi => idi.id.SkuId, sku => sku.Id, (idi, sku) => new { idi.id, idi.i, sku })
                    //   .Join(_emamiContext.OilTypes.AsNoTracking(), ids => ids.id.OilTypeId, o => o.Id, (ids, o) => new { ids.id, ids.i, ids.sku, o })
                    //   .Join(_emamiContext.SalesRegister.AsNoTracking(), idl => idl.id.InvoiceId, sr => sr.InvoiceId, (idl, sr) => new { idl.id, idl.i, idl.sku, idl.o, sr })
                    //   .Where(_ => _.id != null && _.i != null && _.sku != null && _.o != null
                    //   && DbFunctions.TruncateTime(_.i.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //   && DbFunctions.TruncateTime(_.i.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    //   && dealersList.Contains(_.i.UserId)
                    //   //&& _.i.SalesDocumentType != "ZHCR"
                    //   );

                    var cityContext = _emamiContext.City.AsNoTracking();
                    var stateContext = _emamiContext.State.AsNoTracking();
                    var userlistContext = _emamiContext.Users.AsNoTracking();



                    IEnumerable<DailyBookedSaudaOutputDto> invoiceContext = new List<DailyBookedSaudaOutputDto>();
                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {
                        var sqlQuery = @"DECLARE @DealerTemp TABLE (DealerId BIGINT)
DECLARE @UserDivision TABLE (SalesOrganizationId BIGINT, DistributionChannelId BIGINT, DivisionId BIGINT)

insert into @UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

if(@DealerString!='')
begin
Insert Into @DealerTemp
 Select Data From dbo.Split(@DealerString,',')
end
else
begin
 insert into @DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
 where UserId=@UserId
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
(o.Name+'-'+sorg.Code+'/'+dist.Code+'/'+div.Code) as OilType
from SalesRegisters s with(NOLOCK)
join Skus sku on s.MaterialCode=sku.SkuCode and s.SalesOrganizationId=sku.SalesOrganizationId
and s.DistributionChannelId=sku.DistributionChannelId and s.DivisionId=sku.DivisionId
join PackGroups p on p.Id=sku.PackGroupId
join OilTypes o on sku.OilTypeId=o.Id and o.SalesOrganizationId=s.SalesOrganizationId and o.DistributionChannelId=s.DistributionChannelId and o.DivisionId=s.DivisionId
join SalesOrganizations sorg on sorg.Id=o.SalesOrganizationId
join DistributionChannels dist on dist.Id=o.DistributionChannelId
join Divisions div on div.Id=o.DivisionId
join @UserDivision ud on ud.SalesOrganizationId=s.SalesOrganizationId
and ud.DistributionChannelId=s.DistributionChannelId and ud.DivisionId=s.DivisionId
join Users u on s.CustomerCode=u.Code
where 
Cast(s.InvoiceDate as date) >= Cast(@FromDate as date)
and Cast(s.InvoiceDate as date) <= Cast(@ToDate as date)
and u.Id in (select DealerId from @DealerTemp)";
                        invoiceContext = conn.Query<DailyBookedSaudaOutputDto>(sqlQuery, new
                        {
                            UserId = inputDto.LoginUserId,
                            DealerString = UtilityHelper.ConvertLongListToCommaSeparatedString(inputDto.Dealers),
                            FromDate = inputDto.FromDate,
                            ToDate = inputDto.ToDate
                        });

                    }

                    //var invoiceContext = (from s in _emamiContext.SalesRegister.AsNoTracking()
                    //                      join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                    //                      join ud in divisionslogieduser on new { SalesOrganizationId=s.SalesOrganizationId, DistributionChannelId=s.DistributionChannelId, DivisionId=s.DivisionId} 
                    //                      equals new { SalesOrganizationId=ud.SalesOrganizationId, DistributionChannelId=ud.DistributionChannelId, DivisionId=ud.DivisionId}
                    //                      join u in userlistContext on s.CustomerCode equals u.Code
                    //                      where  dealersList.Contains(u.Id)
                    //                      && DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //                        && DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    //                        && s.SalesOrganizationId == sku.SalesOrganizationId 
                    //                        && s.DistributionChannelId == sku.DistributionChannelId
                    //                        && s.DivisionId == sku.DivisionId
                    //                      select new { s, sku, u }
                    //                    );

                    //var invoiceContext = _emamiContext.SalesRegister.AsNoTracking()
                    //    .Join(_emamiContext.Skus.AsNoTracking(), sr => sr.MaterialCode, sk => sk.SkuCode, (sr, sk) => new { SalesRegister = sr, Sku = sk })
                    //        .Join(userlistContext, sr => sr.SalesRegister.CustomerCode, us => us.Code, (sr, us) => new { SalesRegister = sr.SalesRegister, Sku = sr.Sku, User = us })
                    //   .Where(_ => dealersList.Contains(_.User.Id)
                    //      && DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //      && DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                    //   //&& _.Sku.DivisionId == userContext.DivisionId
                    //   && _.SalesRegister.SalesOrganizationId == _.Sku.SalesOrganizationId && _.SalesRegister.DistributionChannelId == _.Sku.DistributionChannelId
                    //    && _.SalesRegister.DivisionId == _.Sku.DivisionId
                    //   );


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
                        if (invoiceContext != null)
                        {
                            dailyBookedSaudaOutputDto = invoiceContext.Select(_ => new DailyBookedSaudaOutputDto()
                            {
                                BookedDate = _.BookedDate != null ? _.BookedDate : DateTime.Now,
                                //PartyName = string.Concat((userlistContext.FirstOrDefault(u => u.Id == _.UserId).Name) + "-" + (cityContext.FirstOrDefault(c => c.Id == userlistContext.FirstOrDefault(u => u.Id == _.UserId).CityId) != null ? cityContext.FirstOrDefault(c => c.Id == userlistContext.FirstOrDefault(u => u.Id == _.UserId).CityId).CityName : string.Empty) + "-" + (stateContext.FirstOrDefault(s => s.Id == userlistContext.FirstOrDefault(u => u.Id == _.UserId).StateId) != null ? stateContext.FirstOrDefault(s => s.Id == userlistContext.FirstOrDefault(u => u.Id == _.UserId).StateId).StateName : string.Empty) + "-" + (userlistContext.FirstOrDefault(u => u.Id == _.UserId) != null ? userlistContext.FirstOrDefault(u => u.Id == _.UserId).Code : string.Empty)),
                                OilType = _.OilType,
                                OilTypeId = _.OilTypeId,
                                ProductGroupId = _.ProductGroupId,
                                ProductGroup = _.ProductGroup,
                                OilPackGroupType = _.OilPackGroupType,
                                QuantityInMT = _.QuantityInMT,
                                QuantityCase = _.QuantityCase
                                //SaleDocumentType = _.i.SalesDocumentType,
                                //MaterialType = _.Sku.MaterialType.Name
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
                                _.OilPackGroupType,
                                //_.SaleDocumentType,
                                _.MaterialType
                            }).Select(_ => new DailyBookedSaudaOutputDto()
                            {
                                OilType = _.Key.OilType,
                                OilTypeId = _.Key.OilTypeId,
                                ProductGroup = _.Key.ProductGroup,
                                ProductGroupId = _.Key.ProductGroupId,
                                OilPackGroupType = _.Key.OilPackGroupType,
                                //SaleDocumentType = _.Key.SaleDocumentType,
                                QuantityInMT = _.Sum(s => s.QuantityInMT),
                                QuantityCase = _.Sum(s => s.QuantityCase),
                                MaterialType = _.Key.MaterialType
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
                                        dto.MaterialType = item.MaterialType;
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
            return _resultService.SuccessObject(BookedSaudaOutputDto);
        }


        public ResultDto InvoiceDetailsByDealer(IdInputDto inputDto)
        {
            var invoiceDetailsOutputDto = new InvoiceDetailsOutputDto();
            _methodName = "InvoiceDetailsByDealer";
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

                #region OldCode
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

                //var invoiceContext = _emamiContext.InvoiceDetails.AsNoTracking()
                //       .Join(_emamiContext.Invoices.AsNoTracking(), ivd => ivd.InvoiceId, i => i.Id, (ivd, i) => new { ivd, i })
                //       .Join(_emamiContext.Skus.AsNoTracking(), ivdi => ivdi.ivd.SkuId, s => s.Id, (ivdi, s) => new { ivdi.ivd, ivdi.i, s })
                //       .Join(_emamiContext.OilTypes.AsNoTracking(), ivdis => ivdis.ivd.OilTypeId, o => o.Id, (ivdis, o) => new { ivdis.ivd, ivdis.i, ivdis.s, o })
                //       .Join(_emamiContext.SalesRegister.AsNoTracking(), ivdis => ivdis.ivd.InvoiceId, sr => sr.InvoiceId, (ivdis, sr) => new { ivdis.ivd, ivdis.i, ivdis.s, ivdis.o, SalesRegister = sr })
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
                //    invoiceDetailsOutputDto.InvoiceQuantity = invoiceContext.Select(_ => _.SalesRegister.QuantityMT).DefaultIfEmpty(0).Sum();

                //    invoiceDetailsOutputDto.InvoiceSKUDetails = invoiceContext.Select(_ => new InvoiceSKUDetailsOutputDto()
                //    {
                //        OilTypeId = _.ivd.OilTypeId,
                //        SkuId = _.ivd.SkuId,
                //        Quantity = _.SalesRegister.QuantityMT,
                //        QuantityInCase = _.ivd.QuantityInCase,
                //        QunatityPrice = _.ivd.SKUInvoiceTax,
                //        OilType = _.o.Name,
                //        sku = _.s.SkuName,
                //    }).ToList();



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


                //    var invoiceContext = _emamiContext.SalesRegister.AsNoTracking()
                //        .Join(_emamiContext.Skus.AsNoTracking(), sr => sr.MaterialCode, sk => sk.SkuCode, (sr, sk) => new { SalesRegister = sr, Sku = sk })
                //        .Join(_emamiContext.Users.AsNoTracking(), sr => sr.SalesRegister.Payer, us => us.Code, (sr, us) => new { SalesRegister = sr.SalesRegister, Sku = sr.Sku, User = us })
                //        .Where(w => w.SalesRegister.Id == inputDto.Id && w.User.VerticalId == userContext.VerticalId)
                //        .Select(s => new
                //        {
                //            PackGroupId = s.Sku.PackGroupId,
                //            QuantityMT = s.SalesRegister.QuantityMT,
                //            QuantityInCase = s.SalesRegister.QuantityCase,
                //            OilTypeId = s.Sku.OilTypeId,
                //            OilType = s.Sku.OilType.Name,
                //            SkuId = s.Sku.Id,
                //            Sku = s.Sku.SkuName,
                //            BillNumber = s.SalesRegister.BillNumber,
                //            BillDate = s.SalesRegister.BillingDate,
                //            InvoiceId = s.SalesRegister.InvoiceId,
                //            TotalGST = s.SalesRegister.TotalGST
                //        }).ToList();

                //if (invoiceContext != null && invoiceContext.Any())
                //{
                //    if (inputDto.IsBulkPack)
                //    {
                //        invoiceContext = invoiceContext.Where(_ => _.PackGroupId == (int)DTO.Enums.PackGroupType.BulkPacking).ToList();
                //    }
                //    else
                //    {
                //        invoiceContext = invoiceContext.Where(_ => _.PackGroupId == (int)DTO.Enums.PackGroupType.ConsumerPacking).ToList();
                //    }
                //}
                //if (invoiceContext != null && invoiceContext.Any())
                //{
                //    invoiceDetailsOutputDto.InvoiceId = invoiceContext.FirstOrDefault().InvoiceId;
                //    invoiceDetailsOutputDto.InvoiceNumber = invoiceContext.FirstOrDefault().BillNumber;
                //    invoiceDetailsOutputDto.InvoiceDate = Convert.ToDateTime(invoiceContext.FirstOrDefault().BillDate);
                //    invoiceDetailsOutputDto.TotalInvoiceValue = invoiceContext.Select(_ => string.IsNullOrEmpty(_.TotalGST) ? 0 : Convert.ToDecimal(_.TotalGST)).DefaultIfEmpty(0).Sum(); //string.IsNullOrEmpty(_.TotalGST) ? 0 : Convert.ToDecimal(_.TotalGST)).DefaultIfEmpty(0).Sum();
                //    invoiceDetailsOutputDto.InvoiceQuantity = invoiceContext.Select(_ => _.QuantityMT).DefaultIfEmpty(0).Sum();

                //    invoiceDetailsOutputDto.InvoiceSKUDetails = invoiceContext.Select(_ => new InvoiceSKUDetailsOutputDto()
                //    {
                //        OilTypeId = (int)_.OilTypeId,
                //        SkuId = _.SkuId,
                //        Quantity = _.QuantityMT,
                //        QuantityInCase = _.QuantityInCase,
                //        QunatityPrice = string.IsNullOrEmpty(_.TotalGST) ? 0 : Convert.ToDecimal(_.TotalGST), //  _.TotalGST,
                //        OilType = _.OilType,
                //        sku = _.Sku,
                //    }).ToList();

                #endregion
                #region 27-12-2019


                IEnumerable<DivisionDetailsDto> divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
               .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                var invoiceContext=(from i in _emamiContext.SalesRegister.AsNoTracking()
                                    //join id in _emamiContext.InvoiceDetails.AsNoTracking() on i.Id equals id.InvoiceId
                                    join ud in divisionslogieduser on new { SalesOrganizationId = i.SalesOrganizationId, DistributionChannelId = i.DistributionChannelId, DivisionId = i.DivisionId }
                                            equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                    join sku in _emamiContext.Skus.AsNoTracking() on i.SkuId equals sku.Id
                                    join o in _emamiContext.OilTypes.AsNoTracking() on sku.OilTypeId equals o.Id
                                    where i.Id==inputDto.Id 
                                   // && id !=null
                                    && i!=null 
                                    && sku!=null
                                    && o !=null
                                    select new {i,s=sku,o}
                                    );

                //var invoiceContext = _emamiContext.InvoiceDetails.AsNoTracking()
                //       .Join(_emamiContext.Invoices.AsNoTracking(), ivd => ivd.InvoiceId, i => i.Id, (ivd, i) => new { ivd, i })
                //       .Join(_emamiContext.Skus.AsNoTracking(), ivdi => ivdi.ivd.MaterialNumber, s => s.SkuCode, (ivdi, s) => new { ivdi.ivd, ivdi.i, s })
                //       .Join(_emamiContext.OilTypes.AsNoTracking(), ivdis => ivdis.ivd.OilTypeId, o => o.Id, (ivdis, o) => new { ivdis.ivd, ivdis.i, ivdis.s, o })
                //       .Where(_ => _.i.Id == inputDto.Id && _.ivd != null && _.i != null && _.s != null && _.o != null
                //       );

                if (invoiceContext != null && invoiceContext.Any())
                {
                    invoiceDetailsOutputDto.InvoiceId = invoiceContext.FirstOrDefault().i.Id;
                    invoiceDetailsOutputDto.InvoiceNumber = invoiceContext.FirstOrDefault().i.InvoiceNumber;
                    invoiceDetailsOutputDto.InvoiceDate = invoiceContext.FirstOrDefault().i.InvoiceDate;
                    invoiceDetailsOutputDto.TotalInvoiceValue = Convert.ToDecimal(invoiceContext.FirstOrDefault().i.TotalAmount);
                    invoiceDetailsOutputDto.InvoiceQuantity = invoiceContext.Select(_ => _.i.QuantityMT).DefaultIfEmpty(0).Sum();

                    invoiceDetailsOutputDto.InvoiceSKUDetails = invoiceContext.Select(_ => new InvoiceSKUDetailsOutputDto()
                    {
                        OilTypeId = _.o.Id,
                        SkuId = _.i.SkuId,
                        Quantity = _.i.QuantityCase,
                        QuantityInCase = _.i.QuantityMT,
                        OilType = _.o.Name,
                        sku = _.s.SkuName,
                    }).ToList();

                    #endregion
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

        public ResultDto GetSalesOrderDetails(IdInputDto inputDto)
        {
            var salesOrderDOdetails = new List<SalesOrderDODetails>();
            _methodName = "GetSalesOrderDetails";
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

               
                if(inputDto.DealerId==0 && !inputDto.DealerCodes.Any())
                {
                    return _resultService.ErrorMessage(Constants.DealerIdEmpty);
                }

                if (!inputDto.DealerIds.Any())
                {
                    inputDto.DealerIds = new List<long>();
                    inputDto.DealerIds.Add(inputDto.DealerId);
                }
                IEnumerable<DivisionDetailsDto> divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
             .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });

                salesOrderDOdetails = (from l in _emamiContext.LiftingRequest.AsNoTracking()
                                       join u in _emamiContext.Users.AsNoTracking() on l.UserId equals u.Id
                                 join ld in _emamiContext.LiftingRequestDetails.AsNoTracking() on l.Id equals ld.LiftingRequestId
                                 join ud in divisionslogieduser on new { SalesOrganizationId = ld.SalesOrganizationId, DistributionChannelId = ld.DistributionhannelId, DivisionId = ld.DivisionId }
                                                equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                 where inputDto.DealerIds.Contains(l.UserId) && !l.IsCompleted && !String.IsNullOrEmpty(l.SAPDeliveryNo)
                                 select new SalesOrderDODetails()
                                 {
                                     DeliveryOrderNumber = l.SAPDeliveryNo,
                                     DealerCode=u.Code,
                                     DealerId=u.Id,
                                     //DeliveryOrderNumbers = l.SAPDeliveryNo.Split(',').ToList(),
                                     Id=l.Id,
                                     IsCompleted=l.IsCompleted
                                 }
                               ).ToList();

               
              
               
                if (salesOrderDOdetails != null && salesOrderDOdetails.Any())
                {
                     salesOrderDOdetails.ForEach(s =>
                      {
                          if (!String.IsNullOrEmpty(s.DeliveryOrderNumber))
                          {
                              s.DeliveryOrderNumbers = s.DeliveryOrderNumber.Split(',').ToList();
                          }
                          
  
                      });

                    return _resultService.SuccessObject(salesOrderDOdetails);
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

        public ResultDto UpdateSalesOrderDetails(LiftingUpdateDto inputDto)
        {
           
            _methodName = "UpdateSalesOrderDetails";
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
                if (inputDto.DoNumbers.Any())
                {
                    foreach(var donumber in inputDto.DoNumbers)
                    {
                        var completedo = new CompletedDoNumber()
                        {
                            CreatedDate = DateTime.Now,
                            DoNumber = donumber
                        };
                        _emamiContext.CompletedDoNumbers.Add(completedo);
                    }
                    _emamiContext.SaveChanges();
                }

                

                //var liftingcontext = _emamiContext.LiftingRequest.FirstOrDefault(_ => _.Id == inputDto.Id);
                //if (liftingcontext == null)
                //{
                //    return _resultService.ErrorMessage(Constants.InvalidRequest);
                //}
                //liftingcontext.IsCompleted = inputDto.IsComplete;

                _emamiContext.SaveChanges();

                return _resultService.SuccessObject(inputDto.DoNumbers);


               
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
