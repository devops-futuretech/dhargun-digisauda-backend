using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using Aspose.Cells;
using Dapper;
using GMCore.Helper;
using GMCore.Logger;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using TagLib.Ape;

namespace Adani.Solution.Service
{
    public interface ICrossAndUpsellService
    {
        ResultDto AddCrossAndUpsellConfigurations(CrossAndUpsellConfigurationDto inputDto);
        ResultDto UpdateCrossAndUpsellConfigurations(CrossAndUpsellConfigurationDto inputDto);
        ResultDto GetCrossAndUpsellConfigurationList(SuadaConditionalBookingInputDto inputDto);
        ResultDto GetCrossAndUpsellConfigurationSkusList(SuadaConditionalBookingInputDto inputDto);
        ResultDto GetCrossAndUpsellConfigurationDetails(SuadaConditionalBookingInputDto inputDto);
        ResultDto GetCrossAndUpsellMandatorySkusConfigurationDetails(SuadaConditionalBookingSkusInputDto inputDto);
        ResultDto GetCrossAndUpsellConfigurationListForReport(SuadaConditionalBookingInputDto inputDto);
    }

    public class CrossAndUpsellService : ICrossAndUpsellService
    {
        private const string ServiceName = "Dealer Controller";
        private readonly ILogger _logger = Logging.GetLogger(ServiceName);
        private readonly IAdaniContext _adaniContext;
        private readonly IResultService _resultService;
        private string _methodName;

        public CrossAndUpsellService(IAdaniContext adaniContext, IResultService resultService)
        {
            _adaniContext = adaniContext;
            _resultService = resultService;
        }

        public ResultDto AddCrossAndUpsellConfigurations(CrossAndUpsellConfigurationDto inputDto)
        {
            _methodName = "AddAndUpdateCrossAndUpsellConfigurations";
            var resultDto = new ResultDto();

            try
            {
                if (inputDto?.LoginUserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = inputDto == null ? Constants.InvalidRequest : Constants.InvalidUser;
                    return resultDto;
                }

                using (var dbContextTransaction = _adaniContext.Database.BeginTransaction())
                {
                    try
                    {
                        var entityModel = new SaudaConditionalBookingConfiguration
                        {
                            SalesOrganizationId = inputDto.SalesOrganizationId,
                            DistributionChannelId = inputDto.DistributionChannelId,
                            DivisionId = inputDto.DivisionId,
                            StartDate = inputDto.StartDate,
                            EndDate = inputDto.EndDate,
                            CreatedBy = inputDto.LoginUserId,
                            CreatedDate = DateTime.Now,
                            IsActive = inputDto.IsActive
                        };

                        _adaniContext.SaudaConditionalBookingConfigurations.Add(entityModel);
                        _adaniContext.SaveChanges();

                        foreach (var zoneId in inputDto.ZoneId)
                        {
                            foreach (var stateId in inputDto.StateId)
                            {
                                var zoneStateMapping = _adaniContext.ZoneStateMappings.AsNoTracking().FirstOrDefault(x => x.ZoneId == zoneId && x.StateId == stateId);
                                if (zoneStateMapping != null)
                                {
                                    var mapping = new SaudaConditionalBookingZoneStateMapping
                                    {
                                        SaudaConditionalConfigurationId = entityModel.Id,
                                        StateId = stateId,
                                        ZoneId = zoneId,
                                        CreatedBy = inputDto.LoginUserId,
                                        CreatedDate = DateTime.Now
                                    };

                                    _adaniContext.SaudaConditionalBookingZoneStateMappings.Add(mapping);
                                }
                            }
                        }

                        _adaniContext.SaveChanges();


                        if (inputDto.SkuBookingCombinationList.Any())
                        {
                            foreach (var sauda in inputDto.SkuBookingCombinationList)
                            {
                                var essentialSkuMapping = new SaudaConditionalBookingEssentialSkuMapping
                                {
                                    SaudaConditionalConfigurationId = entityModel.Id,
                                    EssentialSkuId = sauda.EssentialSkuId.Any() ? string.Join(",", sauda.EssentialSkuId) : string.Empty,
                                    OilTypeId = string.Join(",", sauda.EssentialOilTypeId),
                                    PackGroupId =  sauda.EssentialPackGroupId,
                                    IsActive = sauda.IsActive,
                                    CreatedBy = inputDto.LoginUserId,
                                    CreatedDate = DateTime.Now
                                };

                                _adaniContext.SaudaConditionalBookingEssentialSkuMappings.Add(essentialSkuMapping);
                                _adaniContext.SaveChanges();

                                foreach (var mandatorySku in sauda.MandatorySkuMappingList)
                                {
                                    var mandatorySkuMapping = new SaudaConditionalBookingMandatorySkuMapping
                                    {
                                        ConditionalBookingEssentialSkuMappingId = essentialSkuMapping.Id,
                                        MandatorySkuId = mandatorySku.MandatorySkuId,
                                        MandatorySkuCode = mandatorySku.MandatorySkuCode,
                                        MandatorySkuPercentage = mandatorySku.MandatoryBookingQuantityPercentage,
                                        OilTypeId = mandatorySku.MandatoryOilTypeId,
                                        PackGroupId = mandatorySku.MandatoryPackGroupId,
                                        CreatedBy = inputDto.LoginUserId,
                                        CreatedDate = DateTime.Now
                                    };

                                    _adaniContext.SaudaConditionalBookingMandatorySkuMappings.Add(mandatorySkuMapping);
                                }
                            }

                            _adaniContext.SaveChanges();
                        }

                        dbContextTransaction.Commit();

                        DeactiveExistingSaudaConditionalConfigurationData(inputDto, entityModel.Id);
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        var message = $"{ServiceName} Service-Method {_methodName} Exception: {ex}";
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.ErrorCode = Constants.Exception;
                        resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                        _logger.Error(message);
                        return resultDto;
                    }
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Message = Constants.SaudaConditionalBookingConfigAddSuccessfully;
                return resultDto;
            }
            catch (Exception ex)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {ex}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto UpdateCrossAndUpsellConfigurations(CrossAndUpsellConfigurationDto inputDto)
        {
            _methodName = "UpdateCrossAndUpsellConfigurations";
            var resultDto = new ResultDto();

            try
            {
                if (inputDto?.LoginUserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = inputDto == null ? Constants.InvalidRequest : Constants.InvalidUser;
                    return resultDto;
                }

                if (inputDto.Id == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.IdEmpty;
                    return resultDto;
                }

                using (var dbContextTransaction = _adaniContext.Database.BeginTransaction())
                {
                    try
                    {
                        var saudaCondiConfigContext = _adaniContext.SaudaConditionalBookingConfigurations
                                          .FirstOrDefault(_ => _.Id == inputDto.Id);

                        if (saudaCondiConfigContext != null)
                        {
                            saudaCondiConfigContext.SalesOrganizationId = inputDto.SalesOrganizationId;
                            saudaCondiConfigContext.DistributionChannelId = inputDto.DistributionChannelId;
                            saudaCondiConfigContext.DivisionId = inputDto.DivisionId;
                            //saudaCondiConfigContext.OilTypeId = string.Join(",", inputDto.OilTypeId);
                            //saudaCondiConfigContext.PackGroupId = inputDto.PackGroupId;
                            saudaCondiConfigContext.StartDate = inputDto.StartDate;
                            saudaCondiConfigContext.EndDate = inputDto.EndDate;
                            saudaCondiConfigContext.ModifiedBy = inputDto.LoginUserId;
                            saudaCondiConfigContext.ModifiedDate = DateTime.Now;
                            saudaCondiConfigContext.IsActive = inputDto.IsActive;

                            _adaniContext.SaudaConditionalBookingConfigurations.AddOrUpdate(saudaCondiConfigContext);
                            _adaniContext.SaveChanges();

                            var existingMappings = _adaniContext.SaudaConditionalBookingZoneStateMappings
                                .Where(x => x.SaudaConditionalConfigurationId == saudaCondiConfigContext.Id)
                                .ToList();

                            if (existingMappings.Any())
                            {
                                foreach (var item in existingMappings)
                                {
                                    _adaniContext.SaudaConditionalBookingZoneStateMappings.Remove(item);
                                }

                                _adaniContext.SaveChanges();
                            }

                            foreach (var zoneId in inputDto.ZoneId)
                            {
                                foreach (var stateId in inputDto.StateId)
                                {
                                    var isValidMapping = _adaniContext.ZoneStateMappings
                                        .AsNoTracking()
                                        .Any(x => x.ZoneId == zoneId && x.StateId == stateId);

                                    if (isValidMapping)
                                    {
                                        var newMapping = new SaudaConditionalBookingZoneStateMapping
                                        {
                                            SaudaConditionalConfigurationId = saudaCondiConfigContext.Id,
                                            ZoneId = zoneId,
                                            StateId = stateId,
                                            CreatedBy = inputDto.LoginUserId,
                                            CreatedDate = DateTime.Now
                                        };

                                        _adaniContext.SaudaConditionalBookingZoneStateMappings.Add(newMapping);
                                    }
                                }
                            }

                            _adaniContext.SaveChanges();


                            var skuBookingCombinationList = _adaniContext.SaudaConditionalBookingEssentialSkuMappings.
                                             Where(_ => _.SaudaConditionalConfigurationId == saudaCondiConfigContext.Id).ToList();

                            if (inputDto.SkuBookingCombinationList.Any())
                            {
                                var unmatchedValues = skuBookingCombinationList
                                    .Where(x => !inputDto.SkuBookingCombinationList.Any(_ => _.Id == x.Id)).ToList();

                                if (unmatchedValues.Any())
                                {
                                    foreach (var item in unmatchedValues)
                                    {
                                        var mandatotorySkuMappingList = _adaniContext.SaudaConditionalBookingMandatorySkuMappings.Where(_ => _.ConditionalBookingEssentialSkuMappingId == item.Id).ToList();

                                        foreach (var mandatorySku in mandatotorySkuMappingList)
                                        {
                                            _adaniContext.SaudaConditionalBookingMandatorySkuMappings.Remove(mandatorySku);
                                        }

                                        _adaniContext.SaudaConditionalBookingEssentialSkuMappings.Remove(item);
                                    }

                                    _adaniContext.SaveChanges();
                                }

                                foreach (var sauda in inputDto.SkuBookingCombinationList)
                                {
                                    var saudaConfigSkuContext = _adaniContext.SaudaConditionalBookingEssentialSkuMappings.FirstOrDefault(_ => _.Id == sauda.Id && _.SaudaConditionalConfigurationId == saudaCondiConfigContext.Id);

                                    if (saudaConfigSkuContext != null)
                                    {
                                        saudaConfigSkuContext.EssentialSkuId = sauda.EssentialSkuId.Any() ? string.Join(",", sauda.EssentialSkuId) : string.Empty;
                                        saudaConfigSkuContext.IsActive = sauda.IsActive;
                                        saudaConfigSkuContext.ModifiedBy = inputDto.LoginUserId;
                                        saudaConfigSkuContext.ModifiedDate = DateTime.Now;

                                        foreach (var mandatorySku in sauda.MandatorySkuMappingList)
                                        {
                                            var isExistsMandatorySku = _adaniContext.SaudaConditionalBookingMandatorySkuMappings.FirstOrDefault(_ => _.ConditionalBookingEssentialSkuMappingId == saudaConfigSkuContext.Id && _.MandatorySkuId == mandatorySku.MandatorySkuId);
                                            if (isExistsMandatorySku != null)
                                            {
                                                isExistsMandatorySku.MandatorySkuPercentage = mandatorySku.MandatoryBookingQuantityPercentage;
                                                isExistsMandatorySku.ModifiedBy = inputDto.LoginUserId;
                                                isExistsMandatorySku.ModifiedDate = DateTime.Now;
                                            }
                                            else
                                            {
                                                var mandatorySkuMapping = new SaudaConditionalBookingMandatorySkuMapping
                                                {
                                                    ConditionalBookingEssentialSkuMappingId = saudaConfigSkuContext.Id,
                                                    MandatorySkuId = mandatorySku.MandatorySkuId,
                                                    MandatorySkuCode = mandatorySku.MandatorySkuCode,
                                                    MandatorySkuPercentage = mandatorySku.MandatoryBookingQuantityPercentage,
                                                    CreatedBy = inputDto.LoginUserId,
                                                    CreatedDate = DateTime.Now
                                                };

                                                _adaniContext.SaudaConditionalBookingMandatorySkuMappings.Add(mandatorySkuMapping);
                                            }
                                        }

                                        _adaniContext.SaveChanges();
                                    }
                                    else
                                    {
                                        var essentialSkuMapping = new SaudaConditionalBookingEssentialSkuMapping
                                        {
                                            SaudaConditionalConfigurationId = saudaCondiConfigContext.Id,
                                            EssentialSkuId = sauda.EssentialSkuId.Any() ? string.Join(",", sauda.EssentialSkuId) : string.Empty,
                                            OilTypeId = string.Join(",", sauda.EssentialOilTypeId),
                                            PackGroupId = sauda.EssentialPackGroupId,
                                            IsActive = sauda.IsActive,
                                            CreatedBy = inputDto.LoginUserId,
                                            CreatedDate = DateTime.Now
                                        };

                                        _adaniContext.SaudaConditionalBookingEssentialSkuMappings.Add(essentialSkuMapping);
                                        _adaniContext.SaveChanges();

                                        foreach (var mandatorySku in sauda.MandatorySkuMappingList)
                                        {
                                            var mandatorySkuMapping = new SaudaConditionalBookingMandatorySkuMapping
                                            {
                                                ConditionalBookingEssentialSkuMappingId = essentialSkuMapping.Id,
                                                MandatorySkuId = mandatorySku.MandatorySkuId,
                                                MandatorySkuCode = mandatorySku.MandatorySkuCode,
                                                MandatorySkuPercentage = mandatorySku.MandatoryBookingQuantityPercentage,
                                                OilTypeId = mandatorySku.MandatoryOilTypeId,
                                                PackGroupId = mandatorySku.MandatoryPackGroupId,
                                                CreatedBy = inputDto.LoginUserId,
                                                CreatedDate = DateTime.Now
                                            };

                                            _adaniContext.SaudaConditionalBookingMandatorySkuMappings.Add(mandatorySkuMapping);
                                        }
                                    }
                                }

                                _adaniContext.SaveChanges();
                            }
                            
                            dbContextTransaction.Commit();

                            DeactiveExistingSaudaConditionalConfigurationData(inputDto, saudaCondiConfigContext.Id,true);
                        }
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        var message = $"{ServiceName} Service-Method {_methodName} Exception: {ex}";
                        resultDto.IsSuccess = false;
                        resultDto.ErrorDto.ErrorCode = Constants.Exception;
                        resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                        _logger.Error(message);
                        return resultDto;
                    }
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Message = Constants.SaudaConditionalBookingConfigUpdateSuccessfully;
                return resultDto;
            }
            catch (Exception ex)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {ex}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetCrossAndUpsellConfigurationList(SuadaConditionalBookingInputDto inputDto)
        {
            _methodName = "GetCrossAndUpsellConfigurationList";
            var resultDto = new ResultDto();

            try
            {
                if (inputDto == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidRequest;
                    return resultDto;
                }

                if (inputDto.LoginUserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.InvalidUser;
                    return resultDto;
                }

                using (var con = new SqlConnection(Config.DBConnectionString))
                {
                   var saudaConditionalConfigList = con.Query<SaudaConditionalBookingConfigurationListDto>("[dbo].[usp_GetCrossAndUpsellConfigurationMasterList]",
                        commandType: System.Data.CommandType.StoredProcedure, commandTimeout: 0).ToList();

                    if (saudaConditionalConfigList.Any())
                    {
                        foreach (var item in saudaConditionalConfigList)
                        {
                            item.EncryptedId = UtilityHelper.ConvertToMd5(item.Id.ToString(), SecurityConstants.EncryptionKey);
                        }
                    }

                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = saudaConditionalConfigList;
                    return resultDto;
                }               
            }
            catch (Exception ex)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {ex}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(message);
                return resultDto;
            }
        }

        public ResultDto GetCrossAndUpsellConfigurationSkusList(SuadaConditionalBookingInputDto inputDto)
        {
            _methodName = "GetCrossAndUpsellConfigurationSkusList";
            var resultDto = new ResultDto();
            var saudaConditionalConfigSkuList = new List<SaudaConditionalBookingSkuMappingListDto>();

            try
            {
                if (inputDto?.LoginUserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = inputDto == null ? Constants.InvalidRequest : Constants.InvalidUser;
                    return resultDto;
                }

                if (inputDto.Id == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.IdEmpty;
                    return resultDto;
                }

                var saudaConditionalConfigSkuContext = (
                    from config in _adaniContext.SaudaConditionalBookingConfigurations.AsNoTracking()
                    join mapping in _adaniContext.SaudaConditionalBookingEssentialSkuMappings.AsNoTracking()
                    on config.Id equals mapping.SaudaConditionalConfigurationId
                    where config.Id == inputDto.Id
                    select mapping).ToList();

                if (saudaConditionalConfigSkuContext.Any())
                {
                    foreach (var mapping in saudaConditionalConfigSkuContext)
                    {
                        var skuIds = mapping.EssentialSkuId.Split(',').Select(s => Convert.ToInt64(s)).ToList();
                        var essentialSkuNames = _adaniContext.Skus.Where(sku => skuIds.Contains(sku.Id))
                            .Select(sku => new
                            {
                                SkuName = sku.SkuName + "-" + sku.SkuCode
                            }).ToList();

                        var MandatorySkuMappingList = _adaniContext.SaudaConditionalBookingMandatorySkuMappings.AsNoTracking().Where(_ => _.ConditionalBookingEssentialSkuMappingId == mapping.Id).ToList();

                        foreach (var mandatory in MandatorySkuMappingList)
                        {
                            var sku = _adaniContext.Skus.AsNoTracking().FirstOrDefault(m => m.Id == mandatory.MandatorySkuId);
                            saudaConditionalConfigSkuList.Add(new SaudaConditionalBookingSkuMappingListDto
                            {
                                Id = mapping.Id,
                                EncryptedId = UtilityHelper.ConvertToMd5(mapping.Id.ToString(), SecurityConstants.EncryptionKey),
                                EssentialSkuName = string.Join(", ", essentialSkuNames.Select(_ => _.SkuName)),
                                MandatorySkuId = mandatory.MandatorySkuId,
                                MandatorySkuName = sku.SkuName,
                                MandatorySkuCode = mandatory.MandatorySkuCode,
                                MandatoryBookingQuantityPercentage = mandatory.MandatorySkuPercentage,
                                IsActive = mapping.IsActive,
                                OilType = _adaniContext.OilTypes.AsNoTracking().Where(_ => _.Id == mandatory.OilTypeId)
                                          .Select(_ => _.Name + "-" + _.SalesOrganization.Code + "/" + _.DistributionChannel.Code + "/" + _.Division.Code)
                                          .FirstOrDefault() + "-" + 
                                          _adaniContext.OilPackingTypes.AsNoTracking().Where(_ => _.Id == mandatory.PackGroupId)
                                          .Select(_ => _.Name).FirstOrDefault()
                            });
                        }
                }
                    }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = saudaConditionalConfigSkuList;
                return resultDto;
            }
            catch (Exception ex)
            {
                var errorMessage = $"{ServiceName} Service-Method {_methodName} Exception: {ex}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(errorMessage);
                return resultDto;
            }
        }

        public ResultDto GetCrossAndUpsellConfigurationDetails(SuadaConditionalBookingInputDto inputDto)
        {
            _methodName = "GetCrossAndUpsellConfigurationDetails";
            var resultDto = new ResultDto();
            var saudaConditionalConfigDetails = new CrossAndUpsellConfigurationDto();

            try
            {
                if (inputDto?.LoginUserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = inputDto == null ? Constants.InvalidRequest : Constants.InvalidUser;
                    return resultDto;
                }

                if (inputDto.EncryptedId == null)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = Constants.IdEmpty;
                    return resultDto;
                }

                inputDto.EncryptedId = inputDto.EncryptedId.Replace(' ', '+');
                var decryptedId = UtilityHelper.ConvertMd5ToString(inputDto.EncryptedId, SecurityConstants.EncryptionKey);
                inputDto.Id = UtilityHelper.IntTryToParse(decryptedId);

                var saudaConditionalList = _adaniContext.SaudaConditionalBookingConfigurations.AsNoTracking()
                    .Where(_ => _.Id == inputDto.Id).ToList();

                if (saudaConditionalList.Any())
                {
                    saudaConditionalConfigDetails = (from scb in saudaConditionalList
                                                     select new CrossAndUpsellConfigurationDto
                                                     {
                                                         Id = scb.Id,
                                                         EncryptedId = UtilityHelper.ConvertToMd5(scb.Id.ToString(), SecurityConstants.EncryptionKey),
                                                         SalesOrganizationId = scb.SalesOrganization.Id,
                                                         DistributionChannelId = scb.DistributionChannel.Id,
                                                         DivisionId = scb.Division.Id,
                                                         //OilTypeId = scb.OilTypeId.Split(',', (char)StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList(),
                                                         //PackGroupId = scb.PackGroup.Id,
                                                         StartDate = scb.StartDate,
                                                         EndDate = scb.EndDate,
                                                         IsActive = scb.IsActive,
                                                         StateId = _adaniContext.SaudaConditionalBookingZoneStateMappings.Where(_ => _.SaudaConditionalConfigurationId == scb.Id).Select(x => x.StateId).ToList(),
                                                         ZoneId = _adaniContext.SaudaConditionalBookingZoneStateMappings.Where(_ => _.SaudaConditionalConfigurationId == scb.Id).Select(x => x.ZoneId).ToList(),
                                                         LoginUserId = inputDto.LoginUserId
                                                     }).FirstOrDefault();
                }

                var saudaConditionalConfigSkuList = (
                    from config in _adaniContext.SaudaConditionalBookingConfigurations.AsNoTracking()
                    join mapping in _adaniContext.SaudaConditionalBookingEssentialSkuMappings.AsNoTracking()
                        on config.Id equals mapping.SaudaConditionalConfigurationId
                    where config.Id == inputDto.Id
                    select mapping
                ).OrderByDescending(x => x.Id).ToList();

                var essentialSkuMappingList = new List<SaudaConditionalBookingSkuDto>();

                if (saudaConditionalConfigSkuList.Any())
                {
                    foreach (var saudaConditionalConfigSku in saudaConditionalConfigSkuList)
                    {
                        var essentialSku = saudaConditionalConfigSku.EssentialSkuId
                            .Split(',')
                            .ToList()
                            .ConvertAll(s => Convert.ToInt64(s));

                        if (essentialSku.Any())
                        {
                            var essentialSkuMappingData = (from skus in _adaniContext.Skus.AsNoTracking()
                                                           where essentialSku.Contains(skus.Id)
                                                           select skus).ToList();

                            if (essentialSkuMappingData.Any())
                            {
                                var mandatorySkuMappingList = _adaniContext.SaudaConditionalBookingMandatorySkuMappings.AsNoTracking()
                                    .Where(_ => _.ConditionalBookingEssentialSkuMappingId == saudaConditionalConfigSku.Id)
                                     .Join(_adaniContext.Skus.AsNoTracking(), skuMapping => skuMapping.MandatorySkuId, sku => sku.Id, (skuMapping, sku) => new { skuMapping, sku })
                                    .Select(_ => new SaudaConditionalBookingMandatorySkuMappingDto
                                    {                                        
                                        MandatoryBookingQuantityPercentage = _.skuMapping.MandatorySkuPercentage,
                                        MandatorySkuCode = _.skuMapping.MandatorySkuCode,
                                        MandatorySkuId = _.skuMapping.MandatorySkuId,
                                        ParentId = saudaConditionalConfigSku.Id,
                                        MandatorySkuName = _.sku.SkuName,
                                        MandatoryOilTypeId = _.skuMapping.OilTypeId,
                                        MandatoryPackGroupId = _.skuMapping.PackGroupId
                                    }).ToList();

                                essentialSkuMappingList.Add(
                                new SaudaConditionalBookingSkuDto
                                {
                                    Id = saudaConditionalConfigSku.Id,
                                    EssentialSkuId = essentialSkuMappingData.Select(_ => _.Id).ToList(),
                                    EssentialSkuName = string.Join(",", essentialSkuMappingData.Select(_ => _.SkuName)),
                                    EssentialSkuData = essentialSkuMappingData.Select(s => new SaudaConditionalBookingEssentialSkuMappingDto { SkuId = s.Id, SkuName = s.SkuName, SkuCode = s.SkuCode }).ToList(),
                                    IsActive = saudaConditionalConfigSku.IsActive,
                                    MandatorySkuMappingList = mandatorySkuMappingList,
                                    EssentialOilTypeId = essentialSkuMappingData.Select(_ => (long)_.OilTypeId).ToList(),
                                    EssentialPackGroupId = saudaConditionalConfigSku.PackGroupId
                                });
                            }
                        }
                    }

                    saudaConditionalConfigDetails.SkuBookingCombinationList = essentialSkuMappingList;
                }

                resultDto.IsSuccess = true;
                resultDto.SuccessDto.Response = saudaConditionalConfigDetails;
                return resultDto;
            }
            catch (Exception ex)
            {
                var errorMessage = $"{ServiceName} Service-Method {_methodName} Exception: {ex}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(errorMessage);
                return resultDto;
            }
        }

        public ResultDto GetCrossAndUpsellMandatorySkusConfigurationDetails(SuadaConditionalBookingSkusInputDto inputDto)
        {
            _methodName = "GetCrossAndUpsellConfigurationSkusDetails";
            var result = new ResultDto();
            var mandatorySkuMappingList = new List<SaudaConditionalBookingMandatorySkuPricingDto>();
            var skuOutput = new SaudaConditionalBookingSkuOutputDto();
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);

            try
            {
                if (inputDto?.LoginUserId == 0)
                {
                    result.IsSuccess = false;
                    result.ErrorDto.Message = inputDto == null ? Constants.InvalidRequest : Constants.InvalidUser;
                    return result;
                }

                var customerStateId = _adaniContext.Users.AsNoTracking()
                    .FirstOrDefault(u => u.Id == inputDto.DealerId)?.StateId ?? 0;

                var configurationData = (from config in _adaniContext.SaudaConditionalBookingConfigurations.AsNoTracking()
                                         join mapping in _adaniContext.SaudaConditionalBookingEssentialSkuMappings.AsNoTracking()
                                             on config.Id equals mapping.SaudaConditionalConfigurationId
                                         join zsmapping in _adaniContext.SaudaConditionalBookingZoneStateMappings.AsNoTracking()
                                         on config.Id equals zsmapping.SaudaConditionalConfigurationId
                                         where config.SalesOrganizationId == inputDto.SalesOrganizationId && config.DistributionChannelId == inputDto.DistributionChannelId &&
                                               config.DivisionId == inputDto.DivisionId &&
                                               config.IsActive && mapping.IsActive &&
                                               config.StartDate <= DateTime.Now &&
                                               config.EndDate >= DateTime.Now &&
                                               zsmapping.StateId == customerStateId
                                         select new { mapping, config })
                                         .OrderByDescending(_ => _.config.Id)
                                         .ThenBy(_ => _.config.CreatedDate)
                                         .Distinct().ToList();

                if (configurationData.Any())
                {
                    var configurationDataList = configurationData.Select(_ => _.mapping).ToList();

                    foreach (var item in configurationDataList)
                    {
                        var essentialSkuIds = item.EssentialSkuId.Split(',').Select(long.Parse).ToList();

                        using (var con = new SqlConnection(Config.DBConnectionString))
                        {
                             mandatorySkuMappingList = con.Query<SaudaConditionalBookingMandatorySkuPricingDto>("[dbo].[usp_GetMandatorySkuMappingList]",
                                new
                                {
                                    EssentialSkuMappingId = item.Id,
                                    PlantId = inputDto.PlantId,
                                    CurrentDate = currentDate
                                },
                                commandType:System.Data.CommandType.StoredProcedure,commandTimeout:0).ToList();
                        }

                        bool allEssentialSkusPresent = essentialSkuIds.All(id => inputDto.Skus.Select(_ => _.SkuId).Contains(id));

                        if (allEssentialSkusPresent && essentialSkuIds.Any())
                        {
                            var essentialSkus = _adaniContext.Skus.AsNoTracking().Where(sku => essentialSkuIds.Contains(sku.Id)).ToList();

                            if (essentialSkus.Any() && mandatorySkuMappingList.Any())
                            {
                                GetMaterialDiscountData(inputDto, ref mandatorySkuMappingList, essentialSkus.Select(s => s.Id).ToList());
                                skuOutput = new SaudaConditionalBookingSkuOutputDto
                                {
                                    Id = item.Id,
                                    EssentialSkuId = essentialSkus.Select(s => s.Id).ToList(),
                                    EssentialSkuName = essentialSkus.Select(s => s.SkuName).ToList(),
                                    EssentialSkuCode = essentialSkus.Select(s => s.SkuCode).ToList(),
                                    IsActive = item.IsActive,
                                    MandatorySkuMappingList = mandatorySkuMappingList
                                };

                                break;
                            }
                        }
                    }
                }

                result.IsSuccess = true;
                result.SuccessDto.Response = skuOutput;
                return result;
            }
            catch (Exception ex)
            {
                var errorMsg = $"{ServiceName} Service - Method {_methodName} Exception: {ex}";
                result.IsSuccess = false;
                result.ErrorDto.ErrorCode = Constants.Exception;
                result.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(errorMsg);
                return result;
            }
        }
        public ResultDto GetCrossAndUpsellConfigurationListForReport(SuadaConditionalBookingInputDto inputDto)
        {
            _methodName = "GetCrossAndUpsellConfigurationListForReport";
            var resultDto = new ResultDto();

            try
            {
                if (inputDto?.LoginUserId == 0)
                {
                    resultDto.IsSuccess = false;
                    resultDto.ErrorDto.Message = inputDto == null ? Constants.InvalidRequest : Constants.InvalidUser;
                    return resultDto;
                }

                using (var con = new SqlConnection(Config.DBConnectionString))
                {
                    var crossAndUpsellConfigurationReportList = con.Query<CrossAndUpsellConfigurationReportDto>(
                        "[dbo].[usp_GetCrossAndUpsellConfigurationList]",
                        commandType: System.Data.CommandType.StoredProcedure,
                        commandTimeout: 0
                    ).ToList();

                    resultDto.IsSuccess = true;
                    resultDto.SuccessDto.Response = crossAndUpsellConfigurationReportList;
                }

                return resultDto;
            }
            catch (Exception ex)
            {
                var errorMessage = $"{ServiceName} Service-Method {_methodName} Exception: {ex}";
                resultDto.IsSuccess = false;
                resultDto.ErrorDto.ErrorCode = Constants.Exception;
                resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
                _logger.Error(errorMessage);
                return resultDto;
            }
        }

        private void DeactiveExistingSaudaConditionalConfigurationData(CrossAndUpsellConfigurationDto inputDto, long currentConfigId,bool isUpdate = false)
        {
            try
            {
                var zoneIdTable = new DataTable();
                zoneIdTable.Columns.Add("Id", typeof(long));
                foreach (var id in inputDto.ZoneId)
                    zoneIdTable.Rows.Add(id);

                var stateIdTable = new DataTable();
                stateIdTable.Columns.Add("Id", typeof(long));
                foreach (var id in inputDto.StateId)
                    stateIdTable.Rows.Add(id);

                using (var con = new SqlConnection(Config.DBConnectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@SalesOrganizationId", inputDto.SalesOrganizationId);
                    parameters.Add("@DistributionChannelId", inputDto.DistributionChannelId);
                    parameters.Add("@DivisionId", inputDto.DivisionId);
                    parameters.Add("@OilTypeId",string.Join("," ,inputDto.OilTypeId));
                    parameters.Add("@PackGroupId", inputDto.PackGroupId);
                    parameters.Add("@CurrentConfigId", currentConfigId);
                    parameters.Add("@IsUpdate", isUpdate);
                    parameters.Add("@ZoneIds", zoneIdTable.AsTableValuedParameter("dbo.IdList"));
                    parameters.Add("@StateIds", stateIdTable.AsTableValuedParameter("dbo.IdList"));

                    con.Execute("[dbo].[usp_DeactivateExistingSaudaConfigurations]", parameters, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"{ServiceName} Service-Method {_methodName} Exception: {ex}";
                _logger.Error(errorMessage);
            }
        }

        private void GetMaterialDiscountData(SuadaConditionalBookingSkusInputDto inputDto, ref List<SaudaConditionalBookingMandatorySkuPricingDto> mandatorySkuMappingDtoList, List<long> SkuIds)
        {
            var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);

            try
            {
                var userContext = _adaniContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
                var userrole = _adaniContext.UserRoles.AsNoTracking().FirstOrDefault(user => user.UserId == inputDto.LoginUserId).RoleId;

                #region Get Common Data's

                foreach (var mandatorySkuMappingDto in mandatorySkuMappingDtoList)
                {
                    var discountGeographyDatas = _adaniContext.DiscountGeography.AsNoTracking()
                        .Where(_ => currentDate >= _.ValidFrom
                        && currentDate <= _.ValidTo
                        && ((_.CityId == userContext.CityId || _.CityId == 0) && (_.DistrictId == userContext.DistrictId || _.DistrictId == 0) && (_.StateId == userContext.StateId || _.StateId == 0) && _.ZoneId == userContext.ZoneId)
                        && _.SkuId == mandatorySkuMappingDto.MandatorySkuId)
                        .Select(s => new
                        {
                            Id = s.Id,
                            CityId = s.CityId,
                            ActualDiscount = s.ActualDiscount,
                            SkuId = s.SkuId
                        }).ToList();

                    var discountUserDatas = _adaniContext.DiscountUsers.AsNoTracking()
                        .Where(_ => _.ParentId != 0 && currentDate >= _.ValidFrom
                        && currentDate <= _.ValidTo
                        && _.UserId == inputDto.LoginUserId && _.SkuId == mandatorySkuMappingDto.MandatorySkuId)
                        .Select(s => new
                        {
                            Id = s.Id,
                            UserId = s.UserId,
                            ActualDiscount = s.ActualDiscount,
                            SkuId = s.SkuId,
                            StateId = s.StateId
                        }).ToList();

                    var premiumUserDatas = _adaniContext.PremiumUser.AsNoTracking()
                        .Where(_ => _.ParentId != 0 && DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom)
                        && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)
                        && _.UserId == inputDto.LoginUserId && _.SkuId == mandatorySkuMappingDto.MandatorySkuId)
                        .Select(s => new
                        {
                            Id = s.Id,
                            UserId = s.UserId,
                            ActualPremium = s.ActualPremium,
                            SkuId = s.SkuId
                        }).ToList();

                    var skuUomMappingDatas = _adaniContext.SkuUomMapping.AsNoTracking()
                        .Where(_ => _.SkuId == mandatorySkuMappingDto.MandatorySkuId)
                        .Select(s => new
                        {
                            SkuId = s.SkuId,
                            UomId = s.UomId,
                            RelationUomId = s.RelationUomId,
                            ConversionFactor1 = s.ConversionFactor1,
                            ConversionFactor2 = s.ConversionFactor2,
                        });

                    var uomList = _adaniContext.Uom.AsNoTracking();
                    #endregion

                    var skuUomdata = skuUomMappingDatas.FirstOrDefault(_ => _.SkuId == mandatorySkuMappingDto.MandatorySkuId);
                    if (skuUomdata != null)
                    {
                        mandatorySkuMappingDto.UOMId = skuUomdata.UomId;
                        mandatorySkuMappingDto.UOM = uomList.FirstOrDefault(_ => _.Id == skuUomdata.UomId).SAPName;
                        mandatorySkuMappingDto.CaseToMetricTonValue = _resultService.ConvertCasetoMetricTon(1, mandatorySkuMappingDto.MandatorySkuId);
                    }

                    if (discountUserDatas != null && discountUserDatas.Any())
                    {
                        if (userrole == (int)DTO.Enums.Role.ZonalTrader || userrole == (int)DTO.Enums.Role.StateTrader)
                        {
                            var discountLoginUserContext = discountUserDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.SkuId == mandatorySkuMappingDto.MandatorySkuId && _.StateId == userContext.StateId);
                            if (discountLoginUserContext != null && discountLoginUserContext.ActualDiscount > 0)
                            {
                                mandatorySkuMappingDto.EmployeeSkuDiscount = discountLoginUserContext.ActualDiscount;
                                mandatorySkuMappingDto.EmployeeSkuDiscountId = discountLoginUserContext.Id;
                            }
                            else
                            {
                                if (discountGeographyDatas != null && discountGeographyDatas.Any())
                                {
                                    var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == userContext.CityId || _.CityId == 0) && _.SkuId == mandatorySkuMappingDto.MandatorySkuId);
                                    if (discountGeographySkuContext != null)
                                    {
                                        mandatorySkuMappingDto.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                        mandatorySkuMappingDto.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                    }
                                }
                            }
                        }
                        else
                        {
                            var discountLoginUserContext = discountUserDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.SkuId == mandatorySkuMappingDto.MandatorySkuId);
                            if (discountLoginUserContext != null && discountLoginUserContext.ActualDiscount > 0)
                            {
                                mandatorySkuMappingDto.EmployeeSkuDiscount = discountLoginUserContext.ActualDiscount;
                                mandatorySkuMappingDto.EmployeeSkuDiscountId = discountLoginUserContext.Id;
                            }
                            else
                            {
                                if (discountGeographyDatas != null && discountGeographyDatas.Any())
                                {
                                    var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == userContext.CityId || _.CityId == 0) && _.SkuId == mandatorySkuMappingDto.MandatorySkuId);
                                    if (discountGeographySkuContext != null)
                                    {
                                        mandatorySkuMappingDto.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                        mandatorySkuMappingDto.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (discountGeographyDatas != null && discountGeographyDatas.Any())
                        {
                            var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == userContext.CityId || _.CityId == 0) && _.SkuId == mandatorySkuMappingDto.MandatorySkuId);
                            if (discountGeographySkuContext != null)
                            {
                                mandatorySkuMappingDto.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                mandatorySkuMappingDto.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                            }
                        }
                    }

                    if (premiumUserDatas != null && premiumUserDatas.Any())
                    {
                        var premiumLoginUserContext = premiumUserDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.SkuId == mandatorySkuMappingDto.MandatorySkuId);
                        if (premiumLoginUserContext != null)
                        {
                            mandatorySkuMappingDto.EmployeeSkuPremium = premiumLoginUserContext.ActualPremium;
                            mandatorySkuMappingDto.EmployeeSkuPremiumId = premiumLoginUserContext.ActualPremium;
                        }
                    }

                    if (mandatorySkuMappingDto.MandatoryBookingQuantityPercentage > 0)
                    {
                        var essentialSkuQuantity = inputDto.Skus.Where(_ => SkuIds.Contains(_.SkuId)).Select(_ => _.Quantity).Sum();
                        mandatorySkuMappingDto.MandatorySkuQuantity = CalculateQuantityFromPercentage(essentialSkuQuantity, mandatorySkuMappingDto.MandatoryBookingQuantityPercentage);
                    }
                }
            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method Exception: {exception.StackTrace}";
                _logger.Error(message);
            }
        }
        private static decimal CalculateQuantityFromPercentage(decimal actualQuantity, decimal percentage)
        {
            var value = (actualQuantity * percentage) / 100;
            return Math.Ceiling(value);
        }
    }
}
