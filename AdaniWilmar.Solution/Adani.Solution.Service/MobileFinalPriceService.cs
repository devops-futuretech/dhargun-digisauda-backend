using Adani.Solution.Data;
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
using System.Text;
using System.Threading.Tasks;

namespace Adani.Solution.Service
{
    public interface IMobileFinalPriceService
    {
        ResultDto FinalPriceSkuNameListForMobile(FinalPriceSkuInputDto inputDto);
        ResultDto FinalPriceSkuNameListForSpecialRateMobile(FinalPriceSkuInputDto inputDto);
        ResultDto GetSkuFinalPriceWithBdoDiscountPremiumForMobile(FinalPriceInputDto inputDto);


    }
    public class MobileFinalPriceService : IMobileFinalPriceService
    {
        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Final Price Service");
        private const string ServiceName = "Final Price Service";
        private string _methodName;
        private readonly IResultService _resultService;
        private readonly INotificationService _notificationService;

        public MobileFinalPriceService(IAdaniContext emamiContext, IResultService resultService, INotificationService notificationService)
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
                _logger.Error("Error instantiating dependencies for Final Price Service", exception);
            }
        }

        #region Mobile

        /// <summary>
        /// Method to get individual sku final price with StateTrader discount and premium
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        /// 

        public ResultDto GetSkuFinalPriceWithBdoDiscountPremiumForMobile(FinalPriceInputDto inputDto)
        {
            _methodName = "GetSkuFinalPriceWithBdoDiscountPremiumForMobile";
            var resultDto = new ResultDto();
            var outputDto = new FinalPriceOutputDto();
            try
            {
                var isPlant = false;
                var skuId = inputDto.SkuId;
                var incoTermsId = inputDto.IncoTermsId;
                var plantId = 0L;
                var depotId = 0L;
                var verticalId = 0L;
                var oilTypeId = 0L;
                var oilPackingTypeId = 0L;
                var cityId = 0L;
                var stateId = 0L;
                var uomId = 0L;

                var litreConversion = (decimal)0;
                var quantity = (decimal)0;
                var discount = (decimal)0;
                var premium = (decimal)0;
                var finalPrice = (decimal)0;


                var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == skuId);
                if (skuContext == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                oilTypeId = Convert.ToInt64(skuContext.OilTypeId);
                oilPackingTypeId = Convert.ToInt64(skuContext.PackGroupId);
                uomId = Convert.ToInt64(skuContext.UomId);
                quantity = skuContext.Quantity;
                outputDto.SkuId = skuContext.Id;
                outputDto.SkuName = skuContext.SkuName;
                var oilTypeContext = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == oilTypeId);
                if (oilTypeContext == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }
                verticalId = oilTypeContext.DivisionId;
               // litreConversion = oilTypeContext.LitreConversion;

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
                cityId = userContext.CityId;
                stateId = userContext.StateId;

                var depotContext = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.PlantDepotId);
                if (depotContext == null)
                {
                    return _resultService.ErrorMessage(Constants.DeportNotExistEmpty);
                }
                isPlant = depotContext.IsPlant;



                var pricing = new Pricing();
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);


                if (plantId > 0)
                {
                    pricing = _emamiContext.Pricing.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId && _.PlantId == plantId);
                }
                else
                {
                    pricing = _emamiContext.Pricing.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId);
                }
                if (pricing == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                //if (incoTermsId == (int)DTO.Enums.IncoTerms.ExDepot)
                //    finalPrice = pricing.ExDepotPrice;
                //else if (incoTermsId == (int)DTO.Enums.IncoTerms.ForDepot)
                //    finalPrice = pricing.ForDepotPrice;
                //else if (incoTermsId == (int)DTO.Enums.IncoTerms.ExPlant)
                //    finalPrice = pricing.ExPlantPrice;
                //else if (incoTermsId == (int)DTO.Enums.IncoTerms.ForPlant)
                //    finalPrice = pricing.ForPlantPrice;
                //else if (incoTermsId == (int)DTO.Enums.IncoTerms.ExRake)
                //    finalPrice = pricing.ExRakePrice;
                //else if (incoTermsId == (int)DTO.Enums.IncoTerms.ForRake)
                //    finalPrice = pricing.ForRakePrice;

                var numberOfPcs = (decimal)0;
                var skuUomContext = _emamiContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                if (skuUomContext != null)
                {
                    numberOfPcs = skuUomContext.ConversionFactor1;
                }

                //Discount
                var discountUserContext = _emamiContext.DiscountUsers.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.UserId == inputDto.DealerId && _.SkuId == skuId);
                if (discountUserContext != null)
                {
                    discount = discountUserContext.ActualDiscount;
                    //discount = _resultService.GetSkuQuanityRate(uomId, quantity, discountUserContext.ActualDiscount, litreConversion);
                    //discount = numberOfPcs * discount;
                }

                var discountGeographySkuContext = _emamiContext.DiscountGeography.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.CityId == cityId && _.SkuId == skuId);
                if (discountGeographySkuContext != null)
                {
                    var geographyDiscount = discountGeographySkuContext.ActualDiscount;
                    //var geographyDiscount = _resultService.GetSkuQuanityRate(uomId, quantity, discountGeographySkuContext.ActualDiscount, litreConversion);
                    //geographyDiscount = numberOfPcs * geographyDiscount;
                    discount = discount + geographyDiscount;
                }

                ////Premium
                var premiumUserContext = _emamiContext.PremiumUser.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.UserId == inputDto.DealerId && _.SkuId == skuId);
                if (premiumUserContext != null)
                {
                    premium = premiumUserContext.ActualPremium;
                    //premium = _resultService.GetSkuQuanityRate(uomId, quantity, premiumUserContext.ActualPremium, litreConversion);
                    premium = numberOfPcs * premium;

                }

                var premiumGeographySkuContext = _emamiContext.PremiumGeography.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.CityId == cityId && _.SkuId == skuId);
                if (premiumGeographySkuContext != null)
                {
                    var geoGraphyPremium = premiumGeographySkuContext.ActualPremium;
                    //var geoGraphyPremium = _resultService.GetSkuQuanityRate(uomId, quantity, premiumGeographySkuContext.ActualPremium, litreConversion);
                    //geoGraphyPremium = numberOfPcs * geoGraphyPremium;
                    premium = premium + geoGraphyPremium;
                }

                finalPrice = (finalPrice - discount) + premium;
                outputDto.FinalPrice = numberOfPcs * finalPrice;
                var discountLoginUserContext = _emamiContext.DiscountUsers.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.SkuId == skuId);
                if (discountLoginUserContext != null)
                {
                    //var oneQtyDiscount = _resultService.GetSkuQuanityRate(uomId, quantity, discountLoginUserContext.ActualDiscount, litreConversion);
                    //outputDto.BdoDiscount = numberOfPcs * oneQtyDiscount;
                    outputDto.BdoDiscount = discountLoginUserContext.ActualDiscount;
                }

                var premiumLoginUserContext = _emamiContext.PremiumUser.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.SkuId == skuId);
                if (premiumLoginUserContext != null)
                {
                    //var oneQtyPremium = _resultService.GetSkuQuanityRate(uomId, quantity, premiumLoginUserContext.ActualPremium, litreConversion);
                    //outputDto.BdoPremium = numberOfPcs * oneQtyPremium;
                    outputDto.BdoPremium = premiumLoginUserContext.ActualPremium;
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


        /// Method to get  final price sku name list for mobile app
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        public ResultDto FinalPriceSkuNameListForMobile(FinalPriceSkuInputDto inputDto)
        {
            _methodName = "FinalPriceSkuNameListForMobile";
            var resultDto = new ResultDto();
            var outputDto = new List<FinalPriceSkuOutputDto>();
            List<string> LineIds = new List<string>();

            try
            {
                //var stateId = 0;
                var cityId = 0;
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                cityId = Convert.ToInt32(userContext.CityId);

                if (inputDto.PlantId == 0)
                    return _resultService.ErrorMessage(Constants.PlantMissing);

                var userrole = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(user => user.UserId == inputDto.LoginUserId).RoleId;
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var skuDatas = _emamiContext.Skus.AsNoTracking()
                    .Select(s => new
                    {
                        Id = s.Id,
                        Name = s.SkuName + "-" + s.SkuCode + "-" + s.PackGroup.Name,
                        Code = s.SkuCode,
                        OilType = s.OilTypeId,
                        Quantity = s.Quantity,
                        UomId = s.UomId,
                        s.PremiumAmount,
                        s.StorageLocation,
                        OilPackGroupTypeId = s.OilPackGroupTypeId
                    }).ToList();


                var tempoutput = _emamiContext.TodayPricing.AsNoTracking().Join(_emamiContext.Skus.AsNoTracking(), t => t.SkuId, s => s.Id, (t, s) => new { t, s }).Where(_ =>
                       _.t.PlantId == inputDto.PlantId && (inputDto.OilTypeId > 0 ? (_.t.OilTypeId == inputDto.OilTypeId) : _.t.OilTypeId > 0)
                       && _.t.SkuId != 0
                       && (DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.t.ValidFrom)
                       && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.t.ValidTo)) && _.s.IsActive)/*.Take(2000000)*/.AsQueryable();

                if (tempoutput == null)
                    return _resultService.ErrorMessage(Constants.SkuMissingInTodayPricing);

                List<DivisionDetailsDto> userdivMappings = _emamiContext.UserDivisionMappings.AsNoTracking().Where(udiv => udiv.UserId == inputDto.LoginUserId)
                               .Select(s => new DivisionDetailsDto()
                               {
                                   SalesOrganizationId = s.SalesOrganizationId,
                                   DistributionChannelId = s.DistributionChannelId,
                                   DivisionId = s.DivisionId
                               }).ToList();

                if (userrole == (int)DTO.Enums.Role.Dealer)
                {
                    outputDto = tempoutput.Select(s => s.t).OrderByDescending(_ => _.Id).ToList().Select(_ => new FinalPriceSkuOutputDto
                    {
                        PricingId = _.Id,
                        SkuId = _.SkuId,
                        SkuName = skuDatas.FirstOrDefault(s => s.Id == _.SkuId) != null ? skuDatas.FirstOrDefault(s => s.Id == _.SkuId).Name : "",
                        PlantId = _.PlantId,
                        Price = _.Price,
                        DistributionChannelId = _.DistributionChannelId,
                        DivisionId = _.DivisionId,
                        SalesOrganizationId = _.SalesOrganizationId,
                        OilTypeId = skuDatas.FirstOrDefault(s => s.Id == _.SkuId) != null ? skuDatas.FirstOrDefault(s => s.Id == _.SkuId).OilType.GetValueOrDefault() : 0,
                        OilPackGroupTypeId = skuDatas.FirstOrDefault(s => s.Id == _.SkuId) != null ? skuDatas.FirstOrDefault(s => s.Id == _.SkuId).OilPackGroupTypeId : null
                    }).ToList();

                    outputDto = (from p in outputDto
                                 join udiv in userdivMappings on new
                                 {
                                     SalesOrganizationId = p.SalesOrganizationId,
                                     DistributionChannelId = p.DistributionChannelId,
                                     DivisionId = p.DivisionId
                                 } equals new
                                 {
                                     SalesOrganizationId = udiv.SalesOrganizationId,
                                     DistributionChannelId = udiv.DistributionChannelId,
                                     DivisionId = udiv.DivisionId
                                 }
                                 select new FinalPriceSkuOutputDto
                                 {
                                     PricingId = p.PricingId,
                                     SkuId = p.SkuId,
                                     SkuName = p.SkuName,
                                     PlantId = p.PlantId,
                                     Price = p.Price,
                                     DistributionChannelId = p.DistributionChannelId,
                                     DivisionId = p.DivisionId,
                                     SalesOrganizationId = p.SalesOrganizationId,
                                     OilTypeId = p.OilTypeId,
                                     OilPackGroupTypeId = p.OilPackGroupTypeId
                                 }).ToList();

                    LineIds = userContext.LineId != null ? userContext.LineId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() : null;
                }
                else
                {
                    outputDto = tempoutput.Select(s => s.t).OrderByDescending(_ => _.Id).ToList()
                        .Select(_ => new FinalPriceSkuOutputDto
                        {
                            PricingId = _.Id,
                            SkuId = _.SkuId,
                            SkuName = skuDatas.FirstOrDefault(s => s.Id == _.SkuId) != null ? skuDatas.FirstOrDefault(s => s.Id == _.SkuId).Name : "",
                            PlantId = _.PlantId,
                            Price = _.Price,
                            DistributionChannelId = _.DistributionChannelId,
                            DivisionId = _.DivisionId,
                            SalesOrganizationId = _.SalesOrganizationId,
                            OilTypeId = skuDatas.FirstOrDefault(s => s.Id == _.SkuId) != null ? skuDatas.FirstOrDefault(s => s.Id == _.SkuId).OilType.GetValueOrDefault() : 0,
                            OilPackGroupTypeId = skuDatas.FirstOrDefault(s => s.Id == _.SkuId) != null ? skuDatas.FirstOrDefault(s => s.Id == _.SkuId).OilPackGroupTypeId : null,
                        }).ToList();
                    LineIds = userContext.LineId != null ? userContext.LineId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() : null;
                }


                var RecentPricings = from e in outputDto
                                     group e by new { e.SkuId, e.PlantId, e.SalesOrganizationId, e.DistributionChannelId, e.DivisionId } into dptgrp
                                     let topsal = dptgrp.Max(x => x.PricingId)
                                     select new FinalPriceSkuOutputDto
                                     {
                                         SkuId = dptgrp.Key.SkuId,
                                         PlantId = dptgrp.Key.PlantId,
                                         Price = dptgrp.First(y => y.PricingId == topsal).Price,
                                         PricingId = dptgrp.First(y => y.PricingId == topsal).PricingId,
                                         SkuName = dptgrp.First(y => y.PricingId == topsal).SkuName,
                                         OilTypeId = dptgrp.First(y => y.PricingId == topsal).OilTypeId,
                                         DistributionChannelId = dptgrp.Key.DistributionChannelId,
                                         DivisionId = dptgrp.Key.DivisionId,
                                         SalesOrganizationId = dptgrp.Key.SalesOrganizationId,
                                         OilPackGroupTypeId = dptgrp.First(y => y.PricingId == topsal).OilPackGroupTypeId
                                     };
                outputDto = RecentPricings.ToList();


                if (outputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                var finalOutputDto = new List<FinalPriceSkuOutputDto>();

                var SkuDistinct = from a in outputDto.ToList()
                                  group a by new { a.SkuId, a.PlantId } into grp
                                  let topsku = grp.Max(X => X.PricingId)
                                  select new FinalPriceSkuOutputDto
                                  {
                                      SkuId = grp.Key.SkuId,
                                      PlantId = grp.Key.PlantId,
                                  };


                foreach (var item in SkuDistinct.ToList())
                {
                    var RecentPricingContext = (from a in outputDto.ToList()
                                                where a.SkuId == item.SkuId && a.PlantId == item.PlantId
                                                select a).ToList();

                    if (RecentPricingContext != null && RecentPricingContext.Any())
                    {
                        if (RecentPricingContext.Count > 1)
                        {
                            finalOutputDto.AddRange(RecentPricingContext.Where(_ => _.SkuId == item.SkuId && _.PlantId == item.PlantId).ToList());
                        }
                        else
                        {
                            finalOutputDto.AddRange(RecentPricingContext.Where(_ => _.SkuId == item.SkuId && _.PlantId == item.PlantId).ToList());
                        }
                    }
                }

                if (finalOutputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                #region Get Common Data's

                var skuIds = finalOutputDto.Select(s => s.SkuId).Distinct().ToList();
                var discountGeographyDatas = (
                from dg in _emamiContext.DiscountGeography.AsNoTracking()
                join sku in _emamiContext.Skus.AsNoTracking()
                    on dg.SkuId equals sku.Id into skuGroup
                from sku in skuGroup.DefaultIfEmpty()
                where currentDate >= dg.ValidFrom
                    && currentDate <= dg.ValidTo
                    && ((dg.CityId == cityId || dg.CityId == 0)
                        && (dg.DistrictId == userContext.DistrictId || dg.DistrictId == 0)
                        && (dg.StateId == userContext.StateId || dg.StateId == 0)
                        && dg.ZoneId == userContext.ZoneId)
                    && skuIds.Contains(dg.SkuId) && dg.IsActive
                select new
                {
                    Id = dg.Id,
                    CityId = dg.CityId,
                    ActualDiscount = dg.ActualDiscount,
                    SkuId = dg.SkuId,
                    OilTypeId = dg.OilTypeId,
                    OilPackGroupTypeId = sku != null ? sku.OilPackGroupTypeId : null
                }).ToList();

                var premiumGeographyDatas = _emamiContext.PremiumGeography.AsNoTracking()
                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom)
                    && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)
                    && _.CityId == cityId && skuIds.Contains(_.SkuId))
                    .Select(s => new
                    {
                        Id = s.Id,
                        CityId = s.CityId,
                        ActualPremium = s.ActualPremium,
                        SkuId = s.SkuId
                    }).ToList();

                var discountUserDatas = _emamiContext.DiscountUsers.AsNoTracking()
                    .Where(_ => _.ParentId != 0 && currentDate >= _.ValidFrom
                    && currentDate <= _.ValidTo
                    && _.UserId == inputDto.LoginUserId && skuIds.Contains(_.SkuId))
                    .Select(s => new
                    {
                        Id = s.Id,
                        UserId = s.UserId,
                        ActualDiscount = s.ActualDiscount,
                        SkuId = s.SkuId,
                        StateId = s.StateId
                    }).ToList();

                var premiumUserDatas = _emamiContext.PremiumUser.AsNoTracking()
                    .Where(_ => _.ParentId != 0 && DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom)
                    && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)
                    && _.UserId == inputDto.LoginUserId && skuIds.Contains(_.SkuId))
                    .Select(s => new
                    {
                        Id = s.Id,
                        UserId = s.UserId,
                        ActualPremium = s.ActualPremium,
                        SkuId = s.SkuId
                    }).ToList();

                var skuUomMappingDatas = _emamiContext.SkuUomMapping.AsNoTracking()
                    .Where(_ => skuIds.Contains(_.SkuId))
                    .Select(s => new
                    {
                        SkuId = s.SkuId,
                        UomId = s.UomId,
                        RelationUomId = s.RelationUomId,
                        ConversionFactor1 = s.ConversionFactor1,
                        ConversionFactor2 = s.ConversionFactor2,
                    });

                var uomList = _emamiContext.Uom.AsNoTracking();
                #endregion

                foreach (var pricing in finalOutputDto)
                {
                    pricing.SkuName = skuDatas.FirstOrDefault(x => x.Id == pricing.SkuId) != null ? skuDatas.FirstOrDefault(x => x.Id == pricing.SkuId).Name : string.Empty;
                    var skuId = pricing.SkuId;
                    var oilTypeId = pricing.OilTypeId;
                    var uomId = 0L;

                    var discount = (decimal)0;
                    var premium = (decimal)0;


                    var skuContext = skuDatas.FirstOrDefault(_ => _.Id == skuId);
                    if (skuContext == null)
                    {
                        return _resultService.ErrorMessage(Constants.RecordNotFound);
                    }
                    var skuUomdata = skuUomMappingDatas.FirstOrDefault(_ => _.SkuId == skuId);
                    if (skuUomdata != null)
                    {
                        uomId = skuUomdata.UomId;
                        pricing.UOMId = uomId;
                        pricing.UOM = uomList.FirstOrDefault(_ => _.Id == uomId).SAPName;
                        pricing.CaseToMetricTonValue = _resultService.ConvertCasetoMetricTon(1, skuId);
                    }
                    //if (discountGeographyDatas != null && discountGeographyDatas.Any())
                    //{
                    //    var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.CityId == cityId && _.SkuId == skuId);
                    //    if (discountGeographySkuContext != null)
                    //    {
                    //        var geographyDiscount = discountGeographySkuContext.ActualDiscount;
                    //        //discount = discount + geographyDiscount;
                    //    }
                    //}

                    if (premiumGeographyDatas != null && premiumGeographyDatas.Any())
                    {
                        var premiumGeographySkuContext = premiumGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.CityId == cityId && _.SkuId == skuId);
                        if (premiumGeographySkuContext != null)
                        {
                            var geoGraphyPremium = premiumGeographySkuContext.ActualPremium;
                            premium = premium + geoGraphyPremium;
                        }
                    }

                    if (discountUserDatas != null && discountUserDatas.Any())
                    {
                        if (userrole == (int)DTO.Enums.Role.ZonalTrader || userrole == (int)DTO.Enums.Role.StateTrader)
                        {
                            var discountLoginUserContext = discountUserDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.SkuId == skuId && _.StateId == userContext.StateId);
                            if (discountLoginUserContext != null && discountLoginUserContext.ActualDiscount > 0)
                            {
                                pricing.EmployeeSkuDiscount = discountLoginUserContext.ActualDiscount;
                            }
                            else
                            {
                                if (discountGeographyDatas != null && discountGeographyDatas.Any())
                                {
                                    var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.SkuId == skuId);

                                    if (discountGeographySkuContext != null)
                                    {
                                        pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                        pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                    }
                                    else
                                    {
                                        discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.OilTypeId == oilTypeId && _.OilPackGroupTypeId == pricing.OilPackGroupTypeId);

                                        if (discountGeographySkuContext != null && pricing.OilPackGroupTypeId != null)
                                        {
                                            if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                            {
                                                pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                                pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                            }
                                            else if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                                            {
                                                pricing.EmployeeSkuDiscount = _resultService.CalculateAutomatedDiscount(
                                                    discountGeographySkuContext.ActualDiscount,
                                                    discountGeographySkuContext.SkuId,
                                                    pricing.SkuId);
                                                pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var discountLoginUserContext = discountUserDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.SkuId == skuId);
                            if (discountLoginUserContext != null && discountLoginUserContext.ActualDiscount > 0)
                            {
                                pricing.EmployeeSkuDiscount = discountLoginUserContext.ActualDiscount;
                            }
                            else
                            {
                                if (discountGeographyDatas != null && discountGeographyDatas.Any())
                                {
                                    var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.SkuId == skuId);

                                    if (discountGeographySkuContext != null)
                                    {
                                        pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                        pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                    }
                                    else
                                    {
                                        discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.OilTypeId == oilTypeId && _.OilPackGroupTypeId == pricing.OilPackGroupTypeId);

                                        if (discountGeographySkuContext != null && pricing.OilPackGroupTypeId != null)
                                        {
                                            if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                            {
                                                pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                                pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                            }
                                            else if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                                            {
                                                pricing.EmployeeSkuDiscount = _resultService.CalculateAutomatedDiscount(
                                                    discountGeographySkuContext.ActualDiscount,
                                                    discountGeographySkuContext.SkuId,
                                                    pricing.SkuId);
                                                pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                            }
                                        }
                                    }
                                }

                            }
                        }            
                    }
                    else
                    {
                        if (discountGeographyDatas != null && discountGeographyDatas.Any())
                        {
                            var discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.SkuId == skuId);

                            if (discountGeographySkuContext != null)
                            {
                                pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                            }
                            else
                            {
                                discountGeographySkuContext = discountGeographyDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => (_.CityId == cityId || _.CityId == 0) && _.OilTypeId == oilTypeId && _.OilPackGroupTypeId == pricing.OilPackGroupTypeId);

                                if (discountGeographySkuContext != null && pricing.OilPackGroupTypeId != null)
                                {
                                    if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.BP)
                                    {
                                        pricing.EmployeeSkuDiscount = discountGeographySkuContext.ActualDiscount;
                                        pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                    }
                                    else if (pricing.OilPackGroupTypeId == (int)DTO.Enums.BpCpType.CP)
                                    {
                                        pricing.EmployeeSkuDiscount = _resultService.CalculateAutomatedDiscount(
                                            discountGeographySkuContext.ActualDiscount,
                                            discountGeographySkuContext.SkuId,
                                            pricing.SkuId);
                                        pricing.EmployeeSkuDiscountId = discountGeographySkuContext.Id;
                                    }
                                }
                            }
                        }
                    }

                    if (premiumUserDatas != null && premiumUserDatas.Any())
                    {
                        var premiumLoginUserContext = premiumUserDatas.OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.SkuId == skuId);
                        if (premiumLoginUserContext != null)
                        {
                            pricing.EmployeeSkuPremium = premiumLoginUserContext.ActualPremium;
                        }
                    }
                }

                if (LineIds != null && LineIds.Any())
                {
                    List<long> mappingSkuIds = new List<long>();

                    foreach (var id in LineIds.Distinct())
                    {
                        if (_emamiContext.Skus.Where(_ => _.LineId != null && _.LineId != string.Empty).Count() > 0)
                        {
                            var skuContextList = _emamiContext.Skus.Where(_ => _.LineId != null && _.LineId != string.Empty).ToList();
                            var skuIdList = skuContextList.Where(_ => _.LineId.Split(',').ToList().Contains(id)).Select(_ => _.Id).ToList();
                            mappingSkuIds.AddRange(skuIdList);
                        }
                    }

                    if (mappingSkuIds != null && mappingSkuIds.Any())
                    {
                        finalOutputDto = finalOutputDto.Where(_ => mappingSkuIds.Distinct().Contains(_.SkuId)).ToList();
                    }
                    //else
                    //{
                    //    finalOutputDto = new List<FinalPriceSkuOutputDto>();
                    //}
                }
                //else
                //{
                //    finalOutputDto = new List<FinalPriceSkuOutputDto>();
                //}

                //if (userrole == (int)DTO.Enums.Role.Dealer)
                //{
                //    if (LineIds != null && LineIds.Any())
                //    {
                //        List<long> mappingSkuIds = new List<long>();

                //        foreach (var id in LineIds.Distinct())
                //        {
                //            if (_emamiContext.Skus.Where(_ => _.LineId != null && _.LineId != string.Empty).Count() > 0)
                //            {
                //                var skuContextList = _emamiContext.Skus.Where(_ => _.LineId != null && _.LineId != string.Empty).ToList();
                //                var skuIdList = skuContextList.Where(_ => _.LineId.Split(',').ToList().Contains(id)).Select(_ => _.Id).ToList();
                //                mappingSkuIds.AddRange(skuIdList);
                //            }
                //        }

                //        if (mappingSkuIds != null)
                //        {
                //            finalOutputDto = finalOutputDto.Where(_ => mappingSkuIds.Distinct().Contains(_.SkuId)).ToList();
                //        }
                //    }
                //    else
                //    {
                //        finalOutputDto= new List<FinalPriceSkuOutputDto>();
                //    }
                //}
                //else if (userrole == (int)DTO.Enums.Role.StateTrader)
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
                //                        var skuIdList = skuContextList.Where(_ => _.LineId.Split(',').ToList().Contains(id)).Select(_ => _.Id).ToList();
                //                        mappingSkuIds.AddRange(skuIdList);
                //                    }
                //                }
                //            }
                //        }

                //        if (mappingSkuIds != null)
                //        {
                //            finalOutputDto = finalOutputDto.Where(_ => mappingSkuIds.Distinct().Contains(_.SkuId)).ToList();
                //        }
                //        else
                //        {
                //            finalOutputDto = new List<FinalPriceSkuOutputDto>();
                //        }
                //    }
                //}

                return _resultService.SuccessObject(finalOutputDto);

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception.StackTrace}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }

        }

        public ResultDto FinalPriceSkuNameListForMobileOld(FinalPriceSkuInputDto inputDto)
        {
            _methodName = "FinalPriceSkuNameListForMobile";
            var resultDto = new ResultDto();
            var outputDto = new List<FinalPriceSkuOutputDto>();
            try
            {
                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }

                var stateId = Convert.ToInt32(userContext.StateId);
                var cityId = Convert.ToInt32(userContext.CityId);

                var plantIds = _emamiContext.UserDepotMapping.AsNoTracking()
                    .Where(_ => _.UserId == inputDto.DealerId && _.Depot.IsPlant).Select(_ => _.DepotId);
                if (plantIds == null || !plantIds.Any())
                {
                    return _resultService.ErrorMessage(Constants.PlantCodeEmpty);
                }

                var depotIds = _emamiContext.UserDepotMapping.AsNoTracking()
                    .Where(_ => _.UserId == inputDto.DealerId && !_.Depot.IsPlant).Select(_ => _.DepotId);
                if (depotIds == null || !depotIds.Any())
                {
                    return _resultService.ErrorMessage(Constants.DepotCodeEmpty);
                }

                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                var skuDatas = _emamiContext.Skus.AsNoTracking().Where(w => w.IsActive)
                    .Select(s => new
                    {
                        Id = s.Id,
                        Name = s.SkuName,
                        OilType = s.OilTypeId,
                        Quantity = s.Quantity,
                     //   LitreConversion = s.OilType.LitreConversion,
                        UomId = s.UomId
                    }).ToList();
                if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess)
                {
                    if (Config.MobileSkuFinalPrice)
                    {
                        outputDto = _emamiContext.Pricing.AsNoTracking()
                            .Where(_ => //_.IsPublish &&
                            plantIds.Contains(_.PlantId)
                             ).AsQueryable()
                               .Select(_ => new FinalPriceSkuOutputDto
                               {
                                   PricingId = _.Id,
                                   SkuId = _.SkuId,
                                   SkuName = skuDatas.FirstOrDefault(x => x.Id == _.SkuId).Name,
                                   PlantId = _.PlantId,
                               }).ToList();
                    }
                    else
                    {
                        outputDto = _emamiContext.Pricing.AsNoTracking().Where(_ =>
                     plantIds.Contains(_.PlantId)).AsQueryable()
                       .Select(_ => new FinalPriceSkuOutputDto
                       {
                           PricingId = _.Id,
                           SkuId = _.SkuId,
                           SkuName = _emamiContext.Skus.FirstOrDefault(x => x.Id == _.SkuId) != null ? _emamiContext.Skus.FirstOrDefault(x => x.Id == _.SkuId).SkuName : string.Empty,
                           PlantId = _.PlantId,

                       }).ToList();
                    }


                    var RecentPricings = from e in outputDto
                                         group e by new { e.SkuId, e.PlantId } into dptgrp
                                         let topsal = dptgrp.Max(x => x.PricingId)
                                         select new FinalPriceSkuOutputDto
                                         {
                                             SkuId = dptgrp.Key.SkuId,
                                             PlantId = dptgrp.Key.PlantId,
                                             PricingId = dptgrp.First(y => y.PricingId == topsal).PricingId,
                                             SkuName = dptgrp.First(y => y.PricingId == topsal).SkuName,
                                             OilTypeId = dptgrp.First(y => y.PricingId == topsal).OilTypeId,
                                         };


                    outputDto = RecentPricings.ToList();
                }
                else
                {
                    outputDto = _emamiContext.Pricing.AsNoTracking().Where(_ =>
                      plantIds.Contains(_.PlantId)).AsQueryable()
                         .Select(_ => new FinalPriceSkuOutputDto
                         {
                             PricingId = _.Id,
                             SkuId = _.SkuId,
                             SkuName = _emamiContext.Skus.FirstOrDefault(x => x.Id == _.SkuId) != null ? _emamiContext.Skus.FirstOrDefault(x => x.Id == _.SkuId).SkuName : string.Empty,
                             PlantId = _.PlantId,
                         }).ToList();

                    var RecentPricings = from e in outputDto
                                         group e by new { e.SkuId, e.PlantId } into dptgrp
                                         let topsal = dptgrp.Max(x => x.PricingId)
                                         select new FinalPriceSkuOutputDto
                                         {
                                             SkuId = dptgrp.Key.SkuId,
                                             PlantId = dptgrp.Key.PlantId,
                                             PricingId = dptgrp.First(y => y.PricingId == topsal).PricingId,
                                             SkuName = dptgrp.First(y => y.PricingId == topsal).SkuName,
                                             OilTypeId = dptgrp.First(y => y.PricingId == topsal).OilTypeId,
                                         };

                    outputDto = RecentPricings.ToList();

                }


                if (outputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                var finalOutputDto = new List<FinalPriceSkuOutputDto>();

                var SkuDistinct = from a in outputDto.ToList()
                                  group a by new { a.SkuId, a.PlantId } into grp
                                  let topsku = grp.Max(X => X.PricingId)
                                  select new FinalPriceSkuOutputDto
                                  {
                                      SkuId = grp.Key.SkuId,
                                      PlantId = grp.Key.PlantId,
                                  };


                foreach (var item in SkuDistinct.ToList())
                {
                    var RecentPricingContext = (from a in outputDto.ToList()
                                                where a.SkuId == item.SkuId && a.PlantId == item.PlantId
                                                select a).ToList();

                    if (RecentPricingContext != null && RecentPricingContext.Any())
                    {
                        if (RecentPricingContext.Count > 1)
                        {
                            finalOutputDto.AddRange(RecentPricingContext.Where(_ => _.SkuId == item.SkuId && _.PlantId == item.PlantId).ToList());
                        }
                        else
                        {
                            finalOutputDto.AddRange(RecentPricingContext.Where(_ => _.SkuId == item.SkuId && _.PlantId == item.PlantId).ToList());
                        }
                    }
                }

                if (finalOutputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                foreach (var pricing in finalOutputDto)
                {
                    var skuId = pricing.SkuId;
                    var litreConversion = (decimal)0;
                    var quantity = (decimal)0;
                    var uomId = 0L;

                    var discount = (decimal)0;
                    var premium = (decimal)0;

                    //pricing.CaseToMetricTonValue = _resultService.ConvertCasetoMetricTon(1, skuId);

                    var skuContext = skuDatas.FirstOrDefault(_ => _.Id == skuId);
                    if (skuContext == null)
                    {
                        return _resultService.ErrorMessage(Constants.RecordNotFound);
                    }
                    litreConversion = /*skuContext.OilType != null ? skuContext.LitreConversion :*/ 0;
                    uomId = Convert.ToInt64(skuContext.UomId);
                    quantity = skuContext.Quantity;

                    var numberOfPcs = (decimal)0;
                    var skuUomContext = _emamiContext.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                    if (skuUomContext != null)
                    {
                        numberOfPcs = skuUomContext.ConversionFactor1;
                    }

                    pricing.CaseToMetricTonValue = _resultService.ConvertCasetoMetricTon(1, skuId);
                   // pricing.MetricTonToCaseValue = _resultService.ConvertMetricTontoCase(1, skuContext.Quantity, numberOfPcs, uomId, litreConversion);
                    //Discount
                    //var discountUserContext = _emamiContext.DiscountUsers.AsNoTracking().Where(_ => _.ParentId != 0 && DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.UserId == inputDto.DealerId && _.SkuId == skuId);
                    //if (discountUserContext != null)
                    //{
                    //    discount = discountUserContext.ActualDiscount;
                    //    //discount = _resultService.GetSkuQuanityRate(uomId, quantity, discountUserContext.ActualDiscount, litreConversion);
                    //    //discount = numberOfPcs * discount;
                    //}

                    var discountGeographySkuContext = _emamiContext.DiscountGeography.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.CityId == cityId && _.SkuId == skuId);
                    if (discountGeographySkuContext != null)
                    {
                        var geographyDiscount = discountGeographySkuContext.ActualDiscount;
                        //var geographyDiscount = _resultService.GetSkuQuanityRate(uomId, quantity, discountGeographySkuContext.ActualDiscount, litreConversion);
                        //geographyDiscount = numberOfPcs * geographyDiscount;
                        discount = discount + geographyDiscount;
                    }

                    ////Premium
                    //var premiumUserContext = _emamiContext.PremiumUser.AsNoTracking().Where(_ => _.ParentId != 0 && DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).FirstOrDefault(_ => _.UserId == inputDto.DealerId && _.SkuId == skuId);
                    //if (premiumUserContext != null)
                    //{
                    //    premium = premiumUserContext.ActualPremium;
                    //    //premium = _resultService.GetSkuQuanityRate(uomId, quantity, premiumUserContext.ActualPremium, litreConversion);
                    //    //premium = numberOfPcs * premium;
                    //}

                    var premiumGeographySkuContext = _emamiContext.PremiumGeography.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.CityId == cityId && _.SkuId == skuId);
                    if (premiumGeographySkuContext != null)
                    {
                        var geoGraphyPremium = premiumGeographySkuContext.ActualPremium;
                        //var geoGraphyPremium = _resultService.GetSkuQuanityRate(uomId, quantity, premiumGeographySkuContext.ActualPremium, litreConversion);
                        //geoGraphyPremium = numberOfPcs * geoGraphyPremium;
                        premium = premium + geoGraphyPremium;
                    }



                    var discountLoginUserContext = _emamiContext.DiscountUsers.AsNoTracking().Where(_ => _.ParentId != 0 && DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom)
                    && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.SkuId == skuId);
                    if (discountLoginUserContext != null)
                    {
                        //var oneQtyDiscount = _resultService.GetSkuQuanityRate(uomId, quantity, discountLoginUserContext.ActualDiscount, litreConversion);
                        //pricing.EmployeeSkuDiscount = Math.Round(numberOfPcs * oneQtyDiscount);
                        pricing.EmployeeSkuDiscount = discountLoginUserContext.ActualDiscount;
                    }


                    var premiumLoginUserContext = _emamiContext.PremiumUser.AsNoTracking().Where(_ => _.ParentId != 0 && DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom)
                    && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.SkuId == skuId);
                    if (premiumLoginUserContext != null)
                    {
                        //var oneQtyPremium = _resultService.GetSkuQuanityRate(uomId, quantity, premiumLoginUserContext.ActualPremium, litreConversion);
                        //pricing.EmployeeSkuPremium = Math.Round(numberOfPcs * oneQtyPremium);
                        pricing.EmployeeSkuPremium = premiumLoginUserContext.ActualPremium;
                    }

                }

                return _resultService.SuccessObject(finalOutputDto);

            }
            catch (Exception exception)
            {
                var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                _logger.Error(message);
                return _resultService.ErrorMessage(Constants.Exception);
            }

        }

        /// Method to get  final price sku name list for special rate mobile app
        /// </summary>
        /// <param name="inputDto"></param>
        /// <returns></returns>        
        public ResultDto FinalPriceSkuNameListForSpecialRateMobile(FinalPriceSkuInputDto inputDto)
        {
            _methodName = "FinalPriceSkuNameListForSpecialRateMobile";
            var resultDto = new ResultDto();
            var outputDto = new List<FinalPriceSkuOutputDto>();
            try
            {

                var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
                if (userContext == null)
                {
                    return _resultService.ErrorMessage(Constants.UserNotFound);
                }
               // var stateId = Convert.ToInt32(userContext.StateId);
                var cityId = Convert.ToInt32(userContext.CityId);

                //var plantIds = _emamiContext.UserDepotMapping.AsNoTracking()
                //    .Where(_ => _.UserId == inputDto.DealerId && _.Depot.IsPlant).Select(_ => _.Depot.Id);
                if (inputDto.PlantId == 0)
                {
                    return _resultService.ErrorMessage(Constants.PlantCodeEmpty);
                }

                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                //if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess)
                //{
                    outputDto = _emamiContext.TodayPricing.AsNoTracking().Where(_ =>
                     _.PlantId == inputDto.PlantId).AsQueryable()
                      .Select(_ => new FinalPriceSkuOutputDto
                      {
                          PricingId = _.Id,
                          SkuId = _.SkuId,
                          //PremiumAmount = _emamiContext.Skus.FirstOrDefault(s => s.Id == _.SkuId).PremiumAmount,
                          //StorageLocation = _emamiContext.Skus.FirstOrDefault(s => s.Id == _.SkuId).StorageLocation,
                          SkuName = _emamiContext.Skus.FirstOrDefault(s => s.Id == _.SkuId).SkuName,
                          OilTypeId = _.OilTypeId,
                          Price = _.Price,
                          PlantId = _.PlantId,

                      }).ToList();

                    var RecentPricings = from e in outputDto
                                         group e by new { e.SkuId, e.PlantId } into dptgrp
                                         let topsal = dptgrp.Max(x => x.PricingId)
                                         select new FinalPriceSkuOutputDto
                                         {
                                             SkuId = dptgrp.Key.SkuId,
                                             PlantId = dptgrp.Key.PlantId,
                                             PricingId = dptgrp.First(y => y.PricingId == topsal).PricingId,
                                             SkuName = dptgrp.First(y => y.PricingId == topsal).SkuName,
                                             OilTypeId = dptgrp.First(y => y.PricingId == topsal).OilTypeId,
                                         };

                    outputDto = RecentPricings.ToList();
                //}
                //else
                //{
                //    var currentTime = new TimeSpan(currentDate.Hour, currentDate.Minute, currentDate.Second);
                //    var biddingWindowContext = _emamiContext.BiddingWindowTiming.AsNoTracking().OrderByDescending(_ => _.ToHours).FirstOrDefault(_ => ((_.ToHours > currentTime && _.FromHours < currentTime) || _.ToHours < currentTime)
                //          && DbFunctions.TruncateTime(_.BiddingDate) == DbFunctions.TruncateTime(currentDate));
                //    if (biddingWindowContext != null)
                //    {
                //        outputDto = _emamiContext.TodayPricing.AsNoTracking().Where(_ =>
                //          plantIds.Contains(_.PlantId)
                //            ).AsQueryable()
                //         .Select(_ => new FinalPriceSkuOutputDto
                //         {
                //             PricingId = _.Id,
                //             SkuId = _.SkuId,
                //             //PremiumAmount = _emamiContext.Skus.FirstOrDefault(s => s.Id == _.SkuId).PremiumAmount,
                //             //StorageLocation = _emamiContext.Skus.FirstOrDefault(s => s.Id == _.SkuId).StorageLocation,
                //             SkuName = _emamiContext.Skus.FirstOrDefault(s => s.Id == _.SkuId).SkuName,
                //             OilTypeId = _.OilTypeId,
                //             Price = _.Price,
                //             PlantId = _.PlantId,
                //         }).ToList();

                //        var RecentPricings = from e in outputDto
                //                             group e by new { e.SkuId, e.PlantId } into dptgrp
                //                             let topsal = dptgrp.Max(x => x.PricingId)
                //                             select new FinalPriceSkuOutputDto
                //                             {
                //                                 SkuId = dptgrp.Key.SkuId,
                //                                 PlantId = dptgrp.Key.PlantId,
                //                                 PricingId = dptgrp.First(y => y.PricingId == topsal).PricingId,
                //                                 SkuName = dptgrp.First(y => y.PricingId == topsal).SkuName,
                //                                 OilTypeId = dptgrp.First(y => y.PricingId == topsal).OilTypeId,
                //                             };

                //        outputDto = RecentPricings.ToList();
                //    }
                //}

                if (outputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                var finalOutputDto = new List<FinalPriceSkuOutputDto>();

                var SkuDistinct = from a in outputDto.ToList()
                                  group a by new { a.SkuId, a.PlantId } into grp
                                  let topsku = grp.Max(X => X.PricingId)
                                  select new FinalPriceSkuOutputDto
                                  {
                                      SkuId = grp.Key.SkuId,
                                      PlantId = grp.Key.PlantId,
                                  };


                foreach (var item in SkuDistinct.ToList())
                {
                    var RecentPricingContext = (from a in outputDto.ToList()
                                                where a.SkuId == item.SkuId && a.PlantId == item.PlantId
                                                select a).ToList();

                    if (RecentPricingContext != null && RecentPricingContext.Any())
                    {
                        if (RecentPricingContext.Count > 1)
                        {
                            finalOutputDto.AddRange(RecentPricingContext.Where(_ => _.SkuId == item.SkuId && _.PlantId == item.PlantId).ToList());
                        }
                        else
                        {
                            finalOutputDto.AddRange(RecentPricingContext.Where(_ => _.SkuId == item.SkuId && _.PlantId == item.PlantId).ToList());
                        }
                    }
                }

                if (finalOutputDto == null)
                {
                    return _resultService.ErrorMessage(Constants.RecordNotFound);
                }

                var skuIds = finalOutputDto.Select(s => s.SkuId).Distinct().ToList();
                var skuUomMappingDatas = _emamiContext.SkuUomMapping.AsNoTracking()
                    .Where(_ => skuIds.Contains(_.SkuId))
                    .Select(s => new
                    {
                        SkuId = s.SkuId,
                        UomId = s.UomId,
                        RelationUomId = s.RelationUomId,
                        ConversionFactor1 = s.ConversionFactor1,
                        ConversionFactor2 = s.ConversionFactor2,
                    });

                var uomList = _emamiContext.Uom.AsNoTracking();

                foreach (var pricing in finalOutputDto)
                {
                    var skuId = pricing.SkuId;
                    //var litreConversion = (decimal)0;
                    //var ConversionFactor2 = (decimal)0;
                    var uomId = 0L;

                    var discount = (decimal)0;
                    var premium = (decimal)0;

                    var skuContext = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == skuId);
                    if (skuContext == null)
                    {
                        return _resultService.ErrorMessage(Constants.RecordNotFound);
                    }
                  //  litreConversion = skuContext.OilType != null ? skuContext.OilType.LitreConversion : 0;
                    var skuUomdata = skuUomMappingDatas.FirstOrDefault(_ => _.SkuId == skuId);
                    uomId = skuUomdata.UomId;
                    pricing.UOMId = uomId;
                    pricing.UOM = uomList.FirstOrDefault(_ => _.Id == uomId).SAPName;
                    //ConversionFactor2 = skuUomdata.ConversionFactor2;

                    //var conversionFactor1 = skuUomdata.ConversionFactor1;
                    //if (skuUomMappingDatas != null && skuUomMappingDatas.Any())
                    //{
                    //    var skuUomContext = skuUomMappingDatas.FirstOrDefault(_ => _.SkuId == skuId && _.UomId == uomId);
                    //    if (skuUomContext != null)
                    //    {
                    //        conversionFactor1 = skuUomContext.ConversionFactor;
                    //    }
                    //}

                    pricing.CaseToMetricTonValue = _resultService.ConvertCasetoMetricTon(1, uomId);
                    //pricing.MetricTonToCaseValue = _resultService.ConvertMetricTontoCase(1, skuContext.Quantity, numberOfPcs, uomId, litreConversion);


                    //Discount
                    var discountUserContext = _emamiContext.DiscountUsers.AsNoTracking().Where(_ => _.ParentId != 0 && DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.UserId == inputDto.DealerId && _.SkuId == skuId);
                    if (discountUserContext != null)
                    {
                        discount = discountUserContext.ActualDiscount;
                    }

                    var discountGeographySkuContext = _emamiContext.DiscountGeography.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.CityId == cityId && _.SkuId == skuId);
                    if (discountGeographySkuContext != null)
                    {
                        var geographyDiscount = discountGeographySkuContext.ActualDiscount;
                        discount = discount + geographyDiscount;
                    }

                    ////Premium
                    var premiumUserContext = _emamiContext.PremiumUser.AsNoTracking().Where(_ => _.ParentId != 0 && DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.UserId == inputDto.DealerId && _.SkuId == skuId);
                    if (premiumUserContext != null)
                    {
                        premium = premiumUserContext.ActualPremium;
                    }

                    var premiumGeographySkuContext = _emamiContext.PremiumGeography.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.CityId == cityId && _.SkuId == skuId);
                    if (premiumGeographySkuContext != null)
                    {
                        var geoGraphyPremium = premiumGeographySkuContext.ActualPremium;
                        premium = premium + geoGraphyPremium;
                    }

                    var discountLoginUserContext = _emamiContext.DiscountUsers.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom)
                    && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.SkuId == skuId);
                    if (discountLoginUserContext != null)
                    {
                        pricing.EmployeeSkuDiscount = discountLoginUserContext.ActualDiscount;
                    }


                    var premiumLoginUserContext = _emamiContext.PremiumUser.AsNoTracking().Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom)
                    && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo)).OrderByDescending(_ => _.Id).FirstOrDefault(_ => _.UserId == inputDto.LoginUserId && _.SkuId == skuId);
                    if (premiumLoginUserContext != null)
                    {
                        pricing.EmployeeSkuPremium = premiumLoginUserContext.ActualPremium;
                    }
                }

                return _resultService.SuccessObject(finalOutputDto);

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
