using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.Service.Common;
using GMCore.Helper;
using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Adani.Solution.Service
{
    public interface IMobileDealerStockService
    {
        ResultDto GetSkuListForStockEntry(LoginUserIdDto inputDto);
        ResultDto SaveDistributorStockEntry(DistributorStockEntrySaveDto inputDto);
        ResultDto GetDistributorStockEntryList(LoginUserIdDto inputDto);
        ResultDto GetDealerLatestStockPerSku(LoginUserIdDto inputDto);
    }

    public class MobileDealerStockService : IMobileDealerStockService
    {
        private readonly IAdaniContext _emamiContext;
        private const string ServiceName = "Mobile Dealer Stock Service";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private string _methodName;
        private readonly IResultService _resultService;

        public MobileDealerStockService(IAdaniContext salesContext, IResultService resultService)
        {
            try
            {
                _emamiContext = salesContext;
                _resultService = resultService;
                _emamiContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for mobile dealer stock Service", exception);
            }
        }

        /// <summary>
        /// Sku dropdown for the distributor stock entry screen. Skus are scoped to the
        /// distributor's SalesOrganization/DistributionChannel/Division combinations and
        /// carry the case to metric ton conversion value for the mobile app.
        /// </summary>
        public ResultDto GetSkuListForStockEntry(LoginUserIdDto inputDto)
        {
            _methodName = "GetSkuListForStockEntry";
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

                IEnumerable<DivisionDetailsDto> combinationList = _emamiContext.UserDivisionMappings.AsNoTracking()
                    .Where(s => s.UserId == inputDto.LoginUserId)
                    .Select(s => new DivisionDetailsDto
                    {
                        SalesOrganizationId = s.SalesOrganizationId,
                        DistributionChannelId = s.DistributionChannelId,
                        DivisionId = s.DivisionId
                    });

                var skuList = (from s in _emamiContext.Skus.AsNoTracking()
                               join divm in combinationList on new { s.SalesOrganizationId, s.DistributionChannelId, s.DivisionId } equals
                               new { divm.SalesOrganizationId, divm.DistributionChannelId, divm.DivisionId }
                               where s.IsActive
                               select new
                               {
                                   SkuId = s.Id,
                                   SkuName = s.SkuName + "/" + s.SkuCode,
                                   SkuCode = s.SkuCode
                               }).Distinct().ToList();

                if (skuList == null || !skuList.Any())
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                var skuIds = skuList.Select(s => s.SkuId).ToList();
                var skuUomMappings = _emamiContext.SkuUomMapping.AsNoTracking()
                    .Where(m => skuIds.Contains(m.SkuId))
                    .Select(m => new SkuUomMappingDto
                    {
                        Id = m.Id,
                        SkuId = m.SkuId,
                        UomId = m.UomId,
                        ConversionFactor = m.ConversionFactor,
                        ConversionFactor1 = m.ConversionFactor1,
                        ConversionFactor2 = m.ConversionFactor2
                    }).ToList();

                var skuData = skuList.OrderBy(s => s.SkuName).Select(s => new DropDownDto
                {
                    Id = s.SkuId,
                    Code = s.SkuCode,
                    Name = s.SkuName,
                    CaseToMetricTonValue = _resultService.ConvertCasetoMetricTonWithoutDB(1, s.SkuId, skuUomMappings)
                }).ToList();

                return _resultService.SuccessObject(skuData);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        /// <summary>
        /// Saves one distributor stock entry with its Sku lines. Entries are append only -
        /// corrections are made by submitting a new entry. Quantity in MT is derived from
        /// the number of cases and persisted so later conversion factor changes do not
        /// rewrite the reported history.
        /// </summary>
        public ResultDto SaveDistributorStockEntry(DistributorStockEntrySaveDto inputDto)
        {
            _methodName = "SaveDistributorStockEntry";
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
                if (!userContext.IsActive)
                {
                    return _resultService.ErrorMessage(Constants.InActiveUser);
                }

                var isDealer = _emamiContext.UserRoles.AsNoTracking()
                    .Any(_ => _.UserId == inputDto.LoginUserId && _.RoleId == (int)DTO.Enums.Role.Dealer);
                if (!isDealer)
                {
                    return _resultService.ErrorMessage(Constants.Unauthorised);
                }

                if (inputDto.SkuList == null || !inputDto.SkuList.Any())
                {
                    return _resultService.ErrorMessage(Constants.SkuEmpty);
                }
                if (inputDto.SkuList.Any(_ => _.NoOfCases <= 0))
                {
                    return _resultService.ErrorMessage(Constants.InvalidStockQuantity);
                }

                var mergedSkuList = inputDto.SkuList
                    .GroupBy(_ => _.SkuId)
                    .Select(g => new DistributorStockSkuInputDto
                    {
                        SkuId = g.Key,
                        NoOfCases = g.Sum(_ => _.NoOfCases)
                    }).ToList();

                var skuIds = mergedSkuList.Select(_ => _.SkuId).ToList();
                var validSkuIds = _emamiContext.Skus.AsNoTracking().Where(_ => skuIds.Contains(_.Id)).Select(_ => _.Id).ToList();
                if (validSkuIds.Count != skuIds.Count)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                var skuUomMappings = _emamiContext.SkuUomMapping.AsNoTracking()
                    .Where(m => skuIds.Contains(m.SkuId))
                    .Select(m => new SkuUomMappingDto
                    {
                        Id = m.Id,
                        SkuId = m.SkuId,
                        UomId = m.UomId,
                        ConversionFactor = m.ConversionFactor,
                        ConversionFactor1 = m.ConversionFactor1,
                        ConversionFactor2 = m.ConversionFactor2
                    }).ToList();

                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var stockEntry = new DistributorStockEntry
                {
                    UserId = inputDto.LoginUserId,
                    ReportedDate = currentDate,
                    CreatedBy = inputDto.LoginUserId,
                    CreatedDate = currentDate
                };
                _emamiContext.DistributorStockEntries.Add(stockEntry);

                foreach (var skuLine in mergedSkuList)
                {
                    _emamiContext.DistributorStockEntryDetails.Add(new DistributorStockEntryDetail
                    {
                        DistributorStockEntry = stockEntry,
                        SkuId = skuLine.SkuId,
                        QuantityInCase = skuLine.NoOfCases,
                        QuantityInMT = _resultService.ConvertCasetoMetricTonWithoutDB(skuLine.NoOfCases, skuLine.SkuId, skuUomMappings),
                        CreatedBy = inputDto.LoginUserId,
                        CreatedDate = currentDate
                    });
                }
                _emamiContext.SaveChanges();

                return _resultService.SuccessMessage(Constants.StockReportedSuccessfully);
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }
        }

        /// <summary>
        /// Paginated list of the distributor's own stock entries - one item per entry with
        /// the reported Sku lines nested for the expandable grid row in the mobile app.
        /// </summary>
        public ResultDto GetDistributorStockEntryList(LoginUserIdDto inputDto)
        {
            _methodName = "GetDistributorStockEntryList";
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

                var outputDto = new DistributorStockEntryListOutputDto
                {
                    StockEntries = new List<DistributorStockEntryDto>()
                };

                var entryQuery = _emamiContext.DistributorStockEntries.AsNoTracking()
                    .Where(e => e.UserId == inputDto.LoginUserId);

                outputDto.ListCount = entryQuery.Count();

                var pageSize = Constants.PageSize;
                var skip = pageSize * inputDto.PageNo;
                var entries = entryQuery
                    .OrderByDescending(e => e.ReportedDate)
                    .ThenByDescending(e => e.Id)
                    .Skip(skip).Take(pageSize)
                    .Select(e => new { e.Id, e.ReportedDate }).ToList();

                if (entries.Any())
                {
                    var entryIds = entries.Select(e => e.Id).ToList();
                    var details = (from d in _emamiContext.DistributorStockEntryDetails.AsNoTracking()
                                   join s in _emamiContext.Skus.AsNoTracking() on d.SkuId equals s.Id
                                   where entryIds.Contains(d.DistributorStockEntryId)
                                   select new
                                   {
                                       d.DistributorStockEntryId,
                                       d.SkuId,
                                       s.SkuName,
                                       s.SkuCode,
                                       d.QuantityInCase,
                                       d.QuantityInMT
                                   }).ToList();

                    outputDto.StockEntries = entries.Select(e =>
                    {
                        var entryDetails = details.Where(d => d.DistributorStockEntryId == e.Id)
                            .OrderBy(d => d.SkuName)
                            .Select(d => new DistributorStockSkuDetailDto
                            {
                                SkuId = d.SkuId,
                                SkuName = d.SkuName,
                                SkuCode = d.SkuCode,
                                QuantityInCase = d.QuantityInCase,
                                QuantityInMT = d.QuantityInMT
                            }).ToList();

                        return new DistributorStockEntryDto
                        {
                            EntryId = e.Id,
                            ReportedDate = e.ReportedDate,
                            SkuCount = entryDetails.Count,
                            TotalQuantityInCase = entryDetails.Sum(d => d.QuantityInCase),
                            TotalQuantityInMT = entryDetails.Sum(d => d.QuantityInMT),
                            SkuDetails = entryDetails
                        };
                    }).ToList();
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

        /// <summary>
        /// Latest reported stock per Sku for the selected distributor (DealerId). Used by
        /// State/Zonal/National traders after drilling down to a distributor.
        /// </summary>
        public ResultDto GetDealerLatestStockPerSku(LoginUserIdDto inputDto)
        {
            _methodName = "GetDealerLatestStockPerSku";
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
                if (inputDto.DealerId == 0)
                {
                    return _resultService.ErrorMessage(Constants.InvalidRequest);
                }

                var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
                if (dealerContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var callerRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.LoginUserId);
                if (callerRoleContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                if (callerRoleContext.RoleId != (int)DTO.Enums.Role.Admin)
                {
                    var bdoIds = new List<long>();
                    if (callerRoleContext.RoleId == (int)DTO.Enums.Role.NationalTrader)
                    {
                        var zhIds = _emamiContext.UserReportingToMappings.AsNoTracking()
                            .Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                        bdoIds = _emamiContext.UserReportingToMappings.AsNoTracking()
                            .Where(_ => zhIds.Contains(_.ReportingToUserId)).Select(_ => _.UserId).ToList();
                    }
                    else if (callerRoleContext.RoleId == (int)DTO.Enums.Role.ZonalTrader)
                    {
                        bdoIds = _emamiContext.UserReportingToMappings.AsNoTracking()
                            .Where(_ => _.ReportingToUserId == inputDto.LoginUserId).Select(_ => _.UserId).ToList();
                    }
                    else if (callerRoleContext.RoleId == (int)DTO.Enums.Role.StateTrader)
                    {
                        bdoIds.Add(inputDto.LoginUserId);
                    }
                    else
                    {
                        return _resultService.ErrorMessage(Constants.Unauthorised);
                    }

                    var isDealerUnderCaller = bdoIds.Any() && _emamiContext.UserCustomerMapping.AsNoTracking()
                        .Any(_ => bdoIds.Contains(_.UserId) && _.CustomerId == inputDto.DealerId);
                    if (!isDealerUnderCaller)
                    {
                        return _resultService.ErrorMessage(Constants.Unauthorised);
                    }
                }

                var stockRows = (from e in _emamiContext.DistributorStockEntries.AsNoTracking()
                                 join d in _emamiContext.DistributorStockEntryDetails.AsNoTracking() on e.Id equals d.DistributorStockEntryId
                                 join s in _emamiContext.Skus.AsNoTracking() on d.SkuId equals s.Id
                                 where e.UserId == inputDto.DealerId
                                 select new
                                 {
                                     d.SkuId,
                                     s.SkuName,
                                     s.SkuCode,
                                     d.QuantityInCase,
                                     d.QuantityInMT,
                                     e.ReportedDate,
                                     EntryId = e.Id
                                 }).ToList();

                if (!stockRows.Any())
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                var latestStockList = stockRows
                    .GroupBy(r => r.SkuId)
                    .Select(g => g.OrderByDescending(r => r.ReportedDate).ThenByDescending(r => r.EntryId).FirstOrDefault())
                    .OrderBy(r => r.SkuName)
                    .Select(r => new DealerLatestStockOutputDto
                    {
                        SkuId = r.SkuId,
                        SkuName = r.SkuName,
                        SkuCode = r.SkuCode,
                        QuantityInCase = r.QuantityInCase,
                        QuantityInMT = r.QuantityInMT,
                        ReportedDate = r.ReportedDate
                    }).ToList();

                return _resultService.SuccessObject(latestStockList);
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
