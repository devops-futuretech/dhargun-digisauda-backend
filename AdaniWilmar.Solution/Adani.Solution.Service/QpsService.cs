using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.DTO.QPSDiscount;
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
using System.Text;
using System.Threading.Tasks;
using TagLib.Ape;

namespace Adani.Solution.Service
{
    public interface IQpsService
    {
        ResultDto QpsAddOrUpdate(QPSSchemeDiscountDto inputDto);
        ResultDto QpsList(QPSSchemeDiscountDto inputDto);
        ResultDto GetQpsDiscountById(string inputDto);
        ResultDto GetQpsDiscountByIdnew(long inputDto);
        ResultDto ExportQpsSchemeDiscount(LoginUserIdDto inputDto);
        ResultDto GetQpsDiscountListWithPagination(KendoGridResult inputDto);
        ResultDto GetQPSDiscountWithSlab(SkuQpsInputDto x);
        ResultDto GetQPSDiscountForQuantity(SkuQpsInputDto x);
    }

    public class QpsService : IQpsService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Qps Service");
        private const string ServiceName = "Qps Service";
        private string _methodName;
        private readonly IResultService _resultService;
        private readonly INotificationService _notificationService;

        public QpsService(IAdaniContext emamiContext, IResultService resultService, INotificationService notificationService)
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
                _logger.Error("Error instantiating dependencies for Qps Service", exception);
            }
        }

        public ResultDto QpsAddOrUpdate(QPSSchemeDiscountDto inputDto)
        {
            _methodName = "QpsAddOrUpdate";
            var resultDto = new ResultDto();
            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }
                if (inputDto.EndDate < inputDto.StartDate)
                {
                    return _resultService.ErrorMessage(Constants.ToDateInvalid);
                }
                if (inputDto.OilTypeId == null || !inputDto.OilTypeId.Any())
                {
                    return _resultService.ErrorMessage(Constants.OilTypeEmpty);
                }
                if (inputDto.ZoneId == null || !inputDto.ZoneId.Any())
                {
                    return _resultService.ErrorMessage(Constants.ZoneEmpty);
                }
                if (inputDto.StateId == null || !inputDto.StateId.Any())
                {
                    return _resultService.ErrorMessage(Constants.StateIdEmpty);
                }
                //var qpsDateRangeInvalid = _emamiContext.QpsDiscount.AsNoTracking().Any(_ =>
                //_.OilTypeId == inputDto.OilTypeId && _.SkuId == inputDto.SkuIds && _.SalesOrgId == inputDto.SalesOrgId &&
                //_.DistributionChannelId == inputDto.DistributionChannelId
                //&& _.DivisionId == inputDto.DivisionId && _.StateId == inputDto.StateId && _.ZoneId == inputDto.ZoneId
                //&& (_.StartDate != inputDto.StartDate && _.EndDate != inputDto.EndDate)
                //&& ((inputDto.StartDate >= _.StartDate && inputDto.StartDate <= _.EndDate) ||
                //(inputDto.EndDate >= _.StartDate && inputDto.EndDate <= _.EndDate)));
                #region Date Range Validation 
                /*var qpsDateRangeInvalid = (
                    from discount in _emamiContext.QpsDiscount.AsNoTracking()
                    join skuMapping in _emamiContext.QPSDiscountSkuMapping.AsNoTracking()
                    on discount.Id equals skuMapping.QpsDiscountId
                    join zoneStateMapping in _emamiContext.ZoneStateMappings.AsNoTracking()
                    on skuMapping.StateId equals zoneStateMapping.StateId
                    where discount.SalesOrgId == inputDto.SalesOrgId
                        && discount.DistributionChannelId == inputDto.DistributionChannelId
                        && discount.DivisionId == inputDto.DivisionId
                        && inputDto.OilTypeId.Contains(skuMapping.OilTypeId)
                        && inputDto.SkuIds.Contains(skuMapping.SkuId)
                        && inputDto.ZoneId.Contains(zoneStateMapping.ZoneId)
                        && inputDto.StateId.Contains(skuMapping.StateId)
                        && (discount.StartDate != inputDto.StartDate || discount.EndDate != inputDto.EndDate)
                        && ((inputDto.StartDate >= discount.StartDate && inputDto.StartDate <= discount.EndDate) ||
                            (inputDto.EndDate >= discount.StartDate && inputDto.EndDate <= discount.EndDate) ||
                            (discount.StartDate >= inputDto.StartDate && discount.StartDate <= inputDto.EndDate) ||
                            (discount.EndDate >= inputDto.StartDate && discount.EndDate <= inputDto.EndDate))
                    select discount
                ).Any();
                if (qpsDateRangeInvalid)
                {
                    return _resultService.ErrorMessage(Constants.DateRangeInvalid);
                }*/
                #endregion

                //var qpsduplicate = (
                //    from discount in _emamiContext.QpsDiscount.AsNoTracking()
                //    join skuMapping in _emamiContext.QPSDiscountSkuMapping.AsNoTracking()
                //    on discount.Id equals skuMapping.QpsDiscountId
                //    join zoneStateMapping in _emamiContext.ZoneStateMappings.AsNoTracking()
                //    on skuMapping.StateId equals zoneStateMapping.StateId
                //    where discount.SalesOrgId == inputDto.SalesOrgId
                //        && discount.DistributionChannelId == inputDto.DistributionChannelId
                //        && discount.DivisionId == inputDto.DivisionId
                //        && discount.StartDate == inputDto.StartDate
                //        && discount.EndDate == inputDto.EndDate
                //        && inputDto.OilTypeId.Contains(skuMapping.OilTypeId)
                //        && inputDto.SkuIds.Contains(skuMapping.SkuId)
                //        && inputDto.ZoneId.Contains(zoneStateMapping.ZoneId)
                //        && inputDto.StateId.Contains(skuMapping.StateId)select discount
                //).Any();

                #region Deactivate sku for same combination
                /*var skuIdsForAllOiltype = _emamiContext.Skus.AsNoTracking().Where(s => s.IsActive && s.OilTypeId.HasValue && inputDto.OilTypeId.Contains(s.OilTypeId.Value) && inputDto.SkuIds.Contains(s.Id))
                        .Select(s => new { s.Id, s.OilTypeId }).ToList();

                var oilTypeIdsWithoutSkus = inputDto.OilTypeId
                    .Where(oilTypeId => !skuIdsForAllOiltype.Any(sku => sku.OilTypeId == oilTypeId))
                    .Select(oilTypeId => new { Id = 0L, OilTypeId = (long?)oilTypeId })
                    .ToList();

                var updatedInputDtoSkus = skuIdsForAllOiltype
                        .Select(s => new { s.Id, s.OilTypeId })
                        .Union(oilTypeIdsWithoutSkus).Select(s => s.Id).Distinct()
                        .ToList();*/
                #endregion

                if (inputDto.Id == 0)
                {
                    #region Deactivate sku for same combination
                    /*var existingDiscounts = (
                        from discount in _emamiContext.QpsDiscount.AsNoTracking()
                        join skuMapping in _emamiContext.QPSDiscountSkuMapping.AsNoTracking()
                        on discount.Id equals skuMapping.QpsDiscountId
                        join zoneStateMapping in _emamiContext.ZoneStateMappings.AsNoTracking()
                        on skuMapping.StateId equals zoneStateMapping.StateId
                        where discount.SalesOrgId == inputDto.SalesOrgId
                            && discount.DistributionChannelId == inputDto.DistributionChannelId
                            && discount.DivisionId == inputDto.DivisionId
                            && inputDto.OilTypeId.Contains(skuMapping.OilTypeId)
                            && (updatedInputDtoSkus.Contains(skuMapping.SkuId))
                            && inputDto.ZoneId.Contains(zoneStateMapping.ZoneId)
                            //&& inputDto.StateId.Contains(skuMapping.StateId)
                            //&& inputDto.StartDate == discount.StartDate
                            && inputDto.EndDate == discount.EndDate && discount.IsActive
                        select new { discount.Id, skuMapping.SkuId, skuMapping.OilTypeId }
                    ).ToList();

                    if (existingDiscounts.Any())
                    {
                        var skusToDeactivate = existingDiscounts
                            .GroupBy(x => x.Id)
                            .SelectMany(g =>
                            {
                                var existingSkuIds = g.Select(x => x.SkuId).ToList();
                                return existingSkuIds.Intersect(updatedInputDtoSkus);
                            }).Distinct().ToList();
                        if (skusToDeactivate.Any())
                        {
                            //Removing the skuMap data from mapping table
                            foreach (var id in existingDiscounts.Select(x => x.Id).Distinct())
                            {
                                var totalActiveSku = _emamiContext.QPSDiscountSkuMapping.Where(s => s.QpsDiscountId == id && s.IsActive).ToList();
                                if (totalActiveSku.Count == existingDiscounts.Count)
                                {
                                    foreach (var skuMap in existingDiscounts.Select(x => new { x.Id, x.OilTypeId }).Distinct())
                                    {
                                        var skuMappings = _emamiContext.QPSDiscountSkuMapping
                                                                      .Where(mapping => mapping.QpsDiscountId == skuMap.Id &&
                                                                       skusToDeactivate.Contains(mapping.SkuId) && mapping.OilTypeId == skuMap.OilTypeId);
                                        foreach (var skuMapping in skuMappings)
                                        {
                                            skuMapping.IsActive = false;
                                        }
                                    }
                                    _emamiContext.SaveChanges();
                                }
                                else
                                {
                                    foreach (var skuMap in existingDiscounts.Select(x => new { x.Id, x.OilTypeId }).Distinct())
                                    {
                                        var existingSkuDetails = _emamiContext.QPSDiscountSkuMapping.Where(_ => _.QpsDiscountId == skuMap.Id &&
                                            skusToDeactivate.Contains(_.SkuId) && _.OilTypeId == skuMap.OilTypeId).ToList();
                                        foreach (var sku in existingSkuDetails)
                                        {
                                            _emamiContext.QPSDiscountSkuMapping.Remove(sku);
                                        }
                                    }
                                    _emamiContext.SaveChanges();
                                }
                                //To deactivate the QPSDiscount
                                totalActiveSku = _emamiContext.QPSDiscountSkuMapping.Where(s => s.QpsDiscountId == id && s.IsActive).ToList();
                                if (!totalActiveSku.Any())
                                {
                                    var qpsDiscount = _emamiContext.QpsDiscount.Where(x => x.Id == id);
                                    foreach (var qps in qpsDiscount)
                                    {
                                        qps.IsActive = false;
                                        qps.ModifiedBy = inputDto.LoginUserId;
                                        qps.ModifiedDate = DateTime.Now;
                                    }
                                }
                            }
                            _emamiContext.SaveChanges();
                        }
                    }
                    else
                    {
                        var qpsexists = (
                            from discount in _emamiContext.QpsDiscount.AsNoTracking()
                            join skuMapping in _emamiContext.QPSDiscountSkuMapping.AsNoTracking()
                            on discount.Id equals skuMapping.QpsDiscountId
                            join zoneStateMapping in _emamiContext.ZoneStateMappings.AsNoTracking()
                            on skuMapping.StateId equals zoneStateMapping.StateId
                            where discount.SalesOrgId == inputDto.SalesOrgId
                                && discount.DistributionChannelId == inputDto.DistributionChannelId
                                && discount.DivisionId == inputDto.DivisionId
                                && inputDto.OilTypeId.Contains(skuMapping.OilTypeId)
                                && inputDto.SkuIds.Contains(skuMapping.SkuId)
                                && inputDto.ZoneId.Contains(zoneStateMapping.ZoneId)
                                && inputDto.StateId.Contains(skuMapping.StateId)
                                && inputDto.StartDate == discount.StartDate
                                && inputDto.EndDate == discount.EndDate
                            select discount.Id
                        ).ToList();

                        if (qpsexists.Any())
                        {
                            foreach (var id in qpsexists)
                            {
                                var qpsdiscount = _emamiContext.QpsDiscount.FirstOrDefault(_ => _.Id == id);
                                qpsdiscount.IsActive = false;
                            }
                            _emamiContext.SaveChanges();
                        }
                    }*/
                    #endregion

                    QpsDiscount data = new QpsDiscount()
                    {
                        Id = inputDto.Id,
                        StartDate = inputDto.StartDate,
                        EndDate = inputDto.EndDate,
                        SalesOrgId = inputDto.SalesOrgId,
                        DistributionChannelId = inputDto.DistributionChannelId,
                        DivisionId = inputDto.DivisionId,
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = DateTime.Now,
                        IsActive = inputDto.IsActive,
                    };
                    _emamiContext.QpsDiscount.Add(data);
                    _emamiContext.SaveChanges();

                    var combinations = GetCombinations(inputDto.SkuIds, inputDto.StateId,inputDto.OilTypeId);

                    foreach (var combination in combinations)
                    {
                        QPSDiscountSkuMapping mapping = new QPSDiscountSkuMapping()
                        {
                            QpsDiscountId = data.Id,
                            SkuId = combination.SkuId,
                            //ZoneId = combination.ZoneId,
                            StateId = combination.StateId,
                            OilTypeId = combination.OilTypeId,
                            IsActive = data.IsActive,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateTime.Now
                        };
                        _emamiContext.QPSDiscountSkuMapping.Add(mapping);
                    }

                    //_emamiContext.SaveChanges();

                    foreach (var qps in inputDto.QPSSlabDetails)
                    {
                        var discoundetails = new QPSSlabDetails();
                        discoundetails.QpsDiscountId = data.Id;
                        discoundetails.FromRange = qps.FromRange;
                        discoundetails.ToRange = qps.ToRange;
                        discoundetails.Discount = qps.Discount;
                        discoundetails.CreatedBy = inputDto.LoginUserId;
                        discoundetails.CreatedDate = DateTime.Now;
                        _emamiContext.QPSSlabDetails.Add(discoundetails);
                    }
                    _emamiContext.SaveChanges();
                }
                else if (inputDto.Id > 0)
                {
                    #region Deactivate sku for same combination
                    /*var existingQps = (
                        from discount in _emamiContext.QpsDiscount.AsNoTracking()
                        join skuMapping in _emamiContext.QPSDiscountSkuMapping.AsNoTracking()
                        on discount.Id equals skuMapping.QpsDiscountId
                        join zoneStateMapping in _emamiContext.ZoneStateMappings.AsNoTracking()
                        on skuMapping.StateId equals zoneStateMapping.StateId
                        where discount.SalesOrgId == inputDto.SalesOrgId
                            && discount.DistributionChannelId == inputDto.DistributionChannelId
                            && discount.DivisionId == inputDto.DivisionId
                            && inputDto.OilTypeId.Contains(skuMapping.OilTypeId)
                            && (updatedInputDtoSkus.Contains(skuMapping.SkuId))
                            && inputDto.ZoneId.Contains(zoneStateMapping.ZoneId)
                            && inputDto.EndDate == discount.EndDate && discount.Id != inputDto.Id && discount.IsActive
                        select new { discount.Id, skuMapping.SkuId, skuMapping.OilTypeId }
                    ).ToList();

                    if (existingQps.Any())
                    {
                        var skusToDeactivate = existingQps
                            .GroupBy(x => x.Id)
                            .SelectMany(g =>
                            {
                                var existingSkuIds = g.Select(x => x.SkuId).ToList();
                                return existingSkuIds.Intersect(updatedInputDtoSkus);
                            }).Distinct().ToList();

                        if (skusToDeactivate.Any())
                        {
                            //Removing the skuMap data from mapping table
                            foreach (var id in existingQps.Select(x => x.Id).Distinct())
                            {
                                var totalActiveSku = _emamiContext.QPSDiscountSkuMapping.Where(s => s.QpsDiscountId == id && s.IsActive).ToList();
                                if (totalActiveSku.Count == existingQps.Count)
                                {
                                    foreach (var skuMap in existingQps.Select(x => new { x.Id, x.OilTypeId }).Distinct())
                                    {
                                        var skuMappings = _emamiContext.QPSDiscountSkuMapping
                                                                      .Where(mapping => mapping.QpsDiscountId == skuMap.Id &&
                                                                       skusToDeactivate.Contains(mapping.SkuId) && mapping.OilTypeId == skuMap.OilTypeId);
                                        //.ToList();
                                        foreach (var skuMapping in skuMappings)
                                        {
                                            skuMapping.IsActive = false;
                                        }
                                    }
                                    _emamiContext.SaveChanges();
                                }
                                else
                                {
                                    foreach (var skuMap in existingQps.Select(x => new { x.Id, x.OilTypeId }).Distinct())
                                    {
                                        var existingSkuDetails = _emamiContext.QPSDiscountSkuMapping.Where(_ => _.QpsDiscountId == skuMap.Id &&
                                            skusToDeactivate.Contains(_.SkuId) && _.OilTypeId == skuMap.OilTypeId).ToList();
                                        foreach (var sku in existingSkuDetails)
                                        {
                                            _emamiContext.QPSDiscountSkuMapping.Remove(sku);
                                        }
                                    }
                                    _emamiContext.SaveChanges();
                                }
                                //To deactivate the QPSDiscount
                                totalActiveSku = _emamiContext.QPSDiscountSkuMapping.Where(s => s.QpsDiscountId == id && s.IsActive).ToList();
                                if (!totalActiveSku.Any())
                                {
                                    var qpsDiscount = _emamiContext.QpsDiscount.Where(x => x.Id == id);
                                    foreach (var qps in qpsDiscount)
                                    {
                                        qps.IsActive = false;
                                        qps.ModifiedBy = inputDto.LoginUserId;
                                        qps.ModifiedDate = DateTime.Now;
                                    }
                                }
                            }
                            _emamiContext.SaveChanges();
                        }
                    }*/
                    #endregion

                    var existQps = _emamiContext.QpsDiscount.FirstOrDefault(_ => _.Id == inputDto.Id);

                    var existingSlabDetails = _emamiContext.QPSSlabDetails.Where(_ => _.QpsDiscountId == inputDto.Id).ToList();
                    foreach (var slab in existingSlabDetails)
                    {
                        _emamiContext.QPSSlabDetails.Remove(slab);
                    }
                    _emamiContext.SaveChanges();
                    if (inputDto.QPSSlabDetails != null)
                    {
                        foreach (var qps in inputDto.QPSSlabDetails)
                        {
                            var discoundetails = new QPSSlabDetails();
                            discoundetails.QpsDiscountId = inputDto.Id;
                            discoundetails.FromRange = qps.FromRange;
                            discoundetails.ToRange = qps.ToRange;
                            discoundetails.Discount = qps.Discount;
                            discoundetails.CreatedBy = inputDto.LoginUserId;
                            discoundetails.CreatedDate = DateTime.Now;
                            _emamiContext.QPSSlabDetails.Add(discoundetails);
                        }
                    }
                    if (existQps != null)
                    {
                        var slabdetails = _emamiContext.QpsDiscount.Where(_ => _.Id == inputDto.Id);
                        existQps.SalesOrgId = inputDto.SalesOrgId;
                        existQps.DistributionChannelId = inputDto.DistributionChannelId;
                        existQps.DivisionId = inputDto.DivisionId;
                        existQps.IsActive = inputDto.IsActive;
                        existQps.ModifiedBy = inputDto.LoginUserId;
                        existQps.ModifiedDate = DateTime.Now;
                    }
                    _emamiContext.SaveChanges();
                    var existingQPSDiscountSkuMap = _emamiContext.QPSDiscountSkuMapping.Where(_ => _.QpsDiscountId == inputDto.Id);
                    
                    if (existingQPSDiscountSkuMap != null)
                    {
                        foreach(var qpsSku in existingQPSDiscountSkuMap)
                        {
                            qpsSku.IsActive = inputDto.IsActive;
                            qpsSku.ModifiedBy = inputDto.LoginUserId;
                            qpsSku.ModifiedDate = DateTime.Now;
                        }
                        _emamiContext.SaveChanges();
                    }
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.QPSdiscountNotFound;
                    return resultDto;
                }
                   

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Message = "Update Successfully";
                return resultDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }
        
        public ResultDto QpsList(QPSSchemeDiscountDto inputDto)
        {
            _methodName = "QpsList";
            var resultDto = new ResultDto();
            var ConversionTypes1 = new List<QPSSchemeDiscountDto>();
            var QpsSkumap = new List<QPSDiscountSkuMapping>();
            try
            {
                #region Deactivate the Expired data
                var previousDate = DateTime.Today.AddDays(-1);
                var deactivateQPS = _emamiContext.QpsDiscount.AsNoTracking().Where(q => q.EndDate <= previousDate && q.IsActive)
                    .Select(s => s.Id).ToList();
                if (deactivateQPS.Any())
                {
                    foreach (var id in deactivateQPS)
                    {
                        var qpsDiscount = _emamiContext.QpsDiscount.Where(q => q.Id == id).FirstOrDefault();

                        if (qpsDiscount != null)
                        {
                            qpsDiscount.IsActive = false;
                        }
                        _emamiContext.SaveChanges();
                        var existingQPSDiscountSkuMap = _emamiContext.QPSDiscountSkuMapping.Where(_ => _.QpsDiscountId == id);

                        if (existingQPSDiscountSkuMap != null)
                        {
                            foreach (var qpsSku in existingQPSDiscountSkuMap)
                            {
                                qpsSku.IsActive = false;
                            }
                            _emamiContext.SaveChanges();
                        }
                    }
                }
                
                #endregion

                var qpsDiscounts = _emamiContext.QpsDiscount.AsNoTracking().ToList();
                var qpsSkumaps = _emamiContext.QPSDiscountSkuMapping.AsNoTracking().ToList();
                var joinedData = qpsDiscounts.Join(qpsSkumaps,
                                qd => qd.Id, qsm => qsm.QpsDiscountId, (qd, qsm) => new { QPSSchemeDiscountDto = qd, QPSDiscountSkuMapping = qsm })
                                    .GroupBy(item => new { item.QPSSchemeDiscountDto.Id })
                                    .Select(group => new
                                    {
                                        Id = group.Key.Id,
                                        Items = group.FirstOrDefault()
                                    })
                                    .ToList();

                var qpsZoneMap = (from qs in _emamiContext.QPSDiscountSkuMapping
                                  join zm in _emamiContext.ZoneStateMappings
                                  on qs.StateId equals zm.StateId
                                  select new { StateId = qs.StateId, ZoneId = zm.ZoneId , QPSDiscountId = qs.QpsDiscountId })
                 .ToList();
                var oilTypes = _emamiContext.OilTypes.AsNoTracking();
                var combinedList = joinedData.Select(_ => new
                {
                    Id = _.Items.QPSSchemeDiscountDto.Id,
                    StartDate = _.Items.QPSSchemeDiscountDto.StartDate,
                    EndDate = _.Items.QPSSchemeDiscountDto.EndDate,
                    SalesOrgId = _.Items.QPSSchemeDiscountDto.SalesOrgId,
                    DistributionChannelId = _.Items.QPSSchemeDiscountDto.DistributionChannelId,
                    DivisionId = _.Items.QPSSchemeDiscountDto.DivisionId,
                    OilTypeIds = _emamiContext.QpsDiscount.AsNoTracking().Where(q => q.Id == _.Items.QPSSchemeDiscountDto.Id && q.IsActive).Any() ?
                        _emamiContext.QPSDiscountSkuMapping
                    .Where(qsm => qsm.QpsDiscountId == _.Items.QPSSchemeDiscountDto.Id && qsm.IsActive)
                    .Select(qsm => qsm.OilTypeId)
                    .Distinct().ToList() : _emamiContext.QPSDiscountSkuMapping.Where(qsm => qsm.QpsDiscountId == _.Items.QPSSchemeDiscountDto.Id && !qsm.IsActive)
                    .Select(qsm => qsm.OilTypeId)
                    .Distinct().ToList(),
                    OilTypeName = string.Join(", ", _emamiContext.QpsDiscount.AsNoTracking().Where(q => q.Id == _.Items.QPSSchemeDiscountDto.Id && q.IsActive).Any() ?
                    _emamiContext.QPSDiscountSkuMapping.Where(qsm => qsm.QpsDiscountId == _.Items.QPSSchemeDiscountDto.Id && qsm.IsActive).Select(qsm => qsm.OilTypeId)
                    .Distinct().Join(oilTypes, id => id, o => o.Id, (id, o) => o.Name).ToList() :
                    _emamiContext.QPSDiscountSkuMapping.Where(qsm => qsm.QpsDiscountId == _.Items.QPSSchemeDiscountDto.Id && !qsm.IsActive).Select(qsm => qsm.OilTypeId)
                    .Distinct().Join(oilTypes, id => id, o => o.Id, (id, o) => o.Name).ToList()),
                    IsActive = _.Items.QPSSchemeDiscountDto.IsActive,
                    SkuId = _.Items.QPSDiscountSkuMapping.SkuId,
                    StateIds = _emamiContext.QPSDiscountSkuMapping.Where(qsm => qsm.QpsDiscountId == _.Items.QPSSchemeDiscountDto.Id).Select(qsm => qsm.StateId).Distinct().ToList(),
                    ZoneIds = qpsZoneMap.Where(zsm => zsm.QPSDiscountId == _.Items.QPSSchemeDiscountDto.Id).Select(zsm => zsm.ZoneId).Distinct().ToList(),
                    EncryptedId = UtilityHelper.ConvertToMd5(_.Items.QPSSchemeDiscountDto.Id.ToString(), SecurityConstants.EncryptionKey),
            }).OrderByDescending(_ => _.Id).ToList();
                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = combinedList;
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
        public ResultDto GetQpsDiscountByIdnew(long inputDto)
        {
            _methodName = "GetQpsDiscountById";
            var resultDto = new ResultDto();
            var qpsdiscount = new QPSSchemeDiscountDto();
            try
            {
                //var decryptedId = UtilityHelper.ConvertMd5ToString(QPSSchemeDiscountDto, SecurityConstants.EncryptionKey);

                //var Id = UtilityHelper.LongTryToParse(decryptedId);
                var resultContext = _emamiContext.QpsDiscount.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto);
               
                if (resultContext != null)
                {
                    qpsdiscount.EncryptedId = UtilityHelper.ConvertToMd5(resultContext.Id.ToString(), SecurityConstants.EncryptionKey);
                    qpsdiscount.Id = resultContext.Id;
                    qpsdiscount.StartDate = resultContext.StartDate;
                    qpsdiscount.EndDate = resultContext.EndDate;
                    qpsdiscount.SalesOrgId = resultContext.SalesOrgId;
                    qpsdiscount.DistributionChannelId = resultContext.DistributionChannelId;
                    qpsdiscount.DivisionId = resultContext.DivisionId;
                    //qpsdiscount.ZoneId = resultContext.ZoneId;
                    //qpsdiscount.StateId = resultContext.StateId;
                    //qpsdiscount.OilTypeId = resultContext.OilTypeId;
                    //qpsdiscount.QPSdiscount = _emamiContext.SlabDiscountDetails.Where(_ => _.QPSId == resultContext.Id).Select(_ => new QPSDetails {
                    //    SlabName = _.SlabName,
                    //    FromRange = _.FromRange,
                    //    ToRange = _.ToRange,
                    //    Discount = _.DiscountAmount

                    //}).ToList();
                    if (resultContext.IsActive)
                    {
                        qpsdiscount.OilTypeId = _emamiContext.QPSDiscountSkuMapping
                                            .Where(sku => sku.QpsDiscountId == resultContext.Id && sku.IsActive)
                                            .Select(sku => sku.OilTypeId)
                                            .ToList();
                    }
                    else
                    {
                        qpsdiscount.OilTypeId = _emamiContext.QPSDiscountSkuMapping
                                            .Where(sku => sku.QpsDiscountId == resultContext.Id)
                                            .Select(sku => sku.OilTypeId)
                                            .ToList();
                    }
                    
                    qpsdiscount.StateId = _emamiContext.QPSDiscountSkuMapping
                                            .Where(s => s.QpsDiscountId == resultContext.Id)
                                            .Select(s => s.StateId)
                                            .ToList();

                    qpsdiscount.ZoneId = _emamiContext.ZoneStateMappings
                                         .Where(zs => qpsdiscount.StateId.Contains(zs.StateId))
                                         .Select(zs => zs.ZoneId)
                                         .Distinct()
                                         .ToList();

                    qpsdiscount.SkuIds = _emamiContext.QPSDiscountSkuMapping
                                       .Where(sku => sku.QpsDiscountId == resultContext.Id)
                                       .Select(sku => sku.SkuId)
                                       .ToList();

                    qpsdiscount.QPSSlabDetails = _emamiContext.QPSSlabDetails
                                               .Where(slab => slab.QpsDiscountId == resultContext.Id)
                                               .Select(slab => new QPSSlabDetailsDto
                                               {
                                                   FromRange = slab.FromRange,
                                                   ToRange = slab.ToRange,
                                                   Discount = slab.Discount
                                               }).ToList();
                    qpsdiscount.IsActive = resultContext.IsActive;
                }

                return _resultService.SuccessObject(qpsdiscount);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";


                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }
        public ResultDto GetQpsDiscountById(string inputDto)
        {
            _methodName = "GetQpsDiscountById";
            var resultDto = new ResultDto();
            var qpsdiscount = new QPSSchemeDiscountDto();
            try
            {
                var decryptedId = UtilityHelper.ConvertMd5ToString(inputDto, SecurityConstants.EncryptionKey);

                var Id = UtilityHelper.LongTryToParse(decryptedId);
                var resultContext = _emamiContext.QpsDiscount.AsNoTracking().FirstOrDefault(_ => _.Id == Id);
                if (resultContext != null)
                {
                    qpsdiscount.EncryptedId = UtilityHelper.ConvertToMd5(resultContext.Id.ToString(), SecurityConstants.EncryptionKey);
                    qpsdiscount.Id = resultContext.Id;
                    qpsdiscount.StartDate = resultContext.StartDate;
                    qpsdiscount.EndDate = resultContext.EndDate;
                    qpsdiscount.SalesOrgId = resultContext.SalesOrgId;
                    qpsdiscount.DistributionChannelId = resultContext.DistributionChannelId;
                    qpsdiscount.DivisionId = resultContext.DivisionId;
                    //qpsdiscount.ZoneId = resultContext.ZoneId;
                    //qpsdiscount.StateId = resultContext.StateId;
                    //qpsdiscount.OilTypeId = resultContext.OilTypeId;
                    qpsdiscount.IsActive = resultContext.IsActive;
                }

                return _resultService.SuccessObject(qpsdiscount);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";


                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        public ResultDto ExportQpsSchemeDiscount(LoginUserIdDto inputDto)
        {
            _methodName = "ExportQpsSchemeDiscount";
            var resultDto = new ResultDto();
            try
            {
                var qpsDiscounts = _emamiContext.QpsDiscount.AsNoTracking().ToList();
                var qpsSkumaps = _emamiContext.QPSDiscountSkuMapping.AsNoTracking().ToList();
                var oilTypes = _emamiContext.OilTypes.AsNoTracking().ToList();
                var zoneMappings = _emamiContext.ZoneStateMappings.AsNoTracking().ToList();
                var zones = _emamiContext.Zones.AsNoTracking().ToList();
                var states = _emamiContext.State.AsNoTracking().ToList();

                var joinedData = qpsDiscounts.Join(qpsSkumaps,
                                qd => qd.Id, qsm => qsm.QpsDiscountId, (qd, qsm) => new { QPSSchemeDiscountDto = qd, QPSDiscountSkuMapping = qsm })
                                    .GroupBy(item => new { item.QPSSchemeDiscountDto.Id })
                                    .Select(group => new
                                    {
                                        Id = group.Key.Id,
                                        Items = group.FirstOrDefault()
                                    })
                                    .ToList();

                var qpsZoneMap = (from qs in qpsSkumaps
                                  join zm in zoneMappings
                                  on qs.StateId equals zm.StateId
                                  select new { StateId = qs.StateId, ZoneId = zm.ZoneId, QPSDiscountId = qs.QpsDiscountId })
                 .ToList();

                var combinedList = joinedData.Select(_ => new QPSSchemeDiscountDto
                {
                    Id = _.Items.QPSSchemeDiscountDto.Id,
                    StartDate = _.Items.QPSSchemeDiscountDto.StartDate,
                    EndDate = _.Items.QPSSchemeDiscountDto.EndDate,
                    SalesOrgId = _.Items.QPSSchemeDiscountDto.SalesOrgId,
                    DistributionChannelId = _.Items.QPSSchemeDiscountDto.DistributionChannelId,
                    DivisionId = _.Items.QPSSchemeDiscountDto.DivisionId,
                    OilTypeId = qpsSkumaps
                        .Where(qsm => qsm.QpsDiscountId == _.Items.QPSSchemeDiscountDto.Id)
                        .Select(qsm => qsm.OilTypeId)
                        .Distinct().ToList(),
                    OilTypeName = string.Join(", ", qpsSkumaps
                        .Where(qsm => qsm.QpsDiscountId == _.Items.QPSSchemeDiscountDto.Id)
                        .Select(qsm => qsm.OilTypeId)
                        .Distinct().Join(oilTypes, id => id, o => o.Id, (id, o) => o.Name).ToList()),
                    IsActive = _.Items.QPSSchemeDiscountDto.IsActive,
                    SkuIds = qpsSkumaps.Where(qsm => qsm.QpsDiscountId == _.Items.QPSSchemeDiscountDto.Id).Select(qsm => qsm.SkuId).Distinct().ToList(),
                    StateId = qpsSkumaps.Where(qsm => qsm.QpsDiscountId == _.Items.QPSSchemeDiscountDto.Id).Select(qsm => qsm.StateId).Distinct().ToList(),
                    ZoneId = qpsZoneMap.Where(zsm => zsm.QPSDiscountId == _.Items.QPSSchemeDiscountDto.Id).Select(zsm => zsm.ZoneId).Distinct().ToList(),
                    EncryptedId = UtilityHelper.ConvertToMd5(_.Items.QPSSchemeDiscountDto.Id.ToString(), SecurityConstants.EncryptionKey),
                    ZoneName = string.Join(", ", qpsZoneMap.Where(zsm => zsm.QPSDiscountId == _.Items.QPSSchemeDiscountDto.Id)
                                .Select(zsm => zsm.ZoneId)
                                .Distinct()
                                .Join(zones, zId => zId, z => z.Id, (zId, z) => z.Name)
                                .ToList()),
                    StateName = string.Join(", ", qpsSkumaps.Where(qsm => qsm.QpsDiscountId == _.Items.QPSSchemeDiscountDto.Id)
                                .Select(qsm => qsm.StateId)
                                .Distinct()
                                .Join(states, sId => sId, s => s.Id, (sId, s) => s.StateName)
                                .ToList()),
                }).OrderByDescending(_ => _.Id).ToList();

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = combinedList;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto = _resultService.ErrorMessage(Constants.Exception);
            }
            return resultDto;
        }

        public ResultDto GetQpsDiscountListWithPagination(KendoGridResult inputDto)
        {
            _methodName = "GetQpsDiscountListWithPagination";
            var resultDto = new ResultDto();
            var outputDto = new List<QPSSchemeDiscountDto>();
            try
            {
                List<QpsDiscount> qpsdiscounts;
                if (inputDto.IsToReturnInactiveData)
                {
                    qpsdiscounts = _emamiContext.QpsDiscount.AsNoTracking().ToList();
                }
                else
                {
                    qpsdiscounts = _emamiContext.QpsDiscount.AsNoTracking().Where(w => w.IsActive).ToList();
                }

                if (qpsdiscounts != null && qpsdiscounts.Any())
                {
                    outputDto = qpsdiscounts
                    //    .GroupJoin(_emamiContext.CompetitorSku.AsQueryable().Where(_ => _.Sku != null), c => c.Id, cs => cs.CompetitorId, (c, cs) => new { c, Skus = cs.Select(_ => _.Sku.SkuCode) }).ToList().AsQueryable()

                        .Select(s => new QPSSchemeDiscountDto()
                        {
                            StartDate = s.StartDate,
                            EndDate = s.EndDate,
                            SalesOrgId = s.SalesOrgId,
                            DistributionChannelId = s.DistributionChannelId,
                            DivisionId = s.DivisionId,
                            //ZoneId = s.ZoneId,
                            //OilTypeId = s.OilTypeId,
                            //StateId = s.StateId
                        }).ToList();

                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = outputDto != null ? outputDto.OrderByDescending(_ => _.Id).ToDataSourceResult(inputDto.DataSourceRequest) : outputDto.ToDataSourceResult(inputDto.DataSourceRequest);
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.RecordNotFound;
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
            }
            return resultDto;
        }

        public ResultDto GetQPSDiscountWithSlab(SkuQpsInputDto inputDto)
        {
            _methodName = "GetQPSDiscountWithSlab";
            var resultDto = new ResultDto();
            var outputDto = new List<SkuQpsDiscountResultDto>();
            try
            {
                var usercontext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
                if (usercontext != null)
                {
                    foreach (var input in inputDto.SkuDetails)
                    {
                        var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);

                        outputDto = _emamiContext.Skus.AsNoTracking().Join(_emamiContext.QPSDiscountSkuMapping.AsNoTracking(), s => s.OilTypeId, q => q.OilTypeId,
                            (s, q) => new { skus = s, qpsDiscountSku = q }).Join(_emamiContext.QpsDiscount.AsNoTracking(), _ => _.qpsDiscountSku.QpsDiscountId, qd => qd.Id,
                            (_, qd) => new { skus = _.skus, qpsDiscountSku = _.qpsDiscountSku, qpsDiscount = qd }).Join(_emamiContext.QPSSlabDetails.AsNoTracking(),
                            _ => _.qpsDiscountSku.QpsDiscountId, sd => sd.QpsDiscountId, (_, sd) => new { skus = _.skus, qpsDiscountSku = _.qpsDiscountSku, qpsDiscount = _.qpsDiscount, slab = sd })
                            .Join(_emamiContext.ZoneStateMappings.AsNoTracking(), _ => _.qpsDiscountSku.StateId, z => z.StateId,
                            (_, z) => new { skus = _.skus, qpsDiscountSku = _.qpsDiscountSku, qpsDiscount = _.qpsDiscount, slab = _.slab, zone = z })
                            .Where(_ => (_.qpsDiscountSku.SkuId == input.SkuId || (_.qpsDiscountSku.SkuId == 0 && _.qpsDiscountSku.OilTypeId == _.skus.OilTypeId)) && _.skus.Id == input.SkuId && _.qpsDiscountSku.StateId == usercontext.StateId && _.zone.ZoneId == usercontext.ZoneId && _.skus.IsActive && _.qpsDiscountSku.IsActive &&
                             (DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.qpsDiscount.StartDate)) &&
                             (DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.qpsDiscount.EndDate)))
                            .Select(s => new SkuQpsDiscountResultDto
                             {
                                 SkuId = s.skus.Id,
                                 FromRange = s.slab.FromRange,
                                 ToRange = s.slab.ToRange,
                                 Discount = s.slab.Discount,
                                 SkuType = s.qpsDiscountSku.SkuId == 0 ? 0 : 1,
                                 QpsDiscountId = s.qpsDiscount.Id
                             }).Distinct().OrderBy(s => s.QpsDiscountId).ThenBy(s => s.FromRange).ThenByDescending(s => s.SkuType).ToList();
                    }
                    if (outputDto == null || !outputDto.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.QPSDiscountNotAvailable;
                        return resultDto;
                    }
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }
                
                

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
            }
            return resultDto;
        }

        public ResultDto GetQPSDiscountForQuantity(SkuQpsInputDto inputDto)
        {
            _methodName = "GetQPSDiscountForQuantity";
            var resultDto = new ResultDto();
            var outputDto = new List<MultipleSkuQpsDiscountResultDto>();
            try
            {

                var usercontext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);

                if (usercontext != null)
                {
                    var UT_skuQps = Constants.ToDataTable(inputDto.SkuDetails);

                    using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
                    {
                        var parameters = new { skuQps = UT_skuQps,ZoneId=usercontext.ZoneId,StateId=usercontext.StateId };
                        outputDto = conn.Query<MultipleSkuQpsDiscountResultDto>("GetQpsDiscountDetailForSku",
                            parameters,
                            commandType: CommandType.StoredProcedure
                            ).ToList();
                    }

                    if (outputDto == null || !outputDto.Any())
                    {
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.Message = Constants.QPSDiscountNotAvailable;
                        return resultDto;
                    }
                }
                else
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.UserNotFound;
                    return resultDto;
                }

               

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = outputDto;
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.Message = Constants.Exception;
            }
            return resultDto;
        }
        
        private List<(long SkuId, long StateId, long OilTypeId)> GetCombinations(List<long> skuIds, List<long> stateIds,List<long> oilTypeIds)
        {
            var combinations = new List<(long SkuId, long StateId , long OilTypeId)>();
            var combinations1 = new List<(long SkuId, long StateId , long OilTypeId)>();
            var uniqueCombinations = new HashSet<(long SkuId, long StateId, long OilTypeId)>();
            var uniqueCombinations1 = new HashSet<(long SkuId, long StateId, long OilTypeId)>();
            foreach (long oilTypeId in oilTypeIds)
            {
                var validSkus = _emamiContext.Skus.AsNoTracking().Where(s => s.OilTypeId == oilTypeId && s.IsActive).Select(s => s.Id).ToList();
                var commonSku = validSkus.Where(v => skuIds.Contains(v)).ToList();
                if (commonSku.Any())
                {
                    foreach (long skuId in commonSku)
                    {
                        foreach (long stateId in stateIds)
                        {
                            var combination = (skuId, stateId, oilTypeId);
                            if (uniqueCombinations1.Add(combination))
                            {
                                combinations1.Add(combination);
                            }
                        }
                    }
                }
                else
                {
                    long skuId = 0;
                    foreach (long stateId in stateIds)
                    {
                        var combination = (skuId, stateId, oilTypeId);
                        if (uniqueCombinations1.Add(combination))
                        {
                            combinations1.Add(combination);
                        }
                    }
                }
                
                //foreach (long skuId in skuIds)
                //{
                //    long finalSkuId = validSkus.Contains(skuId) ? skuId : 0;
                //    foreach (long stateId in stateIds)
                //    {
                //        var combination = (finalSkuId, stateId, oilTypeId);
                //        if (uniqueCombinations.Add(combination)) // Only add if it's unique
                //        {
                //            combinations.Add(combination);
                //        }
                //    }
                //}
            }
            return combinations1;
        }
    }    
}
