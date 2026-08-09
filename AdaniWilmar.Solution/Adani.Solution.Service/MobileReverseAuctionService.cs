using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.Service.Common;
using Adani.Solution.Data.Entities;
using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using GMCore.Helper;
using System.Web.Hosting;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Adani.Solution.MVC.Common;
using System.Configuration;

namespace Adani.Solution.Service
{
    public interface IMobileReverseAuctionService
    {
        //ResultDto GetBiddingWindowListForMobile(BiddingWindowInputDto inputDto);
        //ResultDto GetDealerBiddingWindowDetails(LoginUserIdDto inputDto);
        //ResultDto GetBDOBiddingWindowDetails(LoginUserIdDto inputDto);
        //ResultDto GetDiscountsAndBenefits(IdInputDto dealerId);
        //ResultDto GetAvailableBidQuantity(AvailableBidQuantityInputDto inputDto);
        //ResultDto BiddingCartOilTypes(DealerAndBrokersInputDto inputDto);
        //ResultDto GetDealerAndBrokersByBiddingWindow(DealerAndBrokersInputDto inputDto);
        //ResultDto BiddingCartSkuDetails(BiddingCartSkuInputDto inputDto);
        ////ResultDto SaudaBiddingCreation(SaudaBiddingCreationInputDto inputDto);
        //ResultDto SaudaBiddingLists(LoginUserIdDto inputDto);
        //ResultDto SaudaBiddingDetails(IdInputDto inputDto);
        ////ResultDto EditSaudaBiddingQuantity(SaudaBiddingQuantityEditInputDto inputDto);
        //ResultDto SaudaCounterbitStatusUpdate(SaudaCounterBidOfferStatusUpdate inputDto);
        //ResultDto GetCounterBidNotificationDetails(LoginUserIdDto inputDto);
        //ResultDto SaudaConversionFormulaForMobile(IdInputDto inputDto);

        //#region Sauda Allocation

        //ResultDto GetSaudaListForSaudaAllocationByUserId(SaudaFilterDto inputDto);
        //ResultDto SaudaAllocationCreation(SaudaBiddingCreationInputDto inputDto);
        //ResultDto SaudaAllocationSkuDetails(BiddingCartSkuInputDto inputDto);
        //ResultDto GetSaudaAllocationListForDealer(SaudaFilterDto inputDto);
        //ResultDto GetSaudaAllocationListForBDO(SaudaFilterDto inputDto);
        //ResultDto GetSaudaAllocationDetails(SaudaAllocationInputDto inputDto);

        //#endregion

        //#region Sauda Details

        //ResultDto GetSaudaDetailsForDealer(SaudaDetailInputDto inputDto);
        //ResultDto GetSaudaDetailsForBDO(SaudaDetailInputDto inputDto);
        //ResultDto GetSkusWithDealerAsList(SkuwithDealerFilterInputDto inputDto);
        //ResultDto GetSaudaDetailsRANew(SaudaDetailInputDto inputDto);
        //#endregion

    }

    public class MobileReverseAuctionService : IMobileReverseAuctionService
    {

        private readonly IAdaniContext _emamiContext;
        private readonly ILogger _logger = Logging.GetLogger("Reverse Auction Service");
        private const string ServiceName = "Reverse Auction Service";
        private string _methodName;
        private readonly IResultService _resultService;
        private readonly INotificationService _notificationService;
        static string connectionString = ConfigHelper.SPConnectionString;

        public MobileReverseAuctionService(IAdaniContext salesContext, IResultService resultService, INotificationService notificationService)
        {
            try
            {
                _emamiContext = salesContext;
                _resultService = resultService;
                _notificationService = notificationService;
            }
            catch (Exception exception)
            {
                _logger.Error("Error instantiating dependencies for Reverse Auction Service", exception);
            }
        }

        //#region Bidding Window

        ///// <summary>
        ///// Method to Get Bidding Window List
        ///// </summary>
        ///// <param name="inputDto"></param>
        ///// <returns></returns>
        //public ResultDto GetBiddingWindowListForMobile(BiddingWindowInputDto inputDto)
        //{
        //    _methodName = "GetBiddingWindowList";
        //    var resultDto = new ResultDto();
        //    var outputDto = new List<BiddingWindowListDto>();
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }

        //        var biddingWindowList = _emamiContext.BiddingWindowTiming.AsNoTracking().Where(_ => DbFunctions.TruncateTime(_.BiddingDate) == DbFunctions.TruncateTime(inputDto.CurrentDate) && _.Isactive).AsQueryable();
        //        if (!biddingWindowList.Any())
        //        {
        //            return _resultService.ErrorMessage(Constants.RecordNotFound);
        //        }
        //        foreach (var bidWindow in biddingWindowList.ToList())
        //        {
        //            var biddingWindowDto = new BiddingWindowListDto
        //            {
        //                Id = bidWindow.Id,
        //                Date = bidWindow.BiddingDate,
        //                FromHours = bidWindow.FromHours,
        //                ToHours = bidWindow.ToHours
        //            };
        //            outputDto.Add(biddingWindowDto);
        //        }
        //        return _resultService.SuccessObject(outputDto);
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        //public ResultDto GetDiscountsAndBenefits(IdInputDto dealerId)
        //{
        //    _methodName = "GetDiscountsAndBenefits";
        //    var resultDto = new ResultDto();
        //    var outputDto = new RADiscountsAndBenefitsDto();
        //    try
        //    {
        //        if (dealerId.Id == 0)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        var UserContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == dealerId.Id);
        //        if (UserContext != null)
        //        {
        //            var RoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(f => f.UserId == dealerId.Id);
        //            if (RoleContext.RoleId == (int)DTO.Enums.Role.Dealer)
        //            {
        //                outputDto.DealerName = UserContext.Name;
        //                DateTime Today = DateHelper.UtcToIndia(DateTime.UtcNow);

        //                var skus = _emamiContext.Skus.AsNoTracking().Select(s => new { Id = s.Id, Name = s.SkuName }).ToList();

        //                var CustomerGroupContext = _emamiContext.CustomerGroupDetails.FirstOrDefault(_ => _.CustomerId == dealerId.Id);
        //                if (CustomerGroupContext != null)
        //                {

        //                    //#region SKU Discount
        //                    var skuIds = new List<long>();
        //                    //var SkuDiscountGeographyDatas = _emamiContext.SkuDiscountGeography.AsNoTracking()
        //                    //                        .Join(_emamiContext.SkuDiscountGeographyMappings.AsNoTracking(), s => s.Id, sd => sd.SkuDiscountGeographyId, (s, sd) => new { SkuDiscount = s, SkuDiscountGeography = sd })
        //                    //                        .Join(_emamiContext.Skus.AsNoTracking(), su => su.SkuDiscountGeography.SkuId, sk => sk.Id, (su, sk) => new { SkuName = sk.SkuName, su.SkuDiscount, su.SkuDiscountGeography })
        //                    //                        .Where(f => DbFunctions.TruncateTime(Today) <= DbFunctions.TruncateTime(f.SkuDiscount.ValidTo)
        //                    //                        && DbFunctions.TruncateTime(f.SkuDiscount.ValidFrom) <= DbFunctions.TruncateTime(Today)
        //                    //                        && f.SkuDiscountGeography.CustomerId == dealerId.Id
        //                    //                        && f.SkuDiscountGeography.CityId == UserContext.CityId
        //                    //                        && f.SkuDiscountGeography.IsActive)
        //                    //                        .Select(s => new SkuDiscountDto()
        //                    //                        {
        //                    //                            SkuId = s.SkuDiscountGeography.SkuId,
        //                    //                            SkuName = s.SkuName,
        //                    //                            Discount = s.SkuDiscount.Discount
        //                    //                        }).ToList();

        //                    //if (SkuDiscountGeographyDatas.IsAny())
        //                    //{
        //                    //    skuIds = SkuDiscountGeographyDatas.Select(s => s.SkuId).ToList();
        //                    //    outputDto.SkuDiscount.AddRange(SkuDiscountGeographyDatas);
        //                    //}

        //                    //var SkuDiscountUserDatas = _emamiContext.SkuDiscountUsers.AsNoTracking()
        //                    //        .Join(_emamiContext.SkuDiscountUserMappings.AsNoTracking(), s => s.Id, sd => sd.SkuDiscountUserId, (s, sd) => new { SkuDiscount = s, SkuUserDiscount = sd })
        //                    //        .Join(_emamiContext.Skus.AsNoTracking(), su => su.SkuUserDiscount.SkuId, sk => sk.Id, (su, sk) => new { SkuName = sk.SkuName, su.SkuDiscount, su.SkuUserDiscount })
        //                    //        .Where(f => DbFunctions.TruncateTime(Today) <= DbFunctions.TruncateTime(f.SkuDiscount.ValidTo)
        //                    //        && DbFunctions.TruncateTime(f.SkuDiscount.ValidFrom) <= DbFunctions.TruncateTime(Today)
        //                    //        && f.SkuUserDiscount.CustomerId == dealerId.Id
        //                    //        && f.SkuUserDiscount.IsActive
        //                    //        && !skuIds.Contains(f.SkuUserDiscount.SkuId))
        //                    //        .Select(s => new SkuDiscountDto()
        //                    //        {
        //                    //            SkuId = s.SkuUserDiscount.SkuId,
        //                    //            SkuName = s.SkuName,
        //                    //            Discount = s.SkuDiscount.Discount
        //                    //        }).ToList();

        //                    //if (SkuDiscountUserDatas.IsAny())
        //                    //{
        //                    //    outputDto.SkuDiscount.AddRange(SkuDiscountUserDatas);
        //                    //}

        //                    //#endregion

        //                    //#region Volume Discount

        //                    skuIds = new List<long>();
        //                    //var VolumeDiscountGeographyDatas = _emamiContext.VolumeDiscountGeography.AsNoTracking()
        //                    //                        .Join(_emamiContext.VolumeDiscountGeographyMappings.AsNoTracking(), s => s.Id, sd => sd.VolumeDiscountGeographyId, (s, sd) => new { VolumeDiscount = s, VolumeDiscountGeography = sd })
        //                    //                        .Join(_emamiContext.VolumeDiscountGeographySlab.AsNoTracking(), s => s.VolumeDiscount.Id, vs => vs.VolumeDiscountGeographyId, (s, vs) => new { s.VolumeDiscount, s.VolumeDiscountGeography, VolumeSlab = vs })
        //                    //                        .Join(_emamiContext.Skus.AsNoTracking(), su => su.VolumeDiscountGeography.SkuId, sk => sk.Id, (su, sk) => new { SkuName = sk.SkuName, su.VolumeDiscount, su.VolumeDiscountGeography, su.VolumeSlab })
        //                    //                        .Where(f => DbFunctions.TruncateTime(Today) <= DbFunctions.TruncateTime(f.VolumeDiscount.ValidTo)
        //                    //                        && DbFunctions.TruncateTime(f.VolumeDiscount.ValidFrom) <= DbFunctions.TruncateTime(Today)
        //                    //                        && f.VolumeDiscountGeography.CustomerId == dealerId.Id
        //                    //                        && f.VolumeDiscountGeography.CityId == UserContext.CityId
        //                    //                        && f.VolumeDiscountGeography.IsActive)
        //                    //                        .Select(s => new RAVolumeDiscountDto()
        //                    //                        {
        //                    //                            StartVolumeSlabInMT = s.VolumeSlab.SlabStart,
        //                    //                            EndVolumeSlabInMT = s.VolumeSlab.SlabEnd,
        //                    //                            Discount = s.VolumeSlab.Discount,
        //                    //                            SkuName = s.SkuName,
        //                    //                            SkuId = s.VolumeDiscountGeography.SkuId,
        //                    //                        }).ToList();

        //                    ////if (VolumeDiscountGeographyDatas.IsAny())
        //                    ////{
        //                    ////    skuIds = VolumeDiscountGeographyDatas.Select(s => s.SkuId).ToList();
        //                    ////    outputDto.VolumeDiscount.AddRange(VolumeDiscountGeographyDatas);
        //                    ////}

        //                    ////var VolumeDiscountUserDatas = _emamiContext.VolumeDiscountUsers.AsNoTracking()
        //                    ////        .Join(_emamiContext.VolumeDiscountUserMappings.AsNoTracking(), s => s.Id, sd => sd.VolumeDiscountUserId, (s, sd) => new { VolumeDiscount = s, VolumeUserDiscount = sd })
        //                    ////        .Join(_emamiContext.VolumeDiscountUserSlabs.AsNoTracking(), s => s.VolumeDiscount.Id, vs => vs.VolumeDiscountUserId, (s, vs) => new { s.VolumeDiscount, s.VolumeUserDiscount, VolumeSlab = vs })
        //                    ////        .Join(_emamiContext.Skus.AsNoTracking(), su => su.VolumeUserDiscount.SkuId, sk => sk.Id, (su, sk) => new { SkuName = sk.SkuName, su.VolumeDiscount, su.VolumeUserDiscount, su.VolumeSlab })
        //                    ////        .Where(f => DbFunctions.TruncateTime(Today) <= DbFunctions.TruncateTime(f.VolumeDiscount.ValidTo)
        //                    ////        && DbFunctions.TruncateTime(f.VolumeDiscount.ValidFrom) <= DbFunctions.TruncateTime(Today)
        //                    ////        && f.VolumeUserDiscount.CustomerId == dealerId.Id
        //                    ////        && f.VolumeUserDiscount.IsActive
        //                    ////        && !skuIds.Contains(f.VolumeUserDiscount.SkuId))
        //                    ////        .Select(s => new RAVolumeDiscountDto()
        //                    ////        {
        //                    ////            StartVolumeSlabInMT = s.VolumeSlab.SlabStart,
        //                    ////            EndVolumeSlabInMT = s.VolumeSlab.SlabEnd,
        //                    ////            Discount = s.VolumeSlab.Discount,
        //                    ////            SkuName = s.SkuName,
        //                    ////            SkuId = s.VolumeUserDiscount.SkuId,
        //                    ////        }).ToList();

        //                    ////if (VolumeDiscountUserDatas.IsAny())
        //                    ////{
        //                    ////    outputDto.VolumeDiscount.AddRange(VolumeDiscountUserDatas);
        //                    ////}

        //                    //#endregion

        //                    #region Scheme Discount
        //                    skuIds = new List<long>();

        //                    var SchemeDiscountGeographyDatas = _emamiContext.SchemeDiscountGeography.AsNoTracking()
        //                                            .Join(_emamiContext.SchemeDiscountGeographyMappings.AsNoTracking(), s => s.Id, sd => sd.SchemeDiscountGeographyId, (s, sd) => new { SchemeDiscount = s, SchemeDiscountGeography = sd })
        //                                            .Join(_emamiContext.Skus.AsNoTracking(), su => su.SchemeDiscountGeography.SkuId, sk => sk.Id, (su, sk) => new { SkuName = sk.SkuName, su.SchemeDiscount, SchemeDiscountGeography = su.SchemeDiscountGeography })
        //                                            .Where(f => DbFunctions.TruncateTime(Today) <= DbFunctions.TruncateTime(f.SchemeDiscount.ValidTo)
        //                                            && DbFunctions.TruncateTime(f.SchemeDiscount.ValidFrom) <= DbFunctions.TruncateTime(Today)
        //                                            && f.SchemeDiscountGeography.CustomerId == dealerId.Id
        //                                            && f.SchemeDiscountGeography.CityId == UserContext.CityId
        //                                            && f.SchemeDiscountGeography.IsActive)
        //                                            .Select(s => new SkuDiscountDto()
        //                                            {
        //                                                SkuId = s.SchemeDiscountGeography.SkuId,
        //                                                SkuName = s.SkuName,
        //                                                Discount = s.SchemeDiscount.Discount
        //                                            }).ToList();

        //                    if (SchemeDiscountGeographyDatas.IsAny())
        //                    {
        //                        skuIds = SchemeDiscountGeographyDatas.Select(s => s.SkuId).ToList();
        //                        outputDto.SchemeDiscount.AddRange(SchemeDiscountGeographyDatas);
        //                    }

        //                    //var SchemeDiscountUserDatas = _emamiContext.SchemeDiscountUsers.AsNoTracking()
        //                    //        .Join(_emamiContext.SchemeDiscountUserMappings.AsNoTracking(), s => s.Id, sd => sd.SchemeDiscountUserId, (s, sd) => new { SchemeDiscount = s, SchemeDiscountUser = sd })
        //                    //        .Join(_emamiContext.Skus.AsNoTracking(), su => su.SchemeDiscountUser.SkuId, sk => sk.Id, (su, sk) => new { SkuName = sk.SkuName, su.SchemeDiscount, su.SchemeDiscountUser })
        //                    //        .Where(f => DbFunctions.TruncateTime(Today) <= DbFunctions.TruncateTime(f.SchemeDiscount.ValidTo)
        //                    //        && DbFunctions.TruncateTime(f.SchemeDiscount.ValidFrom) <= DbFunctions.TruncateTime(Today)
        //                    //        && f.SchemeDiscountUser.CustomerId == dealerId.Id
        //                    //        && f.SchemeDiscountUser.IsActive
        //                    //        && !skuIds.Contains(f.SchemeDiscountUser.SkuId))
        //                    //        .Select(s => new SkuDiscountDto()
        //                    //        {
        //                    //            SkuId = s.SchemeDiscountUser.SkuId,
        //                    //            SkuName = s.SkuName,
        //                    //            Discount = s.SchemeDiscount.Discount
        //                    //        }).ToList();

        //                    //if (SchemeDiscountUserDatas.IsAny())
        //                    //{
        //                    //    outputDto.SchemeDiscount.AddRange(SchemeDiscountUserDatas);
        //                    //}

        //                    #endregion

        //                }
        //            }
        //            resultDto.IsSuccess = true;
        //            resultDto.SuccessDto.Response = outputDto;
        //        }
        //        else
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.Message = Constants.RecordNotFound;
        //        }
        //        return resultDto;
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        resultDto.IsSuccess = false;
        //        resultDto.ErrorDto.ErrorCode = Constants.Exception;
        //        resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
        //        _logger.Error(message);
        //        return resultDto;
        //    }
        //}

        //public void TotalChances(long dealerId, long biddingWindowId)
        //{

        //}

        //public ResultDto GetAvailableBidQuantityOld(AvailableBidQuantityInputDto inputDto)
        //{
        //    _methodName = "GetAvailableBidQuantity";
        //    var resultDto = new ResultDto();
        //    var outputDto = new AvailableBidQuantityDto();
        //    try
        //    {
        //        if (inputDto.Id == 0)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        var UserContext = _emamiContext.Users.FirstOrDefault(f => f.Id == inputDto.Id);
        //        if (UserContext != null)
        //        {
        //            outputDto.DealerName = UserContext.Name;
        //            DateTime Today = DateHelper.UtcToIndia(DateTime.UtcNow);
        //            var CustomerGroupContext = _emamiContext.CustomerGroupDetails.FirstOrDefault(_ => _.CustomerId == inputDto.Id);
        //            if (CustomerGroupContext != null)
        //            {
        //                var BiddingWindowContext = _emamiContext.BiddingWindowCustomerGroups.AsNoTracking()
        //                    .Join(_emamiContext.BiddingWindow.AsNoTracking(), bwcg => bwcg.BiddingWindowId, bw => bw.Id, (bwcg, bw) => new { bwcg, bw })
        //                    .Where(_ => _.bwcg.CustomerGroupId == CustomerGroupContext.CustomerGroupId
        //                  && DbFunctions.TruncateTime(Today) == DbFunctions.TruncateTime(_.bw.BiddingDate)).ToList();

        //                //var BiddingWindowContext = _emamiContext.BiddingWindow.AsNoTracking().Where(f => f.Id == inputDto.BiddingWindowId);

        //                if (BiddingWindowContext != null)
        //                {
        //                    foreach (var BiddingWindow in BiddingWindowContext)
        //                    {
        //                        var BiddingWindowVolumeCapacitiesContext = _emamiContext.BiddingWindowVolumeCapacity.Where(_ => _.BiddingWindowId == BiddingWindow.bw.Id).ToList();
        //                        outputDto.TotalChances = BiddingWindowVolumeCapacitiesContext.Count() * BiddingWindow.bw.NoOfAttemptsForBidding;
        //                        outputDto.ChancesLeft = _emamiContext.SaudaBiddingCart.AsNoTracking().Where(_ => _.BiddingWindowId == BiddingWindow.bw.Id && _.DealerId == inputDto.Id).Count();
        //                        foreach (var BiddingWindowVolumeCapacities in BiddingWindowVolumeCapacitiesContext)
        //                        {
        //                            decimal BidQuantity = 0;
        //                            var BidQuantityContext = _emamiContext.SaudaBiddingCart.AsNoTracking().Where(_ => _.BiddingWindowId == BiddingWindow.bw.Id && _.DealerId == inputDto.Id && _.OilTypeId == BiddingWindowVolumeCapacities.OilTypeId).ToList();
        //                            if (BidQuantityContext != null && BidQuantityContext.Any())
        //                            {
        //                                BidQuantity = BidQuantityContext.Sum(s => s.BidQuantityInMT);
        //                            }
        //                            var oilType = new AvailableBidQuantityOilType
        //                            {
        //                                OilTypeId = BiddingWindowVolumeCapacities.OilTypeId,
        //                                OilTypeName = _emamiContext.OilTypes.FirstOrDefault(_ => _.Id == BiddingWindowVolumeCapacities.OilTypeId).Name,
        //                                VolumeCapacity = BiddingWindowVolumeCapacities.VolumeCapacity,
        //                                TotalChances = BiddingWindow.bw.NoOfAttemptsForBidding,
        //                                ChancesLeft = BiddingWindow.bw.NoOfAttemptsForBidding - BidQuantityContext.Count(),
        //                                AvailableQuantity = BiddingWindowVolumeCapacities.VolumeCapacity - BidQuantity
        //                            };
        //                            outputDto.AvailableBidQuantityOilType.Add(oilType);
        //                        }
        //                    }
        //                }
        //            }
        //            resultDto.IsSuccess = true;
        //            resultDto.SuccessDto.Response = outputDto;
        //        }
        //        else
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.Message = Constants.RecordNotFound;
        //        }
        //        return resultDto;
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        resultDto.IsSuccess = false;
        //        resultDto.ErrorDto.ErrorCode = Constants.Exception;
        //        resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
        //        return resultDto;
        //    }
        //}

        //public ResultDto GetAvailableBidQuantity(AvailableBidQuantityInputDto inputDto)
        //{
        //    _methodName = "GetAvailableBidQuantityNew";
        //    var resultDto = new ResultDto();
        //    var outputDto = new AvailableBidQuantityDto();
        //    int oilskuChanceLeftCount = 0;
        //    var totalBiddChances = 0;
        //    var totalBiddChancesLeft = 0;

        //    try
        //    {
        //        if (inputDto.Id == 0)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        var UserContext = _emamiContext.Users.FirstOrDefault(f => f.Id == inputDto.Id);
        //        if (UserContext != null)
        //        {
        //            outputDto.DealerName = UserContext.Name;
        //            DateTime Today = DateHelper.UtcToIndia(DateTime.UtcNow);
        //            var CustomerGroupContext = _emamiContext.CustomerGroupDetails.FirstOrDefault(_ => _.CustomerId == inputDto.Id);
        //            if (CustomerGroupContext != null)
        //            {
        //                //(int)DTO.Enums.BiddWindowStatus.Processing
        //                var BiddingWindowContext = _emamiContext.BiddingWindow.AsNoTracking()
        //                    .FirstOrDefault(f => f.Id == inputDto.BiddingWindowId);

        //                if (BiddingWindowContext != null)
        //                {
        //                    var baseSkuIds = _emamiContext.Skus.AsNoTracking().Where(w => w.IsBaseSku).Select(s => s.Id).ToList();
        //                    //var baseSkucodes = _emamiContext.Skus.AsNoTracking().Where(w => w.IsBaseSku).Select(s => s.SkuCode).ToList();

        //                    var publishedPricinga = _emamiContext.TodayPricing.AsNoTracking()  //_emamiContext.Pricing
        //                        .Where(w =>
        //                        //w.BiddingWindowId == inputDto.BiddingWindowId 
        //                        baseSkuIds.Contains(w.SkuId))
        //                        .Select(s => /*new { */
        //                             //OilTypeId = s.OilTypeId,
        //                             s.SkuId
        //                // }
        //                ).ToList();

        //                    var publishedPricing = _emamiContext.Skus.AsNoTracking().Where(w => publishedPricinga.Contains(w.Id)).Select(s => s.Id).ToList();


        //                    if (publishedPricing.IsAny())
        //                    {
        //                        //var skuChances = publishedPricing.GroupBy(g => new { g.OilTypeId });

        //                        //if (skuChances.IsAny())
        //                        //{
        //                        var saudaBiddingCartDatas = _emamiContext.SaudaBiddingCart.AsNoTracking()
        //                                .Where(_ => _.BiddingWindowId == inputDto.BiddingWindowId)
        //                                .Select(s => new
        //                                {
        //                                    DealerId = s.DealerId,
        //                                    OilTypeId = s.OilTypeId,
        //                                    BiddingWindowId = s.BiddingWindowId,
        //                                    BidQuantityInMT = s.BidQuantityInMT
        //                                }).ToList();

        //                        //if (saudaBiddingCartDatas.IsAny())
        //                        //{
        //                        var saudaBiddingCartData = saudaBiddingCartDatas.Where(_ => _.BiddingWindowId == inputDto.BiddingWindowId && _.DealerId == inputDto.Id);
        //                        //var oliIds = skuChances.Select(s => s.Key.OilTypeId).ToList();
        //                        //var oilTypeData = _emamiContext.OilTypes.AsNoTracking().Where(w => oliIds.Contains(w.Id))
        //                        //    .Select(s => new { Id = s.Id, OilName = s.Name }).ToList();
        //                        //comment
        //                        //foreach (var skuData in skuChances)
        //                        //{
        //                        //    decimal BidQuantity = 0;
        //                        //    var skuCount = skuData.Distinct().Count();
        //                        //    var windowVolumeCapacity = BiddingWindowContext.BiddingWindowVolumeCapacity.FirstOrDefault(f => f.OilTypeId == skuData.Key.OilTypeId);
        //                        //    var BidQuantityContext = saudaBiddingCartDatas.Where(_ => _.BiddingWindowId == inputDto.BiddingWindowId && _.OilTypeId == skuData.Key.OilTypeId).ToList(); //&& _.DealerId == inputDto.Id

        //                        //    if (BidQuantityContext != null && BidQuantityContext.Any())
        //                        //    {
        //                        //        BidQuantity = BidQuantityContext.Sum(s => s.BidQuantityInMT);
        //                        //    }

        //                        //    #region Chances Left
        //                        //    if (saudaBiddingCartData.IsAny())
        //                        //    {
        //                        //        oilskuChanceLeftCount = saudaBiddingCartData.Count(_ => _.OilTypeId == skuData.Key.OilTypeId);
        //                        //    }
        //                        //    #endregion

        //                        //    var oilskuTotalChance = BiddingWindowContext.NoOfAttemptsForBidding * skuCount;
        //                        //    totalBiddChances += oilskuTotalChance;
        //                        //    totalBiddChancesLeft += oilskuTotalChance - oilskuChanceLeftCount;

        //                        //    var oilType = new AvailableBidQuantityOilType
        //                        //    {
        //                        //        OilTypeId = skuData.Key.OilTypeId,
        //                        //        OilTypeName = oilTypeData.FirstOrDefault(_ => _.Id == skuData.Key.OilTypeId).OilName,
        //                        //        VolumeCapacity = windowVolumeCapacity != null ? windowVolumeCapacity.VolumeCapacity : 0,
        //                        //        TotalChances = oilskuTotalChance,
        //                        //        ChancesLeft = oilskuTotalChance - oilskuChanceLeftCount,
        //                        //        AvailableQuantity = windowVolumeCapacity != null ? (windowVolumeCapacity.VolumeCapacity - BidQuantity) : 0
        //                        //    };
        //                        //    outputDto.AvailableBidQuantityOilType.Add(oilType);
        //                        //}
        //                        //}
        //                        outputDto.TotalChances = totalBiddChances;
        //                        outputDto.ChancesLeft = totalBiddChancesLeft;
        //                        //}
        //                    }
        //                }
        //            }
        //            resultDto.IsSuccess = true;
        //            resultDto.SuccessDto.Response = outputDto;
        //        }
        //        else
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.Message = Constants.RecordNotFound;
        //        }
        //        return resultDto;
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        resultDto.IsSuccess = false;
        //        resultDto.ErrorDto.ErrorCode = Constants.Exception;
        //        resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
        //        return resultDto;
        //    }
        //}

        ////public ResultDto TotalChance(long dealerId, long biddingWindowId)
        ////{
        ////    var biddingWindow = _emamiContext.BiddingWindow.AsNoTracking()
        ////                    .FirstOrDefault(f => f.Id == biddingWindowId && f.StatusId == (int)DTO.Enums.BiddWindowStatus.Processing);
        ////    if (biddingWindow == null)
        ////    {
        ////        return _resultService.ErrorMessage(Constants.RecordNotFound);
        ////    }

        ////    var publishedPricing = _emamiContext.Pricing.AsNoTracking().Where(w => w.BiddingWindowId == biddingWindowId)
        ////        .Select(s => new { OilTypeId = s.OilTypeId, SkuId = s.SkuId }).ToList();

        ////    if (publishedPricing.IsAny())
        ////    {
        ////        var skuChances = publishedPricing.GroupBy(g => new { g.OilTypeId });

        ////        if (skuChances.IsAny())
        ////        {
        ////            foreach (var skuData in skuChances)
        ////            {
        ////                var skuCount = skuData.Count();
        ////                var totalChances = biddingWindow.NoOfAttemptsForBidding * skuCount;
        ////            }
        ////        }
        ////    }
        ////}

        //public void ChanceLeft(long dealerId, long biddingWindowId)
        //{

        //}

        //#endregion

        //#region StateTrader Bidding Window list

        //public ResultDto GetBDOBiddingWindowDetails(LoginUserIdDto inputDto)
        //{
        //    _methodName = "GetBDOBiddingWindowDetails";
        //    var todayDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //    IList<BdoBiddingWindowDetailsDto> biddingWindowList = new List<BdoBiddingWindowDetailsDto>();
        //    BdoBiddingWindowDetailsDto biddingDto = new BdoBiddingWindowDetailsDto();

        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }
        //        //StateTrader role validation
        //        var userRoleId = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(f => f.UserId == inputDto.LoginUserId)?.RoleId;
        //        if (userRoleId != null && userRoleId > 0)
        //        {
        //            if (userRoleId == (int)DTO.Enums.Role.StateTrader)
        //            {
        //                //Validate if any customers mapping for StateTrader
        //                var customerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(w => w.UserId == inputDto.LoginUserId)
        //                    .Select(s => s.CustomerId).Distinct().ToList();

        //                if (customerIds.IsAny())
        //                {
        //                    //Validate If StateTrader customers are available in any customer group
        //                    var isCustomerAvailable = _emamiContext.CustomerGroups.AsNoTracking()
        //                   .Join(_emamiContext.CustomerGroupDetails.AsNoTracking(), cg => cg.Id, cgd => cgd.CustomerGroupId, (cg, cgd) => new { CustomerGroup = cg, CustomerGroupDetail = cgd })
        //                   .Any(w => w.CustomerGroup.IsActive && customerIds.Contains(w.CustomerGroupDetail.CustomerId));

        //                    if (isCustomerAvailable)
        //                    {
        //                        //Get bidding window list for the customer group based
        //                        var biddingWindowLists = _emamiContext.CustomerGroups.AsNoTracking()
        //                        .Join(_emamiContext.CustomerGroupDetails.AsNoTracking(), cg => cg.Id, cgd => cgd.CustomerGroupId, (cg, cgd) => new { CustomerGroup = cg, CustomerGroupDetail = cgd })
        //                        .Join(_emamiContext.BiddingWindowCustomerGroups.AsNoTracking(), c => c.CustomerGroup.Id, bw => bw.CustomerGroupId, (c, bw) => new { c.CustomerGroup, c.CustomerGroupDetail, BiddWindowCustomerGroups = bw })
        //                        .Join(_emamiContext.BiddingWindow.AsNoTracking(), bwc => bwc.BiddWindowCustomerGroups.BiddingWindowId, bw => bw.Id, (bwc, bw) => new { bwc.CustomerGroup, BiddWindow = bw, bwc.CustomerGroupDetail, bwc.BiddWindowCustomerGroups, BiddingWindow = bw })
        //                        .Join(_emamiContext.City.AsNoTracking(), cg => cg.CustomerGroupDetail.Customer.CityId, c => c.Id, (cg, c) => new { City = c, cg.CustomerGroup, cg.BiddWindowCustomerGroups, cg.CustomerGroupDetail, cg.BiddingWindow })
        //                        .Where(w => customerIds.Contains(w.CustomerGroupDetail.CustomerId)
        //                        && w.CustomerGroup.IsActive
        //                        && DbFunctions.TruncateTime(w.BiddingWindow.CreatedDate) == DbFunctions.TruncateTime(todayDate)
        //                        && (w.BiddingWindow.StatusId == (int)DTO.Enums.BiddWindowStatus.Pending || w.BiddingWindow.StatusId == (int)DTO.Enums.BiddWindowStatus.Processing))
        //                        .Select(s => new
        //                        {
        //                            BiddingWindowId = s.BiddingWindow.Id,
        //                            BiddingWindowName = s.BiddingWindow.Name,
        //                            CustomerGroupId = s.CustomerGroup.Id,
        //                            CustomerGroupName = s.CustomerGroup.Name,
        //                            StartTime = s.BiddingWindow.StartTime,
        //                            EndTime = s.BiddingWindow.EndTime,
        //                            WindowStatusId = s.BiddingWindow.StatusId,
        //                            DealerId = s.CustomerGroupDetail.Customer.Id,
        //                            DealerName = s.CustomerGroupDetail.Customer.Name,
        //                            CityId = s.City.Id,
        //                            CityName = s.City.CityName
        //                        }).ToList();

        //                        if (biddingWindowLists.IsAny())
        //                        {
        //                            var biddingWindowDetails = biddingWindowLists.OrderBy(o => o.DealerName).GroupBy(g => new { g.BiddingWindowId, g.CustomerGroupId });

        //                            if (biddingWindowDetails.IsAny())
        //                            {
        //                                foreach (var windows in biddingWindowDetails)
        //                                {
        //                                    biddingDto = new BdoBiddingWindowDetailsDto();
        //                                    var window = windows.FirstOrDefault();
        //                                    bool isValidWindow = false;
        //                                    //      bool isValidWindow = _emamiContext.TodayPricing.AsNoTracking()   //_emamiContext.Pricing
        //                                    //.Any(w => w.BiddingWindowId == window.BiddingWindowId && w.IsPublish);
        //                                    if (isValidWindow)
        //                                    {
        //                                        biddingDto.BiddingWindowId = window.BiddingWindowId;
        //                                        biddingDto.BiddingWindowName = window.BiddingWindowName;
        //                                        biddingDto.CustomerGroupId = window.CustomerGroupId;
        //                                        biddingDto.CustomerGroupName = window.CustomerGroupName;
        //                                        biddingDto.StartEndTime = Utility.ConvertToTime(window.StartTime) + " - " + Utility.ConvertToTime(window.EndTime);
        //                                        biddingDto.StartTime = window.StartTime;
        //                                        biddingDto.EndTime = window.EndTime;
        //                                        biddingDto.ServerDateTime = todayDate;
        //                                        biddingDto.WindowStatusId = window.WindowStatusId;
        //                                        biddingDto.WindowStatus = Utility.GetEnumFromString<DTO.Enums.BiddWindowStatus>(window.WindowStatusId);
        //                                        biddingDto.UsersCount = windows.Count();
        //                                        if (windows.IsAny())
        //                                        {
        //                                            var customerDetails = windows.Select(s => new DealerDetailsDto()
        //                                            {
        //                                                DealerId = s.DealerId,
        //                                                DealerName = s.DealerName.TrimAndReduce(),
        //                                                CityId = s.CityId,
        //                                                City = s.CityName.TrimAndReduce()
        //                                            }).ToList();
        //                                            biddingDto.DealerDetails.AddRange(customerDetails);
        //                                        }
        //                                        biddingWindowList.Add(biddingDto);
        //                                    }
        //                                }
        //                            }
        //                            return _resultService.SuccessMessageWitObject(biddingWindowList, Constants.SuccessMessage);
        //                        }
        //                        else
        //                            return _resultService.ErrorMessage(Constants.NoBiddingWindows);
        //                    }
        //                    else
        //                        return _resultService.ErrorMessage(Constants.UserNotMappedToCustomerGroup);
        //                }
        //                else
        //                    return _resultService.ErrorMessage(Constants.UserNotMappingToBdo);
        //            }
        //            else
        //                return _resultService.ErrorMessage(Constants.BdoRoleOnlyAccepted);
        //        }
        //        else
        //            return _resultService.ErrorMessage(Constants.RecordNotFound);
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.Error($"{ServiceName} Service-Method {_methodName} Exception: {exception}");
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        //#endregion

        //#region Dealer Bidding Window list

        //public ResultDto GetDealerBiddingWindowDetails(LoginUserIdDto inputDto)
        //{
        //    _methodName = "GetDealerBiddingWindowDetails";
        //    var todayDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //    IList<DealerBiddingWindowDetailsDto> biddingWindowList = new List<DealerBiddingWindowDetailsDto>();
        //    DealerBiddingWindowDetailsDto biddingDto = new DealerBiddingWindowDetailsDto();
        //    bool isValidWindow = false;

        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }

        //        var userRoleId = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(f => f.UserId == inputDto.LoginUserId)?.RoleId;
        //        if (userRoleId != null && userRoleId > 0)
        //        {
        //            if (userRoleId == (int)DTO.Enums.Role.Dealer)
        //            {

        //                var isCustomerAvailable = _emamiContext.CustomerGroups.AsNoTracking()
        //               .Join(_emamiContext.CustomerGroupDetails.AsNoTracking(), cg => cg.Id, cgd => cgd.CustomerGroupId, (cg, cgd) => new { CustomerGroup = cg, CustomerGroupDetail = cgd })
        //               .Any(w => w.CustomerGroup.IsActive && w.CustomerGroupDetail.CustomerId == inputDto.LoginUserId);
        //                if (isCustomerAvailable)
        //                {
        //                    //Get bidding window list for the customer group based
        //                    var biddingWindowLists = _emamiContext.CustomerGroups.AsNoTracking()
        //                     .Join(_emamiContext.CustomerGroupDetails.AsNoTracking(), cg => cg.Id, cgd => cgd.CustomerGroupId, (cg, cgd) => new { CustomerGroup = cg, CustomerGroupDetail = cgd })
        //                     .Join(_emamiContext.BiddingWindowCustomerGroups.AsNoTracking(), c => c.CustomerGroup.Id, bw => bw.CustomerGroupId, (c, bw) => new { c.CustomerGroup, c.CustomerGroupDetail, BiddWindowCustomerGroups = bw })
        //                     .Join(_emamiContext.BiddingWindow.AsNoTracking(), bwc => bwc.BiddWindowCustomerGroups.BiddingWindowId, bw => bw.Id, (bwc, bw) => new { bwc.CustomerGroup, BiddWindow = bw, bwc.CustomerGroupDetail, bwc.BiddWindowCustomerGroups, BiddingWindow = bw })
        //                     .Join(_emamiContext.City.AsNoTracking(), cg => cg.CustomerGroupDetail.Customer.CityId, c => c.Id, (cg, c) => new { City = c, cg.CustomerGroup, cg.BiddWindowCustomerGroups, cg.CustomerGroupDetail, cg.BiddingWindow })
        //                     .Where(w => w.CustomerGroupDetail.CustomerId == inputDto.LoginUserId
        //                     && w.CustomerGroup.IsActive
        //                     && DbFunctions.TruncateTime(w.BiddingWindow.CreatedDate) == DbFunctions.TruncateTime(todayDate)
        //                     && (w.BiddingWindow.StatusId == (int)DTO.Enums.BiddWindowStatus.Pending || w.BiddingWindow.StatusId == (int)DTO.Enums.BiddWindowStatus.Processing))
        //                     .Select(s => new
        //                     {
        //                         BiddingWindowId = s.BiddingWindow.Id,
        //                         BiddingWindowName = s.BiddingWindow.Name,
        //                         CustomerGroupId = s.CustomerGroup.Id,
        //                         CustomerGroupName = s.CustomerGroup.Name,
        //                         StartTime = s.BiddingWindow.StartTime,
        //                         EndTime = s.BiddingWindow.EndTime,
        //                         WindowStatusId = s.BiddingWindow.StatusId,
        //                         DealerId = s.CustomerGroupDetail.Customer.Id,
        //                         DealerName = s.CustomerGroupDetail.Customer.Name,
        //                         CityId = s.City.Id,
        //                         CityName = s.City.CityName
        //                     }).ToList();

        //                    if (biddingWindowLists.IsAny())
        //                    {
        //                        foreach (var window in biddingWindowLists)
        //                        {
        //                            //isValidWindow = _emamiContext.TodayPricing.AsNoTracking() //_emamiContext.Pricing
        //                            // .Any(w => w.BiddingWindowId == window.BiddingWindowId && w.IsPublish);
        //                            if (isValidWindow)
        //                            {
        //                                biddingDto = new DealerBiddingWindowDetailsDto();
        //                                biddingDto.BiddingWindowId = window.BiddingWindowId;
        //                                biddingDto.BiddingWindowName = window.BiddingWindowName;
        //                                biddingDto.CustomerGroupId = window.CustomerGroupId;
        //                                biddingDto.CustomerGroupName = window.CustomerGroupName;
        //                                biddingDto.StartEndTime = Utility.ConvertToTime(window.StartTime) + " - " + Utility.ConvertToTime(window.EndTime);
        //                                biddingDto.StartTime = window.StartTime;
        //                                biddingDto.EndTime = window.EndTime;
        //                                biddingDto.ServerDateTime = todayDate;
        //                                biddingDto.WindowStatusId = window.WindowStatusId;
        //                                biddingDto.WindowStatus = Utility.GetEnumFromString<DTO.Enums.BiddWindowStatus>(window.WindowStatusId);
        //                                biddingWindowList.Add(biddingDto);
        //                            }
        //                        }
        //                        return _resultService.SuccessMessageWitObject(biddingWindowList, Constants.SuccessMessage);
        //                    }
        //                    else
        //                        return _resultService.ErrorMessage(Constants.NoBiddingWindows);
        //                }
        //                else
        //                    return _resultService.ErrorMessage(Constants.UserNotMappedToCustomerGroup);
        //            }
        //            else
        //            {
        //                return _resultService.ErrorMessage(Constants.DealerRoleOnlyAccepted);
        //            }
        //        }
        //        else
        //        {
        //            return _resultService.ErrorMessage(Constants.RecordNotFound);
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        _logger.Error($"{ServiceName} Service-Method {_methodName} Exception: {exception}");
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        //#endregion

        //#region BiddingCart, Discount, CounterBid, SaudaConversionFormula

        //public ResultDto GetDealerAndBrokersByBiddingWindow(DealerAndBrokersInputDto inputDto)
        //{
        //    _methodName = "GetDealerAndBrokersByBiddingWindow";
        //    var userMasterDto = new List<UserMasterDto>();
        //    if (inputDto == null)
        //    {
        //        return _resultService.ErrorMessage(Constants.InvalidRequest);
        //    }
        //    try
        //    {
        //        var LoginuserContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.LoginUserId);
        //        if (inputDto.DealerId > 0)
        //        {
        //            userMasterDto = (from bw in _emamiContext.BiddingWindow.AsNoTracking()
        //                             join bwcg in _emamiContext.BiddingWindowCustomerGroups.AsNoTracking() on bw.Id equals bwcg.BiddingWindowId
        //                             join cg in _emamiContext.CustomerGroups.AsNoTracking() on bwcg.CustomerGroupId equals cg.Id
        //                             join cgd in _emamiContext.CustomerGroupDetails.AsNoTracking() on cg.Id equals cgd.CustomerGroupId
        //                             join u in _emamiContext.Users.AsNoTracking() on cgd.CustomerId equals u.Id
        //                             join ucm in _emamiContext.UserCustomerMapping.AsNoTracking() on u.Id equals ucm.CustomerId
        //                             where //ucm.CustomerId == inputDto.LoginUserId
        //                             //&& 
        //                             //u.DivisionId == LoginuserContext.DivisionId
        //                              cgd.CustomerId == inputDto.DealerId
        //                             select new UserMasterDto
        //                             {
        //                                 Id = u.Id,
        //                                 EmployeeName = u.Name,
        //                                 EmployeeCode = u.Code,
        //                                 //FrieghtRoute = u.FreightRoute.Name,
        //                                 //FrieghtZone = u.FreightZone.Name,
        //                                 // Loadability = u.Loadability,
        //                                 //DepotLoadability = u.DepotLoadability,
        //                                 //VerticalId = u.DivisionId != null ? u.DivisionId.Value : 0
        //                             }).ToList();
        //        }
        //        else
        //        {
        //            userMasterDto = (from bw in _emamiContext.BiddingWindow.AsNoTracking()
        //                             join bwcg in _emamiContext.BiddingWindowCustomerGroups.AsNoTracking() on bw.Id equals bwcg.BiddingWindowId
        //                             join cg in _emamiContext.CustomerGroups.AsNoTracking() on bwcg.CustomerGroupId equals cg.Id
        //                             join cgd in _emamiContext.CustomerGroupDetails.AsNoTracking() on cg.Id equals cgd.CustomerGroupId
        //                             join u in _emamiContext.Users.AsNoTracking() on cgd.CustomerId equals u.Id
        //                             join ucm in _emamiContext.UserCustomerMapping.AsNoTracking() on u.Id equals ucm.CustomerId
        //                             where ucm.UserId == inputDto.LoginUserId
        //                             //&& u.DivisionId == LoginuserContext.DivisionId
        //                             select new UserMasterDto
        //                             {
        //                                 Id = u.Id,
        //                                 EmployeeName = u.Name,
        //                                 EmployeeCode = u.Code,
        //                                 //FrieghtRoute = u.FreightRoute.Name,
        //                                 //FrieghtZone = u.FreightZone.Name,
        //                                 // Loadability = u.Loadability,
        //                                 //DepotLoadability = u.DepotLoadability,
        //                                 //VerticalId = u.DivisionId != null ? u.DivisionId.Value : 0
        //                             }).ToList();
        //        }

        //        var oilskuChanceLeftCount = 0;
        //        var skuTotalChances = 0;
        //        var skuTotalChancesLeft = 0;

        //        var baseSkuIds = _emamiContext.Skus.AsNoTracking().Where(w => w.IsBaseSku).Select(s => s.Id).ToList();
        //        //var baseSkucodes = _emamiContext.Skus.AsNoTracking().Where(w => w.IsBaseSku).Select(s => s.SkuCode).ToList();

        //        var publishedPricinga = _emamiContext.TodayPricing.AsNoTracking()  //_emamiContext.Pricing
        //            .Where(w =>
        //            //w.BiddingWindowId == inputDto.BiddingWindowId 
        //            baseSkuIds.Contains(w.SkuId))
        //            .Select(s => /*new { */
        //                 //OilTypeId = s.OilTypeId,
        //                 s.SkuId
        //    // }
        //    ).ToList();

        //        var publishedPricing = _emamiContext.Skus.AsNoTracking().Where(w => publishedPricinga.Contains(w.Id)).Select(s => s.Id).ToList();


        //        var BiddingWindowContext = _emamiContext.BiddingWindow.AsNoTracking()
        //                   .FirstOrDefault(f => f.Id == inputDto.BiddingWindowId);

        //        if (publishedPricing.IsAny() && BiddingWindowContext != null)
        //        {
        //            var totalSkuCount = publishedPricing.Distinct().ToList().Count;
        //            skuTotalChances = BiddingWindowContext.NoOfAttemptsForBidding * totalSkuCount;

        //            var saudaBiddingCartData = _emamiContext.SaudaBiddingCart.AsNoTracking()
        //                                     .Where(_ => _.BiddingWindowId == inputDto.BiddingWindowId
        //                                     && _.DealerId == inputDto.DealerId
        //                                     && baseSkuIds.Contains(_.SkuId));
        //            #region Chances Left
        //            if (saudaBiddingCartData.IsAny())
        //            {
        //                oilskuChanceLeftCount = saudaBiddingCartData.Count();
        //            }
        //            #endregion

        //            skuTotalChancesLeft = skuTotalChances - oilskuChanceLeftCount;
        //        }

        //        var userIds = userMasterDto.Select(s => s.Id).Distinct().ToList();
        //        var overallStatus = Constants.OverallSaudaStatus;

        //        var overAllSaudaDatas = (from s in _emamiContext.Sauda.AsNoTracking()
        //                                 join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
        //                                 where userIds.Contains(s.UserId)
        //                                 && so.StatusId == (int)DTO.Enums.Status.Pending
        //                                 select new
        //                                 {
        //                                     BidQuantity = so.BidQuantity,
        //                                     SkuId = so.SkuId,
        //                                     UserId = s.UserId,
        //                                     StatusId = so.StatusId
        //                                 }).ToList();

        //        var invoiceDatas = (from inv in _emamiContext.Invoices.AsNoTracking()
        //                            join invDet in _emamiContext.InvoiceDetails.AsNoTracking() on inv.Id equals invDet.InvoiceId
        //                            where userIds.Contains(inv.UserId)
        //                            select new
        //                            {
        //                                UserId = inv.UserId,
        //                                SkuId = invDet.SkuId,
        //                                ActualBilledQuantity = invDet.ActualBilledQuantity
        //                            }).ToList();

        //        var UserDatas = _emamiContext.Users.AsNoTracking()
        //            .Where(w => userIds.Contains(w.Id))
        //            .Select(s => new
        //            {
        //                Id = s.Id
        //                //, SaudaLimit = s.SaudaLimit
        //            }).ToList();

        //        foreach (var user in userMasterDto)
        //        {
        //            decimal invoiceQuantity = 0;

        //            user.IsBroker = _emamiContext.UserRoles.AsNoTracking().Any(_ => _.UserId == user.Id && _.RoleId == (int)DTO.Enums.Role.Broker) ? true : false;

        //            //var overallStatus = Constants.OverallSaudaStatus;
        //            //var overAllSaudaContext = (from s in _emamiContext.Sauda.AsNoTracking()
        //            //                           join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
        //            //                           where s.UserId == user.Id
        //            //                           && overallStatus.Contains(so.StatusId)
        //            //                           select so
        //            //                               ).ToList();
        //            var overAllSaudaContext = overAllSaudaDatas
        //                .Where(w => w.UserId == user.Id
        //                && w.StatusId == (int)DTO.Enums.Status.Pending);

        //            var userdivContext = _emamiContext.UserDivisionMappings.AsNoTracking()
        //           .FirstOrDefault(_ => _.UserId == user.Id
        //           && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId
        //           && _.DivisionId == inputDto.DivisionId);

        //            //var SaudaLimitContext = UserDatas.FirstOrDefault(_ => _.Id == user.Id);
        //            if (userdivContext != null)
        //            {
        //                user.SaudaAndBiddingChances.TotalSaudaLimit = userdivContext.SaudaLimit ?? 0;
        //                user.SaudaAndBiddingChances.AvailableSaudaLimit = userdivContext.SaudaLimit ?? 0;
        //            }
        //            if (overAllSaudaContext != null)
        //            {
        //                var existingSaudaQuantity = overAllSaudaContext.Select(s => s.BidQuantity).DefaultIfEmpty(0).Sum();
        //                //var skuIds = overAllSaudaContext.Select(_ => _.SkuId).Distinct().ToList();
        //                //if (invoiceDatas.IsAny())
        //                //{
        //                //var invoiceContext = (from inv in _emamiContext.Invoices.AsNoTracking()
        //                //                      join invDet in _emamiContext.InvoiceDetails.AsNoTracking() on inv.Id equals invDet.InvoiceId
        //                //                      where inv.UserId == user.Id
        //                //                      && skuIds.Contains(invDet.SkuId)
        //                //                      select invDet
        //                //                      ).ToList();
        //                //var invoiceContext = invoiceDatas.Where(w => w.UserId == user.Id && skuIds.Contains(w.SkuId));
        //                //if (invoiceContext != null && invoiceContext.Any())
        //                //{
        //                //    invoiceQuantity = invoiceContext.Select(s => s.ActualBilledQuantity).DefaultIfEmpty(0).Sum();
        //                //}
        //                //}
        //                var saudaLimitTableValue = _emamiContext.SaudaLimit.AsNoTracking().FirstOrDefault(_ => _.UserId == user.Id);
        //                var saudaLimitTableValueTotal = saudaLimitTableValue != null ? (saudaLimitTableValue.PendingContract + saudaLimitTableValue.PendingDO + saudaLimitTableValue.PendingOBD) : 0;

        //                user.SaudaAndBiddingChances.AvailableSaudaLimit = (userdivContext.SaudaLimit ?? 0) - saudaLimitTableValueTotal - existingSaudaQuantity;
        //            }

        //            //var BiddingWindowContext = _emamiContext.BiddingWindow.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.BiddingWindowId);
        //            //if (BiddingWindowContext != null)
        //            //{
        //            //    long NoofOilTypes = BiddingWindowContext.BiddingWindowVolumeCapacity.Count();
        //            //    user.SaudaAndBiddingChances.TotalChances = NoofOilTypes * BiddingWindowContext.NoOfAttemptsForBidding;
        //            //    user.SaudaAndBiddingChances.ChancesLeft = NoofOilTypes * BiddingWindowContext.NoOfAttemptsForBidding;  //(NoofOilTypes * BiddingWindowContext.NoOfAttemptsForBidding) - _emamiContext.SaudaBiddingCart.AsNoTracking().Where(_ => _.BiddingWindowId == inputDto.BiddingWindowId && _.DealerId == inputDto.DealerId).Count();
        //            //}
        //            user.SaudaAndBiddingChances.TotalChances = skuTotalChances;
        //            user.SaudaAndBiddingChances.ChancesLeft = skuTotalChancesLeft;  //(NoofOilTypes * BiddingWindowContext.NoOfAttemptsForBidding) - _emamiContext.SaudaBiddingCart.AsNoTracking().Where(_ => _.BiddingWindowId == inputDto.BiddingWindowId && _.DealerId == inputDto.DealerId).Count();
        //        }

        //        return _resultService.SuccessObject(userMasterDto);
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        //public ResultDto BiddingCartOilTypes(DealerAndBrokersInputDto inputDto)
        //{
        //    _methodName = "BiddingCartOilTypes";
        //    var oilTypeOutputDto = new List<OilTypeDto>();
        //    if (inputDto == null)
        //    {
        //        return _resultService.ErrorMessage(Constants.InvalidRequest);
        //    }
        //    if (inputDto.BiddingWindowId == 0)
        //    {
        //        return _resultService.ErrorMessage(Constants.BiddingWindowisMissing);
        //    }
        //    try
        //    {

        //        var biddingWindowContext = _emamiContext.BiddingWindow.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.BiddingWindowId);

        //        if (biddingWindowContext != null)
        //        {
        //            foreach (var bwd in biddingWindowContext.BiddingWindowVolumeCapacity.ToList())
        //            {
        //                decimal BidQuantity = 0;
        //                var BidQuantityContext = _emamiContext.SaudaBiddingCart.AsNoTracking().Where(_ => _.BiddingWindowId == inputDto.BiddingWindowId && _.OilTypeId == bwd.OilTypeId).ToList();
        //                if (BidQuantityContext != null && BidQuantityContext.Any())
        //                {
        //                    BidQuantity = BidQuantityContext.Sum(s => s.BidQuantityInMT);
        //                }

        //                var biddingWindowDto = new OilTypeDto
        //                {
        //                    Id = bwd.OilTypeId,
        //                    Name = bwd.OilType.Name,
        //                    VolumeCapacity = bwd.VolumeCapacity - BidQuantity,
        //                    VerticalId = bwd.OilType.DivisionId,
        //                    VerticalName = bwd.OilType.Division.Name,
        //                    IsActive = bwd.OilType.IsActive
        //                };
        //                oilTypeOutputDto.Add(biddingWindowDto);
        //            }
        //        }
        //        return _resultService.SuccessObject(oilTypeOutputDto);
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        //public ResultDto BiddingCartSkuDetails(BiddingCartSkuInputDto inputDto)
        //{
        //    _methodName = "BiddingCartSkuDetails";
        //    var skuOutputDto = new List<BiddingCartSkuOutputDto>();
        //    if (inputDto == null)
        //    {
        //        return _resultService.ErrorMessage(Constants.InvalidRequest);
        //    }
        //    if (inputDto.OilTypeIds == null)
        //    {
        //        return _resultService.ErrorMessage(Constants.OilTypeMissing);
        //    }
        //    if (inputDto.IncotermId == 0)
        //    {
        //        return _resultService.ErrorMessage(Constants.IncotermsMissing);
        //    }
        //    if (inputDto.PlantId == 0)
        //    {
        //        return _resultService.ErrorMessage(Constants.PlantMissing);
        //    }
        //    if (inputDto.DealerId == 0)
        //    {
        //        return _resultService.ErrorMessage(Constants.DealerMissing);
        //    }
        //    try
        //    {
        //        DateTime currentdate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //        decimal skuDiscount = 0;
        //        decimal schemeDiscount = 0;

        //        var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
        //        if (dealerContext != null)
        //        {

        //            var plantTruckCapacities = _emamiContext.CustomerTruckCapacityMapping.Where(_ => _.UserId == inputDto.DealerId && _.StorageTypeId == (int)DTO.Enums.StorageType.Plant).Select(s => s.TruckCapacity).ToList();
        //            var DepotTruckCapacities = _emamiContext.CustomerTruckCapacityMapping.Where(_ => _.UserId == inputDto.DealerId && _.StorageTypeId == (int)DTO.Enums.StorageType.Depot).Select(s => s.TruckCapacity).ToList();
        //            List<long?> oilTypeId = new List<long?>();
        //            inputDto.OilTypeIds.ForEach(f => oilTypeId.Add(f));

        //            var BiddingWindowContext = _emamiContext.BiddingWindow.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.BiddingWindowId);
        //            if (BiddingWindowContext != null)
        //            {

        //                #region Ger Common Data's
        //                var oilTypeDatas = _emamiContext.OilTypes.AsNoTracking().Where(w => inputDto.OilTypeIds.Contains(w.Id))
        //                                    .Select(s => new
        //                                    {
        //                                        Id = s.Id,
        //                                        Name = s.Name
        //                                    }).ToList();
        //                if (oilTypeDatas.IsNotAny())
        //                {
        //                    return _resultService.ErrorMessage(Constants.OilTypeNotFound);
        //                }

        //                var skuDatas = _emamiContext.Skus.AsNoTracking()
        //                    .Where(w => oilTypeId.Contains(w.OilTypeId) && w.IsBaseSku)
        //                   .Select(s => new
        //                   {
        //                       Id = s.Id,
        //                       Name = s.SkuName,
        //                       Code = s.SkuCode
        //                   }).ToList();
        //                if (skuDatas.IsNotAny())
        //                {
        //                    return _resultService.ErrorMessage(Constants.BaseSkuEmpty);
        //                }

        //                var customerGroupId = _emamiContext.CustomerGroupDetails.AsNoTracking()
        //                    .FirstOrDefault(f => f.CustomerId == inputDto.DealerId && f.CustomerGroup.IsActive).CustomerGroupId;

        //                var skuIds = skuDatas.Select(s => s.Id).Distinct().ToList();
        //                //var skucodes = skuDatas.Select(s => s.Code).Distinct().ToList();
        //                // var plantCode = _emamiContext.Depots.AsNoTracking().FirstOrDefault(s => s.Id == inputDto.PlantId).Code;
        //                var pricingContext = _emamiContext.TodayPricing.AsNoTracking() //_emamiContext.Pricing
        //                .Where(_ =>
        //                 //_.BiddingWindowId == inputDto.BiddingWindowId
        //                 //&& inputDto.OilTypeIds.Contains(_.OilTypeId)
        //                 skuIds.Contains(_.SkuId)
        //                && _.PlantId == inputDto.PlantId
        //                //&& _.FrieghtRouteId == dealerContext.FreightRouteId
        //                )
        //                .Select(s => new
        //                {
        //                    Id = s.Id,
        //                    //OilTypeId = s.OilTypeId,
        //                    SkuId = s.SkuId,
        //                    PlantId = s.PlantId,
        //                    Price = s.Price,
        //                    OilTypeId = s.OilTypeId
        //                    //FrieghtRouteId = s.FrieghtRouteId,
        //                    //DepotId = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Code == s.DepotCode).Id,
        //                    //LoadQuantity = s.LoadQuantity,
        //                    //ExPlantGuaranteePrice = s.ExPlantGuaranteePrice,
        //                    //ForPlantGuaranteePrice = s.ForPlantGuaranteePrice,
        //                    //ExDepotGuaranteePrice = s.ExDepotGuaranteePrice,
        //                    //ForDepotGuaranteePrice = s.ForDepotGuaranteePrice,
        //                    //ExRakeGuaranteePrice = s.ExRakeGuaranteePrice,
        //                    //ForRakeGuaranteePrice = s.ForRakeGuaranteePrice,
        //                    //ExPlantPrice = s.ExPlantPrice,
        //                    //ForPlantPrice = s.ForPlantPrice,
        //                    //ExDepotPrice = s.ExDepotPrice,
        //                    //ForDepotPrice = s.ForDepotPrice,
        //                    //ExRakePrice = s.ExRakePrice,
        //                    //ForRakePrice = s.ForRakePrice
        //                });

        //                var benefitSkuIds = pricingContext.Select(s => s.SkuId).Distinct().ToList();

        //                #endregion

        //                if (pricingContext.IsAny())
        //                {
        //                    //if (inputDto.DepotId > 0)
        //                    //{
        //                    //    if (pricingContext.IsAny())
        //                    //    {
        //                    //        pricingContext = pricingContext.Where(_ => _.DepotId == inputDto.DepotId);
        //                    //    }
        //                    //}
        //                    //if (pricingContext.IsAny())
        //                    //{
        //                    //    if (inputDto.IncotermId == (int)DTO.Enums.IncoTerms.ExPlant || inputDto.IncotermId == (int)DTO.Enums.IncoTerms.ForPlant)
        //                    //    {
        //                    //        //pricingContext = pricingContext.Where(_ => plantTruckCapacities.Contains(_.LoadQuantity));
        //                    //    }
        //                    //    else if (inputDto.IncotermId == (int)DTO.Enums.IncoTerms.ExDepot || inputDto.IncotermId == (int)DTO.Enums.IncoTerms.ForDepot)
        //                    //    {
        //                    //        //pricingContext = pricingContext.Where(_ => DepotTruckCapacities.Contains(_.LoadQuantity));
        //                    //    }
        //                    //}
        //                    if (pricingContext != null)
        //                    {

        //                        #region Get Common Data's

        //                        var plantData = _emamiContext.Depots.AsNoTracking()
        //                            .FirstOrDefault(f => f.Id == inputDto.PlantId && f.StorageTypeId == (int)DTO.Enums.StorageType.Plant);

        //                        var incoTermData = _emamiContext.IncoTerms.AsNoTracking()
        //                            .FirstOrDefault(f => f.Id == inputDto.IncotermId);

        //                        //var depotIds = pricingContext.Select(s => s.DepotId).Distinct().ToList();
        //                        //var depotDatas = _emamiContext.Depots.AsNoTracking()
        //                        //    .Where(f => depotIds.Contains(f.Id) && f.StorageTypeId == (int)DTO.Enums.StorageType.Depot)
        //                        //    .Select(s => new { Id = s.Id, Name = s.Name }).ToList();

        //                        //var frieghtRouteIds = pricingContext.Select(s => s.FrieghtRouteId).Distinct().ToList();
        //                        //var FreightRoutesDatas = _emamiContext.FreightRoutes.AsNoTracking()
        //                        //    .Where(_ => frieghtRouteIds.Contains(_.Id))
        //                        //    .Select(s => new { Id = s.Id, Name = s.Name }).ToList();

        //                        //#region Get Common Data's

        //                        //var GpBenefitGeography = _emamiContext.GPBenefitGeography.AsNoTracking()
        //                        //    .Join(_emamiContext.GPBenefitGeographyMappings.AsNoTracking(), g => g.Id, gd => gd.GPBenefitGeographyId, (g, gd) => new { Geography = g, GeographyDetail = gd })
        //                        //    .Where(f => DbFunctions.TruncateTime(currentdate) <= DbFunctions.TruncateTime(f.Geography.ValidTo)
        //                        //            && DbFunctions.TruncateTime(f.Geography.ValidFrom) <= DbFunctions.TruncateTime(currentdate)
        //                        //            && f.GeographyDetail.CustomerId == dealerContext.Id
        //                        //            && f.GeographyDetail.CityId == dealerContext.CityId
        //                        //            && benefitSkuIds.Contains(f.GeographyDetail.SkuId)
        //                        //            && f.GeographyDetail.IsActive).ToList();

        //                        //var GpBenefitUser = _emamiContext.GPBenefitUsers.AsNoTracking()
        //                        //.Join(_emamiContext.GPBenefitUserMappings.AsNoTracking(), g => g.Id, gd => gd.GPBenefitUserId, (g, gd) => new { User = g, UserDetail = gd })
        //                        //.Where(f => DbFunctions.TruncateTime(currentdate) <= DbFunctions.TruncateTime(f.User.ValidTo)
        //                        //        && DbFunctions.TruncateTime(f.User.ValidFrom) <= DbFunctions.TruncateTime(currentdate)
        //                        //        && f.UserDetail.CustomerId == dealerContext.Id
        //                        //        && benefitSkuIds.Contains(f.UserDetail.SkuId)
        //                        //        && f.UserDetail.IsActive).ToList();

        //                        //#endregion

        //                        #endregion


        //                        #region SCHEME Discount
        //                        var SchemeDiscountGeographyDatas = _emamiContext.SchemeDiscountGeography.AsNoTracking()
        //                                            .Join(_emamiContext.SchemeDiscountGeographyMappings.AsNoTracking(), s => s.Id, sd => sd.SchemeDiscountGeographyId, (s, sd) => new { SchemeDiscount = s, SchemeDiscountGeography = sd })
        //                                            .Where(f => DbFunctions.TruncateTime(currentdate) <= DbFunctions.TruncateTime(f.SchemeDiscount.ValidTo)
        //                                            && DbFunctions.TruncateTime(f.SchemeDiscount.ValidFrom) <= DbFunctions.TruncateTime(currentdate)
        //                                            && skuIds.Contains(f.SchemeDiscountGeography.SkuId)
        //                                            && f.SchemeDiscountGeography.CustomerId == dealerContext.Id
        //                                            && f.SchemeDiscountGeography.CityId == dealerContext.CityId
        //                                            && f.SchemeDiscountGeography.IsActive)
        //                                            .Select(s => new
        //                                            {
        //                                                SkuId = s.SchemeDiscountGeography.SkuId,
        //                                                Discount = s.SchemeDiscount.Discount
        //                                            }).ToList();

        //                        #endregion

        //                        long gpBenefitType = 0;
        //                        long gpBenefitAppliedType = 0;
        //                        long gpBenefitCategoryTypeId = 0;
        //                        decimal gpBenefitDiscountOrDays = 0;
        //                        string gpBenefitCategoryType = "";
        //                        decimal gpBenefitDiscountCase = 0;
        //                        decimal gpDiscount = 0;


        //                        foreach (var pricing in pricingContext.ToList())
        //                        {
        //                            var guaranteePrice = 0;
        //                            var baseRate = 0;

        //                            if (skuOutputDto.IsAny())
        //                            {
        //                                bool isExistSku = skuOutputDto.Any(f => f.SkuId == pricing.SkuId
        //                                 && f.GuaranteePrice == guaranteePrice
        //                                 && f.PlantId == pricing.PlantId
        //                                 );
        //                                if (isExistSku)
        //                                {
        //                                    continue;
        //                                }
        //                            }

        //                            string benefitType = string.Empty;
        //                            string benefitSap = string.Empty;
        //                            string benefitNonSap = string.Empty;
        //                            var benefitDays = 0L;
        //                            var gpBenefitDiscount = 0.0m;

        //                            int skuDiscountType = 0;
        //                            int schemeDiscountType = 0;


        //                            var BiddingCartSku = new BiddingCartSkuOutputDto
        //                            {
        //                                PricingId = pricing.Id,
        //                                SkuId = pricing.SkuId,
        //                                OilTypeId = pricing.OilTypeId,
        //                                OilType = oilTypeDatas.IsAny() ? oilTypeDatas.FirstOrDefault(f => f.Id == pricing.OilTypeId).Name : string.Empty,  //pricing.OilType.Name,
        //                                IncotermId = inputDto.IncotermId,
        //                                PlantId = pricing.PlantId,
        //                                //DepotId = pricing.DepotId,
        //                                SkuName = skuDatas.IsAny() ? skuDatas.FirstOrDefault(f => f.Id == pricing.SkuId).Name : string.Empty,          //pricing.Sku.SkuName,
        //                                GuaranteePrice = guaranteePrice,
        //                                BaseRate = baseRate,
        //                                IncotermName = incoTermData.Name,        //_emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.IncotermId).Name,
        //                                PlantName = plantData.Name,              //_emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == pricing.PlantId && _.IsPlant).Name,
        //                                //DepotName = depotDatas.IsAny() ? depotDatas.FirstOrDefault(f => f.Id == pricing.DepotId).Name : string.Empty, //_emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == pricing.DepotId).Name,   
        //                                GPBenefitType = gpBenefitType,
        //                                GPBenefitAppliedTypeId = gpBenefitAppliedType,
        //                                GPBenefitOrCategoryId = gpBenefitCategoryTypeId,
        //                                GPBenefitOrCategory = gpBenefitCategoryType,
        //                                GPBenefitDiscountOrDay = gpBenefitDiscountOrDays
        //                            };

        //                            #region SKU Discount
        //                            skuDiscount = 0;
        //                            skuDiscountType = 0;

        //                            //if (SkuDiscountGeographyDatas.IsAny())
        //                            //{
        //                            //    var discountData = SkuDiscountGeographyDatas.FirstOrDefault(f => f.SkuId == pricing.SkuId);
        //                            //    if (discountData != null)
        //                            //    {
        //                            //        skuDiscount = discountData.Discount;
        //                            //        skuDiscountType = (int)DTO.Enums.RaDiscountType.Geography;
        //                            //    }
        //                            //}

        //                            //if (SkuDiscountUserDatas.IsAny() && skuDiscountType == 0)
        //                            //{
        //                            //    var discountData = SkuDiscountUserDatas.FirstOrDefault(f => f.SkuId == pricing.SkuId);
        //                            //    if (discountData != null)
        //                            //    {
        //                            //        skuDiscount = discountData.Discount;
        //                            //        skuDiscountType = (int)DTO.Enums.RaDiscountType.User;
        //                            //    }
        //                            //}
        //                            //BiddingCartSku.SkuDiscount = SkuDiscountUsers(pricing.SkuId, dealerContext.Id, currentdate);
        //                            BiddingCartSku.SkuDiscount = skuDiscount;
        //                            BiddingCartSku.SkuDiscountType = skuDiscountType;
        //                            #endregion

        //                            #region SCHEME Discount

        //                            schemeDiscount = 0;
        //                            schemeDiscountType = 0;
        //                            if (SchemeDiscountGeographyDatas.IsAny())
        //                            {
        //                                var discountData = SchemeDiscountGeographyDatas.FirstOrDefault(f => f.SkuId == pricing.SkuId);
        //                                if (discountData != null)
        //                                {
        //                                    schemeDiscount = discountData.Discount;
        //                                    schemeDiscountType = (int)DTO.Enums.RaDiscountType.Geography;
        //                                }
        //                            }

        //                            //if (SchemeDiscountUserDatas.IsAny() && schemeDiscountType == 0)
        //                            //{
        //                            //    var discountData = SchemeDiscountUserDatas.FirstOrDefault(f => f.SkuId == pricing.SkuId);
        //                            //    if (discountData != null)
        //                            //    {
        //                            //        schemeDiscount = discountData.Discount;
        //                            //        schemeDiscountType = (int)DTO.Enums.RaDiscountType.User;
        //                            //    }
        //                            //}
        //                            //BiddingCartSku.SchemeDiscount = SchemeDiscountUsers(pricing.SkuId, inputDto.DealerId, currentdate);
        //                            BiddingCartSku.SchemeDiscount = schemeDiscount;
        //                            BiddingCartSku.SchemeDiscountType = schemeDiscountType;
        //                            #endregion

        //                            BiddingCartSku.CaseToMTValue = _resultService.ConvertCasetoMetricTon(1, pricing.SkuId);
        //                            BiddingCartSku.TotalChances = SkuTotalChance(inputDto.BiddingWindowId);
        //                            skuOutputDto.Add(BiddingCartSku);
        //                        }
        //                        if (skuOutputDto != null)
        //                        {
        //                            skuOutputDto = skuOutputDto.Where(_ => _.GuaranteePrice != 0).ToList();
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        return _resultService.SuccessObject(skuOutputDto);
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        //public ResultDto BiddingCartSkuDetailsOld(BiddingCartSkuInputDto inputDto)
        //{
        //    _methodName = "BiddingCartSkuDetails";
        //    var skuOutputDto = new List<BiddingCartSkuOutputDto>();
        //    if (inputDto == null)
        //    {
        //        return _resultService.ErrorMessage(Constants.InvalidRequest);
        //    }
        //    if (inputDto.OilTypeIds == null)
        //    {
        //        return _resultService.ErrorMessage(Constants.OilTypeMissing);
        //    }
        //    if (inputDto.IncotermId == 0)
        //    {
        //        return _resultService.ErrorMessage(Constants.IncotermsMissing);
        //    }
        //    if (inputDto.PlantId == 0)
        //    {
        //        return _resultService.ErrorMessage(Constants.PlantMissing);
        //    }
        //    if (inputDto.DealerId == 0)
        //    {
        //        return _resultService.ErrorMessage(Constants.DealerMissing);
        //    }
        //    try
        //    {
        //        DateTime currentdate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //        decimal skuDiscount = 0;
        //        decimal schemeDiscount = 0;

        //        var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
        //        if (dealerContext != null)
        //        {
        //            List<long?> oilTypeId = new List<long?>();
        //            inputDto.OilTypeIds.ForEach(f => oilTypeId.Add(f));

        //            var BiddingWindowContext = _emamiContext.BiddingWindow.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.BiddingWindowId);
        //            if (BiddingWindowContext != null)
        //            {

        //                #region Ger Common Data's
        //                var oilTypeDatas = _emamiContext.OilTypes.AsNoTracking().Where(w => inputDto.OilTypeIds.Contains(w.Id))
        //                                    .Select(s => new
        //                                    {
        //                                        Id = s.Id,
        //                                        Name = s.Name
        //                                    }).ToList();
        //                if (oilTypeDatas.IsNotAny())
        //                {
        //                    return _resultService.ErrorMessage(Constants.OilTypeNotFound);
        //                }

        //                var skuDatas = _emamiContext.Skus.AsNoTracking()
        //                    .Where(w => oilTypeId.Contains(w.OilTypeId) && w.IsBaseSku)
        //                   .Select(s => new
        //                   {
        //                       Id = s.Id,
        //                       Name = s.SkuName,
        //                       Code = s.SkuCode
        //                   }).ToList();
        //                if (skuDatas.IsNotAny())
        //                {
        //                    return _resultService.ErrorMessage(Constants.BaseSkuEmpty);
        //                }

        //                var skuIds = skuDatas.Select(s => s.Id).Distinct().ToList();
        //                var pricingContext = _emamiContext.TodayPricing.AsNoTracking() //_emamiContext.Pricing
        //                 .Where(_ =>
        //                  //_.BiddingWindowId == inputDto.BiddingWindowId
        //                  //&& inputDto.OilTypeIds.Contains(_.OilTypeId)
        //                  skuIds.Contains(_.SkuId)
        //                 && _.PlantId == inputDto.PlantId
        //                 )
        //                 .Select(s => new
        //                 {
        //                     Id = s.Id,
        //                     OilTypeId = s.OilTypeId,
        //                     SkuId = s.SkuId,
        //                     PlantId = s.PlantId,
        //                     Price = s.Price
        //                 });

        //                var benefitSkuIds = pricingContext.Select(s => s.SkuId).Distinct().ToList();

        //                #endregion

        //                if (pricingContext.IsAny())
        //                {
        //                    if (pricingContext != null)
        //                    {

        //                        #region Get Common Data's

        //                        var plantData = _emamiContext.Depots.AsNoTracking()
        //                            .FirstOrDefault(f => f.Id == inputDto.PlantId && f.StorageTypeId == (int)DTO.Enums.StorageType.Plant);

        //                        var incoTermData = _emamiContext.IncoTerms.AsNoTracking()
        //                            .FirstOrDefault(f => f.Id == inputDto.IncotermId);

        //                        #endregion                                

        //                        #region SCHEME Discount
        //                        var SchemeDiscountGeographyDatas = _emamiContext.SchemeDiscountGeography.AsNoTracking()
        //                                            .Join(_emamiContext.SchemeDiscountGeographyMappings.AsNoTracking(), s => s.Id, sd => sd.SchemeDiscountGeographyId, (s, sd) => new { SchemeDiscount = s, SchemeDiscountGeography = sd })
        //                                            .Where(f => DbFunctions.TruncateTime(currentdate) <= DbFunctions.TruncateTime(f.SchemeDiscount.ValidTo)
        //                                            && DbFunctions.TruncateTime(f.SchemeDiscount.ValidFrom) <= DbFunctions.TruncateTime(currentdate)
        //                                            && skuIds.Contains(f.SchemeDiscountGeography.SkuId)
        //                                            && f.SchemeDiscountGeography.CustomerId == dealerContext.Id
        //                                            && f.SchemeDiscountGeography.CityId == dealerContext.CityId
        //                                            && f.SchemeDiscountGeography.IsActive)
        //                                            .Select(s => new
        //                                            {
        //                                                SkuId = s.SchemeDiscountGeography.SkuId,
        //                                                Discount = s.SchemeDiscount.Discount
        //                                            }).ToList();

        //                        //var SchemeDiscountUserDatas = _emamiContext.SchemeDiscountUsers.AsNoTracking()
        //                        //        .Join(_emamiContext.SchemeDiscountUserMappings.AsNoTracking(), s => s.Id, sd => sd.SchemeDiscountUserId, (s, sd) => new { SchemeDiscount = s, SchemeDiscountUser = sd })
        //                        //        .Where(f => DbFunctions.TruncateTime(currentdate) <= DbFunctions.TruncateTime(f.SchemeDiscount.ValidTo)
        //                        //        && DbFunctions.TruncateTime(f.SchemeDiscount.ValidFrom) <= DbFunctions.TruncateTime(currentdate)
        //                        //        && skuIds.Contains(f.SchemeDiscountUser.SkuId)
        //                        //        && f.SchemeDiscountUser.CustomerId == dealerContext.Id
        //                        //        && f.SchemeDiscountUser.IsActive)
        //                        //        .Select(s => new
        //                        //        {
        //                        //            SkuId = s.SchemeDiscountUser.SkuId,
        //                        //            Discount = s.SchemeDiscount.Discount
        //                        //        }).ToList();
        //                        #endregion          

        //                        foreach (var pricing in pricingContext.ToList())
        //                        {
        //                            var guaranteePrice = 0;
        //                            var baseRate = 0;

        //                            if (skuOutputDto.IsAny())
        //                            {
        //                                bool isExistSku = skuOutputDto.Any(f => f.SkuId == pricing.SkuId
        //                                 && f.GuaranteePrice == guaranteePrice
        //                                 && f.PlantId == pricing.PlantId
        //                                 );
        //                                if (isExistSku)
        //                                {
        //                                    continue;
        //                                }
        //                            }

        //                            string benefitType = string.Empty;
        //                            string benefitSap = string.Empty;
        //                            string benefitNonSap = string.Empty;
        //                            int schemeDiscountType = 0;

        //                            var BiddingCartSku = new BiddingCartSkuOutputDto
        //                            {
        //                                PricingId = pricing.Id,
        //                                SkuId = pricing.SkuId,
        //                                OilTypeId = pricing.OilTypeId,
        //                                OilType = oilTypeDatas.IsAny() ? oilTypeDatas.FirstOrDefault(f => f.Id == pricing.OilTypeId).Name : string.Empty,  //pricing.OilType.Name,
        //                                IncotermId = inputDto.IncotermId,
        //                                PlantId = pricing.PlantId,
        //                                SkuName = skuDatas.IsAny() ? skuDatas.FirstOrDefault(f => f.Id == pricing.SkuId).Name : string.Empty,          //pricing.Sku.SkuName,
        //                                GuaranteePrice = guaranteePrice,
        //                                BaseRate = baseRate,
        //                                IncotermName = incoTermData.Name,        //_emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.IncotermId).Name,
        //                                PlantName = plantData.Name,              //_emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == pricing.PlantId && _.IsPlant).Name,
        //                            };

        //                            #region SCHEME Discount
        //                            if (SchemeDiscountGeographyDatas.IsAny())
        //                            {
        //                                var discountData = SchemeDiscountGeographyDatas.FirstOrDefault(f => f.SkuId == pricing.SkuId);
        //                                if (discountData != null)
        //                                {
        //                                    schemeDiscount = discountData.Discount;
        //                                    schemeDiscountType = (int)DTO.Enums.RaDiscountType.Geography;
        //                                }
        //                            }

        //                            //if (SchemeDiscountUserDatas.IsAny() && schemeDiscount == 0)
        //                            //{
        //                            //    var discountData = SchemeDiscountUserDatas.FirstOrDefault(f => f.SkuId == pricing.SkuId);
        //                            //    if (discountData != null)
        //                            //    {
        //                            //        schemeDiscount = discountData.Discount;
        //                            //        schemeDiscountType = (int)DTO.Enums.RaDiscountType.User;
        //                            //    }
        //                            //}
        //                            //BiddingCartSku.SchemeDiscount = SchemeDiscountUsers(pricing.SkuId, inputDto.DealerId, currentdate);
        //                            BiddingCartSku.SchemeDiscount = schemeDiscount;
        //                            BiddingCartSku.SchemeDiscountType = schemeDiscountType;
        //                            #endregion

        //                            BiddingCartSku.CaseToMTValue = _resultService.ConvertCasetoMetricTon(1, pricing.SkuId);
        //                            BiddingCartSku.TotalChances = SkuTotalChance(inputDto.BiddingWindowId);
        //                            BiddingCartSku.ChancesLeft = SkuChancesLeftnew(inputDto.BiddingWindowId, 0, inputDto.DealerId, pricing.SkuId);

        //                            skuOutputDto.Add(BiddingCartSku);
        //                        }
        //                        if (skuOutputDto != null)
        //                        {
        //                            skuOutputDto = skuOutputDto.Where(_ => _.GuaranteePrice != 0).ToList();
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        return _resultService.SuccessObject(skuOutputDto);
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        //public long SkuTotalChance(long BiddingWindowId)
        //{
        //    var BiddingWindowsContext = _emamiContext.BiddingWindow.AsNoTracking().FirstOrDefault(f => f.Id == BiddingWindowId);
        //    if (BiddingWindowsContext != null)
        //    {
        //        return BiddingWindowsContext.NoOfAttemptsForBidding;
        //    }
        //    return 0;
        //}

        //public long SkuChancesLeft(long BiddingWindowId, long OilTypeId, long DealerId, long skuId)
        //{
        //    long ChancesLeft = 0;
        //    var BiddingWindowsContext = _emamiContext.BiddingWindow.AsNoTracking().FirstOrDefault(f => f.Id == BiddingWindowId);

        //    if (BiddingWindowsContext != null)
        //    {
        //        var biddedChances = _emamiContext.SaudaBiddingCart.AsNoTracking()
        //            .Count(_ => _.BiddingWindowId == BiddingWindowId
        //            && _.DealerId == DealerId
        //            && _.SkuId == skuId);

        //        ChancesLeft = BiddingWindowsContext.NoOfAttemptsForBidding - biddedChances;
        //    }
        //    return ChancesLeft;
        //}

        //public long SkuChancesLeftnew(long BiddingWindowId, long OilTypeId, long DealerId, long skuId)
        //{
        //    long ChancesLeft = 0;
        //    var BiddingWindowsContext = _emamiContext.BiddingWindow.AsNoTracking().FirstOrDefault(f => f.Id == BiddingWindowId);

        //    if (BiddingWindowsContext != null)
        //    {
        //        bool isEligibleForBidding = _emamiContext.SaudaBiddingCart.AsNoTracking()
        //            .Any(_ => _.BiddingWindowId == BiddingWindowId
        //            && _.DealerId == DealerId
        //            && _.SkuId == skuId && _.StatusId == (int)DTO.Enums.Status.Approved);
        //        if (!isEligibleForBidding)
        //        {
        //            var biddedChances = _emamiContext.SaudaBiddingCart.AsNoTracking()
        //            .Count(_ => _.BiddingWindowId == BiddingWindowId
        //            && _.DealerId == DealerId
        //            && _.SkuId == skuId);

        //            ChancesLeft = BiddingWindowsContext.NoOfAttemptsForBidding - biddedChances;
        //        }
        //    }
        //    return ChancesLeft;
        //}

        ////public decimal SkuDiscountUsers(long SkuId, long DealerId, DateTime todayDate)
        ////{
        ////    decimal Discount = 0;
        ////    var CustomerGroupContext = _emamiContext.CustomerGroupDetails.AsNoTracking().FirstOrDefault(_ => _.CustomerId == DealerId);
        ////    if (CustomerGroupContext != null)
        ////    {
        ////        //var SkuDiscountGeographyContext = _emamiContext.SkuDiscountGeography
        ////        //    .Where(_ => DbFunctions.TruncateTime(todayDate) <= DbFunctions.TruncateTime(_.ValidTo)
        ////        //  && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(todayDate)).ToList();

        ////        //var SkuDiscountUserContext = _emamiContext.SkuDiscountUsers.Where(_ => _.CustomerGroupId == CustomerGroupContext.CustomerGroupId
        ////        //  && DbFunctions.TruncateTime(todayDate) <= DbFunctions.TruncateTime(_.ValidTo)
        ////        //  && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(todayDate)).ToList();

        ////        //if (SkuDiscountGeographyContext != null && SkuDiscountGeographyContext.Any())
        ////        //{
        ////        //    foreach (var SkuDiscountGeography in SkuDiscountGeographyContext)
        ////        //    {
        ////        //        var SkuDiscountGeographyMappingsContext = _emamiContext.SkuDiscountGeographyMappings.FirstOrDefault(w => w.SkuDiscountGeographyId == SkuDiscountGeography.Id
        ////        //                                                      && w.SkuId == SkuId && w.DealerId == DealerId);
        ////        //        if (SkuDiscountGeographyMappingsContext != null)
        ////        //        {
        ////        //            Discount = SkuDiscountGeography.Discount;
        ////        //        }
        ////        //    }
        ////        //}

        ////        //if (SkuDiscountUserContext != null && SkuDiscountUserContext.Any() && Discount == 0)
        ////        //{
        ////        //    foreach (var SkuDiscountUser in SkuDiscountUserContext)
        ////        //    {
        ////        //        var SkuDiscountUserMappingsContext = _emamiContext.SkuDiscountUserMappings
        ////        //            .FirstOrDefault(w => w.SkuDiscountUserId == SkuDiscountUser.Id
        ////        //             && w.SkuId == SkuId
        ////        //             && w.UserId == DealerId);

        ////        //        if (SkuDiscountUserMappingsContext != null)
        ////        //        {
        ////        //            Discount = SkuDiscountUser.Discount;
        ////        //        }
        ////        //    }
        ////        //}

        ////        #region New Code
        ////        var skuGeographyDiscount = _emamiContext.SkuDiscountGeography.AsNoTracking()
        ////                            .Join(_emamiContext.SkuDiscountGeographyMappings.AsNoTracking(), s => s.Id, sd => sd.SkuDiscountGeographyId, (s, sd) => new { SkuDiscount = s, SkuDiscountGeography = sd })
        ////                            .FirstOrDefault(f => DbFunctions.TruncateTime(todayDate) <= DbFunctions.TruncateTime(f.SkuDiscount.ValidTo)
        ////                            && DbFunctions.TruncateTime(f.SkuDiscount.ValidFrom) <= DbFunctions.TruncateTime(todayDate)
        ////                            && f.SkuDiscountGeography.SkuId == SkuId
        ////                            && f.SkuDiscountGeography.CustomerId == DealerId
        ////                            && f.SkuDiscountGeography.CityId == CustomerGroupContext.Customer.CityId
        ////                            && f.SkuDiscountGeography.IsActive);

        ////        if (skuGeographyDiscount != null && skuGeographyDiscount.SkuDiscount != null && skuGeographyDiscount.SkuDiscountGeography != null)
        ////        {
        ////            Discount = skuGeographyDiscount.SkuDiscount.Discount;
        ////        }
        ////        else
        ////        {
        ////            var skuUserDiscount = _emamiContext.SkuDiscountUsers.AsNoTracking()
        ////                .Join(_emamiContext.SkuDiscountUserMappings.AsNoTracking(), s => s.Id, sd => sd.SkuDiscountUserId, (s, sd) => new { SkuDiscount = s, SkuUserDiscount = sd })
        ////                .FirstOrDefault(f => DbFunctions.TruncateTime(todayDate) <= DbFunctions.TruncateTime(f.SkuDiscount.ValidTo)
        ////                && DbFunctions.TruncateTime(f.SkuDiscount.ValidFrom) <= DbFunctions.TruncateTime(todayDate)
        ////                && f.SkuUserDiscount.SkuId == SkuId
        ////                && f.SkuUserDiscount.CustomerId == DealerId
        ////                && f.SkuUserDiscount.IsActive);
        ////            if (skuUserDiscount != null && skuUserDiscount.SkuDiscount != null && skuUserDiscount.SkuUserDiscount != null)
        ////            {
        ////                Discount = skuUserDiscount.SkuDiscount.Discount;
        ////            }
        ////        }

        ////        #endregion

        ////    }
        ////    return Discount;
        ////}

        ////public decimal SkuDiscountGeography(long SkuId, long DealerId, DateTime BiddingDate)
        ////{
        ////    var Discount = (decimal)0;
        ////    var SkuDiscountGeographyContext = _emamiContext.SkuDiscountGeography.AsNoTracking()
        ////        .Where(_ => DbFunctions.TruncateTime(BiddingDate) <= DbFunctions.TruncateTime(_.ValidTo)
        ////              && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(BiddingDate)).ToList();

        ////    if (SkuDiscountGeographyContext.IsAny())
        ////    {
        ////        foreach (var SkuDiscountGeography in SkuDiscountGeographyContext)
        ////        {
        ////            var SkuDiscountGeographyMappingsContext = _emamiContext.SkuDiscountGeographyMappings.AsNoTracking()
        ////                .FirstOrDefault(w => w.SkuDiscountGeographyId == SkuDiscountGeography.Id
        ////                 && w.SkuId == SkuId
        ////                 && w.CustomerId == DealerId);

        ////            if (SkuDiscountGeographyMappingsContext != null)
        ////            {
        ////                Discount = SkuDiscountGeography.Discount;
        ////            }
        ////        }
        ////    }
        ////    return Discount;
        ////}

        ////public BiddingCartVolumeDiscount VolumeDiscountUsers(long SkuId, long DealerId, DateTime currentDate, long cityId)
        ////{
        ////    var resultDto = new BiddingCartVolumeDiscount();
        ////    var skuIds = new List<long>();
        ////    var VolumeDiscountGeographyDatas = _emamiContext.VolumeDiscountGeography.AsNoTracking()
        ////                            .Join(_emamiContext.VolumeDiscountGeographyMappings.AsNoTracking(), s => s.Id, sd => sd.VolumeDiscountGeographyId, (s, sd) => new { VolumeDiscount = s, VolumeDiscountGeography = sd })
        ////                            .Join(_emamiContext.VolumeDiscountGeographySlab.AsNoTracking(), s => s.VolumeDiscount.Id, vs => vs.VolumeDiscountGeographyId, (s, vs) => new { s.VolumeDiscount, s.VolumeDiscountGeography, VolumeSlab = vs })
        ////                            .Join(_emamiContext.Skus.AsNoTracking(), su => su.VolumeDiscountGeography.SkuId, sk => sk.Id, (su, sk) => new { SkuName = sk.SkuName, su.VolumeDiscount, su.VolumeDiscountGeography, su.VolumeSlab })
        ////                            .Where(f => DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(f.VolumeDiscount.ValidTo)
        ////                            && DbFunctions.TruncateTime(f.VolumeDiscount.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
        ////                            && f.VolumeDiscountGeography.CustomerId == DealerId
        ////                            && f.VolumeDiscountGeography.CityId == cityId
        ////                            && f.VolumeDiscountGeography.IsActive
        ////                            && f.VolumeDiscountGeography.SkuId == SkuId)
        ////                            .Select(s => new RAVolumeDiscountDto()
        ////                            {
        ////                                StartVolumeSlabInMT = s.VolumeSlab.SlabStart,
        ////                                EndVolumeSlabInMT = s.VolumeSlab.SlabEnd,
        ////                                Discount = s.VolumeSlab.Discount,
        ////                                SkuName = s.SkuName,
        ////                                SkuId = s.VolumeDiscountGeography.SkuId,
        ////                            }).ToList();

        ////    //if (VolumeDiscountGeographyDatas.IsAny())
        ////    //{
        ////    //    resultDto.VolumeDiscount.AddRange(VolumeDiscountGeographyDatas);
        ////    //    resultDto.VolumeDiscountType = (int)DTO.Enums.RaDiscountType.Geography;
        ////    //}
        ////    //else
        ////    //{
        ////    //    var VolumeDiscountUserDatas = _emamiContext.VolumeDiscountUsers.AsNoTracking()
        ////    //        .Join(_emamiContext.VolumeDiscountUserMappings.AsNoTracking(), s => s.Id, sd => sd.VolumeDiscountUserId, (s, sd) => new { VolumeDiscount = s, VolumeUserDiscount = sd })
        ////    //        .Join(_emamiContext.VolumeDiscountUserSlabs.AsNoTracking(), s => s.VolumeDiscount.Id, vs => vs.VolumeDiscountUserId, (s, vs) => new { s.VolumeDiscount, s.VolumeUserDiscount, VolumeSlab = vs })
        ////    //        .Join(_emamiContext.Skus.AsNoTracking(), su => su.VolumeUserDiscount.SkuId, sk => sk.Id, (su, sk) => new { SkuName = sk.SkuName, su.VolumeDiscount, su.VolumeUserDiscount, su.VolumeSlab })
        ////    //        .Where(f => DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(f.VolumeDiscount.ValidTo)
        ////    //        && DbFunctions.TruncateTime(f.VolumeDiscount.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
        ////    //        && f.VolumeUserDiscount.CustomerId == DealerId
        ////    //        && f.VolumeUserDiscount.IsActive
        ////    //        && f.VolumeUserDiscount.SkuId == SkuId)
        ////    //        .Select(s => new RAVolumeDiscountDto()
        ////    //        {
        ////    //            StartVolumeSlabInMT = s.VolumeSlab.SlabStart,
        ////    //            EndVolumeSlabInMT = s.VolumeSlab.SlabEnd,
        ////    //            Discount = s.VolumeSlab.Discount,
        ////    //            SkuName = s.SkuName,
        ////    //            SkuId = s.VolumeUserDiscount.SkuId,
        ////    //        }).ToList();
        ////    //    if (VolumeDiscountUserDatas.IsAny())
        ////    //    {
        ////    //        resultDto.VolumeDiscount.AddRange(VolumeDiscountUserDatas);
        ////    //        resultDto.VolumeDiscountType = (int)DTO.Enums.RaDiscountType.User;
        ////    //    }
        ////    //}
        ////    return resultDto;
        ////}

        ////public BiddingCartVolumeDiscount VolumeDiscountUsersOld(long SkuId, long DealerId, DateTime BiddingDate)
        ////{
        ////    //var Discount = (decimal)0;
        ////    var resultDto = new BiddingCartVolumeDiscount();
        ////    //var UserContext = _emamiContext.Users.FirstOrDefault(f => f.Id == DealerId);
        ////    //if (UserContext != null)
        ////    //{
        ////    var CustomerGroupContext = _emamiContext.CustomerGroupDetails.AsNoTracking().FirstOrDefault(_ => _.CustomerId == DealerId);
        ////    if (CustomerGroupContext != null)
        ////    {
        ////        var VolumeDiscountGeographyContext = _emamiContext.VolumeDiscountGeography.AsNoTracking()
        ////            .Where(_ => DbFunctions.TruncateTime(BiddingDate) <= DbFunctions.TruncateTime(_.ValidTo)
        ////       && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(BiddingDate)).ToList();

        ////        var VolumeDiscountUserContext = _emamiContext.VolumeDiscountUsers.AsNoTracking()
        ////            .Where(_ => DbFunctions.TruncateTime(BiddingDate) <= DbFunctions.TruncateTime(_.ValidTo)
        ////            && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(BiddingDate)).ToList();

        ////        if (VolumeDiscountGeographyContext != null && VolumeDiscountGeographyContext.Any())
        ////        {
        ////            foreach (var VolumeDiscountGeography in VolumeDiscountGeographyContext)
        ////            {
        ////                var VolumeDiscountGeographyDetailsContext = _emamiContext.VolumeDiscountGeographyMappings
        ////                    .Where(w => w.VolumeDiscountGeographyId == VolumeDiscountGeography.Id
        ////                    && w.SkuId == SkuId && w.CustomerId == DealerId && w.IsActive).ToList();

        ////                if (VolumeDiscountGeographyDetailsContext != null && VolumeDiscountGeographyDetailsContext.Any())
        ////                {
        ////                    var VolumeDiscountSlabDetailsContext = _emamiContext.VolumeDiscountGeographySlab.FirstOrDefault(w => w.VolumeDiscountGeographyId == VolumeDiscountGeography.Id);
        ////                    if (VolumeDiscountSlabDetailsContext != null)
        ////                    {
        ////                        var VolumeDiscount = new RAVolumeDiscountDto
        ////                        {
        ////                            Discount = VolumeDiscountSlabDetailsContext.Discount,
        ////                            StartVolumeSlabInMT = VolumeDiscountSlabDetailsContext.SlabStart,
        ////                            EndVolumeSlabInMT = VolumeDiscountSlabDetailsContext.SlabEnd
        ////                        };
        ////                        resultDto.VolumeDiscount.Add(VolumeDiscount);
        ////                    }
        ////                }
        ////            }
        ////        }

        ////        if (VolumeDiscountUserContext != null && VolumeDiscountUserContext.Any() && resultDto.VolumeDiscount.IsNotAny())
        ////        {
        ////            foreach (var VolumeDiscountUser in VolumeDiscountUserContext)
        ////            {
        ////                var VolumeDiscountUserDetailsContext = _emamiContext.VolumeDiscountUserMappings.AsNoTracking()
        ////                    .FirstOrDefault(_ => _.VolumeDiscountUserId == VolumeDiscountUser.Id
        ////                    && _.CustomerGroupId == CustomerGroupContext.CustomerGroupId
        ////                    && _.IsActive
        ////                    && _.SkuId == SkuId
        ////                    && _.CustomerId == DealerId);

        ////                if (VolumeDiscountUserDetailsContext != null)
        ////                {
        ////                    if (VolumeDiscountUserDetailsContext.CustomerId == DealerId)
        ////                    {
        ////                        var VolumeDiscountSlabDetailsContext = _emamiContext.VolumeDiscountUserSlabs.AsNoTracking()
        ////                            .FirstOrDefault(w => w.VolumeDiscountUserId == VolumeDiscountUser.Id);
        ////                        if (VolumeDiscountSlabDetailsContext != null)
        ////                        {
        ////                            var VolumeDiscount = new RAVolumeDiscountDto
        ////                            {
        ////                                Discount = VolumeDiscountSlabDetailsContext.Discount,
        ////                                StartVolumeSlabInMT = VolumeDiscountSlabDetailsContext.SlabStart,
        ////                                EndVolumeSlabInMT = VolumeDiscountSlabDetailsContext.SlabEnd
        ////                            };
        ////                            resultDto.VolumeDiscount.Add(VolumeDiscount);
        ////                        }
        ////                    }
        ////                }
        ////            }
        ////        }
        ////    }
        ////    //}
        ////    return resultDto;
        ////}

        ////public decimal VolumeDiscountGeography(long SkuId, long DealerId, DateTime BiddingDate)
        ////{
        ////    var Discount = (decimal)0;
        ////    var VolumeDiscountUserContext = _emamiContext.VolumeDiscountGeography.AsNoTracking()
        ////        .Where(_ => DbFunctions.TruncateTime(BiddingDate) <= DbFunctions.TruncateTime(_.ValidTo)
        ////      && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(BiddingDate)).ToList();

        ////    if (VolumeDiscountUserContext.IsAny())
        ////    {
        ////        foreach (var VolumeDiscountUser in VolumeDiscountUserContext)
        ////        {
        ////            var VolumeDiscountUserDetailsContext = _emamiContext.VolumeDiscountGeographyMappings.AsNoTracking()
        ////                .FirstOrDefault(w => w.VolumeDiscountGeographyId == VolumeDiscountUser.Id
        ////                 && w.SkuId == SkuId
        ////                 && w.CustomerId == DealerId
        ////                 && w.IsActive);

        ////            if (VolumeDiscountUserDetailsContext != null)
        ////            {
        ////                var VolumeDiscountSlabDetailsContext = _emamiContext.VolumeDiscountUserSlabs.AsNoTracking()
        ////                    .FirstOrDefault(w => w.VolumeDiscountUserId == VolumeDiscountUser.Id);
        ////                if (VolumeDiscountSlabDetailsContext != null)
        ////                {
        ////                    Discount = VolumeDiscountSlabDetailsContext.Discount;
        ////                }
        ////            }
        ////        }
        ////    }
        ////    return Discount;
        ////}

        ////public decimal SchemeDiscountUsers(long SkuId, long DealerId, DateTime currentDate)
        ////{
        ////    var Discount = (decimal)0;
        ////    var UserContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == DealerId);
        ////    if (UserContext != null)
        ////    {
        ////        //var CustomerGroupContext = _emamiContext.CustomerGroupDetails.AsNoTracking().FirstOrDefault(_ => _.CustomerId == DealerId);
        ////        //if (CustomerGroupContext != null)
        ////        //{

        ////        //    var SchemeDiscountGeographyContext = _emamiContext.SchemeDiscountGeography.Where(_ => DbFunctions.TruncateTime(BiddingDate) <= DbFunctions.TruncateTime(_.ValidTo)
        ////        //             && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(BiddingDate)).ToList();

        ////        //    var SchemeDiscountUserContext = _emamiContext.SchemeDiscountUsers.Where(_ => _.CustomerGroupId == CustomerGroupContext.CustomerGroupId
        ////        //             && DbFunctions.TruncateTime(BiddingDate) <= DbFunctions.TruncateTime(_.ValidTo)
        ////        //             && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(BiddingDate)).ToList();

        ////        //    if (SchemeDiscountGeographyContext != null && SchemeDiscountGeographyContext.Any())
        ////        //    {
        ////        //        foreach (var SchemeDiscountUser in SchemeDiscountGeographyContext)
        ////        //        {
        ////        //            var SchemeDiscountUserMappingsContext = _emamiContext.SchemeDiscountGeographyDetails.FirstOrDefault(w => w.SchemeDiscountGeographyId == SchemeDiscountUser.Id
        ////        //                                                   && w.DealerId == UserContext.Id && w.SkuId == SkuId);
        ////        //            if (SchemeDiscountUserMappingsContext != null)
        ////        //            {
        ////        //                if (SchemeDiscountUserMappingsContext.CityId == UserContext.CityId)
        ////        //                {
        ////        //                    Discount = SchemeDiscountUser.Discount;
        ////        //                }
        ////        //            }
        ////        //        }
        ////        //    }

        ////        //    if (SchemeDiscountUserContext != null && SchemeDiscountUserContext.Any() && Discount == 0)
        ////        //    {
        ////        //        foreach (var SchemeDiscountUser in SchemeDiscountUserContext)
        ////        //        {
        ////        //            var SchemeDiscountUserMappingsContext = _emamiContext.SchemeDiscountUserMappings.AsNoTracking()
        ////        //                .FirstOrDefault(w => w.SchemeDiscountUserId == SchemeDiscountUser.Id
        ////        //                 && w.UserId == UserContext.Id
        ////        //                 && w.SkuId == SkuId);

        ////        //            if (SchemeDiscountUserMappingsContext != null)
        ////        //            {
        ////        //                if (SchemeDiscountUserMappingsContext.UserId == DealerId)
        ////        //                {
        ////        //                    Discount = SchemeDiscountUser.Discount;
        ////        //                }
        ////        //            }
        ////        //        }
        ////        //    }
        ////        //}

        ////        #region Scheme Discount               

        ////        var SchemeDiscountGeographyData = _emamiContext.SchemeDiscountGeography.AsNoTracking()
        ////                                .Join(_emamiContext.SchemeDiscountGeographyMappings.AsNoTracking(), s => s.Id, sd => sd.SchemeDiscountGeographyId, (s, sd) => new { SchemeDiscount = s, SchemeDiscountGeography = sd })
        ////                                .FirstOrDefault(f => DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(f.SchemeDiscount.ValidTo)
        ////                                && DbFunctions.TruncateTime(f.SchemeDiscount.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
        ////                                && f.SchemeDiscountGeography.CustomerId == UserContext.Id
        ////                                && f.SchemeDiscountGeography.CityId == UserContext.CityId
        ////                                && f.SchemeDiscountGeography.SkuId == SkuId
        ////                                && f.SchemeDiscountGeography.IsActive);

        ////        if (SchemeDiscountGeographyData != null && SchemeDiscountGeographyData.SchemeDiscount != null && SchemeDiscountGeographyData.SchemeDiscountGeography != null)
        ////        {
        ////            Discount = SchemeDiscountGeographyData.SchemeDiscount.Discount;
        ////        }
        ////        else
        ////        {
        ////            var SchemeDiscountUserData = _emamiContext.SchemeDiscountUsers.AsNoTracking()
        ////                .Join(_emamiContext.SchemeDiscountUserMappings.AsNoTracking(), s => s.Id, sd => sd.SchemeDiscountUserId, (s, sd) => new { SchemeDiscount = s, SchemeDiscountUser = sd })
        ////                .Join(_emamiContext.Skus.AsNoTracking(), su => su.SchemeDiscountUser.SkuId, sk => sk.Id, (su, sk) => new { SkuName = sk.SkuName, SkuDiscount = su.SchemeDiscount, SkuUserDiscount = su.SchemeDiscountUser })
        ////                .FirstOrDefault(f => DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(f.SkuDiscount.ValidTo)
        ////                && DbFunctions.TruncateTime(f.SkuDiscount.ValidFrom) <= DbFunctions.TruncateTime(currentDate)
        ////                && f.SkuUserDiscount.CustomerId == UserContext.Id
        ////                && f.SkuUserDiscount.IsActive
        ////                && f.SkuUserDiscount.SkuId == SkuId);

        ////            if (SchemeDiscountUserData != null && SchemeDiscountUserData.SkuDiscount != null && SchemeDiscountUserData.SkuUserDiscount != null)
        ////            {
        ////                Discount = SchemeDiscountUserData.SkuDiscount.Discount;
        ////            }
        ////        }
        ////        #endregion
        ////    }
        ////    return Discount;
        ////}

        //public decimal SchemeDiscountGeography(long SkuId, long DealerId, DateTime BiddingDate)
        //{
        //    var Discount = (decimal)0;
        //    var UserContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == DealerId);
        //    if (UserContext != null)
        //    {
        //        var CustomerGroupContext = _emamiContext.CustomerGroupDetails.AsNoTracking().FirstOrDefault(_ => _.CustomerId == DealerId);
        //        if (CustomerGroupContext != null)
        //        {
        //            var SchemeDiscountGeographyDatas = _emamiContext.SchemeDiscountGeography.AsNoTracking()
        //                .Join(_emamiContext.SchemeDiscountGeographyMappings.AsNoTracking(), s => s.Id, sd => sd.SchemeDiscountGeographyId, (s, sd) => new { SchemeDiscount = s, SchemeDiscountGeography = sd })
        //                .Where(f => DbFunctions.TruncateTime(BiddingDate) <= DbFunctions.TruncateTime(f.SchemeDiscount.ValidTo)
        //                && DbFunctions.TruncateTime(f.SchemeDiscount.ValidFrom) <= DbFunctions.TruncateTime(BiddingDate)
        //                && f.SchemeDiscountGeography.SkuId == SkuId
        //                && f.SchemeDiscountGeography.CustomerGroupId == CustomerGroupContext.CustomerGroupId
        //                && f.SchemeDiscountGeography.CityId == UserContext.CityId
        //                && f.SchemeDiscountGeography.IsActive).ToList();

        //            var SchemeDiscountUserContext = SchemeDiscountGeographyDatas.Select(_ => _.SchemeDiscount);
        //            var SchemeDiscountUserMappings = SchemeDiscountGeographyDatas.Select(_ => _.SchemeDiscountGeography);

        //            //var SchemeDiscountUserContext = _emamiContext.SchemeDiscountGeography.AsNoTracking()
        //            //    .Where(_ => _.CustomerGroupId == CustomerGroupContext.CustomerGroupId
        //            //         && DbFunctions.TruncateTime(BiddingDate) <= DbFunctions.TruncateTime(_.ValidTo)
        //            //         && DbFunctions.TruncateTime(_.ValidFrom) <= DbFunctions.TruncateTime(BiddingDate)).ToList();

        //            if (SchemeDiscountUserContext.IsAny() && SchemeDiscountUserMappings.IsAny())
        //            {
        //                foreach (var SchemeDiscountUser in SchemeDiscountUserContext)
        //                {
        //                    //var SchemeDiscountUserMappingsContext = _emamiContext.SchemeDiscountGeographyMappings.AsNoTracking()
        //                    //    .FirstOrDefault(w => w.SchemeDiscountGeographyId == SchemeDiscountUser.Id
        //                    //    && w.CityId == UserContext.CityId
        //                    //    && w.SkuId == SkuId);

        //                    var SchemeDiscountUserMappingsContext = SchemeDiscountUserMappings
        //                        .FirstOrDefault(w => w.SchemeDiscountGeographyId == SchemeDiscountUser.Id);

        //                    if (SchemeDiscountUserMappingsContext != null)
        //                    {
        //                        if (SchemeDiscountUserMappingsContext.CityId == UserContext.CityId)
        //                        {
        //                            Discount = SchemeDiscountUser.Discount;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return Discount;
        //}

        ////public ResultDto SaudaBiddingCreation(SaudaBiddingCreationInputDto inputDto)
        ////{
        ////    _methodName = "SaudaBiddingCreation";

        ////    long DealerTypeId = 0;
        ////    string IncotermsType = string.Empty;
        ////    long BrokerId = 0;
        ////    long? depotIdForRake = 0;

        ////    List<VolumeCapacityDto> volumeList = new List<VolumeCapacityDto>();
        ////    List<SaudaCreateNotificationDto> saudaCreateEmailList = new List<SaudaCreateNotificationDto>();
        ////    DateTime currentdate = DateHelper.UtcToIndia(DateTime.UtcNow);
        ////    long saudaId = 0;

        ////    try
        ////    {
        ////        if (inputDto == null)
        ////        {
        ////            return _resultService.ErrorMessage(Constants.InvalidRequest);
        ////        }
        ////        if (inputDto.BiddingWindowId <= 0)
        ////        {
        ////            return _resultService.ErrorMessage(Constants.BiddingWindowisMissing);
        ////        }
        ////        if (inputDto.DealerId <= 0)
        ////        {
        ////            return _resultService.ErrorMessage(Constants.DealerMissing);
        ////        }
        ////        if (inputDto.LoginUserId <= 0)
        ////        {
        ////            return _resultService.ErrorMessage(Constants.InvalidUser);
        ////        }
        ////        if (inputDto.SaudaBiddingDetails.IsNotAny())
        ////        {
        ////            return _resultService.ErrorMessage(Constants.InvalidRequest);
        ////        }
        ////        List<BiddingWindowDashboardChartVolumeCapacityDto> result = new List<BiddingWindowDashboardChartVolumeCapacityDto>();
        ////        var mtValidationMessage = "";
        ////        using (SqlConnection connection = new SqlConnection(Config.DBConnectionString))
        ////        {
        ////            try
        ////            {
        ////                connection.Open();
        ////                result = connection.Query<BiddingWindowDashboardChartVolumeCapacityDto>("GetBiddingWindowTotalAndRemaining", new
        ////                {
        ////                    BiddingWindowId = inputDto.BiddingWindowId
        ////                }, commandType: CommandType.StoredProcedure).ToList();
        ////            }
        ////            catch (Exception exception)
        ////            {
        ////                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
        ////                _logger.Error(message);
        ////            }
        ////            finally
        ////            {
        ////                connection.Close();
        ////            }
        ////        }
        ////        var OilTypes = _emamiContext.OilTypes.AsNoTracking().ToList();
        ////        foreach (var data in inputDto.SaudaBiddingDetails)
        ////        {
        ////            var validatemessage = "";
        ////            var detailsAgainstOilType = result.FirstOrDefault(_ => _.OilTypeId == data.OilTypeId);
        ////            var OilTypeName = OilTypes.FirstOrDefault(_ => _.Id == data.OilTypeId).Name;
        ////            if (detailsAgainstOilType.BookedVolumeCapacity == 0 && detailsAgainstOilType.RemainingVolumeCapacity == 0)
        ////            {
        ////                if (data.BidQuantityInMT > detailsAgainstOilType.TotalVolumeCapacity)
        ////                {
        ////                    validatemessage = "For OilType" + OilTypeName + " available quantity is " + detailsAgainstOilType.TotalVolumeCapacity + "," + Environment.NewLine;
        ////                }
        ////            }
        ////            else if (detailsAgainstOilType.RemainingVolumeCapacity == 0 || data.BidQuantityInMT > detailsAgainstOilType.RemainingVolumeCapacity)
        ////            {
        ////                validatemessage = "For OilType" + OilTypeName + " available quantity is " + detailsAgainstOilType.RemainingVolumeCapacity + "," + Environment.NewLine;
        ////            }
        ////            else
        ////            {
        ////                validatemessage = "";
        ////            }
        ////            mtValidationMessage = mtValidationMessage + validatemessage;
        ////        }

        ////        if (!String.IsNullOrEmpty(mtValidationMessage))
        ////        {
        ////            return _resultService.ErrorMessage(mtValidationMessage);
        ////        }

        ////        var skuIds = inputDto.SaudaBiddingDetails.Select(s => s.SkuId).Distinct().ToList();
        ////        var skuDatas = _emamiContext.Skus.AsNoTracking().Where(w => skuIds.Contains(w.Id)).Select(s => new { Id = s.Id, Name = s.SkuName }).ToList();

        ////        var biddingWindows = _emamiContext.BiddingWindow.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.BiddingWindowId);
        ////        if (biddingWindows != null && biddingWindows.StatusId != (int)DTO.Enums.BiddWindowStatus.Processing)
        ////        {
        ////            var errorMessage = Constants.BiddingWindowStatusChanged + Utility.GetEnumFromString<DTO.Enums.BiddWindowStatus>(biddingWindows.StatusId);
        ////            return _resultService.ErrorMessage(errorMessage);
        ////        }

        ////        var saudaConfiguration = _emamiContext.RaSaudaConfiguration.AsNoTracking().FirstOrDefault(f => f.IsActive);

        ////        if (saudaConfiguration != null)
        ////        {
        ////            var saudaAllocationTime = saudaConfiguration.SaudaAllocationTime;
        ////            var saudaAllocationDateTime = new DateTime(currentdate.Year, currentdate.Month, currentdate.Day, saudaAllocationTime.Hours, saudaAllocationTime.Minutes, saudaAllocationTime.Seconds, saudaAllocationTime.Milliseconds);
        ////            inputDto.SaudaAllocationTime = string.Format(Constants.SaudaAllocationTimeFormat, saudaAllocationDateTime);
        ////        }

        ////        var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
        ////        if (dealerContext == null)
        ////        {
        ////            return _resultService.ErrorMessage(Constants.UserNotFound);
        ////        }

        ////        var dealerRole = _emamiContext.UserRoles.FirstOrDefault(_ => _.UserId == inputDto.DealerId);
        ////        if (dealerRole != null)
        ////        {
        ////            DealerTypeId = dealerRole.RoleId == (int)DTO.Enums.Role.Broker ? (int)DTO.Enums.DealerType.Broker : (int)DTO.Enums.DealerType.Direct;
        ////            if (dealerRole.RoleId == (int)DTO.Enums.Role.Broker)
        ////            {
        ////                BrokerId = inputDto.DealerId;
        ////            }
        ////            else
        ////            {
        ////                var BrokerContext = (from ucm in _emamiContext.UserCustomerMapping
        ////                                     join ur in _emamiContext.UserRoles on ucm.UserId equals ur.UserId
        ////                                     where ur.RoleId == (int)DTO.Enums.Role.Broker
        ////                                     && ucm.CustomerId == inputDto.DealerId
        ////                                     select new
        ////                                     {
        ////                                         BrokerId = ucm.UserId
        ////                                     }).FirstOrDefault();

        ////                if (BrokerContext != null)
        ////                {
        ////                    BrokerId = BrokerContext.BrokerId;
        ////                }
        ////            }
        ////        }

        ////        int gpBenefitType = 0;
        ////        long gpBenefitAppliedType = 0;
        ////        long gpBenefitCategoryType = 0;
        ////        decimal gpBenefitDiscountOrDays = 0;
        ////        decimal gpBenefitDiscountCase = 0;
        ////        decimal gpDiscount = 0;
        ////        var validToAddDays = 0;

        ////        #region SaudaBiddingCartHeader Insert
        ////        var SaudaBiddingCartHeader = new SaudaBiddingCartHeader
        ////        {
        ////            BiddingWindowId = inputDto.BiddingWindowId,
        ////            BiddingDateAndTime = DateHelper.UtcToIndia(DateTime.UtcNow),
        ////            DealerId = inputDto.DealerId,
        ////            CreatedBy = inputDto.LoginUserId,
        ////            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        ////        };
        ////        _emamiContext.SaudaBiddingCartHeaders.Add(SaudaBiddingCartHeader);
        ////        _emamiContext.SaveChanges();
        ////        #endregion

        ////        List<SaudaBiddingCart> saudaBiddingCartList = new List<SaudaBiddingCart>();
        ////        List<SaudaOrder> saudaOrderList = new List<SaudaOrder>();
        ////        List<CounterBiddingInputDto> counterBidList = new List<CounterBiddingInputDto>();

        ////        int i = 0;
        ////        foreach (var SaudaBiddingDetail in inputDto.SaudaBiddingDetails)
        ////        {
        ////            gpBenefitType = 0;
        ////            gpBenefitAppliedType = 0;
        ////            gpBenefitCategoryType = 0;
        ////            gpBenefitDiscountOrDays = 0;
        ////            gpBenefitDiscountCase = 0;
        ////            gpDiscount = 0;
        ////            long plantDepotId = 0;

        ////            long StatusId = (int)DTO.Enums.Status.Pending;
        ////            DateTime? saudaValidFromDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        ////            if (SaudaBiddingDetail.IncotermId == (int)DTO.Enums.IncoTerms.ExRake || SaudaBiddingDetail.IncotermId == (int)DTO.Enums.IncoTerms.ExRake)
        ////            {
        ////                depotIdForRake = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == SaudaBiddingDetail.PlantId && !_.IsPlant)?.DepotId;
        ////            }
        ////            var IncotermContext = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == SaudaBiddingDetail.IncotermId).Name;
        ////            IncotermsType = IncotermContext.ToLower().Contains("for") ? "For" : "Ex";

        ////            if (SaudaBiddingDetail.BidPricePerCase >= SaudaBiddingDetail.GuarateedPricePerCase)
        ////            {
        ////                StatusId = (int)DTO.Enums.Status.Approved;

        ////                #region Sauda GP Benefits
        ////                gpBenefitType = SaudaBiddingDetail.GPBenefitType;
        ////                gpBenefitAppliedType = SaudaBiddingDetail.GPBenefitAppliedTypeId;
        ////                gpBenefitCategoryType = SaudaBiddingDetail.GPBenefitOrCategoryId;
        ////                if (SaudaBiddingDetail.GPBenefitType == (int)DTO.Enums.BenefitType.NONSAP)
        ////                {
        ////                    gpBenefitDiscountOrDays = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.GPBenefitDiscountOrDay;
        ////                    gpBenefitDiscountCase = SaudaBiddingDetail.GPBenefitDiscountOrDay;
        ////                }
        ////                else
        ////                {
        ////                    gpBenefitDiscountOrDays = SaudaBiddingDetail.GPBenefitDiscountOrDay;
        ////                }
        ////                #endregion

        ////                #region Maximum Guaratee Price Validation
        ////                if (saudaConfiguration != null && saudaConfiguration.GuaranteePricePercentage > 0)
        ////                {
        ////                    var maximumAmount = Utility.PercentageCalculation(saudaConfiguration.GuaranteePricePercentage, SaudaBiddingDetail.GuarateedPricePerCase);
        ////                    if (SaudaBiddingDetail.BidPricePerCase > maximumAmount)
        ////                    {
        ////                        SaudaBiddingDetail.BidPricePerCase = maximumAmount;
        ////                    }
        ////                }
        ////                #endregion

        ////            }
        ////            else if (SaudaBiddingDetail.BidPricePerCase >= SaudaBiddingDetail.BaseRate)
        ////            {
        ////                StatusId = (int)DTO.Enums.Status.Approved;
        ////            }
        ////            else if (SaudaBiddingDetail.BidPricePerCase < SaudaBiddingDetail.BaseRate)
        ////            {
        ////                StatusId = (int)DTO.Enums.Status.Pending;
        ////            }

        ////            #region Discount Calculation
        ////            var skuDiscount = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.SkuDiscount;
        ////            var schemeDiscount = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.SchemeDiscount;
        ////            var volumeDiscount = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.VolumeDiscountCal;  //SaudaBiddingDetail.VolumeDiscountCal;
        ////            if (gpBenefitType == (int)DTO.Enums.BenefitType.NONSAP)
        ////            {
        ////                gpDiscount = gpBenefitDiscountOrDays;
        ////            }
        ////            var totalDiscount = skuDiscount + schemeDiscount + volumeDiscount + gpDiscount;
        ////            var bidPrice = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.BidPricePerCase;
        ////            var bidPriceGrandTotal = bidPrice - totalDiscount;
        ////            #endregion

        ////            #region Sauda Validity Calculation
        ////            decimal saudaValidityPeriod = Convert.ToDecimal(dealerContext.SaudaValidityPeriod > 0 ? dealerContext.SaudaValidityPeriod : Config.DefaultSaudaValidity);
        ////            validToAddDays = Convert.ToInt32(saudaValidityPeriod);
        ////            if (gpBenefitType == (int)DTO.Enums.BenefitType.SAP)
        ////            {
        ////                validToAddDays = Convert.ToInt32((saudaValidityPeriod + gpBenefitDiscountOrDays));
        ////            }
        ////            #endregion

        ////            SaudaBiddingDetail.StatusId = StatusId;
        ////            SaudaBiddingDetail.Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == StatusId).Name;
        ////            SaudaBiddingDetail.SkuName = skuDatas.IsAny() ? skuDatas.FirstOrDefault(f => f.Id == SaudaBiddingDetail.SkuId).Name : string.Empty;

        ////            #region PricingLive to Pricing DataInsert and Rearrange the Pricing Data
        ////            ///Pricing Live is contain Current day Pricing
        ////            ///So, we insert the Pricing Live data into Pricing table for Sauda booked records only
        ////            /// Daily we cleanup and fresh data insert into the pricing live table
        ////            var pricingLiveContext = _emamiContext.TodayPricing.FirstOrDefault(_ => _.Id == SaudaBiddingDetail.PricingId);
        ////            //var pricingContext = default(Pricing);

        ////            if (pricingLiveContext == null)
        ////            {
        ////                return _resultService.ErrorMessage(Constants.PricingIdisnotValid);
        ////            }
        ////            if (pricingLiveContext.PricingReferneceId == 0)
        ////            {
        ////                var pricing = new Pricing()
        ////                {
        ////                    SkuId = pricingLiveContext.SkuId,
        ////                    OilTypeId = pricingLiveContext.OilTypeId,
        ////                    OilPackingTypeId = pricingLiveContext.OilPackingTypeId,
        ////                    PlantId = pricingLiveContext.PlantId,
        ////                    Price = pricingLiveContext.Price,
        ////                    SalesOrganizationId = pricingLiveContext.SalesOrganizationId,
        ////                    DistributionChannelId = pricingLiveContext.DistributionChannelId,
        ////                    DivisionId = pricingLiveContext.DivisionId,
        ////                    SAPPricingCode = pricingLiveContext.SAPPricingCode,
        ////                    ValidFrom = pricingLiveContext.ValidFrom,
        ////                    ValidTo = pricingLiveContext.ValidTo,
        ////                    CreatedBy = pricingLiveContext.CreatedBy,
        ////                    CreatedDate = pricingLiveContext.CreatedDate,
        ////                    ModifiedBy = pricingLiveContext.ModifiedBy,
        ////                    ModifiedDate = pricingLiveContext.ModifiedDate,
        ////                };
        ////                _emamiContext.Pricing.Add(pricing);
        ////                _emamiContext.SaveChanges();
        ////                /// Update pricingLive Record Pricing Reference Id
        ////                //var pricingLiveRecord = _emamiContext.TodayPricing.FirstOrDefault(s => s.Id == pricingLiveContext.Id);
        ////                pricingLiveContext.PricingReferneceId = pricing.Id;
        ////                SaudaBiddingDetail.PricingId = pricing.Id;
        ////                _emamiContext.SaveChanges();
        ////                //pricingContext = pricing;
        ////            }
        ////            else
        ////            {
        ////                SaudaBiddingDetail.PricingId = pricingLiveContext.PricingReferneceId;
        ////                //pricingContext = _emamiContext.Pricing.FirstOrDefault(s => s.Id == pricingLiveContext.PricingReferneceId);
        ////            }

        ////            #endregion

        ////            #region SaudaBiddingCart Insert
        ////            var saudaBiddingCart = new SaudaBiddingCart
        ////            {
        ////                BiddingWindowId = inputDto.BiddingWindowId,
        ////                BiddingDateAndTime = DateHelper.UtcToIndia(DateTime.UtcNow),
        ////                DealerId = inputDto.DealerId,
        ////                IncotermId = SaudaBiddingDetail.IncotermId,
        ////                PlantId = SaudaBiddingDetail.PlantId,
        ////                DepotId = SaudaBiddingDetail.DepotId,
        ////                OilTypeId = SaudaBiddingDetail.OilTypeId,
        ////                SkuId = SaudaBiddingDetail.SkuId,
        ////                BidPrice = bidPriceGrandTotal,
        ////                GuarateedPricePerCase = SaudaBiddingDetail.GuarateedPricePerCase,
        ////                BidPricePerCase = SaudaBiddingDetail.BidPricePerCase,
        ////                BidQuantityInCase = SaudaBiddingDetail.BidQuantityInCase,
        ////                BidQuantityInMT = _resultService.ConvertCasetoMetricTon(SaudaBiddingDetail.BidQuantityInCase, SaudaBiddingDetail.SkuId),
        ////                TotalPrice = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.BidPricePerCase,
        ////                ChanceNumber = SaudaBiddingDetail.ChanceNumber,
        ////                TotalChance = SaudaBiddingDetail.TotalChance,
        ////                StatusId = StatusId,
        ////                SaudaBiddingCartHeaderId = SaudaBiddingCartHeader.Id,
        ////                CreatedBy = inputDto.LoginUserId,
        ////                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        ////                PricingId = SaudaBiddingDetail.PricingId,

        ////                //SKU
        ////                SkuDiscountCase = SaudaBiddingDetail.SkuDiscount,
        ////                SkuDiscount = skuDiscount,
        ////                SkuDiscountType = SaudaBiddingDetail.SkuDiscountType,
        ////                //SCHEME
        ////                SchemeDiscountCase = SaudaBiddingDetail.SchemeDiscount,
        ////                SchemeDiscount = schemeDiscount,
        ////                SchemeDiscountType = SaudaBiddingDetail.SchemeDiscountType,
        ////                //VOLUME
        ////                VolumeDiscountCase = SaudaBiddingDetail.VolumeDiscountCal,
        ////                VolumeDiscount = volumeDiscount,
        ////                VolumeDiscountType = SaudaBiddingDetail.VolumeDiscountType,
        ////                //GP BENEFITS
        ////                GPBenefitType = gpBenefitType,
        ////                GPBenefitAppliedTypeId = gpBenefitAppliedType,
        ////                GPBenefitOrCategoryId = gpBenefitCategoryType,
        ////                GPBenefitDiscountOrDay = gpBenefitDiscountOrDays,
        ////                GPBenefitDiscountInCase = gpBenefitDiscountCase,

        ////                BaseRate = SaudaBiddingDetail.BaseRate,
        ////                ValidFromDate = saudaValidFromDate.Value,
        ////                ValidToDate = saudaValidFromDate.Value.AddDays(validToAddDays),

        ////                BaseBidQuantityInCase = SaudaBiddingDetail.BidQuantityInCase,
        ////                CounterBidPrice = SaudaBiddingDetail.BidPricePerCase
        ////            };
        ////            saudaBiddingCartList.Add(saudaBiddingCart);
        ////            //_emamiContext.SaudaBiddingCart.Add(saudaBiddingCart);
        ////            //_emamiContext.SaveChanges();
        ////            #endregion

        ////            if (StatusId == (int)DTO.Enums.Status.Approved)
        ////            {
        ////                if (SaudaBiddingDetail.IncotermId == (int)DTO.Enums.IncoTerms.ExPlant || SaudaBiddingDetail.IncotermId == (int)DTO.Enums.IncoTerms.ForPlant)
        ////                {
        ////                    plantDepotId = SaudaBiddingDetail.PlantId;
        ////                }
        ////                else if (SaudaBiddingDetail.IncotermId == (int)DTO.Enums.IncoTerms.ExDepot || SaudaBiddingDetail.IncotermId == (int)DTO.Enums.IncoTerms.ForDepot)
        ////                {
        ////                    plantDepotId = SaudaBiddingDetail.DepotId;
        ////                }
        ////                else
        ////                {
        ////                    plantDepotId = SaudaBiddingDetail.DepotId;
        ////                }

        ////                #region Sauda & SaudaOrders Insert
        ////                if (saudaId == 0)
        ////                {
        ////                    var saudaContext = new Sauda
        ////                    {
        ////                        BiddingDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        ////                        UserId = inputDto.DealerId,
        ////                        //SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.ReverseAuction,
        ////                        CreatedBy = inputDto.LoginUserId,
        ////                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        ////                        IsSAPDataSync = false,
        ////                        IsSAPDataSyncApproval = false,
        ////                        RABookingId = SaudaBiddingCartHeader.Id
        ////                    };
        ////                    _emamiContext.Sauda.Add(saudaContext);
        ////                    _emamiContext.SaveChanges();
        ////                    saudaId = saudaContext.Id;
        ////                }

        ////                i = i + 10;
        ////                var saudaOrder = new SaudaOrder
        ////                {
        ////                    SaudaId = saudaId,
        ////                    SaudaNumber = i.ToString(),
        ////                    SkuId = SaudaBiddingDetail.SkuId,
        ////                    OilTypeId = SaudaBiddingDetail.OilTypeId,
        ////                    BidPriceBeforeDiscount = SaudaBiddingDetail.BidPricePerCase,
        ////                    BidPriceBeforeDiscountForDailyReport = SaudaBiddingDetail.BidPricePerCase,
        ////                    BidPrice = bidPriceGrandTotal,
        ////                    BidPriceForDailyReport = bidPriceGrandTotal,
        ////                    BidPricePerCase = SaudaBiddingDetail.BidPricePerCase,
        ////                    BidPricePerCaseForDailyReport = SaudaBiddingDetail.BidPricePerCase,
        ////                    DiscountTypeId = 0,
        ////                    DiscountAmount = 0,
        ////                    DiscountTypeIdForDailyReport = 0,
        ////                    DiscountAmountForDailyReport = 0,
        ////                    BidQuantity = _resultService.ConvertCasetoMetricTon(SaudaBiddingDetail.BidQuantityInCase, SaudaBiddingDetail.SkuId),
        ////                    BidQuantityCase = SaudaBiddingDetail.BidQuantityInCase,
        ////                    QuotedPrice = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.BidPricePerCase,
        ////                    BidQuantityForDailyReport = _resultService.ConvertCasetoMetricTon(SaudaBiddingDetail.BidQuantityInCase, SaudaBiddingDetail.SkuId),
        ////                    BidQuantityCaseForDailyReport = SaudaBiddingDetail.BidQuantityInCase,
        ////                    QuotedPriceForDailyReport = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.BidPricePerCase,
        ////                    CreatedBy = inputDto.LoginUserId,
        ////                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        ////                    BiddingwindowId = inputDto.BiddingWindowId,
        ////                    //SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.ReverseAuction,
        ////                    PricingId = SaudaBiddingDetail.PricingId,
        ////                    DealerTypeId = DealerTypeId,
        ////                    Incoterms1 = IncotermsType,
        ////                    PlantId = plantDepotId,
        ////                    //DealerLocationId = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId).FreightRouteId ?? 0,
        ////                    CustomerPONumber = dealerContext.Code + DateHelper.UtcToIndia(DateTime.UtcNow).ToShortDateString(),
        ////                    StatusId = (int)DTO.Enums.Status.Pending,
        ////                    SaudaStatusId = (int)DTO.Enums.SaudaStatus.NotReleased,
        ////                    CustomerPONumberForDailyReport = dealerContext.Code + DateHelper.UtcToIndia(DateTime.UtcNow).ToShortDateString(),
        ////                    StatusIdForDailyReport = (int)DTO.Enums.Status.Pending,
        ////                    SaudaStatusIdForDailyReport = (int)DTO.Enums.SaudaStatus.NotReleased,
        ////                    Incoterms2 = SaudaBiddingDetail.IncotermId,
        ////                    BrokerId = BrokerId,
        ////                    BrokerIdForDailyReport = BrokerId,
        ////                    IsSAPDataSync = false,
        ////                    IsSAPDataSyncApproval = false,
        ////                    DepotIdForRake = depotIdForRake.Value,

        ////                    //SKU
        ////                    SkuDiscountCase = SaudaBiddingDetail.SkuDiscount,
        ////                    SkuDiscount = skuDiscount,
        ////                    SkuDiscountType = SaudaBiddingDetail.SkuDiscountType,
        ////                    SkuDiscountCaseForDailyReport = SaudaBiddingDetail.SkuDiscount,
        ////                    SkuDiscountForDailyReport = skuDiscount,
        ////                    SkuDiscountTypeForDailyReport = SaudaBiddingDetail.SkuDiscountType,
        ////                    //SCHEME
        ////                    SchemeDiscountCase = SaudaBiddingDetail.SchemeDiscount,
        ////                    SchemeDiscount = schemeDiscount,
        ////                    SchemeDiscountType = SaudaBiddingDetail.SchemeDiscountType,
        ////                    SchemeDiscountCaseForDailyReport = SaudaBiddingDetail.SchemeDiscount,
        ////                    SchemeDiscountForDailyReport = schemeDiscount,
        ////                    SchemeDiscountTypeForDailyReport = SaudaBiddingDetail.SchemeDiscountType,
        ////                    //VOLUME
        ////                    VolumeDiscountCase = SaudaBiddingDetail.VolumeDiscountCal,
        ////                    VolumeDiscount = volumeDiscount,
        ////                    VolumeDiscountType = SaudaBiddingDetail.VolumeDiscountType,
        ////                    VolumeDiscountCaseForDailyReport = SaudaBiddingDetail.VolumeDiscountCal,
        ////                    VolumeDiscountForDailyReport = volumeDiscount,
        ////                    VolumeDiscountTypeForDailyReport = SaudaBiddingDetail.VolumeDiscountType,
        ////                    //GP BENEFITS
        ////                    GPBenefitType = gpBenefitType,
        ////                    GPBenefitAppliedTypeId = gpBenefitAppliedType,
        ////                    GPBenefitOrCategoryId = gpBenefitCategoryType,
        ////                    GPBenefitDiscountOrDay = gpBenefitDiscountOrDays,
        ////                    GPBenefitDiscountInCase = gpBenefitDiscountCase,
        ////                    GPBenefitTypeForDailyReport = gpBenefitType,
        ////                    GPBenefitAppliedTypeIdForDailyReport = gpBenefitAppliedType,
        ////                    GPBenefitOrCategoryIdForDailyReport = gpBenefitCategoryType,
        ////                    GPBenefitDiscountOrDayForDailyReport = gpBenefitDiscountOrDays,
        ////                    GPBenefitDiscountInCaseForDailyReport = gpBenefitDiscountCase,

        ////                    BaseRate = SaudaBiddingDetail.BaseRate,
        ////                    BaseRateForDailyReport = SaudaBiddingDetail.BaseRate,
        ////                    ValidFromDate = saudaValidFromDate.Value,
        ////                    ValidToDate = saudaValidFromDate.Value.AddDays(validToAddDays),
        ////                    IsBaseSauda = true
        ////                };
        ////                saudaOrderList.Add(saudaOrder);
        ////                //_emamiContext.SaudaOrders.Add(saudaOrder);
        ////                //_emamiContext.SaveChanges();
        ////                #endregion

        ////                #region Notification Details
        ////                var raTotalDiscount = saudaOrder.VolumeDiscountCase +
        ////                         saudaOrder.SchemeDiscountCase +
        ////                         saudaOrder.SkuDiscountCase +
        ////                         (saudaOrder.GPBenefitType == (int)DTO.Enums.BenefitType.NONSAP ? saudaOrder.GPBenefitDiscountInCase : 0) +
        ////                         (saudaOrder.SurpriseBenefitType == (int)DTO.Enums.BenefitType.NONSAP ? saudaOrder.SurpriseBenefitDiscountInCase : 0);
        ////                var bidPricePerCause = saudaOrder.QuotedPrice / saudaOrder.BidQuantityCase;
        ////                decimal discountGstPercentage = 0;
        ////                decimal discountWithTax = 0;
        ////                decimal discountTaxAmount = 0;
        ////                decimal taxPaidValue = 0;
        ////                if (saudaOrder.Incoterms2 == (long)DTO.Enums.IncoTerms.ExPlant || saudaOrder.Incoterms2 == (long)DTO.Enums.IncoTerms.ForPlant)
        ////                {
        ////                    //discountGstPercentage = Utility.GetGstAmount(1, pricingLiveContext.PlantGSTPercentage);
        ////                    discountWithTax = raTotalDiscount * discountGstPercentage;
        ////                    discountTaxAmount = discountWithTax - raTotalDiscount;
        ////                    taxPaidValue = Utility.DecimalFormatTwo(bidPricePerCause);// - discountTaxAmount;
        ////                }
        ////                else if (saudaOrder.Incoterms2 == (long)DTO.Enums.IncoTerms.ExDepot || saudaOrder.Incoterms2 == (long)DTO.Enums.IncoTerms.ForDepot)
        ////                {
        ////                    //discountGstPercentage = Utility.GetGstAmount(1, pricingLiveContext.DepotGSTPercentage);
        ////                    discountWithTax = raTotalDiscount * discountGstPercentage;
        ////                    discountTaxAmount = discountWithTax - raTotalDiscount;
        ////                    taxPaidValue = Utility.DecimalFormatTwo(bidPricePerCause);// - discountTaxAmount;
        ////                }

        ////                saudaCreateEmailList.Add(new SaudaCreateNotificationDto()
        ////                {
        ////                    StatusId = Convert.ToInt32(SaudaBiddingDetail.StatusId),
        ////                    //SaudaOrderId = saudaOrder.Id,
        ////                    SaudaBookingTypeId = saudaOrder.SaudaBookingTypeId,
        ////                    LoginUserId = inputDto.LoginUserId,
        ////                    SkuName = SaudaBiddingDetail.SkuName,
        ////                    BiddingWindowId = inputDto.BiddingWindowId,
        ////                    OilTypeId = saudaOrder.OilTypeId,
        ////                    BidPrice = taxPaidValue,//bidPriceGrandTotal / SaudaBiddingDetail.BidQuantityInCase,
        ////                    BidQuantityInCase = SaudaBiddingDetail.BidQuantityInCase,
        ////                    WindowName = biddingWindows.Name
        ////                });
        ////                #endregion
        ////            }

        ////            #region Counter Bid Offer
        ////            if (StatusId == (int)DTO.Enums.Status.Pending)
        ////            {
        ////                CounterBiddingInputDto param = new CounterBiddingInputDto()
        ////                {
        ////                    LoginUserId = inputDto.LoginUserId,
        ////                    DealerId = inputDto.DealerId,
        ////                    BiddingWindowId = inputDto.BiddingWindowId,
        ////                    OilTypeId = SaudaBiddingDetail.OilTypeId,
        ////                    SkuId = SaudaBiddingDetail.SkuId,
        ////                    IncotermId = SaudaBiddingDetail.IncotermId,
        ////                    DealerMobileNumber = dealerContext.MobileNumber
        ////                };
        ////                //CounterBidNotification(param);
        ////                counterBidList.Add(param);
        ////            }
        ////            #endregion
        ////        }

        ////        if (saudaBiddingCartList.IsAny())
        ////        {
        ////            _emamiContext.BulkInsertProxy(saudaBiddingCartList);
        ////        }
        ////        if (saudaOrderList.IsAny())
        ////        {
        ////            _emamiContext.BulkInsertProxy(saudaOrderList);
        ////        }
        ////        _emamiContext.SaveChanges();

        ////        if (counterBidList.IsAny())
        ////        {
        ////            var maxQty = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(f => f.Key == DTO.Enums.Configuration.MaximumQtyForCounterBidCalculation.ToString());
        ////            var maxBid = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(f => f.Key == DTO.Enums.Configuration.MaximumBidForCounterBidCalculation.ToString());
        ////            foreach (var data in counterBidList)
        ////            {
        ////                CounterBidNotification(data, Convert.ToInt32(maxBid.Value), Convert.ToInt32(maxQty.Value));
        ////            }
        ////        }

        ////        if (inputDto.SaudaBiddingDetails.IsAny())
        ////        {
        ////            bool isPending = inputDto.SaudaBiddingDetails.All(a => a.StatusId == (int)DTO.Enums.Status.Pending);
        ////            if (isPending)
        ////            {
        ////                inputDto.Message = Constants.SaudaBookedErrorMessage;
        ////            }
        ////            else
        ////            {
        ////                inputDto.Message = Constants.SaudaBookedSuccessMessage.Replace("##SAUDAALLOCATIONTIME##", inputDto.SaudaAllocationTime);   // $"Your sauda has been booked. Please complete SKU allocation within {inputDto.SaudaAllocationTime} time";
        ////            }
        ////        }

        ////        if (saudaCreateEmailList.IsAny())
        ////        {
        ////            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => RASaudaCreateNotificationAsync(saudaCreateEmailList, inputDto.LoginUserId, inputDto.DealerId, (int)DTO.Enums.NotificationType.SaudaCreation, cancellationToken));
        ////        }
        ////        return _resultService.SuccessObject(inputDto);
        ////    }
        ////    catch (Exception exception)
        ////    {
        ////        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        ////        _logger.Error(message);
        ////        return _resultService.ErrorMessage(Constants.Exception);
        ////    }
        ////}

        //public void RASaudaCreateNotificationAsync(List<SaudaCreateNotificationDto> inputDto, long LoginUserId, long DealerId, int notificationType, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        //{
        //    try
        //    {
        //        if (inputDto.IsAny())
        //        {
        //            using (IDbConnection conn = new SqlConnection(Config.DBConnectionString))
        //            {
        //                AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
        //                List<string> toUsers = new List<string>();
        //                var saudaordersku = string.Join(",", inputDto.Select(S => S.SkuName));
        //                string userName = "";

        //                #region Get Common Datas

        //                string userQuery = @"Select Id,Name,Email,MobileNumber,RegistrationTypeId,PushTokenKey From Users Where Id = @LoginUserId Or Id = @DealerId";
        //                var userDetails = conn.Query<RaNotificationSendDto>(userQuery, new
        //                {
        //                    LoginUserId = LoginUserId,
        //                    DealerId = DealerId
        //                }).ToList();
        //                if (userDetails.IsAny())
        //                {
        //                    toUsers = userDetails.Select(s => s.Email).ToList();
        //                    userName = userDetails.FirstOrDefault(f => f.Id == LoginUserId)?.Name;
        //                }
        //                var usersContext = userDetails.FirstOrDefault(_ => _.Id == LoginUserId);

        //                string[] actionKeys = new string[]
        //                {
        //                        Constants.IsSMSUrl,
        //                        Constants.IsEMAILUrl,
        //                        Constants.IsPushNotificationUrl,
        //                        Constants.FirebaseSenderId,
        //                        Constants.PushNotifyServerkey,
        //                        Constants.PushNotifyUrl,
        //                };
        //                string configurationsQuery = @"Select Id,[Key],Value From Configurations Where [Key] In @Keys";
        //                var actionResult = conn.Query<NotificationsStatusDto>(configurationsQuery, new
        //                {
        //                    Keys = actionKeys
        //                }).ToList();

        //                string[] templateNames = new string[]
        //                {
        //                    Constants.SaudaBiddingApprovedNotificationEmail,
        //                    Constants.SaudaBiddingApprovedNotificationSMS
        //                };
        //                string emailTemplatesQuery = @"Select Name,PlainTemplate,Template From EmailTemplates Where Name In @Name";
        //                var emailTemplatesDatas = conn.Query<NotificationsStatusDto>(emailTemplatesQuery, new
        //                {
        //                    Name = templateNames
        //                }).ToList();

        //                #endregion

        //                #region EMAIL
        //                bool isEmail = false;
        //                var IsEmail = actionResult.FirstOrDefault(_ => _.Key == Constants.IsEMAILUrl)?.Value;
        //                if (IsEmail.Equals("1") || IsEmail.Equals("True"))
        //                    isEmail = true;
        //                else
        //                    isEmail = false;

        //                if (isEmail && toUsers.IsAny())
        //                {
        //                    var fromEmail = Constants.FromEmail;
        //                    var plainText = string.Empty;

        //                    string emailSubject = Constants.SaudaBiddingApprovedSubject;
        //                    var emailTemplate = emailTemplatesDatas.FirstOrDefault(email => email.Name == Constants.SaudaBiddingApprovedNotificationEmail);
        //                    if (emailTemplate != null)
        //                    {
        //                        var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, saudaordersku).Replace(Constants.BY_FOR, Constants.BY).Replace(Constants.UserName, userName);
        //                        var emailResult = _notificationService.SaudaCreateEmailTemplate(inputDto, userName, notificationType);
        //                        var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, emailResult);
        //                        amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
        //                    }
        //                }
        //                #endregion

        //                #region SMS
        //                var smsPlainTemplate = string.Empty;
        //                var smsMessage = string.Empty;
        //                bool isSms = false;
        //                var IsSMS = actionResult.FirstOrDefault(_ => _.Key == Constants.IsSMSUrl).Value;
        //                if (IsSMS.Equals("1") || IsSMS.Equals("True"))
        //                    isSms = true;
        //                else
        //                    isSms = false;

        //                var smsTemplate = emailTemplatesDatas.FirstOrDefault(email => email.Name == Constants.SaudaBiddingApprovedNotificationSMS);
        //                smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, saudaordersku)
        //                            .Replace(Constants.BY_FOR, Constants.BY).Replace(Constants.UserName, userName);
        //                var result = _notificationService.SaudaCreateSmsTemplate(inputDto, userName, notificationType);
        //                smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, result);

        //                if (isSms && !string.IsNullOrEmpty(smsMessage))
        //                {
        //                    if (smsTemplate != null && !string.IsNullOrEmpty(usersContext.MobileNumber))
        //                    {
        //                        amazonNotificationService.SendMessage(smsMessage, usersContext.MobileNumber);
        //                    }
        //                }
        //                #endregion

        //                #region Push Notification
        //                bool isPushNotification = false;
        //                var IsPushNotification = actionResult.FirstOrDefault(_ => _.Key == Constants.IsPushNotificationUrl)?.Value;
        //                if (IsPushNotification.Equals("1") || IsPushNotification.Equals("True"))
        //                    isPushNotification = true;
        //                else
        //                    isPushNotification = false;

        //                if (isPushNotification && !string.IsNullOrEmpty(smsMessage) && usersContext.RegistrationTypeId > 0 && !string.IsNullOrEmpty(usersContext.PushTokenKey))
        //                {
        //                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                    {
        //                        PushTokenKey = usersContext.PushTokenKey,
        //                        RegistrationTypeId = usersContext.RegistrationTypeId != null ? (int)usersContext.RegistrationTypeId : 0,
        //                        Title = Constants.SaudaCreationSubject,
        //                        Message = smsMessage, //smsPlainTemplate
        //                        //Id = saudaOrderContext.Id,
        //                    };
        //                    SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                }
        //                #endregion

        //                #region Push Notification Nested Method
        //                void SendPushNotificationThroughFirebase(PushNotificationInputDto pushNotificationInputDto)
        //                {
        //                    try
        //                    {
        //                        var firebaseSenderId = actionResult.FirstOrDefault(_ => _.Key == Constants.FirebaseSenderId).Value;
        //                        var pushNotifyServerkey = actionResult.FirstOrDefault(_ => _.Key == Constants.PushNotifyServerkey).Value;
        //                        var pushNotifyUrl = actionResult.FirstOrDefault(_ => _.Key == Constants.PushNotifyUrl).Value;

        //                        WebRequest tRequest = WebRequest.Create(pushNotifyUrl);
        //                        tRequest.Method = "post";
        //                        tRequest.ContentType = "application/json";
        //                        var json = new JavaScriptSerializer().Serialize(string.Empty);
        //                        if (pushNotificationInputDto.RegistrationTypeId == (int)DTO.Enums.RegistrationType.Android)
        //                        {
        //                            var data = new
        //                            {
        //                                to = pushNotificationInputDto.PushTokenKey,
        //                                data = new
        //                                {
        //                                    sound = "default",
        //                                    message = pushNotificationInputDto.Message,
        //                                    title = pushNotificationInputDto.Title,
        //                                    id = pushNotificationInputDto.Id,
        //                                },
        //                                priority = "high"
        //                            };
        //                            json = new JavaScriptSerializer().Serialize(data);
        //                        }
        //                        else if (pushNotificationInputDto.RegistrationTypeId == (int)DTO.Enums.RegistrationType.IOS)
        //                        {
        //                            var data = new
        //                            {
        //                                to = pushNotificationInputDto.PushTokenKey,
        //                                data = new
        //                                {
        //                                    sound = "default",
        //                                    message = pushNotificationInputDto.Message,
        //                                    title = pushNotificationInputDto.Title,
        //                                    id = pushNotificationInputDto.Id,
        //                                },
        //                                notification = new
        //                                {
        //                                    title = pushNotificationInputDto.Title,
        //                                    body = pushNotificationInputDto.Message,
        //                                    id = pushNotificationInputDto.Id,
        //                                    sound = "default",
        //                                },
        //                                priority = "high"
        //                            };
        //                            json = new JavaScriptSerializer().Serialize(data);
        //                        }

        //                        Byte[] byteArray = Encoding.UTF8.GetBytes(json);
        //                        tRequest.Headers.Add(string.Format("Authorization: key={0}", pushNotifyServerkey));
        //                        tRequest.Headers.Add(string.Format("Sender: id={0}", firebaseSenderId));
        //                        tRequest.ContentLength = byteArray.Length;
        //                        using (Stream dataStream = tRequest.GetRequestStream())
        //                        {
        //                            dataStream.Write(byteArray, 0, byteArray.Length);
        //                            using (WebResponse tResponse = tRequest.GetResponse())
        //                            {
        //                                using (Stream dataStreamResponse = tResponse.GetResponseStream())
        //                                {
        //                                    using (StreamReader tReader = new StreamReader(dataStreamResponse))
        //                                    {
        //                                        String sResponseFromServer = tReader.ReadToEnd();
        //                                        string str = sResponseFromServer;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                    catch (Exception exception)
        //                    {
        //                        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //                        _logger.Error(message);
        //                    }
        //                }
        //                #endregion

        //                #region Volume Capacity Email
        //                var oilTypeData = inputDto.Select(s => s.OilTypeId).Distinct().ToList();
        //                var biddingWindowId = inputDto.FirstOrDefault().BiddingWindowId;
        //                foreach (var oilTypeId in oilTypeData)
        //                {
        //                    var volume = conn.QueryFirstOrDefault<VolumeCapacityDto>("GetVolumeCapacityDetails",
        //                          new
        //                          {
        //                              BiddingWoindowId = biddingWindowId,
        //                              OilTypeId = oilTypeId
        //                          }, commandType: CommandType.StoredProcedure);

        //                    if (volume != null)
        //                    {
        //                        StringBuilder sb = new StringBuilder();
        //                        List<VolumeCapacityDto> volumeCapacities = new List<VolumeCapacityDto>();

        //                        sb.Clear();
        //                        sb.Append(" Select Value as ValueRange From Configurations");
        //                        sb.Append(" Where Name in (@Name)");
        //                        var volumeCapacityNotificationRange = conn.QueryFirstOrDefault<string>(sb.ToString(),
        //                        new
        //                        {
        //                            Name = UtilityHelper.GetEnumDescription(DTO.Enums.Configuration.VolumeCapacityNotificationRange)
        //                        });

        //                        if (!string.IsNullOrEmpty(volumeCapacityNotificationRange))
        //                        {
        //                            var volumeRangeList = volumeCapacityNotificationRange.Split(',').ToList();
        //                            if (volumeRangeList.IsAny())
        //                            {
        //                                foreach (var item in volumeRangeList)
        //                                {
        //                                    var startRange = Convert.ToDecimal(item.Split('-')[0]);
        //                                    var endRange = Convert.ToDecimal(item.Split('-')[1]);

        //                                    if (volume.UsedPercentage >= startRange && volume.UsedPercentage <= endRange)
        //                                    {
        //                                        volumeCapacities.Add(volume);
        //                                        //VolumeCapacityNotification(volumeCapacities);

        //                                        sb.Clear();
        //                                        sb.Append(" Select Value as Email From Configurations");
        //                                        sb.Append(" Where Name in (@Name)");
        //                                        var mailIds = conn.QueryFirstOrDefault<UserNotificationDto>(sb.ToString(),
        //                                        new
        //                                        {
        //                                            Name = UtilityHelper.GetEnumDescription(DTO.Enums.Configuration.VolumeCapacityNotificationEmail)
        //                                        });

        //                                        if (mailIds != null && !string.IsNullOrEmpty(mailIds.Email))
        //                                        {
        //                                            var mailIdsList = mailIds.Email.Split(',').ToList();
        //                                            mailIdsList.RemoveAll(x => string.IsNullOrEmpty(x));

        //                                            sb.Clear();
        //                                            sb.Append(" Select Name,PlainTemplate,Template From EmailTemplates");
        //                                            sb.Append(" Where Name in (@Name)");
        //                                            var emailTemplate = conn.QueryFirstOrDefault<EmailTemplateDto>(sb.ToString(),
        //                                            new
        //                                            {
        //                                                Name = "VolumeCapacity"
        //                                            });

        //                                            string emailSubject = string.Empty;
        //                                            var fromEmail = Constants.FromEmail;
        //                                            var plainText = string.Empty;
        //                                            emailSubject = "Bidding Window - Volume Capacity";

        //                                            sb.Clear();
        //                                            sb.Append("<p>Dear User,The volume capacity details are <br>");
        //                                            foreach (var volumeDetail in volumeCapacities)
        //                                            {
        //                                                sb.Append(" Bidding Window Name : <b>" + volumeDetail.WindowName + "</b>, Window Start & End Time : <b>" + string.Format("{0:HH:mm tt}", volumeDetail.StartTime) + "-" + string.Format("{0:HH:mm tt}", volumeDetail.EndTime) + "</b>");
        //                                            }

        //                                            sb.Append("<p><br></p><div style='padding-bottom: 50px;'><table text-align=left border=1  width=100% align=center cellpadding=10 style='border-collapse:collapse'>");
        //                                            foreach (var volumeDetail in volumeCapacities)
        //                                            {
        //                                                sb.Append("<tr><td width=50% style='padding: 10px;'><b>Oil Name</b></td><td width=50% style='padding: 10px;'>" + volumeDetail.OilName + "</td></tr><tr><td width=50% style='padding: 10px;'><b>Total Volume Capacity(MT)</b></td><td width=50% style='padding: 10px;'>" + Math.Round(volumeDetail.TotalVolumeCapacity, 2) + "</td></tr><tr><td width=50% style='padding: 10px;'><b>Remaining Volume Capacity(MT)</b></td><td width=50% style='padding: 10px;'>" + Math.Round(volumeDetail.RemainingVolumeCapacity, 2) + "</td></tr><tr><td width=50% style='padding: 10px;'><b> Used Percentage </b></td><td width=50% style='padding: 10px;'>" + Math.Round(volumeDetail.UsedPercentage, 2) + "%</td></tr>");
        //                                            }
        //                                            sb.Append("</table></div></p>");

        //                                            var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, sb.ToString());
        //                                            amazonNotificationService.SendEmail(mailIdsList, emailSubject, plainText, htmlTemplate, true);
        //                                            break;
        //                                        }
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                #endregion
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        ////public ResultDto SaudaBiddingCreationOld(SaudaBiddingCreationInputDto inputDto)
        ////{
        ////    _methodName = "SaudaBiddingCreationOld";

        ////    long DealerTypeId = 0;
        ////    string IncotermsType = string.Empty;
        ////    long BrokerId = 0;
        ////    long? depotIdForRake = 0;

        ////    List<VolumeCapacityDto> volumeList = new List<VolumeCapacityDto>();
        ////    List<SaudaCreateNotificationDto> saudaCreateEmailList = new List<SaudaCreateNotificationDto>();
        ////    DateTime currentdate = DateHelper.UtcToIndia(DateTime.UtcNow);

        ////    try
        ////    {
        ////        if (inputDto == null)
        ////        {
        ////            return _resultService.ErrorMessage(Constants.InvalidRequest);
        ////        }
        ////        if (inputDto.BiddingWindowId <= 0)
        ////        {
        ////            return _resultService.ErrorMessage(Constants.BiddingWindowisMissing);
        ////        }
        ////        if (inputDto.DealerId <= 0)
        ////        {
        ////            return _resultService.ErrorMessage(Constants.DealerMissing);
        ////        }
        ////        if (inputDto.LoginUserId <= 0)
        ////        {
        ////            return _resultService.ErrorMessage(Constants.InvalidUser);
        ////        }
        ////        if (inputDto.SaudaBiddingDetails.Count() < 0 && inputDto.SaudaBiddingDetails.Any())
        ////        {
        ////            return _resultService.ErrorMessage(Constants.InvalidRequest);
        ////        }

        ////        var skuIds = inputDto.SaudaBiddingDetails.Select(s => s.SkuId).Distinct().ToList();
        ////        var skuDatas = _emamiContext.Skus.AsNoTracking().Where(w => skuIds.Contains(w.Id)).Select(s => new { Id = s.Id, Name = s.SkuName }).ToList();

        ////        var biddingWindows = _emamiContext.BiddingWindow.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.BiddingWindowId);
        ////        if (biddingWindows != null && biddingWindows.StatusId != (int)DTO.Enums.BiddWindowStatus.Processing)
        ////        {
        ////            var errorMessage = Constants.BiddingWindowStatusChanged + Utility.GetEnumFromString<DTO.Enums.BiddWindowStatus>(biddingWindows.StatusId);
        ////            return _resultService.ErrorMessage(errorMessage);
        ////        }

        ////        var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
        ////        if (dealerContext == null)
        ////        {
        ////            return _resultService.ErrorMessage(Constants.UserNotFound);
        ////        }

        ////        var dealerRole = _emamiContext.UserRoles.FirstOrDefault(_ => _.UserId == inputDto.DealerId);
        ////        if (dealerRole != null)
        ////        {
        ////            DealerTypeId = dealerRole.RoleId == (int)DTO.Enums.Role.Broker ? (int)DTO.Enums.DealerType.Broker : (int)DTO.Enums.DealerType.Direct;
        ////            if (dealerRole.RoleId == (int)DTO.Enums.Role.Broker)
        ////            {
        ////                BrokerId = inputDto.DealerId;
        ////            }
        ////            else
        ////            {
        ////                var BrokerContext = (from ucm in _emamiContext.UserCustomerMapping
        ////                                     join ur in _emamiContext.UserRoles on ucm.UserId equals ur.UserId
        ////                                     where ur.RoleId == (int)DTO.Enums.Role.Broker
        ////                                     && ucm.CustomerId == inputDto.DealerId
        ////                                     select new
        ////                                     {
        ////                                         BrokerId = ucm.UserId
        ////                                     }).FirstOrDefault();

        ////                if (BrokerContext != null)
        ////                {
        ////                    BrokerId = BrokerContext.BrokerId;
        ////                }
        ////            }
        ////        }

        ////        int gpBenefitType = 0;
        ////        long gpBenefitAppliedType = 0;
        ////        long gpBenefitCategoryType = 0;
        ////        decimal gpBenefitDiscountOrDays = 0;

        ////        #region Get Common Data's

        ////        var bookedSkuIds = inputDto.SaudaBiddingDetails.Select(s => s.SkuId).ToList();
        ////        var GpBenefitGeography = _emamiContext.GPBenefitGeography.AsNoTracking()
        ////            .Join(_emamiContext.GPBenefitGeographyMappings.AsNoTracking(), g => g.Id, gd => gd.GPBenefitGeographyId, (g, gd) => new { Geography = g, GeographyDetail = gd })
        ////            .Where(f => DbFunctions.TruncateTime(currentdate) <= DbFunctions.TruncateTime(f.Geography.ValidTo)
        ////                    && DbFunctions.TruncateTime(f.Geography.ValidFrom) <= DbFunctions.TruncateTime(currentdate)
        ////                    && f.GeographyDetail.CustomerId == dealerContext.Id
        ////                    && f.GeographyDetail.CityId == dealerContext.CityId
        ////                    && bookedSkuIds.Contains(f.GeographyDetail.SkuId)
        ////                    && f.Geography.IsActive);

        ////        var GpBenefitUser = _emamiContext.GPBenefitUsers.AsNoTracking()
        ////        .Join(_emamiContext.GPBenefitUserMappings.AsNoTracking(), g => g.Id, gd => gd.GPBenefitUserId, (g, gd) => new { User = g, UserDetail = gd })
        ////        .Where(f => DbFunctions.TruncateTime(currentdate) <= DbFunctions.TruncateTime(f.User.ValidTo)
        ////                && DbFunctions.TruncateTime(f.User.ValidFrom) <= DbFunctions.TruncateTime(currentdate)
        ////                && f.UserDetail.UserId == dealerContext.Id
        ////                && bookedSkuIds.Contains(f.UserDetail.SkuId)
        ////                && f.User.IsActive);

        ////        #endregion


        ////        var SaudaBiddingCartHeader = new SaudaBiddingCartHeader
        ////        {
        ////            BiddingWindowId = inputDto.BiddingWindowId,
        ////            BiddingDateAndTime = DateHelper.UtcToIndia(DateTime.UtcNow),
        ////            DealerId = inputDto.DealerId,
        ////            CreatedBy = inputDto.LoginUserId,
        ////            CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        ////        };
        ////        _emamiContext.SaudaBiddingCartHeaders.Add(SaudaBiddingCartHeader);
        ////        _emamiContext.SaveChanges();

        ////        foreach (var SaudaBiddingDetail in inputDto.SaudaBiddingDetails)
        ////        {
        ////            long StatusId = (int)DTO.Enums.Status.Pending;
        ////            DateTime? saudaValidFromDate = DateHelper.UtcToIndia(DateTime.UtcNow);

        ////            if (SaudaBiddingDetail.IncotermId == (int)DTO.Enums.IncoTerms.ExRake || SaudaBiddingDetail.IncotermId == (int)DTO.Enums.IncoTerms.ExRake)
        ////            {
        ////                depotIdForRake = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == SaudaBiddingDetail.PlantId && !_.IsPlant)?.DepotId;
        ////            }

        ////            var IncotermContext = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == SaudaBiddingDetail.IncotermId).Name;
        ////            IncotermsType = IncotermContext.ToLower().Contains("for") ? "For" : "Ex";

        ////            #region oId
        ////            if (SaudaBiddingDetail.BidPricePerCase >= SaudaBiddingDetail.GuarateedPricePerCase)
        ////            {
        ////                StatusId = (int)DTO.Enums.Status.Approved;


        ////                #region GP Benefits

        ////                if (GpBenefitGeography.IsAny())
        ////                {
        ////                    var geography = GpBenefitGeography.FirstOrDefault(f => f.GeographyDetail.SkuId == SaudaBiddingDetail.SkuId);
        ////                    if (geography != null)
        ////                    {
        ////                        gpBenefitType = (int)DTO.Enums.RaDiscountType.Geography;
        ////                        gpBenefitAppliedType = geography.Geography.BenefitTypesId;
        ////                        gpBenefitCategoryType = geography.Geography.BenefitOrCategoryId;
        ////                        gpBenefitDiscountOrDays = geography.Geography.DiscountOrDays;
        ////                    }
        ////                }
        ////                else
        ////                {
        ////                    if (GpBenefitUser.IsAny())
        ////                    {
        ////                        var userBenefit = GpBenefitUser.FirstOrDefault(f => f.UserDetail.SkuId == SaudaBiddingDetail.SkuId);
        ////                        if (userBenefit != null)
        ////                        {
        ////                            gpBenefitType = (int)DTO.Enums.RaDiscountType.User;
        ////                            gpBenefitAppliedType = userBenefit.User.BenefitTypesId;
        ////                            gpBenefitCategoryType = userBenefit.User.BenefitOrCategoryId;
        ////                            gpBenefitDiscountOrDays = userBenefit.User.DiscountOrDays;
        ////                        }
        ////                    }
        ////                }

        ////                #endregion

        ////            }
        ////            else if (SaudaBiddingDetail.BidPricePerCase > SaudaBiddingDetail.BaseRate)
        ////            {
        ////                StatusId = (int)DTO.Enums.Status.Pending;
        ////            }
        ////            else if (SaudaBiddingDetail.BidPricePerCase <= SaudaBiddingDetail.BaseRate)
        ////            {
        ////                StatusId = (int)DTO.Enums.Status.Rejected;
        ////            }
        ////            #endregion

        ////            #region New
        ////            if (SaudaBiddingDetail.BidPricePerCase >= SaudaBiddingDetail.GuarateedPricePerCase)
        ////            {
        ////                StatusId = (int)DTO.Enums.Status.Approved;



        ////            }
        ////            else if (SaudaBiddingDetail.BidPricePerCase >= SaudaBiddingDetail.BaseRate)
        ////            {
        ////                StatusId = (int)DTO.Enums.Status.Approved;
        ////            }
        ////            else if (SaudaBiddingDetail.BidPricePerCase < SaudaBiddingDetail.BaseRate)
        ////            {
        ////                StatusId = (int)DTO.Enums.Status.Pending;
        ////            }
        ////            #endregion

        ////            SaudaBiddingDetail.StatusId = StatusId;
        ////            SaudaBiddingDetail.Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == StatusId).Name;

        ////            SaudaBiddingDetail.SkuName = skuDatas.IsAny() ? skuDatas.FirstOrDefault(f => f.Id == SaudaBiddingDetail.SkuId).Name : string.Empty;
        ////            var saudaAllocationTime = $"{string.Format("{0:HH:mm tt}", biddingWindows.SaudaAllocationStartTime)}  -  {string.Format("{0:HH:mm tt}", biddingWindows.SaudaAllocationEndTime)}";
        ////            inputDto.SaudaAllocationTime = saudaAllocationTime;

        ////            var saudaBiddingCart = new SaudaBiddingCart
        ////            {
        ////                BiddingWindowId = inputDto.BiddingWindowId,
        ////                BiddingDateAndTime = DateHelper.UtcToIndia(DateTime.UtcNow),
        ////                DealerId = inputDto.DealerId,
        ////                IncotermId = SaudaBiddingDetail.IncotermId,
        ////                PlantId = SaudaBiddingDetail.PlantId,
        ////                DepotId = SaudaBiddingDetail.DepotId,
        ////                OilTypeId = SaudaBiddingDetail.OilTypeId,
        ////                SkuId = SaudaBiddingDetail.SkuId,
        ////                GuarateedPricePerCase = SaudaBiddingDetail.GuarateedPricePerCase,
        ////                BidPricePerCase = SaudaBiddingDetail.BidPricePerCase,
        ////                BidQuantityInCase = SaudaBiddingDetail.BidQuantityInCase,
        ////                BidQuantityInMT = _resultService.ConvertCasetoMetricTon(SaudaBiddingDetail.BidQuantityInCase, SaudaBiddingDetail.SkuId),
        ////                TotalPrice = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.BidPricePerCase,
        ////                ChanceNumber = SaudaBiddingDetail.ChanceNumber,
        ////                TotalChance = SaudaBiddingDetail.TotalChance,
        ////                StatusId = StatusId,

        ////                SaudaBiddingCartHeaderId = SaudaBiddingCartHeader.Id,
        ////                CreatedBy = inputDto.LoginUserId,
        ////                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),

        ////                SkuDiscountCase = SaudaBiddingDetail.SkuDiscount,
        ////                SkuDiscount = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.SkuDiscount,
        ////                SkuDiscountType = SaudaBiddingDetail.SkuDiscountType,

        ////                SchemeDiscountCase = SaudaBiddingDetail.SchemeDiscount,
        ////                SchemeDiscount = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.SchemeDiscount,
        ////                SchemeDiscountType = SaudaBiddingDetail.SchemeDiscountType,

        ////                VolumeDiscount = SaudaBiddingDetail.VolumeDiscountCal,
        ////                VolumeDiscountType = SaudaBiddingDetail.VolumeDiscountType,

        ////                BaseRate = SaudaBiddingDetail.BaseRate
        ////            };
        ////            _emamiContext.SaudaBiddingCart.Add(saudaBiddingCart);
        ////            _emamiContext.SaveChanges();

        ////            if (StatusId == (int)DTO.Enums.Status.Pending)
        ////            {
        ////                CounterBiddingInputDto param = new CounterBiddingInputDto()
        ////                {
        ////                    LoginUserId = inputDto.LoginUserId,
        ////                    DealerId = inputDto.DealerId,
        ////                    BiddingWindowId = inputDto.BiddingWindowId,
        ////                    OilTypeId = SaudaBiddingDetail.OilTypeId,
        ////                    SkuId = SaudaBiddingDetail.SkuId,
        ////                    IncotermId = SaudaBiddingDetail.IncotermId
        ////                };
        ////                CounterBidNotification(param);
        ////            }
        ////        }

        ////        var ApprovedBiddings = _emamiContext.SaudaBiddingCart.Where(_ => _.SaudaBiddingCartHeaderId == SaudaBiddingCartHeader.Id && _.StatusId == (int)DTO.Enums.Status.Approved);
        ////        if (ApprovedBiddings != null && ApprovedBiddings.Any())
        ////        {
        ////            var saudaContext = new Sauda
        ////            {
        ////                BiddingDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        ////                UserId = inputDto.DealerId,
        ////                SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.ReverseAuction,
        ////                CreatedBy = inputDto.LoginUserId,
        ////                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        ////                IsSAPDataSync = false,
        ////                IsSAPDataSyncApproval = false,
        ////                RABookingId = SaudaBiddingCartHeader.Id
        ////            };
        ////            _emamiContext.Sauda.Add(saudaContext);
        ////            _emamiContext.SaveChanges();

        ////            foreach (var SaudaBiddingDetail in inputDto.SaudaBiddingDetails)
        ////            {
        ////                depotIdForRake = 0;
        ////                long StatusId = (int)DTO.Enums.Status.Pending;
        ////                DateTime? saudaValidFromDate = DateHelper.UtcToIndia(DateTime.UtcNow);

        ////                if (SaudaBiddingDetail.IncotermId == (int)DTO.Enums.IncoTerms.ExRake || SaudaBiddingDetail.IncotermId == (int)DTO.Enums.IncoTerms.ExRake)
        ////                {
        ////                    depotIdForRake = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == SaudaBiddingDetail.PlantId && !_.IsPlant)?.DepotId;
        ////                }

        ////                var IncotermContext = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == SaudaBiddingDetail.IncotermId).Name;
        ////                IncotermsType = IncotermContext.ToLower().Contains("for") ? "For" : "Ex";

        ////                if (SaudaBiddingDetail.BidPricePerCase >= SaudaBiddingDetail.GuarateedPricePerCase)
        ////                {
        ////                    StatusId = (int)DTO.Enums.Status.Approved;
        ////                }
        ////                else if (SaudaBiddingDetail.BidPricePerCase > SaudaBiddingDetail.BaseRate)
        ////                {
        ////                    StatusId = (int)DTO.Enums.Status.Pending;
        ////                }
        ////                else if (SaudaBiddingDetail.BidPricePerCase <= SaudaBiddingDetail.BaseRate)
        ////                {
        ////                    StatusId = (int)DTO.Enums.Status.Rejected;
        ////                }

        ////                if (StatusId == (int)DTO.Enums.Status.Approved)
        ////                {
        ////                    #region GP Benefits

        ////                    long benefitTypeId = 0L;
        ////                    string benefitType = string.Empty;
        ////                    string benefitOrCategory = string.Empty;
        ////                    var benefitDays = 0L;
        ////                    var gpBenefitDiscount = 0.0m;
        ////                    var gpBenefitUser = new List<GPBenefitUser>();

        ////                    var gpBenefitGeography = _emamiContext.GPBenefitGeography.AsNoTracking()
        ////                        .Join(_emamiContext.GPBenefitGeographyMappings.AsNoTracking(), gp => gp.Id, gpd => gpd.GPBenefitGeographyId, (gp, gpd) => new { gp, gpd })
        ////                        .Where(_ => _.gpd.CustomerId == inputDto.DealerId && _.gpd.SkuId == SaudaBiddingDetail.SkuId
        ////                        && _.gp.IsActive
        ////                        && DbFunctions.TruncateTime(currentdate) <= DbFunctions.TruncateTime(_.gp.ValidTo)
        ////                        && DbFunctions.TruncateTime(_.gp.ValidFrom) <= DbFunctions.TruncateTime(currentdate))
        ////                        .Select(_ => _.gp).ToList();

        ////                    if (gpBenefitGeography.IsAny())
        ////                    {
        ////                        #region GP Benefits -Geography Based

        ////                        foreach (var benefit in gpBenefitGeography)
        ////                        {
        ////                            benefitTypeId = benefit.BenefitTypesId;
        ////                            benefitType = benefit.BenefitTypes?.Name;
        ////                            //benefitDiscountOrDays = benefit.DiscountOrDays;

        ////                            if (benefit.BenefitTypesId == (int)DTO.Enums.BenefitType.SAP)
        ////                            {
        ////                                benefitDays = (long)benefit.DiscountOrDays;
        ////                                benefitOrCategory = benefit.BenefitOrCategoryId > 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.BenefitCategory)Convert.ToInt32(benefit.BenefitOrCategoryId)) : string.Empty;
        ////                            }
        ////                            else
        ////                            {
        ////                                gpBenefitDiscount = benefit.DiscountOrDays;
        ////                                var nonsapBenefit = _emamiContext.Benefits.AsNoTracking().FirstOrDefault(_ => _.Id == benefit.BenefitOrCategoryId);
        ////                                if (nonsapBenefit != null)
        ////                                {
        ////                                    benefitOrCategory = nonsapBenefit.BenefitCategory;
        ////                                }
        ////                            }
        ////                        }

        ////                        #endregion
        ////                    }
        ////                    else
        ////                    {
        ////                        #region GP Benefits - UserBased

        ////                        gpBenefitUser = _emamiContext.GPBenefitUsers.AsNoTracking()
        ////                            .Join(_emamiContext.GPBenefitUserMappings.AsNoTracking(), gp => gp.Id, gpd => gpd.GPBenefitUserId, (gp, gpd) => new { gp, gpd })
        ////                            .Where(_ => _.gpd.UserId == inputDto.DealerId && _.gpd.SkuId == SaudaBiddingDetail.SkuId
        ////                            && _.gp.IsActive
        ////                            && DbFunctions.TruncateTime(currentdate) <= DbFunctions.TruncateTime(_.gp.ValidTo)
        ////                            && DbFunctions.TruncateTime(_.gp.ValidFrom) <= DbFunctions.TruncateTime(currentdate))
        ////                            .Select(_ => _.gp).ToList();

        ////                        if (gpBenefitUser != null && gpBenefitUser.Any())
        ////                        {
        ////                            foreach (var benefit in gpBenefitUser)
        ////                            {
        ////                                benefitTypeId = benefit.BenefitTypesId;
        ////                                benefitType = benefit.BenefitTypes?.Name;

        ////                                if (benefit.BenefitTypesId == (int)DTO.Enums.BenefitType.SAP)
        ////                                {
        ////                                    benefitDays = (long)benefit.DiscountOrDays;
        ////                                    benefitOrCategory = benefit.BenefitOrCategoryId > 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.BenefitCategory)Convert.ToInt32(benefit.BenefitOrCategoryId)) : string.Empty;
        ////                                }
        ////                                else
        ////                                {
        ////                                    gpBenefitDiscount = benefit.DiscountOrDays;
        ////                                    var nonsapBenefit = _emamiContext.Benefits.AsNoTracking().FirstOrDefault(_ => _.Id == benefit.BenefitOrCategoryId);
        ////                                    if (nonsapBenefit != null)
        ////                                    {
        ////                                        benefitOrCategory = nonsapBenefit.BenefitCategory;
        ////                                    }
        ////                                }
        ////                            }
        ////                        }

        ////                        #endregion
        ////                    }

        ////                    #endregion

        ////                    var skuDiscount = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.SkuDiscount;
        ////                    var schemeDiscount = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.SchemeDiscount;
        ////                    var volumeDiscount = SaudaBiddingDetail.VolumeDiscountCal;
        ////                    var gpDiscount = gpBenefitDiscount > 0 ? SaudaBiddingDetail.BidQuantityInCase * gpBenefitDiscount : 0;
        ////                    var totalDiscount = skuDiscount + schemeDiscount + volumeDiscount + gpDiscount;
        ////                    var bidPrice = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.BidPricePerCase;
        ////                    var bidPriceGrandTotal = bidPrice - totalDiscount;
        ////                    var saudaValidityPeriod = Convert.ToDouble(dealerContext.SaudaValidityPeriod > 0 ? dealerContext.SaudaValidityPeriod : Config.DefaultSaudaValidity);
        ////                    var validToAddDays = saudaValidityPeriod + benefitDays;

        ////                    var saudaOrder = new SaudaOrder
        ////                    {
        ////                        SaudaId = saudaContext.Id,
        ////                        SkuId = SaudaBiddingDetail.SkuId,
        ////                        OilTypeId = SaudaBiddingDetail.OilTypeId,
        ////                        BidPriceBeforeDiscount = SaudaBiddingDetail.BidPricePerCase,
        ////                        BidPrice = bidPriceGrandTotal,
        ////                        BidPricePerCase = SaudaBiddingDetail.BidPricePerCase,
        ////                        DiscountTypeId = 0,
        ////                        DiscountAmount = 0,
        ////                        BidQuantity = _resultService.ConvertCasetoMetricTon(SaudaBiddingDetail.BidQuantityInCase, SaudaBiddingDetail.SkuId),
        ////                        BidQuantityCase = SaudaBiddingDetail.BidQuantityInCase,
        ////                        QuotedPrice = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.BidPricePerCase,
        ////                        CreatedBy = inputDto.LoginUserId,
        ////                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        ////                        BiddingwindowId = inputDto.BiddingWindowId,
        ////                        SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.ReverseAuction,
        ////                        PricingId = SaudaBiddingDetail.PricingId,
        ////                        DealerTypeId = DealerTypeId,
        ////                        Incoterms1 = IncotermsType,
        ////                        PlantId = SaudaBiddingDetail.PlantId,
        ////                        DealerLocationId = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId).FreightRouteId ?? 0,
        ////                        CustomerPONumber = dealerContext.Code + DateHelper.UtcToIndia(DateTime.UtcNow).ToShortDateString(),
        ////                        GPBenefitDays = benefitDays,
        ////                        ValidFromDate = saudaValidFromDate.Value,
        ////                        ValidToDate = saudaValidFromDate.Value.AddDays(validToAddDays),
        ////                        StatusId = (int)DTO.Enums.Status.Pending,
        ////                        SaudaStatusId = (int)DTO.Enums.SaudaStatus.NotReleased,
        ////                        Incoterms2 = SaudaBiddingDetail.IncotermId,
        ////                        BrokerId = BrokerId,
        ////                        IsSAPDataSync = false,
        ////                        IsSAPDataSyncApproval = false,
        ////                        DepotIdForRake = depotIdForRake.Value,

        ////                        SkuDiscountCase = SaudaBiddingDetail.SkuDiscount,
        ////                        SkuDiscount = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.SkuDiscount,
        ////                        SkuDiscountType = SaudaBiddingDetail.SkuDiscountType,

        ////                        SchemeDiscountCase = SaudaBiddingDetail.SchemeDiscount,
        ////                        SchemeDiscount = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.SchemeDiscount,
        ////                        SchemeDiscountType = SaudaBiddingDetail.SchemeDiscountType,

        ////                        VolumeDiscount = SaudaBiddingDetail.VolumeDiscountCal,
        ////                        VolumeDiscountType = SaudaBiddingDetail.VolumeDiscountType,

        ////                        GPDiscount = gpDiscount,
        ////                        GPDiscountCase = gpBenefitDiscount,
        ////                        BaseRate = SaudaBiddingDetail.BaseRate
        ////                    };
        ////                    _emamiContext.SaudaOrders.Add(saudaOrder);
        ////                    _emamiContext.SaveChanges();

        ////                    #region Sauda - GP Benefits Mapping

        ////                    if (gpBenefitGeography != null && gpBenefitGeography.Any())
        ////                    {
        ////                        //Sauda Benefits Mapping
        ////                        foreach (var benefit in gpBenefitGeography)
        ////                        {
        ////                            var saudaBenefitsMapping = new SurpriseAndGPBenefitHistory();
        ////                            saudaBenefitsMapping.SaudaOrderId = saudaOrder.Id;
        ////                            saudaBenefitsMapping.BenefitTypeId = benefit.BenefitTypesId;
        ////                            saudaBenefitsMapping.BenefitOrCategoryId = benefit.BenefitOrCategoryId;
        ////                            saudaBenefitsMapping.BenefitDiscountOrDays = benefit.DiscountOrDays;
        ////                            saudaBenefitsMapping.BenefitUserOrGeographyId = benefit.Id;
        ////                            saudaBenefitsMapping.SurpriseBenefitAppliedType = Constants.GeographyBased;
        ////                            saudaBenefitsMapping.IsGPBenefit = true;
        ////                            saudaBenefitsMapping.CreatedBy = inputDto.LoginUserId;
        ////                            saudaBenefitsMapping.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        ////                            _emamiContext.SurpriseAndGPBenefitHistory.Add(saudaBenefitsMapping);
        ////                            _emamiContext.SaveChanges();
        ////                        }
        ////                    }
        ////                    else if (gpBenefitUser != null && gpBenefitUser.Any())
        ////                    {
        ////                        //Sauda Benefits Mapping
        ////                        foreach (var benefit in gpBenefitUser)
        ////                        {
        ////                            var saudaBenefitsMapping = new SurpriseAndGPBenefitHistory();
        ////                            saudaBenefitsMapping.SaudaOrderId = saudaOrder.Id;
        ////                            saudaBenefitsMapping.BenefitTypeId = benefit.BenefitTypesId;
        ////                            saudaBenefitsMapping.BenefitOrCategoryId = benefit.BenefitOrCategoryId;
        ////                            saudaBenefitsMapping.BenefitDiscountOrDays = benefit.DiscountOrDays;
        ////                            saudaBenefitsMapping.BenefitUserOrGeographyId = benefit.Id;
        ////                            saudaBenefitsMapping.SurpriseBenefitAppliedType = Constants.UserBased;
        ////                            saudaBenefitsMapping.IsGPBenefit = true;
        ////                            saudaBenefitsMapping.CreatedBy = inputDto.LoginUserId;
        ////                            saudaBenefitsMapping.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        ////                            _emamiContext.SurpriseAndGPBenefitHistory.Add(saudaBenefitsMapping);
        ////                            _emamiContext.SaveChanges();
        ////                        }
        ////                    }
        ////                    #endregion

        ////                    saudaCreateEmailList.Add(new SaudaCreateNotificationDto()
        ////                    {
        ////                        StatusId = Convert.ToInt32(SaudaBiddingDetail.StatusId),
        ////                        SaudaOrderId = saudaOrder.Id,
        ////                        SaudaBookingTypeId = saudaOrder.SaudaBookingTypeId,
        ////                        LoginUserId = inputDto.LoginUserId
        ////                    });

        ////                    #region Window Volume Capacity

        ////                    if (volumeList.IsAny())
        ////                    {
        ////                        bool isExistsOilType = volumeList.Any(a => a.OilTypeId == SaudaBiddingDetail.OilTypeId);
        ////                        if (!isExistsOilType)
        ////                        {
        ////                            volumeList.Add(new VolumeCapacityDto()
        ////                            {
        ////                                BiddingWindowId = inputDto.BiddingWindowId,
        ////                                OilTypeId = SaudaBiddingDetail.OilTypeId
        ////                            });
        ////                        }
        ////                    }
        ////                    else
        ////                    {
        ////                        volumeList.Add(new VolumeCapacityDto()
        ////                        {
        ////                            BiddingWindowId = inputDto.BiddingWindowId,
        ////                            OilTypeId = SaudaBiddingDetail.OilTypeId
        ////                        });
        ////                    }

        ////                    #endregion
        ////                }
        ////            }
        ////            if (volumeList.IsAny())
        ////            {
        ////                ProcessFileTrigger processFileTrigger = new ProcessFileTrigger();
        ////                processFileTrigger.VolumeCapacityRemainderNotificationNew(volumeList);
        ////            }
        ////            HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => RASaudaCreateNotificationAsync(saudaCreateEmailList, inputDto.LoginUserId, inputDto.DealerId, cancellationToken));
        ////        }

        ////        return _resultService.SuccessObject(inputDto);
        ////    }
        ////    catch (Exception exception)
        ////    {
        ////        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        ////        _logger.Error(message);
        ////        return _resultService.ErrorMessage(Constants.Exception);
        ////    }
        ////}

        //public void RASaudaCreateNotificationAsyncold(List<SaudaCreateNotificationDto> inputDto, long DealerId, long LoginUserId, CancellationToken cancellationToken = default(System.Threading.CancellationToken))
        //{
        //    try
        //    {
        //        using (EmamiContext _context = new EmamiContext())
        //        {
        //            List<string> toUsers = new List<string>();
        //            List<string> UsersName = new List<string>();
        //            var saudaordersku = "";

        //            var usersContext = _context.Users.AsNoTracking().FirstOrDefault(_ => _.Id == LoginUserId);
        //            var dealerContext = _context.Users.AsNoTracking().FirstOrDefault(_ => _.Id == DealerId);

        //            if (!string.IsNullOrEmpty(usersContext.Email) && !string.IsNullOrEmpty(usersContext.Email))
        //            {
        //                toUsers.Add(usersContext.Email);
        //                toUsers.Add(dealerContext.Email);
        //                UsersName.Add(usersContext.Name);
        //            }

        //            if (inputDto != null && inputDto.Any())
        //            {
        //                var saudaOrderIds = inputDto.Select(_ => _.SaudaOrderId).ToList();
        //                var saudaOrderContext = _context.SaudaOrders.AsNoTracking().Where(_ => saudaOrderIds.Contains(_.Id)).Select(_ => _.Sku.SkuName).ToList();
        //                if (saudaOrderContext != null && saudaOrderContext.Any())
        //                {
        //                    saudaordersku = String.Join(", ", saudaOrderContext);
        //                }
        //            }

        //            AmazonNotificationService amazonNotificationService = new AmazonNotificationService();

        //            bool isEmail = false;
        //            var IsEmail = _context.Configurations.AsNoTracking().Where(_ => _.Id == (int)DTO.Enums.Configuration.IsEMAIL).Select(_ => _.Value).Single();
        //            if (IsEmail.Equals("1") || IsEmail.Equals("True"))
        //                isEmail = true;
        //            else
        //                isEmail = false;

        //            if (isEmail && toUsers != null && toUsers.Any())
        //            {
        //                var fromEmail = Constants.FromEmail;
        //                string emailSubject = string.Empty;
        //                var plainText = string.Empty;
        //                EmailTemplate emailTemplate = new EmailTemplate();

        //                emailSubject = Constants.SaudaBiddingApprovedSubject;
        //                emailTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaBiddingApprovedNotificationEmail);
        //                if (emailTemplate != null)
        //                {
        //                    var plainTemplate = emailTemplate.PlainTemplate.Replace(Constants.SkuName, saudaordersku).Replace(Constants.BY_FOR, Constants.BY).Replace(Constants.UserName, usersContext.Name);
        //                    var htmlTemplate = emailTemplate.Template.Replace(Constants.ReplaceMainContent, plainTemplate);
        //                    amazonNotificationService.SendEmail(toUsers, emailSubject, plainText, htmlTemplate, true);
        //                }

        //            }
        //            var smsPlainTemplate = string.Empty;

        //            bool isSms = false;
        //            var IsSMS = _context.Configurations.AsNoTracking().Where(_ => _.Id == (int)DTO.Enums.Configuration.IsSMS).Select(_ => _.Value).Single();
        //            if (IsSMS.Equals("1") || IsSMS.Equals("True"))
        //                isSms = true;
        //            else
        //                isSms = false;

        //            if (isSms)
        //            {
        //                var smsMessage = string.Empty;
        //                EmailTemplate smsTemplate = new EmailTemplate();

        //                smsTemplate = _context.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.SaudaBiddingApprovedNotificationSMS);
        //                if (smsTemplate != null)
        //                {
        //                    smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, saudaordersku)
        //                        .Replace(Constants.BY_FOR, Constants.BY).Replace(Constants.UserName, usersContext.Name);
        //                    smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
        //                    if (!string.IsNullOrEmpty(usersContext.MobileNumber))
        //                    {
        //                        amazonNotificationService.SendMessage(smsMessage, usersContext.MobileNumber);
        //                    }
        //                }
        //            }

        //            bool isPushNotification = false;
        //            var IsPushNotification = _context.Configurations.AsNoTracking().Where(_ => _.Id == (int)DTO.Enums.Configuration.IsPushNotification).Select(_ => _.Value).Single();
        //            if (IsPushNotification.Equals("1") || IsPushNotification.Equals("True"))
        //                isPushNotification = true;
        //            else
        //                isPushNotification = false;


        //            if (usersContext.RegistrationTypeId != null && usersContext.RegistrationTypeId > 0 && !string.IsNullOrEmpty(usersContext.PushTokenKey))
        //            {
        //                PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                {
        //                    PushTokenKey = usersContext.PushTokenKey,
        //                    RegistrationTypeId = usersContext.RegistrationTypeId != null ? (int)usersContext.RegistrationTypeId : 0,
        //                    Title = Constants.SaudaCreationSubject,
        //                    Message = smsPlainTemplate,
        //                    //Id = saudaOrderContext.Id,
        //                };
        //                //_notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //            }

        //            #region Push Notification Nested Method
        //            void SendPushNotificationThroughFirebase(PushNotificationInputDto pushNotificationInputDto)
        //            {
        //                try
        //                {
        //                    var firebaseSenderId = _context.Configurations.FirstOrDefault(_ => _.Key == Constants.FirebaseSenderId).Value;
        //                    var pushNotifyServerkey = _context.Configurations.FirstOrDefault(_ => _.Key == Constants.PushNotifyServerkey).Value;
        //                    var pushNotifyUrl = _context.Configurations.FirstOrDefault(_ => _.Key == Constants.PushNotifyUrl).Value;

        //                    WebRequest tRequest = WebRequest.Create(pushNotifyUrl);
        //                    tRequest.Method = "post";
        //                    tRequest.ContentType = "application/json";
        //                    var json = new JavaScriptSerializer().Serialize(string.Empty);
        //                    if (pushNotificationInputDto.RegistrationTypeId == (int)DTO.Enums.RegistrationType.Android)
        //                    {
        //                        var data = new
        //                        {
        //                            to = pushNotificationInputDto.PushTokenKey,
        //                            data = new
        //                            {
        //                                sound = "default",
        //                                message = pushNotificationInputDto.Message,
        //                                title = pushNotificationInputDto.Title,
        //                                id = pushNotificationInputDto.Id,
        //                            },
        //                            priority = "high"
        //                        };
        //                        json = new JavaScriptSerializer().Serialize(data);
        //                    }
        //                    else if (pushNotificationInputDto.RegistrationTypeId == (int)DTO.Enums.RegistrationType.IOS)
        //                    {
        //                        var data = new
        //                        {
        //                            to = pushNotificationInputDto.PushTokenKey,
        //                            data = new
        //                            {
        //                                sound = "default",
        //                                message = pushNotificationInputDto.Message,
        //                                title = pushNotificationInputDto.Title,
        //                                id = pushNotificationInputDto.Id,
        //                            },
        //                            notification = new
        //                            {
        //                                title = pushNotificationInputDto.Title,
        //                                body = pushNotificationInputDto.Message,
        //                                id = pushNotificationInputDto.Id,
        //                                sound = "default",
        //                            },
        //                            priority = "high"
        //                        };
        //                        json = new JavaScriptSerializer().Serialize(data);
        //                    }

        //                    Byte[] byteArray = Encoding.UTF8.GetBytes(json);
        //                    tRequest.Headers.Add(string.Format("Authorization: key={0}", pushNotifyServerkey));
        //                    tRequest.Headers.Add(string.Format("Sender: id={0}", firebaseSenderId));
        //                    tRequest.ContentLength = byteArray.Length;
        //                    using (Stream dataStream = tRequest.GetRequestStream())
        //                    {
        //                        dataStream.Write(byteArray, 0, byteArray.Length);
        //                        using (WebResponse tResponse = tRequest.GetResponse())
        //                        {
        //                            using (Stream dataStreamResponse = tResponse.GetResponseStream())
        //                            {
        //                                using (StreamReader tReader = new StreamReader(dataStreamResponse))
        //                                {
        //                                    String sResponseFromServer = tReader.ReadToEnd();
        //                                    string str = sResponseFromServer;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                catch (Exception exception)
        //                {
        //                    var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //                    _logger.Error(message);
        //                }
        //            }
        //            #endregion
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //public ResultDto SaudaBiddingLists(LoginUserIdDto inputDto)
        //{
        //    var saudaListDto = new List<BookedSaudaDto>();
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }
        //        if (inputDto.LoginUserId < 0)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidUser);
        //        }

        //        var BiddingContext = _emamiContext.SaudaBiddingCartHeaders.AsNoTracking().Where(_ => _.CreatedBy == inputDto.LoginUserId).ToList();
        //        if (BiddingContext != null && BiddingContext.Any())
        //        {
        //            foreach (var bidding in BiddingContext)
        //            {
        //                var BiddingCartContext = _emamiContext.SaudaBiddingCart.AsNoTracking().Where(_ => _.SaudaBiddingCartHeaderId == bidding.Id).ToList();
        //                var Dealer = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == bidding.DealerId);

        //                var saudaDto = new BookedSaudaDto
        //                {
        //                    DealerId = bidding.DealerId,
        //                    Dealer = Dealer.Name,
        //                    SaudaBookedDate = bidding.BiddingDateAndTime,
        //                    IsBroker = _emamiContext.UserRoles.AsNoTracking().Any(_ => _.UserId == bidding.DealerId && _.RoleId == (int)DTO.Enums.Role.Broker) ? true : false,
        //                    SaudaNumber = bidding.Id.ToString()
        //                };

        //                var results = BiddingCartContext.GroupBy(
        //                        p => p.OilTypeId,
        //                        p => p.SkuId,
        //                        (key, g) => new { OilTypeId = key, Skus = g.ToList() }).ToList();

        //                foreach (var detail in results)
        //                {
        //                    var DetailDto = new BookedSaudaDetailDto
        //                    {
        //                        OilTypeId = detail.OilTypeId,
        //                        OilType = _emamiContext.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == detail.OilTypeId).Name,
        //                        SkuCount = detail.Skus.Count
        //                    };
        //                    saudaDto.BookedSaudaDetailDto.Add(DetailDto);
        //                }
        //                saudaListDto.Add(saudaDto);
        //            }
        //        }
        //        return _resultService.SuccessObject(saudaListDto);
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        //public ResultDto SaudaBiddingDetails(IdInputDto inputDto)
        //{
        //    var BiddingDetailoutput = new SaudaBiddingListOutputDto();
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }
        //        var BiddingContext = _emamiContext.SaudaBiddingCartHeaders.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.Id);
        //        if (BiddingContext != null)
        //        {
        //            BiddingDetailoutput.biddingWindowDetails.BiddingWindowId = BiddingContext.BiddingWindow.Id;
        //            BiddingDetailoutput.biddingWindowDetails.BiddingWindowName = BiddingContext.BiddingWindow.Name;
        //            BiddingDetailoutput.biddingWindowDetails.CustomerGroupId = _emamiContext.BiddingWindowCustomerGroups.AsNoTracking().FirstOrDefault(_ => _.BiddingWindowId == BiddingContext.BiddingWindow.Id).CustomerGroupId;
        //            BiddingDetailoutput.biddingWindowDetails.CustomerGroupName = _emamiContext.BiddingWindowCustomerGroups.AsNoTracking().FirstOrDefault(_ => _.BiddingWindowId == BiddingContext.BiddingWindow.Id).CustomerGroup.Name;
        //            BiddingDetailoutput.biddingWindowDetails.StartTime = BiddingContext.BiddingWindow.StartTime;
        //            BiddingDetailoutput.biddingWindowDetails.EndTime = BiddingContext.BiddingWindow.EndTime;
        //            BiddingDetailoutput.biddingWindowDetails.StartEndTime = Utility.ConvertToTime(BiddingContext.BiddingWindow.StartTime) + " - " + Utility.ConvertToTime(BiddingContext.BiddingWindow.EndTime);
        //            BiddingDetailoutput.biddingWindowDetails.WindowStatus = Utility.GetEnumFromString<DTO.Enums.BiddWindowStatus>(BiddingContext.BiddingWindow.StatusId);
        //            BiddingDetailoutput.biddingWindowDetails.WindowStatusId = BiddingContext.BiddingWindow.StatusId;
        //            BiddingDetailoutput.biddingWindowDetails.ServerDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);

        //            var overallStatus = Constants.OverallSaudaStatus;
        //            decimal invoiceQuantity = 0;
        //            var overAllSaudaContext = (from s in _emamiContext.Sauda.AsNoTracking()
        //                                       join so in _emamiContext.SaudaOrders.AsNoTracking() on s.Id equals so.SaudaId
        //                                       where s.UserId == BiddingContext.DealerId
        //                                       && overallStatus.Contains(so.StatusId)
        //                                       select so
        //                                           ).ToList();
        //            var SaudaLimitContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == BiddingContext.DealerId);
        //            BiddingDetailoutput.SaudaDetail.DealerId = BiddingContext.DealerId;
        //            BiddingDetailoutput.SaudaDetail.Dealer = BiddingContext.Dealer.Name;
        //            var userdivContext = _emamiContext.UserDivisionMappings.AsNoTracking()
        //                   .FirstOrDefault(_ => _.UserId == inputDto.DealerId
        //                   && _.SalesOrganizationId == inputDto.SalesOrganizationId && _.DistributionChannelId == inputDto.DistributionChannelId
        //                   && _.DivisionId == inputDto.DivisionId);


        //            if (userdivContext != null)
        //            {
        //                BiddingDetailoutput.SaudaDetail.TotalSaudaLimit = userdivContext.SaudaLimit ?? 0;
        //                BiddingDetailoutput.SaudaDetail.AvailableSaudaLimit = userdivContext.SaudaLimit ?? 0;
        //            }
        //            if (overAllSaudaContext != null)
        //            {
        //                var existingSaudaQuantity = overAllSaudaContext.Sum(_ => _.BidQuantity);
        //                var skuIds = overAllSaudaContext.Select(_ => _.SkuId).Distinct().ToList();
        //                var invoiceContext = (from inv in _emamiContext.Invoices.AsNoTracking()
        //                                      join invDet in _emamiContext.InvoiceDetails.AsNoTracking() on inv.Id equals invDet.InvoiceId
        //                                      where inv.UserId == BiddingContext.DealerId
        //                                      && skuIds.Contains(invDet.SkuId)
        //                                      select invDet
        //                                          ).ToList();

        //                if (invoiceContext != null && invoiceContext.Any())
        //                {
        //                    invoiceQuantity = invoiceContext.Sum(_ => _.ActualBilledQuantity);
        //                }

        //                BiddingDetailoutput.SaudaDetail.AvailableSaudaLimit = (userdivContext.SaudaLimit ?? 0) + invoiceQuantity - existingSaudaQuantity;
        //            }
        //            var BiddingWindowContext = _emamiContext.BiddingWindow.AsNoTracking().FirstOrDefault(_ => _.Id == BiddingContext.BiddingWindowId);
        //            if (BiddingWindowContext != null)
        //            {
        //                long NoofOilTypes = BiddingWindowContext.BiddingWindowVolumeCapacity.Count();
        //                BiddingDetailoutput.SaudaDetail.TotalChances = NoofOilTypes * BiddingWindowContext.NoOfAttemptsForBidding;
        //                BiddingDetailoutput.SaudaDetail.ChancesLeft = (NoofOilTypes * BiddingWindowContext.NoOfAttemptsForBidding) - _emamiContext.SaudaBiddingCart.AsNoTracking().Where(_ => _.BiddingWindowId == BiddingContext.BiddingWindowId && _.DealerId == BiddingContext.DealerId).Count();
        //            }

        //            var BiddingCartDetails = _emamiContext.SaudaBiddingCart.AsNoTracking().Where(_ => _.SaudaBiddingCartHeaderId == BiddingContext.Id).ToList();
        //            if (BiddingCartDetails != null && BiddingCartDetails.Any())
        //            {
        //                var oilTypeIds = BiddingCartDetails.Select(s => s.OilTypeId).Distinct().ToList();
        //                var skuIds = BiddingCartDetails.Select(s => s.SkuId).Distinct().ToList();
        //                var skuDatas = _emamiContext.Skus.AsNoTracking().Where(w => skuIds.Contains(w.Id))
        //                     .Select(s => new { Id = s.Id, SkuName = s.SkuName, PackGroupId = s.PackGroupId }).ToList();
        //                var packGroupIds = skuDatas.Select(s => s.PackGroupId).Distinct().ToList();
        //                var saudaQuantityDatas = _emamiContext.SaudaQuantityConfiguration.AsNoTracking()
        //                    .Where(w => oilTypeIds.Contains(w.OilTypeId) && packGroupIds.Contains(w.PackGroupId) && w.IsActive)
        //                    .Select(s => new { OilTypeId = s.OilTypeId, PackGroupId = s.PackGroupId, MaximumPercentage = s.MaximumPercentageQtyIncrease }).ToList();

        //                foreach (var item in BiddingCartDetails)
        //                {
        //                    #region Discount With Tax



        //                    decimal taxPaidValue = 0;
        //                    decimal discountWithTax = 0;
        //                    decimal discountTaxAmount = 0;
        //                    decimal discountGstPercentage = 0;
        //                    decimal bidPricePerCase = 0;
        //                    var raTotalDiscount = item.VolumeDiscountCase +
        //                       item.SchemeDiscountCase +
        //                       item.SkuDiscountCase;
        //                    //(order.GPBenefitType == (int)DTO.Enums.BenefitType.NONSAP ? order.GPBenefitDiscountInCase : 0) +
        //                    //(order.SurpriseBenefitType == (int)DTO.Enums.BenefitType.NONSAP ? order.SurpriseBenefitDiscountInCase : 0);
        //                    var pricingData = _emamiContext.Pricing.AsNoTracking().Where(_ => _.Id == item.PricingId).Select(s => new
        //                    {
        //                        //PlantGSTPercentage = s.PlantGSTPercentage,
        //                        //DepotGSTPercentage = s.DepotGSTPercentage
        //                    }).FirstOrDefault();


        //                    bidPricePerCase = (item.BidPrice / item.BidQuantityInCase);
        //                    if (item.IncotermId == (long)DTO.Enums.IncoTerms.ExPlant || item.IncotermId == (long)DTO.Enums.IncoTerms.ForPlant)
        //                    {
        //                        //discountGstPercentage = Utility.GetGstAmount(1, pricingData.PlantGSTPercentage);
        //                        discountWithTax = raTotalDiscount * discountGstPercentage;
        //                        discountTaxAmount = discountWithTax - raTotalDiscount;
        //                        taxPaidValue = Utility.DecimalFormatTwo(bidPricePerCase);// - discountTaxAmount;
        //                    }
        //                    else if (item.IncotermId == (long)DTO.Enums.IncoTerms.ExDepot || item.IncotermId == (long)DTO.Enums.IncoTerms.ForDepot)
        //                    {
        //                        //discountGstPercentage = Utility.GetGstAmount(1, pricingData.DepotGSTPercentage);
        //                        discountWithTax = raTotalDiscount * discountGstPercentage;
        //                        discountTaxAmount = discountWithTax - raTotalDiscount;
        //                        taxPaidValue = Utility.DecimalFormatTwo(bidPricePerCase);// - discountTaxAmount;
        //                    }



        //                    #endregion



        //                    var skuData = skuDatas.FirstOrDefault(_ => _.Id == item.SkuId);
        //                    var dto = new SKUDetail()
        //                    {
        //                        BiddingCartId = item.Id,
        //                        SkuId = item.SkuId,
        //                        OilTypeId = item.OilTypeId,
        //                        OilType = item.OilType.Name,
        //                        IncotermId = item.IncotermId,
        //                        PlantId = item.PlantId,
        //                        DepotId = item.DepotId,
        //                        SkuName = skuData.SkuName,
        //                        GuaranteePrice = item.GuarateedPricePerCase,
        //                        IncotermName = item.Incoterm.Name,
        //                        PlantName = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == item.PlantId).Name,
        //                        DepotName = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == item.DepotId).Name,
        //                        BidQuantityInCase = item.BidQuantityInCase,
        //                        BidQuantityMT = item.BidQuantityInMT,
        //                        AvailableBidQuantityForOilType = item.BiddingWindow.BiddingWindowVolumeCapacity.FirstOrDefault(_ => _.OilTypeId == item.OilTypeId).VolumeCapacity,
        //                        BidPricePerCase = item.BidPricePerCase,
        //                        BidPricePerCaseWithoutTax = taxPaidValue,
        //                        GuarateedPricePerCase = item.GuarateedPricePerCase,
        //                        CaseToMTValue = _resultService.ConvertCasetoMetricTon(1, item.SkuId),
        //                        SkuDiscount = item.SkuDiscountCase, //SkuDiscountUsers(item.SkuId, item.DealerId, item.BiddingDateAndTime),
        //                        AppliedVolumeDiscount = item.VolumeDiscountCase, //item.VolumeDiscount
        //                        SchemeDiscount = item.SchemeDiscountCase,  //SchemeDiscountUsers(item.SkuId, item.DealerId, item.BiddingDateAndTime),
        //                        //ChancesLeft = item.BiddingWindow.NoOfAttemptsForBidding - _emamiContext.SaudaBiddingCart.AsNoTracking().Where(_ => _.BiddingWindowId == BiddingContext.BiddingWindowId && _.DealerId == BiddingContext.DealerId && _.OilTypeId == item.OilTypeId).Count(),
        //                        //TotalChances = item.BiddingWindow.NoOfAttemptsForBidding,
        //                        //StatusId = item.StatusId,
        //                        //Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == item.StatusId).Name,
        //                        //FreightRouteId = item.Dealer.FreightRouteId ?? 0,
        //                        //FreightRouteName = item.Dealer.FreightRoute.Name,
        //                        ChancesLeft = SkuChancesLeft(item.BiddingWindow.Id, item.OilTypeId, item.DealerId, item.SkuId),
        //                        TotalChances = SkuTotalChance(item.BiddingWindow.Id),
        //                        //VolumeDiscount = VolumeDiscountUsers(item.SkuId, item.DealerId, item.BiddingDateAndTime, BiddingContext.Dealer.CityId),
        //                        IsSaudaAllocated = item.IsSaudaAllocated,
        //                    };



        //                    dto.StatusId = item.StatusId;
        //                    dto.MinimumQuantity = item.BaseBidQuantityInCase;
        //                    if (item.CounterBidStatusId == (int)DTO.Enums.Status.Pending && dto.StatusId == (int)DTO.Enums.Status.Pending)
        //                    {
        //                        dto.IsCounterBidSku = true;
        //                        dto.Status = Constants.InCounterBidOffer;
        //                    }
        //                    else
        //                    {
        //                        dto.Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == item.StatusId).Name;
        //                    }
        //                    if (saudaQuantityDatas.IsAny())
        //                    {
        //                        var saudaQuantity = saudaQuantityDatas.FirstOrDefault(f => f.OilTypeId == item.OilTypeId && f.PackGroupId == skuData.PackGroupId);
        //                        if (saudaQuantity != null)
        //                        {
        //                            dto.MaximumQuantity = Utility.PercentageCalculation(saudaQuantity.MaximumPercentage, item.BaseBidQuantityInCase);
        //                        }
        //                        else
        //                        {
        //                            dto.MaximumQuantity = item.BaseBidQuantityInCase;
        //                        }
        //                    }
        //                    BiddingDetailoutput.SKUDetail.Add(dto);
        //                }
        //            }
        //        }
        //        return _resultService.SuccessObject(BiddingDetailoutput);
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        ////public ResultDto EditSaudaBiddingQuantity(SaudaBiddingQuantityEditInputDto inputDto)
        ////{
        ////    try
        ////    {
        ////        if (inputDto == null)
        ////        {
        ////            return _resultService.ErrorMessage(Constants.InvalidRequest);
        ////        }
        ////        if (inputDto.SaudaBiddingQuantity == null)
        ////        {
        ////            return _resultService.ErrorMessage(Constants.InvalidRequest);
        ////        }

        ////        List<BiddingWindowDashboardChartVolumeCapacityDto> result = new List<BiddingWindowDashboardChartVolumeCapacityDto>();
        ////        var mtValidationMessage = "";
        ////        using (SqlConnection connection = new SqlConnection(Config.DBConnectionString))
        ////        {
        ////            try
        ////            {
        ////                connection.Open();
        ////                result = connection.Query<BiddingWindowDashboardChartVolumeCapacityDto>("GetBiddingWindowTotalAndRemaining", new
        ////                {
        ////                    BiddingWindowId = inputDto.BiddingWindowId
        ////                }, commandType: CommandType.StoredProcedure).ToList();
        ////            }
        ////            catch (Exception exception)
        ////            {
        ////                var message = $"{ServiceName} Controller-Method {_methodName} Exception: {exception}";
        ////                _logger.Error(message);
        ////            }
        ////            finally
        ////            {
        ////                connection.Close();
        ////            }
        ////        }
        ////        var OilTypes = _emamiContext.OilTypes.AsNoTracking().ToList();
        ////        foreach (var data in inputDto.SaudaBiddingQuantity)
        ////        {
        ////            var inputBidQuantityInMT = _resultService.ConvertCasetoMetricTon(data.Quantity, data.SkuId);
        ////            var validatemessage = "";
        ////            var detailsAgainstOilType = result.FirstOrDefault(_ => _.OilTypeId == data.OilTypeId);
        ////            var OilTypeName = OilTypes.FirstOrDefault(_ => _.Id == data.OilTypeId).Name;
        ////            if (detailsAgainstOilType.BookedVolumeCapacity == 0 && detailsAgainstOilType.RemainingVolumeCapacity == 0)
        ////            {
        ////                if (inputBidQuantityInMT > detailsAgainstOilType.TotalVolumeCapacity)
        ////                {
        ////                    validatemessage = "For OilType " + OilTypeName + " available quantity is " + detailsAgainstOilType.TotalVolumeCapacity + "," + Environment.NewLine;
        ////                }
        ////            }
        ////            else if (detailsAgainstOilType.RemainingVolumeCapacity == 0 || inputBidQuantityInMT > detailsAgainstOilType.RemainingVolumeCapacity)
        ////            {
        ////                validatemessage = "For OilType " + OilTypeName + " available quantity is " + detailsAgainstOilType.RemainingVolumeCapacity + "," + Environment.NewLine;
        ////            }
        ////            else
        ////            {
        ////                validatemessage = "";
        ////            }
        ////            mtValidationMessage = mtValidationMessage + validatemessage;
        ////        }

        ////        if (!String.IsNullOrEmpty(mtValidationMessage))
        ////        {
        ////            return _resultService.ErrorMessage(mtValidationMessage);
        ////        }

        ////        var biddingWindows = _emamiContext.BiddingWindow.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.BiddingWindowId);
        ////        if (biddingWindows != null && biddingWindows.StatusId != (int)DTO.Enums.BiddWindowStatus.Processing)
        ////        {
        ////            var windowErrorMessage = Constants.BiddingWindowStatusChanged + Utility.GetEnumFromString<DTO.Enums.BiddWindowStatus>(biddingWindows.StatusId);
        ////            return _resultService.ErrorMessage(windowErrorMessage);
        ////        }

        ////        var errorMessage = string.Empty; bool isError = false;
        ////        if (inputDto.SaudaBiddingQuantity.Any())
        ////        {
        ////            foreach (var item in inputDto.SaudaBiddingQuantity)
        ////            {
        ////                if (item.Id > 0)
        ////                {
        ////                    var BiddingCartContext = _emamiContext.SaudaBiddingCart.FirstOrDefault(_ => _.Id == item.Id && _.OilTypeId == item.OilTypeId && _.SkuId == item.SkuId);
        ////                    if (BiddingCartContext != null)
        ////                    {
        ////                        BiddingCartContext.BidQuantityInCase = item.Quantity;
        ////                        BiddingCartContext.BidQuantityInMT = _resultService.ConvertCasetoMetricTon(item.Quantity, item.SkuId);
        ////                        BiddingCartContext.TotalPrice = item.Quantity * BiddingCartContext.BidPricePerCase;

        ////                        BiddingCartContext.SkuDiscount = item.Quantity * BiddingCartContext.SkuDiscountCase;
        ////                        BiddingCartContext.SchemeDiscount = item.Quantity * BiddingCartContext.SchemeDiscountCase;

        ////                        BiddingCartContext.VolumeDiscount = item.Quantity * item.VolumeDiscountCal; //item.VolumeDiscountCal
        ////                        BiddingCartContext.VolumeDiscountCase = item.VolumeDiscountCal; //item.VolumeDiscountCal

        ////                        if (BiddingCartContext.GPBenefitType == (int)DTO.Enums.BenefitType.NONSAP)
        ////                            BiddingCartContext.GPBenefitDiscountOrDay = item.Quantity * BiddingCartContext.GPBenefitDiscountInCase;

        ////                        var bidPriceInSaudaBiddingCart = BiddingCartContext.TotalPrice - (BiddingCartContext.SkuDiscount + BiddingCartContext.SchemeDiscount + BiddingCartContext.VolumeDiscount + BiddingCartContext.GPBenefitDiscountOrDay);
        ////                        BiddingCartContext.BidPrice = bidPriceInSaudaBiddingCart;
        ////                        BiddingCartContext.ModifiedBy = inputDto.LoginUserId;
        ////                        BiddingCartContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        ////                        _emamiContext.SaveChanges();

        ////                        var SaudaContext = _emamiContext.Sauda.FirstOrDefault(_ => _.RABookingId == BiddingCartContext.SaudaBiddingCartHeaderId);
        ////                        if (SaudaContext != null)
        ////                        {
        ////                            var SaudaOrdersContext = _emamiContext.SaudaOrders.FirstOrDefault(_ => _.SaudaId == SaudaContext.Id && _.OilTypeId == item.OilTypeId && _.SkuId == item.SkuId);
        ////                            if (SaudaOrdersContext != null)
        ////                            {
        ////                                var quotedPrice = item.Quantity * BiddingCartContext.BidPricePerCase;
        ////                                SaudaOrdersContext.BidQuantityCase = item.Quantity;
        ////                                SaudaOrdersContext.BidQuantity = _resultService.ConvertCasetoMetricTon(item.Quantity, item.SkuId);
        ////                                SaudaOrdersContext.QuotedPrice = quotedPrice;
        ////                                SaudaOrdersContext.BidQuantityCaseForDailyReport = item.Quantity;
        ////                                SaudaOrdersContext.BidQuantityForDailyReport = _resultService.ConvertCasetoMetricTon(item.Quantity, item.SkuId);
        ////                                SaudaOrdersContext.QuotedPriceForDailyReport = quotedPrice;

        ////                                var skuDiscount = item.Quantity * SaudaOrdersContext.SkuDiscountCase;
        ////                                var schmeDiscount = item.Quantity * SaudaOrdersContext.SchemeDiscountCase;

        ////                                var volumeDiscount = item.Quantity * item.VolumeDiscountCal;  //item.VolumeDiscountCal
        ////                                decimal gpDiscount = 0;

        ////                                if (SaudaOrdersContext.GPBenefitType == (int)DTO.Enums.BenefitType.NONSAP)
        ////                                    gpDiscount = item.Quantity * SaudaOrdersContext.GPBenefitDiscountInCase;

        ////                                var bidPrice = quotedPrice - (skuDiscount + schmeDiscount + volumeDiscount + gpDiscount);
        ////                                SaudaOrdersContext.BidPrice = bidPrice;

        ////                                SaudaOrdersContext.SkuDiscount = skuDiscount;
        ////                                SaudaOrdersContext.SchemeDiscount = schmeDiscount;
        ////                                SaudaOrdersContext.VolumeDiscount = volumeDiscount;
        ////                                SaudaOrdersContext.VolumeDiscountCase = item.VolumeDiscountCal;
        ////                                SaudaOrdersContext.GPBenefitDiscountOrDay = gpDiscount;
        ////                                SaudaOrdersContext.SkuDiscountForDailyReport = skuDiscount;
        ////                                SaudaOrdersContext.SchemeDiscountForDailyReport = schmeDiscount;
        ////                                SaudaOrdersContext.VolumeDiscountForDailyReport = volumeDiscount;
        ////                                SaudaOrdersContext.VolumeDiscountCaseForDailyReport = item.VolumeDiscountCal;
        ////                                SaudaOrdersContext.GPBenefitDiscountOrDayForDailyReport = gpDiscount;

        ////                                SaudaOrdersContext.ModifiedBy = inputDto.LoginUserId;
        ////                                SaudaOrdersContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        ////                                _emamiContext.SaveChanges();
        ////                            }
        ////                        }
        ////                    }
        ////                    else
        ////                    {
        ////                        isError = true;
        ////                        var message = Constants.BiddingCartIsMissing + "OilType Id : " + item.OilTypeId + ",Sku Id : " + item.SkuId;
        ////                        errorMessage = Constants.BindErrorMessage(message + " - ", errorMessage);
        ////                    }
        ////                }
        ////                else
        ////                {
        ////                    isError = true;
        ////                    var message = Constants.BiddingCartIdIsMissing + "OilType Id : " + item.OilTypeId + ",Sku Id : " + item.SkuId;
        ////                    errorMessage = Constants.BindErrorMessage(message + " - ", errorMessage);
        ////                }
        ////            }
        ////        }
        ////        if (isError)
        ////        {
        ////            var resultDto = new ResultDto();
        ////            resultDto.IsSuccess = false;
        ////            resultDto.ErrorDto.Message = errorMessage;
        ////            return resultDto;
        ////        }
        ////        else
        ////        {
        ////            return _resultService.SuccessObject(Constants.QuantityUpdated);
        ////        }

        ////    }
        ////    catch (Exception exception)
        ////    {
        ////        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        ////        _logger.Error(message);
        ////        return _resultService.ErrorMessage(Constants.Exception);
        ////    }
        ////}

        //public void CounterBidNotification(CounterBiddingInputDto inputDto, int maxBid, int maxQty)
        //{
        //    try
        //    {
        //        DateTime currentDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);
        //        int noOfAttemptsForBidding = 0;
        //        int biddedAttempts = 0;
        //        int skuNoOfAttemptsForBidding = 0;
        //        bool isValidSkuForCounterBid = false;
        //        decimal cbjumpAmount = 0;
        //        decimal percentileCalculation = 0;
        //        decimal counterBidOffer = 0;
        //        decimal percentileAmount = 0;

        //        //var biddingWindow = _emamiContext.BiddingWindow.AsNoTracking()
        //        //    .FirstOrDefault(f => f.Id == inputDto.BiddingWindowId
        //        //    && f.StatusId == (int)DTO.Enums.BiddWindowStatus.Processing);

        //        var biddingWindow = _emamiContext.BiddingWindow.AsNoTracking()
        //            .FirstOrDefault(f => f.Id == inputDto.BiddingWindowId);
        //        var skudata = _emamiContext.Skus.AsNoTracking()
        //           .FirstOrDefault(f => f.Id == inputDto.SkuId);

        //        var counterBidJump = _emamiContext.CounterBidJump.AsNoTracking()
        //        .FirstOrDefault(f => f.IsActive && f.OilTypeId == skudata.OilTypeId && f.PackGroupId == skudata.PackGroupId
        //        && DbFunctions.TruncateTime(currentDateTime) >= DbFunctions.TruncateTime(f.ValidFrom)
        //        && DbFunctions.TruncateTime(currentDateTime) <= DbFunctions.TruncateTime(f.ValidTo));
        //        //CounterBidJump counterBidJump = new CounterBidJump() { CounterbidJump = 5 };
        //        if (counterBidJump == null)
        //        {
        //            //return _resultService.ErrorMessage(Constants.RecordNotFound);
        //            _logger.Debug($"{ServiceName} Service-Method {_methodName} Message: {Constants.RecordNotFound}");
        //        }

        //        if (biddingWindow != null)
        //        {
        //            noOfAttemptsForBidding = biddingWindow.NoOfAttemptsForBidding;

        //            var saudaBiddingCartHeaders = _emamiContext.SaudaBiddingCartHeaders.AsNoTracking()
        //                .Join(_emamiContext.SaudaBiddingCart.AsNoTracking(), sh => sh.Id, sc => sc.SaudaBiddingCartHeaderId, (sh, sc) => new { SaudaBiddingCartHeaders = sh, SaudaBiddingCart = sc })
        //                .Where(w => w.SaudaBiddingCart.BiddingWindowId == inputDto.BiddingWindowId && w.SaudaBiddingCart.DealerId == inputDto.DealerId && w.SaudaBiddingCart.SkuId == inputDto.SkuId)
        //                .Select(s => new
        //                {
        //                    Id = s.SaudaBiddingCart.Id,
        //                    BiddingWindowId = s.SaudaBiddingCart.BiddingWindowId,
        //                    DealerId = s.SaudaBiddingCart.DealerId,
        //                    OilTypeId = s.SaudaBiddingCart.OilTypeId,
        //                    SkuId = s.SaudaBiddingCart.SkuId,
        //                    SkuName = s.SaudaBiddingCart.Sku.SkuName,
        //                    StatusId = s.SaudaBiddingCart.StatusId,
        //                    BaseRate = s.SaudaBiddingCart.BaseRate
        //                }).GroupBy(g => g.SkuId).ToList();

        //            if (saudaBiddingCartHeaders.IsAny())
        //            {
        //                var userData = _emamiContext.Users.AsNoTracking().FirstOrDefault(f => f.Id == inputDto.LoginUserId);
        //                foreach (var bidding in saudaBiddingCartHeaders)
        //                {
        //                    biddedAttempts = bidding.Count();
        //                    if (noOfAttemptsForBidding == biddedAttempts)
        //                    {
        //                        if (bidding.IsAny())
        //                        {
        //                            var detail = bidding.OrderByDescending(o => o.Id).FirstOrDefault();
        //                            skuNoOfAttemptsForBidding = bidding.Count();
        //                            isValidSkuForCounterBid = bidding.Any(a => a.StatusId == (int)DTO.Enums.Status.Pending);

        //                            if (noOfAttemptsForBidding == skuNoOfAttemptsForBidding && isValidSkuForCounterBid)
        //                            {

        //                                //var amountList = _emamiContext.SaudaOrders.AsNoTracking()
        //                                //        .Where(w => w.SkuId == detail.SkuId
        //                                //        && w.StatusId == (int)DTO.Enums.Status.Pending
        //                                //        && w.BiddingwindowId == inputDto.BiddingWindowId
        //                                //        && w.Incoterms2 == inputDto.IncotermId)
        //                                //        .Select(s => s.BidPricePerCase - s.BaseRate).ToList();

        //                                var message = $"{ServiceName} Service-Method {_methodName} MaxQty: {maxQty}";
        //                                _logger.Info(message);

        //                                var biddingDetails = _emamiContext.SaudaOrders.AsNoTracking()
        //                                        .Where(w => w.SkuId == detail.SkuId
        //                                        && w.StatusId == (int)DTO.Enums.Status.Pending
        //                                        && w.BiddingwindowId == inputDto.BiddingWindowId
        //                                        && w.Incoterms2 == inputDto.IncotermId
        //                                        && w.BidQuantityCase > maxQty)
        //                                        .Select(s => new WeightedAverageDto() { Weight = s.BidQuantityCase, Price = s.BidPricePerCase })
        //                                        .Take(maxBid).ToList();

        //                                //var amountList = saudaBiddingCartDatas.Select(s => s.BidPricePerCase).ToList();

        //                                #region Counter Bid Offer

        //                                //var percentile = _emamiContext.PercentileNumber.AsNoTracking()
        //                                //    .Join(_emamiContext.PercentileNumberDetails.AsNoTracking(),
        //                                //    a => a.Id, b => b.PercentileNumberId, (a, b) => new { a, b })
        //                                //    .FirstOrDefault(f => f.b.OilTypeId == detail.OilTypeId
        //                                //    && f.a.IsActive
        //                                //    && DbFunctions.TruncateTime(currentDateTime) >= DbFunctions.TruncateTime(f.a.ValidFrom)
        //                                //    && DbFunctions.TruncateTime(currentDateTime) <= DbFunctions.TruncateTime(f.a.ValidTo));

        //                                //if (percentile != null)
        //                                //{
        //                                cbjumpAmount = detail.BaseRate + counterBidJump.CounterbidJump;

        //                                message = $"{ServiceName} Service-Method {_methodName} First CbjumpAmount: {cbjumpAmount}";
        //                                _logger.Info(message);

        //                                percentileCalculation = Utility.WeightedAverage(biddingDetails);
        //                                if (percentileCalculation > 0)
        //                                {
        //                                    message = $"{ServiceName} Service-Method {_methodName} Second WeightedAverage: BaseRate {detail.BaseRate} WeitageCalculation {percentileCalculation} {percentileCalculation - detail.BaseRate}";
        //                                    _logger.Info(message);
        //                                    percentileAmount = detail.BaseRate + (percentileCalculation - detail.BaseRate);
        //                                }
        //                                else
        //                                {
        //                                    percentileAmount = detail.BaseRate;
        //                                }
        //                                counterBidOffer = Math.Max(cbjumpAmount, percentileAmount);

        //                                #endregion

        //                                var SaudaBiddingCart = _emamiContext.SaudaBiddingCart.FirstOrDefault(f => f.Id == detail.Id);
        //                                if (SaudaBiddingCart != null)
        //                                {
        //                                    #region Counter Bid History
        //                                    SaudaBiddingCart.CounterBidOffer = counterBidOffer;
        //                                    SaudaBiddingCart.CounterBidStatusId = (int)DTO.Enums.Status.Pending;

        //                                    CounterBidNotification notification = new CounterBidNotification()
        //                                    {
        //                                        SaudaBiddingCartId = detail.Id,
        //                                        BiddingWindowId = biddingWindow.Id,
        //                                        DealerId = SaudaBiddingCart.DealerId,
        //                                        SkuId = SaudaBiddingCart.SkuId,
        //                                        CounterBidOffer = counterBidOffer,
        //                                        StatusId = (int)DTO.Enums.Status.Pending,
        //                                        CreatedBy = SaudaBiddingCart.CreatedBy,
        //                                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow)
        //                                    };
        //                                    _emamiContext.CounterBidNotifications.Add(notification);
        //                                    _emamiContext.SaveChanges();

        //                                    _logger.Info($"{ServiceName} Service-Method {_methodName} Message: CounterBidSaved");

        //                                    #endregion


        //                                    string emailSubject = string.Empty;
        //                                    emailSubject = Constants.CounterBidOffer;
        //                                    EmailTemplate smsTemplate = new EmailTemplate();
        //                                    var smsPlainTemplate = string.Empty;
        //                                    smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.CounterBidOfferNotificationSMS);

        //                                    smsPlainTemplate = smsTemplate.PlainTemplate
        //                                        .Replace(Constants.BiddingWindowName, biddingWindow.Name)
        //                                        .Replace(Constants.SkuName, detail.SkuName)
        //                                        .Replace(Constants.CounterBidOfferPrice, (Math.Round(counterBidOffer, 2)).ToString());
        //                                    var smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);

        //                                    AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
        //                                    amazonNotificationService.SendMessage(smsMessage, inputDto.DealerMobileNumber);

        //                                    PushNotificationInputDto pushNotificationInputDto = new PushNotificationInputDto
        //                                    {
        //                                        Id = detail.Id.ToString(),
        //                                        PushTokenKey = userData.PushTokenKey,
        //                                        RegistrationTypeId = userData.RegistrationTypeId != null ? (int)userData.RegistrationTypeId : 0,
        //                                        Title = emailSubject,
        //                                        Message = smsMessage
        //                                    };
        //                                    _notificationService.SendPushNotificationThroughFirebase(pushNotificationInputDto);
        //                                }
        //                                //}
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //return _resultService.SuccessMessage(Constants.NoBiddingWindows);
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //    }
        //}

        //public ResultDto GetCounterBidSaudaDetails(SaudaBiddingQuantityEditInputDto inputDto)
        //{
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }
        //        if (inputDto.SaudaBiddingQuantity == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }

        //        if (inputDto.SaudaBiddingQuantity.Any())
        //        {
        //            foreach (var item in inputDto.SaudaBiddingQuantity)
        //            {
        //                var BiddingCartContext = _emamiContext.SaudaBiddingCart.FirstOrDefault(_ => _.Id == item.Id && _.OilTypeId == item.OilTypeId && _.SkuId == item.SkuId);
        //                if (BiddingCartContext != null)
        //                {
        //                    BiddingCartContext.BidQuantityInCase = item.Quantity;
        //                    BiddingCartContext.BidQuantityInMT = _resultService.ConvertCasetoMetricTon(item.Quantity, item.SkuId);
        //                    BiddingCartContext.TotalPrice = item.Quantity * BiddingCartContext.BidPricePerCase;
        //                    BiddingCartContext.ModifiedBy = inputDto.LoginUserId;
        //                    BiddingCartContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //                    _emamiContext.SaveChanges();
        //                }

        //                var SaudaContext = _emamiContext.Sauda.FirstOrDefault(_ => _.RABookingId == BiddingCartContext.SaudaBiddingCartHeaderId);
        //                if (SaudaContext != null)
        //                {
        //                    var SaudaOrdersContext = _emamiContext.SaudaOrders.FirstOrDefault(_ => _.SaudaId == SaudaContext.Id && _.OilTypeId == item.OilTypeId && _.SkuId == item.SkuId);
        //                    if (SaudaOrdersContext != null)
        //                    {
        //                        SaudaOrdersContext.BidQuantityCase = item.Quantity;
        //                        SaudaOrdersContext.BidQuantity = _resultService.ConvertCasetoMetricTon(item.Quantity, item.SkuId);
        //                        SaudaOrdersContext.QuotedPrice = item.Quantity * BiddingCartContext.BidPricePerCase;
        //                        SaudaOrdersContext.ModifiedBy = inputDto.LoginUserId;
        //                        SaudaOrdersContext.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //                        _emamiContext.SaveChanges();
        //                    }
        //                }
        //            }
        //        }
        //        return _resultService.SuccessObject(Constants.QuantityUpdated);
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        //public ResultDto SaudaCounterbitStatusUpdate(SaudaCounterBidOfferStatusUpdate inputDto)
        //{
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }

        //        int i = 0;
        //        var saudaBiddingCart = _emamiContext.SaudaBiddingCart.FirstOrDefault(_ => _.Id == inputDto.Id);
        //        var biddingWindows = _emamiContext.BiddingWindow.AsNoTracking().FirstOrDefault(_ => _.Id == saudaBiddingCart.BiddingWindowId);
        //        if (biddingWindows != null && biddingWindows.StatusId != (int)DTO.Enums.BiddWindowStatus.Processing)
        //        {
        //            var errorMessage = Constants.BiddingWindowStatusChanged + Utility.GetEnumFromString<DTO.Enums.BiddWindowStatus>(biddingWindows.StatusId);
        //            return _resultService.ErrorMessage(errorMessage);
        //        }

        //        long DealerTypeId = 0;
        //        long BrokerId = 0;
        //        var counterBidNotification = _emamiContext.CounterBidNotifications.FirstOrDefault(_ => _.SaudaBiddingCartId == inputDto.Id);
        //        var dealerRole = _emamiContext.UserRoles.FirstOrDefault(_ => _.UserId == saudaBiddingCart.DealerId);
        //        DealerTypeId = dealerRole.RoleId == (int)DTO.Enums.Role.Broker ? (int)DTO.Enums.DealerType.Broker : (int)DTO.Enums.DealerType.Direct;

        //        if (saudaBiddingCart != null)
        //        {
        //            var cartTotalDiscount = saudaBiddingCart.SkuDiscount + saudaBiddingCart.SchemeDiscount + saudaBiddingCart.VolumeDiscount + saudaBiddingCart.GPBenefitDiscountOrDay;
        //            var cartQuotedPrice = saudaBiddingCart.BidQuantityInCase * saudaBiddingCart.CounterBidOffer;
        //            var cartBidPriceGrandTotal = cartQuotedPrice - cartTotalDiscount;
        //            saudaBiddingCart.BidPrice = cartBidPriceGrandTotal;
        //            saudaBiddingCart.BidPricePerCase = saudaBiddingCart.CounterBidOffer;
        //            saudaBiddingCart.StatusId = inputDto.StatusId;
        //            saudaBiddingCart.CounterBidStatusId = inputDto.StatusId;
        //            saudaBiddingCart.ModifiedBy = inputDto.LoginUserId;
        //            saudaBiddingCart.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);

        //            if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
        //            {
        //                var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaBiddingCart.DealerId);

        //                if (dealerRole != null)
        //                {
        //                    DealerTypeId = dealerRole.RoleId == (int)DTO.Enums.Role.Broker ? (int)DTO.Enums.DealerType.Broker : (int)DTO.Enums.DealerType.Direct;
        //                    if (dealerRole.RoleId == (int)DTO.Enums.Role.Broker)
        //                    {
        //                        BrokerId = saudaBiddingCart.DealerId;
        //                    }
        //                    else
        //                    {
        //                        var BrokerContext = (from ucm in _emamiContext.UserCustomerMapping
        //                                             join ur in _emamiContext.UserRoles on ucm.UserId equals ur.UserId
        //                                             where ur.RoleId == (int)DTO.Enums.Role.Broker
        //                                             && ucm.CustomerId == saudaBiddingCart.DealerId
        //                                             select new
        //                                             {
        //                                                 BrokerId = ucm.UserId
        //                                             }).FirstOrDefault();

        //                        if (BrokerContext != null)
        //                        {
        //                            BrokerId = BrokerContext.BrokerId;
        //                        }
        //                    }
        //                }

        //                var saudaContext = new Sauda
        //                {
        //                    BiddingDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        //                    UserId = saudaBiddingCart.DealerId,
        //                    //SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.ReverseAuction,
        //                    CreatedBy = inputDto.LoginUserId,
        //                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        //                    IsSAPDataSync = false,
        //                    IsSAPDataSyncApproval = false,
        //                    RABookingId = saudaBiddingCart.SaudaBiddingCartHeaderId
        //                };
        //                _emamiContext.Sauda.Add(saudaContext);
        //                _emamiContext.SaveChanges();

        //                DateTime? saudaValidFromDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //                long? depotIdForRake = 0;
        //                if (saudaBiddingCart.IncotermId == (int)DTO.Enums.IncoTerms.ExRake || saudaBiddingCart.IncotermId == (int)DTO.Enums.IncoTerms.ExRake)
        //                {
        //                    depotIdForRake = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == saudaBiddingCart.PlantId && !_.IsPlant)?.DepotId;
        //                }

        //                var IncotermContext = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == saudaBiddingCart.IncotermId).Name;
        //                var IncotermsType = IncotermContext.ToLower().Contains("for") ? "For" : "Ex";

        //                if (inputDto.StatusId == (int)DTO.Enums.Status.Approved)
        //                {
        //                    DateTime currentdate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //                    var totalDiscount = saudaBiddingCart.SkuDiscount + saudaBiddingCart.SchemeDiscount + saudaBiddingCart.VolumeDiscount + saudaBiddingCart.GPBenefitDiscountOrDay;
        //                    var quotedPrice = saudaBiddingCart.BidQuantityInCase * saudaBiddingCart.BidPricePerCase;
        //                    var bidPriceGrandTotal = quotedPrice - totalDiscount;
        //                    var saudaValidityPeriod = Convert.ToDouble(dealerContext.SaudaValidityPeriod > 0 ? dealerContext.SaudaValidityPeriod : Config.DefaultSaudaValidity);

        //                    i = i + 10;
        //                    var saudaOrder = new SaudaOrder
        //                    {
        //                        SaudaId = saudaContext.Id,
        //                        SaudaNumber = i.ToString(),
        //                        SkuId = saudaBiddingCart.SkuId,
        //                        OilTypeId = saudaBiddingCart.OilTypeId,
        //                        BidPriceBeforeDiscount = saudaBiddingCart.BidPricePerCase,
        //                        BidPrice = bidPriceGrandTotal,
        //                        BidPricePerCase = saudaBiddingCart.BidPricePerCase,
        //                        DiscountTypeId = 0,
        //                        DiscountAmount = 0,
        //                        BidQuantity = _resultService.ConvertCasetoMetricTon(saudaBiddingCart.BidQuantityInCase, saudaBiddingCart.SkuId),
        //                        BidQuantityCase = saudaBiddingCart.BidQuantityInCase,
        //                        QuotedPrice = quotedPrice,
        //                        CreatedBy = inputDto.LoginUserId,
        //                        CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        //                        BiddingwindowId = saudaBiddingCart.BiddingWindowId,
        //                        //SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.ReverseAuction,
        //                        PricingId = saudaBiddingCart.PricingId,
        //                        DealerTypeId = DealerTypeId,
        //                        Incoterms1 = IncotermsType,
        //                        PlantId = saudaBiddingCart.PlantId,
        //                        //DealerLocationId = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaBiddingCart.DealerId).FreightRouteId ?? 0,
        //                        CustomerPONumber = dealerContext.Code + DateHelper.UtcToIndia(DateTime.UtcNow).ToShortDateString(),
        //                        ValidFromDate = saudaBiddingCart.ValidFromDate,
        //                        ValidToDate = saudaBiddingCart.ValidToDate,
        //                        StatusId = (int)DTO.Enums.Status.Pending,
        //                        SaudaStatusId = (int)DTO.Enums.SaudaStatus.NotReleased,
        //                        Incoterms2 = saudaBiddingCart.IncotermId,
        //                        BrokerId = BrokerId,
        //                        IsSAPDataSync = false,
        //                        IsSAPDataSyncApproval = false,
        //                        DepotIdForRake = depotIdForRake.Value,
        //                        CounterBidOffer = saudaBiddingCart.BidPricePerCase,
        //                        CounterBidOfferDate = DateHelper.UtcToIndia(DateTime.UtcNow),

        //                        SkuDiscount = saudaBiddingCart.SkuDiscount,
        //                        SkuDiscountCase = saudaBiddingCart.SkuDiscountCase,
        //                        SkuDiscountType = saudaBiddingCart.SkuDiscountType,

        //                        SchemeDiscount = saudaBiddingCart.SchemeDiscount,
        //                        SchemeDiscountCase = saudaBiddingCart.SchemeDiscountCase,
        //                        SchemeDiscountType = saudaBiddingCart.SchemeDiscountType,

        //                        VolumeDiscount = saudaBiddingCart.VolumeDiscount,
        //                        VolumeDiscountCase = saudaBiddingCart.VolumeDiscountCase,
        //                        VolumeDiscountType = saudaBiddingCart.VolumeDiscountType,

        //                        //GP BENEFITS
        //                        GPBenefitType = saudaBiddingCart.GPBenefitType,
        //                        GPBenefitAppliedTypeId = saudaBiddingCart.GPBenefitAppliedTypeId,
        //                        GPBenefitOrCategoryId = saudaBiddingCart.GPBenefitOrCategoryId,
        //                        GPBenefitDiscountOrDay = saudaBiddingCart.GPBenefitDiscountOrDay,
        //                        GPBenefitDiscountInCase = saudaBiddingCart.GPBenefitDiscountInCase,
        //                        BaseRate = saudaBiddingCart.BaseRate,
        //                        IsBaseSauda = true
        //                    };
        //                    _emamiContext.SaudaOrders.Add(saudaOrder);
        //                    _emamiContext.SaveChanges();

        //                    List<SaudaCreateNotificationDto> saudaCreateEmailList = new List<SaudaCreateNotificationDto>()
        //                    {
        //                        new SaudaCreateNotificationDto() {
        //                        StatusId = Convert.ToInt32(inputDto.StatusId),
        //                        SaudaOrderId = saudaOrder.Id,
        //                        SaudaBookingTypeId = saudaOrder.SaudaBookingTypeId,
        //                        LoginUserId = inputDto.LoginUserId,
        //                        SkuName = saudaBiddingCart.Sku.SkuName,
        //                        BidQuantityInCase = saudaBiddingCart.BidQuantityInCase,
        //                        BidPrice = bidPriceGrandTotal,
        //                        WindowName = biddingWindows.Name
        //                        }
        //                    };

        //                    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => RASaudaCreateNotificationAsync(saudaCreateEmailList, inputDto.LoginUserId, saudaBiddingCart.DealerId, (int)DTO.Enums.NotificationType.CounterBidoffer, cancellationToken));
        //                }
        //            }

        //            if (counterBidNotification != null)
        //            {
        //                counterBidNotification.StatusId = inputDto.StatusId;
        //                counterBidNotification.ModifiedBy = inputDto.LoginUserId;
        //                counterBidNotification.ModifiedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //            }

        //            _emamiContext.SaveChanges();
        //            return _resultService.SuccessObject(Constants.SaudaCounterBitOffer);
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

        //public ResultDto GetCounterBidNotificationDetails(LoginUserIdDto inputDto)
        //{
        //    var notificationList = new List<NotificationDto>();
        //    var dealerIds = new List<long>();
        //    var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }

        //        var userData = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(f => f.UserId == inputDto.LoginUserId);

        //        if (userData.RoleId == (long)DTO.Enums.Role.StateTrader)
        //        {
        //            var userMappedCustomer = _emamiContext.UserCustomerMapping.AsNoTracking()
        //                .Where(w => w.UserId == inputDto.LoginUserId).Select(s => s.CustomerId).ToList();
        //            dealerIds = _emamiContext.CustomerGroupDetails.AsNoTracking()
        //                .Where(w => userMappedCustomer.Contains(w.CustomerId)).Select(s => s.CustomerId).ToList();
        //        }
        //        else if (userData.RoleId == (long)DTO.Enums.Role.Dealer)
        //        {
        //            dealerIds.Add(inputDto.LoginUserId);
        //        }

        //        var CounterBidNotificationDatas = _emamiContext.CounterBidNotifications.AsNoTracking()
        //            .Where(_ => dealerIds.Contains(_.DealerId)
        //            && DbFunctions.TruncateTime(_.CreatedDate) == DbFunctions.TruncateTime(currentDate)).ToList();

        //        if (CounterBidNotificationDatas.IsAny())
        //        {
        //            foreach (var notification in CounterBidNotificationDatas)
        //            {
        //                var biddingWindow = _emamiContext.BiddingWindow.AsNoTracking().FirstOrDefault(f => f.Id == notification.BiddingWindowId);
        //                //if (biddingWindow.StatusId == (int)DTO.Enums.BiddWindowStatus.Processing)
        //                //{
        //                var smsTemplate = _emamiContext.EmailTemplate.AsNoTracking().FirstOrDefault(email => email.Name == Constants.CounterBidOfferNotificationSMS);

        //                //smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.ContractQty, actualLimit.ToString()).Replace(Constants.Quantity, extendedLimit.ToString()).Replace(Constants.CustomerName, dealer.Name);
        //                //smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);
        //                var skuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(f => f.Id == notification.SkuId).SkuName;
        //                var smsPlainTemplate = smsTemplate.PlainTemplate.Replace(Constants.SkuName, skuName)
        //                     .Replace(Constants.CounterBidOfferPrice, (Math.Round(notification.CounterBidOffer, 2)).ToString());
        //                var smsMessage = smsTemplate.Template.Replace(Constants.ReplaceValueContent, smsPlainTemplate);

        //                notificationList.Add(new NotificationDto()
        //                {
        //                    Request = DTO.Enums.NotificationRequest.CounterBid.ToString(),
        //                    RequestId = (int)DTO.Enums.NotificationRequest.CounterBid,
        //                    Notification = smsMessage,
        //                    BiddingDate = notification.CreatedDate,
        //                    FromHour = null,
        //                    ToHour = null,
        //                    NotificationDateTime = notification.CreatedDate,
        //                    StatusId = notification.StatusId,
        //                    ReferenceId = notification.SaudaBiddingCartId,
        //                    SaudaId = notification.SaudaBiddingCartId
        //                });
        //                //}
        //            }

        //            return _resultService.SuccessMessageWitObject(notificationList, Constants.SuccessMessage);
        //        }
        //        else
        //        {
        //            return _resultService.ErrorMessageWitObject(notificationList, Constants.RecordNotFound);
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        //public ResultDto SaudaConversionFormulaForMobile(IdInputDto inputDto)
        //{
        //    var conversionFormulaList = new List<ConversionFormulaOutputDto>();
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }

        //        var ConversionFormulaList = _emamiContext.ConversionFormulas.AsNoTracking().Where(_ => _.OilTypeId == inputDto.Id && _.IsActive).ToList();
        //        if (ConversionFormulaList != null && ConversionFormulaList.Any())
        //        {
        //            foreach (var item in ConversionFormulaList)
        //            {
        //                var formuladetail = item.ConversionFormulaDetails.ToList();
        //                foreach (var formula in formuladetail)
        //                {
        //                    var conversionFormulaDto = new ConversionFormulaOutputDto()
        //                    {
        //                        OilType = item.OilType.Name + " " + item.PackGroup.Name,
        //                        DerivedSku = formula.Sku.SkuName,
        //                        Formula = formula.Formula.Replace("*", " Multiply By ").Replace("+", " Add ").Replace("-", " Subtract ").Replace("/", " Divided By ").Replace("BASESKU", item.Sku.SkuName)
        //                    };
        //                    conversionFormulaList.Add(conversionFormulaDto);
        //                }
        //            }
        //        }
        //        return _resultService.SuccessMessageWitObject(conversionFormulaList, Constants.SuccessMessage);

        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        //#endregion

        //#region Sauda Allocation

        //public ResultDto GetSaudaListForSaudaAllocationByUserId(SaudaFilterDto inputDto)
        //{
        //    _methodName = "GetSaudaListForSaudaAllocationByUserId";
        //    var saudaList = new List<SaudaListForAllocationDto>();
        //    decimal SKUWiseWeight = 0;
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }
        //        if (inputDto.DealerId == 0)
        //        {
        //            return _resultService.ErrorMessage(Constants.DealerMissing);
        //        }
        //        var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
        //        if (dealerContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.DealerNotFound);
        //        }


        //        var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //        var saudaOrderListContext = _emamiContext.Sauda.AsNoTracking()
        //            .Where(_ => _.UserId == inputDto.DealerId && DbFunctions.TruncateTime(_.BiddingDate) == DbFunctions.TruncateTime(currentDate))
        //            .Join(_emamiContext.SaudaOrders.AsNoTracking().Where(_ => !_.IsSaudaAllocated)
        //            .Where(_ => _.SaudaNumber == "" || _.SaudaNumber == null && _.StatusId == (int)DTO.Enums.Status.Pending)
        //            , s => s.Id, so => so.SaudaId, (s, so) => new { Sauda = s, SaudaOrder = so })
        //            .Where(_ => _.Sauda != null && _.SaudaOrder != null).ToList();

        //        if (saudaOrderListContext != null && saudaOrderListContext.Any())
        //        {
        //            var skuIds = saudaOrderListContext.Select(s => s.SaudaOrder.SkuId).Distinct().ToList();
        //            var SkuUomMappingDatas = _emamiContext.SkuUomMapping
        //                .Where(_ => skuIds.Contains(_.SkuId) && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)(int)DTO.Enums.Uom.Nos)
        //                .Select(s => new
        //                {
        //                    SkuId = s.SkuId,
        //                    UomId = s.UomId,
        //                    RelationUomId = s.RelationUomId,
        //                    ConversionFactor = s.ConversionFactor
        //                }).ToList();

        //            foreach (var itemContext in saudaOrderListContext)
        //            {
        //                var saudaListForAllocationDto = new SaudaListForAllocationDto();
        //                saudaListForAllocationDto.SaudaId = itemContext.Sauda != null ? itemContext.Sauda.Id : 0;
        //                saudaListForAllocationDto.SaudaOrderId = itemContext.SaudaOrder != null ? itemContext.SaudaOrder.Id : 0;
        //                saudaListForAllocationDto.SaudaNumber = itemContext.SaudaOrder != null ? itemContext.SaudaOrder.SaudaNumber : string.Empty;
        //                saudaListForAllocationDto.SkuId = itemContext.SaudaOrder != null ? itemContext.SaudaOrder.SkuId : 0;
        //                saudaListForAllocationDto.SkuName = itemContext.SaudaOrder != null && itemContext.SaudaOrder.Sku != null ? itemContext.SaudaOrder.Sku.SkuName : string.Empty;
        //                saudaListForAllocationDto.OilTypeId = itemContext.SaudaOrder != null ? itemContext.SaudaOrder.OilTypeId : 0;
        //                saudaListForAllocationDto.OilTypeName = itemContext.SaudaOrder != null && itemContext.SaudaOrder.OilType != null ? itemContext.SaudaOrder.OilType.Name : string.Empty;
        //                saudaListForAllocationDto.BidQuantity = itemContext.SaudaOrder != null ? itemContext.SaudaOrder.BidQuantity : 0;
        //                saudaListForAllocationDto.BidQuantityCase = itemContext.SaudaOrder != null ? itemContext.SaudaOrder.BidQuantityCase : 0;
        //                saudaListForAllocationDto.BiddingDate = itemContext.Sauda.BiddingDate;
        //                saudaListForAllocationDto.QuotedPrice = itemContext.SaudaOrder.QuotedPrice;
        //                saudaListForAllocationDto.DealerId = dealerContext.Id;
        //                saudaListForAllocationDto.DealerName = dealerContext?.Name;


        //                var biddingCartHeaderDetails = _emamiContext.SaudaBiddingCartHeaders.AsNoTracking().FirstOrDefault(_ => _.Id == itemContext.Sauda.RABookingId);
        //                if (biddingCartHeaderDetails != null)
        //                {
        //                    saudaListForAllocationDto.BiddingWindowId = biddingCartHeaderDetails.BiddingWindowId;
        //                    saudaListForAllocationDto.BiddingCartHeaderId = biddingCartHeaderDetails.Id;
        //                    saudaListForAllocationDto.StartTime = biddingCartHeaderDetails.BiddingWindow.StartTime;
        //                    saudaListForAllocationDto.EndTime = biddingCartHeaderDetails.BiddingWindow.EndTime;
        //                    saudaListForAllocationDto.StartEndTime = Utility.ConvertToTime(biddingCartHeaderDetails.BiddingWindow.StartTime) + " - " + Utility.ConvertToTime(biddingCartHeaderDetails.BiddingWindow.EndTime);
        //                    saudaListForAllocationDto.WindowStatus = Utility.GetEnumFromString<DTO.Enums.BiddWindowStatus>(biddingCartHeaderDetails.BiddingWindow.StatusId);
        //                    saudaListForAllocationDto.WindowStatusId = biddingCartHeaderDetails.BiddingWindow.StatusId;
        //                    saudaListForAllocationDto.ServerDateTime = DateHelper.UtcToIndia(DateTime.UtcNow);

        //                    var saStartTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 0, 0, 0, 0);
        //                    var saudaAllocationTime = _emamiContext.RaSaudaConfiguration.AsNoTracking().FirstOrDefault(f => f.IsActive).SaudaAllocationTime;
        //                    var saEndTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, saudaAllocationTime.Hours, saudaAllocationTime.Minutes, saudaAllocationTime.Seconds, saudaAllocationTime.Milliseconds);
        //                    saudaListForAllocationDto.SaudaAllocationStartTime = saStartTime;
        //                    saudaListForAllocationDto.SaudaAllocationEndTime = saEndTime;
        //                    //saudaListForAllocationDto.SaudaAllocationStartTime = biddingCartHeaderDetails.BiddingWindow.SaudaAllocationStartTime;
        //                    //saudaListForAllocationDto.SaudaAllocationEndTime = biddingCartHeaderDetails.BiddingWindow.SaudaAllocationEndTime;

        //                    saudaListForAllocationDto.SaudaAllocationStatusId = biddingCartHeaderDetails.BiddingWindow.SaudaAllocationStatusId;

        //                    var biddingCartDetails = _emamiContext.SaudaBiddingCart.AsNoTracking()
        //                        .FirstOrDefault(_ => _.SaudaBiddingCartHeaderId == biddingCartHeaderDetails.Id && _.SkuId == itemContext.SaudaOrder.SkuId);
        //                    if (biddingCartDetails != null)
        //                    {

        //                        #region SKU Weight

        //                        SKUWiseWeight = 0;
        //                        if (biddingCartDetails.Sku.Uom.Name == DTO.Enums.Uom.Ltr.ToString())
        //                        {
        //                            var SkuUomMappingContext = SkuUomMappingDatas.FirstOrDefault(_ => _.SkuId == biddingCartDetails.SkuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)(int)DTO.Enums.Uom.Nos);
        //                            if (SkuUomMappingContext != null)
        //                            {
        //                                SKUWiseWeight = biddingCartDetails.OilType.LitreConversion > 0 ? (biddingCartDetails.Sku.Quantity * 1000 * SkuUomMappingContext.ConversionFactor) / biddingCartDetails.OilType.LitreConversion : 0;
        //                            }
        //                            else
        //                            {
        //                                SKUWiseWeight = biddingCartDetails.OilType.LitreConversion > 0 ? (biddingCartDetails.Sku.Quantity * 1000) / biddingCartDetails.OilType.LitreConversion : 0;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            SKUWiseWeight = biddingCartDetails.Sku.Quantity;
        //                        }

        //                        #endregion

        //                        saudaListForAllocationDto.SKUDetail = new SKUDetail()
        //                        {
        //                            BiddingCartId = biddingCartDetails.Id,
        //                            SkuId = biddingCartDetails.SkuId,
        //                            OilTypeId = biddingCartDetails.OilTypeId,
        //                            OilType = biddingCartDetails.OilType.Name,
        //                            IncotermId = biddingCartDetails.IncotermId,
        //                            PlantId = biddingCartDetails.PlantId,
        //                            DepotId = biddingCartDetails.DepotId,
        //                            SkuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == biddingCartDetails.SkuId).SkuName,
        //                            GuaranteePrice = biddingCartDetails.GuarateedPricePerCase,
        //                            IncotermName = biddingCartDetails.Incoterm.Name,
        //                            PlantName = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == biddingCartDetails.PlantId).Name,
        //                            DepotName = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == biddingCartDetails.DepotId).Name,
        //                            BidQuantityInCase = biddingCartDetails.BidQuantityInCase,
        //                            BidQuantityMT = biddingCartDetails.BidQuantityInMT,
        //                            AvailableBidQuantityForOilType = biddingCartDetails.BiddingWindow.BiddingWindowVolumeCapacity.FirstOrDefault(_ => _.OilTypeId == biddingCartDetails.OilTypeId).VolumeCapacity,
        //                            BidPricePerCase = biddingCartDetails.BidPricePerCase,
        //                            GuarateedPricePerCase = biddingCartDetails.GuarateedPricePerCase,
        //                            CaseToMTValue = _resultService.ConvertCasetoMetricTon(1, biddingCartDetails.SkuId),

        //                            //SkuDiscount = SkuDiscountUsers(biddingCartDetails.SkuId, biddingCartDetails.DealerId, biddingCartDetails.BiddingDateAndTime),

        //                            //VolumeDiscount = VolumeDiscountUsers(biddingCartDetails.SkuId, biddingCartDetails.DealerId, biddingCartDetails.BiddingDateAndTime, dealerContext.CityId),

        //                            //SchemeDiscount = SchemeDiscountUsers(biddingCartDetails.SkuId, biddingCartDetails.DealerId, biddingCartDetails.BiddingDateAndTime),

        //                            ChancesLeft = biddingCartDetails.BiddingWindow.NoOfAttemptsForBidding - _emamiContext.SaudaBiddingCart.AsNoTracking()
        //                                .Where(_ => _.BiddingWindowId == biddingCartHeaderDetails.BiddingWindowId && _.DealerId == biddingCartHeaderDetails.DealerId && _.OilTypeId == biddingCartDetails.OilTypeId).Count(),
        //                            TotalChances = biddingCartDetails.BiddingWindow.NoOfAttemptsForBidding,
        //                            StatusId = biddingCartDetails.StatusId,
        //                            Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == biddingCartDetails.StatusId).Name,
        //                            //FreightRouteId = biddingCartDetails.Dealer.FreightRouteId ?? 0,
        //                            //FreightRouteName = biddingCartDetails.Dealer.FreightRoute.Name,
        //                            SkuWeightPerCase = Math.Round(SKUWiseWeight, 3)
        //                        };
        //                    }
        //                }
        //                saudaList.Add(saudaListForAllocationDto);
        //            }
        //        }


        //        if (saudaList != null && saudaList.Any())
        //        {

        //            return _resultService.SuccessObject(saudaList);
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

        //public ResultDto SaudaAllocationCreation(SaudaBiddingCreationInputDto inputDto)
        //{
        //    _methodName = "SaudaAllocationCreation";
        //    try
        //    {

        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }
        //        if (inputDto.BiddingWindowId <= 0)
        //        {
        //            return _resultService.ErrorMessage(Constants.BiddingWindowisMissing);
        //        }
        //        if (inputDto.DealerId <= 0)
        //        {
        //            return _resultService.ErrorMessage(Constants.DealerMissing);
        //        }
        //        if (inputDto.LoginUserId <= 0)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidUser);
        //        }
        //        if (inputDto.SaudaBiddingDetails.IsNotAny())
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }

        //        var biddingWindows = _emamiContext.BiddingWindow.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.BiddingWindowId);
        //        //if (biddingWindows != null && biddingWindows.SaudaAllocationStatusId != (int)DTO.Enums.SaudaAllocationStatus.Processing)
        //        //{
        //        //    var errorMessage = Constants.SaudaAllocationhasbeen + Utility.GetEnumFromString<DTO.Enums.SaudaAllocationStatus>(biddingWindows.SaudaAllocationStatusId);
        //        //    return _resultService.ErrorMessage(errorMessage);
        //        //}
        //        //if (biddingWindows != null && biddingWindows.StatusId != (int)DTO.Enums.BiddWindowStatus.Processing)
        //        //{
        //        //    var windowErrorMessage = Constants.BiddingWindowStatusChanged + Utility.GetEnumFromString<DTO.Enums.BiddWindowStatus>(biddingWindows.StatusId);
        //        //    return _resultService.ErrorMessage(windowErrorMessage);
        //        //}

        //        DateTime currentdate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //        var saudaAllocation = _emamiContext.RaSaudaConfiguration.AsNoTracking().FirstOrDefault(f => f.IsActive).SaudaAllocationTime;
        //        var saudaAlocationTime = new DateTime(currentdate.Year, currentdate.Month, currentdate.Day, saudaAllocation.Hours, saudaAllocation.Minutes, 0);
        //        var currentTime = new DateTime(currentdate.Year, currentdate.Month, currentdate.Day, currentdate.Hour, currentdate.Minute, 0);

        //        if (saudaAlocationTime < currentTime)
        //        {
        //            return _resultService.ErrorMessage(Constants.SaudaAllocationTimeHasBeenExceeds);
        //        }

        //        var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
        //        if (dealerContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserNotFound);
        //        }

        //        long DealerTypeId = 0;
        //        string IncotermsType = string.Empty;
        //        long BrokerId = 0;
        //        int gpBenefitType = 0;
        //        long gpBenefitAppliedType = 0;
        //        long gpBenefitCategoryType = 0;
        //        decimal gpBenefitDiscountOrDays = 0;
        //        decimal gpBenefitDiscountCase = 0;
        //        decimal gpDiscount = 0;
        //        var validToAddDays = 0;


        //        var dealerRole = _emamiContext.UserRoles.FirstOrDefault(_ => _.UserId == inputDto.DealerId);
        //        if (dealerRole != null)
        //        {
        //            DealerTypeId = dealerRole.RoleId == (int)DTO.Enums.Role.Broker ? (int)DTO.Enums.DealerType.Broker : (int)DTO.Enums.DealerType.Direct;
        //            if (dealerRole.RoleId == (int)DTO.Enums.Role.Broker)
        //            {
        //                BrokerId = inputDto.DealerId;
        //            }
        //            else
        //            {
        //                var BrokerContext = (from ucm in _emamiContext.UserCustomerMapping
        //                                     join ur in _emamiContext.UserRoles on ucm.UserId equals ur.UserId
        //                                     where ur.RoleId == (int)DTO.Enums.Role.Broker
        //                                     && ucm.CustomerId == inputDto.DealerId
        //                                     select new
        //                                     {
        //                                         BrokerId = ucm.UserId
        //                                     }).FirstOrDefault();

        //                if (BrokerContext != null)
        //                {
        //                    BrokerId = BrokerContext.BrokerId;
        //                }
        //            }
        //        }
        //        //For Notification
        //        List<SaudaCreateNotificationDto> saudaCreateEmailList = new List<SaudaCreateNotificationDto>();
        //        decimal baseSkuBidPrice = 0;
        //        var BaseSkuIdContext = _emamiContext.SaudaOrders.FirstOrDefault(_ => _.Id == inputDto.SaudaOrderId);
        //        if (BaseSkuIdContext != null)
        //        {
        //            baseSkuBidPrice = BaseSkuIdContext.BidPriceBeforeDiscount;
        //            long baseSkuId = BaseSkuIdContext.SkuId;
        //            bool isBaseSKUAvailable = false;
        //            foreach (var SaudaBiddingDetail in inputDto.SaudaBiddingDetails)
        //            {
        //                if (baseSkuId == SaudaBiddingDetail.SkuId)
        //                {
        //                    isBaseSKUAvailable = true;
        //                }
        //            }
        //            if (isBaseSKUAvailable == false)
        //            {
        //                long SaudaRABookingId = BaseSkuIdContext.Sauda.RABookingId;
        //                _emamiContext.SaudaOrders.Remove(BaseSkuIdContext);
        //                _logger.Error("Delete SaudaOrders Allocation " + BaseSkuIdContext.Id.ToString());

        //                var BiddingCartHeaderContext = _emamiContext.SaudaBiddingCartHeaders.FirstOrDefault(_ => _.Id == SaudaRABookingId);
        //                if (BiddingCartHeaderContext != null)
        //                {
        //                    var BiddingCartContext = _emamiContext.SaudaBiddingCart.FirstOrDefault(_ => _.SaudaBiddingCartHeaderId == BiddingCartHeaderContext.Id && _.SkuId == BaseSkuIdContext.SkuId);
        //                    if (BiddingCartContext != null)
        //                    {
        //                        _emamiContext.SaudaBiddingCart.Remove(BiddingCartContext);
        //                        _logger.Error("Delete BiddingCart  " + BiddingCartContext.Id.ToString());
        //                    }
        //                }
        //                _emamiContext.SaveChanges();
        //            }
        //        }

        //        int i = 0;
        //        foreach (var SaudaBiddingDetail in inputDto.SaudaBiddingDetails)
        //        {
        //            decimal baseSkuDiscount = 0;
        //            decimal baseSchemeDiscount = 0;
        //            decimal baseTotalDiscount = 0;
        //            decimal baseBidPrice = 0;
        //            decimal baseBidPriceGrandTotal = 0;
        //            decimal baseGpDiscount = 0;

        //            gpBenefitType = 0;
        //            gpBenefitAppliedType = 0;
        //            gpBenefitCategoryType = 0;
        //            gpBenefitDiscountOrDays = 0;
        //            gpBenefitDiscountCase = 0;
        //            gpDiscount = 0;
        //            long plantDepotId = 0;

        //            #region GP Benefits

        //            gpBenefitType = SaudaBiddingDetail.GPBenefitType;
        //            gpBenefitAppliedType = SaudaBiddingDetail.GPBenefitAppliedTypeId;
        //            gpBenefitCategoryType = SaudaBiddingDetail.GPBenefitOrCategoryId;
        //            if (SaudaBiddingDetail.GPBenefitType == (int)DTO.Enums.BenefitType.NONSAP)
        //            {
        //                gpBenefitDiscountOrDays = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.GPBenefitDiscountOrDay;
        //                gpBenefitDiscountCase = SaudaBiddingDetail.GPBenefitDiscountOrDay;
        //            }
        //            else
        //            {
        //                gpBenefitDiscountOrDays = SaudaBiddingDetail.GPBenefitDiscountOrDay;
        //            }

        //            #endregion

        //            long StatusId = (int)DTO.Enums.Status.Pending;
        //            DateTime? saudaValidFromDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //            var IncotermContext = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == SaudaBiddingDetail.IncotermId).Name;
        //            IncotermsType = IncotermContext.ToLower().Contains("for") ? "For" : "Ex";
        //            SaudaBiddingDetail.StatusId = StatusId;
        //            SaudaBiddingDetail.Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == StatusId).Name;

        //            #region Discount Calculation
        //            var skuDiscount = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.SkuDiscountUsers;
        //            var schemeDiscount = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.SchemeDiscountUsers;
        //            var volumeDiscount = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.VolumeDiscountUsers;  //SaudaBiddingDetail.VolumeDiscountUsers
        //            if (gpBenefitType == (int)DTO.Enums.BenefitType.NONSAP)
        //            {
        //                gpDiscount = gpBenefitDiscountOrDays;
        //            }
        //            var totalDiscount = skuDiscount + schemeDiscount + volumeDiscount + gpDiscount;
        //            var bidPrice = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.GuarateedPricePerCase;
        //            var bidPriceGrandTotal = bidPrice - totalDiscount;
        //            #endregion

        //            #region Sauda Validity Calculation
        //            decimal saudaValidityPeriod = Convert.ToDecimal(dealerContext.SaudaValidityPeriod > 0 ? dealerContext.SaudaValidityPeriod : Config.DefaultSaudaValidity);
        //            validToAddDays = Convert.ToInt32(saudaValidityPeriod);
        //            if (gpBenefitType == (int)DTO.Enums.BenefitType.SAP)
        //            {
        //                validToAddDays = Convert.ToInt32((saudaValidityPeriod + gpBenefitDiscountOrDays));
        //            }
        //            #endregion

        //            #region SaudaBiddingCart Insert
        //            var saudaBiddingCartExists = _emamiContext.SaudaBiddingCart.FirstOrDefault(_ => _.SaudaBiddingCartHeaderId == SaudaBiddingDetail.SaudaBiddingCartHeaderId && _.SkuId == SaudaBiddingDetail.SkuId);
        //            if (saudaBiddingCartExists != null)
        //            {
        //                #region Discount Calculation
        //                baseSkuDiscount = SaudaBiddingDetail.BidQuantityInCase * saudaBiddingCartExists.SkuDiscountCase;
        //                baseSchemeDiscount = SaudaBiddingDetail.BidQuantityInCase * saudaBiddingCartExists.SchemeDiscountCase;
        //                volumeDiscount = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.VolumeDiscountUsers;  //SaudaBiddingDetail.VolumeDiscountUsers
        //                if (saudaBiddingCartExists.GPBenefitType == (int)DTO.Enums.BenefitType.NONSAP)
        //                {
        //                    baseGpDiscount = SaudaBiddingDetail.BidQuantityInCase * saudaBiddingCartExists.GPBenefitDiscountInCase;
        //                }
        //                baseTotalDiscount = baseSkuDiscount + baseSchemeDiscount + volumeDiscount + baseGpDiscount;
        //                baseBidPrice = SaudaBiddingDetail.BidQuantityInCase * saudaBiddingCartExists.BidPricePerCase;
        //                baseBidPriceGrandTotal = baseBidPrice - baseTotalDiscount;
        //                #endregion

        //                saudaBiddingCartExists.BidQuantityInCase = SaudaBiddingDetail.BidQuantityInCase;
        //                saudaBiddingCartExists.BidQuantityInMT = SaudaBiddingDetail.BidQuantityInMT;  //_resultService.ConvertCasetoMetricTon(SaudaBiddingDetail.BidQuantityInCase, SaudaBiddingDetail.SkuId);
        //                saudaBiddingCartExists.TotalPrice = SaudaBiddingDetail.BidQuantityInCase * saudaBiddingCartExists.BidPricePerCase; // SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.GuarateedPricePerCase;  //SaudaBiddingDetail.BidPricePerCase

        //                saudaBiddingCartExists.SkuDiscount = baseSkuDiscount;
        //                saudaBiddingCartExists.SchemeDiscount = baseSchemeDiscount;

        //                saudaBiddingCartExists.VolumeDiscount = volumeDiscount;
        //                saudaBiddingCartExists.VolumeDiscountCase = SaudaBiddingDetail.VolumeDiscountUsers;

        //                if (saudaBiddingCartExists.GPBenefitType == (int)DTO.Enums.BenefitType.NONSAP)
        //                {
        //                    saudaBiddingCartExists.GPBenefitDiscountOrDay = baseGpDiscount;
        //                }
        //                saudaBiddingCartExists.IsSaudaAllocated = true;
        //                saudaBiddingCartExists.BidPrice = bidPriceGrandTotal;
        //            }
        //            else
        //            {
        //                var saudaBiddingCart = new SaudaBiddingCart
        //                {
        //                    PricingId = SaudaBiddingDetail.PricingId,
        //                    BiddingWindowId = inputDto.BiddingWindowId,
        //                    BiddingDateAndTime = DateHelper.UtcToIndia(DateTime.UtcNow),
        //                    DealerId = inputDto.DealerId,
        //                    IncotermId = SaudaBiddingDetail.IncotermId,
        //                    OilTypeId = SaudaBiddingDetail.OilTypeId,
        //                    SkuId = SaudaBiddingDetail.SkuId,
        //                    PlantId = SaudaBiddingDetail.PlantId,
        //                    DepotId = SaudaBiddingDetail.DepotId,
        //                    BidPrice = bidPriceGrandTotal,
        //                    GuarateedPricePerCase = SaudaBiddingDetail.GuarateedPricePerCase,
        //                    BidPricePerCase = SaudaBiddingDetail.GuarateedPricePerCase,
        //                    BaseRate = SaudaBiddingDetail.BaseRate,
        //                    TotalPrice = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.GuarateedPricePerCase,
        //                    BidQuantityInCase = SaudaBiddingDetail.BidQuantityInCase,
        //                    BidQuantityInMT = SaudaBiddingDetail.BidQuantityInMT, // _resultService.ConvertCasetoMetricTon(SaudaBiddingDetail.BidQuantityInCase, SaudaBiddingDetail.SkuId),
        //                    SaudaBiddingCartHeaderId = SaudaBiddingDetail.SaudaBiddingCartHeaderId,
        //                    //SKU DISCOUNT
        //                    SkuDiscount = skuDiscount,
        //                    SkuDiscountCase = SaudaBiddingDetail.SkuDiscountUsers,
        //                    SkuDiscountType = SaudaBiddingDetail.SkuDiscountType,
        //                    //SCHEME DISCOUNT
        //                    SchemeDiscount = schemeDiscount,
        //                    SchemeDiscountCase = SaudaBiddingDetail.SchemeDiscountUsers,
        //                    SchemeDiscountType = SaudaBiddingDetail.SchemeDiscountType,
        //                    //VOLUME DISCOUNT
        //                    VolumeDiscount = volumeDiscount,
        //                    VolumeDiscountCase = SaudaBiddingDetail.VolumeDiscountUsers,
        //                    VolumeDiscountType = SaudaBiddingDetail.VolumeDiscountType,
        //                    //GP BENEFITS
        //                    GPBenefitType = gpBenefitType,
        //                    GPBenefitAppliedTypeId = gpBenefitAppliedType,
        //                    GPBenefitOrCategoryId = gpBenefitCategoryType,
        //                    GPBenefitDiscountOrDay = gpBenefitDiscountOrDays,
        //                    GPBenefitDiscountInCase = gpBenefitDiscountCase,

        //                    StatusId = (int)DTO.Enums.Status.Approved, //StatusId,
        //                    IsSaudaAllocated = true,
        //                    CreatedBy = inputDto.LoginUserId,
        //                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        //                    ValidFromDate = saudaValidFromDate.Value,
        //                    ValidToDate = saudaValidFromDate.Value.AddDays(validToAddDays),
        //                    BaseBidQuantityInCase = SaudaBiddingDetail.BidQuantityInCase
        //                };
        //                _emamiContext.SaudaBiddingCart.Add(saudaBiddingCart);
        //            }
        //            _emamiContext.SaveChanges();
        //            #endregion

        //            #region Sauda & SaudaOrder Insert

        //            if (SaudaBiddingDetail.IncotermId == (int)DTO.Enums.IncoTerms.ExPlant || SaudaBiddingDetail.IncotermId == (int)DTO.Enums.IncoTerms.ForPlant)
        //            {
        //                plantDepotId = SaudaBiddingDetail.PlantId;
        //            }
        //            else if (SaudaBiddingDetail.IncotermId == (int)DTO.Enums.IncoTerms.ExDepot || SaudaBiddingDetail.IncotermId == (int)DTO.Enums.IncoTerms.ForDepot)
        //            {
        //                plantDepotId = SaudaBiddingDetail.DepotId;
        //            }
        //            else
        //            {
        //                plantDepotId = SaudaBiddingDetail.DepotId;
        //            }
        //            var SaudaContext = _emamiContext.SaudaOrders.FirstOrDefault(_ => _.Id == inputDto.SaudaOrderId && _.SkuId == SaudaBiddingDetail.SkuId);  //&& _.SkuId == SaudaBiddingDetail.SkuId

        //            if (SaudaContext != null)
        //            {

        //                #region Discount Calculation
        //                baseSkuDiscount = SaudaBiddingDetail.BidQuantityInCase * saudaBiddingCartExists.SkuDiscountCase;
        //                baseSchemeDiscount = SaudaBiddingDetail.BidQuantityInCase * saudaBiddingCartExists.SchemeDiscountCase;
        //                volumeDiscount = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.VolumeDiscountUsers; //SaudaBiddingDetail.VolumeDiscountUsers
        //                if (saudaBiddingCartExists.GPBenefitType == (int)DTO.Enums.BenefitType.NONSAP)
        //                {
        //                    baseGpDiscount = SaudaBiddingDetail.BidQuantityInCase * saudaBiddingCartExists.GPBenefitDiscountInCase;
        //                }
        //                baseTotalDiscount = baseSkuDiscount + baseSchemeDiscount + volumeDiscount + baseGpDiscount;
        //                baseBidPrice = SaudaBiddingDetail.BidQuantityInCase * saudaBiddingCartExists.BidPricePerCase;
        //                baseBidPriceGrandTotal = bidPrice - totalDiscount;
        //                #endregion

        //                SaudaContext.BidPriceBeforeDiscount = SaudaBiddingDetail.GuarateedPricePerCase;
        //                SaudaContext.BidPriceBeforeDiscountForDailyReport = SaudaBiddingDetail.GuarateedPricePerCase;
        //                SaudaContext.QuotedPrice = bidPrice;
        //                SaudaContext.QuotedPriceForDailyReport = bidPrice;
        //                SaudaContext.BidQuantity = SaudaBiddingDetail.BidQuantityInMT;
        //                SaudaContext.BidQuantityForDailyReport = SaudaBiddingDetail.BidQuantityInMT; // _resultService.ConvertCasetoMetricTon(SaudaBiddingDetail.BidQuantityInCase, SaudaBiddingDetail.SkuId);
        //                SaudaContext.BidPrice = bidPriceGrandTotal;
        //                SaudaContext.BidPriceForDailyReport = bidPriceGrandTotal;
        //                SaudaContext.BidQuantityCase = SaudaBiddingDetail.BidQuantityInCase;
        //                SaudaContext.BidQuantityCaseForDailyReport = SaudaBiddingDetail.BidQuantityInCase;
        //                SaudaContext.SkuDiscount = baseSkuDiscount;
        //                SaudaContext.SkuDiscountForDailyReport = baseSkuDiscount;
        //                SaudaContext.SchemeDiscount = baseSchemeDiscount;
        //                SaudaContext.SchemeDiscountForDailyReport = baseSchemeDiscount;
        //                SaudaContext.VolumeDiscount = volumeDiscount; //SaudaBiddingDetail.VolumeDiscountUsers;
        //                SaudaContext.VolumeDiscountForDailyReport = volumeDiscount;
        //                SaudaContext.VolumeDiscountCase = SaudaBiddingDetail.VolumeDiscountUsers;
        //                SaudaContext.VolumeDiscountCaseForDailyReport = SaudaBiddingDetail.VolumeDiscountUsers;
        //                if (SaudaContext.GPBenefitType == (int)DTO.Enums.BenefitType.NONSAP)
        //                {
        //                    SaudaContext.GPBenefitDiscountOrDay = baseGpDiscount;
        //                    SaudaContext.GPBenefitDiscountOrDayForDailyReport = baseGpDiscount;
        //                }
        //                SaudaContext.IsSaudaAllocated = true;
        //                _emamiContext.SaveChanges();

        //                if (baseSkuBidPrice <= 0)
        //                {
        //                    baseSkuBidPrice = SaudaContext.BidPriceBeforeDiscount;
        //                }
        //            }
        //            else
        //            {
        //                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //                var pricingLiveContext = _emamiContext.TodayPricing.AsNoTracking().FirstOrDefault(_ => _.Id == SaudaBiddingDetail.TodayPricingId);
        //                long pricingId = 0;
        //                if (pricingLiveContext == null)
        //                {
        //                    return _resultService.ErrorMessage(Constants.PricingIdisnotValid);
        //                }

        //                if (pricingLiveContext.PricingReferneceId == 0)
        //                {
        //                    var pricing = new Pricing()
        //                    {
        //                        SkuId = pricingLiveContext.SkuId,
        //                        OilTypeId = pricingLiveContext.OilTypeId,
        //                        OilPackingTypeId = pricingLiveContext.OilPackingTypeId,
        //                        PlantId = pricingLiveContext.PlantId,
        //                        Price = pricingLiveContext.Price,
        //                        SalesOrganizationId = pricingLiveContext.SalesOrganizationId,
        //                        DistributionChannelId = pricingLiveContext.DistributionChannelId,
        //                        DivisionId = pricingLiveContext.DivisionId,
        //                        SAPPricingCode = pricingLiveContext.SAPPricingCode,
        //                        ValidFrom = pricingLiveContext.ValidFrom,
        //                        ValidTo = pricingLiveContext.ValidTo,
        //                        CreatedBy = pricingLiveContext.CreatedBy,
        //                        CreatedDate = pricingLiveContext.CreatedDate,
        //                        ModifiedBy = pricingLiveContext.ModifiedBy,
        //                        ModifiedDate = pricingLiveContext.ModifiedDate,
        //                    };
        //                    _emamiContext.Pricing.Add(pricing);
        //                    _emamiContext.SaveChanges();
        //                    pricingId = pricing.Id;
        //                    /// Update pricingLive Record Pricing Reference Id
        //                    pricingLiveContext.PricingReferneceId = pricing.Id;
        //                    _emamiContext.SaveChanges();
        //                }
        //                else
        //                {
        //                    pricingId = pricingLiveContext.PricingReferneceId;
        //                }

        //                i = i + 10;
        //                var saudaOrder = new SaudaOrder
        //                {
        //                    SaudaId = inputDto.SaudaId,
        //                    SaudaNumber = i.ToString(),
        //                    SkuId = SaudaBiddingDetail.SkuId,
        //                    OilTypeId = SaudaBiddingDetail.OilTypeId,
        //                    BidPriceBeforeDiscount = SaudaBiddingDetail.GuarateedPricePerCase,
        //                    BidPriceBeforeDiscountForDailyReport = SaudaBiddingDetail.GuarateedPricePerCase,
        //                    BidPrice = bidPriceGrandTotal,
        //                    BidPriceForDailyReport = bidPriceGrandTotal,
        //                    BidPricePerCase = SaudaBiddingDetail.GuarateedPricePerCase,
        //                    BidPricePerCaseForDailyReport = SaudaBiddingDetail.GuarateedPricePerCase,
        //                    DiscountTypeId = 0,
        //                    DiscountAmount = 0,
        //                    DiscountTypeIdForDailyReport = 0,
        //                    DiscountAmountForDailyReport = 0,
        //                    BidQuantity = SaudaBiddingDetail.BidQuantityInMT, // _resultService.ConvertCasetoMetricTon(SaudaBiddingDetail.BidQuantityInCase, SaudaBiddingDetail.SkuId),
        //                    BidQuantityForDailyReport = SaudaBiddingDetail.BidQuantityInMT,
        //                    BidQuantityCase = SaudaBiddingDetail.BidQuantityInCase,
        //                    BidQuantityCaseForDailyReport = SaudaBiddingDetail.BidQuantityInCase,
        //                    QuotedPrice = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.GuarateedPricePerCase,
        //                    QuotedPriceForDailyReport = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.GuarateedPricePerCase, //SaudaBiddingDetail.BidPricePerCase
        //                    CreatedBy = inputDto.LoginUserId,
        //                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        //                    BiddingwindowId = inputDto.BiddingWindowId,
        //                    //SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.ReverseAuction,
        //                    PricingId = SaudaBiddingDetail.PricingId,
        //                    DealerTypeId = DealerTypeId,
        //                    Incoterms1 = IncotermsType,
        //                    PlantId = plantDepotId,
        //                    //DealerLocationId = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId).FreightRouteId ?? 0,
        //                    CustomerPONumber = dealerContext.Code + DateHelper.UtcToIndia(DateTime.UtcNow).ToShortDateString(),
        //                    CustomerPONumberForDailyReport = dealerContext.Code + DateHelper.UtcToIndia(DateTime.UtcNow).ToShortDateString(),
        //                    ValidFromDate = saudaValidFromDate.Value,
        //                    ValidToDate = saudaValidFromDate.Value.AddDays(validToAddDays),
        //                    StatusId = (int)DTO.Enums.Status.Pending,
        //                    SaudaStatusId = (int)DTO.Enums.SaudaStatus.NotReleased,
        //                    StatusIdForDailyReport = (int)DTO.Enums.Status.Pending,
        //                    SaudaStatusIdForDailyReport = (int)DTO.Enums.SaudaStatus.NotReleased,
        //                    Incoterms2 = SaudaBiddingDetail.IncotermId,
        //                    BrokerId = BrokerId,
        //                    BrokerIdForDailyReport = BrokerId,
        //                    IsSAPDataSync = false,
        //                    IsSAPDataSyncApproval = false,
        //                    IsSaudaAllocated = true,
        //                    BaseRate = SaudaBiddingDetail.BaseRate,
        //                    BaseRateForDailyReport = SaudaBiddingDetail.BaseRate,
        //                    //SKU DISOUNT
        //                    SkuDiscountCase = SaudaBiddingDetail.SkuDiscountUsers,
        //                    SkuDiscount = skuDiscount,
        //                    SkuDiscountType = SaudaBiddingDetail.SkuDiscountType,
        //                    SkuDiscountCaseForDailyReport = SaudaBiddingDetail.SkuDiscountUsers,
        //                    SkuDiscountForDailyReport = skuDiscount,
        //                    SkuDiscountTypeForDailyReport = SaudaBiddingDetail.SkuDiscountType,
        //                    //SCHEME DISOUNT
        //                    SchemeDiscountCase = SaudaBiddingDetail.SchemeDiscountUsers,
        //                    SchemeDiscount = schemeDiscount,
        //                    SchemeDiscountType = SaudaBiddingDetail.SchemeDiscountType,
        //                    SchemeDiscountCaseForDailyReport = SaudaBiddingDetail.SchemeDiscountUsers,
        //                    SchemeDiscountForDailyReport = schemeDiscount,
        //                    SchemeDiscountTypeForDailyReport = SaudaBiddingDetail.SchemeDiscountType,
        //                    //VOLUME DISOUNT       
        //                    VolumeDiscount = volumeDiscount, // SaudaBiddingDetail.VolumeDiscountUsers,
        //                    VolumeDiscountCase = SaudaBiddingDetail.VolumeDiscountUsers,
        //                    VolumeDiscountType = SaudaBiddingDetail.VolumeDiscountType,
        //                    VolumeDiscountForDailyReport = volumeDiscount, // SaudaBiddingDetail.VolumeDiscountUsers,
        //                    VolumeDiscountCaseForDailyReport = SaudaBiddingDetail.VolumeDiscountUsers,
        //                    VolumeDiscountTypeForDailyReport = SaudaBiddingDetail.VolumeDiscountType,
        //                    //GP BENEFITS
        //                    GPBenefitType = gpBenefitType,
        //                    GPBenefitAppliedTypeId = gpBenefitAppliedType,
        //                    GPBenefitOrCategoryId = gpBenefitCategoryType,
        //                    GPBenefitDiscountOrDay = gpBenefitDiscountOrDays,
        //                    GPBenefitDiscountInCase = gpBenefitDiscountCase,
        //                    BaseSaudaOrderId = inputDto.SaudaOrderId,
        //                    BaseSkuBidPrice = baseSkuBidPrice,
        //                    GPBenefitTypeForDailyReport = gpBenefitType,
        //                    GPBenefitAppliedTypeIdForDailyReport = gpBenefitAppliedType,
        //                    GPBenefitOrCategoryIdForDailyReport = gpBenefitCategoryType,
        //                    GPBenefitDiscountOrDayForDailyReport = gpBenefitDiscountOrDays,
        //                    GPBenefitDiscountInCaseForDailyReport = gpBenefitDiscountCase,
        //                    BaseSkuBidPriceForDailyReport = baseSkuBidPrice
        //                };
        //                _emamiContext.SaudaOrders.Add(saudaOrder);
        //                _emamiContext.SaveChanges();
        //            }

        //            #endregion
        //        }
        //        return _resultService.SuccessMessage(Constants.SaudaAllocationSuccessfully);
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        //public ResultDto SaudaAllocationCreationOld(SaudaBiddingCreationInputDto inputDto)
        //{
        //    //_methodName = "SaudaAllocationCreation";
        //    //try
        //    //{
        //    //    if (inputDto == null)
        //    //    {
        //    //        return _resultService.ErrorMessage(Constants.InvalidRequest);
        //    //    }
        //    //    if (inputDto.BiddingWindowId <= 0)
        //    //    {
        //    //        return _resultService.ErrorMessage(Constants.BiddingWindowisMissing);
        //    //    }
        //    //    if (inputDto.DealerId <= 0)
        //    //    {
        //    //        return _resultService.ErrorMessage(Constants.DealerMissing);
        //    //    }
        //    //    if (inputDto.LoginUserId <= 0)
        //    //    {
        //    //        return _resultService.ErrorMessage(Constants.InvalidUser);
        //    //    }
        //    //    if (inputDto.SaudaBiddingDetails.Count() < 0 && inputDto.SaudaBiddingDetails.Any())
        //    //    {
        //    //        return _resultService.ErrorMessage(Constants.InvalidRequest);
        //    //    }

        //    //    var biddingWindows = _emamiContext.BiddingWindow.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.BiddingWindowId);
        //    //    //if (biddingWindows != null && biddingWindows.SaudaAllocationStatusId != (int)DTO.Enums.SaudaAllocationStatus.Processing)
        //    //    //{
        //    //    //    var errorMessage = Constants.SaudaAllocationhasbeen + Utility.GetEnumFromString<DTO.Enums.SaudaAllocationStatus>(biddingWindows.SaudaAllocationStatusId);
        //    //    //    return _resultService.ErrorMessage(errorMessage);
        //    //    //}

        //    //    var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
        //    //    if (dealerContext == null)
        //    //    {
        //    //        return _resultService.ErrorMessage(Constants.UserNotFound);
        //    //    }

        //    //    long DealerTypeId = 0;
        //    //    string IncotermsType = string.Empty;
        //    //    long BrokerId = 0;
        //    //    var dealerRole = _emamiContext.UserRoles.FirstOrDefault(_ => _.UserId == inputDto.DealerId);
        //    //    if (dealerRole != null)
        //    //    {
        //    //        DealerTypeId = dealerRole.RoleId == (int)DTO.Enums.Role.Broker ? (int)DTO.Enums.DealerType.Broker : (int)DTO.Enums.DealerType.Direct;
        //    //        if (dealerRole.RoleId == (int)DTO.Enums.Role.Broker)
        //    //        {
        //    //            BrokerId = inputDto.DealerId;
        //    //        }
        //    //        else
        //    //        {
        //    //            var BrokerContext = (from ucm in _emamiContext.UserCustomerMapping
        //    //                                 join ur in _emamiContext.UserRoles on ucm.UserId equals ur.UserId
        //    //                                 where ur.RoleId == (int)DTO.Enums.Role.Broker
        //    //                                 && ucm.CustomerId == inputDto.DealerId
        //    //                                 select new
        //    //                                 {
        //    //                                     BrokerId = ucm.UserId
        //    //                                 }).FirstOrDefault();

        //    //            if (BrokerContext != null)
        //    //            {
        //    //                BrokerId = BrokerContext.BrokerId;
        //    //            }
        //    //        }
        //    //    }
        //    //    //For Notification
        //    //    List<SaudaCreateNotificationDto> saudaCreateEmailList = new List<SaudaCreateNotificationDto>();

        //    //    var BaseSkuIdContext = _emamiContext.SaudaOrders.FirstOrDefault(_ => _.Id == inputDto.SaudaOrderId);
        //    //    if (BaseSkuIdContext != null)
        //    //    {
        //    //        long baseSkuId = BaseSkuIdContext.SkuId;
        //    //        bool isBaseSKUAvailable = false;
        //    //        foreach (var SaudaBiddingDetail in inputDto.SaudaBiddingDetails)
        //    //        {
        //    //            if (baseSkuId == SaudaBiddingDetail.SkuId)
        //    //            {
        //    //                isBaseSKUAvailable = true;
        //    //            }
        //    //        }
        //    //        if (isBaseSKUAvailable == false)
        //    //        {
        //    //            long SaudaRABookingId = BaseSkuIdContext.Sauda.RABookingId;
        //    //            _emamiContext.SaudaOrders.Remove(BaseSkuIdContext);
        //    //            _logger.Error("Delete SaudaOrders Allocation " + BaseSkuIdContext.Id.ToString());

        //    //            var BiddingCartHeaderContext = _emamiContext.SaudaBiddingCartHeaders.FirstOrDefault(_ => _.Id == SaudaRABookingId);
        //    //            if (BiddingCartHeaderContext != null)
        //    //            {
        //    //                var BiddingCartContext = _emamiContext.SaudaBiddingCart.FirstOrDefault(_ => _.SaudaBiddingCartHeaderId == BiddingCartHeaderContext.Id && _.SkuId == BaseSkuIdContext.SkuId);
        //    //                if (BiddingCartContext != null)
        //    //                {
        //    //                    _emamiContext.SaudaBiddingCart.Remove(BiddingCartContext);
        //    //                    _logger.Error("Delete BiddingCart  " + BiddingCartContext.Id.ToString());
        //    //                }
        //    //            }
        //    //            _emamiContext.SaveChanges();
        //    //        }
        //    //    }
        //    //    //Guid g = Guid.NewGuid();
        //    //    foreach (var SaudaBiddingDetail in inputDto.SaudaBiddingDetails)
        //    //    {
        //    //        long StatusId = (int)DTO.Enums.Status.Pending;
        //    //        DateTime? saudaValidFromDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //    //        long? depotIdForRake = 0;
        //    //        if (SaudaBiddingDetail.IncotermId == (int)DTO.Enums.IncoTerms.ExRake || SaudaBiddingDetail.IncotermId == (int)DTO.Enums.IncoTerms.ForRake)
        //    //        {
        //    //            depotIdForRake = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == SaudaBiddingDetail.PlantId && !_.IsPlant)?.DepotId;
        //    //        }

        //    //        var IncotermContext = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == SaudaBiddingDetail.IncotermId).Name;
        //    //        IncotermsType = IncotermContext.ToLower().Contains("for") ? "For" : "Ex";
        //    //        SaudaBiddingDetail.StatusId = StatusId;
        //    //        SaudaBiddingDetail.Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == StatusId).Name;

        //    //        var saudaBiddingCartExists = _emamiContext.SaudaBiddingCart.FirstOrDefault(_ => _.SaudaBiddingCartHeaderId == SaudaBiddingDetail.SaudaBiddingCartHeaderId && _.SkuId == SaudaBiddingDetail.SkuId);
        //    //        if (saudaBiddingCartExists != null)
        //    //        {
        //    //            //saudaBiddingCartExists.BidPricePerCase = SaudaBiddingDetail.GuarateedPricePerCase; //SaudaBiddingDetail.BidPricePerCase
        //    //            saudaBiddingCartExists.BidQuantityInCase = SaudaBiddingDetail.BidQuantityInCase;
        //    //            saudaBiddingCartExists.BidQuantityInMT = _resultService.ConvertCasetoMetricTon(SaudaBiddingDetail.BidQuantityInCase, SaudaBiddingDetail.SkuId);
        //    //            saudaBiddingCartExists.TotalPrice = SaudaBiddingDetail.BidQuantityInCase * saudaBiddingCartExists.BidPricePerCase; // SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.GuarateedPricePerCase;  //SaudaBiddingDetail.BidPricePerCase

        //    //            saudaBiddingCartExists.SkuDiscount = SaudaBiddingDetail.BidQuantityInCase * saudaBiddingCartExists.SkuDiscountCase;
        //    //            saudaBiddingCartExists.SchemeDiscount = SaudaBiddingDetail.BidQuantityInCase * saudaBiddingCartExists.SchemeDiscountCase;
        //    //            saudaBiddingCartExists.VolumeDiscount = SaudaBiddingDetail.VolumeDiscountUsers;

        //    //            saudaBiddingCartExists.IsSaudaAllocated = true;
        //    //            _emamiContext.SaveChanges();
        //    //        }
        //    //        else
        //    //        {
        //    //            var saudaBiddingCart = new SaudaBiddingCart
        //    //            {
        //    //                BiddingWindowId = inputDto.BiddingWindowId,
        //    //                BiddingDateAndTime = DateHelper.UtcToIndia(DateTime.UtcNow),
        //    //                DealerId = inputDto.DealerId,
        //    //                IncotermId = SaudaBiddingDetail.IncotermId,
        //    //                PlantId = SaudaBiddingDetail.PlantId,
        //    //                DepotId = SaudaBiddingDetail.DepotId,
        //    //                OilTypeId = SaudaBiddingDetail.OilTypeId,
        //    //                SkuId = SaudaBiddingDetail.SkuId,

        //    //                GuarateedPricePerCase = SaudaBiddingDetail.GuarateedPricePerCase,
        //    //                BidPricePerCase = SaudaBiddingDetail.GuarateedPricePerCase,  // SaudaBiddingDetail.BidPricePerCase,
        //    //                BaseRate = SaudaBiddingDetail.BaseRate,
        //    //                TotalPrice = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.GuarateedPricePerCase,  //SaudaBiddingDetail.BidPricePerCase

        //    //                BidQuantityInCase = SaudaBiddingDetail.BidQuantityInCase,
        //    //                BidQuantityInMT = _resultService.ConvertCasetoMetricTon(SaudaBiddingDetail.BidQuantityInCase, SaudaBiddingDetail.SkuId),

        //    //                StatusId = StatusId,
        //    //                SaudaBiddingCartHeaderId = SaudaBiddingDetail.SaudaBiddingCartHeaderId,
        //    //                CreatedBy = inputDto.LoginUserId,
        //    //                CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),

        //    //                SkuDiscount = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.SkuDiscountUsers,
        //    //                SkuDiscountCase = SaudaBiddingDetail.SkuDiscountUsers,
        //    //                SkuDiscountType = SaudaBiddingDetail.SkuDiscountType,

        //    //                SchemeDiscount = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.SchemeDiscountUsers,
        //    //                SchemeDiscountCase = SaudaBiddingDetail.SchemeDiscountUsers,
        //    //                SchemeDiscountType = SaudaBiddingDetail.SchemeDiscountType,

        //    //                VolumeDiscount = SaudaBiddingDetail.VolumeDiscountUsers,
        //    //                VolumeDiscountType = SaudaBiddingDetail.VolumeDiscountType,

        //    //                IsSaudaAllocated = true
        //    //            };
        //    //            _emamiContext.SaudaBiddingCart.Add(saudaBiddingCart);
        //    //            _emamiContext.SaveChanges();
        //    //        }
        //    //    }


        //    //    DateTime currentdate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //    //    foreach (var SaudaBiddingDetail in inputDto.SaudaBiddingDetails)
        //    //    {
        //    //        long StatusId = (int)DTO.Enums.Status.Approved;
        //    //        DateTime? saudaValidFromDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //    //        long? depotIdForRake = 0;
        //    //        if (SaudaBiddingDetail.IncotermId == (int)DTO.Enums.IncoTerms.ExRake || SaudaBiddingDetail.IncotermId == (int)DTO.Enums.IncoTerms.ForRake)
        //    //        {
        //    //            depotIdForRake = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == SaudaBiddingDetail.PlantId && !_.IsPlant)?.DepotId;

        //    //        }

        //    //        var IncotermContext = _emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == SaudaBiddingDetail.IncotermId).Name;
        //    //        IncotermsType = IncotermContext.ToLower().Contains("for") ? "For" : "Ex";

        //    //        if (StatusId == (int)DTO.Enums.Status.Approved)
        //    //        {
        //    //            #region GP Benefits

        //    //            long benefitTypeId = 0L;
        //    //            string benefitType = string.Empty;
        //    //            string benefitOrCategory = string.Empty;
        //    //            var benefitDays = 0L;
        //    //            var gpBenefitDiscount = 0.0m;
        //    //            var gpBenefitUser = new List<GPBenefitUser>();

        //    //            var gpBenefitGeography = _emamiContext.GPBenefitGeography.AsNoTracking()
        //    //                .Join(_emamiContext.GPBenefitGeographyMappings.AsNoTracking(), gp => gp.Id, gpd => gpd.GPBenefitGeographyId, (gp, gpd) => new { gp, gpd })
        //    //                .Where(_ => _.gpd.CustomerId == inputDto.DealerId && _.gpd.SkuId == SaudaBiddingDetail.SkuId
        //    //                && _.gp.IsActive
        //    //                && DbFunctions.TruncateTime(currentdate) <= DbFunctions.TruncateTime(_.gp.ValidTo)
        //    //                && DbFunctions.TruncateTime(_.gp.ValidFrom) <= DbFunctions.TruncateTime(currentdate))
        //    //                .Select(_ => _.gp).ToList();

        //    //            if (gpBenefitGeography.IsAny())
        //    //            {
        //    //                #region GP Benefits -Geography Based

        //    //                foreach (var benefit in gpBenefitGeography)
        //    //                {
        //    //                    benefitTypeId = benefit.BenefitTypesId;
        //    //                    benefitType = benefit.BenefitTypes?.Name;
        //    //                    //benefitDiscountOrDays = benefit.DiscountOrDays;

        //    //                    if (benefit.BenefitTypesId == (int)DTO.Enums.BenefitType.SAP)
        //    //                    {
        //    //                        benefitDays = (long)benefit.DiscountOrDays;
        //    //                        benefitOrCategory = benefit.BenefitOrCategoryId > 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.BenefitCategory)Convert.ToInt32(benefit.BenefitOrCategoryId)) : string.Empty;
        //    //                    }
        //    //                    else
        //    //                    {
        //    //                        gpBenefitDiscount = benefit.DiscountOrDays;
        //    //                        var nonsapBenefit = _emamiContext.Benefits.AsNoTracking().FirstOrDefault(_ => _.Id == benefit.BenefitOrCategoryId);
        //    //                        if (nonsapBenefit != null)
        //    //                        {
        //    //                            benefitOrCategory = nonsapBenefit.BenefitCategory;
        //    //                        }
        //    //                    }
        //    //                }

        //    //                #endregion
        //    //            }
        //    //            else
        //    //            {
        //    //                #region GP Benefits - UserBased

        //    //                gpBenefitUser = _emamiContext.GPBenefitUsers.AsNoTracking()
        //    //                    .Join(_emamiContext.GPBenefitUserMappings.AsNoTracking(), gp => gp.Id, gpd => gpd.GPBenefitUserId, (gp, gpd) => new { gp, gpd })
        //    //                    .Where(_ => _.gpd.UserId == inputDto.DealerId && _.gpd.SkuId == SaudaBiddingDetail.SkuId
        //    //                    && _.gp.IsActive
        //    //                    && DbFunctions.TruncateTime(currentdate) <= DbFunctions.TruncateTime(_.gp.ValidTo)
        //    //                    && DbFunctions.TruncateTime(_.gp.ValidFrom) <= DbFunctions.TruncateTime(currentdate))
        //    //                    .Select(_ => _.gp).ToList();

        //    //                if (gpBenefitUser != null && gpBenefitUser.Any())
        //    //                {
        //    //                    foreach (var benefit in gpBenefitUser)
        //    //                    {
        //    //                        benefitTypeId = benefit.BenefitTypesId;
        //    //                        benefitType = benefit.BenefitTypes?.Name;

        //    //                        if (benefit.BenefitTypesId == (int)DTO.Enums.BenefitType.SAP)
        //    //                        {
        //    //                            benefitDays = (long)benefit.DiscountOrDays;
        //    //                            benefitOrCategory = benefit.BenefitOrCategoryId > 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.BenefitCategory)Convert.ToInt32(benefit.BenefitOrCategoryId)) : string.Empty;
        //    //                        }
        //    //                        else
        //    //                        {
        //    //                            gpBenefitDiscount = benefit.DiscountOrDays;
        //    //                            var nonsapBenefit = _emamiContext.Benefits.AsNoTracking().FirstOrDefault(_ => _.Id == benefit.BenefitOrCategoryId);
        //    //                            if (nonsapBenefit != null)
        //    //                            {
        //    //                                benefitOrCategory = nonsapBenefit.BenefitCategory;
        //    //                            }
        //    //                        }
        //    //                    }
        //    //                }

        //    //                #endregion
        //    //            }

        //    //            #endregion

        //    //            var skuDiscount = SaudaBiddingDetail.SkuDiscountUsers > 0 ? SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.SkuDiscountUsers : 0;
        //    //            var schemeDiscount = SaudaBiddingDetail.SchemeDiscountUsers > 0 ? SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.SchemeDiscountUsers : 0;
        //    //            var volumeDiscount = SaudaBiddingDetail.VolumeDiscountUsers > 0 ? SaudaBiddingDetail.VolumeDiscountUsers : 0;
        //    //            var gpDiscount = gpBenefitDiscount > 0 ? SaudaBiddingDetail.BidQuantityInCase * gpBenefitDiscount : 0;

        //    //            var totalDiscount = skuDiscount + schemeDiscount + volumeDiscount + gpDiscount;

        //    //            var bidPrice = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.GuarateedPricePerCase;
        //    //            var bidPriceGrandTotal = bidPrice - totalDiscount;

        //    //            var saudaValidityPeriod = Convert.ToDouble(dealerContext.SaudaValidityPeriod > 0 ? dealerContext.SaudaValidityPeriod : Config.DefaultSaudaValidity);
        //    //            var validToAddDays = saudaValidityPeriod + benefitDays;

        //    //            var SaudaContext = _emamiContext.SaudaOrders.FirstOrDefault(_ => _.Id == inputDto.SaudaOrderId && _.SkuId == SaudaBiddingDetail.SkuId);  //&& _.SkuId == SaudaBiddingDetail.SkuId
        //    //            if (SaudaContext != null)
        //    //            {
        //    //                SaudaContext.BidPriceBeforeDiscount = SaudaBiddingDetail.GuarateedPricePerCase;  //SaudaBiddingDetail.BidPricePerCase
        //    //                //SaudaContext.BidPricePerCase = SaudaBiddingDetail.GuarateedPricePerCase;  //SaudaBiddingDetail.BidPricePerCase
        //    //                SaudaContext.BidPrice = bidPriceGrandTotal; //SaudaBiddingDetail.BidPricePerCase - benefitDiscount;
        //    //                SaudaContext.BidQuantity = _resultService.ConvertCasetoMetricTon(SaudaBiddingDetail.BidQuantityInCase, SaudaBiddingDetail.SkuId);
        //    //                SaudaContext.BidQuantityCase = SaudaBiddingDetail.BidQuantityInCase;
        //    //                SaudaContext.QuotedPrice = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.BidPricePerCase; //SaudaBiddingDetail.BidPricePerCase
        //    //                SaudaContext.IsSaudaAllocated = true;

        //    //                SaudaContext.SkuDiscount = SaudaBiddingDetail.BidQuantityInCase * SaudaContext.SkuDiscountCase; //skuDiscount
        //    //                SaudaContext.SchemeDiscount = SaudaBiddingDetail.BidQuantityInCase * SaudaContext.SchemeDiscountCase; //schemeDiscount;
        //    //                SaudaContext.VolumeDiscount = SaudaBiddingDetail.VolumeDiscountUsers; //volumeDiscount;

        //    //                SaudaContext.GPDiscount = SaudaBiddingDetail.BidQuantityInCase * SaudaContext.GPDiscountCase;   //gpDiscount
        //    //                SaudaContext.GPBenefitDays = benefitDays;
        //    //                SaudaContext.ValidToDate = benefitDays > 0 ? saudaValidFromDate.Value.AddDays(benefitDays) : SaudaContext.ValidToDate;
        //    //                _emamiContext.SaveChanges();
        //    //            }
        //    //            else
        //    //            {
        //    //                var saudaOrder = new SaudaOrder
        //    //                {
        //    //                    SaudaId = inputDto.SaudaId,
        //    //                    SkuId = SaudaBiddingDetail.SkuId,
        //    //                    OilTypeId = SaudaBiddingDetail.OilTypeId,
        //    //                    BidPriceBeforeDiscount = SaudaBiddingDetail.GuarateedPricePerCase,
        //    //                    BidPrice = bidPriceGrandTotal,
        //    //                    BidPricePerCase = SaudaBiddingDetail.GuarateedPricePerCase,
        //    //                    DiscountTypeId = 0,
        //    //                    DiscountAmount = 0,
        //    //                    BidQuantity = _resultService.ConvertCasetoMetricTon(SaudaBiddingDetail.BidQuantityInCase, SaudaBiddingDetail.SkuId),
        //    //                    BidQuantityCase = SaudaBiddingDetail.BidQuantityInCase,
        //    //                    QuotedPrice = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.GuarateedPricePerCase,   //SaudaBiddingDetail.BidPricePerCase
        //    //                    CreatedBy = inputDto.LoginUserId,
        //    //                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
        //    //                    BiddingwindowId = inputDto.BiddingWindowId,
        //    //                    SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.ReverseAuction,
        //    //                    PricingId = SaudaBiddingDetail.PricingId,
        //    //                    DealerTypeId = DealerTypeId,
        //    //                    Incoterms1 = IncotermsType,
        //    //                    PlantId = SaudaBiddingDetail.PlantId,
        //    //                    DealerLocationId = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId).FreightRouteId ?? 0,
        //    //                    CustomerPONumber = dealerContext.Code + DateHelper.UtcToIndia(DateTime.UtcNow).ToShortDateString(),
        //    //                    GPBenefitDays = benefitDays,
        //    //                    ValidFromDate = saudaValidFromDate.Value,
        //    //                    ValidToDate = saudaValidFromDate.Value.AddDays(validToAddDays),
        //    //                    StatusId = (int)DTO.Enums.Status.Pending,
        //    //                    SaudaStatusId = (int)DTO.Enums.SaudaStatus.NotReleased,
        //    //                    Incoterms2 = SaudaBiddingDetail.IncotermId,
        //    //                    BrokerId = BrokerId,
        //    //                    IsSAPDataSync = false,
        //    //                    IsSAPDataSyncApproval = false,
        //    //                    DepotIdForRake = depotIdForRake.Value,
        //    //                    IsSaudaAllocated = true,

        //    //                    //SkuDiscount = skuDiscount,
        //    //                    SkuDiscountCase = SaudaBiddingDetail.SkuDiscountUsers,
        //    //                    SkuDiscount = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.SkuDiscountUsers,
        //    //                    SkuDiscountType = SaudaBiddingDetail.SkuDiscountType,

        //    //                    //SchemeDiscount = schemeDiscount,
        //    //                    SchemeDiscountCase = SaudaBiddingDetail.SchemeDiscountUsers,
        //    //                    SchemeDiscount = SaudaBiddingDetail.BidQuantityInCase * SaudaBiddingDetail.SchemeDiscountUsers,
        //    //                    SchemeDiscountType = SaudaBiddingDetail.SchemeDiscountType,

        //    //                    //GeographySchemeDiscount = geographySchemeDiscount,
        //    //                    //VolumeDiscount = volumeDiscount,                                
        //    //                    VolumeDiscount = SaudaBiddingDetail.VolumeDiscountUsers,
        //    //                    VolumeDiscountType = SaudaBiddingDetail.VolumeDiscountType,

        //    //                    GPDiscount = gpDiscount,
        //    //                    GPDiscountCase = gpBenefitDiscount,
        //    //                    BaseRate = SaudaBiddingDetail.BaseRate
        //    //                };
        //    //                _emamiContext.SaudaOrders.Add(saudaOrder);
        //    //                _emamiContext.SaveChanges();

        //    //                #region Sauda - GP Benefits Mapping

        //    //                //if (gpBenefitGeography != null && gpBenefitGeography.Any())
        //    //                //{
        //    //                //    //Sauda Benefits Mapping
        //    //                //    foreach (var benefit in gpBenefitGeography)
        //    //                //    {
        //    //                //        var saudaBenefitsMapping = new SurpriseAndGPBenefitHistory();
        //    //                //        saudaBenefitsMapping.SaudaOrderId = saudaOrder.Id;
        //    //                //        saudaBenefitsMapping.BenefitTypeId = benefit.BenefitTypesId;
        //    //                //        saudaBenefitsMapping.BenefitOrCategoryId = benefit.BenefitOrCategoryId;
        //    //                //        saudaBenefitsMapping.BenefitDiscountOrDays = benefit.DiscountOrDays;
        //    //                //        saudaBenefitsMapping.BenefitUserOrGeographyId = benefit.Id;
        //    //                //        saudaBenefitsMapping.SurpriseBenefitAppliedType = Constants.GeographyBased;
        //    //                //        saudaBenefitsMapping.IsGPBenefit = true;
        //    //                //        saudaBenefitsMapping.CreatedBy = inputDto.LoginUserId;
        //    //                //        saudaBenefitsMapping.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //    //                //        _emamiContext.SurpriseAndGPBenefitHistory.Add(saudaBenefitsMapping);
        //    //                //        _emamiContext.SaveChanges();
        //    //                //    }
        //    //                //}
        //    //                //else if (gpBenefitUser != null && gpBenefitUser.Any())
        //    //                //{
        //    //                //    //Sauda Benefits Mapping
        //    //                //    foreach (var benefit in gpBenefitUser)
        //    //                //    {
        //    //                //        var saudaBenefitsMapping = new SurpriseAndGPBenefitHistory();
        //    //                //        saudaBenefitsMapping.SaudaOrderId = saudaOrder.Id;
        //    //                //        saudaBenefitsMapping.BenefitTypeId = benefit.BenefitTypesId;
        //    //                //        saudaBenefitsMapping.BenefitOrCategoryId = benefit.BenefitOrCategoryId;
        //    //                //        saudaBenefitsMapping.BenefitDiscountOrDays = benefit.DiscountOrDays;
        //    //                //        saudaBenefitsMapping.BenefitUserOrGeographyId = benefit.Id;
        //    //                //        saudaBenefitsMapping.SurpriseBenefitAppliedType = Constants.UserBased;
        //    //                //        saudaBenefitsMapping.IsGPBenefit = true;
        //    //                //        saudaBenefitsMapping.CreatedBy = inputDto.LoginUserId;
        //    //                //        saudaBenefitsMapping.CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //    //                //        _emamiContext.SurpriseAndGPBenefitHistory.Add(saudaBenefitsMapping);
        //    //                //        _emamiContext.SaveChanges();
        //    //                //    }
        //    //                //}

        //    //                #endregion
        //    //            }
        //    //        }
        //    //    }
        //    //    return _resultService.SuccessMessage(Constants.SaudaAllocationSuccessfully);
        //    //}
        //    //catch (Exception exception)
        //    //{
        //    //    var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //    //    _logger.Error(message);
        //    //    return _resultService.ErrorMessage(Constants.Exception);
        //    //}
        //    return _resultService.SuccessMessage(Constants.SaudaAllocationSuccessfully);
        //}

        //public ResultDto SaudaAllocationSkuDetails(BiddingCartSkuInputDto inputDto)
        //{
        //    _methodName = "SaudaAllocationSkuDetails";
        //    var skuOutputDto = new List<BiddingCartSkuOutputDto>();

        //    if (inputDto == null)
        //    {
        //        return _resultService.ErrorMessage(Constants.InvalidRequest);
        //    }
        //    if (inputDto.OilTypeIds == null)
        //    {
        //        return _resultService.ErrorMessage(Constants.OilTypeMissing);
        //    }
        //    if (inputDto.BiddingWindowId == 0)
        //    {
        //        return _resultService.ErrorMessage(Constants.BiddingWindowisMissing);
        //    }

        //    try
        //    {
        //        DateTime currentdate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //        decimal skuDiscount = 0;
        //        decimal schemeDiscount = 0;
        //        int skuDiscountType = 0;
        //        int schemeDiscountType = 0;

        //        var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
        //        if (dealerContext != null)
        //        {
        //            var saudaOrder = _emamiContext.SaudaOrders.AsNoTracking()
        //            .FirstOrDefault(f => f.Id == inputDto.SaudaOrderId && !f.IsSaudaAllocated);

        //            if (saudaOrder != null)
        //            {
        //                #region Get Common Data's

        //                var plantData = _emamiContext.Depots.AsNoTracking()
        //                    .FirstOrDefault(f => f.Id == inputDto.PlantId && f.StorageTypeId == (int)DTO.Enums.StorageType.Plant);

        //                var incoTermData = _emamiContext.IncoTerms.AsNoTracking()
        //                    .FirstOrDefault(f => f.Id == inputDto.IncotermId);

        //                var oilTypeDatas = _emamiContext.OilTypes.AsNoTracking().Where(w => inputDto.OilTypeIds.Contains(w.Id))
        //                                                   .Select(s => new
        //                                                   {
        //                                                       Id = s.Id,
        //                                                       Name = s.Name
        //                                                   }).ToList();
        //                if (oilTypeDatas.IsNotAny())
        //                {
        //                    return _resultService.ErrorMessage(Constants.OilTypeNotFound);
        //                }

        //                var skuDatas = _emamiContext.Skus.AsNoTracking()
        //                    .Where(w => saudaOrder.OilTypeId == w.OilTypeId)
        //                   .Select(s => new
        //                   {
        //                       Id = s.Id,
        //                       Name = s.SkuName,
        //                       SkuUom = s.Uom.Name,
        //                       LitreConversion = s.OilType.LitreConversion,
        //                       Quantity = s.Quantity
        //                   }).ToList();
        //                if (skuDatas.IsNotAny())
        //                {
        //                    return _resultService.ErrorMessage(Constants.BaseSkuEmpty);
        //                }
        //                #endregion

        //                #region Base SKU Details
        //                var BiddingCartSku = new BiddingCartSkuOutputDto
        //                {
        //                    SkuId = saudaOrder.SkuId,
        //                    SkuName = skuDatas.IsAny() ? skuDatas.FirstOrDefault(f => f.Id == saudaOrder.SkuId).Name : string.Empty,
        //                    OilTypeId = saudaOrder.OilTypeId,
        //                    OilType = oilTypeDatas.IsAny() ? oilTypeDatas.FirstOrDefault(f => f.Id == saudaOrder.OilTypeId).Name : string.Empty,
        //                    IncotermId = inputDto.IncotermId,
        //                    IncotermName = incoTermData.Name,
        //                    PricingId = saudaOrder.PricingId,
        //                    //FreightRouteId = (int)dealerContext.FreightRouteId,
        //                    PlantId = saudaOrder.PlantId,
        //                    PlantName = plantData.Name,

        //                    GuaranteePrice = saudaOrder.BidPricePerCase,
        //                    BaseRate = saudaOrder.BaseRate,

        //                    GPBenefitType = saudaOrder.GPBenefitType,
        //                    GPBenefitAppliedTypeId = saudaOrder.GPBenefitAppliedTypeId,
        //                    GPBenefitOrCategoryId = saudaOrder.GPBenefitOrCategoryId,
        //                    GPBenefitDiscountOrDay = saudaOrder.GPBenefitDiscountInCase,

        //                    //DepotId = saudaOrder.DepotId,
        //                    //DepotName = depotDatas.IsAny() ? depotDatas.FirstOrDefault(f => f.Id == pricing.DepotId).Name : string.Empty,
        //                    //FreightRouteName = FreightRoutesDatas.IsAny() ? FreightRoutesDatas.FirstOrDefault(f => f.Id == pricing.FrieghtRouteId).Name : string.Empty,
        //                    //GPBenefitOrCategory = _emamiContext.Benefits.AsNoTracking().FirstOrDefault(f => f.BenefitTypeId == saudaOrder.GPBenefitType).BenefitCategory,
        //                };

        //                BiddingCartSku.SkuDiscount = saudaOrder.SkuDiscountCase;
        //                BiddingCartSku.SchemeDiscount = saudaOrder.SchemeDiscountCase;

        //                //var volumeDatas = VolumeDiscountUsers(saudaOrder.SkuId, inputDto.DealerId, currentdate, dealerContext.CityId);
        //                //BiddingCartSku.VolumeDiscount = volumeDatas;
        //                //BiddingCartSku.VolumeDiscountType = volumeDatas.VolumeDiscountType;

        //                BiddingCartSku.AppliedVolumeDiscount = saudaOrder.VolumeDiscount;
        //                BiddingCartSku.AppliedVolumeDiscountType = saudaOrder.VolumeDiscountType;

        //                BiddingCartSku.CaseToMTValue = _resultService.ConvertCasetoMetricTon(1, saudaOrder.SkuId);
        //                BiddingCartSku.SkuDiscountType = saudaOrder.SkuDiscountType;
        //                BiddingCartSku.SchemeDiscountType = saudaOrder.SchemeDiscountType;

        //                #region SKU Weight

        //                var baseSkuDetail = skuDatas.FirstOrDefault(f => f.Id == saudaOrder.SkuId);
        //                decimal SKUWiseWeight = 0;
        //                if (baseSkuDetail.SkuUom == DTO.Enums.Uom.Ltr.ToString())
        //                {
        //                    var skuUomMappingData = _emamiContext.SkuUomMapping.AsNoTracking()
        //                 .FirstOrDefault(_ => _.SkuId == saudaOrder.SkuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)(int)DTO.Enums.Uom.Nos);
        //                    if (skuUomMappingData != null)
        //                    {
        //                        SKUWiseWeight = baseSkuDetail.LitreConversion > 0 ? (baseSkuDetail.Quantity * 1000 * skuUomMappingData.ConversionFactor) / baseSkuDetail.LitreConversion : 0;
        //                    }
        //                    else
        //                    {
        //                        SKUWiseWeight = baseSkuDetail.LitreConversion > 0 ? (baseSkuDetail.Quantity * 1000) / baseSkuDetail.LitreConversion : 0;
        //                    }
        //                }
        //                else
        //                {
        //                    SKUWiseWeight = baseSkuDetail.Quantity;
        //                }
        //                BiddingCartSku.SkuWeightPerCase = Math.Round(SKUWiseWeight, 3);

        //                #endregion

        //                skuOutputDto.Add(BiddingCartSku);
        //                #endregion

        //                var skuIds = new List<long>();
        //                var derivedSkus = new List<ConversionFormulaDetails>();
        //                var conversionFormulas = _emamiContext.ConversionFormulas.AsNoTracking().FirstOrDefault(f => f.SkuId == inputDto.BaseSkuId && f.IsActive);
        //                if (conversionFormulas != null && conversionFormulas.ConversionFormulaDetails.IsAny())
        //                {
        //                    derivedSkus = conversionFormulas.ConversionFormulaDetails.ToList();
        //                    skuIds = conversionFormulas.ConversionFormulaDetails.Select(s => s.SkuId).ToList();
        //                    skuIds.Add(inputDto.BaseSkuId);
        //                }
        //                else
        //                {
        //                    return _resultService.ErrorMessageWitObject(skuOutputDto, Constants.BaseSkuEmpty);
        //                }

        //                #region BaseDetails

        //                //var skuDatas = _emamiContext.Skus.AsNoTracking().Where(w => skuIds.Contains(w.Id))
        //                //    .Select(s => s);

        //                var SkuUomMappingDatas = _emamiContext.SkuUomMapping
        //                .Where(_ => skuIds.Contains(_.SkuId) && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)(int)DTO.Enums.Uom.Nos)
        //                .Select(s => new
        //                {
        //                    SkuId = s.SkuId,
        //                    UomId = s.UomId,
        //                    RelationUomId = s.RelationUomId,
        //                    ConversionFactor = s.ConversionFactor
        //                }).ToList();

        //                #endregion                        

        //                //#region GP Benefits
        //                //var baseSkuIds = skuIds;
        //                //var GpBenefitGeography = _emamiContext.GPBenefitGeography.AsNoTracking()
        //                //        .Join(_emamiContext.GPBenefitGeographyMappings.AsNoTracking(), g => g.Id, gd => gd.GPBenefitGeographyId, (g, gd) => new { Geography = g, GeographyDetail = gd })
        //                //        .Where(f => DbFunctions.TruncateTime(currentdate) <= DbFunctions.TruncateTime(f.Geography.ValidTo)
        //                //        && DbFunctions.TruncateTime(f.Geography.ValidFrom) <= DbFunctions.TruncateTime(currentdate)
        //                //        && f.GeographyDetail.CustomerId == dealerContext.Id
        //                //        && f.GeographyDetail.CityId == dealerContext.CityId
        //                //        && baseSkuIds.Contains(f.GeographyDetail.SkuId)
        //                //        && f.GeographyDetail.IsActive);

        //                //var GpBenefitUser = _emamiContext.GPBenefitUsers.AsNoTracking()
        //                //        .Join(_emamiContext.GPBenefitUserMappings.AsNoTracking(), g => g.Id, gd => gd.GPBenefitUserId, (g, gd) => new { User = g, UserDetail = gd })
        //                //        .Where(f => DbFunctions.TruncateTime(currentdate) <= DbFunctions.TruncateTime(f.User.ValidTo)
        //                //        && DbFunctions.TruncateTime(f.User.ValidFrom) <= DbFunctions.TruncateTime(currentdate)
        //                //        && f.UserDetail.CustomerId == dealerContext.Id
        //                //        && baseSkuIds.Contains(f.UserDetail.SkuId)
        //                //        && f.UserDetail.IsActive);
        //                //#endregion

        //                //#region SKU Discount
        //                //var SkuDiscountGeographyDatas = _emamiContext.SkuDiscountGeography.AsNoTracking()
        //                //                    .Join(_emamiContext.SkuDiscountGeographyMappings.AsNoTracking(), s => s.Id, sd => sd.SkuDiscountGeographyId, (s, sd) => new { SkuDiscount = s, SkuDiscountGeography = sd })
        //                //                    .Where(f => DbFunctions.TruncateTime(currentdate) <= DbFunctions.TruncateTime(f.SkuDiscount.ValidTo)
        //                //                    && DbFunctions.TruncateTime(f.SkuDiscount.ValidFrom) <= DbFunctions.TruncateTime(currentdate)
        //                //                    && skuIds.Contains(f.SkuDiscountGeography.SkuId)
        //                //                    && f.SkuDiscountGeography.CustomerId == dealerContext.Id
        //                //                    && f.SkuDiscountGeography.CityId == dealerContext.CityId
        //                //                    && f.SkuDiscountGeography.IsActive)
        //                //                    .Select(s => new
        //                //                    {
        //                //                        SkuId = s.SkuDiscountGeography.SkuId,
        //                //                        Discount = s.SkuDiscount.Discount
        //                //                    }).ToList();

        //                //var SkuDiscountUserDatas = _emamiContext.SkuDiscountUsers.AsNoTracking()
        //                //        .Join(_emamiContext.SkuDiscountUserMappings.AsNoTracking(), s => s.Id, sd => sd.SkuDiscountUserId, (s, sd) => new { SkuDiscount = s, SkuUserDiscount = sd })
        //                //        .Where(f => DbFunctions.TruncateTime(currentdate) <= DbFunctions.TruncateTime(f.SkuDiscount.ValidTo)
        //                //        && DbFunctions.TruncateTime(f.SkuDiscount.ValidFrom) <= DbFunctions.TruncateTime(currentdate)
        //                //        && skuIds.Contains(f.SkuUserDiscount.SkuId)
        //                //        && f.SkuUserDiscount.CustomerId == dealerContext.Id
        //                //        && f.SkuUserDiscount.IsActive)
        //                //        .Select(s => new
        //                //        {
        //                //            SkuId = s.SkuUserDiscount.SkuId,
        //                //            Discount = s.SkuDiscount.Discount
        //                //        }).ToList();
        //                //#endregion

        //                #region SCHEME Discount
        //                var SchemeDiscountGeographyDatas = _emamiContext.SchemeDiscountGeography.AsNoTracking()
        //                                    .Join(_emamiContext.SchemeDiscountGeographyMappings.AsNoTracking(), s => s.Id, sd => sd.SchemeDiscountGeographyId, (s, sd) => new { SchemeDiscount = s, SchemeDiscountGeography = sd })
        //                                    .Where(f => DbFunctions.TruncateTime(currentdate) <= DbFunctions.TruncateTime(f.SchemeDiscount.ValidTo)
        //                                    && DbFunctions.TruncateTime(f.SchemeDiscount.ValidFrom) <= DbFunctions.TruncateTime(currentdate)
        //                                    && skuIds.Contains(f.SchemeDiscountGeography.SkuId)
        //                                    && f.SchemeDiscountGeography.CustomerId == dealerContext.Id
        //                                    && f.SchemeDiscountGeography.CityId == dealerContext.CityId
        //                                    && f.SchemeDiscountGeography.IsActive)
        //                                    .Select(s => new
        //                                    {
        //                                        SkuId = s.SchemeDiscountGeography.SkuId,
        //                                        Discount = s.SchemeDiscount.Discount
        //                                    }).ToList();

        //                //var SchemeDiscountUserDatas = _emamiContext.SchemeDiscountUsers.AsNoTracking()
        //                //        .Join(_emamiContext.SchemeDiscountUserMappings.AsNoTracking(), s => s.Id, sd => sd.SchemeDiscountUserId, (s, sd) => new { SchemeDiscount = s, SchemeDiscountUser = sd })
        //                //        .Where(f => DbFunctions.TruncateTime(currentdate) <= DbFunctions.TruncateTime(f.SchemeDiscount.ValidTo)
        //                //        && DbFunctions.TruncateTime(f.SchemeDiscount.ValidFrom) <= DbFunctions.TruncateTime(currentdate)
        //                //        && skuIds.Contains(f.SchemeDiscountUser.SkuId)
        //                //        && f.SchemeDiscountUser.CustomerId == dealerContext.Id
        //                //        && f.SchemeDiscountUser.IsActive)
        //                //        .Select(s => new
        //                //        {
        //                //            SkuId = s.SchemeDiscountUser.SkuId,
        //                //            Discount = s.SchemeDiscount.Discount
        //                //        }).ToList();
        //                #endregion

        //                decimal loadability = 0;
        //                if (inputDto.IncotermId == (int)DTO.Enums.IncoTerms.ForPlant || inputDto.IncotermId == (int)DTO.Enums.IncoTerms.ExPlant)
        //                {
        //                    loadability = dealerContext.Loadability;
        //                }
        //                else if (inputDto.IncotermId == (int)DTO.Enums.IncoTerms.ForDepot || inputDto.IncotermId == (int)DTO.Enums.IncoTerms.ExDepot)
        //                {
        //                    loadability = dealerContext.DepotLoadability;
        //                }
        //                //var plantcode = _emamiContext.Depots.FirstOrDefault(_ => _.Id == saudaOrder.PlantId).Code;

        //                foreach (var derivedSku in derivedSkus)
        //                {
        //                    var isDerivedSkuPriceGenerated = _emamiContext.TodayPricing.AsNoTracking()
        //                        .FirstOrDefault(_ => _.SkuId == derivedSku.Sku.Id
        //                     // && _.OilPackingTypeId == derivedSku.Sku.PackGroupId && _.OilTypeId == saudaOrder.OilTypeId && _.StateId == (long)dealerContext.StateId && _.FrieghtZoneId == (long)dealerContext.FreightZoneId &&
        //                     //_.FrieghtRouteId == (long)dealerContext.FreightRouteId && _.TransportModeId == (long)dealerContext.TransportModeId
        //                     && _.PlantId == inputDto.PlantId// || _.DepotCode == plantcode)
        //                                                     //&& _.LoadQuantity == loadability && _.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction && _.BiddingWindowId == saudaOrder.BiddingwindowId
        //                     );
        //                    long gpBenefitType = 0;
        //                    long gpBenefitAppliedType = 0;
        //                    string gpBenefitCategoryType = "";
        //                    long gpBenefitCategoryTypeId = 0;
        //                    decimal gpBenefitDiscountOrDays = 0;

        //                    if (isDerivedSkuPriceGenerated != null)
        //                    {
        //                        //#region GP Benefits

        //                        //if (saudaOrder.GPBenefitType > 0)
        //                        //{
        //                        //    if (GpBenefitGeography.IsAny())
        //                        //    {
        //                        //        var geography = GpBenefitGeography.FirstOrDefault(f => f.GeographyDetail.SkuId == derivedSku.SkuId);
        //                        //        if (geography != null)
        //                        //        {
        //                        //            if (geography.Geography.BenefitTypesId == (int)DTO.Enums.BenefitType.SAP)
        //                        //            {
        //                        //                gpBenefitCategoryTypeId = geography.Geography.BenefitOrCategoryId;
        //                        //                gpBenefitCategoryType = geography.Geography.BenefitOrCategoryId > 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.BenefitCategory)Convert.ToInt32(geography.Geography.BenefitOrCategoryId)) : string.Empty;
        //                        //            }
        //                        //            else
        //                        //            {
        //                        //                gpBenefitCategoryTypeId = geography.Geography.BenefitOrCategoryId;
        //                        //                var nonsapBenefit = _emamiContext.Benefits.AsNoTracking().FirstOrDefault(_ => _.Id == geography.Geography.BenefitOrCategoryId);
        //                        //                gpBenefitCategoryType = nonsapBenefit.BenefitCategory;
        //                        //            }
        //                        //            gpBenefitType = geography.Geography.BenefitTypesId;
        //                        //            gpBenefitAppliedType = (int)DTO.Enums.RaDiscountType.Geography;
        //                        //            gpBenefitDiscountOrDays = geography.Geography.DiscountOrDays;
        //                        //        }
        //                        //        else if (GpBenefitUser.IsAny())
        //                        //        {
        //                        //            var userBenefit = GpBenefitUser.FirstOrDefault(f => f.UserDetail.SkuId == derivedSku.SkuId);
        //                        //            if (userBenefit != null)
        //                        //            {
        //                        //                if (userBenefit.User.BenefitTypesId == (int)DTO.Enums.BenefitType.SAP)
        //                        //                {
        //                        //                    gpBenefitCategoryTypeId = userBenefit.User.BenefitOrCategoryId;
        //                        //                    gpBenefitCategoryType = userBenefit.User.BenefitOrCategoryId > 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.BenefitCategory)Convert.ToInt32(userBenefit.User.BenefitOrCategoryId)) : string.Empty;
        //                        //                }
        //                        //                else
        //                        //                {
        //                        //                    gpBenefitCategoryTypeId = userBenefit.User.BenefitOrCategoryId;
        //                        //                    var nonsapBenefit = _emamiContext.Benefits.AsNoTracking().FirstOrDefault(_ => _.Id == userBenefit.User.BenefitOrCategoryId);
        //                        //                    gpBenefitCategoryType = nonsapBenefit.BenefitCategory;
        //                        //                }
        //                        //                gpBenefitType = userBenefit.User.BenefitTypesId;
        //                        //                gpBenefitAppliedType = (int)DTO.Enums.RaDiscountType.User;
        //                        //                gpBenefitDiscountOrDays = userBenefit.User.DiscountOrDays;
        //                        //            }
        //                        //        }
        //                        //    }
        //                        //    else if (GpBenefitUser.IsAny())
        //                        //    {
        //                        //        var userBenefit = GpBenefitUser.FirstOrDefault(f => f.UserDetail.SkuId == derivedSku.SkuId);
        //                        //        if (userBenefit != null)
        //                        //        {
        //                        //            if (userBenefit.User.BenefitTypesId == (int)DTO.Enums.BenefitType.SAP)
        //                        //            {
        //                        //                gpBenefitCategoryTypeId = userBenefit.User.BenefitOrCategoryId;
        //                        //                gpBenefitCategoryType = userBenefit.User.BenefitOrCategoryId > 0 ? UtilityHelper.GetEnumDescription((DTO.Enums.BenefitCategory)Convert.ToInt32(userBenefit.User.BenefitOrCategoryId)) : string.Empty;
        //                        //            }
        //                        //            else
        //                        //            {
        //                        //                gpBenefitCategoryTypeId = userBenefit.User.BenefitOrCategoryId;
        //                        //                var nonsapBenefit = _emamiContext.Benefits.AsNoTracking().FirstOrDefault(_ => _.Id == userBenefit.User.BenefitOrCategoryId);
        //                        //                gpBenefitCategoryType = nonsapBenefit.BenefitCategory;
        //                        //            }
        //                        //            gpBenefitType = userBenefit.User.BenefitTypesId;
        //                        //            gpBenefitAppliedType = (int)DTO.Enums.RaDiscountType.User;
        //                        //            gpBenefitDiscountOrDays = userBenefit.User.DiscountOrDays;
        //                        //        }
        //                        //    }
        //                        //}
        //                        //#endregion

        //                        #region SKU Weight

        //                        var skuDetail = skuDatas.FirstOrDefault(f => f.Id == derivedSku.SkuId);
        //                        SKUWiseWeight = 0;
        //                        if (skuDetail.SkuUom == DTO.Enums.Uom.Ltr.ToString())
        //                        {
        //                            var SkuUomMappingContext = SkuUomMappingDatas.FirstOrDefault(_ => _.SkuId == derivedSku.SkuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)(int)DTO.Enums.Uom.Nos);
        //                            if (SkuUomMappingContext != null)
        //                            {
        //                                SKUWiseWeight = skuDetail.LitreConversion > 0 ? (skuDetail.Quantity * 1000 * SkuUomMappingContext.ConversionFactor) / skuDetail.LitreConversion : 0;
        //                            }
        //                            else
        //                            {
        //                                SKUWiseWeight = skuDetail.LitreConversion > 0 ? (skuDetail.Quantity * 1000) / skuDetail.LitreConversion : 0;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            SKUWiseWeight = skuDetail.Quantity;
        //                        }

        //                        #endregion

        //                        BiddingCartSku = new BiddingCartSkuOutputDto
        //                        {
        //                            SkuId = derivedSku.SkuId,
        //                            SkuName = skuDatas.IsAny() ? skuDatas.FirstOrDefault(f => f.Id == derivedSku.SkuId).Name : string.Empty,
        //                            OilTypeId = saudaOrder.OilTypeId,
        //                            OilType = oilTypeDatas.IsAny() ? oilTypeDatas.FirstOrDefault(f => f.Id == saudaOrder.OilTypeId).Name : string.Empty,
        //                            IncotermId = inputDto.IncotermId,
        //                            IncotermName = incoTermData.Name,
        //                            PlantId = saudaOrder.PlantId,
        //                            PlantName = plantData.Name,


        //                            //GuaranteePrice = Utility.StringToDouble(derivedSku.Formula, saudaOrder.BidPricePerCase, ConsoleSettings.SkuReplace),
        //                            BaseRate = saudaOrder.BaseRate,

        //                            //FreightRouteId = (int)dealerContext.FreightRouteId,
        //                            PricingId = saudaOrder.PricingId,

        //                            GPBenefitType = gpBenefitType,
        //                            GPBenefitAppliedTypeId = gpBenefitAppliedType,
        //                            GPBenefitOrCategoryId = gpBenefitCategoryTypeId,
        //                            GPBenefitOrCategory = gpBenefitCategoryType,
        //                            GPBenefitDiscountOrDay = gpBenefitDiscountOrDays,

        //                            //DepotName = depotDatas.IsAny() ? depotDatas.FirstOrDefault(f => f.Id == pricing.DepotId).Name : string.Empty,
        //                            //GuaranteePrice = saudaOrder.BidPricePerCase,
        //                            //FreightRouteName = FreightRoutesDatas.IsAny() ? FreightRoutesDatas.FirstOrDefault(f => f.Id == pricing.FrieghtRouteId).Name : string.Empty,
        //                            //DepotId = saudaOrder.DepotId,

        //                            SkuWeightPerCase = Math.Round(SKUWiseWeight, 3),
        //                            TodayPricingId = isDerivedSkuPriceGenerated.Id
        //                        };

        //                        //#region SKU Discount
        //                        //skuDiscount = 0;
        //                        //skuDiscountType = 0;

        //                        ////if (SkuDiscountGeographyDatas.IsAny())
        //                        ////{
        //                        ////    var discountData = SkuDiscountGeographyDatas.FirstOrDefault(f => f.SkuId == derivedSku.SkuId);
        //                        ////    if (discountData != null)
        //                        ////    {
        //                        ////        skuDiscount = discountData.Discount;
        //                        ////        skuDiscountType = (int)DTO.Enums.RaDiscountType.Geography;
        //                        ////    }
        //                        ////}

        //                        ////if (SkuDiscountUserDatas.IsAny() && skuDiscountType == 0)
        //                        ////{
        //                        ////    var discountData = SkuDiscountUserDatas.FirstOrDefault(f => f.SkuId == derivedSku.SkuId);
        //                        ////    if (discountData != null)
        //                        ////    {
        //                        ////        skuDiscount = discountData.Discount;
        //                        ////        skuDiscountType = (int)DTO.Enums.RaDiscountType.User;
        //                        ////    }
        //                        ////}
        //                        ////BiddingCartSku.SkuDiscount = SkuDiscountUsers(pricing.SkuId, dealerContext.Id, currentdate);
        //                        //BiddingCartSku.SkuDiscount = skuDiscount;
        //                        //#endregion

        //                        #region SCHEME Discount
        //                        schemeDiscount = 0;
        //                        schemeDiscountType = 0;

        //                        if (SchemeDiscountGeographyDatas.IsAny())
        //                        {
        //                            var discountData = SchemeDiscountGeographyDatas.FirstOrDefault(f => f.SkuId == derivedSku.SkuId);
        //                            if (discountData != null)
        //                            {
        //                                schemeDiscount = discountData.Discount;
        //                                schemeDiscountType = (int)DTO.Enums.RaDiscountType.Geography;
        //                            }
        //                        }

        //                        //if (SchemeDiscountUserDatas.IsAny() && schemeDiscountType == 0)
        //                        //{
        //                        //    var discountData = SchemeDiscountUserDatas.FirstOrDefault(f => f.SkuId == derivedSku.SkuId);
        //                        //    if (discountData != null)
        //                        //    {
        //                        //        schemeDiscount = discountData.Discount;
        //                        //        schemeDiscountType = (int)DTO.Enums.RaDiscountType.User;
        //                        //    }
        //                        //}

        //                        BiddingCartSku.SchemeDiscount = schemeDiscount;
        //                        #endregion

        //                        //var DerivedVolumeDatas = VolumeDiscountUsers(derivedSku.SkuId, inputDto.DealerId, currentdate, dealerContext.CityId);
        //                        //BiddingCartSku.VolumeDiscount = DerivedVolumeDatas;
        //                        BiddingCartSku.CaseToMTValue = _resultService.ConvertCasetoMetricTon(1, derivedSku.SkuId);

        //                        BiddingCartSku.SkuDiscountType = skuDiscountType;
        //                        BiddingCartSku.SchemeDiscountType = schemeDiscountType;
        //                        //BiddingCartSku.VolumeDiscountType = DerivedVolumeDatas.VolumeDiscountType;

        //                        skuOutputDto.Add(BiddingCartSku);
        //                    }
        //                }
        //            }
        //        }

        //        return _resultService.SuccessObject(skuOutputDto);
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        //public ResultDto SaudaAllocationSkuDetailsOld(BiddingCartSkuInputDto inputDto)
        //{
        //    _methodName = "SaudaAllocationSkuDetailsOld";
        //    var skuOutputDto = new List<BiddingCartSkuOutputDto>();

        //    if (inputDto == null)
        //    {
        //        return _resultService.ErrorMessage(Constants.InvalidRequest);
        //    }
        //    if (inputDto.OilTypeIds == null)
        //    {
        //        return _resultService.ErrorMessage(Constants.OilTypeMissing);
        //    }
        //    if (inputDto.BiddingWindowId == 0)
        //    {
        //        return _resultService.ErrorMessage(Constants.BiddingWindowisMissing);
        //    }

        //    try
        //    {
        //        DateTime currentdate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //        decimal skuDiscount = 0;
        //        decimal schemeDiscount = 0;
        //        int skuDiscountType = 0;
        //        int schemeDiscountType = 0;
        //        var skuIds = new List<long>();

        //        var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
        //        if (dealerContext != null)
        //        {
        //            var BiddingWindowContext = _emamiContext.BiddingWindow.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.BiddingWindowId);
        //            if (BiddingWindowContext != null)
        //            {
        //                List<long?> oilTypeId = new List<long?>();
        //                inputDto.OilTypeIds.ForEach(f => oilTypeId.Add(f));

        //                #region Get Common Data's
        //                var oilTypeDatas = _emamiContext.OilTypes.AsNoTracking().Where(w => inputDto.OilTypeIds.Contains(w.Id))
        //                                                   .Select(s => new
        //                                                   {
        //                                                       Id = s.Id,
        //                                                       Name = s.Name
        //                                                   }).ToList();
        //                if (oilTypeDatas.IsNotAny())
        //                {
        //                    return _resultService.ErrorMessage(Constants.OilTypeNotFound);
        //                }

        //                var skuDatas = _emamiContext.Skus.AsNoTracking()
        //                    .Where(w => oilTypeId.Contains(w.OilTypeId))  //&& !w.IsBaseSku
        //                   .Select(s => new
        //                   {
        //                       Id = s.Id,
        //                       Name = s.SkuName
        //                   }).ToList();
        //                if (skuDatas.IsNotAny())
        //                {
        //                    return _resultService.ErrorMessage(Constants.BaseSkuEmpty);
        //                }

        //                var conversionFormulas = _emamiContext.ConversionFormulas.AsNoTracking().FirstOrDefault(f => f.SkuId == inputDto.BaseSkuId && f.IsActive);
        //                if (conversionFormulas != null && conversionFormulas.ConversionFormulaDetails.IsAny())
        //                {
        //                    skuIds = conversionFormulas.ConversionFormulaDetails.Select(s => s.SkuId).ToList();
        //                    skuIds.Add(inputDto.BaseSkuId);
        //                }
        //                else
        //                {
        //                    return _resultService.ErrorMessage(Constants.BaseSkuEmpty);
        //                }

        //                var pricingContext = _emamiContext.Pricing.AsNoTracking()
        //                .Where(_ => //_.BiddingWindowId == inputDto.BiddingWindowId
        //                //&& inputDto.OilTypeIds.Contains(_.OilTypeId)
        //                 skuIds.Contains(_.SkuId)
        //                && _.PlantId == inputDto.PlantId
        //                //&& _.FrieghtRouteId == dealerContext.FreightRouteId
        //                )
        //                .Select(s => new
        //                {
        //                    Id = s.Id,
        //                    //OilTypeId = s.OilTypeId,
        //                    SkuId = s.SkuId,
        //                    PlantId = s.PlantId,
        //                    //FrieghtRouteId = s.FrieghtRouteId,
        //                    //DepotId = s.DepotId,
        //                    //LoadQuantity = s.LoadQuantity,
        //                    //ExPlantGuaranteePrice = s.ExPlantGuaranteePrice,
        //                    //ForPlantGuaranteePrice = s.ForPlantGuaranteePrice,
        //                    //ExDepotGuaranteePrice = s.ExDepotGuaranteePrice,
        //                    //ForDepotGuaranteePrice = s.ForDepotGuaranteePrice,
        //                    //ExRakeGuaranteePrice = s.ExRakeGuaranteePrice,
        //                    //ForRakeGuaranteePrice = s.ForRakeGuaranteePrice,
        //                    //ExPlantPrice = s.ExPlantPrice,
        //                    //ForPlantPrice = s.ForPlantPrice,
        //                    //ExDepotPrice = s.ExDepotPrice,
        //                    //ForDepotPrice = s.ForDepotPrice,
        //                    //ExRakePrice = s.ExRakePrice,
        //                    //ForRakePrice = s.ForRakePrice
        //                });
        //                #endregion

        //                if (pricingContext.IsAny())
        //                {
        //                    if (pricingContext != null)
        //                    {

        //                        #region Get Common Data's

        //                        var plantData = _emamiContext.Depots.AsNoTracking()
        //                            .FirstOrDefault(f => f.Id == inputDto.PlantId && f.StorageTypeId == (int)DTO.Enums.StorageType.Plant);

        //                        var incoTermData = _emamiContext.IncoTerms.AsNoTracking()
        //                            .FirstOrDefault(f => f.Id == inputDto.IncotermId);

        //                        var baseSkuIds = pricingContext.Select(s => s.SkuId).Distinct().ToList();

        //                        #endregion


        //                        #region SCHEME Discount
        //                        var SchemeDiscountGeographyDatas = _emamiContext.SchemeDiscountGeography.AsNoTracking()
        //                                            .Join(_emamiContext.SchemeDiscountGeographyMappings.AsNoTracking(), s => s.Id, sd => sd.SchemeDiscountGeographyId, (s, sd) => new { SchemeDiscount = s, SchemeDiscountGeography = sd })
        //                                            .Where(f => DbFunctions.TruncateTime(currentdate) <= DbFunctions.TruncateTime(f.SchemeDiscount.ValidTo)
        //                                            && DbFunctions.TruncateTime(f.SchemeDiscount.ValidFrom) <= DbFunctions.TruncateTime(currentdate)
        //                                            && skuIds.Contains(f.SchemeDiscountGeography.SkuId)
        //                                            && f.SchemeDiscountGeography.CustomerId == dealerContext.Id
        //                                            && f.SchemeDiscountGeography.CityId == dealerContext.CityId
        //                                            && f.SchemeDiscountGeography.IsActive)
        //                                            .Select(s => new
        //                                            {
        //                                                SkuId = s.SchemeDiscountGeography.SkuId,
        //                                                Discount = s.SchemeDiscount.Discount
        //                                            }).ToList();

        //                        //var SchemeDiscountUserDatas = _emamiContext.SchemeDiscountUsers.AsNoTracking()
        //                        //        .Join(_emamiContext.SchemeDiscountUserMappings.AsNoTracking(), s => s.Id, sd => sd.SchemeDiscountUserId, (s, sd) => new { SchemeDiscount = s, SchemeDiscountUser = sd })
        //                        //        .Where(f => DbFunctions.TruncateTime(currentdate) <= DbFunctions.TruncateTime(f.SchemeDiscount.ValidTo)
        //                        //        && DbFunctions.TruncateTime(f.SchemeDiscount.ValidFrom) <= DbFunctions.TruncateTime(currentdate)
        //                        //        && skuIds.Contains(f.SchemeDiscountUser.SkuId)
        //                        //        && f.SchemeDiscountUser.CustomerId == dealerContext.Id
        //                        //        && f.SchemeDiscountUser.IsActive)
        //                        //        .Select(s => new
        //                        //        {
        //                        //            SkuId = s.SchemeDiscountUser.SkuId,
        //                        //            Discount = s.SchemeDiscount.Discount
        //                        //        }).ToList();
        //                        #endregion

        //                        foreach (var pricing in pricingContext.ToList())
        //                        {
        //                            long gpBenefitType = 0;
        //                            long gpBenefitAppliedType = 0;
        //                            string gpBenefitCategoryType = "";
        //                            long gpBenefitCategoryTypeId = 0;
        //                            decimal gpBenefitDiscountOrDays = 0;
        //                            decimal gpBenefitDiscountCase = 0;
        //                            decimal gpDiscount = 0;

        //                            var guaranteePrice = 0;
        //                            var baseRate = 0;

        //                            if (skuOutputDto.IsAny())
        //                            {
        //                                bool isExistSku = skuOutputDto.Any(f => f.SkuId == pricing.SkuId
        //                                 && f.GuaranteePrice == guaranteePrice
        //                                 && f.PlantId == pricing.PlantId
        //                                 );
        //                                if (isExistSku)
        //                                {
        //                                    continue;
        //                                }
        //                            }

        //                            var BiddingCartSku = new BiddingCartSkuOutputDto
        //                            {
        //                                PricingId = pricing.Id,
        //                                SkuId = pricing.SkuId,
        //                                //OilTypeId = pricing.OilTypeId,
        //                                //OilType = oilTypeDatas.IsAny() ? oilTypeDatas.FirstOrDefault(f => f.Id == pricing.OilTypeId).Name : string.Empty,  //pricing.OilType.Name,
        //                                IncotermId = inputDto.IncotermId,
        //                                PlantId = pricing.PlantId,
        //                                SkuName = skuDatas.IsAny() ? skuDatas.FirstOrDefault(f => f.Id == pricing.SkuId).Name : string.Empty,          //pricing.Sku.SkuName,
        //                                GuaranteePrice = guaranteePrice,
        //                                BaseRate = baseRate,
        //                                IncotermName = incoTermData.Name,              //_emamiContext.IncoTerms.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.IncotermId).Name,
        //                                PlantName = plantData.Name,                    //_emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == pricing.PlantId && _.IsPlant).Name,
        //                                GPBenefitType = gpBenefitType,
        //                                GPBenefitAppliedTypeId = gpBenefitAppliedType,
        //                                GPBenefitOrCategoryId = gpBenefitCategoryTypeId,
        //                                GPBenefitOrCategory = gpBenefitCategoryType,
        //                                GPBenefitDiscountOrDay = gpBenefitDiscountOrDays
        //                            };

        //                            #region SKU Discount
        //                            skuDiscount = 0;
        //                            skuDiscountType = 0;

        //                            //if (SkuDiscountGeographyDatas.IsAny())
        //                            //{
        //                            //    var discountData = SkuDiscountGeographyDatas.FirstOrDefault(f => f.SkuId == pricing.SkuId);
        //                            //    if (discountData != null)
        //                            //    {
        //                            //        skuDiscount = discountData.Discount;
        //                            //        skuDiscountType = (int)DTO.Enums.RaDiscountType.Geography;
        //                            //    }
        //                            //}

        //                            //if (SkuDiscountUserDatas.IsAny() && skuDiscountType == 0)
        //                            //{
        //                            //    var discountData = SkuDiscountUserDatas.FirstOrDefault(f => f.SkuId == pricing.SkuId);
        //                            //    if (discountData != null)
        //                            //    {
        //                            //        skuDiscount = discountData.Discount;
        //                            //        skuDiscountType = (int)DTO.Enums.RaDiscountType.User;
        //                            //    }
        //                            //}
        //                            //BiddingCartSku.SkuDiscount = SkuDiscountUsers(pricing.SkuId, dealerContext.Id, currentdate);
        //                            BiddingCartSku.SkuDiscount = skuDiscount;
        //                            #endregion

        //                            #region SCHEME Discount
        //                            schemeDiscount = 0;
        //                            schemeDiscountType = 0;

        //                            if (SchemeDiscountGeographyDatas.IsAny())
        //                            {
        //                                var discountData = SchemeDiscountGeographyDatas.FirstOrDefault(f => f.SkuId == pricing.SkuId);
        //                                if (discountData != null)
        //                                {
        //                                    schemeDiscount = discountData.Discount;
        //                                    schemeDiscountType = (int)DTO.Enums.RaDiscountType.Geography;
        //                                }
        //                            }

        //                            //if (SchemeDiscountUserDatas.IsAny() && schemeDiscountType == 0)
        //                            //{
        //                            //    var discountData = SchemeDiscountUserDatas.FirstOrDefault(f => f.SkuId == pricing.SkuId);
        //                            //    if (discountData != null)
        //                            //    {
        //                            //        schemeDiscount = discountData.Discount;
        //                            //        schemeDiscountType = (int)DTO.Enums.RaDiscountType.User;
        //                            //    }
        //                            //}
        //                            //BiddingCartSku.SchemeDiscount = SchemeDiscountUsers(pricing.SkuId, inputDto.DealerId, currentdate);
        //                            BiddingCartSku.SchemeDiscount = schemeDiscount;
        //                            #endregion
        //                            BiddingCartSku.CaseToMTValue = _resultService.ConvertCasetoMetricTon(1, pricing.SkuId);

        //                            BiddingCartSku.SkuDiscountType = skuDiscountType;
        //                            BiddingCartSku.SchemeDiscountType = schemeDiscountType;
        //                            skuOutputDto.Add(BiddingCartSku);
        //                        }
        //                        if (skuOutputDto != null)
        //                        {
        //                            skuOutputDto = skuOutputDto.Where(_ => _.GuaranteePrice != 0).ToList();
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        return _resultService.SuccessObject(skuOutputDto);
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        _logger.Error(message);
        //        return _resultService.ErrorMessage(Constants.Exception);
        //    }
        //}

        //public ResultDto GetSaudaAllocationListForDealer(SaudaFilterDto inputDto)
        //{
        //    _methodName = "GetSaudaAllocationList";

        //    try
        //    {
        //        var saudaAllocationListDto = new List<SaudaAllocatedListDto>();
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }
        //        if (inputDto.DealerId == 0)
        //        {
        //            return _resultService.ErrorMessage(Constants.DealerMissing);
        //        }
        //        var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
        //        if (dealerContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.DealerNotFound);
        //        }

        //        var saudaOrderListContext = _emamiContext.Sauda.AsNoTracking().Where(_ => _.UserId == inputDto.DealerId)
        //            .Join(_emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.IsSaudaAllocated), s => s.Id, so => so.SaudaId, (s, so) => new { Sauda = s, SaudaOrder = so })
        //            .Where(_ => _.Sauda != null && _.SaudaOrder != null).OrderByDescending(_ => _.SaudaOrder.Id).ToList();

        //        if (saudaOrderListContext != null && saudaOrderListContext.Any())
        //        {
        //            saudaAllocationListDto = saudaOrderListContext.Select(_ => new SaudaAllocatedListDto
        //            {
        //                SaudaId = _.SaudaOrder != null ? _.SaudaOrder.Id : 0,
        //                SaudaOrderId = _.SaudaOrder != null ? _.SaudaOrder.Id : 0,
        //                SaudaNumber = _.SaudaOrder != null ? _.SaudaOrder.SaudaNumber : string.Empty,
        //                BidQuantity = _.SaudaOrder != null ? _.SaudaOrder.BidQuantity : 0,
        //                BidQuantityCase = _.SaudaOrder != null ? _.SaudaOrder.BidQuantityCase : 0,
        //                BookedDate = _.Sauda.BiddingDate,
        //                DealerId = _.Sauda.UserId,
        //                DealerName = _.Sauda.UserId > 0 ? _emamiContext.Users.AsNoTracking().FirstOrDefault(u => u.Id == _.Sauda.UserId)?.Name : string.Empty,
        //                TotalQuantity = saudaOrderListContext.Sum(s => s.SaudaOrder.BidQuantityCase),
        //                TotalAmount = _.SaudaOrder.BidPrice,
        //                StatusId = _.Sauda != null ? _.SaudaOrder.StatusId : 0,
        //                StatusName = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(f => f.Id == _.SaudaOrder.StatusId)?.Name,
        //                OilTypeId = _.SaudaOrder.OilTypeId,
        //                OilTypeName = _.SaudaOrder.OilType != null ? _.SaudaOrder.OilType.Name : string.Empty,
        //                SkuId = _.SaudaOrder.SkuId,
        //                SkuName = _.SaudaOrder?.Sku?.SkuName,
        //                //OilTypes = saudaOrderListContext.GroupBy(g => g.SaudaOrder.OilTypeId).Select(s => new SpecialRateOilTypeDto
        //                //{
        //                //    OilTypeId = s.FirstOrDefault().SaudaOrder.OilTypeId,
        //                //    OilTypeName = s.FirstOrDefault()?.SaudaOrder.OilType != null ? s.FirstOrDefault()?.SaudaOrder.OilType.Name : string.Empty,
        //                //    SkuCount = s.Count(),
        //                //}).ToList(),
        //            }).ToList();
        //        }
        //        if (saudaAllocationListDto != null && saudaAllocationListDto.Any())
        //        {
        //            return _resultService.SuccessObject(saudaAllocationListDto);
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

        //public ResultDto GetSaudaAllocationListForBDO(SaudaFilterDto inputDto)
        //{
        //    _methodName = "GetSaudaAllocationListForBDO";

        //    try
        //    {
        //        var saudaAllocationListDto = new List<SaudaAllocatedListDto>();
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }
        //        if (inputDto.UserId == 0)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserIdMissing);
        //        }
        //        var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.UserId);
        //        if (userContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserNotFound);
        //        }

        //        var userRoleContext = _emamiContext.UserRoles.AsNoTracking().FirstOrDefault(_ => _.UserId == userContext.Id && _.RoleId == (int)DTO.Enums.Role.StateTrader);
        //        if (userRoleContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.UserNotFound);
        //        }

        //        IQueryable<UserCustomerMapping> dealersList = _emamiContext.UserCustomerMapping.AsNoTracking().Where(_ => _.UserId == inputDto.UserId);
        //        if (dealersList != null && dealersList.Any())
        //        {
        //            var saudaOrderListContext = _emamiContext.Sauda.AsNoTracking().Where(_ => dealersList.Any(a => a.CustomerId == _.UserId))
        //            .Join(_emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.IsSaudaAllocated), s => s.Id, so => so.SaudaId, (s, so) => new { Sauda = s, SaudaOrder = so })
        //            .Where(_ => _.Sauda != null && _.SaudaOrder != null).OrderByDescending(_ => _.SaudaOrder.Id).ToList();

        //            if (saudaOrderListContext != null && saudaOrderListContext.Any())
        //            {
        //                saudaAllocationListDto = saudaOrderListContext.Select(_ => new SaudaAllocatedListDto
        //                {
        //                    SaudaId = _.SaudaOrder != null ? _.SaudaOrder.Id : 0,
        //                    SaudaOrderId = _.SaudaOrder != null ? _.SaudaOrder.Id : 0,
        //                    SaudaNumber = _.SaudaOrder != null ? _.SaudaOrder.SaudaNumber : string.Empty,
        //                    BidQuantity = _.SaudaOrder != null ? _.SaudaOrder.BidQuantity : 0,
        //                    BidQuantityCase = _.SaudaOrder != null ? _.SaudaOrder.BidQuantityCase : 0,
        //                    BookedDate = _.Sauda.BiddingDate,
        //                    DealerId = _.Sauda.UserId,
        //                    DealerName = _.Sauda.UserId > 0 ? _emamiContext.Users.AsNoTracking().FirstOrDefault(u => u.Id == _.Sauda.UserId)?.Name : string.Empty,
        //                    TotalQuantity = saudaOrderListContext.Sum(s => s.SaudaOrder.BidQuantityCase),
        //                    TotalAmount = _.SaudaOrder.BidPrice,
        //                    StatusId = _.Sauda != null ? _.SaudaOrder.StatusId : 0,
        //                    StatusName = _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(f => f.Id == _.SaudaOrder.StatusId)?.Name,
        //                    OilTypeId = _.SaudaOrder.OilTypeId,
        //                    OilTypeName = _.SaudaOrder.OilType != null ? _.SaudaOrder.OilType.Name : string.Empty,
        //                    SkuId = _.SaudaOrder.SkuId,
        //                    SkuName = _.SaudaOrder?.Sku?.SkuName,
        //                }).ToList();
        //            }
        //        }
        //        if (saudaAllocationListDto != null && saudaAllocationListDto.Any())
        //        {
        //            return _resultService.SuccessObject(saudaAllocationListDto);
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

        //public ResultDto GetSaudaAllocationDetails(SaudaAllocationInputDto inputDto)
        //{
        //    _methodName = "GetSaudaAllocationDetails";
        //    var saudaListForAllocationDto = new SaudaAllocationDetailsDto();
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.InvalidRequest);
        //        }
        //        if (inputDto.DealerId == 0)
        //        {
        //            return _resultService.ErrorMessage(Constants.DealerMissing);
        //        }
        //        var dealerContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.DealerId);
        //        if (dealerContext == null)
        //        {
        //            return _resultService.ErrorMessage(Constants.DealerNotFound);
        //        }

        //        var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
        //        var saudaContext = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.UserId == inputDto.DealerId);

        //        if (saudaContext != null)
        //        {
        //            var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().FirstOrDefault(_ => _.IsSaudaAllocated && _.Id == inputDto.SaudaOrderId && _.Sauda.UserId == inputDto.DealerId);

        //            if (saudaOrderContext != null)
        //            {
        //                saudaListForAllocationDto.SaudaId = saudaOrderContext.SaudaId;
        //                saudaListForAllocationDto.SaudaOrderId = saudaOrderContext.Id;
        //                saudaListForAllocationDto.SaudaNumber = saudaOrderContext.SaudaNumber;
        //                //saudaListForAllocationDto.BidQuantity = saudaOrderContext.BidQuantity;
        //                //saudaListForAllocationDto.BidQuantityCase = saudaOrderContext.BidQuantityCase;
        //                saudaListForAllocationDto.BiddingDate = saudaOrderContext.Sauda.BiddingDate;
        //                saudaListForAllocationDto.QuotedPrice = saudaOrderContext.QuotedPrice;
        //                saudaListForAllocationDto.DealerId = dealerContext.Id;
        //                saudaListForAllocationDto.DealerName = dealerContext?.Name;

        //                var biddingCartHeaderDetails = _emamiContext.SaudaBiddingCartHeaders.AsNoTracking().FirstOrDefault(_ => _.Id == saudaOrderContext.Sauda.RABookingId);
        //                if (biddingCartHeaderDetails != null)
        //                {

        //                    var biddingCartDetails = _emamiContext.SaudaBiddingCart.AsNoTracking()
        //                        .Where(_ => _.SaudaBiddingCartHeaderId == biddingCartHeaderDetails.Id && _.SkuId == saudaOrderContext.SkuId).ToList();
        //                    if (biddingCartDetails != null && biddingCartDetails.Any())
        //                    {
        //                        saudaListForAllocationDto.BidQuantity = biddingCartDetails.Sum(_ => _.BidQuantityInMT);
        //                        saudaListForAllocationDto.BidQuantityCase = biddingCartDetails.Sum(_ => _.BidQuantityInCase);

        //                        foreach (var item in biddingCartDetails)
        //                        {
        //                            var skuDetails = new SKUDetail()
        //                            {
        //                                BiddingCartId = item.Id,
        //                                SkuId = item.SkuId,
        //                                OilTypeId = item.OilTypeId,
        //                                OilType = item.OilType.Name,
        //                                IncotermId = item.IncotermId,
        //                                PlantId = item.PlantId,
        //                                DepotId = item.DepotId,
        //                                SkuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == item.SkuId).SkuName,
        //                                GuaranteePrice = item.GuarateedPricePerCase,
        //                                IncotermName = item.Incoterm.Name,
        //                                PlantName = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == item.PlantId).Name,
        //                                DepotName = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == item.DepotId).Name,
        //                                BidQuantityInCase = item.BidQuantityInCase,
        //                                BidQuantityMT = item.BidQuantityInMT,
        //                                AvailableBidQuantityForOilType = item.BiddingWindow.BiddingWindowVolumeCapacity.FirstOrDefault(_ => _.OilTypeId == item.OilTypeId).VolumeCapacity,
        //                                BidPricePerCase = item.BidPricePerCase,
        //                                GuarateedPricePerCase = item.GuarateedPricePerCase,
        //                                CaseToMTValue = _resultService.ConvertCasetoMetricTon(1, item.SkuId),
        //                                //SkuDiscount = SkuDiscountUsers(item.SkuId, item.DealerId, item.BiddingDateAndTime),
        //                                //VolumeDiscount = VolumeDiscountUsers(item.SkuId, item.DealerId, item.BiddingDateAndTime, dealerContext.CityId),
        //                                //SchemeDiscount = SchemeDiscountUsers(item.SkuId, item.DealerId, item.BiddingDateAndTime),
        //                                ChancesLeft = item.BiddingWindow.NoOfAttemptsForBidding - _emamiContext.SaudaBiddingCart.AsNoTracking()
        //                                     .Where(_ => _.BiddingWindowId == biddingCartHeaderDetails.BiddingWindowId && _.DealerId == biddingCartHeaderDetails.DealerId && _.OilTypeId == item.OilTypeId).Count(),
        //                                TotalChances = item.BiddingWindow.NoOfAttemptsForBidding,
        //                                StatusId = item.StatusId,
        //                                Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == item.StatusId).Name,
        //                                //FreightRouteId = item.Dealer.FreightRouteId ?? 0,
        //                                //FreightRouteName = item.Dealer.FreightRoute.Name
        //                            };

        //                            saudaListForAllocationDto.SKUDetail.Add(skuDetails);
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        if (saudaListForAllocationDto != null)
        //        {
        //            return _resultService.SuccessObject(saudaListForAllocationDto);
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

        //#endregion

        //#region Sauda Details

        //public ResultDto GetSaudaDetailsForDealer(SaudaDetailInputDto inputDto)
        //{
        //    _methodName = "GetSaudaDetails";
        //    var resultDto = new ResultDto();
        //    var saudaDetails = new SaudaDetailOutputDto();
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            return resultDto;
        //        }

        //        if (inputDto.UserId <= 0)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.DealerIdEmpty;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.DealerIdEmpty, Constants.EnglishLanguage);
        //            return resultDto;
        //        }

        //        var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.UserId);
        //        if (userContext == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
        //            return resultDto;
        //        }

        //        var saudaContext = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SaudaId);
        //        if (saudaContext == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
        //            return resultDto;
        //        }

        //        var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id);
        //        var totalBidAmount = saudaOrderContext.Sum(_ => (decimal?)_.BidPrice) ?? 0;
        //        var totalBidQuantity = saudaOrderContext.Sum(_ => (decimal?)_.BidQuantity) ?? 0;
        //        var BrokerContext = saudaOrderContext.FirstOrDefault(_ => _.SaudaId == saudaContext.Id);

        //        saudaDetails.SaudaNumber = saudaContext.Id.ToString();
        //        saudaDetails.SaudaDate = saudaContext.BiddingDate;
        //        saudaDetails.DealerId = saudaContext.UserId;
        //        saudaDetails.DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaContext.UserId).Name;
        //        saudaDetails.TotalAmount = totalBidAmount;
        //        saudaDetails.TotalQuantity = totalBidQuantity;
        //        saudaDetails.SaudaValidityDays = Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity;
        //        saudaDetails.SaudaExpireDays = (DateHelper.UtcToIndia(DateTime.UtcNow) - saudaContext.BiddingDate).Days;
        //        saudaDetails.ExpiryDate = saudaContext.BiddingDate.AddDays(Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity);

        //        if (BrokerContext != null)
        //        {
        //            saudaDetails.BrokerId = BrokerContext.BrokerId;
        //            saudaDetails.BrokerName = BrokerContext.BrokerId != 0 ? _emamiContext.Users.FirstOrDefault(_ => _.Id == BrokerContext.BrokerId).Name : string.Empty;
        //        }

        //        var saudaOrders = new List<SaudaOrderDetails>();

        //        var saudaOrderListContext = saudaOrderContext.Where(_ => _.SaudaId == saudaContext.Id).ToList();
        //        foreach (var order in saudaOrderListContext)
        //        {
        //            var saudaOrderItem = new SaudaOrderDetails();

        //            saudaOrderItem.SkuId = order.SkuId;
        //            saudaOrderItem.SkuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == order.SkuId).SkuName;
        //            saudaOrderItem.BidPrice = order.QuotedPrice; //order.BidPrice;
        //            saudaOrderItem.BidQuantity = order.BidQuantity;
        //            saudaOrderItem.BidQuantityCases = order.BidQuantityCase;
        //            saudaOrderItem.IncoTerms = order.Incoterms1;
        //            saudaOrderItem.Discount = order.DiscountAmount;
        //            saudaOrderItem.PlantDepot = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == order.PlantId).Name;
        //            //saudaOrderItem.FrieghtRoute = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == order.DealerLocationId).Name;
        //            saudaOrderItem.StatusId = order.StatusId;
        //            //saudaOrderItem.Status = order.StatusId == (int)DTO.Enums.Status.Pending ? Constants.Accepted : _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == order.StatusId).Name;
        //            saudaOrderItem.SaudaNumber = order.SaudaNumber != null ? order.SaudaNumber : string.Empty;

        //            var biddingCartHeaderDetails = _emamiContext.SaudaBiddingCartHeaders.AsNoTracking().FirstOrDefault(_ => _.Id == order.Sauda.RABookingId);
        //            if (biddingCartHeaderDetails != null)
        //            {
        //                var biddingCartDetails = _emamiContext.SaudaBiddingCart.AsNoTracking()
        //                    .FirstOrDefault(_ => _.SaudaBiddingCartHeaderId == biddingCartHeaderDetails.Id && _.SkuId == order.SkuId);
        //                if (biddingCartDetails != null)
        //                {

        //                    saudaOrderItem.SKUDetail = new SKUDetail()
        //                    {
        //                        BiddingCartId = biddingCartDetails.Id,
        //                        SkuId = biddingCartDetails.SkuId,
        //                        OilTypeId = biddingCartDetails.OilTypeId,
        //                        OilType = biddingCartDetails.OilType.Name,
        //                        IncotermId = biddingCartDetails.IncotermId,
        //                        PlantId = biddingCartDetails.PlantId,
        //                        DepotId = biddingCartDetails.DepotId,
        //                        SkuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == biddingCartDetails.SkuId).SkuName,
        //                        GuaranteePrice = biddingCartDetails.GuarateedPricePerCase,
        //                        IncotermName = biddingCartDetails.Incoterm.Name,
        //                        PlantName = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == biddingCartDetails.PlantId).Name,
        //                        DepotName = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == biddingCartDetails.DepotId).Name,
        //                        BidQuantityInCase = biddingCartDetails.BidQuantityInCase,
        //                        BidQuantityMT = biddingCartDetails.BidQuantityInMT,
        //                        AvailableBidQuantityForOilType = biddingCartDetails.BiddingWindow.BiddingWindowVolumeCapacity.FirstOrDefault(_ => _.OilTypeId == biddingCartDetails.OilTypeId).VolumeCapacity,
        //                        BidPricePerCase = biddingCartDetails.BidPricePerCase,
        //                        GuarateedPricePerCase = biddingCartDetails.GuarateedPricePerCase,
        //                        CaseToMTValue = _resultService.ConvertCasetoMetricTon(1, biddingCartDetails.SkuId),
        //                        SkuDiscount = order.SkuDiscountCase, // SkuDiscountUsers(biddingCartDetails.SkuId, biddingCartDetails.DealerId, biddingCartDetails.BiddingDateAndTime),
        //                        //VolumeDiscount = VolumeDiscountUsers(biddingCartDetails.SkuId, biddingCartDetails.DealerId, biddingCartDetails.BiddingDateAndTime, userContext.CityId),
        //                        SchemeDiscount = order.SchemeDiscountCase, // SchemeDiscountUsers(biddingCartDetails.SkuId, biddingCartDetails.DealerId, biddingCartDetails.BiddingDateAndTime),
        //                        ChancesLeft = biddingCartDetails.BiddingWindow.NoOfAttemptsForBidding - _emamiContext.SaudaBiddingCart.AsNoTracking()
        //                            .Where(_ => _.BiddingWindowId == biddingCartHeaderDetails.BiddingWindowId && _.DealerId == biddingCartHeaderDetails.DealerId && _.OilTypeId == biddingCartDetails.OilTypeId).Count(),
        //                        TotalChances = biddingCartDetails.BiddingWindow.NoOfAttemptsForBidding,
        //                        StatusId = biddingCartDetails.StatusId,
        //                        //Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == biddingCartDetails.StatusId).Name,
        //                        //FreightRouteId = biddingCartDetails.Dealer.FreightRouteId ?? 0,
        //                        //FreightRouteName = biddingCartDetails.Dealer.FreightRoute.Name,
        //                        SurpriseDiscount = order.SurpriseBenefitType == (long)DTO.Enums.BenefitType.NONSAP ? order.SurpriseBenefitDiscountOrDay : 0,
        //                        AppliedVolumeDiscount = order.VolumeDiscountCase
        //                    };

        //                    string statusName = (order.StatusId == (int)DTO.Enums.Status.Pending) ? Constants.Accepted : _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == order.StatusId).Name;
        //                    if (order.StatusId == (int)DTO.Enums.Status.Pending && biddingCartDetails.CounterBidStatusId == (int)DTO.Enums.Status.Approved)
        //                    {
        //                        saudaOrderItem.Status = statusName + Constants.SaudaAcceptedMessage;
        //                    }
        //                    else
        //                        saudaOrderItem.Status = statusName;

        //                }
        //            }

        //            saudaOrders.Add(saudaOrderItem);
        //        }
        //        saudaDetails.SaudaOrders = saudaOrders;

        //        if (saudaOrderListContext != null && saudaOrderListContext.Any())
        //        {
        //            List<long> saudaOrderIds = saudaOrderListContext.Select(s => s.Id).ToList();

        //        }

        //        resultDto.IsSuccess = true;
        //        resultDto.SuccessDto.Response = saudaDetails;
        //        return resultDto;
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        resultDto.IsSuccess = false;
        //        resultDto.ErrorDto.ErrorCode = Constants.Exception;
        //        resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
        //        _logger.Error(message);
        //        return resultDto;
        //    }
        //}

        //public ResultDto GetSaudaDetailsForBDO(SaudaDetailInputDto inputDto)
        //{
        //    _methodName = "GetSaudaDetails";
        //    var resultDto = new ResultDto();
        //    var saudaDetails = new SaudaDetailOutputDto();
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            return resultDto;
        //        }

        //        if (inputDto.UserId <= 0)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.DealerIdEmpty;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.DealerIdEmpty, Constants.EnglishLanguage);
        //            return resultDto;
        //        }

        //        var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.UserId);
        //        if (userContext == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
        //            return resultDto;
        //        }

        //        var saudaContext = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SaudaId);
        //        if (saudaContext == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
        //            return resultDto;
        //        }

        //        var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id);
        //        var totalBidAmount = saudaOrderContext.Sum(_ => (decimal?)_.BidPrice) ?? 0;
        //        var totalBidQuantity = saudaOrderContext.Sum(_ => (decimal?)_.BidQuantity) ?? 0;
        //        var BrokerContext = saudaOrderContext.FirstOrDefault(_ => _.SaudaId == saudaContext.Id);

        //        saudaDetails.SaudaNumber = saudaContext.Id.ToString();
        //        saudaDetails.SaudaDate = saudaContext.BiddingDate;
        //        saudaDetails.DealerId = saudaContext.UserId;
        //        saudaDetails.DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaContext.UserId).Name;
        //        saudaDetails.TotalAmount = totalBidAmount;
        //        saudaDetails.TotalQuantity = totalBidQuantity;
        //        saudaDetails.SaudaValidityDays = Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity;
        //        saudaDetails.SaudaExpireDays = (DateHelper.UtcToIndia(DateTime.UtcNow) - saudaContext.BiddingDate).Days;
        //        saudaDetails.ExpiryDate = saudaContext.BiddingDate.AddDays(Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity);

        //        if (BrokerContext != null)
        //        {
        //            saudaDetails.BrokerId = BrokerContext.BrokerId;
        //            saudaDetails.BrokerName = BrokerContext.BrokerId != 0 ? _emamiContext.Users.FirstOrDefault(_ => _.Id == BrokerContext.BrokerId).Name : string.Empty;
        //        }

        //        var saudaAudioFileMappingContext = _emamiContext.SaudaAudioFileMapping.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id);
        //        var key = UtilityHelper.GetEnumDescription((DTO.Enums.Configuration.CallRecordMappingReattachBufferTime));
        //        var reattachBufferTime = _emamiContext.Configurations.AsNoTracking().FirstOrDefault(_ => _.Key == key)?.Value ?? "0";
        //        var BufferTimeToAdd = Convert.ToDouble(reattachBufferTime);
        //        if (!saudaAudioFileMappingContext.IsAny())
        //        {
        //            saudaDetails.CanSubmitAudioMapping = true;
        //        }
        //        else if (saudaAudioFileMappingContext.IsAny())
        //        {
        //            var ImageCreatedDate = saudaAudioFileMappingContext.FirstOrDefault(_ => _.SaudaId == saudaContext.Id && (_.MediaTypeId == (int)DTO.Enums.MediaType.Audio || _.MediaTypeId == (int)DTO.Enums.MediaType.Image)).CreatedDate;
        //            var timeUntilReattachmentDone = ImageCreatedDate.AddMinutes(BufferTimeToAdd);
        //            if (DateHelper.UtcToIndia(DateTime.UtcNow) <= timeUntilReattachmentDone)
        //            {
        //                saudaDetails.CanSubmitAudioMapping = true;
        //            }
        //        }
        //        saudaDetails.AudiofileDetailIds = saudaAudioFileMappingContext.Where(_ => _.MediaTypeId == (int)DTO.Enums.MediaType.Audio).Select(s => s.AudioFileDetailsForActiveCustomersId ?? 0).ToList();

        //        var imageNames = saudaAudioFileMappingContext.FirstOrDefault(_ => _.SaudaId == saudaContext.Id && _.MediaTypeId == (int)DTO.Enums.MediaType.Image) != null ? saudaAudioFileMappingContext.FirstOrDefault(_ => _.SaudaId == saudaContext.Id && _.MediaTypeId == (int)DTO.Enums.MediaType.Image).ImagePath : string.Empty;

        //        if (imageNames != string.Empty)
        //        {
        //            saudaDetails.ImagePaths = imageNames.Split(',').ToList();
        //            string folderName = UtilityHelper.GetEnumDescription(DTO.Enums.PageType.ImagesSaudaMappingwithCallRecording);
        //            string mediapath = Path.Combine(ConfigurationManager.AppSettings["UploadAttachments"], folderName);

        //            if (saudaDetails.ImagePaths.IsAny())
        //            {
        //                saudaDetails.ImagePaths = saudaDetails.ImagePaths.Select(filename => Path.Combine(mediapath, filename)).ToList();
        //            }
        //        }

        //        var saudaOrders = new List<SaudaOrderDetails>();

        //        var saudaOrderListContext = saudaOrderContext.Where(_ => _.SaudaId == saudaContext.Id).ToList();
        //        foreach (var order in saudaOrderListContext)
        //        {
        //            var saudaOrderItem = new SaudaOrderDetails();
        //            decimal taxPaidValue = 0;
        //            decimal discountGstPercentage = 0;
        //            decimal discountWithTax = 0;
        //            decimal discountTaxAmount = 0;



        //            var raTotalDiscount = order.VolumeDiscountCase +
        //                       order.SchemeDiscountCase +
        //                       order.SkuDiscountCase +
        //                       (order.GPBenefitType == (int)DTO.Enums.BenefitType.NONSAP ? order.GPBenefitDiscountInCase : 0) +
        //                       (order.SurpriseBenefitType == (int)DTO.Enums.BenefitType.NONSAP ? order.SurpriseBenefitDiscountInCase : 0);



        //            decimal bidPricePerCause = (order.QuotedPrice / order.BidQuantityCase);
        //            var pricingData = _emamiContext.Pricing.AsNoTracking()
        //                .Where(f => f.Id == order.PricingId)
        //                .Select(s => new
        //                {
        //                    //PlantGSTPercentage = s.PlantGSTPercentage,
        //                    //DepotGSTPercentage = s.DepotGSTPercentage
        //                }).FirstOrDefault();



        //            if (order.Incoterms2 == (long)DTO.Enums.IncoTerms.ExPlant || order.Incoterms2 == (long)DTO.Enums.IncoTerms.ForPlant)
        //            {
        //                //discountGstPercentage = Utility.GetGstAmount(1, pricingData.PlantGSTPercentage);
        //                discountWithTax = raTotalDiscount * discountGstPercentage;
        //                discountTaxAmount = discountWithTax - raTotalDiscount;
        //                taxPaidValue = Utility.DecimalFormatTwo(bidPricePerCause);// - discountTaxAmount;
        //                saudaOrderItem.BidPricePerCaseWithoutTax = taxPaidValue;
        //            }
        //            else if (order.Incoterms2 == (long)DTO.Enums.IncoTerms.ExDepot || order.Incoterms2 == (long)DTO.Enums.IncoTerms.ForDepot)
        //            {
        //                //discountGstPercentage = Utility.GetGstAmount(1, pricingData.DepotGSTPercentage);
        //                discountWithTax = raTotalDiscount * discountGstPercentage;
        //                discountTaxAmount = discountWithTax - raTotalDiscount;
        //                taxPaidValue = Utility.DecimalFormatTwo(bidPricePerCause);// - discountTaxAmount;
        //                saudaOrderItem.BidPricePerCaseWithoutTax = taxPaidValue;
        //            }



        //            saudaOrderItem.SkuId = order.SkuId;
        //            saudaOrderItem.SkuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == order.SkuId).SkuName;
        //            saudaOrderItem.BidPrice = order.QuotedPrice;
        //            saudaOrderItem.BidQuantity = order.BidQuantity;
        //            saudaOrderItem.BidQuantityCases = order.BidQuantityCase;
        //            saudaOrderItem.IncoTerms = order.Incoterms1;
        //            saudaOrderItem.Discount = order.DiscountAmount;
        //            saudaOrderItem.PlantDepot = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == order.PlantId).Name;
        //            //saudaOrderItem.FrieghtRoute = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == order.DealerLocationId).Name;
        //            saudaOrderItem.StatusId = order.StatusId;
        //            //saudaOrderItem.Status = order.StatusId == (int)DTO.Enums.Status.Pending ? Constants.Accepted : _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == order.StatusId).Name;
        //            saudaOrderItem.SaudaNumber = order.SaudaNumber != null ? order.SaudaNumber : string.Empty;




        //            saudaOrderItem.SKUDetail = new SKUDetail()
        //            {
        //                SkuDiscount = order.SkuDiscountCase, // SkuDiscountUsers(biddingCartDetails.SkuId, biddingCartDetails.DealerId, biddingCartDetails.BiddingDateAndTime),
        //                SchemeDiscount = order.SchemeDiscountCase,// SchemeDiscountUsers(biddingCartDetails.SkuId, biddingCartDetails.DealerId, biddingCartDetails.BiddingDateAndTime),
        //                SurpriseDiscount = order.SurpriseBenefitType == (long)DTO.Enums.BenefitType.NONSAP ? order.SurpriseBenefitDiscountOrDay : 0,
        //                AppliedVolumeDiscount = order.VolumeDiscountCase
        //            };



        //            var biddingCartHeaderDetails = _emamiContext.SaudaBiddingCartHeaders.AsNoTracking().FirstOrDefault(_ => _.Id == order.Sauda.RABookingId);
        //            if (biddingCartHeaderDetails != null)
        //            {
        //                var biddingCartDetails = _emamiContext.SaudaBiddingCart.AsNoTracking()
        //                    .FirstOrDefault(_ => _.SaudaBiddingCartHeaderId == biddingCartHeaderDetails.Id && _.SkuId == order.SkuId);
        //                if (biddingCartDetails != null)
        //                {
        //                    string statusName = (order.StatusId == (int)DTO.Enums.Status.Pending) ? Constants.Accepted : _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == order.StatusId).Name;
        //                    if (order.StatusId == (int)DTO.Enums.Status.Pending && biddingCartDetails.CounterBidStatusId == (int)DTO.Enums.Status.Approved)
        //                    {
        //                        saudaOrderItem.Status = statusName + Constants.SaudaAcceptedMessage;
        //                    }
        //                    else
        //                        saudaOrderItem.Status = statusName;
        //                }
        //            }
        //            saudaOrders.Add(saudaOrderItem);
        //        }
        //        saudaDetails.SaudaOrders = saudaOrders;

        //        resultDto.IsSuccess = true;
        //        resultDto.SuccessDto.Response = saudaDetails;
        //        return resultDto;
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        resultDto.IsSuccess = false;
        //        resultDto.ErrorDto.ErrorCode = Constants.Exception;
        //        resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
        //        _logger.Error(message);
        //        return resultDto;
        //    }
        //}

        //public ResultDto GetSaudaDetailsForBDOOld(SaudaDetailInputDto inputDto)
        //{
        //    _methodName = "GetSaudaDetails";
        //    var resultDto = new ResultDto();
        //    var saudaDetails = new SaudaDetailOutputDto();
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            return resultDto;
        //        }

        //        if (inputDto.UserId <= 0)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.DealerIdEmpty;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.DealerIdEmpty, Constants.EnglishLanguage);
        //            return resultDto;
        //        }

        //        var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.UserId);
        //        if (userContext == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
        //            return resultDto;
        //        }

        //        var saudaContext = _emamiContext.Sauda.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.SaudaId);
        //        if (saudaContext == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
        //            return resultDto;
        //        }

        //        var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == saudaContext.Id);
        //        var totalBidAmount = saudaOrderContext.Sum(_ => (decimal?)_.BidPrice) ?? 0;
        //        var totalBidQuantity = saudaOrderContext.Sum(_ => (decimal?)_.BidQuantity) ?? 0;
        //        var BrokerContext = saudaOrderContext.FirstOrDefault(_ => _.SaudaId == saudaContext.Id);

        //        saudaDetails.SaudaNumber = saudaContext.Id.ToString();
        //        saudaDetails.SaudaDate = saudaContext.BiddingDate;
        //        saudaDetails.DealerId = saudaContext.UserId;
        //        saudaDetails.DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == saudaContext.UserId).Name;
        //        saudaDetails.TotalAmount = totalBidAmount;
        //        saudaDetails.TotalQuantity = totalBidQuantity;
        //        saudaDetails.SaudaValidityDays = Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity;
        //        saudaDetails.SaudaExpireDays = (DateHelper.UtcToIndia(DateTime.UtcNow) - saudaContext.BiddingDate).Days;
        //        saudaDetails.ExpiryDate = saudaContext.BiddingDate.AddDays(Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity);

        //        if (BrokerContext != null)
        //        {
        //            saudaDetails.BrokerId = BrokerContext.BrokerId;
        //            saudaDetails.BrokerName = BrokerContext.BrokerId != 0 ? _emamiContext.Users.FirstOrDefault(_ => _.Id == BrokerContext.BrokerId).Name : string.Empty;
        //        }

        //        var saudaOrders = new List<SaudaOrderDetails>();

        //        var saudaOrderListContext = saudaOrderContext.Where(_ => _.SaudaId == saudaContext.Id).ToList();
        //        foreach (var order in saudaOrderListContext)
        //        {
        //            var saudaOrderItem = new SaudaOrderDetails();

        //            saudaOrderItem.SkuId = order.SkuId;
        //            saudaOrderItem.SkuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == order.SkuId).SkuName;
        //            saudaOrderItem.BidPrice = order.QuotedPrice;
        //            saudaOrderItem.BidQuantity = order.BidQuantity;
        //            saudaOrderItem.BidQuantityCases = order.BidQuantityCase;
        //            saudaOrderItem.IncoTerms = order.Incoterms1;
        //            saudaOrderItem.Discount = order.DiscountAmount;
        //            saudaOrderItem.PlantDepot = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == order.PlantId).Name;
        //            //saudaOrderItem.FrieghtRoute = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == order.DealerLocationId).Name;
        //            saudaOrderItem.StatusId = order.StatusId;
        //            //saudaOrderItem.Status = order.StatusId == (int)DTO.Enums.Status.Pending ? Constants.Accepted : _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == order.StatusId).Name;
        //            saudaOrderItem.SaudaNumber = order.SaudaNumber != null ? order.SaudaNumber : string.Empty;

        //            var biddingCartHeaderDetails = _emamiContext.SaudaBiddingCartHeaders.AsNoTracking().FirstOrDefault(_ => _.Id == order.Sauda.RABookingId);
        //            if (biddingCartHeaderDetails != null)
        //            {
        //                var biddingCartDetails = _emamiContext.SaudaBiddingCart.AsNoTracking()
        //                    .FirstOrDefault(_ => _.SaudaBiddingCartHeaderId == biddingCartHeaderDetails.Id && _.SkuId == order.SkuId);
        //                if (biddingCartDetails != null)
        //                {
        //                    saudaOrderItem.SKUDetail = new SKUDetail()
        //                    {
        //                        BiddingCartId = biddingCartDetails.Id,
        //                        SkuId = biddingCartDetails.SkuId,
        //                        OilTypeId = biddingCartDetails.OilTypeId,
        //                        OilType = biddingCartDetails.OilType.Name,
        //                        IncotermId = biddingCartDetails.IncotermId,
        //                        PlantId = biddingCartDetails.PlantId,
        //                        DepotId = biddingCartDetails.DepotId,
        //                        SkuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == biddingCartDetails.SkuId).SkuName,
        //                        GuaranteePrice = biddingCartDetails.GuarateedPricePerCase,
        //                        IncotermName = biddingCartDetails.Incoterm.Name,
        //                        PlantName = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == biddingCartDetails.PlantId).Name,
        //                        DepotName = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == biddingCartDetails.DepotId).Name,
        //                        BidQuantityInCase = biddingCartDetails.BidQuantityInCase,
        //                        BidQuantityMT = biddingCartDetails.BidQuantityInMT,
        //                        AvailableBidQuantityForOilType = biddingCartDetails.BiddingWindow.BiddingWindowVolumeCapacity.FirstOrDefault(_ => _.OilTypeId == biddingCartDetails.OilTypeId).VolumeCapacity,
        //                        BidPricePerCase = biddingCartDetails.BidPricePerCase,
        //                        GuarateedPricePerCase = biddingCartDetails.GuarateedPricePerCase,
        //                        CaseToMTValue = _resultService.ConvertCasetoMetricTon(1, biddingCartDetails.SkuId),
        //                        SkuDiscount = order.SkuDiscountCase, // SkuDiscountUsers(biddingCartDetails.SkuId, biddingCartDetails.DealerId, biddingCartDetails.BiddingDateAndTime),
        //                        //VolumeDiscount = VolumeDiscountUsers(biddingCartDetails.SkuId, biddingCartDetails.DealerId, biddingCartDetails.BiddingDateAndTime, biddingCartDetails.Dealer.CityId),
        //                        SchemeDiscount = order.SchemeDiscountCase,// SchemeDiscountUsers(biddingCartDetails.SkuId, biddingCartDetails.DealerId, biddingCartDetails.BiddingDateAndTime),
        //                        ChancesLeft = biddingCartDetails.BiddingWindow.NoOfAttemptsForBidding - _emamiContext.SaudaBiddingCart.AsNoTracking()
        //                            .Where(_ => _.BiddingWindowId == biddingCartHeaderDetails.BiddingWindowId && _.DealerId == biddingCartHeaderDetails.DealerId && _.OilTypeId == biddingCartDetails.OilTypeId).Count(),
        //                        TotalChances = biddingCartDetails.BiddingWindow.NoOfAttemptsForBidding,
        //                        StatusId = biddingCartDetails.StatusId,
        //                        //Status = _emamiContext.ApprovalStatus.FirstOrDefault(_ => _.Id == biddingCartDetails.StatusId).Name,
        //                        //FreightRouteId = biddingCartDetails.Dealer.FreightRouteId ?? 0,
        //                        //FreightRouteName = biddingCartDetails.Dealer.FreightRoute.Name,
        //                        SurpriseDiscount = order.SurpriseBenefitType == (long)DTO.Enums.BenefitType.NONSAP ? order.SurpriseBenefitDiscountOrDay : 0,
        //                        AppliedVolumeDiscount = order.VolumeDiscountCase
        //                    };

        //                    string statusName = (order.StatusId == (int)DTO.Enums.Status.Pending) ? Constants.Accepted : _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == order.StatusId).Name;
        //                    if (order.StatusId == (int)DTO.Enums.Status.Pending && biddingCartDetails.CounterBidStatusId == (int)DTO.Enums.Status.Approved)
        //                    {
        //                        saudaOrderItem.Status = statusName + Constants.SaudaAcceptedMessage;
        //                    }
        //                    else
        //                        saudaOrderItem.Status = statusName;
        //                }
        //            }

        //            saudaOrders.Add(saudaOrderItem);
        //        }
        //        saudaDetails.SaudaOrders = saudaOrders;

        //        resultDto.IsSuccess = true;
        //        resultDto.SuccessDto.Response = saudaDetails;
        //        return resultDto;
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        resultDto.IsSuccess = false;
        //        resultDto.ErrorDto.ErrorCode = Constants.Exception;
        //        resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
        //        _logger.Error(message);
        //        return resultDto;
        //    }
        //}


        //public ResultDto GetSkusWithDealerAsList(SkuwithDealerFilterInputDto inputDto)
        //{
        //    _methodName = "GetSkusWithDealerAsList";
        //    var resultDto = new ResultDto();
        //    var dealerWithSkuDetails = new List<SkuwithDealerOutputDto>();
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            return resultDto;
        //        }

        //        if (inputDto.SkuIds == null)
        //        {
        //            var skuIds = _emamiContext.Skus.AsNoTracking().Where(sku => sku.IsActive).Select(_ => _.Id).ToList();
        //            inputDto.SkuIds = new List<long>();
        //            inputDto.SkuIds.AddRange(skuIds);
        //        }
        //        //Dealer 

        //        if (inputDto.DealerIds.IsAny())
        //        {
        //            var sauda = _emamiContext.Sauda.AsNoTracking().Where(_ => DbFunctions.TruncateTime(_.BiddingDate) == DbFunctions.TruncateTime(inputDto.BiddingDate) && inputDto.DealerIds.Contains(_.UserId)).GroupBy(_ => _.UserId).Select(a => new
        //            {
        //                UserId = a.Key,
        //                SaudaIds = a.Select(saudas => saudas.Id).ToList()
        //            }).ToList();
        //            var sudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking();
        //            foreach (var item in sauda)
        //            {
        //                var outputDto = new SkuwithDealerOutputDto();
        //                sudaOrderContext = sudaOrderContext
        //                    .Where(_ => item.SaudaIds.Contains(_.SaudaId) && inputDto.SkuIds.Contains(_.SkuId));

        //                if (inputDto.SalesOrganizationId > 0 && inputDto.DistributionChannelId > 0 && inputDto.DivisionId > 0)
        //                {
        //                    sudaOrderContext = sudaOrderContext.Where(_ => _.SalesOrganizationId == inputDto.SalesOrganizationId
        //                     && _.DistributionChannelId == inputDto.DistributionChannelId
        //                     && _.DivisionId == inputDto.DivisionId);
        //                }

        //                var saudaOrderContext = sudaOrderContext.Where(_ => item.SaudaIds.Contains(_.SaudaId)
        //                && inputDto.SkuIds.Contains(_.SkuId))
        //                       .Select(sku => new DropDownDto()
        //                       {
        //                           Id = sku.SkuId,
        //                           Name = sku.Sku.SkuName
        //                       }).Distinct().ToList();

        //                if (saudaOrderContext.IsAny())
        //                {
        //                    outputDto.DealerId = item.UserId;
        //                    outputDto.DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(user => user.Id == item.UserId).Name;
        //                    outputDto.SkuList.AddRange(saudaOrderContext);
        //                    dealerWithSkuDetails.Add(outputDto);
        //                }
        //            }

        //        }
        //        //StateTrader
        //        else if (inputDto.DealerIds == null && inputDto.BdoIds.IsAny())
        //        {
        //            var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(usercustomer => inputDto.BdoIds.Contains(usercustomer.UserId)).Select(customer => customer.CustomerId).ToList();
        //            var sauda = _emamiContext.Sauda.AsNoTracking().Where(_ => DbFunctions.TruncateTime(_.BiddingDate) == DbFunctions.TruncateTime(inputDto.BiddingDate) && dealerIds.Contains(_.UserId)).GroupBy(_ => _.UserId).Select(a => new
        //            {
        //                UserId = a.Key,
        //                SaudaIds = a.Select(saudas => saudas.Id).ToList()
        //            }).ToList();

        //            var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking();
        //            foreach (var item in sauda)
        //            {
        //                var outputDto = new SkuwithDealerOutputDto();

        //                if (inputDto.SalesOrganizationId > 0 && inputDto.DistributionChannelId > 0 && inputDto.DivisionId > 0)
        //                {
        //                    saudaOrderContext = saudaOrderContext.Where(_ => _.SalesOrganizationId == inputDto.SalesOrganizationId
        //                     && _.DistributionChannelId == inputDto.DistributionChannelId
        //                     && _.DivisionId == inputDto.DivisionId);
        //                }

        //                var saudaOrdersContext = saudaOrderContext.Where(_ => item.SaudaIds.Contains(_.SaudaId) && inputDto.SkuIds.Contains(_.SkuId)).Select(sku => new DropDownDto()
        //                {
        //                    Id = sku.SkuId,
        //                    Name = sku.Sku.SkuName
        //                }).Distinct().ToList();

        //                if (saudaOrdersContext.IsAny())
        //                {
        //                    outputDto.DealerId = item.UserId;
        //                    outputDto.DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(user => user.Id == item.UserId).Name;
        //                    outputDto.SkuList.AddRange(saudaOrdersContext);
        //                    dealerWithSkuDetails.Add(outputDto);
        //                }

        //            }
        //        }
        //        //ZonalTrader
        //        else if (inputDto.DealerIds == null && inputDto.BdoIds == null)
        //        {
        //            var bdoIds = _emamiContext.Users.AsNoTracking().Where(user => user.ReportingToId == inputDto.LoginUserId).Select(a => a.Id).ToList();
        //            var dealerIds = _emamiContext.UserCustomerMapping.AsNoTracking().Where(usercustomer => bdoIds.Contains(usercustomer.UserId)).Select(customer => customer.CustomerId).ToList();
        //            var sauda = _emamiContext.Sauda.AsNoTracking().Where(_ => DbFunctions.TruncateTime(_.BiddingDate) == DbFunctions.TruncateTime(inputDto.BiddingDate) && dealerIds.Contains(_.UserId)).GroupBy(_ => _.UserId).Select(a => new
        //            {
        //                UserId = a.Key,
        //                SaudaIds = a.Select(saudas => saudas.Id).ToList()
        //            }).ToList();

        //            var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking();
        //            foreach (var item in sauda)
        //            {
        //                var outputDto = new SkuwithDealerOutputDto();

        //                if (inputDto.SalesOrganizationId > 0 && inputDto.DistributionChannelId > 0 && inputDto.DivisionId > 0)
        //                {
        //                    saudaOrderContext = saudaOrderContext.Where(_ => _.SalesOrganizationId == inputDto.SalesOrganizationId
        //                     && _.DistributionChannelId == inputDto.DistributionChannelId
        //                     && _.DivisionId == inputDto.DivisionId);
        //                }
        //                var saudaOrdersContext = saudaOrderContext.Where(_ => item.SaudaIds.Contains(_.SaudaId) && inputDto.SkuIds.Contains(_.SkuId)).Select(sku => new DropDownDto()
        //                {
        //                    Id = sku.SkuId,
        //                    Name = sku.Sku.SkuName
        //                }).Distinct().ToList();

        //                if (saudaOrdersContext.IsAny())
        //                {
        //                    outputDto.DealerId = item.UserId;
        //                    outputDto.DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(user => user.Id == item.UserId).Name;
        //                    outputDto.SkuList.AddRange(saudaOrdersContext);
        //                    dealerWithSkuDetails.Add(outputDto);
        //                }
        //            }

        //        }

        //        resultDto.IsSuccess = true;
        //        resultDto.SuccessDto.Response = dealerWithSkuDetails;
        //        return resultDto;
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        resultDto.IsSuccess = false;
        //        resultDto.ErrorDto.ErrorCode = Constants.Exception;
        //        resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
        //        _logger.Error(message);
        //        return resultDto;
        //    }
        //}

        //public ResultDto GetSaudaDetailsRANew(SaudaDetailInputDto inputDto)
        //{
        //    _methodName = "GetSaudaDetailsRANew";
        //    var resultDto = new ResultDto();
        //    var saudaDetails = new List<SaudaDetailOutputDto>();
        //    try
        //    {
        //        if (inputDto == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.InvalidRequest;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.InvalidRequest, Constants.EnglishLanguage);
        //            return resultDto;
        //        }

        //        if (inputDto.UserId <= 0)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.DealerIdEmpty;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.DealerIdEmpty, Constants.EnglishLanguage);
        //            return resultDto;
        //        }

        //        var userContext = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == inputDto.UserId);
        //        if (userContext == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.UserNotFound;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.UserNotFound, Constants.EnglishLanguage);
        //            return resultDto;
        //        }

        //        var saudaContext = _emamiContext.Sauda.AsNoTracking().Where(_ => _.UserId == inputDto.UserId && DbFunctions.TruncateTime(_.BiddingDate) == DbFunctions.TruncateTime(inputDto.BiddingDate)).ToList();
        //        if (saudaContext == null)
        //        {
        //            resultDto.IsSuccess = false;
        //            resultDto.ErrorDto.ErrorCode = Constants.RecordNotFound;
        //            resultDto.ErrorDto.Message = Constants.GetMessage(Constants.RecordNotFound, Constants.EnglishLanguage);
        //            return resultDto;
        //        }
        //        foreach (var sauda in saudaContext)
        //        {
        //            var saudaOrderContext = _emamiContext.SaudaOrders.AsNoTracking().Where(_ => _.SaudaId == sauda.Id);
        //            var saudaOrderListContext = saudaOrderContext.Where(_ => _.SaudaId == sauda.Id && inputDto.SkuIds.Contains(_.SkuId)).ToList();
        //            foreach (var order in saudaOrderListContext)
        //            {
        //                var saudaOrders = new List<SaudaOrderDetails>();
        //                var saudaDetail = new SaudaDetailOutputDto();
        //                var totalBidAmount = saudaOrderContext.Sum(_ => (decimal?)_.BidPrice) ?? 0;
        //                var totalBidQuantity = saudaOrderContext.Sum(_ => (decimal?)_.BidQuantity) ?? 0;
        //                var BrokerContext = saudaOrderContext.FirstOrDefault(_ => _.SaudaId == sauda.Id);

        //                saudaDetail.SaudaNumber = sauda.Id.ToString();
        //                saudaDetail.SaudaDate = sauda.BiddingDate;
        //                saudaDetail.DealerId = sauda.UserId;
        //                saudaDetail.DealerName = _emamiContext.Users.AsNoTracking().FirstOrDefault(_ => _.Id == sauda.UserId).Name;
        //                saudaDetail.TotalAmount = totalBidAmount;
        //                saudaDetail.TotalQuantity = totalBidQuantity;
        //                saudaDetail.SaudaValidityDays = Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity;
        //                saudaDetail.SaudaExpireDays = (DateHelper.UtcToIndia(DateTime.UtcNow) - sauda.BiddingDate).Days;
        //                saudaDetail.ExpiryDate = sauda.BiddingDate.AddDays(Convert.ToInt32(userContext.SaudaValidityPeriod) != 0 ? Convert.ToInt32(userContext.SaudaValidityPeriod) : Config.DefaultSaudaValidity);

        //                if (BrokerContext != null)
        //                {
        //                    saudaDetail.BrokerId = BrokerContext.BrokerId;
        //                    saudaDetail.BrokerName = BrokerContext.BrokerId != 0 ? _emamiContext.Users.FirstOrDefault(_ => _.Id == BrokerContext.BrokerId).Name : string.Empty;
        //                }

        //                var saudaOrderItem = new SaudaOrderDetails();
        //                decimal taxPaidValue = 0;
        //                decimal discountGstPercentage = 0;
        //                decimal discountWithTax = 0;
        //                decimal discountTaxAmount = 0;

        //                var raTotalDiscount = order.VolumeDiscountCase +
        //                       order.SchemeDiscountCase +
        //                       order.SkuDiscountCase +
        //                       (order.GPBenefitType == (int)DTO.Enums.BenefitType.NONSAP ? order.GPBenefitDiscountInCase : 0) +
        //                       (order.SurpriseBenefitType == (int)DTO.Enums.BenefitType.NONSAP ? order.SurpriseBenefitDiscountInCase : 0);



        //                decimal bidPricePerCause = (order.BidPrice / order.BidQuantityCase);
        //                var pricingData = _emamiContext.Pricing.AsNoTracking()
        //                    .Where(f => f.Id == order.PricingId)
        //                    .Select(s => new
        //                    {
        //                        //PlantGSTPercentage = s.PlantGSTPercentage,
        //                        //DepotGSTPercentage = s.DepotGSTPercentage
        //                    }).FirstOrDefault();



        //                if (order.Incoterms2 == (long)DTO.Enums.IncoTerms.ExPlant || order.Incoterms2 == (long)DTO.Enums.IncoTerms.ForPlant)
        //                {
        //                    //discountGstPercentage = Utility.GetGstAmount(1, pricingData.PlantGSTPercentage);
        //                    discountWithTax = raTotalDiscount * discountGstPercentage;
        //                    discountTaxAmount = discountWithTax - raTotalDiscount;
        //                    taxPaidValue = Utility.DecimalFormatTwo(bidPricePerCause);// - discountTaxAmount;
        //                    saudaOrderItem.BidPricePerCaseWithoutTax = taxPaidValue;
        //                }
        //                else if (order.Incoterms2 == (long)DTO.Enums.IncoTerms.ExDepot || order.Incoterms2 == (long)DTO.Enums.IncoTerms.ForDepot)
        //                {
        //                    //discountGstPercentage = Utility.GetGstAmount(1, pricingData.DepotGSTPercentage);
        //                    discountWithTax = raTotalDiscount * discountGstPercentage;
        //                    discountTaxAmount = discountWithTax - raTotalDiscount;
        //                    taxPaidValue = Utility.DecimalFormatTwo(bidPricePerCause);// - discountTaxAmount;
        //                    saudaOrderItem.BidPricePerCaseWithoutTax = taxPaidValue;
        //                }



        //                saudaOrderItem.SkuId = order.SkuId;
        //                saudaOrderItem.SkuName = _emamiContext.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == order.SkuId).SkuName;
        //                saudaOrderItem.BidPrice = order.QuotedPrice;
        //                saudaOrderItem.BidQuantity = order.BidQuantity;
        //                saudaOrderItem.BidQuantityCases = order.BidQuantityCase;
        //                saudaOrderItem.IncoTerms = order.Incoterms1;
        //                saudaOrderItem.Discount = order.DiscountAmount;
        //                saudaOrderItem.PlantDepot = _emamiContext.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == order.PlantId).Name;
        //                //saudaOrderItem.FrieghtRoute = _emamiContext.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == order.DealerLocationId).Name;
        //                saudaOrderItem.StatusId = order.StatusId;
        //                //saudaOrderItem.Status = order.StatusId == (int)DTO.Enums.Status.Pending ? Constants.Accepted : _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == order.StatusId).Name;
        //                saudaOrderItem.SaudaNumber = order.SaudaNumber != null ? order.SaudaNumber : string.Empty;


        //                saudaOrderItem.SKUDetail = new SKUDetail()
        //                {
        //                    SkuDiscount = order.SkuDiscountCase, // SkuDiscountUsers(biddingCartDetails.SkuId, biddingCartDetails.DealerId, biddingCartDetails.BiddingDateAndTime),
        //                    SchemeDiscount = order.SchemeDiscountCase,// SchemeDiscountUsers(biddingCartDetails.SkuId, biddingCartDetails.DealerId, biddingCartDetails.BiddingDateAndTime),
        //                    SurpriseDiscount = order.SurpriseBenefitType == (long)DTO.Enums.BenefitType.NONSAP ? order.SurpriseBenefitDiscountOrDay : 0,
        //                    AppliedVolumeDiscount = order.VolumeDiscountCase
        //                };

        //                var biddingCartHeaderDetails = _emamiContext.SaudaBiddingCartHeaders.AsNoTracking().FirstOrDefault(_ => _.Id == order.Sauda.RABookingId);
        //                if (biddingCartHeaderDetails != null)
        //                {
        //                    var biddingCartDetails = _emamiContext.SaudaBiddingCart.AsNoTracking()
        //                        .FirstOrDefault(_ => _.SaudaBiddingCartHeaderId == biddingCartHeaderDetails.Id && _.SkuId == order.SkuId);
        //                    if (biddingCartDetails != null)
        //                    {
        //                        string statusName = (order.StatusId == (int)DTO.Enums.Status.Pending) ? Constants.Accepted : _emamiContext.ApprovalStatus.AsNoTracking().FirstOrDefault(_ => _.Id == order.StatusId).Name;
        //                        if (order.StatusId == (int)DTO.Enums.Status.Pending && biddingCartDetails.CounterBidStatusId == (int)DTO.Enums.Status.Approved)
        //                        {
        //                            saudaOrderItem.Status = statusName + Constants.SaudaAcceptedMessage;
        //                        }
        //                        else
        //                            saudaOrderItem.Status = statusName;
        //                    }
        //                }
        //                saudaOrders.Add(saudaOrderItem);
        //                saudaDetail.SaudaOrders = saudaOrders;
        //                saudaDetails.Add(saudaDetail);
        //            }
        //        }
        //        resultDto.IsSuccess = true;
        //        resultDto.SuccessDto.Response = saudaDetails;
        //        return resultDto;
        //    }
        //    catch (Exception exception)
        //    {
        //        var message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
        //        resultDto.IsSuccess = false;
        //        resultDto.ErrorDto.ErrorCode = Constants.Exception;
        //        resultDto.ErrorDto.Message = Constants.GetMessage(Constants.Exception, Utility.MessageLanguage);
        //        _logger.Error(message);
        //        return resultDto;
        //    }
        //}
        //#endregion
    }
}