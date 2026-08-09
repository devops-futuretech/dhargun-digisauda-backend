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
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Adani.Solution.Service
{
    public interface IMobileSalesService
    {
        //Sales Credit Limit
        ResultDto GetTotalCreditLimit(CreditLimitInputDto loginUserIdDto);
        ResultDto GetCreditLimitList(LoginUserIdDto loginUserIdDto);
        ResultDto DashboardOverallSales(DashboardOverallSaudaInputDto inputDto);
        ResultDto OverallPerformanceByUser(DashboardOverallSaudaInputDto inputDto);
        ResultDto GetTotalSkuSales(SkuSalesFilterDto skuSalesFilterDto);
        ResultDto PerformanceRankingList(DashboardOverallSaudaInputDto inputDto);

        ResultDto GetGPSTracking(GPSTrackingDto inputDto);
        ResultDto AddOrUpdateGPSTracking(GPSTrackingDto inputDto);
    }
    public class MobileSalesService : IMobileSalesService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Sales Service");
        private const string ServiceName = "Sales Service";
        private string _methodName;
        private readonly IResultService _resultService;

        public MobileSalesService(IAdaniContext salesContext, IResultService resultService)
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

        #region Credit Limit
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
                IQueryable<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId).Select(s => s.CustomerId);
                if (dealersList != null && dealersList.Any())
                {

                    var userCreditListContext = (from ucm in _emamiContext.UserCreditMaster.AsNoTracking()
                                                     //join ud in divisionslogieduser on new { SalesOrganizationId = ucm.SalesOrgId, DistributionChannelId = ucm.DistChnlId, DivisionId = ucm.DivisionId }
                                                     //        equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                                 where dealersList.Contains(ucm.UserId)
                                                 orderby ucm.CreatedDate descending
                                                 group ucm by ucm.UserId into ucredit
                                                 select new { Id = ucredit.Key, value = ucredit.OrderByDescending(_ => _.CreatedDate).FirstOrDefault() }
                                    );
                    //var userCreditListContext = _emamiContext.UserCreditMaster.AsNoTracking().Where(_ => dealersList.Any(a => a.CustomerId == _.UserId) /*&& _.Isactive*/).ToList();
                    if (userCreditListContext != null && userCreditListContext.Any())
                    {
                        creditLimitTotalDto.DealersCount = userCreditListContext.Count();
                        creditLimitTotalDto.TotalCreditLimit = Math.Round((userCreditListContext.Sum(_ => _.value.CreditLimit) / 100000), 2);
                        creditLimitTotalDto.TotalCreditExposure = Math.Round((userCreditListContext.Sum(_ => _.value.CreditExposure) / 100000), 2);
                    }
                    //var invoiceDetailsContextList = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.Invoice != null && (DbFunctions.TruncateTime(_.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                    // DbFunctions.TruncateTime(_.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)) && _.Invoice.SalesDocumentType != "ZHCR" && dealersList.Any(a => a.CustomerId == _.Invoice.UserId));
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
                    #region 27-12-2019
                    //var invoiceDetailsContextList = _emamiContext.InvoiceDetails.AsNoTracking()
                    //    .Join(_emamiContext.SalesRegister.AsNoTracking(), inv => inv.InvoiceId, sr => sr.InvoiceId, (inv, sr) => new { InvoiceDetails = inv, SalesRegister = sr })
                    //    .Where(_ => _.InvoiceDetails.Invoice != null
                    //    && (DbFunctions.TruncateTime(_.InvoiceDetails.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //    && DbFunctions.TruncateTime(_.InvoiceDetails.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                    //    && _.InvoiceDetails.Invoice.SalesDocumentType != "ZHCR"
                    //    && dealersList.Any(a => a.CustomerId == _.InvoiceDetails.Invoice.UserId));
                    //if (invoiceDetailsContextList != null && invoiceDetailsContextList.Any())
                    //{
                    //    var bulkInvoiceDetailsContextList = invoiceDetailsContextList.Join(_emamiContext.Skus.AsNoTracking(), i => i.InvoiceDetails.SkuId, s => s.Id, (i, s) => new { i, s })
                    //     .Where(_ => _.s.PackGroupId == (int)DTO.Enums.PackGroupType.BulkPacking);
                    //    if (bulkInvoiceDetailsContextList != null && bulkInvoiceDetailsContextList.Any())
                    //    {
                    //        creditLimitTotalDto.TotalBulkPack = bulkInvoiceDetailsContextList.Sum(_ => _.i.SalesRegister.QuantityMT);
                    //    }
                    //    var customInvoiceDetailsContextList = invoiceDetailsContextList.Join(_emamiContext.Skus.AsNoTracking(), i => i.InvoiceDetails.SkuId, s => s.Id, (i, s) => new { i, s })
                    //     .Where(_ => _.s.PackGroupId == (int)DTO.Enums.PackGroupType.ConsumerPacking);
                    //    if (customInvoiceDetailsContextList != null && customInvoiceDetailsContextList.Any())
                    //    {
                    //        creditLimitTotalDto.TotalCustomPack = customInvoiceDetailsContextList.Sum(_ => _.i.SalesRegister.QuantityMT);
                    //    }
                    //} 
                    #endregion


                    var invoiceDetailsContextList = (from s in _emamiContext.SalesRegister.AsNoTracking()
                                                     join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                                                     join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                                                     join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                                                        equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                                     where (DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                                    && DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                                    && dealersList.Contains(u.Id)
                                    && s.SalesOrganizationId == sku.SalesOrganizationId && s.DistributionChannelId == sku.DistributionChannelId
                                    && s.DivisionId == sku.DivisionId
                                                     //&& s.SkuId > 0
                                                     select new { PackGroupId = sku.PackGroupId, QuantityMT = s.QuantityMT }
                                         );

                    //var invoiceDetailsContextList = _emamiContext.SalesRegister.AsNoTracking()
                    //    .Join(_emamiContext.Users.AsNoTracking(), sr => sr.CustomerCode, us => us.Code, (sr, us) => new { SalesRegister = sr, User = us })
                    //    .Join(_emamiContext.Skus.AsNoTracking(), sr => sr.SalesRegister.MaterialCode, sk => sk.SkuCode, (sr, sk) => new { SalesRegister = sr.SalesRegister, Sku = sk, User = sr.User })
                    //    .Where(_ => (DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    //    && DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                    //    && dealersList.Any(a => a.CustomerId == _.User.Id
                    //    && _.SalesRegister.SalesOrganizationId == _.Sku.SalesOrganizationId && _.SalesRegister.DistributionChannelId == _.Sku.DistributionChannelId
                    //    && _.SalesRegister.DivisionId == _.Sku.DivisionId)
                    //    //&& _.User.DivisionId == userContext.DivisionId && _.Sku.DivisionId== _.User.DivisionId
                    //    )
                    //    .Select(s => new
                    //    {
                    //        PackGroupId = s.Sku.PackGroupId,
                    //        QuantityMT = s.SalesRegister.QuantityMT
                    //    }).ToList();

                    if (invoiceDetailsContextList != null && invoiceDetailsContextList.Any())
                    {
                        if (inputDto.PackGroupId > 0)
                        {
                            var bulkInvoiceDetailsContextList = invoiceDetailsContextList
                             .Where(_ => _.PackGroupId == inputDto.PackGroupId);
                            if (bulkInvoiceDetailsContextList != null && bulkInvoiceDetailsContextList.Any())
                            {
                                creditLimitTotalDto.TotalPack = bulkInvoiceDetailsContextList.Sum(_ => _.QuantityMT);
                            }
                        }
                        else
                        {
                            var customInvoiceDetailsContextList = invoiceDetailsContextList;
                            if (customInvoiceDetailsContextList != null && customInvoiceDetailsContextList.Any())
                            {
                                creditLimitTotalDto.TotalPack = customInvoiceDetailsContextList.Sum(_ => _.QuantityMT);
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                IQueryable<UserCustomerMapping> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == loginUserIdDto.LoginUserId);
                if (dealersList != null && dealersList.Any())
                {
                    var userCreditListContext = _emamiContext.UserCreditMaster.AsNoTracking().Where(_ => dealersList.Any(a => a.CustomerId == _.UserId) && _.Isactive).ToList();
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
        #endregion

        #region performance

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

        public ResultDto DashboardOverallSales(DashboardOverallSaudaInputDto inputDto)
        {
            _methodName = "DashboardOverallSales";
            var OutputDto = new List<DashboardOverallSalesOutputDto>();
            if (inputDto == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }
            var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
            if (userContext == null)
            {
                return _resultService.ErrorMessage(Constants.UserNotFound);
            }

            var roleContext = _emamiContext.UserRoles.FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
            try
            {

                #region NewCode
                if (!inputDto.IsShowDealer)
                {
                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {
                        var chartResult = conn.Query<DashboardOverallSalesOutputDto>("Get_STSalesTargetChartsOilTypeWise",
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
                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {
                        var chartResult = conn.Query<DashboardOverallSalesOutputDto>("Get_STSalesTargetChartsDealerWise",
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
                #endregion


                #region OldCode
                //    List<MonthDto> months = new List<MonthDto>();
                //months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);

                //var dealerlist = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                //                  join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                //                  where ucm.UserId == inputDto.LoginUserId
                //                  select ucm.CustomerId).ToList();

                //IEnumerable<DivisionDetailsDto> divisionslogieduser = new List<DivisionDetailsDto>();
                //if (roleContext.RoleId == (int)DTO.Enums.Role.Admin)
                //{
                //    divisionslogieduser = _emamiContext.Divisions.AsNoTracking().Select(s => new DivisionDetailsDto()
                //    {
                //        SalesOrganizationId = s.SalesOrganizationId,
                //        DistributionChannelId = s.DistributionChannelId,
                //        DivisionId = s.Id
                //    });
                //}
                //else
                //{
                //    divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                // .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });
                //}



                //if (!inputDto.IsShowDealer)
                //{
                //    IEnumerable<SalesRegisterDashDto> salesreport = new List<SalesRegisterDashDto>();
                //    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                //    {

                //        var sqlQuery = @"CREATE TABLE #DealerTemp(DealerId BIGINT)
                //            Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                //            insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                //            select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

                //             insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
                //             where UserId=@UserId

                //             select 
                //            s.QuantityCase,
                //            sku.PackGroupId,
                //            sku.OilTypeId,
                //            o.Name as OilTypeName,
                //            s.InvoiceDate as Date
                //             from SalesRegisters s with(NOLOCK)
                //             join Skus sku on s.MaterialCode=sku.SkuCode and s.SalesOrganizationId=sku.SalesOrganizationId
                //             and s.DistributionChannelId=sku.DistributionChannelId and s.DivisionId=sku.DivisionId
                //             join Users u on s.CustomerCode=u.Code
                //             join OilTypes o on sku.OilTypeId=o.Id
                //             join #UserDivision ud on s.SalesOrganizationId=ud.SalesOrganizationId
                //             and s.DistributionChannelId=ud.DistributionChannelId and s.DivisionId=ud.DivisionId
                //             and Cast(s.InvoiceDate as date) >= Cast(@StartDate as date)
                //              and Cast(s.InvoiceDate as date) <= Cast(@EndDate as date)
                //              and u.Id in (select DealerId from #DealerTemp)
                //              drop table #DealerTemp
                //              drop table #UserDivision
                //        ";
                //        salesreport = conn.Query<SalesRegisterDashDto>(sqlQuery, new
                //        {
                //            UserId = inputDto.LoginUserId,
                //            StartDate = inputDto.FromDate,
                //            EndDate = inputDto.ToDate
                //            //Status = status
                //        });

                //    }

                //    foreach (var item in months)
                //    {
                //        var dashboardOverallsaudaOutpuDto = new List<DashboardOverallSalesOutputDto>();

                //        List<long> oilTypeIds = new List<long>();
                //        List<long> targetOilTypeIds = new List<long>();
                //        List<long> salesOilTypeIds = new List<long>();
                //        var targetListContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId && _.MonthId == item.Id && _.Year == item.Year);
                //        if (targetListContext != null && targetListContext.Any())
                //        {
                //            targetOilTypeIds = targetListContext.Where(_ => _.OilTypeId != 0).Select(_ => _.OilTypeId).ToList();
                //            oilTypeIds.AddRange(targetOilTypeIds);
                //        }

                //        //var salesListContext = _emamiContext.Invoices.AsNoTracking()
                //        //    .Join(_emamiContext.InvoiceDetails.AsNoTracking(), i => i.Id, ind => ind.InvoiceId, (i, ind) => new { i, ind })
                //        //    .Join(_emamiContext.OilTypes.AsNoTracking(), x => x.ind.OilTypeId, o => o.Id, (x, o) => new { x.ind, x.i, OilTypeName = o.Name })
                //        //    .Where(_ => _.i != null
                //        //    && dealerlist.Contains(_.i.UserId) && (DbFunctions.TruncateTime(_.i.InvoiceDate) >= DbFunctions.TruncateTime(item.StartDate)
                //        //    && DbFunctions.TruncateTime(_.i.InvoiceDate) <= DbFunctions.TruncateTime(item.EndDate))
                //        //    && _.i.SalesDocumentType != "ZHCR")
                //        //    .Select(_ => new { _.ind, _.OilTypeName });
                //        var salesListContext = salesreport.Where(_ => _.Date.Date >= item.StartDate.Date
                //        && _.Date.Date <= item.EndDate.Date
                //        );

                //        //var salesListContext = (from s in _emamiContext.SalesRegister.AsNoTracking()
                //        //                        join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                //        //             join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                //        //                        join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                //        //                               equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                //        //                        where 
                //        //             DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(item.StartDate)
                //        //                && DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(item.EndDate)
                //        //                && dealerlist.Contains(u.Id)
                //        //                && s.SalesOrganizationId == sku.SalesOrganizationId && s.DistributionChannelId == sku.DistributionChannelId
                //        //                && s.DivisionId == sku.DivisionId
                //        //                //&& s.SkuId > 0
                //        //                select new
                //        //                {
                //        //                    PackGroupId=sku.PackGroupId,
                //        //                    QuantityCase=s.QuantityCase,
                //        //                    OilTypeId=sku.OilTypeId ?? 0,
                //        //                    OilTypeName=sku.OilType.Name
                //        //                }
                //        //             );

                //        //var salesListContext = _emamiContext.SalesRegister.AsNoTracking()
                //        //.Join(_emamiContext.Skus.AsNoTracking(), sr => sr.MaterialCode, sk => sk.SkuCode, (sr, sk) => new { SalesRegister = sr, Sku = sk })
                //        //.Where(w => (DbFunctions.TruncateTime(w.SalesRegister.InvoiceDate) >= DbFunctions.TruncateTime(item.StartDate)
                //        //&& DbFunctions.TruncateTime(w.SalesRegister.InvoiceDate) <= DbFunctions.TruncateTime(item.EndDate))
                //        //&& dealerlist.Contains(w.SalesRegister.UserId)
                //        //&& w.SalesRegister.SalesOrganizationId == w.Sku.SalesOrganizationId && w.SalesRegister.DistributionChannelId == w.Sku.DistributionChannelId
                //        //&& w.SalesRegister.DivisionId == w.Sku.DivisionId
                //        //)
                //        //.Select(s => new
                //        //{
                //        //    PackGroupId = s.Sku.PackGroupId,
                //        //    QuantityCase = s.SalesRegister.QuantityCase,
                //        //    OilTypeId = s.Sku.OilTypeId ?? 0,
                //        //    OilTypeName = s.Sku.OilType.Name,
                //        //});

                //        if (salesListContext != null && salesListContext.Any())
                //        {
                //            salesOilTypeIds = salesListContext.Where(_ => _.OilTypeId != 0).Select(_ => _.OilTypeId).ToList();
                //            oilTypeIds.AddRange(salesOilTypeIds);
                //        }

                //        if (oilTypeIds != null && oilTypeIds.Any())
                //        {
                //            oilTypeIds = oilTypeIds.Distinct().ToList();
                //        }
                //        if (oilTypeIds != null && oilTypeIds.Any())
                //        {
                //            foreach (var oilTypeId in oilTypeIds)
                //            {
                //                var acheivment = new DashboardOverallSalesOutputDto();
                //                if (targetListContext != null && targetListContext.Any())
                //                {
                //                    var oilTypeTargetListContext = targetListContext.Where(_ => _.OilTypeId == oilTypeId);
                //                    if (oilTypeTargetListContext != null && oilTypeTargetListContext.Any())
                //                    {
                //                        acheivment.OilTypeId = oilTypeTargetListContext.FirstOrDefault().OilTypeId;
                //                        acheivment.OilType = oilTypeTargetListContext.FirstOrDefault().OilType.Name;
                //                        acheivment.TotalTarget = oilTypeTargetListContext.Select(_ => _.Target).DefaultIfEmpty(0).Sum();
                //                        acheivment.MonthId = item.Id;
                //                        acheivment.Month = new DateTime(DateTime.Now.Year, item.Id, 01).ToString("MMMM");
                //                    }
                //                }
                //                if (salesListContext != null && salesListContext.Any())
                //                {
                //                    var oilTypeSalesListContext = salesListContext.Where(_ => _.OilTypeId == oilTypeId);
                //                    if (oilTypeSalesListContext != null && oilTypeSalesListContext.Any())
                //                    {
                //                        acheivment.OilTypeId = oilTypeSalesListContext.FirstOrDefault().OilTypeId;
                //                        acheivment.OilType = oilTypeSalesListContext.FirstOrDefault().OilTypeName;
                //                        acheivment.MonthId = item.Id;
                //                        acheivment.TotalAchievment = oilTypeSalesListContext.Select(_ => _.QuantityCase).DefaultIfEmpty(0).Sum();
                //                    }
                //                }
                //                dashboardOverallsaudaOutpuDto.Add(acheivment);
                //            }
                //            OutputDto.AddRange(dashboardOverallsaudaOutpuDto);
                //        }

                //    }

                //}
                //else
                //{

                //    IEnumerable<SalesRegisterDashDto> salesreport = new List<SalesRegisterDashDto>();
                //    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                //    {

                //        var sqlQuery = @"CREATE TABLE #DealerTemp(DealerId BIGINT)
                //            Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                //            insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                //            select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

                //             insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
                //             where UserId=@UserId

                //             select 
                //            s.QuantityCase,
                //            sku.PackGroupId,
                //            sku.OilTypeId,
                //            o.Name as OilTypeName,
                //            s.InvoiceDate as Date
                //             from SalesRegisters s with(NOLOCK)
                //             join Skus sku on s.MaterialCode=sku.SkuCode and s.SalesOrganizationId=sku.SalesOrganizationId
                //             and s.DistributionChannelId=sku.DistributionChannelId and s.DivisionId=sku.DivisionId
                //             join Users u on s.CustomerCode=u.Code
                //             join OilTypes o on sku.OilTypeId=o.Id
                //             join #UserDivision ud on s.SalesOrganizationId=ud.SalesOrganizationId
                //             and s.DistributionChannelId=ud.DistributionChannelId and s.DivisionId=ud.DivisionId
                //             and Cast(s.InvoiceDate as date) >= Cast(@StartDate as date)
                //              and Cast(s.InvoiceDate as date) <= Cast(@EndDate as date)
                //              and u.Id in (select DealerId from #DealerTemp)
                //              drop table #DealerTemp
                //              drop table #UserDivision
                //        ";
                //        salesreport = conn.Query<SalesRegisterDashDto>(sqlQuery, new
                //        {
                //            UserId = inputDto.LoginUserId,
                //            StartDate = inputDto.FromDate,
                //            EndDate = inputDto.ToDate
                //            //Status = status
                //        });

                //    }
                //    var dealerContext = _emamiContext.Users.AsNoTracking().Where(_ => dealerlist.Contains(_.Id)).Select(_ => new { dealerId = _.Id, dealerName = _.Name }).ToList();

                //    var targetContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => dealerlist.Contains(_.AssignedToId ?? 0)).ToList().Where(_ => months.Any(d => d.Id == _.MonthId && d.Year == _.Year))
                //        .GroupBy(_ => _.AssignedToId).
                //        Select(_ => 
                //        new {
                //            dealerId = _.FirstOrDefault().AssignedToId,
                //            totalTarget = _.Sum(d => d.Target)
                //        }).ToList();

                //    var monthFilter = months.Select(_ => new { _.StartDate, _.EndDate }).ToList();
                //    var StartMonthDate = monthFilter.Min(_ => _.StartDate);
                //    var EndMonthDate = monthFilter.Max(_ => _.EndDate);



                //    IEnumerable<SalesRegisterDataDto> salesContext = new List<SalesRegisterDataDto>();
                //    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                //    {

                //        var sqlQuery = @"CREATE TABLE #DealerTemp(DealerId BIGINT)
                //                    Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                //                    insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                //                    select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

                //                     insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
                //                     where UserId=@UserId

                //                     select 
                //                     u.Id as UserId,Sum(s.QuantityCase) as QuantityMT
                //                     from SalesRegisters s with(NOLOCK)
                //                     join Skus sku on s.MaterialCode=sku.SkuCode and s.SalesOrganizationId=sku.SalesOrganizationId
                //                     and s.DistributionChannelId=sku.DistributionChannelId and s.DivisionId=sku.DivisionId
                //                     join Users u on s.CustomerCode=u.Code
                //                     right join #DealerTemp dealer on u.Id=dealer.DealerId
                //                     join OilTypes o on sku.OilTypeId=o.Id
                //                     join #UserDivision ud on s.SalesOrganizationId=ud.SalesOrganizationId
                //                     and s.DistributionChannelId=ud.DistributionChannelId and s.DivisionId=ud.DivisionId
                //                     and Cast(s.InvoiceDate as date) >= Cast(@StartDate as date)
                //                      and Cast(s.InvoiceDate as date) <= Cast(@EndDate as date)
                //                      --and u.Id in (select DealerId from #DealerTemp)
                //                      group by u.Id
                //                      drop table #DealerTemp
                //                      drop table #UserDivision
                //        ";
                //        salesContext = conn.Query<SalesRegisterDataDto>(sqlQuery, new
                //        {
                //            UserId = inputDto.LoginUserId,
                //            StartDate = StartMonthDate,
                //            EndDate = EndMonthDate
                //            //Status = status
                //        });

                //    }

                //    //var salesContext = (from s in _emamiContext.SalesRegister.AsNoTracking()
                //    //                    join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                //    //                 join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                //    //                 join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                //    //                        equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                //    //                 where DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(StartMonthDate)
                //    //        && DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(EndMonthDate)
                //    //        && dealerlist.Contains(u.Id)
                //    //        && s.SalesOrganizationId == sku.SalesOrganizationId && s.DistributionChannelId == sku.DistributionChannelId
                //    //        && s.DivisionId == sku.DivisionId
                //    //        //&& s.SkuId > 0
                //    //                 group s by s.UserId into sales 
                //    //                 select new { dealerId=sales.Key, TotalAchievment=sales.Sum(x => x.QuantityCase)}

                //    //                 );

                //    //var salesContext = _emamiContext.SalesRegister.AsNoTracking()
                //    //    .Join(_emamiContext.Skus.AsNoTracking(), sr => sr.MaterialCode, sk => sk.SkuCode, (sr, sk) => new { SalesRegister = sr, Sku = sk })
                //    //    .Where(w => DbFunctions.TruncateTime(w.SalesRegister.InvoiceDate) >= DbFunctions.TruncateTime(StartMonthDate)
                //    //        && DbFunctions.TruncateTime(w.SalesRegister.InvoiceDate) <= DbFunctions.TruncateTime(EndMonthDate)
                //    //        && dealerlist.Contains(w.SalesRegister.UserId)
                //    //        && w.SalesRegister.SalesOrganizationId == w.Sku.SalesOrganizationId && w.SalesRegister.DistributionChannelId == w.Sku.DistributionChannelId
                //    //        && w.SalesRegister.DivisionId == w.Sku.DivisionId
                //    //    )
                //    //    .GroupBy(_ => _.SalesRegister.UserId)
                //    //    .Select(s => new
                //    //    {
                //    //        dealerId = s.FirstOrDefault().SalesRegister.UserId,
                //    //        TotalAchievment = s.Sum(_ => _.SalesRegister.QuantityCase)
                //    //    }).ToList();

                //    OutputDto = (from dealer in dealerContext
                //                     join sales in salesContext on dealer.dealerId equals sales.UserId into dealerSales
                //                     from allSale in dealerSales.DefaultIfEmpty()
                //                     join target in targetContext on dealer.dealerId equals target.dealerId into dealerTarget
                //                     from allTarget in dealerTarget.DefaultIfEmpty()
                //                     select new DashboardOverallSalesOutputDto
                //                     {
                //                         DealerId = dealer.dealerId,
                //                         Dealer = dealer.dealerName,
                //                         TotalTarget = allTarget != null ? allTarget.totalTarget : 0,
                //                         TotalAchievment = allSale != null ? allSale.QuantityMT : 0,
                //                         AchievmentPercentage = allTarget != null && allTarget.totalTarget > 0 && allSale != null ? (allSale.QuantityMT / allTarget.totalTarget) * 100 : 0
                //                     }).ToList();
                //}
                #endregion



                var resultData = new NewDashboardOverallSalesOutputDto()
                {
                    SalesList = OutputDto,
                    //TotalTarget = OutputDto.Select(_ => _.TotalTarget).DefaultIfEmpty(0).Sum(),
                    //OverallSales = OutputDto.Select(_ => _.TotalAchievment).DefaultIfEmpty(0).Sum()
                };
                resultData.TotalTarget = OutputDto.FirstOrDefault().OverallTarget;
                resultData.OverallSales = OutputDto.FirstOrDefault().OverallAchievment;
                return _resultService.SuccessObject(resultData);
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
            var dashboardOverallsaudaOutpuDto = new OverallPerformanceByUserOutputDto();
            if (inputDto == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }
            try
            {

                decimal target = 0;
                List<MonthDto> months = new List<MonthDto>();
                months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);
                foreach (var item in months)
                {
                    var outputDto = new DashboardOverallsaudaOutpuDto();
                    var targetContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId && _.MonthId == item.Id && _.Year == item.Year).ToList();
                    if (targetContext != null)
                    {
                        target = target + targetContext.Sum(_ => _.Target);
                    }
                }
                var dealerlist = new List<long>();
                if (inputDto.RoleId == (int)DTO.Enums.Role.ZonalTrader)
                {
                    var bdoIds = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(s => s.Id).ToList();
                    dealerlist = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                  join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                                  where bdoIds.Contains(ucm.UserId)
                                  select ucm.CustomerId).ToList();
                }
                else
                {
                    dealerlist = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                  join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                                  where ucm.UserId == inputDto.LoginUserId
                                  select ucm.CustomerId).ToList();
                }

                IEnumerable<DivisionDetailsDto> divisionslogieduser = new List<DivisionDetailsDto>();
                if (inputDto.RoleId == (int)DTO.Enums.Role.Admin)
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

                if (dealerlist != null)
                {
                    var usercontext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);

                    var salesContextList = (from s in _emamiContext.SalesRegister.AsNoTracking()
                                            join u in _emamiContext.Users.AsNoTracking() on s.CustomerCode equals u.Code
                                            join sku in _emamiContext.Skus.AsNoTracking() on s.MaterialCode equals sku.SkuCode
                                            join ud in divisionslogieduser on new { SalesOrganizationId = s.SalesOrganizationId, DistributionChannelId = s.DistributionChannelId, DivisionId = s.DivisionId }
                                                 equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                            where dealerlist.Contains(u.Id)
                                                  && (DbFunctions.TruncateTime(s.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                  DbFunctions.TruncateTime(s.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                                                  //&& _.c.u.DivisionId == usercontext.DivisionId && _.sku.DivisionId== _.c.u.DivisionId
                                                  && s.SalesOrganizationId == sku.SalesOrganizationId
                                                  && s.DistributionChannelId == sku.DistributionChannelId
                                                  && s.DivisionId == sku.DivisionId
                                                  && s.SkuId > 0
                                            select new { s, sku, u }
                                       );


                    //var salesContextList = _emamiContext.SalesRegister.AsNoTracking()
                    //                          .Join(_emamiContext.Users.AsNoTracking(), x => x.CustomerCode, u => u.Code, (x, u) => new { x, u })
                    //                          .Join(_emamiContext.Skus.AsNoTracking(), c => c.x.MaterialCode, sku => sku.SkuCode, (c, sku) => new { c, sku })
                    //                         //.Join(_emamiContext.InvoiceDetails.AsNoTracking(), sr => sr.InvoiceId, id => id.InvoiceId, (sr, id) => new { sr, id })
                    //                         .Where(_ => dealerlist.Contains(_.c.u.Id)
                    //                         && (DbFunctions.TruncateTime(_.c.x.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                    //                         DbFunctions.TruncateTime(_.c.x.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                    //                         //&& _.c.u.DivisionId == usercontext.DivisionId && _.sku.DivisionId== _.c.u.DivisionId
                    //                         && _.c.x.SalesOrganizationId == _.sku.SalesOrganizationId && _.c.x.DistributionChannelId == _.sku.DistributionChannelId
                    //                         && _.c.x.DivisionId == _.sku.DivisionId
                    //                         ).ToList();

                    if (salesContextList != null)
                    {
                        decimal Achievement = 0;
                        Achievement = salesContextList.Select(_ => _.s.QuantityMT).DefaultIfEmpty(0).Sum();


                        //var targetContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId && _.MonthId == inputDto.FromDate.Month && _.Year == inputDto.FromDate.Year).ToList();
                        //if(targetContext != null && targetContext.Any())
                        //{
                        //    var acheivment = new OverallPerformanceByUserOutputDto
                        //    {
                        //        UserId = usercontext.Id,
                        //        Usercode = usercontext.Code,
                        //        Username = usercontext.Name,
                        //        UserTarget = targetContext.Sum(_ => _.Target),
                        //        UserAchievment = Achievement,
                        //        AchievmentPercentage = targetContext.Sum(_ => _.Target) > 0 ? (Achievement / targetContext.Sum(_ => _.Target)) * 100 : 0
                        //    };
                        //    dashboardOverallsaudaOutpuDto = acheivment;
                        //}

                        var acheivment = new OverallPerformanceByUserOutputDto
                        {
                            UserId = usercontext.Id,
                            Usercode = usercontext.Code,
                            Username = usercontext.Name,
                            UserTarget = target,
                            UserAchievment = Achievement,
                            AchievmentPercentage = target > 0 ? (Achievement / target) * 100 : 0
                        };
                        dashboardOverallsaudaOutpuDto = acheivment;
                    }
                }
                return _resultService.SuccessObject(dashboardOverallsaudaOutpuDto);
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
            var dashboardOverallsaudaOutpuDto = new List<OverallPerformanceByUserOutputDto>();
            var OrderOutpuDto = new List<OverallPerformanceByUserOutputDto>();
            if (inputDto == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
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
                var loginUserContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
                if (loginUserContext == null)
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



                var isAchievementRank = false;

                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {
                    var chartResult = conn.Query<OverallPerformanceByUserOutputDto>("GetPerformanceRankingList",
                        new
                        {
                            LoginUserId = inputDto.LoginUserId,
                            FromDate = inputDto.FromDate,
                            ToDate = inputDto.ToDate,
                            RoleId=inputDto.RoleId
                        }, commandType: CommandType.StoredProcedure).ToList();

                    dashboardOverallsaudaOutpuDto.AddRange(chartResult);
                }

                #region OldCode
                //List<MonthDto> months = new List<MonthDto>();
                //months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);
                //List<int> MonthIds = months.Select(s => s.Id).Distinct().ToList();
                //List<long> Years = months.Select(s => (long)s.Year).Distinct().ToList();

                //IEnumerable<DivisionDetailsDto> divisionslogieduser = new List<DivisionDetailsDto>();
                //if (inputDto.RoleId == (int)DTO.Enums.Role.Admin)
                //{
                //    divisionslogieduser = _emamiContext.Divisions.AsNoTracking().Select(s => new DivisionDetailsDto()
                //    {
                //        SalesOrganizationId = s.SalesOrganizationId,
                //        DistributionChannelId = s.DistributionChannelId,
                //        DivisionId = s.Id
                //    });
                //}
                //else
                //{
                //    divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
                // .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });
                //}
                //var userRoleContext = (from user in _emamiContext.Users.AsNoTracking()
                //                       join role in _emamiContext.UserRoles.AsNoTracking() on user.Id equals role.UserId
                //                       join udiv in _emamiContext.UserDivisionMappings.AsNoTracking() on user.Id equals udiv.UserId
                //                       join ud in divisionslogieduser on new { SalesOrganizationId = udiv.SalesOrganizationId, DistributionChannelId = udiv.DistributionChannelId, DivisionId = udiv.DivisionId }
                //                               equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                //                       where role.RoleId == inputDto.RoleId && user.IsActive
                //                       //&& user.DivisionId == loginUserContext.DivisionId
                //                       select new UserMasterDto
                //                       {
                //                           Id = user.Id,
                //                           EmployeeName = user.Name,
                //                           EmployeeCode = user.Code
                //                       }).Distinct().ToList();

                //var inputRoleUserIds = userRoleContext.Select(_ => _.Id).ToList();
                //var dealerlist = new List<long>();
                //                if (userRoleContext != null)
                //                {
                //                    foreach (var user in userRoleContext)
                //                    {

                //                        var totalTarget = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == user.Id
                //                            && MonthIds.Contains(_.MonthId) && Years.Contains(_.Year)).Select(_ => _.Target).DefaultIfEmpty(0).Sum();
                //                        dealerlist = new List<long>();
                //                        IEnumerable<SalesRegister> salesContextList = new List<SalesRegister>();
                //                        decimal achievement = 0;
                //                        if (inputDto.RoleId == (int)DTO.Enums.Role.ZonalTrader)
                //                        {

                //                            using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                //                            {

                //                                var sqlQuery = @"Create Table #DealerIdsTemp(DealerId bigint)
                //Create Table #BdoId(BdoId bigint)
                //Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)

                //insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                //select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@LoginUserId

                //insert into #BdoId(BdoId) select UserId from UserReportingToMappings where ReportingToUserId=@UserId

                //insert into #DealerIdsTemp(DealerId) select CustomerId from UserCustomerMappings where UserId in (select BdoId from #BdoId)

                //select
                //(Case when Sum(s.QuantityMT) is null then 0 else Sum(s.QuantityMT) end)  as QuantityMT
                //from SalesRegisters s with(NOLOCK)
                //join Skus sku with(NOLOCK) on s.MaterialCode=sku.SkuCode and s.SalesOrganizationId=sku.SalesOrganizationId
                //and s.DistributionChannelId=sku.DistributionChannelId and s.DivisionId=sku.DivisionId
                //join Users u with(NOLOCK) on s.CustomerCode=u.Code
                //join #UserDivision ud on s.SalesOrganizationId=ud.SalesOrganizationId and s.DistributionChannelId=ud.DistributionChannelId
                //and s.DivisionId=ud.DivisionId
                //where 
                //u.Id in (select DealerId from #DealerIdsTemp)
                //and Cast(s.InvoiceDate as date) <= Cast(@ToDate as date)
                //and Cast(s.InvoiceDate as date) >= Cast(@FromDate as date)

                //drop table #BdoId
                //drop table #DealerIdsTemp
                //drop table #UserDivision";
                //                                var invoice = conn.Query<decimal>(sqlQuery, new
                //                                {
                //                                    UserId = user.Id,
                //                                    LoginUserId = inputDto.LoginUserId,
                //                                    FromDate = inputDto.FromDate,
                //                                    ToDate = inputDto.ToDate,
                //                                });
                //                                achievement = invoice != null ? invoice.FirstOrDefault() : 0;
                //                            }
                //                        }
                //                        else
                //                        {
                //                            using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                //                            {

                //                                var sqlQuery = @"Create Table #DealerIdsTemp(DealerId bigint)
                //Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)

                //insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                //select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@LoginUserId

                //insert into #DealerIdsTemp(DealerId) select CustomerId from UserCustomerMappings where UserId=@UserId

                //select
                //(Case when Sum(s.QuantityMT) is null then 0 else Sum(s.QuantityMT) end)  as QuantityMT
                //from SalesRegisters s with(NOLOCK)
                //join Skus sku with(NOLOCK) on s.MaterialCode=sku.SkuCode and s.SalesOrganizationId=sku.SalesOrganizationId
                //and s.DistributionChannelId=sku.DistributionChannelId and s.DivisionId=sku.DivisionId
                //join Users u with(NOLOCK) on s.CustomerCode=u.Code
                //join #UserDivision ud on s.SalesOrganizationId=ud.SalesOrganizationId and s.DistributionChannelId=ud.DistributionChannelId
                //and s.DivisionId=ud.DivisionId
                //where 
                //u.Id in (select DealerId from #DealerIdsTemp)
                //and Cast(s.InvoiceDate as date) <= Cast(@ToDate as date)
                //and Cast(s.InvoiceDate as date) >= Cast(@FromDate as date)

                //drop table #DealerIdsTemp
                //drop table #UserDivision";
                //                                var invoice = conn.Query<decimal>(sqlQuery, new
                //                                {
                //                                    UserId = user.Id,
                //                                    LoginUserId = inputDto.LoginUserId,
                //                                    FromDate = inputDto.FromDate,
                //                                    ToDate = inputDto.ToDate,
                //                                });
                //                                achievement = invoice != null ? invoice.FirstOrDefault() : 0;

                //                            }             select ucm.CustomerId).ToList();
                //                        }
                //                        var usercontext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == user.Id);
                //                        var acheivment = new OverallPerformanceByUserOutputDto
                //                        {
                //                            UserId = usercontext.Id,
                //                            Usercode = usercontext.Code,
                //                            Username = usercontext.Name,
                //                            UserTarget = totalTarget,
                //                            UserAchievment = achievement,
                //                            AchievmentPercentage = totalTarget > 0 ? (achievement / totalTarget) * 100 : 0
                //                        };
                //                        if (acheivment.AchievmentPercentage > 0 && isAchievementRank == false)
                //                        {
                //                            isAchievementRank = true;
                //                        }
                //                        dashboardOverallsaudaOutpuDto.Add(acheivment);


                //                    }
                //                }

                #endregion



                if (dashboardOverallsaudaOutpuDto != null)
                {
                    foreach(var item in dashboardOverallsaudaOutpuDto)
                    {
                        if (item.AchievmentPercentage > 0 && isAchievementRank == false)
                        {
                            isAchievementRank = true;
                        }
                    }
                    
                    if (isAchievementRank)
                    {
                        OrderOutpuDto.AddRange(dashboardOverallsaudaOutpuDto.OrderBy(_ => _.AchievmentPercentage));
                    }
                    else
                    {
                        OrderOutpuDto.AddRange(dashboardOverallsaudaOutpuDto.OrderByDescending(_ => _.UserAchievment));
                    }


                    if (OrderOutpuDto != null)
                    {
                        int Rank = 1;
                        OrderOutpuDto.ForEach(_ => _.Rank = Rank++);
                    }
                }

                return _resultService.SuccessObject(OrderOutpuDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetTotalSkuSales(SkuSalesFilterDto skuSalesFilterDto)
        {
            _methodName = "GetTotalSkuSales";
            try
            {
                var totalSkuSales = new SkuSalesDto();
                if (skuSalesFilterDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                //Sales person
                if (skuSalesFilterDto.UserId == 0)
                {
                    return _resultService.ErrorMessage(Constants.UserIdMissing);
                }
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == skuSalesFilterDto.UserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                IQueryable<InvoiceDetail> invoiceOrderContextList = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.SkuId == skuSalesFilterDto.SkuId && _.CreatedBy == skuSalesFilterDto.UserId);
                if (invoiceOrderContextList != null && invoiceOrderContextList.Any())
                {
                    var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == skuSalesFilterDto.SkuId);
                    if (skuContext != null)
                    {
                        totalSkuSales.SkuName = skuContext.SkuName;
                    }
                    if (skuSalesFilterDto.UomId == (int)DTO.Enums.Uom.Case)
                    {
                        totalSkuSales.TotalQuantity = invoiceOrderContextList.Sum(_ => _.QuantityInCase);
                    }
                    else if (skuSalesFilterDto.UomId == (int)DTO.Enums.Uom.MT)
                    {
                        totalSkuSales.TotalQuantity = invoiceOrderContextList.Sum(_ => _.ActualBilledQuantity);
                    }

                }
                if (totalSkuSales != null)
                {
                    return _resultService.SuccessObject(totalSkuSales);
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

        public ResultDto BilledAndNonBilledPartiesByChart(DashboardOverallSaudaInputDto inputDto)
        {
            _methodName = "BilledAndNonBilledPartiesByChart";
            var OutputDto = new List<DashboardOverallSalesOutputDto>();
            if (inputDto == null)
            {
                return _resultService.ErrorMessage(Constants.InvalidRequest);
            }
            try
            {
                List<MonthDto> months = new List<MonthDto>();
                months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);
                var dealerlist = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                  join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                                  where ucm.UserId == inputDto.LoginUserId
                                  select ucm.CustomerId).Distinct().ToList();

                var SalesBilledandNonBilledPartiesOutpuDto = new List<SalesBilledPartiesDto>();
                foreach (var dealer in dealerlist)
                {
                    decimal Target = 0;
                    foreach (var item in months)
                    {
                        var targetContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId && _.MonthId == item.Id && _.FinancialYearId == item.Year).ToList();
                        if (targetContext != null)
                        {
                            foreach (var detail in targetContext)
                            {
                                Target = Target + detail.Target;
                            }
                        }
                    }
                    var salesContext = (from invoice in _emamiContext.Invoices.AsNoTracking()
                                        where invoice.UserId == dealer
                                        && DbFunctions.TruncateTime(invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                            DbFunctions.TruncateTime(invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                                        select invoice
                                                ).ToList();
                    if (salesContext != null)
                    {
                        var objdto = new SalesBilledPartiesDto
                        {
                            TotalTarget = Target,
                            TotalAchievment = salesContext.Sum(_ => (decimal)_.TotalInvoice),
                            DealerId = dealer,
                            Dealer = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == dealer).Name
                        };
                        SalesBilledandNonBilledPartiesOutpuDto.Add(objdto);
                    }
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

        #endregion


        #region GPSTracking

        public ResultDto GetGPSTracking(GPSTrackingDto inputDto)
        {
            _methodName = "GetGPSTracking";
            var resultDto = new ResultDto();
            try
            {
                var gpsTracking = _emamiContext.GPSTrackings.AsNoTracking()
                    .Select(s => new GPSTrackingDto()
                    {
                        Id = s.Id,
                        UserId = s.UserId,
                        Latitude = s.Latitude,
                        Longitude = s.Longitude,
                        //CreatedDate = s.CreatedDate,
                    }).ToList();

                resultDto.SuccessDto.Response = gpsTracking;
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

        /// <summary>
        /// Add or Update Oil Type
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>
        public ResultDto AddOrUpdateGPSTracking(GPSTrackingDto inputDto)
        {
            _methodName = "AddOrUpdateGPSTracking";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (!(inputDto.Id > 0))
                {
                    var gpsTracking = new GPSTracking();
                    gpsTracking.UserId = inputDto.UserId;
                    gpsTracking.Latitude = inputDto.Latitude;
                    gpsTracking.Longitude = inputDto.Longitude;
                    gpsTracking.CreatedBy = inputDto.LoginUserId;
                    gpsTracking.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    _emamiContext.GPSTrackings.Add(gpsTracking);
                }
                else
                {
                    var gpsTracking = _emamiContext.GPSTrackings.FirstOrDefault(f => f.Id == inputDto.Id);
                    gpsTracking.UserId = inputDto.UserId;
                    gpsTracking.Latitude = inputDto.Latitude;
                    gpsTracking.Longitude = inputDto.Longitude;
                    gpsTracking.ModifiedBy = inputDto.LoginUserId;
                    gpsTracking.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                }
                _emamiContext.SaveChanges();
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

    }
}

