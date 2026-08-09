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
using System.IO;
using System.Globalization;
using System.Net;
using System.Web.Script.Serialization;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace Adani.Solution.Service
{
    public interface IMobileDealerSalesService
    {
        ResultDto GetTotalCreditLimit(CreditLimitInputDto inputDto);
        ResultDto GetCreditLimitList(LoginUserIdDto loginUserIdDto);
        ResultDto DashboardOverallSales(DashboardOverallSaudaInputDto inputDto);
        ResultDto OverallPerformanceByUser(DashboardOverallSaudaInputDto inputDto);
        ResultDto GetTotalSkuSales(SkuSalesFilterDto skuSalesFilterDto);
        ResultDto PerformanceRankingList(DashboardOverallSaudaInputDto inputDto);
    }
    public class MobileDealerSalesService : IMobileDealerSalesService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Mobile Dealer Sales Service");
        private const string ServiceName = "Mobile Dealer Sales Service";
        private string _methodName;
        private readonly IResultService _resultService;

        public MobileDealerSalesService(IAdaniContext salesContext, IResultService resultService)
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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userCreditListContext = _emamiContext.UserCreditMaster.AsNoTracking()
                    .Where(_ => _.UserId == inputDto.LoginUserId)
                    .OrderByDescending(_ => _.CreatedDate).FirstOrDefault();
                if (userCreditListContext != null)
                {
                    creditLimitTotalDto.DealersCount = 1;
                    creditLimitTotalDto.TotalCreditLimit = Math.Round((userCreditListContext.CreditLimit / 100000), 2);
                    creditLimitTotalDto.TotalCreditExposure = Math.Round((userCreditListContext.CreditExposure / 100000), 2);
                }
                //var invoiceDetailsContextList = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.Invoice != null && (DbFunctions.TruncateTime(_.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                //     DbFunctions.TruncateTime(_.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)) && _.Invoice.UserId == inputDto.LoginUserId && _.Invoice.SalesDocumentType != "ZHCR");
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

                //var invoiceDetailsContextList = _emamiContext.InvoiceDetails.AsNoTracking()
                //    .Join(_emamiContext.SalesRegister.AsNoTracking(), invd => invd.InvoiceId, sr => sr.InvoiceId, (invd, sr) => new { InvoiceDetails = invd, SalesRegister = sr })
                //    .Where(_ => _.InvoiceDetails.Invoice != null
                //    && (DbFunctions.TruncateTime(_.InvoiceDetails.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                //    && DbFunctions.TruncateTime(_.InvoiceDetails.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                //    && _.InvoiceDetails.Invoice.UserId == inputDto.LoginUserId
                //    && _.InvoiceDetails.Invoice.SalesDocumentType != "ZHCR");

                var invoiceDetailsContextList = _emamiContext.SalesRegister.AsNoTracking()
                    .Join(_emamiContext.Users.AsNoTracking(), sr => sr.CustomerCode, us => us.Code, (sr, us) => new { SalesRegister = sr, User = us })
                    .Join(_emamiContext.Skus.AsNoTracking(), i => i.SalesRegister.MaterialCode, s => s.SkuCode, (i, s) => new { SalesRegister = i.SalesRegister, User = i.User, sku = s })
                    .Where(_ => (DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    && DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
                    && _.User.Id == userContext.Id
                    && _.SalesRegister.SalesOrganizationId == _.sku.SalesOrganizationId && _.SalesRegister.DistributionChannelId == _.sku.DistributionChannelId
                       && _.SalesRegister.DivisionId == _.sku.DivisionId
                    //&& _.User.DivisionId == userContext.DivisionId && _.sku.DivisionId == _.User.DivisionId
                    ).Select(s => new
                    {
                        PackGroupId = s.sku.PackGroupId,
                        QuantityMT = s.SalesRegister.QuantityMT
                    }).ToList();

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
                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userCreditListContext = _emamiContext.UserCreditMaster.AsNoTracking().Where(_ => _.UserId == loginUserIdDto.LoginUserId && _.Isactive).ToList();
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
            try
            {
                List<MonthDto> months = new List<MonthDto>();
                months = GetMonthListfromInput(inputDto.FromDate, inputDto.ToDate);
                //foreach (var item in months)
                //{
                //    var dashboardOverallsaudaOutpuDto = new List<DashboardOverallSalesOutputDto>();
                //    var targetContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId && _.MonthId == item.Id && _.Year == item.Year).ToList();
                //    if (targetContext != null)
                //    {
                //        foreach (var detail in targetContext)
                //        {
                //            var salesContext = (from invoice in _emamiContext.Invoices.AsNoTracking()
                //                                join invdetail in _emamiContext.InvoiceDetails.AsNoTracking() on invoice.Id equals invdetail.InvoiceId
                //                                where invdetail.OilTypeId == detail.OilTypeId && invoice.UserId == inputDto.LoginUserId
                //                                && DbFunctions.TruncateTime(invoice.InvoiceDate) >= DbFunctions.TruncateTime(item.StartDate) &&
                //                                    DbFunctions.TruncateTime(invoice.InvoiceDate) <= DbFunctions.TruncateTime(item.EndDate)
                //                                select invdetail
                //                                ).ToList();

                //            if(salesContext != null)
                //            {
                //                var acheivment = new DashboardOverallSalesOutputDto
                //                {
                //                    OilTypeId = detail.OilTypeId,
                //                    OilType = detail.OilType.Name,
                //                    TotalTarget = detail.Target,
                //                    TotalAchievment = salesContext.Sum(_ => (decimal?)_.ActualBilledQuantity) ?? 0,
                //                    MonthId = item.Id
                //                };
                //                dashboardOverallsaudaOutpuDto.Add(acheivment);
                //            }
                //        }
                //    }
                //    OutputDto.AddRange(dashboardOverallsaudaOutpuDto);
                //}

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                foreach (var item in months)
                {
                    var dashboardOverallsaudaOutpuDto = new List<DashboardOverallSalesOutputDto>();

                    List<string> oilTypes = new List<string>();
                    List<string> targetOilTypeIds = new List<string>();
                    List<string> salesOilTypeIds = new List<string>();
                    var targetListContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId && _.MonthId == item.Id && _.Year == item.Year);
                    if (targetListContext != null && targetListContext.Any())
                    {
                        targetOilTypeIds = targetListContext.Where(_ => _.OilType.Name != string.Empty).Select(_ => _.OilType.Name).ToList();
                        oilTypes.AddRange(targetOilTypeIds);
                    }

                    //var salesListContext = _emamiContext.Invoices.AsNoTracking().Join(_emamiContext.InvoiceDetails.AsNoTracking(), i => i.Id, ind => ind.InvoiceId, (i, ind) => new { i, ind })
                    //    .Join(_emamiContext.OilTypes.AsNoTracking(), x => x.ind.OilTypeId, o => o.Id, (x, o) => new { x.ind, x.i, OilTypeName = o.Name })
                    //    .Where(_ => _.i != null && _.i.UserId == inputDto.LoginUserId && DbFunctions.TruncateTime(_.i.InvoiceDate) >= DbFunctions.TruncateTime(item.StartDate) &&
                    //                            DbFunctions.TruncateTime(_.i.InvoiceDate) <= DbFunctions.TruncateTime(item.EndDate) && _.i.SalesDocumentType != "ZHCR").Select(_ => new { _.ind, _.OilTypeName });
                    var salesListContext = new List<SalesRegisterDashDto>();
                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {

                        var sqlQuery = @"select
                                        sku.PackGroupId,
                                        s.QuantityCase,
                                        sku.OilTypeId,
                                        o.Name as OilTypeName
                                        from SalesRegisters s with(NOLOCK)
                                        join Skus sku on s.MaterialCode=sku.SkuCode and s.SalesOrganizationId=sku.SalesOrganizationId
                                        and s.DistributionChannelId=sku.DistributionChannelId and s.DivisionId=sku.DivisionId
                                        join OilTypes o on sku.OilTypeId=o.Id
                                        join Users u on s.CustomerCode=u.Code
                                        where u.Id=@UserId
                                        and Cast(s.InvoiceDate as date)>=Cast(@StartDate as date)
                                        and Cast(s.InvoiceDate as date) <= Cast(@EndDate as date)";
                        salesListContext = conn.Query<SalesRegisterDashDto>(sqlQuery, new
                        {
                            UserId = inputDto.LoginUserId,
                            StartDate=item.StartDate,
                            EndDate=item.EndDate
                        }).ToList();

                    }

                    //var salesListContext = _emamiContext.SalesRegister.AsNoTracking()
                    //    .Join(_emamiContext.Skus.AsNoTracking(), sr => sr.MaterialCode, sk => sk.SkuCode, (sr, sk) => new { SalesRegister = sr, Sku = sk })
                    //    .Join(_emamiContext.Users.AsNoTracking(), sr => sr.SalesRegister.CustomerCode, us => us.Code, (sr, us) => new { SalesRegister = sr.SalesRegister, Sku = sr.Sku, User = us })
                    //    .Where(_ => _.User.Id == userContext.Id && DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) >= DbFunctions.TruncateTime(item.StartDate) &&
                    //                            DbFunctions.TruncateTime(_.SalesRegister.InvoiceDate) <= DbFunctions.TruncateTime(item.EndDate)
                    //                            && _.SalesRegister.SalesOrganizationId == _.Sku.SalesOrganizationId && _.SalesRegister.DistributionChannelId == _.Sku.DistributionChannelId
                    //   && _.SalesRegister.DivisionId == _.Sku.DivisionId
                    //                            //&& _.User.DivisionId == userContext.DivisionId && _.Sku.DivisionId == _.User.DivisionId
                    //                            )
                    //    .Select(s => new
                    //    {
                    //        PackGroupId = s.Sku.PackGroupId,
                    //        QuantityCase = s.SalesRegister.QuantityCase,
                    //        OilTypeId = s.Sku.OilTypeId ?? 0,
                    //        OilTypeName = s.Sku.OilType.Name,
                    //    }).ToList();

                    if (salesListContext != null && salesListContext.Any())
                    {
                        salesOilTypeIds = salesListContext.Where(_ => _.OilTypeName != string.Empty).Select(_ => _.OilTypeName).ToList();
                        oilTypes.AddRange(salesOilTypeIds);
                    }

                    if (oilTypes != null && oilTypes.Any())
                    {
                        oilTypes = oilTypes.Distinct().ToList();
                    }
                    if (oilTypes != null && oilTypes.Any())
                    {
                        foreach (var oilTypeId in oilTypes)
                        {
                            var acheivment = new DashboardOverallSalesOutputDto();
                            if (targetListContext != null && targetListContext.Any())
                            {
                                var oilTypeTargetListContext = targetListContext.Where(_ => _.OilType.Name == oilTypeId);
                                if (oilTypeTargetListContext != null && oilTypeTargetListContext.Any())
                                {
                                    acheivment.OilTypeId = oilTypeTargetListContext.FirstOrDefault().OilTypeId;
                                    acheivment.OilType = oilTypeTargetListContext.FirstOrDefault().OilType.Name;
                                    acheivment.TotalTarget = oilTypeTargetListContext.Select(_ => _.Target).DefaultIfEmpty(0).Sum();
                                    acheivment.MonthId = item.Id;
                                }
                            }
                            if (salesListContext != null && salesListContext.Any())
                            {
                                var oilTypeSalesListContext = salesListContext.Where(_ => _.OilTypeName == oilTypeId);
                                if (oilTypeSalesListContext != null && oilTypeSalesListContext.Any())
                                {
                                    acheivment.OilTypeId = oilTypeSalesListContext.FirstOrDefault().OilTypeId;
                                    acheivment.OilType = oilTypeSalesListContext.FirstOrDefault().OilTypeName;
                                    acheivment.MonthId = item.Id;
                                    acheivment.TotalAchievment = oilTypeSalesListContext.Select(_ => _.QuantityCase).DefaultIfEmpty(0).Sum();
                                }
                            }
                            dashboardOverallsaudaOutpuDto.Add(acheivment);
                        }
                        OutputDto.AddRange(dashboardOverallsaudaOutpuDto);
                    }

                }

                var result = new NewDashboardOverallSalesOutputDto();
                result.SalesList = OutputDto;
                result.TotalTarget = OutputDto.Select(_ => _.TotalTarget).DefaultIfEmpty(0).Sum();
                result.OverallSales = OutputDto.Select(_ => _.TotalAchievment).DefaultIfEmpty(0).Sum();

                return _resultService.SuccessObject(result);
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
                var salesContext = _emamiContext.Invoices.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId && DbFunctions.TruncateTime(_.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
            DbFunctions.TruncateTime(_.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)).OrderByDescending(_ => _.InvoiceDate).ToList();
                if (salesContext != null)
                {
                    decimal Achievement = 0;
                    foreach (var item in salesContext)
                    {
                        var invoiceDetailContext = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.InvoiceId == item.Id);
                        Achievement = Achievement + (invoiceDetailContext.Sum(_ => (decimal?)_.ActualBilledQuantity) ?? 0);
                    }
                    var usercontext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);

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

                    //var targetContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == inputDto.LoginUserId && _.MonthId == inputDto.FromDate.Month && _.Year == inputDto.FromDate.Year).ToList();
                    //if (targetContext != null)
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
                var userRoleContext = (from user in _emamiContext.Users.AsNoTracking()
                                       join role in _emamiContext.UserRoles.AsNoTracking() on user.Id equals role.UserId
                                       where role.RoleId == inputDto.RoleId
                                       select new UserMasterDto
                                       {
                                           Id = user.Id,
                                           EmployeeName = user.Name,
                                           EmployeeCode = user.Code
                                       }).ToList();


                if (userRoleContext != null)
                {
                    foreach (var user in userRoleContext)
                    {
                        var targetContext = _emamiContext.UserCustomerSalesTarget.AsNoTracking().Where(_ => _.AssignedToId == user.Id && _.MonthId == inputDto.FromDate.Month && _.Year == inputDto.FromDate.Year).ToList();
                        var dealerlist = (from ucm in _emamiContext.UserCustomerMapping.AsNoTracking()
                                          join u in _emamiContext.Users.AsNoTracking() on ucm.CustomerId equals u.Id
                                          where ucm.UserId == user.Id
                                          select ucm.CustomerId).ToList();

                        if (dealerlist != null)
                        {
                            var salesContext = _emamiContext.Invoices.AsNoTracking().Where(_ => dealerlist.Contains(_.UserId) && DbFunctions.TruncateTime(_.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                            DbFunctions.TruncateTime(_.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate)).ToList();

                            if (salesContext != null)
                            {
                                decimal Achievement = 0;
                                foreach (var item in salesContext)
                                {
                                    var invoiceDetailContext = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.InvoiceId == item.Id);
                                    Achievement = Achievement + invoiceDetailContext.Sum(_ => (decimal?)_.ActualBilledQuantity) ?? 0;
                                }
                                var usercontext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
                                if (targetContext != null)
                                {
                                    var acheivment = new OverallPerformanceByUserOutputDto
                                    {
                                        UserId = usercontext.Id,
                                        Usercode = usercontext.Code,
                                        Username = usercontext.Name,
                                        UserTarget = targetContext.Sum(_ => _.Target),
                                        UserAchievment = Achievement,
                                        AchievmentPercentage = targetContext.Sum(_ => _.Target) > 0 ? (Achievement / targetContext.Sum(_ => _.Target)) * 100 : 0
                                    };
                                    dashboardOverallsaudaOutpuDto.Add(acheivment);
                                }
                            }
                        }
                    }
                }
                if (dashboardOverallsaudaOutpuDto != null)
                {
                    OrderOutpuDto.AddRange(dashboardOverallsaudaOutpuDto.OrderBy(_ => _.AchievmentPercentage));
                    if (OrderOutpuDto != null)
                    {
                        int Rank = 0;
                        foreach (var item in OrderOutpuDto)
                        {
                            item.Rank = Rank + 1;
                        }
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


        #region Load Test

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
                if (inputDto == null)
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
                var dealerContext = _emamiContext.Users.FirstOrDefault(_ => _.Id == inputDto.DealerId);
                if (dealerContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var userdivContext = _emamiContext.UserDivisionMappings.AsNoTracking()
                           .FirstOrDefault(_ => _.UserId == inputDto.DealerId
                           && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId
                           && _.DivisionId == inputDto.DivisionId);


                decimal TotalQtyInMT = 0;
                foreach (var item in inputDto.SaudaOrders)
                {
                    TotalQtyInMT = TotalQtyInMT + _resultService.ConvertCasetoMetricTon(item.BidQuantity, item.SkuId);
                }
                var statuses = Constants.OverallSaudaStatus;
                var SaudaOutstandingContext = (from s in _emamiContext.Sauda.AsNoTracking()
                                               join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                                               where s.UserId == inputDto.DealerId
                                               && statuses.Contains(so.StatusId)
                                               select new { BidQuantity = so.BidQuantity, SkuId = so.SkuId }
                                               ).ToList();
                if (SaudaOutstandingContext != null && SaudaOutstandingContext.Any())
                {
                    decimal invoiceQuantity = 0;
                    var existingSaudaQuantity = SaudaOutstandingContext.Sum(_ => _.BidQuantity);
                    var skuIds = SaudaOutstandingContext.Select(_ => _.SkuId).Distinct().ToList();
                    var invoiceContext = (from inv in _emamiContext.Invoices.AsNoTracking()
                                          join invDet in _emamiContext.InvoiceDetails.AsNoTracking() on inv.Id equals invDet.InvoiceId
                                          where inv.UserId == inputDto.DealerId
                                          && skuIds.Contains(invDet.SkuId)
                                          select new
                                          {
                                              ActualBilledQuantity = invDet.ActualBilledQuantity
                                          }).ToList();

                    if (invoiceContext != null && invoiceContext.Any())
                    {
                        invoiceQuantity = invoiceContext.Sum(_ => _.ActualBilledQuantity);
                    }

                    var SaudaOutstanding = existingSaudaQuantity + TotalQtyInMT;
                    var SaudaLimit = (userdivContext.SaudaLimit ?? 0) + invoiceQuantity;
                    if (SaudaLimit < SaudaOutstanding)
                    {
                        return _resultService.ErrorMessage(Constants.SaudaLimitIsExceeds);
                    }
                }

                if (inputDto.SaudaOrders != null && inputDto.SaudaOrders.Any())
                {
                    var overallSaudaStatuses = Constants.OverallSaudaStatus;
                    foreach (var item in inputDto.SaudaOrders)
                    {

                        var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == item.SkuId);
                        if (skuContext != null && skuContext.DivisionId == (int)DTO.Enums.Division.SpecialityFat)
                        {
                            decimal availableQuantityBdo = 0;

                            var bdoContext = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.CustomerId == inputDto.DealerId)
                                             .Join(_emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.StateTrader), u => u.CustomerId, ur => ur.UserId, (u, ur) => new { u, ur }).ToList();

                            var bdoId = bdoContext.FirstOrDefault(_ => _.u.CustomerId == inputDto.DealerId)?.u.UserId;

                            SpecalityFatDiscountUser bdoLimitContext = null;
                            if (bdoId != null)
                            {
                                bdoLimitContext = _emamiContext.SpecalityFatDiscountUsers.AsNoTracking().FirstOrDefault(_ => _.UserId == bdoId && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
                                          && DbFunctions.TruncateTime(_.ValidTo) >= DbFunctions.TruncateTime(currentDate));
                                if (bdoLimitContext != null)
                                {
                                    decimal saudaBidQuantity = 0;
                                    List<long> dealerList = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur })
                                        .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.u.Id, uc => uc.CustomerId, (x, uc) => new { DealerId = x.u.Id, x.ur, uc })
                                        .Where(_ => _.uc.UserId == bdoId && (_.ur.RoleId == (int)DTO.Enums.Role.Dealer || _.ur.RoleId == (int)DTO.Enums.Role.Broker)).Select(_ => _.DealerId).ToList();
                                    if (dealerList != null && dealerList.Any())
                                    {
                                        saudaBidQuantity = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && _.SkuId == item.SkuId && dealerList.Contains(_.Sauda.UserId)
                                              && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(bdoLimitContext.ValidFrom)
                                              && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(bdoLimitContext.ValidTo) && overallSaudaStatuses.Contains(_.StatusId))
                                              .Select(s => s.BidQuantity).DefaultIfEmpty(0).Sum();
                                    }
                                    decimal requestedQuantityBdo = _resultService.ConvertCasetoMetricTon(item.BidQuantity, item.SkuId);
                                    decimal orderedQuantityBdo = 0;
                                    decimal totalQuantityBdo = requestedQuantityBdo;
                                    if (saudaBidQuantity != 0)
                                    {
                                        orderedQuantityBdo = saudaBidQuantity;
                                        totalQuantityBdo = orderedQuantityBdo + requestedQuantityBdo;
                                    }
                                    if (totalQuantityBdo > bdoLimitContext.ActualDiscount)
                                    {
                                        availableQuantityBdo = bdoLimitContext.ActualDiscount - orderedQuantityBdo;
                                        if (availableQuantityBdo < 0)
                                        {
                                            availableQuantityBdo = 0;
                                        }
                                        return _resultService.ErrorMessage(Constants.SkuBdoLimitExceeds.Replace(Constants.SkuName, skuContext.SkuName).Replace(Constants.Quantity, availableQuantityBdo.ToString()));

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

                        //if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                        //{
                        //    int CounterBidAllowCount = 0;
                        //    var CounterBidAllowContext = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Id == (int)DTO.Enums.Configuration.CounterBidCount);
                        //    if (CounterBidAllowContext != null)
                        //    {
                        //        CounterBidAllowCount = Convert.ToInt32(CounterBidAllowContext.Value);
                        //    }
                        //    var isSKuExistsContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.BiddingwindowId == item.BiddingwindowId && _.SkuId == item.SkuId
                        //        && _.OilTypeId == item.OilTypeId && _.Incoterms2 == item.IncotermsId && _.PlantId == item.PlantId).ToList();
                        //    if (isSKuExistsContext != null && isSKuExistsContext.Count >= CounterBidAllowCount)
                        //    {
                        //        return _resultService.ErrorMessage(Constants.SkuAlreadyBookedinBidding);
                        //    }

                        //    var TodayBiddingIds = _emamiContext.BiddingWindowTiming.AsNoTracking().Where(_ => _.BiddingDate == DbFunctions.TruncateTime(currentDate));
                        //    if (TodayBiddingIds != null)
                        //    {
                        //        var SaudaContext = (from sauda in _emamiContext.Sauda
                        //                            join saudaorder in _emamiContext.SaudaOrders on sauda.Id equals saudaorder.SaudaId
                        //                            join biddings in TodayBiddingIds on saudaorder.BiddingwindowId equals biddings.Id
                        //                            where sauda.UserId == inputDto.DealerId && saudaorder.StatusId == (int)DTO.Enums.Status.Hold
                        //                            && saudaorder.SkuId == item.SkuId
                        //                            && saudaorder.OilTypeId == item.OilTypeId && saudaorder.Incoterms2 == item.IncotermsId && saudaorder.PlantId == item.PlantId
                        //                            select saudaorder
                        //                        ).ToList();

                        //        if (SaudaContext.Count > 1)
                        //        {
                        //            return _resultService.ErrorMessage(Constants.SaudaHoldMessage);
                        //        }
                        //    }
                        //}
                    }
                }


                var statusId = (int)DTO.Enums.Status.Pending;
                if (inputDto.SaudaOrders != null && inputDto.SaudaOrders.Any())
                {
                    foreach (var item in inputDto.SaudaOrders)
                    {
                        var status = 0;
                        var pricingContext = _emamiContext.Pricing.AsNoTracking().FirstOrDefault(_ => _.Id == item.PricingId);
                        if (pricingContext != null)
                        {
                            var cleranceRate = (decimal)0;
                            var baseRate = (decimal)0;
                            //if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ExDepot)
                            //{
                            //    cleranceRate = pricingContext.ExDepotPrice * pricingContext.CounterBidLimit;
                            //    baseRate = pricingContext.ExDepotPrice;
                            //}
                            //else if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ExPlant)
                            //{
                            //    cleranceRate = pricingContext.ExPlantPrice * pricingContext.CounterBidLimit;
                            //    baseRate = pricingContext.ExPlantPrice;
                            //}
                            //else if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ForDepot)
                            //{
                            //    cleranceRate = pricingContext.ForDepotPrice * pricingContext.CounterBidLimit;
                            //    baseRate = pricingContext.ForDepotPrice;
                            //}
                            //else if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ForPlant)
                            //{
                            //    cleranceRate = pricingContext.ForPlantPrice * pricingContext.CounterBidLimit;
                            //    baseRate = pricingContext.ForPlantPrice;
                            //}
                            //else if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ExRake)
                            //{
                            //    cleranceRate = pricingContext.ExRakePrice * pricingContext.CounterBidLimit;
                            //    baseRate = pricingContext.ExRakePrice;
                            //}
                            //else if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ForRake)
                            //{
                            //    cleranceRate = pricingContext.ForRakePrice * pricingContext.CounterBidLimit;
                            //    baseRate = pricingContext.ForRakePrice;
                            //}

                            //if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                            //{
                            //    if (item.BidPrice < cleranceRate)
                            //        status = (int)DTO.Enums.Status.Rejected;
                            //    else if (item.BidPrice >= cleranceRate && item.BidPrice <= baseRate)
                            //        status = (int)DTO.Enums.Status.Hold;
                            //    else if (item.BidPrice > baseRate)
                            //        status = (int)DTO.Enums.Status.Pending;
                            //}
                            //else
                            //{
                            status = (int)DTO.Enums.Status.Pending;
                            //}
                            item.StatusId = status;
                        }
                    }

                    if (inputDto.SaudaOrders.Any(_ => _.StatusId == (int)DTO.Enums.Status.Hold))
                        statusId = (int)DTO.Enums.Status.Hold;
                    else if (inputDto.SaudaOrders.Any(_ => _.StatusId == (int)DTO.Enums.Status.Rejected))
                        statusId = (int)DTO.Enums.Status.Rejected;

                }

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


                var saudaContext = new Sauda
                {

                    BiddingDate = currentDate,
                    UserId = inputDto.DealerId,

                    SaudaBookingTypeId = inputDto.SaudaBookingTypeId,

                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = currentDate,
                    IsSAPDataSync = false,
                    IsSAPDataSyncApproval = false

                };
                _emamiContext.Sauda.Add(saudaContext);
                _emamiContext.SaveChanges();

                List<SaudaCreateNotificationDto> saudaCreateEmailList = new List<SaudaCreateNotificationDto>();
                if (inputDto.SaudaOrders != null && inputDto.SaudaOrders.Any())
                {
                    int i = 0;
                    foreach (var item in inputDto.SaudaOrders)
                    {
                        DateTime? saudaValidFromDate = currentDate;
                        long? depotIdForRake = 0;
                        if (item.IncotermsId == (int)DTO.Enums.IncoTerms.ExRake || item.IncotermsId == (int)DTO.Enums.IncoTerms.ExRake)
                        {
                            depotIdForRake = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == item.PlantId && !_.IsPlant)?.DepotId;
                            if (item.SaudaValidFromDate != null)
                                saudaValidFromDate = item.SaudaValidFromDate;
                        }

                        var IncotermContext = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == item.IncotermsId).Name;
                        IncotermsType = IncotermContext.ToLower().Contains("for") ? "For" : "Ex";


                        item.DiscountAmount = item.BidQuantity * item.DiscountAmount;
                        decimal itemquotedprice = item.BidQuantity * item.QuotedPrice;
                        item.QuotedPrice = itemquotedprice;
                        item.BidPrice = //inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction ?
                                        // item.BidQuantity * item.BidPrice : 
                            itemquotedprice;

                        if (item.DiscountTypeId == 1)
                        {
                            item.BidPrice = item.BidPrice - item.DiscountAmount;
                        }
                        else
                        {
                            item.BidPrice = item.BidPrice + item.DiscountAmount;
                        }

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
                            CreatedDate = currentDate,
                            //BiddingwindowId = item.BiddingwindowId,
                            SaudaBookingTypeId = inputDto.SaudaBookingTypeId,
                            PricingId = item.PricingId,
                            //DealerTypeId = DealerTypeId,
                            Incoterms1 = IncotermsType,
                            PlantId = item.PlantId,
                            //DealerLocationId = Convert.ToInt64(dealerContext.FreightRouteId),
                           // CustomerPONumber = dealerContext.Code + currentDate.ToShortDateString(),
                            ValidFromDate = saudaValidFromDate.Value,
                            ValidToDate = saudaValidFromDate.Value.AddDays(Convert.ToDouble(dealerContext.SaudaValidityPeriod > 0 ? dealerContext.SaudaValidityPeriod : Config.DefaultSaudaValidity)),
                            StatusId = statusId,
                           // SaudaStatusId = (int)DTO.Enums.SaudaStatus.NotReleased,
                            Incoterms2 = item.IncotermsId,
                            BrokerId = BrokerId,
                            IsSAPDataSync = false,
                            IsSAPDataSyncApproval = false,
                            QuotedPriceBeforeSAPDiscount = item.BidQuantity == 0 ? 0m : item.BidPrice / item.BidQuantity
                            // DepotIdForRake = depotIdForRake.Value
                        };

                        _emamiContext.SaudaOrders.Add(saudaOrder);
                        _emamiContext.SaveChanges();

                        saudaCreateEmailList.Add(new SaudaCreateNotificationDto()
                        {
                            StatusId = item.StatusId,
                            SaudaOrderId = saudaOrder.Id,
                            SaudaBookingTypeId = saudaOrder.SaudaBookingTypeId,
                            SaudaOrderStatusId = saudaOrder.StatusId,
                            LoginUserId = inputDto.LoginUserId
                        });
                    }

                    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => SaudaCreateNotificationAsync(saudaCreateEmailList, cancellationToken));

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
            string mStartTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
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
                            loginUserId = saudaData.LoginUserId;
                            var usersContext = _context.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaData.LoginUserId);
                            var saudaOrderContext = _context.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.Id == saudaData.SaudaOrderId);
                            if (usersContext != null && saudaOrderContext != null)
                            {
                                List<string> toUsers = new List<string>();
                                if (!string.IsNullOrEmpty(usersContext.Email))
                                {
                                    toUsers.Add(usersContext.Email);
                                }
                                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();

                                bool isEmail = false;
                                var IsEmail = _context.Configurations.AsNoTracking().Where(_ => _.Id == (int)DTO.Enums.Configuration.IsEMAIL).Select(_ => _.Value).Single();
                                if (IsEmail.Equals("1") || IsEmail.Equals("True"))
                                    isEmail = true;
                                else
                                    isEmail = false;

                                if (isEmail && toUsers != null && toUsers.Any())
                                {
                                    var fromEmail = Constants.FromEmail;
                                    string emailSubject = string.Empty;
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
                                            .Replace(Constants.BY_FOR, Constants.BY).Replace(Constants.UserName, usersContext.Name);
                                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
                                        amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
                                    }

                                }
                                var smsPlainTemplate = string.Empty;

                                bool isSms = false;
                                var IsSMS = _context.Configurations.AsNoTracking().Where(_ => _.Id == (int)DTO.Enums.Configuration.IsSMS).Select(_ => _.Value).Single();
                                if (IsSMS.Equals("1") || IsSMS.Equals("True"))
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
                                            .Replace(Constants.BY_FOR, Constants.BY).Replace(Constants.UserName, usersContext.Name);
                                        smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
                                        if (!string.IsNullOrEmpty(usersContext.MobileNumber))
                                        {
                                            amazonNotificationService.SendMessage(smsMessage, usersContext.MobileNumber, smsTemplate.SMSTemplateID);
                                        }
                                    }
                                }

                                bool isPushNotification = false;
                                var IsPushNotification = _context.Configurations.AsNoTracking().Where(_ => _.Id == (int)DTO.Enums.Configuration.IsPushNotification).Select(_ => _.Value).Single();
                                if (IsPushNotification.Equals("1") || IsPushNotification.Equals("True"))
                                    isPushNotification = true;
                                else
                                    isPushNotification = false;

                                //if (isPushNotification && saudaData.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
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
                                //        //_notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                //        SendPushNotificationThroughFirebase(pushNotificationInputDto);
                                //    }
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


            string mEndTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            TimeSpan timeSpan = Convert.ToDateTime(mEndTime) - Convert.ToDateTime(mStartTime);
            int mTotalMilliSeconds = (int)timeSpan.TotalMilliseconds;
            string logData = $"EmailSaudaCreation, StartTime, {mStartTime}, EndTime, {mEndTime}, EmailSendTotalTime, {mTotalMilliSeconds}, LoginUserId, {loginUserId}";
            string serverFoloderPath = HostingEnvironment.MapPath("~/LogFiles/");
            string filePath = Path.Combine(serverFoloderPath + "SaudaCreateEmail.txt");
            File.AppendAllText(filePath, logData + Environment.NewLine);
        }


        #endregion
    }
}
