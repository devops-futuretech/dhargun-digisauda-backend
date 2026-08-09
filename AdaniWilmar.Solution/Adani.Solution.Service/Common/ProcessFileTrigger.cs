using Adani.Solution.Data.DatabaseContext;
using Adani.Solution.Data.Entities;
using Adani.Solution.DTO;
using Adani.Solution.DTO.Common;
using Adani.Solution.DTO.Enums;
using GMCore.Helper;
using GMCore.Logger;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace Adani.Solution.Service.Common
{
    public class ProcessFileTrigger
    {
        private readonly ILogger _logger = Logging.GetLogger("Final Price Service");

        /// <summary>
        /// Method to run the final price generate services(exe).
        /// </summary>
        /// <param name="stateIdList"></param>
        ///// public void FinalPriceExeInvoke(List<int> stateIdList)
        public void FinalPriceExeInvoke(long saudaBookingTypeId)
        {
            try
            {
                string exePath = "";
                if (saudaBookingTypeId == (int)SaudaBookingTypes.TraditionalProcess)
                    exePath = ConsoleSettings.TraditionalProcessExePath;
                //else if (saudaBookingTypeId == (int)SaudaBookingTypes.ReverseAuction)
                //    exePath = ConsoleSettings.ReverseAuctionExePath;

                var filePath = string.Concat(exePath, "\\Adani.Solution.FinalPrice.exe");
                var myProcess = new Process();
                myProcess.StartInfo.FileName = filePath;
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
            }
            catch (Exception exception)
            {
                _logger.Error("FinalPriceExeInvoke : " + exception.Message);
            }
        }

        public void SkuFinalpriceListForAdmin(long cStateId)
        {
            string ServiceName = "FinalPriceService";
            string _methodName = "";
            _logger.Info("SkuFinalpriceListForAdmin , State Id = " + cStateId + " Process Started");
            using (var _context = new AdaniContext())
            {
                ResultService _resultService = new ResultService();
                #region Get Params

                var priceGenerateDetail = _context.PriceGenerateDetail
                     .FirstOrDefault(w => w.StatusId == (int)DTO.Enums.PricePublishStatus.Pending
                     && DbFunctions.TruncateTime(w.CreatedDate) == DbFunctions.TruncateTime(DateTime.Now)
                     && w.StateId == cStateId);
                SkuFinalpriceListInputDto inputDto = new SkuFinalpriceListInputDto();
                if (priceGenerateDetail != null)
                {
                    inputDto = new SkuFinalpriceListInputDto()
                    {
                        SaudaBookingTypeId = 1,
                        VerticalId = _context.PriceGenerate.AsNoTracking().FirstOrDefault(f => f.Id == priceGenerateDetail.PriceGenerateId).VerticalId,
                        OilTypeIds = priceGenerateDetail.OilTypeId.Split(',').Select(Int64.Parse).ToList(),
                        OilPackingTypeIds = priceGenerateDetail.PackGroupId.Split(',').Select(Int64.Parse).ToList(),
                        PlantId = priceGenerateDetail.PlantId,
                        StateIds = new List<long>() { priceGenerateDetail.StateId },
                        LoginUserId = priceGenerateDetail.CreatedBy
                    };
                }
                else
                {
                    _logger.Info("PriceGenerateDetail Table No Records Found");
                    return;
                }

                #endregion


                _context.Database.CommandTimeout = 0;
                string message = "";
                string saudaBookingType = "";

                saudaBookingType = //inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction
                                   // ? DTO.Enums.SaudaBookingTypes.ReverseAuction.ToString()
                                   //: 
                    (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess
                    ? DTO.Enums.SaudaBookingTypes.TraditionalProcess.ToString() : string.Empty);
                message = $"---------------------------------------- {saudaBookingType} Final Price Generate Started ----------------------------------------";
                _logger.Info(message);
                _methodName = "SkuFinalpriceListForAdmin";
                message = $"{ServiceName} Service-Method {_methodName} Process Start Date Time : " + DateHelper.UtcToIndia(DateTime.UtcNow);
                _logger.Info(message);

                var resultDtoMain = new ResultDto();
                var outputDtoMain = new List<SkuFinalpriceListOutputDto>();
                var errorListMain = new List<string>();
                var currentDate = DateHelper.UtcToIndia(DateTime.UtcNow);
                bool isAvailable = false;
                string smsContent = string.Empty;
                string mainErrorMessage = string.Empty;
                List<string> errorMessageList = new List<string>();
                int count = 0;
                //PricePublish pricePublishContext = new PricePublish();
                List<string> mobileNoList = UtilityHelper.ConvertStringToStringArray(inputDto.MobileNoList).ToList();
                List<Pricing> pricings = new List<Pricing>();
                var depotPlantIds = new List<long>();
                string skuLoopErrorMsg = "";
                string depotLoopErrorMsg = "";
                string stateLoopErrorMsg = "";
                string freightRouteLoopErrorMsg = "";

                try
                {

                    var skuList = _context.Skus.AsNoTracking()
                        .Where(_ => inputDto.OilTypeIds.Contains(_.OilTypeId ?? 0) && inputDto.OilPackingTypeIds.Contains(_.PackGroupId ?? 0) && _.IsActive)
                                       .Select(s => new { Id = s.Id, SkuName = s.SkuName, SkuCode = s.SkuCode, Quantity = s.Quantity, UomId = s.UomId, VerticalId = s.DivisionId, OilTypeId = s.OilTypeId, PackGroupId = s.PackGroupId, ProcessCost = s.ProcessCost }).ToList();
                    //Process the SKU's0
                    if (skuList != null && skuList.Any())
                    {
                        //Get Depots-----------------------------
                        var depotIds = _context.Users.AsNoTracking()
                                .Join(_context.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u = u, ur = ur })
                                .Join(_context.UserDepotMapping.AsNoTracking(), ud => ud.ur.UserId, udm => udm.UserId, (ud, udm) => new { udm.DepotId })
                                  .Select(s => s.DepotId).Distinct().ToList();

                        //inputDto.DepotIds = _context.PlantDepotMapping.AsNoTracking().Where(_ => _.PlantId == inputDto.PlantId).Select(_ => _.DepotId).ToList();
                        inputDto.DepotIds = _context.Depots.AsNoTracking().Join(_context.PlantDepotMapping.AsNoTracking(), d => d.Id, pd => pd.PlantId, (d, pd) => new { d, pd })
                            .Where(w => w.d.IsActive && w.pd.PlantId == inputDto.PlantId).Select(s => s.pd.DepotId).ToList();
                        inputDto.DepotIds = inputDto.DepotIds.Where(w => depotIds.Contains(w)).Select(s => s).ToList();

                        List<long> stateIds = inputDto.StateIds;
                        //Get Rasoi oiltypes
                        //var rasoiOilTypeIds = _context.OilTypes.AsNoTracking().Where(w => w.IsRasoi).Select(s => s.Id).ToList();


                        if (inputDto.DepotIds != null && inputDto.DepotIds.Any())
                        {
                            depotPlantIds.Add(inputDto.PlantId);
                            depotPlantIds.AddRange(inputDto.DepotIds);

                            //CommentedForAdani
                            //var freightRoutes = _context.Users.AsNoTracking().Join(_context.UserRoles.AsNoTracking(), u => u.Id, ur => ur.UserId, (u, ur) => new { u = u, ur = ur })
                            //     .Where(w => w.ur.RoleId == (long)DTO.Enums.Role.Dealer).Select(s => s.u.FreightRouteId).Distinct().ToList();

                            //Get Freight Route details-----------------------------
                            //var freightZoneId = _context.FreightZones.AsNoTracking().ToList().Where(w => inputDto.StateIds.Contains(Convert.ToInt32(w.StateId)) && w.IsActive)
                            //    .Select(s => s.Id).ToList();

                            //if (freightZoneId != null && freightZoneId.Any())
                            //{
                            //var freightRouteIds = _context.FreightRoutes.AsNoTracking()
                            //    .Where(_ => _.IsActive && freightZoneId.Contains(_.FreightZoneId) && freightRoutes.Contains(_.Id))
                            //    .Select(_ => _.Id).ToList();

                            ////New Freight Route
                            //var freightZonesDatas = _context.FreightZones.AsNoTracking().ToList().Where(w => inputDto.StateIds.Contains(Convert.ToInt32(w.StateId)) && w.IsActive)
                            //    .Select(s => new { Id = s.Id, StateId = s.StateId }).ToList();
                            //var fzIds = freightZonesDatas.Select(s => s.Id).Distinct();
                            //var freightRoutesDatas = _context.FreightRoutes.AsNoTracking().Where(w => fzIds.Contains(w.FreightZoneId))
                            //    .Select(s => new { Id = s.Id, FreightZoneId = s.FreightZoneId }).ToList();

                            //if (freightRouteIds != null && freightRouteIds.Any())
                            //{
                            //Get Transport Modes
                            //var transportModeData = _context.TransportModes.AsNoTracking().Where(w => w.IsActive).Select(s => s);
                            var transportModeData = _context.TransportModes.AsNoTracking().Where(_ => _.IsActive)
                                        .Select(s => new
                                        {
                                            Id = s.Id,
                                            Name = s.Name
                                        }).ToList();
                            var transportModes = transportModeData.Select(s => s.Id).ToList();
                            if (transportModes != null && transportModes.Any())
                            {
                                #region Price Generate Details
                                _logger.Info("Price Generate Started : " + DateHelper.UtcToIndia(DateTime.UtcNow));
                                priceGenerateDetail.StatusId = (int)DTO.Enums.PricePublishStatus.Started;
                                priceGenerateDetail.StartDate = DateTime.UtcNow;
                                _context.SaveChanges();
                                #endregion

                                #region Get Common data

                                var MaterialCostData = _context.MaterialCosts.AsNoTracking()
                                    .Where(_ => currentDate >= _.ValidFrom && currentDate <= _.ValidTo && _.IsActive)
                                    .Select(s => new
                                    {
                                        Id = s.Id,
                                        PlantId = s.PlantId,
                                        OilTypeId = s.OilTypeId,
                                        RatePerMt = s.RatePerMt
                                    }).ToList();

                                var PackingCostData = _context.PackingCosts.AsNoTracking()
                                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive)
                                    .Select(s => new
                                    {
                                        Id = s.Id,
                                        PlantId = s.PlantId,
                                        OilTypeId = s.OilTypeId,
                                        SalesPackingCost = s.SalesPackingCost,
                                        SkuId = s.SkuId
                                    }).ToList();

                                var DepotCostData = _context.DepotCosts.AsNoTracking()
                                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive)
                                    .Select(s => new
                                    {
                                        Id = s.Id,
                                        DepotId = s.DepotId,
                                        VerticalId = s.DivisionId,
                                        RatePerMt = s.RatePerMt
                                    }).ToList();


                                var DetentionCostData = _context.DetentionCosts.AsNoTracking()
                                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive)
                                    .Select(s => new
                                    {
                                        Id = s.Id,
                                        DepotId = s.DepotId,
                                        VerticalId = s.DivisionId,
                                        RatePerMt = s.RatePerMt
                                    }).ToList();


                                var ProfitMarginsData = _context.ProfitMargins.AsNoTracking()
                                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive)
                                    .Select(s => new
                                    {
                                        Id = s.Id,
                                        SkuId = s.SkuId,
                                        StateId = s.StateId,
                                        RatePerMt = s.RatePerMt
                                    }).ToList();

                                var CushionMarginData = _context.CushionMargins.AsNoTracking()
                                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive)
                                    .Select(s => new
                                    {
                                        Id = s.Id,
                                        SkuId = s.SkuId,
                                        StateId = s.StateId,
                                        RatePerMt = s.RatePerMt
                                    }).ToList();

                                var SchemeCostData = _context.SchemeCosts.AsNoTracking()
                                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive)
                                    .Select(s => new
                                    {
                                        Id = s.Id,
                                        PackGroupId = s.PackGroupId,
                                        OilTypeId = s.OilTypeId,
                                        StateId = s.StateId,
                                        RatePerMt = s.RatePerMt,
                                        SkuId = s.SkuId
                                    }).ToList();

                                var PrimaryFreightData = _context.PrimaryFreights.AsNoTracking()
                                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive)
                                    .Select(s => new
                                    {
                                        Id = s.Id,
                                        VerticalId = s.VerticalId,
                                        PlantId = s.PlantId,
                                        DepotId = s.DepotId,
                                        TransportModeId = s.TransportModeId,
                                        LoadCapacity = s.LoadCapacity,
                                        SalesFreight = s.SalesFreight
                                    }).ToList();

                                var SecondaryFreightData = _context.SecondaryFreights.AsNoTracking()
                                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive)
                                    .Select(s => new
                                    {
                                        Id = s.Id,
                                        VerticalId = s.VerticalId,
                                        DepotId = s.DepotId,
                                        FreightRouteId = s.FreightRouteId,
                                        TransportModeId = s.TransportModeId,
                                        Capacity = s.Capacity,
                                        SalesFreight = s.SalesFreight
                                    }).ToList();

                                var HoneycombCostData = _context.HoneycombCosts.AsNoTracking()
                                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive)
                                    .Select(s => new
                                    {
                                        Id = s.Id,
                                        PlantId = s.PlantId,
                                        StateId = s.StateId,
                                        SkuId = s.SkuId,
                                        RatePerMt = s.RatePerMt,
                                        TransportModeId = s.TransportModeId
                                    }).ToList();

                                var RaMarginData = _context.RaMargin.AsNoTracking()
                                    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive)
                                    .Select(s => new
                                    {
                                        Id = s.Id,
                                        SkuId = s.SkuId,
                                        OilPackingTypeId = s.OilPackingTypeId,
                                        StateId = s.StateId,
                                        RatePerMt = s.RatePerMt
                                    }).ToList();

                                var LoadCapacityConversionData = _context.LoadCapacityConversion.AsNoTracking()
                                   .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive)
                                   .Select(s => s).ToList();

                                //var PricingData = _context.Pricing.AsNoTracking().Where(w => DbFunctions.TruncateTime(w.CreatedDate) == DbFunctions.TruncateTime(currentDate)).ToList();

                                var PricingData = _context.Pricing.AsNoTracking()
                                                   .Where(w => DbFunctions.TruncateTime(w.CreatedDate) == DbFunctions.TruncateTime(currentDate)
                                                    && w.PlantId == inputDto.PlantId
                                                    //&& inputDto.DepotIds.Contains(w.DepotId)
                                                    //&& inputDto.OilTypeIds.Contains(w.OilTypeId)
                                                    //&& inputDto.StateIds.Contains(w.StateId)
                                                    )
                                                   .Select(s => new
                                                   {

                                                       SkuId = s.SkuId,
                                                       //OilTypeId = s.OilTypeId,
                                                       //SaudaBookingTypeId = s.SaudaBookingTypeId,
                                                       //OilPackingTypeId = s.OilPackingTypeId,
                                                       //StateId = s.StateId,
                                                       //CityId = s.CityId,
                                                       //TransportModeId = s.TransportModeId,
                                                       PlantId = s.PlantId,
                                                       //DepotId = s.DepotId,
                                                       //FrieghtZoneId = s.FrieghtZoneId,
                                                       //FrieghtRouteId = s.FrieghtRouteId,
                                                       //BiddingWindowId = s.BiddingWindowId,
                                                       //MaterialCost = s.MaterialCost,
                                                       //PackingCost = s.PackingCost,
                                                       //PrimaryFrieght = s.PrimaryFrieght,
                                                       //SecondaryFrieght = s.SecondaryFrieght,
                                                       //DepotCost = s.DepotCost,
                                                       //DetentionCost = s.DetentionCost,
                                                       //HoneycombCost = s.HoneycombCost,
                                                       //Margin = s.Margin,
                                                       //CushionMargin = s.CushionMargin,
                                                       //SchemeCostRecovery = s.SchemeCostRecovery,
                                                       //Discount = s.Discount,
                                                       //Premium = s.Premium,
                                                       //ProcessCost = s.ProcessCost,
                                                       //SumOfIngredientCost = s.SumOfIngredientCost,
                                                       //TpPrice = s.TpPrice,
                                                       //RaMargin = s.RaMargin,
                                                       //BaseRate = s.BaseRate,
                                                       //XMargin = s.XMargin,
                                                       //FinalRate = s.FinalRate,
                                                       //ExPlantPrice = s.ExPlantPrice,
                                                       //ForDepotPrice = s.ForDepotPrice,
                                                       //ForPlantPrice = s.ForPlantPrice,
                                                       //ExDepotPrice = s.ExDepotPrice,
                                                       //ExRakePrice = s.ExRakePrice,
                                                       //ForRakePrice = s.ForRakePrice,
                                                       //ClearanceRate = s.ClearanceRate,
                                                       //CounterBidOffer = s.CounterBidOffer,
                                                       //CounterBidLimit = s.CounterBidLimit,
                                                       //BpCpJumb = s.BpCpJumb
                                                   }).ToList();

                                var skuIds = skuList.Select(s => s.Id).ToList();
                                var SkuUomMappingData = _context.SkuUomMapping.AsNoTracking().Where(_ => skuIds.Contains(_.SkuId)).ToList();
                                //var SkuIngrediantData = _context.SkuIngrediant.AsNoTracking().Where(_ => skuIds.Contains(_.SkuId)).ToList(); //inputDto.PlantId
                                //var SkuIngrediantData = _context.SkuIngrediant.AsNoTracking()
                                //    .Join(_context.SkuIngrediantPlant.AsNoTracking(), si => si.SkuIngrediantPlantId, sp => sp.Id, (si, sp) => new { SkuIngrediant = si, SkuIngrediantPlant = sp })
                                //    .Where(w => skuIds.Contains(w.SkuIngrediantPlant.SkuId)
                                //    && w.SkuIngrediantPlant.PlantId == inputDto.PlantId
                                //    && DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(w.SkuIngrediantPlant.ValidFrom)
                                //    && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(w.SkuIngrediantPlant.ValidTo)
                                //    && w.SkuIngrediantPlant.IsActive)
                                //    .Select(s => new
                                //    {
                                //        SkuIngrediantPlantId = s.SkuIngrediantPlant.Id,
                                //        IngredientId = s.SkuIngrediant.IngredientId,
                                //        SkuId = s.SkuIngrediantPlant.SkuId,
                                //        Percentage = s.SkuIngrediant.Percentage,
                                //        OilTypeId = s.SkuIngrediant.OilTypeId
                                //    }).ToList();

                                //var ingrediantIds = (SkuIngrediantData != null && SkuIngrediantData.Any()) ? SkuIngrediantData.Select(s => s.IngredientId).ToList() : new List<long>();
                                //var IngredientCostData = _context.IngredientCost.AsNoTracking()
                                //    .Where(w => ingrediantIds.Contains(w.IngredientId)
                                //&& currentDate >= w.ValidFrom
                                //&& currentDate <= w.ValidTo
                                //&& w.IsActive
                                //&& w.PlantId == inputDto.PlantId).ToList();

                                var DepotsData = _context.Depots.AsNoTracking().Where(_ => inputDto.DepotIds.Contains(_.Id)).Select(s => new
                                {
                                    Id = s.Id,
                                    Name = s.Name,
                                    StorageTypeId = s.StorageTypeId,
                                    MappedStateIds = s.MappedStateId
                                }).ToList();

                                var StateData = _context.State.AsNoTracking().Where(_ => stateIds.Contains(_.Id) && _.IsActive).Select(s => new
                                {
                                    Id = s.Id,
                                    StateName = s.StateName
                                }).ToList();
                                //CommentedForAdani
                                //var FreightRoutesData = _context.FreightRoutes.AsNoTracking().Where(w => freightRouteIds.Contains(w.Id) && w.IsActive).Select(s => new
                                //{
                                //    Id = s.Id,
                                //    Name = s.Name,
                                //    FreightZoneId = s.FreightZoneId
                                //}).ToList();

                                var OilTypesData = _context.OilTypes.AsNoTracking().Where(_ => _.IsActive).ToList();

                                #endregion

                                #region Load Capacity filter based on PrimaryFreight and SecondaryFreight

                                var primaryFreightLoadCapacity = PrimaryFreightData
                                    .Where(w => w.VerticalId == inputDto.VerticalId && w.PlantId == inputDto.PlantId && inputDto.DepotIds.Contains(w.DepotId) && transportModes.Contains(w.TransportModeId))
                                    .Select(s => s.LoadCapacity).ToList();

                                var secondaryFreightLoadCapacity = SecondaryFreightData.Where(w => w.VerticalId == inputDto.VerticalId && depotPlantIds.Contains(w.DepotId)
                                 && transportModes.Contains(w.TransportModeId)).Select(s => s.Capacity).ToList();
                                var loadCapacities = new List<decimal>();
                                //loadCapacities = (primaryFreightLoadCapacity ?? new List<decimal>()).Concat(secondaryFreightLoadCapacity ?? new List<decimal>()).Distinct().ToList();
                                loadCapacities = (secondaryFreightLoadCapacity ?? new List<decimal>()).Distinct().ToList();

                                #endregion

                                foreach (var sku in skuList)
                                {
                                    skuLoopErrorMsg = "";
                                    var isValidSku = true;
                                    var valErrorMessage = sku.SkuName + " ~ " + sku.SkuCode + " ~ ~ ~ ~ ~ ~ " + Constants.MissingSkuRequiredField;

                                    #region Valid SKU Validation
                                    if (sku.Quantity <= 0)
                                    {
                                        valErrorMessage = Constants.BindErrorMessage(Constants.MissingSkuQuantityField, valErrorMessage);
                                        isValidSku = false;
                                    }

                                    if (sku.UomId == null || sku.UomId <= 0)
                                    {
                                        valErrorMessage = Constants.BindErrorMessage(Constants.MissingSkuPackSizeQuantityField, valErrorMessage);
                                        isValidSku = false;
                                    }

                                    ////If Vertical type is (SpecialityFat or HBC) and it contains rasoi oiltypes condition true
                                    ////if (sku.VerticalId == (int)DTO.Enums.Vertical.SpecialityFat || (sku.VerticalId == (int)DTO.Enums.Vertical.Hbc && (rasoiOilTypeIds != null && rasoiOilTypeIds.Any() && rasoiOilTypeIds.Contains(sku.OilTypeId.GetValueOrDefault()))))
                                    //{
                                    //    //If Vertical type is SpecialityFat or (HBC - Rasoi oiltypes), SKU ingredients is required
                                    //    //if (!_context.SkuIngrediant.AsNoTracking().Any(_ => _.SkuId == sku.Id))
                                    //    if (!SkuIngrediantData.Any(_ => _.SkuId == sku.Id))
                                    //    {
                                    //        valErrorMessage = Constants.BindErrorMessage(Constants.SkuIngredientNotAdded, valErrorMessage);
                                    //        isValidSku = false;
                                    //    }
                                    //}

                                    //if (!_context.SkuUomMapping.AsNoTracking().Any(_ => _.SkuId == sku.Id && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos))
                                    if (!SkuUomMappingData.Any(_ => _.SkuId == sku.Id && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos))
                                    {
                                        valErrorMessage = Constants.BindErrorMessage(Constants.MissingSkuUom2Field, valErrorMessage);
                                        isValidSku = false;
                                    }

                                    //if (!_context.SkuUomMapping.AsNoTracking().Any(_ => _.SkuId == sku.Id && _.UomId == (int)DTO.Enums.Uom.MT && _.RelationUomId == (int)DTO.Enums.Uom.Nos))
                                    if (!SkuUomMappingData.Any(_ => _.SkuId == sku.Id && _.UomId == (int)DTO.Enums.Uom.MT && _.RelationUomId == (int)DTO.Enums.Uom.Nos))
                                    {
                                        valErrorMessage = Constants.BindErrorMessage(Constants.MissingSkuUom3Field, valErrorMessage);
                                        isValidSku = false;
                                    }

                                    #endregion

                                    if (isValidSku)
                                    {
                                        //_logger.Info("Valid  SKU Calculation Start :" + DateHelper.UtcToIndia(DateTime.UtcNow));
                                        inputDto.SkuId = sku.Id;
                                        List<long> ingredientCostId = new List<long>();
                                        var resultDtoList = new List<ResultDto>();
                                        var verticalId = 0L;
                                        var oilTypeId = 0L;
                                        var oilPackingTypeId = 0L;
                                        var uomId = 0L;
                                        var skuId = inputDto.SkuId;
                                        var plantId = inputDto.PlantId;
                                        long skuIngrediantPlantId = 0;
                                        //var dataMissingErrorMessage = Constants.DataMissingToCalculate;

                                        decimal litreConversion = 0;
                                        decimal quantity = 0;
                                        decimal materialCost = 0;
                                        decimal packingCost = 0;
                                        decimal noofPiecesperCase = 0;

                                        long materialCostId = 0;
                                        long packingCostId = 0;

                                        bool isError = false;

                                        //Get SKU details
                                        var skuContext = sku;         // _context.Skus.AsNoTracking().FirstOrDefault(_ => _.Id == skuId);
                                        oilTypeId = Convert.ToInt64(skuContext.OilTypeId);
                                        oilPackingTypeId = Convert.ToInt64(skuContext.PackGroupId);
                                        uomId = Convert.ToInt64(skuContext.UomId);
                                        quantity = skuContext.Quantity;
                                        var dataTitleMissingErrorMessage = skuContext.SkuName + " ~ " + skuContext.SkuCode + " ~ ~ ~ ~ ~ ~ ";

                                        //Get OilType details
                                        //var oilTypeContext = _context.OilTypes.AsNoTracking().FirstOrDefault(_ => _.Id == oilTypeId);
                                        var oilTypeContext = OilTypesData.FirstOrDefault(_ => _.Id == oilTypeId);
                                        verticalId = oilTypeContext.DivisionId;
                                       // litreConversion = oilTypeContext.LitreConversion;

                                        //var skuUomContext = _context.SkuUomMapping.AsNoTracking().FirstOrDefault(_ => _.SkuId == skuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                                        var skuUomContext = SkuUomMappingData.FirstOrDefault(_ => _.SkuId == skuId && _.UomId == (int)DTO.Enums.Uom.Case && _.RelationUomId == (int)DTO.Enums.Uom.Nos);
                                        if (skuUomContext != null)
                                        {
                                            //noofPiecesperCase = skuUomContext.ConversionFactor;
                                        }

                                        decimal formulationCost = 0;
                                        //if (verticalId == (int)DTO.Enums.Vertical.Hbc && ((rasoiOilTypeIds == null || !rasoiOilTypeIds.Any()) || !rasoiOilTypeIds.Contains(oilTypeId)))
                                        //{
                                        //    #region Material Cost calculations
                                        //    var materialCostContext = MaterialCostData.FirstOrDefault(_ => _.PlantId == plantId && _.OilTypeId == oilTypeId);
                                        //    if (materialCostContext != null)
                                        //    {
                                        //        materialCost = _resultService.GetSkuQuanityRate(uomId, quantity, materialCostContext.RatePerMt, litreConversion);
                                        //        materialCost = noofPiecesperCase * materialCost;
                                        //        materialCostId = materialCostContext.Id;
                                        //    }
                                        //    else
                                        //    {
                                        //        isError = true;
                                        //        skuLoopErrorMsg = Constants.BindErrorMessage(Constants.DataMissingToMaterialCost, skuLoopErrorMsg);
                                        //        //dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToMaterialCost, dataMissingErrorMessage);
                                        //    }
                                        //    #endregion
                                        //}
                                        //else if (verticalId == (int)DTO.Enums.Vertical.SpecialityFat || (verticalId == (int)DTO.Enums.Vertical.Hbc && (rasoiOilTypeIds != null && rasoiOilTypeIds.Any() && rasoiOilTypeIds.Contains(oilTypeId))))
                                        //{

                                        //    #region SKU Ingredients calculations
                                        //    //var skuIngredientList = _context.SkuIngrediant.AsNoTracking().Where(_ => _.SkuId == skuId && _.OilTypeId == oilTypeId).ToList();
                                        //    var skuIngredientList = SkuIngrediantData.Where(_ => _.SkuId == skuId && _.OilTypeId == oilTypeId).ToList();
                                        //    skuIngrediantPlantId = SkuIngrediantData.FirstOrDefault().SkuIngrediantPlantId;

                                        //    foreach (var skuIngredient in skuIngredientList)
                                        //    {
                                        //        //var ingredientCost = _context.IngredientCost.AsNoTracking()
                                        //        //    .Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive).FirstOrDefault(_ => _.IngredientId == skuIngredient.IngredientId);
                                        //        //var ingredientCost = IngredientCostData.Where(_ => DbFunctions.TruncateTime(currentDate) >= DbFunctions.TruncateTime(_.ValidFrom) && DbFunctions.TruncateTime(currentDate) <= DbFunctions.TruncateTime(_.ValidTo) && _.IsActive)
                                        //        //       .FirstOrDefault(_ => _.IngredientId == skuIngredient.IngredientId);
                                        //        var ingredientCost = IngredientCostData
                                        //              .FirstOrDefault(_ =>
                                        //               _.IsActive && _.IngredientId == skuIngredient.IngredientId);
                                        //        if (ingredientCost != null)
                                        //        {
                                        //            var oneKgIngredientCost = (ingredientCost.LooseOilRate * skuIngredient.Percentage) / 100;
                                        //            formulationCost = formulationCost + oneKgIngredientCost;
                                        //            ingredientCostId.Add(ingredientCost.Id);
                                        //        }
                                        //        else
                                        //        {
                                        //            isError = true;
                                        //            skuLoopErrorMsg = Constants.BindErrorMessage(Constants.DataMissingToIngredientCost, skuLoopErrorMsg);
                                        //            //dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToIngredientCost, dataMissingErrorMessage);
                                        //        }
                                        //    }

                                        //    var specialityFatMaterialCost = formulationCost + skuContext.ProcessCost;

                                        //    if (verticalId == (int)DTO.Enums.Vertical.SpecialityFat)
                                        //    {
                                        //        var noofPiecesperCaseConstant = quantity * Constants.SFNoOfPiiceConstant;
                                        //        var kgToLtrConstant = 1000 * Constants.SFKgtoLtrConstant;
                                        //        var kp = kgToLtrConstant / DecimalFormat2(noofPiecesperCaseConstant);
                                        //        materialCost = specialityFatMaterialCost / kp;
                                        //        formulationCost = formulationCost / kp;
                                        //    }
                                        //    else
                                        //    {
                                        //        materialCost = _resultService.GetSkuQuanityRate(uomId, quantity, specialityFatMaterialCost, litreConversion);
                                        //        materialCost = noofPiecesperCase * materialCost;
                                        //        formulationCost = _resultService.GetSkuQuanityRate(uomId, quantity, formulationCost, litreConversion);
                                        //        formulationCost = noofPiecesperCase * formulationCost;
                                        //    }
                                        //    #endregion

                                        //}

                                        #region Packing Cost calculations
                                        var packingCostContext = PackingCostData.FirstOrDefault(_ => _.PlantId == plantId && _.SkuId == skuId);
                                        if (packingCostContext != null)
                                        {
                                            packingCost = packingCostContext.SalesPackingCost;
                                            packingCostId = packingCostContext.Id;
                                        }
                                        else
                                        {
                                            isError = true;
                                            skuLoopErrorMsg = Constants.BindErrorMessage(Constants.DataMissingToPackingCost, skuLoopErrorMsg);
                                            //dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToPackingCost, dataMissingErrorMessage); 
                                        }
                                        #endregion

                                        foreach (long depotId in inputDto.DepotIds)
                                        {
                                            depotLoopErrorMsg = "";
                                            var isDepotError = false;
                                            decimal depoCost = 0;
                                            decimal detentionCost = 0;
                                            long depotCostId = 0;
                                            long detentionCostId = 0;
                                            var depot = DepotsData.FirstOrDefault(_ => _.Id == depotId);
                                            var rakeMappedStateIds = !string.IsNullOrEmpty(depot.MappedStateIds) ? UtilityHelper.ConvertStringToLongList(depot.MappedStateIds) :
                                                new List<long>();
                                            //DepotsData.Select(s => Convert.ToInt64(s.MappedStateIds)).Distinct().ToList();
                                            dataTitleMissingErrorMessage = skuContext.SkuName + " ~ " + skuContext.SkuCode + " ~ " + depot?.Name + " ~ ~ ~ ~ ~ ";

                                            //var depotName = DepotsData.FirstOrDefault(_ => _.Id == depotId)?.Name;
                                            //var depotName = _context.Depots.AsNoTracking().FirstOrDefault(_ => _.Id == depotId)?.Name;

                                            #region Depot Cost calculations
                                            var depoCostContext = DepotCostData.FirstOrDefault(_ => _.DepotId == depotId && _.VerticalId == verticalId);
                                            if (depoCostContext != null)
                                            {
                                                depoCost = _resultService.GetSkuQuanityRate(uomId, quantity, depoCostContext.RatePerMt, litreConversion);
                                                depoCost = noofPiecesperCase * depoCost;
                                                depotCostId = depoCostContext.Id;
                                            }
                                            else
                                            {
                                                isDepotError = true;
                                                depotLoopErrorMsg = Constants.BindErrorMessage(Constants.DataMissingToDepoCost, depotLoopErrorMsg);
                                                //dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToDepoCost, dataMissingErrorMessage);
                                            }
                                            #endregion

                                            #region Detention Cost calculations
                                            var detentionCostContext = DetentionCostData.FirstOrDefault(_ => _.DepotId == depotId && _.VerticalId == verticalId);
                                            if (detentionCostContext != null)
                                            {
                                                detentionCost = _resultService.GetSkuQuanityRate(uomId, quantity, detentionCostContext.RatePerMt, litreConversion);
                                                detentionCost = noofPiecesperCase * detentionCost;
                                                detentionCostId = detentionCostContext.Id;
                                            }
                                            else
                                            {
                                                isDepotError = true;
                                                depotLoopErrorMsg = Constants.BindErrorMessage(Constants.DataMissingToDetentionCost, depotLoopErrorMsg);
                                                //dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToDetentionCost, dataMissingErrorMessage); 
                                            }
                                            #endregion

                                            foreach (var stateId in stateIds)
                                            {
                                                //CommentedForAdani
                                                //#region State Validation
                                                //var isValidState = _context.FreightZones.AsNoTracking().Any(w => w.StateId == stateId && w.IsActive);
                                                //if (!isValidState) { continue; }

                                                //List<long> freightRouteIdsNew = new List<long>();
                                                //var freightZoneIdNew = freightZonesDatas.Where(w => w.StateId == stateId).Select(s => s.Id).ToList();
                                                //if (freightZoneIdNew != null && freightZoneIdNew.Any())
                                                //{
                                                //    freightRouteIdsNew = freightRoutesDatas.Where(_ => freightZoneIdNew.Contains(_.FreightZoneId) && freightRoutes.Contains(_.Id))
                                                //       .Select(_ => _.Id).ToList();
                                                //}
                                                //#endregion

                                                stateLoopErrorMsg = "";
                                                var isStateError = false;
                                                decimal marginCost = 0;
                                                decimal cushionMarginCost = 0;
                                                decimal schemeCostRecovery = 0;
                                                long marginCostId = 0;
                                                long cushionMarginCostId = 0;
                                                long schemeCostRecoveryId = 0;
                                                decimal raMarginCost = 0;
                                                long raMarginCostId = 0;

                                                //var stateName = _context.State.AsNoTracking().FirstOrDefault(_ => _.Id == stateId)?.StateName;
                                                var stateName = StateData.FirstOrDefault(_ => _.Id == stateId)?.StateName;
                                                dataTitleMissingErrorMessage = skuContext.SkuName + " ~ " + skuContext.SkuCode + " ~ " + depot?.Name + " ~ " + stateName + " ~ ~ ~ ~ ";

                                                //Traditional Process
                                                if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess)
                                                {

                                                    #region Profit Margins calculations
                                                    var marginCostContext = ProfitMarginsData.FirstOrDefault(_ => _.SkuId == skuId && _.StateId == stateId);
                                                    if (marginCostContext != null)
                                                    {
                                                        marginCost = _resultService.GetSkuQuanityRate(uomId, quantity, marginCostContext.RatePerMt, litreConversion);
                                                        marginCost = noofPiecesperCase * marginCost;
                                                        marginCostId = marginCostContext.Id;
                                                    }
                                                    else
                                                    {
                                                        isStateError = true;
                                                        stateLoopErrorMsg = Constants.BindErrorMessage(Constants.DataMissingToMarginCost, stateLoopErrorMsg);
                                                        //dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToMarginCost, dataMissingErrorMessage);
                                                    }
                                                    #endregion

                                                    #region Cushion Margin Cost calculations
                                                    var cushionMarginCostContext = CushionMarginData.FirstOrDefault(_ => _.SkuId == skuId && _.StateId == stateId);
                                                    if (cushionMarginCostContext != null)
                                                    {
                                                        cushionMarginCost = _resultService.GetSkuQuanityRate(uomId, quantity, cushionMarginCostContext.RatePerMt, litreConversion);
                                                        cushionMarginCost = noofPiecesperCase * cushionMarginCost;
                                                        cushionMarginCostId = cushionMarginCostContext.Id;
                                                    }
                                                    else
                                                    {
                                                        isStateError = true;
                                                        stateLoopErrorMsg = Constants.BindErrorMessage(Constants.DataMissingToCushionMarginCost, stateLoopErrorMsg);
                                                        //dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToCushionMarginCost, dataMissingErrorMessage);
                                                    }
                                                    #endregion

                                                }

                                                //Reverse Auction
                                                //if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                                //{

                                                //    #region RaMargin Cost calculations
                                                //    var raMarginCostContext = RaMarginData.FirstOrDefault(_ => _.SkuId == skuId && _.StateId == stateId &&
                                                //                                                       _.OilPackingTypeId == oilPackingTypeId);
                                                //    if (raMarginCostContext != null)
                                                //    {
                                                //        raMarginCost = _resultService.GetSkuQuanityRate(uomId, quantity, raMarginCostContext.RatePerMt, litreConversion);
                                                //        raMarginCost = noofPiecesperCase * raMarginCost;
                                                //        raMarginCostId = raMarginCostContext.Id;
                                                //    }
                                                //    else
                                                //    {
                                                //        isStateError = true;
                                                //        stateLoopErrorMsg = Constants.BindErrorMessage(Constants.DataMissingToRAMarginCost, stateLoopErrorMsg);
                                                //        //dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToRAMarginCost + " - ", dataMissingErrorMessage);
                                                //    }
                                                //    #endregion

                                                //}

                                                #region Scheme Cost Recovery calculations
                                                var schemeCostContext = SchemeCostData.FirstOrDefault(_ => _.PackGroupId == sku.PackGroupId && _.OilTypeId == sku.OilTypeId && _.StateId == stateId && _.SkuId == skuId);
                                                if (schemeCostContext != null)
                                                {
                                                    schemeCostRecovery = _resultService.GetSkuQuanityRate(uomId, quantity, schemeCostContext.RatePerMt, litreConversion);
                                                    schemeCostRecovery = noofPiecesperCase * schemeCostRecovery;
                                                    schemeCostRecoveryId = schemeCostContext.Id;
                                                }
                                                #endregion

                                                //foreach (var freightRouteId in freightRouteIdsNew)
                                                //{
                                                //    //var freightRouteName = _context.FreightRoutes.AsNoTracking().FirstOrDefault(_ => _.Id == freightRouteId)?.Name;
                                                //    var freightRoute = FreightRoutesData.FirstOrDefault(_ => _.Id == freightRouteId);
                                                //    var freightRouteName = freightRoute?.Name;

                                                foreach (var transportId in transportModes)
                                                {
                                                    if (transportId == (int)DTO.Enums.TransportMode.Rake && depot.StorageTypeId == (int)DTO.Enums.StorageType.Depot)
                                                    {
                                                        continue;
                                                    }

                                                    if (transportId == (int)DTO.Enums.TransportMode.Truck && depot.StorageTypeId == (int)DTO.Enums.StorageType.Rake)
                                                    {
                                                        continue;
                                                    }

                                                    if (transportId == (int)DTO.Enums.TransportMode.Rake && depot.StorageTypeId == (int)DTO.Enums.StorageType.Rake)
                                                    {
                                                        if (!rakeMappedStateIds.Contains(stateId))
                                                        {
                                                            //string commonMessage = ErrorMessageFormat(skuLoopErrorMsg, depotLoopErrorMsg, stateLoopErrorMsg, freightRouteLoopErrorMsg);
                                                            //var finalDataMissingErrorMessage = dataTitleMissingErrorMessage + commonMessage + " Rake state not mapped " + "|";
                                                            //errorMessageList.Add(finalDataMissingErrorMessage);
                                                            continue;
                                                        }
                                                    }

                                                    freightRouteLoopErrorMsg = "";
                                                    var isFrieghtRouteError = false;
                                                    var transportMode = string.Empty;
                                                    decimal honeycombCost = 0;
                                                    long honeycombCostId = 0;

                                                    var transportModeName = transportModeData.FirstOrDefault(_ => _.Id == transportId)?.Name;

                                                    dataTitleMissingErrorMessage = skuContext.SkuName + " ~ " + skuContext.SkuCode + " ~ " + depot?.Name
                                                                               + " ~ " + stateName
                                                                               //+ " ~ " + freightRouteName
                                                                               + " ~ " + transportModeName + " ~ ~ ";

                                                    var loadCapacityContextList = LoadCapacityConversionData.Where(_ => _.SkuId == skuId
                                                                                                 && _.DivisionId == verticalId && _.TransportModeId == transportId && loadCapacities.Contains(_.LoadCapacity)).ToList();



                                                    #region Honeycomb Cost calculations
                                                    var honeycombCostContext = HoneycombCostData.FirstOrDefault(_ => _.PlantId == plantId && _.StateId == stateId &&
                                                                                                       _.SkuId == skuId && _.TransportModeId == transportId);
                                                    if (honeycombCostContext != null)
                                                    {
                                                        honeycombCost = _resultService.GetSkuQuanityRate(uomId, quantity, honeycombCostContext.RatePerMt, litreConversion);
                                                        honeycombCost = noofPiecesperCase * honeycombCost;
                                                        honeycombCostId = honeycombCostContext.Id;
                                                    }
                                                    else
                                                    {
                                                        isFrieghtRouteError = true;
                                                        freightRouteLoopErrorMsg = Constants.BindErrorMessage(Constants.DataMissingToHoneyCombCost, freightRouteLoopErrorMsg);
                                                        //dataMissingErrorMessage = Constants.BindErrorMessage(Constants.DataMissingToHoneyCombCost, dataMissingErrorMessage);
                                                    }
                                                    #endregion


                                                    if ((loadCapacities != null && loadCapacities.Any() && loadCapacityContextList != null && loadCapacityContextList.Any())
                                                           || (!loadCapacities.Any() && loadCapacityContextList == null || !loadCapacityContextList.Any()))
                                                    {
                                                        if (loadCapacityContextList == null || !loadCapacityContextList.Any())
                                                        {
                                                            var loadCapacityItem = new LoadCapacityConversion
                                                            {
                                                                Id = 0
                                                            };
                                                            loadCapacityContextList.Add(loadCapacityItem);
                                                        }

                                                        foreach (var loadCapacityItem in loadCapacityContextList)
                                                        {
                                                            decimal primaryFrieght = 0;
                                                            decimal secondaryFrieght = 0;
                                                            decimal discount = 0;
                                                            decimal premium = 0;
                                                            decimal secondaryFrieghtForPlant = 0;
                                                            decimal exPlantPrice = 0;
                                                            decimal forPlantPrice = 0;
                                                            decimal exDepotPrice = 0;
                                                            decimal exRakePrice = 0;
                                                            decimal finalPrice = 0;
                                                            long primaryFrieghtId = 0;
                                                            long secondaryFrieghtId = 0;
                                                            long secondaryFrieghtForPlantId = 0;
                                                            var resultDtoSub = new ResultDto();
                                                            var loadCapacity = loadCapacityItem.LoadCapacity;
                                                            var loadQuantityCase = loadCapacityItem.LoadQuantity;

                                                            dataTitleMissingErrorMessage = skuContext.SkuName + " ~ " + skuContext.SkuCode + " ~ " + depot?.Name
                                                                                       + " ~ " + stateName
                                                                                       //+ " ~ " + freightRouteName 
                                                                                       + " ~ " + transportModeName + " ~ " +
                                                                                       loadCapacity + " ~ ";


                                                            #region Primary Freight Calculation
                                                            //var primaryFrieghtContext = PrimaryFreightData.FirstOrDefault(_ => _.PlantId == plantId && _.DepotId == depotId &&
                                                            //                                                               _.VerticalId == verticalId && _.TransportModeId == transportId && _.LoadCapacity == Constants.DefaultLoadQuantity);
                                                            var primaryFrieghtContext = PrimaryFreightData.FirstOrDefault(_ => _.PlantId == plantId && _.DepotId == depotId &&
                                                                                                                           _.VerticalId == verticalId && _.TransportModeId == transportId);
                                                            if (primaryFrieghtContext != null)
                                                            {
                                                                //var defaultLoadCapacity16MT = LoadCapacityConversionData.FirstOrDefault(_ => _.OilTypeId == oilTypeId
                                                                //   && _.VerticalId == verticalId && _.TransportModeId == transportId && _.LoadCapacity == Constants.DefaultLoadQuantity
                                                                //   && _.SkuId == skuId);
                                                                var defaultLoadCapacity16MT = LoadCapacityConversionData.FirstOrDefault(_ => _.OilTypeId == oilTypeId
                                                                   && _.DivisionId == verticalId && _.TransportModeId == transportId && _.LoadCapacity == primaryFrieghtContext.LoadCapacity
                                                                   && _.SkuId == skuId);

                                                                if (defaultLoadCapacity16MT != null && defaultLoadCapacity16MT.LoadQuantity > 0)
                                                                {
                                                                    primaryFrieght = primaryFrieghtContext.SalesFreight;
                                                                    primaryFrieght = (primaryFrieght / defaultLoadCapacity16MT.LoadQuantity) * 1;
                                                                    primaryFrieghtId = primaryFrieghtContext.Id;
                                                                }
                                                            }
                                                            //else
                                                            //{
                                                            //    isError = true;
                                                            //} 
                                                            #endregion

                                                            #region Secondary Freight Calculations
                                                            var secondaryFrieghtContext = SecondaryFreightData.FirstOrDefault(_ => _.TransportModeId == transportId && _.Capacity == loadCapacity && _.DepotId == depotId && _.VerticalId == verticalId);
                                                            if (secondaryFrieghtContext != null && loadQuantityCase > 0)
                                                            {
                                                                secondaryFrieght = secondaryFrieghtContext.SalesFreight;
                                                                secondaryFrieght = (secondaryFrieght / loadQuantityCase) * 1;
                                                                secondaryFrieghtId = secondaryFrieghtContext.Id;
                                                            }
                                                            //else
                                                            //{
                                                            //    isError = true;
                                                            //} 
                                                            #endregion

                                                            #region secondary Frieght For Plant
                                                            var secondaryFrieghtContextForPlant = SecondaryFreightData.FirstOrDefault(_ => _.TransportModeId == transportId && _.Capacity == loadCapacity && _.DepotId == plantId && _.VerticalId == verticalId);
                                                            if (secondaryFrieghtContextForPlant != null && loadQuantityCase > 0)
                                                            {
                                                                secondaryFrieghtForPlant = secondaryFrieghtContextForPlant.SalesFreight;
                                                                secondaryFrieghtForPlant = (secondaryFrieghtForPlant / loadQuantityCase) * 1;
                                                                secondaryFrieghtForPlantId = secondaryFrieghtContextForPlant.Id;
                                                            }
                                                            //else
                                                            //{
                                                            //    isError = true;
                                                            //} 
                                                            #endregion

                                                            if (!isError && !isStateError && !isDepotError && !isFrieghtRouteError)
                                                            {
                                                                if (primaryFrieght > 0 && secondaryFrieght > 0)
                                                                {
                                                                    finalPrice = ((materialCost + packingCost + primaryFrieght + secondaryFrieght + depoCost + detentionCost +
                                                                                         honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                                }

                                                                //Ex Plant Price
                                                                if (depot.StorageTypeId == (int)DTO.Enums.StorageType.Depot)
                                                                {
                                                                    exPlantPrice = ((materialCost + packingCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                                }
                                                                //For Plant Price
                                                                if (depot.StorageTypeId == (int)DTO.Enums.StorageType.Depot && secondaryFrieghtForPlant > 0)
                                                                {
                                                                    forPlantPrice = ((materialCost + packingCost + secondaryFrieghtForPlant +
                                                                                             honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                                }
                                                                //Ex Depot Price
                                                                if (depot.StorageTypeId == (int)DTO.Enums.StorageType.Depot && primaryFrieght > 0)
                                                                {
                                                                    exDepotPrice = ((materialCost + packingCost + primaryFrieght + depoCost + detentionCost +
                                                                                                    marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                                }
                                                                //Ex Rake Price
                                                                if (depot.StorageTypeId == (int)DTO.Enums.StorageType.Rake && primaryFrieght > 0)
                                                                {
                                                                    exRakePrice = ((materialCost + packingCost + primaryFrieght +
                                                                                            honeycombCost + marginCost + cushionMarginCost + schemeCostRecovery) - discount) + premium;
                                                                }

                                                                var pricingContext = new Pricing()
                                                                {
                                                                    SkuId = skuId,
                                                                    //OilTypeId = oilTypeId,
                                                                    //OilPackingTypeId = oilPackingTypeId,
                                                                    PlantId = plantId,
                                                                    //DepotId = depotId,
                                                                    //StateId = (int)stateId,
                                                                    //FrieghtRouteId = freightRouteId,
                                                                    //FrieghtZoneId = freightRoute?.FreightZoneId ?? 0,
                                                                    //TransportModeId = transportId,
                                                                    //LoadQuantity = loadCapacity,
                                                                    //SumOfIngredientCost = formulationCost,
                                                                    CreatedBy = inputDto.LoginUserId,
                                                                    CreatedDate = DateHelper.UtcToIndia(DateTime.UtcNow),
                                                                    //IsActive = true,
                                                                };

                                                                //if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                                                //{

                                                                //    exDepotPrice = exDepotPrice > 0 ? (exDepotPrice + raMarginCost) : 0;
                                                                //    exPlantPrice = exPlantPrice + raMarginCost;
                                                                //    forPlantPrice = forPlantPrice > 0 ? (forPlantPrice + raMarginCost) : 0;
                                                                //    exRakePrice = exRakePrice > 0 ? (exRakePrice + raMarginCost) : 0;

                                                                //    ////For Depot Price
                                                                //    //if (depot.StorageTypeId == (int)DTO.Enums.StorageType.Depot)
                                                                //    //    pricingContext.ForDepotPrice = finalPrice > 0 ? (finalPrice + raMarginCost) : 0;

                                                                //    ////For Rake Price
                                                                //    //if (depot.StorageTypeId == (int)DTO.Enums.StorageType.Rake)
                                                                //    //    pricingContext.ForRakePrice = finalPrice > 0 ? (finalPrice + raMarginCost) : 0;


                                                                //    //pricingContext.TpPrice = DecimalFormat2(exPlantPrice);
                                                                //    finalPrice = exPlantPrice > 0 ? DecimalFormat2((exPlantPrice + raMarginCost)) : 0;
                                                                //    //pricingContext.ClearanceRate = finalPrice > 0 ? DecimalFormat2((finalPrice * inputDto.CounterBidLimit)) : 0;
                                                                //    //pricingContext.CounterBidOffer = finalPrice > 0 ? DecimalFormat2((finalPrice + inputDto.BpCpJump)) : 0;
                                                                //    //pricingContext.BaseRate = DecimalFormat2(finalPrice);
                                                                //    //pricingContext.XMargin = DecimalFormat2(inputDto.XMargin);
                                                                //    //pricingContext.FinalRate = finalPrice > 0 ? DecimalFormat2((finalPrice + inputDto.XMargin)) : 0;
                                                                //    //pricingContext.CounterBidLimit = DecimalFormat2(inputDto.CounterBidLimit);
                                                                //    //pricingContext.BpCpJumb = DecimalFormat2(inputDto.BpCpJump);
                                                                //    //pricingContext.SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.ReverseAuction;
                                                                //    //pricingContext.BiddingWindowId = inputDto.BiddingWindowId;
                                                                //}
                                                                //else
                                                                //{
                                                                ////For Depot Price
                                                                //if (depot.StorageTypeId == (int)DTO.Enums.StorageType.Depot)
                                                                //    pricingContext.ForDepotPrice = DecimalFormat2(finalPrice);

                                                                ////For Rake Price
                                                                //if (depot.StorageTypeId == (int)DTO.Enums.StorageType.Rake)
                                                                //    pricingContext.ForRakePrice = DecimalFormat2(finalPrice);

                                                                pricingContext.Price = DecimalFormat2(finalPrice);
                                                                //pricingContext.SaudaBookingTypeId = (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess;
                                                                //pricingContext.TpPrice = DecimalFormat2(finalPrice);
                                                                //}
                                                                //pricingContext.MaterialCost = DecimalFormat2(materialCost);
                                                                //pricingContext.PackingCost = DecimalFormat2(packingCost);
                                                                //pricingContext.Premium = DecimalFormat2(premium);
                                                                //pricingContext.Discount = DecimalFormat2(discount);
                                                                //pricingContext.PrimaryFrieght = DecimalFormat2(primaryFrieght);
                                                                //pricingContext.SecondaryFrieght = DecimalFormat2(secondaryFrieght);
                                                                //pricingContext.PlantSecondaryFrieght = DecimalFormat2(secondaryFrieghtForPlant);
                                                                //pricingContext.DepotCost = DecimalFormat2(depoCost);
                                                                //pricingContext.DetentionCost = DecimalFormat2(detentionCost);
                                                                //pricingContext.HoneycombCost = DecimalFormat2(honeycombCost);
                                                                //pricingContext.Margin = DecimalFormat2(marginCost);
                                                                //pricingContext.CushionMargin = DecimalFormat2(cushionMarginCost);
                                                                //pricingContext.SchemeCostRecovery = DecimalFormat2(schemeCostRecovery);
                                                                //pricingContext.ProcessCost = DecimalFormat2(schemeCostRecovery);
                                                                //pricingContext.RaMargin = DecimalFormat2(raMarginCost);

                                                                //pricingContext.ExPlantPrice = DecimalFormat2(exPlantPrice);
                                                                //pricingContext.ExDepotPrice = DecimalFormat2(exDepotPrice);
                                                                //pricingContext.ForPlantPrice = DecimalFormat2(forPlantPrice);
                                                                //pricingContext.ExRakePrice = DecimalFormat2(exRakePrice);
                                                                //pricingContext.MaterialCostId = materialCostId;
                                                                //pricingContext.IngredientCostId = (ingredientCostId != null && ingredientCostId.Any()) ? string.Join(",", ingredientCostId) : "";
                                                                //pricingContext.PackingCostId = packingCostId;
                                                                //pricingContext.DepotCostId = depotCostId;
                                                                //pricingContext.DetentionCostId = detentionCostId;
                                                                //pricingContext.ProfitMarginId = marginCostId;
                                                                //pricingContext.CushionMarginId = cushionMarginCostId;
                                                                //pricingContext.SchemeCostId = schemeCostRecoveryId;
                                                                //pricingContext.PrimaryFrieghtId = primaryFrieghtId;
                                                                //pricingContext.SecondaryFrieghtId = secondaryFrieghtId;
                                                                //pricingContext.SecondaryFrieghtForPlantId = secondaryFrieghtForPlantId;
                                                                //pricingContext.HoneycombCostId = honeycombCostId;
                                                                //pricingContext.RaMarginId = raMarginCostId;
                                                                //pricingContext.LoadCapacityId = loadCapacityItem.Id;
                                                                //pricingContext.SkuIngrediantPlantId = skuIngrediantPlantId;

                                                                if (!isError)
                                                                {
                                                                    //if (depot.StorageTypeId == (int)DTO.Enums.StorageType.Rake && pricingContext.ExRakePrice == 0 && pricingContext.ForRakePrice == 0)
                                                                    //{
                                                                    //    string commonMessage = ErrorMessageFormat(skuLoopErrorMsg, depotLoopErrorMsg, stateLoopErrorMsg, freightRouteLoopErrorMsg);
                                                                    //    var finalDataMissingErrorMessage = dataTitleMissingErrorMessage + commonMessage + " Data missing to calculate - Primary Frieght" + "|";
                                                                    //    errorMessageList.Add(finalDataMissingErrorMessage);
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    if (PricingData != null && PricingData.Any())
                                                                    {
                                                                        List<Pricing> publishedPrice = new List<Pricing>();
                                                                        bool isValidPrice = false;

                                                                        if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.TraditionalProcess)
                                                                        {
                                                                            isValidPrice = PricingData.Any(w => w.SkuId == pricingContext.SkuId
                                                                                   //&& w.OilTypeId == pricingContext.OilTypeId
                                                                                   //&& w.SaudaBookingTypeId == pricingContext.SaudaBookingTypeId
                                                                                   //&& w.OilPackingTypeId == pricingContext.OilPackingTypeId
                                                                                   //&& w.StateId == pricingContext.StateId
                                                                                   //&& w.CityId == pricingContext.CityId
                                                                                   //&& w.TransportModeId == pricingContext.TransportModeId
                                                                                   && w.PlantId == pricingContext.PlantId
                                                                                   //&& w.DepotId == pricingContext.DepotId
                                                                                   //&& w.FrieghtZoneId == pricingContext.FrieghtZoneId
                                                                                   //&& w.FrieghtRouteId == pricingContext.FrieghtRouteId
                                                                                   //&& w.BiddingWindowId == pricingContext.BiddingWindowId
                                                                                   //&& w.MaterialCost == pricingContext.MaterialCost
                                                                                   //&& w.PackingCost == pricingContext.PackingCost
                                                                                   //&& w.PrimaryFrieght == pricingContext.PrimaryFrieght
                                                                                   //&& w.SecondaryFrieght == pricingContext.SecondaryFrieght
                                                                                   //&& w.DepotCost == pricingContext.DepotCost
                                                                                   //&& w.DetentionCost == pricingContext.DetentionCost
                                                                                   //&& w.HoneycombCost == pricingContext.HoneycombCost
                                                                                   //&& w.Margin == pricingContext.Margin
                                                                                   //&& w.CushionMargin == pricingContext.CushionMargin
                                                                                   //&& w.SchemeCostRecovery == pricingContext.SchemeCostRecovery
                                                                                   //&& w.Discount == pricingContext.Discount
                                                                                   //&& w.Premium == pricingContext.Premium
                                                                                   //&& w.ProcessCost == pricingContext.ProcessCost
                                                                                   //&& w.SumOfIngredientCost == pricingContext.SumOfIngredientCost
                                                                                   //&& w.TpPrice == pricingContext.TpPrice
                                                                                   //&& w.RaMargin == pricingContext.RaMargin
                                                                                   //&& w.BaseRate == pricingContext.BaseRate
                                                                                   //&& w.XMargin == pricingContext.XMargin
                                                                                   //&& w.FinalRate == pricingContext.FinalRate
                                                                                   //&& w.ExPlantPrice == pricingContext.ExPlantPrice
                                                                                   //&& w.ForDepotPrice == pricingContext.ForDepotPrice
                                                                                   //&& w.ForPlantPrice == pricingContext.ForPlantPrice
                                                                                   //&& w.ExDepotPrice == pricingContext.ExDepotPrice
                                                                                   //&& w.ExRakePrice == pricingContext.ExRakePrice
                                                                                   //&& w.ForRakePrice == pricingContext.ForRakePrice
                                                                                   //&& w.ClearanceRate == pricingContext.ClearanceRate
                                                                                   //&& w.CounterBidOffer == pricingContext.CounterBidOffer
                                                                                   //&& w.CounterBidLimit == pricingContext.CounterBidLimit
                                                                                   //&& w.BpCpJumb == pricingContext.BpCpJumb
                                                                                   );
                                                                        }
                                                                        //else if (inputDto.SaudaBookingTypeId == (int)DTO.Enums.SaudaBookingTypes.ReverseAuction)
                                                                        //{
                                                                        //    isValidPrice = PricingData.Any(w => w.BiddingWindowId == inputDto.BiddingWindowId
                                                                        //           && w.SkuId == pricingContext.SkuId
                                                                        //           && w.OilTypeId == pricingContext.OilTypeId
                                                                        //           && w.SaudaBookingTypeId == pricingContext.SaudaBookingTypeId
                                                                        //           && w.OilPackingTypeId == pricingContext.OilPackingTypeId
                                                                        //           && w.StateId == pricingContext.StateId
                                                                        //           && w.CityId == pricingContext.CityId
                                                                        //           && w.TransportModeId == pricingContext.TransportModeId
                                                                        //           && w.PlantId == pricingContext.PlantId
                                                                        //           && w.DepotId == pricingContext.DepotId
                                                                        //           && w.FrieghtZoneId == pricingContext.FrieghtZoneId
                                                                        //           && w.FrieghtRouteId == pricingContext.FrieghtRouteId
                                                                        //           && w.BiddingWindowId == pricingContext.BiddingWindowId
                                                                        //           && w.MaterialCost == pricingContext.MaterialCost
                                                                        //           && w.PackingCost == pricingContext.PackingCost
                                                                        //           && w.PrimaryFrieght == pricingContext.PrimaryFrieght
                                                                        //           && w.SecondaryFrieght == pricingContext.SecondaryFrieght
                                                                        //           && w.DepotCost == pricingContext.DepotCost
                                                                        //           && w.DetentionCost == pricingContext.DetentionCost
                                                                        //           && w.HoneycombCost == pricingContext.HoneycombCost
                                                                        //           && w.Margin == pricingContext.Margin
                                                                        //           && w.CushionMargin == pricingContext.CushionMargin
                                                                        //           && w.SchemeCostRecovery == pricingContext.SchemeCostRecovery
                                                                        //           && w.Discount == pricingContext.Discount
                                                                        //           && w.Premium == pricingContext.Premium
                                                                        //           && w.ProcessCost == pricingContext.ProcessCost
                                                                        //           && w.SumOfIngredientCost == pricingContext.SumOfIngredientCost
                                                                        //           && w.TpPrice == pricingContext.TpPrice
                                                                        //           && w.RaMargin == pricingContext.RaMargin
                                                                        //           && w.BaseRate == pricingContext.BaseRate
                                                                        //           && w.XMargin == pricingContext.XMargin
                                                                        //           && w.FinalRate == pricingContext.FinalRate
                                                                        //           && w.ExPlantPrice == pricingContext.ExPlantPrice
                                                                        //           && w.ForDepotPrice == pricingContext.ForDepotPrice
                                                                        //           && w.ForPlantPrice == pricingContext.ForPlantPrice
                                                                        //           && w.ExDepotPrice == pricingContext.ExDepotPrice
                                                                        //           && w.ExRakePrice == pricingContext.ExRakePrice
                                                                        //           && w.ForRakePrice == pricingContext.ForRakePrice
                                                                        //           && w.ClearanceRate == pricingContext.ClearanceRate
                                                                        //           && w.CounterBidOffer == pricingContext.CounterBidOffer
                                                                        //           && w.CounterBidLimit == pricingContext.CounterBidLimit
                                                                        //           && w.BpCpJumb == pricingContext.BpCpJumb);
                                                                        //}

                                                                        if (isValidPrice)
                                                                        {
                                                                            var finalDataTitleMissingErrorMessage = dataTitleMissingErrorMessage + " Price Already Generated " + "|";
                                                                            errorMessageList.Add(finalDataTitleMissingErrorMessage);
                                                                        }
                                                                        else
                                                                        {
                                                                            //pricingContext.IsPublish = false;
                                                                            //pricingContext.PublishId = priceGenerateDetail.Id;
                                                                            pricings.Add(pricingContext);
                                                                            count++;
                                                                            isAvailable = true;

                                                                            if (pricings.Count == ConsoleSettings.BulkInsertRecordCount)
                                                                            {
                                                                                _context.BulkInsertProxy(pricings);
                                                                                _context.SaveChanges();
                                                                                pricings = new List<Pricing>();
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        //pricingContext.IsPublish = false;
                                                                        //pricingContext.PublishId = priceGenerateDetail.Id;
                                                                        pricings.Add(pricingContext);
                                                                        count++;
                                                                        isAvailable = true;

                                                                        if (pricings.Count == ConsoleSettings.BulkInsertRecordCount)
                                                                        {
                                                                            _context.BulkInsertProxy(pricings);
                                                                            _context.SaveChanges();
                                                                            pricings = new List<Pricing>();
                                                                        }
                                                                    }
                                                                    //}
                                                                }
                                                                else
                                                                {
                                                                    string commonMessage = ErrorMessageFormat(skuLoopErrorMsg, depotLoopErrorMsg, stateLoopErrorMsg, freightRouteLoopErrorMsg);
                                                                    var finalDataMissingErrorMessage = dataTitleMissingErrorMessage + commonMessage + "|";
                                                                    errorMessageList.Add(finalDataMissingErrorMessage);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                string commonMessage = ErrorMessageFormat(skuLoopErrorMsg, depotLoopErrorMsg, stateLoopErrorMsg, freightRouteLoopErrorMsg);
                                                                var finalDataMissingErrorMessage = dataTitleMissingErrorMessage + commonMessage + "|";
                                                                errorMessageList.Add(finalDataMissingErrorMessage.TrimAndReduce());
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        freightRouteLoopErrorMsg = Constants.BindErrorMessage(Constants.DataMissingToLoadCapacity, freightRouteLoopErrorMsg);

                                                        string commonMessage = ErrorMessageFormat(skuLoopErrorMsg, depotLoopErrorMsg, stateLoopErrorMsg, freightRouteLoopErrorMsg);
                                                        var finalDataMissingErrorMessage = dataTitleMissingErrorMessage + commonMessage + "|";
                                                        errorMessageList.Add(finalDataMissingErrorMessage.TrimAndReduce());
                                                    }
                                                }
                                                //}
                                            }
                                        }
                                    }
                                    else
                                    {
                                        valErrorMessage = valErrorMessage + "|";
                                        errorMessageList.Add(valErrorMessage.TrimAndReduce());
                                    }
                                }

                            }
                            else { _logger.Error("TransportModes is empty:" + DateHelper.UtcToIndia(DateTime.UtcNow)); }
                            //}
                            //else { _logger.Error("FreightRouteIds is empty:" + DateHelper.UtcToIndia(DateTime.UtcNow)); }
                            //}
                            //else { _logger.Error("FreightZoneId is empty:" + DateHelper.UtcToIndia(DateTime.UtcNow)); }
                        }
                        else { _logger.Error("DepotIds is empty:" + DateHelper.UtcToIndia(DateTime.UtcNow)); }
                    }
                    else { _logger.Error("SKU is empty : " + DateHelper.UtcToIndia(DateTime.UtcNow)); }

                    priceGenerateDetail.EndDate = DateTime.UtcNow;
                    if (isAvailable)
                    {
                        _context.BulkInsertProxy(pricings);
                        if (errorMessageList != null && errorMessageList.Any())
                        {
                            priceGenerateDetail.StatusId = (int)DTO.Enums.PricePublishStatus.CompletedWithError;
                        }
                        else
                        {
                            priceGenerateDetail.StatusId = (int)DTO.Enums.PricePublishStatus.Completed;
                        }

                        _context.SaveChanges();
                    }
                    else
                    {
                        priceGenerateDetail.StatusId = (int)DTO.Enums.PricePublishStatus.Failed;
                    }
                    if (errorMessageList != null && errorMessageList.Any())
                    {
                        priceGenerateDetail.ErrorMessage = string.Join("", errorMessageList);
                    }
                    _context.SaveChanges();

                    if (isAvailable)
                    {
                        smsContent = Constants.PriceCalculationCompleted.Replace(Constants.Count, count.ToString()).Replace(Constants.StartTime, priceGenerateDetail.StartDate.ToString("hh:mm tt"))
                            .Replace(Constants.EndTime, priceGenerateDetail.EndDate.ToString("hh:mm tt"));
                    }
                    else { smsContent = Constants.PriceCalculationFailed; }

                    if (!string.IsNullOrEmpty(smsContent))
                    {
                        try
                        {
                            AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                            foreach (var mobileNo in mobileNoList)
                            {
                                try
                                {
                                    amazonNotificationService.SendMessage(smsContent, mobileNo);
                                }
                                catch (Exception) { }
                            }
                        }
                        catch (Exception) { }
                    }
                }
                catch (Exception exception)
                {
                    message = $"{ServiceName} Service-Method {_methodName} Exception: {exception}";
                    _logger.Error(message);
                    if (priceGenerateDetail != null && (priceGenerateDetail.StatusId == (int)DTO.Enums.PricePublishStatus.Started || priceGenerateDetail.StatusId == (int)DTO.Enums.PricePublishStatus.Failed))
                    {
                        priceGenerateDetail.StatusId = (int)DTO.Enums.PricePublishStatus.Failed;
                        _context.SaveChanges();
                    }
                    smsContent = Constants.PriceCalculationFailed;
                    try
                    {
                        AmazonNotificationService amazonNotificationService = new AmazonNotificationService();
                        foreach (var mobileNo in mobileNoList)
                        {
                            try
                            {
                                amazonNotificationService.SendMessage(smsContent, mobileNo);
                            }
                            catch (Exception) { }
                        }
                    }
                    catch (Exception) { }
                }

                message = $"{ServiceName} Service-Method {_methodName} Process End Date Time : " + DateHelper.UtcToIndia(DateTime.UtcNow);
                _logger.Info(message);
                message = $"---------------------------------------- {saudaBookingType} Final Price Generate Completed ----------------------------------------";
                _logger.Info(message);
            }
        }

        public decimal DecimalFormat2(decimal? value)
        {
            return Convert.ToDecimal(string.Format("{0:0.00}", (value ?? 0)));
        }

        public string ErrorMessageFormat(string skuLoopErrorMsg, string depotLoopErrorMsg, string stateLoopErrorMsg, string freightRouteLoopErrorMsg)
        {
            string commonMessage = string.Empty;
            if (!string.IsNullOrWhiteSpace(skuLoopErrorMsg))
            {
                commonMessage = Constants.BindErrorMessage(skuLoopErrorMsg, (string.IsNullOrEmpty(commonMessage) ? commonMessage = Constants.DataMissingToCalculate : commonMessage));
            }
            if (!string.IsNullOrWhiteSpace(depotLoopErrorMsg))
            {
                commonMessage = Constants.BindErrorMessage(depotLoopErrorMsg, (string.IsNullOrEmpty(commonMessage) ? commonMessage = Constants.DataMissingToCalculate : commonMessage));
            }
            if (!string.IsNullOrWhiteSpace(stateLoopErrorMsg))
            {
                commonMessage = Constants.BindErrorMessage(stateLoopErrorMsg, (string.IsNullOrEmpty(commonMessage) ? commonMessage = Constants.DataMissingToCalculate : commonMessage));
            }
            if (!string.IsNullOrWhiteSpace(freightRouteLoopErrorMsg))
            {
                commonMessage = Constants.BindErrorMessage(freightRouteLoopErrorMsg, (string.IsNullOrEmpty(commonMessage) ? commonMessage = Constants.DataMissingToCalculate : commonMessage));
            }
            return commonMessage;
        }


        public void VolumeCapacityRemainderNotification(long biddingWindowId, long oilTypeId)
        {
            try
            {
                var filePath = string.Concat(ConsoleSettings.VolumeCapacityRemainderNotification, "\\Adani.Solution.FinalPrice.exe");
                var myProcess = new Process();
                myProcess.StartInfo.FileName = filePath;
                myProcess.StartInfo.Arguments = $"{biddingWindowId} {oilTypeId}";
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
            }
            catch (Exception exception)
            {
                _logger.Error("FinalPriceExeInvoke : " + exception.Message);
            }
        }

        public void VolumeCapacityRemainderNotificationNew(List<VolumeCapacityDto> volumeCapacity)
        {
            try
            {
                foreach (var volume in volumeCapacity)
                {
                    var filePath = string.Concat(ConsoleSettings.VolumeCapacityRemainderNotification, "\\Adani.Solution.FinalPrice.exe");
                    var myProcess = new Process();
                    myProcess.StartInfo.FileName = filePath;
                    myProcess.StartInfo.Arguments = $"{volume.BiddingWindowId} {volume.OilTypeId}";
                    myProcess.StartInfo.CreateNoWindow = true;
                    myProcess.Start();
                }
            }
            catch (Exception exception)
            {
                _logger.Error("FinalPriceExeInvoke : " + exception.Message);
            }
        }
    }
}
