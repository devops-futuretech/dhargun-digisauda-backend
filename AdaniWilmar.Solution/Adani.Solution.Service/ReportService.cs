using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using Dapper;
using GMCore.Helper;
using GMCore.Logger;
using Kendo.Mvc.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace Adani.Solution.Service
{
    public interface IReportService
    {
        ResultDto OilPriceReport(OilPriceReportInputDto inputDto);
        ResultDto GetSaudaBookingReport(SaudaReportFilterDto inputDto);
        //ResultDto GetCounterBidOfferReport(SaudaReportFilterDto inputDto);

        ResultDto CostChangeReport(ReportInputDto inputDto);
        ResultDto GetSalesReport(SalesReportInputDto inputDto);
        ResultDto GetBDOWiseSalesReport(SalesReportInputDto inputDto);

        ResultDto GetSaudaOrderDetailsReport(SaudaOrderReportInputputDto inputDto);
        //ResultDto GetSaudaOrderDetailsReportForMobile(SaudaOrderReportInputputDto inputDto);

        ResultDto GetBDOWiseSaudaReport(SaudaOrderReportInputputDto inputDto);
        ResultDto GetCustomerSaudaLimitReport(ReportFilterDto inputDto);

        ResultDto IndentListReport(IndentReportInputDto inputDto);

        ResultDto GetMTPDetailsReport(MonthlyTourPlanReportInputDto inputDto);
        ResultDto GetPCPDetailsReport(PermanentCoveragePlanReportInputDto inputDto);
        ResultDto GetPendingSaudaReport(PendingSaudaReportInput inputDto);

        ResultDto GetPendingContractExport(PendingContractReportDto inputDto);
        ResultDto GetPendingContractReport(PendingContractReportDto inputDto);
        ResultDto GetVerticalId(long userId);
        ResultDto GetOilTypesPendingContractReport(LoginUserIdDto inputDto);
        ResultDto GetPendingContractReportForMobile(PendingContractReportInputDto inputDto);
        ResultDto GetPendingContractReportForManager(PendingContractReportInputDto inputDto);
        ResultDto GetSaudaCallRecordMappingAttachments(long saudaId);
        //ResultDto GetDailyBookingReport(SaudaOrderReportInputputDto inputDto);
        ResultDto GetCreditLimitReport(ReportFilterDto inputDto);

        //ResultDto GetTruckPlacementTrackerReport(TruckReportInputDto inputDto);
        //ResultDto GetTruckPlacementTrackerReportAPP(TruckReportInputDto inputDto);
        ResultDto GetPendingContractReportForManagerAPP(PendingContractReportInputDto inputDto);

        ResultDto GetSchemeGeographyDetailsReport(SchemeGeographyReportInputputDto inputDto);
        ResultDto GetDemandPlanBillingDetailsReport(DemandPlanBillingReportInputputDto inputDto);

        ResultDto GetDistributorStockReport(DistributorStockReportInputDto inputDto);
    }

    public class ReportService : IReportService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Report Service");
        private const string ServiceName = "Report Service";
        private string _methodName;
        private readonly IResultService _resultService;

        public ReportService(IAdaniContext emamiContext, IResultService resultService)
        {
            try
            {
                _emamiContext = emamiContext;
                _resultService = resultService;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for Report Service", exception);
            }
        }

        /// <summary>
        /// Distributor stock report - all stock entries reported from the mobile app in the
        /// selected date range. Data is scoped to the distributors under the logged in
        /// trader's hierarchy; Admin sees all distributors.
        /// </summary>
        public ResultDto GetDistributorStockReport(DistributorStockReportInputDto inputDto)
        {
            _methodName = "GetDistributorStockReport";
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

                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                if (userRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                var roleId = userRoleContext.RoleId;

                var bdoIds = new List<long>();
                var customerIds = new List<long>();

                if (roleId == (int)DTO.Enums.Role.NationalTrader)
                {
                    var zhIds = _emamiContext.UserReportingToMappings.AsNoTracking()
                        .Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                    bdoIds = _emamiContext.UserReportingToMappings.AsNoTracking()
                        .Where(_ => zhIds.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();
                }
                if (roleId == (int)DTO.Enums.Role.ZonalTrader)
                {
                    bdoIds = _emamiContext.UserReportingToMappings.AsNoTracking()
                        .Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                }
                if (roleId == (int)DTO.Enums.Role.StateTrader)
                {
                    bdoIds.Add(inputDto.LoginUserId);
                }
                if (bdoIds.Any())
                {
                    customerIds = _emamiContext.UserCustomerMapping.AsNoTracking()
                        .Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
                }

                var stockQuery = from e in _emamiContext.DistributorStockEntries.AsNoTracking()
                                 join d in _emamiContext.DistributorStockEntryDetails.AsNoTracking() on e.Id equals d.DistributorStockEntryId
                                 join s in _emamiContext.Skus.AsNoTracking() on d.SkuId equals s.Id
                                 join u in _emamiContext.Users.AsNoTracking() on e.UserId equals u.Id
                                 where DbFunctions.TruncateTime(e.ReportedDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                                    && DbFunctions.TruncateTime(e.ReportedDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
                                 select new { e, d, s, u };

                if (roleId != (int)DTO.Enums.Role.Admin)
                {
                    stockQuery = stockQuery.Where(x => customerIds.Contains(x.e.UserId));
                }
                if (inputDto.StateIds != null && inputDto.StateIds.Any())
                {
                    stockQuery = stockQuery.Where(x => inputDto.StateIds.Contains(x.u.StateId));
                }
                if (inputDto.SalesOrganizationId > 0)
                {
                    stockQuery = stockQuery.Where(x => x.s.SalesOrganizationId == inputDto.SalesOrganizationId);
                }
                if (inputDto.DistributionChannelId > 0)
                {
                    stockQuery = stockQuery.Where(x => x.s.DistributionChannelId == inputDto.DistributionChannelId);
                }
                if (inputDto.VerticalId > 0)
                {
                    stockQuery = stockQuery.Where(x => x.s.DivisionId == inputDto.VerticalId);
                }
                if (inputDto.OilTypeId > 0)
                {
                    stockQuery = stockQuery.Where(x => x.s.OilTypeId == inputDto.OilTypeId);
                }

                var stockRows = stockQuery
                    .OrderByDescending(x => x.e.ReportedDate).ThenBy(x => x.u.Name)
                    .Select(x => new
                    {
                        SalesOrganization = x.s.SalesOrganization.Name,
                        DistributionChannel = x.s.DistributionChannel.Name,
                        Division = x.s.Division.Name,
                        DistributorName = x.u.Name,
                        DistributorCode = x.u.Code,
                        OilType = x.s.OilType != null ? x.s.OilType.Name : string.Empty,
                        MaterialName = x.s.SkuName,
                        MaterialCode = x.s.SkuCode,
                        QtyInCase = x.d.QuantityInCase,
                        QtyInMT = x.d.QuantityInMT,
                        ReportedDate = x.e.ReportedDate
                    }).ToList();

                if (!stockRows.Any())
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                var reportData = stockRows.Select(x => new DistributorStockReportOutputDto
                {
                    SalesOrganization = x.SalesOrganization,
                    DistributionChannel = x.DistributionChannel,
                    Division = x.Division,
                    DistributorName = x.DistributorName,
                    DistributorCode = x.DistributorCode,
                    OilType = x.OilType,
                    MaterialName = x.MaterialName,
                    MaterialCode = x.MaterialCode,
                    QtyInCase = x.QtyInCase,
                    QtyInMT = x.QtyInMT,
                    ReportedDateTime = x.ReportedDate.ToString("dd-MMM-yyyy hh:mm tt")
                }).ToList();

                return _resultService.SuccessObject(reportData);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto OilPriceReport(OilPriceReportInputDto inputDto)
        {
            _methodName = "OilPriceReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            try
            {
                var data = new List<Pricing>();
                if (inputDto.SkuId != null)
                {
                    data = _emamiContext.Pricing.Include(s => s.Sku).Where(s => //s.SaudaBookingTypeId == inputDto.SaudaBookingTypeId 
                                                                                // && s.OilTypeId == inputDto.OilTypeId && s.StateId == inputDto.StateId && s.CityId == inputDto.CityId && s.TransportModeId == inputDto.TransportModeId && s.OilPackingTypeId == inputDto.OilPackingTypeId 
                     s.PlantId == inputDto.PlantId
                   //&& s.DepotId == inputDto.DepotId 
                   //&& s.FrieghtZoneId == inputDto.FreightZoneId && s.FrieghtRouteId == s.FrieghtRouteId &&
                   && inputDto.SkuId.Contains(s.SkuId)
                   // &&
                   //DbFunctions.TruncateTime(s.BiddingDate) >= inputDto.FromDate && DbFunctions.TruncateTime(s.BiddingDate) <= inputDto.ToDate
                   ).ToList();
                }
                else
                {
                    data = data = _emamiContext.Pricing.Include(s => s.Sku).Where(s => //s.SaudaBookingTypeId == inputDto.SaudaBookingTypeId 
                    //&& s.OilTypeId == inputDto.OilTypeId && s.StateId == inputDto.StateId && s.CityId == inputDto.CityId && s.TransportModeId == inputDto.TransportModeId && s.OilPackingTypeId == inputDto.OilPackingTypeId 
                     s.PlantId == inputDto.PlantId
                   //&& s.DepotId == inputDto.DepotId 
                   //&& s.FrieghtZoneId == inputDto.FreightZoneId && s.FrieghtRouteId == s.FrieghtRouteId &&
                   //DbFunctions.TruncateTime(s.BiddingDate) >= inputDto.FromDate && DbFunctions.TruncateTime(s.BiddingDate) <= inputDto.ToDate
                   ).ToList();
                }
                var incoTermName = _emamiContext.IncoTerms.FirstOrDefault(s => s.Id == inputDto.IncoTermId).Name;
                var res = new DataTable();
                res.Columns.Add("Sku Name");

                var processdate = inputDto.FromDate;
                var dates = new List<DateTime>();


                while (inputDto.ToDate >= processdate)
                {

                    dates.Add(processdate);
                    res.Columns.Add(processdate.ToShortDateString() + " (" + incoTermName + ")");
                    processdate = processdate.AddDays(1);
                }
                foreach (var rec in data.GroupBy(s => s.Sku))
                {
                    var row = res.NewRow();
                    foreach (var dt in dates)
                    {

                        row["Sku Name"] = rec.Key.SkuName;
                        var obj = data.FirstOrDefault(s =>
                        //s.BiddingDate.Date == dt                         && 
                        s.SkuId == rec.Key.Id);
                        var colName = dt.ToShortDateString() + " (" + incoTermName + ")";
                        if (obj != null)
                        {

                            switch (inputDto.IncoTermId)
                            {
                                ////Check the Incoterms table 
                                //case 1: // ForPlant
                                //    row[colName] = obj.ForPlantPrice;
                                //    break;
                                //case 2: //ExPlantPrice
                                //    row[colName] = obj.ExPlantPrice;
                                //    break;
                                //case 3: //ForDepotPrice
                                //    row[colName] = obj.ForDepotPrice;
                                //    break;
                                //case 4: //ExDepotPrice
                                //    row[colName] = obj.ExDepotPrice;
                                //    break;
                                //case 5: //ForRakePrice
                                //    row[colName] = obj.ForRakePrice;
                                //    break;
                                //case 6: //ExRakePrice 
                                //    row[colName] = obj.ExRakePrice;
                                //    break;

                            }
                        }
                        else
                        {
                            row[colName] = 0;
                        }

                    }
                    res.Rows.Add(row);
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = res;
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

        public ResultDto CostChangeReport(ReportInputDto inputDto)
        {
            _methodName = "CostChangeReport";
            _logger.Info($"{ServiceName} Controller-Method {_methodName}");
            var resultDto = new ResultDto();
            try
            {
                var oilType = _emamiContext.OilTypes.FirstOrDefault(s => s.Id == inputDto.OilTypeId).Name;
                var skus = default(List<Sku>);
                if (inputDto.SkuId != null)
                {
                    skus = _emamiContext.Skus.Where(sk => inputDto.SkuId.Contains(sk.Id)).ToList();
                }
                var res = default(DataTable);
                switch (inputDto.CostType)
                {
                    case CostType.PackingCost:
                        res = CalculatePackingCostRate(inputDto);
                        break;
                    case CostType.HoneyCombCost:
                        res = CalculateHoneyCombCostRate(inputDto);
                        break;
                    case CostType.SchemeCost:
                        res = CalculateSchemeCostRate(inputDto);
                        break;
                    case CostType.CushionMarginCost:
                        res = CalculateCushionMarginCostRate(inputDto);
                        break;
                }
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = res;
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

        public ResultDto GetSaudaBookingReport(SaudaReportFilterDto inputDto)
        {
            _methodName = "GetSaudaBookingReport";
            var saudaReportOutputDto = new List<SaudaReportOutputDto>();
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
                if (inputDto.IncotermsId == 0)
                {
                    return _resultService.ErrorMessage(Constants.IncotermsMissing);
                }
                if (inputDto.BookingTypeId == 0)
                {
                    return _resultService.ErrorMessage(Constants.BookingTypeMissing);
                }
                var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Incoterms2 == inputDto.IncotermsId && _.Sauda != null
                      && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(inputDto.FromDate) && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(inputDto.ToDate));
                if (inputDto.DealerId != 0)
                {
                    saudaOrderContext = saudaOrderContext.AsNoTracking().Where(_ => _.Sauda.UserId == inputDto.DealerId);
                }
                if (inputDto.OilTypeId != 0)
                {
                    saudaOrderContext = saudaOrderContext.AsNoTracking().Where(_ => _.OilTypeId == inputDto.OilTypeId);
                }
                if (inputDto.SkuId != 0)
                {
                    saudaOrderContext = saudaOrderContext.AsNoTracking().Where(_ => _.SkuId == inputDto.SkuId);
                }

                if (saudaOrderContext != null && saudaOrderContext.Any())
                {
                    saudaReportOutputDto = saudaOrderContext.AsNoTracking().Join(_emamiContext.Sauda.AsNoTracking(), so => so.SaudaId, s => s.Id, (so, s) => new { so, s })
                        .Join(_emamiContext.Users.AsNoTracking(), sos => sos.s.UserId, u => u.Id, (sos, u) => new { sos.so, sos.s, u })
                        .Join(_emamiContext.ApprovalStatus.AsNoTracking(), sosu => sosu.so.StatusId, sts => sts.Id, (sosu, sts) => new { sosu.so, sosu.s, sosu.u, sts })
                        .GroupJoin(_emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected), sosus => sosus.so.Id, lr => lr.SaudaOrderId, (sosus, lr) => new { sosus.so, sosus.s, sosus.u, sosus.sts, lr })
                        .Where(_ => _.so.SaudaBookingTypeId == inputDto.BookingTypeId && _.so != null && _.s != null && _.u != null).OrderByDescending(_ => _.s.BiddingDate).ToList()
                        .Select(_ => new SaudaReportOutputDto()
                        {
                            SaudaOrderId = _.so.Id,
                            OilTypeName = _.so.OilType.Name,
                            SkuName = _.so.Sku.SkuName,
                            DealerName = _.u.Name,
                            BookingDate = _.s.BiddingDate,
                            BookingQuantity = _.so.BidQuantity,
                            BookingQuantityCase = _.so.BidQuantityCase,
                            BookingPrice = _.so.BidPrice,
                            Status = _.sts.Name,
                            Remarks = _.so.Remarks,
                            LiftedQuantity = _.lr != null && _.lr.Any() ? _.lr.Select(s => s.LiftingQuantity).DefaultIfEmpty(0).Sum() : 0,
                            LiftedQuantityCase = _.lr != null && _.lr.Any() ? _.lr.Select(s => s.LiftingQuantityCase).DefaultIfEmpty(0).Sum() : 0,
                        }).ToList();
                }

                if (saudaReportOutputDto != null && saudaReportOutputDto.Any())
                {
                    return _resultService.SuccessObject(saudaReportOutputDto);
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

        //public ResultDto GetCounterBidOfferReport(SaudaReportFilterDto inputDto)
        //{
        //    _methodName = "GetCounterBidOfferReport";
        //    var counterBidReportOutputDto = new List<SaudaReportOutputDto>();
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }
        //        if (inputDto.LoginUserId == 0)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserIdMissing);
        //        }
        //        var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
        //        if (userContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserNotFound);
        //        }
        //        if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
        //        {
        //            return _resultService.ErrorMessage(Constants.FromDateEmpty);
        //        }
        //        if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
        //        {
        //            return _resultService.ErrorMessage(Constants.ToDateEmpty);
        //        }
        //        if (inputDto.IncotermsId == 0)
        //        {
        //            return _resultService.ErrorMessage(Constants.IncotermsMissing);
        //        }
        //        var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Incoterms2 == inputDto.IncotermsId && _.Sauda != null && _.CounterBidOffer != 0
        //              && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(inputDto.FromDate) && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(inputDto.ToDate));
        //        if (inputDto.DealerId != 0)
        //        {
        //            saudaOrderContext = saudaOrderContext.AsNoTracking().Where(_ => _.Sauda.UserId == inputDto.DealerId);
        //        }
        //        if (inputDto.OilTypeId != 0)
        //        {
        //            saudaOrderContext = saudaOrderContext.AsNoTracking().Where(_ => _.OilTypeId == inputDto.OilTypeId);
        //        }
        //        if (inputDto.SkuId != 0)
        //        {
        //            saudaOrderContext = saudaOrderContext.AsNoTracking().Where(_ => _.SkuId == inputDto.SkuId);
        //        }

        //        if (saudaOrderContext != null && saudaOrderContext.Any())
        //        {
        //            counterBidReportOutputDto = saudaOrderContext.AsNoTracking().Join(_emamiContext.Sauda.AsNoTracking(), so => so.SaudaId, s => s.Id, (so, s) => new { so, s })
        //                .Join(_emamiContext.Users.AsNoTracking(), sos => sos.s.UserId, u => u.Id, (sos, u) => new { sos.so, sos.s, u })
        //                .Join(_emamiContext.ApprovalStatus.AsNoTracking(), sosu => sosu.so.StatusId, sts => sts.Id, (sosu, sts) => new { sosu.so, sosu.s, sosu.u, sts })
        //                .Where(_ => _.so != null && _.s != null && _.u != null && _.sts != null).OrderByDescending(_ => _.s.BiddingDate).ToList()
        //                .Select(_ => new SaudaReportOutputDto()
        //                {
        //                    SaudaOrderId = _.so.Id,
        //                    OilTypeName = _.so.OilType.Name,
        //                    SkuName = _.so.Sku.SkuName,
        //                    DealerName = _.u.Name,
        //                    BookingDate = _.s.BiddingDate,
        //                    BookingQuantity = _.so.BidQuantity,
        //                    BookingQuantityCase = _.so.BidQuantityCase,
        //                    BookingPrice = _.so.BidPrice,
        //                    Status = _.sts.Name,
        //                    Remarks = _.so.Remarks,
        //                    CounterBidOffer = _.so.CounterBidOffer,
        //                }).ToList();
        //        }

        //        if (counterBidReportOutputDto != null && counterBidReportOutputDto.Any())
        //        {
        //            return _resultService.SuccessObject(counterBidReportOutputDto);
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

        #region Sales Report
        public ResultDto GetSalesReport(SalesReportInputDto inputDto)
        {
            var salesReportOutputDto = new List<SalesReportOutputDto>();
            _methodName = "GetSalesReport";
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
                var invoiceListContext = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.Invoice != null && DbFunctions.TruncateTime(_.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    && DbFunctions.TruncateTime(_.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate));
                if (inputDto.DealerId != 0)
                {
                    invoiceListContext = invoiceListContext.AsNoTracking().Where(_ => _.Invoice.UserId == inputDto.DealerId);
                }
                if (inputDto.OilTypeId != 0)
                {
                    invoiceListContext = invoiceListContext.AsNoTracking().Where(_ => _.OilTypeId == inputDto.OilTypeId);
                }
                if (inputDto.SkuId != 0)
                {
                    invoiceListContext = invoiceListContext.AsNoTracking().Where(_ => _.SkuId == inputDto.SkuId);
                }
                if (invoiceListContext != null && invoiceListContext.Any())
                {
                    salesReportOutputDto = invoiceListContext.AsNoTracking().Join(_emamiContext.Invoices.AsNoTracking(), ivd => ivd.InvoiceId, i => i.Id, (ivd, i) => new { ivd, i })
                       .Join(_emamiContext.Users.AsNoTracking(), ivdi => ivdi.i.UserId, u => u.Id, (ivdi, u) => new { ivdi.ivd, ivdi.i, u })
                       .Join(_emamiContext.Skus.AsNoTracking(), ivdiu => ivdiu.ivd.SkuId, s => s.Id, (ivdiu, s) => new { ivdiu.ivd, ivdiu.i, ivdiu.u, s })
                       .Join(_emamiContext.OilTypes.AsNoTracking(), ivdius => ivdius.ivd.OilTypeId, o => o.Id, (ivdius, o) => new { ivdius.ivd, ivdius.i, ivdius.u, ivdius.s, o })
                       .Where(_ => _.ivd != null && _.i != null && _.s != null && _.o != null && _.u != null).GroupBy(g => g.i.Id).Select(_ => new SalesReportOutputDto()
                       {
                           DealerId = _.FirstOrDefault().i.UserId,
                           DealerName = _.FirstOrDefault().u.Name,
                           SkuId = _.FirstOrDefault().ivd.SkuId,
                           SkuName = _.FirstOrDefault().s.SkuName,
                           OilTypeId = _.FirstOrDefault().ivd.OilTypeId,
                           OilTypeName = _.FirstOrDefault().o.Name,
                           Quantity = _.Select(s => s.ivd.ActualBilledQuantity).DefaultIfEmpty(0).Sum(),
                          // Price = _.Select(s => s.ivd.SKUInvoiceTax).DefaultIfEmpty(0).Sum(),
                       }).ToList();
                }

                if (salesReportOutputDto != null && salesReportOutputDto.Any())
                {
                    return _resultService.SuccessObject(salesReportOutputDto);
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

        public ResultDto GetBDOWiseSalesReport(SalesReportInputDto inputDto)
        {
            var salesReportOutputDto = new List<SalesBDOWiseReportDto>();
            _methodName = "GetBDOWiseSalesReport";
            try
            {
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
                if (inputDto.BDOIds == null || !inputDto.BDOIds.Any())
                {
                    return _resultService.ErrorMessage(Constants.SalesPersonMissing);
                }
                List<long> UserIds = new List<long>();
                UserIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => inputDto.BDOIds.Contains(_.UserId)).Select(_ => _.CustomerId).Distinct().ToList();
                //StateTrader Filter
                var invoiceListContext = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.Invoice != null && DbFunctions.TruncateTime(_.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    && DbFunctions.TruncateTime(_.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && UserIds.Contains(_.Invoice.UserId));
                //BP-CP-ALL
                if (inputDto.PackTypeId != 0)
                {
                    invoiceListContext = invoiceListContext.AsNoTracking().Join(_emamiContext.Skus.AsNoTracking().Where(_ => _.PackGroupId == inputDto.PackTypeId),
                        x => x.SkuId, s => s.Id, (x, s) => new { x }).Select(_ => _.x);
                }

                if (invoiceListContext != null && invoiceListContext.Any())
                {

                    salesReportOutputDto = invoiceListContext.Join(_emamiContext.Users.AsNoTracking(), x => x.Invoice.UserId, u => u.Id, (x, u) => new { InvoiceDetail = x, PartyName = u.Name, PartyCode = u.Code })
                        .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.InvoiceDetail.Invoice.UserId, uc => uc.CustomerId, (x, uc) => new { x.InvoiceDetail, x.PartyCode, x.PartyName, BDOId = uc.UserId })
                        .Join(_emamiContext.Users.AsNoTracking(), x => x.BDOId, u => u.Id, (x, u) => new { x.InvoiceDetail, x.PartyCode, x.PartyName, BDOName = u.Name, BDOCode = u.Code })
                        .Join(_emamiContext.OilTypes.AsNoTracking(), x => x.InvoiceDetail.OilTypeId, ot => ot.Id, (x, ot) => new { x.InvoiceDetail, x.PartyCode, x.PartyName, x.BDOCode, x.BDOName, OilType = ot.Name })
                        .Join(_emamiContext.Skus.AsNoTracking(), x => x.InvoiceDetail.SkuId, s => s.Id, (x, s) => new { x.InvoiceDetail, x.PartyCode, x.PartyName, x.BDOCode, x.BDOName, x.OilType, PackGroupId = s.PackGroupId })
                        .GroupBy(_ => new { _.InvoiceDetail.Invoice.UserId, _.InvoiceDetail.OilTypeId }).Select(_ => new SalesBDOWiseReportDto
                        {
                            BDOCode = _.FirstOrDefault().BDOCode,
                            BDOName = _.FirstOrDefault().BDOName,
                            DealerCode = _.FirstOrDefault().PartyCode,
                            DealerName = _.FirstOrDefault().PartyName,
                            OilTypeName = _.FirstOrDefault().OilType,
                            // BPInMT = _.Where(w => w.PackGroupId == (int)DTO.Enums.PackGroupType.Premium).Sum(s => s.InvoiceDetail.ActualBilledQuantity),
                            //BPInCase = _.Where(w => w.PackGroupId == (int)DTO.Enums.PackGroupType.Premium).Sum(s => s.InvoiceDetail.QuantityInCase),
                            //CPInCase = _.Where(w => w.PackGroupId == (int)DTO.Enums.PackGroupType.Bakery).Sum(s => s.InvoiceDetail.QuantityInCase),
                            //CPInMT = _.Where(w => w.PackGroupId == (int)DTO.Enums.PackGroupType.Bakery).Sum(s => s.InvoiceDetail.ActualBilledQuantity),
                            TotalSalesInCase = _.Sum(s => s.InvoiceDetail.QuantityInCase),
                            TotalSalesInMT = _.Sum(s => s.InvoiceDetail.ActualBilledQuantity),
                        }).OrderBy(_ => _.BDOName).ThenBy(_ => _.DealerName).ThenBy(_ => _.OilTypeName).ToList();
                }

                return _resultService.SuccessObject(salesReportOutputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }
        #endregion

        private DataTable CalculateSchemeCostRate(ReportInputDto inputDto)
        {
            var data = _emamiContext.SchemeCosts.Where(s => s.OilTypeId == inputDto.OilTypeId && (DbFunctions.TruncateTime(s.ValidFrom) >= inputDto.FromDate || DbFunctions.TruncateTime(s.ValidTo) <= inputDto.ToDate)).ToList();
            var dt = new DataTable();
            dt.Columns.Add("Oil Type");

            var processeddata = inputDto.FromDate;
            var totalDates = new List<DateTime>();

            while (inputDto.ToDate >= processeddata)
            {

                totalDates.Add(processeddata);
                dt.Columns.Add(processeddata.ToShortDateString());
                processeddata = processeddata.AddDays(1);
            }
            foreach (var rec in data.GroupBy(s => s.OilType))
            {
                var row = dt.NewRow();
                row["Oil Type"] = rec.Key.Name;
                foreach (var date in totalDates)
                {

                    var obj = data.FirstOrDefault(s => (s.ValidFrom.Date <= date && s.ValidTo.Date >= date));
                    var colName = date.ToShortDateString();
                    if (obj != null)
                    {
                        row[colName] = obj.RatePerMt;
                    }
                    else
                    {
                        row[colName] = "N/A";
                    }

                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        private DataTable CalculatePackingCostRate(ReportInputDto inputDto)
        {
            var data = _emamiContext.PackingCosts.Where(s => s.OilTypeId == inputDto.OilTypeId && (DbFunctions.TruncateTime(s.ValidFrom) >= inputDto.FromDate || DbFunctions.TruncateTime(s.ValidTo) <= inputDto.ToDate)).ToList();
            var dt = new DataTable();
            dt.Columns.Add("Sku Name");
            //dt.Columns.Add("Oil Type");
            var processeddata = inputDto.FromDate;
            var totalDates = new List<DateTime>();

            while (inputDto.ToDate >= processeddata)
            {
                totalDates.Add(processeddata);
                dt.Columns.Add(processeddata.ToShortDateString());
                processeddata = processeddata.AddDays(1);
            }
            foreach (var rec in data.GroupBy(s => new { s.Sku, s.OilType }))
            {
                var row = dt.NewRow();
                row["Sku Name"] = rec.Key.Sku.SkuName;
                //row["Oil Type"] = rec.Key.OilType.Name;
                foreach (var date in totalDates)
                {
                    var obj = data.FirstOrDefault(s => (s.ValidFrom.Date <= date && s.ValidTo.Date >= date));
                    var colName = date.ToShortDateString();
                    if (obj != null)
                    {
                        row[colName] = obj.ActualPackingCost;
                    }
                    else
                    {
                        row[colName] = "N/A";
                    }

                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        private DataTable CalculateHoneyCombCostRate(ReportInputDto inputDto)
        {
            var data = _emamiContext.HoneycombCosts.Where(s => s.OilTypeId == inputDto.OilTypeId && (DbFunctions.TruncateTime(s.ValidFrom) >= inputDto.FromDate || DbFunctions.TruncateTime(s.ValidTo) <= inputDto.ToDate)).ToList();
            var dt = new DataTable();
            dt.Columns.Add("Sku Name");
            //dt.Columns.Add("Oil Type");
            var processeddata = inputDto.FromDate;
            var totalDates = new List<DateTime>();

            while (inputDto.ToDate >= processeddata)
            {
                totalDates.Add(processeddata);
                dt.Columns.Add(processeddata.ToShortDateString());
                processeddata = processeddata.AddDays(1);
            }
            foreach (var rec in data.GroupBy(s => new { s.Sku, s.OilType }))
            {
                var row = dt.NewRow();
                row["Sku Name"] = rec.Key.Sku.SkuName;
                //row["Oil Type"] = rec.Key.OilType.Name;
                foreach (var date in totalDates)
                {
                    var obj = data.FirstOrDefault(s => (s.ValidFrom.Date <= date && s.ValidTo.Date >= date));
                    var colName = date.ToShortDateString();
                    if (obj != null)
                    {
                        row[colName] = obj.RatePerMt;
                    }
                    else
                    {
                        row[colName] = "N/A";
                    }

                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        private DataTable CalculateCushionMarginCostRate(ReportInputDto inputDto)
        {
            var data = _emamiContext.CushionMargins.Where(s => s.OilTypeId == inputDto.OilTypeId && (DbFunctions.TruncateTime(s.ValidFrom) >= inputDto.FromDate || DbFunctions.TruncateTime(s.ValidTo) <= inputDto.ToDate)).ToList();
            var dt = new DataTable();
            dt.Columns.Add("Sku Name");
            //dt.Columns.Add("Oil Type");
            var processeddata = inputDto.FromDate;
            var totalDates = new List<DateTime>();

            while (inputDto.ToDate >= processeddata)
            {
                totalDates.Add(processeddata);
                dt.Columns.Add(processeddata.ToShortDateString());
                processeddata = processeddata.AddDays(1);
            }
            foreach (var rec in data.GroupBy(s => new { s.Sku, s.OilType }))
            {
                var row = dt.NewRow();
                row["Sku Name"] = rec.Key.Sku.SkuName;
                //row["Oil Type"] = rec.Key.OilType.Name;
                foreach (var date in totalDates)
                {
                    var obj = data.FirstOrDefault(s => (s.ValidFrom.Date <= date && s.ValidTo.Date >= date));
                    var colName = date.ToShortDateString();
                    if (obj != null)
                    {
                        row[colName] = obj.RatePerMt;
                    }
                    else
                    {
                        row[colName] = "N/A";
                    }
                }
                dt.Rows.Add(row);
            }
            return dt;
        }


        #region Sauda Report
        public ResultDto GetSaudaOrderDetailsReport(SaudaOrderReportInputputDto inputDto)
        {
            _methodName = "GetSaudaOrderDetailsReport";
            var saudaList = new List<ActualSaudaOrderReportOutputDto>();
            try
            {
                var roleId = _emamiContext.UserRoles.Where(_ => _.UserId == inputDto.LoginUserId).FirstOrDefault().RoleId;

                var bdoIds = new List<long>();
                var ZHIds = new List<long>();
                var customerIds = new List<long>();

                if (roleId == (int)DTO.Enums.Role.NationalTrader)
                {
                    ZHIds = _emamiContext.Users.Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(s => s.Id).ToList();
                    bdoIds = _emamiContext.Users.Where(_ => ZHIds.Contains((long)_.ReportingToId)).Select(s => s.Id).ToList();
                }
                if (roleId == (int)DTO.Enums.Role.ZonalTrader)
                {
                    ZHIds.Add(inputDto.LoginUserId);
                    bdoIds = _emamiContext.Users.Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(s => s.Id).ToList();
                }
                if (roleId == (int)DTO.Enums.Role.StateTrader)
                {
                    bdoIds.Add(inputDto.LoginUserId);
                }
                if (bdoIds.IsAny())
                {
                    customerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(s => s.CustomerId).ToList();
                }
                var saudaOrderList = _emamiContext.Sauda.AsNoTracking()
                    .Join(_emamiContext.SaudaOrders.AsNoTracking(), s => s.Id, so => so.SaudaId, (s, so) =>
                    new
                    {
                        Sauda = new { UserId = s.UserId, BiddingDate = s.BiddingDate, SaudaBookingType = s.SaudaBookingType.Name, SaudaBookingTypeId = s.SaudaBookingTypeId },
                        SaudaOrders = new
                        {
                            sku=so.Sku,
                            PlantId=so.PlantId,
                            Id = so.Id,
                            SaudaId = so.SaudaId,
                            DiscountTypeId = so.DiscountTypeId,
                            DiscountAmount = so.DiscountAmount,
                            BidQuantityCase = so.BidQuantityCase,
                            SkuId = so.SkuId,
                            Incoterms2 = so.Incoterms2,
                            BrokerId = so.BrokerId,
                            SkuName = so.Sku.SkuName,
                            SkuCode = so.Sku.SkuCode,
                            BidPrice = so.BidPrice,
                            SaudaNumber = s.SaudaNumber,
                            ValidFromDate = so.ValidFromDate,
                            ValidToDate = so.ValidToDate,
                            BidQuantity = so.BidQuantity,
                            PackTypeName = so.Sku.PackType.Name,
                            PackGroupName = so.Sku.PackGroup.Name,
                            VerticalName = so.OilType.Division.Name,
                            SalesOrganization=so.OilType.SalesOrganization.Name,
                            DistributionChannel=so.OilType.DistributionChannel.Name,
                            PricingId = so.PricingId,
                            StatusId = so.StatusId,
                            SkuQuantity = so.Sku.Quantity,
                            SkuUom = so.Sku.Uom.Name,
                            SpecialRateId = so.SpecialRateRequestId,
                            QuotedPrice = so.QuotedPrice,
                            VerticalId = so.OilType.DivisionId,
                            SalesOrganizationId=so.SalesOrganizationId,
                            DistributionChannelId=so.DistributionChannelId,
                            Remarks = so.Remarks,
                            OilType = so.OilType.Name+"-"+so.OilType.SalesOrganization.Code+"/"+so.OilType.DistributionChannel.Code+"/"+so.OilType.Division.Code,
                           
                            BaseRate = so.BaseRate,
                            
                            so.BidPriceBeforeDiscount,
                            so.IsBaseSauda,
                            so.BaseSkuBidPrice
                        }
                    })
                    //.Join(_emamiContext.Pricing.AsNoTracking(), so => so.SaudaOrders.PricingId, p => p.Id, (so, p) =>
                    //new
                    //{
                    //    so.SaudaOrders,
                    //    so.Sauda,
                    //    Pricing = new
                    //    {
                    //        PlantId = p.PlantId,
                            
                    //        sku = p.Sku
                    //    },
                    //})
                    .Join(_emamiContext.Users.AsNoTracking(), s => s.Sauda.UserId, u => u.Id, (s, u) =>
                    new
                    { s.Sauda, s.SaudaOrders, Pricing=new { PlantId=s.SaudaOrders.PlantId,sku=s.SaudaOrders.sku}, User = new { StateId = u.StateId, Code = u.Code, Name = u.Name,u.CustomerGroupFiveId , Id = u.Id,} })
                    
                    .Where(w => DbFunctions.TruncateTime(w.Sauda.BiddingDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    && DbFunctions.TruncateTime(w.Sauda.BiddingDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && inputDto.StateIds.Contains(w.User.StateId)
                    && (inputDto.VerticalId > 0 ? w.SaudaOrders.VerticalId == inputDto.VerticalId && w.SaudaOrders.SalesOrganizationId==inputDto.SalesOrganizationId && w.SaudaOrders.DistributionChannelId==inputDto.DistributionChannelId : w.SaudaOrders.VerticalId > 0))
                    .Select(s => s).ToList();

                if (inputDto.StatusIds != null && inputDto.StatusIds.Count > 0)
                {
                    if (inputDto.StatusIds.Contains(-1))
                    {
                        saudaOrderList = saudaOrderList.ToList();
                    }
                    else
                    {
                        saudaOrderList = saudaOrderList.Where(_ => inputDto.StatusIds.Contains(_.SaudaOrders.StatusId)).ToList();
                    }
                }

                saudaOrderList.RemoveAll(item => item.Pricing.PlantId == 0);

                

                if (saudaOrderList != null && saudaOrderList.Any())
                {
                    #region Common Data's
                    var specialRateId = saudaOrderList.Select(s => s.SaudaOrders.SpecialRateId).Distinct().ToList();
                    var SpecialRateDatas = _emamiContext.SpecialRate.AsNoTracking().Where(_ => specialRateId.Contains(_.Id))
                        .Select(s => new
                        {
                            Id = s.Id,
                            IsLTD = s.IsLTD
                        }).ToList();

                    //var tradeTicketNos = saudaOrderList.Select(s => s.SaudaOrders.TradeTicketNo).Distinct().ToList();
                    //var TradeTicketDatas = _emamiContext.TradeTicket.AsNoTracking().Where(_ => tradeTicketNos.Contains(_.TradeTicketNumber))
                    //    .Select(s => new
                    //    {
                    //        TotalCost = s.TotalCost,
                    //        TradeTicketNumber = s.TradeTicketNumber
                    //    }).ToList();

                    var skuIds = saudaOrderList.Select(s => s.SaudaOrders.SkuId).Distinct().ToList();
                    var SkuUomMappingDatas = _emamiContext.SkuUomMapping
                        .Where(_ => skuIds.Contains(_.SkuId) && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)(int)DTO.Enums.Uom.Nos)
                        .Select(s => new
                        {
                            SkuId = s.SkuId,
                            UomId = s.UomId,
                            RelationUomId = s.RelationUomId,
                            //  ConversionFactor = s.ConversionFactor
                        }).ToList();

                    var SkuDatas = _emamiContext.Skus.AsNoTracking().Where(w => skuIds.Contains(w.Id))
                        .Select(s => new
                        {
                            Id = s.Id,
                            UomId = s.UomId,
                          //  LitreConversion = s.OilType.LitreConversion,
                            Quantity = s.Quantity
                        });

                    var brokerIds = saudaOrderList.Select(s => s.SaudaOrders.BrokerId).Distinct().ToList();
                    var UserDatas = _emamiContext.Users.AsNoTracking().Where(w => brokerIds.Contains(w.Id))
                        .Select(s => new
                        {
                            Id = s.Id,
                            Name = s.Name,
                            Code = s.Code
                        }).ToList();


                    var saudaUserIds = saudaOrderList.Select(s => s.Sauda.UserId).Distinct().ToList();
                    var BdoDatas = _emamiContext.Users.AsNoTracking()
                        .Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { User = u, UserRoles = ur })
                        .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), us => us.User.Id, ucm => ucm.CustomerId, (us, ucm) => new { us.User, us.UserRoles, UserCustomer = ucm })
                        .Where(w => w.UserRoles.RoleId == (long)DTO.Enums.Role.StateTrader && saudaUserIds.Contains(w.UserCustomer.CustomerId))
                        .Select(s => new
                        {
                            Id = s.User.Id,
                            Name = s.User.Name,
                            Code = s.User.Code
                        }).ToList();

                    //var customerGroupOneIds = saudaOrderList.Select(s => s.User.CustomerGroupOneId).ToList();
                    var customerGroupFiveIds = saudaOrderList.Select(s => s.User.CustomerGroupFiveId).ToList();

                    var CustomerGroupFiveDatas = _emamiContext.CustomerGroupFive.AsNoTracking().Where(w => customerGroupFiveIds.Contains(w.Id))
                        .Select(s => new
                        {
                            Id = s.Id,
                            Name = s.GroupName
                        }).ToList();

                    //var CustomerGroupTwoDatas = _emamiContext.CustomerGroupTwo.AsNoTracking().Where(w => customerGroupTwoIds.Contains(w.Id))
                    //    .Select(s => new
                    //    {
                    //        Id = s.Id,
                    //        Name = s.GroupName
                    //    }).ToList();

                    #endregion

                    var depotContext = _emamiContext.Depots.AsNoTracking();

                    if (bdoIds.IsAny())
                    {
                        if (customerIds.IsAny())
                        {
                            saudaOrderList = saudaOrderList.Where(_ => customerIds.Contains(_.Sauda.UserId) || bdoIds.Contains(_.Sauda.UserId)).ToList();
                        }
                        else
                        {
                            saudaOrderList = saudaOrderList.Where(_ => bdoIds.Contains(_.Sauda.UserId)).ToList();
                        }
                    }

                    foreach (var s in saudaOrderList)
                    {
                        if (s.SaudaOrders.BidQuantityCase <= 0)
                        {
                            continue;
                        }
                        decimal raPremiumWithtax = 0;
                        decimal raPremiumWithoutTax = 0;
                        decimal allocationPremiumWithtax = 0;
                        decimal allocationPremiumWithoutTax = 0;
                        decimal raTotalDiscount = 0;
                        decimal saleRate = 0;
                        //decimal honeycombCost = s.Pricing.HoneycombCost;
                        decimal discount = 0, premium = 0, LtdValue = 0, specialRate = 0, specialRateDiscount = 0;
                        bool isLtd = false;
                        if (s.SaudaOrders.SpecialRateId > 0)
                        {
                            //isLtd = _emamiContext.SpecialRate.AsNoTracking().FirstOrDefault(_ => _.Id == s.SaudaOrders.SpecialRateId).IsLTD;
                            if (SpecialRateDatas != null && SpecialRateDatas.Any())
                                isLtd = SpecialRateDatas.FirstOrDefault(_ => _.Id == s.SaudaOrders.SpecialRateId).IsLTD;

                            var result = s.SaudaOrders.BidQuantityCase > 0 ? (s.SaudaOrders.QuotedPrice - s.SaudaOrders.BidPrice) / s.SaudaOrders.BidQuantityCase : 0;
                            if (result >= 0)
                            {
                                specialRateDiscount = result;
                            }
                            else
                            {
                                premium = -(result);
                            }
                            if (isLtd)
                            {
                                LtdValue = specialRateDiscount;
                            }
                            else
                            {
                                specialRate = specialRateDiscount;
                            }
                        }
                        else
                        {
                            discount = s.SaudaOrders.DiscountTypeId == 1 ? CalculateOneCase(s.SaudaOrders.DiscountAmount, s.SaudaOrders.BidQuantityCase) : 0;
                            premium = s.SaudaOrders.DiscountTypeId == 2 ? CalculateOneCase(s.SaudaOrders.DiscountAmount, s.SaudaOrders.BidQuantityCase) : 0;
                        }

                        //if (s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ExPlant || s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ExDepot)
                        //honeycombCost = 0;

                        //SaleRate
                        /*var PR00 = s.SaudaOrders.Proo*/ //> 0
                                                      //? s.SaudaOrders.Proo
                                                      //: ((s.Pricing.MaterialCost
                                                      //+ s.Pricing.PackingCost
                                                      //+ honeycombCost
                                                      //+ (s.Sauda.SaudaBookingTypeId == (long)DTO.Enums.SaudaBookingTypes.TraditionalProcess ? (s.Pricing.Margin + s.Pricing.CushionMargin) : s.Pricing.RaMargin)
                                                      //+ s.Pricing.SchemeCostRecovery
                           //+ premium
                           ////+ s.Pricing.AdditionalCost) 
                           //- (discount + LtdValue + specialRate);


                      //  var FRC1 = s.SaudaOrders.Frc1 > 0 ? s.SaudaOrders.Frc1 : Utility.CalculateFRC1(0, 0, 0, 0, s.SaudaOrders.Incoterms2, 0, 0);

                        decimal sRate = 0;
                        decimal taxPaidValue = 0;
                        decimal saleRateWithTax = 0;
                        decimal discountGstPercentage = 0;
                        decimal discountWithTax = 0;
                        decimal discountTaxAmount = 0;

                        #region oldCode
                        //if (s.Sauda.SaudaBookingTypeId == (long)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                        //{
                        //    //RA2.0 Changes
                        //    //raPremiumWithtax = (s.SaudaOrders.BidPrice / s.SaudaOrders.BidQuantityCase) - s.SaudaOrders.BaseRate;
                        //    if (s.SaudaOrders.IsBaseSauda)
                        //    {
                        //        raPremiumWithtax = s.SaudaOrders.BidPriceBeforeDiscount - s.SaudaOrders.BaseRate;
                        //    }
                        //    else
                        //    {
                        //        raPremiumWithtax = s.SaudaOrders.BaseSkuBidPrice - s.SaudaOrders.BaseRate;
                        //    }

                        //    decimal bidPricePerCause = (s.SaudaOrders.BidPrice / s.SaudaOrders.BidQuantityCase);
                        //    raTotalDiscount = s.SaudaOrders.VolumeDiscountCase +
                        //        s.SaudaOrders.SchemeDiscountCase +
                        //        s.SaudaOrders.SkuDiscountCase +
                        //        (s.SaudaOrders.GPBenefitTypeId == (int)DTO.Enums.BenefitType.NONSAP ? s.SaudaOrders.GPBenefitDiscountInCase : 0) +
                        //        (s.SaudaOrders.SurpriseBenefitTypeId == (int)DTO.Enums.BenefitType.NONSAP ? s.SaudaOrders.SurpriseBenefitDiscountInCase : 0);
                        //    // decimal discountWithTax = Utility.IncludeGst(1, s.Pricing.PlantGSTPercentage, raTotalDiscount);

                        //    switch (s.SaudaOrders.Incoterms2)
                        //    {
                        //        case (long)DTO.Enums.IncoTerms.ExPlant:
                        //            discountGstPercentage = Utility.GetGstAmount(1, s.Pricing.PlantGSTPercentage);
                        //            discountWithTax = Utility.DecimalFormatTwo(raTotalDiscount) * discountGstPercentage;
                        //            discountTaxAmount = discountWithTax - Utility.DecimalFormatTwo(raTotalDiscount);
                        //            taxPaidValue = Utility.DecimalFormatTwo(bidPricePerCause) /*- discountTaxAmount*/;
                        //            saleRateWithTax = bidPricePerCause; //- discountTaxAmount;
                        //            saleRate = Utility.ExcludeGst(1, s.Pricing.PlantGSTPercentage, saleRateWithTax);
                        //            if (s.SaudaOrders.IsBaseSauda)
                        //            {
                        //                raPremiumWithoutTax = Utility.DecimalFormatTwo(Utility.ExcludeGst(1, s.Pricing.PlantGSTPercentage, raPremiumWithtax));
                        //            }
                        //            else
                        //            {
                        //                raPremiumWithoutTax = Utility.DecimalFormatTwo(Utility.ExcludeGst(1, s.Pricing.PlantGSTPercentage, raPremiumWithtax));
                        //            }
                        //            break;
                        //        case (long)DTO.Enums.IncoTerms.ForPlant:
                        //            //saleRate = Utility.ExcludeGst(1, s.Pricing.PlantGSTPercentage, bidPricePerCause);
                        //            //taxPaidValue = Utility.DecimalFormatTwo(saleRate * Utility.GetGstAmount(1, s.Pricing.PlantGSTPercentage));
                        //            discountGstPercentage = Utility.GetGstAmount(1, s.Pricing.PlantGSTPercentage);
                        //            discountWithTax = raTotalDiscount * discountGstPercentage;
                        //            discountTaxAmount = discountWithTax - raTotalDiscount;
                        //            taxPaidValue = Utility.DecimalFormatTwo(bidPricePerCause)/* - discountTaxAmount*/;
                        //            saleRateWithTax = bidPricePerCause; //- discountTaxAmount;
                        //            saleRate = Utility.ExcludeGst(1, s.Pricing.PlantGSTPercentage, saleRateWithTax);
                        //            if (s.SaudaOrders.IsBaseSauda)
                        //            {
                        //                raPremiumWithoutTax = Utility.ExcludeGst(1, s.Pricing.PlantGSTPercentage, raPremiumWithtax);
                        //            }
                        //            else
                        //            {
                        //                raPremiumWithoutTax = Utility.ExcludeGst(1, s.Pricing.PlantGSTPercentage, raPremiumWithtax);
                        //            }
                        //            break;
                        //        case (long)DTO.Enums.IncoTerms.ExDepot:
                        //            //saleRate = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, bidPricePerCause);
                        //            //taxPaidValue = Utility.DecimalFormatTwo(saleRate * Utility.GetGstAmount(1, s.Pricing.DepotGSTPercentage));
                        //            discountGstPercentage = Utility.GetGstAmount(1, s.Pricing.DepotGSTPercentage);
                        //            discountWithTax = raTotalDiscount * discountGstPercentage;
                        //            discountTaxAmount = discountWithTax - raTotalDiscount;
                        //            taxPaidValue = Utility.DecimalFormatTwo(bidPricePerCause) /*- discountTaxAmount*/;
                        //            saleRateWithTax = bidPricePerCause;// - discountTaxAmount;
                        //            saleRate = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, saleRateWithTax);
                        //            if (s.SaudaOrders.IsBaseSauda)
                        //            {
                        //                raPremiumWithoutTax = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, raPremiumWithtax);
                        //            }
                        //            else
                        //            {
                        //                raPremiumWithoutTax = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, raPremiumWithtax);
                        //            }
                        //            break;
                        //        case (long)DTO.Enums.IncoTerms.ForDepot:
                        //            //saleRate = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, bidPricePerCause);
                        //            //taxPaidValue = Utility.DecimalFormatTwo(saleRate * Utility.GetGstAmount(1, s.Pricing.DepotGSTPercentage));
                        //            discountGstPercentage = Utility.GetGstAmount(1, s.Pricing.DepotGSTPercentage);
                        //            discountWithTax = raTotalDiscount * discountGstPercentage;
                        //            discountTaxAmount = discountWithTax - raTotalDiscount;
                        //            taxPaidValue = Utility.DecimalFormatTwo(bidPricePerCause) /*- discountTaxAmount*/;
                        //            saleRateWithTax = bidPricePerCause;// - discountTaxAmount;
                        //            saleRate = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, saleRateWithTax);

                        //            if (s.SaudaOrders.IsBaseSauda)
                        //            {
                        //                raPremiumWithoutTax = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, raPremiumWithtax);
                        //            }
                        //            else
                        //            {
                        //                raPremiumWithoutTax = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, raPremiumWithtax);
                        //            }
                        //            break;
                        //        default:
                        //            saleRate = 0;
                        //            break;
                        //    }
                        //    saleRate = Utility.DecimalFormatTwo(saleRate);
                        //    sRate = saleRate;
                        //    PR00 = (PR00 + s.Pricing.CustomerGroupMargin) - Utility.DecimalFormatTwo(raTotalDiscount / discountGstPercentage);
                        //    PR00 = Convert.ToDecimal(string.Format("{0:0.00}", PR00 + raPremiumWithoutTax));// Convert.ToDecimal(string.Format("{0:0.00}", PR00)) + Convert.ToDecimal(string.Format("{0:0.00}", raPremiumWithoutTax)); // (PR00 + raPremiumWithoutTax);

                        //    if (!s.SaudaOrders.IsBaseSauda)
                        //    {
                        //        decimal gstPercentage = 0;

                        //        if (s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExPlant || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForPlant)
                        //        {
                        //            gstPercentage = s.Pricing.PlantGSTPercentage;
                        //        }
                        //        else if (s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExDepot || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForDepot)
                        //        {
                        //            gstPercentage = s.Pricing.DepotGSTPercentage;
                        //        }

                        //        allocationPremiumWithtax = s.SaudaOrders.BaseSkuBidPrice - s.SaudaOrders.BidPriceBeforeDiscount; //Utility.DecimalFormatTwo(saleRate * gstPercentage);
                        //        allocationPremiumWithoutTax = Utility.DecimalFormatTwo(Utility.ExcludeGst(1, gstPercentage, allocationPremiumWithtax));
                        //        PR00 = Convert.ToDecimal(string.Format("{0:0.00}", PR00 - allocationPremiumWithoutTax));
                        //    }
                        //}
                        //else
                        //{

                        //}

                        //RealizationPerCase
                        //var realizationPerCase = s.SaudaOrders.Proo > 0 ? s.SaudaOrders.Proo : CalculateRealizationPerCase(s.Pricing.MaterialCost, s.Pricing.Margin, s.Pricing.CushionMargin, s.Pricing.RaMargin, premium, discount, s.Sauda.SaudaBookingTypeId, raPremiumWithoutTax);



                        //RealizationPerMT
                        //var realizationPerMT = CalculateReliazationCaseToMatericTon(s.SaudaOrders.SkuId, realizationPerCase);           //(realizationPerCase * 1000);



                        //RealizationPerCase
                        //var realizationPerCase = CalculateRealizationPerCase(PR00, s.Pricing.MaterialCost, s.Pricing.Margin, s.Pricing.CushionMargin, s.Pricing.RaMargin, premium, discount, s.Sauda.SaudaBookingTypeId, s.Pricing.PackingCost, honeycombCost, s.Pricing.SchemeCostRecovery);
                        //RealizationPerMT
                        //var realizationPerMT = CalculateReliazationCaseToMatericTon(s.SaudaOrders.SkuId, realizationPerCase);           //(realizationPerCase * 1000);

                        //var skuContext = SkuDatas.FirstOrDefault(_ => _.Id == s.SaudaOrders.SkuId);
                        //if (skuContext != null)
                        //{
                        //    decimal numberOfPcs = 0;
                        //    var skuUomContext = SkuUomMappingDatas.FirstOrDefault(_ => _.SkuId == s.SaudaOrders.SkuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                        //    if (skuUomContext != null)
                        //    {
                        //        numberOfPcs = skuUomContext.ConversionFactor;
                        //    }

                        //    var quantityTypeId = skuContext.UomId;
                        //    var ltrConversion = skuContext.LitreConversion;
                        //    if (quantityTypeId == (int)DTO.Enums.Uom.Ltr)
                        //    {
                        //        realizationPerMT = realizationPerCase * (ltrConversion / skuContext.Quantity);
                        //    }
                        //    else
                        //    {
                        //        realizationPerMT = realizationPerCase * (1000 / skuContext.Quantity);
                        //    }
                        //}
                        #endregion
                        
                        
                        //saleRate = PR00 + FRC1;
                        sRate = (s.SaudaOrders.QuotedPrice > 0 && s.SaudaOrders.BidQuantityCase > 0) ? s.SaudaOrders.QuotedPrice / s.SaudaOrders.BidQuantityCase : 0;
                        //taxPaidValue = Utility.DecimalFormatTwo(saleRate * Convert.ToDecimal(Constants.TaxPaidValue));
                        //var realizationPerCase = CalculateRealizationPerCase(PR00, 0, 0, 0, 0, premium, discount, s.Sauda.SaudaBookingTypeId, 0, 0, 0, raPremiumWithoutTax, 0);
                        decimal realizationPerMT = 0;

                        var totalValue = s.SaudaOrders.QuotedPrice;


                        //var broker = BrokerNameCode(s.SaudaOrders.BrokerId);
                        var broker = UserDatas.FirstOrDefault(f => f.Id == s.SaudaOrders.BrokerId);

                      //  decimal brokerage = 0, realizationPerCasePostBrokerage = 0, realizationPerMTPostBrokerage = 0, finalRealization = 0, purchaseCost = 0;
                        //if (s.SaudaOrders.TradeTicketNo != null)
                        //{
                        //    //var purchaseCostContext = _emamiContext.TradeTicket.AsNoTracking().FirstOrDefault(_ => _.TradeTicketNumber == s.SaudaOrders.TradeTicketNo)?.TotalCost;
                        //    var purchaseCostContext = TradeTicketDatas.FirstOrDefault(_ => _.TradeTicketNumber == s.SaudaOrders.TradeTicketNo)?.TotalCost;
                        //    if (purchaseCostContext != null)
                        //    {
                        //        purchaseCost = (decimal)purchaseCostContext;
                        //    }
                        //}

                        //if (broker != null)
                        //{
                        //    brokerage = 2;
                        //}
                        //realizationPerCasePostBrokerage = realizationPerCase - brokerage;
                        decimal SKUWiseWeight = 0;
                        if (s.SaudaOrders.SkuUom == DTO.Enums.Uom.Ltr.ToString())
                        {
                            //var SkuUomMappingContext = _emamiContext.SkuUomMapping.FirstOrDefault(_ => _.SkuId == s.SaudaOrders.SkuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)(int)DTO.Enums.Uom.Nos);
                            var SkuUomMappingContext = SkuUomMappingDatas.FirstOrDefault(_ => _.SkuId == s.SaudaOrders.SkuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)(int)DTO.Enums.Uom.Nos);
                            if (SkuUomMappingContext != null)
                            {
                                // SKUWiseWeight = s.SaudaOrders.LitreConversion > 0 ? (s.SaudaOrders.SkuQuantity * 1000 * SkuUomMappingContext.ConversionFactor) / s.SaudaOrders.LitreConversion : 0;
                            }
                            else
                            {
                                SKUWiseWeight = /*s.SaudaOrders.LitreConversion > 0 ? (s.SaudaOrders.SkuQuantity * 1000) / s.SaudaOrders.LitreConversion : */0;
                            }
                        }
                        else
                        {
                            SKUWiseWeight = s.SaudaOrders.SkuQuantity;
                        }

                        //if (realizationPerCase > 0 && SKUWiseWeight > 0)
                        //{
                        //    realizationPerMT = realizationPerCase / SKUWiseWeight * 1000;
                        //}
                        //realizationPerMTPostBrokerage = realizationPerCasePostBrokerage != 0 && SKUWiseWeight > 0 ? (realizationPerCasePostBrokerage / SKUWiseWeight) * 1000 : 0;
                        //finalRealization = realizationPerMTPostBrokerage;// - honeycombCost;
                        var employeeData = GetBdoname(s.Sauda.UserId);
                        saudaList.Add(new ActualSaudaOrderReportOutputDto()
                        {
                            
                            CustomerCode = s.User.Code,
                            CustomerName = s.User.Name,
                            //FreightRoute = s.User.FreightRouteName,
                            BrokerName = broker != null ? broker.Name : "",
                            BrokerCode = broker != null ? broker.Code : "",
                            SkuName = s.SaudaOrders.SkuName,
                            SkuCode = s.SaudaOrders.SkuCode,
                            BidQuantityCase = s.SaudaOrders.BidQuantityCase,
                            //PR00 = PR00,
                            //FRC1 = FRC1,
                            SaleRate = sRate,
                            //BidPrice = s.SaudaOrders.BidPrice,
                            Incoterms = Utility.GetEnumFromString<DTO.Enums.IncoTerms>(s.SaudaOrders.Incoterms2), // IncotermsName(s.SaudaOrders.Incoterms2),
                            AppBookingNo = s.SaudaOrders.SaudaId.ToString(),
                            BiddingDate = s.Sauda.BiddingDate.ToString("dd/MM/yyyy hh:mm:ss"),
                            ValidFromDate = s.SaudaOrders.ValidFromDate.ToString("dd/MM/yyyy hh:mm:ss"),
                            ValidToDate = s.SaudaOrders.ValidToDate.ToString("dd/MM/yyyy hh:mm:ss"),
                            BidQuantity = Utility.DecimalFormatThree(s.SaudaOrders.BidQuantity),
                            PackGroup = s.SaudaOrders.PackGroupName,
                            //DepotCode = (s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ForDepot || s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ExDepot) ? (s.Pricing.DepotId == 0 ? "" : depotContext.FirstOrDefault(depot => depot.Id == s.Pricing.DepotId).Code) : (s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ForRake || s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ExRake) ? (s.Pricing.DepotId == 0 ? "" : depotContext.FirstOrDefault(depot => depot.Id == s.Pricing.DepotId).Code) : "",
                            //DepotName = (s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ForDepot || s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ExDepot) ? (s.Pricing.DepotId == 0 ? "" : depotContext.FirstOrDefault(depot => depot.Id == s.Pricing.DepotId).Name) : (s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ForRake || s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ExRake) ? (s.Pricing.DepotId == 0 ? "" : depotContext.FirstOrDefault(depot => depot.Id == s.Pricing.DepotId).Name) : "",
                            State = s.User.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(state => state.Id == s.User.StateId).StateName : "",
                            PlantName = s.Pricing.PlantId > 0 ? depotContext.FirstOrDefault(plant => plant.Id == s.Pricing.PlantId).Name : "",
                            //PlantCode = s.Pricing.PlantId > 0 ? depotContext.FirstOrDefault(plant => plant.Id == s.Pricing.PlantId).Code : "",
                            //RealizationPerMt = realizationPerMT,
                            UOM = s.SaudaOrders.SkuUom,
                            //PackSize = s.SaudaOrders.SkuQuantity + " " + s.SaudaOrders.SkuUom,
                            //MaterialCost = s.Pricing.MaterialCost,
                            //PrimaryFreight = (s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExDepot || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForDepot || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForRake) ?
                            //s.Pricing.PrimaryFrieght : 0,
                            //PackingCost = s.Pricing.PackingCost,
                            //HoneycombCost = honeycombCost,
                            //BrokerageCost = 0,
                            //DetentionCharges =
                            //(s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExDepot || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForDepot || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForRake) ? s.Pricing.DetentionCost :
                            //0,
                            //DepotCost = (s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExDepot || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForDepot || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForRake) ? s.Pricing.DepotCost : 0,
                            //MarginCostTP = (s.Pricing.Margin + s.Pricing.CushionMargin),
                            //MarginCostRA = s.Pricing.RaMargin,
                            //SecondaryFreight = s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForDepot ? s.Pricing.SecondaryFrieght :
                            //(s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForPlant ? s.Pricing.PlantSecondaryFrieght : 0),
                            TD = Utility.DecimalFormatTwo(discount),
                            //LTD = 0,
                            TotalValue = totalValue,
                            EmployeeName = employeeData != null ? employeeData.Name : "", // GetBdoname(s.Sauda.UserId).Name,
                            EmployeeCode = employeeData != null ? employeeData.Code : "", // GetBdoname(s.Sauda.UserId).Code,
                            Vertical = s.SaudaOrders.VerticalName,
                            SalesOrganization=s.SaudaOrders.SalesOrganization,
                            DistributionChannel=s.SaudaOrders.DistributionChannel,
                            Premium = Utility.DecimalFormatTwo(premium),
                            //SaudaBookingType = s.Sauda.SaudaBookingType,
                           // RealizationPerCase = realizationPerCase,
                            //ActualPackingCost = s.Pricing.PackingCost,
                            Status = Enum.GetName(typeof(DTO.Enums.Status), s.SaudaOrders.StatusId),
                            LTDValue = LtdValue,
                            SpecialRate = specialRate,
                            Remarks = s.SaudaOrders.Remarks,
                            //CushionMargin = s.Pricing.CushionMargin,
                            BiddingTime = s.Sauda.BiddingDate.TimeOfDay.ToString(),
                            OilType = s.SaudaOrders.OilType,
                            //TaxPaid = taxPaidValue, // Utility.DecimalFormatTwo(saleRate * Convert.ToDecimal(Constants.TaxPaidValue)),
                           // Brokerage = brokerage,
                            //Area = s.Pricing.PlantId == 0 ? ""  :"",
                            //RealizationPerCasePostBrokerage = realizationPerCasePostBrokerage,
                            //SkuWiseWeight = SKUWiseWeight,
                            //RealizationPerMTPostBrokerage = realizationPerMTPostBrokerage,
                            //FinalRealization = finalRealization,
                            //RealizationTotal = finalRealization * s.SaudaOrders.BidQuantity,
                            //Purchase = purchaseCost,
                            //PurchaseTotal = purchaseCost * s.SaudaOrders.BidQuantity,
                            //MarginPMTLineItem = finalRealization - purchaseCost,
                            //SchemeCost = s.Pricing.SchemeCostRecovery,
                            //MaterialType = s.SaudaOrders.MaterialType,
                            //CustomerGroupMargin = s.Pricing.CustomerGroupMargin,
                            //RaTotalDiscount = raTotalDiscount,
                            //SaudaBookingTypeId = s.Sauda.SaudaBookingTypeId,
                            //RAPremiumWithTax = raPremiumWithtax,
                            //RAPremiumWithoutTax = raPremiumWithoutTax,
                            //AdditionalCost = s.Pricing.AdditionalCost,
                            //OilTransferCost = s.Pricing.OilTransferCostForPlant > 0 ? s.Pricing.OilTransferCostForPlant : s.Pricing.OilTransferCostForDepot,
                            //IsBaseSauda = s.SaudaOrders.IsBaseSauda,
                            //SkuAllocationPremiumWithTax = allocationPremiumWithtax,
                            //SkuAllocationPremiumWithoutTax = allocationPremiumWithoutTax,
                            //CustomerGroupOne = CustomerGroupOneDatas.FirstOrDefault(f => f.Id == s.User.CustomerGroupOneId)?.Name,
                            CustomerGroupFive = CustomerGroupFiveDatas.FirstOrDefault(f => f.Id == s.User.CustomerGroupFiveId)?.Name,
                            SaudaOrderId = s.SaudaOrders.Id,
                            SaudaNumber = s.SaudaOrders.SaudaNumber != null ? s.SaudaOrders.SaudaNumber : string.Empty
                        });

                    }
                    
                }

                return _resultService.SuccessObject(saudaList);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetBDOWiseSaudaReport(SaudaOrderReportInputputDto inputDto)
        {
            var saudaReportOutputDto = new List<SaudaBDOWiseReportDto>();
            _methodName = "GetBDOWiseSaudaReport";
            try
            {
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
                if (inputDto.BDOIds == null || !inputDto.BDOIds.Any())
                {
                    return _resultService.ErrorMessage(Constants.SalesPersonMissing);
                }
                List<long> UserIds = new List<long>();
                UserIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => inputDto.BDOIds.Contains(_.UserId)).Select(_ => _.CustomerId).Distinct().ToList();
                //StateTrader Filter
                var saudaOrdersContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                    && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && UserIds.Contains(_.Sauda.UserId));
                //Sauda booking type filter
                if (inputDto.SaudaBookingTypeId != 0)
                {
                    saudaOrdersContext = saudaOrdersContext.AsNoTracking().Where(_ => _.SaudaBookingTypeId == inputDto.SaudaBookingTypeId);
                }
                //BP-CP filter
                if (inputDto.PackTypeId != 0)
                {
                    saudaOrdersContext = saudaOrdersContext.AsNoTracking().Join(_emamiContext.Skus.AsNoTracking().Where(_ => _.PackGroupId == inputDto.PackTypeId),
                        x => x.SkuId, s => s.Id, (x, s) => new { x }).Select(_ => _.x);
                }

                if (saudaOrdersContext != null && saudaOrdersContext.Any())
                {

                    saudaReportOutputDto = saudaOrdersContext.Join(_emamiContext.Users.AsNoTracking(), x => x.Sauda.UserId, u => u.Id, (x, u) => new { SaudaOrder = x, PartyName = u.Name, PartyCode = u.Code })
                        .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.SaudaOrder.Sauda.UserId, uc => uc.CustomerId, (x, uc) => new { x.SaudaOrder, x.PartyCode, x.PartyName, BDOId = uc.UserId })
                        .Join(_emamiContext.Users.AsNoTracking(), x => x.BDOId, u => u.Id, (x, u) => new { x.SaudaOrder, x.PartyCode, x.PartyName, BDOName = u.Name, BDOCode = u.Code })
                        .Join(_emamiContext.OilTypes.AsNoTracking(), x => x.SaudaOrder.OilTypeId, ot => ot.Id, (x, ot) => new { x.SaudaOrder, x.PartyCode, x.PartyName, x.BDOCode, x.BDOName, OilType = ot.Name })
                        .Join(_emamiContext.Skus.AsNoTracking(), x => x.SaudaOrder.SkuId, s => s.Id, (x, s) => new { x.SaudaOrder, x.PartyCode, x.PartyName, x.BDOCode, x.BDOName, x.OilType, PackGroupId = s.PackGroupId })
                        .GroupBy(_ => new { _.SaudaOrder.Sauda.UserId, _.SaudaOrder.OilTypeId }).Select(_ => new SaudaBDOWiseReportDto
                        {
                            BDOCode = _.FirstOrDefault().BDOCode,
                            BDOName = _.FirstOrDefault().BDOName,
                            DealerCode = _.FirstOrDefault().PartyCode,
                            DealerName = _.FirstOrDefault().PartyName,
                            OilTypeName = _.FirstOrDefault().OilType,
                            //BPInCase = _.Where(w => w.PackGroupId == (int)DTO.Enums.PackGroupType.Premium).Sum(s => s.SaudaOrder.BidQuantityCase),
                            //BPInMT = _.Where(w => w.PackGroupId == (int)DTO.Enums.PackGroupType.Premium).Sum(s => s.SaudaOrder.BidQuantity),
                            //CPInCase = _.Where(w => w.PackGroupId == (int)DTO.Enums.PackGroupType.Bakery).Sum(s => s.SaudaOrder.BidQuantityCase),
                            //CPInMT = _.Where(w => w.PackGroupId == (int)DTO.Enums.PackGroupType.Bakery).Sum(s => s.SaudaOrder.BidQuantity),
                            TotalSalesInCase = _.Sum(s => s.SaudaOrder.BidQuantityCase),
                            TotalSalesInMT = _.Sum(s => s.SaudaOrder.BidQuantity),
                        }).OrderBy(_ => _.BDOName).ThenBy(_ => _.DealerName).ThenBy(_ => _.OilTypeName).ToList();
                }

                return _resultService.SuccessObject(saudaReportOutputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public User GetBdoname(long userId)
        {
            var userdetails = new User();
            var userIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(w => w.CustomerId == userId).Select(s => s.UserId).ToList();
            if (userIds != null && userIds.Any())
            {
                var bdoId = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(w => w.RoleId == (long)DTO.Enums.Role.StateTrader && userIds.Contains(w.UserId))?.UserId;
                if (bdoId != null && bdoId > 0)
                    userdetails = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == bdoId);
            }
            return userdetails;
        }

        /// <summary>
        /// Calculate one case discount
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="bidQuantityCase"></param>
        /// <returns></returns>
        public decimal CalculateOneCase(decimal amount, decimal bidQuantityCase)
        {
            var result = bidQuantityCase > 0 ? (amount / bidQuantityCase) : 0;
            return result;
        }

        /// <summary>
        /// Convert case to metericton price
        /// </summary>
        /// <param name="skuId"></param>
        /// <param name="caseValue"></param>
        /// <returns></returns>
        public decimal CalculateReliazationCaseToMatericTon(long skuId, decimal caseValue)
        {
            decimal metricTone = 0;
            var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == skuId);
            if (skuContext != null)
            {
                decimal numberOfPcs = 0;
                var skuUomContext = _emamiContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                if (skuUomContext != null)
                {
                    // numberOfPcs = skuUomContext.ConversionFactor;
                }

                var quantityTypeId = skuContext.UomId;
                var ltrConversion = /*skuContext.OilType.LitreConversion*/ 0;
                if (quantityTypeId == (int)DTO.Enums.Uom.Ltr)
                {
                    metricTone = caseValue * (ltrConversion / skuContext.Quantity);
                }
                else
                {
                    metricTone = caseValue * (1000 / skuContext.Quantity);
                }
            }
            return metricTone;
        }

        /// <summary>
        /// Return broker name and code
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string BrokerNameCode(long userId)
        {
            return _emamiContext.Users.AsNoTracking().Where(w => w.Id == userId).Select(s => s.Name + "~" + s.Code).FirstOrDefault();
        }

        /// <summary>
        /// Returns incoterm name
        /// </summary>
        /// <param name="incotermId"></param>
        /// <returns></returns>
        public string IncotermsName(long incotermId)
        {
            string name = "";
            switch (incotermId)
            {
                case (long)DTO.Enums.IncoTerms.ForPlant:
                    name = DTO.Enums.IncoTerms.ForPlant.ToString();
                    break;
                case (long)DTO.Enums.IncoTerms.ForDepot:
                    name = DTO.Enums.IncoTerms.ForDepot.ToString();
                    break;
                case (long)DTO.Enums.IncoTerms.ExPlant:
                    name = DTO.Enums.IncoTerms.ExPlant.ToString();
                    break;
                case (long)DTO.Enums.IncoTerms.ExDepot:
                    name = DTO.Enums.IncoTerms.ExDepot.ToString();
                    break;
                case (long)DTO.Enums.IncoTerms.ForRake:
                    name = DTO.Enums.IncoTerms.ForRake.ToString();
                    break;
                case (long)DTO.Enums.IncoTerms.ExRake:
                    name = DTO.Enums.IncoTerms.ExRake.ToString();
                    break;
                default:
                    break;
            }
            return name;
        }

        //public decimal CalculateRealizationPerCase(decimal materialCost, decimal profitMargin, decimal cushionMargin, decimal raMargin, decimal premium, decimal discount, long bookingType, decimal raPremium)
        //{
        //    if (bookingType == (long)DTO.Enums.SaudaBookingTypes.TraditionalProcess)
        //    {
        //        //return PROO - packingcost - honeycombcost - schemecost;
        //        //return ((materialCost + profitMargin + cushionMargin + premium) - discount) - packingcost - honeycombcost - schemecost;
        //        return 0;
        //    }
        //    else if (bookingType == (long)DTO.Enums.SaudaBookingTypes.ReverseAuction)
        //    {
        //        return (materialCost + raMargin + raPremium);
        //    }
        //    else
        //    {
        //        return 0;
        //    }
        //}

        public decimal CalculateRealizationPerCase(decimal PROO, decimal materialCost, decimal profitMargin, decimal cushionMargin, decimal raMargin, decimal premium, decimal discount, long bookingType, decimal packingcost, decimal honeycombcost, decimal schemecost, decimal raPremiumWithoutTax, decimal oilTransferCost)
        {
            if (bookingType == (long)DTO.Enums.SaudaBookingTypes.TraditionalProcess)
            {
                return PROO - packingcost - honeycombcost - schemecost - oilTransferCost;
                //return ((materialCost + profitMargin + cushionMargin + premium) - discount) - packingcost - honeycombcost - schemecost;
            }
            //else if (bookingType == (long)DTO.Enums.SaudaBookingTypes.ReverseAuction)
            //{
            //    return (materialCost + raMargin + raPremiumWithoutTax);
            //}
            else
            {
                return 0;
            }
        }

        public decimal CalculateRealizationPerMT(decimal realizationPerCase)
        {
            return (realizationPerCase * 1000);
        }
        #endregion

        #region Sauda Limt
        public ResultDto GetCustomerSaudaLimitReport(ReportFilterDto inputDto)
        {
            _methodName = "GetCustomerSaudaLimitReport";
            var saudaLimitOutputDto = new List<SaudaLimitDto>();
            try
            {
                var dealerCode = new List<string>();

                inputDto.zhId = inputDto.zhId.IsAny() ? inputDto.zhId : _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur }).Where(_ => _.ur.RoleId == (int)DTO.Enums.Role.ZonalTrader).Select(_ => _.u.Id).ToList();
                inputDto.bdoId = inputDto.bdoId.IsAny() ? inputDto.bdoId : _emamiContext.Users.AsNoTracking().Where(_ => inputDto.zhId.Contains((long)_.ReportingToId)).Select(_ => _.Id).ToList();
                dealerCode = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserCustomerMapping.AsNoTracking(), u => u.Id, ucm => ucm.CustomerId, (u, ucm) => new { u, ucm }).Where(_ => inputDto.bdoId.Contains(_.ucm.UserId)).Select(a => a.u.Code).ToList();

                if (!string.IsNullOrEmpty(inputDto.dealerCode))
                {
                    if (dealerCode.Contains(inputDto.dealerCode))
                    {
                        dealerCode.Clear();
                        dealerCode.Add(inputDto.dealerCode);
                    }
                    else
                    {
                        return _resultService.ErrorMessage(Constants.InvalidDealer);
                    }

                }

                var dealerContext = _emamiContext.Users.AsNoTracking().Where(_ => inputDto.StateIds.Contains(_.StateId) && dealerCode.Contains(_.Code))
                    .Join(_emamiContext.UserDivisionMappings.AsNoTracking(), c => c.Id, ud => ud.UserId, (c, ud) => new { c, ud })
                    .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), u => u.c.Id,uc=> uc.CustomerId, (u, uc)=> new { u.c,u.ud,uc} )
                    .Join(_emamiContext.State.AsNoTracking(), y => y.c.StateId, state => state.Id, (y, state) => new { User = y.c, Division = y.ud, State = state,Employee=y.uc })
                    .Where(_ => inputDto.bdoId.Contains(_.Employee.UserId) && (_.Division.SalesOrganizationId == inputDto.SalesOrganizationId || _.Division.SalesOrganizationId > 0) && (_.Division.DistributionChannelId == inputDto.DistributionChannelId || _.Division.DistributionChannelId > 0) && (_.Division.DivisionId == inputDto.DivisionId || _.Division.DivisionId > 0))
                    .Select(_ => new { _.User.Id, _.User.Code, _.User.Name, TotalSaudaLimit = _.Division.SaudaLimit, _.State.StateName, _.Division.SalesOrganizationId, _.Division.DivisionId, _.Division.DistributionChannelId, DivisionName = _.Division.Division.Name , SalesOrganization = _.Division.SalesOrganization.Name , DistributionChannel = _.Division.DistributionChannel.Name ,EmployeeName=_.Employee.User.Name}).ToList() ;


                if (dealerContext != null && dealerContext.Any())
                {
                    var dealerIds = dealerContext.Select(x => x.Id).ToList();
                    var saudacontext = new List<SaudaLimitDto>();
                    var pendingcontractcontext = new List<SaudaLimitDto>();

                    saudacontext = (from s in _emamiContext.Sauda.AsNoTracking().Where(x => dealerIds.Contains(x.UserId) && (x.SalesOrganizationId == inputDto.SalesOrganizationId || x.SalesOrganizationId > 0) && (x.DistributionChannelId == inputDto.DistributionChannelId || x.DistributionChannelId > 0) && (x.DivisionId == inputDto.DivisionId || x.DivisionId > 0))
                                    join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
                                    where so.StatusId != (int)DTO.Enums.Status.Rejected && s.StatusId != (int)DTO.Enums.Status.Rejected
                                    group new { s, so } by new { s.UserId, so.SalesOrganizationId, so.DistributionChannelId, so.DivisionId } into gp
                                    select new SaudaLimitDto
                                    {
                                        DistributionChannelId = gp.FirstOrDefault().s.DistributionChannelId,
                                        SalesOrganizationId = gp.FirstOrDefault().s.SalesOrganizationId,
                                        DivisionId = gp.FirstOrDefault().s.DivisionId,
                                        CustomerId = gp.FirstOrDefault().s.UserId,
                                        SaudaOrderQty = gp.Sum(x => x.so.BidQuantity),
                                        SaudaOrderQtyCase = gp.Sum(x => x.so.BidQuantityCase)
                                    }).ToList();

                    pendingcontractcontext = (from pc in _emamiContext.PendingContracts.AsNoTracking().Where(x => dealerIds.Contains(x.UserId) && (x.SalesOrgId == inputDto.SalesOrganizationId || x.SalesOrgId > 0) && (x.DistChnlId == inputDto.DistributionChannelId || x.DistChnlId > 0) && (x.DivisionId == inputDto.DivisionId || x.DivisionId > 0))
                                              group new { pc } by new { pc.UserId, pc.DivisionId, pc.SalesOrgId, pc.DistChnlId } into gp
                                              select new SaudaLimitDto
                                              {
                                                  DistributionChannelId = gp.FirstOrDefault().pc.DistChnlId,
                                                  SalesOrganizationId = gp.FirstOrDefault().pc.SalesOrgId,
                                                  DivisionId = gp.FirstOrDefault().pc.DivisionId,
                                                  CustomerId = gp.FirstOrDefault().pc.UserId,
                                                  PendingContractQty = gp.Sum(x => x.pc.SaudaQuantity),
                                                  PendingContractQtyCase = gp.Sum(x => x.pc.PendingQuantityInCase)
                                              }).ToList();

                    SaudaLimitDto sauda = new SaudaLimitDto();
                    foreach (var dealer in dealerContext)
                    {

                        sauda = new SaudaLimitDto()
                        {
                            //CustomerId = sc.CustomerId,
                            Name = dealer.Name,
                            CustomerCode = dealer.Code,
                            Employee = dealer.EmployeeName,
                            State = dealer.StateName,
                            SaudaLimit = dealer.TotalSaudaLimit ?? 0,
                            //DivisionId = (saudacontext.Where(_ => _.CustomerId == dealer.Id).FirstOrDefault().DivisionId),
                            SalesOrganizationName = dealer.SalesOrganization,
                            DistributionChannelName = dealer.DistributionChannel,
                            DivisionName = dealer.DivisionName,
                            SaudaOrderQty = (saudacontext.Where(s => s.CustomerId == dealer.Id && s.SalesOrganizationId == dealer.SalesOrganizationId && s.DistributionChannelId == dealer.DistributionChannelId && s.DivisionId == dealer.DivisionId).Sum(_ => _.SaudaOrderQty)),
                            SaudaOrderQtyCase = (saudacontext.Where(s => s.CustomerId == dealer.Id && s.SalesOrganizationId == dealer.SalesOrganizationId && s.DistributionChannelId == dealer.DistributionChannelId && s.DivisionId == dealer.DivisionId).Sum(_ => _.SaudaOrderQtyCase)),
                            PendingContractQty = (pendingcontractcontext.Where(s => s.CustomerId == dealer.Id && s.SalesOrganizationId == dealer.SalesOrganizationId && s.DistributionChannelId == dealer.DistributionChannelId && s.DivisionId == dealer.DivisionId).Sum(_ => _.PendingContractQty)),
                            PendingContractQtyCase = (pendingcontractcontext.Where(s => s.CustomerId == dealer.Id && s.SalesOrganizationId == dealer.SalesOrganizationId && s.DistributionChannelId == dealer.DistributionChannelId && s.DivisionId == dealer.DivisionId).Sum(_ => _.PendingContractQtyCase)), 
                            //AvailableSaudaLimit = (decimal)(dealer.TotalSaudaLimit - sc.SaudaOrderQty - pc.PendingContractQty
                        };
                        sauda.AvailableSaudaLimit = (decimal)(dealer.TotalSaudaLimit - sauda.PendingContractQty);
                        saudaLimitOutputDto.Add(sauda);
                    }

                }
                
                return _resultService.SuccessObject(saudaLimitOutputDto);

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }
        #endregion

        #region Lifting/Indent Report

        public ResultDto IndentListReport(IndentReportInputDto inputDto)
        {
            _methodName = "IndentListReport";
            var resultDto = new ResultDto();
            var outputDto = new List<LiftingListReportDto>();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var liftingRequestListQueryContext = _emamiContext.LiftingRequest.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(_.CreatedDate) >= DbFunctions.TruncateTime(inputDto.StartDate)
                    && DbFunctions.TruncateTime(_.CreatedDate) <= DbFunctions.TruncateTime(inputDto.EndDate)).AsNoTracking().AsQueryable();
                if (liftingRequestListQueryContext == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
                    return resultDto;
                }

                if (inputDto.StateIds != null && inputDto.StateIds.Any())
                {
                    liftingRequestListQueryContext = liftingRequestListQueryContext
                        .Join(_emamiContext.Users.AsNoTracking().Where(_ => inputDto.StateIds.Contains(_.StateId)), lfq => lfq.UserId, u => u.Id, (lfq, u) => new { lfq }).Select(_ => _.lfq);
                }

                var statusIds = new List<long>();
                if (inputDto.StatusId > 0)
                {
                    statusIds.Add(inputDto.StatusId);
                }
                else
                {
                    statusIds = new List<long>() { (long)DTO.Enums.Status.Approved, (long)DTO.Enums.Status.Pending, (long)DTO.Enums.Status.Rejected };
                }

                if (statusIds.Any())
                {
                    liftingRequestListQueryContext = liftingRequestListQueryContext.Where(_ => statusIds.Contains(_.StatusId));
                }

                var LiftingRequestDetailsContext = _emamiContext.LiftingRequestDetails.AsNoTracking().ToList();
                var StateContext = _emamiContext.State.AsNoTracking().ToList();
                var SaudaOrderLiftingRequestMappingContext = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => !string.IsNullOrEmpty(_.DeliveryOrderNumber));
                var ApprovalStatus = _emamiContext.ApprovalStatus.AsNoTracking().ToList();
                foreach (var liftingRequest in liftingRequestListQueryContext.ToList())
                {
                    var liftingRequestDetail = LiftingRequestDetailsContext.Where(_ => _.LiftingRequestId == liftingRequest.Id).ToList();

                    var bdoContext = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.StateTrader)
                        .Join(_emamiContext.UserCustomerMapping.Where(_ => _.CustomerId == liftingRequest.UserId), u => u.UserId, ur => ur.UserId, (ur, ucm) => new { UserCustomerMapping = ucm, UserRoles = ur })
                        .Select(_ => _.UserCustomerMapping.User).FirstOrDefault();

                    string bdoName = string.Empty;
                    if (bdoContext != null)
                    {
                        bdoName = bdoContext.Name;
                    }

                    string stateName = StateContext.FirstOrDefault(_ => _.Id == liftingRequest.User.StateId)?.StateName;

                    foreach (var detail in liftingRequestDetail)
                    {
                        SaudaOrderLiftingRequestMapping saudaOrderLiftingRequestMapping;
                        if (inputDto.IsAfterDeliverOrderNumber)
                        {
                            saudaOrderLiftingRequestMapping = SaudaOrderLiftingRequestMappingContext.FirstOrDefault(_ => _.LiftingRequestDetailId == detail.Id);

                            if (saudaOrderLiftingRequestMapping != null)
                            {
                                var statusId2 = saudaOrderLiftingRequestMapping.StatusId;
                                var deliveryOrderNumber = saudaOrderLiftingRequestMapping != null ? saudaOrderLiftingRequestMapping.DeliveryOrderNumber : string.Empty;
                                var SaudaOrderNumber = saudaOrderLiftingRequestMapping.SaudaOrderId > 0 ? _emamiContext.SaudaOrders.FirstOrDefault(_ => _.Id == saudaOrderLiftingRequestMapping.SaudaOrderId).SaudaNumber : "";

                                var liftingDetailDto = new LiftingListReportDto
                                {
                                    LiftingRequestId = liftingRequest.Id,
                                    IndentNo = liftingRequest.LiftingRequestNumber,
                                    DealerName = liftingRequest.User?.Name,
                                    DealerCode = liftingRequest.User?.Code,
                                    //Destination = liftingRequest.User?.FreightRoute.Name,
                                    State = stateName,
                                    ShipToPartyId = liftingRequest.ShipToPartyId,
                                    ShipToPartyName = liftingRequest.ShipToParty?.Name,
                                    ShipToPartyCode = liftingRequest.ShipToParty?.Code,
                                    IndentReceivedDate = liftingRequest.LiftingDate,
                                    //IndentReceivedTime = liftingRequest.LiftingDate.TimeOfDay,
                                    TotalQuantityInMT = liftingRequestDetail.Sum(_ => _.LiftingQuantity),
                                    TotalQuantityInCase = liftingRequestDetail.Sum(_ => _.LiftingQuantityCase),
                                    SkuName = detail.Sku?.SkuName,
                                    SkuCode = detail.Sku?.SkuCode,
                                    LiftingQuantityInMT = detail.LiftingQuantity,
                                    LiftingQuantityCase = detail.LiftingQuantityCase,
                                    DeliveryOrderNumber = deliveryOrderNumber,
                                    Status1 = detail.StatusId > 0 ? ApprovalStatus.FirstOrDefault(_ => _.Id == liftingRequest.StatusId).Name : ApprovalStatus.FirstOrDefault(_ => _.Id == liftingRequest.StatusId).Name,
                                    Status2 = statusId2 > 0 ? ApprovalStatus.FirstOrDefault(_ => _.Id == statusId2).Name : string.Empty,
                                    BDOName = bdoName,
                                    InquiryNumber = detail.EnquiryNumber,
                                    ContractNumber = SaudaOrderNumber,
                                    DOStatus = detail.DOStatusId > 0 ? ApprovalStatus.FirstOrDefault(_ => _.Id == detail.DOStatusId).Name : string.Empty,
                                };
                                outputDto.Add(liftingDetailDto);
                            }
                        }
                        else
                        {
                            saudaOrderLiftingRequestMapping = SaudaOrderLiftingRequestMappingContext.FirstOrDefault(_ => _.LiftingRequestDetailId == detail.Id);

                            var liftingDetailDto = new LiftingListReportDto
                            {
                                LiftingRequestId = liftingRequest.Id,
                                IndentNo = liftingRequest.LiftingRequestNumber,
                                DealerName = liftingRequest.User?.Name,
                                DealerCode = liftingRequest.User?.Code,
                                //Destination = liftingRequest.User?.FreightRoute.Name,
                                State = stateName,
                                ShipToPartyId = liftingRequest.ShipToPartyId,
                                ShipToPartyName = liftingRequest.ShipToParty?.Name,
                                ShipToPartyCode = liftingRequest.ShipToParty?.Code,
                                IndentReceivedDate = liftingRequest.LiftingDate,
                                //IndentReceivedTime = liftingRequest.LiftingDate.TimeOfDay,
                                TotalQuantityInMT = liftingRequestDetail.Sum(_ => _.LiftingQuantity),
                                TotalQuantityInCase = liftingRequestDetail.Sum(_ => _.LiftingQuantityCase),
                                SkuName = detail.Sku?.SkuName,
                                SkuCode = detail.Sku?.SkuCode,
                                LiftingQuantityInMT = detail.LiftingQuantity,
                                LiftingQuantityCase = detail.LiftingQuantityCase,
                                DeliveryOrderNumber = detail.DeliveryOrderNumber ?? string.Empty,
                                Status1 = detail.StatusId > 0 ? ApprovalStatus.FirstOrDefault(_ => _.Id == liftingRequest.StatusId).Name : ApprovalStatus.FirstOrDefault(_ => _.Id == liftingRequest.StatusId).Name,
                                Status2 = string.Empty,
                                BDOName = bdoName,
                                InquiryNumber = detail.EnquiryNumber,
                                DOStatus = detail.DOStatusId > 0 ? ApprovalStatus.FirstOrDefault(_ => _.Id == detail.DOStatusId).Name : string.Empty,
                                //ContractNumber = SaudaOrderNumber
                            };
                            outputDto.Add(liftingDetailDto);
                        }
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

        #endregion


        #region PCP & MTP Report

        public ResultDto GetMTPDetailsReport(MonthlyTourPlanReportInputDto inputDto)
        {
            _methodName = "GetMTPDetailsReport";
            var resultDto = new ResultDto();
            var mtpDetailsList = new List<MonthlyTourPlanOutputDto>();

            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (inputDto.ZonalHeadIds == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (inputDto.VerticalId < 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var MonthlyTourPlans = _emamiContext.MonthlyTourPlans.AsNoTracking().ToList();
                var MonthlyTourPlanDetails = _emamiContext.MonthlyTourPlanDetails.AsNoTracking().ToList();
                var Users = _emamiContext.Users.AsNoTracking().ToList();
                var Cities = _emamiContext.City.AsNoTracking().ToList();
                var userDivisionMapping = _emamiContext.UserDivisionMappings.AsNoTracking().ToList();
                var ZonalHeadContext = Users.Where(_ => inputDto.ZonalHeadIds.Contains(_.Id)).Select(_ => _.Id).Distinct().ToList();
                
                

                var BDOList = new List<ReportSelectDto>();
                var cityDetails = _emamiContext.City.AsNoTracking().ToList();
                if (inputDto.BDOIds != null && inputDto.BDOIds.Any())
                {
                    if (inputDto.VerticalId > 0 && inputDto.SalesOrganizationId > 0 && inputDto.DistributionChannelId > 0)
                    {
                        var mtpdet = _emamiContext.MonthlyTourPlans.AsNoTracking()
                            .Join(_emamiContext.MonthlyTourPlanDetails.AsNoTracking(), mt => mt.Id, mtd => mtd.MonthlyTourPlanId, (mt, mtd) => new { mt, mtd })
                            .Join(_emamiContext.Users.AsNoTracking(), det => det.mt.CreatedBy, StateTrader => StateTrader.Id, (det, StateTrader) => new { mt = det.mt, mtd = det.mtd, StateTrader })
                            .Join(_emamiContext.UserDivisionMappings.AsNoTracking(), mdetails => mdetails.StateTrader.Id, ud => ud.UserId, (mdetails, ud) => new { mt = mdetails.mt, mtd = mdetails.mtd, StateTrader = mdetails.StateTrader, ud })
                            .Join(_emamiContext.Users.AsNoTracking(), _ => _.StateTrader.ReportingToId, zhead => zhead.Id, (_, zhead) => new { mtPlans = _.mt, mtDetatils = _.mtd, User = _.StateTrader, udiv = _.ud, zhead })
                            .Where(_ => inputDto.BDOIds.Contains(_.User.Id) && inputDto.SalesOrganizationId == _.udiv.SalesOrganizationId && inputDto.DistributionChannelId == _.udiv.DistributionChannelId && inputDto.VerticalId == _.udiv.DivisionId && (DbFunctions.TruncateTime(_.mtDetatils.Date) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                        DbFunctions.TruncateTime(_.mtDetatils.Date) <= DbFunctions.TruncateTime(inputDto.ToDate)))
                            //.Join(_emamiContext.Users.AsNoTracking(), udetails=>long.Parse(udetails.mtd.DealerId), uc=>uc.Id, (udetails,uc)=>new { User=udetails.StateTrader, mtDetatils = udetails.mtd, mtPlans= udetails.mt, zhead=udetails.zhead,uc })
                            .AsEnumerable().Select(s => new MonthlyTourPlanOutputDto()
                            {
                                ZonalHeadName = s.zhead.Name,
                                BDOName = s.User.Name,
                                Date = s.mtDetatils.Date,
                                CityId = s.mtDetatils.TownId,
                                City = cityDetails.FirstOrDefault(_ => _.Id == s.mtDetatils.TownId) != null ? cityDetails.FirstOrDefault(_ => _.Id == s.mtDetatils.TownId).CityName : String.Empty,
                                Area = s.mtDetatils.Area,
                                DealerId = s.mtDetatils.DealerId,
                                //Dealer=s.uc.Name,
                                //HeadquartersId = s.mtDetatils.HeadquartersId,
                                //Headquarters = s.mtDetatils.Headquarters.Name,
                                Remarks = s.mtDetatils.Remarks,
                                InHQNoVisitId = s.mtDetatils.InHQNoVisit,
                                //InHQNoVisitName = s.mtDetatils.InHQNoVisit != 0 ? Utility.GetEnumFromString<DTO.Enums.STPVisitType>(s.mtDetatils.InHQNoVisit) : string.Empty,
                                CreatedDate = s.mtDetatils.CreatedDate,
                                MTPNumber = s.mtPlans.MTPNumber
                            }).Distinct().ToList(); ;
                        foreach (var mtp in mtpdet)
                        {
                            if (!String.IsNullOrEmpty(mtp.DealerId) && mtp.DealerId != "0")
                            {
                                var id = long.Parse(mtp.DealerId);
                                mtp.Dealer = _emamiContext.Users.Where(_ => _.Id == id).FirstOrDefault() != null ? _emamiContext.Users.Where(_ => _.Id == id).FirstOrDefault().Name : string.Empty;
                            }
                            else
                            {
                                mtp.Dealer = string.Empty;
                            }
                            mtpDetailsList.Add(mtp);
                        }

                    }
                    else
                    {

                        var mtpdet = _emamiContext.MonthlyTourPlans.AsNoTracking()
                             .Join(_emamiContext.MonthlyTourPlanDetails.AsNoTracking(), mt => mt.Id, mtd => mtd.MonthlyTourPlanId, (mt, mtd) => new { mt, mtd })
                             .Join(_emamiContext.Users.AsNoTracking(), det => det.mt.CreatedBy, StateTrader => StateTrader.Id, (det, StateTrader) => new { mt = det.mt, mtd = det.mtd, StateTrader })
                             .Join(_emamiContext.UserDivisionMappings.AsNoTracking(), mdetails => mdetails.StateTrader.Id, ud => ud.UserId, (mdetails, ud) => new { mt = mdetails.mt, mtd = mdetails.mtd, StateTrader = mdetails.StateTrader, ud })
                             .Join(_emamiContext.Users.AsNoTracking(), _ => _.StateTrader.ReportingToId, zhead => zhead.Id, (_, zhead) => new { mtPlans = _.mt, mtDetatils = _.mtd, User = _.StateTrader, udiv = _.ud, zhead })
                             .Where(_ => inputDto.BDOIds.Contains(_.User.Id) && (DbFunctions.TruncateTime(_.mtDetatils.Date) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                         DbFunctions.TruncateTime(_.mtDetatils.Date) <= DbFunctions.TruncateTime(inputDto.ToDate)))
                             //.Join(_emamiContext.Users.AsNoTracking(), udetails=>long.Parse(udetails.mtd.DealerId), uc=>uc.Id, (udetails,uc)=>new { User=udetails.StateTrader, mtDetatils = udetails.mtd, mtPlans= udetails.mt, zhead=udetails.zhead,uc })
                             .AsEnumerable().Select(s => new MonthlyTourPlanOutputDto()
                             {
                                 ZonalHeadName = s.zhead.Name,
                                 BDOName = s.User.Name,
                                 Date = s.mtDetatils.Date,
                                 CityId = s.mtDetatils.TownId,
                                 City = cityDetails.FirstOrDefault(_ => _.Id == s.mtDetatils.TownId) != null ? cityDetails.FirstOrDefault(_ => _.Id == s.mtDetatils.TownId).CityName : String.Empty,
                                 Area = s.mtDetatils.Area,
                                 DealerId = s.mtDetatils.DealerId,
                                 //Dealer=s.uc.Name,
                                 //HeadquartersId = s.mtDetatils.HeadquartersId,
                                 //Headquarters = s.mtDetatils.Headquarters.Name,
                                 Remarks = s.mtDetatils.Remarks,
                                 InHQNoVisitId = s.mtDetatils.InHQNoVisit,
                                 //InHQNoVisitName = s.mtDetatils.InHQNoVisit != 0 ? Utility.GetEnumFromString<DTO.Enums.STPVisitType>(s.mtDetatils.InHQNoVisit) : string.Empty,
                                 CreatedDate = s.mtDetatils.CreatedDate,
                                 MTPNumber = s.mtPlans.MTPNumber
                             }).Distinct().ToList(); ;
                        foreach (var mtp in mtpdet)
                        {
                            if (!String.IsNullOrEmpty(mtp.DealerId) && mtp.DealerId != "0")
                            {
                                var id = long.Parse(mtp.DealerId);
                                mtp.Dealer = _emamiContext.Users.Where(_ => _.Id == id).FirstOrDefault() != null ? _emamiContext.Users.Where(_ => _.Id == id).FirstOrDefault().Name : string.Empty;
                            }
                            else
                            {
                                mtp.Dealer = string.Empty;
                            }
                            mtpDetailsList.Add(mtp);
                        }

                    }
                }
                else
                {
                    if (inputDto.VerticalId > 0)
                    {
                        var mtpdet = _emamiContext.MonthlyTourPlans.AsNoTracking()
                            .Join(_emamiContext.MonthlyTourPlanDetails.AsNoTracking(), mt => mt.Id, mtd => mtd.MonthlyTourPlanId, (mt, mtd) => new { mt, mtd })
                            .Join(_emamiContext.Users.AsNoTracking(), det => det.mt.CreatedBy, StateTrader => StateTrader.Id, (det, StateTrader) => new { mt = det.mt, mtd = det.mtd, StateTrader })
                            .Join(_emamiContext.UserDivisionMappings.AsNoTracking(), mdetails => mdetails.StateTrader.Id, ud => ud.UserId, (mdetails, ud) => new { mt = mdetails.mt, mtd = mdetails.mtd, StateTrader = mdetails.StateTrader, ud })
                            .Join(_emamiContext.Users.AsNoTracking(), _ => _.StateTrader.ReportingToId, zhead => zhead.Id, (_, zhead) => new { mtPlans = _.mt, mtDetatils = _.mtd, User = _.StateTrader, udiv = _.ud, zhead })
                            .Where(_ => inputDto.ZonalHeadIds.Contains((long)_.User.ReportingToId) && inputDto.SalesOrganizationId == _.udiv.SalesOrganizationId && inputDto.DistributionChannelId == _.udiv.DistributionChannelId && inputDto.VerticalId == _.udiv.DivisionId && (DbFunctions.TruncateTime(_.mtDetatils.Date) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                        DbFunctions.TruncateTime(_.mtDetatils.Date) <= DbFunctions.TruncateTime(inputDto.ToDate)))
                            //.Join(_emamiContext.Users.AsNoTracking(), udetails=>long.Parse(udetails.mtd.DealerId), uc=>uc.Id, (udetails,uc)=>new { User=udetails.StateTrader, mtDetatils = udetails.mtd, mtPlans= udetails.mt, zhead=udetails.zhead,uc })
                            .AsEnumerable().Select(s => new MonthlyTourPlanOutputDto()
                            {
                                ZonalHeadName = s.zhead.Name,
                                BDOName = s.User.Name,
                                Date = s.mtDetatils.Date,
                                CityId = s.mtDetatils.TownId,
                                City = cityDetails.FirstOrDefault(_ => _.Id == s.mtDetatils.TownId) != null ? cityDetails.FirstOrDefault(_ => _.Id == s.mtDetatils.TownId).CityName : String.Empty,
                                Area = s.mtDetatils.Area,
                                DealerId = s.mtDetatils.DealerId,
                                //Dealer=s.uc.Name,
                                //HeadquartersId = s.mtDetatils.HeadquartersId,
                                //Headquarters = s.mtDetatils.Headquarters.Name,
                                Remarks = s.mtDetatils.Remarks,
                                InHQNoVisitId = s.mtDetatils.InHQNoVisit,
                                //InHQNoVisitName = s.mtDetatils.InHQNoVisit != 0 ? Utility.GetEnumFromString<DTO.Enums.STPVisitType>(s.mtDetatils.InHQNoVisit) : string.Empty,
                                CreatedDate = s.mtDetatils.CreatedDate,
                                MTPNumber = s.mtPlans.MTPNumber
                            }).Distinct().ToList(); ;
                        foreach (var mtp in mtpdet)
                        {
                            if (!String.IsNullOrEmpty(mtp.DealerId) && mtp.DealerId != "0")
                            {
                                var id = long.Parse(mtp.DealerId);
                                mtp.Dealer = _emamiContext.Users.Where(_ => _.Id == id).FirstOrDefault() != null ? _emamiContext.Users.Where(_ => _.Id == id).FirstOrDefault().Name : string.Empty;
                            }
                            else
                            {
                                mtp.Dealer = string.Empty;
                            }
                            mtpDetailsList.Add(mtp);
                        }

                    }
                    else
                    {
                        
                        var mtpdet = _emamiContext.MonthlyTourPlans.AsNoTracking()
                             .Join(_emamiContext.MonthlyTourPlanDetails.AsNoTracking(), mt => mt.Id, mtd => mtd.MonthlyTourPlanId, (mt, mtd) => new { mt, mtd })
                             .Join(_emamiContext.Users.AsNoTracking(), det => det.mt.CreatedBy, StateTrader => StateTrader.Id, (det, StateTrader) => new { mt = det.mt, mtd = det.mtd, StateTrader })
                             .Join(_emamiContext.UserDivisionMappings.AsNoTracking(), mdetails => mdetails.StateTrader.Id, ud => ud.UserId, (mdetails, ud) => new { mt = mdetails.mt, mtd = mdetails.mtd, StateTrader = mdetails.StateTrader, ud })
                             .Join(_emamiContext.Users.AsNoTracking(), _ => _.StateTrader.ReportingToId, zhead => zhead.Id, (_, zhead) => new { mtPlans = _.mt, mtDetatils = _.mtd, User = _.StateTrader, udiv = _.ud, zhead })
                             .Where(_ => inputDto.ZonalHeadIds.Contains((long)_.User.ReportingToId) && (DbFunctions.TruncateTime(_.mtDetatils.Date) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                         DbFunctions.TruncateTime(_.mtDetatils.Date) <= DbFunctions.TruncateTime(inputDto.ToDate)))
                             //.Join(_emamiContext.Users.AsNoTracking(), udetails=>long.Parse(udetails.mtd.DealerId), uc=>uc.Id, (udetails,uc)=>new { User=udetails.StateTrader, mtDetatils = udetails.mtd, mtPlans= udetails.mt, zhead=udetails.zhead,uc })
                             .AsEnumerable().Select(s => new MonthlyTourPlanOutputDto()
                             {
                                 ZonalHeadName = s.zhead.Name,
                                 BDOName = s.User.Name,
                                 Date = s.mtDetatils.Date,
                                 CityId = s.mtDetatils.TownId,
                                 City = cityDetails.FirstOrDefault(_ => _.Id==s.mtDetatils.TownId) != null ? cityDetails.FirstOrDefault(_ => _.Id == s.mtDetatils.TownId).CityName : String.Empty ,
                                 Area = s.mtDetatils.Area,
                                 DealerId = s.mtDetatils.DealerId,
                                 //Dealer=s.uc.Name,
                                 //HeadquartersId = s.mtDetatils.HeadquartersId,
                                 //Headquarters = s.mtDetatils.Headquarters.Name,
                                 Remarks = s.mtDetatils.Remarks,
                                 InHQNoVisitId = s.mtDetatils.InHQNoVisit,
                                 //InHQNoVisitName = s.mtDetatils.InHQNoVisit != 0 ? Utility.GetEnumFromString<DTO.Enums.STPVisitType>(s.mtDetatils.InHQNoVisit) : string.Empty,
                                 CreatedDate = s.mtDetatils.CreatedDate,
                                 MTPNumber = s.mtPlans.MTPNumber
                             }).Distinct().ToList(); ;
                        foreach (var mtp in mtpdet)
                        {
                            if (!String.IsNullOrEmpty(mtp.DealerId) && mtp.DealerId != "0")
                            {
                                var id = long.Parse(mtp.DealerId);
                                mtp.Dealer = _emamiContext.Users.Where(_ => _.Id == id).FirstOrDefault() != null ? _emamiContext.Users.Where(_ => _.Id == id).FirstOrDefault().Name : string.Empty;
                            }
                            else
                            {
                                mtp.Dealer = string.Empty;
                            }
                            mtpDetailsList.Add(mtp);
                        }
                    }
                }
                //var mtpDetails= _emamiContext.MonthlyTourPlans.Where(_ => BDOList.Contains(_.CreatedBy))

                //foreach (var StateTrader in BDOList.ToList())
                //{
                //    var bdoCreatedContext = MonthlyTourPlans.Where(_ => _.CreatedBy == StateTrader.BDOId).ToList();
                //    foreach (var MTP in bdoCreatedContext)
                //    {
                //        var Date = MonthlyTourPlanDetails.FirstOrDefault(_ => _.MonthlyTourPlanId == MTP.Id);
                //        if (Date != null)
                //        {
                //            var mtpContext = _emamiContext.MonthlyTourPlans.FirstOrDefault(_ => _.Id == MTP.Id && (DbFunctions.TruncateTime(Date.Date) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                //                                                           DbFunctions.TruncateTime(Date.Date) <= DbFunctions.TruncateTime(inputDto.ToDate)));
                //            if (mtpContext != null)
                //            {
                //                if (mtpContext.MTPDetails.Any())
                //                {
                //                    foreach (var mtpdetails in mtpContext.MTPDetails)
                //                    {
                //                        var mtpDetails = new MonthlyTourPlanOutputDto
                //                        {
                //                            ZonalHeadName = Users.FirstOrDefault(_ => _.Id == StateTrader.ZonalHeadId).Name,
                //                            BDOName = Users.FirstOrDefault(_ => _.Id == StateTrader.BDOId).Name,
                //                            Date = mtpdetails.Date,
                //                            CityId = mtpdetails.TownId,
                //                            City = Cities.FirstOrDefault(_ => _.Id == mtpdetails.TownId)?.CityName,
                //                            Area = mtpdetails.Area,
                //                            DealerId = mtpdetails.DealerId,
                //                            HeadquartersId = mtpdetails.HeadquartersId,
                //                            Headquarters = _emamiContext.Headquarters.FirstOrDefault(_ => _.Id == mtpdetails.HeadquartersId)?.Name,
                //                            Remarks = mtpdetails.Remarks,
                //                            InHQNoVisitId = mtpdetails.InHQNoVisit,
                //                            InHQNoVisitName = mtpdetails.InHQNoVisit != 0 ? Utility.GetEnumFromString<DTO.Enums.STPVisitType>(mtpdetails.InHQNoVisit) : string.Empty,
                //                            CreatedDate = mtpdetails.CreatedDate,
                //                            MTPNumber = mtpContext.MTPNumber
                //                        };

                //                        if (!string.IsNullOrEmpty(mtpDetails.DealerId) && mtpDetails.DealerId != "0")
                //                        {
                //                            var dealerIdsList = mtpdetails.DealerId.Split(',');
                //                            var dealerNames = string.Empty;
                //                            //var state = string.Empty;
                //                            //var district = string.Empty;
                //                            foreach (var dealer in dealerIdsList)
                //                            {
                //                                var dealerId = long.Parse(dealer);
                //                                dealerNames = dealerId != 0 ? Users.FirstOrDefault(_ => _.Id == dealerId).Name : "";
                //                                //state =  stateId !=0 ? _emamiContext.State.FirstOrDefault(_ => _.Id == stateId).StateName : "";
                //                                //district = districtId !=0 ? _emamiContext.District.FirstOrDefault(_ => _.Id == districtId).DistrictName : "";
                //                            }
                //                            mtpDetails.Dealer = dealerNames.Remove(dealerNames.Length - 1, 1);
                //                            //mtpDetails.State = state.Remove(state.Length - 1, 1);
                //                            //mtpDetails.District = district.Remove(district.Length - 1, 1);
                //                        }
                //                        else
                //                        {
                //                            mtpDetails.Dealer = string.Empty;
                //                            //mtpDetails.State = string.Empty;
                //                            //mtpDetails.District = string.Empty;
                //                        }

                //                        mtpDetailsList.Add(mtpDetails);
                //                    }
                //                }
                //            }
                //        }
                        
                //    }
                //}
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = mtpDetailsList;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetPCPDetailsReport(PermanentCoveragePlanReportInputDto inputDto)
        {
            _methodName = "GetMTPDetailsReport";
            var resultDto = new ResultDto();
            var pcpDetailsList = new List<PermanentCoveragePlanReportOutputDto>();

            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (inputDto.ZonalHeadIds == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }
                if (inputDto.VerticalId < 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
                    resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
                    return resultDto;
                }

                var Users = _emamiContext.Users.AsNoTracking().ToList();
                var City = _emamiContext.City.AsNoTracking().ToList();
                var State = _emamiContext.State.AsNoTracking().ToList();
                var district = _emamiContext.District.AsNoTracking().ToList();
                var territory = _emamiContext.Territory.AsNoTracking().ToList();
                var ZonalHeadContext = Users.Where(_ => inputDto.ZonalHeadIds.Contains(_.Id)).Select(_ => _.Id).Distinct().ToList();
                var userDivisionMapping = _emamiContext.UserDivisionMappings.AsNoTracking().ToList();

                var BDOList = new List<ReportSelectDto>();

                if (inputDto.BDOIds != null && inputDto.BDOIds.Any())
                {
                    if (inputDto.VerticalId > 0 && inputDto.SalesOrganizationId>0 && inputDto.DistributionChannelId>0)
                    {
                        pcpDetailsList = _emamiContext.PermanentJourneyPlans.AsNoTracking()
                    .Join(_emamiContext.PermanentJourneyPlanDetails.AsNoTracking(), y => y.Id, pd => pd.PermanentJourneyPlanId, (y, pd) => new { pjPlans = y, pjdetails = pd })
                    //.Join(_emamiContext.City.AsNoTracking(), y => y.pjdetails.TownId, c => c.Id, (y, c) => new { pjPlans = y.pjPlans, pjdetails = y.pjdetails, city = c.CityName })
                    //.Join(_emamiContext.District.AsNoTracking(), y => y.pjdetails.DistrictId, d => d.Id, (y, d) => new { pjPlans = y.pjPlans, pjdetails = y.pjdetails, city = y.city, district = d.DistrictName })
                    .Join(_emamiContext.State.AsNoTracking(), y => y.pjdetails.StateId, d => d.Id, (y, d) => new { pjPlans = y.pjPlans, pjdetails = y.pjdetails,/* city = y.city, district = y.district,*/ state = d.StateName })
                    //.Join(_emamiContext.Territory.AsNoTracking(), y => y.pjdetails.TerritoryId, d => d.Id, (y, d) => new { pjPlans = y.pjPlans, pjdetails = y.pjdetails, city = y.city, district = y.district, state = y.state, territory = d.Name })
                    .Join(_emamiContext.FinancialYears.AsNoTracking(), p => p.pjPlans.FinancialYearId, fy => fy.Id, (p, fy) => new { pjPlans = p.pjPlans, pjdetails = p.pjdetails, fyear = fy,/* city = p.city, district = p.district,*/ state = p.state/* territory = p.territory*/ })
                    .Join(_emamiContext.Users.AsNoTracking(), y => y.pjPlans.CreatedBy, StateTrader => StateTrader.Id, (y, StateTrader) => new { pjPlans = y.pjPlans, fyear = y.fyear, StateTrader, pjdetails = y.pjdetails, /*city = y.city, district = y.district,*/ state = y.state/*, territory = y.territory*/ })
                    .Join(_emamiContext.Users.AsNoTracking(), y => y.StateTrader.ReportingToId, zhead => zhead.Id, (y, zhead) => new { pjPlans = y.pjPlans, fyear = y.fyear, StateTrader = y.StateTrader, zhead, pjdetails = y.pjdetails, /*city = y.city, district = y.district,*/ state = y.state/*, territory = y.territory*/ })
                    .Join(_emamiContext.UserDivisionMappings.AsNoTracking(), y => y.StateTrader.Id, ud => ud.UserId, (y, ud) => new { pjPlans = y.pjPlans, fyear = y.fyear, StateTrader = y.StateTrader, zhead = y.zhead, udiv = ud, pjdetails = y.pjdetails, /*city = y.city, district = y.district,*/ state = y.state/*, territory = y.territory*/ })
                    .Where(_ => inputDto.BDOIds.Contains(_.StateTrader.Id) && _.udiv.SalesOrganizationId == inputDto.SalesOrganizationId && _.udiv.DistributionChannelId == inputDto.DistributionChannelId && _.udiv.DivisionId == inputDto.VerticalId && ((DbFunctions.TruncateTime(_.pjPlans.EffectiveFrom) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                           DbFunctions.TruncateTime(_.pjPlans.EffectiveFrom) <= DbFunctions.TruncateTime(inputDto.ToDate))
                                                                           ||
                                                                           (DbFunctions.TruncateTime(_.pjPlans.EffectiveTo) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                           DbFunctions.TruncateTime(_.pjPlans.EffectiveTo) <= DbFunctions.TruncateTime(inputDto.ToDate))))
                    .AsEnumerable().Select(s => new PermanentCoveragePlanReportOutputDto()
                    {
                        ZonalHeadName = s.zhead.Name,
                        BDOName = s.StateTrader.Name,
                        City = s.pjdetails.TownId > 0 ? City.FirstOrDefault(_ => _.Id == s.pjdetails.TownId).CityName : String.Empty,
                        CityId = s.pjdetails.TownId,
                        CreatedDate = s.pjPlans.CreatedDate,
                        District = s.pjdetails.DistrictId > 0 ? district.FirstOrDefault(_ => _.Id == s.pjdetails.DistrictId).DistrictName : String.Empty,
                        DistrictId = (int)s.pjdetails.DistrictId,
                        EffectiveFrom = s.pjPlans.EffectiveFrom,
                        EffectiveTo = s.pjPlans.EffectiveTo,
                        Year = s.fyear.Year,
                        State = s.state,
                        //Territory = s.territory,
                        NoOfSubDealer = s.pjdetails.NoofSubDealer,
                        NoOfWholeSeller = s.pjdetails.NoOfWholeSeller,
                        NoOfVisit = s.pjdetails.NoOfVisit,
                        Remarks = s.pjdetails.Remarks,
                        InHQNoVisitId = s.pjdetails.InHQNoVisit,
                        //InHQNoVisitName= Utility.GetEnumFromString<DTO.Enums.STPVisitType>(s.pjdetails.InHQNoVisit),
                        InHQNoVisitName = DTO.Enums.STPVisitType.InHQNoVisit.ToString(),
                        PCPNumber = s.pjPlans.PJPNumber

                    }).Distinct().ToList();
                  
                    }
                    else
                    {
                        pcpDetailsList = _emamiContext.PermanentJourneyPlans.AsNoTracking()
                    .Join(_emamiContext.PermanentJourneyPlanDetails.AsNoTracking(), y => y.Id, pd => pd.PermanentJourneyPlanId, (y, pd) => new { pjPlans = y, pjdetails = pd })
                    //.Join(_emamiContext.City.AsNoTracking(), y => y.pjdetails.TownId, c => c.Id, (y, c) => new { pjPlans = y.pjPlans, pjdetails = y.pjdetails, city = c.CityName })
                    //.Join(_emamiContext.District.AsNoTracking(), y => y.pjdetails.DistrictId, d => d.Id, (y, d) => new { pjPlans = y.pjPlans, pjdetails = y.pjdetails, city = y.city, district = d.DistrictName })
                    .Join(_emamiContext.State.AsNoTracking(), y => y.pjdetails.StateId, d => d.Id, (y, d) => new { pjPlans = y.pjPlans, pjdetails = y.pjdetails,/* city = y.city, district = y.district,*/ state = d.StateName })
                    //.Join(_emamiContext.Territory.AsNoTracking(), y => y.pjdetails.TerritoryId, d => d.Id, (y, d) => new { pjPlans = y.pjPlans, pjdetails = y.pjdetails, city = y.city, district = y.district, state = y.state, territory = d.Name })
                    .Join(_emamiContext.FinancialYears.AsNoTracking(), p => p.pjPlans.FinancialYearId, fy => fy.Id, (p, fy) => new { pjPlans = p.pjPlans, pjdetails = p.pjdetails, fyear = fy,/* city = p.city, district = p.district,*/ state = p.state/* territory = p.territory*/ })
                    .Join(_emamiContext.Users.AsNoTracking(), y => y.pjPlans.CreatedBy, StateTrader => StateTrader.Id, (y, StateTrader) => new { pjPlans = y.pjPlans, fyear = y.fyear, StateTrader, pjdetails = y.pjdetails, /*city = y.city, district = y.district,*/ state = y.state/*, territory = y.territory*/ })
                    .Join(_emamiContext.Users.AsNoTracking(), y => y.StateTrader.ReportingToId, zhead => zhead.Id, (y, zhead) => new { pjPlans = y.pjPlans, fyear = y.fyear, StateTrader = y.StateTrader, zhead, pjdetails = y.pjdetails, /*city = y.city, district = y.district,*/ state = y.state/*, territory = y.territory*/ })
                    //.Join(_emamiContext.UserDivisionMappings.AsNoTracking(), y => y.StateTrader.Id, ud => ud.UserId, (y, ud) => new { pjPlans = y.pjPlans, fyear = y.fyear, StateTrader = y.StateTrader, zhead = y.zhead, udiv = ud, pjdetails = y.pjdetails, city = y.city, district = y.district, state = y.state, territory = y.territory })
                    .Where(_ => inputDto.BDOIds.Contains(_.StateTrader.Id) && ((DbFunctions.TruncateTime(_.pjPlans.EffectiveFrom) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                           DbFunctions.TruncateTime(_.pjPlans.EffectiveFrom) <= DbFunctions.TruncateTime(inputDto.ToDate))
                                                                           ||
                                                                           (DbFunctions.TruncateTime(_.pjPlans.EffectiveTo) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                           DbFunctions.TruncateTime(_.pjPlans.EffectiveTo) <= DbFunctions.TruncateTime(inputDto.ToDate))))
                    .AsEnumerable().Select(s => new PermanentCoveragePlanReportOutputDto()
                    {
                        ZonalHeadName = s.zhead.Name,
                        BDOName = s.StateTrader.Name,
                        City = s.pjdetails.TownId > 0 ? City.FirstOrDefault(_ => _.Id==s.pjdetails.TownId).CityName : String.Empty,
                        CityId = s.pjdetails.TownId,
                        CreatedDate = s.pjPlans.CreatedDate,
                        District = s.pjdetails.DistrictId > 0 ? district.FirstOrDefault(_ => _.Id==s.pjdetails.DistrictId).DistrictName : String.Empty,
                        DistrictId = (int)s.pjdetails.DistrictId,
                        EffectiveFrom = s.pjPlans.EffectiveFrom,
                        EffectiveTo = s.pjPlans.EffectiveTo,
                        Year = s.fyear.Year,
                        State = s.state,
                        //Territory = s.territory,
                        NoOfSubDealer = s.pjdetails.NoofSubDealer,
                        NoOfWholeSeller = s.pjdetails.NoOfWholeSeller,
                        NoOfVisit = s.pjdetails.NoOfVisit,
                        Remarks = s.pjdetails.Remarks,
                        InHQNoVisitId = s.pjdetails.InHQNoVisit,
                        //InHQNoVisitName= Utility.GetEnumFromString<DTO.Enums.STPVisitType>(s.pjdetails.InHQNoVisit),
                        InHQNoVisitName = DTO.Enums.STPVisitType.InHQNoVisit.ToString(),
                        PCPNumber = s.pjPlans.PJPNumber

                    }).ToList();
                    }
                }
                else
                {
                    if (inputDto.VerticalId > 0 && inputDto.SalesOrganizationId > 0 && inputDto.DistributionChannelId > 0)
                    {
                        pcpDetailsList = _emamiContext.PermanentJourneyPlans.AsNoTracking()
                    .Join(_emamiContext.PermanentJourneyPlanDetails.AsNoTracking(), y => y.Id, pd => pd.PermanentJourneyPlanId, (y, pd) => new { pjPlans = y, pjdetails = pd })
                    //.Join(_emamiContext.City.AsNoTracking(), y => y.pjdetails.TownId, c => c.Id, (y, c) => new { pjPlans = y.pjPlans, pjdetails = y.pjdetails, city = c.CityName })
                    //.Join(_emamiContext.District.AsNoTracking(), y => y.pjdetails.DistrictId, d => d.Id, (y, d) => new { pjPlans = y.pjPlans, pjdetails = y.pjdetails, city = y.city, district = d.DistrictName })
                    .Join(_emamiContext.State.AsNoTracking(), y => y.pjdetails.StateId, d => d.Id, (y, d) => new { pjPlans = y.pjPlans, pjdetails = y.pjdetails,/* city = y.city, district = y.district,*/ state = d.StateName })
                    //.Join(_emamiContext.Territory.AsNoTracking(), y => y.pjdetails.TerritoryId, d => d.Id, (y, d) => new { pjPlans = y.pjPlans, pjdetails = y.pjdetails, city = y.city, district = y.district, state = y.state, territory = d.Name })
                    .Join(_emamiContext.FinancialYears.AsNoTracking(), p => p.pjPlans.FinancialYearId, fy => fy.Id, (p, fy) => new { pjPlans = p.pjPlans, pjdetails = p.pjdetails, fyear = fy,/* city = p.city, district = p.district,*/ state = p.state/* territory = p.territory*/ })
                    .Join(_emamiContext.Users.AsNoTracking(), y => y.pjPlans.CreatedBy, StateTrader => StateTrader.Id, (y, StateTrader) => new { pjPlans = y.pjPlans, fyear = y.fyear, StateTrader, pjdetails = y.pjdetails, /*city = y.city, district = y.district,*/ state = y.state/*, territory = y.territory*/ })
                    .Join(_emamiContext.Users.AsNoTracking(), y => y.StateTrader.ReportingToId, zhead => zhead.Id, (y, zhead) => new { pjPlans = y.pjPlans, fyear = y.fyear, StateTrader = y.StateTrader, zhead, pjdetails = y.pjdetails, /*city = y.city, district = y.district,*/ state = y.state/*, territory = y.territory*/ })
                    .Join(_emamiContext.UserDivisionMappings.AsNoTracking(), y => y.StateTrader.Id, ud => ud.UserId, (y, ud) => new { pjPlans = y.pjPlans, fyear = y.fyear, StateTrader = y.StateTrader, zhead = y.zhead, udiv = ud, pjdetails = y.pjdetails, /*city = y.city, district = y.district,*/ state = y.state/*, territory = y.territory */})
                    .Where(_ => inputDto.ZonalHeadIds.Contains((long)_.StateTrader.ReportingToId) && _.udiv.SalesOrganizationId == inputDto.SalesOrganizationId && _.udiv.DistributionChannelId == inputDto.DistributionChannelId && _.udiv.DivisionId == inputDto.VerticalId && ((DbFunctions.TruncateTime(_.pjPlans.EffectiveFrom) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                           DbFunctions.TruncateTime(_.pjPlans.EffectiveFrom) <= DbFunctions.TruncateTime(inputDto.ToDate))
                                                                           ||
                                                                           (DbFunctions.TruncateTime(_.pjPlans.EffectiveTo) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                           DbFunctions.TruncateTime(_.pjPlans.EffectiveTo) <= DbFunctions.TruncateTime(inputDto.ToDate))))
                    .AsEnumerable().Select(s => new PermanentCoveragePlanReportOutputDto()
                    {
                        ZonalHeadName = s.zhead.Name,
                        BDOName = s.StateTrader.Name,
                        City = s.pjdetails.TownId > 0 ? City.FirstOrDefault(_ => _.Id == s.pjdetails.TownId).CityName : String.Empty,
                        District = s.pjdetails.DistrictId > 0 ? district.FirstOrDefault(_ => _.Id == s.pjdetails.DistrictId).DistrictName : String.Empty,
                        CityId = s.pjdetails.TownId,
                        CreatedDate = s.pjPlans.CreatedDate,
                        DistrictId = (int)s.pjdetails.DistrictId,
                        EffectiveFrom = s.pjPlans.EffectiveFrom,
                        EffectiveTo = s.pjPlans.EffectiveTo,
                        Year = s.fyear.Year,
                        State = s.state,
                        //Territory = s.territory,
                        NoOfSubDealer = s.pjdetails.NoofSubDealer,
                        NoOfWholeSeller = s.pjdetails.NoOfWholeSeller,
                        NoOfVisit = s.pjdetails.NoOfVisit,
                        Remarks = s.pjdetails.Remarks,
                        InHQNoVisitId = s.pjdetails.InHQNoVisit,
                        //InHQNoVisitName= Utility.GetEnumFromString<DTO.Enums.STPVisitType>(s.pjdetails.InHQNoVisit),
                        InHQNoVisitName = DTO.Enums.STPVisitType.InHQNoVisit.ToString(),
                        PCPNumber = s.pjPlans.PJPNumber

                    }).ToList();
                       
                    }
                    else
                    {
                        pcpDetailsList = pcpDetailsList = _emamiContext.PermanentJourneyPlans.AsNoTracking()
                    .Join(_emamiContext.PermanentJourneyPlanDetails.AsNoTracking(), y => y.Id, pd => pd.PermanentJourneyPlanId, (y, pd) => new { pjPlans = y, pjdetails = pd })
                    //.Join(_emamiContext.City.AsNoTracking(), y => y.pjdetails.TownId, c => c.Id, (y, c) => new { pjPlans = y.pjPlans, pjdetails = y.pjdetails, city = c.CityName })
                    //.Join(_emamiContext.District.AsNoTracking(), y => y.pjdetails.DistrictId, d => d.Id, (y, d) => new { pjPlans = y.pjPlans, pjdetails = y.pjdetails, city = y.city, district = d.DistrictName })
                    .Join(_emamiContext.State.AsNoTracking(), y => y.pjdetails.StateId, d => d.Id, (y, d) => new { pjPlans = y.pjPlans, pjdetails = y.pjdetails,/* city = y.city, district = y.district,*/ state = d.StateName })
                    //.Join(_emamiContext.Territory.AsNoTracking(), y => y.pjdetails.TerritoryId, d => d.Id, (y, d) => new { pjPlans = y.pjPlans, pjdetails = y.pjdetails, city = y.city, district = y.district, state = y.state, territory = d.Name })
                    .Join(_emamiContext.FinancialYears.AsNoTracking(), p => p.pjPlans.FinancialYearId, fy => fy.Id, (p, fy) => new { pjPlans = p.pjPlans, pjdetails = p.pjdetails, fyear = fy,/* city = p.city, district = p.district,*/ state = p.state/* territory = p.territory*/ })
                    .Join(_emamiContext.Users.AsNoTracking(), y => y.pjPlans.CreatedBy, StateTrader => StateTrader.Id, (y, StateTrader) => new { pjPlans = y.pjPlans, fyear = y.fyear, StateTrader, pjdetails = y.pjdetails, /*city = y.city, district = y.district,*/ state = y.state/*, territory = y.territory*/ })
                    .Join(_emamiContext.Users.AsNoTracking(), y => y.StateTrader.ReportingToId, zhead => zhead.Id, (y, zhead) => new { pjPlans = y.pjPlans, fyear = y.fyear, StateTrader = y.StateTrader, zhead, pjdetails = y.pjdetails, /*city = y.city, district = y.district,*/ state = y.state/*, territory = y.territory*/ })
                    //.Join(_emamiContext.UserDivisionMappings.AsNoTracking(), y => y.StateTrader.Id, ud => ud.UserId, (y, ud) => new { pjPlans = y.pjPlans, fyear = y.fyear, StateTrader = y.StateTrader, zhead = y.zhead, udiv = ud, pjdetails = y.pjdetails, city = y.city, district = y.district, state = y.state, territory = y.territory })
                    .Where(_ => inputDto.ZonalHeadIds.Contains((long)_.StateTrader.ReportingToId) && ((DbFunctions.TruncateTime(_.pjPlans.EffectiveFrom) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                           DbFunctions.TruncateTime(_.pjPlans.EffectiveFrom) <= DbFunctions.TruncateTime(inputDto.ToDate))
                                                                           ||
                                                                           (DbFunctions.TruncateTime(_.pjPlans.EffectiveTo) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                           DbFunctions.TruncateTime(_.pjPlans.EffectiveTo) <= DbFunctions.TruncateTime(inputDto.ToDate))))
                    .AsEnumerable().Select(s => new PermanentCoveragePlanReportOutputDto()
                    {
                        ZonalHeadName = s.zhead.Name,
                        BDOName = s.StateTrader.Name,
                        City = s.pjdetails.TownId > 0 ? City.FirstOrDefault(_ => _.Id == s.pjdetails.TownId).CityName : String.Empty,
                        District = s.pjdetails.DistrictId > 0 ? district.FirstOrDefault(_ => _.Id == s.pjdetails.DistrictId).DistrictName : String.Empty,
                        CityId = s.pjdetails.TownId,
                        CreatedDate = s.pjPlans.CreatedDate,
                        DistrictId = (int)s.pjdetails.DistrictId,
                        EffectiveFrom = s.pjPlans.EffectiveFrom,
                        EffectiveTo = s.pjPlans.EffectiveTo,
                        Year = s.fyear.Year,
                        State = s.state,
                        //Territory = s.territory,
                        NoOfSubDealer = s.pjdetails.NoofSubDealer,
                        NoOfWholeSeller = s.pjdetails.NoOfWholeSeller,
                        NoOfVisit = s.pjdetails.NoOfVisit,
                        Remarks = s.pjdetails.Remarks,
                        InHQNoVisitId = s.pjdetails.InHQNoVisit,
                        //InHQNoVisitName= Utility.GetEnumFromString<DTO.Enums.STPVisitType>(s.pjdetails.InHQNoVisit),
                        InHQNoVisitName = DTO.Enums.STPVisitType.InHQNoVisit.ToString(),
                        PCPNumber = s.pjPlans.PJPNumber

                    }).ToList();
                    }
                }
                var pjp = _emamiContext.PermanentJourneyPlans.AsNoTracking()
                    .Join(_emamiContext.PermanentJourneyPlanDetails.AsNoTracking(), y => y.Id, pd => pd.PermanentJourneyPlanId, (y, pd) => new { pjPlans = y, pjdetails = pd })
                    //.Join(_emamiContext.City.AsNoTracking(), y => y.pjdetails.TownId, c => c.Id, (y, c) => new { pjPlans = y.pjPlans, pjdetails = y.pjdetails, city = c.CityName })
                    //.Join(_emamiContext.District.AsNoTracking(), y => y.pjdetails.DistrictId, d => d.Id, (y, d) => new { pjPlans = y.pjPlans, pjdetails = y.pjdetails, city = y.city, district = d.DistrictName })
                    .Join(_emamiContext.State.AsNoTracking(), y => y.pjdetails.StateId, d => d.Id, (y, d) => new { pjPlans = y.pjPlans, pjdetails = y.pjdetails,/* city = y.city, district = y.district,*/ state = d.StateName })
                    //.Join(_emamiContext.Territory.AsNoTracking(), y => y.pjdetails.TerritoryId, d => d.Id, (y, d) => new { pjPlans = y.pjPlans, pjdetails = y.pjdetails, city = y.city, district = y.district, state = y.state, territory = d.Name })
                    .Join(_emamiContext.FinancialYears.AsNoTracking(), p => p.pjPlans.FinancialYearId, fy => fy.Id, (p, fy) => new { pjPlans = p.pjPlans, pjdetails = p.pjdetails, fyear = fy,/* city = p.city, district = p.district,*/ state = p.state/* territory = p.territory*/ })
                    .Join(_emamiContext.Users.AsNoTracking(), y => y.pjPlans.CreatedBy, StateTrader => StateTrader.Id, (y, StateTrader) => new { pjPlans = y.pjPlans, fyear = y.fyear, StateTrader, pjdetails = y.pjdetails, /*city = y.city, district = y.district,*/ state = y.state/*, territory = y.territory*/ })
                    .Join(_emamiContext.Users.AsNoTracking(), y => y.StateTrader.ReportingToId, zhead => zhead.Id, (y, zhead) => new { pjPlans = y.pjPlans, fyear = y.fyear, StateTrader = y.StateTrader, zhead, pjdetails = y.pjdetails, /*city = y.city, district = y.district,*/ state = y.state/*, territory = y.territory*/ })
                    .Join(_emamiContext.UserDivisionMappings.AsNoTracking(), y => y.StateTrader.Id, ud => ud.UserId, (y, ud) => new { pjPlans = y.pjPlans, fyear = y.fyear, StateTrader = y.StateTrader,zhead=y.zhead,udiv = ud, pjdetails = y.pjdetails/*, city = y.city , district = y.district */, state = y.state/*, territory = y.territory */})
                    .Where(_ => inputDto.BDOIds.Contains(_.StateTrader.Id) && _.udiv.SalesOrganizationId == inputDto.SalesOrganizationId && _.udiv.DistributionChannelId == inputDto.DistributionChannelId && _.udiv.DivisionId == inputDto.VerticalId && (DbFunctions.TruncateTime(_.pjPlans.EffectiveFrom) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                           DbFunctions.TruncateTime(_.pjPlans.EffectiveFrom) <= DbFunctions.TruncateTime(inputDto.ToDate))
                                                                           ||
                                                                           (DbFunctions.TruncateTime(_.pjPlans.EffectiveTo) >= DbFunctions.TruncateTime(inputDto.FromDate) &&
                                                                           DbFunctions.TruncateTime(_.pjPlans.EffectiveTo) <= DbFunctions.TruncateTime(inputDto.ToDate)))
                    .AsEnumerable().Select(s => new PermanentCoveragePlanReportOutputDto()
                    {
                        ZonalHeadName = s.zhead.Name,
                        BDOName = s.StateTrader.Name,
                        City = s.pjdetails.TownId > 0 ? City.FirstOrDefault(_ => _.Id == s.pjdetails.TownId).CityName : String.Empty,
                        District = s.pjdetails.DistrictId > 0 ? district.FirstOrDefault(_ => _.Id == s.pjdetails.DistrictId).DistrictName : String.Empty,
                        CityId = s.pjdetails.TownId,
                        CreatedDate =s.pjPlans.CreatedDate,
                        DistrictId = (int)s.pjdetails.DistrictId,
                        EffectiveFrom=s.pjPlans.EffectiveFrom,
                        EffectiveTo=s.pjPlans.EffectiveTo,
                        Year=s.fyear.Year,
                        State=s.state,
                        //Territory=s.territory,
                        NoOfSubDealer=s.pjdetails.NoofSubDealer,
                        NoOfWholeSeller=s.pjdetails.NoOfWholeSeller,
                        NoOfVisit=s.pjdetails.NoOfVisit,
                        Remarks=s.pjdetails.Remarks,
                        InHQNoVisitId=s.pjdetails.InHQNoVisit,
                        //InHQNoVisitName= Utility.GetEnumFromString<DTO.Enums.STPVisitType>(s.pjdetails.InHQNoVisit),
                        InHQNoVisitName= DTO.Enums.STPVisitType.InHQNoVisit.ToString(),
                        PCPNumber=s.pjPlans.PJPNumber

                    }) ;


               
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = pcpDetailsList;
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }
        #endregion

        #region Pending Sauda Report

        public ResultDto GetPendingSaudaReport(PendingSaudaReportInput inputDto)
        {
            _methodName = "GetPendingSaudaReport";
            var PendingSaudaReportOutput = new List<PendingSaudaReportOutputDto>();
            try
            {
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

                List<long> UserIds = new List<long>();

                var saudaOrdersContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.Sauda != null && DbFunctions.TruncateTime(_.Sauda.BiddingDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                && DbFunctions.TruncateTime(_.Sauda.BiddingDate) <= DbFunctions.TruncateTime(inputDto.ToDate));

                if (saudaOrdersContext != null)
                {
                    if (inputDto.BDOIds != null && inputDto.BDOIds.Any())
                    {
                        UserIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => inputDto.BDOIds.Contains(_.UserId)).Select(_ => _.CustomerId).Distinct().ToList();

                        saudaOrdersContext = saudaOrdersContext.Where(_ => UserIds.Contains(_.Sauda.UserId));
                    }
                    if (inputDto.OilTypeIds != null && inputDto.OilTypeIds.Any())
                    {
                        saudaOrdersContext = saudaOrdersContext.Where(_ => inputDto.OilTypeIds.Contains(_.OilTypeId));
                    }
                    if (inputDto.PlantIds != null && inputDto.PlantIds.Any())
                    {
                        saudaOrdersContext = saudaOrdersContext.Where(_ => inputDto.PlantIds.Contains(_.PlantId));
                    }
                }

                if (saudaOrdersContext != null && saudaOrdersContext.Any())
                {

                    PendingSaudaReportOutput = saudaOrdersContext.Join(_emamiContext.Users.AsNoTracking(), x => x.Sauda.UserId, u => u.Id, (x, u) => new { SaudaOrder = x, PartyName = u.Name, PartyCode = u.Code })
                        .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.SaudaOrder.Sauda.UserId, uc => uc.CustomerId, (x, uc) => new { x.SaudaOrder, x.PartyCode, x.PartyName, BDOId = uc.UserId })
                        .Join(_emamiContext.Users.AsNoTracking(), x => x.BDOId, u => u.Id, (x, u) => new { x.SaudaOrder, x.PartyCode, x.PartyName, BDOName = u.Name, BDOCode = u.Code })
                        .Join(_emamiContext.OilTypes.AsNoTracking(), x => x.SaudaOrder.OilTypeId, ot => ot.Id, (x, ot) => new { x.SaudaOrder, x.PartyCode, x.PartyName, x.BDOCode, x.BDOName, OilType = ot.Name })
                        .Join(_emamiContext.Skus.AsNoTracking(), x => x.SaudaOrder.SkuId, s => s.Id, (x, s) => new { x.SaudaOrder, x.PartyCode, x.PartyName, x.BDOCode, x.BDOName, x.OilType, PackGroupId = s.PackGroupId })
                        .Select(_ => new PendingSaudaReportOutputDto
                        {
                            SaudaOrderId = _.SaudaOrder.Id,
                            BDOName = _.BDOName,
                            PlantName = _emamiContext.Depots.FirstOrDefault(p => p.Id == _.SaudaOrder.PlantId && p.IsPlant && p.IsActive).Name,
                            DealerName = _.PartyName,
                            SaudaNumber = _.SaudaOrder.SaudaNumber,
                            OilType = _.SaudaOrder.OilType.Name,
                            SkuName = _.SaudaOrder.Sku.SkuName,
                            ValidFrom = _.SaudaOrder.ValidFromDate,
                            ValidTo = _.SaudaOrder.ValidToDate,
                            ContractQtyInCase = _.SaudaOrder.BidQuantityCase,
                            ContractQtyInMT = _.SaudaOrder.BidQuantity,
                            SaudaBidPrice = _.SaudaOrder.BidPrice
                        }).ToList();

                    foreach (var item in PendingSaudaReportOutput)
                    {
                        List<SaudaOrderLiftingRequestMapping> orderLiftMappingListContext = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where(_ => _.SaudaOrderId == item.SaudaOrderId
                                && _.StatusId != (int)DTO.Enums.Status.Deleted && _.StatusId != (int)DTO.Enums.Status.Rejected).ToList();
                        if (orderLiftMappingListContext != null && orderLiftMappingListContext.Any())
                        {
                            //Pending orders count
                            item.PendingQtyInCase = item.ContractQtyInCase - orderLiftMappingListContext.Sum(_ => _.LiftingQuantityCase);
                            item.PendingQtyInMT = item.ContractQtyInMT - orderLiftMappingListContext.Sum(_ => _.LiftingQuantity);
                        }
                        else
                        {
                            item.PendingQtyInCase = item.ContractQtyInCase;
                            item.PendingQtyInMT = item.ContractQtyInMT;
                        }
                    }
                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
            return _resultService.SuccessObject(PendingSaudaReportOutput);
        }

        #endregion

        #region PendingContractReport

        public ResultDto GetPendingContractExport(PendingContractReportDto inputDto)
        {
            var pendingContractReportOutputDto = new List<PendingContractReportOutputDto>();

            var outputDto = new PendingContractReportOutputDto();

            _methodName = "GetPendingContractExport";

            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.OilTypeIds == null || !inputDto.OilTypeIds.Any())
                {
                    return _resultService.ErrorMessage(Constants.OilTypeMissing);
                }

                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                IQueryable<UserCustomerMapping> dealersList = null;

                if (userRoleContext != null)
                {
                    dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId);
                }
                else
                {
                    dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => inputDto.BDOIds.Contains(_.UserId)).Distinct();
                }

                var saudaContextList = _emamiContext.Sauda.AsNoTracking().Where(_ => dealersList.Any(a => a.CustomerId == _.UserId)).ToList();

                if (saudaContextList != null && saudaContextList.Any())
                {
                    List<long> saudaContextListIds = saudaContextList.Select(_ => _.Id).ToList();
                    var saudaOrderContextList = saudaContextList.Join(_emamiContext.SaudaOrders.AsNoTracking(), a => a.Id, so => so.SaudaId, (a, so) => new
                    {
                        Sauda = new { SaudaId = a.Id, SaudaUserId = a.UserId, SaudaCreatedDate = a.CreatedDate },
                        SaudaOrders = new { SaudaId = so.SaudaId, SaudaOrderId = so.Id, BidQuantityCase = so.BidQuantityCase, SkuId = so.SkuId, PlantId = so.PlantId, BrokerId = so.BrokerId, Incoterms1 = so.Incoterms1, SkuName = so.Sku.SkuName, SkuCode = so.Sku.SkuCode, SKUId = so.Sku.Id, BidPrice = so.BidPrice, SaudaNumber = so.SaudaNumber, ValidFromDate = so.ValidFromDate, ValidToDate = so.ValidToDate, BidQuantity = so.BidQuantity, VerticalName = so.OilType.Division.Name, SkuQuantity = so.Sku.Quantity, VerticalId = so.OilType.DivisionId, OilType = so.OilType.Name, OilTypeId = so.OilType.Id }
                    })
                        .Where(_ => saudaContextListIds.Contains(_.Sauda.SaudaId)).ToList();

                    if (saudaOrderContextList != null && saudaOrderContextList.Any())
                    {

                        List<long> saudaOrderContextIds = saudaOrderContextList.Select(_ => _.SaudaOrders.SaudaOrderId).ToList();

                        var orderLiftMappingListContext = saudaOrderContextList.Join(_emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking(), so => so.SaudaOrders.SaudaOrderId, sol => sol.SaudaOrderId, (so, sol) =>
                        new
                        {
                            so.SaudaOrders,
                            so.Sauda,
                            SaudaOrderLifting = new { LiftingQty = sol.LiftingQuantity, LiftingQtyCase = sol.LiftingQuantityCase, SaudaOrderid = sol.SaudaOrderId }
                        })
                                .Where(_ => saudaOrderContextIds.Contains(_.SaudaOrders.SaudaOrderId))
                                .Select(s => s)
                                .ToList();

                        List<long> plantIds = orderLiftMappingListContext.Select(_ => _.SaudaOrders.PlantId).ToList();

                        if (orderLiftMappingListContext != null && orderLiftMappingListContext.Any())
                        {
                            var plantDepotMappingContext = orderLiftMappingListContext.Join(_emamiContext.PlantDepotMapping.AsNoTracking(), r => r.SaudaOrders.PlantId, pd => pd.PlantId, (r, pd) =>
                         new { r.Sauda, r.SaudaOrders, r.SaudaOrderLifting, PlantDepot = new { PlantId = pd.PlantId, MappeddepotId = pd.DepotId } })
                                .Where(_ => plantIds.Contains(_.SaudaOrders.PlantId))
                                .Select(s => s).Distinct()
                                .ToList();

                            if (plantDepotMappingContext != null && plantDepotMappingContext.Any())
                            {
                                List<long> depotIds = plantDepotMappingContext.Select(_ => _.PlantDepot.MappeddepotId).ToList();

                                var depotListContext = plantDepotMappingContext.Join(_emamiContext.Depots.AsNoTracking(), a => a.PlantDepot.MappeddepotId, d => d.Id, (a, d) =>
                                  new { a.Sauda, a.SaudaOrders, a.SaudaOrderLifting, a.PlantDepot, Depot = new { DepotId = d.Id, Depotname = d.Name, DepotCode = d.Code/*ZoneId = d.ZoneId, StateId = d.StateId*/  } })
                                         .Where(_ => depotIds.Contains(_.PlantDepot.MappeddepotId) /*&& inputDto.ZoneIds.Contains((Int64)_.Depot.ZoneId)*/)
                                         .Select(s => s).Distinct()
                                         .ToList();

                                if (plantDepotMappingContext != null && plantDepotMappingContext.Any())
                                {
                                    var skuListContext = depotListContext
                                  .Join(_emamiContext.Skus.AsNoTracking(), dl => dl.SaudaOrders.SkuId, sk => sk.Id, (dl, sk) =>
                                  new { dl.Sauda, dl.SaudaOrders, dl.SaudaOrderLifting, dl.PlantDepot, dl.Depot, Sku = new { SkuId = sk.Id, SkuName = sk.SkuName, SkuCode = sk.SkuCode, OilTypeId = sk.OilTypeId, VerticalId = sk.DivisionId } })
                                  .Join(_emamiContext.OilTypes.AsNoTracking(), v => v.SaudaOrders.OilTypeId, ot => ot.Id, (v, ot) =>
                                  new { v.Sauda, v.SaudaOrders, v.SaudaOrderLifting, v.PlantDepot, v.Depot, v.Sku, OilType = new { OilTypeId = ot.Id, OilTypeName = ot.Name } })
                                        .Where(_ => _.Sku.VerticalId == inputDto.VerticalId && inputDto.OilTypeIds.Contains(_.OilType.OilTypeId))
                                         .Select(s => s).Distinct()
                                         .ToList();

                                    if (skuListContext != null && skuListContext.Any())
                                    {
                                        foreach (var item in skuListContext)
                                        {
                                            decimal ReturnedQuantity = 0;
                                            decimal ReturnedQuantityCase = 0;

                                            //decimal tmpReturnedQuantity = 0;
                                            //decimal tmpReturnedQuantityCase = 0;
                                            long id = 0;

                                            var ReturnInvoiceContext = _emamiContext.Invoices.AsNoTracking().Where(_ => item.Sauda.SaudaUserId == _.UserId /*&& _.SalesDocumentType == "ZHCR"*/);

                                            if (ReturnInvoiceContext != null && ReturnInvoiceContext.Any())
                                            {
                                                var ReturnedQtyContext = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => ReturnInvoiceContext.Any(a => a.Id == _.InvoiceId) /*&& saudaOrderContextIds.Contains(_.SaudaOrderId)*/).GroupBy(a => a.InvoiceId)
                                                    //.Select(_=>new {
                                                    //    tmpReturnedQuantity = _.Select(s => s.ActualBilledQuantity).DefaultIfEmpty(0).Sum(),
                                                    //    tmpReturnedQuantityCase = _.Select(s => s.QuantityInCase).DefaultIfEmpty(0).Sum()
                                                    //})
                                                    .ToList();

                                                if (ReturnedQtyContext != null && ReturnedQtyContext.Any())
                                                {
                                                    foreach (var i in ReturnedQtyContext)
                                                    {

                                                        //ReturnedQuantity = i.Sum(_ => _.ActualBilledQuantity);
                                                        //ReturnedQuantityCase = i.Sum(_ => _.QuantityInCase);

                                                        outputDto = new PendingContractReportOutputDto
                                                        {
                                                            PlantName = item.Depot.Depotname,
                                                            PlantCode = item.Depot.DepotCode,
                                                            //State = item.State.StateName,
                                                            CustomerCode = _emamiContext.Users.FirstOrDefault(p => p.Id == item.Sauda.SaudaUserId && p.IsActive).Code,
                                                            CustomerName = _emamiContext.Users.FirstOrDefault(p => p.Id == item.Sauda.SaudaUserId && p.IsActive).Name,
                                                            MaterialCode = item.Sku.SkuCode,
                                                            MaterialDescription = item.Sku.SkuName,
                                                            OilType = item.OilType.OilTypeName,
                                                            //PendingQtyCases = item.SaudaOrders.BidQuantityCase - item.SaudaOrderLifting.LiftingQtyCase + ReturnedQuantityCase,
                                                            PendingQtyCases = 0,
                                                            //PendingQty_MT = item.SaudaOrders.BidQuantity - item.SaudaOrderLifting.LiftingQty + ReturnedQuantity,
                                                            PendingQty_MT = 0,
                                                            BasicRatePerCase = (item.SaudaOrders.BidPrice / item.SaudaOrders.BidQuantityCase),
                                                            IncoTerms = item.SaudaOrders.Incoterms1,
                                                            ContractNo = item.SaudaOrders.SaudaId.ToString(),
                                                            SAPContractNo = item.SaudaOrders.SaudaNumber,
                                                            SaudaDate = item.Sauda.SaudaCreatedDate,
                                                            ContractValidFrom = item.SaudaOrders.ValidFromDate,
                                                            ContractValidTo = item.SaudaOrders.ValidToDate,
                                                            BrokerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(w => item.SaudaOrders.BrokerId == w.Id).Name

                                                        };
                                                        pendingContractReportOutputDto.Add(outputDto);
                                                    }

                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return _resultService.SuccessObject(pendingContractReportOutputDto);

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetPendingContractReport(PendingContractReportDto inputDto)
        {
            _methodName = "GetPendingContractExport";
            var pendingContractReportOutputDto = new List<PendingContractReportOutputDto>();
            var outputDto = new PendingContractReportOutputDto();
            try
            {
                if (inputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }
                if (inputDto.OilTypeIds == null || !inputDto.OilTypeIds.Any())
                {
                    return _resultService.ErrorMessage(Constants.OilTypeMissing);
                }

                var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.RoleId == (int)DTO.Enums.Role.StateTrader);
                IQueryable<UserCustomerMapping> dealersList = null;

                if (userRoleContext != null)
                {
                    dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId);
                }
                else
                {
                    dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => inputDto.BDOIds.Contains(_.UserId)).Distinct();
                }

                var saudaListContext = _emamiContext.Sauda.AsNoTracking()
                        .Join(_emamiContext.SaudaOrders.AsNoTracking(), s => s.Id, so => so.SaudaId, (s, so) => new
                        { /*Sauda = s, SaudaOrders = so*/
                            Sauda = new { SaudaId = s.Id, UserId = s.UserId, SaudaCreatedDate = s.CreatedDate, CreatedBy = s.CreatedBy, BiddingDate = s.BiddingDate },
                            SaudaOrders = new { SaudaId = so.SaudaId, Id = so.Id, BidQuantityCase = so.BidQuantityCase, Incoterms2 = so.Incoterms2, StatusId = so.StatusId, DealerLocationId = so.DealerLocationId, SkuId = so.SkuId, PlantId = so.PlantId, BrokerId = so.BrokerId, Incoterms1 = so.Incoterms1, SkuName = so.Sku.SkuName, SkuCode = so.Sku.SkuCode, SKUId = so.Sku.Id, BidPrice = so.BidPrice, SaudaNumber = so.SaudaNumber, ValidFromDate = so.ValidFromDate, ValidToDate = so.ValidToDate, BidQuantity = so.BidQuantity, VerticalName = so.OilType.Division.Name, SkuQuantity = so.Sku.Quantity, VerticalId = so.OilType.DivisionId, OilType = so.OilType.Name, OilTypeId = so.OilType.Id }
                        })
                        .Join(_emamiContext.ApprovalStatus.AsNoTracking(), x => x.SaudaOrders.StatusId, a => a.Id, (x, a) => new { x.SaudaOrders, x.Sauda, ApprovalStatus = a.Name })
                        .Join(_emamiContext.Depots.AsNoTracking(), x => x.SaudaOrders.PlantId, p => p.Id, (x, p) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, Depots = p.Name })
                        //.Join(_emamiContext.FreightRoutes.AsNoTracking(), x => x.SaudaOrders.DealerLocationId, f => f.Id, (x, f) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, FreightRoutes = f.Name })
                        .Join(_emamiContext.Users.AsNoTracking(), x => x.Sauda.UserId, u => u.Id, (x, u) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, DealerName = u.Name, DealerCode = u.Code, StateId = u.StateId })
                        .Join(_emamiContext.State.AsNoTracking(), x => x.StateId, s => s.Id, (x, s) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.DealerCode, x.StateId, StateName = s.StateName })
                        .Join(_emamiContext.Users.AsNoTracking(), x => x.Sauda.CreatedBy, u => u.Id, (x, u) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, CreatedByName = u.Name, x.DealerCode, x.StateName })
                        .Join(_emamiContext.IncoTerms.AsNoTracking(), x => x.SaudaOrders.Incoterms2, i => i.Id, (x, i) => new { x.SaudaOrders, x.Sauda, x.ApprovalStatus, x.Depots, x.DealerName, x.CreatedByName, IncoTermsName = i.Name, x.DealerCode, x.StateName })
                        .Where(w => dealersList.Any(a => a.CustomerId == w.Sauda.UserId) && inputDto.OilTypeIds.Contains(w.SaudaOrders.OilTypeId)).Distinct()
                       .ToList();

                var SaudaOrderLiftingRequestMapping = _emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().ToList();
                var Invoices = _emamiContext.Invoices.AsNoTracking().ToList();
                var InvoiceDetails = _emamiContext.InvoiceDetails.AsNoTracking().ToList();
                var Users = _emamiContext.Users.AsNoTracking().ToList();
                if (saudaListContext != null && saudaListContext.Any())
                {
                    foreach (var se in saudaListContext)
                    {
                        decimal liftingQuantityIncase = 0;
                        decimal liftingQuantityInMT = 0;
                        decimal ReturnQuantityIncase = 0;
                        decimal ReturnQuantityInMT = 0;
                        var orderLiftMappingListContext = SaudaOrderLiftingRequestMapping.Where(_ => _.SaudaOrderId == se.SaudaOrders.Id && _.StatusId != (int)DTO.Enums.Status.Deleted);
                        if (orderLiftMappingListContext != null && orderLiftMappingListContext.Any())
                        {
                            liftingQuantityIncase = orderLiftMappingListContext.Sum(_ => _.LiftingQuantityCase);
                            liftingQuantityInMT = orderLiftMappingListContext.Sum(_ => _.LiftingQuantity);
                        }

                        var InvoiceListContext = Invoices
                        .Join(InvoiceDetails, I => I.Id, ID => ID.InvoiceId, (I, ID) => new { Invoice = I, InvoiceDetail = ID });
                        //.Where(_ => _.Invoice.SalesDocumentType == "ZHCR" && _.InvoiceDetail.SaudaOrderId == se.SaudaOrders.Id)

                        if (InvoiceListContext != null && InvoiceListContext.Any())
                        {
                            ReturnQuantityIncase = InvoiceListContext.Sum(_ => _.InvoiceDetail.QuantityInCase);
                            ReturnQuantityInMT = InvoiceListContext.Sum(_ => _.InvoiceDetail.ActualBilledQuantity);
                        }

                        outputDto = new PendingContractReportOutputDto();
                        outputDto.PlantName = se.Depots;
                        outputDto.State = se.StateName;
                        outputDto.CustomerCode = se.DealerCode;
                        outputDto.CustomerName = se.DealerName;
                        outputDto.MaterialCode = se.SaudaOrders.SkuCode;
                        outputDto.MaterialDescription = se.SaudaOrders.SkuName;
                        outputDto.OilType = se.SaudaOrders.OilType;
                        outputDto.PendingQtyCases = se.SaudaOrders.BidQuantityCase - liftingQuantityIncase + ReturnQuantityIncase;
                        outputDto.PendingQty_MT = se.SaudaOrders.BidQuantity - liftingQuantityInMT + ReturnQuantityInMT;
                        outputDto.BasicRatePerCase = (se.SaudaOrders.BidQuantityCase > 0 && se.SaudaOrders.BidPrice > 0) ? (se.SaudaOrders.BidPrice / se.SaudaOrders.BidQuantityCase) : 0;
                        outputDto.IncoTerms = se.IncoTermsName;
                        outputDto.ContractNo = se.SaudaOrders.SaudaId.ToString();
                        outputDto.SAPContractNo = se.SaudaOrders.SaudaNumber;
                        outputDto.SaudaDate = se.Sauda.BiddingDate;
                        outputDto.ContractValidFrom = se.SaudaOrders.ValidFromDate;
                        outputDto.ContractValidTo = se.SaudaOrders.ValidToDate;
                        outputDto.BrokerName = se.SaudaOrders.BrokerId > 0 ? Users.FirstOrDefault(w => w.Id == se.SaudaOrders.BrokerId).Name : string.Empty;

                        pendingContractReportOutputDto.Add(outputDto);
                    }
                }

                return _resultService.SuccessObject(pendingContractReportOutputDto);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto GetVerticalId(long userId)
        {
            _methodName = "GetVerticalId";
            PendingContractReportDto outputDto = new PendingContractReportDto();
            try
            {

                var verticalIdContext = _emamiContext.UserRoles.AsNoTracking()
                    .Join(_emamiContext.Users.AsNoTracking(), a => a.UserId, b => b.Id, (a, b) => new { UserId = b.Id, UserRole = new { RoleId = a.RoleId } })
                    .Join(_emamiContext.UserDivisionMappings.AsNoTracking(), a => a.UserId, b => b.UserId, (a, b) => new { UserId = b.UserId, UserRole = new { RoleId = a.UserRole.RoleId }, DivisionId = b.DivisionId })
                    .Where(_ => _.UserId == userId && _.UserRole.RoleId == (int)DTO.Enums.Role.StateTrader).FirstOrDefault();

                if (verticalIdContext != null)
                {
                    outputDto.VerticalId = (long)verticalIdContext.DivisionId;
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

        #region Pending Contract Report for Mobile
        public ResultDto GetOilTypesPendingContractReport(LoginUserIdDto inputDto)
        {
            var skuoutput = new List<OilTypesPendingContractReportDto>();
            var output = new ListSkuandPackGroupDto();
            var verticalList = new List<long>();
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
            try
            {
                var oilTypeContext = new List<OilType>();
                //var packtype = _emamiContext.OilPackingTypes.AsNoTracking().Select(_ => new PackTypeDto()
                //{
                //    Id = _.Id,
                //    Name = _.Name
                //});
                //output.PackGroup.AddRange(packtype);
                IEnumerable<DivisionDetailsDto> divisionslogieduser = _emamiContext.UserDivisionMappings.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId)
            .Select(_ => new DivisionDetailsDto { SalesOrganizationId = _.SalesOrganizationId, DistributionChannelId = _.DistributionChannelId, DivisionId = _.DivisionId });
                
                var roleId = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id).RoleId;
                if (roleId == (int)DTO.Enums.Role.Admin)
                {
                    oilTypeContext = _emamiContext.OilTypes.AsNoTracking().Where(_ => _.IsActive).ToList();
                }
                else
                {
                    //oilTypeContext = _emamiContext.OilTypes.AsNoTracking()
                    //    .Where(_ => 
                    //    _.IsActive).ToList();

                    oilTypeContext = (from ud in _emamiContext.OilTypes.AsNoTracking()
                                      join lud in divisionslogieduser on new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                                                  equals new { SalesOrganizationId = lud.SalesOrganizationId, DistributionChannelId = lud.DistributionChannelId, DivisionId = lud.DivisionId }
                                      select ud ).ToList();
                }
                output.PackGroup = _emamiContext.OilPackingTypes.Where(_ => _.IsActive).Select(_ => new PackTypeDto()
                {
                    Id = _.Id,
                    Name = _.Name
                }).ToList();

                if (oilTypeContext != null && oilTypeContext.Any())
                {
                    var skuContext = _emamiContext.Skus.AsNoTracking().Where(_ => _.IsActive).ToList();
                    foreach (var item in oilTypeContext)
                    {
                        var dto = new OilTypesPendingContractReportDto();
                        dto.OilTypeId = item.Id;
                        dto.OilTypeName = item.Name+" - "+item.SalesOrganization.Code+"/"+item.DistributionChannel.Code+"/"+item.Division.Code;
                        var skudto = skuContext.Where(_ => _.OilTypeId == item.Id).Select(_ => new SkuandPackGroupDto()
                        {
                            SkuId = _.Id,
                            SkuName = _.SkuName,
                            PackGroupId = _.PackGroupId ?? 0,
                            PackGroupName = _.PackGroup.Name
                        });
                        dto.SkuandPackGroup.AddRange(skudto);
                        skuoutput.Add(dto);
                    }
                    output.OilTypesPendingContractReport.AddRange(skuoutput);
                }
                if (output != null)
                {
                    return _resultService.SuccessObject(output);
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

        public ResultDto GetPendingContractReportForMobile(PendingContractReportInputDto inputDto)
        {
            try
            {
                var skuoutputListDto = new List<PendingContractSkuOutputDto>();
                var output = new PendingContractOutputDto();
                output.CurrentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
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
                var role = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
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

                IQueryable<UserCustomerMapping> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == inputDto.LoginUserId);
                //var saudaOrdersContext = new List<PendingContractSkuOutputDto>();
                var saudaStatus = Constants.OverallSaudaStatus;
                var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().AsEnumerable();
                var saudaContext = _emamiContext.Sauda.AsQueryable();
                IEnumerable<PendingContractSkuOutputDto> saudaOrdersContext = new List<PendingContractSkuOutputDto>();
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    var sqlQuery = @"CREATE TABLE #DealerTemp(DealerId BIGINT)
                                        Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                                        insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                                        select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

                                         insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
                                         where UserId=@UserId
                                        select 
                                        u.StateId,
                                        u.Id as UserId,
                                        (Case when pc.ContractValidFrom is null then Cast('0001-01-01T00:00:00' as datetime2) else pc.ContractValidFrom end) as BiddingDate,
                                        pc.SaudaQuantity as QuantityInMT,
                                        sku.Id as SkuId,
                                        pc.PendingQuantityInCase as QuantityInCase,
                                        sku.SkuName as Sku,
                                        u.Name as Dealer,
										pc.BasicRate as Rate,
                                        pc.SaudaNumber as ContractNumber,
                                        (Case when pc.ContractValidTo is null then Cast('0001-01-01T00:00:00' as datetime2) else pc.ContractValidTo end) as ContractValidTo,
                                        (Case when pc.ContractValidFrom is null then Cast('0001-01-01T00:00:00' as datetime2) else pc.ContractValidFrom end) as ContractValidFrom
                                        from PendingContracts pc with(NOLOCK)
                                        join Users u on pc.UserId=u.Id
                                        join Skus sku on pc.MaterialCode=sku.SkuCode and pc.SalesOrgId=sku.SalesOrganizationId
                                        and pc.DistChnlId=sku.DistributionChannelId and pc.DivisionId=sku.DivisionId
                                        join #UserDivision ud on pc.SalesOrgId=ud.SalesOrganizationId 
                                        and pc.DistChnlId=ud.DistributionChannelId and pc.DivisionId=ud.DivisionId
                                        where 
                                        ((@CustomerId > 0 and u.Id=@CustomerId) or u.Id in (Select DealerId from #DealerTemp))
                                        and pc.PendingQuantityInCase > 0.99
                                          drop table #DealerTemp
                                          drop table #UserDivision
                    ";
                    saudaOrdersContext = conn.Query<PendingContractSkuOutputDto>(sqlQuery, new
                    {
                        UserId = inputDto.LoginUserId,
                        CustomerId = inputDto.Id
                    });

                }

                if (inputDto.SkuId != null)
                {
                    saudaOrderContext = saudaOrderContext.Where(_ => inputDto.SkuId.Contains(_.SkuId)).ToList();
                    
                    //saudaOrdersContext = (from pct in _emamiContext.PendingContracts.AsNoTracking()
                    //                      where pct.PendingQuantityInCase !=0 select pct into pc
                    //                      join u in _emamiContext.Users.AsNoTracking() on pc.UserId equals u.Id
                    //                      join sku in _emamiContext.Skus.AsNoTracking() on pc.MaterialCode equals sku.SkuCode where  pc.SalesOrgId == sku.SalesOrganizationId && pc.DistChnlId == sku.DistributionChannelId && pc.DivisionId == sku.DivisionId
                    //                      //join sauda in _emamiContext.Sauda.AsNoTracking() on pc.SaudaNumber equals sauda.SaudaNumber
                    //                      join ud in divisionslogieduser on new { SalesOrganizationId = pc.SalesOrgId, DistributionChannelId = pc.DistChnlId, DivisionId = pc.DivisionId } equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                    //                      where 
                    //                      //DbFunctions.TruncateTime(pc.ContractValidTo) >= DbFunctions.TruncateTime(output.CurrentDate) &&
                    //                       (inputDto.Id > 0 && u.Id == inputDto.Id) || (dealersList.Any(_ => _.CustomerId==u.Id)) 
                    //                      //&& (sauda.BdoId==inputDto.LoginUserId || sauda.BdoId==0)                                         
                    //                      select new PendingContractSkuOutputDto
                    //                      {
                    //                          UserId = u.Id,
                    //                          BiddingDate = saudaContext.Where(s => s.SaudaNumber == pc.SaudaNumber).FirstOrDefault() != null
                    //                             && saudaContext.Where(s => s.SaudaNumber == pc.SaudaNumber).FirstOrDefault().SaudaNumber != null ? saudaContext.Where(s => s.SaudaNumber == pc.SaudaNumber).FirstOrDefault().BiddingDate : DateTime.Now,
                    //                          QuantityInMT = pc.SaudaQuantity,
                    //                          SkuId = sku.Id,
                    //                          QuantityInCase = pc.PendingQuantityInCase,
                    //                          Sku = sku.SkuName,
                    //                          Dealer = u.Name,
                    //                          Rate = pc.BasicRate,
                    //                          ContractNumber=pc.SaudaNumber,
                    //                          ContractValidFrom = saudaOrderContext.FirstOrDefault(_ => _.SaudaId == saudaContext.FirstOrDefault(sauda => sauda.SaudaNumber == pc.SaudaNumber).Id) !=null ? saudaOrderContext.FirstOrDefault(_ => _.SaudaId == saudaContext.FirstOrDefault(sauda => sauda.SaudaNumber == pc.SaudaNumber).Id).ValidFromDate:DateTime.MinValue,
                    //                          ContractValidTo = saudaOrderContext.FirstOrDefault(_ => _.SaudaId == saudaContext.FirstOrDefault(sauda => sauda.SaudaNumber == pc.SaudaNumber).Id) != null ? saudaOrderContext.FirstOrDefault(_ => _.SaudaId == saudaContext.FirstOrDefault(sauda => sauda.SaudaNumber == pc.SaudaNumber).Id).ValidToDate:DateTime.MinValue,

                    //                      }).ToList();
                 }
                //else
                //{
                //    saudaOrdersContext = (from pct in _emamiContext.PendingContracts.AsNoTracking()
                //                          where pct.PendingQuantityInCase != 0
                //                          select pct into pc
                //                          join u in _emamiContext.Users.AsNoTracking() on pc.UserId equals u.Id
                //                          join sku in _emamiContext.Skus.AsNoTracking() on pc.MaterialCode equals sku.SkuCode
                //                         // join sauda in _emamiContext.Sauda.AsNoTracking() on pc.SaudaNumber equals sauda.SaudaNumber
                //                          join ud in divisionslogieduser on new { SalesOrganizationId = pc.SalesOrgId, DistributionChannelId = pc.DistChnlId, DivisionId = pc.DivisionId } equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                //                          where pc.PendingQuantityInCase != 0 && ((inputDto.Id > 0 && u.Id == inputDto.Id) || (dealersList.Any(a => a.CustomerId == u.Id)))
                //                          //&& (sauda.BdoId==inputDto.LoginUserId || sauda.BdoId==0)   
                //                          //&& u.DivisionId == userContext.DivisionId
                //                          //&& sku.DivisionId == u.DivisionId
                //                          //&& DbFunctions.TruncateTime(pc.ContractValidTo) >= DbFunctions.TruncateTime(output.CurrentDate)
                //                          && pc.SalesOrgId == sku.SalesOrganizationId && pc.DistChnlId == sku.DistributionChannelId
                //                          && pc.DivisionId == sku.DivisionId
                //                          && (inputDto.SkuId.Contains(sku.Id))
                //                          select new PendingContractSkuOutputDto
                //                          {
                //                              UserId = u.Id,
                //                              BiddingDate = saudaContext.Where(s => s.SaudaNumber == pc.SaudaNumber).FirstOrDefault() != null
                //                                 && saudaContext.Where(s => s.SaudaNumber == pc.SaudaNumber).FirstOrDefault().SaudaNumber != null ? saudaContext.Where(s => s.SaudaNumber == pc.SaudaNumber).FirstOrDefault().BiddingDate : DateTime.Now,
                //                              QuantityInMT = pc.SaudaQuantity,
                //                              SkuId = sku.Id,
                //                              QuantityInCase = pc.PendingQuantityInCase,
                //                              Sku = sku.SkuName ,
                //                              Dealer = u.Name,
                //                              Rate = pc.BasicRate,
                //                              ContractNumber=pc.SaudaNumber,
                //                              ContractValidFrom = saudaOrderContext.FirstOrDefault(_ => _.SaudaId == saudaContext.FirstOrDefault(sauda => sauda.SaudaNumber == pc.SaudaNumber).Id) != null ? saudaOrderContext.FirstOrDefault(_ => _.SaudaId == saudaContext.FirstOrDefault(sauda => sauda.SaudaNumber == pc.SaudaNumber).Id).ValidFromDate : DateTime.MinValue,
                //                              ContractValidTo = saudaOrderContext.FirstOrDefault(_ => _.SaudaId == saudaContext.FirstOrDefault(sauda => sauda.SaudaNumber == pc.SaudaNumber).Id) != null ? saudaOrderContext.FirstOrDefault(_ => _.SaudaId == saudaContext.FirstOrDefault(sauda => sauda.SaudaNumber == pc.SaudaNumber).Id).ValidToDate : DateTime.MinValue,

                //                          }).ToList();
                //}

                if (saudaOrdersContext != null && saudaOrdersContext.Any())
                {
                    output.PendingDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    output.TotalQuantityInCase = saudaOrdersContext.Sum(_ => _.QuantityInCase);
                    output.TotalQuantityInMT = saudaOrdersContext.Sum(_ => _.QuantityInMT);




                    var dealergroupby = saudaOrdersContext.OrderByDescending(a => a.BiddingDate).GroupBy(g => g.UserId).Select(a => new
                    {
                        dealer = a.Key,
                        skulist = a.ToList()
                    }).ToList();



                    //saudaOrdersContext = saudaOrdersContext.GroupBy(s => s.SkuId)

                    foreach (var data in dealergroupby)
                    {
                        var item = new PendingContractDealerOutputDto()
                        {
                            DealerId = data.dealer,
                            Dealer = data.skulist.FirstOrDefault().Dealer,
                            PendingContractSkuOutput = data.skulist.GroupBy(s => s.SkuId).Select(s => new PendingContractSkuOutputDto()
                            {
                                SkuId = s.Key,
                                Sku = s.FirstOrDefault().Sku,
                                PendingContractSkuDetails = s.Select(sku => new PendingContractSkuDetailsDto()
                                {
                                    UserId = sku.UserId,
                                    BiddingDate = sku.BiddingDate,
                                    QuantityInMT = sku.QuantityInMT,
                                    QuantityInCase = sku.QuantityInCase,
                                    Dealer = sku.Dealer,
                                    Rate = sku.Rate,
                                    ContractNumber = sku.ContractNumber,
                                    ContractValidFrom = sku.ContractValidFrom,
                                    ContractValidTo = sku.ContractValidTo
                                }).ToList(),
                            }).ToList()

                        };

                        output.PendingContractDealerOutput.Add(item);
                    }
                }
                

                if (output != null)
                {
                    return _resultService.SuccessObject(output);
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
        #region Sales Report
        public ResultDto GetPendingSalesReport(PendingSaudaReportInput inputDto)
        {
            _methodName = "GetPendingSalesReport";
            var PendingSaudaReportOutput = new List<PendingSaudaReportOutputDto>();
            try
            {
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

                List<long> UserIds = new List<long>();

                var invoiceContext = _emamiContext.InvoiceDetails.AsNoTracking().Where(_ => _.Invoice != null && DbFunctions.TruncateTime(_.Invoice.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
                && DbFunctions.TruncateTime(_.Invoice.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate));

                if (invoiceContext != null)
                {
                    if (inputDto.BDOIds != null && inputDto.BDOIds.Any())
                    {
                        UserIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => inputDto.BDOIds.Contains(_.UserId)).Select(_ => _.CustomerId).Distinct().ToList();

                        invoiceContext = invoiceContext.Where(_ => UserIds.Contains(_.Invoice.UserId));
                    }
                    if (inputDto.OilTypeIds != null && inputDto.OilTypeIds.Any())
                    {
                        invoiceContext = invoiceContext.Where(_ => inputDto.OilTypeIds.Contains(_.OilTypeId));
                    }
                }

                if (invoiceContext != null && invoiceContext.Any())
                {

                    PendingSaudaReportOutput = invoiceContext.Join(_emamiContext.Users.AsNoTracking(), x => x.Invoice.UserId, u => u.Id, (x, u) => new { Invoice = x, PartyName = u.Name, PartyCode = u.Code })
                    .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.Invoice.Invoice.UserId, uc => uc.CustomerId, (x, uc) => new { x.Invoice, x.PartyCode, x.PartyName, BDOId = uc.UserId })
                    .Join(_emamiContext.Users.AsNoTracking(), x => x.BDOId, u => u.Id, (x, u) => new { x.Invoice, x.PartyCode, x.PartyName, BDOName = u.Name, BDOCode = u.Code })
                    .Join(_emamiContext.OilTypes.AsNoTracking(), x => x.Invoice.OilTypeId, ot => ot.Id, (x, ot) => new { x.Invoice, x.PartyCode, x.PartyName, x.BDOCode, x.BDOName, OilType = ot.Name })
                    .Join(_emamiContext.Skus.AsNoTracking(), x => x.Invoice.SkuId, s => s.Id, (x, s) => new { x.Invoice, x.PartyCode, x.PartyName, x.BDOCode, x.BDOName, x.OilType, PackGroupId = s.PackGroupId })
                    .Select(_ => new PendingSaudaReportOutputDto
                    {
                        SaudaOrderId = _.Invoice.Id,
                        BDOName = _.BDOName,
                        //PlantName = emamiContext.Depots.FirstOrDefault(p => p.Id == .SaudaOrder.PlantId && p.IsPlant && p.IsActive).Name,
                        DealerName = _.PartyName,
                        SaudaNumber = _.Invoice.Invoice.BillingDocument,
                        OilType = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(o => o.Id == _.Invoice.OilTypeId).Name,
                        SkuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(o => o.Id == _.Invoice.SkuId).SkuName,
                        //ValidFrom = _.Invoice.Invoice.InvoiceDate,
                        //ValidTo = _.Invoice.Invoice.InvoiceDueDate,
                        ContractQtyInCase = _.Invoice.QuantityInCase,
                        ContractQtyInMT = _.Invoice.ActualBilledQuantity,
                       // SaudaBidPrice = _.Invoice.Invoice.NetValue
                    }).ToList();

                    //foreach (var item in PendingSaudaReportOutput)
                    //{
                    // List<SaudaOrderLiftingRequestMapping> orderLiftMappingListContext = emamiContext.SaudaOrderLiftingRequestMapping.AsNoTracking().Where( => _.SaudaOrderId == item.SaudaOrderId
                    // && .StatusId != (int)DTO.Enums.Status.Deleted && .StatusId != (int)DTO.Enums.Status.Rejected).ToList();
                    // if (orderLiftMappingListContext != null && orderLiftMappingListContext.Any())
                    // {
                    // //Pending orders count
                    // item.PendingQtyInCase = item.ContractQtyInCase - orderLiftMappingListContext.Sum( => .LiftingQuantityCase);
                    // item.PendingQtyInMT = item.ContractQtyInMT - orderLiftMappingListContext.Sum( => .LiftingQuantity);
                    // }
                    // else
                    // {
                    // item.PendingQtyInCase = item.ContractQtyInCase;
                    // item.PendingQtyInMT = item.ContractQtyInMT;
                    // }
                    //}
                }

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
            return _resultService.SuccessObject(PendingSaudaReportOutput);
        }

        public ResultDto GetPendingContractReportForManager(PendingContractReportInputDto inputDto)
        {
            try
            {
                var skuoutputListDto = new List<PendingContractSkuOutputDto>();
                var output = new PendingContractOutputDto();
                List<long> bdoList = new List<long>();
                output.CurrentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
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

                if (inputDto.Id > 0)
                {
                    bdoList.Add(inputDto.Id);
                }
                else
                {
                    //New Reporting to table change
                    //bdoList = _emamiContext.Users.AsNoTracking().Where(_ => _.ReportingToId == inputDto.LoginUserId).Select(_ => _.Id).ToList();
                    bdoList = _emamiContext.UserReportingToMappings.AsNoTracking().Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                }

                IQueryable<long> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(s => s.CustomerId);
                var saudaOrdersContext = new List<PendingContractSkuOutputDto>();
                var saudaStatus = Constants.OverallSaudaStatus;
                var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().AsEnumerable();

                var role = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                long roleId = 0;
                if (role != null)
                {
                    roleId = role.RoleId;
                }
                else
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
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
                IEnumerable<PendingContractSkuOutputDto> saudaListContext = new List<PendingContractSkuOutputDto>();
                using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                {

                    var sqlQuery = @"CREATE TABLE #DealerTemp(DealerId BIGINT)
                                        CREATE TABLE #BdoTemp(BdoId BIGINT)
                                        Create Table #UserDivision(SalesOrganizationId bigint,DistributionChannelId bigint,DivisionId bigint)
                                        insert into #UserDivision(SalesOrganizationId,DistributionChannelId,DivisionId) 
                                        select SalesOrganizationId,DistributionChannelId,DivisionId from UserDivisionMappings where UserId=@UserId

                                        if(@BdoId>0)
                                        begin
                                        insert into #BdoTemp(BdoId) select @BdoId
                                         insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
                                         where UserId in (select BdoId from #BdoTemp)
                                        end
                                        else
                                        begin
                                        insert into #BdoTemp(BdoId) select UserId from UserReportingToMappings where ReportingToUserId=@UserId
                                         insert into #DealerTemp(DealerId) select CustomerId from UserCustomerMappings 
                                         where UserId in (select BdoId from #BdoTemp)
                                        end

                                        select 
                                        u.StateId,
                                        u.Id as UserId,
                                        (Case when pc.ContractValidFrom is null then Cast('0001-01-01T00:00:00' as datetime2) else pc.ContractValidFrom end) as BiddingDate,
                                        pc.SaudaQuantity as QuantityInMT,
                                        sku.Id as SkuId,
                                        pc.PendingQuantityInCase as QuantityInCase,
                                        (sku.SkuName+' - '+pc.SaudaNumber) as Sku,
                                        u.Name as Dealer,
                                        pc.SaudaNumber as ContractNumber,
                                        (Case when pc.ContractValidTo is null then Cast('0001-01-01T00:00:00' as datetime2) else pc.ContractValidTo end) as ContractValidTo,
                                        (Case when pc.ContractValidFrom is null then Cast('0001-01-01T00:00:00' as datetime2) else pc.ContractValidFrom end) as ContractValidFrom
                                        from PendingContracts pc 
                                        join Users u on pc.UserId=u.Id
                                        join Skus sku on pc.MaterialCode=sku.SkuCode and pc.SalesOrgId=sku.SalesOrganizationId
                                        and pc.DistChnlId=sku.DistributionChannelId and pc.DivisionId=sku.DivisionId
                                        join #UserDivision ud on pc.SalesOrgId=ud.SalesOrganizationId 
                                        and pc.DistChnlId=ud.DistributionChannelId and pc.DivisionId=ud.DivisionId
                                        where 
                                        u.Id in (Select DealerId from #DealerTemp)
                                        and pc.PendingQuantityInCase > 0.99
                                          drop table #DealerTemp
                                          drop table #BdoTemp
                                          drop table #UserDivision
                    ";
                    saudaListContext = conn.Query<PendingContractSkuOutputDto>(sqlQuery, new
                    {
                        UserId = inputDto.LoginUserId,
                        BdoId = inputDto.Id
                    });

                }
                var saudaContext = _emamiContext.Sauda.AsNoTracking();
                if ((inputDto.SkuId == null && inputDto.StateIds == null))
                {

                   
                    inputDto.SkuId = _emamiContext.Skus.AsNoTracking().Where(_ => _.IsActive).Select(s => s.Id).ToList();
                    var StateIds = _emamiContext.State.AsNoTracking().Where(_ => _.IsActive).Select(s => s.Id).ToList();
                    inputDto.StateIds = StateIds.Select(a => (long)a).ToList();

                    saudaOrdersContext = (from pct in saudaListContext
                                          where inputDto.SkuId.Contains(pct.SkuId)
                                          && inputDto.StateIds.Contains(pct.StateId)
                                          select pct
                                         ).ToList();

                    //saudaOrdersContext = (from pct in _emamiContext.PendingContracts.AsNoTracking()
                    //                      where pct.PendingQuantityInCase !=0 select pct into p
                    //                    join u in _emamiContext.Users.AsNoTracking() on p.UserId equals u.Id
                    //                    join sku in _emamiContext.Skus.AsNoTracking() on p.MaterialCode equals sku.SkuCode
                    //                   // join sauda in _emamiContext.Sauda.AsNoTracking() on p.SaudaNumber equals sauda.SaudaNumber
                    //                    join ud in divisionslogieduser on new { SalesOrganizationId=p.SalesOrgId, DistributionChannelId=p.DistChnlId, DivisionId=p.DivisionId } equals new { SalesOrganizationId=ud.SalesOrganizationId, DistributionChannelId=ud.DistributionChannelId, DivisionId=ud.DivisionId }
                    //                    where dealersList.Contains(u.Id)
                    //                    && inputDto.SkuId.Contains(sku.Id)
                    //                    && inputDto.StateIds.Contains(u.StateId)
                    //                     //&& DbFunctions.TruncateTime(p.ContractValidTo) >= DbFunctions.TruncateTime(output.CurrentDate)
                    //                    && p.SalesOrgId == sku.SalesOrganizationId && p.DistChnlId == sku.DistributionChannelId
                    //                    && p.DivisionId == sku.DivisionId
                    //                    //&& (bdoList.Contains(sauda.BdoId) || sauda.BdoId==0)
                    //                    select new PendingContractSkuOutputDto()
                    //                    {
                    //                        UserId = u.Id,
                    //                        BiddingDate = saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber) != null ? saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber).BiddingDate : DateTime.MinValue,
                    //                        QuantityInMT = p.SaudaQuantity,
                    //                        SkuId = sku.Id,
                    //                        QuantityInCase = p.PendingQuantityInCase,
                    //                        Sku = sku.SkuName + " - " + p.SaudaNumber,
                    //                        Dealer = u.Name,
                    //                        ContractNumber = p.SaudaNumber,
                    //                        ContractValidFrom = saudaOrderContext.FirstOrDefault(so => so.SaudaId == saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber).Id) != null ? saudaOrderContext.FirstOrDefault(so => so.SaudaId == saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber).Id).ValidFromDate : DateTime.MinValue,
                    //                        ContractValidTo = saudaOrderContext.FirstOrDefault(so => so.SaudaId == saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber).Id) != null ? saudaOrderContext.FirstOrDefault(so => so.SaudaId == saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber).Id).ValidToDate : DateTime.MinValue,
                    //                    }

                    //                  ).ToList(); 


                    //saudaOrdersContext = _emamiContext.PendingContracts.AsNoTracking().Join(_emamiContext.Users.AsNoTracking(), p => p.UserId, u => u.Id, (p, u) => new { p, u })
                    //                     .Join(_emamiContext.Skus.AsNoTracking(), x => x.p.MaterialCode, sku => sku.SkuCode, (x, sku) => new { x, sku })
                    //                     .Join(_emamiContext.Sauda.AsNoTracking(), a => a.x.p.SaudaNumber, sauda => sauda.SaudaNumber, (a, sauda) => new { a, sauda }).
                    //                     Where(_ => _.a.x.p.PendingQuantityInCase != 0 && dealersList.Contains(_.a.x.u.Id)
                    //                       && (inputDto.SkuId.Contains(_.a.sku.Id))
                    //                      && (inputDto.StateIds.Contains(_.a.x.u.StateId))
                    //                      && _.a.x.p.SalesOrgId == _.a.sku.SalesOrganizationId && _.a.x.p.DistChnlId == _.a.sku.DistributionChannelId
                    //                      && _.a.x.p.DivisionId == _.a.sku.DivisionId).Select(s => new PendingContractSkuOutputDto()
                    //                      {
                    //                          UserId = s.a.x.u.Id,
                    //                          BiddingDate = s.sauda.BiddingDate,
                    //                          QuantityInMT = s.a.x.p.SaudaQuantity,
                    //                          SkuId = s.a.sku.Id,
                    //                          QuantityInCase = s.a.x.p.PendingQuantityInCase,
                    //                          Sku = s.a.sku.SkuName + " - " + s.a.x.p.SaudaNumber,
                    //                          Dealer = s.a.x.u.Name,
                    //                          ContractNumber = s.a.x.p.SaudaNumber,
                    //                          ContractValidFrom = saudaOrderContext.FirstOrDefault(so => so.SaudaId == s.sauda.Id)!=null ? saudaOrderContext.FirstOrDefault(so => so.SaudaId == s.sauda.Id).ValidFromDate:DateTime.MinValue,
                    //                          ContractValidTo = saudaOrderContext.FirstOrDefault(so => so.SaudaId == s.sauda.Id)!=null ? saudaOrderContext.FirstOrDefault(so => so.SaudaId == s.sauda.Id).ValidToDate: DateTime.MinValue,
                    //                      }).ToList();              
                }
                else if (inputDto.SkuId != null && inputDto.StateIds == null)
                {
                    saudaOrdersContext = (from pct in saudaListContext
                                          where inputDto.SkuId.Contains(pct.SkuId)
                                          select pct
                                         ).ToList();
                    //saudaOrdersContext = (from pct in _emamiContext.PendingContracts.AsNoTracking()
                    //                      where pct.PendingQuantityInCase !=0 select pct into p
                    //                    join u in _emamiContext.Users.AsNoTracking() on p.UserId equals u.Id
                    //                    join sku in _emamiContext.Skus.AsNoTracking() on p.MaterialCode equals sku.SkuCode
                    //                   // join sauda in _emamiContext.Sauda.AsNoTracking() on p.SaudaNumber equals sauda.SaudaNumber
                    //                      join ud in divisionslogieduser on new { SalesOrganizationId = p.SalesOrgId, DistributionChannelId = p.DistChnlId, DivisionId = p.DivisionId } equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                    //                      where  dealersList.Contains(u.Id)
                    //                    && inputDto.SkuId.Contains(sku.Id)
                    //                    && p.SalesOrgId == sku.SalesOrganizationId 
                    //                    && p.DistChnlId == sku.DistributionChannelId
                    //                    && p.DivisionId == sku.DivisionId
                    //                     //&& DbFunctions.TruncateTime(p.ContractValidTo) >= DbFunctions.TruncateTime(output.CurrentDate)
                    //                      //&& (bdoList.Contains(sauda.BdoId) || sauda.BdoId==0)
                    //                      select new PendingContractSkuOutputDto()
                    //                    {
                    //                          UserId = u.Id,
                    //                          BiddingDate = saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber) != null ? saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber).BiddingDate : DateTime.MinValue,
                    //                          QuantityInMT = p.SaudaQuantity,
                    //                          SkuId = sku.Id,
                    //                          QuantityInCase = p.PendingQuantityInCase,
                    //                          Sku = sku.SkuName + " - " + p.SaudaNumber,
                    //                          Dealer = u.Name,
                    //                          ContractNumber = p.SaudaNumber,
                    //                          ContractValidFrom = saudaOrderContext.FirstOrDefault(so => so.SaudaId == saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber).Id) != null ? saudaOrderContext.FirstOrDefault(so => so.SaudaId == saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber).Id).ValidFromDate : DateTime.MinValue,
                    //                          ContractValidTo = saudaOrderContext.FirstOrDefault(so => so.SaudaId == saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber).Id) != null ? saudaOrderContext.FirstOrDefault(so => so.SaudaId == saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber).Id).ValidToDate : DateTime.MinValue,
                    //                      }

                    //                  ).ToList();


                    //saudaOrdersContext = _emamiContext.PendingContracts.AsNoTracking().Join(_emamiContext.Users.AsNoTracking(), p => p.UserId, u => u.Id, (p, u) => new { p, u })
                    //                    .Join(_emamiContext.Skus.AsNoTracking(), x => x.p.MaterialCode, sku => sku.SkuCode, (x, sku) => new { x, sku })
                    //                    .Join(_emamiContext.Sauda.AsNoTracking(), a => a.x.p.SaudaNumber, sauda => sauda.SaudaNumber, (a, sauda) => new { a, sauda }).
                    //                    Where(_ => _.a.x.p.PendingQuantityInCase != 0 && dealersList.Contains(_.a.x.u.Id)
                    //                      && (inputDto.SkuId.Contains(_.a.sku.Id))
                    //                     && _.a.x.p.SalesOrgId == _.a.sku.SalesOrganizationId && _.a.x.p.DistChnlId == _.a.sku.DistributionChannelId
                    //                     && _.a.x.p.DivisionId == _.a.sku.DivisionId).Select(s => new PendingContractSkuOutputDto()
                    //                     {
                    //                         UserId = s.a.x.u.Id,
                    //                         BiddingDate = s.sauda.BiddingDate,
                    //                         QuantityInMT = s.a.x.p.SaudaQuantity,
                    //                         SkuId = s.a.sku.Id,
                    //                         QuantityInCase = s.a.x.p.PendingQuantityInCase,
                    //                         Sku = s.a.sku.SkuName + " - " + s.a.x.p.SaudaNumber,
                    //                         Dealer = s.a.x.u.Name,
                    //                         ContractNumber = s.a.x.p.SaudaNumber,
                    //                         ContractValidFrom = saudaOrderContext.FirstOrDefault(so => so.SaudaId == s.sauda.Id) != null ? saudaOrderContext.FirstOrDefault(so => so.SaudaId == s.sauda.Id).ValidFromDate : DateTime.MinValue,
                    //                         ContractValidTo = saudaOrderContext.FirstOrDefault(so => so.SaudaId == s.sauda.Id) != null ? saudaOrderContext.FirstOrDefault(so => so.SaudaId == s.sauda.Id).ValidToDate : DateTime.MinValue,
                    //                     }).ToList();
                }
                else if (inputDto.StateIds != null && inputDto.SkuId == null)
                {

                    saudaOrdersContext = (from pct in saudaListContext
                                          where inputDto.StateIds.Contains(pct.StateId)
                                          select pct
                                         ).ToList();

                    //saudaOrdersContext = (from pct in _emamiContext.PendingContracts.AsNoTracking()
                    //                      where pct.PendingQuantityInCase!=0 select pct into p
                    //                    join u in _emamiContext.Users.AsNoTracking() on p.UserId equals u.Id
                    //                    join sku in _emamiContext.Skus.AsNoTracking() on p.MaterialCode equals sku.SkuCode
                    //                    //join sauda in _emamiContext.Sauda.AsNoTracking() on p.SaudaNumber equals sauda.SaudaNumber
                    //                      join ud in divisionslogieduser on new { SalesOrganizationId = p.SalesOrgId, DistributionChannelId = p.DistChnlId, DivisionId = p.DivisionId } equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                    //                      where  
                    //                      //DbFunctions.TruncateTime(p.ContractValidTo) >= DbFunctions.TruncateTime(output.CurrentDate) &&
                    //                     dealersList.Contains(u.Id)
                    //                    && inputDto.StateIds.Contains(u.StateId)
                    //                    && p.SalesOrgId == sku.SalesOrganizationId && p.DistChnlId == sku.DistributionChannelId
                    //                    && p.DivisionId == sku.DivisionId
                    //                      //&& (bdoList.Contains(sauda.BdoId) || sauda.BdoId==0)
                    //                      select new PendingContractSkuOutputDto()
                    //                    {
                    //                          UserId = u.Id,
                    //                          BiddingDate = saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber) != null ? saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber).BiddingDate : DateTime.MinValue,
                    //                          QuantityInMT = p.SaudaQuantity,
                    //                          SkuId = sku.Id,
                    //                          QuantityInCase = p.PendingQuantityInCase,
                    //                          Sku = sku.SkuName + " - " + p.SaudaNumber,
                    //                          Dealer = u.Name,
                    //                          ContractNumber = p.SaudaNumber,
                    //                          ContractValidFrom = saudaOrderContext.FirstOrDefault(so => so.SaudaId == saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber).Id) != null ? saudaOrderContext.FirstOrDefault(so => so.SaudaId == saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber).Id).ValidFromDate : DateTime.MinValue,
                    //                          ContractValidTo = saudaOrderContext.FirstOrDefault(so => so.SaudaId == saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber).Id) != null ? saudaOrderContext.FirstOrDefault(so => so.SaudaId == saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber).Id).ValidToDate : DateTime.MinValue,
                    //                      }

                    //                  ).ToList();


                    //saudaOrdersContext = _emamiContext.PendingContracts.AsNoTracking().Join(_emamiContext.Users.AsNoTracking(), p => p.UserId, u => u.Id, (p, u) => new { p, u })
                    //                    .Join(_emamiContext.Skus.AsNoTracking(), x => x.p.MaterialCode, sku => sku.SkuCode, (x, sku) => new { x, sku })
                    //                    .Join(_emamiContext.Sauda.AsNoTracking(), a => a.x.p.SaudaNumber, sauda => sauda.SaudaNumber, (a, sauda) => new { a, sauda }).
                    //                    Where(_ => _.a.x.p.PendingQuantityInCase != 0 && dealersList.Contains( _.a.x.u.Id)
                    //                     && (inputDto.StateIds.Contains(_.a.x.u.StateId))
                    //                     && _.a.x.p.SalesOrgId == _.a.sku.SalesOrganizationId && _.a.x.p.DistChnlId == _.a.sku.DistributionChannelId
                    //                     && _.a.x.p.DivisionId == _.a.sku.DivisionId).Select(s => new PendingContractSkuOutputDto()
                    //                     {
                    //                         UserId = s.a.x.u.Id,
                    //                         BiddingDate = s.sauda.BiddingDate,
                    //                         QuantityInMT = s.a.x.p.SaudaQuantity,
                    //                         SkuId = s.a.sku.Id,
                    //                         QuantityInCase = s.a.x.p.PendingQuantityInCase,
                    //                         Sku = s.a.sku.SkuName + " - " + s.a.x.p.SaudaNumber,
                    //                         Dealer = s.a.x.u.Name,
                    //                         ContractNumber = s.a.x.p.SaudaNumber,
                    //                         ContractValidFrom = saudaOrderContext.FirstOrDefault(so => so.SaudaId == s.sauda.Id) != null ? saudaOrderContext.FirstOrDefault(so => so.SaudaId == s.sauda.Id).ValidFromDate : DateTime.MinValue,
                    //                         ContractValidTo = saudaOrderContext.FirstOrDefault(so => so.SaudaId == s.sauda.Id) != null ? saudaOrderContext.FirstOrDefault(so => so.SaudaId == s.sauda.Id).ValidToDate : DateTime.MinValue,
                    //                     }).ToList();

                }
                else
                {
                    saudaOrdersContext = (from pct in saudaListContext
                                         where inputDto.SkuId.Contains(pct.SkuId)
                                         && inputDto.StateIds.Contains(pct.StateId)
                                         select pct
                                         ).ToList();
                    //saudaOrdersContext = (from pct in _emamiContext.PendingContracts.AsNoTracking()
                    //                      where pct.PendingQuantityInCase!=0 select pct into p
                    //                    join u in _emamiContext.Users.AsNoTracking() on p.UserId equals u.Id
                    //                    join sku in _emamiContext.Skus.AsNoTracking() on p.MaterialCode equals sku.SkuCode
                    //                    //join sauda in _emamiContext.Sauda.AsNoTracking() on p.SaudaNumber equals sauda.SaudaNumber
                    //                      join ud in divisionslogieduser on new { SalesOrganizationId = p.SalesOrgId, DistributionChannelId = p.DistChnlId, DivisionId = p.DivisionId } equals new { SalesOrganizationId = ud.SalesOrganizationId, DistributionChannelId = ud.DistributionChannelId, DivisionId = ud.DivisionId }
                    //                      where dealersList.Contains(u.Id)
                    //                    && inputDto.SkuId.Contains(sku.Id)
                    //                    && inputDto.StateIds.Contains(u.StateId)
                    //                     //&& DbFunctions.TruncateTime(p.ContractValidTo) >= DbFunctions.TruncateTime(output.CurrentDate)
                    //                    && p.SalesOrgId == sku.SalesOrganizationId && p.DistChnlId == sku.DistributionChannelId
                    //                    && p.DivisionId == sku.DivisionId
                    //                      //&& (bdoList.Contains(sauda.BdoId) || sauda.BdoId==0)
                    //                      select new PendingContractSkuOutputDto()
                    //                    {
                    //                          UserId = u.Id,
                    //                          BiddingDate = saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber) != null ? saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber).BiddingDate : DateTime.MinValue,
                    //                          QuantityInMT = p.SaudaQuantity,
                    //                          SkuId = sku.Id,
                    //                          QuantityInCase = p.PendingQuantityInCase,
                    //                          Sku = sku.SkuName + " - " + p.SaudaNumber,
                    //                          Dealer = u.Name,
                    //                          ContractNumber = p.SaudaNumber,
                    //                          ContractValidFrom = saudaOrderContext.FirstOrDefault(so => so.SaudaId == saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber).Id) != null ? saudaOrderContext.FirstOrDefault(so => so.SaudaId == saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber).Id).ValidFromDate : DateTime.MinValue,
                    //                          ContractValidTo = saudaOrderContext.FirstOrDefault(so => so.SaudaId == saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber).Id) != null ? saudaOrderContext.FirstOrDefault(so => so.SaudaId == saudaContext.FirstOrDefault(_ => _.SaudaNumber == p.SaudaNumber).Id).ValidToDate : DateTime.MinValue,
                    //                      }

                    //                  ).ToList();

                    //saudaOrdersContext = _emamiContext.PendingContracts.AsNoTracking().Join(_emamiContext.Users.AsNoTracking(), p => p.UserId, u => u.Id, (p, u) => new { p, u })
                    //                    .Join(_emamiContext.Skus.AsNoTracking(), x => x.p.MaterialCode, sku => sku.SkuCode, (x, sku) => new { x, sku })
                    //                    .Join(_emamiContext.Sauda.AsNoTracking(), a => a.x.p.SaudaNumber, sauda => sauda.SaudaNumber, (a, sauda) => new { a, sauda }).
                    //                    Where(_ => _.a.x.p.PendingQuantityInCase != 0 && dealersList.Contains(_.a.x.u.Id)
                    //                      && (inputDto.SkuId.Contains(_.a.sku.Id))
                    //                     && (inputDto.StateIds.Contains(_.a.x.u.StateId))
                    //                     && _.a.x.p.SalesOrgId == _.a.sku.SalesOrganizationId && _.a.x.p.DistChnlId == _.a.sku.DistributionChannelId
                    //                     && _.a.x.p.DivisionId == _.a.sku.DivisionId).Select(s => new PendingContractSkuOutputDto()
                    //                     {
                    //                         UserId = s.a.x.u.Id,
                    //                         BiddingDate = s.sauda.BiddingDate,
                    //                         QuantityInMT = s.a.x.p.SaudaQuantity,
                    //                         SkuId = s.a.sku.Id,
                    //                         QuantityInCase = s.a.x.p.PendingQuantityInCase,
                    //                         Sku = s.a.sku.SkuName + " - " + s.a.x.p.SaudaNumber,
                    //                         Dealer = s.a.x.u.Name,
                    //                         ContractNumber = s.a.x.p.SaudaNumber,
                    //                         ContractValidFrom = saudaOrderContext.FirstOrDefault(so => so.SaudaId == s.sauda.Id) != null ? saudaOrderContext.FirstOrDefault(so => so.SaudaId == s.sauda.Id).ValidFromDate : DateTime.MinValue,
                    //                         ContractValidTo = saudaOrderContext.FirstOrDefault(so => so.SaudaId == s.sauda.Id) != null ? saudaOrderContext.FirstOrDefault(so => so.SaudaId == s.sauda.Id).ValidToDate : DateTime.MinValue,
                    //                     }).ToList();

                }
                if (saudaOrdersContext != null && saudaOrdersContext.Any())
                {
                    output.PendingDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                    output.TotalQuantityInCase = saudaOrdersContext.Sum(_ => _.QuantityInCase);
                    output.TotalQuantityInMT = saudaOrdersContext.Sum(_ => _.QuantityInMT);

                    var dealergroupby = saudaOrdersContext.GroupBy(g => g.UserId).Select(a => new
                    {
                        dealer = a.Key,
                        skulist = a.ToList()
                    }).ToList();

                    output.PendingContractDealerOutput = new List<PendingContractDealerOutputDto>();
                    foreach (var data in dealergroupby)
                    {
                        var item = new PendingContractDealerOutputDto()
                        {
                            Dealer = data.skulist.FirstOrDefault().Dealer,
                            PendingContractSkuOutput = data.skulist,
                        };

                        output.PendingContractDealerOutput.Add(item);
                    }
                    
                }

                if (output != null)
                {
                    return _resultService.SuccessObject(output);
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

        public ResultDto GetSaudaCallRecordMappingAttachments(long saudaId)
        {
            try
            {
                var output = new CallRecordingDto();
                var usercontext = _emamiContext.Users.AsNoTracking();
                var SuadaMappingContext = _emamiContext.SaudaAudioFileMapping.AsNoTracking().Where(_ => _.SaudaId == saudaId);
                if (SuadaMappingContext.IsAny())
                {
                    var UserIds = SuadaMappingContext.Join(_emamiContext.UserRoles.AsNoTracking(), u => u.UserId, ur => ur.UserId, (u, ur) => new { u, ur }).Where(_ => _.ur.RoleId == (int)DTO.Enums.Role.Dealer).Select(a => a.u.UserId).ToList();
                    if (!UserIds.IsAny())
                    {
                        output.FileDownloadName = usercontext.Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur }).FirstOrDefault(_ => _.u.Id == SuadaMappingContext.FirstOrDefault().UserId && _.ur.RoleId == (int)DTO.Enums.Role.Broker).u.Name;
                    }
                    else
                    {
                        output.FileDownloadName = usercontext.Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur }).FirstOrDefault(_ => _.u.Id == SuadaMappingContext.FirstOrDefault().UserId && _.ur.RoleId == (int)DTO.Enums.Role.Dealer).u.Name;
                    }

                    //date when audio entry mapped against sauda
                    output.CallRecordedDate = SuadaMappingContext.FirstOrDefault().CreatedDate;
                    output.CallRecordingListOutput = SuadaMappingContext.Where(_ => _.MediaTypeId == (int)DTO.Enums.MediaType.Audio).Select(s => new CallRecordingListOutputDto
                    {
                        CallRecordedFileName = s.AudioFileDetailsForActiveCustomers.AudioFileName,
                        AudioFileDetailId = s.AudioFileDetailsForActiveCustomersId ?? 0,
                        MediaTypeId = s.MediaTypeId,
                        CallRecordedDate = s.AudioFileDetailsForActiveCustomers.CreatedDate,
                        FileDownloadName = usercontext.FirstOrDefault(user => user.Id == s.UserId).Name
                    }).ToList();
                    var ImagePaths = SuadaMappingContext.FirstOrDefault(_ => _.MediaTypeId == (int)DTO.Enums.MediaType.Image)?.ImagePath;
                    if (!string.IsNullOrEmpty(ImagePaths))
                    {
                        var imagelist = ImagePaths.Split(',').ToList();

                        foreach (var data in imagelist)
                        {
                            var result = new CallRecordingListOutputDto()
                            {
                                CallRecordedFileName = data,
                                MediaTypeId = (int)DTO.Enums.MediaType.Image,
                            };
                            output.CallRecordingListOutput.Add(result);
                        }
                    }
                }

                if (output != null)
                {
                    return _resultService.SuccessObject(output);
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

        #region DailyBooking Report

        //public ResultDto GetDailyBookingReport(SaudaOrderReportInputputDto inputDto)
        //{
        //    _methodName = "GetDailyBookingReport";
        //    var saudaList = new List<SaudaOrderReportOutputDto>();
        //    try
        //    {
        //        var saudaOrderList = _emamiContext.Sauda.AsNoTracking()
        //            .Join(_emamiContext.SaudaOrders.AsNoTracking(), s => s.Id, so => so.SaudaId, (s, so) =>
        //            new
        //            {
        //                Sauda = new { UserId = s.UserId, BiddingDate = s.BiddingDate, SaudaBookingType = s.SaudaBookingType.Name, SaudaBookingTypeId = s.SaudaBookingTypeId },
        //                SaudaOrders = new
        //                {
        //                    Id = so.Id,
        //                    SaudaId = so.SaudaId,
        //                    DiscountTypeId = so.DiscountTypeIdForDailyReport,
        //                    DiscountAmount = so.DiscountAmountForDailyReport,
        //                    BidQuantityCase = so.BidQuantityCaseForDailyReport,
        //                    SkuId = so.SkuId,
        //                    Incoterms2 = so.Incoterms2,
        //                    BrokerId = so.BrokerId,
        //                    SkuName = so.Sku.SkuName,
        //                    SkuCode = so.Sku.SkuCode,
        //                    BidPrice = so.BidPriceForDailyReport,
        //                    SaudaNumber = so.SaudaNumber,
        //                    ValidFromDate = so.ValidFromDate,
        //                    ValidToDate = so.ValidToDate,
        //                    BidQuantity = so.BidQuantityForDailyReport,
        //                    PackTypeName = so.Sku.PackType.Name,
        //                    PackGroupName = so.Sku.PackGroup.Name,
        //                    VerticalName = so.OilType.Division.Name,
        //                    PricingId = so.PricingId,
        //                    StatusId = so.StatusIdForDailyReport,
        //                    SkuQuantity = so.Sku.Quantity,
        //                    SkuUom = so.Sku.Uom.Name,
        //                    Proo = so.ProoForDailyReport,
        //                    Frc1 = so.Frc1ForDailyReport,
        //                    SpecialRateId = so.SpecialRateRequestId,
        //                    QuotedPrice = so.QuotedPriceForDailyReport,
        //                    VerticalId = so.OilType.DivisionId,
        //                    SalesOrganizationId = so.OilType.SalesOrganizationId,
        //                    DistributionChannelId = so.OilType.DistributionChannelId,
        //                   // Remarks = so.RemarksForDailyReport,
        //                    OilType = so.OilType.Name,
        //                   // TradeTicketNo = so.TradeTicketNumber,
        //                    LitreConversion = so.OilType.LitreConversion,
        //                    //MaterialType = so.Sku.MaterialType.Name, 
        //                    //VolumeDiscount = so.VolumeDiscountForDailyReport,
        //                    //SchemeDiscountCase = so.SchemeDiscountCaseForDailyReport,
        //                    //SkuDiscountCase = so.SkuDiscountCaseForDailyReport,
        //                    //GPBenefitTypeId = so.GPBenefitTypeForDailyReport,
        //                    //so.GPBenefitDiscountOrDayForDailyReport,
        //                    //SurpriseBenefitTypeId = so.SurpriseBenefitTypeForDailyReport,
        //                    //SurpriseBenefitDiscountOrDay = so.SurpriseBenefitDiscountOrDayForDailyReport,
        //                    //BaseRate = so.BaseRateForDailyReport,
        //                    //VolumeDiscountCase = so.VolumeDiscountCaseForDailyReport,
        //                    //so.GPBenefitDiscountInCaseForDailyReport,
        //                    //so.SurpriseBenefitDiscountInCaseForDailyReport,
        //                    //so.BidPriceBeforeDiscountForDailyReport,
        //                    //so.IsBaseSauda,
        //                    //so.BaseSkuBidPriceForDailyReport
        //                }
        //            }).Join(_emamiContext.Pricing.AsNoTracking(), so => so.SaudaOrders.PricingId, p => p.Id, (so, p) =>
        //            new
        //            {
        //                so.SaudaOrders,
        //                so.Sauda,
        //                Pricing = new
        //                {
        //                    PlantId = p.PlantId,
        //                    //Discount = p.Discount, Premium = p.Premium, StateId = p.StateId, MaterialCost = p.MaterialCost, Margin = p.Margin, CushionMargin = p.CushionMargin, RaMargin = p.RaMargin, PackingCost = p.PackingCost, SchemeCostRecovery = p.SchemeCostRecovery, HoneycombCost = p.HoneycombCost, PrimaryFrieght = p.PrimaryFrieght, SecondaryFrieght = p.SecondaryFrieght, PlantSecondaryFrieght = p.PlantSecondaryFrieght, DepotCost = p.DepotCost, DetentionCost = p.DetentionCost, AdditionalCost = p.AdditionalCost, OilTransferCostForPlant = p.OilTransferCostForPlant, OilTransferCostForDepot = p.OilTransferCostForDepot, CustomerGroupMargin = p.CustomerGroupMargin, PlantGSTPercentage = p.PlantGSTPercentage, DepotGSTPercentage = p.DepotGSTPercentage,
        //                    sku = p.Sku
        //                },
        //            }).Join(_emamiContext.Users.AsNoTracking(), s => s.Sauda.UserId, u => u.Id, (s, u) =>
        //            new
        //            { s.Sauda, s.SaudaOrders, s.Pricing, User = new { StateId = u.StateId, Code = u.Code, Name = u.Name,/* FreightRouteName = u.FreightRoute.Name,*/ Id = u.Id, /* CustomerGroupOneId = u.CustomerGroupOneId, CustomerGroupTwoId = u.CustomerGroupTwoId */} })
        //            //.Join(_emamiContext.Depots.AsNoTracking(), p => p.Pricing.PlantId, d => d.Id, (p, d) => new { p.Sauda, p.SaudaOrders, p.Pricing, p.User, Plant = new { Name = d.Name, Code = d.Code, StateId = d.StateId, StateName = d.State.StateName } })
        //            //.Join(_emamiContext.Depots.AsNoTracking(), p => p.Pricing.DepotId, d => d.Id, (p, d) =>
        //            //new
        //            //{
        //            //    p.Sauda,
        //            //    p.SaudaOrders,
        //            //    p.Pricing,
        //            //    p.User,
        //            //    p.Plant,
        //            //    Depot = new { Name = d.Name, Code = d.Code },
        //            //})
        //            //.Join(_emamiContext.State.AsNoTracking(), p => p.Pricing.StateId, st => st.Id, (p, st) =>
        //            //new
        //            //{
        //            //    p.Sauda,
        //            //    p.SaudaOrders,
        //            //    p.Pricing,
        //            //    p.User,
        //            //    p.Plant,
        //            //    p.Depot,
        //            //    State = new { Name = st.StateName },
        //            //})
        //            //.Join(_emamiContext.UserCustomerMapping.AsNoTracking(), p => p.User.Id, uc => uc.CustomerId, (p, uc) =>
        //            //new
        //            //{
        //            //    p.Sauda,
        //            //    p.SaudaOrders,
        //            //    p.Pricing,
        //            //    p.User,
        //            //    p.Plant,
        //            //    p.Depot,
        //            //    p.State,
        //            //    UserCustomerMapping = uc.UserId,
        //            //})
        //            //.Join(_emamiContext.Users.AsNoTracking(), p => p.UserCustomerMapping, ucb => ucb.Id, (p, ucb) =>
        //            //new
        //            //{
        //            //    p.Sauda,
        //            //    p.SaudaOrders,
        //            //    p.Pricing,
        //            //    p.User,
        //            //    p.Plant,
        //            //    p.Depot,
        //            //    p.State,
        //            //    p.UserCustomerMapping,
        //            //    BdoName = ucb.Name,
        //            //})
        //            .Where(w => DbFunctions.TruncateTime(w.Sauda.BiddingDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
        //            && DbFunctions.TruncateTime(w.Sauda.BiddingDate) <= DbFunctions.TruncateTime(inputDto.ToDate) && inputDto.StateIds.Contains(w.User.StateId)
        //            && (inputDto.VerticalId > 0 ? (w.SaudaOrders.VerticalId == inputDto.VerticalId && w.SaudaOrders.SalesOrganizationId==inputDto.SalesOrganizationId && w.SaudaOrders.DistributionChannelId==inputDto.DistributionChannelId) : w.SaudaOrders.VerticalId > 0))
        //            .Select(s => s).ToList();

        //        if (inputDto.StatusIds.Count > 0)
        //        {
        //            if (inputDto.StatusIds.Contains(-1))
        //            {
        //                saudaOrderList = saudaOrderList.ToList();
        //            }
        //            else
        //            {
        //                saudaOrderList = saudaOrderList.Where(_ => inputDto.StatusIds.Contains(_.SaudaOrders.StatusId)).ToList();
        //            }
        //        }

        //        //saudaOrderList.RemoveAll(item => item.Pricing.PlantId == 0 && (item.Pricing.sku.DivisionId == (int)DTO.Enums.Division.Hbc || item.Pricing.sku.DivisionId == (int)DTO.Enums.Division.SpecialityFat));

        //        if (saudaOrderList != null && saudaOrderList.Any())
        //        {
        //            #region Common Data's
        //            var specialRateId = saudaOrderList.Select(s => s.SaudaOrders.SpecialRateId).Distinct().ToList();
        //            var SpecialRateDatas = _emamiContext.SpecialRate.AsNoTracking().Where(_ => specialRateId.Contains(_.Id))
        //                .Select(s => new
        //                {
        //                    Id = s.Id,
        //                    IsLTD = s.IsLTD
        //                }).ToList();

        //            var tradeTicketNos = saudaOrderList.Select(s => s.SaudaOrders.TradeTicketNo).Distinct().ToList();
        //            var TradeTicketDatas = _emamiContext.TradeTicket.AsNoTracking().Where(_ => tradeTicketNos.Contains(_.TradeTicketNumber))
        //                .Select(s => new
        //                {
        //                    TotalCost = s.TotalCost,
        //                    TradeTicketNumber = s.TradeTicketNumber
        //                }).ToList();

        //            var skuIds = saudaOrderList.Select(s => s.SaudaOrders.SkuId).Distinct().ToList();
        //            var SkuUomMappingDatas = _emamiContext.SkuUomMapping
        //                .Where(_ => skuIds.Contains(_.SkuId) && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)(int)DTO.Enums.Uom.Nos)
        //                .Select(s => new
        //                {
        //                    SkuId = s.SkuId,
        //                    UomId = s.UomId,
        //                    RelationUomId = s.RelationUomId,
        //                    ConversionFactor = s.ConversionFactor
        //                }).ToList();

        //            var SkuDatas = _emamiContext.Skus.AsNoTracking().Where(w => skuIds.Contains(w.Id))
        //                .Select(s => new
        //                {
        //                    Id = s.Id,
        //                    UomId = s.UomId,
        //                    LitreConversion = s.OilType.LitreConversion,
        //                    Quantity = s.Quantity
        //                });

        //            var brokerIds = saudaOrderList.Select(s => s.SaudaOrders.BrokerId).Distinct().ToList();
        //            var UserDatas = _emamiContext.Users.AsNoTracking().Where(w => brokerIds.Contains(w.Id))
        //                .Select(s => new
        //                {
        //                    Id = s.Id,
        //                    Name = s.Name,
        //                    Code = s.Code
        //                }).ToList();


        //            var saudaUserIds = saudaOrderList.Select(s => s.Sauda.UserId).Distinct().ToList();
        //            var BdoDatas = _emamiContext.Users.AsNoTracking()
        //                .Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { User = u, UserRoles = ur })
        //                .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), us => us.User.Id, ucm => ucm.CustomerId, (us, ucm) => new { us.User, us.UserRoles, UserCustomer = ucm })
        //                .Where(w => w.UserRoles.RoleId == (long)DTO.Enums.Role.StateTrader && saudaUserIds.Contains(w.UserCustomer.CustomerId))
        //                .Select(s => new
        //                {
        //                    Id = s.User.Id,
        //                    Name = s.User.Name,
        //                    Code = s.User.Code
        //                }).ToList();

        //            //var customerGroupOneIds = saudaOrderList.Select(s => s.User.CustomerGroupOneId).ToList();
        //            //var customerGroupTwoIds = saudaOrderList.Select(s => s.User.CustomerGroupTwoId).ToList();

        //            //var CustomerGroupOneDatas = _emamiContext.CustomerGroupOne.AsNoTracking().Where(w => customerGroupOneIds.Contains(w.Id))
        //            //    .Select(s => new
        //            //    {
        //            //        Id = s.Id,
        //            //        Name = s.GroupName
        //            //    }).ToList();

        //            //var CustomerGroupTwoDatas = _emamiContext.CustomerGroupTwo.AsNoTracking().Where(w => customerGroupTwoIds.Contains(w.Id))
        //            //    .Select(s => new
        //            //    {
        //            //        Id = s.Id,
        //            //        Name = s.GroupName
        //            //    }).ToList();

        //            #endregion

        //            var depotContext = _emamiContext.Depots.AsNoTracking();

        //            foreach (var s in saudaOrderList)
        //            {
        //                if (s.SaudaOrders.BidQuantityCase <= 0)
        //                {
        //                    continue;
        //                }
        //                decimal raPremiumWithtax = 0;
        //                decimal raPremiumWithoutTax = 0;
        //                decimal allocationPremiumWithtax = 0;
        //                decimal allocationPremiumWithoutTax = 0;
        //                decimal raTotalDiscount = 0;
        //                decimal saleRate = 0;
        //                //decimal honeycombCost = s.Pricing.HoneycombCost;
        //                decimal discount = 0, premium = 0, LtdValue = 0, specialRate = 0, specialRateDiscount = 0;
        //                bool isLtd = false;
        //                if (s.SaudaOrders.SpecialRateId > 0)
        //                {
        //                    //isLtd = _emamiContext.SpecialRate.AsNoTracking().FirstOrDefault(_ => _.Id == s.SaudaOrders.SpecialRateId).IsLTD;
        //                    if (SpecialRateDatas != null && SpecialRateDatas.Any())
        //                        isLtd = SpecialRateDatas.FirstOrDefault(_ => _.Id == s.SaudaOrders.SpecialRateId).IsLTD;

        //                    var result = s.SaudaOrders.BidQuantityCase > 0 ? (s.SaudaOrders.QuotedPrice - s.SaudaOrders.BidPrice) / s.SaudaOrders.BidQuantityCase : 0;
        //                    if (result >= 0)
        //                    {
        //                        specialRateDiscount = result;
        //                    }
        //                    else
        //                    {
        //                        premium = -(result);
        //                    }
        //                    if (isLtd)
        //                    {
        //                        LtdValue = specialRateDiscount;
        //                    }
        //                    else
        //                    {
        //                        specialRate = specialRateDiscount;
        //                    }
        //                }
        //                else
        //                {
        //                    discount = s.SaudaOrders.DiscountTypeId == 1 ? CalculateOneCase(s.SaudaOrders.DiscountAmount, s.SaudaOrders.BidQuantityCase) : 0;
        //                    premium = s.SaudaOrders.DiscountTypeId == 2 ? CalculateOneCase(s.SaudaOrders.DiscountAmount, s.SaudaOrders.BidQuantityCase) : 0;
        //                }

        //                //if (s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ExPlant || s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ExDepot)
        //                //    honeycombCost = 0;

        //                //SaleRate
        //                var PR00 = s.SaudaOrders.Proo //> 0
        //                                              //? s.SaudaOrders.Proo
        //                                              //: ((s.Pricing.MaterialCost
        //                                              //+ s.Pricing.PackingCost
        //                                              //+ honeycombCost
        //                                              //+ (s.Sauda.SaudaBookingTypeId == (long)DTO.Enums.SaudaBookingTypes.TraditionalProcess ? (s.Pricing.Margin + s.Pricing.CushionMargin) : s.Pricing.RaMargin)
        //                                              //+ s.Pricing.SchemeCostRecovery
        //                     + premium
        //                     //+ s.Pricing.AdditionalCost) 
        //                     - (discount + LtdValue + specialRate);

        //                var FRC1 = s.SaudaOrders.Frc1 > 0 ? s.SaudaOrders.Frc1 : Utility.CalculateFRC1(0, 0, 0, 0, s.SaudaOrders.Incoterms2, 0, 0);

        //                decimal sRate = 0;
        //                decimal taxPaidValue = 0;
        //                decimal saleRateWithTax = 0;
        //                decimal discountGstPercentage = 0;
        //                decimal discountWithTax = 0;
        //                decimal discountTaxAmount = 0;

        //                //if (s.Sauda.SaudaBookingTypeId == (long)DTO.Enums.SaudaBookingTypes.ReverseAuction)
        //                //{
        //                //    //RA2.0 Changes
        //                //    //raPremiumWithtax = (s.SaudaOrders.BidPrice / s.SaudaOrders.BidQuantityCase) - s.SaudaOrders.BaseRate;
        //                //    if (s.SaudaOrders.IsBaseSauda)
        //                //    {
        //                //        raPremiumWithtax = s.SaudaOrders.BidPriceBeforeDiscountForDailyReport - s.SaudaOrders.BaseRate;
        //                //    }
        //                //    else
        //                //    {
        //                //        raPremiumWithtax = s.SaudaOrders.BaseSkuBidPriceForDailyReport - s.SaudaOrders.BaseRate;
        //                //    }

        //                //    decimal bidPricePerCause = (s.SaudaOrders.BidPrice / s.SaudaOrders.BidQuantityCase);
        //                //    raTotalDiscount = s.SaudaOrders.VolumeDiscountCase +
        //                //        s.SaudaOrders.SchemeDiscountCase +
        //                //        s.SaudaOrders.SkuDiscountCase +
        //                //        (s.SaudaOrders.GPBenefitTypeId == (int)DTO.Enums.BenefitType.NONSAP ? s.SaudaOrders.GPBenefitDiscountInCaseForDailyReport : 0) +
        //                //        (s.SaudaOrders.SurpriseBenefitTypeId == (int)DTO.Enums.BenefitType.NONSAP ? s.SaudaOrders.SurpriseBenefitDiscountInCaseForDailyReport : 0);
        //                //    // decimal discountWithTax = Utility.IncludeGst(1, s.Pricing.PlantGSTPercentage, raTotalDiscount);

        //                //    switch (s.SaudaOrders.Incoterms2)
        //                //    {
        //                //        case (long)DTO.Enums.IncoTerms.ExPlant:
        //                //            discountGstPercentage = Utility.GetGstAmount(1, s.Pricing.PlantGSTPercentage);
        //                //            discountWithTax = raTotalDiscount * discountGstPercentage;
        //                //            discountTaxAmount = discountWithTax - raTotalDiscount;
        //                //            taxPaidValue = Utility.DecimalFormatTwo(bidPricePerCause);// - discountTaxAmount;
        //                //            saleRateWithTax = bidPricePerCause;// - discountTaxAmount;
        //                //            saleRate = Utility.ExcludeGst(1, s.Pricing.PlantGSTPercentage, saleRateWithTax);
        //                //            if (s.SaudaOrders.IsBaseSauda)
        //                //            {
        //                //                raPremiumWithoutTax = Utility.ExcludeGst(1, s.Pricing.PlantGSTPercentage, raPremiumWithtax);
        //                //            }
        //                //            else
        //                //            {
        //                //                raPremiumWithoutTax = Utility.ExcludeGst(1, s.Pricing.PlantGSTPercentage, raPremiumWithtax);
        //                //            }
        //                //            break;
        //                //        case (long)DTO.Enums.IncoTerms.ForPlant:
        //                //            //saleRate = Utility.ExcludeGst(1, s.Pricing.PlantGSTPercentage, bidPricePerCause);
        //                //            //taxPaidValue = Utility.DecimalFormatTwo(saleRate * Utility.GetGstAmount(1, s.Pricing.PlantGSTPercentage));
        //                //            discountGstPercentage = Utility.GetGstAmount(1, s.Pricing.PlantGSTPercentage);
        //                //            discountWithTax = raTotalDiscount * discountGstPercentage;
        //                //            discountTaxAmount = discountWithTax - raTotalDiscount;
        //                //            taxPaidValue = Utility.DecimalFormatTwo(bidPricePerCause);// - discountTaxAmount;
        //                //            saleRateWithTax = bidPricePerCause;// - discountTaxAmount;
        //                //            saleRate = Utility.ExcludeGst(1, s.Pricing.PlantGSTPercentage, saleRateWithTax);
        //                //            if (s.SaudaOrders.IsBaseSauda)
        //                //            {
        //                //                raPremiumWithoutTax = Utility.ExcludeGst(1, s.Pricing.PlantGSTPercentage, raPremiumWithtax);
        //                //            }
        //                //            else
        //                //            {
        //                //                raPremiumWithoutTax = Utility.ExcludeGst(1, s.Pricing.PlantGSTPercentage, raPremiumWithtax);
        //                //            }
        //                //            break;
        //                //        case (long)DTO.Enums.IncoTerms.ExDepot:
        //                //            //saleRate = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, bidPricePerCause);
        //                //            //taxPaidValue = Utility.DecimalFormatTwo(saleRate * Utility.GetGstAmount(1, s.Pricing.DepotGSTPercentage));
        //                //            discountGstPercentage = Utility.GetGstAmount(1, s.Pricing.DepotGSTPercentage);
        //                //            discountWithTax = raTotalDiscount * discountGstPercentage;
        //                //            discountTaxAmount = discountWithTax - raTotalDiscount;
        //                //            taxPaidValue = Utility.DecimalFormatTwo(bidPricePerCause);// - discountTaxAmount;
        //                //            saleRateWithTax = bidPricePerCause;// - discountTaxAmount;
        //                //            saleRate = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, saleRateWithTax);
        //                //            if (s.SaudaOrders.IsBaseSauda)
        //                //            {
        //                //                raPremiumWithoutTax = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, raPremiumWithtax);
        //                //            }
        //                //            else
        //                //            {
        //                //                raPremiumWithoutTax = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, raPremiumWithtax);
        //                //            }
        //                //            break;
        //                //        case (long)DTO.Enums.IncoTerms.ForDepot:
        //                //            //saleRate = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, bidPricePerCause);
        //                //            //taxPaidValue = Utility.DecimalFormatTwo(saleRate * Utility.GetGstAmount(1, s.Pricing.DepotGSTPercentage));
        //                //            discountGstPercentage = Utility.GetGstAmount(1, s.Pricing.DepotGSTPercentage);
        //                //            discountWithTax = raTotalDiscount * discountGstPercentage;
        //                //            discountTaxAmount = discountWithTax - raTotalDiscount;
        //                //            taxPaidValue = Utility.DecimalFormatTwo(bidPricePerCause);// - discountTaxAmount;
        //                //            saleRateWithTax = bidPricePerCause;// - discountTaxAmount;
        //                //            saleRate = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, saleRateWithTax);

        //                //            if (s.SaudaOrders.IsBaseSauda)
        //                //            {
        //                //                raPremiumWithoutTax = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, raPremiumWithtax);
        //                //            }
        //                //            else
        //                //            {
        //                //                raPremiumWithoutTax = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, raPremiumWithtax);
        //                //            }
        //                //            break;
        //                //        default:
        //                //            saleRate = 0;
        //                //            break;
        //                //    }
        //                //    saleRate = Utility.DecimalFormatTwo(saleRate);
        //                //    sRate = saleRate;
        //                //    PR00 = (PR00 + s.Pricing.CustomerGroupMargin) - Utility.DecimalFormatTwo(raTotalDiscount / discountGstPercentage);
        //                //    PR00 = Convert.ToDecimal(string.Format("{0:0.00}", PR00 + raPremiumWithoutTax));// Convert.ToDecimal(string.Format("{0:0.00}", PR00)) + Convert.ToDecimal(string.Format("{0:0.00}", raPremiumWithoutTax)); // (PR00 + raPremiumWithoutTax);

        //                //    if (!s.SaudaOrders.IsBaseSauda)
        //                //    {
        //                //        decimal gstPercentage = 0;

        //                //        if (s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExPlant || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForPlant)
        //                //        {
        //                //            gstPercentage = s.Pricing.PlantGSTPercentage;
        //                //        }
        //                //        else if (s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExDepot || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForDepot)
        //                //        {
        //                //            gstPercentage = s.Pricing.DepotGSTPercentage;
        //                //        }

        //                //        allocationPremiumWithtax = s.SaudaOrders.BaseSkuBidPriceForDailyReport - s.SaudaOrders.BidPriceBeforeDiscountForDailyReport; //Utility.DecimalFormatTwo(saleRate * gstPercentage);
        //                //        allocationPremiumWithoutTax = Utility.DecimalFormatTwo(Utility.ExcludeGst(1, gstPercentage, allocationPremiumWithtax));
        //                //        PR00 = Convert.ToDecimal(string.Format("{0:0.00}", PR00 - allocationPremiumWithoutTax));
        //                //    }
        //                //}
        //                //else
        //                //{
        //                saleRate = PR00 + FRC1;
        //                sRate = (s.SaudaOrders.BidPrice > 0 && s.SaudaOrders.BidQuantityCase > 0) ? s.SaudaOrders.BidPrice / s.SaudaOrders.BidQuantityCase : 0;
        //                taxPaidValue = Utility.DecimalFormatTwo(saleRate * Convert.ToDecimal(Constants.TaxPaidValue));
        //                //}

        //                //RealizationPerCase
        //                //var realizationPerCase = s.SaudaOrders.Proo > 0 ? s.SaudaOrders.Proo : CalculateRealizationPerCase(s.Pricing.MaterialCost, s.Pricing.Margin, s.Pricing.CushionMargin, s.Pricing.RaMargin, premium, discount, s.Sauda.SaudaBookingTypeId, raPremiumWithoutTax);

        //                var realizationPerCase = CalculateRealizationPerCase(PR00, 0, 0, 0, 0, premium, discount, s.Sauda.SaudaBookingTypeId, 0, 0, 0, raPremiumWithoutTax, 0);

        //                //RealizationPerMT
        //                //var realizationPerMT = CalculateReliazationCaseToMatericTon(s.SaudaOrders.SkuId, realizationPerCase);           //(realizationPerCase * 1000);



        //                //RealizationPerCase
        //                //var realizationPerCase = CalculateRealizationPerCase(PR00, s.Pricing.MaterialCost, s.Pricing.Margin, s.Pricing.CushionMargin, s.Pricing.RaMargin, premium, discount, s.Sauda.SaudaBookingTypeId, s.Pricing.PackingCost, honeycombCost, s.Pricing.SchemeCostRecovery);
        //                //RealizationPerMT
        //                //var realizationPerMT = CalculateReliazationCaseToMatericTon(s.SaudaOrders.SkuId, realizationPerCase);           //(realizationPerCase * 1000);

        //                decimal realizationPerMT = 0;
        //                //var skuContext = SkuDatas.FirstOrDefault(_ => _.Id == s.SaudaOrders.SkuId);
        //                //if (skuContext != null)
        //                //{
        //                //    decimal numberOfPcs = 0;
        //                //    var skuUomContext = SkuUomMappingDatas.FirstOrDefault(_ => _.SkuId == s.SaudaOrders.SkuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
        //                //    if (skuUomContext != null)
        //                //    {
        //                //        numberOfPcs = skuUomContext.ConversionFactor;
        //                //    }

        //                //    var quantityTypeId = skuContext.UomId;
        //                //    var ltrConversion = skuContext.LitreConversion;
        //                //    if (quantityTypeId == (int)DTO.Enums.Uom.Ltr)
        //                //    {
        //                //        realizationPerMT = realizationPerCase * (ltrConversion / skuContext.Quantity);
        //                //    }
        //                //    else
        //                //    {
        //                //        realizationPerMT = realizationPerCase * (1000 / skuContext.Quantity);
        //                //    }
        //                //}


        //                var totalValue = (saleRate * s.SaudaOrders.BidQuantityCase);


        //                //var broker = BrokerNameCode(s.SaudaOrders.BrokerId);
        //                var broker = UserDatas.FirstOrDefault(f => f.Id == s.SaudaOrders.BrokerId);

        //                decimal brokerage = 0, realizationPerCasePostBrokerage = 0, realizationPerMTPostBrokerage = 0, finalRealization = 0, purchaseCost = 0;
        //                if (s.SaudaOrders.TradeTicketNo != null)
        //                {
        //                    //var purchaseCostContext = _emamiContext.TradeTicket.AsNoTracking().FirstOrDefault(_ => _.TradeTicketNumber == s.SaudaOrders.TradeTicketNo)?.TotalCost;
        //                    var purchaseCostContext = TradeTicketDatas.FirstOrDefault(_ => _.TradeTicketNumber == s.SaudaOrders.TradeTicketNo)?.TotalCost;
        //                    if (purchaseCostContext != null)
        //                    {
        //                        purchaseCost = (decimal)purchaseCostContext;
        //                    }
        //                }

        //                if (broker != null)
        //                {
        //                    brokerage = 2;
        //                }
        //                realizationPerCasePostBrokerage = realizationPerCase - brokerage;
        //                decimal SKUWiseWeight = 0;
        //                if (s.SaudaOrders.SkuUom == DTO.Enums.Uom.Ltr.ToString())
        //                {
        //                    //var SkuUomMappingContext = _emamiContext.SkuUomMapping.FirstOrDefault(_ => _.SkuId == s.SaudaOrders.SkuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)(int)DTO.Enums.Uom.Nos);
        //                    var SkuUomMappingContext = SkuUomMappingDatas.FirstOrDefault(_ => _.SkuId == s.SaudaOrders.SkuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)(int)DTO.Enums.Uom.Nos);
        //                    if (SkuUomMappingContext != null)
        //                    {
        //                        SKUWiseWeight = s.SaudaOrders.LitreConversion > 0 ? (s.SaudaOrders.SkuQuantity * 1000 * SkuUomMappingContext.ConversionFactor) / s.SaudaOrders.LitreConversion : 0;
        //                    }
        //                    else
        //                    {
        //                        SKUWiseWeight = s.SaudaOrders.LitreConversion > 0 ? (s.SaudaOrders.SkuQuantity * 1000) / s.SaudaOrders.LitreConversion : 0;
        //                    }
        //                }
        //                else
        //                {
        //                    SKUWiseWeight = s.SaudaOrders.SkuQuantity;
        //                }

        //                if (realizationPerCase > 0 && SKUWiseWeight > 0)
        //                {
        //                    realizationPerMT = realizationPerCase / SKUWiseWeight * 1000;
        //                }
        //                realizationPerMTPostBrokerage = realizationPerCasePostBrokerage != 0 && SKUWiseWeight > 0 ? (realizationPerCasePostBrokerage / SKUWiseWeight) * 1000 : 0;
        //                finalRealization = realizationPerMTPostBrokerage;// - honeycombCost;
        //                var employeeData = GetBdoname(s.Sauda.UserId);
        //                saudaList.Add(new SaudaOrderReportOutputDto()
        //                {
        //                    CustomerCode = s.User.Code,
        //                    CustomerName = s.User.Name,
        //                    //FreightRoute = s.User.FreightRouteName,
        //                    BrokerName = broker != null ? broker.Name : "",
        //                    BrokerCode = broker != null ? broker.Code : "",
        //                    SkuName = s.SaudaOrders.SkuName,
        //                    SkuCode = s.SaudaOrders.SkuCode,
        //                    BidQuantityCase = s.SaudaOrders.BidQuantityCase,
        //                    PR00 = PR00,
        //                    FRC1 = FRC1,
        //                    SaleRate = sRate,
        //                    BidPrice = s.SaudaOrders.BidPrice,
        //                    Incoterms = Utility.GetEnumFromString<DTO.Enums.IncoTerms>(s.SaudaOrders.Incoterms2), // IncotermsName(s.SaudaOrders.Incoterms2),
        //                    AppBookingNo = s.SaudaOrders.SaudaId.ToString(),
        //                    BiddingDate = s.Sauda.BiddingDate,
        //                    ValidFromDate = s.SaudaOrders.ValidFromDate,
        //                    ValidToDate = s.SaudaOrders.ValidToDate,
        //                    BidQuantity = Utility.DecimalFormatThree(s.SaudaOrders.BidQuantity),
        //                    PackGroup = s.SaudaOrders.PackGroupName,
        //                    //DepotCode = (s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ForDepot || s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ExDepot) ? (s.Pricing.DepotId == 0 ? "" : depotContext.FirstOrDefault(depot => depot.Id == s.Pricing.DepotId).Code) : (s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ForRake || s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ExRake) ? (s.Pricing.DepotId == 0 ? "" : depotContext.FirstOrDefault(depot => depot.Id == s.Pricing.DepotId).Code) : "",
        //                    //DepotName = (s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ForDepot || s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ExDepot) ? (s.Pricing.DepotId == 0 ? "" : depotContext.FirstOrDefault(depot => depot.Id == s.Pricing.DepotId).Name) : (s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ForRake || s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ExRake) ? (s.Pricing.DepotId == 0 ? "" : depotContext.FirstOrDefault(depot => depot.Id == s.Pricing.DepotId).Name) : "",
        //                    //State = s.Pricing.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(state => state.Id == s.Pricing.StateId).StateName : "",
        //                    PlantName = s.Pricing.PlantId > 0 ? depotContext.FirstOrDefault(plant => plant.Id == s.Pricing.PlantId).Name : "",
        //                    PlantCode = s.Pricing.PlantId > 0 ? depotContext.FirstOrDefault(plant => plant.Id == s.Pricing.PlantId).Code : "",
        //                    RealizationPerMt = realizationPerMT,
        //                    UOM = "Case",
        //                    PackSize = s.SaudaOrders.SkuQuantity + " " + s.SaudaOrders.SkuUom,
        //                    //MaterialCost = s.Pricing.MaterialCost,
        //                    //PrimaryFreight = (s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExDepot || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForDepot || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForRake) ?
        //                    //s.Pricing.PrimaryFrieght : 0,
        //                    //PackingCost = s.Pricing.PackingCost,
        //                    //HoneycombCost = honeycombCost,
        //                    BrokerageCost = 0,
        //                    DetentionCharges =
        //                    //(s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExDepot || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForDepot || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForRake) ? s.Pricing.DetentionCost : 
        //                    0,
        //                    //DepotCost = (s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExDepot || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForDepot || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForRake) ? s.Pricing.DepotCost : 0,
        //                    //MarginCostTP = (s.Pricing.Margin + s.Pricing.CushionMargin),
        //                    //MarginCostRA = s.Pricing.RaMargin,
        //                    //SecondaryFreight = s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForDepot ? s.Pricing.SecondaryFrieght :
        //                    //(s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForPlant ? s.Pricing.PlantSecondaryFrieght : 0),
        //                    TD = Utility.DecimalFormatTwo(discount),
        //                    //LTD = 0,
        //                    TotalValue = totalValue,
        //                    EmployeeName = employeeData != null ? employeeData.Name : "", // GetBdoname(s.Sauda.UserId).Name,
        //                    EmployeeCode = employeeData != null ? employeeData.Code : "", // GetBdoname(s.Sauda.UserId).Code,
        //                    Vertical = s.SaudaOrders.VerticalName,
        //                    Premium = Utility.DecimalFormatTwo(premium),
        //                    SaudaBookingType = s.Sauda.SaudaBookingType,
        //                    RealizationPerCase = realizationPerCase,
        //                    //ActualPackingCost = s.Pricing.PackingCost,
        //                    Status = Enum.GetName(typeof(DTO.Enums.Status), s.SaudaOrders.StatusId),
        //                    LTDValue = LtdValue,
        //                    SpecialRate = specialRate,
        //                    Remarks = s.SaudaOrders.Remarks,
        //                    //CushionMargin = s.Pricing.CushionMargin,
        //                    BiddingTime = s.Sauda.BiddingDate.TimeOfDay,
        //                    OilType = s.SaudaOrders.OilType,
        //                    TaxPaid = taxPaidValue, // Utility.DecimalFormatTwo(saleRate * Convert.ToDecimal(Constants.TaxPaidValue)),
        //                    Brokerage = brokerage,
        //                    Area = s.Pricing.PlantId == 0 ? "" : (s.User.StateId == (depotContext.FirstOrDefault(plant => plant.Id == s.Pricing.PlantId).StateId) ? (depotContext.FirstOrDefault(plant => plant.Id == s.Pricing.PlantId).State.StateName) : Constants.OutOfState.Replace(Constants.StateName, depotContext.FirstOrDefault(plant => plant.Id == s.Pricing.PlantId).State.StateName)),
        //                    RealizationPerCasePostBrokerage = realizationPerCasePostBrokerage,
        //                    SkuWiseWeight = SKUWiseWeight,
        //                    RealizationPerMTPostBrokerage = realizationPerMTPostBrokerage,
        //                    FinalRealization = finalRealization,
        //                    RealizationTotal = finalRealization * s.SaudaOrders.BidQuantity,
        //                    Purchase = purchaseCost,
        //                    PurchaseTotal = purchaseCost * s.SaudaOrders.BidQuantity,
        //                    MarginPMTLineItem = finalRealization - purchaseCost,
        //                    //SchemeCost = s.Pricing.SchemeCostRecovery,
        //                    //MaterialType = s.SaudaOrders.MaterialType,
        //                    //CustomerGroupMargin = s.Pricing.CustomerGroupMargin,
        //                    RaTotalDiscount = raTotalDiscount,
        //                    SaudaBookingTypeId = s.Sauda.SaudaBookingTypeId,
        //                    RAPremiumWithTax = raPremiumWithtax,
        //                    RAPremiumWithoutTax = raPremiumWithoutTax,
        //                    //AdditionalCost = s.Pricing.AdditionalCost,
        //                    //OilTransferCost = s.Pricing.OilTransferCostForPlant > 0 ? s.Pricing.OilTransferCostForPlant : s.Pricing.OilTransferCostForDepot,
        //                    IsBaseSauda = s.SaudaOrders.IsBaseSauda,
        //                    SkuAllocationPremiumWithTax = allocationPremiumWithtax,
        //                    SkuAllocationPremiumWithoutTax = allocationPremiumWithoutTax,
        //                    //CustomerGroupOne = CustomerGroupOneDatas.FirstOrDefault(f => f.Id == s.User.CustomerGroupOneId)?.Name,
        //                    //CustomerGroupTwo = CustomerGroupTwoDatas.FirstOrDefault(f => f.Id == s.User.CustomerGroupTwoId)?.Name,
        //                    SaudaOrderId = s.SaudaOrders.Id,
        //                    SaudaNumber = s.SaudaOrders.SaudaNumber != null ? s.SaudaOrders.SaudaNumber : string.Empty
        //                });
        //            }
        //        }

        //        return _resultService.SuccessObject(saudaList);
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        #endregion

        #region Credit Limt
        public ResultDto GetCreditLimitReport(ReportFilterDto inputDto)
        {
            _methodName = "GetCreditLimitReport";
            var creditLimitOutputDto = new List<HANACreditMasterDto>();
            try
            {
                var dealerCode = new List<string>();

                inputDto.zhId = inputDto.zhId.IsAny() ? inputDto.zhId : _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u, ur }).Where(_ => _.ur.RoleId == (int)DTO.Enums.Role.ZonalTrader).Select(_ => _.u.Id).ToList();
                inputDto.bdoId = inputDto.bdoId.IsAny() ? inputDto.bdoId : _emamiContext.Users.AsNoTracking().Where(_ => inputDto.zhId.Contains((long)_.ReportingToId)).Select(_ => _.Id).ToList();
                dealerCode = _emamiContext.Users.AsNoTracking().Join(_emamiContext.UserCustomerMapping.AsNoTracking(), u => u.Id, ucm => ucm.CustomerId, (u, ucm) => new { u, ucm }).Where(_ => inputDto.bdoId.Contains(_.ucm.UserId) && _.u.IsActive).Select(a => a.u.Code).ToList();

                if (!string.IsNullOrEmpty(inputDto.dealerCode))
                {
                    if (dealerCode.Contains(inputDto.dealerCode))
                    {
                        dealerCode.Clear();
                        dealerCode.Add(inputDto.dealerCode);
                    }
                    else
                    {
                        return _resultService.ErrorMessage(Constants.InvalidDealer);
                    }

                }
                var dealerContext = _emamiContext.Users.AsNoTracking().Where(_ => _.IsActive && inputDto.StateIds.Contains(_.StateId) && dealerCode.Contains(_.Code))
                  .Join(_emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.Dealer), u => u.Id, ur => ur.UserId, (u, ur) => new { u })
                  //.Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.u.Id, uc => uc.CustomerId, (x, uc) => new { x.u, uc });
                  //.Join(_emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.StateTrader), x => x.uc.UserId, ur => ur.UserId, (x, ur) => new { x.u, Employee = ur.User.Name });
                  .Join(_emamiContext.State.AsNoTracking(), y => y.u.StateId, state => state.Id, (y, state) => new { y, state })
                  .Join(_emamiContext.UserDivisionMappings.AsNoTracking(), ud => ud.y.u.Id, udiv => udiv.UserId, (ud, udiv) => new { ud, udiv })
                  .Where(_ => (inputDto.SalesOrganizationId>0 ? _.udiv.SalesOrganizationId == inputDto.SalesOrganizationId : _.udiv.SalesOrganizationId > 0) && (inputDto.DistributionChannelId > 0 ?_.udiv.DistributionChannelId == inputDto.DistributionChannelId : _.udiv.DistributionChannelId > 0) && (inputDto.DivisionId > 0 ?_.udiv.DivisionId == inputDto.DivisionId : _.udiv.DivisionId > 0))
                  .Select(_ => new { _.ud.y.u.Id, _.ud.y.u.Code, _.ud.y.u.Name /*TotalSaudaLimit = _.ud.y.u.SaudaLimit, _.ud.state.StateName,_.udiv.SalesOrganizationId,_.udiv.DistributionChannelId,_.udiv.DivisionId*//*, _.y.u.DivisionId, _.y.u.Division*/ }
                  );


                //if (inputDto.DivisionId > 0)
                //{

                //    //dealerContext = dealerContext.Where(_ => _.udiv.SalesOrganizationId == inputDto.SalesOrganizationId && _.udiv.DistributionChannelId == inputDto.DistributionChannelId && _.udiv.DivisionId == inputDto.DivisionId);
                //    //////var divisionmapping=dealerContext.Where(_ => _.u.Id);
                //    ////dealerContext = dealerContext//.Where(_ => _.DivisionId == inputDto.verticalIds)
                //    ////    .Select(s => s).Distinct();
                //}
                //else
                //{
                //    var dealerContext = _emamiContext.Users.AsNoTracking().Where(_ => _.IsActive && inputDto.StateIds.Contains(_.StateId) && dealerCode.Contains(_.Code))
                //   .Join(_emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.Dealer), u => u.Id, ur => ur.UserId, (u, ur) => new { u })
                //   //.Join(_emamiContext.UserCustomerMapping.AsNoTracking(), x => x.u.Id, uc => uc.CustomerId, (x, uc) => new { x.u, uc });
                //   //.Join(_emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.StateTrader), x => x.uc.UserId, ur => ur.UserId, (x, ur) => new { x.u, Employee = ur.User.Name });
                //   .Join(_emamiContext.State.AsNoTracking(), y => y.u.StateId, state => state.Id, (y, state) => new { y, state })
                //   .Join(_emamiContext.UserDivisionMappings.AsNoTracking(), ud => ud.y.u.Id, udiv => udiv.UserId, (ud, udiv) => new { ud, udiv })
                //   .Select(_ => new { _.udiv, _.ud.y.u.Id, _.ud.y.u.Code, _.ud.y.u.Name, TotalSaudaLimit = _.ud.y.u.SaudaLimit, _.ud.state.StateName/*, _.y.u.DivisionId, _.y.u.Division*/ }
                //   );
                //}
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var tomDate = currentDate.AddDays(1);
                var dealerlist = dealerContext.Distinct().ToList();
                if (dealerlist != null && dealerlist.Any())
                {
                    foreach (var item in dealerlist)
                    {
                        var creditMaster = _emamiContext.UserCreditMaster.AsNoTracking().Where(_ => _.UserId == item.Id).ToList();
                        var overduePaymentContext = _emamiContext.OverduePayment.AsNoTracking().Where(_ => _.UserId == item.Id);
                        if (creditMaster.IsAny())
                        {
                            var dto = new HANACreditMasterDto()
                            {
                                CustomerCode = item.Code,
                                CustomerName = item.Name,
                                CreditLimit = creditMaster.Sum(_ => _.CreditLimit) > 0 ? Math.Round((creditMaster.Sum(_ => _.CreditLimit) / 100000), 2) : 0,
                                CreditExposure = creditMaster.Sum(_ => _.CreditExposure) > 0 ? Math.Round((creditMaster.Sum(_ => _.CreditExposure) / 100000), 2):0,
                                TotalReceivable = creditMaster.Sum(_ => _.BillingDocumentValue) > 0 ? Math.Round((creditMaster.Sum(_ => _.BillingDocumentValue) / 100000), 2) : 0,
                                AvailableCreditLimit = creditMaster.Sum(_ => _.AvailableCreditLimit) > 0 ? Math.Round((creditMaster.Sum(_ => _.AvailableCreditLimit) / 100000), 2) : 0,
                                OpenExposure= Math.Round(((creditMaster.Sum(_ => _.OpenOrders) + creditMaster.Sum(_ => _.DeliveryValue)) / 100000), 2),
                                GrossExposure= Math.Round(((creditMaster.Sum(_ => _.OpenOrders) + creditMaster.Sum(_ => _.DeliveryValue) + creditMaster.Sum(_ => _.BillingDocumentValue)) / 100000), 2),
                                TomorrowsDue = overduePaymentContext.IsAny() ? overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) == DbFunctions.TruncateTime(tomDate)).DefaultIfEmpty().Sum(_ => (decimal?)_.Balance).GetValueOrDefault() : 0 ,
                                Overdue = overduePaymentContext.IsAny() ? overduePaymentContext.Where(_ => DbFunctions.TruncateTime(_.DueDate) <= DbFunctions.TruncateTime(currentDate)).DefaultIfEmpty().Sum(_ => (decimal?)_.Balance).GetValueOrDefault() : 0
                            };
                            creditLimitOutputDto.Add(dto);
                        }
                    }
                }
                if (inputDto.DataSourceRequest != null)
                {
                    var result = creditLimitOutputDto != null ? creditLimitOutputDto.Distinct().ToDataSourceResult(inputDto.DataSourceRequest) : creditLimitOutputDto.ToDataSourceResult(inputDto.DataSourceRequest);
                    return _resultService.SuccessObject(result);
                }
                else
                {
                    return _resultService.SuccessObject(creditLimitOutputDto);
                }
                
                //return _resultService.SuccessObject(creditLimitOutputDto.Distinct().ToList());
                

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }
        #endregion

        //#region Truck Placement Tracker Report

        //public ResultDto GetTruckPlacementTrackerReport(TruckReportInputDto inputDto)
        //{
        //    _methodName = "GetTruckPlacementTrackerReport";
        //    var outputDto = new List<TruckPlacementTrackerDto>();
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }
        //        if (inputDto.LoginUserId == 0)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserIdMissing);
        //        }
        //        var users = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
        //        if (users == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserNotFound);
        //        }
        //        if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
        //        {
        //            return _resultService.ErrorMessage(Constants.FromDateEmpty);
        //        }
        //        if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
        //        {
        //            return _resultService.ErrorMessage(Constants.ToDateEmpty);
        //        }

        //        IQueryable<User> userContext;
        //        userContext = _emamiContext.Users.Where(_ => (inputDto.VerticalId > 0 ? _.DivisionId == inputDto.VerticalId : _.DivisionId > 0));

        //        var dealerIds = new List<long>();
        //        if (inputDto.DealerIds.IsAny())
        //        {
        //            dealerIds = inputDto.DealerIds;
        //        }
        //        else if (inputDto.BdoIds.IsAny())
        //        {
        //            dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => inputDto.BdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
        //        }
        //        else if (inputDto.ZonalHeadIds.IsAny())
        //        {
        //            var bdoIds = userContext.Where(_ => _.OrganizationReportingToId != null && inputDto.ZonalHeadIds.Contains(_.OrganizationReportingToId.Value)).Select(_ => _.Id).ToList();
        //            dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
        //        }
        //        else if (inputDto.NationalHeadIds.IsAny())
        //        {
        //            var zonalHeadIds = userContext.Where(_ => _.OrganizationReportingToId != null && inputDto.NationalHeadIds.Contains(_.OrganizationReportingToId.Value)).Select(_ => _.Id).ToList();
        //            var bdoIds = userContext.Where(_ => _.OrganizationReportingToId != null && zonalHeadIds.Contains(_.OrganizationReportingToId.Value)).Select(_ => _.Id).ToList();
        //            dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
        //        }
        //        else
        //        {
        //            var nationalHeadIds = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.NationalTrader).Select(_ => _.UserId).ToList();
        //            var zonalHeadIds = userContext.Where(_ => _.OrganizationReportingToId != null && nationalHeadIds.Contains(_.OrganizationReportingToId.Value)).Select(_ => _.Id).ToList();
        //            var bdoIds = userContext.Where(_ => _.OrganizationReportingToId != null && zonalHeadIds.Contains(_.OrganizationReportingToId.Value)).Select(_ => _.Id).ToList();
        //            dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
        //        }

        //        var truckPlacementTrackerContext = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => dealerIds.Contains(_.UserId))
        //              .Join(_emamiContext.Users.Where(_ => (inputDto.VerticalId > 0 ? _.DivisionId == inputDto.VerticalId : _.DivisionId > 0)), l => l.UserId, u => u.Id, (l, u) => new { l, u })
        //              .Join(_emamiContext.TruckPlacementTracker.AsNoTracking().Where(_ => _.AppIndentDate != null
        //              && DbFunctions.TruncateTime(_.AppIndentDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
        //              && DbFunctions.TruncateTime(_.AppIndentDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
        //              && DbFunctions.TruncateTime(_.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
        //              && DbFunctions.TruncateTime(_.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
        //              , l => l.l.Id, t => t.AppIndentNo, (l, t) => new { t }).Select(_ => _.t);

        //        if (truckPlacementTrackerContext != null && truckPlacementTrackerContext.Any())
        //        {
        //            outputDto = truckPlacementTrackerContext
        //                .Select(_ => new TruckPlacementTrackerDto()
        //                {
        //                    Id = _.Id,
        //                    Plant = _.Plant,
        //                    PlantOrDepotDesc = _.PlantOrDepotDesc,
        //                    AppIndentNo = _.AppIndentNo,
        //                    AppIndentDate = _.AppIndentDate,
        //                    AppIndentTime = _.AppIndentTime,
        //                    InquiryNo = _.InquiryNo,
        //                    InquiryDate = _.InquiryDate,
        //                    InquiryTime = _.InquiryTime,
        //                    CreationDate = _.CreationDate,
        //                    CreationTime = _.CreationTime,
        //                    ContractNumber = _.ContractNumber,
        //                    ContractValidFromDate = _.ContractValidFromDate,
        //                    DONo = _.DONo,
        //                    DOCreationDate = _.DOCreationDate,
        //                    DOCreationTime = _.DOCreationTime,
        //                    Incoterms = _.Incoterms,
        //                    VehicleType = _.VehicleType,
        //                    VehicleCapacity = _.VehicleCapacity,
        //                    TruckIndentNo = _.TruckIndentNo,
        //                    TruckReleaseDate = _.TruckReleaseDate,
        //                    TruckReleaseTime = _.TruckReleaseTime,
        //                    RevisedTruckIndentReleaseDate = _.RevisedTruckIndentReleaseDate,
        //                    RevisedTruckIndentReleaseTime = _.RevisedTruckIndentReleaseTime,
        //                    DespatchNo = _.DespatchNo,
        //                    DPCeationDate = _.DPCeationDate,
        //                    DPCeationTime = _.DPCeationTime,
        //                    VehicleReportingDate = _.VehicleReportingDate,
        //                    VehicleReportingTime = _.VehicleReportingTime,
        //                    GateInDate = _.GateInDate,
        //                    GateInTime = _.GateInTime,
        //                    VehicleInDate = _.VehicleInDate,
        //                    VehicleInTime = _.VehicleInTime,
        //                    BillToParty = _.BillToParty,
        //                    BillToPartyName = _.BillToPartyName,
        //                    ShipToParty = _.ShipToParty,
        //                    ShipToPartyName = _.ShipToPartyName,
        //                    Destination = _.Destination,
        //                    City = _.City,
        //                    DestinationState = _.DestinationState,
        //                    DestStateDescription = _.DestStateDescription,
        //                    SKUCode = _.SKUCode,
        //                    SKUDescription = _.SKUDescription,
        //                    PrimaryTransporterCode = _.PrimaryTransporterCode,
        //                    PrimaryTransporterName = _.PrimaryTransporterName,
        //                    PrimaryTransporterVehicleNumber = _.PrimaryTransporterVehicleNumber,
        //                    PrimaryTransporterIndentDate = _.PrimaryTransporterIndentDate,
        //                    PrimaryTransporterIndentTime = _.PrimaryTransporterIndentTime,
        //                    DoCreationHeaderStatus = _.DoCreationHeaderStatus,
        //                    InvoiceNumber = _.InvoiceNumber,
        //                    InvoiceDate = _.InvoiceDate,
        //                    InvoiceTime = _.InvoiceTime,
        //                    VehicleOutDate = _.VehicleOutDate,
        //                    VehicleOutTime = _.VehicleOutTime,
        //                }).ToList();
        //        }

        //        if (outputDto != null && outputDto.Any())
        //        {
        //            return _resultService.SuccessObject(outputDto);
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

        ////public ResultDto GetTruckPlacementTrackerReportAPP(TruckReportInputDto inputDto)
        ////{
        ////    _methodName = "GetTruckPlacementTrackerReportAPP";
        ////    var outputDto = new TruckPlacementTrackerAPPDto();
        ////    outputDto.TruckPlacementTrackerList = new List<TruckPlacementTrackerListDto>();
        ////    try
        ////    {
        ////        if (inputDto == null)
        ////        {
        ////            return _resultService.ErrorMessage(Constants.InvalidRequest);
        ////        }
        ////        if (inputDto.LoginUserId == 0)
        ////        {
        ////            return _resultService.ErrorMessage(Constants.UserIdMissing);
        ////        }
        ////        var users = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId && _.IsActive);
        ////        if (users == null)
        ////        {
        ////            return _resultService.ErrorMessage(Constants.UserNotFound);
        ////        }
        ////        if (inputDto.FromDate == null || inputDto.FromDate == DateTime.MinValue)
        ////        {
        ////            return _resultService.ErrorMessage(Constants.FromDateEmpty);
        ////        }
        ////        if (inputDto.ToDate == null || inputDto.ToDate == DateTime.MinValue)
        ////        {
        ////            return _resultService.ErrorMessage(Constants.ToDateEmpty);
        ////        }

        ////        IQueryable<User> userContext;
        ////        userContext = _emamiContext.Users.Where(_ => (inputDto.VerticalId > 0 ? _.DivisionId == inputDto.VerticalId : _.DivisionId > 0));

        ////        var dealerIds = new List<long>();
        ////        if (inputDto.DealerIds.IsAny())
        ////        {
        ////            dealerIds = inputDto.DealerIds;
        ////        }
        ////        else if (inputDto.BdoIds.IsAny())
        ////        {
        ////            dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => inputDto.BdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
        ////        }
        ////        else if (inputDto.ZonalHeadIds.IsAny())
        ////        {
        ////            var bdoIds = userContext.Where(_ => _.OrganizationReportingToId != null && inputDto.ZonalHeadIds.Contains(_.OrganizationReportingToId.Value)).Select(_ => _.Id).ToList();
        ////            dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
        ////        }
        ////        else if (inputDto.NationalHeadIds.IsAny())
        ////        {
        ////            var zonalHeadIds = userContext.Where(_ => _.OrganizationReportingToId != null && inputDto.NationalHeadIds.Contains(_.OrganizationReportingToId.Value)).Select(_ => _.Id).ToList();
        ////            var bdoIds = userContext.Where(_ => _.OrganizationReportingToId != null && zonalHeadIds.Contains(_.OrganizationReportingToId.Value)).Select(_ => _.Id).ToList();
        ////            dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
        ////        }
        ////        else
        ////        {
        ////            var nationalHeadIds = _emamiContext.UserRoles.AsNoTracking().Where(_ => _.RoleId == (int)DTO.Enums.Role.NationalTrader).Select(_ => _.UserId).ToList();
        ////            var zonalHeadIds = userContext.Where(_ => _.OrganizationReportingToId != null && nationalHeadIds.Contains(_.OrganizationReportingToId.Value)).Select(_ => _.Id).ToList();
        ////            var bdoIds = userContext.Where(_ => _.OrganizationReportingToId != null && zonalHeadIds.Contains(_.OrganizationReportingToId.Value)).Select(_ => _.Id).ToList();
        ////            dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoIds.Contains(_.UserId)).Select(_ => _.CustomerId).ToList();
        ////        }

        ////        //var lifting = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => dealerIds.Contains(_.UserId)).ToList();
        ////        var truckPlacementTrackerContext = _emamiContext.LiftingRequest.AsNoTracking().Where(_ => dealerIds.Contains(_.UserId))
        ////              .Join(_emamiContext.Users.Where(_ => (inputDto.VerticalId > 0 ? _.DivisionId == inputDto.VerticalId : _.DivisionId > 0)), l => l.UserId, u => u.Id, (l, u) => new { l, u })
        ////              .Join(_emamiContext.TruckPlacementTracker.AsNoTracking().Where(_ => _.AppIndentDate != null
        ////              && DbFunctions.TruncateTime(_.AppIndentDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
        ////              && DbFunctions.TruncateTime(_.AppIndentDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
        ////              && DbFunctions.TruncateTime(_.InvoiceDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
        ////              && DbFunctions.TruncateTime(_.InvoiceDate) <= DbFunctions.TruncateTime(inputDto.ToDate))
        ////              , l => l.l.Id, t => t.AppIndentNo, (l, t) => new { t }).Select(_ => _.t).ToList();


        ////        if (truckPlacementTrackerContext != null && truckPlacementTrackerContext.Any())
        ////        {
        ////            var pageSize = Constants.PageSize;
        ////            var skip = pageSize * inputDto.PageNo;

        ////            outputDto.ListCount = truckPlacementTrackerContext.Count();

        ////            outputDto.TruckPlacementTrackerList = truckPlacementTrackerContext
        ////                .Select(_ => new TruckPlacementTrackerListDto()
        ////                {
        ////                    Id = _.Id,
        ////                    Plant = _.Plant,
        ////                    PlantOrDepotDesc = _.PlantOrDepotDesc,
        ////                    AppIndentNo = _.AppIndentNo, // Lifting Request Table - Id(AppIndentNo), Dealer(UserId), Vertical(Dealer Vertical - Users Table)
        ////                    AppIndentDate = _.AppIndentDate,
        ////                    InquiryNo = _.InquiryNo,
        ////                    InquiryDate = _.InquiryDate,
        ////                    InquiryTime = _.InquiryTime,
        ////                    DONo = _.DONo,
        ////                    DOCreationDate = _.DOCreationDate,
        ////                    DOCreationTime = _.DOCreationTime,
        ////                    VehicleCapacity = _.VehicleCapacity,
        ////                    GateInDate = _.GateInDate,
        ////                    GateInTime = _.GateInTime,
        ////                    InvoiceNumber = _.InvoiceNumber,
        ////                    InvoiceDate = _.InvoiceDate,
        ////                    InvoiceTime = _.InvoiceTime,
        ////                    PrimaryTransporterVehicleNumber = _.PrimaryTransporterVehicleNumber,
        ////                }).OrderByDescending(_ => _.Id).ToList().Skip(skip).Take(pageSize).ToList();
        ////        }

        ////        return _resultService.SuccessObject(outputDto);

        ////    }
        ////    catch (Exception exception)
        ////    {
        ////        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        ////        _logger.Error(message);
        ////        return _resultService.ErrorMessage(Constants.Exception);
        ////    }
        ////}

        //#endregion

        public ResultDto GetPendingContractReportForManagerAPP(PendingContractReportInputDto inputDto)
        {
            try
            {
                var skuoutputListDto = new List<PendingContractSkuOutputDto>();
                var output = new PendingContractOutputDto();
                List<long> bdoList = new List<long>();
                output.CurrentDate = DateHelper.UtcToIndia(DateTime.UtcNow);

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
                if (inputDto.BdoIds.IsAny())
                {
                    bdoList = inputDto.BdoIds;
                }
                else if (inputDto.ZonalHeadIds.IsAny())
                {
                    bdoList = _emamiContext.Users.AsNoTracking().Where(user => inputDto.ZonalHeadIds.Contains(user.ReportingToId ?? 0)).Select(a => a.Id).ToList();
                }
                //else if (inputDto.NationalHeadIds.IsAny())
                //{
                //    var zonalHeadIds = _emamiContext.Users.AsNoTracking().Where(user => inputDto.NationalHeadIds.Contains(user.OrganizationReportingToId ?? 0)).Select(a => a.Id).ToList();
                //    bdoList = _emamiContext.Users.AsNoTracking().Where(user => zonalHeadIds.Contains(user.OrganizationReportingToId ?? 0)).Select(a => a.Id).ToList();
                //}
                else
                {
                    //var nationalHeadIds = _emamiContext.UserRoles.AsNoTracking().Where(roles => roles.RoleId == (int)DTO.Enums.Role.NationalTrader).Select(a => a.UserId).ToList();
                    var zonalHeadIds = _emamiContext.UserRoles.AsNoTracking().Where(roles => roles.RoleId == (int)DTO.Enums.Role.ZonalTrader).Select(a => a.UserId).ToList();
                    bdoList = _emamiContext.Users.AsNoTracking().Where(user => zonalHeadIds.Contains(user.ReportingToId ?? 0)).Select(a => a.Id).ToList();

                }
                //IQueryable<UserCustomerMapping> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId));
                //var bdoDetails = _emamiContext.Users.AsNoTracking().Where(x => bdoList.Contains(x.Id)).Select(x => new { x.Id, x.Name });
                var dealerDetails = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => bdoList.Contains(_.UserId)).Select(x => new { x.UserId, x.User.Name, x.CustomerId });

                var saudaOrdersContext = new List<PendingContractReportSaudaOrderContextDto>();
                var saudaStatus = Constants.OverallSaudaStatus;
                if ((inputDto.SkuId == null && inputDto.StateIds == null))
                {
                    inputDto.SkuId = _emamiContext.Skus.AsNoTracking().Where(_ => _.IsActive).Select(s => s.Id).ToList();
                    var StateIds = _emamiContext.State.AsNoTracking().Where(_ => _.IsActive).Select(s => s.Id).ToList();
                    inputDto.StateIds = StateIds.Select(a => (long)a).ToList();
                    saudaOrdersContext = (from pc in _emamiContext.PendingContracts.AsNoTracking()
                                          join u in _emamiContext.Users.AsNoTracking() on pc.CustomerCode equals u.Code
                                          //join udm in _emamiContext.UserDivisionMappings.AsNoTracking() on u.Id equals udm.UserId
                                          join sku in _emamiContext.Skus.AsNoTracking() on pc.MaterialCode equals sku.SkuCode
                                          join bd in dealerDetails on u.Id equals bd.CustomerId
                                          where //dealersList.Any(a => a.CustomerId == u.Id) &&
                                             (inputDto.SkuId.Contains(sku.Id))
                                          && (inputDto.StateIds.Contains(u.StateId))
                                          && pc.SalesOrgId == sku.SalesOrganizationId && pc.DistChnlId == sku.DistributionChannelId
                                          && pc.DivisionId == sku.DivisionId
                                          //&& sku.DivisionId == udm.DivisionId
                                          select new PendingContractReportSaudaOrderContextDto
                                          {
                                              UserId = u.Id,
                                              BdoId = bd.UserId,
                                              BdoName = bd.Name,
                                             // BiddingDate = pc.SaudaDate ?? DateTime.Now,
                                              BidQuantity = pc.SaudaQuantity,
                                              Id = pc.Id,
                                              SkuId = sku.Id,
                                              OilTypeId = sku.OilTypeId ?? 0,
                                              BidQuantityCase = pc.PendingQuantityInCase,
                                              SkuName = sku.SkuName,
                                             // Rate = pc.BasicRateAfterDiscount
                                          }).ToList();
                }
                else if (inputDto.SkuId != null && inputDto.StateIds == null)
                {
                    saudaOrdersContext = (from pc in _emamiContext.PendingContracts.AsNoTracking()
                                          join u in _emamiContext.Users.AsNoTracking() on pc.CustomerCode equals u.Code
                                          //join udm in _emamiContext.UserDivisionMappings.AsNoTracking() on u.Id equals udm.UserId
                                          join sku in _emamiContext.Skus.AsNoTracking() on pc.MaterialCode equals sku.SkuCode
                                          join bd in dealerDetails on u.Id equals bd.CustomerId
                                          where //dealersList.Any(a => a.CustomerId == u.Id) &&
                                             (inputDto.SkuId.Contains(sku.Id))
                                             && pc.SalesOrgId == sku.SalesOrganizationId && pc.DistChnlId == sku.DistributionChannelId
                                          && pc.DivisionId == sku.DivisionId
                                          // && sku.DivisionId == udm.DivisionId
                                          select new PendingContractReportSaudaOrderContextDto
                                          {
                                              UserId = u.Id,
                                              BdoId = bd.UserId,
                                              BdoName = bd.Name,
                                          //    BiddingDate = pc.SaudaDate ?? DateTime.Now,
                                              BidQuantity = pc.SaudaQuantity,
                                              Id = pc.Id,
                                              SkuId = sku.Id,
                                              OilTypeId = sku.OilTypeId ?? 0,
                                              BidQuantityCase = pc.PendingQuantityInCase,
                                              SkuName = sku.SkuName,
                                             // Rate = pc.BasicRateAfterDiscount
                                          }).ToList();
                }
                else if (inputDto.StateIds != null && inputDto.SkuId == null)
                {
                    saudaOrdersContext = (from pc in _emamiContext.PendingContracts.AsNoTracking()
                                          join u in _emamiContext.Users.AsNoTracking() on pc.CustomerCode equals u.Code
                                          //join udm in _emamiContext.UserDivisionMappings.AsNoTracking() on u.Id equals udm.UserId
                                          join sku in _emamiContext.Skus.AsNoTracking() on pc.MaterialCode equals sku.SkuCode
                                          join bd in dealerDetails on u.Id equals bd.CustomerId
                                          where //dealersList.Any(a => a.CustomerId == u.Id) &&
                                             (inputDto.SkuId.Contains(sku.Id))
                                          //&& saudaStatus.Contains(so.StatusId)
                                          && (inputDto.StateIds.Contains(u.StateId))
                                         // && sku.DivisionId == udm.DivisionId
                                         && pc.SalesOrgId == sku.SalesOrganizationId && pc.DistChnlId == sku.DistributionChannelId
                                          && pc.DivisionId == sku.DivisionId
                                          select new PendingContractReportSaudaOrderContextDto
                                          {
                                              UserId = u.Id,
                                              BdoId = bd.UserId,
                                              BdoName = bd.Name,
                                            //  BiddingDate = pc.SaudaDate ?? DateTime.Now,
                                              BidQuantity = pc.SaudaQuantity,
                                              Id = pc.Id,
                                              SkuId = sku.Id,
                                              OilTypeId = sku.OilTypeId ?? 0,
                                              BidQuantityCase = pc.PendingQuantityInCase,
                                              SkuName = sku.SkuName,
                                             // Rate = pc.BasicRateAfterDiscount
                                          }).ToList();

                }
                else
                {
                    saudaOrdersContext = (from pc in _emamiContext.PendingContracts.AsNoTracking()
                                          join u in _emamiContext.Users.AsNoTracking() on pc.CustomerCode equals u.Code
                                         // join udm in _emamiContext.UserDivisionMappings.AsNoTracking() on u.Id equals udm.UserId
                                          join sku in _emamiContext.Skus.AsNoTracking() on pc.MaterialCode equals sku.SkuCode
                                          join bd in dealerDetails on u.Id equals bd.CustomerId
                                          where //dealersList.Any(a => a.CustomerId == u.Id) &&
                                             (inputDto.SkuId.Contains(sku.Id))
                                          //&& saudaStatus.Contains(so.StatusId)
                                          && (inputDto.SkuId.Contains(sku.Id))
                                          && (inputDto.StateIds.Contains(u.StateId))
                                          && pc.SalesOrgId == sku.SalesOrganizationId && pc.DistChnlId == sku.DistributionChannelId
                                          && pc.DivisionId == sku.DivisionId
                                          // && sku.DivisionId == udm.DivisionId
                                          select new PendingContractReportSaudaOrderContextDto
                                          {
                                              UserId = u.Id,
                                              BdoId = bd.UserId,
                                              BdoName = bd.Name,
                                          //    BiddingDate = pc.SaudaDate ?? DateTime.Now,
                                              BidQuantity = pc.SaudaQuantity,
                                              Id = pc.Id,
                                              SkuId = sku.Id,
                                              OilTypeId = sku.OilTypeId ?? 0,
                                              BidQuantityCase = pc.PendingQuantityInCase,
                                              SkuName = sku.SkuName,
                                           //   Rate = pc.BasicRateAfterDiscount
                                          }).ToList();

                }

                if (saudaOrdersContext != null && saudaOrdersContext.Any())
                {
                    if (!inputDto.isGroupByBdo) //group by SKU
                    {
                        foreach (var item in saudaOrdersContext)
                        {
                            var skudto = new PendingContractSkuOutputDto()
                            {
                                SkuId = item.SkuId,
                                Sku = item.SkuName,
                                QuantityInCase = item.BidQuantityCase,
                                QuantityInMT = item.BidQuantity
                            };
                            skuoutputListDto.Add(skudto);
                        }
                        if (skuoutputListDto != null && skuoutputListDto.Any())
                        {
                            var outputlist = skuoutputListDto.GroupBy(g => g.SkuId).Select(s => new PendingContractSkuOutputDto
                            {
                                SkuId = s.First().SkuId,
                                Sku = s.First().Sku,
                                QuantityInCase = s.Sum(c => c.QuantityInCase),
                                QuantityInMT = s.Sum(c => c.QuantityInMT)
                            }).ToList();
                            //output.PendingContractSkuOutput = outputlist.Where(_ => _.QuantityInCase != 0).ToList();
                        }
                    }
                    else //group by StateTrader,SKU
                    {
                        foreach (var item in saudaOrdersContext)
                        {
                            var skudto = new PendingContractSkuOutputDto()
                            {
                                BdoId = item.BdoId,
                                BdoName = item.BdoName,
                                SkuId = item.SkuId,
                                Sku = item.SkuName,
                                QuantityInCase = item.BidQuantityCase,
                                QuantityInMT = item.BidQuantity,
                                Rate = item.Rate
                            };
                            skuoutputListDto.Add(skudto);
                        }
                        if (skuoutputListDto != null && skuoutputListDto.Any())
                        {
                            var outputlist = skuoutputListDto.GroupBy(g => new { g.SkuId, g.BdoId }).Select(s => new PendingContractSkuOutputDto
                            {
                                BdoId = s.First().BdoId,
                                BdoName = s.First().BdoName,
                                SkuId = s.First().SkuId,
                                Sku = s.First().Sku,
                                QuantityInCase = s.Sum(c => c.QuantityInCase),
                                QuantityInMT = s.Sum(c => c.QuantityInMT),
                                Rate = s.Sum(c => c.Rate)
                            }).ToList();
                         //   output.PendingContractSkuOutput = outputlist.Where(_ => _.QuantityInCase != 0).ToList();

                            output.TotalQuantityInCase = outputlist.Sum(x => x.QuantityInCase);
                            output.TotalQuantityInMT = outputlist.Sum(x => x.QuantityInMT);
                        }
                    }
                }

                if (output != null)
                {
                    return _resultService.SuccessObject(output);
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

        #region Sauda Report for Mobile

        //public ResultDto GetSaudaOrderDetailsReportForMobile(SaudaOrderReportInputputDto inputDto)
        //{
        //    _methodName = "GetSaudaOrderDetailsReportForMobile";
        //    var saudaList = new List<SaudaOrderReportOutputDto>();
        //    var resultDto = new ResultDto();
        //    var outputDto = new SaudaOrderListOutputDto();
        //    _logger.Info($"{ServiceName} Service-Method - {_methodName} Started");

        //    try
        //    {

        //        var userContext = _emamiContext.Users.AsNoTracking();
        //        if (inputDto.StateIds != null && inputDto.StateIds.Any())
        //        {
        //            userContext = userContext.Where(_ => inputDto.StateIds.Contains(_.StateId));
        //        }
        //        var saudaOrderList = _emamiContext.Sauda.AsNoTracking()
        //            .Join(_emamiContext.SaudaOrders.AsNoTracking(), s => s.Id, so => so.SaudaId, (s, so) =>
        //            new
        //            {
        //                Sauda = new { UserId = s.UserId, BiddingDate = s.BiddingDate, SaudaBookingType = s.SaudaBookingType.Name, SaudaBookingTypeId = s.SaudaBookingTypeId },
        //                SaudaOrders = new
        //                {
        //                    Id = so.Id,
        //                    SaudaId = so.SaudaId,
        //                    DiscountTypeId = so.DiscountTypeId,
        //                    DiscountAmount = so.DiscountAmount,
        //                    BidQuantityCase = so.BidQuantityCase,
        //                    SkuId = so.SkuId,
        //                    Incoterms2 = so.Incoterms2,
        //                    BrokerId = so.BrokerId,
        //                    SkuName = so.Sku.SkuName,
        //                    SkuCode = so.Sku.SkuCode,
        //                    BidPrice = so.BidPrice,
        //                    SaudaNumber = so.SaudaNumber,
        //                    ValidFromDate = so.ValidFromDate,
        //                    ValidToDate = so.ValidToDate,
        //                    BidQuantity = so.BidQuantity,
        //                    PackTypeName = so.Sku.PackType.Name,
        //                    PackGroupName = so.Sku.PackGroup.Name,
        //                    VerticalName = so.OilType.Division.Name,
        //                    PricingId = so.PricingId,
        //                    StatusId = so.StatusId,
        //                    SkuQuantity = so.Sku.Quantity,
        //                    SkuUom = so.Sku.Uom.Name,
        //                    //Proo = so.Proo,
        //                    //Frc1 = so.Frc1,
        //                    SpecialRateId = so.SpecialRateRequestId,
        //                    QuotedPrice = so.QuotedPrice,
        //                    VerticalId = so.OilType.DivisionId,
        //                    Remarks = so.Remarks,
        //                    OilType = so.OilType.Name,
        //                  //  TradeTicketNo = so.TradeTicketNumber,
        //                    LitreConversion = so.OilType.LitreConversion,
        //                    //MaterialType = so.Sku.MaterialType.Name, 
        //                    //VolumeDiscount = so.VolumeDiscount,
        //                    //SchemeDiscountCase = so.SchemeDiscountCase,
        //                    //SkuDiscountCase = so.SkuDiscountCase,
        //                    //GPBenefitTypeId = so.GPBenefitType,
        //                    //so.GPBenefitDiscountOrDay,
        //                    //SurpriseBenefitTypeId = so.SurpriseBenefitType,
        //                    //SurpriseBenefitDiscountOrDay = so.SurpriseBenefitDiscountOrDay,
        //                    BaseRate = so.BaseRate,
        //                    //VolumeDiscountCase = so.VolumeDiscountCase,
        //                    //so.GPBenefitDiscountInCase,
        //                    //so.SurpriseBenefitDiscountInCase,
        //                    so.BidPriceBeforeDiscount,
        //                    so.IsBaseSauda,
        //                    so.BaseSkuBidPrice
        //                }
        //            }).Join(_emamiContext.Pricing.AsNoTracking(), so => so.SaudaOrders.PricingId, p => p.Id, (so, p) =>
        //            new
        //            {
        //                so.SaudaOrders,
        //                so.Sauda,
        //                Pricing = new
        //                {
        //                    PlantId = p.PlantId,
        //                    //Discount = p.Discount, Premium = p.Premium, StateId = p.StateId, MaterialCost = p.MaterialCost, Margin = p.Margin, CushionMargin = p.CushionMargin, RaMargin = p.RaMargin, PackingCost = p.PackingCost, SchemeCostRecovery = p.SchemeCostRecovery, HoneycombCost = p.HoneycombCost, PrimaryFrieght = p.PrimaryFrieght, SecondaryFrieght = p.SecondaryFrieght, PlantSecondaryFrieght = p.PlantSecondaryFrieght, DepotCost = p.DepotCost, DetentionCost = p.DetentionCost, AdditionalCost = p.AdditionalCost, OilTransferCostForPlant = p.OilTransferCostForPlant, OilTransferCostForDepot = p.OilTransferCostForDepot, CustomerGroupMargin = p.CustomerGroupMargin, PlantGSTPercentage = p.PlantGSTPercentage, DepotGSTPercentage = p.DepotGSTPercentage,
        //                    sku = p.Sku
        //                },
        //            }).Join(userContext, s => s.Sauda.UserId, u => u.Id, (s, u) =>
        //            new
        //            { s.Sauda, s.SaudaOrders, s.Pricing, User = new { StateId = u.StateId, Code = u.Code, Name = u.Name, /* FreightRouteName = u.FreightRoute.Name, */ Id = u.Id,/* CustomerGroupOneId = u.CustomerGroupOneId, CustomerGroupTwoId = u.CustomerGroupTwoId */} })
        //            .Where(w => DbFunctions.TruncateTime(w.Sauda.BiddingDate) >= DbFunctions.TruncateTime(inputDto.FromDate)
        //            && DbFunctions.TruncateTime(w.Sauda.BiddingDate) <= DbFunctions.TruncateTime(inputDto.ToDate)
        //             && (inputDto.VerticalId > 0 ? w.SaudaOrders.VerticalId == inputDto.VerticalId : w.SaudaOrders.VerticalId > 0))
        //            .Select(s => s).ToList();

        //        if (inputDto.StatusIds != null && inputDto.StatusIds.Count > 0)
        //        {
        //            if (inputDto.StatusIds.Contains(-1))
        //            {
        //                saudaOrderList = saudaOrderList.ToList();
        //            }
        //            else
        //            {
        //                saudaOrderList = saudaOrderList.Where(_ => inputDto.StatusIds.Contains(_.SaudaOrders.StatusId)).ToList();
        //            }
        //        }

        //        saudaOrderList.RemoveAll(item => item.Pricing.PlantId == 0 && (item.Pricing.sku.DivisionId == (int)DTO.Enums.Division.Hbc || item.Pricing.sku.DivisionId == (int)DTO.Enums.Division.SpecialityFat));

        //        if (saudaOrderList != null && saudaOrderList.Any())
        //        {
        //            #region Common Data's
        //            var specialRateId = saudaOrderList.Select(s => s.SaudaOrders.SpecialRateId).Distinct().ToList();
        //            var SpecialRateDatas = _emamiContext.SpecialRate.AsNoTracking().Where(_ => specialRateId.Contains(_.Id))
        //                .Select(s => new
        //                {
        //                    Id = s.Id,
        //                    IsLTD = s.IsLTD
        //                }).ToList();

        //            var tradeTicketNos = saudaOrderList.Select(s => s.SaudaOrders.TradeTicketNo).Distinct().ToList();
        //            var TradeTicketDatas = _emamiContext.TradeTicket.AsNoTracking().Where(_ => tradeTicketNos.Contains(_.TradeTicketNumber))
        //                .Select(s => new
        //                {
        //                    TotalCost = s.TotalCost,
        //                    TradeTicketNumber = s.TradeTicketNumber
        //                }).ToList();

        //            var skuIds = saudaOrderList.Select(s => s.SaudaOrders.SkuId).Distinct().ToList();
        //            var SkuUomMappingDatas = _emamiContext.SkuUomMapping
        //                .Where(_ => skuIds.Contains(_.SkuId) && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)(int)DTO.Enums.Uom.Nos)
        //                .Select(s => new
        //                {
        //                    SkuId = s.SkuId,
        //                    UomId = s.UomId,
        //                    RelationUomId = s.RelationUomId,
        //                    // ConversionFactor = s.ConversionFactor
        //                }).ToList();

        //            var SkuDatas = _emamiContext.Skus.AsNoTracking().Where(w => skuIds.Contains(w.Id))
        //                .Select(s => new
        //                {
        //                    Id = s.Id,
        //                    UomId = s.UomId,
        //                    LitreConversion = s.OilType.LitreConversion,
        //                    Quantity = s.Quantity
        //                });

        //            var brokerIds = saudaOrderList.Select(s => s.SaudaOrders.BrokerId).Distinct().ToList();
        //            var UserDatas = _emamiContext.Users.AsNoTracking().Where(w => brokerIds.Contains(w.Id))
        //                .Select(s => new
        //                {
        //                    Id = s.Id,
        //                    Name = s.Name,
        //                    Code = s.Code
        //                }).ToList();


        //            var saudaUserIds = saudaOrderList.Select(s => s.Sauda.UserId).Distinct().ToList();
        //            var BdoDatas = _emamiContext.Users.AsNoTracking()
        //                .Join(_emamiContext.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { User = u, UserRoles = ur })
        //                .Join(_emamiContext.UserCustomerMapping.AsNoTracking(), us => us.User.Id, ucm => ucm.CustomerId, (us, ucm) => new { us.User, us.UserRoles, UserCustomer = ucm })
        //                .Where(w => w.UserRoles.RoleId == (long)DTO.Enums.Role.StateTrader && saudaUserIds.Contains(w.UserCustomer.CustomerId))
        //                .Select(s => new
        //                {
        //                    Id = s.User.Id,
        //                    Name = s.User.Name,
        //                    Code = s.User.Code
        //                }).ToList();

        //            //var customerGroupOneIds = saudaOrderList.Select(s => s.User.CustomerGroupOneId).ToList();
        //            //var customerGroupTwoIds = saudaOrderList.Select(s => s.User.CustomerGroupTwoId).ToList();

        //            //var CustomerGroupOneDatas = _emamiContext.CustomerGroupOne.AsNoTracking().Where(w => customerGroupOneIds.Contains(w.Id))
        //            //    .Select(s => new
        //            //    {
        //            //        Id = s.Id,
        //            //        Name = s.GroupName
        //            //    }).ToList();

        //            //var CustomerGroupTwoDatas = _emamiContext.CustomerGroupTwo.AsNoTracking().Where(w => customerGroupTwoIds.Contains(w.Id))
        //            //    .Select(s => new
        //            //    {
        //            //        Id = s.Id,
        //            //        Name = s.GroupName
        //            //    }).ToList();

        //            #endregion

        //            var depotContext = _emamiContext.Depots.AsNoTracking();

        //            foreach (var s in saudaOrderList)
        //            {
        //                if (s.SaudaOrders.BidQuantityCase <= 0)
        //                {
        //                    continue;
        //                }
        //                decimal raPremiumWithtax = 0;
        //                decimal raPremiumWithoutTax = 0;
        //                decimal allocationPremiumWithtax = 0;
        //                decimal allocationPremiumWithoutTax = 0;
        //                decimal raTotalDiscount = 0;
        //                decimal saleRate = 0;
        //                //decimal honeycombCost = s.Pricing.HoneycombCost;
        //                decimal discount = 0, premium = 0, LtdValue = 0, specialRate = 0, specialRateDiscount = 0;
        //                bool isLtd = false;
        //                if (s.SaudaOrders.SpecialRateId > 0)
        //                {
        //                    //isLtd = _emamiContext.SpecialRate.AsNoTracking().FirstOrDefault(_ => _.Id == s.SaudaOrders.SpecialRateId).IsLTD;
        //                    if (SpecialRateDatas != null && SpecialRateDatas.Any())
        //                        isLtd = SpecialRateDatas.FirstOrDefault(_ => _.Id == s.SaudaOrders.SpecialRateId) != null ?
        //                            SpecialRateDatas.FirstOrDefault(_ => _.Id == s.SaudaOrders.SpecialRateId).IsLTD : false;

        //                    var result = s.SaudaOrders.BidQuantityCase > 0 ? (s.SaudaOrders.QuotedPrice - s.SaudaOrders.BidPrice) / s.SaudaOrders.BidQuantityCase : 0;
        //                    if (result >= 0)
        //                    {
        //                        specialRateDiscount = result;
        //                    }
        //                    else
        //                    {
        //                        premium = -(result);
        //                    }
        //                    if (isLtd)
        //                    {
        //                        LtdValue = specialRateDiscount;
        //                    }
        //                    else
        //                    {
        //                        specialRate = specialRateDiscount;
        //                    }
        //                }
        //                else
        //                {
        //                    discount = s.SaudaOrders.DiscountTypeId == 1 ? CalculateOneCase(s.SaudaOrders.DiscountAmount, s.SaudaOrders.BidQuantityCase) : 0;
        //                    premium = s.SaudaOrders.DiscountTypeId == 2 ? CalculateOneCase(s.SaudaOrders.DiscountAmount, s.SaudaOrders.BidQuantityCase) : 0;
        //                }

        //                //if (s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ExPlant || s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ExDepot)
        //                //    honeycombCost = 0;

        //                //SaleRate
        //                var PR00 = s.SaudaOrders.Proo //> 0
        //                                              //? s.SaudaOrders.Proo
        //                                              //: ((s.Pricing.MaterialCost
        //                                              //+ s.Pricing.PackingCost
        //                                              //+ honeycombCost
        //                                              //+ (s.Sauda.SaudaBookingTypeId == (long)DTO.Enums.SaudaBookingTypes.TraditionalProcess ? (s.Pricing.Margin + s.Pricing.CushionMargin) : s.Pricing.RaMargin)
        //                                              //+ s.Pricing.SchemeCostRecovery
        //                    + premium
        //                    //+ s.Pricing.AdditionalCost) 
        //                    - (discount + LtdValue + specialRate);

        //                var FRC1 = s.SaudaOrders.Frc1 > 0 ? s.SaudaOrders.Frc1
        //                   : Utility.CalculateFRC1(0, 0, 0, 0,
        //                    //s.Pricing.PrimaryFrieght, s.Pricing.SecondaryFrieght, s.Pricing.DepotCost, s.Pricing.DetentionCost, 
        //                    s.SaudaOrders.Incoterms2, 0, 0
        //                    //s.Pricing.PlantSecondaryFrieght, s.Pricing.OilTransferCostForPlant
        //                    );

        //                decimal sRate = 0;
        //                decimal taxPaidValue = 0;
        //                decimal saleRateWithTax = 0;
        //                decimal discountGstPercentage = 0;
        //                decimal discountWithTax = 0;
        //                decimal discountTaxAmount = 0;

        //                //if (s.Sauda.SaudaBookingTypeId == (long)DTO.Enums.SaudaBookingTypes.ReverseAuction)
        //                //{
        //                //    //RA2.0 Changes
        //                //    //raPremiumWithtax = (s.SaudaOrders.BidPrice / s.SaudaOrders.BidQuantityCase) - s.SaudaOrders.BaseRate;
        //                //    if (s.SaudaOrders.IsBaseSauda)
        //                //    {
        //                //        raPremiumWithtax = s.SaudaOrders.BidPriceBeforeDiscount - s.SaudaOrders.BaseRate;
        //                //    }
        //                //    else
        //                //    {
        //                //        raPremiumWithtax = s.SaudaOrders.BaseSkuBidPrice - s.SaudaOrders.BaseRate;
        //                //    }

        //                //    decimal bidPricePerCause = (s.SaudaOrders.BidPrice / s.SaudaOrders.BidQuantityCase);
        //                //    raTotalDiscount = s.SaudaOrders.VolumeDiscountCase +
        //                //        s.SaudaOrders.SchemeDiscountCase +
        //                //        s.SaudaOrders.SkuDiscountCase +
        //                //        (s.SaudaOrders.GPBenefitTypeId == (int)DTO.Enums.BenefitType.NONSAP ? s.SaudaOrders.GPBenefitDiscountInCase : 0) +
        //                //        (s.SaudaOrders.SurpriseBenefitTypeId == (int)DTO.Enums.BenefitType.NONSAP ? s.SaudaOrders.SurpriseBenefitDiscountInCase : 0);
        //                //    // decimal discountWithTax = Utility.IncludeGst(1, s.Pricing.PlantGSTPercentage, raTotalDiscount);

        //                //    switch (s.SaudaOrders.Incoterms2)
        //                //    {
        //                //        case (long)DTO.Enums.IncoTerms.ExPlant:
        //                //            discountGstPercentage = Utility.GetGstAmount(1, s.Pricing.PlantGSTPercentage);
        //                //            discountWithTax = Utility.DecimalFormatTwo(raTotalDiscount) * discountGstPercentage;
        //                //            discountTaxAmount = discountWithTax - Utility.DecimalFormatTwo(raTotalDiscount);
        //                //            taxPaidValue = Utility.DecimalFormatTwo(bidPricePerCause) /*- discountTaxAmount*/;
        //                //            saleRateWithTax = bidPricePerCause; //- discountTaxAmount;
        //                //            saleRate = Utility.ExcludeGst(1, s.Pricing.PlantGSTPercentage, saleRateWithTax);
        //                //            if (s.SaudaOrders.IsBaseSauda)
        //                //            {
        //                //                raPremiumWithoutTax = Utility.DecimalFormatTwo(Utility.ExcludeGst(1, s.Pricing.PlantGSTPercentage, raPremiumWithtax));
        //                //            }
        //                //            else
        //                //            {
        //                //                raPremiumWithoutTax = Utility.DecimalFormatTwo(Utility.ExcludeGst(1, s.Pricing.PlantGSTPercentage, raPremiumWithtax));
        //                //            }
        //                //            break;
        //                //        case (long)DTO.Enums.IncoTerms.ForPlant:
        //                //            //saleRate = Utility.ExcludeGst(1, s.Pricing.PlantGSTPercentage, bidPricePerCause);
        //                //            //taxPaidValue = Utility.DecimalFormatTwo(saleRate * Utility.GetGstAmount(1, s.Pricing.PlantGSTPercentage));
        //                //            discountGstPercentage = Utility.GetGstAmount(1, s.Pricing.PlantGSTPercentage);
        //                //            discountWithTax = raTotalDiscount * discountGstPercentage;
        //                //            discountTaxAmount = discountWithTax - raTotalDiscount;
        //                //            taxPaidValue = Utility.DecimalFormatTwo(bidPricePerCause)/* - discountTaxAmount*/;
        //                //            saleRateWithTax = bidPricePerCause; //- discountTaxAmount;
        //                //            saleRate = Utility.ExcludeGst(1, s.Pricing.PlantGSTPercentage, saleRateWithTax);
        //                //            if (s.SaudaOrders.IsBaseSauda)
        //                //            {
        //                //                raPremiumWithoutTax = Utility.ExcludeGst(1, s.Pricing.PlantGSTPercentage, raPremiumWithtax);
        //                //            }
        //                //            else
        //                //            {
        //                //                raPremiumWithoutTax = Utility.ExcludeGst(1, s.Pricing.PlantGSTPercentage, raPremiumWithtax);
        //                //            }
        //                //            break;
        //                //        case (long)DTO.Enums.IncoTerms.ExDepot:
        //                //            //saleRate = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, bidPricePerCause);
        //                //            //taxPaidValue = Utility.DecimalFormatTwo(saleRate * Utility.GetGstAmount(1, s.Pricing.DepotGSTPercentage));
        //                //            discountGstPercentage = Utility.GetGstAmount(1, s.Pricing.DepotGSTPercentage);
        //                //            discountWithTax = raTotalDiscount * discountGstPercentage;
        //                //            discountTaxAmount = discountWithTax - raTotalDiscount;
        //                //            taxPaidValue = Utility.DecimalFormatTwo(bidPricePerCause) /*- discountTaxAmount*/;
        //                //            saleRateWithTax = bidPricePerCause;// - discountTaxAmount;
        //                //            saleRate = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, saleRateWithTax);
        //                //            if (s.SaudaOrders.IsBaseSauda)
        //                //            {
        //                //                raPremiumWithoutTax = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, raPremiumWithtax);
        //                //            }
        //                //            else
        //                //            {
        //                //                raPremiumWithoutTax = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, raPremiumWithtax);
        //                //            }
        //                //            break;
        //                //        case (long)DTO.Enums.IncoTerms.ForDepot:
        //                //            //saleRate = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, bidPricePerCause);
        //                //            //taxPaidValue = Utility.DecimalFormatTwo(saleRate * Utility.GetGstAmount(1, s.Pricing.DepotGSTPercentage));
        //                //            discountGstPercentage = Utility.GetGstAmount(1, s.Pricing.DepotGSTPercentage);
        //                //            discountWithTax = raTotalDiscount * discountGstPercentage;
        //                //            discountTaxAmount = discountWithTax - raTotalDiscount;
        //                //            taxPaidValue = Utility.DecimalFormatTwo(bidPricePerCause) /*- discountTaxAmount*/;
        //                //            saleRateWithTax = bidPricePerCause;// - discountTaxAmount;
        //                //            saleRate = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, saleRateWithTax);

        //                //            if (s.SaudaOrders.IsBaseSauda)
        //                //            {
        //                //                raPremiumWithoutTax = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, raPremiumWithtax);
        //                //            }
        //                //            else
        //                //            {
        //                //                raPremiumWithoutTax = Utility.ExcludeGst(1, s.Pricing.DepotGSTPercentage, raPremiumWithtax);
        //                //            }
        //                //            break;
        //                //        default:
        //                //            saleRate = 0;
        //                //            break;
        //                //    }
        //                //    saleRate = Utility.DecimalFormatTwo(saleRate);
        //                //    sRate = saleRate;
        //                //    PR00 = (PR00 + s.Pricing.CustomerGroupMargin) - Utility.DecimalFormatTwo(raTotalDiscount / discountGstPercentage);
        //                //    PR00 = Convert.ToDecimal(string.Format("{0:0.00}", PR00 + raPremiumWithoutTax));// Convert.ToDecimal(string.Format("{0:0.00}", PR00)) + Convert.ToDecimal(string.Format("{0:0.00}", raPremiumWithoutTax)); // (PR00 + raPremiumWithoutTax);

        //                //    if (!s.SaudaOrders.IsBaseSauda)
        //                //    {
        //                //        decimal gstPercentage = 0;

        //                //        if (s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExPlant || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForPlant)
        //                //        {
        //                //            gstPercentage = s.Pricing.PlantGSTPercentage;
        //                //        }
        //                //        else if (s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExDepot || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForDepot)
        //                //        {
        //                //            gstPercentage = s.Pricing.DepotGSTPercentage;
        //                //        }

        //                //        allocationPremiumWithtax = s.SaudaOrders.BaseSkuBidPrice - s.SaudaOrders.BidPriceBeforeDiscount; //Utility.DecimalFormatTwo(saleRate * gstPercentage);
        //                //        allocationPremiumWithoutTax = Utility.DecimalFormatTwo(Utility.ExcludeGst(1, gstPercentage, allocationPremiumWithtax));
        //                //        PR00 = Convert.ToDecimal(string.Format("{0:0.00}", PR00 - allocationPremiumWithoutTax));
        //                //    }
        //                //}
        //                //else
        //                //{
        //                saleRate = PR00 + FRC1;
        //                sRate = (s.SaudaOrders.BidPrice > 0 && s.SaudaOrders.BidQuantityCase > 0) ? s.SaudaOrders.BidPrice / s.SaudaOrders.BidQuantityCase : 0;
        //                taxPaidValue = Utility.DecimalFormatTwo(saleRate * Convert.ToDecimal(Constants.TaxPaidValue));
        //                //}


        //                var realizationPerCase = CalculateRealizationPerCase(PR00, 0, 0, 0, 0,
        //                    //s.Pricing.MaterialCost, s.Pricing.Margin, s.Pricing.CushionMargin, s.Pricing.RaMargin,
        //                    premium, discount, s.Sauda.SaudaBookingTypeId, 0, 0, 0,
        //                    //s.Pricing.PackingCost, honeycombCost, s.Pricing.SchemeCostRecovery, 
        //                    raPremiumWithoutTax, 0
        //                    // s.Pricing.OilTransferCostForPlant
        //                    );

        //                decimal realizationPerMT = 0;

        //                var totalValue = (saleRate * s.SaudaOrders.BidQuantityCase);


        //                //var broker = BrokerNameCode(s.SaudaOrders.BrokerId);
        //                var broker = UserDatas.FirstOrDefault(f => f.Id == s.SaudaOrders.BrokerId);

        //                decimal brokerage = 0, realizationPerCasePostBrokerage = 0, realizationPerMTPostBrokerage = 0, finalRealization = 0, purchaseCost = 0;
        //                if (s.SaudaOrders.TradeTicketNo != null)
        //                {
        //                    //var purchaseCostContext = _emamiContext.TradeTicket.AsNoTracking().FirstOrDefault(_ => _.TradeTicketNumber == s.SaudaOrders.TradeTicketNo)?.TotalCost;
        //                    var purchaseCostContext = TradeTicketDatas.FirstOrDefault(_ => _.TradeTicketNumber == s.SaudaOrders.TradeTicketNo)?.TotalCost;
        //                    if (purchaseCostContext != null)
        //                    {
        //                        purchaseCost = (decimal)purchaseCostContext;
        //                    }
        //                }

        //                if (broker != null)
        //                {
        //                    brokerage = 2;
        //                }
        //                realizationPerCasePostBrokerage = realizationPerCase - brokerage;
        //                decimal SKUWiseWeight = 0;
        //                if (s.SaudaOrders.SkuUom == DTO.Enums.Uom.Ltr.ToString())
        //                {
        //                    //var SkuUomMappingContext = _emamiContext.SkuUomMapping.FirstOrDefault(_ => _.SkuId == s.SaudaOrders.SkuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)(int)DTO.Enums.Uom.Nos);
        //                    var SkuUomMappingContext = SkuUomMappingDatas.FirstOrDefault(_ => _.SkuId == s.SaudaOrders.SkuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)(int)DTO.Enums.Uom.Nos);
        //                    if (SkuUomMappingContext != null)
        //                    {
        //                        // SKUWiseWeight = s.SaudaOrders.LitreConversion > 0 ? (s.SaudaOrders.SkuQuantity * 1000 * SkuUomMappingContext.ConversionFactor) / s.SaudaOrders.LitreConversion : 0;
        //                    }
        //                    else
        //                    {
        //                        SKUWiseWeight = s.SaudaOrders.LitreConversion > 0 ? (s.SaudaOrders.SkuQuantity * 1000) / s.SaudaOrders.LitreConversion : 0;
        //                    }
        //                }
        //                else
        //                {
        //                    SKUWiseWeight = s.SaudaOrders.SkuQuantity;
        //                }

        //                if (realizationPerCase > 0 && SKUWiseWeight > 0)
        //                {
        //                    realizationPerMT = realizationPerCase / SKUWiseWeight * 1000;
        //                }
        //                realizationPerMTPostBrokerage = realizationPerCasePostBrokerage != 0 && SKUWiseWeight > 0 ? (realizationPerCasePostBrokerage / SKUWiseWeight) * 1000 : 0;
        //                finalRealization = realizationPerMTPostBrokerage;// - honeycombCost;
        //                var employeeData = GetBdoname(s.Sauda.UserId);
        //                saudaList.Add(new SaudaOrderReportOutputDto()
        //                {
        //                    CustomerCode = s.User.Code,
        //                    CustomerName = s.User.Name,
        //                    //FreightRoute = s.User.FreightRouteName,
        //                    BrokerName = broker != null ? broker.Name : "",
        //                    BrokerCode = broker != null ? broker.Code : "",
        //                    SkuName = s.SaudaOrders.SkuName,
        //                    SkuCode = s.SaudaOrders.SkuCode,
        //                    BidQuantityCase = s.SaudaOrders.BidQuantityCase,
        //                    PR00 = PR00,
        //                    FRC1 = FRC1,
        //                    SaleRate = sRate,
        //                    BidPrice = s.SaudaOrders.BidPrice,
        //                    Incoterms = Utility.GetEnumFromString<DTO.Enums.IncoTerms>(s.SaudaOrders.Incoterms2), // IncotermsName(s.SaudaOrders.Incoterms2),
        //                    AppBookingNo = s.SaudaOrders.SaudaId.ToString(),
        //                    BiddingDate = s.Sauda.BiddingDate,
        //                    ValidFromDate = s.SaudaOrders.ValidFromDate,
        //                    ValidToDate = s.SaudaOrders.ValidToDate,
        //                    BidQuantity = Utility.DecimalFormatThree(s.SaudaOrders.BidQuantity),
        //                    PackGroup = s.SaudaOrders.PackGroupName,
        //                    //DepotCode = (s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ForDepot || s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ExDepot) ? (s.Pricing.DepotId == 0 ? "" : depotContext.FirstOrDefault(depot => depot.Id == s.Pricing.DepotId).Code) : (s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ForRake || s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ExRake) ? (s.Pricing.DepotId == 0 ? "" : depotContext.FirstOrDefault(depot => depot.Id == s.Pricing.DepotId).Code) : "",
        //                    //DepotName = (s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ForDepot || s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ExDepot) ? (s.Pricing.DepotId == 0 ? "" : depotContext.FirstOrDefault(depot => depot.Id == s.Pricing.DepotId).Name) : (s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ForRake || s.SaudaOrders.Incoterms2 == (long)DTO.Enums.IncoTerms.ExRake) ? (s.Pricing.DepotId == 0 ? "" : depotContext.FirstOrDefault(depot => depot.Id == s.Pricing.DepotId).Name) : "",
        //                    //State = s.Pricing.StateId > 0 ? _emamiContext.State.AsNoTracking().FirstOrDefault(state => state.Id == s.Pricing.StateId).StateName : "",
        //                    PlantName = s.Pricing.PlantId > 0 ? depotContext.FirstOrDefault(plant => plant.Id == s.Pricing.PlantId).Name : "",
        //                    PlantCode = s.Pricing.PlantId > 0 ? depotContext.FirstOrDefault(plant => plant.Id == s.Pricing.PlantId).Code : "",
        //                    RealizationPerMt = realizationPerMT,
        //                    UOM = "Case",
        //                    PackSize = s.SaudaOrders.SkuQuantity + " " + s.SaudaOrders.SkuUom,
        //                    //MaterialCost = s.Pricing.MaterialCost,
        //                    //PrimaryFreight = (s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExDepot || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForDepot || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForRake) ?
        //                    //s.Pricing.PrimaryFrieght : 0,
        //                    //PackingCost = s.Pricing.PackingCost,
        //                    //HoneycombCost = honeycombCost,
        //                    BrokerageCost = 0,
        //                    DetentionCharges =
        //                    //(s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExDepot || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForDepot || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForRake) ? s.Pricing.DetentionCost : 
        //                    0,
        //                    //DepotCost = (s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExDepot || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForDepot || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ExRake || s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForRake) ? s.Pricing.DepotCost : 0,
        //                    //MarginCostTP = (s.Pricing.Margin + s.Pricing.CushionMargin),
        //                    //MarginCostRA = s.Pricing.RaMargin,
        //                    //SecondaryFreight = s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForDepot ? s.Pricing.SecondaryFrieght :
        //                    //(s.SaudaOrders.Incoterms2 == (int)DTO.Enums.IncoTerms.ForPlant ? s.Pricing.PlantSecondaryFrieght : 0),
        //                    TD = Utility.DecimalFormatTwo(discount),
        //                    //LTD = 0,
        //                    TotalValue = totalValue,
        //                    EmployeeName = employeeData != null ? employeeData.Name : "", // GetBdoname(s.Sauda.UserId).Name,
        //                    EmployeeCode = employeeData != null ? employeeData.Code : "", // GetBdoname(s.Sauda.UserId).Code,
        //                    Vertical = s.SaudaOrders.VerticalName,
        //                    Premium = Utility.DecimalFormatTwo(premium),
        //                    SaudaBookingType = s.Sauda.SaudaBookingType,
        //                    RealizationPerCase = realizationPerCase,
        //                    //ActualPackingCost = s.Pricing.PackingCost,
        //                    Status = Enum.GetName(typeof(DTO.Enums.Status), s.SaudaOrders.StatusId),
        //                    LTDValue = LtdValue,
        //                    SpecialRate = specialRate,
        //                    Remarks = s.SaudaOrders.Remarks != null ? s.SaudaOrders.Remarks : string.Empty,
        //                    //CushionMargin = s.Pricing.CushionMargin,
        //                    BiddingTime = s.Sauda.BiddingDate.TimeOfDay,
        //                    OilType = s.SaudaOrders.OilType,
        //                    TaxPaid = taxPaidValue, // Utility.DecimalFormatTwo(saleRate * Convert.ToDecimal(Constants.TaxPaidValue)),
        //                    Brokerage = brokerage,
        //                    Area = s.Pricing.PlantId == 0 ? "" : (s.User.StateId == (depotContext.FirstOrDefault(plant => plant.Id == s.Pricing.PlantId).StateId) ? (depotContext.FirstOrDefault(plant => plant.Id == s.Pricing.PlantId).State.StateName) : Constants.OutOfState.Replace(Constants.StateName, depotContext.FirstOrDefault(plant => plant.Id == s.Pricing.PlantId).State.StateName)),
        //                    RealizationPerCasePostBrokerage = realizationPerCasePostBrokerage,
        //                    SkuWiseWeight = SKUWiseWeight,
        //                    RealizationPerMTPostBrokerage = realizationPerMTPostBrokerage,
        //                    FinalRealization = finalRealization,
        //                    RealizationTotal = finalRealization * s.SaudaOrders.BidQuantity,
        //                    Purchase = purchaseCost,
        //                    PurchaseTotal = purchaseCost * s.SaudaOrders.BidQuantity,
        //                    MarginPMTLineItem = finalRealization - purchaseCost,
        //                    //SchemeCost = s.Pricing.SchemeCostRecovery,
        //                    //MaterialType = s.SaudaOrders.MaterialType,
        //                    //CustomerGroupMargin = s.Pricing.CustomerGroupMargin,
        //                    RaTotalDiscount = raTotalDiscount,
        //                    SaudaBookingTypeId = s.Sauda.SaudaBookingTypeId,
        //                    RAPremiumWithTax = raPremiumWithtax,
        //                    RAPremiumWithoutTax = raPremiumWithoutTax,
        //                    //AdditionalCost = s.Pricing.AdditionalCost,
        //                    //OilTransferCost = s.Pricing.OilTransferCostForPlant > 0 ? s.Pricing.OilTransferCostForPlant : s.Pricing.OilTransferCostForDepot,
        //                    IsBaseSauda = s.SaudaOrders.IsBaseSauda,
        //                    SkuAllocationPremiumWithTax = allocationPremiumWithtax,
        //                    SkuAllocationPremiumWithoutTax = allocationPremiumWithoutTax,
        //                    //CustomerGroupOne = CustomerGroupOneDatas.FirstOrDefault(f => f.Id == s.User.CustomerGroupOneId)?.Name,
        //                    //CustomerGroupTwo = CustomerGroupTwoDatas.FirstOrDefault(f => f.Id == s.User.CustomerGroupTwoId)?.Name,
        //                    SaudaOrderId = s.SaudaOrders.Id,
        //                    SaudaNumber = s.SaudaOrders.SaudaNumber != null ? s.SaudaOrders.SaudaNumber : string.Empty
        //                });
        //            }
        //        }

        //        var pageSize = Constants.PageSize;
        //        var skip = pageSize * inputDto.PageNo;
        //        outputDto.ListCount = saudaList.Count();
        //        outputDto.SaudaOrderReports = saudaList.OrderByDescending(_ => _.SaudaOrderId).Skip((inputDto.PageNo - 1) * pageSize).Take(pageSize).ToList(); //Skip(skip).Take(pageSize).ToList();

        //        resultDto.IsSuccess = true;
        //        resultDto.SuccessDto.Response = outputDto;
        //        return resultDto;
        //        //return _resultService.SuccessObject(saudaList);
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        #endregion


        #region SchemeGeographyReport

        public ResultDto GetSchemeGeographyDetailsReport(SchemeGeographyReportInputputDto inputDto)
        {
            _methodName = "GetSchemeGeographyDetailsReport";
            var saudaList = new List<SchemeGeographyReportOutputDto>();
            try
            {
                var parameters = new object[]
                {
                    inputDto.FromDate
                    ,inputDto.ToDate
                    ,string.Join(",",inputDto.GeographySchemeIds)
                    ,string.Join(",",inputDto.StateIds)
                    ,inputDto.VerticalId
                    ,inputDto.SalesOrganizationId,
                    inputDto.DistributionChannelId
               };

                var schemeGeography = _emamiContext.Database.SqlQuery<SchemeGeographyReportOutputDto>("usp_schemeGeographyReport {0}, {1}, {2}, {3}, {4},{5},{6}", parameters).ToList<SchemeGeographyReportOutputDto>();

                return _resultService.SuccessObject(schemeGeography);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion

        #region DemandPlanBillingReport

        public ResultDto GetDemandPlanBillingDetailsReport(DemandPlanBillingReportInputputDto inputDto)
        {
            _methodName = "GetDemandPlanBillingDetailsReport";
            var saudaList = new List<DemandPlanBillingReportOutputDto>();
            try
            {
                var parameters = new object[]
                {
                    inputDto.FromDate,
                    inputDto.ToDate
               };

                var dpbReport = _emamiContext.Database.SqlQuery<DemandPlanBillingReportOutputDto>("usp_demandPlanBillingReport {0}, {1}", parameters).ToList<DemandPlanBillingReportOutputDto>();

                return _resultService.SuccessObject(dpbReport);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        #endregion


    }
}
